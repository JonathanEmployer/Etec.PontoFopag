using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IParametros : DAL.IDAL
    {
        Modelo.Parametros LoadObject(int id);
        Modelo.Parametros LoadPrimeiro();
        List<Modelo.Parametros> GetAllList();
        void AtualizaTipoExtraFaltaMarcacoes(int id, Int16 tipohoraextrafalta, DateTime? dataInicial, DateTime? dataFinal);
        int GetExportaValorZerado();
        int? GetIdPorCod(int Cod);
        List<Modelo.Parametros> GetAllList(List<int> ids);
        bool Flg_Separar_Trabalhadas_Noturna_Extras_Noturna(int idfuncionario);
    }
}
