using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class Jornada
    {
        public Jornada()
        {
            Entrada_1 = "--:--";
            Entrada_2 = "--:--";
            Entrada_3 = "--:--";
            Entrada_4 = "--:--";
            Saida_1 = "--:--";
            Saida_2 = "--:--";
            Saida_3 = "--:--";
            Saida_4 = "--:--";
        }

        public int Id { get; set; }

        public int Codigo { get; set; }

        public string Entrada_1 { get; set; }
      
        public string Entrada_2 { get; set; }
        
        public string Entrada_3 { get; set; }
        
        public string Entrada_4 { get; set; }
        
        public string Saida_1 { get; set; }
        
        public string Saida_2 { get; set; }
        
        public string Saida_3 { get; set; }
        
        public string Saida_4 { get; set; }

        public virtual string horarios
        {
            get
            {
                string hor = Entrada_1 + " - " + Saida_1;
                if (Entrada_2 != "--:--") { hor = hor + " - " + Entrada_2; };
                if (Saida_2 != "--:--") { hor = hor + " - " + Saida_2; };
                if (Entrada_3 != "--:--") { hor = hor + " - " + Entrada_3; };
                if (Saida_3 != "--:--") { hor = hor + " - " + Saida_3; };
                if (Entrada_4 != "--:--") { hor = hor + " - " + Entrada_4; };
                if (Saida_4 != "--:--") { hor = hor + " - " + Saida_4; };
                return hor;
            }
        }
    }
}