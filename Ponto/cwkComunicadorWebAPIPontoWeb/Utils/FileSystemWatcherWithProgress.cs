using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cwkComunicadorWebAPIPontoWeb.Utils
{
    public class FileSystemWatcherWithProgress<T> : FileSystemWatcher
    {
        public IProgress<T> ProgressObject { get; set; }
        public FileSystemWatcherWithProgress(string path, string filter, IProgress<T> progressObject)
            : base(path, filter)
        {
            ProgressObject = progressObject;
        }

        public FileSystemWatcherWithProgress(string path, IProgress<T> progressObject)
            : base(path)
        {
            ProgressObject = progressObject;
        }

        public FileSystemWatcherWithProgress(IProgress<T> progressObject)
            : base()
        {
            ProgressObject = progressObject;
        }
    }
}
