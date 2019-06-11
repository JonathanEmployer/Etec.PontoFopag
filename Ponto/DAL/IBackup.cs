using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    
    public interface IBackup : DAL.IDAL
    {

        Modelo.Backup LoadObject(int id);
        List<Modelo.Backup> GetAllList();

    }
}
