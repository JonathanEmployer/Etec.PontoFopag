using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Diagnostics;

namespace InstalacaoSQL
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (MessageBox.Show("Deseja instalar o SQL Server 2008 Express?", "Cwork Sistemas", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                Application.Exit();
            }
            else
            {
                //Application.Run(new Form1());
                string diretorioSQL = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "");

                if (!instalaSetup(diretorioSQL))
                {
                    FileInfo f1 = new FileInfo(diretorioSQL + @"\SQLEXPR_x86_ENU.exe");
                    if (f1.Exists)
                    {
                        Process p1 = Process.Start(f1.FullName, "/X");
                        Application.DoEvents();
                        p1.WaitForExit();

                        if (!instalaSetup(diretorioSQL))
                        {
                            MessageBox.Show("Erro ao instalar SQL Server: Arquivo de instalação não encontrado.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erro ao instalar SQL Server: Arquivo de instalação não encontrado.");
                    }
                }
            }



            if (MessageBox.Show("Deseja restaurar um banco de dados?", "Cwork Ponto MT", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Run(new FormRestaurarBanco());
            }
        }

        private static bool instalaSetup(string diretorioSQL)
        {
            FileInfo f1 = new FileInfo(diretorioSQL + @"\setup.exe");
            if (f1.Exists)
            {
                Process p1 = Process.Start(f1.FullName, "/ACTION=install /SAPWD=\"cwork#0110\" /INSTANCENAME=\"CWORK\" /TCPENABLED=1 /SECURITYMODE=\"SQL\"");
                Application.DoEvents();
                p1.WaitForExit();
                return true;
            }
            return false;
        }
    }
}
