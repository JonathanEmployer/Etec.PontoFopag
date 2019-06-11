
using cwkWebAPIPontoWeb.Models;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using CentralCliente;

namespace cwkWebAPIPontoWeb.Controllers.BLLAPI
{
    public class Empresa
    {
        public static DadosConexao BuscaDadosConexaoDocumento(Int64 documento)
        {
            DadosConexao Conexao = new DadosConexao();
            using (var db = new CENTRALCLIENTEEntities())
            {
                Entidade entidade = db.Entidade.Where(w => w.CNPJ_CPF.Replace("-", "").Replace(".", "").Replace("/", "") == documento.ToString()).FirstOrDefault();
                if (entidade == null || entidade.Cliente == null)
                {
                    Conexao.Conexao = "";
                    Conexao.Sucesso = false;
                    Conexao.Erro = new RetornoErro();
                    Conexao.Erro.erroGeral = "Empresa não encontrada";
                    if ((entidade != null) && (entidade.Cliente == null))
                    {
                        Conexao.Erro.erroGeral = "Empresa encontrada, mas não esta cadastrada como cliente.";
                    }
                }
                else
                {
                    Conexao.Sucesso = true;
                    Conexao.Conexao = entidade.Cliente.FirstOrDefault().CaminhoBD;
                }
            }

            return Conexao;
        }

  
    }
}