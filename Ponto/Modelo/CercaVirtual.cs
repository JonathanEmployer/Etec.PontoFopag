using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class CercaVirtual : ModeloBase
    {
        [TableHTMLAttribute("Id", 0, false, ItensSearch.none, OrderType.none)]
        public int Ids { get { return this.Id; } }
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Código")]
        public int Codigos { get { return this.Codigo; } }
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição")]
        [Required]
        [StringLength(150, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Descricao { get; set; }

        [TableHTMLAttribute("Tipo de Cerca Virtual", 3, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Tipo de Cerca Virtual")]
        [Required]
        [StringLength(150, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string TipoDescricao { get; set; }

        [TableHTMLAttribute("Endereço", 4, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Endereço")]
        [Required]
        public string Endereco { get; set; }

        [TableHTMLAttribute("Latitude", 5, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Latitude")]
        [Required]
        public string Latitude { get; set; }

        [TableHTMLAttribute("Longitude", 6, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Longitude")]
        [Required]
        public string Longitude { get; set; }

        [TableHTMLAttribute("Raio (m)", 7, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Raio (m)")]
        [Required]
        [Range(100, Int32.MaxValue)]
        public int Raio { get; set; }

        public bool Ativo { get; set; }

        [TableHTMLAttribute("Ativo", 8, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Ativo")]
        public string AtivoStr
        {
            get
            {
                return Ativo == true ? "Sim" : "Não";
            }
        }

        public string idsFuncionariosSelecionados { get; set; }

        public List<pxyCercaVirtualFuncionarioGrid> Funcionario { get; set; }
    }
}
