using BLL.CalculoMarcacoes;
using BLL.CalculoMarcacoes.EstrategiasCalculo.Factories;
using BLL.CalculoMarcacoes.EstrategiasCalculo.Interfaces;
using DAL.SQL;
using Modelo;
using Modelo.Proxy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BLL
{
    public class CalculaMarcacao
    {
        #region Declarações de Variaveis

        #region Progress Bar

        private Modelo.ProgressBar objProgressBar;

        private Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        #endregion

        #region DAL e BLL
        private DAL.ICalculaMarcacao dalCalculaMarcacao;
        private BLL.BancoHoras bllBancoHoras;
        private BLL.Compensacao bllCompensacao;
        private BLL.JornadaAlternativa bllJornadaAlternativa;
        private BLL.BilhetesImp bllBilhetesImp;
        private BLL.Parametros bllParametros;
        private BLL.InclusaoBanco bllInclusaoBanco;
        private BLL.FechamentoBHD bllFechamentoBHD;
        private BLL.Ocorrencia bllOcorrencia;
        private BLL.Funcionario bllFuncionario;
        private BLL.Marcacao bllMarcacao;
        private BLL.HorarioDinamico bllHorarioDinamico;

        #endregion

        #region Listas
        private DataTable Marcacoes;
        private Hashtable bancoHorasList;
        private Hashtable jornadaAlternativaList;
        private Hashtable ocorrenciaList;
        private List<Modelo.BilhetesImp> tratamentomarcacaoList;
        private List<Modelo.FechamentoBHD> fechamentoBHDList;
        private List<Modelo.Compensacao> compensacaoList;
        private List<Modelo.Proxy.PxySaldoBancoHoras> saldoBHFuncs = new List<Modelo.Proxy.PxySaldoBancoHoras>();
        private List<PxyJornadaSubstituirCalculo> pxyJornadaSubstituirCalculos = new List<PxyJornadaSubstituirCalculo>();
        private Hashtable saldoBancoHorasSemanalNaoCarregado;
        #endregion

        #region Atributos do Parametro
        private int inicioAdNoturno, fimAdNoturno, separaExtraFalta, toleranciaAdicionalNoturno;
        int habilitarControleInItinere;
        int? tHoraExtraMin, tHoraExtraEntradaMin, tHoraExtraSaidaMin, tHoraFaltaMin, tHoraFaltaEntradaMin, tHoraFaltaSaidaMin, tHoraExtraIntervaloMin, tHoraFaltaIntervaloMin;
        private string tHoraExtra, tHoraFalta, reducaohoranoturna;
        private bool bConsiderarHEFeriadoPHoraNoturna, bFlgEstenderPeriodoNoturno, bSepararTrabalhadasNoturnaExtrasNoturna;
        #endregion

        #region Atributos da Marcação

        private Modelo.Marcacao objMarcacao = new Modelo.Marcacao();

        private List<Modelo.BilhetesImp> tratamentosMarcacao = null;

        private int id, diaInt, horasTrabalhadasMin, horasTrabalhadasNoturnasMin, horasAdicionalNoturno
        , horasFaltasMin, horasFaltaNoturnaMin, horasCompensadasMin, horasCompensarMin
        , horasExtrasDiurnaMin, horasExtraNoturnaMin, valorDsrMin
        , entrada_1Min, entrada_2Min, entrada_3Min, entrada_4Min
        , entrada_5Min, entrada_6Min, entrada_7Min, entrada_8Min,
        saida_1Min, saida_2Min, saida_3Min, saida_4Min
        , saida_5Min, saida_6Min, saida_7Min, saida_8Min, horasExtraNoturnaFeriadoMin, horasExtraDiurnaFeriadoMin, IdDocumentoWorkflow, AdicionalNoturno;

        private string dscodigo, ocorrencia, horasCompensadas, bancoHorasCre, bancoHorasDeb, legenda, dia, expHorasExtraNot, totalHorasTrabalhadas, totalHorasIntervalo, Interjornada,
                       InItinereHrsDentroJornada, InItinereHrsForaJornada, LegendasConcatenadas, LoginBloqueioEdicaoPnlRh, horaExtraInterjornada;

        private decimal InItinerePercDentroJornada, InItinerePercForaJornada;

        private short dsr, folgaMarcacao, naoConsiderarCafe, naoEntrarBanco, semCalculo, naoConsiderarFeriado, ContabilizarFaltasMarc, ContAtrasosSaidasAntecMarc, ContabilizarCreditosMarc;
        private bool neutroMarcacao, documentoWorkflowAberto, NaoConsiderarInItinere, bCafe;

        DateTime data;

        DateTime? DataBloqueioEdicaoPnlRh;

        private bool calcularInItinere { get { return habilitarControleInItinere == 1 && diaPossuiInItinere && NaoConsiderarInItinere == false; } }
        #endregion

        #region Atributos do Funcionário

        int idFuncionario, idFuncao, idDepartamento, idEmpresa;

        private DateTime? dataAdmissao, dataDemissao, dataInativacao;

        private short naoEntrarNaCompensacao, naoEntrarBancoFunc;

        #endregion

        #region Atributos do Horário e do Horário Detalhe

        private int tipoHorario, diaSemanaDsr
        , entrada_1MinHD, entrada_2MinHD, entrada_3MinHD, entrada_4MinHD
        , saida_1MinHD, saida_2MinHD, saida_3MinHD, saida_4MinHD, limite_max, limite_min, ordenabilhetesaida
        , horasTrabalhadasDentroFeriadoParcialDiurna, horasTrabalhadasDentroFeriadoParcialNoturna
        , horasPrevistasDentroFeriadoParcialDiurna, horasPrevistasDentroFeriadoParcialNoturna;

        private int? totalTrabalhadaDiurnaMin, totalTrabalhadaNoturnaMin, cargaHorariaMistaMin, idJornadaHorario, idJornadaSubstituir;

        private string totalTrabalhadaDiurna, totalTrabalhadaNoturna, cargaHorariaMista;

        private short consideraAdHTrabalhadas, conversaoHoranoturna, habilitaPeriodo01, habilitaPeriodo02
        , dias_cafe_1, dias_cafe_2, dias_cafe_3, dias_cafe_4, dias_cafe_5, dias_cafe_6, dias_cafe_7
        , marcaCargaHorariaMistaHorario, intervaloAutomatico, Preassinaladas1, Preassinaladas2, Preassinaladas3
        , consideraradicionalnoturnointerv, momentoPreAssinalado;

        private short? marcaCargaHorariaMistaHD, bCarregar, flagFolga, diaDsr, HabilitaInItinere;
        bool flagNeutro, DescontarAtrasoInItinere, DescontarFaltaInItinere, diaPossuiInItinere, pontoPorExcecao;
        decimal percentualDentroJornadaInItinere, percentualForaJornadaInItinere;

        #endregion

        #region Atributos Afastamento

        int? idAfastamentoFunc, idAfastamentoDep, idAfastamentoEmp, idAfastamentoCont
        , idOcorrenciaFunc, idOcorrenciaDep, idOcorrenciaEmp, idOcorrenciaCont;

        short? abonadoFunc, abonadoDep, abonadoEmp, abonadoCont, SemCalculoFunc, SemCalculoDep, SemCalculoEmp, SemCalculoCont, SemAbonoFunc, SemAbonoDep, SemAbonoEmp, SemAbonoCont, contabilizarjornadaFunc, contabilizarjornadaDep, contabilizarjornadaCont, contabilizarjornadaEmp;

        string horaiFunc, horaiDep, horaiEmp, horaiCont, horafFunc, horafDep, horafEmp, horafCont;

        int idOcorrencia;
        short abonado, semCalculoAfastamento, semAbonoAfastamento, contabilizarjornada;
        string horai, horaf;

        #endregion

        #region Outros Atributos

        private Modelo.JornadaAlternativa objJornadaAlternativa = null;

        private int? idCompensado, idFeriado, idJornadaAlternativa
        , idMudancaHorario, idBancoHoras;
        private bool feriadoParcial;
        private int? tipo;
        private int identificacao, idFechamentoBH, idFechamentoPonto;
        private List<int> lIdsFuncionarios;
        private DateTime datai, dataf;
        private int totalizaCargaHoraria, feriadoParcialInicioMin, feriadoParcialFimMin;
        private string ConnectionString, feriadoParcialInicio, feriadoParcialFim;
        private Modelo.Cw_Usuario UsuarioLogado;
        int HoraExtraDiurnaDiaAnterior;
        int HoraExtraNoturnaDiaAnterior;
        int HoraFaltaDiurnaDiaAnterior;
        int HoraFaltaNoturnaDiaAnterior;
        bool feriadoProximoDia = false;
        int idaInItinere = 0;
        int voltaInItine = 0;
        bool batidasValidasParaIntinere = true;

        private Dictionary<int, DataTable> diasBancoHorasAuxCalculo = new Dictionary<int, DataTable>();
        #endregion


        #endregion

        #region Construtores

        /// <summary>
        /// Construtor da Classe
        /// </summary>
        /// <param name="pTipo">null = geral; 0 = empresa; 1 = departamento; 2 = funcionário; 3 = função; 4 = horário; 6 = contrato</param>
        /// <param name="pIndentificacao">Identificação do Tipo</param>
        /// <param name="pDataI">Data inicial das marcações</param>
        /// <param name="pDataF">Data final das marcações</param>
        /// WNO
        public CalculaMarcacao(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF, Modelo.ProgressBar pProgressBar, bool pPegaExcluidos, string connString, Modelo.Cw_Usuario usuarioLogado, bool recalculaBHFechado)
        {
            tipo = pTipo;
            identificacao = pIdentificacao;
            pProgressBar = SetaParametros(pDataI, pDataF, pProgressBar, connString, usuarioLogado, pTipo, new List<int>() { pIdentificacao });

            Marcacoes = dalCalculaMarcacao.GetMarcacoesCalculo(pTipo, pIdentificacao, pDataI, pDataF, false, pPegaExcluidos, recalculaBHFechado);
            bllHorarioDinamico = new BLL.HorarioDinamico(ConnectionString, UsuarioLogado);
            if (bllHorarioDinamico.GerarHorariosDetalhesAPartirMarcacoes(Marcacoes))
            {
                Marcacoes = dalCalculaMarcacao.GetMarcacoesCalculo(pTipo, pIdentificacao, pDataI, pDataF, false, pPegaExcluidos, recalculaBHFechado);
            }

            if (pTipo == null)
            {
                tratamentomarcacaoList = bllBilhetesImp.GetImportadosPeriodo(5, identificacao, pDataI, pDataF);
            }
            else
            {
                tratamentomarcacaoList = bllBilhetesImp.GetImportadosPeriodo(tipo.Value, identificacao, pDataI, pDataF);
            }
            List<int> idsBH = Marcacoes.AsEnumerable().Where(w => w.Field<int?>("idbancohoras").HasValue).Select(s => s.Field<int>("idbancohoras")).Distinct().ToList();
            bancoHorasList = bllBancoHoras.GetHashIdObjeto(pDataI, pDataF, idsBH);
        }

        public CalculaMarcacao(List<int> idsFuncionarios, DateTime pDataI, DateTime pDataF, Modelo.ProgressBar pProgressBar, bool pPegaExcluidos, string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            pProgressBar = SetaParametros(pDataI, pDataF, pProgressBar, connString, usuarioLogado, 2, idsFuncionarios);
            lIdsFuncionarios = idsFuncionarios;
            Marcacoes = dalCalculaMarcacao.GetMarcacoesCalculo(idsFuncionarios, pDataI, pDataF, false, pPegaExcluidos);
            bllHorarioDinamico = new BLL.HorarioDinamico(ConnectionString, UsuarioLogado);
            if (bllHorarioDinamico.GerarHorariosDetalhesAPartirMarcacoes(Marcacoes))
            {
                Marcacoes = dalCalculaMarcacao.GetMarcacoesCalculo(idsFuncionarios, pDataI, pDataF, false, pPegaExcluidos);
            }
            tratamentomarcacaoList = bllBilhetesImp.GetImportadosPeriodo(idsFuncionarios, pDataI, pDataF, false);
            List<int> idsBH = Marcacoes.AsEnumerable().Where(w => w.Field<int?>("idbancohoras").HasValue).Select(s => s.Field<int>("idbancohoras")).Distinct().ToList();
            bancoHorasList = bllBancoHoras.GetHashIdObjeto(pDataI, pDataF, idsBH);
        }

        public CalculaMarcacao(DataTable marcacoes, List<Modelo.BilhetesImp> tratamentomarcacaoList, Hashtable bancoHorasList, Hashtable jornadaAlternativaList, List<Modelo.FechamentoBHD> fechamentoBHDList,
            Hashtable ocorrenciaList,
            List<Modelo.Compensacao> compensacaoList,
            List<PxyJornadaSubstituirCalculo> pxyJornadaSubstituirCalculo,
            Modelo.ProgressBar pProgressBar, string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            UsuarioLogado = usuarioLogado;
            ConnectionString = connString;
            if (marcacoes.Rows.Count > 0)
            {
                ObjProgressBar = pProgressBar;
                VerificaProgressNula();

                datai = marcacoes.AsEnumerable().Min(s => s.Field<DateTime>("data"));
                dataf = marcacoes.AsEnumerable().Max(s => s.Field<DateTime>("data"));


                lIdsFuncionarios = marcacoes.AsEnumerable().Select(s => s.Field<int>("idfuncionario")).Distinct().ToList();

                dalCalculaMarcacao = new DAL.SQL.CalculaMarcacao(new DataBase(ConnectionString));
                dalCalculaMarcacao.UsuarioLogado = UsuarioLogado;
                bllBancoHoras = new BLL.BancoHoras(ConnectionString, usuarioLogado);
                bllCompensacao = new BLL.Compensacao(ConnectionString, usuarioLogado);
                bllBilhetesImp = new BLL.BilhetesImp(ConnectionString, usuarioLogado);
                bllInclusaoBanco = new BLL.InclusaoBanco(ConnectionString, usuarioLogado);
                bllMarcacao = new BLL.Marcacao(ConnectionString, usuarioLogado);

                this.jornadaAlternativaList = jornadaAlternativaList;
                this.fechamentoBHDList = fechamentoBHDList;
                this.ocorrenciaList = ocorrenciaList;
                this.compensacaoList = compensacaoList;
                this.Marcacoes = marcacoes;
                this.tratamentomarcacaoList = tratamentomarcacaoList;
                this.bancoHorasList = bancoHorasList;
                this.pxyJornadaSubstituirCalculos = pxyJornadaSubstituirCalculo;
            }
        }

        private Modelo.ProgressBar SetaParametros(DateTime pDataI, DateTime pDataF, Modelo.ProgressBar pProgressBar, string connString, Modelo.Cw_Usuario usuarioLogado, int? pTipo, List<int> pIdentificacoes)
        {
            datai = pDataI;
            dataf = pDataF;

            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
                dalCalculaMarcacao = new DAL.SQL.CalculaMarcacao(new DataBase(ConnectionString));
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
                dalCalculaMarcacao = new DAL.SQL.CalculaMarcacao(new DataBase(ConnectionString));
            }
            UsuarioLogado = usuarioLogado;
            bllBancoHoras = new BLL.BancoHoras(ConnectionString, usuarioLogado);
            bllCompensacao = new BLL.Compensacao(ConnectionString, usuarioLogado);
            bllJornadaAlternativa = new BLL.JornadaAlternativa(ConnectionString, usuarioLogado);
            bllBilhetesImp = new BLL.BilhetesImp(ConnectionString, usuarioLogado);
            bllParametros = new BLL.Parametros(ConnectionString, usuarioLogado);
            bllInclusaoBanco = new BLL.InclusaoBanco(ConnectionString, usuarioLogado);
            bllFechamentoBHD = new BLL.FechamentoBHD(ConnectionString, usuarioLogado);
            bllOcorrencia = new BLL.Ocorrencia(ConnectionString, usuarioLogado);
            bllFuncionario = new BLL.Funcionario(ConnectionString, usuarioLogado);
            bllMarcacao = new BLL.Marcacao(ConnectionString, usuarioLogado);
            dalCalculaMarcacao.UsuarioLogado = UsuarioLogado;
            ObjProgressBar = pProgressBar;
            VerificaProgressNula();
            jornadaAlternativaList = bllJornadaAlternativa.GetHashIdObjeto(pDataI, pDataF, pTipo, pIdentificacoes);
            fechamentoBHDList = bllFechamentoBHD.getPorPeriodo(pDataI, pDataF, pTipo, pIdentificacoes);
            ocorrenciaList = bllOcorrencia.GetHashIdDescricao();
            compensacaoList = bllCompensacao.GetPeriodo(pDataI, pDataF, pTipo, pIdentificacoes);
            JornadaSubstituir bllJornadaSubstituir = new BLL.JornadaSubstituir(ConnectionString, usuarioLogado);
            pxyJornadaSubstituirCalculos = bllJornadaSubstituir.GetPxyJornadaSubstituirCalculo(pDataI, pDataF, pIdentificacoes);
            ObjProgressBar.setaMensagem("Carregando os dados...");
            return pProgressBar;
        }

        public CalculaMarcacao(int? pTipo, int pIdentificacao, DateTime pDataI, DateTime pDataF, Modelo.ProgressBar pProgressBar, bool pPegaExcluidos, string connString, bool bWebApi, bool recalculaBHFechado)
        {
            tipo = pTipo;
            identificacao = pIdentificacao;
            datai = pDataI;
            dataf = pDataF;

            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
                dalCalculaMarcacao = new DAL.SQL.CalculaMarcacao(new DataBase(ConnectionString));
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
                dalCalculaMarcacao = new DAL.SQL.CalculaMarcacao(new DataBase(ConnectionString));
            }

            if (UsuarioLogado == null)
            {
                UsuarioLogado = new Modelo.Cw_Usuario() { Login = "cwork", Nome = "cwork" };
            }

            bllBancoHoras = new BLL.BancoHoras(ConnectionString, UsuarioLogado);
            bllCompensacao = new BLL.Compensacao(ConnectionString, UsuarioLogado);
            bllJornadaAlternativa = new BLL.JornadaAlternativa(ConnectionString, UsuarioLogado);
            bllBilhetesImp = new BLL.BilhetesImp(ConnectionString, UsuarioLogado);
            bllParametros = new BLL.Parametros(ConnectionString, UsuarioLogado);
            bllInclusaoBanco = new BLL.InclusaoBanco(ConnectionString, UsuarioLogado);
            bllFechamentoBHD = new BLL.FechamentoBHD(ConnectionString, UsuarioLogado);
            bllOcorrencia = new BLL.Ocorrencia(ConnectionString, UsuarioLogado);
            bllFuncionario = new BLL.Funcionario(ConnectionString, UsuarioLogado);
            bllMarcacao = new BLL.Marcacao(ConnectionString, UsuarioLogado);

            dalCalculaMarcacao.UsuarioLogado = UsuarioLogado;
            ObjProgressBar = SetaParametros(pDataI, pDataF, pProgressBar, connString, UsuarioLogado, pTipo, new List<int>() { identificacao });
            ObjProgressBar.setaMensagem("Carregando os dados...");

            bllHorarioDinamico = new BLL.HorarioDinamico(ConnectionString, UsuarioLogado);
            if (bWebApi)
            {
                Marcacoes = dalCalculaMarcacao.GetMarcacoesCalculoWebApi(pTipo, pIdentificacao, pDataI, pDataF, false, pPegaExcluidos);
                if (bllHorarioDinamico.GerarHorariosDetalhesAPartirMarcacoes(Marcacoes))
                {
                    Marcacoes = dalCalculaMarcacao.GetMarcacoesCalculoWebApi(pTipo, pIdentificacao, pDataI, pDataF, false, pPegaExcluidos);
                }
            }
            else
            {
                Marcacoes = dalCalculaMarcacao.GetMarcacoesCalculo(pTipo, pIdentificacao, pDataI, pDataF, false, pPegaExcluidos, recalculaBHFechado);
                if (bllHorarioDinamico.GerarHorariosDetalhesAPartirMarcacoes(Marcacoes))
                {
                    Marcacoes = dalCalculaMarcacao.GetMarcacoesCalculo(pTipo, pIdentificacao, pDataI, pDataF, false, pPegaExcluidos, recalculaBHFechado);
                }
            }

            if (pTipo == null)
                tratamentomarcacaoList = bllBilhetesImp.GetImportadosPeriodo(5, identificacao, pDataI, pDataF);
            else
                tratamentomarcacaoList = bllBilhetesImp.GetImportadosPeriodo(tipo.Value, identificacao, pDataI, pDataF);
            List<int> idsBH = Marcacoes.AsEnumerable().Where(w => w.Field<int?>("idbancohoras").HasValue).Select(s => s.Field<int>("idbancohoras")).Distinct().ToList();
            bancoHorasList = bllBancoHoras.GetHashIdObjeto(pDataI, pDataF, idsBH);
        }

        #endregion

        #region Métodos Principais

        /// <summary>
        /// Método que recebe um datatable de marcações e faz o recalculo
        /// </summary>
        /// <param name="pMarcacoes">Data table de marcações</param>
        /// WNO
        public void CalculaMarcacoes()
        {
            try
            {
                AuxCalculaMarcacoes();
                this.CalculaDSR(false, true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void AuxCalculaMarcacoesWebApi(string login)
        {
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();

            IList<Modelo.Marcacao> marcacoes = new List<Modelo.Marcacao>();

            ObjProgressBar.setaMinMaxPB(0, Marcacoes.Rows.Count);
            ObjProgressBar.setaValorPB(0);
            ObjProgressBar.setaMensagem("Calculando Marcações...");

            foreach (DataRow marc in Marcacoes.Rows)
            {
                ObjProgressBar.setaMensagem("Calculando Marcações...[" + marc["nomeFuncionario"].ToString() + "] " + Convert.ToDateTime(marc["data"]).ToShortDateString());
                this.RealizaCalculo(marc, marcacoes, bilhetes);
                ObjProgressBar.incrementaPB(1);
            }
            ObjProgressBar.setaValorPB(0);
            ObjProgressBar.setaMensagem("Salvando Marcações...");

            dalCalculaMarcacao.PersistirDadosWebApi(marcacoes, bilhetes, login);
        }

        public void AuxCalculaMarcacoes()
        {
            if (Marcacoes.Rows.Count > 0)
            {
                ObjProgressBar.setaMinMaxPB(0, Marcacoes.Rows.Count);
                ObjProgressBar.setaValorPB(0);
                ObjProgressBar.setaMensagem("Calculando Marcações...");

                var MarcsFuncs = Marcacoes.AsEnumerable().GroupBy(row => new
                {
                    idfuncionario = row.Field<int>("idfuncionario"),
                    dscodigo = row.Field<string>("dscodigo"),
                    nomeFunc = row.Field<string>("nomeFuncionario"),
                }).OrderBy(o => o.Key.nomeFunc);
                List<LoteMarcacaoProcessar> lote = new List<LoteMarcacaoProcessar>();
                foreach (var group in MarcsFuncs)
                {
                    LoteMarcacaoProcessar l = new LoteMarcacaoProcessar();
                    l.IdFuncionario = group.Key.idfuncionario;
                    l.NomeFuncionario = group.Key.nomeFunc;
                    l.DtMarcacoes = group.CopyToDataTable();
                    l.Marcacoes = new List<Modelo.Marcacao>();
                    l.Bilhetes = new List<Modelo.BilhetesImp>();
                    lote.Add(l);
                }

                int qtdCalcular = 0;
                foreach (var group in lote)
                {
                    string nomeFuncionario = "";
                    int contador = 0;
                    qtdCalcular++;
                    foreach (DataRow marc in group.DtMarcacoes.Rows)
                    {
                        nomeFuncionario = marc["nomeFuncionario"].ToString();
                        contador++;
                        ObjProgressBar.setaMensagem(qtdCalcular + "/" + lote.Count() + " (" + nomeFuncionario + " - Calculando " + contador + " de " + group.DtMarcacoes.Rows.Count + " Marcações - " + Convert.ToDateTime(marc["data"]).ToShortDateString() + ")");
                        this.RealizaCalculo(marc, group.Marcacoes, group.Bilhetes);
                        ObjProgressBar.incrementaPB(1);
                    }
                }

                List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
                List<Modelo.Marcacao> marcacoes = new List<Modelo.Marcacao>();
                int i = 0;
                int totalInteracoes = lote.Count();
                ObjProgressBar.setaMinMaxPB(0, totalInteracoes);
                foreach (var func in lote)
                {
                    i++;
                    ObjProgressBar.setaValorPB(i);
                    ObjProgressBar.setaMensagem(i + "/" + totalInteracoes + " (" + func.NomeFuncionario + " - Salvando " + (func.Marcacoes.Count() + func.Bilhetes.Count()) + " Registros)");
                    bilhetes.AddRange(func.Bilhetes);
                    marcacoes.AddRange(func.Marcacoes);
                    if (i == lote.Count() || (bilhetes.Count() + marcacoes.Count()) > 300)
                    {
                        dalCalculaMarcacao.PersistirDados(marcacoes, bilhetes);
                        bilhetes = new List<Modelo.BilhetesImp>();
                        marcacoes = new List<Modelo.Marcacao>();
                    }
                }

                ObjProgressBar.setaMensagem("Classificando horas extras");
                PreClassificarHorasExtras(lote);
            }
        }

        public void PreClassificarHorasExtras(List<LoteMarcacaoProcessar> lote)
        {
            if (Marcacoes != null && Marcacoes.Rows.Count > 0)
            {
                BLL.ClassificacaoHorasExtras bllClassHE = new ClassificacaoHorasExtras(ConnectionString, UsuarioLogado);
                List<int> idsFuncs = lote.Where(w => w.Marcacoes.Count > 0).Select(s => s.IdFuncionario).Distinct().ToList();
                IEnumerable<IEnumerable<int>> idsFuncsParts = idsFuncs.Section(50);
                DateTime dataIni = Marcacoes.AsEnumerable().Min(row => row.Field<DateTime>("data"));
                DateTime dataFin = Marcacoes.AsEnumerable().Max(row => row.Field<DateTime>("data"));
                foreach (IEnumerable<int> idsFuncsLote in idsFuncsParts)
                {
                    bllClassHE.ExcluirClassificacoesHEPreClassificadas(idsFuncsLote.ToList(), dataIni, dataFin);
                    bllClassHE.PreClassificarHorasExtras(idsFuncsLote.ToList(), dataIni, dataFin);
                }
            }
        }

        public LoteMarcacaoProcessar CalcularMarcacoes()
        {
            LoteMarcacaoProcessar lote = new LoteMarcacaoProcessar();
            lote.Marcacoes = new List<Modelo.Marcacao>();
            lote.Bilhetes = new List<Modelo.BilhetesImp>();
            ObjProgressBar.setaMinMaxPB(0, Marcacoes.Rows.Count);
            ObjProgressBar.setaValorPB(0);
            ObjProgressBar.setaMensagem("Calculando Marcações...");
            foreach (DataRow marc in Marcacoes.Rows)
            {
                this.RealizaCalculo(marc, lote.Marcacoes, lote.Bilhetes);
            }

            return lote;
        }

        public void SalvarMarcacoesCalculadas(List<LoteMarcacaoProcessar> lote, Modelo.ProgressBar pb, string mensagem)
        {
            List<Modelo.BilhetesImp> bilhetes = new List<Modelo.BilhetesImp>();
            List<Modelo.Marcacao> marcacoes = new List<Modelo.Marcacao>();
            int i = 0;
            int total = lote.Select(s => s.Marcacoes.Count).ToList().Sum() + lote.Select(s => s.Bilhetes.Count).ToList().Sum();
            if (total > 0)
            {
                int stepCalc = 0;
                foreach (var func in lote)
                {
                    i++;
                    if (func.Bilhetes != null)
                        bilhetes.AddRange(func.Bilhetes);
                    if (func.Marcacoes != null)
                        marcacoes.AddRange(func.Marcacoes);
                    if (i == lote.Count() || (bilhetes.Count() + marcacoes.Count()) > 3000)
                    {
                        stepCalc += marcacoes.Count() + bilhetes.Count();
                        if (pb.setaMensagem != null)
                        {
                            pb.setaMensagem(String.Format(mensagem + " ( Quantidade {0}/{1})", stepCalc, total));
                        }
                        dalCalculaMarcacao.PersistirDados(marcacoes, bilhetes);
                        bilhetes = new List<Modelo.BilhetesImp>();
                        marcacoes = new List<Modelo.Marcacao>();
                    }
                }
            }
        }

        /// <summary>
        /// Método que recebe um DataRow de marcação e realiza o calculo
        /// </summary>
        /// <param name="pMarcacao">DataRow da marcação</param>
        /// <param name="pUpdates">lista onde será adicionado o comando para salvar a marcação</param>
        /// WNO
        private void RealizaCalculo(DataRow pMarcacao, ICollection<Modelo.Marcacao> pMarcacoes, ICollection<Modelo.BilhetesImp> pBilhetes)
        {
            this.SetaVariaveisAfastamento(pMarcacao);
            this.SetaVariaveisHorario(pMarcacao);
            this.SetaVariaveisMarcacao(pMarcacao);
            //Se estiver calculando e algum dos tratamentos estiver com o campo importado = 2, mudo ele para 1
            tratamentosMarcacao.Where(w => w.Importado == 2).ToList().ForEach(f => f.Importado = 1);
            //Guarda dados anterior
            List<Modelo.BilhetesImp> tratamentosMarcacaoAnt = new Modelo.BilhetesImp().Clone(tratamentosMarcacao);

            ProcessaPontoPorExcecao(tratamentosMarcacao, pMarcacao, pBilhetes);


            horasExtraDiurnaFeriadoMin = 0;
            horasExtraNoturnaFeriadoMin = 0;
            feriadoProximoDia = false;
            PreencheMarcacao(pMarcacao);
            Modelo.Marcacao objMarcacaoAnt = new Modelo.Marcacao().Clone(objMarcacao);
            //Verifica Parametro Estender Periodo Noturno
            bllParametros = new BLL.Parametros(ConnectionString, UsuarioLogado);
            if (bFlgEstenderPeriodoNoturno)
            {
                int[] entrada = new int[4] { entrada_1Min, entrada_2Min, entrada_3Min, entrada_4Min };
                int[] saida = new int[4] { saida_1Min, saida_2Min, saida_3Min, saida_4Min };
                int trabDiurna = 0, trabNoturna = 0;
                BLL.CalculoHoras.QtdHorasDiurnaNoturna(entrada, saida, inicioAdNoturno, fimAdNoturno, ref trabDiurna, ref trabNoturna);
                if (trabNoturna > 0)
                {
                    List<int> saidasPrevistas = new List<int>() { saida_1MinHD, saida_2MinHD, saida_3MinHD, saida_4MinHD };
                    int saidaPrevista = saidasPrevistas.Where(w => w >= 0).LastOrDefault();
                    int saidaRealizada = Modelo.cwkFuncoes.ConvertHorasMinuto(tratamentosMarcacaoAnt.Where(w => w.Ent_sai == "S" && w.Ocorrencia != 'D').OrderBy(o => o.Posicao).Select(s => s.Mar_hora).LastOrDefault());

                    List<int> saidasParaFimAdNoturno = new List<int>() { saidaPrevista, saidaRealizada, fimAdNoturno };
                    int fimAdNoturnoCalc = saidasParaFimAdNoturno.OrderByDescending(o => o).FirstOrDefault();

                    //Se o fim do ad noturno for maior que 12:00 significa que não é para extender mais
                    if (fimAdNoturno < fimAdNoturnoCalc && fimAdNoturnoCalc > 720)
                    {
                        fimAdNoturno = fimAdNoturno;
                    }
                    else
                    {
                        fimAdNoturno = fimAdNoturnoCalc;
                    }
                }
            }

            legenda = BuscaLegenda();
            LegendasConcatenadas = BuscaLegendaConcatenada();
            CalculaHorasPrevistasDentroFeriado();
            if (idCompensado != 0)
            {
                legenda = "C";
                LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "C");
            }

            horasCompensarMin = 0;
            if (naoEntrarNaCompensacao == 0) //Somente entra na compensação se o funcionario não estiver marcado para nao entrar
            {
                List<Modelo.Compensacao> listaCompensacao = bllCompensacao.getListaCompensacao(data, idFuncionario, idDepartamento, idFuncao, idEmpresa, compensacaoList);
                foreach (Modelo.Compensacao comp in listaCompensacao)
                {
                    if (comp.getDias()[diaInt - 1] == 1)
                    {
                        //Hora a ser compensada
                        horasCompensarMin = Modelo.cwkFuncoes.ConvertHorasMinuto(comp.getTotalHorasSerCompensadas()[diaInt - 1]);
                    }
                }
            }

            ConsiderarHEFeriadoPHoraNoturna(ref feriadoProximoDia);

            //Verifica se o funcionário está demitido
            if ((dataDemissao != null && dataDemissao < data) || (dataInativacao != null && dataInativacao <= data))
            {
                idFuncionario = Convert.ToInt32(pMarcacao["idfuncionario"]);
                if (dataDemissao != null && dataDemissao < data)
                {
                    this.PreencheFuncionarioDemitido();
                }
                else
                {
                    this.PreencheFuncionarioInativo();
                }

                if (VerificaAlteracaoMarcao(pMarcacao, objMarcacaoAnt))
                {
                    objMarcacao.Acao = Modelo.Acao.Alterar;
                    pMarcacoes.Add(objMarcacao);
                }
                return;
            }

            //Verifica se a data é anterior a Data de admissão do funcionário
            if (dataAdmissao != null && data < dataAdmissao)
            {
                idFuncionario = Convert.ToInt32(pMarcacao["idfuncionario"]);
                this.PreencheFuncionarioNaoAdmitido();
                if (VerificaAlteracaoMarcao(pMarcacao, objMarcacaoAnt))
                {
                    objMarcacao.Acao = Modelo.Acao.Alterar;
                    pMarcacoes.Add(objMarcacao);
                }
                return;
            }

            this.SetaDsr();
            //Busca Jornada Alternativa
            this.SetaJornadaAlternativa();

            //Chamar método que faz a marcação automatica
            bool temIntervaloAuto = this.IntervaloAutomatico(pMarcacao);

            int[] Entrada = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
            int[] Saida = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
            this.GetEntradasSaidasValidas(ref Entrada, ref Saida);

            this.CalculaHorasTrabalhadas(Entrada, Saida);
            this.CalculaTotalHorasTrabalhadas(Entrada, Saida);
            this.CalculaHorasInItinere(Entrada, Saida);
            this.CalculaAdicionalNoturnoIncluindoIntervalo(Entrada, Saida);

            //Localiza a ocorrencia verificando se tem horas para abonar
            int abono = 0;
            bool semcalc = false;
            bool semAbono = false;
            string abonoD = "--:--";
            string abonoN = "--:--";
            AdicionalNoturno = 0;
            this.LocalizaOcorrencia(ref ocorrencia, ref abono, ref semcalc, ref abonoD, ref abonoN, ref semAbono);

            int horarioD = 0;
            int horarioN = 0;
            int horarioM = 0;
            int bCargaMista = 0;
            int[] HoraEntrada = new int[] { -1, -1, -1, -1 };
            int[] HoraSaida = new int[] { -1, -1, -1, -1 };
            int adicionalNoturno = 0;//Diferença entre a hora noturna sem calculo e a com o adicinal noturno
            if (this.VerificaJornadaAlternativa(ref HoraEntrada, ref HoraSaida, ref horarioD, ref horarioN, ref horarioM, ref bCafe, ref bCargaMista))
            {
                horarioD = Modelo.cwkFuncoes.ConvertHorasMinuto(objJornadaAlternativa.TotalTrabalhadaDiurna);
                horarioN = Modelo.cwkFuncoes.ConvertHorasMinuto(objJornadaAlternativa.TotalTrabalhadaNoturna);
                horarioM = Modelo.cwkFuncoes.ConvertHorasMinuto(objJornadaAlternativa.TotalMista);
                consideraAdHTrabalhadas = Convert.ToInt16(objJornadaAlternativa.CalculoAdicionalNoturno);
                conversaoHoranoturna = Convert.ToInt16(objJornadaAlternativa.ConversaoHoraNoturna);
            }
            else
            {
                horarioD = Modelo.cwkFuncoes.ConvertHorasMinuto(totalTrabalhadaDiurna);
                horarioN = Modelo.cwkFuncoes.ConvertHorasMinuto(totalTrabalhadaNoturna);
                horarioM = Modelo.cwkFuncoes.ConvertHorasMinuto(cargaHorariaMista);
                SetaVetoresHorarioDetalhe(ref HoraEntrada, ref HoraSaida, ref horarioD, ref horarioN, ref horarioM, ref bCafe, ref bCargaMista);
            }

            if (consideraAdHTrabalhadas == 1)
            {
                AdicionalNoturno = horasAdicionalNoturno;
                if (horasAdicionalNoturno != 0 && conversaoHoranoturna == 1)
                {
                    horasAdicionalNoturno = BLL.CalculoHoras.HoraNoturna(horasAdicionalNoturno, reducaohoranoturna);
                    adicionalNoturno = horasAdicionalNoturno;
                    horasTrabalhadasNoturnasMin = BLL.CalculoHoras.HoraNoturna(horasTrabalhadasNoturnasMin, reducaohoranoturna);
                    AdicionalNoturno = horasAdicionalNoturno;
                    adicionalNoturno = horasAdicionalNoturno - adicionalNoturno;
                }
            }

            if (!this.CalculaHoraExtraFalta(abonoD, abonoN, adicionalNoturno, feriadoProximoDia))
            {
                horasExtraNoturnaMin = 0;
                horasExtrasDiurnaMin = 0;
                horasFaltaNoturnaMin = 0;
                horasFaltasMin = 0;
                horasTrabalhadasMin = 0;
                horasTrabalhadasNoturnasMin = 0;
                AdicionalNoturno = 0;
            }

            //Atribui o valor das horas extras noturnas para a variavel de exportação das horas que forem para o BH
            expHorasExtraNot = Modelo.cwkFuncoes.ConvertMinutosHora(horasExtraNoturnaMin);
            int expHorasExtraNotMin = horasExtraNoturnaMin;
            //Copia o valor da variavel 'expHorasExtraNot' em minutos, para chamada da funcao 'CalculaHoraExtraInterjornada'.
            if (consideraAdHTrabalhadas == 0)
            {
                if (horasAdicionalNoturno != 0)
                {
                    AdicionalNoturno = horasAdicionalNoturno;
                    if (conversaoHoranoturna == 1)
                    {
                        AdicionalNoturno = BLL.CalculoHoras.HoraNoturna(AdicionalNoturno, reducaohoranoturna);
                    }
                }

                //Calcula o adcional Noturno
                if (consideraAdHTrabalhadas == 1 && horasAdicionalNoturno != 0 && conversaoHoranoturna == 1)
                {
                    horasTrabalhadasNoturnasMin = BLL.CalculoHoras.HoraNoturna(horasTrabalhadasNoturnasMin, reducaohoranoturna);
                }
                if (consideraAdHTrabalhadas == 1 && horasExtraNoturnaMin != 0 && conversaoHoranoturna == 1)
                {
                    horasExtraNoturnaMin = BLL.CalculoHoras.HoraNoturna(horasExtraNoturnaMin, reducaohoranoturna);
                }
                //Se for carga horaria mista, tem que tirar o valor das faltas também.
                if (marcaCargaHorariaMistaHD == 1)
                {
                    if (horasFaltaNoturnaMin != 0 && conversaoHoranoturna == 1)
                        horasFaltaNoturnaMin -= adicionalNoturno;
                }
            }

            if (folgaMarcacao == 1)
            {
                horasExtraNoturnaMin = horasExtraNoturnaMin + horasTrabalhadasNoturnasMin;
                horasExtrasDiurnaMin = horasExtrasDiurnaMin + horasTrabalhadasMin;
                horasFaltaNoturnaMin = 0;
                horasFaltasMin = 0;
                horasTrabalhadasMin = 0;
                horasTrabalhadasNoturnasMin = 0;
                ocorrencia = "";
            }
            else
            {
                RegraParaExtrasFeriado();

                RegrasParaDiurno(abono, semcalc, abonoD, horarioD, horarioN, horarioM, semAbono);
                RegrasParaNoturno(abono, semcalc, abonoN, horarioN, horarioD, semAbono);
                // Após aplicar os Abonos valida se ainda restou falta, caso não tenha restado falta remove a ocorrência de falta
                if (horasFaltasMin == 0 && horasFaltaNoturnaMin == 0 && ocorrencia.Contains("Falta"))
                    ocorrencia = ocorrencia.Replace("Falta", "").Trim();

            }
            this.LocalizaOcorrencia(ref ocorrencia, ref abono, ref semcalc, ref abonoD, ref abonoN, ref semAbono);
            this.TestaMarcacoesCorretas();
            setaOcorrenciaDemitido();
            //Caso existam vários banco para o mesmo dia, retorna verdadeiro no mais prioritário
            if (!this.CalculaBancoHoras(idFuncionario, pMarcacoes))
                return;

            if (idJornadaSubstituir != null)
            {
                legenda = "S";
                LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "S");
            }

            if (objJornadaAlternativa != null)
            {
                if (legenda != "A")
                {
                    if (legenda != "F")
                        legenda = "J";
                    LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "J");
                    //Colocado para atualziar a ocorrencia somente quando não for banco de horas
                    //CRNC - 18/01/2010
                    if ((bancoHorasCre == "---:--") && (bancoHorasDeb == "---:--") && (dataDemissao != data) && (idFechamentoBH == 0 && !ocorrencia.Contains("H. Pagas")))
                    {
                        ocorrencia = "Jornada Alternativa";
                    }
                    else if ((bancoHorasCre == "---:--") && (bancoHorasDeb == "---:--") && (dataDemissao == data))
                    {
                        setaOcorrenciaDemitido();
                    }
                }
            }

            if (idMudancaHorario != null)
            {
                legenda = "M";
                LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "M");
            }

            //Regra de cálculo de horas extras de feriado com jornada noturna
            horasExtrasDiurnaMin += horasExtraDiurnaFeriadoMin;
            horasExtraNoturnaMin += horasExtraNoturnaFeriadoMin;
            AdicionalNoturno += horasExtraNoturnaFeriadoMin;

            if (bSepararTrabalhadasNoturnaExtrasNoturna)
            {
                AdicionalNoturno -= horasExtraNoturnaMin;
            }


            List<Modelo.BilhetesImp> tratamentosDia = tratamentomarcacaoList.Where(x => x.Mar_data == data && x.DsCodigo == Convert.ToString(pMarcacao["dscodigo"])).ToList();
            List<Modelo.BilhetesImp> tratamentosDiaAnterior = tratamentomarcacaoList.Where(x => x.Mar_data == data.AddDays(-1) && x.DsCodigo == Convert.ToString(pMarcacao["dscodigo"])).ToList();
            List<Modelo.BilhetesImp> tratamentosDiaSeguinte = tratamentomarcacaoList.Where(x => x.Mar_data == data.AddDays(1) && x.DsCodigo == Convert.ToString(pMarcacao["dscodigo"])).ToList();
            Interjornada = CalculaInterjornada(tratamentosDia, tratamentosDiaAnterior);
            horaExtraInterjornada = CalculaHoraExtraInterjornada(tratamentosDia, tratamentosDiaAnterior, horasExtrasDiurnaMin + horasExtraNoturnaMin); //Para setar no dia seguinte

            if (VerificaAlteracaoMarcao(pMarcacao, objMarcacaoAnt))
            {
                objMarcacao.Acao = Modelo.Acao.Alterar;
                pMarcacoes.Add(objMarcacao);

                if (temIntervaloAuto && tratamentosMarcacao != null && tratamentosMarcacao.Count > 0)
                {
                    List<Modelo.BilhetesImp> bilhetesSalvar = new List<Modelo.BilhetesImp>();
                    foreach (var item in tratamentosMarcacao)
                    {
                        List<Modelo.BilhetesImp> bilIguaisAnt = tratamentosMarcacaoAnt.Where(e => item.BilheteIsEqual(e)).ToList();
                        List<Modelo.BilhetesImp> bilIguaisAtual = tratamentosMarcacao.Where(e => item.BilheteIsEqual(e)).ToList();
                        // Salva apenas os bilhetes que possuem alteração (não tem um alterior igual), ou se for PA salva os que não estão sendo incluídos e excluídos com o mesmo valor (dois iguais)
                        // Ou se é registro por excessão e foi assinalado para excluir.
                        if ((bilIguaisAnt.Count() == 0 || item.Relogio == "PA" && bilIguaisAtual.Count() <= 1) || (item.Relogio == "PE" && item.Acao == Acao.Excluir))
                        {
                            bilhetesSalvar.Add(item);
                        }
                    }
                    if (bilhetesSalvar != null && bilhetesSalvar.Count() > 0)
                    {
                        bilhetesSalvar.ForEach(s => pBilhetes.Add(s));
                    }
                }
            }
        }

        private void ProcessaPontoPorExcecao(List<Modelo.BilhetesImp> tratamentosMarcacao, DataRow pMarcacao, ICollection<Modelo.BilhetesImp> pBilhetes)
        {
            if ((TemAfastamento() || !pontoPorExcecao) && tratamentosMarcacao.Where(w => w.Relogio == "PE").Any())
            {
                tratamentosMarcacao = tratamentomarcacaoList.Where(t => t.Mar_data == data && t.DsCodigo == dscodigo).ToList();
                tratamentosMarcacao.Where(w => w.Relogio == "PE").ToList().ForEach(f => f.Acao = Acao.Excluir);
                tratamentosMarcacao = tratamentosMarcacao.OrderBy(o => o.Posicao).ThenBy(o => o.Ent_sai).ToList();
                string ent_sai = "E";
                int posicao = 1;
                //for (int i = 0; i < tratamentosMarcacao.Count; i++)
                //{
                //    var tratamento = tratamentosMarcacao[i];
                //    if (tratamento.Acao != Acao.Excluir)
                //    {
                //        tratamento.Ent_sai = ent_sai;
                //        tratamento.Posicao = posicao;
                //        ent_sai = ent_sai == "E" ? "S" : "E";
                //        posicao = ent_sai == "E" ? posicao++ : posicao;
                //    }
                //}
                for (int i = 1; i < 9; i++)
                {
                    var entrada = tratamentosMarcacao.Where(w => w.Ent_sai == "E" && w.Posicao == i && w.Acao != Acao.Excluir).FirstOrDefault();
                    if (entrada != null)
                    {
                        pMarcacao[$"entrada_{i}min"] = entrada.Hora.ConvertHorasMinuto();
                        pMarcacao[$"entrada_{i}"] = entrada.Hora;
                    }
                    else
                    {
                        pMarcacao[$"entrada_{i}min"] = -1;
                        pMarcacao[$"entrada_{i}"] = "--:--";
                    }

                    var saida = tratamentosMarcacao.Where(w => w.Ent_sai == "S" && w.Posicao == i && w.Acao != Acao.Excluir).FirstOrDefault();
                    if (saida != null)
                    {
                        pMarcacao[$"saida_{i}min"] = saida.Hora.ConvertHorasMinuto();
                        pMarcacao[$"saida_{i}"] = saida.Hora;
                    }
                    else
                    {
                        pMarcacao[$"saida_{i}min"] = -1;
                        pMarcacao[$"saida_{i}"] = "--:--";
                    }
                }
                this.SetaVariaveisMarcacao(pMarcacao);
            }


            //Ponto por excecao
            if (pontoPorExcecao && tratamentosMarcacao.Where(w => w.Relogio == "PE").Any())
            {
                //Verifica se tem feriado
                if (pMarcacao["idferiado"] != DBNull.Value)
                {
                    int idFeriado = pMarcacao["idferiado"] == DBNull.Value ? 0 : (int)pMarcacao["idferiado"];
                    if (idFeriado > 0)
                    {
                        tratamentosMarcacao.Where(w => w.Relogio == "PE").ToList().ForEach(c => c.Acao = Acao.Excluir);
                    }

                }

                //Verifica se tem algum ponto manual a ser inserido
                if (tratamentosMarcacao.Where(w => w.Relogio == "MA" && w.Acao == Acao.Desconhecida).Any())
                {
                    foreach (var objBilheteManual in tratamentosMarcacao.Where(w => w.Relogio == "MA" && w.Acao == Acao.Desconhecida))
                    {

                        int minutosManual = objBilheteManual.Hora.ConvertHorasMinuto();

                        int diferencaminutoPontoPosExcecaoAnterior = 0;

                        Modelo.BilhetesImp bilhetesImp = new Modelo.BilhetesImp();


                        foreach (var item in tratamentosMarcacao.Where(w => w.Relogio == "PE"))
                        {
                            int minutoPontoPosExcecao = 0;
                            int diferencaminutos = 0;

                            minutoPontoPosExcecao = item.Hora.ConvertHorasMinuto();

                            if (minutoPontoPosExcecao > minutosManual)
                                diferencaminutos = minutoPontoPosExcecao - minutosManual;
                            else
                                diferencaminutos = minutosManual - minutoPontoPosExcecao;

                            if (diferencaminutos <= 120 && (diferencaminutoPontoPosExcecaoAnterior == 0 || diferencaminutos < diferencaminutoPontoPosExcecaoAnterior))
                            {
                                diferencaminutoPontoPosExcecaoAnterior = diferencaminutos;

                                bilhetesImp = item;
                            }
                        }
                        //Caso tenha pega o cronologicamente mais perto
                        bilhetesImp.Acao = Acao.Excluir;
                    }

                }
            }
            BilhetesImp.AjustarPosicaoBilhetes(tratamentosMarcacao);

        }

        private void CalculaHorasPrevistasDentroFeriado()
        {
            int[] entrada = new int[4] { entrada_1MinHD, entrada_2MinHD, entrada_3MinHD, entrada_4MinHD };
            int[] saida = new int[4] { saida_1MinHD, saida_2MinHD, saida_3MinHD, saida_4MinHD };
            CalculaHorasTrabalhadasNoFeriado(entrada, saida, inicioAdNoturno, fimAdNoturno, feriadoParcialInicioMin, feriadoParcialFimMin, out horasPrevistasDentroFeriadoParcialDiurna, out horasPrevistasDentroFeriadoParcialNoturna);
        }

        private bool VerificaAlteracaoMarcao(DataRow pMarcacao, Modelo.Marcacao objMarcacaoAnt)
        {
            PreencheMarcacao(pMarcacao);
            var changes = objMarcacao.GetChanges(objMarcacaoAnt);

            return (changes != null && changes.Where(w => w.PropertyName != "BilhetesMarcacao").Count() > 0);
        }

        /// <summary>
        /// Altera o método de cálculo do adicional noturno
        /// Qualquer intervalo dentro do período de adicional noturno é considerado como hora trabalhada
        /// EX:
        ///     Início Adicional Noturno: 22:00
        ///     Final Adicional Noturno: 05:00
        /// EX1:
        ///     ENTRADAS - SAÍDAS
        ///     18:00    - 23:00
        ///     01:00    - 03:00
        ///     ADICIONAL NOTURNO: 05 horas
        /// EX2:
        ///     22:30    - 23:30
        ///     03:00    - 05:00
        ///     ADICIONAL NOTURNO: 06horas30minutos
        /// </summary>
        /// <param name="Entrada"></param>
        /// <param name="Saida"></param>
        private void CalculaAdicionalNoturnoIncluindoIntervalo(int[] Entrada, int[] Saida)
        {
            if (consideraradicionalnoturnointerv == 1)
            {
                int PrimeiraEntradaNoAdicionalNoturno = -1;
                int UltimaSaidaNoAdicionalNoturno = -1;
                bool HouveMudancaDeDia = false;
                int InicioAdicionalNoturno = inicioAdNoturno;
                int FimAdicionalNoturno = fimAdNoturno;
                // Verifica se houve troca de dia
                if (InicioAdicionalNoturno > FimAdicionalNoturno)
                {
                    FimAdicionalNoturno += 1440;
                    HouveMudancaDeDia = true;
                }
                for (int i = 0; i < Entrada.Length; i++)
                {
                    if (Entrada[i] != -1 && Saida[i] != -1)
                    {
                        // Pegando a marcação
                        int EntradaMin = Entrada[i];
                        int SaidaMin = Saida[i];
                        // Verifica se houve troca de dia na configuração de adicional noturno
                        if (i == 0)
                        {
                            // Se a marcação virou o dia
                            if (EntradaMin > SaidaMin)
                            {
                                SaidaMin += 1440;
                                HouveMudancaDeDia = true;
                            }
                        }
                        else
                        {
                            // Se houve virada de dia entre uma marcação e outra
                            if (Saida[i - 1] > EntradaMin)
                            {
                                EntradaMin += 1440;
                                SaidaMin += 1440;
                                HouveMudancaDeDia = true;
                            }
                            // Se a marcação virou o dia
                            if (EntradaMin > SaidaMin)
                            {
                                SaidaMin += 1440;
                                HouveMudancaDeDia = true;
                            }
                        }
                        // Se houve mudança de dia porém a marcação atual está no próximo dia
                        if (SaidaMin < InicioAdicionalNoturno && HouveMudancaDeDia)
                        {
                            EntradaMin += 1440;
                            SaidaMin += 1440;
                        }
                        // Se a marcação está dentro do adicional noturno então grava ela no controle
                        if (((InicioAdicionalNoturno < EntradaMin) && (FimAdicionalNoturno > EntradaMin)) || ((InicioAdicionalNoturno < SaidaMin) && (FimAdicionalNoturno > SaidaMin)))
                        {
                            // Se não peguei nenhum início de marcação ainda então ele é o menor
                            if (PrimeiraEntradaNoAdicionalNoturno == -1)
                            {
                                PrimeiraEntradaNoAdicionalNoturno = EntradaMin;
                            }
                            else
                            {
                                // Verifica se a marcação atual possui uma entrada menor que o 
                                if (PrimeiraEntradaNoAdicionalNoturno > EntradaMin)
                                {
                                    PrimeiraEntradaNoAdicionalNoturno = EntradaMin;
                                }
                            }
                            if (UltimaSaidaNoAdicionalNoturno == -1)
                            {
                                UltimaSaidaNoAdicionalNoturno = SaidaMin;
                            }
                            else
                            {
                                if (UltimaSaidaNoAdicionalNoturno < SaidaMin)
                                {
                                    UltimaSaidaNoAdicionalNoturno = SaidaMin;
                                }
                            }
                        }
                    }
                }
                // Validando se o laço pegou uma entrada menor que o início do adicional noturno
                if (PrimeiraEntradaNoAdicionalNoturno < InicioAdicionalNoturno)
                {
                    PrimeiraEntradaNoAdicionalNoturno = InicioAdicionalNoturno;
                }
                // Validando se o laço pegou uma saída maior que o fim do adicional noturno
                if (UltimaSaidaNoAdicionalNoturno > FimAdicionalNoturno)
                {
                    UltimaSaidaNoAdicionalNoturno = FimAdicionalNoturno;
                }

                horasTrabalhadasNoturnasMin = UltimaSaidaNoAdicionalNoturno - PrimeiraEntradaNoAdicionalNoturno;
                horasAdicionalNoturno = horasTrabalhadasNoturnasMin;
            }
        }

        private string CalculaInterjornada(List<Modelo.BilhetesImp> tratamentosDia, List<Modelo.BilhetesImp> tratamentosDiaAnterior)
        {
            string interjornada = "--:--";
            //Verifica se o dia possui uma batida(entrada)
            if (tratamentosDia.Count() > 0)
            {
                //Se não encontrou tratamentos pré carregados em memória, busca no banco
                if (tratamentosDiaAnterior.Count() == 0)
                {
                    tratamentosDiaAnterior = bllBilhetesImp.GetImportadosPeriodo(2, idFuncionario, data.AddDays(-1), data.AddDays(-1));
                }

                tratamentosDiaAnterior = tratamentosDiaAnterior.Where(w => w.Ocorrencia != 'D').ToList();

                //Verifica se o dia anterior possui pelo menos duas batidas (para possuir uma saída) e se as marcações estão corretas (número par)
                if (tratamentosDiaAnterior.Count() > 1 && tratamentosDiaAnterior.Count() % 2 == 0)
                {
                    Modelo.BilhetesImp be = tratamentosDia.Where(x => x.Ent_sai == "E" && x.Posicao == 1 && x.Ocorrencia != 'D').FirstOrDefault();
                    Modelo.BilhetesImp bsa = tratamentosDiaAnterior.Where(x => x.Ent_sai == "S").OrderByDescending(x => x.Posicao).FirstOrDefault();
                    if (be != null && bsa != null)
                    {
                        DateTime dtIni = Convert.ToDateTime(bsa.Data.ToShortDateString().ToString() + ' ' + bsa.Mar_hora);
                        DateTime dtFim = Convert.ToDateTime(be.Data.ToShortDateString().ToString() + ' ' + be.Mar_hora);
                        if (bsa.Data == be.Mar_data) ///Se é virada de turno, a saída foi alocada no dia anterior, considero a data do bilhete não da marcacao
                        {
                            dtIni = Convert.ToDateTime(bsa.Data.ToShortDateString().ToString() + ' ' + bsa.Mar_hora);
                            dtFim = Convert.ToDateTime(be.Data.ToShortDateString().ToString() + ' ' + be.Mar_hora);
                        }


                        int minutos = (int)dtFim.Subtract(dtIni).TotalMinutes;

                        interjornada = Modelo.cwkFuncoes.ConvertMinutosHora2(2, minutos);
                    }
                }
            }
            return interjornada;
        }

        //Para setar horaExtraInterjornada no dia seguinte ao da interjornada
        private string CalculaHoraExtraInterjornada(List<Modelo.BilhetesImp> tratamentosDia, List<Modelo.BilhetesImp> tratamentosDiaAnterior, int minutosHoraExtra)
        {
            string horaExtraInterjornada = "--:--";
            int tempoInterjornadaMinima = 660; //TODO: Recuperar valor padrão do banco de dados. (Padrão é 11:00 = 660 minutos)

            int interjornadaMin = Modelo.cwkFuncoes.ConvertHorasMinuto(Interjornada);

            if (interjornadaMin < tempoInterjornadaMinima && interjornadaMin > 0)
            {
                horaExtraInterjornada = Modelo.cwkFuncoes.ConvertMinutosHora2(2, tempoInterjornadaMinima - interjornadaMin);
            }
            return horaExtraInterjornada;
        }

        private bool ConsiderarHEFeriadoPHoraNoturna(ref bool feriadoProximoDia)
        {
            if (bConsiderarHEFeriadoPHoraNoturna)
            {
                BLL.Feriado bllFeriado = new BLL.Feriado(ConnectionString, UsuarioLogado);
                feriadoProximoDia = bllFeriado.PossuiRegistro(data.AddDays(+1), idEmpresa, idDepartamento);
                if (feriadoProximoDia)
                {
                    if (entrada_1Min > saida_1Min)
                    {
                        entrada_8Min = entrada_7Min;
                        saida_8Min = saida_7Min;
                        entrada_7Min = entrada_6Min;
                        saida_7Min = saida_6Min;
                        entrada_6Min = entrada_5Min;
                        saida_6Min = saida_5Min;
                        entrada_5Min = entrada_4Min;
                        saida_5Min = saida_4Min;
                        entrada_4Min = entrada_3Min;
                        saida_4Min = saida_3Min;
                        entrada_3Min = entrada_2Min;
                        saida_3Min = saida_2Min;

                        saida_2Min = saida_1Min;
                        entrada_2Min = 0;
                        saida_1Min = 1440;
                    }
                    if (entrada_2Min > saida_2Min)
                    {
                        entrada_8Min = entrada_7Min;
                        saida_8Min = saida_7Min;
                        entrada_7Min = entrada_6Min;
                        saida_7Min = saida_6Min;
                        entrada_6Min = entrada_5Min;
                        saida_6Min = saida_5Min;
                        entrada_5Min = entrada_4Min;
                        saida_5Min = saida_4Min;
                        entrada_4Min = entrada_3Min;
                        saida_4Min = saida_3Min;

                        saida_3Min = saida_2Min;
                        entrada_3Min = 0;
                        saida_2Min = 1440;
                    }
                    if (entrada_3Min > saida_3Min)
                    {
                        entrada_8Min = entrada_7Min;
                        saida_8Min = saida_7Min;
                        entrada_7Min = entrada_6Min;
                        saida_7Min = saida_6Min;
                        entrada_6Min = entrada_5Min;
                        saida_6Min = saida_5Min;
                        entrada_5Min = entrada_4Min;
                        saida_5Min = saida_4Min;

                        saida_4Min = saida_3Min;
                        entrada_4Min = 0;
                        saida_3Min = 1440;
                    }
                    if (entrada_4Min > saida_4Min)
                    {
                        entrada_8Min = entrada_7Min;
                        saida_8Min = saida_7Min;
                        entrada_7Min = entrada_6Min;
                        saida_7Min = saida_6Min;
                        entrada_6Min = entrada_5Min;
                        saida_6Min = saida_5Min;

                        saida_5Min = saida_4Min;
                        entrada_5Min = 0;
                        saida_4Min = 1440;
                    }
                    if (entrada_5Min > saida_5Min)
                    {
                        entrada_8Min = entrada_7Min;
                        saida_8Min = saida_7Min;
                        entrada_7Min = entrada_6Min;
                        saida_7Min = saida_6Min;

                        saida_6Min = saida_5Min;
                        entrada_6Min = 0;
                        saida_5Min = 1440;
                    }
                    if (entrada_6Min > saida_6Min)
                    {
                        entrada_8Min = entrada_7Min;
                        saida_8Min = saida_7Min;

                        saida_7Min = saida_6Min;
                        entrada_7Min = 0;
                        saida_6Min = 1440;
                    }
                    if (entrada_7Min > saida_7Min)
                    {
                        saida_7Min = 0;
                    }
                    if (entrada_8Min > saida_8Min)
                    {
                        saida_8Min = 0;
                    }
                }

                if (idFeriado != null && naoConsiderarFeriado == 0)
                {
                    CalculoDeJornadaNoturnaComFeriado();
                }
            }
            return feriadoProximoDia;
        }
        /// <summary>
        /// Calcula o tempo trabalhado em uma jornada noturna que começou em um dia sem feriado e terminou em um feriado.
        /// Adiciona o tempo(da virada do dia) trabalhado no feriado como hora extra no feriado.
        /// </summary>
        private void CalculoDeJornadaNoturnaComFeriado()
        {
            if (bConsiderarHEFeriadoPHoraNoturna && idFeriado != null && naoConsiderarFeriado == 0)
            {
                DataTable Marcacao = dalCalculaMarcacao.GetMarcacoesCalculo(2, idFuncionario, data.AddDays(-1), data.AddDays(-1), false, true, false);
                if (Marcacao.Rows.Count > 0)
                {
                    DataRow drMarc = Marcacao.Rows[0];
                    horasExtraNoturnaFeriadoMin = HorasExtraNoturnaFeriado(drMarc);
                    horasExtraDiurnaFeriadoMin = HorasExtraDiurnaFeriado(drMarc);
                }
            }
        }

        private int HorasExtraNoturnaFeriado(DataRow drMarc)
        {
            int? idBancoHorasF = drMarc["idbancohoras"] is DBNull ? null : (Int32?)(drMarc["idbancohoras"]);
            int naoEntrarBancoF = Convert.ToInt16(drMarc["naoentrarbanco"]);
            int? naoEntrarBancoFuncF = drMarc["naoentrarbancofunc"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(drMarc["naoentrarbancofunc"]);
            int heNoturnaFeriadoMin = 0;
            if (((idBancoHorasF == null) || (naoEntrarBancoF == 1) || (naoEntrarBancoFuncF == 1)))
            {
                int[] Entrada = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
                int[] Saida = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
                for (int i = 0; i < Entrada.Count(); i++)
                {
                    Entrada[i] = Convert.ToInt32(drMarc["entrada_" + (i + 1) + "min"]);
                }
                for (int i = 0; i < Saida.Count(); i++)
                {
                    Saida[i] = Convert.ToInt32(drMarc["saida_" + (i + 1) + "min"]);
                }
                int HoraDiurna = 0;
                int HoraNoturna = 0;

                BLL.CalculoHoras.QtdHorasDiurnaNoturna(Entrada, Saida, 1440, fimAdNoturno, ref HoraDiurna, ref HoraNoturna);
                if (HoraNoturna > 0)
                {
                    heNoturnaFeriadoMin += (HoraNoturna);
                }
                if (conversaoHoranoturna == 1)
                {
                    heNoturnaFeriadoMin = BLL.CalculoHoras.HoraNoturna(heNoturnaFeriadoMin, reducaohoranoturna);
                }
            }
            return heNoturnaFeriadoMin;
        }

        private int HorasExtraDiurnaFeriado(DataRow drMarc)
        {
            int? idBancoHorasF = drMarc["idbancohoras"] is DBNull ? null : (Int32?)(drMarc["idbancohoras"]);
            int naoEntrarBancoF = Convert.ToInt16(drMarc["naoentrarbanco"]);
            int? naoEntrarBancoFuncF = drMarc["naoentrarbancofunc"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(drMarc["naoentrarbancofunc"]);
            int horasExtraDiurnaFeriado = 0;
            if (((idBancoHorasF == null) || (naoEntrarBancoF == 1) || (naoEntrarBancoFuncF == 1)))
            {
                int[] Entrada = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
                int[] Saida = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
                for (int i = 0; i < Entrada.Count(); i++)
                {
                    Entrada[i] = Convert.ToInt32(drMarc["entrada_" + (i + 1) + "min"]);
                }
                for (int i = 0; i < Saida.Count(); i++)
                {
                    Saida[i] = Convert.ToInt32(drMarc["saida_" + (i + 1) + "min"]);
                }

                List<int> Entradas = new List<int>();
                List<int> Saidas = new List<int>();

                bool ativouDepoisMeiaNoite = false;

                for (int i = 1; i < Entrada.Length; i++)
                {
                    if (Entrada[i] < Entrada[i - 1] || Saida[i] < Saida[i - 1])
                    {
                        ativouDepoisMeiaNoite = true;
                    }
                    if (ativouDepoisMeiaNoite)
                    {
                        if (Entrada[i] > Saida[i])
                        {
                            Entradas.Add(0);
                        }
                        else
                        {
                            Entradas.Add(Entrada[i]);
                        }
                        Saidas.Add(Saida[i]);
                    }
                }

                while (Entradas.Count < 8)
                {
                    Entradas.Add(-1);
                }

                while (Saidas.Count < 8)
                {
                    Saidas.Add(-1);
                }

                int HoraDiurna = 0;
                int HoraNoturna = 0;

                for (int i = 0; i < Entrada.Length; i++)
                {
                    if (Entrada[i] > Saida[i])
                    {
                        Entrada[i] = 0;
                    }
                }

                BLL.CalculoHoras.QtdHorasDiurnaNoturna(Entradas.ToArray(), Saidas.ToArray(), inicioAdNoturno, fimAdNoturno, ref HoraDiurna, ref HoraNoturna);
                if (HoraDiurna > 0)
                {
                    horasExtraDiurnaFeriado += (HoraDiurna);
                }
            }
            return horasExtraDiurnaFeriado;
        }

        /// <summary>
        ///  Esse método realiza o calculo do DSR
        /// </summary>
        /// WNO - 23/03/2010
        public List<Modelo.Marcacao> CalculaDSR(bool bWebApi, bool salvarRegistrosCalculados)
        {
            VerificaProgressNula();
            BLL.Horario bllHorario = new Horario(ConnectionString, UsuarioLogado);
            ObjProgressBar.setaMensagem("Calculando DSR...");
            List<Modelo.Marcacao> comandos = new List<Modelo.Marcacao>();
            DataTable funcionarios;

            if (lIdsFuncionarios != null && lIdsFuncionarios.Count() > 0)
                funcionarios = dalCalculaMarcacao.GetFuncionariosDSR(lIdsFuncionarios, datai, dataf);
            else if (bWebApi)
                funcionarios = dalCalculaMarcacao.GetFuncionariosDSRWebApi(tipo, identificacao, datai, dataf);
            else
                funcionarios = dalCalculaMarcacao.GetFuncionariosDSR(tipo, identificacao, datai, dataf);

            DateTime? dataIFunc, dataFFunc;
            int idfunc = 0, qtdDSR = 0;
            DataTable marcacoesDsr = null;
            //Realiza o cálculo do DSR individual por funcionário

            ObjProgressBar.setaMinMaxPB(0, funcionarios.Rows.Count);
            ObjProgressBar.setaValorPB(0);
            foreach (DataRow func in funcionarios.Rows)
            {
                qtdDSR = Convert.ToInt32(func["qtddsr"]);
                if (qtdDSR > 0)
                {
                    ObjProgressBar.setaMensagem("Calculando DSR - Funcionário(a): " + func["nome"].ToString());
                    idfunc = Convert.ToInt32(func["idfuncionario"]);
                    //Busca a data do ultimo dsr antes do período cálculado
                    dataIFunc = bllMarcacao.GetDataDSRAnterior(idfunc, datai);
                    if (dataIFunc == null)
                        dataIFunc = datai;
                    else
                        dataIFunc = dataIFunc.Value.AddDays(1);

                    dataFFunc = bllMarcacao.GetDataDSRProximo(idfunc, dataf);
                    if (dataFFunc == null)
                        dataFFunc = dataf;

                    marcacoesDsr = dalCalculaMarcacao.GetMarcacoesDSR(idfunc, dataIFunc.Value, dataFFunc.Value);

                    try
                    {
                        var marcEnumerable = marcacoesDsr.AsEnumerable();
                        var marcGroupByHorario = marcacoesDsr.AsEnumerable().GroupBy(g => g.Field<int>("idhorario"));

                        Dictionary<int, Modelo.Horario> HorariosDasMarcacoes = new Dictionary<int, Modelo.Horario>();
                        foreach (var item in marcGroupByHorario)
                        {
                            if (!HorariosDasMarcacoes.ContainsKey(item.Key))
                            {
                                HorariosDasMarcacoes.Add(item.Key, bllHorario.LoadObject(item.Key));
                            }
                        }


                        if (marcGroupByHorario.FirstOrDefault(f => HorariosDasMarcacoes[f.Key].bUtilizaDDSRProporcional || HorariosDasMarcacoes[f.Key].DSRPorPercentual) != null)
                        { //existem marcações com horários que usam DDSR Proporcional
                            int rowCounter = 0;

                            IEnumerable<DataRow> semana = marcEnumerable;
                            while (semana != null)
                            {
                                rowCounter += semana.Count();
                                IDDSRStrategy calcDDSR = DDSRStrategyFactory.Produce(semana, HorariosDasMarcacoes, ConnectionString);
                                try
                                {
                                    comandos.AddRange(calcDDSR.CalcularDDSR());
                                    semana = marcEnumerable.Skip(rowCounter).InclusiveTakeWhile(t => t.Field<Int16?>("dsr").GetValueOrDefault() != 1);
                                    if (semana.Count() == 0)
                                    {
                                        semana = null;
                                    }
                                }
                                catch (Exception z)
                                {
                                    Exception a = new Exception("Houve um erro ao recalcular DDSR Proporcional. Tipo de cálculo: " + calcDDSR.ToString(), z);
                                    throw a;
                                }
                            }
                        }
                        else
                        {
                            #region Calculo Sem DDSR Proporcional
                            //Campos marcação
                            bool semanaComFeriado = false;
                            bool _DDSRConsideraFaltaDuranteSemana = false;
                            short Dsr = 0;
                            int valorDsr = 0, horasFaltasDsr = 0;
                            string legendaDsr = "", valorDsrStr = "";
                            decimal descontoHorasDsr;
                            //Campos Horário
                            int limitePerdaDsr = 0, qtdHorasDsr = 0, descontarDsr = 0, abonarDsr = 0;
                            //Totais DSR
                            int totalValorDsr = 0, totalDsrFeriado = 0;

                            Modelo.Horario h = new Modelo.Horario();
                            int idHorario = 0;
                            foreach (DataRow marc in marcacoesDsr.Rows)
                            {
                                DateTime data = Convert.ToDateTime(marc["data"]);
                                Dsr = marc["dsr"] is DBNull ? (Int16)0 : Convert.ToInt16(marc["dsr"]);
                                legendaDsr = marc["legenda"].ToString();
                                valorDsr = Convert.ToInt32(marc["valordsrmin"]);
                                _DDSRConsideraFaltaDuranteSemana = Convert.ToBoolean(marc["DDSRConsideraFaltaDuranteSemana"]);
                                if (_DDSRConsideraFaltaDuranteSemana)
                                    horasFaltasDsr = horasFaltasDsr + Convert.ToInt32(marc["horasfaltasdsr"]);
                                else
                                    horasFaltasDsr = Convert.ToInt32(marc["horasfaltasdsr"]);
                                limitePerdaDsr = Convert.ToInt32(marc["limiteperdadsr"]);
                                qtdHorasDsr = Convert.ToInt32(marc["qtdhorasdsrmin"]);
                                descontarDsr = Convert.ToInt32(marc["descontardsr"]);
                                idHorario = Convert.ToInt32(marc["idhorario"]);
                                descontoHorasDsr = (marc["DescontoHorasDSR"] is DBNull) ? 0 : Convert.ToDecimal(marc["DescontoHorasDSR"]);

                                if (h == null)
                                {
                                    h = bllHorario.LoadObject(idHorario);
                                }
                                else if (idHorario != h.Id)
                                {
                                    h = bllHorario.LoadObject(idHorario);
                                }
                                abonarDsr = marc["abonardsr"] is DBNull ? (Int32)0 : Convert.ToInt32(marc["abonardsr"]);

                                //Aquele dia é dsr
                                if (Dsr == 1)
                                {
                                    if (h.DescontarFeriadoDDSR)
                                    {
                                        valorDsrStr = Modelo.cwkFuncoes.ConvertMinutosHora(2, totalValorDsr + totalDsrFeriado);
                                    }
                                    else
                                    {
                                        valorDsrStr = Modelo.cwkFuncoes.ConvertMinutosHora(2, totalValorDsr);
                                    }
                                    comandos.Add(PreencheMarcacaoDSR(marc, valorDsrStr, Dsr, legendaDsr));
                                    totalValorDsr = 0; totalDsrFeriado = 0;
                                    horasFaltasDsr = 0;
                                }
                                else
                                {
                                    if (horasFaltasDsr != 0 && horasFaltasDsr >= limitePerdaDsr
                                                            && descontarDsr == 1 && abonarDsr == 0)
                                    {
                                        totalValorDsr = qtdHorasDsr;
                                    }

                                    if (legendaDsr == "F")
                                        semanaComFeriado = true;

                                    if (semanaComFeriado && totalValorDsr > 0)
                                    {
                                        totalDsrFeriado += qtdHorasDsr;
                                        semanaComFeriado = false;
                                    }
                                }

                            }
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                ObjProgressBar.incrementaPB(1);
            }
            if (salvarRegistrosCalculados)
            {
                dalCalculaMarcacao.PersistirDados(comandos, null);
            }
            return comandos;
        }

        private void VerificaProgressNula()
        {
            if ((ObjProgressBar.incrementaPB == null) &&
                (ObjProgressBar.setaMensagem == null) &&
                (ObjProgressBar.setaMinMaxPB == null) &&
                (ObjProgressBar.setaValorPB == null))
            {
                objProgressBar.incrementaPB = this.IncrementaProgressBar;
                objProgressBar.setaMensagem = this.SetaMensagem;
                objProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
                objProgressBar.setaValorPB = this.SetaValorProgressBar;
            }
        }

        private void SetaValorProgressBar(int valor)
        {
        }

        private void SetaMinMaxProgressBar(int min, int max)
        {
        }

        private void SetaMensagem(string mensagem)
        {
        }

        private void IncrementaProgressBar(int incremento)
        {
        }

        #endregion

        public string GetLegenda(int pidFuncionario, DateTime pData)
        {
            return dalCalculaMarcacao.GetLegenda(pidFuncionario, pData);
        }

        #region Métodos de Cálculo

        private void CalculaCompensacao()
        {
            horasCompensadasMin = 0;
            horasCompensadas = "--:--";

            //Somente entra na compensação se o funcionario não estiver marcado para nao entrar
            if (naoEntrarNaCompensacao == 0)
            {
                List<Modelo.Compensacao> listaCompensacao = bllCompensacao.getListaCompensacao(data, idFuncionario, idDepartamento, idFuncao, idEmpresa, compensacaoList);
                foreach (Modelo.Compensacao comp in listaCompensacao)
                {
                    if (comp.getDias()[diaInt - 1] == 1)
                    {
                        if (legenda != "A")
                        {
                            legenda = "C";
                            LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "C");
                        }

                        if (horasExtrasDiurnaMin != 0 || horasExtraNoturnaMin != 0)
                        {
                            //Hora extra diurna
                            int hed = horasExtrasDiurnaMin;
                            //Hora extra diurna
                            int hen = horasExtraNoturnaMin;
                            //Hora extra diurna
                            int he = hed + hen;

                            //Hora a ser compensada
                            int hc = Modelo.cwkFuncoes.ConvertHorasMinuto(comp.getTotalHorasSerCompensadas()[diaInt - 1]);

                            if (he <= hc)//Horas a serem compensadas é exatamente igual as horas extras
                            {
                                horasCompensadasMin = he;
                                horasExtraNoturnaMin = 0;//Zera
                                horasExtrasDiurnaMin = 0;//Zera
                                AdicionalNoturno = 0;
                            }
                            else //Sobram horas extras
                            {
                                //Aqui sempre as horas foram compensadas completas.
                                //OBJ recebe o maximo de horas a ser compensada naquele dia e o resto fica como sendo hora extra mesmo
                                horasCompensadasMin = Modelo.cwkFuncoes.ConvertHorasMinuto(comp.getTotalHorasSerCompensadas()[diaInt - 1]);

                                if (hed > hc) //Se tem horas extras diurnas sobrando
                                {
                                    hed = hed - hc;//Tira um pouco das horas extras diurnas e a noturnas sobram intactas
                                    horasExtrasDiurnaMin = hed;
                                    horasExtraNoturnaMin = hen;
                                }
                                else if (hed == hc)//Se as horas diurnas são exatamente iguais
                                {
                                    horasExtrasDiurnaMin = 0;//Zera
                                    horasExtraNoturnaMin = hen;
                                }
                                else //Se tem menos horas extras diurnas
                                {
                                    horasExtrasDiurnaMin = 0;//Zera
                                    hen = hen - (hc - hed); // Tira das horas extras noturnas o que falta para completar as horas
                                    horasExtraNoturnaMin = hen;
                                }
                            }

                            horasCompensadas = Modelo.cwkFuncoes.ConvertMinutosHora(horasCompensadasMin);

                            ocorrencia = horasCompensadas + " Hr Compensação";
                        }
                        else
                        {
                            horasCompensadasMin = 0;
                            horasCompensadas = "--:--";
                        }
                    }
                }
            }
            else
            {
                horasCompensadasMin = 0;
                horasCompensadas = "--:--";
            }
        }

        #region InclusãoBancoHoras

        public void InclusaoBancoHoras(ref int CreditoBH, ref int DebitoBH, int pIdFuncionario, Modelo.BancoHoras bdh)
        {
            int credito = 0;
            int debito = 0;
            string justificativa;
            bllInclusaoBanco.getSaldo(data, idEmpresa, idDepartamento, idFuncionario, idFuncao, out credito, out debito, out justificativa);
            CreditoBH = CreditoBH + credito;
            if (bdh != null)
            {
                if (bdh.BancoHorasAcumulativo)
                {
                    if (bdh.TipoAcumulo == 0) //acumulo semanal
                    {
                        DateTime dataInicio = new DateTime(data.StartOfWeek().Year, data.StartOfWeek().Month, data.StartOfWeek().Day);
                        DateTime dataFim = new DateTime(data.Year, data.Month, data.Day);
                        dataFim = dataFim.AddDays(-1);
                        int creditoSemana = bllInclusaoBanco.getCreditoPeriodoAtual(idFuncionario, dataInicio, dataFim);
                        if ((!(data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)) &&
                            objMarcacao.Folga == 0 && idFeriado == null && (objMarcacao.Afastamento != null && !(objMarcacao.Afastamento.bSuspensao)))
                        {
                            CreditoBH = CalculaAcrescimoBcoHorasAcumulativo(bdh, creditoSemana, CreditoBH);
                        }

                    }
                    else //acumulo mensal
                    {
                        DateTime dataInicio = new DateTime(data.StartOfMonth().Year, data.StartOfMonth().Month, data.StartOfMonth().Day);
                        DateTime dataFim = new DateTime(data.Year, data.Month, data.Day);
                        dataFim = dataFim.AddDays(-1);
                        int creditoMes = bllInclusaoBanco.getCreditoPeriodoAtual(idFuncionario, dataInicio, dataFim);
                        if ((!(data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)) &&
                            objMarcacao.Folga == 0 && idFeriado == null && (objMarcacao.Afastamento != null && !(objMarcacao.Afastamento.bSuspensao)))
                        {
                            CreditoBH = CalculaAcrescimoBcoHorasAcumulativo(bdh, creditoMes, CreditoBH);
                        }
                    }
                }
            }
            DebitoBH = DebitoBH + debito;

            bool teminc = false;
            if (credito > 0 || debito > 0)
            {
                teminc = true;
            }


            //Verifica se neste dia possui fechamento
            Modelo.FechamentoBHD objFechamentoBHD = null;
            if (fechamentoBHDList.Count > 0)
            {
                if (fechamentoBHDList.Exists(x => x.DataFechamento == data))
                {
                    foreach (Modelo.FechamentoBHD fbhd in fechamentoBHDList)
                    {
                        if (fbhd.DataFechamento == data && fbhd.Identificacao == pIdFuncionario)
                        {
                            objFechamentoBHD = fbhd;
                        }
                    }
                }
            }

            if (objFechamentoBHD != null)
            {
                if (ocorrencia != "Marcações Incorretas")
                {
                    ocorrencia = objFechamentoBHD.Saldo + " [" + (objFechamentoBHD.Tiposaldo == 0 ? "C" : "D") + "] H. Pagas";
                    if (!String.IsNullOrEmpty(objFechamentoBHD.MotivoFechamento))
                    {
                        ocorrencia += " - " + objFechamentoBHD.MotivoFechamento;
                    }

                    idFechamentoBH = objFechamentoBHD.Idfechamentobh;
                }

                bancoHorasCre = Modelo.cwkFuncoes.ConvertMinutosHora2(3, CreditoBH) != "000:00" ? Modelo.cwkFuncoes.ConvertMinutosHora(3, CreditoBH) : "---:--";
                bancoHorasDeb = Modelo.cwkFuncoes.ConvertMinutosHora2(3, DebitoBH) != "000:00" ? Modelo.cwkFuncoes.ConvertMinutosHora(3, DebitoBH) : "---:--"; ;
            }
            else
            {
                if (CreditoBH > 0 || DebitoBH > 0)
                {
                    if (ocorrencia != "Marcações Incorretas" && dataDemissao != data)
                    {
                        if (CreditoBH == DebitoBH)
                        {
                            ocorrencia = Modelo.cwkFuncoes.ConvertMinutosHora2(3, CreditoBH - DebitoBH) + " - Crédito no BH";
                            ocorrencia += string.IsNullOrEmpty(justificativa) ? "" : " - " + justificativa;
                        }
                        else if (CreditoBH > DebitoBH && CreditoBH > 0)
                        {
                            ocorrencia = Modelo.cwkFuncoes.ConvertMinutosHora2(3, CreditoBH - DebitoBH) + " - Crédito no BH";
                            ocorrencia += string.IsNullOrEmpty(justificativa) ? "" : " - " + justificativa;
                        }
                        else if (DebitoBH > 0)
                        {
                            if (!(legenda != "A" && semCalculo == 1))
                            {
                                ocorrencia = Modelo.cwkFuncoes.ConvertMinutosHora2(3, DebitoBH - CreditoBH) + " - Débito no BH";
                                ocorrencia += string.IsNullOrEmpty(justificativa) ? "" : " - " + justificativa;
                            }
                            else
                            {
                                ocorrencia = Modelo.cwkFuncoes.ConvertMinutosHora2(3, DebitoBH - CreditoBH) + " - " + ocorrencia;
                                ocorrencia += string.IsNullOrEmpty(justificativa) ? "" : " - " + justificativa;
                            }
                        }
                    }
                    else if (ocorrencia != "Marcações Incorretas" && dataDemissao == data)
                    {
                        setaOcorrenciaDemitido();
                    }

                    bancoHorasCre = Modelo.cwkFuncoes.ConvertMinutosHora2(3, CreditoBH) != "000:00" ? Modelo.cwkFuncoes.ConvertMinutosHora2(3, CreditoBH) : "---:--";
                    bancoHorasDeb = Modelo.cwkFuncoes.ConvertMinutosHora2(3, DebitoBH) != "000:00" ? Modelo.cwkFuncoes.ConvertMinutosHora2(3, DebitoBH) : "---:--"; ;
                }
            }

            if (teminc == true && legenda != "A")
            {
                legenda = "I";
                LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "I");
            }
        }

        private int CalculaAcrescimoBcoHorasAcumulativo(Modelo.BancoHoras bdh, int SaldoAtual, int AcumuloAtual)
        {
            int minutosCalculados = 0;
            if (bdh == null)
            {
                return AcumuloAtual;
            }
            bool[] limitesPercentualHoras = bdh.getLimitesBcoAcumulativo();
            string[] limiteQtdHoras = bdh.getQtdHorasBcoAcumulativo();
            decimal[] pctsHorasBanco = bdh.getPctHorasBcoAcumulativo();
            IList<ParmsBancoHorasAcumulativo> parametros = new List<ParmsBancoHorasAcumulativo>();
            for (int i = 0; i < limitesPercentualHoras.Length; i++)
            {
                if (limitesPercentualHoras[i])
                {
                    parametros.Add(new ParmsBancoHorasAcumulativo()
                    {
                        Ativo = true,
                        Percentual = pctsHorasBanco[i],
                        LimiteMinutos = Modelo.cwkFuncoes.ConvertHorasMinuto(limiteQtdHoras[i])
                    });
                }
            }
            if (parametros.Count == 0)
            {
                return AcumuloAtual;
            }
            if (AcumuloAtual > 0)
            {
                int totalMinutos = SaldoAtual + AcumuloAtual;
                ParmsBancoHorasAcumulativo parametroMenor = parametros.Where(w => w.LimiteMinutos >= SaldoAtual).FirstOrDefault();

                parametroMenor = parametroMenor == null ? new ParmsBancoHorasAcumulativo() : parametroMenor;

                ParmsBancoHorasAcumulativo parametroMaior = parametros.Where(w => w.LimiteMinutos > parametroMenor.LimiteMinutos).FirstOrDefault();

                parametroMaior = parametroMaior == null ? parametroMenor : parametroMaior;

                if (totalMinutos <= parametroMenor.LimiteMinutos)
                {
                    minutosCalculados = Convert.ToInt32(AcumuloAtual * (1 + (parametroMenor.Percentual / 100)));
                }
                else
                {
                    int minutosRestantesParaoLimite = Math.Abs(SaldoAtual - parametroMenor.LimiteMinutos);

                    minutosCalculados += Convert.ToInt32(minutosRestantesParaoLimite * (1 + (parametroMenor.Percentual / 100)));

                    minutosCalculados += Convert.ToInt32((AcumuloAtual - minutosRestantesParaoLimite) * (1 + (parametroMaior.Percentual / 100)));
                }
            }
            return minutosCalculados;
        }
        #endregion

        private bool CalculaBancoHoras(int pIdFuncionario, ICollection<Modelo.Marcacao> marcacoes)
        {
            Modelo.BancoHoras objBancoHoras = null;

            if (idBancoHoras != null)
            {
                try
                {
                    if (bancoHorasList != null && bancoHorasList.ContainsKey(idBancoHoras))
                    {
                        objBancoHoras = (Modelo.BancoHoras)bancoHorasList[idBancoHoras.GetValueOrDefault()];
                    }
                    else
                    {
                        objBancoHoras = bllBancoHoras.LoadObjectSemRestricao(idBancoHoras.GetValueOrDefault());
                        if (bancoHorasList == null)
                        {
                            bancoHorasList = new Hashtable();
                        }
                        bancoHorasList.Add(idBancoHoras, objBancoHoras);
                    }
                }
                catch (Exception)
                {
                    throw;
                }

            }

            bancoHorasCre = "---:--";
            bancoHorasDeb = "---:--";
            int CreditoBH = 0;
            int DebitoBH = 0;

            if ((objBancoHoras == null) || (naoEntrarBanco == 1) || (naoEntrarBancoFunc == 1))
            {
                return CalculoNaoEntraEmBanco(pIdFuncionario, objBancoHoras, out CreditoBH, out DebitoBH);
            }

            //Se houver entrada/saida OK ou se houver faltas
            if ((entrada_1Min != -1 && saida_1Min != -1) || (horasFaltasMin != 0) || (horasFaltaNoturnaMin != 0))
            {
                int fdia = 0;
                bool folga = false;
                //pega dados do BH
                bool[] dias = objBancoHoras.getDias();
                int[] percentuais = objBancoHoras.getPercentuais();
                string[] limitesHoras = objBancoHoras.getLimiteHoras();
                string[] limitesHorasDiarios = objBancoHoras.getLimiteHorasLimiteDiarios();
                string[] limiteHoraExtra = objBancoHoras.getLimiteHoraExtra();
                string[] limiteHoraSaldoBH = objBancoHoras.getLimiteSaldoBH();

                //Valida primero os parametros inseridos por grid de marcação
                if (ContabilizarFaltasMarc != 2)
                    objBancoHoras.ContabilizarFaltas = ContabilizarFaltasMarc == 1 && ContabilizarFaltasMarc != 2 ? objBancoHoras.ContabilizarFaltas = false : objBancoHoras.ContabilizarFaltas = true;
                if (ContAtrasosSaidasAntecMarc != 2)
                    objBancoHoras.ContAtrasosSaidasAntec = ContAtrasosSaidasAntecMarc == 1 && ContAtrasosSaidasAntecMarc != 2 ? objBancoHoras.ContAtrasosSaidasAntec = false : objBancoHoras.ContAtrasosSaidasAntec = true;
                if (ContabilizarCreditosMarc != 2)
                    objBancoHoras.ContabilizarCreditos = ContabilizarCreditosMarc == 1 && ContabilizarCreditosMarc != 2 ? objBancoHoras.ContabilizarCreditos = false : objBancoHoras.ContabilizarCreditos = true;
                // soma das horas-extras e faltas
                if (objBancoHoras.ContabilizarCreditos == true)
                {
                    CreditoBH = horasExtrasDiurnaMin + horasExtraNoturnaMin;
                }

                if (legenda != "F" || feriadoParcial)
                {
                    if ((objBancoHoras.ContabilizarFaltas == true && objBancoHoras.ContAtrasosSaidasAntec == true) ||
                        (objBancoHoras.ContabilizarFaltas == true && objBancoHoras.ContAtrasosSaidasAntec == false && horasTrabalhadasMin <= 0) ||
                        (objBancoHoras.ContabilizarFaltas == false && objBancoHoras.ContAtrasosSaidasAntec == true && horasTrabalhadasMin > 0))
                    {
                        DebitoBH = horasFaltasMin + horasFaltaNoturnaMin;
                    }
                }
                else
                {
                    DebitoBH = 0;
                }

                if (marcaCargaHorariaMistaHD == 1 && CreditoBH > 0 && DebitoBH > 0 && !feriadoParcial)
                {
                    if (CreditoBH >= DebitoBH)
                    {
                        CreditoBH -= DebitoBH;
                        DebitoBH = 0;
                    }
                    else
                    {
                        DebitoBH -= CreditoBH;
                        CreditoBH = 0;
                    }
                }

                if (idFeriado != null && naoConsiderarFeriado == 0)
                {
                    fdia = 8;
                }
                else
                {
                    fdia = 0;
                }

                if (tipoHorario == 1)
                {
                    folga = bCarregar == 1 ? false : true;
                }
                else
                {
                    folga = Convert.ToBoolean(flagFolga);
                }
                // Se for folga e o percentual dela for maior que o dia corrente, 
                // Ou se a folga estiver marcada para não entrar no banco, seta a folga como dia corrente
                if ((folga || folgaMarcacao == 1) && (horasTrabalhadasMin + horasTrabalhadasNoturnasMin) == 0 && CreditoBH != 0)
                {
                    if (percentuais[9] >= percentuais[diaInt] || dias[diaInt])
                    {
                        diaInt = 9;
                    }
                }

                // Se for feriado e o percentual dele for maior que o dia corrente, 
                // Ou se o feriado estiver marcado para não entrar no banco, seta o feriado como dia corrente
                if (fdia > 0 && ((percentuais[fdia] >= percentuais[diaInt]) || (!dias[fdia])))
                {
                    diaInt = fdia;
                }
                //Se o dia não entrar no banco de horas retorna
                if (!dias[diaInt])
                {
                    return CalculoNaoEntraEmBanco(pIdFuncionario, objBancoHoras, out CreditoBH, out DebitoBH);
                }
                int hora = 0;
                int limite = -1;
                //string limite_str = "--:--";

                if (dias[diaInt] && CreditoBH != 0 && percentuais[diaInt] != 0 && objBancoHoras.Bancoprimeiro == 1 && !objBancoHoras.BancoHorasPorPercentual)
                {
                    hora = CreditoBH + (CreditoBH * percentuais[diaInt] / 100);
                    CreditoBH = hora;

                }

                int? limitePSaldo = null;
                bool limitePorSaldo = false;

                if (dias[diaInt] && limiteHoraSaldoBH[diaInt] != null && Regex.IsMatch(limiteHoraSaldoBH[diaInt], @"\d"))
                {
                    int limiteSaldo = Modelo.cwkFuncoes.ConvertHorasMinuto(limiteHoraSaldoBH[diaInt]);

                    Modelo.Proxy.PxySaldoBancoHoras saldoFunc = saldoBHFuncs.Where(s => s.IdFuncionario == idFuncionario).FirstOrDefault();
                    if (saldoFunc == null || saldoFunc.IdFuncionario == 0)
                    {
                        saldoFunc = bllBancoHoras.SaldoBancoHoras(datai.AddDays(-1), new List<int>() { idFuncionario }).FirstOrDefault();
                        saldoBHFuncs.Add(saldoFunc);
                    }

                    int saldoMarcacoesJaRecalculadas = marcacoes.ToList().Where(m => m.Idfuncionario == idFuncionario && m.Data < data).Sum(s => Modelo.cwkFuncoes.ConvertHorasMinuto(s.Bancohorascre) - Modelo.cwkFuncoes.ConvertHorasMinuto(s.Bancohorasdeb));
                    int saldoAnterior = saldoFunc.Saldo + saldoMarcacoesJaRecalculadas;
                    if (saldoAnterior < 0)
                    {
                        limitePSaldo = (saldoAnterior * -1) + limiteSaldo;
                    }
                    else if (saldoAnterior > 0)
                    {
                        limitePSaldo = limiteSaldo - saldoAnterior;
                        limitePSaldo = limitePSaldo < 0 ? 0 : limitePSaldo;
                    }
                    else
                    {
                        limitePSaldo = limiteSaldo;
                    }
                }

                if (limitePSaldo != null &&
                    ((limitesHoras[diaInt] == null || limitesHoras[diaInt] == "--:--" || limitePSaldo <= Modelo.cwkFuncoes.ConvertHorasMinuto(limitesHoras[diaInt])) &&
                     (limitesHorasDiarios[diaInt] == null || limitesHorasDiarios[diaInt] == "--:--" || limitePSaldo <= Modelo.cwkFuncoes.ConvertHorasMinuto(limitesHorasDiarios[diaInt]))))
                {
                    limitePorSaldo = true;
                    limite = limitePSaldo.GetValueOrDefault();
                }
                else if (CreditoBH > Modelo.cwkFuncoes.ConvertHorasMinuto(limitesHoras[diaInt]) && dias[diaInt] && objBancoHoras.Bancoprimeiro == 1)
                {
                    int newLimiteDia;
                    if (limitesHorasDiarios[diaInt] != "--:--")
                    {
                        int limiteDiarioInteiro = Modelo.cwkFuncoes.ConvertHorasMinuto(limitesHorasDiarios[diaInt]);
                        newLimiteDia = limiteDiarioInteiro - totalizaCargaHoraria;

                        if (newLimiteDia > 0)
                        {
                            limite = newLimiteDia;
                        }
                        else
                        {
                            limite = 0;
                        }
                    }
                    else if (limitesHoras[diaInt] != "--:--" && !String.IsNullOrEmpty(limitesHoras[diaInt]))
                    {
                        limite = Modelo.cwkFuncoes.ConvertHorasMinuto(limitesHoras[diaInt]);
                    }
                }
                else if (CreditoBH > Modelo.cwkFuncoes.ConvertHorasMinuto(limiteHoraExtra[diaInt]) && dias[diaInt] && objBancoHoras.ExtraPrimeiro == 1)
                {
                    int aux = Modelo.cwkFuncoes.ConvertHorasMinuto(limiteHoraExtra[diaInt]);
                    limite = BLL.CalculoHoras.OperacaoHoras('-', horasExtrasDiurnaMin + horasExtraNoturnaMin, aux);
                }
                else if (dias[diaInt] && objBancoHoras.ExtraPrimeiro == 1)
                {
                    CreditoBH = 0;
                }

                bool limiteSemanal = false;
                bool limiteMensal = false;

                int totalExcedenteSemana = 0;
                int saldoDia = CreditoBH - DebitoBH;
                if (VerificaLimeteSemanal(objBancoHoras, data, saldoDia, diaSemanaDsr, diaInt, pIdFuncionario, out totalExcedenteSemana, marcacoes, ref limite))
                {
                    //aplicaPercentualBancoHora(ref CreditoBH, objBancoHoras, percentuais[diaInt]);
                    AjustaLimite(CreditoBH, totalExcedenteSemana, ref limite);
                    limiteSemanal = true;
                }

                int LimiteMesAcumulado, TotalExcedenteMensalDiaMin, TotalExcedenteMensalMin;

                //Verifica se excedeu o limite de crédito do dia no mês
                if (VerificaExcedeuLimiteMensal(objBancoHoras, CreditoBH, diaInt, pIdFuncionario, out TotalExcedenteMensalDiaMin, marcacoes, ref limite))
                {
                    AjustaLimite(CreditoBH, TotalExcedenteMensalDiaMin, ref limite);
                    limiteMensal = true;
                    //Depois de validar se o limite do dia no mês foi ultrapassado é preciso validar se o limite do mês foi ultrapassado
                    if (objBancoHoras.BancoHorasDiarioMensal)
                    {
                        LimiteMesAcumulado = bllInclusaoBanco.getCreditoPeriodoAcumuladoMes(idFuncionario, data);
                        int LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.LimiteHorasBancoHorasDiarioMensal);
                        TotalExcedenteMensalMin = ((CreditoBH + LimiteMesAcumulado) - LimiteMesParametro);

                        if (TotalExcedenteMensalMin > 0)
                        {
                            AjustaLimite(CreditoBH, TotalExcedenteMensalMin, ref limite);
                        }
                    }
                }
                // verifica se excedeu o limite de crédito do mês
                else if (VerificaExcedeuLimiteMensal(objBancoHoras, CreditoBH, null, pIdFuncionario, out TotalExcedenteMensalMin, marcacoes, ref limite))
                {
                    AjustaLimite(CreditoBH, TotalExcedenteMensalMin, ref limite);
                    limiteMensal = true;
                }

                if ((limite > 0 || limitePorSaldo || limiteSemanal || limiteMensal) && objBancoHoras.ContabilizarCreditos == true)
                {
                    if (CreditoBH > limite)
                    {
                        CreditoBH = limite;
                    }
                    AplicaPercentualBancoHora(ref CreditoBH, ref DebitoBH, objBancoHoras, percentuais[diaInt]);
                    if (dias[diaInt] && CreditoBH != 0 && percentuais[diaInt] != 0)
                    {
                        if (objBancoHoras.ExtraPrimeiro == 1)
                        {
                            hora = CreditoBH + (CreditoBH * percentuais[diaInt] / 100);
                            CreditoBH = hora;
                        }

                        if (objBancoHoras.PercentualComoHoraExtra)
                        {
                            horasExtrasDiurnaMin = horasExtrasDiurnaMin + (horasExtrasDiurnaMin * percentuais[diaInt] / 100);
                            horasExtraNoturnaMin = horasExtraNoturnaMin + (horasExtraNoturnaMin * percentuais[diaInt] / 100);
                        }
                    }

                    if (horasExtraNoturnaMin != 0 && horasExtrasDiurnaMin == 0 && horasExtraNoturnaMin > limite)
                    {
                        horasExtraNoturnaMin = BLL.CalculoHoras.OperacaoHoras('-', horasExtraNoturnaMin, limite);
                    }
                    else if (horasExtraNoturnaMin == 0 && horasExtrasDiurnaMin != 0 && horasExtrasDiurnaMin > limite)
                    {
                        horasExtrasDiurnaMin = BLL.CalculoHoras.OperacaoHoras('-', horasExtrasDiurnaMin, limite);
                    }
                    else
                    {
                        if (objBancoHoras.ExtraPrimeiro == 1)
                        {
                            //horasExtrasDiurnaMin = BLL.CalculoHoras.OperacaoHoras('-', horasExtrasDiurnaMin + horasExtraNoturnaMin, limite);
                            //horasExtraNoturnaMin = 0;

                            if (horasExtrasDiurnaMin >= limite)
                            {
                                horasExtrasDiurnaMin = BLL.CalculoHoras.OperacaoHoras('-', horasExtrasDiurnaMin, limite);
                            }
                            //Tira todas as horas do diurno e ainda tem que tirar algumas do noturno
                            else
                            {
                                //Calcula as horas que faltam para completar o limite
                                int faltaParaCompletar = limite - horasExtrasDiurnaMin;
                                horasExtrasDiurnaMin = 0;
                                horasExtraNoturnaMin = BLL.CalculoHoras.OperacaoHoras('-', horasExtraNoturnaMin, faltaParaCompletar);
                            }
                        }
                        else
                        {
                            int somaHoras = horasExtrasDiurnaMin + horasExtraNoturnaMin;
                            if (somaHoras >= limite)
                            {
                                //CreditoBH = limite;
                                // Se não tem horas extras noturnas tudo vai para diurno
                                if (horasExtraNoturnaMin == 0)
                                {
                                    horasExtrasDiurnaMin = BLL.CalculoHoras.OperacaoHoras('-', horasExtrasDiurnaMin, limite);
                                }
                                // Tem hora extra noturna e não tem hora extra diurna
                                else if (horasExtrasDiurnaMin == 0)
                                {
                                    horasExtraNoturnaMin = BLL.CalculoHoras.OperacaoHoras('-', horasExtraNoturnaMin, BLL.CalculoHoras.OperacaoHoras('-', limite, horasExtrasDiurnaMin));
                                }
                                // As horas são divididas entre extra noturna e diurna
                                else
                                {
                                    //Sobra extras diurnas e as noturnas continuam iguais
                                    if (horasExtrasDiurnaMin >= limite)
                                    {
                                        horasExtrasDiurnaMin = BLL.CalculoHoras.OperacaoHoras('-', horasExtrasDiurnaMin, limite);
                                    }
                                    //Tira todas as horas do diurno e ainda tem que tirar algumas do noturno
                                    else
                                    {
                                        //Calcula as horas que faltam para completar o limite do banco
                                        int faltaParaCompletar = limite - horasExtrasDiurnaMin;
                                        horasExtrasDiurnaMin = 0;
                                        //O resto vai para hora extra
                                        horasExtraNoturnaMin = BLL.CalculoHoras.OperacaoHoras('-', horasExtraNoturnaMin, faltaParaCompletar);
                                    }
                                }
                            }
                            else
                            {
                                if (limitesHorasDiarios[diaInt] != "--:--")
                                {
                                    AplicaPercentualBancoHora(ref somaHoras, ref DebitoBH, objBancoHoras, percentuais[diaInt]);
                                    CreditoBH = somaHoras;
                                }
                                horasExtrasDiurnaMin = 0;
                                horasExtraNoturnaMin = 0;
                            }
                        }
                    }
                }
                else
                {
                    if (objBancoHoras.ExtraPrimeiro == 0 && objBancoHoras.ContabilizarCreditos == true)
                    {
                        AplicaPercentualBancoHora(ref CreditoBH, ref DebitoBH, objBancoHoras, percentuais[diaInt]);
                        horasExtrasDiurnaMin = 0;
                        horasExtraNoturnaMin = 0;
                        horasExtraNoturnaMin = 0;
                    }
                }

                if ((objBancoHoras.ContAtrasosSaidasAntec == true && objBancoHoras.ContabilizarFaltas == true) ||
                    (objBancoHoras.ContAtrasosSaidasAntec == true && horasTrabalhadasMin > 0 && objBancoHoras.ContabilizarFaltas == false && horasTrabalhadasMin > 0) ||
                    (objBancoHoras.ContAtrasosSaidasAntec == false && horasTrabalhadasMin <= 0 && objBancoHoras.ContabilizarFaltas == true))
                {
                    horasFaltasMin = 0;
                    horasFaltaNoturnaMin = 0;
                }

                InclusaoBancoHoras(ref CreditoBH, ref DebitoBH, pIdFuncionario, objBancoHoras);
            }
            else
            {
                bancoHorasCre = "---:--";
                bancoHorasDeb = "---:--";
                InclusaoBancoHoras(ref CreditoBH, ref DebitoBH, pIdFuncionario, objBancoHoras);
            }
            return true;
        }

        private bool CalculoNaoEntraEmBanco(int pIdFuncionario, Modelo.BancoHoras objBancoHoras, out int CreditoBH, out int DebitoBH)
        {
            //Zera o campo de exportação de horas extras noturnas do banco
            expHorasExtraNot = "--:--";
            CreditoBH = 0;
            DebitoBH = 0;
            InclusaoBancoHoras(ref CreditoBH, ref DebitoBH, pIdFuncionario, objBancoHoras);
            return true;
        }

        private void AjustaLimite(int CreditoBH, int TotalExcedente, ref int limite)
        {
            //Encontra a valor que pode ficar no credito
            CreditoBH -= TotalExcedente;
            CreditoBH = CreditoBH < 0 ? 0 : CreditoBH;
            //Se for maior que o limite anterior, sobrepoe o limete para o menor
            if (limite > CreditoBH)
            {
                limite = CreditoBH;
            }
        }

        private bool VerificaLimeteSemanal(Modelo.BancoHoras objBancoHoras, DateTime dataCorrente, int saldoDia, int diaDsr, int? diaInt, int pIdFuncionario, out int TotalExcedenteSemanaMin, ICollection<Modelo.Marcacao> marcacoes, ref int limite)
        {
            int limiteSemana = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.LimiteBancoHorasSemanal);
            if (!String.IsNullOrEmpty(objBancoHoras.LimiteBancoHorasSemanal) && limiteSemana > 0 && saldoDia > 0) // Se utiliza bando de horas semanal, valida os saldo da semana
            {
                List<Modelo.Marcacao> marcsFuncs = marcacoes.Where(w => w.Idfuncionario == pIdFuncionario).ToList(); // Marcacoes do funcionário que foram calculadas nesse recalculo
                DateTime inicioMarca = data; // se for a primera iteração do recalculo significa que a data inicial é essa
                if (marcsFuncs != null && marcsFuncs.Count() > 0) // caso não seja aa primeira iteração pego como data inicial a menor data já calculada.
                {
                    inicioMarca = marcsFuncs.Min(m => m.Data);
                }
                // pega o dia que inicia a semana baseada na data corrente e no dia do DSR
                DateTime inicioSemana = dataCorrente.GetInicioSemanaBasedadoDSR(Modelo.Utils.DiasSemana.DiaPontoToDayOfWeek(dsr));
                int saldoSemana = saldoDia; // agrega o calculo do que foi calculado no dia

                int saldoSemanaNaoCarregado = 0;
                if (inicioMarca > inicioSemana) // Se o inicio da semana é menor que a data inicial, precisamos carregar as marcações desde o início para pegar o saldo da semana que não foi carregado por esse recalculo
                {
                    List<Modelo.Marcacao> marcsSemana = new List<Modelo.Marcacao>();
                    //Carrega do banco ou memória, essa lógica abaixo é responsavel para que não va de dia em dia buscar os dados em banco, carrega apenas uma vez
                    if (saldoBancoHorasSemanalNaoCarregado != null && saldoBancoHorasSemanalNaoCarregado.ContainsKey(pIdFuncionario))
                    {
                        saldoSemanaNaoCarregado = (int)saldoBancoHorasSemanalNaoCarregado[pIdFuncionario];
                    }
                    else
                    {
                        marcsSemana = bllMarcacao.GetPorFuncionario(pIdFuncionario, inicioSemana, inicioMarca.AddDays(-1), true);

                        //Soma ao saldo da semana, os dias que não foram carregados para o calculo, mas interferem no saldo da semana
                        if (marcsSemana != null && marcsSemana.Count() > 0)
                            saldoSemanaNaoCarregado = marcsSemana.Sum(m => Modelo.cwkFuncoes.ConvertHorasMinuto(m.Bancohorascre) - Modelo.cwkFuncoes.ConvertHorasMinuto(m.Bancohorasdeb));

                        if (saldoBancoHorasSemanalNaoCarregado == null)
                            saldoBancoHorasSemanalNaoCarregado = new Hashtable();
                        saldoBancoHorasSemanalNaoCarregado.Add(pIdFuncionario, saldoSemanaNaoCarregado);
                    }
                }
                saldoSemana += saldoSemanaNaoCarregado;

                //Soma ao saldo da semana o saldo já recalculado por esse recalculo
                List<Modelo.Marcacao> marcsCarregadasSemana = marcsFuncs.Where(w => w.Data >= inicioSemana && w.Data <= dataCorrente.AddDays(-1)).ToList();
                if (marcsCarregadasSemana != null && marcsCarregadasSemana.Count() > 0)
                {
                    saldoSemana += marcsCarregadasSemana.Sum(m => Modelo.cwkFuncoes.ConvertHorasMinuto(m.Bancohorascre) - Modelo.cwkFuncoes.ConvertHorasMinuto(m.Bancohorasdeb));
                }
                if (limite > limiteSemana || limite == -1)
                {
                    limite = limiteSemana;
                }
                TotalExcedenteSemanaMin = saldoSemana - limiteSemana;
                TotalExcedenteSemanaMin = TotalExcedenteSemanaMin < 0 ? TotalExcedenteSemanaMin = 0 : TotalExcedenteSemanaMin;
                return (TotalExcedenteSemanaMin > 0);
            }
            TotalExcedenteSemanaMin = 0;
            return false;
        }

        private bool VerificaExcedeuLimiteMensal(Modelo.BancoHoras objBancoHoras, int CreditoBH, int? diaInt, int pIdFuncionario, out int TotalExcedenteMensalMin, ICollection<Modelo.Marcacao> marcacoes, ref int limite)
        {
            int LimiteMesAcumulado = 0;
            int LimiteMesParametro = 0;

            try
            {
                if (diaInt.HasValue)
                {
                    LimiteMesAcumulado = bllInclusaoBanco.getCreditoPeriodoAcumuladoMesPDia(idFuncionario, data, Modelo.cwkFuncoes.Dia(data));

                    switch (diaInt.Value)
                    {
                        case 1:
                            LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.limitehorasDiarioMensal_1);
                            break;
                        case 2:
                            LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.limitehorasDiarioMensal_2);
                            break;
                        case 3:
                            LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.limitehorasDiarioMensal_3);
                            break;
                        case 4:
                            LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.limitehorasDiarioMensal_4);
                            break;
                        case 5:
                            LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.limitehorasDiarioMensal_5);
                            break;
                        case 6:
                            LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.limitehorasDiarioMensal_6);
                            break;
                        case 7:
                            LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.limitehorasDiarioMensal_7);
                            break;
                        case 8:
                            LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.limitehorasDiarioMensal_8);
                            break;
                        case 9:
                            LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(objBancoHoras.limitehorasDiarioMensal_9);
                            break;
                    }

                    if ((LimiteMesParametro > 0) && (CreditoBH > 0))
                    {
                        if ((limite > LimiteMesParametro || limite == -1))
                        {
                            limite = LimiteMesParametro;
                        }
                        TotalExcedenteMensalMin = (CreditoBH + LimiteMesAcumulado) - LimiteMesParametro;
                        return (LimiteMesParametro != 0 && TotalExcedenteMensalMin > 0);
                    }
                    else
                    {
                        TotalExcedenteMensalMin = 0;
                    }
                }
                else
                {
                    if (CreditoBH > 0)
                    {
                        LimiteMesAcumulado = bllInclusaoBanco.getCreditoPeriodoAcumuladoMes(idFuncionario, data);

                        if (objBancoHoras.BancoHorasDiarioMensal)
                        {
                            string limiteAcmMensal = objBancoHoras.LimiteHorasBancoHorasDiarioMensal;
                            LimiteMesParametro = Modelo.cwkFuncoes.ConvertHorasMinuto(limiteAcmMensal);
                            if ((limite > LimiteMesParametro || limite == -1) && limiteAcmMensal != "--:--" && !String.IsNullOrEmpty(limiteAcmMensal))
                            {
                                limite = LimiteMesParametro;
                            }
                        }
                        else
                        {
                            LimiteMesParametro = 0;
                        }


                        TotalExcedenteMensalMin = (CreditoBH + LimiteMesAcumulado) - LimiteMesParametro;
                        return (LimiteMesParametro != 0 && TotalExcedenteMensalMin > 0);
                    }
                    else
                    {
                        TotalExcedenteMensalMin = 0;
                    }
                }
            }
            catch (Exception)
            {
                TotalExcedenteMensalMin = 0;
            }

            return false;
        }

        private void AplicaPercentualBancoHora(ref int creditoBH, ref int debitoBH, Modelo.BancoHoras objBancoHoras, int percentual)
        {
            if (objBancoHoras.BancoHorasPorPercentual && objBancoHoras.ExtraPrimeiro == 0)
            {
                if (feriadoParcial)
                {
                    int horasComAcrescimo = horasTrabalhadasDentroFeriadoParcialDiurna + horasTrabalhadasDentroFeriadoParcialNoturna;
                    int horasSemAcrescimo = creditoBH - horasComAcrescimo;
                    horasComAcrescimo = horasComAcrescimo + (horasComAcrescimo * percentual / 100);
                    creditoBH = horasComAcrescimo + horasSemAcrescimo;
                    if (creditoBH >= debitoBH)
                    {
                        creditoBH -= debitoBH;
                        debitoBH = 0;
                    }
                    else
                    {
                        debitoBH -= creditoBH;
                        creditoBH = 0;
                    }
                }
                else
                {
                    int hora = creditoBH + (creditoBH * percentual / 100);
                    creditoBH = hora;
                }
            }
        }

        #region regras

        /// <summary>
        /// Método que calcula as horas trabalhadas. Pega todas as entradas e saídas validas e chama o método
        /// que separa as quantidades de horas noturnas e diurnas.
        /// </summary>
        public void CalculaHorasTrabalhadas(int[] Entrada, int[] Saida)
        {
            int HoraDiurna = 0;
            int HoraNoturna = 0;

            BLL.CalculoHoras.QtdHorasDiurnaNoturna(Entrada, Saida, inicioAdNoturno, fimAdNoturno, toleranciaAdicionalNoturno, ref HoraDiurna, ref HoraNoturna);

            horasTrabalhadasMin = HoraDiurna;
            horasTrabalhadasNoturnasMin = HoraNoturna;
            horasAdicionalNoturno = HoraNoturna;
        }

        public static string CalculaTotalIntervalo(int[] Entrada, int[] Saida)
        {
            int total = 0;
            for (int i = 0; i < 7; i++)
            {
                total += CalculaTotalIntervaloDasBatidas(Entrada, Saida, i);
            }
            return Modelo.cwkFuncoes.ConvertMinutosHora(Math.Abs(total));
        }

        public static string CalculaTotalHorasAlmoco(int[] Entradas, int[] Saidas, bool bConsideraCafe)
        {
            int total = 0;
            if (bConsideraCafe)
            {
                if ((Entradas[2] != -1) && (Saidas[1] != -1))
                {
                    total = Entradas[2] - Saidas[1];
                    return Modelo.cwkFuncoes.ConvertMinutosHora(Math.Abs(total));
                }
            }
            else
            {
                if ((Entradas[1] != -1) && (Saidas[0] != -1))
                {
                    total = Entradas[1] - Saidas[0];
                    return Modelo.cwkFuncoes.ConvertMinutosHora(Math.Abs(total));
                }
            }
            return Modelo.cwkFuncoes.ConvertMinutosHora(Math.Abs(total));
        }

        private static int CalculaTotalIntervaloDasBatidas(int[] Entrada, int[] Saida, int contador)
        {
            int total = 0;
            if ((Saida[contador] > 0) && (Entrada[contador + 1] > 0))
            {
                total = Entrada[contador + 1] - Saida[contador];
            }
            return total;
        }

        /// <summary>
        /// Método que calcula as horas trabalhadas. Pega todas as entradas e saídas validas e chama o método
        /// que retorna o total de horas trabalhadas saidas - entradas.
        /// </summary>
        public void CalculaTotalHorasTrabalhadas(int[] Entrada, int[] Saida)
        {
            totalHorasTrabalhadas = CalculaTotalHorasTrabalhadas(Entrada, Saida, data, false);
        }

        public void CalculaHorasInItinere(int[] Entrada, int[] Saida)
        {
            idaInItinere = 0;
            voltaInItine = 0;

            if (calcularInItinere && batidasValidasParaIntinere)
            {
                int e = Entrada[0];
                int s = Saida[0];
                if (e >= 0 && s >= 0)
                {
                    idaInItinere = BLL.CalculoHoras.QtdHoras(Entrada[0], Saida[0]);
                }

                e = Entrada.Where(w => w != -1).Last();
                s = Saida.Where(w => w != -1).Last();
                if (e >= 0 && s >= 0)
                {
                    voltaInItine = BLL.CalculoHoras.QtdHoras(e, s);
                }
            }
        }

        public static string CalculaTotalHorasTrabalhadas(int[] Entrada, int[] Saida, DateTime data, bool jornada24)
        {
            int total = CalculaMarcacao.CalculaTotalHorasTrabalhadasMin(Entrada, Saida, data, jornada24);
            return Modelo.cwkFuncoes.ConvertMinutosHora(2, Math.Abs(total));
        }

        public static int CalculaTotalHorasTrabalhadasMin(int[] Entrada, int[] Saida, DateTime data, bool jornada24)
        {
            int total = 0;
            int ind = 0;
            if (Entrada.Count() > Saida.Count())
                ind = Entrada.Count();
            else ind = Saida.Count();
            for (int i = 0; i < ind; i++)
            {
                if (Entrada[i] >= 0)
                {
                    if (Saida[i] >= 0)
                    {
                        total += BLL.CalculoHoras.QtdHoras(Entrada[i], Saida[i]);
                    }
                    else
                    {
                        #region Old method to calculate hours
                        //int agora = Modelo.cwkFuncoes.ConvertHorasMinuto(DateTime.Now.ToShortTimeString());

                        ////Se a marcação é do dia corrente ou se é jornada que passa da meia noite
                        //if (Convert.ToDateTime(data).ToShortDateString() == DateTime.Now.ToShortDateString() || (jornada24 && Convert.ToDateTime(data).ToShortDateString() == DateTime.Now.AddDays(-1).ToShortDateString()))
                        //{
                        //    // Se jornada passa da meia noite adiciono mais 24 horas
                        //    if (jornada24 && Convert.ToDateTime(data).ToShortDateString() == DateTime.Now.AddDays(-1).ToShortDateString())
                        //    {
                        //        agora += 1440;
                        //    }
                        //    total += agora - Entrada[i];
                        //}
                        //else
                        //{
                        //    // Se jornada passa da meia noite e é referente a um dia que já passou considero 48 horas (meia noite do outro dia ao qual ele entrou.)
                        //    if (jornada24)
                        //    {
                        //        agora = 2880;
                        //    }
                        //    else
                        //    {
                        //        agora = 1440;
                        //    }
                        //    total += agora - Entrada[i];
                        //}
                        #endregion
                        total += 0;
                    }
                }
            }
            return total;
        }

        private bool CalculaHoraExtraFalta(string abonoD, string abonoN, int adicionalNoturno, bool feriadoProximoDia)
        {
            bool feriadoNoDia = (idFeriado != null && naoConsiderarFeriado == 0);
            if (feriadoNoDia && separaExtraFalta == 1)
                separaExtraFalta = 0;

            int[] HoraEntrada = new int[] { -1, -1, -1, -1 };
            int[] HoraSaida = new int[] { -1, -1, -1, -1 };

            int CargaHorariaD = 0;
            int CargaHorariaN = 0;
            int CargaHorariaM = 0;

            int HoraD = horasTrabalhadasMin;
            int HoraN = horasTrabalhadasNoturnasMin;

            //Caso o horário seja flexível e não exista horário detalhe
            //a função retorna false
            if (tipoHorario == 2 && marcaCargaHorariaMistaHD == null && idJornadaAlternativa == null)
            {
                return false;
            }

            int Marcacargahorariamista = 0;
            //Verifica Jornada Alternativa
            if (!this.VerificaJornadaAlternativa(ref HoraEntrada, ref HoraSaida, ref CargaHorariaD, ref CargaHorariaN, ref CargaHorariaM, ref bCafe, ref Marcacargahorariamista))
            {
                SetaVetoresHorarioDetalhe(ref HoraEntrada, ref HoraSaida, ref CargaHorariaD, ref CargaHorariaN, ref CargaHorariaM, ref bCafe, ref Marcacargahorariamista);
            }

            #region HorarioCafe
            if (bCafe && naoConsiderarCafe == 0)
            {
                BLL.Horario.CalculaCafe(HoraEntrada, HoraSaida, habilitaPeriodo01, habilitaPeriodo02, ref HoraD, ref HoraN);
            }
            #endregion

            int[] MarSaida = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
            int[] MarEntrada = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };

            this.GetEntradasSaidasValidas(ref MarEntrada, ref MarSaida);

            #region Cálculo Tolerância
            int FaltasToleradasD = 0;
            int FaltasToleradasN = 0;
            int ExtrasToleradasD = 0;
            int ExtrasToleradasN = 0;
            bool toleranciaPorBatida = false;
            if (flagFolga.GetValueOrDefault() != 1 && folgaMarcacao != 1 && (idFeriado == null || feriadoParcial == true || naoConsiderarFeriado == 1))
            {
                if (separaExtraFalta == 0)
                    //o calculo de horas toleradas para separa_extra_e_falta será executado dentro da rotina separa extra e falta 
                    //(esse calculo é utilizado apenas para carga horaria mista e normal) --BLL.CalculoHoras.SeparaExtraFalta
                    CalculaHorasToleradas(HoraEntrada, HoraSaida, MarSaida, MarEntrada, out toleranciaPorBatida, out FaltasToleradasD, out FaltasToleradasN, out ExtrasToleradasD, out ExtrasToleradasN);
            }
            else
            {
                tHoraExtraMin = 0;
                tHoraFaltaMin = 0;
                CargaHorariaD = 0;
                CargaHorariaN = 0;
                CargaHorariaM = 0;
            }
            #endregion

            int HoraExtraD = 0;
            int HoraExtraN = 0;
            int HoraFaltaD = 0;
            int HoraFaltaN = 0;
            string Ocorrencia = "";

            //WNO - Separa Extra falta tem prioridade sobre as outras rotinas, pois está setado por marcação.
            if (separaExtraFalta == 1 && contabilizarjornada == 0)
            {
                bool calculouToleranciaBatidaIntervalo = false;
                toleranciaPorBatida = ToleranciaPorBatida();
                FaltasToleradasD = FaltasToleradasN = ExtrasToleradasD = ExtrasToleradasN = 0;
                BLL.CalculoHoras.SeparaExtraFalta(data, diaInt, legenda, HoraEntrada, HoraSaida, CargaHorariaD, CargaHorariaN, inicioAdNoturno, fimAdNoturno, false, MarEntrada, MarSaida, "--:--", false, abonoD, abonoN, 0, ref HoraD, ref HoraN, out HoraExtraD, out HoraFaltaD, out HoraExtraN, out HoraFaltaN, out Ocorrencia);
                BLL.CalculoHoras.CalculaCompensacao(horasCompensarMin, ref HoraExtraD, ref HoraExtraN, ref horasCompensadasMin, ref horasCompensadas, ref legenda, ref LegendasConcatenadas, ref Ocorrencia);
                if ((HoraFaltaD + HoraFaltaN) <= tHoraFaltaMin || tHoraFaltaMin == null)
                {
                    if (toleranciaPorBatida)
                    {
                        calculouToleranciaBatidaIntervalo = CalculaToleranciaBatidaIntervalo(HoraEntrada, HoraSaida, MarSaida, MarEntrada, ref toleranciaPorBatida, ref FaltasToleradasD, ref FaltasToleradasN, ref ExtrasToleradasD, ref ExtrasToleradasN);
                        HoraD += FaltasToleradasD;
                        HoraFaltaD -= FaltasToleradasD;
                        HoraN += FaltasToleradasN;
                        HoraFaltaN -= FaltasToleradasN;
                    }
                    else
                    {
                        HoraD += HoraFaltaD;
                        HoraN += HoraFaltaN;
                        HoraFaltaD = 0;
                        HoraFaltaN = 0;
                    }
                }

                if ((HoraExtraD + HoraExtraN + horasCompensadasMin) > 0)
                {
                    HoraD -= HoraExtraD + horasCompensadasMin;
                    if (HoraD < 0)
                    {
                        HoraN -= Math.Abs(HoraD);
                    }
                    HoraN -= HoraExtraN;
                }


                if ((HoraExtraD + HoraExtraN) <= tHoraExtraMin || tHoraExtraMin == null)
                {
                    if (toleranciaPorBatida)
                    {
                        if (!calculouToleranciaBatidaIntervalo)
                            calculouToleranciaBatidaIntervalo = CalculaToleranciaBatidaIntervalo(HoraEntrada, HoraSaida, MarSaida, MarEntrada, ref toleranciaPorBatida, ref FaltasToleradasD, ref FaltasToleradasN, ref ExtrasToleradasD, ref ExtrasToleradasN);

                        HoraExtraD -= ExtrasToleradasD;
                        HoraExtraN -= ExtrasToleradasN;
                    }
                    else
                    {
                        if ((HoraExtraD + HoraExtraN) <= tHoraExtraMin)
                        {
                            HoraExtraD = 0;
                            HoraExtraN = 0;
                        }
                    }
                }
            }
            //Colocado para verificar a var e não a var do objHorario pois a JornadaAlternativa pode ser diferente
            //CRNC - 09/01/2010
            else if (Marcacargahorariamista == 1)
            {
                BLL.CalculoHoras.CalculaCargaHorariaMista2(CargaHorariaM, CargaHorariaD, CargaHorariaN, toleranciaPorBatida, tHoraExtraMin, ExtrasToleradasD, ExtrasToleradasN, tHoraFaltaMin, FaltasToleradasD, FaltasToleradasN, inicioAdNoturno, fimAdNoturno, MarEntrada, MarSaida, ref HoraD, ref HoraN, out HoraExtraD, out HoraFaltaD, out HoraExtraN, out HoraFaltaN, ref Ocorrencia, adicionalNoturno, horasCompensarMin, ref horasCompensadasMin, ref horasCompensadas, ref legenda, ref LegendasConcatenadas);
                totalizaCargaHoraria = CargaHorariaM;
            }
            else
            {
                /* Quando Parâmetro bConsiderarHEFeriadoPHoraNoturna for verdadeiro e o próximo dia for um feriado 
                 * não considera as horas trabalhadas após 0:00, pois elas entrarão no feriado como hora extra 
                 * pelo método CalculoDeJornadaNoturnaComFeriado */

                if (bConsiderarHEFeriadoPHoraNoturna && feriadoProximoDia)
                {
                    //Carga Horária Diurna e Noturna separadas antes e depois da meia noite
                    BLL.CalculoHoras.CargaHorariaAntesPosMeiaNoite(inicioAdNoturno, fimAdNoturno, tHoraExtraMin, tHoraFaltaMin, HoraEntrada, HoraSaida, ref HoraD, ref HoraN, MarSaida, MarEntrada, ref HoraExtraD, ref HoraExtraN, ref HoraFaltaD, ref HoraFaltaN, ref Ocorrencia);
                }
                else
                {
                    BLL.CalculoHoras.CargaHoraria(CargaHorariaD, CargaHorariaN, toleranciaPorBatida, tHoraExtraMin, ExtrasToleradasD, ExtrasToleradasN, tHoraFaltaMin, FaltasToleradasD, FaltasToleradasN, ref HoraD, ref HoraN, out HoraExtraD, out HoraFaltaD, out HoraExtraN, out HoraFaltaN, out Ocorrencia, horasCompensarMin, ref horasCompensadasMin, ref horasCompensadas, ref legenda, ref LegendasConcatenadas);
                    totalizaCargaHoraria = CargaHorariaD + CargaHorariaN;
                }
            }


            if (calcularInItinere)
            {
                if ((idaInItinere > 0 || voltaInItine > 0) && batidasValidasParaIntinere)
                {
                    int intinereDentroJornada = (idaInItinere + voltaInItine) - (HoraExtraD + HoraExtraN);
                    intinereDentroJornada = intinereDentroJornada < 0 ? 0 : intinereDentroJornada;
                    int intinereForaJornada = (idaInItinere + voltaInItine) - intinereDentroJornada;
                    intinereForaJornada = intinereForaJornada < 0 ? 0 : intinereForaJornada;

                    InItinereHrsDentroJornada = Modelo.cwkFuncoes.ConvertMinutosHora2(2, intinereDentroJornada);
                    InItinereHrsForaJornada = Modelo.cwkFuncoes.ConvertMinutosHora2(2, intinereForaJornada);
                    InItinerePercDentroJornada = percentualDentroJornadaInItinere;
                    InItinerePercForaJornada = percentualForaJornadaInItinere;

                    if (HoraExtraD > 0 || HoraExtraN > 0)
                    {
                        if (idaInItinere > HoraExtraD)
                        {
                            idaInItinere -= HoraExtraD;
                            HoraExtraD = 0;
                        }
                        else
                        {
                            HoraExtraD -= idaInItinere;
                            idaInItinere = 0;
                            if (HoraExtraD > 0)
                            {
                                if (voltaInItine > HoraExtraD)
                                {
                                    voltaInItine -= HoraExtraD;
                                    HoraExtraD = 0;
                                }
                                else
                                {
                                    HoraExtraD -= voltaInItine;
                                    voltaInItine = 0;
                                }
                            }
                        }

                        if ((voltaInItine + idaInItinere) > HoraExtraN)
                        {
                            HoraExtraN = 0;
                        }
                        else
                        {
                            HoraExtraN -= (voltaInItine + idaInItinere);
                        }

                        if (HoraExtraD < 0)
                        {
                            HoraExtraD = 0;
                        }

                        if (HoraExtraN < 0)
                        {
                            HoraExtraN = 0;
                        }
                    }
                    int dentroJornadaCalcHorasTrab = intinereDentroJornada;
                    if (dentroJornadaCalcHorasTrab > 0 && (HoraD > 0 || HoraN > 0))
                    {
                        if (HoraD > 0 && dentroJornadaCalcHorasTrab > 0)
                        {
                            if (HoraD > dentroJornadaCalcHorasTrab)
                            {
                                HoraD -= dentroJornadaCalcHorasTrab;
                                dentroJornadaCalcHorasTrab = 0;
                            }
                            else
                            {
                                dentroJornadaCalcHorasTrab -= HoraD;
                                horasTrabalhadasMin = 0;
                            }
                        }


                        if (dentroJornadaCalcHorasTrab > 0 && HoraN > 0)
                        {
                            if (HoraN > dentroJornadaCalcHorasTrab)
                            {
                                HoraN -= dentroJornadaCalcHorasTrab;
                                dentroJornadaCalcHorasTrab = 0;
                            }
                            else
                            {
                                dentroJornadaCalcHorasTrab -= HoraN;
                                horasTrabalhadasNoturnasMin = 0;
                            }
                        }
                    }
                }
                else
                {
                    InItinereHrsDentroJornada = Modelo.cwkFuncoes.ConvertMinutosHora2(2, 0);
                    InItinereHrsForaJornada = Modelo.cwkFuncoes.ConvertMinutosHora2(2, 0);
                    InItinerePercDentroJornada = 0;
                    InItinerePercForaJornada = 0;
                }

                if ((HoraFaltaD + HoraFaltaN) > 0 && (!DescontarAtrasoInItinere || !DescontarFaltaInItinere))
                {
                    if (!DescontarFaltaInItinere && ((HoraFaltaD + HoraFaltaN) == (CargaHorariaD + CargaHorariaN) || (HoraFaltaD + HoraFaltaN) == (cargaHorariaMistaMin)))
                    {
                        HoraFaltaD = 0;
                        HoraFaltaN = 0;
                        Ocorrencia = Ocorrencia.Replace("Falta", "");
                    }
                    else if (!DescontarAtrasoInItinere)
                    {
                        HoraFaltaD = 0;
                        HoraFaltaN = 0;
                    }
                }
            }
            else
            {
                InItinereHrsDentroJornada = Modelo.cwkFuncoes.ConvertMinutosHora2(2, 0);
                InItinereHrsForaJornada = Modelo.cwkFuncoes.ConvertMinutosHora2(2, 0);
                InItinerePercDentroJornada = 0;
                InItinerePercForaJornada = 0;
            }



            horasTrabalhadasMin = HoraD;
            horasExtrasDiurnaMin = HoraExtraD;
            horasFaltasMin = HoraFaltaD;

            horasTrabalhadasNoturnasMin = HoraN;
            horasExtraNoturnaMin = HoraExtraN;
            horasFaltaNoturnaMin = HoraFaltaN;
            ocorrencia = Ocorrencia;

            return true;
        }

        private bool ToleranciaPorBatida()
        {
            return (tHoraFaltaEntradaMin > 0 || tHoraFaltaSaidaMin > 0 || tHoraExtraEntradaMin > 0 || tHoraExtraSaidaMin > 0 || tHoraExtraIntervaloMin > 0 || tHoraFaltaIntervaloMin > 0);
        }

        private void CalculaHorasToleradas(int[] HoraEntrada, int[] HoraSaida, int[] MarSaida, int[] MarEntrada, out bool toleranciaPorBatida, out int FaltasToleradasD, out int FaltasToleradasN, out int ExtrasToleradasD, out int ExtrasToleradasN)
        {
            FaltasToleradasD = 0;
            FaltasToleradasN = 0;
            ExtrasToleradasD = 0;
            ExtrasToleradasN = 0;
            toleranciaPorBatida = ToleranciaPorBatida();
            if (toleranciaPorBatida)
            {
                bool ultrapassouLimiteTolerancia = false;
                if (tHoraExtraMin > 0 || tHoraFaltaMin > 0)
                {
                    int totalTrabalhada = CalculaMarcacao.CalculaTotalHorasTrabalhadasMin(MarEntrada, MarSaida, data, false);
                    int totalCarga = CalculaMarcacao.CalculaTotalHorasTrabalhadasMin(HoraEntrada, HoraSaida, data, false);
                    int difTrab = totalTrabalhada - totalCarga;

                    //Caso seja diferença negativa deve ser tratado negativo
                    if (difTrab < 0 && Math.Abs(difTrab) <= tHoraFaltaMin * -1)
                    {
                        ultrapassouLimiteTolerancia = true;
                    }
                    else if (difTrab > 0 && Math.Abs(difTrab) >= tHoraExtraMin)
                    {
                        ultrapassouLimiteTolerancia = true;
                    }
                }

                if (!ultrapassouLimiteTolerancia)
                {
                    CalculaToleranciaBatidaIntervalo(HoraEntrada, HoraSaida, MarSaida, MarEntrada, ref toleranciaPorBatida, ref FaltasToleradasD, ref FaltasToleradasN, ref ExtrasToleradasD, ref ExtrasToleradasN);
                }
                else
                {
                    toleranciaPorBatida = false;
                }
            }
        }

        private bool CalculaToleranciaBatidaIntervalo(int[] HoraEntrada, int[] HoraSaida, int[] MarSaida, int[] MarEntrada, ref bool toleranciaPorBatida, ref int FaltasToleradasD, ref int FaltasToleradasN, ref int ExtrasToleradasD, ref int ExtrasToleradasN)
        {
            // primeiro valor é a saida, segunda entrada do intervalo, o terceiro valor é o tempo do intervalo
            Tuple<int, int, int> intervaloAlmocoRealizado = new Tuple<int, int, int>(0, 0, 0);
            CalculaToleranciaIntervaloAlmoco(HoraEntrada, HoraSaida, MarEntrada, MarSaida, ref toleranciaPorBatida, ref FaltasToleradasD, ref FaltasToleradasN, ref ExtrasToleradasD, ref ExtrasToleradasN, ref intervaloAlmocoRealizado);

            for (int i = 0; i < 4; i++)
            {
                int MarcacaoRegistro = MarEntrada[i];

                int adicionalMeiaNoiteJornada = 0, adicionalMeiaNoiteMarcacao = 0;
                if (i > 0)
                {
                    int MarcacaoRegistroAnt = MarEntrada[i - 1];

                    if (MarcacaoRegistro < MarcacaoRegistroAnt)
                    {
                        adicionalMeiaNoiteMarcacao = 1440;
                    }
                }
                // Não calcula se tem tolerancia por intervalo e caso seja entrada da volta do almoço
                if (MarcacaoRegistro != intervaloAlmocoRealizado.Item2 || intervaloAlmocoRealizado.Item3 == 0)
                {
                    int JornadaRegistro = HoraEntrada[i];
                    if (i > 0)
                    {
                        int entradaAnt = HoraEntrada[i - 1];
                        if (JornadaRegistro < entradaAnt)
                        {
                            adicionalMeiaNoiteJornada = 1440;
                        }
                    }
                    CalculaToleranciaRegistros(JornadaRegistro, MarcacaoRegistro, ref FaltasToleradasD, ref FaltasToleradasN, ref ExtrasToleradasD, ref ExtrasToleradasN, 0, adicionalMeiaNoiteJornada, adicionalMeiaNoiteMarcacao);
                }

                // Não calcula se tem tolerancia por intervalo e caso seja saida do almoço
                MarcacaoRegistro = MarSaida[i];

                if (i > 0)
                {
                    int MarcacaoRegistroAnt = i > 0 ? MarSaida[i - 1] : 0;

                    if (MarcacaoRegistro < MarcacaoRegistroAnt)
                    {
                        adicionalMeiaNoiteMarcacao = 1440;
                    }
                }

                if (MarcacaoRegistro != intervaloAlmocoRealizado.Item1 || intervaloAlmocoRealizado.Item3 == 0)
                {
                    int JornadaRegistro = HoraSaida[i];
                    if (i > 0)
                    {
                        int saidaAnt = HoraSaida[i - 1];
                        if (JornadaRegistro < saidaAnt)
                        {
                            adicionalMeiaNoiteJornada = 1440;
                        }
                    }
                    CalculaToleranciaRegistros(JornadaRegistro, MarcacaoRegistro, ref FaltasToleradasD, ref FaltasToleradasN, ref ExtrasToleradasD, ref ExtrasToleradasN, 1, adicionalMeiaNoiteJornada, adicionalMeiaNoiteMarcacao);
                }

            }
            return true;
        }

        private void CalculaToleranciaIntervaloAlmoco(int[] HoraEntrada, int[] HoraSaida, int[] MarEntrada, int[] MarSaida, ref bool toleranciaPorBatida, ref int FaltasToleradasD, ref int FaltasToleradasN, ref int ExtrasToleradasD, ref int ExtrasToleradasN, ref Tuple<int, int, int> intervaloAlmocoRealizado)
        {
            // Se existir tolerância por intervalo
            if (tHoraExtraIntervaloMin != null || tHoraFaltaIntervaloMin != null)
            {
                int intervaloAlmocoPrevisto = 0;
                if (bCafe && naoConsiderarCafe == 0)
                {
                    intervaloAlmocoPrevisto = CalculaMarcacao.CalculaTotalHorasTrabalhadasMin(new int[1] { HoraSaida[1] }, new int[1] { HoraEntrada[2] }, data, false);
                }
                else
                {
                    intervaloAlmocoPrevisto = CalculaMarcacao.CalculaTotalHorasTrabalhadasMin(new int[1] { HoraSaida[0] }, new int[1] { HoraEntrada[1] }, data, false);
                }
                // primeiro valor é a saida, segunda entrada do intervalo, o terceiro valor é o tempo do intervalo
                List<Tuple<int, int, int>> intervalosRealizados = new List<Tuple<int, int, int>>();
                for (int i = 1; i < 3; i++)
                {
                    int entrada = MarEntrada[i];
                    int saida = MarSaida[i - 1];
                    if (saida != -1 && entrada != -1)
                    {
                        int intervaloRealizado = CalculaMarcacao.CalculaTotalHorasTrabalhadasMin(new int[1] { saida }, new int[1] { entrada }, data, false);
                        intervalosRealizados.Add(new Tuple<int, int, int>(saida, entrada, intervaloRealizado));
                    }
                }

                if (intervalosRealizados.Count() > 0 && intervaloAlmocoPrevisto > 0)
                {
                    int intervaloAlmoco = 0;
                    if (intervalosRealizados.Count() == 1)
                    {
                        intervaloAlmoco = intervalosRealizados.Select(s => s.Item3).Max();
                    }
                    else
                    {
                        intervaloAlmoco = (int)Modelo.cwkFuncoes.NumeroProximo(intervalosRealizados.Select(s => (double)s.Item3).ToList(), (double)intervaloAlmocoPrevisto);
                    }

                    if (intervaloAlmoco > 0 && intervaloAlmocoPrevisto > 0)
                    {
                        int dif = intervaloAlmocoPrevisto - intervaloAlmoco;
                        intervaloAlmocoRealizado = intervalosRealizados.Where(s => s.Item3 == intervaloAlmoco).FirstOrDefault();
                        if ((dif < 0 && Math.Abs(dif) <= tHoraFaltaIntervaloMin) || // Se tolera a Falta )
                            (dif > 0 && Math.Abs(dif) <= tHoraExtraIntervaloMin)
                           )
                        {

                            int qtdAlmDiurno = 0;
                            int qtdAlmNoturno = 0;
                            BLL.CalculoHoras.QtdHorasDiurnaNoturna(new int[] { intervaloAlmocoRealizado.Item1 }, new int[] { intervaloAlmocoRealizado.Item2 }, inicioAdNoturno, fimAdNoturno, ref qtdAlmDiurno, ref qtdAlmNoturno);

                            if (dif < 0)
                            {
                                if (qtdAlmNoturno > qtdAlmDiurno)
                                    FaltasToleradasN = Math.Abs(dif);
                                else
                                    FaltasToleradasD = Math.Abs(dif);
                                toleranciaPorBatida = true;
                            }
                            else
                            {
                                if (qtdAlmNoturno > qtdAlmDiurno)
                                    ExtrasToleradasN = Math.Abs(dif);
                                else
                                    ExtrasToleradasD = Math.Abs(dif);
                                toleranciaPorBatida = true;
                            }
                        }
                    }
                }
            }
        }

        private void CalculaToleranciaRegistros(int jornada, int marcacao, ref int acumuladorFaltasToleradasD, ref int acumuladorFaltasToleradasN, ref int acumuladorExtrasToleradasD, ref int acumuladorExtrasToleradasN, int TipoEntradaSaida, int adicionalMeiaNoiteJornada, int adicionalMeiaNoiteMar)
        {
            int dif = 0;
            int? tolerancia = 0;
            if (jornada >= 0)
            {
                int falta = 0;
                int extra = 0;
                int[] e = new int[1];
                int[] s = new int[1];

                if (jornada == 0)
                {
                    var tamanho = marcacao.ToString();
                    if (tamanho.Length > 2)
                    {
                        jornada = 1440;
                    }
                }

                //Se entrada
                if (TipoEntradaSaida == 0)
                {
                    dif = (adicionalMeiaNoiteJornada + jornada) - (adicionalMeiaNoiteMar + marcacao);
                    CalculaExtraFaltaDif(dif, ref falta, ref extra);
                    if (falta > 0)
                    {
                        tolerancia = tHoraFaltaEntradaMin;
                        e[0] = jornada;
                        s[0] = marcacao;
                    }
                    else
                    {
                        tolerancia = tHoraExtraEntradaMin;
                        e[0] = marcacao;
                        s[0] = jornada;
                    }
                }
                else
                {
                    dif = (adicionalMeiaNoiteMar + marcacao) - (adicionalMeiaNoiteJornada + jornada);
                    CalculaExtraFaltaDif(dif, ref falta, ref extra);
                    if (falta > 0)
                    {
                        tolerancia = tHoraFaltaSaidaMin;
                        e[0] = marcacao;
                        s[0] = jornada;
                    }
                    else
                    {
                        tolerancia = tHoraExtraSaidaMin;
                        e[0] = jornada;
                        s[0] = marcacao;
                    }
                }

                if ((tolerancia != null && falta > 0 && falta <= tolerancia.GetValueOrDefault()))
                {
                    int HoraDiurna = 0;
                    int HoraNoturna = 0;
                    BLL.CalculoHoras.QtdHorasDiurnaNoturna(e, s, inicioAdNoturno, fimAdNoturno, ref HoraDiurna, ref HoraNoturna);
                    acumuladorFaltasToleradasD += HoraDiurna;
                    acumuladorFaltasToleradasN += HoraNoturna;
                }
                else if (tolerancia != null && extra > 0 && extra <= tolerancia.GetValueOrDefault())
                {
                    int HoraDiurna = 0;
                    int HoraNoturna = 0;
                    BLL.CalculoHoras.QtdHorasDiurnaNoturna(e, s, inicioAdNoturno, fimAdNoturno, ref HoraDiurna, ref HoraNoturna);
                    acumuladorExtrasToleradasD += HoraDiurna;
                    acumuladorExtrasToleradasN += HoraNoturna;
                }
            }



            //if (jornada >= 0)
            //{
            //    //Diferença entre a marcação e a jornada
            //    dif = marcacao - jornada;
            //    int tolerancia = 0;
            //    //Se a diferença entre a marcação e a jornada for maior que 0 na entrada ou se for menor que 0 na saída significa que é falta
            //    if ((dif > 0 && TipoEntradaSaida == 0) || (dif < 0 && TipoEntradaSaida == 1))
            //    {
            //        if (TipoEntradaSaida == 0) 
            //            tolerancia = tHoraFaltaEntradaMin; 
            //        else
            //            tolerancia = tHoraFaltaSaidaMin;

            //        if (jornada >= inicioAdNoturno || jornada <= fimAdNoturno)
            //        {
            //            CalculoToleranciaRegistros(Math.Abs(dif), ref acumuladorFaltasToleradasN, tolerancia);
            //        }
            //        else
            //        {
            //            CalculoToleranciaRegistros(Math.Abs(dif), ref acumuladorFaltasToleradasD, tolerancia);
            //        }

            //    }
            //    else
            //    {
            //        if (TipoEntradaSaida == 0)
            //            tolerancia = tHoraExtraEntradaMin;
            //        else
            //            tolerancia = tHoraExtraSaidaMin;
            //        if (jornada >= inicioAdNoturno || jornada <= fimAdNoturno)
            //        {
            //            CalculoToleranciaRegistros(Math.Abs(dif), ref acumuladorExtrasToleradasN, tolerancia);
            //        }
            //        else
            //        {
            //            CalculoToleranciaRegistros(Math.Abs(dif), ref acumuladorExtrasToleradasD, tolerancia);
            //        }
            //    }
            //}
        }

        private static void CalculaExtraFaltaDif(int dif, ref int falta, ref int extra)
        {
            if (dif < 0) // Falta
                falta = Math.Abs(dif);
            else
                extra = Math.Abs(dif);
        }

        private bool BuscaExistenciaFeriado(DateTime data, int idEmpresa, int idDepartamento)
        {
            BLL.Feriado bllFeriado = new BLL.Feriado(ConnectionString, UsuarioLogado);
            bool feriadoNoDia = bllFeriado.PossuiRegistro(data, idEmpresa, idDepartamento);
            return feriadoNoDia;
        }

        /// <summary>
        /// Seta as variáveis com os dados do horario detalhe. Só deve ser chamado se o horario detalhe existir
        /// </summary>
        /// <param name="HoraEntrada"></param>
        /// <param name="HoraSaida"></param>
        /// <param name="CargaHorariaD"></param>
        /// <param name="CargaHorariaN"></param>
        /// <param name="CargaHorariaM"></param>
        /// <param name="bCafe"></param>
        /// <param name="Marcacargahorariamista"></param>
        private void SetaVetoresHorarioDetalhe(ref int[] HoraEntrada, ref int[] HoraSaida, ref int CargaHorariaD, ref int CargaHorariaN, ref int CargaHorariaM, ref bool bCafe, ref int Marcacargahorariamista)
        {
            bCafe = this.SetaCafé();
            //Se for um feriado parcial, o sistema acerta o horário de trabalho de acordo com o feriado parcial, ou seja, remove as horas a serem trabalhadas dentro do perído do feriado parcial.
            if (feriadoParcial)
            {
                int[] entrada = new int[4] { entrada_1MinHD, entrada_2MinHD, entrada_3MinHD, entrada_4MinHD };
                int[] saida = new int[4] { saida_1MinHD, saida_2MinHD, saida_3MinHD, saida_4MinHD };
                RemoveRegistrosDentroFeriadoParcial(ref entrada, ref saida);
                entrada_1MinHD = entrada[0];
                entrada_2MinHD = entrada[1];
                entrada_3MinHD = entrada[2];
                entrada_4MinHD = entrada[3];
                saida_1MinHD = saida[0];
                saida_2MinHD = saida[1];
                saida_3MinHD = saida[2];
                saida_4MinHD = saida[3];
            }

            HoraEntrada[0] = entrada_1MinHD;
            HoraEntrada[1] = entrada_2MinHD;
            HoraEntrada[2] = entrada_3MinHD;
            HoraEntrada[3] = entrada_4MinHD;

            HoraSaida[0] = saida_1MinHD;
            HoraSaida[1] = saida_2MinHD;
            HoraSaida[2] = saida_3MinHD;
            HoraSaida[3] = saida_4MinHD;

            if (marcaCargaHorariaMistaHD == null)
                marcaCargaHorariaMistaHD = 0;

            BLL.CalculoHoras.QtdHorasDiurnaNoturna(HoraEntrada, HoraSaida, inicioAdNoturno, fimAdNoturno, ref CargaHorariaD, ref CargaHorariaN);
            if (bCafe && naoConsiderarCafe == 0)
            {
                BLL.Horario.CalculaCafe(HoraEntrada, HoraSaida, habilitaPeriodo01, habilitaPeriodo02, ref CargaHorariaD, ref CargaHorariaN);
            }
            CargaHorariaM = CargaHorariaD + CargaHorariaN;
            Marcacargahorariamista = marcaCargaHorariaMistaHD.Value; //CRNC - 09/01/2010   
        }

        private void RemoveRegistrosDentroFeriadoParcial(ref int[] Entrada, ref int[] Saida)
        {
            List<int> entradaAlt = new List<int>();
            List<int> saidaAlt = new List<int>();

            int posicaoNova = 0;
            for (int i = 0; i < Entrada.Where(s => s != -1).Count(); i++)
            {
                if (EstaContidoNoFeriadoParcial(Entrada[i], feriadoParcialInicioMin, feriadoParcialFimMin) && EstaContidoNoFeriadoParcial(Saida[i], feriadoParcialInicioMin, feriadoParcialFimMin))
                {
                    continue;
                }
                else if (EstaContidoNoFeriadoParcial(Entrada[i], feriadoParcialInicioMin, feriadoParcialFimMin) && !EstaContidoNoFeriadoParcial(Saida[i], feriadoParcialInicioMin, feriadoParcialFimMin))
                {
                    entradaAlt.Add(feriadoParcialFimMin);
                    saidaAlt.Add(Saida[i]);
                    posicaoNova++;
                }
                else if (!EstaContidoNoFeriadoParcial(Entrada[i], feriadoParcialInicioMin, feriadoParcialFimMin) && EstaContidoNoFeriadoParcial(Saida[i], feriadoParcialInicioMin, feriadoParcialFimMin))
                {
                    entradaAlt.Add(Entrada[i]);
                    saidaAlt.Add(feriadoParcialInicioMin);
                    posicaoNova++;
                }
                else if (Entrada[i] < feriadoParcialInicioMin && Saida[i] > feriadoParcialFimMin)
                {
                    entradaAlt.Add(Entrada[i]);
                    saidaAlt.Add(feriadoParcialInicioMin);
                    posicaoNova++;
                    entradaAlt.Add(feriadoParcialFimMin);
                    saidaAlt.Add(Saida[i]);
                    posicaoNova++;
                }
                else
                {
                    entradaAlt.Add(Entrada[i]);
                    saidaAlt.Add(Saida[i]);
                    posicaoNova++;
                }
            }
            for (int i = 0; i < Entrada.Length; i++)
            {
                if (entradaAlt.Count() > i)
                {
                    Entrada[i] = entradaAlt.ElementAtOrDefault(i);
                    Saida[i] = saidaAlt.ElementAtOrDefault(i);
                }
                else
                {
                    Entrada[i] = -1;
                    Saida[i] = -1;
                }
            }
        }

        private static bool EstaContidoNoFeriadoParcial(int hora, int feriadoParcialInicioMin, int feriadoParcialFimMin)
        {
            return (hora >= feriadoParcialInicioMin && hora <= feriadoParcialFimMin);
        }

        /// <summary>
        /// Retorna se aquele dia é café ou não, de acordo com o horário detalhe
        /// </summary>
        /// <returns>true = possui café; false = não possui café</returns>
        private bool SetaCafé()
        {
            switch (diaInt)
            {
                case 1:
                    return dias_cafe_1 == 1;
                case 2:
                    return dias_cafe_2 == 1;
                case 3:
                    return dias_cafe_3 == 1;
                case 4:
                    return dias_cafe_4 == 1;
                case 5:
                    return dias_cafe_5 == 1;
                case 6:
                    return dias_cafe_6 == 1;
                case 7:
                    return dias_cafe_7 == 1;
            }
            return false;
        }

        private void RegrasParaDiurno(int abono, bool semcalc, string abonoD, int horarioD, int horarioN, int horarioM, bool semAbono)
        {
            bool possuiAbono = LegendasConcatenadas.Split(',').Where(s => s == "A").Count() > 0;
            #region Novo calculo abono total e parcial para todos os tipos de horário (Misto, normal e separa e falta)
            if (legenda == "F" && possuiAbono && abono != 0)
            {
                abono = 0;
            }

            if (possuiAbono && abono == 2) //Abono total
            {
                if (separaExtraFalta != 1 && contabilizarjornada == 0) // Apenas retira as horas extras caso não seja para separar extra e falta
                {
                    horasExtrasDiurnaMin = 0;
                }
                horasFaltasMin = 0;
                //Contabiliza Jornada Trabalhada se estiver flegado.                
                horasExtrasDiurnaMin = contabilizarjornada == 1 ? horasExtrasDiurnaMin = horasTrabalhadasMin + horasExtrasDiurnaMin : horasExtrasDiurnaMin;
                horasTrabalhadasMin = horarioD;
            }
            else if (possuiAbono && abono == 1)
            {
                int cargaProposta = horarioD;

                int abonoDMin = Modelo.cwkFuncoes.ConvertHorasMinuto(abonoD);
                // Calcula apenas se for parcial e tiver valor para o diurno
                if (abonoDMin > 0)
                {
                    int faltaAbonada = horasFaltasMin - abonoDMin;
                    if (faltaAbonada >= 0) // Se abonando ainda ficou com falta, apenas remove o abono da falta e soma o abono nas horas trabalhadas
                    {
                        horasTrabalhadasMin += abonoDMin;  // Adiciona o abono nas horas trabalhadas
                        horasFaltasMin = faltaAbonada; // Mantem o restante da falta
                    }
                    else // abono é maior que a falta, deve gerar extra
                    {
                        // Verifica se o abono excedente vai influenciar nas horas extras Diurnas ou Noturnas, pois quando funcionário trabalhou um pouco em cada período, e ele se no dia compensou as horas, ou seja, mesmo faltando um pedaço trabalhou a mais, as horas extras geradas pelo abono devem ser verificadas se devem ficar nas diurnas ou nas noturnas.
                        int diffRealizadaXProtosta = horarioD - (horasTrabalhadasMin); //"Falta" entre total proposto e realizado
                        int abonoParaVirarExtra = Math.Abs(faltaAbonada);
                        if (diffRealizadaXProtosta >= 0 && diffRealizadaXProtosta <= abonoParaVirarExtra)
                        {
                            horasTrabalhadasMin += Math.Max(0, diffRealizadaXProtosta);
                        }
                        else
                        {
                            horasTrabalhadasMin += (Math.Max(0, abonoParaVirarExtra) + horasFaltasMin);
                            if (horasTrabalhadasMin > horarioD && (horasTrabalhadasMin + horasTrabalhadasNoturnasMin) > (horarioD + horarioN))
                            {
                                horasTrabalhadasMin = horarioD;
                            }
                        }
                        diffRealizadaXProtosta -= horasFaltasMin;
                        horasFaltasMin = 0; // Zera a falta

                        int diff = abonoParaVirarExtra - Math.Max(0, diffRealizadaXProtosta);
                        horasExtrasDiurnaMin += Math.Max(0, diff);
                        int horasExcedentesParaNoturna = abonoParaVirarExtra - Math.Max(0, diff);
                        horasExtraNoturnaMin += horasExcedentesParaNoturna;
                        horasTrabalhadasNoturnasMin -= horasExcedentesParaNoturna;
                    }
                }
                abonoDMin = 0; abonoD = "--:--";// zera o abono, já foi tratado acima (distribuido)
            }
            #endregion

            if (legenda != "" && horasTrabalhadasMin != 0)
            {
                if (!((legenda == "F") && bConsiderarHEFeriadoPHoraNoturna))
                {
                    if (possuiAbono && abono == 0 && abonoD == "--:--" && semcalc == false && semAbono == true)
                    {
                        if (marcaCargaHorariaMistaHorario == 1 && separaExtraFalta == 0)
                        {
                            int resultado = (horasFaltasMin - horasExtrasDiurnaMin);
                            if (resultado > 0)
                            {
                                horasFaltasMin = 0;
                                horasExtrasDiurnaMin = 0;
                            }
                            else
                            {
                                horasFaltasMin = 0;
                                horasExtrasDiurnaMin = Math.Abs(resultado);
                            }
                        }
                        else
                        {
                            horasFaltasMin = 0;
                            horasExtrasDiurnaMin = 0;
                        }
                    }
                    else if (possuiAbono && abono == 0 && abonoD == "--:--" && semcalc == false && semAbono == false)
                    {
                        horasExtrasDiurnaMin = BLL.CalculoHoras.OperacaoHoras('+', horasExtrasDiurnaMin, horasTrabalhadasMin);
                        horasFaltasMin = 0;
                        horasTrabalhadasMin = 0;
                        AdicionalNoturno = 0;
                    }
                    else if (legenda == "F" && feriadoParcial == false && marcaCargaHorariaMistaHorario == 1)
                    {
                        horasExtrasDiurnaMin = CalculoHoras.OperacaoHoras('+', horasExtrasDiurnaMin, horasTrabalhadasMin); //CRNC
                        horasFaltasMin = 0;
                        horasTrabalhadasMin = 0;
                        AdicionalNoturno = 0;
                    }
                }
            }
            else if (possuiAbono && horasTrabalhadasMin == 0)
            {
                if (abono == 0 && semcalc == true)
                {
                    if (marcaCargaHorariaMistaHorario == 0 || separaExtraFalta == 1)
                    {
                        horasFaltasMin = horarioD;
                    }
                    else
                    {
                        horasFaltasMin = horarioM;
                    }
                    horasExtrasDiurnaMin = 0;
                }
                else if (abono == 0 && abonoD == "--:--" && semcalc == false && semAbono == true)
                {
                    horasFaltasMin = 0;
                }
                else if (abono == 0 && horasExtrasDiurnaMin == 0 && semcalc == false)
                {
                    horasFaltasMin = 0;
                    horasExtrasDiurnaMin = 0;
                }
            }
            else if ((legenda == "F" && !feriadoParcial) && horasTrabalhadasMin == 0 && horasExtrasDiurnaMin == 0)
            {
                horasFaltasMin = 0;
                horasExtrasDiurnaMin = 0;
                ocorrencia = "";
            }
            else if (neutroMarcacao && (legenda == "F" || string.IsNullOrEmpty(legenda)) && horasFaltasMin != 0)
            {
                horasFaltasMin = 0;
                ocorrencia = "";
            }
            else if (string.IsNullOrEmpty(legenda) && ((marcaCargaHorariaMistaHorario != 1 && horasFaltasMin != 0 && horasFaltasMin == horarioD) || (marcaCargaHorariaMistaHorario == 1 && (horarioM > 0 && (horasFaltasMin + horasFaltaNoturnaMin) == horarioM))))
            {
                ocorrencia = "Falta";
            }
        }

        private void RegraParaExtrasFeriado()
        {
            if (((legenda == "F") && bConsiderarHEFeriadoPHoraNoturna))
            {
                int horaTrabalhadaDiurna, horaExtraDiurna, horaTrabalhadaNoturna, horaExtraNoturna;
                HorasTrabalhadasFeriado(out horaTrabalhadaDiurna, out horaExtraDiurna, out horaTrabalhadaNoturna, out horaExtraNoturna);
                horasExtrasDiurnaMin = horaExtraDiurna;
                horasTrabalhadasMin = horaTrabalhadaDiurna;

                if (conversaoHoranoturna == 1)
                {
                    horasExtraNoturnaMin = BLL.CalculoHoras.HoraNoturna(horaExtraNoturna, reducaohoranoturna);
                    horasTrabalhadasNoturnasMin = BLL.CalculoHoras.HoraNoturna(horaTrabalhadaNoturna, reducaohoranoturna);
                    horasFaltasMin = 0;
                }
                else
                {
                    horasExtraNoturnaMin = horaExtraNoturna;
                    horasTrabalhadasNoturnasMin = horaTrabalhadaNoturna;
                    horasFaltasMin = 0;
                }
                horasExtrasDiurnaMin += HoraExtraDiurnaDiaAnterior;
                horasExtraNoturnaMin += HoraExtraNoturnaDiaAnterior;

            }

            if (legenda == "F" && feriadoParcial)
            {
                horasExtrasDiurnaMin += horasTrabalhadasDentroFeriadoParcialDiurna;
                horasExtraNoturnaMin += horasTrabalhadasDentroFeriadoParcialNoturna;
                //if (horasExtrasDiurnaMin < horasTrabalhadasDentroFeriadoParcialDiurna)
                //{
                //    int diff = horasTrabalhadasDentroFeriadoParcialDiurna - horasExtrasDiurnaMin;
                //    if (diff > 0 && horasTrabalhadasMin > 0)
                //    {
                //        if (horasTrabalhadasMin > diff)
                //        {
                //            horasTrabalhadasMin -= diff;
                //            horasExtrasDiurnaMin += diff;
                //        }
                //        else
                //        {
                //            horasExtrasDiurnaMin += horasTrabalhadasMin;
                //            horasTrabalhadasMin = 0;
                //        }
                //    }
                //}


                //if (horasExtraNoturnaMin < horasTrabalhadasDentroFeriadoParcialNoturna)
                //{
                //    int diff = horasTrabalhadasDentroFeriadoParcialNoturna - horasExtraNoturnaMin;
                //    if (diff > 0 && horasTrabalhadasNoturnasMin > 0)
                //    {
                //        if (horasTrabalhadasNoturnasMin > diff)
                //        {
                //            horasTrabalhadasNoturnasMin -= diff;
                //            horasExtraNoturnaMin += diff;
                //        }
                //        else
                //        {
                //            horasExtraNoturnaMin += horasTrabalhadasNoturnasMin;
                //            horasTrabalhadasNoturnasMin = 0;
                //        }
                //    }
                //}
            }

            if (legenda == "F" && feriadoParcial)
            {
                if (horasExtrasDiurnaMin < horasTrabalhadasDentroFeriadoParcialDiurna)
                {
                    int diff = horasTrabalhadasDentroFeriadoParcialDiurna - horasExtrasDiurnaMin;
                    if (diff > 0 && horasTrabalhadasMin > 0)
                    {
                        if (horasTrabalhadasMin > diff)
                        {
                            horasTrabalhadasMin -= diff;
                            horasExtrasDiurnaMin += diff;
                        }
                        else
                        {
                            horasExtrasDiurnaMin += horasTrabalhadasMin;
                            horasTrabalhadasMin = 0;
                        }
                    }
                }


                if (horasExtraNoturnaMin < horasTrabalhadasDentroFeriadoParcialNoturna)
                {
                    int diff = horasTrabalhadasDentroFeriadoParcialNoturna - horasExtraNoturnaMin;
                    if (diff > 0 && horasTrabalhadasNoturnasMin > 0)
                    {
                        if (horasTrabalhadasNoturnasMin > diff)
                        {
                            horasTrabalhadasNoturnasMin -= diff;
                            horasExtraNoturnaMin += diff;
                        }
                        else
                        {
                            horasExtraNoturnaMin += horasTrabalhadasNoturnasMin;
                            horasTrabalhadasNoturnasMin = 0;
                        }
                    }
                }
            }
        }

        private void HorasTrabalhadasFeriado(out int horaTrabalhadaDiurna, out int horaExtraDiurna, out int horaTrabalhadaNoturna, out int horaExtraNoturna)
        {
            horaTrabalhadaDiurna = 0;
            horaExtraDiurna = 0;
            horaTrabalhadaNoturna = 0;
            horaExtraNoturna = 0;

            IList<int> MarEntradaHoraExtraDiurna;
            IList<int> MarEntradaHoraTrabalhadaDiurna;
            IList<int> MarSaidaHoraExtraDiurna;
            IList<int> MarSaidaHoraTrabalhadaDiurna;
            IList<int> MarEntradaHoraExtraNoturna;
            IList<int> MarEntradaHoraTrabalhadaNoturna;
            IList<int> MarSaidaHoraExtraNoturna;
            IList<int> MarSaidaHoraTrabalhadaNoturna;
            bool bSaidaProximoDia;

            CriarListasRegraExtraNoturno(out MarEntradaHoraExtraDiurna, out MarEntradaHoraTrabalhadaDiurna, out MarSaidaHoraExtraDiurna, out MarSaidaHoraTrabalhadaDiurna,
                                        out MarEntradaHoraExtraNoturna, out MarEntradaHoraTrabalhadaNoturna, out MarSaidaHoraExtraNoturna, out MarSaidaHoraTrabalhadaNoturna,
                                        out bSaidaProximoDia);
            AcertarListasRegraExtraNoturno(MarEntradaHoraExtraDiurna, MarEntradaHoraTrabalhadaDiurna, MarSaidaHoraExtraDiurna, MarSaidaHoraTrabalhadaDiurna,
                                            MarEntradaHoraExtraNoturna, MarEntradaHoraTrabalhadaNoturna, MarSaidaHoraExtraNoturna, MarSaidaHoraTrabalhadaNoturna,
                                            bSaidaProximoDia);
            TotalizarHorariosRegraExtraNoturno(ref horaTrabalhadaDiurna, ref horaExtraDiurna, ref horaTrabalhadaNoturna, ref horaExtraNoturna, MarEntradaHoraExtraDiurna,
                                                MarEntradaHoraTrabalhadaDiurna, MarSaidaHoraExtraDiurna, MarSaidaHoraTrabalhadaDiurna, MarEntradaHoraExtraNoturna,
                                                MarEntradaHoraTrabalhadaNoturna, MarSaidaHoraExtraNoturna, MarSaidaHoraTrabalhadaNoturna);
        }

        private void AcertarListasRegraExtraNoturno(IList<int> MarEntradaHoraExtraDiurna, IList<int> MarEntradaHoraTrabalhadaDiurna, IList<int> MarSaidaHoraExtraDiurna, IList<int> MarSaidaHoraTrabalhadaDiurna, IList<int> MarEntradaHoraExtraNoturna, IList<int> MarEntradaHoraTrabalhadaNoturna, IList<int> MarSaidaHoraExtraNoturna, IList<int> MarSaidaHoraTrabalhadaNoturna, bool bSaidaProximoDia)
        {
            if (bSaidaProximoDia && MarSaidaHoraExtraNoturna.Count() == 0)
            {
                MarSaidaHoraExtraNoturna.Add(1440);
            }

            if (MarEntradaHoraExtraDiurna.Count() != MarSaidaHoraExtraDiurna.Count())
            {
                if (MarEntradaHoraExtraDiurna.Count() > MarSaidaHoraExtraDiurna.Count())
                {
                    MarSaidaHoraExtraDiurna.Add(inicioAdNoturno); // Inclui 10 horas para a saida limite para extras diurnos                    
                }
                else
                {
                    MarEntradaHoraExtraDiurna.Add(fimAdNoturno);
                }
            }
            if (MarEntradaHoraTrabalhadaDiurna.Count() != MarSaidaHoraTrabalhadaDiurna.Count())
            {
                if (MarEntradaHoraTrabalhadaDiurna.Count() > MarSaidaHoraTrabalhadaDiurna.Count())
                {
                    MarSaidaHoraTrabalhadaDiurna.Add(inicioAdNoturno);
                }
                else
                {
                    MarEntradaHoraTrabalhadaDiurna.Add(fimAdNoturno);
                }
            }
            if (MarEntradaHoraExtraNoturna.Count() != MarSaidaHoraExtraNoturna.Count())
            {
                if (MarEntradaHoraExtraNoturna.Count() > MarSaidaHoraExtraNoturna.Count())
                {
                    MarSaidaHoraExtraNoturna.Add(1440);
                }
                else
                {
                    MarEntradaHoraExtraNoturna.Add(inicioAdNoturno);
                }
            }
            if (MarEntradaHoraTrabalhadaNoturna.Count() != MarSaidaHoraTrabalhadaNoturna.Count())
            {
                if (MarEntradaHoraTrabalhadaNoturna.Count() > MarSaidaHoraTrabalhadaNoturna.Count())
                {
                    MarSaidaHoraTrabalhadaNoturna.Add(fimAdNoturno);
                }
                else
                {
                    MarEntradaHoraTrabalhadaNoturna.Add(0);
                }
            }

        }

        private void CriarListasRegraExtraNoturno(out IList<int> MarEntradaHoraExtraDiurna, out IList<int> MarEntradaHoraTrabalhadaDiurna, out IList<int> MarSaidaHoraExtraDiurna, out IList<int> MarSaidaHoraTrabalhadaDiurna, out IList<int> MarEntradaHoraExtraNoturna, out IList<int> MarEntradaHoraTrabalhadaNoturna, out IList<int> MarSaidaHoraExtraNoturna, out IList<int> MarSaidaHoraTrabalhadaNoturna, out bool bSaidaProximoDia)
        {
            int[] MarSaida = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };
            int[] MarEntrada = new int[] { -1, -1, -1, -1, -1, -1, -1, -1 };

            MarEntradaHoraExtraDiurna = new List<int>();
            MarEntradaHoraTrabalhadaDiurna = new List<int>();
            MarSaidaHoraExtraDiurna = new List<int>();
            MarSaidaHoraTrabalhadaDiurna = new List<int>();
            MarEntradaHoraExtraNoturna = new List<int>();
            MarEntradaHoraTrabalhadaNoturna = new List<int>();
            MarSaidaHoraExtraNoturna = new List<int>();
            MarSaidaHoraTrabalhadaNoturna = new List<int>();

            bSaidaProximoDia = false;
            this.GetEntradasSaidasValidas(ref MarEntrada, ref MarSaida);

            for (int i = 0; i < MarEntrada.Length; i++)
            {
                if (MarEntrada[i] != -1)
                {
                    if ((MarEntrada[i] <= inicioAdNoturno) &&
                        (MarEntrada[i] >= MarEntrada[0]))
                    {
                        MarEntradaHoraExtraDiurna.Add(MarEntrada[i]);
                    }
                    else if ((MarEntrada[i] <= 1440) &&
                        (MarEntrada[i] > inicioAdNoturno))
                    {
                        MarEntradaHoraExtraNoturna.Add(MarEntrada[i]);
                    }
                    else if (MarEntrada[i] <= fimAdNoturno)
                    {
                        MarEntradaHoraTrabalhadaNoturna.Add(MarEntrada[i]);
                    }
                    else
                    {
                        MarEntradaHoraTrabalhadaDiurna.Add(MarEntrada[i]);
                    }
                }
            }

            for (int i = 0; i < MarSaida.Length; i++)
            {
                if (MarSaida[i] != -1)
                {
                    if (MarEntrada[i] > MarSaida[i])// saida proximo dia
                    {
                        MarEntradaHoraTrabalhadaNoturna.Insert(0, 0);
                        bSaidaProximoDia = true;
                    }
                    if (bSaidaProximoDia)
                    {
                        bSaidaProximoDia = true;
                        if (MarSaida[i] <= fimAdNoturno)
                        {
                            MarSaidaHoraTrabalhadaNoturna.Add(MarSaida[i]);
                        }
                        else
                        {
                            MarSaidaHoraTrabalhadaDiurna.Add(MarSaida[i]);
                        }
                    }
                    else
                    {
                        if ((i > 0) &&
                            (MarSaida[i] < MarSaida[i - 1]))
                        {
                            if (MarSaida[i] <= fimAdNoturno)
                            {
                                MarSaidaHoraTrabalhadaNoturna.Add(MarSaida[i]);
                            }
                            else
                            {
                                MarSaidaHoraTrabalhadaDiurna.Add(MarSaida[i]);
                            }
                        }
                        else
                        {
                            if ((MarSaida[i] <= inicioAdNoturno) &&
                                (MarSaida[i] >= MarSaida[0]))
                            {
                                MarSaidaHoraExtraDiurna.Add(MarSaida[i]);
                            }
                            else
                            {
                                MarSaidaHoraExtraNoturna.Add(MarSaida[i]);
                            }
                        }
                    }

                }
            }
        }

        private void TotalizarHorariosRegraExtraNoturno(ref int horaTrabalhadaDiurna, ref int horaExtraDiurna, ref int horaTrabalhadaNoturna, ref int horaExtraNoturna, IList<int> MarEntradaHoraExtraDiurna, IList<int> MarEntradaHoraTrabalhadaDiurna, IList<int> MarSaidaHoraExtraDiurna, IList<int> MarSaidaHoraTrabalhadaDiurna, IList<int> MarEntradaHoraExtraNoturna, IList<int> MarEntradaHoraTrabalhadaNoturna, IList<int> MarSaidaHoraExtraNoturna, IList<int> MarSaidaHoraTrabalhadaNoturna)
        {
            for (int i = 0; i < MarEntradaHoraExtraDiurna.Count; i++)
            {
                if ((MarSaidaHoraExtraDiurna[i] - MarEntradaHoraExtraDiurna[i]) >= 0)
                {
                    horaExtraDiurna += MarSaidaHoraExtraDiurna[i] - MarEntradaHoraExtraDiurna[i];
                }
            }

            for (int i = 0; i < MarEntradaHoraExtraNoturna.Count; i++)
            {
                if ((MarSaidaHoraExtraNoturna[i] - MarEntradaHoraExtraNoturna[i]) < 0)
                {
                    horaExtraNoturna += MarSaidaHoraExtraNoturna[i] - 1320;
                    horaExtraNoturna += 1440 - MarEntradaHoraExtraNoturna[i];
                }
                else
                {
                    horaExtraNoturna += MarSaidaHoraExtraNoturna[i] - MarEntradaHoraExtraNoturna[i];
                }
            }

            for (int i = 0; i < MarEntradaHoraTrabalhadaDiurna.Count; i++)
            {
                if ((MarSaidaHoraTrabalhadaDiurna[i] - MarEntradaHoraTrabalhadaDiurna[i]) >= 0)
                {
                    horaTrabalhadaDiurna += MarSaidaHoraTrabalhadaDiurna[i] - MarEntradaHoraTrabalhadaDiurna[i];
                }
            }

            for (int i = 0; i < MarEntradaHoraTrabalhadaNoturna.Count; i++)
            {
                if ((MarSaidaHoraTrabalhadaNoturna[i] - MarEntradaHoraTrabalhadaNoturna[i]) < 0)
                {
                    horaTrabalhadaNoturna += MarSaidaHoraTrabalhadaNoturna[i] - 0;
                    horaTrabalhadaNoturna += fimAdNoturno - MarEntradaHoraTrabalhadaNoturna[i];
                }
                else
                {
                    horaTrabalhadaNoturna += MarSaidaHoraTrabalhadaNoturna[i] - MarEntradaHoraTrabalhadaNoturna[i];
                }
            }
        }

        private void RegrasParaNoturno(int abono, bool semcalc, string abonoN, int horarioN, int horarioD, bool SemAbono)
        {
            bool possuiAbono = LegendasConcatenadas.Split(',').Where(s => s == "A").Count() > 0;
            #region Novo calculo abono total e parcial para todos os tipos de horário (Misto, normal e separa e falta)
            if (legenda == "F" && possuiAbono && abono != 0)
            {
                abono = 0;
            }
            if (possuiAbono && abono == 2)//Abono total
            {
                if (separaExtraFalta != 1 && contabilizarjornada == 0) // Apenas retira as horas extras caso não seja para separar extra e falta
                {
                    horasExtraNoturnaMin = 0;
                }
                horasFaltaNoturnaMin = 0;
                //Contabiliza Jornada Trabalhada se estiver flegado.                
                horasExtraNoturnaMin = contabilizarjornada == 1 ? horasExtraNoturnaMin = horasTrabalhadasNoturnasMin + horasExtraNoturnaMin : horasExtraNoturnaMin;
                horasTrabalhadasNoturnasMin = horarioN;
            }
            else if (possuiAbono && abono == 1) // Abono parcial
            {
                int abonoNMin = Modelo.cwkFuncoes.ConvertHorasMinuto(abonoN);
                // Calculo apenas se for abono Parcial e tiver valor para noturno.
                if (abonoNMin > 0)
                {
                    int faltaAbonada = horasFaltaNoturnaMin - abonoNMin;
                    if (faltaAbonada >= 0) // Se abonando ainda ficou com falta, apenas remove o abono da falta e soma o abono nas horas trabalhadas
                    {
                        horasTrabalhadasNoturnasMin += abonoNMin;  // Adiciona o abono nas horas trabalhadas
                        horasFaltaNoturnaMin = faltaAbonada; // Mantem o restante da falta
                    }
                    else // abono é maior que a falta, deve gerar extra
                    {
                        int diffRealizadaXProtosta = horarioN - (horasTrabalhadasNoturnasMin); //"Falta" entre total proposto e realizado

                        int abonoParaVirarExtra = Math.Abs(faltaAbonada);
                        if (diffRealizadaXProtosta >= 0 && diffRealizadaXProtosta <= abonoParaVirarExtra)
                        {
                            horasTrabalhadasNoturnasMin += Math.Max(0, diffRealizadaXProtosta);
                        }
                        else
                        {
                            horasTrabalhadasNoturnasMin += (Math.Max(0, abonoParaVirarExtra) + horasFaltaNoturnaMin);
                            if (horasTrabalhadasNoturnasMin > horarioN && (horasTrabalhadasMin + horasTrabalhadasNoturnasMin) > (horarioD + horarioN))
                            {
                                horasTrabalhadasNoturnasMin = horarioN;
                            }
                        }
                        diffRealizadaXProtosta -= horasFaltaNoturnaMin;
                        horasFaltaNoturnaMin = 0; // Zera a falta

                        int diff = abonoParaVirarExtra - diffRealizadaXProtosta;
                        horasExtraNoturnaMin = Math.Max(0, diff);
                        int horasExcedentesParaDiurna = abonoParaVirarExtra - Math.Max(0, diff);
                        horasExtrasDiurnaMin += horasExcedentesParaDiurna;
                        horasTrabalhadasMin -= horasExcedentesParaDiurna;
                    }
                }

                abonoNMin = 0; abonoN = "--:--";// zera o abono, já foi tratado acima (distribuido)
            }
            #endregion
            if (legenda != "" && horasTrabalhadasNoturnasMin != 0)
            {
                if (!((legenda == "F") && bConsiderarHEFeriadoPHoraNoturna))
                {
                    if (possuiAbono && abono == 0 && abonoN == "--:--" && semcalc == false && SemAbono == true)
                    {
                        if (marcaCargaHorariaMistaHorario == 1 && separaExtraFalta == 0)
                        {
                            int resultado = (horasFaltaNoturnaMin - horasExtraNoturnaMin);
                            if (resultado > 0)
                            {
                                horasFaltaNoturnaMin = 0;
                                horasExtraNoturnaMin = 0;
                            }
                            else
                            {
                                horasFaltaNoturnaMin = 0;
                                horasExtraNoturnaMin = Math.Abs(resultado);
                            }
                        }
                        else
                        {
                            horasFaltaNoturnaMin = 0;
                            horasExtraNoturnaMin = 0;
                        }

                    }
                    else if (possuiAbono && abono == 0 && abonoN == "--:--" && semcalc == false && SemAbono == false)
                    {
                        horasExtraNoturnaMin = BLL.CalculoHoras.OperacaoHoras('+', horasExtraNoturnaMin, horasTrabalhadasNoturnasMin);
                        horasFaltaNoturnaMin = 0;
                        horasTrabalhadasNoturnasMin = 0;
                    }
                    else if (legenda == "F" && feriadoParcial == false && marcaCargaHorariaMistaHorario == 1)
                    {
                        horasExtraNoturnaMin = CalculoHoras.OperacaoHoras('+', horasExtraNoturnaMin, horasTrabalhadasNoturnasMin);//CRNC
                        horasFaltaNoturnaMin = 0;
                        horasTrabalhadasNoturnasMin = 0;
                    }
                }
            }
            else if (possuiAbono && horasTrabalhadasNoturnasMin == 0)
            {
                if (abono == 0 && semcalc == true)
                {
                    horasFaltaNoturnaMin = horarioN;
                }
                else if (abono == 0 && horasExtrasDiurnaMin == 0 && semcalc == false)
                {
                    horasFaltaNoturnaMin = 0;
                    horasExtraNoturnaMin = 0;
                }
            }
            else if ((legenda == "F" && !feriadoParcial) && horasTrabalhadasNoturnasMin == 0 && horasExtraNoturnaMin == 0)
            {
                horasFaltaNoturnaMin = 0;
                horasExtraNoturnaMin = 0;
            }
            else if (neutroMarcacao && (legenda == "F" || legenda == "") && horasFaltaNoturnaMin != 0)
            {
                horasFaltaNoturnaMin = 0;
                ocorrencia = "";
            }
            //else if (legenda == "" && horasFaltaNoturnaMin != 0 && horasFaltaNoturnaMin == horarioN)
            //{
            //    ocorrencia = "Falta";
            //}
        }
        #endregion

        #endregion

        #region Afastamento

        public void LocalizaOcorrencia(ref string pOcorrencia, ref int pAbono, ref bool pSemCalc, ref string pAbonoD, ref string pAbonoN, ref bool psemAbono)
        {
            pAbono = 0;
            pSemCalc = false;
            pAbonoD = "--:--";
            pAbonoN = "--:--";
            psemAbono = false;

            //Verifica se possui registro por Funcionario
            if (idAfastamentoFunc != null)
            {
                abonado = abonadoFunc.Value;
                semCalculoAfastamento = SemCalculoFunc.Value;
                semAbonoAfastamento = SemAbonoFunc.Value;
                horai = horaiFunc;
                horaf = horafFunc;
                idOcorrencia = idOcorrenciaFunc.Value;
                contabilizarjornada = contabilizarjornadaFunc.Value;
                AuxLocalizaOcorrencia(ref pOcorrencia, ref pAbono, ref pSemCalc, ref pAbonoD, ref pAbonoN, ref psemAbono);
                return;
            }

            //Verifica se possui registro por Departamento
            if (idAfastamentoDep != null)
            {
                abonado = abonadoDep.Value;
                semCalculoAfastamento = SemCalculoDep.Value;
                semAbonoAfastamento = SemAbonoDep.Value;
                horai = horaiDep;
                horaf = horafDep;
                idOcorrencia = idOcorrenciaDep.Value;
                contabilizarjornada = contabilizarjornadaDep.Value;
                AuxLocalizaOcorrencia(ref pOcorrencia, ref pAbono, ref pSemCalc, ref pAbonoD, ref pAbonoN, ref psemAbono);
                return;
            }

            //Verifica se possui registro por Empresa
            if (idAfastamentoEmp != null)
            {
                abonado = abonadoEmp.Value;
                semCalculoAfastamento = SemCalculoEmp.Value;
                semAbonoAfastamento = SemAbonoEmp.Value;
                horai = horaiEmp;
                horaf = horafEmp;
                idOcorrencia = idOcorrenciaEmp.Value;
                contabilizarjornada = contabilizarjornadaEmp.Value;
                AuxLocalizaOcorrencia(ref pOcorrencia, ref pAbono, ref pSemCalc, ref pAbonoD, ref pAbonoN, ref psemAbono);
            }

            //Verifica se possui registro por Contrato
            if (idAfastamentoCont != null)
            {
                abonado = abonadoCont.Value;
                semCalculoAfastamento = SemCalculoCont.Value;
                semAbonoAfastamento = SemAbonoCont.Value;
                horai = horaiCont;
                horaf = horafCont;
                idOcorrencia = idOcorrenciaCont.Value;
                contabilizarjornada = contabilizarjornadaCont.Value;
                AuxLocalizaOcorrencia(ref pOcorrencia, ref pAbono, ref pSemCalc, ref pAbonoD, ref pAbonoN, ref psemAbono);
            }
        }

        private void AuxLocalizaOcorrencia(ref string pOcorrencia, ref int pAbono, ref bool pSemCalc, ref string pAbonoD, ref string pAbonoN, ref bool pSemAbono)
        {
            pOcorrencia = (string)ocorrenciaList[idOcorrencia];

            if (horai == "--:--" && horaf == "--:--" && abonado == 1)
            {
                pAbono = 2;
            }
            else
            {
                pAbono = abonado;
            }

            pSemCalc = Convert.ToBoolean(semCalculoAfastamento);
            pSemAbono = Convert.ToBoolean(semAbonoAfastamento);

            if (pSemCalc)
            {
                pAbono = 0;
                pAbonoD = "--:--";
                pAbonoN = "--:--";
            }
            else if (pSemAbono)
            {
                pAbono = 0;
                pAbonoD = "--:--";
                pAbonoN = "--:--";
            }
            else
            {
                pAbonoD = horai;
                pAbonoN = horaf;
            }
        }

        #endregion

        #region Intervalo Automático

        /// <summary>
        /// Método que gera os intervalos automáticos
        /// </summary>
        /// <param name="objMarcacao">Marcação para gerar os intervalos</param>
        /// <param name="objHorario">Horário da marcação</param>
        /// WNO - 15/01/2010
        private bool IntervaloAutomatico(DataRow pMarcacao)
        {
            bool possuiJornadaAlternativa = objJornadaAlternativa != null;
            bool possuia = false;
            #region Verifica se existe intervalo automático para aquela data

            if (Convert.ToBoolean(folgaMarcacao) || tratamentosMarcacao.Where(x => x.Relogio == "PA" && x.Ocorrencia == 'D').Count() > 0)
            {
                return false;
            }
            if (marcaCargaHorariaMistaHD == null)
            {
                if (!possuiJornadaAlternativa)
                {
                    return false;
                }
            }

            if (possuiJornadaAlternativa)
            {
                intervaloAutomatico = objJornadaAlternativa.Intervaloautomatico;
                Preassinaladas1 = objJornadaAlternativa.Preassinaladas1;
                Preassinaladas2 = objJornadaAlternativa.Preassinaladas2;
                Preassinaladas3 = objJornadaAlternativa.Preassinaladas3;
            }
            List<BilhetesImpProxyIA> resultado = new List<BilhetesImpProxyIA>();

            if (tratamentosMarcacao.Where(x => x.Relogio == "PA").Count() > 0)
            {
                possuia = true;
                tratamentosMarcacao.Where(x => x.Relogio == "PA").ToList().ForEach(s => { s.Acao = Modelo.Acao.Excluir; s.Ocorrencia = 'D'; });
                BilhetesImp.AjustarPosicaoBilhetes(tratamentosMarcacao);
                foreach (Modelo.BilhetesImp bil in tratamentosMarcacao.Where(w => w.Acao != Modelo.Acao.Excluir))
                {
                    BilhetesImpProxyIA bip = new BilhetesImpProxyIA();
                    bip.Bilhete = bil;
                    resultado.Add(bip);
                }
            }

            #endregion
            int[] entradasHorario = null, saidasHorario = null;
            if (possuiJornadaAlternativa)
            {
                entradasHorario = new int[4];
                entradasHorario[0] = objJornadaAlternativa.EntradaMin_1;
                entradasHorario[1] = objJornadaAlternativa.EntradaMin_2;
                entradasHorario[2] = objJornadaAlternativa.EntradaMin_3;
                entradasHorario[3] = objJornadaAlternativa.EntradaMin_4;

                saidasHorario = new int[4];
                saidasHorario[0] = objJornadaAlternativa.SaidaMin_1;
                saidasHorario[1] = objJornadaAlternativa.SaidaMin_2;
                saidasHorario[2] = objJornadaAlternativa.SaidaMin_3;
                saidasHorario[3] = objJornadaAlternativa.SaidaMin_4;
            }
            else
            {
                entradasHorario = this.GetEntradasHorarioDetalheIA();
                saidasHorario = this.GetSaidasHorarioDetalheIA();
            }

            GeradorIntervaloAutomatico gerador = new GeradorIntervaloAutomatico(dscodigo, data, entradasHorario, saidasHorario, tratamentosMarcacao, momentoPreAssinalado);
            gerador.SetIntervalos(Preassinaladas1 == 1, Preassinaladas2 == 1, Preassinaladas3 == 1);
            gerador.SetCalcularInItinere(calcularInItinere, HabilitaInItinere.GetValueOrDefault());
            resultado = gerador.Gerar();
            if (resultado.Count == 0)
            {
                foreach (var bil in tratamentosMarcacao.Where(w => (w.Acao == Modelo.Acao.Excluir && w.Relogio == "PA") || (w.Acao == Modelo.Acao.Alterar || w.Acao == Modelo.Acao.Incluir)))
                {
                    BilhetesImpProxyIA bip = new BilhetesImpProxyIA();
                    bip.Bilhete = bil;
                    bip.HoraEmMinutos = Modelo.cwkFuncoes.ConvertHorasMinuto(bil.Hora);
                    resultado.Add(bip);
                }
            }

            if (resultado.Count > 0)
            {
                Type tipoClasse = this.GetType();
                for (int i = 1; i <= 8; i++)
                {
                    var bilEnt = resultado.Where(b => b.Bilhete.Posicao == i && b.Bilhete.Ent_sai == "E" && b.Bilhete.Acao != Modelo.Acao.Excluir).FirstOrDefault();
                    var bilSai = resultado.Where(b => b.Bilhete.Posicao == i && b.Bilhete.Ent_sai == "S" && b.Bilhete.Acao != Modelo.Acao.Excluir).FirstOrDefault();

                    if (bilEnt != null)
                    {
                        pMarcacao["entrada_" + i] = bilEnt.Bilhete.Hora;
                        pMarcacao["ent_num_relogio_" + i] = bilEnt.Bilhete.Relogio;
                        pMarcacao["entrada_" + i + "min"] = bilEnt.HoraEmMinutos;
                        tipoClasse.GetField("entrada_" + i + "Min", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this, bilEnt.HoraEmMinutos);
                    }
                    else
                    {
                        pMarcacao["entrada_" + i] = "--:--";
                        pMarcacao["ent_num_relogio_" + i] = "";
                        pMarcacao["entrada_" + i + "min"] = 0;
                        tipoClasse.GetField("entrada_" + i + "Min", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this, 0);
                    }

                    if (bilSai != null)
                    {
                        pMarcacao["saida_" + i] = bilSai.Bilhete.Hora;
                        pMarcacao["sai_num_relogio_" + i] = bilSai.Bilhete.Relogio;
                        pMarcacao["saida_" + i + "min"] = bilSai.HoraEmMinutos;
                        tipoClasse.GetField("saida_" + i + "Min", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this, bilSai.HoraEmMinutos);
                    }
                    else
                    {
                        pMarcacao["saida_" + i] = "--:--";
                        pMarcacao["sai_num_relogio_" + i] = "";
                        pMarcacao["saida_" + i + "min"] = 0;
                        tipoClasse.GetField("saida_" + i + "Min", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(this, 0);
                    }
                }
                return true;
            }
            return false;
        }

        private int[] GetEntradasHorarioDetalheIA()
        {
            var entradas = new int[4];
            int i = 0;
            entradas[i++] = entrada_1MinHD;
            entradas[i++] = entrada_2MinHD;
            entradas[i++] = entrada_3MinHD;
            entradas[i] = entrada_4MinHD;
            return entradas;
        }

        private int[] GetSaidasHorarioDetalheIA()
        {
            var saidas = new int[4];
            int i = 0;
            saidas[i++] = saida_1MinHD;
            saidas[i++] = saida_2MinHD;
            saidas[i++] = saida_3MinHD;
            saidas[i] = saida_4MinHD;
            return saidas;
        }

        private int ValidaDataBilhete(string horabilhete)
        {
            int entrada, saida, limiteMin, limiteMax, horabilheteMin;
            bool possuihorario = true;

            horabilheteMin = Modelo.cwkFuncoes.ConvertHorasMinuto(horabilhete);

            //Verifica se tem jornada alternativa
            if (objJornadaAlternativa != null)
            {
                entrada = objJornadaAlternativa.EntradaMin_1;
                limiteMin = Modelo.cwkFuncoes.ConvertHorasMinuto(objJornadaAlternativa.LimiteMin);
                limiteMax = Modelo.cwkFuncoes.ConvertHorasMinuto(objJornadaAlternativa.LimiteMax);

                if (objJornadaAlternativa.SaidaMin_4 >= 0)
                    saida = objJornadaAlternativa.SaidaMin_4;
                else if (objJornadaAlternativa.SaidaMin_3 >= 0)
                    saida = objJornadaAlternativa.SaidaMin_3;
                else if (objJornadaAlternativa.SaidaMin_2 >= 0)
                    saida = objJornadaAlternativa.SaidaMin_2;
                else if (objJornadaAlternativa.SaidaMin_1 >= 0)
                    saida = objJornadaAlternativa.SaidaMin_1;
                else
                {
                    saida = -1;
                    possuihorario = false;
                }
            }
            else
            {
                entrada = entrada_1MinHD;
                limiteMin = limite_min;
                limiteMax = limite_max;
                //Encontra á última Saida
                if (saida_4MinHD >= 0)
                    saida = saida_4MinHD;
                else if (saida_3MinHD >= 0)
                    saida = saida_3MinHD;
                else if (saida_2MinHD >= 0)
                    saida = saida_2MinHD;
                else if (saida_1MinHD >= 0)
                    saida = saida_1MinHD;
                else
                {
                    saida = -1;
                    possuihorario = false;
                }
            }

            int minuto = 0;
            //int minuto_max = 0;
            if (possuihorario)
            {
                if (entrada <= limiteMin)
                {
                    minuto = (1440 - entrada) - (limiteMin - entrada);
                }
                else
                {
                    minuto = entrada - limiteMin;
                }
            }

            //Chama a função que encontra o dia da marcação do Bilhete
            return BLL.CalculoHoras.RegraDiaAnteriorOld(saida, limite_max, possuihorario, ordenabilhetesaida, horabilheteMin, minuto);
        }

        #endregion

        #region Métodos Auxiliares

        private Modelo.Marcacao PreencheMarcacaoDSR(DataRow pMarcacao, string pValorDsr, short pDsr, string pLegenda)
        {
            Modelo.Marcacao obj = new Modelo.Marcacao();
            obj.Valordsr = pValorDsr;
            obj.Dsr = pDsr;
            obj.Legenda = pLegenda;
            obj.Id = Convert.ToInt32(pMarcacao["id"]);
            obj.Dscodigo = Convert.ToString(pMarcacao["dscodigo"]);
            obj.Idfuncionario = Convert.ToInt32(pMarcacao["idfuncionario"]);
            obj.Dia = Convert.ToString(pMarcacao["dia"]);
            obj.Ocorrencia = Convert.ToString(pMarcacao["ocorrencia"]);
            obj.Horastrabalhadas = Convert.ToString(pMarcacao["horastrabalhadas"]);
            obj.Horastrabalhadasnoturnas = Convert.ToString(pMarcacao["horastrabalhadasnoturnas"]);
            obj.Horasextrasdiurna = Convert.ToString(pMarcacao["horasextrasdiurna"]);
            obj.Horasextranoturna = Convert.ToString(pMarcacao["horasextranoturna"]);
            obj.Horasfaltas = Convert.ToString(pMarcacao["horasfaltas"]);
            obj.Horasfaltanoturna = Convert.ToString(pMarcacao["horasfaltanoturna"]);
            obj.Bancohorascre = Convert.ToString(pMarcacao["bancohorascre"]);
            obj.Bancohorasdeb = Convert.ToString(pMarcacao["bancohorasdeb"]);
            obj.Folga = Convert.ToInt16(pMarcacao["folga"]);
            obj.Neutro = Convert.ToBoolean(pMarcacao["neutro"]);
            obj.TotalHorasTrabalhadas = Convert.ToString(pMarcacao["totalHorasTrabalhadas"]);
            obj.Naoconsiderarcafe = Convert.ToInt16(pMarcacao["naoconsiderarcafe"]);
            obj.Naoentrarbanco = Convert.ToInt16(pMarcacao["naoentrarbanco"]);
            obj.Semcalculo = Convert.ToInt16(pMarcacao["semcalculo"]);
            obj.Data = Convert.ToDateTime(pMarcacao["data"]);
            obj.Horascompensadas = Convert.ToString(pMarcacao["horascompensadas"]);
            obj.Idcompensado = pMarcacao["idcompensado"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["idcompensado"]);
            obj.Idhorario = Convert.ToInt32(pMarcacao["idhorario"]);
            obj.Entrada_1 = Convert.ToString(pMarcacao["entrada_1"]);
            obj.Entrada_2 = Convert.ToString(pMarcacao["entrada_2"]);
            obj.Entrada_3 = Convert.ToString(pMarcacao["entrada_3"]);
            obj.Entrada_4 = Convert.ToString(pMarcacao["entrada_4"]);
            obj.Entrada_5 = Convert.ToString(pMarcacao["entrada_5"]);
            obj.Entrada_6 = Convert.ToString(pMarcacao["entrada_6"]);
            obj.Entrada_7 = Convert.ToString(pMarcacao["entrada_7"]);
            obj.Entrada_8 = Convert.ToString(pMarcacao["entrada_8"]);
            obj.Saida_1 = Convert.ToString(pMarcacao["saida_1"]);
            obj.Saida_2 = Convert.ToString(pMarcacao["saida_2"]);
            obj.Saida_3 = Convert.ToString(pMarcacao["saida_3"]);
            obj.Saida_4 = Convert.ToString(pMarcacao["saida_4"]);
            obj.Saida_5 = Convert.ToString(pMarcacao["saida_5"]);
            obj.Saida_6 = Convert.ToString(pMarcacao["saida_6"]);
            obj.Saida_7 = Convert.ToString(pMarcacao["saida_7"]);
            obj.Saida_8 = Convert.ToString(pMarcacao["saida_8"]);
            obj.Entradaextra = Convert.ToString(pMarcacao["entradaextra"]);
            obj.Saidaextra = Convert.ToString(pMarcacao["saidaextra"]);
            obj.Ent_num_relogio_1 = Convert.ToString(pMarcacao["ent_num_relogio_1"]);
            obj.Ent_num_relogio_2 = Convert.ToString(pMarcacao["ent_num_relogio_2"]);
            obj.Ent_num_relogio_3 = Convert.ToString(pMarcacao["ent_num_relogio_3"]);
            obj.Ent_num_relogio_4 = Convert.ToString(pMarcacao["ent_num_relogio_4"]);
            obj.Ent_num_relogio_5 = Convert.ToString(pMarcacao["ent_num_relogio_5"]);
            obj.Ent_num_relogio_6 = Convert.ToString(pMarcacao["ent_num_relogio_6"]);
            obj.Ent_num_relogio_7 = Convert.ToString(pMarcacao["ent_num_relogio_7"]);
            obj.Ent_num_relogio_8 = Convert.ToString(pMarcacao["ent_num_relogio_8"]);
            obj.Sai_num_relogio_1 = Convert.ToString(pMarcacao["sai_num_relogio_1"]);
            obj.Sai_num_relogio_2 = Convert.ToString(pMarcacao["sai_num_relogio_2"]);
            obj.Sai_num_relogio_3 = Convert.ToString(pMarcacao["sai_num_relogio_3"]);
            obj.Sai_num_relogio_4 = Convert.ToString(pMarcacao["sai_num_relogio_4"]);
            obj.Sai_num_relogio_5 = Convert.ToString(pMarcacao["sai_num_relogio_5"]);
            obj.Sai_num_relogio_6 = Convert.ToString(pMarcacao["sai_num_relogio_6"]);
            obj.Sai_num_relogio_7 = Convert.ToString(pMarcacao["sai_num_relogio_7"]);
            obj.Sai_num_relogio_8 = Convert.ToString(pMarcacao["sai_num_relogio_8"]);
            obj.Naoentrarnacompensacao = Convert.ToInt16(pMarcacao["naoentrarnacompensacao"]);
            obj.Idfechamentobh = pMarcacao["idfechamentobh"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["idfechamentobh"]);
            obj.Abonardsr = Convert.ToInt16(pMarcacao["abonardsr"]);
            obj.Totalizadoresalterados = Convert.ToInt16(pMarcacao["totalizadoresalterados"]);
            obj.Calchorasextrasdiurna = Convert.ToInt32(pMarcacao["calchorasextrasdiurna"]);
            obj.Calchorasextranoturna = Convert.ToInt32(pMarcacao["calchorasextranoturna"]);
            obj.Calchorasfaltas = Convert.ToInt32(pMarcacao["calchorasfaltas"]);
            obj.Calchorasfaltanoturna = Convert.ToInt32(pMarcacao["calchorasfaltanoturna"]);
            obj.Incdata = Convert.ToDateTime(pMarcacao["incdata"]);
            obj.Inchora = Convert.ToDateTime(pMarcacao["inchora"]);
            obj.Incusuario = Convert.ToString(pMarcacao["incusuario"]);
            obj.Codigo = Convert.ToInt32(pMarcacao["codigo"]);
            obj.ExpHorasextranoturna = Convert.ToString(pMarcacao["exphorasextranoturna"]);
            obj.Chave = obj.ToMD5();
            obj.IdFechamentoPonto = pMarcacao["idfechamentoponto"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["idfechamentoponto"]);
            obj.Interjornada = Convert.ToString(pMarcacao["Interjornada"]);
            obj.IdDocumentoWorkflow = pMarcacao["IdDocumentoWorkflow"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["IdDocumentoWorkflow"]);
            obj.DocumentoWorkflowAberto = pMarcacao["DocumentoWorkflowAberto"] is DBNull ? false : Convert.ToBoolean(pMarcacao["DocumentoWorkflowAberto"]);
            obj.InItinereHrsDentroJornada = pMarcacao["InItinereHrsDentroJornada"] is DBNull ? "--:--" : Convert.ToString(pMarcacao["InItinereHrsDentroJornada"]);
            obj.InItinerePercDentroJornada = pMarcacao["InItinerePercDentroJornada"] is DBNull ? 0 : Convert.ToDecimal(pMarcacao["InItinerePercDentroJornada"]);
            obj.InItinereHrsForaJornada = pMarcacao["InItinereHrsForaJornada"] is DBNull ? "--:--" : Convert.ToString(pMarcacao["InItinereHrsForaJornada"]);
            obj.InItinerePercForaJornada = pMarcacao["InItinerePercForaJornada"] is DBNull ? 0 : Convert.ToDecimal(pMarcacao["InItinerePercForaJornada"]);
            obj.NaoConsiderarInItinere = pMarcacao["NaoConsiderarInItinere"] is DBNull ? false : Convert.ToBoolean(pMarcacao["NaoConsiderarInItinere"]);
            obj.LegendasConcatenadas = Convert.ToString(pMarcacao["LegendasConcatenadas"]);
            obj.AdicionalNoturno = Convert.ToString(pMarcacao["AdicionalNoturno"]);
            obj.DataBloqueioEdicaoPnlRh = pMarcacao["DataBloqueioEdicaoPnlRh"] is DBNull ? (DateTime?)null : Convert.ToDateTime(pMarcacao["DataBloqueioEdicaoPnlRh"]);
            obj.LoginBloqueioEdicaoPnlRh = pMarcacao["LoginBloqueioEdicaoPnlRh"] is DBNull ? null : Convert.ToString(pMarcacao["LoginBloqueioEdicaoPnlRh"]);
            obj.horaExtraInterjornada = Convert.ToString(pMarcacao["horaExtraInterjornada"]);
            obj.HorasTrabalhadasDentroFeriadoDiurna = Convert.ToString(pMarcacao["horasTrabalhadasDentroFeriadoDiurna"]);
            obj.HorasTrabalhadasDentroFeriadoNoturna = Convert.ToString(pMarcacao["horasTrabalhadasDentroFeriadoNoturna"]);
            obj.HorasPrevistasDentroFeriadoDiurna = Convert.ToString(pMarcacao["horasPrevistasDentroFeriadoDiurna"]);
            obj.HorasPrevistasDentroFeriadoNoturna = Convert.ToString(pMarcacao["horasPrevistasDentroFeriadoNoturna"]);
            obj.NaoConsiderarFeriado = Convert.ToInt16(pMarcacao["naoconsiderarferiado"]);
            obj.ContabilizarFaltas = Convert.ToInt16(pMarcacao["ContabilizarFaltas"]);
            obj.ContAtrasosSaidasAntec = Convert.ToInt16(pMarcacao["ContAtrasosSaidasAntec"]);
            obj.ContabilizarCreditos = Convert.ToInt16(pMarcacao["ContabilizarCreditos"]);
            obj.IdJornadaSubstituir = pMarcacao["IdJornadaSubstituir"] is DBNull ? (int?)null : Convert.ToInt32(pMarcacao["IdJornadaSubstituir"]);
            return obj;
        }

        private void PreencheMarcacao(DataRow pMarcacao)
        {
            objMarcacao = new Modelo.Marcacao();

            objMarcacao.Id = id;
            objMarcacao.Idfuncionario = idFuncionario;
            objMarcacao.Dia = dia;
            objMarcacao.Ocorrencia = ocorrencia;
            objMarcacao.Dscodigo = dscodigo;
            objMarcacao.Horastrabalhadas = Modelo.cwkFuncoes.ConvertMinutosHora(horasTrabalhadasMin);
            objMarcacao.Horastrabalhadasnoturnas = Modelo.cwkFuncoes.ConvertMinutosHora(horasTrabalhadasNoturnasMin);
            objMarcacao.Horasextrasdiurna = Modelo.cwkFuncoes.ConvertMinutosHora(horasExtrasDiurnaMin);
            objMarcacao.Horasextranoturna = Modelo.cwkFuncoes.ConvertMinutosHora(horasExtraNoturnaMin);
            objMarcacao.Horasfaltas = Modelo.cwkFuncoes.ConvertMinutosHora(horasFaltasMin);
            objMarcacao.Horasfaltanoturna = Modelo.cwkFuncoes.ConvertMinutosHora(horasFaltaNoturnaMin);
            objMarcacao.Bancohorascre = bancoHorasCre;
            objMarcacao.Bancohorasdeb = bancoHorasDeb;
            objMarcacao.Legenda = legenda.Trim();
            objMarcacao.Dsr = dsr;
            objMarcacao.Folga = folgaMarcacao;
            objMarcacao.Neutro = neutroMarcacao;
            objMarcacao.TotalHorasTrabalhadas = totalHorasTrabalhadas;
            objMarcacao.TotalIntervalo = totalHorasIntervalo;
            objMarcacao.IdDocumentoWorkflow = IdDocumentoWorkflow;
            objMarcacao.DocumentoWorkflowAberto = documentoWorkflowAberto;
            objMarcacao.InItinereHrsDentroJornada = InItinereHrsDentroJornada;
            objMarcacao.InItinerePercDentroJornada = InItinerePercDentroJornada;
            objMarcacao.InItinereHrsForaJornada = InItinereHrsForaJornada;
            objMarcacao.InItinerePercForaJornada = InItinerePercForaJornada;
            objMarcacao.NaoConsiderarInItinere = NaoConsiderarInItinere;
            objMarcacao.Naoconsiderarcafe = naoConsiderarCafe;
            objMarcacao.Naoentrarbanco = naoEntrarBanco;
            objMarcacao.Semcalculo = semCalculo;
            objMarcacao.Data = data;
            objMarcacao.ExpHorasextranoturna = expHorasExtraNot;
            objMarcacao.Idhorario = Convert.ToInt32(pMarcacao["idhorario"]);
            objMarcacao.Entradaextra = Convert.ToString(pMarcacao["entradaextra"]);
            objMarcacao.Saidaextra = Convert.ToString(pMarcacao["saidaextra"]);
            objMarcacao.TipoHoraExtraFalta = Convert.ToInt16(separaExtraFalta);

            objMarcacao.Entrada_1 = Convert.ToString(pMarcacao["entrada_1"]);
            objMarcacao.Entrada_2 = Convert.ToString(pMarcacao["entrada_2"]);
            objMarcacao.Entrada_3 = Convert.ToString(pMarcacao["entrada_3"]);
            objMarcacao.Entrada_4 = Convert.ToString(pMarcacao["entrada_4"]);
            objMarcacao.Entrada_5 = Convert.ToString(pMarcacao["entrada_5"]);
            objMarcacao.Entrada_6 = Convert.ToString(pMarcacao["entrada_6"]);
            objMarcacao.Entrada_7 = Convert.ToString(pMarcacao["entrada_7"]);
            objMarcacao.Entrada_8 = Convert.ToString(pMarcacao["entrada_8"]);
            objMarcacao.Saida_1 = Convert.ToString(pMarcacao["saida_1"]);
            objMarcacao.Saida_2 = Convert.ToString(pMarcacao["saida_2"]);
            objMarcacao.Saida_3 = Convert.ToString(pMarcacao["saida_3"]);
            objMarcacao.Saida_4 = Convert.ToString(pMarcacao["saida_4"]);
            objMarcacao.Saida_5 = Convert.ToString(pMarcacao["saida_5"]);
            objMarcacao.Saida_6 = Convert.ToString(pMarcacao["saida_6"]);
            objMarcacao.Saida_7 = Convert.ToString(pMarcacao["saida_7"]);
            objMarcacao.Saida_8 = Convert.ToString(pMarcacao["saida_8"]);
            objMarcacao.Ent_num_relogio_1 = Convert.ToString(pMarcacao["ent_num_relogio_1"]);
            objMarcacao.Ent_num_relogio_2 = Convert.ToString(pMarcacao["ent_num_relogio_2"]);
            objMarcacao.Ent_num_relogio_3 = Convert.ToString(pMarcacao["ent_num_relogio_3"]);
            objMarcacao.Ent_num_relogio_4 = Convert.ToString(pMarcacao["ent_num_relogio_4"]);
            objMarcacao.Ent_num_relogio_5 = Convert.ToString(pMarcacao["ent_num_relogio_5"]);
            objMarcacao.Ent_num_relogio_6 = Convert.ToString(pMarcacao["ent_num_relogio_6"]);
            objMarcacao.Ent_num_relogio_7 = Convert.ToString(pMarcacao["ent_num_relogio_7"]);
            objMarcacao.Ent_num_relogio_8 = Convert.ToString(pMarcacao["ent_num_relogio_8"]);
            objMarcacao.Sai_num_relogio_1 = Convert.ToString(pMarcacao["sai_num_relogio_1"]);
            objMarcacao.Sai_num_relogio_2 = Convert.ToString(pMarcacao["sai_num_relogio_2"]);
            objMarcacao.Sai_num_relogio_3 = Convert.ToString(pMarcacao["sai_num_relogio_3"]);
            objMarcacao.Sai_num_relogio_4 = Convert.ToString(pMarcacao["sai_num_relogio_4"]);
            objMarcacao.Sai_num_relogio_5 = Convert.ToString(pMarcacao["sai_num_relogio_5"]);
            objMarcacao.Sai_num_relogio_6 = Convert.ToString(pMarcacao["sai_num_relogio_6"]);
            objMarcacao.Sai_num_relogio_7 = Convert.ToString(pMarcacao["sai_num_relogio_7"]);
            objMarcacao.Sai_num_relogio_8 = Convert.ToString(pMarcacao["sai_num_relogio_8"]);

            objMarcacao.Naoentrarnacompensacao = pMarcacao["naoentrarnacompensacao"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(pMarcacao["naoentrarnacompensacao"]);
            objMarcacao.Horascompensadas = horasCompensadas;
            objMarcacao.Idcompensado = idCompensado != null ? idCompensado.Value : 0;
            objMarcacao.Idfechamentobh = idFechamentoBH;
            objMarcacao.IdFechamentoPonto = pMarcacao["idfechamentoponto"] is DBNull ? (Int32)0 : Convert.ToInt32(pMarcacao["idfechamentoponto"]); ;
            objMarcacao.Interjornada = Interjornada;
            objMarcacao.Valordsr = "--:--";
            objMarcacao.Abonardsr = pMarcacao["abonardsr"] is DBNull ? (Int16)0 : Convert.ToInt16(pMarcacao["abonardsr"]);
            objMarcacao.Totalizadoresalterados = pMarcacao["totalizadoresalterados"] is DBNull ? (Int16)0 : Convert.ToInt16(pMarcacao["totalizadoresalterados"]);
            objMarcacao.Calchorasextrasdiurna = pMarcacao["calchorasextrasdiurna"] is DBNull ? (Int16)0 : Convert.ToInt32(pMarcacao["calchorasextrasdiurna"]);
            objMarcacao.Calchorasextranoturna = pMarcacao["calchorasextranoturna"] is DBNull ? (Int16)0 : Convert.ToInt32(pMarcacao["calchorasextranoturna"]);
            objMarcacao.Calchorasfaltas = pMarcacao["calchorasfaltas"] is DBNull ? (Int16)0 : Convert.ToInt32(pMarcacao["calchorasfaltas"]);
            objMarcacao.Calchorasfaltanoturna = pMarcacao["calchorasfaltanoturna"] is DBNull ? (Int16)0 : Convert.ToInt32(pMarcacao["calchorasfaltanoturna"]);
            objMarcacao.Incdata = pMarcacao["incdata"] is DBNull ? DateTime.Now : Convert.ToDateTime(pMarcacao["incdata"]);
            objMarcacao.Inchora = pMarcacao["inchora"] is DBNull ? DateTime.Now : Convert.ToDateTime(pMarcacao["inchora"]);
            objMarcacao.Incusuario = pMarcacao["incusuario"] is DBNull ? "cwork" : Convert.ToString(pMarcacao["incusuario"]);
            objMarcacao.Codigo = Convert.ToInt32(pMarcacao["codigo"]);
            objMarcacao.Chave = objMarcacao.ToMD5();
            objMarcacao.LegendasConcatenadas = LegendasConcatenadas;
            objMarcacao.AdicionalNoturno = Modelo.cwkFuncoes.ConvertMinutosHora(AdicionalNoturno);
            objMarcacao.DataBloqueioEdicaoPnlRh = pMarcacao["DataBloqueioEdicaoPnlRh"] is DBNull ? (DateTime?)null : Convert.ToDateTime(pMarcacao["DataBloqueioEdicaoPnlRh"]);
            objMarcacao.LoginBloqueioEdicaoPnlRh = pMarcacao["LoginBloqueioEdicaoPnlRh"] is DBNull ? null : Convert.ToString(pMarcacao["LoginBloqueioEdicaoPnlRh"]);
            objMarcacao.horaExtraInterjornada = horaExtraInterjornada;
            objMarcacao.HorasTrabalhadasDentroFeriadoDiurna = Modelo.cwkFuncoes.ConvertMinutosHora(horasTrabalhadasDentroFeriadoParcialDiurna);
            objMarcacao.HorasTrabalhadasDentroFeriadoNoturna = Modelo.cwkFuncoes.ConvertMinutosHora(horasTrabalhadasDentroFeriadoParcialNoturna);
            objMarcacao.HorasPrevistasDentroFeriadoDiurna = Modelo.cwkFuncoes.ConvertMinutosHora(horasPrevistasDentroFeriadoParcialDiurna);
            objMarcacao.HorasPrevistasDentroFeriadoNoturna = Modelo.cwkFuncoes.ConvertMinutosHora(horasPrevistasDentroFeriadoParcialNoturna);
            objMarcacao.NaoConsiderarFeriado = Convert.ToInt16(pMarcacao["naoconsiderarferiado"]);
            objMarcacao.ContabilizarFaltas = Convert.ToInt16(pMarcacao["ContabilizarFaltas"]);
            objMarcacao.ContAtrasosSaidasAntec = Convert.ToInt16(pMarcacao["ContAtrasosSaidasAntec"]);
            objMarcacao.ContabilizarCreditos = Convert.ToInt16(pMarcacao["ContabilizarCreditos"]);
            objMarcacao.IdJornadaSubstituir = idJornadaSubstituir;
        }

        private void SetaVariaveisMarcacao(DataRow pMarcacao)
        {
            idCompensado = pMarcacao["idcompensado"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["idcompensado"]);
            id = Convert.ToInt32(pMarcacao["id"]);

            data = Convert.ToDateTime(pMarcacao["data"]);
            dia = Modelo.cwkFuncoes.DiaSemana(data, Modelo.cwkFuncoes.TipoDiaSemana.Reduzido);
            diaInt = Modelo.cwkFuncoes.Dia(data);
            dataAdmissao = pMarcacao["dataadmissao"] is DBNull ? null : (DateTime?)(pMarcacao["dataadmissao"]);
            dataDemissao = pMarcacao["datademissao"] is DBNull ? null : (DateTime?)(pMarcacao["datademissao"]);
            dataInativacao = pMarcacao["dataInativacao"] is DBNull ? null : (DateTime?)(pMarcacao["dataInativacao"]);

            //Seta o id do funcionário
            idFuncionario = Convert.ToInt32(pMarcacao["idfuncionario"]);
            horasTrabalhadasMin = Convert.ToInt32(pMarcacao["horastrabalhadasmin"]);
            horasTrabalhadasNoturnasMin = Convert.ToInt32(pMarcacao["horastrabalhadasnoturnasmin"]);
            horasFaltasMin = Convert.ToInt32(pMarcacao["horasfaltasmin"]);
            horasFaltaNoturnaMin = Convert.ToInt32(pMarcacao["horasfaltanoturnamin"]);
            horasExtrasDiurnaMin = Convert.ToInt32(pMarcacao["horasextrasdiurnamin"]);
            horasExtraNoturnaMin = Convert.ToInt32(pMarcacao["horasextranoturnamin"]);
            valorDsrMin = Convert.ToInt32(pMarcacao["valordsrmin"]);
            horasCompensadasMin = Convert.ToInt32(pMarcacao["horascompensadasmin"]);
            naoEntrarBanco = pMarcacao["naoentrarbanco"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(pMarcacao["naoentrarbanco"]);
            naoEntrarBancoFunc = pMarcacao["naoentrarbancofunc"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(pMarcacao["naoentrarbancofunc"]);
            naoConsiderarCafe = pMarcacao["naoconsiderarcafe"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(pMarcacao["naoconsiderarcafe"]);
            semCalculo = pMarcacao["semcalculo"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(pMarcacao["semcalculo"]);
            entrada_1Min = Convert.ToInt32(pMarcacao["entrada_1min"]);
            entrada_2Min = Convert.ToInt32(pMarcacao["entrada_2min"]);
            entrada_3Min = Convert.ToInt32(pMarcacao["entrada_3min"]);
            entrada_4Min = Convert.ToInt32(pMarcacao["entrada_4min"]);
            entrada_5Min = Convert.ToInt32(pMarcacao["entrada_5min"]);
            entrada_6Min = Convert.ToInt32(pMarcacao["entrada_6min"]);
            entrada_7Min = Convert.ToInt32(pMarcacao["entrada_7min"]);
            entrada_8Min = Convert.ToInt32(pMarcacao["entrada_8min"]);
            saida_1Min = Convert.ToInt32(pMarcacao["saida_1min"]);
            saida_2Min = Convert.ToInt32(pMarcacao["saida_2min"]);
            saida_3Min = Convert.ToInt32(pMarcacao["saida_3min"]);
            saida_4Min = Convert.ToInt32(pMarcacao["saida_4min"]);
            saida_5Min = Convert.ToInt32(pMarcacao["saida_5min"]);
            saida_6Min = Convert.ToInt32(pMarcacao["saida_6min"]);
            saida_7Min = Convert.ToInt32(pMarcacao["saida_7min"]);
            saida_8Min = Convert.ToInt32(pMarcacao["saida_8min"]);

            //CorrigeEntradasESaidas();

            if (pMarcacao["folga"] is DBNull)
            {
                folgaMarcacao = 0;
            }
            else
            {
                folgaMarcacao = Convert.ToInt16(pMarcacao["folga"]);
            }

            if (pMarcacao["neutro"] is DBNull)
            {
                neutroMarcacao = false;
            }
            else
            {
                neutroMarcacao = flagNeutro;
            }

            if (pMarcacao["totalHorasTrabalhadas"] is DBNull)
            {
                totalHorasTrabalhadas = "--:--";
            }
            else
            {
                totalHorasTrabalhadas = Convert.ToString(pMarcacao["totalHorasTrabalhadas"]);
            }

            id = Convert.ToInt32(pMarcacao["id"]);
            dscodigo = Convert.ToString(pMarcacao["dscodigo"]);
            idFeriado = pMarcacao["idferiado"] is DBNull ? null : (Int32?)(pMarcacao["idferiado"]);
            feriadoParcial = pMarcacao["FeriadoParcial"] is DBNull ? false : (bool)(pMarcacao["FeriadoParcial"]);
            feriadoParcialInicio = Convert.ToString(pMarcacao["FeriadoParcialInicio"]);
            feriadoParcialInicioMin = Modelo.cwkFuncoes.ConvertHorasMinuto(feriadoParcialInicio);
            feriadoParcialFim = Convert.ToString(pMarcacao["FeriadoParcialFim"]);
            feriadoParcialFimMin = Modelo.cwkFuncoes.ConvertHorasMinuto(feriadoParcialFim);
            idMudancaHorario = pMarcacao["idmudancahorario"] is DBNull ? null : (Int32?)(pMarcacao["idmudancahorario"]);
            idBancoHoras = pMarcacao["idbancohoras"] is DBNull ? null : (Int32?)(pMarcacao["idbancohoras"]);
            idFechamentoBH = pMarcacao["idfechamentobh"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["idfechamentobh"]);
            idFechamentoPonto = pMarcacao["idfechamentoPonto"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["idfechamentoPonto"]);
            Interjornada = Convert.ToString(pMarcacao["Interjornada"]);
            IdDocumentoWorkflow = pMarcacao["IdDocumentoWorkflow"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["IdDocumentoWorkflow"]);
            documentoWorkflowAberto = pMarcacao["DocumentoWorkflowAberto"] is DBNull ? false : Convert.ToBoolean(pMarcacao["DocumentoWorkflowAberto"]);
            InItinereHrsDentroJornada = pMarcacao["InItinereHrsDentroJornada"] is DBNull ? "--:--" : Convert.ToString(pMarcacao["InItinereHrsDentroJornada"]);
            InItinerePercDentroJornada = pMarcacao["InItinerePercDentroJornada"] is DBNull ? 0 : Convert.ToDecimal(pMarcacao["InItinerePercDentroJornada"]);
            InItinereHrsForaJornada = pMarcacao["InItinereHrsForaJornada"] is DBNull ? "--:--" : Convert.ToString(pMarcacao["InItinereHrsForaJornada"]);
            InItinerePercForaJornada = pMarcacao["InItinerePercForaJornada"] is DBNull ? 0 : Convert.ToDecimal(pMarcacao["InItinerePercForaJornada"]);
            NaoConsiderarInItinere = pMarcacao["NaoConsiderarInItinere"] is DBNull ? false : Convert.ToBoolean(pMarcacao["NaoConsiderarInItinere"]);
            idFuncionario = Convert.ToInt32(pMarcacao["idfuncionario"]);
            idFuncao = Convert.ToInt32(pMarcacao["idfuncao"]);
            idDepartamento = Convert.ToInt32(pMarcacao["iddepartamento"]);
            idEmpresa = Convert.ToInt32(pMarcacao["idempresa"]);
            expHorasExtraNot = Convert.ToString(pMarcacao["exphorasextranoturna"]);

            tratamentosMarcacao = tratamentomarcacaoList.Where(t => t.Mar_data == data && t.DsCodigo == dscodigo).ToList();

            DataBloqueioEdicaoPnlRh = pMarcacao["DataBloqueioEdicaoPnlRh"] is DBNull ? (DateTime?)null : Convert.ToDateTime(pMarcacao["DataBloqueioEdicaoPnlRh"]);
            LoginBloqueioEdicaoPnlRh = pMarcacao["LoginBloqueioEdicaoPnlRh"] is DBNull ? null : Convert.ToString(pMarcacao["LoginBloqueioEdicaoPnlRh"]);
            horaExtraInterjornada = Convert.ToString(pMarcacao["horaExtraInterjornada"]);
            horasTrabalhadasDentroFeriadoParcialDiurna = Modelo.cwkFuncoes.ConvertHorasMinuto(Convert.ToString(pMarcacao["horasTrabalhadasDentroFeriadoDiurna"]));
            horasTrabalhadasDentroFeriadoParcialNoturna = Modelo.cwkFuncoes.ConvertHorasMinuto(Convert.ToString(pMarcacao["horasTrabalhadasDentroFeriadoNoturna"]));
            horasPrevistasDentroFeriadoParcialDiurna = Modelo.cwkFuncoes.ConvertHorasMinuto(Convert.ToString(pMarcacao["horasPrevistasDentroFeriadoDiurna"]));
            horasPrevistasDentroFeriadoParcialNoturna = Modelo.cwkFuncoes.ConvertHorasMinuto(Convert.ToString(pMarcacao["horasPrevistasDentroFeriadoNoturna"]));
            naoConsiderarFeriado = pMarcacao["naoconsiderarferiado"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(pMarcacao["naoconsiderarferiado"]);

            legenda = Convert.ToString(pMarcacao["Legenda"]).Trim();
            bancoHorasCre = Convert.ToString(pMarcacao["Bancohorascre"]);
            bancoHorasDeb = Convert.ToString(pMarcacao["Bancohorasdeb"]);
            horasCompensadas = Convert.ToString(pMarcacao["Horascompensadas"]);
            ocorrencia = Convert.ToString(pMarcacao["Ocorrencia"]);
            LegendasConcatenadas = Convert.ToString(pMarcacao["LegendasConcatenadas"]);
            dsr = Convert.ToInt16(pMarcacao["Dsr"]);
            AdicionalNoturno = pMarcacao["AdicionalNoturno"] is DBNull ? 0 : Modelo.cwkFuncoes.ConvertHorasMinuto(Convert.ToString(pMarcacao["AdicionalNoturno"]));
            ContabilizarFaltasMarc = pMarcacao["ContabilizarFaltas"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(pMarcacao["ContabilizarFaltas"]);
            ContAtrasosSaidasAntecMarc = pMarcacao["ContAtrasosSaidasAntec"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(pMarcacao["ContAtrasosSaidasAntec"]);
            ContabilizarCreditosMarc = pMarcacao["ContabilizarCreditos"] is DBNull ? Convert.ToInt16(0) : Convert.ToInt16(pMarcacao["ContabilizarCreditos"]);
            idJornadaAlternativa = pMarcacao["idjornadaalternativa"] is DBNull ? null : (int?)(pMarcacao["idjornadaalternativa"]);
        }

        private void SetaVariaveisAfastamento(DataRow pMarcacao)
        {
            if (pMarcacao["idafastamentofunc"] is DBNull)
                idAfastamentoFunc = null;
            else
                idAfastamentoFunc = Convert.ToInt32(pMarcacao["idafastamentofunc"]);

            if (pMarcacao["idafastamentodep"] is DBNull)
                idAfastamentoDep = null;
            else
                idAfastamentoDep = Convert.ToInt32(pMarcacao["idafastamentodep"]);

            if (pMarcacao["idafastamentoemp"] is DBNull)
                idAfastamentoEmp = null;
            else
                idAfastamentoEmp = Convert.ToInt32(pMarcacao["idafastamentoemp"]);

            if (pMarcacao["idafastamentocont"] is DBNull)
                idAfastamentoCont = null;
            else
                idAfastamentoCont = Convert.ToInt32(pMarcacao["idafastamentocont"]);

            if (pMarcacao["idocorrenciafunc"] is DBNull)
                idOcorrenciaFunc = null;
            else
                idOcorrenciaFunc = Convert.ToInt32(pMarcacao["idocorrenciafunc"]);

            if (pMarcacao["idocorrenciadep"] is DBNull)
                idOcorrenciaDep = null;
            else
                idOcorrenciaDep = Convert.ToInt32(pMarcacao["idocorrenciadep"]);

            if (pMarcacao["idocorrenciaemp"] is DBNull)
                idOcorrenciaEmp = null;
            else
                idOcorrenciaEmp = Convert.ToInt32(pMarcacao["idocorrenciaemp"]);

            if (pMarcacao["idocorrenciacont"] is DBNull)
                idOcorrenciaCont = null;
            else
                idOcorrenciaCont = Convert.ToInt32(pMarcacao["idocorrenciacont"]);

            if (pMarcacao["abonadofunc"] is DBNull)
                abonadoFunc = null;
            else
                abonadoFunc = Convert.ToInt16(pMarcacao["abonadofunc"]);

            if (pMarcacao["abonadodep"] is DBNull)
                abonadoDep = null;
            else
                abonadoDep = Convert.ToInt16(pMarcacao["abonadodep"]);

            if (pMarcacao["abonadoemp"] is DBNull)
                abonadoEmp = null;
            else
                abonadoEmp = Convert.ToInt16(pMarcacao["abonadoemp"]);

            if (pMarcacao["abonadocont"] is DBNull)
                abonadoCont = null;
            else
                abonadoCont = Convert.ToInt16(pMarcacao["abonadocont"]);


            if (pMarcacao["semcalculofunc"] is DBNull)
                SemCalculoFunc = null;
            else
                SemCalculoFunc = Convert.ToInt16(pMarcacao["semcalculofunc"]);

            if (pMarcacao["semcalculodep"] is DBNull)
                SemCalculoDep = null;
            else
                SemCalculoDep = Convert.ToInt16(pMarcacao["semcalculodep"]);

            if (pMarcacao["semcalculoemp"] is DBNull)
                SemCalculoEmp = null;
            else
                SemCalculoEmp = Convert.ToInt16(pMarcacao["semcalculoemp"]);

            if (pMarcacao["semcalculocont"] is DBNull)
                SemCalculoCont = null;
            else
                SemCalculoCont = Convert.ToInt16(pMarcacao["semcalculocont"]);

            if (pMarcacao["semabonofunc"] is DBNull)
                SemAbonoFunc = null;
            else
                SemAbonoFunc = Convert.ToInt16(pMarcacao["semabonofunc"]);

            if (pMarcacao["semabonodep"] is DBNull)
                SemAbonoDep = null;
            else
                SemAbonoDep = Convert.ToInt16(pMarcacao["semabonodep"]);

            if (pMarcacao["semabonoemp"] is DBNull)
                SemAbonoEmp = null;
            else
                SemAbonoEmp = Convert.ToInt16(pMarcacao["semabonoemp"]);

            if (pMarcacao["semabonocont"] is DBNull)
                SemAbonoCont = null;
            else
                SemAbonoCont = Convert.ToInt16(pMarcacao["semabonocont"]);

            if (pMarcacao["horaifunc"] is DBNull)
                horaiFunc = null;
            else
                horaiFunc = Convert.ToString(pMarcacao["horaifunc"]);

            if (pMarcacao["horaidep"] is DBNull)
                horaiDep = null;
            else
                horaiDep = Convert.ToString(pMarcacao["horaidep"]);

            if (pMarcacao["horaiemp"] is DBNull)
                horaiEmp = null;
            else
                horaiEmp = Convert.ToString(pMarcacao["horaiemp"]);

            if (pMarcacao["horaicont"] is DBNull)
                horaiCont = null;
            else
                horaiCont = Convert.ToString(pMarcacao["horaicont"]);

            if (pMarcacao["horaffunc"] is DBNull)
                horafFunc = null;
            else
                horafFunc = Convert.ToString(pMarcacao["horaffunc"]);

            if (pMarcacao["horafdep"] is DBNull)
                horafDep = null;
            else
                horafDep = Convert.ToString(pMarcacao["horafdep"]);

            if (pMarcacao["horafemp"] is DBNull)
                horafEmp = null;
            else
                horafEmp = Convert.ToString(pMarcacao["horafemp"]);

            if (pMarcacao["horafcont"] is DBNull)
                horafCont = null;
            else
                horafCont = Convert.ToString(pMarcacao["horafcont"]);

            if (pMarcacao["contabilizarjornadafunc"] is DBNull)
                contabilizarjornadaFunc = null;
            else
                contabilizarjornadaFunc = Convert.ToInt16(pMarcacao["contabilizarjornadafunc"]);

            if (pMarcacao["contabilizarjornadadep"] is DBNull)
                contabilizarjornadaDep = null;
            else
                contabilizarjornadaDep = Convert.ToInt16(pMarcacao["contabilizarjornadadep"]);

            if (pMarcacao["contabilizarjornadacont"] is DBNull)
                contabilizarjornadaCont = null;
            else
                contabilizarjornadaCont = Convert.ToInt16(pMarcacao["contabilizarjornadacont"]);

            if (pMarcacao["contabilizarjornadaemp"] is DBNull)
                contabilizarjornadaEmp = null;
            else
                contabilizarjornadaEmp = Convert.ToInt16(pMarcacao["contabilizarjornadaemp"]);


        }

        private void SetaVariaveisHorario(DataRow pMarcacao)
        {
            tipoHorario = Convert.ToInt32(pMarcacao["tipohorario"]);
            diaSemanaDsr = Convert.ToInt32(pMarcacao["diasemanadsr"]);
            marcaCargaHorariaMistaHorario = Convert.ToInt16(pMarcacao["marcacargahorariamistahorario"]);

            if (pMarcacao["diadsr"] is DBNull) { diaDsr = null; }
            else { diaDsr = Convert.ToInt16(pMarcacao["diadsr"]); }

            consideraAdHTrabalhadas = Convert.ToInt16(pMarcacao["consideraadhtrabalhadas"]);
            consideraradicionalnoturnointerv = Convert.ToInt16(pMarcacao["consideraradicionalnoturnointerv"] is DBNull ? 0 : pMarcacao["consideraradicionalnoturnointerv"]);
            conversaoHoranoturna = Convert.ToInt16(pMarcacao["conversaohoranoturna"]);
            toleranciaAdicionalNoturno = Convert.ToInt16(pMarcacao["toleranciaAdicionalNoturno"]);
            totalTrabalhadaDiurna = pMarcacao["totaltrabalhadadiurna"] is DBNull ? "--:--" : pMarcacao["totaltrabalhadadiurna"].ToString();
            totalTrabalhadaNoturna = pMarcacao["totaltrabalhadanoturna"] is DBNull ? "--:--" : pMarcacao["totaltrabalhadanoturna"].ToString();
            totalTrabalhadaDiurnaMin = pMarcacao["totaltrabalhadadiurnamin"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["totaltrabalhadadiurnamin"]);
            totalTrabalhadaNoturnaMin = pMarcacao["totaltrabalhadanoturnamin"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["totaltrabalhadanoturnamin"]);
            cargaHorariaMistaMin = pMarcacao["cargahorariamistamin"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["cargahorariamistamin"]);
            cargaHorariaMista = pMarcacao["cargahorariamista"] is DBNull ? "--:--" : pMarcacao["cargahorariamista"].ToString();
            habilitaPeriodo01 = Convert.ToInt16(pMarcacao["habilitaperiodo01"]);
            habilitaPeriodo02 = Convert.ToInt16(pMarcacao["habilitaperiodo02"]);
            dias_cafe_1 = Convert.ToInt16(pMarcacao["dias_cafe_1"]);
            dias_cafe_2 = Convert.ToInt16(pMarcacao["dias_cafe_2"]);
            dias_cafe_3 = Convert.ToInt16(pMarcacao["dias_cafe_3"]);
            dias_cafe_4 = Convert.ToInt16(pMarcacao["dias_cafe_4"]);
            dias_cafe_5 = Convert.ToInt16(pMarcacao["dias_cafe_5"]);
            dias_cafe_6 = Convert.ToInt16(pMarcacao["dias_cafe_6"]);
            dias_cafe_7 = Convert.ToInt16(pMarcacao["dias_cafe_7"]);
            idJornadaHorario = pMarcacao["IdJornadaHorario"] is DBNull ? null : (Int32?)pMarcacao["IdJornadaHorario"];
            idFuncionario = Convert.ToInt32(pMarcacao["idfuncionario"]);
            data = Convert.ToDateTime(pMarcacao["data"]);
            PxyJornadaSubstituirCalculo jornadaSubstituir = pxyJornadaSubstituirCalculos.Where(w => w.IdFuncionario == idFuncionario && data >= w.DataInicio && data <= w.DataFim && w.IdJornadaDe == idJornadaHorario).OrderByDescending(o => o.IncHora).FirstOrDefault();
            idJornadaSubstituir = (jornadaSubstituir != null && jornadaSubstituir.Id > 0) ? (int?)jornadaSubstituir.Id : null;
            if (idJornadaSubstituir != null)
            {
                entrada_1MinHD = jornadaSubstituir.Entrada1.ConvertHorasMinuto();
                entrada_2MinHD = jornadaSubstituir.Entrada2.ConvertHorasMinuto();
                entrada_3MinHD = jornadaSubstituir.Entrada3.ConvertHorasMinuto();
                entrada_4MinHD = jornadaSubstituir.Entrada4.ConvertHorasMinuto();
                saida_1MinHD = jornadaSubstituir.Saida1.ConvertHorasMinuto();
                saida_2MinHD = jornadaSubstituir.Saida2.ConvertHorasMinuto();
                saida_3MinHD = jornadaSubstituir.Saida3.ConvertHorasMinuto();
                saida_4MinHD = jornadaSubstituir.Saida4.ConvertHorasMinuto();
            }
            else
            {
                entrada_1MinHD = Convert.ToInt32(pMarcacao["entrada_1minhd"]);
                entrada_2MinHD = Convert.ToInt32(pMarcacao["entrada_2minhd"]);
                entrada_3MinHD = Convert.ToInt32(pMarcacao["entrada_3minhd"]);
                entrada_4MinHD = Convert.ToInt32(pMarcacao["entrada_4minhd"]);
                saida_1MinHD = Convert.ToInt32(pMarcacao["saida_1minhd"]);
                saida_2MinHD = Convert.ToInt32(pMarcacao["saida_2minhd"]);
                saida_3MinHD = Convert.ToInt32(pMarcacao["saida_3minhd"]);
                saida_4MinHD = Convert.ToInt32(pMarcacao["saida_4minhd"]);
            }
            separaExtraFalta = pMarcacao["tipohoraextrafalta"] is DBNull ? 0 : Convert.ToInt32(pMarcacao["tipohoraextrafalta"]);
            tHoraExtra = Convert.ToString(pMarcacao["thoraextra"]);
            tHoraFalta = Convert.ToString(pMarcacao["thorafalta"]);
            tHoraExtraMin = pMarcacao["thoraextramin"] == System.DBNull.Value ? (int?)null : Convert.ToInt32(pMarcacao["thoraextramin"]);
            tHoraExtraEntradaMin = pMarcacao["thoraextraEntradamin"] == System.DBNull.Value ? (int?)null : Convert.ToInt32(pMarcacao["thoraextraEntradamin"]);
            tHoraExtraSaidaMin = pMarcacao["thoraextraSaidamin"] == System.DBNull.Value ? (int?)null : Convert.ToInt32(pMarcacao["thoraextraSaidamin"]);
            tHoraFaltaMin = pMarcacao["thorafaltamin"] == System.DBNull.Value ? (int?)null : Convert.ToInt32(pMarcacao["thorafaltamin"]);
            tHoraFaltaEntradaMin = pMarcacao["thorafaltaEntradamin"] == System.DBNull.Value ? (int?)null : Convert.ToInt32(pMarcacao["thorafaltaEntradamin"]);
            tHoraFaltaSaidaMin = pMarcacao["thorafaltaSaidamin"] == System.DBNull.Value ? (int?)null : Convert.ToInt32(pMarcacao["thorafaltaSaidamin"]);
            tHoraExtraIntervaloMin = pMarcacao["TIntervaloExtra"] == System.DBNull.Value ? (int?)null : Convert.ToInt32(pMarcacao["TIntervaloExtra"]);
            tHoraFaltaIntervaloMin = pMarcacao["TIntervaloFalta"] == System.DBNull.Value ? (int?)null : Convert.ToInt32(pMarcacao["TIntervaloFalta"]);
            inicioAdNoturno = Convert.ToInt32(pMarcacao["inicioadnoturnomin"]);
            fimAdNoturno = Convert.ToInt32(pMarcacao["fimadnoturnomin"]);
            ordenabilhetesaida = Convert.ToInt32(pMarcacao["ordenabilhetesaida"]);
            limite_max = Convert.ToInt32(pMarcacao["limitemax"]);
            limite_min = Convert.ToInt32(pMarcacao["limitemin"]);
            bConsiderarHEFeriadoPHoraNoturna = pMarcacao["bConsiderarHEFeriadoPHoraNoturna"] is DBNull ? false : Convert.ToBoolean(pMarcacao["bConsiderarHEFeriadoPHoraNoturna"]);
            bFlgEstenderPeriodoNoturno = pMarcacao["Flg_Estender_Periodo_Noturno"] is DBNull ? false : Convert.ToBoolean(pMarcacao["Flg_Estender_Periodo_Noturno"]);
            bSepararTrabalhadasNoturnaExtrasNoturna = pMarcacao["Flg_Separar_Trabalhadas_Noturna_Extras_Noturna"] is DBNull ? false : Convert.ToBoolean(pMarcacao["Flg_Separar_Trabalhadas_Noturna_Extras_Noturna"]);
            reducaohoranoturna = Convert.ToString(pMarcacao["reducaohoranoturna"]);
            habilitarControleInItinere = Convert.ToInt16(pMarcacao["HabilitarControleInItinere"]);
            DescontarAtrasoInItinere = pMarcacao["DescontarAtrasoInItinere"] is DBNull ? false : Convert.ToBoolean(pMarcacao["DescontarAtrasoInItinere"]);
            DescontarFaltaInItinere = pMarcacao["DescontarFaltaInItinere"] is DBNull ? false : Convert.ToBoolean(pMarcacao["DescontarFaltaInItinere"]);
            HabilitaInItinere = Convert.ToInt16(pMarcacao["HabilitaInItinere"]);
            diaPossuiInItinere = pMarcacao["DiaPossuiInItinere"] is DBNull ? false : Convert.ToBoolean(pMarcacao["DiaPossuiInItinere"]);
            percentualDentroJornadaInItinere = Convert.ToDecimal(pMarcacao["PercentualDentroJornadaInItinere"] is DBNull ? 0 : pMarcacao["PercentualDentroJornadaInItinere"]);
            percentualForaJornadaInItinere = Convert.ToDecimal(pMarcacao["PercentualForaJornadaInItinere"] is DBNull ? 0 : pMarcacao["PercentualForaJornadaInItinere"]);
            horaExtraInterjornada = pMarcacao["horaExtraInterjornada"] is DBNull ? "--:--" : pMarcacao["horaExtraInterjornada"].ToString();
            pontoPorExcecao = Convert.ToBoolean(pMarcacao["PontoPorExcecao"]);
            //CorrigeEntradasESaidasHD();
            if ((pMarcacao["naoentrarnacompensacao"] is DBNull) && (pMarcacao["naocompensacaofunc"] is DBNull))
            {
                naoEntrarNaCompensacao = Convert.ToInt16(0);
            }
            else
            {
                //Pelo menos um dos valores de entrar na compensação não é nulo
                //O flag é 1 ou no funcionario ou na marcação
                bool naoEntrarCompMarc = pMarcacao["naoentrarnacompensacao"] is DBNull ? false : Convert.ToBoolean(pMarcacao["naoentrarnacompensacao"]);
                bool naoEntrarCompFunc = pMarcacao["naocompensacaofunc"] is DBNull ? false : Convert.ToBoolean(pMarcacao["naocompensacaofunc"]);
                if (naoEntrarCompMarc || naoEntrarCompFunc)
                    naoEntrarNaCompensacao = (short)1;
                else
                    naoEntrarNaCompensacao = (short)0;
            }

            if (pMarcacao["marcacargahorariamistahd"] is DBNull) { marcaCargaHorariaMistaHD = null; }
            else { marcaCargaHorariaMistaHD = Convert.ToInt16(pMarcacao["marcacargahorariamistahd"]); }

            if (pMarcacao["bcarregar"] is DBNull) { bCarregar = null; }
            else { bCarregar = Convert.ToInt16(pMarcacao["bcarregar"]); }

            if (pMarcacao["flagfolga"] is DBNull) { flagFolga = null; }
            else { flagFolga = Convert.ToInt16(pMarcacao["flagfolga"]); }

            if (pMarcacao["flagneutro"] is DBNull) { flagNeutro = false; }
            else { flagNeutro = Convert.ToBoolean(pMarcacao["flagneutro"]); }

            if (pMarcacao["intervaloautomatico"] is DBNull) { intervaloAutomatico = 0; }
            else { intervaloAutomatico = Convert.ToInt16(pMarcacao["intervaloautomatico"]); }

            if (pMarcacao["preassinaladas1"] is DBNull) { Preassinaladas1 = 0; }
            else { Preassinaladas1 = Convert.ToInt16(pMarcacao["preassinaladas1"]); }

            if (pMarcacao["preassinaladas2"] is DBNull) { Preassinaladas2 = 0; }
            else { Preassinaladas2 = Convert.ToInt16(pMarcacao["preassinaladas2"]); }

            if (pMarcacao["preassinaladas3"] is DBNull) { Preassinaladas3 = 0; }
            else { Preassinaladas3 = Convert.ToInt16(pMarcacao["preassinaladas3"]); }

            if (pMarcacao["MomentoPreAssinalado"] is DBNull) { momentoPreAssinalado = 0; }
            else { momentoPreAssinalado = Convert.ToInt16(pMarcacao["MomentoPreAssinalado"]); }
        }

        /// <summary>
        /// Método para retornar um array com todas as batidas válidas
        /// </summary>
        /// <returns>Array Int</returns>
        public int[] GetMarcacoesValidas()
        {
            int[] marcacao = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            int i = 0;

            for (int j = 1; j < 9; j++)
            {
                Modelo.BilhetesImp bi = tratamentosMarcacao.Where(t => t.Ent_sai == "E" && t.Posicao == j && t.Acao != Modelo.Acao.Excluir).FirstOrDefault();
                if (bi != null && bi.Ocorrencia != 'D')
                    marcacao[i++] = (int)typeof(CalculaMarcacao).GetField("entrada_" + j + "Min", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);

                bi = tratamentosMarcacao.Where(t => t.Ent_sai == "S" && t.Posicao == j && t.Acao != Modelo.Acao.Excluir).FirstOrDefault();
                if (bi != null && bi.Ocorrencia != 'D')
                    marcacao[i++] = (int)typeof(CalculaMarcacao).GetField("saida_" + j + "Min", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            }

            if (i < marcacao.Length)
            {
                for (int j = i; j < marcacao.Length; j++)
                {
                    marcacao[j] = -1;
                }
            }

            return marcacao;
        }

        public void GetEntradasSaidasValidas(ref int[] entradas, ref int[] saidas)
        {
            int[] marcacoes = this.GetMarcacoesValidas();
            batidasValidasParaIntinere = marcacoes.Where(w => w != -1).Count() % 2 == 0 ? true : false; // Se marcação estiver impar não gera InItinere
            if (calcularInItinere)
            {
                if (HabilitaInItinere == 1) // Verifica se tem InItinere no horário e qual o tipo do Intinere (no caso Primeiro e último registro de ponto)
                {
                    batidasValidasParaIntinere = marcacoes.Where(w => w != -1).Count() >= 4 ? true : false;
                }
                else if (HabilitaInItinere == 2)
                {
                    batidasValidasParaIntinere = marcacoes.Where(w => w != -1).Count() >= 6 ? true : false;
                }

                if (calcularInItinere && batidasValidasParaIntinere && HabilitaInItinere == 1) // Verifica se tem InItinere no horário e qual o tipo do Intinere (no caso Primeiro e último registro de ponto)
                {
                    int[] marcacaoComIntinere = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
                    int contIn = 0;
                    for (int i = 0; i < marcacoes.Count(); i++)
                    {


                        if (i == 1 && marcacoes[i] >= 0)
                        {
                            marcacaoComIntinere[contIn] = marcacoes[i];
                            contIn++;
                        }

                        if (marcacoes[i + 1] == -1 && i > 1)
                        {
                            if (marcacoes[i] >= 0)
                            {
                                marcacaoComIntinere[contIn] = marcacoes[i - 1];
                                contIn++;
                            }
                        }

                        marcacaoComIntinere[contIn] = marcacoes[i];
                        contIn++;
                        if (contIn == 15)
                        {
                            break;
                        }
                    }
                    marcacoes = marcacaoComIntinere;
                }
            }

            int s = 0, e = 0;
            for (int i = 0; i < 16; i++)
            {
                if ((i % 2) != 0)
                {
                    saidas[s] = marcacoes[i];
                    s++;
                }
                else
                {
                    entradas[e] = marcacoes[i];
                    e++;
                }
            }

            if (feriadoParcial)
            {
                CalculaHorasTrabalhadasNoFeriado(entradas, saidas, inicioAdNoturno, fimAdNoturno, feriadoParcialInicioMin, feriadoParcialFimMin, out horasTrabalhadasDentroFeriadoParcialDiurna, out horasTrabalhadasDentroFeriadoParcialNoturna);
                RemoveRegistrosDentroFeriadoParcial(ref entradas, ref saidas);
            }
            else
            {
                horasTrabalhadasDentroFeriadoParcialDiurna = 0;
                horasTrabalhadasDentroFeriadoParcialNoturna = 0;
            }
        }

        public static void CalculaHorasTrabalhadasNoFeriado(int[] entradas, int[] saidas, int inicioAdNoturno, int fimAdNoturno, int feriadoParcialInicioMin, int feriadoParcialFimMin, out int horasTrabalhadasDentroFeriadoParcialDiurna, out int horasTrabalhadasDentroFeriadoParcialNoturna)
        {
            int[] EntradaDentroFeriado = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 };
            int[] SaidaDentroFeriado = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 };
            for (int i = 0; i < entradas.Length; i++)
            {
                if (EstaContidoNoFeriadoParcial(entradas[i], feriadoParcialInicioMin, feriadoParcialFimMin) && EstaContidoNoFeriadoParcial(saidas[i], feriadoParcialInicioMin, feriadoParcialFimMin))
                {
                    EntradaDentroFeriado[i] = entradas[i];
                    SaidaDentroFeriado[i] = saidas[i];
                }
                else if (EstaContidoNoFeriadoParcial(entradas[i], feriadoParcialInicioMin, feriadoParcialFimMin) && !EstaContidoNoFeriadoParcial(saidas[i], feriadoParcialInicioMin, feriadoParcialFimMin))
                {
                    EntradaDentroFeriado[i] = entradas[i];
                    SaidaDentroFeriado[i] = feriadoParcialFimMin;
                }
                else if (!EstaContidoNoFeriadoParcial(entradas[i], feriadoParcialInicioMin, feriadoParcialFimMin) && EstaContidoNoFeriadoParcial(saidas[i], feriadoParcialInicioMin, feriadoParcialFimMin))
                {
                    EntradaDentroFeriado[i] = feriadoParcialInicioMin;
                    SaidaDentroFeriado[i] = saidas[i];
                }
                else if (entradas[i] < feriadoParcialInicioMin && saidas[i] > feriadoParcialFimMin)
                {
                    EntradaDentroFeriado[i] = feriadoParcialInicioMin;
                    SaidaDentroFeriado[i] = feriadoParcialFimMin;
                }
                else
                {
                    EntradaDentroFeriado[i] = -1;
                    SaidaDentroFeriado[i] = -1;
                }
            }
            int trabFeriadoDiurnada = 0, trabFeriadoNoturna = 0;
            BLL.CalculoHoras.QtdHorasDiurnaNoturna(EntradaDentroFeriado, SaidaDentroFeriado, inicioAdNoturno, fimAdNoturno, ref trabFeriadoDiurnada, ref trabFeriadoNoturna);
            horasTrabalhadasDentroFeriadoParcialDiurna = trabFeriadoDiurnada;
            horasTrabalhadasDentroFeriadoParcialNoturna = trabFeriadoNoturna;
        }

        private void TestaMarcacoesCorretas()
        {
            int aux = 0;
            int[] marcacoes = this.GetMarcacoesValidas();
            for (int i = 0; i < marcacoes.Length; i++)
            {
                if (marcacoes[i] != -1)
                {
                    aux++;
                }
            }
            if ((aux % 2) != 0)
            {
                if (!(legenda == "A"))
                {
                    ocorrencia = "Marcações Incorretas";
                }
            }
            else
            {
                if (ocorrencia == "Marcações Incorretas")
                {
                    ocorrencia = "";
                }
            }
        }

        private void SetaJornadaAlternativa()
        {
            if (idJornadaAlternativa != null)
            {
                objJornadaAlternativa = (Modelo.JornadaAlternativa)jornadaAlternativaList[idJornadaAlternativa];
            }
            else
            {
                objJornadaAlternativa = null;
            }
        }

        /// <summary>
        /// Verifica se existe jornada alternativa. 
        /// Caso exista, seta as variáveis de horario detalhe com os dados da jornada alternativa.
        /// </summary>
        /// <param name="HoraEntrada"></param>
        /// <param name="HoraSaida"></param>
        /// <param name="CargaHorariaD"></param>
        /// <param name="CargaHorariaN"></param>
        /// <param name="CargaHorariaM"></param>
        /// <param name="pMarcaCargaMista"></param>
        /// <returns>true = existe jornada; false = não existe jornada</returns>
        private bool VerificaJornadaAlternativa(ref int[] HoraEntrada, ref int[] HoraSaida, ref int CargaHorariaD, ref int CargaHorariaN, ref int CargaHorariaM, ref bool bCafe, ref int pMarcaCargaMista)
        {
            if (objJornadaAlternativa != null)
            {
                //Jornada Alternativa não tem café
                bCafe = false;

                HoraEntrada[0] = objJornadaAlternativa.EntradaMin_1;
                HoraEntrada[1] = objJornadaAlternativa.EntradaMin_2;
                HoraEntrada[2] = objJornadaAlternativa.EntradaMin_3;
                HoraEntrada[3] = objJornadaAlternativa.EntradaMin_4;

                HoraSaida[0] = objJornadaAlternativa.SaidaMin_1;
                HoraSaida[1] = objJornadaAlternativa.SaidaMin_2;
                HoraSaida[2] = objJornadaAlternativa.SaidaMin_3;
                HoraSaida[3] = objJornadaAlternativa.SaidaMin_4;

                CargaHorariaD = Modelo.cwkFuncoes.ConvertHorasMinuto(objJornadaAlternativa.TotalTrabalhadaDiurna);
                CargaHorariaN = Modelo.cwkFuncoes.ConvertHorasMinuto(objJornadaAlternativa.TotalTrabalhadaNoturna);
                CargaHorariaM = Modelo.cwkFuncoes.ConvertHorasMinuto(objJornadaAlternativa.TotalMista);

                pMarcaCargaMista = objJornadaAlternativa.CargaMista;
                CalculaHorasTrabalhadasNoFeriado(HoraEntrada, HoraSaida, inicioAdNoturno, fimAdNoturno, feriadoParcialInicioMin, feriadoParcialFimMin, out horasPrevistasDentroFeriadoParcialDiurna, out horasPrevistasDentroFeriadoParcialNoturna);
                if (objJornadaAlternativa.CargaMista == 1)
                {
                    BLL.CalculoHoras.QtdHorasDiurnaNoturna(HoraEntrada, HoraSaida, inicioAdNoturno, fimAdNoturno, ref CargaHorariaD, ref CargaHorariaN);
                }

                return true;
            }

            return false;
        }

        private void SetaDsr()
        {
            //Marca se a marcação é DSR
            if (tipoHorario == 1) //Normal
            {
                if (diaInt == diaSemanaDsr)
                {
                    dsr = 1;
                }
                else
                {
                    dsr = 0;
                }
            }
            else //Flexível
            {
                if (diaDsr != null)
                {
                    dsr = diaDsr.Value;
                }
                else
                {
                    dsr = 0;
                }
            }
        }

        ///No caso de ocorrencia no mesmo dia de demissão seta ocorrencia como "Funcionário Demitido"
        private void setaOcorrenciaDemitido()
        {
            if (dataDemissao == data)
            {
                ocorrencia = "Funcionário Demitido";
            }
        }
        /// <summary>
        /// Preenche funcionario não admitido, separado para organização do código
        /// </summary>
        private void PreencheFuncionarioDemitido()
        {
            ZeraValores();
            ocorrencia = "Funcionário Demitido";
        }

        private void PreencheFuncionarioInativo()
        {
            ZeraValores();
            ocorrencia = "Funcionário Inativo";
        }

        private void ZeraValores()
        {
            horasTrabalhadasMin = 0;
            horasTrabalhadasNoturnasMin = 0;
            horasExtrasDiurnaMin = 0;
            horasExtraNoturnaMin = 0;
            horasFaltasMin = 0;
            horasFaltaNoturnaMin = 0;
            bancoHorasCre = "---:--";
            bancoHorasDeb = "---:--";
            AdicionalNoturno = 0;
        }

        /// <summary>
        /// Preenche o funcionario não admitido, separado para organização do codigo
        /// </summary>
        private void PreencheFuncionarioNaoAdmitido()
        {
            horasTrabalhadasMin = 0;
            horasTrabalhadasNoturnasMin = 0;
            horasExtrasDiurnaMin = 0;
            horasExtraNoturnaMin = 0;
            horasFaltasMin = 0;
            horasFaltaNoturnaMin = 0;
            bancoHorasCre = "---:--";
            bancoHorasDeb = "---:--";
            ocorrencia = "Não Admitido";
            legenda = Convert.ToString("");
            AdicionalNoturno = 0;
            dia = Modelo.cwkFuncoes.DiaSemana(data, Modelo.cwkFuncoes.TipoDiaSemana.Reduzido);
        }

        private string BuscaLegenda()
        {
            if (idFeriado != null && naoConsiderarFeriado == 0)
            {
                //Tem um feriado naquela data               
                return "F";
            }

            if (TemAfastamento())
            {
                //Existe um afastamento para aquela data                
                return "A";
            }
            return "";
        }

        private bool TemAfastamento()
        {
            return idAfastamentoFunc != null || idAfastamentoDep != null || idAfastamentoEmp != null || idAfastamentoCont != null;
        }

        private string BuscaLegendaConcatenada()
        {
            if ((idFeriado != null && naoConsiderarFeriado == 0) && (idAfastamentoFunc != null || idAfastamentoDep != null || idAfastamentoEmp != null || idAfastamentoCont != null))
            {
                return "F,A";
            }

            if (idFeriado != null && naoConsiderarFeriado == 0)
            {
                //Tem um feriado naquela data               
                return "F";
            }

            if (idAfastamentoFunc != null || idAfastamentoDep != null || idAfastamentoEmp != null || idAfastamentoCont != null)
            {
                //Existe um afastamento para aquela data
                return "A";
            }
            return "";
        }

        #endregion

        public void CalculaMarcacoesWebApi(string login)
        {
            try
            {
                AuxCalculaMarcacoesWebApi(login);
                this.CalculaDSR(true, true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }

    #region Classes Auxiliares
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt)
        {
            int diff = dt.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
            {
                diff += 7;
            }
            DateTime data = dt.AddDays(-1 * diff).Date;
            return data;
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }
    }

    public class ParmsBancoHorasAcumulativo
    {
        public bool Ativo { get; set; }
        public decimal Percentual { get; set; }
        public int LimiteMinutos { get; set; }

        public ParmsBancoHorasAcumulativo()
        {
            Ativo = false;
            Percentual = 0;
            LimiteMinutos = 0;
        }
    }

    public static class LINQExtensions
    {
        public static IEnumerable<T> InclusiveTakeWhile<T>(this IEnumerable<T> data, Func<T, bool> predicate)
        {
            foreach (var item in data)
            {
                yield return item;
                if (!predicate(item))
                    break;
            }
            yield break;
        }
    }
    #endregion
}
