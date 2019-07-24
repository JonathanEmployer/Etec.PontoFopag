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
	public class Ocorrencia : DAL.SQL.DALBase, DAL.IOcorrencia
	{

		public Ocorrencia(DataBase database)
		{
			db = database;
			TABELA = "ocorrencia";

			SELECTPID = @"   SELECT * FROM ocorrencia WHERE id = @id";

			SELECTALL = @"   SELECT   ocorrencia.id
                                    , ocorrencia.descricao
                                    , ocorrencia.codigo
                                    , ocorrencia.absenteismo
                                    , ocorrencia.TipoAbono
                                    , ocorrencia.ExibePaineldoRH
                                    , ocorrencia.ObrigarAnexoPainel
                                    , ocorrencia.OcorrenciaFerias
									, ocorrencia.HorasAbonoPadrao
                                    , ocorrencia.HorasAbonoPadraoNoturno
                                    , ocorrencia.Sigla
									, ocorrencia.Ativo
                                    , ocorrencia.DefaultTipoAfastamento
                             FROM ocorrencia";

			INSERT = @"  INSERT INTO ocorrencia
							( codigo,  descricao,  incdata,  inchora,  incusuario,  absenteismo,  TipoAbono,  ExibePaineldoRH,  ObrigarAnexoPainel, OcorrenciaFerias,  HorasAbonoPadrao,  HorasAbonoPadraoNoturno,  Sigla, Ativo,  DefaultTipoAfastamento)
							VALUES
							(@codigo, @descricao, @incdata, @inchora, @incusuario, @absenteismo, @TipoAbono, @ExibePaineldoRH, @ObrigarAnexoPainel, @OcorrenciaFerias, @HorasAbonoPadrao, @HorasAbonoPadraoNoturno, @Sigla, @Ativo, @DefaultTipoAfastamento) 
						SET @id = SCOPE_IDENTITY()";

			UPDATE = @"  UPDATE ocorrencia SET
							  codigo = @codigo
							, descricao = @descricao
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , absenteismo = @absenteismo
                            , TipoAbono = @TipoAbono
                            , ExibePaineldoRH = @ExibePaineldoRH
                            , ObrigarAnexoPainel = @ObrigarAnexoPainel
                            , HorasAbonoPadrao = @HorasAbonoPadrao
                            , HorasAbonoPadraoNoturno = @HorasAbonoPadraoNoturno
                            , Sigla = @Sigla
							, Ativo = @Ativo
                            , DefaultTipoAfastamento = @DefaultTipoAfastamento
						WHERE id = @id";

			DELETE = @"  DELETE FROM ocorrencia WHERE id = @id";

			MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM ocorrencia";

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
				obj = new Modelo.Ocorrencia();
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
			((Modelo.Ocorrencia)obj).Codigo = Convert.ToInt32(dr["codigo"]);
			((Modelo.Ocorrencia)obj).Descricao = Convert.ToString(dr["descricao"]);
			((Modelo.Ocorrencia)obj).Absenteismo = Convert.ToBoolean(dr["absenteismo"]);
			((Modelo.Ocorrencia)obj).TipoAbono = (dr["TipoAbono"] is DBNull ? null : (int?)dr["TipoAbono"]);
			((Modelo.Ocorrencia)obj).ExibePaineldoRH = Convert.ToBoolean(dr["ExibePaineldoRH"]);
			((Modelo.Ocorrencia)obj).ObrigarAnexoPainel = Convert.ToBoolean(dr["ObrigarAnexoPainel"]);
			((Modelo.Ocorrencia)obj).OcorrenciaFerias = dr["OcorrenciaFerias"] is DBNull ? false : Convert.ToBoolean(dr["OcorrenciaFerias"]);
			((Modelo.Ocorrencia)obj).HorasAbonoPadrao = Convert.ToString(dr["HorasAbonoPadrao"]);
			((Modelo.Ocorrencia)obj).HorasAbonoPadraoNoturno = Convert.ToString(dr["HorasAbonoPadraoNoturno"]);
			((Modelo.Ocorrencia)obj).Sigla = Convert.ToString(dr["Sigla"]);
			((Modelo.Ocorrencia)obj).Ativo = dr["Ativo"] is DBNull ? false : Convert.ToBoolean(dr["Ativo"]);
            ((Modelo.Ocorrencia)obj).DefaultTipoAfastamento = Convert.ToInt16(dr["DefaultTipoAfastamento"]);
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
				new SqlParameter ("@absenteismo", SqlDbType.Bit),
				new SqlParameter ("@TipoAbono", SqlDbType.Int),
				new SqlParameter ("@ExibePaineldoRH", SqlDbType.Bit),
				new SqlParameter ("@ObrigarAnexoPainel", SqlDbType.Bit),
				new SqlParameter ("@OcorrenciaFerias", SqlDbType.Bit),
				new SqlParameter ("@HorasAbonoPadrao", SqlDbType.VarChar),
				new SqlParameter ("@HorasAbonoPadraoNoturno", SqlDbType.VarChar),
				new SqlParameter ("@Sigla", SqlDbType.VarChar),
				new SqlParameter ("@Ativo", SqlDbType.Bit),
                new SqlParameter ("@DefaultTipoAfastamento", SqlDbType.SmallInt)
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
			parms[1].Value = ((Modelo.Ocorrencia)obj).Codigo;
			parms[2].Value = ((Modelo.Ocorrencia)obj).Descricao;
			parms[3].Value = ((Modelo.Ocorrencia)obj).Incdata;
			parms[4].Value = ((Modelo.Ocorrencia)obj).Inchora;
			parms[5].Value = ((Modelo.Ocorrencia)obj).Incusuario;
			parms[6].Value = ((Modelo.Ocorrencia)obj).Altdata;
			parms[7].Value = ((Modelo.Ocorrencia)obj).Althora;
			parms[8].Value = ((Modelo.Ocorrencia)obj).Altusuario;
			parms[9].Value = ((Modelo.Ocorrencia)obj).Absenteismo;
			parms[10].Value = ((Modelo.Ocorrencia)obj).TipoAbono;
			parms[11].Value = ((Modelo.Ocorrencia)obj).ExibePaineldoRH;
			parms[12].Value = ((Modelo.Ocorrencia)obj).ObrigarAnexoPainel;
			parms[13].Value = ((Modelo.Ocorrencia)obj).OcorrenciaFerias;
			parms[14].Value = ((Modelo.Ocorrencia)obj).HorasAbonoPadrao;
			parms[15].Value = ((Modelo.Ocorrencia)obj).HorasAbonoPadraoNoturno;
			parms[16].Value = ((Modelo.Ocorrencia)obj).Sigla;
			parms[17].Value = ((Modelo.Ocorrencia)obj).Ativo;
            parms[18].Value = ((Modelo.Ocorrencia)obj).DefaultTipoAfastamento;
		}

		public Modelo.Ocorrencia LoadObject(int id)
		{
			SqlDataReader dr = LoadDataReader(id);

			Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
			try
			{

				SetInstance(dr, objOcorrencia);
			}
			catch (Exception ex)
			{
				throw (ex);
			}
			return objOcorrencia;
		}

		public Modelo.Ocorrencia LoadObjectByCodigo(int pCodigo)
		{

			SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter("@codigo", SqlDbType.Int)
			};
			parms[0].Value = pCodigo;

			string sql = " SELECT * " +
							" FROM ocorrencia" +
							" WHERE codigo = @codigo" +
                              " AND ativo = 1 ";

			SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

			Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
			try
			{
				AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Ocorrencia>();
				objOcorrencia = AutoMapper.Mapper.Map<List<Modelo.Ocorrencia>>(dr).FirstOrDefault();
			}
			catch (Exception ex)
			{
				throw (ex);
			}
			return objOcorrencia;
		}

		public Hashtable GetHashIdDescricao()
		{
			SqlParameter[] parms = new SqlParameter[0];

			SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM ocorrencia", parms);

			Hashtable lista = new Hashtable();
			try
			{
				while (dr.Read())
				{
					lista.Add(Convert.ToInt32(dr["id"]), Convert.ToString(dr["descricao"]));
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

		public List<Modelo.Ocorrencia> GetAllList()
		{
			SqlParameter[] parms = new SqlParameter[0];

			SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM ocorrencia", parms);

			List<Modelo.Ocorrencia> lista = new List<Modelo.Ocorrencia>();
			try
			{
				while (dr.Read())
				{
					Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
					AuxSetInstance(dr, objOcorrencia);
					lista.Add(objOcorrencia);
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

        public List<Modelo.Ocorrencia> GetAllListConsultaEvento()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM ocorrencia WHERE ativo = 1", parms);

            List<Modelo.Ocorrencia> lista = new List<Modelo.Ocorrencia>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
                    AuxSetInstance(dr, objOcorrencia);
                    lista.Add(objOcorrencia);
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

        public List<Modelo.Ocorrencia> GetAllPorExibePainelRHPorEmpresa(int idEmpresa)
		{
			SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter("@idEmpresa", SqlDbType.Int)
			};
			parms[0].Value = idEmpresa;

			SqlDataReader dr = db.ExecuteReader(CommandType.Text,
										@"IF ((SELECT COUNT(ocoEmp.id) 
		                                       FROM ocorrenciaempresa ocoEmp 
											   JOIN ocorrencia o on o.id = ocoEmp.idOcorrencia
		                                       WHERE ocoEmp.idEmpresa = @idEmpresa AND ocoEmp.idOcorrencia = o.id) > 0)
                                            SELECT o.* 
                                            FROM ocorrencia o
                                            WHERE o.ExibePaineldoRH = 1 
                                            AND o.ID in (SELECT ocoEmp.idOcorrencia
		                                                   FROM ocorrenciaempresa ocoEmp 
		                                                   WHERE ocoEmp.idEmpresa = @idEmpresa AND ocoEmp.idOcorrencia = o.id);
                                          ELSE 
                                            SELECT o.* 
                                            FROM ocorrencia o
                                            WHERE o.ExibePaineldoRH = 1 ", parms);

			List<Modelo.Ocorrencia> lista = new List<Modelo.Ocorrencia>();
			try
			{
				AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Ocorrencia>();
				lista = AutoMapper.Mapper.Map<List<Modelo.Ocorrencia>>(dr);
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

		public List<Modelo.Ocorrencia> GetAllPorExibePaineldoRH()
		{
			SqlParameter[] parms = new SqlParameter[0];

			SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM ocorrencia where ExibePaineldoRH = 1 and ativo = 1", parms);

			List<Modelo.Ocorrencia> lista = new List<Modelo.Ocorrencia>();
			try
			{
				while (dr.Read())
				{
					Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
					AuxSetInstance(dr, objOcorrencia);
					lista.Add(objOcorrencia);
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

		public List<Modelo.Proxy.pxyOcorrenciaEvento> GetAllOcorrenciaEventoList()
		{
			SqlParameter[] parms = new SqlParameter[0];

			SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM ocorrencia", parms);

			List<Modelo.Proxy.pxyOcorrenciaEvento> lista = new List<Modelo.Proxy.pxyOcorrenciaEvento>();
			try
			{
				Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyOcorrenciaEvento>();
				lista = Mapper.Map<List<Modelo.Proxy.pxyOcorrenciaEvento>>(dr);
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

		public int? getOcorrenciaNome(string pDescricao)
		{
			SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter("@descricao", SqlDbType.VarChar)
			};
			parms[0].Value = pDescricao;

			string cmd = " SELECT id " +
							" FROM ocorrencia" +
							" WHERE descricao LIKE '" + pDescricao + "' COLLATE Latin1_General_CI_AI  ";

			int? valor = (int?)db.ExecuteScalar(CommandType.Text, cmd, parms);

			return valor;
		}

		public List<Modelo.Ocorrencia> GetAllListPorIds(List<int> ids)
		{
			List<Modelo.Ocorrencia> result = new List<Modelo.Ocorrencia>();

			try
			{
				var parameters = new string[ids.Count];
				List<SqlParameter> parmList = new List<SqlParameter>();
				for (int i = 0; i < ids.Count; i++)
				{
					parameters[i] = string.Format("@Id{0}", i);
					parmList.Add(new SqlParameter(parameters[i], ids[i]));
				}

				string sql = string.Format("SELECT * from Ocorrencia WHERE Id IN ({0})", string.Join(", ", parameters));

				SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parmList.ToArray());

				try
				{
					while (dr.Read())
					{
						Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
						AuxSetInstance(dr, objOcorrencia);
						result.Add(objOcorrencia);
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

		public int? GetIdPorIdIntegracao(int idIntegracao)
		{
			SqlParameter[] parms = new SqlParameter[0];
			DataTable dt = new DataTable();
			string sql = "select top 1 id from ocorrencia where idIntegracao = " + idIntegracao;
			int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, sql, parms));
			return Id;
		}

		#endregion
	}
}