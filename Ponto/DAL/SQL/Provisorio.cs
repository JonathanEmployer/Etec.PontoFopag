using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DAL.SQL
{
    public class Provisorio : DAL.SQL.DALBase, DAL.IProvisorio
    {

        public Provisorio(DataBase database)
        {
            db = database;
            TABELA = "provisorio";

            SELECTPID = @"   SELECT p.*, 
                                    f.dscodigo + ' | ' + f.nome AS NomeFuncionario 
                             FROM provisorio p 
                             JOIN funcionario f on p.dsfuncionario = f.dscodigo
                             WHERE p.id = @id";

            SELECTALL = @"   SELECT   p.*,
                                      f.dscodigo + ' | ' + f.nome AS NomeFuncionario  
                             FROM provisorio p
                             JOIN funcionario f on p.dsfuncionario = f.dscodigo";

            INSERT = @"  INSERT INTO provisorio
							(codigo, dsfuncionario, dsfuncionarionovo, dt_inicial, dt_final, incdata, inchora, incusuario)
							VALUES
							(@codigo, @dsfuncionario, @dsfuncionarionovo, @dt_inicial, @dt_final, @incdata, @inchora, @incusuario) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE provisorio SET codigo = @codigo
							, dsfuncionario = @dsfuncionario
							, dsfuncionarionovo = @dsfuncionarionovo
							, dt_inicial = @dt_inicial
							, dt_final = @dt_final
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
						WHERE id = @id";

            DELETE = @"  DELETE FROM provisorio WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM provisorio";

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
                obj = new Modelo.Provisorio();
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
            ((Modelo.Provisorio)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Provisorio)obj).Dsfuncionario = Convert.ToString(dr["dsfuncionario"]);
            ((Modelo.Provisorio)obj).Dsfuncionarionovo = Convert.ToString(dr["dsfuncionarionovo"]);
            ((Modelo.Provisorio)obj).Dt_inicial = (dr["dt_inicial"] is DBNull ? null : (DateTime?)dr["dt_inicial"]);
            ((Modelo.Provisorio)obj).Dt_final = (dr["dt_final"] is DBNull ? null : (DateTime?)dr["dt_final"]);
            ((Modelo.Provisorio)obj).NomeFuncionario = Convert.ToString(dr["NomeFuncionario"]);
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@dsfuncionario", SqlDbType.VarChar),
				new SqlParameter ("@dsfuncionarionovo", SqlDbType.VarChar),
				new SqlParameter ("@dt_inicial", SqlDbType.DateTime),
				new SqlParameter ("@dt_final", SqlDbType.DateTime),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.Provisorio)obj).Codigo;
            parms[2].Value = ((Modelo.Provisorio)obj).Dsfuncionario;
            parms[3].Value = ((Modelo.Provisorio)obj).Dsfuncionarionovo;
            parms[4].Value = ((Modelo.Provisorio)obj).Dt_inicial;
            parms[5].Value = ((Modelo.Provisorio)obj).Dt_final;
            parms[6].Value = ((Modelo.Provisorio)obj).Incdata;
            parms[7].Value = ((Modelo.Provisorio)obj).Inchora;
            parms[8].Value = ((Modelo.Provisorio)obj).Incusuario;
            parms[9].Value = ((Modelo.Provisorio)obj).Altdata;
            parms[10].Value = ((Modelo.Provisorio)obj).Althora;
            parms[11].Value = ((Modelo.Provisorio)obj).Altusuario;
        }

        public Modelo.Provisorio LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

            Modelo.Provisorio objProvisorio = new Modelo.Provisorio();
            try
            {

                SetInstance(dr, objProvisorio);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objProvisorio;
        }

        public List<Modelo.Provisorio> getLista(string pCodigo, DateTime pData)
        {
            List<Modelo.Provisorio> lista = new List<Modelo.Provisorio>();

            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@data", SqlDbType.DateTime)
            };
            parms[0].Value = pCodigo;
            parms[1].Value = pData;

            string aux = "SELECT * FROM provisorio WHERE dsfuncionario = @codigo AND @data >= dt_inicial AND @data <= dt_final";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Provisorio objProvisorio = new Modelo.Provisorio();
                    AuxSetInstance(dr, objProvisorio);

                    lista.Add(objProvisorio);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }       

        public List<Modelo.Provisorio> GetAllList()
        {
            SqlParameter[] parms = new SqlParameter[0];

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALL, parms);

            List<Modelo.Provisorio> lista = new List<Modelo.Provisorio>();
            try
            {
                while (dr.Read())
                {
                    Modelo.Provisorio objProvisorio = new Modelo.Provisorio();
                    AuxSetInstance(dr, objProvisorio);
                    lista.Add(objProvisorio);
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

        public bool ExisteProvisorio(string pCodigo, DateTime pDataI, DateTime pDataF, int pIdProvisorio)
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@dscodigo", SqlDbType.Int),
                new SqlParameter ("@datai", SqlDbType.DateTime),
                new SqlParameter ("@dataf", SqlDbType.DateTime)
            };
            parms[0].Value = pCodigo;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string aux = "SELECT COUNT(id) AS qtd FROM provisorio WHERE dsfuncionarionovo = @dscodigo"
                        + " AND ((@datai >= dt_inicial AND @datai <= dt_final)"
                        + " OR (@dataf >= provisorio.dt_inicial AND @dataf <= provisorio.dt_final)"
                        + " OR (@datai <= provisorio.dt_inicial AND @dataf >= provisorio.dt_final))";

            if (pIdProvisorio > 0)
            {
                aux += " AND provisorio.id <> " + pIdProvisorio.ToString();
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            int qtd = 0;
            if (dr.Read())
            {
                qtd = Convert.ToInt32(dr["qtd"]);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return (qtd > 0);
        }

        public bool VerificaBilhete(string pDSCodigo, DateTime pDatai, DateTime pDataf, out DateTime? ultimaData)
        {
            int qtd = 0;
            ultimaData = null;
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@dscodigo", SqlDbType.VarChar, 50),
                new SqlParameter ("@datai", SqlDbType.DateTime),
                new SqlParameter ("@dataf", SqlDbType.DateTime)
             
            };
            parms[0].Value = pDSCodigo;
            parms[1].Value = pDatai;
            parms[2].Value = pDataf;

            string aux = "SELECT COUNT(id) AS qtd, MAX(data) AS ultimadata FROM bilhetesimp WHERE func = @dscodigo AND data >= @datai AND data <= @dataf";           

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            bool ret = false;
            if (dr.Read())
            {
                qtd = dr["qtd"] is DBNull ? 0 : Convert.ToInt32(dr["qtd"]);
                ultimaData = dr["ultimadata"] is DBNull ? null : (DateTime?)dr["ultimadata"];
                if (qtd > 0)
                {
                    ret = true;
                }
                else
                {
                    ret = false;
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public bool ExisteProvisorio(string pCodigo, DateTime pData)
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@dscodigo", SqlDbType.VarChar),
                new SqlParameter ("@data", SqlDbType.DateTime)
            };
            parms[0].Value = pCodigo;
            parms[1].Value = pData;

            string aux = "SELECT COUNT(id) AS qtd FROM provisorio WHERE dsfuncionarionovo = @dscodigo"
                        + " AND ((@data >= dt_inicial AND @data <= dt_final)"
                        + " OR (@data <= provisorio.dt_inicial))";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            int qtd = 0;
            if (dr.Read())
            {
                qtd = Convert.ToInt32(dr["qtd"]);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return (qtd > 0);
        }

        #endregion
    }
}
