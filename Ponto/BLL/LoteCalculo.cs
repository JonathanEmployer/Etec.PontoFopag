using System;
using DAL.SQL;

namespace BLL
{
    public class LoteCalculo
    {
        private readonly DAL.SQL.LoteCalculo loteCalculoDAL;

        public LoteCalculo(DataBase db)
        {
            loteCalculoDAL = new DAL.SQL.LoteCalculo(db);
        }
        public Guid Salvar(DateTime dataInicio, DateTime dataFim)
        {
            return loteCalculoDAL.Adicionar(dataInicio, dataFim);
        }
    }
}
