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
    
    public partial class TABELACREDENCIAIS
    {
        public long IDTABELACREDENCIAIS { get; set; }
        public string CARTAO { get; set; }
        public string MATRICULA { get; set; }
        public Nullable<long> PIS { get; set; }
        public string SENHA { get; set; }
        public string NOME { get; set; }
        public Nullable<byte> VIA { get; set; }
        public Nullable<int> TIPOTECNOLOGIALEITURA { get; set; }
        public Nullable<int> CODIGOALTERNATIVO { get; set; }
        public Nullable<System.DateTime> DATAINICIAL { get; set; }
        public Nullable<System.DateTime> DATAFINAL { get; set; }
        public Nullable<int> IDJORNADA { get; set; }
        public Nullable<byte> PRIMEIRAFAIXA { get; set; }
        public Nullable<byte> SEGUNDAFAIXA { get; set; }
        public Nullable<byte> TERCEIRAFAIXA { get; set; }
        public Nullable<byte> QUARTAFAIXA { get; set; }
        public Nullable<byte> QUINTAFAIXA { get; set; }
        public Nullable<byte> SEXTAFAIXA { get; set; }
        public Nullable<int> CODIGOMENSAGEMUSUARIO { get; set; }
        public string NOMEARQUIVO { get; set; }
        public Nullable<int> IDRELOGIO { get; set; }
        public Nullable<int> CODIGOLEITURA { get; set; }
        public Nullable<int> TIPOJORNADAACESSO { get; set; }
        public Nullable<bool> PROCESSADO { get; set; }
        public Nullable<bool> USOIDENTIFICACAOBIOMETRICA { get; set; }
    }
}
