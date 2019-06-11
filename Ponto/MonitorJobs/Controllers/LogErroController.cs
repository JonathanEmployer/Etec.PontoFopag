using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonitorJobs.Controllers
{
    [Authorize]
    public class LogErroController : Controller
    {
        // GET: LogErro
        public ActionResult Index(string data, string appId)
        {
            DateTime dataLog = DateTime.Now;
            string erro = string.Empty;
            string connEntities = System.Configuration.ConfigurationManager.ConnectionStrings["PLATAFORMA_LOG_ERROEntities"].ConnectionString;
            EntityConnectionStringBuilder connEnt = new EntityConnectionStringBuilder(connEntities);
            string conn = connEnt.ProviderConnectionString;
            DAL.TabLogErro dalTablogErro = new DAL.TabLogErro(conn);
            if (String.IsNullOrEmpty(appId))
            {
                appId = ConfigurationManager.AppSettings["AppID"] ?? "";
            }

            if (!String.IsNullOrEmpty(data))
            {
                try
                {
                    dataLog = Convert.ToDateTime(data);
                }
                catch (Exception e)
                {
                    erro = "A data informada é inválida, formato correto dd/mm/yyyy Ex: "+DateTime.Now.ToShortDateString();
                }
                
            }
            
            List<Models.TabLogErro> erros = new List<Models.TabLogErro>();
            
            try
            {
                erros = dalTablogErro.GetErros(dataLog, appId);
            }
            catch (SqlException s)
            {
                erro = "O servidor não respondeu a tempo, tente novamente daqui alguns minutos, ou contate o DBA, detalhe = "+s.Message;
            }
            catch (Exception e)
            {
                erro = e.Message;
            }

            ViewBag.Data = dataLog.ToShortDateString();
            ViewBag.AppId = appId;
            ViewBag.ErroTratado = erro;
            return View(erros);
        }
    }
}