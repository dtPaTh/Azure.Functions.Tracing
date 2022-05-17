using System;

namespace Azure.Functions.Tracing.Internal
{

    internal static class TracerProviderBuilderExtension
    {
        public static TracerProviderBuilder With<TracerProviderBuilder>(this TracerProviderBuilder obj, Action<TracerProviderBuilder> action)
        {
            action(obj);
            return obj;
        }

    }
}
