using cwkWebAPIPontoWeb.Controllers.BLLAPI;
using cwkWebAPIPontoWeb.Models;
using cwkWebAPIPontoWeb.Utils;
using Modelo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CentralCliente;
using System.Web.Http.Description;

namespace cwkWebAPIPontoWeb.Controllers
{
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ImportarAFDController : ExtendedApiController
    {
        ////
        //[HttpPost]
        //public string ImportarAFD()
        //{
        //    var request = HttpContext.Current.Request;
        //    var nomeArquivo = request.Headers["filename"];
        //    string idEnvio = Guid.NewGuid().ToString();

        //    nomeArquivo = idEnvio + nomeArquivo;
        //    if (nomeArquivo.IndexOf(".txt") <= -1)
        //    {
        //        nomeArquivo += ".txt";
        //    }
        //    string caminhoPasta = HttpContext.Current.Server.MapPath(String.Format("~/{0}", "Temp"));
        //    string filePath = MetodosAuxiliares.PreparaLocalArquivo(nomeArquivo, caminhoPasta);
        //    using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
        //    {
        //        request.InputStream.CopyTo(fs);
        //    }
        //    return "Arquivo recebido com sucesso, nome do arquivo criado: " + nomeArquivo;
        //}

        /// <summary>
        /// Método para receber um arquivo AFD para importação dos bilhetes
        /// </summary>
        /// <returns>Retorno um HttpStatusCode com o Ok caso a importação tenha ocorrido corretamente ou o codigo de erro</returns>
        [HttpPost]
        [TratamentoDeErro]
        public async Task<HttpResponseMessage> UploadAFD()
        {
            RetornoErro retErro = new RetornoErro();
            retErro.ErrosDetalhados = new List<ErroDetalhe>();
            string erro = String.Empty;
            if (!Request.Content.IsMimeMultipartContent())
            {
                retErro.erroGeral = "A requisição contém conteudo inválido!";
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, retErro);
            }

            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var file in provider.Contents)
                {
                    var dataStream = await file.ReadAsStreamAsync();

                    string nomeArquivo = file.Headers.ContentDisposition.FileName;
                    if (nomeArquivo.StartsWith("\"") && nomeArquivo.EndsWith("\""))
                    {
                        nomeArquivo = nomeArquivo.Trim('"');
                    }
                    if (nomeArquivo.Contains(@"/") || nomeArquivo.Contains(@"\"))
                    {
                        nomeArquivo = Path.GetFileName(nomeArquivo);
                    }

                    string extensao = Path.GetExtension(nomeArquivo);
                    if (string.IsNullOrEmpty(file.Headers.ContentDisposition.FileName) || extensao != ".txt")
                    {
                        retErro.erroGeral = "Erro no nome ou formato do arquivo, apenas arquivos .txt são aceitos, arquivo com erro: " + nomeArquivo;
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, retErro);
                    }

                    string idEnvio = Guid.NewGuid().ToString();

                    nomeArquivo = idEnvio +"^"+ nomeArquivo;

                    string caminhoPasta = HttpContext.Current.Server.MapPath(String.Format("~/{0}", "Temp"));
                    string filePath = MetodosAuxiliares.PreparaLocalArquivo(nomeArquivo, caminhoPasta);

                    using (FileStream fileStream = File.Create(filePath, (int)dataStream.Length))
                    {
                        byte[] bytesInStream = new byte[dataStream.Length];
                        dataStream.Read(bytesInStream, 0, (int)bytesInStream.Length);
                        fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                    }
                    Usuario usuario = MetodosAuxiliares.Usuario();
                    Modelo.REP objRep = new Modelo.REP();
                    string line1 = File.ReadLines(filePath).First();
                    string numSerie = line1.Substring(187, 17);
                    objRep = BLLAPI.Rep.GetReps(usuario.connectionString).Where(s => s.NumSerie == numSerie).FirstOrDefault();
                    if (objRep == null || objRep.Id == 0)
	                {
                        retErro.erroGeral = "Rep "+numSerie+" não cadastrado no Pontofopag!";
                        ErroDetalhe erd = new ErroDetalhe();
                        retErro.ErrosDetalhados.Add(new ErroDetalhe { campo = "NumSerie", erro = "Rep não cadastrado no sistema" });
                        return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
	                }
                    Task.Run(() => ProcessarAFD(erro, nomeArquivo, filePath, usuario, objRep));
                }
                return Request.CreateResponse(HttpStatusCode.OK, "AFD Recebido com sucesso");
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                retErro.erroGeral += ex;
                return Request.CreateResponse(HttpStatusCode.BadRequest, retErro);
            }
        }

        private string ProcessarAFD(string erro, string nomeArquivo, string filePath, Usuario usuario, Modelo.REP objRep)
        {
            BLLAPI.AFD afd = new BLLAPI.AFD();
            FileInfo arquivo = new FileInfo(filePath);
            bool retorno = afd.ImportarAFD(usuario, objRep, nomeArquivo, arquivo, out erro);
            return erro;
        }

        private void ValidaDadosUploadAfd(ref HttpStatusCode httpRetorno, ref string erro)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                httpRetorno = HttpStatusCode.UnsupportedMediaType;
                erro = "A requisição contém conteudo inválido!";
            }
            //if (!BLL.cwkFuncoes.ValidarCNPJ(DocumentoPrincipal.ToString()) && !BLL.cwkFuncoes.ValidarCPF(DocumentoPrincipal.ToString()))
            //{
            //    httpRetorno = HttpStatusCode.BadRequest;
            //    erro = "Documento da empresa principal inválido!";
            //}
            //if (!setaConexaoPorDocumento(DocumentoPrincipal))
            //{
            //    httpRetorno = HttpStatusCode.BadRequest;
            //    erro = "Conexão da empresa não encontrada, Verifique se o CNPJ informado esta correto e se esse a base desse cliente foi gerada corretamente";
            //}
        }
    }
}
