using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioGradeHorarioMovelBLL : RelatorioBaseBLL
    {
        public RelatorioGradeHorarioMovelBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
            RelatorioPadraoModel parms = ((RelatorioPadraoModel)relatorioFiltro);
            parms.NomeArquivo = "Relatorio_Grade_Horaria_Flexivel" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
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

        protected ParametrosReportHTML ParametrosHTML()
        {
            return new ParametrosReportHTML()
            {
                Dados = GetDados(),
                NomeArquivo = ((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo,
                ResourceName = "BLL.Relatorios.V2.cshtml.RelatorioHorarioMovelHtml.cshtml"
            };
        }

        private IList<Modelo.Proxy.PxyHorarioMovel> GetDados()
        {
            _progressBar.setaMensagem("Carregando dados...");
            _progressBar.setaValorPB(-1);
            BLL.HorarioDetalhe bllHorariODetalhe = new BLL.HorarioDetalhe(_usuario.ConnectionString, _usuario);
            _progressBar.setaMensagem("Agrupando dados...");

            RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
            IList<Modelo.Proxy.PxyHorarioMovel> listaPxyHorarioMovel = bllHorariODetalhe.GetPxyGradeHorario(Convert.ToInt32(parms.IdSelecionados.Split(',')[0]), parms.InicioPeriodo, parms.FimPeriodo);

            return listaPxyHorarioMovel;
        }
    }
}
