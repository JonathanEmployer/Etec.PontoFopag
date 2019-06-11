using System;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using Modelo;

namespace DAL.FB
{
    public class TipoBilhetes : DAL.FB.DALBase, DAL.ITipoBilhetes
    {
        public string SELECTIMP { get; set; }

        private TipoBilhetes()
        {
            GEN = "GEN_tipobilhetes_id";

            TABELA = "tipobilhetes";

            SELECTPID = "SELECT * FROM \"tipobilhetes\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT   \"tipobilhetes\".\"id\"" +
                                    ", \"tipobilhetes\".\"descricao\"" +
                                    ", \"tipobilhetes\".\"codigo\"" +
                                    ", COALESCE(\"tipobilhetes\".\"diretorio\", ' ') AS \"diretorio\" " +
                                    ", case when \"tipobilhetes\".\"formatobilhete\" = 0 then 'TOPDATA (5 Digitos)' when \"tipobilhetes\".\"formatobilhete\" = 1 then 'TOPDATA (16 Digitos)' when \"tipobilhetes\".\"formatobilhete\" = 2 then 'Layout Livre' " + 
                                    " when \"tipobilhetes\".\"formatobilhete\" = 3 then 'REP (AFD)' end AS \"formatobilhete\"" +
                                    ", case when \"tipobilhetes\".\"bimporta\" = 0 then 'Não' when \"tipobilhetes\".\"bimporta\" = 1 then 'Sim' end As \"importar\"" +
                             " FROM \"tipobilhetes\"";

            SELECTIMP = "   SELECT   \"tipobilhetes\".* FROM \"tipobilhetes\"";

            INSERT = "  INSERT INTO \"tipobilhetes\"" +
                                        "(\"codigo\", \"descricao\", \"diretorio\", \"formatobilhete\", \"bimporta\", \"ordem_c\", \"ordem_t\", \"dia_c\", \"dia_t\", \"mes_c\", \"mes_t\", \"ano_c\", \"ano_t\", \"hora_c\", \"hora_t\", \"minuto_c\", \"minuto_t\", \"funcionario_c\", \"funcionario_t\", \"relogio_c\", \"relogio_t\", \"strlayout\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @descricao, @diretorio, @formatobilhete, @bimporta, @ordem_c, @ordem_t, @dia_c, @dia_t, @mes_c, @mes_t, @ano_c, @ano_t, @hora_c, @hora_t, @minuto_c, @minuto_t, @funcionario_c, @funcionario_t, @relogio_c, @relogio_t, @strlayout, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"tipobilhetes\" SET \"codigo\" = @codigo " +
                                        ", \"descricao\" = @descricao " +
                                        ", \"diretorio\" = @diretorio " +
                                        ", \"formatobilhete\" = @formatobilhete " +
                                        ", \"bimporta\" = @bimporta " +
                                        ", \"ordem_c\" = @ordem_c " +
                                        ", \"ordem_t\" = @ordem_t " +
                                        ", \"dia_c\" = @dia_c " +
                                        ", \"dia_t\" = @dia_t " +
                                        ", \"mes_c\" = @mes_c " +
                                        ", \"mes_t\" = @mes_t " +
                                        ", \"ano_c\" = @ano_c " +
                                        ", \"ano_t\" = @ano_t " +
                                        ", \"hora_c\" = @hora_c " +
                                        ", \"hora_t\" = @hora_t " +
                                        ", \"minuto_c\" = @minuto_c " +
                                        ", \"minuto_t\" = @minuto_t " +
                                        ", \"funcionario_c\" = @funcionario_c " +
                                        ", \"funcionario_t\" = @funcionario_t " +
                                        ", \"relogio_c\" = @relogio_c " +
                                        ", \"relogio_t\" = @relogio_t " +
                                        ", \"strlayout\" = @strlayout " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"tipobilhetes\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"tipobilhetes\"";

        }

        #region Singleton

        private static volatile FB.TipoBilhetes _instancia = null;

        public static FB.TipoBilhetes GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.TipoBilhetes))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.TipoBilhetes();
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
                    AtribuiTipoBilhete(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.TipoBilhetes();
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

        private void AtribuiTipoBilhete(FbDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.TipoBilhetes)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.TipoBilhetes)obj).Descricao = Convert.ToString(dr["descricao"]);
            ((Modelo.TipoBilhetes)obj).Diretorio = Convert.ToString(dr["diretorio"]);
            ((Modelo.TipoBilhetes)obj).FormatoBilhete = Convert.ToInt32(dr["formatobilhete"]);
            ((Modelo.TipoBilhetes)obj).BImporta = Convert.ToBoolean(dr["bimporta"]);
            ((Modelo.TipoBilhetes)obj).Ordem_c = Convert.ToInt32(dr["ordem_c"]);
            ((Modelo.TipoBilhetes)obj).Ordem_t = Convert.ToInt32(dr["ordem_t"]);
            ((Modelo.TipoBilhetes)obj).Dia_c = Convert.ToInt32(dr["dia_c"]);
            ((Modelo.TipoBilhetes)obj).Dia_t = Convert.ToInt32(dr["dia_t"]);
            ((Modelo.TipoBilhetes)obj).Mes_c = Convert.ToInt32(dr["mes_c"]);
            ((Modelo.TipoBilhetes)obj).Mes_t = Convert.ToInt32(dr["mes_t"]);
            ((Modelo.TipoBilhetes)obj).Ano_c = Convert.ToInt32(dr["ano_c"]);
            ((Modelo.TipoBilhetes)obj).Ano_t = Convert.ToInt32(dr["ano_t"]);
            ((Modelo.TipoBilhetes)obj).Hora_c = Convert.ToInt32(dr["hora_c"]);
            ((Modelo.TipoBilhetes)obj).Hora_t = Convert.ToInt32(dr["hora_t"]);
            ((Modelo.TipoBilhetes)obj).Minuto_c = Convert.ToInt32(dr["minuto_c"]);
            ((Modelo.TipoBilhetes)obj).Minuto_t = Convert.ToInt32(dr["minuto_t"]);
            ((Modelo.TipoBilhetes)obj).Funcionario_c = Convert.ToInt32(dr["funcionario_c"]);
            ((Modelo.TipoBilhetes)obj).Funcionario_t = Convert.ToInt32(dr["funcionario_t"]);
            ((Modelo.TipoBilhetes)obj).Relogio_c = Convert.ToInt32(dr["relogio_c"]);
            ((Modelo.TipoBilhetes)obj).Relogio_t = Convert.ToInt32(dr["relogio_t"]);
            ((Modelo.TipoBilhetes)obj).StrLayout = Convert.ToString(dr["strlayout"]);
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@descricao", FbDbType.VarChar),
				new FbParameter ("@diretorio", FbDbType.VarChar),
				new FbParameter ("@formatobilhete", FbDbType.SmallInt),
				new FbParameter ("@bimporta", FbDbType.SmallInt),
				new FbParameter ("@ordem_c", FbDbType.Integer),
				new FbParameter ("@ordem_t", FbDbType.Integer),
				new FbParameter ("@dia_c", FbDbType.Integer),
				new FbParameter ("@dia_t", FbDbType.Integer),
				new FbParameter ("@mes_c", FbDbType.Integer),
				new FbParameter ("@mes_t", FbDbType.Integer),
				new FbParameter ("@ano_c", FbDbType.Integer),
				new FbParameter ("@ano_t", FbDbType.Integer),
				new FbParameter ("@hora_c", FbDbType.Integer),
				new FbParameter ("@hora_t", FbDbType.Integer),
				new FbParameter ("@minuto_c", FbDbType.Integer),
				new FbParameter ("@minuto_t", FbDbType.Integer),
				new FbParameter ("@funcionario_c", FbDbType.Integer),
				new FbParameter ("@funcionario_t", FbDbType.Integer),
				new FbParameter ("@relogio_c", FbDbType.Integer),
				new FbParameter ("@relogio_t", FbDbType.Integer),
				new FbParameter ("@strlayout", FbDbType.VarChar),
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
            parms[1].Value = ((Modelo.TipoBilhetes)obj).Codigo;
            parms[2].Value = ((Modelo.TipoBilhetes)obj).Descricao;
            parms[3].Value = ((Modelo.TipoBilhetes)obj).Diretorio;
            parms[4].Value = ((Modelo.TipoBilhetes)obj).FormatoBilhete;
            parms[5].Value = ((Modelo.TipoBilhetes)obj).BImporta;
            parms[6].Value = ((Modelo.TipoBilhetes)obj).Ordem_c;
            parms[7].Value = ((Modelo.TipoBilhetes)obj).Ordem_t;
            parms[8].Value = ((Modelo.TipoBilhetes)obj).Dia_c;
            parms[9].Value = ((Modelo.TipoBilhetes)obj).Dia_t;
            parms[10].Value = ((Modelo.TipoBilhetes)obj).Mes_c;
            parms[11].Value = ((Modelo.TipoBilhetes)obj).Mes_t;
            parms[12].Value = ((Modelo.TipoBilhetes)obj).Ano_c;
            parms[13].Value = ((Modelo.TipoBilhetes)obj).Ano_t;
            parms[14].Value = ((Modelo.TipoBilhetes)obj).Hora_c;
            parms[15].Value = ((Modelo.TipoBilhetes)obj).Hora_t;
            parms[16].Value = ((Modelo.TipoBilhetes)obj).Minuto_c;
            parms[17].Value = ((Modelo.TipoBilhetes)obj).Minuto_t;
            parms[18].Value = ((Modelo.TipoBilhetes)obj).Funcionario_c;
            parms[19].Value = ((Modelo.TipoBilhetes)obj).Funcionario_t;
            parms[20].Value = ((Modelo.TipoBilhetes)obj).Relogio_c;
            parms[21].Value = ((Modelo.TipoBilhetes)obj).Relogio_t;
            parms[22].Value = ((Modelo.TipoBilhetes)obj).StrLayout;
            parms[23].Value = ((Modelo.TipoBilhetes)obj).Incdata;
            parms[24].Value = ((Modelo.TipoBilhetes)obj).Inchora;
            parms[25].Value = ((Modelo.TipoBilhetes)obj).Incusuario;
            parms[26].Value = ((Modelo.TipoBilhetes)obj).Altdata;
            parms[27].Value = ((Modelo.TipoBilhetes)obj).Althora;
            parms[28].Value = ((Modelo.TipoBilhetes)obj).Altusuario;
        }

        public Modelo.TipoBilhetes LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.TipoBilhetes objTipoBilhetes = new Modelo.TipoBilhetes();
            try
            {

                SetInstance(dr, objTipoBilhetes);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objTipoBilhetes;
        }

        public List<Modelo.TipoBilhetes> getListaImportacao()
        {
            List<Modelo.TipoBilhetes> lista = new List<Modelo.TipoBilhetes>();

            FbParameter[] parms = new FbParameter[0];

            string aux = "SELECT * FROM \"tipobilhetes\"";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, aux, parms);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.TipoBilhetes objTipoBilhete = new Modelo.TipoBilhetes();
                    AtribuiTipoBilhete(dr, objTipoBilhete);
                    lista.Add(objTipoBilhete);
                }
            }
            dr.Close();

            return lista;
        }
       
        //esta classe verifica quantos tipos de bilhetes tem cadastrados
        public int ContaNumRegistros()
        {
            FbParameter[] parms = new FbParameter[0];
            string aux = "SELECT Count(\"id\") FROM \"tipobilhetes\"";

            return (int)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);
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
        #endregion
    }
}
