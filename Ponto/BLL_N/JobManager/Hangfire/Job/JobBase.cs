using BLL_N.Hubs;
using Hangfire.Console;
using Hangfire.Server;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Proxy;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BLL_N.JobManager.Hangfire.Job
{
    public abstract class JobBase
    {
        protected int minprogress;
        protected int maxprogress;
        protected int valorcorrenteprogress = 0;
        protected Modelo.ProgressBar pb = new Modelo.ProgressBar();
        protected PxyJobReturn jobRetorno;
        protected PxyJobReturn jobRetornoAnt;
        protected PerformContext context;
        protected Modelo.UsuarioPontoWeb userPF;
        protected int progress = 0;
        private DateTime ultimoReport = DateTime.Now;
        protected bool AdicinaValorCorrenteDeTotalNaMensagem { get; set; }


        public JobBase()
        {
            pb.incrementaPB = this.IncrementaProgressBar;
            pb.setaMensagem = this.SetaMensagem;
            pb.setaMinMaxPB = this.SetaMinMaxProgressBar;
            pb.setaValorPB = this.SetaValorProgressBar;
            pb.incrementaPBCMensagem = this.IncrementaProgressBarCMensagem;
            pb.setaValorPBCMensagem = this.SetaValorProgressBarCMensagem;
            pb.valorCorrenteProgress = this.ValorCorrenteProgress;
            pb.validaCancelationToken = this.ValidaCancelationToken;
        }

        protected void SetParametersBase(PerformContext context, JobControl jobReport, string db, string usuario)
        {
            this.jobRetorno = new PxyJobReturn(jobReport);
            this.context = context;
            this.userPF = new Modelo.UsuarioPontoWeb() { Login = usuario, ConnectionString = BLL.cwkFuncoes.ConstroiConexao(db).ConnectionString };
        }

        protected string CaminhoArquivo()
        {
            string path = ConfigurationManager.AppSettings["ArquivosPontofopag"];
            if (String.IsNullOrEmpty(path))
                throw new Exception("O patch(Caminho) para salvar os relatório não foi informado, informe no arquivo de configuração o valor da variavel PathRelatorios");

            if (String.IsNullOrEmpty(userPF.DataBase))
                throw new Exception("Nome do banco de dados não encontrado");

            return Path.Combine(path, userPF.DataBase.Contains("_") ? userPF.DataBase.Split('_')[1] : userPF.DataBase);
        }

        #region Métodos para progress
        protected void IncrementaProgressBarCMensagem(int incremento, string mensagem)
        {
            VerificaCancelamentoJob(context);
            valorcorrenteprogress = valorcorrenteprogress + incremento;
            if (AdicinaValorCorrenteDeTotalNaMensagem)
            {
                mensagem = String.Format("({0}/{1}) " + mensagem, valorcorrenteprogress, maxprogress);
            }
            if (valorcorrenteprogress <= maxprogress)
            {
                decimal percProgress = ((valorcorrenteprogress * 100) / maxprogress);
                progress = Convert.ToInt32(Decimal.Round(percProgress));
            }
            else
            {
                progress = 99; // Passo o valor setado ultrapasse o maximo do progresso, seto o progresso como 100%
            }
            if (jobRetorno.Progress != progress || (jobRetorno.Mensagem != mensagem && !String.IsNullOrEmpty(mensagem)))
            {
                jobRetorno.Progress = progress;
                jobRetorno.Mensagem = !String.IsNullOrEmpty(mensagem) ? mensagem : jobRetorno.Mensagem;
                ReportProgress();
            }
    }

        protected void SetaValorProgressBarCMensagem(int valor, string mensagem)
        {
            VerificaCancelamentoJob(context);
            if (AdicinaValorCorrenteDeTotalNaMensagem)
            {
                mensagem = String.Format("({0}/{1}) " + mensagem, valor, maxprogress);
            }
            
            if (jobRetorno.Progress != valor || (jobRetorno.Mensagem != mensagem && !String.IsNullOrEmpty(mensagem)))
            {
                jobRetorno.Progress = valor;
                jobRetorno.Mensagem = !String.IsNullOrEmpty(mensagem) ? mensagem : jobRetorno.Mensagem;
                ReportProgress();
            }
        }

        protected void IncrementaProgressBar(int incremento)
        {
            VerificaCancelamentoJob(context);
            IncrementaProgressBarCMensagem(incremento, jobRetorno.Mensagem);
        }

        protected void SetaValorProgressBar(int valor)
        {
            valorcorrenteprogress = valor;
        }

        protected void SetaMinMaxProgressBar(int min, int max)
        {
            VerificaCancelamentoJob(context);
            minprogress = min;
            maxprogress = max;
            valorcorrenteprogress = 0;
        }

        protected void SetaMensagem(string mensagem)
        {
            VerificaCancelamentoJob(context);
            if ((jobRetorno.Mensagem != mensagem && !String.IsNullOrEmpty(mensagem)))
            {
                jobRetorno.Mensagem = !String.IsNullOrEmpty(mensagem) ? mensagem : jobRetorno.Mensagem;
                ReportProgress();
            }
        }

        protected void ValidaCancelationToken()
        {
            VerificaCancelamentoJob(context);
        }

        protected void IncrementaProgressBarVazio(int incremento)
        {
        }

        protected void SetaValorProgressBarVazio(int valor)
        {
        }

        protected void SetaMinMaxProgressBarVazio(int min, int max)
        {
        }

        protected void SetaMensagemVazio(string mensagem)
        {
        }

        protected int ValorCorrenteProgress()
        {
            return valorcorrenteprogress;
        }

        protected void ReportProgress()
        {
            if (context != null)
            {
                jobRetorno.IdTask = context.BackgroundJob.Id;
                if ((DateTime.Now - ultimoReport).TotalSeconds > 1 || jobRetornoAnt == null || jobRetornoAnt.StatusNovo != jobRetorno.StatusNovo || jobRetornoAnt.Progress != jobRetorno.Progress)
                {
                    ultimoReport = DateTime.Now;
                    jobRetornoAnt = jobRetorno;
                    Task.Run(() => {
                        context.WriteLine("Progresso = " + JsonConvert.SerializeObject(jobRetorno));
                        NotificationHub.ReportarJobProgresso(jobRetorno);
                    });
                    
                }
            }
        }
        #endregion

        protected void VerificaCancelamentoJob(PerformContext context)
        {
            try
            {
                if (context != null)
                {
                    context.CancellationToken.ThrowIfCancellationRequested(); 
                }
            }
            catch (Exception)
            {
                JobControl jc = JobControlManager.GetJobControl(context.BackgroundJob.Id);
                NotificationHub.ReportarJobProgresso(new PxyJobReturn(jc));
                throw;
            }
        }

        protected object ExecuteMethodThredCancellation<T>(Func<T> funcToRun)
        {
            object retorno = null;
            var thread = new Thread(() => { retorno = funcToRun(); });
            thread.Start();

            while (!thread.Join(TimeSpan.FromSeconds(2)))
            {
                try
                {
                    VerificaCancelamentoJob(context);
                }
                catch (OperationCanceledException)
                {
                    thread.Abort();
                    throw;
                }
            }
            return retorno;
        }

        public object GetDataTableThred(Delegate method, params object[] args)
        {
            object retorno = new DataTable();
            var thread = new Thread(() => { retorno = method.DynamicInvoke(args); });
            thread.Start();

            while (!thread.Join(TimeSpan.FromSeconds(1)))
            {
                try
                {
                    VerificaCancelamentoJob(context);
                }
                catch (OperationCanceledException)
                {
                    thread.Abort();
                    throw;
                }
            }
            return retorno;
        }
    }
}
