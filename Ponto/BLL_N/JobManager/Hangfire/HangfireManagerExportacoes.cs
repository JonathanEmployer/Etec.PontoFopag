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
    public class HangfireManagerExportacoes : HangfireManagerBase
    {
        public HangfireManagerExportacoes(string dataBase) : base(dataBase)
        {
        }

        public HangfireManagerExportacoes(string dataBase, string usuario, string hostAddress, string urlReferencia) : base(dataBase, usuario, hostAddress, urlReferencia)
        {
        }

        public Modelo.Proxy.PxyJobReturn ExportaArquivoFolhaPgto(Modelo.Proxy.pxyExportacaoFolha obj)
        {
            String tipo = "";
            switch (obj.TipoSelecao)
            {
                case 0:
                    tipo = "Empresa - "+obj.Empresa;
                    break;
                case 1:
                    tipo = "Departamento - " + obj.Departamento;
                    break;
                case 2:
                    tipo = "Funcionário - " + obj.Funcionario;
                    break;
                default:
                    break;
            }
            string parametrosExibicao = String.Format("Período {0} a {1}, {2}", obj.DataI.GetValueOrDefault().ToShortDateString(), obj.DataF.GetValueOrDefault().ToShortDateString(), obj.idSelecionados.Split(',').Length);
            
            JobControl jobControl = GerarJobControl("Exportação de dados para folha", parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<ExportacoesJob>(x => x.ExportaArquivoFolhaPgto(null, jobControl, dataBase, usuarioLogado, obj), _enqueuedStateNormal);

            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn ExportaArquivoWebfopag(Modelo.Proxy.pxyExportacaoFolha obj)
        {
            string parametrosExibicao = String.Format("Período {0} a {1}, Funcionários: {2}", obj.DataI.GetValueOrDefault().ToShortDateString(), obj.DataF.GetValueOrDefault().ToShortDateString(), obj.idSelecionados.Split(',').Length);

            JobControl jobControl = GerarJobControl("Exportação de dados para Webfopag", parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<ExportacoesJob>(x => x.ExportaArquivoWebfopag(null, jobControl, dataBase, usuarioLogado, obj), _enqueuedStateNormal);

            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn ExportaArquivosAFDTACJEF(int tipoArquivo, Modelo.Empresa empresa, DateTime dataInicial, DateTime dataFinal)
        {
            string nomeArquivo = "";

            switch (tipoArquivo)
            {
                case 0:
                    nomeArquivo = "AFDT";
                    break;
                case 1:
                    nomeArquivo = "ACJEF";
                    break;
            }

            string parametrosExibicao = String.Format("Período {0} a {1}, Empresa: {2}", dataFinal.ToShortDateString(), dataFinal.ToShortDateString(), empresa.Codigo + " | " + empresa.Nome);

            JobControl jobControl = GerarJobControl("Exportação de "+ nomeArquivo, parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<ExportacoesJob>(x => x.ExportaArquivosAFDTACJEF(null, jobControl, dataBase, usuarioLogado, tipoArquivo, empresa, dataInicial, dataFinal), _enqueuedStateNormal);

            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }

        public Modelo.Proxy.PxyJobReturn ExportarAFD(int codLocal, int numSerie, DateTime dataInicial, DateTime dataFinal)
        {
            string parametrosExibicao = String.Format("Número de série {0}, período {1} a {2}", numSerie, dataFinal.ToShortDateString(), dataFinal.ToShortDateString());

            JobControl jobControl = GerarJobControl("Exportação de AFD", parametrosExibicao);
            string idJob = new BackgroundJobClient().Create<ExportacoesJob>(x => x.ExportarAFD(null, jobControl, dataBase, usuarioLogado, codLocal, numSerie, dataInicial, dataFinal), _enqueuedStateNormal);

            Modelo.Proxy.PxyJobReturn jobReturn = GerarJobReturn(jobControl, idJob);
            return jobReturn;
        }
    }
}
