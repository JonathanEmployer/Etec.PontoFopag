using System;
using System.Text;

namespace Modelo
{
    public class Cw_AcessoCampo : Modelo.ModeloBase
    {
        public int IdAcesso { get; set; }
        public string Campo { get; set; }
        public string Display { get; set; }
        public bool Acesso { get; set; }
        public Modelo.Acao Acao { get; set; }
    }
}