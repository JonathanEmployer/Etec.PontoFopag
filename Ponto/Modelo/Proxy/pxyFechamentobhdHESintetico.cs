using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyFechamentobhdHESintetico
    {
        public pxyFechamentobhdHESintetico()
        {
        }

        [Display(Name = "Período")]
        public String Periodo { get; set; }

        [Display(Name = "Nome")]
        public String Nome { get; set; }

        [Display(Name = "Matrícula")]
        public String Matricula { get; set; }

        [Display(Name = "Alocação")]
        public String Alocacao { get; set; }

        [Display(Name = "Departamento")]
        public Int32 Departamento { get; set; }

        [Display(Name = "Função")]
        public String Funcao { get; set; }

        [Display(Name = "Horário")]
        public String Horario { get; set; }

        [Display(Name = "Créd. BH")]
        public String CredBH { get; set; }

        [Display(Name = "Déb. BH")]
        public String DebBH { get; set; }

        [Display(Name = "Saldo BH")]
        public String SaldoBH { get; set; }

        [Display(Name = "Perc. 1")]
        public String Perc1 { get; set; }

        [Display(Name = "Perc. 2")]
        public String Perc2 { get; set; }

        [Display(Name = "Código Fechamento")]
        public String CodigoFechamento { get; set; }

        [Display(Name = "Data Fechamento")]
        public String DataFechamento { get; set; }
    }
}
