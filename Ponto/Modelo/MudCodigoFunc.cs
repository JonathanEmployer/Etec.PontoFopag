using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class MudCodigoFunc : Modelo.ModeloBase
    {
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Data inicial da mudan�a de c�digo
        /// </summary>
        [Display(Name = "Data")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
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
        /// Identifica��o do Funcion�rio
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public int IdFuncionario { get; set; }
        /// <summary>
        /// C�digo antigo do Funcion�rio
        /// </summary>
        [Display(Name = "C�digo Antigo")]
        [TableHTMLAttribute("C�digo Antigo", 4, true, ItensSearch.text, OrderType.none)]
        public string DSCodigoAntigo { get; set; }
        /// <summary>
        /// C�digo novo do Funcion�rio
        /// </summary>
        [Display(Name = "C�digo Novo")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [TableHTMLAttribute("C�digo Novo", 5, true, ItensSearch.text, OrderType.none)]
        public string DSCodigoNovo { get; set; }
        public Int16 Tipohorario { get; set; }
        /// <summary>
        /// Identifica��o do Hor�rio Normal
        /// </summary>
        public int Idhorarionormal { get; set; }
        /// <summary>
        /// Identifica��o do Departamento
        /// </summary>
        public int Iddepartamento { get; set; }
        /// <summary>
        /// Identifica��o da Empresa
        /// </summary>
        public int Idempresa { get; set; }

        [Display(Name = "Funcion�rio")]
        [TableHTMLAttribute("Funcion�rio", 3, true, ItensSearch.text, OrderType.asc)]
        public string NomeFuncionario { get; set; }
    }
}
