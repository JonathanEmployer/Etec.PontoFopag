using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class Eventos : DAL.FB.DALBase, DAL.IEventos
    {

        private Eventos()
        {
            GEN = "GEN_eventos_id";

            TABELA = "eventos";

            SELECTPID = "SELECT * FROM \"eventos\" WHERE \"id\" = @id";

            SELECTALL = "SELECT  \"eventos\".\"id\" "
                        + ", \"eventos\".\"descricao\" "
                        + ", \"eventos\".\"codigo\" "
                        + " FROM \"eventos\" ";

            INSERT = "  INSERT INTO \"eventos\"" +
                                        "(\"codigo\", \"descricao\", \"htd\", \"htn\", \"adicionalnoturno\", \"hed\", \"hen\", \"he50\", \"he60\", \"he70\", \"he80\", \"he90\", \"he100\", \"hesab\", \"hedom\", \"hefer\", \"folga\", \"tipofalta\", \"fd\", \"fn\", \"dsr\", \"tipohoras\", \"bh_cred\", \"bh_deb\", \"at_d\", \"at_n\", \"incdata\", \"inchora\", \"incusuario\",\"he50n\", \"he60n\", \"he70n\", \"he80n\", \"he90n\", \"he100n\", \"hesabn\", \"hedomn\", \"hefern\", \"folgan\", \"extranoturnabh\")" +
                                        "VALUES" +
                                        "(@codigo, @descricao, @htd, @htn, @adicionalnoturno, @hed, @hen, @he50, @he60, @he70, @he80, @he90, @he100, @hesab, @hedom, @hefer, @folga, @tipofalta, @fd, @fn, @dsr, @tipohoras, @bh_cred, @bh_deb, @at_d, @at_n, @incdata, @inchora, @incusuario, @he50n, @he60n, @he70n, @he80n, @he90n, @he100n, @hesabn, @hedomn, @hefern, @folgan, @extranoturnabh)";

            UPDATE = "  UPDATE \"eventos\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"htd\" = @htd " +
                                        ", \"htn\" = @htn " +
                                        ", \"adicionalnoturno\" = @adicionalnoturno " +
                                        ", \"hed\" = @hed " +
                                        ", \"hen\" = @hen " +
                                        ", \"he50\" = @he50 " +
                                        ", \"he60\" = @he60 " +
                                        ", \"he70\" = @he70 " +
                                        ", \"he80\" = @he80 " +
                                        ", \"he90\" = @he90 " +
                                        ", \"he100\" = @he100 " +
                                        ", \"hesab\" = @hesab " +
                                        ", \"hedom\" = @hedom " +
                                        ", \"hefer\" = @hefer " +
                                        ", \"folga\" = @folga " +
                                        ", \"tipofalta\" = @tipofalta " +
                                        ", \"fd\" = @fd " +
                                        ", \"fn\" = @fn " +
                                        ", \"dsr\" = @dsr " +
                                        ", \"tipohoras\" = @tipohoras " +
                                        ", \"bh_cred\" = @bh_cred " +
                                        ", \"bh_deb\" = @bh_deb " +
                                        ", \"at_d\" = @at_d " +
                                        ", \"at_n\" = @at_n " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"he50n\" = @he50n " +
                                        ", \"he60n\" = @he60n " +
                                        ", \"he70n\" = @he70n " +
                                        ", \"he80n\" = @he80n " +
                                        ", \"he90n\" = @he90n " +
                                        ", \"he100n\" = @he100n " +
                                        ", \"hesabn\" = @hesabn " +
                                        ", \"hedomn\" = @hedomn " +
                                        ", \"hefern\" = @hefern " +
                                        ", \"folgan\" = @folgan " +
                                        ", \"extranoturnabh\" = @extranoturnabh " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"eventos\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"eventos\"";

        }

        #region Singleton

        private static volatile FB.Eventos _instancia = null;

        public static FB.Eventos GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Eventos))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Eventos();
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
                obj = new Modelo.Eventos();
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
            ((Modelo.Eventos)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Eventos)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Eventos)obj).Htd = Convert.ToInt16(dr["htd"]);
            ((Modelo.Eventos)obj).Htn = Convert.ToInt16(dr["htn"]);
            ((Modelo.Eventos)obj).AdicionalNoturno = Convert.ToInt16(dr["adicionalnoturno"]);
            ((Modelo.Eventos)obj).Hed = Convert.ToInt16(dr["hed"]);
            ((Modelo.Eventos)obj).Hen = Convert.ToInt16(dr["hen"]);
            ((Modelo.Eventos)obj).He50 = Convert.ToInt16(dr["he50"]);
            ((Modelo.Eventos)obj).He60 = Convert.ToInt16(dr["he60"]);
            ((Modelo.Eventos)obj).He70 = Convert.ToInt16(dr["he70"]);
            ((Modelo.Eventos)obj).He80 = Convert.ToInt16(dr["he80"]);
            ((Modelo.Eventos)obj).He90 = Convert.ToInt16(dr["he90"]);
            ((Modelo.Eventos)obj).He100 = Convert.ToInt16(dr["he100"]);
            ((Modelo.Eventos)obj).Hesab = Convert.ToInt16(dr["hesab"]);
            ((Modelo.Eventos)obj).Hedom = Convert.ToInt16(dr["hedom"]);
            ((Modelo.Eventos)obj).Hefer = Convert.ToInt16(dr["hefer"]);
            ((Modelo.Eventos)obj).Folga = Convert.ToInt16(dr["folga"]);
            ((Modelo.Eventos)obj).TipoFalta = Convert.ToInt16(dr["tipofalta"]);
            ((Modelo.Eventos)obj).Fd = Convert.ToInt16(dr["fd"]);
            ((Modelo.Eventos)obj).Fn = Convert.ToInt16(dr["fn"]);
            ((Modelo.Eventos)obj).Dsr = Convert.ToInt16(dr["dsr"]);
            ((Modelo.Eventos)obj).Tipohoras = Convert.ToInt16(dr["tipohoras"]);
            ((Modelo.Eventos)obj).Bh_cred = Convert.ToInt16(dr["bh_cred"]);
            ((Modelo.Eventos)obj).Bh_deb = Convert.ToInt16(dr["bh_deb"]);
            ((Modelo.Eventos)obj).At_d = Convert.ToInt16(dr["at_d"]);
            ((Modelo.Eventos)obj).At_n = Convert.ToInt16(dr["at_n"]);
            ((Modelo.Eventos)obj).He50N = dr["he50n"] is DBNull ? (short)0 : Convert.ToInt16(dr["he50n"]);
            ((Modelo.Eventos)obj).He60N = dr["he60n"] is DBNull ? (short)0 : Convert.ToInt16(dr["he60n"]);
            ((Modelo.Eventos)obj).He70N = dr["he70n"] is DBNull ? (short)0 : Convert.ToInt16(dr["he70n"]);
            ((Modelo.Eventos)obj).He80N = dr["he80n"] is DBNull ? (short)0 : Convert.ToInt16(dr["he80n"]);
            ((Modelo.Eventos)obj).He90N = dr["he90n"] is DBNull ? (short)0 : Convert.ToInt16(dr["he90n"]);
            ((Modelo.Eventos)obj).He100N = dr["he100n"] is DBNull ? (short)0 : Convert.ToInt16(dr["he100n"]);
            ((Modelo.Eventos)obj).HesabN = dr["hesabn"] is DBNull ? (short)0 : Convert.ToInt16(dr["hesabn"]);
            ((Modelo.Eventos)obj).HedomN = dr["hedomn"] is DBNull ? (short)0 : Convert.ToInt16(dr["hedomn"]);
            ((Modelo.Eventos)obj).HeferN = dr["hefern"] is DBNull ? (short)0 : Convert.ToInt16(dr["hefern"]);
            ((Modelo.Eventos)obj).FolgaN = dr["folgan"] is DBNull ? (short)0 : Convert.ToInt16(dr["folgan"]);
            ((Modelo.Eventos)obj).Extranoturnabh = dr["extranoturnabh"] is DBNull ? (short)0 : Convert.ToInt16(dr["extranoturnabh"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
				new FbParameter ("@htd", FbDbType.SmallInt),
				new FbParameter ("@htn", FbDbType.SmallInt),
				new FbParameter ("@adicionalnoturno", FbDbType.SmallInt),
				new FbParameter ("@hed", FbDbType.SmallInt),
				new FbParameter ("@hen", FbDbType.SmallInt),
				new FbParameter ("@he50", FbDbType.SmallInt),
				new FbParameter ("@he60", FbDbType.SmallInt),
				new FbParameter ("@he70", FbDbType.SmallInt),
				new FbParameter ("@he80", FbDbType.SmallInt),
				new FbParameter ("@he90", FbDbType.SmallInt),
				new FbParameter ("@he100", FbDbType.SmallInt),
				new FbParameter ("@hesab", FbDbType.SmallInt),
				new FbParameter ("@hedom", FbDbType.SmallInt),
				new FbParameter ("@hefer", FbDbType.SmallInt),
				new FbParameter ("@folga", FbDbType.SmallInt),
				new FbParameter ("@tipofalta", FbDbType.SmallInt),
				new FbParameter ("@fd", FbDbType.SmallInt),
				new FbParameter ("@fn", FbDbType.SmallInt),
				new FbParameter ("@dsr", FbDbType.SmallInt),
				new FbParameter ("@tipohoras", FbDbType.SmallInt),
				new FbParameter ("@bh_cred", FbDbType.SmallInt),
				new FbParameter ("@bh_deb", FbDbType.SmallInt),
				new FbParameter ("@at_d", FbDbType.SmallInt),
				new FbParameter ("@at_n", FbDbType.SmallInt),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
                new FbParameter ("@he50n", FbDbType.SmallInt),
				new FbParameter ("@he60n", FbDbType.SmallInt),
				new FbParameter ("@he70n", FbDbType.SmallInt),
				new FbParameter ("@he80n", FbDbType.SmallInt),
				new FbParameter ("@he90n", FbDbType.SmallInt),
				new FbParameter ("@he100n", FbDbType.SmallInt),
                new FbParameter ("@hesabn", FbDbType.SmallInt),
				new FbParameter ("@hedomn", FbDbType.SmallInt),
				new FbParameter ("@hefern", FbDbType.SmallInt),
				new FbParameter ("@folgan", FbDbType.SmallInt),
                new FbParameter ("@extranoturnabh", FbDbType.SmallInt),
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
            parms[1].Value = ((Modelo.Eventos)obj).Codigo;
            parms[2].Value = ((Modelo.Eventos)obj).Descricao;
            parms[3].Value = ((Modelo.Eventos)obj).Htd;
            parms[4].Value = ((Modelo.Eventos)obj).Htn;
            parms[5].Value = ((Modelo.Eventos)obj).AdicionalNoturno;
            parms[6].Value = ((Modelo.Eventos)obj).Hed;
            parms[7].Value = ((Modelo.Eventos)obj).Hen;
            parms[8].Value = ((Modelo.Eventos)obj).He50;
            parms[9].Value = ((Modelo.Eventos)obj).He60;
            parms[10].Value = ((Modelo.Eventos)obj).He70;
            parms[11].Value = ((Modelo.Eventos)obj).He80;
            parms[12].Value = ((Modelo.Eventos)obj).He90;
            parms[13].Value = ((Modelo.Eventos)obj).He100;
            parms[14].Value = ((Modelo.Eventos)obj).Hesab;
            parms[15].Value = ((Modelo.Eventos)obj).Hedom;
            parms[16].Value = ((Modelo.Eventos)obj).Hefer;
            parms[17].Value = ((Modelo.Eventos)obj).Folga;
            parms[18].Value = ((Modelo.Eventos)obj).TipoFalta;
            parms[19].Value = ((Modelo.Eventos)obj).Fd;
            parms[20].Value = ((Modelo.Eventos)obj).Fn;
            parms[21].Value = ((Modelo.Eventos)obj).Dsr;
            parms[22].Value = ((Modelo.Eventos)obj).Tipohoras;
            parms[23].Value = ((Modelo.Eventos)obj).Bh_cred;
            parms[24].Value = ((Modelo.Eventos)obj).Bh_deb;
            parms[25].Value = ((Modelo.Eventos)obj).At_d;
            parms[26].Value = ((Modelo.Eventos)obj).At_n;
            parms[27].Value = ((Modelo.Eventos)obj).Incdata;
            parms[28].Value = ((Modelo.Eventos)obj).Inchora;
            parms[29].Value = ((Modelo.Eventos)obj).Incusuario;
            parms[30].Value = ((Modelo.Eventos)obj).Altdata;
            parms[31].Value = ((Modelo.Eventos)obj).Althora;
            parms[32].Value = ((Modelo.Eventos)obj).Altusuario;
            parms[33].Value = ((Modelo.Eventos)obj).He50N;
            parms[34].Value = ((Modelo.Eventos)obj).He60N;
            parms[35].Value = ((Modelo.Eventos)obj).He70N;
            parms[36].Value = ((Modelo.Eventos)obj).He80N;
            parms[37].Value = ((Modelo.Eventos)obj).He90N;
            parms[38].Value = ((Modelo.Eventos)obj).He100N;
            parms[39].Value = ((Modelo.Eventos)obj).HesabN;
            parms[40].Value = ((Modelo.Eventos)obj).HedomN;
            parms[41].Value = ((Modelo.Eventos)obj).HeferN;
            parms[42].Value = ((Modelo.Eventos)obj).FolgaN;
            parms[43].Value = ((Modelo.Eventos)obj).Extranoturnabh;
        }

        public Modelo.Eventos LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Eventos objEventos = new Modelo.Eventos();
            try
            {
                SetInstance(dr, objEventos);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEventos;
        }

        public List<Modelo.Eventos> GetAllList()
        {
            List<Modelo.Eventos> lista = new List<Modelo.Eventos>();
            FbParameter[] parms = new FbParameter[0];
            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"eventos\"", parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Eventos objEventos = new Modelo.Eventos();
                    AuxSetInstance(dr, objEventos);
                    lista.Add(objEventos);
                }
            }
            return lista;
        }
        #endregion

        public Modelo.Eventos LoadObjectByCodigo(int codigo)
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

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }
    }
}
