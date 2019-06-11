using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Entidades
{
    public class Empregado
    {
        public string DsCodigo { get; set; }
        public string Pis { get; set; }
        public string Nome { get; set; }
        public bool Biometria { get; set; }
        public string Senha { get; set; }
        public string Matricula { get; set; }
        public Int64? RFID { get; set; }
        public Int64? MIFARE { get; set; }
        public bool Ativo { get; set; }
        public byte[] valorBiometria { get; set; }
        public int id { get; set; }
    }
}
