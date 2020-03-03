using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Modelo.Proxy;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BLL_N.JobManager.Hangfire;
using Modelo.EntityFramework.MonitorPontofopag;

namespace BLL_N.Hubs
{
    public class NotificationHub : Hub
    {
        public override Task OnConnected()
        {
            string name = UsuarioConectado();
            Groups.Add(Context.ConnectionId, name);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = UsuarioConectado();
            Groups.Remove(Context.ConnectionId, name);
            if (stopCalled)
            {
                Debug.WriteLine(String.Format("Cliente {0}(id= {1}) fechou a conexão.", name, Context.ConnectionId));
            }
            else
            {
                Debug.WriteLine(String.Format("Cliente {0}(id= {1}) time out.", name, Context.ConnectionId));
            }
            return base.OnDisconnected(stopCalled);
        }

        private string UsuarioConectado()
        {
            return Context.User == null ? "TaskPontofopag" : Context.User.Identity.Name;
        }

        public void TaskComplete(PxyJobReturn jobReturn)
        {
            Clients.Group(ValidaGrupo(jobReturn.NomeGrupo)).taskComplete(jobReturn);
        }

        public void TaskProgress(PxyJobReturn jobReturn)
        {
            Clients.Group(ValidaGrupo(jobReturn.NomeGrupo)).taskProgress(jobReturn);
        }

        private string ValidaGrupo(string nomeGrupo)
        {
            string userIdentity = Context.User == null ? "JobManager" : Context.User.Identity.Name;
            nomeGrupo = string.IsNullOrEmpty(nomeGrupo) ? userIdentity : nomeGrupo;
            return nomeGrupo;
        }

        public static void ReportarJobCompleto(PxyJobReturn job)
        {
            try
            {
                ReportarJobProgresso(job);
            }
            catch (Exception)
            {
            }

            try
            {
                sendMensageHub(job.UrlHost, "NotificationHub", "TaskComplete", job);
            }
            catch (Exception ex)
            {
            }
        }

        public static void ReportarJobProgresso(JobControl jobControl)
        {
            PxyJobReturn jobReturn = new PxyJobReturn(jobControl);
            ReportarJobProgresso(jobReturn);
        }

        public static void ReportarJobProgresso(PxyJobReturn job)
        {
            try
            {
                if (job.Progress < 0 || job.Progress == 100) // Regra para não persistir o progresso, apenas termino e ou erros, pois podem ser muitos por segundo
                {
                    JobControl ret = JobControlManager.ProgressUpdate(job.IdTask, job.Mensagem, job.Progress);
                    job = new PxyJobReturn(ret);
                }
                sendMensageHub(job.UrlHost, "NotificationHub", "TaskProgress", job);
            }
            catch (Exception ex)
            {
            }
        }

        public static async void sendMensageHub(string host, string hubNome, string metodo, params object[] args)
        {
            using (HubConnection hubConnection = new HubConnection(host))
            {
                IHubProxy hub = hubConnection.CreateHubProxy(hubNome);
                await hubConnection.Start();
                await hub.Invoke(metodo, args);
            }
        }
    }
}
