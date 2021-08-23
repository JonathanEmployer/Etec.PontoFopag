using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Modelo.Utils;
using System;

namespace Modelo
{
    public class LogImportacaoAFD : Modelo.ModeloBase
    {
        public DateTime DataImportacao { get; set; }

        public string nomeArquivo { get; set; }

        public string usuario { get; set; }

        public DateTime DataInicial { get; set; }

        public DateTime DataFinal { get; set; }


    }
}
