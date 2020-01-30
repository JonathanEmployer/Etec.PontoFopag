namespace RabbitMq.Configuration
{
    public static class RabbitConfig
    {
        public static string HostName { get; set; }
        public static int Port { get; set; }
        public static string UserName { get; set; }
        public static string Password { get; set; }
        public static string QueueName { get; set; }
        public static string ErrorQueueName { get; set; }
    }
    public class RabbitConfigAux
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
        public string ErrorQueueName { get; set; }
    }
}
