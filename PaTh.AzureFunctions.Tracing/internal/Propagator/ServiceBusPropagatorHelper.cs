using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Primitives;
using OpenTelemetry.Context.Propagation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Azure.Functions.Tracing.Internal.Propagator
{
    internal static class ServiceBusPropagatorHelper
    {
        //W3C Trace Context
        const string W3CHeaderTraceParent = "traceparent";
        const string W3CHeaderTraceState = "tracestate";

        //Dynatrace Context
        const string DTHeaderTraceParent = "x-dynatrace";

        //ServiceBus context properties (see https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-end-to-end-tracing?tabs=net-standard-sdk)
        const string ServiceBusTraceParent = "Diagnostic-Id"; //available in Azure.Messaging.ServiceBus as well as Microsoft.Azure.ServiceBus SDK
        const string ServiceBusTraceState = "Correlation-Context"; //only used in (old) Microsoft.Azure.ServiceBus SDK
        
        public static ActivityContext ExtractParentContext(ServiceBusReceivedMessage msg)
        {
            var context = Propagators.DefaultTextMapPropagator.Extract(default, msg, ServiceBusPropagatorHelper.MessagePropertiesGetter);
            return context.ActivityContext;
        }

        public static Func<ServiceBusReceivedMessage, string, IEnumerable<string>> MessagePropertiesGetter => (msg, name) =>
        {

            //map W3C-TraceContext to properties used by ServiceBus SDKs
            if (name == W3CHeaderTraceParent)
                name = ServiceBusTraceParent;
            else if (name == W3CHeaderTraceState)
                name = ServiceBusTraceState;
            else if (name == DTHeaderTraceParent)
                name = DTHeaderTraceParent;
            else
                return null;

            if (msg.ApplicationProperties.TryGetValue(ServiceBusTraceParent, out var obj) && obj is string traceCtx)
                return new StringValues(traceCtx);

            return null;
        };

        public static Action<ServiceBusMessage, string, string> MessagePropertiesSetter => (msg, name, value) =>
        {
            if (name == W3CHeaderTraceParent && !msg.ApplicationProperties.ContainsKey(ServiceBusTraceParent)) //map W3C-TraceContext to properties used by ServiceBus SDKs
                msg.ApplicationProperties[ServiceBusTraceParent] = value;
            else if (name == W3CHeaderTraceState && !msg.ApplicationProperties.ContainsKey(ServiceBusTraceState)) //Enhance with tracestate if not already set
                msg.ApplicationProperties[ServiceBusTraceState] = value;
            else if (name == DTHeaderTraceParent && !msg.ApplicationProperties.ContainsKey(DTHeaderTraceParent)) //Enhance with x-dynatrace if not already set
                msg.ApplicationProperties[DTHeaderTraceParent] = value;
        };

    }
}
