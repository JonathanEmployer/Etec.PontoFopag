using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using RegistradorPontoWeb.Controllers.Util;

namespace RegistradorPontoWeb.Models
{
    public class Comprovante
    {
        [Display(Name = "Empresa")]
        public string EmpresaNome { get; set; }

        [Display(Name = "CNPJ")]
        public string EmpresaCNPJ { get; set; }

        [Display(Name = "CEI")]
        public string EmpresaCEI { get; set; }

        [Display(Name = "Nome")]
        public string FuncionarioNome { get; set; }

        [Display(Name = "PIS")]
        public string FuncionarioPIS { get; set; }

        public string Data { get; set; }

        public string Hora { get; set; }

        public string NS { get; set; }

        public string Chave { get; set; }

        public byte[] QrCode { get; set; }

        public string ChaveSeguranca { get; set; }

        public List<string> ChaveSegurancaView
        {
            get
            {
                if (!String.IsNullOrEmpty(ChaveSeguranca))
                {
                    string[] chaveSplit = ChaveSeguranca.SplitOnLength(44).ToArray();
                    return chaveSplit.ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
        }
    }
}