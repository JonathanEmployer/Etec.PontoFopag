using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.ViewModels
{
    public class RepProcessando
    {
        public RepViewModel RepVM { get; set; }
        public bool Processando { get; set; }
        public DateTime InicioImportacao { get; set; }
        public DateTime FimImportacao { get; set; }
    }
}
