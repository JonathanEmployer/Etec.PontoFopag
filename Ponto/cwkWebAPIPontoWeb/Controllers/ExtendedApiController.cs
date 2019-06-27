using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using Modelo;
using System.Web.Http.Description;
using cwkWebAPIPontoWeb.Models;
using System.Text;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Configuration;
using SimpleCrypto;
using Employer.STS.AuthenticateStandAlone;
using System.Web.Http.Controllers;

namespace cwkWebAPIPontoWeb.Controllers
{
    public class ExtendedApiController : ApiController
    {
        public RetornoErro Erros { get; set; }

        public string StrConexao { get; set; }

        public Modelo.Funcionario func = new Modelo.Funcionario();


        public Modelo.UsuarioPontoWeb _usuarioPontoWeb { get; set; }
        /// <summary>
        /// Objeto que contém o nome(Login) do usuário, Conexão e CS
        /// </summary>
        public Modelo.UsuarioPontoWeb UsuarioPontoWeb { 
            get 
            {
                return _usuarioPontoWeb ?? (_usuarioPontoWeb = cwkWebAPIPontoWeb.Utils.MetodosAuxiliares.UsuarioPontoWebNovo(this.ActionContext));
            } 
        }
        

        public ExtendedApiController()
        {
            Erros = new RetornoErro();
            Modelo.cwkGlobal.BD = 1;
            Modelo.Cw_Usuario usuarioControle = new Modelo.Cw_Usuario();
            usuarioControle.Login = "Registrador";
            Modelo.cwkGlobal.objUsuarioLogado = usuarioControle;
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public string DescriptografarConexao(string conexao)
        {
            try
            {
                StrConexao = BLL.CriptoString.Decrypt(conexao);
                return String.Empty;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return "Erro na criptografia da chave do usuário.";
            }
        }

        private void DescriptografarConexaoFuncionario(string conexao)
        {
            string retorno = DescriptografarConexao(conexao);
            if (!String.IsNullOrEmpty(retorno))
            {
                ModelState.AddModelError("ChaveFuncionario", "Erro na criptografia da chave do usuário.");
            }
            else
            {
                System.Data.SqlClient.SqlConnectionStringBuilder _build = new System.Data.SqlClient.SqlConnectionStringBuilder(StrConexao);
                _build.ApplicationName = "cwkWebAPI";
                StrConexao = _build.ConnectionString;
            }
        }

        /// <summary>
        /// Adiona os erros encontrados em uma lista para ser retornado para o cliente.
        /// </summary>
        /// <param name="campo">Campo em que ocorreu o erro</param>
        /// <param name="erro">Mensagem de erro</param>
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void AddErro(string campo, string erro)
        {
            int i = campo.IndexOf('.');
            campo = campo.Remove(0, i + 1);
            ErroDetalhe ed = null;
            if (Erros.ErrosDetalhados != null)
            {
                ed = Erros.ErrosDetalhados.Where(x => x.campo == campo).FirstOrDefault(); 
            }
            else
            {
                Erros.ErrosDetalhados = new List<ErroDetalhe>();
            }

            if (ed != null)
            {
                List<string> erros = new List<string>(ed.erro.Split(','));
                erros.Add(erro);
                ed.erro = string.Join<string>(",", erros);
            }
            else
            {
                ed = new ErroDetalhe();
                ed.campo = campo;
                ed.erro = erro;
                Erros.ErrosDetalhados.Add(ed);
            }
        }
        /// <summary>
        /// Retorna a quantidade de erros encontrados
        /// </summary>
        /// <returns>Quantidade de erros encontrados</returns>
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public int QuantidadeErros()
        {
            if (Erros != null && Erros.ErrosDetalhados != null)
            {
                return Erros.ErrosDetalhados.Count();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Pega o ModalState e transforma em uma lista de erros detalhes e retorna os erros para o cliente.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public HttpResponseMessage TrataErroModelState()
        {
            var errorList = ModelState.Where(x => x.Value.Errors.Count > 0)
                                        .ToDictionary(
                                        kvp => kvp.Key,
                                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).Distinct().ToArray()
                                        );
            foreach (var item in errorList)
            {
                foreach (string errod in item.Value)
                {
                    AddErro(item.Key, errod);
                }
            }
            if (String.IsNullOrEmpty(Erros.erroGeral))
            {
                Erros.erroGeral = string.Join<string>("; ",Erros.ErrosDetalhados.Select(x => x.erro));
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest, Erros);
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void TrataErros(Dictionary<string, string> erros)
        {
            //Componente Ex:txtCodigo, Nome no modelo onde o erro será adicionado Ex: Codigo
            Dictionary<string, string> ComponenteToModel = new Dictionary<string, string>();
            ComponenteToModel.Add("txtCodigo", "Codigo");
            ComponenteToModel.Add("txtDescricao", "Descricao");
            ComponenteToModel.Add("cbIdEmpresa", "CodigoEmpresa");
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
        
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void ValidaDadosFuncionarioSetaConexao(LoginFuncionario login)
        {
            if (login == null)
            {
                ModelState.AddModelError("CustomError", "Dados não informados");
            }
            if (ModelState.IsValid)
            {
                if (BLL.cwkFuncoes.ValidarCPF(login.Username))
                {
                    if (String.IsNullOrEmpty(login.ChaveFuncionario))
                    { 
                        DadosConexao Conexao = new DadosConexao();
                        Conexao = BLLAPI.Funcionario.BuscaDadosConexao(login.Username);
                        if (Conexao.Sucesso)
                        {
                            DescriptografarConexaoFuncionario(Conexao.Conexao);
                        }
                        else
                        {
                            ModelState.AddModelError("Username", "Sua empresa não esta habilitada para registro de ponto eletrônico ou não utiliza o Sistema Pontofopag. Por favor entre em contato com o setor de RH de sua empresa!");
                        }
                    }
                    else
                    {
                        DescriptografarConexaoFuncionario(login.ChaveFuncionario);
                    } 
                }
                else
                {
                    ModelState.AddModelError("Username", "CPF Inválido");
                }

                try
                {
                    if (ModelState.IsValid)
                    {
                        UsuarioPainelAutenticacaoRetorno UsuarioPainel = VerificaUsuarioPainel(login.Username, login.Password);
                        if (UsuarioPainel.mensagem != null && UsuarioPainel.status == true)
                        {
                            if (UsuarioPainel.status == true)
                            {
                                func = BLLAPI.Funcionario.BuscaFuncionario(login.Username, login.Password, StrConexao);
                            }
                        }
                        else
                        {
                            func = BLLAPI.Funcionario.BuscaFuncionario(login.Username, login.Password, StrConexao);
                            if (func == null || func.Id <= 0 || BLL.ClSeguranca.Descriptografar(func.Mob_Senha) != login.Password)
                            {
                                ModelState.AddModelError("Password", "CPF ou senha incorretos.");
                            }
                        }
                        if (ModelState.IsValid)
                        {
                            Modelo.Funcionario funcFechamento = BLLAPI.Funcionario.FuncionarioComUltimosFechamentos(func.Id, StrConexao);
                            if (DateTime.Now.Date <= funcFechamento.DataUltimoFechamento.GetValueOrDefault() || DateTime.Now.Date <= funcFechamento.DataUltimoFechamentoBH.GetValueOrDefault())
                            {
                                ModelState.AddModelError("Password", "Seu cartão ponto esta fechado, entre em contato com o RH.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                    ModelState.AddModelError("Password", "Erro: " + ex.Message);    
                }
            }
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public UsuarioPainelAutenticacaoRetorno VerificaUsuarioPainel(string cpf, string senha)
        {
            BLL.ParametroPainelRH bllParametroPainel = new BLL.ParametroPainelRH(StrConexao);
            Modelo.ParametroPainelRH parm = new ParametroPainelRH();
            parm = bllParametroPainel.GetAllList().FirstOrDefault();

            try
            {
                var autenticarLogin = new AuthenticateUser(new Uri(ConfigurationManager.AppSettings["WebServicePainel"]));
                var respostaLogin = autenticarLogin.AutenticarUsuarioSistema(new RequisicaoLogin(cpf, senha, parm.CS));
                if (respostaLogin.LoginAceito == true)
                {
                    UsuarioPainelAutenticacaoRetorno retornoUsuarioPainel = new UsuarioPainelAutenticacaoRetorno();
                    retornoUsuarioPainel.mensagem = "Login Aceito!";
                    retornoUsuarioPainel.status = true;
                    return retornoUsuarioPainel;
                }
                else
                {
                    return new UsuarioPainelAutenticacaoRetorno();
                }

            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw new Exception("Erro ao validar senha do Painel do RH.");
            }
        } 

        ///// <summary>
        ///// Esse método busca e seta a conexão do cliente de acordo com o CNPJ
        ///// </summary>
        ///// <param name="documento">CNPJ ou CPF da empresa principal (o mesmo que foi criado a base de dados)</param>
        ///// <returns></returns>
        //[NonAction]
        //public bool setaConexaoPorDocumento(Int64 documento)
        //{
        //    DadosConexao Conexao = new DadosConexao();
        //    Conexao = BLLAPI.Empresa.BuscaDadosConexaoDocumento(documento);
        //    if (Conexao.Sucesso)
        //    {
        //        string erro = DescriptografarConexao(Conexao.Conexao);
        //        if (!String.IsNullOrEmpty(erro))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void ValidaUtilizacaoRegistrador()
        {
            if (!func.utilizaregistrador)
            {
                ModelState.AddModelError("Username", "Não autorizada. Entre em contato com o setor de RH");
            }
        }


        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void IncrementaProgressBar(int incremento)
        {

        }
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void SetaValorProgressBar(int valor)
        {

        }
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void SetaMinMaxProgressBar(int min, int max)
        {

        }
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void SetaMensagem(string mensagem)
        {

        }
    }
}