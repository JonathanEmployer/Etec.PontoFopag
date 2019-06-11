using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.ViewModels
{
    public class ImportacaoDadosRepViewModel
    {
            public RepViewModel Rep { get; set; }
            public List<cwkPontoMT.Integracao.Entidades.Empresa> Empresas { get; set; }
            public List<cwkPontoMT.Integracao.Entidades.Empregado> Empregados { get; set; }
            public EnvioDadosRep EnvioDadosRep { get; set; }
    }
}
