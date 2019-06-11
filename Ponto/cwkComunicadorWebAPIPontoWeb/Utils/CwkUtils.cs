using cwkComunicadorWebAPIPontoWeb.BLL;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace cwkComunicadorWebAPIPontoWeb.Utils
{
    public static class CwkUtils
    {

        public enum tipoErro 
        { 
            Internet = 1,
            Relogio = 2,
            WS = 3
        }

        public static string Base64Encode(string plainText)
        {
            if (String.IsNullOrEmpty(plainText))
            {
                plainText = string.Empty;
            }
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            if (String.IsNullOrEmpty(base64EncodedData))
            {
                base64EncodedData = string.Empty;
            }
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string FileLogStringUtil()
        {
            string dirApp = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dirApp = Path.Combine(dirApp, "Logs");
            if (!Directory.Exists(dirApp))
            {
                try
                {
                    Directory.CreateDirectory(dirApp);
                    VerificaArquivosAntigos(dirApp);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            
            return dirApp;
        }

        public static string FileLogStringUtil(params string[] arvoreDiretorios)
        {
            List<string> dirs = new List<string>();
            string dirApp = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            dirs.Add(dirApp);
            dirs.Add("Logs");
            dirs.AddRange(arvoreDiretorios);
            dirApp = Path.Combine(dirs.ToArray());
            if (!Directory.Exists(dirApp))
            {
                try
                {
                    Directory.CreateDirectory(dirApp);
                    VerificaArquivosAntigos(dirApp);
                }
                catch (Exception ec)
                {
                    return "";
                }
            }
            return dirApp;
        }

        private static void VerificaArquivosAntigos(string dirApp)
        {
            string[] caminhoArquivos = Directory.GetFiles(dirApp);
            DateTime creationDT;
            foreach (var caminhoArquivo in caminhoArquivos)
            {
                creationDT = new DateTime();
                creationDT = File.GetCreationTime(caminhoArquivo);

                if ((DateTime.Now - creationDT).Days > 7)
                {
                    File.Delete(caminhoArquivo);
                }
            }
        }

        public static void LogarExceptions(string tituloLog, Exception ex, string arquivoLog)
        {
            try
            {
//                try
//                {
//                    cwkPontoMT.Integracao.Entidades.Empresa empregador = new cwkPontoMT.Integracao.Entidades.Empresa();
//                    empregador = Utils.CwkUtils.EmpresaRep();
//                    string usuario = Utils.CwkUtils.NomeUsuario();
//                    string assunto = "Erro no Comunicador da Empresa " + empregador.RazaoSocial + "Gerado as " + System.DateTime.Now.ToString("G");
//                    string conteudoEmail = @"<p><span><strong>Empresa </strong>{empresa}</span></p>
//                                                <p><span><strong>Documento Empresa </strong>{documento}</span></p>
//                                                <p><span><strong>Usuario Logado: </strong>{usuario}</span></p>
//                                                <p><span><strong>Descrição do Erro: </strong>{Message}</span></p>
//                                                <p><span><strong>StackTrace: </strong>{StackTrace}</span></p>";
//                            conteudoEmail = conteudoEmail.Replace("{empresa}", empregador.RazaoSocial)
//                                .Replace("{documento}", empregador.Documento)
//                                .Replace("{usuario}", usuario)
//                                .Replace("{Message}", ex.Message)
//                                .Replace("{StackTrace}", ex.StackTrace);
//                            EnviarEmail.EnviarEmailErroComLogs(assunto, conteudoEmail,"");
//                }
//                catch (Exception)
//                {
//                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("-------------------------------------------------------------------------------------------");
                sb.AppendLine("Log de " + tituloLog + ": " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString());
                sb.AppendLine("-------------------------------------------------------------------------------------------");
                if (ex is AggregateException)
                {
                    AggregateException exc = (AggregateException)ex;
                    exc.Flatten();
                    foreach (var item in exc.InnerExceptions)
                    {
                        sb.AppendLine("-------------------------------------------------------------------------------------------");
                        sb.AppendLine(item.Message);
                        sb.AppendLine("-------------------------------------------------------------------------------------------");
                        sb.AppendLine(item.StackTrace);
                        sb.AppendLine("-------------------------------------------------------------------------------------------");
                    }
                }
                else
                {
                    sb.AppendLine("-------------------------------------------------------------------------------------------");
                    sb.AppendLine(ex.Message);
                    sb.AppendLine("-------------------------------------------------------------------------------------------");
                    sb.AppendLine(ex.StackTrace);
                    sb.AppendLine("-------------------------------------------------------------------------------------------");
                }

                using (StreamWriter file = new StreamWriter(arquivoLog, true))
                {
                    file.WriteLine(sb.ToString());
                }
            }
            catch (Exception z)
            {
            }
        }

        public static async Task<object> WaitForFileAndExecuteRead(string fullPath, CancellationToken ct, IProgress<ReportaErro> progress, Func<Stream, CancellationToken, IProgress<ReportaErro>, Task<object>> func)
        {
            int numTries = 0;
            if (!File.Exists(fullPath))
            {
                return null;
            }
            ReportarProgresso(new ReportaErro() { Mensagem = "(Worker - Importação) Aguardando arquivo " + fullPath, TipoMsg = TipoMensagem.Info }, progress);
            while (true)
            {
                ++numTries;
                try
                {
                    using (FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        object o = await func(fs, ct, progress);
                        return o;
                    }
                }
                catch (Exception ex)
                {
                    ReportarProgresso(new ReportaErro() { Mensagem = "(Worker - Importação) Arquivo bloqueado. Tentativa #" + numTries + " Arquivo: " + fullPath, TipoMsg = TipoMensagem.Aviso }, progress);
                    if (numTries > 10)
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "(Worker - Importação) Arquivo bloqueado. Tentativa #" + numTries + " Arquivo: " + fullPath + "Detalhes do Bloqueio: " + ex.Message, TipoMsg = TipoMensagem.Erro }, progress);
                        string filePath = Path.Combine(CwkUtils.FileLogStringUtil(), "Log_Envio_Arquivo_Servidor" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".txt");
                        CwkUtils.LogarExceptions("Leitura Arquivo txt de AFD", ex, filePath);
                        return null;
                    }

                    // Wait for the lock to be released
                    System.Threading.Thread.Sleep(500);
                }
            }
        }

        public static async Task<bool> InternetDisponivel()
        {
            bool res = await EnderecoDisponivel("8.8.8.8");
            if (!res)
            {
                res = await EnderecoDisponivel("google.com");
            }
            if (!res)
            {
                res = await EnderecoDisponivel("bing.com");
            }
            if (!res)
            {
                res = await EnderecoDisponivel(VariaveisGlobais.WsProducao);
            }
            if (!res)
            {
                res = VerificaConexaoEndereco("http://www.google.com");
            }
            if (!res)
            {
                res = VerificaConexaoEndereco(VariaveisGlobais.WsProducao);
            }

            return res;
        }

        public static bool VerificaConexaoEndereco(string endereco)
        {
            bool res = false;
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead(endereco))
                    {
                        res = true;
                    }
                }
                return res;
            }
            catch (Exception)
            {
                return false;
            }
          
        }

        public static async Task<bool> EnderecoDisponivel(string host)
        {
            return await Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    string hostOk = "";
                    if (host.Contains("http://"))
                    {
                        hostOk = host.Replace("http://", "");
                        if (hostOk.Contains(":"))
                        {
                            hostOk = hostOk.Split(':')[0];
                        }
                    }
                    else if (host.Contains("https://"))
                    {
                        hostOk = host.Replace("https://", "");
                        if (hostOk.Contains(":"))
                        {
                            hostOk = hostOk.Split(':')[0];
                        }
                    }
                    else
                    {
                        hostOk = host;
                    }
                    byte[] buffer = Encoding.ASCII.GetBytes(".a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a");
                    PingOptions options = new PingOptions(50, true);
                    AutoResetEvent reset = new AutoResetEvent(false);
                    Ping ping = new Ping();
                    PingReply reply = ping.Send(hostOk, 5000, buffer, options);
                    bool res = reply.Status == IPStatus.Success;
                    return res;
                }
                catch (Exception e)
                {
                    return false;
                }
            });
        }

        public async static Task<bool> VerificarPortaHost(string ip, int porta, int timeout)
        {
            bool isUp;
            if (!await EnderecoDisponivel(ip))
            {
                return false;
            }
            return await Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    string hostOk = "";
                    if (ip.Contains("http://"))
                    {
                        hostOk = ip.Replace("http://", "");
                        if (hostOk.Contains(":"))
                        {
                            var spt = hostOk.Split(':');
                            hostOk = spt[0];
                            if (porta == 0)
                            {
                                porta = Convert.ToInt32(new string(spt[1].ToCharArray().Where((c => char.IsDigit(c))).ToArray()));
                            }
                        }
                    }
                    else if (ip.Contains("https://"))
                    {
                        hostOk = ip.Replace("https://", "");
                        if (hostOk.Contains(":"))
                        {
                            hostOk = hostOk.Split(':')[0];
                        }
                    }
                    else
                    {
                        hostOk = ip;
                    }
                    using (TcpClient tcp = new TcpClient())
                    {
                        IAsyncResult ar = tcp.BeginConnect(hostOk, porta, null, null);
                        WaitHandle wh = ar.AsyncWaitHandle;

                        try
                        {
                            if (!wh.WaitOne(TimeSpan.FromMilliseconds(timeout), false))
                            {
                                tcp.EndConnect(ar);
                                tcp.Close();
                                throw new SocketException();
                            }

                            isUp = true;
                            tcp.EndConnect(ar);
                        }
                        finally
                        {
                            wh.Close();
                        }
                    }
                }
                catch (SocketException e)
                {
                    isUp = false;
                }

                return isUp;
            });
        }

        public static void ReiniciarSistema()
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath); // to start new instance of application
            Process.GetCurrentProcess().Kill();
        }

        private static void ReportarProgresso(ReportaErro texto, IProgress<ReportaErro> progress)
        {
            if (progress != null)
            {
                progress.Report(texto);
            }
        }

        public static DateTime RandomDate(Random generator, DateTime rangeStart, DateTime rangeEnd)
        {
            TimeSpan span = rangeEnd - rangeStart;

            int randomMinutes = generator.Next(0, (int)span.TotalMinutes);
            return rangeStart + TimeSpan.FromMinutes(randomMinutes);
        }

        public static string NomeUsuario()
        {
            LoginBLL bll = new LoginBLL();
            XDocument xD = new XDocument();
            try
            {
                xD = bll.GetXmlConf();
                if (!String.IsNullOrEmpty(xD.Element("ConfiguracaoPontofopag").Element("un").Value))
                {
                    return xD.Element("ConfiguracaoPontofopag").Element("un").Value;
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static cwkPontoMT.Integracao.Entidades.Empresa EmpresaRep()
        {
            cwkPontoMT.Integracao.Entidades.Empresa emp = new cwkPontoMT.Integracao.Entidades.Empresa();
            try
            {
                ListaRepsViewModel lRepsViewModel = new ListaRepsViewModel();
                RepBLL bll = new RepBLL();
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
                            lRepsViewModel = ret;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        if (numTries > 10)
                        {
                            lRepsViewModel = new ListaRepsViewModel();
                            break;
                        }
                        Thread.Sleep(500);
                    }
                }
                RepViewModel rep1 = lRepsViewModel.Reps.Where(x => x.ImportacaoAtivada == true).FirstOrDefault();
                emp.RazaoSocial = rep1.NomeEmpregador;
                emp.Documento = rep1.CpfCnpjEmpregador;
                emp.Local = rep1.EnderecoEmpregador;
            }
            catch (Exception)
            {
            }
            return emp;
        }

        public static void EscreveLog(string nomeArquivo, string log)
        {
            EscreveLog(VariaveisGlobais.CaminhoLog, nomeArquivo, log);
        }

        public static void EscreveLog(string caminhoPasta, string nomeArquivo, string log)
        {
            StreamWriter file2 = new StreamWriter(caminhoPasta + "\\" + nomeArquivo + ".txt", true);
            CultureInfo cult = new CultureInfo("pt-BR");
            string dta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", cult);
            file2.WriteLine(dta + " - " + log);
            file2.Close();
        }

        public static void CriaPastaInexistente(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }
    }

}
