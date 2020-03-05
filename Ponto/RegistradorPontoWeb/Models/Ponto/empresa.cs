//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegistradorPontoWeb.Models.Ponto
{
    using System;
    using System.Collections.Generic;
    
    public partial class empresa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public empresa()
        {
            this.afastamento = new HashSet<afastamento>();
            this.contrato = new HashSet<contrato>();
            this.departamento = new HashSet<departamento>();
            this.EmpresaTermoUso = new HashSet<EmpresaTermoUso>();
            this.HorarioDinamicoRestricao = new HashSet<HorarioDinamicoRestricao>();
            this.HorarioRestricao = new HashSet<HorarioRestricao>();
            this.JustificativaRestricao = new HashSet<JustificativaRestricao>();
            this.OcorrenciaRestricao = new HashSet<OcorrenciaRestricao>();
            this.empresacwusuario = new HashSet<empresacwusuario>();
            this.EnvioDadosRepDet = new HashSet<EnvioDadosRepDet>();
            this.feriado = new HashSet<feriado>();
            this.funcionario = new HashSet<funcionario>();
            this.IP = new HashSet<IP>();
            this.EmpresaLogo = new HashSet<EmpresaLogo>();
            this.mudcodigofunc = new HashSet<mudcodigofunc>();
            this.ocorrenciaempresa = new HashSet<ocorrenciaempresa>();
            this.rep = new HashSet<rep>();
        }
    
        public int id { get; set; }
        public int codigo { get; set; }
        public string nome { get; set; }
        public string endereco { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string cep { get; set; }
        public string cnpj { get; set; }
        public string cpf { get; set; }
        public string chave { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public string cei { get; set; }
        public Nullable<int> bprincipal { get; set; }
        public Nullable<int> tipolicenca { get; set; }
        public Nullable<int> quantidade { get; set; }
        public string numeroserie { get; set; }
        public Nullable<int> bdalterado { get; set; }
        public bool bloqueiousuarios { get; set; }
        public bool relatorioabsenteismo { get; set; }
        public bool exportacaohorasabonadas { get; set; }
        public Nullable<bool> modulorefeitorio { get; set; }
        public Nullable<int> IDRevenda { get; set; }
        public Nullable<System.DateTime> validade { get; set; }
        public string ultimoacesso { get; set; }
        public string InstanciaBD { get; set; }
        public bool utilizacontrolecontratos { get; set; }
        public bool relatorioInconsistencia { get; set; }
        public bool relatorioComparacaoBilhetes { get; set; }
        public bool utilizaregistradorfunc { get; set; }
        public Nullable<int> idIntegracao { get; set; }
        public Nullable<short> DiaFechamentoInicial { get; set; }
        public Nullable<short> DiaFechamentoFinal { get; set; }
        public bool PermiteClassHorasExtrasPainel { get; set; }
        public bool BloqueiaJustificativaForaPeriodo { get; set; }
        public Nullable<short> DtInicioJustificativa { get; set; }
        public Nullable<short> DtFimJustificativa { get; set; }
        public Nullable<int> IdHorarioPadraoFunc { get; set; }
        public Nullable<int> TipoHorarioPadraoFunc { get; set; }
        public bool PermiteAbonoParcialPainel { get; set; }
        public Nullable<bool> LimitarQtdAbono { get; set; }
        public bool bloqueioEdicaoEmp { get; set; }
        public bool Ativo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<afastamento> afastamento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<contrato> contrato { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<departamento> departamento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmpresaTermoUso> EmpresaTermoUso { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HorarioDinamicoRestricao> HorarioDinamicoRestricao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HorarioRestricao> HorarioRestricao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JustificativaRestricao> JustificativaRestricao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OcorrenciaRestricao> OcorrenciaRestricao { get; set; }
        public virtual Revendas Revendas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<empresacwusuario> empresacwusuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnvioDadosRepDet> EnvioDadosRepDet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<feriado> feriado { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<funcionario> funcionario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IP> IP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmpresaLogo> EmpresaLogo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<mudcodigofunc> mudcodigofunc { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ocorrenciaempresa> ocorrenciaempresa { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rep> rep { get; set; }
    }
}
