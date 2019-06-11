using System;
using System.Data;
using System.Collections.Generic;
using Modelo.Proxy;
using Modelo;

namespace DAL
{
    public interface IEmpresaCw_Usuario : DAL.IDAL
    {

        EmpresaCw_Usuario LoadObject(int id);
        DataTable GetPorEmpresa(int idEmpresa);
        List<EmpresaCw_Usuario> GetListaPorEmpresa(int idEmpresa);
        pxyEmpresaCwUsuario GetListaUsuariosLiberadosBloquadosPorEmpresa(int idEmpresa);
        Cw_Usuario GetUsuarioPorCodigo(int codigo);
    }
}
