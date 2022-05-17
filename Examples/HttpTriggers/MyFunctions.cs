using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace MyNamespace
{

    public class MyFunctions
    {
        [FunctionName("Ping")]
        public virtual async Task<IActionResult> RunPing(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log, ExecutionContext ctx)
        {
            log.LogInformation("Ping triggered");

            using (var httpClient = new HttpClient())
            {
                var res = await httpClient.GetAsync("http://localhost:7071/api/Test2");
                
                if (!res.IsSuccessStatusCode)
                    return new OkObjectResult("Hello World, something went wrong!") as IActionResult;  
            }

            return new OkObjectResult("Hello World") as IActionResult;
        }


        [FunctionName("Pong")]
        public virtual IActionResult RunPong(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Pong triggered");

            return new OkObjectResult("Hello World, too") as IActionResult;
        }

    }
}
