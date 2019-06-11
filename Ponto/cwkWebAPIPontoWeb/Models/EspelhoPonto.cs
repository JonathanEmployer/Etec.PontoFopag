using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwkWebAPIPontoWeb.Models
{
    public class EspelhoPonto
    {
        public List<Marcacao> ListaMarcacao { get; set; }
        public TotalizadorEspelhoPonto TotalizadorEspelhoPonto { get; set; }
        public bool ClassificarHorasExtras { get; set; }
        public List<Models.Jornada> JornadasTrabalhadas { get; set; }
        public bool PermiteAprovar { get {
            bool retorno = true;
            if (!PermiteAprovarMarcacaoIncorreta && PossuiMarcacaoIncorreta)
            {
                retorno = false;
            }
            return retorno;
        } }
        public bool PossuiMarcacaoIncorreta { get { return ListaMarcacao.Where(w => w.MarcacaoIncorreta).Count() > 0 ? true : false; } }
        public bool PermiteAprovarMarcacaoIncorreta  { get; set; }
        public List<Bilhetes> BilhetesTratados { get; set; }
        public bool BloqueiaJustificativaForaPeriodo { get; set; }
        public int DtInicioJustificativa { get; set; }
        public int DtFimJustificativa { get; set; }
        public List<PeriodoFerias> PeriodosFerias { get; set; }
        public List<Feriados> Feriados { get; set; }
        public bool PermiteAbonoParcialPainel { get; set; }
        public bool LimitarQtdAbono { get; set; }
    }

}