using ServicoServico;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServicoGerenciadorBloqueioEstacoes
{
    [RunInstaller(true)]
    public class Instalador : System.Configuration.Install.Installer
    {
    
        public Instalador()
        {
            ServiceProcessInstaller Process = new ServiceProcessInstaller();
            Process.Account = ServiceAccount.LocalSystem;

            ServiceInstaller Exportador = new ServiceInstaller();
            Exportador.StartType = ServiceStartMode.Automatic;
            Exportador.ServiceName = ServicoBloqueadorPontofopag.ServiceID;
            Exportador.DisplayName = ServicoBloqueadorPontofopag.ServName;
            Exportador.Description = ServicoBloqueadorPontofopag.ServDescription;

            Installers.Add(Exportador);
            Installers.Add(Process);
        }

        private void InitializeComponent()
        {

        }
    }
}
