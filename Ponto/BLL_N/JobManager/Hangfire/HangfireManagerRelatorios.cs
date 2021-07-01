using BLL_N.JobManager.Hangfire.Job;
using Hangfire;
using Hangfire.States;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Relatorios;
using System;
using System.Linq;

namespace BLL_N.JobManager.Hangfire
{
    public class HangfireManagerRelatorios : HangfireManagerBase
    {

        public HangfireManagerRelatorios(string dataBase) : base(dataBase)
        {
        }

        public HangfireManagerRelatorios(string dataBase, string usuario, string hostAddress, string urlReferencia) : base(dataBase, usuario, hostAddress, urlReferencia)
        {
        }

        public Modelo.Proxy.PxyJobReturn RelatorioCartaoPonto(IRelatorioModel parametros)
        {
            JobControl jobControl = GerarJobControl("Relatório Cartão Ponto", GetDescricaoParametrosJob(parametros));
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioCartaoPonto(null, jobControl, (RelatorioCartaoPontoModel)parametros, dataBase, usuarioLogado), _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public void RelatorioCartaoPontoFechamento(IRelatorioModel parametros)
        {
            JobControl jobControl = GerarJobControl("Relatório Cartão Ponto Fechamento", GetDescricaoParametrosJob(parametros));
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioCartaoPontoFechamento(null, jobControl, (RelatorioCartaoPontoModel)parametros, dataBase, usuarioLogado), _enqueuedStateNormal);
        }

        public Modelo.Proxy.PxyJobReturn RelatorioAbsenteismo(IRelatorioModel parametros)
        {
            string descricaoParametros = GetDescricaoParametrosJob(parametros);
            JobControl jobControl = GerarJobControl("Relatório Absenteísmo", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioAbsenteismo(null, jobControl, (RelatorioAbsenteismoModel)parametros, dataBase, usuarioLogado), _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioAbono(IRelatorioModel parametros)
        {
            RelatorioAbonoModel parms = (RelatorioAbonoModel)parametros;
            var descricaoParametros = GetDescricaoParametrosJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório Abono", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioAbono(null, 
                                                                                                    jobControl, 
                                                                                                    (RelatorioAbonoModel)parametros, 
                                                                                                    dataBase, 
                                                                                                    usuarioLogado),
                                                                                                    _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioFuncionario(IRelatorioModel parametros)
        {
			var descricaoParametros = GetDescricaoParametrosFun(parametros);


			JobControl jobControl = GerarJobControl("Relatório de Funcionários", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioFuncionario  (null,
                                                                                                            jobControl,
                                                                                                            (RelatorioFuncionarioModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioHistorico(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);
            JobControl jobControl = GerarJobControl("Relatório de Histórico Funcionário", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioHistorico(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioPadraoModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioHorario(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);
            JobControl jobControl = GerarJobControl("Relatório de Horario", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioHorario(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioHorarioModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioBilhetesImp(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório Bilhetes Importados", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioBilhetesImp(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioPadraoModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioLocalizacaoRegistroPonto(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);
            JobControl jobControl = GerarJobControl("Relatório de Localização de Registro Ponto", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioLocalizacaoRegistroPonto(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioPadraoModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioHomemHora(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);
        
            JobControl jobControl = GerarJobControl("Relatório Homem Hora", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioHomemHora(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioHomemHoraModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

		public Modelo.Proxy.PxyJobReturn RelHorasTrabalhadasNoturnas(IRelatorioModel parametros)
		{
			var descricaoParametros = GetDescricaoParametrosJob(parametros);

			JobControl jobControl = GerarJobControl("Relatório Horas Trabalhadas Noturnas", descricaoParametros);
			jobControl.PermiteCancelar = true;
			string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioHorasTrabalhadasNoturnas(null,
																											jobControl,
																											(RelatorioPadraoModel)parametros,
																											dataBase,
																											usuarioLogado),
																											_enqueuedStateNormal);
			Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
			return jobReturn;
		}
		
		public Modelo.Proxy.PxyJobReturn RelIntervalos(IRelatorioModel parametros)
		{
			var descricaoParametros = GetDescricaoParametrosJob(parametros);

			JobControl jobControl = GerarJobControl("Relatório de Intervalos", descricaoParametros);
			jobControl.PermiteCancelar = true;
			string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioIntervalos(null,
																											jobControl,
																											(RelatorioPadraoModel)parametros,
																											dataBase,
																											usuarioLogado),
																											_enqueuedStateNormal);
			Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
			return jobReturn;
		}
		
        public Modelo.Proxy.PxyJobReturn RelatorioAbsenteismoV2(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório Absenteísmo Modelo 2", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioAbsenteismoV2(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioPadraoModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioSubstituicaoJornada(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório de Substituição de Jornada", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioSubstituicaoJornada(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioPadraoModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioRegistros(IRelatorioModel parametros)
		{
			var descricaoParametros = GetDescricaoParametrosJob(parametros);

			JobControl jobControl = GerarJobControl("Relatório de Registros", descricaoParametros);
			jobControl.PermiteCancelar = true;
			string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioRegistros(null,
																											jobControl,
																											(RelatorioPadraoModel)parametros,
																											dataBase,
																											usuarioLogado),
																											_enqueuedStateNormal);
			Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
			return jobReturn;
		}

        public Modelo.Proxy.PxyJobReturn RelatorioRefeicao(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório Refeições", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioRefeicao(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioRefeicaoModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioConclusoesBloqueioPnlRh(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório Conclusoes/Bloqueio Painel RH", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioConclusoesBloqueioPnlRh(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioConclusoesBloqueioPnlRhModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioBancoHoras(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório Banco de Horas", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioBancoHoras(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioBancoHorasModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }
	
        public Modelo.Proxy.PxyJobReturn RelatorioFechamentoPercentualHE(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório Fechamento Percentual de Horas Extras", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioFechamentoPercentualHE(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioFechamentoPercentualHEModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }
	    public Modelo.Proxy.PxyJobReturn RelatorioEspelho(IRelatorioModel parametros)
		{
			var descricaoParametros = GetDescricaoParametrosJob(parametros);

			JobControl jobControl = GerarJobControl("Relatório de Espelho", descricaoParametros);
			jobControl.PermiteCancelar = true;
			string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioEspelho(null,
																											jobControl,
																											(RelatorioPadraoModel)parametros,
																											dataBase,
																											usuarioLogado),
																											_enqueuedStateNormal);
			Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
			return jobReturn;
		}

		public Modelo.Proxy.PxyJobReturn RelatorioPresenca(IRelatorioModel parametros)
		{
			var descricaoParametros = GetDescricaoParametrosDataJob(parametros);

			JobControl jobControl = GerarJobControl("Relatório de Funcionário por Presença", descricaoParametros);
			jobControl.PermiteCancelar = true;
			string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioPresenca(null,
																											jobControl,
																											(RelatorioPadraoModel)parametros,
																											dataBase,
																											usuarioLogado),
																											_enqueuedStateNormal);
			Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
			return jobReturn;
		}
		
 public Modelo.Proxy.PxyJobReturn RelatorioHorasExtrasLocal(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosDataJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório de Horas Extras - Local ", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioHorasExtrasLocal(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioHorasExtrasLocalModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

			public Modelo.Proxy.PxyJobReturn RelatorioInItinere(IRelatorioModel parametros)
		{
			var descricaoParametros = GetDescricaoParametrosJob(parametros);

			JobControl jobControl = GerarJobControl("Relatório de Horas In Itinere", descricaoParametros);
			jobControl.PermiteCancelar = true;
			string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioInItinere(null,
																											jobControl,
																											(RelatorioHorasInItinereModel)parametros,
																											dataBase,
																											usuarioLogado),
																											_enqueuedStateNormal);
			Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
			return jobReturn;
		}

        public Modelo.Proxy.PxyJobReturn RelatorioAfastamentoOcorrencia(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosDataJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório de Afastamento por Ocorrência", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioAfastamentoOcorrencia(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioAfastamentoModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }
        public Modelo.Proxy.PxyJobReturn RelatorioAfastamentoPeriodo(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório de Afastamento por Ocorrência", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioAfastamentoPeriodo(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioAfastamentoModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioInconsistencias(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosDataJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório de Inconsistências", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioInconsistencias(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioInconsistenciasModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioHoraExtra(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosDataJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório de Hora Extra", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioHoraExtra(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioHoraExtraModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioManutencaoDiaria(IRelatorioModel parametros)
        {
            var par = (RelatorioManutencaoDiariaModel)parametros;
            var qtd = par.IdSelecionados.Split(',').ToList().Count();
            var descricaoParametros = String.Format("Período {0} a {1} e {2}", par.InicioPeriodo.ToShortDateString(), par.FimPeriodo.ToShortDateString(), (par.TipoSelecao == 0 ? (qtd + " Empresas ") : (qtd + " Departamentos ")));

            JobControl jobControl = GerarJobControl("Relatório de Manutenção Diária", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioManutencaoDiaria(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioManutencaoDiariaModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioClassificacaoHorasExtras(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosDataJob(parametros);

            JobControl jobControl = GerarJobControl("Relatório de Classificação de Horas Extras", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioClassificacaoHorasExtras(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioClassificacaoHorasExtrasModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioCartaoPontoExcecaoHTML(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);
            JobControl jobControl = GerarJobControl("Relatório de Cartão Ponto por Exceção", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioCartaoPontoExcecaoHTML(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioCartaoPontoHTMLModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }
        public Modelo.Proxy.PxyJobReturn RelatorioCartaoPontoHTML(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);
            JobControl jobControl = GerarJobControl("Relatório de Cartão Ponto - Modelo 2", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioCartaoPontoHTML(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioCartaoPontoHTMLModel)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

		public Modelo.Proxy.PxyJobReturn RelatorioCartaoPontoCustom(IRelatorioModel parametros)
		{
			var descricaoParametros = GetDescricaoParametrosJob(parametros);
			JobControl jobControl = GerarJobControl("Relatório de Cartão Ponto - Customizável", descricaoParametros);
			jobControl.PermiteCancelar = true;
			string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioCartaoPontoCustom(null,
																											jobControl,
																											(RelatorioCartaoPontoHTMLModel)parametros,
																											dataBase,
																											usuarioLogado),
																											_enqueuedStateNormal);
			Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
			return jobReturn;
		}

		public Modelo.Proxy.PxyJobReturn RelatorioOcorrencias(IRelatorioModel parametros)
		{
			var descricaoParametros = GetDescricaoParametrosDataJob(parametros);

			JobControl jobControl = GerarJobControl("Relatório de Ocorrências", descricaoParametros);
			jobControl.PermiteCancelar = true;
			string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioOcorrencias(null,
																											jobControl,
																											(RelatorioOcorrenciasModel)parametros,
																											dataBase,
																											usuarioLogado),
																											_enqueuedStateNormal);
			Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
			return jobReturn;
		}

        public Modelo.Proxy.PxyJobReturn RelatorioGradeHorarioMovel(IRelatorioModel parametros)
        {
            RelatorioPadraoModel parms = (RelatorioPadraoModel)parametros;
            BLL.Horario bllHorario = new BLL.Horario(BLL.cwkFuncoes.ConstroiConexao(dataBase).ConnectionString);
            Modelo.Horario horario = bllHorario.LoadObject(Convert.ToInt32(parms.IdSelecionados.Split(',')[0]));
            var descricaoParametros = String.Format("Período {0} a {1} e horário {2}", parms.InicioPeriodo.ToShortDateString(), parms.FimPeriodo.ToShortDateString(), horario.Codigo + " | " + horario.Descricao);

            JobControl jobControl = GerarJobControl("Relatório de Grade Horária Flexível", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioGradeHorarioMovel(null,
                                                                                                            jobControl,
                                                                                                            parms,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

		public Modelo.Proxy.PxyJobReturn RelatorioBilhetesRep(IRelatorioModel parametros)
		{
			RelatorioPadraoModel parms = (RelatorioPadraoModel)parametros;
			BLL.REP bllRep = new BLL.REP(BLL.cwkFuncoes.ConstroiConexao(dataBase).ConnectionString);
			Modelo.REP Rep = bllRep.LoadObject(Convert.ToInt32(parms.IdSelecionados.Split(',')[0]));
			var descricaoParametros = String.Format("Período {0} a {1} e Núm. Relógio {2}", parms.InicioPeriodo.ToShortDateString(), parms.FimPeriodo.ToShortDateString(), Rep.Codigo);

			JobControl jobControl = GerarJobControl("Relatório de Bilhetes", descricaoParametros);
			jobControl.PermiteCancelar = true;
			string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioBilhetesRep(null,
																											jobControl,
																											parms,
																											dataBase,
																											usuarioLogado),
																											_enqueuedStateNormal);
			Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
			return jobReturn;
		}

        public Modelo.Proxy.PxyJobReturn RelatorioTotalHoras(IRelatorioModel parametros)
        {
            var descricaoParametros = GetDescricaoParametrosJob(parametros);
            JobControl jobControl = GerarJobControl("Relatório Total de Horas", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioTotalHoras(null,
                                                                                                            jobControl,
                                                                                                            (RelatorioTotalHoras)parametros,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn RelatorioAFDPortaria373(IRelatorioModel parametros)
        {
            RelatorioAfdPortaria373Model parms = (RelatorioAfdPortaria373Model)parametros;
            var descricaoParametros = String.Format("Período {0} a {1}, tipo {2}", parms.InicioPeriodo.ToShortDateString(), parms.FimPeriodo.ToShortDateString(), (parms.TipoArquivo == "PDF" ? "Txt" : parms.TipoArquivo));

            JobControl jobControl = GerarJobControl("Relatório Bilhetes Importados", descricaoParametros);
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GetRelatorioAFDPortaria373(null,
                                                                                                            jobControl,
                                                                                                            parms,
                                                                                                            dataBase,
                                                                                                            usuarioLogado),
                                                                                                            _enqueuedStateNormal);
            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public string GerarJobsTesteProcessamento(int tempo, string queued)
        {
            string idJob = new BackgroundJobClient().Create<RelatoriosJob>(x => x.GerarJobsTesteProcessamento(null, tempo), new EnqueuedState(queued));
            return idJob;
        }

        #region Metodos

        public string GetDescricaoParametrosFun(IRelatorioModel parametros)
		{
			RelatorioFuncionarioModel parms = (RelatorioFuncionarioModel)parametros;
			string var = string.Empty;

			switch (parms.Relatorio.ToString())
			{
				case "1":
					var = String.Format("Letra Inicial {0} até {1}, {2} funcionários {3}", parms.LetrasIniciais, parms.LetrasFinais, parms.IdSelecionados.Split(',').ToList().Count(), string.IsNullOrEmpty(parms.TipoArquivo) ? "" : String.Format("({0})", parms.TipoArquivo));
					break;

				case "2":
					var = String.Format("Código inicial de {0} até {1}, {2} funcionários {3}", parms.CodigoInicial, parms.CodigoFinal, parms.IdSelecionados.Split(',').ToList().Count(), string.IsNullOrEmpty(parms.TipoArquivo) ? "" : String.Format("({0})", parms.TipoArquivo));
					break;

				case "3":
					var = String.Format("Por Departamento. {0} Funcionários {1}", parms.IdSelecionados.Split(',').ToList().Count(), string.IsNullOrEmpty(parms.TipoArquivo) ? "" : String.Format("({0})", parms.TipoArquivo));
					break;

				case "4":
					var = String.Format("Por Empresa. {0} Funcionários {1}", parms.IdSelecionados.Split(',').ToList().Count(), string.IsNullOrEmpty(parms.TipoArquivo) ? "" : String.Format("({0})", parms.TipoArquivo));
					break;

				case "5":
					var = String.Format("Por Horario. {0} Funcionários {1}", parms.IdSelecionados.Split(',').ToList().Count(), string.IsNullOrEmpty(parms.TipoArquivo) ? "" : String.Format("({0})", parms.TipoArquivo));
					break;

				case "6":
					string Fun = parms.AtivoInativo == 0 ? Fun = "ativos": Fun = "inativos";			
					var = String.Format("{1} Funcionários {0} {2}", Fun, parms.IdSelecionados.Split(',').ToList().Count(), string.IsNullOrEmpty(parms.TipoArquivo) ? "" : String.Format("({0})", parms.TipoArquivo));
					break;

				case "7":
					var = String.Format("Período {0} a {1} de {2} funcionários {3}", parms.InicioPeriodo.ToShortDateString(), parms.FimPeriodo.ToShortDateString(), parms.IdSelecionados.Split(',').ToList().Count(), string.IsNullOrEmpty(parms.TipoArquivo) ? "" : String.Format("({0})", parms.TipoArquivo));
					break;

				case "8":
					var = String.Format("Período {0} a {1} de {2} funcionários {3}", parms.InicioPeriodo.ToShortDateString(), parms.FimPeriodo.ToShortDateString(), parms.IdSelecionados.Split(',').ToList().Count(), string.IsNullOrEmpty(parms.TipoArquivo) ? "" : String.Format("({0})", parms.TipoArquivo));
					break;

			}

			return var;

		}
		
		#endregion
	}
}
