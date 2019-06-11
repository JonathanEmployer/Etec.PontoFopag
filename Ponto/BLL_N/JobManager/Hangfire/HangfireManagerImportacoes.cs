using System;
using System.Collections.Generic;
using System.IO;
using BLL_N.JobManager.Hangfire.Job;
using Hangfire;
using Hangfire.States;
using Modelo;
using Modelo.EntityFramework.MonitorPontofopag;

namespace BLL_N.JobManager.Hangfire
{
    public class HangfireManagerImportacoes : HangfireManagerBase
    {
        public HangfireManagerImportacoes(string dataBase) : base(dataBase)
        {
        }

        public HangfireManagerImportacoes(string dataBase, string usuario, string hostAddress, string urlReferencia) : base(dataBase, usuario, hostAddress, urlReferencia)
        {
        }

        public Modelo.Proxy.PxyJobReturn ImportarArquivoAFD(IList<REP> listaReps, DateTime? dataInicial, DateTime? dataFinal, FileInfo arquivo, bool pIndividual, string dsCodFuncionario)
        {
            string parametrosExibicao = String.Format("Período {0} a {1}, arquivo: {2}", dataInicial.GetValueOrDefault().ToShortDateString(), dataFinal.GetValueOrDefault().ToShortDateString(), arquivo.Name);
            try
            {
                if (pIndividual)
                {
                    string conexao = BLL.cwkFuncoes.ConstroiConexao(dsCodFuncionario).ConnectionString;
                    BLL.Funcionario bllFunc = new BLL.Funcionario(conexao);
                    Funcionario func = bllFunc.RetornaFuncDsCodigo(dsCodFuncionario);
                    parametrosExibicao += String.Format(" Funcionário: {0}", func.Dscodigo + " | " + func.Nome);
                }
            }
            catch (Exception)
            {

            }
            JobControl jobControl = GerarJobControl("Importação de AFD", parametrosExibicao);
            jobControl.FileUpload = arquivo.FullName;
            string idJob = new BackgroundJobClient().Create<ImportacoesJob>(x => x.ImportacaoAFD(null, jobControl, listaReps, dataInicial, dataFinal, arquivo, pIndividual, dsCodFuncionario, dataBase, usuarioLogado), _enqueuedStateNormal);

            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }
    }
}
