using Microsoft.Win32;
using RegistradorBiometrico.Model;
using RegistradorBiometrico.Model.Util;
using RegistradorBiometrico.Util;
using RegistradorBiometrico.View;
using RegistradorBiometrico.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistradorBiometrico
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value;

            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("O Registrador já está em execução.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                VariaveisGlobais.SetaEnderecoWS(EnumeraveisUtil.TipoExecucao.Homologacao);

                Configuracao objConfiguracao = Configuracao.AbrirConfiguracoes();
                Boolean bMacLocalValido = LoginViewModel.VerificaMacAddress(objConfiguracao.MacAdress);

                objConfiguracao = VerificaMacAdress(objConfiguracao, bMacLocalValido);
                PodeExecutarSistema(objConfiguracao, bMacLocalValido);
            }

        }

        private static void PodeExecutarSistema(Configuracao objConfiguracao, Boolean bMacLocalValido)
        {
            try
            {
                if ((bMacLocalValido) && (Configuracao.ConfiguracaoValida(objConfiguracao)))
                {
                    DateTime validadeToken = Convert.ToDateTime(objConfiguracao.ValidadeLogin);
                    if (validadeToken < DateTime.Now)
                    {
                        bool loginEfetuado = LoginViewModel.EfetuarLoginAutomatico(objConfiguracao.Usuario, objConfiguracao.Senha, objConfiguracao.MacAdress).Result;
                        if (!loginEfetuado)
                        {
                            CharmarTelaDeLogin();
                            return;
                        }
                    }
                    RodarSistema(true);
                }
                else
                {
                    CharmarTelaDeLogin();
                }
            }
            catch
            {
                CharmarTelaDeLogin();
            }
        }

        private static void CharmarTelaDeLogin()
        {
            string macLocal = LoginViewModel.BuscaMacLocal();

            FormLogin formLogin = new FormLogin(macLocal);
            formLogin.ShowDialog();

            if (formLogin.Loggado)
                RodarSistema(true);
        }

        private static void RodarSistema(bool minimizar)
        {
            AddAtalhoNaInicializacaoWindows();

            Application.Run(new FormRegistraPonto(minimizar));
        }


        private static void AddAtalhoNaInicializacaoWindows()
        {
            Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8"));
            dynamic shell = Activator.CreateInstance(t);
            try
            {
                var lnk = shell.CreateShortcut(String.Format("{0}\\{1}.lnk", Environment.GetFolderPath(Environment.SpecialFolder.Startup), AppDomain.CurrentDomain.FriendlyName));
                try
                {
                    lnk.TargetPath = Application.ExecutablePath;
                    lnk.IconLocation = "shell32.dll, 1";
                    lnk.Save();
                }
                finally
                {
                    Marshal.FinalReleaseComObject(lnk);
                }
            }
            finally
            {
                Marshal.FinalReleaseComObject(shell);
            }
        }

        private static Configuracao VerificaMacAdress(Configuracao objConfiguracao, Boolean bMacAdressValido)
        {
            if (!bMacAdressValido)
            {
                XmlUtil<Configuracao>.DeletarArquivo(VariaveisGlobais.caminhoArquivoConfiguracao);
                objConfiguracao = new Configuracao(); 
            }

            return objConfiguracao;
        }

    }
}
