using cwkPontoMT.Integracao;
using Modelo;
using ModeloAux;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ComunicacaoApi : BLLBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string token = "";
        public ComunicacaoApi(string token)
        {
            this.token = token;
        }

        public async Task<List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora>> GetConfiguracoesDataHoraAsync(List<int> idsRep)
        {
            List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora> dadosImportacao = new List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string requisicao = "api/ConfiguracoesDataHora?";
                    string parametros = "";
                    for (int i = 0; i < idsRep.Count; i++)
                    {
                        parametros += "idRelogio[" + i.ToString() + "]=" + idsRep[i] + "&";
                    }
                    parametros = parametros.Substring(0, parametros.Length - 1);
                    requisicao = requisicao + parametros;

                    httpClient.BaseAddress = new Uri(ModeloAux.VariaveisGlobais.UrlWebAPi);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                    // HTTP GET
                    HttpResponseMessage response = await httpClient.GetAsync(requisicao);
                    if (response.IsSuccessStatusCode)
                    {
                        dadosImportacao = await response.Content.ReadAsAsync<List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora>>();
                        //string ret = await response.Content.ReadAsStringAsync();
                        //dadosImportacao = JsonConvert.DeserializeObject<List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora>>(ret);
                    }
                    else
                    {
                        TratarErroRetornoApi(response, "Solicitar dados de envio de data/hora");
                    }
                    return dadosImportacao;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public async Task<Dictionary<int, string>> DeletaDadosConfigDataHora(List<int> idsDadosConfigDataHora)
        {
            using (var httpClient = new HttpClient())
            {
                string idsimp = "";
                for (int i = 0; i < idsDadosConfigDataHora.Count; i++)
                {
                    idsimp += idsDadosConfigDataHora[i] + ", ";
                }
                try
                {
                    string requisicao = "api/ConfiguracoesDataHora?";
                    string parametros = "";
                    for (int i = 0; i < idsDadosConfigDataHora.Count; i++)
                    {
                        parametros += "idsConfigs[" + i.ToString() + "]=" + idsDadosConfigDataHora[i] + "&";
                    }
                    parametros = parametros.Substring(0, parametros.Length - 1);
                    requisicao = requisicao + parametros;
                    //ReportarProgresso(new ReportaErro() { Mensagem = "Enviando Configurações de Data/Hora a serem excluídas!", TipoMsg = TipoMensagem.Info }, progress);
                    httpClient.BaseAddress = new Uri(VariaveisGlobais.UrlWebAPi);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    HttpResponseMessage response = httpClient.DeleteAsync(requisicao).Result;
                    //ReportarProgresso(new ReportaErro() { Mensagem = "Recebido retorno das configurações de data e hora a serem excluídos!", TipoMsg = TipoMensagem.Info }, progress);
                    Dictionary<int, string> retorno = new Dictionary<int, string>();

                    if (response.IsSuccessStatusCode)
                    {
                        retorno = await response.Content.ReadAsAsync<Dictionary<int, string>>();
                    }
                    else
                    {
                        TratarErroRetornoApi(response, "Excluir dados de envio de data/hora");
                    }
                    return retorno;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public async Task<List<ImportacaoDadosRep>> GetEmpFuncExpRepAsync(List<int> idsRep)
        {
            List<ImportacaoDadosRep> dadosImportacao = new List<ImportacaoDadosRep>();
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string requisicao = "api/ImportarDadosRep?";
                    string parametros = "";
                    for (int i = 0; i < idsRep.Count; i++)
                    {
                        parametros += "idRelogio[" + i.ToString() + "]=" + idsRep[i] + "&";
                    }
                    parametros = parametros.Substring(0, parametros.Length - 1);
                    requisicao = requisicao + parametros;

                    httpClient.BaseAddress = new Uri(ModeloAux.VariaveisGlobais.UrlWebAPi);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);

                    // HTTP GET
                    HttpResponseMessage response = await httpClient.GetAsync(requisicao);
                    if (response.IsSuccessStatusCode)
                    {
                        dadosImportacao = await response.Content.ReadAsAsync<List<ImportacaoDadosRep>>();
                        //string ret = await response.Content.ReadAsStringAsync();
                        //dadosImportacao = JsonConvert.DeserializeObject<List<Modelo.Proxy.PxyEnvioConfiguracoesDataHora>>(ret);
                    }
                    else
                    {
                        TratarErroRetornoApi(response, "Solicitar dados de envio de Empresa/Funcionário");
                    }
                    return dadosImportacao;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public async Task<Dictionary<int, string>> DeletaEmpFuncExpRep(List<int> idsDadosImportacao)
        {
            using (var httpClient = new HttpClient())
            {
                string idsimp = "";
                for (int i = 0; i < idsDadosImportacao.Count; i++)
                {
                    idsimp += idsDadosImportacao[i] + ", ";
                }
                try
                {
                    string requisicao = "api/ImportarDadosRep?";
                    string parametros = "";
                    for (int i = 0; i < idsDadosImportacao.Count; i++)
                    {
                        parametros += "idsDadosImportacao[" + i.ToString() + "]=" + idsDadosImportacao[i] + "&";
                    }
                    parametros = parametros.Substring(0, parametros.Length - 1);
                    requisicao = requisicao + parametros;
                    //ReportarProgresso(new ReportaErro() { Mensagem = "Enviando Configurações de Data/Hora a serem excluídas!", TipoMsg = TipoMensagem.Info }, progress);
                    httpClient.BaseAddress = new Uri(VariaveisGlobais.UrlWebAPi);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    HttpResponseMessage response = httpClient.DeleteAsync(requisicao).Result;
                    //ReportarProgresso(new ReportaErro() { Mensagem = "Recebido retorno das configurações de data e hora a serem excluídos!", TipoMsg = TipoMensagem.Info }, progress);
                    Dictionary<int, string> retorno = new Dictionary<int, string>();

                    if (response.IsSuccessStatusCode)
                    {
                        retorno = await response.Content.ReadAsAsync<Dictionary<int, string>>();
                    }
                    else
                    {
                        TratarErroRetornoApi(response, "Excluir dados de envio de Empresa/Funcionário");
                    }
                    return retorno;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public async Task<HttpResponseMessage> EnviarBiometria(Biometria Biometria)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var retorno = new HttpResponseMessage();
                    string str = "api/Biometria";
                    string contentJson = JsonConvert.SerializeObject(Biometria);
                    log.Info("Json do Rep " + Biometria.idRep + " Enviado: " + contentJson);
                    HttpContent content = new StringContent(contentJson, Encoding.UTF8, "application/json");
                    log.Info(Biometria.idRep + " Token recebido");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    httpClient.BaseAddress = new Uri(VariaveisGlobais.UrlWebAPi);
                    httpClient.Timeout = TimeSpan.FromMinutes(60);
                    HttpResponseMessage response = await httpClient.PostAsync(str, content);
                    if (response.IsSuccessStatusCode)
                    {
                        log.Info(Biometria.idRep + " Sucesso ao logar no sistema");
                        retorno = await response.Content.ReadAsAsync<HttpResponseMessage>();
                    }
                    else
                    {
                        log.Error(Biometria.idRep + " Falha ao logar no sistema");
                        TratarErroRetornoApi(response, "Enviar Biometria");
                    }
                    return retorno;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public async Task<ResultadoImportacao> EnviarLinhasAfdServidor(List<RegistroAFD> registros)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    ResultadoImportacao retorno = new ResultadoImportacao();
                    string str = "api/ImportarAFDServicoComunicador";
                    string contentJson = JsonConvert.SerializeObject(registros);
                    string NumSerie = registros.Where(x => x.Campo01 == "000000000").FirstOrDefault().Campo07;
                    log.Info(NumSerie + " Enviando conteúdo do JSON");
                    log.Info("Json do Rep " + NumSerie + " Enviado: " + contentJson);
                    HttpContent content = new StringContent(contentJson, Encoding.UTF8, "application/json");
                    log.Info(NumSerie + " Token recebido");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    httpClient.BaseAddress = new Uri(VariaveisGlobais.UrlWebAPi);
                    httpClient.Timeout = TimeSpan.FromMinutes(60);
                    HttpResponseMessage response = await httpClient.PostAsync(str, content);
                    if (response.IsSuccessStatusCode)
                    {
                        log.Info(NumSerie + " Sucesso ao logar no sistema");
                        retorno = await response.Content.ReadAsAsync<ResultadoImportacao>();
                    }
                    else
                    {
                        log.Error(NumSerie + " Falha ao logar no sistema");
                        TratarErroRetornoApi(response, "Enviar AFD");
                    }
                    return retorno;
                }
                catch (Exception e)
                {
                    //CwkUtils.LogarExceptions("EnviarLinhasAfdServidor", e, filePath);
                    throw e;
                }
            }
        }

        public async Task<bool> GravarLog(Modelo.RepLog log)
        {

            HttpResponseMessage response = await EnviarLog(log);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Configuracao.RequisitarNovoToken();
                    Modelo.Proxy.PxyConfigComunicadorServico conf = Configuracao.GetConfiguracao();
                    token = conf.TokenAccess;
                    response = await EnviarLog(log);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            throw new Exception("Requisição não autorizada, solicitando novo token.");
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
        }

        private async Task<HttpResponseMessage> EnviarLog(RepLog log)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    Modelo.RepLog retorno = new Modelo.RepLog();
                    string str = "api/RepLog";
                    string contentJson = JsonConvert.SerializeObject(log);
                    HttpContent content = new StringContent(contentJson, Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    httpClient.BaseAddress = new Uri(VariaveisGlobais.UrlWebAPi);
                    httpClient.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage response = await httpClient.PostAsync(str, content);
                    return response;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
