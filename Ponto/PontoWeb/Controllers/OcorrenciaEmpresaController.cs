using PontoWeb.Controllers.BLLWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class OcorrenciaEmpresaController : Controller
    {
        // GET: OcorrenciaEmpresa/Cadastrar/5
        public ActionResult Cadastrar(int id)
        {
            Modelo.Proxy.pxyOcorrenciaEmpresa pxyOcorrencia = new Modelo.Proxy.pxyOcorrenciaEmpresa();
            pxyOcorrencia.idEmpresa = id;
            BLL.OcorrenciaEmpresa bllOcorrenciaEmpresa = new BLL.OcorrenciaEmpresa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            IList<Modelo.OcorrenciaEmpresa> dados = bllOcorrenciaEmpresa.GetAllExibePainel(id);
            dados.ToList().ForEach(f => f.Id = f.idOcorrencia);
            pxyOcorrencia.idsSelecionados = String.Join(",",dados.Where(x => x.Selecionado).Select(s => s.Id));

            return View(pxyOcorrencia);
        }

        // POST: OcorrenciaEmpresa/Cadastrar/5
        [HttpPost]
        public ActionResult Cadastrar(Modelo.Proxy.pxyOcorrenciaEmpresa pxyOcorrencia)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    List<Modelo.OcorrenciaEmpresa> lstOcorrenciasSelecionadas = new List<Modelo.OcorrenciaEmpresa>();
                    lstOcorrenciasSelecionadas = pxyOcorrencia.GerarOcorrencias();

                    BLL.OcorrenciaEmpresa bllOcorrenciaEmpresa = 
                    new BLL.OcorrenciaEmpresa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());

                    bllOcorrenciaEmpresa.DeleteAllByIdEmpresa(pxyOcorrencia.idEmpresa);
                    bllOcorrenciaEmpresa.IncluirOcorrenciasEmpresa(lstOcorrenciasSelecionadas);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return RedirectToAction("Grid", "Empresa");
        }

        [Authorize]
        public JsonResult DadosGrid(int id)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.OcorrenciaEmpresa bllOcorrenciaEmpresa = new BLL.OcorrenciaEmpresa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                IList<Modelo.OcorrenciaEmpresa> dados = bllOcorrenciaEmpresa.GetAllExibePainel(id);
                dados.ToList().ForEach(f => f.Id = f.idOcorrencia);
                JsonResult jsonResult = Json(new { data = dados }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }
    }
}
