using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using OpenTelemetry.Trace;
using Azure.Functions.Tracing;

[assembly: FunctionsStartup(typeof(MyFunctions.Startup))]

namespace MyFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.AddFunctionTracing(t =>
            {
                t.AddHttpClientInstrumentation();
            });
        }
    }
}