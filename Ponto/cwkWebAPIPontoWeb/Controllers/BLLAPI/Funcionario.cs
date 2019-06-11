using cwkWebAPIPontoWeb.Models;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CentralCliente;

namespace cwkWebAPIPontoWeb.Controllers.BLLAPI
{
    public class Funcionario
    {
        public static DadosConexao BuscaDadosConexao(string CPF)
        {
            CPF = CPF.Replace("-", "").Replace(".", "").Replace("/", "");
            DadosConexao Conexao = new DadosConexao();
            using (var db = new CENTRALCLIENTEEntities())
            {
                Funcionarios funcionarios = db.Funcionarios.Where(w => w.Login.Replace("-", "").Replace(".", "").Replace("/", "") == CPF).FirstOrDefault();
                if (funcionarios == null || funcionarios.Cliente == null)
                {
                    Conexao.Conexao = "";
                    Conexao.Sucesso = false;
                    Conexao.Erro = new RetornoErro();
                    Conexao.Erro.erroGeral = "Funcionário Não Encontrado";
                    if ((funcionarios != null) && (funcionarios.Cliente == null))
                    {
                        Conexao.Erro.erroGeral = "Você não esta vinculado a uma empresa, entre em contrato com o setor de RH";
                    }
                }
                else
                {
                    Conexao.Sucesso = true;
                    Conexao.Conexao = funcionarios.Cliente.CaminhoBD;
                }
            }

            return Conexao;
        }

        public static Modelo.Funcionario BuscaFuncionario(string CPF, string Senha, string StrConexao)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(StrConexao);
            Modelo.Funcionario func = bllFuncionario.LoadAtivoPorCPF(CPF);
            func.Conexao = BLL.CriptoString.Encrypt(StrConexao);
            return func;
        }

        public static Modelo.Funcionario FuncionarioComUltimosFechamentos(int IdFuncionario, string StrConexao)
        {
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(StrConexao);
            Modelo.Funcionario func = bllFuncionario.GetAllListComUltimosFechamentos(true, new List<int>() { IdFuncionario }).FirstOrDefault();
            func.Conexao = BLL.CriptoString.Encrypt(StrConexao);
            return func;
        }
    }
}