using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FirebirdSql.Data.Isql;
using FirebirdSql.Data.FirebirdClient;


namespace CopiaIlegal
{
    class Program
    {
        static void Main(string[] args)
        {
            Modelo.Cw_Usuario objUsuario = new Modelo.Cw_Usuario();
            objUsuario.Login = "cwork";
            Modelo.cwkGlobal.objUsuarioLogado = objUsuario;
            Modelo.cwkGlobal.BD = 2;

            Console.WriteLine("Cwork - Cópia Ilegal");
            Console.WriteLine("1 - Criar Log com as marcações manipuladas.");
            Console.WriteLine("2 - Refazer chave das marcações manipuladas e das empresas.");
            Console.WriteLine("Digite o número da opção desejada:");
            string o = Console.ReadLine();
            int opcao = 0;
            if (!Int32.TryParse(o, out opcao))
            {
                Console.WriteLine("Opção incorreta!");
                Console.ReadKey();
                return;
            }

            bool refazer = (opcao == 2);

            //Log das marcações com chave diferente
            List<string> log = new List<string>();
            corrigeBilhetes(log, refazer);
            DAL.FB.Funcionario dalFuncionario = DAL.FB.Funcionario.GetInstancia;
            List<Modelo.Funcionario> funcionarios = dalFuncionario.GetAllList(true);
            StringBuilder str = new StringBuilder();
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                Console.WriteLine("Marcação");
                Console.WriteLine("Aguarde, caso existam muitas marcações este processo pode demorar...");
                Console.WriteLine();
                DAL.FB.Marcacao dalMarcacao = DAL.FB.Marcacao.GetInstancia;
                int qtd = funcionarios.Count, count = 0;
                decimal perido = Math.Round((decimal)qtd / 100);
                FbBatchExecution bath = new FbBatchExecution(conn);

                List<Modelo.Marcacao> marcacoes = null;
                foreach (Modelo.Funcionario func in funcionarios)
                {
                    marcacoes = dalMarcacao.GetPorFuncionario(func.Id);
                    foreach (Modelo.Marcacao marc in marcacoes)
                    {
                        string chave = marc.ToMD5();
                        if (marc.Chave != chave)
                        {
                            log.Add("DsCódigo Funcionario: " + func.Dscodigo + " - Data Marcação: " + marc.Data.ToShortDateString() + " - Id Marcação: " + marc.Id);
                            marc.Chave = chave;
                            str.Remove(0, str.Length);
                            str.Append("UPDATE \"marcacao\" SET \"chave\" = '");
                            str.Append(marc.Chave);
                            str.Append("' WHERE \"marcacao\".\"id\" = ");
                            str.Append(marc.Id);
                            bath.SqlStatements.Add(str.ToString());
                        }
                    }
                    count++;
                    if (count == perido)
                    {
                        Console.Write("|");
                        count = 0;
                    }
                }

                StreamWriter file = new StreamWriter(System.Windows.Forms.Application.StartupPath + "\\logMarcacoesManipuladas.txt", false);
                file.WriteLine("Log de Marcações Manipuladas");
                foreach (string l in log)
                {
                    file.WriteLine(l);
                }
                file.Close();

                if (refazer)
                {
                    DAL.FB.Empresa dalEmpresa = DAL.FB.Empresa.GetInstancia;
                    List<Modelo.Empresa> empresas = dalEmpresa.GetAllList();

                    foreach (Modelo.Empresa emp in empresas)
                    {
                        emp.BDAlterado = false;
                        emp.Chave = MD5HashGenerator.GenerateKey(emp.ToMD5());

                        str.Remove(0, str.Length);
                        str.Append("UPDATE \"empresa\" SET \"chave\" = '");
                        str.Append(emp.Chave);
                        str.Append("', \"bdalterado\" = 0");
                        str.Append(" WHERE \"empresa\".\"id\" = ");
                        str.Append(emp.Id);
                        bath.SqlStatements.Add(str.ToString());
                    }

                    Console.WriteLine("Salvando Marcações...");
                    if (bath.SqlStatements.Count > 0)
                    {
                        bath.Execute();
                    }
                }
                Console.WriteLine();
            }
        }

        private static void corrigeBilhetes(List<string> log, bool refazer)
        {
            List<string> comandos = new List<string>();
            StringBuilder str = new StringBuilder();
            DAL.FB.Funcionario dalFuncionario = DAL.FB.Funcionario.GetInstancia;
            DAL.FB.Marcacao dalMarcacao = DAL.FB.Marcacao.GetInstancia;
            DAL.FB.BilhetesImp dalBilhetes = DAL.FB.BilhetesImp.GetInstancia;
            DAL.FB.Tratamentomarcacao dalTratamento = DAL.FB.Tratamentomarcacao.GetInstancia;
            List<Modelo.Funcionario> funcionarios = dalFuncionario.GetAllList(true);
            List<Modelo.BilhetesImp> bilhetes = null;
            string chave = String.Empty;
            Console.WriteLine("BilhetesImp");
            Console.WriteLine("Aguarde, caso existam muitos bilhetes este processo pode demorar...");
            Console.WriteLine();
            foreach (Modelo.Funcionario func in funcionarios)
            {
                bilhetes = dalBilhetes.LoadPorFuncionario(func.Dscodigo);
                foreach (Modelo.BilhetesImp bil in bilhetes)
                {
                    chave = bil.ToMD5();
                    if (bil.Chave != chave)
                    {
                        log.Add("DsCódigo Funcionario: " + func.Dscodigo + " - Data Bilhete: " + bil.Data.ToShortDateString() + " - Id Bilhete: " + bil.Id);
                        str.Remove(0, str.Length);
                        str.AppendLine("UPDATE \"bilhetesimp\" SET");
                        str.AppendLine("\"chave\" = '" + chave + "'");
                        str.AppendLine("WHERE \"bilhetesimp\".\"id\" = " + bil.Id);
                        comandos.Add(str.ToString());
                    }
                }
            }

            if (comandos.Count > 0 && refazer)
            {
                Console.WriteLine("Aguarde...");
                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    FbBatchExecution batch = new FbBatchExecution(conn);
                    int metade = (int)Math.Floor((decimal)comandos.Count / 2);
                    if (metade > 0)
                    {
                        batch.SqlStatements.AddRange(comandos.GetRange(0, metade));
                        batch.Execute();
                        batch.SqlStatements.Clear();
                    }
                    List<string> l = comandos.GetRange(metade, comandos.Count - metade);
                    if (l.Count > 0)
                    {
                        batch.SqlStatements.AddRange(l);
                        batch.Execute();
                    }
                }
            }

        }
    }
}
