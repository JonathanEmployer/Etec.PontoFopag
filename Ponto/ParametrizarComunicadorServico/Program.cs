using System;
using System.Windows.Forms;

namespace ParametrizarComunicadorServico
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormLogin fl = new FormLogin();
            Application.Run(fl);

            //ModeloPonto.Proxy.PxyConfigComunicadorServico conf = BLL.Configuracao.GetConfiguracao();
            if (!fl.fechou)
            {
                RodarSistema(false);
            }
        }

        private static void RodarSistema(bool minimizar)
        {
            Application.Run(new FormConfiguracoes(minimizar));
        }
    }
}
