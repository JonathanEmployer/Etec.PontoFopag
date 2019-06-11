using Newtonsoft.Json;
using RegistradorBiometrico.Exceptions;
using RegistradorBiometrico.Model;
using RegistradorBiometrico.Model.Util;
using RegistradorBiometrico.Service.Base;
using RegistradorBiometrico.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RegistradorBiometrico.Service
{
    public class LoginService : ServiceBase
    {
        public async Task<TokenResponseViewModel> LoginAsync(Usuario usuario, CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string str = String.Format("grant_type=password&username={0}&password={1}", usuario.Login, usuario.Senha);
                    HttpContent content = new StringContent(str);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    cancellationToken.ThrowIfCancellationRequested();
                    System.Net.ServicePointManager.Expect100Continue = false;
                    httpClient.BaseAddress = new Uri(VariaveisGlobais.wsHomolocao);
                    HttpResponseMessage response = await httpClient.PostAsync("Token", content);

                    String token = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        LancaExceptionServidorNaoEncontrado(Convert.ToString(token));
                        throw new Exception("Usuário não encontrado", new Exception(Convert.ToString(token)));
                    }
                    else
                    {
                        TokenResponseViewModel objToken = response.Content.ReadAsAsync<TokenResponseViewModel>().Result;
                        return objToken;
                    }
                }
                catch (ServidorNaoEncontradoException sneEx)
                {
                    throw sneEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<bool> VerificaUsuarioCentralCliente(Usuario usuario, String token, CancellationToken cancellationToken)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(usuario);
                    HttpContent content = new StringContent(json);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    cancellationToken.ThrowIfCancellationRequested();

                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    httpClient.BaseAddress = new Uri(VariaveisGlobais.wsHomolocao);

                    // HTTP POST
                    var response = await httpClient.PostAsync(@"api/UsuarioRegistrador", content);
                    var result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Configuracao objConfiguracao;
                        if (VerificaExceptionSemAutorizacao(result, out objConfiguracao) && (objConfiguracao != null))
                        {
                            await VerificaUsuarioCentralCliente(new Usuario() { Login = objConfiguracao.Usuario, Senha = objConfiguracao.Senha }, objConfiguracao.Token, cancellationToken);
                        }

                        LancaExceptionServidorNaoEncontrado(result);
                        throw new Exception("Usuário não encontrado", new Exception(result));
                    }

                    return true;
                }
                catch (NaoAutorizadoException naEx)
                {
                    throw naEx;
                }
                catch (ServidorNaoEncontradoException sneEx)
                {
                    throw sneEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
