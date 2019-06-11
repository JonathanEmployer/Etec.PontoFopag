using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ClassificacaoController : IControllerPontoWeb<Classificacao>
    {
        [PermissoesFiltro(Roles = "Classificacao")]
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Classificacao bllClassificacao = new BLL.Classificacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(new Modelo.Classificacao());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Classificacao bllClassificacao = new BLL.Classificacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Classificacao> dados = bllClassificacao.GetAllList();
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

        [PermissoesFiltro(Roles = "ClassificacaoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ClassificacaoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "ClassificacaoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ClassificacaoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Classificacao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ClassificacaoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Classificacao obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ClassificacaoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Classificacao bllClassificacao = new BLL.Classificacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Classificacao Classificacao = bllClassificacao.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllClassificacao.Salvar(Acao.Excluir, Classificacao);
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
            if (ex.Message.Contains("FK_Funcionario_Classificacao"))
                return Json(new
                {
                    Success = false,
                    Erro = "Este registro não pode ser excluído pois esta vinculado com um funcionário cadastrado."
                },
                                  JsonRequestBehavior.AllowGet);
            else
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(Classificacao obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Classificacao bllClassificacao = new BLL.Classificacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllClassificacao.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Classificacao");
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
            BLL.Classificacao bllClassificacao = new BLL.Classificacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Classificacao Classificacao = new Classificacao();
            Classificacao = bllClassificacao.LoadObject(id);
            if (id == 0)
            {
                Classificacao.Codigo = bllClassificacao.MaxCodigo();
            }
            return View("Cadastrar", Classificacao);
        }

        protected override void ValidarForm(Classificacao obj)
        {
            throw new NotImplementedException();
        }


        #region Eventos Consulta
        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Classificacao bllClass = new BLL.Classificacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IList<Classificacao> lClassificacao = new List<Classificacao>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllClass.GetIdPorCod(codigo).GetValueOrDefault();
                Classificacao Classificacao = bllClass.LoadObject(id);
                if (Classificacao != null && Classificacao.Id > 0)
                {
                    lClassificacao.Add(Classificacao);
                }
            }

            if (lClassificacao.Count == 0)
            {
                lClassificacao = bllClass.GetAllList();
                if (!String.IsNullOrEmpty(consulta))
                {
                    lClassificacao = lClassificacao.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Classificação";
            return View(lClassificacao);
        }

        public static int BuscaIdClassificacao(string Classificacao)
        {
            Classificacao c = new Classificacao();
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Classificacao bllClassificacao = new BLL.Classificacao(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);

            string codigo = Classificacao.Split('|')[0].Trim();
            int cod = 0;
            try
            {
                cod = Convert.ToInt32(codigo);
            }
            catch (Exception)
            {
                cod = 0;
            }
            int? idClassificacao = bllClassificacao.GetIdPorCod(cod);
            return idClassificacao.GetValueOrDefault();
        }
        #endregion
    }
}
