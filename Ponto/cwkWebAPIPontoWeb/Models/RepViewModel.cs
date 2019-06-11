using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    /// <summary>
    /// Objeto com os dados do relógio.
    /// </summary>
    public class RepViewModel
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
        private string _CpfUsuarioRep;
        public string CpfUsuarioRep
        {
            get { return CriptoString.Encrypt(_CpfUsuarioRep); }
            set { _CpfUsuarioRep = value; }
        }

        private string _LoginRep;

        public string LoginRep
        {
            get { return CriptoString.Encrypt(_LoginRep); }
            set { _LoginRep = value; }
        }

        private string _SenhaRep;

        public string SenhaRep
        {
            get { return CriptoString.Encrypt(_SenhaRep); }
            set { _SenhaRep = value; }
        }

        private DateTime _dataInicioImportacao;

        public DateTime DataInicioImportacao
        {
            get { return _dataInicioImportacao; }
            set { _dataInicioImportacao = value; }
        }

        public int TempoRequisicao { get; set; }
        public bool ImportacaoAtivada { get; set; }
        public int UltimoNSR { get; set; }
        public bool ServicoComunicador { get; set; }
        public bool EnviarDataHora { get; set; }
        public bool EnviarHorarioVerao { get; set; }
        public bool EnviarEmpresa { get; set; }
        public bool EnviarFuncionario { get; set; }
        public DateTime? UltimaIntegracao { get; set; }
        public string IdTimeZoneInfo { get; set; }
        public string TipoBiometria { get; set; }
        /// <summary>
        /// Define qual o campo será considerado o número do crachá, 0 = dscodigo; 1 = Matrícula
        /// </summary>
        public Int16 CampoCracha { get; set; }
    }
}