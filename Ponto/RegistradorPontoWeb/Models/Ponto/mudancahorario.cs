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
    
    public partial class mudancahorario
    {
        public int id { get; set; }
        public int codigo { get; set; }
        public Nullable<int> idfuncionario { get; set; }
        public Nullable<short> tipohorario { get; set; }
        public Nullable<int> idhorario { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public Nullable<short> tipohorario_ant { get; set; }
        public Nullable<short> idhorario_ant { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public string altusuario { get; set; }
        public Nullable<int> idLancamentoLoteFuncionario { get; set; }
        public Nullable<int> idHorarioDinamico { get; set; }
        public Nullable<int> CicloSequenciaIndice { get; set; }
    
        public virtual funcionario funcionario { get; set; }
        public virtual horario horario { get; set; }
        public virtual HorarioDinamico HorarioDinamico { get; set; }
        public virtual LancamentoLoteFuncionario LancamentoLoteFuncionario { get; set; }
    }
}
