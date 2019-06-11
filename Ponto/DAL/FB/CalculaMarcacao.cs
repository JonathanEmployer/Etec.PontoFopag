using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;

namespace DAL.FB
{
    public class CalculaMarcacao : DAL.ICalculaMarcacao
    {
        public Modelo.Cw_Usuario UsuarioLogado { get; set; }
        #region Calculo DSR
        public DataTable GetFuncionariosDSR(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@identificacao", FbDbType.Integer),
                new FbParameter("@datai", FbDbType.Date),
                new FbParameter("@dataf", FbDbType.Date)
            };

            parms[0].Value = pIdentificacao;
            parms[1].Value = pDataI;
            parms[2].Value = pDataF;

            string comando = "SELECT \"marcacao\".\"idfuncionario\", \"funcionario\".\"nome\", SUM(COALESCE(\"horario\".\"descontardsr\", 0)) AS qtddsr "
                             + " FROM \"marcacao\""
                             + " INNER JOIN \"horario\" ON \"horario\".\"id\" = \"marcacao\".\"idhorario\""
                             + " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\""
                             + " WHERE \"marcacao\".\"data\" >= @datai AND \"marcacao\".\"data\" <= @dataf";
            if (pTipo != null)
            {
                switch (pTipo.Value)
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

            comando += " GROUP BY \"marcacao\".\"idfuncionario\", \"funcionario\".\"nome\"";

            DataTable dt = new DataTable();
            dt.Load(DataBase.ExecuteReader(CommandType.Text, comando, parms));
            return dt;
        }

        public DataTable GetMarcacoesDSR(int pIdFuncionario, DateTime pDataI, DateTime pDataF)
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

            StringBuilder comando = new StringBuilder("SELECT \"marcacao\".\"id\"");
            comando.AppendLine(", \"marcacao\".\"dscodigo\"");
            comando.AppendLine(", \"marcacao\".\"dsr\"");
            comando.AppendLine(", \"marcacao\".\"legenda\"");            
            comando.AppendLine(", \"marcacao\".\"idfuncionario\"");
            comando.AppendLine(", \"marcacao\".\"dia\"");
            comando.AppendLine(", \"marcacao\".\"data\"");
            comando.AppendLine(", \"marcacao\".\"ocorrencia\"");
            comando.AppendLine(", \"marcacao\".\"horastrabalhadas\"");
            comando.AppendLine(", \"marcacao\".\"horastrabalhadasnoturnas\"");
            comando.AppendLine(", \"marcacao\".\"horasextrasdiurna\"");
            comando.AppendLine(", \"marcacao\".\"horasextranoturna\"");
            comando.AppendLine(", \"marcacao\".\"horasfaltas\"");
            comando.AppendLine(", \"marcacao\".\"horasfaltanoturna\"");
            comando.AppendLine(", \"marcacao\".\"bancohorascre\"");
            comando.AppendLine(", \"marcacao\".\"bancohorasdeb\"");
            comando.AppendLine(", \"marcacao\".\"folga\"");
            comando.AppendLine(", \"marcacao\".\"naoconsiderarcafe\"");
            comando.AppendLine(", \"marcacao\".\"naoentrarbanco\"");
            comando.AppendLine(", \"marcacao\".\"semcalculo\"");
            comando.AppendLine(", \"marcacao\".\"horascompensadas\"");
            comando.AppendLine(", \"marcacao\".\"idcompensado\"");
            comando.AppendLine(", \"marcacao\".\"idhorario\"");
            comando.AppendLine(", \"marcacao\".\"entrada_1\"");
            comando.AppendLine(", \"marcacao\".\"entrada_2\"");
            comando.AppendLine(", \"marcacao\".\"entrada_3\"");
            comando.AppendLine(", \"marcacao\".\"entrada_4\"");
            comando.AppendLine(", \"marcacao\".\"entrada_5\"");
            comando.AppendLine(", \"marcacao\".\"entrada_6\"");
            comando.AppendLine(", \"marcacao\".\"entrada_7\"");
            comando.AppendLine(", \"marcacao\".\"entrada_8\"");
            comando.AppendLine(", \"marcacao\".\"saida_1\"");
            comando.AppendLine(", \"marcacao\".\"saida_2\"");
            comando.AppendLine(", \"marcacao\".\"saida_3\"");
            comando.AppendLine(", \"marcacao\".\"saida_4\"");
            comando.AppendLine(", \"marcacao\".\"saida_5\"");
            comando.AppendLine(", \"marcacao\".\"saida_6\"");
            comando.AppendLine(", \"marcacao\".\"saida_7\"");
            comando.AppendLine(", \"marcacao\".\"saida_8\"");
            comando.AppendLine(", \"marcacao\".\"entradaextra\"");
            comando.AppendLine(", \"marcacao\".\"saidaextra\"");
            comando.AppendLine(", \"marcacao\".\"ent_num_relogio_1\"");
            comando.AppendLine(", \"marcacao\".\"ent_num_relogio_2\"");
            comando.AppendLine(", \"marcacao\".\"ent_num_relogio_3\"");
            comando.AppendLine(", \"marcacao\".\"ent_num_relogio_4\"");
            comando.AppendLine(", \"marcacao\".\"ent_num_relogio_5\"");
            comando.AppendLine(", \"marcacao\".\"ent_num_relogio_6\"");
            comando.AppendLine(", \"marcacao\".\"ent_num_relogio_7\"");
            comando.AppendLine(", \"marcacao\".\"ent_num_relogio_8\"");
            comando.AppendLine(", \"marcacao\".\"sai_num_relogio_1\"");
            comando.AppendLine(", \"marcacao\".\"sai_num_relogio_2\"");
            comando.AppendLine(", \"marcacao\".\"sai_num_relogio_3\"");
            comando.AppendLine(", \"marcacao\".\"sai_num_relogio_4\"");
            comando.AppendLine(", \"marcacao\".\"sai_num_relogio_5\"");
            comando.AppendLine(", \"marcacao\".\"sai_num_relogio_6\"");
            comando.AppendLine(", \"marcacao\".\"sai_num_relogio_7\"");
            comando.AppendLine(", \"marcacao\".\"sai_num_relogio_8\"");
            comando.AppendLine(", \"marcacao\".\"naoentrarnacompensacao\"");
            comando.AppendLine(", \"marcacao\".\"idfechamentobh\"");
            comando.AppendLine(", \"marcacao\".\"abonardsr\"");
            comando.AppendLine(", \"marcacao\".\"totalizadoresalterados\"");
            comando.AppendLine(", \"marcacao\".\"calchorasextrasdiurna\"");
            comando.AppendLine(", \"marcacao\".\"calchorasextranoturna\"");
            comando.AppendLine(", \"marcacao\".\"calchorasfaltas\"");
            comando.AppendLine(", \"marcacao\".\"calchorasfaltanoturna\"");
            comando.AppendLine(", \"marcacao\".\"incdata\"");
            comando.AppendLine(", \"marcacao\".\"inchora\"");
            comando.AppendLine(", \"marcacao\".\"incusuario\"");
            comando.AppendLine(", \"marcacao\".\"codigo\"");
            comando.AppendLine(", (CAST((SELECT RET FROM CONVERTHORAMINUTO(\"marcacao\".\"horasfaltas\")) AS INTEGER)");
            comando.AppendLine("+ CAST((SELECT RET FROM CONVERTHORAMINUTO(\"marcacao\".\"horasfaltanoturna\")) AS INTEGER)) AS \"horasfaltasdsr\"");
            comando.AppendLine(", (SELECT RET FROM CONVERTHORAMINUTO(\"marcacao\".\"valordsr\")) AS \"valordsrmin\"");
            comando.AppendLine(", \"horario\".\"diasemanadsr\"");
            comando.AppendLine(", \"horario\".\"descontardsr\"");
            comando.AppendLine(", \"horario\".\"DescontoHorasDSR\"");
            comando.AppendLine(", (SELECT RET FROM CONVERTHORAMINUTO(\"horario\".\"limiteperdadsr\")) AS \"limiteperdadsr\"");
            comando.AppendLine(", (SELECT RET FROM CONVERTHORAMINUTO(\"horario\".\"qtdhorasdsr\")) AS \"qtdhorasdsrmin\"");
            comando.AppendLine(", COALESCE(\"marcacao\".\"exphorasextranoturna\", '--:--') AS exphorasextranoturna ");
            comando.AppendLine(", \"horario\".\"DSRPorPercentual\" ");
            comando.AppendLine(" FROM \"marcacao\"");
            comando.AppendLine(" INNER JOIN \"horario\" ON \"horario\".\"id\" = \"marcacao\".\"idhorario\"");
            comando.AppendLine(" WHERE \"marcacao\".\"idfuncionario\" = @idfuncionario");
            comando.AppendLine(" AND \"marcacao\".\"data\" >= @datai AND \"marcacao\".\"data\" <= @dataf");
            comando.AppendLine("ORDER BY \"marcacao\".\"data\"");

            DataTable dt = new DataTable();
            dt.Load(DataBase.ExecuteReader(CommandType.Text, comando.ToString(), parms));
            return dt;
        }
        #endregion

        #region Calculo Marcação
        /// <summary>
        /// Pega as marcações para realizar o calculo
        /// </summary>
        /// <param name="pTipo">Tipo</param>
        /// <param name="identificacao">Identificação</param>
        /// <param name="pDataI">Data inicial</param>
        /// <param name="pDataF">Data final</param>
        /// <param name="PegaInativos">true = pega inativos, false = não pega os inativos</param>
        /// <returns>Datatable com as marcações e outros dados.</returns>
        /// 

        public string GetLegenda(int pidFuncionario, DateTime pData)
        {
            FbParameter[] parms = new FbParameter[2]
            { 
                    new FbParameter("@data", FbDbType.Date),
                    new FbParameter("@idfuncionario", FbDbType.Integer)
            };
            parms[0].Value = pData;
            parms[1].Value = pidFuncionario;

            string aux = "SELECT \"legenda\" FROM \"marcacao\" WHERE \"data\" = @data AND \"idfuncionario\" = @idfuncionario";
            
            return (string)FB.DataBase.ExecuteScalar(CommandType.Text, aux, parms);
           
        }

        public DataTable GetMarcacoesCalculo(int? pTipo, int identificacao, DateTime pDataI, DateTime pDataF, bool pegaInativos, bool pegaExcluidos, bool recalculaBHFechado)
        {
            FbParameter[] parms = new FbParameter[2]
            { 
                    new FbParameter("@datai", FbDbType.Date),
                    new FbParameter("@dataf", FbDbType.Date)
            };
            parms[0].Value = pDataI;
            parms[1].Value = pDataF;

            string aux = "SELECT \"marcacao\".\"id\" " +
                        ", \"marcacao\".\"codigo\" " +
                        ", \"marcacao\".\"idcompensado\" " +
                        ", \"marcacao\".\"data\" " +
                        ", \"marcacao\".\"dia\" " +
                        ", \"marcacao\".\"ocorrencia\" " +
                        ", \"marcacao\".\"abonardsr\" " +
                        ", \"marcacao\".\"totalizadoresalterados\" " +
                        ", \"marcacao\".\"calchorasextrasdiurna\" " +
                        ", \"marcacao\".\"calchorasextranoturna\" " +
                        ", \"marcacao\".\"calchorasfaltas\" " +
                        ", \"marcacao\".\"calchorasfaltanoturna\" " +
                        ", \"marcacao\".\"naoentrarbanco\" " +
                        ", \"marcacao\".\"naoentrarnacompensacao\" " +
                        ", \"marcacao\".\"naoconsiderarcafe\" " +
                        ", \"marcacao\".\"semcalculo\" " +
                        ", \"marcacao\".\"folga\" " +
                        ", \"marcacao\".\"dscodigo\" " +
                        ", \"marcacao\".\"idfechamentobh\" " +
                        ", \"marcacao\".\"idhorario\" " +
                        ", \"marcacao\".\"idfuncionario\" " +
                        ", \"marcacao\".\"entradaextra\" " +
                        ", \"marcacao\".\"saidaextra\" " +
                        ", \"marcacao\".\"entrada_1\" " +
                        ", \"marcacao\".\"entrada_2\" " +
                        ", \"marcacao\".\"entrada_3\" " +
                        ", \"marcacao\".\"entrada_4\" " +
                        ", \"marcacao\".\"entrada_5\" " +
                        ", \"marcacao\".\"entrada_6\" " +
                        ", \"marcacao\".\"entrada_7\" " +
                        ", \"marcacao\".\"entrada_8\" " +
                        ", \"marcacao\".\"saida_1\" " +
                        ", \"marcacao\".\"saida_2\" " +
                        ", \"marcacao\".\"saida_3\" " +
                        ", \"marcacao\".\"saida_4\" " +
                        ", \"marcacao\".\"saida_5\" " +
                        ", \"marcacao\".\"saida_6\" " +
                        ", \"marcacao\".\"saida_7\" " +
                        ", \"marcacao\".\"saida_8\" " +
                        ", \"marcacao\".\"ent_num_relogio_1\" " +
                        ", \"marcacao\".\"ent_num_relogio_2\" " +
                        ", \"marcacao\".\"ent_num_relogio_3\" " +
                        ", \"marcacao\".\"ent_num_relogio_4\" " +
                        ", \"marcacao\".\"ent_num_relogio_5\" " +
                        ", \"marcacao\".\"ent_num_relogio_6\" " +
                        ", \"marcacao\".\"ent_num_relogio_7\" " +
                        ", \"marcacao\".\"ent_num_relogio_8\" " +
                        ", \"marcacao\".\"sai_num_relogio_1\" " +
                        ", \"marcacao\".\"sai_num_relogio_2\" " +
                        ", \"marcacao\".\"sai_num_relogio_3\" " +
                        ", \"marcacao\".\"sai_num_relogio_4\" " +
                        ", \"marcacao\".\"sai_num_relogio_5\" " +
                        ", \"marcacao\".\"sai_num_relogio_6\" " +
                        ", \"marcacao\".\"sai_num_relogio_7\" " +
                        ", \"marcacao\".\"sai_num_relogio_8\" " +
                        ", \"marcacao\".\"incdata\" " +
                        ", \"marcacao\".\"inchora\" " +
                        ", \"marcacao\".\"incusuario\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"horastrabalhadas\", '--:--'))) AS \"horastrabalhadasmin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"horastrabalhadasnoturnas\", '--:--'))) AS \"horastrabalhadasnoturnasmin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"horasfaltas\", '--:--'))) AS \"horasfaltasmin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"horasfaltanoturna\", '--:--'))) AS \"horasfaltanoturnamin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"horasextrasdiurna\", '--:--'))) AS \"horasextrasdiurnamin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"horasextranoturna\", '--:--'))) AS \"horasextranoturnamin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"valordsr\", '--:--'))) AS \"valordsrmin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"marcacao\".\"horascompensadas\", '--:--'))) AS \"horascompensadasmin\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"entrada_1\", '--:--'))) AS \"entrada_1min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"entrada_2\", '--:--'))) AS \"entrada_2min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"entrada_3\", '--:--'))) AS \"entrada_3min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"entrada_4\", '--:--'))) AS \"entrada_4min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"entrada_5\", '--:--'))) AS \"entrada_5min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"entrada_6\", '--:--'))) AS \"entrada_6min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"entrada_7\", '--:--'))) AS \"entrada_7min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"entrada_8\", '--:--'))) AS \"entrada_8min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"saida_1\", '--:--'))) AS \"saida_1min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"saida_2\", '--:--'))) AS \"saida_2min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"saida_3\", '--:--'))) AS \"saida_3min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"saida_4\", '--:--'))) AS \"saida_4min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"saida_5\", '--:--'))) AS \"saida_5min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"saida_6\", '--:--'))) AS \"saida_6min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"saida_7\", '--:--'))) AS \"saida_7min\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"marcacao\".\"saida_8\", '--:--'))) AS \"saida_8min\" " +
                        ", \"funcionario\".\"idfuncao\" " +
                        ", \"funcionario\".\"iddepartamento\" " +
                        ", \"funcionario\".\"idempresa\" " +
                        ", \"funcionario\".\"dataadmissao\" " +
                        ", \"funcionario\".\"datademissao\" " +
                        ", \"funcionario\".\"naoentrarbanco\" AS \"naoentrarbancofunc\" " +
                        ", \"funcionario\".\"naoentrarcompensacao\" AS \"naocompensacaofunc\" " +
                        ", \"horario\".\"tipohorario\" " +
                        ", \"horario\".\"consideraadhtrabalhadas\" " +
                        ", \"horario\".\"conversaohoranoturna\" " +
                        ", \"horario\".\"dias_cafe_1\" " +
                        ", \"horario\".\"dias_cafe_2\" " +
                        ", \"horario\".\"dias_cafe_3\" " +
                        ", \"horario\".\"dias_cafe_4\" " +
                        ", \"horario\".\"dias_cafe_5\" " +
                        ", \"horario\".\"dias_cafe_6\" " +
                        ", \"horario\".\"dias_cafe_7\" " +
                        ", \"horario\".\"diasemanadsr\" " +
                        ", \"horario\".\"marcacargahorariamista\" AS \"marcacargahorariamistahorario\" " +
                        ", \"horario\".\"habilitaperiodo01\" " +
                        ", \"horario\".\"habilitaperiodo02\" " +
                        ", \"horario\".\"consideraradicionalnoturnointerv\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"horario\".\"limitemax\", '--:--'))) AS \"limitemax\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"horario\".\"limitemin\", '--:--'))) AS \"limitemin\" " +
                        ", COALESCE(\"horario\".\"ordenabilhetesaida\", 0) AS \"ordenabilhetesaida\" " +
                        ", \"horariodetalhe\".\"diadsr\" " +
                        ", \"horariodetalhe\".\"intervaloautomatico\" AS \"intervaloautomatico\" " +
                        ", \"horariodetalhe\".\"preassinaladas1\" AS \"preassinaladas1\" " +
                        ", \"horariodetalhe\".\"preassinaladas2\" AS \"preassinaladas2\" " +
                        ", \"horariodetalhe\".\"preassinaladas3\" AS \"preassinaladas3\" " +
                        ", \"horariodetalhe\".\"dia\" AS \"dia\" " +
                        ", \"horariodetalhe\".\"entrada_1\" AS \"entrada_1hd\" " +
                        ", \"horariodetalhe\".\"entrada_2\" AS \"entrada_2hd\" " +
                        ", \"horariodetalhe\".\"entrada_3\" AS \"entrada_3hd\" " +
                        ", \"horariodetalhe\".\"entrada_4\" AS \"entrada_4hd\" " +
                        ", \"horariodetalhe\".\"saida_1\" AS \"saida_1hd\" " +
                        ", \"horariodetalhe\".\"saida_2\" AS \"saida_2hd\" " +
                        ", \"horariodetalhe\".\"saida_3\" AS \"saida_3hd\" " +
                        ", \"horariodetalhe\".\"saida_4\" AS \"saida_4hd\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"horariodetalhe\".\"entrada_1\", '--:--'))) AS \"entrada_1minhd\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"horariodetalhe\".\"entrada_2\", '--:--'))) AS \"entrada_2minhd\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"horariodetalhe\".\"entrada_3\", '--:--'))) AS \"entrada_3minhd\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"horariodetalhe\".\"entrada_4\", '--:--'))) AS \"entrada_4minhd\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"horariodetalhe\".\"saida_1\", '--:--'))) AS \"saida_1minhd\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"horariodetalhe\".\"saida_2\", '--:--'))) AS \"saida_2minhd\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"horariodetalhe\".\"saida_3\", '--:--'))) AS \"saida_3minhd\" " +
                        ", (SELECT RET FROM CONVERTBATIDAMINUTO(COALESCE(\"horariodetalhe\".\"saida_4\", '--:--'))) AS \"saida_4minhd\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"horariodetalhe\".\"totaltrabalhadadiurna\", '--:--'))) AS \"totaltrabalhadadiurnamin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"horariodetalhe\".\"totaltrabalhadanoturna\", '--:--'))) AS \"totaltrabalhadanoturnamin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"horariodetalhe\".\"cargahorariamista\", '--:--'))) AS \"cargahorariamistamin\" " +
                        ", \"horariodetalhe\".\"totaltrabalhadadiurna\" " +
                        ", \"horariodetalhe\".\"totaltrabalhadanoturna\" " +
                        ", \"horariodetalhe\".\"cargahorariamista\" " +
                        ", \"horariodetalhe\".\"marcacargahorariamista\" AS \"marcacargahorariamistahd\" " +
                        ", \"horariodetalhe\".\"bcarregar\" " +
                        ", \"horariodetalhe\".\"flagfolga\" " +
                        ", \"bancohoras\".\"id\" AS \"idbancohoras\" " +
                        ", \"feriado\".\"id\" AS \"idferiado\" " +
                        ", \"afastamentofunc\".\"id\" AS \"idafastamentofunc\" " +
                        ", \"afastamentofunc\".\"abonado\" AS \"abonadofunc\" " +
                        ", \"afastamentofunc\".\"semcalculo\" AS \"semcalculofunc\" " +
                        ", \"afastamentofunc\".\"horai\" AS \"horaifunc\" " +
                        ", \"afastamentofunc\".\"horaf\" AS \"horaffunc\" " +
                        ", \"afastamentofunc\".\"idocorrencia\" AS \"idocorrenciafunc\" " +
                        ", \"afastamentodep\".\"id\" AS \"idafastamentodep\" " +
                        ", \"afastamentodep\".\"abonado\" AS \"abonadodep\" " +
                        ", \"afastamentodep\".\"semcalculo\" AS \"semcalculodep\" " +
                        ", \"afastamentodep\".\"horai\" AS \"horaidep\" " +
                        ", \"afastamentodep\".\"horaf\" AS \"horafdep\" " +
                        ", \"afastamentodep\".\"idocorrencia\" AS \"idocorrenciadep\" " +
                        ", \"afastamentoemp\".\"id\" AS \"idafastamentoemp\" " +
                        ", \"afastamentoemp\".\"abonado\" AS \"abonadoemp\" " +
                        ", \"afastamentoemp\".\"semcalculo\" AS \"semcalculoemp\" " +
                        ", \"afastamentoemp\".\"horai\" AS \"horaiemp\" " +
                        ", \"afastamentoemp\".\"horaf\" AS \"horafemp\" " +
                        ", \"afastamentoemp\".\"idocorrencia\" AS \"idocorrenciaemp\" " +
                        ", \"jornadaalternativa_view\".\"id\" AS \"idjornadaalternativa\" " +
                        ", \"mudancahorario\".\"id\" AS \"idmudancahorario\" " +
                        ", \"marcacao\".\"tipohoraextrafalta\" " +
                        ", \"parametros\".\"thoraextra\" " +
                        ", \"parametros\".\"thorafalta\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"parametros\".\"thoraextra\", '--:--'))) AS \"thoraextramin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"parametros\".\"thorafalta\", '--:--'))) AS \"thorafaltamin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"parametros\".\"inicioadnoturno\", '--:--'))) AS \"inicioadnoturnomin\" " +
                        ", (SELECT RET FROM CONVERTHORAMINUTO(COALESCE(\"parametros\".\"fimadnoturno\", '--:--'))) AS \"fimadnoturnomin\" " +
                        ", COALESCE(\"marcacao\".\"exphorasextranoturna\", '--:--') AS \"exphorasextranoturna\" " +
                "FROM \"marcacao\" " +
                "INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                "INNER JOIN \"horario\" ON \"horario\".\"id\" = \"marcacao\".\"idhorario\" " +
                "INNER JOIN \"parametros\" ON \"parametros\".\"id\" = \"horario\".\"idparametro\" " +
                "LEFT JOIN \"horariodetalhe\" " +
                "ON \"horariodetalhe\".\"idhorario\" = \"marcacao\".\"idhorario\" AND " +
                "((\"horario\".\"tipohorario\" = 2 AND \"horariodetalhe\".\"data\" = \"marcacao\".\"data\") OR " +
                "(\"horariodetalhe\".\"idhorario\" = \"marcacao\".\"idhorario\" " +
                "AND \"horario\".\"tipohorario\" = 1  " +
                "AND \"horariodetalhe\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") END))) " +
                "LEFT JOIN \"afastamento\" \"afastamentofunc\" ON \"afastamentofunc\".\"tipo\" = 0  " +
                "AND \"afastamentofunc\".\"idfuncionario\" = \"marcacao\".\"idfuncionario\"  " +
                "AND \"marcacao\".\"data\" >= \"afastamentofunc\".\"datai\" " +
                "AND \"marcacao\".\"data\" <= \"afastamentofunc\".\"dataf\" " +
                "LEFT JOIN \"afastamento\" \"afastamentodep\" ON \"afastamentodep\".\"tipo\" = 1 " +
                "AND \"afastamentodep\".\"iddepartamento\" = \"funcionario\".\"iddepartamento\" " +
                "AND \"marcacao\".\"data\" >= \"afastamentodep\".\"datai\" " +
                "AND \"marcacao\".\"data\" <= \"afastamentodep\".\"dataf\" " +
                "LEFT JOIN \"afastamento\" \"afastamentoemp\" ON \"afastamentoemp\".\"tipo\" = 2 " +
                "AND \"afastamentoemp\".\"idempresa\" = \"funcionario\".\"idempresa\" " +
                "AND \"marcacao\".\"data\" >= \"afastamentoemp\".\"datai\" " +
                "AND \"marcacao\".\"data\" <= \"afastamentoemp\".\"dataf\" " +
                "LEFT JOIN \"feriado\" ON \"feriado\".\"data\" = \"marcacao\".\"data\" " +
                "AND (\"feriado\".\"tipoferiado\" = 0 " +
                "OR (\"feriado\".\"tipoferiado\" = 1 AND \"feriado\".\"idempresa\" = \"funcionario\".\"idempresa\") " +
                "OR (\"feriado\".\"tipoferiado\" = 2 AND \"feriado\".\"iddepartamento\" = \"funcionario\".\"iddepartamento\")) " +
                "LEFT JOIN \"bancohoras\" ON \"marcacao\".\"data\" >= \"bancohoras\".\"datainicial\" " +
                "AND \"marcacao\".\"data\" <= \"bancohoras\".\"datafinal\" " +
                "AND ((\"bancohoras\".\"tipo\" = 0 AND \"bancohoras\".\"identificacao\" = \"funcionario\".\"idempresa\") " +
                "OR (\"bancohoras\".\"tipo\" = 1 AND \"bancohoras\".\"identificacao\" = \"funcionario\".\"iddepartamento\") " +
                "OR (\"bancohoras\".\"tipo\" = 2 AND \"bancohoras\".\"identificacao\" = \"marcacao\".\"idfuncionario\") " +
                "OR (\"bancohoras\".\"tipo\" = 3 AND \"bancohoras\".\"identificacao\" = \"funcionario\".\"idfuncao\")) " +
                "LEFT JOIN \"jornadaalternativa_view\" ON " +
                "((\"jornadaalternativa_view\".\"tipo\" = 0 AND \"jornadaalternativa_view\".\"identificacao\" = \"funcionario\".\"idempresa\") " +
                "OR (\"jornadaalternativa_view\".\"tipo\" = 1 AND \"jornadaalternativa_view\".\"identificacao\" = \"funcionario\".\"iddepartamento\") " +
                "OR (\"jornadaalternativa_view\".\"tipo\" = 2 AND \"jornadaalternativa_view\".\"identificacao\" = \"funcionario\".\"id\") " +
                "OR (\"jornadaalternativa_view\".\"tipo\" = 3 AND \"jornadaalternativa_view\".\"identificacao\" = \"funcionario\".\"idfuncao\")) " +
                "AND (\"jornadaalternativa_view\".\"datacompensada\" = \"marcacao\".\"data\" " +
                "OR (\"jornadaalternativa_view\".\"datacompensada\" IS NULL " +
                "AND \"marcacao\".\"data\" >= \"jornadaalternativa_view\".\"datainicial\" " +
                "AND \"marcacao\".\"data\" <= \"jornadaalternativa_view\".\"datafinal\")) " +
                "LEFT JOIN \"mudancahorario\" ON \"mudancahorario\".\"idfuncionario\" = \"marcacao\".\"idfuncionario\" " +
                "AND \"mudancahorario\".\"data\" = \"marcacao\".\"data\" ";
            aux += "WHERE ";
            if (pegaInativos)
            {
                if (!pegaExcluidos)
                    aux += "COALESCE(\"funcionario\".\"excluido\",0) = 0 AND ";
            }
            else
            {
                if (!pegaExcluidos)
                {
                    aux += "COALESCE(\"funcionario\".\"excluido\",0) = 0 AND ";
                }
                 aux += "\"funcionarioativo\" = 1 AND ";
            }

            if (pTipo.HasValue && identificacao != 0)
            {
                switch (pTipo)
                {
                    case 0:
                        aux += "\"funcionario\".\"idempresa\" = ";
                        break;
                    case 1:
                        aux += "\"funcionario\".\"iddepartamento\" = ";
                        break;
                    case 2:
                        aux += "\"marcacao\".\"idfuncionario\" = ";
                        break;
                    case 3:
                        aux += "\"funcionario\".\"idfuncao\" = ";
                        break;
                    case 4:
                        aux += "\"horario\".\"id\" = ";
                        break;
                }

                aux += identificacao.ToString() + " AND ";
            }

            aux += "\"marcacao\".\"data\" >= @datai AND \"marcacao\".\"data\" <= @dataf ";
            aux += "ORDER BY \"funcionario\".\"id\", \"marcacao\".\"data\"";

            DataTable dt = new DataTable();

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        public void ExecutarComandos(List<string> comandos)
        {
            DataBase.ExecutarComandos(comandos);
        }

        private string MontaStringUpdateTratamentoMarc(Modelo.BilhetesImp bil)
        {
            StringBuilder str = new StringBuilder("UPDATE \"bilhetesimp\" SET ");
            str.AppendLine("\"codigo\" = " + bil.Codigo);
            str.AppendLine(", \"ordem\" = '" + bil.Ordem + "'");
            str.AppendLine(", \"data\" = '" + bil.Data.Month + "/" + bil.Data.Day + "/" + bil.Data.Year + "'");
            str.AppendLine(", \"hora\" = '" + bil.Hora + "'");
            str.AppendLine(", \"func\" = '" + bil.Func + "'");
            str.AppendLine(", \"relogio\" = '" + bil.Relogio + "'");
            str.AppendLine(", \"importado\" = " + bil.Importado);
            str.AppendLine(", \"mar_data\" = '" + bil.Mar_data.Value.Month + "/" + bil.Mar_data.Value.Day + "/" + bil.Mar_data.Value.Year + "'");
            str.AppendLine(", \"mar_hora\" = '" + bil.Mar_hora + "'");
            str.AppendLine(", \"mar_relogio\" = '" + bil.Mar_relogio + "'");
            str.AppendLine(", \"posicao\" = " + bil.Posicao);
            str.AppendLine(", \"ent_sai\" = '" + bil.Ent_sai + "'");
            str.AppendLine(", \"chave\" = '" + bil.ToMD5() + "'");
            str.AppendLine(", \"dscodigo\" = '" + bil.DsCodigo + "'");
            str.AppendLine(", \"motivo\" = '" + DataBase.RemoveQuote(bil.Motivo) + "'");
            if (bil.Ocorrencia != '\0')
                str.AppendLine(", \"ocorrencia\" = '" + bil.Ocorrencia.ToString() + "'");
            if (bil.Idjustificativa > 0)
                str.AppendLine(", \"idjustificativa\" = " + bil.Idjustificativa);
            else
                str.AppendLine(", \"idjustificativa\" = NULL");
            DateTime dt = DateTime.Now;
            str.AppendLine(", \"altdata\" = '" + dt.Month + "/" + dt.Day + "/" + dt.Year + "'");
            str.AppendLine(", \"althora\" = '" + dt.Month + "/" + dt.Day + "/" + dt.Year + " " + dt.ToLongTimeString() + "'");
            str.AppendLine(", \"altusuario\" = '" + cwkControleUsuario.Facade.getUsuarioLogado.Login + "'");
            str.AppendLine("WHERE \"id\" = " + bil.Id);

            return str.ToString();
        }

        private string MontaStringInsertTratamentoMarc(Modelo.BilhetesImp bil)
        {
            StringBuilder str = new StringBuilder();
            
            bil.Chave = bil.ToMD5();
            str.Remove(0, str.Length);
            str.Append("INSERT INTO \"bilhetesimp\" ");
            str.Append("(\"codigo\",\"ordem\", \"data\", \"hora\", \"func\", \"relogio\", \"importado\", \"mar_data\", \"mar_hora\", \"mar_relogio\", \"posicao\", \"ent_sai\", \"incdata\", \"inchora\", \"incusuario\", \"chave\", \"dscodigo\", \"motivo\", \"ocorrencia\", \"idjustificativa\") ");
            str.Append("VALUES (");
            str.Append(bil.Codigo);
            str.Append(", '");
            str.Append(bil.Ordem);
            str.Append("', '");
            str.Append(bil.Data.Month + "/" + bil.Data.Day + "/" + bil.Data.Year);
            str.Append("', '");
            str.Append(bil.Hora);
            str.Append("', '");
            str.Append(bil.Func);
            str.Append("', '");
            str.Append(bil.Relogio);
            str.Append("', ");
            str.Append(bil.Importado);
            str.Append(", '");
            str.Append(bil.Mar_data.Value.Month + "/" + bil.Mar_data.Value.Day + "/" + bil.Mar_data.Value.Year);
            str.Append("', '");
            str.Append(bil.Mar_hora);
            str.Append("', '");
            str.Append(bil.Mar_relogio);
            str.Append("', ");
            str.Append(bil.Posicao);
            str.Append(", '");
            str.Append(bil.Ent_sai);
            str.Append("', '");
            str.Append(DateTime.Now.Date.Month + "/" + DateTime.Now.Date.Day + "/" + DateTime.Now.Date.Year);
            str.Append("', '");
            str.Append(DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year + " " + DateTime.Now.ToLongTimeString());
            str.Append("', '");
            str.Append(Modelo.cwkGlobal.objUsuarioLogado.Login);
            str.Append("', '");
            str.Append(bil.Chave);
            str.Append("', '");
            str.Append(bil.DsCodigo);
            str.Append("', '");
            str.Append(bil.Motivo);
            str.Append("', '");
            str.Append(bil.Ocorrencia);
            str.Append("', ");
            if (bil.Idjustificativa > 0)
                str.Append(bil.Idjustificativa);
            else
                str.Append("NULL");
            str.Append(")");

            return str.ToString();
        }

        private string MontaStringDeleteTratamentoMarc(Modelo.BilhetesImp bil)
        {
            StringBuilder ret = new StringBuilder("DELETE FROM \"bilhetesimp\" WHERE \"bilhetesimp\".\"id\" = " + bil.Id);

            return ret.ToString();
        }

        public List<string> RetornaComandosTratamentoMarcacao(List<Modelo.BilhetesImp> pTratamentos)
        {
            List<string> ret = new List<string>();
            foreach (Modelo.BilhetesImp trat in pTratamentos)
            {
                switch (trat.Acao)
                {
                    case Modelo.Acao.Alterar:
                        ret.Add(this.MontaStringUpdateTratamentoMarc(trat));
                        break;
                    case Modelo.Acao.Incluir:
                        ret.Add(this.MontaStringInsertTratamentoMarc(trat));
                        break;
                    case Modelo.Acao.Excluir:
                        ret.Add(this.MontaStringDeleteTratamentoMarc(trat));
                        break;
                }
            }
            return ret;
        }
        #endregion

        #region ICalculaMarcacao Members


        public void PersistirDados(IEnumerable<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes)
        {
            throw new NotImplementedException();
        }

        #endregion

        public Modelo.Proxy.pxyAbonoDsrFuncionario GetAbonoDsrFuncionario(int idFuncionario, DateTime dataInicio, DateTime dataFim)
        {
            return new Modelo.Proxy.pxyAbonoDsrFuncionario() 
            { 
                IdFuncionario = 0, 
                Nome = "", 
                QtdMinutosDsr = 0, 
                QtdDiasDsr = 0, 
                QtdMinutosAbono = 0, 
                QtdDiasAbono = 0 
            };
        }


        public DataTable GetMarcacoesCalculoWebApi(int? pTipo, int identificacao, DateTime pDataI, DateTime pDataF, bool pegaInativos, bool pegaExcluidos)
        {
            throw new NotImplementedException();
        }


        public void PersistirDadosWebApi(IEnumerable<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes, string login)
        {
            throw new NotImplementedException();
        }

        public DataTable GetFuncionariosDSRWebApi(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF)
        {
            throw new NotImplementedException();
        }


        public DataTable GetMarcacoesCalculo(List<int> idsFuncionarios, DateTime pDataI, DateTime pDataF, bool pegaInativos, bool pegaExcluidos)
        {
            throw new NotImplementedException();
        }


        public DataTable GetFuncionariosDSR(List<int> idsFuncionarios, DateTime pDataI, DateTime pDataF)
        {
            throw new NotImplementedException();
        }
    }
}
