using Modelo.Proxy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RegistroClassificacao = Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras;
using System.Collections.Concurrent;
using BLL.Util;

namespace BLL.Relatorios
{
    public class RelClassHorasExtras2
    {

        private Modelo.UsuarioPontoWeb UsuarioLogado;
        private string HtmlParcial;

        public RelClassHorasExtras2(Modelo.UsuarioPontoWeb usuario)
        {
            this.UsuarioLogado = usuario;
            string dll = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "bin", "BLL_N.dll");
            Assembly assembly = Assembly.LoadFrom(dll);
            Stream stream = assembly.GetManifestResourceStream("BLL_N.Relatorios.View.RelClassHorasExtrasHtml.cshtml");
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            HtmlParcial = sr.ReadToEnd();
        }

        public IList<RegistroClassificacao> GetDadosRelatorio(List<int> idsFuncionarios, DateTime datainicial, DateTime datafinal, int filtroClass)
        {
            BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new BLL.ClassificacaoHorasExtras(UsuarioLogado.ConnectionString, UsuarioLogado);
            IList<Modelo.Proxy.Relatorios.PxyRelClassificacaoHorasExtras> listRel = bllClassificacaoHorasExtras.RelatorioClassificacaoHorasExtras(idsFuncionarios, datainicial, datafinal, filtroClass);
            return listRel;
        }

        public IList<RegistroClassificacao> GetDadosRelatorio(List<string> cpfsFuncionarios, DateTime datainicial, DateTime datafinal, int filtroClass)
        {
            BLL.ClassificacaoHorasExtras bllClassificacaoHorasExtras = new BLL.ClassificacaoHorasExtras(UsuarioLogado.ConnectionString, UsuarioLogado);
            IList<RegistroClassificacao> listRel = bllClassificacaoHorasExtras.RelatorioClassificacaoHorasExtras(cpfsFuncionarios, datainicial, datafinal, filtroClass);
            return listRel;
        }

        public PxyFileResult GerarRelatorio(List<RegistroClassificacao> dados, string formato)
        {
            switch (formato)
            {
                case "Excel":
                    return GerarRelatorioExcel(dados);
                case "PDF":
                    return GerarRelatorioPdf(dados);
                default:
                    return GerarRelatorioHtml(dados);
            }
        }

        public PxyFileResult GerarRelatorioExcel(List<RegistroClassificacao> dados)
        {
            PxyFileResult arquivo = new PxyFileResult();
            ListaObjetosToExcel objToExcel = new ListaObjetosToExcel();
            arquivo.Arquivo = objToExcel.ObjectToExcel("ClassificacaoHorasExtras", dados.ToList());
            arquivo.MimeType = "application/vnd.ms-excel";
            arquivo.FileName = GetNomeArquivo("xls");
            return arquivo;
        }

        public PxyFileResult GerarRelatorioHtml(List<RegistroClassificacao> dados)
        {
            return GerarRelatorioHtml(dados.GroupBy(g => new { g.EmpresaNome, g.EmpresaCNPJ }).Select(s => s.ToList()).ToList());
        }

        public PxyFileResult GerarRelatorioHtml(List<List<RegistroClassificacao>> dados)
        {
            HtmlReport htmlReport = new HtmlReport();
            List<string> htmls = (from registro in dados select GerarHtmlParcial(registro)).ToList();
            PxyFileResult arquivo = new PxyFileResult();
            arquivo.Arquivo = System.Text.Encoding.UTF8.GetBytes(String.Join(String.Empty, htmls.ToArray()));
            arquivo.MimeType = "text/html";
            arquivo.FileName = GetNomeArquivo("html");
            return arquivo;
        }

        public PxyFileResult GerarRelatorioPdf(List<RegistroClassificacao> dados)
        {
            return GerarRelatorioPdf(dados.GroupBy(g => new { g.EmpresaNome, g.EmpresaCNPJ }).Select(s => s.ToList()).ToList());
        }

        public PxyFileResult GerarRelatorioPdf(List<List<RegistroClassificacao>> dados)
        {

            HtmlReport htmlReport = new HtmlReport();
            int ordem = 0;
            var registrosRender = (from grupo in dados select new { Grupo = grupo, Ordem = ordem++ }).ToList();
            ConcurrentDictionary<int, byte[]> buffers = new ConcurrentDictionary<int, byte[]>();

            Parallel.ForEach(registrosRender, grupo =>
            {
                buffers.TryAdd(grupo.Ordem, htmlReport.RenderPDF(GerarHtmlParcial(grupo.Grupo), false, false));
            });

            PxyFileResult arquivo = new PxyFileResult();
            arquivo.Arquivo = htmlReport.MergeFiles(buffers.OrderBy(o => o.Key).Select(s => s.Value).ToList(), true, true);
            arquivo.MimeType = "application/PDF";
            arquivo.FileName = GetNomeArquivo("PDF");
            return arquivo;

        }

        private string GetNomeArquivo(string extensao)
        {
            return "Classificação Horas Extras" + DateTime.Now.ToString("dd-MM-yyyy-HH_mm") + "." + extensao;
        }

        public string GerarHtmlParcial(List<RegistroClassificacao> grupo)
        {
            return RazorEngine.Razor.Parse(HtmlParcial, grupo);
        }

    }
}
