using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public enum Acao
    {
        Incluir = 1,
        Alterar = 2,
        Excluir = 3,
        Consultar = 4
    }

    public enum NivelType
    {
        Supervisor = 0,
        Gerente = 1,
        Operador = 2
    }

    public enum BoolType
    {
        Sim = 1,
        Não = 0
    }

    public class ConfiguracoesGerais: Modelo.ModeloBase
    {
        [Display(Name = "Dia inicial")]
        [Range(0,31, ErrorMessage = "O dia de fechamento deve possuir valores entre 0 e 31")]
        public int DataInicial { get; set; }

        [Display(Name = "Dia final")]
        [Range(0, 31, ErrorMessage = "O dia de fechamento deve possuir valores entre 0 e 31")]
        public int DataFinal { get; set; }
        
        [Display(Name = "Mudar período imediatamente após data final")]
        public bool MudarPeriodoAposDataFinal { get; set; }

        [Display(Name = "Habilitar controle In Itinere")]
        public bool controleInItinere { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [StringLength(200, ErrorMessage = "Número máximo de caracteres: {1}")]
        [DataType(DataType.Password)]
        public string SenhaEmail { get; set; }

        [Display(Name = "SMTP")]
        [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string SMTP { get; set; }

        [Display(Name = "SSL")]
        public bool SSL { get; set; }

        [Display(Name = "Porta")]
        public int? Porta { get; set; }

        public Proxy.pxyRetornoEmail RetornoEmail { get; set; }
        public bool TestarEmail { get; set; }
        public bool Salvando { get; set; }

        public string LogoEmpresa { get; set; }

        [Display(Name = "Integrar Salário Funcionário?")]
        public bool IntegrarSalarioFuncionario { get; set; }
        [Display(Name = "Horário padrão para integração de Funcionários")]
        public string Horario { get; set; }
        public int? IdHorarioPadraoFunc { get; set; }
        [Display(Name = "Tipo Horário")]
        public int TipoHorarioPadraoFunc { get; set; }
    }

    public enum TipoSaldo
    {
        Credito = 0,
        Debito = 1
    }

    public struct DadosFechamento
    {
        public int idFuncionario;
        public int saldoBH;
        /// <summary>
        /// 0 = credito/ 1 = debito
        /// </summary>
        public int TipoSaldoBH;
    }

    public struct BuscaMarcacaoFunc
    {
        public int idFuncionario;
        public int idMarcacao;
        public DateTime data;
    }

    public class PeriodoFechamento
    {
        public int DiaFechamentoInicial { get; set; }
        public int DiaFechamentoFinal { get; set; }
        public DateTime DataFechamentoInicial { get; set; }
        public DateTime DataFechamentoFinal { get; set; }

        public string DataFechamentoInicialStr { get { return DataFechamentoInicial.ToShortDateString(); } }
        public string DataFechamentoFinalStr { get { return DataFechamentoFinal.ToShortDateString(); } }
    }
}
