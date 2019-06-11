namespace Modelo
{
    public class FechamentobhdHE : Modelo.ModeloBase
    {
        /// <summary>
        /// ID da Marcação
        /// </summary>
        public int IdMarcacao { get; set; }

        /// <summary>
        /// ID do fechamentoBH
        /// </summary>
        public int IdFechamentoBH { get; set; }

        public string QuantHorasPerc1 { get; set; }
        public string QuantHorasPerc2 { get; set; }
        public int PercQuantHorasPerc1 { get; set; }
        public int PercQuantHorasPerc2 { get; set; }
        public virtual Marcacao marcacao { get; set; }
        public virtual FechamentoBH fechamentoBH { get; set; }
    }
}
