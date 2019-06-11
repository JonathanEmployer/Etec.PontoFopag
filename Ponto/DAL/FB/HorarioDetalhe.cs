using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FirebirdSql.Data.Isql;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class HorarioDetalhe : DAL.FB.DALBase, DAL.IHorarioDetalhe
    {
        public string SELECTPHOR { get; set; }

        private HorarioDetalhe()
        {
            GEN = "GEN_horariodetalhe_id";

            TABELA = "horariodetalhe";

            SELECTPID = "SELECT \"horariodetalhe\".* "
                         + ", case when \"dia\" = 1 then 'Seg.' when \"dia\" = 2 then 'Ter.' when \"dia\" = 3 then 'Qua.' when \"dia\" = 4 then 'Qui.'"
                         + " when \"dia\" = 5 then 'Sex.' when \"dia\" = 6 then 'Sab.' when \"dia\" = 7 then 'Dom.' else '' end AS \"diastr\""
                         + " FROM \"horariodetalhe\" "
                         + " WHERE \"id\" = @id";

            SELECTALL = "   SELECT \"horariodetalhe\".* "
                         + ", case when \"dia\" = 1 then 'Seg.' when \"dia\" = 2 then 'Ter.' when \"dia\" = 3 then 'Qua.' when \"dia\" = 4 then 'Qui.'"
                         + " when \"dia\" = 5 then 'Sex.' when \"dia\" = 6 then 'Sab.' when \"dia\" = 7 then 'Dom.' else '' end AS \"diastr\" "
                         + " FROM \"horariodetalhe\" ";

            SELECTPHOR = "   SELECT \"horariodetalhe\".* "
                         + ", case when \"dia\" = 1 then 'Seg.' when \"dia\" = 2 then 'Ter.' when \"dia\" = 3 then 'Qua.' when \"dia\" = 4 then 'Qui.'"
                         + " when \"dia\" = 5 then 'Sex.' when \"dia\" = 6 then 'Sab.' when \"dia\" = 7 then 'Dom.' else '' end AS \"diastr\""
                         + ",case when \"diadsr\" = 0 then 'Não' when \"diadsr\" = 1 then 'Sim' end AS \"dsr\""
                         + ",case when \"flagfolga\" = 0 then 'Não' when \"flagfolga\" = 1 then 'Sim' end AS \"folga\""
                         + " FROM \"horariodetalhe\" WHERE \"idhorario\" = @idhorario ORDER BY \"horariodetalhe\".\"data\" ";

            INSERT = "  INSERT INTO \"horariodetalhe\"" +
                                        "(\"codigo\", \"idhorario\", \"dia\", \"data\", \"entrada_1\", \"entrada_2\", \"entrada_3\", \"entrada_4\", \"saida_1\", \"saida_2\", \"saida_3\", \"saida_4\", \"totaltrabalhadadiurna\", \"totaltrabalhadanoturna\", \"cargahorariamista\", \"flagfolga\", \"diadsr\", \"bcarregar\", \"incdata\", \"inchora\", \"incusuario\", \"intervaloautomatico\", \"preassinaladas1\", \"preassinaladas2\", \"preassinaladas3\", \"marcacargahorariamista\", \"idjornada\")" +
                                        " VALUES " +
                                        "(@codigo, @idhorario, @dia, @data, @entrada_1, @entrada_2, @entrada_3, @entrada_4, @saida_1, @saida_2, @saida_3, @saida_4, @totaltrabalhadadiurna, @totaltrabalhadanoturna, @cargahorariamista, @flagfolga, @diadsr, @bcarregar, @incdata, @inchora, @incusuario,  @intervaloautomatico, @preassinaladas1, @preassinaladas2, @preassinaladas3, @marcacargahorariamista, @idjornada)";

            UPDATE = "  UPDATE \"horariodetalhe\" SET \"codigo\" = @codigo " +
                                        ", \"idhorario\" = @idhorario " +
                                        ", \"dia\" = @dia " +
                                        ", \"data\" = @data " +
                                        ", \"entrada_1\" = @entrada_1 " +
                                        ", \"entrada_2\" = @entrada_2 " +
                                        ", \"entrada_3\" = @entrada_3 " +
                                        ", \"entrada_4\" = @entrada_4 " +
                                        ", \"saida_1\" = @saida_1 " +
                                        ", \"saida_2\" = @saida_2 " +
                                        ", \"saida_3\" = @saida_3 " +
                                        ", \"saida_4\" = @saida_4 " +
                                        ", \"totaltrabalhadadiurna\" = @totaltrabalhadadiurna " +
                                        ", \"totaltrabalhadanoturna\" = @totaltrabalhadanoturna " +
                                        ", \"cargahorariamista\" = @cargahorariamista " +
                                        ",\"flagfolga\" = @flagfolga " +
                                        ",\"diadsr\" = @diadsr " +
                                        ", \"bcarregar\" = @bcarregar " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"intervaloautomatico\" = @intervaloautomatico " +
                                        ", \"preassinaladas1\" = @preassinaladas1 " +
                                        ", \"preassinaladas2\" = @preassinaladas2 " +
                                        ", \"preassinaladas3\" = @preassinaladas3 " +
                                        ", \"marcacargahorariamista\" = @marcacargahorariamista " +
                                        ", \"idjornada\" = @idjornada " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"horariodetalhe\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"horariodetalhe\"";

        }

        #region Singleton

        private static volatile FB.HorarioDetalhe _instancia = null;

        public static FB.HorarioDetalhe GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.HorarioDetalhe))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.HorarioDetalhe();
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
                obj = new Modelo.HorarioDetalhe();
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
            ((Modelo.HorarioDetalhe)obj).Id = Convert.ToInt32(dr["id"]);
            ((Modelo.HorarioDetalhe)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.HorarioDetalhe)obj).Idhorario = Convert.ToInt32(dr["idhorario"]);
            ((Modelo.HorarioDetalhe)obj).Dia = Convert.ToInt32(dr["dia"]);
            ((Modelo.HorarioDetalhe)obj).Data = (dr["data"] is DBNull ? null : (DateTime?)(dr["data"]));
            ((Modelo.HorarioDetalhe)obj).Entrada_1 = Convert.ToString(dr["entrada_1"]);
            ((Modelo.HorarioDetalhe)obj).Entrada_2 = Convert.ToString(dr["entrada_2"]);
            ((Modelo.HorarioDetalhe)obj).Entrada_3 = Convert.ToString(dr["entrada_3"]);
            ((Modelo.HorarioDetalhe)obj).Entrada_4 = Convert.ToString(dr["entrada_4"]);
            ((Modelo.HorarioDetalhe)obj).Saida_1 = Convert.ToString(dr["saida_1"]);
            ((Modelo.HorarioDetalhe)obj).Saida_2 = Convert.ToString(dr["saida_2"]);
            ((Modelo.HorarioDetalhe)obj).Saida_3 = Convert.ToString(dr["saida_3"]);
            ((Modelo.HorarioDetalhe)obj).Saida_4 = Convert.ToString(dr["saida_4"]);
            ((Modelo.HorarioDetalhe)obj).Totaltrabalhadadiurna = Convert.ToString(dr["totaltrabalhadadiurna"]);
            ((Modelo.HorarioDetalhe)obj).Totaltrabalhadanoturna = Convert.ToString(dr["totaltrabalhadanoturna"]);
            ((Modelo.HorarioDetalhe)obj).Cargahorariamista = Convert.ToString(dr["cargahorariamista"]);
            ((Modelo.HorarioDetalhe)obj).Flagfolga = Convert.ToInt16(dr["flagfolga"]);
            ((Modelo.HorarioDetalhe)obj).Diadsr = Convert.ToInt16(dr["diadsr"]);
            ((Modelo.HorarioDetalhe)obj).bCarregar = Convert.ToInt16(dr["bcarregar"]);
            ((Modelo.HorarioDetalhe)obj).DiaStr = Convert.ToString(dr["diastr"]);
            ((Modelo.HorarioDetalhe)obj).Intervaloautomatico = (dr["intervaloautomatico"] is DBNull ? (short)0 : Convert.ToInt16(dr["intervaloautomatico"]));
            ((Modelo.HorarioDetalhe)obj).Preassinaladas1 = (dr["preassinaladas1"] is DBNull ? (short)0 : Convert.ToInt16(dr["preassinaladas1"]));
            ((Modelo.HorarioDetalhe)obj).Preassinaladas2 = (dr["preassinaladas2"] is DBNull ? (short)0 : Convert.ToInt16(dr["preassinaladas2"]));
            ((Modelo.HorarioDetalhe)obj).Preassinaladas3 = (dr["preassinaladas3"] is DBNull ? (short)0 : Convert.ToInt16(dr["preassinaladas3"]));
            ((Modelo.HorarioDetalhe)obj).Marcacargahorariamista = dr["marcacargahorariamista"] is DBNull ? (short)0 : Convert.ToInt16(dr["marcacargahorariamista"]);
            ((Modelo.HorarioDetalhe)obj).Idjornada = dr["idjornada"] is DBNull ? 0 : Convert.ToInt32(dr["idjornada"]);
            ((Modelo.HorarioDetalhe)obj).ConverteHoraStringToInt();
        }

        private void AuxSetInstance2(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.HorarioDetalhe)obj).Id = Convert.ToInt32(dr["id"]);
            ((Modelo.HorarioDetalhe)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.HorarioDetalhe)obj).Idhorario = Convert.ToInt32(dr["idhorario"]);
            ((Modelo.HorarioDetalhe)obj).Dia = Convert.ToInt32(dr["dia"]);
            ((Modelo.HorarioDetalhe)obj).Data = (dr["data"] is DBNull ? null : (DateTime?)(dr["data"]));
            ((Modelo.HorarioDetalhe)obj).Entrada_1 = Convert.ToString(dr["entrada_1"]);
            ((Modelo.HorarioDetalhe)obj).Entrada_2 = Convert.ToString(dr["entrada_2"]);
            ((Modelo.HorarioDetalhe)obj).Entrada_3 = Convert.ToString(dr["entrada_3"]);
            ((Modelo.HorarioDetalhe)obj).Entrada_4 = Convert.ToString(dr["entrada_4"]);
            ((Modelo.HorarioDetalhe)obj).Saida_1 = Convert.ToString(dr["saida_1"]);
            ((Modelo.HorarioDetalhe)obj).Saida_2 = Convert.ToString(dr["saida_2"]);
            ((Modelo.HorarioDetalhe)obj).Saida_3 = Convert.ToString(dr["saida_3"]);
            ((Modelo.HorarioDetalhe)obj).Saida_4 = Convert.ToString(dr["saida_4"]);
            ((Modelo.HorarioDetalhe)obj).Totaltrabalhadadiurna = Convert.ToString(dr["totaltrabalhadadiurna"]);
            ((Modelo.HorarioDetalhe)obj).Totaltrabalhadanoturna = Convert.ToString(dr["totaltrabalhadanoturna"]);
            ((Modelo.HorarioDetalhe)obj).Cargahorariamista = Convert.ToString(dr["cargahorariamista"]);
            ((Modelo.HorarioDetalhe)obj).Flagfolga = Convert.ToInt16(dr["flagfolga"]);
            ((Modelo.HorarioDetalhe)obj).Folga = Convert.ToString(dr["folga"]);
            ((Modelo.HorarioDetalhe)obj).Diadsr = Convert.ToInt16(dr["diadsr"]);
            ((Modelo.HorarioDetalhe)obj).DSR = Convert.ToString(dr["dsr"]);
            ((Modelo.HorarioDetalhe)obj).bCarregar = Convert.ToInt16(dr["bcarregar"]);
            ((Modelo.HorarioDetalhe)obj).DiaStr = Convert.ToString(dr["diastr"]);
            ((Modelo.HorarioDetalhe)obj).Intervaloautomatico = (dr["intervaloautomatico"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["intervaloautomatico"]));
            ((Modelo.HorarioDetalhe)obj).Preassinaladas1 = (dr["preassinaladas1"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas1"]));
            ((Modelo.HorarioDetalhe)obj).Preassinaladas2 = (dr["preassinaladas2"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas2"]));
            ((Modelo.HorarioDetalhe)obj).Preassinaladas3 = (dr["preassinaladas3"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas3"]));
            ((Modelo.HorarioDetalhe)obj).Marcacargahorariamista = dr["marcacargahorariamista"] is DBNull ? (short)0 : Convert.ToInt16(dr["marcacargahorariamista"]);
            ((Modelo.HorarioDetalhe)obj).Idjornada = dr["idjornada"] is DBNull ? 0 : Convert.ToInt32(dr["idjornada"]);
            ((Modelo.HorarioDetalhe)obj).ConverteHoraStringToInt();
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@idhorario", FbDbType.Integer),
				new FbParameter ("@dia", FbDbType.Integer),
				new FbParameter ("@data", FbDbType.Date),
				new FbParameter ("@entrada_1", FbDbType.VarChar),
				new FbParameter ("@entrada_2", FbDbType.VarChar),
				new FbParameter ("@entrada_3", FbDbType.VarChar),
				new FbParameter ("@entrada_4", FbDbType.VarChar),
				new FbParameter ("@saida_1", FbDbType.VarChar),
				new FbParameter ("@saida_2", FbDbType.VarChar),
				new FbParameter ("@saida_3", FbDbType.VarChar),
				new FbParameter ("@saida_4", FbDbType.VarChar),
				new FbParameter ("@totaltrabalhadadiurna", FbDbType.VarChar),
				new FbParameter ("@totaltrabalhadanoturna", FbDbType.VarChar),
				new FbParameter ("@cargahorariamista", FbDbType.VarChar),
				new FbParameter ("@flagfolga", FbDbType.VarChar),
				new FbParameter ("@diadsr", FbDbType.VarChar),
				new FbParameter ("@bcarregar", FbDbType.SmallInt),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
                new FbParameter ("@intervaloautomatico", FbDbType.Integer),
                new FbParameter ("@preassinaladas1", FbDbType.Integer),
                new FbParameter ("@preassinaladas2", FbDbType.Integer),
                new FbParameter ("@preassinaladas3", FbDbType.Integer),
                new FbParameter ("@marcacargahorariamista", FbDbType.Integer),
                new FbParameter ("@idjornada", FbDbType.Integer),
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
            parms[1].Value = ((Modelo.HorarioDetalhe)obj).Codigo;
            parms[2].Value = ((Modelo.HorarioDetalhe)obj).Idhorario;
            parms[3].Value = ((Modelo.HorarioDetalhe)obj).Dia;
            parms[4].Value = ((Modelo.HorarioDetalhe)obj).Data;
            parms[5].Value = ((Modelo.HorarioDetalhe)obj).Entrada_1;
            parms[6].Value = ((Modelo.HorarioDetalhe)obj).Entrada_2;
            parms[7].Value = ((Modelo.HorarioDetalhe)obj).Entrada_3;
            parms[8].Value = ((Modelo.HorarioDetalhe)obj).Entrada_4;
            parms[9].Value = ((Modelo.HorarioDetalhe)obj).Saida_1;
            parms[10].Value = ((Modelo.HorarioDetalhe)obj).Saida_2;
            parms[11].Value = ((Modelo.HorarioDetalhe)obj).Saida_3;
            parms[12].Value = ((Modelo.HorarioDetalhe)obj).Saida_4;
            parms[13].Value = ((Modelo.HorarioDetalhe)obj).Totaltrabalhadadiurna;
            parms[14].Value = ((Modelo.HorarioDetalhe)obj).Totaltrabalhadanoturna;
            parms[15].Value = ((Modelo.HorarioDetalhe)obj).Cargahorariamista;
            parms[16].Value = ((Modelo.HorarioDetalhe)obj).Flagfolga;
            parms[17].Value = ((Modelo.HorarioDetalhe)obj).Diadsr;
            parms[18].Value = ((Modelo.HorarioDetalhe)obj).bCarregar;
            parms[19].Value = ((Modelo.HorarioDetalhe)obj).Incdata;
            parms[20].Value = ((Modelo.HorarioDetalhe)obj).Inchora;
            parms[21].Value = ((Modelo.HorarioDetalhe)obj).Incusuario;
            parms[22].Value = ((Modelo.HorarioDetalhe)obj).Altdata;
            parms[23].Value = ((Modelo.HorarioDetalhe)obj).Althora;
            parms[24].Value = ((Modelo.HorarioDetalhe)obj).Altusuario;
            parms[25].Value = ((Modelo.HorarioDetalhe)obj).Intervaloautomatico;
            parms[26].Value = ((Modelo.HorarioDetalhe)obj).Preassinaladas1;
            parms[27].Value = ((Modelo.HorarioDetalhe)obj).Preassinaladas2;
            parms[28].Value = ((Modelo.HorarioDetalhe)obj).Preassinaladas3;
            parms[29].Value = ((Modelo.HorarioDetalhe)obj).Marcacargahorariamista;
            if (((Modelo.HorarioDetalhe)obj).Idjornada > 0)
                parms[30].Value = ((Modelo.HorarioDetalhe)obj).Idjornada;
        }

        public Modelo.HorarioDetalhe LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
            try
            {
                SetInstance(dr, objHorarioDetalhe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objHorarioDetalhe;
        }

        public Modelo.HorarioDetalhe LoadParaCartaoPonto(int pHorario, int pDia, DateTime? pData, int pTipoHorario)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                  new FbParameter("@idhorario", FbDbType.Integer) 
                , new FbParameter("@dia", FbDbType.Integer) 
                , new FbParameter("@data", FbDbType.Date)
            };
            parms[0].Value = pHorario;
            parms[1].Value = pDia;
            parms[2].Value = pData;

            string aux = " SELECT    \"horariodetalhe\".* "
                           + ", case when \"dia\" = 1 then 'Seg.' when \"dia\" = 2 then 'Ter.' when \"dia\" = 3 then 'Qua.' when \"dia\" = 4 then 'Qui.'"
                           + "when \"dia\" = 5 then 'Sex.' when \"dia\" = 6 then 'Sab.' when \"dia\" = 7 then 'Dom.' else '' end AS \"diastr\""
                           + "FROM \"horariodetalhe\" WHERE \"idhorario\" = @idhorario and \"dia\" = @dia";

            if (pTipoHorario == 2)
            {
                aux += " AND \"horariodetalhe\".\"data\" = @data ";
            }

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
            try
            {
                SetInstance(dr, objHorarioDetalhe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return objHorarioDetalhe;
        }

        protected FbDataReader LoadDataReaderPorHorario(int idHorario)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idhorario", FbDbType.Integer, 4) };
            parms[0].Value = idHorario;

            return FB.DataBase.ExecuteReader(CommandType.Text, SELECTPHOR, parms);
        }

        protected FbDataReader LoadDataReaderPorHorario(FbTransaction trans, int idHorario)
        {
            FbParameter[] parms = new FbParameter[] { new FbParameter("@idhorario", FbDbType.Integer, 4) };
            parms[0].Value = idHorario;

            return FB.DataBase.ExecuteReader(trans, CommandType.Text, SELECTPHOR, parms);
        }

        public List<Modelo.HorarioDetalhe> LoadPorHorario(int idHorario)
        {
            FbDataReader dr = LoadDataReaderPorHorario(idHorario);

            List<Modelo.HorarioDetalhe> lista = new List<Modelo.HorarioDetalhe>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
                    AuxSetInstance2(dr, objHorarioDetalhe);
                    lista.Add(objHorarioDetalhe);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        public List<Modelo.HorarioDetalhe> LoadPorHorario(Hashtable ids)
        {
            FbParameter[] parms = new FbParameter[0];
            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("SELECT \"horariodetalhe\".*");
            cmd.AppendLine(", case when \"dia\" = 1 then 'Seg.' when \"dia\" = 2 then 'Ter.' when \"dia\" = 3 then 'Qua.' when \"dia\" = 4 then 'Qui.'");
            cmd.AppendLine(" when \"dia\" = 5 then 'Sex.' when \"dia\" = 6 then 'Sab.' when \"dia\" = 7 then 'Dom.' else '' end AS \"diastr\"");
            cmd.AppendLine(",case when \"diadsr\" = 0 then 'Não' when \"diadsr\" = 1 then 'Sim' end AS \"dsr\"");
            cmd.AppendLine(",case when \"flagfolga\" = 0 then 'Não' when \"flagfolga\" = 1 then 'Sim' end AS \"folga\"");
            cmd.AppendLine(" FROM \"horariodetalhe\" WHERE \"idhorario\" IN (");
            int count = 0;
            foreach (object id in ids.Keys)
            {
                if (count > 0)
                    cmd.Append(", ");
                cmd.Append(Convert.ToInt32(id));
                count++;
            }
            cmd.Append(")");
            cmd.AppendLine("ORDER BY \"horariodetalhe\".\"data\" ");
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, cmd.ToString(), parms);

            List<Modelo.HorarioDetalhe> lista = new List<Modelo.HorarioDetalhe>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
                    AuxSetInstance2(dr, objHorarioDetalhe);
                    lista.Add(objHorarioDetalhe);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        public List<Modelo.HorarioDetalhe> LoadPorHorario(FbTransaction trans, int idHorario)
        {
            FbDataReader dr = LoadDataReaderPorHorario(trans, idHorario);

            List<Modelo.HorarioDetalhe> lista = new List<Modelo.HorarioDetalhe>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
                    AuxSetInstance(dr, objHorarioDetalhe);
                    lista.Add(objHorarioDetalhe);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        protected override void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        protected override void IncluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = this.getID(trans);

            cmd.Parameters.Clear();
        }

        public Modelo.HorarioDetalhe LoadObject(int pHorario, DateTime pData)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                  new FbParameter("@idhorario", FbDbType.Integer, 4) 
                , new FbParameter("@data", FbDbType.Date) 
            };
            parms[0].Value = pHorario;
            parms[1].Value = pData;

            string aux = "SELECT \"horariodetalhe\".* "
                         + ", case when \"dia\" = 1 then 'Seg.' when \"dia\" = 2 then 'Ter.' when \"dia\" = 3 then 'Qua.' when \"dia\" = 4 then 'Qui.'"
                         + " when \"dia\" = 5 then 'Sex.' when \"dia\" = 6 then 'Sab.' when \"dia\" = 7 then 'Dom.' else '' end AS \"diastr\" "
                         + " FROM \"horariodetalhe\" WHERE \"idhorario\" = @idhorario and \"data\" = @data";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
            try
            {
                SetInstance(dr, objHorarioDetalhe);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return objHorarioDetalhe;
        }

        public List<Modelo.HorarioDetalhe> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];

            string aux = "SELECT \"horariodetalhe\".* "
                         + ", case when \"dia\" = 1 then 'Seg.' when \"dia\" = 2 then 'Ter.' when \"dia\" = 3 then 'Qua.' when \"dia\" = 4 then 'Qui.'"
                         + " when \"dia\" = 5 then 'Sex.' when \"dia\" = 6 then 'Sab.' when \"dia\" = 7 then 'Dom.' else '' end AS \"diastr\""
                         + " FROM \"horariodetalhe\" ";

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.HorarioDetalhe> lista = new List<Modelo.HorarioDetalhe>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
                    AuxSetInstance(dr, objHorarioDetalhe);
                    lista.Add(objHorarioDetalhe);
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

        public List<Modelo.HorarioDetalhe> GetPorJornada(int idJornada)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idjornada", FbDbType.Integer)
            };
            parms[0].Value = idJornada;

            string aux = "SELECT \"horariodetalhe\".* "
                         + ", case when \"dia\" = 1 then 'Seg.' when \"dia\" = 2 then 'Ter.' when \"dia\" = 3 then 'Qua.' when \"dia\" = 4 then 'Qui.'"
                         + " when \"dia\" = 5 then 'Sex.' when \"dia\" = 6 then 'Sab.' when \"dia\" = 7 then 'Dom.' else '' end AS \"diastr\""
                         + ", \"horario\".\"marcacargahorariamista\""
                         + ", \"parametros\".\"inicioadnoturno\""
                         + ", \"parametros\".\"fimadnoturno\""
                         + " FROM \"horariodetalhe\" "
                         + " INNER JOIN \"horario\" ON \"horario\".\"id\" = \"horariodetalhe\".\"idhorario\" "
                         + " INNER JOIN \"parametros\" ON \"parametros\".\"id\" = \"horario\".\"idparametro\" "
                         + " WHERE \"horariodetalhe\".\"idjornada\" = @idjornada ";

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.HorarioDetalhe> lista = new List<Modelo.HorarioDetalhe>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
                    AuxSetInstance(dr, objHorarioDetalhe);
                    objHorarioDetalhe.Marcacargahorariamista = Convert.ToInt16(dr["marcacargahorariamista"]);
                    objHorarioDetalhe.InicioAdNoturno = Convert.ToString(dr["inicioadnoturno"]);
                    objHorarioDetalhe.FimAdNoturno = Convert.ToString(dr["fimadnoturno"]);
                    lista.Add(objHorarioDetalhe);
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

        public void AtualizaHorarioDetalheJornada(List<Modelo.Jornada> jornadas)
        {
            List<string> comandos = new List<string>();
            StringBuilder cmdj = new StringBuilder();
            foreach (Modelo.Jornada jornada in jornadas)
            {
                if (jornada.Entrada_1 != "--:--" && jornada.Saida_1 != "--:--")
                {
                    cmdj.Remove(0, cmdj.Length);
                    cmdj.AppendLine("UPDATE \"horariodetalhe\" SET \"idjornada\" = ");
                    cmdj.AppendLine(jornada.Id.ToString());
                    cmdj.AppendLine("WHERE");
                    cmdj.AppendLine("\"entrada_1\" = '" + jornada.Entrada_1 + "'");
                    cmdj.AppendLine("AND \"entrada_2\" = '" + jornada.Entrada_2 + "'");
                    cmdj.AppendLine("AND \"entrada_3\" = '" + jornada.Entrada_3 + "'");
                    cmdj.AppendLine("AND \"entrada_4\" = '" + jornada.Entrada_4 + "'");
                    cmdj.AppendLine("AND \"saida_1\" = '" + jornada.Saida_1 + "'");
                    cmdj.AppendLine("AND \"saida_2\" = '" + jornada.Saida_2 + "'");
                    cmdj.AppendLine("AND \"saida_3\" = '" + jornada.Saida_3 + "'");
                    cmdj.AppendLine("AND \"saida_4\" = '" + jornada.Saida_4 + "'");
                    comandos.Add(cmdj.ToString());
                }
            }

            cmdj.Remove(0, cmdj.Length);
            cmdj.AppendLine("UPDATE \"horariodetalhe\" SET \"idjornada\" = NULL");
            cmdj.AppendLine("WHERE \"entrada_1\" = '--:--'");
            comandos.Add(cmdj.ToString());

            if (comandos.Count > 0)
            {
                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    FbBatchExecution batch = new FbBatchExecution(conn);
                    batch.SqlStatements.AddRange(comandos);
                    batch.Execute();
                }
            }
        }

        #endregion

        #region IHorarioDetalhe Members


        public Hashtable LoadHorariosOrdenaSaida()
        {
            throw new NotImplementedException();
        }

        #endregion


        public List<Modelo.pxyHorarioDetalheFuncionario> HorarioDetalheSegundoRegistroPorIdHorarioDoPrimeiroRegistro(int idHorario)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.HorarioDetalhe> GetRelGradeHorarioMovel(int idhorario)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Proxy.PxyHorarioMovel> GetRelPxyGradeHorario(int idhorario)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Proxy.PxyHorarioMovel> GetRelPxyGradeHorario(int idhorario, DateTime dataIni, DateTime dataFin)
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
