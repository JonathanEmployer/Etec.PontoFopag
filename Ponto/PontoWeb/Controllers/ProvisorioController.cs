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
    public class ProvisorioController : IControllerPontoWeb<Provisorio>
    {
        Provisorio provisorioAntigo;

        [PermissoesFiltro(Roles = "Provisorio")]
        public override ActionResult Grid()
        {
            BLL.Provisorio bllProvisorio = new BLL.Provisorio(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            return View(new Modelo.Provisorio());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Provisorio bllProvisorio = new BLL.Provisorio(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Provisorio> dados = bllProvisorio.GetAllList();
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

        [PermissoesFiltro(Roles = "ProvisorioConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ProvisorioCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "ProvisorioAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ProvisorioCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Provisorio obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ProvisorioAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Provisorio obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "Provisorio")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.Provisorio bllProvisorio = new BLL.Provisorio(conn, Usuario.GetUsuarioPontoWebLogadoCache());
            Dictionary<string, string> erros = new Dictionary<string, string>();
            DateTime? ultimaData = null;
            Provisorio provisorio = bllProvisorio.LoadObject(id);

            provisorioAntigo = CarregaProvisorioAntesAlteracao(provisorio.Id, conn);
            Acao acao = Acao.Excluir;

            try
            {
                bool bVerificaBiletes = bllProvisorio.VerificaBilhete(provisorioAntigo.Dsfuncionarionovo, (DateTime)provisorioAntigo.Dt_inicial,
                                                                      (DateTime)provisorioAntigo.Dt_final, out ultimaData);

                if (!bVerificaBiletes)
                {
                    erros = bllProvisorio.Salvar(acao, provisorio);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Success = false, Erro = "Código provisório não pode ser deletado.\nJá existem bilhetes importados com este código." },
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        protected override ActionResult Salvar(Provisorio obj)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.Provisorio bllProvisorio = new BLL.Provisorio(conn, Usuario.GetUsuarioPontoWebLogadoCache());
            DateTime? ultimaData = null;
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    if (obj.Id == 0)
                        acao = Acao.Incluir;
                    else
                        acao = Acao.Alterar;

                    if (acao == Acao.Alterar)
                    {
                        //Alterar
                        provisorioAntigo = CarregaProvisorioAntesAlteracao(obj.Id, conn);
                        if (obj.Dsfuncionarionovo == provisorioAntigo.Dsfuncionarionovo &&
                            obj.Dt_inicial == (DateTime)provisorioAntigo.Dt_inicial &&
                            obj.Dt_final == (DateTime)provisorioAntigo.Dt_final &&
                            obj.Dsfuncionario == provisorioAntigo.Dsfuncionario)
                        {
                            erros = bllProvisorio.Salvar(acao, obj);
                        }
                        else
                        {
                            bllProvisorio.VerificaBilhete(provisorioAntigo.Dsfuncionarionovo, (DateTime)provisorioAntigo.Dt_inicial,
                                                                       (DateTime)provisorioAntigo.Dt_final, out ultimaData);

                            erros = VerificaUltimaData(erros, ultimaData, obj, acao);
                        }
                    }
                    else
                    {
                        //Incluir
                        erros = bllProvisorio.Salvar(acao, obj);
                    }

                    if (erros.Count > 0)
                    {
                        TrataErros(erros);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Provisorio");
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
            BLL.Provisorio bllProvisorio = new BLL.Provisorio(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Provisorio provisorio = new Provisorio();
            provisorio = bllProvisorio.LoadObject(id);
            if (id == 0)
            {
                provisorio.Codigo = bllProvisorio.MaxCodigo();
            }
            return View("Cadastrar", provisorio);
        }

        protected override void ValidarForm(Provisorio obj)
        {
            int idFunc = FuncionarioController.BuscaIdFuncionario(obj.NomeFuncionario);
            if (idFunc > 0)
            {
                obj.Dsfuncionario = obj.NomeFuncionario.Split('|').Count() > 0 ? obj.NomeFuncionario.Split('|').FirstOrDefault().Trim() : string.Empty;
            }
            else
            {
                ModelState["NomeFuncionario"].Errors.Add("Funcionário " + obj.NomeFuncionario + " não cadastrado!");
            }
        }

        private void TrataErros(Dictionary<string, string> erros)
        {
            Dictionary<string, string> erroLocais = new Dictionary<string, string>();

            erroLocais = erros.Where(x => x.Key.Equals("txtDt_final")).ToDictionary(x => x.Key, x => x.Value);
            if (erroLocais.Count > 0)
            {
                ModelState["Dt_final"].Errors.Add(string.Join(";", erroLocais.Select(x => x.Value).ToArray()));
            }

            erroLocais = erros.Where(x => x.Key.Equals("txtDsfuncionarionovo")).ToDictionary(x => x.Key, x => x.Value);
            if (erroLocais.Count > 0)
            {
                ModelState["Dsfuncionarionovo"].Errors.Add(string.Join(";", erroLocais.Select(x => x.Value).ToArray()));
            }

            erros = erros.Where(x => !x.Key.Equals("txtDt_final")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("txtDsfuncionarionovo")).ToDictionary(x => x.Key, x => x.Value);

            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                ModelState.AddModelError("CustomError", erro);
            }
        }

        private Dictionary<string, string> VerificaUltimaData(Dictionary<string, string> erros, DateTime? ultimaData, Provisorio provisorio, Acao acao)
        {
            BLL.Provisorio bllProvisorio = new BLL.Provisorio(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            if (ultimaData.HasValue)
            {
                if (ultimaData > provisorio.Dt_final)
                {
                    erros.Add("txtDt_final", "Já existem bilhetes importados com este código provisório na data " + ultimaData.Value.ToShortDateString()
                            + ".\nA data final do código provisório deve ser maior ou igual a data do ultimo bilhete importado com este código.");
                }
                else if (erros.Count == 0)
                {
                    erros = bllProvisorio.Salvar(acao, provisorio);
                }
            }
            else if (erros.Count == 0)
            {
                erros = bllProvisorio.Salvar(acao, provisorio);
            }
            return erros;
        }

        private Provisorio CarregaProvisorioAntesAlteracao(int idProvisorio, string conn)
        {
            BLL.Provisorio bllProvisorio = new BLL.Provisorio(conn, Usuario.GetUsuarioPontoWebLogadoCache());
            Provisorio provisorio = new Provisorio();
            provisorio = bllProvisorio.LoadObject(idProvisorio);

            return provisorio;
        }
    }
}