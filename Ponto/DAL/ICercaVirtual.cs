using System.Collections.Generic;

namespace DAL
{
    public interface ICercaVirtual : DAL.IDAL
    {
        Modelo.CercaVirtual LoadObject(int id);

        List<Modelo.CercaVirtual> GetAllList(int Codigo);
        void Excluir(int CodigoCercaVirtual, int CodigoFuncionario);
    }
}
