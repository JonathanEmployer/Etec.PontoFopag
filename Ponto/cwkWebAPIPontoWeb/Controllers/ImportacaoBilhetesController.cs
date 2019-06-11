using cwkWebAPIPontoWeb.Controllers.BLLAPI;
using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using CentralCliente;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ImportacaoBilhetesController : ApiController
    {

        // POST api/ImportacaoBilhetes?login={login}&idRep={idRep}
        [HttpPost]
        [TratamentoDeErro]
        public bool Post(string login, int idRep, IList<string> arquivoStr)
        {
            try
            {
                string idEnvio = Guid.NewGuid().ToString();
                string nomeArquivo = idEnvio + ".txt";
                string caminhoPasta = HttpContext.Current.Server.MapPath(String.Format("~/{0}", "Temp"));
                FileInfo arquivo = GravarArquivo(nomeArquivo, caminhoPasta, arquivoStr);
                string erro = String.Empty;
                Usuario usuario = MetodosAuxiliares.Usuario();
                Modelo.REP objRep = new Modelo.REP();
                objRep = BLLAPI.Rep.GetReps(usuario.connectionString).Where(s => s.Id == idRep).FirstOrDefault();
                AFD afd = new AFD();
                bool retorno = afd.ImportarAFD(usuario, objRep, nomeArquivo, arquivo, out erro);
                return retorno;
            }
            catch (Exception e)
            {
                BLL.cwkFuncoes.LogarErro(e);
                throw;
            }
        }

        private FileInfo GravarArquivo(string nomeArquivo, string caminhoPasta, IList<string> arquivoStr)
        {

            string caminhoArquivo = MetodosAuxiliares.PreparaLocalArquivo(nomeArquivo, caminhoPasta);
            using (StreamWriter file = new StreamWriter(caminhoArquivo))
            {
                foreach (var linha in arquivoStr)
                {
                    file.WriteLine(linha);
                }
            }

            FileInfo retorno = new FileInfo(caminhoPasta + "\\" + nomeArquivo);
            return retorno;
        }
    }
}