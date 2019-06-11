using cwkComunicadorWebAPIPontoWeb.Integracao;
using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cwkComunicadorWebAPIPontoWeb.BLL
{
    public class ImportacaoDadosRepBLL
    {
        public async Task EnviarEmpresaFuncionarios(List<RepViewModel> lReps, string token, string url, CancellationToken ct, Progress<ReportaErro> progress)
        {
            ct = await OperacaoEnvio(lReps, token, url, ct, progress);

            ct = await OperacaoExclusao(lReps, token, url, ct, progress);
        }

        private async Task<CancellationToken> OperacaoExclusao(List<RepViewModel> lReps, string token, string url, CancellationToken ct, Progress<ReportaErro> progress)
        {
            try
            {
                List<int> idsReps = new List<int>();
                foreach (RepViewModel rep in lReps.Where(x => x.ImportacaoAtivada == true))
                {
                    idsReps.Add(rep.Id);
                }
                ImportacaoDadosRepBLL idr = new ImportacaoDadosRepBLL();
                List<ImportacaoDadosRepViewModel> dadosImportacao = await idr.GetImportacaoDadosRepAsync(idsReps, token, url, ct, progress);
                bool enviou = false;

                // Filtro aparenas para registros com operação de enviar.
                //s.EnvioDadosRep.Operacao == 1 
                foreach (ImportacaoDadosRepViewModel idrvm in dadosImportacao.Where(s => s.EnvioDadosRep.bOperacao == 1))
                {
                    ExcluiFuncionariosRelogio efr = new ExcluiFuncionariosRelogio(idrvm.Empresas.Where(x => x.Documento == idrvm.Rep.CpfCnpjEmpregador.Replace(".", "").Replace("/", "").Replace("-", "")).FirstOrDefault(), idrvm.Empregados, idrvm.Rep);
                    bool exibirLog;
                    try
                    {
                        string log = String.Empty;
                        enviou = await Task.Factory.StartNew<bool>(() => { return efr.Enviar(out exibirLog, out log); }, ct);
                        if (!enviou)
                        {
                            if (String.IsNullOrEmpty(log))
                            {
                                throw new Exception("Comando não enviado. Possível Causa: Falta de Conexão Com o Rep.");
                            }
                            else
                            {
                                throw new Exception("Comando não enviado. Erro: " + log);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Houve um erro ao enviar os comandos ao REP " + idrvm.Rep.NomeFabricante + " - " + idrvm.Rep.NomeModelo + " (ID: " + idrvm.Rep.Id + ") \r\n" + exc.Message, TipoMsg = TipoMensagem.Erro }, progress);
                        throw new Exception("Houve um erro ao enviar os comandos ao REP " + idrvm.Rep.NomeFabricante + " - " + idrvm.Rep.NomeModelo + " (ID: " + idrvm.Rep.Id + ") \r\n" + exc.Message);
                    }
                }
                if (enviou)
                {
                    List<int> idsImport = dadosImportacao.Select(x => x.EnvioDadosRep.Id).ToList();
                    Dictionary<int, string> retorno = await idr.DeletaDadosImportadosRepAsync(idsImport, token, url, ct, null);
                    if (retorno.ContainsKey(0))
                    {
                        string resultado = retorno[0];
                        ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao deletar os registros de importação para o rep, Erro: " + resultado, TipoMsg = TipoMensagem.Erro }, progress);
                        throw new Exception("Erro ao deletar os registros de importação para o rep, Erro: " + resultado);
                    }
                    else
                    {
                        foreach (KeyValuePair<int, string> re in retorno)
                        {
                            ReportarProgresso(new ReportaErro()
                            {
                                Mensagem = (String.Format("{0}, {1}, {2}",
                                "Retorno exclusão de registros Importação: ",
                                re.Key,
                                re.Value)),
                                TipoMsg = TipoMensagem.Info
                            }, progress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string arquivoLog = "LogExclusaoFuncionariosAntesDaImportação" + DateTime.Now.ToString("") + ".txt";
                arquivoLog = Path.Combine(CwkUtils.FileLogStringUtil(), arquivoLog);
                CwkUtils.LogarExceptions("Log de Exclusão de Funcionarios para WS/Remoção de comandos do WS", ex, arquivoLog);
                ReportarProgresso(new ReportaErro() { Mensagem = "Erro na exclusão de funcionários! Detalhe: " + ex.Message, TipoMsg = TipoMensagem.Erro }, progress);
                throw ex;
            }
            return ct;
        }

        private async Task<CancellationToken> OperacaoEnvio(List<RepViewModel> lReps, string token, string url, CancellationToken ct, Progress<ReportaErro> progress)
        {
            try
            {
                List<int> idsReps = new List<int>();
                foreach (RepViewModel rep in lReps.Where(x => x.ImportacaoAtivada == true))
                {
                    idsReps.Add(rep.Id);
                }
                ImportacaoDadosRepBLL idr = new ImportacaoDadosRepBLL();
                List<ImportacaoDadosRepViewModel> dadosImportacao = await idr.GetImportacaoDadosRepAsync(idsReps, token, url, ct, progress);
                bool enviou = false;

                // Filtro apenas para registros com operação de enviar.
                //s.EnvioDadosRep.Operacao == 0 
                foreach (ImportacaoDadosRepViewModel idrvm in dadosImportacao.Where(s => s.EnvioDadosRep.bOperacao == 0))
                {
                    EnvioEmpresaEFuncionarios eef = new EnvioEmpresaEFuncionarios(idrvm.Empresas.Where(x => x.Documento == idrvm.Rep.CpfCnpjEmpregador.Replace(".", "").Replace("/", "").Replace("-", "")).FirstOrDefault(), idrvm.Empregados, lReps.Where(x => x.Id == idrvm.Rep.Id).FirstOrDefault());
                    bool exibirLog;
                    try
                    {
                        string log = String.Empty;
                        enviou = await Task.Factory.StartNew<bool>(() => { return eef.Enviar(out exibirLog, out log); }, ct);
                        if (!enviou)
                        {
                            if (String.IsNullOrEmpty(log))
                            {
                                throw new Exception("Comando não enviado. Possível Causa: Falta de Conexão Com o Rep.");
                            }
                            else
                            {
                                throw new Exception("Comando não enviado. Erro: " + log);
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Houve um erro ao enviar os comandos ao REP " + idrvm.Rep.NomeFabricante + " - " + idrvm.Rep.NomeModelo + " (ID: " + idrvm.Rep.Id + ") \r\n" + exc.Message, TipoMsg = TipoMensagem.Erro }, progress);
                        throw new Exception("Houve um erro ao enviar os comandos ao REP " + idrvm.Rep.NomeFabricante + " - " + idrvm.Rep.NomeModelo + " (ID: " + idrvm.Rep.Id + ") \r\n" + exc.Message);
                    }
                }
                if (enviou)
                {
                    List<int> idsImport = dadosImportacao.Select(x => x.EnvioDadosRep.Id).ToList();
                    Dictionary<int, string> retorno = await idr.DeletaDadosImportadosRepAsync(idsImport, token, url, ct, null);
                    if (retorno.ContainsKey(0))
                    {
                        string resultado = retorno[0];
                        ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao deletar os registros de importação para o rep, Erro: " + resultado, TipoMsg = TipoMensagem.Erro }, progress);
                        throw new Exception("Erro ao deletar os registros de importação para o rep, Erro: " + resultado);
                    }
                    else
                    {
                        foreach (KeyValuePair<int, string> re in retorno)
                        {
                            ReportarProgresso(new ReportaErro()
                            {
                                Mensagem = (String.Format("{0}, {1}, {2}",
                                "Retorno exclusão de registros Importação: ",
                                re.Key,
                                re.Value)),
                                TipoMsg = TipoMensagem.Info
                            }, progress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string arquivoLog = "LogEnvioFuncionariosAntesDaImportação" + DateTime.Now.ToString("") + ".txt";
                arquivoLog = Path.Combine(CwkUtils.FileLogStringUtil(), arquivoLog);
                CwkUtils.LogarExceptions("Log de Envio de Funcionarios para WS/Remoção de comandos do WS", ex, arquivoLog);
                ReportarProgresso(new ReportaErro() { Mensagem = "Erro no envio de funcionários! Detalhe: " + ex.Message, TipoMsg = TipoMensagem.Erro }, progress);
                throw ex;
            }
            return ct;
        }

        public async Task<List<ImportacaoDadosRepViewModel>> GetImportacaoDadosRepAsync(List<int> idsRep, string token, string url, CancellationToken cancellationToken, IProgress<ReportaErro> progress)
        {
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

                    ReportarProgresso(new ReportaErro() { Mensagem = "Requisitando dados a serem importados no rep!", TipoMsg = TipoMensagem.Info }, progress);
                    if (cancellationToken != null)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HttpResponseMessage response = await httpClient.GetAsync(requisicao, HttpCompletionOption.ResponseContentRead, cancellationToken);
                    ReportarProgresso(new ReportaErro() { Mensagem = "Recebendo dados a serem importados no rep!", TipoMsg = TipoMensagem.Info }, progress);
                    if (!response.IsSuccessStatusCode)
                    {
                        string idsimp = "";
                        for (int i = 0; i < idsRep.Count; i++)
                        {
                            idsimp += idsRep[i] + ", ";
                        }
                        ReportarProgresso(new ReportaErro() { Mensagem = "Não foi possivel importar os dados dos relógios de ids: " + idsimp, TipoMsg = TipoMensagem.Erro }, progress);
                        throw new Exception("Não foi possivel importar os dados dos relógios de ids: " + idsimp);
                    }
                    List<ImportacaoDadosRepViewModel> dadosImportacao = await response.Content.ReadAsAsync<List<ImportacaoDadosRepViewModel>>();
                    return dadosImportacao;
                }
                catch (Exception e)
                {
                    string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "GetImportacaoDadosRepAsync" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                    CwkUtils.LogarExceptions("GetImportacaoDadosRepAsync", e, filePath);
                    if (e.Message.Contains("401"))
                    {
                        LoginBLL.SolicitaLogin();
                    }
                    if (e.Message.Contains("An error occurred while sending the request."))
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Impossível conectar-se ao servidor remoto", TipoMsg = TipoMensagem.Erro }, progress);
                    }
                    return new List<ImportacaoDadosRepViewModel>();
                }
            }
        }


        public async Task<Dictionary<int, string>> DeletaDadosImportadosRepAsync(List<int> idsDadosImportacao, string token, string url, CancellationToken cancellationToken, IProgress<ReportaErro> progress)
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
                    ReportarProgresso(new ReportaErro() { Mensagem = "Enviando dados de Importações a serem excluídos!", TipoMsg = TipoMensagem.Info }, progress);
                    if (cancellationToken != null)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HttpResponseMessage response = await httpClient.DeleteAsync(requisicao, cancellationToken);
                    ReportarProgresso(new ReportaErro() { Mensagem = "Recebido retorno dos dados de importações a serem excluídos!", TipoMsg = TipoMensagem.Info }, progress);
                    Dictionary<int, string> retorno = new Dictionary<int, string>();
                    if (!response.IsSuccessStatusCode)
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Não foi possível deletar os dados importados de ids: " + idsimp + " Detalhe: " + response, TipoMsg = TipoMensagem.Erro }, progress);
                        throw new Exception("Não foi possível deletar os dados importados de ids: " + idsimp + " Detalhe: " + response);
                    }
                    else
                    {
                        retorno = await response.Content.ReadAsAsync<Dictionary<int, string>>();
                    }

                    return retorno;
                }
                catch (Exception e)
                {
                    string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "DeletaDadosImportadosRepAsync" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                    CwkUtils.LogarExceptions("DeletaDadosImportadosRepAsync", e, filePath);
                    if (e.Message.Contains("401"))
                    {
                        LoginBLL.SolicitaLogin();
                    }
                    Dictionary<int, string> erro = new Dictionary<int, string>();
                    ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao deletar os dados importados de ids: " + idsimp + " Detalhes: " + e.Message, TipoMsg = TipoMensagem.Erro }, progress);
                    erro.Add(0, "Erro ao deletar os dados importados de ids: " + idsimp + " Detalhes: " + e.Message);
                    return erro;
                }
            }
        }

        public async Task<List<EnvioConfiguracoesDataHoraViewModel>> GetConfiguracoesDataHoraAsync(List<int> idsRep, string token, string url, CancellationToken cancellationToken, IProgress<ReportaErro> progress)
        {
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

                    ReportarProgresso(new ReportaErro() { Mensagem = "Requisitando dados de Data e Hora a serem importados no rep!", TipoMsg = TipoMensagem.Info }, progress);
                    if (cancellationToken != null)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    System.Net.ServicePointManager.Expect100Continue = false;

                    // HTTP GET
                    HttpResponseMessage response = await httpClient.GetAsync(requisicao);
                    ReportarProgresso(new ReportaErro() { Mensagem = "Recebendo dados de Data e Hora a serem importados no rep!", TipoMsg = TipoMensagem.Info }, progress);
                    if (!response.IsSuccessStatusCode)
                    {
                        string idsimp = "";
                        for (int i = 0; i < idsRep.Count; i++)
                        {
                            idsimp += idsRep[i] + ", ";
                        }
                        ReportarProgresso(new ReportaErro() { Mensagem = "Não foi possível importar os dados de Data e Hora dos relógios de ids: " + idsimp, TipoMsg = TipoMensagem.Erro }, progress);
                        throw new Exception("Não foi possível importar os dados de Data e Hora dos relógios de ids: " + idsimp);
                    }
                    List<EnvioConfiguracoesDataHoraViewModel> dadosImportacao = await response.Content.ReadAsAsync<List<EnvioConfiguracoesDataHoraViewModel>>();
                    return dadosImportacao;
                }
                catch (Exception e)
                {
                    string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "GetConfiguracoesDataHoraAsync" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                    CwkUtils.LogarExceptions("GetConfiguracoesDataHoraAsync", e, filePath);
                    if (e.Message.Contains("401"))
                    {
                        LoginBLL.SolicitaLogin();
                    }
                    if (e.Message.Contains("An error occurred while sending the request."))
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Impossível conectar-se ao servidor remoto", TipoMsg = TipoMensagem.Erro }, progress);
                    }
                    return new List<EnvioConfiguracoesDataHoraViewModel>();
                }
            }
        }

        private void ReportarProgresso(ReportaErro texto, IProgress<ReportaErro> progress)
        {
            if (progress != null)
            {
                progress.Report(texto);
            }
        }
    }
}
