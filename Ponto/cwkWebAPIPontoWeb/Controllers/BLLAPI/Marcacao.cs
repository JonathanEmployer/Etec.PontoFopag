using cwkWebAPIPontoWeb.Utils;
using Hangfire;
using Hangfire.States;
using Modelo;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace cwkWebAPIPontoWeb.Controllers.BLLAPI
{
    public class Marcacao
    {
        public static void ThreadRecalculaMarcacao(BilhetesImp objBilhete, int idFunc, DateTime? dataInicial, DateTime? dataFinal, List<string> log, Modelo.ProgressBar pb, string conexao)
        {
            Thread m;
            Action met = () => RecalculaMarcacao(objBilhete, idFunc, dataInicial, dataFinal, log, pb, conexao);
            m = new Thread(new ThreadStart(met));
            m.Start();
        }

        private static void RecalculaMarcacao(Modelo.BilhetesImp objBilhete, int idFunc, DateTime? dataInicial, DateTime? dataFinal, List<string> log, Modelo.ProgressBar pb, string conexao)
        {
            try
            {
                BLL.ImportaBilhetes bllImportaBilhetes = new BLL.ImportaBilhetes(conexao);
                if (bllImportaBilhetes.ImportarBilhetes(objBilhete.Func, false, objBilhete.Data, objBilhete.Data, out dataInicial, out dataFinal, pb, log))
                {
                    BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(2, idFunc, dataInicial.Value.AddDays(-1), dataFinal.Value.AddDays(1), pb, false, conexao, true, false);
                    bllCalculaMarcacao.CalculaMarcacoes();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método Responsável por recalcular as marcações de um determinado período
        /// </summary>
        /// <param name="pTipo">null = geral; 0 = empresa; 1 = departamento; 2 = funcionário; 3 = função; 4 = horário</param>
        /// <param name="pIdTipo">Identificação do tipo</param>
        /// <param name="pDataInicial">Data Inicial</param>
        /// <param name="pDataFinal">Data Final</param>
        public static void ThreadRecalculaMarcacao(int? pTipo, int pIdTipo, DateTime pDataInicial, DateTime pDataFinal, Modelo.ProgressBar pProgressBar, string conexao)
        {
            Thread m;
            Action met = () => RecalculaMarcacao(pTipo, pIdTipo, pDataInicial, pDataFinal, pProgressBar, conexao);
            m = new Thread(new ThreadStart(met));
            m.Start();
        }
        private static void RecalculaMarcacao(int? pTipo, int pIdTipo, DateTime pDataInicial, DateTime pDataFinal, Modelo.ProgressBar pProgressBar, string conexao)
        {
            try
            {
                BLL.Marcacao bllMarcacao = new BLL.Marcacao(conexao);
                bllMarcacao.RecalculaMarcacao(pTipo, pIdTipo, pDataInicial, pDataFinal, pProgressBar);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Método Responsável por recalcular as marcações de um determinado período
        /// </summary>
        /// <param name="pTipo">null = geral; 0 = empresa; 1 = departamento; 2 = funcionário; 3 = função; 4 = horário</param>
        /// <param name="pIdTipo">Lista de IDs referente ao tipo</param>
        /// <param name="pDataInicial">Data Inicial</param>
        /// <param name="pDataFinal">Data Final</param>
        public static void ThreadRecalculaMarcacaoList(List<int> idsFuncs, DateTime pDataInicial, DateTime pDataFinal, Modelo.ProgressBar pProgressBar, string conexao, Modelo.Cw_Usuario usuario)
        {
            Thread m;
            Action met = () => RecalculaMarcacaoList(idsFuncs, pDataInicial, pDataFinal, pProgressBar, conexao, usuario);
            m = new Thread(new ThreadStart(met));
            m.Start();
        }
        private static void RecalculaMarcacaoList(List<int> idsFuncs, DateTime pDataInicial, DateTime pDataFinal, Modelo.ProgressBar pProgressBar, string conexao, Modelo.Cw_Usuario usuario)
        {
            try
            {
                BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(idsFuncs, pDataInicial, pDataFinal, pProgressBar, false, conexao, usuario);
                bllCalculaMarcacao.CalculaMarcacoes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calcula Funcionários por períodos distintos em Thred
        /// </summary>
        /// <param name="funcsRecalculo">Lista dos funcionários com as datas</param>
        /// <param name="user">usuário logado</param>
        /// <param name="conexao">connection string</param>
        /// <param name="pb">Obj progress, pode ser informado nulo</param>
        public static void ThreadRecalculaMarcacaoFuncionariosPeriodo(List<PxyFuncionariosRecalcular> funcsRecalculo, Modelo.Cw_Usuario user, string conexao, Modelo.ProgressBar pb)
        {
            Thread m;
            Action met = () => BLL_N.JobManager.CalculoMarcacoes.Recalcular(funcsRecalculo, user, conexao, pb);
            m = new Thread(new ThreadStart(met));
            m.Start();
        }

        public static void FilaRecalculaMarcacaoFuncionariosPeriodo(List<PxyFuncionariosRecalcular> funcsRecalculo, Modelo.Cw_Usuario user, string conexao, Modelo.ProgressBar pb)
        {
            var client = new BackgroundJobClient();
            #if DEBUG
                    EnqueuedState state = new EnqueuedState(BLL.cwkFuncoes.RemoveAcentosECaracteresEspeciais(Environment.MachineName.ToLower()));
            #else
                   EnqueuedState state = new EnqueuedState("normal");
            #endif
            var jobId = client.Create(() => BLL_N.JobManager.CalculoMarcacoes.Recalcular(funcsRecalculo, user, conexao, pb), state);
        }

        public static void RecalcularAfastamento(Afastamento afastamento, UsuarioPontoWeb user)
        {
            var funcsCalcular = new List<PxyFuncionariosRecalcular>();
            DateTime dtIni = afastamento.Datai.GetValueOrDefault();
            if (afastamento.Datai_Ant < afastamento.Datai && afastamento.Datai_Ant != null)
            {
                dtIni = afastamento.Datai_Ant.GetValueOrDefault();
            }

            DateTime? dtFim = afastamento.Dataf.GetValueOrDefault();
            if ((afastamento.Dataf_Ant > afastamento.Dataf && afastamento.Dataf_Ant != null) || (afastamento.Datai_Ant != null && afastamento.Dataf_Ant == null))
            {
                dtFim = afastamento.Dataf_Ant;
            }

            funcsCalcular.Add(new PxyFuncionariosRecalcular() { IdFuncionario = afastamento.IdFuncionario, DataInicio = dtIni, DataFim = dtFim });
            BLLAPI.Marcacao.FilaRecalculaMarcacaoFuncionariosPeriodo(funcsCalcular, user, user.ConnectionString, new ProgressBar());
        }
    }
}