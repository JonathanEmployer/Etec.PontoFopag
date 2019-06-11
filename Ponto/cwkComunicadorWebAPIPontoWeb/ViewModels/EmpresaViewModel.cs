using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.ViewModels
{
    public class EmpresaViewModel: cwkPontoMT.Integracao.Entidades.Empresa
    {
        public string DocumentoFormat { get {
            switch (TipoDocumento)
            {
                case cwkPontoMT.Integracao.Entidades.TipoDocumento.CNPJ:
                    return Convert.ToUInt64(Documento).ToString(@"00\.000\.000\/0000\-00");
                default:
                    return Convert.ToUInt64(Documento).ToString(@"000\.000\.000\-00");;
            }
        } }
    }
}
