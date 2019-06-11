using System.Collections.Generic;
using System.Linq;
using BLL.Util;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioClassificacaoHorasExtrasBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioClassificacaoHorasExtrasBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
            RelatorioClassificacaoHorasExtrasModel parms = ((RelatorioClassificacaoHorasExtrasModel)relatorioFiltro);
            parms.NomeArquivo = "Relatorio_Classificacao_Horas_Extras" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
        }

        protected override string GetRelatorioExcel()
        {
            ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
            byte[] Arquivo = objToExcel.ObjectToExcel("ClassificacaoHorasExtras", GetDados().ToList());
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = ((RelatorioClassificacaoHorasExtrasModel)_relatorioFiltro).NomeArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = Arquivo };
            return base.GerarArquivoExcel(p);
        }

        protected override string GetRelatorioHTML()
        {
            return GerarArquivoHTML(ParametrosHTML());
        }

        protected override string GetRelatorioPDF()
        {
            return GerarArquivoPdfBaseHTML(ParametrosHTML());
        }

        private List<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> GetDados()
        {
            _progressBar.setaMensagem("Carregando dados...");
            _progressBar.setaValorPB(-1);
            BLL.Relatorios.RelClassHorasExtras2 bllClassificacaoHorasExtras = new BLL.Relatorios.RelClassHorasExtras2(_usuario);
            RelatorioClassificacaoHorasExtrasModel parms = (RelatorioClassificacaoHorasExtrasModel)_relatorioFiltro;
            IList<int> funcs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

            _progressBar.setaMensagem("Agrupando dados...");
            List<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> listRel = bllClassificacaoHorasExtras.GetDadosRelatorio(funcs.ToList(), parms.InicioPeriodo, parms.FimPeriodo, parms.TipoSelecao).ToList();
            return listRel;
        }

        protected ParametrosReportHTML ParametrosHTML()
        {
            return new ParametrosReportHTML()
            {
                Dados = GetDados(),
                NomeArquivo = ((RelatorioClassificacaoHorasExtrasModel)_relatorioFiltro).NomeArquivo,
                ResourceName = "BLL.Relatorios.V2.cshtml.RelClassHorasExtrasHtml.cshtml"
            };
        }
    }
}
