using System;
using System.Collections.Generic;

namespace DAL
{
    public interface IFechamentoPonto : DAL.IDAL
    {
        Modelo.FechamentoPonto LoadObject(int id);
        List<Modelo.FechamentoPonto> GetAllList();
        List<Modelo.FechamentoPonto> GetFechamentosPorTipoFiltro(DateTime data, int tipoFiltro, List<int> idsRegistros);

        (int? Mes, int? Ano) GetMesAnoFechamento(int idFechamento, int idEmpresa, int idFuncionario);

        void UpdateIdJob(int idFechamento, string idJob);

        string GetIdJob(int idFechamento);

    }
}
