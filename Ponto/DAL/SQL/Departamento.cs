using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace DAL.SQL
{
    public class Departamento : DAL.SQL.DALBase, DAL.IDepartamento
    {
        public string SELECTPEM
        {
            get
            {
                string sql = @"   SELECT   departamento.*
                                    , convert(varchar,emp.codigo)+' | '+emp.nome as empresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
                             FROM departamento 
                             INNER JOIN empresa emp ON emp.id = departamento.idempresa 
                             left JOIN horario h ON departamento.idhorariopadraofunc = h.id"
                    + GetWhereSelectAll();
                sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "departamento.idempresa", null);
                return sql;
            }
        }

        protected override string SELECTALL
        {
            get
            {
                string sql = @"   SELECT   departamento.id
                                    , departamento.descricao
                                    , departamento.codigo
                                    , convert(varchar,emp.codigo)+' | '+emp.nome as empresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
                                    , idIntegracao
                             FROM departamento
                             LEFT JOIN empresa emp ON emp.id = departamento.idempresa
                             left JOIN horario h ON departamento.idhorariopadraofunc = h.id"
                    + GetWhereSelectAll();

                sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "departamento.idempresa", null);
                return sql;
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        public Departamento(DataBase database)
        {
            db = database;
            TABELA = "departamento";

            SELECTPID = @"SELECT departamento.*, 
                                  convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
                             FROM departamento
                             LEFT JOIN empresa ON empresa.id = departamento.idempresa 
                             left JOIN horario h ON departamento.idhorariopadraofunc = h.id
                            WHERE departamento.id = @id";

            INSERT = @"  INSERT INTO departamento
							(codigo, descricao, idempresa, incdata, inchora, incusuario, PercentualMaximoHorasExtras, idIntegracao, IdHorarioPadraoFunc, TipoHorarioPadraoFunc)
							VALUES
							(@codigo, @descricao, @idempresa, @incdata, @inchora, @incusuario, @PercentualMaximoHorasExtras, @idIntegracao, @IdHorarioPadraoFunc, @TipoHorarioPadraoFunc) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE departamento SET 
							  codigo = @codigo
							, descricao = @descricao
							, idempresa = @idempresa
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , PercentualMaximoHorasExtras = @PercentualMaximoHorasExtras
                            , idIntegracao = @idIntegracao
                            , IdHorarioPadraoFunc = @IdHorarioPadraoFunc
                            , TipoHorarioPadraoFunc = @TipoHorarioPadraoFunc
						WHERE id = @id";

            DELETE = @"  DELETE FROM departamento WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM departamento";

        }

        #region Metodos

        private string SqlGetAllPorCodigo()
        {
            return @"SELECT departamento.*, 
                                  convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
                             FROM departamento
                             LEFT JOIN empresa ON empresa.id = departamento.idempresa 
                             left JOIN horario h ON departamento.idhorariopadraofunc = h.id
                            WHERE departamento.codigo = @codigo";
        }

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AtribuiDepartamento(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Departamento();
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
        }

        private void AtribuiDepartamento(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Departamento)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Departamento)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Departamento)obj).IdEmpresa = Convert.ToInt32(dr["idempresa"]);
            ((Modelo.Departamento)obj).empresaNome = Convert.ToString(dr["empresa"]);
            object val = dr["PercentualMaximoHorasExtras"];
            Decimal? perc = (val == null || val is DBNull) ? (decimal?)null : (decimal?)val;
            ((Modelo.Departamento)obj).PercentualMaximoHorasExtras = perc;
            object idIntegracao = dr["idIntegracao"];
            object IdHorarioPadraoFunc = dr["IdHorarioPadraoFunc"];
            ((Modelo.Departamento)obj).idIntegracao = (idIntegracao == null || idIntegracao is DBNull) ? (int?)null : (int?)idIntegracao;
            ((Modelo.Departamento)obj).IdHorarioPadraoFunc = dr["IdHorarioPadraoFunc"] is DBNull ? 0 : Convert.ToInt32(dr["IdHorarioPadraoFunc"]);
            ((Modelo.Departamento)obj).TipoHorarioPadraoFunc = dr["TipoHorarioPadraoFunc"] is DBNull ? 0 : Convert.ToInt32(dr["TipoHorarioPadraoFunc"]);
            ((Modelo.Departamento)obj).Horario = Convert.ToString(dr["NomeHorario"]);
            
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
				new SqlParameter ("@idempresa", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@PercentualMaximoHorasExtras", SqlDbType.Decimal),
                new SqlParameter ("@idIntegracao", SqlDbType.Int),
                new SqlParameter ("@IdHorarioPadraoFunc", SqlDbType.Int),
                new SqlParameter ("@TipoHorarioPadraoFunc", SqlDbType.Int)
			};
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.Departamento)obj).Codigo;
            parms[2].Value = ((Modelo.Departamento)obj).Descricao;
            parms[3].Value = ((Modelo.Departamento)obj).IdEmpresa;
            parms[4].Value = ((Modelo.Departamento)obj).Incdata;
            parms[5].Value = ((Modelo.Departamento)obj).Inchora;
            parms[6].Value = ((Modelo.Departamento)obj).Incusuario;
            parms[7].Value = ((Modelo.Departamento)obj).Altdata;
            parms[8].Value = ((Modelo.Departamento)obj).Althora;
            parms[9].Value = ((Modelo.Departamento)obj).Altusuario;
            parms[10].Value = ((Modelo.Departamento)obj).PercentualMaximoHorasExtras;
            parms[11].Value = ((Modelo.Departamento)obj).idIntegracao;
            parms[12].Value = ((Modelo.Departamento)obj).IdHorarioPadraoFunc;
            parms[13].Value = ((Modelo.Departamento)obj).TipoHorarioPadraoFunc;
        }

        public Modelo.Departamento LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = id;
            string sql = SELECTPID + PermissaoUsuarioEmpresa(UsuarioLogado, SELECTPID, "departamento.idempresa", null); 
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Modelo.Departamento objDepartamento = new Modelo.Departamento();
            try
            {

                SetInstance(dr, objDepartamento);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objDepartamento;
        }

        public Modelo.Departamento LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SqlGetAllPorCodigo();
            aux += PermissaoUsuarioEmpresa(UsuarioLogado, aux, "departamento.idempresa", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Departamento objDep = new Modelo.Departamento();
            SetInstance(dr, objDep);
            return objDep;
        }

        public List<Modelo.Departamento> LoadPEmpresa(int IdEmpresa)
        {
            SqlDataReader dr = GetPorEmpresaAux(IdEmpresa);

            List<Modelo.Departamento> lista = new List<Modelo.Departamento>();
            Modelo.Departamento objDepartamento;
            while (dr.Read())
            {
                objDepartamento = new Modelo.Departamento();

                AtribuiDepartamento(dr, objDepartamento);

                objDepartamento.Acao = Modelo.Acao.Consultar;

                lista.Add(objDepartamento);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        private SqlDataReader GetPorEmpresaAux(int IdEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idempresa", SqlDbType.Int, 4) };
            parms[0].Value = IdEmpresa;

            return db.ExecuteReader(CommandType.Text, SELECTPEM + " AND idempresa = @idempresa", parms);
        }

        public DataTable GetPorEmpresa(int IdEmpresa)
        {
            DataTable dt = new DataTable();
            SqlDataReader dr = GetPorEmpresaAux(IdEmpresa);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public DataTable GetPorEmpresa(string empresas)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPEM + " AND idempresa IN " + empresas, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public List<Modelo.Departamento> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string sql = @"SELECT departamento.*, 
                               convert(varchar,emp.codigo)+' | '+emp.nome as empresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
                             FROM departamento 
                             INNER JOIN empresa emp ON emp.id = departamento.idempresa 
                             left JOIN horario h ON departamento.idhorariopadraofunc = h.id ";
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "departamento.idempresa", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Departamento> lista = new List<Modelo.Departamento>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Departamento objDepartamento = new Modelo.Departamento();
                    AtribuiDepartamento(dr, objDepartamento);
                    lista.Add(objDepartamento);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.Departamento> GetAllListLike(string desc)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@desc", SqlDbType.VarChar)
            };
            parms[0].Value = desc;

            string sql = @"SELECT departamento.*, 
                               convert(varchar,emp.codigo)+' | '+emp.nome as empresa, CONVERT(varchar, h.codigo) + ' | ' + h.descricao as NomeHorario
                             FROM departamento 
                             INNER JOIN empresa emp ON emp.id = departamento.idempresa 
                             left JOIN horario h ON departamento.idhorariopadraofunc = h.id 
                            WHERE departamento.descricao like '%'+@desc+'%'";
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "departamento.idempresa", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Departamento> lista = new List<Modelo.Departamento>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Departamento objDepartamento = new Modelo.Departamento();
                    AtribuiDepartamento(dr, objDepartamento);
                    lista.Add(objDepartamento);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.Departamento> GetAllListByIds(List<int> ids)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", ids);

            string sql = @"SELECT * FROM Departamento WHERE id IN (SELECT * FROM dbo.F_ClausulaIn(@ids))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Departamento> lista = new List<Modelo.Departamento>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Departamento>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Departamento>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        private string GetWhereSelectAll()
        {
            Empresa dalEmpresa = new Empresa(db);
            dalEmpresa.UsuarioLogado = UsuarioLogado;
            if (dalEmpresa.FazerRestricaoUsuarios())
            {
                return " WHERE departamento.id > 0 AND (SELECT COUNT(id) FROM empresacwusuario WHERE empresacwusuario.idcw_usuario = "
                    + UsuarioLogado.Id.ToString() + " AND empresacwusuario.idempresa = emp.id) > 0 ";
            }
            return "";
        }

        public int? GetIdPorDesc(String Descricao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string sql = "select top 1 id from departamento where descricao = " + "'" + Descricao + "'";
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "departamento.idempresa", null);
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql, parms));

            return Id;
        }

        public int? GetIdPorCodigo(int Cod)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string sql = "select top 1 id from departamento where codigo = " + Cod;
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "departamento.idempresa", null);
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql, parms));

            return Id;
        }

        public List<int> GetIdsPorCodigos(List<int> codigos)
        {
            SqlDataReader dr = null;
            try
            {
                SqlParameter[] parms = new SqlParameter[]
                {
                new SqlParameter("@ids", SqlDbType.VarChar)
                };
                parms[0].Value = String.Join(",", codigos);

                string sql = @"SELECT id FROM Departamento WHERE codigo IN (SELECT * FROM dbo.F_ClausulaIn(@ids))";

                dr = db.ExecuteReader(CommandType.Text, sql, parms);
                DataTable dt = new DataTable();
                if (dr.HasRows)
                {

                    dt.Load(dr);
                    dt.AsEnumerable().Where(r => !dt.AsEnumerable().Select(s => s.ItemArray[0]).Contains(r.ItemArray[0])).ToList().ForEach(r => dt.ImportRow(r));
                }

                if (dt != null && dt.Rows.Count == 0)
                    return new List<int>();
                else
                    return dt.AsEnumerable().Select(x => Convert.ToInt32(x[0])).ToList();
            }
            finally
            {
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
        }

        public int? GetIdPorIdIntegracao(int idIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string sql = "select top 1 id from departamento where idIntegracao = " + idIntegracao;
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "departamento.idempresa", null);
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql, parms));

            return Id;
        }

        public bool PossuiFuncionarios(int id)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("id", id)
                };
            var qtd = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select count(*) from funcionario where iddepartamento = @id", parms));
            return qtd > 0;
        }

        public Modelo.Departamento GetDepartamentoPadrao(string cnpj)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@cnpj", SqlDbType.VarChar)
            };

            parms[0].Value = cnpj;

            string sql = @"select top(1) d.* 
                              from departamento d
                             inner join empresa e on d.idempresa = e.id
                             where CONVERT(bigint,REPLACE(REPLACE(REPLACE(e.cnpj,'.',''),'-',''),'/','')) = CONVERT(bigint,REPLACE(REPLACE(REPLACE(@cnpj,'.',''),'-',''),'/',''))
                             order by idIntegracao, codigo";
            sql += PermissaoUsuarioEmpresa(UsuarioLogado, sql, "departamento.idempresa", null);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                if (dr.HasRows && dr.Read())
                {
                    var map = AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Departamento>();
                    return AutoMapper.Mapper.Map<Modelo.Departamento>(dr);
                }
                else
                {
                    return new Modelo.Departamento();
                }
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
        }

        #endregion
    }
}
