using System;

namespace Modelo
{
    public class OcorrenciaEmpresa : Modelo.ModeloBase
    {
        public OcorrenciaEmpresa()
        {

        }

        public OcorrenciaEmpresa(int pIdOcorrencia, int pIdEmpresa)
        {
            this.idOcorrencia = pIdOcorrencia;
            this.idEmpresa = pIdEmpresa;
        }

        [TableHTMLAttribute("Código", 1, true, ItensSearch.text, OrderType.none)]
        public int CodigoGrid { get { return this.Codigo; } }

        public Boolean Selecionado { get; set; }
        [TableHTMLAttribute("Descrição", 2, true, ItensSearch.text, OrderType.asc)]
        public String Descricao { get; set; }

        public int idEmpresa { get; set; }

        public int idOcorrencia { get; set; }
    }
}
