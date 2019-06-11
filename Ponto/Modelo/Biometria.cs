using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DAL.SQL;
using Veridis.Biometric;

namespace Modelo
{
    public class Biometria : Modelo.ModeloBase
    {
        public Byte[] valorBiometria { get; set; }
        public int idfuncionario { get; set; }

        public int Quantidade { get; set; }
        public int idRep { get; set; }
        public string Tipo { get; set; }
        public string Tecnologia { get; set; }

        public Biometria()
        {

        }

        public Biometria(Byte[] pValorBiomatria, int pIdFuncionario)
        {
            valorBiometria = pValorBiomatria;
            idfuncionario = pIdFuncionario;
        }


     
    }
}
