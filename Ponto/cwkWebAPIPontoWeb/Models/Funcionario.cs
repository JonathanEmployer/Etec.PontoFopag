using cwkWebAPIPontoWeb.Areas.HelpPage.ModelDescriptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Objeto com os dados do Funcionário
    /// </summary>
    [ModelName("FuncionarioIntegracao")]
    public class Funcionario
    {
        /// <summary>
        /// Código Funcionário.
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        public int Codigo { get; set; }
        /// <summary>
        /// Nome do Funcionário
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Nome { get; set; }
        /// <summary>
        /// Código da Folha de Pagamento do Funcionário
        /// </summary>
        public int Codigofolha { get; set; }
        /// <summary>
        /// Matrícula do Funcionário
        /// </summary>
        [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Matricula { get; set; }
        /// <summary>
        /// Número Carteira de Trabalho do Funcionário
        /// </summary>
        [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Carteira { get; set; }
        /// <summary>
        /// Número do PIS do Funcionário
        /// </summary>
        [StringLength(20, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Pis { get; set; }
        /// <summary>
        /// CPF do Funcionário
        /// </summary>
        public string CPF { get; set; }
        /// <summary>
        /// Salário do  Funcionário
        /// </summary>
        public decimal Salario { get; set; }
        /// <summary>
        /// Senha do Relógio
        /// </summary>
        public string SenhaRelogio { get; set; }
        /// <summary>
        /// Data de Admissão do Funcionário
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        public DateTime? Dataadmissao { get; set; }
        /// <summary>
        /// Data de Demissão do Funcionário
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        public DateTime? Datademissao { get; set; }
        /// <summary>
        /// Data de Inativação do funcionário
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        public DateTime? DataInativacao { get; set; }

        /// <summary>
        /// CNPJ da Empresa onde o Funcionário esta Registrado
        /// </summary>
        public Int64 DocumentoEmpresa { get; set; }
        /// <summary>
        /// ID de integração do Departamento do Funcionário
        /// </summary>
        public int? IdIntegracaoDepartamento { get; set; }
        /// <summary>
        /// ID de integração da Função do Funcionário
        /// </summary>
        public int? IdIntegracaoFuncao { get; set; }
        /// <summary>
        /// Descrição da Função do Funcionário
        /// </summary>
        public string DescricaoFuncao { get; set; }
        /// <summary>
        /// Campo obsoleto, será removido em versões futuras, não utilizar, caso esteja sendo enviado na requisição removê-lo.
        /// Campo substituído pela DataInativacao
        /// </summary>
        public bool FuncionarioAtivo { get; set; }
        /// <summary>
        /// Observação sobre o Funcionário
        /// </summary>
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string CampoObservacao { get; set; }
        /// <summary>
        /// Foto do Funcionário (imagem convertida para base64)
        /// </summary>
        public string Foto { get; set; }
        public int IdIntegracao { get; set; }
        public string IdIntegracaoPessoaSupervisor { get; set; }

        public int? CodTipoVinculo { get; set; }
        public int? IdintegracaoContrato { get; set; }

        public bool FuncionarioExcluido { get; set; }
        public int? TipoMaoObra { get; set; }
        public Models.Pessoa PessoaSupervisor { get; set; }
        public string Celular { get; set; }
    }

}