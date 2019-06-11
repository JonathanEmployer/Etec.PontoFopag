using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PontoWeb.Controllers
{

    public class CustomFileResult : FileContentResult
    {
        public String nomeArquivo { get; set; }

        public CustomFileResult(byte[] fileContents, string contentType, string nome)
            : base(fileContents, contentType)
        {
            this.nomeArquivo = nome.Replace('/', '-');
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            response.AppendHeader("Content-Disposition", string.Format("inline; filename=\"{0}\"", nomeArquivo));
            response.Clear();
            base.WriteFile(response);
        }
 
    }
}