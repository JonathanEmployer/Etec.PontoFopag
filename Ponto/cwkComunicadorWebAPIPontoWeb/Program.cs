using cwkComunicadorWebAPIPontoWeb.BLL;
using cwkComunicadorWebAPIPontoWeb.Utils;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace cwkComunicadorWebAPIPontoWeb
{
    static class Program
    {
        public static IScheduler scheduler;
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main()
        {
            try
            {
                Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter { Level = Common.Logging.LogLevel.Info };
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                CancellationToken ct = new CancellationToken();
                LoginBLL bll = new LoginBLL();
                XDocument xD = bll.GetXmlConfAsync(ct, null).Result;
                var conf = bll.GetXmlRegisterDataAsync().Result;
                string dirApp = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (String.IsNullOrEmpty(CwkUtils.FileLogStringUtil()) || 
                    String.IsNullOrEmpty(CwkUtils.FileLogStringUtil("AFDsImportados")) ||
                    String.IsNullOrEmpty(CwkUtils.FileLogStringUtil("AFDsExportados")))
                {
                    MessageBox.Show("Não foi possível criar diretórios auxiliares do sistema. Verifique as permissões de acesso na pasta e tente novamente.");
                    Application.Exit();
                }

                if (String.IsNullOrEmpty(conf.AccessToken) || conf.ExpiresAt == new DateTime() || conf.ExpiresAt < DateTime.Now)
                {
                    FormLogin fl = new FormLogin();
                    fl.ShowDialog();
                    xD = bll.GetXmlConfAsync(ct, null).Result;
                    if (String.IsNullOrEmpty(xD.Element("ConfiguracaoPontofopag").Element("tk").Value) || fl.fechou)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        RodarSistema(false);
                    }
                }
                else
                {
                    ViewModels.VariaveisGlobais.SetEndWS("");
                    RodarSistema(false);
                }
            }
            catch (Exception e)
            {
                if (e is AggregateException)
                {
                    e = ((AggregateException)e).Flatten();
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in ((AggregateException)e).InnerExceptions)
                    {
                        sb.AppendLine(item.Message);
                    }
                    MessageBox.Show("Ocorreu um erro ao executar a aplicação: \r\n" + sb.ToString());
                }
                else
                {
                    MessageBox.Show("Ocorreu um erro ao executar a aplicação: \r\n" + e.Message);
                }
            }
            finally
            {
                if (scheduler.IsStarted)
                {
                    scheduler.Shutdown();
                }
            }

        }
        private static void RodarSistema(bool minimizar)
        {
            Application.Run(new MenuPrincipal(minimizar));
        }
    }
}
