using BLL_N.Hubs;
using Modelo.Proxy;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendMessagePontoWeb
{
    public partial class RabbitMqConsumer : ServiceBase
    {
        protected override void OnStart(string[] args)
        {
            Receive();
        }

        private static void Receive()
        {
            try
            {
                string hostName = ConfigurationManager.AppSettings["HostName"];
                string port = ConfigurationManager.AppSettings["Port"];
                string userName = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                string queueName = ConfigurationManager.AppSettings["QueueName"];

                if (int.TryParse(port, out int portInt))
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = hostName,
                        Port = portInt,
                        UserName = userName,
                        Password = password
                    };
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: queueName,
                                             durable: true,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine(" [x] Received {0}", message);


                            PxyJobReturn job = JsonConvert.DeserializeObject<PxyJobReturn>(message);
                            NotificationHub.ReportarJobProgresso(job);
                            if (job.Progress == 100)
                            {
                                NotificationHub.ReportarJobCompleto(job);
                            }
                        };
                        channel.BasicConsume(queue: "SendMessagePontoWeb",
                                             autoAck: true,
                                             consumer: consumer);

                        Console.WriteLine(" Press [enter] to exit.");
                        Console.ReadLine();
                    } 
                }
            }
            catch (Exception)
            {
                Thread.Sleep(10000);
                Receive();
            }
        }
    }
}
