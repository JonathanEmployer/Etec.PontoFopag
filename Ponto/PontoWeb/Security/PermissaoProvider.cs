using PontoWeb.Controllers.BLLWeb;
using PontoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PontoWeb.Security
{
    public class PermissaoProvider: System.Web.Security.RoleProvider
    {
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            List<string> permissoes = Usuario.GetAcessoCache();
            if (permissoes == null || permissoes.Count() == 0)
            {
                cw_usuario user = Controllers.BLLWeb.Usuario.GetUsuarioLogadoCache();
                Controllers.BLLWeb.Usuario.LimpaAcessoCache();
                Controllers.BLLWeb.Usuario.AdicionaAcessoCache(user);
                permissoes = Usuario.GetAcessoCache();
            }
            return permissoes.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}