using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class Parametros : DAL.FB.DALBase, DAL.IParametros
    {
        public string SELECTPRI { get; set; }

        private Parametros()
        {
            GEN = "GEN_parametros_id";

            TABELA = "parametros";

            SELECTPID = "SELECT * FROM \"parametros\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT   \"parametros\".\"id\" " +
                                    ", \"parametros\".\"codigo\" " +
                                    ", \"parametros\".\"descricao\" " +
                                    ", COALESCE (\"parametros\".\"inicioadnoturno\", '--:--') AS \"inicioadnoturno\" " +
                                    ", COALESCE (\"parametros\".\"fimadnoturno\", '--:--') AS \"fimadnoturno\" " +
                                    ", COALESCE  (\"parametros\".\"thoraextra\", '--:--') As \"thoraextra\" " +
                                    ", COALESCE  (\"parametros\".\"thorafalta\", '--:--') As \"thorafalta\" " +
                             "FROM \"parametros\" ";

            SELECTPRI = "   SELECT FIRST 1 * FROM \"parametros\" ORDER BY \"parametros\".\"codigo\"";

            INSERT = "  INSERT INTO \"parametros\"" +
                                        "(\"codigo\", \"descricao\", \"inicioadnoturno\", \"fimadnoturno\", \"thoraextra\", \"thorafalta\", \"tipocompactador\", \"fazerbackupentrada\", \"fazerbackupsaida\", \"verificarbilhetes\", \"faltaemdias\", \"imprimeresponsavel\", \"imprimeobservacao\", \"tipohoraextrafalta\", \"imprimirnumrelogio\", \"campoobservacao\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @descricao, @inicioadnoturno, @fimadnoturno, @thoraextra, @thorafalta, @tipocompactador, @fazerbackupentrada, @fazerbackupsaida, @verificarbilhetes, @faltaemdias, @imprimeresponsavel, @imprimeobservacao, @tipohoraextrafalta, @imprimirnumrelogio, @campoobservacao, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"parametros\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"inicioadnoturno\" = @inicioadnoturno " +
                                        ", \"fimadnoturno\" = @fimadnoturno " +
                                        ", \"thoraextra\" = @thoraextra " +
                                        ", \"thorafalta\" = @thorafalta " +
                                        ", \"tipocompactador\" = @tipocompactador " +
                                        ", \"fazerbackupentrada\" = @fazerbackupentrada " +
                                        ", \"fazerbackupsaida\" = @fazerbackupsaida " +
                                        ", \"verificarbilhetes\" = @verificarbilhetes " +
                                        ", \"faltaemdias\" = @faltaemdias " +
                                        ", \"imprimeresponsavel\" = @imprimeresponsavel " +
                                        ", \"imprimeobservacao\" = @imprimeobservacao " +
                                        ", \"tipohoraextrafalta\" = @tipohoraextrafalta " +
                                        ", \"imprimirnumrelogio\" = @imprimirnumrelogio " +
                                        ", \"campoobservacao\" = @campoobservacao " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"parametros\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"parametros\"";

        }
        #region Singleton

        private static volatile FB.Parametros _instancia = null;

        public static FB.Parametros GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Parametros))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Parametros();
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
                obj = new Modelo.Parametros();
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
            ((Modelo.Parametros)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Parametros)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.Parametros)obj).InicioAdNoturno = Convert.ToString(dr["inicioadnoturno"]);
            ((Modelo.Parametros)obj).FimAdNoturno = Convert.ToString(dr["fimadnoturno"]);
            ((Modelo.Parametros)obj).THoraExtra = Convert.ToString(dr["thoraextra"]);
            ((Modelo.Parametros)obj).THoraFalta = Convert.ToString(dr["thorafalta"]);
            ((Modelo.Parametros)obj).TipoCompactador = Convert.ToInt16(dr["tipocompactador"]);
            ((Modelo.Parametros)obj).FazerBackupEntrada = Convert.ToInt16(dr["fazerbackupentrada"]);
            ((Modelo.Parametros)obj).FazerBackupSaida = Convert.ToInt16(dr["fazerbackupsaida"]);
            ((Modelo.Parametros)obj).VerificarBilhetes = Convert.ToInt16(dr["verificarbilhetes"]);
            ((Modelo.Parametros)obj).FaltaEmDias = Convert.ToInt16(dr["faltaemdias"]);
            ((Modelo.Parametros)obj).ImprimeResponsavel = Convert.ToInt16(dr["imprimeresponsavel"]);
            ((Modelo.Parametros)obj).ImprimeObservacao = Convert.ToInt16(dr["imprimeobservacao"]);
            ((Modelo.Parametros)obj).TipoHoraExtraFalta = Convert.ToInt16(dr["tipohoraextrafalta"]);
            ((Modelo.Parametros)obj).ImprimirNumRelogio = Convert.ToInt16(dr["imprimirnumrelogio"]);
            ((Modelo.Parametros)obj).CampoObservacao = Convert.ToString(dr["campoobservacao"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
				new FbParameter ("@inicioadnoturno", FbDbType.VarChar),
				new FbParameter ("@fimadnoturno", FbDbType.VarChar),
				new FbParameter ("@thoraextra", FbDbType.VarChar),
				new FbParameter ("@thorafalta", FbDbType.VarChar),
				new FbParameter ("@tipocompactador", FbDbType.SmallInt),
				new FbParameter ("@fazerbackupentrada", FbDbType.SmallInt),
				new FbParameter ("@fazerbackupsaida", FbDbType.SmallInt),
				new FbParameter ("@verificarbilhetes", FbDbType.SmallInt),
				new FbParameter ("@faltaemdias", FbDbType.SmallInt),
				new FbParameter ("@imprimeresponsavel", FbDbType.SmallInt),
				new FbParameter ("@imprimeobservacao", FbDbType.SmallInt),
				new FbParameter ("@tipohoraextrafalta", FbDbType.SmallInt),
				new FbParameter ("@imprimirnumrelogio", FbDbType.SmallInt),
				new FbParameter ("@campoobservacao", FbDbType.VarChar),
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
            parms[1].Value = ((Modelo.Parametros)obj).Codigo;
            parms[2].Value = ((Modelo.Parametros)obj).Descricao;
            parms[3].Value = ((Modelo.Parametros)obj).InicioAdNoturno;
            parms[4].Value = ((Modelo.Parametros)obj).FimAdNoturno;
            parms[5].Value = ((Modelo.Parametros)obj).THoraExtra;
            parms[6].Value = ((Modelo.Parametros)obj).THoraFalta;
            parms[7].Value = ((Modelo.Parametros)obj).TipoCompactador;
            parms[8].Value = ((Modelo.Parametros)obj).FazerBackupEntrada;
            parms[9].Value = ((Modelo.Parametros)obj).FazerBackupSaida;
            parms[10].Value = ((Modelo.Parametros)obj).VerificarBilhetes;
            parms[11].Value = ((Modelo.Parametros)obj).FaltaEmDias;
            parms[12].Value = ((Modelo.Parametros)obj).ImprimeResponsavel;
            parms[13].Value = ((Modelo.Parametros)obj).ImprimeObservacao;
            parms[14].Value = ((Modelo.Parametros)obj).TipoHoraExtraFalta;
            parms[15].Value = ((Modelo.Parametros)obj).ImprimirNumRelogio;
            parms[16].Value = ((Modelo.Parametros)obj).CampoObservacao;
            parms[17].Value = ((Modelo.Parametros)obj).Incdata;
            parms[18].Value = ((Modelo.Parametros)obj).Inchora;
            parms[19].Value = ((Modelo.Parametros)obj).Incusuario;
            parms[20].Value = ((Modelo.Parametros)obj).Altdata;
            parms[21].Value = ((Modelo.Parametros)obj).Althora;
            parms[22].Value = ((Modelo.Parametros)obj).Altusuario;
        }

        public Modelo.Parametros LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Parametros objParametros = new Modelo.Parametros();
            try
            {

                SetInstance(dr, objParametros);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objParametros;
        }
        
        public Modelo.Parametros LoadPrimeiro()
        {
            Modelo.Parametros objParametros = new Modelo.Parametros();
            try
            {
                FbParameter[] parms = new FbParameter[0];

                FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, SELECTPRI, parms);

                SetInstance(dr, objParametros);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objParametros;
        }

        public List<Modelo.Parametros> GetAllList()
        {
            List<Modelo.Parametros> ret = new List<Modelo.Parametros>();
            try
            {
                FbParameter[] parms = new FbParameter[0];

                FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"parametros\"", parms);

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Modelo.Parametros objParametros = new Modelo.Parametros();
                        AuxSetInstance(dr, objParametros);
                        ret.Add(objParametros);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return ret;
        }

        public void AtualizaTipoExtraFaltaMarcacoes(int id, Int16 tipohoraextrafalta, DateTime? dataInicial, DateTime? dataFinal)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idparametro", FbDbType.Integer),
                new FbParameter("@tipoextrafalta", FbDbType.Integer),
                new FbParameter("@datai", FbDbType.Date),
                new FbParameter("@dataf", FbDbType.Date)
            };
            parms[0].Value = id;
            parms[1].Value = tipohoraextrafalta;
            parms[2].Value = dataInicial;
            parms[3].Value = dataFinal;

            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("UPDATE \"marcacao\"");
            cmd.AppendLine("SET \"marcacao\".\"tipohoraextrafalta\" = @tipoextrafalta");
            cmd.AppendLine("WHERE");
            if (dataInicial != null && dataFinal != null)
                cmd.AppendLine("\"marcacao\".\"data\" >= @datai AND \"marcacao\".\"data\" <= @dataf AND");
            cmd.AppendLine("\"marcacao\".\"idhorario\" IN");
            cmd.AppendLine("(SELECT \"id\" FROM \"horario\" WHERE \"idparametro\" = @idparametro)");

            DataBase.ExecuteNonQuery(CommandType.Text, cmd.ToString(), parms);
        }

        public int GetExportaValorZerado()
        {
            Modelo.Parametros objParametros = new Modelo.Parametros();
            try
            {
                FbParameter[] parms = new FbParameter[0];

                FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, "Select ExportarValorZerado from parametros", parms);

                SetInstance(dr, objParametros);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objParametros.ExportarValorZerado;
        }

        public int? GetIdPorCod(int Cod)
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

        public List<Modelo.Parametros> GetAllList(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public bool Flg_Separar_Trabalhadas_Noturna_Extras_Noturna(int idfuncionario)
        {
            throw new NotImplementedException();
        }

        public void Adicionar(ModeloBase obj, bool Codigo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
