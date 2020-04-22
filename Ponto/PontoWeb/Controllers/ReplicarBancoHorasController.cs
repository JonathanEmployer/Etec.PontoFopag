using BLL_N.JobManager.Hangfire;
using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ReplicarBancoHorasController : Controller
    {
        UsuarioPontoWeb user = Usuario.GetUsuarioPontoWebLogadoCache();
        // GET: ReplicarBancoHoras
        public ActionResult Index(int id)
        {
            PxyCopiaBancoHoras reg = GetProxyCopiaBanco(id);
            return View(reg);
        }

        private PxyCopiaBancoHoras GetProxyCopiaBanco(int id)
        {
            BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(user.ConnectionString, user);
            BancoHoras bh = bllBancoHoras.LoadObject(id);
            PxyCopiaBancoHoras reg = new PxyCopiaBancoHoras() { IdBancoHoras = bh.Id, Codigo = bh.Codigo, Nome = bh.Nome, TipoDescricao = bh.TipoDescricao, DataInicial = bh.DataInicial, DataFinal = bh.DataFinal };
            return reg;
        }

        // GET: ReplicarBancoHoras
        [HttpPost]
        public ActionResult Index(PxyCopiaBancoHoras registro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(user.ConnectionString, user);
                    var idsfuncs = registro.IdSelecionados.Split(',').Where(s => !string.IsNullOrEmpty(s)).Select(s => Convert.ToInt32(s)).ToList();
                    bllBancoHoras.ReplicarBancoHoras(registro.IdBancoHoras, idsfuncs);
                    
                    HangfireManagerCalculos hfm = new HangfireManagerCalculos(user.DataBase, user.Login, "", "/BancoHoras/Grid");
                    string parametrosExibicao = String.Format("{0} Funcionários no período de {1} a {2}", idsfuncs.Count(), registro.DataInicialStr, registro.DataFinalStr);

                    List<PxyFuncionariosRecalcular> funcsPeriodo = new List<PxyFuncionariosRecalcular>();
                    idsfuncs.ForEach(f => funcsPeriodo.Add(new PxyFuncionariosRecalcular() { IdFuncionario = f, DataInicio = registro.DataInicial, DataFim = registro.DataFinal }));
                    
                    PxyJobReturn ret = hfm.RecalculaMarcacao("Recalculo de marcações por cópia de banco de horas", parametrosExibicao, funcsPeriodo, false);

                    return RedirectToAction("Grid", "BancoHoras");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("IdSelecionados", e.Message);
                }
            }
            PxyCopiaBancoHoras reg = GetProxyCopiaBanco(registro.IdBancoHoras);
            reg.IdSelecionados = registro.IdSelecionados;
            return View(reg);
        }

        [Authorize]
        public JsonResult GetFuns(int idBancoHoras)
        {
            try
            {
                BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(user.ConnectionString, user);
                List<pxyFuncionarioRelatorio> funcs = bllBancoHoras.GetFuncionarioParaCopia(idBancoHoras);

                JsonResult jsonResult = Json(new { data = funcs }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}