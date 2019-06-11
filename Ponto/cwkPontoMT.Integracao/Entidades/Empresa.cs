using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Entidades
{
    public class Empresa
    {
        public TipoDocumento TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string CEI { get; set; }
        public string RazaoSocial { get; set; }
        public string Local { get; set; }
    }

    public enum TipoDocumento
    {
        CNPJ = 1,
        CPF = 2
    }
}
