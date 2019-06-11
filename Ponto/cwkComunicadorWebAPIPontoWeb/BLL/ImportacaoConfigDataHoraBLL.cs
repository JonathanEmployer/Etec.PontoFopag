using cwkComunicadorWebAPIPontoWeb.Integracao;
using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cwkComunicadorWebAPIPontoWeb.BLL
{
    public class ImportacaoConfigDataHoraBLL
    {
        public async Task EnviarConfigDataHora(List<RepViewModel> lReps, String token, CancellationToken ct, Progress<ReportaErro> progress, bool tela)
        {
            List<EnvioConfiguracoesDataHoraViewModel> dadosEnviados = await OperacaoEnvio(lReps, token, ct, progress, tela);
            if (dadosEnviados.Count() > 0)
            {
                ct = await OperacaoExclusao(dadosEnviados, token, ct, progress);
            }
        }

        public async Task<List<EnvioConfiguracoesDataHoraViewModel>> OperacaoEnvio(List<RepViewModel> lReps, string token, CancellationToken ct, Progress<ReportaErro> progress, bool tela)
        {
            List<EnvioConfiguracoesDataHoraViewModel> DadosConfigDataHora = new List<EnvioConfiguracoesDataHoraViewModel>();
            DadosConfigDataHora = await buscaDadosConfigDataHoraWS(lReps, token, ct, progress, tela);
            foreach (EnvioConfiguracoesDataHoraViewModel ecdhvm in DadosConfigDataHora)
            {
                ct = await EnviaConfigDataHoraRep(ecdhvm, ct, progress);
            }
            return DadosConfigDataHora;
        }

        public async Task<CancellationToken> EnviaConfigDataHoraRep(EnvioConfiguracoesDataHoraViewModel ecdhvm, CancellationToken ct, Progress<ReportaErro> progress)
        {
            bool exibirLog;
            try
            {
                ConfiguracaoHorario ch = new ConfiguracaoHorario(ecdhvm.RepVM);
                bool enviar = false;
                if (ecdhvm.bEnviaDataHoraServidor)
                {
                    ReportarProgresso(new ReportaErro() { Mensagem = "Enviando Data e Hora para o REP " + ecdhvm.RepVM.DescRelogio + " (ID: " + ecdhvm.RepVM.Id + ")", TipoMsg = TipoMensagem.Info }, progress);
                    ch.EnviarDataHoraComputador = ecdhvm.bEnviaDataHoraServidor;
                    enviar = true;
                }
                if (ecdhvm.bEnviaHorarioVerao)
                {
                    ReportarProgresso(new ReportaErro() { Mensagem = "Enviando Horário de Verão para o REP " + ecdhvm.RepVM.DescRelogio + " (ID: " + ecdhvm.RepVM.Id + ")", TipoMsg = TipoMensagem.Info }, progress);
                    ch.SetHorarioVerao(ecdhvm.dtInicioHorarioVerao, ecdhvm.dtFimHorarioVerao);
                    enviar = true;
                }
                if (enviar)
                {
                    string log = String.Empty;
                    bool enviou = await Task.Factory.StartNew<bool>(() => { return ch.Enviar(out exibirLog, out log); }, ct);
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
                    ReportarProgresso(new ReportaErro() { Mensagem = "Configuração de Horário Enviada para o REP " + ecdhvm.RepVM.DescRelogio + " (ID: " + ecdhvm.RepVM.Id + ")", TipoMsg = TipoMensagem.Sucesso }, progress);
                }
            }
            catch (Exception exc)
            {
                ReportarProgresso(new ReportaErro() { Mensagem = "Houve um erro ao enviar os comandos ao REP " + ecdhvm.RepVM.DescRelogio + " (ID: " + ecdhvm.RepVM.Id + ") \r\n" + exc.Message, TipoMsg = TipoMensagem.Erro }, progress);
                CwkUtils.LogarExceptions("Log Envio Data Hora Rep", exc, CwkUtils.FileLogStringUtil());
            }
            return ct;
        }

        #region métodos para exclusão
        private async Task<CancellationToken> OperacaoExclusao(List<EnvioConfiguracoesDataHoraViewModel> dadosImportacao, String token, CancellationToken ct, Progress<ReportaErro> progress)
        {
            try
            {
                bool enviou = false;
                List<int> idsImport = dadosImportacao.Select(x => x.Id).ToList();
                Dictionary<int, string> retorno = await DeletaDadosConfigDataHora(idsImport, token, ct, progress);
                if (retorno.ContainsKey(0))
                {
                    string resultado = retorno[0];
                    ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao deletar os registros de configuração de data e hora, Erro: " + resultado, TipoMsg = TipoMensagem.Erro }, progress);
                    throw new Exception("Erro ao deletar os registros de configuração de data e hora, Erro: " + resultado);
                }
                else
                {
                    foreach (KeyValuePair<int, string> re in retorno)
                    {
                        ReportarProgresso(new ReportaErro()
                        {
                            Mensagem = (String.Format("{0}, {1}, {2}",
                            "Retorno exclusão de registros de configuração de data e hora: ",
                            re.Key,
                            re.Value)),
                            TipoMsg = TipoMensagem.Info
                        }, progress);
                    }
                }
            }
            catch (Exception ex)
            {
                string arquivoLog = "LogExclusaoConfiguracaoDataHora" + DateTime.Now.ToString("") + ".txt";
                arquivoLog = Path.Combine(CwkUtils.FileLogStringUtil(), arquivoLog);
                CwkUtils.LogarExceptions("Log de Exclusão de configuração de data e hora para WS/Remoção de comandos do WS", ex, arquivoLog);
                ReportarProgresso(new ReportaErro() { Mensagem = "Erro na exclusão de configuração de data e hora! Detalhe: " + ex.Message, TipoMsg = TipoMensagem.Erro }, progress);
                throw ex;
            }
            return ct;
        }
        
        public async Task<Dictionary<int, string>> DeletaDadosConfigDataHora(List<int> idsDadosConfigDataHora, string token, CancellationToken cancellationToken, IProgress<ReportaErro> progress)
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
                    ReportarProgresso(new ReportaErro() { Mensagem = "Enviando Configurações de Data/Hora a serem excluídas!", TipoMsg = TipoMensagem.Info }, progress);
                    if (cancellationToken != null)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    System.Net.ServicePointManager.Expect100Continue = false;
                    httpClient.BaseAddress = new Uri(ViewModels.VariaveisGlobais.URL_WS);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HttpResponseMessage response = await httpClient.DeleteAsync(requisicao, cancellationToken);
                    ReportarProgresso(new ReportaErro() { Mensagem = "Recebido retorno das configurações de data e hora a serem excluídos!", TipoMsg = TipoMensagem.Info }, progress);
                    Dictionary<int, string> retorno = new Dictionary<int, string>();
                    if (!response.IsSuccessStatusCode)
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Não foi possível deletar configurações de data e hora de ids: " + idsimp + " Detalhe: " + response, TipoMsg = TipoMensagem.Erro }, progress);
                        throw new Exception("Não foi possível deletar configurações de data e hora de ids: " + idsimp + " Detalhe: " + response);
                    }
                    else
                    {
                        retorno = await response.Content.ReadAsAsync<Dictionary<int, string>>();
                    }

                    return retorno;
                }
                catch (Exception e)
                {
                    string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "DeletaDadosConfigDataHora" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                    CwkUtils.LogarExceptions("DeletaDadosConfigDataHora", e, filePath);
                    if (e.Message.Contains("401"))
                    {
                        LoginBLL.SolicitaLogin();
                    }
                    Dictionary<int, string> erro = new Dictionary<int, string>();
                    ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao deletar os dados de configuração de data e hora de ids: " + idsimp + " Detalhes: " + e.Message, TipoMsg = TipoMensagem.Erro }, progress);
                    erro.Add(0, "Erro ao deletar os dados de configuração de data e hora de ids: " + idsimp + " Detalhes: " + e.Message);
                    return erro;
                }
            }
        }
        #endregion

        public async Task<List<EnvioConfiguracoesDataHoraViewModel>> buscaDadosConfigDataHoraWS(List<RepViewModel> lReps, String token, CancellationToken ct, Progress<ReportaErro> progress, bool tela)
        {
            try
            {
                List<EnvioConfiguracoesDataHoraViewModel> DadosConfigDataHora = new List<EnvioConfiguracoesDataHoraViewModel>();
                List<int> idsReps = new List<int>();
                if (lReps.Where(w => w.ImportacaoAtivada).Count() == 0)
                {
                    ReportarProgresso(new ReportaErro() { Mensagem = "Nenhum Rep Ativado para Importação. Entre na Opção \"Config. Reps\" e Ative a opção \"Ativar Importação\" o Rep Desejado!", TipoMsg = TipoMensagem.Aviso }, progress);
                    if (tela)
                    {
                        MessageBox.Show("Nenhum Rep Ativado para Importação. Entre na Opção \"Config. Reps\" e Ative a opção \"Ativar Importação\" o Rep Desejado!");   
                    }
                    return new List<EnvioConfiguracoesDataHoraViewModel>();
                }
                foreach (RepViewModel rep in lReps.Where(x => x.ImportacaoAtivada == true))
                {
                    idsReps.Add(rep.Id);
                }
                ImportacaoDadosRepBLL idr = new ImportacaoDadosRepBLL();
                DadosConfigDataHora = await idr.GetConfiguracoesDataHoraAsync(idsReps, token, ViewModels.VariaveisGlobais.URL_WS, ct, progress);
                foreach (EnvioConfiguracoesDataHoraViewModel item in DadosConfigDataHora)
                {
                    Modelo.REP relogio = new Modelo.REP();
                    RepViewModel rvm = lReps.Where(x => x.Id == item.idRelogio).FirstOrDefault();
                    item.RepVM = rvm;
                }
                return DadosConfigDataHora;
            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                {
                    ex = ((AggregateException)ex).Flatten();
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in ((AggregateException)ex).InnerExceptions)
                    {
                        sb.Append(item.Message);
                    }
                    if (tela)
                    {
                        MessageBox.Show(sb.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                else
                {
                    if (tela)
                    {
                        MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                return new List<EnvioConfiguracoesDataHoraViewModel>();
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
