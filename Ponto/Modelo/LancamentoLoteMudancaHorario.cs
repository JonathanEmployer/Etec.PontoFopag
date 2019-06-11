using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class LancamentoLoteMudancaHorario : Modelo.ModeloBase
    {
        public int IdLancamentoLote { get; set; }
        public LancamentoLote LancamentoLote { get; set; }
        
        /// <summary>
        /// Tipo do Horário: 1 = Normal ; 2  = Flexível; 3 = Dinamico
        /// </summary>
        [Display(Name = "Tipo Horário")]
        public Int32 Tipohorario { get; set; }

        /// <summary>
        /// Identificação do Horário
        /// </summary>
        public int Idhorario { get; set; }

        ///// <summary>
        ///// Valor anterior da variável Tipohorario
        ///// </summary>
        //public Int32 Tipohorario_ant { get; set; }
        /// <summary>
        /// Valor anterior da variável Idhorario
        /// </summary>
        public Int32 Idhorario_ant { get; set; }

        [Display(Name = "Horário Normal")]
        [RequiredIf("Tipohorario", 1, "Tipo Horário", "Normal")]
        public virtual string HorarioNormal { get; set; }
        [Display(Name = "Horário Flexível")]
        [RequiredIf("Tipohorario", 2, "Tipo Horário", "Flexível")]
        public virtual string HorarioFlexivel { get; set; }
        [Display(Name = "Horário Dinâmico")]
        [RequiredIf("Tipohorario", 3, "Tipo Horário", "Dinâmico")]
        public string HorarioDinamico { get; set; }
        public int? IdHorarioDinamico { get; set; }
        [Display(Name = "Índice Ciclo")]
        [RequiredIf("Tipohorario", 3, "Tipo Horário", "Dinâmico")]
        public int? CicloSequenciaIndice { get; set; }
    }
}


