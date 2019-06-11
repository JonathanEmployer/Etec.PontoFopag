using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PontoWeb.Models
{
    public class ResultadoArquivoUpload
    {
        public string Nome { get; set; }
        public int Tamanho { get; set; }
        public string Tipo { get; set; }
        public int IdRelogio { get; set; }
        public string Erro { get; set; }
    }

    public class ValidaArquivoUpload
    {
        public bool Valido { get {
            return String.IsNullOrEmpty(Erro);
        } }
        public string Erro { get; set; }
        public int IdRelogio { get; set; }
    }
}