using ServicoServico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServicoGerenciadorBloqueioEstacoes
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            try
            {
                ServiceController servico = new ServiceController(ServicoBloqueadorPontofopag.ServiceID);
                switch (servico.Status)
                {
                    case ServiceControllerStatus.StartPending:
                        InitAsService();
                        break;
                    case ServiceControllerStatus.Stopped:
                        InitAsConsole();
                        break;
                    default:
                        InitAsConsole();
                        break;
                }
            }
            catch (Exception)
            {
                InitAsConsole();
            }
        }

        static void InitAsService()
        {
            try
            {
                ServiceBase.Run(new ServicoBloqueadorPontofopag());
            }
            catch (Exception ex)
            {
                throw; //Se falhou como serviço, vai iniciar como console.
            }
        }

        static void InitAsConsole()
        {
            try
            {
                new ServicoBloqueadorPontofopag().Iniciar(true);
            }
            catch (Exception ex)
            {
            }
        }

    }


}
