using cwkComunicadorWebAPIPontoWeb.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace cwkComunicadorWebAPIPontoWeb.ViewModels
{
    [XmlRoot(ElementName = "ListaReps", IsNullable = false)]
    public class ListaRepsViewModel
    {
        [XmlArray("Relogios")]
        public List<RepViewModel> Reps { get; set; }
        public ListaRepsViewModel()
        {
            Reps = new List<RepViewModel>();
        }
    }
    public class RepViewModel : ICloneable
    {
        public int Id { get; set; }
        public int Codigo { get; set; }
        public string NumSerie { get; set; }
        public string NumRelogio { get; set; }
        public int NumModeloRelogio { get; set; }
        public int TipoComunicacao { get; set; }
        public string SenhaComunicacao { get; set; }
        public string Ip { get; set; }
        public string Porta { get; set; }
        public int QtdDigitosCartao { get; set; }
        public bool UtilizaBiometria { get; set; }
        public string NomeFabricante { get; set; }
        public string NomeModelo { get; set; }
        public string NomeEmpregador { get; set; }
        public string EnderecoEmpregador { get; set; }
        public string CEI { get; set; }
        public string CpfCnpjEmpregador { get; set; }
        public bool EquipamentoHomologadoInmetro { get; set; }
        public Int16 TipoIP { get; set; }
        
        private string _LoginRep;
        public string LoginRep
        {
            get { return CriptoString.Encrypt(_LoginRep); }
            set { _LoginRep = CriptoString.Decrypt(value); }
        }

        private string _SenhaRep;
        public string SenhaRep
        {
            get { return CriptoString.Encrypt(_SenhaRep); }
            set { _SenhaRep = CriptoString.Decrypt(value); }
        }

        private DateTime _dataInicioImportacao;

        public DateTime DataInicioImportacao
        {
            get { return _dataInicioImportacao; }
            set { _dataInicioImportacao = value; }
        }

        private string _CpfUsuarioRep;
        public string CpfUsuarioRep
        {
            get { return CriptoString.Encrypt(_CpfUsuarioRep); }
            set { _CpfUsuarioRep = CriptoString.Decrypt(value); }
        }

        [XmlIgnore]
        public string LoginRepDec
        {
            get
            {
                return _LoginRep;
            }
        }

        [XmlIgnore]
        public string SenhaRepDec
        {
            get
            {
                return _SenhaRep;
            }
        }

        [XmlIgnore]
        public string CpfRepDec
        {
            get
            {
                return _CpfUsuarioRep;
            }
        }

        [JsonIgnore]
        public bool ImportacaoAtivada { get; set; }
        [JsonIgnore]
        public TempoImportacao TipoImportacao { get; set; }
        [JsonIgnore]
        public int TempoImportacao { get; set; }
        [JsonIgnore]
        public int UltimoNsr { get; set; }
        [JsonIgnore]
        public DateTime DataUltimoNsr { get; set; }
        [JsonIgnore]
        public int NumeroTentativasComunicacao { get; set; }
        [JsonIgnore]
        public DateTime DataUltimaTentativa { get; set; }
        [JsonIgnore]
        public string DescRelogio { 
            get {
                return NumRelogio + " - " + NomeModelo + " - "+EnderecoEmpregador;
                } 
        }
        [JsonIgnore]
        public string TipoBio { get; set; }
        public RepViewModel()
        {
            ImportacaoAtivada = false;
            TipoImportacao = ViewModels.TempoImportacao.Horas;
            TempoImportacao = 1;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public enum TempoImportacao
    {
        Horas, Minutos, Dias
    }
}

