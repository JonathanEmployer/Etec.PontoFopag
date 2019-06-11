using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Modelo;
using Modelo.Proxy;
using Modelo.Relatorios;

namespace BLL.Relatorios.V2
{
    public class RelatorioHorasExtrasLocalBLL : RelatorioBaseBLL, IRelatorioBLL
    {
        public RelatorioHorasExtrasLocalBLL(IRelatorioModel relatorioFiltro, Modelo.UsuarioPontoWeb usuario, ProgressBar progressBar) : base(relatorioFiltro, usuario, progressBar)
        {
        }

        protected override string GetRelatorioExcel()
        {
            RelatorioHorasExtrasLocalModel parms = ((RelatorioHorasExtrasLocalModel)_relatorioFiltro);
            _progressBar.setaMensagem("Carregando dados...");
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(_usuario.ConnectionString, _usuario);

            _progressBar.setaMensagem("Carregando dados de " + parms.InicioPeriodo.ToShortDateString() + " a " + parms.FimPeriodo.ToShortDateString() + " para " + parms.IdSelecionados.Split(',').Count().ToString() + " Funcionário(s)");
            DataTable dtMarcacoes = bllMarcacao.GetRelatorioObras(parms.IdSelecionados, parms.InicioPeriodo, parms.FimPeriodo, parms.idSelecionados2);
            List<Int64> lIndex = new List<Int64>();
            int index = 0;
            foreach (DataRow dr in dtMarcacoes.Rows)
            {
                if ((Convert.ToInt32(dr["contFunc"])) == 1 && index > 0)
                {
                    lIndex.Add(index);
                }
                index++;
            }
            foreach (DataColumn col in dtMarcacoes.Columns)
            {
                col.AllowDBNull = true;
            }

            _progressBar.setaMensagem("Carregado " + dtMarcacoes.Rows.Count.ToString() + " Registros");
            BLL.HoraExtra HE = new BLL.HoraExtra(dtMarcacoes);
            _progressBar.setaMensagem("Calculando Horas Extras de " + dtMarcacoes.Rows.Count.ToString() + " Registros");
            IList<HorasExtrasPorDia> horasExtrasDoPeriodo = HE.CalcularHoraExtraDiaria();
            IList<string> ColunasAddDinamic = new List<string>();
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

            _progressBar.setaMinMaxPB(0, dtMarcacoes.Rows.Count);
            int cont = 1;
            foreach (DataRow dr in dtMarcacoes.Rows) // Adiciona os valores nos percentuais
            {
                DateTime dataMarc = Convert.ToDateTime(dr["data"]);
                int idFuncionario = Convert.ToInt32(dr["idFuncionario"]);
                _progressBar.incrementaPB(1);
                _progressBar.setaMensagem("Atribuindo Hora Extra do dia  " + dataMarc.ToShortDateString() + " do funcionário " + dr["nome"].ToString());
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

            index = 0;
            _progressBar.setaMensagem("Atribuindo separadores entre os funcionários");
            _progressBar.setaMinMaxPB(0, lIndex.Count);
            foreach (int ind in lIndex)
            {
                DataRow dr = dtMarcacoes.NewRow();
                dtMarcacoes.Rows.InsertAt(dr, ind + index);
                index++;
                _progressBar.incrementaPB(1);
            }

            _progressBar.setaMensagem("Gerando Arquivo...");
            // Cria o Dicionario das Colunas do Excel a ser gerado do relatório
            Dictionary<string, GerarExcel.Modelo.Coluna> colunasExcel = new Dictionary<string, GerarExcel.Modelo.Coluna>();
            colunasExcel.Add("obra", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.NUMERO, NomeColuna = "Obra", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("matricula", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Matrícula", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("nome", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Nome", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("funcao", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Função", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("departamento", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Departamento", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("data", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.DATA, NomeColuna = "Data", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("dia", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Dia", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("ocorrencia", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ocorrência", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("entrada_1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 01", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("saida_1", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 01", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("entrada_2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 02", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("saida_2", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 02", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("entrada_3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 03", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("saida_3", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 03", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("entrada_4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 04", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("saida_4", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 04", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("entrada_5", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 05", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("saida_5", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 05", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("entrada_6", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 06", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("saida_6", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 06", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("entrada_7", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 07", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("saida_7", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 07", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("entrada_8", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Ent. 08", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("saida_8", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Saí. 08", Visivel = true, NomeColunaNegrito = true });
            colunasExcel.Add("HorasTrabalhadas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "H. Trabalhadas", Visivel = true, NomeColunaNegrito = true });
            foreach (string nomeColuna in ColunasAddDinamic)
            {
                colunasExcel.Add(nomeColuna, new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = nomeColuna, Visivel = true, NomeColunaNegrito = true });
            }
            colunasExcel.Add("totalHorasTrabalhadas", new GerarExcel.Modelo.Coluna() { Formato = GerarExcel.Modelo.PadraoFormatacaoExcel.TEXTO, NomeColuna = "Total", Visivel = true, NomeColunaNegrito = true });

            byte[] Arquivo = GerarExcel.GerarExcel.Gerar(colunasExcel, dtMarcacoes);
            string nomeDoArquivo = "Relatório_Horas_Extras_Local" + parms.InicioPeriodo.ToString("ddMMyyyy") + "_" + parms.FimPeriodo.ToString("ddMMyyyy");
            ParametrosReportExcel p = new ParametrosReportExcel() { NomeArquivo = nomeDoArquivo, TipoArquivo = Enumeradores.TipoArquivo.Excel, RenderedBytes = Arquivo };
            return base.GerarArquivoExcel(p);
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
