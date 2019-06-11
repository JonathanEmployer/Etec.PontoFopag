using System;
using System.Data;

namespace DAL
{
    public interface ICw_AcessoCampo : DAL.IDAL
    {
        Modelo.Cw_AcessoCampo LoadObject(int id);
        bool PossuiRegistro(int pIdAcesso, string pCampo);
        bool PossuiAcesso(int pIdAcesso, string pCampo);
    }
}