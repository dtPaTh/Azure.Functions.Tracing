using OpenTelemetry.Trace;
using System;

namespace Azure.Functions.Tracing.Internal
{

    internal static class TracerProviderBuilderExtensionInternal
    {
        public static TracerProviderBuilder With<TracerProviderBuilder>(this TracerProviderBuilder builder, Action<TracerProviderBuilder> action)
        {
            action(builder);
            return builder;
        }
    }
}
