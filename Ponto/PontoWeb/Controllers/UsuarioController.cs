using Employer.Componentes.UI.WebMVC.Helpers;
using Modelo;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using PontoWeb.Security;
using SimpleCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace PontoWeb.Controllers
{
    public class UsuarioController : Controller
    {
        CentralCliente.Usuario usuarioCentralCliente;

        #region Padrão Novo
        [PermissoesFiltro(Roles = "Usuario")]
        [HttpGet]
        public ActionResult Grid()
        {
            BLL.UsuarioPontoWeb bllUsuarioPontoWeb = new BLL.UsuarioPontoWeb(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);
            return View(new Modelo.Proxy.PxyGridUsuario());
        }

        [Authorize]
        [OutputCache(Duration = 10)] // Cache dura apenas 10 sec
        public JsonResult DadosGrid()
        {
            try
            {
                BLL.Cw_GrupoAcesso bllGrupo = new BLL.Cw_GrupoAcesso(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt, Usuario.GetUsuarioPontoWebLogadoCache());
                List<Modelo.Proxy.PxyGridUsuario> GridUsuario = bllGrupo.GetAllGridU();
                JsonResult jsonResult = Json(new { data = GridUsuario }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw;
            }
        }

        [PermissoesFiltro(Roles = "UsuarioConsultar")]
        public ActionResult Consultar(int id)
        {
            ViewBag.Consultar = 1;
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "UsuarioCadastrar")]
        public ActionResult Cadastrar()
        {
            return GetPagina(0);
        }

        [PermissoesFiltro(Roles = "UsuarioAlterar")]
        public ActionResult Alterar(int id)
        {
            return GetPagina(id);
        }

        [PermissoesFiltro(Roles = "UsuarioCadastrar")]
        [HttpPost]
        public ActionResult Cadastrar(UsuarioPontoWeb obj)
        {
            return Salvar(obj);
        }

        [PermissoesFiltro(Roles = "UsuarioAlterar")]
        [HttpPost]
        public ActionResult Alterar(UsuarioPontoWeb obj)
        {
            return Salvar(obj);
        }

        [HttpPost]
        [Authorize]
        [PermissoesFiltro(Roles = "UsuarioExcluir")]
        public ActionResult Excluir(int id)
        {
            Dictionary<string, string> erros = new Dictionary<string, string>();
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            UsuarioPontoWeb objUsuarioLogado = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.UsuarioPontoWeb bllUsuarioPontoWeb = new BLL.UsuarioPontoWeb(conn);
            BLL.Cw_Usuario bllcw_usuario = new BLL.Cw_Usuario(conn, objUsuarioLogado);
            string login = bllcw_usuario.LoadObject(id).Login;
            Modelo.UsuarioPontoWeb objUsuarioSelecionadoPontoWeb = bllUsuarioPontoWeb.LoadObjectLogin(login);
            CentralCliente.Usuario objUsuarioCentralCliente = Usuario.BuscaUsuarioCentralCliente(objUsuarioSelecionadoPontoWeb.Login);

            // Vai deletar o usuário do ponto web nas duas cituações abaixo:
            // Se conseguiu deletar do Central Cliente
            // Se Não encontrou mais o usuário no Central Cliente
            if (Usuario.DeletarUsuarioCentralCliente(objUsuarioCentralCliente) || objUsuarioCentralCliente == null)
            {
                try
                {
                    erros = bllUsuarioPontoWeb.Salvar(cwkAcao.Excluir, objUsuarioSelecionadoPontoWeb);
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
            else
            {
                erros.Add("Central do Cliente", "Não foi possivel excluir o usuário, por favor entre em contato com a revenda");
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                return Json(new { Success = false, Erro = erro }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        private ActionResult Salvar(UsuarioPontoWeb obj)
        {
            cwkAcao acao;
            cw_usuario usuarioLogado = Usuario.GetUsuarioLogadoCache();
            BLL.UsuarioPontoWeb bllUsuarioPontoWeb = new BLL.UsuarioPontoWeb(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);
            BLL.Cw_Usuario bllCwUsuario = new BLL.Cw_Usuario(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);

            Dictionary<string, string> erros = new Dictionary<string, string>();
            ValidarForm(obj);
            if (ModelState.IsValid)
            {
                using (var db = new cworkpontoEntities())
                {
                    try
                    {
                        if (obj.Id == 0)
                        {
                            acao = cwkAcao.Incluir;
                            usuarioCentralCliente = new CentralCliente.Usuario();

                            if (ValidaInclusao(obj, out usuarioCentralCliente, out erros))
                            {
                                usuarioCentralCliente = PreencheObjetoUsuarioCentralCliente(obj, usuarioLogado, usuarioCentralCliente);
                                if (Usuario.IncluiUsuarioCentralCliente(usuarioCentralCliente))
                                {
                                    int idCentralCliente = Usuario.BuscaUsuarioCentralCliente(usuarioCentralCliente.Login).ID;

                                    obj.idUsuarioCentralCliente = idCentralCliente;
                                    obj.Senha = BLL.Cw_Usuario.Cifrar(obj.Senha);
                                    obj.Password = usuarioCentralCliente.Password;
                                    obj.PasswordSalt = usuarioCentralCliente.PasswordSalt;
                                    if (usuarioCentralCliente.UltimoAcesso != null)
                                    {
                                        obj.UltimoAcesso = (DateTime)usuarioCentralCliente.UltimoAcesso;
                                    }
                                    obj.ConnectionString = usuarioCentralCliente.connectionString;
                                    try
                                    {
                                        erros = bllUsuarioPontoWeb.Salvar(acao, obj);
                                    }
                                    catch (Exception)
                                    {
                                        // Caso não consiga incluir no ponto, exclui da central do cliente
                                        Usuario.DeletarUsuarioCentralClienteByUsuario(usuarioCentralCliente.Login);
                                        throw;
                                    }
                                    
                                    if (erros.Count > 0)
                                    {
                                        // Caso não consiga incluir no ponto, exclui da central do cliente
                                        Usuario.DeletarUsuarioCentralClienteByUsuario(usuarioCentralCliente.Login);
                                        string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                                        ModelState.AddModelError("CustomError", erro);
                                    }
                                    else
                                    {
                                        //Limpa e adiciona o usuário novamente para atualizar as permissões (Por Empresa/Contrato/Supervisor no cache.)
                                        if (Usuario.GetUsuarioLogadoCache().login == obj.Login)
                                        {
                                            Usuario.LimpaUsuarioPontoWebLogadoCache();
                                            Usuario.GetUsuarioPontoWebLogadoCache();
                                        }
                                    }
                                }
                                else
                                {
                                    erros.Add("Central do Cliente", "Não foi possivel incluir o usuário, por favor entre em contato com a revenda");
                                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                                    ModelState.AddModelError("CustomError", erro);
                                }
                            }
                            else
                            {
                                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                                ModelState.AddModelError("CustomError", erro);
                            }
                        }
                        else
                        {
                            acao = cwkAcao.Alterar;
                            usuarioCentralCliente = Usuario.BuscaUsuarioCentralClientePorId(obj.idUsuarioCentralCliente);
                            bool flCentralClienteNovo = usuarioCentralCliente != null ? false : true;

                            if (usuarioCentralCliente != null)
                            {
                                usuarioCentralCliente.Login = obj.Login;
                                usuarioCentralCliente.EMAIL = obj.Email;
                            }

                            // Regra:
                            // Se encontrou o Usuario na Central Cliente, vai validar a Alteração.
                            // Se não encontrou, vai validar a Inclusão
                            if (usuarioCentralCliente != null ? ValidaAlteracao(usuarioCentralCliente, out erros) : ValidaInclusao(obj, out usuarioCentralCliente, out erros))
                            {
                                if (obj.SenhaRep == "#SeNhAnAoAlTeRaDa#" && !flCentralClienteNovo)
                                {
                                    Cw_Usuario userOriginal = bllCwUsuario.LoadObject(obj.Id);
                                    obj.SenhaRep = userOriginal.SenhaRep;
                                }

                                usuarioCentralCliente = PreencheObjetoUsuarioCentralCliente(obj, usuarioLogado, usuarioCentralCliente);
                                if (!flCentralClienteNovo ? Usuario.AtualizaUsuarioCentralCliente(usuarioCentralCliente) : Usuario.IncluiUsuarioCentralCliente(usuarioCentralCliente))
                                {
                                    if (flCentralClienteNovo)
                                        obj.idUsuarioCentralCliente = Usuario.BuscaUsuarioCentralCliente(usuarioCentralCliente.Login).ID;
                                    else
                                    {
                                        obj.Senha = usuarioCentralCliente.Senha;
                                        obj.Password = usuarioCentralCliente.Password;
                                        obj.PasswordSalt = usuarioCentralCliente.PasswordSalt;

                                        if (usuarioCentralCliente.UltimoAcesso != null)
                                            obj.UltimoAcesso = (DateTime)usuarioCentralCliente.UltimoAcesso;
                                        obj.ConnectionString = usuarioCentralCliente.connectionString;
                                    }

                                    erros = bllUsuarioPontoWeb.Salvar(acao, obj);
                                    TrataErros(erros);
                                }
                                else
                                {
                                    erros.Add("Central do Cliente", "Não foi possivel alterar o usuário, por favor entre em contato com a revenda");
                                    string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                                    ModelState.AddModelError("CustomError", erro);
                                }

                                //Limpa e adiciona o usuário novamente para atualizar as permissões (Por Empresa/Contrato/Supervisor no cache.)
                                if (Usuario.GetUsuarioLogadoCache().login == obj.Login)
                                {
                                    Usuario.LimpaUsuarioPontoWebLogadoCache();
                                    Usuario.GetUsuarioPontoWebLogadoCache();
                                }
                            }
                            else
                            {
                                List<string> customError = new List<string>();
                                foreach (var erro in erros)
                                {
                                    List<string> propClass = obj.GetType().GetProperties().Select(s => s.Name).ToList();
                                    if (propClass.Contains(erro.Key))
                                    {
                                        ModelState[erro.Key].Errors.Add(erro.Value);
                                    }
                                    else
                                    {
                                        customError.Add(erro.Value);
                                    }
                                }

                                if (customError.Count > 0)
	                            {
                                    ModelState.AddModelError("CustomError", String.Join("; ", customError));
	                            }
                            }
                        }
                        if (erros.Count == 0)
                        {
                            return RedirectToAction("Grid", "Usuario");
                        }
                    }
                    catch (Exception ex)
                    {
                        BLL.cwkFuncoes.LogarErro(ex);
                        ModelState.AddModelError("CustomError", ex.Message);
                    }


                }
            }
            return View("Registrar", obj);
        }

        private ActionResult GetPagina(int id)
        {
            string conn = Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt;
            var usr = Usuario.GetUsuarioPontoWebLogadoCache();
            BLL.UsuarioPontoWeb bllUsuarioPontoWeb = new BLL.UsuarioPontoWeb(conn);
            BLL.Cw_Usuario bllcw_usuario = new BLL.Cw_Usuario(conn, usr);
            Modelo.Cw_Usuario user = bllcw_usuario.LoadObject(id);
            string login = user.Login;
            Modelo.UsuarioPontoWeb usuarioSelecionado = bllUsuarioPontoWeb.LoadObjectLogin(login);
            if (id == 0)
            {
                if (usuarioSelecionado == null)
                    usuarioSelecionado = new UsuarioPontoWeb();
                usuarioSelecionado.Codigo = bllUsuarioPontoWeb.MaxCodigo();
            }
            else
            {
                var objUsuarioCentralCliente = Usuario.BuscaUsuarioCentralCliente(usuarioSelecionado.Login);

                // Se Não encontrar o usuário no Central Cliente, Obriga inserir a senha novamente porque irá criar um novo usuário lá.
                usuarioSelecionado.Senha = objUsuarioCentralCliente != null ? "#SeNhAnAoAlTeRaDa#" : string.Empty;
                usuarioSelecionado.SenhaRep = objUsuarioCentralCliente != null ? "#SeNhAnAoAlTeRaDa#" : string.Empty;
            }

            return View("Registrar", usuarioSelecionado);
        }

        private void TrataErros(Dictionary<string, string> erros)
        {
            Dictionary<string, string> erroUsuario = new Dictionary<string, string>();
            erroUsuario = erros.Where(x => x.Key.Equals("txtLoginRep")).ToDictionary(x => x.Key, x => x.Value);
            if (erroUsuario.Count > 0)
            {
                ModelState["LoginRep"].Errors.Add(string.Join(";", erroUsuario.Select(x => x.Value).ToArray()));
            }

            Dictionary<string, string> erroSenhaRep = new Dictionary<string, string>();
            erroSenhaRep = erros.Where(x => x.Key.Equals("txtSenhaRep")).ToDictionary(x => x.Key, x => x.Value);
            if (erroSenhaRep.Count > 0)
            {
                ModelState["SenhaRep"].Errors.Add(string.Join(";", erroSenhaRep.Select(x => x.Value).ToArray()));
            }

            Dictionary<string, string> erroCPF = new Dictionary<string, string>();
            erroCPF = erros.Where(x => x.Key.Equals("txtCpf")).ToDictionary(x => x.Key, x => x.Value);
            if (erroCPF.Count > 0)
            {
                ModelState["Cpf"].Errors.Add(string.Join(";", erroCPF.Select(x => x.Value).ToArray()));
            }

            erros = erros.Where(x => !x.Key.Equals("txtLoginRep")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("txtSenhaRep")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("txtCpf")).ToDictionary(x => x.Key, x => x.Value);
            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                ModelState.AddModelError("CustomError", erro);
            }
        }

        private CentralCliente.Usuario PreencheObjetoUsuarioCentralCliente(Modelo.UsuarioPontoWeb user, cw_usuario usuarioLogado, CentralCliente.Usuario usuarioCentralCliente)
        {
            ICryptoService crypto = new PBKDF2();
            string salt = crypto.GenerateSalt();
            string passCrypto = crypto.Compute(user.Senha);

            usuarioCentralCliente.Login = user.Login;

            if (user.Senha == "#SeNhAnAoAlTeRaDa#")
            {
                user.Senha = usuarioCentralCliente.Senha;
                user.Password = usuarioCentralCliente.Password;
                user.PasswordSalt = usuarioCentralCliente.PasswordSalt;
            }
            else
            {
                usuarioCentralCliente.Senha = BLL.Cw_Usuario.Cifrar(user.Senha);
                usuarioCentralCliente.PasswordSalt = salt;
                usuarioCentralCliente.Password = passCrypto;
            }

            usuarioCentralCliente.UltimoAcesso = DateTime.Now;
            usuarioCentralCliente.EMAIL = user.Email;
            usuarioCentralCliente.connectionString = usuarioLogado.connectionString;
            usuarioCentralCliente.UtilizaPontoWeb = 1;

            return usuarioCentralCliente;
        }

        #region Validações
        private void ValidarForm(UsuarioPontoWeb obj)
        {
            int idGrupo;
            if (obj.Tipo != 0)
            {
                idGrupo = GrupoUsuarioController.BuscaIdgrupo(obj.GrupoUsuario);

                if (idGrupo == 0)
                    ModelState["GrupoUsuario"].Errors.Add("Grupo Inválido. O campo Grupo é obrigatório para usuários não administradores");
                else
                    obj.IdGrupo = idGrupo;
            }

            if ((obj.Senha.Length < 5) || (obj.Senha.Length > 200))
            {
                ModelState["Senha"].Errors.Add("Senha deve ter entre 5 e 200 caracteres.");
            }

            if (!String.IsNullOrEmpty(obj.CPFUsuario))
            {
                if (obj.CPFUsuario != "___.___.___-__")
                {
                    if (!BLL.cwkFuncoes.ValidarCPF(obj.CPFUsuario))
                    {
                        ModelState["CPFUsuario"].Errors.Add("CPF inválido. Verifique!");
                    }
                }
            }

            if (obj.PermissaoConcluirFluxoPnl == true && String.IsNullOrEmpty(obj.CPFUsuario))
            {
                ModelState["CPFUsuario"].Errors.Add("CPF é obrigatório para concluir fluxo! Por favor, digite um CPF válido.");
            }

            if (obj.Login.Length > 20)
            {
                ModelState["Login"].Errors.Add("O login deve possuir no máximo 20 caracteres.");
            }
        }

        private bool ValidaAlteracao(CentralCliente.Usuario user, out Dictionary<string, string> erro)
        {
            IList<CentralCliente.Usuario> usuarios = new List<CentralCliente.Usuario>();

            erro = new Dictionary<string, string>();
            usuarios = Usuario.GetAll();

            foreach (var item in usuarios.Where(s => s.ID != user.ID).ToList())
            {
                if (item.Login.ToLower() == user.Login.ToLower())
                {
                    erro.Add("Login", "Login já existente");
                    return false;
                }
                if (!String.IsNullOrEmpty(item.EMAIL) && !String.IsNullOrEmpty(user.EMAIL))
                {
                    if (item.EMAIL.ToLower().Equals(user.EMAIL.ToLower()))
                    {
                        erro.Add("Email", "E-mail já existente");
                        return false;
                    }
                }

            }

            return true;
        }

        private bool ValidaInclusao(Modelo.UsuarioPontoWeb user, out CentralCliente.Usuario centralClienteUsuario, out Dictionary<string, string> erro)
        {
            erro = new Dictionary<string, string>();
            centralClienteUsuario = new CentralCliente.Usuario();

            centralClienteUsuario = ValidaUsuarioPorLogin(user);
            if ((centralClienteUsuario != null) &&
                (centralClienteUsuario.ID != 0))
            {
                erro.Add("Login", "Login já existente");
                return false;
            }
            else
            {
                centralClienteUsuario = new CentralCliente.Usuario();
            }

            if (!String.IsNullOrEmpty(user.Email))
            {
                centralClienteUsuario = ValidaUsuarioPorEmail(user);
                if ((centralClienteUsuario != null) &&
                   (centralClienteUsuario.ID != 0))
                {
                    erro.Add("Email", "E-mail já existente");
                    return false;
                }
                else
                {
                    centralClienteUsuario = new CentralCliente.Usuario();
                }
            }
            else
            {
                user.Email = String.Empty;
            }

            return true;
        }

        private CentralCliente.Usuario ValidaUsuarioPorLogin(UsuarioPontoWeb user)
        {
            user.Login = user.Login == null ? "" : user.Login;
            return BLLWeb.Usuario.ValidaUsuarioPorLogin(user.Login);
        }

        private CentralCliente.Usuario ValidaUsuarioPorEmail(UsuarioPontoWeb user)
        {
            user.Email = user.Email == null ? "" : user.Email;
            return BLLWeb.Usuario.ValidaUsuarioPorEmail(user.Email);
        }
        #endregion

        #region RegistrarAntigo
        //public ActionResult Registrar(Models.cw_usuario user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (var db = new cworkpontoEntities())
        //        {
        //            ICryptoService crypto = new PBKDF2();
        //            string salt = crypto.GenerateSalt();
        //            string passCrypto = crypto.Compute(user.Password);
        //            cw_usuario usuario = db.cw_usuario.Create();

        //            usuario.nome = user.nome;
        //            usuario.login = user.login;
        //            usuario.Password = passCrypto;
        //            usuario.PasswordSalt = salt;

        //            db.cw_usuario.Add(usuario);
        //            try
        //            {
        //                db.SaveChanges();
        //            }
        //            catch (Exception ex)
        //            {
        //                string erro = "";
        //                if (ex.InnerException.ToString().Contains("Uq_cw_usuario_Login"))
        //                {
        //                    erro = "Já existe o usuário " + user.login + " cadastrado no sistema!";
        //                }
        //                else
        //                {
        //                    erro = ex.InnerException.ToString();
        //                }
        //                ModelState.AddModelError("", erro);
        //                ViewBag.resultError = erro;
        //                return View();
        //            }
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    return View();
        //} 
        #endregion

        #region Log In/Out
        [HttpGet]
        public ActionResult LogIn(string returnUrl)
        {
            TentativasLogin tl = Usuario.GetTentativaLogin("");
            if (tl == null)
            {
                tl = new TentativasLogin();
            }

            if (tl.Tentativas >= 2)
            {
                ViewBag.Validar = 1;
            }

           Models.UsuarioLogin user = new Models.UsuarioLogin();
            user.ReturnURL = returnUrl;
            user.Cpt = "0";
            //Se o projeto estiver em Debug faz o login automático
#if DEBUG
             String con = System.Configuration.ConfigurationManager.ConnectionStrings["ConnCentralCliente"].ConnectionString;
            if (con.ToUpper().Contains(@"\PRD"))
            {
                user.login = "comercial";
                user.Password = "comercialpfp";
            } else if (con.ToUpper().Contains(@"\hom"))
            {
                user.login = "homjmalucelli";
                user.Password = "pfphom";
            }
            else if (con.ToUpper().Contains(@"\DEV"))
            {
                user.login = "devtibrasil";
                user.Password = "pfpdev";
            }
            else if (con.ToUpper().Contains(@"Data Source=localhost"))
            {
                user.login = "localemployer";
                user.Password = "pfphom";
            }
            else
            {
                return View(user);
            }

                string retorno = "";
            if (Usuario.ValidaUsuario(user.login, user.Password, ref retorno))
            {
                tl.Tentativas = 0;
                tl.UltimaTentativa = DateTime.Now;
                Usuario.AdicionaTentativasLogin(tl);
                return RealizaLogin(user);
            }
            else
            {
                return View(user);
            }
#else
                return View();
#endif
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogIn(Models.UsuarioLogin user, FormCollection formulario)
        {
            bool captchaValido = true;
            TentativasLogin tl = Usuario.GetTentativaLogin("");
            if (tl == null)
            {
                tl = new TentativasLogin();
                tl.Usuario = "";
            }
            if (ModelState.IsValid)
            {
                string retorno = "";
                if (tl.Tentativas > 2)
                {
                    if (!CaptchaHelper.Validar(formulario))
                    {
                        ViewBag.resultCaptcha = "Captcha inválido";
                        captchaValido = false;
                    }
                }

                if (captchaValido)
                {
                    if (Usuario.ValidaUsuario(user.login, user.Password, ref retorno))
                    {
                        tl.Tentativas = 0;
                        tl.UltimaTentativa = DateTime.Now;
                        Usuario.AdicionaTentativasLogin(tl);
                        return RealizaLogin(user);
                    }
                    ModelState["Password"].Errors.Add(retorno);

                    tl.Tentativas++;
                    tl.UltimaTentativa = DateTime.Now;
                    Usuario.AdicionaTentativasLogin(tl);
                }
            }
            if (tl.Tentativas >= 2)
            {
                ViewBag.Validar = 1;
            }
            return View(user);
        }

        public ActionResult Captcha()
        {
            return View();
        }

        private ActionResult RealizaLogin(Models.UsuarioLogin user)
        {
            FormsAuthentication.SetAuthCookie(user.login, false);
            if (!String.IsNullOrEmpty(user.ReturnURL) && user.ReturnURL.Replace("/", "").Length > 0)
            {
                return Redirect(user.ReturnURL);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            Usuario.LimpaCacheUsuario();
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn", "Usuario");
        }

        public ActionResult Negado()
        {
            return View();
        }
        #endregion

        [Authorize]
        public ActionResult EventoConsulta(String consulta, String filtro)
        {
            BLL.UsuarioPontoWeb bllUsuarioPontoWeb = new BLL.UsuarioPontoWeb(Usuario.GetUsuarioLogadoCache().ConnectionStringDecrypt);

            IList<UsuarioPontoWeb> lUser = new List<UsuarioPontoWeb>();
            int codigo = -1;
            try { codigo = Int32.Parse(consulta); }
            catch (Exception) { codigo = -1; }
            if (codigo != -1)
            {
                UsuarioPontoWeb func = bllUsuarioPontoWeb.LoadObjectByCodigo(codigo);
                if (func != null && func.Id > 0)
                {
                    lUser.Add(func);
                }
            }

            if (lUser.Count == 0)
            {
                if (!String.IsNullOrEmpty(consulta))
                {
                    lUser = bllUsuarioPontoWeb.GetAllListWeb().Where(w => w.UtilizaControleSupervisor).Where(w =>
                        w.Login.ToLower().Contains(consulta.ToLower()) ||
                        w.Nome.ToLower().Contains(consulta.ToLower())
                    ).ToList();
                }
                else
                {
                    lUser = bllUsuarioPontoWeb.GetAllListWeb().Where(w => w.UtilizaControleSupervisor).ToList();
                }
            }
            ViewBag.Title = "Pesquisar Usuários";
            return View(lUser);
        }
    }
}