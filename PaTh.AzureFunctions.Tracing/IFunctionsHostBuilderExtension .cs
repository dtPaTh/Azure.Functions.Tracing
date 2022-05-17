using Autofac;
using Autofac.Extensions.DependencyInjection.AzureFunctions;
using Autofac.Extras.DynamicProxy;
using Azure.Functions.Tracing.Internal;
using Dynatrace.OpenTelemetry;
using Dynatrace.OpenTelemetry.Instrumentation.AzureFunctions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Trace;
using System;
using System.Linq;

namespace Azure.Functions.Tracing
{

    public static class IFunctionsHostBuilderExtension
    {
        public static IFunctionsHostBuilder AddFunctionTracing(this IFunctionsHostBuilder builder, Action<TracerProviderBuilder>? configureTracerProvider = null)
        {
            var tracerProvider = Sdk.CreateTracerProviderBuilder()
                    .AddAzureFunctionsInstrumentation() 
                    .AddDynatrace() //Configures to send traces to Dynatrace, automatically reading configuration from environmetn variables.
                    .With(t=>
                    {
                        if (configureTracerProvider != null) 
                            configureTracerProvider(t);
                    })
                    .Build();

            builder.Services.AddSingleton((builder) => tracerProvider);

            builder.UseAutofacServiceProviderFactory((containerBuilder) =>
            {
                containerBuilder
                   .RegisterAssemblyTypes(typeof(IFunctionsHostBuilderExtension).Assembly)
                   .InNamespace("Azure.Functions.Tracing")
                   .AsSelf() 
                   .InstancePerTriggerRequest(); // This will scope nested dependencies to each function execution

                var assemblies = AppDomain.CurrentDomain.GetAssemblies().Matches(FunctionsConfig.Read().Keys).ToList();

                assemblies.ForEach(
                    assembly => containerBuilder
                        .RegisterAssemblyTypes(assembly).PublicOnly()
                        .AsSelf()
                        .EnableClassInterceptors()
                        .InterceptedBy(typeof(FunctionInvocationInterceptor))
                    );

                containerBuilder.Register(c => new FunctionInvocationInterceptor(tracerProvider, null));
            });


            return builder;
        }
    }
}
