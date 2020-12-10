using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Management;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    [Authorize]
    public class FuncionarioController : IControllerPontoWeb<Funcionario>
    {
        public override ActionResult Grid()
        {
            return View(new Modelo.Proxy.pxyFuncionarioGrid());
        }

        [Authorize]
        public JsonResult FuncsGrid()
        {
            return getFuncionarios();
        }
        [Authorize]
        public JsonResult FuncsGridVazioXaxo()
        {
            try
            {

                List<Modelo.Proxy.pxyFuncionarioGrid> funcs = new List<Modelo.Proxy.pxyFuncionarioGrid>();
                JsonResult jsonResult = Json(new { data = funcs }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }
        [Authorize]
        public JsonResult FuncsGridComParms([System.Web.Http.FromBody] int opcao)
        {
            return getFuncionarios(opcao);
        }

        private JsonResult getFuncionarios()
        {
            return getFuncionarios(1);
        }

        private JsonResult getFuncionarios(int flag)
        {
            try
            {
                var usr = Usuario.GetUsuarioPontoWebLogadoCache();
                BLL.Funcionario bllFuncionario = new BLL.Funcionario(usr.ConnectionString, usr);
                List<Modelo.Proxy.pxyFuncionarioGrid> funcs = bllFuncionario.GetAllGrid(flag);
                JsonResult jsonResult = Json(new { data = funcs }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [Authorize]
        public JsonResult GetFunsComParms(int opcao)
        {
            try
            {
                var userPW = Usuario.GetUsuarioPontoWebLogadoCache();
                var _conn = new DAL.SQL.DataBase(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);
                BLL.Funcionario _bll = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                var _lista = _bll.GetAllGrid(opcao);

                JsonResult jsonResult = Json(new { data = _lista }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [PermissoesFiltro(Roles = "FuncionarioConsultar")]
        public override ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FuncionarioCadastrar")]
        public override ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "FuncionarioAlterar")]
        public override ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "FuncionarioCadastrar")]
        [HttpPost]
        public override ActionResult Cadastrar(Funcionario obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FuncionarioAlterar")]
        [HttpPost]
        public override ActionResult Alterar(Funcionario obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "FuncionarioExcluir")]
        [HttpPost]
        public override ActionResult Excluir(int id)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Funcionario funcionario = bllFuncionario.LoadObject(id);
            try
            {
                Dictionary<string, string> erros = new Dictionary<string, string>();
                erros = bllFuncionario.Salvar(Acao.Excluir, funcionario);
                if (erros.Count > 0)
                {
                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                    return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                }
                try
                {
                    if (funcionario.utilizaregistrador || funcionario.UtilizaAppPontofopag)
                    {
                        RemoveFuncionariosCentralCliente(funcionario);
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

        [Authorize]
        public ActionResult InativarFuncionarioRFID(int idfuncionario, int idrfid)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);

            try
            {
                FuncionarioRFID funcionarioRFID = bllFuncionarioRFID.LoadObject(idrfid);
                funcionarioRFID.Ativo = false;

                Dictionary<string, string> erros = new Dictionary<string, string>();

                erros = bllFuncionarioRFID.Salvar(Acao.Alterar, funcionarioRFID);

                if (erros.Count > 0)
                {
                    ViewBag.ErrosRFID = erros;
                }

                return PartialFuncionarioRFID(bllFuncionario, idfuncionario, usr);


            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new
                {
                    Success = false,
                    Erro = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult AdicionarFuncionarioRFID(int idfuncionario, string Senha, string Cracha, int TipoCracha)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);

            try
            {
                Funcionario funcionario = bllFuncionario.LoadObject(idfuncionario);
                bllFuncionario.SetSenha(funcionario, Senha);
                bllFuncionario.Salvar(Acao.Alterar, funcionario);

                Dictionary<string, string> erros = new Dictionary<string, string>();
                if (((TipoCracha == 0) && (Cracha.Length <= 8)) || ((TipoCracha == 1) && (Cracha.Length >= 10)))
                {
                    erros = bllFuncionarioRFID.Salvar(Acao.Incluir, new FuncionarioRFID
                    {
                        IdFuncionario = idfuncionario,
                        Codigo = bllFuncionarioRFID.MaxCodigo(),
                        RFID = TipoCracha == 0 ? Convert.ToInt64(Cracha) : (long?)null,
                        MIFARE = TipoCracha == 1 ? Cracha : string.Empty

                    });

                    if (erros.Count > 0)
                    {
                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                        return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Success = false, Erro = "Numero de caracteres era muito grande ou muito pequeno. Favor verificar." }, JsonRequestBehavior.AllowGet);
                }
                return PartialFuncionarioRFID(bllFuncionario, idfuncionario, usr);

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Json(new { Success = false, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private ActionResult PartialFuncionarioRFID(BLL.Funcionario bllFuncionario, int idfuncionario, Cw_Usuario usr)
        {
            BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);

            Funcionario funcionario = new Funcionario();
            funcionario = bllFuncionario.LoadObject(idfuncionario);
            funcionario.FuncionarioRFID = bllFuncionarioRFID.GetAllListByFuncionario(idfuncionario, true).ToList();
            funcionario.Senha = bllFuncionario.GetSenha(funcionario);

            return PartialView("FuncionarioRFID", funcionario);

        }

        //UtilizaAppPontofopag = false
        //utilizaregistrador = true

        protected override ActionResult Salvar(Funcionario funcionario)
        {
            string connString = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb usuarioLogado = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(connString, usuarioLogado);


            Modelo.Funcionario funcionarioAntigo = bllFuncionario.LoadObject(funcionario.Id);

            ValidarForm(funcionario);

            Acao acao = EscolhaDaAcao(funcionario);

            if (ModelState.IsValid)
            {
                try
                {
                    funcionario.RFID = null;
                    //funcionario.Senha = null;
                    funcionario.TipoCracha = null;

                    string conteudoEmail = "";
                    string conteudoSMS = "";

                    if (funcionario.OpcaoSMSEmailSenha == "Email")
                    {
                        conteudoEmail = GeraConteudoEmail(funcionario.UtilizaAppPontofopag, funcionario.UtilizaWebAppPontofopag, funcionario.Mob_Senha);
                    }
                    else if (funcionario.OpcaoSMSEmailSenha == "SMS")
                    {
                        conteudoSMS = GeraConteudoSMS(funcionario.UtilizaAppPontofopag, funcionario.UtilizaWebAppPontofopag, funcionario.Mob_Senha);
                    }
                    else if (funcionario.OpcaoSMSEmailSenha == "Ambos")
                    {
                        conteudoEmail = GeraConteudoEmail(funcionario.UtilizaAppPontofopag, funcionario.UtilizaWebAppPontofopag, funcionario.Mob_Senha);
                        conteudoSMS = GeraConteudoSMS(funcionario.UtilizaAppPontofopag, funcionario.UtilizaWebAppPontofopag, funcionario.Mob_Senha);
                    }

                    Funcionario DadosAntFunc = bllFuncionario.LoadObject(funcionario.Id);
                    tratamentoSenhas(funcionario, DadosAntFunc);
                    if (funcionario.Pis != null)
                    {
                        funcionario.Pis = funcionario.Pis.Replace(".", "").Replace("-", "");
                    }

                    Dictionary<string, string> erros = new Dictionary<string, string>();
                    funcionario.NaoRecalcular = true;

                    erros = bllFuncionario.Salvar(acao, funcionario);
                    if (erros.Count > 0)
                    {
                        TrataErros(erros);
                    }
                    else
                    {
                        try
                        {
                            EnvioEmail.EnviaEmailClient enviaEmail = new EnvioEmail.EnviaEmailClient();
                            EnviaSms.WsEnviaSmsClient objEnvioSMS = new EnviaSms.WsEnviaSmsClient();
                            if (funcionario.OpcaoSMSEmailSenha == "Email")
                            {
                                enviaEmail.EnviaEmail("no-reply@employer.com.br", funcionario.Email, "Pontofopag", conteudoEmail,
                                    "", null, "", "", "", "", "", null);
                            }
                            else if (funcionario.OpcaoSMSEmailSenha == "SMS")
                            {
                                Int64 numCelular = Convert.ToInt64("55" + funcionario.Celular);
                              
                                objEnvioSMS.EnviaSmsPadrao(funcionario.Celular, numCelular, conteudoSMS, null, 2);
                            }
                            else if (funcionario.OpcaoSMSEmailSenha == "Ambos")
                            {
                                Int64 numCelular = Convert.ToInt64("55" + funcionario.Celular);

                                enviaEmail.EnviaEmail("no-reply@employer.com.br", funcionario.Email, "Pontofopag", conteudoEmail,
                                    "", null, "", "", "", "", "", null);

                                objEnvioSMS.EnviaSmsPadrao(funcionario.Celular, numCelular, conteudoSMS, null, 2);
                            }

                            bllFuncionario.SetContratoFuncionario(funcionario.Id, funcionario.Contrato);
                            BLL_N.JobManager.CalculoMarcacoes.RecalculaEdicaoFuncionario(funcionario, usuarioLogado, true);
                            //Se o Funcionário utiliza registrador insere funcionario no Central do cliente
                            if (funcionario.utilizaregistrador || funcionario.UtilizaAppPontofopag || funcionario.UtilizaWebAppPontofopag)
                            {
                                if (!AdicionaAlteraFuncionariosCentralCliente(funcionario, funcionarioAntigo))
                                {
                                    TrataErroInclusaoCentralCliente();
                                    AdicionaFotoPadrao(funcionario);
                                    PreencheTipoVinculo(connString, usuarioLogado);
                                    return View("Cadastrar", funcionario);
                                }
                            }
                            //Se utilizava o registrador/app e "Deixou" de utilizar, remove Funcionário da Central do cliente
                            else if ((funcionarioAntigo.utilizaregistrador || funcionarioAntigo.UtilizaAppPontofopag || funcionarioAntigo.UtilizaWebAppPontofopag) &&
                                     (!funcionario.utilizaregistrador && !funcionario.UtilizaAppPontofopag && !funcionario.UtilizaWebAppPontofopag))
                            {
                                RemoveFuncionariosCentralCliente(funcionarioAntigo);
                            }
                        }
                        catch (Exception)
                        {
                            TrataErroInclusaoCentralCliente();
                        }

                        return RedirectToAction("Grid", "Funcionario");
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    if (ex.Message.Contains("session is in the kill state") || ex.Message.Contains("erro no nível de transporte"))
                    {
                        ModelState.AddModelError("Falha ao conectar-se com o banco de dados, tente novamente em instantes.", ex.Message);
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError", ex.Message);
                    }
                }
            }
            Modelo.Parametros parm = new Parametros();
            BLL.Parametros bllparm = new BLL.Parametros(connString, usuarioLogado);
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;
            AdicionaFotoPadrao(funcionario);
            PreencheTipoVinculo(connString, usuarioLogado);
            funcionario.FuncionarioRFID = funcionario.FuncionarioRFID == null ? new List<FuncionarioRFID>() : funcionario.FuncionarioRFID;
            return View("Cadastrar", funcionario);
        }

        private string GeraConteudoEmail(bool utilizaApp, bool utilizaWEBApp, string senha)
        {
            string conteudo = "";

            if (utilizaApp == true && utilizaWEBApp == false)
            {
                conteudo = string.Format("<div style='max-width: 580; text-align: center;'>"
                        + "<img src='https://i.ibb.co/2MH6RBm/thumbnail-image-1.png' ><br>"
                        + "<p><b>Aplicativo Pontofopag habilitado para uso</b></p> <br><br>"
                        + "O aplicativo Pontofopag para registro de Ponto está habilitado e disponível para seu uso.<br> Basta baixar o app através da Play Store ou Apple Store e instalar.  <br>"
                        + "No momento do login, se identifique com seu <b>CPF e utilize a senha:{0}</b> <br>"
                        + "Qualquer dúvida na utilização, contate seu RH.  <br><br>"
                        + "Att. <br>"
                        + "Equipe Pontofopag"
                        + "<br><img src='https://i.ibb.co/GtLSByz/thumbnail-image-2.png'>"
                        + "</div>", senha);
            }
            else if (utilizaApp == false && utilizaWEBApp == true)
            {
                conteudo = string.Format("<div style='max-width: 580; text-align: center;'>"
                        + "<img src ='https://i.ibb.co/2MH6RBm/thumbnail-image-1.png' ><br>"
                        + "<p><b>Web APP Pontofopag habilitado para uso</b></p> <br><br>"
                        + "O aplicativo Pontofopag para registro de Ponto está habilitado e disponível para seu uso.<br> Basta acessar o link <a href='https://webapp.pontofopag.com.br'>webapp.pontofopag.com.br</a>.  <br>"
                        + "No momento do login, se identifique com seu <b>CPF e utilize a senha:{0}</b> <br>"
                        + "Qualquer dúvida na utilização, contate seu RH.  <br><br>"
                        + "Att. <br>"
                        + "Equipe Pontofopag"
                        + "<br><img src = 'https://i.ibb.co/GtLSByz/thumbnail-image-2.png'>"
                        + "</div>", senha);
            }
            else if (utilizaApp == true && utilizaWEBApp == true)
            {
                conteudo = string.Format("<div style='max-width: 580; text-align: center;'>"
                        + "<img src='https://i.ibb.co/2MH6RBm/thumbnail-image-1.png' ><br>"
                        + "<p><b>Web APP Pontofopag e Aplicativo Pontofopag habilitados para uso</b></p> <br><br>"
                        + "O aplicativo Pontofopag para registro de Ponto está habilitado e disponível para seu uso.<br> Basta acessar o link <a href='https://webapp.pontofopag.com.br'>webapp.pontofopag.com.br</a> <br> e baixar o app através da Play Store ou Apple Store e instalar.  <br>"
                        + "No momento do login, se identifique com seu <b>CPF e utilize a senha:{0}</b> <br>"
                        + "Qualquer dúvida na utilização, contate seu RH.  <br><br>"
                        + "Att. <br>"
                        + "Equipe Pontofopag"
                        + "<br><img src='https://i.ibb.co/GtLSByz/thumbnail-image-2.png'>"
                        + "</div>", senha);
            }

            return conteudo;
        }

        private string GeraConteudoSMS(bool utilizaApp, bool utilizaWEBApp, string senha)
        {
            string conteudo = "";

            if (utilizaApp == true && utilizaWEBApp == false)
            {
                conteudo = string.Format("Para acessar o App do Ponto utilize seu CPF e senha: {0}, disponível na Play Store e Apple Store.", senha);
            }
            else if (utilizaApp == false && utilizaWEBApp == true)
            {
                conteudo = string.Format("Para acessar o Web App do Ponto utilize seu CPF e senha: {0}, disponível no link www.webapp.pontofopag.com.br.", senha);
            }
            else if (utilizaApp == true && utilizaWEBApp == true)
            {
                conteudo = string.Format("Para acessar o Web App e App do Ponto do Ponto utilize seu CPF e senha: {0}, disponível no link www.webapp.pontofopag.com.br e na Play Store e Apple Store.", senha);
            }

            return conteudo;
        }

        private void PreencheTipoVinculo(string connString, UsuarioPontoWeb usuarioPontoWebLogado)
        {
            BLL.TipoVinculo bllTipoVinculo = new BLL.TipoVinculo(connString, usuarioPontoWebLogado);
            List<Modelo.TipoVinculo> campos = bllTipoVinculo.GetAllList();
            List<SelectListItem> sli = campos.Select(i => new SelectListItem
            {
                Text = i.Descricao,
                Value = i.Id.ToString()
            }).ToList();
            sli.Add(new SelectListItem { Value = "0", Text = "" });
            ViewBag.Campos = sli;
        }

        private void TrataErroInclusaoCentralCliente()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("Não é possível efetuar o cadastro com o campo Utiliza Registrador habilitado, desmarque o campo para continuar. ");
            strBuilder.AppendLine("Por favor, entre em contato com o suporte.");
            string erro = strBuilder.ToString();
            ModelState.AddModelError("CustomError", erro);
        }

        private static bool AdicionaAlteraFuncionariosCentralCliente(Modelo.Funcionario func, Modelo.Funcionario funcAnterior)
        {
            bool bGravou = false;
            using (var db = new CentralCliente.CENTRALCLIENTEEntities())
            {
                BLL.Empresa bllEmpresa = new BLL.Empresa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                Modelo.Empresa empresaPrincipal = bllEmpresa.GetEmpresaPrincipal();

                CentralCliente.Funcionarios funcsCC = null;
                funcsCC = BuscaFuncionariosCentralCliente(funcAnterior, db);
                if (funcsCC == null)
                {
                    funcsCC = BuscaFuncionariosCentralCliente(func, db);
                }

                if (funcsCC == null)
                {
                    string cnpjEmpPrincipalSemSimbolos = empresaPrincipal.Cnpj.Replace("-", "").Replace(".", "").Replace("/", "");

                    CentralCliente.Cliente cliente = db.Cliente.Where(x => x.Entidade.CNPJ_CPF.Replace("-", "").Replace(".", "").Replace("/", "").Trim().Equals(cnpjEmpPrincipalSemSimbolos)).FirstOrDefault();

                    funcsCC = new CentralCliente.Funcionarios();
                    if (cliente != null)
                    {
                        funcsCC.IDCliente = cliente.ID;
                    }
                    else
                    {
                        funcsCC.IDCliente = 0;
                    }
                }

                if (funcsCC.IDCliente != 0)
                {
                    funcsCC.Login = func.CPF;
                    funcsCC.Senha = func.Mob_Senha;

                    if (funcsCC.ID == 0)
                    {
                        funcsCC.incdata = func.Incdata;
                        funcsCC.inchora = func.Inchora;
                        funcsCC.incusuario = func.Incusuario;
                        db.Funcionarios.Add(funcsCC);
                    }
                    else
                    {
                        funcsCC.altdata = func.Altdata;
                        funcsCC.althora = func.Althora;
                        funcsCC.altusuario = func.Altusuario;
                        db.Entry(funcsCC).State = EntityState.Modified;
                    }

                    db.SaveChanges();

                    bGravou = true;
                }
            }

            return bGravou;
        }

        private static CentralCliente.Funcionarios BuscaFuncionariosCentralCliente(Modelo.Funcionario func, CentralCliente.CENTRALCLIENTEEntities db)
        {
            CentralCliente.Funcionarios funcsCC = db.Funcionarios.Where(x => x.Login == func.CPF).FirstOrDefault();
            return funcsCC;
        }

        private static void RemoveFuncionariosCentralCliente(Modelo.Funcionario func)
        {
            using (var db = new CentralCliente.CENTRALCLIENTEEntities())
            {
                CentralCliente.Funcionarios funcsCC = BuscaFuncionariosCentralCliente(func, db);
                if (funcsCC != null)
                {
                    db.Funcionarios.Remove(funcsCC);
                    db.SaveChanges();
                }
            }
        }

        private static Modelo.Empresa GetEmpresaFuncionario(Funcionario funcionario, BLL.Empresa bllEmpresa)
        {
            Modelo.Empresa empresaRetorno = new Empresa();

            int idEmpresa = 0;
            int codEmpresa;
            string[] empresaArray = funcionario.Empresa.Split('|');
            if (empresaArray != null)
            {
                string codEmpresaString = empresaArray.FirstOrDefault().Trim();
                Int32.TryParse(codEmpresaString, out codEmpresa);

                idEmpresa = bllEmpresa.getId(codEmpresa, null, null);

                empresaRetorno = bllEmpresa.LoadObject(idEmpresa);
            }

            return empresaRetorno;
        }

        private Acao EscolhaDaAcao(Funcionario funcionario)
        {
            Acao acao = new Acao();
            if (funcionario.Id == 0)
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

        protected override ActionResult GetPagina(int id)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);
            BLL.FuncionarioHistorico bllFuncHist = new BLL.FuncionarioHistorico(conn, usr);
            BLL.Biometria bllBiometria = new BLL.Biometria(conn, usr);
            BLL.FuncionarioRFID bllFuncionarioRFID = new BLL.FuncionarioRFID(conn, usr);


            Empresa empresaFuncionario = new Empresa();
            Funcionario funcionario = new Funcionario();
            PreencheTipoVinculo(conn, usr);
            funcionario = bllFuncionario.LoadObject(id);
            funcionario.Biometrias = bllBiometria.GetBiometriaTipoBiometria(id);
            funcionario.FuncionarioRFID = bllFuncionarioRFID.GetAllListByFuncionario(id, true).ToList();

            BLL.Parametros bllparm = new BLL.Parametros(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            Modelo.Parametros parm = new Parametros();
            parm = bllparm.LoadPrimeiro();
            ViewBag.BloqueiaDadosIntegrados = parm.BloqueiaDadosIntegrados;

            if (id == 0)
            {
                funcionario.Codigo = bllFuncionario.MaxCodigo();
                funcionario.Dscodigo = funcionario.Codigo.ToString();
                funcionario.TipoMaoObra = 0;
                funcionario.bFuncionarioativo = true;
                ViewBag.Disabled = false;
            }
            else
            {
                ViewBag.Disabled = true;
                #region Valida Fechamento
                if (ViewBag.Consultar != 1)
                {
                    string msg = "";
                    if (!validaFechamento(bllFuncionario, funcionario, out msg))
                    {
                        @ViewBag.MensagemFechamento = msg;
                    }
                }
                #endregion
            }
            if (!String.IsNullOrEmpty(funcionario.Senha))
            {
                string nsenha = "";
                funcionario.Senha = bllFuncionario.GetSenha(funcionario);
                //funcionario.Senha = nsenha.PadLeft(bllFuncionario.GetSenha(funcionario).Length, '#');
            }

            if (!String.IsNullOrEmpty(funcionario.Mob_Senha))
            {
                string mobsenha = "";
                funcionario.Mob_Senha = mobsenha.PadLeft(bllFuncionario.GetMobSenha(funcionario).Length, '#');
            }

            funcionario.Historico = bllFuncHist.LoadPorFuncionario(funcionario.Id);

            AdicionaFotoPadrao(funcionario);
            funcionario.Tipohorario = funcionario.IdHorarioDinamico.GetValueOrDefault() > 0 ? Convert.ToInt16(3) : funcionario.Tipohorario;
            return View("Cadastrar", funcionario);
        }

        private bool validaFechamento(BLL.Funcionario bllFunc, Modelo.Funcionario func, out string erro)
        {
            List<FechamentoPonto> lFechamentoPonto = bllFunc.FechamentoPontoFuncionario(new List<int> { func.Id });
            if (lFechamentoPonto.Count > 0)
            {
                func.DataUltimoFechamento = lFechamentoPonto.Select(s => s.DataFechamento).Max(m => m.Date);
                erro = "Algumas informações não poderão ser editadas, existe fechamento de ponto relacionado a esse registro. Fechamento: <br/> " + String.Join("<br/> ", lFechamentoPonto.Select(s => "Código: " + s.Codigo + " Data: " + s.DataFechamento.ToShortDateString()));
                return false;
            }
            erro = "";
            return true;
        }

        protected override void ValidarForm(Funcionario funcionario)
        {
            ValidaEmpresa(funcionario);
            ValidaDepartamento(funcionario);
            ValidaContrato(funcionario);
            ValidaFotoFuncionario(funcionario);
            ValidaFuncao(funcionario);
            ValidaHorario(funcionario);
            ValidaSupervisor(funcionario);
            ValidaPessoaSupervisor(funcionario);
            ValidaFechamento(funcionario);
            ValidaAlocacao(funcionario);
        }

        [Authorize]
        public ActionResult EventoConsulta(String consulta, String filtro)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            List<Funcionario> lFunc = new List<Funcionario>();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            int idDepartamento = 0;
            // T é parametro para todos, por tanto não deve filtrar nada
            if (filtro != "T")
            {
                idDepartamento = DepartamentoController.BuscaIdDepartamento(filtro);
            }
            Int64 codigo = -1;
            try { codigo = Int64.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Funcionario func = bllFuncionario.RetornaFuncDsCodigo(consulta);
                // Se existir o departamento pesquisado e pertencer a empresa selecionada ou se for para trazer todos independente de empresa add na lista
                if (func != null && func.Id > 0 && (func.Iddepartamento == idDepartamento || filtro == "T"))
                {
                    lFunc.Add(func);
                }
            }

            // Se pesquisou tudo ou não encontrou o codigo pesquisado, retorna todos os departamentos
            if (lFunc.Count == 0)
            {
                if (!String.IsNullOrEmpty(consulta))
                {
                    lFunc = bllFuncionario.GetAllListLike(false, false, consulta);
                }
                else
                {
                    lFunc = bllFuncionario.GetAllList(false, false);
                }

                // Se p Filtro diferente de todos, retorna apenas os departamentos da empresa selecionada.
                if (filtro != "T")
                {
                    lFunc = lFunc.Where(d => d.Iddepartamento == idDepartamento).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Funcionários";
            return View(lFunc);
        }

        [Authorize]
        public ActionResult EventoConsultaComInativos(String consulta, String filtro)
        {
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            List<Funcionario> lFunc = new List<Funcionario>();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, usr);
            int idDepartamento = 0;
            // T é parametro para todos, por tanto não deve filtrar nada
            if (filtro != "T")
            {
                idDepartamento = DepartamentoController.BuscaIdDepartamento(filtro);
            }
            Int64 codigo = -1;
            try { codigo = Int64.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                Funcionario func = bllFuncionario.RetornaFuncDsCodigo(consulta);
                // Se existir o departamento pesquisado e pertencer a empresa selecionada ou se for para trazer todos independente de empresa add na lista
                if (func != null && func.Id > 0 && (func.Iddepartamento == idDepartamento || filtro == "T"))
                {
                    lFunc.Add(func);
                }
            }

            // Se pesquisou tudo ou não encontrou o codigo pesquisado, retorna todos os departamentos
            if (lFunc.Count == 0)
            {
                if (!String.IsNullOrEmpty(consulta))
                {
                    lFunc = bllFuncionario.GetAllListLike(true, false, consulta);
                }
                else
                {
                    lFunc = bllFuncionario.GetAllList(true, false);
                }

                // Se p Filtro diferente de todos, retorna apenas os departamentos da empresa selecionada.
                if (filtro != "T")
                {
                    lFunc = lFunc.Where(d => d.Iddepartamento == idDepartamento).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Funcionários";
            return View("EventoConsulta", lFunc);
        }

        [Authorize]
        public ActionResult GetTabelaMarcacao(String consulta, String filtro, String tipo)
        {
            UsuarioPontoWeb usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(usr.ConnectionString, usr);
            int idIdentificacao = 0;
            if (tipo == "5" && !string.IsNullOrEmpty(filtro))
            {
                BLL.Contrato bllContrato = new BLL.Contrato(usr.ConnectionString, usr);
                idIdentificacao = bllContrato.GetIdByConsulta(filtro);
            }
            else if (tipo == "2")
            {
                idIdentificacao = DepartamentoController.BuscaIdDepartamento(filtro);
            }
            else
            {
                idIdentificacao = EmpresaController.BuscaIdEmpresa(filtro);
            }

            IList<Funcionario> lFunc = new List<Funcionario>();
            Int64 codigo = -1;
            try { codigo = Int64.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                int id = bllFuncionario.GetIdDsCodigo(consulta);
                Funcionario func = bllFuncionario.LoadObject(id);
                int ContratoFuncionario = tipo == "5" && !string.IsNullOrEmpty(filtro) ? Convert.ToInt32(func.Contrato.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0]) : 0;

                if (func.Funcionarioativo == 1 && func.Excluido == 0 && func != null && func.Id > 0 && ((tipo == "2" && func.Iddepartamento == idIdentificacao) || (tipo == "1" && func.Idempresa == idIdentificacao) || tipo == "3" || (tipo == "5" && ContratoFuncionario == idIdentificacao)))
                {
                    lFunc.Add(func);
                }
            }

            if (lFunc.Count == 0 && (tipo == "1" || tipo == "2" || tipo == "3" || tipo == "5"))
            {
                lFunc = bllFuncionario.GetTabelaMarcacao(Convert.ToInt32(tipo), idIdentificacao, consulta);

                if (!String.IsNullOrEmpty(consulta))
                {
                    lFunc = lFunc.Where(p => p.Nome.ToUpper().Contains(consulta.ToUpper())).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Funcionários";
            return View("EventoConsulta", lFunc);
        }

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*", VaryByCustom = "User")]
        public ActionResult ProximoFuncionario(String consulta, String tipoFiltro, String filtro, String tipoPosicao)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            BLL.Contrato bllContrato = new BLL.Contrato(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            JsonResult retornoErro = Json(new { Funcionario = "", Primeiro = true, Ultimo = false }, JsonRequestBehavior.AllowGet);
            if (tipoFiltro == "1" || tipoFiltro == "2" || tipoFiltro == "3" || tipoFiltro == "5")
            {
                try
                {
                    int idIdentificacao = 0;
                    bool primeiro = false;
                    bool ultimo = false;
                    if (tipoFiltro == "2")
                    {
                        idIdentificacao = DepartamentoController.BuscaIdDepartamento(filtro);
                    }
                    else if (tipoFiltro == "5")
                    {
                        idIdentificacao = bllContrato.LoadPorCodigo(Convert.ToInt32(filtro.Split(new string[] { " | " }, StringSplitOptions.None)[0])).Id;
                    }
                    else
                    {
                        idIdentificacao = EmpresaController.BuscaIdEmpresa(filtro);
                    }


                    Funcionario proximoFunc = null;
                    Modelo.Funcionario func = new Modelo.Funcionario();
                    Int64 codigo = -1;
                    try { codigo = Int64.Parse(consulta); }
                    catch (Exception) { codigo = -1; }
                    if (codigo != -1)
                    {
                        func = bllFuncionario.RetornaFuncDsCodigo(consulta);
                    }

                    List<Funcionario> lFunc = new List<Funcionario>();
                    if (tipoPosicao == "1") //Primeiro Registro
                    {
                        lFunc = bllFuncionario.GetProximoOuAnterior(Convert.ToInt32(tipoFiltro), idIdentificacao, 10, "", 0, 1);
                        proximoFunc = lFunc.FirstOrDefault();
                    }
                    else
                        if (tipoPosicao == "2") // Registro Anterior
                    {
                        if (func.Id > 0)
                        {
                            lFunc = bllFuncionario.GetProximoOuAnterior(Convert.ToInt32(tipoFiltro), idIdentificacao, 10, func.Nome, 1, 1);
                            lFunc = lFunc.OrderBy(o => o.Nome).ToList();
                            int posicaoAtual = lFunc.IndexOf(lFunc.Where(x => x.Id == func.Id).FirstOrDefault());
                            if (posicaoAtual > 0)
                            {
                                proximoFunc = lFunc[posicaoAtual - 1];
                            }
                            else
                            {
                                proximoFunc = lFunc.FirstOrDefault();
                            }
                        }
                        else
                        {
                            lFunc = bllFuncionario.GetProximoOuAnterior(Convert.ToInt32(tipoFiltro), idIdentificacao, 10, func.Nome, 0, 1);
                            proximoFunc = lFunc.FirstOrDefault();
                        }
                    }
                    else
                            if (tipoPosicao == "3") // Proximo Registro
                    {
                        if (func.Id > 0)
                        {
                            lFunc = bllFuncionario.GetProximoOuAnterior(Convert.ToInt32(tipoFiltro), idIdentificacao, 10, func.Nome, 0, 0);
                            int posicaoAtual = lFunc.IndexOf(lFunc.Where(x => x.Id == func.Id).FirstOrDefault());
                            if (lFunc.Count() > posicaoAtual + 1)
                            {
                                proximoFunc = lFunc[posicaoAtual + 1];
                            }
                        }
                        else
                        {
                            lFunc = bllFuncionario.GetProximoOuAnterior(Convert.ToInt32(tipoFiltro), idIdentificacao, 10, "", 0, 0);
                            proximoFunc = lFunc.FirstOrDefault();
                        }
                    }
                    else
                                if (tipoPosicao == "4") // Ultimo registro
                    {
                        lFunc = bllFuncionario.GetProximoOuAnterior(Convert.ToInt32(tipoFiltro), idIdentificacao, 10, "", 1, 0);
                        lFunc = lFunc.OrderBy(o => o.Nome).ToList();
                        proximoFunc = lFunc.LastOrDefault();
                    }


                    if (proximoFunc != null)
                    {
                        int posicaoAtual = lFunc.IndexOf(proximoFunc);
                        if (posicaoAtual + 1 == 1)
                        {
                            primeiro = true;
                        }
                        if (lFunc.Count() <= posicaoAtual + 1)
                        {
                            ultimo = true;
                        }
                    }
                    else
                    {
                        return retornoErro;
                    }

                    return Json(new { Funcionario = proximoFunc.Dscodigo + " | " + proximoFunc.Nome, Primeiro = primeiro, Ultimo = ultimo }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    return Json(new { funcionario = "Erro ao Buscar Funcionario", primeiro = true, ultimo = false }, JsonRequestBehavior.AllowGet);
                }
            }
            return retornoErro;
        }

        [PermissoesFiltro(Roles = "FuncionarioConsultar")]
        public ActionResult ConsultaHorario(int id)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(conn, usr);
            BLL.Horario bllHorario = new BLL.Horario(conn, usr);
            if (id > 0)
            {
                Funcionario funcionario = new Funcionario();
                funcionario = bllFuncionario.LoadObject(id);
                if (funcionario.Idhorario > 0)
                {
                    Horario h = bllHorario.LoadObject(funcionario.Idhorario);
                    if (h.TipoHorario == 1) // normal
                    {
                        return RedirectToAction("ConsultarExt", "Horario", new { id = h.Id, ctrl = "Funcionario", acao = "Grid" });
                    }
                    else
                    {
                        return RedirectToAction("ConsultarExt", "HorarioMovel", new { id = h.Id, ctrl = "Funcionario", acao = "Grid" });
                    }
                }
                else
                {
                    return View();
                }
            }
            return View();
        }


        public static int BuscaIdFuncionario(string funcionario)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            int id = 0;
            try
            {
                if (!String.IsNullOrEmpty(funcionario))
                {

                    Funcionario func = new Funcionario();
                    string cod = funcionario.Split('|')[0].Trim();
                    Int64 codigo = Convert.ToInt64(cod);
                    id = bllFuncionario.GetIdDsCodigo(cod);
                }
                return id;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void tratamentoSenhas(Funcionario funcionario, Funcionario DadosAntFunc)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
            /*Metodo necessário para tratar a senha se foi alterada ou não, pois o asp.net mvc não carrega a senha na edição
              Até existe o modo como foi feito para carregar a senha na tela, mas isso causa um problema de segurança, é possivel ver a senha no código fonte da página.
              por isso na edição a senha é alterada para ###### e se no momento de gravar estiver com esses caracteres ainda o sistema recoloca a senha antiga devolta.*/
            if (!String.IsNullOrEmpty(funcionario.Senha))
            {
                string nsenha = "";
                if (funcionario.Senha != nsenha.PadLeft(bllFuncionario.GetSenha(DadosAntFunc).Length, '#'))
                {
                    bllFuncionario.SetSenha(funcionario, funcionario.Senha.ToString());
                }
                else
                {
                    funcionario.Senha = DadosAntFunc.Senha;
                }
            }

            if (!String.IsNullOrEmpty(funcionario.Mob_Senha))
            {
                string mobsenha = "";
                if (funcionario.Mob_Senha != mobsenha.PadLeft(bllFuncionario.GetMobSenha(DadosAntFunc).Length, '#'))
                {
                    bllFuncionario.SetMobSenha(funcionario, funcionario.Mob_Senha.ToString());
                }
                else
                {
                    funcionario.Mob_Senha = DadosAntFunc.Mob_Senha;
                }
            }
        }

        private void TrataErros(Dictionary<string, string> erros)
        {
            List<string> propriedades = typeof(Modelo.Funcionario).GetProperties().Select(s => s.Name).ToList();

            List<string> errosCustom = new List<string>();
            if (erros.Count() > 0)
            {
                foreach (var err in erros)
                {
                    string prop = propriedades.Where(w => w == err.Key).FirstOrDefault();
                    if (!String.IsNullOrEmpty(prop))
                    {
                        ModelState.AddModelError(prop, err.Value);
                    }
                    else
                    {
                        errosCustom.Add(err.Key + " = " + err.Value);
                    }
                }
            }

            if (errosCustom.Count > 0)
            {
                string erro = string.Join(";", errosCustom);
                ModelState.AddModelError("CustomError", erro);
            }
        }

        public static void SendPassEmail(string server)
        {

        }

        #region Validações

        private static void ValidaFotoFuncionario(Funcionario funcionario)
        {

            if (funcionario.Foto.Contains("data:image/"))
            {
                string base64 = ";base64,";
                int posicao = funcionario.Foto.IndexOf(base64);
                posicao += base64.Length;
                funcionario.Foto = funcionario.Foto.Substring(posicao);
            }
            if (funcionario.Foto == "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")
            {
                funcionario.Foto = null;
            }
        }

        private static void AdicionaFotoPadrao(Funcionario funcionario)
        {
            if (string.IsNullOrEmpty(funcionario.Foto))
            {
                funcionario.Foto = "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAIAAABMXPacAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAABp0RVh0U29mdHdhcmUAUGFpbnQuTkVUIHYzLjUuMTFH80I3AAAGtklEQVR4Xu2dUYLcIAiG9/433dlLlB1SSlUQFdRk/B6mBlGRX0z2qV+vw+v1/f19tTz4+fm5WnKb+AIrcdkOf8mF8ZKKsn0qYIhxPRoE8K3TA9JWAeeassPPq9QGhq6go8cgIMYlwO8r+C9oOczhvIQXMyrAqZhBegQ4SXfkXEGLOQIs5hKAPk7f30EXaDmE4lMBR61uzhW0mCEBkoOPj/ir/PGN5EajG2Jxbhq7ilMBF6tUCRHgSa+EXBhfqU4FjDKoh7MA53Oolf8EIDEhjwRaDh3w4pDa5x2wmPMOWIyDAMXzforAiH8FnNQ3ca6gxZQFOJ9D0wipgFwtiwWQZLY4N43dhw+9gvZRxVOA4pHkf3TsTy5MtFTnJdyGux5HgMUsFkA6UEV7YmwaOwG+rtTO0QQY/xht9Y9jk0jyMCZVQLKwlI6ifWRslXxU3zx9wFozBJi5pYTxpaOD9xegI+Lip2pilD5n+z5z8yBXnZK6AONvAiAfApY8d4mRHhPP4liO3mshDziH+0jtKuFXUFM0I8BC1bznDuNS6VS3P+kl3AdmB/YgbaO6PQB8kiznFsKiB19UakvkPlECtEbWBKQJ5rRPm3ja9ejGHtvWFdCNnlB7djh8TqktkayIj/g7TwB92+5nMKEv6TCKApPanI5dzK6AvkT3jSrCk4gNC4qzJAaQ2HES/guYBOBBE2ix0zFkT/SNQK/9uIDzM98BvuhFUz1YOJz/cmYLQOHqu9qHPGUWYFMwkP9eHW/wEX6BQAGUzBqTjoFuAgVTDL66I5QB3fi+nnAFrdJJX9d4yKwCcP0JtBxGWFMBg+KR/KvOviNTBaDEeSHN5ruKTtNa/CLBRoMAHbdQ1SGOpDgwkgkV07rEgitIV0XplboUu5QO6KJRvN1Hvop9wtgK0JlwHgfR91jdftUBCK8ASxCbk5w8bHsx+wraSg8pmKI9KPLwK8jio6PMIHW12mfC0wi/a94BNLB7BnfG99LH7CsIkCLWd6L0tk6oLzSZ9VeQZZJpSMHoQVJmOEVjTngF5KGjZau8x1Hd5oIr6L50HxploEmAkdev5El2+1S74RJ5XQDHjx/EMlz3UXqlrlb7NCoCuGcfyGcYnzOC6Kgwt4HvAH0DvLfoaR+eIHW12uegCRBx9QPYlTvQcvtg2a/kY9yOKEBQ9ovg5BsKME41FQUBRlJvJ2LOVVSPjrJZ/3eAslhHl743pVfqarVzEh+XM+QsgG/2P4Evriq1ISMEWqronrw39ySL5Rg+BtzsVQEjqW9CmjZouf25BHgn/AItjtCc0uTvZX+77lgB0qaMOLwD9AgG43s8QwI0pX5/nXgMTXdysXCN1dwjgBKKhDQksd/xCtKp5srhCtJRIsi7lghQzZGEfaDiaRUApzAuaXEjH+OcT8W5Alqzmfg/7wqqUhEAEmTJadFHGYhdlplnMll+XO73P3SGf+A3T0fRaEEf1TenC1KKyf7e8QVaqtg9ixQqwLg8+nDP6ijF+XmXj3FH4V9BRR6WfSX+6qH8TwBjJXIjtotuRXLPJdk37lTHPkTx7K+Apog7tkfoCim9S6RtRRTA5YwA+qhb5CgUh3dAMcW5MbdEZ19ZEboItFhocq6CwTQIoEcPbSmh3A2JTv1M8t014VABCRTQYGReDJ76aPwFmIYljy7Zrw4pFrSxyocEsGwvsRjD6gZjAK7n1VQjmVcB0an3xVdCZbZNr6Ak4uIGuNHStmMc1Td5wgIBXOLuI9EGH7FBYO80QgSwvBskYKzd+dZglja9goDdZAiKZwsBYG/VV3Sy/2I6uNHStqOMkrqMHx2zBeC3EzY4fdkZga8IbXzEBoG9SPJYpeq/6RUkxW1JBzda2nakUa3ly9n3HQD0pamDRBt8xAaBvZyisZVLAOOFFQe/mgi0PJt9K2D5mYgGN7j1FaQA9fEMhW4gAL+dsMEhJaTeq2VrdzA4vCLAjU7ZYCKIRBt8xAaBvS7c9QriZYHgYw7vsrQ7KB5T49m9qwAWkhRAlo2J5m40ChsIdlmoOvcLcKPbKQi7EopnWYDnJRd3RPuiBqYGfpUchfLkK+gW+Avw1KuJashrgzhPswBey9+R4jU1eHelAqy6Cp+H8aROfQc8rHrosCr7qh5oNwE++WrCvUOuMd3UIJJHToMAyiwj3Fq58ZxMvYKa4MI8uLxmCPD422mkDhwECLqaJFBO+sXGfVl2Bd09cV78E2DyQZbgwnyCSLECbCLqzmxXARJYDfSLjQ1pTaPpHeCrzSkLzgIBJPgqtxapKfiNBPhEXq8/jTiObrrUuFUAAAAASUVORK5CYII=";
            }
        }

        private void ValidaSupervisor(Funcionario funcionario)
        {
            UsuarioPontoWeb _user = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.UsuarioPontoWeb bllUser = new BLL.UsuarioPontoWeb(_user.ConnectionString, _user);
            if (!String.IsNullOrEmpty(funcionario.Supervisor))
            {
                try
                {
                    int codigo = Convert.ToInt32(funcionario.Supervisor.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    UsuarioPontoWeb user = bllUser.LoadObjectByCodigo(codigo);
                    funcionario.IdCw_Usuario = user.Id;
                }
                catch (Exception)
                {
                    funcionario.IdCw_Usuario = null;
                }
            }
        }

        private void ValidaPessoaSupervisor(Funcionario funcionario)
        {
            BLL.Pessoa bllPessoa = new BLL.Pessoa(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);
            if (!String.IsNullOrEmpty(funcionario.PessoaSupervisor))
            {
                try
                {
                    int codigo = Convert.ToInt32(funcionario.PessoaSupervisor.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0]);
                    Pessoa pessoa = bllPessoa.GetPessoaPorCodigo(codigo).FirstOrDefault();
                    funcionario.IdPessoaSupervisor = pessoa.Id;
                }
                catch (Exception)
                {
                    funcionario.IdPessoaSupervisor = null;
                }
            }
        }

        private void ValidaEmpresa(Funcionario funcionario)
        {
            if (!String.IsNullOrEmpty(funcionario.Empresa))
            {
                int idEmpresa = EmpresaController.BuscaIdEmpresa(funcionario.Empresa);
                if (idEmpresa > 0)
                {
                    funcionario.Idempresa = idEmpresa;
                }
                else
                {
                    ModelState["Empresa"].Errors.Add("Empresa " + funcionario.Empresa + " não cadastrada!");
                }
            }
        }


        private void ValidaAlocacao(Funcionario funcionario)
        {
            if (!String.IsNullOrEmpty(funcionario.Alocacao))
            {
                int idAlocacao = AlocacaoController.BuscaIdAlocacao(funcionario.Alocacao);
                if (idAlocacao > 0)
                { funcionario.IdAlocacao = idAlocacao; }
                else
                { ModelState["Alocacao"].Errors.Add("Alocação " + funcionario.Alocacao + " não cadastrada!"); }
            }
            else
            {
                funcionario.IdAlocacao = null;
            }

        }


        private void ValidaDepartamento(Funcionario funcionario)
        {
            int idDepartamento = DepartamentoController.BuscaIdDepartamento(funcionario.Departamento);
            if (idDepartamento > 0)
            {
                BLL.Departamento bllDepartamento = new BLL.Departamento(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                Departamento dep = bllDepartamento.LoadObject(idDepartamento);
                if (dep.IdEmpresa == funcionario.Idempresa)
                { funcionario.Iddepartamento = idDepartamento; }
                else
                { ModelState["Departamento"].Errors.Add("Departamento " + funcionario.Departamento + " não pertence a empresa selecionada!"); }
            }
            else
            {
                ModelState["Departamento"].Errors.Add("Departamento " + funcionario.Departamento + " não cadastrado!");
            }
        }

        private void ValidaFuncao(Funcionario funcionario)
        {
            int idFuncao = FuncaoController.BuscaIdFuncao(funcionario.Funcao);
            if (idFuncao > 0)
            { funcionario.Idfuncao = idFuncao; }
            else
            { ModelState["Funcao"].Errors.Add("Função " + funcionario.Funcao + " não cadastrada!"); }
        }


        private void ValidaHorario(Funcionario funcionario)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.Horario bllHorario = new BLL.Horario(conn, usr);

            int idHorario = 0;

            switch (funcionario.Tipohorario)
            {
                case 1:
                    idHorario = HorarioController.BuscaIdHorario(funcionario.Horario, funcionario.Tipohorario);
                    if (idHorario > 0) funcionario.Idhorario = idHorario;
                    else ModelState["Horario"].Errors.Add("Horário " + funcionario.Horario + " não cadastrado!");
                    ModelState["HorarioDinamico"].Errors.Clear();
                    break;
                case 2:
                    idHorario = HorarioMovelController.BuscaIdHorario(funcionario.Horario);
                    if (idHorario > 0) funcionario.Idhorario = idHorario;
                    else ModelState["Horario"].Errors.Add("Horário " + funcionario.Horario + " não cadastrado!");
                    ModelState["HorarioDinamico"].Errors.Clear();
                    break;
                case 3:
                    idHorario = HorarioDinamicoController.BuscaIdHorario(funcionario.HorarioDinamico);
                    if (idHorario > 0) funcionario.IdHorarioDinamico = idHorario;
                    else ModelState["HorarioDinamico"].Errors.Add("Horário " + funcionario.HorarioDinamico + " não cadastrado!");
                    ModelState["Horario"].Errors.Clear();
                    break;
                default:
                    idHorario = HorarioController.BuscaIdHorario(funcionario.Horario, funcionario.Tipohorario);
                    break;
            }
        }

        private void ValidaFechamento(Funcionario funcionario)
        {
            if (funcionario.Dataadmissao != funcionario.Dataadmissao_Ant && funcionario.Dataadmissao <= funcionario.DataUltimoFechamento)
            {
                ModelState["Dataadmissao"].Errors.Add("A data de admissão não pode ser menor que a data do último fechamento.");
            }

            if (funcionario.Datademissao != funcionario.Datademissao_Ant && funcionario.Datademissao <= funcionario.DataUltimoFechamento)
            {
                ModelState["Datademissao"].Errors.Add("A data de demissão não pode ser menor que a data do último fechamento.");
            }
        }
        private void ValidaContrato(Funcionario funcionario)
        {
            if (funcionario.Contrato != null)
            {
                int CodContrato = Convert.ToInt32(funcionario.Contrato.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[0]);
                BLL.Contrato bllContrato = new BLL.Contrato(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                bool validacontrato = bllContrato.ValidaContratoCodigo(CodContrato, funcionario.Idempresa);
                if (validacontrato == false)
                { ModelState["Contrato"].Errors.Add("Contrato " + funcionario.Contrato + " não pertence a empresa selecionada!"); }
            }
        }

        #endregion
    }
}