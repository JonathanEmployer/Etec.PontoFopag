using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy.Relatorios
{
    public class PxyRelAfastamento
    {
        public int FuncionarioID { get; set; }

        public string FuncionarioCodigo { get; set; }

        public string FuncionarioNome { get; set; }

        public string FuncionarioCpf { get; set; }

        public decimal FuncionarioCpfDecimal { get; set; }

        public string FuncionarioMatricula { get; set; }

        public int AfastamentoId { get; set; }

        public int AfastamentoCodigo { get; set; }

        public string AfastamentoDescricao { get; set; }

        public Int16 AfastamentoAbonado { get; set; }

        public Int16 AfastamentoParcial { get; set; }

        public Int16 AfastamentoSemCalculo { get; set; }

        public Int16 AfastamentoSuspensao { get; set; }

        public Int16 AfastamentoSemAbono { get; set; }

        public DateTime AfastamentoData { get; set; }

        public DateTime AfastamentoDataInicio { get; set; }

        public DateTime? AfastamentoDataFim { get; set; }

        public string AfastamentoAbonoParcialDiurno { get; set; }

        public int AfastamentoAbonoParcialDiurnoMin { get; set; }

        public string AfastamentoAbonoParcialNoturno { get; set; }

        public int AfastamentoAbonoParcialNoturnoMin { get; set; }

        public string AfastamentoIdIntegrador { get; set; }

        public string AfastamentoObservacao { get; set; }

        public int OcorrenciaID { get; set; }

        public string OcorrenciaDescricao { get; set; }

        public bool OcorrenciaAbsenteismo { get; set; }

        public int TipoAbono { get; set; }

    }
}
