using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace MyNamespace
{

    public class MyFunctions
    {
        IMyContainerInterface data;

        public MyFunctions(IMyContainerInterface _data)
        {
            data = _data;
        }

        [FunctionName("Ping")]
        public virtual async Task<IActionResult> RunPing(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log, ExecutionContext ctx)
        {
            log.LogInformation("Ping triggered");

            using (var httpClient = new HttpClient())
            {
                var res = await httpClient.GetAsync("http://localhost:7071/api/Pong");
                
                if (!res.IsSuccessStatusCode)
                    return new OkObjectResult(data.Get()+", something went wrong!") as IActionResult;  
            }
            

            return new OkObjectResult(data?.Get()) as IActionResult;
        }


        [FunctionName("Pong")]
        public virtual IActionResult RunPong(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Pong triggered");

            return new OkObjectResult(data?.Get()+", too") as IActionResult;
        }

    }
}
