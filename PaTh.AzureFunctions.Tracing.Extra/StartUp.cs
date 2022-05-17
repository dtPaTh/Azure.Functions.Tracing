using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Azure.Functions.Tracing;
using OpenTelemetry.Trace;

[assembly: FunctionsStartup(typeof(Azure.Functions.Trace.Extra.Startup))]

namespace Azure.Functions.Trace.Extra
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.AddFunctionTracing(t =>
            {
                t.AddHttpClientInstrumentation();
                t.AddSqlClientInstrumentation();
            });
        }

    }
}