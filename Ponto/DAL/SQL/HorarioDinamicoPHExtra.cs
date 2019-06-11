using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL.SQL
{
    public class HorarioDinamicoPHExtra : DAL.SQL.DALBase, DAL.IHorarioDinamicoPHExtra
    {

        public HorarioDinamicoPHExtra(DataBase database)
        {
            db = database;
            TABELA = "HorarioDinamicoPHExtra";

            SELECTPID = @"   SELECT * FROM HorarioDinamicoPHExtra WHERE id = @id";

            SELECTALL = @"   SELECT   HorarioDinamicoPHExtra.*
                             FROM HorarioDinamicoPHExtra";

            INSERT = @"  INSERT INTO HorarioDinamicoPHExtra
							(codigo, incdata, inchora, incusuario, IdHorarioDinamico,PercentualExtra,QuantidadeExtra,MarcaPercentualExtra,ConsideraPercExtraSemana,TipoAcumulo,PercentualExtraSegundo,PercentualExtraNoturna,QuantidadeExtraNoturna,PercentualExtraSegundoNoturna)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @IdHorarioDinamico,@PercentualExtra,@QuantidadeExtra,@MarcaPercentualExtra,@ConsideraPercExtraSemana,@TipoAcumulo,@PercentualExtraSegundo,@PercentualExtraNoturna,@QuantidadeExtraNoturna,@PercentualExtraSegundoNoturna)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @" UPDATE HorarioDinamicoPHExtra SET  
                                codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                           ,IdHorarioDinamico = @IdHorarioDinamico
                           ,PercentualExtra = @PercentualExtra
                           ,QuantidadeExtra = @QuantidadeExtra
                           ,MarcaPercentualExtra = @MarcaPercentualExtra
                           ,ConsideraPercExtraSemana = @ConsideraPercExtraSemana
                           ,TipoAcumulo = @TipoAcumulo
                           ,PercentualExtraSegundo = @PercentualExtraSegundo
                           ,PercentualExtraNoturna = @PercentualExtraNoturna
                           ,QuantidadeExtraNoturna = @QuantidadeExtraNoturna
                           ,PercentualExtraSegundoNoturna = @PercentualExtraSegundoNoturna

						WHERE id = @id";

            DELETE = @"  DELETE FROM HorarioDinamicoPHExtra WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM HorarioDinamicoPHExtra";

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
                obj = new Modelo.HorarioDinamicoPHExtra();
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
            ((Modelo.HorarioDinamicoPHExtra)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.HorarioDinamicoPHExtra)obj).IdHorarioDinamico = Convert.ToInt32(dr["IdHorarioDinamico"]);
            ((Modelo.HorarioDinamicoPHExtra)obj).PercentualExtra = Convert.ToDecimal(dr["percentualextra"]);
            ((Modelo.HorarioDinamicoPHExtra)obj).QuantidadeExtra = Convert.ToString(dr["quantidadeextra"]);
            ((Modelo.HorarioDinamicoPHExtra)obj).MarcaPercentualExtra = Convert.ToInt16(dr["marcapercentualextra"]);
            ((Modelo.HorarioDinamicoPHExtra)obj).ConsideraPercExtraSemana = Convert.ToInt16(dr["considerapercextrasemana"]);
            ((Modelo.HorarioDinamicoPHExtra)obj).TipoAcumulo = (dr["tipoacumulo"] is DBNull ? Convert.ToInt16(-1) : Convert.ToInt16(dr["tipoacumulo"]));
            ((Modelo.HorarioDinamicoPHExtra)obj).PercentualExtraSegundo = (dr["percentualextrasegundo"] is DBNull ? null : (Int16?)Convert.ToInt16(dr["percentualextrasegundo"]));
            if (!(dr["PercentualExtraNoturna"] is DBNull))
            {
                ((Modelo.HorarioDinamicoPHExtra)obj).PercentualExtraNoturna = Convert.ToDecimal(dr["PercentualExtraNoturna"]);
            }
            ((Modelo.HorarioDinamicoPHExtra)obj).QuantidadeExtraNoturna = Convert.ToString(dr["QuantidadeExtraNoturna"]);
            if (!(dr["percentualextrasegundoNoturna"] is DBNull))
            {
                ((Modelo.HorarioDinamicoPHExtra)obj).PercentualExtraSegundoNoturna = Convert.ToInt16(dr["percentualextrasegundoNoturna"]);
            }

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
                ,new SqlParameter ("@IdHorarioDinamico", SqlDbType.Int)
                ,new SqlParameter ("@PercentualExtra", SqlDbType.SmallInt)
                ,new SqlParameter ("@QuantidadeExtra", SqlDbType.VarChar)
                ,new SqlParameter ("@MarcaPercentualExtra", SqlDbType.Decimal)
                ,new SqlParameter ("@ConsideraPercExtraSemana", SqlDbType.Int)
                ,new SqlParameter ("@TipoAcumulo", SqlDbType.SmallInt)
                ,new SqlParameter ("@PercentualExtraSegundo", SqlDbType.SmallInt)
                ,new SqlParameter ("@PercentualExtraNoturna", SqlDbType.SmallInt)
                ,new SqlParameter ("@QuantidadeExtraNoturna", SqlDbType.VarChar)
                ,new SqlParameter ("@PercentualExtraSegundoNoturna", SqlDbType.SmallInt)

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
            parms[1].Value = ((Modelo.HorarioDinamicoPHExtra)obj).Codigo;
            parms[2].Value = ((Modelo.HorarioDinamicoPHExtra)obj).Incdata;
            parms[3].Value = ((Modelo.HorarioDinamicoPHExtra)obj).Inchora;
            parms[4].Value = ((Modelo.HorarioDinamicoPHExtra)obj).Incusuario;
            parms[5].Value = ((Modelo.HorarioDinamicoPHExtra)obj).Altdata;
            parms[6].Value = ((Modelo.HorarioDinamicoPHExtra)obj).Althora;
            parms[7].Value = ((Modelo.HorarioDinamicoPHExtra)obj).Altusuario;
           parms[8].Value = ((Modelo.HorarioDinamicoPHExtra)obj).IdHorarioDinamico;
           parms[9].Value = ((Modelo.HorarioDinamicoPHExtra)obj).PercentualExtra;
           parms[10].Value = ((Modelo.HorarioDinamicoPHExtra)obj).QuantidadeExtra;
           parms[11].Value = ((Modelo.HorarioDinamicoPHExtra)obj).MarcaPercentualExtra;
           parms[12].Value = ((Modelo.HorarioDinamicoPHExtra)obj).ConsideraPercExtraSemana;
           parms[13].Value = ((Modelo.HorarioDinamicoPHExtra)obj).TipoAcumulo;
           parms[14].Value = ((Modelo.HorarioDinamicoPHExtra)obj).PercentualExtraSegundo;
           parms[15].Value = ((Modelo.HorarioDinamicoPHExtra)obj).PercentualExtraNoturna;
           parms[16].Value = ((Modelo.HorarioDinamicoPHExtra)obj).QuantidadeExtraNoturna;
           parms[17].Value = ((Modelo.HorarioDinamicoPHExtra)obj).PercentualExtraSegundoNoturna;

        }

        public Modelo.HorarioDinamicoPHExtra LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.HorarioDinamicoPHExtra obj = new Modelo.HorarioDinamicoPHExtra();
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

        public List<Modelo.HorarioDinamicoPHExtra> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.HorarioDinamicoPHExtra> lista = new List<Modelo.HorarioDinamicoPHExtra>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioDinamicoPHExtra objHorarioPHExtra = new Modelo.HorarioDinamicoPHExtra();
                    AuxSetInstance(dr, objHorarioPHExtra);
                    lista.Add(objHorarioPHExtra);
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


        public List<Modelo.HorarioDinamicoPHExtra> LoadObjectByHorarioDinamico(int idHorarioDinamico)
        {
            return LoadObjectByHorarioDinamico(new List<int>() { idHorarioDinamico });
        }

        public List<Modelo.HorarioDinamicoPHExtra> LoadObjectByHorarioDinamico(List<int> idsHorarioDinamico)
        {
            SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@idHorarioDinamico", SqlDbType.VarChar)
                };
            parms[0].Value = String.Join(",", idsHorarioDinamico);

            string sql = @"select * from HorarioDinamicoPHExtra where idhorariodinamico in  (SELECT * FROM dbo.f_clausulaIn(@idHorarioDinamico))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.HorarioDinamicoPHExtra> lista = new List<Modelo.HorarioDinamicoPHExtra>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioDinamicoPHExtra objHorarioPHExtra = new Modelo.HorarioDinamicoPHExtra();
                    AuxSetInstance(dr, objHorarioPHExtra);
                    lista.Add(objHorarioPHExtra);
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
    }
}
