using System;
using System.Collections;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;
using Modelo;
using System.Data.SqlClient;

namespace DAL.FB
{
    public class BancoHoras : DAL.FB.DALBase, DAL.IBancoHoras
    {
        public string VERIFICA { get; set; }
        public string SELECTRELF { get; set; }
        public string SELECTRELD { get; set; }
        public string SELECTRELE { get; set; }

        private BancoHoras()
        {
            GEN = "GEN_bancohoras_id";

            TABELA = "bancohoras";

            SELECTPID = "SELECT * FROM \"bancohoras\" WHERE \"id\" = @id";

            SELECTALL = "   SELECT         \"id\" " +
                                        ", \"codigo\" " +
                                        ", \"datainicial\" " +
                                        ", \"datafinal\" " +
                                        ", case when \"tipo\" = 0 then 'Empresa' when \"tipo\" = 1 then 'Departamento' when \"tipo\" = 2 then 'Funcionário' when \"tipo\" = 3 then 'Função' end AS \"tipo\" " +
                                        ", case when \"tipo\" = 0 then (SELECT \"empresa\".\"nome\" FROM \"empresa\" WHERE \"empresa\".\"id\" = \"bancohoras\".\"identificacao\") " +
                                        "     when \"tipo\" = 1 then (SELECT \"departamento\".\"descricao\" FROM \"departamento\" WHERE \"departamento\".\"id\" = \"bancohoras\".\"identificacao\") " +
                                        "     when \"tipo\" = 2 then (SELECT \"funcionario\".\"nome\" FROM \"funcionario\" WHERE \"funcionario\".\"id\" = \"bancohoras\".\"identificacao\") " +
                                        "     when \"tipo\" = 3 then (SELECT \"funcao\".\"descricao\" FROM \"funcao\" WHERE \"funcao\".\"id\" = \"bancohoras\".\"identificacao\") end AS \"nome\" " +             
                             " FROM \"bancohoras\" ";

            SELECTRELF = "   SELECT     \"bh\".\"id\" " +
                                     " ,\"bh\".\"datainicial\" " +
                                     " ,\"bh\".\"datafinal\" " +
                                     " , COALESCE((SELECT \"ib\".\"credito\" FROM \"inclusaobanco\" \"ib\" WHERE \"ib\".\"identificacao\" = \"emp\".\"id\"), '0000:00') AS \"credito\" " +
                                     " , COALESCE((SELECT \"ib\".\"debito\" FROM \"inclusaobanco\" \"ib\" WHERE \"ib\".\"identificacao\" = \"emp\".\"id\"), '0000:00') AS \"debito\" " +
                                     " , \"dep\".\"descricao\" AS \"departamento\" " +
                                     " , \"func\".\"codigo\" AS \"funccodigo\" " +
                                     " , \"func\".\"nome\" AS \"funcnome\" " +
                                     " , \"emp\".\"nome\" AS \"empnome\" " +
                                     " , \"emp\".\"cep\" AS \"empcep\" " +
                                     " , \"emp\".\"cidade\" AS \"empcidade\" " +
                                     " , \"emp\".\"estado\" AS \"empuf\" " +
                                     " , \"emp\".\"endereco\" AS \"empendereco\" " +              
                                     " , \"emp\".\"cnpj\" AS \"empcnpj\" " +
                              " FROM \"bancohoras\" \"bh\" " +
                              " INNER JOIN \"funcionario\" \"func\" ON \"func\".\"id\" = \"bh\".\"identificacao\" AND \"func\".\"funcionarioativo\" = 1 " +
                              " INNER JOIN \"departamento\" \"dep\" ON \"dep\".\"id\" = \"func\".\"iddepartamento\" " +
                              " INNER JOIN \"empresa\" \"emp\" ON \"emp\".\"id\" = \"dep\".\"idempresa\" " + 
                              " WHERE ((@datainicial >= \"datainicial\" AND @datainicial <= \"datafinal\") " +
                              " OR (@datafinal >= \"bh\".\"datainicial\" AND @datafinal <= \"bh\".\"datafinal\") " +
                              " OR (@datainicial <= \"bh\".\"datainicial\" AND @datafinal >= \"bh\".\"datafinal\")) ";

            SELECTRELD = "   SELECT     \"bh\".\"id\" " +
                                     " ,\"bh\".\"datainicial\" " +
                                     " ,\"bh\".\"datafinal\" " +
                                     " , COALESCE((SELECT \"ib\".\"credito\" FROM \"inclusaobanco\" \"ib\" WHERE \"ib\".\"identificacao\" = \"emp\".\"id\"), '0000:00') AS \"credito\" " +
                                     " , COALESCE((SELECT \"ib\".\"debito\" FROM \"inclusaobanco\" \"ib\" WHERE \"ib\".\"identificacao\" = \"emp\".\"id\"), '0000:00') AS \"debito\" " +
                                     " , \"dep\".\"descricao\" AS \"departamento\" " +
                                     " , \"func\".\"codigo\" AS \"funccodigo\" " +
                                     " , \"func\".\"nome\" AS \"funcnome\" " +
                                     " , \"emp\".\"nome\" AS \"empnome\" " +
                                     " , \"emp\".\"cep\" AS \"empcep\" " +
                                     " , \"emp\".\"cidade\" AS \"empcidade\" " +
                                     " , \"emp\".\"estado\" AS \"empuf\" " +
                                     " , \"emp\".\"endereco\" AS \"empendereco\" " +
                                     " , \"emp\".\"cnpj\" AS \"empcnpj\" " +
                              " FROM \"bancohoras\" \"bh\" " +
                              " INNER JOIN \"departamento\" \"dep\" ON \"dep\".\"id\" = \"bh\".\"identificacao\" " +
                              " INNER JOIN \"empresa\" \"emp\" ON \"emp\".\"id\" = \"dep\".\"idempresa\" " +
                              " INNER JOIN \"funcionario\" \"func\" ON \"func\".\"iddepartamento\" = \"dep\".\"id\" AND \"func\".\"funcionarioativo\" = 1 " +
                              " WHERE ((@datainicial >= \"datainicial\" AND @datainicial <= \"datafinal\") " +
                              " OR (@datafinal >= \"bh\".\"datainicial\" AND @datafinal <= \"bh\".\"datafinal\") " +
                              " OR (@datainicial <= \"bh\".\"datainicial\" AND @datafinal >= \"bh\".\"datafinal\")) ";

            SELECTRELE = "   SELECT     \"bh\".\"id\" " +
                                     " ,\"bh\".\"datainicial\" " +
                                     " ,\"bh\".\"datafinal\" " +
                                     " , COALESCE((SELECT \"ib\".\"credito\" FROM \"inclusaobanco\" \"ib\" WHERE \"ib\".\"identificacao\" = \"emp\".\"id\"), '0000:00') AS \"credito\" " +
                                     " , COALESCE((SELECT \"ib\".\"debito\" FROM \"inclusaobanco\" \"ib\" WHERE \"ib\".\"identificacao\" = \"emp\".\"id\"), '0000:00') AS \"debito\" " +
                                     " , \"dep\".\"descricao\" AS \"departamento\" " +
                                     " , \"func\".\"codigo\" AS \"funccodigo\" " +
                                     " , \"func\".\"nome\" AS \"funcnome\" " +
                                     " , \"emp\".\"nome\" AS \"empnome\" " +
                                     " , \"emp\".\"cep\" AS \"empcep\" " +
                                     " , \"emp\".\"cidade\" AS \"empcidade\" " +
                                     " , \"emp\".\"estado\" AS \"empuf\" " +
                                     " , \"emp\".\"endereco\" AS \"empendereco\" " +
                                     " , \"emp\".\"cnpj\" AS \"empcnpj\" " +
                              " FROM \"bancohoras\" \"bh\" " +
                              " INNER JOIN \"empresa\" \"emp\" ON \"emp\".\"id\" = \"bh\".\"identificacao\" " +
                              " INNER JOIN \"departamento\" \"dep\" ON \"dep\".\"idempresa\" = \"emp\".\"id\" " +
                              " INNER JOIN \"funcionario\" \"func\" ON \"func\".\"iddepartamento\" = \"dep\".\"id\" AND \"func\".\"funcionarioativo\" = 1 " +
                              " WHERE ((@datainicial >= \"datainicial\" AND @datainicial <= \"datafinal\") " +
                              " OR (@datafinal >= \"bh\".\"datainicial\" AND @datafinal <= \"bh\".\"datafinal\") " +
                              " OR (@datainicial <= \"bh\".\"datainicial\" AND @datafinal >= \"bh\".\"datafinal\")) ";   


            VERIFICA = "   SELECT COALESCE(COUNT(\"id\"), 0) AS \"qt\" " +
                            "FROM \"bancohoras\" " +
                            "WHERE ((@datainicial >= \"datainicial\" AND @datainicial <= \"datafinal\") " +
                            "OR (@datafinal >= \"datainicial\" AND @datafinal <= \"datafinal\") " +
                            "OR (@datainicial <= \"datainicial\" AND @datafinal >= \"datafinal\")) " +
                            "AND \"tipo\" = @tipo " +
                            "AND \"identificacao\" = @identificacao "+
                            "AND \"id\" <> @id ";

            INSERT = "  INSERT INTO \"bancohoras\"" +
                                        "(\"codigo\", \"tipo\", \"identificacao\", \"datainicial\", \"datafinal\", \"dias_1\", \"dias_2\", \"dias_3\", \"dias_4\", \"dias_5\", \"dias_6\", \"dias_7\", \"dias_8\", \"dias_9\", \"dias_10\", \"bancoprimeiro\", \"limitehoras_1\", \"limitehoras_2\", \"limitehoras_3\", \"limitehoras_4\", \"limitehoras_5\", \"limitehoras_6\", \"limitehoras_7\", \"limitehoras_8\", \"limitehoras_9\", \"limitehoras_10\", \"extraprimeiro\", \"limitehorasextras_1\", \"limitehorasextras_2\", \"limitehorasextras_3\", \"limitehorasextras_4\", \"limitehorasextras_5\", \"limitehorasextras_6\", \"limitehorasextras_7\", \"limitehorasextras_8\", \"limitehorasextras_9\", \"limitehorasextras_10\", \"percentuais_1\", \"percentuais_2\", \"percentuais_3\", \"percentuais_4\", \"percentuais_5\", \"percentuais_6\", \"percentuais_7\", \"percentuais_8\", \"percentuais_9\", \"percentuais_10\", \"incdata\", \"inchora\", \"incusuario\")" +
                                        "VALUES" +
                                        "(@codigo, @tipo, @identificacao, @datainicial, @datafinal, @dias_1, @dias_2, @dias_3, @dias_4, @dias_5, @dias_6, @dias_7, @dias_8, @dias_9, @dias_10, @bancoprimeiro, @limitehoras_1, @limitehoras_2, @limitehoras_3, @limitehoras_4, @limitehoras_5, @limitehoras_6, @limitehoras_7, @limitehoras_8, @limitehoras_9, @limitehoras_10, @extraprimeiro, @limitehorasextras_1, @limitehorasextras_2, @limitehorasextras_3, @limitehorasextras_4, @limitehorasextras_5, @limitehorasextras_6, @limitehorasextras_7, @limitehorasextras_8, @limitehorasextras_9, @limitehorasextras_10, @percentuais_1, @percentuais_2, @percentuais_3, @percentuais_4, @percentuais_5, @percentuais_6, @percentuais_7, @percentuais_8, @percentuais_9, @percentuais_10, @incdata, @inchora, @incusuario)";

            UPDATE = "  UPDATE \"bancohoras\" SET \"codigo\" = @codigo " +
                                        ", \"tipo\" = @tipo " +
                                        ", \"identificacao\" = @identificacao " +
                                        ", \"datainicial\" = @datainicial " +
                                        ", \"datafinal\" = @datafinal " +                                        
                                        ", \"dias_1\" = @dias_1 " +
                                        ", \"dias_2\" = @dias_2 " +
                                        ", \"dias_3\" = @dias_3 " +
                                        ", \"dias_4\" = @dias_4 " +
                                        ", \"dias_5\" = @dias_5 " +
                                        ", \"dias_6\" = @dias_6 " +
                                        ", \"dias_7\" = @dias_7 " +
                                        ", \"dias_8\" = @dias_8 " +
                                        ", \"dias_9\" = @dias_9 " +
                                        ", \"dias_10\" = @dias_10 " +
                                        ", \"bancoprimeiro\" = @bancoprimeiro " +
                                        ", \"limitehoras_1\" = @limitehoras_1 " +
                                        ", \"limitehoras_2\" = @limitehoras_2 " +
                                        ", \"limitehoras_3\" = @limitehoras_3 " +
                                        ", \"limitehoras_4\" = @limitehoras_4 " +
                                        ", \"limitehoras_5\" = @limitehoras_5 " +
                                        ", \"limitehoras_6\" = @limitehoras_6 " +
                                        ", \"limitehoras_7\" = @limitehoras_7 " +
                                        ", \"limitehoras_8\" = @limitehoras_8 " +
                                        ", \"limitehoras_9\" = @limitehoras_9 " +
                                        ", \"limitehoras_10\" = @limitehoras_10 " +
                                        ", \"extraprimeiro\" = @extraprimeiro " +
                                        ", \"limitehorasextras_1\" = @limitehorasextras_1 " +
                                        ", \"limitehorasextras_2\" = @limitehorasextras_2 " +
                                        ", \"limitehorasextras_3\" = @limitehorasextras_3 " +
                                        ", \"limitehorasextras_4\" = @limitehorasextras_4 " +
                                        ", \"limitehorasextras_5\" = @limitehorasextras_5 " +
                                        ", \"limitehorasextras_6\" = @limitehorasextras_6 " +
                                        ", \"limitehorasextras_7\" = @limitehorasextras_7 " +
                                        ", \"limitehorasextras_8\" = @limitehorasextras_8 " +
                                        ", \"limitehorasextras_9\" = @limitehorasextras_9 " +
                                        ", \"limitehorasextras_10\" = @limitehorasextras_10 " +
                                        ", \"percentuais_1\" = @percentuais_1 " +
                                        ", \"percentuais_2\" = @percentuais_2 " +
                                        ", \"percentuais_3\" = @percentuais_3 " +
                                        ", \"percentuais_4\" = @percentuais_4 " +
                                        ", \"percentuais_5\" = @percentuais_5 " +
                                        ", \"percentuais_6\" = @percentuais_6 " +
                                        ", \"percentuais_7\" = @percentuais_7 " +
                                        ", \"percentuais_8\" = @percentuais_8 " +
                                        ", \"percentuais_9\" = @percentuais_9 " +
                                        ", \"percentuais_10\" = @percentuais_10 " +
                                        ", \"altdata\" = @altdata " +
                                        ", \"althora\" = @althora " +
                                        ", \"altusuario\" = @altusuario " +
                                    "WHERE \"id\" = @id";

            DELETE = "DELETE FROM \"bancohoras\" WHERE \"id\" = @id";

            MAXCOD = "SELECT MAX(\"codigo\") AS codigo FROM \"bancohoras\"";

        }

        #region Singleton

        private static volatile FB.BancoHoras _instancia = null;

        public static FB.BancoHoras GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.BancoHoras))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.BancoHoras();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        #region Metodos

        #region Métodos Básicos

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
                obj = new Modelo.BancoHoras();
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
            ((Modelo.BancoHoras)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.BancoHoras)obj).Tipo = Convert.ToInt16(dr["tipo"]);
            ((Modelo.BancoHoras)obj).Identificacao = Convert.ToInt32(dr["identificacao"]);
            ((Modelo.BancoHoras)obj).DataInicial = Convert.ToDateTime(dr["datainicial"]);
            ((Modelo.BancoHoras)obj).DataFinal = Convert.ToDateTime(dr["datafinal"]);          
            ((Modelo.BancoHoras)obj).Dias_1 = Convert.ToInt16(dr["dias_1"]);
            ((Modelo.BancoHoras)obj).Dias_2 = Convert.ToInt16(dr["dias_2"]);
            ((Modelo.BancoHoras)obj).Dias_3 = Convert.ToInt16(dr["dias_3"]);
            ((Modelo.BancoHoras)obj).Dias_4 = Convert.ToInt16(dr["dias_4"]);
            ((Modelo.BancoHoras)obj).Dias_5 = Convert.ToInt16(dr["dias_5"]);
            ((Modelo.BancoHoras)obj).Dias_6 = Convert.ToInt16(dr["dias_6"]);
            ((Modelo.BancoHoras)obj).Dias_7 = Convert.ToInt16(dr["dias_7"]);
            ((Modelo.BancoHoras)obj).Dias_8 = Convert.ToInt16(dr["dias_8"]);
            ((Modelo.BancoHoras)obj).Dias_9 = Convert.ToInt16(dr["dias_9"]);
            ((Modelo.BancoHoras)obj).Dias_10 = Convert.ToInt16(dr["dias_10"]);
            ((Modelo.BancoHoras)obj).Bancoprimeiro = Convert.ToInt16(dr["bancoprimeiro"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_1 = Convert.ToString(dr["limitehoras_1"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_2 = Convert.ToString(dr["limitehoras_2"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_3 = Convert.ToString(dr["limitehoras_3"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_4 = Convert.ToString(dr["limitehoras_4"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_5 = Convert.ToString(dr["limitehoras_5"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_6 = Convert.ToString(dr["limitehoras_6"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_7 = Convert.ToString(dr["limitehoras_7"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_8 = Convert.ToString(dr["limitehoras_8"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_9 = Convert.ToString(dr["limitehoras_9"]);
            ((Modelo.BancoHoras)obj).LimiteHoras_10 = Convert.ToString(dr["limitehoras_10"]);
            ((Modelo.BancoHoras)obj).ExtraPrimeiro = Convert.ToInt16(dr["extraprimeiro"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_1 = Convert.ToString(dr["limitehorasextras_1"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_2 = Convert.ToString(dr["limitehorasextras_2"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_3 = Convert.ToString(dr["limitehorasextras_3"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_4 = Convert.ToString(dr["limitehorasextras_4"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_5 = Convert.ToString(dr["limitehorasextras_5"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_6 = Convert.ToString(dr["limitehorasextras_6"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_7 = Convert.ToString(dr["limitehorasextras_7"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_8 = Convert.ToString(dr["limitehorasextras_8"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_9 = Convert.ToString(dr["limitehorasextras_9"]);
            ((Modelo.BancoHoras)obj).LimiteHorasextras_10 = Convert.ToString(dr["limitehorasextras_10"]);
            ((Modelo.BancoHoras)obj).Percentuais_1 = Convert.ToInt16(dr["percentuais_1"]);
            ((Modelo.BancoHoras)obj).Percentuais_2 = Convert.ToInt16(dr["percentuais_2"]);
            ((Modelo.BancoHoras)obj).Percentuais_3 = Convert.ToInt16(dr["percentuais_3"]);
            ((Modelo.BancoHoras)obj).Percentuais_4 = Convert.ToInt16(dr["percentuais_4"]);
            ((Modelo.BancoHoras)obj).Percentuais_5 = Convert.ToInt16(dr["percentuais_5"]);
            ((Modelo.BancoHoras)obj).Percentuais_6 = Convert.ToInt16(dr["percentuais_6"]);
            ((Modelo.BancoHoras)obj).Percentuais_7 = Convert.ToInt16(dr["percentuais_7"]);
            ((Modelo.BancoHoras)obj).Percentuais_8 = Convert.ToInt16(dr["percentuais_8"]);
            ((Modelo.BancoHoras)obj).Percentuais_9 = Convert.ToInt16(dr["percentuais_9"]);
            ((Modelo.BancoHoras)obj).Percentuais_10 = Convert.ToInt16(dr["percentuais_10"]);

            ((Modelo.BancoHoras)obj).Tipo_Ant = ((Modelo.BancoHoras)obj).Tipo;
            ((Modelo.BancoHoras)obj).Identificacao_Ant = ((Modelo.BancoHoras)obj).Identificacao;
            ((Modelo.BancoHoras)obj).DataInicial_Ant = ((Modelo.BancoHoras)obj).DataInicial;
            ((Modelo.BancoHoras)obj).DataFinal_Ant = ((Modelo.BancoHoras)obj).DataFinal;
        }

        protected override FbParameter[] GetParameters()
        {
            FbParameter[] parms = new FbParameter[]
			{
				new FbParameter ("@id", FbDbType.Integer),
				new FbParameter ("@codigo", FbDbType.Integer),
				new FbParameter ("@tipo", FbDbType.SmallInt),
				new FbParameter ("@identificacao", FbDbType.Integer),
				new FbParameter ("@datainicial", FbDbType.Date),
				new FbParameter ("@datafinal", FbDbType.Date),		
				new FbParameter ("@dias_1", FbDbType.SmallInt),
				new FbParameter ("@dias_2", FbDbType.SmallInt),
				new FbParameter ("@dias_3", FbDbType.SmallInt),
				new FbParameter ("@dias_4", FbDbType.SmallInt),
				new FbParameter ("@dias_5", FbDbType.SmallInt),
				new FbParameter ("@dias_6", FbDbType.SmallInt),
				new FbParameter ("@dias_7", FbDbType.SmallInt),
				new FbParameter ("@dias_8", FbDbType.SmallInt),
				new FbParameter ("@dias_9", FbDbType.SmallInt),
				new FbParameter ("@dias_10", FbDbType.SmallInt),
				new FbParameter ("@bancoprimeiro", FbDbType.SmallInt),
				new FbParameter ("@limitehoras_1", FbDbType.VarChar),
				new FbParameter ("@limitehoras_2", FbDbType.VarChar),
				new FbParameter ("@limitehoras_3", FbDbType.VarChar),
				new FbParameter ("@limitehoras_4", FbDbType.VarChar),
				new FbParameter ("@limitehoras_5", FbDbType.VarChar),
				new FbParameter ("@limitehoras_6", FbDbType.VarChar),
				new FbParameter ("@limitehoras_7", FbDbType.VarChar),
				new FbParameter ("@limitehoras_8", FbDbType.VarChar),
				new FbParameter ("@limitehoras_9", FbDbType.VarChar),
				new FbParameter ("@limitehoras_10", FbDbType.VarChar),
				new FbParameter ("@extraprimeiro", FbDbType.SmallInt),
				new FbParameter ("@limitehorasextras_1", FbDbType.VarChar),
				new FbParameter ("@limitehorasextras_2", FbDbType.VarChar),
				new FbParameter ("@limitehorasextras_3", FbDbType.VarChar),
				new FbParameter ("@limitehorasextras_4", FbDbType.VarChar),
				new FbParameter ("@limitehorasextras_5", FbDbType.VarChar),
				new FbParameter ("@limitehorasextras_6", FbDbType.VarChar),
				new FbParameter ("@limitehorasextras_7", FbDbType.VarChar),
				new FbParameter ("@limitehorasextras_8", FbDbType.VarChar),
				new FbParameter ("@limitehorasextras_9", FbDbType.VarChar),
				new FbParameter ("@limitehorasextras_10", FbDbType.VarChar),
				new FbParameter ("@percentuais_1", FbDbType.SmallInt),
				new FbParameter ("@percentuais_2", FbDbType.SmallInt),
				new FbParameter ("@percentuais_3", FbDbType.SmallInt),
				new FbParameter ("@percentuais_4", FbDbType.SmallInt),
				new FbParameter ("@percentuais_5", FbDbType.SmallInt),
				new FbParameter ("@percentuais_6", FbDbType.SmallInt),
				new FbParameter ("@percentuais_7", FbDbType.SmallInt),
				new FbParameter ("@percentuais_8", FbDbType.SmallInt),
				new FbParameter ("@percentuais_9", FbDbType.SmallInt),
				new FbParameter ("@percentuais_10", FbDbType.SmallInt),
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
            parms[1].Value = ((Modelo.BancoHoras)obj).Codigo;
            parms[2].Value = ((Modelo.BancoHoras)obj).Tipo;
            parms[3].Value = ((Modelo.BancoHoras)obj).Identificacao;
            parms[4].Value = ((Modelo.BancoHoras)obj).DataInicial;
            parms[5].Value = ((Modelo.BancoHoras)obj).DataFinal;          
            parms[6].Value = ((Modelo.BancoHoras)obj).Dias_1;
            parms[7].Value = ((Modelo.BancoHoras)obj).Dias_2;
            parms[8].Value = ((Modelo.BancoHoras)obj).Dias_3;
            parms[9].Value = ((Modelo.BancoHoras)obj).Dias_4;
            parms[10].Value = ((Modelo.BancoHoras)obj).Dias_5;
            parms[11].Value = ((Modelo.BancoHoras)obj).Dias_6;
            parms[12].Value = ((Modelo.BancoHoras)obj).Dias_7;
            parms[13].Value = ((Modelo.BancoHoras)obj).Dias_8;
            parms[14].Value = ((Modelo.BancoHoras)obj).Dias_9;
            parms[15].Value = ((Modelo.BancoHoras)obj).Dias_10;
            parms[16].Value = ((Modelo.BancoHoras)obj).Bancoprimeiro;
            parms[17].Value = ((Modelo.BancoHoras)obj).LimiteHoras_1;
            parms[18].Value = ((Modelo.BancoHoras)obj).LimiteHoras_2;
            parms[19].Value = ((Modelo.BancoHoras)obj).LimiteHoras_3;
            parms[20].Value = ((Modelo.BancoHoras)obj).LimiteHoras_4;
            parms[21].Value = ((Modelo.BancoHoras)obj).LimiteHoras_5;
            parms[22].Value = ((Modelo.BancoHoras)obj).LimiteHoras_6;
            parms[23].Value = ((Modelo.BancoHoras)obj).LimiteHoras_7;
            parms[24].Value = ((Modelo.BancoHoras)obj).LimiteHoras_8;
            parms[25].Value = ((Modelo.BancoHoras)obj).LimiteHoras_9;
            parms[26].Value = ((Modelo.BancoHoras)obj).LimiteHoras_10;
            parms[27].Value = ((Modelo.BancoHoras)obj).ExtraPrimeiro;
            parms[28].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_1;
            parms[29].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_2;
            parms[30].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_3;
            parms[31].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_4;
            parms[32].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_5;
            parms[33].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_6;
            parms[34].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_7;
            parms[35].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_8;
            parms[36].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_9;
            parms[37].Value = ((Modelo.BancoHoras)obj).LimiteHorasextras_10;
            parms[38].Value = ((Modelo.BancoHoras)obj).Percentuais_1;
            parms[39].Value = ((Modelo.BancoHoras)obj).Percentuais_2;
            parms[40].Value = ((Modelo.BancoHoras)obj).Percentuais_3;
            parms[41].Value = ((Modelo.BancoHoras)obj).Percentuais_4;
            parms[42].Value = ((Modelo.BancoHoras)obj).Percentuais_5;
            parms[43].Value = ((Modelo.BancoHoras)obj).Percentuais_6;
            parms[44].Value = ((Modelo.BancoHoras)obj).Percentuais_7;
            parms[45].Value = ((Modelo.BancoHoras)obj).Percentuais_8;
            parms[46].Value = ((Modelo.BancoHoras)obj).Percentuais_9;
            parms[47].Value = ((Modelo.BancoHoras)obj).Percentuais_10;
            parms[48].Value = ((Modelo.BancoHoras)obj).Incdata;
            parms[49].Value = ((Modelo.BancoHoras)obj).Inchora;
            parms[50].Value = ((Modelo.BancoHoras)obj).Incusuario;
            parms[51].Value = ((Modelo.BancoHoras)obj).Altdata;
            parms[52].Value = ((Modelo.BancoHoras)obj).Althora;
            parms[53].Value = ((Modelo.BancoHoras)obj).Altusuario;
        }

        public Modelo.BancoHoras LoadObject(int id)
        {
            FbDataReader dr = LoadDataReader(id);

            Modelo.BancoHoras objBancoHoras = new Modelo.BancoHoras();
            try
            {
                SetInstance(dr, objBancoHoras);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objBancoHoras;
        }


        public List<Modelo.BancoHoras> GetAllList(bool verificaPermissao)
        {
            FbParameter[] parms = new FbParameter[0];
            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, "SELECT * FROM \"bancohoras\"", parms);
            List<Modelo.BancoHoras> lista = new List<Modelo.BancoHoras>();

            try
            {
                while (dr.Read())
                {
                    Modelo.BancoHoras objBancoHoras = new Modelo.BancoHoras();
                    AuxSetInstance(dr, objBancoHoras);
                    lista.Add(objBancoHoras);
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

        public Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF)
        {
            FbParameter[] parms = new FbParameter[] 
            { 
                new FbParameter("@datainicial", FbDbType.TimeStamp),
                new FbParameter("@datafinal", FbDbType.TimeStamp)
            };

            Hashtable lista = new Hashtable();
            string SQL = "SELECT * FROM \"bancohoras\"";

            if (pDataI != null && pDataF != null)
            {
                parms[0].Value = pDataI;
                parms[1].Value = pDataF;

                SQL += " WHERE ((@datainicial >= \"datainicial\" AND @datainicial <= \"datafinal\")"
                       + " OR (@datafinal >= \"datainicial\" AND @datafinal <= \"datafinal\")"
                       + " OR (@datainicial <= \"datainicial\" AND @datafinal >= \"datafinal\"))";
            }

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, SQL, parms);

            if (dr.HasRows)
            {
                Modelo.BancoHoras objBancoHoras = null;
                while (dr.Read())
                {
                    objBancoHoras = new Modelo.BancoHoras();
                    AuxSetInstance(dr, objBancoHoras);
                    lista.Add(Convert.ToInt32(dr["id"]), objBancoHoras);
                }
            }

            return lista;
        }

        #endregion

        #region Relatórios

        public DataTable GetRelatorioResumo(DateTime pDataI, DateTime pDataF, int pTipo, string pEmpresas, string pDepartamentos, string pFuncionarios)
        {
            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@datainicial", FbDbType.TimeStamp),
                new FbParameter("@datafinal", FbDbType.TimeStamp)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            DataTable dt = new DataTable();
            string aux = "SELECT \"funcionario\".\"id\" AS \"idfuncionario\", \"funcionario\".\"nome\" AS \"nomefuncionario\", \"funcionario\".\"dscodigo\""
                         + ", \"funcionario\".\"idempresa\", \"funcionario\".\"iddepartamento\", \"funcionario\".\"idfuncao\""
                         + ", \"empresa\".\"nome\" AS \"nomeempresa\", \"empresa\".\"endereco\", \"empresa\".\"cidade\""
                         + ", \"empresa\".\"estado\", \"empresa\".\"cep\""
                         + ", case when COALESCE(\"empresa\".\"cnpj\", '') <> '' then \"empresa\".\"cnpj\" else \"empresa\".\"cpf\" end AS \"cnpj_cpf\""
                         + ", \"departamento\".\"descricao\" AS \"nomedepartamento\""
                         + " FROM \"funcionario\""
                         + " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\""
                         + " INNER JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\""
                         + " WHERE COALESCE(\"funcionario\".\"excluido\", 0) = 0 AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1";

            switch (pTipo)
            {
                case 0: aux += " AND \"empresa\".\"id\" IN " + pEmpresas; break;
                case 1: aux += " AND \"departamento\".\"id\" IN " + pDepartamentos; break;
                case 2: aux += " AND \"funcionario\".\"id\" IN " + pFuncionarios; break;
            }

            aux += " ORDER BY LOWER(\"empresa\".\"nome\"),  \"nomedepartamento\", \"nomefuncionario\"";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));
            return dt;
        }

        public DataTable LoadRelatorio(DateTime pDataInicial, DateTime pDataFinal, int pTipo, string pEmpresas, string pDepartamentos, string pFuncionarios)
        {
            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@datainicial", FbDbType.TimeStamp),
                new FbParameter("@datafinal", FbDbType.TimeStamp)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            DataTable dt = new DataTable();
            string aux = "";

            switch (pTipo)
            {
                case 0:
                    aux = SELECTRELE;
                    aux += " AND \"emp\".\"id\" IN " + pEmpresas;
                    break;
                case 1:
                    aux = SELECTRELD;
                    aux += " AND \"emp\".\"id\" IN " + pEmpresas + " AND \"dep\".\"id\" IN " + pDepartamentos;
                    break;
                case 2:
                    aux = SELECTRELF;
                    aux += " AND \"func\".\"id\" IN " + pFuncionarios;
                    break;
            }

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));
            return dt;
        }

        public DataTable GetRelatorioHorario(DateTime pDataInicial, DateTime pDataFinal, int pTipo, string pIds)
        {
            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@datainicial", FbDbType.TimeStamp),
                new FbParameter("@datafinal", FbDbType.TimeStamp)
            };
            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;

            DataTable dt = new DataTable();

            string SQL = "SELECT \"marcacao\".\"id\", \"marcacao\".\"idhorario\" "
                        + ", COALESCE(\"marcacao\".\"legenda\", ' ') AS \"legenda\" "
                        + ", \"marcacao\".\"data\", \"marcacao\".\"dia\", \"marcacao\".\"entrada_1\", \"marcacao\".\"entrada_2\" "
                        + ", \"marcacao\".\"entrada_3\", \"marcacao\".\"entrada_4\", \"marcacao\".\"entrada_5\", \"marcacao\".\"entrada_6\" "
                        + ", \"marcacao\".\"entrada_7\", \"marcacao\".\"entrada_8\", \"marcacao\".\"saida_1\", \"marcacao\".\"saida_2\" "
                        + ", \"marcacao\".\"saida_3\" "
                        + ", \"marcacao\".\"saida_4\" "
                        + ", \"marcacao\".\"saida_5\" "
                        + " , \"marcacao\".\"saida_6\" "
                        + " , \"marcacao\".\"saida_7\" "
                        + " , \"marcacao\".\"saida_8\" "
                        + " , \"marcacao\".\"bancohorascre\" "
                        + " , \"marcacao\".\"bancohorasdeb\" "
                        + " , \"marcacao\".\"folga\" "
                        + " , \"funcionario\".\"id\" AS \"idfuncionario\" "
                        + " , \"funcionario\".\"dscodigo\" "
                        + " , \"funcionario\".\"nome\" AS \"funcionario\" "
                        + " , \"funcionario\".\"matricula\" "
                        + " , \"funcionario\".\"dataadmissao\" "
                        + " , \"funcionario\".\"pis\" "
                        + " , \"horariofunc\".\"descricao\" AS \"horario\" "
                        + " , \"funcao\".\"descricao\" AS \"funcao\" "
                        + " , \"departamento\".\"descricao\" AS \"departamento\" "
                        + " , \"empresa\".\"nome\" AS \"empresa\" "
                        + " , \"empresa\".\"codigo\" AS \"codigoempresa\" "
                        + " , case when COALESCE(\"empresa\".\"cnpj\", '') <> '' then \"empresa\".\"cnpj\" else \"empresa\".\"cpf\" end AS \"cnpj_cpf\" "
                        + " , \"empresa\".\"endereco\" "
                        + " , \"empresa\".\"cidade\"  "
                        + " , \"empresa\".\"estado\" "
                        + " , \"empresa\".\"cep\" "
                        + " , \"empresa\".\"id\" AS \"idempresa\""
                        + " , COALESCE(\"horariomarc\".\"tipohorario\", 0) AS \"tipohorario\" "

                        + ", \"horariodetalhenormal\".\"entrada_1\" AS \"entrada_1normal\" "
                        + ", \"horariodetalhenormal\".\"entrada_2\" AS \"entrada_2normal\" "
                        + ", \"horariodetalhenormal\".\"entrada_3\" AS \"entrada_3normal\" "
                        + ", \"horariodetalhenormal\".\"entrada_4\" AS \"entrada_4normal\" "
                        + ", \"horariodetalhenormal\".\"saida_1\" AS \"saida_1normal\" "
                        + ", \"horariodetalhenormal\".\"saida_2\" AS \"saida_2normal\" "
                        + ", \"horariodetalhenormal\".\"saida_3\" AS \"saida_3normal\" "
                        + ", \"horariodetalhenormal\".\"saida_4\" AS \"saida_4normal\" "
                        + ", \"horariodetalhenormal\".\"flagfolga\" AS \"flagfolganormal\" "
                        + ", \"horariodetalheflexivel\".\"entrada_1\" AS \"entrada_1flexivel\" "
                        + ", \"horariodetalheflexivel\".\"entrada_2\" AS \"entrada_2flexivel\" "
                        + ", \"horariodetalheflexivel\".\"entrada_3\" AS \"entrada_3flexivel\" "
                        + ", \"horariodetalheflexivel\".\"entrada_4\" AS \"entrada_4flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_1\" AS \"saida_1flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_2\" AS \"saida_2flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_3\" AS \"saida_3flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_4\" AS \"saida_4flexivel\" "
                        + ", \"horariodetalheflexivel\".\"flagfolga\" AS \"flagfolgaflexivel\" "

                        + " FROM \"marcacao\" "
                        + " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" AND \"funcionario\".\"funcionarioativo\" = 1 "
                        + " INNER JOIN \"horario\" \"horariomarc\" ON \"horariomarc\".\"id\" = \"marcacao\".\"idhorario\" "
                        + " INNER JOIN \"horario\" \"horariofunc\" ON \"horariofunc\".\"id\" = \"funcionario\".\"idhorario\" "
                        + " INNER JOIN \"funcao\" ON \"funcao\".\"id\" = \"funcionario\".\"idfuncao\" "
                        + " INNER JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" "
                        + " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" "
                        
                        + " LEFT JOIN \"horariodetalhe\" \"horariodetalhenormal\" ON \"horariodetalhenormal\".\"idhorario\" = \"marcacao\".\"idhorario\" "
                        + " AND \"horariomarc\".\"tipohorario\" = 1 AND \"horariodetalhenormal\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") END) "
                        + " LEFT JOIN \"horariodetalhe\" \"horariodetalheflexivel\" ON \"horariodetalheflexivel\".\"idhorario\" = \"marcacao\".\"idhorario\" "
                        + " AND \"horariomarc\".\"tipohorario\" = 2 AND \"horariodetalheflexivel\".\"data\" = \"marcacao\".\"data\" " 
                        
                        + " WHERE COALESCE(\"funcionario\".\"excluido\", 0) = 0 AND COALESCE(\"funcionario\".\"funcionarioativo\", 0) = 1"
                        + " AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal";
                        ;

            switch (pTipo)
            {
                case 0: SQL += " AND \"empresa\".\"id\" IN " + pIds; break;
                case 1: SQL += " AND \"departamento\".\"id\" IN " + pIds; break;
                case 2: SQL += " AND \"funcionario\".\"id\" IN " + pIds; break;
            }

            SQL += " ORDER BY \"empresa\".\"id\", \"departamento\".\"id\", \"funcionario\".\"id\", \"marcacao\".\"data\"";

            dt.Load(DataBase.ExecuteReader(CommandType.Text, SQL, parms));

            return dt;
        }

        #endregion

        public int VerificaExiste(int pId, DateTime? pDataInicial, DateTime? pDataFinal, int pTipo, int pIdentificacao)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@datainicial", FbDbType.Date),
                new FbParameter("@datafinal", FbDbType.Date),
                new FbParameter("@tipo", FbDbType.Integer),
                new FbParameter("@identificacao", FbDbType.Integer),
                new FbParameter("@id", FbDbType.Integer)
            };

            parms[0].Value = pDataInicial;
            parms[1].Value = pDataFinal;
            parms[2].Value = pTipo;
            parms[3].Value = pIdentificacao;
            parms[4].Value = pId;

            int qt = (int)FB.DataBase.ExecuteScalar(CommandType.Text, VERIFICA, parms);

            return qt;
        }

        private int VerificaPeriodo(DateTime pData, FbDataReader dr)
        {
            if (!dr.HasRows)
            {
                return 0;
            }

            while (dr.Read())
            {
                if (pData >= Convert.ToDateTime(dr["datainicial"]) && pData <= Convert.ToDateTime(dr["datafinal"]))
                {
                    return Convert.ToInt32(dr["id"]);
                }
            }
            dr.Close();

            return 0;
        }

        public void getInicioFimBH(int pIdBancoHoras, out DateTime? pDtInicio, out DateTime? pDtFim)
        {
            pDtInicio = null;
            pDtFim = null;

            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@id", SqlDbType.Int),
            };
            parms[0].Value = pIdBancoHoras;

            string cmd = "SELECT \"datainicial\", \"datafinal\" "+
                         "FROM \"bancohoras\" "+
                         "WHERE \"id\" = @id";

            FbDataReader dr = FB.DataBase.ExecuteReader(CommandType.Text, cmd, parms);

            if (dr.Read())
            {
                pDtInicio = Convert.ToDateTime(dr["datainicial"]);
                pDtFim = Convert.ToDateTime(dr["datafinal"]);
            }

            return;
        }
        /// <summary>
        /// este metodo pega a data inicial minima de todos os bancos de horas.
        /// </summary>
       


        #endregion



        public Modelo.BancoHoras LoadPorCodigo(int codigo)
        {
            throw new NotImplementedException();
        }


        public List<Modelo.Proxy.PxySaldoBancoHoras> SaldoBancoHoras(DateTime dataSaldo, List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }

        public List<Modelo.Proxy.Relatorios.PxyRelBancoHoras> RelatorioSaldoBancoHoras(string MesInicio, string AnoInicio, string MesFim, string AnoFim, List<int> idsFuncs)
        {
            throw new NotImplementedException();
        }


        public Modelo.BancoHoras BancoHorasPorFuncionario(DateTime data, int idFuncionario)
        {
            throw new NotImplementedException();
        }


        public Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF, List<int> ids)
        {
            throw new NotImplementedException();
        }

        //public DataTable GetCreditoDebitoCalculoBanco(DateTime pInicial, DateTime pFinal, List<int> idsFuncs)
        //{
        //    throw new NotImplementedException();
        //}

        public List<Modelo.BancoHoras> GetAllListFuncs(bool verificaPermissao, List<int> idsFuncs)
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

        public DataTable GetCredDebBancoHorasComSaldoPeriodo(List<int> idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal)
        {
            throw new NotImplementedException();
        }
    }
}
