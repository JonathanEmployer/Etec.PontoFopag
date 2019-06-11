using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class MudancaHorario : Modelo.ModeloBase
    {
        /// <summary>
        /// Identificação do Funcionário
        /// </summary>
        public int Idfuncionario {get; set; }
        /// <summary>
        /// Tipo do Horário: 1 = Normal ; 2  = Flexível
        /// </summary>
        [Display(Name = "Tipo Horário")]
        public Int32 Tipohorario {get; set; }
        /// <summary>
        /// Identificação do Horário
        /// </summary>
        public int Idhorario {get; set; }
        /// <summary>
        /// Data da Mudança de Horário
        /// </summary>
        [Display (Name = "Data da Mudança")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime? Data {get; set; }

        [TableHTMLAttribute("Data", 1, true, ItensSearch.text, OrderType.asc)]
        public string DataStr { get { return String.Format("{0:dd/MM/yyyy}", Data);} }
        /// <summary>
        /// Valor anterior da variável Tipohorario
        /// </summary>
        public Int32 Tipohorario_ant {get; set; }
        /// <summary>
        /// Valor anterior da variável Idhorario
        /// </summary>
        public Int32 Idhorario_ant {get; set; }
        /// <summary>
        /// Valor anterior da variável Data
        /// </summary>
        public DateTime? Data_Ant { get; set; }
        /// <summary>
        /// Tipo da Mudança: 0 = Funcionario ; 1  = Departamento; 2 = Empresa; 3 = Função
        /// </summary>
        [Display(Name = "Tipo Mudança")]
        public int Tipo { get; set; }
        [Display(Name = "Empresa")]
        [RequiredIf("Tipo", 2, "Tipo Mudança", "Empresa")]
        public virtual string NomeEmpresa { get; set; }

        [Display(Name = "Departamento")]
        [RequiredIf("Tipo", 1, "Tipo Mudança", "Departamento")]
        public virtual string NomeDepartamento { get; set; }

        [Display(Name = "Funcionário")]
        [RequiredIf("Tipo", 0, "Tipo Mudança", "Funcionário")]
        public virtual string NomeFuncionario { get; set; }
        [Display(Name = "Função")]
        [RequiredIf("Tipo", 3, "Tipo Mudança", "Função")]
        public virtual string NomeFuncao { get; set; }
        public int IdEmpresa { get; set; }
        public int IdDepartamento { get; set; }
        public int IdFuncao { get; set; }
        [Display(Name = "Horário Normal")]
        [RequiredIf("Tipohorario", 1, "Tipo Horário", "Normal")]
        public virtual string HorarioNormal { get; set; }
        [Display(Name = "Horário Flexível")]
        [RequiredIf("Tipohorario", 2, "Tipo Horário", "Flexível")]
        public virtual string HorarioFlexivel { get; set; }
        [Display(Name = "Horário Dinâmico")]
        [RequiredIf("Tipohorario", 3, "Tipo Horário", "Dinâmico")]
        public string HorarioDinamico { get; set; }
        public bool NaoRecalcular { get; set; }
        [TableHTMLAttribute("Turno Antigo", 2, true, ItensSearch.select, OrderType.none)]
        public string descricaohorario_ant { get; set; }
        [TableHTMLAttribute("Turno Novo", 4, true, ItensSearch.select, OrderType.none)]
        public string descricaohorario { get; set; }
        [TableHTMLAttribute("Tipo Horário", 3, true, ItensSearch.select, OrderType.none)]
        public string tipohorariodesc_ant { get; set; }
        [TableHTMLAttribute("Tipo Horário", 5, true, ItensSearch.select, OrderType.none)]
        public string tipohorariodesc { get; set; }
        public int? IdLancamentoLoteFuncionario { get; set; }

        public int? IdHorarioDinamico { get; set; }
        [Display(Name = "Índice Ciclo")]
        [RequiredIf("Tipohorario", 3, "Tipo Horário", "Dinâmico")]
        public int? CicloSequenciaIndice { get; set; }
    }
}
