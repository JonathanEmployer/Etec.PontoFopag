using Modelo;
using Modelo.Registrador;
using Newtonsoft.Json;
using RegistradorBiometrico.Exceptions;
using RegistradorBiometrico.Model;
using RegistradorBiometrico.Model.Util;
using RegistradorBiometrico.Service.Base;
using RegistradorBiometrico.Util;
using RegistradorBiometrico.View;
using RegistradorBiometrico.ViewModel;
using RegistradorPonto.Util;
using RegistradorPonto.View.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegistradorBiometrico.Service
{
    public class RegistradorService : ServiceBase
    {
        private static Configuracao objConfiguracao { get; set; }

        public RegistradorService()
        {
            objConfiguracao = Configuracao.AbrirConfiguracoes();
        }

        public async Task<Boolean> CadastrarBiometria(Modelo.Funcionario objFuncionario)
        {
            BilheteBioEnvio objBiometriaRegistrador = new BilheteBioEnvio(objFuncionario, objConfiguracao.Usuario, objConfiguracao.Token);

            Boolean sucesso = false;
            Byte[] marcacao = new byte[0];
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    String jsonString = JsonConvert.SerializeObject(objBiometriaRegistrador);
                    StringContent strConteudo = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    string posturi = VariaveisGlobais.URL_WS + "api/FuncionarioBiometrias";

                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + objBiometriaRegistrador.Token);
                    client.BaseAddress = new Uri(VariaveisGlobais.URL_WS);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HttpResponseMessage response = await client.PostAsync(posturi, strConteudo);
                    var result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Configuracao objConfiguracaoLocal;
                        if (VerificaExceptionSemAutorizacao(result, out objConfiguracaoLocal) && (objConfiguracaoLocal != null))
                        {
                            objConfiguracao = objConfiguracaoLocal;
                            await CadastrarBiometria(objFuncionario);
                        }

                        LancaExceptionServidorNaoEncontrado(result);
                        throw new Exception("Não foi possível cadastrar a biometria", new Exception(result));
                    }
                }

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

            return sucesso;
        }

        public async Task<Modelo.Funcionario> GetListaBiometriasPorCPF(string cpf)
        {
            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + objConfiguracao.Token);
                    httpClient.BaseAddress = new Uri(VariaveisGlobais.URL_WS);

                    String requisicao = String.Format("api/FuncionarioBiometrias?cpf={0}&login={1}", cpf, objConfiguracao.Usuario);
                    HttpResponseMessage response = await httpClient.GetAsync(requisicao);

                    var result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Configuracao objConfiguracaoLocal;
                        if (VerificaExceptionSemAutorizacao(result, out objConfiguracaoLocal) && (objConfiguracaoLocal != null))
                        {
                            objConfiguracao = objConfiguracaoLocal;
                            await GetListaBiometriasPorCPF(cpf);
                        }

                        LancaExceptionServidorNaoEncontrado(result);
                        LancaExceptionFuncionarioNaoEncontrado(result);
                        throw new Exception("Não foi encontrar o funcionário pelo CPF", new Exception(result));
                    }

                    objFuncionario = JsonConvert.DeserializeObject<Modelo.Funcionario>(result);

                    return objFuncionario;
                }
                catch (NaoAutorizadoException naEx)
                {
                    throw naEx;
                }
                catch (ServidorNaoEncontradoException sneEx)
                {
                    throw sneEx;
                }
                catch (FuncionarioNaoEncontradoException funEx)
                {
                    throw funEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<List<Modelo.Biometria>> GetBiometriasPorUsuarioSistema()
        {
            List<Modelo.Biometria> lstBiometrias = new List<Modelo.Biometria>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + objConfiguracao.Token);
                    httpClient.BaseAddress = new Uri(VariaveisGlobais.URL_WS);

                    String requisicao = String.Format("api/Biometria?login={0}", objConfiguracao.Usuario);
                    HttpResponseMessage response = await httpClient.GetAsync(requisicao);

                    var result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Configuracao objConfiguracaoLocal;
                        if (VerificaExceptionSemAutorizacao(result, out objConfiguracaoLocal) && (objConfiguracaoLocal != null))
                        {
                            objConfiguracao = objConfiguracaoLocal;
                            await GetBiometriasPorUsuarioSistema();
                        }

                        LancaExceptionServidorNaoEncontrado(result);
                        throw new Exception("Não foi encontrar o funcionário pelo biometria", new Exception(result));
                    }

                    lstBiometrias = JsonConvert.DeserializeObject<List<Modelo.Biometria>>(result);

                    return lstBiometrias;
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

        public async Task<RegistraPonto> RegistrarPonto(BilheteBioEnvio bilhete)
        {
            RegistraPonto objRegistraPonto = new RegistraPonto();

            Byte[] marcacao = new byte[0];
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    String jsonString = JsonConvert.SerializeObject(bilhete);

                    StringContent conteudo = new StringContent(jsonString, Encoding.UTF8, "application/json");

                    string posturi = VariaveisGlobais.URL_WS + "api/RegistradorBiometrico";
                  
                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + objConfiguracao.Token);
                    client.BaseAddress = new Uri(VariaveisGlobais.URL_WS);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HttpResponseMessage response = await client.PostAsync(posturi, conteudo);

                    string content = response.Content.ReadAsStringAsync().Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        objRegistraPonto = null;

                        Configuracao objConfiguracaoLocal;
                        if (VerificaExceptionSemAutorizacao(content, out objConfiguracaoLocal) && (objConfiguracaoLocal != null))
                        {
                            objConfiguracao = objConfiguracaoLocal;
                            await RegistrarPonto(bilhete);
                        }

                        LancaExceptionServidorNaoEncontrado(content);
                        throw new Exception("Erro ao registrar o Marcação", new Exception(content));
                    }
                    else
                    {
                        objRegistraPonto = JsonConvert.DeserializeObject<RegistraPonto>(content);
                    }
                }
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

            return objRegistraPonto;
        }


        public async Task<DateTime> AtualizarHora(CancellationToken cancellationToken)
        {
            DateTime dtAtualizada;
            using (var httpClient = new HttpClient())
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + objConfiguracao.Token);
                    httpClient.BaseAddress = new Uri(VariaveisGlobais.URL_WS);
                    HttpResponseMessage response = await httpClient.GetAsync(String.Format("api/RegistradorBiometrico?FusoHorario={0}", objConfiguracao.FusoHorario));

                    var result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Configuracao objConfiguracaoLocal;
                        if (VerificaExceptionSemAutorizacao(result, out objConfiguracaoLocal) && (objConfiguracaoLocal != null))
                        {
                            objConfiguracao = objConfiguracaoLocal;
                            await AtualizarHora(new CancellationToken());
                        }

                        LancaExceptionServidorNaoEncontrado(result);
                    }

                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                    {
                        DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                    };

                    Object dtAtualizadaObject = JsonConvert.DeserializeObject(result, jsonSerializerSettings);
                    dtAtualizada = Convert.ToDateTime(dtAtualizadaObject);

                    return dtAtualizada;
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
