﻿using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMq.Interface;

namespace RabbitMq
{
    public class RabbitMqController : IRabbitMq
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string _queueName;
        private readonly string _errorQueueName;
        public RabbitMqController(string hostName, string userName, string password, int port, string queueName, string errorQueueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = port
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            _queueName = queueName;
            _errorQueueName = errorQueueName;
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
            QueueDeclare(_queueName);
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
            channel.BasicConsume(_queueName, autoAck: false, consumer: consumer);
        }
    }
}
