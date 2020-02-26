using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PontoWeb.Models
{
    public class RecalculaMarcacaoViewModel
    {
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Data Inicial")]
        public DateTime? DataInicial { get; set; }
        [Required(ErrorMessage = "Campo Obrigatório")]
        [Display(Name = "Data Final")]
        public DateTime? DataFinal { get; set; }
        public string Empresa { get; set; }
        public string Departamento { get; set; }
        [Display(Name = "Função")]
        public string Funcao { get; set; }
        public string IdsFuncionariosSelecionados { get; set; }
        public string IdsFuncionariosSelecionados_Ant { get; set; }
        public int Tipo { get; set; }
        public List<int> Identificadores { get; set; }
        public string ParametroTipo { get {
                switch (Tipo)
                {
                    case 0:
                        return "Empresa - "+Empresa;
                    case 1:
                        return "Departamento - "+Departamento;
                    case 3:
                        return "Função - "+Funcao;
                    case 2:
                        return Identificadores.Count() + " Funcionário(s)";
                    default:
                        return "";
                }
            } }
    }
}