using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;

namespace DAL
{
    public interface IHorarioDetalhe : DAL.IDAL
    {
        Modelo.HorarioDetalhe LoadObject(int id);
        Modelo.HorarioDetalhe LoadObject(int pHorario, DateTime pData);
        List<Modelo.HorarioDetalhe> LoadPorHorario(int idHorario);
        List<Modelo.HorarioDetalhe> GetAllList();
        Modelo.HorarioDetalhe LoadParaCartaoPonto(int pHorario, int pDia, DateTime? pData, int pTipoHorario);
        List<Modelo.HorarioDetalhe> GetPorJornada(int idJornada);
        void AtualizaHorarioDetalheJornada(List<Modelo.Jornada> jornadas);
        Hashtable LoadHorariosOrdenaSaida(int idHorario);
        List<Modelo.pxyHorarioDetalheFuncionario> HorarioDetalheSegundoRegistroPorIdHorarioDoPrimeiroRegistro(int idHorario);
        List<Modelo.Proxy.PxyHorarioMovel> GetRelPxyGradeHorario(int idhorario);
        List<Modelo.Proxy.PxyHorarioMovel> GetRelPxyGradeHorario(int idhorario, DateTime dataIni, DateTime dataFin);
    }
}
