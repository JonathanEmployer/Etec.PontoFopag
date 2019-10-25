using SpreadsheetGear;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System;

namespace BLL
{
    public static class RelatorioExcelGenerico
    {
        public static byte[] GeraRelatorio(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);
            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }

        private static void GeraRelatorio(IWorkbook workbook, DataTable dados)
        {
            IWorksheet worksheet = workbook.Worksheets[0];
            worksheet.Name = "Resultado";
            IRange cells = worksheet.Cells["A1"];
            if (dados != null)
            {
                cells.CopyFromDataTable(dados, SpreadsheetGear.Data.SetDataFlags.None);
            }
            // Formatando o relatório
            cells = worksheet.Cells["A1:CA1"];
            cells.Font.Bold = true;
            cells.HorizontalAlignment = HAlign.Center;

            worksheet.UsedRange.Columns.AutoFit();
        }

        public static string ContentType(int FileType)
        {
            string Format = "application/vnd.ms-excel";

            switch (FileType)
            {
                case 0:
                    Format = "application/vnd.ms-excel";
                    break;
                case 1:
                    Format = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                default:
                    Format = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
            }

            return Format;
        }

        public static byte[] Relatorio_Funcionario_Por_Nome(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            // Removendo colunas do dataset que não podem ser exportadas
            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;
            cells["A:A"].Delete(DeleteShiftDirection.Left);
            cells["C:D"].Delete(DeleteShiftDirection.Left);
            cells["D:E"].Delete(DeleteShiftDirection.Left);
            cells["E:N"].Delete(DeleteShiftDirection.Left);

            // Corrigindo a ordem das colunas
            // Empresa da D para a A
            cells["A:A"].Insert();
            cells["E:E"].Copy(cells["A:A"]);
            cells["E:E"].Delete(DeleteShiftDirection.Left);
            // Nome da C para a B
            cells["B:B"].Insert();
            cells["D:D"].Copy(cells["B:B"]);
            cells["D:D"].Delete(DeleteShiftDirection.Left);

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Nome";
            cells["C1"].Formula = "Código";
            cells["D1"].Formula = "Matrícula";

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Funcionario_Por_Codigo(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();

            //Insere os valores nas colunas criadas
            cells["M:M"].Copy(cells["A:A"]);
            cells["G:G"].Copy(cells["B:B"]);
            cells["F:F"].Copy(cells["C:C"]);
            cells["J:J"].Copy(cells["D:D"]);

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Nome";
            cells["C1"].Formula = "Código";
            cells["D1"].Formula = "Matrícula";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["E:Z"].Delete(DeleteShiftDirection.Left);

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Funcionario_Por_Empresa(DataTable dados)
        {
            string teste = string.Empty;

            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();

            //Insere os valores nas colunas criadas
            cells["M:M"].Copy(cells["A:A"]);
            cells["G:G"].Copy(cells["B:B"]);
            cells["F:F"].Copy(cells["C:C"]);
            cells["J:J"].Copy(cells["D:D"]);

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Nome";
            cells["C1"].Formula = "Código";
            cells["D1"].Formula = "Matrícula";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["E:Z"].Delete(DeleteShiftDirection.Left);

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Funcionario_Por_Departamento(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();


            //Insere os valores nas colunas criadas
            cells["N:N"].Copy(cells["A:A"]);
            cells["O:O"].Copy(cells["B:B"]);
            cells["H:H"].Copy(cells["C:C"]);
            cells["G:G"].Copy(cells["D:D"]);
            cells["K:K"].Copy(cells["E:E"]);

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Departamento";
            cells["C1"].Formula = "Nome";
            cells["D1"].Formula = "Código";
            cells["E1"].Formula = "Matrícula";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["F:Z"].Delete(DeleteShiftDirection.Left);

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Funcionario_Por_Horario(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();


            //Insere os valores nas colunas criadas
            cells["N:N"].Copy(cells["A:A"]);
            cells["U:U"].Copy(cells["B:B"]);
            cells["H:H"].Copy(cells["C:C"]);
            cells["G:G"].Copy(cells["D:D"]);
            cells["K:K"].Copy(cells["E:E"]);

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Horário";
            cells["C1"].Formula = "Nome";
            cells["D1"].Formula = "Código";
            cells["E1"].Formula = "Matrícula";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["F:Z"].Delete(DeleteShiftDirection.Left);

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Funcionario_Admissao(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();

            //Insere os valores nas colunas criadas
            cells["M:M"].Copy(cells["A:A"]);
            cells["G:G"].Copy(cells["B:B"]);
            cells["F:F"].Copy(cells["C:C"]);
            cells["K:K"].Copy(cells["D:D"]);

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Nome";
            cells["C1"].Formula = "Código";
            cells["D1"].Formula = "Admissão";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["E:Z"].Delete(DeleteShiftDirection.Left);

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Funcionario_Demissao(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();

            //Insere os valores nas colunas criadas
            cells["M:M"].Copy(cells["A:A"]);
            cells["G:G"].Copy(cells["B:B"]);
            cells["F:F"].Copy(cells["C:C"]);
            cells["L:L"].Copy(cells["D:D"]);

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Nome";
            cells["C1"].Formula = "Código";
            cells["D1"].Formula = "Demissão";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["E:Z"].Delete(DeleteShiftDirection.Left);

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Funcionario_Ativo_Inativo(DataTable dados, string Ordenacao)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();


            //Insere os valores nas colunas criadas
            cells["A2: A" + (dados.Rows.Count + 1)].Formula = Ordenacao;
            cells["N:N"].Copy(cells["B:B"]);
            cells["H:H"].Copy(cells["C:C"]);
            cells["G:G"].Copy(cells["D:D"]);
            cells["K:K"].Copy(cells["E:E"]);

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Ordenação";
            cells["B1"].Formula = "Empresa";
            cells["C1"].Formula = "Nome";
            cells["D1"].Formula = "Código";
            cells["E1"].Formula = "Matrícula";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["F:Z"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a primeira coluna
            IRange cells2 = worksheet.Cells["A1:A1"];
            cells2.Font.Bold = true;
            cells2.HorizontalAlignment = HAlign.Center;
            cells2.EntireColumn.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Hora_Extra_Por_Funcionario(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();
            cells["F:F"].Insert();
            cells["G:G"].Insert();
            cells["H:H"].Insert();
            cells["I:I"].Insert();
            cells["J:J"].Insert();
            cells["K:K"].Insert();
            cells["L:L"].Insert();
            cells["M:M"].Insert();
            cells["N:N"].Insert();
            cells["O:O"].Insert();


            //Insere os valores nas colunas criadas
            cells["Q:Q"].Copy(cells["A:A"]);
            cells["U:U"].Copy(cells["B:B"]);
            cells["R:R"].Copy(cells["C:C"]);
            cells["S:S"].Copy(cells["D:D"]);
            cells["T:T"].Copy(cells["E:E"]);
            cells["Z:Z"].Copy(cells["F:F"]);
            cells["AA:AA"].Copy(cells["G:G"]);
            cells["AB:AB"].Copy(cells["H:H"]);
            cells["AC:AC"].Copy(cells["I:I"]);
            cells["AD:AD"].Copy(cells["J:J"]);
            cells["AE:AE"].Copy(cells["K:K"]);
            cells["AF:AF"].Copy(cells["L:L"]);
            cells["AG:AG"].Copy(cells["M:M"]);

            // Formatando celulas de quantidade de horas
            cells["H:M"].NumberFormat = "[h]:mm";

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "CNPJ";
            cells["C1"].Formula = "Endereço";
            cells["D1"].Formula = "Cidade";
            cells["E1"].Formula = "UF";
            cells["F1"].Formula = "Funcionário";
            cells["G1"].Formula = "Código";
            cells["H1"].Formula = "Trab Diu";
            cells["I1"].Formula = "Trab Not";
            cells["J1"].Formula = "HE Diu";
            cells["K1"].Formula = "HE Not";
            cells["L1"].Formula = "Falta Diu";
            cells["M1"].Formula = "Falta Not";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["N:AZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Hora_Extra_Por_Departamento(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();
            cells["F:F"].Insert();
            cells["G:G"].Insert();
            cells["H:H"].Insert();
            cells["I:I"].Insert();
            cells["J:J"].Insert();
            cells["K:K"].Insert();
            cells["L:L"].Insert();
            cells["M:M"].Insert();
            cells["N:N"].Insert();
            cells["O:O"].Insert();
            cells["P:P"].Insert();
            cells["Q:Q"].Insert();
            cells["R:R"].Insert();
            cells["S:S"].Insert();
            cells["T:T"].Insert();
            cells["U:U"].Insert();

            //Insere os valores nas colunas criadas
            cells["W:W"].Copy(cells["A:A"]);
            cells["AA:AA"].Copy(cells["B:B"]);
            cells["X:X"].Copy(cells["C:C"]);
            cells["Y:Y"].Copy(cells["D:D"]);
            cells["Z:Z"].Copy(cells["E:E"]);
            cells["AC:AC"].Copy(cells["F:F"]);
            cells["AD:AD"].Copy(cells["G:G"]);
            cells["AF:AF"].Copy(cells["H:H"]);
            cells["AG:AG"].Copy(cells["I:I"]);
            cells["AH:AH"].Copy(cells["J:J"]);
            cells["AI:AI"].Copy(cells["K:K"]);
            cells["AJ:AJ"].Copy(cells["L:L"]);
            cells["AK:AK"].Copy(cells["M:M"]);
            cells["AL:AL"].Copy(cells["N:N"]);
            cells["AM:AM"].Copy(cells["O:O"]);
            cells["AN:AN"].Copy(cells["P:P"]);
            cells["AO:AO"].Copy(cells["Q:Q"]);
            cells["AP:AP"].Copy(cells["R:R"]);
            cells["AQ:AQ"].Copy(cells["S:S"]);
            cells["AR:AR"].Copy(cells["T:T"]);
            cells["AS:AS"].Copy(cells["U:U"]);

            // Removendo colunas do dataset que não podem ser exportadas
            cells["V:AZ"].Delete(DeleteShiftDirection.Left);

            // Formatando celulas de quantidade de horas
            cells["J:U"].NumberFormat = "[h]:mm";


            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "CNPJ";
            cells["C1"].Formula = "Endereço";
            cells["D1"].Formula = "Cidade";
            cells["E1"].Formula = "UF";
            cells["F1"].Formula = "Id. Dep.";
            cells["G1"].Formula = "Departamento";
            cells["H1"].Formula = "Funcionário";
            cells["I1"].Formula = "Código";
            cells["J1"].Formula = "Trab Diu";
            cells["K1"].Formula = "Trab Not";
            cells["L1"].Formula = "HE Diu";
            cells["M1"].Formula = "HE Not";
            cells["N1"].Formula = "Falta Diu";
            cells["O1"].Formula = "Falta Not";
            cells["P1"].Formula = "Tot Trab Diu";
            cells["Q1"].Formula = "Tot Trab Not";
            cells["R1"].Formula = "Tot HE Diu";
            cells["S1"].Formula = "Tot HE Not";
            cells["T1"].Formula = "Tot Falta Diu";
            cells["U1"].Formula = "Total Falta Not";

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Hora_Extra_Por_Percentual(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();
            cells["F:F"].Insert();
            cells["G:G"].Insert();
            cells["H:H"].Insert();
            cells["I:I"].Insert();
            cells["J:J"].Insert();
            cells["K:K"].Insert();
            cells["L:L"].Insert();
            cells["M:M"].Insert();
            cells["N:N"].Insert();
            cells["O:O"].Insert();
            cells["P:P"].Insert();
            cells["Q:Q"].Insert();
            cells["R:R"].Insert();
            cells["S:S"].Insert();
            cells["T:T"].Insert();
            cells["U:U"].Insert();


            //Insere os valores nas colunas criadas
            cells["W:W"].Copy(cells["A:A"]);
            cells["AA:AA"].Copy(cells["B:B"]);
            cells["Y:Y"].Copy(cells["C:C"]);
            cells["Z:Z"].Copy(cells["D:D"]);
            cells["AF:AF"].Copy(cells["E:E"]);
            cells["AG:AG"].Copy(cells["F:F"]);
            cells["AH:AH"].Copy(cells["G:G"]);
            cells["AI:AI"].Copy(cells["H:H"]);
            cells["AJ:AJ"].Copy(cells["I:I"]);
            cells["AK:AK"].Copy(cells["J:J"]);
            cells["AL:AL"].Copy(cells["K:K"]);
            cells["AM:AM"].Copy(cells["L:L"]);
            cells["AN:AN"].Copy(cells["M:M"]);
            cells["AO:AO"].Copy(cells["N:N"]);
            cells["BF:BF"].Copy(cells["O:O"]);
            cells["BG:BG"].Copy(cells["P:P"]);
            cells["BH:BH"].Copy(cells["Q:Q"]);
            cells["BI:BI"].Copy(cells["R:R"]);
            cells["BJ:BJ"].Copy(cells["S:S"]);
            cells["BK:BK"].Copy(cells["T:T"]);
            cells["BL:BL"].Copy(cells["U:U"]);

            // Formatando celulas de quantidade de horas
            cells["H:U"].NumberFormat = "[h]:mm";

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "CNPJ";
            cells["C1"].Formula = "Cidade";
            cells["D1"].Formula = "UF";
            cells["E1"].Formula = "Funcionário";
            cells["F1"].Formula = "Código";
            cells["G1"].Formula = "Percentuais";
            cells["H1"].Formula = "Trab Diu";
            cells["I1"].Formula = "Trab Not";
            cells["J1"].Formula = "HE Diu";
            cells["K1"].Formula = "HE Not";
            cells["L1"].Formula = "Falta Diu";
            cells["M1"].Formula = "Falta Not";
            cells["N1"].Formula = "DSR";
            cells["O1"].Formula = "Tot Trab Diu";
            cells["P1"].Formula = "Tot Trab Not";
            cells["Q1"].Formula = "Tot HE Diu";
            cells["R1"].Formula = "Tot HE Not";
            cells["S1"].Formula = "Tot Falta Diu";
            cells["T1"].Formula = "Total Falta Not";
            cells["U1"].Formula = "Total DSR";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["V:BZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_de_Abono_por_Funcionario_Data(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            cells["G:G"].NumberFormat = "dd/mm/yyyy";

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "CNPJ";
            cells["C1"].Formula = "Departamento";
            cells["D1"].Formula = "Funcionário";
            cells["E1"].Formula = "Código";
            cells["F1"].Formula = "Ocorrência";
            cells["G1"].Formula = "Data";
            cells["H1"].Formula = "Dia";
            cells["I1"].Formula = "Abono Parcial";
            cells["J1"].Formula = "Abono Total";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["K:AZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }

		public static byte[] Relatorio_de_Cartao_Ponto_Individual(DataTable dados)
		{
			dados.Columns.Add("DataFormatada", typeof(String));

			foreach (DataRow row in dados.Rows)
			{
				row["DataFormatada"] = Convert.ToDateTime((row["DataMarcacao"]).ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
			}

			string[] selectedColumns = new[] { "empresa", "cnpj_cpf", "endereco", "cidade", "estado", "DataFormatada", "funcionario", "dscodigo", "hentrada_1", "hentrada_2", "hsaida_1", "hsaida_2", "legenda", "entrada_1", "saida_1", "entrada_2", "saida_2", "entrada_3", "saida_3", "entrada_4", "saida_4", "horasTrabalhadas", "horasTrabalhadasnoturnas", "horasextrasdiurna", "horasextranoturna", "horasfaltas", "horasfaltanoturna", "ocorrencia" };
			DataTable dt = new DataView(dados).ToTable(false, selectedColumns);
			IWorkbook workbook = Factory.GetWorkbook();
			GeraRelatorio(workbook, dt);

			IWorksheet worksheet = workbook.Worksheets[0];
			IRange cells = worksheet.Cells;

			// Formatando celulas de quantidade de horas
			cells["I:AE"].NumberFormat = "[h]:mm";

			// Corrigindo o nome dos cabeçalhos
			cells["A1"].Formula = "Empresa";
			cells["B1"].Formula = "CNPJ";
			cells["C1"].Formula = "Endereço";
			cells["D1"].Formula = "Cidade";
			cells["E1"].Formula = "UF";
			cells["F1"].Formula = "Data";
			cells["G1"].Formula = "Funcionário";
			cells["H1"].Formula = "Código";
			cells["I1"].Formula = "J.Ent1";
			cells["J1"].Formula = "J.Sai1";
			cells["K1"].Formula = "J.Ent2";
			cells["L1"].Formula = "J.Sai2";
			cells["M1"].Formula = "Legenda";
			cells["N1"].Formula = "Ent";
			cells["O1"].Formula = "Sai";
			cells["P1"].Formula = "Ent";
			cells["Q1"].Formula = "Sai";
			cells["R1"].Formula = "Ent";
			cells["S1"].Formula = "Sai";
			cells["T1"].Formula = "Ent";
			cells["U1"].Formula = "Sai";
			cells["V1"].Formula = "Trab Diu";
			cells["W1"].Formula = "Trab Not";
			cells["X1"].Formula = "HE Diu";
			cells["Y1"].Formula = "HE Not";
			cells["Z1"].Formula = "Faltas Diu";
			cells["AA1"].Formula = "Faltas Not";
			cells["AB1"].Formula = "Ocorrências adicionais";

			// Removendo colunas do dataset que não podem ser exportadas
			cells["AG:EZ"].Delete(DeleteShiftDirection.Left);

			// Reconfigura coluna
			IRange cells2 = worksheet.Cells["A1:AF1"];
			cells2.Font.Bold = true;
			cells2.HorizontalAlignment = HAlign.Center;
			cells2.EntireColumn.AutoFit();

			return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);

		}

		public static byte[] Relatorio_de_Inconsistencias(DataTable dados)
		{
            dados.Columns.Add("DataFormatada", typeof(String)); 
            dados.Columns.Add("F", typeof(String));
            dados.Columns.Add("horariojornada", typeof(String));
            int i = 2;
            foreach (DataRow row in dados.Rows)
            {
                string hentrada1 = row["hentrada_1"].ToString();
                string hentrada2 = row["hentrada_2"].ToString();

                row["DataFormatada"] = Convert.ToDateTime((row["DataMarcacao"]).ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture); 
                if (hentrada1 == "" || hentrada2 == "")
                {
                    row["F"] = "=(J"+i+"-I"+i+"+(J"+i+"<I"+i+"))+(L"+i+"-K"+i+"+(L"+i+ "<K" + i + "))";
                    row["horariojornada"] = "=AF"+i;
                    if (i != 0) { i = i + 1; }
                }
                else
                {
                    row["F"] = "=(((J"+i+"-I"+i+"+(J"+i+"<I"+i+"))*24)/24)+(((L"+i+"-K"+i+"+(L"+i+"<K"+i+"))*24)/24)";
                    row["horariojornada"] = "=AF"+i;
                    if (i != 0) { i = i + 1; }
                }

            }

            string[] selectedColumns = new[] { "empresa", "cnpj_cpf", "endereco", "cidade", "estado", "DataFormatada", "funcionario", "dscodigo", "hentrada_1", "hsaida_1", "hentrada_2", "hsaida_2", "entrada_1", "saida_1", "entrada_2", "saida_2", "entrada_3", "saida_3", "entrada_4", "saida_4","TotalHorasAlmoco", "TotalIntervaloPrev","horastrabalhadas", "totaltrabalhadanoturna", "horariojornada", "horasextrasdiurna", "horasextranoturna", "bancohorascre", "TotalHorasTrabalhadas", "Interjornada", "legenda", "F" };
            DataTable dt = dados.Rows.Count > 0 ? new DataView(dados).ToTable(false, selectedColumns):null;
            IWorkbook workbook = Factory.GetWorkbook();
			GeraRelatorio(workbook, dt);
            IWorksheet worksheet = workbook.Worksheets[0];
			IRange cells = worksheet.Cells;

			// Formatando celulas de quantidade de horas
			cells["I:AG"].NumberFormat = "[h]:mm";
		
			// Corrigindo o nome dos cabeçalhos
			cells["A1"].Formula = "Empresa";
			cells["B1"].Formula = "CNPJ";
			cells["C1"].Formula = "Endereço";
			cells["D1"].Formula = "Cidade";
			cells["E1"].Formula = "UF";
			cells["F1"].Formula = "Data";

			cells["G1"].Formula = "Funcionário";
			cells["H1"].Formula = "Código";
			cells["I1"].Formula = "J.Ent1";
			cells["J1"].Formula = "J.Sai1";
			cells["K1"].Formula = "J.Ent2";
			cells["L1"].Formula = "J.Sai2";
			cells["M1"].Formula = "Ent 1";
			cells["N1"].Formula = "Sai 1";
			cells["O1"].Formula = "Ent 2";
			cells["P1"].Formula = "Sai 2";
			cells["Q1"].Formula = "Ent 3";
			cells["R1"].Formula = "Sai 3";
			cells["S1"].Formula = "Ent 4";
			cells["T1"].Formula = "Sai 4";
			cells["U1"].Formula = "IntraJ Real";
			cells["V1"].Formula = "IntraJ Esp";
			cells["W1"].Formula = "Trab Diu";
			cells["X1"].Formula = "Trab Not";
			cells["Y1"].Formula = "Trab Jornada";
			cells["Z1"].Formula = "HE Diu";
			cells["AA1"].Formula = "HE Not";
			cells["AB1"].Formula = "BH Cred";
			cells["AC1"].Formula = "Total Trab";
			cells["AD1"].Formula = "Interjornada";
			cells["AE1"].Formula = "Legenda";
            cells["AF1"].Formula = "F";
            // Removendo colunas do dataset que não podem ser exportadas
            cells["AG:EZ"].Delete(DeleteShiftDirection.Left);
            
            // Reconfigura coluna
            IRange cells2 = worksheet.Cells["A1:AE1"];
			cells2.Font.Bold = true;
            worksheet.Cells["AF1:AG1"].Columns.Hidden=true;
            cells2.HorizontalAlignment = HAlign.Center;
			cells2.EntireColumn.AutoFit();

			return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);

		}
		
        public static byte[] Relatorio_Absenteismo_Analitico(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;
/*            
            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();
            cells["F:F"].Insert();
            cells["G:G"].Insert();
            cells["H:H"].Insert();
            cells["I:I"].Insert();
            cells["J:J"].Insert();


            //Insere os valores nas colunas criadas
            cells["A:A"].Copy(cells["A:A"]);
            cells["B:B"].Copy(cells["B:B"]);
            cells["C:C"].Copy(cells["C:C"]);
            cells["D:D"].Copy(cells["D:D"]);
            cells["E:E"].Copy(cells["E:E"]);
            cells["F:F"].Copy(cells["F:F"]);
            cells["G:G"].Copy(cells["G:G"]);
            cells["H:H"].Copy(cells["H:H"]);
            cells["I:I"].Copy(cells["I:I"]);
            cells["J:J"].Copy(cells["J:J"]);
*/
            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Departamento";
            cells["C1"].Formula = "Código";
            cells["D1"].Formula = "Funcionário"; 
            cells["E1"].Formula = "Cod.Ocorrência";
            cells["F1"].Formula = "Ocorrência";
            cells["G1"].Formula = "Absenteismo";
            cells["H1"].Formula = "Tot Dep";
            cells["I1"].Formula = "Tot Emp";
            cells["J1"].Formula = "Tot Func";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["K:AZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Absenteismo_Sintetico(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;
/*            
            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();
            cells["F:F"].Insert();
            cells["G:G"].Insert();
            cells["H:H"].Insert();
            cells["I:I"].Insert();


            //Insere os valores nas colunas criadas
            cells["A:A"].Copy(cells["A:A"]);
            cells["B:B"].Copy(cells["B:B"]);
            cells["C:C"].Copy(cells["C:C"]);
            cells["D:D"].Copy(cells["D:D"]);
            cells["E:E"].Copy(cells["E:E"]);
            cells["F:F"].Copy(cells["F:F"]);
            cells["G:G"].Copy(cells["G:G"]);
            cells["H:H"].Copy(cells["H:H"]);
            cells["I:I"].Copy(cells["I:I"]);
*/
            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Departamento";
            cells["C1"].Formula = "Código";
            cells["D1"].Formula = "Funcionário"; 
            cells["E1"].Formula = "Cod.Ocorrência";
            cells["F1"].Formula = "Ocorrência";
            cells["G1"].Formula = "Absenteismo";
            cells["H1"].Formula = "Tot Dep";
            cells["I1"].Formula = "Tot Emp";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["J:AZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_de_Ocorrencias_por_Data_Funcionario(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();
            cells["F:F"].Insert();
            cells["G:G"].Insert();
            cells["H:H"].Insert();
            cells["I:I"].Insert();

            //J new DataColumn("id"),
            //K new DataColumn("empresa"),
            //L new DataColumn("cnpj_cpf"),
            //M new DataColumn("departamento"),
            //N new DataColumn("dscodigo"),
            //O new DataColumn("funcionario"),
            //P new DataColumn("data") { DataType = typeof(DateTime) },
            //Q new DataColumn("dia"),
            //R new DataColumn("ocorrencia"),
            //S new DataColumn("marcacoes"),
            //T new DataColumn("bancohorasdeb"),
            //U new DataColumn("horasextradiurna"),
            //V new DataColumn("horasextranoturna"),
            //W new DataColumn("horastrabalhadas"),
            //X new DataColumn("horastrabalhadasnoturnas"),
            //Y new DataColumn("Matricula"),
            //Z new DataColumn("CPF"),
            //AA new DataColumn("Observacao"),
            //AB new DataColumn("Competencia"),
            //AC new DataColumn("IdDocumentoWorkflow")


            //Insere os valores nas colunas criadas
            cells["K:K"].Copy(cells["A:A"]);
            cells["M:M"].Copy(cells["B:B"]);
            cells["O:O"].Copy(cells["C:C"]);
            cells["P:P"].Copy(cells["D:D"]);
            cells["R:R"].Copy(cells["E:E"]);
            cells["S:S"].Copy(cells["F:F"]);
            cells["T:T"].Copy(cells["G:G"]);
            cells["U:U"].Copy(cells["H:H"]);
            cells["V:V"].Copy(cells["I:I"]);

            // Formatando celulas de quantidade de horas
            cells["G:I"].NumberFormat = "[h]:mm";
            cells["D:D"].NumberFormat = "dd/mm/yyyy";

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Departamento";
            cells["C1"].Formula = "Funcionário";
            cells["D1"].Formula = "Data";
            cells["E1"].Formula = "Ocorrência";
            cells["F1"].Formula = "Marcações";
            cells["G1"].Formula = "Deb. BH";
            cells["H1"].Formula = "Horas Diurnas";
            cells["I1"].Formula = "Horas Noturnas";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["J:AZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_de_Ocorrencias_por_Funcionario_Data(DataTable dados)
        {
            string[] selectedColumns = new[] { "empresa", "cnpj_cpf", "departamento", "dscodigo", "funcionario", "data", "ocorrencia", "marcacoes", "bancohorasdeb", "horasextradiurna", "horasextranoturna"};
            DataTable dt = new DataView(dados).ToTable(false, selectedColumns);
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dt);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "CNPJ-CPF";
            cells["C1"].Formula = "Departamento";
            cells["D1"].Formula = "Código";
            cells["E1"].Formula = "Funcionário";
            cells["F1"].Formula = "Data";
            cells["G1"].Formula = "Ocorrência";
            cells["H1"].Formula = "Marcações";
            cells["I1"].Formula = "Deb. BH";
            cells["J1"].Formula = "Horas Diurnas ";
            cells["K1"].Formula = "Horas Noturnas";

            // Formatando celulas de quantidade de horas
            cells["I:K"].NumberFormat = "[h]:mm";
            cells["F:F"].NumberFormat = "dd/mm/yyyy";

            // Reconfigura a primeira coluna
            IRange cells2 = worksheet.Cells["A1:A1"];
            cells2.Font.Bold = true;
            cells2.HorizontalAlignment = HAlign.Center;
            cells2.EntireColumn.AutoFit();

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
		
		public static byte[] Relatorio_Espelho(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();
            cells["F:F"].Insert();
            cells["G:G"].Insert();
            cells["H:H"].Insert();
            cells["I:I"].Insert();
            cells["J:J"].Insert();
            cells["K:K"].Insert();
            cells["L:L"].Insert();
            cells["M:M"].Insert();
            cells["N:N"].Insert();
            cells["O:O"].Insert();
            cells["P:P"].Insert();
            cells["Q:Q"].Insert();
            cells["R:R"].Insert();
            cells["S:S"].Insert();
            cells["T:T"].Insert();


            //Insere os valores nas colunas criadas
            cells["Z:Z"].Copy(cells["A:A"]);
            cells["AB:AB"].Copy(cells["B:B"]);
            cells["AD:AD"].Copy(cells["C:C"]);
            cells["AE:AE"].Copy(cells["D:D"]);
            cells["Y:Y"].Copy(cells["E:E"]);
            cells["W:W"].Copy(cells["F:F"]);
            cells["V:V"].Copy(cells["G:G"]);
            cells["X:X"].Copy(cells["H:H"]);
            cells["AF:AF"].Copy(cells["I:I"]);
            cells["AK:AK"].Copy(cells["J:J"]);
            cells["AM:AM"].Copy(cells["K:K"]);
            cells["AN:AN"].Copy(cells["L:L"]);
            cells["AO:AO"].Copy(cells["M:M"]);
            cells["AP:AP"].Copy(cells["N:N"]);
            cells["AQ:AQ"].Copy(cells["O:O"]);
            cells["AR:AR"].Copy(cells["P:P"]);
            cells["AL:AL"].Copy(cells["Q:Q"]);
            cells["AS:AS"].Copy(cells["R:R"]);
            cells["AT:AT"].Copy(cells["S:S"]);
            cells["AU:AU"].Copy(cells["T:T"]);

            // Formatando celulas de quantidade de horas
            cells["K:P"].NumberFormat = "[h]:mm";

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "CNPJ";
            cells["C1"].Formula = "Cidade";
            cells["D1"].Formula = "UF";
            cells["E1"].Formula = "PIS";
            cells["F1"].Formula = "Funcionário";
            cells["G1"].Formula = "Código";
            cells["H1"].Formula = "Dt Admissão";
            cells["I1"].Formula = "Dia";
            cells["J1"].Formula = "Marcações registradas no ponto eletrônico";
            cells["K1"].Formula = "Ent";
            cells["L1"].Formula = "Sai";
            cells["M1"].Formula = "Ent";
            cells["N1"].Formula = "Sai";
            cells["O1"].Formula = "Ent";
            cells["P1"].Formula = "Sai";
            cells["Q1"].Formula = "Cod.horario";
            cells["R1"].Formula = "Horario";
            cells["S1"].Formula = "Ocorrência";
            cells["T1"].Formula = "Motivo";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["U:BZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Historico(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();
            cells["F:F"].Insert();


            //Insere os valores nas colunas criadas
            cells["H:H"].Copy(cells["A:A"]);
            cells["I:I"].Copy(cells["B:B"]);
            cells["J:J"].Copy(cells["C:C"]);
            cells["K:K"].Copy(cells["D:D"]);
            cells["L:L"].Copy(cells["E:E"]);
            cells["M:M"].Copy(cells["F:F"]);

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Departamento";
            cells["C1"].Formula = "Funcionário";
            cells["D1"].Formula = "Data";
            cells["E1"].Formula = "Hora";
            cells["F1"].Formula = "Histórico";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["G:AZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Manutencao_Diaria(DataTable dados)
        {
            foreach (DataRow row in dados.Rows)
            {
                row["dataCompleta"] = Convert.ToDateTime((row["dataCompleta"]).ToString()).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();
            cells["F:F"].Insert();
            cells["G:G"].Insert();
            cells["H:H"].Insert();
            cells["I:I"].Insert();
            cells["J:J"].Insert();
            cells["K:K"].Insert();
            cells["L:L"].Insert();
            cells["M:M"].Insert();
            cells["N:N"].Insert();
            cells["O:O"].Insert();
            cells["P:P"].Insert();
            cells["Q:Q"].Insert();
            cells["R:R"].Insert();
            cells["S:S"].Insert();
            cells["T:T"].Insert();
            cells["U:U"].Insert();
            cells["V:V"].Insert();
            cells["W:W"].Insert();
            cells["X:X"].Insert();
            cells["Y:Y"].Insert();
            cells["Z:Z"].Insert();
            cells["AA:AA"].Insert();

            //Insere os valores nas colunas criadas
            cells["BE:BE"].Copy(cells["A:A"]);
            cells["BF:BF"].Copy(cells["B:B"]);
            cells["BG:BG"].Copy(cells["C:C"]);
            cells["BH:BH"].Copy(cells["D:D"]);
            cells["BI:BI"].Copy(cells["E:E"]);
            cells["CZ:CZ"].Copy(cells["F:F"]);
            cells["AF:AF"].Copy(cells["G:G"]);
            cells["AY:AY"].Copy(cells["H:H"]);
            cells["AX:AX"].Copy(cells["I:I"]);
            cells["BN:BN"].Copy(cells["J:J"]);
            cells["BP:BP"].Copy(cells["K:K"]);
            cells["BO:BO"].Copy(cells["L:L"]);
            cells["BQ:BQ"].Copy(cells["M:M"]);
            cells["AD:AD"].Copy(cells["N:N"]);
            cells["AG:AG"].Copy(cells["O:O"]);
            cells["AK:AK"].Copy(cells["P:P"]);
            cells["AH:AH"].Copy(cells["Q:Q"]);
            cells["AL:AL"].Copy(cells["R:R"]);
            cells["AI:AI"].Copy(cells["S:S"]);
            cells["AM:AM"].Copy(cells["T:T"]);

            cells["AO:AO"].Copy(cells["U:U"]);
            cells["AT:AT"].Copy(cells["V:V"]);
            cells["AP:AP"].Copy(cells["W:W"]);
            cells["AU:AU"].Copy(cells["X:X"]);
            cells["AQ:AQ"].Copy(cells["Y:Y"]);
            cells["AV:AV"].Copy(cells["Z:Z"]);

            cells["AW:AW"].Copy(cells["AA:AA"]);

            // Formatando celulas de quantidade de horas
            cells["J:M"].NumberFormat = "[h]:mm";
            cells["O:Z"].NumberFormat = "[h]:mm";

            // Formatando celulas de data dia/mes
            cells["F:F"].NumberFormat = "dd/mm/yyyy";

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "CNPJ";
            cells["C1"].Formula = "Endereço";
            cells["D1"].Formula = "Cidade";
            cells["E1"].Formula = "UF";
            cells["F1"].Formula = "Data";
            cells["G1"].Formula = "Dia";
            cells["H1"].Formula = "Funcionário";
            cells["I1"].Formula = "Código";
            cells["J1"].Formula = "J.Ent1";
            cells["K1"].Formula = "J.Sai1";
            cells["L1"].Formula = "J.Ent2";
            cells["M1"].Formula = "J.Sai2";
            cells["N1"].Formula = "Legenda";
            cells["O1"].Formula = "Ent";
            cells["P1"].Formula = "Sai";
            cells["Q1"].Formula = "Ent";
            cells["R1"].Formula = "Sai";
            cells["S1"].Formula = "Ent";
            cells["T1"].Formula = "Sai";
            cells["U1"].Formula = "H Trab Diu";
            cells["V1"].Formula = "H Trab Not";
            cells["W1"].Formula = "HE Diu";
            cells["X1"].Formula = "HE Not";
            cells["Y1"].Formula = "Falta Diu";
            cells["Z1"].Formula = "Falta Not";
            cells["AA1"].Formula = "Observação";

            // Removendo colunas do dataset que não podem ser exportadas
            cells["AB:EZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Intervalos(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;
            
           //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();
            cells["D:D"].Insert();
            cells["E:E"].Insert();
            cells["F:F"].Insert();
            cells["G:G"].Insert();
            cells["H:H"].Insert();
            cells["I:I"].Insert();
            cells["J:J"].Insert();
            cells["K:K"].Insert();
            cells["L:L"].Insert();
            cells["M:M"].Insert();
            cells["N:N"].Insert();
            cells["O:O"].Insert();
            cells["P:P"].Insert();
            cells["Q:Q"].Insert();
            cells["R:R"].Insert();
            cells["S:S"].Insert();
            cells["T:T"].Insert();
            cells["U:U"].Insert();
            cells["V:V"].Insert();
            cells["W:W"].Insert();
            cells["X:X"].Insert();
            cells["Y:Y"].Insert();
            cells["Z:Z"].Insert();
            cells["AA:AA"].Insert();
            cells["AB:AB"].Insert();
            cells["AC:AC"].Insert();
            cells["AD:AD"].Insert();
            cells["AE:AE"].Insert();
            cells["AF:AF"].Insert();

            //Insere os valores nas colunas criadas
            cells["BH:BK"].Copy(cells["A:A"]);
            cells["BL:BL"].Copy(cells["B:B"]);
            cells["BM:BM"].Copy(cells["C:C"]);
            cells["BN:BN"].Copy(cells["D:D"]);
            cells["BO:BO"].Copy(cells["E:E"]);
            cells["AJ:AJ"].Copy(cells["F:F"]);
            cells["BD:BD"].Copy(cells["G:G"]);
            cells["BC:BC"].Copy(cells["H:H"]);
            cells["CD:CD"].Copy(cells["I:I"]);
            cells["CF:CF"].Copy(cells["J:J"]);
            cells["CE:CE"].Copy(cells["K:K"]);
            cells["CG:CG"].Copy(cells["L:L"]);
            cells["AL:AL"].Copy(cells["M:M"]);
            cells["AP:AP"].Copy(cells["N:N"]);
            cells["AM:AM"].Copy(cells["O:O"]);
            cells["AQ:AQ"].Copy(cells["P:P"]);
            cells["AN:AN"].Copy(cells["Q:Q"]);
            cells["AR:AR"].Copy(cells["R:R"]);
            cells["AO:AO"].Copy(cells["S:S"]);
            cells["AS:AS"].Copy(cells["T:T"]);

            cells["DS:DS"].Copy(cells["U:U"]);
            cells["DT:DT"].Copy(cells["V:V"]);



            // Formatando celulas de quantidade de horas
            cells["I:V"].NumberFormat = "[h]:mm";

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "CNPJ";
            cells["C1"].Formula = "Endereço";
            cells["D1"].Formula = "Cidade";
            cells["E1"].Formula = "UF";
            cells["F1"].Formula = "Data";

            cells["G1"].Formula = "Funcionário";
            cells["H1"].Formula = "Código";
            cells["I1"].Formula = "Jorn.Ent1";
            cells["J1"].Formula = "Jorn.Sai1";
            cells["K1"].Formula = "Jorn.Ent2";
            cells["L1"].Formula = "Jorn.Sai2";
            cells["M1"].Formula = "Ent 1";
            cells["N1"].Formula = "Sai 1";
            cells["O1"].Formula = "Ent 2";
            cells["P1"].Formula = "Sai 2";
            cells["Q1"].Formula = "Ent 3";
            cells["R1"].Formula = "Sai 3";
            cells["S1"].Formula = "Ent 4";
            cells["T1"].Formula = "Sai 4";
            cells["U1"].Formula = "Total Intervalo Real";
            cells["V1"].Formula = "Total Intervalo Prev";


            // Removendo colunas do dataset que não podem ser exportadas
            cells["X:EZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a primeira coluna
            IRange cells2 = worksheet.Cells["A1:A1"];
            cells2.Font.Bold = true;
            cells2.HorizontalAlignment = HAlign.Center;
            cells2.EntireColumn.AutoFit();


            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Funcionario_Presenca(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Endereço";
            cells["C1"].Formula = "CNPJ";
            cells["D1"].Formula = "Nome";
            cells["E1"].Formula = "Código";
            cells["F1"].Formula = "Departamento";
            cells["G1"].Formula = "Ent 1";
            cells["H1"].Formula = "Sai 1";
            cells["I1"].Formula = "Ent 2";
            cells["J1"].Formula = "Sai 2";
            cells["K1"].Formula = "Ent 3";
            cells["L1"].Formula = "Sai 3";
            cells["M1"].Formula = "Ent 4";
            cells["N1"].Formula = "Sai 4";
            cells["O1"].Formula = "Ent 5";
            cells["P1"].Formula = "Sai 5";
            cells["Q1"].Formula = "Ent 6";
            cells["R1"].Formula = "Sai 6";
            cells["S1"].Formula = "Ent 7";
            cells["T1"].Formula = "Sai 7";
            cells["U1"].Formula = "Ent 8";
            cells["V1"].Formula = "Sai 8";


            // Removendo colunas do dataset que não podem ser exportadas
            cells["X:AZ"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Horario_Descricao(DataTable dados)
        {
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dados);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;
 
            //Insere colunas que farão parte da planilha
            cells["A:A"].Insert();
            cells["B:B"].Insert();
            cells["C:C"].Insert();


            //Insere os valores nas colunas criadas
            cells["J:J"].Copy(cells["A:A"]);
            cells["F:F"].Copy(cells["B:B"]);
            cells["E:E"].Copy(cells["C:C"]);

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Descrição do Horário";
            cells["B1"].Formula = "Descrição do Turno";
            cells["C1"].Formula = "Código";


            // Removendo colunas do dataset que não podem ser exportadas
            cells["D:Z"].Delete(DeleteShiftDirection.Left);
   
            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Afastamento_por_Ocorrencia(DataTable dados, string emp)
        {
            string[] selectedColumns = new[] { "empresa", "nome2", "descricao", "datai", "dataf", "ocorrencia"};
            DataTable dt = new DataView(dados).ToTable(false, selectedColumns);
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dt);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Nome";
            cells["C1"].Formula = "Descrição do Departamento";
            cells["D1"].Formula = "Dt Inicial";
            cells["E1"].Formula = "Dt Final";
            cells["F1"].Formula = "Ocorrência";
            cells["C:C"].NumberFormat = "@";

            // Removendo colunas do dataset que não podem ser exportadas
            //cells["G:Z"].Delete(DeleteShiftDirection.Left);

            // Reconfigura a primeira coluna
            IRange cells2 = worksheet.Cells["A1:A1"];
            cells2.Font.Bold = true;
            cells2.HorizontalAlignment = HAlign.Center;
            cells2.EntireColumn.AutoFit();
                      
            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
        public static byte[] Relatorio_Afastamento_por_Tipo(DataTable dados, string emp)
        {
            string[] selectedColumns = new[] { "empresa", "nome2", "descricao", "datai", "dataf", "ocorrencia" };

            DataTable dt = new DataView(dados).ToTable(false, selectedColumns);
            IWorkbook workbook = Factory.GetWorkbook();
            GeraRelatorio(workbook, dt);

            IWorksheet worksheet = workbook.Worksheets[0];
            IRange cells = worksheet.Cells;

            // Corrigindo o nome dos cabeçalhos
            cells["A1"].Formula = "Empresa";
            cells["B1"].Formula = "Nome";
            cells["C1"].Formula = "Descrição do Departamento";
            cells["D1"].Formula = "Dt Inicial";
            cells["E1"].Formula = "Dt Final";
            cells["F1"].Formula = "Ocorrência";
            cells["C:C"].NumberFormat = "@";
            // Reconfigura a primeira coluna
            IRange cells2 = worksheet.Cells["A1:A1"];
            cells2.Font.Bold = true;
            cells2.HorizontalAlignment = HAlign.Center;
            cells2.EntireColumn.AutoFit();

            // Reconfigura a largura das colunas
            worksheet.UsedRange.Columns.AutoFit();

            return workbook.SaveToMemory(FileFormat.OpenXMLWorkbook);
        }
    }
}
