using System;

namespace cwkPontoMT.Integracao.Entidades
{
    public class Biometria
    {
        public int codigo { get; set; }
        public Byte[] valorBiometria { get; set; }
        public int idfuncionario { get; set; }
        public int idRep { get; set; }
    }
}
