using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Text;
using System.Data.SqlClient;
using DAL.SQL;

namespace BLL
{
    public class Parametros : IBLL<Modelo.Parametros>
    {
        string conexao, arquivoOriginal;
        string[] arquivoOriginalList;
        DAL.SQL.Parametros dalParametros;
        private BLL.Backup bllBackup;
        private string ConnectionString;
        private Modelo.Cw_Usuario UsuarioLogado;

        public Parametros() : this(null)
        {
            
        }

        public Parametros(string connString)
            : this(connString, cwkControleUsuario.Facade.getUsuarioLogado)
        {

        }

        public Parametros(string connString, Modelo.Cw_Usuario usuarioLogado)
        {
            if (!String.IsNullOrEmpty(connString))
                ConnectionString = connString;
            else
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;

            dalParametros = new DAL.SQL.Parametros(new DataBase(ConnectionString));
            bllBackup = new BLL.Backup(ConnectionString);
            dalParametros.UsuarioLogado = usuarioLogado;
            UsuarioLogado = usuarioLogado;
        }

        public int MaxCodigo()
        {
            return dalParametros.MaxCodigo();
        }

        public DataTable GetAll()
        {
            return dalParametros.GetAll();
        }

        public List<Modelo.Parametros> GetAllList(List<int> ids)
        {
            return dalParametros.GetAllList(ids);
        }

        public List<Modelo.Parametros> GetAllList()
        {
            return dalParametros.GetAllList();
        }

        public Modelo.Parametros LoadObject(int id)
        {
            return dalParametros.LoadObject(id);
        }

        public Modelo.Parametros LoadPrimeiro()
        {
            return dalParametros.LoadPrimeiro();
        }

        public Dictionary<string, string> ValidaObjeto(Modelo.Parametros objeto)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            if (objeto.Codigo == 0)
            {
                ret.Add("txtCodigo", "Campo obrigatório.");
            }

            if (String.IsNullOrEmpty(objeto.Descricao))
            {
                ret.Add("txtDescricao", "Campo obrigatório.");
            }

            if (objeto.DiaFechamentoInicial < 0)
            {
                ret.Add("DataInicial", "A data inicial deve ser maior que zero(0).");
            }
            else if (objeto.DiaFechamentoInicial > 31)
            {
                ret.Add("DataInicial", "A data não pode ser maior do que trinta(30).");
            }
            else if (objeto.DiaFechamentoInicial == 0 && objeto.DiaFechamentoFinal != 0)
            {
                ret.Add("DataInicial", "Não é possível gravar apenas uma das datas com o valor zero(0).");
            }

            if (objeto.DiaFechamentoFinal < 0)
            {
                ret.Add("DataFinal", "A data final deve ser maior que zero(0).");
            }
            else if (objeto.DiaFechamentoFinal > 31)
            {
                ret.Add("DataFinal", "A data não pode ser maior do que trinta(30).");
            }
            else if (objeto.DiaFechamentoInicial != 0 && objeto.DiaFechamentoFinal == 0)
            {
                ret.Add("DataFinal", "Não é possível gravar apenas uma das datas com o valor zero(0).");
            }

            return ret;
        }

        public Dictionary<string, string> Salvar(Modelo.Acao pAcao, Modelo.Parametros objeto)
        {
            Dictionary<string, string> erros = ValidaObjeto(objeto);
            if (erros.Count == 0)
            {
                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        dalParametros.Incluir(objeto);
                        break;
                    case Modelo.Acao.Alterar:
                        dalParametros.Alterar(objeto);
                        break;
                    case Modelo.Acao.Excluir:
                        dalParametros.Excluir(objeto);
                        break;
                }
            }
            return erros;
        }

        /// <summary>
        /// Método responsável em retornar o id da tabela. O campo padrão para busca é o campo código, podendo
        /// utilizar o parametro pCampo e pValor2 para utilizar mais um campo na busca
        /// OBS: Caso não desejar utilizar um segundo campo na busca passar "null" nos parametros pCampo e pValor
        /// </summary>
        /// <param name="pValor">Valor do campo Código</param>
        /// <param name="pCampo">Nome do segundo campo que será utilizado na buscao</param>
        /// <param name="pValor2">Valor do segundo campo (INT)</param>
        /// <returns>Retorna o ID</returns>
        public int getId(int pValor, string pCampo, int? pValor2)
        {
            return dalParametros.getId(pValor, pCampo, pValor2);
        }

        public void Backup(int pentradasaida)
        {
            Modelo.Parametros objparametro = LoadPrimeiro();

            if ((pentradasaida == 1 && objparametro.FazerBackupEntrada == 1)
                || (pentradasaida == 2 && objparametro.FazerBackupSaida == 1)
                || pentradasaida == 3)
            {
                DirectoryInfo dir = new DirectoryInfo(Modelo.cwkGlobal.DirApp + "\\Backup\\");
                if (!dir.Exists)
                {
                    dir.Create();
                }

                if (Modelo.cwkGlobal.BD == 1)
                {
                    BackupSqlServer(objparametro, dir);
                }
                else if (Modelo.cwkGlobal.BD == 2)
                {
                    BackupFirebird(objparametro, dir);
                }
            }
        }

        private void BackupSqlServer(Modelo.Parametros objparametro, DirectoryInfo dir)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Modelo.cwkGlobal.CONN_STRING);
            string nome, nomeZip;
            nome = "cwkPontoMT_" + GeraNomeBackup("bak", objparametro);
            nomeZip = "cwkPontoMT_" + GeraNomeBackup("ZIP", objparametro);
            //Backup local
            if (builder.DataSource.Split('\\')[0] == ".")
            {
                string caminhoZip = dir.ToString() + nomeZip;
                string caminhoArquivo = dir.ToString() + nome;
                if (File.Exists(caminhoArquivo))
                {
                    File.Delete(caminhoArquivo);
                }

                dalParametros.GerarBackupBanco(builder.InitialCatalog, caminhoArquivo);
                Compacta(objparametro, caminhoArquivo, caminhoZip);
                File.Delete(caminhoArquivo);
                FileInfo arquivo = new FileInfo(caminhoZip);
                CopiaBackup(nomeZip, arquivo);
            }
            //Backup via rede no servidor de banco de dados
            else
            {
                string caminhoServidor, caminhoCompartilhamento;
                BLL.ConfigServidor.CarregaCaminhos(out caminhoServidor, out caminhoCompartilhamento);
                //Os caminhos estão configurados
                if (!String.IsNullOrEmpty(caminhoServidor) && !String.IsNullOrEmpty(caminhoCompartilhamento))
                {
                    string caminhoArquivo = caminhoServidor + "\\" + nome;
                    string caminhoZip = caminhoCompartilhamento + "\\" + nomeZip;
                    string caminhoBak = caminhoCompartilhamento + "\\" + nome;
                    //Para fazer o backup, tem que usar o caminho completo da pasta de backup no servidor
                    dalParametros.GerarBackupBanco(builder.InitialCatalog, caminhoArquivo);
                    //Para compactar tem que colocar o caminho da pasta de backup mapeada
                    Compacta(objparametro, caminhoBak, caminhoZip);
                    File.Delete(caminhoBak);
                    FileInfo arquivo = new FileInfo(caminhoZip);
                    CopiaBackup(nomeZip, arquivo);
                }
            }
        }

        private void BackupFirebird(Modelo.Parametros objparametro, DirectoryInfo dir)
        {
            string nome;
            string[] CONN_STRING;
            string[] nomefdb;
            XmlDocument doc = new XmlDocument();
            doc.Load(Modelo.cwkGlobal.DirApp + "\\DAL.xml");
            XmlNode no = doc.SelectSingleNode("DAL").SelectSingleNode("cwkConnectionString").SelectSingleNode("Fb");

            CONN_STRING = no.Attributes["connectionString"].Value.ToString().Split(';');

            for (int i = 0; i <= CONN_STRING.Length; i++)
            {
                if (CONN_STRING[i].Substring(0, 8) != "Database")
                {
                    continue;
                }

                nomefdb = CONN_STRING[i].Substring(9, CONN_STRING[i].Length - 9).ToString().Split('\\');
                conexao = Modelo.cwkGlobal.DirApp + "\\Dados\\" + nomefdb[nomefdb.Length - 1];
                break;
            }

            nome = GeraNomeBackup("ZIP", objparametro);

            AuxBackupFirebird(objparametro, nome, dir, conexao);
        }

        public void AuxBackupFirebird(Modelo.Parametros objparametro, string nome, DirectoryInfo dir, string conexao)
        {
            FileInfo arquivo = new FileInfo(dir + nome);
            int tamanhoArquivoOriginal;

            arquivoOriginalList = conexao.Split('\\');
            tamanhoArquivoOriginal = arquivoOriginalList.Length;

            for (int i = 0; i <= tamanhoArquivoOriginal; i++)
            {
                if (i == tamanhoArquivoOriginal)
                {
                    arquivoOriginal = arquivoOriginalList[i - 1];
                }
            }

            FileInfo ArquivoCopia = new FileInfo(conexao);

            string DiretorioCopia = dir.ToString();
            string Destino;

            ArquivoCopia.CopyTo(DiretorioCopia + "\\" + arquivoOriginal, true);

            conexao = "";
            conexao = DiretorioCopia + arquivoOriginal;
            conexao = "\"" + conexao + "\"";
            Destino = "\"" + dir + nome + "\"";

            Compacta(objparametro, conexao, Destino);

            ArquivoCopia = new FileInfo(conexao.Replace('"', ' '));
            ArquivoCopia.Delete();

            CopiaBackup(nome, arquivo);
        }

        private static void Compacta(Modelo.Parametros objparametro, string origem, string destino)
        {
            ProcessStartInfo bak = new ProcessStartInfo();
            bak.FileName = "7za.exe";
            bak.Arguments = "a -tzip \"" + destino + "\" \"" + origem + "\" -mx=5";
            Process x = Process.Start(bak);
            x.WaitForExit();

            #region Método Antigo
            //Process bak;
            //if (objparametro.TipoCompactador == 0)
            //{
            //    bak = Process.Start(@"C:\Arquivos de programas\WinZip\WINZIP32.EXE", " -min -a -r \"" + destino + "\" \"" + origem + "\"");
            //}
            //else if (objparametro.TipoCompactador == 1)
            //{
            //    bak = Process.Start(@"C:\Arquivos de programas\WinRAR\WinRAR.exe", " a \"" + destino + "\" \"" + origem + "\"");
            //}
            //else if (objparametro.TipoCompactador == 2)
            //{
            //    bak = Process.Start(@"C:\Arquivos de programas\BraZip\BraZip.exe", " A \"" + destino + "\" \"" + origem + "\"");
            //}
            //else if (objparametro.TipoCompactador == 3)
            //{
            //    bak = Process.Start(@"C:\Arquivos de programas\Filzip\Filzip.exe", " -a -rp \"" + destino + "\" \"" + origem + "\"");
            //}
            //else
            //{
            //    throw new Exception("Não foi selecionado nenhum tipo de compactador!");
            //}
            //bak.WaitForExit();
            #endregion
        }

        private void CopiaBackup(string nome, FileInfo arquivo)
        {
            Modelo.Backup objBackup = new Modelo.Backup();
            List<Modelo.Backup> listaBackup;
            listaBackup = bllBackup.GetAllList();
            foreach (Modelo.Backup backup in listaBackup)
            {
                if (backup.Diretorio != null)
                {
                    arquivo.CopyTo(backup.Diretorio + @"\" + nome, true);
                }
            }
        }

        private static string GeraNomeBackup(string extensao, Modelo.Parametros parametros)
        {
            if (parametros.ArquivoBackup == 1)
            {
                return "Backup_" + Modelo.cwkFuncoes.DiaSemana(DateTime.Now, Modelo.cwkFuncoes.TipoDiaSemana.Completo) + "." + extensao;
            }
            else
            {
                if (DateTime.Now.Day > 9)
                {
                    if (DateTime.Now.Month < 9)
                    {
                        return ("Backup_" + DateTime.Now.Day.ToString() + "-" + "0" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "." + extensao);
                    }
                    else
                    {
                        return ("Backup_" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "." + extensao);
                    }
                }
                else
                {
                    if (DateTime.Now.Month < 9)
                    {
                        return ("Backup_" + "0" + DateTime.Now.Day.ToString() + "-" + "0" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "." + extensao);
                    }
                    else
                    {
                        return ("Backup_" + "0" + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "." + extensao);
                    }
                }
            }
        }

        public void AtualizaTipoExtraFaltaMarcacoes(int id, Int16 tipohoraextrafalta, DateTime? dataInicial, DateTime? dataFinal)
        {
            dalParametros.AtualizaTipoExtraFaltaMarcacoes(id, tipohoraextrafalta, dataInicial, dataFinal);
        }

        public int GetExportaValueZerado()
        {
            return dalParametros.GetExportaValorZerado();
        }

        public int? GetIdPorCod(int Cod)
        {
            return dalParametros.GetIdPorCod(Cod);
        }

        public bool Flg_Separar_Trabalhadas_Noturna_Extras_Noturna(int idfuncionario)
        {
            return dalParametros.Flg_Separar_Trabalhadas_Noturna_Extras_Noturna(idfuncionario);
        }

        public bool Flg_Estender_Periodo_Noturno(int idfuncionario)
        {
            return dalParametros.Flg_Estender_Periodo_Noturno(idfuncionario);
        }

    }
}
