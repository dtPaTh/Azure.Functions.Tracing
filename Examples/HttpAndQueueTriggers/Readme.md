# Advanced Tracing Sample

This sample provides an advanced use-case. The Azure Function constains 3 triggers:
1. A HTTP function ("Ping"), making a http call into 
2. a second HTTP triggered function ("Pong"), which puts a message into a ServiceBus queue received
3. via 3rd ServiceBus triggered function ("Booh")

It uses a custom startup class to manually configure the TraceProvider and inject a IHttpClientFactory into the function. 

## Prerequisites
Requires to an Azure ServiceBus instance to send/receive messages. The configured queue name is ```work```

# Configuration

1. Configure dtconfig.json as described in [Dynatrace Help](https://www.dynatrace.com/support/help/setup-and-configuration/setup-on-cloud-platforms/microsoft-azure-services/opentelemetry-integration/opentelemetry-on-azure-functions) to meet your Dynatrace configuration.

2. Configure localhost.settings.json 
```
{
  "IsEncrypted": false,
  "Values": {
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "SBConnection": "<Your-ServiceBus-Connectionstring>"
    /*",PongUrl": "http://localhost:7071/api/Pong" //optional */
  }
}
```
Replace your ```<Your-ServiceBus-Connectionstring>``` with the connection string of your servicebus

