using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public interface IAlertasLog : DAL.IDAL
    {
        Modelo.AlertasLog LoadObject(int id);
        List<Modelo.AlertasLog> GetAllList();
        void ExcluirLogPorAlerta(SqlTransaction trans, int idAlerta);
        List<Modelo.AlertasLog> GetAllListByAlerta(int idAlerta);
    }
}

