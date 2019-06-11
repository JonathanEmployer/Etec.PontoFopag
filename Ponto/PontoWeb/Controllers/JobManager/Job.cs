using System;
using System.Threading;

namespace PontoWeb.Controllers.JobManager
{
    public class Job
    {
        public event EventHandler<EventArgs> ProgressChanged;
        public event EventHandler<EventArgs> Completed;

        private volatile int _progress;
        private volatile string _msgProgress;
        private volatile bool _completed;
        private volatile string _msgErro;
        private volatile bool _erro;
        private volatile string _msgSucesso;
        private volatile string _msgAviso;

        private Action<Job> _action;

        public string msgSucesso
        {
            get { return _msgSucesso; }
        }

        public string msgAviso
        {
            get { return _msgAviso; }
        }

        private CancellationTokenSource _cancellationTokenSource;
        public bool CompleteNovaAba { get; set; }
        public string TipoArquivo { get; set; }

        public Job(string id)
        {
            Id = id;
            _msgProgress = "Iniciando";
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public Job(Action<Job> action)
            : this(Guid.NewGuid().ToString())
        {            
            _action = action;
        }

        public string Id { get; private set; }

        public string IdHangFire { get; set; }

        public int Progress
        {
            get { return _progress; }
        }

        public string msgProgress
        {
            get { return _msgProgress; }
        }

        public bool IsComplete
        {
            get { return _completed; }
        }

        public string msgErro
        {
            get { return _msgErro; }
        }

        public bool IsErro
        {
            get { return _erro; }
        }

        public CancellationToken CancellationToken
        {
            get { return _cancellationTokenSource.Token; }
        }

        public void Invoke()
        {
            _action.Invoke(this);
        }

        public void ReportProgress(int progress)
        {
            if (_progress != progress)
            {
                _progress = progress;
                OnProgressChanged();
            }
            if (_msgProgress != msgProgress)
            {
                _msgProgress = msgProgress;
                OnProgressChanged();
            }
        }

        public void ReportMsgProgress(string msgProgress)
        {
            if (_msgProgress != msgProgress)
            {
                _msgProgress = msgProgress;
                OnProgressChanged();
            }
        }

        public void ReportComplete()
        {
            if (!IsComplete)
            {
                _completed = true;
                OnCompleted();
            }
        }

        public void ReportError(string msgErro)
        {
            if (!IsErro)
            {
                _erro = true;
                _msgErro = msgErro;
                _completed = true;
                OnCompleted();
            }
        }

        public void ReportSucess(string msgSucess)
        {
            if (!IsErro)
            {
                _erro = false;
                _msgErro = "";
                _msgSucesso = msgSucess;
            }
        }

        public void ReportAviso(string msgAviso)
        {
            if (!IsErro)
            {
                _erro = false;
                _msgErro = "";
                _msgAviso = msgAviso;
            }
        }

        protected virtual void OnCompleted()
        {
            var handler = Completed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnProgressChanged()
        {
            var handler = ProgressChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}