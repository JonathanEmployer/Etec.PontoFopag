using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;

namespace InstalacaoSQL
{
    public partial class FormRestaurarBanco : Form
    {
        public FormRestaurarBanco()
        {
            InitializeComponent();
            txtDados.Text = @"C:\CworkPontoMT\Dados";
            txtServidor.Text = @".\CWORK";
            txtBanco.Text = "cwkpontomt";
            txtUsuario.Text = "sa";
            txtSenha.Text = "cwork#0110";
        }

        private bool DadosValidos()
        {
            StringBuilder str = new StringBuilder();
            if (!File.Exists(txtArquivo.Text))
            {
                str.AppendLine("Arquivo: O arquivo especificado não existe.");
            }

            if (!Directory.Exists(txtDados.Text))
            {
                str.AppendLine("Dretório Dados: O diretório especificado não existe.");
            }

            if (String.IsNullOrEmpty(txtServidor.Text))
            {
                str.AppendLine("SQL Servidor: Digite o nome do servidor.");
            }

            if (String.IsNullOrEmpty(txtBanco.Text))
            {
                str.AppendLine("SQL Banco: Digite o nome do banco de dados.");
            }

            if (String.IsNullOrEmpty(txtUsuario.Text))
            {
                str.AppendLine("SQL Usuário: Digite o nome do usuário.");
            }

            if (String.IsNullOrEmpty(txtSenha.Text))
            {
                str.AppendLine("SQL Senha: Digite a senha.");
            }

            if (str.Length > 0)
            {
                MessageBox.Show(str.ToString(), "Preencha os dados corretamente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btRestaurar_Click(object sender, EventArgs e)
        {

            if (DadosValidos())
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = txtServidor.Text;
                builder.UserID = txtUsuario.Text;
                builder.Password = txtSenha.Text;
                builder.InitialCatalog = txtBanco.Text;
                int tentativas = 0;
                FormAguarde formAguarde;
                Cursor.Current = Cursors.WaitCursor;
                while (true)
                {
                    try
                    {
                        //Process p = Process.Start("sqlcmd", "-S " + txtServidor.Text + " -U " + txtUsuario.Text + " -P " + txtSenha.Text + " -Q \"RESTORE DATABASE " + txtBanco.Text + " FROM DISK = '" + txtArquivo.Text + "' WITH MOVE 'cwkPontoMT' TO '" + txtDados.Text + "\\" + txtBanco.Text + ".mdf', MOVE 'cwkPontoMT_Log' TO '" + txtDados.Text + "\\" + txtBanco.Text + "_Log.ldf',REPLACE\"");
                        //p.WaitForExit();
                        //if (p.ExitCode == 0)
                        //    this.Close();
                        //else
                        //{
                        //    MessageBox.Show("Erro ao restaurar banco de dados. Verifique os dados de conexão.");
                        //}

                        if (tentativas == 0)
                        {
                            this.Enabled = false;
                            formAguarde = new FormAguarde();
                            formAguarde.Show();
                            formAguarde.Refresh();
                        }

                        SqlConnectionStringBuilder builderAux = new SqlConnectionStringBuilder();
                        builderAux.DataSource = txtServidor.Text;
                        builderAux.UserID = txtUsuario.Text;
                        builderAux.Password = txtSenha.Text;
                        builderAux.InitialCatalog = "master";
                        using (SqlConnection conn = new SqlConnection(builderAux.ConnectionString))
                        {
                            conn.Open();
                            string str = "RESTORE DATABASE " + txtBanco.Text + " FROM DISK = '" + txtArquivo.Text + "' WITH MOVE 'cwkPontoMT' TO '" + txtDados.Text + "\\" + txtBanco.Text + ".mdf', MOVE 'cwkPontoMT_Log' TO '" + txtDados.Text + "\\" + txtBanco.Text + "_Log.ldf',REPLACE";
                            SqlCommand cmd = new SqlCommand(str, conn);
                            cmd.CommandTimeout = 240;
                            cmd.ExecuteNonQuery();
                        }
                        this.Close();
                        break;
                    }
                    catch (Exception ex)
                    {
                        tentativas++;
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(1000);
                        if (tentativas == 5)
                        {
                            MessageBox.Show("Erro ao restaurar banco de dados:\n" + ex.Message);
                            break;
                        }
                    }
                }
            }
        }

        private void btArquivo_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.FileName = "";
                if (!String.IsNullOrEmpty(txtArquivo.Text))
                {
                    openFileDialog1.InitialDirectory = txtArquivo.Text;
                }
                openFileDialog1.Filter = "Banco SQL Server(*.bak)|*.bak";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtArquivo.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                folderBrowserDialog1.SelectedPath = "";
                if (!String.IsNullOrEmpty(txtDados.Text))
                {
                    folderBrowserDialog1.SelectedPath = txtDados.Text;
                }

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    txtDados.Text = folderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
