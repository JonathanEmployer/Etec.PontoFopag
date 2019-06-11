using cwkWebAPIPontoWeb.Models;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CentralCliente;

namespace cwkWebAPIPontoWeb.Controllers.BLLAPI
{
    public class UsuarioBLL
    {
        public static DadosConexao GetConexaoUsuario(String login)
        {
            DadosConexao Conexao = new DadosConexao();

            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            CentralCliente.Usuario usuario = new CentralCliente.Usuario();

            AspNetUsers objAspNetUsers = db.AspNetUsers.Where(a => a.UserName.Equals(login)).FirstOrDefault();
            if (objAspNetUsers != null)
            {
                usuario = objAspNetUsers.Usuario;

                if (usuario == null || usuario.Cliente == null)
                {
                    Conexao.Conexao = "";
                    Conexao.Sucesso = false;
                    Conexao.Erro = new RetornoErro();
                    Conexao.Erro.erroGeral = "Funcionário Não Encontrado";
                    if ((usuario != null) && (usuario.Cliente == null))
                    {
                        Conexao.Erro.erroGeral = "Você não esta vinculado a uma empresa, entre em contrato com o setor de RH";
                    }
                }
                else
                {
                    Conexao.Sucesso = true;
                    Conexao.Conexao = usuario.Cliente.CaminhoBD;
                }
            }

            return Conexao;
        }
    }
}