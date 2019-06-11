using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace BLL
{
    public class ArquivoBLL
    {
        private string _pathRelatorios;
        public string PathRelatorios { get { return _pathRelatorios; } set { _pathRelatorios = value; } }
        protected Modelo.UsuarioPontoWeb _usuario;
        protected Modelo.ProgressBar _progressBar;

        public ArquivoBLL(Modelo.UsuarioPontoWeb usuario, Modelo.ProgressBar progressBar)
        {
            _pathRelatorios = ConfigurationManager.AppSettings["ArquivosPontofopag"];
            if (String.IsNullOrEmpty(_pathRelatorios))
                throw new Exception("O patch(Caminho) para salvar os relatório não foi informado, informe no arquivo de configuração o valor da variavel PathRelatorios");

            if (String.IsNullOrEmpty(usuario.DataBase))
                throw new Exception("Nome do banco de dados não encontrado");

            _pathRelatorios = Path.Combine(_pathRelatorios, usuario.DataBase.Contains("_") ? usuario.DataBase.Split('_')[1] : usuario.DataBase);
            _pathRelatorios = Path.Combine(_pathRelatorios, "Arquivos");
            _usuario = usuario;
            _progressBar = progressBar;
        }

        public string SaveFile(string NomeArquivo, string fileNameExtension, byte[] renderedBytes)
        {
            if (string.IsNullOrEmpty(NomeArquivo))
                throw new Exception("Nome do arquivo não foi informado");
            string caminho = _pathRelatorios;
            if (!Directory.Exists(caminho))
            {
                Directory.CreateDirectory(caminho);
            }
            NomeArquivo += String.Format("_c{0}", DateTime.Now.ToString("ddMMyyyy_HHmmss"));
            caminho = Path.Combine(caminho, NomeArquivo + "." + fileNameExtension);
            using (FileStream fs = new FileStream(caminho, FileMode.Create))
            {
                fs.Write(renderedBytes, 0, renderedBytes.Length);
            }

            return caminho;
        }

        protected object ExecuteMethodThredCancellation<T>(Func<T> funcToRun)
        {
            object retorno = null;
            var thread = new Thread(() => { retorno = funcToRun(); });
            thread.Start();

            while (!thread.Join(TimeSpan.FromSeconds(2)))
            {
                try
                {
                    _progressBar.validaCancelationToken();
                }
                catch (OperationCanceledException)
                {
                    thread.Abort();
                    throw;
                }
            }
            return retorno;
        }
    }
}
