using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class MudCodigoFunc : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Data inicial da mudança de código
        /// </summary>
        [Display(Name = "Data")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime? Datainicial { get; set; }
        [TableHTMLAttribute("Data", 2, true, ItensSearch.text, OrderType.none)]
        public string DataInicialStr
        {
            get
            {
                return Datainicial == null ? "" : Datainicial.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        /// <summary>
        /// Identificação do Funcionário
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int IdFuncionario { get; set; }
        /// <summary>
        /// Código antigo do Funcionário
        /// </summary>
        [Display(Name = "Código Antigo")]
        [TableHTMLAttribute("Código Antigo", 4, true, ItensSearch.text, OrderType.none)]
        public string DSCodigoAntigo { get; set; }
        /// <summary>
        /// Código novo do Funcionário
        /// </summary>
        [Display(Name = "Código Novo")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [TableHTMLAttribute("Código Novo", 5, true, ItensSearch.text, OrderType.none)]
        public string DSCodigoNovo { get; set; }
        public Int16 Tipohorario { get; set; }
        /// <summary>
        /// Identificação do Horário Normal
        /// </summary>
        public int Idhorarionormal { get; set; }
        /// <summary>
        /// Identificação do Departamento
        /// </summary>
        public int Iddepartamento { get; set; }
        /// <summary>
        /// Identificação da Empresa
        /// </summary>
        public int Idempresa { get; set; }

        [Display(Name = "Funcionário")]
        [TableHTMLAttribute("Funcionário", 3, true, ItensSearch.text, OrderType.asc)]
        public string NomeFuncionario { get; set; }
    }
}
