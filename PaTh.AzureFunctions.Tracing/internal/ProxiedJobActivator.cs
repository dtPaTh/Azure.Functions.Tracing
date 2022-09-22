using Castle.DynamicProxy;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Azure.Functions.Tracing.Internal
{
    internal class ProxiedJobActivator : IJobActivatorEx
    {
        private readonly IServiceProvider _serviceProvider;

        public ProxiedJobActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public T CreateInstance<T>()
        {
            return CreateInstance<T>(_serviceProvider);
        }

        public T CreateInstance<T>(IFunctionInstanceEx functionInstance)
        {
            return CreateInstance<T>(functionInstance.InstanceServices);
        }

        private T CreateInstance<T>(IServiceProvider serviceProvider)
        {
            var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
            var interceptors = serviceProvider.GetServices<IInterceptor>().ToArray();

            var ctorInfo = typeof(T).GetConstructors();
            var ctorParams = ctorInfo[0].GetParameters();
            if (ctorParams.Length > 0)
            {
                var ctor = new object[ctorParams.Length];

                for (var i = 0; i < ctorParams.Length; i++)
                {
                    ctor[i] = serviceProvider.GetService(ctorParams[i].ParameterType);
                }
                return (T)proxyGenerator.CreateClassProxy(typeof(T), ctor, interceptors);
            }
            else
                return (T)proxyGenerator.CreateClassProxy(typeof(T), interceptors);
        }
    }
}
