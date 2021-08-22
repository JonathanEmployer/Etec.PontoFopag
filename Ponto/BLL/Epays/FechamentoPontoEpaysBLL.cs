using BLL.Util;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace BLL.Epays
{
    public class FechamentoPontoEpaysBLL
    {
        private readonly string _azureWebJobsStorage;
        private readonly string _apiIntegradorEndpoint;
        private readonly string _apiIntegradorKey;
        private readonly CloudStorageAccount _storageAccount;
        private readonly string _connString;
        private readonly Empresa _empresaBll;

        public FechamentoPontoEpaysBLL(string connString)
        {
            _azureWebJobsStorage = ConfigurationManager.AppSettings["AzureWebJobsStorage"];
            _apiIntegradorEndpoint = ConfigurationManager.AppSettings["ApiIntegradorEpays"];
            _apiIntegradorKey = ConfigurationManager.AppSettings["ApiIntegradorKeyEpays"];
            _storageAccount = CloudStorageAccount.Parse(_azureWebJobsStorage);
            _connString = connString;

            _empresaBll = new BLL.Empresa(_connString);
        }

        public void UploadStorage(List<DocumentoHashDto> lstDocumentos)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var storageClient = _storageAccount.CreateCloudBlobClient();
            var blobContainer = storageClient.GetContainerReference("files");

            blobContainer.CreateIfNotExists();

            foreach (var doc in lstDocumentos ?? new List<DocumentoHashDto>())
            {
                var docName = doc.NomeArquivo;
                var fileName = $"{doc.Tracking}/{Path.GetFileName(docName)}";
                var blockBlob = blobContainer.GetBlockBlobReference(fileName);

                blockBlob.DeleteIfExists();

                blockBlob.Properties.ContentType = "application/pdf";
                blockBlob.Metadata.Add("Hash", doc.ToEncrypt());
                blockBlob.UploadFromFile(docName);
                
                Thread.Sleep(500);
                RemoveFile(docName);
            }

        }

        public void FinishUpload(string tracking)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (var handler = new HttpClientHandler())
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                using (var _client = new HttpClient(handler, false))
                {
                    _client.BaseAddress = new Uri(_apiIntegradorEndpoint);
                    _client.DefaultRequestHeaders.Accept.Clear();
                    _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                    _client.DefaultRequestHeaders.Add("x-functions-key", _apiIntegradorKey);

                    using (var content = new StringContent(tracking))
                    {
                        var result = _client.PostAsync("/api/finishupload", content).Result;
                    }
                }
            }
        }

        private void RemoveFile(string nomeArquivo)
        {
            try
            {
                File.Delete(nomeArquivo);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                Debug.WriteLine(ex.Message);
            }

            try
            {
                var folder = Path.GetDirectoryName(nomeArquivo);
                if (Directory.Exists(folder) && Directory.GetFiles(folder).Length == 0)
                    Directory.Delete(folder);
            }
            catch (Exception ex)
            {
                BLL.cwkFuncoes.LogarErro(ex);
                Debug.WriteLine(ex.Message);
            }
        }

        public void SendToEpaysDeleted(int idFechamento, IEnumerable<int> idsExcluidos)
        {
            var result = _empresaBll.GetCnpjsByFuncIds(idsExcluidos.ToArray());
            foreach (var item in result.GroupBy(r => r.cnpj))
            {
                var itensToDel = item.Where(i => idsExcluidos.Contains(i.idFuncionario));
                if (itensToDel.Any())
                {
                    using (var RabbitMqController = new RabbitMqController())
                    {
                        var messageIntegration = new MsgIntegrationFechamentoPontoDto(_connString)
                        {
                            IdFechamento = idFechamento,
                            IdsFuncionario = itensToDel.Select(i => i.idFuncionario),
                            Tracking = Guid.NewGuid().ToString(),
                            Cnpj = item.Key,
                            Acao = enuAcaoFechamentoPonto.Excluir
                        };
                        RabbitMqController.SendFechamentoIntegration(messageIntegration);
                    }
                }
            }
        }

    }
}
