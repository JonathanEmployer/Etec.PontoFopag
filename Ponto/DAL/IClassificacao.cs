using System.Collections.Generic;

namespace DAL
{
    public interface IClassificacao : DAL.IDAL
    {
        Modelo.Classificacao LoadObject(int id);
        List<Modelo.Classificacao> GetAllList();
        List<Modelo.Classificacao> GetAllPorExibePaineldoRH();
        int? GetIdPorCod(int cod);
    }
}

