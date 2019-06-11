using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class Parametros : Modelo.ModeloBase, ICloneable
    {
        [TableHTMLAttribute("C�digo", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Descri��o do Par�metro
        /// </summary>
        [Display(Name = "Descri��o")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Descri��o", 2, true, ItensSearch.text, OrderType.none)]
        public string Descricao { get; set; }
        /// <summary>
        /// In�cio Adicional do Per�odo Noturno
        /// </summary>
        [Display(Name = "In�cio")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inv�lida!")]
        [TableHTMLAttribute("In�cio", 3, true, ItensSearch.text, OrderType.none)]
        public string InicioAdNoturno { get; set; }
        /// <summary>
        /// Fim Adicional do Per�odo Noturno
        /// </summary>
        [Display(Name = "Fim")]
        [Required(ErrorMessage = "Campo Obrigat�rio")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inv�lida!")]
        [TableHTMLAttribute("Fim", 4, true, ItensSearch.text, OrderType.none)]
        public string FimAdNoturno { get; set; }
        
        /// <summary>
        /// Toler�ncia Entrada Hora Extra
        /// </summary>
        [Display(Name = "Entrada")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inv�lida!")]
        [TableHTMLAttribute("Toler�ncia Ex. Ent.", 7, true, ItensSearch.text, OrderType.none)]
        public string THoraExtraEntrada { get; set; }
        /// <summary>
        /// Toler�ncia Intervalo Hora Extra
        /// </summary>
        [Display(Name = "Intervalo")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inv�lida!")]
        [TableHTMLAttribute("Toler�ncia Ex. Interv.", 8, true, ItensSearch.text, OrderType.none)]
        public string TIntervaloExtra { get; set; }
        /// <summary>
        /// Toler�ncia Sa�da Hora Extra
        /// </summary>
        [Display(Name = "Saida")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inv�lida!")]
        [TableHTMLAttribute("Toler�ncia Ex. Sa�.", 9, true, ItensSearch.text, OrderType.none)]
        public string THoraExtraSaida { get; set; }
        /// <summary>
        /// Limite Toler�ncia Hora Extra
        /// </summary>
        [Display(Name = "Limite")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inv�lida!")]
        [TableHTMLAttribute("Limite Toler�ncia Ex.", 10, true, ItensSearch.text, OrderType.none)]
        public string THoraExtra { get; set; }
        /// <summary>
        /// Toler�ncia Entrada Hora Falta
        /// </summary>
        [Display(Name = "Entrada")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inv�lida!")]
        [TableHTMLAttribute("Toler�ncia Fal. Ent.", 11, true, ItensSearch.text, OrderType.none)]
        public string THoraFaltaEntrada { get; set; }
        /// <summary>
        /// Toler�ncia Intervalo Falta
        /// </summary>
        [Display(Name = "Intervalo")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inv�lida!")]
        [TableHTMLAttribute("Toler�ncia Fal. Interv.", 12, true, ItensSearch.text, OrderType.none)]
        public string TIntervaloFalta { get; set; }
        /// <summary>
        /// Toler�ncia Sa�da Hora Falta
        /// </summary>
        [Display(Name = "Sa�da")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inv�lida!")]
        [TableHTMLAttribute("Toler�ncia Fal. Sa�.", 13, true, ItensSearch.text, OrderType.none)]
        public string THoraFaltaSaida { get; set; }
        /// <summary>
        /// Toler�ncias: Faltas
        /// </summary>
        [Display(Name = "Limite")]
        [RegularExpression(@"^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "Hora inv�lida!")]
        [TableHTMLAttribute("Limite Toler�ncia Fal.", 14, true, ItensSearch.text, OrderType.none)]
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
        /// Vari�vel do Flag que marca se vai fazer Backup na Entrada ou n�o
        /// </summary>
        public Int16 FazerBackupEntrada { get; set; }
        /// <summary>
        /// Vari�vel do Flag que marca se vai fazer Backup na Saida ou n�o
        /// </summary>
        public Int16 FazerBackupSaida { get; set; }
        /// <summary>
        /// Vari�vel do Flag que marca se vai verificar Bilhetes para Importar ou n�o
        /// </summary>
        public Int16 VerificarBilhetes { get; set; }
        [Display(Name = "Verificar se h� bilhetes para Importar")]
        public bool VerificarBilhetesBool
        {
            get { return VerificarBilhetes == 1 ? true : false; }
            set { VerificarBilhetes = value ? (Int16)1 : (Int16)0; }
        }
        /// <summary>
        /// Vari�vel do Flag que marca se vai Imprimi Faltas em Dias ou n�o
        /// </summary>
        public Int16 FaltaEmDias { get; set; }
        /// <summary>
        /// Vari�vel do Flag que marca  se vai Imprimir Respons�vel ou n�o
        /// </summary>
        public Int16 ImprimeResponsavel { get; set; }
        [Display(Name = "Imprimir Respons�vel")]
        public bool ImprimeResponsavelBool
        {
            get { return ImprimeResponsavel == 1 ? true : false; }
            set { ImprimeResponsavel = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Imprimir Respons�vel", 21, true, ItensSearch.text, OrderType.none)]
        public string ImprimeResponsavelStr
        {
            get
            {
                return TipoHoraExtraFaltaBool == true ? "Sim" : "N�o";
            }
        }
        /// <summary>
        /// Vari�vel do Flag que marca se vai Imprimir Observa��o ou n�o
        /// </summary>
        public Int16 ImprimeObservacao { get; set; }
        [Display(Name = "Imprimir Observa��o")]
        public bool ImprimeObservacaoBool
        {
            get { return ImprimeObservacao == 1 ? true : false; }
            set { ImprimeObservacao = value ? (Int16)1 : (Int16)0; }
        }
        [TableHTMLAttribute("Imprimir Observa��o", 16, true, ItensSearch.text, OrderType.none)]
        public string ImprimeObservacaoStr
        {
            get
            {
                return TipoHoraExtraFaltaBool == true ? "Sim" : "N�o";
            }
        }
        /// <summary>
        /// Vari�vel do Flag que marca se vai Separar Extra / Falta ou n�o
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
                return TipoHoraExtraFaltaBool == true ? "Sim" : "N�o";
            }
        }
        /// <summary>
        /// Vari�vel do Flag que marca  se vai Imprimir o N�mero do Rel�gio ou n�o
        /// </summary>
        public Int16 ImprimirNumRelogio { get; set; }
        /// <summary>
        /// Observa��o do Par�metro
        /// </summary>
        [Display(Name = "Observa��o")]
        [StringLength(100, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [TableHTMLAttribute("Observa��o", 17, true, ItensSearch.text, OrderType.none)]
        public string CampoObservacao { get; set; }
        public int ExportarValorZerado { get; set; }
        [Display(Name = "N�o Exporta Saldo Zerado")]
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
                return TipoHoraExtraFaltaBool == true ? "Sim" : "N�o";
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
        [StringLength(200, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [StringLength(200, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
        [DataType(DataType.Password)]
        public string SenhaEmail { get; set; }

        [Display(Name = "SMTP")]
        [StringLength(50, ErrorMessage = "N�mero m�ximo de caracteres: {1}")]
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

        [Display(Name = "Redu��o Hora Noturna")]
        [TableHTMLAttribute("Redu��o", 6, true, ItensSearch.text, OrderType.none)]
        public string ReducaoHoraNoturna { get; set; }
        public bool BloqueiaDadosIntegrados { get; set; }
        [Display(Name = "Hor�rio padr�o para integra��o de Funcion�rios")]
        public string Horario { get; set; }
        public int? IdHorarioPadraoFunc { get; set; }
        [Display(Name = "Tipo Hor�rio")]
        public int TipoHorarioPadraoFunc { get; set; }
        [Display(Name = "Toler�ncia Adicional Noturno")]
        [Range(0, 59, ErrorMessage = "Toler�ncia Adicional Noturno inv�lido! Ele deve ser um n�mero entre 0 e 59.")]
        [StringLength(2, ErrorMessage = "Toler�ncia Adicional Noturno inv�lido! Ele deve ser um n�mero entre 0 e 59.")]
        [Required(ErrorMessage = "O campo Toler�ncia Adicional Noturno � obrigat�rio.")]
        public string ToleranciaAdicionalNoturnoStr {
            get {
                return ToleranciaAdicionalNoturno.ToString();
            }
            set {
                ToleranciaAdicionalNoturno = int.Parse(value);
            }
        }
        public int ToleranciaAdicionalNoturno { get; set; }
        [Required(ErrorMessage = "O momento de gera��o do Intervalo Autom�tico � obrigat�rio")]
        [Display(Name = "Momento Intervalo Autom�tico")]
        [Range(0, 1, ErrorMessage = "O valor selecionado n�o � v�lido para o momento da gera��o do Intervalo Autom�tico")]
        public Int16 MomentoPreAssinalado { get; set; }

        /// <summary>
        /// Separar Horas Trabalhadas Noturnas / Horas Extras Noturnas
        /// </summary>
        [Display(Name = "Separar Trabalhadas Not. / Extra Not.")]
        public bool Flg_Separar_Trabalhadas_Noturna_Extras_Noturna { get; set; }

    }
}
