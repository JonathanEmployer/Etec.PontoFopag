using BLL_N.JobManager.Hangfire.Job;
using Hangfire;
using Modelo;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Proxy;
using System;
using System.Collections.Generic;

namespace BLL_N.JobManager.Hangfire
{
    public class HangfireManagerCalculos : HangfireManagerBase
    {
        public HangfireManagerCalculos(string dataBase) : base(dataBase)
        {
        }

        public HangfireManagerCalculos(string dataBase, string usuario, string hostAddress, string urlReferencia) : base(dataBase, usuario, hostAddress, urlReferencia)
        {
        }

        #region Calculos
        public PxyJobReturn RecalculaMarcacao(string nomeProcesso, string parametrosExibicao, List<PxyIdPeriodo> funcsPeriodo, bool considerarInativos = false)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.RecalculaMarcacao(null, jobControl, dataBase, usuarioLogado, funcsPeriodo, considerarInativos), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn RecalculaExclusaoMudHorario(string nomeProcesso, string parametrosExibicao, MudancaHorario objMudancaHorario)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.RecalculaExclusaoMudHorario(null, jobControl, dataBase, usuarioLogado, objMudancaHorario), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn RecalculaMarcacao(string nomeProcesso, string parametrosExibicao, int? pTipo, List<int> pIdsTipo, DateTime dataInicial, DateTime dataFinal)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.RecalculaMarcacao(null, jobControl, dataBase, usuarioLogado, pTipo, pIdsTipo, dataInicial, dataFinal), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn RecalculaMarcacao(string nomeProcesso, string parametrosExibicao, int? pTipo, int pIdTipo, DateTime dataInicial, DateTime dataFinal)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.RecalculaMarcacao(null, jobControl, dataBase, usuarioLogado, pTipo, pIdTipo, dataInicial, dataFinal), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn RecalculaMarcacao(string nomeProcesso, string parametrosExibicao, List<int> idsFuncionario, DateTime dataInicial, DateTime dataFinal, bool considerarInativos = false)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.RecalculaMarcacao(null, jobControl, dataBase, usuarioLogado, idsFuncionario, dataInicial, dataFinal, considerarInativos), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn RecalculaMarcacao(string nomeProcesso, string parametrosExibicao, List<PxyFuncionariosRecalcular> funcsRecalculo, bool considerarInativos = false)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.RecalculaMarcacao(null, jobControl, dataBase, usuarioLogado, funcsRecalculo, considerarInativos), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn RecalculaMarcacao(string nomeProcesso, string parametrosExibicao, List<PxyFuncionariosRecalcular> funcsRecalculo)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.RecalculaMarcacao(null, jobControl, dataBase, usuarioLogado, funcsRecalculo), _enqueuedStateNormal);                                                                       
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn RecalculaMarcacao(string nomeProcesso, string parametrosExibicao, List<int> idsFuncionario, DateTime dataInicial, DateTime dataFinal, DateTime dataInicial_Ant, DateTime dataFinal_Ant, bool considerarInativos = false)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.RecalculaMarcacao(null, jobControl, dataBase, usuarioLogado, idsFuncionario, dataInicial, dataFinal, dataInicial_Ant, dataFinal_Ant, considerarInativos), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn AtualizaMarcacaoJornadaAlternativa(string nomeProcesso, string parametrosExibicao, JornadaAlternativa jornada)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.AtualizaMarcacaoJornadaAlternativa(null, jobControl, dataBase, usuarioLogado, jornada), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn AtualizarMarcacoesFeriado(string nomeProcesso, string parametrosExibicao, Acao acao, Feriado feriado)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.AtualizarMarcacoesFeriado(null, jobControl, dataBase, usuarioLogado, acao, feriado), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }
        
        public PxyJobReturn AtualizaMarcacoesCompensacao(string nomeProcesso, string parametrosExibicao, Acao acao, Compensacao compensacao)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.AtualizaMarcacoesCompensacao(null, jobControl, dataBase, usuarioLogado, acao, compensacao), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn DesfazCompensacao(string nomeProcesso, string parametrosExibicao, int pIdCompensacao)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.DesfazCompensacao(null, jobControl, dataBase, usuarioLogado, pIdCompensacao), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }
        public PxyJobReturn FechaCompensacao(string nomeProcesso, string parametrosExibicao, int pIdCompensacao)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.FechaCompensacao(null, jobControl, dataBase, usuarioLogado, pIdCompensacao), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn CalculaAfastamento(string nomeProcesso, string parametrosExibicao, Modelo.Afastamento pAfastamento)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.CalculaAfastamento(null, jobControl, dataBase, usuarioLogado, pAfastamento), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }  
        
        public PxyJobReturn CalculaBancoHoras(string nomeProcesso, string parametrosExibicao, Modelo.Acao acao, Modelo.BancoHoras bancoHoras)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.CalculaBancoHoras(null, jobControl, dataBase, usuarioLogado, acao, bancoHoras), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn CalculaLancamentoCreditoDebito(string nomeProcesso, string parametrosExibicao, Modelo.InclusaoBanco inclusaoBanco)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.CalculaLancamentoCreditoDebito(null, jobControl, dataBase, usuarioLogado, inclusaoBanco), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }


        public PxyJobReturn CalculaParametro(string nomeProcesso, string parametrosExibicao, int idParametro, DateTime dataInicial, DateTime dataFinal)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.CalculaParametro(null, jobControl, dataBase, usuarioLogado, idParametro, dataInicial, dataFinal), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn CalculaMudancaHorario(string nomeProcesso, string parametrosExibicao, Modelo.MudancaHorario mudHorario)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.CalculaMudancaHorario(null, jobControl, dataBase, usuarioLogado, mudHorario), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn OrdenaMarcacao(string nomeProcesso, string parametrosExibicao, Modelo.Marcacao marcacao)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.OrdenaMarcacao(null, jobControl, dataBase, usuarioLogado, marcacao), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn OrdenaMarcacaoPeriodo(string nomeProcesso, string parametrosExibicao, int id, DateTime dataInicial, DateTime dataFinal)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.OrdenaMarcacaoPeriodo(null, jobControl, dataBase, usuarioLogado, id, dataInicial, dataFinal), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn ManutencaoBilhete(string nomeProcesso, string parametrosExibicao, Modelo.Marcacao objMarcacao, Modelo.BilhetesImp objBilheteImp, int tipoManutencao)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.ManutencaoBilhete(null, jobControl, dataBase, usuarioLogado, objMarcacao, objBilheteImp, tipoManutencao), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn LancamentoLoteBilhetesProcessar(string nomeProcesso, string parametrosExibicao, Modelo.LancamentoLote LLFunc)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.LancamentoLoteBilhetesProcessar(null, jobControl, dataBase, usuarioLogado, LLFunc), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn CalculaMarcacaoLancamentoLoteDiaJob(string nomeProcesso, string parametrosExibicao, Modelo.LancamentoLote LLFunc)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.CalculaMarcacaoLancamentoLoteDiaJob(null, jobControl, dataBase, usuarioLogado, LLFunc), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn RestauraFuncionarioJob(Modelo.Funcionario objfunc)
        {
            JobControl jobControl = GerarJobControl("Restaurar funcionário", String.Format("Restaurado o funcionário {0} | {1}", objfunc.Dscodigo, objfunc.Nome));
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.RestauraFuncionarioJob(null, jobControl, dataBase, usuarioLogado, objfunc), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn TransferirBilhetes(int idTransferenciaBilhetes)
        {
            Modelo.UsuarioPontoWeb user = new Modelo.UsuarioPontoWeb() { Login = usuarioLogado, ConnectionString = BLL.cwkFuncoes.ConstroiConexao(dataBase).ConnectionString };
            BLL.TransferenciaBilhetes bllTransferenciaBilhetes = new BLL.TransferenciaBilhetes(user.ConnectionString, user);
            Modelo.TransferenciaBilhetes transferenciaBilhetes = bllTransferenciaBilhetes.LoadObject(idTransferenciaBilhetes);
            string parametrosExibicao = string.Format("Transferindo {0} registro(s) de Ponto do dia {1} a {2} do registro de emprego {3} para {4}",
                                                        bllTransferenciaBilhetes.CountDetalhes(transferenciaBilhetes.Id),
                                                        transferenciaBilhetes.DataInicio.GetValueOrDefault().ToString("dd/MM/yyyy"),
                                                        transferenciaBilhetes.DataFim.GetValueOrDefault().ToString("dd/MM/yyyy"),
                                                        transferenciaBilhetes.FuncionarioOrigem,
                                                        transferenciaBilhetes.FuncionarioDestino
                                                        );

            JobControl jobControl = GerarJobControl("Transferência de Registro Ponto", parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.TransferirBilhetes(null, jobControl, dataBase, usuarioLogado, transferenciaBilhetes), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn FechamentoPonto(string nomeProcesso, string parametrosExibicao, FechamentoBH objFechamentoBH, BancoHoras objBancoHoras)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.FechamentoBH(null, jobControl, dataBase, usuarioLogado, objFechamentoBH, objBancoHoras), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public PxyJobReturn ExcluirFechamentoPonto(string nomeProcesso, string parametrosExibicao, FechamentoBH objFechamentoBH)
        {
            JobControl jobControl = GerarJobControl(nomeProcesso, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<CalculosJob>(x => x.ExcluirFechamentoBH(null, jobControl, dataBase, usuarioLogado, objFechamentoBH), _enqueuedStateNormal);
            PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }
        #endregion
    }
}
