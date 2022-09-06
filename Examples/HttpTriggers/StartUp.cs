using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using Azure.Functions.Tracing;
using MyNamespace;
using Microsoft.Extensions.DependencyInjection;
using Autofac;

[assembly: FunctionsStartup(typeof(MyFunctions.Startup))]

namespace MyFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IMyContainerInterface, MySampleContainer>();

            //Create an intermedate seviceprovider, so to resolve the previously added dependencies later. 
            var sp = builder.Services.BuildServiceProvider();

            builder.AddFunctionTracing(t => //configure traceprovider
            {
                t.AddHttpClientInstrumentation();
            }, 
            r=> //Customize autofac interceptor configuration
            {
                //By default only function classes with standard constructors are considered
                //To consider parameterized constructors, enhance config and resolving params from serviceprovider created above. 
                r.WithParameter(new TypedParameter(typeof(IMyContainerInterface),sp.GetService<IMyContainerInterface>()));
            });
            
        }
    }
}