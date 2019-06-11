using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{
    public class EnviarEmailController : Controller
    {
        [HttpPost]
        public ActionResult EnviarEmail(string nome, string email, string telefone, string mensagem)
        {
                RetornoEmail ret = new RetornoEmail();
                try
                {
                    
                    ret.Success = true;
                    ret.Mensagem = "Mensagem enviada com sucesso, em breve entraremos em contato.";
                    ret.Erros =  new Dictionary<string, string>();
                    
                    if (String.IsNullOrEmpty(nome))
                    {
                        ret.Success = false;
                        ret.Erros.Add("nome", "Nome deve ser informado.");
                    }

                    if (String.IsNullOrEmpty(email))
                    {
                        ret.Success = false;
                        ret.Erros.Add("email", "E-mail deve ser informado.");
                    }

                    if (String.IsNullOrEmpty(telefone))
                    {
                        ret.Success = false;
                        ret.Erros.Add("telefone", "Telefone deve ser informado.");
                    }
                    if (String.IsNullOrEmpty(telefone))
                    {
                        ret.Success = false;
                        ret.Erros.Add("mensagem", "Mensagem deve ser informada.");
                    }

                    if (!ret.Success)
                    {
                        ret.Mensagem = "Um ou mais erros encontrados, verifique os campos destacados em vermelho.";
                    }
                    else
                    {
                        string conteudoEmail = @"<p><span><strong>Contato Realizado Pela Solu&ccedil;&atilde;o Pontofopag.</strong> </span></p>
                                                <p><span><strong>Contato de </strong>{NomeContato}</span></p>
                                                <p><span><strong>Telefone: </strong>{Telefone}</span></p>
                                                <p><span><strong>E-mail: </strong>{Email}</span></p>
                                                <p><span><strong>Mensagem: </strong>{Mensagem}</span></p>";
                        conteudoEmail = conteudoEmail.Replace("{NomeContato}", nome)
                                        .Replace("{Telefone}", telefone)
                                        .Replace("{Email}", email)
                                        .Replace("{Mensagem}", mensagem);
                        string assunto = "Contato Realizado Pela Solução Pontofopag Por " + nome;
                        SendMailVendas(assunto, conteudoEmail);
                    }

                }
                catch (Exception ex)
                {
                    ret.Success = false;
                    ret.Mensagem = ex.Message+"<p>Entre em contato com nosso telefone: (44) 3031-5351 ou pelo e-mail: vendas@cwork.com.br</p>";
                }
                return Json(new { ret }, JsonRequestBehavior.AllowGet);
        }

        public class RetornoEmail
        {
            public bool Success { get; set; }
            public string Mensagem { get; set; }
            public Dictionary<string,string> Erros { get; set; }
        }

        private static void SendMailVendas(string assunto, string conteudoEmail)
        {
            string smtp = "smtp.cwork.com.br";
            int porta = 25;
            bool enableSSL = false;
            string usuario = "vendas@cwork.com.br";
            string senha = "cwork@1212";
            string remetente = usuario;
            string destinatario = "vendas@cwork.com.br";
            BLL.cwkFuncoes.CwkSendMail(smtp, porta, enableSSL, usuario, senha, remetente, destinatario, assunto, conteudoEmail, true);
        }
    }
}
