using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Newtonsoft.Json;
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

namespace cwkComunicadorWebAPIPontoWeb.BLL
{
    public class LoginBLL
    {
        public async Task<TokenResponseViewModel> LoginAsync(string usuario, string senha, string url, CancellationToken cancellationToken, IProgress<ReportaErro> progress)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string str = "grant_type=password&username=" + usuario + "&password=" + senha;
                    HttpContent content = new StringContent(str);
                    ReportarProgresso(new ReportaErro() { Mensagem = "Enviando dados para login", TipoMsg = TipoMensagem.Info }, progress);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    cancellationToken.ThrowIfCancellationRequested();
                    System.Net.ServicePointManager.Expect100Continue = false;
                    httpClient.BaseAddress = new Uri(url);
                    HttpResponseMessage response = await httpClient.PostAsync("Token", content);
                    Thread.Sleep(1000);
                    response.EnsureSuccessStatusCode();
                    ReportarProgresso(new ReportaErro() { Mensagem = "Login Efetuado", TipoMsg = TipoMensagem.Info }, progress);
                    return response.Content.ReadAsAsync<TokenResponseViewModel>().Result;
                }
                catch (Exception e)
                {
                    string msg = string.Empty;
                    if (e.Message.Contains("400 (Bad Request)"))
                    {
                        msg = "Usuário/Senha incorretos.";
                        ReportarProgresso(new ReportaErro() { Mensagem = msg, TipoMsg = TipoMensagem.Erro }, progress);
                    }
                    else
                    {
                        msg = "Não foi possível contactar o servidor. " + e.Message;
                        ReportarProgresso(new ReportaErro() { Mensagem = msg, TipoMsg = TipoMensagem.Erro }, progress);
                    }

                    return new TokenResponseViewModel() { ErrorDescription = msg };
                }
            }
        }

        public async Task<XDocument> GetXmlConfAsync(CancellationToken cancellationToken, IProgress<ReportaErro> progress)
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dir = Path.Combine(dir, "ConfiguracaoPontofopag.xml");
            cancellationToken.ThrowIfCancellationRequested();
            XDocument xD = new XDocument();
            try
            {
                xD = await VerificarArquivoConfAsync(dir, cancellationToken, progress);
                if (progress != null)
                {
                    ReportarProgresso(new ReportaErro() { Mensagem = "Configuração OK", TipoMsg = TipoMensagem.Info }, progress);
                }
            }
            catch (Exception e)
            {
                if (e is AggregateException)
                {
                    if (progress != null)
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao abrir a configuração", TipoMsg = TipoMensagem.Erro }, progress);
                    }
                    throw ((AggregateException)e).Flatten();
                }
            }
            return xD;
        }

        public XDocument GetXmlConf()
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dir = Path.Combine(dir, "ConfiguracaoPontofopag.xml");
            XDocument xD = new XDocument();
            try
            {
                if (!File.Exists(dir))
                {
                    try
                    {
                        using (FileStream fs = new FileStream(dir, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                        {
                            TokenResponseViewModel viewModel = new TokenResponseViewModel()
                            {
                                AccessToken = "",
                                ExpiresAt = new DateTime(),
                                Username = "",
                                AtualizacaoAutomatica = 1
                            };
                            XmlSerializer serializer = new XmlSerializer(typeof(TokenResponseViewModel));
                            serializer.Serialize(fs, viewModel);
                        }
                        return XDocument.Load(dir);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    XDocument x = new XDocument();
                    try
                    {
                        x = XDocument.Load(dir);
                    }
                    catch (Exception e)
                    {
                        
                        throw e;
                    }
                    return x;
                }
            }
            catch (Exception e)
            {
                if (e is AggregateException)
                {
                    throw ((AggregateException)e).Flatten();
                }
            }
            return xD;
        }

        private async Task<XDocument> VerificarArquivoConfAsync(string caminhoArquivo, CancellationToken ct, IProgress<ReportaErro> progress)
        {
            ct.ThrowIfCancellationRequested();
            if (progress != null)
            {
                ReportarProgresso(new ReportaErro() { Mensagem = "Criando/Abrindo Configuração", TipoMsg = TipoMensagem.Info }, progress);
            }
            return await Task.Factory.StartNew<XDocument>(() =>
            {
                if (!File.Exists(caminhoArquivo))
                {
                    try
                    {
                        using (FileStream fs = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                        {
                            TokenResponseViewModel viewModel = new TokenResponseViewModel()
                            {
                                AccessToken = "",
                                ExpiresAt = new DateTime(),
                                Username = "",
                                AtualizacaoAutomatica = 1
                            };
                            XmlSerializer serializer = new XmlSerializer(typeof(TokenResponseViewModel));
                            serializer.Serialize(fs, viewModel);
                        }
                        return XDocument.Load(caminhoArquivo);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    XDocument x = new XDocument();
                    try
                    {
                        x = XDocument.Load(caminhoArquivo);
                    }
                    catch (Exception e)
                    {
                        
                        throw e;
                    }
                    return x;
                }
            }, ct);
        }

        public async Task<TokenResponseViewModel> GetXmlRegisterDataAsync()
        {
            return await Task<TokenResponseViewModel>.Factory.StartNew(() =>
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TokenResponseViewModel));
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                dir = Path.Combine(dir, "ConfiguracaoPontofopag.xml");
                using (FileStream fs = new FileStream(dir, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    TokenResponseViewModel ret = (TokenResponseViewModel)serializer.Deserialize(fs);
                    return ret;
                }
            });
        }

        public async Task<bool> SetXmlRegisterData(TokenResponseViewModel conf)
        {
            return await Task<bool>.Factory.StartNew(() =>
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TokenResponseViewModel));
                string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                dir = Path.Combine(dir, "ConfiguracaoPontofopag.xml");
                using (FileStream fs = new FileStream(dir, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    try
                    {
                        serializer.Serialize(fs, conf);
                        return true;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
            });
        }

        public async Task LogoutAsync()
        {

        }

        public static void SolicitaLogin()
        {
            try
            {
                LimpaTokenCwork();
                CwkUtils.ReiniciarSistema();
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void LimpaConfiguracaoCwork()
        {
            string caminhoArquivo = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            caminhoArquivo = Path.Combine(caminhoArquivo, "ConfiguracaoPontofopag.xml");
            LoginBLL bll = new LoginBLL();
            XDocument xD = bll.GetXmlConf();

            using (FileStream fs = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                TokenResponseViewModel viewModel = new TokenResponseViewModel()
                {
                    AccessToken = "",
                    ExpiresAt = new DateTime(),
                    Username = "",
                    AtualizacaoAutomatica = Convert.ToInt32(AtualizarAplicativo.AtualizacaoAutomatica())
                };
                XmlSerializer serializer = new XmlSerializer(typeof(TokenResponseViewModel));
                serializer.Serialize(fs, viewModel);
            }
        }

        public static void LimpaTokenCwork()
        {
            LoginBLL bll = new LoginBLL();
            XDocument xD = bll.GetXmlConf();
            string caminhoArquivo = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            caminhoArquivo = Path.Combine(caminhoArquivo, "ConfiguracaoPontofopag.xml");
            File.Delete(caminhoArquivo);

            using (FileStream fs = new FileStream(caminhoArquivo, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                TokenResponseViewModel viewModel = new TokenResponseViewModel()
                {
                    AccessToken = "",
                    ExpiresAt = new DateTime(),
                    Username = xD.Element("ConfiguracaoPontofopag").Element("un").Value,
                    pass = CriptoString.Decrypt(xD.Element("ConfiguracaoPontofopag").Element("ps").Value),
                    AtualizacaoAutomatica = Convert.ToInt32(AtualizarAplicativo.AtualizacaoAutomatica())
                };
                XmlSerializer serializer = new XmlSerializer(typeof(TokenResponseViewModel));
                serializer.Serialize(fs, viewModel);
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
