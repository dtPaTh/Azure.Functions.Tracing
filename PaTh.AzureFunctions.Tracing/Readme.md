# Azure Functions Tracing
Distributed tracing enhancements for Azure Functions using OpenTelemetry and Dynatrace. This package makes use of [Castle Dynamic Proxy](http://www.castleproject.org/projects/dynamicproxy/) to intercept function invocations, adding distributed tracing via [Dynatrace OpenTeleetry extension](https://www.dynatrace.com/support/help/setup-and-configuration/setup-on-cloud-platforms/microsoft-azure-services/opentelemetry-integration/opentelemetry-on-azure-functions).

[Read more about tracing Azure Functions](../readme.md)

## Features
The package provides a IFunctionHostBuilder extension to register all necessary components in your functions [startup class](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection). 

It allows you to add tracing if you already use a startup class or want to customize the TraceProvider configuration. 

By default the instrumentation adds automatic function tracing for any function trigger type and enables automatic distributed tracing for following triggers: 
* **HttpTrigger**  using a *HttpRequest* binding
* **ServiceBusTrigger** using a *ServiceBusReceivedMessage* binding

You can enable .NET Framworks additional instrumentation such as [outgoing http or SQLclient calls](https://github.com/open-telemetry/opentelemetry-dotnet). 

Since October 2021, .NET Azure SDK comes with experimental OpenTelemetry support which gives additional trace details. While you can capture these spans span as [described here](https://devblogs.microsoft.com/azure-sdk/introducing-experimental-opentelemetry-support-in-the-azure-sdk-for-net/), the Azure.Function.Tracing library adds selected and validated instrumentation filters via simple TraceProviderBuilder extension methods:
* **AddServiceBusInstrumentation** adds spans for Azure.Messaging.ServiceBus Client

## ChangeLog
* v1.0.0 - Removed dependency on [AutoFac](https://autofac.org/). Removed parameter *RegisterBuilder* from configuration method *AddFunctionTracing* as it is no longer necessary to pass the services when using dependency injection in Azure Functions.
* v1.1.0 - Adding support for ServiceBus message tracing

## Contribute
This is an open source project, and we gladly accept new contributions and contributors.  

### Enhancing the instrumentation to support other triggers

Enhacing the instrumentation is easy, as you only have to advance the interceptor logic in class *FunctionInvocationInterceptor*.
```
...
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
```

## Support
This project is not an offical release of Dynatrace. If you have questions or any problems, open a github issue.  

## License
Licensed under Apache 2.0 license. See [LICENSE](LICENSE) for details.