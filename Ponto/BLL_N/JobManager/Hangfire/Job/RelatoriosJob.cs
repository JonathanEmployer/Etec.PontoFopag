using Hangfire.Server;
using BLL.Relatorios.V2;
using Modelo.Relatorios;
using Modelo.EntityFramework.MonitorPontofopag;
using System.Threading;

namespace BLL_N.JobManager.Hangfire.Job
{
    public class RelatoriosJob : JobBase
    {
        public RelatoriosJob() : base()
        {

        }

        public void GetRelatorioCartaoPonto(PerformContext context, JobControl jobReport, RelatorioCartaoPontoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioCartaoPontoPadraoBLL rel = new RelatorioCartaoPontoPadraoBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioCartaoPontoFechamento(PerformContext context, JobControl jobReport, RelatorioCartaoPontoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioCartaoPontoPadraoBLL rel = new RelatorioCartaoPontoPadraoBLL(relatorioFiltro, userPF, pb);

            string caminhoArquivo = rel.GetRelatorio();
        }

        public void GetRelatorioAbsenteismo(PerformContext context, JobControl jobReport, RelatorioAbsenteismoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioAbsenteismoBLL rel = new RelatorioAbsenteismoBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioAbono(PerformContext context, JobControl jobReport, RelatorioAbonoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioAbonoBLL rel = new RelatorioAbonoBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioFuncionario(PerformContext context, JobControl jobReport, RelatorioFuncionarioModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioFuncionarioBLL rel = new RelatorioFuncionarioBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioHistorico(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioHistoricoBLL rel = new RelatorioHistoricoBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioHorario(PerformContext context, JobControl jobReport, RelatorioHorarioModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioHorarioBLL rel = new RelatorioHorarioBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }


        public void GetRelatorioBilhetesImp(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioBilhetesImpBLL rel = new RelatorioBilhetesImpBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioLocalizacaoRegistroPonto(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioLocalizacaoRegistroPontoBLL rel = new RelatorioLocalizacaoRegistroPontoBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioHomemHora(PerformContext context, JobControl jobReport, RelatorioHomemHoraModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioHomemHoraBLL rel = new RelatorioHomemHoraBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

		public void GetRelatorioHorasTrabalhadasNoturnas(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
		{
			SetParametersBase(context, jobReport, db, usuario);
			RelatorioHorasTrabalhadasNoturnasBLL rel = new RelatorioHorasTrabalhadasNoturnasBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
			JobControlManager.UpdateFileDownload(context, caminhoArquivo);
		}

		public void GetRelatorioIntervalos(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
		{
			SetParametersBase(context, jobReport, db, usuario);
			RelatorioIntervalosBLL rel = new RelatorioIntervalosBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
			JobControlManager.UpdateFileDownload(context, caminhoArquivo);
		}

	    public void GetRelatorioAbsenteismoV2(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioAbsenteismoV2BLL rel = new RelatorioAbsenteismoV2BLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

	    public void GetRelatorioRegistros(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
		{
			SetParametersBase(context, jobReport, db, usuario);
			RelatorioRegistrosBLL rel = new RelatorioRegistrosBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
			JobControlManager.UpdateFileDownload(context, caminhoArquivo);
		}
        public void GetRelatorioSubstituicaoJornada(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioSubstituicaoJornadaBLL rel = new RelatorioSubstituicaoJornadaBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }
        public void GetRelatorioRefeicao(PerformContext context, JobControl jobReport, RelatorioRefeicaoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioRefeicaoBLL rel = new RelatorioRefeicaoBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioConclusoesBloqueioPnlRh(PerformContext context, JobControl jobReport, RelatorioConclusoesBloqueioPnlRhModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioConclusoesBloqueioPnlRhBLL rel = new RelatorioConclusoesBloqueioPnlRhBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
			JobControlManager.UpdateFileDownload(context, caminhoArquivo);
		}

        public void GetRelatorioBancoHoras(PerformContext context, JobControl jobReport, RelatorioBancoHorasModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioBancoHorasBLL rel = new RelatorioBancoHorasBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioFechamentoPercentualHE(PerformContext context, JobControl jobReport, RelatorioFechamentoPercentualHEModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioFechamentoPercentualHEBLL rel = new RelatorioFechamentoPercentualHEBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioEspelho(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
		{
			SetParametersBase(context, jobReport, db, usuario);
			RelatorioEspelhoBLL rel = new RelatorioEspelhoBLL(relatorioFiltro, userPF, pb);
			string caminhoArquivo = rel.GetRelatorio();
			JobControlManager.UpdateFileDownload(context, caminhoArquivo);
		}
		
		public void GetRelatorioPresenca(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
		{
			SetParametersBase(context, jobReport, db, usuario);
			RelatorioPresencaBLL rel = new RelatorioPresencaBLL(relatorioFiltro, userPF, pb);
			string caminhoArquivo = rel.GetRelatorio();
			JobControlManager.UpdateFileDownload(context, caminhoArquivo);
		}
		
        public void GetRelatorioHorasExtrasLocal(PerformContext context, JobControl jobReport, RelatorioHorasExtrasLocalModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioHorasExtrasLocalBLL rel = new RelatorioHorasExtrasLocalBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

		public void GetRelatorioInItinere(PerformContext context, JobControl jobReport, RelatorioHorasInItinereModel relatorioFiltro, string db, string usuario)
		{
			SetParametersBase(context, jobReport, db, usuario);
			RelatorioInItinereBLL rel = new RelatorioInItinereBLL(relatorioFiltro, userPF, pb);
			string caminhoArquivo = rel.GetRelatorio();
			JobControlManager.UpdateFileDownload(context, caminhoArquivo);
		}

        public void GetRelatorioAfastamentoOcorrencia(PerformContext context, JobControl jobReport, RelatorioAfastamentoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioAfastamentoOcorrenciaBLL rel = new RelatorioAfastamentoOcorrenciaBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioAfastamentoPeriodo(PerformContext context, JobControl jobReport, RelatorioAfastamentoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioAfastamentoPeriodoBLL rel = new RelatorioAfastamentoPeriodoBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioInconsistencias(PerformContext context, JobControl jobReport, RelatorioInconsistenciasModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioInconsistenciasBLL rel = new RelatorioInconsistenciasBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioHoraExtra(PerformContext context, JobControl jobReport, RelatorioHoraExtraModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioHoraExtraBLL rel = new RelatorioHoraExtraBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioManutencaoDiaria(PerformContext context, JobControl jobReport, RelatorioManutencaoDiariaModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioManutencaoDiariaBLL rel = new RelatorioManutencaoDiariaBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioClassificacaoHorasExtras(PerformContext context, JobControl jobReport, RelatorioClassificacaoHorasExtrasModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioClassificacaoHorasExtrasBLL rel = new RelatorioClassificacaoHorasExtrasBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }
		
        public void GetRelatorioCartaoPontoHTML(PerformContext context, JobControl jobReport, RelatorioCartaoPontoHTMLModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioCartaoPontoHTMLBLL rel = new RelatorioCartaoPontoHTMLBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioCartaoPontoExcecaoHTML(PerformContext context, JobControl jobReport, RelatorioCartaoPontoHTMLModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioCartaoPontoExcecaoHTMLBLL rel = new RelatorioCartaoPontoExcecaoHTMLBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }
        public void GetRelatorioCartaoPontoCustom(PerformContext context, JobControl jobReport, RelatorioCartaoPontoHTMLModel relatorioFiltro, string db, string usuario)
		{
			SetParametersBase(context, jobReport, db, usuario);
			RelatorioCartaoPontoCustomBLL rel = new RelatorioCartaoPontoCustomBLL(relatorioFiltro, userPF, pb);
			string caminhoArquivo = rel.GetRelatorio();
			JobControlManager.UpdateFileDownload(context, caminhoArquivo);
		}

		public void GetRelatorioOcorrencias(PerformContext context, JobControl jobReport, RelatorioOcorrenciasModel relatorioFiltro, string db, string usuario)
		{
			SetParametersBase(context, jobReport, db, usuario);
			RelatorioOcorrenciasBLL rel= new RelatorioOcorrenciasBLL(relatorioFiltro, userPF, pb);
			string caminhoArquivo = rel.GetRelatorio();
			JobControlManager.UpdateFileDownload(context, caminhoArquivo);
		}

        public void GetRelatorioGradeHorarioMovel(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioGradeHorarioMovelBLL rel = new RelatorioGradeHorarioMovelBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

		public void GetRelatorioBilhetesRep(PerformContext context, JobControl jobReport, RelatorioPadraoModel relatorioFiltro, string db, string usuario)
		{
			SetParametersBase(context, jobReport, db, usuario);
			RelatorioBilhetesRepV2BLL rel = new RelatorioBilhetesRepV2BLL(relatorioFiltro, userPF, pb);
			string caminhoArquivo = rel.GetRelatorio();
			JobControlManager.UpdateFileDownload(context, caminhoArquivo);
		}

        public void GetRelatorioTotalHoras(PerformContext context, JobControl jobReport, RelatorioTotalHoras relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioTotalHorasBLL rel = new RelatorioTotalHorasBLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public void GetRelatorioAFDPortaria373(PerformContext context, JobControl jobReport, RelatorioAfdPortaria373Model relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioAFDPortaria373BLL rel = new RelatorioAFDPortaria373BLL(relatorioFiltro, userPF, pb);
            string caminhoArquivo = rel.GetRelatorio();
            JobControlManager.UpdateFileDownload(context, caminhoArquivo);
        }

        public int GerarJobsTesteProcessamento(PerformContext context, int tempo)
        {
            Thread.Sleep(tempo*1000);
            return tempo;
        }
    }
}
