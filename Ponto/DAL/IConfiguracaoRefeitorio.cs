using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IConfiguracaoRefeitorio : DAL.IDAL
    {
        Modelo.ConfiguracaoRefeitorio LoadObject(int id);
        int getId(int pValor, string pCampo, int? pValor2);
    }
}
