using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LoteCalculoFuncionario
    {
        private readonly DAL.SQL.LoteCalculoFuncionario loteCalculoDAL;

        public LoteCalculoFuncionario()
        {
            loteCalculoDAL = new DAL.SQL.LoteCalculoFuncionario();
        }
        public Guid Salvar(DateTime dataInicio, DateTime dataFim)
        {
            return loteCalculoDAL.Adicionar(dataInicio, dataFim);
        }
    }
}
