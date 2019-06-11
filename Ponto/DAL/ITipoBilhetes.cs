using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface ITipoBilhetes : DAL.IDAL
    {
        Modelo.TipoBilhetes LoadObject(int id);
        List<Modelo.TipoBilhetes> getListaImportacao();
        int ContaNumRegistros();
    }
}
