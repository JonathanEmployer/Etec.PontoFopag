using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cwkAtualizadorArquivosComunicadorWebAPIPontoWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Iniciando Atualização do Pontofopag Comunicador...");
            Console.WriteLine("Por Favor Aguarde!");
            //Delay para ter certeza que a atualização so começe após o comunicador estiver fechado.
            System.Threading.Thread.Sleep(1000);
            //Diretorio do atualizador
            string dirApp = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //Diretorio onde está a versão a ser atualizada
            string dirAtualizar = Path.Combine(dirApp, "Atualizar");
            //Diretório onde será realizado o Backup
            string dirBackup = Path.Combine(dirApp, "BkpVersoes");
            //Verifica se existe versão a ser atualizada.
            if (System.IO.Directory.GetDirectories(dirAtualizar).Length > 0)
            {
                try
                {
                    DirectoryInfo dirInfo;
                    string DiretorioComunicador;
                    string versaoComunicador, pathComunicador;
                    GetVersaoComunicador(dirApp, out dirInfo, out DiretorioComunicador, out versaoComunicador, out pathComunicador);
                    Console.WriteLine("Versão Atual: "+versaoComunicador);
                    string DiretorioRoot = dirInfo.Parent.Parent.FullName;
                    // Separa diretórios que serão realizados backup
                    List<string> DiretoriosBackup = new List<string>();
                    DiretoriosBackup.Add(DiretorioComunicador);
                    Console.WriteLine("Iniciando Backup dos Arquivos... (Diretório: " + DiretorioComunicador + ")");
                    foreach (string dirPath in Directory.GetDirectories(DiretorioComunicador, "*", SearchOption.AllDirectories))
                    {
                        // SubDiretorios que entram no backup
                        if (!((dirPath.Contains("\\Logs")) || (dirPath.Contains("\\Release")) || (dirPath.Contains("\\Atualizacoes"))))
                        {
                            DiretoriosBackup.Add(dirPath);
                        }
                    }

                    try
                    {
                        //Cria Backup do Comunicador antes de substituir os arquivos
                        CriarBackup(dirBackup, versaoComunicador, DiretorioRoot, DiretoriosBackup);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception ("Erro ao Criar Backup, Erro: "+ex.Message);
                    }

                    //Pega diretório da versão disponível.
                    List<string> versoesDisponiveis = new List<string>(Directory.EnumerateDirectories(dirAtualizar));
                    var VersaoAtualizarDir = versoesDisponiveis.OrderBy(x => x).LastOrDefault();
                    int pos = VersaoAtualizarDir.LastIndexOf("\\")+1;
                    string versao = VersaoAtualizarDir.Substring(pos, VersaoAtualizarDir.Length-pos);
                    //Copia/Adiciona os Arquivos/Pastas da nova versão para pasta do comunicador.
                    Console.WriteLine("Atualizando Arquivos do Sistema para Versão: " + versao + "...");
                    CopiarDiretoriosEArquivos(VersaoAtualizarDir, DiretorioComunicador);
                    Console.WriteLine("Pontofopag Comunicador Atualizado com Sucesso para a Versão: " + versao + ".");
                    Console.WriteLine("Iniciando Pontofopag Comunicador " + versao + ".");
                    Process.Start(pathComunicador);
                }
                catch (UnauthorizedAccessException UAEx)
                {
                    throw UAEx;
                }
                catch (PathTooLongException PathEx)
                {
                    throw PathEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            } 
        }

        private static void CriarBackup(string dirBackup, string versaoComunicador, string DiretorioRoot, List<string> DiretoriosBackup)
        {
            //Apaga Backups antigos.
            Console.WriteLine("Procurando por Backup's Antigos...");
            string[] backupsAntigos = Directory.GetFiles(dirBackup, "*.zip");
            for (int i = 0; i < backupsAntigos.Count() - 1; i++)
            {
                Console.WriteLine("Removendo Backup Antigo " + backupsAntigos.OrderBy(x => x).ToList()[i]);
                File.Delete(backupsAntigos.OrderBy(x => x).ToList()[i]);
            }
            Console.WriteLine("Criando Backup dos Arquivos... (Salvando em: " + dirBackup + ")");
            //Cria Zip do Backup
            using (var zip = new Ionic.Zip.ZipFile())
            {
                //Percorre os diretórios que serão realizados backups
                foreach (string dirArqBackup in DiretoriosBackup)
                {
                    //Percorre os arquivos dos diretórios que serão realizados backups
                    foreach (string arquivo in Directory.GetFiles(dirArqBackup, "*.*"))
                    {
                        //Pega a estrutura de pasta do root até onde esta o arquivo
                        string nomedir = dirArqBackup.Replace(DiretorioRoot + "\\", "");
                        //Adiciona o arquino no zip
                        Console.WriteLine("Criando Backup do Arquivo: "+arquivo);
                        zip.AddFile(arquivo, nomedir);
                    }
                }
                Console.WriteLine("Salvando Backup dos Arquivos...");
                //Salva o Backup no formato zip
                zip.Save(Path.Combine(dirBackup, versaoComunicador + ".zip"));
            }
        }

        private static void GetVersaoComunicador(string dirApp, out DirectoryInfo di, out string DiretorioComunicador, out string versaoComunicador, out string pathComunicador)
        {
            //Pega versão do comunicador
            di = new DirectoryInfo(dirApp);
            DiretorioComunicador = di.Parent.FullName;
            String[] PossiveisExeComunicador = Directory.GetFiles(DiretorioComunicador, "*.exe");
            pathComunicador = PossiveisExeComunicador.Where(x => x.Contains("Comunicador")).FirstOrDefault();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(pathComunicador);
            versaoComunicador = fvi.FileVersion;
        }

        public static void CopiarDiretoriosEArquivos(string SourcePath, string DestinationPath)
        {
            CopiaEstruturaDiretorios(SourcePath, DestinationPath);
            MoverArquivos(SourcePath, DestinationPath);
        }

        public static void MoverArquivos(string SourcePath, string DestinationPath)
        {
            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                string dirCopiar = newPath.Replace(SourcePath + "\\", "");
                string dirNovo = Path.Combine(DestinationPath, dirCopiar);
                File.Copy(newPath, dirNovo, true);
            }
        }

        private static void CopiaEstruturaDiretorios(string SourcePath, string DestinationPath)
        {
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",SearchOption.AllDirectories))
            {
                //Extrai o diretório a ser copiado
                string dirCopiar = dirPath.Replace(SourcePath+"\\", "");
                // Adiciona o novo diretório na pasta de destino
                string dirNovo = Path.Combine(DestinationPath, dirCopiar);
                if (!System.IO.Directory.Exists(dirNovo))
                {
                    //Caso ainda não exista cria o diretório
                    Directory.CreateDirectory(dirNovo);
                }
            }
        }
    }
}
