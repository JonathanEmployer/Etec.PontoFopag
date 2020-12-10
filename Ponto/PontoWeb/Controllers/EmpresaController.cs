using Modelo;
using Modelo.Proxy;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class EmpresaController : IControllerPontoWeb<Empresa>
    {
        private UsuarioPontoWeb _user = Usuario.GetUsuarioPontoWebLogadoCache();
        [PermissoesFiltro(Roles = "Empresa")]
        public override ActionResult Grid()
        {
            BLL.Empresa bllEmpresa = new BLL.Empresa(_user.ConnectionString, _user);
            var UsuarioEmpresa = bllEmpresa.ConsultaBloqueiousuariosEmpresa();
            if (UsuarioEmpresa)
            {
                ViewBag.ControleUsuario = true;
            }
            else
            {
                ViewBag.ControleUsuario = false;
            }
            return View(new Modelo.Empresa());
        }

        [Authorize]
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.Empresa bllEmpresa = new BLL.Empresa(_user.ConnectionString, _user);
                List<Modelo.Empresa> dados = bllEmpresa.GetAllListEmpresa();
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

        [PermissoesFiltro(Roles = "EmpresaConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "EmpresaCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "EmpresaAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "EmpresaCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Empresa obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "EmpresaAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Empresa obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "EmpresaExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            BLL.Empresa bllEmpresa = new BLL.Empresa(_user.ConnectionString, _user);
            BLL.EmpresaCw_Usuario bllacessoPEmpresa = new BLL.EmpresaCw_Usuario(_user.ConnectionString, _user);
            Empresa empresa = bllEmpresa.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                ApagarEmpresaCWUsuario(empresa.Id, bllacessoPEmpresa, _user);

                erros = bllEmpresa.Salvar(Acao.Excluir, empresa);
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
                String msgErro = String.Empty;
                if (ex.Message.Trim().Contains("FK_funcionario_empresa"))
                    msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Funcionário";
                else if (ex.Message.Trim().Contains("FK_contrato_empresa"))
                    msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Contrato";
                else if (ex.Message.Trim().Contains("FK_departamento_empresa"))
                    msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Departamento";
                else if (ex.Message.Trim().Contains("FK_empresa_Revendas"))
                    msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Revendas";
                else if (ex.Message.Trim().Contains("FK_empresacwusuario_empresa"))
                    msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de EmpresaCWUsuario";
                else if (ex.Message.Trim().Contains("FK_feriado_empresa"))
                    msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Feriado";
                else if (ex.Message.Trim().Contains("FK_rep_empresa"))
                    msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de REP";
                else if (ex.Message.Trim().Contains("FK_mudcodigofunc_empresa"))
                    msgErro = "Não é possivel excluir esse registro pois ele está relacionado à um registro de Mudança de código do funcionário";
                else
                    msgErro = ex.Message;

                return Json(new { Success = false, Erro = msgErro }, JsonRequestBehavior.AllowGet);
            }
        }

        private static void ApagarEmpresaCWUsuario(int id, BLL.EmpresaCw_Usuario bllacessoPEmpresa, UsuarioPontoWeb usuarioPontoWebLogadoCache)
        {
            if ((usuarioPontoWebLogadoCache != null) && (usuarioPontoWebLogadoCache.UtilizaControleEmpresa))
            {
                IList<Modelo.EmpresaCw_Usuario> acessosPEmpresa = new List<Modelo.EmpresaCw_Usuario>();
                acessosPEmpresa = bllacessoPEmpresa.GetListaPorEmpresa(id);
                foreach (var acessoPEmpresa in acessosPEmpresa)
                {
                    bllacessoPEmpresa.Salvar(Acao.Excluir, acessoPEmpresa);
                }
            }
        }

        protected override ActionResult Salvar(Empresa obj)
        {
            BLL.Empresa bllEmp = new BLL.Empresa(_user.ConnectionString, _user);
            BLL.EmpresaCw_Usuario bllacessoPEmpresa = new BLL.EmpresaCw_Usuario(_user.ConnectionString, _user);
            BLL_N.BLLEmpresa.ValidarCNPJEmpresa validaCnpj = new BLL_N.BLLEmpresa.ValidarCNPJEmpresa();
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                try
                {
                    Acao acao = new Acao();
                    acao = EscolhaDaAcao(obj);

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    DateTime dt = DateTime.Now;


                    if (acao == Acao.Incluir)
                    {
                        if (validaCnpj.VerificaCnpj(obj.Cnpj))
                        {
                            throw new Exception("Existe uma ou mais empresas cadastradas com o mesmo CNPJ.");
                        }
                        obj.Inchora = dt;
                        obj.Incdata = dt.Date; ;
                        obj.IDRevenda = BuscaIDRevenda();
                        obj.Numeroserie = String.Empty;
                    }
                    if (acao == Acao.Alterar)
                    {
                        if (!validaCnpj.ValidarAlterarCNPJ(obj.Cnpj, obj.Id, _user.ConnectionString, _user))
                        {
                            throw new Exception("Existe uma ou mais empresas cadastradas com o mesmo CNPJ.");
                        }
                    }

                    obj.Chave = obj.HashMD5ComRelatoriosValidacaoNova();
                    erros = bllEmp.Salvar(acao, obj);
                    if (erros.Count > 0)
                    {
                        ValidaRetornoBLLSalvar(erros);
                    }
                    else
                    {
                        SalvarEmpresaCWUsuario(obj, bllacessoPEmpresa, acao, _user);

                        return RedirectToAction("Grid", "Empresa");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("CustomError", ex.Message);
                }

            }
            Modelo.Parametros parm = new Parametros();
            BLL.Parametros bllparm = new BLL.Parametros(_user.ConnectionString, _user);
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            ViewBag.Disabled = true;
            ViewBag.Estados = Listas.Estados;
            AdicionaLogoPadrao(obj);
            return View("Cadastrar", obj);
        }

        private static void SalvarEmpresaCWUsuario(Empresa obj, BLL.EmpresaCw_Usuario bllacessoPEmpresa, Acao acao, UsuarioPontoWeb usuarioPontoWebLogadoCache)
        {
            if (((usuarioPontoWebLogadoCache != null) &&
                (usuarioPontoWebLogadoCache.UtilizaControleEmpresa)) &&
                (acao == Acao.Incluir))
            {
                Modelo.EmpresaCw_Usuario acessoPEmpresa = new Modelo.EmpresaCw_Usuario();
                acessoPEmpresa.Codigo = bllacessoPEmpresa.MaxCodigo();
                acessoPEmpresa.IdEmpresa = obj.Id;
                acessoPEmpresa.IdCw_Usuario = usuarioPontoWebLogadoCache.Id;

                bllacessoPEmpresa.Salvar(acao, acessoPEmpresa);
            }
        }

        private int BuscaIDRevenda()
        {
            int idRevenda = 0;
            BLL.Empresa bllEmp = new BLL.Empresa(_user.ConnectionString, _user);
            Modelo.Empresa empresaPrincipal = new Modelo.Empresa();
            empresaPrincipal = bllEmp.GetEmpresaPrincipal();
            if ((empresaPrincipal != null) && (empresaPrincipal != new Empresa()))
            {
                if (empresaPrincipal.IDRevenda != 0)
                {
                    idRevenda = empresaPrincipal.IDRevenda;
                    return idRevenda;
                }
                idRevenda = 3;
            }
            else
            {
                idRevenda = 3;
            }

            return idRevenda;
        }

        protected override ActionResult GetPagina(int id)
        {
            BLL.Empresa bllEmp = new BLL.Empresa(_user.ConnectionString, _user);
            Empresa e = bllEmp.LoadObject(id);
            BLL.Parametros bllparm = new BLL.Parametros(_user.ConnectionString, _user);
            Modelo.Parametros parm = new Parametros();
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            if (id == 0)
            {
                e.Codigo = bllEmp.MaxCodigo();
                ViewBag.Disabled = false;
            }
            else
            {
                bllEmp.SetTermosUsoApp(e);
                ViewBag.Disabled = true;
            }
            if (e.bloqueioEdicaoEmp == 1)
            {
                ViewBag.Consultar = 1;
            }

            BLL.EmpresaLogo bllEmpresaLogo = new BLL.EmpresaLogo(_user.ConnectionString, _user);
            EmpresaLogo L = bllEmpresaLogo.GetAllListPorEmpresa(e.Id).FirstOrDefault();

            if (L == null)
            {
                L = new EmpresaLogo();
            }
            e.EmpresaLogo = L;

            ViewBag.Estados = Listas.Estados;

            AdicionaLogoPadrao(e);
            return View("Cadastrar", e);
        }

        private Acao EscolhaDaAcao(Empresa empresa)
        {
            Acao acao = new Acao();
            if (empresa.Id == 0)
            {
                acao = Acao.Incluir;
                ViewBag.Disabled = false;
            }
            else
            {
                acao = Acao.Alterar;
                ViewBag.Disabled = true;
            }
            return acao;
        }

        private static void ValidaLogoEmpresa(Empresa Empresa)
        {

            if (Empresa.EmpresaLogo.Logo.Contains("data:image/"))
            {
                string base64 = ";base64,";
                int posicao = Empresa.EmpresaLogo.Logo.IndexOf(base64);
                posicao += base64.Length;
                Empresa.EmpresaLogo.Logo = Empresa.EmpresaLogo.Logo.Substring(posicao);
            }
            if (Empresa.EmpresaLogo.Logo == "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")
            {
                Empresa.EmpresaLogo.Logo = null;
            }

        }

        private static void AdicionaLogoPadrao(Empresa EmpresaLogo)
        {
            if (string.IsNullOrEmpty(EmpresaLogo.EmpresaLogo.Logo))
            {
                EmpresaLogo.EmpresaLogo.Logo = "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAIAAABMXPacAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABp0RVh0U29mdHdhcmUAUGFpbnQuTkVUIHYzLjUuMTFH80I3AAAGtklEQVR4Xu2dUYLcIAiG9/433dlLlB1SSlUQFdRk/B6mBlGRX0z2qV+vw+v1/f19tTz4+fm5WnKb+AIrcdkOf8mF8ZKKsn0qYIhxPRoE8K3TA9JWAeeassPPq9QGhq6go8cgIMYlwO8r+C9oOczhvIQXMyrAqZhBegQ4SXfkXEGLOQIs5hKAPk7f30EXaDmE4lMBR61uzhW0mCEBkoOPj/ir/PGN5EajG2Jxbhq7ilMBF6tUCRHgSa+EXBhfqU4FjDKoh7MA53Oolf8EIDEhjwRaDh3w4pDa5x2wmPMOWIyDAMXzforAiH8FnNQ3ca6gxZQFOJ9D0wipgFwtiwWQZLY4N43dhw+9gvZRxVOA4pHkf3TsTy5MtFTnJdyGux5HgMUsFkA6UEV7YmwaOwG+rtTO0QQY/xht9Y9jk0jyMCZVQLKwlI6ifWRslXxU3zx9wFozBJi5pYTxpaOD9xegI+Lip2pilD5n+z5z8yBXnZK6AONvAiAfApY8d4mRHhPP4liO3mshDziH+0jtKuFXUFM0I8BC1bznDuNS6VS3P+kl3AdmB/YgbaO6PQB8kiznFsKiB19UakvkPlECtEbWBKQJ5rRPm3ja9ejGHtvWFdCNnlB7djh8TqktkayIj/g7TwB92+5nMKEv6TCKApPanI5dzK6AvkT3jSrCk4gNC4qzJAaQ2HES/guYBOBBE2ix0zFkT/SNQK/9uIDzM98BvuhFUz1YOJz/cmYLQOHqu9qHPGUWYFMwkP9eHW/wEX6BQAGUzBqTjoFuAgVTDL66I5QB3fi+nnAFrdJJX9d4yKwCcP0JtBxGWFMBg+KR/KvOviNTBaDEeSHN5ruKTtNa/CLBRoMAHbdQ1SGOpDgwkgkV07rEgitIV0XplboUu5QO6KJRvN1Hvop9wtgK0JlwHgfR91jdftUBCK8ASxCbk5w8bHsx+wraSg8pmKI9KPLwK8jio6PMIHW12mfC0wi/a94BNLB7BnfG99LH7CsIkCLWd6L0tk6oLzSZ9VeQZZJpSMHoQVJmOEVjTngF5KGjZau8x1Hd5oIr6L50HxploEmAkdev5El2+1S74RJ5XQDHjx/EMlz3UXqlrlb7NCoCuGcfyGcYnzOC6Kgwt4HvAH0DvLfoaR+eIHW12uegCRBx9QPYlTvQcvtg2a/kY9yOKEBQ9ovg5BsKME41FQUBRlJvJ2LOVVSPjrJZ/3eAslhHl743pVfqarVzEh+XM+QsgG/2P4Evriq1ISMEWqronrw39ySL5Rg+BtzsVQEjqW9CmjZouf25BHgn/AItjtCc0uTvZX+77lgB0qaMOLwD9AgG43s8QwI0pX5/nXgMTXdysXCN1dwjgBKKhDQksd/xCtKp5srhCtJRIsi7lghQzZGEfaDiaRUApzAuaXEjH+OcT8W5Alqzmfg/7wqqUhEAEmTJadFHGYhdlplnMll+XO73P3SGf+A3T0fRaEEf1TenC1KKyf7e8QVaqtg9ixQqwLg8+nDP6ijF+XmXj3FH4V9BRR6WfSX+6qH8TwBjJXIjtotuRXLPJdk37lTHPkTx7K+Apog7tkfoCim9S6RtRRTA5YwA+qhb5CgUh3dAMcW5MbdEZ19ZEboItFhocq6CwTQIoEcPbSmh3A2JTv1M8t014VABCRTQYGReDJ76aPwFmIYljy7Zrw4pFrSxyocEsGwvsRjD6gZjAK7n1VQjmVcB0an3xVdCZbZNr6Ak4uIGuNHStmMc1Td5wgIBXOLuI9EGH7FBYO80QgSwvBskYKzd+dZglja9goDdZAiKZwsBYG/VV3Sy/2I6uNHStqOMkrqMHx2zBeC3EzY4fdkZga8IbXzEBoG9SPJYpeq/6RUkxW1JBzda2nakUa3ly9n3HQD0pamDRBt8xAaBvZyisZVLAOOFFQe/mgi0PJt9K2D5mYgGN7j1FaQA9fEMhW4gAL+dsMEhJaTeq2VrdzA4vCLAjU7ZYCKIRBt8xAaBvS7c9QriZYHgYw7vsrQ7KB5T49m9qwAWkhRAlo2J5m40ChsIdlmoOvcLcKPbKQi7EopnWYDnJRd3RPuiBqYGfpUchfLkK+gW+Avw1KuJashrgzhPswBey9+R4jU1eHelAqy6Cp+H8aROfQc8rHrosCr7qh5oNwE++WrCvUOuMd3UIJJHToMAyiwj3Fq58ZxMvYKa4MI8uLxmCPD422mkDhwECLqaJFBO+sXGfVl2Bd09cV78E2DyQZbgwnyCSLECbCLqzmxXARJYDfSLjQ1pTaPpHeCrzSkLzgIBJPgqtxapKfiNBPhEXq8/jTiObrrUuFUAAAAASUVORK5CYII=";
            }
        }

        protected override void ValidarForm(Empresa obj)
        {
            BLL.Empresa bllEmp = new BLL.Empresa(_user.ConnectionString, _user);

            if ((String.IsNullOrEmpty(obj.Cnpj)) && (String.IsNullOrEmpty(obj.Cpf)))
            {
                ModelState["Cnpj"].Errors.Add("É obrigatório o preenchimento do CNPJ ou do CPF");
            }

            else if (!(String.IsNullOrEmpty(obj.Cpf)) && !(String.IsNullOrEmpty(obj.Cnpj)))
            {
                ModelState["Cpf"].Errors.Add("Não é permitido o preenchimento do CNPJ e do CPF");
            }

            else if (!String.IsNullOrEmpty(obj.Cnpj) && !BLL.cwkFuncoes.ValidarCNPJ(obj.Cnpj))
            {
                ModelState["Cnpj"].Errors.Add("Número do CNPJ Inválido");
            }

            else if (!String.IsNullOrEmpty(obj.Cpf) && !BLL.cwkFuncoes.ValidarCPF(obj.Cpf))
            {
                ModelState["Cpf"].Errors.Add("Número do CPF Inválido");
            }

            if (String.IsNullOrEmpty(obj.Nome))
            {
                ModelState["Nome"].Errors.Add("É Obrigatório o preenchimento do Nome da Empresa");
            }

            if (String.IsNullOrEmpty(obj.Endereco))
            {
                ModelState["Endereco"].Errors.Add("É Obrigatório o preenchimento do Endereço da Empresa");
            }

            if (String.IsNullOrEmpty(obj.Cidade))
            {
                ModelState["Cidade"].Errors.Add("É Obrigatório o preenchimento da Cidade da Empresa");
            }
            if (String.IsNullOrEmpty(obj.Cep))
            {
                ModelState["Cep"].Errors.Add("É Obrigatório o preenchimento do Cep da Empresa");
            }

            ValidaLogoEmpresa(obj);
            if (obj.NomeHorario != null)
            {
                ValidaHorario(obj);
            }

        }

        private void ValidaHorario(Empresa obj)
        {
            BLL.Horario bllHorario = new BLL.Horario(_user.ConnectionString, _user);

            int idHorario = HorarioController.BuscaIdHorario(obj.NomeHorario, obj.TipoHorarioPadraoFunc);
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
                ModelState["Horario"].Errors.Add("Horário " + obj.NomeHorario + " não cadastrado!");
            }
            else
            {
                if (idHorario > 0)
                { obj.IdHorarioPadraoFunc = idHorario; }
                else
                { ModelState["Horario"].Errors.Add("Horário " + obj.NomeHorario + " não cadastrado!"); }
            }
        }

        public ActionResult Atestado(int id)
        {
            BLL.Empresa bllEmp = new BLL.Empresa(_user.ConnectionString, _user);
            Empresa e = bllEmp.LoadObject(id);
            string parametro = e.Cnpj + "|" + e.Nome;
            parametro = BLL.CriptoString.Encrypt(parametro);
            parametro = Uri.EscapeDataString(parametro);
            string url = ConfigurationManager.AppSettings["ApiPontofopag"] + "AtestadoTecnico/GerarAtestado?parametro=" + parametro;

            return Redirect(url);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult PeriodoFechamento(int codigo)
        {
            pxyRetornoPeriodoFechamento ret = new pxyRetornoPeriodoFechamento();
            try
            {
                BLL.Empresa bllEmp = new BLL.Empresa(_user.ConnectionString, _user);
                PeriodoFechamento pf = bllEmp.PeriodoFechamentoPorCodigo(codigo);

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

        public ActionResult PermissaoAPPs(string empSelecionada)
        {
            try
            {
                BLL.Empresa bllEmpresa = new BLL.Empresa(_user.ConnectionString, _user);
                Modelo.Empresa empresa = bllEmpresa.GetEmpresaConsultada(empSelecionada);
                bool registradorEmpresa = bllEmpresa.VerificaPermisaoRegistrador(empresa);
                bool appEmpresa = false;
                bool webAppEmpresa = false;
                bool utilizaReconhecimentoFacilApp = false;
                bool utilizaReconhecimentoFacilWebApp = false;
                bllEmpresa.VerificaPermissoesApps(empresa.Id, out appEmpresa, out utilizaReconhecimentoFacilApp, out webAppEmpresa, out utilizaReconhecimentoFacilWebApp);

                return Json(new { RegistradorEmpresa = registradorEmpresa, APPEmpresa = appEmpresa, UtilizaReconhecimentoFacilApp = utilizaReconhecimentoFacilApp, WebAPPEmpresa = webAppEmpresa, UtilizaReconhecimentoFacilWebApp = utilizaReconhecimentoFacilWebApp }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { RegistradorEmpresa = false, APPEmpresa = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Metodos
        [Authorize]
        public ActionResult EventoConsulta(String consulta)
        {
            IList<Empresa> lEmp = PesquisaEmpresa(consulta, false);
            ViewBag.Title = "Pesquisar Empresa";
            return View(lEmp);
        }

        [Authorize]
        public ActionResult EventoConsultaOpcaoTodas(String consulta)
        {
            IList<Empresa> lEmp = PesquisaEmpresa(consulta, true);
            ViewBag.Title = "Pesquisar Empresa";
            return View("EventoConsulta", lEmp);
        }

        private IList<Empresa> PesquisaEmpresa(String consulta, bool opcaotodas)
        {
            BLL.UsuarioPontoWeb bllUpw = new BLL.UsuarioPontoWeb(_user.ConnectionString, _user);
            BLL.Empresa bllEmp = new BLL.Empresa(_user.ConnectionString, _user);
            IList<Empresa> lEmp = new List<Empresa>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                if (codigo == 0 && opcaotodas)
                {
                    Modelo.Empresa TEmp = new Modelo.Empresa { Codigo = 0, Nome = "TODAS AS EMPRESAS" };
                    lEmp.Add(TEmp);
                }
                else
                {
                    Empresa e = bllEmp.LoadObjectByCodigo(codigo);
                    if (e != null && e.Id > 0)
                    {
                        lEmp.Add(e);
                    }
                }
            }

            if (lEmp.Count == 0)
            {
                if (opcaotodas)
                    lEmp = bllEmp.GetAllListComOpcaoTodos();
                else
                    lEmp = bllEmp.GetAllList();
                if (!String.IsNullOrEmpty(consulta))
                {
                    lEmp = lEmp.Where(p => p.Nome.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            return lEmp;
        }

        public static int BuscaIdEmpresa(string filtro)
        {
            int idEmpresa = 0;
            try
            {
                UsuarioPontoWeb _user = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.UsuarioPontoWeb bllUpw = new BLL.UsuarioPontoWeb(_user.ConnectionString, _user);
                BLL.Empresa bllEmp = new BLL.Empresa(_user.ConnectionString, _user);
                Empresa e = new Empresa();
                string codigo = filtro.Split('|')[0].Trim();
                int cod = Convert.ToInt32(codigo);
                e = bllEmp.LoadObjectByCodigo(cod);
                if (e != null && e.Id > 0)
                    idEmpresa = e.Id;
                else idEmpresa = 0;
            }
            catch (Exception)
            {
                idEmpresa = 0;
            }
            return idEmpresa;
        }

        //[Authorize]
        public async Task<JsonResult> PostGenerationTokenEPays(bool EPays)
        {
            //IList<Empresa> lEmp = PesquisaEmpresa(consulta, true);
            //ViewBag.Title = "Pesquisar Empresa";
            //return View("EventoConsulta", lEmp);

            return Json(new { Success = true, Token = Guid.NewGuid() }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}