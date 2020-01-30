using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMq.Interface;

namespace RabbitMq.Configuration
{
    public static class InjectionRabbitMq
    {
        public static void RabbitMqService(this IServiceCollection Services, string rabbitJson)
        {
            var rabbitConfig = JsonConvert.DeserializeObject<RabbitConfigAux>(rabbitJson);

            RabbitConfig.HostName = rabbitConfig.HostName;
            RabbitConfig.Port = rabbitConfig.Port;
            RabbitConfig.UserName = rabbitConfig.UserName;
            RabbitConfig.Password = rabbitConfig.Password;
            RabbitConfig.QueueName = rabbitConfig.QueueName;
            RabbitConfig.ErrorQueueName = rabbitConfig.ErrorQueueName;

            Services.AddTransient(typeof(IRabbitMq), typeof(RabbitMqController));
        }
    }
}
