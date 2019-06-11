using cwkAtualizadorComunicadorPontoWeb.ViewModels;
using cwkComunicadorWebAPIPontoWeb.BLL;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

namespace cwkComunicadorWebAPIPontoWeb.Utils
{
    class AtualizarAplicativo
    {
        public Progress<ReportaErro> progress { get; set; }
        public VersaoPastaViewModel maiorVersaoDisponivel = new VersaoPastaViewModel();
        List<ArquivosAtualizacaoViewModel> lArquivosAtualizar;
        public CwkFtp cwkFtp;
        public AtualizarAplicativo(Progress<ReportaErro> progresso)
        {
            progress = progresso;
        }

        public List<ArquivosAtualizacaoViewModel> VerificaAtualizacao(out string versao, out string ultimaVersao)
        {
            lArquivosAtualizar = new List<ArquivosAtualizacaoViewModel>();
            try
            {
                //Busca dados do aplicativo
                Assembly assembly = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                ultimaVersao = "";
                versao = fvi.FileVersion;
                ReportarProgresso(new ReportaErro() { Mensagem = "Conectando ao servidor para verificar versões disponíveis", TipoMsg = TipoMensagem.Info }, progress);
                cwkFtp = new CwkFtp(VariaveisGlobais.EnderecoFTP, VariaveisGlobais.UsuarioFTP, VariaveisGlobais.SenhaFTP);
                IList<InfoArquivoFtp> DadosDiretorio = cwkFtp.ListaDadosDiretorioDetalhado();
                IList<VersaoPastaViewModel> versoes = new List<VersaoPastaViewModel>();
                VersaoPastaViewModel maiorVersaoDisponivel = new VersaoPastaViewModel();
                cwkFtp.SubDiretorio("Application Files/Pontofopag Comunicador");
                ReportarProgresso(new ReportaErro() { Mensagem = "Listando versões disponíveis.", TipoMsg = TipoMensagem.Info }, progress);
                DadosDiretorio = cwkFtp.ListaDadosDiretorioDetalhado();

                foreach (InfoArquivoFtp linhaDir in DadosDiretorio)
                {
                    PreencheObjVersoes(versoes, linhaDir);
                }

                if (versoes.Max(x => x.Principal) > 0)
                {
                    maiorVersaoDisponivel = versoes.OrderBy(o => o.Principal).ThenBy(o => o.Secundaria).ThenBy(o => o.Compilacao).ThenBy(o => o.Revisao).LastOrDefault();
                    ultimaVersao = maiorVersaoDisponivel.VersaoStringResumida;
                    ReportarProgresso(new ReportaErro() { Mensagem = "Última versão encontrada: " + maiorVersaoDisponivel.VersaoStringResumida, TipoMsg = TipoMensagem.Info }, progress);
                    ReportarProgresso(new ReportaErro() { Mensagem = "Comparando versão do sistema: " + versao + " com a última versão disponível: " + maiorVersaoDisponivel.VersaoStringResumida, TipoMsg = TipoMensagem.Info }, progress);
                    //Caso as versões não sejam iguais atualiza o sistema
                    if (!comparaVersoes(maiorVersaoDisponivel.VersaoStringResumida, versao))
                    {
                        cwkFtp.SubDiretorio(maiorVersaoDisponivel.Nome);
                        DadosDiretorio = cwkFtp.ListaDadosDiretorioDetalhado();
                        string dirAtual = AppDomain.CurrentDomain.BaseDirectory;
                        foreach (InfoArquivoFtp arquivoFtp in DadosDiretorio)
                        {
                            string arquivo = dirAtual + @"\" + arquivoFtp.Nome;
                            FileInfo arquivoAtual = new FileInfo(arquivo);
                            if (!arquivoFtp.ehDiretorio)
                            {
                                if (arquivoAtual.Exists)
                                {
                                    if (arquivoFtp.Tamanho > 0)
                                    {
                                        PreencherArquivoAtualizar(lArquivosAtualizar, arquivoFtp, acao.Atualizar);
                                    }
                                }
                                else
                                {
                                    PreencherArquivoAtualizar(lArquivosAtualizar, arquivoFtp, acao.Incluir);
                                }
                            }
                        }
                    }
                    else
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Sistema já se encontra atualizado. Versão do sistema: " + versao + " última versão disponível: " + maiorVersaoDisponivel.VersaoStringResumida, TipoMsg = TipoMensagem.Info }, progress);
                    }
                }
            }
            catch (Exception ex)
            {
                ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao verificar atualizações, Erro: " + ex.Message, TipoMsg = TipoMensagem.Erro }, progress);
                throw ex;
            }
            return lArquivosAtualizar;
        }
        private static bool comparaVersoes(string versaoFtp, string versaoAplicativo)
        {
            string[] arrayVersaoFtp = versaoFtp.Split('.');
            string[] arrayVersaoAplicativo = versaoAplicativo.Split('.');
            //Count -1 para não comparar a Release (último número da versão)
            for (int i = 0; i < arrayVersaoFtp.Count(); i++)
            {
                if (i < 3)
                {
                    if (Convert.ToInt32(arrayVersaoFtp[i]) > Convert.ToInt32(arrayVersaoAplicativo[i]))
                    {
                        return false;
                    }    
                }
                else
                {
                    if (Convert.ToInt32(arrayVersaoFtp[i]) > Convert.ToInt32(arrayVersaoAplicativo[i]) && Convert.ToInt32(arrayVersaoFtp[i-1]) == Convert.ToInt32(arrayVersaoAplicativo[i-1]))
                    {
                        return false;
                    }  
                }
                
            }
            return true;
        }

        private static void PreencherArquivoAtualizar(List<ArquivosAtualizacaoViewModel> lArquivosAtualizar, InfoArquivoFtp arquivoFtp, acao acao)
        {
            ArquivosAtualizacaoViewModel arquivoAtualizar = new ArquivosAtualizacaoViewModel();
            arquivoAtualizar.NomeArquivo = arquivoFtp.Nome;
            arquivoAtualizar.DataModificacao = arquivoFtp.DataModificacao;
            arquivoAtualizar.TamanhoBytes = Convert.ToInt32(arquivoFtp.Tamanho);
            arquivoAtualizar.Acao = acao;
            lArquivosAtualizar.Add(arquivoAtualizar);
        }

        private static void PreencheObjVersoes(IList<VersaoPastaViewModel> versoes, InfoArquivoFtp linhaDir)
        {
            VersaoPastaViewModel vp = new VersaoPastaViewModel();
            vp.Nome = linhaDir.Nome;
            vp.VersaoString = linhaDir.Nome;
            versoes.Add(vp);
        }


        public List<ArquivosAtualizacaoViewModel> BaixarArquivosParaAtualizacao(string versao, ProgressBar progressBar, Label lbProgress, Label lbArquivo, string versaoCorrente)
        {
            try
            {

                //Cria estrutura de pasta necessária para controle das versões.
                if (CriaPastasAtualizacao())
                {
                    //Verifica se já foi chamado o método para Verificar Atualizações, caso ainda não tenha executado, executa.
                    if (lArquivosAtualizar == null)
                    {
                        lArquivosAtualizar = VerificaAtualizacao(out versaoCorrente, out versao);
                    }
                    if (progressBar == null) { progressBar = new System.Windows.Forms.ProgressBar(); }
                    if (lbProgress == null) { lbProgress = new System.Windows.Forms.Label(); }
                    if (lbArquivo == null) { lbArquivo = new System.Windows.Forms.Label(); }


                    string dirApp = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    String dirAtualizar = Path.Combine(dirApp, "Atualizacoes\\Atualizar\\" + versao);
                    if (!CriarPasta(dirAtualizar))
                    {
                        return lArquivosAtualizar;
                    }
                    //Set up progress bar
                    progressBar.Value = 0;
                    double totalKB = lArquivosAtualizar.Sum(x => x.TamanhoKB);
                    int totalKBint;
                    Int32.TryParse(totalKB.ToString(), out totalKBint);
                    progressBar.Maximum = totalKBint;
                    lbProgress.Text = "0 / " + totalKBint.ToString() + " KB";
                    int qtdArquivos = lArquivosAtualizar.Count();
                    lbArquivo.Text = "Baixando: 0 / " + qtdArquivos;
                    Application.DoEvents();
                    int count = 0;
                    int KBLidosTotal = 0;
                    foreach (ArquivosAtualizacaoViewModel atualizar in lArquivosAtualizar)
                    {
                        try
                        {
                            count++;
                            lbArquivo.Text = "Baixando: " + count + " / " + qtdArquivos + " - " + atualizar.NomeArquivo;
                            Application.DoEvents();
                            using (FileStream outputStream = new FileStream(dirAtualizar + "\\" + atualizar.NomeArquivo, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                            {
                                cwkFtp.SubDiretorio(atualizar.NomeArquivo);
                                int tamanhoArquivo = 0;
                                Stream ftpStream = cwkFtp.DownloadArquivo(out tamanhoArquivo);
                                int bufferSize = 2048;
                                byte[] buffer = new byte[bufferSize];

                                int bytesRead = ftpStream.Read(buffer, 0, bufferSize);
                                int bytes = 0;
                                int kbRead = 0;
                                while (bytesRead > 0)
                                {
                                    outputStream.Write(buffer, 0, bytesRead);
                                    bytesRead = ftpStream.Read(buffer, 0, bufferSize);
                                    bytes += bytesRead;
                                    double kbreal = Math.Round((bytes / 1024f));
                                    Int32.TryParse(kbreal.ToString(), out kbRead);
                                    if (KBLidosTotal + kbRead <= progressBar.Maximum)
                                    {
                                        progressBar.Value = KBLidosTotal + kbRead;
                                        lbProgress.Text = progressBar.Value.ToString() + " / " + totalKBint.ToString() + " KB";

                                        progressBar.Refresh();
                                        Application.DoEvents();
                                    }
                                }
                                KBLidosTotal += kbRead;
                                ftpStream.Close();
                                cwkFtp.DiretorioAnterior();
                            }
                            atualizar.Acao = acao.Baixado;
                            string arquivoCriado = dirAtualizar + "\\" + atualizar.NomeArquivo;
                        }
                        catch (Exception w)
                        {
                            throw w;
                        }
                    }
                    progressBar.Value = totalKBint;
                    lbProgress.Text = totalKBint.ToString() + " / " + totalKBint.ToString() + " KB";
                    lbArquivo.Text = "Download concluído com sucesso!";
                    lbArquivo.Text = "Preparando atualização!";
                    Application.DoEvents();
                    #region Atualizando o "Atualizador"
                    dirAtualizar = Path.Combine(dirApp, "Atualizacoes\\");
                    cwkFtp.SubDiretorio("Atualizacoes");
                    string arquivo = "cwkAtualizadorArquivosComunicadorWebAPIPontoWeb.exe";
                    DownloadArquivoFTP(dirAtualizar, KBLidosTotal, arquivo);
                    arquivo = "Ionic.Zip.dll";
                    DownloadArquivoFTP(dirAtualizar, KBLidosTotal, arquivo);
                    try
                    {
                        arquivo = "Ionic.Zip.xml";
                        DownloadArquivoFTP(dirAtualizar, KBLidosTotal, arquivo);
                    }
                    catch (Exception ex)
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Não foi possivel atualizar o arquivo: " + arquivo, TipoMsg = TipoMensagem.Erro }, progress);
                    }


                    #endregion
                    lbArquivo.Text = "Iniciando Atualização!";
                    try
                    {
                        cwkPontoMT.Integracao.Entidades.Empresa empregador = new cwkPontoMT.Integracao.Entidades.Empresa();
                        empregador = Utils.CwkUtils.EmpresaRep();
                        string usuario = Utils.CwkUtils.NomeUsuario();
                        string assunto = "Iniciando Atualização Comunicador para versão " + versao + " da empresa: " + empregador.RazaoSocial + " as " + System.DateTime.Now.ToString("G");
                        string conteudoEmail = @"<p><span><strong>Empresa </strong>{empresa}</span></p>
                                                <p><span><strong>Documento Empresa </strong>{documento}</span></p>
                                                <p><span><strong>Usuario Logado: </strong>{usuario}</span></p>
                                                <p><span><strong>Versão Corrente: </strong>{versaoCorrente}</span></p>
                                                <p><span><strong>Nova Versão: </strong>{novaVersao}</span></p>";
                        conteudoEmail = conteudoEmail.Replace("{empresa}", empregador.RazaoSocial)
                            .Replace("{documento}", empregador.Documento)
                            .Replace("{usuario}", usuario)
                            .Replace("{versaoCorrente}", versaoCorrente)
                            .Replace("{novaVersao}", versao);
                        EnviarEmail.SendMailErros(assunto, conteudoEmail, "", "");
                    }
                    catch (Exception)
                    {
                    }
                    ChamaAtualizador(dirApp);
                }
                return lArquivosAtualizar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int DownloadArquivoFTP(String dirAtualizar, int KBLidosTotal, string arquivo)
        {
            using (FileStream outputStream = new FileStream(dirAtualizar + "\\" + arquivo, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                cwkFtp.SubDiretorio(arquivo);
                int tamanhoArquivo = 0;
                Stream ftpStream = cwkFtp.DownloadArquivo(out tamanhoArquivo);
                int bufferSize = 2048;
                byte[] buffer = new byte[bufferSize];

                int bytesRead = ftpStream.Read(buffer, 0, bufferSize);
                int bytes = 0;
                int kbRead = 0;
                while (bytesRead > 0)
                {
                    outputStream.Write(buffer, 0, bytesRead);
                    bytesRead = ftpStream.Read(buffer, 0, bufferSize);
                    bytes += bytesRead;
                    double kbreal = Math.Round((bytes / 1024f));
                    Int32.TryParse(kbreal.ToString(), out kbRead);
                }
                KBLidosTotal += kbRead;
                ftpStream.Close();
                cwkFtp.DiretorioAnterior();
            }
            return KBLidosTotal;
        }

        private static void ChamaAtualizador(string dirApp)
        {
            //Pasta onde esta o aplicativo atualizador
            String dirExeAtualizador = Path.Combine(dirApp, "Atualizacoes");
            //Pega todos os exe's da pasta
            String[] PossiveisExeAtualziador = Directory.GetFiles(dirExeAtualizador, "*.exe");
            //Pega o exe que contiver o nome de atualizador
            string pathAtualizador = PossiveisExeAtualziador.Where(x => x.Contains("Atualizador")).FirstOrDefault();
            //Abre o Atualizador
            Process.Start(pathAtualizador);
            //Fecha o Comunicador
            Process.GetCurrentProcess().Kill();
        }

        private bool CriarPasta(String dirAtualizar)
        {
            int tentativas = 0;
            while (tentativas <= 5)
            {
                try
                {
                    tentativas++;
                    Directory.CreateDirectory(dirAtualizar);
                    return true;
                }
                catch (Exception ex)
                {
                    if (tentativas == 5)
                    {
                        ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao tentar realizar a atualização do sistema! Pasta de atualização deve estar aberta ou com algum arquivo em execução!", TipoMsg = TipoMensagem.Erro }, progress);
                        CwkUtils.LogarExceptions("Log Atualização do Sistema", ex, CwkUtils.FileLogStringUtil());
                        return false;
                    }
                    System.Threading.Thread.Sleep(500);
                }
            }
            return true;
        }

        private bool CriaPastasAtualizacao()
        {
            try
            {
                int tentativas = 0;
                while (true)
                {
                    try
                    {
                        string dirApp = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        dirApp = Path.Combine(dirApp, "Atualizacoes");
                        if (!Directory.Exists(dirApp))
                        {
                            Directory.CreateDirectory(dirApp);
                        }

                        String dirAtualizar = Path.Combine(dirApp, "Atualizar");
                        if (Directory.Exists(dirAtualizar))
                        {
                            Directory.Delete(dirAtualizar, true);
                        }
                        Directory.CreateDirectory(dirAtualizar);

                        String dirBkpVersoes = Path.Combine(dirApp, "BkpVersoes");
                        if (!Directory.Exists(dirBkpVersoes))
                        {
                            Directory.CreateDirectory(dirBkpVersoes);
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        tentativas++;
                        if (tentativas == 5)
                        {
                            ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao tentar realizar a atualização do sistema! Pasta de atualização deve estar aberta ou com algum arquivo em execução!", TipoMsg = TipoMensagem.Erro }, progress);
                            CwkUtils.LogarExceptions("Log Atualização do Sistema", ex, CwkUtils.FileLogStringUtil());
                            return false;
                        }
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void VerificaPastasAtualizacao()
        {

        }

        private void ReportarProgresso(ReportaErro texto, IProgress<ReportaErro> progress)
        {
            if (progress != null)
            {
                progress.Report(texto);
            }
        }

        public static bool AtualizacaoAutomatica()
        {
            try
            {
                LoginBLL loginBLL = new LoginBLL();
                XDocument xD = loginBLL.GetXmlConf();
                if (Convert.ToInt32(xD.Element("ConfiguracaoPontofopag").Element("AtualizacaoAutomatica").Value) > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
