using Modelo;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IContratoFuncionario : IDAL
    {
        ContratoFuncionario LoadObject(int id);
        List<ContratoFuncionario> GetAllList();
        List<ContratoFuncionario> GetAllListPorContrato(int idContrato);
        ContratoFuncionario LoadPorCodigo(int codigo);
        pxyContratoFuncionario GetListaFuncionariosLiberadosBloqueadosPorContrato(int idContrato);
        int? GetIdPorIdContratoeIdFuncionario(int idcontrato, int idfuncionario);
        int? getContratoId( int idfuncionario);
        int getContratoCodigo(int idcontrato, int idfuncionario);

    }
}
