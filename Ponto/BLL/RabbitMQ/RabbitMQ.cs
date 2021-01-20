using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.RabbitMQ
{
    public class RabbitMQ
    {
        private readonly string _hostName;
        private readonly int _port;
        private readonly string _userName;
        private readonly string _password;

        public RabbitMQ()
        {
            _hostName = ConfigurationManager.AppSettings["RabbitMqHostName"];
            _ = int.TryParse(ConfigurationManager.AppSettings["RabbitMqPort"], out _port);
            _userName = ConfigurationManager.AppSettings["RabbitMqUserName"];
            _password = ConfigurationManager.AppSettings["RabbitMqPassword"];
        }

        public RabbitMQ(string hostName, int port, string userName, string password)
        {
            _hostName = hostName;
            _port = port;
            _userName = userName;
            _password = password;
        }

        public void SendMessage(string queueName, string mensagem)
        {
            //Cria a conexão com o RabbitMq
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password,
                Port = _port
            };
            //Cria a conexão
            IConnection connection = factory.CreateConnection();
            //cria a canal de comunicação com a rabbit mq
            IModel channel = connection.CreateModel();
            //Cria a fila caso não exista
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            byte[] body = Encoding.Default.GetBytes(mensagem);
            //Seta a mensagem como persistente
            IBasicProperties properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            //Envia a mensagem para fila
            channel.BasicPublish(exchange: String.Empty, routingKey: queueName, basicProperties: properties, body: body);
        }
    }
}