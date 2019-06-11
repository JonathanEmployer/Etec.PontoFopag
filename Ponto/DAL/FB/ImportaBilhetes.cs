using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;


namespace DAL.FB
{
    public class ImportaBilhetes : IImportaBilhetes
    {
        public Modelo.Cw_Usuario UsuarioLogado { get; set; }
        #region Singleton

        private static volatile FB.ImportaBilhetes _instancia = null;

        public static FB.ImportaBilhetes GetInstancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (typeof(FB.ImportaBilhetes))
                    {
                        if (_instancia == null)
                        {
                            _instancia = new FB.ImportaBilhetes();
                        }
                    }
                }
                return _instancia;
            }
        }

        #endregion

        /// <summary>
        /// Método responsável em retornar um DataTable com todos os bilhetes que não foram importados
        /// </summary>
        /// <param name="pDataI"></param>
        /// <param name="pDataF"></param>
        /// <param name="pDsCodigo"></param>
        /// <returns></returns>
        public Hashtable GetBilhetesImportar(string pDsCodigo, DateTime? pDataI, DateTime? pDataF)
        {
            FbParameter[] parms = new FbParameter[]
			{
                new FbParameter ("@datai", FbDbType.Date),
                new FbParameter ("@dataf", FbDbType.Date),
                new FbParameter ("@dscodigo", FbDbType.VarChar)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;
            parms[2].Value = pDsCodigo;

            StringBuilder sql = new StringBuilder();
            sql.AppendLine("select    \"mar\".\"id\" as \"marcacaoid\"");
            sql.AppendLine(", \"mar\".\"data\" as \"data\"");

            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"entrada_1\", '--:--'))) as \"marcacao_ent1\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"entrada_2\", '--:--'))) as \"marcacao_ent2\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"entrada_3\", '--:--'))) as \"marcacao_ent3\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"entrada_4\", '--:--'))) as \"marcacao_ent4\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"entrada_5\", '--:--'))) as \"marcacao_ent5\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"entrada_6\", '--:--'))) as \"marcacao_ent6\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"entrada_7\", '--:--'))) as \"marcacao_ent7\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"entrada_8\", '--:--'))) as \"marcacao_ent8\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"saida_1\", '--:--'))) as \"marcacao_sai1\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"saida_2\", '--:--'))) as \"marcacao_sai2\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"saida_3\", '--:--'))) as \"marcacao_sai3\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"saida_4\", '--:--'))) as \"marcacao_sai4\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"saida_5\", '--:--'))) as \"marcacao_sai5\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"saida_6\", '--:--'))) as \"marcacao_sai6\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"saida_7\", '--:--'))) as \"marcacao_sai7\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"mar\".\"saida_8\", '--:--'))) as \"marcacao_sai8\"");
            sql.AppendLine(", COALESCE(\"mar\".\"ent_num_relogio_1\", '') as \"ent_num_relogio_1\"");
            sql.AppendLine(", COALESCE(\"mar\".\"ent_num_relogio_2\", '') as \"ent_num_relogio_2\"");
            sql.AppendLine(", COALESCE(\"mar\".\"ent_num_relogio_3\", '') as \"ent_num_relogio_3\"");
            sql.AppendLine(", COALESCE(\"mar\".\"ent_num_relogio_4\", '') as \"ent_num_relogio_4\"");
            sql.AppendLine(", COALESCE(\"mar\".\"ent_num_relogio_5\", '') as \"ent_num_relogio_5\"");
            sql.AppendLine(", COALESCE(\"mar\".\"ent_num_relogio_6\", '') as \"ent_num_relogio_6\"");
            sql.AppendLine(", COALESCE(\"mar\".\"ent_num_relogio_7\", '') as \"ent_num_relogio_7\"");
            sql.AppendLine(", COALESCE(\"mar\".\"ent_num_relogio_8\", '') as \"ent_num_relogio_8\"");
            sql.AppendLine(", COALESCE(\"mar\".\"sai_num_relogio_1\", '') as \"sai_num_relogio_1\"");
            sql.AppendLine(", COALESCE(\"mar\".\"sai_num_relogio_2\", '') as \"sai_num_relogio_2\"");
            sql.AppendLine(", COALESCE(\"mar\".\"sai_num_relogio_3\", '') as \"sai_num_relogio_3\"");
            sql.AppendLine(", COALESCE(\"mar\".\"sai_num_relogio_4\", '') as \"sai_num_relogio_4\"");
            sql.AppendLine(", COALESCE(\"mar\".\"sai_num_relogio_5\", '') as \"sai_num_relogio_5\"");
            sql.AppendLine(", COALESCE(\"mar\".\"sai_num_relogio_6\", '') as \"sai_num_relogio_6\"");
            sql.AppendLine(", COALESCE(\"mar\".\"sai_num_relogio_7\", '') as \"sai_num_relogio_7\"");
            sql.AppendLine(", COALESCE(\"mar\".\"sai_num_relogio_8\", '') as \"sai_num_relogio_8\"");

            sql.AppendLine(", case when \"mar\".\"idhorario\" is null then \"func\".\"idhorario\" else \"mar\".\"idhorario\" end as \"marcacaohorario\"");
            sql.AppendLine(", \"par\".\"inicioadnoturno\" as \"parametro_inicioadnoturno\"");
            sql.AppendLine(", \"par\".\"fimadnoturno\" as \"parametro_fimadnoturno\"");
            sql.AppendLine(", \"hor\".\"ordem_ent\" as \"horario_ordem_ent\"");

            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hor\".\"limitemin\", '--:--'))) as \"horario_limitemin\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hor\".\"limitemax\", '--:--'))) as \"horario_limitemax\"");

            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"entrada_1\", '--:--'))) as \"horario_ent1\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"entrada_2\", '--:--'))) as \"horario_ent2\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"entrada_3\", '--:--'))) as \"horario_ent3\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"entrada_4\", '--:--'))) as \"horario_ent4\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"saida_1\", '--:--'))) as \"horario_sai1\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"saida_2\", '--:--'))) as \"horario_sai2\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"saida_3\", '--:--'))) as \"horario_sai3\"");
            sql.AppendLine(", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"hord\".\"saida_4\", '--:--'))) as \"horario_sai4\"");
            sql.AppendLine(", COALESCE(\"hor\".\"ordenabilhetesaida\", 0) as \"horario_ordenabilhetesaida\"");
            sql.AppendLine(", COALESCE(\"jor\".\"id\",0) as \"jornadaid\"");
            sql.AppendLine(", \"func\".\"id\" as \"funcionarioid\"");
            sql.AppendLine(", \"func\".\"dscodigo\" as \"funcionariodscodigo\"");
            sql.AppendLine(", 0 as \"acao\"");
            sql.AppendLine(", \"mar\".\"tipohoraextrafalta\"");
            sql.AppendLine("from \"marcacao\" as \"mar\"");
            sql.AppendLine("left join \"funcionario\" as \"func\" on \"func\".\"id\" = \"mar\".\"idfuncionario\"");
            sql.AppendLine("left join \"horario\" as \"hor\" on \"hor\".\"id\" = case when \"mar\".\"idhorario\" is null then \"func\".\"idhorario\" else \"mar\".\"idhorario\" end");
            sql.AppendLine("left join \"parametros\" as \"par\" on \"par\".\"id\" = \"hor\".\"idparametro\"");
            sql.AppendLine("left join \"horariodetalhe\" as \"hord\" on \"hord\".\"idhorario\" = case when \"mar\".\"idhorario\" is null then \"func\".\"idhorario\" else \"mar\".\"idhorario\" end ");
            sql.AppendLine("and ( (\"hor\".\"tipohorario\" = 2 ");
            sql.AppendLine("and \"hord\".\"data\" = \"mar\".\"data\"");
            sql.AppendLine(") ");
            sql.AppendLine("or");
            sql.AppendLine("(\"hord\".\"idhorario\" = case when \"mar\".\"idhorario\" is null then \"func\".\"idhorario\" else \"mar\".\"idhorario\" end");
            sql.AppendLine("and \"hor\".\"tipohorario\" = 1 ");
            sql.AppendLine("and \"hord\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"mar\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"mar\".\"data\") end)");
            sql.AppendLine(")");
            sql.AppendLine(")");
            sql.AppendLine("left join \"jornadaalternativa_view\" as \"jor\" on ((\"jor\".\"tipo\" = 0 and \"jor\".\"identificacao\" = \"func\".\"idempresa\")");
            sql.AppendLine("or (\"jor\".\"tipo\" = 1 and \"jor\".\"identificacao\" = \"func\".\"iddepartamento\")");
            sql.AppendLine("or (\"jor\".\"tipo\" = 2 and \"jor\".\"identificacao\" = \"func\".\"id\")");
            sql.AppendLine("or (\"jor\".\"tipo\" = 3 and \"jor\".\"identificacao\" = \"func\".\"idfuncao\"))");
            sql.AppendLine("and (\"jor\".\"datacompensada\" = \"mar\".\"data\"");
            sql.AppendLine("or (\"jor\".\"datacompensada\" is null");
            sql.AppendLine("and \"mar\".\"data\" >= \"jor\".\"datainicial\"");
            sql.AppendLine("and \"mar\".\"data\" <= \"jor\".\"datafinal\"))");
            sql.AppendLine("where \"mar\".\"data\" >= @datai");
            sql.AppendLine("and \"mar\".\"data\" <= @dataf");

            if (!String.IsNullOrEmpty(pDsCodigo))
                sql.AppendLine("and \"func\".\"dscodigo\" = @dscodigo");

            sql.AppendLine("order by \"mar\".\"idfuncionario\", \"mar\".\"data\"");

            DataTable dt = new DataTable();
            
            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, sql.ToString(), parms));

            Hashtable listaHTMarcacao = new Hashtable();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!listaHTMarcacao.ContainsKey(String.Format("{0:dd/MM/yyyy}", dt.Rows[i]["data"]) + dt.Rows[i]["funcionarioid"].ToString()))
                    listaHTMarcacao.Add(String.Format("{0:dd/MM/yyyy}", dt.Rows[i]["data"]) + dt.Rows[i]["funcionarioid"].ToString(), dt.Rows[i]);                
            }

            return listaHTMarcacao;
        }

        public string MontaStringBilhetesImp(DataRow drBilhete)
        {
            Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
            StringBuilder sql = new StringBuilder();

            objBilhete.Ordem = (string)drBilhete["ordem"];
            objBilhete.Data = Convert.ToDateTime(drBilhete["data"]);
            objBilhete.Hora = Modelo.cwkFuncoes.ConvertMinutosBatida((int)drBilhete["hora"]);
            objBilhete.Func = (string)drBilhete["func"];
            objBilhete.Relogio = (string)drBilhete["relogio"];
            objBilhete.Importado = Convert.ToInt16(drBilhete["importado"]);
            objBilhete.Mar_data = Convert.ToDateTime(drBilhete["mar_data"]);
            objBilhete.Mar_hora = (string)drBilhete["mar_hora"];
            objBilhete.Mar_relogio = (string)drBilhete["mar_relogio"];
            objBilhete.Posicao = Convert.ToInt32(drBilhete["posicao"]);
            objBilhete.Ent_sai = (string)drBilhete["ent_sai"];
            objBilhete.DsCodigo = (string)drBilhete["bildscodigo"];

            sql.AppendLine("update \"bilhetesimp\"");
            sql.AppendLine("set \"mar_data\" = '" + String.Format("{0:MM/dd/yyyy}", objBilhete.Mar_data) + "'");
            sql.AppendLine(", \"mar_hora\" = '" + objBilhete.Mar_hora + "'");
            sql.AppendLine(", \"mar_relogio\" = '" + objBilhete.Mar_relogio + "'");
            sql.AppendLine(", \"importado\" = " + objBilhete.Importado);
            sql.AppendLine(", \"ent_sai\" = '" + objBilhete.Ent_sai + "'");
            sql.AppendLine(", \"posicao\" = " + objBilhete.Posicao);
            sql.AppendLine(", \"dscodigo\" = '" + objBilhete.DsCodigo + "'");
            sql.AppendLine(", \"chave\" = '" + objBilhete.ToMD5() + "'");
            sql.AppendLine("where \"id\"= " + (int)drBilhete["bimp_id"]);

            return sql.ToString();
        }

        public string MontaComandoMarcacao(Modelo.Marcacao pObjMarcacao)
        {
            string comando = "";

            if (pObjMarcacao.Acao == Modelo.Acao.Alterar)
            {
                comando = MontaComandoUpdate(pObjMarcacao);
            }
            else if (pObjMarcacao.Acao == Modelo.Acao.Incluir)
            {
                comando = MontaComandoInsert(pObjMarcacao);
            }

            return comando;
        }

        private string MontaComandoInsert(Modelo.Marcacao pObjMarcacao)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("insert into \"marcacao\" (");
            sql.AppendLine("\"idfuncionario\"");
            sql.AppendLine(", \"dscodigo\"");
            sql.AppendLine(", \"data\"");
            sql.AppendLine(", \"idhorario\"");
            sql.AppendLine(", \"entrada_1\"");
            sql.AppendLine(", \"entrada_2\"");
            sql.AppendLine(", \"entrada_3\"");
            sql.AppendLine(", \"entrada_4\"");
            sql.AppendLine(", \"entrada_5\"");
            sql.AppendLine(", \"entrada_6\"");
            sql.AppendLine(", \"entrada_7\"");
            sql.AppendLine(", \"entrada_8\"");
            sql.AppendLine(", \"saida_1\"");
            sql.AppendLine(", \"saida_2\"");
            sql.AppendLine(", \"saida_3\"");
            sql.AppendLine(", \"saida_4\"");
            sql.AppendLine(", \"saida_5\"");
            sql.AppendLine(", \"saida_6\"");
            sql.AppendLine(", \"saida_7\"");
            sql.AppendLine(", \"saida_8\"");
            sql.AppendLine(", \"ent_num_relogio_1\"");
            sql.AppendLine(", \"ent_num_relogio_2\"");
            sql.AppendLine(", \"ent_num_relogio_3\"");
            sql.AppendLine(", \"ent_num_relogio_4\"");
            sql.AppendLine(", \"ent_num_relogio_5\"");
            sql.AppendLine(", \"ent_num_relogio_6\"");
            sql.AppendLine(", \"ent_num_relogio_7\"");
            sql.AppendLine(", \"ent_num_relogio_8\"");
            sql.AppendLine(", \"sai_num_relogio_1\"");
            sql.AppendLine(", \"sai_num_relogio_2\"");
            sql.AppendLine(", \"sai_num_relogio_3\"");
            sql.AppendLine(", \"sai_num_relogio_4\"");
            sql.AppendLine(", \"sai_num_relogio_5\"");
            sql.AppendLine(", \"sai_num_relogio_6\"");
            sql.AppendLine(", \"sai_num_relogio_7\"");
            sql.AppendLine(", \"sai_num_relogio_8\"");
            sql.AppendLine(", \"horastrabalhadas\"");
            sql.AppendLine(", \"horasextrasdiurna\"");
            sql.AppendLine(", \"horasfaltas\"");
            sql.AppendLine(", \"entradaextra\"");
            sql.AppendLine(", \"saidaextra\"");
            sql.AppendLine(", \"horastrabalhadasnoturnas\"");
            sql.AppendLine(", \"horasextranoturna\"");
            sql.AppendLine(", \"horasfaltanoturna\"");
            sql.AppendLine(", \"bancohorascre\"");
            sql.AppendLine(", \"bancohorasdeb\"");
            sql.AppendLine(", \"incdata\"");
            sql.AppendLine(", \"inchora\"");
            sql.AppendLine(", \"incusuario\"");
            sql.AppendLine(", \"codigo\"");
            sql.AppendLine(", \"chave\"");
            sql.AppendLine(", \"exphorasextranoturna\"");
            sql.AppendLine(", \"tipohoraextrafalta\"");
            sql.AppendLine(")");
            sql.AppendLine("values");
            sql.AppendLine("(");
            sql.AppendLine(pObjMarcacao.Idfuncionario.ToString());
            sql.AppendLine(", '" + pObjMarcacao.Dscodigo + "'");
            sql.AppendLine(", '" + String.Format("{0:MM/dd/yyyy}", pObjMarcacao.Data.Date) + "'");
            sql.AppendLine(", " + pObjMarcacao.Idhorario);
            sql.AppendLine(", '" + pObjMarcacao.Entrada_1 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Entrada_2 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Entrada_3 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Entrada_4 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Entrada_5 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Entrada_6 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Entrada_7 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Entrada_8 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Saida_1 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Saida_2 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Saida_3 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Saida_4 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Saida_5 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Saida_6 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Saida_7 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Saida_8 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Ent_num_relogio_1 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Ent_num_relogio_2 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Ent_num_relogio_3 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Ent_num_relogio_4 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Ent_num_relogio_5 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Ent_num_relogio_6 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Ent_num_relogio_7 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Ent_num_relogio_8 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Sai_num_relogio_1 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Sai_num_relogio_2 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Sai_num_relogio_3 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Sai_num_relogio_4 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Sai_num_relogio_5 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Sai_num_relogio_6 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Sai_num_relogio_7 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Sai_num_relogio_8 + "'");
            sql.AppendLine(", '" + pObjMarcacao.Horastrabalhadas + "'");
            sql.AppendLine(", '" + pObjMarcacao.Horasextrasdiurna + "'");
            sql.AppendLine(", '" + pObjMarcacao.Horasfaltas + "'");
            sql.AppendLine(", '" + pObjMarcacao.Entradaextra + "'");
            sql.AppendLine(", '" + pObjMarcacao.Saidaextra + "'");
            sql.AppendLine(", '" + pObjMarcacao.Horastrabalhadasnoturnas + "'");
            sql.AppendLine(", '" + pObjMarcacao.Horasextranoturna + "'");
            sql.AppendLine(", '" + pObjMarcacao.Horasfaltanoturna + "'");
            sql.AppendLine(", '" + pObjMarcacao.Bancohorascre + "'");
            sql.AppendLine(", '" + pObjMarcacao.Bancohorasdeb + "'");
            sql.AppendLine(", '" + String.Format("{0:MM/dd/yyyy}", pObjMarcacao.Incdata) + "'");
            sql.AppendLine(", '" + String.Format("{0:MM/dd/yyyy hh:mm:ss}", pObjMarcacao.Inchora) + "'");
            sql.AppendLine(", '" + pObjMarcacao.Incusuario + "'");
            sql.AppendLine(", " + pObjMarcacao.Codigo);
            sql.AppendLine(", '" + pObjMarcacao.Chave + "'");
            sql.AppendLine(", '" + pObjMarcacao.ExpHorasextranoturna + "'");
            sql.AppendLine(", " + pObjMarcacao.TipoHoraExtraFalta);
            sql.AppendLine(")");

            return sql.ToString();
        }

        public string MontaComandoUpdate(Modelo.Marcacao pObjMarcacao)
        {
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("UPDATE \"marcacao\"");
            sql.AppendLine("SET");
            sql.AppendLine("\"entrada_1\" = '" + pObjMarcacao.Entrada_1 + "'");
            sql.AppendLine(", \"entrada_2\" = '" + pObjMarcacao.Entrada_2 + "'");
            sql.AppendLine(", \"entrada_3\" = '" + pObjMarcacao.Entrada_3 + "'");
            sql.AppendLine(", \"entrada_4\" = '" + pObjMarcacao.Entrada_4 + "'");
            sql.AppendLine(", \"entrada_5\" = '" + pObjMarcacao.Entrada_5 + "'");
            sql.AppendLine(", \"entrada_6\" = '" + pObjMarcacao.Entrada_6 + "'");
            sql.AppendLine(", \"entrada_7\" = '" + pObjMarcacao.Entrada_7 + "'");
            sql.AppendLine(", \"entrada_8\" = '" + pObjMarcacao.Entrada_8 + "'");
            sql.AppendLine(", \"saida_1\" = '" + pObjMarcacao.Saida_1 + "'");
            sql.AppendLine(", \"saida_2\" = '" + pObjMarcacao.Saida_2 + "'");
            sql.AppendLine(", \"saida_3\" = '" + pObjMarcacao.Saida_3 + "'");
            sql.AppendLine(", \"saida_4\" = '" + pObjMarcacao.Saida_4 + "'");
            sql.AppendLine(", \"saida_5\" = '" + pObjMarcacao.Saida_5 + "'");
            sql.AppendLine(", \"saida_6\" = '" + pObjMarcacao.Saida_6 + "'");
            sql.AppendLine(", \"saida_7\" = '" + pObjMarcacao.Saida_7 + "'");
            sql.AppendLine(", \"saida_8\" = '" + pObjMarcacao.Saida_8 + "'");
            sql.AppendLine(", \"horastrabalhadas\" = '" + pObjMarcacao.Horastrabalhadas + "'");
            sql.AppendLine(", \"horasextrasdiurna\" = '" + pObjMarcacao.Horasextrasdiurna + "'");
            sql.AppendLine(", \"horasfaltas\" = '" + pObjMarcacao.Horasfaltas + "'");
            sql.AppendLine(", \"entradaextra\" = '" + pObjMarcacao.Entradaextra + "'");
            sql.AppendLine(", \"saidaextra\" = '" + pObjMarcacao.Saidaextra + "'");
            sql.AppendLine(", \"horastrabalhadasnoturnas\" = '" + pObjMarcacao.Horastrabalhadasnoturnas + "'");
            sql.AppendLine(", \"horasextranoturna\" = '" + pObjMarcacao.Horasextranoturna + "'");
            sql.AppendLine(", \"horasfaltanoturna\" = '" + pObjMarcacao.Horasfaltanoturna + "'");
            sql.AppendLine(", \"ent_num_relogio_1\" = '" + pObjMarcacao.Ent_num_relogio_1 + "'");
            sql.AppendLine(", \"ent_num_relogio_2\" = '" + pObjMarcacao.Ent_num_relogio_2 + "'");
            sql.AppendLine(", \"ent_num_relogio_3\" = '" + pObjMarcacao.Ent_num_relogio_3 + "'");
            sql.AppendLine(", \"ent_num_relogio_4\" = '" + pObjMarcacao.Ent_num_relogio_4 + "'");
            sql.AppendLine(", \"ent_num_relogio_5\" = '" + pObjMarcacao.Ent_num_relogio_5 + "'");
            sql.AppendLine(", \"ent_num_relogio_6\" = '" + pObjMarcacao.Ent_num_relogio_6 + "'");
            sql.AppendLine(", \"ent_num_relogio_7\" = '" + pObjMarcacao.Ent_num_relogio_7 + "'");
            sql.AppendLine(", \"ent_num_relogio_8\" = '" + pObjMarcacao.Ent_num_relogio_8 + "'");
            sql.AppendLine(", \"sai_num_relogio_1\" = '" + pObjMarcacao.Sai_num_relogio_1 + "'");
            sql.AppendLine(", \"sai_num_relogio_2\" = '" + pObjMarcacao.Sai_num_relogio_2 + "'");
            sql.AppendLine(", \"sai_num_relogio_3\" = '" + pObjMarcacao.Sai_num_relogio_3 + "'");
            sql.AppendLine(", \"sai_num_relogio_4\" = '" + pObjMarcacao.Sai_num_relogio_4 + "'");
            sql.AppendLine(", \"sai_num_relogio_5\" = '" + pObjMarcacao.Sai_num_relogio_5 + "'");
            sql.AppendLine(", \"sai_num_relogio_6\" = '" + pObjMarcacao.Sai_num_relogio_6 + "'");
            sql.AppendLine(", \"sai_num_relogio_7\" = '" + pObjMarcacao.Sai_num_relogio_7 + "'");
            sql.AppendLine(", \"sai_num_relogio_8\" = '" + pObjMarcacao.Sai_num_relogio_8 + "'");
            sql.AppendLine(", \"altdata\" = '" + String.Format("{0:MM/dd/yyyy}", System.DateTime.Today) + "'");
            sql.AppendLine(", \"althora\" = '" + String.Format("{0:MM/dd/yyyy hh:mm:ss}", System.DateTime.Now) + "'");
            sql.AppendLine(", \"altusuario\" = '" + cwkControleUsuario.Facade.getUsuarioLogado.Login + "'");
            sql.AppendLine(", \"chave\" = '" + pObjMarcacao.Chave + "'");
            sql.AppendLine("where \"id\" = " + pObjMarcacao.Id);

            return sql.ToString();
        }

        public DataTable GetFuncionariosImportacao(string dscodigo)
        {
            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@dscodigo", FbDbType.Integer),
            };
            parms[0].Value = dscodigo;

            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("SELECT");
            cmd.AppendLine("\"funcionario\".\"id\"");
            cmd.AppendLine(", \"funcionario\".\"dscodigo\"");
            cmd.AppendLine(", \"funcionario\".\"idhorario\"");
            cmd.AppendLine(", \"parametros\".\"tipohoraextrafalta\"");
            cmd.AppendLine("FROM \"funcionario\"");
            cmd.AppendLine("INNER JOIN \"horario\" ON \"horario\".\"id\" = \"funcionario\".\"idhorario\"");
            cmd.AppendLine("INNER JOIN \"parametros\" ON \"parametros\".\"id\" = \"horario\".\"idparametro\"");
            if (!String.IsNullOrEmpty(dscodigo))
                cmd.AppendLine("WHERE \"funcionario\".\"dscodigo\" = @dscodigo");
            DataTable dt = new DataTable();
            dt.Load(DataBase.ExecuteReader(CommandType.Text, cmd.ToString(), parms));
            return dt;
        }

        public short GetTipoHExtraFalta(int idhorario)
        {
            FbParameter[] parms = new FbParameter[] 
            {
                new FbParameter("@idhorario", FbDbType.Integer),
            };
            parms[0].Value = idhorario;

            StringBuilder cmd = new StringBuilder();
            cmd.AppendLine("SELECT");
            cmd.AppendLine("\"parametros\".\"tipohoraextrafalta\"");
            cmd.AppendLine("FROM \"horario\"");
            cmd.AppendLine("INNER JOIN \"parametros\"");
            cmd.AppendLine("ON \"parametros\".\"id\" = @idhorario");

            FbDataReader dr = DataBase.ExecuteReader(CommandType.Text, cmd.ToString(), parms);
            if (dr.Read())
            {
                return Convert.ToInt16(dr["tipohoraextrafalta"]);
            }
            return (short)0;
        }

        public void PersisteDados(List<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes)
        {
            throw new NotImplementedException();
        }


        #region IImportaBilhetes Members

        public Dictionary<int, short> GetTipoHExtraFaltaHorarios(List<int> idHorarios)
        {
            throw new NotImplementedException();
        }

        #endregion


        public DataTable GetFuncionariosImportacaoWebApi(string dscodigo)
        {
            throw new NotImplementedException();
        }


        public void PersisteDadosWebApi(List<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes, string login)
        {
            throw new NotImplementedException();
        }

        public DataTable GetFuncsWithoutDscodigo(DateTime? dataI, DateTime? dataF)
        {
            throw new NotImplementedException();
        }
    }
}
