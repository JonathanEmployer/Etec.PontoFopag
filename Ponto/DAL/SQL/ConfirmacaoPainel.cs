using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.SQL
{
    public class ConfirmacaoPainel : DAL.SQL.DALBase, DAL.IConfirmacaoPainel
    {

        public ConfirmacaoPainel(DataBase database)
        {
            db = database;
            TABELA = "ConfirmacaoPainel";

            SELECTPID = @"   SELECT * FROM ConfirmacaoPainel WHERE id = @id";

            SELECTALL = @"   SELECT   ConfirmacaoPainel.id
                                    , ConfirmacaoPainel.codigo
                                    , ConfirmacaoPainel.Mes
                                    , ConfirmacaoPainel.Ano
                                    , ConfirmacaoPainel.idFuncionario
                             FROM ConfirmacaoPainel";

            INSERT = @"  INSERT INTO ConfirmacaoPainel
							(codigo, incdata, inchora, incusuario, Mes, Ano, idFuncionario)
							VALUES
							(@codigo, @incdata, @inchora, @incusuario, @Mes, @Ano, @idFuncionario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE ConfirmacaoPainel SET
							  codigo = @codigo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , Mes = @Mes
                            , Ano = @Ano
                            , idFuncionario = @idFuncionario
						WHERE id = @id";

            DELETE = @"  DELETE FROM ConfirmacaoPainel WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM ConfirmacaoPainel";

        }

        public string SqlLoadByCodigo()
        {
            return @"   SELECT * FROM ConfirmacaoPainel WHERE codigo = @codigo";
        }

        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);
                    ((Modelo.ConfirmacaoPainel)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.ConfirmacaoPainel)obj).Mes = Convert.ToInt32(dr["Mes"]);
                    ((Modelo.ConfirmacaoPainel)obj).Ano = Convert.ToInt32(dr["Ano"]);
                    ((Modelo.ConfirmacaoPainel)obj).idFuncionario = Convert.ToInt32(dr["idFuncionario"]);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.ConfirmacaoPainel();
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

        private void AtribuiConfirmacaoPainel(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.ConfirmacaoPainel)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.ConfirmacaoPainel)obj).Mes = Convert.ToInt32(dr["Mes"]);
            ((Modelo.ConfirmacaoPainel)obj).Ano = Convert.ToInt32(dr["Ano"]);
            ((Modelo.ConfirmacaoPainel)obj).idFuncionario = Convert.ToInt32(dr["idFuncionario"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@Mes", SqlDbType.Int),
                new SqlParameter ("@Ano", SqlDbType.Int),
                new SqlParameter ("@idFuncionario", SqlDbType.Int),
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
            parms[1].Value = ((Modelo.ConfirmacaoPainel)obj).Codigo;
            parms[2].Value = ((Modelo.ConfirmacaoPainel)obj).Incdata;
            parms[3].Value = ((Modelo.ConfirmacaoPainel)obj).Inchora;
            parms[4].Value = ((Modelo.ConfirmacaoPainel)obj).Incusuario;
            parms[5].Value = ((Modelo.ConfirmacaoPainel)obj).Altdata;
            parms[6].Value = ((Modelo.ConfirmacaoPainel)obj).Althora;
            parms[7].Value = ((Modelo.ConfirmacaoPainel)obj).Altusuario;
            parms[8].Value = ((Modelo.ConfirmacaoPainel)obj).Mes;
            parms[9].Value = ((Modelo.ConfirmacaoPainel)obj).Ano;
            parms[10].Value = ((Modelo.ConfirmacaoPainel)obj).idFuncionario;
        }

        public Modelo.ConfirmacaoPainel LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.ConfirmacaoPainel objConfirmacaoPainel = new Modelo.ConfirmacaoPainel();
            try
            {

                SetInstance(dr, objConfirmacaoPainel);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objConfirmacaoPainel;
        }

    
        public List<Modelo.ConfirmacaoPainel> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            string cmd = " SELECT * " +
                            " FROM ConfirmacaoPainel";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, parms);

            List<Modelo.ConfirmacaoPainel> lista = new List<Modelo.ConfirmacaoPainel>();
            try
            {
                while (dr.Read())
                {
                    Modelo.ConfirmacaoPainel objConfirmacaoPainel = new Modelo.ConfirmacaoPainel();
                    AtribuiConfirmacaoPainel(dr, objConfirmacaoPainel);
                    lista.Add(objConfirmacaoPainel);
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

        #endregion

        public Modelo.ConfirmacaoPainel LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SqlLoadByCodigo();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.ConfirmacaoPainel objConfirmacaoPainel = new Modelo.ConfirmacaoPainel();
            SetInstance(dr, objConfirmacaoPainel);
            return objConfirmacaoPainel;
        }

        public Modelo.ConfirmacaoPainel GetPorMesAnoIdFunc(int Mes, int Ano, int idFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@Mes", SqlDbType.Int),
                new SqlParameter("@Ano", SqlDbType.Int),
                new SqlParameter("@idFuncionario", SqlDbType.Int)

            };
            parms[0].Value = Mes;
            parms[1].Value = Ano;
            parms[2].Value = idFuncionario;

            string sql = @"select * from ConfirmacaoPainel where Mes = @Mes and Ano = @Ano and idFuncionario = @idFuncionario";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            IList<Modelo.ConfirmacaoPainel> lista = new List<Modelo.ConfirmacaoPainel>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.ConfirmacaoPainel>();
                lista = AutoMapper.Mapper.Map<List<Modelo.ConfirmacaoPainel>>(dr);
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
            return lista.FirstOrDefault();
        }
    }
}
