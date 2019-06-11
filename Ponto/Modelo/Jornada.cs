using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{

    public class Jornada : Modelo.ModeloBase
    {
        public Jornada()
        {
            Descricao = String.Empty;
            Entrada_1 = "--:--";
            Entrada_2 = "--:--";
            Entrada_3 = "--:--";
            Entrada_4 = "--:--";
            Saida_1 = "--:--";
            Saida_2 = "--:--";
            Saida_3 = "--:--";
            Saida_4 = "--:--";
        }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.asc)]
        public int CodigoGrid { get { return this.Codigo; } }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Display(Name = "Ent. 1")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        public string Entrada_1 { get; set; }
        [Display(Name = "Ent. 2")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        public string Entrada_2 { get; set; }
        [Display(Name = "Ent. 3")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        public string Entrada_3 { get; set; }
        [Display(Name = "Ent. 4")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        public string Entrada_4 { get; set; }
        [Display(Name = "Sai. 1")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        public string Saida_1 { get; set; }
        [Display(Name = "Sai. 2")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        public string Saida_2 { get; set; }
        [Display(Name = "Sai. 3")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        public string Saida_3 { get; set; }
        [Display(Name = "Sai. 4")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        public string Saida_4 { get; set; }

        public string Entrada_1Ant { get; set; }
        public string Entrada_2Ant { get; set; }
        public string Entrada_3Ant { get; set; }
        public string Entrada_4Ant { get; set; }
        public string Saida_1Ant { get; set; }
        public string Saida_2Ant { get; set; }
        public string Saida_3Ant { get; set; }
        public string Saida_4Ant { get; set; }
        [TableHTMLAttribute("Horários", 2, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Horários")]
        public virtual string horarios { get {
            string hor = Entrada_1+" - "+Saida_1;
            if (Entrada_2 != "--:--") { hor = hor + " - " + Entrada_2; };
            if (Saida_2 != "--:--") { hor = hor + " - " + Saida_2; };
            if (Entrada_3 != "--:--") { hor = hor + " - " + Entrada_3; };
            if (Saida_3 != "--:--") { hor = hor + " - " + Saida_3; };
            if (Entrada_4 != "--:--") { hor = hor + " - " + Entrada_4; };
            if (Saida_4 != "--:--") { hor = hor + " - " + Saida_4; };
            return hor;
        } }

        public int[] EntradasMin {
            get {
                return new int[] { cwkFuncoes.ConvertBatidaMinuto(Entrada_1), cwkFuncoes.ConvertBatidaMinuto(Entrada_2), cwkFuncoes.ConvertBatidaMinuto(Entrada_3), cwkFuncoes.ConvertBatidaMinuto(Entrada_4) };
            }
        }

        public int[] SaidasMin
        {
            get
            {
                return new int[] { cwkFuncoes.ConvertBatidaMinuto(Saida_1), cwkFuncoes.ConvertBatidaMinuto(Saida_2), cwkFuncoes.ConvertBatidaMinuto(Saida_3), cwkFuncoes.ConvertBatidaMinuto(Saida_4) };
            }
        }

        public string TotalTrabalhadaDiurna { get; set; }
        public string TotalTrabalhadaNoturna { get; set; }
        public string TotalTrabalhada { get; set; }
    }
}
