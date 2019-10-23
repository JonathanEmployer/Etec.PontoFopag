using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Utils;

namespace PontoWeb.Models
{
    public class ConexaoPonto
    {
        public ConexaoPonto()
        {
            var user = cwkControleUsuario.Facade.getUsuarioLogado;
            if (user == null)
            {
                //string strConn = ConfigurationManager.ConnectionStrings["PontoWebContext"].ConnectionString;
                cw_usuario usuario = Usuario.GetUsuarioLogadoCache();
                Modelo.Cw_Usuario usuarioControle = new Modelo.Cw_Usuario();
                if (usuario.id > 0)
                {
                    string strConn = BLL.CriptoString.Decrypt(usuario.connectionString);
                    usuarioControle.Codigo = usuario.codigo;
                    usuarioControle.Login = usuario.login;
                    usuarioControle.Senha = usuario.senha;
                    usuarioControle.Nome = usuario.nome;
                    usuarioControle.Tipo = usuario.tipo.HasValue ? usuario.tipo.Value : 0;
                    usuarioControle.IdGrupo = usuario.idgrupo.HasValue ? usuario.idgrupo.Value : 0;
                    usuarioControle.Email = usuario.EMAIL;

                    cwkControleUsuario.Facade.AutenticaPontoWeb(strConn, usuarioControle);
                } 
            }
            else
            {
                cwkControleUsuario.Facade.AutenticaPontoWeb(BLL.CriptoString.Decrypt(Usuario.GetUsuarioLogadoCache().connectionString), user);
            }
        }
    }
}