using System.Collections.Generic;

namespace DAL
{
    public interface ICamposSelecionadosRelCartaoPonto : DAL.IDAL
    {
        Modelo.CamposSelecionadosRelCartaoPonto LoadObject(int id);
        List<Modelo.CamposSelecionadosRelCartaoPonto> GetAllList();
    }
}

