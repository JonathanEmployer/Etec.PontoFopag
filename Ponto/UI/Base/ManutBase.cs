using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI.Base
{
    public partial class ManutBase : Form
    {
        #region Atributos
        private Modelo.Acao _acao;
        private int _id;
        private string _tabela;
        private Dictionary<string, string> _erro = new Dictionary<string, string>();
        private List<string> _telasAbertas = new List<string>();
        private StringBuilder message = new StringBuilder();
        #endregion

        #region Propriedades
        public Modelo.Acao cwAcao
        {
            get { return _acao; }
            set { _acao = value; }
        }
        public int cwID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string cwTabela
        {
            get { return _tabela; }
            set { _tabela = value; }
        }
        public Dictionary<string, string> cwErro
        {
            get { return _erro; }
            set { _erro = value; }
        }
        public List<string> TelasAbertas
        {
            get { return _telasAbertas; }
            set { _telasAbertas = value; }
        }
        #endregion

        public ManutBase()
        {
            InitializeComponent();
        }

        #region Métodos Privados
        private void ManutBase_Load(object sender, EventArgs e)
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    this.Text = "Incluindo registro de " + cwTabela;
                    break;
                case Modelo.Acao.Alterar:
                    this.Text = "Alterando registro de " + cwTabela;
                    break;
                case Modelo.Acao.Excluir:
                    this.Text = "Excluindo registro de " + cwTabela;
                    xtraTabControl1.Enabled = false;
                    sbGravar.Text = "&Ok";
                    break;
                case Modelo.Acao.Consultar:
                    this.Text = "Consultando registro de " + cwTabela;
                    sbGravar.Visible = false;
                    sbGravar.Enabled = false;
                    break;
                default:
                    break;
            }

            CarregaObjeto();
        }
        protected void setErro(Control.ControlCollection pControles)
        {
            string label = "";

            //}
            //foreach (Control ctr in pControles)
            //{
            Control ctr;
            for (int i = 0; i < pControles.Count; i++)
            {
                ctr = pControles[i];

                if ((ctr is DevExpress.XtraTab.XtraTabControl) || (ctr is DevExpress.XtraTab.XtraTabPage) || (ctr is DevExpress.XtraEditors.GroupControl))
                {
                    setErro(ctr.Controls);
                }
                else
                {
                    if ((ctr is DevExpress.XtraEditors.LabelControl) || (ctr is DevExpress.XtraEditors.SimpleButton))
                    {
                        continue;
                    }

                    //errorProvider1.SetError(ctr, "");                    
                    dxErroProvider.SetError(ctr, "");
                    label = "";
                    foreach (KeyValuePair<string, string> erro in cwErro)
                    {
                        if (ctr.Name.ToLower() == erro.Key.ToLower())
                        {
                            dxErroProvider.SetError(ctr, erro.Value);

                            label = EncontraLabel(pControles, ctr);
                            if (label.Length != 0)
                                message.AppendLine(label + " " + erro.Value);
                            break;
                        }
                    }
                }
            }
        }
        private string EncontraLabel(Control.ControlCollection pControles, Control pControle)
        {
            string label = "";

            //foreach (Control ctr in pControles)
            //{
            Control ctr;
            for (int i = 0; i < pControles.Count; i++)
            {
                ctr = pControles[i];
                if ((ctr is DevExpress.XtraTab.XtraTabControl) || (ctr is DevExpress.XtraTab.XtraTabPage) || (ctr is DevExpress.XtraEditors.GroupControl))
                {
                    EncontraLabel(ctr.Controls, pControle);
                }
                else
                {
                    if (ctr.TabIndex == pControle.TabIndex - 1)
                    {
                        label = ctr.Text;
                        break;
                    }
                }
            }

            return label;
        }
        protected virtual void sbGravar_Click(object sender, EventArgs e)
        {
            try
            {
                sbGravar.Enabled = false;
                sbCancelar.Enabled = false;
                cwErro = Salvar();
                if (cwErro == null || cwErro.Count == 0)
                {
                    this.Close();
                }
                else
                {
                    message.Remove(0, message.Length);
                    setErro(this.Controls);
                    if (!String.IsNullOrEmpty(message.ToString()))
                        MessageBox.Show("Verifique Anomalias.\n\n" + message.ToString(), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Violation of UNIQUE KEY constraint"))
                {
                    string[] aux = ex.Message.Split('.');
                    string mensagem = "Violação de chave única da tabela ";
                    mensagem += aux[2].Replace("'","");
                    MessageBox.Show(mensagem, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    MessageBox.Show("Problema com o banco de dados: \n" + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                sbGravar.Enabled = true; 
                sbCancelar.Enabled = true;
            }
        }
        protected virtual void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Métodos Virtuais
        public virtual void CarregaObjeto()
        {
        }
        public virtual Dictionary<string, string> Salvar()
        {
            return cwErro;
        }
        protected virtual void GridSelecao<T>(GridBase pGrid, Componentes.devexpress.cwk_DevLookup pCb, BLL.IBLL<T> bll)
        {
            pGrid.TelasAbertas = TelasAbertas;
            UI.Util.Funcoes.ChamaGridSelecao(pGrid);
            if (pGrid.cwAtualiza == true)
            {
                pCb.Properties.DataSource = bll.GetAll();
            }
            if (pGrid.cwRetorno != 0)
            {
                pCb.EditValue = pGrid.cwRetorno;
            }
            pCb.Focus();
        }
        #endregion

        private void ManutBase_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F9:
                    if (cwAcao != Modelo.Acao.Consultar)
                    {
                        sbGravar.Focus();
                        sbGravar_Click(sender, e);
                    }
                    break;
                case Keys.Enter:
                    if (cwAcao != Modelo.Acao.Consultar)
                    {
                        sbGravar.Focus();
                        sbGravar_Click(sender, e);
                    }
                    break;
                case Keys.Escape:
                    if (MessageBox.Show("Tem certeza de que deseja fechar esta janela sem salvar as alterações?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        sbCancelar.Focus();
                        sbCancelar_Click(sender, e);
                    }
                    break;
                case Keys.F1:
                    sbAjuda_Click(sender, e);
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, cwTabela);
                    break;
            }
        }

        private void ManutBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }

        private void sbAjuda_Click(object sender, EventArgs e)
        {
            ChamaHelp();
        }

        protected virtual void ChamaHelp()
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }
    }
}
