using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkWebAPIPontoWeb.Controllers
{
    public class LogImportacaoWebApi
    {
        public int IDRep { get; set; }

        public DateTime DataImportacao { get; set; }

        public bool bErro { get; set; }

        public Int16 erro
        {
            get { return bErro == true ? (Int16)1 : (Int16)0; }
        }

        public string LogDeImportacao { get; set; }

        public string nomeArquivo { get; set; }

        public string usuario { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

    }
}
