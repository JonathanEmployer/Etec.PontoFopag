using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IExportacaoCampos : DAL.IDAL
    {
        Modelo.ExportacaoCampos LoadObject(int id);
        List<Modelo.ExportacaoCampos> GetAllList();
        List<Modelo.ExportacaoCampos> LoadPLayout(int idLayout);
        int CodigoMaximo(int idLayout);
        
    }
}
