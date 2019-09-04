using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;
using RazorEngine;
using RazorEngine.Templating;

namespace BLL.Relatorios.V2
{
    public class RelatorioCartaoPontoHTMLBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioCartaoPontoHTMLBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
            RelatorioCartaoPontoHTMLModel parms = ((RelatorioCartaoPontoHTMLModel)relatorioFiltro);
            parms.NomeArquivo = "Relatorio_Cartao_Ponto_HTML" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
        }

        protected override string GetRelatorioExcel()
        {
            throw new NotImplementedException();
        }

        protected override string GetRelatorioHTML()
        {
            return GerarArquivoHTML(ParametrosHTML());
        }

        protected override string GetRelatorioPDF()
        {
            return GerarArquivoPdfBaseHTML(ParametrosHTML());
        }

        private IList<pxyCartaoPontoEmployer> GetDados()
        {
            _progressBar.setaMensagem("Carregando dados...");
            _progressBar.setaValorPB(-1);
            BLL.CartaoPontoV2 bllCartaoPonto = new BLL.CartaoPontoV2(_usuario.ConnectionString, _usuario);
            RelatorioCartaoPontoHTMLModel parms = (RelatorioCartaoPontoHTMLModel)_relatorioFiltro;
            IList<int> funcs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

            _progressBar.setaMensagem("Agrupando dados...");
            IList<pxyCartaoPontoEmployer> cps = bllCartaoPonto.BuscaDadosRelatorio(funcs, parms.InicioPeriodo, parms.FimPeriodo, _progressBar, parms.OrdemRelatorio, parms.quebraAuto);
            return cps;
        }

        protected ParametrosReportHTML ParametrosHTML()
        {
            return new ParametrosReportHTML()
            {
                Dados = GetDados(),
                NomeArquivo = ((RelatorioCartaoPontoHTMLModel)_relatorioFiltro).NomeArquivo,
                ResourceName = "BLL.Relatorios.V2.cshtml.CartaoPontoHtml.cshtml"
            };
        }
    }
}
