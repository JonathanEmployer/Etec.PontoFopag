using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class JornadaSubstituir : Modelo.ModeloBase
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int32 IdJornadaDe { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int32 IdJornadaPara { get; set; }

        [Display(Name = "DataInicio")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "DataFim")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataFim { get; set; }

        [Display(Name = "Jornada De")]
        [TableHTMLAttribute("Jornada De", 5, false, ItensSearch.text, OrderType.none)]
        public string DescricaoDe { get; set; }

        [Display(Name = "Descrição")]
        [TableHTMLAttribute("Jornada Para", 6, false, ItensSearch.text, OrderType.none)]
        public string DescricaoPara { get; set; }

        #region Campos para Grid
        [TableHTMLAttribute("Data Início", 3, false, ItensSearch.text, OrderType.none)]
        public string DataInicioStr => DataInicio.ToShortDateString();

        [TableHTMLAttribute("Data Fim", 4, false, ItensSearch.text, OrderType.none)]
        public string DataFimStr => DataFim.ToShortDateString();

        [TableHTMLAttribute("Usuario Inc.", 7, false, ItensSearch.select, OrderType.none)]
        public string IncUsuarioGrid => Incusuario;

        [TableHTMLAttribute("Data/Hora Inc.", 8, false, ItensSearch.text, OrderType.none)]
        public string IncHoraGrid => Inchora == null ? "" : Inchora.GetValueOrDefault().ToShortDateString() + " " + Inchora.GetValueOrDefault().ToShortTimeString();

        [TableHTMLAttribute("Usuario Alt.", 9, false, ItensSearch.select, OrderType.none)]
        public string AltUsuarioGrid => Altusuario;

        [TableHTMLAttribute("Data/Hora Alt.", 10, false, ItensSearch.text, OrderType.none)]
        public string AltHoraGrid => Althora == null ? "" : Althora.GetValueOrDefault().ToShortDateString() + " " + Althora.GetValueOrDefault().ToShortTimeString(); 
        #endregion
    }
}
