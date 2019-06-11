using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelo
{
    public class Departamento : Modelo.ModeloBase
    {
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Descri��o do Departamento
        /// </summary>  
        [TableHTMLAttribute("Descri��o", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descri��o")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [StringLength(200, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        public string Descricao { get; set; }
        /// <summary>
        /// Identifica��o da Empresa
        /// </summary>
        public int IdEmpresa { get; set; }

        [TableHTMLAttribute("Empresa", 3, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public virtual string empresaNome { get; set; }

        /// <summary>
        /// Define qual a meta de horas extras para o departamento
        /// </summary>
        public Decimal? PercentualMaximoHorasExtras { get; set; }
        public int? idIntegracao { get; set; }
        public int? IdHorarioPadraoFunc { get; set; }
        [Display(Name = "Tipo Hor�rio")]
        public int TipoHorarioPadraoFunc { get; set; }
        [Display(Name = "Hor�rio padr�o para integra��o de Funcion�rios")]
        public string Horario { get; set; }
    }
}
