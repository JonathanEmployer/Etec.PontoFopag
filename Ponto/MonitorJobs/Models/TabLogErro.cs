using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitorJobs.Models
{
    public class TabLogErro
    {
        public Guid Idf_Erro_Log { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public string Aplicacao { get; set; }

        public string Usuario { get; set; }

        public string Centro_Servico { get; set; }

        public string Empresa { get; set; }

        public string Sessao { get; set; }

        public string URL { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public Guid? Inner_Exception { get; set; }

        public string Stack_Trace { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string MachineName { get; set; }

        public string Custon_Message { get; set; }

        public string URL_Referrer { get; set; }

        public string WsRequest { get; set; }
    }
}