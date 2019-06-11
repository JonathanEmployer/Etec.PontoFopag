using System;
using System.Text;

namespace Modelo
{
    public class Cw_Usuario : Modelo.ModeloBase
    {                
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public int Tipo { get; set; }
        public int IdGrupo { get; set; }
    }
}
