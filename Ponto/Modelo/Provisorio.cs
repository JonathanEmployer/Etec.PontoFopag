using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Provisorio : Modelo.ModeloBase
    {
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }

        /// <summary>
        /// C�digo provisorio do funcionario
        /// </summary>
        
        [TableHTMLAttribute("C�digo Funcion�rio", 2, true, ItensSearch.text, OrderType.none)]
        public string Dsfuncionario { get; set; }

        /// <summary>
        /// C�digo real do funcionario (depois de cadastrado certinho)
        /// </summary>
        [Display(Name = "C�digo Tempor�rio")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [TableHTMLAttribute("C�digo Tempor�rio", 3, true, ItensSearch.text, OrderType.none)]
        public string Dsfuncionarionovo { get; set; }

        /// <summary>
        /// Data inicial do C�digo Provi�rio
        /// </summary>
        [Display(Name = "Data Inicial")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public DateTime? Dt_inicial { get; set; }
        [TableHTMLAttribute("Data Inicial", 4, true, ItensSearch.text, OrderType.none)]
        public string DtInicialStr
        {
            get
            {
                return Dt_inicial == null ? "" : Dt_inicial.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        /// <summary>
        /// Data final do C�digo Provi�rio
        /// </summary>
        [Display(Name = "Data Final")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public DateTime? Dt_final { get; set; }
        [TableHTMLAttribute("Data Final", 5, true, ItensSearch.text, OrderType.none)]
        public string DtFinalStr
        {
            get
            {
                return Dt_final == null ? "" : Dt_final.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        [Display(Name = "C�digo Funcion�rio")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public string NomeFuncionario { get; set; }
    }
}
