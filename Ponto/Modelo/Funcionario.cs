using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Funcionario : Modelo.ModeloBase
    {
        public Funcionario()
        {
            Historico = new List<FuncionarioHistorico>();
            Biometrias = new List<Biometria>();
        }

        /// <summary>
        /// Código do Funcionário
        /// </summary>
        /// [Display(Name = "Número Série")]
        [Display(Name = "Código")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Informar apenas números")]
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string Dscodigo { get; set; }

        /// <summary>
        /// Matrícula do Funcionário
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Matrícula")]
        [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Matrícula", 3, true, ItensSearch.text, OrderType.none)]
        public string Matricula { get; set; }

        /// <summary>
        /// Nome do Funcionário
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }

        /// <summary>
        /// Código da Folha
        /// </summary>
        [Display(Name = "Código Folha")]
        public Int32 Codigofolha { get; set; }

        /// <summary>
        /// Identificação da Empresa
        /// </summary>
        public int Idempresa { get; set; }
        public int Idempresa_Ant { get; set; }
        /// <summary>
        /// Identificação do Departamento
        /// </summary>
        public int Iddepartamento { get; set; }
        public int Iddepartamento_Ant { get; set; }
        /// <summary>
        /// Identificação da Função
        /// </summary>
        public int Idfuncao { get; set; }
        public int? IdAlocacao { get; set; }
        public int? IdTipoVinculo { get; set; }
        public int Idfuncao_Ant { get; set; }
        /// <summary>
        /// Identificação do Horário
        /// </summary>
        public int Idhorario { get; set; }

        /// <summary>
        /// Tipo do horário
        /// </summary>
        [Display(Name = "Tipo Horário")]
        public Int16 Tipohorario { get; set; }

        /// <summary>
        /// Número da Carteira do Funcionário
        /// </summary>
        [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Carteira", 7, true, ItensSearch.text, OrderType.none)]
        public string Carteira { get; set; }

        /// <summary>
        /// Data de Admissão do Funcionário
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Data Admissão")]
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dataadmissao { get; set; }

        [TableHTMLAttribute("Admissão", 9, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string DataadmissaoDesc
        {
            get
            {
                return Dataadmissao == null ? "" : Dataadmissao.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        public DateTime? Dataadmissao_Ant { get; set; }

        /// <summary>
        /// Data de Demissão do Funcionário
        /// </summary>
        [Display(Name = "Data Demissão")]
        [DataType(DataType.Date, ErrorMessage = "Data inválida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Datademissao { get; set; }

        [TableHTMLAttribute("Demissão", 10, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string DatademissaoDesc
        {
            get
            {
                return Datademissao == null ? "" : Datademissao.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        public DateTime? Datademissao_Ant { get; set; }

        /// <summary>
        /// Salário do Funcionário
        /// </summary>
        [Display(Name = "Salário")]
        public decimal Salario { get; set; }

        /// <summary>
        /// Flag de funcionário ativo ou não
        /// </summary>
        public Int16 Funcionarioativo { get; set; }

        [Display(Name = "Funcionário Ativo")]
        public virtual bool bFuncionarioativo
        {
            get { return Funcionarioativo == 1 ? true : false; }
            set { Funcionarioativo = value ? (short)1 : (short)0; }
        }

        [TableHTMLAttribute("Situação", 11, true, ItensSearch.text, OrderType.none)]
        public string FuncionarioAtivoStr
        {
            get
            {
                return bFuncionarioativo == true ? "Sim" : "Não";
            }
        }

        
        [Display(Name = "Data Inativação")]
        public DateTime? DataInativacao { get; set; }
        public DateTime? DataInativacao_Ant { get; set; }

        [TableHTMLAttribute("Data Inativação", 12, true, ItensSearch.text, OrderType.none, ColumnType.data)]
        public string DataInativacaoDesc
        {
            get
            {
                return DataInativacao == null ? "" : DataInativacao.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        /// <summary>
        /// Flag que marca se o funcionário vai ou não entrar no BH
        /// </summary>
        public Int16 Naoentrarbanco { get; set; }

        [Display(Name = "Não Entrar no Banco de Horas")]
        public virtual bool bNaoentrarbanco
        {
            get { return Naoentrarbanco == 1 ? true : false; }
            set { Naoentrarbanco = value ? (short)1 : (short)0; }
        }
        public Int16 Naoentrarbanco_Ant { get; set; }

        /// <summary>
        /// Flag que marca se o funcionário vai ou não entrar na Compensação
        /// </summary>
        public Int16 Naoentrarcompensacao_Ant { get; set; }
        public Int16 Naoentrarcompensacao { get; set; }

        [Display(Name = "Não Entrar na Compensação")]
        public virtual bool bNaoentrarcompensacao
        {
            get { return Naoentrarcompensacao == 1 ? true : false; }
            set { Naoentrarcompensacao = value ? (short)1 : (short)0; }
        }
        public Int16 Excluido { get; set; }

        /// <summary>
        /// Observação do Funcionário
        /// </summary>
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        [Display(Name = "Observação")]
        public string Campoobservacao { get; set; }

        /// <summary>
        /// Foto do Funcionário
        /// </summary>
        public string Foto { get; set; }

        /// <summary>
        /// Número do PIS do Funcionário
        /// </summary>
        [StringLength(20, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Pis { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Horário")]
        public string Horario { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [TableHTMLAttribute("Empresa", 5, true, ItensSearch.text, OrderType.none)]
        public string Empresa { get; set; }

        [Display(Name = "Senha Relógio")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name = "Crachá proximidade")]
        [Range(0, double.MaxValue, ErrorMessage = "Campo inválido")]
        public Int64? RFID { get; set; }
        public Int64? RFID_Ant { get; set; }

        /// <summary>
        /// Valor anterior da variável Funcionárioativo
        /// </summary>
        public Int16 Funcionarioativo_Ant { get; set; }
        /// <summary>
        /// Lista de Funcionário Histórico
        /// </summary>
        public List<Modelo.FuncionarioHistorico> Historico { get; set; }

        public string ToleranciaEntrada { get; set; }
        public string ToleranciaSaida { get; set; }
        public int? QuantidadeTickets { get; set; }
        public int? TipoTickets { get; set; }

        [Display(Name = "Senha APP")]
        public String Mob_Senha { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "CPF")]
        [TableHTMLAttribute("CPF", 8, true, ItensSearch.text, OrderType.none)]
        public String CPF { get; set; }

        [TableHTMLAttribute("Jornada", 4, true, ItensSearch.text, OrderType.none)]
        public String Jornada { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [TableHTMLAttribute("Departamento", 6, true, ItensSearch.text, OrderType.none)]
        public String Departamento { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Função")]
        public String Funcao { get; set; }

        [Display(Name = "Alocação")]
        public String Alocacao { get; set; }

        [Display(Name = "Endereço Alocação")]
        public String AlocacaoEndereco { get; set; }

        [Display(Name = "Tipo Vínculo")]
        public String TipoVinculo { get; set; }

        public bool ImportarMarcacoes { get; set; }

        public bool NaoRecalcular { get; set; }

        public bool Selecionado { get; set; }

        public string nomeCodigo { get; set; }

        [Display(Name = "Mão de Obra")]
        public int? TipoMaoObra { get; set; }

        [Display(Name = "Tipo do Crachá")]
        public int? TipoCracha { get; set; }

        private Int32? _IdCw_Usuario;
        /// <summary>
        /// FK para a funcionalidade de Usuário Supervisor
        /// </summary>
        public Int32? IdCw_Usuario
        {
            get { return _IdCw_Usuario; }
            set { _IdCw_Usuario = value; }
        }

        [Display(Name = "Supervisor")]
        public string Supervisor { get; set; }

        private Int32? _IdPessoaSupervisor;
        /// <summary>
        /// FK para a funcionalidade de Pessoa Supervisor
        /// </summary>
        public Int32? IdPessoaSupervisor
        {
            get { return _IdPessoaSupervisor; }
            set { _IdPessoaSupervisor = value; }
        }

        [Display(Name = "Pessoa Supervisor")]
        public string PessoaSupervisor { get; set; }

        [Display(Name = "Contrato")]
        public string Contrato { get; set; }
        public Pessoa ObjPessoaSupervisor { get; set; }

        [Display(Name = "Utiliza Registrador")]
        public bool utilizaregistrador { get; set; }

        [Display(Name = "Utiliza App Pontofopag")]
        public bool UtilizaAppPontofopag { get; set; }

        [Display(Name = "Utiliza Reconhecimento Facial")]
        public bool UtilizaReconhecimentoFacialApp { get; set; }

        [Display(Name = "Utiliza Web App Pontofopag")]
        public bool UtilizaWebAppPontofopag { get; set; }

        [Display(Name = "Utiliza Reconhecimento Facial")]
        public bool UtilizaReconhecimentoFacialWebApp { get; set; }

        /// <summary>
        /// Conexao do banco onde o funcionario esta cadastrado
        /// </summary>
        public string Conexao { get; set; }
        public int? idIntegracao { get; set; }
        public DateTime? DataUltimoFechamento { get; set; }
        public DateTime? DataUltimoFechamentoBH { get; set; }

        public int? IdIntegracaoPainel { get; set; }
        public string Email { get; set; }
        [Display(Name = "Biometria")]
        public List<Modelo.Biometria> Biometrias { get; set; }

        public List<FuncionarioRFID> FuncionarioRFID { get; set; }

        public int? diaInicioFechamento { get; set; }
        public int? diaFimfechamento { get; set; }

        public List<Horario> HorariosFuncionario { get; set; }

        [Display(Name = "Horário Dinâmico")]
        [RequiredIf("Tipohorario", 3, "Tipo Horário", "Dinâmico")]
        public string HorarioDinamico { get; set; }
        public int? IdHorarioDinamico { get; set; }
        [Display(Name = "Índice Ciclo")]
        [RequiredIf("Tipohorario", 3, "Tipo Horário", "Dinâmico")]
        public int? CicloSequenciaIndice { get; set; }

        public string OpcaoSMSEmailSenha { get; set; }
    }
}
