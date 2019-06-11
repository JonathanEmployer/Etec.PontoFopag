using cwkWebAPIPontoWeb.Models;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class BiometriaController : ExtendedApiController
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Login"></param>
        /// <returns></returns>
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage GetBiometrias(String Login)
        {
            Erros = new RetornoErro();
            IList<Modelo.Biometria> lstBiometrias = new List<Modelo.Biometria>();

            try
            {
                DadosConexao objDadosConexao = new DadosConexao();
                objDadosConexao = BLLAPI.UsuarioBLL.GetConexaoUsuario(Login);

                DescriptografarConexao(objDadosConexao.Conexao);
                if (!String.IsNullOrEmpty(objDadosConexao.Conexao))
                {
                    BLL.Biometria BiometriaBLL = new BLL.Biometria(StrConexao);
                    lstBiometrias = BiometriaBLL.GetAllList();             
                    return Request.CreateResponse(HttpStatusCode.OK, lstBiometrias);
                }
                else
                {
                    Erros.erroGeral += "Usuário não encontrado";
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                Erros.erroGeral += ex.Message;
            }

            return TrataErroModelState();
        }

    }
}
