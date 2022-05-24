using Castle.DynamicProxy;
using Dynatrace.OpenTelemetry;
using Dynatrace.OpenTelemetry.Instrumentation.AzureFunctions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.Functions.Tracing.Internal
{

    internal class FunctionInvocationInterceptor : IInterceptor
    {
        readonly TracerProvider tracerProvider;
        public FunctionInvocationInterceptor(TracerProvider tracerProvider, Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory)
        {
            this.tracerProvider = tracerProvider ?? throw new ArgumentNullException(nameof(tracerProvider));
            DynatraceSetup.InitializeLogging(loggerFactory);
        }

        public static ActivityContext ExtractParentContext(HttpRequest request)
        {
            var context = Propagators.DefaultTextMapPropagator.Extract(default, request, HeaderValuesGetter);
            return context.ActivityContext;
        }

        public static IEnumerable<string>? HeaderValuesGetter(HttpRequest request, string name) =>
           request.Headers.TryGetValue(name, out var values) ? values : (IEnumerable<string>?)null;

        public void Intercept(IInvocation invocation)
        {
            var attr = invocation.Method.GetCustomAttributes(typeof(FunctionNameAttribute), true);

            if (attr != null)
            {
                //check for incoming htttprequest
                var httpReq = invocation.Arguments.Where(a => a.GetType().IsSubclassOf(typeof(HttpRequest))).SingleOrDefault() as HttpRequest;
                ActivityContext parentContext = httpReq != null ? ExtractParentContext(httpReq) : default;

                AzureFunctionsCoreInstrumentation.TraceAsync(tracerProvider, ((FunctionNameAttribute)attr[0]).Name, () =>
                {
                    invocation.Proceed();

                    return Task.FromResult(invocation.ReturnValue);
                }, parentContext);
            }

        }
    }
}
