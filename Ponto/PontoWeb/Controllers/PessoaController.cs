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
    public class PessoaController : IControllerPontoWeb<Pessoa>
    {
        [PermissoesFiltro(Roles = "Pessoa")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Pessoa());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Pessoa bllPessoa = new BLL.Pessoa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Pessoa> dados = bllPessoa.GetAllList();
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

        [PermissoesFiltro(Roles = "PessoaConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "PessoaCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "PessoaAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "PessoaCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Pessoa obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "PessoaAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Pessoa obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "PessoaExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Pessoa bllPessoa = new BLL.Pessoa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Pessoa Pessoa = bllPessoa.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllPessoa.Salvar(Acao.Excluir, Pessoa);
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
            return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
        }

        protected override ActionResult Salvar(Pessoa obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Pessoa bllPessoa = new BLL.Pessoa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllPessoa.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Pessoa");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            BLL.Parametros bllparm = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Modelo.Parametros parm = new Parametros();
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Pessoa bllPessoa = new BLL.Pessoa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Pessoa Pessoa = new Pessoa();
            BLL.Parametros bllparm = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Modelo.Parametros parm = new Parametros();
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            Pessoa = bllPessoa.LoadObject(id);
            if (id == 0)
            {
                Pessoa.Codigo = bllPessoa.MaxCodigo();
            }
            return View("Cadastrar", Pessoa);
        }

        protected override void ValidarForm(Pessoa obj)
        {
            if (obj.TipoPessoa == 0)
            {
                if (!BLL.cwkFuncoes.ValidarCPF(obj.CNPJ_CPF))
                {
                    ModelState["CNPJ_CPF"].Errors.Add("CPF Inválido");
                }
            }
            else
            {
                if (!BLL.cwkFuncoes.ValidarCNPJ(obj.CNPJ_CPF))
                {
                    ModelState["CNPJ_CPF"].Errors.Add("CNPJ Inválido");
                }
            }

            string erros = "";
            if (!String.IsNullOrEmpty(obj.Email))
            {
                erros = BLL.cwkFuncoes.ValidarEmails(obj.Email);
            }

            if (!String.IsNullOrEmpty(erros))
            {
                ModelState["Email"].Errors.Add(erros);
            }
        }

        #region Eventos Consulta
        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Pessoa bllPessoa = new BLL.Pessoa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            IList<Pessoa> lPessoa = new List<Pessoa>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Pessoa Pessoa = bllPessoa.GetPessoaPorCodigo(codigo).FirstOrDefault();
                if (Pessoa != null && Pessoa.Id > 0)
                {
                    lPessoa.Add(Pessoa);
                }
            }

            if (lPessoa.Count == 0)
            {
                lPessoa = bllPessoa.GetListPessoaPorNome(consulta);
            }
            ViewBag.Title = "Pesquisar Pessoa";
            return View(lPessoa);
        }

        public static int BuscaIdPessoa(string Pessoa)
        {
            Pessoa f = new Pessoa();
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Pessoa bllPessoa = new BLL.Pessoa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            string codigo = Pessoa.Split('|')[0].Trim();
            int cod = 0;
            try
            {
                cod = Convert.ToInt32(codigo);
            }
            catch (Exception)
            {
                cod = 0;
            }
            f = bllPessoa.GetPessoaPorCodigo(cod).FirstOrDefault();
            return f.Id;
        }
        #endregion
    }
}