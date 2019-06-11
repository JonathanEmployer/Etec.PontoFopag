using System.Collections.Generic;
using System.Data;

namespace Modelo
{
    public class LoteMarcacaoProcessar
    {
        public int IdFuncionario { get; set; }
        public string NomeFuncionario { get; set; }
        public DataTable DtMarcacoes { get; set; }
        public List<Modelo.Marcacao> Marcacoes { get; set; }
        public List<Modelo.BilhetesImp> Bilhetes { get; set; }
    }
}
