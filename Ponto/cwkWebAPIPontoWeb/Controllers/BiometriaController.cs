using CentralCliente;
using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
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

        [HttpPost]
        [TratamentoDeErro]
        public HttpResponseMessage PostBiometria([FromBody] Biometria Biometria)
        {
            List<ImportacaoDadosRep> LImportacaoDadosRep = new List<ImportacaoDadosRep>();
            string usuario = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                    && HttpContext.Current.User.Identity.Name != null)
            {
                usuario = HttpContext.Current.User.Identity.Name;
            }
            CENTRALCLIENTEEntities db = new CENTRALCLIENTEEntities();
            Usuario usu = new Usuario();
            try
            {
                usu = db.AspNetUsers.Where(r => r.UserName == usuario).FirstOrDefault().Usuario;
                string connectionStr = CriptoString.Decrypt(usu.connectionString);

                BLL.Biometria BiometriaBLL = new BLL.Biometria(connectionStr);

                var listFuncionario = new List<Modelo.Biometria>();

                CarregaBiometriaFuncionarioRep(Biometria.idRep, Biometria.idfuncionario, connectionStr, ref listFuncionario);

                if (listFuncionario.Count() > 0 && Biometria.Codigo == 0)
                {
                    string erro = "";
                    DeleteBiometriaFuncionarioRep(Biometria.idRep, Biometria.idfuncionario, connectionStr, ref erro);

                    if (!string.IsNullOrEmpty(erro))
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, erro);
                    }
                    else
                    {
                        if(Encoding.UTF8.GetString(Biometria.valorBiometria) == "0")
                            return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }

                //Biometria.Codigo = BiometriaBLL.MaxCodigo();

                BiometriaBLL.Salvar(Biometria);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                Erros.erroGeral += ex.Message;
            }

            return TrataErroModelState();
        }

        private static void CarregaBiometriaFuncionarioRep(int IdRep, int IdFuncionario, string connectionStr, ref List<Modelo.Biometria> Biometrias)
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@idRep", IdRep);
                    cmd.Parameters.AddWithValue("@idFuncionario", IdFuncionario);
                    conn.Open();
                    cmd.CommandText = @"select id from biometria
                                        where IdRep = @idRep and idfuncionario = @idFuncionario";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            List<Modelo.Biometria> dados = new List<Modelo.Biometria>();
                            Biometrias = MetodosAuxiliares.DataReaderMapToList<Modelo.Biometria>(reader);
                        }
                    }
                }
            }
        }

        private static void DeleteBiometriaFuncionarioRep(int IdRep, int IdFuncionario, string connectionStr, ref string Erro)
        {
            using (var conn = new SqlConnection(connectionStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@idRep", IdRep);
                    cmd.Parameters.AddWithValue("@idFuncionario", IdFuncionario);
                    conn.Open();
                    cmd.CommandText = @"delete biometria
                                        where IdRep = @idRep and idfuncionario = @idFuncionario";

                    int i = cmd.ExecuteNonQuery();
                    if (i < 0)
                    {
                        Erro = "Erro ao excluir registro do rep " + IdRep + ", funcionario " + IdFuncionario;
                    }
                }
            }
        }
    }
}
