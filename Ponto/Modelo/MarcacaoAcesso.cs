using System;

namespace Modelo
{
    public class MarcacaoAcesso : ModeloBase
    {
        public int IdFuncionario { get; set; }
        public int Tipo { get; set; }
        public DateTime DtMarcacao { get; set; }
        public int IdEquipamento { get; set; }
        public string Acesso { get; set; }
    }
}
