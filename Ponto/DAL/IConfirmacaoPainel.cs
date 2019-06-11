using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IConfirmacaoPainel : DAL.IDAL
    {
        Modelo.ConfirmacaoPainel LoadObject(int id);
        List<Modelo.ConfirmacaoPainel> GetAllList();
        Modelo.ConfirmacaoPainel LoadObjectByCodigo(int idConfirmacaoPainel);
        Modelo.ConfirmacaoPainel GetPorMesAnoIdFunc(int Mes, int Ano, int idFuncionario);
    }
}


