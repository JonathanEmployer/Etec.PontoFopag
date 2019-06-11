using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class MudancaHorario : Modelo.ModeloBase
    {
        /// <summary>
        /// Identifica��o do Funcion�rio
        /// </summary>
        public int Idfuncionario {get; set; }
        /// <summary>
        /// Tipo do Hor�rio: 1 = Normal ; 2  = Flex�vel
        /// </summary>
        [Display(Name = "Tipo Hor�rio")]
        public Int32 Tipohorario {get; set; }
        /// <summary>
        /// Identifica��o do Hor�rio
        /// </summary>
        public int Idhorario {get; set; }
        /// <summary>
        /// Data da Mudan�a de Hor�rio
        /// </summary>
        [Display (Name = "Data da Mudan�a")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        public DateTime? Data {get; set; }

        [TableHTMLAttribute("Data", 1, true, ItensSearch.text, OrderType.asc)]
        public string DataStr { get { return String.Format("{0:dd/MM/yyyy}", Data);} }
        /// <summary>
        /// Valor anterior da vari�vel Tipohorario
        /// </summary>
        public Int32 Tipohorario_ant {get; set; }
        /// <summary>
        /// Valor anterior da vari�vel Idhorario
        /// </summary>
        public Int32 Idhorario_ant {get; set; }
        /// <summary>
        /// Valor anterior da vari�vel Data
        /// </summary>
        public DateTime? Data_Ant { get; set; }
        /// <summary>
        /// Tipo da Mudan�a: 0 = Funcionario ; 1  = Departamento; 2 = Empresa; 3 = Fun��o
        /// </summary>
        [Display(Name = "Tipo Mudan�a")]
        public int Tipo { get; set; }
        [Display(Name = "Empresa")]
        [RequiredIf("Tipo", 2, "Tipo Mudan�a", "Empresa")]
        public virtual string NomeEmpresa { get; set; }

        [Display(Name = "Departamento")]
        [RequiredIf("Tipo", 1, "Tipo Mudan�a", "Departamento")]
        public virtual string NomeDepartamento { get; set; }

        [Display(Name = "Funcion�rio")]
        [RequiredIf("Tipo", 0, "Tipo Mudan�a", "Funcion�rio")]
        public virtual string NomeFuncionario { get; set; }
        [Display(Name = "Fun��o")]
        [RequiredIf("Tipo", 3, "Tipo Mudan�a", "Fun��o")]
        public virtual string NomeFuncao { get; set; }
        public int IdEmpresa { get; set; }
        public int IdDepartamento { get; set; }
        public int IdFuncao { get; set; }
        [Display(Name = "Hor�rio Normal")]
        [RequiredIf("Tipohorario", 1, "Tipo Hor�rio", "Normal")]
        public virtual string HorarioNormal { get; set; }
        [Display(Name = "Hor�rio Flex�vel")]
        [RequiredIf("Tipohorario", 2, "Tipo Hor�rio", "Flex�vel")]
        public virtual string HorarioFlexivel { get; set; }
        [Display(Name = "Hor�rio Din�mico")]
        [RequiredIf("Tipohorario", 3, "Tipo Hor�rio", "Din�mico")]
        public string HorarioDinamico { get; set; }
        public bool NaoRecalcular { get; set; }
        [TableHTMLAttribute("Turno Antigo", 2, true, ItensSearch.select, OrderType.none)]
        public string descricaohorario_ant { get; set; }
        [TableHTMLAttribute("Turno Novo", 4, true, ItensSearch.select, OrderType.none)]
        public string descricaohorario { get; set; }
        [TableHTMLAttribute("Tipo Hor�rio", 3, true, ItensSearch.select, OrderType.none)]
        public string tipohorariodesc_ant { get; set; }
        [TableHTMLAttribute("Tipo Hor�rio", 5, true, ItensSearch.select, OrderType.none)]
        public string tipohorariodesc { get; set; }
        public int? IdLancamentoLoteFuncionario { get; set; }

        public int? IdHorarioDinamico { get; set; }
        [Display(Name = "�ndice Ciclo")]
        [RequiredIf("Tipohorario", 3, "Tipo Hor�rio", "Din�mico")]
        public int? CicloSequenciaIndice { get; set; }
    }
}
