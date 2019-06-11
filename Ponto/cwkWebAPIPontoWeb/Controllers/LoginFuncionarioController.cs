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
    /// Controller para validar login de funcionário
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LoginFuncionarioController : ExtendedApiController
    {
        /// <summary>
        /// Controller para validar o método de login, caso login ok, retorna os dados do funcionário
        /// </summary>
        /// <returns>Se Login for Válido retorna os dados do Funcionário, se não retorna objeto RetornoErro</returns>
        [ResponseType(typeof(Modelo.Funcionario))]
        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage ValidaLoginFuncionario(LoginFuncionario login)
        {
            ValidaDadosFuncionarioSetaConexao(login);
            if (ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, func);
            }
            return TrataErroModelState();
        }
    }
}
