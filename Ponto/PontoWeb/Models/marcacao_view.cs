//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PontoWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class marcacao_view
    {
        public int id { get; set; }
        public Nullable<int> codigo { get; set; }
        public int idfuncionario { get; set; }
        public string dscodigo { get; set; }
        public string legenda { get; set; }
        public System.DateTime data { get; set; }
        public string dia { get; set; }
        public string entradaextra { get; set; }
        public string saidaextra { get; set; }
        public string ocorrencia { get; set; }
        public Nullable<int> idhorario { get; set; }
        public Nullable<int> idfechamentobh { get; set; }
        public Nullable<short> semcalculo { get; set; }
        public string ent_num_relogio_1 { get; set; }
        public string ent_num_relogio_2 { get; set; }
        public string ent_num_relogio_3 { get; set; }
        public string ent_num_relogio_4 { get; set; }
        public string ent_num_relogio_5 { get; set; }
        public string ent_num_relogio_6 { get; set; }
        public string ent_num_relogio_7 { get; set; }
        public string ent_num_relogio_8 { get; set; }
        public string sai_num_relogio_1 { get; set; }
        public string sai_num_relogio_2 { get; set; }
        public string sai_num_relogio_3 { get; set; }
        public string sai_num_relogio_4 { get; set; }
        public string sai_num_relogio_5 { get; set; }
        public string sai_num_relogio_6 { get; set; }
        public string sai_num_relogio_7 { get; set; }
        public string sai_num_relogio_8 { get; set; }
        public Nullable<short> naoentrarbanco { get; set; }
        public Nullable<short> naoentrarnacompensacao { get; set; }
        public string horascompensadas { get; set; }
        public Nullable<int> idcompensado { get; set; }
        public Nullable<short> naoconsiderarcafe { get; set; }
        public Nullable<short> dsr { get; set; }
        public Nullable<short> abonardsr { get; set; }
        public Nullable<short> totalizadoresalterados { get; set; }
        public Nullable<short> calchorasextrasdiurna { get; set; }
        public Nullable<short> calchorasextranoturna { get; set; }
        public Nullable<short> calchorasfaltas { get; set; }
        public Nullable<short> calchorasfaltanoturna { get; set; }
        public Nullable<System.DateTime> incdata { get; set; }
        public Nullable<System.DateTime> inchora { get; set; }
        public string incusuario { get; set; }
        public Nullable<System.DateTime> altdata { get; set; }
        public string altusuario { get; set; }
        public Nullable<System.DateTime> althora { get; set; }
        public Nullable<short> folga { get; set; }
        public Nullable<short> neutro { get; set; }
        public string chave { get; set; }
        public Nullable<short> tipohoraextrafalta { get; set; }
        public string entrada_1 { get; set; }
        public string entrada_2 { get; set; }
        public string entrada_3 { get; set; }
        public string entrada_4 { get; set; }
        public string entrada_5 { get; set; }
        public string entrada_6 { get; set; }
        public string entrada_7 { get; set; }
        public string entrada_8 { get; set; }
        public string saida_1 { get; set; }
        public string saida_2 { get; set; }
        public string saida_3 { get; set; }
        public string saida_4 { get; set; }
        public string saida_5 { get; set; }
        public string saida_6 { get; set; }
        public string saida_7 { get; set; }
        public string saida_8 { get; set; }
        public string horastrabalhadas { get; set; }
        public string horasextrasdiurna { get; set; }
        public string horasfaltas { get; set; }
        public string horastrabalhadasnoturnas { get; set; }
        public string horasextranoturna { get; set; }
        public string horasfaltanoturna { get; set; }
        public string bancohorascre { get; set; }
        public string bancohorasdeb { get; set; }
        public string valordsr { get; set; }
        public string exphorasextranoturna { get; set; }
        public string totalHorasTrabalhadas { get; set; }
        public Nullable<int> idFechamentoPonto { get; set; }
        public string Interjornada { get; set; }
        public Nullable<int> IdDocumentoWorkflow { get; set; }
        public Nullable<int> DocumentoWorkflowAberto { get; set; }
        public string InItinereHrsDentroJornada { get; set; }
        public Nullable<decimal> InItinerePercDentroJornada { get; set; }
        public string InItinereHrsForaJornada { get; set; }
        public Nullable<decimal> InItinerePercForaJornada { get; set; }
        public Nullable<bool> NaoConsiderarInItinere { get; set; }
        public string LegendasConcatenadas { get; set; }
        public string AdicionalNoturno { get; set; }
        public int flgtrabalhou { get; set; }
        public Nullable<System.DateTime> DataBloqueioEdicaoPnlRh { get; set; }
        public string LoginBloqueioEdicaoPnlRh { get; set; }
        public Nullable<System.DateTime> DataConclusaoFluxoPnlRh { get; set; }
        public string LoginConclusaoFluxoPnlRh { get; set; }
        public string horaExtraInterjornada { get; set; }
        public string horasTrabalhadasDentroFeriadoDiurna { get; set; }
        public string horasTrabalhadasDentroFeriadoNoturna { get; set; }
        public string horasPrevistasDentroFeriadoDiurna { get; set; }
        public string horasPrevistasDentroFeriadoNoturna { get; set; }
        public short naoconsiderarferiado { get; set; }
        public short ContabilizarFaltas { get; set; }
        public short ContAtrasosSaidasAntec { get; set; }
        public short ContabilizarCreditos { get; set; }
        public Nullable<int> IdJornadaSubstituir { get; set; }
        public string SaldoBH { get; set; }
    }
}
