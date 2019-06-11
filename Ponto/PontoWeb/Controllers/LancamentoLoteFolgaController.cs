using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using ProgressReporting.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace PontoWeb.Controllers
{
    public class LancamentoLoteFolgaController : IControllerPontoWeb<LancamentoLote>
    {

        [PermissoesFiltro(Roles = "LancamentoLoteFolga")]
        public override ActionResult Grid()
        {
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.Folga};
            ViewBag.Title = "Lançamento de Folga em Lote";
            ViewBag.Controller = "LancamentoLoteFolga";
            return View("../LancamentoLote/Grid");
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<TipoLancamento> lstLancamento = new List<TipoLancamento>() { TipoLancamento.Folga };
                List<Modelo.LancamentoLote> dados = bllLancamentoLote.GetAllListTipoLancamento(lstLancamento);
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

        [PermissoesFiltro(Roles = "LancamentoLoteFolgaConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteFolgaCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteFolgaAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteFolgaCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteFolgaAlterar")]
        [HttpPost]
        public override ActionResult Alterar(LancamentoLote obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "LancamentoLoteFolgaExcluir")]
        public override ActionResult Excluir(int id)
        {
            LancamentoLoteController llC = new LancamentoLoteController();
            return llC.Excluir(id);
        }

        public bool ExcluirLote(int id, out string erro, out LancamentoLote ll)
        {
            string conexao = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb userpw = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(conexao, userpw);
            ll = bllLancamentoLote.LoadObject(id);
            ll.DataLancamentoAnt = ll.DataLancamento;
            bool retorno = true;
            erro = string.Empty;
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllLancamentoLote.Salvar(Acao.Excluir, ll);
                if (erros.Count > 0)
                {
                    erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    retorno = false;
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                erro = ex.Message;
                retorno = false;
            }
            return retorno;
        }

        protected override ActionResult Salvar(LancamentoLote obj)
        {
            PontoWeb.Models.cw_usuario usuario = Usuario.GetUsuarioLogadoCache();
            UsuarioPontoWeb userpw = Usuario.GetUsuarioPontoWebLogadoCache();
            //ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    string erro = String.Empty;
                    LancamentoLoteController llc = new LancamentoLoteController();
                    bool salvou = llc.SalvarLote(obj, out erro);
                    if (!salvou)
                    {
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }

            LancamentoLoteController.SetaDadosPadrao(obj, usuario.ConnectionStringDecrypt, userpw);
            ViewBag.Title = "Lançamento de Folga em Lote";
            ViewBag.Controller = "LancamentoLoteFolga";

            return View("../LancamentoLote/Cadastrar", obj);
        }

        public static Dictionary<string, string> SalvarLote(LancamentoLote obj)
        {
            Acao acao;
            PontoWeb.Models.cw_usuario usuario = Usuario.GetUsuarioLogadoCache();
            UsuarioPontoWeb userpw = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.LancamentoLoteFuncionario bllLancamentoLoteFunc = new BLL.LancamentoLoteFuncionario(usuario.ConnectionStringDecrypt, userpw);
            BLL.LancamentoLote bllLancamentoLote = new BLL.LancamentoLote(usuario.ConnectionStringDecrypt, userpw);
            List<int> idsFuncionariosSelecionados = new List<int>();
            try
            {
                if (!String.IsNullOrEmpty(obj.idSelecionados))
                {
                    idsFuncionariosSelecionados = obj.idSelecionados.Split(',').ToList().Select(s => int.Parse(s)).ToList();
                }
                else
                {
                    throw new Exception("Nenhum funcionário informado para o lote de folga");
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw new Exception("Erro ao carregar a lista de funcionários selecionados");
            }

            acao = new Acao();
            if (obj.Id == 0)
            {
                acao = Acao.Incluir;
                obj.LancamentoLoteFuncionarios = new List<LancamentoLoteFuncionario>();
                obj.Codigo = bllLancamentoLote.MaxCodigo();
            }
            else
            {
                acao = Acao.Alterar;
                obj.LancamentoLoteFuncionarios = bllLancamentoLoteFunc.GetListWhere("and idLancamentoLote = " + obj.Id.ToString());
                //Seta todo mundo para ser excluido
                obj.LancamentoLoteFuncionarios.ToList().ForEach(i => i.Acao = Acao.Excluir);
                //os que foram selecionados seta para alterar
                obj.LancamentoLoteFuncionarios.Where(w => idsFuncionariosSelecionados.Contains(w.IdFuncionario)).ToList().ForEach(f => { f.Acao = Acao.Alterar; });
                //Retiro dos selecionados os que já existiam
                idsFuncionariosSelecionados.RemoveAll(x => obj.LancamentoLoteFuncionarios.Select(s => s.IdFuncionario).Contains(x));
            }

            //Adiciona os novos funcionarios selecionados
            foreach (int idFunc in idsFuncionariosSelecionados)
            {
                LancamentoLoteFuncionario fpf = new LancamentoLoteFuncionario();
                fpf.IdLancamentoLote = obj.Id;
                fpf.IdFuncionario = idFunc;
                fpf.Acao = Acao.Incluir;
                fpf.Codigo = bllLancamentoLoteFunc.MaxCodigo();
                if (obj.LancamentoLoteFuncionarios == null)
                {
                    obj.LancamentoLoteFuncionarios = new List<LancamentoLoteFuncionario>();
                }
                obj.LancamentoLoteFuncionarios.Add(fpf);
            }
            Dictionary<string, string> erros = bllLancamentoLote.Salvar(acao, obj);
            return erros;
        }

        protected override ActionResult GetPagina(int id)
        {
            ViewBag.Title = "Lançamento de Folga em Lote";
            ViewBag.Controller = "LancamentoLoteFolga";
            LancamentoLoteController llC = new LancamentoLoteController();
            LancamentoLote ll = llC.GetPagina(id, TipoLancamento.Folga);
            return View("../LancamentoLote/Cadastrar", ll);
        }

        protected override void ValidarForm(LancamentoLote obj)
        {
            throw new NotImplementedException();
        }
    }
}