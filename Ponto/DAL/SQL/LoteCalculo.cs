using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class LoteCalculo : DAL.SQL.DALBase, DAL.ILoteCalculo
    {
        private LoteCalculoFuncionario _dalLoteCalculoFuncionario;
        public LoteCalculo(DataBase database)
        {
            db = database;
            _dalLoteCalculoFuncionario = new LoteCalculoFuncionario(db);
            TABELA = "LoteCalculo";

            SELECTPID = @"   SELECT * FROM LoteCalculo WHERE id = @id";

            SELECTALL = @"   SELECT   LoteCalculo.*
                             FROM LoteCalculo";

            INSERT = @"  INSERT INTO LoteCalculo
							(codigo, incdata, inchora, incusuario, DataInicio,DataFim)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @DataInicio,@DataFim)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE LoteCalculo SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,DataInicio = @DataInicio
                           ,DataFim = @DataFim

						WHERE id = @id";

            DELETE = @"  DELETE FROM LoteCalculo WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM LoteCalculo";            
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
                obj = new Modelo.LoteCalculo();
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
            ((Modelo.LoteCalculo)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.LoteCalculo)obj).DataInicio = Convert.ToDateTime(dr["DataInicio"]);
             ((Modelo.LoteCalculo)obj).DataFim = Convert.ToDateTime(dr["DataFim"]);

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
            parms[1].Value = ((Modelo.LoteCalculo)obj).Codigo;
            parms[2].Value = ((Modelo.LoteCalculo)obj).Incdata;
            parms[3].Value = ((Modelo.LoteCalculo)obj).Inchora;
            parms[4].Value = ((Modelo.LoteCalculo)obj).Incusuario;
            parms[5].Value = ((Modelo.LoteCalculo)obj).Altdata;
            parms[6].Value = ((Modelo.LoteCalculo)obj).Althora;
            parms[7].Value = ((Modelo.LoteCalculo)obj).Altusuario;
           parms[8].Value = ((Modelo.LoteCalculo)obj).DataInicio;
           parms[9].Value = ((Modelo.LoteCalculo)obj).DataFim;

        }

        protected override void IncluirAux(SqlTransaction trans, ModeloBase obj)
        {
            base.IncluirAux(trans, obj);
            _dalLoteCalculoFuncionario.UsuarioLogado = UsuarioLogado;
            ((Modelo.LoteCalculo)obj).LoteCalculoFuncionario.ForEach(f => { f.IdLoteCalculo = ((Modelo.LoteCalculo)obj).Id; });
            _dalLoteCalculoFuncionario.InserirRegistros(((Modelo.LoteCalculo)obj).LoteCalculoFuncionario.Where(W => W.Id == 0).ToList(), trans);
        }

        public Modelo.LoteCalculo LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.LoteCalculo obj = new Modelo.LoteCalculo();
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

        public List<Modelo.LoteCalculo> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.LoteCalculo> lista = new List<Modelo.LoteCalculo>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.LoteCalculo>();
                lista = AutoMapper.Mapper.Map<List<Modelo.LoteCalculo>>(dr);
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
