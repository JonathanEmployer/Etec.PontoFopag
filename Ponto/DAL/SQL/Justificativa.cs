using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class Justificativa : DAL.SQL.DALBase, DAL.IJustificativa
    {
        private DAL.SQL.JustificativaRestricao _dalJustificativaRestricao;
        public DAL.SQL.JustificativaRestricao dalJustificativaRestricao
        {
            get { return _dalJustificativaRestricao; }
            set { _dalJustificativaRestricao = value; }
        }

        public Justificativa(DataBase database)
        {
            db = database;
            _dalJustificativaRestricao = new JustificativaRestricao(db);
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

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.IncluirAux(trans, obj);
            AuxSalvarJustificativaRestricao(trans, ((Modelo.Justificativa)obj));
        }
        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase objBase)
        {
            base.AlterarAux(trans, objBase);
            AuxSalvarJustificativaRestricao(trans, ((Modelo.Justificativa)objBase));
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase objBase)
        {
            dalJustificativaRestricao.ExcluirByJustificativas(trans, new List<int>() { objBase.Id });
            base.ExcluirAux(trans, objBase);
        }

        private void AuxSalvarJustificativaRestricao(SqlTransaction trans, Modelo.Justificativa obj)
        {
            IList<Modelo.JustificativaRestricao> restricoes = ((Modelo.Justificativa)obj).JustificativaRestricao;
            if (restricoes != null && restricoes.Count > 0)
            {
                restricoes.ToList().ForEach(f => f.IdJustificativa = ((Modelo.Justificativa)obj).Id);
                if (restricoes != null && restricoes.Count > 0)
                {
                    var despresaDuplicados = restricoes.GroupBy(g => new { g.Acao, g.Excluir, g.IdContrato, g.IdEmpresa, g.IdJustificativa, g.TipoRestricao });
                    List<Modelo.JustificativaRestricao> registrosSalvar = new List<Modelo.JustificativaRestricao>();
                    foreach (var grupos in despresaDuplicados)
                    {
                        Modelo.JustificativaRestricao JustificativaRestricaoOperacao = grupos.FirstOrDefault();
                        registrosSalvar.Add(JustificativaRestricaoOperacao);
                    }
                    var RegistrosExcluir = registrosSalvar.Where(w => w.Excluir && w.Id > 0).ToList();
                    if (RegistrosExcluir.Count() > 0)
                    {
                        List<Modelo.ModeloBase> RegistrosExcluirBase = RegistrosExcluir.ConvertAll(x => (Modelo.ModeloBase)x);
                        dalJustificativaRestricao.ExcluirRegistros(RegistrosExcluirBase, trans);
                    }

                    var RegistrosIncluir = registrosSalvar.Where(w => !w.Excluir && w.Id == 0).ToList();
                    if (RegistrosIncluir.Count() > 0)
                    {
                        dalJustificativaRestricao.InserirRegistros(RegistrosIncluir, trans);
                    }
                }
            }
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

        public int? GetIdPorCod(int Cod, bool validaPermissaoUser)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string consulta = String.Format("select top 1 id from justificativa where codigo = {0} and ativo = 1 ", Cod);
            if (validaPermissaoUser)
            {
                consulta += AddPermissaoUsuario("justificativa.id");
            }
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, consulta, parms));

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

        public List<Modelo.Justificativa> GetAllList(bool validaPermissaoUser)
        {
            SqlParameter[] parms = new SqlParameter[0];
            string sql = " SELECT * FROM justificativa where 1 = 1 ";
            if (validaPermissaoUser)
            {
                sql += AddPermissaoUsuario(" justificativa.id ");
            }
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

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

        public List<Modelo.Justificativa> GetAllListConsultaEvento(bool validaPermissaoUser)
        {
            SqlParameter[] parms = new SqlParameter[0];

            string consulta = " SELECT * FROM justificativa WHERE ativo = 1 ";
            if (validaPermissaoUser)
            {
                consulta += AddPermissaoUsuario("justificativa.id");
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, consulta, parms);

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

        public List<Modelo.Justificativa> GetAllPorExibePainelRHPorFuncionario(int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idFuncionario", SqlDbType.Int)
            };
            parms[0].Value = idFuncionario;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text,
                                        @"  SELECT j.*
                                              FROM justificativa j
                                             WHERE j.ExibePaineldoRH = 1
                                               AND NOT EXISTS (SELECT top 1 1 FROM JustificativaRestricao jr WHERE jr.IdJustificativa = j.id)
                                               AND j.Ativo = 1
                                             UNION ALL 
                                            SELECT j.*
                                              FROM justificativa j
                                              JOIN JustificativaRestricao jr on j.id = jr.IdJustificativa
                                              JOIN empresa e on jr.IdEmpresa = e.id
                                              JOIN funcionario f on e.id = f.idempresa and f.id = @idFuncionario
                                             WHERE j.ExibePaineldoRH = 1
                                               AND j.Ativo = 1
                                             UNION ALL 
                                            SELECT j.*
                                              FROM justificativa j
                                              JOIN JustificativaRestricao jr on j.id = jr.IdJustificativa
                                              JOIN contratofuncionario cf on jr.IdContrato = cf.idcontrato
                                              JOIN funcionario f on cf.idfuncionario = f.id and f.id = @idFuncionario
                                             WHERE j.ExibePaineldoRH = 1
                                               AND j.Ativo = 1 ", parms);

            List<Modelo.Justificativa> lista = new List<Modelo.Justificativa>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Justificativa>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Justificativa>>(dr);
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

        #region Permissões
        /// <summary>
        /// Esse método adicionará a condição de acordo com a permissão do usuário e a restrição de permissão da Justificativa.
        /// </summary>
        /// <param name="campoIdJustificativa">informar o campo que está o id da Justificativa, por exemplo em uma consulta "select * from Justificativa where 1 = 1 " informar Justificativa.id em uma consulta com alias "select O.* from Justificativa h where 1 = 1 " informar O.id ... </param>
        /// <returns></returns>
        public string AddPermissaoUsuario(string campoIdJustificativa)
        {
            if (UsuarioLogado != null)
            {
                string condicionalEmpresa = String.Format(@" (EXISTS ((SELECT 1
									 FROM empresacwusuario eu
									inner join JustificativaRestricao jr on eu.idempresa = jr.IdEmpresa
												   WHERE eu.idcw_usuario = {0} AND jr.IdJustificativa = {1}))
								  ) ", UsuarioLogado.Id, campoIdJustificativa);
                string condicionalContrato = String.Format(@" (EXISTS ((SELECT 1
									 FROM contratousuario cu
									inner join JustificativaRestricao jr on cu.idcontrato = jr.IdContrato
												   WHERE cu.idcwusuario = {0} AND jr.IdJustificativa = {1}))
								  ) ", UsuarioLogado.Id, campoIdJustificativa);
                string condicionalJustificativaSemRestricao = String.Format(@" (NOT EXISTS(SELECT 1
                                                      FROM JustificativaRestricao jr
                                                    WHERE jr.idJustificativa = {0} )) ", campoIdJustificativa);

                if (UsuarioLogado.UtilizaControleEmpresa && UsuarioLogado.UtilizaControleContratos)
                {
                    return String.Format(" AND ({0} OR {1} OR {2}) ", condicionalJustificativaSemRestricao, condicionalEmpresa, condicionalContrato);
                }
                else if (UsuarioLogado.UtilizaControleEmpresa && !UsuarioLogado.UtilizaControleContratos)
                {
                    return String.Format(" AND ({0} OR {1}) ", condicionalJustificativaSemRestricao, condicionalEmpresa);
                }
                else if (UsuarioLogado.UtilizaControleContratos && !UsuarioLogado.UtilizaControleEmpresa)
                {
                    return String.Format(" AND ({0} OR {1}) ", condicionalJustificativaSemRestricao, condicionalContrato);
                }
            }
            return "";
        }
        #endregion
    }
}
