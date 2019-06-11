using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class Classificacao : DAL.SQL.DALBase, DAL.IClassificacao
    {

        public Classificacao(DataBase database)
        {
            db = database;
            TABELA = "Classificacao";

            SELECTPID = @"   SELECT * FROM Classificacao WHERE id = @id";

            SELECTALL = @"   SELECT   Classificacao.*
                             FROM Classificacao";

            INSERT = @"  INSERT INTO Classificacao
							(codigo, incdata, inchora, incusuario, Descricao, ExibePaineldoRH)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @Descricao, @ExibePaineldoRH)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE Classificacao SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,Descricao = @Descricao
                           ,ExibePaineldoRH = @ExibePaineldoRH

						WHERE id = @id";

            DELETE = @"  DELETE FROM Classificacao WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM Classificacao";

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
                obj = new Modelo.Classificacao();
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
            ((Modelo.Classificacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
             ((Modelo.Classificacao)obj).Descricao = Convert.ToString(dr["Descricao"]);
             ((Modelo.Classificacao)obj).ExibePaineldoRH = Convert.ToBoolean(dr["ExibePaineldoRH"]);

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
                ,new SqlParameter ("@Descricao", SqlDbType.VarChar)
                ,new SqlParameter ("@ExibePaineldoRH", SqlDbType.Bit)

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
            parms[1].Value = ((Modelo.Classificacao)obj).Codigo;
            parms[2].Value = ((Modelo.Classificacao)obj).Incdata;
            parms[3].Value = ((Modelo.Classificacao)obj).Inchora;
            parms[4].Value = ((Modelo.Classificacao)obj).Incusuario;
            parms[5].Value = ((Modelo.Classificacao)obj).Altdata;
            parms[6].Value = ((Modelo.Classificacao)obj).Althora;
            parms[7].Value = ((Modelo.Classificacao)obj).Altusuario;
            parms[8].Value = ((Modelo.Classificacao)obj).Descricao;
            parms[9].Value = ((Modelo.Classificacao)obj).ExibePaineldoRH;



        }

        public Modelo.Classificacao LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Classificacao obj = new Modelo.Classificacao();
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

        public List<Modelo.Classificacao> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.Classificacao> lista = new List<Modelo.Classificacao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Classificacao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Classificacao>>(dr);
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

        public List<Modelo.Classificacao> GetAllPorExibePaineldoRH()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM Classificacao where ExibePaineldoRH = 1", parms);

            List<Modelo.Classificacao> lista = new List<Modelo.Classificacao>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Classificacao>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Classificacao>>(dr);
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

        public int? GetIdPorCod(int cod)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            int? Id = Convert.ToInt32(db.ExecuteScalar(CommandType.Text, "select top 1 id from Classificacao where codigo = " + cod, parms));

            return Id;
        }
    }
}
