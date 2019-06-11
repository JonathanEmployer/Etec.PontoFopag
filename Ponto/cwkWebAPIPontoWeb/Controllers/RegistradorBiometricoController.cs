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
    public class RegistradorBiometricoController : RegistraPontoBaseController
    {

        [HttpPost]
        [TratamentoDeErro]
        [ResponseType(typeof(Modelo.RegistraPonto))]
        public HttpResponseMessage RegistrarPonto(BilheteBioEnvio bilheteBio)
        {
            Erros = new RetornoErro();
            Bilhete bil;

            try
            {
                DadosConexao objDadosConexao = new DadosConexao();
                objDadosConexao = BLLAPI.UsuarioBLL.GetConexaoUsuario(bilheteBio.Username);

                DescriptografarConexao(objDadosConexao.Conexao);

                BLL.Funcionario bllFuncionario = new BLL.Funcionario(StrConexao);
                func = bllFuncionario.LoadObject(bilheteBio.IDFuncionario);

                if ((func != null) && (func.Id > 0))
                {
                    bil = new Bilhete(bilheteBio, func);
                    return EfetuaRegistroPonto(bil, false);
                }
                else
                {
                    string erro = "Biometria não encontrada";
                    ModelState.AddModelError("CustomError", erro);
                    return TrataErroModelState();
                }
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                Erros.erroGeral += ex.Message;
            }
         

            return TrataErroModelState();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [TratamentoDeErro]
        public HttpResponseMessage AtualizaHora(String FusoHorario)
        {
            Erros = new RetornoErro();
            DateTime agora = DateTime.Now;
            try
            {
                if (!String.IsNullOrEmpty(FusoHorario))
                {
                    agora = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, FusoHorario);
                }

                return Request.CreateResponse(HttpStatusCode.OK, agora);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                Erros.erroGeral = "Erro ao tentar atualizar o horário do registrador";
            }

            return TrataErroModelState();
        }

    }
}
