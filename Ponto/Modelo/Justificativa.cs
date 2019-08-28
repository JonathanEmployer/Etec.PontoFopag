using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Justificativa : Modelo.ModeloBase
    {
        public Justificativa()
        {
            Descricao = "";
        }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Descrição da Justificativa
        /// </summary>    
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(80, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        public string Descricao { get; set; }
        public int? IdIntegracao { get; set; }
        [Display(Name = "Exibe no Painel do RH")]
        public bool ExibePaineldoRH { get; set; }
        [TableHTMLAttribute("Exibe no Painel do RH", 3, true, ItensSearch.text, OrderType.none)]
        public string ExibePaineldoRHStr
        {
            get
            {
                return ExibePaineldoRH == true ? "Sim" : "Não";
            }
        }
		[Display(Name = "Ativo")]
		public bool Ativo { get; set; }
		[TableHTMLAttribute("Ativo", 4, true, ItensSearch.text, OrderType.none)]
		public string AtivoStr
		{
			get
			{
				return Ativo == true ? "Sim" : "Não";
			}
		}

        public List<JustificativaRestricao> JustificativaRestricao { get; set; }
    }
}
