using System;
using System.ComponentModel.DataAnnotations;

namespace Modelo
{
    public class RegistraPonto: Modelo.ModeloBase
    {
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public String CPF { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Campo Obrigatório")]
        public string Senha { get; set; }
        public DateTime DataHoraBatida { get; set; }
        public String HoraRegistroPonto { get; set; }
        [Display(Name = "Empresa:")]
        public string empresa { get; set; }
        [Display(Name = "CNPJ:")]
        public string cnpj { get; set; }
        [Display(Name = "CEI:")]
        public string cei { get; set; }
        [Display(Name = "Nome:")]
        public string nome { get; set; }
        [Display(Name = "PIS:")]
        public string pis { get; set; }
        [Display(Name = "Data:")]
        public string data { get; set; }
        [Display(Name = "Hora:")]
        public string hora { get; set; }
        [Display(Name = "NS:")]
        public string ns { get; set; }

        #region horarios jornada
        [Display(Name = "Ent. 1")]
        public string JEntrada_1 { get; set; }
        [Display(Name = "Ent. 2")]
        public string JEntrada_2 { get; set; }
        [Display(Name = "Ent. 3")]
        public string JEntrada_3 { get; set; }
        [Display(Name = "Ent. 4")]
        public string JEntrada_4 { get; set; }
        [Display(Name = "Sai. 1")]
        public string JSaida_1 { get; set; }
        [Display(Name = "Sai. 2")]
        public string JSaida_2 { get; set; }
        [Display(Name = "Sai. 3")]
        public string JSaida_3 { get; set; }
        [Display(Name = "Sai. 4")]
        public string JSaida_4 { get; set; }
        #endregion

        #region horarios bilhete
        [Display(Name = "Ent.1")]
        public string Entrada_1 { get; set; }
        [Display(Name = "Ent.2")]
        public string Entrada_2 { get; set; }
        [Display(Name = "Ent.3")]
        public string Entrada_3 { get; set; }
        [Display(Name = "Ent.4")]
        public string Entrada_4 { get; set; }
        [Display(Name = "Sai.1")]
        public string Saida_1 { get; set; }
        [Display(Name = "Sai.2")]
        public string Saida_2 { get; set; }
        [Display(Name = "Sai.3")]
        public string Saida_3 { get; set; }
        [Display(Name = "Sai.4")]
        public string Saida_4 { get; set; }
        
        public string Chave { get; set; }
        #endregion

    }
}
