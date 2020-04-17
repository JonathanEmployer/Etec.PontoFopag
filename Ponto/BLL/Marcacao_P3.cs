using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using DAL.SQL;

namespace BLL
{
    public partial class Marcacao : IBLL<Modelo.Marcacao>
    {
        private string LegendasConcatenadas;

        #region Métodos de Cálculo

        /// <summary>
        /// Método Responsável por recalcular as marcações de um determinado período
        /// </summary>
        /// <param name="pTipo">null = geral; 0 = empresa; 1 = departamento; 2 = funcionário; 3 = função; 4 = horário</param>
        /// <param name="pIdTipo">Identificação do tipo</param>
        /// <param name="pDataInicial">Data Inicial</param>
        /// <param name="pDataFinal">Data Final</param>
        public void RecalculaMarcacao(int? pTipo, int pIdTipo, DateTime pDataInicial, DateTime pDataFinal, Modelo.ProgressBar pProgressBar)
        {
            try
            {
                CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(pTipo, pIdTipo, pDataInicial, pDataFinal, pProgressBar, false, ConnectionString, UsuarioLogado, true);
                bllCalculaMarcacao.CalculaMarcacoes();
                bllCalculaMarcacao = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void RecalculaMarcacaoList(int? pTipo, List<int> plIdTipo, DateTime pDataInicial, DateTime pDataFinal, Modelo.ProgressBar pProgressBar)
        {
            try
            {
                foreach (int pIdTipo in plIdTipo)
                {
                    CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(pTipo, pIdTipo, pDataInicial, pDataFinal, pProgressBar, false, ConnectionString, UsuarioLogado, false);
                    bllCalculaMarcacao.CalculaMarcacoes();
                    bllCalculaMarcacao = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Preenche o funcionario não admitido, separado para organização do codigo
        /// </summary>
        /// <param name="objMarcacao">Objeto Marcação</param>
        ///PAM - 15/12/2009
        private void PreencheFuncionarioNaoAdmitido(Modelo.Marcacao objMarcacao)
        {
            objMarcacao.Horastrabalhadas = "--:--";
            objMarcacao.Horastrabalhadasnoturnas = "--:--";
            objMarcacao.Horasextrasdiurna = "--:--";
            objMarcacao.Horasextranoturna = "--:--";
            objMarcacao.Horasfaltas = "--:--";
            objMarcacao.Horasfaltanoturna = "--:--";
            objMarcacao.Bancohorascre = "---:--";
            objMarcacao.Bancohorasdeb = "---:--";
            objMarcacao.Ocorrencia = "Não Admitido";
            objMarcacao.Legenda = Convert.ToString("");
            objMarcacao.Dia = Modelo.cwkFuncoes.DiaSemana(objMarcacao.Data, Modelo.cwkFuncoes.TipoDiaSemana.Reduzido);
        }

        /// <summary>
        /// Preenche funcionario não admitido, separado para organização do código
        /// </summary>
        /// <param name="objMarcacao">Objeto Marcação</param>
        private void PreencheFuncionarioDemitido(Modelo.Marcacao objMarcacao)
        {
            objMarcacao.Ocorrencia = "Funcionário Demitido";
            objMarcacao.Horastrabalhadas = "--:--";
            objMarcacao.Horastrabalhadasnoturnas = "--:--";
            objMarcacao.Horasextrasdiurna = "--:--";
            objMarcacao.Horasextranoturna = "--:--";
            objMarcacao.Horasfaltas = "--:--";
            objMarcacao.Horasfaltanoturna = "--:--";
            objMarcacao.Bancohorascre = "---:--";
            objMarcacao.Bancohorasdeb = "---:--";
        }

        #endregion

        #region Busca Legenda

        /// <summary>
        /// Busca a legenda buscando os dados no banco de dados
        /// </summary>
        /// <param name="pObjMarcacao">Marcação</param>
        /// <param name="pFuncionario">Funcionário</param>
        /// <returns></returns>
        private string BuscaLegenda(Modelo.Marcacao pObjMarcacao, Modelo.Funcionario pFuncionario)
        {
            BLL.Feriado bllFeriado = new Feriado(ConnectionString, UsuarioLogado);
            BLL.Afastamento bllAfastamento = new Afastamento(ConnectionString, UsuarioLogado);
            string legenda = "";

            if (bllFeriado.PossuiRegistro(pObjMarcacao.Data, pFuncionario.Idempresa, pFuncionario.Iddepartamento))
            {
                legenda = "F";
                LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "F");
            }

            if (bllAfastamento.PossuiRegistro(pObjMarcacao.Data, pFuncionario.Idempresa, pFuncionario.Iddepartamento, pFuncionario.Id))
            {
                legenda = "A";
                LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "A");
            }

            return legenda;
        }

        /// <summary>
        /// Busca a legenda recebendo as listas de feriados, afastamentos e mudanças de horario já prontas
        /// </summary>
        /// <param name="pObjMarcacao">Marcação</param>
        /// <param name="pFuncionario">Funcionário</param>
        /// <param name="pFeriadosList">Lista de Feriados</param>
        /// <param name="pAfastamentoList">Lista de Afastamentos</param>
        /// <param name="pMudancaHorarioList">Lista de Mudanças de Horários</param>
        /// <returns></returns>
        private string BuscaLegenda(Modelo.Marcacao pObjMarcacao, int pIdFuncionario, int pIdDepartamento, int pIdEmpresa, List<Modelo.Feriado> pFeriadosList, List<Modelo.Afastamento> pAfastamentoList)
        {
            string legenda = "";
            string LegendasConcatenadas = "";
            if (pFeriadosList.Count > 0)
            {
                //Tem um feriado naquela data?
                if (pFeriadosList.Exists(f => (f.Data == pObjMarcacao.Data) && ((f.TipoFeriado == 0) || (f.TipoFeriado == 1 && f.IdEmpresa == pIdEmpresa) || (f.TipoFeriado == 2 && f.IdDepartamento == pIdDepartamento) || (f.TipoFeriado == 3 && (f.FeriadoFuncionarios != null && f.FeriadoFuncionarios.Where(w => w.IdFuncionario == pIdFuncionario).Count() > 0)))))
                {
                    legenda = "F";
                    LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "F");
                }
            }

            if (pAfastamentoList.Count > 0)
            {
                //Existe um afastamento para aquela data?
                if (pAfastamentoList.Exists(a => (a.Datai <= pObjMarcacao.Data) && ((a.Dataf == null ? DateTime.MaxValue : a.Dataf) >= pObjMarcacao.Data) && ((a.Tipo == 0 && a.IdFuncionario == pIdFuncionario) || (a.Tipo == 1 && a.IdDepartamento == pIdDepartamento) || (a.Tipo == 2 && a.IdEmpresa == pIdEmpresa))))
                {
                    legenda = "A";
                    LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "A");
                }
            }
            return legenda;
        }

        #endregion

        #region Insere Marcação em Branco

        public Modelo.Marcacao InsereMarcacaoLimpa(int pIdFuncionario, string pDsCodigo, int pIdHorario, DateTime pData)
        {
            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
            //objMarcacao.Codigo = this.MaxCodigo();
            objMarcacao.Codigo = 1;
            objMarcacao.Idfuncionario = pIdFuncionario;
            objMarcacao.Data = pData;
            objMarcacao.Dia = Modelo.cwkFuncoes.DiaSemana(pData, Modelo.cwkFuncoes.TipoDiaSemana.Reduzido);
            objMarcacao.Dscodigo = pDsCodigo;
            objMarcacao.Idhorario = pIdHorario;

            return objMarcacao;
        }

        public void MontaMarcFunc(int pIdFuncionario, DateTime pData, ref string comando)
        {
            dalMarcacao.MontaMarcFunc(pIdFuncionario, pData, ref comando);
            return;
        }

        #endregion

        #region Atualiza Data

        /// <summary>
        /// Método responsável por inserir, para um determinado funcionário, as marcações que não existem em um determinado período.
        /// </summary>
        /// <param name="pDataI">Data Inicial</param>
        /// <param name="pDataF">Data Final</param>
        /// <param name="objFuncionario">Funcionário</param>
        /// <param name="objHorario">Horário</param>
        /// <param name="pInclusaoBancoLista">lista de inclusões em banco existentes</param>
        public void AtualizaData(DateTime pDataI, DateTime pDataF, Modelo.Funcionario objFuncionario)
        {
            BLL.JornadaAlternativa bllJornadaAlternativa = new JornadaAlternativa(ConnectionString, UsuarioLogado);
            BLL.FechamentoBHD bllFechamentoBHD = new FechamentoBHD(ConnectionString, UsuarioLogado);
            BLL.Feriado bllFeriado = new Feriado(ConnectionString, UsuarioLogado);
            BLL.Afastamento bllAfastamento = new Afastamento(ConnectionString, UsuarioLogado);
            BLL.BancoHoras bllBancoHoras = new BancoHoras(ConnectionString, UsuarioLogado);
            BLL.MudancaHorario bllMudancaoHorario = new MudancaHorario(ConnectionString, UsuarioLogado);
            BLL.Parametros bllParametro = new Parametros(ConnectionString, UsuarioLogado);
            BLL.FechamentoPontoFuncionario bllFechamentoPontoFuncionario = new BLL.FechamentoPontoFuncionario(ConnectionString, UsuarioLogado);
            BLL.InclusaoBanco bllInclusaoBanco = new BLL.InclusaoBanco(ConnectionString, UsuarioLogado);
            BLL.ContratoFuncionario bllContratoFuncionario = new BLL.ContratoFuncionario(ConnectionString, UsuarioLogado);
            BLL.Contrato bllContrato = new BLL.Contrato(ConnectionString, UsuarioLogado);
            DateTime dt = new DateTime();
            dt = pDataI;

            List<int> idsFuncs = new List<int>() { objFuncionario.Id };

            List<Modelo.InclusaoBanco> pInclusaoBancoLista = bllInclusaoBanco.GetAllListByFuncionarios(idsFuncs);
            List<Modelo.JornadaAlternativa> jornadasAlternativas = bllJornadaAlternativa.GetPeriodoFuncionarios(pDataI, pDataF, idsFuncs);
            List<Modelo.FechamentoBHD> pFechamentoBHDLista = bllFechamentoBHD.getPorFuncionario(objFuncionario.Id);
            List<Modelo.Feriado> feriadoLista = bllFeriado.GetFeriadosFuncionarioPeriodo(objFuncionario.Id, pDataI, pDataF);
            List<Modelo.BancoHoras> bancoHorasLista = bllBancoHoras.GetAllListFuncs(false, new List<int>() { objFuncionario.Id });
            List<Modelo.Afastamento> afastamentosList = bllAfastamento.GetAfastamentoFuncionarioPeriodo(objFuncionario.Id, pDataI, pDataF);
            List<Modelo.MudancaHorario> mudancaHorarioList = bllMudancaoHorario.GetPeriodo(pDataI, pDataF, new List<int>() { objFuncionario.Id });
            List<Modelo.Marcacao> marcacoesPeriodo = this.GetPorFuncionario(objFuncionario.Id, pDataI, pDataF, true);
            ConcurrentBag<Modelo.Marcacao> marcacoes = new ConcurrentBag<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            List<Modelo.Contrato> contratosLista = new List<Modelo.Contrato>();

            if (afastamentosList.Any(x=>x.IdContrato !=null))
            {
                afastamentosList.ForEach(x =>
                {
                    if (x.IdContrato != null)
                    {
                        var obj = bllContrato.ContratosPorFuncionario(x.IdFuncionario);
                        contratosLista.Union(obj);
                    }
                });
            }

            IList<Modelo.Proxy.pxyFechamentoPontoFuncionario> fechamentos = bllFechamentoPontoFuncionario.ListaFechamentoPontoFuncionario(2, idsFuncs, pDataI);

            Hashtable listaIdsHorario = new Hashtable();
            listaIdsHorario.Add(objFuncionario.Idhorario, null);
            foreach (var m in mudancaHorarioList)
            {
                if (!listaIdsHorario.ContainsKey(m.Idhorario))
                    listaIdsHorario.Add(m.Idhorario, null);
                if (!listaIdsHorario.ContainsKey(m.Idhorario_ant))
                    listaIdsHorario.Add(m.Idhorario_ant, null);
            }
            List<Modelo.Horario> horarios = dalHorario.GetParaIncluirMarcacao(listaIdsHorario, true);
            List<Modelo.Parametros> parametros = bllParametro.GetAllList();

            List<DateTime> datas = Enumerable.Range(0, 1 + pDataF.Subtract(pDataI).Days)
              .Select(offset => pDataI.AddDays(offset))
              .ToList();
            marcacoes = new ConcurrentBag<Modelo.Marcacao>();
            Parallel.ForEach(datas, (data) =>
            {
                if (!marcacoesPeriodo.Exists(m => m.Data == data))
                {
                    if (data < (objFuncionario.DataInativacao ?? DateTime.MaxValue))
                    {
                        objMarcacao = AuxAtualizaData(objFuncionario.Dataadmissao.Value, objFuncionario.Datademissao, objFuncionario.Id, objFuncionario.Dscodigo, objFuncionario.Idhorario, objFuncionario.Idfuncao, objFuncionario.Iddepartamento, objFuncionario.Idempresa, objFuncionario.Naoentrarbanco, horarios, data, jornadasAlternativas, pFechamentoBHDLista, feriadoLista, bancoHorasLista, pInclusaoBancoLista, afastamentosList, mudancaHorarioList, parametros, false, fechamentos);
                        marcacoes.Add(objMarcacao);
                    }
                }
            });

            this.Salvar(Modelo.Acao.Incluir, marcacoes.OrderBy(o => o.Idfuncionario).ThenBy(o => o.Data).ToList());
            BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(2, objMarcacao.Idfuncionario, pDataI, pDataF, ObjProgressBar, false, ConnectionString, UsuarioLogado, false);
            bllCalculaMarcacao.CalculaMarcacoes();
        }

        public Dictionary<string, string> AtualizaData(List<Modelo.Parametros> Parametros, List<Modelo.Horario> pListaHorario, DateTime pDataI, DateTime pDataF, List<Modelo.Funcionario> ListFuncionario, List<Modelo.InclusaoBanco> InclusaoBancoLista, List<Modelo.JornadaAlternativa> JornadasAlternativas, List<Modelo.FechamentoBHD> FechamentoBHDLista, List<Modelo.Feriado> FeriadoLista, List<Modelo.BancoHoras> BancoHorasLista, List<Modelo.Afastamento> AfastamentosLista, List<Modelo.Contrato> ContratoLista, List<Modelo.MudancaHorario> MudancaHorarioList, List<Modelo.Marcacao> MarcacoesPeriodo, IList<Modelo.Proxy.pxyFechamentoPontoFuncionario> Fechamentos)
        {
            List<DateTime> datas = Enumerable.Range(0, 1 + pDataF.Subtract(pDataI).Days).Select(offset => pDataI.AddDays(offset)).ToList();
            Modelo.Marcacao objMarcacao = null;
            var items = new[] { 1, 2, 3 };
            var options = new ParallelOptions();
            ConcurrentBag<Modelo.Marcacao> _marcacoes = new ConcurrentBag<Modelo.Marcacao>();
            Parallel.ForEach(datas, options, (data) =>
            {
                Parallel.ForEach(ListFuncionario.Where(w => w.Excluido == 0), options, (funcionario) =>
                { //Apagado validação de initivos para permitir tratar as marcações de funcionarios Inativos no cartão ponto posteriores de 60 dias (antes o depois) Backlog 80832
                    if (!MarcacoesPeriodo.Exists(m => m.Data == data && m.Idfuncionario == funcionario.Id))
                    {
                        objMarcacao = AuxAtualizaData(
                                                funcionario,
                                                pListaHorario,
                                                data,
                                                JornadasAlternativas.Where(x => (
                                                    (x.Tipo == 0 && x.Identificacao == funcionario.Idempresa) ||
                                                    (x.Tipo == 1 && x.Identificacao == funcionario.Iddepartamento) ||
                                                    (x.Tipo == 2 && x.Identificacao == funcionario.Id) ||
                                                    (x.Tipo == 3 && x.Identificacao == funcionario.Idfuncao))

                                                ).ToList(),

                                                FechamentoBHDLista.Where(x => x.Identificacao == funcionario.Id).ToList(),

                                                FeriadoLista,

                                                BancoHorasLista.Where(x => (
                                                    (x.Tipo == 0 && x.Identificacao == funcionario.Idempresa) ||
                                                    (x.Tipo == 1 && x.Identificacao == funcionario.Iddepartamento) ||
                                                    (x.Tipo == 2 && x.Identificacao == funcionario.Id) ||
                                                    (x.Tipo == 3 && x.Identificacao == funcionario.Idfuncao))
                                                ).ToList(),

                                                InclusaoBancoLista.Where(x => (
                                                    (x.Tipo == 0 && x.Identificacao == funcionario.Idempresa) ||
                                                    (x.Tipo == 1 && x.Identificacao == funcionario.Iddepartamento) ||
                                                    (x.Tipo == 2 && x.Identificacao == funcionario.Id) ||
                                                    (x.Tipo == 3 && x.Identificacao == funcionario.Idfuncao))
                                                ).ToList(),

                                                AfastamentosLista.Where(x => (
                                                    (x.Tipo == 0 && x.IdFuncionario == funcionario.Id) ||
                                                    (x.Tipo == 1 && x.IdDepartamento == funcionario.Iddepartamento) ||
                                                    (x.Tipo == 2 && x.IdEmpresa == funcionario.Idempresa)
                                                //|| (x.Tipo == 3 && ContratoLista.Contains(ContratoLista.Where(y=>y.Id == x.IdContrato ).FirstOrDefault()))
                                                )).ToList(),

                                                MudancaHorarioList.Where(x => (
                                                    (x.Tipo == 0 && x.Idfuncionario == funcionario.Id) ||
                                                    (x.Tipo == 1 && x.IdDepartamento == funcionario.Iddepartamento) ||
                                                    (x.Tipo == 2 && x.IdEmpresa == funcionario.Idempresa) ||
                                                    (x.Tipo == 3 && x.IdFuncao == funcionario.Idfuncao))
                                                ).ToList(),

                                                Parametros,

                                                false,

                                                Fechamentos.Where(x => x.IdFuncionario == funcionario.Id).ToList()
                                    );
                        _marcacoes.Add(objMarcacao);
                    }
                });
            });

            Dictionary<string, string> ret = this.Salvar(Modelo.Acao.Incluir, _marcacoes.ToList());
            BLL.CalculaMarcacao bllCalculaMarcacao = new BLL.CalculaMarcacao(ListFuncionario.Select(s => s.Id).ToList(), pDataI, pDataF, ObjProgressBar, false, ConnectionString, UsuarioLogado);
            bllCalculaMarcacao.CalculaMarcacoes();
            return ret;
        }

        public void AtualizaManutDiaria(int pTipo, int pId, DateTime pDataIni, DateTime pDataFim, Modelo.ProgressBar barra)
        {
            //Insere as marcações que não existem
            this.InsereMarcacoesNaoExistentes(pTipo, pId, pDataIni, pDataFim, barra, true);
        }

        public Modelo.Marcacao AuxAtualizaData(Modelo.Funcionario Funcionario, List<Modelo.Horario> pHorarios, DateTime dt, List<Modelo.JornadaAlternativa> pJornadasAlternativas, List<Modelo.FechamentoBHD> pFechamentoBHDLista, List<Modelo.Feriado> pFeriadoLista, List<Modelo.BancoHoras> pBancoHorasLista, List<Modelo.InclusaoBanco> pInclusaoBancoLista, List<Modelo.Afastamento> pAfastamentoLista, List<Modelo.MudancaHorario> pMudancaHorario, List<Modelo.Parametros> parametros, bool incluir, IList<Modelo.Proxy.pxyFechamentoPontoFuncionario> fechamentos)
        {
            var pIdFuncionario = Funcionario.Id;
            var pDsCodigo = Funcionario.Dscodigo;
            var pIdDepartamento = Funcionario.Iddepartamento;
            var pIdEmpresa = Funcionario.Idempresa;
            var pIdHorario = Funcionario.Idhorario;
            var pDataAdmissao = Funcionario.Dataadmissao.Value;
            var pDataDemissao = Funcionario.Datademissao;
            var pIdFuncao = Funcionario.Idfuncao;
            var pNaoEntrarBanco = Funcionario.Naoentrarbanco;

            return this.AuxAtualizaData(pDataAdmissao, pDataDemissao, pIdFuncionario, pDsCodigo, pIdHorario, pIdFuncao, pIdDepartamento, pIdEmpresa, pNaoEntrarBanco, pHorarios, dt, pJornadasAlternativas, pFechamentoBHDLista, pFeriadoLista, pBancoHorasLista, pInclusaoBancoLista, pAfastamentoLista, pMudancaHorario, parametros, incluir, fechamentos);
        }

        /// <summary>
        /// Método que cria a marcação para uma determinada data
        /// </summary>
        /// <param name="objFuncionario">Funcionário</param>
        /// <param name="objHorario">Horário</param>
        /// <param name="dt">Data</param>
        /// <param name="pJornadasAlternativas">Lista de jornadas alternativas</param>        
        /// <param name="pFechamentoBHDLista">Lista de FechamentosBH</param>
        /// <param name="pFeriadoLista">Lista de Feriados</param>
        /// <param name="pBancoHorasLista">Lista de Bancos de Horas</param>
        /// <param name="pInclusaoBancoLista">Lista de Inclusões em banco</param>
        /// <param name="incluir">Flag que indica se a marcação deve ser gravada no banco de dados</param>
        /// <returns>Marcação criada</returns>
        public Modelo.Marcacao AuxAtualizaData(DateTime pDataAdmissao, DateTime? pDataDemissao, int pIdFuncionario, string pDsCodigo, int pIdHorario, int pIdFuncao, int pIdDepartamento, int pIdEmpresa, short pNaoEntrarBanco, List<Modelo.Horario> pHorarios, DateTime dt, List<Modelo.JornadaAlternativa> pJornadasAlternativas, List<Modelo.FechamentoBHD> pFechamentoBHDLista, List<Modelo.Feriado> pFeriadoLista, List<Modelo.BancoHoras> pBancoHorasLista, List<Modelo.InclusaoBanco> pInclusaoBancoLista, List<Modelo.Afastamento> pAfastamentoLista, List<Modelo.MudancaHorario> pMudancaHorario, List<Modelo.Parametros> parametros, bool incluir, IList<Modelo.Proxy.pxyFechamentoPontoFuncionario> fechamentos)
        {
            BLL.JornadaAlternativa bllJornadaAlternativa = new JornadaAlternativa(ConnectionString, UsuarioLogado);
            Modelo.Marcacao objMarcacao = new Modelo.Marcacao();

            objMarcacao.Codigo = 0;
            objMarcacao.Idfuncionario = pIdFuncionario;
            objMarcacao.Data = dt;
            objMarcacao.Dia = Modelo.cwkFuncoes.DiaSemana(dt, Modelo.cwkFuncoes.TipoDiaSemana.Reduzido);
            objMarcacao.Dscodigo = pDsCodigo;
            Modelo.Horario objHorario = null;
            Modelo.Parametros objParametro = null;
            string leg = BuscaLegenda(objMarcacao, pIdFuncionario, pIdDepartamento, pIdEmpresa, pFeriadoLista, pAfastamentoLista);
            int idhorario = pIdHorario;

            VerificaMudancaHorario(dt, pMudancaHorario, ref leg, ref idhorario);

            objMarcacao.Idhorario = idhorario;

            objHorario = pHorarios.Where(h => h.Id == objMarcacao.Idhorario).First();
            objParametro = parametros.Where(p => p.Id == objHorario.Idparametro).First();

            objMarcacao.TipoHoraExtraFalta = objParametro.TipoHoraExtraFalta;
            objMarcacao.Horasfaltas = "--:--";
            objMarcacao.Horasfaltanoturna = "--:--";

            #region Fechamento ponto
            objMarcacao.IdFechamentoPonto = 0;

            if (fechamentos != null)
            {
                Modelo.Proxy.pxyFechamentoPontoFuncionario fechamentoPontoFuncionario = fechamentos.Where(x => x.DataFechamento >= objMarcacao.Data && x.IdFuncionario == objMarcacao.Idfuncionario).OrderBy(x => x.DataFechamento).FirstOrDefault();
                if (fechamentoPontoFuncionario != null)
                {
                    objMarcacao.IdFechamentoPonto = fechamentoPontoFuncionario.IdFechamentoPonto;
                }
            }

            #endregion
            if (objMarcacao.IdFechamentoPonto == 0)
            {
                objMarcacao.Legenda = leg;
                objMarcacao.LegendasConcatenadas = leg;
                int i = Modelo.cwkFuncoes.Dia(objMarcacao.Data);
                if (objHorario.TipoHorario == 1)
                {
                    if ((objMarcacao.Legenda != "F") && (objMarcacao.Legenda != "A") && (!objHorario.HorariosDetalhe[i - 1].Neutro))
                    {
                        if (objHorario.Marcacargahorariamista == 1)
                        {
                            objMarcacao.Horasfaltas = objHorario.HorariosDetalhe[i - 1].Cargahorariamista;
                        }
                        else
                        {
                            objMarcacao.Horasfaltas = objHorario.HorariosDetalhe[i - 1].Totaltrabalhadadiurna;
                            objMarcacao.Horasfaltanoturna = objHorario.HorariosDetalhe[i - 1].Totaltrabalhadanoturna;
                        }
                    }
                }
                else
                {
                    foreach (Modelo.HorarioDetalhe hd in objHorario.HorariosFlexiveis)
                    {
                        if (hd.Data != objMarcacao.Data)
                        {
                            continue;
                        }
                        if (hd.Marcacargahorariamista == 1)
                        {
                            objMarcacao.Horasfaltas = hd.Cargahorariamista;
                        }
                        else
                        {
                            objMarcacao.Horasfaltas = hd.Totaltrabalhadadiurna;
                            objMarcacao.Horasfaltanoturna = hd.Totaltrabalhadanoturna;
                        }
                        break;
                    }
                }

                Modelo.JornadaAlternativa objJornadaAlternativa = bllJornadaAlternativa.PossuiRegistro(pJornadasAlternativas, objMarcacao.Data, pIdFuncionario, pIdFuncao, pIdDepartamento, pIdEmpresa);

                if (objJornadaAlternativa != null)
                {
                    objMarcacao.Horasfaltas = objJornadaAlternativa.TotalTrabalhadaDiurna;
                    objMarcacao.Horasfaltanoturna = objJornadaAlternativa.TotalTrabalhadaNoturna;
                }

                //Verifica se o funcionário está demitido
                if (pDataDemissao != null && pDataDemissao <= objMarcacao.Data)
                {
                    PreencheFuncionarioDemitido(objMarcacao);
                }
                else
                {
                    //Verifica se a data é anterior a Data de admissão do funcionário
                    if (pDataAdmissao != null && objMarcacao.Data < pDataAdmissao)
                    {
                        PreencheFuncionarioNaoAdmitido(objMarcacao);
                    }
                    else if (objMarcacao.Horasfaltas == "--:--" && objMarcacao.Horasfaltanoturna == "--:--")
                    {
                        objMarcacao.Ocorrencia = "";
                    }
                    else
                    {
                        objMarcacao.Ocorrencia = "Falta";
                    }
                }

                // {TESTAR}
                CalculaBancoHorasAtualizaData(objMarcacao, pIdFuncionario, pIdFuncao, pIdDepartamento, pIdEmpresa, pNaoEntrarBanco, objHorario, pFechamentoBHDLista, pInclusaoBancoLista, pBancoHorasLista);

                if (objJornadaAlternativa != null)
                {
                    if (objMarcacao.Legenda != "A")
                    {
                        if (objMarcacao.Legenda != "F")
                            objMarcacao.Legenda = "J";
                        LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "J");
                        //Colocado para atualziar a ocorrencia somente quando não for banco de horas
                        //CRNC - 18/01/2010
                        if ((objMarcacao.Bancohorascre == "---:--") && (objMarcacao.Bancohorasdeb == "---:--"))
                        {
                            objMarcacao.Ocorrencia = "Jornada Alternativa";
                        }
                    }
                }
            }
            objMarcacao.Chave = objMarcacao.ToMD5();
            if (incluir)
            {
                dalMarcacao.Incluir(objMarcacao);
            }

            return objMarcacao;
        }

        public void VerificaMudancaHorario(DateTime dt, List<Modelo.MudancaHorario> pMudancaHorario, ref string leg, ref int idhorario)
        {
            if (pMudancaHorario.Count > 0)
            {
                var mud = pMudancaHorario.Where(m => m.Data >= dt);

                if (mud.Count() > 0)
                {
                    Modelo.MudancaHorario objMudancaHorario = mud.Where(m => m.Data == mud.Min(x => x.Data)).First();
                    if (objMudancaHorario.Data == dt)
                    {
                        leg = "M";
                        LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "M");
                        idhorario = objMudancaHorario.Idhorario;
                    }
                    else
                    {
                        idhorario = objMudancaHorario.Idhorario_ant;
                    }
                }
            }
        }

        public void VerificaMudancaHorario(int idfuncionario, DateTime dt, List<Modelo.MudancaHorario> pMudancaHorario, ref string leg, ref int idhorario)
        {
            if (pMudancaHorario.Count > 0)
            {
                var mud = pMudancaHorario.Where(m => m.Data >= dt && m.Idfuncionario == idfuncionario);

                if (mud.Count() > 0)
                {
                    Modelo.MudancaHorario objMudancaHorario = mud.Where(m => m.Data == mud.Min(x => x.Data)).First();
                    if (objMudancaHorario.Data == dt)
                    {
                        leg = "M";
                        LegendasConcatenadas = cwkFuncoes.ConcatenarStrings(LegendasConcatenadas, "M");
                        idhorario = objMudancaHorario.Idhorario;
                    }
                    else
                    {
                        idhorario = objMudancaHorario.Idhorario_ant;
                    }
                }
            }
        }

        #endregion

        #region Ordena Marcação

        public string RetornaCampoIndice(int indice, ref short posicao)
        {
            switch (indice)
            {
                case 0:
                    posicao = 1;
                    return "E";
                case 1:
                    posicao = 1;
                    return "S";
                case 2:
                    posicao = 2;
                    return "E";
                case 3:
                    posicao = 2;
                    return "S";
                case 4:
                    posicao = 3;
                    return "E";
                case 5:
                    posicao = 3;
                    return "S";
                case 6:
                    posicao = 4;
                    return "E";
                case 7:
                    posicao = 4;
                    return "S";
                case 8:
                    posicao = 5;
                    return "E";
                case 9:
                    posicao = 5;
                    return "S";
                case 10:
                    posicao = 6;
                    return "E";
                case 11:
                    posicao = 6;
                    return "S";
                case 12:
                    posicao = 7;
                    return "E";
                case 13:
                    posicao = 7;
                    return "S";
                case 14:
                    posicao = 8;
                    return "E";
                case 15:
                    posicao = 8;
                    return "S";
                default:
                    posicao = 0;
                    return "";
            }
        }

        public void AtribuiHorarioCampo(int indice, auxOrdenaMarcacao aux, Modelo.Marcacao objMarcacao)
        {
            switch (indice)
            {
                case 0:
                    objMarcacao.Entrada_1 = aux.horario;
                    objMarcacao.Ent_num_relogio_1 = aux.numRelogio;
                    break;
                case 1:
                    objMarcacao.Saida_1 = aux.horario;
                    objMarcacao.Sai_num_relogio_1 = aux.numRelogio;
                    break;
                case 2:
                    objMarcacao.Entrada_2 = aux.horario;
                    objMarcacao.Ent_num_relogio_2 = aux.numRelogio;
                    break;
                case 3:
                    objMarcacao.Saida_2 = aux.horario;
                    objMarcacao.Sai_num_relogio_2 = aux.numRelogio;
                    break;
                case 4:
                    objMarcacao.Entrada_3 = aux.horario;
                    objMarcacao.Ent_num_relogio_3 = aux.numRelogio;
                    break;
                case 5:
                    objMarcacao.Saida_3 = aux.horario;
                    objMarcacao.Sai_num_relogio_3 = aux.numRelogio;
                    break;
                case 6:
                    objMarcacao.Entrada_4 = aux.horario;
                    objMarcacao.Ent_num_relogio_4 = aux.numRelogio;
                    break;
                case 7:
                    objMarcacao.Saida_4 = aux.horario;
                    objMarcacao.Sai_num_relogio_4 = aux.numRelogio;
                    break;
                case 8:
                    objMarcacao.Entrada_5 = aux.horario;
                    objMarcacao.Ent_num_relogio_5 = aux.numRelogio;
                    break;
                case 9:
                    objMarcacao.Saida_5 = aux.horario;
                    objMarcacao.Sai_num_relogio_5 = aux.numRelogio;
                    break;
                case 10:
                    objMarcacao.Entrada_6 = aux.horario;
                    objMarcacao.Ent_num_relogio_6 = aux.numRelogio;
                    break;
                case 11:
                    objMarcacao.Saida_6 = aux.horario;
                    objMarcacao.Sai_num_relogio_6 = aux.numRelogio;
                    break;
                case 12:
                    objMarcacao.Entrada_7 = aux.horario;
                    objMarcacao.Ent_num_relogio_7 = aux.numRelogio;
                    break;
                case 13:
                    objMarcacao.Saida_7 = aux.horario;
                    objMarcacao.Sai_num_relogio_7 = aux.numRelogio;
                    break;
                case 14:
                    objMarcacao.Entrada_8 = aux.horario;
                    objMarcacao.Ent_num_relogio_8 = aux.numRelogio;
                    break;
                case 15:
                    objMarcacao.Saida_8 = aux.horario;
                    objMarcacao.Sai_num_relogio_8 = aux.numRelogio;
                    break;
            }
        }

        public void OrdenaTodasMarcacoes(DateTime pDataInicial, DateTime pDataFinal, int pIdFuncionario)
        {
            if (pDataInicial != new DateTime() && pDataFinal != new DateTime())
            {
                List<Modelo.Marcacao> lstMaracacao = new List<Modelo.Marcacao>();
                List<Modelo.Marcacao> lstMaracacaoNova = new List<Modelo.Marcacao>();
                List<Modelo.BilhetesImp> lstBilhetesNova = new List<Modelo.BilhetesImp>();
                Modelo.Marcacao objMarcacao;

                //Pega a lista de marcações por funcionario daquele funcionario
                lstMaracacao = this.getListaMarcacao(2, pIdFuncionario, pDataInicial, pDataFinal);

                foreach (Modelo.Marcacao marc in lstMaracacao)
                {
                    objMarcacao = new Modelo.Marcacao();

                    objMarcacao = marc;
                    objMarcacao.Acao = Modelo.Acao.Alterar;
                    this.OrdenaMarcacao(objMarcacao, true);
                    lstMaracacaoNova.Add(objMarcacao);
                    if (objMarcacao.BilhetesMarcacao.Count > 0)
                        lstBilhetesNova.AddRange(objMarcacao.BilhetesMarcacao);
                }

                dalImportaBilhetes.PersisteDados(lstMaracacaoNova, lstBilhetesNova);
                BLL.CalculaMarcacao bllCalculaMarcacao = new CalculaMarcacao(2, pIdFuncionario, pDataInicial, pDataFinal, ObjProgressBar, false, ConnectionString, UsuarioLogado, false);
                bllCalculaMarcacao.CalculaMarcacoes();
            }
        }

        /// <summary>
        /// Método que ordena as batiadas de uma marcação em ordem crescente
        /// </summary>
        /// <param name="objMarcacao">Marcação que será ordenada</param>
        /// <param name="ordenar">Indica se é para ordenar ou apenas realocar as marcações</param>
        /// WNO - 18/12/09        
        public void OrdenaMarcacao(Modelo.Marcacao objMarcacao, bool ordenar)
        {
            List<auxOrdenaMarcacao> lista = new List<auxOrdenaMarcacao>();

            auxOrdenaMarcacao marc1, marc2, marc3, marc4, marc5, marc6, marc7, marc8, marc9, marc10, marc11,
                marc12, marc13, marc14, marc15, marc16;

            marc1 = new auxOrdenaMarcacao(objMarcacao.Entrada_1, "E", 1, objMarcacao.Ent_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_1).Select(x => x.Data).FirstOrDefault());
            marc2 = new auxOrdenaMarcacao(objMarcacao.Saida_1, "S", 1, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_1).Select(x => x.Data).FirstOrDefault());
            marc3 = new auxOrdenaMarcacao(objMarcacao.Entrada_2, "E", 2, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_2).Select(x => x.Data).FirstOrDefault());
            marc4 = new auxOrdenaMarcacao(objMarcacao.Saida_2, "S", 2, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_2).Select(x => x.Data).FirstOrDefault());
            marc5 = new auxOrdenaMarcacao(objMarcacao.Entrada_3, "E", 3, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_3).Select(x => x.Data).FirstOrDefault());
            marc6 = new auxOrdenaMarcacao(objMarcacao.Saida_3, "S", 3, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_3).Select(x => x.Data).FirstOrDefault());
            marc7 = new auxOrdenaMarcacao(objMarcacao.Entrada_4, "E", 4, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_4).Select(x => x.Data).FirstOrDefault());
            marc8 = new auxOrdenaMarcacao(objMarcacao.Saida_4, "S", 4, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_4).Select(x => x.Data).FirstOrDefault());
            marc9 = new auxOrdenaMarcacao(objMarcacao.Entrada_5, "E", 5, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_5).Select(x => x.Data).FirstOrDefault());
            marc10 = new auxOrdenaMarcacao(objMarcacao.Saida_5, "S", 5, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_5).Select(x => x.Data).FirstOrDefault());
            marc11 = new auxOrdenaMarcacao(objMarcacao.Entrada_6, "E", 6, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_6).Select(x => x.Data).FirstOrDefault());
            marc12 = new auxOrdenaMarcacao(objMarcacao.Saida_6, "S", 6, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_6).Select(x => x.Data).FirstOrDefault());
            marc13 = new auxOrdenaMarcacao(objMarcacao.Entrada_7, "E", 7, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_7).Select(x => x.Data).FirstOrDefault());
            marc14 = new auxOrdenaMarcacao(objMarcacao.Saida_7, "S", 7, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_7).Select(x => x.Data).FirstOrDefault());
            marc15 = new auxOrdenaMarcacao(objMarcacao.Entrada_8, "E", 8, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Entrada_8).Select(x => x.Data).FirstOrDefault());
            marc16 = new auxOrdenaMarcacao(objMarcacao.Saida_8, "S", 8, objMarcacao.Sai_num_relogio_1, objMarcacao.BilhetesMarcacao.Where(x => x.Mar_data == objMarcacao.Data && x.Mar_hora == objMarcacao.Saida_8).Select(x => x.Data).FirstOrDefault());

            if (marc1.horario != "--:--") { lista.Add(marc1); }
            if (marc2.horario != "--:--") { lista.Add(marc2); }
            if (marc3.horario != "--:--") { lista.Add(marc3); }
            if (marc4.horario != "--:--") { lista.Add(marc4); }
            if (marc5.horario != "--:--") { lista.Add(marc5); }
            if (marc6.horario != "--:--") { lista.Add(marc6); }
            if (marc7.horario != "--:--") { lista.Add(marc7); }
            if (marc8.horario != "--:--") { lista.Add(marc8); }
            if (marc9.horario != "--:--") { lista.Add(marc9); }
            if (marc10.horario != "--:--") { lista.Add(marc10); }
            if (marc11.horario != "--:--") { lista.Add(marc11); }
            if (marc12.horario != "--:--") { lista.Add(marc12); }
            if (marc13.horario != "--:--") { lista.Add(marc13); }
            if (marc14.horario != "--:--") { lista.Add(marc14); }
            if (marc15.horario != "--:--") { lista.Add(marc15); }
            if (marc16.horario != "--:--") { lista.Add(marc16); }

            List<auxOrdenaMarcacao> removerDuplicado = new List<auxOrdenaMarcacao>();
            foreach (var item in lista.GroupBy(g => new { g.data, g.horario }).Where(W => W.Count() > 1))
            {
                auxOrdenaMarcacao rm = item.Select(s => s).OrderBy(o => o.posicao).ThenBy(o => o.ent_sai).Last();
                if (objMarcacao.BilhetesMarcacao.Where(w => w.Data == rm.data && w.Hora == rm.horario).Count() <= 1)
                {
                    removerDuplicado.Add(rm);
                }
            }

            var naoDuplicados = lista.Except(removerDuplicado);
            lista = naoDuplicados.ToList();

            if (ordenar)
            {
                if (lista.Where(w => w.data != DateTime.MinValue).Count() > 0)
                {
                    lista = lista.OrderBy(m => m.data).ThenBy(m => Modelo.cwkFuncoes.ConvertHorasMinuto(m.horario)).ToList<auxOrdenaMarcacao>();
                }
                else
                {
                    lista = lista.OrderBy(m => m.posicao).ThenBy(m => m.ent_sai).ToList<auxOrdenaMarcacao>();
                }
            }

            bool alterarTratamento = (objMarcacao.BilhetesMarcacao.Count > 0);

            List<Modelo.BilhetesImp> auxTratamento = new List<Modelo.BilhetesImp>();
            foreach (Modelo.BilhetesImp tm in objMarcacao.BilhetesMarcacao)
            {
                Modelo.BilhetesImp tratamentoM = new Modelo.BilhetesImp();
                Modelo.cwkFuncoes.CopiaPropriedades(tm, tratamentoM);
                auxTratamento.Add(tratamentoM);
            }

            for (int i = lista.Count; i < 16; i++)
            {
                auxOrdenaMarcacao item = new auxOrdenaMarcacao("--:--", "", 0, "", DateTime.MinValue);
                lista.Add(item);
            }

            for (int i = 0; i < lista.Count; i++)
            {
                AtribuiHorarioCampo(i, lista[i], objMarcacao);

                //caso haja tratamento marcação ele percorre a lista de tratamentos para alterar
                if (alterarTratamento)
                {
                    string ent_sai = "S";
                    if (i % 2 == 0)
                    {
                        ent_sai = "E";
                    }
                    int posicao = (int)Math.Ceiling((decimal)(i / 2));
                    posicao++;
                    objMarcacao.BilhetesMarcacao.Where(t => t.Data == lista[i].data && t.Hora == lista[i].horario).ToList().ForEach(f => { f.Ent_sai = ent_sai; f.Posicao = posicao; });
                }
            }
        }

        #endregion

        #region Intervalo Automático OLD

        /// <summary>
        /// Método que gera os intervalos automáticos
        /// </summary>
        /// <param name="objMarcacao">Marcação para gerar os intervalos</param>
        /// <param name="objHorario">Horário da marcação</param>
        /// WNO - 15/01/2010
        private void IntervaloAutomatico(Modelo.Marcacao objMarcacao, Modelo.Horario objHorario, int pIdFuncionario, int pIdFuncao, int pIdDepartamento, int pIdEmpresa, List<Modelo.JornadaAlternativa> pJornadasLista)
        {
            BLL.JornadaAlternativa bllJornadaAlternativa = new JornadaAlternativa(ConnectionString, UsuarioLogado);
            Modelo.JornadaAlternativa objJornadaAlternativa = bllJornadaAlternativa.PossuiRegistro(pJornadasLista, objMarcacao.Data, pIdFuncionario, pIdFuncao, pIdDepartamento, pIdEmpresa);
            bool possuiJornadaAlternativa = objJornadaAlternativa != null;

            #region Carrega o Horário detalhe
            Modelo.HorarioDetalhe objHorarioDetalhe = null;
            if (objHorario.TipoHorario == 1)
            {
                objHorarioDetalhe = objHorario.HorariosDetalhe.Where(d => d.Dia == Modelo.cwkFuncoes.Dia(objMarcacao.Data)).First();
            }
            else
            {
                if (objHorario.HorariosFlexiveis.Exists(d => d.Data == objMarcacao.Data))
                {
                    objHorarioDetalhe = objHorario.HorariosFlexiveis.Where(d => d.Data == objMarcacao.Data).First();
                }
            }
            #endregion

            #region Verifica se existe intervalo automático para aquela data

            if (objHorarioDetalhe == null)
            {
                if (!possuiJornadaAlternativa)
                {
                    return;
                }
                else
                {
                    objHorarioDetalhe = new Modelo.HorarioDetalhe();
                }
            }

            if (possuiJornadaAlternativa)
            {
                objHorarioDetalhe.Intervaloautomatico = objJornadaAlternativa.Intervaloautomatico;
                objHorarioDetalhe.Preassinaladas1 = objJornadaAlternativa.Preassinaladas1;
                objHorarioDetalhe.Preassinaladas2 = objJornadaAlternativa.Preassinaladas2;
                objHorarioDetalhe.Preassinaladas3 = objJornadaAlternativa.Preassinaladas3;
            }

            if (objHorarioDetalhe.Intervaloautomatico == 0)
            {
                return;
            }

            #endregion

            bool preencheu = false;
            int[] batidas = objMarcacao.GetMarcacoes();
            string[] numRelogio = objMarcacao.GetNumRelogio();

            string[] entradasHorario = null;
            string[] saidasHorario = null;

            if (possuiJornadaAlternativa)
            {
                entradasHorario = objJornadaAlternativa.getEntradas();
                saidasHorario = objJornadaAlternativa.getSaidas();
            }
            else
            {
                entradasHorario = objHorarioDetalhe.getEntradas();
                saidasHorario = objHorarioDetalhe.getSaidas();
            }

            string[] batidasAlteradas;
            string[] numRelogioAlterados;

            PreencheVetoresIntervaloAutomatico(objMarcacao, objHorarioDetalhe, ref preencheu, batidas, numRelogio, entradasHorario, saidasHorario, out batidasAlteradas, out numRelogioAlterados);
            //Caso tenha preenchido o intervalo automático, a marcação é alterada
            if (preencheu)
            {
                PreencheMarcacaoIntervaloAuto(objMarcacao, batidasAlteradas, numRelogioAlterados);
            }
        }

        private void PreencheVetoresIntervaloAutomatico(Modelo.Marcacao objMarcacao, Modelo.HorarioDetalhe objHorarioDetalhe, ref bool preencheu, int[] batidas, string[] numRelogio, string[] entradasHorario, string[] saidasHorario, out string[] batidasAlteradas, out string[] numRelogioAlterados)
        {
            batidasAlteradas = new string[16];
            numRelogioAlterados = new string[16];

            if (objHorarioDetalhe.Preassinaladas1 == 1 && objHorarioDetalhe.Preassinaladas2 == 1 && objHorarioDetalhe.Preassinaladas3 == 1)
            {
                #region Três intervalos pré assinalados
                //as batidas se encaixam no padrão para inserir os intervalos automáticos
                if (((objHorarioDetalhe.EntradaMin_4 > objHorarioDetalhe.EntradaMin_3
                    && objHorarioDetalhe.EntradaMin_4 > objHorarioDetalhe.EntradaMin_2
                    && batidas[1] > objHorarioDetalhe.EntradaMin_4)
                    ||
                    (objHorarioDetalhe.EntradaMin_4 < objHorarioDetalhe.EntradaMin_1
                    && objHorarioDetalhe.EntradaMin_4 < objHorarioDetalhe.SaidaMin_1
                    && batidas[1] > objHorarioDetalhe.EntradaMin_4
                    && batidas[1] < objHorarioDetalhe.EntradaMin_1))
                    && entradasHorario[3] != "--:--"
                    && batidas.Where(b => b != -1).Count() <= 9)
                {
                    preencheu = true;
                    string NR = "PA";
                    batidasAlteradas[0] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[0]);
                    numRelogioAlterados[0] = numRelogio[0];

                    batidasAlteradas[1] = saidasHorario[0];
                    numRelogioAlterados[1] = NR;

                    batidasAlteradas[2] = entradasHorario[1];
                    numRelogioAlterados[2] = NR;

                    batidasAlteradas[3] = saidasHorario[1];
                    numRelogioAlterados[3] = NR;

                    batidasAlteradas[4] = entradasHorario[2];
                    numRelogioAlterados[4] = NR;

                    batidasAlteradas[5] = saidasHorario[2];
                    numRelogioAlterados[5] = NR;

                    batidasAlteradas[6] = entradasHorario[3];
                    numRelogioAlterados[6] = NR;

                    int count = 7;
                    for (int i = 1; i < 10; i++, count++)
                    {
                        batidasAlteradas[count] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[i]);
                        numRelogioAlterados[count] = numRelogio[i];
                    }

                    AlteraTratamentoIntAuto(objMarcacao, 1, 5, 3);

                    InsereTratamentoPA(objMarcacao, 1, 4, batidasAlteradas);
                }
                #endregion
            }
            else if (objHorarioDetalhe.Preassinaladas1 == 1 && objHorarioDetalhe.Preassinaladas2 == 1)
            {
                #region Dois primeiros intervalos pré assinalados

                if (((objHorarioDetalhe.SaidaMin_1 < objHorarioDetalhe.SaidaMin_2
                    && objHorarioDetalhe.SaidaMin_1 < objHorarioDetalhe.EntradaMin_1
                    && batidas[1] > objHorarioDetalhe.EntradaMin_3
                    && (batidas[0] < objHorarioDetalhe.SaidaMin_1 || batidas[0] > objHorarioDetalhe.SaidaMin_3))
                    ||
                    (objHorarioDetalhe.SaidaMin_1 < objHorarioDetalhe.SaidaMin_2
                    && objHorarioDetalhe.SaidaMin_1 > objHorarioDetalhe.EntradaMin_3
                    && batidas[1] > objHorarioDetalhe.EntradaMin_3
                    && batidas[1] > objHorarioDetalhe.EntradaMin_2)
                    ||
                    (objHorarioDetalhe.SaidaMin_1 > objHorarioDetalhe.EntradaMin_1
                    && objHorarioDetalhe.SaidaMin_1 > objHorarioDetalhe.SaidaMin_3
                    && batidas[0] < objHorarioDetalhe.SaidaMin_1
                    && batidas[1] < batidas[0]
                    && batidas[1] > objHorarioDetalhe.EntradaMin_3)
                    ||
                    (objHorarioDetalhe.SaidaMin_1 > objHorarioDetalhe.EntradaMin_1
                    && objHorarioDetalhe.SaidaMin_1 < objHorarioDetalhe.SaidaMin_3
                    && batidas[1] > objHorarioDetalhe.EntradaMin_1
                    && batidas[1] > objHorarioDetalhe.EntradaMin_3
                    && batidas[0] < batidas[1]
                    && batidas[0] < objHorarioDetalhe.SaidaMin_1))
                    && entradasHorario[2] != "--:--"
                    && batidas.Where(b => b != -1).Count() <= 12)
                {
                    if (batidas[0] < batidas[1]
                       &&
                       ((objHorarioDetalhe.SaidaMin_1 > batidas[0] && objHorarioDetalhe.SaidaMin_1 > batidas[1])
                       ||
                       (objHorarioDetalhe.SaidaMin_1 < batidas[0] && objHorarioDetalhe.SaidaMin_1 < batidas[1])))
                    {
                        return;
                    }

                    preencheu = true;
                    string NR = "PA";
                    batidasAlteradas[0] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[0]);
                    numRelogioAlterados[0] = numRelogio[0];

                    batidasAlteradas[1] = saidasHorario[0];
                    numRelogioAlterados[1] = NR;

                    batidasAlteradas[2] = entradasHorario[1];
                    numRelogioAlterados[2] = NR;

                    batidasAlteradas[3] = saidasHorario[1];
                    numRelogioAlterados[3] = NR;

                    batidasAlteradas[4] = entradasHorario[2];
                    numRelogioAlterados[4] = NR;

                    int count = 5;
                    for (int i = 1; i < 12; i++, count++)
                    {
                        batidasAlteradas[count] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[i]);
                        numRelogioAlterados[count] = numRelogio[i];
                    }

                    AlteraTratamentoIntAuto(objMarcacao, 1, 6, 2);
                    InsereTratamentoPA(objMarcacao, 1, 3, batidasAlteradas);
                }
                #endregion
            }
            else if (objHorarioDetalhe.Preassinaladas1 == 1 && objHorarioDetalhe.Preassinaladas3 == 1)
            {
                #region Primeiro e terceiro intervalos pré assinalados

                if (((objHorarioDetalhe.SaidaMin_4 > objHorarioDetalhe.SaidaMin_3
                    && objHorarioDetalhe.SaidaMin_2 > objHorarioDetalhe.SaidaMin_1
                    && batidas[1] > objHorarioDetalhe.EntradaMin_2
                    && batidas[1] < objHorarioDetalhe.SaidaMin_3
                    && batidas[2] > objHorarioDetalhe.EntradaMin_2
                    && batidas[2] < objHorarioDetalhe.SaidaMin_3
                    && batidas[3] > objHorarioDetalhe.EntradaMin_4)
                    ||
                    (objHorarioDetalhe.SaidaMin_2 < objHorarioDetalhe.SaidaMin_1
                    && objHorarioDetalhe.SaidaMin_4 > objHorarioDetalhe.SaidaMin_3
                    && batidas[1] < objHorarioDetalhe.EntradaMin_1
                    && batidas[1] < objHorarioDetalhe.SaidaMin_3
                    && batidas[3] > objHorarioDetalhe.EntradaMin_4))
                    && batidas.Where(b => b != -1).Count() <= 12)
                {
                    preencheu = true;
                    string NR = "PA";
                    batidasAlteradas[0] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[0]);
                    numRelogioAlterados[0] = numRelogio[0];

                    batidasAlteradas[1] = saidasHorario[0];
                    numRelogioAlterados[1] = NR;

                    batidasAlteradas[2] = entradasHorario[1];
                    numRelogioAlterados[2] = NR;

                    batidasAlteradas[3] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[1]);
                    numRelogioAlterados[3] = numRelogio[1];

                    batidasAlteradas[4] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[2]);
                    numRelogioAlterados[4] = numRelogio[2];

                    batidasAlteradas[5] = saidasHorario[2];
                    numRelogioAlterados[5] = NR;

                    batidasAlteradas[6] = entradasHorario[3];
                    numRelogioAlterados[6] = NR;

                    int count = 7;
                    for (int i = 3; i < 12; i++, count++)
                    {
                        batidasAlteradas[count] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[i]);
                        numRelogioAlterados[count] = numRelogio[i];
                    }

                    AlteraTratamentoIntAuto(objMarcacao, 1, 3, 1);
                    AlteraTratamentoIntAuto(objMarcacao, 3, 7, 1);
                    InsereTratamentoPA(objMarcacao, 1, 2, batidasAlteradas);
                    InsereTratamentoPA(objMarcacao, 3, 4, batidasAlteradas);
                }

                #endregion
            }
            else if (objHorarioDetalhe.Preassinaladas2 == 1 && objHorarioDetalhe.Preassinaladas3 == 1)
            {
                #region Segundo e terceiro intervalos pré assinalados

                if (((objHorarioDetalhe.EntradaMin_4 > objHorarioDetalhe.SaidaMin_2
                    && batidas[3] > objHorarioDetalhe.EntradaMin_4)
                    ||
                    (objHorarioDetalhe.EntradaMin_4 < objHorarioDetalhe.SaidaMin_2
                    && batidas[3] > objHorarioDetalhe.EntradaMin_4
                    && batidas[3] < objHorarioDetalhe.SaidaMin_2))
                    && objHorarioDetalhe.SaidaMin_4 != -1
                    && batidas.Skip(3).Where(b => b != -1).Count() <= 9)
                {
                    preencheu = true;
                    string NR = "PA";
                    batidasAlteradas[0] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[0]);
                    numRelogioAlterados[0] = numRelogio[0];

                    batidasAlteradas[1] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[1]);
                    numRelogioAlterados[1] = numRelogio[1];

                    batidasAlteradas[2] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[2]);
                    numRelogioAlterados[2] = numRelogio[2];

                    batidasAlteradas[3] = saidasHorario[1];
                    numRelogioAlterados[3] = NR;

                    batidasAlteradas[4] = entradasHorario[2];
                    numRelogioAlterados[4] = NR;

                    batidasAlteradas[5] = saidasHorario[2];
                    numRelogioAlterados[5] = NR;

                    batidasAlteradas[6] = entradasHorario[3];
                    numRelogioAlterados[6] = NR;

                    int count = 7;
                    for (int i = 3; i < 12; i++, count++)
                    {
                        batidasAlteradas[count] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[i]);
                        numRelogioAlterados[count] = numRelogio[i];
                    }

                    AlteraTratamentoIntAuto(objMarcacao, 2, 6, 2);
                    InsereTratamentoPA(objMarcacao, 2, 4, batidasAlteradas);
                }

                #endregion
            }
            else if (objHorarioDetalhe.Preassinaladas1 == 1)
            {
                #region Primeiro intervalo pré assinalados

                if (((objHorarioDetalhe.SaidaMin_1 < objHorarioDetalhe.EntradaMin_2
                    && objHorarioDetalhe.SaidaMin_1 > objHorarioDetalhe.EntradaMin_1
                    && batidas[0] < objHorarioDetalhe.SaidaMin_1
                    && batidas[1] > objHorarioDetalhe.EntradaMin_2)
                    ||
                    (objHorarioDetalhe.SaidaMin_1 < objHorarioDetalhe.EntradaMin_2
                    && objHorarioDetalhe.SaidaMin_1 < objHorarioDetalhe.EntradaMin_1
                    && (batidas[0] > objHorarioDetalhe.SaidaMin_2 || batidas[0] < objHorarioDetalhe.SaidaMin_1)
                    && batidas[1] > objHorarioDetalhe.EntradaMin_2)
                    ||
                    (objHorarioDetalhe.SaidaMin_1 > objHorarioDetalhe.EntradaMin_2
                    && batidas[1] > objHorarioDetalhe.EntradaMin_2 && batidas[1] < objHorarioDetalhe.SaidaMin_1))
                    && entradasHorario[1] != "--:--"
                    && batidas.Where(b => b != -1).Count() <= 12)
                {
                    if (batidas[0] < batidas[1]
                        &&
                        ((objHorarioDetalhe.SaidaMin_1 > batidas[0] && objHorarioDetalhe.SaidaMin_1 > batidas[1])
                        ||
                        (objHorarioDetalhe.SaidaMin_1 < batidas[0] && objHorarioDetalhe.SaidaMin_1 < batidas[1])))
                    {
                        return;
                    }

                    preencheu = true;
                    string NR = "PA";
                    batidasAlteradas[0] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[0]);
                    numRelogioAlterados[0] = numRelogio[0];

                    batidasAlteradas[1] = saidasHorario[0];
                    numRelogioAlterados[1] = NR;

                    batidasAlteradas[2] = entradasHorario[1];
                    numRelogioAlterados[2] = NR;

                    int count = 3;
                    for (int i = 1; i < 14; i++, count++)
                    {
                        batidasAlteradas[count] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[i]);
                        numRelogioAlterados[count] = numRelogio[i];
                    }

                    AlteraTratamentoIntAuto(objMarcacao, 1, 7, 1);
                    InsereTratamentoPA(objMarcacao, 1, 2, batidasAlteradas);
                }
                #endregion
            }
            else if (objHorarioDetalhe.Preassinaladas2 == 1)
            {
                #region Segundo intervalo pré assinalados

                if (((objHorarioDetalhe.SaidaMin_2 < objHorarioDetalhe.EntradaMin_3
                    && batidas[3] > objHorarioDetalhe.EntradaMin_3)
                    ||
                    (objHorarioDetalhe.SaidaMin_2 > objHorarioDetalhe.EntradaMin_3
                    && batidas[3] > objHorarioDetalhe.EntradaMin_3 && batidas[1] < objHorarioDetalhe.SaidaMin_2))
                    && entradasHorario[2] != "--:--"
                    && batidas.Skip(3).Where(b => b != -1).Count() <= 11)
                {
                    preencheu = true;
                    string NR = "PA";
                    batidasAlteradas[0] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[0]);
                    numRelogioAlterados[0] = numRelogio[0];

                    batidasAlteradas[1] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[1]);
                    numRelogioAlterados[1] = numRelogio[1];

                    batidasAlteradas[2] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[2]);
                    numRelogioAlterados[2] = numRelogio[2];

                    batidasAlteradas[3] = saidasHorario[1];
                    numRelogioAlterados[3] = NR;

                    batidasAlteradas[4] = entradasHorario[2];
                    numRelogioAlterados[4] = NR;

                    int count = 5;
                    for (int i = 3; i < 14; i++, count++)
                    {
                        batidasAlteradas[count] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[i]);
                        numRelogioAlterados[count] = numRelogio[i];
                    }

                    AlteraTratamentoIntAuto(objMarcacao, 2, 7, 1);
                    InsereTratamentoPA(objMarcacao, 2, 3, batidasAlteradas);
                }

                #endregion
            }
            else if (objHorarioDetalhe.Preassinaladas3 == 1)
            {
                #region Terceiro intervalo pré assinalados

                if (((objHorarioDetalhe.SaidaMin_3 < objHorarioDetalhe.EntradaMin_4
                    && batidas[5] > objHorarioDetalhe.EntradaMin_4
                    && batidas[4] < objHorarioDetalhe.SaidaMin_3)
                    ||
                    (objHorarioDetalhe.SaidaMin_3 > objHorarioDetalhe.EntradaMin_4
                    && batidas[5] > objHorarioDetalhe.EntradaMin_4
                    && batidas[5] < objHorarioDetalhe.SaidaMin_3))
                    && entradasHorario[3] != "--:--"
                    && batidas.Skip(5).Where(b => b != -1).Count() <= 9)
                {
                    preencheu = true;
                    string NR = "PA";
                    batidasAlteradas[0] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[0]);
                    numRelogioAlterados[0] = numRelogio[0];

                    batidasAlteradas[1] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[1]);
                    numRelogioAlterados[1] = numRelogio[1];

                    batidasAlteradas[2] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[2]);
                    numRelogioAlterados[2] = numRelogio[2];

                    batidasAlteradas[3] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[3]);
                    numRelogioAlterados[3] = numRelogio[3];

                    batidasAlteradas[4] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[4]);
                    numRelogioAlterados[4] = numRelogio[4];

                    batidasAlteradas[5] = saidasHorario[2];
                    numRelogioAlterados[5] = NR;

                    batidasAlteradas[6] = entradasHorario[3];
                    numRelogioAlterados[6] = NR;

                    int count = 7;
                    for (int i = 5; i < 14; i++, count++)
                    {
                        batidasAlteradas[count] = Modelo.cwkFuncoes.ConvertMinutosBatida(batidas[i]);
                        numRelogioAlterados[count] = numRelogio[i];
                    }

                    AlteraTratamentoIntAuto(objMarcacao, 3, 7, 1);
                    InsereTratamentoPA(objMarcacao, 3, 4, batidasAlteradas);
                }

                #endregion
            }
        }

        private void PreencheMarcacaoIntervaloAuto(Modelo.Marcacao objMarcacao, string[] batidasAlteradas, string[] numRelogioAlterados)
        {
            objMarcacao.Entrada_1 = batidasAlteradas[0];
            objMarcacao.Ent_num_relogio_1 = numRelogioAlterados[0];
            List<Modelo.BilhetesImp> aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "E" && b.Posicao == 1).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Entrada_1, objMarcacao.Ent_num_relogio_1);

            objMarcacao.Saida_1 = batidasAlteradas[1];
            objMarcacao.Sai_num_relogio_1 = numRelogioAlterados[1];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "S" && b.Posicao == 1).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Saida_1, objMarcacao.Sai_num_relogio_1);

            objMarcacao.Entrada_2 = batidasAlteradas[2];
            objMarcacao.Ent_num_relogio_2 = numRelogioAlterados[2];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "E" && b.Posicao == 2).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Entrada_2, objMarcacao.Ent_num_relogio_2);

            objMarcacao.Saida_2 = batidasAlteradas[3];
            objMarcacao.Sai_num_relogio_2 = numRelogioAlterados[3];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "S" && b.Posicao == 2).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Saida_2, objMarcacao.Sai_num_relogio_2);

            objMarcacao.Entrada_3 = batidasAlteradas[4];
            objMarcacao.Ent_num_relogio_3 = numRelogioAlterados[4];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "E" && b.Posicao == 3).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Entrada_3, objMarcacao.Ent_num_relogio_3);

            objMarcacao.Saida_3 = batidasAlteradas[5];
            objMarcacao.Sai_num_relogio_3 = numRelogioAlterados[5];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "S" && b.Posicao == 3).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Saida_3, objMarcacao.Sai_num_relogio_3);

            objMarcacao.Entrada_4 = batidasAlteradas[6];
            objMarcacao.Ent_num_relogio_4 = numRelogioAlterados[6];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "E" && b.Posicao == 4).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Entrada_4, objMarcacao.Ent_num_relogio_4);

            objMarcacao.Saida_4 = batidasAlteradas[7];
            objMarcacao.Sai_num_relogio_4 = numRelogioAlterados[7];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "S" && b.Posicao == 4).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Saida_4, objMarcacao.Sai_num_relogio_4);

            objMarcacao.Entrada_5 = batidasAlteradas[8];
            objMarcacao.Ent_num_relogio_5 = numRelogioAlterados[8];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "E" && b.Posicao == 5).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Entrada_5, objMarcacao.Ent_num_relogio_5);

            objMarcacao.Saida_5 = batidasAlteradas[9];
            objMarcacao.Sai_num_relogio_5 = numRelogioAlterados[9];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "S" && b.Posicao == 5).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Saida_5, objMarcacao.Sai_num_relogio_5);

            objMarcacao.Entrada_6 = batidasAlteradas[10];
            objMarcacao.Ent_num_relogio_6 = numRelogioAlterados[10];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "E" && b.Posicao == 6).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Entrada_6, objMarcacao.Ent_num_relogio_6);

            objMarcacao.Saida_6 = batidasAlteradas[11];
            objMarcacao.Sai_num_relogio_6 = numRelogioAlterados[11];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "S" && b.Posicao == 6).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Saida_6, objMarcacao.Sai_num_relogio_6);

            objMarcacao.Entrada_7 = batidasAlteradas[12];
            objMarcacao.Ent_num_relogio_7 = numRelogioAlterados[12];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "E" && b.Posicao == 7).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Entrada_7, objMarcacao.Ent_num_relogio_7);

            objMarcacao.Saida_7 = batidasAlteradas[13];
            objMarcacao.Sai_num_relogio_7 = numRelogioAlterados[13];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "S" && b.Posicao == 7).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Saida_7, objMarcacao.Sai_num_relogio_7);

            objMarcacao.Entrada_8 = batidasAlteradas[14];
            objMarcacao.Ent_num_relogio_8 = numRelogioAlterados[14];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "E" && b.Posicao == 8).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Entrada_8, objMarcacao.Ent_num_relogio_8);

            objMarcacao.Saida_8 = batidasAlteradas[15];
            objMarcacao.Sai_num_relogio_8 = numRelogioAlterados[15];
            aux = objMarcacao.BilhetesMarcacao.Where(b => b.Ent_sai == "S" && b.Posicao == 8).ToList();
            AuxPMarcIntervaloAuto(aux, objMarcacao.Saida_8, objMarcacao.Sai_num_relogio_8);

        }

        private void AuxPMarcIntervaloAuto(List<Modelo.BilhetesImp> aux, string batida, string relogio)
        {
            if (aux.Count() > 0)
            {
                Modelo.BilhetesImp objBilhete = aux.First();
                objBilhete.Hora = batida;
                objBilhete.Mar_hora = batida;
                objBilhete.Relogio = relogio;
                objBilhete.Mar_relogio = relogio;
            }
        }

        private void InsereTratamentoPA(Modelo.Marcacao objMarcacao, short inicio, short fim, string[] batidas)
        {
            Modelo.BilhetesImp objTratamento = null;
            for (short i = inicio; i <= fim; i++)
            {
                if (i > inicio)
                {
                    objTratamento = new Modelo.BilhetesImp();
                    objTratamento.Acao = Modelo.Acao.Incluir;
                    objTratamento.Motivo = "Marcação Pré-Assinalada.";
                    objTratamento.Ocorrencia = 'P';
                    objTratamento.Posicao = i;
                    objTratamento.Ent_sai = "E";
                    objTratamento.Relogio = "PA";
                    objTratamento.Mar_relogio = "PA";
                    //{VER}
                    objTratamento.Data = objMarcacao.Data;
                    objTratamento.Mar_data = objMarcacao.Data;
                    objTratamento.Func = objMarcacao.Dscodigo;
                    objTratamento.DsCodigo = objMarcacao.Dscodigo;
                    objMarcacao.BilhetesMarcacao.Add(objTratamento);
                }

                if (i < fim)
                {
                    objTratamento = new Modelo.BilhetesImp();
                    objTratamento.Acao = Modelo.Acao.Incluir;
                    objTratamento.Motivo = "Marcação Pré-Assinalada.";
                    objTratamento.Ocorrencia = 'P';
                    objTratamento.Posicao = i;
                    objTratamento.Ent_sai = "S";
                    objTratamento.Relogio = "PA";
                    objTratamento.Mar_relogio = "PA";
                    //{VER}
                    objTratamento.Data = objMarcacao.Data;
                    objTratamento.Mar_data = objMarcacao.Data;
                    objTratamento.Func = objMarcacao.Dscodigo;
                    objTratamento.DsCodigo = objMarcacao.Dscodigo;
                    objMarcacao.BilhetesMarcacao.Add(objTratamento);
                }
            }
        }

        /// <summary>
        /// Altera um determinado tratamento de uma marcação ao realizar intervalo automático
        /// </summary>
        /// <param name="objMarcacao">Marcação a ser alterada</param>
        /// <param name="indicadorAnt">indicador anterior do tratamento</param>
        /// <param name="indicador">novo indicador do tratamento</param>
        /// <param name="sequencia">nova sequencia do tratamento</param>
        /// WNO - 16/01/2010
        public void AlteraTratamentoIntAuto(Modelo.Marcacao objMarcacao, short inicio, short limite, short deslocamento)
        {
            List<Modelo.BilhetesImp> tratamentos = new List<Modelo.BilhetesImp>();
            for (short i = 1; i <= inicio; i++)
            {
                AuxAlteraTratamento(tratamentos, objMarcacao, "E", i, "E", i);

                if (i < inicio)
                {
                    AuxAlteraTratamento(tratamentos, objMarcacao, "S", i, "S", i);
                }
            }

            for (short i = inicio; i < limite; i++)
            {
                if (i > inicio)
                {
                    AuxAlteraTratamento(tratamentos, objMarcacao, "E", (i + deslocamento), "E", (i + deslocamento));
                }

                AuxAlteraTratamento(tratamentos, objMarcacao, "S", (i + deslocamento), "S", (i + deslocamento));
            }
            objMarcacao.BilhetesMarcacao = tratamentos;
        }

        public void AuxAlteraTratamento(List<Modelo.BilhetesImp> tratamentos, Modelo.Marcacao objMarcacao, string ent_saiAnt, int posicaoAnt, string ent_sai, int posicao)
        {
            var aux = objMarcacao.BilhetesMarcacao.Where(tm => tm.Ent_sai == ent_saiAnt && tm.Posicao == posicaoAnt);
            if (aux.Count() > 0)
            {
                Modelo.BilhetesImp objTratamentoMarcacaoAnt = aux.First();
                Modelo.BilhetesImp objTratamentoMarcacao = new Modelo.BilhetesImp();
                Modelo.cwkFuncoes.CopiaPropriedades(objTratamentoMarcacaoAnt, objTratamentoMarcacao);
                objTratamentoMarcacao.Ent_sai = ent_sai;
                objTratamentoMarcacao.Posicao = posicao;
                if (objTratamentoMarcacao.Id > 0)
                {
                    objTratamentoMarcacao.Acao = Modelo.Acao.Alterar;
                }
                else
                {
                    objTratamentoMarcacao.Acao = Modelo.Acao.Incluir;
                }
                tratamentos.Add(objTratamentoMarcacao);
            }
        }

        #endregion

        private Modelo.Afastamento LocalizaAfastamento(List<Modelo.Afastamento> pAfastamentoList, int pIdFuncionario, int pIdDepartamento, int pIdEmpresa, DateTime pData)
        {
            if (pAfastamentoList.Count > 0)
            {
                //Existe um afastamento para aquela data?
                if (pAfastamentoList.Exists(a => (a.Datai <= pData) && ((a.Dataf == null ? DateTime.MaxValue : a.Dataf) >= pData) && ((a.Tipo == 0 && a.IdFuncionario == pIdFuncionario) || (a.Tipo == 1 && a.IdDepartamento == pIdDepartamento) || (a.Tipo == 2 && a.IdEmpresa == pIdEmpresa))))
                {
                    return pAfastamentoList.Find(a => (a.Datai <= pData) && ((a.Dataf == null ? DateTime.MaxValue : a.Dataf) >= pData) && ((a.Tipo == 0 && a.IdFuncionario == pIdFuncionario) || (a.Tipo == 1 && a.IdDepartamento == pIdDepartamento) || (a.Tipo == 2 && a.IdEmpresa == pIdEmpresa)));
                }
            }

            return null;
        }

        public List<Modelo.Marcacao> GetListaCompesacao(List<DateTime> datas, int tipo, int identificacao)
        {
            return dalMarcacao.GetListaCompesacao(datas, tipo, identificacao);
        }

        public Modelo.GridMarcacoes GerarGridMarcacoes(List<Modelo.MarcacaoLista> marcs)
        {
            Modelo.GridMarcacoes marc = new Modelo.GridMarcacoes();
            marc.Marcacoes = marcs;
            TimeSpan parse;
            List<decimal> percentuaisDentro = (from marcacao in marcs
                                               where TimeSpan.TryParse(marcacao.InItinereHrsDentroJornada, out parse)
                                                     && parse.TotalMilliseconds > 0
                                               select marcacao.InItinerePercDentroJornada).Distinct().ToList();
            List<decimal> percentuaisFora = (from marcacao in marcs
                                             where TimeSpan.TryParse(marcacao.InItinereHrsForaJornada, out parse)
                                                     && parse.TotalMilliseconds > 0
                                             select marcacao.InItinerePercForaJornada).Distinct().ToList();
            marc.Percentuais = percentuaisDentro.Union(percentuaisFora).Distinct().OrderBy(x => x).ToList();
            foreach (Modelo.MarcacaoLista marcacao in marcs)
            {
                if (marcacao.AdicionalNoturno == "")
                {
                    marcacao.AdicionalNoturno = "--:--";
                }
                marcacao.ValoresInItinere = new string[marc.Percentuais.Count];
                for (int i = 0; i < marc.Percentuais.Count; i++)
                {
                    TimeSpan tempoAcumulado = new TimeSpan();
                    if (TimeSpan.TryParse(marcacao.InItinereHrsDentroJornada, out parse)
                        && parse.TotalMilliseconds > 0
                        && marcacao.InItinerePercDentroJornada == marc.Percentuais[i])
                    {
                        tempoAcumulado = tempoAcumulado.Add(parse);
                    }
                    if (TimeSpan.TryParse(marcacao.InItinereHrsForaJornada, out parse)
                        && parse.TotalMilliseconds > 0
                        && marcacao.InItinerePercForaJornada == marc.Percentuais[i])
                    {
                        tempoAcumulado = tempoAcumulado.Add(parse);
                    }

                    if (tempoAcumulado.TotalMilliseconds > 0)
                        marcacao.ValoresInItinere[i] = tempoAcumulado.ToString(@"hh\:mm");
                    else
                        marcacao.ValoresInItinere[i] = "--:--";
                }
            }
            return marc;
        }

        public void BloqLibEdicaoPnlRh(DateTime dataInicio, DateTime dataFim, int idFunc, string tipoSolicitacao)
        {
            dalMarcacao.AtualizarDataLoginBloqueioEdicaoPnlRh(dataInicio, dataFim, idFunc, tipoSolicitacao);
        }

        public DateTime? GetLastDateMarcacao(int idFunc)
        {
            return dalMarcacao.GetLastDateMarcacao(idFunc);
        }

        public int GetIdDocumentoWorkflow(int idMarcacao)
        {
            return dalMarcacao.GetIdDocumentoWorkflow(idMarcacao);
        }

        public void IncluiUsrDtaConclusaoFluxoPnlRh(int idMarcacao, DateTime dataConclusao, string usrLogin)
        {
            dalMarcacao.IncluiUsrDtaConclusaoFluxoPnlRh(idMarcacao, dataConclusao, usrLogin);
        }

        public DataTable ConclusoesBloqueioPnlRh(string idsFuncionarios, DateTime dataInicial, DateTime dataFinal, int tipoFiltro)
        {
            return dalMarcacao.ConclusoesBloqueioPnlRh(idsFuncionarios, dataInicial, dataFinal, tipoFiltro);
        }

        #region Métodos de updates pontuais na marcação
        public void ManipulaDocumentoWorkFlowPnlRH(int idMarcacao, int idDocumentoWorkflow, bool documentoWorkflowAberto)
        {
            dalMarcacao.ManipulaDocumentoWorkFlowPnlRH(idMarcacao, idDocumentoWorkflow, documentoWorkflowAberto);
        }
        #endregion

        public List<Modelo.Marcacao> GetPorFuncionariosContratosAtivos(List<int> ids, DateTime pdataInicial, DateTime pDataFinal, bool PegaInativos)
        {
            return dalMarcacao.GetPorFuncionariosContratosAtivos(ids, pdataInicial, pDataFinal, PegaInativos);
        }

        public static List<Modelo.Marcacao> VincularBilhetesMarcacao(List<Modelo.Marcacao> marcacoes, List<Modelo.BilhetesImp> bilhetes)
        {
            #region Lógica para melhor desempenho
            // Separa marcacoes com seus bilhetes
            var qMarcBilhetes = from marc in marcacoes
                                join bil in bilhetes
                                on new { idFunc = marc.Idfuncionario, data = marc.Data }
                                        equals new { idFunc = bil.IdFuncionario, data = bil.Mar_data.GetValueOrDefault() }
                                select new { Marcacao = marc, Bilhete = bil };

            // Gera novos objetos de marcacao com os bilhetes relacionados
            var marcBilhetes = from m in qMarcBilhetes
                               group m by new { m.Marcacao.Idfuncionario, m.Marcacao.Data } into g
                               select new Modelo.Marcacao().Clone(g.FirstOrDefault().Marcacao, g.Select(s => s.Bilhete).ToList());

            List<Modelo.Marcacao> MarcsComBilhetes = marcBilhetes.ToList();

            List<int> idsMarcsNaoAdd = MarcsComBilhetes.Select(s => s.Id).ToList();
            //Retorna as marcações que não possuem bilhetes
            List<Modelo.Marcacao> MarcsN = marcacoes.Where(w => !idsMarcsNaoAdd.Contains(w.Id)).ToList();
            MarcsComBilhetes.AddRange(MarcsN);
            marcacoes = MarcsComBilhetes;
            #endregion
            return marcacoes;
        }

        public List<Modelo.Marcacao> GetPorFuncionariosContratosAtivosComBilhetes(List<int> idsFunc, DateTime pdataInicial, DateTime pDataFinal)
        {
            DAL.SQL.BilhetesImp dalBilhetesImp = new DAL.SQL.BilhetesImp(new DataBase(ConnectionString));
            dalBilhetesImp.UsuarioLogado = UsuarioLogado;

            List<Task> taskList = new List<Task>();
            Task<List<Modelo.Marcacao>> tMarcacao = Task.Run(() =>
                dalMarcacao.GetPorFuncionariosContratosAtivos(idsFunc, pdataInicial, pDataFinal, true));
            taskList.Add(tMarcacao);
            Task<List<Modelo.BilhetesImp>> tBilhetes = Task.Run(() =>
                    dalBilhetesImp.GetImportadosPeriodo(idsFunc, pdataInicial, pDataFinal, true));
            taskList.Add(tBilhetes);

            Task.WaitAll(taskList.ToArray());
            return VincularBilhetesMarcacao(tMarcacao.Result, tBilhetes.Result);
        }
    }

    public struct MarcPA
    {
        public int marcacao;
        public string numRelogio;
        public Modelo.Tratamentomarcacao tratamento;

        public string GetMarcacaoStr {
            get {
                return Modelo.cwkFuncoes.ConvertMinutosBatida(marcacao);
            }
        }
    }

    
}