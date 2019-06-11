using System;
using System.Text;
using System.Collections.Generic;

namespace Modelo
{
    public class Cw_Acesso : Modelo.ModeloBase
    {
        public int IdGrupo { get; set; }
        public string Formulario { get; set; }
        public bool Acesso { get; set; }
        public List<Modelo.Cw_AcessoCampo> Campos { get; set; }
    }
}
