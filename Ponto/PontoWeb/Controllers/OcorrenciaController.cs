using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class OcorrenciaController : IControllerPontoWeb<Ocorrencia>
    {
        [PermissoesFiltro(Roles = "Ocorrencia")]
        public override ActionResult Grid()
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            return View(new Modelo.Ocorrencia());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Ocorrencia> dados = bllOcorrencia.GetAllList(true);
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

        [PermissoesFiltro(Roles = "OcorrenciaConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "OcorrenciaCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "OcorrenciaAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "OcorrenciaCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Ocorrencia obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "OcorrenciaAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Ocorrencia obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "OcorrenciaExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            Ocorrencia ocorrencia = bllOcorrencia.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllOcorrencia.Salvar(Acao.Excluir, ocorrencia);
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

        protected override ActionResult Salvar(Ocorrencia obj)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
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
                    erros = bllOcorrencia.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Ocorrencia");
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
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(usr.ConnectionString, usr);
            Ocorrencia ocorrencia = new Ocorrencia();
            if (id == 0)
            {
                ocorrencia.Codigo = bllOcorrencia.MaxCodigo();
                ocorrencia.TipoAbono = 0;
                ocorrencia.Ativo = true;
                ocorrencia.OcorrenciaRestricao = new List<OcorrenciaRestricao>();
            }
            else
            {
                ocorrencia = bllOcorrencia.LoadObject(id);
                BLL.OcorrenciaRestricao bllOcorrenciaRestricao = new BLL.OcorrenciaRestricao(usr.ConnectionString, usr);
                ocorrencia.OcorrenciaRestricao = bllOcorrenciaRestricao.GetAllListByOcorrencias(new List<int>() { id });
            }
            
            return View("Cadastrar", ocorrencia);
           
        }

        protected override void ValidarForm(Ocorrencia obj)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(usr.ConnectionString, usr);
            IList<Ocorrencia> lOcorrencia = new List<Ocorrencia>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Ocorrencia ocorrencia = bllOcorrencia.LoadObjectByCodigo(codigo, true);
                if (ocorrencia != null && ocorrencia.Id > 0)
                {
                    lOcorrencia.Add(ocorrencia);
                }
            }

            if (lOcorrencia.Count == 0)
            {
                lOcorrencia = bllOcorrencia.GetAllListConsultaEvento(true);
                if (!String.IsNullOrEmpty(consulta))
                {
                    lOcorrencia = lOcorrencia.Where(p => p.Descricao.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Ocorrência";
            return View(lOcorrencia);
        }

        public static int BuscaIdOcorrencia(string ocorrencia)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Ocorrencia bllOcorrencia = new BLL.Ocorrencia(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            int id = 0;
            try
            {
                if (!String.IsNullOrEmpty(ocorrencia))
                {

                    string cod = ocorrencia.Split('|')[0].Trim();
                    int codigo = Convert.ToInt32(cod);
                    Ocorrencia objOcorrencia = new Ocorrencia();
                    objOcorrencia = bllOcorrencia.LoadObjectByCodigo(codigo, false);
                    if (objOcorrencia.Id > 0) id = objOcorrencia.Id;
                    else id = 0;
                }
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public ActionResult AdicionaNovaRestricao(int index, int tipoRestricao, string restricao, int idOcorrencia)
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

                var ocorrencia = new Modelo.Ocorrencia();
                ocorrencia.OcorrenciaRestricao = new List<Modelo.OcorrenciaRestricao>();
                ocorrencia.OcorrenciaRestricao.Add(
                    new OcorrenciaRestricao()
                    {
                        IdEmpresa = idEmpresa,
                        IdContrato = idContrato,
                        DescEmpresa = restricao,
                        IdOcorrencia = idOcorrencia
                    }
                );
                string novo = RenderViewToString("AdicionaNovaRestricao", ocorrencia);
                novo = novo.Replace("OcorrenciaRestricao_0__", "OcorrenciaRestricao_" + index + "__");
                novo = novo.Replace("OcorrenciaRestricao[0].", "OcorrenciaRestricao[" + index + "].");
                return Json(new { Success = true, HTML = novo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Erro = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}