using BLL.CalculoMarcacoes.EstrategiasCalculo.Impl.DDSR;
using BLL.CalculoMarcacoes.EstrategiasCalculo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.CalculoMarcacoes.EstrategiasCalculo.Factories
{
    public static class DDSRStrategyFactory
    {
        public static IDDSRStrategy Produce(IEnumerable<DataRow> Marcacoes, Dictionary<int, Modelo.Horario> horariosDasMarcacoes, string connectionString)
        {
            var MarcacaoFeriado = Marcacoes.FirstOrDefault(f => f.Field<string>("legenda").ToLower().Contains("f"));
            var MarcacaoMudancaHorario = Marcacoes.FirstOrDefault(f => f.Field<string>("legenda").ToLower().Contains("m"));

            if ((MarcacaoFeriado != null) && (MarcacaoMudancaHorario != null))
            {
                return new DDSRStrategyNormalFeriadoMudancaHorario(Marcacoes, horariosDasMarcacoes, connectionString);
            }
            else if ((MarcacaoFeriado != null) && (MarcacaoMudancaHorario == null))
            {
                return new DDSRStrategyNormalFeriado(Marcacoes, horariosDasMarcacoes, connectionString);
            }
            else if ((MarcacaoFeriado == null) && (MarcacaoMudancaHorario != null))
            {
                return new DDSRStrategyNormalMudancaHorario(Marcacoes, horariosDasMarcacoes, connectionString);
            }
            else
            {
                return new DDSRStrategyNormal(Marcacoes, horariosDasMarcacoes, connectionString);
            }
            
        }
    }
}
