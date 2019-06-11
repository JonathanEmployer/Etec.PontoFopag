using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Http.Description;
using cwkWebAPIPontoWeb.Models;
using Modelo;
using SimpleCrypto;
using System.Text;
using CentralCliente;

namespace cwkWebAPIPontoWeb.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UsuarioRegistradorController : ExtendedApiController
    {
        /// <summary>
        /// Método para validar o login nas Configurações do Registrador Biométrico.
        /// </summary>
        /// <returns>Se Login for Válido retorna a ConnectionString, se não retorna o Erro</returns>
        [HttpPost]
        [TratamentoDeErro]
        public async Task<HttpResponseMessage> ValidaLoginUsuario(LoginFuncionario user)
        {
            Erros = new RetornoErro();
            try
            {
                if (String.IsNullOrEmpty(user.Username) || String.IsNullOrEmpty(user.Password))
                {
                    Erros.erroGeral += "Dados não informados.";
                    return TrataErroModelState();
                }

                CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
                Usuario usuario = new Usuario();

                ICryptoService crypto = new PBKDF2();
                usuario = db.Usuario.Where(a => a.Login.Equals(user.Username)).FirstOrDefault();

                if (usuario != null)
                {
                    string passCrypto = crypto.Compute(user.Password, usuario.PasswordSalt);

                    if (crypto.Compare(usuario.Password, passCrypto))
                    {
                        if (VerificaUsuarioHabilitadoParaUtulizarConfiguracao(usuario))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            Erros.erroGeral += "Usuário não esta habilitado para cadastrar/alterar as Configurações do Registrador. Por favor entre em contato com o setor de RH de sua empresa!";
                        }
                    }
                    else
                    {
                        Erros.erroGeral += "Senha incorreta.";
                    }
                }
                else
                {
                    Erros.erroGeral += "Usuário não encontrado.";
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
        ///Verificar se usuário está habilitado para cadastrar/alterar as configurações do Registrador Biométrico.
        /// </summary>
        /// <param name="user">Json com os dados do Funcionário</param>
        /// <returns> Retorna json de função quando cadastrado com sucesso, quando apresentar erro retorno um json de erro.</returns>
        private bool VerificaUsuarioHabilitadoParaUtulizarConfiguracao(Usuario user)
        {
            try
            {
                DescriptografarConexao(user.connectionString);

                BLL.Cw_Usuario cw = new BLL.Cw_Usuario(StrConexao);
                Modelo.Cw_Usuario usuarioLogado = cw.LoadObjectLogin(user.Login);

                return usuarioLogado.utilizaregistradordesktop;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                throw ex;
            }
        }
    }
}