# Azure Functions Tracing

Distributed tracing enhancements for Azure Functions using OpenTelemetry and Dynatrace.

[Read more about tracing Azure Functions](../readme.md)

## Features
Provides a IFunctionHostBuilder extension to register all necessary components in your functions [startup class](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection). 

It's helpful if you already use a startup class or want to customize the TraceProvider configuration. 

By default the TraceProvider configuration adds automatic function tracing including distributed tracing for HttpTriggers, but does not include any additional instrumentation such as outgoing http or SQLclient calls. 

## Dependencies
The project makes use of [AutoFac](https://autofac.org/) and [Castle Dynamic Proxy](http://www.castleproject.org/projects/dynamicproxy/). 

## Support
This project is a proof-of-concept and not an offical release of Dynatrace. 

## License
Licensed under Apache 2.0 license. See [LICENSE](LICENSE) for details.