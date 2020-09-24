using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
	public class RelatorioRegistrosBLL : RelatorioBaseBLL, IRelatorioBLL
	{
		public RelatorioRegistrosBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
		{
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			((RelatorioPadraoModel)_relatorioFiltro).NomeArquivo = "Relatório_de _Registros" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
		}
          
        protected override string GetRelatorioExcel()
		{
			BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usuario.ConnectionString, _usuario);
			RelatorioPadraoModel parms = ((RelatorioPadraoModel)_relatorioFiltro);
			DataTable dtRelatorio = new DataTable();
			IList<string> ColunasAddDinamic = new List<string>();
			_progressBar.setaMensagem("Carregando dados de " + parms.InicioPeriodo.ToShortDateString() + " a " + parms.FimPeriodo.ToShortDateString() + " para " + parms.IdSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
			_progressBar.setaMinMaxPB(0, 1);
			_progressBar.setaValorPB(1);
			List<int> idsFuncs = parms.IdSelecionados.Split(',').Select(s => Convert.ToInt32(s)).ToList();
			int partes = idsFuncs.Count();
			if (partes >= 3)
			{
				partes = idsFuncs.Count() / 3;
			}
			ConcurrentBag<DataTable> PedacosRelatorio = new ConcurrentBag<DataTable>();
			IList<List<int>> idsParciais = BLL.cwkFuncoes.SplitList(idsFuncs, partes);
			Parallel.ForEach(idsParciais,
			ids =>
			{
				try
				{
					PedacosRelatorio.Add(bllMarcacao.GetRelatorioRegistros(String.Join(",", ids), parms.InicioPeriodo, parms.FimPeriodo));
				}
				catch (Exception e)
				{

					throw e;
				}
			});

			_progressBar.setaMensagem("Calculando Horas Extras");
			_progressBar.setaMinMaxPB(0, 1);
			_progressBar.setaValorPB(1);
			BLL.HoraExtra.ValidaHorariosDiferenteDiarioMensal(_usuario.ConnectionString, _usuario, PedacosRelatorio.ToList());

			Parallel.ForEach(PedacosRelatorio,
				dtMarcacoes =>
				{
					try
					{
						BLL.HoraExtra HE = new BLL.HoraExtra(dtMarcacoes);
						IList<HorasExtrasPorDia> horasExtrasDoPeriodo = HE.CalcularHoraExtraDiaria();

						//Pegas os percentuais existentes
						IList<Modelo.Proxy.HoraExtra> heMS = horasExtrasDoPeriodo.SelectMany(x => x.HorasExtras).ToList();
						var TotalHoras = heMS.GroupBy(l => l.Percentual)
										.Select(lg =>
											new
											{
												Percentual = lg.Key
											}).OrderBy(x => x.Percentual);

						foreach (var horasExtras in TotalHoras) // Adiciona os percentuais existentes como coluna no datatable
						{
							string nomeColuna = "Extras " + horasExtras.Percentual + "%";
							dtMarcacoes.Columns.Add(nomeColuna, typeof(System.String));
							ColunasAddDinamic.Add(nomeColuna);
						}

						foreach (DataRow dr in dtMarcacoes.Rows) // Adiciona os valores nos percentuais
						{
							DateTime dataMarc = Convert.ToDateTime(dr["data"]);
							int idFuncionario = Convert.ToInt32(dr["idFuncionario"]);
							//Busca as horas extras do dia do funcionário
							IList<Modelo.Proxy.HoraExtra> HEDiaFuncionario = horasExtrasDoPeriodo.Where(x => x.IdFuncionario == idFuncionario && x.DataMarcacao == dataMarc).SelectMany(x => x.HorasExtras).ToList();
							var horasExtrasFuncDia = HEDiaFuncionario.GroupBy(l => l.Percentual)
											.Select(lg =>
												new
												{
													Percentual = lg.Key,
													HoraDiurna = lg.Sum(w => w.HoraDiurna),
													HoraNoturna = lg.Sum(w => w.HoraNoturna)
												}).OrderBy(x => x.Percentual);

							foreach (var item in horasExtrasFuncDia)// Adiciona os percentuais nas respectivas colunas
							{
								string nomeColuna = "Extras " + item.Percentual + "%";
								dr[nomeColuna] = Modelo.cwkFuncoes.ConvertMinutosHora(item.HoraDiurna + item.HoraNoturna).Replace("--:--", "");
							}
						}
						lock (dtRelatorio)
						{
							dtRelatorio.Merge(dtMarcacoes);
						}
					}
					catch (Exception e)
					{

						throw e;
					}
				});

			_progressBar.setaMensagem("Ordenando Dados");
			_progressBar.setaMinMaxPB(0, 1);
			_progressBar.setaValorPB(1);
			DataView dtV = dtRelatorio.DefaultView;
			dtV.Sort = "nome,dataSemFormat,Matrícula";
			dtRelatorio = dtV.ToTable();

            dtRelatorio.Columns.Add("Hra_BH_Positivas", typeof(String));
            dtRelatorio.Columns.Add("Hra_BH_Negativas", typeof(String));
            dtRelatorio.Columns.Add("DataFormatada", typeof(String));

            foreach (DataRow row in dtRelatorio.Rows)
            {
                row["DataFormatada"] = Convert.ToDateTime((row["Data"]).ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture); ;
                if (row["Hra_Banco_Horas"].ToString().Contains("-"))
                {
                    row["Hra_BH_Negativas"] = row["Hra_Banco_Horas"].ToString().Replace("-", "");
                }
                else if (!(row["Hra_Banco_Horas"].ToString().Contains("-")))
                {
                    if (row["Hra_Banco_Horas"].ToString().Contains("0:00"))
                    {
                        row["Hra_BH_Positivas"] = row["Hra_Banco_Horas"];
                        row["Hra_BH_Negativas"] = row["Hra_Banco_Horas"];
                    }
                    else
                    {
                        row["Hra_BH_Positivas"] = row["Hra_Banco_Horas"];
                    }
                        
                }
            }

            _progressBar.setaMensagem("Gerando Arquivo...");
			// Cria o Dicionario das Colunas do Excel a ser gerado do relatório
			Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
			colunasExcel.Add("DataFormatada", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Dia", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Dia", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Nome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Matrícula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Alocação", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Alocação", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
			//colunasExcel.Add("Contrato", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Contrato", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Função", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Jornada", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Jornada", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Ent1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 01", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Sai1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 01", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Ent2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 02", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Sai2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 02", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Ent3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 03", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Sai3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 03", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Ent4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 04", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Sai4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 04", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Ent5", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 05", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Sai5", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 05", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Ent6", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 06", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Sai6", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 06", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Ent7", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 07", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Sai7", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 07", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Ent8", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ent. 08", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Sai8", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Saí. 08", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Desconsideradas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Desconsideradas", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("H. Diurnas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "H. Diurnas", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("H. Noturnas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "H. Noturnas", Visivel = true, NomeColunaNegrito = true });
			ColunasAddDinamic = ColunasAddDinamic.Distinct().ToList();
			try
			{
				ColunasAddDinamic = ColunasAddDinamic.OrderBy(o => Convert.ToInt32(o.Replace("Horas ", "").Replace("%", ""))).ToList();
			}
			catch (Exception)
			{

			}
			foreach (string nomeColuna in ColunasAddDinamic.Distinct())
			{
				colunasExcel.Add(nomeColuna, new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = nomeColuna, Visivel = true, NomeColunaNegrito = true });
			}
			colunasExcel.Add("Ad. Noturno", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Ad. Noturno", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Dsr", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Dsr", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("horasfaltadiurna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Faltas Diu", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("horasfaltanoturna", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Faltas Not", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Créd. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Créd. BH", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Déb. BH", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Déb. BH", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Hra_BH_Positivas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA3, NomeColuna = "Saldo BH positivo", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("Hra_BH_Negativas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA3, NomeColuna = "Saldo BH negativo", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Total", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.HORA2, NomeColuna = "Total", Visivel = true, NomeColunaNegrito = true });
			colunasExcel.Add("Ocorrência", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ocorrência", Visivel = true, NomeColunaNegrito = true });

			byte[] Arquivo = null;
			Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtRelatorio);
			string nomear = "Relatório_de _Registros" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
			ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomear, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = Arquivo };
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
