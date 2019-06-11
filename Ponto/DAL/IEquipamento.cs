using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IEquipamento : DAL.IDAL
    {
        Modelo.Equipamento LoadObject(int id);
        DataTable GetEquipamentoAtivo();
        bool BuscaInner(int pInner, int pId);
        int getId(int pValor, string pCampo, int? pValor2);
    }
}
