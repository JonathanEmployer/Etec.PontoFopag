using Modelo;
using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IContratoUsuario : IDAL
    {
        ContratoUsuario LoadObject(int id);
        List<ContratoUsuario> GetAllList();
        List<ContratoUsuario> GetAllListPorContrato(int idContrato);
        ContratoUsuario LoadPorCodigo(int codigo);
        pxyContratoCwUsuario GetListaUsuariosLiberadosBloqueadosPorContrato(int idContrato);
    }
}
