using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DAL.SQL
{
    public class HorarioDetalhe : DAL.SQL.DALBase, DAL.IHorarioDetalhe
    {

        public string SELECTPHOR { get; set; }

        public HorarioDetalhe(DataBase database)
        {
            db = database;
            TABELA = "horariodetalhe";

            SELECTPID = @"   SELECT   horariodetalhe.*
                                    , case when dia = 1 then 'Seg.' when dia = 2 then 'Ter.' when dia = 3 then 'Qua.' when dia = 4 then 'Qui.'
                                       when dia = 5 then 'Sex.' when dia = 6 then 'Sab.' when dia = 7 then 'Dom.' else '' end AS diastr
                             FROM horariodetalhe WHERE id = @id";

            SELECTALL = @"   SELECT    horariodetalhe.*
                                     , case when dia = 1 then 'Seg.' when dia = 2 then 'Ter.' when dia = 3 then 'Qua.' when dia = 4 then 'Qui.'
                                       when dia = 5 then 'Sex.' when dia = 6 then 'Sab.' when dia = 7 then 'Dom.' else '' end AS diastr
                             FROM horariodetalhe";

            SELECTPHOR = @"   SELECT   horariodetalhe.*
                                     , case when dia = 1 then 'Seg.' when dia = 2 then 'Ter.' when dia = 3 then 'Qua.' when dia = 4 then 'Qui.'
                                       when dia = 5 then 'Sex.' when dia = 6 then 'Sab.' when dia = 7 then 'Dom.' else '' end AS diastr
                                     , case when diadsr = 0 then 'Não' when diadsr = 1 then 'Sim' end AS dsr
                                     , case when flagfolga = 0 then 'Não' when flagfolga = 1 then 'Sim' end AS folga
                              FROM horariodetalhe 
                              WHERE idhorario = @idhorario ORDER BY horariodetalhe.data";

            INSERT = @"  INSERT INTO horariodetalhe
							( codigo,  idhorario,  dia,  data,  entrada_1,  entrada_2,  entrada_3,  entrada_4,  saida_1,  saida_2,  saida_3,  saida_4,  totaltrabalhadadiurna,  totaltrabalhadanoturna,  cargahorariamista,  flagfolga,  diadsr,  bcarregar,  incdata,  inchora,  incusuario,  intervaloautomatico,  preassinaladas1,  preassinaladas2,  preassinaladas3,  marcacargahorariamista,  idjornada,  Neutro,  CicloSequenciaIndice)
							VALUES
							(@codigo, @idhorario, @dia, @data, @entrada_1, @entrada_2, @entrada_3, @entrada_4, @saida_1, @saida_2, @saida_3, @saida_4, @totaltrabalhadadiurna, @totaltrabalhadanoturna, @cargahorariamista, @flagfolga, @diadsr, @bcarregar, @incdata, @inchora, @incusuario, @intervaloautomatico, @preassinaladas1, @preassinaladas2, @preassinaladas3, @marcacargahorariamista, @idjornada, @Neutro, @CicloSequenciaIndice)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE horariodetalhe SET 
                              codigo = @codigo
							, idhorario = @idhorario
							, dia = @dia
							, data = @data
							, entrada_1 = @entrada_1
							, entrada_2 = @entrada_2
							, entrada_3 = @entrada_3
							, entrada_4 = @entrada_4
							, saida_1 = @saida_1
							, saida_2 = @saida_2
							, saida_3 = @saida_3
							, saida_4 = @saida_4
							, totaltrabalhadadiurna = @totaltrabalhadadiurna
							, totaltrabalhadanoturna = @totaltrabalhadanoturna
							, cargahorariamista = @cargahorariamista
							, flagfolga = @flagfolga
							, diadsr = @diadsr
                            , bcarregar = @bcarregar
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , intervaloautomatico = @intervaloautomatico 
                            , preassinaladas1 = @preassinaladas1
                            , preassinaladas2 = @preassinaladas2
                            , preassinaladas3 = @preassinaladas3
                            , marcacargahorariamista = @marcacargahorariamista
                            , idjornada = @idjornada
                            , Neutro = @Neutro
                            , CicloSequenciaIndice = @CicloSequenciaIndice
						WHERE id = @id";

            DELETE = @"  DELETE FROM horariodetalhe WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM horariodetalhe";

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

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
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
            ((Modelo.HorarioDetalhe)obj).Intervaloautomatico = (dr["intervaloautomatico"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["intervaloautomatico"]));
            ((Modelo.HorarioDetalhe)obj).Preassinaladas1 = (dr["preassinaladas1"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas1"]));
            ((Modelo.HorarioDetalhe)obj).Preassinaladas2 = (dr["preassinaladas2"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas2"]));
            ((Modelo.HorarioDetalhe)obj).Preassinaladas3 = (dr["preassinaladas3"] is DBNull ? (Int16)0 : Convert.ToInt16(dr["preassinaladas3"]));
            ((Modelo.HorarioDetalhe)obj).Marcacargahorariamista = dr["marcacargahorariamista"] is DBNull ? (short)0 : Convert.ToInt16(dr["marcacargahorariamista"]);
            ((Modelo.HorarioDetalhe)obj).Neutro = dr["neutro"] is DBNull || Convert.ToInt16(dr["neutro"]) == 0 ? false : true;
            ((Modelo.HorarioDetalhe)obj).Idjornada = dr["idjornada"] is DBNull ? 0 : Convert.ToInt32(dr["idjornada"]);
            ((Modelo.HorarioDetalhe)obj).ConverteHoraStringToInt();
            if (dr["CicloSequenciaIndice"] != System.DBNull.Value)
            {
                ((Modelo.HorarioDetalhe)obj).CicloSequenciaIndice = (int)dr["CicloSequenciaIndice"];
            }
        }

        private void AuxSetInstance2(SqlDataReader dr, Modelo.ModeloBase obj)
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
            ((Modelo.HorarioDetalhe)obj).Neutro = dr["neutro"] is DBNull || Convert.ToInt16(dr["neutro"]) == 0 ? false : true;
            ((Modelo.HorarioDetalhe)obj).Idjornada = dr["idjornada"] is DBNull ? 0 : Convert.ToInt32(dr["idjornada"]);
            ((Modelo.HorarioDetalhe)obj).ConverteHoraStringToInt();
            if (dr["CicloSequenciaIndice"] != System.DBNull.Value)
            {
                ((Modelo.HorarioDetalhe)obj).CicloSequenciaIndice = (int)dr["CicloSequenciaIndice"];
            }
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
			{
				new SqlParameter ("@id", SqlDbType.Int),
				new SqlParameter ("@codigo", SqlDbType.Int),
				new SqlParameter ("@idhorario", SqlDbType.Int),
				new SqlParameter ("@dia", SqlDbType.Int),
				new SqlParameter ("@data", SqlDbType.DateTime),
				new SqlParameter ("@entrada_1", SqlDbType.VarChar),
				new SqlParameter ("@entrada_2", SqlDbType.VarChar),
				new SqlParameter ("@entrada_3", SqlDbType.VarChar),
				new SqlParameter ("@entrada_4", SqlDbType.VarChar),
				new SqlParameter ("@saida_1", SqlDbType.VarChar),
				new SqlParameter ("@saida_2", SqlDbType.VarChar),
				new SqlParameter ("@saida_3", SqlDbType.VarChar),
				new SqlParameter ("@saida_4", SqlDbType.VarChar),
				new SqlParameter ("@totaltrabalhadadiurna", SqlDbType.VarChar),
				new SqlParameter ("@totaltrabalhadanoturna", SqlDbType.VarChar),
				new SqlParameter ("@cargahorariamista", SqlDbType.VarChar),
				new SqlParameter ("@flagfolga", SqlDbType.TinyInt),
				new SqlParameter ("@diadsr", SqlDbType.VarChar),
                new SqlParameter ("@bcarregar", SqlDbType.TinyInt),
				new SqlParameter ("@incdata", SqlDbType.DateTime),
				new SqlParameter ("@inchora", SqlDbType.DateTime),
				new SqlParameter ("@incusuario", SqlDbType.VarChar),
				new SqlParameter ("@altdata", SqlDbType.DateTime),
				new SqlParameter ("@althora", SqlDbType.DateTime),
				new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@intervaloautomatico", SqlDbType.Int),
                new SqlParameter ("@preassinaladas1", SqlDbType.Int),
                new SqlParameter ("@preassinaladas2", SqlDbType.Int),
                new SqlParameter ("@preassinaladas3", SqlDbType.Int),
                new SqlParameter ("@marcacargahorariamista", SqlDbType.Int),
                new SqlParameter ("@idjornada", SqlDbType.Int),
                new SqlParameter ("@Neutro", SqlDbType.Int),
                new SqlParameter ("@CicloSequenciaIndice", SqlDbType.Int)
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
            parms[31].Value = ((Modelo.HorarioDetalhe)obj).Neutro;
            parms[32].Value = ((Modelo.HorarioDetalhe)obj).CicloSequenciaIndice;
        }

        public Modelo.HorarioDetalhe LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);

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
            SqlParameter[] parms = new SqlParameter[] 
            { 
                  new SqlParameter("@idhorario", SqlDbType.Int) 
                , new SqlParameter("@dia", SqlDbType.Int) 
                , new SqlParameter("@data", SqlDbType.DateTime) 
            };
            parms[0].Value = pHorario;
            parms[1].Value = pDia;
            parms[2].Value = pData;

            string aux = @" SELECT    horariodetalhe.* 
                                    , case when dia = 1 then 'Seg.' when dia = 2 then 'Ter.' when dia = 3 then 'Qua.' when dia = 4 then 'Qui.'
                                       when dia = 5 then 'Sex.' when dia = 6 then 'Sab.' when dia = 7 then 'Dom.' else '' end AS diastr
                            FROM horariodetalhe WHERE idhorario = @idhorario and dia = @dia ";

            if (pTipoHorario == 2)
            {
                aux += " AND horariodetalhe.data = @data ";
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

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

        protected SqlDataReader LoadDataReaderPorHorario(int idHorario)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idhorario", SqlDbType.Int, 4) };
            parms[0].Value = idHorario;

            return db.ExecuteReader(CommandType.Text, SELECTPHOR, parms);
        }

        protected SqlDataReader LoadDataReaderPorHorario(SqlTransaction trans, int idHorario)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@idhorario", SqlDbType.Int, 4) };
            parms[0].Value = idHorario;

            return TransactDbOps.ExecuteReader(trans, CommandType.Text, SELECTPHOR, parms);
        }

        public List<Modelo.HorarioDetalhe> LoadPorHorario(int idHorario)
        {
            SqlDataReader dr = LoadDataReaderPorHorario(idHorario);

            List<Modelo.HorarioDetalhe> lista = new List<Modelo.HorarioDetalhe>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.HorarioDetalhe>();
                lista = AutoMapper.Mapper.Map<List<Modelo.HorarioDetalhe>>(dr);
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        public List<Modelo.HorarioDetalhe> LoadPorHorario(Hashtable ids)
        {
            SqlParameter[] parms = new SqlParameter[0];
            int[] arIDs= new int[ids.Count];
            ids.Keys.CopyTo(arIDs, 0);
            var strIDs = string.Join(",", arIDs);

            var sql = string.Format( @"SELECT 
                                        horariodetalhe.* 
                                        , case  when dia = 1 then 'Seg.' 
                                                when dia = 2 then 'Ter.' 
                                                when dia = 3 then 'Qua.' 
                                                when dia = 4 then 'Qui.'
                                                when dia = 5 then 'Sex.' 
                                                when dia = 6 then 'Sab.' 
                                                when dia = 7 then 'Dom.' 
                                                else '' end AS diastr
                                        ,case   when diadsr = 0 then 'Não' 
                                                when diadsr = 1 then 'Sim' end AS dsr
                                        ,case   when flagfolga = 0 then 'Não' 
                                                when flagfolga = 1 then 'Sim' 
                                                end AS folga
                                     FROM horariodetalhe WHERE idhorario IN ({0}) ORDER BY horariodetalhe.data ", strIDs);
            
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.HorarioDetalhe> lista = new List<Modelo.HorarioDetalhe>();
            while (dr.Read())
            {
                Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
                AuxSetInstance2(dr, objHorarioDetalhe);
                lista.Add(objHorarioDetalhe);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        public List<Modelo.HorarioDetalhe> LoadPorHorario(SqlTransaction trans, int idHorario)
        {
            SqlDataReader dr = LoadDataReaderPorHorario(trans, idHorario);

            List<Modelo.HorarioDetalhe> lista = new List<Modelo.HorarioDetalhe>();
            try
            {
                while (dr.Read())
                {
                    Modelo.HorarioDetalhe objHorarioDetalhe = new Modelo.HorarioDetalhe();
                    AuxSetInstance(dr, objHorarioDetalhe);
                    lista.Add(objHorarioDetalhe);
                }
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return lista;
        }

        public Hashtable LoadHorariosOrdenaSaida(int idHorario)
        {
            string str = $@"SELECT horariodetalhe.*
                           , case when dia = 1 then 'Seg.' when dia = 2 then 'Ter.' when dia = 3 then 'Qua.' when dia = 4 then 'Qui.'
                                       when dia = 5 then 'Sex.' when dia = 6 then 'Sab.' when dia = 7 then 'Dom.' else '' end AS diastr
                           , horario.tipohorario 
                           FROM horariodetalhe
                           INNER JOIN horario on horario.id = horariodetalhe.idhorario
                           WHERE horario.ordenabilhetesaida = 1
                             and horario.id = {idHorario}
                           ORDER BY horario.id";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, str, null);

            Hashtable ret = new Hashtable();
            if (dr.HasRows)
            {
                Modelo.pxyHorarioDetalheImportacao horario = new Modelo.pxyHorarioDetalheImportacao();
                Modelo.HorarioDetalhe objHorarioDetalhe;
                int idHorarioControle = 0, idHorario_Ant = 0;
                while (dr.Read())
                {
                    idHorarioControle = Convert.ToInt32(dr["idhorario"]);
                    if (idHorarioControle != idHorario_Ant || idHorario_Ant == 0)
                    {
                        if (idHorario_Ant > 0)
                            ret.Add(idHorario_Ant, horario);
                        horario = new Modelo.pxyHorarioDetalheImportacao();
                        horario.tipoHorario = Convert.ToInt32(dr["tipohorario"]);
                        idHorario_Ant = idHorarioControle;
                    }
                    objHorarioDetalhe = new Modelo.HorarioDetalhe();
                    AuxSetInstance(dr, objHorarioDetalhe);
                    horario.horariosDetalhe.Add(objHorarioDetalhe);
                }
                ret.Add(idHorario_Ant, horario);
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            cmd.Parameters.Clear();
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            cmd.Parameters.Clear();
        }

        public Modelo.HorarioDetalhe LoadObject(int pHorario, DateTime pData)
        {
            SqlParameter[] parms = new SqlParameter[] 
            { 
                  new SqlParameter("@idhorario", SqlDbType.Int, 4) 
                , new SqlParameter("@data", SqlDbType.DateTime) 
            };
            parms[0].Value = pHorario;
            parms[1].Value = pData;

            string aux = @"SELECT    horariodetalhe.* 
                                   , case when dia = 1 then 'Seg.' when dia = 2 then 'Ter.' when dia = 3 then 'Qua.' when dia = 4 then 'Qui.'
                                     when dia = 5 then 'Sex.' when dia = 6 then 'Sab.' when dia = 7 then 'Dom.' else '' end AS diastr
                           FROM horariodetalhe WHERE idhorario = @idhorario and data = @data";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

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
            SqlParameter[] parms = new SqlParameter[0];

            string aux = @"   SELECT   horariodetalhe.*
                                    , case when dia = 1 then 'Seg.' when dia = 2 then 'Ter.' when dia = 3 then 'Qua.' when dia = 4 then 'Qui.'
                                       when dia = 5 then 'Sex.' when dia = 6 then 'Sab.' when dia = 7 then 'Dom.' else '' end AS diastr
                             FROM horariodetalhe ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

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
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.HorarioDetalhe> GetPorJornada(int idJornada)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idjornada", SqlDbType.Int)
            };
            parms[0].Value = idJornada;

            string aux = "SELECT horariodetalhe.* "
                         + ", case when dia = 1 then 'Seg.' when dia = 2 then 'Ter.' when dia = 3 then 'Qua.' when dia = 4 then 'Qui.'"
                         + " when dia = 5 then 'Sex.' when dia = 6 then 'Sab.' when dia = 7 then 'Dom.' else '' end AS diastr"
                         + ", horario.marcacargahorariamista"
                         + ", parametros.inicioadnoturno"
                         + ", parametros.fimadnoturno"
                         + " FROM horariodetalhe "
                         + " INNER JOIN horario ON horario.id = horariodetalhe.idhorario "
                         + " INNER JOIN parametros ON parametros.id = horario.idparametro "
                         + " WHERE horariodetalhe.idjornada = @idjornada ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

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
                dr.Dispose();
            }
            return lista;
        }

        public void AtualizaHorarioDetalheJornada(List<Modelo.Jornada> jornadas)
        {
            StringBuilder cmdj = new StringBuilder();
            int count = 0;
            foreach (Modelo.Jornada jornada in jornadas)
            {
                if (jornada.Entrada_1 != "--:--" && jornada.Saida_1 != "--:--")
                {
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
                    cmdj.AppendLine(";");
                    count++;
                    if (count == 300)
                    {
                        using (SqlConnection conn = db.GetConnection)
                        {
                            SqlCommand cmd = new SqlCommand(cmdj.ToString(), conn);
                            cmd.ExecuteNonQuery();
                            cmdj.Remove(0, cmdj.Length);
                        }
                        count = 0;
                    }
                }
            }

            cmdj.AppendLine("UPDATE \"horariodetalhe\" SET \"idjornada\" = NULL");
            cmdj.AppendLine("WHERE \"entrada_1\" = '--:--';");

            using (SqlConnection conn = db.GetConnection)
            {
                SqlCommand cmd = new SqlCommand(cmdj.ToString(), conn);
                cmd.CommandTimeout = 120;
                cmd.ExecuteNonQuery();
                cmdj.Remove(0, cmdj.Length);
            }
        }

        public List<Modelo.pxyHorarioDetalheFuncionario> HorarioDetalheSegundoRegistroPorIdHorarioDoPrimeiroRegistro(int idHorario)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@idHorario", SqlDbType.Int)
            };
            parms[0].Value = idHorario;

            string sql = @"SELECT f2reg.id idFuncionario,
	                        f2reg.dscodigo,
	                        f2reg.nome,
	                        f2reg.matricula,
	                        h.codigo CodigoHorario,
	                        h.descricao,
                            hd.id ,
                            hd.idhorario ,
                            hd.dia ,
                            hd.data ,
                            hd.entrada_1 ,
                            hd.entrada_2 ,
                            hd.entrada_3 ,
                            hd.entrada_4 ,
                            hd.saida_1 ,
                            hd.saida_2 ,
                            hd.saida_3 ,
                            hd.saida_4,
	                        ISNULL(hd.entrada_1, '00:00') primeiraEntrada,
	                       CASE WHEN ISNULL(hd.saida_4, '--:--') != '--:--' THEN hd.saida_4
						        WHEN ISNULL(hd.saida_3, '--:--') != '--:--' THEN hd.saida_3
						        WHEN ISNULL(hd.saida_2, '--:--') != '--:--' THEN hd.saida_2
						        WHEN ISNULL(hd.saida_1, '--:--') != '--:--' THEN hd.saida_1
						    ELSE '--:--' END ultimaSaida
                            FROM dbo.funcionario f
                                LEFT JOIN funcionario f2reg ON f2reg.pis = f.pis
                                LEFT JOIN dbo.horario h ON h.id = f2reg.idhorario
                                LEFT JOIN dbo.horariodetalhe hd ON hd.idhorario = h.id
                            WHERE
                                f.idhorario = @idHorario
                                AND f.funcionarioativo = 1
	                            AND f.excluido = 0
                                AND f2reg.funcionarioativo = 1
                                AND f2reg.excluido = 0
                                AND f2reg.idhorario != @idHorario
                                AND hd.entrada_1 != '--:--'";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.pxyHorarioDetalheFuncionario> lista = new List<Modelo.pxyHorarioDetalheFuncionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.pxyHorarioDetalheFuncionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.pxyHorarioDetalheFuncionario>>(dr);
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

        public List<Modelo.Proxy.PxyHorarioMovel> GetRelPxyGradeHorario(int idhorario)
        {
            SqlParameter[] parms = new SqlParameter[1] { new SqlParameter("@idhorario", SqlDbType.Int) };
            parms[0].Value = idhorario;

            string aux;

            aux = @"
            SELECT
            	    hd.id,
                    h.codigo,
            	    hd.idhorario,
            	    hd.dia,
            	    hd.data,
            	    hd.entrada_1,
            	    hd.entrada_2,
            	    hd.entrada_3,
            	    hd.entrada_4,
            	    hd.saida_1,
            	    hd.saida_2,
            	    hd.saida_3,
            	    hd.saida_4,
            	    hd.totaltrabalhadadiurna,
            	    hd.totaltrabalhadanoturna,
            	    hd.cargahorariamista,
            	    hd.flagfolga,
            	    hd.diadsr,
            	    CASE 
            	    	when hd.dia = 1 then 'Seg.' 
            	    	WHEN hd.dia = 2 then 'Ter.' 
            	    	WHEN hd.dia = 3 then 'Qua.'
            	    	when hd.dia = 4 then 'Qui.'
            	    	WHEN hd.dia = 5 then 'Sex.' 
            	    	WHEN hd.dia = 6 then 'Sab.' 
            	    	WHEN hd.dia = 7 then 'Dom.' 
            	    ELSE '' end AS diastr,
            	    h.descricao
            FROM dbo.horariodetalhe hd
            INNER JOIN dbo.horario h ON h.id = hd.idhorario
            WHERE idhorario = @idhorario
            ORDER BY data ASC
                    ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Proxy.PxyHorarioMovel> listaPxyHorarioDetalhe = new List<Modelo.Proxy.PxyHorarioMovel>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyHorarioMovel>();
                listaPxyHorarioDetalhe = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyHorarioMovel>>(dr);
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();
               
            }catch (Exception ex)
            {
                throw (ex);
            }

            return listaPxyHorarioDetalhe;
        }

        public List<Modelo.Proxy.PxyHorarioMovel> GetRelPxyGradeHorario(int idhorario, DateTime dataInicial, DateTime dataFinal)
        {
            SqlParameter[] parms = new SqlParameter[] 
            {
                new SqlParameter("@idhorario", SqlDbType.Int),
                new SqlParameter("@dataInicial", SqlDbType.DateTime),
                new SqlParameter("dataFinal", SqlDbType.DateTime)
            };
            parms[0].Value = idhorario;
            parms[1].Value = dataInicial;
            parms[2].Value = dataFinal;

            string aux;

            aux = @"
            SELECT
            	    hd.id,
                    h.codigo,
            	    hd.idhorario,
            	    hd.dia,
            	    hd.data,
            	    hd.entrada_1,
            	    hd.entrada_2,
            	    hd.entrada_3,
            	    hd.entrada_4,
            	    hd.saida_1,
            	    hd.saida_2,
            	    hd.saida_3,
            	    hd.saida_4,
            	    hd.totaltrabalhadadiurna,
            	    hd.totaltrabalhadanoturna,
            	    hd.cargahorariamista,
            	    hd.flagfolga,
            	    hd.diadsr,
            	    CASE 
            	    	when hd.dia = 1 then 'Seg.' 
            	    	WHEN hd.dia = 2 then 'Ter.' 
            	    	WHEN hd.dia = 3 then 'Qua.'
            	    	when hd.dia = 4 then 'Qui.'
            	    	WHEN hd.dia = 5 then 'Sex.' 
            	    	WHEN hd.dia = 6 then 'Sab.' 
            	    	WHEN hd.dia = 7 then 'Dom.' 
            	    ELSE '' end AS diastr,
            	    h.descricao
            FROM dbo.horariodetalhe hd
            JOIN dbo.horario h ON h.id = hd.idhorario
            WHERE idhorario = @idhorario
            AND hd.data BETWEEN @dataInicial AND @dataFinal
            ORDER BY data ASC
                    ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Proxy.PxyHorarioMovel> listaPxyHorarioDetalhe = new List<Modelo.Proxy.PxyHorarioMovel>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyHorarioMovel>();
                listaPxyHorarioDetalhe = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyHorarioMovel>>(dr);
                if (!dr.IsClosed)
                    dr.Close();
                dr.Dispose();

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return listaPxyHorarioDetalhe;
        }
        #endregion
    }
}
