using BLL.Epays;
using BLL_N.JobManager.Hangfire.Job;
using Hangfire;
using Modelo;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Relatorios;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BLL_N.JobManager.Hangfire
{
    public class HangfireIntegrationEpays : HangfireManagerBase
    {
        private UsuarioPontoWeb userPF;

        public HangfireIntegrationEpays(string dataBase, string urlReferencia = "/FechamentoPonto/Grid") : base(dataBase)
        {
            base.urlReferencia = urlReferencia; 
        }

        private void SetParameter(string db, string usuario)
        {
            string conexao = BLL.cwkFuncoes.ConstroiConexao(db).ConnectionString;
            DAL.SQL.UsuarioPontoWeb dalUsuarioPontoWeb = new DAL.SQL.UsuarioPontoWeb(new DAL.SQL.DataBase(conexao));
            userPF = dalUsuarioPontoWeb.LoadObjectLogin(usuario);
            userPF.ConnectionString = conexao;
        }

        private void UpdateIdJob(int idFechamento, string idJob)
        {
            SetParameter(dataBase, usuarioLogado);
            var _fechamentoPontoBll = new BLL.FechamentoPonto(userPF.ConnectionString);
            _fechamentoPontoBll.UpdateIdJob(idFechamento, idJob);
        }

        public void SendToEpaysDeleted(int idFechamento, IEnumerable<int> idsExcluidos)
        {
            SetParameter(dataBase, usuarioLogado);
            JobControl jobControl = GerarJobControl("Integração Epays", $"{idsExcluidos.Count()} documento(s) enviados p/ exclusão.");
            jobControl.PermiteCancelar = true;

            string idJob = new BackgroundJobClient()
                            .Create<EpaysJob>(x => x.SendToEpaysDeleted(null, jobControl, idFechamento, idsExcluidos, dataBase, usuarioLogado), _enqueuedStateNormal);

            UpdateIdJob(idFechamento, idJob);
        } 

        public void RelatorioExportacaoPontoFechamento(int idFechamento, ExportacaoFechamentoPontoModel parametros)
        {
            JobControl jobControl = GerarJobControl("Integração Epays", $"{parametros.IdSelecionados.Split(',').Count()} documento(s).");
            
            jobControl.PermiteCancelar = true;
            string idJob = new BackgroundJobClient().Create<EpaysJob>(x => x.GetRelatorioExportacaoPontoFechamento(null, jobControl, parametros, dataBase, usuarioLogado), _enqueuedStateNormal);

            UpdateIdJob(idFechamento, idJob);
        }

    }
}
