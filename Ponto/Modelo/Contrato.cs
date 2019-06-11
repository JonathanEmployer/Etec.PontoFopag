using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class Contrato : ModeloBase
    {
        public int IdEmpresa { get; set; }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }

        [TableHTMLAttribute("Código do Contrato", 2, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Código do Contrato")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(255, ErrorMessage = "O Código do Contrato deve ter no máximo 255 caracteres.")]
        public string CodigoContrato { get; set; }
        [TableHTMLAttribute("Descrição", 3, true, ItensSearch.text, OrderType.asc)]
        [Display(Name = "Descrição do Contrato")]
        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(1024, ErrorMessage = "A Descrição do Contrato deve ter no máximo 1024 caracteres.")]
        public string DescricaoContrato { get; set; }
        public List<ContratoUsuario> Usuarios { get; set; }
        public List<ContratoFuncionario> FuncionariosContratados { get; set; }
        [TableHTMLAttribute("Empresa", 4, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Campo obrigatório")]
        public string NomeEmpresa { get; set; }
        public int? idIntegracao { get; set; }
        public int Cnpj { get; set; }

        /// DiaFechamento
        [TableHTMLAttribute("Fechamento Inicial", 5, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Dia inicial")]
        [Range(0, 31, ErrorMessage = "A data inicial deve possuir valores entre 0 e 31")]
        public int DiaFechamentoInicial { get; set; }
        [TableHTMLAttribute("Fechamento Final", 6, true, ItensSearch.text, OrderType.none)]
        [Display(Name = "Dia final")]
        [Range(0, 31, ErrorMessage = "O dia de fechamento deve possuir valores entre 0 e 31")]
        public int DiaFechamentoFinal { get; set; }
        [Display(Name = "Horário padrão para integração de Funcionários")]
        public string Horario { get; set; }
        public int? IdHorarioPadraoFunc { get; set; }
        [Display(Name = "Tipo Horário")]
        public int TipoHorarioPadraoFunc { get; set; }
    }
}
