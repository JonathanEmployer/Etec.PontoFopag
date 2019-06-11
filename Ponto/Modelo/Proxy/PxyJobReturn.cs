using Modelo.EntityFramework.MonitorPontofopag;
using System;
using System.Collections.Generic;

namespace Modelo.Proxy
{
    public class PxyJobReturn
    {
        public PxyJobReturn()
        {
        }

        public PxyJobReturn(JobControl jobControl)
        {
            this.DescricaoJob = jobControl.NomeProcesso;
            this.IdTask = jobControl.JobId.ToString();
            this.Mensagem = jobControl.Mensagem;
            this.Progress = jobControl.Progresso.GetValueOrDefault();
            this.UrlHost = jobControl.UrlHost;
            this.UrlReferencia = jobControl.UrlReferencia;
            this.StatusAnterior = jobControl.StatusAnterior;
            this.StatusNovo = jobControl.StatusNovo;
            this.ParametrosExibicao = jobControl.ParametrosExibicao;
            this.FileUpload = jobControl.FileUpload;
            this.FileDownload = jobControl.FileDownload;
            this.NomeGrupo = jobControl.Usuario;
            this.Reprocessar = jobControl.Reprocessar;
            this.PermiteCancelar = jobControl.PermiteCancelar;
            this.Inchora = jobControl.Inchora;
        }

        /// <summary>
        /// Campo para descrever o Job
        /// </summary>
        public string DescricaoJob { get; set; }
        /// <summary>
        /// Campo para controlar o grupo que recebera o retorno (signalr), inicialmente é o login do usuário
        /// </summary>
        public string NomeGrupo { get; set; }
        /// <summary>
        /// Id da task (Id do Hangfire)
        /// </summary>
        public string IdTask { get; set; }
        /// <summary>
        /// Progresso, de 0 a 100 (% do progresso), -1 Aguardando, -2 Job Criado, -3 Warning, -8 Cancelado/Deletado e -9 Erro
        /// </summary>
        public int Progress { get; set; }
        /// <summary>
        /// Mensagem a ser exibida junto com a barra de progresso, ou no caso de concluído, mensagem de conclusão.
        /// </summary>
        public string Mensagem { get; set; }
        /// <summary>
        /// Url de onde partiu a requisição no usuário (no caso de produção normalmente seria prd.pontofopag.com.br)
        /// </summary>
        public string UrlHost { get; set; }
        /// <summary>
        /// Url onde o job foi chamado, para que possa ser exibido o status do job apenas na url que originou a ação.
        /// </summary>
        public string UrlReferencia { get; set; }
        /// <summary>
        /// Status Anterior do Job
        /// </summary>
        public string StatusAnterior { get; set; }
        /// <summary>
        /// Status Atual do Job
        /// </summary>
        public string StatusNovo { get; set; }
        public string ParametrosExibicao { get; set; }
        public string FileUpload { get; set; }
        public string FileDownload { get; set; }
        public bool Reprocessar { get; set; }
        public bool PermiteCancelar { get; set; }

        public DateTime Inchora { get; set; }
        public string InchoraStr { get { return Inchora.ToString("dd/MM/yyyy HH:mm:ss"); } }

        public static List<PxyJobReturn> JobControlToPxyJobReturn(List<JobControl> jobsControl)
        {
            List<Modelo.Proxy.PxyJobReturn> lista = new List<Modelo.Proxy.PxyJobReturn>();
            jobsControl.ForEach(f => lista.Add(new Modelo.Proxy.PxyJobReturn(f)));
            return lista;
        }
    }
}