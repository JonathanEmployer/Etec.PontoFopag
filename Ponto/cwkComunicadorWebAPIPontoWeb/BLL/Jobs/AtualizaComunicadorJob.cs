using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cwkComunicadorWebAPIPontoWeb.BLL.Jobs
{
    public class AtualizaComunicadorJob: IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {

                JobDataMap map = context.JobDetail.JobDataMap;
                Progress<ReportaErro> progress = (Progress<ReportaErro>)map.Get("progress");
                Form formAtualizacao = (Form)map.Get("form");
                ReportarProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job - Atualização) Definindo dados para atualização do sistema: " + context.JobDetail.Description }, progress);

                AtualizarAplicativo AtualizarApp = new AtualizarAplicativo(progress);
                String versaoAtual, versaoFTP;
                ReportarProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job - Atualização) Verificando arquivos para atualização: " + context.JobDetail.Description }, progress);
                List<ArquivosAtualizacaoViewModel> lArquivosAtualizar = AtualizarApp.VerificaAtualizacao(out versaoAtual, out versaoFTP);

                ReportarProgresso(new ReportaErro() { TipoMsg = TipoMensagem.Info, Mensagem = "(Job - Atualização) Número de arquivos para atualização: " + lArquivosAtualizar.Count.ToString() + " (Job)" + context.JobDetail.Description }, progress);
                if (lArquivosAtualizar.Count > 0)
                {
                    Task.Factory.StartNew(() => { formAtualizacao.ShowDialog(); }).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                CwkUtils.LogarExceptions("Log de Atualização do Pontofopag Comunicador", e, CwkUtils.FileLogStringUtil());
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
