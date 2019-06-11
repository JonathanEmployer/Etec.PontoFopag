using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo.RegraBloqueio;
using System.Data;

namespace BLL
{
    public class HistoricoBloqueio
    {
        public void Gravar(Modelo.RegraBloqueio.HistoricoBloqueio historico)
        {
            DAL.HistoricoBloqueio dal = new DAL.HistoricoBloqueio();
            dal.Inserir(historico);
        }

        public DataTable GerarRelatorioBloqueios(Modelo.Parametros.RelatorioHistoricoBloqueio parametros)
        {
            DAL.HistoricoBloqueio dal = new DAL.HistoricoBloqueio();
            return dal.GerarRelatorioBloqueios(parametros);
        }
    }
}
