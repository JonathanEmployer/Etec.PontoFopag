using cwkWebAPIPontoWeb.Models;
using Modelo;
using Modelo.Registrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FuncionarioBiometriasController : ExtendedApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objFuncionario"></param>
        /// <returns></returns>

        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage Cadastrar(BilheteBioEnvio objBiometriaRegistrador)
        {
            Erros = new RetornoErro();
            CentralCliente.Usuario usu = new CentralCliente.Usuario();

            try
            {
                Acao acao;
                bool erro = false;

                DadosConexao objDadosConexao = new DadosConexao();
                objDadosConexao = BLLAPI.UsuarioBLL.GetConexaoUsuario(objBiometriaRegistrador.Username);

                DescriptografarConexao(objDadosConexao.Conexao);
                BLL.Biometria BiometriaBLL = new BLL.Biometria(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                BLL.Funcionario FuncionarioBLL = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);

                if ((ModelState.IsValid) && 
                    (objBiometriaRegistrador.Biometrias != null && objBiometriaRegistrador.Biometrias.Count() > 0))
                {
                    foreach (var biometria in objBiometriaRegistrador.Biometrias)
                    {
                        erro = ValidaDados(Erros, objDadosConexao.Conexao, biometria);
                        if (!erro && ModelState.IsValid)
                        {
                            if (biometria.Id == 0)
                            {
                                acao = Acao.Incluir;
                                biometria.Codigo = BiometriaBLL.MaxCodigo();
                            }
                            else
                                acao = Acao.Alterar;


                            biometria.idfuncionario = objBiometriaRegistrador.IDFuncionario;
                            Dictionary<string, string> erros = new Dictionary<string, string>();
                            erros = BiometriaBLL.Salvar(acao, biometria);
                            if (erros.Count > 0)
                            {
                                TrataErros(erros);
                            }
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, objBiometriaRegistrador);
                }
                else
                {
                    Erros.erroGeral += "Biometrias não encontradas";
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                Erros.erroGeral += ex.Message;
            }

            return TrataErroModelState();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage GetFuncionarioBiometriasPorCPF(String cpf, String login)
        {
            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            Erros = new RetornoErro();
            try
            {

                DadosConexao objDadosConexao = new DadosConexao();
                objDadosConexao = BLLAPI.UsuarioBLL.GetConexaoUsuario(login);

                DescriptografarConexao(objDadosConexao.Conexao);

                BLL.Biometria BiometriaBLL = new BLL.Biometria(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);
                BLL.Funcionario FuncionarioBLL = new BLL.Funcionario(usuarioPontoWeb.ConnectionString, usuarioPontoWeb);

                objFuncionario = FuncionarioBLL.LoadAtivoPorCPF(cpf);

                if (objFuncionario != null && objFuncionario.Id > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, objFuncionario);
                }
                else
                {
                    Erros.erroGeral += "Funcionário não encontrado";
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                Erros.erroGeral += ex.Message;
            }
            return TrataErroModelState();
        }

        private bool ValidaDados(RetornoErro retErro, string connectionStr, Modelo.Biometria biometria)
        {
            bool erro = false;

            if ((biometria != null && biometria.valorBiometria == null) || (biometria == null))
            {
                retErro.erroGeral += "O valor da Biometria está vazio. ";
                ModelState.AddModelError("Biometria", "Biometria não inserida");
                erro = true;
            }

            return erro;
        }

        private void TrataErros(Dictionary<string, string> erros)
        {
            string erro = string.Join(";", erros.Select(x => x.Key + "=" + x.Value).ToArray());
            ModelState.AddModelError("CustomError", erro);
        }
    }
}
