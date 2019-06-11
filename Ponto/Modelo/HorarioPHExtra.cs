using System;

namespace Modelo
{
    public class HorarioPHExtra : Modelo.ModeloBase
    {
        [DataTableAttribute()]
        public int Idhorario { get; set; }
        [DataTableAttribute()]
        public Int16 Aplicacao { get; set; }
        [DataTableAttribute()]
        public decimal Percentualextra { get; set; }
        [DataTableAttribute()]
        public string Quantidadeextra { get; set; }
        [DataTableAttribute()]
        public decimal? PercentualExtraNoturna { get; set; }
        [DataTableAttribute()]
        public string QuantidadeExtraNoturna { get; set; }
        [DataTableAttribute()]
        public Int16 Marcapercentualextra { get; set; }
        public bool MarcapercentualextraBool
        {
            get { return Marcapercentualextra == 1 ? true : false; }
            set { Marcapercentualextra = value ? (Int16)1 : (Int16)0; }
        }
        [DataTableAttribute()]
        public Int16 Considerapercextrasemana { get; set; }
        [DataTableAttribute()]
        public Int16 TipoAcumulo { get; set; }
        [DataTableAttribute()]
        public Int16? PercentualExtraSegundo { get; set; }
        [DataTableAttribute()]
        public Int16? PercentualExtraSegundoNoturna { get; set; }
    }
}
