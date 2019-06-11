using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Modelo.Proxy
{
    public class PxyFileResult
    {
        public byte[] Arquivo { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public Dictionary<string, string> Erros { get; set; }
    }
}
