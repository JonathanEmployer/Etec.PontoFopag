using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Modelo
{
    public class FuncionarioRFID : Modelo.ModeloBase
    {
        [TableHTMLAttribute("RFID", 1, true, ItensSearch.text, OrderType.asc)]
        [Range(0, Int64.MaxValue)]
        public Int64? RFID { get; set; }
        public int IdFuncionario { get; set; }
        public string MIFARE { get; set; }
        public bool Ativo { get; set; }
        //public string Senha { get; set; }
        public Funcionario Funcionario { get; set; }
    }
}
