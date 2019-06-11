using System;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;

namespace Modelo
{
    public class HorarioDetalhe : Modelo.ModeloBase
    {        
        [DataTableAttribute()]
        public int Idhorario { get; set; }
        [DataTableAttribute()]
        public int Dia { get; set; }
        public string DiaDesc {
            get { return DiasSemana.DiaSemanaExtenso(Dia); }
        }
        [DataTableAttribute()]
        public DateTime? Data { get; set; }
        public string DataString
        {
            get
            {
                return Data == null ? "" : Data.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        [DataTableAttribute()]
        [Display(Name = "Ent. 1")]
        public string Entrada_1 { get; set; }
        [DataTableAttribute()]
        [Display(Name = "Ent. 2")]
        public string Entrada_2 { get; set; }
        [DataTableAttribute()]
        [Display(Name = "Ent. 3")]
        public string Entrada_3 { get; set; }
        [DataTableAttribute()]
        [Display(Name = "Ent. 4")]
        public string Entrada_4 { get; set; }
        [DataTableAttribute()]
        [Display(Name = "Sai. 1")]
        public string Saida_1 { get; set; }
        [DataTableAttribute()]
        [Display(Name = "Sai. 2")]
        public string Saida_2 { get; set; }
        [DataTableAttribute()]
        [Display(Name = "Sai. 3")]
        public string Saida_3 { get; set; }
        [DataTableAttribute()]
        [Display(Name = "Sai. 4")]
        public string Saida_4 { get; set; }

        public int EntradaMin_1 { get; set; }
        public int EntradaMin_2 { get; set; }
        public int EntradaMin_3 { get; set; }
        public int EntradaMin_4 { get; set; }
        public int SaidaMin_1 { get; set; }
        public int SaidaMin_2 { get; set; }
        public int SaidaMin_3 { get; set; }
        public int SaidaMin_4 { get; set; }
        [DataTableAttribute()]
        public string Totaltrabalhadadiurna { get; set; }
        [DataTableAttribute()]
        public string Totaltrabalhadanoturna { get; set; }
        [DataTableAttribute()]
        public string Cargahorariamista { get; set; }

        public int TotaltrabalhadadiurnaMin { get; set; }
        public int TotaltrabalhadanoturnaMin { get; set; }
        public int CargahorariamistaMin { get; set; }
        [DataTableAttribute()]
        public Int16 Flagfolga { get; set; }
        public string Folga { get; set; }
        [DataTableAttribute()]
        public Int16 Diadsr { get; set; }
        public string DSR { get; set; }
        [DataTableAttribute()]
        public Int16 Intervaloautomatico { get; set; }
        [DataTableAttribute()]
        public Int16 Preassinaladas1 { get; set; }
        public bool Preassinaladas1Bool
        {
            get { return Preassinaladas1 == 1 ? true : false; }
            set { Preassinaladas1 = value ? (Int16)1 : (Int16)0; }
        }
        [DataTableAttribute()]
        public Int16 Preassinaladas2 { get; set; }
        public bool Preassinaladas2Bool
        {
            get { return Preassinaladas2 == 1 ? true : false; }
            set { Preassinaladas2 = value ? (Int16)1 : (Int16)0; }
        }
        [DataTableAttribute()]
        public Int16 Preassinaladas3 { get; set; }
        public bool Preassinaladas3Bool
        {
            get { return Preassinaladas3 == 1 ? true : false; }
            set { Preassinaladas3 = value ? (Int16)1 : (Int16)0; }
        }
        [DataTableAttribute()]
        public Int16 bCarregar { get; set; }
        [Required]
        public bool bCarregarBool
        {
            get { return bCarregar == 1 ? true : false; }
            set { bCarregar = value ? (Int16)1 : (Int16)0; }
        }
        [DataTableAttribute()]
        public Int16 Marcacargahorariamista { get; set; }
        public string InicioAdNoturno { get; set; }
        public string FimAdNoturno { get; set; }
        public string DiaStr { get; set; }
        [DataTableAttribute()]
        public int? Idjornada { get; set; }
        [Display(Name = "Jornada")]
        public virtual string DescJornada { get; set; }
        [DataTableAttribute()]
        [Display(Name = "Neutro")]
        public bool Neutro { get; set; }
        [DataTableAttribute()]
        public int? CicloSequenciaIndice { get; set; }

        public void ConverteHoraStringToInt()
        {
            EntradaMin_1 = cwkFuncoes.ConvertBatidaMinuto(Entrada_1);
            EntradaMin_2 = cwkFuncoes.ConvertBatidaMinuto(Entrada_2);
            EntradaMin_3 = cwkFuncoes.ConvertBatidaMinuto(Entrada_3);
            EntradaMin_4 = cwkFuncoes.ConvertBatidaMinuto(Entrada_4);

            SaidaMin_1 = cwkFuncoes.ConvertBatidaMinuto(Saida_1);
            SaidaMin_2 = cwkFuncoes.ConvertBatidaMinuto(Saida_2);
            SaidaMin_3 = cwkFuncoes.ConvertBatidaMinuto(Saida_3);
            SaidaMin_4 = cwkFuncoes.ConvertBatidaMinuto(Saida_4);

            TotaltrabalhadadiurnaMin = cwkFuncoes.ConvertHorasMinuto(Totaltrabalhadadiurna);
            TotaltrabalhadanoturnaMin = cwkFuncoes.ConvertHorasMinuto(Totaltrabalhadanoturna);
            CargahorariamistaMin = cwkFuncoes.ConvertHorasMinuto(Cargahorariamista);
        }

        public string[] getEntradas()
        {
            string[] entradas = new string[] { Entrada_1, Entrada_2, Entrada_3, Entrada_4 };

            return entradas;
        }

        public string[] getSaidas()
        {
            string[] saidas = new string[] { Saida_1, Saida_2, Saida_3, Saida_4 };

            return saidas;
        }

        public string[] getEntradasSaidas()
        {
            string[] batidas = new string[] { Entrada_1, Saida_1, Entrada_2, Saida_2, Entrada_3, Saida_3, Entrada_4, Saida_4 };

            return batidas;
        }

        public int[] getEntradasSaidasMin()
        {
            int[] batidas = new int[] { EntradaMin_1, SaidaMin_1, EntradaMin_2, SaidaMin_2, EntradaMin_3, SaidaMin_3, EntradaMin_4, SaidaMin_4 };

            return batidas;
        }
    }

    public class HorarioDetalheMovel
    {
        public string Entrada_1 { get; set; }
        public string Entrada_2 { get; set; }
        public string Entrada_3 { get; set; }
        public string Entrada_4 { get; set; }
        public string Saida_1 { get; set; }
        public string Saida_2 { get; set; }
        public string Saida_3 { get; set; }
        public string Saida_4 { get; set; }
        public string Totaltrabalhadadiurna { get; set; }
        public string Totaltrabalhadanoturna { get; set; }
        public string Cargahorariamista { get; set; }
        public string Toleranciaantes { get; set; }
        public string Toleranciadepois { get; set; }
        public Int16 Marcacargahorariamista { get; set; }
        public Int16 Intervaloautomatico { get; set; }
        public Int16 Preassinaladas1 { get; set; }
        public Int16 Preassinaladas2 { get; set; }
        public Int16 Preassinaladas3 { get; set; }
        public int? Idjornada { get; set; }
        
    }
}
