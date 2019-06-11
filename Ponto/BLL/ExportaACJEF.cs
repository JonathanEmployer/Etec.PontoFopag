using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using DAL.SQL;
using System.Linq;

namespace BLL
{
    public class ExportaACJEF
    {
        public struct IntervaloStruct
        {
            public string inicio;
            public string fim;
        };

        public struct HorasExtrasStruct
        {
            public string horasExtras;
            public decimal percentual;
            public char tipo;
        };

        public struct BancoDeHorasStruct
        {
            public char sinal;
            public string horas;
        };

        private List<Modelo.Marcacao> marcacao_list = new List<Modelo.Marcacao>();

        private string nome_arquivo;
        private int sequencial_cnt = 1;
        private StreamWriter acjef_file;
        private MemoryStream acjef_ms;

        System.Windows.Forms.ProgressBar pbExporta;

        //Dados da empresa
        private Modelo.Empresa empresa;
        private int id_empresa;
        private List<Modelo.Jornada> horariosEmpresaList;

        //Dados do funcionario
        private DataTable marcacoesDT;
        private string pis_empregado = "";

        //Outras dals
        private DAL.IEmpresa dalEmpresa;
        private DAL.IMarcacao dalMarcacao;
        private DAL.IBancoHoras dalBancoHoras;
        private DAL.IFechamentoBHD dalFechamentoBHD;
        private DAL.IFechamentoBH dalFechamentoBH;
        private DAL.IJornadaAlternativa dalJornadaAlternativa;
        private DAL.IJornada dalJornada;

        //Variaveis do cabeçalho
        private string cnpj_cpf_empregador;
        private string cei_empregador;
        private string razaoSocial_nome_empregador;
        private byte ident_empregador;
        private DateTime data_inicial;
        private DateTime data_final;
        private DateTime data_hora_geracao; // Contem a data e a hora da geração do arquivo

        //Variaveis dos detalhes
        private HorasExtrasStruct[] horasExtras;
        private Modelo.TotalHoras objTotalHoras;
        private BancoDeHorasStruct bancoDeHoras;

        private StringBuilder todosDados = new StringBuilder();
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public ExportaACJEF(string pNomeArquivo, int pIdEmpresa, DateTime pDataInicial, DateTime pDataFinal, System.Windows.Forms.ProgressBar pbExporta, string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            //Inicializa as variaveis
            this.data_hora_geracao = DateTime.Now;
            this.id_empresa = pIdEmpresa;
            this.data_inicial = pDataInicial;
            this.data_final = pDataFinal;

            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    DataBase db = new DataBase(ConnectionString);
                    dalEmpresa = new DAL.SQL.Empresa(db);
                    dalMarcacao = new DAL.SQL.Marcacao(db);
                    dalBancoHoras = new DAL.SQL.BancoHoras(db);
                    dalFechamentoBHD = new DAL.SQL.FechamentoBHD(db);
                    dalFechamentoBH = new DAL.SQL.FechamentoBH(db);
                    dalJornadaAlternativa = new DAL.SQL.JornadaAlternativa(db);
                    dalJornada = new DAL.SQL.Jornada(db);
                    break;
                case 2:
                    dalEmpresa = DAL.FB.Empresa.GetInstancia;
                    dalMarcacao = DAL.FB.Marcacao.GetInstancia;
                    dalBancoHoras = DAL.FB.BancoHoras.GetInstancia;
                    dalFechamentoBH = DAL.FB.FechamentoBH.GetInstancia;
                    dalFechamentoBHD = DAL.FB.FechamentoBHD.GetInstancia;
                    dalJornada = DAL.FB.Jornada.GetInstancia;
                    dalJornadaAlternativa = DAL.FB.JornadaAlternativa.GetInstancia;
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalEmpresa.UsuarioLogado = usuarioLogado;
            dalMarcacao.UsuarioLogado = usuarioLogado;
            dalBancoHoras.UsuarioLogado = usuarioLogado;
            dalFechamentoBHD.UsuarioLogado = usuarioLogado;
            dalFechamentoBH.UsuarioLogado = usuarioLogado;
            dalJornadaAlternativa.UsuarioLogado = usuarioLogado;
            dalJornada.UsuarioLogado = usuarioLogado;
            


            this.marcacoesDT = dalMarcacao.GetParaACJEF(id_empresa, data_inicial, data_final, false);
            this.horariosEmpresaList = dalJornada.getTodosHorariosDaEmpresa(this.id_empresa);
            //this.horariosEmpresaList = bllHorario.getTodosHorariosDaEmpresa(this.id_empresa);

            this.pbExporta = pbExporta;
            this.pbExporta.Value = 0;
            this.pbExporta.Minimum = 0;
            TimeSpan dias = data_final - data_inicial;
            int numMarc = marcacoesDT.Rows.Count;
            this.pbExporta.Maximum = (numMarc + horariosEmpresaList.Count);

            carrega_empresa();
            monta_arquivo();

            this.nome_arquivo = pNomeArquivo;
            this.acjef_file = new StreamWriter(pNomeArquivo, false, Encoding.Unicode);
            acjef_file.Write(todosDados.ToString());
            acjef_file.Close();
        }

        public ExportaACJEF(ref byte[] arquivoMemoria, int pIdEmpresa, DateTime pDataInicial, DateTime pDataFinal, Modelo.ProgressBar exportaPB, string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            acjef_ms = new MemoryStream();
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (Modelo.cwkGlobal.BD)
            {
                case 1:
                    DataBase db = new DataBase(ConnectionString);
                    dalEmpresa = new DAL.SQL.Empresa(db);
                    dalMarcacao = new DAL.SQL.Marcacao(db);
                    dalBancoHoras = new DAL.SQL.BancoHoras(db);
                    dalFechamentoBHD = new DAL.SQL.FechamentoBHD(db);
                    dalFechamentoBH = new DAL.SQL.FechamentoBH(db);
                    dalJornadaAlternativa = new DAL.SQL.JornadaAlternativa(db);
                    dalJornada = new DAL.SQL.Jornada(db);
                    break;
                case 2:
                    dalEmpresa = DAL.FB.Empresa.GetInstancia;
                    dalMarcacao = DAL.FB.Marcacao.GetInstancia;
                    dalBancoHoras = DAL.FB.BancoHoras.GetInstancia;
                    dalFechamentoBH = DAL.FB.FechamentoBH.GetInstancia;
                    dalFechamentoBHD = DAL.FB.FechamentoBHD.GetInstancia;
                    dalJornada = DAL.FB.Jornada.GetInstancia;
                    dalJornadaAlternativa = DAL.FB.JornadaAlternativa.GetInstancia;
                    break;
            }
            UsuarioLogado = usuarioLogado;
            dalEmpresa.UsuarioLogado = usuarioLogado;
            dalMarcacao.UsuarioLogado = usuarioLogado;
            dalBancoHoras.UsuarioLogado = usuarioLogado;
            dalFechamentoBHD.UsuarioLogado = usuarioLogado;
            dalFechamentoBH.UsuarioLogado = usuarioLogado;
            dalJornadaAlternativa.UsuarioLogado = usuarioLogado;
            dalJornada.UsuarioLogado = usuarioLogado;

            this.data_hora_geracao = DateTime.Now;
            this.id_empresa = pIdEmpresa;
            this.data_inicial = pDataInicial;
            this.data_final = pDataFinal;

            this.marcacoesDT = dalMarcacao.GetParaACJEF(id_empresa, data_inicial, data_final, true);
            this.horariosEmpresaList = dalJornada.getTodosHorariosDaEmpresa(this.id_empresa);
            
            TimeSpan dias = data_final - data_inicial;
            int numMarc = marcacoesDT.Rows.Count;

            exportaPB.setaMinMaxPB(0, numMarc + horariosEmpresaList.Count);
            exportaPB.setaValorPB(0);
            exportaPB.setaMensagem("Exportando arquivo...");

            carrega_empresa();
            this.monta_cabecalho();
            this.grava_horarios_contratuaisWeb(exportaPB);
            this.carrega_marcacoes_web(exportaPB);
            this.gravaTrailer();

            var bytes = Encoding.UTF8.GetBytes(todosDados.ToString());
            acjef_ms.Write(bytes, 0, bytes.Length);
            arquivoMemoria = acjef_ms.ToArray();
        }

        /// <summary>
        /// Esse método carrega a empresa e já formata os dados da mesma para ficarem no formato a ser gravado no arquivo
        /// </summary>
        //PAM - 15/12/2009
        public void carrega_empresa()
        {
            this.empresa = this.dalEmpresa.LoadObject(this.id_empresa);

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
        /// Método principal, chama todos os outros para montar o arquivo
        /// </summary>
        //PAM - 15/12/2009
        public void monta_arquivo()
        {
            this.monta_cabecalho();
            this.grava_horarios_contratuais();
            this.carrega_marcacoes();//Esse metodo chama o que grava efetivamente no arquivo
            this.gravaTrailer();
        }

        /// <summary>
        /// Método que grava no arquivo o cabeçalho
        /// </summary>
        //PAM - 15/12/2009
        private void monta_cabecalho()
        {
            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append(String.Format("{0:000000000}", sequencial_cnt)); //Sequencial do registro no arquivo
            cabecalho.Append("1"); // Tipo do registro = 1
            cabecalho.Append(ident_empregador.ToString()); // Identificador do empregador 1 = cpf, 2 = cnpj
            cabecalho.Append(String.Format("{0,-14}", cnpj_cpf_empregador));// CPF ou CNPJ do empregador
            cabecalho.Append(String.Format("{0,-12}", cei_empregador)); // CEI do empregador
            cabecalho.Append(String.Format("{0,-150}", razaoSocial_nome_empregador));//Razao Social ou Nome do empregador
            cabecalho.Append(data_inicial.ToString("ddMMyyyy"));
            cabecalho.Append(data_final.ToString("ddMMyyyy"));
            cabecalho.Append(data_hora_geracao.ToString("ddMMyyyyHHmm"));

            todosDados.AppendLine(cabecalho.ToString());
        }

        /// <summary>
        /// Método que grava no arquivo os horarios contratuais
        /// </summary>
        //PAM - 15/12/2009
        private void grava_horarios_contratuais()
        {
            List<IntervaloStruct> listaIntervalos = new List<IntervaloStruct>();
            string fimDaJornada;

            //Para todos os itens de tipos de horario, escreve aqui
            foreach (Modelo.Jornada horario in horariosEmpresaList)
            {
                StringBuilder horariosContratuais = new StringBuilder();
                sequencial_cnt++;//Incrementa o contador

                horariosContratuais.Append(String.Format("{0:000000000}", sequencial_cnt)); //Sequencial do registro no arquivo
                horariosContratuais.Append("2"); // Tipo do registro = 2
                horariosContratuais.Append(String.Format("{0:0000}", horario.Codigo));//Codigo do horario
                horariosContratuais.Append(String.Format("{0,-4}", horario.Entrada_1 != "--:--" ? horario.Entrada_1.Replace(":", "") : ""));//Entrada da Jornada
                listaIntervalos = PegaIntervalos(horario, out fimDaJornada);

                horariosContratuais.Append(String.Format("{0,-4}", fimDaJornada != "--:--" ? fimDaJornada.Replace(":", "") : ""));
                //No caso de não ter nenhum intervalo, optou-se por escrever no horario de entrada e saida do intervalo
                if (listaIntervalos.Count == 0)
                {
                    horariosContratuais.Append("0000");//Inicio do intervalos
                    horariosContratuais.Append("0000");//Fim do intervalo
                }
                else
                {// Se tem intervalos, escreve todos os intervalos existentes
                    int i = 22;
                    foreach (IntervaloStruct inter in listaIntervalos)
                    {
                        horariosContratuais.Append(inter.inicio.Replace(":", ""));
                        horariosContratuais.Append(inter.fim.Replace(":", ""));
                        i = i + 4;
                    }
                }

                todosDados.AppendLine(horariosContratuais.ToString());
                pbExporta.Value++;
            }
        }

        private void grava_horarios_contratuaisWeb(Modelo.ProgressBar exportaPB)
        {
            List<IntervaloStruct> listaIntervalos = new List<IntervaloStruct>();
            string fimDaJornada;

            //Para todos os itens de tipos de horario, escreve aqui
            foreach (Modelo.Jornada horario in horariosEmpresaList)
            {
                StringBuilder horariosContratuais = new StringBuilder();
                sequencial_cnt++;//Incrementa o contador

                horariosContratuais.Append(String.Format("{0:000000000}", sequencial_cnt)); //Sequencial do registro no arquivo
                horariosContratuais.Append("2"); // Tipo do registro = 2
                horariosContratuais.Append(String.Format("{0:0000}", horario.Codigo));//Codigo do horario
                horariosContratuais.Append(String.Format("{0,-4}", horario.Entrada_1 != "--:--" ? horario.Entrada_1.Replace(":", "") : ""));//Entrada da Jornada
                listaIntervalos = PegaIntervalos(horario, out fimDaJornada);

                horariosContratuais.Append(String.Format("{0,-4}", fimDaJornada != "--:--" ? fimDaJornada.Replace(":", "") : ""));
                //No caso de não ter nenhum intervalo, optou-se por escrever no horario de entrada e saida do intervalo
                if (listaIntervalos.Count == 0)
                {
                    horariosContratuais.Append("0000");//Inicio do intervalos
                    horariosContratuais.Append("0000");//Fim do intervalo
                }
                else
                {// Se tem intervalos, escreve todos os intervalos existentes
                    int i = 22;
                    foreach (IntervaloStruct inter in listaIntervalos)
                    {
                        horariosContratuais.Append(inter.inicio.Replace(":", ""));
                        horariosContratuais.Append(inter.fim.Replace(":", ""));
                        i = i + 4;
                    }
                }

                todosDados.AppendLine(horariosContratuais.ToString());
                exportaPB.incrementaPB(1);
            }
        }

        /// <summary>
        /// Esse método pega o fim da jornada e atribui para a variavel.
        /// Também testa os intervalos e os coloca em uma lista para serem escritos no arquivo.
        /// </summary>
        /// <param name="pObjHorario">Objeto Horario.</param>
        /// <param name="pListaIntervalos">Lista de intervalos retornada pelo metodo, se nao tem intervalos count = 0.</param>
        /// <param name="pFimDaJornada">Fim da jornada.</param>
        //PAM - 15/12/2009
        private List<IntervaloStruct> PegaIntervalos(Modelo.Jornada pObjHorario, out string pFimDaJornada)
        {
            IntervaloStruct intervalo;
            List<IntervaloStruct> listaIntervalos = new List<IntervaloStruct>();

            //Não tem intervalos
            if (pObjHorario.Saida_2 == "--:--")
                pFimDaJornada = pObjHorario.Saida_1;

            //Tem apenas um intervalo
            else if (pObjHorario.Saida_3 == "--:--")
            {
                pFimDaJornada = pObjHorario.Saida_2;
                intervalo.inicio = pObjHorario.Saida_1;
                intervalo.fim = pObjHorario.Entrada_2;
                listaIntervalos.Add(intervalo);
            }

            //Tem dois intervalos
            else if (pObjHorario.Entrada_4 == "--:--")
            {
                pFimDaJornada = pObjHorario.Saida_3;
                intervalo.inicio = pObjHorario.Saida_1;
                intervalo.fim = pObjHorario.Entrada_2;
                listaIntervalos.Add(intervalo);
                intervalo.inicio = pObjHorario.Saida_2;
                intervalo.fim = pObjHorario.Entrada_3;
                listaIntervalos.Add(intervalo);
            }
            //Tem 3 intervalos, esse é o maximo possivel
            else
            {
                pFimDaJornada = pObjHorario.Saida_4;
                intervalo.inicio = pObjHorario.Saida_1;
                intervalo.fim = pObjHorario.Entrada_2;
                listaIntervalos.Add(intervalo);
                intervalo.inicio = pObjHorario.Saida_2;
                intervalo.fim = pObjHorario.Entrada_3;
                listaIntervalos.Add(intervalo);
                intervalo.inicio = pObjHorario.Saida_3;
                intervalo.fim = pObjHorario.Entrada_4;
                listaIntervalos.Add(intervalo);
            }

            return listaIntervalos;
        }

        private void carrega_marcacoes_web(Modelo.ProgressBar exportaPB)
        {
            int id_func, id_empresa, id_departamento, id_funcao, codigo_horario;
            string entrada_1;
            Modelo.Horario objHorario = new Modelo.Horario();
            
            DateTime data;
            DataTable dt = new DataTable();

            //Cria as colunas da datatable
            foreach (DataColumn col in marcacoesDT.Columns)
            {
                dt.Columns.Add(col.ColumnName);
            }

            List<int> idsFuncs = new List<int>();
            if (marcacoesDT.Rows.Count > 0)
            {
                idsFuncs = marcacoesDT.AsEnumerable().Select(s => Convert.ToInt32(s.Field<int>("idfuncionario"))).ToList();
            }

            List<Modelo.BancoHoras> bancoHorasList = dalBancoHoras.GetAllListFuncs(false, idsFuncs);
            List<Modelo.FechamentoBH> fechamentoBHList = dalFechamentoBH.GetAllListFuncs(idsFuncs, true);
            List<Modelo.FechamentoBHD> fechamentoBHDList = dalFechamentoBHD.getPorListaFuncionario(idsFuncs);
            List<Modelo.JornadaAlternativa> jornadasList = dalJornadaAlternativa.GetPeriodoFuncionarios(data_inicial, data_final, idsFuncs);

            exportaPB.setaValorPB(0);
            foreach (DataRow func in marcacoesDT.Rows)
            {
                objTotalHoras = new Modelo.TotalHoras(data_inicial, data_final);
                horasExtras = new HorasExtrasStruct[4];
                codigo_horario = 0;
                id_func = (int)func["id"];
                id_empresa = (int)func["idempresa"];
                id_departamento = (int)func["iddepartamento"];
                id_funcao = (int)func["idfuncao"];
                if ((int)func["tipohorario"] == 1)
                {
                    if (func["codigohorario"] is DBNull)
                        continue;
                    else
                        codigo_horario = (int)func["codigohorario"];
                }
                if ((int)func["tipohorario"] == 2)
                {
                    if (func["codigohorario1"] is DBNull)
                        continue;
                    else
                        codigo_horario = (int)func["codigohorario1"];
                }
                entrada_1 = (string)func["entrada_1"];
                data = Convert.ToDateTime(func["data"]);

                if (func["pis"] != DBNull.Value)
                    pis_empregado = (string)func["pis"];

                dt.Rows.Clear();
                dt.Rows.Add(func.ItemArray);

                var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(id_empresa, id_departamento, id_func, id_funcao, data, data, jornadasList, dt, null, null, ConnectionString, UsuarioLogado);
                totalizadorHoras.CalcularAtraso = false;
                totalizadorHoras.TotalizeHoras(objTotalHoras);

                this.setaHorasExtras(func);
                this.setaTotalBancoDeHoras(func);
                this.grava_marcacoes(codigo_horario, data, entrada_1);
                exportaPB.incrementaPB(1);
            }           
        }
        /// <summary>
        /// Processo que carrega todos os funcionarios e para cada um deles faz a totalização das horas extras para
        /// aquele período e chama o método que grava essas marcações.
        /// </summary>
        //PAM - 22/12/2009
        private void carrega_marcacoes()
        {
            int id_func, id_empresa, id_departamento, id_funcao, codigo_horario;
            string entrada_1;
            Modelo.Horario objHorario = new Modelo.Horario();
            DateTime data;
            DataTable dt = new DataTable();

            //Cria as colunas da datatable
            foreach (DataColumn col in marcacoesDT.Columns)
            {
                dt.Columns.Add(col.ColumnName);
            }

            List<Modelo.BancoHoras> bancoHorasList = dalBancoHoras.GetAllList(false);
            List<Modelo.FechamentoBH> fechamentoBHList = dalFechamentoBH.GetAllList();
            List<Modelo.FechamentoBHD> fechamentoBHDList = dalFechamentoBHD.GetAllList();
            List<Modelo.JornadaAlternativa> jornadasList = dalJornadaAlternativa.GetPeriodo(data_inicial, data_final);

            foreach (DataRow func in marcacoesDT.Rows)
            {
                objTotalHoras = new Modelo.TotalHoras(data_inicial, data_final);
                horasExtras = new HorasExtrasStruct[4];
                codigo_horario = 0;
                id_func = (int)func["id"];
                id_empresa = (int)func["idempresa"];
                id_departamento = (int)func["iddepartamento"];
                id_funcao = (int)func["idfuncao"];
                if ((int)func["tipohorario"] == 1)
                {
                    if (func["codigohorario"] is DBNull)
                        continue;
                    else
                        codigo_horario = (int)func["codigohorario"];
                }
                if ((int)func["tipohorario"] == 2)
                {
                    if (func["codigohorario1"] is DBNull)
                        continue;
                    else
                        codigo_horario = (int)func["codigohorario1"];
                }
                entrada_1 = (string)func["entrada_1"];
                data = Convert.ToDateTime(func["data"]);

                if (func["pis"] != DBNull.Value)
                    pis_empregado = (string)func["pis"];

                dt.Rows.Clear();
                dt.Rows.Add(func.ItemArray);

                var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(id_empresa, id_departamento, id_func, id_funcao, data, data, jornadasList, dt, null, null, ConnectionString, UsuarioLogado);
                totalizadorHoras.CalcularAtraso = false;
                totalizadorHoras.TotalizeHoras(objTotalHoras);

                this.setaHorasExtras(func);
                this.setaTotalBancoDeHoras(func);
                this.grava_marcacoes(codigo_horario, data, entrada_1);
                pbExporta.Value++;
            }
        }

        /// <summary>
        /// Grava as marcações no arquivo
        /// </summary>
        //PAM - 15/12/2009
        private void grava_marcacoes(int pCodigoHorario, DateTime pData, string pEntrada1)
        {
            StringBuilder linha = new StringBuilder();
            string horasFalta = BLL.CalculoHoras.OperacaoHoras('+', objTotalHoras.horasFaltaDiurna, objTotalHoras.horasFaltaNoturna);

            sequencial_cnt++;//Incrementa o contador
            linha.Append(String.Format("{0:000000000}", sequencial_cnt)); //Sequencial do registro no arquivo
            linha.Append("3"); // Tipo do registro = 3
            linha.Append(String.Format("{0,-12}", pis_empregado));//Pis do empregado
            linha.Append(pData.ToString("ddMMyyyy"));//Data de inicio da jornada
            linha.Append(pEntrada1 != "--:--" ? pEntrada1.Replace(":", "") : "0000");//Primeiro horario de entrada da jornada
            linha.Append(String.Format("{0:0000}", pCodigoHorario));//Codigo do horario
            linha.Append(objTotalHoras.horasTrabDiurna != "--:--" ? objTotalHoras.horasTrabDiurna.Replace(":", "") : "0000");//Horas diurnas nao extraordinarias
            linha.Append(objTotalHoras.horasTrabNoturna != "--:--" ? objTotalHoras.horasTrabNoturna.Replace(":", "") : "0000");//Horas noturnas não extraordinarias
            linha.Append(horasExtras[0].horasExtras.Replace(":", ""));//Horas extras 1
            linha.Append(String.Format("{0,-4:000.0}", horasExtras[0].percentual).Replace(",", ""));//Percentual do adicional de horas extra 1 "iidd" - i = inteira - d = decimal
            linha.Append(horasExtras[0].tipo.ToString());//Modalidade da hora extra 1 => d = diurna, n = noturna
            linha.Append(horasExtras[1].horasExtras.Replace(":", ""));//Horas extras 2
            linha.Append(String.Format("{0,-4:000.0}", horasExtras[1].percentual).Replace(",", ""));//Percentual do adicional de horas extra 2 "iidd" - i = inteira - d = decimal
            linha.Append(horasExtras[1].tipo.ToString());//Modalidade da hora extra 2 => d = diurna, n = noturna
            linha.Append(horasExtras[2].horasExtras.Replace(":", ""));//Horas extras 3
            linha.Append(String.Format("{0,-4:000.0}", horasExtras[2].percentual).Replace(",", ""));//Percentual do adicional de horas extra 3 "iidd" - i = inteira - d = decimal
            linha.Append(horasExtras[2].tipo.ToString());//Modalidade da hora extra 3 => d = diurna, n = noturna
            linha.Append(horasExtras[3].horasExtras.Replace(":", ""));//Horas extras 4
            linha.Append(String.Format("{0,-4:000.0}", horasExtras[3].percentual).Replace(",", ""));//Percentual do adicional de horas extra 4 "iidd" - i = inteira - d = decimal
            linha.Append(horasExtras[3].tipo.ToString());//Modalidade da hora extra 4 => d = diurna, n = noturna
            linha.Append(horasFalta != "--:--" ? horasFalta.Replace(":", "") : "0000");//Horas de faltas e/ou atrasos => Total, diurnas + noturnas
            linha.Append(bancoDeHoras.sinal.ToString());//Sinal de horas a compensar => 1 = horas a maior, 2 = horas a menor
            linha.Append(bancoDeHoras.horas != null ? bancoDeHoras.horas.Replace(":", "").Substring(0, 4) : "0000");//Saldo de horas a compensar

            todosDados.AppendLine(linha.ToString());
        }

        /// <summary>
        /// Testa quantas horas extras tem diurnas e quantas horas tem noturnas para cada percentual escolhido.
        /// Testa primeiro o: sabado, domingo, feriado e a folga, se tem horas, se tiver coloca, se não vai para os percentuais.
        /// Se tiver mais que 4 tipos de horas extras diferentes, ignora todas a partir da quarta.
        /// </summary>
        /// <param name="pObjHorario">Objeto Horario</param>
        //PAM - 28/12/2009
        private void setaHorasExtras(DataRow row)
        {
            short hed = 0;
            try
            {
                foreach (var item in objTotalHoras.RateioHorasExtras.Keys)
                {
                    if (objTotalHoras.RateioHorasExtras[item].Diurno > 0)
                    {
                        this.horasExtras[hed].horasExtras = Modelo.cwkFuncoes.ConvertMinutosHora(objTotalHoras.RateioHorasExtras[item].Diurno);
                        this.horasExtras[hed].percentual = item;
                        this.horasExtras[hed].tipo = 'D';

                    }

                    if (objTotalHoras.RateioHorasExtras[item].Noturno > 0)
                    {
                        this.horasExtras[hed].horasExtras = Modelo.cwkFuncoes.ConvertMinutosHora(objTotalHoras.RateioHorasExtras[item].Noturno);
                        this.horasExtras[hed].percentual = item;
                        this.horasExtras[hed].tipo = 'N';

                    }

                    hed++;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            /*
            #region Extra Sabado
            //Tem hora extra diurna de Sabado
            if (objTotalHoras.extraSabadoDiurna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extraSabadoDiurna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextrasab"]);
                this.horasExtras[hed].tipo = 'D';
                hed++;
            }

            //Testa hora extra noturna de Sábado
            if (objTotalHoras.extraSabadoNoturna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extraSabadoNoturna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextrasab"]);
                this.horasExtras[hed].tipo = 'N';
                hed++;
            }
            #endregion

            #region Extra Domingo
            //Tem hora extra diurna de Domingo
            if (objTotalHoras.extraDomingoDiurna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extraDomingoDiurna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextradom"]);
                this.horasExtras[hed].tipo = 'D';
                hed++;
            }

            //Testa hora extra noturna de Domingo
            if (objTotalHoras.extraDomingoNoturna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extraDomingoNoturna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextradom"]);
                this.horasExtras[hed].tipo = 'N';
                hed++;
            }
            #endregion

            #region Extra Feriado

            if (hed == 4) return;

            //Tem hora extra diurna de Feriados
            if (objTotalHoras.extraFeriadoDiurna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extraFeriadoDiurna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextrafer"]);
                this.horasExtras[hed].tipo = 'D';
                hed++;
            }

            if (hed == 4) return;

            //Testa hora extra noturna de Feriados
            if (objTotalHoras.extraFeriadoNoturna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extraFeriadoNoturna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextrafer"]);
                this.horasExtras[hed].tipo = 'N';
                hed++;
            }
            #endregion

            #region Extra Folga

            if (hed == 4) return;

            //Tem hora extra diurna de Folga
            if (objTotalHoras.extraFolgaDiurna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extraFolgaDiurna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextrafol"]);
                this.horasExtras[hed].tipo = 'D';
                hed++;
            }

            if (hed == 4) return;

            //Testa hora extra noturna de Sábado
            if (objTotalHoras.extraFolgaNoturna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extraFolgaNoturna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextrafol"]);
                this.horasExtras[hed].tipo = 'N';
                hed++;
            }
            #endregion

            #region Extra 50%

            if (hed == 4) return;

            //Tem hora extra diurna de 50%
            if (objTotalHoras.extra50Diurna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra50Diurna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra50"]);
                this.horasExtras[hed].tipo = 'D';
                hed++;
            }

            if (hed == 4) return;

            //Testa hora extra noturna de 50%
            if (objTotalHoras.extra50Noturna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra50Noturna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra50"]);
                this.horasExtras[hed].tipo = 'N';
                hed++;
            }
            #endregion

            #region Extra 60%

            if (hed == 4) return;

            //Tem hora extra diurna de 60%
            if (objTotalHoras.extra60Diurna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra60Diurna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra60"]);
                this.horasExtras[hed].tipo = 'D';
                hed++;
            }

            if (hed == 4) return;

            //Testa hora extra noturna de 60%
            if (objTotalHoras.extra60Noturna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra60Noturna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra60"]);
                this.horasExtras[hed].tipo = 'N';
                hed++;
            }
            #endregion

            #region Extra 70%

            if (hed == 4) return;

            //Tem hora extra diurna de 70%
            if (objTotalHoras.extra70Diurna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra70Diurna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra70"]);
                this.horasExtras[hed].tipo = 'D';
                hed++;
            }

            if (hed == 4) return;

            //Testa hora extra noturna de 70%
            if (objTotalHoras.extra70Noturna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra70Noturna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra70"]);
                this.horasExtras[hed].tipo = 'N';
                hed++;
            }
            #endregion

            #region Extra 80%

            if (hed == 4) return;

            //Tem hora extra diurna de 80%
            if (objTotalHoras.extra80Diurna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra80Diurna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra80"]);
                this.horasExtras[hed].tipo = 'D';
                hed++;
            }

            if (hed == 4) return;

            //Testa hora extra noturna de 80%
            if (objTotalHoras.extra80Noturna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra80Noturna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra80"]);
                this.horasExtras[hed].tipo = 'N';
                hed++;
            }
            #endregion

            #region Extra 90%

            if (hed == 4) return;

            //Tem hora extra diurna de 90%
            if (objTotalHoras.extra90Diurna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra90Diurna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra90"]);
                this.horasExtras[hed].tipo = 'D';
                hed++;
            }

            if (hed == 4) return;

            //Testa hora extra noturna de 90%
            if (objTotalHoras.extra90Noturna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra90Noturna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra90"]);
                this.horasExtras[hed].tipo = 'N';
                hed++;
            }
            #endregion

            #region Extra 100%

            if (hed == 4) return;

            //Tem hora extra diurna de 100%
            if (objTotalHoras.extra100Diurna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra100Diurna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra100"]);
                this.horasExtras[hed].tipo = 'D';
                hed++;
            }

            if (hed == 4) return;

            //Testa hora extra noturna de 100%
            if (objTotalHoras.extra100Noturna != "00:00")
            {
                this.horasExtras[hed].horasExtras = objTotalHoras.extra100Noturna;
                this.horasExtras[hed].percentual = Convert.ToDecimal(row["percentualextra100"]);
                this.horasExtras[hed].tipo = 'N';
                hed++;
            }
            #endregion
            */

            if (hed == 4) return;
            else //Se não preencheu todos os campos, preenche com espaços vazios os que sobraram
                for (int i = hed; i < 4; i++)
                {
                    horasExtras[i].horasExtras = "0000";
                    horasExtras[i].percentual = 0;
                    horasExtras[i].tipo = ' ';
                }
        }

        /// <summary>
        /// Seta o valor do banco de horas que vai ser escrito no arquivo
        /// </summary>
        private void setaTotalBancoDeHoras(DataRow pDados)
        {
            //Sinal vai ser 1, no caso de horas positivas e 2 no caso de horas a compensar            
            string credito = (string)pDados["bancohorascre"];
            string debito = (string)pDados["bancohorasdeb"];
            bancoDeHoras.horas = null;
            bancoDeHoras.sinal = '0';

            if (credito != "---:--") //Tem credito
            {
                bancoDeHoras.horas = credito;
                bancoDeHoras.sinal = '1';
            }
            else if (debito != "---:--") // Tem debito
            {
                bancoDeHoras.horas = debito;
                bancoDeHoras.sinal = '2';
            }

        }


        private void gravaTrailer()
        {
            StringBuilder trailer = new StringBuilder();

            sequencial_cnt++;//Incrementa o contador
            trailer.Append(String.Format("{0:000000000}", sequencial_cnt)); //Sequencial do registro no arquivo
            trailer.Append("9"); // Tipo do registro = 9 (TR)

            todosDados.AppendLine(trailer.ToString());
        }
    }
}
