using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class pxyFechamentobhdHEAnalitico
    {
        [Display(Name = "Matrícula")]
        public String Matricula { get; set; }

        [Display(Name = "Alocação")]
        public String Alocacao { get; set; }

        [Display(Name = "Departamento")]
        public int Departamento { get; set; }

        [Display(Name = "Função")]
        public String Funcao { get; set; }

        [Display(Name = "Jornada")]
        public String Jornada { get; set; }

        [Display(Name = "DataBatida")]
        public DateTime DataBatida { get; set; }

        [Display(Name = "Ent. 1")]
        public String Ent1 { get; set; }

        [Display(Name = "Sai. 1")]
        public String Sai1 { get; set; }

        [Display(Name = "Ent. 2")]
        public String Ent2 { get; set; }

        [Display(Name = "Sai. 2")]
        public String Sai2 { get; set; }

        [Display(Name = "Ent. 3")]
        public String Ent3 { get; set; }

        [Display(Name = "Sai. 3")]
        public String Sai3 { get; set; }

        [Display(Name = "Ent. 4")]
        public String Ent4 { get; set; }

        [Display(Name = "Sai. 4")]
        public String Sai4 { get; set; }

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

        [Display(Name = "Supervisor")]
        public String Supervisor { get; set; }

        [Display(Name = "Ocorrência")]
        public String Ocorrencia { get; set; }

        [Display(Name = "Código Fechamento")]
        public String CodigoFechamento { get; set; }

        public pxyFechamentobhdHEAnalitico()
        {
        }

    }
}
