using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMq.Configuration;
using RabbitMq.Interface;

namespace RabbitMq
{
    public class RabbitMqController : IRabbitMq
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string queueName;
        private readonly string errorQueueName;
        public RabbitMqController()
        {
            var factory = new ConnectionFactory()
            {
                HostName = RabbitConfig.HostName,
                UserName = RabbitConfig.UserName,
                Password = RabbitConfig.Password,
                Port = RabbitConfig.Port
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            queueName = RabbitConfig.QueueName;
            errorQueueName = RabbitConfig.QueueName;
        }

        public void SendMessage(string queue, string msg)
        {
            QueueDeclare(queue);

            var body = Encoding.UTF8.GetBytes(msg);

            IBasicProperties props = channel.CreateBasicProperties();
            props.Persistent = true;

            channel.BasicPublish("",
                                 queue,
                                 props,
                                 body);
        }

        private void QueueDeclare(string queue)
        {
            channel.QueueDeclare(queue: queue,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
        }

        public void ReceiveMessage(Func<string, bool> Result)
        {
            QueueDeclare(queueName);
            channel.BasicQos(prefetchSize: 0,
                             prefetchCount: 1,
                             global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = "";

                message = Encoding.UTF8.GetString(ea.Body);
                Result(message);
                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };
            channel.BasicConsume(queueName, autoAck: false, consumer: consumer);
        }
    }
}
