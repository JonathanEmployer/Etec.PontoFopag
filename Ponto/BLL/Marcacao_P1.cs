using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DAL.SQL;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public partial class Marcacao : IBLL<Modelo.Marcacao>
    {
        private DAL.IMarcacao dalMarcacao;
        private DAL.IImportaBilhetes dalImportaBilhetes;
        private DAL.IHorario dalHorario;
        
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }


        public Marcacao() : this(null)
        {

        }

        public Marcacao(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }
        public Marcacao(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))            
                ConnectionString = connString;            
            else            
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            DataBase db = new DataBase(ConnectionString);
            dalMarcacao = new DAL.SQL.Marcacao(db);
            dalImportaBilhetes = new DAL.SQL.ImportaBilhetes(db);
            dalHorario = new DAL.SQL.Horario(db);

            dalMarcacao.UsuarioLogado = usuarioLogado;
            dalHorario.UsuarioLogado = usuarioLogado;
            dalImportaBilhetes.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        #region Métodos Básicos

        public int MaxCodigo()
        {
            return dalMarcacao.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalMarcacao.GetAll();
        }

        public Modelo.Marcacao LoadObject(int id)
        {
            return dalMarcacao.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Marcacao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Afastamento.Abonado == 1 && objeto.Afastamento.IdOcorrencia == 0)
            {
                ret.Add("cbIdOcorrencia", "Para abonar uma marcação é necessário selecionar uma ocorrência.");
            }

            if (objeto.BilhetesMarcacao != null && objeto.BilhetesMarcacao.Count > 0)
            {
                //Valida se alguma das batidas estão fora do padrão hh:mm e se estão passando das 23:59
                string bilhetesErro = String.Join(" - ", objeto.BilhetesMarcacao.Where(s => (s.Mar_hora.Length > 5 || Modelo.cwkFuncoes.ConvertBatidaMinuto(s.Mar_hora) > 1439 || !Modelo.cwkFuncoes.HoraValida(s.Mar_hora)) && s.Acao != Modelo.Acao.Excluir).Select(s => s.Mar_hora));
                if (!String.IsNullOrEmpty(bilhetesErro))
                {
                    throw new Exception(String.Format("Os registros ({0}) estão fora do padrão aceitável (HH:mm e menor ou igual a 23:59), Verifique! ", bilhetesErro));
                }
            }
            return ret;
        }

        public Dictionary<string, string> ValidaObjeto(List<Modelo.Marcacao> listaObjeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (Modelo.Marcacao objeto in listaObjeto)
            {
                if (objeto.Afastamento.Abonado == 1 && objeto.Afastamento.IdOcorrencia == 0)
                {
                    ret.Add("cbIdOcorrencia", "Para abonar uma marcação é necessário selecionar uma ocorrência.");
                }
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Marcacao objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalMarcacao.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalMarcacao.Alterar(objeto);
                        if (objeto.Idfechamentobh > 0 || objeto.IdFechamentoPonto > 0)
                        {
                            if (objeto.Idfechamentobh > 0)
                            {
                                BLL.FechamentoBH bllBH = new BLL.FechamentoBH(ConnectionString, UsuarioLogado);
                                Modelo.FechamentoBH fechamentoBH = new Modelo.FechamentoBH();
                                fechamentoBH = bllBH.LoadObject(objeto.Idfechamentobh);
                                throw new Exception ("Marcação não pode ser alterada, já existe um fechamento de banco de horas no dia " + fechamentoBH.Data.GetValueOrDefault().ToShortDateString());
                            }
                            else
                            {
                                BLL.FechamentoPonto bllFP = new BLL.FechamentoPonto(ConnectionString, UsuarioLogado);
                                Modelo.FechamentoPonto fechamentoPonto = new Modelo.FechamentoPonto();
                                fechamentoPonto = bllFP.LoadObject(objeto.IdFechamentoPonto);
                                throw new Exception ("Marcação não pode ser alterada, já existe um fechamento de ponto no dia " + fechamentoPonto.DataFechamento.ToShortDateString());
                            }
                        }

                        DateTime dataInicial = objeto.Data.AddDays(-1);
                        DateTime dataFinal = objeto.Data.AddDays(1);

                        BLL.BancoHoras bllBancoHoras = new BLL.BancoHoras(ConnectionString, UsuarioLogado);
                        Modelo.BancoHoras bh = new Modelo.BancoHoras();

                        DateTime dataUltimoBH = objeto.Data.AddDays(-1);
                        bool bancoHorasPorSaldo = false;
                        do
                        {
                           bh = bllBancoHoras.BancoHorasPorFuncionario(dataUltimoBH.AddDays(+1), objeto.Idfuncionario);
                           if (bh != null && bh.Id > 0)
                           {
                               dataUltimoBH = bh.DataFinal.GetValueOrDefault();
                           }
                           // Se o funcionario tiver parametrizado que usa limite de BH por saldo, recalcula a marcacao até o ultimo dia do banco ou até o ultimo dia corrente.
                           if ((bh != null && bh.Id > 0) && 
                               ((bh.SaldoBh_1 != null && Regex.IsMatch(bh.SaldoBh_1, @"\d")) || (bh.SaldoBh_2 != null && Regex.IsMatch(bh.SaldoBh_2, @"\d")) || (bh.SaldoBh_3 != null && Regex.IsMatch(bh.SaldoBh_3, @"\d")) || (bh.SaldoBh_4 != null && Regex.IsMatch(bh.SaldoBh_4, @"\d")) ||
                                (bh.SaldoBh_5 != null && Regex.IsMatch(bh.SaldoBh_5, @"\d")) || (bh.SaldoBh_6 != null && Regex.IsMatch(bh.SaldoBh_6, @"\d")) || (bh.SaldoBh_7 != null && Regex.IsMatch(bh.SaldoBh_7, @"\d")) || (bh.SaldoBh_8 != null && Regex.IsMatch(bh.SaldoBh_8, @"\d")) || (bh.SaldoBh_9 != null && Regex.IsMatch(bh.SaldoBh_9, @"\d"))))
                           {
                               bancoHorasPorSaldo = true;
                           }

                        } while (bh != null && bh.Id > 0);

                        if (bancoHorasPorSaldo)
                        {
                            if (dataUltimoBH >= DateTime.Now)
                                dataFinal = DateTime.Now;
                            else
                                dataFinal = dataUltimoBH;
                        }
                        

                        //Calcula a marcação
                        BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(2, objeto.Idfuncionario, dataInicial, dataFinal, ObjProgressBar, false, ConnectionString, UsuarioLogado, false);
                        bllCalculaMarcacao.CalculaMarcacoes();
                        break;
                    case Modelo.Acao.Excluir:
                        dalMarcacao.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, List<Modelo.Marcacao> listaObjeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(listaObjeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalMarcacao.IncluirMarcacoesEmLote(listaObjeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalMarcacao.Alterar(listaObjeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalMarcacao.Excluir(listaObjeto);
                        break;
                }
            }
            return erros;
        }

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalMarcacao.getId(pValor, pCampo, pValor2);
        }

        public void AtualizaMudancaHorarioMarcacao(List<int> idsFuncionarios, DateTime dataInicio)
        {
            dalMarcacao.AtualizaMudancaHorarioMarcacao(idsFuncionarios, dataInicio);
        }

        #endregion

        #region Listagens

        public List<Modelo.Marcacao> GetAllList()
        {
            return dalMarcacao.GetAllList();
        }

        public List<Modelo.Marcacao> GetTratamentosMarcacao(DateTime datainicial, DateTime datafinal)
        {
            return dalMarcacao.GetTratamentosMarcacao(datainicial, datafinal);
        }

        public List<Modelo.Marcacao> GetPorFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            return dalMarcacao.GetPorFuncionario(pIdFuncionario, pdataInicial, pDataFinal, PegaInativos);
        }

        public List<Modelo.Marcacao> GetPorFuncionarios(List<int> pIdsFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            return dalMarcacao.GetPorFuncionarios(pIdsFuncionario, pdataInicial, pDataFinal, PegaInativos);
        }

        public List<Modelo.Marcacao> GetPorEmpresa(int pEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            return dalMarcacao.GetPorEmpresa(pEmpresa, pdataInicial, pDataFinal, false);
        }

        public List<Modelo.Marcacao> GetPorDepartamento(int pDepartamento, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            return dalMarcacao.GetPorDepartamento(pDepartamento, pdataInicial, pDataFinal, PegaInativos);
        }

        public List<Modelo.Marcacao> GetPorFuncao(int pFuncao, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            return dalMarcacao.GetPorFuncao(pFuncao, pdataInicial, pDataFinal, PegaInativos);
        }

        public List<Modelo.Marcacao> GetPorPeriodo(DateTime pdataInicial, DateTime pDataFinal)
        {
            return dalMarcacao.GetPorPeriodo(pdataInicial, pDataFinal);
        }

        public List<Modelo.Marcacao> GetPorHorario(int pIdHorario, DateTime pdataInicial, DateTime pDataFinal)
        {
            return dalMarcacao.GetPorHorario(pIdHorario, pdataInicial, pDataFinal);
        }

        public DataTable GetRelatorioObras(string idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal, string codsLocalReps)
        {
            return dalMarcacao.GetRelatorioObras(idsFuncionarios, pdataInicial, pDataFinal, codsLocalReps);
        }
        public DataTable GetRelatorioSubstituicaoJornada(string idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal)
        {         
            return dalMarcacao.GetRelatorioSubstituicaoJornada(idsFuncionarios, pdataInicial, pDataFinal);
        }
        public DataTable GetRelatorioRegistros(string idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal)
        {
            DataTable dadosRel = new DataTable();
            //if (Versao == 3)
            //{
            //    Versao = 2;
            List<int> idsFuncs = idsFuncionarios.Split(',').Select(s => Convert.ToInt32(s)).ToList();
            int partes = idsFuncs.Count();
            if (partes >= 3)
            {
                partes = idsFuncs.Count() / 3;
            }
            IList<List<int>> idsParciais = BLL.cwkFuncoes.SplitList(idsFuncs, partes);

            Parallel.ForEach(idsParciais,
            ids =>
            {
                DataTable dt = dalMarcacao.GetRelatorioRegistros(String.Join(",", ids), pdataInicial, pDataFinal);
                lock (dadosRel)
                {
                    dadosRel.Merge(dt);
                }

            });
            //}
            //else
            //{
                //DataTable dt = dalMarcacao.GetRelatorioRegistros(idsFuncionarios, pdataInicial, pDataFinal);
                //dadosRel.Merge(dt);
            //}
            

            DataView dtV = dadosRel.DefaultView;
            dtV.Sort = "nome,dataSemFormat,Matrícula";
            return dadosRel;
        }

        public List<Modelo.MarcacaoLista> GetPorDataManutDiaria(DateTime pDataIni, DateTime pDataFin, bool PegaInativos, bool inserirMarcacoes)
        {
            if (inserirMarcacoes)
            {
                this.AtualizaManutDiaria(5, 0, pDataIni, pDataFin, objProgressBar);
                RecalculaMarcacao(null, 0, pDataIni, pDataFin, objProgressBar);
            }
            return dalMarcacao.GetPorDataManutDiaria(pDataIni, pDataFin, PegaInativos);
        }

        public List<Modelo.MarcacaoLista> GetPorManutDiariaCont(int pIdContrato, DateTime pDataIni, DateTime pDataFin, bool PegaInativos, bool inserirMarcacoes)
        {
            if (inserirMarcacoes)
            {
                this.AtualizaManutDiaria(6, pIdContrato, pDataIni, pDataFin, objProgressBar);
                RecalculaMarcacao(6, pIdContrato, pDataIni, pDataFin, objProgressBar);
            }
            return dalMarcacao.GetPorManutDiariaCont(pIdContrato, pDataIni, pDataFin, false);
        }

        public List<Modelo.MarcacaoLista> GetPorManutDiariaDep(int pIdDepartamento, DateTime pDataIni, DateTime pDataFin, bool PegaInativos, bool inserirMarcacoes)
        {
            if (inserirMarcacoes)
            {
                this.AtualizaManutDiaria(1, pIdDepartamento, pDataIni, pDataFin, objProgressBar);
                RecalculaMarcacao(1, pIdDepartamento, pDataIni, pDataFin, objProgressBar);
            }
            return dalMarcacao.GetPorManutDiariaDep(pIdDepartamento, pDataIni, pDataFin, false);
        }

        public List<Modelo.MarcacaoLista> GetPorManutDiariaEmp(int pEmpresa, DateTime pDataIni, DateTime pDataFin, bool PegaInativos, bool inserirMarcacoes)
        {
            if (inserirMarcacoes)
            {
                this.AtualizaManutDiaria(0, pEmpresa, pDataIni, pDataFin, objProgressBar);
                RecalculaMarcacao(0, pEmpresa, pDataIni, pDataFin, objProgressBar);
            }
            return dalMarcacao.GetPorManutDiariaEmp(pEmpresa, pDataIni, pDataFin, false);
        }

        public List<Modelo.MarcacaoLista> GetMarcacaoListaPorFuncionario(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal)
        {
            BLL.Funcionario bllFuncionario = new Funcionario(ConnectionString, UsuarioLogado);
            BLL.InclusaoBanco bllInclusaoBanco = new InclusaoBanco(ConnectionString, UsuarioLogado);
            //Adiciona mais um dia, pois para comparar com a quantidade de dias, tem que contar o dia inicial e o final como dia completo e o TotalDays não considera o ultimo di a como dia, (ele é a diferença) 
            TimeSpan ts = pDataFinal.AddDays(1) - pdataInicial;

            int qtd = dalMarcacao.QuantidadeMarcacoes(pIdFuncionario, pdataInicial, pDataFinal);

            if (ts.TotalDays > qtd)
            {
                Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                objFuncionario = bllFuncionario.LoadObject(pIdFuncionario);
                this.AtualizaData(pdataInicial, pDataFinal, objFuncionario);
                CalculoMarcacoes.PontoPorExcecao bllPontoPorExcecao = new CalculoMarcacoes.PontoPorExcecao(ConnectionString, UsuarioLogado);
                bllPontoPorExcecao.CriarRegistroPontoPorExcecao(new List<int>() { pIdFuncionario}, new List<int>());
                System.Threading.Thread.Sleep(5000);
            }

            return dalMarcacao.GetMarcacaoListaPorFuncionario(pIdFuncionario, pdataInicial, pDataFinal);
        }

        public int QuantidadeMarcacoes(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal)
        {
            return dalMarcacao.QuantidadeMarcacoes(pIdFuncionario, pdataInicial, pDataFinal);
        }

		public Dictionary<int, int> QuantidadeMarcacoesPorLista(List<int> pIdsFuncionario, DateTime pdataInicial, DateTime pDataFinal)
        {
            return dalMarcacao.QuantidadeMarcacoes(pIdsFuncionario, pdataInicial, pDataFinal);
        }
		
        public List<Modelo.MarcacaoLista> GetPorEmpresaList(int pEmpresa, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            return dalMarcacao.GetPorEmpresaList(pEmpresa, pdataInicial, pDataFinal, false);
        }

        /// <summary>
        /// Pega a lista de marcações de um determinado tipo
        /// </summary>
        /// <param name="tipoLista"> Tipo da compensação: 0 - Empresa, 1 - Departamento, 2 - Funcionario, 3 - Função, 4 - Horário, 5 - Todos </param>
        /// <param name="IdDoTipo">Id do tipo escolhido</param>
        /// <param name="dataInicial">Data inicial para o periodo que se deseja pegar as marcações</param>
        /// <param name="dataFinal">Data final para o periodo que se deseja pegar as marcações</param>
        /// <returns> Lista de marcações de um tipo determinado para aquele periodo </returns>
        ///PAM - 15/12/2009
        public List<Modelo.Marcacao> getListaMarcacao(int pTipoLista, int pIdDoTipo, DateTime pDataInicial, DateTime pDataFinal)
        {
            switch (pTipoLista)
            {
                case 0: return GetPorEmpresa(pIdDoTipo, pDataInicial, pDataFinal, false);
                case 1: return GetPorDepartamento(pIdDoTipo, pDataInicial, pDataFinal, false);
                case 2: return GetPorFuncionario(pIdDoTipo, pDataInicial, pDataFinal, false);
                case 3: return GetPorFuncao(pIdDoTipo, pDataInicial, pDataFinal, false);
                case 4: return GetPorHorario(pIdDoTipo, pDataInicial, pDataFinal);
                case 5: return GetPorPeriodo(pDataInicial, pDataFinal);
                default: return null;
            }
        }

        public DataTable GetParaRelatorioOcorrencia(int pTipo, string pIdentificacao, DateTime pDataI, DateTime pDataF, int pModoOrdenacao, int pAgrupaDepartamento)
        {
            return dalMarcacao.GetParaRelatorioOcorrencia(pTipo, pIdentificacao, pDataI, pDataF, pModoOrdenacao, pAgrupaDepartamento);
        }

        public DataTable GetParaTotalizaHoras(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            return dalMarcacao.GetParaTotalizaHoras(pIdFuncionario, pdataInicial, pDataFinal, PegaInativos);
        }

        public DataTable GetParaTotalizaHorasFuncs(List<int> pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            return dalMarcacao.GetParaTotalizaHorasFuncs(pIdFuncionario, pdataInicial, pDataFinal, PegaInativos);
        }

        public DataTable GetParaACJEF(int pIdEmpresa, DateTime pDataI, DateTime pDataF, bool pPegaInativos)
        {
            return dalMarcacao.GetParaACJEF(pIdEmpresa, pDataI, pDataF, pPegaInativos);
        }

        /// <summary>
        /// Esse método verifica se tem alguma marcação para aquele funcionario naquele dia e se não tiver, insere.
        /// Após inserir todas as marcações, recalcula todas as que foram inseridas.
        /// </summary>
        /// <param name="pTipo">Tipo: 0: Empresa, 1: Departamento, 2: Funcionario, 3: Função, 4:Horario, 5: Todos, 6: Contrato</param>
        /// <param name="pIdTipo">Id do tipo</param>
        /// <param name="pDataInicial">Data Inicial</param>
        /// <param name="pDataFinal">Data Final</param>
        /// <param name="pObjProgressBar">ProgressBar</param>
        //PAM - 05/04/2010
        public void InsereMarcacoesNaoExistentes(int pTipo, int pIdTipo, DateTime pDataInicial, DateTime pDataFinal, Modelo.ProgressBar pObjProgressBar, bool isTabManutDiaria)
        {
            VerificaProgressNula(ref pObjProgressBar);
            BLL.Funcionario bllFuncionario = new Funcionario(ConnectionString, UsuarioLogado);
            try
            {
                string strMarcInseridas = null;
                DateTime data = pDataInicial;
                string key = "";

                List<Modelo.Funcionario> lstFuncionarios = null;
                List<Modelo.Marcacao> lstMarcacao = new List<Modelo.Marcacao>();

                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();

                TimeSpan ts = pDataFinal - pDataInicial;

                lstFuncionarios = bllFuncionario.GetFuncionarios(pTipo, pIdTipo);
                Hashtable ht = this.GetMarcDiaFunc(pTipo, pIdTipo, pDataInicial, pDataFinal);

                pObjProgressBar.setaMinMaxPB(0, (ts.Days + 1) * lstFuncionarios.Count);

                foreach (Modelo.Funcionario func in lstFuncionarios)
                {
                    
                    key = "";
                    pObjProgressBar.setaMensagem("Funcionário: " + func.Nome);

                    data = pDataInicial;

                    while (data <= pDataFinal)
                    {
                        key = func.Id.ToString() + data.Date.ToString();
                        //Caso o funcionário não possuir registro o sistema irá incluir
                        if (!ht.ContainsKey(key))
                        {
                            objMarcacao = this.InsereMarcacaoLimpa(func.Id, func.Dscodigo, func.Idhorario, data);
                            this.MontaMarcFunc(func.Id, data, ref strMarcInseridas);
                            lstMarcacao.Add(objMarcacao);
                        }
                        data = data.AddDays(1);
                        pObjProgressBar.incrementaPB(1);
                    }
                }


                if (lstMarcacao.Count > 0)
                {
                    pObjProgressBar.setaMensagem("Salvando marcações...");
                    this.Salvar(Modelo.Acao.Incluir, lstMarcacao);
                    BLL.CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(pTipo, pIdTipo, pDataInicial, pDataFinal, pObjProgressBar, false, ConnectionString, UsuarioLogado, false);
                    if (isTabManutDiaria)
                        bllCalculaMarcacao.AuxCalculaMarcacoes();
                    else
                        bllCalculaMarcacao.CalculaMarcacoes();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Hashtable GetMarcDiaFunc(int pTipo, int pIdTipo, DateTime pDataInicial, DateTime pDataFinal)
        {
            return dalMarcacao.GetMarcDiaFunc(pTipo, pIdTipo, pDataInicial, pDataFinal);
        }

        private void VerificaProgressNula(ref Modelo.ProgressBar pObjProgressBar)
        {
            if ((pObjProgressBar.incrementaPB == null) &&
                (pObjProgressBar.setaMensagem == null) &&
                (pObjProgressBar.setaMinMaxPB == null) &&
                (pObjProgressBar.setaValorPB == null))
            {
                pObjProgressBar.incrementaPB = this.IncrementaProgressBar;
                pObjProgressBar.setaMensagem = this.SetaMensagem;
                pObjProgressBar.setaMinMaxPB = this.SetaMinMaxProgressBar;
                pObjProgressBar.setaValorPB = this.SetaValorProgressBar;
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

        public DataTable GetParaRelatorioAbstinencia(int pIdFuncionario, DateTime pdataInicial, DateTime pDataFinal)
        {
            return dalMarcacao.GetParaRelatorioAbstinencia(pIdFuncionario, pdataInicial, pDataFinal);
        }

        public List<Modelo.Marcacao> GetCartaoPontoV2(List<int> pIdFuncionarios, DateTime pdataInicial, DateTime pDataFinal)
        {
            return dalMarcacao.GetCartaoPontoV2(pIdFuncionarios, pdataInicial, pDataFinal);
        }

        public List<Modelo.Proxy.Relatorios.PxyRelConferenciaHoras> GetRelatorioConferenciaHoras(List<string>cpfsFuncionarios, DateTime dataInicial, DateTime DataFinal)
        {
            return dalMarcacao.GetRelatorioConferenciaHoras(cpfsFuncionarios, dataInicial, DataFinal);
        }

        public int retornaIdMarcacao(int idFuncionario, DateTime data)
        {
            string strCmd = @" select @id = id 
                                from marcacao_view 
                                where idfuncionario = @idfuncionario and data = @data";

            SqlConnection sqlCon = new SqlConnection(ConnectionString);
            SqlCommand sqlCom = new SqlCommand(strCmd, sqlCon);

            sqlCom.Parameters.Add(new SqlParameter("@idfuncionario", SqlDbType.Int)).Value = idFuncionario;
            sqlCom.Parameters.Add(new SqlParameter("@data", SqlDbType.Date)).Value = data.ToString("yyyy/MM/dd");

            SqlParameter sqlpar = new SqlParameter("@id", SqlDbType.Int);
            sqlpar.Direction = ParameterDirection.Output;

            sqlCom.Parameters.Add(sqlpar);
            sqlCom.CommandType = CommandType.Text;

            int ret = 0;

            try
            {
                sqlCon.Open();
                sqlCom.ExecuteNonQuery();

                if (sqlCom.Parameters[2].Value != null)
                {
                    ret = Convert.ToInt32(sqlCom.Parameters[2].Value);
                }

                return ret;

            }finally
            {
                sqlCon.Close();
            }
        }

        #endregion
    }

    public class auxOrdenaMarcacao
    {
        public string numRelogio { get; set; }
        public string horario { get; set; }
        public string ent_sai { get; set; }
        public int posicao { get; set; }
        public DateTime? data { get; set; }

        public auxOrdenaMarcacao(string horario, string ent_sai, int posicao, string numRelogio, DateTime? data)
        {
            this.numRelogio = numRelogio;
            this.horario = horario;
            this.ent_sai = ent_sai;
            this.posicao = posicao;
            this.data = data;
        }
    }
}
