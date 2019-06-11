using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace BLL
{
    public class ExportaArquivos
    {
        private int tipoArquivo; //0 - AFDT , 1 = ACJEF
        private string nomeArquivo;
        private ExportaAFDT arquivoAFDT;
        private ExportaACJEF arquivoACJEF;
        private int idEmpresa;
        private DateTime dataInicial;
        private DateTime dataFinal;
        private ProgressBar exportaPB;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public ExportaArquivos() : this(null)
        {
        }

        public ExportaArquivos(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public ExportaArquivos(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            UsuarioLogado = usuarioLogado;
        }

        /// <summary>
        /// Contrutor da classe com o nome do arquivo
        /// </summary>
        /// <param name="pNomeArquivo">
        /// Deve vir do usuário final com o caminho completo do diretorio 
        /// (É aconselhavel pegar o caminho da janela que foi aberta para o usuário) </param> 
        /// </param>
        /// <param name="tipo_arquivo"> 0 - AFDT , 1 = ACJEF </param>
        /// <param name="pIdEmpresa">Id da Empresa</param>
        /// <param name="pDataInicial">Data inicial</param>
        /// <param name="pDataFinal">Data final</param>
        public ExportaArquivos(string pNomeArquivo, int pTipoArquivo, int pIdEmpresa, DateTime pDataInicial, DateTime pDataFinal, System.Windows.Forms.ProgressBar pExportaPB)
        {
            this.idEmpresa = pIdEmpresa;
            this.dataInicial = pDataInicial;
            this.dataFinal = pDataFinal;
            this.exportaPB = pExportaPB;

            this.tipoArquivo = pTipoArquivo;
            this.nomeArquivo = pNomeArquivo;
            this.efetuaExportacao(nomeArquivo, tipoArquivo, null);
        }

        /// <summary>
        /// Método que chama a Classe correta para exportação do arquivo desejado.
        /// </summary>
        /// <param name="pNomeArquivo">Nome do Arquivo</param>
        /// <param name="pTipoArquivo">Tipo do arquivo => 0 - AFDT, 1 - ACJEF </param>
        private void efetuaExportacao (string pNomeArquivo, int pTipoArquivo, string connString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            switch (pTipoArquivo)
            {
                case 0: 
                    arquivoAFDT = new ExportaAFDT(nomeArquivo, idEmpresa, dataInicial, dataFinal, exportaPB, ConnectionString, UsuarioLogado); 
                    break; 
                case 1:
                    arquivoACJEF = new ExportaACJEF(nomeArquivo, idEmpresa, dataInicial, dataFinal, exportaPB, ConnectionString, UsuarioLogado);
                    break;
                default: break;
            }
        }

        public void efetuaExportacaoWeb(out byte[] arquivoMemoria, int pTipoArquivo, int IDEmpresa, DateTime dtInicio, DateTime dtFim, Modelo.ProgressBar modExportaPB, out string nomeArquivo, string connString, string username)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            nomeArquivo = String.Empty;
            arquivoMemoria = null;
            switch (pTipoArquivo)
            {
                case 0:
                    nomeArquivo = "AFDT"+(username)+".txt";
                    arquivoAFDT = new ExportaAFDT(ref arquivoMemoria, IDEmpresa, dtInicio, dtFim, modExportaPB, ConnectionString, UsuarioLogado);
                    break;
                case 1:
                    nomeArquivo = "ACJEF" + (username) + ".txt";
                    arquivoACJEF = new ExportaACJEF(ref arquivoMemoria, IDEmpresa, dtInicio, dtFim, modExportaPB, ConnectionString, UsuarioLogado);
                    break;
            }
        }
    }
}
