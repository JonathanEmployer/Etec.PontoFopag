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
    
    public partial class MENSAGEMSISTEMA
    {
        public int IDMENSAGEMSISTEMA { get; set; }
        public byte CODIGOMENSAGEMSISTEMA { get; set; }
        public string DESCRICAO { get; set; }
        public string MENSAGEM { get; set; }
        public int IDMENSAGEMSISTEMAGRUPO { get; set; }
    
        public virtual MENSAGEMSISTEMAGRUPO MENSAGEMSISTEMAGRUPO { get; set; }
    }
}
