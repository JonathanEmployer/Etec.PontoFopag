using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DownloadController : ApiController
    {
        /// <summary>
        /// Baixar última versão disponível do Pontofopag Comunicador
        /// </summary>
        /// <returns>Retorna um aquivo .zip contendo o instaldor</returns>
        [Route("Comunicador/UltimaVersao")]
        public HttpResponseMessage GetFile()
        {
            try
            {
                HttpResponseMessage result = null;
                var localFilePath = HttpContext.Current.Server.MapPath("~/App_Data");

                string partialName = "Instalacao_PontofopagComunicador";

                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(localFilePath);
                FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + partialName + "*.*");

                FileInfo ultimaVersao = filesInDir.OrderByDescending(f => f.LastWriteTime).First();

                if (!File.Exists(ultimaVersao.FullName))
                {
                    result = Request.CreateResponse(HttpStatusCode.Gone);
                }
                else
                {
                    // Serve the file to the client
                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new FileStream(ultimaVersao.FullName, FileMode.Open, FileAccess.Read));
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = ultimaVersao.Name;
                    var contentType = MimeMapping.GetMimeMapping(Path.GetExtension(ultimaVersao.FullName));
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                }

                return result;
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Baixar última versão disponível do Pontofopag Comunicador
        /// </summary>
        /// <returns>Retorna um aquivo .zip contendo o instaldor</returns>
        [Route("Download")]
        public HttpResponseMessage GetFile(String arquivo)
        {
            try
            {
                if (arquivo.Contains("/") || arquivo.Contains(".."))
                {
                    throw new Exception("Nome do arquivo ("+arquivo+") inválido para requisição, nome não deve conter os caracteres / ou .. ");
                }
                HttpResponseMessage result = null;
                var localFilePath = HttpContext.Current.Server.MapPath("~/App_Data/Arquivos Compartilhados");

                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(localFilePath);
                FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles("*" + arquivo + "*.*");

                if (filesInDir == null || filesInDir.Count() == 0)
                {
                    throw new Exception("O arquivo solicitado não foi encontrado");
                }

                FileInfo ultimaVersao = filesInDir.OrderByDescending(f => f.LastWriteTime).First();

                if (!File.Exists(ultimaVersao.FullName))
                {
                    result = Request.CreateResponse(HttpStatusCode.Gone);
                }
                else
                {
                    // Serve the file to the client
                    result = Request.CreateResponse(HttpStatusCode.OK);
                    result.Content = new StreamContent(new FileStream(ultimaVersao.FullName, FileMode.Open, FileAccess.Read));
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    result.Content.Headers.ContentDisposition.FileName = ultimaVersao.Name;
                    var contentType = MimeMapping.GetMimeMapping(Path.GetExtension(ultimaVersao.FullName));
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                }

                return result;
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("não foi encontrado"))
                {
                    BLL.cwkFuncoes.LogarErro(ex);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
