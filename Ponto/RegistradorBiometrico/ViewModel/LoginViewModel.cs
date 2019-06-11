using RegistradorBiometrico.Model;
using RegistradorBiometrico.Model.Util;
using RegistradorBiometrico.Service;
using RegistradorBiometrico.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegistradorBiometrico.ViewModel
{
    public class LoginViewModel
    {
        LoginService loginService = new LoginService();

        public async Task<Boolean> EfetuarLogin(Usuario usuario, string mac_adress, CancellationToken cancellationToken)
        {
            try
            {
                TokenResponseViewModel tokenResponse = new TokenResponseViewModel();
                tokenResponse = await loginService.LoginAsync(usuario, cancellationToken);

                Configuracao objConfiguracao = new Configuracao(usuario.Login, usuario.Senha, tokenResponse.AccessToken,
                    tokenResponse.ExpiresAt.ToString(), mac_adress);


                Configuracao.SalvarConfiguracoes(objConfiguracao);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> EfetuarLoginConfiguracao(Usuario usuario, CancellationToken cancellationToken)
        {
            try
            {
                Configuracao objConfiguracao = Configuracao.AbrirConfiguracoes();
                return await loginService.VerificaUsuarioCentralCliente(usuario, objConfiguracao.Token, cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<bool> EfetuarLoginAutomatico(string usuario, string senha, string mac)
        {
            try
            {
                LoginViewModel login = new LoginViewModel();
                return await login.EfetuarLogin(new Usuario() { Login = usuario, Senha = senha }, mac, new CancellationToken());
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Boolean VerificaMacAddress(String macAdressSalvo)
        {
            List<NetworkInterface> macsAdressComputadorAtual = NetworkInterface.GetAllNetworkInterfaces().ToList();
            try
            {
                String macLocalAtivo = BuscaMacLocalAtivo(macAdressSalvo);
                if (!String.IsNullOrEmpty(macLocalAtivo))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                return false;
            }
        }

        private static String BuscaMacLocalAtivo(String macAdressSalvo)
        {
            String sMacAddress = String.Empty;

            try
            {
                NetworkInterface macAdressComputadorAtual = NetworkInterface.GetAllNetworkInterfaces().
                    Where(s => s.GetPhysicalAddress().ToString() == macAdressSalvo).FirstOrDefault();

                sMacAddress = macAdressComputadorAtual.GetPhysicalAddress().ToString();
            }
            catch
            {
                sMacAddress = String.Empty;
            }

            return sMacAddress;
        }

        public static String BuscaMacLocal()
        {
            String sMacAddress = String.Empty;

            try
            {
                List<NetworkInterface> nics = NetworkInterface.GetAllNetworkInterfaces().ToList();

                foreach (NetworkInterface adapter in nics)// only return MAC Address from first card
                {
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                    break;
                } 
            }
            catch
            {
                sMacAddress = String.Empty;
            }

            return sMacAddress;
        }
    }
}
