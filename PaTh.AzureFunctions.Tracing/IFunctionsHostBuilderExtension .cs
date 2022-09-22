using Azure.Functions.Tracing.Internal;
using Castle.DynamicProxy;
using Dynatrace.OpenTelemetry;
using Dynatrace.OpenTelemetry.Instrumentation.AzureFunctions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System;

namespace Azure.Functions.Tracing
{

    public static class IFunctionsHostBuilderExtension
    {
        public static IFunctionsHostBuilder AddFunctionTracing(this IFunctionsHostBuilder builder, Action<TracerProviderBuilder>? configureTracerProvider = null)
        {
            var tracerProvider = Sdk.CreateTracerProviderBuilder()
                    .AddAzureFunctionsInstrumentation()
                    .AddDynatrace() //Configures to send traces to Dynatrace, automatically reading configuration from environment variables.
                    .With(t =>
                    {
                        if (configureTracerProvider != null)
                            configureTracerProvider(t);
                    })
                    .Build();

            builder.Services.AddSingleton((builder) => tracerProvider);

            builder.Services.AddSingleton(new ProxyGenerator());
            builder.Services.AddScoped<IInterceptor, FunctionInvocationInterceptor>();

            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IJobActivator), typeof(ProxiedJobActivator)));
            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IJobActivatorEx), typeof(ProxiedJobActivator)));
            
            return builder;
        }
       
    }
}
