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
    
    public partial class STATUSCOMPLETO
    {
        public int IDSTATUSCOMPLETO { get; set; }
        public Nullable<int> IDRELOGIO { get; set; }
        public Nullable<System.DateTime> DATASTATUS { get; set; }
        public Nullable<int> TAMANHOLISTACARTOES { get; set; }
        public Nullable<byte> TIPOCHECAGEM { get; set; }
        public Nullable<System.DateTime> DATAHORA { get; set; }
        public string VERSAOFIRMWARE { get; set; }
        public Nullable<bool> REGISTRABLOQUEADOS { get; set; }
        public Nullable<int> QUANTIDADEREGISTROS { get; set; }
        public Nullable<int> CAPACIDADEREGISTROS { get; set; }
        public Nullable<int> CONTADORESACESSO { get; set; }
        public Nullable<bool> TEMMENSAGEM { get; set; }
        public Nullable<bool> TEMSENHA { get; set; }
        public Nullable<bool> TEMVIA { get; set; }
        public Nullable<bool> TEMJORNADA { get; set; }
        public Nullable<byte> MAXIMODIGITOSCARTAO { get; set; }
        public Nullable<byte> MAXIMOMENSAGENSUSUARIO { get; set; }
        public Nullable<byte> MINIMODIGITOSCARTAO { get; set; }
        public Nullable<byte> QUANTIDADEMAXIMAALARMES { get; set; }
        public Nullable<int> QTDMAXCARTOESALT { get; set; }
        public Nullable<int> QUANTIDADEMAXIMACARTOES { get; set; }
        public Nullable<byte> QUANTIDADEMAXIMAFUNCOES { get; set; }
        public Nullable<byte> QUANTIDADEMAXIMAFERIADOS { get; set; }
        public Nullable<int> QTDMAXJORNADASMENSAIS { get; set; }
        public Nullable<int> QTDMAXJORNADASPERIODICAS { get; set; }
        public Nullable<int> QUANTIDADEMAXIMATURNOS { get; set; }
        public Nullable<int> QTDMAXJORNADASSEMANAIS { get; set; }
        public Nullable<byte> TIPOCHECAGEMBIO { get; set; }
        public string FORMATOCARTAO { get; set; }
        public Nullable<int> QUANTIDADECARTOESNALISTA { get; set; }
        public Nullable<System.DateTime> INICIOHORARIOVERAO { get; set; }
        public Nullable<System.DateTime> FIMHORARIOVERAO { get; set; }
        public Nullable<byte> ESTADOBOBINA { get; set; }
        public Nullable<byte> ESTADORELOGIO { get; set; }
        public Nullable<byte> ESTADOMRP { get; set; }
        public Nullable<byte> TIPOALIMENTACAO { get; set; }
        public Nullable<int> OCPCMRPEMPERCENTCLUSTER { get; set; }
        public string ULTIMONSR { get; set; }
        public Nullable<long> TOTALREGISTROSDEPONTO { get; set; }
        public Nullable<int> REGDEPONTOARECOLHER { get; set; }
        public Nullable<bool> TEMPLATESACOLETAR { get; set; }
        public Nullable<bool> MARCACOESACOLETAR { get; set; }
        public string VERSAOFIRMWAREMRP { get; set; }
        public string MACADDRESS { get; set; }
        public Nullable<byte> TAMANHOAVANCOIMPRESSAO { get; set; }
        public Nullable<byte> TIPOCORTE { get; set; }
        public Nullable<int> CAPACIDADEDEFUNCIONARIOS { get; set; }
        public Nullable<int> OCUPACAODEFUNCIONARIOS { get; set; }
        public Nullable<int> CAPACIDADEDECREDENCIAIS { get; set; }
        public Nullable<int> OCUPACAODECREDENCIAIS { get; set; }
        public Nullable<int> CAPACIDADEDETEMPLATES { get; set; }
        public Nullable<int> OCUPACAODETEMPLATES { get; set; }
        public Nullable<int> CAPACIDADEDESUPERVISORES { get; set; }
        public Nullable<int> CAPACIDADEMODULOBIO { get; set; }
        public Nullable<int> OCUPACAODESUPERVISORES { get; set; }
        public Nullable<bool> CARTAOHABILITADO { get; set; }
        public Nullable<byte> TIPOACESSOCREDENCIAL { get; set; }
        public Nullable<byte> TIPOAUTENTICACAOCRED { get; set; }
        public Nullable<bool> TECLADOHABILITADO { get; set; }
        public Nullable<byte> TIPOACESSOTECLADO { get; set; }
        public Nullable<byte> TIPOAUTENTICACAOTECLADO { get; set; }
        public Nullable<bool> IDENT1PARANHABILITADO { get; set; }
        public Nullable<byte> AUTTIPOIDENTIFICACAO { get; set; }
        public Nullable<byte> TIPOAUTENTICACAOBIO { get; set; }
        public Nullable<byte> NIVELSEGURANCASUPREMA { get; set; }
        public Nullable<byte> NIVELSEGURANCASAGEM { get; set; }
        public Nullable<bool> GERANDOAFD { get; set; }
        public Nullable<bool> GERANDORIM { get; set; }
        public string NUMEROSERIEREP { get; set; }
        public string NUMEROSERIEPLACA { get; set; }
        public string NUMEROSERIEMRP { get; set; }
        public string NUMEROLACREMRP { get; set; }
        public Nullable<bool> UTILIZA2DE5INTERCALADO { get; set; }
        public string FORMATO2DE5INTERCALADO { get; set; }
        public Nullable<byte> TIPOCRIPTOGRAFIA { get; set; }
        public Nullable<bool> UTILIZA2DE5ESPECIAL { get; set; }
        public string FORMATO2DE5ESPECIAL { get; set; }
        public Nullable<bool> UTILIZA2DE9 { get; set; }
        public string FORMATO3DE9 { get; set; }
        public Nullable<bool> UTILIZAEAN13 { get; set; }
        public string FORMATOEAN13 { get; set; }
        public Nullable<bool> UTILIZAMAGNETICO { get; set; }
        public string FORMATOMAGNETICO { get; set; }
        public Nullable<bool> FORMATOESPECIALMAGNETICO { get; set; }
        public Nullable<bool> UTILIZAABA { get; set; }
        public Nullable<bool> FORMATOESPECIALABA { get; set; }
        public string FORMATOABA { get; set; }
        public Nullable<bool> HABPARIDADELEITWIEGAND { get; set; }
        public Nullable<bool> UTILIZAWIEGAND { get; set; }
        public string FORMATOWIEGAND { get; set; }
        public Nullable<bool> UTILIZAWIEGAND32ESPECIAL { get; set; }
        public Nullable<bool> UTILIZAWIEGAND34 { get; set; }
        public string FORMATOWIEGAND34 { get; set; }
        public Nullable<bool> UTILIZAWIEGAND35 { get; set; }
        public string FORMATOWIEGAND35 { get; set; }
        public Nullable<bool> UTILIZAWIEGAND37 { get; set; }
        public Nullable<byte> TIPOUSOWIEGAND37 { get; set; }
        public string FORMATOWIEGAND37 { get; set; }
        public Nullable<bool> UTILIZASMARTCARD { get; set; }
        public Nullable<byte> TIPOUSOSMARTCARD { get; set; }
        public string FORMATOSMARTCARD { get; set; }
        public Nullable<int> OCUPACAODESENHAS { get; set; }
        public Nullable<byte> QUANTIDADEDIGITOSCARTAO { get; set; }
        public Nullable<bool> UTILIZAMODULO11 { get; set; }
        public Nullable<byte> SENTIDOENTRADA { get; set; }
        public Nullable<byte> QUANTIDADEDELEITORES { get; set; }
        public Nullable<byte> TIPOACIONAMENTO { get; set; }
        public Nullable<short> DATAULTIMACOLETA { get; set; }
        public Nullable<int> OCUPACAOMEMORIAEMPERCENTUAL { get; set; }
        public Nullable<int> DATAULTIMAFALHAAC { get; set; }
        public Nullable<int> HORAINICIOULTIMAFALHAAC { get; set; }
        public Nullable<int> DURACAOULTIMAFALHAAC { get; set; }
        public Nullable<byte> CONDICAOAC { get; set; }
        public Nullable<byte> ESTADOTERMINAL { get; set; }
        public Nullable<byte> ESTADOHORARIOVERAO { get; set; }
        public Nullable<byte> ESTADOTABCARTOES { get; set; }
        public Nullable<byte> ESTADOTABFAIXAHORARIA { get; set; }
        public Nullable<byte> ESTADOTABGRUPOFAIXAHORARIA { get; set; }
        public Nullable<byte> ESTADOTABSINALEIRO { get; set; }
        public Nullable<byte> ESTADOTABMENSAGEM { get; set; }
        public Nullable<int> FUNCAOCORRENTE { get; set; }
    
        public virtual RELOGIOS RELOGIOS { get; set; }
    }
}
