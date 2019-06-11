using ComunicadorServico;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteComunicadorServico
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Aperte qualquer tecla para iniciar");
                log.Info("Log Info");
                log.Warn("Log Warning");
                log.Error("Log Erro");
                log.Fatal("Log Fatal");
                log.Debug("Log Debug");
                Console.ReadKey();
                //Negocio.Jobs.MonitorarRepsJob();
            }
            catch (Exception e)
            {
                string detalhe = e.InnerException == null ? " " : " detalhes = " + e.InnerException.Message.ToString();
                Console.WriteLine("Erro na execução, erro: " + e.Message + detalhe);
                Console.ReadKey();
            }
        }
    }
}
