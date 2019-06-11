using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Proxy
{
    public class pxyRelPontoWeb
    {
        [Display(Name = "Tipo")]
        public int TipoSelecao { get; set; }
        [Display(Name = "Empresas")]
        public List<pxyEmpresa> Empresas { get; set; }
        [Display(Name = "Empresa Relatório")]
        public List<PxyGridEmpresaRelatorioFunc> EmpresaRelatorio { get; set; }
        [Display(Name = "Departamentos")]
        public List<pxyDepartamento> Departamentos { get; set; }
        [Display(Name = "Funcionários")]
        public List<pxyFuncionario> Funcionarios { get; set; }
        [Display(Name = "Funcionários")]
        public List<pxyFuncionarioRelatorio> FuncionariosRelatorio { get; set; }
        public List<PxyHorarioMovel> HorarioMovelRel { get; set; }
       
        [Display(Name = "Departamentos")]
        public List<pxyDepartamentoRelatorio> DepartamentosRelatorio { get; set; }

        public string TipoArquivo { get; set; }
        [Display(Name="Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        public DateTime InicioPeriodo { get; set; }
        [Display(Name="Fim")]
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [MinDate("01/01/1760")]
        [DateGreaterThan("InicioPeriodo", "Início")]
        public DateTime FimPeriodo { get; set; }

        [Display(Name = "Ocorrências")]
        public List<pxyOcorrencias> Ocorrencias { get; set; }

        [Display(Name = "Justificativas")]
        public List<pxyJustificativas> Justificativas { get; set; }

        public List<Modelo.Ocorrencia> OcorrenciasAfastamento { get; set; }
        /// <summary>
        /// Ids dos registros selecionados na grid da página.
        /// </summary>
        public string idSelecionados { get; set; }
        /// <summary>
        /// Ids dos registros selecionados na segunda grig da página.
        /// </summary>
        public string idSelecionados2 { get; set; }
        /// <summary>
        /// Campo para quardar um valor de id quando necessário
        /// </summary>
        public int idRegistro { get; set; }
        /// <summary>
        /// Indica se a empresa junto com a permissão do usuário permite acesso a controle de contrato.
        /// </summary>
        public bool UtilizaControleContrato { get; set; }
        public int Intervalo { get; set; }

        public List<pxyRepHistoricoLocalAgrupado> lPxyRepHistoricoLocalAgrupado { get; set; }

        public string idSelecionadosOcorrencias { get; set; }
        [Display(Name = "Selecionar Ocorrência")]
        public bool bOcorrencia { get; set; }
        public bool Generico { get; set; }
    }

    public enum TipoArquivo
    {
        PDF,
        Imagem,
        Excel,
        Word
    }
}
