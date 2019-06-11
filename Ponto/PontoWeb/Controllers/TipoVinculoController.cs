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
    public class TipoVinculoController : IControllerPontoWeb<TipoVinculo>
    {
        [PermissoesFiltro(Roles = "TipoVinculo")]
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.TipoVinculo bllTipoVinculo = new BLL.TipoVinculo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(bllTipoVinculo.GetAllList());
        }

        [PermissoesFiltro(Roles = "TipoVinculoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "TipoVinculoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "TipoVinculoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "TipoVinculoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(TipoVinculo obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "TipoVinculoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(TipoVinculo obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "TipoVinculoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.TipoVinculo bllTipoVinculo = new BLL.TipoVinculo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            TipoVinculo TipoVinculo = bllTipoVinculo.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllTipoVinculo.Salvar(Acao.Excluir, TipoVinculo);
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
                return TratarErros(ex);
            }
        }

        public ActionResult TratarErros(Exception ex)
        {
            if (ex.Message.Contains("FK_Funcionario_TipoVinculo"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(TipoVinculo obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.TipoVinculo bllTipoVinculo = new BLL.TipoVinculo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllTipoVinculo.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "TipoVinculo");
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
            BLL.TipoVinculo bllTipoVinculo = new BLL.TipoVinculo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            TipoVinculo TipoVinculo = new TipoVinculo();
            TipoVinculo = bllTipoVinculo.LoadObject(id);
            if (id == 0)
            {
                TipoVinculo.Codigo = bllTipoVinculo.MaxCodigo();
            }
            return View("Cadastrar", TipoVinculo);
        }

        protected override void ValidarForm(TipoVinculo obj)
        {
            throw new NotImplementedException();
        }

        #region Eventos Consulta
        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.TipoVinculo bllTipoVinculo = new BLL.TipoVinculo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IList<TipoVinculo> lTipoVinculo = new List<TipoVinculo>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllTipoVinculo.GetIdPorCod(codigo).GetValueOrDefault();
                TipoVinculo TipoVinculo = bllTipoVinculo.LoadObject(id);
                if (TipoVinculo != null && TipoVinculo.Id > 0)
                {
                    lTipoVinculo.Add(TipoVinculo);
                }
            }

            if (lTipoVinculo.Count == 0)
            {
                lTipoVinculo = bllTipoVinculo.GetAllList();
                if (!String.IsNullOrEmpty(consulta))
                {
                    lTipoVinculo = lTipoVinculo.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Tipo Vínculo";
            return View(lTipoVinculo);
        }

        public static int BuscaIdtipoVinculo(string TipoVinculo)
        {
            TipoVinculo f = new TipoVinculo();
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.TipoVinculo bllTipoVinculo = new BLL.TipoVinculo(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);

            string codigo = TipoVinculo.Split('|')[0].Trim();
            int cod = 0;
            try
            {
                cod = Convert.ToInt32(codigo);
            }
            catch (Exception)
            {
                cod = 0;
            }
            int? idTipoVinculo = bllTipoVinculo.GetIdPorCod(cod);
            return idTipoVinculo.GetValueOrDefault();
        }
        #endregion
    }
    }