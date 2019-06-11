using BLL.CalculoMarcacoes.EstrategiasCalculo.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL.CalculoMarcacoes.EstrategiasCalculo.Impl.DDSR
{
    public class DDSRStrategyNormal : DDSRStrategyAbstract, IDDSRStrategy
    {
        public DDSRStrategyNormal(IEnumerable<DataRow> marcacoes, Dictionary<int, Modelo.Horario> horariosDasMarcacoes, string connectionString)
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

                Modelo.Horario h = HorariosDasMarcacoes[Marcacoes.FirstOrDefault().Field<int>("idhorario")];
                int totalHorasFaltas = semanaComDSR.Sum(s => s.Field<int>("horasfaltasdsr"));

                DataRow dt = semanaComDSR.FirstOrDefault(f => f.Field<Int16?>("dsr").GetValueOrDefault() == 1);
                if (dt == null)
                {
                    return result;
                }
                result.Add(MarcacaoDSRFactory(dt, RetornaValorDescontoDSR(h, totalHorasFaltas)));
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
            return "Normal";
        }
    }
}
