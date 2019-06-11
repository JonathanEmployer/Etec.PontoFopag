using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Text;

namespace DAL.SQL
{
    public class OcorrenciaEmpresa : DAL.SQL.DALBase, DAL.IOcorrenciaEmpresa
    {

        public OcorrenciaEmpresa(DataBase database)
        {
            db = database;
            TABELA = "OcorrenciaEmpresa";

            SELECTPID = @"   SELECT * FROM OcorrenciaEmpresa WHERE id = @id";

            SELECTALL = @"   SELECT   OcorrenciaEmpresa.id
                                    , OcorrenciaEmpresa.codigo
                                    , OcorrenciaEmpresa.idEmpresa
                                    , OcorrenciaEmpresa.idOcorrencia
                             FROM OcorrenciaEmpresa";

            INSERT = @"  INSERT INTO OcorrenciaEmpresa
							(codigo, idEmpresa, idOcorrencia, incdata, inchora, incusuario)
							VALUES
							(@codigo, @idEmpresa, @idOcorrencia, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE OcorrenciaEmpresa SET
							  codigo = @codigo
							, idEmpresa = @idEmpresa
                            , idOcorrencia = @idOcorrencia
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM OcorrenciaEmpresa WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM OcorrenciaEmpresa";

        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AuxSetInstance(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.OcorrenciaEmpresa();
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
            ((Modelo.OcorrenciaEmpresa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.OcorrenciaEmpresa)obj).idEmpresa = Convert.ToInt32(dr["idEmpresa"]);
            ((Modelo.OcorrenciaEmpresa)obj).idOcorrencia = Convert.ToInt32(dr["idOcorrencia"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@idEmpresa", SqlDbType.Int),
                new SqlParameter ("@idOcorrencia", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
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
            parms[1].Value = ((Modelo.OcorrenciaEmpresa)obj).Codigo;
            parms[2].Value = ((Modelo.OcorrenciaEmpresa)obj).idEmpresa;
            parms[3].Value = ((Modelo.OcorrenciaEmpresa)obj).idOcorrencia;
            parms[4].Value = ((Modelo.OcorrenciaEmpresa)obj).Incdata;
            parms[5].Value = ((Modelo.OcorrenciaEmpresa)obj).Inchora;
            parms[6].Value = ((Modelo.OcorrenciaEmpresa)obj).Incusuario;
            parms[7].Value = ((Modelo.OcorrenciaEmpresa)obj).Altdata;
            parms[8].Value = ((Modelo.OcorrenciaEmpresa)obj).Althora;
            parms[9].Value = ((Modelo.OcorrenciaEmpresa)obj).Altusuario;
        }

        public Modelo.OcorrenciaEmpresa LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.OcorrenciaEmpresa objOcorrenciaEmpresa = new Modelo.OcorrenciaEmpresa();
            try
            {
                SetInstance(dr, objOcorrenciaEmpresa);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objOcorrenciaEmpresa;
        }

        public Modelo.OcorrenciaEmpresa LoadObjectByCodigo(int pCodigo)
        {

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@codigo", SqlDbType.Int)
            };
            parms[0].Value = pCodigo;

            string sql = " SELECT * " +
                            " FROM OcorrenciaEmpresa" +
                            " WHERE codigo = @codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Modelo.OcorrenciaEmpresa objOcorrenciaEmpresa = new Modelo.OcorrenciaEmpresa();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.OcorrenciaEmpresa>();
                objOcorrenciaEmpresa = AutoMapper.Mapper.Map<List<Modelo.OcorrenciaEmpresa>>(dr).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objOcorrenciaEmpresa;
        }

        public List<Modelo.OcorrenciaEmpresa> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM OcorrenciaEmpresa", parms);

            List<Modelo.OcorrenciaEmpresa> lista = new List<Modelo.OcorrenciaEmpresa>();
            try
            {
                while (dr.Read())
                {
                    Modelo.OcorrenciaEmpresa objOcorrenciaEmpresa = new Modelo.OcorrenciaEmpresa();
                    AuxSetInstance(dr, objOcorrenciaEmpresa);
                    lista.Add(objOcorrenciaEmpresa);
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

        public List<Modelo.OcorrenciaEmpresa> GetAllPorExibePainelRHPorEmpresa(int idEmpresa)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idEmpresa", SqlDbType.Int)
            };
            parms[0].Value = idEmpresa;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text,
                                          @"SELECT 
                                                (CASE WHEN (SELECT COUNT(ocoEmp.id) 
		                                                   FROM ocorrenciaempresa ocoEmp 
		                                                   WHERE ocoEmp.idEmpresa = @idEmpresa and ocoEmp.idOcorrencia = o.id) > 0
                                                THEN 1
                                                ELSE 0
                                                END) Selecionado, 
                                                o.codigo Codigo, o.descricao Descricao, o.id idOcorrencia
                                                FROM ocorrencia o
                                                WHERE exibepaineldorh = 1", parms);

            List<Modelo.OcorrenciaEmpresa> lista = new List<Modelo.OcorrenciaEmpresa>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.OcorrenciaEmpresa>();
                lista = AutoMapper.Mapper.Map<List<Modelo.OcorrenciaEmpresa>>(dr);
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

        public void DeleteAllByIdEmpresa(int idEmpresa)
        {
            String deleteByEmpresa = @"DELETE FROM ocorrenciaempresa WHERE idEmpresa = @idEmpresa";

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idEmpresa", SqlDbType.Int)
            };
            parms[0].Value = idEmpresa;

            int linhasAfetasdas = db.ExecuteNonQuery(CommandType.Text, deleteByEmpresa, parms);
        }

        public void IncluirOcorrenciasEmpresa(List<Modelo.OcorrenciaEmpresa> ocorrenciasEmpresa)
        {
            SqlParameter[] parms = GetParameters();
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd;
                       foreach (Modelo.OcorrenciaEmpresa ocorrencia in ocorrenciasEmpresa)
                       {
                           SetDadosInc(ocorrencia);
                           SetParameters(parms, ocorrencia);
                            cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
                            ocorrencia.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Close();
                        throw (ex);
                    }
                }
            }

            //StringBuilder str = new StringBuilder();

            //bil.Chave = bil.ToMD5();
            //str.Remove(0, str.Length);
            //str.Append("INSERT INTO bilhetesimp ");
            //str.Append("(codigo,ordem, data, hora, func, relogio, importado, mar_data, mar_hora, mar_relogio, posicao, ent_sai, incdata, inchora, incusuario, chave, dscodigo, motivo, ocorrencia, idjustificativa) ");
            //str.Append("VALUES (");
            //str.Append(bil.Codigo);
            //str.Append(", '");
            //str.Append(bil.Ordem);
            //str.Append("', ");
            //str.Append("CONVERT(DATETIME, '" + bil.Data.Day + "-" + bil.Data.Month + "-" + bil.Data.Year + "',105)");
            //str.Append(", '");
            //str.Append(bil.Hora);
            //str.Append("', '");
            //str.Append(bil.Func);
            //str.Append("', '");
            //str.Append(bil.Relogio);
            //str.Append("', ");
            //str.Append(bil.Importado);
            //str.Append(", ");
            //str.Append("CONVERT(DATETIME, '" + bil.Mar_data.Value.Day + "-" + bil.Mar_data.Value.Month + "-" + bil.Mar_data.Value.Year + "', 105)");
            //str.Append(", '");
            //str.Append(bil.Mar_hora);
            //str.Append("', '");
            //str.Append(bil.Mar_relogio);
            //str.Append("', ");
            //str.Append(bil.Posicao);
            //str.Append(", '");
            //str.Append(bil.Ent_sai);
            //str.Append("', ");
            //str.Append("CONVERT(DATETIME, '" + DateTime.Now.Date.Day + "-" + DateTime.Now.Date.Month + "-" + DateTime.Now.Date.Year + "', 105)");
            //str.Append(", ");
            //str.Append("CONVERT(DATETIME, '" + DateTime.Now.Date.Year + "-" + DateTime.Now.Date.Month + "-" + DateTime.Now.Date.Day + " " + DateTime.Now.ToLongTimeString() + "', 120)");
            //str.Append(", '");
            //str.Append(UsuarioLogado.Login);
            //str.Append("', '");
            //str.Append(bil.Chave);
            //str.Append("', '");
            //str.Append(bil.DsCodigo);
            //str.Append("', '");
            //str.Append(bil.Motivo);
            //str.Append("', '");
            //str.Append(bil.Ocorrencia);
            //str.Append("', ");
            //if (bil.Idjustificativa > 0)
            //    str.Append(bil.Idjustificativa);
            //else
            //    str.Append("NULL");
            //str.Append(")");

            //return str.ToString();
        }


        #endregion
    }
}
