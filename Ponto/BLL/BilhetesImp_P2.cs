using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Collections;
using cwkPontoMT.Integracao;
using cwkPontoMT.Integracao.Entidades;

namespace BLL
{
    public partial class BilhetesImp : IBLL<Modelo.BilhetesImp>
    {
        private Modelo.ProgressBar objProgressBar;
        private static readonly DateTime DATA_VAZIA = new DateTime();

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        #region Listagens

        public List<Modelo.BilhetesImp> GetAllList()
        {
            return dalBilheteSimp.GetAllList();
        }

        public List<Modelo.BilhetesImp> GetBilhetesEspelho(DateTime dataInicial, DateTime dataFinal, string ids, int tipo)
        {
            return dalBilheteSimp.GetBilhetesEspelho(dataInicial, dataFinal, ids, tipo);
        }

        public List<Modelo.BilhetesImp> GetListaImportar(string pDsCodigo, DateTime? pDataI, DateTime? pDataF)
        {
            return dalBilheteSimp.GetListaImportar(pDsCodigo, pDataI, pDataF);
        }

        public List<Modelo.BilhetesImp> GetBilhetesFuncPeriodo(string pDsCodigo, DateTime pDataI, DateTime pDataF)
        {
            return dalBilheteSimp.GetBilhetesFuncPeriodo(pDsCodigo, pDataI, pDataF);
        }

        public List<Modelo.BilhetesImp> GetListaNaoImportadosFunc(string pDsCodigo)
        {
            return dalBilheteSimp.GetListaNaoImportadosFunc(pDsCodigo);
        }

        /// <summary>
        /// Retorna uma lista de bilhetes de um período
        /// </summary>
        /// <param name="tipo">Tipo: 0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Função, 4 - Horário, 5 - Todos, 6 - Contrato </param>
        /// <param name="idTipo"></param>
        /// <param name="dataI"></param>
        /// <param name="dataF"></param>
        /// <returns></returns>
        public List<Modelo.BilhetesImp> GetImportadosPeriodo(int tipo, int idTipo, DateTime dataI, DateTime dataF)
        {
            return dalBilheteSimp.GetImportadosPeriodo(tipo, idTipo, dataI, dataF);
        }

        public List<Modelo.BilhetesImp> LoadManutencaoBilhetes(string pDsCodigoFunc, DateTime data, bool pegaPA)
        {
            return dalBilheteSimp.LoadManutencaoBilhetes(pDsCodigoFunc, data, pegaPA);
        }

        public List<Modelo.BilhetesImp> GetImportadosPeriodo(List<int> idsFuncionarios, DateTime dataI, DateTime dataF, bool DesconsiderarFechamento)
        {
            return dalBilheteSimp.GetImportadosPeriodo(idsFuncionarios, dataI, dataF, DesconsiderarFechamento);
        }

        #endregion

        #region NovaImplementação
        /// <summary>
        /// Método responsável em retornar um DataTable com todos os bilhetes que não foram importados
        /// </summary>
        /// <param name="pDataI"></param>
        /// <param name="pDataF"></param>
        /// <param name="pDsCodigo"></param>
        /// <returns></returns>
        public DataTable getBilhetesImportar(string pDsCodigo)
        {
            return dalBilheteSimp.GetBilhetesImportar(pDsCodigo, false, null, null);
        }
        #endregion

        #region Importação de Bilhetes


        public bool ImportacaoBilhetes(List<Modelo.TipoBilhetes> plistaTipoBilhetes, string pArquivo, int pBilhete, bool pIndividual, string pFuncionario,
                                       ref DateTime? pDataI, ref DateTime? pDataF, List<string> log, string numRelogio, Modelo.Cw_Usuario usuarioLogado)
        {
            BLL.REP bllRep;
            BLL.Funcionario bllFunc;
            IList<Modelo.Funcionario> listaFuncionario = new List<Modelo.Funcionario>();

            if (usuarioLogado == null)
            {
                bllRep = new BLL.REP(ConnectionString, cwkControleUsuario.Facade.getUsuarioLogado);
                bllFunc = new BLL.Funcionario(ConnectionString, cwkControleUsuario.Facade.getUsuarioLogado);
            }
            else
            {
                bllRep = new BLL.REP(ConnectionString, usuarioLogado);
                bllFunc = new BLL.Funcionario(ConnectionString, usuarioLogado);
            }
            List<string> listaPis = new List<string>();
            if (!ValidaImportacaoBilhetes(plistaTipoBilhetes, pArquivo, pBilhete, pFuncionario, pDataI, pDataF, log, out listaPis))
            {
                return false;
            }

            ObjProgressBar.setaValorPB(10);
            ObjProgressBar.setaMensagem("Realizando leitura do arquivo de bilhetes...");
            log.Add("---------------------------------------------------------------------------------------");


            #region Atributos
            List<Modelo.Provisorio> provisorioLista = dalProvisorio.GetAllList();
            int max = this.MaxCodigo();
            int contador3 = 0;
            int contadorProcessados = 0;
            int naoPossuiFunc = 0;
            string numeroRelogio = "";
            string controleRelogio = null;
            string controleCNPJCPF = null;
            int qtdLidos = 0, qtdProcessados = 0, qtdErrados = 0, qtdRepetidos = 0, qtdSemPermissao = 0, qtdPontoFechado = 0;
            DateTime dataInicial = new DateTime(), dataFinal = new DateTime();
            #endregion

            DataTable pisFuncionarios = null;
            if (plistaTipoBilhetes.Where(t => (t.FormatoBilhete == 3 || t.FormatoBilhete == 4 || t.FormatoBilhete == 5) && t.BImporta).Count() > 0)
            {
                pisFuncionarios = dalFuncionario.GetPisCodigo(listaPis.Distinct().Select(s => s.TrimStart(new Char[] { '0' })).ToList());
            }

            //Percorre os tipos de bilhete cadastrados, importando seus arquivos
            foreach (Modelo.TipoBilhetes tp in plistaTipoBilhetes)
            {
                if (tp.BImporta == false)
                {
                    continue;
                }

                qtdLidos = 0; qtdProcessados = 0; qtdErrados = 0; qtdRepetidos = 0; qtdSemPermissao = 0; qtdPontoFechado = 0;

                if (tp.FormatoBilhete == 4)
                {
                    if (!(pDataI.HasValue && pDataF.HasValue))
                    {
                        continue;
                    }

                    Modelo.REP objRep = bllRep.LoadObject(tp.IdRep);
                    Relogio relogio = RelogioFactory.GetRelogio((TipoRelogio)objRep.Relogio);
                    relogio.SetDados(objRep.IP, objRep.Porta, objRep.Senha, (TipoComunicacao)objRep.TipoComunicacao, objRep.NumRelogio, objRep.Local);

                    if (objRep.EquipamentoHomologado.EquipamentoHomologadoInmetro)
                    {
                        if (String.IsNullOrEmpty(UsuarioLogado.Cpf) || String.IsNullOrEmpty(UsuarioLogado.LoginRep) || String.IsNullOrEmpty(UsuarioLogado.SenhaRep))
                        {
                            throw new Exception("Para comunicação com REPs Homologados pelo INMETRO é obrigatório informar o Login, Senha e CPF para comunicação com o Equipamento. "
                                + "Verifique o cadastro deste usuário (" + UsuarioLogado.Nome + ") e realize o preenchimento dos campos \"Login REP\", \"Senha REP\" e \"CPF\".");
                        }

                        relogio.UsuarioREP = UsuarioLogado.LoginRep;
                        relogio.SenhaUsuarioREP = UsuarioLogado.SenhaRep;
                        relogio.Cpf = UsuarioLogado.Cpf;
                    }


                    if (relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.TopData.InnerRepBarras2i) ||
                        relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.TopData.InnerRepPlus) ||
                        relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.Henry.PrismaSuperFacil) ||
                        relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.Dimep.DimepPrintPoint) ||
                        relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.DIXI.IDNOX) ||
                        relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.ControlID.IDXBio) ||
                        relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.ZPM.ZPM_R130) ||
                        relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.Henry.Orion6) ||
                        relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.IDData.IDRep_BI01) ||
                        relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.Henry.Hexa) ||
                        relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.Proveu.KuruminRepIIMax))
                    {
                        Modelo.Empresa emp = dalEmp.LoadObject(objRep.IdEmpresa);
                        cwkPontoMT.Integracao.Entidades.Empresa empregador = new cwkPontoMT.Integracao.Entidades.Empresa();
                        empregador.RazaoSocial = emp.Nome;
                        empregador.TipoDocumento = String.IsNullOrEmpty(emp.Cnpj) ? TipoDocumento.CPF : TipoDocumento.CNPJ;
                        empregador.Documento = String.IsNullOrEmpty(emp.Cnpj) ? emp.Cpf : emp.Cnpj;
                        empregador.CEI = emp.CEI;
                        empregador.Local = emp.Endereco;
                        relogio.SetEmpresa(empregador);
                        relogio.SetNumeroSerie(objRep.NumSerie);
                        relogio.Senha = objRep.Senha;
                    }


                    List<RegistroAFD> registros = relogio.GetAFD(pDataI.Value, pDataF.Value);
                    qtdLidos = registros.Count;
                    Modelo.BilhetesImp objBilhete;
                    List<Modelo.BilhetesImp> listaBilhetes = new List<Modelo.BilhetesImp>();
                    List<int> idsFuncs = pisFuncionarios.AsEnumerable().Select(l => l.Field<Int32>("id")).ToList();
                    listaFuncionario = bllFunc.GetAllListComDataUltimoFechamento(false, idsFuncs);
                    foreach (RegistroAFD reg in registros)
                    {
                        if (reg.Campo01 != "999999999")
                        {
                            switch (reg.Campo02)
                            {
                                case "1":
                                    //Busca o número do rep
                                    numeroRelogio = bllRep.GetNumInner(reg.Campo07);
                                    if (numeroRelogio == null || numeroRelogio == "")
                                    {
                                        controleRelogio = ("O REP " + reg.Campo07 + " não está cadastrado no sistema!");
                                        break;
                                    }
                                    if (!bllRep.GetCPFCNPJ(reg.Campo04, reg.Campo03))
                                    {
                                        controleCNPJCPF = (reg.Campo04 + " não esta cadastrado como cnpj ou cpf da empresa");
                                        numeroRelogio = String.Empty;
                                    }
                                    break;
                                case "2":
                                    break;
                                case "3":
                                    DateTime dataBil = Convert.ToDateTime(reg.Campo04.Substring(0, 2) + "/" + reg.Campo04.Substring(2, 2) + "/" + reg.Campo04.Substring(4, 4));
                                    contadorProcessados++;
                                    //Cria o bilhete
                                    objBilhete = new Modelo.BilhetesImp();
                                    objBilhete.Ordem = "000";
                                    objBilhete.Data = dataBil;
                                    objBilhete.Hora = reg.Campo05.Substring(0, 2) + ":" + reg.Campo05.Substring(2, 2);
                                    objBilhete.Nsr = reg.Nsr;
                                    //Busca o DSCodigo do funcionário pelo PIS
                                    //if (pisFuncionarios.ContainsKey(reg.campo06))
                                    //{
                                    //    objBilhete.Func = pisFuncionarios[reg.campo06].ToString();
                                    //    Modelo.Funcionario func = listaFuncionario.Where(s => s.Dscodigo.Contains(objBilhete.Func)).FirstOrDefault();
                                    //    if (pIndividual == false && usuarioLogado != null &&
                                    //        (func == null || func.Id == 0))
                                    //    {
                                    //        qtdSemPermissao++;
                                    //        continue;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (func.DataUltimoFechamento != null && func.DataUltimoFechamento >= objBilhete.Mar_data)
                                    //        {
                                    //            qtdPontoFechado++;
                                    //            continue;
                                    //        }
                                    //    }
                                    //    objBilhete.IdFuncionario = func.Id;
                                    //    objBilhete.PIS = func.Pis;
                                    //}
                                    //else
                                    //{
                                    //    naoPossuiFunc++;
                                    //    qtdErrados++;
                                    //    continue;
                                    //}

                                    if (pIndividual == true && objBilhete.Func != pFuncionario)
                                    {
                                        continue;
                                    }
                                    if (String.IsNullOrEmpty(numeroRelogio))
                                    {
                                        qtdErrados++;
                                        continue;
                                    }
                                    else
                                    {
                                        objBilhete.Relogio = numeroRelogio;
                                    }
                                    objBilhete.Importado = 0;
                                    objBilhete.Codigo = max;
                                    objBilhete.Mar_data = objBilhete.Data;
                                    objBilhete.Mar_hora = objBilhete.Hora;
                                    objBilhete.Mar_relogio = objBilhete.Relogio;
                                    objBilhete.DsCodigo = objBilhete.Func;
                                    listaBilhetes.Add(objBilhete);

                                    if (objBilhete.Data < dataInicial || dataInicial == DATA_VAZIA)
                                        dataInicial = objBilhete.Data;
                                    if (objBilhete.Data > dataFinal || dataFinal == DATA_VAZIA)
                                        dataFinal = objBilhete.Data;

                                    qtdRepetidos++;
                                    max++;
                                    contador3++;
                                    break;
                                case "4":
                                    break;
                                case "5":
                                    break;
                            }
                        }
                    }
                    ObjProgressBar.setaMensagem("Gravando bilhetes...");
                    qtdProcessados = this.Salvar(Modelo.Acao.Incluir, listaBilhetes);
                    if (qtdProcessados > 0)
                    {
                        Modelo.BilhetesImp bilhete = UltimoBilhetePorRep(numeroRelogio);
                        long ultimoNsr = bilhete.Nsr;
                        DateTime dataUltimoBilhete = Convert.ToDateTime(bilhete.Data.ToShortDateString() + " " + bilhete.Hora);
                        bllRep.SetUltimaImportacao(numeroRelogio, ultimoNsr, dataUltimoBilhete);
                    }

                    qtdRepetidos -= qtdProcessados;
                    log.Add("Leitura do TXT");
                    log.Add("Lidos         = " + String.Format("{0, 10}", contadorProcessados));
                    log.Add("Processados   = " + String.Format("{0, 10}", qtdProcessados));
                    log.Add("Errados       = " + String.Format("{0, 10}", qtdErrados));
                    log.Add("Repetidos     = " + String.Format("{0, 10}", qtdRepetidos));
                    log.Add("Sem Permissão = " + String.Format("{0, 10}", qtdSemPermissao));
                    log.Add("Ponto Fechado = " + String.Format("{0, 10}", qtdPontoFechado));
                    log.Add("");
                    if (controleCNPJCPF != null && controleCNPJCPF != "")
                    {
                        log.Add("CNPJ/CPF incorreto = " + String.Format("{0, 10}", controleCNPJCPF));
                    }
                    log.Add("");
                    log.Add("Não Foram importados " + String.Format("{0, 10}", naoPossuiFunc) + " registros porque não possui o funcionário cadastrado no banco");
                    log.Add("");
                    log.Add(controleRelogio);
                    log.Add("");
                    log.Add("---------------------------------------------------------------------------------------");
                }
                else
                {
                    ImportacaoArquivo(pArquivo, pBilhete, pIndividual, pFuncionario, pDataI, pDataF, log, max, contador3, contadorProcessados, naoPossuiFunc, numeroRelogio,
                                      controleRelogio, controleCNPJCPF, qtdLidos, ref qtdProcessados, qtdErrados, qtdRepetidos, qtdSemPermissao, ref dataInicial, ref dataFinal,
                                      tp, pisFuncionarios, usuarioLogado, qtdPontoFechado);
                }
            }

            if ((dataInicial != DATA_VAZIA && dataFinal != DATA_VAZIA && !pIndividual) || (pIndividual && (pDataI == DATA_VAZIA || pDataF == DATA_VAZIA)))
            {
                if (dataInicial == DATA_VAZIA)
                    pDataI = DateTime.Now.AddDays(-7);
                else
                    pDataI = dataInicial.AddDays(-2);
                pDataF = dataFinal;
            }
            return qtdProcessados > 0;
        }

        public bool ImportacaoBilhetesWebApi(List<Modelo.TipoBilhetes> plistaTipoBilhetes, string pArquivo, int pBilhete, bool pIndividual, string pFuncionario,
                                            ref DateTime? pDataI, ref DateTime? pDataF, List<string> log, bool bValidaArquivo, bool webApi, string numRelogio, string login,
                                            string conectionStr, out bool bErro, ref string dsCodigoFunc, bool naoValidaRep)
        {
            BLL.REP bllRep;
            bErro = false;

            bllRep = new BLL.REP(ConnectionString, !bValidaArquivo);

            ObjProgressBar.setaMensagem("Realizando leitura do arquivo de bilhetes...");
            log.Add("---------------------------------------------------------------------------------------");

            #region Atributos
            List<Modelo.Provisorio> provisorioLista = dalProvisorio.GetAllList();
            int max = this.MaxCodigo();
            int contador3 = 0;
            int contadorProcessados = 0;
            int naoPossuiFunc = 0;
            string controleRelogio = null;
            string controleCNPJCPF = null;
            int qtdLidos = 0, qtdProcessados = 0, qtdErrados = 0, qtdRepetidos = 0, qtdPontoFechado = 0;
            DateTime dataInicial = new DateTime(), dataFinal = new DateTime();
            #endregion

            DataTable pisFuncionarios = null;
            if (plistaTipoBilhetes.Where(t => (t.FormatoBilhete == 3 || t.FormatoBilhete == 4 || t.FormatoBilhete == 5) && t.BImporta).Count() > 0)
            {
                pisFuncionarios = dalFuncionario.GetPisCodigo(webApi);
            }

            //Percorre os tipos de bilhete cadastrados, importando seus arquivos
            foreach (Modelo.TipoBilhetes tp in plistaTipoBilhetes)
            {
                if (tp.BImporta == false)
                {
                    continue;
                }

                qtdLidos = 0; qtdProcessados = 0; qtdErrados = 0; qtdRepetidos = 0; qtdPontoFechado = 0;

                ImportacaoArquivoWebApi(pArquivo, pBilhete, pIndividual, pFuncionario, pDataI, pDataF, log, max, contador3, contadorProcessados, naoPossuiFunc,
                         numRelogio, controleRelogio, controleCNPJCPF, qtdLidos, ref qtdProcessados, qtdErrados, qtdRepetidos, ref dataInicial,
                         ref dataFinal, tp, pisFuncionarios, login, conectionStr, bErro, ref dsCodigoFunc, qtdPontoFechado, naoValidaRep);
            }

            if ((dataInicial != DATA_VAZIA && dataFinal != DATA_VAZIA && !pIndividual) || (pIndividual && (pDataI == DATA_VAZIA || pDataF == DATA_VAZIA)))
            {
                if (dataInicial == DATA_VAZIA)
                    pDataI = DateTime.Now.AddDays(-7);
                else
                    pDataI = dataInicial.AddDays(-2);
                pDataF = dataFinal;
            }
            return qtdProcessados > 0;
        }


        private void ImportacaoArquivo(string pArquivo, int pBilhete, bool pIndividual, string pFuncionario, DateTime? pDataI, DateTime? pDataF, List<string> log,
                                       int max, int contador3, int contadorProcessados, int naoPossuiFunc, string numeroRelogio, string controleRelogio,
                                       string controleCNPJCPF, int qtdLidos, ref int qtdProcessados, int qtdErrados, int qtdRepetidos, int qtdSemPermissao,
                                       ref DateTime dataInicial, ref DateTime dataFinal, Modelo.TipoBilhetes tp, DataTable pisFuncionarios, Modelo.Cw_Usuario usuarioLogado, int qtdPontoFechado)
        {
            BLL.REP bllRep;
            BLL.Funcionario bllFunc;
            BLL.Horario bllHorario;
            IList<Modelo.Funcionario> listaFuncionario = new List<Modelo.Funcionario>();

            if (usuarioLogado == null)
            {
                bllRep = new BLL.REP(ConnectionString, cwkControleUsuario.Facade.getUsuarioLogado);
                bllFunc = new BLL.Funcionario(ConnectionString, cwkControleUsuario.Facade.getUsuarioLogado);
                bllHorario = new BLL.Horario(ConnectionString, cwkControleUsuario.Facade.getUsuarioLogado);
            }
            else
            {
                bllRep = new BLL.REP(ConnectionString, usuarioLogado);
                bllFunc = new BLL.Funcionario(ConnectionString, usuarioLogado);
                List<int> idsFuncs = pisFuncionarios.AsEnumerable().Select(l => l.Field<Int32>("id")).ToList();
                listaFuncionario = bllFunc.GetAllListComDataUltimoFechamento(true, idsFuncs);
                bllHorario = new BLL.Horario(ConnectionString, usuarioLogado);
            }

            if (!(pBilhete == tp.Codigo))
            {
                pArquivo = tp.Diretorio;
            }

            //Verifica se o caminho especificado existe
            FileInfo FileInfo = new FileInfo(pArquivo);
            if (!FileInfo.Exists)
            {
                return;
            }
            //Verifica se o arquivo não está vazio                
            List<string> linhas = File.ReadAllLines(pArquivo).ToList();
            if (linhas.Count == 0)
            {
                return;
            }

            List<string> lPisNaoEncontrado = new List<string>();

            List<Modelo.BilhetesImp> listaBilhetes = new List<Modelo.BilhetesImp>();
            Modelo.BilhetesImp objBilhete;
            decimal tamanhoProgress = 30; // Tamanho do pedaço do progress que será destinado a validação das linhas do arquivo, considerando o progress de 0 a 100;
            decimal count = 0;
            int incrementoAnt = 0;
            int tamanhoProgressAtual = objProgressBar.valorCorrenteProgress();
            decimal total = linhas.Count();

            if ((tp.FormatoBilhete == 3) || (tp.FormatoBilhete == 5))
            {
                #region REP
                //Processar o arquivo
                log.Add("Arquivo Importado: " + Path.GetFileName(pArquivo));
                int primeiraLinhaImportar = 0;
                if (pDataI != null)
                {
                    string procurarData = pDataI.GetValueOrDefault().ToString("ddMMyyyy");
                    primeiraLinhaImportar = linhas.FindIndex(f => f.Contains(procurarData));
                }
                List<string> registrosProcessar = new List<string>();
                if (primeiraLinhaImportar > 0)
                    registrosProcessar.Add(linhas.FirstOrDefault());
                registrosProcessar.AddRange(linhas.Skip(primeiraLinhaImportar));
                total = registrosProcessar.Count();
                foreach (string linha in registrosProcessar)
                {
                    count++;
                    int incremento = (int)(tamanhoProgress * (((count * 100) / total)/ 100));
                    if (incrementoAnt != incremento || count == 1 || count == total)
                    {
                        objProgressBar.setaValorPB(tamanhoProgressAtual + incremento);
                        objProgressBar.incrementaPBCMensagem(0, String.Format("Realizando leitura da linha {0} de {1}", count, total));
                        incrementoAnt = incremento;
                    }

                    if (linha == null)
                    {
                        break;
                    }
                    if (linha == String.Empty)
                    {
                        continue;
                    }
                    qtdLidos++;

                    //Verifica se a linha é tipo 1
                    if (linha.Substring(9, 1) == "1")
                    {
                        //Busca o número do rep
                        if (linha.Substring(187, 17) != null && linha.Substring(187, 17) != "")
                        {
                            numeroRelogio = bllRep.GetNumInner(linha.Substring(187, 17));
                            if (numeroRelogio == null || numeroRelogio == "")
                            {
                                controleRelogio = ("O REP " + linha.Substring(187, 17) + " não está cadastrado no sistema!");
                                break;
                            }
                            if (!bllRep.GetCPFCNPJ(linha.Substring(11, 14), linha.Substring(10, 1)))
                            {
                                controleCNPJCPF = (linha.Substring(11, 14) + " não esta cadastrado como cnpj ou cpf da empresa");
                                numeroRelogio = "";
                            }
                        }
                        else
                        {
                            controleRelogio = ("Não existe o número do REP no arquivo txt");
                            break;
                        }
                    }
                    else if (linha.Substring(9, 1) == "3")
                    {
                        // Condição adicionada para descartar o registro que possuir na posição 9 o numero 3 (que seria registro de ponto) mas que possua 100 caracteres e as primeiros não forem numéricos, pois pode ser que seja a linha da assinatura digital e coincidentemente ela tenha na posição 9 o numero 3
                        int conv = 0;
                        if (linha.Length == 100 && !int.TryParse(linha.Substring(0, 9), out conv))
                        {
                            continue;
                        }
                        DateTime dataBil = Convert.ToDateTime(linha.Substring(10, 2) + "/" + linha.Substring(12, 2) + "/" + linha.Substring(14, 4));
                        if (pDataI.HasValue && dataBil < pDataI.Value)
                        {
                            continue;
                        }
                        if (pDataF.HasValue && dataBil > pDataF.Value)
                        {
                            continue;
                        }
                        contadorProcessados++;
                        //Cria o bilhete
                        objBilhete = new Modelo.BilhetesImp();
                        objBilhete.Nsr = Convert.ToInt32(linha.Substring(0, 9));
                        objBilhete.Ordem = "000";
                        objBilhete.Data = dataBil;
                        objBilhete.Hora = linha.Substring(18, 2) + ":" + linha.Substring(20, 2);
                        Dictionary<String, int> dsCodigos = new Dictionary<String, int>();

                        var dsCodigosLista = pisFuncionarios.AsEnumerable().Where(row => row.Field<String>("pis") == linha.Substring(22, 12) && row.Field<int>("excluido") == 0);

                        DataTable dt = dsCodigosLista.Count() > 0
                            ? dsCodigosLista.CopyToDataTable().DefaultView.ToTable(false, "dscodigo", "funcionarioativo") :
                            new DataTable();

                        try
                        {
                            dsCodigos = dt.AsEnumerable().ToDictionary(row => row.Field<String>("dscodigo"), row => row.Field<int>("funcionarioativo"));
                        }
                        catch (Exception)
                        {
                            throw new Exception("Existem funcionários cadastrados com o mesmo DsCodigo.");
                        }

                        Modelo.Funcionario func = new Modelo.Funcionario();
                        if (dsCodigos.Count() > 0)
                        {
                            objBilhete.Func = dsCodigos.OrderByDescending(o => o.Value).FirstOrDefault().Key.ToString();
                            bool erro = false;
                            func = ValidaFuncionarioPermissaoFechamento(pIndividual, ref qtdSemPermissao, usuarioLogado, ref qtdPontoFechado, listaFuncionario, objBilhete, ref erro, pFuncionario);
                            if (erro)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            naoPossuiFunc++;
                            lPisNaoEncontrado.Add(linha.Substring(22, 12));
                            continue;
                        }

                        if (String.IsNullOrEmpty(numeroRelogio))
                        {
                            qtdErrados++;
                            continue;
                        }
                        else
                        {
                            objBilhete.Relogio = numeroRelogio;
                        }
                        objBilhete.Importado = 0;
                        objBilhete.Codigo = max;
                        objBilhete.Mar_data = objBilhete.Data;
                        objBilhete.Mar_hora = objBilhete.Hora;
                        objBilhete.Mar_relogio = objBilhete.Relogio;
                        objBilhete.DsCodigo = objBilhete.Func;
                        objBilhete.IdFuncionario = func.Id;
                        objBilhete.PIS = func.Pis;
                        listaBilhetes.Add(objBilhete);

                        if (objBilhete.Data < dataInicial || dataInicial == DATA_VAZIA)
                            dataInicial = objBilhete.Data;
                        if (objBilhete.Data > dataFinal || dataFinal == DATA_VAZIA)
                            dataFinal = objBilhete.Data;

                        qtdRepetidos++;
                        max++;
                        contador3++;
                    }
                }
                qtdProcessados = SalvarBilhetesEFinalizarLog(ref log, contadorProcessados, naoPossuiFunc, numeroRelogio, controleRelogio, controleCNPJCPF, qtdErrados, ref qtdRepetidos, qtdSemPermissao, qtdPontoFechado, bllRep, lPisNaoEncontrado, listaBilhetes, linhas);
                #endregion
            }
            else
            {
                #region Outros Layouts
                log.Add("Arquivo Importado: " + Path.GetFileName(pArquivo));
                foreach (string linha in linhas)
                {
                    count++;
                    int incremento = (int)(tamanhoProgress * (((count * 100) / total) / 100));
                    if (incrementoAnt != incremento)
                    {
                        objProgressBar.setaValorPB(tamanhoProgressAtual + incremento);
                        objProgressBar.incrementaPBCMensagem(0, String.Format("Realizando leitura da linha {0} de {1}", count, total));
                        incrementoAnt = incremento;
                    }

                    if (linha == null)
                    {
                        break;
                    }
                    if (linha == String.Empty)
                    {
                        continue;
                    }
                    qtdLidos++;

                    objBilhete = new Modelo.BilhetesImp();
                    switch (tp.FormatoBilhete)
                    {
                        case 0:
                            #region TopData 5 Digitos
                            if (linha.TrimEnd().Length != 27)
                            {
                                qtdErrados++;
                                continue;
                            }
                            if (linha.Substring(6, 1) != "/" || linha.Substring(9, 1) != "/")
                            {
                                qtdErrados++;
                                continue;
                            }
                            objBilhete = new Modelo.BilhetesImp();
                            objBilhete.Ordem = linha.Substring(0, 3);
                            objBilhete.Data = Convert.ToDateTime(linha.Substring(4, 8));
                            objBilhete.Hora = linha.Substring(13, 5);
                            objBilhete.Func = linha.Substring(19, 5);
                            objBilhete.Relogio = linha.Substring(25, 2);
                            #endregion
                            break;
                        case 1:
                            #region TopData 16 Digitos
                            if (linha.TrimEnd().Length != 38)
                            {
                                qtdErrados++;
                                continue;
                            }
                            if (linha.Substring(6, 1) != "/" || linha.Substring(9, 1) != "/")
                            {
                                qtdErrados++;
                                continue;
                            }
                            objBilhete = new Modelo.BilhetesImp();
                            objBilhete.Ordem = linha.Substring(0, 3);
                            objBilhete.Data = Convert.ToDateTime(linha.Substring(4, 8));
                            objBilhete.Hora = linha.Substring(13, 5);
                            objBilhete.Func = linha.Substring(19, 16);
                            objBilhete.Relogio = linha.Substring(36, 2);
                            #endregion
                            break;
                        case 2:
                            #region Layout Livre
                            if (linha.Length == 0)
                            {
                                qtdErrados++;
                                continue;
                            }
                            objBilhete = new Modelo.BilhetesImp();
                            objBilhete.Ordem = linha.Substring(tp.Ordem_c, tp.Ordem_t);
                            if (String.IsNullOrEmpty(objBilhete.Ordem.Trim()))
                                objBilhete.Ordem = "000";
                            objBilhete.Data = Convert.ToDateTime(linha.Substring(tp.Dia_c, tp.Dia_t) + "/" + linha.Substring(tp.Mes_c, tp.Mes_t) + "/" + linha.Substring(tp.Ano_c, tp.Ano_t));
                            objBilhete.Hora = linha.Substring(tp.Hora_c, tp.Hora_t) + ":" + linha.Substring(tp.Minuto_c, tp.Minuto_t);
                            objBilhete.Func = linha.Substring(tp.Funcionario_c, tp.Funcionario_t);
                            objBilhete.Relogio = linha.Substring(tp.Relogio_c, tp.Relogio_t);

                            //Caso o layout não possua número de relógio ele é setado como "00"
                            if (String.IsNullOrEmpty(objBilhete.Relogio))
                                objBilhete.Relogio = "00";
                            #endregion
                            break;
                        default:
                            continue;
                    }

                    if (objBilhete.Data.Year <= (System.DateTime.Today.Year - 2))
                    {
                        continue;
                    }
                    objBilhete.Func = Convert.ToInt64(objBilhete.Func).ToString();
                    //Caso seja importação individual, verifica se o bilhete é do funcionário selecionado
                    if (pIndividual == true && objBilhete.Func != pFuncionario)
                    {
                        continue;
                    }
                    //Caso seja importação individual, verifica se o bilhete está dentro do período selecionado
                    if (pDataI.HasValue && objBilhete.Data < pDataI.Value)
                    {
                        continue;
                    }
                    if (pDataF.HasValue && objBilhete.Data > pDataF.Value)
                    {
                        continue;
                    }

                    objBilhete.Importado = 0;
                    objBilhete.Codigo = max;
                    objBilhete.Mar_data = objBilhete.Data;
                    objBilhete.Mar_hora = objBilhete.Hora;
                    objBilhete.Mar_relogio = objBilhete.Relogio;
                    objBilhete.DsCodigo = objBilhete.Func;
                    listaBilhetes.Add(objBilhete);

                    if (objBilhete.Data < dataInicial || dataInicial == DATA_VAZIA)
                        dataInicial = objBilhete.Data;
                    if (objBilhete.Data > dataFinal || dataFinal == DATA_VAZIA)
                        dataFinal = objBilhete.Data;

                    qtdRepetidos++;
                    max++;
                }

                qtdProcessados = SalvarBilhetesEFinalizarLog(ref log, contadorProcessados, naoPossuiFunc, numeroRelogio, controleRelogio, controleCNPJCPF, qtdErrados, ref qtdRepetidos, qtdSemPermissao, qtdPontoFechado, bllRep, lPisNaoEncontrado, listaBilhetes, linhas);
                #endregion
            }
            //LimpaArquivo(pArquivo);
        }

        private int SalvarBilhetesEFinalizarLog(ref List<string> log, int contadorProcessados, int naoPossuiFunc, string numeroRelogio, string controleRelogio, string controleCNPJCPF, int qtdErrados, ref int qtdRepetidos, int qtdSemPermissao, int qtdPontoFechado, REP bllRep, List<string> lPisNaoEncontrado, List<Modelo.BilhetesImp> listaBilhetes, List<string> linhas)
        {
            int qtdProcessados;
            objProgressBar.setaValorPB(40);
            objProgressBar.incrementaPBCMensagem(0, "Salvando bilhetes...");
            qtdProcessados = this.Salvar(Modelo.Acao.Incluir, listaBilhetes);
            if (qtdProcessados > 0)
            {
                Modelo.BilhetesImp bilhete = UltimoBilhetePorRep(numeroRelogio);
                long ultimoNsr = bilhete.Nsr;
                DateTime dataUltimoBilhete = Convert.ToDateTime(bilhete.Data.ToShortDateString() + " " + bilhete.Hora);
                bllRep.SetUltimaImportacao(numeroRelogio, ultimoNsr, dataUltimoBilhete);
            }
            objProgressBar.incrementaPBCMensagem(20, "Bilhetes Salvos (" + qtdProcessados + "), gerando arquivo de retorno");
            qtdRepetidos -= qtdProcessados;
            log.Add("Leitura do TXT");
            log.Add("Total de Linhas = " + String.Format("{0, 10}", linhas.Count));
            log.Add("Descartados     = " + String.Format("{0, 10}", linhas.Count - contadorProcessados));
            log.Add("Lidos           = " + String.Format("{0, 10}", contadorProcessados));
            log.Add("Processados     = " + String.Format("{0, 10}", qtdProcessados));
            log.Add("Não Encontrado  = " + String.Format("{0, 10}", naoPossuiFunc));
            log.Add("Errados         = " + String.Format("{0, 10}", qtdErrados));
            log.Add("Repetidos       = " + String.Format("{0, 10}", qtdRepetidos));
            log.Add("Sem Permissão   = " + String.Format("{0, 10}", qtdSemPermissao));
            log.Add("Ponto Fechado   = " + String.Format("{0, 10}", qtdPontoFechado));
            log.Add("");
            if (controleCNPJCPF != null && controleCNPJCPF != "")
            {
                log.Add("CNPJ/CPF incorreto = " + String.Format("{0, 10}", controleCNPJCPF));
                log.Add("");
            }

            if (lPisNaoEncontrado.Count > 0)
            {
                log.Add("Não Foram importados " + String.Format("{0, 10}", naoPossuiFunc) + " registros, pois os funcionários não foram cadastrados ou estão inativos ou excluídos. PIS não encontrado:");
                log.Add("PIS".PadRight(15, ' ') + " | " + "Quantidade".ToString().PadLeft(10, ' '));
                foreach (var item in lPisNaoEncontrado.GroupBy(g => g))
                {
                    log.Add(item.Key.PadRight(15, ' ') + " | " + item.ToList().Count().ToString().PadLeft(10, ' '));
                }
                log.Add("");
            }

            if (!string.IsNullOrEmpty(controleRelogio))
            {
                log.Add(controleRelogio);
                log.Add("");
            }
            log.Add("---------------------------------------------------------------------------------------");
            return qtdProcessados;
        }

        private static Modelo.Funcionario ValidaFuncionarioPermissaoFechamento(bool pIndividual, ref int qtdSemPermissao, Modelo.Cw_Usuario usuarioLogado, ref int qtdPontoFechado, IList<Modelo.Funcionario> listaFuncionario, Modelo.BilhetesImp objBilhete, ref bool erro, string pFuncionario)
        {
            if (pIndividual == true && objBilhete.Func != pFuncionario)
            {
                erro = true;
            }
            Modelo.Funcionario func = listaFuncionario.Where(s => s.Dscodigo == objBilhete.Func).FirstOrDefault();
            if (pIndividual == false && usuarioLogado != null &&
                    (func == null || func.Id == 0))
            {
                qtdSemPermissao++;
                erro = true;
            }
            else
            {
                if (func.DataUltimoFechamento != null && func.DataUltimoFechamento >= objBilhete.Data)
                {
                    qtdPontoFechado++;
                    erro = true;
                }
            }
            return func;
        }

        private void ImportacaoArquivoWebApi(string pArquivo, int pBilhete, bool pIndividual, string pFuncionario, DateTime? pDataI, DateTime? pDataF, List<string> log,
                               int max, int contador3, int contadorProcessados, int naoPossuiFunc, string numeroRelogio, string controleRelogio,
                               string controleCNPJCPF, int qtdLidos, ref int qtdProcessados, int qtdErrados, int qtdRepetidos, ref DateTime dataInicial,
                               ref DateTime dataFinal, Modelo.TipoBilhetes tp, DataTable pisFuncionarios, string login, string conectionStr, bool bErro, ref string dsCodigoFunc, int qtdPontoFechado, bool naoValidaRep)
        {
            BLL.REP bllRep = new BLL.REP(ConnectionString, new Modelo.Cw_Usuario() { Login = "cwork", Nome = "cwork" });
            if (!(pBilhete == tp.Codigo))
            {
                pArquivo = tp.Diretorio;
            }

            BLL.Funcionario bllFunc = new BLL.Funcionario(ConnectionString, new Modelo.Cw_Usuario() { Login = "cwork", Nome = "cwork" });
            IList<Modelo.Funcionario> listaFuncionario = new List<Modelo.Funcionario>();
            listaFuncionario = bllFunc.GetAllListComUltimosFechamentos(false);
            string linha = "";
            //Verifica se o caminho especificado existe
            FileInfo FileInfo = new FileInfo(pArquivo);
            if (!FileInfo.Exists)
            {
                return;
            }
            //Verifica se o arquivo não está vazio                
            int tam = File.ReadAllLines(pArquivo).GetLength(0);
            if (tam == 0)
            {
                return;
            }

            using (FileStream stream = new FileStream(pArquivo, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                StreamReader objReader = new StreamReader(stream);

                //Atualiza as informações na barra de progresso
                ObjProgressBar.setaMinMaxPB(0, tam);
                ObjProgressBar.setaValorPB(0);

                List<Modelo.BilhetesImp> listaBilhetes = new List<Modelo.BilhetesImp>();

                Modelo.BilhetesImp objBilhete;
                if ((tp.FormatoBilhete == 3) || (tp.FormatoBilhete == 5))
                {
                    #region REP
                    //Processar o arquivo
                    log.Add("Arquivo Importado: " + Path.GetFileName(pArquivo));
                    while (linha != null)
                    {
                        linha = objReader.ReadLine();
                        if (linha == null)
                        {
                            break;
                        }
                        if (linha == String.Empty)
                        {
                            continue;
                        }
                        qtdLidos++;

                        //Verifica se a linha é tipo 1
                        if (linha.Substring(9, 1) == "1")
                        {

                            if (!naoValidaRep)
                            {
                                //Busca o número do rep
                                if (linha.Substring(187, 17) != null && linha.Substring(187, 17) != "")
                                {
                                    numeroRelogio = bllRep.GetNumInner(linha.Substring(187, 17));
                                    if (numeroRelogio == null || numeroRelogio == "")
                                    {
                                        controleRelogio = ("O REP " + linha.Substring(187, 17) + " não está cadastrado no sistema!");
                                        break;
                                    }

                                }
                                else
                                {
                                    controleRelogio = ("Não existe o número do REP no arquivo txt");
                                    break;
                                }
                            }

                            if (!bllRep.GetCPFCNPJ(linha.Substring(11, 14), linha.Substring(10, 1)))
                            {
                                controleCNPJCPF = (linha.Substring(11, 14) + " não esta cadastrado como cnpj ou cpf da empresa");
                                numeroRelogio = "";
                            }
                        }
                        else if (linha.Substring(9, 1) == "3")
                        {
                            DateTime dataBil = Convert.ToDateTime(linha.Substring(10, 2) + "/" + linha.Substring(12, 2) + "/" + linha.Substring(14, 4));
                            if (pDataI.HasValue && dataBil < pDataI.Value)
                            {
                                continue;
                            }
                            if (pDataF.HasValue && dataBil > pDataF.Value)
                            {
                                continue;
                            }
                            contadorProcessados++;
                            //Cria o bilhete
                            objBilhete = new Modelo.BilhetesImp();
                            objBilhete.Nsr = Convert.ToInt32(linha.Substring(0, 9));
                            objBilhete.Ordem = "000";
                            objBilhete.Data = dataBil;
                            objBilhete.Hora = linha.Substring(18, 2) + ":" + linha.Substring(20, 2);
                            bool encontrouFunc = false;
                            //Busca o DSCodigo do funcionário pelo PIS
                            Dictionary<String, int> dsCodigos = pisFuncionarios
                                                            .AsEnumerable()
                                                            .Where(row => row.Field<String>("pis") == linha.Substring(22, 12) && row.Field<int>("excluido") == 0)
                                                            .ToDictionary(row => row.Field<String>("dscodigo"), row => row.Field<int>("funcionarioativo"));
                            encontrouFunc = dsCodigos.Count() > 0;
                            Modelo.Funcionario func = new Modelo.Funcionario();
                            if (encontrouFunc)
                            {
                                objBilhete.Func = dsCodigos.OrderByDescending(o => o.Value).FirstOrDefault().Key.ToString();
                                func = listaFuncionario.Where(s => s.Dscodigo == objBilhete.Func).FirstOrDefault();
                                if (func != null)
                                {
                                    if ((func.DataUltimoFechamento != null && func.DataUltimoFechamento >= objBilhete.Data) || func.DataUltimoFechamentoBH.GetValueOrDefault() >= objBilhete.Data)
                                    {
                                        qtdPontoFechado++;
                                        continue;
                                    }
                                }
                                else
                                {
                                    encontrouFunc = false;
                                }

                            }

                            if (!encontrouFunc)
                            {
                                naoPossuiFunc++;
                                qtdErrados++;
                                continue;
                            }

                            if (pIndividual == true && objBilhete.Func != pFuncionario)
                            {
                                continue;
                            }
                            if (String.IsNullOrEmpty(numeroRelogio))
                            {
                                qtdErrados++;
                                continue;
                            }
                            else
                            {
                                objBilhete.Relogio = numeroRelogio;
                            }
                            objBilhete.Importado = 0;
                            objBilhete.Codigo = max;
                            objBilhete.Mar_data = objBilhete.Data;
                            objBilhete.Mar_hora = objBilhete.Hora;
                            objBilhete.Mar_relogio = objBilhete.Relogio;
                            objBilhete.DsCodigo = objBilhete.Func;
                            objBilhete.IdFuncionario = func.Id;
                            objBilhete.PIS = func.Pis;
                            listaBilhetes.Add(objBilhete);
                            if (objBilhete.Data < dataInicial || dataInicial == DATA_VAZIA)
                                dataInicial = objBilhete.Data;
                            if (objBilhete.Data > dataFinal || dataFinal == DATA_VAZIA)
                                dataFinal = objBilhete.Data;

                            qtdRepetidos++;
                            max++;
                            contador3++;
                        }
                    }

                    objReader.Close();
                    ObjProgressBar.setaMensagem("Gravando bilhetes...");
                    qtdProcessados = this.Salvar(Modelo.Acao.Incluir, listaBilhetes, login, conectionStr);
                    if (qtdProcessados > 0)
                    {
                        Modelo.BilhetesImp bilhete = UltimoBilhetePorRep(numeroRelogio);
                        long ultimoNsr = bilhete.Nsr;
                        DateTime dataUltimoBilhete = Convert.ToDateTime(bilhete.Data.ToShortDateString() + " " + bilhete.Hora);
                        bllRep.SetUltimaImportacao(numeroRelogio, ultimoNsr, dataUltimoBilhete);
                    }

                    dsCodigoFunc = String.Join(",", listaBilhetes.Select(x => "'" + x.DsCodigo + "'").Distinct());
                    qtdRepetidos -= qtdProcessados;
                    log.Add("Leitura do TXT");
                    log.Add("Lidos       = " + String.Format("{0, 10}", contadorProcessados));
                    log.Add("Processados = " + String.Format("{0, 10}", qtdProcessados));
                    log.Add("Errados     = " + String.Format("{0, 10}", qtdErrados));
                    log.Add("Repetidos   = " + String.Format("{0, 10}", qtdRepetidos));
                    log.Add("Ponto Fechado = " + String.Format("{0, 10}", qtdPontoFechado));
                    log.Add("");

                    bErro = VerificaErroImportacao(qtdErrados, qtdRepetidos, bErro);
                    if (controleCNPJCPF != null && controleCNPJCPF != "")
                    {
                        log.Add("CNPJ/CPF incorreto = " + String.Format("{0, 10}", controleCNPJCPF));
                        bErro = true;
                    }

                    log.Add("");
                    log.Add("Não Foram importados " + String.Format("{0, 10}", naoPossuiFunc) + " registros porque não possui o funcionário cadastrado no banco");
                    log.Add("");
                    log.Add(controleRelogio);
                    log.Add("");
                    log.Add("---------------------------------------------------------------------------------------");
                    #endregion
                }
                else
                {
                    #region Outros Layouts
                    log.Add("Arquivo Importado: " + Path.GetFileName(pArquivo));
                    while (linha != null)
                    {
                        linha = objReader.ReadLine();
                        if (linha == null)
                        {
                            break;
                        }
                        if (linha == String.Empty)
                        {
                            continue;
                        }
                        qtdLidos++;

                        objBilhete = new Modelo.BilhetesImp();
                        switch (tp.FormatoBilhete)
                        {
                            case 0:
                                #region TopData 5 Digitos
                                if (linha.TrimEnd().Length != 27)
                                {
                                    qtdErrados++;
                                    continue;
                                }
                                if (linha.Substring(6, 1) != "/" || linha.Substring(9, 1) != "/")
                                {
                                    qtdErrados++;
                                    continue;
                                }
                                objBilhete = new Modelo.BilhetesImp();
                                objBilhete.Ordem = linha.Substring(0, 3);
                                objBilhete.Data = Convert.ToDateTime(linha.Substring(4, 8));
                                objBilhete.Hora = linha.Substring(13, 5);
                                objBilhete.Func = linha.Substring(19, 5);
                                objBilhete.Relogio = linha.Substring(25, 2);
                                #endregion
                                break;
                            case 1:
                                #region TopData 16 Digitos
                                if (linha.TrimEnd().Length != 38)
                                {
                                    qtdErrados++;
                                    continue;
                                }
                                if (linha.Substring(6, 1) != "/" || linha.Substring(9, 1) != "/")
                                {
                                    qtdErrados++;
                                    continue;
                                }
                                objBilhete = new Modelo.BilhetesImp();
                                objBilhete.Ordem = linha.Substring(0, 3);
                                objBilhete.Data = Convert.ToDateTime(linha.Substring(4, 8));
                                objBilhete.Hora = linha.Substring(13, 5);
                                objBilhete.Func = linha.Substring(19, 16);
                                objBilhete.Relogio = linha.Substring(36, 2);
                                #endregion
                                break;
                            case 2:
                                #region Layout Livre
                                if (linha.Length == 0)
                                {
                                    qtdErrados++;
                                    continue;
                                }
                                objBilhete = new Modelo.BilhetesImp();
                                objBilhete.Ordem = linha.Substring(tp.Ordem_c, tp.Ordem_t);
                                if (String.IsNullOrEmpty(objBilhete.Ordem.Trim()))
                                    objBilhete.Ordem = "000";
                                objBilhete.Data = Convert.ToDateTime(linha.Substring(tp.Dia_c, tp.Dia_t) + "/" + linha.Substring(tp.Mes_c, tp.Mes_t) + "/" + linha.Substring(tp.Ano_c, tp.Ano_t));
                                objBilhete.Hora = linha.Substring(tp.Hora_c, tp.Hora_t) + ":" + linha.Substring(tp.Minuto_c, tp.Minuto_t);
                                objBilhete.Func = linha.Substring(tp.Funcionario_c, tp.Funcionario_t);
                                objBilhete.Relogio = linha.Substring(tp.Relogio_c, tp.Relogio_t);

                                //Caso o layout não possua número de relógio ele é setado como "00"
                                if (String.IsNullOrEmpty(objBilhete.Relogio))
                                    objBilhete.Relogio = "00";
                                #endregion
                                break;
                            default:
                                continue;
                        }

                        if (objBilhete.Data.Year <= (System.DateTime.Today.Year - 2))
                        {
                            continue;
                        }
                        objBilhete.Func = Convert.ToInt64(objBilhete.Func).ToString();
                        //Caso seja importação individual, verifica se o bilhete é do funcionário selecionado
                        if (pIndividual == true && objBilhete.Func != pFuncionario)
                        {
                            continue;
                        }
                        Modelo.Funcionario func = listaFuncionario.Where(s => s.Dscodigo == objBilhete.Func).FirstOrDefault();
                        if (func.DataUltimoFechamento != null && func.DataUltimoFechamento >= objBilhete.Data)
                        {
                            qtdPontoFechado++;
                            continue;
                        }
                        //Caso seja importação individual, verifica se o bilhete está dentro do período selecionado
                        if (pDataI.HasValue && objBilhete.Data < pDataI.Value)
                        {
                            continue;
                        }
                        if (pDataF.HasValue && objBilhete.Data > pDataF.Value)
                        {
                            continue;
                        }
                        objBilhete.Importado = 0;
                        objBilhete.Codigo = max;
                        objBilhete.Mar_data = objBilhete.Data;
                        objBilhete.Mar_hora = objBilhete.Hora;
                        objBilhete.Mar_relogio = objBilhete.Relogio;
                        objBilhete.DsCodigo = objBilhete.Func;
                        listaBilhetes.Add(objBilhete);

                        if (objBilhete.Data < dataInicial || dataInicial == DATA_VAZIA)
                            dataInicial = objBilhete.Data;
                        if (objBilhete.Data > dataFinal || dataFinal == DATA_VAZIA)
                            dataFinal = objBilhete.Data;

                        qtdRepetidos++;
                        max++;
                    }
                    objReader.Close();
                    ObjProgressBar.setaMensagem("Gravando bilhetes...");
                    qtdProcessados = this.Salvar(Modelo.Acao.Incluir, listaBilhetes);
                    if (qtdProcessados > 0)
                    {
                        Modelo.BilhetesImp bilhete = UltimoBilhetePorRep(numeroRelogio);
                        long ultimoNsr = bilhete.Nsr;
                        DateTime dataUltimoBilhete = Convert.ToDateTime(bilhete.Data.ToShortDateString() + " " + bilhete.Hora);
                        bllRep.SetUltimaImportacao(numeroRelogio, ultimoNsr, dataUltimoBilhete);
                    }
                    qtdRepetidos -= qtdProcessados;

                    log.Add("Leitura do TXT");
                    log.Add("Lidos       = " + String.Format("{0, 10}", qtdLidos));
                    log.Add("Processados = " + String.Format("{0, 10}", qtdProcessados));
                    log.Add("Errados     = " + String.Format("{0, 10}", qtdErrados));
                    log.Add("Repetidos   = " + String.Format("{0, 10}", qtdRepetidos));
                    log.Add("Ponto Fechado = " + String.Format("{0, 10}", qtdPontoFechado));
                    log.Add("---------------------------------------------------------------------------------------");

                    bErro = VerificaErroImportacao(qtdErrados, qtdRepetidos, bErro);

                    #endregion
                }

            }
        }

        private static bool VerificaErroImportacao(int qtdErrados, int qtdRepetidos, bool bErro)
        {

            if (qtdErrados > 0)
                bErro = true;
            else
                bErro = false;

            return bErro;
        }

        private static void LimpaArquivo(string pArquivo)
        {
            StreamWriter limpar = new StreamWriter(pArquivo, false);
            limpar.Flush();
            limpar.Close();
        }

        private static void CopiaArquivoBilhete(string pArquivo)
        {
            string[] nome = pArquivo.Split(new char[] { '.' });
            string novo = "";
            for (int i = 0; i < nome.Count() - 1; i++)
            {
                if (i > 0)
                {
                    novo += ".";
                }
                novo += nome[i];
            }
            novo += "_" + DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".";
            novo += nome[nome.Count() - 1];
            File.Copy(pArquivo, novo, true);
        }

        public bool ValidaImportacaoBilhetes(List<Modelo.TipoBilhetes> plistaTipoBilhetes, string pArquivo, int pBilhete, string pFuncionario, DateTime? pDataI, DateTime? pDataF, List<string> log, out List<string> listaPis)
        {
            objProgressBar.setaValorPB(5);
            ObjProgressBar.setaMensagem("Validando arquivo de bilhetes...");

            log.Add("---------------------------------------------------------------------------------------");
            int qtdLidos = 0, qtdErrados = 0;
            bool arquivoOk = true;

            Hashtable validar = new Hashtable();
            validar.Add('#', '#');
            validar.Add('.', '.');
            validar.Add('(', '(');
            validar.Add(')', ')');
            validar.Add('*', '*');
            validar.Add('+', '+');
            validar.Add('=', '=');
            validar.Add('_', '_');
            validar.Add('-', '-');
            validar.Add('!', '!');
            validar.Add('@', '@');
            validar.Add('$', '$');
            validar.Add('%', '%');
            DateTime data;
            listaPis = new List<string>();
            foreach (Modelo.TipoBilhetes tp in plistaTipoBilhetes)
            {
                if (tp.BImporta == false)
                {
                    continue;
                }

                if (tp.FormatoBilhete == 4)
                {
                    if (!(pDataI.HasValue && pDataF.HasValue))
                    {
                        log.Add("Para efetuar uma importação direta do relógio é necessário informar o período de importação.");
                    }
                    continue;
                }

                if (!(pBilhete == tp.Codigo))
                {
                    pArquivo = tp.Diretorio;
                }

                string linha = "";
                //Verifica se o caminho especificado existe
                FileInfo FileInfo = new FileInfo(pArquivo);
                if (!FileInfo.Exists)
                {
                    log.Add("Arquivo para Validação não existe: " + Path.GetFileName(pArquivo));
                    continue;
                }

                bool bAfdInmetro = VerificaTipoAfdInmetro(pArquivo, linha);

                switch (tp.FormatoBilhete)
                {
                    case 3:
                        if (bAfdInmetro)
                            tp.FormatoBilhete = 5;
                        break;
                    case 5:
                        if (!bAfdInmetro)
                            tp.FormatoBilhete = 3;
                        break;
                }

                StreamReader objReader = new StreamReader(pArquivo);

                if (File.ReadAllBytes(pArquivo).Length == 0)
                {
                    log.Add("Arquivo para Validação em Branco: " + Path.GetFileName(pArquivo));
                    continue;
                }

                switch (tp.FormatoBilhete)
                {
                    case 3:
                        log.Add("Arquivo para Validação: " + Path.GetFileName(pArquivo));
                        qtdLidos = 0; qtdErrados = 0;
                        int qtdCabecalho = 0;
                        while (linha != null)
                        {
                            linha = objReader.ReadLine();
                            if (linha == null || linha == "")
                            {
                                break;
                            }
                            qtdLidos++;

                            switch (linha.Substring(9, 1))
                            {
                                case "1":
                                    if (qtdCabecalho > 0)
                                    {
                                        log.Add(String.Format("Linha {0}: Layout diferente de REP (AFD), contém mais de um cabeçalho", qtdLidos));
                                        arquivoOk = false;
                                        qtdErrados++;
                                    }
                                    else
                                    {

                                    }
                                    qtdCabecalho++;
                                    break;
                                case "3":
                                    //faz validacao do tipo 3
                                    if (qtdCabecalho == 0)
                                    {
                                        log.Add(String.Format("Linha {0}: Layout diferente de REP (AFD), não contém linha header", qtdLidos));
                                        arquivoOk = false;
                                        qtdErrados++;
                                    }
                                    else
                                    {
                                        if (linha.Length != 34)
                                        {
                                            log.Add(String.Format("Linha {0}: Layout diferente de REP (AFD)", qtdLidos));
                                            arquivoOk = false;
                                            qtdErrados++;
                                        }
                                        else
                                        {
                                            foreach (char c in linha.Substring(10, 24))
                                            {
                                                if (!Char.IsNumber(c))
                                                {
                                                    log.Add(String.Format("Linha {0}: Layout diferente de REP (AFD)", qtdLidos));
                                                    arquivoOk = false;
                                                    qtdErrados++;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    listaPis.Add(linha.Substring(22, 12));
                                    break;
                            }
                        }
                        break;
                    case 5:
                        log.Add("Arquivo para Validação: " + Path.GetFileName(pArquivo));
                        qtdLidos = 0; qtdErrados = 0;
                        while (linha != null)
                        {
                            linha = objReader.ReadLine();
                            if (linha == null || linha == "")
                            {
                                break;
                            }
                            qtdLidos++;

                            switch (linha.Substring(9, 1))
                            {
                                case "3":
                                    //faz validacao do tipo 3
                                    if (linha.Length != 38)
                                    {
                                        // Condição adicionada para descartar o registro que possuir na posição 9 o numero 3 (que seria registro de ponto) mas que possua 100 caracteres e as primeiros não forem numéricos, pois pode ser que seja a linha da assinatura digital e coincidentemente ela tenha na posição 9 o numero 3
                                        int conv = 0;
                                        if (linha.Length == 100 && !int.TryParse(linha.Substring(0, 9), out conv))
                                        {
                                            continue;
                                        }
                                        log.Add(String.Format("Linha {0}: Layout diferente de REP (AFD)", qtdLidos));
                                        arquivoOk = false;
                                        qtdErrados++;
                                    }
                                    else
                                    {
                                        foreach (char c in linha.Substring(10, 24))
                                        {
                                            if (!Char.IsNumber(c))
                                            {
                                                log.Add(String.Format("Linha {0}: Layout diferente de REP (AFD)", qtdLidos));
                                                arquivoOk = false;
                                                qtdErrados++;
                                                break;
                                            }
                                        }
                                    }
                                    listaPis.Add(linha.Substring(22, 12));
                                    break;
                            }
                        }
                        break;
                    default:
                        log.Add("Arquivo para Validação: " + Path.GetFileName(pArquivo));
                        qtdLidos = 0; qtdErrados = 0;
                        while (linha != null)
                        {
                            linha = objReader.ReadLine();
                            if (linha == null)
                            {
                                break;
                            }
                            qtdLidos++;

                            if (linha.TrimEnd().Length == 0)
                                continue;

                            switch (tp.FormatoBilhete)
                            {
                                case 0: // TopData 5 Digitos
                                    if (linha.TrimEnd().Length != 27)
                                    {
                                        log.Add(String.Format("Linha {0}: Layout diferente de TopData 5 Digitos", qtdLidos));
                                        arquivoOk = false;
                                        qtdErrados++;
                                    }
                                    if (linha.Substring(6, 1) != "/" || linha.Substring(9, 1) != "/")
                                    {
                                        log.Add(String.Format("Linha {0}: Layout diferente de TopData 5 Digitos", qtdLidos));
                                        arquivoOk = false;
                                        qtdErrados++;
                                    }
                                    break;
                                case 1: // TopData 16 Digitos
                                    if (linha.TrimEnd().Length != 38)
                                    {
                                        log.Add(String.Format("Linha {0}: Layout diferente de TopData 16 Digitos", qtdLidos));
                                        arquivoOk = false;
                                        qtdErrados++;
                                    }
                                    if (linha.Substring(6, 1) != "/" || linha.Substring(9, 1) != "/")
                                    {
                                        log.Add(String.Format("Linha {0}: Layout diferente de TopData 16 Digitos", qtdLidos));
                                        arquivoOk = false;
                                        qtdErrados++;
                                    }
                                    break;
                                default:
                                    if (linha.Length != tp.StrLayout.Length)
                                    {
                                        log.Add(String.Format("Linha {0}: Layout diferente do Layout Livre definido", qtdLidos));
                                        arquivoOk = false;
                                        qtdErrados++;
                                    }
                                    else
                                    {
                                        if (String.IsNullOrEmpty(linha.Substring(tp.Funcionario_c, tp.Funcionario_t)) || linha.Substring(tp.Funcionario_c, tp.Funcionario_t).Trim().Length != tp.Funcionario_t)
                                        {
                                            log.Add(String.Format("Linha {0}: O código do funcionário está com o valor incorreto", qtdLidos));
                                            arquivoOk = false;
                                            qtdErrados++;
                                        }
                                        try
                                        {
                                            data = Convert.ToDateTime(linha.Substring(tp.Dia_c, tp.Dia_t) + "/" + linha.Substring(tp.Mes_c, tp.Mes_t) + "/" + linha.Substring(tp.Ano_c, tp.Ano_t));

                                            if (data == null || data == new DateTime())
                                            {
                                                log.Add(String.Format("Linha {0}: A data do bilhete está com o valor incorreto", qtdLidos));
                                                arquivoOk = false;
                                                qtdErrados++;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            log.Add(String.Format("Linha {0}: A data do bilhete está com o valor incorreto", qtdLidos));
                                            arquivoOk = false;
                                            qtdErrados++;
                                        }

                                        if (String.IsNullOrEmpty(linha.Substring(tp.Hora_c, tp.Hora_t)) || String.IsNullOrEmpty(linha.Substring(tp.Minuto_c, tp.Minuto_t))
                                            || (linha.Substring(tp.Hora_c, tp.Hora_t).TrimEnd() + ":" + linha.Substring(tp.Minuto_c, tp.Minuto_t).TrimEnd()).Length != 5)
                                        {
                                            log.Add(String.Format("Linha {0}: A hora do bilhete está com o valor incorreto", qtdLidos));
                                            arquivoOk = false;
                                            qtdErrados++;
                                        }
                                    }
                                    break;
                            }

                            for (int j = 0; j < linha.Length; j++)
                            {
                                if (validar.ContainsKey(linha[j]))
                                {
                                    log.Add(String.Format("Linha {0}: [{1}] Caracter não permitido ", qtdLidos, linha[j]));
                                    break;
                                }
                            }
                        }
                        break;
                }

                objReader.Close();

                if (qtdErrados > 0)
                {
                    log.Add("---------------------------------------------------------------------------------------");
                }
                else
                {
                    log.Add("Arquivo validado com sucesso.");
                    log.Add("---------------------------------------------------------------------------------------");
                }
            }

            return arquivoOk;
        }

        private static bool VerificaTipoAfdInmetro(string pArquivo, string linha)
        {
            bool bArqTipo = false;
            String primLinha = String.Empty;

            using (StreamReader reader = new StreamReader(pArquivo))
            {
                primLinha = reader.ReadLine();
                if (!String.IsNullOrEmpty(primLinha))
                {
                    string tipo = primLinha.Substring(9, 1);

                    switch (tipo)
                    {
                        case "1":
                            bArqTipo = primLinha.Trim().Length == 236;
                            break;
                        case "2":
                            bArqTipo = primLinha.Trim().Length == 317;
                            break;
                        case "3":
                            bArqTipo = primLinha.Trim().Length == 38;
                            break;
                        case "4":
                            bArqTipo = primLinha.Trim().Length == 49;
                            break;
                        case "5":
                            bArqTipo = primLinha.Trim().Length == 106;
                            break;
                        case "6":
                            bArqTipo = primLinha.Trim().Length == 24;
                            break;
                    }
                }
            }

            return bArqTipo;
        }

        #endregion

        #region Manutenção de Bilhetes

        /// <summary>
        /// Método responsável pela manutenção do bilhete
        /// </summary>
        /// <param name="pMarcacao">Marcação do bilhete</param>
        /// <param name="pBilhete">Bilhete alterado</param>
        /// <param name="pMudancao">Tipo da Mudança: 0 - Dia Anterior, 1 - Mesmo Dia, 2 - Dia Posterior</param>
        /// WNO - 29/12/2009
        public bool ManutencaoBilhete(Modelo.Marcacao pMarcacao, Modelo.BilhetesImp pBilhete, int pMudancao)
        {
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            BLL.ImportaBilhetes bllImportaBilhetes = new ImportaBilhetes(ConnectionString, UsuarioLogado);
            if (ObjProgressBar.incrementaPB == null)
            {
                ObjProgressBar = progressBarVazia;
            }
            ObjProgressBar.setaMinMaxPB(0, 100);
            ObjProgressBar.setaValorPB(0);
            ObjProgressBar.setaMensagem("Carregando dados...");
            int diasAtras = 1, diasFrente = 1;
            switch (pMudancao)
            {
                case 0:
                    if (DateTime.Compare(pBilhete.Mar_data.Value, pBilhete.Data) > 0)
                    {
                        diasAtras = 2;
                    }
                    pBilhete.Mar_data = pBilhete.Data.AddDays(-1);
                    break;
                case 1:
                    pBilhete.Mar_data = pBilhete.Data;
                    break;
                case 2:
                    if (DateTime.Compare(pBilhete.Mar_data.Value, pBilhete.Data) < 0)
                    {
                        diasFrente = 2;
                    }
                    pBilhete.Mar_data = pBilhete.Data.AddDays(1);
                    break;
            }

            this.Salvar(Modelo.Acao.Alterar, pBilhete);

            RecalculaAlteracaoBilhete(pMarcacao, bllMarcacao, bllImportaBilhetes, diasAtras, diasFrente);

            return true;
        }

        public void RecalculaAlteracaoBilhete(Modelo.Marcacao pMarcacao, BLL.Marcacao bllMarcacao, BLL.ImportaBilhetes bllImportaBilhetes, int diasAtras, int diasFrente)
        {
            Modelo.Funcionario objFuncionario = dalFuncionario.LoadObject(pMarcacao.Idfuncionario);
            DateTime dataI = pMarcacao.Data;
            DateTime dataF = pMarcacao.Data;
            if (diasAtras != 0)
                dataI = pMarcacao.Data.AddDays(-diasAtras);

            if (diasFrente != 0)
                dataF = pMarcacao.Data.AddDays(diasFrente);

            List<Modelo.Marcacao> listaMarcacoes = bllMarcacao.GetPorFuncionario(pMarcacao.Idfuncionario, dataI, dataF, true);

            ObjProgressBar.incrementaPB(25);
            ObjProgressBar.setaMensagem("Alterando bilhetes...");
            List<string> log = new List<string>();

            //Prepara e excluir uma lista de marcação
            foreach (Modelo.Marcacao marc in listaMarcacoes)
            {
                marc.Ent_num_relogio_1 = "";
                marc.Ent_num_relogio_2 = "";
                marc.Ent_num_relogio_3 = "";
                marc.Ent_num_relogio_4 = "";
                marc.Ent_num_relogio_5 = "";
                marc.Ent_num_relogio_6 = "";
                marc.Ent_num_relogio_7 = "";
                marc.Ent_num_relogio_8 = "";
                marc.Sai_num_relogio_1 = "";
                marc.Sai_num_relogio_2 = "";
                marc.Sai_num_relogio_3 = "";
                marc.Sai_num_relogio_4 = "";
                marc.Sai_num_relogio_5 = "";
                marc.Sai_num_relogio_6 = "";
                marc.Sai_num_relogio_7 = "";
                marc.Sai_num_relogio_8 = "";
                marc.Entrada_1 = "--:--";
                marc.Entrada_2 = "--:--";
                marc.Entrada_3 = "--:--";
                marc.Entrada_4 = "--:--";
                marc.Entrada_5 = "--:--";
                marc.Entrada_6 = "--:--";
                marc.Entrada_7 = "--:--";
                marc.Entrada_8 = "--:--";
                marc.Saida_1 = "--:--";
                marc.Saida_2 = "--:--";
                marc.Saida_3 = "--:--";
                marc.Saida_4 = "--:--";
                marc.Saida_5 = "--:--";
                marc.Saida_6 = "--:--";
                marc.Saida_7 = "--:--";
                marc.Saida_8 = "--:--";
                marc.Acao = Modelo.Acao.Alterar;
            }
            bllMarcacao.Salvar(Modelo.Acao.Alterar, listaMarcacoes);

            ObjProgressBar.incrementaPB(25);
            ObjProgressBar.setaMensagem("Recalculando marcações...");

            ObjProgressBar.incrementaPB(25);

            DateTime? dataInicialImp = pMarcacao.Data;
            DateTime? dataFinalImp = pMarcacao.Data;

            if (diasAtras != 0)
                dataInicialImp = pMarcacao.Data.AddDays(-diasAtras);

            if (diasFrente != 0)
                dataFinalImp = pMarcacao.Data.AddDays(diasFrente);

            DateTime? dataInicial = null;
            DateTime? dataFinal = null;
            bllImportaBilhetes.ImportarBilhetes(objFuncionario.Dscodigo, true, dataInicialImp, dataFinalImp, out dataInicial, out dataFinal, ObjProgressBar, log);

            BLL.CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(2, objFuncionario.Id, dataInicialImp.Value, dataFinalImp.Value, ObjProgressBar, false, ConnectionString, UsuarioLogado, false);
            bllCalculaMarcacao.CalculaMarcacoes();
        }

        #endregion

        public static void AjustarPosicaoBilhetes(List<Modelo.BilhetesImp> bilhetes)
        {
            //Inicia como sendo a primeira entrada
            string tipo = "E";
            int posicao = 1;

            foreach (var item in (from bil in bilhetes where bil.Acao != Modelo.Acao.Excluir orderby bil.OrdenacaoPosicaoEnt_Sai select bil))
            {
                item.Acao = item.Id > 0 ? Modelo.Acao.Alterar : Modelo.Acao.Incluir;

                if (tipo == "E")
                {
                    item.Ent_sai = tipo;
                    item.Posicao = posicao;
                    // Apos indicar o registro de entrada, o próximo da interação é de saída
                    tipo = "S";
                }
                else
                {
                    item.Ent_sai = tipo;
                    item.Posicao = posicao;
                    // Apos indicar o registro de saida incremento a posicao e indica que o proximo será uma entrada
                    tipo = "E";
                    posicao++;
                }
            }
        }

        public DataTable GetIdsBilhetesByIdRegistroPonto(IList<int> IdsRegistrosPonto)
        {
            return dalBilheteSimp.GetIdsBilhetesByIdRegistroPonto(IdsRegistrosPonto);
        }

        public DataTable GetBilhetesImportarByIDs(List<int> idsBilhetes)
        {
            return dalBilheteSimp.GetBilhetesImportarByIDs(idsBilhetes);
        }

        public List<Modelo.BilhetesImp> LoadPorRegistroPonto(List<int> IdsRegistrosPonto)
        {
            return dalBilheteSimp.LoadPorRegistroPonto(IdsRegistrosPonto);
        }

        public List<Modelo.BilhetesImp> LoadObject(List<int> Ids)
        {
            return dalBilheteSimp.LoadObject(Ids);
        }

        /// <summary>
        /// Gera os Bilhetes de acordo com os registros a serem processados
        /// </summary>
        /// <param name="registros">Registros para gerar o Bilhetes</param>
        /// <returns>Lista De Bilhetes</returns>
        public List<Modelo.BilhetesImp> GerarBilhetesPelosRegistrosPonto(List<Modelo.RegistroPonto> registros)
        {
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            if (registros.Count > 0)
            {
                int maxCodigo = MaxCodigo();

                BLL.Funcionario bllFuncionario = new BLL.Funcionario(ConnectionString, UsuarioLogado);
                List<Modelo.Funcionario> funcs = new List<Modelo.Funcionario>();
                string idsFuncs = String.Join(",", registros.Select(s => s.IdFuncionario).Distinct().ToList());
                funcs = bllFuncionario.GetAllListByIds("(" + idsFuncs + ")");

                //Inclui apenas o que ainda não foi incluido anteriormente
                foreach (var registro in registros)
                {
                    registro.Funcionario = funcs.Where(f => f.Id == registro.IdFuncionario).FirstOrDefault();
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    objBilhete.Ordem = "000";
                    objBilhete.Data = Convert.ToDateTime(registro.Batida).Date;
                    objBilhete.Hora = registro.Batida.ToShortTimeString();
                    objBilhete.Func = registro.Funcionario.Dscodigo.ToString();
                    objBilhete.Relogio = registro.OrigemRegistro;
                    objBilhete.Importado = 2;
                    objBilhete.Codigo = maxCodigo++;
                    objBilhete.Mar_data = objBilhete.Data;
                    objBilhete.Mar_hora = objBilhete.Hora;
                    objBilhete.Mar_relogio = objBilhete.Relogio;
                    objBilhete.DsCodigo = objBilhete.Func;
                    objBilhete.Incdata = DateTime.Now.Date;
                    objBilhete.Inchora = DateTime.Now;
                    objBilhete.Nsr = registro.NSR.GetValueOrDefault();
                    objBilhete.PIS = registro.Funcionario.Pis;
                    objBilhete.IdFuncionario = registro.Funcionario.Id;
                    objBilhete.IdRegistroPonto = registro.Id;
                    objBilhete.RegistroPonto = registro;
                    if (!String.IsNullOrEmpty(registro.IpInterno) || !String.IsNullOrEmpty(registro.IpPublico) || registro.Latitude != null)
                    {
                        objBilhete.localizacaoRegistroPonto = new Modelo.LocalizacaoRegistroPonto()
                        {
                            Browser = registro.Browser,
                            BrowserPlatform = registro.BrowserPlatform,
                            BrowserVersao = registro.BrowserVersao,
                            IpInterno = registro.IpInterno,
                            IpPublico = registro.IpPublico,
                            Latitude = registro.Latitude.GetValueOrDefault(),
                            Longitude = registro.Longitude.GetValueOrDefault(),
                            X_FORWARDED_FOR = registro.XFORWARDEDFOR
                        };
                    }
                    bilhetes.Add(objBilhete);
                } 
            }

            return bilhetes;
        }


        public int DesconsiderarBilhetesPorRegistroPonto(List<Modelo.RegistroPonto> registrosDesconsiderar)
        {
            int ret = 0;
            registrosDesconsiderar = registrosDesconsiderar.Where(w => w.Acao == (Modelo.Acao)2).ToList();
            if (registrosDesconsiderar.Count > 0)
            {
                BLL.Justificativa bllJustificativa = new BLL.Justificativa(ConnectionString, UsuarioLogado);
                Modelo.Justificativa justificativa = bllJustificativa.LoadObjectParaColetor();

                //IList<int> idsBilhetesDesconsiderar = bilhetesJaInseridos.AsEnumerable().Where(w => registrosDesconsiderar.Select(s => s.Id).Contains(w.Field<int>("IdRegistroPonto"))).Select(i => Convert.ToInt32(i["IdRegistroPonto"])).ToList();
                List<Modelo.BilhetesImp> bilhetesDesconsiderar = LoadPorRegistroPonto(registrosDesconsiderar.Select(s => s.Id).ToList());
                bilhetesDesconsiderar.ForEach(f => { f.Ocorrencia = 'D'; f.Motivo = justificativa.Descricao; f.Idjustificativa = justificativa.Id; f.Importado = 2; });

                ret = Salvar(Modelo.Acao.Alterar, bilhetesDesconsiderar, UsuarioLogado.Login, ConnectionString);
            }
            return ret;
        }


        public int ReconsiderarBilhetesPorRegistroPonto(List<Modelo.BilhetesImp> BilhetesProcessar, List<Modelo.RegistroPonto> registros)
        {
            int ret = 0;
            List<Modelo.BilhetesImp> bilhetesAlterar = new List<Modelo.BilhetesImp>();
            foreach (Modelo.BilhetesImp bilhetesAlt in BilhetesProcessar.Where(w => w.Ocorrencia == 'D' && registros.Where(x => x.Acao == 0).Select(s => s.Id).Contains(w.IdRegistroPonto.GetValueOrDefault())).ToList())
            {
                if (bilhetesAlt.Ocorrencia == 'D' && registros.Where(w => w.Id == bilhetesAlt.IdRegistroPonto).First().Acao == 0)
                {
                    bilhetesAlt.Ocorrencia = '\0';
                    bilhetesAlt.Motivo = "";
                    bilhetesAlt.Idjustificativa = 0;
                    bilhetesAlt.Importado = 2;
                    bilhetesAlterar.Add(bilhetesAlt);
                }
            }
            if (bilhetesAlterar.Count > 0)
            {
                ret = Salvar(Modelo.Acao.Alterar, bilhetesAlterar, UsuarioLogado.Login, ConnectionString);
            }
            return ret;
        }
    }
}
