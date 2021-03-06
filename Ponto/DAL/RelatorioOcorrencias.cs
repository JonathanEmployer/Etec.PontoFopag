using AutoMapper;
using DAL.SQL;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL
{
    public class RelatorioOcorrencias
    {
        private DataBase db;
        public RelatorioOcorrencias(DataBase database)
        {
            db = database;
        }
        public string GetAllEmpresas()
        {
            string sql = "select e.id, e.codigo, CONVERT(varchar, e.codigo) + ' | ' + e.nome as nome from empresa e";
            return sql;
        }

        public string GetAllDepartamentos()
        {
            string sql = @"select d.id, d.codigo, d.descricao, d.idempresa, 
                            CONVERT(varchar, e.codigo) + ' | ' + e.nome as NomeEmpresa from departamento d
                            join empresa e on d.idempresa = e.id;";
            return sql;
        }

        public string GetAllFuncionarios()
        {
            string sql = @"select f.id, f.codigo, f.nome, CONVERT(VARCHAR, d.codigo) + ' | ' + d.descricao as departamento, d.id as IdDepartamento, d.IdEmpresa as IdEmpresa
                           from funcionario f
                           join departamento d on f.iddepartamento = d.id
                           where f.excluido = 0 and f.funcionarioativo = 1";
            return sql;
        }

        public string GetAllOcorrencias(Modelo.Cw_Usuario usuarioLogado)
        {
            Ocorrencia dalOcorrencia = new Ocorrencia(db);
            dalOcorrencia.UsuarioLogado = usuarioLogado;
            string sql = "select o.id, o.codigo, o.descricao from ocorrencia o where 1 = 1 "+ dalOcorrencia.AddPermissaoUsuario("o.id");
            return sql;
        }

        public string GetAllJustificativas(Modelo.Cw_Usuario usuarioLogado)
        {
            Justificativa dalJustificativa = new Justificativa(db);
            dalJustificativa.UsuarioLogado = usuarioLogado;
            string sql = "select o.id, o.codigo, o.descricao from justificativa o  where 1 = 1 " + dalJustificativa.AddPermissaoUsuario("o.id");
            return sql;
        }

        public pxyRelOcorrencias GetRelOcorrencias(pxyRelPontoWeb pxyRelPW, Modelo.Cw_Usuario UsuarioLogado)
        {
            pxyRelOcorrencias res = pxyRelOcorrencias.Produce(pxyRelPW);
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader drOcors = db.ExecuteReader(CommandType.Text, GetAllOcorrencias(UsuarioLogado), parms);
            SqlDataReader drJust = db.ExecuteReader(CommandType.Text, GetAllJustificativas(UsuarioLogado), parms);

            try
            {
                var mapOco = Mapper.CreateMap<IDataReader, pxyOcorrencias>();
                res.Ocorrencias = Mapper.Map<List<pxyOcorrencias>>(drOcors);
                var mapJus = Mapper.CreateMap<IDataReader, pxyJustificativas>();
                res.Justificativas = Mapper.Map<List<pxyJustificativas>>(drJust);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!drOcors.IsClosed)
                {
                    drOcors.Close();
                }
                drOcors.Dispose();
                if (!drJust.IsClosed)
                {
                    drJust.Close();
                }
                drJust.Dispose();
            }
            return res;
        }

        public pxyRelPontoWeb GetRelBase()
        {
            pxyRelPontoWeb res = new pxyRelPontoWeb();
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader drEmp = db.ExecuteReader(CommandType.Text, GetAllEmpresas(), parms);
            SqlDataReader drDep = db.ExecuteReader(CommandType.Text, GetAllDepartamentos(), parms);
            SqlDataReader drFun = db.ExecuteReader(CommandType.Text, GetAllFuncionarios(), parms);
            

            var mapEmp = Mapper.CreateMap<IDataReader, pxyEmpresa>();
            var mapDep = Mapper.CreateMap<IDataReader, pxyDepartamento>();
            var mapFun = Mapper.CreateMap<IDataReader, pxyFuncionario>();
           

            try
            {
                res.Empresas = Mapper.Map<List<pxyEmpresa>>(drEmp);
                res.Departamentos = Mapper.Map<List<pxyDepartamento>>(drDep);
                res.Funcionarios = Mapper.Map<List<pxyFuncionario>>(drFun);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (!drEmp.IsClosed)
                {
                    drEmp.Close();
                }
                if (!drDep.IsClosed)
                {
                    drDep.Close();
                }
                if (!drFun.IsClosed)
                {
                    drFun.Close();
                }
                drEmp.Dispose();
                drDep.Dispose();
                drFun.Dispose();
            }
            return res;
        }
    }
}
