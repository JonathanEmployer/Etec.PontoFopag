using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Provisorio : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }

        /// <summary>
        /// Código provisorio do funcionario
        /// </summary>
        
        [TableHTMLAttribute("Código Funcionário", 2, true, ItensSearch.text, OrderType.none)]
        public string Dsfuncionario { get; set; }

        /// <summary>
        /// Código real do funcionario (depois de cadastrado certinho)
        /// </summary>
        [Display(Name = "Código Temporário")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [TableHTMLAttribute("Código Temporário", 3, true, ItensSearch.text, OrderType.none)]
        public string Dsfuncionarionovo { get; set; }

        /// <summary>
        /// Data inicial do Código Proviório
        /// </summary>
        [Display(Name = "Data Inicial")]
        [Required(ErrorMessage = "Campo Obrigatório")]
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
        /// Data final do Código Proviório
        /// </summary>
        [Display(Name = "Data Final")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime? Dt_final { get; set; }
        [TableHTMLAttribute("Data Final", 5, true, ItensSearch.text, OrderType.none)]
        public string DtFinalStr
        {
            get
            {
                return Dt_final == null ? "" : Dt_final.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        [Display(Name = "Código Funcionário")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string NomeFuncionario { get; set; }
    }
}
