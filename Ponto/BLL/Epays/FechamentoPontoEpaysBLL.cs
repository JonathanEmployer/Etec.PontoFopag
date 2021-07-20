using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Epays
{
    public class FechamentoPontoEpaysBLL
    {
        private readonly string _azureWebJobsStorage;
        private readonly string _apiIntegradorEndpoint;
        private readonly string _apiIntegradorKey;
        private readonly CloudStorageAccount _storageAccount;

        public FechamentoPontoEpaysBLL()
        {
            _azureWebJobsStorage = ConfigurationManager.AppSettings["AzureWebJobsStorage"];
            _apiIntegradorEndpoint = ConfigurationManager.AppSettings["ApiIntegradorEpays"];
            _apiIntegradorKey = ConfigurationManager.AppSettings["ApiIntegradorKeyEpays"];
            _storageAccount = CloudStorageAccount.Parse(_azureWebJobsStorage);
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
    }
}
