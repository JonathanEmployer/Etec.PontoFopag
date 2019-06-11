using Employer.STS.AuthenticateStandAlone;
using RegistradorPontoWeb.Controllers.DAL.Ponto;
using System;
using System.Configuration;
using System.Data.SqlClient;
using Modelo = RegistradorPontoWeb.Models.Painel;
using ModeloPonto = RegistradorPontoWeb.Models.Ponto;

namespace RegistradorPontoWeb.Controllers.BLL
{
    public class PainelRH
    {
        public Modelo.UsuarioPainelAutenticacaoRetorno VerificaUsuarioPainel(string cpf, string senha, string cs)
        {

            try
            {
                var autenticarLogin = new AuthenticateUser(new Uri(ConfigurationManager.AppSettings["WebServicePainel"]));
                var respostaLogin = autenticarLogin.AutenticarUsuarioSistema(new RequisicaoLogin(cpf, senha, cs));
                if (respostaLogin.LoginAceito == true)
                {
                    Modelo.UsuarioPainelAutenticacaoRetorno retornoUsuarioPainel = new Modelo.UsuarioPainelAutenticacaoRetorno();
                    retornoUsuarioPainel.mensagem = "Login Aceito!";
                    retornoUsuarioPainel.status = true;
                    return retornoUsuarioPainel;
                }
                else
                {
                    return new Modelo.UsuarioPainelAutenticacaoRetorno() { status = respostaLogin.LoginAceito, mensagem = respostaLogin.MotivoRecusaLogin };
                }

            }
            catch (Exception ex)
            {
                // TODO: Logar Erro
                throw new Exception("Erro ao validar senha do Painel do RH.");
            }
        } 
    }
}