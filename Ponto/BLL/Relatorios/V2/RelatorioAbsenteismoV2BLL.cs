using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioAbsenteismoV2BLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioAbsenteismoV2BLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
            _progressBar.setaMensagem("Carregando dados");
            _progressBar.setaValorPB(-1);
            List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(s => Convert.ToInt32(s)).ToList();
            BLL.Relatorios.RelatorioAbsenteismoV2 bllRelatorioAbsenteismoV2 = new BLL.Relatorios.RelatorioAbsenteismoV2(_usuario.ConnectionString);
            DataTable dt = bllRelatorioAbsenteismoV2.GetDados(parms.IdSelecionados.Split(',').Select(s => Convert.ToInt32(s)).ToList(), parms.InicioPeriodo, parms.FimPeriodo);
            _progressBar.setaMensagem("Gerando Relatório");

            byte[] Arquivo = bllRelatorioAbsenteismoV2.GetRelatorio(dt);
            string nomeDoArquivo = "Relatório_Absenteismo_Modelo_2_" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = Arquivo };
            string caminho = base.GerarArquivoExcel(p);
            return caminho;

        }

        protected override string GetRelatorioHTML()
        {
            throw new NotImplementedException();
        }

        protected override string GetRelatorioPDF()
        {
            throw new NotImplementedException();
        }
    }
}
