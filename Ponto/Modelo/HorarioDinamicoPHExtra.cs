using System;

namespace Modelo
{
    public class HorarioDinamicoPHExtra : Modelo.ModeloBase
    {
        [DataTableAttribute()]
        public int IdHorarioDinamico { get; set; }
        [DataTableAttribute()]
        public decimal PercentualExtra { get; set; }
        [DataTableAttribute()]
        public string QuantidadeExtra { get; set; }
        [DataTableAttribute()]
        public decimal? PercentualExtraNoturna { get; set; }
        [DataTableAttribute()]
        public string QuantidadeExtraNoturna { get; set; }
        [DataTableAttribute()]
        public Int16 MarcaPercentualExtra { get; set; }
        
        public bool MarcaPercentualExtraBool
        {
            get { return MarcaPercentualExtra == 1 ? true : false; }
            set { MarcaPercentualExtra = value ? (Int16)1 : (Int16)0; }
        }
        [DataTableAttribute()]
        public Int16 ConsideraPercExtraSemana { get; set; }
        [DataTableAttribute()]
        public Int16 TipoAcumulo { get; set; }
        [DataTableAttribute()]
        public Int16? PercentualExtraSegundo { get; set; }
        [DataTableAttribute()]
        public Int16? PercentualExtraSegundoNoturna { get; set; }
    }
}
