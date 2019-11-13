using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using DAL.SQL;
using System.Threading.Tasks;
using BLL.Util;

namespace BLL
{
    class ExportaAFDT
    {
        System.Windows.Forms.ProgressBar exportaPB;

        //Dados da empresa

        private Modelo.Empresa empresa;

        //Dados do funcionario

        private DataTable funcionariosDT;

        //Dados das marcaçoes
        private List<Modelo.Marcacao> marcacao_list = new List<Modelo.Marcacao>();
        private List<Modelo.REP> reps;

        private DAL.IEmpresa dalEmpresa;
        private DAL.IREP dalRep;
        private DAL.IMarcacao dalMarcacao;
        private DAL.IFuncionario dalFuncionario;

        private string nome_arquivo { get; set; }
        private int sequencial_cnt = 1;
        private StreamWriter afdt_file;
        private MemoryStream afdt_ms;
        private int id_empresa { get; set; }

        //Variaveis do cabeçalho
        private string cnpj_cpf_empregador = "";
        private byte ident_empregador;
        private string cei_empregador = "";
        private string razaoSocial_nome_empregador = "";
        private DateTime data_inicial;
        private DateTime data_final;
        private DateTime data_hora_geracao; // Contem a data e a hora da geração do arquivo

        //Variaveis dos registros
        private DateTime data_hora_marcacao { get; set; }
        private string pis_empregado = new string(' ', 12);
        private char[] lista_tipo_marc_entr = { 'E', 'E', 'E', 'E', 'E', 'E', 'E', 'E' };
        private char[] lista_tipo_marc_saida = { 'S', 'S', 'S', 'S', 'S', 'S', 'S', 'S' };
        private char[] lista_tipo_reg_entr = { 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O' };
        private char[] lista_tipo_reg_saida = { 'O', 'O', 'O', 'O', 'O', 'O', 'O', 'O' };
        private string[] lista_motivo_entr = { "", "", "", "", "", "", "", "" };
        private string[] lista_motivo_saida = { "", "", "", "", "", "", "", "" };

        private string cabecalho = "";
        private string trailer = "";
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="nome_arquivo"> 
        /// É pressuposto que o arquivo com aquele nome não exista
        /// O teste da existencia deve ser feito na interface grafica e caso exista 
        /// e o usuario concorde esse arquivo deve ser apagado.
        /// </param>
        /// <param name="id_empresa"> Vem da tela do usuario </param>
        /// <param name="data_inicial"> Vem da tela do usuario </param>
        /// <param name="data_final"> Vem da tela do usuario </param>
        public ExportaAFDT(string pNomeArquivo, int pIdEmpresa, DateTime pDataInicial, DateTime pDataFinal, System.Windows.Forms.ProgressBar pExportaPB, string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            //Inicializa as variaveis
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            DataBase db = new DataBase(ConnectionString);
            dalEmpresa = new DAL.SQL.Empresa(db);
            dalMarcacao = new DAL.SQL.Marcacao(db);
            dalRep = new DAL.SQL.REP(db);
            dalFuncionario = new DAL.SQL.Funcionario(db);

            UsuarioLogado = usuarioLogado;
            dalEmpresa.UsuarioLogado = usuarioLogado;
            dalMarcacao.UsuarioLogado = usuarioLogado;
            dalRep.UsuarioLogado = usuarioLogado;
            dalFuncionario.UsuarioLogado = usuarioLogado;

            this.nome_arquivo = pNomeArquivo;
            this.afdt_file = new StreamWriter(pNomeArquivo);
            this.data_hora_geracao = DateTime.Now;
            this.id_empresa = pIdEmpresa;
            this.data_inicial = pDataInicial;
            this.data_final = pDataFinal;


            this.funcionariosDT = dalFuncionario.GetPorEmpresa(id_empresa, false);

            this.exportaPB = pExportaPB;
            this.exportaPB.Value = 0;
            this.exportaPB.Minimum = 0;
            this.exportaPB.Maximum = funcionariosDT.Rows.Count + 1;

            carrega_empresa();
            monta_cabecalho();
            monta_arquivo();
        }

        public ExportaAFDT(ref byte[] arquivoMemoria, int pIdEmpresa, DateTime pDataInicial, DateTime pDataFinal, Modelo.ProgressBar exportaPB, string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            //Inicializa as variaveis
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            DataBase db = new DataBase(ConnectionString);
            dalEmpresa = new DAL.SQL.Empresa(db);
            dalMarcacao = new DAL.SQL.Marcacao(db);
            dalRep = new DAL.SQL.REP(db);
            dalFuncionario = new DAL.SQL.Funcionario(db);

            UsuarioLogado = usuarioLogado;
            dalEmpresa.UsuarioLogado = usuarioLogado;
            dalMarcacao.UsuarioLogado = usuarioLogado;
            dalRep.UsuarioLogado = usuarioLogado;
            dalFuncionario.UsuarioLogado = usuarioLogado;

            afdt_ms = new MemoryStream();

            this.data_hora_geracao = DateTime.Now;
            this.id_empresa = pIdEmpresa;
            this.data_inicial = pDataInicial;
            this.data_final = pDataFinal;

            this.funcionariosDT = dalFuncionario.GetPorEmpresa(id_empresa, true);

            exportaPB.setaMensagem("Exportando arquivo...");
            List<string> linhasArquivo = new List<string>();

            try
            {
                carrega_empresa();
                monta_cabecalho();
                linhasArquivo.Add(cabecalho);
                linhasArquivo.AddRange(grava_registrosWeb(exportaPB));
                monta_trailer();
                linhasArquivo.Add(trailer);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        foreach (var item in linhasArquivo)
                        {
                            writer.WriteLine(item);
                        }
                    }
                    arquivoMemoria = stream.ToArray();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private List<string> grava_registrosWeb(Modelo.ProgressBar exportaPB)
        {
            List<string> retorno = new List<string>();
            if (funcionariosDT != null && funcionariosDT.Rows.Count > 0)
            {
                Dictionary<int, List<string>> rowsPerFunc = new Dictionary<int, List<string>>();
                List<int> idsFunc = dalFuncionario.IdsFuncPeriodoContratado(Modelo.Enumeradores.TipoFiltroFuncionario.Empresa, new List<int>() { id_empresa }, data_inicial, data_final);

                BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
                reps = dalRep.GetAllList();
                Tuple<DateTime, DateTime>[] intervals = {
                    Tuple.Create(data_inicial, data_final)
                };

                string messagePartsProgress = "";
                if (data_final.Subtract(data_inicial).Days + 1 > 31)
                {
                    intervals = data_inicial.GetIntervalsPerMonths(data_final).ToArray();
                }
                
                int count = 0;
                int totalParts = intervals.Count();
                int maxProgress = totalParts > 100 ? totalParts : 100;
                exportaPB.setaMinMaxPB(0, maxProgress);
                exportaPB.setaValorPB(0);
                int percPartIntervalo = maxProgress / totalParts;
                List<Modelo.Marcacao> marcacoes = new List<Modelo.Marcacao>();
                Task<List<Modelo.Marcacao>> tMarcacao = Task.Run(() => bllMarcacao.GetPorFuncionariosContratosAtivosComBilhetes(idsFunc, intervals[0].Item1, intervals[0].Item2));
                for (int i = 0; i < totalParts; i++)
                {
                    var periodo = intervals[i];
                    count++;
                    if (totalParts > 1)
                    {
                        messagePartsProgress = string.Format("Parte {0} de {1}. ", count, intervals.Count());
                    }
                    if (!string.IsNullOrEmpty(messagePartsProgress))
                    {
                        messagePartsProgress += string.Format(" Período {0} a {1}. ", periodo.Item1.ToString("dd/MM/yyyy"), periodo.Item2.ToString("dd/MM/yyyy"));
                    }

                    exportaPB.setaMensagem(string.Format(messagePartsProgress + "Carregando marcações de {2} funcionários", periodo.Item1.ToString("dd/MM/yyyy"), periodo.Item2.ToString("dd/MM/yyyy"), idsFunc.Count()));
                    marcacoes = tMarcacao.Result;

                    if (i +1 < totalParts)
                    {
                        var proximoPeriodo = intervals[i+1];
                        tMarcacao = Task.Run(() => bllMarcacao.GetPorFuncionariosContratosAtivosComBilhetes(idsFunc, periodo.Item1, periodo.Item2));
                    }

                    try
                    {
                        var marcsGroup = marcacoes.GroupBy(g => g.Idfuncionario);
                        int totalFuncs = marcsGroup.Count();
                        int countFuncionario = 0;
                        int controleIncremento = -1;
                        int numeroIncremento = totalFuncs / percPartIntervalo;
                        foreach (var marcsFuncs in marcsGroup)
                        {
                            countFuncionario++;
                            if (!rowsPerFunc.ContainsKey(marcsFuncs.Key))
                            {
                                rowsPerFunc.Add(marcsFuncs.Key, new List<string>());
                            }
                            List<string> linhasFunc = rowsPerFunc[marcsFuncs.Key];
                            var func = funcionariosDT.AsEnumerable()
                                        .Where(row => row.Field<int>("id") == marcsFuncs.Key).CopyToDataTable().Rows[0];

                            exportaPB.setaMensagem(string.Format(messagePartsProgress + "Exportando funcionário {0}/{1}: {2} | {3}", countFuncionario, totalFuncs, func["dscodigo"], func["nome"]));
                            pis_empregado = func["pis"].ToString();
                            marcacao_list = marcsFuncs.ToList();
                            foreach (Modelo.Marcacao marc in marcacao_list)
                            {
                                trata_marcacao(marc.BilhetesMarcacao);
                                linhasFunc.AddRange(grava_marcacoesWeb(marc, exportaPB));
                            }

                            if (controleIncremento == -1 || controleIncremento == numeroIncremento)
                            {
                                exportaPB.incrementaPB(1);
                                controleIncremento = 0;
                            }

                            controleIncremento++;
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }

                exportaPB.setaMensagem("Gerando txt");
                exportaPB.setaValorPB(maxProgress-1);

                retorno.AddRange(rowsPerFunc.SelectMany(s => s.Value).ToList());
            }
            return retorno;
        }

        /// <summary>
        /// Inicializa o Id do funcionario e o pis do mesmo
        /// Pega toda a lista de marcações daquele funcionario e chama o metodo que grava as marcações no arquivo
        /// </summary>
        //ESSE MÉTODO PRECISA SER OTIMIZADO
        //Pegar todas as marcações no período e buscar na lista as marcações referentes a um determinado funcionario
        //Aí fazer a mesma coisa de agora para aquele funcionario
        private void grava_registros()
        {
            int id_func;

            List<Modelo.Marcacao> marcacoes = dalMarcacao.GetPorEmpresa(id_empresa, data_inicial, data_final, true);
            reps = dalRep.GetAllList();
            foreach (DataRow func in funcionariosDT.Rows)
            {
                id_func = (int)func["id"];
                if (func["pis"] != DBNull.Value)
                    pis_empregado = (string)func["pis"];

                marcacao_list = marcacoes.Where(m => m.Idfuncionario == id_func).ToList();
                //marcacao_list = marcacao.GetPorFuncionario(id_func, data_inicial, data_final, true);

                foreach (Modelo.Marcacao marc in marcacao_list)
                {
                    trata_marcacao(marc.BilhetesMarcacao);
                    grava_marcacoes(marc);
                }

                exportaPB.Value++;//Da barra de progresso
            }
        }

        /// <summary>
        /// Método que carrega a empresa
        /// </summary>
        public void carrega_empresa()
        {
            this.empresa = this.dalEmpresa.LoadObject(this.id_empresa);

            inicializa_dados_fixos();
        }

        /// <summary>
        /// Método que inicializa CPF/CNPJ, CEI e Nome da empresa
        /// </summary>
        private void inicializa_dados_fixos()
        {
            if (empresa.Cpf != "")
            {
                this.cnpj_cpf_empregador = this.empresa.Cpf.Replace(".", "").Replace("/", "").Replace("-", "");
                this.ident_empregador = 2;
            }
            else
            {
                this.cnpj_cpf_empregador = this.empresa.Cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
                this.ident_empregador = 1;
            }

            string[] aux = this.empresa.CEI.Split(new char[] { '.', '/' });
            this.cei_empregador = String.Empty;
            foreach (string item in aux)
            {
                this.cei_empregador += item;
            }
            this.razaoSocial_nome_empregador = this.empresa.Nome;
        }

        /// <summary>
        /// Método principal, esse que chama todos que montam o arquivo.
        /// </summary>
        public void monta_arquivo()
        {
            afdt_file.WriteLine(cabecalho);
            grava_registros();
            monta_trailer();
            afdt_file.Write(trailer);
            afdt_file.Close();
        }

        /// <summary>
        /// Métod que monta o cabeçalho do arquivo
        /// </summary>
        private void monta_cabecalho()
        {
            cabecalho = cabecalho.Insert(0, String.Format("{0:000000000}", sequencial_cnt)); //Sequencial do registro no arquivo
            cabecalho = cabecalho.Insert(9, "1"); // Tipo do registro = 1
            cabecalho = cabecalho.Insert(10, ident_empregador.ToString()); // Identificador do empregador 1 = cpf, 2 = cnpj
            cabecalho = cabecalho.Insert(11, String.Format("{0,-14}", cnpj_cpf_empregador));// CPF ou CNPJ do empregador
            cabecalho = cabecalho.Insert(25, String.Format("{0,-12}", cei_empregador)); // CEI do empregador
            cabecalho = cabecalho.Insert(37, String.Format("{0,-150}", razaoSocial_nome_empregador));//Razao Social ou Nome do empregador
            cabecalho = cabecalho.Insert(187, data_inicial.ToString("ddMMyyyy"));
            cabecalho = cabecalho.Insert(195, data_final.ToString("ddMMyyyy"));
            cabecalho = cabecalho.Insert(203, data_hora_geracao.ToString("ddMMyyyyHHmm"));
        }

        private List<string> grava_marcacoesWeb(Modelo.Marcacao pMaracao, Modelo.ProgressBar exportaPB)
        {
            List<string> linhas = new List<string>();
            string num_fabricacao_rep = "";

            string linhaEntrada, linhaSaida;

            Modelo.BilhetesImp objBilhete = null;
            string aux = "";
            DateTime data;
            string[] tiposRegistros = new string[] { "I", "P" };
            int[] entradasMin = pMaracao.GetEntradas();
            int[] saidasMin = pMaracao.GetSaidas();
            string[] saidas = pMaracao.GetSaidasToString();
            string[] entradas = pMaracao.GetEntradasToString();
            string[] num_rel_entradas = pMaracao.GetNumRelogioEntradas();
            string[] num_rel_saidas = pMaracao.GetNumRelogioSaidas();
            for (int i = 1; i < 8; i++)
            {
                string tipoMarcEnt = "E", tipoRegEnt = "O", tipoMarcSai = "S", tipoRegSai = "O", motivo = "";

                //Para cada entrada escreve todo o registro
                if (entradasMin[i - 1] != -1)
                {
                    var tratamentoE = pMaracao.BilhetesMarcacao.Where(t => t.Ent_sai == "E" && t.Posicao == i);
                    if (tratamentoE.Count() > 0)
                    {
                        objBilhete = tratamentoE.Last();
                        motivo = objBilhete.Motivo;
                        data = objBilhete.Data;
                        if (objBilhete.Ocorrencia == 'D')
                        {
                            tipoMarcEnt = "D";
                        }
                        else
                        {
                            aux = objBilhete.Ocorrencia.ToString();
                            if (tiposRegistros.Contains(aux))
                                tipoRegEnt = aux;
                        }
                    }
                    else
                    {
                        data = pMaracao.Data;
                    }

                    linhaEntrada = new string(' ', 155);

                    sequencial_cnt++;//Incrementa o contador

                    //Testa se existe um numero de serie para o relogio, se nao tiver, preenche com espaço em branco
                    var auxRep = reps.Where(r => r.NumRelogio == num_rel_entradas[i - 1]);
                    if (auxRep.Count() > 0)
                        num_fabricacao_rep = auxRep.First().NumSerie;
                    else
                        num_fabricacao_rep = "00000000000000000";

                    linhaEntrada = linhaEntrada.Insert(0, String.Format("{0:000000000}", sequencial_cnt)); //Sequencial do registro no arquivo
                    linhaEntrada = linhaEntrada.Insert(9, "2"); // Tipo do registro = 2 (TR)
                    linhaEntrada = linhaEntrada.Insert(10, data.ToString("ddMMyyyy"));
                    linhaEntrada = linhaEntrada.Insert(18, entradas[i - 1].Substring(0, 2));//Grava as horas no arquivo
                    linhaEntrada = linhaEntrada.Insert(20, entradas[i - 1].Substring(3, 2));//Grava os minutos no arquivo
                    linhaEntrada = linhaEntrada.Insert(22, String.Format("{0,-12}", pis_empregado));//Pis do empregado
                    linhaEntrada = linhaEntrada.Insert(34, String.Format("{0,-17}", num_fabricacao_rep));//Numero de frabricação REP do relogio
                    linhaEntrada = linhaEntrada.Insert(51, tipoMarcEnt);//E= Entrada, S = Saída, D = Desconsiderado
                    linhaEntrada = linhaEntrada.Insert(52, "0" + i.ToString());//Numero sequencia do conjunto E/S. Ex: E1/S1
                    linhaEntrada = linhaEntrada.Insert(54, tipoRegEnt);//O = Original, I = Incluido, P = Intervalo pre-assinalado
                    if (tipoRegEnt != "E" || tipoMarcEnt != "O")
                        linhaEntrada = linhaEntrada.Insert(55, String.Format("{0,-100}", motivo));

                    linhas.Add(linhaEntrada);
                }

                //Para cada saída escreve todo o registro
                if (saidasMin[i - 1] != -1)
                {
                    var tratamentoS = pMaracao.BilhetesMarcacao.Where(t => t.Ent_sai == "S" && t.Posicao == i);
                    if (tratamentoS.Count() > 0)
                    {
                        objBilhete = tratamentoS.Last();
                        motivo = objBilhete.Motivo;
                        data = objBilhete.Data;
                        if (objBilhete.Ocorrencia == 'D')
                        {
                            tipoMarcSai = "D";
                        }
                        else
                        {
                            aux = objBilhete.Ocorrencia.ToString();
                            if (tiposRegistros.Contains(aux))
                                tipoRegSai = aux;
                        }
                    }
                    else
                    {
                        data = pMaracao.Data;
                    }

                    linhaSaida = "";

                    sequencial_cnt++;//Incrementa o contador
                    var auxRep = reps.Where(r => r.NumRelogio == num_rel_saidas[i - 1]);
                    if (auxRep.Count() > 0)
                        num_fabricacao_rep = auxRep.First().NumSerie;

                    linhaSaida = linhaSaida.Insert(0, String.Format("{0:000000000}", sequencial_cnt)); //Sequencial do registro no arquivo
                    linhaSaida = linhaSaida.Insert(9, "2"); // Tipo do registro = 2 (TR)
                    linhaSaida = linhaSaida.Insert(10, data.ToString("ddMMyyyy"));
                    linhaSaida = linhaSaida.Insert(18, saidas[i - 1].Substring(0, 2));//Grava as horas no arquivo
                    linhaSaida = linhaSaida.Insert(20, saidas[i - 1].Substring(3, 2));//Grava os minutos no arquivo
                    linhaSaida = linhaSaida.Insert(22, String.Format("{0,-12}", pis_empregado));//Pis do empregado
                    linhaSaida = linhaSaida.Insert(34, String.Format("{0,-17}", num_fabricacao_rep));//Numero de frabricação REP do relogio
                    linhaSaida = linhaSaida.Insert(51, tipoMarcSai);//E= Entrada, S = Saída, D = Desconsiderado
                    linhaSaida = linhaSaida.Insert(52, "0" + i.ToString());//Numero sequencia do conjunto E/S. Ex: E1/S1
                    linhaSaida = linhaSaida.Insert(54, tipoRegSai);//O = Original, I = Incluido, P = Intervalo pre-assinalado
                    if (tipoMarcSai != "S" || tipoRegSai != "O")
                        linhaSaida = linhaSaida.Insert(55, String.Format("{0,-100}", motivo));

                    linhas.Add(linhaSaida);
                }
            }
            return linhas;
        }

        /// <summary>
        /// Os registros dos funcionários são do tipo:
        /// Posição 1-9 10  11 - 18 19-22 23-34 35-51  52   53-54   55    56 - 155
        /// Varlo   SR  2  ddmmaaaa hhmm   PIS  REP   E,S,D En/Sn  O,I,P  Motivo
        /// </summary>
        /// <param name="pMaracao"></param>
        private void grava_marcacoes(Modelo.Marcacao pMaracao)
        {
            string num_fabricacao_rep = "";

            string linhaEntrada, linhaSaida;

            Modelo.BilhetesImp objBilhete = null;
            string aux = "";
            DateTime data;
            string[] tiposRegistros = new string[] { "I", "P" };
            for (int i = 1; i < 8; i++)
            {
                string tipoMarcEnt = "E", tipoRegEnt = "O", tipoMarcSai = "S", tipoRegSai = "O", motivo = "";

                //Para cada entrada escreve todo o registro
                if (pMaracao.GetEntradas()[i - 1] != -1)
                {
                    var tratamentoE = pMaracao.BilhetesMarcacao.Where(t => t.Ent_sai == "E" && t.Posicao == i);
                    if (tratamentoE.Count() > 0)
                    {
                        objBilhete = tratamentoE.Last();
                        motivo = objBilhete.Motivo;
                        data = objBilhete.Data;
                        if (objBilhete.Ocorrencia == 'D')
                        {
                            tipoMarcEnt = "D";
                        }
                        else
                        {
                            aux = objBilhete.Ocorrencia.ToString();
                            if (tiposRegistros.Contains(aux))
                                tipoRegEnt = aux;
                        }
                    }
                    else
                    {
                        data = pMaracao.Data;
                    }

                    linhaEntrada = new string(' ', 155);

                    sequencial_cnt++;//Incrementa o contador

                    //Testa se existe um numero de serie para o relogio, se nao tiver, preenche com espaço em branco
                    var auxRep = reps.Where(r => r.NumRelogio == pMaracao.GetNumRelogioEntradas()[i - 1]);
                    if (auxRep.Count() > 0)
                        num_fabricacao_rep = num_fabricacao_rep.Insert(0, auxRep.First().NumSerie);
                    else
                        num_fabricacao_rep = "00000000000000000";

                    linhaEntrada = linhaEntrada.Insert(0, String.Format("{0:000000000}", sequencial_cnt)); //Sequencial do registro no arquivo
                    linhaEntrada = linhaEntrada.Insert(9, "2"); // Tipo do registro = 2 (TR)
                    linhaEntrada = linhaEntrada.Insert(10, data.ToString("ddMMyyyy"));
                    linhaEntrada = linhaEntrada.Insert(18, pMaracao.GetEntradasToString()[i - 1].Substring(0, 2));//Grava as horas no arquivo
                    linhaEntrada = linhaEntrada.Insert(20, pMaracao.GetEntradasToString()[i - 1].Substring(3, 2));//Grava os minutos no arquivo
                    linhaEntrada = linhaEntrada.Insert(22, String.Format("{0,-12}", pis_empregado));//Pis do empregado
                    linhaEntrada = linhaEntrada.Insert(34, String.Format("{0,-17}", num_fabricacao_rep));//Numero de frabricação REP do relogio
                    linhaEntrada = linhaEntrada.Insert(51, tipoMarcEnt);//E= Entrada, S = Saída, D = Desconsiderado
                    linhaEntrada = linhaEntrada.Insert(52, "0" + i.ToString());//Numero sequencia do conjunto E/S. Ex: E1/S1
                    linhaEntrada = linhaEntrada.Insert(54, tipoRegEnt);//O = Original, I = Incluido, P = Intervalo pre-assinalado
                    if (tipoRegEnt != "E" || tipoMarcEnt != "O")
                        linhaEntrada = linhaEntrada.Insert(55, String.Format("{0,-100}", motivo));

                    afdt_file.WriteLine(linhaEntrada);

                }

                //Para cada saída escreve todo o registro
                if (pMaracao.GetSaidas()[i - 1] != -1)
                {
                    var tratamentoS = pMaracao.BilhetesMarcacao.Where(t => t.Ent_sai == "S" && t.Posicao == i);
                    if (tratamentoS.Count() > 0)
                    {
                        objBilhete = tratamentoS.Last();
                        motivo = objBilhete.Motivo;
                        data = objBilhete.Data;
                        if (objBilhete.Ocorrencia == 'D')
                        {
                            tipoMarcSai = "D";
                        }
                        else
                        {
                            aux = objBilhete.Ocorrencia.ToString();
                            if (tiposRegistros.Contains(aux))
                                tipoRegSai = aux;
                        }
                    }
                    else
                    {
                        data = pMaracao.Data;
                    }

                    linhaSaida = "";

                    sequencial_cnt++;//Incrementa o contador
                    var auxRep = reps.Where(r => r.NumRelogio == pMaracao.GetNumRelogioEntradas()[i - 1]);
                    if (auxRep.Count() > 0)
                        num_fabricacao_rep = auxRep.First().NumSerie;

                    linhaSaida = linhaSaida.Insert(0, String.Format("{0:000000000}", sequencial_cnt)); //Sequencial do registro no arquivo
                    linhaSaida = linhaSaida.Insert(9, "2"); // Tipo do registro = 2 (TR)
                    linhaSaida = linhaSaida.Insert(10, data.ToString("ddMMyyyy"));
                    linhaSaida = linhaSaida.Insert(18, pMaracao.GetSaidasToString()[i - 1].Substring(0, 2));//Grava as horas no arquivo
                    linhaSaida = linhaSaida.Insert(20, pMaracao.GetSaidasToString()[i - 1].Substring(3, 2));//Grava os minutos no arquivo
                    linhaSaida = linhaSaida.Insert(22, String.Format("{0,-12}", pis_empregado));//Pis do empregado
                    linhaSaida = linhaSaida.Insert(34, String.Format("{0,-17}", num_fabricacao_rep));//Numero de frabricação REP do relogio
                    linhaSaida = linhaSaida.Insert(51, tipoMarcSai);//E= Entrada, S = Saída, D = Desconsiderado
                    linhaSaida = linhaSaida.Insert(52, "0" + i.ToString());//Numero sequencia do conjunto E/S. Ex: E1/S1
                    linhaSaida = linhaSaida.Insert(54, tipoRegSai);//O = Original, I = Incluido, P = Intervalo pre-assinalado
                    if (tipoMarcSai != "S" || tipoRegSai != "O")
                        linhaSaida = linhaSaida.Insert(55, String.Format("{0,-100}", motivo));

                    afdt_file.WriteLine(linhaSaida);
                }
            }
        }

        /// <summary>
        /// Pega todos os elementos da lista de tratamento das marcações e para cada uma
        /// compara a qual marcação ela é referente e preenche um vetor com os valores encontrados
        /// </summary>
        /// <param name="pListaTratamentos"></param>
        private void trata_marcacao(List<Modelo.BilhetesImp> pListaTratamentos)
        {
            int j = 0;

            if (pListaTratamentos.Count != 0)
            {
                while (j < pListaTratamentos.Count)
                {
                    //Testa se txtEntrada1 tem ocorrencia, faz o mesmo para todas as entradas e saídas
                    for (int i = 1; i <= 8; i++)
                    {
                        if (pListaTratamentos[j].Ent_sai == "E" && pListaTratamentos[j].Posicao == i)
                        {
                            lista_motivo_entr[i - 1] = pListaTratamentos[j].Motivo;

                            switch (pListaTratamentos[j].Ocorrencia)
                            {
                                case 'D': lista_tipo_marc_entr[i - 1] = 'D'; break;
                                case 'I': lista_tipo_reg_entr[i - 1] = 'I'; break;
                                case 'P': lista_tipo_reg_entr[i - 1] = 'P'; break;
                                default: break;
                            }
                        }

                        if (pListaTratamentos[j].Ent_sai == "S" && pListaTratamentos[j].Posicao == i)
                        {
                            lista_motivo_saida[i - 1] = pListaTratamentos[j].Motivo;
                            switch (pListaTratamentos[j].Ocorrencia)
                            {
                                case 'D': lista_tipo_marc_saida[i - 1] = 'D'; break;
                                case 'I': lista_tipo_reg_saida[i - 1] = 'I'; break;
                                case 'P': lista_tipo_reg_saida[i - 1] = 'P'; break;
                                default: break;
                            }
                        }
                    }//Fim do for
                    j++;
                }//Fim do while                
            }
        }

        private void monta_trailer()
        {
            sequencial_cnt++;//Incrementa o contador
            trailer = String.Format("{0:000000000}", sequencial_cnt); //Sequencial do registro no arquivo
            trailer = trailer + "9"; // Tipo do registro = 9 (TR)
        }
    }
}
