using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PontoWeb.Models
{
    public class TentativasLogin
    {
        public string Usuario { get; set; }
        public int Tentativas { get; set; }
        public DateTime UltimaTentativa { get; set; }
    }
}