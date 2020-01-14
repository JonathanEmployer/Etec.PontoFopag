using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class TransferenciaBilhetes : DAL.SQL.DALBase, DAL.ITransferenciaBilhetes
    {

        public TransferenciaBilhetes(DataBase database)
        {
            db = database;
            TABELA = "TransferenciaBilhetes";

            SELECTPID = @"   SELECT tb.*,
                                    CONCAT(fOrigem.codigo, ' | ', fOrigem.nome) FuncionarioOrigem,
                                    CONCAT(fDestino.codigo, ' | ', fDestino.nome) FuncionarioDestino
                               FROM TransferenciaBilhetes tb
                          inner join funcionario fOrigem on tb.IdFuncionarioOrigem = fOrigem.id
                          inner join funcionario fDestino on tb.IdFuncionarioDestino = fDestino.id
                              WHERE tb.id = @id";

            SELECTALL = @"   SELECT   tb.*,
                                      CONCAT(fOrigem.codigo, ' | ', fOrigem.nome) FuncionarioOrigem,
                                      CONCAT(fDestino.codigo, ' | ', fDestino.nome) FuncionarioDestino
                             FROM TransferenciaBilhetes 
                            inner join funcionario fOrigem on tb.IdFuncionarioOrigem = fOrigem.id 
                            inner join funcionario fDestino on tb.IdFuncionarioDestino = fDestino.id";

            INSERT = @"  INSERT INTO TransferenciaBilhetes
							(codigo, incdata, inchora, incusuario, DataInicio,DataFim,IdFuncionarioOrigem,IdFuncionarioDestino)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @DataInicio,@DataFim,@IdFuncionarioOrigem,@IdFuncionarioDestino)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE TransferenciaBilhetes SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,DataInicio = @DataInicio
                           ,DataFim = @DataFim
                           ,IdFuncionarioOrigem = @IdFuncionarioOrigem
                           ,IdFuncionarioDestino = @IdFuncionarioDestino

						WHERE id = @id";

            DELETE = @"  DELETE FROM TransferenciaBilhetes WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM TransferenciaBilhetes";

        }

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
                obj = new Modelo.TransferenciaBilhetes();
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
            ((Modelo.TransferenciaBilhetes)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.TransferenciaBilhetes)obj).DataInicio = Convert.ToDateTime(dr["DataInicio"]);
            ((Modelo.TransferenciaBilhetes)obj).DataFim = Convert.ToDateTime(dr["DataFim"]);
            ((Modelo.TransferenciaBilhetes)obj).IdFuncionarioOrigem = Convert.ToInt32(dr["IdFuncionarioOrigem"]);
            ((Modelo.TransferenciaBilhetes)obj).IdFuncionarioDestino = Convert.ToInt32(dr["IdFuncionarioDestino"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				 new SqlParameter ("@id", SqlDbType.Int)
				,new SqlParameter ("@codigo", SqlDbType.Int)
				,new SqlParameter ("@incdata", SqlDbType.DateTime)
				,new SqlParameter ("@inchora", SqlDbType.DateTime)
				,new SqlParameter ("@incusuario", SqlDbType.VarChar)
				,new SqlParameter ("@altdata", SqlDbType.DateTime)
				,new SqlParameter ("@althora", SqlDbType.DateTime)
				,new SqlParameter ("@altusuario", SqlDbType.VarChar)
                ,new SqlParameter ("@DataInicio", SqlDbType.DateTime)
                ,new SqlParameter ("@DataFim", SqlDbType.DateTime)
                ,new SqlParameter ("@IdFuncionarioOrigem", SqlDbType.Int)
                ,new SqlParameter ("@IdFuncionarioDestino", SqlDbType.Int)
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
            parms[1].Value = ((Modelo.TransferenciaBilhetes)obj).Codigo;
            parms[2].Value = ((Modelo.TransferenciaBilhetes)obj).Incdata;
            parms[3].Value = ((Modelo.TransferenciaBilhetes)obj).Inchora;
            parms[4].Value = ((Modelo.TransferenciaBilhetes)obj).Incusuario;
            parms[5].Value = ((Modelo.TransferenciaBilhetes)obj).Altdata;
            parms[6].Value = ((Modelo.TransferenciaBilhetes)obj).Althora;
            parms[7].Value = ((Modelo.TransferenciaBilhetes)obj).Altusuario;
            parms[8].Value = ((Modelo.TransferenciaBilhetes)obj).DataInicio;
            parms[9].Value = ((Modelo.TransferenciaBilhetes)obj).DataFim;
            parms[10].Value = ((Modelo.TransferenciaBilhetes)obj).IdFuncionarioOrigem;
            parms[11].Value = ((Modelo.TransferenciaBilhetes)obj).IdFuncionarioDestino;
        }

        public Modelo.TransferenciaBilhetes LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);
            List<Modelo.TransferenciaBilhetes> lista = new List<Modelo.TransferenciaBilhetes>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.TransferenciaBilhetes>();
                lista = AutoMapper.Mapper.Map<List<Modelo.TransferenciaBilhetes>>(dr);
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
            return lista.Count > 0 ? lista.FirstOrDefault() : new Modelo.TransferenciaBilhetes();
        }

        public List<Modelo.TransferenciaBilhetes> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.TransferenciaBilhetes> lista = new List<Modelo.TransferenciaBilhetes>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.TransferenciaBilhetes>();
                lista = AutoMapper.Mapper.Map<List<Modelo.TransferenciaBilhetes>>(dr);
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

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.IncluirAux(trans, obj);
            ((Modelo.TransferenciaBilhetes)obj).TransferenciaBilhetesDetalhes.ForEach(f => f.IdTransferenciaBilhetes = obj.Id);
            TransferenciaBilhetesDetalhes dalTransferenciaBilhetesDetalhes = new TransferenciaBilhetesDetalhes(db);
            dalTransferenciaBilhetesDetalhes.UsuarioLogado = UsuarioLogado;
            dalTransferenciaBilhetesDetalhes.InserirRegistros(((Modelo.TransferenciaBilhetes)obj).TransferenciaBilhetesDetalhes, trans);
        }

        public List<Modelo.Proxy.PxyGridTransferenciaBilhetes> GetAllListGrid()
        {
            SqlParameter[] parms = new SqlParameter[0];
            string sql = @"
                            select tb.id,
	                               tb.codigo,
	                               CONVERT(VARCHAR(12),tb.DataInicio,103) DataInicio,
	                               CONVERT(VARCHAR(12),tb.DataFim,103) DataFim,
	                               CONCAT(fOrigem.codigo, ' | ', fOrigem.nome) Origem,
	                               CONCAT(Fdestino.codigo, ' | ', Fdestino.nome) Destino,
	                               tb.Incusuario,
	                               tb.Inchora,
	                               (Select COUNT(1) from TransferenciaBilhetesDetalhes tbd WHERE tb.Id = tbd.IdTransferenciaBilhetes) QtdBilhetesAfetados
                              from TransferenciaBilhetes tb
                             inner join funcionario fOrigem on tb.IdFuncionarioOrigem = fOrigem.id
                             inner join funcionario Fdestino on tb.IdFuncionarioDestino = Fdestino.id
            ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxyGridTransferenciaBilhetes> lista = new List<Modelo.Proxy.PxyGridTransferenciaBilhetes>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyGridTransferenciaBilhetes>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyGridTransferenciaBilhetes>>(dr);
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

        public void TransferirBilhetes(int idTransferenciaBilhetes)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idTransferenciaBilhetes", SqlDbType.Int)
            };
            parms[0].Value = idTransferenciaBilhetes;

            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.StoredProcedure, "SP_TranferirBilhetes", true, parms);
                    trans.Commit();
                    cmd.Dispose();
                }
            }
        }
    }
}
