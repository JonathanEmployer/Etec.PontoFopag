using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface ILogErroPainelAPI : DAL.IDAL
    {
        Modelo.LogErroPainelAPI LoadObject(int id);
        List<Modelo.LogErroPainelAPI> GetGrid();
        List<Modelo.LogErroPainelAPI> GetAllList();
        bool BuscaLogErroPNL(string pLogErro);
        int? getLogErroPNL(string pLogErro);

        
    }
}
