using DAL.SQL;
using SpreadsheetGear;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BLL.Relatorios
{
    public class RelatorioAbsenteismoV2
    {
        private DAL.RelatoriosSQL.RelatorioAbsenteismoV2 dalRelatorioAbsenteismoV2;
        public RelatorioAbsenteismoV2() : this(null)
        {
        }

        public RelatorioAbsenteismoV2(string conn)
        {
            if (String.IsNullOrEmpty(conn))
            {
                conn = Modelo.cwkGlobal.CONN_STRING;
            }
            DataBase db = new DataBase(conn);
            dalRelatorioAbsenteismoV2 = new DAL.RelatoriosSQL.RelatorioAbsenteismoV2(db);
        }

        public DataTable GetDados(List<int> idsFuncionarios, DateTime dataIni, DateTime dataFin)
        {
            return dalRelatorioAbsenteismoV2.GetRelatorioAbsenteismoV2(idsFuncionarios, dataIni, dataFin);
        }

        public byte[] GetRelatorio(List<int> idsFuncionarios, DateTime dataIni, DateTime dataFin)
        {
            return GetRelatorio(GetDados(idsFuncionarios, dataIni, dataFin));
        }

        public byte[] GetRelatorio(DataTable dados)
        {
            try
            {
                DataTable dt = dados; // Busca os dados do Relatório
                IWorkbook workbook = Factory.GetWorkbook();// Cria o Excel
                IWorksheet worksheet = workbook.Worksheets["Sheet1"];// Pega a referência da primeira aba
                worksheet.Name = "Absenteísmo"; // Altera nome da aba
                IRange cells = worksheet.Cells;// Pega a referencia das celulas da planilha
                cells.ClearOutline(); // Limpa as celulas

                GeraCabecalho(worksheet, cells); //Adiciona o cabeçalho no arquivo, com o nome e formatações
                int linha = 0; // variaval responsavel por controlar as linhas da planilha
                int iniConjunto = 2;
                string departamentoAnt = dt.Rows[0]["Departamento"].ToString(); // Carrega o departamento e quanda para fazer o controle de quando o deparmtamento mudar, utilizado para fazer o somatorio do grupo do departamento
                string departamento = "";
                List<int> linhasTotGrupos = new List<int>(); // Variavel responsável por salvar as linhas que tem quebras (totalizador) para ser somado no final no totalizador total
                GerarLinhas(cells, ref linha, ref iniConjunto, dt, ref departamentoAnt, ref departamento, ref linhasTotGrupos); // Gera as linhas do relatório
                linha++;
                Agrupamento(cells, linha, iniConjunto, dt.Rows[dt.Rows.Count - 1], departamento, ref linhasTotGrupos); // Faz a ultima quebra por departamento
                linha++;
                TotalizadorGeral(cells, linha, dt.Rows[dt.Rows.Count - 1], "Geral", linhasTotGrupos); // Faz o totalizador geral

                cells.Columns.AutoFit(); // Redimensiona o tamanho das colunas
                cells["H1"].ColumnWidth = 10; // Fixa o tamanho da coluna QtdeHorasPrevistas para corrigir problema no redimensionamento

                byte[] Arquivo = null;

                Arquivo = workbook.SaveToMemory(FileFormat.OpenXMLWorkbook); // Gera o arquivo
                return Arquivo;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Gerar Linhas

        /// <summary>
        /// Adiciona o cabeçalho no arquivo, com o nome e formatações
        /// </summary>
        /// <param name="worksheet">Planilha</param>
        /// <param name="cells">Celulas</param>
        private static void GeraCabecalho(IWorksheet worksheet, IRange cells)
        {
            int coluna = 0;
            cells[0, coluna].EntireColumn.NumberFormat = @"* dd/MM/yyyy";
            cells[0, coluna].WrapText = true;
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = "Período Inicial";

            cells[0, coluna].EntireColumn.NumberFormat = @"* dd/MM/yyyy";
            cells[0, coluna].WrapText = true;
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = "Período Final";

            //cells[0, coluna].ColumnWidth = 240;
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = "Nome";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = " Matrícula ";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = "Departamento";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = "Função";


            cells[0, coluna].EntireColumn.NumberFormat = @"* dd/MM/yyyy";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = " Admissão ";


            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = "Pessoa Supervisor";

            cells[0, coluna].EntireColumn.NumberFormat = "[H]:MM:SS;@";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Qtde Horas Previstas";

            cells[0, coluna].EntireColumn.NumberFormat = "[H]:MM:SS;@";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Horas Trabalhadas";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Nº Dias Trab.";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0%";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "% Trab.";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0%";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "% Absent.";

            cells[0, coluna].EntireColumn.NumberFormat = "[H]:MM:SS;@";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = "Hrs Extras";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Nº Ocor. Hrs Extras";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0%";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "% Hrs Extras.";

            cells[0, coluna].EntireColumn.NumberFormat = "[H]:MM:SS;@";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Hrs Abono Legal";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Nº Ocor. Hrs Abono Legal";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0%";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "% Hrs Abono Legal";

            cells[0, coluna].EntireColumn.NumberFormat = "[H]:MM:SS;@";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Hrs Abono Não Legal/Outros";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Nº Ocor. Hrs Abono Não Legal/Outros";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0%";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "% Hrs Abono Não Legal/Outros";

            cells[0, coluna].EntireColumn.NumberFormat = "[H]:MM:SS;@";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = "Faltas";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Nº Ocor. Faltas";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0%";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "% Faltas";

            cells[0, coluna].EntireColumn.NumberFormat = "[H]:MM:SS;@";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Saidas Antecip./Atrasos";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Nº Ocor. Saidas Antecip./Atrasos";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0%";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "% Saidas Antecip./Atrasos";

            cells[0, coluna].EntireColumn.NumberFormat = "[H]:MM:SS;@";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = "Créd B.H.";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Nº Ocor. Créd B.H.";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0%";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "% Créd B.H.";

            cells[0, coluna].EntireColumn.NumberFormat = "[H]:MM:SS;@";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna++].Value = "Déb B.H.";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna++].Value = "Nº Ocor. Déb B.H.";

            cells[0, coluna].EntireColumn.NumberFormat = "* 0%";
            cells[0, coluna].VerticalAlignment = VAlign.Center;
            cells[0, coluna].WrapText = true;
            cells[0, coluna].Value = "%Déb B.H.";

            var cabecalho = cells[0, 0, 0, coluna];
            cabecalho.Interior.Color = Color.FromArgb(54, 96, 146);
            cabecalho.Font.Color = Color.White;
            cabecalho.Font.Size = 12;
            cabecalho.HorizontalAlignment = HAlign.Center;
            cabecalho.Font.Bold = true;
            cabecalho.Borders[BordersIndex.InsideVertical].Weight = BorderWeight.Thin;
            cabecalho.Borders[BordersIndex.InsideVertical].LineStyle = LineStyle.Continous;

            cells[1, 0].Select();
            worksheet.WindowInfo.FreezePanes = true;
        }

        private static void GerarLinhas(IRange cells, ref int linha, ref int iniConjunto, DataTable dt, ref string departamentoAnt, ref string departamento, ref List<int> linhasTotGrupos)
        {
            int coluna = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                linha++;
                DataRow registro = dt.Rows[i];
                departamento = registro["Departamento"].ToString();
                if (departamentoAnt != departamento)
                {
                    Agrupamento(cells, linha, iniConjunto, dt.Rows[i - 1], departamentoAnt, ref linhasTotGrupos);
                    departamentoAnt = departamento;
                    linha++;
                    iniConjunto = linha + 1;
                }
                coluna = 0;
                cells[linha, coluna++].Value = registro["PeriodoInicial"].ToString();
                cells[linha, coluna++].Value = registro["PeriodoFinal"].ToString();
                cells[linha, coluna++].Value = registro["NomeFuncionario"].ToString();
                cells[linha, coluna++].Value = registro["Matricula"].ToString();
                cells[linha, coluna++].Value = departamento;
                cells[linha, coluna++].Value = registro["Funcao"].ToString();
                cells[linha, coluna++].Value = registro["Admissao"].ToString();
                cells[linha, coluna++].Value = registro["Supervisor"].ToString();
                cells[linha, coluna++].Value = registro["QtdeHorasPrevistas"].ToString();
                cells[linha, coluna++].Value = registro["HorasTrabalhadas"].ToString();
                cells[linha, coluna++].Value = registro["NdeDiasTrab"].ToString();
                cells[linha, coluna++].Value = registro["PercTrabalhado"].ToString().Replace("**", (linha + 1).ToString());
                cells[linha, coluna++].Value = registro["PercAbsent"].ToString().Replace("**", (linha + 1).ToString());

                cells[linha, coluna++].Value = registro["HorasExtras"].ToString();
                cells[linha, coluna++].Value = registro["QtdHorasExtras"].ToString();
                cells[linha, coluna++].Value = registro["PercHorasExtras"].ToString().Replace("**", (linha + 1).ToString());

                cells[linha, coluna++].Value = registro["AbonoLegal"].ToString();
                cells[linha, coluna++].Value = registro["qtdAbonoLegal"].ToString();
                cells[linha, coluna++].Value = registro["PercAbonoLegal"].ToString().Replace("**", (linha + 1).ToString());

                cells[linha, coluna++].Value = registro["AbonoNaoLegalOutros"].ToString();
                cells[linha, coluna++].Value = registro["qtdAbonoNaoLegalOutros"].ToString();
                cells[linha, coluna++].Value = registro["PercAbonoNaoLegalOutros"].ToString().Replace("**", (linha + 1).ToString());

                cells[linha, coluna++].Value = registro["falta"].ToString();
                cells[linha, coluna++].Value = registro["qtdfalta"].ToString();
                cells[linha, coluna++].Value = registro["Percfalta"].ToString().Replace("**", (linha + 1).ToString());

                cells[linha, coluna++].Value = registro["Atrasos"].ToString();
                cells[linha, coluna++].Value = registro["qtdAtrasos"].ToString();
                cells[linha, coluna++].Value = registro["PercAtrasos"].ToString().Replace("**", (linha + 1).ToString());

                cells[linha, coluna++].Value = registro["CreBH"].ToString();
                cells[linha, coluna++].Value = registro["QtdCreBH"].ToString();
                cells[linha, coluna++].Value = registro["PercCreBH"].ToString().Replace("**", (linha + 1).ToString());

                cells[linha, coluna++].Value = registro["DebBH"].ToString();
                cells[linha, coluna++].Value = registro["qtdDebBH"].ToString();
                cells[linha, coluna++].Value = registro["PercDebBH"].ToString().Replace("**", (linha + 1).ToString());
            }
        }

        private static void Agrupamento(SpreadsheetGear.IRange cells, int linha, int iniConjunto, DataRow registro, string nomeGrupo, ref List<int> linhasGrupos)
        {
            int coluna = 0;

            coluna++;//cells[linha, coluna++].Value = registro["PeriodoInicial"].ToString();
            coluna++;//cells[linha, coluna++].Value = registro["PeriodoFinal"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = nomeGrupo; //registro["NomeFuncionario"].ToString();
            coluna++;//cells[linha, coluna++].Value = registro["Matricula"].ToString();
            coluna++;//cells[linha, coluna++].Value = departamento;
            coluna++;//cells[linha, coluna++].Value = registro["Funcao"].ToString();
            coluna++;//cells[linha, coluna++].Value = registro["Admissao"].ToString();
            cells[linha, coluna].Font.Bold = true;
            coluna++;//cells[linha, coluna++].Value = registro["PessoaSupervisor"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "I"); //registro["QtdeHorasPrevistas"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "J"); //registro["HorasTrabalhadas"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "K"); //registro["NdeDiasTrab"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["PercTrabalhado"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["PercAbsent"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "N"); //registro["HorasExtras"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "O"); //registro["QtdHorasExtras"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["PercHorasExtras"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "Q"); //registro["AbonoLegal"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "R"); //registro["qtdAbonoLegal"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["PercAbonoLegal"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "T"); //registro["AbonoNaoLegalOutros"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "U"); //registro["qtdAbonoNaoLegalOutros"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["PercAbonoNaoLegalOutros"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "W"); //registro["falta"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "X"); //registro["qtdfalta"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["Percfalta"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "Z"); //registro["Atrasos"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "AA"); //registro["qtdAtrasos"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["PercAtrasos"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "AC"); //registro["CreBH"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "AD"); //registro["QtdCreBH"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["PercCreBH"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "AF"); //registro["DebBH"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSoma(iniConjunto, linha, "AG"); //registro["qtdDebBH"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["PercDebBH"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            linhasGrupos.Add(linha + 1);
        }

        private static void TotalizadorGeral(SpreadsheetGear.IRange cells, int linha, DataRow registro, string nomeGrupo, List<int> grupos)
        {
            int coluna = 0;

            coluna++;//cells[linha, coluna++].Value = registro["PeriodoInicial"].ToString();
            coluna++;//cells[linha, coluna++].Value = registro["PeriodoFinal"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = nomeGrupo; //registro["NomeFuncionario"].ToString();
            coluna++;//cells[linha, coluna++].Value = registro["Matricula"].ToString();
            coluna++;//cells[linha, coluna++].Value = departamento;
            coluna++;//cells[linha, coluna++].Value = registro["Funcao"].ToString();
            coluna++;//cells[linha, coluna++].Value = registro["Admissao"].ToString();
            cells[linha, coluna].Font.Bold = true;
            coluna++;//cells[linha, coluna++].Value = registro["PessoaSupervisor"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "I"); //registro["QtdeHorasPrevistas"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "J"); //registro["HorasTrabalhadas"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "K"); //registro["NdeDiasTrab"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = registro["PercTrabalhado"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = registro["PercAbsent"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "N"); //registro["HorasExtras"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "O"); //registro["QtdHorasExtras"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = registro["PercHorasExtras"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "Q"); //registro["AbonoLegal"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "R"); //registro["qtdAbonoLegal"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = registro["PercAbonoLegal"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "T"); //registro["AbonoNaoLegalOutros"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "U"); //registro["qtdAbonoNaoLegalOutros"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = registro["PercAbonoNaoLegalOutros"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "W"); //registro["falta"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "X"); //registro["qtdfalta"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = registro["Percfalta"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "Z"); //registro["Atrasos"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = FormulaSomaGrupo(grupos, "AA"); //registro["qtdAtrasos"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Formula = registro["PercAtrasos"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSomaGrupo(grupos, "AC"); //registro["CreBH"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSomaGrupo(grupos, "AD"); //registro["QtdCreBH"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["PercCreBH"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSomaGrupo(grupos, "AF"); //registro["DebBH"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = FormulaSomaGrupo(grupos, "AG"); //registro["qtdDebBH"].ToString();
            cells[linha, coluna].Font.Bold = true;
            cells[linha, coluna++].Value = registro["PercDebBH"].ToString().Replace("**", (linha + 1).ToString());
            cells[linha, coluna].Font.Bold = true;

        }
        #endregion

        #region Formulas
        private static string FormulaSoma(int iniConjunto, int linha, string letraColuna)
        {
            string formula;
            if (iniConjunto == linha)
                formula = "=SUM(" + letraColuna + "" + iniConjunto + ")";
            else
                formula = "=SUM(" + letraColuna + "" + iniConjunto + ":" + letraColuna + "" + linha + ")";
            return formula;
        }

        private static string FormulaSomaGrupo(List<int> grupos, string letraColuna)
        {
            string formula = "";
            if (grupos != null && grupos.Count > 0)
            {
                if (grupos.Count == 1)
                    formula = "=SUM(" + letraColuna + "" + grupos.FirstOrDefault() + ")";
                else
                    formula = "=SUM(" + String.Join(",", grupos.Select(s => letraColuna + s).ToList()) + ")";
            }
            return formula;
        } 
        #endregion
    }
}
