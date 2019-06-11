using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using cwkComunicadorWebAPIPontoWeb.Utils;
using System.Threading;
using System.Threading.Tasks;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using cwkPontoMT.Integracao;

namespace cwkComunicadorWebAPIPontoWeb.BLL.Workers
{
    public static class EnviaBilheteWorker
    {
        public static void Enviar(object source, FileSystemEventArgs e)
        {
            FileSystemWatcherWithProgress<ReportaErro> fsea = (FileSystemWatcherWithProgress<ReportaErro>)source;
            IProgress<ReportaErro> progress = fsea.ProgressObject;
            Task.Factory.StartNew(async () => await EnviarAsync(e, progress)).ConfigureAwait(false);
        }

        private static async Task<bool> EnviarAsync(FileSystemEventArgs e, IProgress<ReportaErro> progress)
        {
            int tentativas = 0;
            while (!(await CwkUtils.InternetDisponivel()))
            {
                Thread.Sleep(5000);
                tentativas++;
                if (tentativas == 12)
                {
                    ReportaProgresso(new ReportaErro() { Mensagem = "(Worker - EnviaBilhete) Arquivos não enviados por falta de internet, aguardando conexão...", TipoMsg = TipoMensagem.Erro }, progress);
                    tentativas = 0;
                }
            }

            bool migrado = await EnviarArquivo(e.FullPath, progress);
            string dirPosSincronia = CwkUtils.FileLogStringUtil("AFDsExportados");
            if (migrado)
            {
                try
                {
                    await MigrarArquivo(e, dirPosSincronia, progress);
                }
                catch (Exception ex)
                {
                    if (ex is AggregateException)
                    {
                        StringBuilder sb = new StringBuilder();
                        ex = ((AggregateException)ex).Flatten();
                        foreach (var item in ((AggregateException)ex).InnerExceptions)
                        {
                            sb.AppendLine(item.Message);
                        }
                    }
                    throw new Exception ("Erro ao movimentar AFD para exportado, detalhes: "+ex.Message);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private async static Task<bool> MigrarArquivo(FileSystemEventArgs e, string dirPosSincronia, IProgress<ReportaErro> progress)
        {
            return await Task.Factory.StartNew<bool>(() =>
            {
                try
                {
                    ReportaProgresso(new ReportaErro() { Mensagem = "(Worker - EnviaBilhete) Movendo arquivo importado para exportado: " + e.Name, TipoMsg = TipoMensagem.Info }, progress);
                    if (!File.Exists(Path.Combine(dirPosSincronia, Path.GetFileName(e.FullPath))))
                    {
                        File.Move(e.FullPath, Path.Combine(dirPosSincronia, Path.GetFileName(e.FullPath)));
                    }
                    else
                    {
                        Thread.Sleep(10);
                        File.Replace(e.FullPath, Path.Combine(dirPosSincronia, Path.GetFileName(e.FullPath)),
                            Path.Combine(dirPosSincronia, "bkp" + DateTime.Now.Millisecond + Path.GetFileName(e.FullPath)));
                    }
                    ReportaProgresso(new ReportaErro() { Mensagem = "(Worker - EnviaBilhete) Arquivo movido para exportado: " + e.Name, TipoMsg = TipoMensagem.Info }, progress);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }

        private async static Task<bool> EnviarArquivo(string p, IProgress<ReportaErro> progress)
        {
            CancellationToken ct = new CancellationToken();
            LoginBLL bllLogin = new LoginBLL();
            RepBLL bllRep = new RepBLL();
            TokenResponseViewModel login = await bllLogin.GetXmlRegisterDataAsync();
            ExportacaoAfdBLL bllExp = new ExportacaoAfdBLL();
            List<string> linhasAfd = new List<string>();
            var temp = await CwkUtils.WaitForFileAndExecuteRead(p, ct, progress, bllExp.ImportarBilheteArquivo);
            if (temp != null)
            {
                linhasAfd = (List<string>)temp;
            }
            bool enviouWS = false;
            bool setouXmlRep = false;
            string idRep = Path.GetFileName(p).Split('_').ToList().Last().Split('.').First();
            if (linhasAfd.Count > 0)
            {
                try
                {
                    enviouWS = await bllExp.EnviarLinhasAfdServidor(login.Username, login.AccessToken, ViewModels.VariaveisGlobais.URL_WS, idRep, linhasAfd, ct, progress);
                    if (enviouWS)
                    {
                        ListaRepsViewModel reps = await bllRep.GetAllRepsAsync(ct, null);
                        IList<PxyNSR> nsrs = await bllRep.GetRepNsrs(login.Username, login.AccessToken, ViewModels.VariaveisGlobais.URL_WS, reps.Reps, ct, null);
                        foreach (var item in reps.Reps)
                        {
                            if (item.Id.ToString().Equals(idRep))
                            {
                                RegistroAFD reg = Util.RetornaLinhaAFD(linhasAfd.Where(w => !w.StartsWith("999999999")).Last());
                                PxyNSR nsr = nsrs.Where(x => x.IdentificadorREP == item.NumRelogio).FirstOrDefault();
                                int ultimoNsrOnline = nsr.UltimoNSR;
                                item.UltimoNsr = ultimoNsrOnline >= reg.Nsr ? ultimoNsrOnline : item.UltimoNsr;
                            }
                        }
                        setouXmlRep = await bllRep.SetXmlRegisterData(reps);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao enviar o AFD para o servidor, detalhes: " + ex.Message);
                }
            }
            return enviouWS && setouXmlRep;
        }

        /// <summary>
        /// Verifica os arquivos que estão na pasta e envia para o WS
        /// </summary>
        /// <param name="caminho">Caminho da pasta onde ficam os arquivos</param>
        /// <param name="progress">método responsável por mostrar as mensagens no log do sistema</param>
        /// <param name="idRep">Passar o id do rep caso queira verificar apenas os arquivos de um rep específico, passar 0 para todos</param>
        /// <returns>Se conseguir enviar todos os aquivos ou não existir nenhum retorna true, caso ocorra erro retorna false</returns>
        public async static Task<bool> LancadorArquivosExportacao(string caminho, IProgress<ReportaErro> progress, int idRep, string descricaoJob)
        {
            ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Verificando arquivos não enviados do rep" + " Job: " + descricaoJob }, progress);
            bool retorno = true;
            DirectoryInfo di = new DirectoryInfo(caminho);
            var arquivos = di.EnumerateFiles("*.txt", SearchOption.AllDirectories).Where(f => (f.Attributes & FileAttributes.Hidden) == 0);
            if (idRep > 0)
            {
                arquivos = arquivos.Where(w => w.Name.Contains("_IDREP_" + idRep));
            }

            if (arquivos.Count() > 0)
            {

                ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Arquivos não enviados encontrados = " + arquivos.Count() + ", Job: " + descricaoJob }, progress);

                foreach (var item in arquivos)
                {
                    ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Enviados arquivo = " + item.FullName + ", Job: " + descricaoJob }, progress);
                    bool enviou = await EnviarAsync(new FileSystemEventArgs(WatcherChangeTypes.Created, caminho, Path.GetFileName(item.FullName)), progress);
                    if (!enviou)
                    {
                        ReportaProgresso(new ReportaErro() { Mensagem = "(Worker - Importação) Erro ao enviar o arquivo para o servidor, verifique o log de importação. Arquivo: " + item.FullName + ", Job: " + descricaoJob, TipoMsg = TipoMensagem.Erro }, progress);
                        ReportaProgresso(new ReportaErro() { Mensagem = "*** Envio de arquivos interrompido ***", TipoMsg = TipoMensagem.Erro }, progress);
                        retorno = false;
                        break;
                    }
                }
            }
            else
            {
                ReportaProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job) Não foram encontrados arquivos pendentes, Job: " + descricaoJob }, progress);
            }
            return retorno;
        }

        private static void ReportaProgresso(ReportaErro e, IProgress<ReportaErro> progress)
        {
            if (progress != null)
            {
                progress.Report(e);
            }
        }
    }
}
