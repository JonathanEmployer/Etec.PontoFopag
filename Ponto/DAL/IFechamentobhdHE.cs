using System;
using System.Data;
using System.Collections.Generic;
using Modelo;

namespace DAL
{
    public interface IFechamentobhdHE : DAL.IDAL
    {
        Modelo.FechamentobhdHE LoadObject(int id);
        void Incluir(List<Modelo.FechamentobhdHE> lista);
        List<Modelo.Proxy.pxyPessoaMarcacaoParaRateio> GetPessoaMarcacaoParaRateio(int pTipo, String pIdTipo, DateTime dataInicial, DateTime dataFinal);
        IList<Modelo.FechamentobhdHE> GetFechamentobhdHEPorIdFechamentoBH(int idFechamentoBH, int identificacao);
        IList<Modelo.FechamentobhdHE> GetAllList();
        DataTable GetRelatorioFechamentoPercentualHESintetico(string idSelecionados, DateTime inicioPeriodo, DateTime fimPeriodo);
        DataTable GetRelatorioFechamentoPercentualHEAnalitico(string idSelecionados, DateTime inicioPeriodo, DateTime fimPeriodo);
    }
}
