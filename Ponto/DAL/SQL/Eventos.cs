using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

namespace DAL.SQL
{
    public class Eventos : DAL.SQL.DALBase, DAL.IEventos
    {
        public Eventos(DataBase database)
        {
            db = database;
            TABELA = "eventos";

            SELECTPID = @"   SELECT * FROM eventos WHERE id = @id";

            SELECTALL = @"   SELECT   eventos.id
                                    , eventos.descricao
                                    , eventos.codigo
                             FROM eventos";

            INSERT = @"  INSERT INTO eventos
							(codigo, descricao, htd, htn, adicionalnoturno, hed, hen, he50, he60, he70, he80, he90, he100, hesab, hedom, hefer, folga, tipofalta, fd, fn, dsr, tipohoras, bh_cred, bh_deb, at_d, at_n, incdata, inchora, incusuario, he50n, he60n, he70n, he80n, he90n, he100n, hesabn, hedomn, hefern, folgan, extranoturnabh
                             , percentualextra1, percentualextra2, percentualextra3, percentualextra4, percentualextra5, percentualextra6, percentualextra7, percentualextra8, percentualextra9, percentualextra10, horasabonadas, ocorrenciasselecionadas, idsocorrencias, HoristaMensalista, ClassificarHorasExtras, PercInItinere1, PercInItinere2, PercInItinere3, PercInItinere4, PercInItinere5, PercInItinere6, CodigoComplemento, InterjornadaExtra)
							VALUES
							(@codigo, @descricao, @htd, @htn, @adicionalnoturno, @hed, @hen, @he50, @he60, @he70, @he80, @he90, @he100, @hesab, @hedom, @hefer, @folga, @tipofalta, @fd, @fn, @dsr, @tipohoras, @bh_cred, @bh_deb, @at_d, @at_n, @incdata, @inchora, @incusuario, @he50n, @he60n, @he70n, @he80n, @he90n, @he100n, @hesabn, @hedomn, @hefern, @folgan, @extranoturnabh
                            , @percentualextra1, @percentualextra2, @percentualextra3, @percentualextra4, @percentualextra5, @percentualextra6, @percentualextra7, @percentualextra8, @percentualextra9, @percentualextra10, @horasabonadas, @ocorrenciasselecionadas, @idsocorrencias, @HoristaMensalista, @ClassificarHorasExtras, @PercInItinere1, @PercInItinere2, @PercInItinere3, @PercInItinere4, @PercInItinere5, @PercInItinere6, @CodigoComplemento, @InterjornadaExtra)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE eventos SET
							  codigo = @codigo
							, descricao = @descricao
							, htd = @htd
							, htn = @htn
							, adicionalnoturno = @adicionalnoturno
							, hed = @hed
							, hen = @hen
							, he50 = @he50
							, he60 = @he60
							, he70 = @he70
							, he80 = @he80
							, he90 = @he90
							, he100 = @he100
							, hesab = @hesab
							, hedom = @hedom
							, hefer = @hefer
							, folga = @folga
							, tipofalta = @tipofalta
							, fd = @fd
							, fn = @fn
							, dsr = @dsr
							, tipohoras = @tipohoras
							, bh_cred = @bh_cred
							, bh_deb = @bh_deb
							, at_d = @at_d
							, at_n = @at_n
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , he50n = @he50n
                            , he60n = @he60n
                            , he70n = @he70n
                            , he80n = @he80n 
                            , he90n = @he90n 
                            , he100n = @he100n
							, hesabn = @hesabn
							, hedomn = @hedomn
							, hefern = @hefern
							, folgan = @folgan
                            , percentualextra1 = @percentualextra1
                            , percentualextra2 = @percentualextra2
                            , percentualextra3 = @percentualextra3 
                            , percentualextra4 = @percentualextra4 
                            , percentualextra5 = @percentualextra5 
                            , percentualextra6 = @percentualextra6 
                            , percentualextra7 = @percentualextra7 
                            , percentualextra8 = @percentualextra8 
                            , percentualextra9 = @percentualextra9 
                            , percentualextra10 = @percentualextra10
                            , horasabonadas = @horasabonadas
                            , ocorrenciasselecionadas = @ocorrenciasselecionadas
                            , idsocorrencias = @idsocorrencias
                            , HoristaMensalista = @HoristaMensalista
                            , ClassificarHorasExtras = @ClassificarHorasExtras
                            , PercInItinere1 = @PercInItinere1
                            , PercInItinere2 = @PercInItinere2
                            , PercInItinere3 = @PercInItinere3
                            , PercInItinere4 = @PercInItinere4
                            , PercInItinere5 = @PercInItinere5
                            , PercInItinere6 = @PercInItinere6
                            , CodigoComplemento = @CodigoComplemento
                            , InterjornadaExtra = @InterjornadaExtra
						WHERE id = @id";

            DELETE = @"  DELETE FROM eventos WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM eventos";            
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

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
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
            ((Modelo.Eventos)obj).PercentualExtra1 = Convert.ToDecimal(dr["percentualextra1"]);
            ((Modelo.Eventos)obj).PercentualExtra2 = Convert.ToDecimal(dr["percentualextra2"]);
            ((Modelo.Eventos)obj).PercentualExtra3 = Convert.ToDecimal(dr["percentualextra3"]);
            ((Modelo.Eventos)obj).PercentualExtra4 = Convert.ToDecimal(dr["percentualextra4"]);
            ((Modelo.Eventos)obj).PercentualExtra5 = Convert.ToDecimal(dr["percentualextra5"]);
            ((Modelo.Eventos)obj).PercentualExtra6 = Convert.ToDecimal(dr["percentualextra6"]);
            ((Modelo.Eventos)obj).PercentualExtra7 = Convert.ToDecimal(dr["percentualextra7"]);
            ((Modelo.Eventos)obj).PercentualExtra8 = Convert.ToDecimal(dr["percentualextra8"]);
            ((Modelo.Eventos)obj).PercentualExtra9 = Convert.ToDecimal(dr["percentualextra9"]);
            ((Modelo.Eventos)obj).PercentualExtra10 = Convert.ToDecimal(dr["percentualextra10"]);
            ((Modelo.Eventos)obj).HorasAbonadas = Convert.ToInt16(dr["horasabonadas"]);
            ((Modelo.Eventos)obj).OcorrenciasSelecionadas = Convert.ToInt16(dr["ocorrenciasselecionadas"]);
            ((Modelo.Eventos)obj).IdsOcorrencias = Convert.ToString(dr["idsocorrencias"]);
            ((Modelo.Eventos)obj).HoristaMensalista = Convert.ToInt16(dr["HoristaMensalista"]);
            ((Modelo.Eventos)obj).ClassificarHorasExtras = Convert.ToBoolean(dr["ClassificarHorasExtras"]);
            ((Modelo.Eventos)obj).PercInItinere1 = dr["PercInItinere1"] is DBNull ? (short?)null : Convert.ToInt16(dr["PercInItinere1"]);
            ((Modelo.Eventos)obj).PercInItinere2 = dr["PercInItinere2"] is DBNull ? (short?)null : Convert.ToInt16(dr["PercInItinere2"]);
            ((Modelo.Eventos)obj).PercInItinere3 = dr["PercInItinere3"] is DBNull ? (short?)null : Convert.ToInt16(dr["PercInItinere3"]);
            ((Modelo.Eventos)obj).PercInItinere4 = dr["PercInItinere4"] is DBNull ? (short?)null : Convert.ToInt16(dr["PercInItinere4"]);
            ((Modelo.Eventos)obj).PercInItinere5 = dr["PercInItinere5"] is DBNull ? (short?)null : Convert.ToInt16(dr["PercInItinere5"]);
            ((Modelo.Eventos)obj).PercInItinere6 = dr["PercInItinere6"] is DBNull ? (short?)null : Convert.ToInt16(dr["PercInItinere6"]);
            ((Modelo.Eventos)obj).CodigoComplemento = dr["CodigoComplemento"] is DBNull ? (Int32?)null : Convert.ToInt32(dr["CodigoComplemento"]);
            ((Modelo.Eventos)obj).InterjornadaExtra = Convert.ToBoolean(dr["InterjornadaExtra"]);

        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@descricao", SqlDbType.VarChar),
				new SqlParameter ("@htd", SqlDbType.SmallInt),
				new SqlParameter ("@htn", SqlDbType.SmallInt),
				new SqlParameter ("@adicionalnoturno", SqlDbType.SmallInt),
				new SqlParameter ("@hed", SqlDbType.SmallInt),
				new SqlParameter ("@hen", SqlDbType.SmallInt),
				new SqlParameter ("@he50", SqlDbType.SmallInt),
				new SqlParameter ("@he60", SqlDbType.SmallInt),
				new SqlParameter ("@he70", SqlDbType.SmallInt),
				new SqlParameter ("@he80", SqlDbType.SmallInt),
				new SqlParameter ("@he90", SqlDbType.SmallInt),
				new SqlParameter ("@he100", SqlDbType.SmallInt),
				new SqlParameter ("@hesab", SqlDbType.SmallInt),
				new SqlParameter ("@hedom", SqlDbType.SmallInt),
				new SqlParameter ("@hefer", SqlDbType.SmallInt),
				new SqlParameter ("@folga", SqlDbType.SmallInt),
				new SqlParameter ("@tipofalta", SqlDbType.SmallInt),
				new SqlParameter ("@fd", SqlDbType.SmallInt),
				new SqlParameter ("@fn", SqlDbType.SmallInt),
				new SqlParameter ("@dsr", SqlDbType.SmallInt),
				new SqlParameter ("@tipohoras", SqlDbType.SmallInt),
				new SqlParameter ("@bh_cred", SqlDbType.SmallInt),
				new SqlParameter ("@bh_deb", SqlDbType.SmallInt),
				new SqlParameter ("@at_d", SqlDbType.SmallInt),
				new SqlParameter ("@at_n", SqlDbType.SmallInt),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@he50n", SqlDbType.SmallInt),
				new SqlParameter ("@he60n", SqlDbType.SmallInt),
				new SqlParameter ("@he70n", SqlDbType.SmallInt),
				new SqlParameter ("@he80n", SqlDbType.SmallInt),
				new SqlParameter ("@he90n", SqlDbType.SmallInt),
				new SqlParameter ("@he100n", SqlDbType.SmallInt),
                new SqlParameter ("@hesabn", SqlDbType.SmallInt),
				new SqlParameter ("@hedomn", SqlDbType.SmallInt),
				new SqlParameter ("@hefern", SqlDbType.SmallInt),
				new SqlParameter ("@folgan", SqlDbType.SmallInt),
                new SqlParameter ("@extranoturnabh", SqlDbType.SmallInt),
                new SqlParameter ("@percentualextra1", SqlDbType.Decimal),
                new SqlParameter ("@percentualextra2", SqlDbType.Decimal),
                new SqlParameter ("@percentualextra3", SqlDbType.Decimal),
                new SqlParameter ("@percentualextra4", SqlDbType.Decimal),
                new SqlParameter ("@percentualextra5", SqlDbType.Decimal),
                new SqlParameter ("@percentualextra6", SqlDbType.Decimal),
                new SqlParameter ("@percentualextra7", SqlDbType.Decimal),
                new SqlParameter ("@percentualextra8", SqlDbType.Decimal),
                new SqlParameter ("@percentualextra9", SqlDbType.Decimal),
                new SqlParameter ("@percentualextra10", SqlDbType.Decimal),
                new SqlParameter ("@horasabonadas", SqlDbType.SmallInt),
                new SqlParameter ("@ocorrenciasselecionadas", SqlDbType.SmallInt),
                new SqlParameter ("@idsocorrencias", SqlDbType.VarChar),
                new SqlParameter ("@HoristaMensalista", SqlDbType.SmallInt),
                new SqlParameter ("@ClassificarHorasExtras", SqlDbType.Bit),
                new SqlParameter ("@PercInItinere1", SqlDbType.SmallInt),
                new SqlParameter ("@PercInItinere2", SqlDbType.SmallInt),
                new SqlParameter ("@PercInItinere3", SqlDbType.SmallInt),
                new SqlParameter ("@PercInItinere4", SqlDbType.SmallInt),
                new SqlParameter ("@PercInItinere5", SqlDbType.SmallInt),
                new SqlParameter ("@PercInItinere6", SqlDbType.SmallInt),
                new SqlParameter ("@CodigoComplemento", SqlDbType.Int),
                new SqlParameter ("@InterjornadaExtra", SqlDbType.Bit)
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
            parms[44].Value = ((Modelo.Eventos)obj).PercentualExtra1;
            parms[45].Value = ((Modelo.Eventos)obj).PercentualExtra2;
            parms[46].Value = ((Modelo.Eventos)obj).PercentualExtra3;
            parms[47].Value = ((Modelo.Eventos)obj).PercentualExtra4;
            parms[48].Value = ((Modelo.Eventos)obj).PercentualExtra5;
            parms[49].Value = ((Modelo.Eventos)obj).PercentualExtra6;
            parms[50].Value = ((Modelo.Eventos)obj).PercentualExtra7;
            parms[51].Value = ((Modelo.Eventos)obj).PercentualExtra8;
            parms[52].Value = ((Modelo.Eventos)obj).PercentualExtra9;
            parms[53].Value = ((Modelo.Eventos)obj).PercentualExtra10;
            parms[54].Value = ((Modelo.Eventos)obj).HorasAbonadas;
            parms[55].Value = ((Modelo.Eventos)obj).OcorrenciasSelecionadas;
            parms[56].Value = ((Modelo.Eventos)obj).IdsOcorrencias;
            parms[57].Value = ((Modelo.Eventos)obj).HoristaMensalista;
            parms[58].Value = ((Modelo.Eventos)obj).ClassificarHorasExtras;
            parms[59].Value = ((Modelo.Eventos)obj).PercInItinere1;
            parms[60].Value = ((Modelo.Eventos)obj).PercInItinere2;
            parms[61].Value = ((Modelo.Eventos)obj).PercInItinere3;
            parms[62].Value = ((Modelo.Eventos)obj).PercInItinere4;
            parms[63].Value = ((Modelo.Eventos)obj).PercInItinere5;
            parms[64].Value = ((Modelo.Eventos)obj).PercInItinere6;
            parms[65].Value = ((Modelo.Eventos)obj).CodigoComplemento;
            parms[66].Value = ((Modelo.Eventos)obj).InterjornadaExtra;

        }

        public Modelo.Eventos LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

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
            SqlParameter[] parms = new SqlParameter[0];
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, "SELECT * FROM eventos", parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Eventos objEventos = new Modelo.Eventos();
                    AuxSetInstance(dr, objEventos);
                    lista.Add(objEventos);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }
        #endregion

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            base.IncluirAux(trans, obj);
            EventosClassHorasExtras dalClass = dalClass = new EventosClassHorasExtras(db);
            dalClass.UsuarioLogado = UsuarioLogado;
            dalClass.IncluirPorEvento(trans, (Modelo.Eventos)obj);
        }



        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);
            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();

            EventosClassHorasExtras dalClass = dalClass = new EventosClassHorasExtras(db);
            dalClass.UsuarioLogado = UsuarioLogado;
            dalClass.AlterarClassificacoesPorEvento(trans, (Modelo.Eventos)obj);
        }

        protected override void ExcluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            EventosClassHorasExtras dalClass = dalClass = new EventosClassHorasExtras(db);
            dalClass.UsuarioLogado = UsuarioLogado;
            dalClass.ExcluirPorEvento(trans, ((Modelo.Eventos)obj));
            base.ExcluirAux(trans, obj);
        }

        public Modelo.Eventos LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = "SELECT * FROM eventos WHERE eventos.codigo = @codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Eventos objevento = new Modelo.Eventos();
            SetInstance(dr, objevento);
            return objevento;
        }
    }
}
