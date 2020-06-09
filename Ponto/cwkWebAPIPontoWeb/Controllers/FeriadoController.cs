using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    /// <summary>
    /// Envio de feriados para o pontofopag
    /// </summary>
    public class FeriadoController : ExtendedApiController
    {
        

        /// <summary>
        /// Cadastrar/Alterar Feriado.
        /// </summary>
        /// <param name="feriado">Json com os dados do feriado</param>
        /// <returns>Retorna json de feriado quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(Models.Feriado feriado)
        {

            BLL.Feriado bllFeriado = new BLL.Feriado(MetodosAuxiliares.Conexao());
            RetornoErro retErro = new RetornoErro();
            bool salvouTodos = true;
            ValidaDados(feriado, usuarioPontoWeb.ConnectionString);

            if (ModelState.IsValid)
            {
                try
                {
                    List<Feriado> feriados = bllFeriado.GetIdPorIdIntegracao(feriado.IdIntegracao);

                    BLL.Funcionario bllFunc = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                   
                    List<Models.FuncionariosRecalculo> funcsRecalculo;
                    
                    salvouTodos = ExcluirFeriados(feriados, bllFunc, out funcsRecalculo);

                    if (salvouTodos)
                    {
                        List<int> idsFuncs = new List<int>();
                        if (feriado.Tipo == 0)
                        {
                            Modelo.Feriado feriadoNovo = new Feriado();
                            if (SalvarFeriado(Modelo.Acao.Incluir, feriadoNovo, feriado, usuarioPontoWeb.ConnectionString))
                            {
                                idsFuncs = bllFunc.GetIdsFuncsAtivos("");
                                funcsRecalculo.AddRange(idsFuncs.Select(s => new Models.FuncionariosRecalculo() { idFuncionario = s, DataInicial = feriado.Data, DataFinal = feriado.Data }).ToList());
                            }
                            else
                            {
                                salvouTodos = false;
                            }
                        }
                        else if (feriado.Tipo == 1)
                        {
                            foreach (KeyValuePair<int, string> emp in feriado.Empresas)
                            {
                                Modelo.Feriado feriadoNovo = new Feriado();
                                feriadoNovo.IdEmpresa = emp.Key;
                                if (SalvarFeriado(Modelo.Acao.Incluir, feriadoNovo, feriado, usuarioPontoWeb.ConnectionString))
                                {
                                    idsFuncs = bllFunc.GetIdsFuncsAtivos(" and idempresa = " + emp.Key);
                                    funcsRecalculo.AddRange(idsFuncs.Select(s => new Models.FuncionariosRecalculo() { idFuncionario = s, DataInicial = feriado.Data, DataFinal = feriado.Data }).ToList());
                                }
                                else
                                {
                                    salvouTodos = false;
                                }
                            }
                        }
                        else if (feriado.Tipo == 3)
                        {
                            Modelo.Feriado feriadoNovo = new Feriado();
                            feriadoNovo.IdsFeriadosFuncionariosSelecionados = String.Join(",", feriado.Funcionarios.Select(s => s.Key));
                            if (SalvarFeriado(Modelo.Acao.Incluir, feriadoNovo, feriado, usuarioPontoWeb.ConnectionString))
                            {
                                idsFuncs = feriado.Funcionarios.Select(s => s.Key).ToList();
                                funcsRecalculo.AddRange(idsFuncs.Select(s => new Models.FuncionariosRecalculo() { idFuncionario = s, DataInicial = feriado.Data, DataFinal = feriado.Data }).ToList());
                            }
                            else
                            {
                                salvouTodos = false;
                            }
                        } 
                    }

                    if (funcsRecalculo.Count() > 0)
                    {
                        Recalcular(funcsRecalculo);
                    }

                    if (salvouTodos)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, feriado);
                    }
                    else
                    {
                        return TrataErroModelState(retErro);
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

        private void Recalcular(List<Models.FuncionariosRecalculo> funcsRecalculo)
        {
            string connectionStr = MetodosAuxiliares.Conexao();
            Modelo.ProgressBar pb = new Modelo.ProgressBar();

            pb.incrementaPB = this.IncrementaProgressBar;
            pb.setaMensagem = this.SetaMensagem;
            pb.setaMinMaxPB = this.SetaMinMaxProgressBar;
            pb.setaValorPB = this.SetaValorProgressBar;
            Modelo.Cw_Usuario usuario = MetodosAuxiliares.Cw_Usuario();
            foreach (var item in funcsRecalculo.GroupBy(g => new { DataIni = g.DataInicial, DataFinal = g.DataFinal}))
            {
                BLLAPI.Marcacao.ThreadRecalculaMarcacaoList(item.Select(s => s.idFuncionario).Distinct().ToList(), item.Key.DataIni, item.Key.DataFinal, pb, connectionStr, usuario);
            }            
        }

        private bool ExcluirFeriados(List<Feriado> feriados, BLL.Funcionario bllFunc, out List<Models.FuncionariosRecalculo> funcsRecalculo)
        {

            BLL.Feriado bllFeriado = new BLL.Feriado(MetodosAuxiliares.Conexao());
            funcsRecalculo = new List<Models.FuncionariosRecalculo>();
            List<int> idsFuncs = new List<int>();
            bool excluiu = true;
            foreach (Modelo.Feriado feriadoExc in feriados)
            {
                BLL.FeriadoFuncionario bllFerFunc = new BLL.FeriadoFuncionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                List<Modelo.Funcionario> funcs = bllFerFunc.ListaFuncionariosFeriado(feriadoExc.Id);
                Dictionary<string, string> erros = new Dictionary<string, string>();
                feriadoExc.NaoRecalcular = true;
                erros = bllFeriado.Salvar(Acao.Excluir, feriadoExc);
                if (erros.Count() > 0)
                {
                    excluiu = false;
                }
                else
                {
                    if (feriadoExc.TipoFeriado != 3 && (funcs == null || funcs.Count() == 0))//Funcionário
                    {
                        if (feriadoExc.TipoFeriado == 0)//Geral
                        {
                            idsFuncs = bllFunc.GetIdsFuncsAtivos("");
                        }
                        else if (feriadoExc.TipoFeriado == 1) //Empresa
                        {
                            idsFuncs = bllFunc.GetIdsFuncsAtivos(" and idempresa = " + feriadoExc.IdEmpresa);
                        }
                        else if (feriadoExc.TipoFeriado == 2) //Departamento
                        {
                            idsFuncs = bllFunc.GetIdsFuncsAtivos(" and iddepartamento = " + feriadoExc.IdDepartamento);
                        }
                        funcsRecalculo = idsFuncs.Select(s => new Models.FuncionariosRecalculo() { idFuncionario = s, DataInicial = feriadoExc.Data.GetValueOrDefault(), DataFinal = feriadoExc.Data.GetValueOrDefault() }).ToList();
                    }
                    else
                    {
                        funcsRecalculo = funcs.Select(s => new Models.FuncionariosRecalculo() { idFuncionario = s.Id, DataInicial = feriadoExc.Data.GetValueOrDefault(), DataFinal = feriadoExc.Data.GetValueOrDefault() }).ToList();
                    }
                }
            }
            return excluiu;
        }

        private bool SalvarFeriado(Modelo.Acao acao, Modelo.Feriado feriado, Models.Feriado feriadoAPI, string conn)
        {
            BLL.Feriado bllFeriado = new BLL.Feriado(conn);
            feriado.Codigo = bllFeriado.MaxCodigo() + 1;
            feriado.Descricao = feriadoAPI.Descricao;
            feriado.IdIntegracao = feriadoAPI.IdIntegracao;
            feriado.Data = feriadoAPI.Data;
            feriado.TipoFeriado = feriadoAPI.Tipo;
            feriado.NaoRecalcular = true;

            Dictionary<string, string> erros = new Dictionary<string, string>();
            erros = bllFeriado.Salvar(acao, feriado);
            if (erros.Count > 0)
            {
                TrataErros(erros);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ValidaDados(Models.Feriado feriado, string connectionStr)
        {
            ValidaListaDocumentos(feriado, connectionStr);

            ValidaListaCPFs(feriado, connectionStr);
        }

        private void ValidaListaCPFs(Models.Feriado feriado, string connectionStr)
        {
            if (feriado.Tipo == 3)
            {
                if (feriado.CPFFuncionario.Count() == 0)
                {
                    ModelState.AddModelError("CPFFuncionario", "Quando tipo do feriado for por funcionário é necessário informar ao menos um CPF");
                }

                BLL.Funcionario bllFuncionario = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                feriado.Funcionarios = new Dictionary<int, string>();

                foreach (String cpf in feriado.CPFFuncionario)
                {
                    Funcionario func = bllFuncionario.LoadPorCPF(cpf);
                    if (func == null || func.Id == 0)
                    {
                        ModelState.AddModelError("DocumentoEmpresa", "Não foi encontrado o funcionário com o documento " + cpf);
                    }
                    else
                    {
                        feriado.Funcionarios.Add(func.Id, func.CPF);
                    }
                }
            }
        }

        private void ValidaListaDocumentos(Models.Feriado feriado, string connectionStr)
        {
            if (feriado.Tipo == 1)
            {
                if (feriado.DocumentoEmpresa.Count() == 0)
                {
                    ModelState.AddModelError("DocumentoEmpresa", "Quando tipo do feriado for por empresa é necessário informar ao menos um documento de empresa");
                }
                else
                {
                    BLL.Empresa bllEmpresa = new BLL.Empresa(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    feriado.Empresas = new Dictionary<int, string>();
                    foreach (String documento in feriado.DocumentoEmpresa)
                    {
                        Empresa emp = bllEmpresa.LoadObjectByDocumento(Convert.ToInt64(BLL.cwkFuncoes.ApenasNumeros(documento)));
                        if (emp == null || emp.Id == 0)
                        {
                            ModelState.AddModelError("DocumentoEmpresa", "Não foi encontrada empresa com o documento " + documento);
                        }
                        else
                        {
                            feriado.Empresas.Add(emp.Id, String.IsNullOrEmpty(emp.Cnpj) ? emp.Cpf : emp.Cnpj);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Excluir Feriado.
        /// </summary>
        /// <param name="idIntegracao">idIntegracao</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(int idIntegracao)
        {
            RetornoErro retErro = new RetornoErro();
            BLL.Departamento bllDepart = new BLL.Departamento(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
            bool excluiu = true;
            if (ModelState.IsValid)
            {
                try
                {
                    BLL.Feriado bllFeriado = new BLL.Feriado(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    List<Feriado> feriados = bllFeriado.GetIdPorIdIntegracao(idIntegracao);

                    BLL.Funcionario bllFunc = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                    List<Models.FuncionariosRecalculo> funcsRecalculo;
                    
                    bool retorno = ExcluirFeriados(feriados, bllFunc, out funcsRecalculo);

                    if (funcsRecalculo.Count() > 0)
                    {
                        Recalcular(funcsRecalculo);
                    }

                    if (retorno)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, feriados);
                    }
                    else
                    {
                        return TrataErroModelState(retErro);
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

        private void TrataErros(Dictionary<string, string> erros)
        {
            //Componente Ex:txtCodigo, Nome no modelo onde o erro será adicionado Ex: Codigo
            Dictionary<string, string> ComponenteToModel = new Dictionary<string, string>();
            ComponenteToModel.Add("txtCodigo", "Codigo");
            ComponenteToModel.Add("txtDescricao", "Descricao");
            foreach (var item in ComponenteToModel)
            {
                ErroToModelState(erros, item);
                erros = erros.Where(x => !x.Key.Equals(item.Key)).ToDictionary(x => x.Key, x => x.Value);
            }

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

        #region Métodos para progress
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
    }
}
