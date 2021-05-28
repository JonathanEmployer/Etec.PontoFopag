using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class RegistradorMassaController : Controller
    {
        private UsuarioPontoWeb _user = Usuario.GetUsuarioPontoWebLogadoCache();

        // GET: RegistradorMassa
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grid()
        {
            return View(new Modelo.REP());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.REP bllREP = new BLL.REP(_user.ConnectionString, _user);
                List<Modelo.REP> dados = bllREP.GetAllListRegMassa();
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

        [PermissoesFiltro(Roles = "RegistradorMassaConsultar")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return getPagina(id);
        }

        [PermissoesFiltro(Roles = "RegistradorMassaCadastrar")]
        public ActionResult Cadastrar()
        {
            int id = 0;
            return getPagina(id);
        }

        [PermissoesFiltro(Roles = "RegistradorMassaCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(REP rep)
        {
            return SalvarRegMassa(rep);
        }

        private ActionResult getPagina(int id)
        {
            BLL.REP bllRep = new BLL.REP(_user.ConnectionString, _user);
            REP rep = bllRep.LoadObject(id);
          
            if (id == 0)
            {
                rep.Codigo = bllRep.MaxCodigo();
                rep.CodigoLocal = rep.Codigo;
                rep.UltimoNSR = 0;
                rep.TipoIP = 0;
                rep.DataInicioImportacao = DateTime.Now;
                rep.IdTimeZoneInfo = "E. South America Standard Time";
                rep.Acao = Acao.Incluir;

                rep.IdEquipamentoTipoBiometria = 4;
                rep.QtdDigitos = 8;
                rep.TempoRequisicao = 60;

            }
            else
            {
                BLL.BilhetesImp BLLBilhetesImp = new BLL.BilhetesImp(_user.ConnectionString, _user);
                rep.UltimoNSR = BLLBilhetesImp.GetUltimoNSRRep(rep.NumRelogio);
                rep.Acao = Acao.Alterar;
            }
            return View("Cadastrar", rep);
        }

        [PermissoesFiltro(Roles = "RegistradorMassaAlterar")]
        public ActionResult Alterar(int id)
        {
            return getPagina(id);
        }

        [PermissoesFiltro(Roles = "RegistradorMassaAlterar")]
        [HttpPost]
        public ActionResult Alterar(REP rep)
        {
            return SalvarRegMassa(rep);
        }

        [PermissoesFiltro(Roles = "RegistradorMassaExcluir")]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            BLL.REP bllRep = new BLL.REP(_user.ConnectionString, _user);
            REP rep = bllRep.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllRep.Salvar(Acao.Excluir, rep);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                        RemoveRepCentralCliente(rep);
                 }
                catch (Exception)
                {
                }

                return Json(new { Success = true, Erro = " " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private ActionResult SalvarRegMassa(REP rep)
        {
            BLL.REP bllRep = new BLL.REP(_user.ConnectionString, _user);

            Modelo.REP repAntigo = bllRep.LoadObject(rep.Id);
            rep.RegistradorEmMassa = true;
            rep.ImportacaoAtivada = true;

            ValidaEmpresa(rep);
            
            ValidarForm(rep);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    if (rep.Id == 0)
                        acao = Acao.Incluir;
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();

                    erros = bllRep.Salvar(acao, rep);
                    if (erros.Count > 0)
                    {
                        Dictionary<string, string> customError = new Dictionary<string, string>();
                        foreach (KeyValuePair<string, string> item in erros)
                        {
                            if (ModelState.ContainsKey(item.Key))
                            {
                                ModelState[item.Key].Errors.Add(item.Value);
                            }
                            else
                            {
                                customError.Add(item.Key, item.Value);
                            }
                        }
                        if (customError.Count > 0)
                        {
                            string erro = string.Join(";", customError.Select(x => x.Key + "=" + x.Value).ToArray());
                            ModelState.AddModelError("CustomError", erro);
                        }

                    }
                    else
                    {
                        try
                        {
                            AdicionaAlteraRepCentralCliente(rep, repAntigo);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        return RedirectToAction("Grid", "RegistradorMassa");
                    }
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("CK_REP_NumSerie_IsNumber"))
                    {
                        ModelState.AddModelError("NumSerie", "Para o número de série são permitidos apenas números. Remova espaços e caracteres não numéricos");
                    }
                    else
                    {
                        BLL.cwkFuncoes.LogarErro(ex);
                        ModelState.AddModelError("CustomError", ex.Message);
                    }

                }
            }
            return View("Cadastrar", rep);
        }

        private void ValidaEmpresa(REP rep)
        {
            if (!String.IsNullOrEmpty(rep.empresaNome))
            {
                BLL.Empresa bllEmp = new BLL.Empresa(_user.ConnectionString, _user);
                Empresa e = new Empresa();
                int idEmpresa;
                string empresa = rep.empresaNome.Split('|')[0].Trim();
                if (int.TryParse(empresa, out idEmpresa))
                {
                    e = bllEmp.LoadObjectByCodigo(idEmpresa);
                }
                if (e != null && e.Id > 0)
                {
                    rep.IdEmpresa = e.Id;
                    rep.ObjEmpresa = e;
                }
                else
                {
                    ModelState["empresaNome"].Errors.Add("Empresa " + empresa + " não cadastrada!");
                }
            }
        }
        protected void ValidarForm(REP obj)
        {
            BLL.EquipamentoHomologado bllEquipamentoHomologado = new BLL.EquipamentoHomologado(_user.ConnectionString, _user);
            BLL.REP bllRep = new BLL.REP(_user.ConnectionString, _user);
            obj.EquipamentoHomologado = bllEquipamentoHomologado.LoadObject(obj.IdEquipamentoHomologado);
          
        }

        private static void AdicionaAlteraRepCentralCliente(REP rep, REP repAnterior)
        {
            using (var db = new CentralCliente.CENTRALCLIENTEEntities())
            {
                CentralCliente.Rep repCC = null;
                repCC = BuscaRepCentralCliente(repAnterior, db);
                if (repCC == null)
                {
                    repCC = BuscaRepCentralCliente(rep, db);
                }

                if (repCC == null)
                {
                    repCC = new CentralCliente.Rep();
                    repCC.idCliente = db.Cliente.Where(x => x.Entidade.CNPJ_CPF.Replace("-", "").Replace(".", "").Replace("/", "") == rep.ObjEmpresa.Cnpj.Replace("-", "").Replace(".", "").Replace("/", "")).FirstOrDefault().ID;
                }
                repCC.Ativo = rep.ImportacaoAtivada;
                repCC.codigo = rep.Codigo;
                repCC.IdRep = rep.Id;

                repCC.local = rep.Local;
                repCC.numrelogio = rep.NumRelogio;
                repCC.relogio = rep.Relogio;
                repCC.idEmpresa = rep.IdEmpresa;
                repCC.UltimoNSR = Convert.ToInt32(rep.UltimoNSR);
                repCC.tempoDormir = rep.TempoRequisicao;
                repCC.DataInicioImportacao = rep.DataInicioImportacao;
                repCC.numSerie = rep.NumSerie;
                repCC.ServicoComunicador = rep.EquipamentoHomologado.ServicoComunicador;

                repCC.RegistradorEmMassa = rep.RegistradorEmMassa;
                repCC.CrachaAdm = rep.CrachaAdm;

                if (repCC.Id > 0)
                    db.Entry(repCC).State = EntityState.Modified;
                else
                    db.Rep.Add(repCC);

                db.SaveChanges();
            }
        }

        private static void RemoveRepCentralCliente(REP rep)
        {
            using (var db = new CentralCliente.CENTRALCLIENTEEntities())
            {
                CentralCliente.Rep repCC = BuscaRepCentralCliente(rep, db);
                if (repCC != null)
                {
                    db.Rep.Remove(repCC);
                    db.SaveChanges();
                }
            }
        }

        private static CentralCliente.Rep BuscaRepCentralCliente(REP rep, CentralCliente.CENTRALCLIENTEEntities db)
        {
            CentralCliente.Rep repCC = db.Rep.Where(x => x.numSerie == rep.NumSerie).FirstOrDefault();
            return repCC;
        }


        [PermissoesFiltro(Roles = "SituacaoRegistradorMassaConsultar")]
        [OutputCache(Duration = 50, VaryByParam = "none", VaryByCustom = "User")]
        public ActionResult SituacaoRegMassa()
        {
            return View();
        }

        [PermissoesFiltro(Roles = "SituacaoRegistradorMassaConsultar")]
        [OutputCache(Duration = 50, VaryByParam = "none", VaryByCustom = "User")]
        public ActionResult SituacaoRegMassaAtual()
        {
            BLL.REP bllRep = new BLL.REP(_user.ConnectionString, _user);
            List<Modelo.Proxy.RepSituacao> situacaos = bllRep.VerificarSituacaoRegMassa(3600);
            ViewBag.UltimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return PartialView(situacaos);
        }
    }
}