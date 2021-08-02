using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using System;
using System.Configuration;
using System.Text;

namespace PontoWeb.Utils
{
    public class RabbitMqController : IDisposable
    {
        #region Atributos
        private const string Queue_Employee = "Pontofopag_Employee";
        private const string Queue_Employer = "Pontofopag_Employer";
        private const string Queue_RegistradorEmMassa = "pontofopag_registradormassa_{0}";

        private readonly IConnection connection;
        private readonly IModel channel;
        #endregion

        #region EPaysRabbitMQ
        public RabbitMqController()
        {
            var factory = new ConnectionFactory()
            {
                HostName = ConfigurationManager.AppSettings["RabbitMQHost"],
                UserName = ConfigurationManager.AppSettings["RabbitMQUserName"],
                Password = ConfigurationManager.AppSettings["RabbitMQPassword"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["RabbitMQPort"])
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (connection.IsOpen)
                connection.Close();
            connection.Dispose();
            channel.Dispose();
        }
        #endregion

        #region QueueDeclare
        private void QueueDeclare(string Queue)
        {
            channel.QueueDeclare(
            queue: Queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        }
        #endregion

        #region SendMessage
        public void SendMessage(string Queue, string msg)
        {
            try
            {
                QueueDeclare(Queue);


                var payload = Encoding.UTF8.GetBytes(msg);
                channel.BasicPublish("",
                                     Queue.ToString(),
                                     new BasicProperties() { Persistent = true },
                                     payload);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SendEmployerIntegration
        public void SendEmployerIntegration(MessageIntegrationDto MessageIntegration)
        {
            SendMessage(Queue_Employer, JsonConvert.SerializeObject(MessageIntegration));
        }
        #endregion

        #region SendEmployeeIntegration
        public void SendEmployeeIntegration(MessageIntegrationDto MessageIntegration)
        {
            SendMessage(Queue_Employee, JsonConvert.SerializeObject(MessageIntegration));
        }
        #endregion

        #region SendRegistradorEmMassa
        public void SendRegistradorEmMassa(string numSerie, int idEnvioDadosRep)
        {
            SendMessage(string.Format(Queue_RegistradorEmMassa,numSerie), JsonConvert.SerializeObject(idEnvioDadosRep));
        }
        #endregion
    }
}