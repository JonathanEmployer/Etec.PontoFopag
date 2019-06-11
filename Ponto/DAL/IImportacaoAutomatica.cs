using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IImportacaoAutomatica : DAL.IDAL
    {

        Modelo.ImportacaoAutomatica LoadObject(int id);
        List<Modelo.ImportacaoAutomatica> GetAllList();
        
    }
}
