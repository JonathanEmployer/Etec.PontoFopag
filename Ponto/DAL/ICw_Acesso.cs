using System;
using System.Data;

namespace DAL
{
    public interface ICw_Acesso : DAL.IDAL
    {
        Modelo.Cw_Acesso LoadObject(int id);
        Modelo.Cw_Acesso LoadObject(int pIdGrupo, string pFormulario);
        bool PossuiRegistro(int pIdGrupo, string pFormulario);
        bool PossuiAcesso(int pIdGrupo, string pFormulario);
    }
}
