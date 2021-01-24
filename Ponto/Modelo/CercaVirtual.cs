using Modelo.Proxy;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class CercaVirtual : ModeloBase
    {
        [TableHTMLAttribute("Código", 0, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Código")]
        public int Codigo { get; set; }
        [TableHTMLAttribute("Descrição", 1, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição")]
        [Required]
        public string Descricao { get; set; }

        [TableHTMLAttribute("Tipo", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Tipo")]
        [Required]
        public string TipoDescricao { get; set; }

        [TableHTMLAttribute("Endereço", 3, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Endereço")]
        [Required]
        public string Endereco { get; set; }

        [TableHTMLAttribute("Latitude", 4, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Latitude")]
        [Required]
        public string Latitude { get; set; }

        [TableHTMLAttribute("Longitude", 5, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Longitude")]
        [Required]
        public string Longitude { get; set; }

        [TableHTMLAttribute("Raio (m)", 6, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Raio (m)")]
        [Required]
        public int Raio { get; set; }

        public bool Ativo { get; set; }

        [TableHTMLAttribute("Ativo", 7, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Ativo")]
        public string AtivoStr
        {
            get
            {
                return Ativo == true ? "Sim" : "Não";
            }
        }

        public string idsFuncionariosSelecionados { get; set; }

        public List<pxyFuncionarioGrid> Funcionario { get; set; }
    }
}
