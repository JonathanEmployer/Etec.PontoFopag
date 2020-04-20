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
        private DAL.SQL.OcorrenciaRestricao _dalOcorrenciaRestricao;
        public DAL.SQL.OcorrenciaRestricao dalOcorrenciaRestricao
        {
            get { return _dalOcorrenciaRestricao; }
            set { _dalOcorrenciaRestricao = value; }
        }

        public Ocorrencia(DataBase database)
		{
			db = database;
            _dalOcorrenciaRestricao = new OcorrenciaRestricao(db);
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

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.IncluirAux(trans, obj);
            AuxSalvarOcorrenciaRestricao(trans, ((Modelo.Ocorrencia)obj));
        }
        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase objBase)
        {
            base.AlterarAux(trans, objBase);
            AuxSalvarOcorrenciaRestricao(trans, ((Modelo.Ocorrencia)objBase));
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase objBase)
        {
            dalOcorrenciaRestricao.ExcluirByOcorrencias(trans, new List<int>() { objBase.Id });
            base.ExcluirAux(trans, objBase);
        }

        private void AuxSalvarOcorrenciaRestricao(SqlTransaction trans, Modelo.Ocorrencia obj)
        {
            IList<Modelo.OcorrenciaRestricao> restricoes = ((Modelo.Ocorrencia)obj).OcorrenciaRestricao;
            if (restricoes != null && restricoes.Count > 0)
            {
                restricoes.ToList().ForEach(f => f.IdOcorrencia = ((Modelo.Ocorrencia)obj).Id);
                if (restricoes != null && restricoes.Count > 0)
                {
                    var despresaDuplicados = restricoes.GroupBy(g => new { g.Acao, g.Excluir, g.IdContrato, g.IdEmpresa, g.IdOcorrencia, g.TipoRestricao });
                    List<Modelo.OcorrenciaRestricao> registrosSalvar = new List<Modelo.OcorrenciaRestricao>();
                    foreach (var grupos in despresaDuplicados)
                    {
                        Modelo.OcorrenciaRestricao ocorrenciaRestricaoOperacao = grupos.FirstOrDefault();
                        registrosSalvar.Add(ocorrenciaRestricaoOperacao);
                    }
                    var RegistrosExcluir = registrosSalvar.Where(w => w.Excluir && w.Id > 0).ToList();
                    if (RegistrosExcluir.Count() > 0)
                    {
                        List<Modelo.ModeloBase> RegistrosExcluirBase = RegistrosExcluir.ConvertAll(x => (Modelo.ModeloBase)x);
                        dalOcorrenciaRestricao.ExcluirRegistros(RegistrosExcluirBase, trans);
                    }

                    var RegistrosIncluir = registrosSalvar.Where(w => !w.Excluir && w.Id == 0).ToList();
                    if (RegistrosIncluir.Count() > 0)
                    {
                        dalOcorrenciaRestricao.InserirRegistros(RegistrosIncluir, trans);
                    }
                } 
            }
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

		public Modelo.Ocorrencia LoadObjectByCodigo(int pCodigo, bool validaPermissaoUser)
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
            if (validaPermissaoUser)
            {
                sql += AddPermissaoUsuario("ocorrencia.id");
            }

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

		public List<Modelo.Ocorrencia> GetAllList(bool validaPermissaoUser)
		{
			SqlParameter[] parms = new SqlParameter[0];
            string sql = "SELECT * FROM ocorrencia where 1 = 1 ";
            if (validaPermissaoUser)
            {
                sql += AddPermissaoUsuario("ocorrencia.id");
            }

			SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

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

        public List<Modelo.Ocorrencia> GetAllListConsultaEvento(bool validaPermissaoUser)
        {
            SqlParameter[] parms = new SqlParameter[0];

            string sql = "SELECT * FROM ocorrencia WHERE ativo = 1 ";
            if (validaPermissaoUser)
            {
                sql += AddPermissaoUsuario("ocorrencia.id");
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

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

		public List<Modelo.Ocorrencia> GetAllPorExibePainelRHPorFuncionario(int idFuncionario)
		{
			SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter("@idFuncionario", SqlDbType.Int)
			};
			parms[0].Value = idFuncionario;

			SqlDataReader dr = db.ExecuteReader(CommandType.Text,
										@"  SELECT o.*
											  FROM ocorrencia o
											 WHERE o.ExibePaineldoRH = 1
											   AND NOT EXISTS (SELECT top 1 1 FROM OcorrenciaRestricao ocr WHERE ocr.IdOcorrencia = o.id)
											   AND o.Ativo = 1
											 UNION ALL 
											SELECT o.*
											  FROM ocorrencia o
											  JOIN OcorrenciaRestricao ocr on o.id = ocr.IdOcorrencia
											  JOIN empresa e on ocr.IdEmpresa = e.id
											  JOIN funcionario f on e.id = f.idempresa and f.id = @idFuncionario
											 WHERE o.ExibePaineldoRH = 1
											   AND o.Ativo = 1
											 UNION ALL 
											SELECT o.*
											  FROM ocorrencia o
											  JOIN OcorrenciaRestricao ocr on o.id = ocr.IdOcorrencia
											  JOIN contratofuncionario cf on ocr.IdContrato = cf.idcontrato
											  JOIN funcionario f on cf.idfuncionario = f.id and f.id = @idFuncionario
											 WHERE o.ExibePaineldoRH = 1
											   AND o.Ativo = 1 ", parms);

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

        #region Permissões
        /// <summary>
        /// Esse método adicionará a condição de acordo com a permissão do usuário e a restrição de permissão da ocorrencia.
        /// </summary>
        /// <param name="campoIdOcorrencia">informar o campo que está o id da ocorrencia, por exemplo em uma consulta "select * from ocorrencia where 1 = 1 " informar ocorrencia.id em uma consulta com alias "select O.* from ocorrencia h where 1 = 1 " informar O.id ... </param>
        /// <returns></returns>
        public string AddPermissaoUsuario(string campoIdOcorrencia)
        {
            if (UsuarioLogado != null)
            {
                string condicionalEmpresa = String.Format(@" (EXISTS ((SELECT 1
									 FROM empresacwusuario eu
									inner join OcorrenciaRestricao oc on eu.idempresa = oc.IdEmpresa
												   WHERE eu.idcw_usuario = {0} AND oc.IdOcorrencia = {1}))
								  ) ", UsuarioLogado.Id, campoIdOcorrencia);
                string condicionalContrato = String.Format(@" (EXISTS ((SELECT 1
									 FROM contratousuario cu
									inner join OcorrenciaRestricao oc on cu.idcontrato = oc.IdContrato
												   WHERE cu.idcwusuario = {0} AND oc.IdOcorrencia = {1}))
								  ) ", UsuarioLogado.Id, campoIdOcorrencia);
                string condicionalOcorrenciaSemRestricao = String.Format(@" (NOT EXISTS(SELECT 1
                                                      FROM OcorrenciaRestricao oc
                                                    WHERE oc.idOcorrencia = {0} )) ", campoIdOcorrencia);

                if (UsuarioLogado.UtilizaControleEmpresa && UsuarioLogado.UtilizaControleContratos)
                {
                    return String.Format(" AND ({0} OR {1} OR {2}) ", condicionalOcorrenciaSemRestricao, condicionalEmpresa, condicionalContrato);
                }
                else if (UsuarioLogado.UtilizaControleEmpresa && !UsuarioLogado.UtilizaControleContratos)
                {
                    return String.Format(" AND ({0} OR {1}) ", condicionalOcorrenciaSemRestricao, condicionalEmpresa);
                }
                else if (UsuarioLogado.UtilizaControleContratos && !UsuarioLogado.UtilizaControleEmpresa)
                {
                    return String.Format(" AND ({0} OR {1}) ", condicionalOcorrenciaSemRestricao, condicionalContrato);
                }
            }
            return "";
        }
        #endregion
    }
}