using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class JustificativaController : IControllerPontoWeb<Justificativa>
    {
        [PermissoesFiltro(Roles = "Justificativa")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Justificativa());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Justificativa bllJustificativa = new BLL.Justificativa(usr.ConnectionString, usr);
                List<Modelo.Justificativa> dados = bllJustificativa.GetAllList(true);
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

        [PermissoesFiltro(Roles = "JustificativaConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "JustificativaCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "JustificativaAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "JustificativaCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Justificativa obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "JustificativaAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Justificativa obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "JustificativaExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Justificativa justificativa = bllJustificativa.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllJustificativa.Salvar(Acao.Excluir, justificativa);
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

        protected override ActionResult Salvar(Justificativa obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                        acao = Acao.Incluir;
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllJustificativa.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Justificativa");
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
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(usr.ConnectionString, usr);
            Justificativa justificativa = new Justificativa();
            if (id == 0)
            {
                justificativa.Codigo = bllJustificativa.MaxCodigo();
                justificativa.Ativo = true;
                justificativa.JustificativaRestricao = new List<JustificativaRestricao>();
            }
            else
            {
                justificativa = bllJustificativa.LoadObject(id);
                BLL.JustificativaRestricao bllJustificativaRestricao = new BLL.JustificativaRestricao(usr.ConnectionString, usr);
                justificativa.JustificativaRestricao = bllJustificativaRestricao.GetAllListByJustificativas(new List<int>() { justificativa.Id });
            }
            return View("Cadastrar", justificativa);
        }

        protected override void ValidarForm(Justificativa obj)
        {
            
        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IList<Justificativa> ljust = new List<Justificativa>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllJustificativa.GetIdPorCod(codigo, true).GetValueOrDefault();
                Justificativa just = bllJustificativa.LoadObject(id);
                if (just != null && just.Id > 0)
                {
                    ljust.Add(just);
                }
            }

            if (ljust.Count == 0)
            {
                ljust = bllJustificativa.GetAllListConsultaEvento(true);
                if (!String.IsNullOrEmpty(consulta))
                {
                    ljust = ljust.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Justificativa";
            return View(ljust);
        }


        public ActionResult ValidaDescJustificativa(string consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            int codigo = -1;
            consulta = consulta.Split('|')[0];
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Justificativa just = bllJustificativa.LoadObjectByCodigo(codigo);
                if (just != null && just.Id > 0)
                {
                    return Json(new { Success = true, Erro = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Success = false, Erro = "Justificativa " + consulta + " não encontrada, Verifique!" }, JsonRequestBehavior.AllowGet);
        }

        public static int BuscaIdJustificativa(string consulta)
        {
            int id = 0;
            try
            {
                if (!String.IsNullOrEmpty(consulta))
                {
                    var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                    BLL.Justificativa bllJustificativa = new BLL.Justificativa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                    int codigo = -1;
                    consulta = consulta.Split('|')[0];
                    try { codigo = Int32.Parse(consulta); }
                    catch (Exception) { codigo = -1; }
                    if (codigo != -1)
                    {
                        Justificativa just = bllJustificativa.LoadObjectByCodigo(codigo);
                        if (just != null && just.Id > 0)
                        {
                            return just.Id;
                        }
                    } 
                }
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public ActionResult AdicionaNovaRestricao(int index, int tipoRestricao, string restricao, int idJustificativa)
        {
            var _usr = Usuario.GetUsuarioPontoWebLogadoCache();
            try
            {
                int? idEmpresa = null;
                int? idContrato = null;
                string codigo = "";
                int codigoInt;
                try
                {
                    codigo = restricao.Split('|')[0];
                    int.TryParse(codigo, out codigoInt);
                }
                catch (Exception)
                {
                    throw new Exception(string.Format("Valor ({0}) informado para pesquisa inválido, valor esperado no formato \"0 | descricao\"", restricao));
                }

                if (tipoRestricao == 0)
                {
                    idEmpresa = new BLL.Empresa(_usr.ConnectionString, _usr).GetIdsPorCodigos(new List<int>() { codigoInt }).FirstOrDefault();
                    if (idEmpresa.GetValueOrDefault() == 0) throw new Exception(String.Format("Empresa {0} não encontrato", restricao));
                }
                else
                {
                    idContrato = new BLL.Contrato(_usr.ConnectionString, _usr).getId(codigoInt, null, null);
                    if (idContrato.GetValueOrDefault() == 0) throw new Exception(String.Format("Contrato {0} não encontrato", restricao));
                }

                var Justificativa = new Modelo.Justificativa();
                Justificativa.JustificativaRestricao = new List<Modelo.JustificativaRestricao>();
                Justificativa.JustificativaRestricao.Add(
                    new JustificativaRestricao()
                    {
                        IdEmpresa = idEmpresa,
                        IdContrato = idContrato,
                        DescEmpresa = restricao,
                        IdJustificativa = idJustificativa
                    }
                );
                string novo = RenderViewToString("AdicionaNovaRestricao", Justificativa);
                novo = novo.Replace("JustificativaRestricao_0__", "JustificativaRestricao_" + index + "__");
                novo = novo.Replace("JustificativaRestricao[0].", "JustificativaRestricao[" + index + "].");
                return Json(new { Success = true, HTML = novo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Erro = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}