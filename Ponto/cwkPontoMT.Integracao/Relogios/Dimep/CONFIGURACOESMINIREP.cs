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
    
    public partial class CONFIGURACOESMINIREP
    {
        public int IDCONFIGURACAOMINIREP { get; set; }
        public Nullable<bool> HABILITA2DE5INTERCALADO { get; set; }
        public Nullable<bool> HABILITA2DE5PERSONALIZADO { get; set; }
        public Nullable<bool> HABILITA3DE9 { get; set; }
        public Nullable<bool> HABILITAMAGNETICO { get; set; }
        public Nullable<bool> FORMATOMAGNETICOESPECIAL1 { get; set; }
        public Nullable<bool> HABILITAABA { get; set; }
        public Nullable<bool> FORMATOABAESPECIAL1 { get; set; }
        public Nullable<bool> HABILITAWIEGAND26 { get; set; }
        public Nullable<bool> HABILITAWIEGAND32ESPECIAL { get; set; }
        public Nullable<bool> HABILITAWIEGAND34 { get; set; }
        public Nullable<bool> HABILITAWIEGAND35 { get; set; }
        public Nullable<bool> HABILITAWIEGAND37 { get; set; }
        public Nullable<bool> HABILITAPADRAOH10302 { get; set; }
        public Nullable<bool> HABILITAPADRAOH10304 { get; set; }
        public Nullable<bool> HABPARIDADELEITURASWIEGAND { get; set; }
        public Nullable<bool> HABILITASMARTCARD { get; set; }
        public Nullable<bool> HABILIDALEID { get; set; }
        public Nullable<bool> HABILITALEMATRICULA { get; set; }
        public Nullable<bool> HABILITAEAN13 { get; set; }
        public string FORMATOCARTAO2DE5INTERCALADO { get; set; }
        public string FORMATOCARTAO2DE5PERSONALIZADO { get; set; }
        public string FORMATOCARTAO3DE9 { get; set; }
        public string FORMATOCARTAOMAGNETICO { get; set; }
        public string FORMATOCARTAOABA { get; set; }
        public string FORMATOCARTAOWIEGAND26 { get; set; }
        public string FORMATOCARTAOWIEGAND34 { get; set; }
        public string FORMATOCARTAOWIEGAND35 { get; set; }
        public string FORMATOCARTAOWIEGAND37 { get; set; }
        public string FORMATOCARTAOSMARTCARD { get; set; }
        public string FORMATOCARTAOEAN13 { get; set; }
        public Nullable<bool> UTILIZACRIPTOGRAFIA { get; set; }
        public string VETOR1 { get; set; }
        public string VERIFICADOR1 { get; set; }
        public string VETOR2 { get; set; }
        public string VERIFICADOR2 { get; set; }
        public Nullable<bool> CRIPTOGRAFIA8DIGITOS { get; set; }
        public Nullable<bool> CRIPTOGRAFIA10DIGITOS { get; set; }
        public Nullable<bool> CRIPTOGRAFIA12DIGITOS { get; set; }
        public Nullable<bool> HABILITA1PARAN { get; set; }
        public Nullable<bool> IDENTIFICACAONPEDEAUTENTIC { get; set; }
        public Nullable<bool> IDENTIFICACAOAPENASSENHA { get; set; }
        public Nullable<bool> AUTENTICACAOSEMPRE { get; set; }
        public Nullable<bool> AUTENTICACAOPARCIAL { get; set; }
        public Nullable<bool> TECLADO { get; set; }
        public Nullable<bool> TECLADOCREDENCIAL { get; set; }
        public Nullable<bool> TECLADOPIS { get; set; }
        public Nullable<bool> TECLADONAOPEDEAUTENTICACAO { get; set; }
        public Nullable<bool> TECLADOAPENASSENHA { get; set; }
        public Nullable<bool> TECLADOAPENASBIOMETRIA { get; set; }
        public Nullable<bool> TECLADOBIOMETRIAOUSENHA { get; set; }
        public Nullable<bool> TECLADOAMBOS { get; set; }
        public Nullable<bool> CARTAO { get; set; }
        public Nullable<bool> CARTAOCREDENCIAL { get; set; }
        public Nullable<bool> CARTAOPIS { get; set; }
        public Nullable<bool> CARTAONAOPEDEAUTENTICACAO { get; set; }
        public Nullable<bool> CARTAOAPENASSENHA { get; set; }
        public Nullable<bool> CARTAOAPENASBIOMETRIA { get; set; }
        public Nullable<bool> CARTAOBIOMETRIAOUSENHA { get; set; }
        public Nullable<bool> CARTAOAMBOS { get; set; }
        public Nullable<bool> IMPRESSORAPEQUENOAVANCO { get; set; }
        public Nullable<bool> IMPRESSORAMEDIOAVANCO { get; set; }
        public Nullable<bool> IMPRESSORALONGOAVANCO { get; set; }
        public Nullable<bool> IMPRESSORACORTEPARCIAL { get; set; }
        public Nullable<bool> IMPRESSORACORTETOTAL { get; set; }
        public Nullable<bool> NAOTEMPERSONALIZACAO { get; set; }
        public Nullable<bool> ESPECIAL1 { get; set; }
        public Nullable<bool> ESPECIAL2 { get; set; }
        public Nullable<int> NUMERODIGITOSPERSONALIZACAO { get; set; }
        public Nullable<int> CODIGOPERSONALIZACAO { get; set; }
        public int IDCONFIGURACAO { get; set; }
        public Nullable<byte> QUALIDADEIMPRESSAO { get; set; }
        public Nullable<byte> TAMANHOBOBINA { get; set; }
        public Nullable<byte> NIVELSEGURANCABIOMETRIA { get; set; }
        public Nullable<byte> NIVELSEGURANCABIOMETRIASAGEM { get; set; }
        public Nullable<bool> HABILITALEMATRICULAHEXA { get; set; }
    
        public virtual CONFIGURACOES CONFIGURACOES { get; set; }
    }
}
