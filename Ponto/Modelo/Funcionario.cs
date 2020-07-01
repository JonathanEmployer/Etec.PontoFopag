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
        /// C�digo do Funcion�rio
        /// </summary>
        /// [Display(Name = "N�mero S�rie")]
        [Display(Name = "C�digo")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Informar apenas n�meros")]
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string Dscodigo { get; set; }

        /// <summary>
        /// Matr�cula do Funcion�rio
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [Display(Name = "Matr�cula")]
        [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Matr�cula", 3, true, ItensSearch.text, OrderType.none)]
        public string Matricula { get; set; }

        /// <summary>
        /// Nome do Funcion�rio
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [StringLength(100, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Nome", 2, true, ItensSearch.text, OrderType.asc)]
        public string Nome { get; set; }

        /// <summary>
        /// C�digo da Folha
        /// </summary>
        [Display(Name = "C�digo Folha")]
        public Int32 Codigofolha { get; set; }

        /// <summary>
        /// Identifica��o da Empresa
        /// </summary>
        public int Idempresa { get; set; }
        public int Idempresa_Ant { get; set; }
        /// <summary>
        /// Identifica��o do Departamento
        /// </summary>
        public int Iddepartamento { get; set; }
        public int Iddepartamento_Ant { get; set; }
        /// <summary>
        /// Identifica��o da Fun��o
        /// </summary>
        public int Idfuncao { get; set; }
        public int? IdAlocacao { get; set; }
        public int? IdTipoVinculo { get; set; }
        public int Idfuncao_Ant { get; set; }
        /// <summary>
        /// Identifica��o do Hor�rio
        /// </summary>
        public int Idhorario { get; set; }

        /// <summary>
        /// Tipo do hor�rio
        /// </summary>
        [Display(Name = "Tipo Hor�rio")]
        public Int16 Tipohorario { get; set; }

        /// <summary>
        /// N�mero da Carteira do Funcion�rio
        /// </summary>
        [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Carteira", 7, true, ItensSearch.text, OrderType.none)]
        public string Carteira { get; set; }

        /// <summary>
        /// Data de Admiss�o do Funcion�rio
        /// </summary>
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [Display(Name = "Data Admiss�o")]
        [DataType(DataType.Date, ErrorMessage = "Data inv�lida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dataadmissao { get; set; }

        [TableHTMLAttribute("Admiss�o", 9, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string DataadmissaoDesc
        {
            get
            {
                return Dataadmissao == null ? "" : Dataadmissao.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        public DateTime? Dataadmissao_Ant { get; set; }

        /// <summary>
        /// Data de Demiss�o do Funcion�rio
        /// </summary>
        [Display(Name = "Data Demiss�o")]
        [DataType(DataType.Date, ErrorMessage = "Data inv�lida")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Datademissao { get; set; }

        [TableHTMLAttribute("Demiss�o", 10, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string DatademissaoDesc
        {
            get
            {
                return Datademissao == null ? "" : Datademissao.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }
        public DateTime? Datademissao_Ant { get; set; }

        /// <summary>
        /// Sal�rio do Funcion�rio
        /// </summary>
        [Display(Name = "Sal�rio")]
        public decimal Salario { get; set; }

        /// <summary>
        /// Flag de funcion�rio ativo ou n�o
        /// </summary>
        public Int16 Funcionarioativo { get; set; }

        [Display(Name = "Funcion�rio Ativo")]
        public virtual bool bFuncionarioativo
        {
            get { return Funcionarioativo == 1 ? true : false; }
            set { Funcionarioativo = value ? (short)1 : (short)0; }
        }

        [TableHTMLAttribute("Situa��o", 11, true, ItensSearch.text, OrderType.none)]
        public string FuncionarioAtivoStr
        {
            get
            {
                return bFuncionarioativo == true ? "Sim" : "N�o";
            }
        }

        
        [Display(Name = "Data Inativa��o")]
        public DateTime? DataInativacao { get; set; }
        public DateTime? DataInativacao_Ant { get; set; }

        [TableHTMLAttribute("Data Inativa��o", 12, true, ItensSearch.text, OrderType.none, ColumnType.data)]
        public string DataInativacaoDesc
        {
            get
            {
                return DataInativacao == null ? "" : DataInativacao.GetValueOrDefault().ToString("dd/MM/yyyy");
            }
        }

        /// <summary>
        /// Flag que marca se o funcion�rio vai ou n�o entrar no BH
        /// </summary>
        public Int16 Naoentrarbanco { get; set; }

        [Display(Name = "N�o Entrar no Banco de Horas")]
        public virtual bool bNaoentrarbanco
        {
            get { return Naoentrarbanco == 1 ? true : false; }
            set { Naoentrarbanco = value ? (short)1 : (short)0; }
        }
        public Int16 Naoentrarbanco_Ant { get; set; }

        /// <summary>
        /// Flag que marca se o funcion�rio vai ou n�o entrar na Compensa��o
        /// </summary>
        public Int16 Naoentrarcompensacao_Ant { get; set; }
        public Int16 Naoentrarcompensacao { get; set; }

        [Display(Name = "N�o Entrar na Compensa��o")]
        public virtual bool bNaoentrarcompensacao
        {
            get { return Naoentrarcompensacao == 1 ? true : false; }
            set { Naoentrarcompensacao = value ? (short)1 : (short)0; }
        }
        public Int16 Excluido { get; set; }

        /// <summary>
        /// Observa��o do Funcion�rio
        /// </summary>
        [StringLength(100, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [Display(Name = "Observa��o")]
        public string Campoobservacao { get; set; }

        /// <summary>
        /// Foto do Funcion�rio
        /// </summary>
        public string Foto { get; set; }

        /// <summary>
        /// N�mero do PIS do Funcion�rio
        /// </summary>
        [StringLength(20, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        public string Pis { get; set; }

        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [Display(Name = "Hor�rio")]
        public string Horario { get; set; }

        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [TableHTMLAttribute("Empresa", 5, true, ItensSearch.text, OrderType.none)]
        public string Empresa { get; set; }

        [Display(Name = "Senha Rel�gio")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name = "Crach� proximidade")]
        [Range(0, double.MaxValue, ErrorMessage = "Campo inv�lido")]
        public Int64? RFID { get; set; }
        public Int64? RFID_Ant { get; set; }

        /// <summary>
        /// Valor anterior da vari�vel Funcion�rioativo
        /// </summary>
        public Int16 Funcionarioativo_Ant { get; set; }
        /// <summary>
        /// Lista de Funcion�rio Hist�rico
        /// </summary>
        public List<Modelo.FuncionarioHistorico> Historico { get; set; }

        public string ToleranciaEntrada { get; set; }
        public string ToleranciaSaida { get; set; }
        public int? QuantidadeTickets { get; set; }
        public int? TipoTickets { get; set; }

        [Display(Name = "Senha APP")]
        public String Mob_Senha { get; set; }

        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [Display(Name = "CPF")]
        [TableHTMLAttribute("CPF", 8, true, ItensSearch.text, OrderType.none)]
        public String CPF { get; set; }

        [TableHTMLAttribute("Jornada", 4, true, ItensSearch.text, OrderType.none)]
        public String Jornada { get; set; }

        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [TableHTMLAttribute("Departamento", 6, true, ItensSearch.text, OrderType.none)]
        public String Departamento { get; set; }

        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [Display(Name = "Fun��o")]
        public String Funcao { get; set; }

        [Display(Name = "Aloca��o")]
        public String Alocacao { get; set; }

        [Display(Name = "Endere�o Aloca��o")]
        public String AlocacaoEndereco { get; set; }

        [Display(Name = "Tipo V�nculo")]
        public String TipoVinculo { get; set; }

        public bool ImportarMarcacoes { get; set; }

        public bool NaoRecalcular { get; set; }

        public bool Selecionado { get; set; }

        public string nomeCodigo { get; set; }

        [Display(Name = "M�o de Obra")]
        public int? TipoMaoObra { get; set; }

        [Display(Name = "Tipo do Crach�")]
        public int? TipoCracha { get; set; }

        private Int32? _IdCw_Usuario;
        /// <summary>
        /// FK para a funcionalidade de Usu�rio Supervisor
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

        [Display(Name = "Hor�rio Din�mico")]
        [RequiredIf("Tipohorario", 3, "Tipo Hor�rio", "Din�mico")]
        public string HorarioDinamico { get; set; }
        public int? IdHorarioDinamico { get; set; }
        [Display(Name = "�ndice Ciclo")]
        [RequiredIf("Tipohorario", 3, "Tipo Hor�rio", "Din�mico")]
        public int? CicloSequenciaIndice { get; set; }

        public string OpcaoSMSEmailSenha { get; set; }
    }
}
