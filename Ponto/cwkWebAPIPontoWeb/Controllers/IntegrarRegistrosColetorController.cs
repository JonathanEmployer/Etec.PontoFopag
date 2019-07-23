using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class IntegrarRegistrosColetorController : ExtendedApiController
    {
        [HttpPost]
        public HttpResponseMessage IntegrarColetor(Modelo.Proxy.PxyRegistrosPontoIntegrar registrosFuncionarios)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Modelo.UsuarioPontoWeb usuario = MetodosAuxiliares.UsuarioPontoWebNovo(this.ActionContext);
                    BLL.RegistroPonto bllRegistroPonto = new BLL.RegistroPonto(usuario.ConnectionString, usuario);
                    bool retorno = false;
                    retorno = bllRegistroPonto.ProcessarRegistrosIntegrados(registrosFuncionarios, "CL", usuario);
                    if (retorno)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, registrosFuncionarios);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotFound, registrosFuncionarios);
                    }
                }
                else
                {
                    return TrataErroModelState();
                }
            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Erros.erroGeral = e.Message);
                
            }
        }
    }

}
