using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace PontoWeb.Utils
{
    public static class CustomizeHelpers
    {
        public static MvcHtmlString Saudacao(this HtmlHelper helper)
        {
            string saudacao = "Bom dia";
            if ((DateTime.Now.Hour >= 12) && (DateTime.Now.Hour < 18))
            {
                saudacao = "Boa tarde";
            }
            else if ((DateTime.Now.Hour >= 18) && (DateTime.Now.Hour < 0)) { saudacao = "Boa noite"; }
            saudacao += ", seja bem vindo";
            cw_usuario usuario = Usuario.GetUsuarioLogadoCache();
            if (usuario != null && !String.IsNullOrEmpty(usuario.nome))
            {
                saudacao += " "+ usuario.nome + "!";
                saudacao += " Último acesso: " + usuario.UltimoAcesso;
            }
            else
            {
                saudacao += "!";
            }

            return new MvcHtmlString(saudacao);
        }

        public static MvcHtmlString LogoEmpresa(this HtmlHelper helper)
        {
            string logo = "";
            cw_usuario usuario = Usuario.GetUsuarioLogadoCache();
            if (usuario != null && !String.IsNullOrEmpty(usuario.nome))
            {
                logo = usuario.LogoEmpresa;
            }

            if (String.IsNullOrEmpty(logo))
            {
                logo = "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";
            }

            string HtmlImg = " <div class=FrameLogoEmpresaIcones> <span class=\"HelperLogoEmpresaIcones\"></span><img class=\"logoEmpresaIcones\" src=\"data:image;base64," + logo + "\"/> </div>";
            return new MvcHtmlString(HtmlImg);
        }

        public static MvcHtmlString TipoServidor(this HtmlHelper helper)
        {
            string tipoServidor = "";

            if (ConfigurationManager.AppSettings["ApiPontofopag"].Contains("hom") || ConfigurationManager.AppSettings["ApiPontofopag"].Contains("localhost"))
            {
                string msg = "Ambiente de Homologação";
                if (ConfigurationManager.AppSettings["ApiPontofopag"].Contains("localhost"))
                {
                    msg = "Ambiente Local";
                }
                
                tipoServidor =  "<div class=\"alert alert-info\" style=\"position:fixed; margin:-130px 50px;\"> "+
                                "    <h1>"+msg+" <span class=\"glyphicon glyphicon-exclamation-sign\"></span></h1> "+
                                "</div>";
            }

            return new MvcHtmlString(tipoServidor);
        }

        public static MvcHtmlString IsDisabled(this MvcHtmlString htmlString, bool disabled)
        {
            string rawstring = htmlString.ToString();
            if (disabled)
            {
                rawstring = rawstring.Insert(rawstring.Length - 2, "disabled=\"disabled\"");
            }
            return new MvcHtmlString(rawstring);
        }

        public static MvcHtmlString IsReadonly(this MvcHtmlString htmlString, bool @readonly)
        {
            string rawstring = htmlString.ToString();
            if (@readonly)
            {
                rawstring = rawstring.Insert(rawstring.Length - 2, "readonly=\"readonly\"");
            }
            return new MvcHtmlString(rawstring);
        }
    }
}