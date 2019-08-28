using System.Collections.Generic;

namespace DAL
{
    public interface IJustificativaRestricao : DAL.IDAL
    {
        Modelo.JustificativaRestricao LoadObject(int id);
        List<Modelo.JustificativaRestricao> GetAllList();
        List<Modelo.JustificativaRestricao> GetAllListByJustificativas(List<int> idsJustificativa); 
    }
}

