using GerenciadorWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BLL;

namespace GerenciadorWeb.Controllers
{
    [Authorize]
    public class StatusController : ApiController 
    {
        [HttpGet]
        public HttpResponseMessage Status(string usuario)
        {
            BLL.Funcionario bllFuncionario = new Funcionario();
            BLL.LogScript bllLogScript = new BLL.LogScript();
            Modelo.RegraBloqueio.Funcionario objFuncionario = new Modelo.RegraBloqueio.Funcionario();
            StatusAPIModel status = new StatusAPIModel();
            try
            {
                objFuncionario = bllFuncionario.CarregarAtivoPorUsuario(usuario);
                if (objFuncionario == null)
                {
                    objFuncionario = new Modelo.RegraBloqueio.Funcionario();
                    objFuncionario.Nome = usuario;
                    objFuncionario.Usuario = usuario;
                    objFuncionario.Mensagem = "Usuário não encontrado.";
                    bllLogScript.GravarLogScript(objFuncionario);
                    return Request.CreateErrorResponse(HttpStatusCode.OK, "Usuario nao encontrado.");
                }

                if (objFuncionario.ExpiracaoFlagGestor.HasValue && objFuncionario.ExpiracaoFlagGestor.Value > DateTime.Now)
                {
                    status.Bloqueado = objFuncionario.FlagBloqueadoGestor ?? false;
                    if (!status.Bloqueado)
                    {
                        objFuncionario.MensagemFlagGestor = "";
                    }
                    status.Mensagem = objFuncionario.MensagemFlagGestor;
                    objFuncionario.Mensagem = "";
                    objFuncionario.AlertaEnviado = null;
                    objFuncionario.Liberacao = null;
                    objFuncionario.Bloqueado = false;
                }
                else
                {
                    if (!objFuncionario.Bloqueado && !String.IsNullOrEmpty(objFuncionario.Mensagem))
                    {
                        if (objFuncionario.AlertaEnviado == null)
                        {
                            objFuncionario.AlertaEnviado = DateTime.Now;
                            bllFuncionario.AtualizarAlertaEnviado(objFuncionario);
                        }
                        else
                        {
                            objFuncionario.Mensagem = String.Empty;
                        }
                    }

                    status.Bloqueado = objFuncionario.Bloqueado;
                    status.Mensagem = objFuncionario.Mensagem;

                    objFuncionario.FlagBloqueadoGestor = null;
                    objFuncionario.MensagemFlagGestor = null;
                    objFuncionario.ExpiracaoFlagGestor = null;
                }
                bllLogScript.GravarLogScript(objFuncionario);
                return Request.CreateResponse(HttpStatusCode.OK, status);
            }
            catch (Exception ex)
            {
                objFuncionario.Usuario = usuario;
                objFuncionario.Mensagem = ex.Message;
                bllLogScript.GravarLogScript(objFuncionario);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
                throw ex;
            }
        }
    }
}
