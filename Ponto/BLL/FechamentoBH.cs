using DAL.SQL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    public class FechamentoBH : IBLL<Modelo.FechamentoBH>
    {
        private DAL.IFechamentoBH dalFechamentoBH;
        private DAL.IFechamentoBHD dalFechamentoBHD;
        private DAL.IFechamentobhdHE dalFechamentoBHDHE;
        private DAL.IBancoHoras dalBancoHoras;


        private BLL.FechamentobhdHE bllFechamentobhdHE;
        private Modelo.Cw_Usuario UsuarioLogado;

        private string ConnectionString;
        private Modelo.ProgressBar objProgressBar;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        public FechamentoBH()
            : this(null)
        {
            
        }

        public FechamentoBH(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {
            
        }

        public FechamentoBH(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }

            DataBase db = new DataBase(ConnectionString);
            dalFechamentoBH = new DAL.SQL.FechamentoBH(db);
            dalFechamentoBHD = new DAL.SQL.FechamentoBHD(db);
            dalFechamentoBHDHE = new DAL.SQL.FechamentobhdHE(db);
            dalBancoHoras = new DAL.SQL.BancoHoras(db);

            bllFechamentobhdHE = new BLL.FechamentobhdHE(ConnectionString, usuarioLogado);

            dalFechamentoBH.UsuarioLogado = usuarioLogado;
            dalFechamentoBHD.UsuarioLogado = usuarioLogado;
            dalFechamentoBHDHE.UsuarioLogado = usuarioLogado;

            dalBancoHoras.UsuarioLogado = usuarioLogado;

            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalFechamentoBH.MaxCodigo();
        }

        public bool VerificaSeExisteFechamento(int pCodigo)
        {
            return dalFechamentoBH.VerificaSeExisteFechamento(pCodigo);
        }

        public DataTable GetAll()
        {
            return dalFechamentoBH.GetAll();
        }

        public List<int> GetIds()
        {
            return dalFechamentoBH.GetIds();
        }

        /// <summary>
        /// Retorna uma tabela hash onde o código é a chave e o id é o valor
        /// </summary>
        /// <returns>Tabela Hash(Código, Id)</returns>
        public Hashtable GetHashCodigoId()
        {
            return dalFechamentoBH.GetHashCodigoId();
        }

        public Modelo.FechamentoBH LoadObject(int id)
        {
            return dalFechamentoBH.LoadObject(id);
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.FechamentoBH objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Data == new DateTime())
                ret.Add("txtData", "Escolha a data de fechamento do banco.");

            if (objeto.Tipo == -1)
                ret.Add("rgTipo", "Escolha o tipo do acerto.");

            if (objeto.Identificacao == -1)
                ret.Add("cbIdentificacao", "Escolha a identificação.");

            ValidaFechamento(objeto, ref ret);

            return ret;
        }


        public Dictionary<string, string> ValidaObjeto(Modelo.FechamentoBH objeto, Modelo.BancoHoras pBancoHoras)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Data == new DateTime())
                ret.Add("data", "Escolha a data de fechamento do banco.");

            if (objeto.Tipo == -1)
                ret.Add("rgTipo", "Escolha o tipo do acerto.");

            if (objeto.Identificacao == -1)
                ret.Add("cbIdentificacao", "Escolha a identificação.");

            if (objeto.Data < pBancoHoras.DataInicial || objeto.Data > pBancoHoras.DataFinal)
                ret.Add("data", "A data de fechamento está fora do intervalo do Banco de Horas");

            if (pBancoHoras.Id == -1)
                ret.Add("cbBancoHoras", "Escolha o banco de horas.");

            switch (pBancoHoras.Tipo)
            {
                case 1://Banco de Horas por departamento
                    if (objeto.Tipo == 0)//Tipo de fechamento por empresa
                        ret.Add("rgTipo", "O tipo do Banco de Horas é por departamento, o acerto não pode ser por empresa.");
                    break;
                case 2://Banco de horas por funcionario
                    if (objeto.Tipo != 2) //Fechamento de outro tipo
                        ret.Add("rgTipo", "O tipo do Banco de Horas é por funcionario, o acerto não pode ser de outro tipo");
                    break;
                case 3://Banco por função
                    if ((objeto.Tipo == 0) || (objeto.Tipo == 1))//Fechamento por empresa ou departamento
                        ret.Add("rgTipo", "O tipo do Banco de Horas é por funcão, o acerto não pode ser nem por empresa, nem por departamento");
                    break;
                default:
                    break;
            }

            ValidaFechamento(objeto, ref ret);

            return ret;
        }

        public void ValidaFechamento(Modelo.FechamentoBH objeto, ref Dictionary<string, string> ret)
        {
            BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new FechamentoPontoFuncionario(ConnectionString, UsuarioLogado);
            string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(objeto.Tipo, new List<int>() { objeto.Identificacao }, objeto.Data.GetValueOrDefault());
            if (!String.IsNullOrEmpty(mensagemFechamento))
            {
                ret.Add("Fechamento Ponto", mensagemFechamento);
            }
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.FechamentoBH objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalFechamentoBH.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalFechamentoBH.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFechamentoBH.Excluir(objeto);
                        ExcluirFechamento(objeto.Id);
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
            return dalFechamentoBH.getId(pValor, pCampo, pValor2);
        }

        /// <summary>
        /// Esse método realiza o fechamento do banco de horas.
        /// </summary>
        /// <param name="pTipo">Tipo do fechamento</param>
        /// <param name="pIdTipo">Id do tipo</param>
        /// <param name="pIdBancoHoras">Id do banco de horas</param>
        /// <param name="pData">Data de fechamento do banco de horas</param>
        /// <returns>Erros</returns>
        // Esse método inicialmente verifica as marcações dos funcionários que estão inclusos no fechamento e se 
        // não houver marcações para o mesmo o método cria.
        // Em seguida 
        public Dictionary<string, string> FechamentoBancoHoras(short pTipo, int pIdTipo, int pIdBancoHoras, DateTime pData)
        {
            return FachandoBH(pTipo, pIdTipo, pIdBancoHoras, ref pData, string.Empty, false, false, string.Empty, string.Empty);
        }

        public Dictionary<string, string> FechamentoBancoHoras(short pTipo, int pIdTipo, int pIdBancoHoras, DateTime pData, string motivo, bool pagamentoCreditosAutomaticamente, bool pagamentoDebitosAutomaticamente, string LimiteHorasPagamentoCredito, string LimiteHorasPagamentoDebito)
        {
            return FachandoBH(pTipo, pIdTipo, pIdBancoHoras, ref pData, motivo, pagamentoCreditosAutomaticamente, pagamentoDebitosAutomaticamente, LimiteHorasPagamentoCredito, LimiteHorasPagamentoDebito);
        }

        private Dictionary<string, string> FachandoBH(short pTipo, int pIdTipo, int pIdBancoHoras, ref DateTime pData, string  motivo, bool pagamentoCreditosAutomaticamente, bool pagamentoDebitosAutomaticamente, string LimiteHorasPagamentoCredito, string LimiteHorasPagamentoDebito)
        {
            Dictionary<string, string> erros = new Dictionary<string, string>();
            try
            {
                BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, UsuarioLogado);
                objProgressBar.setaMensagem("Carregando as informações para atualizar as marcações...");

                Modelo.BancoHoras objBancoHoras;
                Modelo.FechamentoBH objFechamentoBH;
                IList<Modelo.FechamentoBHDPercentual> listaobjFechamentoBHDPercentual = new List<Modelo.FechamentoBHDPercentual>();

                //Preenche os objetos
                erros = PreencheObjetos(out objBancoHoras, out objFechamentoBH, pTipo, pIdTipo, pIdBancoHoras, pData, motivo, pagamentoCreditosAutomaticamente, LimiteHorasPagamentoCredito, pagamentoDebitosAutomaticamente, LimiteHorasPagamentoDebito);

                if (erros.Count != 0)
                    return erros;

                //Atualiza as marcações
                bllMarcacao.InsereMarcacoesNaoExistentes(objBancoHoras.Tipo, objBancoHoras.Identificacao, objBancoHoras.DataInicial.Value, objFechamentoBH.Data.Value, objProgressBar, false);

                objProgressBar.setaMensagem("Calculando banco de horas...");

                //Realiza o fechamento do banco de horas por funcionario
                CalculaFechamento(objBancoHoras.DataInicial.Value, objBancoHoras.DataFinal.Value, objFechamentoBH, ref listaobjFechamentoBHDPercentual, pagamentoCreditosAutomaticamente, String.Empty, pagamentoDebitosAutomaticamente, String.Empty, objBancoHoras);
                              
                bllMarcacao.RecalculaMarcacao(objBancoHoras.Tipo, objBancoHoras.Identificacao, objFechamentoBH.Data.Value, objFechamentoBH.Data.Value, objProgressBar);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return erros;
        }


        public void ChamaCalculaFechamento(Modelo.BancoHoras objBancoHoras, Modelo.FechamentoBH objFechamentoBH, 
                                            ref IList<Modelo.FechamentoBHDPercentual> listaObjFechamentoBHDPercentual,
                                            bool pagamentoCreditosAutomaticamente, string LimiteHorasPagamentoCredito, bool pagamentoDebitosAutomaticamente,
                                            string LimiteHorasPagamentoDebito, ref Modelo.ProgressBar objProgressBarLocal)
        {
            CalculaFechamento(objBancoHoras.DataInicial.Value, objBancoHoras.DataFinal.Value, objFechamentoBH, ref listaObjFechamentoBHDPercentual, pagamentoCreditosAutomaticamente,
                LimiteHorasPagamentoCredito, pagamentoDebitosAutomaticamente, LimiteHorasPagamentoDebito, objBancoHoras);
        }

        private void CalculaFechamento(DateTime pDataInicial, DateTime pDataFinal, Modelo.FechamentoBH pObjFechamentoBH, ref IList<Modelo.FechamentoBHDPercentual> listaObjFechamentoBHPercentual, 
                                       bool pagamentoCreditosAutomaticamente, string LimiteHorasPagamentoCredito, bool pagamentoDebitosAutomaticamente, string LimiteHorasPagamentoDebito, 
                                       Modelo.BancoHoras objBancoHoras)
        {
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, UsuarioLogado);
            BLL.FechamentoBHDPercentual bllFechamentoBHDPercentual = new FechamentoBHDPercentual(ConnectionString, UsuarioLogado);

            Modelo.FechamentoBHDPercentual pObjFechamentoBHDPercentual;
            List<string> lstStrFechamentoBHD = new List<string>();
            List<string> lstStrUpdateMarcacao = new List<string>();
            Modelo.DadosFechamento fechamentoAnterior;
            Modelo.FechamentoBHD fechamentoBHD = new Modelo.FechamentoBHD();
            int idFunc = 0, credito = 0, debito = 0, saldo = 0, saldobh = 0, tipoSaldo = -1, i = 0;

            //Pega o saldo anterior do banco de todos os funcionarios do fechamento
            Hashtable htUltimoFechamento = dalFechamentoBH.getSaldoAnterior(pObjFechamentoBH.Tipo, pObjFechamentoBH.Identificacao);
            //Pega os totais do banco de cada funcionario do débito e do credito
            DataTable dtSaldoAnterior = dalFechamentoBH.getTotaisFuncionarios(pObjFechamentoBH.Tipo, pObjFechamentoBH.Identificacao, pDataInicial, pObjFechamentoBH.Data.Value);

            if (dtSaldoAnterior.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow funcRow in dtSaldoAnterior.Rows)
            {
                i++;
                idFunc = Convert.ToInt32(funcRow["id"]);
                credito = Convert.ToInt32(funcRow["creditobh"]);
                debito = Convert.ToInt32(funcRow["debitobh"]);

                //Testa se aquele funcinario tem um saldo anterior, se tiver pega, senao vai para o proximo
                if (htUltimoFechamento.ContainsKey(idFunc))
                {
                    fechamentoAnterior = new Modelo.DadosFechamento();
                    fechamentoAnterior = (Modelo.DadosFechamento)htUltimoFechamento[idFunc];

                    if (fechamentoAnterior.TipoSaldoBH == 0)
                        credito += fechamentoAnterior.saldoBH;
                    else
                        debito += fechamentoAnterior.saldoBH;
                }


                saldo = VerificaLimitePagamento(credito, LimiteHorasPagamentoCredito, debito, LimiteHorasPagamentoDebito, ref saldobh);
                tipoSaldo = saldo > 0 ? 0 : 1; //Credito = 0, debito = 1;

                fechamentoBHD = new Modelo.FechamentoBHD();
                fechamentoBHD.Idfechamentobh = pObjFechamentoBH.Id;
                fechamentoBHD.Seq = i;
                fechamentoBHD.Credito = Modelo.cwkFuncoes.ConvertMinutosHora(5, credito);
                fechamentoBHD.Debito = Modelo.cwkFuncoes.ConvertMinutosHora(5, debito);
                fechamentoBHD.Identificacao = idFunc;
                fechamentoBHD.Saldo = Modelo.cwkFuncoes.ConvertMinutosHora(5, Math.Abs(saldo));
                fechamentoBHD.Saldobh = Modelo.cwkFuncoes.ConvertMinutosHora(5, Math.Abs(saldobh));

                fechamentoBHD.MotivoFechamento = pObjFechamentoBH.MotivoFechamento;


                fechamentoBHD.Tiposaldo = tipoSaldo;
                fechamentoBHD.DataFechamento = pObjFechamentoBH.Data;

                DataTable dtFechBHPercentual = bllFechamentoBHDPercentual.GetBancoHorasPercentual(pDataInicial, (DateTime)pObjFechamentoBH.Data, idFunc, 1);

                if (pagamentoCreditosAutomaticamente == false &&
                    pagamentoDebitosAutomaticamente == false)
                {
                    fechamentoBHD.Saldobh = fechamentoBHD.Saldo;
                    fechamentoBHD.Saldo = "-----:--";
                }
                else if ((pagamentoCreditosAutomaticamente == true && debito > credito) &&
                         pagamentoDebitosAutomaticamente == false)
                {
                    fechamentoBHD.Saldobh = fechamentoBHD.Saldo;
                    fechamentoBHD.Saldo = "-----:--";
                }
                else if ((pagamentoDebitosAutomaticamente == true && credito > debito) &&
                          pagamentoCreditosAutomaticamente == false)
                {
                    fechamentoBHD.Saldobh = fechamentoBHD.Saldo;
                    fechamentoBHD.Saldo = "-----:--";
                }

                foreach (DataRow item in dtFechBHPercentual.Rows)
                {
                    pObjFechamentoBHDPercentual = new Modelo.FechamentoBHDPercentual();
                    pObjFechamentoBHDPercentual.Percentual = Convert.ToDecimal(item["percBanco"]);
                    pObjFechamentoBHDPercentual.CreditoPercentual = Convert.ToString(item["creditobhformatado"]);
                    pObjFechamentoBHDPercentual.DebitoPercentual = (Convert.ToString(item["debitobhformatado"]) == "--:--" ? "-----:--" : Convert.ToString(item["debitobhformatado"]));
                    pObjFechamentoBHDPercentual.SaldoPercentual = Convert.ToString(item["saldobhformatado"]);
                    pObjFechamentoBHDPercentual.idFuncionario = idFunc;
                    if (pagamentoCreditosAutomaticamente == true || pagamentoDebitosAutomaticamente)
                    {
                        pObjFechamentoBHDPercentual.HorasPagasPercentual = pObjFechamentoBHDPercentual.SaldoPercentual;
                        pObjFechamentoBHDPercentual.SaldoPercentual = "-----:--";
                    }
                    listaObjFechamentoBHPercentual.Add(pObjFechamentoBHDPercentual);
                }

                lstStrFechamentoBHD.Add(dalFechamentoBHD.MontaStringInsert(fechamentoBHD));
                lstStrUpdateMarcacao.Add(bllMarcacao.MontaUpdateFechamento(idFunc, pObjFechamentoBH.Id, pDataInicial, pObjFechamentoBH.Data.Value));
            }

            if (lstStrFechamentoBHD.Count != 0)
            {
                //Realiza o Rateio do Saldo do Banco para HE caso o banco esteja marcado com esta opção
                if (objBancoHoras.FechamentoPercentualHE)
                {
                    List<Modelo.FechamentobhdHE> lstFechamentobhdHE = bllFechamentobhdHE.CalculaRateioSaldoBhdHE(objBancoHoras, pObjFechamentoBH);
                    foreach (var objFechamentobhdHE in lstFechamentobhdHE)
                    {
                        bllFechamentobhdHE.Salvar(Modelo.Acao.Incluir, objFechamentobhdHE);
                    }
                }

                pObjFechamentoBH.Efetivado = 1;
                dalFechamentoBHD.SalvaLista(lstStrFechamentoBHD);
                this.Salvar(Modelo.Acao.Alterar, pObjFechamentoBH);
                dalFechamentoBHD.SalvaLista(lstStrUpdateMarcacao);
                IList<Modelo.FechamentoBHD> ListaFechamentoBHD = dalFechamentoBHD.GetFechamentoBHDPorIdFechamentoBH(pObjFechamentoBH.Id);

                foreach (var item in listaObjFechamentoBHPercentual)
                {
                    item.IdfechamentoBHD = ListaFechamentoBHD.Where(x => x.Identificacao == item.idFuncionario).FirstOrDefault().Id;
                    bllFechamentoBHDPercentual.Salvar(Modelo.Acao.Incluir, item);
                }

            }

        }

        private int VerificaLimitePagamento(int credito, string LimiteHorasPagamentoCredito, int debito, string LimiteHorasPagamentoDebito, ref int saldobh)
        {
            int saldo = credito - debito;

            try
            {
                int limiteCreditoPag = Modelo.cwkFuncoes.ConvertHorasMinuto(LimiteHorasPagamentoCredito);
                int limiteCreditoDeb = (Modelo.cwkFuncoes.ConvertHorasMinuto(LimiteHorasPagamentoDebito) * -1);

                if (saldo > 0)
                {
                    if (limiteCreditoPag > 0)
                    {
                        saldo -= limiteCreditoPag;

                        if (saldo > 0)
                            saldobh = limiteCreditoPag;
                        else
                            saldo = credito - debito;
                    }
                }
                else
                {
                    if (limiteCreditoDeb < 0)
                    {
                        saldo -= limiteCreditoDeb;

                        if (saldo < 0)
                            saldobh = limiteCreditoDeb;
                        else
                            saldo = credito - debito;
                    }
                }
            }
            catch
            {
                saldobh = 0;
            }
            
            return saldo;
        }

        public Dictionary<string, string> ChamaPreencheObjeto(out Modelo.BancoHoras pObjBancoHoras, out Modelo.FechamentoBH pObjFechamentoBH, short pTipo, int pIdTipo,
                                                              int pIdBancoHoras, DateTime pData, String MotivoFechamento, bool pagamentoCreditosAutomaticamente, string LimiteHorasPagamentoCredito, bool pagamentoDebitosAutomaticamente,
                                                           string LimiteHorasPagamentoDebito)
        {
            return PreencheObjetos(out pObjBancoHoras, out pObjFechamentoBH, pTipo, pIdTipo, pIdBancoHoras, pData, MotivoFechamento, pagamentoCreditosAutomaticamente, LimiteHorasPagamentoCredito, pagamentoDebitosAutomaticamente, LimiteHorasPagamentoDebito);
        }

        private Dictionary<string, string> PreencheObjetos(out Modelo.BancoHoras pObjBancoHoras, out Modelo.FechamentoBH pObjFechamentoBH, short pTipo, int pIdTipo, int pIdBancoHoras, DateTime pData, String MotivoFechamento, bool pagamentoCreditosAutomaticamente, string LimiteHorasPagamentoCredito, bool pagamentoDebitosAutomaticamente, string LimiteHorasPagamentoDebito)
        {
            Dictionary<string, string> erros = new Dictionary<string, string>();
            string NomePessoaObjFechamento = String.Empty;
            //Lê o objeto Banco de horas
            pObjBancoHoras = dalBancoHoras.LoadObject(pIdBancoHoras);

            //Cria e preenche o objeto fechamento BH
            pObjFechamentoBH = new Modelo.FechamentoBH();
            pObjFechamentoBH.Tipo = pTipo;
            pObjFechamentoBH.Identificacao = pIdTipo;
            pObjFechamentoBH.Data = pData;
            pObjFechamentoBH.MotivoFechamento = MotivoFechamento;
            pObjFechamentoBH.PagamentoHoraCreAuto = pagamentoCreditosAutomaticamente;
            pObjFechamentoBH.PagamentoHoraDebAuto = pagamentoDebitosAutomaticamente;
            pObjFechamentoBH.LimiteHorasPagamentoCredito = LimiteHorasPagamentoCredito;
            pObjFechamentoBH.LimiteHorasPagamentoDebito = LimiteHorasPagamentoDebito;
            pObjFechamentoBH.IdBancoHoras = pIdBancoHoras;

            erros = this.ValidaObjeto(pObjFechamentoBH, pObjBancoHoras);

            if (erros.Count == 0)
            {
                pObjFechamentoBH.Codigo = this.MaxCodigo();
                dalFechamentoBH.Incluir(pObjFechamentoBH);
            }

            return erros;
        }


        public void ExcluirFechamento(int pIdFechamento)
        {
            BLL.Marcacao bllMarcacao = new BLL.Marcacao(ConnectionString, UsuarioLogado);
            bllMarcacao.ClearFechamentoBH(pIdFechamento);
            dalFechamentoBH.ClearFechamentoBH(pIdFechamento);

            return;
        }

        public List<Modelo.FechamentoBH> GetAllList()
        {
            return dalFechamentoBH.GetAllList();
        }

        public List<Modelo.FechamentoBH> GetAllListFuncs(List<int> idsFuncs, bool ValidaPermissao)
        {
            return dalFechamentoBH.GetAllListFuncs(idsFuncs, ValidaPermissao);
        }

        public List<Modelo.FechamentoBH> GetAllListFuncs(List<int> idsFuncs)
        {
            return dalFechamentoBH.GetAllListFuncs(idsFuncs, true);
        }

    }
}
