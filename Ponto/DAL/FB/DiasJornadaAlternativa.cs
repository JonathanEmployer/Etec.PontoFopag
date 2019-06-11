using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class DiasJornadaAlternativa : DAL.FB.DALBase, DAL.IDiasJornadaAlternativa
    {
        public string SELECTPJA { get; set; }

        private DiasJornadaAlternativa()
        {
            GEN = "GEN_diasjornadaalternativa_id";

            TABELA = "diasjornadaalternativa";

            SELECTPID = "SELECT * FROM \"diasjornadaalternativa\" WHERE \"id\" = @id";

            SELECTALL = "SELECT * FROM \"diasjornadaalternativa\"";

            SELECTPJA = "SELECT * FROM \"diasjornadaalternativa\" WHERE \"idjornadaalternativa\" = @idjornadaalternativa";

            INSERT = "  INSERT INTO \"diasjornadaalternativa\"" +
                                        "(\"codigo\", \"idjornadaalternativa\", \"datacompensada\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @idjornadaalternativa, @datacompensada, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"diasjornadaalternativa\" SET \"codigo\" = @codigo " +
                                        ", \"idjornadaalternativa\" = @idjornadaalternativa " +
                                        ", \"datacompensada\" = @datacompensada " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"diasjornadaalternativa\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"diasjornadaalternativa\"";

        }

        #region Singleton

        private static volatile FB.DiasJornadaAlternativa _instancia = null;

        public static FB.DiasJornadaAlternativa GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.DiasJornadaAlternativa))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.DiasJornadaAlternativa();
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
                    AtribuiDiasJA(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.DiasJornadaAlternativa();
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

        private void AtribuiDiasJA(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.DiasJornadaAlternativa)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.DiasJornadaAlternativa)obj).IdJornadaAlternativa = Convert.ToInt32(dr["idjornadaalternativa"]);
            ((Modelo.DiasJornadaAlternativa)obj).DataCompensada = Convert.ToDateTime(dr["datacompensada"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@idjornadaalternativa", FbDbType.Integer),
				new FbParameter ("@datacompensada", FbDbType.Date),
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
            parms[1].Value = ((Modelo.DiasJornadaAlternativa)obj).Codigo;
            parms[2].Value = ((Modelo.DiasJornadaAlternativa)obj).IdJornadaAlternativa;
            parms[3].Value = ((Modelo.DiasJornadaAlternativa)obj).DataCompensada;
            parms[4].Value = ((Modelo.DiasJornadaAlternativa)obj).Incdata;
            parms[5].Value = ((Modelo.DiasJornadaAlternativa)obj).Inchora;
            parms[6].Value = ((Modelo.DiasJornadaAlternativa)obj).Incusuario;
            parms[7].Value = ((Modelo.DiasJornadaAlternativa)obj).Altdata;
            parms[8].Value = ((Modelo.DiasJornadaAlternativa)obj).Althora;
            parms[9].Value = ((Modelo.DiasJornadaAlternativa)obj).Altusuario;
        }

        public Modelo.DiasJornadaAlternativa LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.DiasJornadaAlternativa objDiasJornadaAlternativa = new Modelo.DiasJornadaAlternativa();
            try
            {
                SetInstance(dr, objDiasJornadaAlternativa);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objDiasJornadaAlternativa;
        }

        public List<Modelo.DiasJornadaAlternativa> LoadPJornadaAlternativa(int IdJA)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idjornadaalternativa", FbDbType.Integer, 4) };
            parms[0].Value = IdJA;

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, SELECTPJA, parms);

            List<Modelo.DiasJornadaAlternativa> lista = new List<Modelo.DiasJornadaAlternativa>();
            //lista = null;

            Modelo.DiasJornadaAlternativa objDiasJA;
            while (dr.Read())
            {
                objDiasJA = new Modelo.DiasJornadaAlternativa();

                AtribuiDiasJA(dr, objDiasJA);

                objDiasJA.Acao = Modelo.Acao.Consultar;

                lista.Add(objDiasJA);
            }
            return lista;
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
