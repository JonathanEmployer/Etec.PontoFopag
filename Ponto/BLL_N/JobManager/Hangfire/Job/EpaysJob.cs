using BLL.Epays;
using BLL.Relatorios.V2;
using Hangfire.Server;
using Modelo.EntityFramework.MonitorPontofopag;
using Modelo.Relatorios;
using System.Collections.Generic;

namespace BLL_N.JobManager.Hangfire.Job
{
    public class EpaysJob : JobBase
    {
        public EpaysJob() : base()
        {

        }

        public void SendToEpaysDeleted(PerformContext context, JobControl jobReport, int idFechamento, IEnumerable<int> idsExcluidos, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            var _fechamento = new FechamentoPontoEpaysBLL(userPF.ConnectionString);
            _fechamento.SendToEpaysDeleted(idFechamento, idsExcluidos);
        }

        public void GetRelatorioExportacaoPontoFechamento(PerformContext context, JobControl jobReport, ExportacaoFechamentoPontoModel relatorioFiltro, string db, string usuario)
        {
            SetParametersBase(context, jobReport, db, usuario);
            RelatorioExportacaoFechamentoPontoBLL rel = new RelatorioExportacaoFechamentoPontoBLL(relatorioFiltro, userPF, pb, jobReport.JobId);
            rel.GetRelatorio();
        }

    }
}
