using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DAL.SQL;
using Modelo.Proxy;

namespace BLL
{
    public class BancoHoras : IBLL<Modelo.BancoHoras>
    {
        private DAL.IBancoHoras dalBancoHoras;
        private DAL.IFuncionario dalFuncionario;
        private DAL.IEmpresa dalEmpresa;
        private DAL.IDepartamento dalDepartamento;
        private DAL.IFechamentoBH dalFechamentoBH;
        private DAL.IFechamentoBHD dalFechamentoBHD;
        private DAL.IJornadaAlternativa dalJornadaAlternativa;

        private Modelo.ProgressBar objProgressBar;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        #region Métodos Básicos

        #region Construtores
        public BancoHoras() : this(null)
        {
           
        }

        public BancoHoras(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public BancoHoras(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;        
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            DataBase db = new DataBase(ConnectionString);
            dalBancoHoras = new DAL.SQL.BancoHoras(db);
            dalEmpresa = new DAL.SQL.Empresa(db);
            dalDepartamento = new DAL.SQL.Departamento(db);
            dalFechamentoBH = new DAL.SQL.FechamentoBH(db);
            dalFechamentoBHD = new DAL.SQL.FechamentoBHD(db);
            dalJornadaAlternativa = new DAL.SQL.JornadaAlternativa(db);
            dalFuncionario = new DAL.SQL.Funcionario(db);


            dalBancoHoras.UsuarioLogado = usuarioLogado;
            dalEmpresa.UsuarioLogado = usuarioLogado;
            dalDepartamento.UsuarioLogado = usuarioLogado;
            dalFechamentoBH.UsuarioLogado = usuarioLogado;
            dalFechamentoBHD.UsuarioLogado = usuarioLogado;
            dalJornadaAlternativa.UsuarioLogado = usuarioLogado;
            dalFuncionario.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        #endregion

        public int MaxCodigo()
        {
            return dalBancoHoras.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalBancoHoras.GetAll();
        }

        public Modelo.BancoHoras LoadObject(int id)
        {
            return dalBancoHoras.LoadObject(id);
        }

        public Modelo.BancoHoras LoadObjectByCodigo(int codigo)
        {
            return dalBancoHoras.LoadPorCodigo(codigo);
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
            return dalBancoHoras.getId(pValor, pCampo, pValor2);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.BancoHoras objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.DataInicial != null && objeto.DataFinal != null)
            {
                if (objeto.DataInicial > objeto.DataFinal)
                {
                    ret.Add("txtDataFinal", "A data final deve ser maior ou igual à data inicial.");
                }
            }
            else
            {
                if (objeto.DataInicial == null)
                {
                    ret.Add("txtDataInicial", "Campo obrigatório.");
                }

                if (objeto.DataFinal == null)
                {
                    ret.Add("txtDataFinal", "Campo obrigatório.");
                }
            }

            if (objeto.Tipo == -1)
            {
                ret.Add("rgTipo", "Campo obrigatório.");
            }

            if (objeto.Identificacao == 0)
            {
                ret.Add("cbIdentificacao", "Campo obrigatório.");
            }
            else if (objeto.DataInicial != null && objeto.DataFinal != null)
            {
                if (VerificaExiste(objeto.Id, objeto.DataInicial.Value, objeto.DataFinal.Value, objeto.Tipo, objeto.Identificacao))
                {
                    ret.Add("cbIdentificacao", "Já existe um registro gravado dentro deste perído.");
                }
            }

            BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new FechamentoPontoFuncionario(ConnectionString, UsuarioLogado);
            string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(objeto.Tipo, new List<int>() { objeto.Identificacao }, objeto.DataInicial.GetValueOrDefault());
            if (!String.IsNullOrEmpty(mensagemFechamento))
            {
                ret.Add("Fechamento Ponto", mensagemFechamento);
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.BancoHoras objeto)
        {
            return Salvar(pAcao, objeto, false);
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.BancoHoras objeto, bool naoValidaFechamento)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (naoValidaFechamento)
            {
                erros.Remove("Fechamento Ponto");
            }
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalBancoHoras.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalBancoHoras.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalBancoHoras.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        public List<Modelo.BancoHoras> GetAllList(bool verificaPermissao)
        {
            return dalBancoHoras.GetAllList(verificaPermissao);
        }

        public Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF)
        {
            return dalBancoHoras.GetHashIdObjeto(pDataI, pDataF);
        }

        public Hashtable GetHashIdObjeto(DateTime? pDataI, DateTime? pDataF, List<int> ids)
        {
            return dalBancoHoras.GetHashIdObjeto(pDataI, pDataF, ids);
        }

        #endregion

        #region Métodos de Verificação de Existencia

        /// <summary>
        /// Caso o registro exista retorna true, caso contrário false
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="tipo"></param>
        /// <param name="identificacao"></param>
        /// <returns></returns>
        public bool VerificaExiste(int pId, DateTime? dataInicial, DateTime? dataFinal, int tipo, int identificacao)
        {
            return (dalBancoHoras.VerificaExiste(pId, dataInicial, dataFinal, tipo, identificacao) > 0);
        }

        /// <summary>
        /// Rotina que verifica se um registro esta em uma determinada lista
        /// </summary>
        /// <param name="pData">Data</param>
        /// <param name="pEmpresa">Id da Empresa</param>
        /// <param name="pDepartamento">Id do Departamento</param>
        /// <param name="pFuncionario">Id do funcionário</param>
        /// <param name="pFuncao">Id da Função</param>
        /// <param name="pBancoHorasList">Lista de elementos para procurar o elemento especifico</param>
        /// <returns>Objeto encontrado</returns>
        public Modelo.BancoHoras PossuiRegistro(DateTime pData, int pEmpresa, int pDepartamento, int pFuncionario, int pFuncao, List<Modelo.BancoHoras> pBancoHorasList)
        {
            if (pBancoHorasList.Count > 0)
            {
                var bancos = pBancoHorasList.Where(bh => (bh.DataInicial <= pData) && (bh.DataFinal >= pData) &&
                     ((bh.Tipo == 0 && bh.Identificacao == pEmpresa) || (bh.Tipo == 1 && bh.Identificacao == pDepartamento) ||
                      (bh.Tipo == 2 && bh.Identificacao == pFuncionario) || (bh.Tipo == 3 && bh.Identificacao == pFuncao)));

                if (bancos.Count() == 0)
                {
                    return null;
                }
                else if (bancos.Count() == 1)
                {
                    // Se existir um único elemento que satisfaça a condição 
                    return bancos.Single();
                }
                else
                {
                    //Caso exista mais de um registro retorna sempre o mais específico

                    //Funcionário
                    var banco = bancos.Where(bh => (bh.Tipo == 2 && bh.Identificacao == pFuncionario));
                    if (banco.Count() > 0)
                    {
                        return banco.Last();
                    }

                    //Função
                    banco = bancos.Where(bh => (bh.Tipo == 3 && bh.Identificacao == pFuncao));

                    if (banco.Count() > 0)
                    {
                        return banco.Last();
                    }

                    //Departamento
                    banco = bancos.Where(bh => (bh.Tipo == 1 && bh.Identificacao == pDepartamento));
                    if (banco.Count() > 0)
                    {
                        return banco.Last();
                    }

                    //Empresa
                    banco = bancos.Where(bh => (bh.Tipo == 0 && bh.Identificacao == pEmpresa));
                    if (banco.Count() > 0)
                    {
                        return banco.Last();
                    }
                }
            }
            return null;
        }

        #endregion

        #region Métodos de Relatorio

        public DataTable GetRelatorioResumo(DateTime pDataI, DateTime pDataF, int pTipo, string pEmpresas, string pDepartamentos, string pFuncionarios, bool pBuscarUltimoFechamento)
        {
            objProgressBar.setaMensagem("Carregando dados...");
            
            objProgressBar.setaMinMaxPB(0, 100);
            objProgressBar.setaValorPB(0);

            DataTable dt = dalBancoHoras.GetRelatorioResumo(pDataI, pDataF, pTipo, pEmpresas, pDepartamentos, pFuncionarios);

            objProgressBar.incrementaPB(30);

            DataTable ret = new DataTable();

            #region Adiciona as colunas no DataTable de retorno
            List<DataColumn> colunas = new List<DataColumn>();
            foreach (DataColumn c in dt.Columns)
            {
                ret.Columns.Add(c.ColumnName);
            }

            ret.Columns.AddRange
            (
                new DataColumn[]
                {
                    new DataColumn("dataultimofechamento"), 
                    new DataColumn("saldoultimofechamento"),
                    new DataColumn("saldoanteriorbh"),
                    new DataColumn("legendasaldoanteriorbh"),
                    new DataColumn("creditobh"),
                    new DataColumn("debitobh"),
                    new DataColumn("saldobh"),
                    new DataColumn("legendasaldobh"),
                    new DataColumn("totalcreditodepartamento"),
                    new DataColumn("totaldebitodepartamento"),
                    new DataColumn("totalsaldodepartamento"),
                    new DataColumn("legendatotalsaldodepartamento"),
                    new DataColumn("totalsaldoanteriordepartamento"),
                    new DataColumn("legendasaldoantdep"),
                    new DataColumn("totalcreditoempresa"),
                    new DataColumn("totaldebitoempresa"),
                    new DataColumn("totalsaldoempresa"),
                    new DataColumn("legendatotalsaldoempresa"),
                    new DataColumn("totalsaldoanteriorempresa"),
                    new DataColumn("legendasaldoantemp"),
                    new DataColumn("saldoAnteriorComSinal"),
                    new DataColumn("saldoFinalComSinal"),
                }
            );

            #endregion

            //Carrega as listagens que serão utilizadas
            List<Modelo.Empresa> empresaList = dalEmpresa.GetAllList();
            List<Modelo.Departamento> departamentoList = dalDepartamento.GetAllList();
            objProgressBar.incrementaPB(25);
            List<Modelo.BancoHoras> bancoHorasList = this.GetAllList(false);
            objProgressBar.incrementaPB(25);
            var ListIntIdFuncionarios = dt.AsEnumerable().Select(x => x.Field<int>("idfuncionario")).ToList<int>();
            List<Modelo.FechamentoBH> fechamentoBHList = dalFechamentoBH.GetAllListFuncs(ListIntIdFuncionarios, false);


            List<Modelo.FechamentoBHD> fechamentoBHDList = dalFechamentoBHD.GetAllList();
            List<Modelo.JornadaAlternativa> jornadasList = dalJornadaAlternativa.GetPeriodoFuncionarios(pDataI, pDataF, dt.AsEnumerable().Select(x => x.Field<Int32>("idfuncionario")).Distinct().ToList());

            objProgressBar.incrementaPB(20);

            Modelo.TotalHoras objTotalHoras = null;

            int idempresa, iddepartamento, idfuncionario, idfuncao;

            int totalcreditoempresa = 0, totaldebitoempresa = 0, totalSaldoAnteriorEmpCre = 0
                , totalSaldoAnteriorEmpDeb = 0, totalSaldoAtualEmpCre = 0, totalSaldoAtualEmpDeb = 0;

            int totalcreditodepartamento = 0, totaldebitodepartamento = 0, totalSaldoAnteriorDepCre = 0
                , totalSaldoAnteriorDepDeb = 0, totalSaldoAtualDepCre = 0, totalSaldoAtualDepDeb = 0;

            int idempresaant = 0, iddepartamentoant = 0;

            objProgressBar.setaMinMaxPB(0, dt.Rows.Count);
            objProgressBar.setaValorPB(0);
            objProgressBar.setaMensagem("Gerando relatório...");

            foreach (DataRow row in dt.Rows)
            {
                idempresa = Convert.ToInt32(row["idempresa"]);
                iddepartamento = Convert.ToInt32(row["iddepartamento"]);
                idfuncionario = Convert.ToInt32(row["idfuncionario"]);
                idfuncao = Convert.ToInt32(row["idfuncao"]);

                if (iddepartamentoant != iddepartamento)
                {
                    iddepartamentoant = iddepartamento;
                    totalcreditodepartamento = 0;
                    totaldebitodepartamento = 0;
                    totalSaldoAnteriorDepCre = 0;
                    totalSaldoAnteriorDepDeb = 0;
                    totalSaldoAtualDepCre = 0;
                    totalSaldoAtualDepDeb = 0;
                }

                if (idempresaant != idempresa)
                {
                    idempresaant = idempresa;
                    totalcreditoempresa = 0;
                    totaldebitoempresa = 0;
                    totalSaldoAnteriorEmpCre = 0;
                    totalSaldoAnteriorEmpDeb = 0;
                    totalSaldoAtualEmpCre = 0;
                    totalSaldoAtualEmpDeb = 0;
                }

                //Totaliza o banco de horas para o funcionário
                objTotalHoras = new Modelo.TotalHoras(pDataI, pDataF);
                var totalizadorHoras = new BLL.TotalizadorHorasFuncionario(idempresa, iddepartamento, idfuncionario, idfuncao, pDataI, pDataF, jornadasList, null, null, null, ConnectionString, UsuarioLogado);
                totalizadorHoras.TotalizeHoras(objTotalHoras);
                DataTable listMarcacoes = totalizadorHoras.Marcacoes;
                var totalizadorBancoHoras = new BLL.CalculoMarcacoes.TotalizadorBancoHoras(idempresa, iddepartamento, idfuncionario, idfuncao, pDataI, pDataF, bancoHorasList, fechamentoBHList, fechamentoBHDList, listMarcacoes, pBuscarUltimoFechamento, ConnectionString, UsuarioLogado);
                totalizadorBancoHoras.PreenchaBancoHoras(objTotalHoras);

                //Busca fechamento
                string dataUltimoFechamento = "", saldoUltimoFechamento = "";
                if (pBuscarUltimoFechamento)
                {
                    if (fechamentoBHDList.Exists(f => f.Identificacao == idfuncionario))
                    {
                        Modelo.FechamentoBHD objFechamentoBHD = fechamentoBHDList.Where(f => f.Identificacao == idfuncionario).OrderBy(f => f.Id).Last();
                        Modelo.FechamentoBH objFechamentoBH = fechamentoBHList.Where(f => f.Id == objFechamentoBHD.Idfechamentobh).First();

                        dataUltimoFechamento = objFechamentoBH.Data.Value.ToShortDateString();
                        saldoUltimoFechamento = objFechamentoBHD.Saldobh + " " + (objFechamentoBHD.Tiposaldo == 1 ? "D" : objFechamentoBHD.Tiposaldo == 0 ? "C" : "");
                    }
                }

                int credito, debito, saldoantcre = 0, saldoantdeb = 0, saldoatucre = 0, saldoatudeb = 0;

                credito = Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.creditoBHPeriodo);
                debito = Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.debitoBHPeriodo);

                if (objTotalHoras.sinalSaldoBHAtual == '+')
                {
                    saldoatucre = Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.saldoBHAtual);
                }
                else if (objTotalHoras.sinalSaldoBHAtual == '-')
                {
                    saldoatudeb = Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.saldoBHAtual);
                }

                if (pBuscarUltimoFechamento)
                {
                    if (objTotalHoras.sinalSaldoAnteriorBH == '+')
                    {
                        saldoantcre = Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.saldoAnteriorBH);
                    }
                    else if (objTotalHoras.sinalSaldoAnteriorBH == '-')
                    {
                        saldoantdeb = Modelo.cwkFuncoes.ConvertHorasMinuto(objTotalHoras.saldoAnteriorBH);
                    }
                }

                #region totaliza por departamento e por empresa

                totalcreditodepartamento += credito;
                totaldebitodepartamento += debito;
                totalSaldoAnteriorDepCre += saldoantcre;
                totalSaldoAnteriorDepDeb += saldoantdeb;
                totalSaldoAtualDepCre += saldoatucre;
                totalSaldoAtualDepDeb += saldoatudeb;
                totalcreditoempresa += credito;
                totaldebitoempresa += debito;
                totalSaldoAnteriorEmpCre += saldoantcre;
                totalSaldoAnteriorEmpDeb += saldoantdeb;
                totalSaldoAtualEmpCre += saldoatucre;
                totalSaldoAtualEmpDeb += saldoatudeb;

                #region saldo atual

                string totalsaldoatuemp = "", totalsaldoatudep = "", legendasaldoatuemp = "", legendasaldoatudep = "";

                if (totalSaldoAtualDepCre > totalSaldoAtualDepDeb)
                {
                    totalsaldoatudep = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalSaldoAtualDepCre - totalSaldoAtualDepDeb);
                    legendasaldoatudep = "C";
                }
                else if (totalSaldoAtualDepDeb > totalSaldoAtualDepCre)
                {
                    totalsaldoatudep = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalSaldoAtualDepDeb - totalSaldoAtualDepCre);
                    legendasaldoatudep = "D";
                }
                else
                {
                    totalsaldoatudep = "00:00";
                }

                if (totalSaldoAtualEmpCre > totalSaldoAtualEmpDeb)
                {
                    totalsaldoatuemp = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalSaldoAtualEmpCre - totalSaldoAtualEmpDeb);
                    legendasaldoatuemp = "C";
                }
                else if (totalSaldoAtualEmpDeb > totalSaldoAtualEmpCre)
                {
                    totalsaldoatuemp = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalSaldoAtualEmpDeb - totalSaldoAtualEmpCre);
                    legendasaldoatuemp = "D";
                }
                else
                {
                    totalsaldoatuemp = "00:00";
                }

                #endregion

                #region saldo anterior

                string totalsaldoantemp = "", totalsaldoantdep = "", legendasaldoantemp = "", legendasaldoantdep = "";

                if (totalSaldoAnteriorDepCre > totalSaldoAnteriorDepDeb)
                {
                    totalsaldoantdep = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalSaldoAnteriorDepCre - totalSaldoAnteriorDepDeb);
                    legendasaldoantdep = "C";
                }
                else if (totalSaldoAnteriorDepDeb > totalSaldoAnteriorDepCre)
                {
                    totalsaldoantdep = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalSaldoAnteriorDepDeb - totalSaldoAnteriorDepCre);
                    legendasaldoantdep = "D";
                }
                else
                {
                    totalsaldoantdep = "00:00";
                }

                if (totalSaldoAnteriorEmpCre > totalSaldoAnteriorEmpDeb)
                {
                    totalsaldoantemp = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalSaldoAnteriorEmpCre - totalSaldoAnteriorEmpDeb);
                    legendasaldoantemp = "C";
                }
                else if (totalSaldoAnteriorEmpDeb > totalSaldoAnteriorEmpCre)
                {
                    totalsaldoantemp = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalSaldoAnteriorEmpDeb - totalSaldoAnteriorEmpCre);
                    legendasaldoantemp = "D";
                }
                else
                {
                    totalsaldoantemp = "00:00";
                }

                #endregion

                if (!pBuscarUltimoFechamento)
                {
                    objTotalHoras.saldoAnteriorBH = "";
                    objTotalHoras.sinalSaldoAnteriorBH = new char();
                    totalsaldoantemp = "";
                    totalsaldoantdep = "";
                    var totalFunc = credito - debito;
                    var totalDepartamento = totalcreditodepartamento - totaldebitodepartamento;
                    var totalEmpresa = totalcreditoempresa - totaldebitoempresa;

                    objTotalHoras.saldoBHAtual = Modelo.cwkFuncoes.ConvertMinutosHora(5, credito - debito);
                    if (totalFunc < 0)
                    {
                        objTotalHoras.saldoBHAtual = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalFunc * -1);
                        objTotalHoras.sinalSaldoBHAtual = '-';
                    }
                    else if (totalFunc > 0)
                    {
                        objTotalHoras.saldoBHAtual = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalFunc);
                        objTotalHoras.sinalSaldoBHAtual = '+';
                    }
                    else
                    {
                        objTotalHoras.saldoBHAtual = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalFunc); ;
                        objTotalHoras.sinalSaldoBHAtual = new char();
                    }

                    if (totalDepartamento < 0)
                    {
                        totalsaldoatudep = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalDepartamento * -1);
                        legendasaldoatudep = "D";
                    }
                    else if (totalDepartamento > 0)
                    {
                        totalsaldoatudep = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalDepartamento);
                        legendasaldoatudep = "C";
                    }
                    else { legendasaldoatudep = ""; }

                    if (totalEmpresa < 0)
                    {
                        totalsaldoatuemp = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalEmpresa * -1);
                        legendasaldoatuemp = "D";
                    }
                    else if (totalEmpresa > 0)
                    {
                        totalsaldoatuemp = Modelo.cwkFuncoes.ConvertMinutosHora(5, totalEmpresa);
                        legendasaldoatuemp = "C";
                    }
                    else { legendasaldoatuemp = ""; }
                }

                #endregion

                #region Insere os registros no dataset
                object[] values = new object[]
                {
                    row["idfuncionario"],
                    row["nomefuncionario"],
                    row["dscodigo"],
                    row["matricula"],
                    row["idempresa"],
                    row["iddepartamento"],
                    row["idfuncao"],
                    row["nomeempresa"],
                    row["endereco"],
                    row["cidade"],
                    row["estado"],
                    row["cep"],
                    row["cnpj_cpf"],
                    row["nomedepartamento"],
                    row["nomeFuncao"],
                    row["Periodo"],
                    dataUltimoFechamento, 
                    saldoUltimoFechamento,
                    objTotalHoras.saldoAnteriorBH,
                    objTotalHoras.sinalSaldoAnteriorBH == '+' ? "C" : (objTotalHoras.sinalSaldoAnteriorBH == '-' ? "D" : ""),
                    objTotalHoras.creditoBHPeriodo,
                    objTotalHoras.debitoBHPeriodo,
                    objTotalHoras.saldoBHAtual,
                    objTotalHoras.sinalSaldoBHAtual == '+' ? "C" : (objTotalHoras.sinalSaldoBHAtual == '-' ? "D" : ""),
                    Modelo.cwkFuncoes.ConvertMinutosHora(5, totalcreditodepartamento),
                    Modelo.cwkFuncoes.ConvertMinutosHora(5, totaldebitodepartamento),
                    totalsaldoatudep,
                    legendasaldoatudep,
                    totalsaldoantdep,
                    legendasaldoantdep,
                    Modelo.cwkFuncoes.ConvertMinutosHora(5, totalcreditoempresa),
                    Modelo.cwkFuncoes.ConvertMinutosHora(5, totaldebitoempresa),
                    totalsaldoatuemp,
                    legendasaldoatuemp,
                    totalsaldoantemp,
                    legendasaldoantemp,
                    objTotalHoras.sinalSaldoAnteriorBH == '-' ? objTotalHoras.sinalSaldoAnteriorBH + objTotalHoras.saldoAnteriorBH : objTotalHoras.saldoAnteriorBH,
                    objTotalHoras.sinalSaldoBHAtual == '-' ? objTotalHoras.sinalSaldoBHAtual+objTotalHoras.saldoBHAtual : objTotalHoras.saldoBHAtual
                };
                ret.Rows.Add(values);
                #endregion

                objProgressBar.incrementaPB(1);
            }

            return ret;
        }

        public DataTable GetRelatorioHorario(DateTime pDataInicial, DateTime pDataFinal, int pTipo, string pIds)
        {
            objProgressBar.setaMensagem("Carregando dados...");

            System.Diagnostics.Stopwatch tempo = new System.Diagnostics.Stopwatch();
            tempo.Start();

            DataTable dt = dalBancoHoras.GetRelatorioHorario(pDataInicial, pDataFinal, pTipo, pIds);

            tempo.Stop();
            //Modelo.Global.logs = new List<string>();
            //Modelo.Global.logs.Add("--------------------------------------------------------------------------------");
            //Modelo.Global.logs.Add("Máquina: " + Environment.MachineName);
            //Modelo.Global.logs.Add("Banco de Horas Individual - " + DateTime.Now + ": ");
            //Modelo.Global.logs.Add("Consulta no banco de dados: " + tempo.Elapsed);
            //Modelo.Global.logs.Add("--------------------------------------------------------------------------------");
            //Modelo.cwkFuncoes.GravaLog(true);

            objProgressBar.setaMinMaxPB(0, dt.Rows.Count);
            objProgressBar.setaValorPB(0);

            DataTable ret = new DataTable();

            #region Adiciona as colunas no DataTable de retorno
            List<DataColumn> colunas = new List<DataColumn>();
            foreach (DataColumn c in dt.Columns)
            {
                if (Array.IndexOf(new string[] { "idhorario","entrada_5", "entrada_6", "entrada_7", "entrada_8", 
                    "saida_5", "saida_6", "saida_7", "saida_8", "tipohorario"
                    , "entrada_1normal", "entrada_2normal", "entrada_3normal", "entrada_4normal"
                    , "saida_1normal", "saida_2normal", "saida_3normal", "saida_4normal"
                    , "entrada_1flexivel", "entrada_2flexivel", "entrada_3flexivel", "entrada_4flexivel"
                    , "saida_1flexivel", "saida_2flexivel", "saida_3flexivel", "saida_4flexivel", "flagfolganormal", "flagfolgaflexivel"
                }, c.ColumnName) == -1)
                {
                    ret.Columns.Add(c.ColumnName);
                }
            }

            ret.Columns.AddRange
            (
                new DataColumn[]
                {
                    new DataColumn("saldobh"),
                    new DataColumn("legendaSaldo"),
                    new DataColumn("horEntrada1"),
                    new DataColumn("horEntrada2"),
                    new DataColumn("horSaida1"),
                    new DataColumn("horSaida2"),
                    new DataColumn("totalCredito"),
                    new DataColumn("totalDebito"),
                    new DataColumn("totalSaldo"),
                    new DataColumn("legendaTotalSaldo")
                }
            );

            #endregion

            int saldo = 0, credito, debito, totalCredito = 0, totalDebito = 0, totalSaldo = 0;
            char legendaSaldo = new char(), legendaTotalSaldo = new char();
            int idfuncionarioAnt = 0, idfuncionario = 0;

            string entrada_1, entrada_2, entrada_3, entrada_4;
            string saida_1, saida_2, saida_3, saida_4;
            bool flagFolga = false, folga = false;
            foreach (DataRow row in dt.Rows)
            {
                DateTime data = Convert.ToDateTime(row["data"]);
                idfuncionario = Convert.ToInt32(row["idfuncionario"]);
                credito = Modelo.cwkFuncoes.ConvertHorasMinuto(row["bancohorascre"].ToString());
                debito = Modelo.cwkFuncoes.ConvertHorasMinuto(row["bancohorasdeb"].ToString());
                folga = row["folga"] is DBNull ? false : Convert.ToBoolean(row["folga"]);
                CalculaSaldoRel(credito, debito, out saldo, out legendaSaldo);

                #region Calcula os totais

                if (idfuncionarioAnt != idfuncionario)
                {
                    objProgressBar.setaMensagem("Funcionário(a): " + row["funcionario"]);
                    totalCredito = 0;
                    totalDebito = 0;
                    totalSaldo = 0;

                    idfuncionarioAnt = idfuncionario;
                }

                totalCredito += credito;
                totalDebito += debito;
                totalSaldo = saldo;
                CalculaSaldoRel(totalCredito, totalDebito, out totalSaldo, out legendaTotalSaldo);

                #endregion

                int idhorario = Convert.ToInt32(row["idhorario"]);

                #region Carrega o Horário Detalhe

                if (Convert.ToInt32(row["tipohorario"]) == 1)
                {
                    entrada_1 = row["entrada_1normal"].ToString();
                    entrada_2 = row["entrada_2normal"].ToString();
                    entrada_3 = row["entrada_3normal"].ToString();
                    entrada_4 = row["entrada_4normal"].ToString();
                    saida_1 = row["saida_1normal"].ToString();
                    saida_2 = row["saida_2normal"].ToString();
                    saida_3 = row["saida_3normal"].ToString();
                    saida_4 = row["saida_4normal"].ToString();
                    flagFolga = row["flagfolganormal"] is DBNull ? false : Convert.ToBoolean(row["flagfolganormal"]);
                }
                else
                {
                    if (row["entrada_1flexivel"] is DBNull)
                    {
                        entrada_1 = "--:--";
                        entrada_2 = "--:--";
                        entrada_3 = "--:--";
                        entrada_4 = "--:--";
                        saida_1 = "--:--";
                        saida_2 = "--:--";
                        saida_3 = "--:--";
                        saida_4 = "--:--";                        
                    }
                    else
                    {
                        entrada_1 = row["entrada_1flexivel"].ToString();
                        entrada_2 = row["entrada_2flexivel"].ToString();
                        entrada_3 = row["entrada_3flexivel"].ToString();
                        entrada_4 = row["entrada_4flexivel"].ToString();
                        saida_1 = row["saida_1flexivel"].ToString();
                        saida_2 = row["saida_2flexivel"].ToString();
                        saida_3 = row["saida_3flexivel"].ToString();
                        saida_4 = row["saida_4flexivel"].ToString();
                        flagFolga = row["flagfolgaflexivel"] is DBNull ? false : Convert.ToBoolean(row["flagfolgaflexivel"]);
                    }
                }

                #endregion

                #region Insere os registros no dataset
                object[] values = new object[]
                {
                    row["id"],
                    row["legenda"],
                    data.ToShortDateString(),
                    row["dia"],
                    row["entrada_1"],
                    row["entrada_2"],
                    row["entrada_3"],
                    row["entrada_4"],                   
                    row["saida_1"],
                    row["saida_2"],
                    row["saida_3"],
                    row["saida_4"],
                    row["bancohorascre"],
                    row["bancohorasdeb"],
                    folga || flagFolga ? 1 : 0,
                    row["idfuncionario"],
                    row["dscodigo"],
                    row["funcionario"],
                    row["matricula"],
                    Convert.ToDateTime(row["dataadmissao"]).ToShortDateString(),
                    row["pis"],
                    row["horario"],
                    row["funcao"],
                    row["departamento"],
                    row["empresa"],
                    row["codigoempresa"],
                    row["cnpj_cpf"],
                    row["endereco"],
                    row["cidade"],
                    row["estado"],
                    row["cep"],
                    row["idempresa"],
                    Modelo.cwkFuncoes.ConvertMinutosHora(3, saldo),
                    char.IsLetter(legendaSaldo) ? legendaSaldo.ToString() : "",
                    entrada_1,
                    entrada_2,
                    saida_1,
                    saida_2,
                    Modelo.cwkFuncoes.ConvertMinutosHora(5, totalCredito),
                    Modelo.cwkFuncoes.ConvertMinutosHora(5, totalDebito),
                    Modelo.cwkFuncoes.ConvertMinutosHora(5, totalSaldo),
                    char.IsLetter(legendaTotalSaldo) ? legendaTotalSaldo.ToString() : ""
                };
                ret.Rows.Add(values);
                #endregion

                #region Insere os registros novamento se tiver mais de 8 marcações ou uma jornada com mais de 2 periodos
                if ((entrada_3 != "--:--" && !String.IsNullOrEmpty(entrada_3))
                    || (row["entrada_5"].ToString() != "--:--" && !String.IsNullOrEmpty(row["entrada_5"].ToString())))
                {
                    object[] values2 = new object[]
                {
                    "",
                    "",
                    "",
                    "",
                    row["entrada_5"],
                    row["entrada_6"],
                    row["entrada_7"],
                    row["entrada_8"],                   
                    row["saida_5"],
                    row["saida_6"],
                    row["saida_7"],
                    row["saida_8"],
                    "",
                    "",
                    folga || flagFolga ? 1 : 0,
                    row["idfuncionario"],
                    row["dscodigo"],
                    row["funcionario"],
                    row["matricula"],
                    Convert.ToDateTime(row["dataadmissao"]).ToShortDateString(),
                    row["pis"],
                    row["horario"],
                    row["funcao"],
                    row["departamento"],
                    row["empresa"],
                    row["codigoempresa"],
                    row["cnpj_cpf"],
                    row["endereco"],
                    row["cidade"],
                    row["estado"],
                    row["cep"],
                    row["idempresa"],
                    "",
                    "",
                    entrada_3,
                    entrada_4,
                    saida_3,
                    saida_4,
                    Modelo.cwkFuncoes.ConvertMinutosHora(5, totalCredito),
                    Modelo.cwkFuncoes.ConvertMinutosHora(5, totalDebito),
                    Modelo.cwkFuncoes.ConvertMinutosHora(5, totalSaldo),
                    char.IsLetter(legendaTotalSaldo) ? legendaTotalSaldo.ToString() : ""
                };
                    ret.Rows.Add(values2);
                }
                #endregion

                objProgressBar.incrementaPB(1);
            }

            return ret;
        }

        private static void CalculaSaldoRel(int credito, int debito, out int saldo, out char legendaSaldo)
        {
            if (credito > debito)
            {
                saldo = credito - debito;
                legendaSaldo = 'C';
            }
            else if (debito > credito)
            {
                saldo = debito - credito;
                legendaSaldo = 'D';
            }
            else
            {
                saldo = 0;
                legendaSaldo = new char();
            }

        }

        #endregion

        public List<Modelo.Funcionario> getFuncionariosBH(int pTipo, int pIdTipo)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            switch (pTipo)
            {
                case 0:
                    lista = dalFuncionario.getLista(pIdTipo);
                    break;
                case 1:
                    lista = dalFuncionario.GetPorDepartamentoList(pIdTipo);
                    break;
                case 2:
                    lista.Add(dalFuncionario.LoadObject(pIdTipo));
                    break;
                case 3:
                    lista = dalFuncionario.GetPorFuncaoList(pIdTipo);
                    break;
                default:
                    lista = null;
                    break;
            }
            return lista;
        }

        public void getInicioFimBH(int pIdBancoHoras, out DateTime? pDtInicio, out DateTime? pDtFim)
        {
            dalBancoHoras.getInicioFimBH(pIdBancoHoras, out pDtInicio, out pDtFim);
            return;
        }

        public List<Modelo.Proxy.PxySaldoBancoHoras> SaldoBancoHoras(DateTime dataSaldo, List<int> idsFuncs)
        {
            return dalBancoHoras.SaldoBancoHoras(dataSaldo, idsFuncs);
        }

        public List<Modelo.Proxy.Relatorios.PxyRelBancoHoras> RelatorioSaldoBancoHoras(string MesInicio, string AnoInicio, string MesFim, string AnoFim, List<int> idsFuncs)
        {
            return dalBancoHoras.RelatorioSaldoBancoHoras(MesInicio, AnoInicio, MesFim, AnoFim, idsFuncs);
        }

        public Modelo.BancoHoras BancoHorasPorFuncionario(DateTime data, int idFuncionario)
        {
            return dalBancoHoras.BancoHorasPorFuncionario(data, idFuncionario);
        }

        //public DataTable GetCreditoDebitoCalculoBanco(DateTime pInicial, DateTime pFinal, List<int> idsFuncs)
        //{
        //    return dalBancoHoras.GetCreditoDebitoCalculoBanco(pInicial, pFinal, idsFuncs);
        //}
        public List<Modelo.BancoHoras> GetAllListFuncs(bool verificaPermissao, List<int> idsFuncs)
        {
            return dalBancoHoras.GetAllListFuncs(verificaPermissao, idsFuncs);
        }

        public DataTable GetCredDebBancoHorasComSaldoPeriodo(List<int> idsFuncionarios, DateTime pdataInicial, DateTime pDataFinal)
        {
            return dalBancoHoras.GetCredDebBancoHorasComSaldoPeriodo(idsFuncionarios, pdataInicial, pDataFinal);
        }

        public Modelo.BancoHoras LoadObjectSemRestricao(int id)
        {
            return dalBancoHoras.LoadObjectSemRestricao(id);
        }

        public List<pxyFuncionarioRelatorio> GetFuncionarioParaCopia(int idBancoHoras)
        {
            return dalBancoHoras.GetFuncionarioParaCopia(idBancoHoras);
        }

        public void ReplicarBancoHoras(int idBancoHoras, List<int> idsFuncionarios)
        {
            dalBancoHoras.ReplicarBancoHoras(idBancoHoras, idsFuncionarios);
        }
    }
}
