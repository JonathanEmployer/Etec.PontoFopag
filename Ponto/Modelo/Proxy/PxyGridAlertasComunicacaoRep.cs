using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo.Proxy
{
    public class PxyGridAlertasComunicacaoRep : Modelo.ModeloBase
    {
        [TableHTML("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        public String Descricao { get; set; }

        [TableHTML("E-mail Destinatário", 3, true, ItensSearch.select, OrderType.none)]
        public String EmailUsuario { get; set; }

        public DateTime? UltimaExecucao { get; set; }

        public Boolean EmailIndividual { get; set; }

        public Boolean Ativo { get; set; }

        [TableHTML("Intervalo", 4, true, ItensSearch.select, OrderType.none)]
        public string IntervaloVerificacaoLivre { get; set; }

        [TableHTML("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoExibicao { get { return Codigo; } }

        [TableHTML("Última Execução", 5, true, ItensSearch.text, OrderType.none)]
        public String UltimaExecucaoStr { get { return UltimaExecucao == null ? "" : UltimaExecucao.GetValueOrDefault().ToShortDateString(); } }


        [TableHTML("Ativo", 6, true, ItensSearch.select, OrderType.none)]
        public string AtivoStr { get { if (Ativo) return "Sim"; else return "Não"; } }

    }
}
