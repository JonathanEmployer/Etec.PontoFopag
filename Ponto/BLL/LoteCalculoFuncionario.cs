using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.SQL;

namespace BLL
{
    public class LoteCalculoFuncionario
    {
        private readonly DAL.SQL.LoteCalculoFuncionario loteCalculoDAL;

        public LoteCalculoFuncionario(DataBase db)
        {
            loteCalculoDAL = new DAL.SQL.LoteCalculoFuncionario(db);
        }
        public void Salvar(List<int> idsFuncionarios, Guid idLote)
        {
            loteCalculoDAL.Adicionar(idsFuncionarios, idLote);
        }
    }
}