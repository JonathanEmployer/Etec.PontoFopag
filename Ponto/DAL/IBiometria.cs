using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public interface IBiometria : DAL.IDAL
    {
        Modelo.Biometria LoadObject(int id);

        List<Modelo.Biometria> GetAllList();

        Modelo.Biometria LoadObjectByCodigo(int codBiometria);
        List<Modelo.Biometria> LoadPorFuncionario(int idfuncionario);
        List<Modelo.Biometria> GetBiometriaTipoBiometria(int IdFuncionario);
    }
}
