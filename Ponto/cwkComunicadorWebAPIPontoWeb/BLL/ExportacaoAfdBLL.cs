using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using Newtonsoft.Json;
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
    public class ExportacaoAfdBLL
    {
        public async Task<object> ImportarBilheteArquivo(Stream s, CancellationToken ct, IProgress<ReportaErro> progress)
        {
            try
            {

                List<string> lista = new List<string>();
                lista = await Task.Factory.StartNew<List<string>>(() =>
                {
                    using (StreamReader sr = new StreamReader(s))
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "(Worker - Importação AFD) Iniciando leitura de arquivo AFD", TipoMsg = TipoMensagem.Info }, progress);
                        List<string> list = new List<string>();
                        string linha;
                        while ((linha = sr.ReadLine()) != null)
                        {
                            list.Add(linha);
                        }
                        ReportarProgresso(new ReportaErro() { Mensagem = "(Worker - Importação AFD) Leitura de arquivo AFD finalizada. Registros: " + list.Count.ToString(), TipoMsg = TipoMensagem.Info }, progress);
                        return list;
                    }
                }, ct);
                return lista;
            }
            catch (Exception e)
            {
                ReportarProgresso(new ReportaErro() { Mensagem = "(Worker - Importação AFD) Erro ao realizar a leitura do Stream do arquivo AFD: " + e.Message, TipoMsg = TipoMensagem.Erro }, progress);
                throw;
            }
        }


        public async Task<bool> EnviarLinhasAfdServidor(string usuario, string token, string url, string idRep, List<string> linhasAfd, CancellationToken ct, IProgress<ReportaErro> progress)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string str = "Api/ImportacaoBilhetes?login=" + usuario + "&idRep=" + idRep;
                    string contentJson = JsonConvert.SerializeObject(linhasAfd);
                    HttpContent content = new StringContent(contentJson, Encoding.UTF8, "application/json");
                    ReportarProgresso(new ReportaErro() { Mensagem = "(Worker - Comunicação) Enviando dados do arquivo AFD para o servidor", TipoMsg = TipoMensagem.Info }, progress);
                    ct.ThrowIfCancellationRequested();
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.Timeout = TimeSpan.FromMinutes(10);
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HttpResponseMessage response = await httpClient.PostAsync(str, content);
                    Thread.Sleep(1000);
                    response.EnsureSuccessStatusCode();
                    ReportarProgresso(new ReportaErro() { Mensagem = "(Worker - Comunicação) Dados do arquivo AFD enviados para o servidor", TipoMsg = TipoMensagem.Info }, progress);
                    bool retorno = await response.Content.ReadAsAsync<bool>();
                    return retorno;
                }
                catch (Exception e)
                {
                    string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "EnviarLinhasAfdServidor" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                    CwkUtils.LogarExceptions("EnviarLinhasAfdServidor", e, filePath);
                    if (e.Message.Contains("401"))
                    {
                        LoginBLL.SolicitaLogin();
                    }
                    ReportarProgresso(new ReportaErro() { Mensagem = "(Worker - Comunicação) Erro ao comunicar com o servidor: " + e.Message, TipoMsg = TipoMensagem.Erro }, progress);
                    return false;
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
