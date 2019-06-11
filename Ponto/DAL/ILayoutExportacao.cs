using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface ILayoutExportacao : DAL.IDAL
    {
        Modelo.LayoutExportacao LoadObject(int id);
        List<Modelo.LayoutExportacao> GetAllList();
    }
}
