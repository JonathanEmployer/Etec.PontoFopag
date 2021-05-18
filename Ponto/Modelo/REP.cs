using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;

namespace Modelo
{
    public class REP : Modelo.ModeloBase
    {
        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }
        /// <summary>
        /// Número da Série
        /// </summary>
        [Display(Name = "Número Série")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(20, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Número Série", 2, true, ItensSearch.text, OrderType.none)]
        public string NumSerie { get; set; }
        /// <summary>
        /// Local  
        /// </summary>
        [Display(Name = "Local")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Local", 4, true, ItensSearch.text, OrderType.none)]
        public string Local { get; set; }
        /// <summary>
        /// Número do Relógio
        /// </summary>
        [Display(Name = "Núm. Relógio")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [StringLength(3, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Núm. Relógio", 5, true, ItensSearch.text, OrderType.none, ColumnType.automatico)]
        public string NumRelogio { get; set; }
        [Display(Name = "Relógio")]
        public short Relogio { get; set; }
        [StringLength(20, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string Senha { get; set; }
        [Display(Name = "Tipo Comunicação")]
        public short TipoComunicacao { get; set; }
        [StringLength(10, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("Porta", 12, true, ItensSearch.text, OrderType.none)]
        public string Porta { get; set; }
        [Display(Name = "IP")]
        [StringLength(100, ErrorMessage = "Número máximo de caracteres: {1}")]
        [TableHTMLAttribute("IP", 11, true, ItensSearch.text, OrderType.none)]
        public string IP { get; set; }
        [Display(Name = "Tipo Ip")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public Int16 TipoIP { get; set; }
        public string TipoDescricao
        {
            get
            {
                string tipoDescricao = "";
                switch (TipoIP)
                {
                    case 0: tipoDescricao = "IP";
                        break;
                    case 1:
                        tipoDescricao = "DNS";
                        break;
                    default:
                        tipoDescricao = "IP";
                        break;
                }
                return tipoDescricao;
            }
        }
        [CustomValidation(typeof(MetodosValidacao), "ValidaMaiorQueZero")]
        [Display(Name = "Qtd Dígitos")]
        [TableHTMLAttribute("Qtd. Dígitos", 13, true, ItensSearch.text, OrderType.none)]
        public Int32 QtdDigitos { get; set; }
        [Display(Name = "Biométrico")]
        [TableHTMLAttribute("Biométrico", 7, true, ItensSearch.text, OrderType.none)]
        public bool Biometrico { get; set; }

        public int IdEmpresa { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [TableHTMLAttribute("Empresa", 6, true, ItensSearch.select, OrderType.none)]

        public virtual string empresaNome { get; set; }
        public virtual Empresa ObjEmpresa { get; set; }

        private EquipamentoHomologado _EquipamentoHomologado;
        public EquipamentoHomologado EquipamentoHomologado
        {
            get
            {
                return _EquipamentoHomologado;
            }
            set
            {
                if (value != null)
                {
                    _EquipamentoHomologado = value;
                    Relogio = (short)value.identificacaoRelogio;
                    IdEquipamentoHomologado = value.Id;
                    modeloNome = value.nomeModelo; 
                }
                else
                {
                    _EquipamentoHomologado = new EquipamentoHomologado();
                }
            }
        }

        [Display(Name = "Relógio")]
        [TableHTMLAttribute("Relógio", 8, true, ItensSearch.text, OrderType.asc)]
        public string modeloNome { get; set; }
        public int IdEquipamentoHomologado { get; set; }
        [Display(Name = "Último NSR")]
        [CustomValidation(typeof(MetodosValidacao), "ValidaMaiorOuIgualAZero")]
        /// <summary>
        /// Ultimo NSR Importado do equipamento
        /// </summary>
        [TableHTMLAttribute("Último NSR", 10, true, ItensSearch.text, OrderType.none)]
        public Int64 UltimoNSR { get; set; }
        [Display(Name = "Importação Ativa")]
        /// <summary>
        /// Indica se a importação esta ativa
        /// </summary>
        public bool ImportacaoAtivada { get; set; }
        [Display(Name = "Tempo Import.")]
        [Range(30, int.MaxValue)]
        /// <summary>
        /// Tempo entre as equisições do equipamento.
        /// </summary>
        public int TempoRequisicao { get; set; }
        [Display(Name = "Início Importação")]
        /// <summary>
        /// Data para iniciar a importação
        /// </summary>
        public DateTime DataInicioImportacao { get; set; }

        [TableHTMLAttribute("Início da Importação", 9, true, ItensSearch.text, OrderType.none, ColumnType.data)]
        public string DataInicioImportacaoStr
        {
            get
            {
                return DataInicioImportacao == null ? "" : DataInicioImportacao.ToString("dd/MM/yyyy");
            }
        }

        [Display(Name = "UTC")]
        public string IdTimeZoneInfo { get; set; }

        public Dictionary<string, string> TimeZone
        {
            get
            {
                string[] IdsUTCs = new string[] { "UTC-02", "E. South America Standard Time", "Central Brazilian Standard Time", "SA Pacific Standard Time" };
                var UTCs = new Dictionary<string, string>();
                foreach (string ids in IdsUTCs)
                {
                    TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(ids);
                    UTCs.Add(timeZoneInfo.DisplayName, timeZoneInfo.Id);
                }
                return UTCs;
            }
        }

        /// <summary>
        /// Código do local do rep
        /// </summary>
        [Display(Name = "Cód. Local")]
        [TableHTMLAttribute("Cód. Local", 3, true, ItensSearch.text, OrderType.none)]
        public int CodigoLocal { get; set; }
        public bool IntegraComunicador { get; set; }

        [Display(Name = "Última Integração")]
        [TableHTMLAttribute("Última Integração", 14, true, ItensSearch.text, OrderType.none)]
        public DateTime? UltimaIntegracao { get; set; }

        [Display(Name = "Tipo Biometria")]
        public int IdEquipamentoTipoBiometria { get; set; }

        [Display(Name = "Login do REP")]
        [TableHTMLAttribute("Login REP", 15, true, ItensSearch.text, OrderType.none)]
        public string LoginRep { get; set; }
        [Display(Name = "Senha do REP")]
        [TableHTMLAttribute("Senha REP", 16, true, ItensSearch.text, OrderType.none)]
        public string SenhaRep { get; set; }
        [Display(Name = "CPF do REP")]
        [TableHTMLAttribute("CPF REP", 17, true, ItensSearch.text, OrderType.none)]
        public string CpfRep { get; set; }
        /// <summary>
        /// Define qual o campo será considerado o número do crachá, 0 = dscodigo; 1 = Matrícula
        /// </summary>
        [Display(Name = "Campo Crachá")]
        public Int16 CampoCracha { get; set; }

        public List<ItensCombo> ItensEquipamentoTipoBiometria { get; set; }
        public virtual EquipamentoTipoBiometria EquipamentoTipoBiometria { get; set; }
        [Display(Name = "Portaria 373")]
        public bool Portaria373 { get; set; }
        [Display(Name = "Relógio")]
        public int IdEquipamentoHomologado373 { get; set; }

        [Display(Name = "Registrador Em Massa")]
        public bool RegistradorEmMassa { get; set; }

        [Display(Name = "Crachá proximidade")]
        [Range(0, double.MaxValue, ErrorMessage = "Campo inválido")]
        public Int64? RFID { get; set; }
        public Int64? RFID_Ant { get; set; }
        public string MIFARE { get; set; }

    }
}
