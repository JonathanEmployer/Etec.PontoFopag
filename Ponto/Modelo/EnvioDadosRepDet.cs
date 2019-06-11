using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class EnvioDadosRepDet : Modelo.ModeloBase
    {
        public int? idEmpresa { get; set; }

        public int? idFuncionario { get; set; }

        public int idEnvioDadosRep { get; set; }

    }
}
