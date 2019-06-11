using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using cwkWebAPIPontoWeb.Models;
using Modelo;

namespace cwkWebAPIPontoWeb.Controllers
{
    /// <summary>
    /// Método responsável por retornar a conexão do usuário.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ChaveFuncionarioController : ApiController
    {
        /// <summary>
        /// Método Responsável por retornar a Conexão do Banco de onde o funcionários faz parte.
        /// </summary>
        /// <param name="CPF">CPF do Funcionário</param>
        /// <returns>Retorna Objeto DadosConexao</returns>
        public DadosConexao Get(string CPF)
        {
            DadosConexao Conexao = new DadosConexao();
            Conexao = BLLAPI.Funcionario.BuscaDadosConexao(CPF);
            return Conexao;
        }
    }
}