using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using OpenTelemetry.Context.Propagation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Azure.Functions.Tracing.Internal.Propagator
{
    internal static class HttpPropagatorHelper
    {
        public static ActivityContext ExtractParentContext(HttpRequest request)
        {
            var context = Propagators.DefaultTextMapPropagator.Extract(default, request, HeaderValuesGetter);
            return context.ActivityContext;
        }

        public static IEnumerable<string>? HeaderValuesGetter(HttpRequest request, string name) =>
           request.Headers.TryGetValue(name, out var values) ? values : (IEnumerable<string>?)null;

    }
}
