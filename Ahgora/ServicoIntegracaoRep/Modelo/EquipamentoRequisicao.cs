using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoIntegracaoRep.Modelo
{
    /// <summary>
    /// Modelo que guarda os dados das requisições do rep, para serem exibidas na grid.
    /// </summary>
    public class EquipamentoRequisicao
    {
        public String NumSerie { get; set; }
        public String Requisicao { get; set; }
        public String Retorno { get; set; }
        public int TempoDormir { get; set; }
        public DateTime DataHoraRequisicao { get; set; }
        public String Erro { get; set; }
        public Int64 TotalDeRequisicoes { get; set; }
        public Int64 RequisicoesExecucaoAtual { get; set; }
        public string Empresa { get; set; }
    }
}
