using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using Azure.Functions.Tracing;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(MyFunctions.Startup))]

namespace MyFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.AddFunctionTracing(t =>
            {
                t.AddHttpClientInstrumentation();
                t.AddServiceBusInstrumentation();
                
            });
        }
    }
}