using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutBackup : UI.Base.ManutBase
    {
        private BLL.Backup bllBackup;
        private Modelo.Backup objBackup;

        public FormManutBackup()
        {
            InitializeComponent();
            bllBackup = new BLL.Backup();
            this.Name = "FormManutBackup";
        }
        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objBackup = new Modelo.Backup();
                    objBackup.Codigo = bllBackup.MaxCodigo();
                    break;
                case Modelo.Acao.Consultar:
                    sbGravar.Enabled = false;
                    objBackup = bllBackup.LoadObject(cwID);
                    break;
                default:
                    objBackup = bllBackup.LoadObject(cwID);
                    break;
            }
            txtCodigo.DataBindings.Add("EditValue", objBackup, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objBackup, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDiretorio.DataBindings.Add("EditValue", objBackup, "Diretorio", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            objBackup.Descricao = txtDescricao.Text;
            objBackup.Diretorio = txtDiretorio.Text;

            objBackup.Descricao = objBackup.Descricao.TrimEnd();
            base.Salvar();
            ret = bllBackup.Salvar(cwAcao, objBackup);
            return ret;
        }

        private void BotaoDiretorio_Click(object sender, EventArgs e)
        {
            try
            {
                folderBrowserDialog1.Description = "Selecione uma pasta para realizar o Backup";
                folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialog1.ShowNewFolderButton = true;

                folderBrowserDialog1.ShowDialog();

                txtDiretorio.Text = folderBrowserDialog1.SelectedPath;

                objBackup.Diretorio = folderBrowserDialog1.SelectedPath;
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao abrir diretorio!");
            }
        }

        private void txtDiretorio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                BotaoDiretorio.PerformClick();
            }
        }
    }
}
