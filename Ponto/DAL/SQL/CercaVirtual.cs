using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class CercaVirtual : DAL.SQL.DALBase, ICercaVirtual
    {
        string DELETECERCAVIRTUALFUNCIONARIO = "";
        string DELETEFUNCIONARIO = "";
        string INSERTCERCAVIRTUALFUNCIONARIO = "";
        string SELECTCERCAVIRTUALFUNCIONARIO = "";
        string SELECTCERCAVIRTUALFUNCIONARIOCODIGO = "";
        public CercaVirtual(DataBase database)
        {
            db = database;
            TABELA = "CercaVirtual";

            SELECTPID = @"   SELECT * FROM CercaVirtual WHERE id = @id";

            SELECTALL = @"   SELECT * FROM CercaVirtual";

            INSERT = @"  INSERT INTO CercaVirtual
							(Codigo, Descricao,	TipoDescricao, Endereco, Latitude, Longitude, Raio,	Ativo)
							VALUES
							(@Codigo, @Descricao, @TipoDescricao, @Endereco, @Latitude , @Longitude, @Raio, @Ativo) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE CercaVirtual SET
							  Descricao = @Descricao
							, TipoDescricao = @TipoDescricao
                            , Endereco = @Endereco
                            , Latitude = @Latitude
                            , Longitude = @Longitude
                            , Raio = @Raio
                            , Ativo = @Ativo
						WHERE id = @id";

            DELETE = @"  DELETE FROM CercaVirtual WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM CercaVirtual";

            DELETECERCAVIRTUALFUNCIONARIO = @"delete from CercaVirtualFuncionario where idCercaVirtual = @idCercaVirtual";
            DELETEFUNCIONARIO = @"delete 
                                    from CercaVirtualFuncionario 
                                   where idCercaVirtual = (select top 1 id 
                                                             from CercaVirtual 
                                                            where id = @CodigoCercaVirtual) 
                                     and idFuncionario = (select top 1 id 
                                                            from funcionario 
                                                           where codigo = @CodigoFuncionario)";

            INSERTCERCAVIRTUALFUNCIONARIO = @"INSERT INTO CercaVirtualFuncionario (idCercaVirtual, idFuncionario)
                                                     SELECT @idCercaVirtual, id FROM funcionario WHERE id IN (SELECT * FROM dbo.F_ClausulaIn(@idFuncionario))";

            SELECTCERCAVIRTUALFUNCIONARIO = "SELECT idFuncionario FROM CERCAVIRTUALFUNCIONARIO WHERE idCercaVirtual = @idCercaVirtual";

            SELECTCERCAVIRTUALFUNCIONARIOCODIGO = @"SELECT cv.* 
                                                      FROM funcionario f
                                                     INNER JOIN CercaVirtualFuncionario cvf ON cvf.idFuncionario = f.id
                                                     INNER JOIN CercaVirtual cv ON cv.Id = cvf.idCercaVirtual
                                                     WHERE f.codigo = @CodigoFuncionario";
        }

        protected override bool SetInstance(System.Data.SqlClient.SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    //SetInstanceBase(dr, obj);
                    ((Modelo.CercaVirtual)obj).Codigo = (dr["Codigo"] is DBNull ? 0 : int.Parse(dr["Codigo"].ToString()));
                    ((Modelo.CercaVirtual)obj).Descricao = (dr["Descricao"] is DBNull ? "" : dr["Descricao"].ToString());
                    ((Modelo.CercaVirtual)obj).TipoDescricao = (dr["TipoDescricao"] is DBNull ? "" : dr["TipoDescricao"].ToString());
                    ((Modelo.CercaVirtual)obj).Endereco = (dr["Endereco"] is DBNull ? "" : dr["Endereco"].ToString());
                    ((Modelo.CercaVirtual)obj).Latitude = (dr["Latitude"] is DBNull ? "" : dr["Latitude"].ToString().Replace(",",""));
                    ((Modelo.CercaVirtual)obj).Longitude = (dr["Longitude"] is DBNull ? "" : dr["Longitude"].ToString().Replace(",", ""));
                    ((Modelo.CercaVirtual)obj).Raio = (dr["Raio"] is DBNull ? 1000 : Convert.ToInt32(dr["Raio"]));
                    ((Modelo.CercaVirtual)obj).Ativo = (dr["Ativo"] is DBNull ? true : bool.Parse(dr["Ativo"].ToString()));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Biometria();
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

        protected override System.Data.SqlClient.SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@Codigo", SqlDbType.Int),
                new SqlParameter ("@Descricao", SqlDbType.VarChar),
                new SqlParameter ("@TipoDescricao", SqlDbType.VarChar),
                new SqlParameter ("@Endereco", SqlDbType.VarChar),
                new SqlParameter ("@Latitude", SqlDbType.VarChar),
                new SqlParameter ("@Longitude", SqlDbType.VarChar),
                new SqlParameter ("@Raio", SqlDbType.Int),
                new SqlParameter ("@Ativo", SqlDbType.Bit)
            };
            return parms;
        }

        protected override void SetParameters(System.Data.SqlClient.SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.CercaVirtual)obj).Codigo;
            parms[2].Value = ((Modelo.CercaVirtual)obj).Descricao;
            parms[3].Value = ((Modelo.CercaVirtual)obj).TipoDescricao;
            parms[4].Value = ((Modelo.CercaVirtual)obj).Endereco;
            parms[5].Value = ((Modelo.CercaVirtual)obj).Latitude;
            parms[6].Value = ((Modelo.CercaVirtual)obj).Longitude;
            parms[7].Value = ((Modelo.CercaVirtual)obj).Raio;
            parms[8].Value = ((Modelo.CercaVirtual)obj).Ativo;
        }
        public override void Alterar(Modelo.ModeloBase obj)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        AlterarAux(trans, obj);
                        ExcluirCercaVirtualFuncionario(obj.Id, trans);
                        IncluirLoteIdsFuncionario(trans, obj.Id, ((Modelo.CercaVirtual)obj).idsFuncionariosSelecionados);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Dispose();
                        throw (ex);
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                }
            }
        }
        public override void Incluir(Modelo.ModeloBase obj)
        {
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        IncluirAux(trans, obj);
                        IncluirLoteIdsFuncionario(trans, obj.Id, ((Modelo.CercaVirtual)obj).idsFuncionariosSelecionados);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Dispose();
                        throw (ex);
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                }
            }
        }
        public void IncluirLoteIdsFuncionario(SqlTransaction trans, int idCercaVirtual, string idsFuncs)
        {
            if (!string.IsNullOrEmpty(idsFuncs))
            {
                SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@idCercaVirtual", SqlDbType.Int),
                    new SqlParameter("@idFuncionario", SqlDbType.VarChar)
                };

                parms[0].Value = idCercaVirtual;
                parms[1].Value = String.Join(",", idsFuncs);

                TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERTCERCAVIRTUALFUNCIONARIO, true, parms);
            }
        }
        private void ExcluirCercaVirtualFuncionario(int idCercaVirtual, SqlTransaction trans)
        {
            try
            {
                SqlParameter[] parms = { new SqlParameter("@idCercaVirtual", SqlDbType.Int) };
                parms[0].Value = idCercaVirtual;
                TransactDbOps.ExecuteScalar(trans, CommandType.Text, DELETECERCAVIRTUALFUNCIONARIO, parms);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public void Excluir(int CodigoCercaVirtual)
        {
            //evento para excluir os funcionarios vinculados a cerca e logo apos a cerca virtual
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //exclusao dos vinculos dos funcionarios
                        SqlParameter[] parms = {
                            new SqlParameter("@idCercaVirtual", SqlDbType.Int)
                        };
                        parms[0].Value = CodigoCercaVirtual;
                        TransactDbOps.ExecuteScalar(trans, CommandType.Text, DELETECERCAVIRTUALFUNCIONARIO, parms);

                        //exclusao da cerca
                        SqlParameter[] parmsCerca = {
                            new SqlParameter("@id", SqlDbType.Int)
                        };
                        parmsCerca[0].Value = CodigoCercaVirtual;
                        TransactDbOps.ExecuteScalar(trans, CommandType.Text, DELETE , parmsCerca);

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Dispose();
                        throw (ex);
                    }
                    finally
                    {
                        trans.Dispose();
                    }
                }
            }
        }

        private string SelectCercaVirtualFuncionario(int idCercaVirtual)
        {
            try
            {
                SqlParameter[] parms = { new SqlParameter("@idCercaVirtual", SqlDbType.Int) };
                parms[0].Value = idCercaVirtual;
                SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTCERCAVIRTUALFUNCIONARIO, parms);

                var IdfFuncionario = "";
                try
                {
                    while (dr.Read())
                    {
                        if (string.IsNullOrEmpty(IdfFuncionario))
                            IdfFuncionario = dr["idFuncionario"].ToString();
                        else
                            IdfFuncionario += $",{dr["idFuncionario"]}";
                    }
                    return IdfFuncionario;
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
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public Modelo.CercaVirtual LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.CercaVirtual objCercaVirtual = new Modelo.CercaVirtual();
            try
            {
                SetInstance(dr, objCercaVirtual);
                objCercaVirtual.idsFuncionariosSelecionados = SelectCercaVirtualFuncionario(id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCercaVirtual;
        }

        public List<Modelo.CercaVirtual> GetAllList(int Codigo)
        {
            SqlDataReader dr;

            if (Codigo > 0)
            {
                SqlParameter[] parms = { new SqlParameter("@CodigoFuncionario", SqlDbType.Int) };
                parms[0].Value = Codigo;

                dr = db.ExecuteReader(CommandType.Text, SELECTCERCAVIRTUALFUNCIONARIOCODIGO, parms);
            }
            else
            {
                SqlParameter[] parms = new SqlParameter[0];
                dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);
            }

            List<Modelo.CercaVirtual> lista = new List<Modelo.CercaVirtual>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.CercaVirtual>();
                lista = AutoMapper.Mapper.Map<List<Modelo.CercaVirtual>>(dr);
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
    }
}
