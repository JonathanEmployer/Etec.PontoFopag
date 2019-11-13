using cwkWebAPIPontoWeb.Models;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using cwkWebAPIPontoWeb.Utils;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    /// <summary>
    /// Métodos Referentes ao Cadastro de Pessoa
    /// </summary>
    public class PessoaController : ApiController
    {
        /// <summary>
        /// Cadastrar/Alterar Pessoa.
        /// </summary>
        /// <param name="pessoa">Json com os dados de pessoa</param>
        /// <returns> Retorna json de pessoa quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(Models.Pessoa pessoa)
        {
            RetornoErro retErro = new RetornoErro();
            if (!string.IsNullOrEmpty(pessoa.CNPJ_CPF))
            {
                pessoa.FormatarCNPJ_CPF();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    Dictionary<string, string> erros;
                    string connectionStr = MetodosAuxiliares.Conexao();
                    SalvarPessoaWeb(pessoa, connectionStr, out erros);
                    if (erros.Count > 0)
                    {
                        TrataErros(erros);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, pessoa);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pessoa"></param>
        /// <param name="erros"></param>
        public static void SalvarPessoaWeb(Models.Pessoa pessoa, string connectionStr, out Dictionary<string, string> erros)
        {
            Modelo.Pessoa DadosAntFunc;
            BLL.Pessoa bllPessoa = new BLL.Pessoa(connectionStr);
            int? idpessoa = bllPessoa.GetIdPorIdIntegracaoPessoa(pessoa.IdIntegracao);
            DadosAntFunc = new Modelo.Pessoa();
            if (idpessoa > 0)
            {
                DadosAntFunc = bllPessoa.LoadObject(idpessoa.GetValueOrDefault());
            }
            else
            {
                DadosAntFunc = bllPessoa.GetPessoaPorCNPJ_CPF(pessoa.CNPJ_CPF).OrderBy(o => o.IdIntegracao).FirstOrDefault();
                if (DadosAntFunc == null)
                {
                    DadosAntFunc = new Modelo.Pessoa();
                }

            }

            Acao acao = new Acao();
            if (DadosAntFunc.Id == 0)
            {
                acao = Acao.Incluir;
                DadosAntFunc.Codigo = pessoa.Codigo;
            }
            else
            {
                // Se o código atribuido pela folha não estiver em uso por outro funcionário, aceito o código, caso contrario permanece o mesmo.
                Modelo.Pessoa pessoaCodigo = bllPessoa.GetPessoaPorCodigo(pessoa.Codigo).Where(w => w.Id != DadosAntFunc.Id).FirstOrDefault();
                if (pessoaCodigo == null || pessoaCodigo.Id == 0)
                {
                    DadosAntFunc.Codigo = pessoa.Codigo;
                }
                acao = Acao.Alterar;
            }

            DadosAntFunc.TipoPessoa = pessoa.TipoPessoa;
            DadosAntFunc.RazaoSocial = pessoa.RazaoSocial;
            DadosAntFunc.Fantasia = pessoa.Fantasia;
            DadosAntFunc.CNPJ_CPF = pessoa.CNPJ_CPF;
            DadosAntFunc.Insc_RG = pessoa.Insc_RG;
            DadosAntFunc.Email = pessoa.Email;
            DadosAntFunc.IdIntegracao = pessoa.IdIntegracao;

            erros = new Dictionary<string, string>();
            DadosAntFunc.ForcarNovoCodigo = true;
            erros = bllPessoa.Salvar(acao, DadosAntFunc);
            if (erros.Count == 0)
            {
                pessoa.Codigo = DadosAntFunc.Codigo;
            }
        }

        /// <summary>
        /// Excluir pessoa.
        /// </summary>
        /// <param name="Codigo">Código da pessoa Cadastrada</param>
        /// <returns> Retorna um HttpResponseMessage Ok quando excluído com sucesso, quando apresentar erro retorno um json de erro.</returns>
        [HttpDelete]
        [TratamentoDeErro]
        public HttpResponseMessage Excluir(string idIntegracao)
        {
            RetornoErro retErro = new RetornoErro();
            string connectionStr = MetodosAuxiliares.Conexao();
            BLL.Pessoa bllPessoa = new BLL.Pessoa(connectionStr);
            try
            {
                if (ModelState.IsValid)
                {
                    int? idpessoa = bllPessoa.GetIdPorIdIntegracaoPessoa(idIntegracao);
                    if (idpessoa != null && idpessoa > 0)
                    {
                        Dictionary<string, string> erros = new Dictionary<string, string>();
                        Modelo.Pessoa pessoa = bllPessoa.LoadObject(idpessoa.GetValueOrDefault());
                        erros = bllPessoa.Salvar(Acao.Excluir, pessoa);
                        if (erros.Count > 0)
                        {
                            TrataErros(erros);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                    else
                    {
                        retErro.erroGeral = "Pessoa Não Encontrada";
                        return Request.CreateResponse(HttpStatusCode.NotFound, retErro);
                    }

                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                retErro.erroGeral += ex.Message;
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
            Dictionary<string, string> erroCodigo = new Dictionary<string, string>();
            erroCodigo = erros.Where(x => x.Key.Equals("txtCodigo")).ToDictionary(x => x.Key, x => x.Value);
            if (erroCodigo.Count > 0)
            {
                ModelState.AddModelError("Codigo", string.Join(";", erroCodigo.Select(x => x.Value).ToArray()));
            }

            Dictionary<string, string> erroDescricao = new Dictionary<string, string>();
            erroDescricao = erros.Where(x => x.Key.Equals("txtDescricao")).ToDictionary(x => x.Key, x => x.Value);
            if (erroDescricao.Count > 0)
            {
                ModelState.AddModelError("Descricao", string.Join(";", erroDescricao.Select(x => x.Value).ToArray()));
            }

            erros = erros.Where(x => !x.Key.Equals("txtCodigo")).ToDictionary(x => x.Key, x => x.Value);
            erros = erros.Where(x => !x.Key.Equals("txtDescricao")).ToDictionary(x => x.Key, x => x.Value);
            if (erros.Count() > 0)
            {
                string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
                ModelState.AddModelError("CustomError", erro);
            }
        }
    }
}
