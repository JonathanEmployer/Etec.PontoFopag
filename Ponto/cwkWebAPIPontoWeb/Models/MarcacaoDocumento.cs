using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class MarcacaoDocumento
    {
        public int IdDocumentoWorkflow { get; set; }
        public int idMarcacao { get; set; }
        public bool DocumentoWorkflowAberto { get; set; }
    }
}