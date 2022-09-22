using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Azure.Functions.Tracing;

[assembly: FunctionsStartup(typeof(MyNamespace.Startup))]

namespace MyNamespace
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //Register any required services 
            builder.Services.AddScoped<IMyContainerInterface, MySampleLoggingContainer>();

            //Add automatic function tracing
            builder.AddFunctionTracing(t => 
            {
                t.AddHttpClientInstrumentation();
            });



        }
    }


}