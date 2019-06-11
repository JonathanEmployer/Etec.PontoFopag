using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class Marcacao
    {
        public string Bancohorascre { get; set; }
        public string Bancohorasdeb { get; set; }
        public string Data { get; set; }
        public string Dia { get; set; }
        public string Entrada_1 { get; set; }
        public string Entrada_2 { get; set; }
        public string Entrada_3 { get; set; }
        public string Entrada_4 { get; set; }
        public string Entradaextra { get; set; }
        public string Funcionario { get; set; }
        public string Horasextranoturna { get; set; }
        public string Horasextrasdiurna { get; set; }
        public string Horasfaltanoturna { get; set; }
        public string Horasfaltas { get; set; }
        public string Horastrabalhadas { get; set; }
        public string Horastrabalhadasnoturnas { get; set; }
        public int Id { get; set; }
        public int IdFuncionario { get; set; }
        public string Legenda { get; set; }
        public string Ocorrencia { get; set; }
        public string Saida_1 { get; set; }
        public string Saida_2 { get; set; }
        public string Saida_3 { get; set; }
        public string Saida_4 { get; set; }
        public string Saidaextra { get; set; }
        public int IdDocumentoWorkflow { get; set; }
        public string ClassificacaoDescricao { get; set; }
        public int ClassificacaoId { get; set; }
        public string TotalHorasExtras { get {
            string total = "--:--";
            total = Modelo.cwkFuncoes.ConvertMinutosHora2(2,(Modelo.cwkFuncoes.ConvertHorasMinuto(this.Horasextrasdiurna) + Modelo.cwkFuncoes.ConvertHorasMinuto(this.Horasextranoturna)));
            return total;
        } }
        public bool PontoFechado { get; set; }
        public bool BancoHorasFechado { get; set; }
        public bool ClassificarHorasExtras { get; set; }
        public List<Modelo.Proxy.pxyClassHorasExtrasMarcacao> Classificacoes { get; set; }
        public Modelo.Afastamento Afastamento { get; set; }
        public int IdJornada { get; set; }
        public bool MarcacaoIncorreta { get; set; }
        public bool BloquearEdicaoPnlRh { get; set; }
    }
}