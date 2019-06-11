using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Parametros : Modelo.ModeloBase, ICloneable
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Descrição do Parâmetro
        /// </summary>
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.none)]
        public string Descricao { get; set; }
        /// <summary>
        /// Início Adicional do Período Noturno
        /// </summary>
        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        [TableHTMLAttribute("Início", 3, true, ItensSearch.text, OrderType.none)]
        public string InicioAdNoturno { get; set; }
        /// <summary>
        /// Fim Adicional do Período Noturno
        /// </summary>
        [Display(Name = "Fim")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        [TableHTMLAttribute("Fim", 4, true, ItensSearch.text, OrderType.none)]
        public string FimAdNoturno { get; set; }
        
        /// <summary>
        /// Tolerância Entrada Hora Extra
        /// </summary>
        [Display(Name = "Entrada")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        [TableHTMLAttribute("Tolerância Ex. Ent.", 7, true, ItensSearch.text, OrderType.none)]
        public string THoraExtraEntrada { get; set; }
        /// <summary>
        /// Tolerância Intervalo Hora Extra
        /// </summary>
        [Display(Name = "Intervalo")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        [TableHTMLAttribute("Tolerância Ex. Interv.", 8, true, ItensSearch.text, OrderType.none)]
        public string TIntervaloExtra { get; set; }
        /// <summary>
        /// Tolerância Saída Hora Extra
        /// </summary>
        [Display(Name = "Saida")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        [TableHTMLAttribute("Tolerância Ex. Saí.", 9, true, ItensSearch.text, OrderType.none)]
        public string THoraExtraSaida { get; set; }
        /// <summary>
        /// Limite Tolerância Hora Extra
        /// </summary>
        [Display(Name = "Limite")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        [TableHTMLAttribute("Limite Tolerância Ex.", 10, true, ItensSearch.text, OrderType.none)]
        public string THoraExtra { get; set; }
        /// <summary>
        /// Tolerância Entrada Hora Falta
        /// </summary>
        [Display(Name = "Entrada")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        [TableHTMLAttribute("Tolerância Fal. Ent.", 11, true, ItensSearch.text, OrderType.none)]
        public string THoraFaltaEntrada { get; set; }
        /// <summary>
        /// Tolerância Intervalo Falta
        /// </summary>
        [Display(Name = "Intervalo")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        [TableHTMLAttribute("Tolerância Fal. Interv.", 12, true, ItensSearch.text, OrderType.none)]
        public string TIntervaloFalta { get; set; }
        /// <summary>
        /// Tolerância Saída Hora Falta
        /// </summary>
        [Display(Name = "Saída")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        [TableHTMLAttribute("Tolerância Fal. Saí.", 13, true, ItensSearch.text, OrderType.none)]
        public string THoraFaltaSaida { get; set; }
        /// <summary>
        /// Tolerâncias: Faltas
        /// </summary>
        [Display(Name = "Limite")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inválida!")]
        [TableHTMLAttribute("Limite Tolerância Fal.", 14, true, ItensSearch.text, OrderType.none)]
        public string THoraFalta { get; set; }

        /// <summary>
        /// Tipo Compactador: 0 - WinZIP, 1 - WinRAR, 2 - BraZIP, 3 - FilZIP
        /// </summary>
        public Int32 TipoCompactador { get; set; }
        /// <summary>
        /// Nome do Arquivo de Backup: 0 - Data, 1 - Dia da Semana
        /// </summary>
        public Int32 ArquivoBackup { get; set; }
        /// <summary>
        /// Variável do Flag que marca se vai fazer Backup na Entrada ou não
        /// </summary>
        public Int16 FazerBackupEntrada { get; set; }
        /// <summary>
        /// Variável do Flag que marca se vai fazer Backup na Saida ou não
        /// </summary>
        public Int16 FazerBackupSaida { get; set; }
        /// <summary>
        /// Variável do Flag que marca se vai verificar Bilhetes para Importar ou não
        /// </summary>
        public Int16 VerificarBilhetes { get; set; }
        [Display(Name = "Verificar se há bilhetes para Importar")]
        public bool VerificarBilhetesBool
        {
            get { return VerificarBilhetes == 1 ? true : false; }
            set { VerificarBilhetes = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Variável do Flag que marca se vai Imprimi Faltas em Dias ou não
        /// </summary>
        public Int16 FaltaEmDias { get; set; }
        /// <summary>
        /// Variável do Flag que marca  se vai Imprimir Responsável ou não
        /// </summary>
        public Int16 ImprimeResponsavel { get; set; }
        [Display(Name = "Imprimir Responsável")]
        public bool ImprimeResponsavelBool
        {
            get { return ImprimeResponsavel == 1 ? true : false; }
            set { ImprimeResponsavel = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Imprimir Responsável", 21, true, ItensSearch.text, OrderType.none)]
        public string ImprimeResponsavelStr
        {
            get
            {
                return TipoHoraExtraFaltaBool == true ? "Sim" : "Não";
            }
        }
        /// <summary>
        /// Variável do Flag que marca se vai Imprimir Observação ou não
        /// </summary>
        public Int16 ImprimeObservacao { get; set; }
        [Display(Name = "Imprimir Observação")]
        public bool ImprimeObservacaoBool
        {
            get { return ImprimeObservacao == 1 ? true : false; }
            set { ImprimeObservacao = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Imprimir Observação", 16, true, ItensSearch.text, OrderType.none)]
        public string ImprimeObservacaoStr
        {
            get
            {
                return TipoHoraExtraFaltaBool == true ? "Sim" : "Não";
            }
        }
        /// <summary>
        /// Variável do Flag que marca se vai Separar Extra / Falta ou não
        /// </summary>
        public Int16 TipoHoraExtraFalta { get; set; }
        [Display(Name = "Separa Extra / Falta")]
        public bool TipoHoraExtraFaltaBool
        {
            get { return TipoHoraExtraFalta == 1 ? true : false; }
            set { TipoHoraExtraFalta = value ? (Int16)1 : (Int16)0; }
        }

        [TableHTMLAttribute("Separa Extra Falta", 15, true, ItensSearch.text, OrderType.none)]
        public string TipoHoraExtraFaltaStr
        {
            get
            {
                return TipoHoraExtraFaltaBool == true ? "Sim" : "Não";
            }
        }
        /// <summary>
        /// Variável do Flag que marca  se vai Imprimir o Número do Relógio ou não
        /// </summary>
        public Int16 ImprimirNumRelogio { get; set; }
        /// <summary>
        /// Observação do Parâmetro
        /// </summary>
        [Display(Name = "Observação")]
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Observação", 17, true, ItensSearch.text, OrderType.none)]
        public string CampoObservacao { get; set; }
        public int ExportarValorZerado { get; set; }
        [Display(Name = "Não Exporta Saldo Zerado")]
        public bool ExportarValorZeradoBool
        {
            get { return ExportarValorZerado == 1 ? true : false; }
            set { ExportarValorZerado = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Exporta Saldo Zerado", 20, true, ItensSearch.text, OrderType.none)]
        public string ExportaValorZeradoStr
        {
            get
            {
                return TipoHoraExtraFaltaBool == true ? "Sim" : "Não";
            }
        }

        public int DiaFechamentoInicial { get; set; }
        public int DiaFechamentoFinal { get; set; }
	    public bool MudaPeriodoImediatamento { get; set; }
        [Display(Name = "Habilitar Controle InItinere")]
        public bool HabilitarControleInItinere { get; set; }
        public bool IntegrarSalarioFunc { get; set; }
        

        public string LogoEmpresa { get; set; }

        public bool bConsiderarHEFeriadoPHoraNoturna { get; set; }

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
        public int Porta { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        [Display(Name = "Percentual")]
        [TableHTMLAttribute("Percentual", 5, true, ItensSearch.text, OrderType.none)]
        public decimal? PercAdicNoturno { get; set; }

        [Display(Name = "Redução Hora Noturna")]
        [TableHTMLAttribute("Redução", 6, true, ItensSearch.text, OrderType.none)]
        public string ReducaoHoraNoturna { get; set; }
        public bool BloqueiaDadosIntegrados { get; set; }
        [Display(Name = "Horário padrão para integração de Funcionários")]
        public string Horario { get; set; }
        public int? IdHorarioPadraoFunc { get; set; }
        [Display(Name = "Tipo Horário")]
        public int TipoHorarioPadraoFunc { get; set; }
        [Display(Name = "Tolerância Adicional Noturno")]
        [Range(0, 59, ErrorMessage = "Tolerância Adicional Noturno inválido! Ele deve ser um número entre 0 e 59.")]
        [StringLength(2, ErrorMessage = "Tolerância Adicional Noturno inválido! Ele deve ser um número entre 0 e 59.")]
        [Required(ErrorMessage = "O campo Tolerância Adicional Noturno é obrigatório.")]
        public string ToleranciaAdicionalNoturnoStr {
            get {
                return ToleranciaAdicionalNoturno.ToString();
            }
            set {
                ToleranciaAdicionalNoturno = int.Parse(value);
            }
        }
        public int ToleranciaAdicionalNoturno { get; set; }
        [Required(ErrorMessage = "O momento de geração do Intervalo Automático é obrigatório")]
        [Display(Name = "Momento Intervalo Automático")]
        [Range(0, 1, ErrorMessage = "O valor selecionado não é válido para o momento da geração do Intervalo Automático")]
        public Int16 MomentoPreAssinalado { get; set; }

        /// <summary>
        /// Separar Horas Trabalhadas Noturnas / Horas Extras Noturnas
        /// </summary>
        [Display(Name = "Separar Trabalhadas Not. / Extra Not.")]
        public bool Flg_Separar_Trabalhadas_Noturna_Extras_Noturna { get; set; }

    }
}
