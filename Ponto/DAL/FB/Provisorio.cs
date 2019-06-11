using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class Provisorio : DAL.FB.DALBase, DAL.IProvisorio
    {

        private Provisorio()
        {
            GEN = "GEN_provisorio_id";

            TABELA = "provisorio";

            SELECTPID = "SELECT * FROM \"provisorio\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT   \"id\"" +
                        ", \"codigo\"" +
                        ", \"dsfuncionarionovo\"" +
                        ", \"dsfuncionario\"" +
                        ", \"dt_inicial\"" +
                        ", \"dt_final\"" +
                 " FROM \"provisorio\"";

            INSERT = "  INSERT INTO \"provisorio\"" +
                                        "(\"codigo\", \"dsfuncionario\", \"dsfuncionarionovo\", \"dt_inicial\", \"dt_final\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @dsfuncionario, @dsfuncionarionovo, @dt_inicial, @dt_final, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"provisorio\" SET \"codigo\" = @codigo " +
                                        ", \"dsfuncionario\" = @dsfuncionario " +
                                        ", \"dsfuncionarionovo\" = @dsfuncionarionovo " +
                                        ", \"dt_inicial\" = @dt_inicial " +
                                        ", \"dt_final\" = @dt_final " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"provisorio\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"provisorio\"";

        }

        #region Singleton

        private static volatile FB.Provisorio _instancia = null;

        public static FB.Provisorio GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Provisorio))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Provisorio();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        protected override bool SetInstance(FbDataReader dr, Modelo.ModeloBase obj)
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

        private void AuxSetInstance(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Provisorio)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Provisorio)obj).Dsfuncionario = Convert.ToString(dr["dsfuncionario"]);
            ((Modelo.Provisorio)obj).Dsfuncionarionovo = Convert.ToString(dr["dsfuncionarionovo"]);
            ((Modelo.Provisorio)obj).Dt_inicial = (dr["dt_inicial"] is DBNull ? null : (DateTime?)dr["dt_inicial"]);
            ((Modelo.Provisorio)obj).Dt_final = (dr["dt_final"] is DBNull ? null : (DateTime?)dr["dt_final"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@dsfuncionario", FbDbType.VarChar),
				new FbParameter ("@dsfuncionarionovo", FbDbType.VarChar),
				new FbParameter ("@dt_inicial", FbDbType.Date),
				new FbParameter ("@dt_final", FbDbType.Date),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar)
			};
            return parms;
        }

        protected override void SetParameters(FbParameter[] parms, Modelo.ModeloBase obj)
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
            FbDataReader dr = LoadDataReader(id);

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

            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@codigo", FbDbType.Integer),
                new FbParameter ("@data", FbDbType.Date)
            };
            parms[0].Value = pCodigo;
            parms[1].Value = pData;

            string aux = "SELECT * FROM \"provisorio\" WHERE \"dsfuncionario\" = @codigo AND @data >= \"dt_inicial\" AND @data <= \"dt_final\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Provisorio objProvisorio = new Modelo.Provisorio();
                    AuxSetInstance(dr, objProvisorio);

                    lista.Add(objProvisorio);
                }
            }
            dr.Close();

            return lista;
        }        

        public List<Modelo.Provisorio> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"provisorio\"", parms);

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
            }
            return lista;
        }

        public bool ExisteProvisorio(string pCodigo, DateTime pDataI, DateTime pDataF, int pIdProvisorio)
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@dscodigo", FbDbType.Integer),
                new FbParameter ("@datai", FbDbType.Date),
                new FbParameter ("@dataf", FbDbType.Date)
            };
            parms[0].Value = pCodigo;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string aux = "SELECT COUNT(\"id\") AS \"qtd\" FROM \"provisorio\" WHERE \"dsfuncionarionovo\" = @dscodigo"
                        + " AND ((@datai >= \"dt_inicial\" AND @datai <= \"dt_final\")"
                        + " OR (@dataf >= \"provisorio\".\"dt_inicial\" AND @dataf <= \"provisorio\".\"dt_final\")"
                        + " OR (@datai <= \"provisorio\".\"dt_inicial\" AND @dataf >= \"provisorio\".\"dt_final\"))";

            if (pIdProvisorio > 0)
            {
                aux += " AND \"provisorio\".\"id\" <> " + pIdProvisorio.ToString();
            }

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            int qtd = 0;
            if (dr.Read())
            {
                qtd = Convert.ToInt32(dr["qtd"]);
            }
            dr.Close();

            return (qtd > 0);
        }

        public bool VerificaBilhete(string pDSCodigo, DateTime pDatai, DateTime pDataf, out DateTime? ultimaData)
        {
            int qtd = 0;
            ultimaData = null;
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@dscodigo", FbDbType.VarChar, 50),
                new FbParameter ("@datai", FbDbType.Date),
                new FbParameter ("@dataf", FbDbType.Date)
             
            };
            parms[0].Value = pDSCodigo;
            parms[1].Value = pDatai;
            parms[2].Value = pDataf;

            string aux = "SELECT COUNT(\"id\") AS \"qtd\", MAX(\"data\") AS \"ultimadata\" FROM \"bilhetesimp\" WHERE \"func\" = @dscodigo AND \"data\" >= @datai AND \"data\" <= @dataf";

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.Read())
            {
                qtd = dr["qtd"] is DBNull ? 0 : Convert.ToInt32(dr["qtd"]);
                ultimaData = dr["ultimadata"] is DBNull ? null : (DateTime?)dr["ultimadata"];
                if (qtd > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public bool ExisteProvisorio(string pCodigo, DateTime pData)
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@dscodigo", FbDbType.Integer),
                new FbParameter ("@data", FbDbType.Date)
            };
            parms[0].Value = pCodigo;
            parms[1].Value = pData;

            string aux = "SELECT COUNT(\"id\") AS \"qtd\" FROM \"provisorio\" WHERE \"dsfuncionarionovo\" = @dscodigo"
                        + " AND ((@data >= \"dt_inicial\" AND @data <= \"dt_final\")"                        
                        + " OR (@data <= \"provisorio\".\"dt_inicial\"))";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            int qtd = 0;
            if (dr.Read())
            {
                qtd = Convert.ToInt32(dr["qtd"]);
            }
            dr.Close();

            return (qtd > 0);
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void InserirRegistros<T>(List<T> list, SqlTransaction trans)
        {
            throw new NotImplementedException();
        }

        public void AtualizarRegistros<T>(List<T> list, SqlTransaction trans, SqlConnection con)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
