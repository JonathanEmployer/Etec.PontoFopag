using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace DAL.FB
{
    public class RelatorioEspelho : IRelatorioEspelho
    {
        public DataTable GetMarcacoesEspelho(DateTime dataInicial, DateTime dataFinal, string ids, int tipo, IDataBase db)
        {
            FbParameter[] parms = new FbParameter[2]
                {
                    new FbParameter("@datainicial", FbDbType.Date),
                    new FbParameter("@datafinal", FbDbType.Date)
                };
            parms[0].Value = dataInicial;
            parms[1].Value = dataFinal;

            DataTable dt = new DataTable();

            StringBuilder aux = new StringBuilder();
            aux.AppendLine("SELECT    \"marcacao\".\"id\"");
            aux.AppendLine(", \"marcacao\".\"idhorario\"");
            aux.AppendLine(", \"marcacao\".\"data\"");
            aux.AppendLine(", \"marcacao\".\"entrada_1\"");
            aux.AppendLine(", \"marcacao\".\"entrada_2\"");
            aux.AppendLine(", \"marcacao\".\"entrada_3\"");
            aux.AppendLine(", \"marcacao\".\"entrada_4\"");
            aux.AppendLine(", \"marcacao\".\"entrada_5\"");
            aux.AppendLine(", \"marcacao\".\"entrada_6\"");
            aux.AppendLine(", \"marcacao\".\"entrada_7\"");
            aux.AppendLine(", \"marcacao\".\"entrada_8\"");
            aux.AppendLine(", \"marcacao\".\"saida_1\"");
            aux.AppendLine(", \"marcacao\".\"saida_2\"");
            aux.AppendLine(", \"marcacao\".\"saida_3\"");
            aux.AppendLine(", \"marcacao\".\"saida_4\"");
            aux.AppendLine(", \"marcacao\".\"saida_5\"");
            aux.AppendLine(", \"marcacao\".\"saida_6\"");
            aux.AppendLine(", \"marcacao\".\"saida_7\"");
            aux.AppendLine(", \"marcacao\".\"saida_8\"");
            aux.AppendLine(", \"marcacao\".\"dscodigo\"");
            aux.AppendLine(", \"funcionario\".\"nome\" AS \"funcionario\"");
            aux.AppendLine(", \"funcionario\".\"matricula\"");
            aux.AppendLine(", \"funcionario\".\"dataadmissao\"");
            aux.AppendLine(", \"funcionario\".\"pis\"");
            aux.AppendLine(", \"funcionario\".\"idempresa\"");
            aux.AppendLine(", \"funcionario\".\"iddepartamento\"");
            aux.AppendLine(", \"funcionario\".\"idfuncao\"");
            aux.AppendLine(", \"marcacao\".\"idfuncionario\"");
            aux.AppendLine(", \"empresa\".\"nome\" AS \"empresa\"");
            aux.AppendLine(", \"empresa\".\"codigo\" AS \"codigoempresa\"");
            aux.AppendLine(", case when COALESCE(\"empresa\".\"cnpj\", '') <> '' then \"empresa\".\"cnpj\" else \"empresa\".\"cpf\" end AS \"cnpj_cpf\"");
            aux.AppendLine(", \"empresa\".\"endereco\"");
            aux.AppendLine(", \"empresa\".\"cidade\"");
            aux.AppendLine(", \"empresa\".\"estado\"");
            aux.AppendLine(", \"horario\".\"tipohorario\"");
            aux.AppendLine(", \"horario\".\"codigo\" AS \"codigohorario\"");
            aux.AppendLine(", COALESCE(\"marcacao\".\"folga\", 0) AS \"folga\"");
            aux.AppendLine(", \"horariodetalhenormal\".\"entrada_1\" AS \"entrada_1normal\"");
            aux.AppendLine(", \"horariodetalhenormal\".\"entrada_2\" AS \"entrada_2normal\"");
            aux.AppendLine(", \"horariodetalhenormal\".\"entrada_3\" AS \"entrada_3normal\"");
            aux.AppendLine(", \"horariodetalhenormal\".\"entrada_4\" AS \"entrada_4normal\"");
            aux.AppendLine(", \"horariodetalhenormal\".\"saida_1\" AS \"saida_1normal\"");
            aux.AppendLine(", \"horariodetalhenormal\".\"saida_2\" AS \"saida_2normal\"");
            aux.AppendLine(", \"horariodetalhenormal\".\"saida_3\" AS \"saida_3normal\"");
            aux.AppendLine(", \"horariodetalhenormal\".\"saida_4\" AS \"saida_4normal\"");
            aux.AppendLine(", \"horariodetalhenormal\".\"idjornada\" AS \"idjornada_normal\"");
            aux.AppendLine(", \"horariodetalheflexivel\".\"entrada_1\" AS \"entrada_1flexivel\"");
            aux.AppendLine(", \"horariodetalheflexivel\".\"entrada_2\" AS \"entrada_2flexivel\"");
            aux.AppendLine(", \"horariodetalheflexivel\".\"entrada_3\" AS \"entrada_3flexivel\"");
            aux.AppendLine(", \"horariodetalheflexivel\".\"entrada_4\" AS \"entrada_4flexivel\"");
            aux.AppendLine(", \"horariodetalheflexivel\".\"saida_1\" AS \"saida_1flexivel\"");
            aux.AppendLine(", \"horariodetalheflexivel\".\"saida_2\" AS \"saida_2flexivel\"");
            aux.AppendLine(", \"horariodetalheflexivel\".\"saida_3\" AS \"saida_3flexivel\"");
            aux.AppendLine(", \"horariodetalheflexivel\".\"saida_4\" AS \"saida_4flexivel\"");
            aux.AppendLine(", \"horariodetalheflexivel\".\"idjornada\" AS \"idjornada_flexivel\"");
            aux.AppendLine("FROM \"marcacao\"");
            aux.AppendLine("INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" AND \"funcionario\".\"funcionarioativo\" = 1");
            aux.AppendLine("INNER JOIN \"horario\" ON \"horario\".\"id\" = \"marcacao\".\"idhorario\"");
            aux.AppendLine("INNER JOIN \"parametros\" ON \"parametros\".\"id\" = \"horario\".\"idparametro\"");
            aux.AppendLine("INNER JOIN \"empresa\" ON \"empresa\".\"id\" = \"funcionario\".\"idempresa\"");
            aux.AppendLine("LEFT JOIN \"horariodetalhe\" \"horariodetalhenormal\" ON \"horariodetalhenormal\".\"idhorario\" = \"marcacao\".\"idhorario\"");
            aux.AppendLine("AND \"horario\".\"tipohorario\" = 1 AND \"horariodetalhenormal\".\"dia\" = (CASE WHEN EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") = 0 THEN 7 ELSE EXTRACT(WEEKDAY FROM \"marcacao\".\"data\") END)");
            aux.AppendLine("LEFT JOIN \"horariodetalhe\" \"horariodetalheflexivel\" ON \"horariodetalheflexivel\".\"idhorario\" = \"marcacao\".\"idhorario\"");
            aux.AppendLine("AND \"horario\".\"tipohorario\" = 2 AND \"horariodetalheflexivel\".\"data\" = \"marcacao\".\"data\"");
            aux.AppendLine("WHERE \"marcacao\".\"id\" > 0");
            aux.AppendLine("AND \"marcacao\".\"data\" >= @datainicial AND \"marcacao\".\"data\" <= @datafinal");
            aux.AppendLine("AND COALESCE(\"funcionario\".\"excluido\", 0) = 0");

            switch (tipo)
            {
                case 0:
                    aux.AppendLine("AND \"funcionario\".\"idempresa\" IN " + ids);
                    break;
                case 1:
                    aux.AppendLine("AND \"funcionario\".\"iddepartamento\" IN " + ids);
                    break;
                case 2:
                    aux.AppendLine("AND \"marcacao\".\"idfuncionario\" IN " + ids);
                    break;
            }

            aux.AppendLine("ORDER BY \"empresa\".\"nome\", \"funcionario\".\"nome\", \"marcacao\".\"data\"");

            dt.Load(FB.DataBase.ExecuteReader(CommandType.Text, aux.ToString(), parms));

            return dt;
        }

        public DataTable GetJornadasEspelho(List<string> jornadas, int tipo, IDataBase db)
        {
            DataTable dt;
            if (jornadas.Count > 0)
            {
                StringBuilder idsJornadas = new StringBuilder("(");
                int count = 0;
                foreach (string id in jornadas)
                {
                    if (count > 0)
                        idsJornadas.Append(", ");
                    idsJornadas.Append("'" + id + "'");
                    count++;
                }
                idsJornadas.Append(")");

                StringBuilder cmd = new StringBuilder();
                cmd.AppendLine("SELECT \"jornada\".\"id\" AS \"codigo\", \"jornada\".\"entrada_1\", \"jornada\".\"saida_1\", \"jornada\".\"entrada_2\", \"jornada\".\"saida_2\"");
                cmd.AppendLine(", \"funcionario\".\"id\" AS \"idfuncionario\"");
                cmd.AppendLine("FROM \"jornada\"");
                cmd.AppendLine("LEFT JOIN \"funcionario\"");
                cmd.AppendLine("ON CAST(\"funcionario\".\"id\" AS VARCHAR(12)) || '-' || CAST(\"jornada\".\"id\" AS VARCHAR(12)) IN " + idsJornadas.ToString());

                FbParameter[] parms = new FbParameter[] { };
                dt = new DataTable();
                dt.Load(DataBase.ExecuteReader(CommandType.Text, cmd.ToString(), parms));
            }
            else
            {
                dt = new DataTable();
                DataColumn[] colunas = new DataColumn[]
                {
                    new DataColumn("codigo"),
                    new DataColumn("entrada_1"),
                    new DataColumn("saida_1"),
                    new DataColumn("entrada_2"),
                    new DataColumn("saida_2"),
                    new DataColumn("idfuncionario")
                };
                dt.Columns.AddRange(colunas);
            }
            return dt;
        }
    }
}
