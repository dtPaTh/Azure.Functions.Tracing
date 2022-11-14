using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Azure.Messaging.ServiceBus;
using System;

namespace MyNamespace
{

    public class MyFunctions
    {
        private const string SBConnectionStr = "SBConnection";
        private const string QueueName = "work";

        HttpClient _httpClient;
        public MyFunctions(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient();
        }

        [FunctionName("Ping")]
        public virtual async Task<IActionResult> RunHttpPing(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log, ExecutionContext ctx)
        {
            log.LogInformation("Ping triggered");

            var res = await _httpClient.GetAsync(Environment.GetEnvironmentVariable("PongUrl")??"http://localhost:7071/api/Pong");
                
            if (!res.IsSuccessStatusCode)
                return new OkObjectResult("Hello World, something went wrong!") as IActionResult;  

            return new OkObjectResult("Hello World") as IActionResult;
        }


        [FunctionName("Pong")]
        public virtual async Task<IActionResult> RunHttpPong(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Pong triggered");

            var client = new ServiceBusClient(Environment.GetEnvironmentVariable(SBConnectionStr));

            var sender = client.CreateSender(QueueName);

            var msg = $"Booh {DateTime.Now}";
            await sender.SendMessageAsync(new ServiceBusMessage(msg));

            return new OkObjectResult($"Hello World, too. Sent message: {msg}") as IActionResult;
        }


        [FunctionName("Booh")]
        public virtual void RunMessage([ServiceBusTrigger(QueueName, Connection = SBConnectionStr)] ServiceBusReceivedMessage msg, ILogger log)
        {
            log.LogInformation($"Welcome message: {msg.Body}");
        }

    }
}
