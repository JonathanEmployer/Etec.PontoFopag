using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace cwkComunicadorWebAPIPontoWeb.BLL
{
    public class RepBLL
    {

        private CancellationToken cts;
        private Progress<ReportaErro> progress;
        public async Task<ListaRepsViewModel> GetRepListAsync(string usuario, string token, string url, CancellationToken cancellationToken, IProgress<ReportaErro> progress)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string str = "Api/Rep?usuario=" + usuario;
                    ReportarProgresso(new ReportaErro() { Mensagem = "Solicitando Lista de Rep's", TipoMsg = TipoMensagem.Info}, progress);
                    if (cancellationToken != null)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HttpResponseMessage response = await httpClient.GetAsync(str, HttpCompletionOption.ResponseContentRead, cancellationToken);
                    ReportarProgresso(new ReportaErro() { Mensagem = "Recebida Lista de Rep's", TipoMsg = TipoMensagem.Info }, progress);
                    Thread.Sleep(1000);
                    response.EnsureSuccessStatusCode();
                    return new ListaRepsViewModel() { Reps = await response.Content.ReadAsAsync<List<RepViewModel>>() };
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("401"))
                    {
                        LoginBLL.SolicitaLogin();
                    }
                    ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao Solicitar lista de rep, Erro: "+e.Message, TipoMsg = TipoMensagem.Erro }, progress);
                    throw e;
                }
            }
        }

        public async Task<ListaRepsViewModel> GetAllRepsAsync(CancellationToken ct, IProgress<ReportaErro> progress)
        {
            LoginBLL loginBll = new LoginBLL();
            ListaRepsViewModel listaResult = new ListaRepsViewModel();
            try
            {
                 
                if (ct != null)
                {
                    ct.ThrowIfCancellationRequested();
                }
                if (VerificaArquivoRep())
                {
                    TokenResponseViewModel userData = await loginBll.GetXmlRegisterDataAsync();
                    listaResult = await GetAllRepsAsync(userData.Username, userData.AccessToken, VariaveisGlobais.URL_WS, ct, progress);
                }
                
            }
            catch (Exception e)
            {
                throw e;
            }
            return listaResult;
        }

        public async Task<ListaRepsViewModel> GetAllRepsAsync(string usuario, string token, string url, CancellationToken ct, IProgress<ReportaErro> progress)
        {
            ListaRepsViewModel listaResult = new ListaRepsViewModel();
            try
            {
                ListaRepsViewModel listaWeb = await GetRepListAsync(usuario, token, url, ct, progress);
                ListaRepsViewModel listaLocal = await GetXmlRepDataAsync();

                List<int> idsLocais = listaLocal.Reps.Select(s => s.Id).ToList();

                if (listaWeb.Reps.Count > 0)
                {
                    foreach (var item in listaWeb.Reps)
                    {
                        if (idsLocais.Contains(item.Id))
                        {
                            RepViewModel repLocal = listaLocal.Reps.FirstOrDefault(f => f.Id == item.Id);
                            RepViewModel repRemoto = item;
                            repRemoto.TipoImportacao = repLocal.TipoImportacao;
                            repRemoto.TempoImportacao = repLocal.TempoImportacao;
                            repRemoto.ImportacaoAtivada = repLocal.ImportacaoAtivada;
                            repRemoto.UltimoNsr = repLocal.UltimoNsr;
                            repRemoto.DataUltimoNsr = repLocal.DataUltimoNsr;
                            repRemoto.NumeroTentativasComunicacao = repLocal.NumeroTentativasComunicacao;
                            repRemoto.DataUltimaTentativa = repLocal.DataUltimaTentativa;
                            listaResult.Reps.Add(repRemoto);
                        }
                        else
                        {
                            listaResult.Reps.Add(item);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return listaResult;

        }

        public async Task<ListaRepsViewModel> GetAllRepsComNsrAsync(CancellationToken ct, IProgress<ReportaErro> progress)
        {
            LoginBLL loginBll = new LoginBLL();
            ListaRepsViewModel listaResult = new ListaRepsViewModel();
            try
            {
                if (!VerificaArquivoRep())
                {
                    return listaResult;
                }
                if (ct != null)
                {
                    ct.ThrowIfCancellationRequested();
                }
                TokenResponseViewModel userData = await loginBll.GetXmlRegisterDataAsync();
                listaResult = await GetAllRepsAsync(userData.Username, userData.AccessToken, VariaveisGlobais.URL_WS, ct, progress);

                if (listaResult.Reps.Where(w => w.UltimoNsr == 0).Count() > 0)
                {
                    IList<PxyNSR> nsrs = await GetRepNsrs(userData.Username, userData.AccessToken, VariaveisGlobais.URL_WS, listaResult.Reps, ct, progress);
                    foreach (var item in listaResult.Reps)
                    {
                        PxyNSR nsr = nsrs.Where(x => x.IdentificadorREP == item.NumRelogio).FirstOrDefault();
                        item.UltimoNsr = nsr.UltimoNSR;
                        item.DataUltimoNsr = nsr.DataUltimoNsr;
                    }

                    if (!await SetXmlRegisterData(listaResult))
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao gravar REP's no Arquivo de configuração", TipoMsg = TipoMensagem.Erro }, progress);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return listaResult;
        }

        private void ReportarProgresso(ReportaErro texto, IProgress<ReportaErro> progress)
        {
            if (progress != null)
            {
                progress.Report(texto);
            }
        }

        public bool VerificaArquivoRep()
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dir = Path.Combine(dir, "RepPontofopag.xml");
            return VerificarArquivoRep(dir);
        }

        private bool VerificarArquivoRep(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
            {
                try
                {
                    using (FileStream fs = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                    {
                        ListaRepsViewModel viewModel = new ListaRepsViewModel()
                        {
                            Reps = new List<RepViewModel>()
                        };
                        XmlSerializer serializer = new XmlSerializer(typeof(ListaRepsViewModel));
                        serializer.Serialize(fs, viewModel);
                        return true; 
                    }
                }
                catch (Exception e)
                {
                    string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "Erro_Verificar_Arquivo" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                    CwkUtils.LogarExceptions("Erro ao verificar o arquivo", e, filePath);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public async Task<ListaRepsViewModel> GetXmlRepDataAsync()
        {
            return await Task<ListaRepsViewModel>.Factory.StartNew(() =>
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ListaRepsViewModel));
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                dir = Path.Combine(dir, "RepPontofopag.xml");
                int numTries = 0;
                while (true)
                {
                    ++numTries;
                    try
                    {
                        using (FileStream fs = new FileStream(dir, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            ListaRepsViewModel ret = (ListaRepsViewModel)serializer.Deserialize(fs);
                            fs.Close();
                            fs.Dispose();
                            return ret;
                        }
                    }
                    catch (Exception e)
                    {
                        if (numTries > 10)
                        {
                            string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "GetXmlRepDataAsync" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                            CwkUtils.LogarExceptions("GetXmlRepDataAsync", e, filePath);
                            return new ListaRepsViewModel(); 
                        }
                        Thread.Sleep(500);
                    }
                }
                
            });
        }

        public async Task<bool> SetXmlRegisterData(ListaRepsViewModel conf)
        {
            ReportarProgresso(new ReportaErro() { Mensagem = "Atualizando dados do Rep no arquivo RepPontofopag.xml", TipoMsg = TipoMensagem.Info }, progress);
            int count = 0;
            bool importou = false;
            while (count < 30 && !importou)
            {
                importou = await Task<bool>.Factory.StartNew(() =>
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ListaRepsViewModel));
                    string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    dir = Path.Combine(dir, "RepPontofopag.xml");
                    using (FileStream fs = new FileStream(dir, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                    {
                        try
                        {
                            serializer.Serialize(fs, conf);
                            return true;
                        }
                        catch (Exception ex)
                        {
                            if (count == 29)
                            {
                                string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "Abrir_Arquivo_RepPontofopag" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                                CwkUtils.LogarExceptions("Abrir Arquivo RepPontofopag.xml", ex, filePath);
                            }
                            return false;
                        }
                    }
                });
                Thread.Sleep(100);
                count++;
            }
            if (importou)
                ReportarProgresso(new ReportaErro() { Mensagem = "Dados do Rep no arquivo RepPontofopag.xml atualizados com sucesso", TipoMsg = TipoMensagem.Info }, progress);
            else ReportarProgresso(new ReportaErro() { Mensagem = "Não possível atualizar os dados do Rep no arquivo RepPontofopag.xml", TipoMsg = TipoMensagem.Erro }, progress);
            return importou;
        }

        public async Task<IList<PxyNSR>> GetRepNsrs(string usuario, string token, string url, List<RepViewModel> Reps, CancellationToken cancellationToken, IProgress<ReportaErro> progress)
        {
            IList<PxyNSR> res = new List<PxyNSR>();

            using (var httpClient = new HttpClient())
            {
                try
                {
                    string str = "Api/NSR?usuario=" + usuario;
                    for (int i = 0; i < Reps.Count; i++)
                    {
                        str += "&numRelogios[" + i.ToString() + "]=" + Reps[i].NumRelogio;
                    }
                    ReportarProgresso(new ReportaErro() { Mensagem = "Solicitando NSR por Rep", TipoMsg = TipoMensagem.Info }, progress);
                    if (cancellationToken != null)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    httpClient.BaseAddress = new Uri(url);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                    System.Net.ServicePointManager.Expect100Continue = false;
                    HttpResponseMessage response = await httpClient.GetAsync(str, HttpCompletionOption.ResponseContentRead, cancellationToken);
                    ReportarProgresso(new ReportaErro() { Mensagem = "Recebendo NSR por Rep", TipoMsg = TipoMensagem.Info }, progress);
                    Thread.Sleep(1000);
                    response.EnsureSuccessStatusCode();
                    res = await response.Content.ReadAsAsync<IList<PxyNSR>>();
                }
                catch (Exception e)
                {
                    string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "Buscar_Lista_Rep_NSR" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                    CwkUtils.LogarExceptions("Busca Lista de Rep com NSR", e, filePath);
                    if (e.Message.Contains("401"))
                    {
                        LoginBLL.SolicitaLogin();
                    }
                    ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao solicitar lista de Rep's com os NSR, Erro: "+e.Message, TipoMsg = TipoMensagem.Erro }, progress);
                }
            }

            return res;
        }
    }
}
