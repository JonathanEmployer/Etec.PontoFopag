using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DAL.SQL;

namespace BLL
{
    public class Compensacao : IBLL<Modelo.Compensacao>
    {
        private DAL.ICompensacao dalCompensacao;
        private DAL.IFuncionario dalFuncionario;

        private Modelo.ProgressBar objProgressBar;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public Modelo.ProgressBar ObjProgressBar
        {
            get { return objProgressBar; }
            set { objProgressBar = value; }
        }

        struct TotalCompensado
        {
            public int idFuncionario;
            public int totalHorasCompensadas;
        }

        public Compensacao() : this(null)
        {

        }

        public Compensacao(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Compensacao(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            DataBase db = new DataBase(ConnectionString);
            dalCompensacao = new DAL.SQL.Compensacao(db);
            dalFuncionario = new DAL.SQL.Funcionario(db);

            switch (Modelo.cwkGlobal.BD)
            {
                case 2:
                    dalCompensacao = DAL.FB.Compensacao.GetInstancia;
                    dalFuncionario = DAL.FB.Funcionario.GetInstancia;
                    break;
            }
            dalCompensacao.UsuarioLogado = usuarioLogado;
            dalFuncionario.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalCompensacao.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalCompensacao.GetAll();
        }

        public List<Modelo.Compensacao> GetPeriodo(DateTime pDataInicial, DateTime pDataFinal, int? pTipo, List<int> pIdentificacoes)
        {
            return dalCompensacao.GetPeriodo(pDataInicial, pDataFinal, pTipo, pIdentificacoes);
        }

        public Modelo.Compensacao LoadObject(int id)
        {
            return dalCompensacao.LoadObject(id);
        }

        public List<Modelo.Compensacao> GetAllList()
        {
            return dalCompensacao.GetAllList();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Compensacao objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Periodoinicial == null)
            {
                ret.Add("txtPeriodoinicial", "Campo obrigatório.");
            }

            if (objeto.Periodofinal == null)
            {
                ret.Add("txtPeriodofinal", "Campo obrigatório.");
            }
            else if (objeto.Periodofinal < objeto.Periodoinicial)
            {
                ret.Add("txtPeriodofinal", "A data final da compensação deve ser maior do que a data inicial.");
            }

            if (objeto.DiasC.Count == 0)
            {
                if (objeto.Diacompensarinicial == null)
                {
                    ret.Add("txtDiascompensarinicial", "Campo obrigatório.");
                }

                if (objeto.Diacompensarfinal == null)
                {
                    ret.Add("txtDiascompensarfinal", "Campo obrigatório.");
                }
                else if (objeto.Diacompensarfinal < objeto.Diacompensarinicial)
                {
                    ret.Add("txtDiascompensarfinal", "A data final do período a ser compensado deve ser maior do que a data inicial.");
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

            ValidaFechamentoPonto(objeto, ref ret);

            return ret;
        }

        public void ValidaFechamentoPonto(Modelo.Compensacao objeto, ref Dictionary<string, string> ret)
        {
            BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new FechamentoPontoFuncionario(ConnectionString, UsuarioLogado);
            DateTime menorData = DateTime.Now;
            if (objeto.DiasC != null && objeto.DiasC.Count() > 0)
            {
                menorData = objeto.DiasC.Min(x => x.Datacompensada.GetValueOrDefault());
            }

            if (objeto.Periodoinicial.GetValueOrDefault() != default(DateTime) && objeto.Periodoinicial.GetValueOrDefault() < menorData)
                menorData = objeto.Periodoinicial.GetValueOrDefault();
            if (objeto.Diacompensarfinal.GetValueOrDefault() != default(DateTime) && objeto.Diacompensarfinal.GetValueOrDefault() < menorData)
                menorData = objeto.Diacompensarfinal.GetValueOrDefault();
            string mensagemFechamento = bllFechamentoPontoFuncionario.RetornaMensagemFechamentosPorFuncionarios(objeto.Tipo, new List<int>() { objeto.Identificacao }, menorData);
            if (!String.IsNullOrEmpty(mensagemFechamento))
            {
                ret.Add("Fechamento Ponto", mensagemFechamento);
            }
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Compensacao pobjCompensacao)
        {
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            Dictionary<string, string> erros = ValidaObjeto(pobjCompensacao);
            if (erros.Count == 0)
            {

                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalCompensacao.Incluir(pobjCompensacao);
                        if (!pobjCompensacao.NaoRecalcular)
                        {
                            bllMarcacao.InsereMarcacoesNaoExistentes(pobjCompensacao.Tipo,
                                pobjCompensacao.Identificacao, pobjCompensacao.Periodoinicial.Value,
                                pobjCompensacao.Periodofinal.Value, objProgressBar, false);                            
                        }
                        break;
                    case Modelo.Acao.Alterar:
                        dalCompensacao.Alterar(pobjCompensacao);
                        break;
                    case Modelo.Acao.Excluir:
                        dalCompensacao.Excluir(pobjCompensacao);
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
            return dalCompensacao.getId(pValor, pCampo, pValor2);
        }


        /// <summary>
        /// Verifica se tem compesação para aquela empresa/departamento/funcao/funcionario numa
        /// data especifica.
        /// </summary>
        /// <param name="pData">Data a ser consultada. Tem compensação nessa data?</param>
        /// <param name="idFuncionario">Id do funcionario a ser consultado (se tem compensação para ele) </param>
        /// <param name="idDepartamento">Id do departamento que se deseja saber se tem compensações (ou do funcionario do departamento) </param>
        /// <param name="idFuncao">Id da função que se deseja saber as compensações (ou do funcionario)</param>
        /// <param name="idEmpresa">Id da empresa que se deseja saber as compensações (ou do funcionario)</param>
        /// <returns> Lista de compensações ja tratada</returns>
        public List<Modelo.Compensacao> getListaCompensacao(DateTime pData, int idFuncionario, int idDepartamento, int idFuncao, int idEmpresa)
        {
            List<Modelo.Compensacao> listaTratada = new List<Modelo.Compensacao>();

            foreach (Modelo.Compensacao comp in dalCompensacao.getListaCompensacao(pData))
            {
                switch (comp.Tipo)
                {
                    case 0:
                        if (idEmpresa == comp.Identificacao)
                            listaTratada.Add(comp);
                        break;
                    case 1:
                        if (idDepartamento == comp.Identificacao)
                            listaTratada.Add(comp);
                        break;
                    case 2:
                        if (idFuncionario == comp.Identificacao)
                            listaTratada.Add(comp);
                        break;
                    case 3:
                        if (idFuncao == comp.Identificacao)
                            listaTratada.Add(comp);
                        break;
                    default:
                        break;
                }
            }

            return listaTratada;
        }

        public List<Modelo.Compensacao> getListaCompensacao(DateTime pData, int idFuncionario, int idDepartamento, int idFuncao, int idEmpresa, List<Modelo.Compensacao> pCompensacaoLista)
        {
            List<Modelo.Compensacao> lista = new List<Modelo.Compensacao>();

            lista = pCompensacaoLista.Where(c => (pData >= c.Periodoinicial && pData <= c.Periodofinal)
                && (c.Tipo == 2 && c.Identificacao == idFuncionario)).ToList();
            if (lista.Count == 0)
            {
                lista = pCompensacaoLista.Where(c => (pData >= c.Periodoinicial && pData <= c.Periodofinal)
                    && (c.Tipo == 1 && c.Identificacao == idDepartamento)).ToList();
                if (lista.Count == 0)
                {
                    lista = pCompensacaoLista.Where(c => (pData >= c.Periodoinicial && pData <= c.Periodofinal)
                        && (c.Tipo == 3 && c.Identificacao == idFuncao)).ToList();
                    if (lista.Count == 0)
                    {
                        lista = pCompensacaoLista.Where(c => (pData >= c.Periodoinicial && pData <= c.Periodofinal)
                            && (c.Tipo == 0 && c.Identificacao == idEmpresa)).ToList();
                    }
                }
            }

            return lista;
            //return pCompensacaoLista.Where(c => (pData >= c.Periodoinicial && pData <= c.Periodofinal)
            //    && ((c.Tipo == 0 && c.Identificacao == idEmpresa) || (c.Tipo == 1 && c.Identificacao == idDepartamento)
            //    || (c.Tipo == 2 && c.Identificacao == idFuncionario) || (c.Tipo == 3 && c.Identificacao == idFuncao))).ToList();
        }

        /// <summary>
        /// Recebe uma lista de funcionarios com suas respectivas horas compensadas já totalizadas
        /// Pega a lista das datas a serem compensadas, coloca em uma lista ordenada.
        /// Faz o rateio das horas dos funcionarios entre os dias a serem compensados.
        /// </summary>
        /// <param name="pListaCompensacao"> Lista de compensação </param>
        /// <param name="pObjCompensacao"> Objeto compensação que vai ser utilizado. </param>
        //PAM - 11/12/2009
        public List<string> RateioHorasCompensadas(DataTable pListaCompensacao, int pIdCompensacao)
        {
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            List<string> log = new List<string>();
            

            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
            Modelo.Compensacao objCompensacao = dalCompensacao.LoadObject(pIdCompensacao);

            System.DateTime data0 = new DateTime();

            List<System.DateTime> datasList = new List<DateTime>();

            //Lista de datas recebe a lista de datas a serem compensadas
            if ((objCompensacao.Diacompensarfinal != null) && (objCompensacao.Diacompensarinicial != null))
            {
                data0 = objCompensacao.Diacompensarinicial.Value;
                while (data0 <= objCompensacao.Diacompensarfinal.Value)
                {
                    datasList.Add(data0);
                    data0 = data0.AddDays(1);
                }
            }

            bool jaExiste = false;

            //Percorre toda a lista de dias de compensação e se tiver alguma data que já esta na lista não inclui na nova
            foreach (Modelo.DiasCompensacao dias in objCompensacao.DiasC)
            {
                //Testa se aquele dia não esta na lista
                foreach (System.DateTime dt in datasList)
                    if (dias.Datacompensada.Value == dt)
                    {
                        jaExiste = true;
                        break;
                    }
                //Se não existe na lista adiciona, se existe, não
                if (!jaExiste)
                    datasList.Add(dias.Datacompensada.Value);
            }

            //Ordena a lista para fazer o rateio das horas
            datasList.Sort();

            objProgressBar.setaMinMaxPB(0, pListaCompensacao.Rows.Count);
            objProgressBar.setaValorPB(0);

            //Para cada item da lista de compensação percorre a lista de datas e faz o rateio
            bool funcionarioCompensado;
            //Apenas os updates do funcionario
            List<Modelo.Marcacao> marcacoesBanco = bllMarcacao.GetListaCompesacao(datasList, objCompensacao.Tipo, objCompensacao.Identificacao);
            List<Modelo.Marcacao> marcacoesSalvar;
            List<Modelo.Funcionario> funcs = dalFuncionario.GetAllList(false, false);
            Modelo.Funcionario objFuncionario;
            foreach (DataRow compens in pListaCompensacao.Rows)
            {
                objProgressBar.incrementaPB(1);
                objProgressBar.setaMensagem("Funcionário: " + Convert.ToString(compens["nomefunc"]));
                var auxFunc = funcs.Where(f => f.Id == Convert.ToInt32(compens["idfuncionario"]));
                if (auxFunc.Count() > 0)
                    objFuncionario = auxFunc.First();
                else
                {
                    continue;
                }
                funcionarioCompensado = false;
                marcacoesSalvar = new List<Modelo.Marcacao>();
                int totalCompensado = Convert.ToInt32(compens["horascompensadas"]);
                foreach (System.DateTime data in datasList)
                {
                    //Pega a lista de marcação para aquela data
                    //objMarcacao = bllMarcacao.GetPorData(objFuncionario, data);
                    var auxMar = marcacoesBanco.Where(m => m.Idfuncionario == objFuncionario.Id && m.Data == data);
                    if (auxMar.Count() > 0)
                        objMarcacao = auxMar.First();
                    else
                    {
                        continue;
                    }

                    if (objMarcacao.Idcompensado > 0)
                    {
                        log.Add(objFuncionario.Nome + " - " + objMarcacao.Data.ToShortDateString());
                        funcionarioCompensado = true;
                        break;
                    }

                    //Se tiver falta entra, senao nao
                    if (objMarcacao.Horasfaltas != "--:--" || objMarcacao.Horasfaltanoturna != "--:--")
                    {
                        objMarcacao.Idfuncionario = objFuncionario.Id;
                        objMarcacao.Idcompensado = pIdCompensacao;
                        objMarcacao.Legenda = "C";
                        objMarcacao.Ocorrencia = "Horas Compensadas";

                        //Soma as horas trabalhadas com as extras mais o total compensado
                        //Para os casos que o funcionario trabalha no dia que esta sendo contado as horas da compensacao
                        int horasTrabalhadas = Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horastrabalhadas) + Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horastrabalhadasnoturnas) + Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasextrasdiurna) + Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasextranoturna);
                        int auxTotalCompensado = totalCompensado + horasTrabalhadas;
                        int horasFalta = Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasfaltas) + Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasfaltanoturna);

                        //As horas trabalhadas são somadas com as horas compensadas.
                        int hfd = Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasfaltas); //horas faltas diurnas
                        int hfn = Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horasfaltanoturna); //horas falta noturnas

                        //Compensou horas a mais
                        if ((totalCompensado > horasFalta) && objMarcacao.Naoentrarnacompensacao == 0)
                        {
                            objMarcacao.Horascompensadas = Modelo.cwkFuncoes.ConvertMinutosHora(horasFalta);
                            objMarcacao.Horastrabalhadas = CalculoHoras.OperacaoHoras('+', objMarcacao.Horastrabalhadas, objMarcacao.Horasfaltas);
                            objMarcacao.Horastrabalhadasnoturnas = CalculoHoras.OperacaoHoras('+', objMarcacao.Horastrabalhadasnoturnas, objMarcacao.Horasfaltanoturna);
                            objMarcacao.Horasfaltas = "--:--";
                            objMarcacao.Horasfaltanoturna = "--:--";
                            totalCompensado = totalCompensado - Modelo.cwkFuncoes.ConvertHorasMinuto(objMarcacao.Horascompensadas);
                            objMarcacao.Chave = objMarcacao.ToMD5();
                            marcacoesSalvar.Add(objMarcacao);
                        }
                        else //Compensou horas a menos ou igual
                        {
                            objMarcacao.Horascompensadas = Modelo.cwkFuncoes.ConvertMinutosBatida(totalCompensado);

                            //Testa primeiro as faltas diurnas e faz o rateio                            
                            if (objMarcacao.Horasfaltas != "--:--") //Tem faltas diurnas
                            {
                                if (totalCompensado > hfd) //Tem horas sobrando
                                {
                                    objMarcacao.Horastrabalhadas = CalculoHoras.OperacaoHoras('+', objMarcacao.Horastrabalhadas, objMarcacao.Horasfaltas);
                                    objMarcacao.Horasfaltas = "--:--";
                                    totalCompensado = totalCompensado - hfd;
                                }
                                else // Não compensou o total da falta diurna
                                {
                                    objMarcacao.Horastrabalhadas = CalculoHoras.OperacaoHoras('+', objMarcacao.Horastrabalhadas, Modelo.cwkFuncoes.ConvertMinutosHora(totalCompensado));
                                    objMarcacao.Horasfaltas = CalculoHoras.OperacaoHoras('-', objMarcacao.Horasfaltas, Modelo.cwkFuncoes.ConvertMinutosHora(totalCompensado));
                                    totalCompensado = 0;
                                }
                            }

                            //Depois testa as faltas noturnas e usa o que sobrou do diurno, ou se não entrou no diurno usa tudo
                            if (objMarcacao.Horasfaltanoturna != "--:--")
                            {
                                if (totalCompensado > hfn)//Tem horas sobrando
                                {
                                    objMarcacao.Horastrabalhadasnoturnas = CalculoHoras.OperacaoHoras('+', objMarcacao.Horastrabalhadasnoturnas, objMarcacao.Horasfaltanoturna);
                                    objMarcacao.Horasfaltanoturna = "--:--";
                                    totalCompensado = totalCompensado - hfn;
                                }
                                else
                                {
                                    objMarcacao.Horastrabalhadasnoturnas = CalculoHoras.OperacaoHoras('+', objMarcacao.Horastrabalhadasnoturnas, Modelo.cwkFuncoes.ConvertMinutosHora(totalCompensado));
                                    objMarcacao.Horasfaltanoturna = CalculoHoras.OperacaoHoras('-', objMarcacao.Horasfaltanoturna, Modelo.cwkFuncoes.ConvertMinutosHora(totalCompensado));
                                    totalCompensado = 0;
                                }
                            }
                            objMarcacao.Chave = objMarcacao.ToMD5();
                            marcacoesSalvar.Add(objMarcacao);
                            break;
                        }
                    }
                }

                //Se ainda tem horas na compensação, jogar as horas para extra do ultimo dia
                if (totalCompensado != 0 && !funcionarioCompensado)
                {
                    objMarcacao.Idcompensado = pIdCompensacao;
                    objMarcacao.Legenda = "C";
                    objMarcacao.Ocorrencia = "Horas Compensadas";
                    objMarcacao.Horasextrasdiurna = CalculoHoras.OperacaoHoras('+', Modelo.cwkFuncoes.ConvertMinutosBatida(totalCompensado), objMarcacao.Horasextrasdiurna);
                    objMarcacao.Chave = objMarcacao.ToMD5();
                    marcacoesSalvar.Add(objMarcacao);
                }

                if (!funcionarioCompensado && marcacoesSalvar.Count > 0)
                {
                    bllMarcacao.Salvar(Modelo.Acao.Alterar, marcacoesSalvar);
                }
            }
            return log;
        }

        /// <summary>
        /// As horas haviam sido compensadas e tinha tirado as faltas.
        /// Esse método retorna as faltas para o sistema e retira as horas compensadas.  
        /// </summary>
        /// <param name="pObjCompensacao">Objeto compensação</param>
        //PAM - 15/12/2009
        public void RetornaHorasParaFalta(Modelo.Compensacao pObjCompensacao)
        {
            BLL.Marcacao bllMarcacao = new Marcacao(ConnectionString, UsuarioLogado);
            Modelo.Marcacao objMarcacao;
            List<Modelo.Marcacao> marcacaoList = new List<Modelo.Marcacao>();
            List<Modelo.Marcacao> marcacaoListNova = new List<Modelo.Marcacao>();

            #region Período Compensado
            //Retorna a lista de marcações dos dias que foram compensados e devem ser colocado falta
            //Esse if é usado para o perído
            if ((pObjCompensacao.Diacompensarinicial != null) && (pObjCompensacao.Diacompensarfinal != null))
            {
                marcacaoList = bllMarcacao.getListaMarcacao(pObjCompensacao.Tipo, pObjCompensacao.Identificacao, pObjCompensacao.Diacompensarinicial.Value, pObjCompensacao.Diacompensarfinal.Value);

                objProgressBar.setaValorPB(0);
                objProgressBar.setaMinMaxPB(0, marcacaoList.Count);

                foreach (Modelo.Marcacao marc in marcacaoList)
                {
                    objProgressBar.incrementaPB(1);
                    if (marc.Idcompensado == pObjCompensacao.Id)
                    {
                        objMarcacao = marc;
                        objMarcacao.Idcompensado = 0;
                        objMarcacao.Acao = Modelo.Acao.Alterar;
                        objMarcacao.Horascompensadas = "";
                        //bllMarcacao.CalculaHoras(objMarcacao);
                        marcacaoListNova.Add(objMarcacao);
                    }
                }

                bllMarcacao.Salvar(Modelo.Acao.Alterar, marcacaoListNova);
            }


            #endregion

            #region Compensação com lista de dias
            if (pObjCompensacao.DiasC.Count != 0)
            {
                List<Modelo.Marcacao> lstMarc = new List<Modelo.Marcacao>();
                marcacaoListNova = new List<Modelo.Marcacao>();

                //Para cada dia da lista, carrega a lista de compensação por dia daquele tipo e faz as alterações necessarias
                foreach (Modelo.DiasCompensacao diaC in pObjCompensacao.DiasC)
                {
                    lstMarc = bllMarcacao.getListaMarcacao(pObjCompensacao.Tipo, pObjCompensacao.Identificacao, diaC.Datacompensada.Value, diaC.Datacompensada.Value);

                    objProgressBar.setaValorPB(0);
                    objProgressBar.setaMinMaxPB(0, lstMarc.Count);
                    objProgressBar.setaMensagem("Recalculando dia: " + String.Format("{0:dd/MM/yyyy}", diaC.Datacompensada.Value.Date));

                    //Pega todas as marcações de uma data especifica e faz as alterações nela
                    foreach (Modelo.Marcacao marc in lstMarc)
                    {
                        objProgressBar.incrementaPB(1);
                        if (marc.Idcompensado == pObjCompensacao.Id)
                        {
                            objMarcacao = marc;
                            objMarcacao.Acao = Modelo.Acao.Alterar;
                            objMarcacao.Idcompensado = 0;
                            objMarcacao.Horascompensadas = "";
                            //bllMarcacao.CalculaHoras(objMarcacao);
                            marcacaoListNova.Add(objMarcacao);
                        }
                    }
                }

                bllMarcacao.Salvar(Modelo.Acao.Alterar, marcacaoListNova);
            }
            #endregion
        }

        public List<Modelo.Compensacao> GetPeriodoByFuncionario(DateTime pDataInicial, DateTime pDataFinal, List<int> pdIdsFuncs)
        {
            return dalCompensacao.GetPeriodoByFuncionario(pDataInicial, pDataFinal,pdIdsFuncs);
        }

        public DataTable GetTotalCompensado(int pIdCompensacao)
        {
            return dalCompensacao.GetTotalCompensado(pIdCompensacao);
        }
    }
}
