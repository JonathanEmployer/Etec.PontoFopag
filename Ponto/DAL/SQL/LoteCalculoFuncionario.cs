using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class LoteCalculoFuncionario : DAL.SQL.DALBase, DAL.ILoteCalculoFuncionario
    {

        public LoteCalculoFuncionario(DataBase database)
        {
            db = database;
            TABELA = "LoteCalculoFuncionario";

            SELECTPID = @"   SELECT * FROM LoteCalculoFuncionario WHERE id = @id";

            SELECTALL = @"   SELECT   LoteCalculoFuncionario.*
                             FROM LoteCalculoFuncionario";

            INSERT = @"  INSERT INTO LoteCalculoFuncionario
							(codigo,   incdata,  inchora,  incusuario,  IdLoteCalculo,  IdFuncionario, Erro, QtdTentativas, DtaProcessamento)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdLoteCalculo, @IdFuncionario, @Erro, @QtdTentativas, @DtaProcessamento)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE LoteCalculoFuncionario SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , IdLoteCalculo = @IdLoteCalculo
                            , IdFuncionario = @IdFuncionario
                            , Erro = @Erro
                            , QtdTentativas = @QtdTentativas 
                            , DtaProcessamento = @DtaProcessamento

						WHERE id = @id";

            DELETE = @"  DELETE FROM LoteCalculoFuncionario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM LoteCalculoFuncionario";

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
                obj = new Modelo.LoteCalculoFuncionario();
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
            ((Modelo.LoteCalculoFuncionario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.LoteCalculoFuncionario)obj).IdLoteCalculo = Convert.ToInt32(dr["IdLoteCalculo"]);
            ((Modelo.LoteCalculoFuncionario)obj).IdFuncionario = Convert.ToInt32(dr["IdFuncionario"]);
            ((Modelo.LoteCalculoFuncionario)obj).Erro = Convert.ToString(dr["Erro"]);
            ((Modelo.LoteCalculoFuncionario)obj).QtdTentativas = Convert.ToInt32(dr["QtdTentativas"]);
            ((Modelo.LoteCalculoFuncionario)obj).DtaProcessamento = (dr["DtaProcessamento"] is DBNull ? null : (DateTime?)(dr["DtaProcessamento"]));
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
                ,new SqlParameter ("@IdLoteCalculo", SqlDbType.Int)
                ,new SqlParameter ("@IdFuncionario", SqlDbType.Int)
                ,new SqlParameter ("@Erro", SqlDbType.VarChar)
                ,new SqlParameter ("@QtdTentativas", SqlDbType.Int)
                ,new SqlParameter ("@DtaProcessamento", SqlDbType.DateTime)
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
            parms[1].Value = ((Modelo.LoteCalculoFuncionario)obj).Codigo;
            parms[2].Value = ((Modelo.LoteCalculoFuncionario)obj).Incdata;
            parms[3].Value = ((Modelo.LoteCalculoFuncionario)obj).Inchora;
            parms[4].Value = ((Modelo.LoteCalculoFuncionario)obj).Incusuario;
            parms[5].Value = ((Modelo.LoteCalculoFuncionario)obj).Altdata;
            parms[6].Value = ((Modelo.LoteCalculoFuncionario)obj).Althora;
            parms[7].Value = ((Modelo.LoteCalculoFuncionario)obj).Altusuario;
            parms[8].Value = ((Modelo.LoteCalculoFuncionario)obj).IdLoteCalculo;
            parms[9].Value = ((Modelo.LoteCalculoFuncionario)obj).IdFuncionario;
            parms[10].Value = ((Modelo.LoteCalculoFuncionario)obj).Erro;
            parms[11].Value = ((Modelo.LoteCalculoFuncionario)obj).QtdTentativas;
            parms[12].Value = ((Modelo.LoteCalculoFuncionario)obj).DtaProcessamento;

        }

        public Modelo.LoteCalculoFuncionario LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LoteCalculoFuncionario obj = new Modelo.LoteCalculoFuncionario();
            try
            {

                SetInstance(dr, obj);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return obj;
        }

        public List<Modelo.LoteCalculoFuncionario> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.LoteCalculoFuncionario> lista = new List<Modelo.LoteCalculoFuncionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LoteCalculoFuncionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LoteCalculoFuncionario>>(dr);
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
