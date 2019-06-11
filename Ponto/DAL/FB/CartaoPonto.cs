using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace DAL.FB
{
    public class CartaoPonto : ICartaoPonto
    {
        #region Marcação
        public Modelo.Cw_Usuario UsuarioLogado { get; set; }
        public DataTable GetCartaoPontoRel(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, string funcionarios, int tipo, int normalFlexivel, bool ordenaDeptoFuncionario, string filtro)
        {
            string SELECTCP = "   SELECT    \"marcacao\".\"id\", \"marcacao\".\"idhorario\" "
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
                        + " , \"marcacao\".\"horastrabalhadas\" "
                        + " , \"marcacao\".\"horasextrasdiurna\" "
                        + " , \"marcacao\".\"horasfaltas\" "
                        + " , \"marcacao\".\"entradaextra\" "
                        + " , \"marcacao\".\"saidaextra\" "
                        + " , \"marcacao\".\"horastrabalhadasnoturnas\" "
                        + " , \"marcacao\".\"horasextranoturna\" "
                        + " , \"marcacao\".\"horasfaltanoturna\" "
                        + " , \"marcacao\".\"ocorrencia\" "
                        + " , \"funcionario\".\"dscodigo\" "
                        + " , \"funcionario\".\"nome\" AS \"funcionario\" "
                        + " , \"funcionario\".\"matricula\" "
                        + " , \"funcionario\".\"dataadmissao\" "
                        + " , \"funcionario\".\"codigofolha\" "
                        + " , \"funcao\".\"descricao\" AS \"funcao\" "
                        + " , \"departamento\".\"descricao\" AS \"departamento\" "
                        + " , \"empresa\".\"nome\" AS \"empresa\" "
                        + " , case when COALESCE(\"empresa\".\"cnpj\", '') <> '' then \"empresa\".\"cnpj\" else \"empresa\".\"cpf\" end AS \"cnpj_cpf\" "
                        + " , \"empresa\".\"endereco\" "
                        + " , \"empresa\".\"cidade\"  "
                        + " , \"empresa\".\"estado\" "
                        + " , \"parametros\".\"thoraextra\" "
                        + " , \"parametros\".\"thorafalta\"  "
                        + " , COALESCE(\"marcacao\".\"valordsr\", '--:--') AS \"valordsr\" "
                        + " , \"funcionario\".\"idempresa\" "
                        + " , \"funcionario\".\"iddepartamento\" "
                        + " , \"funcionario\".\"idfuncao\" "
                        + " , \"marcacao\".\"idfuncionario\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'E' and \"a\".\"posicao\" = 1),'') AS \"tratent_1\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'E' and \"a\".\"posicao\" = 2),'') AS \"tratent_2\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'E' and \"a\".\"posicao\" = 3),'') AS \"tratent_3\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'E' and \"a\".\"posicao\" = 4),'') AS \"tratent_4\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'E' and \"a\".\"posicao\" = 5),'') AS \"tratent_5\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'E' and \"a\".\"posicao\" = 6),'') AS \"tratent_6\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'E' and \"a\".\"posicao\" = 7),'') AS \"tratent_7\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'E' and \"a\".\"posicao\" = 8),'') AS \"tratent_8\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'S' and \"a\".\"posicao\" = 1),'') AS \"tratsai_1\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'S' and \"a\".\"posicao\" = 2),'') AS \"tratsai_2\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'S' and \"a\".\"posicao\" = 3),'') AS \"tratsai_3\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'S' and \"a\".\"posicao\" = 4),'') AS \"tratsai_4\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'S' and \"a\".\"posicao\" = 5),'') AS \"tratsai_5\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'S' and \"a\".\"posicao\" = 6),'') AS \"tratsai_6\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'S' and \"a\".\"posicao\" = 7),'') AS \"tratsai_7\" "
                        + " , COALESCE((SELECT FIRST 1 COALESCE(\"a\".\"ocorrencia\",'') FROM \"bilhetesimp\" \"a\" where \"a\".\"dscodigo\" = \"marcacao\".\"dscodigo\" and \"a\".\"mar_data\" = \"marcacao\".\"data\" and \"a\".\"ent_sai\" = 'S' and \"a\".\"posicao\" = 8),'') AS \"tratsai_8\" "
                        + " , COALESCE(\"horario\".\"tipohorario\", 0) AS \"tipohorario\" "
                        + ", \"horario\".\"considerasabadosemana\" "
                        + ", \"horario\".\"consideradomingosemana\" "
                        + ", \"horario\".\"tipoacumulo\" "
                        + ", \"horariophextra50\".\"percentualextra\" AS \"percentualextra50\" " 
                        + ", \"horariophextra50\".\"quantidadeextra\" AS \"quantidadeextra50\" " 
                        + ", \"horariophextra60\".\"percentualextra\" AS \"percentualextra60\" " 
                        + ", \"horariophextra60\".\"quantidadeextra\" AS \"quantidadeextra60\" " 
                        + ", \"horariophextra70\".\"percentualextra\" AS \"percentualextra70\" " 
                        + ", \"horariophextra70\".\"quantidadeextra\" AS \"quantidadeextra70\" " 
                        + ", \"horariophextra80\".\"percentualextra\" AS \"percentualextra80\" " 
                        + ", \"horariophextra80\".\"quantidadeextra\" AS \"quantidadeextra80\" "
                        + ", \"horariophextra90\".\"percentualextra\" AS \"percentualextra90\" " 
                        + ", \"horariophextra90\".\"quantidadeextra\" AS \"quantidadeextra90\" " 
                        + ", \"horariophextra100\".\"percentualextra\" AS \"percentualextra100\" " 
                        + ", \"horariophextra100\".\"quantidadeextra\" AS \"quantidadeextra100\" " 
                        + ", \"horariophextrasab\".\"percentualextra\" AS \"percentualextrasab\" " 
                        + ", \"horariophextrasab\".\"quantidadeextra\" AS \"quantidadeextrasab\" " 
                        + ", \"horariophextradom\".\"percentualextra\" AS \"percentualextradom\" " 
                        + ", \"horariophextradom\".\"quantidadeextra\" AS \"quantidadeextradom\" " 
                        + ", \"horariophextrafer\".\"percentualextra\" AS \"percentualextrafer\" " 
                        + ", \"horariophextrafer\".\"quantidadeextra\" AS \"quantidadeextrafer\" " 
                        + ", \"horariophextrafol\".\"percentualextra\" AS \"percentualextrafol\" " 
                        + ", \"horariophextrafol\".\"quantidadeextra\" AS \"quantidadeextrafol\" " 
                        + ", \"horariodetalhenormal\".\"totaltrabalhadadiurna\" AS \"chdiurnanormal\" " 
                        + ", \"horariodetalhenormal\".\"totaltrabalhadanoturna\" AS \"chnoturnanormal\" "
                        + ", \"horariodetalhenormal\".\"flagfolga\" AS \"flagfolganormal\" "
                        + ", \"horariodetalhenormal\".\"entrada_1\" AS \"entrada_1normal\" "
                        + ", \"horariodetalhenormal\".\"entrada_2\" AS \"entrada_2normal\" "
                        + ", \"horariodetalhenormal\".\"entrada_3\" AS \"entrada_3normal\" "
                        + ", \"horariodetalhenormal\".\"entrada_4\" AS \"entrada_4normal\" "
                        + ", \"horariodetalhenormal\".\"saida_1\" AS \"saida_1normal\" "
                        + ", \"horariodetalhenormal\".\"saida_2\" AS \"saida_2normal\" "
                        + ", \"horariodetalhenormal\".\"saida_3\" AS \"saida_3normal\" "
                        + ", \"horariodetalhenormal\".\"saida_4\" AS \"saida_4normal\" "
                        + ", \"horariodetalhenormal\".\"cargahorariamista\" AS \"cargamistanormal\" "
                        + ", \"horariodetalheflexivel\".\"entrada_1\" AS \"entrada_1flexivel\" "
                        + ", \"horariodetalheflexivel\".\"entrada_2\" AS \"entrada_2flexivel\" "
                        + ", \"horariodetalheflexivel\".\"entrada_3\" AS \"entrada_3flexivel\" "
                        + ", \"horariodetalheflexivel\".\"entrada_4\" AS \"entrada_4flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_1\" AS \"saida_1flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_2\" AS \"saida_2flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_3\" AS \"saida_3flexivel\" "
                        + ", \"horariodetalheflexivel\".\"saida_4\" AS \"saida_4flexivel\" "
                        + ", \"horariodetalheflexivel\".\"cargahorariamista\" AS \"cargamistaflexivel\" "
                        + ", \"horariodetalheflexivel\".\"totaltrabalhadadiurna\" AS \"chdiurnaflexivel\" "
                        + ", \"horariodetalheflexivel\".\"totaltrabalhadanoturna\" AS \"chnoturnaflexivel\" "
                        + ", \"horariodetalheflexivel\".\"flagfolga\" AS \"flagfolgaflexivel\" "
                        + " , \"marcacao\".\"bancohorascre\" "
                        + " , \"marcacao\".\"bancohorasdeb\" "
                        + " , COALESCE(\"marcacao\".\"folga\", 0) AS \"folga\" "
                        + " , COALESCE(\"marcacao\".\"chave\", '') AS \"chave\" "
                        + " , \"horario\".\"obs\" AS \"observacao\" "
                        + " , \"funcionario\".\"campoobservacao\" AS \"observacaofunc\" "
                        + " , \"parametros\".\"imprimeobservacao\" "
                        + " , \"parametros\".\"campoobservacao\" "
                        + " , COALESCE(\"marcacao\".\"exphorasextranoturna\", '--:--') AS \"exphorasextranoturna\" "
                        + " FROM \"marcacao\" "
                        + " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" "
                        + " INNER JOIN \"horario\" ON \"horario\".\"id\" = \"marcacao\".\"idhorario\" "
                        + " INNER JOIN \"parametros\" ON \"parametros\".\"id\" = \"horario\".\"idparametro\" "
                        + " INNER JOIN \"funcao\" ON \"funcao\".\"id\" = \"funcionario\".\"idfuncao\" "
                        + " INNER JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" "
                        + " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" "

                        + " INNER JOIN \"horariophextra\" \"horariophextra50\" ON \"horariophextra50\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra50\".\"codigo\" = 0 " 
                        + " INNER JOIN \"horariophextra\" \"horariophextra60\" ON \"horariophextra60\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra60\".\"codigo\" = 1 " 
                        + " INNER JOIN \"horariophextra\" \"horariophextra70\" ON \"horariophextra70\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra70\".\"codigo\" = 2 " 
                        + " INNER JOIN \"horariophextra\" \"horariophextra80\" ON \"horariophextra80\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra80\".\"codigo\" = 3 " 
                        + " INNER JOIN \"horariophextra\" \"horariophextra90\" ON \"horariophextra90\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra90\".\"codigo\" = 4 " 
                        + " INNER JOIN \"horariophextra\" \"horariophextra100\" ON \"horariophextra100\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextra100\".\"codigo\" = 5 " 
                        + " INNER JOIN \"horariophextra\" \"horariophextrasab\" ON \"horariophextrasab\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextrasab\".\"codigo\" = 6 " 
                        + " INNER JOIN \"horariophextra\" \"horariophextradom\" ON \"horariophextradom\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextradom\".\"codigo\" = 7 " 
                        + " INNER JOIN \"horariophextra\" \"horariophextrafer\" ON \"horariophextrafer\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextrafer\".\"codigo\" = 8 "
                        + " INNER JOIN \"horariophextra\" \"horariophextrafol\" ON \"horariophextrafol\".\"idhorario\" = \"marcacao\".\"idhorario\" AND \"horariophextrafol\".\"codigo\" = 9 " 

                        + " LEFT JOIN \"horariodetalhe\" \"horariodetalhenormal\" ON \"horariodetalhenormal\".\"idhorario\" = \"marcacao\".\"idhorario\" " 
                        + " AND \"horario\".\"tipohorario\" = 1 AND \"horariodetalhenormal\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") END) " 
                        + " LEFT JOIN \"horariodetalhe\" \"horariodetalheflexivel\" ON \"horariodetalheflexivel\".\"idhorario\" = \"marcacao\".\"idhorario\" " 
                        + " AND \"horario\".\"tipohorario\" = 2 AND \"horariodetalheflexivel\".\"data\" = \"marcacao\".\"data\" " 
                        + " WHERE \"funcionario\".\"funcionarioativo\" = 1 AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 ";

            FbParameter[] parms = new FbParameter[]
            {
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date),
                    new FbParameter("@normalflexivel", FbDbType.Integer)
            };
            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;
            parms[2].Value = normalFlexivel;

            DataTable dt = new DataTable();

            string aux = SELECTCP;
            if (normalFlexivel > 0)
            {
                aux += " AND \"horario\".\"tipohorario\" = @normalflexivel";
            }
            aux += " AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ";

            switch (tipo)
            {
                case 0:
                    aux += " AND \"funcionario\".\"idempresa\" IN " + empresas;
                    break;
                case 1:
                    aux += " AND \"funcionario\".\"iddepartamento\" IN " + departamentos;
                    break;
                case 2:
                    aux += " AND \"marcacao\".\"idfuncionario\" IN " + funcionarios;
                    break;
            }

            aux += " ORDER BY  \"empresa\".\"nome\", \"funcionario\".\"nome\", \"marcacao\".\"data\" ";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        /// <summary>
        /// retorna o cartao ponto da manutencao diaria
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="empresas"></param>
        /// <param name="departamentos"></param>
        /// <param name="tipo">0 = Empresa; 1 = Departamento</param>
        /// <returns></returns>
        public DataTable GetCartaoPontoDiaria(DateTime dataInicial, DateTime dataFinal, string empresas, string departamentos, int tipo)
        {
            string aux = "   SELECT    \"marcacao\".\"id\", \"marcacao\".\"idhorario\" "
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
                        + " , \"marcacao\".\"horastrabalhadas\" "
                        + " , \"marcacao\".\"horasextrasdiurna\" "
                        + " , \"marcacao\".\"horasfaltas\" "
                        + " , \"marcacao\".\"entradaextra\" "
                        + " , \"marcacao\".\"saidaextra\" "
                        + " , \"marcacao\".\"horastrabalhadasnoturnas\" "
                        + " , \"marcacao\".\"horasextranoturna\" "
                        + " , \"marcacao\".\"horasfaltanoturna\" "
                        + " , \"marcacao\".\"ocorrencia\" "
                        + " , \"funcionario\".\"dscodigo\" "
                        + " , \"funcionario\".\"nome\" AS \"funcionario\" "
                        + " , \"funcionario\".\"matricula\" "
                        + " , \"funcionario\".\"dataadmissao\" "
                        + " , \"funcionario\".\"codigofolha\" "
                        + " , \"funcao\".\"descricao\" AS \"funcao\" "
                        + " , \"departamento\".\"descricao\" AS \"departamento\" "
                        + " , \"empresa\".\"nome\" AS \"empresa\" "
                        + " , case when COALESCE(\"empresa\".\"cnpj\", '') <> '' then \"empresa\".\"cnpj\" else \"empresa\".\"cpf\" end AS \"cnpj_cpf\" "
                        + " , \"empresa\".\"endereco\" "
                        + " , \"empresa\".\"cidade\"  "
                        + " , \"empresa\".\"estado\" "
                        + " , \"parametros\".\"thoraextra\" "
                        + " , \"parametros\".\"thorafalta\"  "
                        + " , COALESCE(\"marcacao\".\"valordsr\", '--:--') AS \"valordsr\" "
                        + " , \"funcionario\".\"idempresa\" "
                        + " , \"funcionario\".\"iddepartamento\" "
                        + " , \"funcionario\".\"idfuncao\" "
                        + " , \"marcacao\".\"idfuncionario\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtEntrada_1'),'') AS \"tratent_1\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtEntrada_2'),'') AS \"tratent_2\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtEntrada_3'),'') AS \"tratent_3\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtEntrada_4'),'') AS \"tratent_4\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtEntrada_5'),'') AS \"tratent_5\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtEntrada_6'),'') AS \"tratent_6\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtEntrada_7'),'') AS \"tratent_7\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtEntrada_8'),'') AS \"tratent_8\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtSaida_1'),'') AS \"tratsai_1\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtSaida_2'),'') AS \"tratsai_2\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtSaida_3'),'') AS \"tratsai_3\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtSaida_4'),'') AS \"tratsai_4\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtSaida_5'),'') AS \"tratsai_5\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtSaida_6'),'') AS \"tratsai_6\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtSaida_7'),'') AS \"tratsai_7\" "
                        + " , COALESCE((SELECT COALESCE(\"a\".\"ocorrencia\",'') FROM \"tratamentomarcacao\" \"a\" where \"a\".\"idmarcacao\" = \"marcacao\".\"id\" and \"a\".\"indicador\" = 'txtSaida_8'),'') AS \"tratsai_8\" "
                        + " , COALESCE(\"horario\".\"tipohorario\", 0) AS \"tipohorario\" "
                        + " , \"marcacao\".\"bancohorascre\" "
                        + " , \"marcacao\".\"bancohorasdeb\" "
                        + " , COALESCE(\"marcacao\".\"folga\", 0) AS \"folga\" "
                        + " , COALESCE(\"marcacao\".\"chave\", '') AS \"chave\" "
                        + " FROM \"marcacao\" "
                        + " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" "
                        + " INNER JOIN \"horario\" ON \"horario\".\"id\" = \"marcacao\".\"idhorario\" "
                        + " INNER JOIN \"parametros\" ON \"parametros\".\"id\" = \"horario\".\"idparametro\" "
                        + " INNER JOIN \"funcao\" ON \"funcao\".\"id\" = \"funcionario\".\"idfuncao\" "
                        + " INNER JOIN \"departamento\" ON \"departamento\".\"id\" = \"funcionario\".\"iddepartamento\" "
                        + " INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\" "
                        + " WHERE \"funcionario\".\"funcionarioativo\" = 1 AND COALESCE(\"funcionario\".\"excluido\", 0) = 0 ";

            FbParameter[] parms = new FbParameter[]
            {
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
            };
            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;

            DataTable dt = new DataTable();
            
            aux += " AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal ";

            switch (tipo)
            {
                case 0:
                    aux += " AND \"funcionario\".\"idempresa\" IN " + empresas;
                    break;
                case 1:
                    aux += " AND \"funcionario\".\"iddepartamento\" IN " + departamentos;
                    break;
                default:
                    break;
            }

            aux += " ORDER BY  \"funcionario\".\"nome\" ";

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux, parms));

            return dt;
        }

        #endregion
    }
}
