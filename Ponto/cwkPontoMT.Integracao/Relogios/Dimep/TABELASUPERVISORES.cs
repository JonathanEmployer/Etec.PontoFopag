//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cwkPontoMT.Integracao.Relogios.Dimep
{
    using System;
    using System.Collections.Generic;
    
    public partial class TABELASUPERVISORES
    {
        public long IDTABELASUPERVISORES { get; set; }
        public string NOME { get; set; }
        public Nullable<long> PIS { get; set; }
        public string CODIGOCARTAO { get; set; }
        public string SENHA { get; set; }
        public Nullable<bool> PERMISSAOPROGRAMACOESTECNICAS { get; set; }
        public Nullable<bool> PERMISSAODATAHORA { get; set; }
        public Nullable<bool> PERMISSAOPENDRIVE { get; set; }
        public Nullable<bool> PERMISSAOTROCABOBINA { get; set; }
        public string NOMEARQUIVO { get; set; }
        public Nullable<int> IDRELOGIO { get; set; }
        public Nullable<int> CODIGOLEITURA { get; set; }
        public Nullable<bool> PROCESSADO { get; set; }
    }
}
