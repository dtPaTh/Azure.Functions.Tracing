# Azure Functions Tracing Extra

An Azure Function startup class leveraging PaTh.AzureFunctions.Tracing package to enable distributed tracing with OpenTelemetry and Dynatrace.

[Read more about tracing Azure Functions](../readme.md)

Automatically enables distributed tracing for:
* HttClient
* SQLClient
* Azure.Messaging.ServiceBus

## ChangeLog
* v1.1.0 - Adding support for automatic ServiceBus message tracing

## Contribute
This is an open source project, and we gladly accept new contributions and contributors.  

## Support
This project is not an offical release of Dynatrace. If you have questions or any problems, open a github issue.  

## License
Licensed under Apache 2.0 license. See [LICENSE](LICENSE) for details.

