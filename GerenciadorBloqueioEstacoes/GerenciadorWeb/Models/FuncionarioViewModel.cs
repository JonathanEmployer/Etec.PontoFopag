using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GerenciadorWeb.Models
{
    public class FuncionarioViewModel
    {
        public int? ID { get; set; }
        public string CPF { get; set; }
        public string NomeUsuario { get; set; }
        public string NomeFuncionario { get; set; }
        public string Matricula { get; set; }
        public string Descricao
        {
            get
            {
                return string.Format("{0} - {1} - {2}", CPF, NomeFuncionario, Matricula);
            }
        }
    }
}