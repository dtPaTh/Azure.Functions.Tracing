namespace MyNamespace
{
    internal class MySampleContainer: IMyContainerInterface
    {
        public MySampleContainer()
        {
        }

        public string Get()
        {
            return "Hello World!";
        }
    }
}