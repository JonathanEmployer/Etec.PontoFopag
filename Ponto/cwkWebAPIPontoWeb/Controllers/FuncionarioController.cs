using CentralCliente;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LModel = cwkWebAPIPontoWeb.Models;

namespace cwkWebAPIPontoWeb.Controllers
{
    /// <summary>
    /// Controlador para Incluir e Alterar funcionário
    /// </summary>
    [Authorize]
    public class FuncionarioController : ExtendedApiController
    {
        /// <summary>
        /// Cadastrar/Alterar Funcionário.
        /// </summary>
        /// <param name="funcionario">Json com os dados do Funcionário</param>
        /// <returns> Retorna json de função quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(LModel.Funcionario funcionario)
        {
            var jsonRequisicao = GetRawPostData();
            RetornoErro retErro = new RetornoErro();
            Usuario usu = new Usuario();
            funcionario.CPF = Utils.MetodosAuxiliares.FormatarCPF(funcionario.CPF);

            if (ModelState.IsValid)
            {
                try
                {
                    bool erro = false;
                    Modelo.Empresa emp;
                    int? IdDep, IdFuncao, idHorario;

                    BLL.Contrato bllContrato = new BLL.Contrato(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    int contid = bllContrato.GetIdPorIdIntegracao(funcionario.IdintegracaoContrato.GetValueOrDefault()).GetValueOrDefault();
                    Modelo.Contrato cont = bllContrato.LoadObject(contid);


                    erro = ValidaDados(funcionario, retErro, usuarioPontoWeb.ConnectionString, out emp, out IdDep, out IdFuncao, funcionario.DescricaoFuncao, cont);

                    if (!erro)
                    {
                        BLL.Pessoa bllPessoa = new BLL.Pessoa(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                        BLL.Funcionario bllFuncionario = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                        BLL.TipoVinculo bllTipoVinculo = new BLL.TipoVinculo(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                        BLL.Parametros bllParametros = new BLL.Parametros(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                        BLL.Departamento bllDepartamento = new BLL.Departamento(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);

                        BLL.Empresa bllEmpresa = new BLL.Empresa(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                        Modelo.Parametros parametro = bllParametros.LoadPrimeiro();


                        Modelo.Funcionario DadosAntFunc = new Modelo.Funcionario();
                        int? idfuncionario = bllFuncionario.GetIdporIdIntegracao(funcionario.IdIntegracao);
                        if (idfuncionario.GetValueOrDefault() != 0)
                        {
                            DadosAntFunc = bllFuncionario.LoadObject(idfuncionario.GetValueOrDefault());
                        }
                        else
                        {
                            DadosAntFunc = bllFuncionario.LoadPorCPF(funcionario.CPF);
                            // Lógica para atender os casos de dados que foram incluídos manualmente no ponto e agora a folha esta tentando integrar via integração.
                            // Nesses casos o idintegração do ponto esta 0 e a folha vai mandar com um idIntegracao, então vejo se o funcionário enviado pela folha bate a matricula, cpf e empresa, caso positivo apenas atualiza o cadastro incluindo o idIntegracao
                            if (funcionario.IdIntegracao > 0 && (DadosAntFunc == null || DadosAntFunc.Matricula != funcionario.Matricula || DadosAntFunc.Idempresa != emp.Id))
                            {
                                DadosAntFunc = new Modelo.Funcionario();
                            }
                        }

                        DadosAntFunc.Nome = funcionario.Nome;
                        DadosAntFunc.Codigofolha = funcionario.Codigofolha;
                        DadosAntFunc.Matricula = funcionario.Matricula;
                        DadosAntFunc.Carteira = funcionario.Carteira;
                        DadosAntFunc.Pis = funcionario.Pis;
                        DadosAntFunc.CPF = funcionario.CPF;
                        if (parametro.IntegrarSalarioFunc == true)
                        {
                            DadosAntFunc.Salario = funcionario.Salario;
                        }
                        else
                        {
                            DadosAntFunc.Salario = 0;
                        }
                        DadosAntFunc.Senha = BLL.ClSeguranca.Criptografar(funcionario.SenhaRelogio == null ? "" : funcionario.SenhaRelogio);
                        DadosAntFunc.Dataadmissao = funcionario.Dataadmissao;
                        DadosAntFunc.Datademissao = funcionario.Datademissao;

                        //Remover o IF inteiro quando campo FuncionarioAtivo for removido, quanter apenas o que esta no ELSE
                        if (jsonRequisicao.IndexOf("FuncionarioAtivo", StringComparison.CurrentCultureIgnoreCase) >= 0 && jsonRequisicao.IndexOf("DataInativacao", StringComparison.CurrentCultureIgnoreCase) == -1)
                        {
                            if (funcionario.FuncionarioAtivo)
                                DadosAntFunc.DataInativacao = null;
                            else DadosAntFunc.DataInativacao = DadosAntFunc.DataInativacao == null ? (DadosAntFunc.Datademissao ?? DateTime.Now.Date) : DadosAntFunc.DataInativacao;
                        }
                        else
                        {
                            DadosAntFunc.DataInativacao = funcionario.DataInativacao;
                        }

                        DadosAntFunc.Idempresa = emp.Id;
                        DadosAntFunc.Iddepartamento = IdDep.GetValueOrDefault();
                        DadosAntFunc.Idfuncao = IdFuncao.GetValueOrDefault();
                        DadosAntFunc.Funcionarioativo = Convert.ToInt16(funcionario.FuncionarioAtivo);
                        DadosAntFunc.Campoobservacao = funcionario.CampoObservacao;
                        DadosAntFunc.Foto = funcionario.Foto;
                        DadosAntFunc.Excluido = 0;
                        DadosAntFunc.idIntegracao = funcionario.IdIntegracao;
                        DadosAntFunc.TipoMaoObra = funcionario.TipoMaoObra;
                        if (funcionario.CodTipoVinculo != null || funcionario.CodTipoVinculo > 0)
                        {
                            var idTipoVInculo = bllTipoVinculo.GetIdPorCod(funcionario.CodTipoVinculo.GetValueOrDefault());
                            DadosAntFunc.IdTipoVinculo = idTipoVInculo;
                        }
                        if ((funcionario.PessoaSupervisor != null && !String.IsNullOrEmpty(funcionario.PessoaSupervisor.RazaoSocial)))
                        {
                            Pessoa supervisor = bllPessoa.GetPessoaPorCNPJ_CPF(funcionario.PessoaSupervisor.CNPJ_CPF).FirstOrDefault();
                            if (supervisor == null || supervisor.Id == 0)
                            {
                                supervisor = bllPessoa.GetListPessoaPorNome(funcionario.PessoaSupervisor.RazaoSocial).FirstOrDefault();
                            }

                            if (supervisor == null || supervisor.Id == 0)
                            {
                                supervisor = PessoaController.SalvarPessoaWeb(funcionario.PessoaSupervisor, usuarioPontoWeb.ConnectionString, out Dictionary<string, string> errosPessoa);
                                if (errosPessoa.Count() != 0)
                                    throw new Exception("Erro ao cadastrar o supervisor do funcionário, Erro: " + string.Join(";", errosPessoa.Select(x => x.Key + "=" + x.Value).ToArray()));
                            }
                            if (supervisor != null)
                            {
                                DadosAntFunc.IdPessoaSupervisor = supervisor.Id;
                            }
                        }
                        else if (funcionario.IdIntegracaoPessoaSupervisor != null)
                        {
                            int? idPessoaSupervisor = bllPessoa.GetIdPorIdIntegracaoPessoa(funcionario.IdIntegracaoPessoaSupervisor);
                            DadosAntFunc.IdPessoaSupervisor = idPessoaSupervisor == 0 ? null : idPessoaSupervisor;
                        }

                        Acao acao = new Acao();
                        if (DadosAntFunc.Id == 0)
                        {
                            acao = Acao.Incluir;
                            int ultimoCodigo = bllFuncionario.MaxCodigo();
                            DadosAntFunc.Codigo = ultimoCodigo;
                            DadosAntFunc.Dscodigo = ultimoCodigo.ToString();
                            DadosAntFunc.idIntegracao = funcionario.IdIntegracao;
                            BLL.Horario bllHorario = new BLL.Horario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                            idHorario = bllHorario.MinIdHorarioNormal();
                            Modelo.Departamento dep = bllDepartamento.LoadObject(DadosAntFunc.Iddepartamento);
                            Modelo.Empresa empr = bllEmpresa.LoadObject(DadosAntFunc.Idempresa);

                            //Lógica para vincular o Horário padrão ao funcionário
                            if (dep != null && dep.IdHorarioPadraoFunc != null && dep.IdHorarioPadraoFunc > 0)
                            {
                                DadosAntFunc.Idhorario = dep.IdHorarioPadraoFunc.GetValueOrDefault();
                            }
                            else if (cont != null && cont.IdHorarioPadraoFunc != null && cont.IdHorarioPadraoFunc > 0)
                            {
                                DadosAntFunc.Idhorario = cont.IdHorarioPadraoFunc.GetValueOrDefault();
                            }
                            else if (empr != null && empr.IdHorarioPadraoFunc != null && empr.IdHorarioPadraoFunc > 0)
                            {
                                DadosAntFunc.Idhorario = empr.IdHorarioPadraoFunc.GetValueOrDefault();
                            }
                            else if (parametro != null && (parametro.IdHorarioPadraoFunc != null && parametro.IdHorarioPadraoFunc > 0))
                            {
                                DadosAntFunc.Idhorario = parametro.IdHorarioPadraoFunc.GetValueOrDefault();
                            }
                            else if (idHorario != null)
                            {
                                DadosAntFunc.Idhorario = idHorario.GetValueOrDefault();
                            }
                            if (idHorario <= 0)
                            {
                                throw new Exception("Nenhum horário cadastrado no Pontofopag. ");
                            }
                            if (DadosAntFunc.Idhorario > 0)
                            {
                                Modelo.Horario ObjHorario = bllHorario.LoadObject(DadosAntFunc.Idhorario);
                                DadosAntFunc.Tipohorario = Convert.ToInt16(ObjHorario.TipoHorario);
                            }
                        }
                        else
                        {
                            acao = Acao.Alterar;
                        }

                        if (funcionario.CodTipoVinculo.GetValueOrDefault() == 3) // Se tipo do vinculo for teceiro remove o funcionário do banco
                            DadosAntFunc.Naoentrarbanco = 1;
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        DadosAntFunc.NaoRecalcular = true;
                        DadosAntFunc.ForcarNovoCodigo = true;
                        erros = bllFuncionario.Salvar(acao, DadosAntFunc);
                        if (erros.Count > 0)
                        {
                            TrataErrosFuncionario(erros);
                        }
                        else
                        {
                            BLL.ContratoFuncionario bllContratoFun = new BLL.ContratoFuncionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                            bllFuncionario.SetContratoFuncionarioIntegracao(funcionario.IdIntegracao, (funcionario.IdintegracaoContrato).GetValueOrDefault(), acao);
                            BLL_N.JobManager.CalculoMarcacoes.RecalculaEdicaoFuncionario(DadosAntFunc, usuarioPontoWeb, true);
                            funcionario.Codigo = DadosAntFunc.Codigo;
                            return Request.CreateResponse(HttpStatusCode.OK, funcionario);
                        }
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += ex.Message;
                }
            }
            return TrataErroModelState(retErro);
        }

        private void SetaMinMaxProgressBar(int min, int max)
        {

        }

        private void SetaValorProgressBar(int valor)
        {

        }

        private void SetaMensagem(string mensagem)
        {

        }

        private void IncrementaProgressBar(int incremento)
        {

        }

        /// <summary>
        /// Excluir Funcionário.
        /// </summary>
        /// <param name="Codigo">Código do Funcionário Cadastrado</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(int IdIntegracao)
        {
            RetornoErro retErro = new RetornoErro();
            BLL.Funcionario bllFuncionario = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            BLL.ContratoFuncionario bllContratoFun = new BLL.ContratoFuncionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            BLL.Contrato bllContrato = new BLL.Contrato(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);

            if (ModelState.IsValid)
            {
                try
                {
                    int? idfuncionario = bllFuncionario.GetIdporIdIntegracao(IdIntegracao);
                    int? idContratoAnt = bllContratoFun.getContratoId((idfuncionario).GetValueOrDefault());
                    Modelo.Funcionario funcionario = bllFuncionario.LoadObject(idfuncionario.GetValueOrDefault());

                    if (funcionario.Id > 0 && funcionario.Id != null)
                    {
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        erros = bllFuncionario.Salvar(Acao.Excluir, funcionario);
                        if (erros.Count > 0)
                        {
                            TrataErrosFuncionario(erros);
                        }
                        else
                        {
                            Modelo.Contrato objContr = bllContrato.LoadObject(idContratoAnt.GetValueOrDefault());
                            int idIntegracaoContrato = objContr.idIntegracao.GetValueOrDefault();
                            bllFuncionario.SetContratoFuncionarioIntegracao(IdIntegracao, idIntegracaoContrato, Acao.Alterar);
                            return Request.CreateResponse(HttpStatusCode.OK, funcionario);
                        }
                    }
                    else
                    {
                        retErro.erroGeral = "Funcionário Não Encontrado";
                        return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    retErro.erroGeral += ex.Message;
                }
            }
            return TrataErroModelState(retErro);
        }

        private bool ValidaDados(LModel.Funcionario funcionario, RetornoErro retErro, string connectionStr, out Modelo.Empresa emp, out int? IdDep, out int? idFunc, string descricaoFuncao, Contrato cont)
        {
            bool erro = false;
            if (funcionario.IdintegracaoContrato.GetValueOrDefault() > 0 && cont.Id == 0)
            {
                ModelState.AddModelError("IdintegracaoContrato", "Contrato não encontrado no Pontofopag");
                erro = true;
            }

            BLL.Empresa empBLL = new BLL.Empresa(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            emp = new Modelo.Empresa();
            emp = empBLL.LoadObjectByDocumento(funcionario.DocumentoEmpresa);
            if (emp.Id <= 0)
            {
                retErro.erroGeral += "Empresa não encontrada. ";
                ModelState.AddModelError("DocumentoEmpresa", "Empresa informada não encontrada");
                erro = true;
            }
            BLL.Departamento depBLL = new BLL.Departamento(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            if (funcionario.IdIntegracaoDepartamento != null)
            {
                IdDep = depBLL.GetIdPoridIntegracao(funcionario.IdIntegracaoDepartamento.GetValueOrDefault());
            }
            else // Else para atender o caso da employer que não é obrigatório o Departamento
            {
                IdDep = depBLL.GetDepartamentoPadrao(funcionario.DocumentoEmpresa.ToString()).Id;
                if (IdDep == 0)
                {
                    try
                    {
                        BLL.Departamento bllDep = new BLL.Departamento(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                        Modelo.Departamento dep = new Modelo.Departamento();
                        dep.Codigo = bllDep.MaxCodigo();
                        dep.Descricao = emp.Nome;
                        dep.IdEmpresa = emp.Id;
                        Dictionary<string, string> erroDep = bllDep.Salvar(Acao.Incluir, dep);
                        if (erroDep.Count() > 0)
                        {
                            throw new Exception("Erro ao criar o departamento padrão para a empresa: " + emp.Nome);
                        }
                        IdDep = dep.Id;
                    }
                    catch (Exception ex)
                    {
                        BLL.cwkFuncoes.LogarErro(ex);
                        throw new Exception("Erro ao criar o departamento padrão para a empresa: " + emp.Nome);
                    }
                }
            }

            if (IdDep <= 0)
            {
                retErro.erroGeral += "Departamento não encontrado. ";
                ModelState.AddModelError("CodigoDepartamento", "Departamento não encontrado");
                erro = true;
            }
            BLL.Funcao funcBLL = new BLL.Funcao(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            idFunc = null;
            if (funcionario.IdIntegracaoFuncao != null)
            {
                idFunc = funcBLL.GetIdPorIdIntegracao(funcionario.IdIntegracaoFuncao);
                erro = ValidaErroFuncao(retErro, idFunc, erro);
            }
            else //Lógica para atender a Employer, que passará apenas a descrição da função (Não controlará a integração de função)
            {
                if (!String.IsNullOrEmpty(descricaoFuncao))
                {
                    idFunc = funcBLL.getFuncaoNome(descricaoFuncao);
                    if (idFunc.GetValueOrDefault() == 0)
                    {
                        Modelo.Funcao DadosAntFunc;
                        Dictionary<string, string> erros;
                        BLLAPI.Funcao.SalvarFuncaoPorDescricao(descricaoFuncao, funcBLL, out DadosAntFunc, out erros);
                        if (erros.Count > 0)
                        {
                            retErro.erroGeral += "Erro ao criar a função para o funcionário. ";
                            foreach (KeyValuePair<string, string> err in erros)
                            {
                                ErroDetalhe ed = new ErroDetalhe();
                                ed.campo = err.Key;
                                ed.campo = err.Value;
                                retErro.ErrosDetalhados.Add(ed);
                            }
                            erro = true;
                        }
                        else
                        {
                            idFunc = DadosAntFunc.Id;
                        }
                    }
                }
                erro = ValidaErroFuncao(retErro, idFunc, erro);
            }

            return erro;
        }

        private bool ValidaErroFuncao(RetornoErro retErro, int? idFunc, bool erro)
        {
            if (idFunc.GetValueOrDefault() <= 0)
            {
                retErro.erroGeral += "Função não encontrada. ";
                ModelState.AddModelError("CodigoFuncao", "Função não encontrada");
                erro = true;
            }
            return erro;
        }

        #region métodos de tratamento

        private HttpResponseMessage TrataErroModelState(RetornoErro retErro)
        {
            List<ErroDetalhe> lErroDet = new List<ErroDetalhe>();
            var errorList = ModelState.Where(x => x.Value.Errors.Count > 0)
                                        .ToDictionary(
                                        kvp => kvp.Key,
                                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                        );
            foreach (var item in errorList)
            {
                ErroDetalhe ed = new ErroDetalhe();
                ed.campo = item.Key;
                ed.erro = String.Join(", ", item.Value);
                lErroDet.Add(ed);
            }
            if (retErro.erroGeral == "")
            {
                retErro.erroGeral = "Um ou mais erros encontrados, verifique os detalhes!";
            }
            retErro.ErrosDetalhados = lErroDet;
            return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
        }

        private void TrataErrosFuncionario(Dictionary<string, string> erros)
        {
            //Componente Ex:txtCodigo, Nome no modelo onde o erro será adicionado Ex: Codigo
            Dictionary<string, string> ComponenteToModel = new Dictionary<string, string>();
            ComponenteToModel.Add("txtCodigoDS", "Codigo");

            foreach (var item in ComponenteToModel)
            {
                ErroToModelState(erros, item);
                erros = erros.Where(x => !x.Key.Equals(item.Key)).ToDictionary(x => x.Key, x => x.Value);
            }

            var camposComErro = typeof(LModel.Funcionario).GetProperties().Where(w => erros.Select(s => s.Key).Contains(w.Name)).Select(s => s.Name);
            erros.Where(w => camposComErro.Contains(w.Key)).ToList().ForEach(f => ModelState.AddModelError(f.Key, f.Value));
            camposComErro.ToList().ForEach(f => erros.Remove(f));

            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                ModelState.AddModelError("CustomError", erro);
            }
        }

        private void ErroToModelState(Dictionary<string, string> erros, KeyValuePair<string, string> item)
        {
            Dictionary<string, string> erro = erros.Where(x => x.Key.Equals(item.Key)).ToDictionary(x => x.Key, x => x.Value);
            if (erro.Count > 0)
            {
                ModelState.AddModelError(item.Value, string.Join(";", erro.Select(x => x.Value).ToArray()));
            }
        }
        #endregion}

        /// <summary>
        /// Método responsável por retornar se o funcionário possui ou não registro no Pontofopag
        /// </summary>
        /// <returns>Retorna objeto contendo os dados principais do funcionário.;
        [HttpGet]
        public HttpResponseMessage GetFuncionarioPorCPFeMatricula(string CPF, string Matricula)
        {
            RetornoErro retErro = new RetornoErro();
            BLL.Funcionario bllFunc = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            try
            {
                CPF = CPF.Replace("-", "").Replace(".", "");
                Int64 CPFint = Convert.ToInt64(CPF);
                Models.Funcionario func = new LModel.Funcionario();
                Modelo.Funcionario funcionario = new Modelo.Funcionario();
                funcionario = bllFunc.GetFuncionarioPorCpfeMatricula(CPFint, Matricula);
                if (funcionario == null || funcionario.Id == 0)
                {
                    retErro.erroGeral = "Funcionário não encontrado";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
                }
                else
                {
                    func.CPF = funcionario.CPF;
                    func.Codigo = Convert.ToInt32(funcionario.Dscodigo);
                    func.Carteira = funcionario.Carteira;
                    func.CampoObservacao = funcionario.Campoobservacao;
                    func.Codigofolha = funcionario.Codigofolha;
                    func.Dataadmissao = funcionario.Dataadmissao;
                    func.Datademissao = funcionario.Datademissao;
                    func.FuncionarioAtivo = Convert.ToBoolean(funcionario.Funcionarioativo);
                    func.IdIntegracao = funcionario.idIntegracao.GetValueOrDefault();
                    func.Matricula = funcionario.Matricula;
                    func.Nome = funcionario.Nome;
                    func.Pis = funcionario.Pis;
                    func.Salario = funcionario.Salario;
                    func.FuncionarioExcluido = Convert.ToBoolean(funcionario.Excluido);
                    return Request.CreateResponse(HttpStatusCode.OK, func);
                }

            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
            }
        }

        /// <summary>
        /// Método responsável por retornar os funcionários ativos na base para o serviço de bloqueio.
        /// </summary>
        [HttpGet]
        public HttpResponseMessage CarregarAtivosBloqueio()
        {
            BLL.Funcionario bllFunc = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            DataTable funcionariosEntrada = bllFunc.CarregarTodosParaAPI();
            List<Models.Funcionario> funcionariosSaida = new List<LModel.Funcionario>();

            foreach (DataRow linha in funcionariosEntrada.Rows)
            {
                Models.Funcionario saida = new LModel.Funcionario();
                saida.CPF = Convert.ToString(linha["cpf"]);
                saida.Matricula = Convert.ToString(linha["matricula"]);
                saida.Nome = Convert.ToString(linha["nome"]);
                funcionariosSaida.Add(saida);
            }

            return Request.CreateResponse(HttpStatusCode.OK, funcionariosSaida);
        }

        /// <summary>
        /// Método para retornar todos os funcionários com opção de filtro por excluídos, Inativos ou todos
        /// </summary>
        /// <param name="ativo">0 = Inativo, 1 = Ativo e 2 = Todos</param>
        /// <param name="excluido">0 = Não Excluído, 1 = Excluído e 2 = Todos</param>
        /// <returns>Retornar DataTable com Funcionários</returns>
        [Route("api/Funconario/GetAllFuncs")]
        [HttpGet]
        public HttpResponseMessage GetAllFuncs(Int16 ativo, Int16 excluido)
        {
            RetornoErro retErro = new RetornoErro();
            BLL.Funcionario bllFunc = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            try
            {
                DataTable dtFuncs = bllFunc.CarregarTodosParaAPI(ativo, excluido);
                DataTableReader reader = new DataTableReader(dtFuncs);
                List<FuncionarioPainel> Ret = MetodosAuxiliares.DataReaderMapToList<FuncionarioPainel>(reader);

                return Request.CreateResponse(HttpStatusCode.OK, Ret);
            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
            }
        }
    }

    public class FuncionarioPainel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Matricula { get; set; }

        public string Cpf { get; set; }

        public int? FuncionarioAtivo { get; set; }

        public int? Excluido { get; set; }

        public string Cnpj { get; set; }

        public string NomeEmpresa { get; set; }

    }
}
