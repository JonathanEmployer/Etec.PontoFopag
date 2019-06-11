using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Modelo.Utils;

namespace Modelo.Proxy
{
    public class PxyHorarioMovel : Modelo.ModeloBase
    {
        public int Id { get; set; }
        public int Idhorario { get; set; }
        public int Dia { get; set; }
        public string DiaDesc
        {
            get { return DiasSemana.DiaSemanaExtenso(Dia); }
        }
        public DateTime Data { get; set; }
        public string DataString
        {
            get
            {
                return Data == null ? "" : Data.ToString("dd/MM/yyyy");
            }
        }
        [Display(Name = "Ent. 1")]
        public string Entrada_1 { get; set; }
        [Display(Name = "Ent. 2")]
        public string Entrada_2 { get; set; }
        [Display(Name = "Ent. 3")]
        public string Entrada_3 { get; set; }
        [Display(Name = "Ent. 4")]
        public string Entrada_4 { get; set; }
        [Display(Name = "Sai. 1")]
        public string Saida_1 { get; set; }
        [Display(Name = "Sai. 2")]
        public string Saida_2 { get; set; }
        [Display(Name = "Sai. 3")]
        public string Saida_3 { get; set; }
        [Display(Name = "Sai. 4")]
        public string Saida_4 { get; set; }

        public string Totaltrabalhadadiurna { get; set; }
        public string Totaltrabalhadanoturna { get; set; }
        public string Cargahorariamista { get; set; }

        public Int16 Flagfolga { get; set; }
        public string Folga { get; set; }
        public Int16 Diadsr { get; set; }
        public string DSR { get; set; }

        public string DiaStr { get; set; }

        public string Descricao { get; set; }

        [Display(Name = "Início")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public DateTime DataInicial { get; set; }

        [Display(Name = "Fim")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        [DateGreaterThanAttributeNull("DataInicial", "Início")]
        public DateTime DataFinal { get; set; }
    }
}