using OpenTelemetry.Trace;
using System;

namespace Azure.Functions.Tracing
{

    public static class TracerProviderBuilderExtension
    {

        public static TracerProviderBuilder AddServiceBusInstrumentation(this TracerProviderBuilder builder)
        {
            //Enable Azure SDK instrumentation
            AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);
            
            //Only enable spans, participating in a single trace (e.g. receiver/processor creates new trace id's)
            builder.AddSource("Azure.Messaging.ServiceBus");
            builder.AddSource("Azure.Messaging.ServiceBus.ServiceBusSender");

            return builder;
        }
    }
}
