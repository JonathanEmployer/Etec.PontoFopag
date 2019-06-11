using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Modelo.Acesso;
using System.Threading.Tasks;

namespace GerenciadorWeb.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {

        #region Owin
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return Owin.Owin.SignInManager;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return Owin.Owin.UserManager;
            }
        }

        #endregion

        #region Usuário
        public ActionResult Index(string id)
        {
            return View(ListarUsuarios());
        }

        protected IQueryable<Usuario> ListarUsuarios()
        {
            return UserManager.Users.Select(user => new Usuario { ID = user.Id, NomeUsuario = user.UserName });
        }
        #endregion

        #region Deletar Usuario
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    Models.ApplicationUser user = UserManager.FindById(id);
                    if (user != null)
                    {
                        if (user.UserName.Equals(User.Identity.Name))
                        {
                            ModelState.AddModelError("", "Não é possível excluir o próprio usuário.");
                        }
                        else
                        {
                            UserManager.Delete(user);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException.ToString().IndexOf("FK_HISBLO_ASPUSR") != -1)
                        ModelState.AddModelError(string.Empty, "Não é possível deletar. O usuário está ligado a bloqueio de estação.");
                    else
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao deletar o Usuário, por favor, tente novamente");
                }
            }

            return View("Index", ListarUsuarios());
        }
        #endregion

        #region Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Inicial");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(Login model, string redirectUrl)
        {
            Validar(model);
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SignInStatus result = await SignInManager.PasswordSignInAsync(model.Usuario, model.Senha, model.Persistente, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if (model.Usuario.ToUpper() == "ADVANT")
                    {
                        ModelState.AddModelError("", "Usuário de integração não tem permissão para acessar a aplicação.");
                        return View(model);
                    }
                    return RedirectToLocal(redirectUrl);
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "Usuário bloqueado.");
                    return View(model);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Login e/ou senha inválido(s).");
                    return View(model);
            }
        }

        private void Validar(Login model)
        {
            if (string.IsNullOrEmpty(model.Usuario))
            {
                ModelState.AddModelError("NomeUsuario", "O nome do usuário é obrigatório.");
            }
            if (string.IsNullOrEmpty(model.Senha))
            {
                ModelState.AddModelError("Senha", "A senha é obrigatória.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            SignInManager.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Usuario");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Inicial");
        }

        #endregion

        #region Cadastro
        public ActionResult Novo()
        {
            return View("Cadastro");
        }

        public ActionResult Cadastro(string id)
        {
            if (string.IsNullOrEmpty(id))
                return View(new Usuario());

            Models.ApplicationUser user = UserManager.FindById(id);
            Usuario usuario = new Usuario();
            if (user != null)
            {
                usuario.ID = id;
                usuario.NomeUsuario = user.UserName;
            }
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Cadastro(Usuario model)
        {
            if (model == null)
                return View(new Usuario());
            if (Validar(model))
            {
                string email = Guid.NewGuid().ToString().Replace("-", "") + "@pontofopag.com.br";
                Models.ApplicationUser user = new Models.ApplicationUser { UserName = model.NomeUsuario, Email = email };
                if (string.IsNullOrEmpty(model.ID))
                {
                    IdentityResult result = UserManager.Create(user, model.Senha);
                    if (result.Succeeded)
                    {
                        model.ID = user.Id;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        if (result.Errors.FirstOrDefault().Contains("Passwords must be at least 6 characters"))
                        {
                            ModelState.AddModelError("", "Falha ao criar o usuário. Senha deve conter no mínimo 6 carácteres.");
                        }
                        else if (result.Errors.FirstOrDefault().Contains("Passwords must have at least one non letter or digit character"))
                        {
                            ModelState.AddModelError("", "Falha ao criar o usuário. Senha deve possuir pelo menos 1 carácter que não seja número ou letra.");
                        }
                        else if (result.Errors.FirstOrDefault().Contains("Passwords must have at least one lowercase ('a'-'z')"))
                        {
                            ModelState.AddModelError("", "Falha ao criar o usuário. Senha deve possuir pelo menos 1 letra minúscula.");
                        }
                        else if (result.Errors.FirstOrDefault().Contains("Passwords must have at least one uppercase ('A'-'Z')"))
                        {
                            ModelState.AddModelError("", "Falha ao criar o usuário. Senha deve possuir pelo menos 1 carácter maiúsculo.");
                        }
                        else if (result.Errors.FirstOrDefault().Contains("Passwords must have at least one digit ('0'-'9')"))
                        {
                            ModelState.AddModelError("", "Falha ao criar o usuário. Senha deve possuir pelo menos 1 digito númerico");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Falha ao criar o usuário: " + string.Join("\n", result.Errors.ToArray()));
                        }
                    }
                }
            }
            return View(model);
        }

        private bool Validar(Usuario model)
        {
            bool valido = true;
            if (string.IsNullOrEmpty(model.NomeUsuario))
            {
                ModelState.AddModelError("NomeUsuario", "O nome do usuário é obrigatório.");
                valido = false;
            }
            if (string.IsNullOrEmpty(model.Senha))
            {
                ModelState.AddModelError("Senha", "A senha é obrigatória.");
                valido = false;
            }
            if (valido && !model.Senha.Equals(model.ConfirmarSenha))
            {
                ModelState.AddModelError("ConfirmarSenha", "As senhas não são iguais.");
                valido = false;
            }
            if (valido)
            {
                Models.ApplicationUser atual = UserManager.FindByName(model.NomeUsuario);
                if (atual != null && !atual.Id.Equals(model.ID))
                {
                    ModelState.AddModelError("NomeUsuario", "Já existe outro usuário cadastrado com esse nome.");
                    valido = false;
                }
            }
            return valido;
        }

        #endregion

        #region Trocar senha
        public ActionResult TrocarSenha(string id)
        {
            return View(new Usuario { ID = id });
        }
        #endregion

    }
}