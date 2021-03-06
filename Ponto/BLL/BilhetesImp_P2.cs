using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Collections;
using cwkPontoMT.Integracao;
using cwkPontoMT.Integracao.Entidades;
using System.Globalization;

namespace BLL
{
    public partial class BilhetesImp : IBLL<Modelo.BilhetesImp>
    {
        private Modelo.ProgressBar objProgressBar;
        private IFormatProvider _cultureInfoBr = new CultureInfo("pt-BR");
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
        /// Retorna uma lista de bilhetes de um per?odo
        /// </summary>
        /// <param name="tipo">Tipo: 0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Fun??o, 4 - Hor?rio, 5 - Todos, 6 - Contrato </param>
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

        #region NovaImplementa??o
        /// <summary>
        /// M?todo respons?vel em retornar um DataTable com todos os bilhetes que n?o foram importados
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

        #region Importa??o de Bilhetes


        public bool ImportacaoBilhetes(List<Modelo.TipoBilhetes> plistaTipoBilhetes, string pArquivo, int pBilhete, bool pIndividual, string pFuncionario,
                                       ref DateTime? pDataI, ref DateTime? pDataF, List<string> log, Modelo.Cw_Usuario usuarioLogado, out IList<Modelo.Funcionario> funcionariosNoArquivo, bool? bRazaoSocial)
        {
            BLL.REP bllRep;
            BLL.Funcionario bllFunc;
            funcionariosNoArquivo = new List<Modelo.Funcionario>();

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
            if (!ValidaImportacaoBilhetes(plistaTipoBilhetes, pArquivo, pBilhete, pDataI, pDataF, log, out listaPis))
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
            List<string> valorErrado = new List<string>();
            int qtdLidos = 0, qtdProcessados = 0, qtdErrados = 0, qtdRepetidos = 0, qtdSemPermissao = 0, qtdPontoFechado = 0;
            DateTime dataInicial = new DateTime(), dataFinal = new DateTime();
            #endregion

            DataTable pisFuncionarios = null;
            if (plistaTipoBilhetes.Where(t => (t.FormatoBilhete == 3 || t.FormatoBilhete == 4 || t.FormatoBilhete == 5) && t.BImporta).Any())
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
                            throw new Exception("Para comunica??o com REPs Homologados pelo INMETRO ? obrigat?rio informar o Login, Senha e CPF para comunica??o com o Equipamento. "
                                + "Verifique o cadastro deste usu?rio (" + UsuarioLogado.Nome + ") e realize o preenchimento dos campos \"Login REP\", \"Senha REP\" e \"CPF\".");
                        }

                        relogio.UsuarioREP = UsuarioLogado.LoginRep;
                        relogio.SenhaUsuarioREP = UsuarioLogado.SenhaRep;
                        relogio.Cpf = UsuarioLogado.Cpf;
                    }


                    if (relogio.GetType() == typeof(cwkPontoMT.Integracao.Relogios.TopData.InnerRep) ||
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
                    funcionariosNoArquivo = bllFunc.GetAllListComDataUltimoFechamento(false, idsFuncs);
                    foreach (RegistroAFD reg in registros)
                    {
                        if (reg.Campo01 != "999999999")
                        {
                            switch (reg.Campo02)
                            {
                                case "1":
                                    //Busca o n?mero do rep
                                    numeroRelogio = bllRep.GetNumInner(reg.Campo07);
                                    if (string.IsNullOrEmpty(numeroRelogio))
                                    {
                                        controleRelogio = ("O REP " + reg.Campo07 + " n?o est? cadastrado no sistema!");
                                        break;
                                    }
                                    if (!bllRep.GetCPFCNPJ(reg.Campo04, reg.Campo03))
                                    {
                                        controleCNPJCPF = (reg.Campo04 + " n?o esta cadastrado como cnpj ou cpf da empresa");
                                        numeroRelogio = String.Empty;
                                    }
                                    break;
                                case "2":
                                    break;
                                case "3":
                                    DateTime dataBil = Convert.ToDateTime(reg.Campo04.Substring(0, 2) + "/" + reg.Campo04.Substring(2, 2) + "/" + reg.Campo04.Substring(4, 4), _cultureInfoBr);
                                    contadorProcessados++;
                                    //Cria o bilhete
                                    string validaHora = reg.Campo05.Substring(0, 4);
                                    int intHora = 0;
                                    if (!int.TryParse(validaHora, out intHora) || intHora > 2359)
                                    {
                                        valorErrado.Add("Linha " + qtdLidos + ": Valor incorreto." + reg.Campo05.Substring(0, 4));
                                        qtdErrados++;
                                        continue;
                                    }
                                    objBilhete = new Modelo.BilhetesImp();
                                    objBilhete.Ordem = "000";
                                    objBilhete.Data = dataBil;
                                    objBilhete.Hora = reg.Campo05.Substring(0, 2) + ":" + reg.Campo05.Substring(2, 2);
                                    objBilhete.Nsr = reg.Nsr;

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
                        DateTime dataUltimoBilhete = Convert.ToDateTime(bilhete.Data.ToShortDateString() + " " + bilhete.Hora, _cultureInfoBr);
                        bllRep.SetUltimaImportacao(numeroRelogio, ultimoNsr, dataUltimoBilhete);
                    }

                    qtdRepetidos -= qtdProcessados;
                    log.Add("Leitura do TXT");
                    log.Add("Lidos         = " + String.Format(_cultureInfoBr, "{0, 10}", contadorProcessados));
                    log.Add("Processados   = " + String.Format(_cultureInfoBr, "{0, 10}", qtdProcessados));
                    log.Add("Errados       = " + String.Format(_cultureInfoBr, "{0, 10}", qtdErrados));
                    log.Add("Repetidos     = " + String.Format(_cultureInfoBr, "{0, 10}", qtdRepetidos));
                    log.Add("Sem Permiss?o = " + String.Format(_cultureInfoBr, "{0, 10}", qtdSemPermissao));
                    log.Add("Ponto Fechado = " + String.Format(_cultureInfoBr, "{0, 10}", qtdPontoFechado));
                    log.Add("");
                    if (!String.IsNullOrEmpty(controleCNPJCPF))
                    {
                        log.Add("CNPJ/CPF incorreto = " + String.Format(_cultureInfoBr, "{0, 10}", controleCNPJCPF));
                    }
                    log.Add("");
                    log.Add("N?o Foram importados " + String.Format(_cultureInfoBr, "{0, 10}", naoPossuiFunc) + " registros porque n?o possui o funcion?rio cadastrado no banco");
                    log.Add("");
                    if (valorErrado.Count > 0)
                    {
                        log.Add("---------------------------------------------------------------------------------------");
                        int i = valorErrado.Count;
                        log.Add("Numero de registros com valores errados: " + i + ". N?o Foram importado: ");
                        foreach (var item in valorErrado.GroupBy(g => g))
                        {
                            log.Add(item.Key.PadRight(15, ' '));
                        }
                        log.Add("---------------------------------------------------------------------------------------");
                    }
                    log.Add(controleRelogio);
                    log.Add("");
                    log.Add("---------------------------------------------------------------------------------------");
                }
                else
                {
                    ImportacaoArquivo(pArquivo, pBilhete, pIndividual, pFuncionario, pDataI, pDataF, log, max, contador3, contadorProcessados, naoPossuiFunc, numeroRelogio,
                                      controleRelogio, controleCNPJCPF, qtdLidos, ref qtdProcessados, qtdErrados, qtdRepetidos, qtdSemPermissao, ref dataInicial, ref dataFinal,
                                      tp, pisFuncionarios, usuarioLogado, qtdPontoFechado, out funcionariosNoArquivo, bRazaoSocial);
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
            if (plistaTipoBilhetes.Where(t => (t.FormatoBilhete == 3 || t.FormatoBilhete == 4 || t.FormatoBilhete == 5) && t.BImporta).Any())
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
                                       ref DateTime dataInicial, ref DateTime dataFinal, Modelo.TipoBilhetes tp, DataTable pisFuncionarios, Modelo.Cw_Usuario usuarioLogado, int qtdPontoFechado, out IList<Modelo.Funcionario> funcionariosNoArquivo, bool? bRazaoSocial)
        {
            BLL.REP bllRep;
            BLL.Funcionario bllFunc;
            BLL.Horario bllHorario;
            funcionariosNoArquivo = new List<Modelo.Funcionario>();

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
                funcionariosNoArquivo = bllFunc.GetAllListComDataUltimoFechamento(true, idsFuncs);
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
            //Verifica se o arquivo n?o est? vazio                
            List<string> linhas = File.ReadAllLines(pArquivo).ToList();
            if (linhas.Count == 0)
            {
                return;
            }

            List<string> lPisNaoEncontrado = new List<string>();

            List<Modelo.BilhetesImp> listaBilhetes = new List<Modelo.BilhetesImp>();
            Modelo.BilhetesImp objBilhete;
            decimal tamanhoProgress = 30; // Tamanho do peda?o do progress que ser? destinado a valida??o das linhas do arquivo, considerando o progress de 0 a 100;
            decimal count = 0;
            int incrementoAnt = 0;
            int tamanhoProgressAtual = objProgressBar.valorCorrenteProgress();
            decimal total = linhas.Count;
            List<string> valorErrado = new List<string>();

            if ((tp.FormatoBilhete == 3) || (tp.FormatoBilhete == 5))
            {
                #region REP
                //Processar o arquivo
                log.Add("Arquivo Importado: " + Path.GetFileName(pArquivo));
                int primeiraLinhaImportar = 0;
                if (pDataI != null)
                {
                    string procurarData = pDataI.GetValueOrDefault().ToString("ddMMyyyy", _cultureInfoBr);
                    primeiraLinhaImportar = linhas.FindIndex(f => f.Contains(procurarData));
                }
                List<string> registrosProcessar = new List<string>();
                if (primeiraLinhaImportar > 0)
                    registrosProcessar.Add(linhas.FirstOrDefault());
                registrosProcessar.AddRange(linhas.Skip(primeiraLinhaImportar));
                total = registrosProcessar.Count;
                foreach (string linha in registrosProcessar)
                {
                    count++;
                    int incremento = (int)(tamanhoProgress * (((count * 100) / total)/ 100));
                    if (incrementoAnt != incremento || count == 1 || count == total)
                    {
                        objProgressBar.setaValorPB(tamanhoProgressAtual + incremento);
                        objProgressBar.incrementaPBCMensagem(0, String.Format(_cultureInfoBr, "Realizando leitura da linha {0} de {1}", count, total));
                        incrementoAnt = incremento;
                    }

                    if (linha == null)
                    {
                        break;
                    }
                    if (string.IsNullOrEmpty(linha))
                    {
                        continue;
                    }
                    qtdLidos++;

                    //Verifica se a linha ? tipo 1
                    if (linha.Substring(9, 1) == "1")
                    {
                        //Busca o n?mero do rep
                        if (!String.IsNullOrEmpty(linha.Substring(187, 17)))
                        {
                            numeroRelogio = bllRep.GetNumInner(linha.Substring(187, 17));
                            if (string.IsNullOrEmpty(numeroRelogio))
                            {
                                controleRelogio = ("O REP " + linha.Substring(187, 17) + " n?o est? cadastrado no sistema!");
                                break;
                            }
                            if (!bllRep.GetCPFCNPJ(linha.Substring(11, 14), linha.Substring(10, 1)) && bRazaoSocial != true)
                            {
                                controleCNPJCPF = (linha.Substring(11, 14) + " n?o esta cadastrado como cnpj ou cpf da empresa");
                                numeroRelogio = "";
                            }
                        }
                        else
                        {
                            controleRelogio = ("N?o existe o n?mero do REP no arquivo txt");
                            break;
                        }
                    }
                    else if (linha.Substring(9, 1) == "3")
                    {
                        //Condi??o adicionada para descartar a linha de assinatura digital do AFD, que coincidentemente pode possuir um caracter 3 na posi??o 9 e o sistema estava entendendo como registro de ponto                        
                        if (!int.TryParse(linha.Substring(0, 9), out int conv))
                        {
                            continue;
                        }
                        DateTime dataBil = Convert.ToDateTime(linha.Substring(10, 2) + "/" + linha.Substring(12, 2) + "/" + linha.Substring(14, 4), _cultureInfoBr);
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
                        string validaHora = linha.Substring(18, 2) + linha.Substring(20, 2);
                        int intValidaHora = 0;
                        if (!int.TryParse(validaHora, out intValidaHora) || Convert.ToInt32(intValidaHora) > 2359)
                        {
                            valorErrado.Add("Linha "+ qtdLidos+": Valor incorreto." + linha.Substring(18, 2) + linha.Substring(20, 2));
                            qtdErrados++;
                            continue;
                        }
                        string nsrString = linha.Substring(0, 9);
                        int nsr = 0;
                        if (!int.TryParse(nsrString, out nsr))
                        {
                            valorErrado.Add("Linha " + qtdLidos + ": Valor de nsr incorreto. (" + linha.Substring(0, 9) + ")");
                            qtdErrados++;
                            continue;

                        }
                        objBilhete = new Modelo.BilhetesImp();
                        objBilhete.Nsr = nsr;
                        objBilhete.Ordem = "000";
                        objBilhete.Data = dataBil;
                        objBilhete.Hora = linha.Substring(18, 2) + ":" + linha.Substring(20, 2);
                        Dictionary<String, int> dsCodigos = new Dictionary<String, int>();

                        var dsCodigosLista = pisFuncionarios.AsEnumerable().Where(row => row.Field<String>("pis") == linha.Substring(22, 12) && row.Field<int>("excluido") == 0);

                        if (dsCodigosLista.Any())
                        {
                            DataTable dt = dsCodigosLista.CopyToDataTable().DefaultView.ToTable(false, "dscodigo", "funcionarioativo");
                            try
                            {
                                dsCodigos = dt.AsEnumerable().ToDictionary(row => row.Field<String>("dscodigo"), row => row.Field<int>("funcionarioativo"));
                            }
                            catch (Exception)
                            {
                                throw new Exception(String.Format(_cultureInfoBr, "Existem funcion?rios cadastrados com o mesmo DsCodigo."));
                            } 
                        }

                        Modelo.Funcionario func = new Modelo.Funcionario();
                        if (dsCodigos.Any())
                        {
                            objBilhete.Func = dsCodigos.OrderByDescending(o => o.Value).FirstOrDefault().Key.ToString(_cultureInfoBr);
                            bool erro = false;
                            func = ValidaFuncionarioPermissaoFechamento(pIndividual, ref qtdSemPermissao, usuarioLogado, ref qtdPontoFechado, funcionariosNoArquivo, objBilhete, ref erro, pFuncionario);
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
                qtdProcessados = SalvarBilhetesEFinalizarLog(ref log, contadorProcessados, naoPossuiFunc, numeroRelogio, controleRelogio, controleCNPJCPF, qtdErrados, ref qtdRepetidos, qtdSemPermissao, qtdPontoFechado, bllRep, lPisNaoEncontrado, listaBilhetes, linhas, valorErrado);
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
                        objProgressBar.incrementaPBCMensagem(0, String.Format(_cultureInfoBr, "Realizando leitura da linha {0} de {1}", count, total));
                        incrementoAnt = incremento;
                    }

                    if (linha == null)
                    {
                        break;
                    }
                    if (String.IsNullOrEmpty(linha))
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
                            //Cria o bilhete
                            string validaHora = linha.Substring(13, 5);
                            int hora = 0;
                            if (!int.TryParse(validaHora, out hora) ||hora > 02359)
                            {
                                valorErrado.Add("Linha " + qtdLidos + ": Valor incorreto." + linha.Substring(13, 5));
                                qtdErrados++;
                                continue;
                            }
                            objBilhete = new Modelo.BilhetesImp();
                            objBilhete.Ordem = linha.Substring(0, 3);
                            objBilhete.Data = Convert.ToDateTime(linha.Substring(4, 8), _cultureInfoBr);
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
                            //Cria o bilhete
                            validaHora = linha.Substring(13, 5);
                            hora = 0;
                            if (!int.TryParse(validaHora, out hora) || hora > 02359)
                            {
                                valorErrado.Add("Linha " + qtdLidos + ": Valor incorreto." + linha.Substring(13, 5));
                                qtdErrados++;
                                continue;
                            }
                            objBilhete = new Modelo.BilhetesImp();
                            objBilhete.Ordem = linha.Substring(0, 3);
                            objBilhete.Data = Convert.ToDateTime(linha.Substring(4, 8), _cultureInfoBr);
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
                            //Cria o bilhete
                            validaHora = linha.Substring(tp.Hora_c, tp.Hora_t) + linha.Substring(tp.Minuto_c, tp.Minuto_t);
                            hora = 0;
                            if (!int.TryParse(validaHora, out hora) || hora > 02359)
                            {
                                valorErrado.Add("Linha " + qtdLidos + ": Valor incorreto." + linha.Substring(tp.Hora_c, tp.Hora_t) + linha.Substring(tp.Minuto_c, tp.Minuto_t));
                                qtdErrados++;
                                continue;
                            }
                            objBilhete = new Modelo.BilhetesImp();
                            objBilhete.Ordem = linha.Substring(tp.Ordem_c, tp.Ordem_t);
                            if (String.IsNullOrEmpty(objBilhete.Ordem.Trim()))
                                objBilhete.Ordem = "000";
                            objBilhete.Data = Convert.ToDateTime(linha.Substring(tp.Dia_c, tp.Dia_t) + "/" + linha.Substring(tp.Mes_c, tp.Mes_t) + "/" + linha.Substring(tp.Ano_c, tp.Ano_t), _cultureInfoBr);
                            objBilhete.Hora = linha.Substring(tp.Hora_c, tp.Hora_t) + ":" + linha.Substring(tp.Minuto_c, tp.Minuto_t);
                            objBilhete.Func = linha.Substring(tp.Funcionario_c, tp.Funcionario_t);
                            objBilhete.Relogio = linha.Substring(tp.Relogio_c, tp.Relogio_t);

                            //Caso o layout n?o possua n?mero de rel?gio ele ? setado como "00"
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
                    //Caso seja importa??o individual, verifica se o bilhete ? do funcion?rio selecionado
                    if (pIndividual == true && objBilhete.Func != pFuncionario)
                    {
                        continue;
                    }
                    //Caso seja importa??o individual, verifica se o bilhete est? dentro do per?odo selecionado
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

                qtdProcessados = SalvarBilhetesEFinalizarLog(ref log, contadorProcessados, naoPossuiFunc, numeroRelogio, controleRelogio, controleCNPJCPF, qtdErrados, ref qtdRepetidos, qtdSemPermissao, qtdPontoFechado, bllRep, lPisNaoEncontrado, listaBilhetes, linhas, valorErrado);
                #endregion
            }
            //LimpaArquivo(pArquivo);
        }

        private int SalvarBilhetesEFinalizarLog(ref List<string> log, int contadorProcessados, int naoPossuiFunc, string numeroRelogio, string controleRelogio, string controleCNPJCPF, int qtdErrados, ref int qtdRepetidos, int qtdSemPermissao, int qtdPontoFechado, REP bllRep, List<string> lPisNaoEncontrado, List<Modelo.BilhetesImp> listaBilhetes, List<string> linhas, List<string> valorErrado)
        {
            int qtdProcessados;
            objProgressBar.setaValorPB(40);
            objProgressBar.incrementaPBCMensagem(0, "Salvando bilhetes...");
            qtdProcessados = this.Salvar(Modelo.Acao.Incluir, listaBilhetes);
            if (qtdProcessados > 0)
            {
                Modelo.BilhetesImp bilhete = UltimoBilhetePorRep(numeroRelogio);
                long ultimoNsr = bilhete.Nsr;
                DateTime dataUltimoBilhete = Convert.ToDateTime(bilhete.Data.ToShortDateString() + " " + bilhete.Hora, _cultureInfoBr);
                bllRep.SetUltimaImportacao(numeroRelogio, ultimoNsr, dataUltimoBilhete);
            }
            objProgressBar.incrementaPBCMensagem(20, "Bilhetes Salvos (" + qtdProcessados + "), gerando arquivo de retorno");
            qtdRepetidos -= qtdProcessados;
            log.Add("Leitura do TXT");
            log.Add("Total de Linhas = " + String.Format(_cultureInfoBr, "{0, 10}", linhas.Count));
            log.Add("Descartados     = " + String.Format(_cultureInfoBr, "{0, 10}", linhas.Count - contadorProcessados));
            log.Add("Lidos           = " + String.Format(_cultureInfoBr, "{0, 10}", contadorProcessados));
            log.Add("Processados     = " + String.Format(_cultureInfoBr, "{0, 10}", qtdProcessados));
            log.Add("N?o Encontrado  = " + String.Format(_cultureInfoBr, "{0, 10}", naoPossuiFunc));
            log.Add("Errados         = " + String.Format(_cultureInfoBr, "{0, 10}", qtdErrados));
            log.Add("Repetidos       = " + String.Format(_cultureInfoBr, "{0, 10}", qtdRepetidos));
            log.Add("Sem Permiss?o   = " + String.Format(_cultureInfoBr, "{0, 10}", qtdSemPermissao));
            log.Add("Ponto Fechado   = " + String.Format(_cultureInfoBr, "{0, 10}", qtdPontoFechado));
            log.Add("");
            if (!String.IsNullOrEmpty(controleCNPJCPF))
            {
                log.Add("CNPJ/CPF incorreto = " + String.Format(_cultureInfoBr, "{0, 10}", controleCNPJCPF));
                log.Add("");
            }

            if (lPisNaoEncontrado.Count > 0)
            {
                log.Add("N?o Foram importados " + String.Format(_cultureInfoBr, "{0, 10}", naoPossuiFunc) + " registros, pois os funcion?rios n?o foram cadastrados ou est?o inativos ou exclu?dos. PIS n?o encontrado:");
                log.Add("PIS".PadRight(15, ' ') + " | " + "Quantidade".ToString(_cultureInfoBr).PadLeft(10, ' '));
                foreach (var item in lPisNaoEncontrado.GroupBy(g => g))
                {
                    log.Add(item.Key.PadRight(15, ' ') + " | " + item.ToList().Count.ToString(_cultureInfoBr).PadLeft(10, ' '));
                }
                log.Add("");
            }
            if (valorErrado.Count > 0)
            {
                log.Add("---------------------------------------------------------------------------------------");
                int i = valorErrado.Count;
                log.Add("Numero de registros com valores errados: " + i + ". N?o Foram importados: ");
                foreach (var item in valorErrado.GroupBy(g => g))
                {
                    log.Add(item.Key.PadRight(15, ' '));
                }
                log.Add("---------------------------------------------------------------------------------------");
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
            //Verifica se o arquivo n?o est? vazio                
            int tam = File.ReadAllLines(pArquivo).GetLength(0);
            if (tam == 0)
            {
                return;
            }

            using (FileStream stream = new FileStream(pArquivo, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                StreamReader objReader = new StreamReader(stream);

                //Atualiza as informa??es na barra de progresso
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
                        if (String.IsNullOrEmpty(linha))
                        {
                            continue;
                        }
                        qtdLidos++;

                        //Verifica se a linha ? tipo 1
                        if (linha.Substring(9, 1) == "1")
                        {

                            if (!naoValidaRep)
                            {
                                //Busca o n?mero do rep
                                if (!String.IsNullOrEmpty(linha.Substring(187, 17)))
                                {
                                    numeroRelogio = bllRep.GetNumInner(linha.Substring(187, 17));
                                    if (String.IsNullOrEmpty(numeroRelogio))
                                    {
                                        controleRelogio = ("O REP " + linha.Substring(187, 17) + " n?o est? cadastrado no sistema!");
                                        break;
                                    }

                                }
                                else
                                {
                                    controleRelogio = ("N?o existe o n?mero do REP no arquivo txt");
                                    break;
                                }
                            }

                            if (!bllRep.GetCPFCNPJ(linha.Substring(11, 14), linha.Substring(10, 1)))
                            {
                                controleCNPJCPF = (linha.Substring(11, 14) + " n?o esta cadastrado como cnpj ou cpf da empresa");
                                numeroRelogio = "";
                            }
                        }
                        else if (linha.Substring(9, 1) == "3")
                        {
                            DateTime dataBil = Convert.ToDateTime(linha.Substring(10, 2) + "/" + linha.Substring(12, 2) + "/" + linha.Substring(14, 4), _cultureInfoBr);
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
                            string validaHora = linha.Substring(18, 4);
                            int hora = 0;
                            if (!int.TryParse(validaHora, out hora) || hora > 2359)
                            {
                                log.Add(String.Format(_cultureInfoBr, "Linha {0}: Valor incorreto", qtdLidos));
                                qtdErrados++;
                                continue;
                            }
                            string nsrString = linha.Substring(0, 9);
                            int nsr = 0;
                            if (!int.TryParse(nsrString, out nsr))
                            {
                                log.Add(String.Format(_cultureInfoBr, "Linha {0}: Valor incorreto do nsr", qtdLidos));
                                qtdErrados++;
                                continue;
                            }
                            objBilhete = new Modelo.BilhetesImp();
                            objBilhete.Nsr = nsr;
                            objBilhete.Ordem = "000";
                            objBilhete.Data = dataBil;
                            objBilhete.Hora = linha.Substring(18, 2) + ":" + linha.Substring(20, 2);
                            bool encontrouFunc = false;
                            //Busca o DSCodigo do funcion?rio pelo PIS
                            Dictionary<String, int> dsCodigos = pisFuncionarios
                                                            .AsEnumerable()
                                                            .Where(row => row.Field<String>("pis") == linha.Substring(22, 12) && row.Field<int>("excluido") == 0)
                                                            .ToDictionary(row => row.Field<String>("dscodigo"), row => row.Field<int>("funcionarioativo"));
                            encontrouFunc = dsCodigos.Count > 0;
                            Modelo.Funcionario func = new Modelo.Funcionario();
                            if (encontrouFunc)
                            {
                                objBilhete.Func = dsCodigos.OrderByDescending(o => o.Value).FirstOrDefault().Key.ToString(_cultureInfoBr);
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
                        DateTime dataUltimoBilhete = Convert.ToDateTime(bilhete.Data.ToShortDateString() + " " + bilhete.Hora, _cultureInfoBr);
                        bllRep.SetUltimaImportacao(numeroRelogio, ultimoNsr, dataUltimoBilhete);
                    }

                    dsCodigoFunc = String.Join(",", listaBilhetes.Select(x => "'" + x.DsCodigo + "'").Distinct());
                    qtdRepetidos -= qtdProcessados;
                    log.Add("Leitura do TXT");
                    log.Add("Lidos       = " + String.Format(_cultureInfoBr, "{0, 10}", contadorProcessados));
                    log.Add("Processados = " + String.Format(_cultureInfoBr, "{0, 10}", qtdProcessados));
                    log.Add("Errados     = " + String.Format(_cultureInfoBr, "{0, 10}", qtdErrados));
                    log.Add("Repetidos   = " + String.Format(_cultureInfoBr, "{0, 10}", qtdRepetidos));
                    log.Add("Ponto Fechado = " + String.Format(_cultureInfoBr, "{0, 10}", qtdPontoFechado));
                    log.Add("");

                    bErro = VerificaErroImportacao(qtdErrados, bErro);
                    if (!string.IsNullOrEmpty(controleCNPJCPF))
                    {
                        log.Add("CNPJ/CPF incorreto = " + String.Format(_cultureInfoBr, "{0, 10}", controleCNPJCPF));
                        bErro = true;
                    }

                    log.Add("");
                    log.Add("N?o Foram importados " + String.Format(_cultureInfoBr, "{0, 10}", naoPossuiFunc) + " registros porque n?o possui o funcion?rio cadastrado no banco");
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
                        if (string.IsNullOrEmpty(linha))
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
                                //Cria o bilhete
                                string validaHora = linha.Substring(13,5);
                                int hora = 0;
                                if (!int.TryParse(validaHora, out hora) || hora > 2359)
                                {
                                    log.Add(String.Format(_cultureInfoBr, "Linha {0}: Valor incorreto", qtdLidos));
                                    qtdErrados++;
                                    continue;
                                }
                                objBilhete = new Modelo.BilhetesImp();
                                objBilhete.Ordem = linha.Substring(0, 3);
                                objBilhete.Data = Convert.ToDateTime(linha.Substring(4, 8), _cultureInfoBr);
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
                                //Cria o bilhete
                                validaHora = linha.Substring(13,5);
                                hora = 0;
                                if (!int.TryParse(validaHora, out hora) || hora > 2359)
                                {
                                    log.Add(String.Format(_cultureInfoBr, "Linha {0}: Valor incorreto", qtdLidos));
                                    qtdErrados++;
                                    continue;
                                }
                                objBilhete = new Modelo.BilhetesImp();
                                objBilhete.Ordem = linha.Substring(0, 3);
                                objBilhete.Data = Convert.ToDateTime(linha.Substring(4, 8), _cultureInfoBr);
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
                                //Cria o bilhete
                                validaHora = linha.Substring(13, 5);
                                hora = 0;
                                if (!int.TryParse(validaHora, out hora) || hora > 2359)
                                {
                                    log.Add(String.Format(_cultureInfoBr, "Linha {0}: Valor incorreto", qtdLidos));
                                    qtdErrados++;
                                    continue;
                                }
                                objBilhete = new Modelo.BilhetesImp();
                                objBilhete.Ordem = linha.Substring(tp.Ordem_c, tp.Ordem_t);
                                if (String.IsNullOrEmpty(objBilhete.Ordem.Trim()))
                                    objBilhete.Ordem = "000";
                                objBilhete.Data = Convert.ToDateTime(linha.Substring(tp.Dia_c, tp.Dia_t) + "/" + linha.Substring(tp.Mes_c, tp.Mes_t) + "/" + linha.Substring(tp.Ano_c, tp.Ano_t), _cultureInfoBr);
                                objBilhete.Hora = linha.Substring(tp.Hora_c, tp.Hora_t) + ":" + linha.Substring(tp.Minuto_c, tp.Minuto_t);
                                objBilhete.Func = linha.Substring(tp.Funcionario_c, tp.Funcionario_t);
                                objBilhete.Relogio = linha.Substring(tp.Relogio_c, tp.Relogio_t);

                                //Caso o layout n?o possua n?mero de rel?gio ele ? setado como "00"
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
                        //Caso seja importa??o individual, verifica se o bilhete ? do funcion?rio selecionado
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
                        //Caso seja importa??o individual, verifica se o bilhete est? dentro do per?odo selecionado
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
                        DateTime dataUltimoBilhete = Convert.ToDateTime(bilhete.Data.ToShortDateString() + " " + bilhete.Hora, _cultureInfoBr);
                        bllRep.SetUltimaImportacao(numeroRelogio, ultimoNsr, dataUltimoBilhete);
                    }
                    qtdRepetidos -= qtdProcessados;

                    log.Add("Leitura do TXT");
                    log.Add("Lidos       = " + String.Format(_cultureInfoBr, "{0, 10}", qtdLidos));
                    log.Add("Processados = " + String.Format(_cultureInfoBr, "{0, 10}", qtdProcessados));
                    log.Add("Errados     = " + String.Format(_cultureInfoBr, "{0, 10}", qtdErrados));
                    log.Add("Repetidos   = " + String.Format(_cultureInfoBr, "{0, 10}", qtdRepetidos));
                    log.Add("Ponto Fechado = " + String.Format(_cultureInfoBr, "{0, 10}", qtdPontoFechado));
                    log.Add("---------------------------------------------------------------------------------------");

                    bErro = VerificaErroImportacao(qtdErrados, bErro);

                    #endregion
                }

            }
        }

        private static bool VerificaErroImportacao(int qtdErrados, bool bErro)
        {

            if (qtdErrados > 0)
                bErro = true;
            else
                bErro = false;

            return bErro;
        }

        public bool ValidaImportacaoBilhetes(List<Modelo.TipoBilhetes> plistaTipoBilhetes, string pArquivo, int pBilhete, DateTime? pDataI, DateTime? pDataF, List<string> log, out List<string> listaPis)
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
                        log.Add("Para efetuar uma importa??o direta do rel?gio ? necess?rio informar o per?odo de importa??o.");
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
                    log.Add("Arquivo para Valida??o n?o existe: " + Path.GetFileName(pArquivo));
                    continue;
                }

                bool bAfdInmetro = VerificaTipoAfdInmetro(pArquivo);

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

                using (StreamReader objReader = new StreamReader(pArquivo))
                {
                    if (File.ReadAllBytes(pArquivo).Length == 0)
                    {
                        log.Add("Arquivo para Valida??o em Branco: " + Path.GetFileName(pArquivo));
                        continue;
                    }

                    switch (tp.FormatoBilhete)
                    {
                        case 3:
                            log.Add("Arquivo para Valida??o: " + Path.GetFileName(pArquivo));
                            qtdLidos = 0; qtdErrados = 0;
                            int qtdCabecalho = 0;
                            while (linha != null)
                            {
                                linha = objReader.ReadLine();
                                if (String.IsNullOrEmpty(linha))
                                {
                                    break;
                                }
                                qtdLidos++;
                                if (int.TryParse(linha.Substring(0, 9), out int conv))
                                {
                                    switch (linha.Substring(9, 1))
                                    {
                                        case "1":
                                            if (qtdCabecalho > 0)
                                            {
                                                log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente de REP (AFD), cont?m mais de um cabe?alho", qtdLidos));
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
                                                log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente de REP (AFD), n?o cont?m linha header", qtdLidos));
                                                arquivoOk = false;
                                                qtdErrados++;
                                            }
                                            else
                                            {
                                                if (linha.Length != 34)
                                                {
                                                    log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente de REP (AFD)", qtdLidos));
                                                    arquivoOk = false;
                                                    qtdErrados++;
                                                }
                                                else
                                                {
                                                    foreach (char c in linha.Substring(10, 24))
                                                    {
                                                        if (!Char.IsNumber(c))
                                                        {
                                                            log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente de REP (AFD)", qtdLidos));
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
                            }
                            break;
                        case 5:
                            log.Add("Arquivo para Valida??o: " + Path.GetFileName(pArquivo));
                            qtdLidos = 0; qtdErrados = 0;
                            while (linha != null)
                            {
                                linha = objReader.ReadLine();
                                if (String.IsNullOrEmpty(linha))
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
                                            //Condi??o adicionada para descartar a linha de assinatura digital do AFD, que coincidentemente pode possuir um caracter 3 na posi??o 9 e o sistema estava entendendo como registro de ponto                                            int conv = 0;
                                            if (!int.TryParse(linha.Substring(0, 9), out int conv))
                                            {
                                                continue;
                                            }
                                            log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente de REP (AFD)", qtdLidos));
                                            arquivoOk = false;
                                            qtdErrados++;
                                        }
                                        else
                                        {
                                            foreach (char c in linha.Substring(10, 24))
                                            {
                                                if (!Char.IsNumber(c))
                                                {
                                                    log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente de REP (AFD)", qtdLidos));
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
                            log.Add("Arquivo para Valida??o: " + Path.GetFileName(pArquivo));
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
                                            log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente de TopData 5 Digitos", qtdLidos));
                                            arquivoOk = false;
                                            qtdErrados++;
                                        }
                                        if (linha.Substring(6, 1) != "/" || linha.Substring(9, 1) != "/")
                                        {
                                            log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente de TopData 5 Digitos", qtdLidos));
                                            arquivoOk = false;
                                            qtdErrados++;
                                        }
                                        break;
                                    case 1: // TopData 16 Digitos
                                        if (linha.TrimEnd().Length != 38)
                                        {
                                            log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente de TopData 16 Digitos", qtdLidos));
                                            arquivoOk = false;
                                            qtdErrados++;
                                        }
                                        if (linha.Substring(6, 1) != "/" || linha.Substring(9, 1) != "/")
                                        {
                                            log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente de TopData 16 Digitos", qtdLidos));
                                            arquivoOk = false;
                                            qtdErrados++;
                                        }
                                        break;
                                    default:
                                        if (linha.Length != tp.StrLayout.Length)
                                        {
                                            log.Add(String.Format(_cultureInfoBr, "Linha {0}: Layout diferente do Layout Livre definido", qtdLidos));
                                            arquivoOk = false;
                                            qtdErrados++;
                                        }
                                        else
                                        {
                                            if (String.IsNullOrEmpty(linha.Substring(tp.Funcionario_c, tp.Funcionario_t)) || linha.Substring(tp.Funcionario_c, tp.Funcionario_t).Trim().Length != tp.Funcionario_t)
                                            {
                                                log.Add(String.Format(_cultureInfoBr, "Linha {0}: O c?digo do funcion?rio est? com o valor incorreto", qtdLidos));
                                                arquivoOk = false;
                                                qtdErrados++;
                                            }

                                            if (!DateTime.TryParse(linha.Substring(tp.Dia_c, tp.Dia_t) + "/" + linha.Substring(tp.Mes_c, tp.Mes_t) + "/" + linha.Substring(tp.Ano_c, tp.Ano_t), out data) || data == null || data == new DateTime())
                                            {
                                                log.Add(String.Format(_cultureInfoBr, "Linha {0}: A data do bilhete est? com o valor incorreto", qtdLidos));
                                                arquivoOk = false;
                                                qtdErrados++;
                                            }

                                            if (String.IsNullOrEmpty(linha.Substring(tp.Hora_c, tp.Hora_t)) || String.IsNullOrEmpty(linha.Substring(tp.Minuto_c, tp.Minuto_t))
                                                || (linha.Substring(tp.Hora_c, tp.Hora_t).TrimEnd() + ":" + linha.Substring(tp.Minuto_c, tp.Minuto_t).TrimEnd()).Length != 5)
                                            {
                                                log.Add(String.Format(_cultureInfoBr, "Linha {0}: A hora do bilhete est? com o valor incorreto", qtdLidos));
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
                                        log.Add(String.Format(_cultureInfoBr, "Linha {0}: [{1}] Caracter n?o permitido ", qtdLidos, linha[j]));
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
            }

            return arquivoOk;
        }

        private static bool VerificaTipoAfdInmetro(string pArquivo)
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

        #region Manuten??o de Bilhetes

        /// <summary>
        /// M?todo respons?vel pela manuten??o do bilhete
        /// </summary>
        /// <param name="pMarcacao">Marca??o do bilhete</param>
        /// <param name="pBilhete">Bilhete alterado</param>
        /// <param name="pMudancao">Tipo da Mudan?a: 0 - Dia Anterior, 1 - Mesmo Dia, 2 - Dia Posterior</param>
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
            pMarcacao = (pMarcacao == null ? new Modelo.Marcacao() : pMarcacao);
            Modelo.Funcionario objFuncionario = dalFuncionario.LoadObject(pMarcacao.Idfuncionario);
            DateTime dataI = pMarcacao.Data;
            DateTime dataF = pMarcacao.Data;
            if (diasAtras != 0)
                dataI = pMarcacao.Data.AddDays(-diasAtras);

            if (diasFrente != 0)
                dataF = pMarcacao.Data.AddDays(diasFrente);

            List<Modelo.Marcacao> listaMarcacoes = (bllMarcacao == null ? new List<Modelo.Marcacao>() : bllMarcacao.GetPorFuncionario(pMarcacao.Idfuncionario, dataI, dataF, true));

            ObjProgressBar.incrementaPB(25);
            ObjProgressBar.setaMensagem("Alterando bilhetes...");
            List<string> log = new List<string>();

            //Prepara e excluir uma lista de marca??o
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
            ObjProgressBar.setaMensagem("Recalculando marca??es...");

            ObjProgressBar.incrementaPB(25);

            DateTime? dataInicialImp = pMarcacao.Data;
            DateTime? dataFinalImp = pMarcacao.Data;

            if (diasAtras != 0)
                dataInicialImp = pMarcacao.Data.AddDays(-diasAtras);

            if (diasFrente != 0)
                dataFinalImp = pMarcacao.Data.AddDays(diasFrente);

            DateTime? dataInicial = null;
            DateTime? dataFinal = null;
            bllImportaBilhetes.ImportarBilhetes(objFuncionario.Dscodigo, true, dataInicialImp, dataFinalImp, out dataInicial, out dataFinal, ObjProgressBar, log , false);

            BLL.CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(2, objFuncionario.Id, dataInicialImp.Value.AddDays(-1), dataFinalImp.Value.AddDays(1), ObjProgressBar, false, ConnectionString, UsuarioLogado, false);
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
                    // Apos indicar o registro de entrada, o pr?ximo da intera??o ? de sa?da
                    tipo = "S";
                }
                else
                {
                    item.Ent_sai = tipo;
                    item.Posicao = posicao;
                    // Apos indicar o registro de saida incremento a posicao e indica que o proximo ser? uma entrada
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

                //Inclui apenas o que ainda n?o foi incluido anteriormente
                foreach (var registro in registros)
                {
                    registro.Funcionario = funcs.Where(f => f.Id == registro.IdFuncionario).FirstOrDefault();
                    Modelo.BilhetesImp objBilhete = new Modelo.BilhetesImp();
                    objBilhete.Ordem = "000";
                    objBilhete.Data = Convert.ToDateTime(registro.Batida).Date;
                    objBilhete.Hora = registro.Batida.ToShortTimeString();
                    objBilhete.Func = registro.Funcionario.Dscodigo.ToString(_cultureInfoBr);
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

        /// <summary>
        /// Busca os bilhetes de uma lista de funcion?rios
        /// </summary>
        /// <param name="idsFuncs">Ids dos funcion?rios</param>
        /// <param name="dtIni">Data in?cio a ser considerada</param>
        /// <param name="dtFin">Data fim a ser considerada</param>
        /// <param name="importado">Valor do campo importado (passar -1 para todos)</param>
        /// <returns>Retorna lista de bilhestes</returns>
        public List<Modelo.BilhetesImp> GetByIDsFuncs(List<int> idsFuncs, DateTime dtIni, DateTime dtFin, int importado)
        {
            return dalBilheteSimp.GetByIDsFuncs(idsFuncs, dtIni, dtFin, importado);
        }
    }
}
