using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class DepartamentoController : IControllerPontoWeb<Departamento>
    {
        [PermissoesFiltro(Roles = "Departamento")]
        public override ActionResult Grid()
        {
            return View(new Modelo.Departamento());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Departamento bllDepartamento = new BLL.Departamento(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
                List<Modelo.Departamento> dados = bllDepartamento.GetAllList();
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

        [PermissoesFiltro(Roles = "DepartamentoConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "DepartamentoCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "DepartamentoAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "DepartamentoCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Departamento obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "DepartamentoAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Departamento obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "DepartamentoExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            BLL.Departamento bllDepartamento = new BLL.Departamento(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Departamento departamento = bllDepartamento.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllDepartamento.Salvar(Acao.Excluir, departamento);
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

        protected override ActionResult Salvar(Departamento obj)
        {
            BLL.Departamento bllDepartamento = new BLL.Departamento(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
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
                    erros = bllDepartamento.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        ModelState.AddModelError("CustomError", erro);
                    }
                    else
                    {
                        return RedirectToAction("Grid", "Departamento");
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
            DesabilitaEmpresa(obj);
            return View("Cadastrar", obj);
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.Departamento bllDepartamento = new BLL.Departamento(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Departamento departamento = new Departamento();
            departamento = bllDepartamento.LoadObject(id);
            BLL.Parametros bllparm = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Modelo.Parametros parm = new Parametros();
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            if (id == 0)
            {
                departamento.Codigo = bllDepartamento.MaxCodigo();
            }

            DesabilitaEmpresa(departamento);
            return View("Cadastrar", departamento);
        }

        protected override void ValidarForm(Departamento obj)
        {
            if (!String.IsNullOrEmpty(obj.empresaNome))
            {
                BLL.Empresa bllEmp = new BLL.Empresa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                Empresa e = new Empresa();
                int codigo;
                string empresa = obj.empresaNome.Split('|')[0].Trim();
                if (int.TryParse(empresa, out codigo))
                {
                    e = bllEmp.LoadObjectByCodigo(codigo);
                }
                if (e != null && e.Id > 0)
                {
                    obj.IdEmpresa = e.Id;
                }
                else
                {
                    ModelState["empresaNome"].Errors.Add("Empresa " + empresa + " não cadastrada!");
                }
                if (obj.Horario != null)
                {
                    ValidaHorario(obj);
                }    
            }
        }

        private void ValidaHorario(Departamento obj)
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

        private void DesabilitaEmpresa(Departamento departamento)
        {
            BLL.Departamento bllDepartamento = new BLL.Departamento(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            if (bllDepartamento.PossuiFuncionarios(departamento.Id))
                ViewBag.Disabled = true;
            else
                ViewBag.Disabled = false;
        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta, String filtro)
        {
            IList<Departamento> lDep = PesquisaDepartamento(consulta, filtro, false);
            ViewBag.Title = "Pesquisar Departamento";
            return View(lDep);
        }
        public ActionResult EventoConsultaOpcaoTodas(String consulta, String filtro)
        {
            IList<Departamento> lDep = PesquisaDepartamento(consulta, filtro, true);
            ViewBag.Title = "Pesquisar Departamento";
            return View("EventoConsulta", lDep);
        }

        private IList<Departamento> PesquisaDepartamento(String consulta, String filtro, bool opcaotodas)
        {
            BLL.Departamento bllDepartamento = new BLL.Departamento(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            int idEmpresa = 0;
            // T é parametro para todos, por tanto não deve filtrar nada
            if (String.IsNullOrEmpty(filtro))
                filtro = "T";

            if (filtro != "T")
            {
                idEmpresa = EmpresaController.BuscaIdEmpresa(filtro);
            }

            IList<Departamento> lDep = new List<Departamento>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllDepartamento.GetIdPorCodigo(codigo).GetValueOrDefault();
                Departamento dep = bllDepartamento.LoadObject(id);
                if (codigo == 0 && opcaotodas)
                {
                    Modelo.Departamento TDep = new Modelo.Departamento { Codigo = 0, Descricao = "TODOS OS DEPARTAMENTOS" };
                    lDep.Add(TDep);
                }
                else
                {
                    // Se existir o departamento pesquisado e pertencer a empresa selecionada ou se for para trazer todos independente de empresa add na lista
                    if (dep != null && dep.Id > 0 && (dep.IdEmpresa == idEmpresa || filtro == "T"))
                    {
                        lDep.Add(dep);
                    }
                }
            }

            // Se pesquisou tudo ou não encontrou o codigo pesquisado, retorna todos os departamentos
            if (lDep.Count == 0)
            {
                if (opcaotodas)
                {
                    lDep = bllDepartamento.GetAllListComOpcaoTodos();
                }
                else
                {
                    if (!String.IsNullOrEmpty(consulta))
                    {
                        lDep = bllDepartamento.GetAllListLike(consulta);
                    }
                    else
                    {
                        lDep = bllDepartamento.GetAllList();
                    }
                }

                // Se p Filtro diferente de todos, retorna apenas os departamentos da empresa selecionada.
                if (filtro != "T")
                {
                    if (opcaotodas)
                        lDep = lDep.Where(d => d.IdEmpresa == idEmpresa || d.IdEmpresa == 0).ToList();
                    else
                        lDep = lDep.Where(d => d.IdEmpresa == idEmpresa).ToList();
                }
            }
            return lDep;
        }

        public static int BuscaIdDepartamento(string departamento)
        {
            int id = 0;
            try
            {
                if (!String.IsNullOrEmpty(departamento))
                {
                    BLL.Departamento bllDep = new BLL.Departamento(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                    Departamento dep = new Departamento();
                    string cod = departamento.Split('|')[0].Trim();
                    int codigo = Convert.ToInt32(cod);
                    id = bllDep.GetIdPorCodigo(codigo).GetValueOrDefault();
                }
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}