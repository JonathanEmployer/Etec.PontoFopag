using System;
using System.Data;
using System.Collections.Generic;

namespace DAL
{
    public interface IUsuarioControleAcesso : DAL.IDAL
    {
        Modelo.UsuarioControleAcesso LoadObject(int id);
        List<Modelo.Contrato> ContratoPorUsuario(int idUsuario);
    }
}
