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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override void OnStart(string[] args)
        {
            log.Info("Iniciando método de recepção");
            Receive();
        }

        protected override void OnStop()
        {
            _channel.Close();
            _connection.Close();
            _channel.Dispose();
            _connection.Dispose();
            base.OnStop();
        }
        private static ConnectionFactory _factory;
        private static IConnection _connection;
        private static IModel _channel;
        private static EventingBasicConsumer _consumer;

        private void Receive()
        {
            try
            {
                log.Info("Pegando parametros rabbit");
                string hostName = ConfigurationManager.AppSettings["HostName"];
                string port = ConfigurationManager.AppSettings["Port"];
                string userName = ConfigurationManager.AppSettings["Username"];
                string password = ConfigurationManager.AppSettings["Password"];
                string queueName = ConfigurationManager.AppSettings["QueueName"];
                log.Info("Parametros recuperados");
                if (int.TryParse(port, out int portInt))
                {
                    log.Info("Criando factory do rabbit");

                    log.Info($"Conectando rabbit, host = {hostName}, porta = {port}, queue = {queueName}");

                    _factory = new ConnectionFactory()
                    {
                        HostName = hostName,
                        UserName = userName,
                        Password = password,
                        Port = portInt
                    };
                    _connection = _factory.CreateConnection();
                    _channel = _connection.CreateModel();

                    _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    _consumer = new EventingBasicConsumer(_channel);

                    _consumer.Received += (s, ev) =>
                    {
                        var body = ev.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [x] Received {0}", message);
                        log.Debug($"Mensagem recebida = {message}");


                        PxyJobReturn job = JsonConvert.DeserializeObject<PxyJobReturn>(message);
                        NotificationHub.ReportarJobProgresso(job);
                        if (job.Progress == 100)
                        {
                            NotificationHub.ReportarJobCompleto(job);
                        }
                    };

                    _channel.BasicConsume(queue: queueName, autoAck: true, consumer: _consumer);
                }
            }
            catch (Exception ex)
            {
                log.Error("XXXXXX Erro: " + ex.Message);
                Thread.Sleep(10000);
                Receive();
            }
        }
    }
}
