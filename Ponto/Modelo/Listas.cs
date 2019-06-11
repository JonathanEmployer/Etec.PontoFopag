using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.Mvc;

namespace Modelo
{
    public static class Listas
    {
        public enum TipoAfastamento
        {
            Funcionario = 3,
            Departamento = 2,
            Empresa = 1
        };
        public static DataTable TipoCampoExportacao()
        {
            DataTable ret = new DataTable();
            ret.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("indice", typeof(string))
                , new DataColumn("nome", typeof(string))
            });

            ret.Rows.Add(new object[] { "Cabeçalho", "Cabeçalho" });
            ret.Rows.Add(new object[] { "Matrícula", "Matrícula" });
            ret.Rows.Add(new object[] { "Código Funcionário", "Código Funcionário" });
            ret.Rows.Add(new object[] { "Código Folha", "Código Folha" });
            ret.Rows.Add(new object[] { "Nome do Funcionário", "Nome do Funcionário" });
            ret.Rows.Add(new object[] { "Campo Fixo", "Campo Fixo" });
            ret.Rows.Add(new object[] { "Código Empresa", "Código Empresa" });
            ret.Rows.Add(new object[] { "Código Evento", "Código Evento" });
            ret.Rows.Add(new object[] { "Complemento Evento", "Complemento Evento" });
            ret.Rows.Add(new object[] { "Valor do Evento", "Valor do Evento" });
            ret.Rows.Add(new object[] { "Pis", "Pis" });
            ret.Rows.Add(new object[] { "CPF Funcionário", "CPF Funcionário" });
            ret.Rows.Add(new object[] { "Mês", "Mês vigência" });
            ret.Rows.Add(new object[] {"Ano", "Ano vigência"});
            
            return ret;
        }

        public static DataTable DelimitadorCampoExportacao()
        {
            DataTable ret = new DataTable();
            ret.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("indice", typeof(string))
                , new DataColumn("nome", typeof(string))
            });

            ret.Rows.Add(new object[] { "|", "|" });
            ret.Rows.Add(new object[] { ",", "," });
            ret.Rows.Add(new object[] { ";", ";" });
            ret.Rows.Add(new object[] { ":", ":" });
            ret.Rows.Add(new object[] { ".", "." });
            ret.Rows.Add(new object[] { "[espaço]", "[espaço]" });
            ret.Rows.Add(new object[] { "[nenhum]", "[nenhum]" });

            return ret;
        }

        public static DataTable QualificadorCampoExportacao()
        {
            DataTable ret = new DataTable();
            ret.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("indice", typeof(string))
                , new DataColumn("nome", typeof(string))
            });

            ret.Rows.Add(new object[] { "\"", "\"" });
            ret.Rows.Add(new object[] { "'", "'" });
            ret.Rows.Add(new object[] { "[nenhum]", "[nenhum]" });

            return ret;
        }

        public static DataTable TiposArquivosExportacao()
        {
            DataTable ret = new DataTable();
            ret.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("indice", typeof(short))
                , new DataColumn("nome", typeof(string))
            });

            ret.Rows.Add(new object[] { 0, "AFDT" });
            ret.Rows.Add(new object[] { 1, "ACJEF" });

            return ret;
        }

        public enum TipoRelogio
        {
            InnerRep = 1,
            Orion6 = 2,
            RepTrilobit = 3
        }

        public enum TipoComunicacao
        {
            [Description("TCP/IP")]
            TCPIP = 0,
            Serial = 1
        }

        public enum UTCBrasilList
        {
            [Description("-5.00")]
            MenosCinco = 0,
            [Description("-4.00")]
            MenosQuatro = 1,
            [Description("-3.00")]
            MenosTres = 2,
            [Description("-2.00")]
            MenosDois = 3
        }

        public enum ManutBilhete
        {
            [Description("Dia Anterior")]
            Dia_Anterior = 0,
            [Description("Mesmo Dia")]
            Mesmo_Dia = 1,
            [Description("Dia Posterior")]
            Dia_Posterior = 2
        }

        public enum TipoHorario
        {
            Normal = 0,
            [Description("Flexível")]
            Flexivel = 1,
            [Description("Dinâmico")]
            Dinamico = 2
        }

        public enum TipoAcumulo
        {
            [Description("Diário")]
            Diario = 1,
            Semanal = 2,
            Mensal = 3
        }

        public static SelectList Estados
        {
            get
            {
                return new SelectList(
                    new Dictionary<string, string>
                {
                    {"AC" , "AC" },
                    {"AL" , "AL" },
                    {"AP" , "AP" },
                    {"AM" , "AM" },
                    {"BA" , "BA" },
                    {"CE" , "CE" },
                    {"DF" , "DF" },
                    {"ES" , "ES" },
                    {"GO" , "GO" },
                    {"MA" , "MA" },
                    {"MT" , "MT" },
                    {"MS" , "MS" },
                    {"MG" , "MG" },
                    {"PA" , "PA" },
                    {"PB" , "PB" },
                    {"PR" , "PR" },
                    {"PE" , "PE" },
                    {"PI" , "PI" },
                    {"RJ" , "RJ" },
                    {"RN" , "RN" },
                    {"RS" , "RS" },
                    {"RO" , "RO" },
                    {"RR" , "RR" },
                    {"SC" , "SC" },
                    {"SP" , "SP" },
                    {"SE" , "SE" },
                    {"TO" , "TO" }
                }, "Key", "Value");
            }
        }

        /// <summary>
        /// Retorna uma lista com as condições aplicaveis no select
        /// </summary>
        public static SelectList CondicoesSelect
        {
            get
            {
                return new SelectList( new[]{
                        new SelectListItem{ Value="0", Text="Igual"},
                        new SelectListItem{ Value="1", Text="Maior"},
                        new SelectListItem{ Value="2",  Text="Maior igual"},
                        new SelectListItem{ Value="3", Text="Menor"},
                        new SelectListItem{ Value="4", Text="Menor igual"},
                        new SelectListItem{ Value="5", Text="Iniciando"},
                        new SelectListItem{ Value="6", Text="Contendo"},
                        new SelectListItem{ Value="7", Text="Terminando"}
                    }, "Value", "Text", "0");
            }
        }

        /// <summary>
        /// Retorna uma lista com as condições aplicaveis no select
        /// </summary>
        public static SelectList OperadorSelect
        {
            get
            {
                return new SelectList(new[]{
                        new SelectListItem{ Value="and", Text="E"},
                        new SelectListItem{ Value="or", Text="Ou"}
                    }, "Value", "Text", "0");
            }
        }

        /// <summary>
        /// Enum com as descrições dos dias da semana
        /// </summary>
        public enum DiaDSR
        {
            Segunda = 1,
            [Description("Terça")]
            Terca = 2,
            Quarta = 3,
            Quinta = 4,
            Sexta = 5,
            [Description("Sábado")]
            Sabado = 6,
            Domingo = 7,
            Indefinido = -1
        }
    }
}
