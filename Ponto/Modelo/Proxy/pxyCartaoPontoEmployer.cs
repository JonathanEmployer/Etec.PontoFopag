using Modelo.Proxy;
using Modelo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyCartaoPontoEmployer
    {
        public PxyFuncionarioCabecalhoRel pxyFuncionarioCabecalhoRel { get; set; }
        public string Periodo { get; set; }
        public PxyCPETotais Totalizador { get; set; }
        public IList<PxyCPEMarcacao> Marcacao { get; set; }
        public IList<Jornada> Jornadas { get; set; }
        public IList<PxyCPEMotivosTratamento> MotivosTratamentos { get; set; }

        public int LinhasQuebra { get; set; }
        public int Posicao { get; set; }
        public List<Modelo.CamposSelecionadosRelCartaoPonto>  CamposSelecionados { get; set; }
    }

    public class PxyCPETotais
    {
        public string SaldoAnteriorBH { get; set; }
        public string SaldoAtualBH { get; set; }
        public string TrabalhadasDiurnas { get; set; }
        public string TrabalhadasNoturnas { get; set; }
        public string ExtrasDiurnas { get; set; }
        public string ExtrasNoturnas { get; set; }
        public string FaltasAtrasos { get; set; }
        public string TotalTrabalhada { get; set; }
        public string TotalTrabalhadaComHorasNoturnas { get; set; }
        public string HorasAdNoturna { get; set; }
    }

    public class PxyCPEMarcacao
    {
        public string Data { get; set; }
        public string DataFormat { get; set; }
        public string Dia { get; set; }
        [CartaoPontoCustomInfo("Marcações Desconsideradas", "Marcações Desconsideradas", false, 0)]
        public string MarcDesconsiderada { get; set; }
        [CartaoPontoCustomInfo("C.J.", "Código Jornada", false, 50)]
        public string CodJornada { get; set; }

        [CartaoPontoCustomInfo("Jornada", "Jornada por Dia (Entradas e Saídas Separadas por -)", false, 50)]
        public string JornadaEmLinha { get; set; }

        private string _horasDiurnas;

        [CartaoPontoCustomInfo("Horas Normais Diu.", "Horas Normais Diurnas", true, 34)]
        public string HorasDiurnas
        {
            get { return _horasDiurnas; }
            set { _horasDiurnas = (value == "--:--" ? "" : value); }
        }

        private string horasNoturnas;
        [CartaoPontoCustomInfo("Horas Normais Not.", "Horas Normais Noturnas", true, 34)]
        public string HorasNoturnas
        {
            get { return horasNoturnas; }
            set { horasNoturnas = (value == "--:--" ? "" : value); }
        }

        private string totalHoraNormal;
        [CartaoPontoCustomInfo("Total H. Normais", "Total de Horas Normais", true, 34)]
        public string TotalHorasNormal
        {
            get { return Modelo.cwkFuncoes.ConvertMinutosHora(Modelo.cwkFuncoes.ConvertHorasMinuto(this.HorasDiurnas) + Modelo.cwkFuncoes.ConvertHorasMinuto(this.HorasNoturnas)); }
        }

        private string horasExtrasDiurnas;
        [CartaoPontoCustomInfo("Horas Extras Diu.", "Horas Extras Diurnas", true, 34)]
        public string HorasExtrasDiurnas
        {
            get { return horasExtrasDiurnas; }
            set { horasExtrasDiurnas = (value == "--:--" ? "" : value); }
        }

        private string horasExtrasNoturnas;
        [CartaoPontoCustomInfo("Horas Extras Not.", "Horas Extras Noturnas", true, 34)]
        public string HorasExtrasNoturnas
        {
            get { return horasExtrasNoturnas; }
            set { horasExtrasNoturnas = (value == "--:--" ? "" : value); }
        }

        private string totalHorasExtras;
        [CartaoPontoCustomInfo("Tot. H.E.", "Total de Horas Extras", true, 34)]
        public string TotalHorasExtras
        {
            get { return Modelo.cwkFuncoes.ConvertMinutosHora(Modelo.cwkFuncoes.ConvertHorasMinuto(this.HorasExtrasDiurnas) + Modelo.cwkFuncoes.ConvertHorasMinuto(this.HorasExtrasNoturnas)); }
        }

        private string faltasAtrasos;
        [CartaoPontoCustomInfo("Faltas / Atrasos", "Faltas e Atrasos", true, 34)]
        public string FaltasAtrasos
        {
            get { return faltasAtrasos; }
            set { faltasAtrasos = (value == "--:--" ? "" : value); }
        }

        private string faltasDiurna;
        [CartaoPontoCustomInfo("Faltas Diurna", "Faltas Diurna", true, 34)]
        public string FaltasDiurna
        {
            get { return faltasDiurna; }
            set { faltasDiurna = (value == "--:--" ? "" : value); }
        }

        private string faltasNoturna;
        [CartaoPontoCustomInfo("Faltas Noturna", "Faltas Noturna", true, 34)]
        public string FaltasNoturna
        {
            get { return faltasNoturna; }
            set { faltasNoturna = (value == "--:--" ? "" : value); }
        }

        [CartaoPontoCustomInfo("Banco Horas", "Banco de Horas Saldo (Crédito - Débito)", true, 34)]
        public string BancoHoras { get; set; }

        [CartaoPontoCustomInfo("B. H. Crédito", "Banco de Horas Crédito", true, 34)]
        public string BancoHorasCredito { get; set; }

        [CartaoPontoCustomInfo("B. H. Débito", "Banco de Horas Débito", true, 34)]
        public string BancoHorasDebito { get; set; }

        private string totalHorasTrabalhadas;
        [CartaoPontoCustomInfo("Total", "Total de Horas Trabalhadas", true, 34)]
        public string TotalHorasTrabalhadas
        {
            get { return totalHorasTrabalhadas; }
            set { totalHorasTrabalhadas = (value == "--:--" ? "" : value); }
        }

        private string totalHorasTrabalhadasComHorasNoturnas;
        [CartaoPontoCustomInfo("Total Trab.+(AD)", "Total de Horas Trabalhadas (+ Redução)", true, 34)]
        public string TotalHorasTrabalhadasComHorasNoturnas
        {
            get { return totalHorasTrabalhadasComHorasNoturnas; }
            set { totalHorasTrabalhadasComHorasNoturnas = (value == "--:--" ? "" : value); }
        }

        [CartaoPontoCustomInfo("Total Trab. Diu.", "Total de Horas Trabalhadas Diurnas", true, 34)]
        public string TotalHorasDiurnas { get; set; }
        [CartaoPontoCustomInfo("Total Trab. Not.", "Total de Horas Trabalhadas Noturnas", true, 34)]
        public string TotalHorasNoturnas { get; set; }
        [CartaoPontoCustomInfo("Total Trab. Not.(AD)", "Total de Horas Trabalhadas Noturnas (+ Redução)", true, 34)]
        public string TotalHorasNoturnasComReducao { get; set; }
        [CartaoPontoCustomInfo("Redução H.N.", "Redução da Hora Noturna", true, 34)]
        public string ReducaoHoraNoturna { get; set; }

        public int ConversaoHoraNoturna { get; set; }
        public int HoraNoturnaMin { get; set; }

        public IList<PxyCPEJornadaRealizada> pxyCPEJornadaRealizada { get; set; }
        public IList<PxyCPETratamentos> pxyCPETratamentos { get; set; }


        [CartaoPontoCustomInfo("Tratamentos e Eventos", "Tratamentos e Eventos", false, 0)]
        public bool TratamentoEventos { get; set; }

        [CartaoPontoCustomInfo("Ocorrências", "Ocorrências", false, 0)]
        public string Ocorrencias { get; set; }

        [CartaoPontoCustomInfo("In Itinere", "Horas In Itinere", false, 0)]
        public string HorasInItinere { get {
            List<string> horasInItinere = new List<string>();
            if (Modelo.cwkFuncoes.ConvertBatidaMinuto(InItinereHrsDentroJornada) > 0)
            {
                horasInItinere.Add(" " + ((int)InItinerePercDentroJornada) + "% " + InItinereHrsDentroJornada);
            }
            if (Modelo.cwkFuncoes.ConvertBatidaMinuto(InItinereHrsForaJornada) > 0)
            {
                horasInItinere.Add(((int)InItinerePercForaJornada) + "% " + InItinereHrsForaJornada + " ");
            }
            if (horasInItinere.Count() > 0)
            {
                return String.Join(" </br> ", horasInItinere);
            }
            else
            {
                return "";
            }
            
        } }

        public string InItinereHrsDentroJornada { get; set; }
        public decimal InItinerePercDentroJornada { get; set; }
        public string InItinereHrsForaJornada { get; set; }
        public decimal InItinerePercForaJornada { get; set; }
        [CartaoPontoCustomInfo("Interjor-nada", "Interjornada, Tempo realizado", true, 34)]
        public string Interjornada { get; set; }
        [CartaoPontoCustomInfo("Interjor-nada Extra", "Interjornada, horas extras por interjornada", true, 34)]
        public string horaExtraInterjornada { get; set; }
    }

    public class PxyCPEJornadaRealizada
    {
        public PxyCPEJornadaRealizada()
        {
            this.Entrada1 = "";
            this.Entrada2 = "";
            this.Saida1 = "";
            this.Saida2 = "";
        }
        public string Entrada1 { get; set; }
        public string Entrada2 { get; set; }
        public string Saida1 { get; set; }
        public string Saida2 { get; set; }
    }

    public class PxyCPETratamentos
    {
        public string Horario { get; set; }
        public string Ocorrencia { get; set; }
        public string Motivo { get; set; }
        public int? IndiceMotivo { get; set; }
        public int IdJustificativa { get; set; }
    }

    public class PxyCPEMotivosTratamento
    {
        public string Motivo { get; set; }
        public int Indice { get; set; }
    }

    public class PxyCPEHorarios
    {
        public string Codigo { get; set; }
        public string E1 { get; set; }
        public string E2 { get; set; }
        public string E3 { get; set; }
        public string E4 { get; set; }
        public string S1 { get; set; }
        public string S2 { get; set; }
        public string S3 { get; set; }
        public string S4 { get; set; }
    }
}

public static class EnumExtensions
{
    public static TAttribute GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        return type.GetField(name) // I prefer to get attributes this way
            .GetCustomAttributes(false)
            .OfType<TAttribute>()
            .SingleOrDefault();
    }
}