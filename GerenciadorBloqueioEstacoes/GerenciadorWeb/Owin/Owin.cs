using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GerenciadorWeb.Models;

namespace GerenciadorWeb.Owin
{
    public static class Owin
    {
        public static ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        public static ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public static ApplicationUser CurrentUser
        {
            get
            {
                return UserManager.FindByName(HttpContext.Current.User.Identity.Name);
            }
        }
    }
}