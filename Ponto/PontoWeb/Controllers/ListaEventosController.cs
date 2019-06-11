using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ListaEventosController : IControllerPontoWeb<ListaEventos>
    {
        [PermissoesFiltro(Roles = "ListaEventos")]
        public override ActionResult Grid()
        {
            return View(new Modelo.ListaEventos());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
                BLL.ListaEventos bllListaEventos = new BLL.ListaEventos(conn, usr);
                List<Modelo.ListaEventos> dados = bllListaEventos.GetAllList();
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

        [PermissoesFiltro(Roles = "ListaEventosConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ListaEventosCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "ListaEventosAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ListaEventosCadastrar")]
        public ActionResult CopiarListaEventos(int idListaEventos)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.ListaEventos bllListaEventos = new BLL.ListaEventos(conn, usr);
            ListaEventos listaEventosCopia = new ListaEventos();
            ListaEventos listaEventosOriginal = new ListaEventos();
            listaEventosOriginal = bllListaEventos.LoadObject(idListaEventos);

            BLL.ListaEventosEvento bllListaEventosEvento = new BLL.ListaEventosEvento(conn, usr);
            List<ListaEventosEvento> listaEventosEvento = bllListaEventosEvento.GetAllPorListaEventos(listaEventosOriginal.Id);
            listaEventosCopia.IdEventosSelecionados = String.Join(",", listaEventosEvento.Select(s => s.Idf_Evento).ToList());
            listaEventosCopia.Codigo = bllListaEventos.MaxCodigo();

            return View("Cadastrar", listaEventosCopia);
        }

        [PermissoesFiltro(Roles = "ListaEventosCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(ListaEventos obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ListaEventosAlterar")]
        [HttpPost]
        public override ActionResult Alterar(ListaEventos obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ListaEventosExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.ListaEventos bllListaEventos = new BLL.ListaEventos(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            ListaEventos ListaEventos = bllListaEventos.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllListaEventos.Salvar(Acao.Excluir, ListaEventos);
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

        private ActionResult TratarErros(Exception ex)
        {
            if (ex.Message.Contains("FK_Funcionario_Alertas"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        [PermissoesFiltro(Roles = "ListaEventosCadastrar")]
        [HttpPost]
        public ActionResult CopiarListaEventos(ListaEventos obj)
        {
            return Salvar(obj);
        }

        protected override ActionResult Salvar(ListaEventos obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            var conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.ListaEventos bllListaEventos = new BLL.ListaEventos(conn, usr);
            ValidarForm(obj);
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
                    erros = bllListaEventos.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        foreach (KeyValuePair<string, string> erro in erros)
                        {
                            ModelState.AddModelError(erro.Key, erro.Value);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Grid", "ListaEventos");
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
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.ListaEventos bllListaEventos = new BLL.ListaEventos(conn, usr);
            ListaEventos listaEventos = new ListaEventos();
            listaEventos = bllListaEventos.LoadObject(id);
            if (id == 0)
            {
                listaEventos.Codigo = bllListaEventos.MaxCodigo();
                listaEventos.Des_Lista_Eventos = "";
                listaEventos.IdEventosSelecionados = "";
                listaEventos.IdEventosSelecionados_Ant = "";
            }
            else
            {
                BLL.ListaEventosEvento bllListaEventosEvento = new BLL.ListaEventosEvento(conn, usr);
                List<ListaEventosEvento> listaEventosEvento = bllListaEventosEvento.GetAllPorListaEventos(listaEventos.Id);
                listaEventos.IdEventosSelecionados = String.Join(",", listaEventosEvento.Select(s => s.Idf_Evento).ToList());
                listaEventos.IdEventosSelecionados_Ant = listaEventos.IdEventosSelecionados;
            }
            return View("Cadastrar", listaEventos);
        }

        protected override void ValidarForm(ListaEventos obj)
        {
        }

        #region Métodos auxiliares

        #endregion
    }
}
