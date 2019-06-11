using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface ILayoutImportacao: DAL.IDAL
    {
        Modelo.LayoutImportacao LoadObject(int id);

        List<Modelo.LayoutImportacao> GetAllList();

        int QtdRegistrosLayout();
    }
}
