using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Modelo
{
    public class Departamento : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Descrição do Departamento
        /// </summary>  
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Descricao { get; set; }
        /// <summary>
        /// Identificação da Empresa
        /// </summary>
        public int IdEmpresa { get; set; }

        [TableHTMLAttribute("Empresa", 3, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public virtual string empresaNome { get; set; }

        /// <summary>
        /// Define qual a meta de horas extras para o departamento
        /// </summary>
        public Decimal? PercentualMaximoHorasExtras { get; set; }
        public int? idIntegracao { get; set; }
        public int? IdHorarioPadraoFunc { get; set; }
        [Display(Name = "Tipo Horário")]
        public int TipoHorarioPadraoFunc { get; set; }
        [Display(Name = "Horário padrão para integração de Funcionários")]
        public string Horario { get; set; }
    }
}
