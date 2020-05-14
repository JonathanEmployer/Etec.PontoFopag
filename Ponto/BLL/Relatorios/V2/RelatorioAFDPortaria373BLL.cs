using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using GerarExcel.Modelo;
using Modelo;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioAFDPortaria373BLL : RelatorioBaseBLL
	{
		public RelatorioAFDPortaria373BLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
            RelatorioAfdPortaria373Model parms = ((RelatorioAfdPortaria373Model)relatorioFiltro);
			parms.NomeArquivo = "AfdPortaria373" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
		}

		protected override string GetRelatorioExcel()
		{

			DataTable dados = GetDados();

			dados.TableName = "AFD";
			foreach (DataColumn col in dados.Columns)
			{
				col.AllowDBNull = true;
			}


			_progressBar.setaMensagem("Gerando Arquivo...");
			// Cria o Dicionario das Colunas do Excel a ser gerado do relatório
			Dictionary<string, Coluna> colunasExcel = new Dictionary<string, Coluna>();
			colunasExcel.Add("PIS", new Coluna() { Formato = PadraoFormatacaoExcel.TEXTO, NomeColuna = "PIS", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Data", new Coluna() { Formato = PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Hora", new Coluna() { Formato = PadraoFormatacaoExcel.HORA2, NomeColuna = "Hora", Visivel = true, NomeColunaNegrito = true });

			byte[] arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dados);
			ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = ((RelatorioAfdPortaria373Model)_relatorioFiltro).NomeArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = arquivo };
			string caminho = base.GerarArquivoExcel(p);
			return caminho;
		}

		protected override string GetRelatorioHTML()
		{
			throw new NotImplementedException();
		}

		protected override string GetRelatorioPDF()
		{
            RelatorioAfdPortaria373Model parms = ((RelatorioAfdPortaria373Model)_relatorioFiltro);
			if (!Directory.Exists(PathRelatorios))
			{
				Directory.CreateDirectory(PathRelatorios);
			}
			string caminho = Path.Combine(PathRelatorios, parms.NomeArquivo + "." + "txt");
			
			Empresa bllEmpresa = new Empresa(_usuario.ConnectionString);
			Modelo.Empresa empresa = bllEmpresa.LoadObject(parms.lIdEmpAndNumRep.FirstOrDefault().Key);
			DataTable dados = GetDados();
            using (TextWriter tw = new StreamWriter(caminho))
            {
                if (dados.Rows.Count > 0)
                {
                    if (parms.GerarCabecalhoRodape)
                    {
                        string tipoIdentificador = String.IsNullOrEmpty(empresa.Cnpj) ? "2" : "1";
                        string documento = empresa.CnpjCpf.Replace(".", "").Replace("-", "").Replace("/", "");
                        string numSerie = documento.PadLeft(17, '0');
                        documento = documento.PadLeft(14, '0');
                        string cei = empresa.CEI.PadLeft(12, '0');
                        string razaoSocial = empresa.Nome.PadRight(150, ' ');
                        DateTime dtIni = Convert.ToDateTime(dados.AsEnumerable().Min(row => row["Data"]));
                        DateTime dtFim = Convert.ToDateTime(dados.AsEnumerable().Max(row => row["Data"]));
                        tw.WriteLine($"0000000001{tipoIdentificador}{documento}{cei}{razaoSocial}{numSerie}{dtIni.ToString("ddMMyyyy")}{dtFim.ToString("ddMMyyyy")}{DateTime.Now.ToString("ddMMyyyy")}{DateTime.Now.ToString("HHmm")}"); 
                    }
                    for (int i = 0; i < dados.Rows.Count; i++)
                    {
                        DataRow row = dados.Rows[i];
                        DateTime dataHoraReg = Convert.ToDateTime(Convert.ToDateTime(row["data"].ToString()).ToString("dd/MM/yyyy") + " " + row["hora"].ToString());
                        string linha = string.Format("{0}{1}{2}{3}{4}",
                            row["nsr"].ToString().PadLeft(9, '0'),
                            row["TipoReg"].ToString(),
                            dataHoraReg.ToString("ddMMyyyy"),
                            dataHoraReg.ToString("HHmm"),
                            row["pis"].ToString().PadLeft(12, '0')
                            );

                        tw.WriteLine(linha);
                    }
                    if (parms.GerarCabecalhoRodape)
                    {
                        string qtdRegistros3 = dados.Rows.Count.ToString().PadLeft(9, '0');
                        tw.WriteLine($"999999999000000000{qtdRegistros3}0000000000000000009"); 
                    }
				}
            }
            return caminho;
        }

		public DataTable GetDados()
		{
            RelatorioAfdPortaria373Model parms = ((RelatorioAfdPortaria373Model)_relatorioFiltro);
			_progressBar.setaMensagem("Carregando dados de " + parms.InicioPeriodo.ToShortDateString() + " a " + parms.FimPeriodo.ToShortDateString());
			_progressBar.setaValorPB(-1);
            RelatorioBilhetesRep bllRel = new RelatorioBilhetesRep(_usuario.ConnectionString);
			DataTable dtRelatorioBilhetesRep = bllRel.GetRelatorioAFDPortaria373(parms.lIdEmpAndNumRep, parms.InicioPeriodo, parms.FimPeriodo);
			return dtRelatorioBilhetesRep;

		}
	}
}
