using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyFuncionarioGrid
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Id { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string Codigo { get; set; }

        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }

        [TableHTMLAttribute("Matrícula", 3, true, ItensSearch.text, OrderType.none)]
        public string Matricula { get; set; }

        [TableHTMLAttribute("Horário", 4, true, ItensSearch.select, OrderType.none)]
        public string Horario { get; set; }

        [TableHTMLAttribute("Empresa", 5, true, ItensSearch.select, OrderType.none)]
        public string Empresa { get; set; }

        [TableHTMLAttribute("Departamento", 6, true, ItensSearch.select, OrderType.none)]
        public string Departamento { get; set; }

        [TableHTMLAttribute("Função", 7, true, ItensSearch.select, OrderType.none)]
        public string Funcao { get; set; }

        public int IdContrato { get; set; }
        [TableHTMLAttribute("Contrato", 8, true, ItensSearch.select, OrderType.none)]
        public string Contrato { get; set; }

        [TableHTMLAttribute("Carteira", 9, true, ItensSearch.text, OrderType.none)]
        public string Carteira { get; set; }

        [TableHTMLAttribute("CPF", 10, true, ItensSearch.text, OrderType.none)]
        public string CPF { get; set; }

        [TableHTMLAttribute("Código Folha", 11, true, ItensSearch.text, OrderType.none)]
        public int? CodigoFolha { get; set; }

        [TableHTMLAttribute("Pis", 12, true, ItensSearch.text, OrderType.none)]
        public string Pis { get; set; }

        [TableHTMLAttribute("Admissão", 13, true, ItensSearch.text, OrderType.none, ColumnType.data)]
        public string DataAdmissao { get; set; }

        [TableHTMLAttribute("Demissão", 14, true, ItensSearch.text, OrderType.none, ColumnType.data)]
        public string DataDemissao { get; set; }

        [TableHTMLAttribute("Ativo", 15, true, ItensSearch.select, OrderType.none)]
        public string Ativo { get; set; }
        [TableHTMLAttribute("Data Inativação", 16, true, ItensSearch.text, OrderType.none, ColumnType.data)]
        public string DataInativacao { get; set; }

        [TableHTMLAttribute("Tipo Horário", 17, true, ItensSearch.select, OrderType.none)]
        public string TipoHorario { get; set; }

        [TableHTMLAttribute("Supervisor", 18, true, ItensSearch.select, OrderType.none)]
        public string Supervisor { get; set; }

        [TableHTMLAttribute("Pessoa Supervisor", 19, true, ItensSearch.select, OrderType.none)]
        public string PessoaSupervisor { get; set; }

        [TableHTMLAttribute("Entra Banco Hora", 20, true, ItensSearch.select, OrderType.none)]
        public string EntraBancoHoras { get; set; }

        [TableHTMLAttribute("Entra Compensação", 21, true, ItensSearch.select, OrderType.none)]
        public string EntraCompensacao { get; set; }

        [TableHTMLAttribute("Utiliza Registrador", 22, true, ItensSearch.select, OrderType.none)]
        public string Utilizaregistrador { get; set; }

        [TableHTMLAttribute("APP", 23, true, ItensSearch.select, OrderType.none)]
        public string UtilizaAppPontofopag { get; set; }

        [TableHTMLAttribute("Reconhecimento Facil APP", 24, true, ItensSearch.select, OrderType.none)]
        public string UtilizaReconhecimentoFacialApp { get; set; }

        [TableHTMLAttribute("Web APP", 25, true, ItensSearch.select, OrderType.none)]
        public string UtilizaWebAppPontofopag { get; set; }

        [TableHTMLAttribute("Reconhecimento Facil Web APP", 26, true, ItensSearch.select, OrderType.none)]
        public string UtilizaReconhecimentoFacialWebApp { get; set; }


        [TableHTMLAttribute("Tipo Mão Obra", 27, true, ItensSearch.select, OrderType.none)]
        public string TipoMaoObra { get; set; }

        [TableHTMLAttribute("Alocação", 28, true, ItensSearch.select, OrderType.none)]
        public string Alocacao { get; set; }

        [TableHTMLAttribute("Tipo Vínculo", 29, true, ItensSearch.select, OrderType.none)]
        public string TipoVinculo { get; set; }

        [TableHTMLAttribute("RFID", 30, true, ItensSearch.text, OrderType.none)]
        public Int64? RFID { get; set; }
        [TableHTMLAttribute("Cerca Virtual", 31, true, ItensSearch.text, OrderType.none)]
        public string PossuiCercaVirtual { get; set; }

    }
}
