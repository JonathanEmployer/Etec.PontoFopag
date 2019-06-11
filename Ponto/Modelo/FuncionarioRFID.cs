using System;

namespace Modelo
{
    public class FuncionarioRFID : Modelo.ModeloBase
    {
        [TableHTMLAttribute("RFID", 1, true, ItensSearch.text, OrderType.asc)]
        public Int64? RFID { get; set; }
        public int IdFuncionario { get; set; }

        public Funcionario Funcionario { get; set; }
    }
}
