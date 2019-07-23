using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class OcorrenciaController : IControllerPontoWeb<Ocorrencia>
    {
        [PermissoesFiltro(Roles = "Ocorrencia")]
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(new Modelo.Ocorrencia());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Ocorrencia> dados = bllOcorrencia.GetAllList();
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

        [PermissoesFiltro(Roles = "OcorrenciaConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "OcorrenciaCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "OcorrenciaAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "OcorrenciaCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Ocorrencia obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "OcorrenciaAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Ocorrencia obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "OcorrenciaExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Ocorrencia ocorrencia = bllOcorrencia.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllOcorrencia.Salvar(Acao.Excluir, ocorrencia);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override ActionResult Salvar(Ocorrencia obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                    {
                        acao = Acao.Incluir;
                    }
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllOcorrencia.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Ocorrencia");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Ocorrencia ocorrencia = new Ocorrencia();
            if (id == 0)
            {
                ocorrencia.Codigo = bllOcorrencia.MaxCodigo();
                ocorrencia.TipoAbono = 0;
                ocorrencia.Ativo = true;
            }
            else
            {
                ocorrencia = bllOcorrencia.LoadObject(id);
            }
            
            return View("Cadastrar", ocorrencia);
           
        }

        protected override void ValidarForm(Ocorrencia obj)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IList<Ocorrencia> lOcorrencia = new List<Ocorrencia>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Ocorrencia ocorrencia = bllOcorrencia.LoadObjectByCodigo(codigo);
                if (ocorrencia != null && ocorrencia.Id > 0)
                {
                    lOcorrencia.Add(ocorrencia);
                }
            }

            if (lOcorrencia.Count == 0)
            {
                lOcorrencia = bllOcorrencia.GetAllListConsultaEvento();
                if (!String.IsNullOrEmpty(consulta))
                {
                    lOcorrencia = lOcorrencia.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Ocorrência";
            return View(lOcorrencia);
        }

        public static int BuscaIdOcorrencia(string ocorrencia)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            int id = 0;
            try
            {
                if (!String.IsNullOrEmpty(ocorrencia))
                {

                    string cod = ocorrencia.Split('|')[0].Trim();
                    int codigo = Convert.ToInt32(cod);
                    Ocorrencia objOcorrencia = new Ocorrencia();
                    objOcorrencia = bllOcorrencia.LoadObjectByCodigo(codigo);
                    if (objOcorrencia.Id > 0) id = objOcorrencia.Id;
                    else id = 0;
                }
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}