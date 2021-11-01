using BLL.Util;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class LayoutExportacaoController : IControllerPontoWeb<LayoutExportacao>
    {

        [PermissoesFiltro(Roles = "LayoutExportacao")]
        public override ActionResult Grid()
        {
            BLL.LayoutExportacao bllLayoutExportacao = new BLL.LayoutExportacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            return View(new Modelo.LayoutExportacao());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.LayoutExportacao bllLayoutExportacao = new BLL.LayoutExportacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.LayoutExportacao> dados = bllLayoutExportacao.GetAllList();
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

        [PermissoesFiltro(Roles = "LayoutExportacaoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LayoutExportacaoCadastrar")]
        public override ActionResult Cadastrar()
        {
            int id = 0;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LayoutExportacaoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LayoutExportacaoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(LayoutExportacao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LayoutExportacaoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(LayoutExportacao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LayoutExportacaoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            BLL.LayoutExportacao bllLayoutExportacao = new BLL.LayoutExportacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            LayoutExportacao objLayoutExportacao = bllLayoutExportacao.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllLayoutExportacao.Salvar(Acao.Excluir, objLayoutExportacao);
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

        protected override ActionResult Salvar(LayoutExportacao obj)
        {
            BLL.LayoutExportacao bllLayoutExportacao = new BLL.LayoutExportacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            ViewBag.TipoCampo = Conversores.ToSelectList2(Modelo.Listas.TipoCampoExportacao(), "indice", "nome");
            ViewBag.DelimitadorCampoExportacao = Conversores.ToSelectList2(Modelo.Listas.DelimitadorCampoExportacao(), "indice", "nome");
            ViewBag.QualificadorCampoExportacao = Conversores.ToSelectList2(Modelo.Listas.QualificadorCampoExportacao(), "indice", "nome");
            ViewBag.tamanhoStrExp = Convert.ToString(BLL.ExportacaoCampos.MontaStringExportacao(obj.ExportacaoCampos).Length + 1);
            ViewBag.IncluindoCamposExportacao = 0;
            ViewBag.VisualizarLayout = 0;
            obj.LabelCamposLayoutExportacao = String.Empty;
            obj.LabelQtdCamposLayoutExportacao = "0";

            if (obj.controlePagina == "VisualizarExportacaoCampos")
            {
                if ((obj.ExportacaoCampos != null) &&
                       (obj.ExportacaoCampos.Count > 0))
                {
                    var listaSemExcluidos = obj.ExportacaoCampos.Where(c => c.Acao != Acao.Excluir).ToList();
                    obj.LabelCamposLayoutExportacao = BLL.ExportacaoCampos.MontaStringExportacao(listaSemExcluidos);
                    obj.LabelQtdCamposLayoutExportacao = Convert.ToString(BLL.ExportacaoCampos.MontaStringExportacao(listaSemExcluidos).Length);
                }
                else
                {
                    obj.LabelCamposLayoutExportacao = String.Empty;
                    obj.LabelQtdCamposLayoutExportacao = "0";
                }
               
                ViewBag.VisualizarLayout = 1;

                return View("Cadastrar", obj);
            }
            else
            {
                if (obj.controlePagina == "IncluirExportacaoCampos")
                {
                    ExportacaoCampos exportacaoCampos = new ExportacaoCampos();
                    exportacaoCampos.IdControle = 1;
                    exportacaoCampos.Codigo = 1;
                    if ((obj.ExportacaoCampos != null) && 
                        (obj.ExportacaoCampos.Count > 0))
                    {
                        exportacaoCampos.IdControle = obj.ExportacaoCampos.Max(s => s.IdControle) + 1;
                        var listaSemExcluidos = obj.ExportacaoCampos.Where(c => c.Acao != Acao.Excluir).ToList();
                        if ((listaSemExcluidos != null) &&
                            (listaSemExcluidos.Count > 0))
                        {
                            exportacaoCampos.Codigo = listaSemExcluidos.Max(s => s.Codigo) + 1;
                        }
                    }
                    else
                    {
                        obj.LabelCamposLayoutExportacao = String.Empty;
                        obj.LabelQtdCamposLayoutExportacao = "0";
                    }
                    exportacaoCampos.Qualificador = "[nenhum]";
                    exportacaoCampos.Delimitador = "[nenhum]";
                    obj.ExportacaoCampos.Add(exportacaoCampos);
                    ViewBag.IncluindoCamposExportacao = exportacaoCampos.IdControle;

                    return View("Cadastrar", obj);
                }
                else
                {
                    Modelo.Acao acao = new Modelo.Acao();
                    Dictionary<string, string> erros = new Dictionary<string, string>();

                    if (obj.Id == 0)                        
                        acao = Acao.Incluir;
                    else
                        acao = Acao.Alterar;

                    //foreach (Modelo.ExportacaoCampos dja in obj.ExportacaoCampos)
                    //{
                    //    if(dja.Codigo == dja.Id)
                    //    {
                    //        dja.Acao = acao;
                    //    }

                    //}
                    
                    erros = bllLayoutExportacao.Salvar(acao, obj);
                    if (erros.Count() > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "LayoutExportacao");
                    }
                }
            }
            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.LayoutExportacao bllLayoutExportacao = new BLL.LayoutExportacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            LayoutExportacao objLayoutExportacao = new LayoutExportacao();
            objLayoutExportacao = bllLayoutExportacao.LoadObject(id);

            if (id == 0)
            {
                objLayoutExportacao.Codigo = bllLayoutExportacao.MaxCodigo();
                objLayoutExportacao.LabelCamposLayoutExportacao = String.Empty;
                objLayoutExportacao.LabelQtdCamposLayoutExportacao = "0";
            }
            else
            {
                foreach (ExportacaoCampos expC in objLayoutExportacao.ExportacaoCampos)
                {
                    expC.IdControle = expC.Id;

                }
                var listaSemExcluidos = objLayoutExportacao.ExportacaoCampos.Where(c => c.Acao != Acao.Excluir).ToList();
                objLayoutExportacao.LabelCamposLayoutExportacao = BLL.ExportacaoCampos.MontaStringExportacao(listaSemExcluidos);
                objLayoutExportacao.LabelQtdCamposLayoutExportacao = Convert.ToString(BLL.ExportacaoCampos.MontaStringExportacao(listaSemExcluidos).Length);
            }
            ViewBag.IncluindoCamposExportacao = 0;
            ViewBag.TipoCampo = Conversores.ToSelectList2(Modelo.Listas.TipoCampoExportacao(), "indice", "nome");
            ViewBag.DelimitadorCampoExportacao = Conversores.ToSelectList2(Modelo.Listas.DelimitadorCampoExportacao(), "indice", "nome");
            ViewBag.QualificadorCampoExportacao = Conversores.ToSelectList2(Modelo.Listas.QualificadorCampoExportacao(), "indice", "nome");
            ViewBag.tamanhoStrExp = Convert.ToString(BLL.ExportacaoCampos.MontaStringExportacao(objLayoutExportacao.ExportacaoCampos).Length + 1);
            return View("Cadastrar", objLayoutExportacao);
        }

        protected override void ValidarForm(LayoutExportacao obj)
        {
           
        }
    }
}