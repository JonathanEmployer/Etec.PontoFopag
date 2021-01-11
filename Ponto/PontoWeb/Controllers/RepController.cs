using Modelo;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Modelo.Utils;
using CentralCliente;

namespace PontoWeb.Controllers
{
    public class RepController : Controller
    {
        private UsuarioPontoWeb _user = BLLWeb.Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "Rep")]
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
                List<Modelo.REP> dados = bllREP.GetAllList();
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

        [PermissoesFiltro(Roles = "RepExcluir")]
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
                    if (rep.Relogio == 17)
                    {
                        RemoveRepCentralCliente(rep);
                    }
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

        [PermissoesFiltro(Roles = "RepConsultar")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "RepAlterar")]
        public ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "RepAlterar")]
        [HttpPost]
        public ActionResult Alterar(REP rep)
        {
            return SalvarRep(rep);
        }

        [PermissoesFiltro(Roles = "RepCadastrar")]
        public ActionResult Cadastrar()
        {
            int id = 0;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "RepCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(REP rep)
        {
            return SalvarRep(rep);
        }

        private ActionResult SalvarRep(REP rep)
        {
            BLL.REP bllRep = new BLL.REP(_user.ConnectionString, _user);

            Modelo.REP repAntigo = bllRep.LoadObject(rep.Id);
            string senhaAntiga = repAntigo == null ? String.Empty : repAntigo.Senha;

            BLL.EquipamentoTipoBiometria bllEquipamentoTipoBiometria = new BLL.EquipamentoTipoBiometria(_user.ConnectionString, _user);
            rep.ItensEquipamentoTipoBiometria = bllEquipamentoTipoBiometria.GetAllList(rep.IdEquipamentoHomologado);
            Valida373(rep);

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

                    if (rep.Senha == "#SeNhAnAoAlTeRaDa#")
                    {
                        rep.Senha = senhaAntiga;
                    }

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
                            //Se o Fabricante é Ahgora insere rep no Central do cliente para funcionar o Serviço de Integração Rep HTTP
                            // ou se for Dimep Printpoint III também insere para funcionar a integração com o ServCom
                            if (rep.Relogio == 17 || rep.EquipamentoHomologado.ServicoComunicador)
                            {
                                AdicionaAlteraRepCentralCliente(rep, repAntigo);
                            }
                            //Se era de um fabricante ahgora e "Deixou" de ser, remove rep da Central do cliente para parar o Serviço de Integração Rep HTTP
                            //ou se for Dimep Printpoint III também executa para funcionar a integração com o ServCom
                            else if ((repAntigo.Relogio == 17 || rep.Relogio != 17) || (repAntigo.EquipamentoHomologado.ServicoComunicador || !rep.EquipamentoHomologado.ServicoComunicador))
                            {
                                RemoveRepCentralCliente(repAntigo);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        return RedirectToAction("Grid", "Rep");
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
            GetEquipamentosPortaria373();
            return View("Cadastrar", rep);
        }

        private void Valida373(REP rep)
        {
            if (rep.Portaria373)
            {
                if (rep.IdEquipamentoHomologado373 > 0)
                {
                    BLL.EquipamentoHomologado bllEquipamentoHomologado = new BLL.EquipamentoHomologado(_user.ConnectionString, _user);
                    var equipHomologado = bllEquipamentoHomologado.LoadObject(rep.IdEquipamentoHomologado373);
                    rep.modeloNome = equipHomologado.nomeModelo;
                    rep.IdEquipamentoHomologado = equipHomologado.Id;
                    rep.Relogio = Convert.ToInt16(equipHomologado.identificacaoRelogio);
                }
                else
                {
                    ModelState.AddModelError("IdEquipamentoHomologado373", "Relógio não foi selecionado");
                }
            }
            else
            {
                rep.IdEquipamentoHomologado373 = rep.IdEquipamentoHomologado;
                ModelState.Remove("IdEquipamentoHomologado373");
            }
        }

        private void AdicionaAlteraRepCentralCliente(REP rep, REP repAnterior)
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

                CentralCliente.ComunicadorServico comServ = new CentralCliente.ComunicadorServico();
                if (!string.IsNullOrEmpty(rep.ServicoPontoCom))
                {
                    try
                    {
                        comServ = BLL.ServicoPontoCom.GetComunicadorServico(rep.ServicoPontoCom, db);
                        if (comServ != null && comServ.Id > 0)
                        {
                            repCC.IdComunicadorServico = comServ.Id;
                        }
                        else
                        {
                            repCC.IdComunicadorServico = null;
                        }
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        CentralCliente.CentroServico centroServico = db.CentroServico.Where(w => w.DataBaseName == _user.DataBase).FirstOrDefault();
                        if (centroServico != null && centroServico.Id > 0)
                        {
                            repCC.IdCentroServico = centroServico.Id;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    rep.ServicoPontoCom = BLL.ServicoPontoCom.GetDescricaoServicoComunicador(repAnterior.NumSerie, db);
                    comServ = BLL.ServicoPontoCom.GetComunicadorServico(rep.ServicoPontoCom, db);
                    repCC.IdComunicadorServico = null;
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
                if (repCC.Id > 0)
                    db.Entry(repCC).State = EntityState.Modified;
                else
                    db.Rep.Add(repCC);

                db.SaveChanges();
                if (comServ != null && comServ.Id > 0)
                {
                    new BLL.RabbitMQ.RabbitMQ().EnviarMensagemServicoPontoCom(comServ.ComunicadorServidor.MAC, Enumeradores.PontoComFuncoes.Atualizar);
                }
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

        private ActionResult GetPagina(int id)
        {
            BLL.REP bllRep = new BLL.REP(_user.ConnectionString, _user);
            REP rep = new REP();
            rep = bllRep.LoadObject(id);
            BLL.EquipamentoHomologado bllEquipamentoHomologado = new BLL.EquipamentoHomologado(_user.ConnectionString, _user);
            if (rep.Portaria373)
            {
                rep.EquipamentoHomologado = bllEquipamentoHomologado.LoadObject(rep.IdEquipamentoHomologado);
            }
            else
            {
                rep.EquipamentoHomologado = bllEquipamentoHomologado.LoadByCodigoModelo(rep.NumSerie);
            }
            rep.IdEquipamentoHomologado373 = rep.EquipamentoHomologado.Id;

            BLL.EquipamentoTipoBiometria bllEquipamentoTipoBiometria = new BLL.EquipamentoTipoBiometria(_user.ConnectionString, _user);
            rep.ItensEquipamentoTipoBiometria = bllEquipamentoTipoBiometria.GetAllList(rep.IdEquipamentoHomologado);

            bool integraComunicador = IntegraComunicadorRep(rep.EquipamentoHomologado);
            if (integraComunicador)
            {
                rep.IntegraComunicador = true;
            }
            else
            {
                rep.IntegraComunicador = false;
                rep.modeloNome = rep.EquipamentoHomologado.nomeModelo + " - Rep não possui integração com o Comunicador Pontofopag";
            }
            if (rep.TempoRequisicao < 20)
            {
                rep.TempoRequisicao = 60;
            }
            if (rep.DataInicioImportacao == null || rep.DataInicioImportacao < DateTime.MinValue)
            {
                rep.DataInicioImportacao = DateTime.Now;
            }
            if (id == 0)
            {
                rep.Codigo = bllRep.MaxCodigo();
                rep.CodigoLocal = rep.Codigo;
                rep.UltimoNSR = 0;
                rep.TipoIP = 0;
                rep.DataInicioImportacao = DateTime.Now;
                rep.IdTimeZoneInfo = "E. South America Standard Time";
                rep.Acao = Acao.Incluir;
            }
            else
            {
                rep.Senha = "#SeNhAnAoAlTeRaDa#";
                BLL.BilhetesImp BLLBilhetesImp = new BLL.BilhetesImp(_user.ConnectionString, _user);
                rep.UltimoNSR = BLLBilhetesImp.GetUltimoNSRRep(rep.NumRelogio);
                if (String.IsNullOrEmpty(rep.IdTimeZoneInfo))
                {
                    rep.IdTimeZoneInfo = "E. South America Standard Time";
                }
                if (rep.Portaria373)
                {
                    rep.IdEquipamentoHomologado373 = rep.IdEquipamentoHomologado;
                }
                rep.Acao = Acao.Alterar;
                rep.ServicoPontoCom = BLL.ServicoPontoCom.GetDescricaoServicoComunicador(rep.NumSerie);
            }

            GetEquipamentosPortaria373();

            return View("Cadastrar", rep);
        }

        private void GetEquipamentosPortaria373()
        {
            List<Modelo.Utils.ItensCombo> equipamentosPortaria373Combo = new List<Modelo.Utils.ItensCombo>();
            BLL.EquipamentoHomologado bllEquipamentoHomologado373 = new BLL.EquipamentoHomologado(_user.ConnectionString, _user);
            var equipamentoPortaria373 = bllEquipamentoHomologado373.GetAllListPortaria373();
            equipamentoPortaria373.ForEach(f => equipamentosPortaria373Combo.Add(new ItensCombo() { Id = f.Id.ToString(), Descricao = f.nomeModelo }));
            ViewBag.equipamentoPortaria373 = equipamentosPortaria373Combo;
        }

        private void ValidaEmpresa(REP rep)
        {
            if (!String.IsNullOrEmpty(rep.empresaNome))
            {
                BLL.Empresa bllEmp = new BLL.Empresa(_user.ConnectionString, _user);
                Modelo.Empresa e = new Modelo.Empresa();
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
            if (obj.Relogio != 17)
            {
                if ((obj.TipoIP == 0) && !BLL.cwkFuncoes.ValidaIP(obj.IP))
                {
                    ModelState["IP"].Errors.Add("Ip inválido!");
                }
                else
                {
                    if (!String.IsNullOrEmpty(obj.IP) && obj.IP != "000.000.000.000")
                    {
                        List<Modelo.REP> lista = bllRep.VerificarIpEntreRep(obj.IP, obj.Id);
                        if (lista != null && lista.Count > 0)
                        {
                            ModelState["IP"].Errors.Add("O Ip informado já está sendo utilizado no relógio de código " + lista.FirstOrDefault().Codigo + ", localizado em " + lista.FirstOrDefault().Local);
                        } 
                    }
                }
            }

        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta, String filtro)
        {
            BLL.REP bllRep = new BLL.REP(_user.ConnectionString, _user);
            IList<Modelo.REP> lREP = new List<Modelo.REP>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Modelo.REP rep = bllRep.LoadObjectByCodigo(codigo);
                if (rep != null && rep.Id > 0)
                {
                    lREP.Add(rep);
                }
            }

            if (lREP.Count == 0)
            {
                lREP = bllRep.GetAllList();
                if (!String.IsNullOrEmpty(consulta))
                {
                    lREP = lREP.Where(p => p.Local.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar REPs";
            return View(lREP);
        }

        [Authorize]
        public ActionResult BuscaEquipamentoHomologado(string NumSerie, string NumRelogio)
        {
            BLL.EquipamentoHomologado bllEquipamentoHomologado = new BLL.EquipamentoHomologado(_user.ConnectionString, _user);
            String nomeEquipamentoHomologado = String.Empty;
            int idEquipamentoHomologado = 0;
            int identificacaoRelogio = 0;
            bool servicoComunicador = false;
            bool integraComunicador = false;
            var ItensEquipamentoTipoBiometria = new List<ItensCombo>();
            BLL.BilhetesImp BLLBilhetesImp = new BLL.BilhetesImp(_user.ConnectionString, _user);
            Int64 ultimoNSR = 0;

            if (!String.IsNullOrEmpty(NumRelogio))
            {
                ultimoNSR = BLLBilhetesImp.GetUltimoNSRRep(NumRelogio);   
            }

            try
            {
                Modelo.EquipamentoHomologado EquipamentoHomologado = bllEquipamentoHomologado.LoadByCodigoModelo(NumSerie);
                if (EquipamentoHomologado != null)
                {
                    BLL.EquipamentoTipoBiometria bllEquipamentoTipoBiometria = new BLL.EquipamentoTipoBiometria(_user.ConnectionString, _user);
                    ItensEquipamentoTipoBiometria = bllEquipamentoTipoBiometria.GetAllList(EquipamentoHomologado.Id);

                    nomeEquipamentoHomologado = EquipamentoHomologado.nomeModelo;
                    idEquipamentoHomologado = EquipamentoHomologado.Id;
                    identificacaoRelogio = EquipamentoHomologado.identificacaoRelogio;
                    servicoComunicador = EquipamentoHomologado.ServicoComunicador;
                    integraComunicador = IntegraComunicadorRep(EquipamentoHomologado);
                    if (integraComunicador)
	                {
                        nomeEquipamentoHomologado = EquipamentoHomologado.nomeModelo;
	                }
                    else
                    {
                        nomeEquipamentoHomologado = EquipamentoHomologado.nomeModelo + " - Rep não possui integração com o Comunicador Pontofopag";
                        integraComunicador = false;
                    }
                    
                }
                return Json(new { Successo = true, Erro = " ", EquipamentoHomNome = nomeEquipamentoHomologado, EquipamentoHomID = idEquipamentoHomologado, IdentificacaoRelogio = identificacaoRelogio, UltimoNSR = ultimoNSR, ServicoComunicador = servicoComunicador, IntegraComunicador = integraComunicador, EquipamentoTipoBiometria = ItensEquipamentoTipoBiometria }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Successo = false, Erro = ex.Message, EquipamentoHomNome = nomeEquipamentoHomologado, EquipamentoHomID = idEquipamentoHomologado, IdentificacaoRelogio = identificacaoRelogio, UltimoNSR = ultimoNSR, ServicoComunicador = servicoComunicador, IntegraComunicador = integraComunicador, EquipamentoTipoBiometria = ItensEquipamentoTipoBiometria }, JsonRequestBehavior.AllowGet);
            }

        }

        private bool IntegraComunicadorRep (EquipamentoHomologado equipamentohomologado)
        {
            bool IntegraComunicador = false;
            if ((equipamentohomologado.numeroFabricante == "00009" && equipamentohomologado.identificacaoRelogio == 1)
                || (equipamentohomologado.numeroFabricante == "00004" && equipamentohomologado.identificacaoRelogio == 2)
                || (equipamentohomologado.numeroFabricante == "00007" && equipamentohomologado.identificacaoRelogio == 3)
                || (equipamentohomologado.numeroFabricante == "00004" && equipamentohomologado.identificacaoRelogio == 4)
                || (equipamentohomologado.numeroFabricante == "00014" && equipamentohomologado.identificacaoRelogio == 5)
                || (equipamentohomologado.numeroFabricante == "00021" && equipamentohomologado.identificacaoRelogio == 6)
                || (equipamentohomologado.numeroFabricante == "00024" && equipamentohomologado.identificacaoRelogio == 7)
                || (equipamentohomologado.numeroFabricante == "00008" && equipamentohomologado.identificacaoRelogio == 8)
                || (equipamentohomologado.numeroFabricante == "00038" && equipamentohomologado.identificacaoRelogio == 9)
                || (equipamentohomologado.numeroFabricante == "00009" && equipamentohomologado.identificacaoRelogio == 10)
                || (equipamentohomologado.numeroFabricante == "00003" && equipamentohomologado.identificacaoRelogio == 11)
                || (equipamentohomologado.numeroFabricante == "00005" && equipamentohomologado.identificacaoRelogio == 11)
                || (equipamentohomologado.numeroFabricante == "00004" && equipamentohomologado.identificacaoRelogio == 12)
                || (equipamentohomologado.numeroFabricante == "00010" && equipamentohomologado.identificacaoRelogio == 17)
                || (equipamentohomologado.numeroFabricante == "00005" && equipamentohomologado.identificacaoRelogio == 19)
                || (equipamentohomologado.numeroFabricante == "00005" && equipamentohomologado.identificacaoRelogio == 20)
                || (equipamentohomologado.numeroFabricante == "00009" && equipamentohomologado.identificacaoRelogio == 21)
                || (equipamentohomologado.numeroFabricante == "00003" && equipamentohomologado.identificacaoRelogio == 22)
                || (equipamentohomologado.numeroFabricante == "00006" && equipamentohomologado.identificacaoRelogio == 23)
                || (equipamentohomologado.numeroFabricante == "00014" && equipamentohomologado.identificacaoRelogio == 24)
                )
            {
                IntegraComunicador = true;
            }
            return IntegraComunicador;
        }

        [PermissoesFiltro(Roles = "SituacaoRepConsultar")]
        [OutputCache(Duration = 50, VaryByParam = "none", VaryByCustom = "User")]
        public ActionResult SituacaoReps()
        {
            return View();
        }

        [PermissoesFiltro(Roles = "SituacaoRepConsultar")]
        [OutputCache(Duration = 50, VaryByParam = "none", VaryByCustom = "User")]
        public ActionResult SituacaoRepsAtual()
        {
            BLL.REP bllRep = new BLL.REP(_user.ConnectionString, _user);
            List<Modelo.Proxy.RepSituacao> situacaos = bllRep.VerificarSituacaoReps(3600);
            ViewBag.UltimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return PartialView(situacaos);
        }
    }
}
