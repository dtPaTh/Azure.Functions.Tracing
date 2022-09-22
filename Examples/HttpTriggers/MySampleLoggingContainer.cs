using Microsoft.Extensions.Logging;

namespace MyNamespace
{
    internal class MySampleLoggingContainer: IMyContainerInterface
    {
        ILogger log;
        public MySampleLoggingContainer(ILoggerFactory f)
        {

            log = f.CreateLogger<MySampleLoggingContainer>();
            
        }

        public string Get()
        {
            log.LogError("Hello World");
            return "Hello World!";
        }
    }
}