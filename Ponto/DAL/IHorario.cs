using System.Data;
using System.Collections.Generic;
using System.Collections;


namespace DAL
{
    public interface IHorario : DAL.IDAL
    {
        Modelo.Horario LoadObject(int id);
        DataTable GetHorarioTipo(int pTipoHorario);
        DataTable GetHorarioNormal();
        DataTable GetHorarioMovel();
        List<Modelo.Horario> GetHorarioNormalMovelList(int tipoHorario, bool validaPermissaoUser);
        DataTable GetPorDescricao(string pHorarios);
        List<Modelo.Horario> getTodosHorariosDaEmpresa(int pIdEmpresa);
        //List<Modelo.Horario> getPorParametro(int pIdParametro);
        List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais);
        List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipohorario, bool validaPermissaoUser);
        List<Modelo.Horario> GetAllList(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipo, string pIds);
        List<Modelo.Horario> GetParaIncluirMarcacao(Hashtable ids, bool carregaHorarioDetalhe);
        List<int> GetIds();
        List<Modelo.Horario> getPorParametro(int idParametro);
        Hashtable GetHashCodigoId();
        Hashtable GetHashCodigoIdNormal();
        Hashtable GetHashCodigoIdFlexivel();
        int? GetIdPorCodigo(int Cod, bool validaPermissaoUser);
        int MinIdHorarioNormal();
        List<Modelo.FechamentoPonto> FechamentoPontoHorario(List<int> ids);
        List<Modelo.Proxy.PxyGridHorarioFlexivel> GetAllGrid(int tipoRelogio);
        List<Modelo.Proxy.pxyHistoricoAlteracaoHorario> GetHistoricoAlteracaoHorario(int id);
        List<Modelo.Horario> GetAllListOrigem(bool carregaHorarioDetalhe, bool carregaPercentuais, int tipohorario);
        List<Modelo.Horario> GetAllListOrigem(bool carregaHorarioDetalhe, bool carregaPercentuais);

        Modelo.Horario GetHorEntradaSaidaFunc(int idhorario);
    }
}
