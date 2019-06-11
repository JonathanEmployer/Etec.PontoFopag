using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;

namespace ParametrizarComunicadorServico
{
    public partial class FormConfiguracoes : Form
    {
        bool tabTelematicaAtivo;
        bool tabServComAtivo;
        private Modelo.Proxy.PxyConfigComunicadorServico config = new Modelo.Proxy.PxyConfigComunicadorServico();
        public FormConfiguracoes(bool minimizar)
        {
            InitializeComponent();
            InitializeNotifyIcon();
            config = Negocio.Configuracao.GetConfiguracao();
            timer1.Start();
        }

        private void FormConfiguracoes_Resize(object sender, EventArgs e)
        {
            VerificarStatusServico();
            if (FormWindowState.Minimized != this.WindowState)
            {
                CarregarGridRep();
                txtUsuario.Text = config.Usuario;
                txtIdentificacaoServico.Text = String.IsNullOrEmpty(config.IdentificacaoDescServico) ? Environment.MachineName : config.IdentificacaoDescServico;
                txtInstanciaServCom.Text = config.InstanciaServCom;
                txtBaseServCom.Text = config.DataBaseServCom;
                txtUsuarioServCom.Text = config.UsuarioServCom;
                txtSenhaServCom.Text = config.SenhaServCom;

                txtInstanciaTelematica.Text = config.InstanciaTelematica;
                txtBaseTelematica.Text = config.DataBaseTelematica;
                txtUsuarioTelematica.Text = config.UsuarioTelematica;
                txtSenhaTelematica.Text = config.SenhaTelematica;
                txtCaminhoPasta.Text = config.LocalArquivoTelematica;
            }
            Notificacao.BalloonTipTitle = "Serviço Comunicador Pontofopag";
            Notificacao.BalloonTipText = "Serviço em execução.";

            if (FormWindowState.Minimized == this.WindowState)
            {
                Notificacao.Visible = true;
                Notificacao.ShowBalloonTip(10000);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                Notificacao.Visible = false;
            }


        }

        private void VerificarStatusServico()
        {
            try
            {
                ServiceController sc = new ServiceController("Pontofopag.ServRepPFP");
                switch (sc.Status)
                {
                    case ServiceControllerStatus.Running:
                        lbStatusServico.Text = "Em execução";
                        lbStatusServico.ForeColor = Color.ForestGreen;
                        btnControleServico.Text = "Parar";
                        btnControleServico.Enabled = true;
                        break;
                    case ServiceControllerStatus.Stopped:
                        lbStatusServico.Text = "Parado";
                        lbStatusServico.ForeColor = Color.Crimson;
                        btnControleServico.Text = "Iniciar";
                        btnControleServico.Enabled = true;
                        break;
                    case ServiceControllerStatus.Paused:
                        lbStatusServico.Text = "Pausado";
                        lbStatusServico.ForeColor = Color.DarkCyan;
                        btnControleServico.Text = "Iniciar";
                        btnControleServico.Enabled = true;
                        break;
                    case ServiceControllerStatus.StopPending:
                        lbStatusServico.Text = "Parando";
                        lbStatusServico.ForeColor = Color.Crimson;
                        btnControleServico.Text = "Aguarde";
                        btnControleServico.Enabled = false;
                        break;
                    case ServiceControllerStatus.StartPending:
                        lbStatusServico.Text = "Iniciando";
                        lbStatusServico.ForeColor = Color.ForestGreen;
                        btnControleServico.Text = "Aguarde";
                        btnControleServico.Enabled = false;
                        break;
                    default:
                        lbStatusServico.Text = "Desconhecido";
                        btnControleServico.Text = "Aguarde";
                        btnControleServico.Enabled = false;
                        break;
                }
            }
            catch (Exception)
            {
                lbStatusServico.Text = "Serviço não encontrado, verifique se o mesmo foi instalado corretamente.";
                lbStatusServico.ForeColor = Color.Crimson;
                btnControleServico.Text = "Erro";
                btnControleServico.Enabled = false;
            }
        }

        private void Notificacao_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
        }

        private void InitializeNotifyIcon()
        {
            MenuItem[] mnuItems = new MenuItem[3];

            //create the menu items array
            mnuItems[0] = new MenuItem("Configurar Serviço", new EventHandler(this.ShowConfiguracoes));
            mnuItems[0].DefaultItem = true;
            mnuItems[2] = new MenuItem("Alterar Status Serviço", new EventHandler(this.ShowConfiguracoes));
            mnuItems[1] = new MenuItem("-");
            mnuItems[2] = new MenuItem("Exit", new EventHandler(this.ExitControlForm));

            //add the menu items to the context menu of the NotifyIcon
            ContextMenu notifyIconMenu = new ContextMenu(mnuItems);
            Notificacao.ContextMenu = notifyIconMenu;
        }

        public void ShowConfiguracoes(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
        }

        public void ExitControlForm(object sender, EventArgs e)
        {
            ////Hide the NotifyIcon.
            //Notificacao.Visible = false;

            //this.Close();
            ServiceController controller = new ServiceController("Pontofopag.ServRepPFP");
            if (controller.Status == ServiceControllerStatus.Running)
                controller.Stop();
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnAtualizarReps_Click(object sender, EventArgs e)
        {
            CarregarGridRep();
        }

        private void CarregarGridRep()
        {
            IList<ModeloAux.RepViewModel> reps = Negocio.Rep.GetRepConfig(config);

            dataGridReps.AutoGenerateColumns = false;
            dataGridReps.DataSource = reps;
            dataGridReps.RowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#f9f9f9");
            dataGridReps.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

            ValidaModeloRep();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                config.IdentificacaoDescServico = txtIdentificacaoServico.Text;
                config.InstanciaServCom = txtInstanciaServCom.Text;
                config.DataBaseServCom = txtBaseServCom.Text;
                config.SenhaServCom = txtSenhaServCom.Text;
                config.UsuarioServCom = txtUsuarioServCom.Text;
                config.InstanciaTelematica = txtInstanciaTelematica.Text;
                config.DataBaseTelematica = txtBaseTelematica.Text;
                config.UsuarioTelematica = txtUsuarioTelematica.Text;
                config.SenhaTelematica = txtSenhaTelematica.Text;
                config.LocalArquivoTelematica = txtCaminhoPasta.Text;
                Negocio.Configuracao.SaveConfiguracao(config);

                List<ModeloAux.RepViewModel> repsSalvar = (List<ModeloAux.RepViewModel>)dataGridReps.DataSource;
                Negocio.Rep.SaveConfiguracao(repsSalvar);

                if (MessageBox.Show("Deseja encerrar a aplicação?", "Configuração salva com sucesso.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar configurações, erro: "+ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnControleServico_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceController sc = new ServiceController("Pontofopag.ServRepPFP");
                switch (sc.Status)
                {
                    case ServiceControllerStatus.Running:
                        sc.Stop();
                        break;
                    case ServiceControllerStatus.Stopped:
                        sc.Start();
                        break;
                    case ServiceControllerStatus.Paused:
                        sc.Start();
                        break;
                    default:
                        lbStatusServico.Text = "Desconhecido";
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao manipular o serviço, verifique se a aplicação foi executada em modo de administrador!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            VerificarStatusServico();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            VerificarStatusServico();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowser.Description = "Selecione o diretório";
            folderBrowser.ShowNewFolderButton = false;

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtCaminhoPasta.Text = folderBrowser.SelectedPath;
            }
        }

        private void dataGridReps_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridReps.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        public void ValidaModeloRep()
        {
            bool tabTelematicaVisivel = false;
            bool tabServComVisivel = false;
            tabControlParamConex.TabPages.Remove(tabServCom);
            tabControlParamConex.TabPages.Remove(tabTelematica);
            btnConexao.Visible = false;
            //int indexFabricante = dataGridReps.Columns["nomeFabricante"].Index;
            foreach (DataGridViewRow row in dataGridReps.Rows)
            {
                if (row.Cells["nomeFabricante"].Value.ToString().Contains("Telemática"))
                {
                    if ((bool)row.Cells["AtivoServico"].Value == true)
                    {
                        tabTelematicaVisivel = true;
                    }
                }
                if (row.Cells["nomeFabricante"].Value.ToString().Contains("Dimas de Melo"))
                {
                    if ((bool)row.Cells["AtivoServico"].Value == true)
                    {
                        tabServComVisivel = true;   
                    }
                }
            }

            if (tabServComVisivel)
            {
                tabControlParamConex.TabPages.Add(tabServCom);
                btnConexao.Visible = true;
                tabServComAtivo = true;
            }
            else
            {
                tabServComAtivo = false;
            }

            if (tabTelematicaVisivel)
            {
                tabControlParamConex.TabPages.Add(tabTelematica);
                btnConexao.Visible = true;
                tabTelematicaAtivo = true;
            }
            else
            {
                tabTelematicaAtivo = false;
            }

            if (tabControlParamConex.TabPages.Count == 0)
            {
                tabControlParamConex.Visible = false;
                btnConexao.Visible = false;
            }
            else
            { 
                tabControlParamConex.Visible = true;
            }
            
        }

        private void dataGridReps_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridReps_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            ValidaModeloRep();
        }

        private void btnConexao_Click(object sender, EventArgs e)
        {
            if (tabControlParamConex.SelectedTab.Text == "Telematica")
            {
                if (tabTelematicaAtivo)
                {
                    if (String.IsNullOrWhiteSpace(txtInstanciaTelematica.Text) && String.IsNullOrWhiteSpace(txtBaseTelematica.Text) && String.IsNullOrWhiteSpace(txtUsuarioTelematica.Text) && String.IsNullOrWhiteSpace(txtSenhaTelematica.Text))
                    {
                        MessageBox.Show("Preencha os campos antes de testar a conexão!",
                            "Status Conexão",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button3);
                    }
                    else
                    {
                        SqlConnection conn = new SqlConnection(@"Data Source=" + txtInstanciaTelematica.Text + ";Initial Catalog=" + txtBaseTelematica.Text + "; User ID = " + txtUsuarioTelematica.Text + "; Password=" + txtSenhaTelematica.Text);

                        try
                        {
                            conn.Open();
                            MessageBox.Show("Conexão realizada com sucesso!",
                                "Status Conexão",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button3);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Falha ao realizar a conexão",
                                "Status Conexão",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button3);
                            conn.Close();
                        }
                        conn.Close();
                    }
                }
            }

            if (tabControlParamConex.SelectedTab.Text == "ServCom")
            {
                if (tabServComAtivo)
                {
                    if (String.IsNullOrWhiteSpace(txtInstanciaServCom.Text) && String.IsNullOrWhiteSpace(txtBaseServCom.Text) && String.IsNullOrWhiteSpace(txtUsuarioServCom.Text) && String.IsNullOrWhiteSpace(txtSenhaServCom.Text))
                    {
                        MessageBox.Show("Preencha os campos antes de testar a conexão!",
                            "Status Conexão",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation,
                            MessageBoxDefaultButton.Button3);
                    }
                    else
                    {
                        SqlConnection conn = new SqlConnection(@"Data Source=" + txtInstanciaServCom.Text + ";Initial Catalog=" + txtBaseServCom.Text + "; User ID = " + txtUsuarioServCom.Text + "; Password=" + txtSenhaServCom.Text);

                        try
                        {
                            conn.Open();
                            MessageBox.Show("Conexão realizada com sucesso!",
                                "Status Conexão",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information,
                                MessageBoxDefaultButton.Button3);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Falha ao realizar a conexão",
                                "Status Conexão",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error,
                                MessageBoxDefaultButton.Button3);
                            conn.Close();
                        }
                        conn.Close();
                    }
                }
            }

            
            
        }

        private void btnTestarPermissaoEscritaTxt_Click(object sender, EventArgs e)
        {
            try
            {

                Util.EscreveArquivo(txtCaminhoPasta.Text, "TestePermissao", new List<string> { "Teste de permissão de escrita"});
                File.Delete(txtCaminhoPasta.Text+@"\TestePermissao.txt");
                MessageBox.Show("Teste de criação de arquivo executada com sucesso");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao criar o arquivo, verifique as permissões de acesso ao destino, detalhes: "+ex.Message);
            }
        }
    }
}
