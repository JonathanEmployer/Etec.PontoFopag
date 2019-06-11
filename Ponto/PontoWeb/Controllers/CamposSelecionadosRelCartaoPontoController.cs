using BLL.Util;
using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using PontoWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class CamposSelecionadosRelCartaoPontoController : Controller
    {
        //[PermissoesFiltro(Roles = "CamposSelecionadosRelCartaoPonto")]
        //public ActionResult Index()
        //{
        //    var usr = Usuario.GetUsuarioPontoWebLogadoCache();
        //    BLL.CamposSelecionadosRelCartaoPonto bllCamposSelecionadosRelCartaoPonto = new BLL.CamposSelecionadosRelCartaoPonto(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
        //    List<Modelo.CamposSelecionadosRelCartaoPonto> camposSelecionadosCartao = bllCamposSelecionadosRelCartaoPonto.GetAllList();
        //    PxyCPEMarcacao marc = new PxyCPEMarcacao();
        //    List<Modelo.Utils.CartaoPontoCamposParaCustomizacao> campos = GetPropertiesCartaoPontoCustom.GetProperties(marc.GetType());
        //    foreach (Modelo.CamposSelecionadosRelCartaoPonto item in camposSelecionadosCartao)
        //    {
        //        item.PropriedadesCampo = campos.Where(c => c.NomePropriedade == item.PropriedadeModelo).FirstOrDefault();
        //    }
        //    return View(camposSelecionadosCartao);
        //}

        [PermissoesFiltro(Roles = "CamposSelecionadosRelCartaoPontoConsultar")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "CamposSelecionadosRelCartaoPontoCadastrar")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        //[PermissoesFiltro(Roles = "CamposSelecionadosRelCartaoPontoAlterar")]
        //public ActionResult Alterar(int id)
        //{
        //    return GetPagina(id);
        //}

        [PermissoesFiltro(Roles = "CamposSelecionadosRelCartaoPontoCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(List<CamposSelecionadosRelCartaoPonto> objs)
        {
            return Salvar(objs);
        }

        //[PermissoesFiltro(Roles = "CamposSelecionadosRelCartaoPontoAlterar")]
        //[HttpPost]
        //public ActionResult Alterar(List<CamposSelecionadosRelCartaoPonto> objs)
        //{
        //    return Salvar(objs);
        //}

        //[PermissoesFiltro(Roles = "CamposSelecionadosRelCartaoPontoExcluir")]
        //[HttpPost]
        //public override ActionResult Excluir(int id)
        //{
        //    var usr = Usuario.GetUsuarioPontoWebLogadoCache();
        //    BLL.CamposSelecionadosRelCartaoPonto bllCamposSelecionadosRelCartaoPonto = new BLL.CamposSelecionadosRelCartaoPonto(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
        //    CamposSelecionadosRelCartaoPonto CamposSelecionadosRelCartaoPonto = bllCamposSelecionadosRelCartaoPonto.LoadObject(id);
        //    try
        //    {
        //        Dictionary<string, string> erros = new Dictionary<string, string>();
        //        erros = bllCamposSelecionadosRelCartaoPonto.Salvar(Acao.Excluir, CamposSelecionadosRelCartaoPonto);
        //        if (erros.Count > 0)
        //        {
        //            string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
        //            return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
        //        }
        //        return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return TratarErros(ex);
        //    }
        //}


        public ActionResult Salvar(List<CamposSelecionadosRelCartaoPonto> objs)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.CamposSelecionadosRelCartaoPonto bllCamposSelecionadosRelCartaoPonto = new BLL.CamposSelecionadosRelCartaoPonto(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Dictionary<string, string> ObjsErros = new Dictionary<string, string>();
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var obj in objs)
                    {
                        Acao acao = new Acao();
                        if (String.IsNullOrEmpty(obj.PropriedadeModelo))
                        {
                            acao = Acao.Excluir;
                        }
                        else if (obj.Id == 0)
                        {
                            acao = Acao.Incluir;
                        }
                        else
                            acao = Acao.Alterar;

                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllCamposSelecionadosRelCartaoPonto.Salvar(acao, obj);
                        if (erros.Count > 0)
                        {
                            ObjsErros.Concat(erros);
                        }
                    }
                    if (ObjsErros.Count > 0)
                    {
                        string erro = string.Join(";", ObjsErros.Distinct().Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }

            PxyCPEMarcacao marc = new PxyCPEMarcacao();
            List<Modelo.Utils.CartaoPontoCamposParaCustomizacao> campos = GetPropertiesCartaoPontoCustom.GetProperties(marc.GetType());
            campos.Add(new Modelo.Utils.CartaoPontoCamposParaCustomizacao { NomePropriedade = "", Header = "" });
            ViewBag.Campos = campos.Select(i => new SelectListItem
            {
                Text = i.Descricao,
                Value = i.NomePropriedade
            });
            return View("Cadastrar", objs);
        }

        public ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.CamposSelecionadosRelCartaoPonto bllCamposSelecionadosRelCartaoPonto = new BLL.CamposSelecionadosRelCartaoPonto(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            List<Modelo.CamposSelecionadosRelCartaoPonto> camposSelecionadosCartao = bllCamposSelecionadosRelCartaoPonto.GetAllList();
            int qtdCamposMaxCartao = 11;
            int qtdFaltando = qtdCamposMaxCartao - camposSelecionadosCartao.Count();
            if (qtdFaltando > 0)
            {
                for (int i = 0; i < qtdFaltando; i++)
                {
                    camposSelecionadosCartao.Add(new CamposSelecionadosRelCartaoPonto { Codigo = camposSelecionadosCartao.Count() > 0 ? camposSelecionadosCartao.Max(m => m.Codigo) + 1 : 1,
                                                                                        Posicao = Convert.ToInt16(camposSelecionadosCartao.Count() > 0 ? camposSelecionadosCartao.Max(m => m.Posicao) + 1 : 1),
                                                                                        PropriedadeModelo = ""
                    });
                }
            }
            PxyCPEMarcacao marc = new PxyCPEMarcacao();
            List<Modelo.Utils.CartaoPontoCamposParaCustomizacao> campos = GetPropertiesCartaoPontoCustom.GetProperties(marc.GetType());
            campos.Add(new Modelo.Utils.CartaoPontoCamposParaCustomizacao { NomePropriedade = "", Header = "" });
            ViewBag.Campos = campos.Select(i => new SelectListItem
            {
                Text = i.Descricao,
                Value = i.NomePropriedade
            });
            return View("Cadastrar", camposSelecionadosCartao);
        }
    }
}
