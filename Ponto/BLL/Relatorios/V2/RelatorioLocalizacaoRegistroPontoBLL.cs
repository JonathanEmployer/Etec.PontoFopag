using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using BLL.Util;
using Modelo;
using Modelo.Relatorios;
using RazorEngine;
using RazorEngine.Templating;

namespace BLL.Relatorios.V2
{
    public class RelatorioLocalizacaoRegistroPontoBLL : RelatorioBaseBLL
    {
        public RelatorioLocalizacaoRegistroPontoBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
            RelatorioPadraoModel parms = ((RelatorioPadraoModel)relatorioFiltro);
            parms.NomeArquivo = "Relatorio_Localizacao_Registro_Ponto" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
        }

        protected override string GetRelatorioExcel()
        {
            ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
            Byte[] arquivo = objToExcel.ObjectToExcel("LocalizacaoRegistro", GetDados().ToList());
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = ((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
            string caminho = base.GerarArquivoExcel(p);
            return caminho;
        }

        private IList<Modelo.Proxy.Relatorios.PxyRelLocalizacaoRegistroPonto> GetDados()
        {
            _progressBar.setaMensagem("Carregando dados...");
            _progressBar.setaValorPB(-1);
            BLL.LocalizacaoRegistroPonto bllLocalizacaoRegistroPonto = new BLL.LocalizacaoRegistroPonto(_usuario.ConnectionString, _usuario);
            RelatorioPadraoModel parms = (RelatorioPadraoModel)_relatorioFiltro;
            IList<int> funcs = parms.IdSelecionados.Split(',').Where(w => !String.IsNullOrEmpty(w)).Select(s => int.Parse(s)).ToList();

            _progressBar.setaMensagem("Agrupando dados...");
            IList<Modelo.Proxy.Relatorios.PxyRelLocalizacaoRegistroPonto> LocRegPonto = bllLocalizacaoRegistroPonto.RelLocalizacaoRegistroPonto(funcs.ToList(), parms.InicioPeriodo, parms.FimPeriodo);
            return LocRegPonto;
        }

        protected override string GetRelatorioHTML()
        {
            return GerarArquivoHTML(ParametrosHTML());
        }

        protected override string GetRelatorioPDF()
        {
            return GerarArquivoPdfBaseHTML(ParametrosHTML());
        }

        protected ParametrosReportHTML ParametrosHTML()
        {
            return new ParametrosReportHTML()
            {
                Dados = GetDados(),
                NomeArquivo = ((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo,
                ResourceName = "BLL.Relatorios.V2.cshtml.RelatorioLocalizacaoRegistroPontoHtml.cshtml"
            };
        }
    }
}
