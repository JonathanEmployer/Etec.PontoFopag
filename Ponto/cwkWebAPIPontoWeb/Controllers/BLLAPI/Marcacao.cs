using cwkWebAPIPontoWeb.Utils;
using Modelo;
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
    }
}