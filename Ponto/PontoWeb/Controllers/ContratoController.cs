using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class ContratoController : IControllerPontoWeb<Contrato>
    {
        [PermissoesFiltro(Roles = "Contrato")]
        public override ActionResult Grid()
        {
            var cwu = Usuario.GetUsuarioLogadoCache();
            BLL.EmpresaCw_Usuario bllEmpresaCwUsuario = new BLL.EmpresaCw_Usuario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            var CWUsuarioCont = bllEmpresaCwUsuario.CWUtilizaControleContratos();
          
            if (CWUsuarioCont)
            {
                ViewBag.ControleUsuario = true;
            }
            else
            {
                ViewBag.ControleUsuario = false;
            }
            return View(new Modelo.Contrato());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Contrato bllContrato = new BLL.Contrato(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Contrato> dados = bllContrato.GetAllList();
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

        [PermissoesFiltro(Roles = "ContratoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ContratoCadastrar")]
        public override ActionResult Cadastrar()
        {
            int id = 0;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ContratoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "ContratoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Contrato obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ContratoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Contrato obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "ContratoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            BLL.Contrato bllContrato = new BLL.Contrato(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            try
            {
                Contrato contrato = bllContrato.LoadObject(id);
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllContrato.Salvar(Acao.Excluir, contrato);
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

        protected override ActionResult Salvar(Contrato obj)
        {
            BLL.Contrato bllContrato = new BLL.Contrato(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    if (obj.Id == 0)
                        acao = Acao.Incluir;
                    else
                        acao = Acao.Alterar;

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    erros = bllContrato.Salvar(acao, obj);

                    if (erros.Count > 0)
                    {
                        ValidaRetornoBLLSalvar(erros);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Contrato");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }
            }
            Modelo.Parametros parm = new Parametros();
            BLL.Parametros bllparm = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.Contrato bllContrato = new BLL.Contrato(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Contrato contrato = new Contrato();
            BLL.Parametros bllparm = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Modelo.Parametros parm = new Parametros();
            contrato = bllContrato.LoadObject(id);
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            if (id == 0)
            {
                contrato.Codigo = bllContrato.MaxCodigo();
            }
            return View("Cadastrar", contrato);
        }

        protected override void ValidarForm(Contrato obj)
        {
            int idEmpresa = EmpresaController.BuscaIdEmpresa(obj.NomeEmpresa);
            if (idEmpresa > 0)
            {
                obj.IdEmpresa = idEmpresa;
            }
            else
            {
                ModelState["NomeEmpresa"].Errors.Add("Empresa " + obj.NomeEmpresa + " não cadastrada!");
            }
            if (obj.Horario != null)
            {
                ValidaHorario(obj);    
            }    
        }
    
        private void ValidaHorario(Contrato obj)         
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(conn, usr);

            int idHorario = HorarioController.BuscaIdHorario(obj.Horario);
            List<Horario> lHorarioNormalMovel = new List<Horario>();

            switch (obj.TipoHorarioPadraoFunc)
            {
                case 1:
                    lHorarioNormalMovel = bllHorario.GetHorarioNormalMovelList(1, false);
                    break;
                case 2:
                    lHorarioNormalMovel = bllHorario.GetHorarioNormalMovelList(2, false);
                    break;
                default:
                    lHorarioNormalMovel = bllHorario.GetHorarioNormalMovelList(1, false);
                    break;
            }

            lHorarioNormalMovel = lHorarioNormalMovel.Where(s => s.Id == idHorario).ToList();
            if (lHorarioNormalMovel.Count() == 0)
            {
                ModelState["Horario"].Errors.Add("Horário " + obj.Horario + " não cadastrado!");
            }
            else
            {
                if (idHorario > 0)
                { obj.IdHorarioPadraoFunc = idHorario; }
                else
                { ModelState["Horario"].Errors.Add("Horário " + obj.Horario + " não cadastrado!"); }
            }
        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta, String filtro)
        {
            bool parametro = true;
            List<Contrato> res = PesquisaContratos(consulta, filtro, parametro);
            ViewBag.Title = "Pesquisar Contratos";
            return View(res);
        }

        [Authorize]
        public ActionResult EventoConsultaSemTodos(String consulta, String filtro)
        {
            List<Contrato> res = PesquisaContratos(consulta, filtro, false);
            ViewBag.Title = "Pesquisar Contratos";
            return View(res);
        }

        private List<Contrato> PesquisaContratos(String consulta, String filtro, bool parametro)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Contrato bllCont = new BLL.Contrato(usr.ConnectionString, usr);
            List<Contrato> conts = new List<Contrato>();
            int idEmpresa = 0;

            if (String.IsNullOrEmpty(filtro))
                filtro = "T";

            if (filtro != "T")
                idEmpresa = EmpresaController.BuscaIdEmpresa(filtro);

            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Contrato ct = bllCont.LoadPorCodigo(codigo);
                if ((ct.Id != 0) || (consulta == "0") && parametro ==true)
                {
                    if (consulta == "0")
                    {
                        Modelo.Contrato cont = new Modelo.Contrato { Codigo = 0, NomeEmpresa = "TODOS OS CONTRATOS" };
                        ct = cont;
                    }
                    conts.Add(ct);
                }

                return conts;
            }
            else
            {
                if (idEmpresa > 0)
                {
                    conts = bllCont.GetAllListPorEmpresa(idEmpresa);
                }
                else if (!String.IsNullOrEmpty(consulta) && conts.Count == 0)
                {
                    conts = bllCont.GetAllList().Where(w =>
                        w.CodigoContrato.ToLower().Contains(consulta.ToLower()) ||
                        w.DescricaoContrato.ToLower().Contains(consulta.ToLower()) ||
                        w.NomeEmpresa.ToLower().Contains(consulta.ToLower())
                        ).ToList();
                }
                else
                {
                    conts = bllCont.GetAllList();
                }
                if (parametro)
                {
                    Modelo.Contrato cont = new Modelo.Contrato { Codigo = 0, CodigoContrato = "0", DescricaoContrato = "TODOS OS CONTRATOS" };
                    conts.Add(cont);
                }
                return conts;
            }
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult PeriodoFechamento(int codigo)
        {
            pxyRetornoPeriodoFechamento ret = new pxyRetornoPeriodoFechamento();
            try
            {
                BLL.Contrato bllContrato = new BLL.Contrato(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                PeriodoFechamento pf = bllContrato.PeriodoFechamentoPorCodigo(codigo);

                bool atribuiu = BLL.cwkFuncoes.AtribuiPeriodoFechamentoPonto(pf);

                if (atribuiu)
                {
                    ret.Sucesso = true;
                    ret.PeriodoFechamento = pf;
                }
                else
                {
                    ret.Sucesso = false;
                }
            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                ret.Sucesso = false;
                ret.Erro = e.Message;
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }        
    }
}