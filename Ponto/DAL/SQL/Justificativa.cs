using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class Justificativa : DAL.SQL.DALBase, DAL.IJustificativa
    {

        public Justificativa(DataBase database)
        {
            db = database;
            TABELA = "justificativa";

            SELECTPID = @"   SELECT * FROM justificativa WHERE id = @id";

            SELECTALL = @"   SELECT   justificativa.id
                                    , justificativa.descricao
                                    , justificativa.codigo
                                    , justificativa.IdIntegracao
                                    , justificativa.ExibePaineldoRH
									, justificativa.Ativo
                             FROM justificativa";

            INSERT = @"  INSERT INTO justificativa
							(codigo, descricao, incdata, inchora, incusuario, idintegracao, ExibePaineldoRH, Ativo)
							VALUES
							(@codigo, @descricao, @incdata, @inchora, @incusuario, @idintegracao, @ExibePaineldoRH, @Ativo) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE justificativa SET
							  codigo = @codigo
							, descricao = @descricao
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , idintegracao = @idintegracao
                            , ExibePaineldoRH = @ExibePaineldoRH
							, justificativa.Ativo = @Ativo
						WHERE id = @id";

            DELETE = @"  DELETE FROM justificativa WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM justificativa";

        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.Justificativa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.Justificativa)obj).Descricao = Convert.ToString(dr["descricao"]);
                    object idIntegracao = dr["IdIntegracao"];
                    ((Modelo.Justificativa)obj).IdIntegracao = (idIntegracao == null || idIntegracao is DBNull) ? (int?)null : (int?)idIntegracao;
                    ((Modelo.Justificativa)obj).ExibePaineldoRH = Convert.ToBoolean(dr["ExibePaineldoRH"]);
					((Modelo.Justificativa)obj).Ativo = Convert.ToBoolean(dr["Ativo"]);
					return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Justificativa();
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
        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Justificativa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Justificativa)obj).Descricao = Convert.ToString(dr["descricao"]);
            object idIntegracao = dr["IdIntegracao"];
            ((Modelo.Justificativa)obj).IdIntegracao = (idIntegracao == null || idIntegracao is DBNull) ? (int?)null : (int?)idIntegracao;
            ((Modelo.Justificativa)obj).ExibePaineldoRH = Convert.ToBoolean(dr["ExibePaineldoRH"]);
			((Modelo.Justificativa)obj).Ativo = Convert.ToBoolean(dr["Ativo"]);
		}

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@IdIntegracao", SqlDbType.Int),
                new SqlParameter ("@ExibePaineldoRH", SqlDbType.Bit),
				new SqlParameter ("@Ativo", SqlDbType.Bit)
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
            parms[1].Value = ((Modelo.Justificativa)obj).Codigo;
            parms[2].Value = ((Modelo.Justificativa)obj).Descricao;
            parms[3].Value = ((Modelo.Justificativa)obj).Incdata;
            parms[4].Value = ((Modelo.Justificativa)obj).Inchora;
            parms[5].Value = ((Modelo.Justificativa)obj).Incusuario;
            parms[6].Value = ((Modelo.Justificativa)obj).Altdata;
            parms[7].Value = ((Modelo.Justificativa)obj).Althora;
            parms[8].Value = ((Modelo.Justificativa)obj).Altusuario;
            parms[9].Value = ((Modelo.Justificativa)obj).IdIntegracao;
            parms[10].Value = ((Modelo.Justificativa)obj).ExibePaineldoRH;
			parms[11].Value = ((Modelo.Justificativa)obj).Ativo;
		}

        public Modelo.Justificativa LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Justificativa objJustificativa = new Modelo.Justificativa();
            try
            {
                SetInstance(dr, objJustificativa);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJustificativa;
        }

        public int? GetIdPorCod(int Cod)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from justificativa where codigo = " + Cod, parms));

            return Id;
        }

        public int GetIdPorIdIntegracao(int IdIntegracao)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from justificativa where IdIntegracao = " + IdIntegracao, parms));
            return Id;
        }

        public Modelo.Justificativa LoadObjectByCodigo(int pCodigo)
        {

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@codigo", SqlDbType.Int)
            };
            parms[0].Value = pCodigo;

            string sql = " SELECT * " +
                            " FROM justificativa" +
                            " WHERE codigo = @codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Modelo.Justificativa objJustificativa = new Modelo.Justificativa();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Justificativa>();
                objJustificativa = AutoMapper.Mapper.Map<List<Modelo.Justificativa>>(dr).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJustificativa;
        }

        public Modelo.Justificativa LoadObjectParaColetor()
        {
            string justificativaColetor = "Desconsiderado pelo Coletor";
            Modelo.Justificativa justificativa = LoadObjectByDescricao(justificativaColetor);
            if (justificativa == null || justificativa.Id == 0)
            {
                Modelo.Justificativa nova = new Modelo.Justificativa();
                nova.Codigo = MaxCodigo();
                nova.Descricao = justificativaColetor;
                nova.ExibePaineldoRH = false;
                Incluir(nova);
                justificativa = LoadObjectByDescricao(justificativaColetor);
            }
            return justificativa;
        }

        public Modelo.Justificativa LoadObjectByDescricao(string descricao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@descricao", SqlDbType.VarChar)
            };
            parms[0].Value = descricao;

            string sql = " SELECT * " +
                            " FROM justificativa" +
                            " WHERE descricao = @descricao";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Modelo.Justificativa objJustificativa = new Modelo.Justificativa();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Justificativa>();
                objJustificativa = AutoMapper.Mapper.Map<List<Modelo.Justificativa>>(dr).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objJustificativa;
        }

        public bool BuscaJustificativa(string pNomeDescricao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@descricao", SqlDbType.VarChar)
            };
            parms[0].Value = pNomeDescricao;

            string cmd = @" SELECT COUNT (id) as quantidade
                            FROM justificativa
                            WHERE descricao = @descricao";

            int valor = (int)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor == 0 ? false : true;

        }

        public List<Modelo.Justificativa> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM justificativa", parms);

            List<Modelo.Justificativa> lista = new List<Modelo.Justificativa>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Justificativa objJustificativa = new Modelo.Justificativa();
                    AuxSetInstance(dr, objJustificativa);
                    lista.Add(objJustificativa);
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

        public List<Modelo.Justificativa> GetAllListConsultaEvento()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM justificativa WHERE ativo = 1", parms);

            List<Modelo.Justificativa> lista = new List<Modelo.Justificativa>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Justificativa objJustificativa = new Modelo.Justificativa();
                    AuxSetInstance(dr, objJustificativa);
                    lista.Add(objJustificativa);
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

        public List<Modelo.Justificativa> GetAllPorExibePaineldoRH()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM justificativa where ExibePaineldoRH = 1 and ativo = 1", parms);

            List<Modelo.Justificativa> lista = new List<Modelo.Justificativa>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Justificativa objJustificativa = new Modelo.Justificativa();
                    AuxSetInstance(dr, objJustificativa);
                    lista.Add(objJustificativa);
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

        public List<Modelo.Justificativa> GetAllListPorIds(List<int> ids)
        {
            List<Modelo.Justificativa> result = new List<Modelo.Justificativa>();

            try
            {
                var parameters = new string[ids.Count];
                List<SqlParameter> parmList = new List<SqlParameter>();
                for (int i = 0; i < ids.Count; i++)
                {
                    parameters[i] = string.Format("@Id{0}", i);
                    parmList.Add(new SqlParameter(parameters[i], ids[i]));
                }

                string sql = string.Format("SELECT * from Justificativa WHERE Id IN ({0})", string.Join(", ", parameters));

                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parmList.ToArray());

                try
                {
                    while (dr.Read())
                    {
                        Modelo.Justificativa objJustificativa = new Modelo.Justificativa();
                        AuxSetInstance(dr, objJustificativa);
                        result.Add(objJustificativa);
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
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
        #endregion
    }
}
