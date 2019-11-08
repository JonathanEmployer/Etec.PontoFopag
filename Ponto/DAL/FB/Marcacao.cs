using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class Marcacao : DAL.FB.DALBase, IMarcacao
    {
        public string SELECTPFU { get; set; }
        public string SELECTCP { get; set; }
        public string SELECTDEP { get; set; }


        private Marcacao()
        {
            GEN = "GEN_marcacao_id";

            TABELA = "marcacao";

            SELECTPID = "SELECT \"marcacao\".* " +
                               ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                        " FROM \"marcacao\" " +
                        " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                        " WHERE \"marcacao\".\"id\" = @id";

            SELECTALL = "SELECT \"marcacao\".* " +
                               ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                        " FROM \"marcacao\" " +
                        " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" ";

            SELECTDEP = "  SELECT   \"marcacao\".*" +
                                    ", \"funcionario\".\"nome\" AS \"funcionario\"" +
                            "FROM \"marcacao\"" +
                            "LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                            "WHERE \"funcionario\".\"iddepartamento\" = @iddepartamento  AND COALESCE(\"funcionario\".\"excluido\",0) = 0";

            SELECTPFU = "SELECT \"marcacao\".* " +
                               ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                        " FROM \"marcacao\" " +
                        " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                        " WHERE \"marcacao\".\"idfuncionario\" = @idfuncionario ";

            INSERT = "INSERT INTO \"marcacao\" " +
                        "(\"idfuncionario\", \"codigo\", \"dscodigo\", \"legenda\", \"data\", \"dia\", \"entrada_1\", \"entrada_2\", \"entrada_3\", \"entrada_4\", \"entrada_5\", \"entrada_6\", \"entrada_7\", \"entrada_8\", \"saida_1\", \"saida_2\", \"saida_3\", \"saida_4\", \"saida_5\", \"saida_6\", \"saida_7\", \"saida_8\", \"horastrabalhadas\", \"horasextrasdiurna\", \"horasfaltas\", \"entradaextra\", \"saidaextra\", \"horastrabalhadasnoturnas\", \"horasextranoturna\", \"horasfaltanoturna\", \"ocorrencia\", \"idhorario\", \"bancohorascre\", \"bancohorasdeb\", \"idfechamentobh\", \"semcalculo\", \"ent_num_relogio_1\", \"ent_num_relogio_2\", \"ent_num_relogio_3\", \"ent_num_relogio_4\", \"ent_num_relogio_5\", \"ent_num_relogio_6\", \"ent_num_relogio_7\", \"ent_num_relogio_8\", \"sai_num_relogio_1\", \"sai_num_relogio_2\", \"sai_num_relogio_3\", \"sai_num_relogio_4\", \"sai_num_relogio_5\", \"sai_num_relogio_6\", \"sai_num_relogio_7\", \"sai_num_relogio_8\", \"naoentrarbanco\", \"naoentrarnacompensacao\", \"horascompensadas\", \"idcompensado\", \"naoconsiderarcafe\", \"dsr\", \"valordsr\", \"abonardsr\", \"totalizadoresalterados\", \"calchorasextrasdiurna\", \"calchorasextranoturna\", \"calchorasfaltas\", \"calchorasfaltanoturna\", \"incdata\", \"inchora\", \"incusuario\", \"folga\", \"chave\", \"exphorasextranoturna\", \"tipohoraextrafalta\")" +
                        " VALUES " +
                        "(@idfuncionario, @codigo, @dscodigo, @legenda, @data, @dia, @entrada_1, @entrada_2, @entrada_3, @entrada_4, @entrada_5, @entrada_6, @entrada_7, @entrada_8, @saida_1, @saida_2, @saida_3, @saida_4, @saida_5, @saida_6, @saida_7, @saida_8, @horastrabalhadas, @horasextrasdiurna, @horasfaltas, @entradaextra, @saidaextra, @horastrabalhadasnoturnas, @horasextranoturna, @horasfaltanoturna, @ocorrencia, @idhorario, @bancohorascre, @bancohorasdeb, @idfechamentobh, @semcalculo, @ent_num_relogio_1, @ent_num_relogio_2, @ent_num_relogio_3, @ent_num_relogio_4, @ent_num_relogio_5, @ent_num_relogio_6, @ent_num_relogio_7, @ent_num_relogio_8, @sai_num_relogio_1, @sai_num_relogio_2, @sai_num_relogio_3, @sai_num_relogio_4, @sai_num_relogio_5, @sai_num_relogio_6, @sai_num_relogio_7, @sai_num_relogio_8, @naoentrarbanco, @naoentrarnacompensacao, @horascompensadas, @idcompensado, @naoconsiderarcafe, @dsr, @valordsr, @abonardsr, @totalizadoresalterados, @calchorasextrasdiurna, @calchorasextranoturna, @calchorasfaltas, @calchorasfaltanoturna, @incdata, @inchora, @incusuario, @folga, @chave, @exphorasextranoturna, @tipohoraextrafalta)";

            UPDATE = "  UPDATE \"marcacao\" SET \"idfuncionario\" = @idfuncionario " +
                                        ", \"dscodigo\" = @dscodigo " +
                                        ", \"codigo\" = @codigo " +
                                        ", \"legenda\" = @legenda " +
                                        ", \"data\" = @data " +
                                        ", \"dia\" = @dia " +
                                        ", \"entrada_1\" = @entrada_1 " +
                                        ", \"entrada_2\" = @entrada_2 " +
                                        ", \"entrada_3\" = @entrada_3 " +
                                        ", \"entrada_4\" = @entrada_4 " +
                                        ", \"entrada_5\" = @entrada_5 " +
                                        ", \"entrada_6\" = @entrada_6 " +
                                        ", \"entrada_7\" = @entrada_7 " +
                                        ", \"entrada_8\" = @entrada_8 " +
                                        ", \"saida_1\" = @saida_1 " +
                                        ", \"saida_2\" = @saida_2 " +
                                        ", \"saida_3\" = @saida_3 " +
                                        ", \"saida_4\" = @saida_4 " +
                                        ", \"saida_5\" = @saida_5 " +
                                        ", \"saida_6\" = @saida_6 " +
                                        ", \"saida_7\" = @saida_7 " +
                                        ", \"saida_8\" = @saida_8 " +
                                        ", \"horastrabalhadas\" = @horastrabalhadas " +
                                        ", \"horasextrasdiurna\" = @horasextrasdiurna " +
                                        ", \"horasfaltas\" = @horasfaltas " +
                                        ", \"entradaextra\" = @entradaextra " +
                                        ", \"saidaextra\" = @saidaextra " +
                                        ", \"horastrabalhadasnoturnas\" = @horastrabalhadasnoturnas " +
                                        ", \"horasextranoturna\" = @horasextranoturna " +
                                        ", \"horasfaltanoturna\" = @horasfaltanoturna " +
                                        ", \"ocorrencia\" = @ocorrencia " +
                                        ", \"idhorario\" = @idhorario " +
                                        ", \"bancohorascre\" = @bancohorascre " +
                                        ", \"bancohorasdeb\" = @bancohorasdeb " +
                                        ", \"idfechamentobh\" = @idfechamentobh " +
                                        ", \"semcalculo\" = @semcalculo " +
                                        ", \"ent_num_relogio_1\" = @ent_num_relogio_1 " +
                                        ", \"ent_num_relogio_2\" = @ent_num_relogio_2 " +
                                        ", \"ent_num_relogio_3\" = @ent_num_relogio_3 " +
                                        ", \"ent_num_relogio_4\" = @ent_num_relogio_4 " +
                                        ", \"ent_num_relogio_5\" = @ent_num_relogio_5 " +
                                        ", \"ent_num_relogio_6\" = @ent_num_relogio_6 " +
                                        ", \"ent_num_relogio_7\" = @ent_num_relogio_7 " +
                                        ", \"ent_num_relogio_8\" = @ent_num_relogio_8 " +
                                        ", \"sai_num_relogio_1\" = @sai_num_relogio_1 " +
                                        ", \"sai_num_relogio_2\" = @sai_num_relogio_2 " +
                                        ", \"sai_num_relogio_3\" = @sai_num_relogio_3 " +
                                        ", \"sai_num_relogio_4\" = @sai_num_relogio_4 " +
                                        ", \"sai_num_relogio_5\" = @sai_num_relogio_5 " +
                                        ", \"sai_num_relogio_6\" = @sai_num_relogio_6 " +
                                        ", \"sai_num_relogio_7\" = @sai_num_relogio_7 " +
                                        ", \"sai_num_relogio_8\" = @sai_num_relogio_8 " +
                                        ", \"naoentrarbanco\" = @naoentrarbanco " +
                                        ", \"naoentrarnacompensacao\" = @naoentrarnacompensacao " +
                                        ", \"horascompensadas\" = @horascompensadas " +
                                        ", \"idcompensado\" = @idcompensado " +
                                        ", \"naoconsiderarcafe\" = @naoconsiderarcafe " +
                                        ", \"dsr\" = @dsr " +
                                        ", \"valordsr\" = @valordsr " +
                                        ", \"abonardsr\" = @abonardsr " +
                                        ", \"totalizadoresalterados\" = @totalizadoresalterados " +
                                        ", \"calchorasextrasdiurna\" = @calchorasextrasdiurna " +
                                        ", \"calchorasextranoturna\" = @calchorasextranoturna " +
                                        ", \"calchorasfaltas\" = @calchorasfaltas " +
                                        ", \"calchorasfaltanoturna\" = @calchorasfaltanoturna " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                        ", \"folga\" = @folga " +
                                        ", \"exphorasextranoturna\" = @exphorasextranoturna " +
                                        ", \"tipohoraextrafalta\" = @tipohoraextrafalta " +
                                        ", \"chave\" = @chave " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"marcacao\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"marcacao\"";

        }

        #region Singleton

        private static volatile FB.Marcacao _instancia = null;

        public static FB.Marcacao GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.Marcacao))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.Marcacao();
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
                obj = new Modelo.Marcacao();
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
            ((Modelo.Marcacao)obj).Idfuncionario = Convert.ToInt32(dr["idfuncionario"]);
            ((Modelo.Marcacao)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Marcacao)obj).Dscodigo = Convert.ToString(dr["dscodigo"]);
            ((Modelo.Marcacao)obj).Legenda = Convert.ToString(dr["legenda"]);
            ((Modelo.Marcacao)obj).Data = Convert.ToDateTime(dr["data"]);
            ((Modelo.Marcacao)obj).Dia = Convert.ToString(dr["dia"]);
            ((Modelo.Marcacao)obj).Entrada_1 = Convert.ToString(dr["entrada_1"]);
            ((Modelo.Marcacao)obj).Entrada_2 = Convert.ToString(dr["entrada_2"]);
            ((Modelo.Marcacao)obj).Entrada_3 = Convert.ToString(dr["entrada_3"]);
            ((Modelo.Marcacao)obj).Entrada_4 = Convert.ToString(dr["entrada_4"]);
            ((Modelo.Marcacao)obj).Entrada_5 = Convert.ToString(dr["entrada_5"]);
            ((Modelo.Marcacao)obj).Entrada_6 = Convert.ToString(dr["entrada_6"]);
            ((Modelo.Marcacao)obj).Entrada_7 = Convert.ToString(dr["entrada_7"]);
            ((Modelo.Marcacao)obj).Entrada_8 = Convert.ToString(dr["entrada_8"]);
            ((Modelo.Marcacao)obj).Saida_1 = Convert.ToString(dr["saida_1"]);
            ((Modelo.Marcacao)obj).Saida_2 = Convert.ToString(dr["saida_2"]);
            ((Modelo.Marcacao)obj).Saida_3 = Convert.ToString(dr["saida_3"]);
            ((Modelo.Marcacao)obj).Saida_4 = Convert.ToString(dr["saida_4"]);
            ((Modelo.Marcacao)obj).Saida_5 = Convert.ToString(dr["saida_5"]);
            ((Modelo.Marcacao)obj).Saida_6 = Convert.ToString(dr["saida_6"]);
            ((Modelo.Marcacao)obj).Saida_7 = Convert.ToString(dr["saida_7"]);
            ((Modelo.Marcacao)obj).Saida_8 = Convert.ToString(dr["saida_8"]);
            ((Modelo.Marcacao)obj).Horastrabalhadas = Convert.ToString(dr["horastrabalhadas"]);
            ((Modelo.Marcacao)obj).Horasextrasdiurna = Convert.ToString(dr["horasextrasdiurna"]);
            ((Modelo.Marcacao)obj).Horasfaltas = Convert.ToString(dr["horasfaltas"]);
            ((Modelo.Marcacao)obj).Entradaextra = Convert.ToString(dr["entradaextra"]);
            ((Modelo.Marcacao)obj).Saidaextra = Convert.ToString(dr["saidaextra"]);
            ((Modelo.Marcacao)obj).Horastrabalhadasnoturnas = Convert.ToString(dr["horastrabalhadasnoturnas"]);
            ((Modelo.Marcacao)obj).Horasextranoturna = Convert.ToString(dr["horasextranoturna"]);
            ((Modelo.Marcacao)obj).Horasfaltanoturna = Convert.ToString(dr["horasfaltanoturna"]);
            ((Modelo.Marcacao)obj).Ocorrencia = Convert.ToString(dr["ocorrencia"]);
            ((Modelo.Marcacao)obj).Idhorario = Convert.ToInt32(dr["idhorario"]);
            ((Modelo.Marcacao)obj).Bancohorascre = Convert.ToString(dr["bancohorascre"]);
            ((Modelo.Marcacao)obj).Bancohorasdeb = Convert.ToString(dr["bancohorasdeb"]);
            ((Modelo.Marcacao)obj).Idfechamentobh = (dr["idfechamentobh"] is DBNull ? 0 : Convert.ToInt32(dr["idfechamentobh"]));
            ((Modelo.Marcacao)obj).Semcalculo = (dr["semcalculo"] is DBNull ? (short)0 : Convert.ToInt16(dr["semcalculo"]));
            ((Modelo.Marcacao)obj).Ent_num_relogio_1 = Convert.ToString(dr["ent_num_relogio_1"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_2 = Convert.ToString(dr["ent_num_relogio_2"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_3 = Convert.ToString(dr["ent_num_relogio_3"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_4 = Convert.ToString(dr["ent_num_relogio_4"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_5 = Convert.ToString(dr["ent_num_relogio_5"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_6 = Convert.ToString(dr["ent_num_relogio_6"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_7 = Convert.ToString(dr["ent_num_relogio_7"]);
            ((Modelo.Marcacao)obj).Ent_num_relogio_8 = Convert.ToString(dr["ent_num_relogio_8"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_1 = Convert.ToString(dr["sai_num_relogio_1"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_2 = Convert.ToString(dr["sai_num_relogio_2"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_3 = Convert.ToString(dr["sai_num_relogio_3"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_4 = Convert.ToString(dr["sai_num_relogio_4"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_5 = Convert.ToString(dr["sai_num_relogio_5"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_6 = Convert.ToString(dr["sai_num_relogio_6"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_7 = Convert.ToString(dr["sai_num_relogio_7"]);
            ((Modelo.Marcacao)obj).Sai_num_relogio_8 = Convert.ToString(dr["sai_num_relogio_8"]);
            ((Modelo.Marcacao)obj).Naoentrarbanco = (dr["naoentrarbanco"] is DBNull ? (short)0 : Convert.ToInt16(dr["naoentrarbanco"]));
            ((Modelo.Marcacao)obj).Naoentrarnacompensacao = (dr["naoentrarnacompensacao"] is DBNull ? (short)0 : Convert.ToInt16(dr["naoentrarnacompensacao"]));
            ((Modelo.Marcacao)obj).Horascompensadas = Convert.ToString(dr["horascompensadas"]);
            ((Modelo.Marcacao)obj).Idcompensado = (dr["idcompensado"] is DBNull ? 0 : Convert.ToInt32(dr["idcompensado"]));
            ((Modelo.Marcacao)obj).Naoconsiderarcafe = (dr["naoconsiderarcafe"] is DBNull ? (short)0 : Convert.ToInt16(dr["naoconsiderarcafe"]));
            ((Modelo.Marcacao)obj).Dsr = (dr["dsr"] is DBNull ? (short)0 : Convert.ToInt16(dr["dsr"]));
            ((Modelo.Marcacao)obj).Valordsr = Convert.ToString(dr["valordsr"]);
            ((Modelo.Marcacao)obj).Abonardsr = (dr["abonardsr"] is DBNull ? (short)0 : Convert.ToInt16(dr["abonardsr"]));
            ((Modelo.Marcacao)obj).Totalizadoresalterados = (dr["totalizadoresalterados"] is DBNull ? (short)0 : Convert.ToInt16(dr["totalizadoresalterados"]));
            ((Modelo.Marcacao)obj).Calchorasextrasdiurna = (dr["calchorasextrasdiurna"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasextrasdiurna"]));
            ((Modelo.Marcacao)obj).Calchorasextranoturna = (dr["calchorasextranoturna"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasextranoturna"]));
            ((Modelo.Marcacao)obj).Calchorasfaltas = (dr["calchorasfaltas"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasfaltas"]));
            ((Modelo.Marcacao)obj).Calchorasfaltanoturna = (dr["calchorasfaltanoturna"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasfaltanoturna"]));
            ((Modelo.Marcacao)obj).Funcionario = Convert.ToString(dr["funcionario"]);
            ((Modelo.Marcacao)obj).Folga = (dr["folga"] is DBNull ? (short)0 : Convert.ToInt16(dr["folga"]));
            ((Modelo.Marcacao)obj).ExpHorasextranoturna = dr["exphorasextranoturna"] is DBNull ? "--:--" : Convert.ToString(dr["exphorasextranoturna"]);
            ((Modelo.Marcacao)obj).TipoHoraExtraFalta = (dr["tipohoraextrafalta"] is DBNull ? (short)0 : Convert.ToInt16(dr["tipohoraextrafalta"]));
            ((Modelo.Marcacao)obj).Chave = dr["chave"] is DBNull ? "" : Convert.ToString(dr["chave"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@idfuncionario", FbDbType.Integer),
				new FbParameter ("@dscodigo", FbDbType.VarChar),
				new FbParameter ("@legenda", FbDbType.Char),
				new FbParameter ("@data", FbDbType.Date),
				new FbParameter ("@dia", FbDbType.VarChar),
				new FbParameter ("@entrada_1", FbDbType.VarChar),
				new FbParameter ("@entrada_2", FbDbType.VarChar),
				new FbParameter ("@entrada_3", FbDbType.VarChar),
				new FbParameter ("@entrada_4", FbDbType.VarChar),
				new FbParameter ("@entrada_5", FbDbType.VarChar),
				new FbParameter ("@entrada_6", FbDbType.VarChar),
				new FbParameter ("@entrada_7", FbDbType.VarChar),
				new FbParameter ("@entrada_8", FbDbType.VarChar),
				new FbParameter ("@saida_1", FbDbType.VarChar),
				new FbParameter ("@saida_2", FbDbType.VarChar),
				new FbParameter ("@saida_3", FbDbType.VarChar),
				new FbParameter ("@saida_4", FbDbType.VarChar),
				new FbParameter ("@saida_5", FbDbType.VarChar),
				new FbParameter ("@saida_6", FbDbType.VarChar),
				new FbParameter ("@saida_7", FbDbType.VarChar),
				new FbParameter ("@saida_8", FbDbType.VarChar),
				new FbParameter ("@horastrabalhadas", FbDbType.VarChar),
				new FbParameter ("@horasextrasdiurna", FbDbType.VarChar),
				new FbParameter ("@horasfaltas", FbDbType.VarChar),
				new FbParameter ("@entradaextra", FbDbType.VarChar),
				new FbParameter ("@saidaextra", FbDbType.VarChar),
				new FbParameter ("@horastrabalhadasnoturnas", FbDbType.VarChar),
				new FbParameter ("@horasextranoturna", FbDbType.VarChar),
				new FbParameter ("@horasfaltanoturna", FbDbType.VarChar),
				new FbParameter ("@ocorrencia", FbDbType.VarChar),
				new FbParameter ("@idhorario", FbDbType.Integer),
				new FbParameter ("@bancohorascre", FbDbType.VarChar),
				new FbParameter ("@bancohorasdeb", FbDbType.VarChar),
				new FbParameter ("@idfechamentobh", FbDbType.Integer),
				new FbParameter ("@semcalculo", FbDbType.SmallInt),
				new FbParameter ("@ent_num_relogio_1", FbDbType.Char),
				new FbParameter ("@ent_num_relogio_2", FbDbType.Char),
				new FbParameter ("@ent_num_relogio_3", FbDbType.Char),
				new FbParameter ("@ent_num_relogio_4", FbDbType.Char),
				new FbParameter ("@ent_num_relogio_5", FbDbType.Char),
				new FbParameter ("@ent_num_relogio_6", FbDbType.Char),
				new FbParameter ("@ent_num_relogio_7", FbDbType.Char),
				new FbParameter ("@ent_num_relogio_8", FbDbType.Char),
				new FbParameter ("@sai_num_relogio_1", FbDbType.Char),
				new FbParameter ("@sai_num_relogio_2", FbDbType.Char),
				new FbParameter ("@sai_num_relogio_3", FbDbType.Char),
				new FbParameter ("@sai_num_relogio_4", FbDbType.Char),
				new FbParameter ("@sai_num_relogio_5", FbDbType.Char),
				new FbParameter ("@sai_num_relogio_6", FbDbType.Char),
				new FbParameter ("@sai_num_relogio_7", FbDbType.Char),
				new FbParameter ("@sai_num_relogio_8", FbDbType.Char),
				new FbParameter ("@naoentrarbanco", FbDbType.SmallInt),
				new FbParameter ("@naoentrarnacompensacao", FbDbType.SmallInt),
				new FbParameter ("@horascompensadas", FbDbType.VarChar),
				new FbParameter ("@idcompensado", FbDbType.Integer),
				new FbParameter ("@naoconsiderarcafe", FbDbType.SmallInt),
				new FbParameter ("@dsr", FbDbType.SmallInt),
				new FbParameter ("@valordsr", FbDbType.VarChar),
				new FbParameter ("@abonardsr", FbDbType.SmallInt),
				new FbParameter ("@totalizadoresalterados", FbDbType.SmallInt),
				new FbParameter ("@calchorasextrasdiurna", FbDbType.Integer),
				new FbParameter ("@calchorasextranoturna", FbDbType.Integer),
				new FbParameter ("@calchorasfaltas", FbDbType.Integer),
				new FbParameter ("@calchorasfaltanoturna", FbDbType.Integer),
				new FbParameter ("@incdata", FbDbType.Date),
				new FbParameter ("@inchora", FbDbType.Date),
				new FbParameter ("@incusuario", FbDbType.VarChar),
				new FbParameter ("@altdata", FbDbType.Date),
				new FbParameter ("@althora", FbDbType.Date),
				new FbParameter ("@altusuario", FbDbType.VarChar),
                new FbParameter ("@codigo", FbDbType.Integer),
                new FbParameter ("@folga", FbDbType.SmallInt),
                new FbParameter ("@exphorasextranoturna", FbDbType.VarChar),
                new FbParameter ("@tipohoraextrafalta", FbDbType.SmallInt),
                new FbParameter ("@chave", SqlDbType.VarChar)
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
            parms[1].Value = ((Modelo.Marcacao)obj).Idfuncionario;
            parms[2].Value = ((Modelo.Marcacao)obj).Dscodigo;
            parms[3].Value = ((Modelo.Marcacao)obj).Legenda;
            parms[4].Value = ((Modelo.Marcacao)obj).Data;
            parms[5].Value = ((Modelo.Marcacao)obj).Dia;
            parms[6].Value = ((Modelo.Marcacao)obj).Entrada_1;
            parms[7].Value = ((Modelo.Marcacao)obj).Entrada_2;
            parms[8].Value = ((Modelo.Marcacao)obj).Entrada_3;
            parms[9].Value = ((Modelo.Marcacao)obj).Entrada_4;
            parms[10].Value = ((Modelo.Marcacao)obj).Entrada_5;
            parms[11].Value = ((Modelo.Marcacao)obj).Entrada_6;
            parms[12].Value = ((Modelo.Marcacao)obj).Entrada_7;
            parms[13].Value = ((Modelo.Marcacao)obj).Entrada_8;
            parms[14].Value = ((Modelo.Marcacao)obj).Saida_1;
            parms[15].Value = ((Modelo.Marcacao)obj).Saida_2;
            parms[16].Value = ((Modelo.Marcacao)obj).Saida_3;
            parms[17].Value = ((Modelo.Marcacao)obj).Saida_4;
            parms[18].Value = ((Modelo.Marcacao)obj).Saida_5;
            parms[19].Value = ((Modelo.Marcacao)obj).Saida_6;
            parms[20].Value = ((Modelo.Marcacao)obj).Saida_7;
            parms[21].Value = ((Modelo.Marcacao)obj).Saida_8;
            parms[22].Value = ((Modelo.Marcacao)obj).Horastrabalhadas;
            parms[23].Value = ((Modelo.Marcacao)obj).Horasextrasdiurna;
            parms[24].Value = ((Modelo.Marcacao)obj).Horasfaltas;
            parms[25].Value = ((Modelo.Marcacao)obj).Entradaextra;
            parms[26].Value = ((Modelo.Marcacao)obj).Saidaextra;
            parms[27].Value = ((Modelo.Marcacao)obj).Horastrabalhadasnoturnas;
            parms[28].Value = ((Modelo.Marcacao)obj).Horasextranoturna;
            parms[29].Value = ((Modelo.Marcacao)obj).Horasfaltanoturna;
            parms[30].Value = ((Modelo.Marcacao)obj).Ocorrencia;
            parms[31].Value = ((Modelo.Marcacao)obj).Idhorario;
            parms[32].Value = ((Modelo.Marcacao)obj).Bancohorascre;
            parms[33].Value = ((Modelo.Marcacao)obj).Bancohorasdeb;
            if (((Modelo.Marcacao)obj).Idfechamentobh > 0)
            {
                parms[34].Value = ((Modelo.Marcacao)obj).Idfechamentobh;
            }
            parms[35].Value = ((Modelo.Marcacao)obj).Semcalculo;
            parms[36].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_1;
            parms[37].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_2;
            parms[38].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_3;
            parms[39].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_4;
            parms[40].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_5;
            parms[41].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_6;
            parms[42].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_7;
            parms[43].Value = ((Modelo.Marcacao)obj).Ent_num_relogio_8;
            parms[44].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_1;
            parms[45].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_2;
            parms[46].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_3;
            parms[47].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_4;
            parms[48].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_5;
            parms[49].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_6;
            parms[50].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_7;
            parms[51].Value = ((Modelo.Marcacao)obj).Sai_num_relogio_8;
            parms[52].Value = ((Modelo.Marcacao)obj).Naoentrarbanco;
            parms[53].Value = ((Modelo.Marcacao)obj).Naoentrarnacompensacao;
            parms[54].Value = ((Modelo.Marcacao)obj).Horascompensadas;
            if (((Modelo.Marcacao)obj).Idcompensado > 0)
            {
                parms[55].Value = ((Modelo.Marcacao)obj).Idcompensado;
            }
            else
                parms[55].Value = null;

            parms[56].Value = ((Modelo.Marcacao)obj).Naoconsiderarcafe;
            parms[57].Value = ((Modelo.Marcacao)obj).Dsr;
            parms[58].Value = ((Modelo.Marcacao)obj).Valordsr;
            parms[59].Value = ((Modelo.Marcacao)obj).Abonardsr;
            parms[60].Value = ((Modelo.Marcacao)obj).Totalizadoresalterados;
            parms[61].Value = ((Modelo.Marcacao)obj).Calchorasextrasdiurna;
            parms[62].Value = ((Modelo.Marcacao)obj).Calchorasextranoturna;
            parms[63].Value = ((Modelo.Marcacao)obj).Calchorasfaltas;
            parms[64].Value = ((Modelo.Marcacao)obj).Calchorasfaltanoturna;
            parms[65].Value = ((Modelo.Marcacao)obj).Incdata;
            parms[66].Value = ((Modelo.Marcacao)obj).Inchora;
            parms[67].Value = ((Modelo.Marcacao)obj).Incusuario;
            parms[68].Value = ((Modelo.Marcacao)obj).Altdata;
            parms[69].Value = ((Modelo.Marcacao)obj).Althora;
            parms[70].Value = ((Modelo.Marcacao)obj).Altusuario;
            parms[71].Value = ((Modelo.Marcacao)obj).Codigo;
            parms[72].Value = ((Modelo.Marcacao)obj).Folga;
            parms[73].Value = ((Modelo.Marcacao)obj).ExpHorasextranoturna;
            parms[74].Value = ((Modelo.Marcacao)obj).TipoHoraExtraFalta; 
            ((Modelo.Marcacao)obj).Chave = ((Modelo.Marcacao)obj).ToMD5();
            parms[75].Value = ((Modelo.Marcacao)obj).Chave;
        }

        public Modelo.Marcacao LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
            try
            {
                SetInstance(dr, objMarcacao);

                DAL.FB.Afastamento dalAfastamento = FB.Afastamento.GetInstancia;
                objMarcacao.Afastamento = dalAfastamento.LoadParaManutencao(objMarcacao.Data, objMarcacao.Idfuncionario);

                DAL.FB.BilhetesImp dalBilhetesImp = DAL.FB.BilhetesImp.GetInstancia;
                objMarcacao.BilhetesMarcacao = dalBilhetesImp.LoadManutencaoBilhetes(objMarcacao.Dscodigo, objMarcacao.Data, true);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objMarcacao;
        }

        protected override void IncluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = this.getID(trans);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            AuxManutencao(trans, obj);

            if (((Modelo.Marcacao)obj).Bilhete != null)
            {
                DAL.FB.BilhetesImp dalBilhete = DAL.FB.BilhetesImp.GetInstancia;
                dalBilhete.Alterar(trans, ((Modelo.Marcacao)obj).Bilhete);
            }

            cmd.Parameters.Clear();
        }

        protected override void ExcluirAux(FbTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = { new FbParameter("@id", FbDbType.Integer, 4) };
            parms[0].Value = obj.Id;

            ValidaDependencia(trans, obj);

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, DELETE, true, parms);

            cmd.Parameters.Clear();
        }

        public void SetaIdCompensadoNulo(int pIdCompensacao)
        {

            FbParameter[] parms = { new FbParameter("@idcompensado", FbDbType.Integer, 4) };
            parms[0].Value = pIdCompensacao;

            string comando = "UPDATE \"marcacao\" SET " +
                                       " \"idcompensado\" = NULL " +
                                   "WHERE \"idcompensado\" = @idcompensado";

            FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando, true, parms);
        }

        public int QuantidadeCompensada(int pIdCompensacao)
        {
            FbParameter[] parms = { new FbParameter("@idcompensado", FbDbType.Integer, 4) };
            parms[0].Value = pIdCompensacao;

            string comando = "SELECT COUNT(\"id\") FROM \"marcacao\"" +

                                   "WHERE \"idcompensado\" = @idcompensado";

            return (int)FB.DataBase.ExecuteScalar(CommandType.Text, comando, parms);
        }

        public int QuantidadeMarcacoes(int pIdFuncionario, DateTime pDataI, DateTime pDataF)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idfuncionario", FbDbType.Integer),
                new FbParameter("@datai", FbDbType.Date),
                new FbParameter("@dataf", FbDbType.Date)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string comando = "SELECT COUNT(DISTINCT \"id\") FROM \"marcacao\""
                           + " WHERE \"idfuncionario\" = @idfuncionario AND \"data\" >= @datai AND \"data\" <= @dataf";

            return (int)FB.DataBase.ExecuteScalar(CommandType.Text, comando, parms);
        }

        public void SalvarMarcacoes(List<Modelo.Marcacao> lista)
        {
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (Modelo.Marcacao obj in lista)
                        {
                            switch (obj.Acao)
                            {
                                case Modelo.Acao.Incluir:
                                    IncluirAux(trans, obj);
                                    break;
                                case Modelo.Acao.Alterar:
                                    AlterarAux(trans, obj);
                                    break;
                                case Modelo.Acao.Excluir:
                                    ExcluirAux(trans, obj);
                                    break;
                            }
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }

        public void IncluirMarcacoesEmLote(List<Modelo.Marcacao> marcacaoes)
        {
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                StringBuilder str = new StringBuilder();
                FbBatchExecution batch = new FbBatchExecution(conn);
                foreach (Modelo.Marcacao marc in marcacaoes)
                {
                    SetDadosInc(marc);
                    marc.Chave = marc.ToMD5();
                    str.Remove(0, str.Length);
                    str.Append("INSERT INTO \"marcacao\" ");
                    str.Append("(\"idfuncionario\", \"codigo\", \"dscodigo\", \"legenda\", \"data\", \"dia\", \"entrada_1\", \"entrada_2\", \"entrada_3\", \"entrada_4\", \"entrada_5\", \"entrada_6\", \"entrada_7\", \"entrada_8\", \"saida_1\", \"saida_2\", \"saida_3\", \"saida_4\", \"saida_5\", \"saida_6\", \"saida_7\", \"saida_8\", \"horastrabalhadas\", \"horasextrasdiurna\", \"horasfaltas\", \"entradaextra\", \"saidaextra\", \"horastrabalhadasnoturnas\", \"horasextranoturna\", \"horasfaltanoturna\", \"ocorrencia\", \"idhorario\", \"bancohorascre\", \"bancohorasdeb\", \"idfechamentobh\", \"semcalculo\", \"ent_num_relogio_1\", \"ent_num_relogio_2\", \"ent_num_relogio_3\", \"ent_num_relogio_4\", \"ent_num_relogio_5\", \"ent_num_relogio_6\", \"ent_num_relogio_7\", \"ent_num_relogio_8\", \"sai_num_relogio_1\", \"sai_num_relogio_2\", \"sai_num_relogio_3\", \"sai_num_relogio_4\", \"sai_num_relogio_5\", \"sai_num_relogio_6\", \"sai_num_relogio_7\", \"sai_num_relogio_8\", \"naoentrarbanco\", \"naoentrarnacompensacao\", \"horascompensadas\", \"idcompensado\", \"naoconsiderarcafe\", \"dsr\", \"valordsr\", \"abonardsr\", \"totalizadoresalterados\", \"calchorasextrasdiurna\", \"calchorasextranoturna\", \"calchorasfaltas\", \"calchorasfaltanoturna\", \"incdata\", \"inchora\", \"incusuario\", \"folga\", \"chave\", \"exphorasextranoturna\", \"tipohoraextrafalta\") ");
                    str.Append("VALUES (");
                    str.Append(marc.Idfuncionario);
                    str.Append(", ");
                    str.Append(marc.Codigo);
                    str.Append(", '");
                    str.Append(marc.Dscodigo);
                    str.Append("', '");
                    str.Append(marc.Legenda);
                    str.Append("', '");
                    str.Append(marc.Data.Month + "/" + marc.Data.Day + "/" + marc.Data.Year);
                    str.Append("', '");
                    str.Append(marc.Dia);
                    str.Append("', '");
                    str.Append(marc.Entrada_1);
                    str.Append("', '");
                    str.Append(marc.Entrada_2);
                    str.Append("', '");
                    str.Append(marc.Entrada_3);
                    str.Append("', '");
                    str.Append(marc.Entrada_4);
                    str.Append("', '");
                    str.Append(marc.Entrada_5);
                    str.Append("', '");
                    str.Append(marc.Entrada_6);
                    str.Append("', '");
                    str.Append(marc.Entrada_7);
                    str.Append("', '");
                    str.Append(marc.Entrada_8);
                    str.Append("', '");
                    str.Append(marc.Saida_1);
                    str.Append("', '");
                    str.Append(marc.Saida_2);
                    str.Append("', '");
                    str.Append(marc.Saida_3);
                    str.Append("', '");
                    str.Append(marc.Saida_4);
                    str.Append("', '");
                    str.Append(marc.Saida_5);
                    str.Append("', '");
                    str.Append(marc.Saida_6);
                    str.Append("', '");
                    str.Append(marc.Saida_7);
                    str.Append("', '");
                    str.Append(marc.Saida_8);
                    str.Append("', '");
                    str.Append(marc.Horastrabalhadas);
                    str.Append("', '");
                    str.Append(marc.Horasextrasdiurna);
                    str.Append("', '");
                    str.Append(marc.Horasfaltas);
                    str.Append("', '");
                    str.Append(marc.Entradaextra);
                    str.Append("', '");
                    str.Append(marc.Saidaextra);
                    str.Append("', '");
                    str.Append(marc.Horastrabalhadasnoturnas);
                    str.Append("', '");
                    str.Append(marc.Horasextranoturna);
                    str.Append("', '");
                    str.Append(marc.Horasfaltanoturna);
                    str.Append("', '");
                    str.Append(marc.Ocorrencia.Length > 60 ? marc.Ocorrencia.Substring(0, 60) : marc.Ocorrencia);
                    str.Append("', ");
                    str.Append(marc.Idhorario);
                    str.Append(", '");
                    str.Append(marc.Bancohorascre);
                    str.Append("', '");
                    str.Append(marc.Bancohorasdeb);
                    str.Append("', ");
                    if (marc.Idfechamentobh > 0)
                    {
                        str.Append(marc.Idfechamentobh);
                    }
                    else
                    {
                        str.Append("NULL");
                    }
                    str.Append(", ");
                    str.Append(marc.Semcalculo);
                    str.Append(", '");
                    str.Append(marc.Ent_num_relogio_1);
                    str.Append("', '");
                    str.Append(marc.Ent_num_relogio_2);
                    str.Append("', '");
                    str.Append(marc.Ent_num_relogio_3);
                    str.Append("', '");
                    str.Append(marc.Ent_num_relogio_4);
                    str.Append("', '");
                    str.Append(marc.Ent_num_relogio_5);
                    str.Append("', '");
                    str.Append(marc.Ent_num_relogio_6);
                    str.Append("', '");
                    str.Append(marc.Ent_num_relogio_7);
                    str.Append("', '");
                    str.Append(marc.Ent_num_relogio_8);
                    str.Append("', '");
                    str.Append(marc.Sai_num_relogio_1);
                    str.Append("', '");
                    str.Append(marc.Sai_num_relogio_2);
                    str.Append("', '");
                    str.Append(marc.Sai_num_relogio_3);
                    str.Append("', '");
                    str.Append(marc.Sai_num_relogio_4);
                    str.Append("', '");
                    str.Append(marc.Sai_num_relogio_5);
                    str.Append("', '");
                    str.Append(marc.Sai_num_relogio_6);
                    str.Append("', '");
                    str.Append(marc.Sai_num_relogio_7);
                    str.Append("', '");
                    str.Append(marc.Sai_num_relogio_8);
                    str.Append("', ");
                    str.Append(marc.Naoentrarbanco);
                    str.Append(", ");
                    str.Append(marc.Naoentrarnacompensacao);
                    str.Append(", '");
                    str.Append(marc.Horascompensadas);
                    str.Append("', ");
                    if (marc.Idcompensado > 0)
                    {
                        str.Append(marc.Idcompensado);
                    }
                    else
                    {
                        str.Append("NULL");
                    }
                    str.Append(", ");
                    str.Append(marc.Naoconsiderarcafe);
                    str.Append(", ");
                    str.Append(marc.Dsr);
                    str.Append(", '");
                    str.Append(marc.Valordsr);
                    str.Append("', ");
                    str.Append(marc.Abonardsr);
                    str.Append(", ");
                    str.Append(marc.Totalizadoresalterados);
                    str.Append(", ");
                    str.Append(marc.Calchorasextrasdiurna);
                    str.Append(", ");
                    str.Append(marc.Calchorasextranoturna);
                    str.Append(", ");
                    str.Append(marc.Calchorasfaltas);
                    str.Append(", ");
                    str.Append(marc.Calchorasfaltanoturna);
                    str.Append(", '");
                    str.Append(marc.Incdata.Value.Month + "/" + marc.Incdata.Value.Day + "/" + marc.Incdata.Value.Year);
                    str.Append("', '");
                    str.Append(marc.Inchora.Value.Month + "/" + marc.Inchora.Value.Day + "/" + marc.Inchora.Value.Year + " " + marc.Inchora.Value.ToLongTimeString());
                    str.Append("', '");
                    str.Append(marc.Incusuario);
                    str.Append("', ");
                    str.Append(marc.Folga);
                    str.Append(", '");
                    str.Append(marc.Chave);
                    str.Append("', '");
                    str.Append(marc.ExpHorasextranoturna);
                    str.Append("', ");
                    str.Append(marc.TipoHoraExtraFalta);
                    str.Append(")");
                    batch.SqlStatements.Add(str.ToString());
                }
                if (batch.SqlStatements.Count > 0)
                {
                    batch.Execute();
                }
            }
        }

        private static void AuxManutencao(FbTransaction trans, Modelo.ModeloBase obj)
        {
            DAL.FB.Afastamento dalAfastamento = FB.Afastamento.GetInstancia;

            switch (((Modelo.Marcacao)obj).Afastamento.Acao)
            {
                case Modelo.Acao.Incluir:
                    dalAfastamento.Incluir(trans, ((Modelo.Marcacao)obj).Afastamento);
                    break;
                case Modelo.Acao.Alterar:
                    dalAfastamento.Alterar(trans, ((Modelo.Marcacao)obj).Afastamento);
                    break;
                case Modelo.Acao.Excluir:
                    dalAfastamento.Excluir(trans, ((Modelo.Marcacao)obj).Afastamento);
                    break;
            }

            DAL.FB.BilhetesImp dalBilhesImp = DAL.FB.BilhetesImp.GetInstancia;
            List<Modelo.BilhetesImp> bilExcluir = new List<Modelo.BilhetesImp>();
            //Executa os comandos por ordem de ação decrescente para excluir antes de incluir ou alterar
            foreach (Modelo.BilhetesImp tmarc in ((Modelo.Marcacao)obj).BilhetesMarcacao.OrderByDescending(m => m.Acao))
            {
                switch (tmarc.Acao)
                {
                    case Modelo.Acao.Incluir:
                        dalBilhesImp.Incluir(trans, tmarc);
                        break;
                    case Modelo.Acao.Alterar:
                        dalBilhesImp.Alterar(trans, tmarc);
                        break;
                    case Modelo.Acao.Excluir:
                        dalBilhesImp.Excluir(trans, tmarc);
                        bilExcluir.Add(tmarc);
                        break;
                }
            }

            foreach (Modelo.BilhetesImp tmarc in bilExcluir)
            {
                ((Modelo.Marcacao)obj).BilhetesMarcacao.Remove(tmarc);
            }

            foreach (Modelo.BilhetesImp bil in ((Modelo.Marcacao)obj).BilhetesMarcacao)
            {
                bil.Acao = Modelo.Acao.Consultar;
            }
        }

        public bool PossuiRegistro(DateTime pDt, int pIdFuncionario)
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@data", FbDbType.Date),
                new FbParameter ("@funcionario", FbDbType.Integer)
            };
            parms[0].Value = pDt;
            parms[1].Value = pIdFuncionario;

            string aux = "SELECT COALESCE(COUNT(\"id\"),0) as \"qtd\" FROM \"marcacao\" WHERE \"idfuncionario\" = @funcionario AND \"data\" = @data";

            int qtd = (int)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);

            if (qtd > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Modelo.MarcacaoLista> GetMarcacaoListaPorFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal)
        {
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idfuncionario", FbDbType.Integer),
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;
            string aux = SELECTPFU;

            aux += " AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);

                Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();
                objMarcLista.Id = objMarcacao.Id;
                objMarcLista.Data = objMarcacao.Data.ToShortDateString();
                objMarcLista.Dia = objMarcacao.Dia;
                objMarcLista.Legenda = objMarcacao.Legenda;
                objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
                objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
                objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
                objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
                objMarcLista.Saida_1 = objMarcacao.Saida_1;
                objMarcLista.Saida_2 = objMarcacao.Saida_2;
                objMarcLista.Saida_3 = objMarcacao.Saida_3;
                objMarcLista.Saida_4 = objMarcacao.Saida_4;
                objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
                objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
                objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
                objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
                objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
                objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
                objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
                objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
                objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;
                objMarcLista.idFechamentoPonto = objMarcacao.IdFechamentoPonto;
                objMarcLista.idFechamentoBH = objMarcacao.Idfechamentobh;

                lista.Add(objMarcLista);

                if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
                {
                    Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                    objMarcLista2.Id = objMarcacao.Id;
                    objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                    objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                    objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                    objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                    objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                    objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                    objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                    objMarcLista2.Saida_4 = objMarcacao.Saida_8;

                    lista.Add(objMarcLista2);
                }
            }

            return lista;
        }

        public DataTable GetParaRelatorioOcorrencia(int pTipo, string pIdentificacao, DateTime pDataI, DateTime pDataF, int pModoOrdenacao, int pAgrupaDepartamento)
        {
            FbParameter[] parms = new FbParameter[]
            { 
                    new FbParameter("@identificacao", FbDbType.VarChar),
                    new FbParameter("@datai", FbDbType.Date),
                    new FbParameter("@dataf", FbDbType.Date)
            };
            
            parms[0].Value = pIdentificacao;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string SQL = "SELECT \"marcacao\".\"id\" "
                        + ", \"marcacao\".\"data\" "
                        + ", \"marcacao\".\"entrada_1\" "
                        + ", \"marcacao\".\"entrada_2\" "
                        + ", \"marcacao\".\"entrada_3\" "
                        + ",\"marcacao\".\"entrada_4\" "
                        + ", \"marcacao\".\"entrada_5\" "
                        + ", \"marcacao\".\"entrada_6\" "
                        + ", \"marcacao\".\"entrada_7\" "
                        + ", \"marcacao\".\"entrada_8\" "
                        + ", \"marcacao\".\"saida_1\" "
                        + ", \"marcacao\".\"saida_2\" "
                        + ", \"marcacao\".\"saida_3\" "
                        + ", \"marcacao\".\"saida_4\" "
                        + ", \"marcacao\".\"saida_5\" "
                        + ", \"marcacao\".\"saida_6\" "
                        + ", \"marcacao\".\"saida_7\" "
                        + ", \"marcacao\".\"saida_8\" "
                        + ", \"marcacao\".\"bancohorascre\" "
                        + ", \"marcacao\".\"bancohorasdeb\" "
                        + ", \"marcacao\".\"ocorrencia\" "
                        + ", \"marcacao\".\"legenda\" "
                        + ", \"marcacao\".\"horasfaltas\" "
                        + ", \"marcacao\".\"horastrabalhadas\" "
                        + ", \"marcacao\".\"horastrabalhadasnoturnas\" "
                        + ", \"funcionario\".\"id\" AS \"idfuncionario\" "
                        + ", \"funcionario\".\"iddepartamento\" "
                        + ", \"funcionario\".\"idempresa\" "
                        + ", \"funcionario\".\"dscodigo\" "
                        + ", \"funcionario\".\"nome\" AS \"funcionario\" "
                        + ", \"departamento\".\"descricao\" AS \"departamento\" "
                        + ", \"empresa\".\"nome\" AS \"empresa\" "
                        + ", case when COALESCE(\"empresa\".\"cnpj\", '') <> '' then \"empresa\".\"cnpj\" else \"empresa\".\"cpf\" end AS \"cnpj_cpf\" "                        
                        + ", \"horario\".\"tipohorario\" "
                        + ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"parametros\".\"thorafalta\", '--:--'))) AS \"thorafalta\" "                        
                        + ", \"horariodetalhenormal\".\"id\" AS \"idhdnormal\" "
                        + ", \"horariodetalhenormal\".\"entrada_1\" AS \"entrada_1normal\" "
                        + ", \"horariodetalhenormal\".\"entrada_2\" AS \"entrada_2normal\" "
                        + ", \"horariodetalhenormal\".\"entrada_3\" AS \"entrada_3normal\" "
                        + ", \"horariodetalhenormal\".\"entrada_4\" AS \"entrada_4normal\" "
                        + ", \"horariodetalhenormal\".\"saida_1\" AS \"saida_1normal\" "
                        + ", \"horariodetalhenormal\".\"saida_2\" AS \"saida_2normal\" "
                        + ", \"horariodetalhenormal\".\"saida_3\" AS \"saida_3normal\" "
                        + ", \"horariodetalhenormal\".\"saida_4\" AS \"saida_4normal\" "
                        + ", \"horariodetalheflexivel\".\"id\" AS \"idhdflexivel\" "
                        + ", \"horariodetalheflexivel\".\"entrada_1\" AS \"entrada_1flexivel\" "
                        + ", \"horariodetalheflexivel\".\"entrada_2\" AS \"entrada_2flexivel\" "
                        + ", \"horariodetalheflexivel\".\"entrada_3\" AS \"entrada_3flexivel\" "
                        + ", \"horariodetalheflexivel\".\"entrada_4\" AS \"entrada_4flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_1\" AS \"saida_1flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_2\" AS \"saida_2flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_3\" AS \"saida_3flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_4\" AS \"saida_4flexivel\" "
                        + " FROM \"marcacao\" "
                        + " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" AND \"funcionario\".\"funcionarioativo\" = 1 "
                        + " INNER JOIN \"horario\" ON \"horario\".\"id\" = \"marcacao\".\"idhorario\" "
                        + " INNER JOIN \"parametros\" ON \"parametros\".\"id\" = \"horario\".\"idparametro\" "                        
                        + " INNER JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" "
                        + " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" "                        
                        + " LEFT JOIN \"horariodetalhe\" \"horariodetalhenormal\" ON \"horariodetalhenormal\".\"idhorario\" = \"marcacao\".\"idhorario\" "
                        + " AND \"horario\".\"tipohorario\" = 1 AND \"horariodetalhenormal\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") END) "
                        + " LEFT JOIN \"horariodetalhe\" \"horariodetalheflexivel\" ON \"horariodetalheflexivel\".\"idhorario\" = \"marcacao\".\"idhorario\" "
                        + " AND \"horario\".\"tipohorario\" = 2 AND \"horariodetalheflexivel\".\"data\" = \"marcacao\".\"data\" "
                        + " WHERE \"marcacao\".\"data\" >= @datai AND \"marcacao\".\"data\" <= @dataf ";

            switch (pTipo)
            {
                //Empresa
                case 0:
                    SQL += "AND \"funcionario\".\"idempresa\" IN @identificacao";
                    break;
                //Departamento
                case 1:
                    SQL += "AND \"funcionario\".\"iddepartamento\" IN @identificacao";
                    break;
                //Individual
                case 2:
                    SQL += "AND \"funcionario\".\"id\" IN @identificacao";
                    break;
            }

            SQL += " ORDER BY \"funcionario\".\"nome\", \"marcacao\".\"data\"";

            DataTable dt = new DataTable();
            dt.Load(DataBase.ExecuteReader(CommandType.Text, SQL, parms));

            return dt;
        }

        public DataTable GetParaTotalizaHoras(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idfuncionario", FbDbType.Integer),
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = "SELECT \"marcacao\".\"id\" " +
                ", \"marcacao\".\"idhorario\" " +
                ", \"marcacao\".\"horasfaltas\" " +
                ", \"funcionario\".\"idfuncao\" " +
                ", \"funcionario\".\"idempresa\" " +
                ", \"funcionario\".\"iddepartamento\" " +
                ", \"marcacao\".\"idfuncionario\" " +
                ", \"marcacao\".\"horasfaltanoturna\" " +
                ", \"marcacao\".\"horasextranoturna\" " +
                ", \"marcacao\".\"horasextrasdiurna\" " +
                ", \"marcacao\".\"horastrabalhadas\" " +
                ", \"marcacao\".\"horastrabalhadasnoturnas\" " +
                ", COALESCE(\"marcacao\".\"valordsr\", '') AS \"valordsr\" " +
                ", \"marcacao\".\"legenda\" " +
                ", COALESCE(\"marcacao\".\"bancohorascre\", '---:--') AS \"bancohorascre\" " +
                ", COALESCE(\"marcacao\".\"bancohorasdeb\", '---:--') AS \"bancohorasdeb\" " +
                ", \"marcacao\".\"dia\" " +
                ", \"marcacao\".\"data\" " +
                ", \"marcacao\".\"folga\" " +
                ", \"horario\".\"tipohorario\" " +
                ", \"horario\".\"considerasabadosemana\" " +
                ", \"horario\".\"consideradomingosemana\" " +
                ", \"horario\".\"tipoacumulo\" " +
                ", \"horariophextra50\".\"percentualextra\" AS \"percentualextra50\" " +
                ", \"horariophextra50\".\"quantidadeextra\" AS \"quantidadeextra50\" " +
                ", \"horariophextra60\".\"percentualextra\" AS \"percentualextra60\" " +
                ", \"horariophextra60\".\"quantidadeextra\" AS \"quantidadeextra60\" " +
                ", \"horariophextra70\".\"percentualextra\" AS \"percentualextra70\" " +
                ", \"horariophextra70\".\"quantidadeextra\" AS \"quantidadeextra70\" " +
                ", \"horariophextra80\".\"percentualextra\" AS \"percentualextra80\" " +
                ", \"horariophextra80\".\"quantidadeextra\" AS \"quantidadeextra80\" " +
                ", \"horariophextra90\".\"percentualextra\" AS \"percentualextra90\" " +
                ", \"horariophextra90\".\"quantidadeextra\" AS \"quantidadeextra90\" " +
                ", \"horariophextra100\".\"percentualextra\" AS \"percentualextra100\" " +
                ", \"horariophextra100\".\"quantidadeextra\" AS \"quantidadeextra100\" " +
                ", \"horariophextrasab\".\"percentualextra\" AS \"percentualextrasab\" " +
                ", \"horariophextrasab\".\"quantidadeextra\" AS \"quantidadeextrasab\" " +
                ", \"horariophextradom\".\"percentualextra\" AS \"percentualextradom\" " +
                ", \"horariophextradom\".\"quantidadeextra\" AS \"quantidadeextradom\" " +
                ", \"horariophextrafer\".\"percentualextra\" AS \"percentualextrafer\" " +
                ", \"horariophextrafer\".\"quantidadeextra\" AS \"quantidadeextrafer\" " +
                ", \"horariophextrafol\".\"percentualextra\" AS \"percentualextrafol\" " +
                ", \"horariophextrafol\".\"quantidadeextra\" AS \"quantidadeextrafol\" " +
                ", \"horariodetalhenormal\".\"totaltrabalhadadiurna\" AS \"chdiurnanormal\" " +
                ", \"horariodetalhenormal\".\"totaltrabalhadanoturna\" AS \"chnoturnanormal\" " +
                ", \"horariodetalhenormal\".\"flagfolga\" AS \"flagfolganormal\" " +
                ", \"horariodetalhenormal\".\"cargahorariamista\" AS \"cargamistanormal\" " +
                ", \"horariodetalheflexivel\".\"totaltrabalhadadiurna\" AS \"chdiurnaflexivel\" " +
                ", \"horariodetalheflexivel\".\"totaltrabalhadanoturna\" AS \"chnoturnaflexivel\" " +
                ", \"horariodetalheflexivel\".\"flagfolga\" AS \"flagfolgaflexivel\" " +
                ", \"horariodetalheflexivel\".\"cargahorariamista\" AS \"cargamistaflexivel\" " +
                ", COALESCE(\"marcacao\".\"exphorasextranoturna\", '--:--') AS \"exphorasextranoturna\" " +
                " FROM \"marcacao\" " +
                " INNER JOIN \"horario\" ON \"horario\".\"id\" = \"marcacao\".\"idhorario\"" +
                " INNER JOIN \"horariophextra\" \"horariophextra50\" ON \"horariophextra50\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra50\".\"codigo\" = 0 " +
                " INNER JOIN \"horariophextra\" \"horariophextra60\" ON \"horariophextra60\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra60\".\"codigo\" = 1 " +
                " INNER JOIN \"horariophextra\" \"horariophextra70\" ON \"horariophextra70\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra70\".\"codigo\" = 2 " +
                " INNER JOIN \"horariophextra\" \"horariophextra80\" ON \"horariophextra80\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra80\".\"codigo\" = 3 " +
                " INNER JOIN \"horariophextra\" \"horariophextra90\" ON \"horariophextra90\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra90\".\"codigo\" = 4 " +
                " INNER JOIN \"horariophextra\" \"horariophextra100\" ON \"horariophextra100\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra100\".\"codigo\" = 5 " +
                " INNER JOIN \"horariophextra\" \"horariophextrasab\" ON \"horariophextrasab\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextrasab\".\"codigo\" = 6 " +
                " INNER JOIN \"horariophextra\" \"horariophextradom\" ON \"horariophextradom\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextradom\".\"codigo\" = 7 " +
                " INNER JOIN \"horariophextra\" \"horariophextrafer\" ON \"horariophextrafer\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextrafer\".\"codigo\" = 8 " +
                " INNER JOIN \"horariophextra\" \"horariophextrafol\" ON \"horariophextrafol\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextrafol\".\"codigo\" = 9 " +
                " LEFT JOIN \"horariodetalhe\" \"horariodetalhenormal\" ON \"horariodetalhenormal\".\"idhorario\" = \"marcacao\".\"idhorario\" " +
                " AND \"horario\".\"tipohorario\" = 1 AND \"horariodetalhenormal\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") END) " +
                " LEFT JOIN \"horariodetalhe\" \"horariodetalheflexivel\" ON \"horariodetalheflexivel\".\"idhorario\" = \"marcacao\".\"idhorario\" " +
                " AND \"horario\".\"tipohorario\" = 2 AND \"horariodetalheflexivel\".\"data\" = \"marcacao\".\"data\" " +
                " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                " WHERE \"marcacao\".\"idfuncionario\" = @idfuncionario ";

            if (PegaInativos)
            {
                aux += " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";
            }
            else
            {
                aux += " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"funcionarioativo\" = 1 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";
            }

            DataTable dt = new DataTable();

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public DataTable GetParaACJEF(int pIdEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idempresa", FbDbType.Integer),
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pIdEmpresa;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = "SELECT \"marcacao\".\"id\" " +
                ", \"marcacao\".\"idhorario\" " +
                ", \"marcacao\".\"entrada_1\" " +
                ", \"marcacao\".\"horasfaltas\" " +
                ", \"marcacao\".\"horasfaltanoturna\" " +
                ", \"marcacao\".\"horasextranoturna\" " +
                ", \"marcacao\".\"horasextrasdiurna\" " +
                ", \"marcacao\".\"horastrabalhadas\" " +
                ", \"marcacao\".\"horastrabalhadasnoturnas\" " +
                ", COALESCE(\"marcacao\".\"valordsr\", '') AS \"valordsr\" " +
                ", \"marcacao\".\"legenda\" " +
                ", COALESCE(\"marcacao\".\"bancohorascre\", '---:--') AS \"bancohorascre\" " +
                ", COALESCE(\"marcacao\".\"bancohorasdeb\", '---:--') AS \"bancohorasdeb\" " +
                ", \"marcacao\".\"dia\" " +
                ", \"marcacao\".\"data\" " +
                ", \"marcacao\".\"folga\" " +
                ", \"horario\".\"tipohorario\" " +
                ", \"jornadanormal\".\"codigo\" AS \"codigohorario\" " +
                ", \"jornadaflexivel\".\"codigo\" AS \"codigohorario1\" " +
                ", \"horario\".\"considerasabadosemana\" " +
                ", \"horario\".\"consideradomingosemana\" " +
                ", \"horario\".\"tipoacumulo\" " +
                ", \"horariophextra50\".\"percentualextra\" AS \"percentualextra50\" " +
                ", \"horariophextra50\".\"quantidadeextra\" AS \"quantidadeextra50\" " +
                ", \"horariophextra60\".\"percentualextra\" AS \"percentualextra60\" " +
                ", \"horariophextra60\".\"quantidadeextra\" AS \"quantidadeextra60\" " +
                ", \"horariophextra70\".\"percentualextra\" AS \"percentualextra70\" " +
                ", \"horariophextra70\".\"quantidadeextra\" AS \"quantidadeextra70\" " +
                ", \"horariophextra80\".\"percentualextra\" AS \"percentualextra80\" " +
                ", \"horariophextra80\".\"quantidadeextra\" AS \"quantidadeextra80\" " +
                ", \"horariophextra90\".\"percentualextra\" AS \"percentualextra90\" " +
                ", \"horariophextra90\".\"quantidadeextra\" AS \"quantidadeextra90\" " +
                ", \"horariophextra100\".\"percentualextra\" AS \"percentualextra100\" " +
                ", \"horariophextra100\".\"quantidadeextra\" AS \"quantidadeextra100\" " +
                ", \"horariophextrasab\".\"percentualextra\" AS \"percentualextrasab\" " +
                ", \"horariophextrasab\".\"quantidadeextra\" AS \"quantidadeextrasab\" " +
                ", \"horariophextradom\".\"percentualextra\" AS \"percentualextradom\" " +
                ", \"horariophextradom\".\"quantidadeextra\" AS \"quantidadeextradom\" " +
                ", \"horariophextrafer\".\"percentualextra\" AS \"percentualextrafer\" " +
                ", \"horariophextrafer\".\"quantidadeextra\" AS \"quantidadeextrafer\" " +
                ", \"horariophextrafol\".\"percentualextra\" AS \"percentualextrafol\" " +
                ", \"horariophextrafol\".\"quantidadeextra\" AS \"quantidadeextrafol\" " +
                ", \"horariodetalhenormal\".\"totaltrabalhadadiurna\" AS \"chdiurnanormal\" " +
                ", \"horariodetalhenormal\".\"totaltrabalhadanoturna\" AS \"chnoturnanormal\" " +
                ", \"horariodetalhenormal\".\"flagfolga\" AS \"flagfolganormal\" " +
                ", \"horariodetalheflexivel\".\"totaltrabalhadadiurna\" AS \"chdiurnaflexivel\" " +
                ", \"horariodetalheflexivel\".\"totaltrabalhadanoturna\" AS \"chnoturnaflexivel\" " +
                ", \"horariodetalheflexivel\".\"flagfolga\" AS \"flagfolgaflexivel\" " +
                ", \"funcionario\".\"pis\" " +
                ", \"funcionario\".\"idempresa\" " +
                ", \"funcionario\".\"iddepartamento\" " +
                ", \"funcionario\".\"idfuncao\" " +
                ", COALESCE(\"marcacao\".\"exphorasextranoturna\", '--:--') AS \"exphorasextranoturna\" " + 
                " FROM \"marcacao\" " +
                " INNER JOIN \"horario\" ON \"horario\".\"id\" = \"marcacao\".\"idhorario\"" +
                " INNER JOIN \"horariophextra\" \"horariophextra50\" ON \"horariophextra50\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra50\".\"codigo\" = 0 " +
                " INNER JOIN \"horariophextra\" \"horariophextra60\" ON \"horariophextra60\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra60\".\"codigo\" = 1 " +
                " INNER JOIN \"horariophextra\" \"horariophextra70\" ON \"horariophextra70\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra70\".\"codigo\" = 2 " +
                " INNER JOIN \"horariophextra\" \"horariophextra80\" ON \"horariophextra80\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra80\".\"codigo\" = 3 " +
                " INNER JOIN \"horariophextra\" \"horariophextra90\" ON \"horariophextra90\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra90\".\"codigo\" = 4 " +
                " INNER JOIN \"horariophextra\" \"horariophextra100\" ON \"horariophextra100\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra100\".\"codigo\" = 5 " +
                " INNER JOIN \"horariophextra\" \"horariophextrasab\" ON \"horariophextrasab\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextrasab\".\"codigo\" = 6 " +
                " INNER JOIN \"horariophextra\" \"horariophextradom\" ON \"horariophextradom\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextradom\".\"codigo\" = 7 " +
                " INNER JOIN \"horariophextra\" \"horariophextrafer\" ON \"horariophextrafer\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextrafer\".\"codigo\" = 8 " +
                " INNER JOIN \"horariophextra\" \"horariophextrafol\" ON \"horariophextrafol\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextrafol\".\"codigo\" = 9 " +
                " LEFT JOIN \"horariodetalhe\" \"horariodetalhenormal\" ON \"horariodetalhenormal\".\"idhorario\" = \"marcacao\".\"idhorario\" " +
                " AND \"horario\".\"tipohorario\" = 1 AND \"horariodetalhenormal\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") END) " +
                " LEFT JOIN \"horariodetalhe\" \"horariodetalheflexivel\" ON \"horariodetalheflexivel\".\"idhorario\" = \"marcacao\".\"idhorario\" " +
                " AND \"horario\".\"tipohorario\" = 2 AND \"horariodetalheflexivel\".\"data\" = \"marcacao\".\"data\" " +
                " LEFT JOIN \"jornada\" \"jornadanormal\" ON \"jornadanormal\".\"id\" = \"horariodetalhenormal\".\"idjornada\" AND \"horario\".\"tipohorario\" = 1 " +
                " LEFT JOIN \"jornada\" \"jornadaflexivel\" ON \"jornadaflexivel\".\"id\" = \"horariodetalheflexivel\".\"idjornada\" AND \"horario\".\"tipohorario\" = 2 " +
                " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                " WHERE \"funcionario\".\"idempresa\" = @idempresa ";

            if (PegaInativos)
            {
                aux += " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal";
            }
            else
            {
                aux += " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"funcionarioativo\" = 1 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal";
            }

            aux += " ORDER BY \"funcionario\".\"id\", \"marcacao\".\"data\"";

            DataTable dt = new DataTable();

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public List<Modelo.Marcacao> GetPorFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idfuncionario", FbDbType.Integer),
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;
            string aux = SELECTPFU;

            if (PegaInativos)
            {
                aux += " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";
            }
            else
            {
                aux += " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"funcionarioativo\" = 1 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";
            }
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            DAL.FB.BilhetesImp dalBilhetesImp = DAL.FB.BilhetesImp.GetInstancia;
            List<Modelo.BilhetesImp> tratamentos = dalBilhetesImp.GetImportadosFunc(pIdFuncionario);
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }

            return lista;
        }

        public List<Modelo.Marcacao> GetPorFuncionario(int pIdFuncionario)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                    new FbParameter("@idfuncionario", FbDbType.Integer)
            };
            parms[0].Value = pIdFuncionario;

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, SELECTPFU, parms);

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            DAL.FB.BilhetesImp dalBilhetesImp = DAL.FB.BilhetesImp.GetInstancia;
            List<Modelo.BilhetesImp> tratamentos = dalBilhetesImp.GetImportadosFunc(pIdFuncionario);
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }

            return lista;
        }

        /// <summary>
        /// este metodo é usado pelas seguintes classes: BLLBancoHoras(atualizadadosalterados,atualizadadosanterior),BLLJornadaAlternativa(atualizadadosalterados,atualizadadosanterior)
        /// BLLFeriado(atualizadadosalterados,atualizadadosanterior),BLLcompensacao(CalculaTotalCompensado),BLLInclusaobanco(atualizadadosalterados,atualizadadosanterior)
        /// BLLAfastamento(atualizadadosalterados,atualizadadosanterior), FormManutDiaria(CarregaGrid).
        ///
        /// </summary>
        /// <param name="pEmpresa"></param>
        /// <param name="pdataInicial"></param>
        /// <param name="pDataFinal"></param>
        /// <param name="PegaInativos"></param>
        /// <returns></returns>
        public List<Modelo.Marcacao> GetPorEmpresa(int pEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            string aux;
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idempresa", FbDbType.Integer),
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pEmpresa;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            if (PegaInativos)
            {
                aux = "SELECT \"marcacao\".* " +
                                  ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                            " FROM \"marcacao\" " +
                            " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                            " WHERE \"funcionario\".\"idempresa\" = @idempresa " +
                            " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";
            }
            else
            {
                aux = "SELECT \"marcacao\".* " +
                                   ", \"funcionario\".\"nome\" AS \"funcionario\"" +
                             " FROM \"marcacao\"" +
                             " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\"" +
                             " WHERE \"funcionario\".\"idempresa\" = @idempresa" +
                             " AND \"funcionario\".\"funcionarioativo\" = 1 AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"funcionario\".\"datademissao\" IS NULL    AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";
            }
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            BilhetesImp dalBilhetesImp = DAL.FB.BilhetesImp.GetInstancia;
            List<Modelo.BilhetesImp> tratamentos = dalBilhetesImp.GetImportadosPeriodo(0, pEmpresa, pdataInicial, pDataFinal);
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == objMarcacao.Dscodigo && t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }

            return lista;
        }

        public List<Modelo.MarcacaoLista> GetPorEmpresaList(int pEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            string aux;
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idempresa", FbDbType.Integer),
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pEmpresa;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            if (PegaInativos)
            {
                aux = "SELECT \"marcacao\".* " +
                                  ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                            " FROM \"marcacao\" " +
                            " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                            " WHERE \"funcionario\".\"idempresa\" = @idempresa " +
                            " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";
            }
            else
            {
                aux = "SELECT \"marcacao\".* " +
                                   ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                             " FROM \"marcacao\" " +
                             " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                             " WHERE \"funcionario\".\"idempresa\" = @idempresa " +
                             " AND \"funcionarioativo\" = 1 AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";
            }
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);

                Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();

                objMarcLista.Id = objMarcacao.Id;
                objMarcLista.Funcionario = objMarcacao.Funcionario;
                objMarcLista.Legenda = objMarcacao.Legenda;
                objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
                objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
                objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
                objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
                objMarcLista.Saida_1 = objMarcacao.Saida_1;
                objMarcLista.Saida_2 = objMarcacao.Saida_2;
                objMarcLista.Saida_3 = objMarcacao.Saida_3;
                objMarcLista.Saida_4 = objMarcacao.Saida_4;
                objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
                objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
                objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
                objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
                objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
                objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
                objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
                objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
                objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;
                objMarcLista.idFechamentoBH = objMarcacao.Idfechamentobh;
                objMarcLista.idFechamentoPonto = objMarcacao.IdFechamentoPonto;

                lista.Add(objMarcLista);

                if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
                {
                    Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                    objMarcLista2.Id = objMarcacao.Id;
                    objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                    objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                    objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                    objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                    objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                    objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                    objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                    objMarcLista2.Saida_4 = objMarcacao.Saida_8;

                    lista.Add(objMarcLista2);
                }
            }

            return lista;
        }

        /// <summary>
        /// metodo usado para tabela de manutencao diaria
        /// </summary>
        /// <param name="pEmpresa"></param>
        /// <param name="pdataInicial"></param>
        /// <param name="pDataFinal"></param>
        /// <param name="PegaInativos"></param>
        /// <returns></returns>
        public List<Modelo.MarcacaoLista> GetPorManutDiariaEmp(int pEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            string aux;
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idempresa", FbDbType.Integer),
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pEmpresa;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            if (PegaInativos)
            {
                aux = "SELECT \"marcacao\".* " +
                                  ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                            " FROM \"marcacao\" " +
                            " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                            " WHERE \"funcionario\".\"idempresa\" = @idempresa " +
                            " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"funcionario\".\"nome\"";
            }
            else
            {
                aux = "SELECT \"marcacao\".* " +
                                   ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                             " FROM \"marcacao\" " +
                             " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                             " WHERE \"funcionario\".\"idempresa\" = @idempresa " +
                             " AND \"funcionarioativo\" = 1 AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"funcionario\".\"nome\"";
            }
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);

                Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();

                objMarcLista.Id = objMarcacao.Id;
                objMarcLista.IdFuncionario = objMarcacao.Idfuncionario;
                objMarcLista.Funcionario = objMarcacao.Funcionario;
                objMarcLista.Legenda = objMarcacao.Legenda;
                objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
                objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
                objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
                objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
                objMarcLista.Saida_1 = objMarcacao.Saida_1;
                objMarcLista.Saida_2 = objMarcacao.Saida_2;
                objMarcLista.Saida_3 = objMarcacao.Saida_3;
                objMarcLista.Saida_4 = objMarcacao.Saida_4;
                objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
                objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
                objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
                objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
                objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
                objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
                objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
                objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
                objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;

                lista.Add(objMarcLista);

                if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
                {
                    Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                    objMarcLista2.Id = objMarcacao.Id;
                    objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                    objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                    objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                    objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                    objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                    objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                    objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                    objMarcLista2.Saida_4 = objMarcacao.Saida_8;

                    lista.Add(objMarcLista2);
                }
            }

            return lista;
        }

        public List<Modelo.Marcacao> GetPorDepartamento(int pDepartamento, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@iddepartamento", FbDbType.Integer),
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pDepartamento;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = "SELECT \"marcacao\".* " +
                               ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                         " FROM \"marcacao\" " +
                         " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                         " WHERE \"funcionario\".\"iddepartamento\" = @iddepartamento " +
                         " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND  \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ";

            if (!PegaInativos)
            {
                aux += "AND \"funcionario\".\"datademissao\" IS NULL AND \"funcionario\".\"funcionarioativo\" = 1 ";
            }

            aux += "ORDER BY \"marcacao\".\"data\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            BilhetesImp dalBilhetesImp = DAL.FB.BilhetesImp.GetInstancia;
            List<Modelo.BilhetesImp> tratamentos = dalBilhetesImp.GetImportadosPeriodo(1, pDepartamento, pdataInicial, pDataFinal);
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == objMarcacao.Dscodigo && t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }

            return lista;
        }

        /// <summary>
        ///  este metodo retorna os funcionarios de determinado departamento.
        /// </summary>
        /// <param name="pIdDepartamento"></param>
        /// <param name="pDataFinal"></param>
        /// <param name="PegaInativos"></param>
        /// <returns></returns>
        public List<Modelo.MarcacaoLista> GetPorDepartamentoList(int pIdDepartamento, DateTime pDataFinal, bool PegaInativos)
        {
            FbParameter[] parms = new FbParameter[2]
            { 
                   
                new FbParameter("@iddepartamento", FbDbType.Integer),
                new FbParameter("@datafinal", FbDbType.Date)
            };

            parms[0].Value = pIdDepartamento;
            parms[1].Value = pDataFinal;
            string aux = SELECTDEP;

            if (PegaInativos)
            {
                aux += " AND \"marcacao\".\"data\" = @datafinal ORDER BY \"marcacao\".\"data\"";
            }
            else
            {
                aux += " AND \"funcionarioativo\" = 1 AND \"marcacao\".\"data\" = @datafinal ORDER BY \"marcacao\".\"data\"";
            }


            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);

                Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();
                objMarcLista.Id = objMarcacao.Id;
                objMarcLista.Funcionario = objMarcacao.Funcionario;
                objMarcLista.Legenda = objMarcacao.Legenda;
                objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
                objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
                objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
                objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
                objMarcLista.Saida_1 = objMarcacao.Saida_1;
                objMarcLista.Saida_2 = objMarcacao.Saida_2;
                objMarcLista.Saida_3 = objMarcacao.Saida_3;
                objMarcLista.Saida_4 = objMarcacao.Saida_4;
                objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
                objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
                objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
                objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
                objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
                objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
                objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
                objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
                objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;

                lista.Add(objMarcLista);

                if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
                {
                    Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                    objMarcLista2.Id = objMarcacao.Id;
                    objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                    objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                    objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                    objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                    objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                    objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                    objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                    objMarcLista2.Saida_4 = objMarcacao.Saida_8;

                    lista.Add(objMarcLista2);
                }
            }

            return lista;
        }

        /// <summary>
        /// metodo usado para tabela de manutencao diaria
        /// </summary>
        /// <param name="pIdDepartamento"></param>
        /// <param name="pDataFinal"></param>
        /// <param name="PegaInativos"></param>
        /// <returns></returns>
        public List<Modelo.MarcacaoLista> GetPorManutDiariaDep(int pIdDepartamento, DateTime pDataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            FbParameter[] parms = new FbParameter[2]
            { 
                   
                new FbParameter("@iddepartamento", FbDbType.Integer),
                new FbParameter("@datafinal", FbDbType.Date)
            };

            parms[0].Value = pIdDepartamento;
            parms[1].Value = pDataInicial;
            string aux = SELECTDEP;

            if (PegaInativos)
            {
                aux += " AND \"marcacao\".\"data\" = @datafinal ORDER BY \"funcionario\".\"nome\"";
            }
            else
            {
                aux += " AND \"funcionarioativo\" = 1 AND \"marcacao\".\"data\" = @datafinal ORDER BY \"funcionario\".\"nome\"";
            }


            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);

                Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();
                objMarcLista.Id = objMarcacao.Id;
                objMarcLista.IdFuncionario = objMarcacao.Idfuncionario;
                objMarcLista.Funcionario = objMarcacao.Funcionario;
                objMarcLista.Legenda = objMarcacao.Legenda;
                objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
                objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
                objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
                objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
                objMarcLista.Saida_1 = objMarcacao.Saida_1;
                objMarcLista.Saida_2 = objMarcacao.Saida_2;
                objMarcLista.Saida_3 = objMarcacao.Saida_3;
                objMarcLista.Saida_4 = objMarcacao.Saida_4;
                objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
                objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
                objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
                objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
                objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
                objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
                objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
                objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
                objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;

                lista.Add(objMarcLista);

                if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
                {
                    Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                    objMarcLista2.Id = objMarcacao.Id;
                    objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                    objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                    objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                    objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                    objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                    objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                    objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                    objMarcLista2.Saida_4 = objMarcacao.Saida_8;

                    lista.Add(objMarcLista2);
                }
            }

            return lista;
        }        

        public List<Modelo.Marcacao> GetPorFuncao(int pIdFuncao, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idfuncao", FbDbType.Integer),
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pIdFuncao;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = "SELECT \"marcacao\".* " +
                               ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                         " FROM \"marcacao\" " +
                         " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                         " WHERE \"funcionario\".\"idfuncao\" = @idfuncao";

            if (!PegaInativos)
            {
                aux += " AND \"funcionario\".\"funcionarioativo\" = 1";
            }

            aux += " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            BilhetesImp dalBilhetesImp = DAL.FB.BilhetesImp.GetInstancia;
            List<Modelo.BilhetesImp> tratamentos = dalBilhetesImp.GetImportadosPeriodo(3, pIdFuncao, pdataInicial, pDataFinal);
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == objMarcacao.Dscodigo && t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }

            return lista;
        }

        public List<Modelo.Marcacao> GetPorPeriodo(DateTime pdataInicial, DateTime pDataFinal)
        {
            FbParameter[] parms = new FbParameter[2]
            { 
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pdataInicial;
            parms[1].Value = pDataFinal;

            string aux = "SELECT \"marcacao\".* " +
                               ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                         " FROM \"marcacao\" " +
                         " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                         " WHERE COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            BilhetesImp dalBilhetesImp = DAL.FB.BilhetesImp.GetInstancia;
            List<Modelo.BilhetesImp> tratamentos = dalBilhetesImp.GetImportadosPeriodo(5, 0, pdataInicial, pDataFinal);
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == objMarcacao.Dscodigo && t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }

            return lista;
        }

        public List<Modelo.Marcacao> GetPorHorario(int pIdHorario, DateTime pdataInicial, DateTime pDataFinal)
        {
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idhorario", FbDbType.Integer),
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = pIdHorario;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = "SELECT \"marcacao\".* " +
                               ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                         " FROM \"marcacao\" " +
                         " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                         " WHERE \"marcacao\".\"idhorario\" = @idhorario" +
                         " AND COALESCE(\"funcionario\".\"excluido\",0) = 0 AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ORDER BY \"marcacao\".\"data\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            BilhetesImp dalBilhetesImp = DAL.FB.BilhetesImp.GetInstancia;
            List<Modelo.BilhetesImp> tratamentos = dalBilhetesImp.GetImportadosPeriodo(4, pIdHorario, pdataInicial, pDataFinal);
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                objMarcacao.BilhetesMarcacao = tratamentos.Where(t => t.DsCodigo == objMarcacao.Dscodigo && t.Mar_data == objMarcacao.Data).ToList();
                lista.Add(objMarcacao);
            }
            return lista;
        }

        public Modelo.Marcacao GetPorData(Modelo.Funcionario pFuncionario, DateTime pData)
        {
            FbParameter[] parms = new FbParameter[]
            { 
                    new FbParameter("@idfuncionario", FbDbType.Integer),
                    new FbParameter("@data", FbDbType.Date)
            };
            parms[0].Value = pFuncionario.Id;
            parms[1].Value = pData;

            string aux = "SELECT \"marcacao\".*, \"funcionario\".\"nome\" AS \"funcionario\" FROM \"marcacao\" LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" WHERE \"idfuncionario\" = @idfuncionario AND \"data\" = @data";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
            if (dr.HasRows)
            {
                SetInstance(dr, objMarcacao);
            }
            return objMarcacao;
        }

        /// <summary>
        /// metodo usado para tabela de manutencao diaria 
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="PegaInativos"></param>
        /// <returns></returns>
        public List<Modelo.MarcacaoLista> GetPorDataManutDiaria(DateTime pDataIni, DateTime pDataFin, bool PegaInativos)
        {
            string aux;

            List<Modelo.MarcacaoLista> lista = new List<Modelo.MarcacaoLista>();
            FbParameter[] parms = new FbParameter[]
            { 
                   new FbParameter("@data", SqlDbType.DateTime)
            };

            parms[0].Value = pDataIni;

            if (PegaInativos)
            {
                aux = "SELECT \"marcacao\".*, \"funcionario\".\"nome\" AS \"funcionario\" FROM \"marcacao\" INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" WHERE \"data\" = @data AND COALESCE(\"funcionario\".\"excluido\",0) = 0 ORDER BY \"funcionario\".\"nome\"";
            }
            else
            {
                aux = "SELECT \"marcacao\".*, \"funcionario\".\"nome\" AS \"funcionario\" FROM \"marcacao\" INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" WHERE \"funcionarioativo\" = 1 AND \"data\" = @data AND COALESCE(\"funcionario\".\"excluido\",0) = 0 ORDER BY \"funcionario\".\"nome\"";
            }
            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);

                Modelo.MarcacaoLista objMarcLista = new Modelo.MarcacaoLista();
                objMarcLista.Id = objMarcacao.Id;
                objMarcLista.IdFuncionario = objMarcacao.Idfuncionario;
                objMarcLista.Funcionario = objMarcacao.Funcionario;
                objMarcLista.Legenda = objMarcacao.Legenda;
                objMarcLista.Entrada_1 = objMarcacao.Entrada_1;
                objMarcLista.Entrada_2 = objMarcacao.Entrada_2;
                objMarcLista.Entrada_3 = objMarcacao.Entrada_3;
                objMarcLista.Entrada_4 = objMarcacao.Entrada_4;
                objMarcLista.Saida_1 = objMarcacao.Saida_1;
                objMarcLista.Saida_2 = objMarcacao.Saida_2;
                objMarcLista.Saida_3 = objMarcacao.Saida_3;
                objMarcLista.Saida_4 = objMarcacao.Saida_4;
                objMarcLista.Horasextrasdiurna = objMarcacao.Horasextrasdiurna;
                objMarcLista.Horasextranoturna = objMarcacao.Horasextranoturna;
                objMarcLista.Horasfaltas = objMarcacao.Horasfaltas;
                objMarcLista.Horasfaltanoturna = objMarcacao.Horasfaltanoturna;
                objMarcLista.Horastrabalhadas = objMarcacao.Horastrabalhadas;
                objMarcLista.Horastrabalhadasnoturnas = objMarcacao.Horastrabalhadasnoturnas;
                objMarcLista.Bancohorascre = objMarcacao.Bancohorascre;
                objMarcLista.Bancohorasdeb = objMarcacao.Bancohorasdeb;
                objMarcLista.Ocorrencia = objMarcacao.Ocorrencia;

                lista.Add(objMarcLista);

                if (objMarcacao.Entrada_5 != "--:--" && !String.IsNullOrEmpty(objMarcacao.Entrada_5))
                {
                    Modelo.MarcacaoLista objMarcLista2 = new Modelo.MarcacaoLista();
                    objMarcLista2.Id = objMarcacao.Id;
                    objMarcLista2.Entrada_1 = objMarcacao.Entrada_5;
                    objMarcLista2.Entrada_2 = objMarcacao.Entrada_6;
                    objMarcLista2.Entrada_3 = objMarcacao.Entrada_7;
                    objMarcLista2.Entrada_4 = objMarcacao.Entrada_8;
                    objMarcLista2.Saida_1 = objMarcacao.Saida_5;
                    objMarcLista2.Saida_2 = objMarcacao.Saida_6;
                    objMarcLista2.Saida_3 = objMarcacao.Saida_7;
                    objMarcLista2.Saida_4 = objMarcacao.Saida_8;

                    lista.Add(objMarcLista2);
                }
            }

            return lista;
        }        

        public List<Modelo.Marcacao> GetListaFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal)
        {
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idfuncionario", FbDbType.Integer),
                    new FbParameter("@dataInicial", FbDbType.Date),
                    new FbParameter("@dataFinal", FbDbType.Date)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pdataInicial;
            parms[2].Value = pDataFinal;

            string aux = SELECTPFU;

            aux += " AND \"marcacao\".\"data\" >= @dataInicial AND \"marcacao\".\"data\" <= @dataFinal ORDER BY \"marcacao\".\"data\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
            }

            return lista;
        }

        public void ClearFechamentoBH(int pIdFechamentoBH)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                    new FbParameter("@idfechamentobh", FbDbType.Integer)
            };
            parms[0].Value = pIdFechamentoBH;

            string aux = " UPDATE \"marcacao\" " +
                         " SET \"idfechamentobh\" = NULL" +
                         " WHERE \"idfechamentobh\" = @idfechamentobh" +
                         " AND \"naoentrarbanco\" = 0";

            FbCommand cmd = FB.DataBase.ExecNonQueryCmd(CommandType.Text, aux, true, parms);
        }

        public List<Modelo.Marcacao> GetAllList()
        {
            FbParameter[] parms = new FbParameter[0];


            string aux = "SELECT \"marcacao\".* " +
                               ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                         " FROM \"marcacao\" " +
                         " LEFT JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" ";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            while (dr.Read())
            {
                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                AuxSetInstance(dr, objMarcacao);
                DAL.FB.BilhetesImp dalBilhetesImp = DAL.FB.BilhetesImp.GetInstancia;
                objMarcacao.BilhetesMarcacao = dalBilhetesImp.LoadManutencaoBilhetes(objMarcacao.Dscodigo, objMarcacao.Data, true);
                lista.Add(objMarcacao);
            }

            return lista;
        }

        public List<DateTime> GetDataMarcacoesPeriodo(int pIdFuncionario, DateTime pDataI, DateTime pDataF)
        {
            FbParameter[] parms = new FbParameter[3]
            { 
                    new FbParameter("@idfuncionario", FbDbType.Integer),
                    new FbParameter("@datai", FbDbType.Date),
                    new FbParameter("@dataf", FbDbType.Date)
            };
            parms[0].Value = pIdFuncionario;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string aux = " SELECT \"data\" FROM \"marcacao\" WHERE \"idfuncionario\" = @idfuncionario AND \"data\" >= @datai AND \"data\" <= @dataf";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);
            List<DateTime> lista = new List<DateTime>();
            while (dr.Read())
            {
                lista.Add(Convert.ToDateTime(dr["data"]));
            }

            return lista;
        }

        #region Incluir, Alterar, Excluir

        public string MontaUpdateFechamento(int pIdFuncionario, int pIdFechamentoBH, DateTime pDataInicial, DateTime pDataFinal)
        {
            StringBuilder comando = new StringBuilder();

            comando.Append("UPDATE \"marcacao\" SET \"idfechamentobh\" = " + pIdFechamentoBH);
            comando.Append(" WHERE \"marcacao\".\"idfuncionario\" = " + pIdFuncionario);
            comando.Append(" AND \"marcacao\".\"naoentrarbanco\" = 0");
            comando.Append(" AND COALESCE(\"marcacao\".\"idfechamentobh\",0) = 0");
            comando.Append(" AND \"marcacao\".\"data\" >= '" + pDataInicial.Month + "/" + pDataInicial.Day + "/" + pDataInicial.Year + "'");
            comando.Append(" AND \"marcacao\".\"data\" <= '" + pDataFinal.Month + "/" + pDataFinal.Day + "/" + pDataFinal.Year + "'");

            return comando.ToString();
        }

        public override void Alterar(Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            FbParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    FbCommand cmd = FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

                    AuxManutencao(trans, obj);

                    if (((Modelo.Marcacao)obj).Bilhete != null)
                    {
                        DAL.FB.BilhetesImp dalBilhete = DAL.FB.BilhetesImp.GetInstancia;
                        dalBilhete.Alterar(trans, ((Modelo.Marcacao)obj).Bilhete);
                    }

                    trans.Commit();

                    cmd.Parameters.Clear();
                }
            }
        }

        public void Incluir(List<Modelo.Marcacao> listaObjeto)
        {
            FbCommand cmd = new FbCommand();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = INSERT;
                FbParameter[] parms = GetParameters();
                foreach (Modelo.Marcacao obj in listaObjeto)
                {
                    try
                    {
                        SetParameters(parms, obj);
                        DataBase.PrepareParameters(parms, true);
                        cmd.Parameters.AddRange(parms);
                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        public void Alterar(List<Modelo.Marcacao> listaObjeto)
        {
            FbCommand cmd = new FbCommand();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = UPDATE;
                FbParameter[] parms = GetParameters();
                foreach (Modelo.Marcacao obj in listaObjeto)
                {
                    SetParameters(parms, obj);
                    DataBase.PrepareParameters(parms, true);
                    cmd.Parameters.AddRange(parms);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
        }

        public void Excluir(List<Modelo.Marcacao> listaObjeto)
        {
            FbCommand cmd = new FbCommand();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = DELETE;
                FbParameter[] parms = new FbParameter[]
                {
                    new FbParameter("@id", FbDbType.Integer)
                };
                foreach (Modelo.Marcacao obj in listaObjeto)
                {
                    parms[0].Value = obj.Id;
                    DataBase.PrepareParameters(parms, true);
                    cmd.Parameters.AddRange(parms);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
        }

        #endregion

        #region Get Ultima Data

        public DateTime? GetUltimaDataFuncionario(int pIdFuncionario)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idfuncionario", FbDbType.Integer)
            };

            parms[0].Value = pIdFuncionario;

            string sql = "SELECT MAX(\"data\") AS \"data\" FROM \"marcacao\" WHERE \"idfuncionario\" = @idfuncionario ";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            if (dr.Read())
            {
                return (dr["data"] is DBNull ? null : (DateTime?)dr["data"]);
            }
            else
            {
                return null;
            }
        }

        public DateTime? GetUltimaDataDepartamento(int pIdDepartamento)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@iddepartamento", FbDbType.Integer)
            };

            parms[0].Value = pIdDepartamento;

            string sql = "SELECT MAX(\"data\") AS \"data\" FROM \"marcacao\" "
            + " INNER JOIN \"funcionario\" ON \"funcionario\".\"iddepartamento\" = @iddepartamento ";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            if (dr.Read())
            {
                return (dr["data"] is DBNull ? null : (DateTime?)dr["data"]);
            }
            else
            {
                return null;
            }
        }

        public DateTime? GetUltimaDataEmpresa(int pIdEmpresa)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idempresa", FbDbType.Integer)
            };

            parms[0].Value = pIdEmpresa;

            string sql = "SELECT MAX(\"data\") AS \"data\" FROM \"marcacao\" "
            + " INNER JOIN \"funcionario\" ON \"funcionario\".\"idempresa\" = @idempresa ";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            if (dr.Read())
            {
                return (dr["data"] is DBNull ? null : (DateTime?)dr["data"]);
            }
            else
            {
                return null;
            }
        }

        public DateTime? GetUltimaDataFuncao(int pIDFuncao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@idfuncao", FbDbType.Integer)
            };

            parms[0].Value = pIDFuncao;

            string sql = "SELECT MAX(\"data\") AS \"data\" FROM \"marcacao\" "
            + " INNER JOIN \"funcionario\" ON \"funcionario\".\"idfuncao\" = @idfuncao ";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            if (dr.Read())
            {
                return Convert.ToDateTime(dr["data"] is DBNull ? null : dr["data"]);
            }
            else
            {
                return null;
            }
        }

        #endregion

        public DateTime? GetDataDSRAnterior(int pIdFuncionario, DateTime pData)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@data", FbDbType.Date),
                new FbParameter("@idfuncionario", FbDbType.Integer)
            };

            parms[0].Value = pData;
            parms[1].Value = pIdFuncionario;

            string comando = "SELECT FIRST 1 \"data\" FROM \"marcacao\""
                             + " WHERE \"marcacao\".\"idfuncionario\" = @idfuncionario"
                             + " AND \"marcacao\".\"data\" <= @data"
                             + " AND \"marcacao\".\"dsr\" = 1"
                             + " ORDER BY \"marcacao\".\"data\" DESC";
            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, comando, parms);

            if (dr.Read())
            {
                return Convert.ToDateTime(dr["data"]);
            }
            return null;
        }

        public DateTime? GetDataDSRProximo(int pIdFuncionario, DateTime pData)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@data", FbDbType.Date),
                new FbParameter("@idfuncionario", FbDbType.Integer)
            };

            parms[0].Value = pData;
            parms[1].Value = pIdFuncionario;

            string comando = "SELECT FIRST 1 \"data\" FROM \"marcacao\""
                             + " WHERE \"marcacao\".\"idfuncionario\" = @idfuncionario"
                             + " AND \"marcacao\".\"data\" >= @data"
                             + " AND \"marcacao\".\"dsr\" = 1"
                             + " ORDER BY \"marcacao\".\"data\"";
            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, comando, parms);

            if (dr.Read())
            {
                return Convert.ToDateTime(dr["data"]);
            }
            return null;
        }

        #endregion

        #region NovaImplementação

        /// <summary>
        /// Esse método monta uma parte do select que é utilizado para pegar apenas as marcações que são daquele funcionario para aquele dia.
        /// </summary>
        /// <param name="pIdFuncionario">Id do funcionario</param>
        /// <param name="pData">Data</param>
        /// <param name="comando">Comando SQL que será montado</param>
        public void MontaMarcFunc(int pIdFuncionario, DateTime pData, ref string comando)
        {
            if (comando == null)
                comando += "(\"funcionario\".\"id\" = " + pIdFuncionario + " AND " + "\"marcacao\".\"data\" = '" + pData.Month + "/" + pData.Day + "/" + pData.Year + "' )";
            else
                comando += "OR (\"funcionario\".\"id\" = " + pIdFuncionario + " AND " + "\"marcacao\".\"data\" = '" + pData.Month + "/" + pData.Day + "/" + pData.Year + "' )";
        }

        /// <summary>
        /// Pega todas as marcações daquele tipo
        /// </summary>
        /// <param name="pTipo">0: Empresa, 1: Departamento, 2: Funcionario, 3: Função, 4: Horario</param>
        /// <param name="pIdTipo">Id do tipo</param>
        /// <param name="pDataInicial">Data inicial</param>
        /// <param name="pDataFinal">Data final</param>
        /// <returns>Hashtable com idmarcacao, idfuncionario e data</returns>
        public Hashtable GetMarcDiaFunc(int pTipo, int pIdTipo, DateTime pDataInicial, DateTime pDataFinal)
        {
            FbParameter[] parms = new FbParameter[]
			{
                new FbParameter ("@datai", FbDbType.Date),
                new FbParameter ("@dataf", FbDbType.Date),
                new FbParameter ("@identificacao", FbDbType.Integer)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            string
                comando = "SELECT \"marcacao\".\"id\" as \"idmarcacao\", \"marcacao\".\"data\", \"marcacao\".\"idfuncionario\""
                + " FROM \"marcacao\""
                + " INNER JOIN \"horario\" ON \"horario\".\"id\" = \"marcacao\".\"idhorario\""
                + " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\""
                + " WHERE \"marcacao\".\"data\" >= @datai AND \"marcacao\".\"data\" <= @dataf";
            
            if (pTipo >= 0 && pTipo <= 4)
            {
                parms[2].Value = pIdTipo;
                switch (pTipo)
                {
                    case 0://Empresa
                        comando += " AND \"funcionario\".\"idempresa\" = @identificacao";
                        break;
                    case 1://Departamento
                        comando += " AND \"funcionario\".\"iddepartamento\" = @identificacao";
                        break;
                    case 2://Funcionário
                        comando += " AND \"marcacao\".\"idfuncionario\" = @identificacao";
                        break;
                    case 3://Função
                        comando += " AND \"funcionario\".\"idfuncao\" = @identificacao";
                        break;
                    case 4://Horário
                        comando += " AND \"marcacao\".\"idhorario\" = @identificacao";
                        break;
                    default:
                        break;
                }
            }

            comando += " ORDER BY \"marcacao\".\"idfuncionario\"";

            Modelo.BuscaMarcacaoFunc objBuscaMarc;
            string key;

            DataTable dt = new DataTable();
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, comando, parms));

            Hashtable listaHTMarcacao = new Hashtable();
            foreach (DataRow marc in dt.Rows)
            {
                key = "";
                objBuscaMarc.data = Convert.ToDateTime(marc["data"]);
                objBuscaMarc.idFuncionario = Convert.ToInt32(marc["idfuncionario"]);
                objBuscaMarc.idMarcacao = Convert.ToInt32(marc["idmarcacao"]);

                key = objBuscaMarc.idFuncionario.ToString() + objBuscaMarc.data.Date.ToString();
                
                //So insere se ainda não tiver na lista. Isso foi feito porque dava erro em bancos que tinham marcações duplicadas
                if (!listaHTMarcacao.ContainsKey(key))
                    listaHTMarcacao.Add(key, objBuscaMarc);
            }

            return listaHTMarcacao;
        }

        #endregion

        #region IMarcacao Members


        public List<Modelo.Marcacao> GetListaCompesacao(List<DateTime> datas, int tipo, int identificacao)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IMarcacao Members


        public DataTable GetParaRelatorioAbstinencia(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal)
        {
            throw new NotImplementedException();
        }

        #endregion


        public List<Modelo.MarcacaoLista> GetPorManutDiariaCont(int pIDContrato, DateTime pDataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            throw new NotImplementedException();
        }

        public void AdicionarFechamentoPonto(System.Data.SqlClient.SqlTransaction trans, int pIdFechamentoPonto)
        {
            throw new NotImplementedException();
        }

        public void ClearFechamentoPonto(System.Data.SqlClient.SqlTransaction trans, int pIdFechamentoPonto)
        {
            throw new NotImplementedException();
        }

        public void AdicionarFechamentoPonto(System.Data.SqlClient.SqlTransaction trans, int pIdFechamentoPonto, int pIdFuncionario)
        {
            throw new NotImplementedException();
        }


        public DataTable GetRelatorioObras(string idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal, string codsLocalReps)
        {
            throw new NotImplementedException();
        }

        public DataTable GetRelatorioRegistros(string idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal6)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> GetRelatorioConferenciaHoras(List<String> cpfsFuncionarios, DateTime dataInicial, DateTime DataFinal)
        {
            throw new NotImplementedException();
        }
        public List<Modelo.Proxy.pxyMarcacaoMudancaHorario> GetMudancasHorarioExportacao(DateTime dataIni, DateTime dataFim, List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }


        public void AtualizaMudancaHorarioMarcacao(List<int> idsFuncionarios, DateTime dataInicio)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Marcacao> GetPorFuncionarios(List<int> pIdsFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Marcacao> GetCartaoPontoV2(List<int> pIdFuncionarios, DateTime pdataInicial, DateTime pDataFinal)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Marcacao> GetTratamentosMarcacao(DateTime datainicial, DateTime datafinal)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Proxy.Relatorios.PxyRelTotalHorasTrabPorDiaPorFunc> GetRelatorioTotalHorasTrabPorFuncionario(List<String> cpfsFuncionarios, DateTime dataInicial, DateTime DataFinal)
        {
            throw new NotImplementedException();
        }

        public void AtualizarDataLoginBloqueioEdicaoPnlRh(DateTime dataInicio, DateTime dataFim, int idFunc, string tipoSolicitacao)
        {
            throw new NotImplementedException();
        }

        public DateTime? GetLastDateMarcacao(int idFunc)
        {
            throw new NotImplementedException();
        }

        public int GetIdDocumentoWorkflow(int idMarcacao)
        {
            throw new NotImplementedException();
        }

        public void IncluiUsrDtaConclusaoFluxoPnlRh(int idMarcacao, DateTime dataConclusao, string usrLogin)
        {
            throw new NotImplementedException();
        }

        public DataTable ConclusoesBloqueioPnlRh(string idsFuncionarios, DateTime dataInicial, DateTime dataFinal, int tipoFiltro)
        {
            throw new NotImplementedException();
        }


        public void ManipulaDocumentoWorkFlowPnlRH(int idMarcacao, int idDocumentoWorkflow, bool documentoWorkflowAberto)
        {
            throw new NotImplementedException();
        }

        public DataTable GetParaTotalizaHorasFuncs(List<int> pIdFuncs, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, int> QuantidadeMarcacoes(List<int> pIdFuncs, DateTime pDataI, DateTime pDataF)
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

        public DataTable GetDataUltimaMarcacaoFuncionario(List<int> idsFuncionarios)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Marcacao> GetPorFuncionariosContratosAtivos(List<int> ids, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            throw new NotImplementedException();
        }
    }
}
