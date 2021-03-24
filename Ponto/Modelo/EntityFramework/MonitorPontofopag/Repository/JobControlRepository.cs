using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Modelo.EntityFramework.MonitorPontofopag.Repository
{
    public class JobControlRepository
    {
        public List<JobControl> GetJobsPaginado(int skip, int qtdRegs, string urlReference, string user)
        {
            List<JobControl> jobs = new List<JobControl>();
            using (MONITOR_PONTOFOPAGEntities db = new MONITOR_PONTOFOPAGEntities())
            {
                #region Consulta
                var parms = new[]{
                       new SqlParameter("urlReferencia", urlReference),
                        new SqlParameter("skip", skip),
                        new SqlParameter("take", qtdRegs),
                        new SqlParameter("usuario", user)
                    };

                string sql = @" select jc.Id
                                  ,jc.Usuario
                                  ,jc.Inchora
                                  ,jc.JobId
                                  ,jc.UrlRota
                                  ,jc.UrlHost
                                  ,jc.UrlReferencia
                                  ,jc.NomeProcesso
                                  ,jc.IdLoteCalculo
                                  ,'' Parametros
                                  ,ParametrosExibicao
                                  ,FileUpload
                                  ,FileDownload
                                  ,DataCalcelameto
                                  ,UsuarioCancelamento
                                  ,StatusAnterior
                                  ,StatusNovo
                                  ,Mensagem
                                  ,Progresso
                                  ,PermiteCancelar
                              from [dbo].[JobControl] jc
                              left join [HangFire].[Job] j on jc.JobId = j.Id
                             where JobId > 0
                               and (    jc.UrlReferencia = @urlReferencia 
                                    or Replace(jc.UrlReferencia,'/Grid','') = @urlReferencia
                                    or @urlReferencia = '')
                               and jc.Usuario = @usuario
                             order by iif(j.id is null, 100, Progresso), Inchora desc

                            OFFSET @skip ROWS -- skip 10 rows
                             FETCH NEXT @take ROWS ONLY; -- take 10 rows ";
                #endregion

                jobs = db.JobControl.SqlQuery(sql, parms).ToList();
            }
            return jobs;
        }
    }
}
