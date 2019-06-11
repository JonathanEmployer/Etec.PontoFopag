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
    public class AlocacaoController : IControllerPontoWeb<Alocacao>
    {
        [PermissoesFiltro(Roles = "Alocacao")]
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Alocacao bllAlocacao = new BLL.Alocacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(new Modelo.Alocacao());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Alocacao bllAlocacao = new BLL.Alocacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Alocacao> dados = bllAlocacao.GetAllList();
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

        [PermissoesFiltro(Roles = "AlocacaoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AlocacaoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "AlocacaoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "AlocacaoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Alocacao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "AlocacaoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Alocacao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "AlocacaoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Alocacao bllAlocacao = new BLL.Alocacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Alocacao Alocacao = bllAlocacao.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllAlocacao.Salvar(Acao.Excluir, Alocacao);
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
            if (ex.Message.Contains("FK_Funcionario_Alocacao"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(Alocacao obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Alocacao bllAlocacao = new BLL.Alocacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllAlocacao.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Alocacao");
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
            BLL.Alocacao bllAlocacao = new BLL.Alocacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Alocacao Alocacao = new Alocacao();
            Alocacao = bllAlocacao.LoadObject(id);
            if (id == 0)
            {
                Alocacao.Codigo = bllAlocacao.MaxCodigo();
            }
            return View("Cadastrar", Alocacao);
        }

        protected override void ValidarForm(Alocacao obj)
        {
            throw new NotImplementedException();
        }

        #region Eventos Consulta
        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Alocacao bllAlocacao = new BLL.Alocacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IList<Alocacao> lAlocacao = new List<Alocacao>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllAlocacao.GetIdPorCod(codigo).GetValueOrDefault();
                Alocacao Alocacao = bllAlocacao.LoadObject(id);
                if (Alocacao != null && Alocacao.Id > 0)
                {
                    lAlocacao.Add(Alocacao);
                }
            }

            if (lAlocacao.Count == 0)
            {
                lAlocacao = bllAlocacao.GetAllList();
                if (!String.IsNullOrEmpty(consulta))
                {
                    lAlocacao = lAlocacao.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Alocação";
            return View(lAlocacao);
        }

        public static int BuscaIdAlocacao(string Alocacao)
        {
            Alocacao f = new Alocacao();
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Alocacao bllAlocacao = new BLL.Alocacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);

            string codigo = Alocacao.Split('|')[0].Trim();
            int cod = 0;
            try
            {
                cod = Convert.ToInt32(codigo);
            }
            catch (Exception)
            {
                cod = 0;
            }
            int? idAlocacao = bllAlocacao.GetIdPorCod(cod);
            return idAlocacao.GetValueOrDefault();
        }
        #endregion
    }
}