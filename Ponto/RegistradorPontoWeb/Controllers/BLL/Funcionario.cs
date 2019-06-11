using RegistradorPontoWeb.Models;
using ModeloPonto = RegistradorPontoWeb.Models.Ponto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using RegistradorPontoWeb.Models.Ponto;
using CentralCliente;

namespace RegistradorPontoWeb.Controllers.BLL
{
    public class Funcionario
    {
        public void GetFuncionarioCC(RegistroPontoMetaData registro, ref RetornoErro erros)
        {
            Funcionario bllFuncionario = new Funcionario();
            string CPF = registro.UserName.Replace("-", "").Replace(".", "").Replace("/", "");
            using (var db = new CENTRALCLIENTEEntities())
            {
                registro.Funcionarios = db.Funcionarios.Where(w => w.Login.Replace("-", "").Replace(".", "").Replace("/", "") == CPF).FirstOrDefault();
                if (registro.Funcionarios == null)
                {
                    erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Funcionário Não Encontrado" });
                }
                else if ((registro.Funcionarios.Cliente == null))
                {
                    erros.ErrosDetalhados.Add(new ErroDetalhe() { campo = "Username", erro = "Você não esta vinculado a uma empresa, entre em contrato com o setor de RH" });
                }
            }
        }

        public ModeloPonto.funcionario GetFuncionarioPontoPorCPF(string cpf, SqlConnectionStringBuilder conn)
        {
            ModeloPonto.funcionario func = new ModeloPonto.funcionario();
            using (var db = new ModeloPonto.PontofopagEntities(conn.DataSource, conn.InitialCatalog, conn.UserID, conn.Password))
            {
                func = db.funcionario.SqlQuery(@"SELECT f.*
                              FROM dbo.funcionario f
                             INNER JOIN dbo.empresa e ON f.idempresa = e.id
                             WHERE convert(BIGINT,isnull(replace(replace(f.CPF,'.',''),'-',''),0)) = convert(BIGINT,isnull((replace(replace(@CPF,'.',''),'-','')),0))
                             ORDER BY funcionarioativo DESC", new SqlParameter("@CPF", cpf)).FirstOrDefault();
                func.empresa = func.empresa;
                func.empresa.IP = func.empresa.IP;
            }
            return func;
        } 
    }
}