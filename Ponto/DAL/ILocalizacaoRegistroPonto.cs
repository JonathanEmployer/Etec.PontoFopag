using System;
using System.Collections.Generic;

namespace DAL
{
    public interface ILocalizacaoRegistroPonto : DAL.IDAL
    {
        Modelo.LocalizacaoRegistroPonto LoadObject(int id);
        List<Modelo.LocalizacaoRegistroPonto> GetAllList();
        Modelo.LocalizacaoRegistroPonto GetPorBilhete(int id);
        List<Modelo.Proxy.Relatorios.PxyRelLocalizacaoRegistroPonto> RelLocalizacaoRegistroPonto(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal);
    }
}

