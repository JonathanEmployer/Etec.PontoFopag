using BLL.CalculoMarcacoes.EstrategiasCalculo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.CalculoMarcacoes.EstrategiasCalculo.Impl.DDSR
{
    public class DDSRStrategyNormalFeriadoMudancaHorario : DDSRStrategyAbstract, IDDSRStrategy
    {
        public DDSRStrategyNormalFeriadoMudancaHorario(IEnumerable<DataRow> marcacoes, Dictionary<int, Modelo.Horario> horariosDasMarcacoes, string connectionString)
            : base(marcacoes, horariosDasMarcacoes, connectionString)
        {

        }
        public List<Modelo.Marcacao> CalcularDDSR()
        {
            List<Modelo.Marcacao> result = new List<Modelo.Marcacao>();
            int rowCounter = 0;
            IEnumerable<DataRow> semanaComDSR = Marcacoes.InclusiveTakeWhile(t => t.Field<Int16?>("dsr").GetValueOrDefault() != 1);
            if (semanaComDSR == null)
            {
                return result;
            }
            while (semanaComDSR != null)
            {
                rowCounter += semanaComDSR.Count();

                List<IEnumerable<DataRow>> MarcacoesAgrupadas = AgrupaMarcacoesPorMudancaHorario(semanaComDSR);
                int totalHorasFaltas = 0;
                foreach (var item in MarcacoesAgrupadas)
                {
                    if (item.Count() == 0)
                    {
                        continue;
                    }

                    int somaParcial = 0;

                    // Se o bloco atual não possui a marcação de mudança
                    // verifica se o próximo bloco de marcações possui um horário que desconte DSR
                    // e a soma das horas faltas no próximo bloco seja maior que zero. Caso seja, 
                    // soma a quantidade de horas faltas antes da mudança (bloco atual) para realizar 
                    // o desconto no próximo dia de DSR.

                    if (item.FirstOrDefault(f => f.Field<string>("legenda").ToLower().Contains("m")) == null)
                    {
                        int idxAtual = MarcacoesAgrupadas.IndexOf(item);
                        if ((idxAtual < MarcacoesAgrupadas.Count - 1))
                        {
                            if (MarcacoesAgrupadas[idxAtual + 1].Count() > 0)
                            {
                                if (MarcacoesAgrupadas[idxAtual + 1].Sum(s => s.Field<int>("horasfaltasdsr")) > 0 &&
                                    HorariosDasMarcacoes[MarcacoesAgrupadas[idxAtual + 1].FirstOrDefault().Field<int>("idhorario")].DescontardsrBool)
                                {
                                    somaParcial = item.Sum(s => s.Field<int>("horasfaltasdsr"));
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }

                    somaParcial = item.Sum(s => s.Field<int>("horasfaltasdsr"));
                    var MarcacoesFeriado = item.Where(f => f.Field<string>("legenda").ToLower().Contains("f"));
                    Modelo.Horario h = HorariosDasMarcacoes[item.FirstOrDefault().Field<int>("idhorario")];
                    somaParcial = Modelo.cwkFuncoes.ConvertHorasMinuto(RetornaValorDescontoDSR(h, somaParcial));

                    if (MarcacoesFeriado != null && h.DescontarFeriadoDDSR)
                    { // Adiciona o valor do desconto do DSR multiplicado pela quantidade de feriados do período analisado
                        somaParcial += ((somaParcial) * (MarcacoesFeriado.Count()));
                    }
                    totalHorasFaltas += somaParcial;
                }

                DataRow dt = semanaComDSR.FirstOrDefault(f => f.Field<Int16?>("dsr").GetValueOrDefault() == 1);
                if (dt == null)
                {
                    return result;
                }
                result.Add(MarcacaoDSRFactory(dt, Modelo.cwkFuncoes.ConvertMinutosHora(totalHorasFaltas)));
                semanaComDSR = Marcacoes.Skip(rowCounter).InclusiveTakeWhile(t => t.Field<Int16?>("dsr").GetValueOrDefault() != 1);
                if (semanaComDSR.Count() == 0)
                {
                    semanaComDSR = null;
                }
            }
            return result;
        }


        public override string ToString()
        {
            return "Normal-Feriado-Mudança de Horário";
        }
    }
}
