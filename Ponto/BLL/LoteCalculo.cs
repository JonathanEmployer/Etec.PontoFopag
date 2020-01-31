using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;

namespace BLL
{
    public class LoteCalculo
    {
        private readonly DAL.SQL.LoteCalculo loteCalculoDAL;

        public LoteCalculo()
        {
            loteCalculoDAL = new DAL.SQL.LoteCalculo();
        }
        public Guid Salvar(DateTime dataInicio, DateTime dataFim)
        {
            return loteCalculoDAL.Adicionar(dataInicio, dataFim);
        }
    }
}
