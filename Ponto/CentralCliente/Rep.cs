//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CentralCliente
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rep
    {
        public Rep()
        {
            this.RepLog = new HashSet<RepLog>();
        }
    
        public int Id { get; set; }
        public int IdRep { get; set; }
        public int codigo { get; set; }
        public string numSerie { get; set; }
        public string local { get; set; }
        public string numrelogio { get; set; }
        public Nullable<short> relogio { get; set; }
        public int idCliente { get; set; }
        public int idEmpresa { get; set; }
        public bool temDataHoraExportar { get; set; }
        public bool temHorarioVeraoExportar { get; set; }
        public bool temEmpresaExportar { get; set; }
        public bool temFuncionarioExportar { get; set; }
        public Nullable<int> UltimoNSR { get; set; }
        public Nullable<System.DateTime> dataUltimaImportacao { get; set; }
        public Nullable<System.DateTime> dataUltimaExportacao { get; set; }
        public int tempoDormir { get; set; }
        public int totalDeRequisicoes { get; set; }
        public Nullable<System.DateTime> DataPrimeiraImportacao { get; set; }
        public bool Ativo { get; set; }
        public Nullable<System.DateTime> DataInicioImportacao { get; set; }
        public bool ServicoComunicador { get; set; }
        public bool Processando { get; set; }
        public Nullable<int> IdCentroServico { get; set; }
        public Nullable<int> IdComunicadorServico { get; set; }
        public Nullable<bool> registradorEmMassa { get; set; }
        public Nullable<long> CrachaAdm { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<RepLog> RepLog { get; set; }
    }
}
