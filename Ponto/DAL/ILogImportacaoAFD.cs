using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface ILogImportacaoAFD : DAL.IDAL
    {
        Modelo.LogImportacaoAFD LoadObject(int id);
        List<Modelo.LogImportacaoAFD> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal);
    }
}
