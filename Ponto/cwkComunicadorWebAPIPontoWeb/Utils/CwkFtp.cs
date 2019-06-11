using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace cwkComunicadorWebAPIPontoWeb.Utils
{
    public class CwkFtp
    {
        /// <summary>
        /// Classe facilitadora para utilização de FTP
        /// </summary>
        /// <param name="_servidor">Endereço do FTP Ex: 187.95.114.40 ou ftp.cwork.com.br (Não deve ser adicionado o ftp:// no início e / no final.)</param>
        /// <param name="_usuario">Informar o usuário do FTP, caso não seja informado será utilizado o "usuário" anonymous</param>
        /// <param name="_senha">Informar a senha do FTP</param>
        public CwkFtp(string _servidor, string _usuario, string _senha)
        {
            servidor = "Ftp://"+_servidor;
            usuario = _usuario == null ? "anonymous" : _usuario;
            senha = _senha == null ? "" : _senha;
            diretorio = "";
        }

        /// <summary>
        /// Classe facilitadora para utilização de FTP, utilizada para autenticação anônima (usuário anonymous e sem senha).
        /// </summary>
        public CwkFtp(string _servidor)
        {
            servidor = "Ftp://" + _servidor;
            usuario = "anonymous";
            senha = "";
            diretorio = "";
        }

        private string servidor { get; set; }
        private string usuario { get; set; }
        private string senha { get; set; }
        private string diretorio { get; set; }
        /// <summary>
        /// Inicializa o FTP com o método que será utilizado Ex: DeleteFile, DownloadFile, ListDirectory, ListDirectoryDetails...
        /// </summary>
        /// <param name="metodo">Utilizar WebRequestMethods.Ftp Ex: WebRequestMethods.Ftp.ListDirectoryDetails</param>
        /// <returns>Retorna um FtpWebRequest</returns>
        public FtpWebRequest InicializaFTP(string metodo)
        {
            FtpWebRequest requisicao = (FtpWebRequest)FtpWebRequest.Create(new Uri(servidor +"/"+ diretorio));
            requisicao.Credentials = new NetworkCredential(usuario, senha);
            requisicao.Method = metodo.ToString();
            requisicao.UseBinary = true;
            requisicao.UsePassive = true;
            requisicao.KeepAlive = false;
            return requisicao;
        }

        /// <summary>
        /// Retorna para o diretório raiz do FTP, Ex: Se estiver em ftp://ftp.cwork.com.br/Versoes/Arquivos volta para ftp://ftp.cwork.com.br
        /// </summary>
        public void DiretorioRaiz()
        {
            diretorio = "";
        }

        /// <summary>
        /// Desce um nivel no diretório do FTP, Ex: Se estiver em ftp://ftp.cwork.com.br/Versoes e quiser ir para o subdiretório Arquivos passar a String "Arquivos"
        /// </summary>
        /// <param name="subDiretorio">Informar o Subdiretório que deseja acessar</param>
        public void SubDiretorio(String subDiretorio)
        {
            diretorio = diretorio + "/" + subDiretorio;
        }

        /// <summary>
        /// Volta um nivel no diretório, Ex: Se estiver em ftp://ftp.cwork.com.br/Versoes/Arquivos volta para ftp://ftp.cwork.com.br/Versoes
        /// </summary>
        public void DiretorioAnterior()
        {
            String[] diretorios = diretorio.Split('/');
            String dirAnterior = "";
            for (int i = 0; i < diretorios.Count()-1; i++)
            {
                if (i < diretorios.Count() - 2)
                {
                    dirAnterior += diretorios[i] + "/";
                }
                else
                {
                    dirAnterior += diretorios[i];
                }
            }
            diretorio = dirAnterior;
        }

        /// <summary>
        /// Lista de String com as informações do arquivo/pasta de um diretório especificado ou do diretório corrente.
        /// </summary>
        /// <param name="diretorio"> Informa o diretório que deseja receber as informações Ex: "Versoes".
        /// Se informar string vazia ("") retorna os dados do diretório principal do FTP.
        /// poder ser passado subdiretórios Ex: "Versoes/Arquivos".
        /// ** Ao informar o parâmetro diferente de null, o diretório corrente passará a ser o informado.
        /// </param>
        /// <returns> Retorna uma lista de string contendo em cada linha as informações do arquivo/pasta.</returns>
        public List<string> ListaDadosDiretarioDetalhado(string diretorio)
        {
            this.diretorio = diretorio;
            return ListaDadosDiretarioDetalhado();
        }
        public List<string> ListaDadosDiretarioDetalhado()
        {
            FtpWebRequest request = InicializaFTP(WebRequestMethods.Ftp.ListDirectoryDetails);
            FtpWebResponse response = request.GetResponse() as FtpWebResponse;
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string str = reader.ReadLine();
            List<string> strList = new List<string>();
            while (str != null)
            {
                strList.Add(str);
                str = reader.ReadLine();
            }
            reader.Close();
            responseStream.Close();
            response.Close();
            return strList;
        }

        /// <summary>
        /// Lista de String com as informações do arquivo/pasta de um diretório especificado ou do diretório corrente.
        /// </summary>
        /// <returns> Retorna uma lista de string contendo em cada linha as informações do arquivo/pasta.</returns>
        public IList<InfoArquivoFtp> ListaDadosDiretorioDetalhado()
        {
            List<string> strList = ListaDadosDiretarioDetalhado();

            return ConverterListaStringParaInfoArquivoFtp(strList);
        }

        /// <summary>
        /// Lista de objetos InfoArquivoFtp com as informações do arquivo/pasta de um diretório especificado ou do diretório corrente.
        /// </summary>
        /// <param name="diretorio"> Informa o diretório que deseja receber as informações Ex: "Versoes".
        /// Se informar string vazia ("") retorna os dados do diretório principal do FTP.
        /// poder ser passado subdiretórios Ex: "Versoes/Arquivos".
        /// ** Ao informar o parâmetro diferente de null, o diretório corrente passará a ser o informado.
        /// </param>
        /// <returns> Retorna uma lista de Objetos do tipo InfoArquivoFtp contendo em cada linha as informações do arquivo/pasta.</returns>
        /// 
        public IList<InfoArquivoFtp> ListaDadosDiretorioDetalhado(string diretorio)
        {
            List<string> strList = ListaDadosDiretarioDetalhado(diretorio);

            return ConverterListaStringParaInfoArquivoFtp(strList);
        }

        /// <summary>
        /// Lista de objetos InfoArquivoFtp com as informações do arquivo/pasta de um diretório especificado ou do diretório corrente.
        /// </summary>
        /// <returns> Retorna uma lista de Objetos do tipo InfoArquivoFtp contendo em cada linha as informações do arquivo/pasta.</returns>
        /// 
        private IList<InfoArquivoFtp> ConverterListaStringParaInfoArquivoFtp(List<string> strList)
        {
            IList<InfoArquivoFtp> DadosDiretorio = new List<InfoArquivoFtp>();
            foreach (var item in strList)
            {
                DadosDiretorio.Add(Parse(item.ToString()));
            }
            return DadosDiretorio;
        }

        public Stream DownloadArquivo(out int tamanhoArquivo)
        {
            FtpWebRequest request = InicializaFTP(WebRequestMethods.Ftp.DownloadFile);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            tamanhoArquivo = (int)response.ContentLength;
            Stream retorno = response.GetResponseStream();
            return retorno;
        }

        // Exemplo de Retorno: -rw-r--r-- 1 user group 7452 Jun 2 2008 myfileName.html
        private Regex unixStyle = new Regex(@"^(?<dir>[-dl])(?<ownerSec>[-r][-w][-x])(?<groupSec>[-r][-w][-x])(?<everyoneSec>[-r][-w][-x])\s+(?:\d)\s+(?<owner>\w+)\s+(?<group>\w+)\s+(?<size>\d+)\s+(?<month>\w+)\s+(?<day>\d{1,2})\s+(?<hour>\d{1,2}):(?<minutes>\d{1,2})\s+(?<name>.*)$", RegexOptions.IgnoreCase);

        // Exemplo de Retorno: 28/07/2010 03:18 PM 937 myfileName.html
        private Regex winStyle = new Regex(@"^(?<month>\d{1,2})-(?<day>\d{1,2})-(?<year>\d{1,2})\s+(?<hour>\d{1,2}):(?<minutes>\d{1,2})(?<ampm>am|pm)\s+(?<dir>[<]dir[>])?\s+(?<size>\d+)?\s+(?<name>.*)$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Recebe um string com informações do arquivo/pasta do FTP e convert em um objeto do tipo InfoArquivoFtp
        /// </summary>
        /// <param name="line">Recebe uma linha com as informações do arquivo.
        /// pode ser no padrão Windows (28/07/2010 03:18 PM 937 myfileName.html)
        /// ou Unix (-rw-r--r-- 1 user group 7452 Jun 2 2008 myfileName.html)</param>
        /// <returns>Retorno um objeto do tipo InfoArquivoFtp com as informações do arquivo</returns>
        public InfoArquivoFtp Parse(string line)
        {

            Match match = unixStyle.Match(line);

            if (match.Success)
            {

                return ParseMatch(match.Groups, ListStyle.Unix);

            }

            match = winStyle.Match(line);

            if (match.Success)
            {

                return ParseMatch(match.Groups, ListStyle.Windows);

            }

            throw new Exception("Invalid line format");

        }
        /// <summary>
        /// Desmembra os as informação do arquivo de um string para o objeto InfoArquivoFtp de acordo com o Regex e do tipo do FTP (Unix ou Windows)
        /// </summary>
        /// <param name="matchGroups">Match do Regex responsável por separar os dados da string.</param>
        /// <param name="style">Tipo do FTP (Unix ou Windows)</param>
        /// <returns>Retorno as informações do arquivo que estava em uma string em um objeto InfoArquivoFtp</returns>
        private InfoArquivoFtp ParseMatch(GroupCollection matchGroups, ListStyle style)
        {

            string dirMatch = (style == ListStyle.Unix ? "d" : "<dir>");

            InfoArquivoFtp result = new InfoArquivoFtp();

            result.TipoFTP = style;

            result.ehDiretorio = matchGroups["dir"].Value.Equals(dirMatch, StringComparison.InvariantCultureIgnoreCase);

            result.Nome = matchGroups["name"].Value;

            string dataString = matchGroups["day"].Value + '/' + matchGroups["month"].Value + '/' + matchGroups["year"].Value + " " +
                                matchGroups["hour"].Value + ':' + matchGroups["minutes"].Value + ' ' + matchGroups["ampm"].Value;
            DateTime.TryParseExact(dataString, "dd/MM/yy hh:mm tt", CultureInfo.InvariantCulture,
                                      DateTimeStyles.None, out result.DataModificacao);
            
            if (!result.ehDiretorio)

                result.Tamanho = long.Parse(matchGroups["size"].Value);

            return result;
        }
    }



    public enum ListStyle
    {

        Unix,

        Windows

    }

    /// <summary>
    /// Modelo contendo as informações do arquivo/pasta no FTP.
    /// </summary>
    public class InfoArquivoFtp
    {
        /// <summary>
        /// Tipo do FTP (Unix, Windows)
        /// </summary>
        public ListStyle TipoFTP;
        /// <summary>
        /// Nome do arquivo/diretório no FTP
        /// </summary>
        public string Nome;
        /// <summary>
        /// Data da Modificação do arquivo/pasta
        /// </summary>
        public DateTime DataModificacao;
        /// <summary>
        /// Indica se é uma pasta (Diretório) ou arquivo
        /// </summary>
        public bool ehDiretorio;
        /// <summary>
        /// Tamanho do arquivo
        /// </summary>
        public long Tamanho;

    }
}
