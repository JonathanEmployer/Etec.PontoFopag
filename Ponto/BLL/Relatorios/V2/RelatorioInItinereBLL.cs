using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using BLL.Util;
using Microsoft.Reporting.WebForms;
using Modelo;
using Modelo.Proxy.Relatorios;
using Modelo.Relatorios;
using RazorEngine;

namespace BLL.Relatorios.V2
{
	public class RelatorioInItinereBLL : RelatorioBaseBLL, IRelatorioBLL
	{
		public RelatorioInItinereBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
            RelatorioHorasInItinereModel parms = ((RelatorioHorasInItinereModel)_relatorioFiltro);
			((RelatorioHorasInItinereModel)_relatorioFiltro).NomeArquivo = "Relatório_de_Horas_In_Itinere" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
		}

		protected override string GetRelatorioExcel()
		{
			throw new NotImplementedException();

		}

		private IList<Modelo.Proxy.Relatorios.PxyRelatorioHorasInItinere> GetDados()
		{
			_progressBar.setaValorPBCMensagem(-1, "Carregando dados...");
            BLL.HorasInItinere bllHorasInItinere = new BLL.HorasInItinere(_usuario.ConnectionString, _usuario);
            RelatorioHorasInItinereModel parms = (RelatorioHorasInItinereModel)_relatorioFiltro;
            IList<int> funcs = parms.IdSelecionados.Split(',').Select(int.Parse).ToList();

            _progressBar.setaMensagem("Agrupando dados...");
            IList<PxyRelatorioHorasInItinere> rels = bllHorasInItinere.BuscaDadosRelatorio(funcs, parms.InicioPeriodo, parms.FimPeriodo, _progressBar);

            return rels;
		}
		
		protected override string GetRelatorioPDF()
		{
            IList<string> htmls = GetHtml();
            _progressBar.setaValorPBCMensagem(-1,"Renderizando " + htmls.Count() + " páginas.");
            int part = 0;
            ConcurrentBag<RelatorioParts> relatorios = new ConcurrentBag<RelatorioParts>();
            Parallel.ForEach(htmls, (ht) =>
            {
                part++;
                RelatorioParts cpb = new RelatorioParts();
                cpb.Parte = part;
                byte[] buffer = GerarArquivoPdfBaseHTML(ht);
                cpb.Arquivo = buffer;
                relatorios.Add(cpb);
            });
            _progressBar.setaValorPBCMensagem(-1, "Agrupando " + htmls.Count() + " páginas.");
            HtmlReport htmlReport = new HtmlReport();

            byte[] arquivo = htmlReport.MergeFiles(relatorios.OrderBy(o => o.Parte).Select(s => s.Arquivo).ToList(), true, false);
            string caminho = SaveFile(((RelatorioHorasInItinereModel)_relatorioFiltro).NomeArquivo, "pdf", arquivo);
            return caminho;
        }

		protected override string GetRelatorioHTML()
        {
            IList<string> htmls = GetHtml();
            string caminho = SaveFile(((RelatorioHorasInItinereModel)_relatorioFiltro).NomeArquivo, "html", System.Text.Encoding.UTF8.GetBytes(String.Join(String.Empty, htmls.ToArray())));
            return caminho;
        }

        private IList<string> GetHtml()
        {
            IList<PxyRelatorioHorasInItinere> rels = GetDados();

            ConcurrentBag<RelatorioParts> relatorios = new ConcurrentBag<RelatorioParts>();
            _progressBar.setaValorPBCMensagem(-1, "Criando " + rels.Count() + " páginas.");
            IList<List<PxyRelatorioHorasInItinere>> partes = rels.GroupBy(x => new { x.PxyFuncionarioCabecalhoRel.EmpresaNome, x.PxyFuncionarioCabecalhoRel.EmpresaCNPJCPF }).Select(s => s.ToList()).ToList();

            HtmlReport htmlReport = new HtmlReport();

            IList<string> htmls = new List<string>();
            foreach (List<PxyRelatorioHorasInItinere> parte in partes)
            {
                ParametrosReportHTML parteParm = new ParametrosReportHTML()
                {
                    Dados = parte,
                    NomeArquivo = ((RelatorioHorasInItinereModel)_relatorioFiltro).NomeArquivo,
                    ResourceName = "BLL.Relatorios.V2.cshtml.RelatorioHorasInItinere.cshtml"
                };
                string htmlText = GerarStringHTML(parteParm);
                htmls.Add(htmlText);
            }

            if (htmls == null || htmls.Count() == 0)
            {
                htmls.Add("<html><body> <div align='center'> <h3> Não há dados para compor o relatório </h3> </div> </body></html>");
            }

            return htmls;
        }

        protected ParametrosReportHTML ParametrosHTML()
		{
			return new ParametrosReportHTML()
			{
				Dados = GetDados(),
				NomeArquivo = ((RelatorioHorasInItinereModel)_relatorioFiltro).NomeArquivo,
				ResourceName = "BLL.Relatorios.V2.cshtml.RelatorioHorasInItinere.cshtml"
			};
		}
	}

}
