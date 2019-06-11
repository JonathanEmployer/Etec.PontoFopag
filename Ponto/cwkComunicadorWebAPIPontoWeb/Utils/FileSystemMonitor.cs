using cwkComunicadorWebAPIPontoWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.Utils
{
    public class FileSystemMonitor<T>
    {
        private FileSystemWatcherWithProgress<T> Watcher;
        public void MonitorarPasta(string Caminho, string TipoArquivo, FileSystemEventHandler LeitorArquivo, IProgress<T> progress)
        {
            try
            {
                if (!Directory.Exists(Caminho))
                {
                    throw new Exception("O Diretório informado não existe.");
                }
                if (String.IsNullOrEmpty(TipoArquivo))
                {
                    throw new Exception("O tipo de arquivo à ser monitorado não foi informado");
                }
                Watcher = new FileSystemWatcherWithProgress<T>(Caminho, progress);
                Watcher.Filter = String.IsNullOrEmpty(TipoArquivo) ? "*.*" : TipoArquivo;
                Watcher.NotifyFilter = NotifyFilters.FileName;
                Watcher.InternalBufferSize = 65536;
                Watcher.Created += new FileSystemEventHandler(LeitorArquivo);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Start()
        {
            if (Watcher == null)
            {
                throw new Exception("O monitor de arquivos não foi iniciado");
            }
            Watcher.EnableRaisingEvents = true;
        }
        public void Stop()
        {
            if (Watcher == null)
            {
                throw new Exception("O monitor de arquivos não foi iniciado");
            }
            Watcher.EnableRaisingEvents = false;
        }
    }
}
