using System;
using System.Collections.Generic;
using System.IO;
using Modelo;
using Modelo.Proxy;
using Modelo.EntityFramework.MonitorPontofopag;
using Hangfire.Server;

namespace BLL_N.JobManager.Hangfire.Job
{
    public class ImportacoesJob : JobBase
    {
        public ImportacoesJob():base()
        {

        }

        public void ImportacaoAFD(PerformContext context, JobControl jobReport, IList<REP> listaReps, DateTime? dataInicial, DateTime? dataFinal, FileInfo arquivo, bool pIndividual, string dsCodFuncionario, string db, string usuario, bool? bRazaoSocial)
        {
            SetParametersBase(context, jobReport, db, usuario);

            pb.setaMinMaxPB(0, 100);
            pb.setaValorPBCMensagem(-1, "Iniciando Importação do arquivo");

            List<Modelo.TipoBilhetes> lstTipoBilhete = MontaListaTipoBilhete(listaReps, arquivo.FullName, userPF.ConnectionString, userPF);

            string nomeArquivo = "LogImportação_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
            List<string> log = new List<string>();
            try
            {
                BLL_N.ImportacaoBilhetesNova bllImp = new BLL_N.ImportacaoBilhetesNova(userPF.ConnectionString, userPF);
                bllImp.HangfireContext = context;
                bllImp.JobReport = jobReport;
                log = bllImp.ImportacaoBilhete(pb, lstTipoBilhete, arquivo.FullName, 1, pIndividual, dsCodFuncionario, dataInicial, dataFinal, userPF ,bRazaoSocial);

                string salvarRetorno = @arquivo.DirectoryName+"\\"+ arquivo.Name.Replace(arquivo.Extension, "") + "Retorno" + arquivo.Extension;
                System.IO.File.WriteAllLines(salvarRetorno, log);
                JobControlManager.UpdateFileDownload(context, salvarRetorno);
            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                string salvarRetorno = @arquivo.DirectoryName + "\\Erro_" + arquivo.Name.Replace(arquivo.Extension, "") + arquivo.Extension;
                log.Add(e.Message);
                System.IO.File.WriteAllLines(salvarRetorno, log);
                JobControlManager.UpdateFileDownload(context, salvarRetorno);
                throw e;
            }
        }

        private List<TipoBilhetes> MontaListaTipoBilhete(IList<REP> listaReps, string arquivo, string ConnectionString, Modelo.UsuarioPontoWeb usuarioLogado)
        {
            List<TipoBilhetes> retorno = new List<TipoBilhetes>();
            TipoBilhetes tpBilhete;
            BLL.TipoBilhetes blltipoBilhete = new BLL.TipoBilhetes(ConnectionString, usuarioLogado);

            foreach (var rep in listaReps)
            {
                tpBilhete = new TipoBilhetes();
                tpBilhete.Codigo = 1;
                tpBilhete.Descricao = "Coleta AFD";
                tpBilhete.Diretorio = arquivo;
                tpBilhete.FormatoBilhete = 3;
                tpBilhete.BImporta = true;
                tpBilhete.Ordem_c = 0;
                tpBilhete.Ordem_t = 0;
                tpBilhete.Dia_c = 0;
                tpBilhete.Dia_t = 0;
                tpBilhete.Mes_c = 0;
                tpBilhete.Mes_t = 0;
                tpBilhete.Ano_c = 0;
                tpBilhete.Ano_t = 0;
                tpBilhete.Hora_c = 0;
                tpBilhete.Hora_t = 0;
                tpBilhete.Minuto_c = 0;
                tpBilhete.Minuto_t = 0;
                tpBilhete.IdRep = rep.Id;

                retorno.Add(tpBilhete);

            }

            return retorno;
        }
    }
}
