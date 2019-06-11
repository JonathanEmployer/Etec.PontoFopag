using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface ITipoVinculo : DAL.IDAL
    {
        Modelo.TipoVinculo LoadObject(int id);
        bool BuscaTipoVinculo(string pNomeDescricao);
        int? getTipoVinculoNome(string pNomeDescricao);
        int? GetIdPorCod(int Cod);
        List<Modelo.TipoVinculo> GetAllList();
        Modelo.TipoVinculo LoadObjectByCodigo(int idTipoVinculo);
    }
}
