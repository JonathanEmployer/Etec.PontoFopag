﻿using BLL_N.Hubs;
using DAL.SQL;
using Hangfire.Console;
using Hangfire.Server;
using Modelo;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Proxy;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BLL_N.JobManager
{
    public class CalculoMarcacoes
    {
        public static void CalculaMarcacoes(string db, string usuario, List<int> idsFuncionario, DateTime dataInicial, DateTime dataFinal)
        {
            dataInicial = dataInicial.Date;
            dataFinal = dataFinal.Date;
            string conexao = BLL.cwkFuncoes.ConstroiConexao(db).ConnectionString;
            Modelo.Cw_Usuario userPF = new Modelo.Cw_Usuario();
            userPF.Login = usuario;
            CalculaMarcacoes(idsFuncionario, dataInicial, dataFinal, conexao, userPF, new Modelo.ProgressBar());
        }

        public static void CalculaMarcacoes(List<int> idsFuncionario, DateTime dataInicial, DateTime dataFinal, string conexao, Modelo.Cw_Usuario userPF, Modelo.ProgressBar pb)
        {
            int qtdSteps = 7;
            int passo = 1;

            if (pb.incrementaPB == null)
            {
                pb.incrementaPB = new Modelo.IncrementaProgressBar(SetaValorProgressBar);
                pb.setaMensagem = new Modelo.SetaMensagem(SetaMensagem);
                pb.setaMinMaxPB = new Modelo.SetaMinMaxProgressBar(SetaMinMaxProgressBar);
                pb.setaValorPB = new Modelo.SetaValorProgressBar(SetaValorProgressBar);
            }

            pb.setaMinMaxPB(0, qtdSteps);

            Modelo.ProgressBar pbInt = new Modelo.ProgressBar();
            pbInt.incrementaPB = new Modelo.IncrementaProgressBar(SetaValorProgressBar);
            pbInt.setaMensagem = new Modelo.SetaMensagem(SetaMensagem);
            pbInt.setaMinMaxPB = new Modelo.SetaMinMaxProgressBar(SetaMinMaxProgressBar);
            pbInt.setaValorPB = new Modelo.SetaValorProgressBar(SetaValorProgressBar);

            pb.setaMensagem(String.Format("({0}/{1}) Carregando dados para calculo", passo, qtdSteps));
            BLL.JornadaAlternativa bllJornadaAlternativa = new BLL.JornadaAlternativa(conexao, userPF);
            Hashtable jornadaAlternativaList = bllJornadaAlternativa.GetHashIdObjeto(dataInicial, dataFinal, 2, idsFuncionario);

            BLL.FechamentoBHD bllFechamentoBHD = new BLL.FechamentoBHD(conexao, userPF);
            List<Modelo.FechamentoBHD> fechamentoBHDList = bllFechamentoBHD.getPorListaFuncionario(idsFuncionario);
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(conexao, userPF);
            Hashtable ocorrenciaList = bllOcorrencia.GetHashIdDescricao();
            BLL.Compensacao bllCompensacao = new BLL.Compensacao(conexao, userPF);
            List<Modelo.Compensacao> compensacaoList = bllCompensacao.GetPeriodo(dataInicial, dataFinal, 2, idsFuncionario);
            DataTable dtMarcacoes = GetMarcacoesCalculo(idsFuncionario, dataInicial, dataFinal, conexao, userPF);

            BLL.BilhetesImp bllBilhetesImp = new BLL.BilhetesImp(conexao, userPF);
            List<Modelo.BilhetesImp> tratamentomarcacaoList = bllBilhetesImp.GetImportadosPeriodo(idsFuncionario, dataInicial, dataFinal, false);
            List<int> idsBH = dtMarcacoes.AsEnumerable().Where(r => !r.IsNull("idbancohoras")).Select(s => s.Field<int>("idbancohoras")).Distinct().ToList();
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(conexao, userPF);
            Hashtable bancoHorasList = bllBancoHoras.GetHashIdObjeto(dataInicial, dataFinal, idsBH);
            pb.incrementaPB(passo++);
            pb.setaMensagem(String.Format("({0}/{1}) Separando dados para calculo", passo, qtdSteps));
            ConcurrentBag<LoteMarcacaoProcessar> lote = GerarLoteParaCalculo(dtMarcacoes);

            pb.incrementaPB(passo++);
            int qtdLote = lote.Count();
            int stepCalc = 0;
            Parallel.ForEach(lote,
                new ParallelOptions { MaxDegreeOfParallelism = 4 },
                (currentFile) =>
                {
                    stepCalc++;
                    pb.setaMensagem(String.Format("({0}/{1}) Calculando dados (Funcionário(s) {2}/{3})", passo, qtdSteps, stepCalc, qtdLote));
                    BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(currentFile.DtMarcacoes, tratamentomarcacaoList, bancoHorasList, jornadaAlternativaList, fechamentoBHDList, ocorrenciaList, compensacaoList, pbInt, conexao, userPF);
                    LoteMarcacaoProcessar lt = bllCalculaMarcacao.CalcularMarcacoes();
                    currentFile.Marcacoes = lt.Marcacoes;
                    currentFile.Bilhetes = lt.Bilhetes;
                });

            pb.incrementaPB(passo++);
            string msgSalvando = String.Format("({0}/{1}) Salvando dados calculados", passo, qtdSteps);
            pb.setaMensagem(msgSalvando);
            BLL.CalculaMarcacao bllCalculaMarcacaoAnt = new BLL.CalculaMarcacao(dtMarcacoes, tratamentomarcacaoList, bancoHorasList, jornadaAlternativaList, fechamentoBHDList, ocorrenciaList, compensacaoList, pb, conexao, userPF);
            bllCalculaMarcacaoAnt.SalvarMarcacoesCalculadas(lote.ToList(), pb, msgSalvando);

            pb.incrementaPB(passo++);
            pb.setaMensagem(String.Format("({0}/{1}) Pré classificando horas", passo, qtdSteps));
            bllCalculaMarcacaoAnt.PreClassificarHorasExtras(lote.ToList());

            pb.incrementaPB(passo++);
            qtdLote = lote.Count();
            stepCalc = 1;
            Parallel.ForEach(lote, (currentFile) =>
            {
                if (currentFile.DtMarcacoes.Rows.Count > 0)
                {
                    pb.setaMensagem(String.Format("({0}/{1}) Calculando DSR (Quantidade {2}/{3})", passo, qtdSteps, stepCalc++, qtdLote));
                    BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(currentFile.DtMarcacoes, tratamentomarcacaoList, bancoHorasList, jornadaAlternativaList, fechamentoBHDList, ocorrenciaList, compensacaoList, pbInt, conexao, userPF);
                    List<Modelo.Marcacao> lt = bllCalculaMarcacao.CalculaDSR(false, false);
                    currentFile.Marcacoes = lt;
                    currentFile.Bilhetes = new List<Modelo.BilhetesImp>();
                }
            });
            pb.incrementaPB(passo++);
            pb.setaMensagem(String.Format("({0}/{1}) Salvando DSR", passo, qtdSteps));
            bllCalculaMarcacaoAnt.SalvarMarcacoesCalculadas(lote.ToList(), pb, msgSalvando);
            pb.incrementaPB(passo++);
        }

        private static ConcurrentBag<LoteMarcacaoProcessar> GerarLoteParaCalculo(DataTable dtMarcacoes)
        {
            var MarcsFuncs = dtMarcacoes.AsEnumerable().GroupBy(row => new
            {
                idfuncionario = row.Field<int>("idfuncionario"),
                dscodigo = row.Field<string>("dscodigo"),
                nomeFunc = row.Field<string>("nomeFuncionario"),
            }).OrderBy(o => o.Key.nomeFunc);
            ConcurrentBag<LoteMarcacaoProcessar> lote = new ConcurrentBag<LoteMarcacaoProcessar>();
            foreach (var group in MarcsFuncs)
            {
                LoteMarcacaoProcessar l = new LoteMarcacaoProcessar();
                l.IdFuncionario = group.Key.idfuncionario;
                l.NomeFuncionario = group.Key.nomeFunc;
                l.DtMarcacoes = group.CopyToDataTable();
                l.Marcacoes = new List<Modelo.Marcacao>();
                l.Bilhetes = new List<Modelo.BilhetesImp>();
                lote.Add(l);
            }

            return lote;
        }

        private static DataTable GetMarcacoesCalculo(List<int> idsFuncionario, DateTime dataInicial, DateTime dataFinal, string conexao, Modelo.Cw_Usuario userPF)
        {
            CalculaMarcacao dalCalculaMarcacao = new CalculaMarcacao(new DataBase(conexao))
            {
                UsuarioLogado = userPF
            };
            DataTable dtMarcacoes = dalCalculaMarcacao.GetMarcacoesCalculo(idsFuncionario, dataInicial, dataFinal, false, false);
            BLL.HorarioDinamico bllHorarioDinamico = new BLL.HorarioDinamico(conexao, userPF);
            if (bllHorarioDinamico.GerarHorariosDetalhesAPartirMarcacoes(dtMarcacoes))
            {
                dtMarcacoes = dalCalculaMarcacao.GetMarcacoesCalculo(idsFuncionario, dataInicial, dataFinal, false, false);
            }

            return dtMarcacoes;
        }

        #region Métodos para progress
        private static void SetaValorProgressBar(int valor)
        {

        }

        private static void SetaMinMaxProgressBar(int min, int max)
        {

        }

        private static void SetaMensagem(string mensagem)
        {

        }

        private static void IncrementaProgressBar(int incremento)
        {

        }

        public void QueueCalculo(PerformContext context, JobControl jobReport)
        {
            PxyJobReturn jobRtetorno = new PxyJobReturn(jobReport);
            Thread.Sleep(2000);
            jobRtetorno.IdTask = context.BackgroundJob.Id;
            int total = 10;
            for (int i = 0; i < total; i++)
            {

                jobRtetorno.Progress = (int)(((double)i / (double)total) * 100);
                jobRtetorno.Mensagem = "Calculando Funcionário 1, parte = " + i;
                if (DateTime.Now > Convert.ToDateTime("19/07/2018 08:42:00") && DateTime.Now < Convert.ToDateTime("19/07/2018 09:00:00"))
                {
                    throw new Exception("Horário não permitido para execução");
                }

                if (jobRtetorno.IdTask == "230")
                {
                    throw new Exception("Task não pode ser executada");
                }
                ReportarJobProgresso(context, jobRtetorno);

            if (i == 8)
                {
                    context.CancellationToken.ThrowIfCancellationRequested();
                }

                Thread.Sleep(2000);
            }
        }

        public void ReportarJobProgresso(PerformContext context, PxyJobReturn jobReport)
        {
            context.WriteLine("Progresso = " + JsonConvert.SerializeObject(jobReport));
            NotificationHub.ReportarJobProgresso(jobReport);
        }

        //public void ReportarJobCompleto(PerformContext context, PxyJobReturn jobReport)
        //{
        //    context.WriteLine("Progresso = " + JsonConvert.SerializeObject(jobReport));
        //    NotificationHub.ReportarJobProgresso(jobReport);
        //    NotificationHub.ReportarJobCompleto(jobReport);
        //}
        #endregion
    }
}
