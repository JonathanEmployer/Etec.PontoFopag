using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class MudCodigoFunc : DAL.FB.DALBase, DAL.IMudCodigoFunc
    {

        private MudCodigoFunc()
        {
            GEN = "GEN_mudcodigofunc_id";

            TABELA = "mudcodigofunc";

            SELECTPID = "SELECT * FROM \"mudcodigofunc\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT \"mudcodigofunc\".\"id\"" + 
		                            ",\"mudcodigofunc\".\"codigo\"" +
                                    ", \"mudcodigofunc\".\"datainicial\" as \"data\"" + 
		                            ", \"funcionario\".\"nome\"" + 
		                            ", \"mudcodigofunc\".\"dscodigoantigo\"" + 
		                            ", \"mudcodigofunc\".\"dscodigonovo\"" + 
                            "FROM \"mudcodigofunc\"" +
                            "INNER JOIN \"funcionario\"  ON \"funcionario\".\"id\" = \"mudcodigofunc\".\"idfuncionario\"";

            INSERT = "  INSERT INTO \"mudcodigofunc\"" +
                                        "(\"codigo\", \"datainicial\", \"idfuncionario\", \"dscodigoantigo\", \"dscodigonovo\", \"tipohorario\", \"idhorarionormal\", \"iddepartamento\", \"idempresa\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        " VALUES" +
                                        "(@codigo, @datainicial, @idfuncionario, @dscodigoantigo, @dscodigonovo, @tipohorario, @idhorarionormal, @iddepartamento, @idempresa, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"mudcodigofunc\" SET \"codigo\" = @codigo " +
                                        ", \"datainicial\" = @datainicial " +
                                        ", \"idfuncionario\" = @idfuncionario " +
                                        ", \"dscodigoantigo\" = @dscodigoantigo " +
                                        ", \"dscodigonovo\" = @dscodigonovo " +
                                        ", \"tipohorario\" = @tipohorario " +
                                        ", \"idhorarionormal\" = @idhorarionormal " +
                                        ", \"iddepartamento\" = @iddepartamento " +
                                        ", \"idempresa\" = @idempresa " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"mudcodigofunc\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"mudcodigofunc\"";

        }

        #region Singleton

        private static volatile FB.MudCodigoFunc _instancia = null;

        public static FB.MudCodigoFunc GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.MudCodigoFunc))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.MudCodigoFunc();
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
                obj = new Modelo.MudCodigoFunc();
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
            ((Modelo.MudCodigoFunc)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.MudCodigoFunc)obj).Datainicial = Convert.ToDateTime(dr["datainicial"]);
            ((Modelo.MudCodigoFunc)obj).IdFuncionario = Convert.ToInt32(dr["idfuncionario"]);
            ((Modelo.MudCodigoFunc)obj).DSCodigoAntigo = Convert.ToString(dr["dscodigoantigo"]);
            ((Modelo.MudCodigoFunc)obj).DSCodigoNovo = Convert.ToString(dr["dscodigonovo"]);
            ((Modelo.MudCodigoFunc)obj).Tipohorario = Convert.ToInt16(dr["tipohorario"]);
            ((Modelo.MudCodigoFunc)obj).Idhorarionormal = Convert.ToInt32(dr["idhorarionormal"]);
            ((Modelo.MudCodigoFunc)obj).Iddepartamento = Convert.ToInt32(dr["iddepartamento"]);
            ((Modelo.MudCodigoFunc)obj).Idempresa = Convert.ToInt32(dr["idempresa"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@datainicial", FbDbType.Date),
				new FbParameter ("@idfuncionario", FbDbType.Integer),
				new FbParameter ("@dscodigoantigo", FbDbType.VarChar),
				new FbParameter ("@dscodigonovo", FbDbType.VarChar),
				new FbParameter ("@tipohorario", FbDbType.SmallInt),
				new FbParameter ("@idhorarionormal", FbDbType.Integer),
				new FbParameter ("@iddepartamento", FbDbType.Integer),
				new FbParameter ("@idempresa", FbDbType.Integer),
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
            parms[1].Value = ((Modelo.MudCodigoFunc)obj).Codigo;
            parms[2].Value = ((Modelo.MudCodigoFunc)obj).Datainicial;
            parms[3].Value = ((Modelo.MudCodigoFunc)obj).IdFuncionario;
            parms[4].Value = ((Modelo.MudCodigoFunc)obj).DSCodigoAntigo;
            parms[5].Value = ((Modelo.MudCodigoFunc)obj).DSCodigoNovo;
            parms[6].Value = ((Modelo.MudCodigoFunc)obj).Tipohorario;
            parms[7].Value = ((Modelo.MudCodigoFunc)obj).Idhorarionormal;
            parms[8].Value = ((Modelo.MudCodigoFunc)obj).Iddepartamento;
            parms[9].Value = ((Modelo.MudCodigoFunc)obj).Idempresa;
            parms[10].Value = ((Modelo.MudCodigoFunc)obj).Incdata;
            parms[11].Value = ((Modelo.MudCodigoFunc)obj).Inchora;
            parms[12].Value = ((Modelo.MudCodigoFunc)obj).Incusuario;
            parms[13].Value = ((Modelo.MudCodigoFunc)obj).Altdata;
            parms[14].Value = ((Modelo.MudCodigoFunc)obj).Althora;
            parms[15].Value = ((Modelo.MudCodigoFunc)obj).Altusuario;
        }

        public Modelo.MudCodigoFunc LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.MudCodigoFunc objMudCodigoFunc = new Modelo.MudCodigoFunc();
            try
            {

                SetInstance(dr, objMudCodigoFunc);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objMudCodigoFunc;
        }

        public bool VerificaMarcacao(int pId, DateTime pData)
        {
            string aux;
            FbParameter[] parms = new FbParameter[]{
                    new FbParameter("@id", FbDbType.Integer, 4),
                    };

            parms[0].Value= pId;

            aux = "SELECT MAX (\"bilhetesimp\".\"data\") AS \"Data\" FROM \"bilhetesimp\" " +
                " WHERE \"bilhetesimp\".\"func\" IN (Select \"funcionario\".\"dscodigo\" FROM \"funcionario\" WHERE \"funcionario\".\"id\" = @id)" +
                " AND \"bilhetesimp\".\"importado\" = '1'";
         
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["Data"] is System.DBNull || pData > Convert.ToDateTime(dr["Data"]))
                        return true;

                }
                return false;
            }
            finally
            {
                dr.Close();
            }
        }

        public List<Modelo.MudCodigoFunc> GetMudancasPeriodo(DateTime datai, DateTime dataf)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@datai", FbDbType.Date),
                new FbParameter("@dataf", FbDbType.Date)
            };
            parms[0].Value = datai;
            parms[1].Value = dataf;

            List<Modelo.MudCodigoFunc> ret = new List<Modelo.MudCodigoFunc>();
            string cmd = "SELECT * FROM mudcodigofunc WHERE datainicial >= @datai AND datainicial <= @dataf";
            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd, parms);
            Modelo.MudCodigoFunc objMudCodigoFunc;
            while (dr.Read())
            {
                objMudCodigoFunc = new Modelo.MudCodigoFunc();
                AuxSetInstance(dr, objMudCodigoFunc);
                ret.Add(objMudCodigoFunc);
            }
            return ret;
        }

        #endregion


        public List<Modelo.MudCodigoFunc> GetAllList()
        {
            throw new NotImplementedException();
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
    }
}
