using Modelo.Utils;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LancamentoLoteAfastamento : Modelo.ModeloBase
    {
        /// <summary>
        /// Id do lote vinculado ao registro.
        /// </summary>
        public int IdLancamentoLote { get; set; }
        public LancamentoLote LancamentoLote { get; set; }

        public LancamentoLoteAfastamento()
        {
            AbonoDiurno = "--:--";
            AbonoNoturno = "--:--";
        }

        /// <summary>
        /// Identificação da Ocorrência
        /// </summary>
        public int IdOcorrencia { get; set; }

        public int IdOcorrencia_Ant { get; set; }

        [DisplayName("Ocorrência")]
        public string Ocorrencia { get; set; }

        /// <summary>
        /// Data Inicial do Afastamento
        /// </summary>
        [DisplayName("Data Inicial")]
        [Required(ErrorMessage = "Campo Data Inicial Obrigatório")]
        [MinDate("01/01/1760")]
        public DateTime? DataI { get; set; }

        /// <summary>
        /// Data Final do Afastamento
        /// </summary>
        [DisplayName("Data Final")]
        public DateTime? DataF { get; set; }

        /// <summary>
        /// Abono Diurno
        /// </summary>
        [DisplayName("Abono Diurno")]
        public string AbonoDiurno { get; set; }
        public string AbonoDiurno_Ant { get; set; }

        /// <summary>
        /// Abono Noturno
        /// </summary>
        [DisplayName("Noturno")]
        public string AbonoNoturno { get; set; }
        public string AbonoNoturno_Ant { get; set; }


        /// <summary>
        /// Calcular Abono Parcialmente
        /// </summary>
        public Int16 Parcial { get; set; }

        [DisplayName("Parcial")]
        public bool BParcial
        {
            get { return Parcial == 1 ? true : false; }
            set { Parcial = value ? (Int16)1 : (Int16)0; }
        }
        public bool BParcial_Ant { get; set; }

        [DisplayName("Suspensão")]
        public bool BSuspensao { get; set; }
        public bool BSuspensao_Ant { get; set; }

        /// <summary>
        /// Abono sem calculo
        /// </summary>
        public Int16 SemCalculo { get; set; }

        [DisplayName("Sem Cálculo")]
        public bool BSemCalculo
        {
            get { return SemCalculo == 1 ? true : false; }
            set { SemCalculo = value ? (Int16)1 : (Int16)0; }
        }
        public bool BSemCalculo_Ant { get; set; }

        /// <summary>
        /// Abonado
        /// </summary>
        public Int16 Abonado { get; set; }
        
        [DisplayName("Abonado")]
        public bool BAbonado
        {
            get { return Abonado == 1 ? true : false; }
            set { Abonado = value ? (Int16)1 : (Int16)0; }
        }

        public bool BAbonado_Ant { get; set; }

        /// <summary>
        /// Valor Anterior da variável DataI
        /// </summary>
        public DateTime? DataI_Ant { get; set; }
        /// <summary>
        /// Valor Anterior da variável DataF
        /// </summary>
        public DateTime? DataF_Ant { get; set; }

        public virtual Funcionario ObjFuncionario { get; set; }

        public string OcorrenciaAnt { get; set; }

        [DisplayName("Sem Abono")]
        public bool SemAbono { get; set; }
        public bool SemAbono_Ant { get; set; }
    }
}


