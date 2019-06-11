using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutLayoutExportacao : UI.Base.ManutBase
    {
        private BLL.LayoutExportacao bllLayoutExportacao;
        private BLL.ExportacaoCampos bllExportacaoCampos;
        private Modelo.LayoutExportacao objLayoutExportacao;

        public FormManutLayoutExportacao()
        {
            InitializeComponent();
            bllExportacaoCampos = new BLL.ExportacaoCampos();
            bllLayoutExportacao = new BLL.LayoutExportacao();
            this.Name = "FormManutLayoutExportacao";
            lblCampos.Text = "";
        }
        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objLayoutExportacao = new Modelo.LayoutExportacao();
                    objLayoutExportacao.Codigo = bllLayoutExportacao.MaxCodigo();
                    objLayoutExportacao.ExportacaoCampos = new List<Modelo.ExportacaoCampos>();
                    break;
                case Modelo.Acao.Consultar:
                    sbGravar.Enabled = false;
                    objLayoutExportacao = bllLayoutExportacao.LoadObject(cwID);
                    break;
                default:
                    objLayoutExportacao = bllLayoutExportacao.LoadObject(cwID);
                    break;
            }
            txtCodigo.DataBindings.Add("EditValue", objLayoutExportacao, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDescricao.DataBindings.Add("EditValue", objLayoutExportacao, "Descricao", true, DataSourceUpdateMode.OnPropertyChanged);
            LoadCampos();
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ret = bllLayoutExportacao.Salvar(cwAcao, objLayoutExportacao);
            return ret;
        }

        #region Exportação Campos

        private void LoadCampos()
        {
            List<Modelo.ExportacaoCampos> lista = new List<Modelo.ExportacaoCampos>();
            foreach (Modelo.ExportacaoCampos exp in objLayoutExportacao.ExportacaoCampos)
            {
                if (exp.Acao != Modelo.Acao.Excluir)
                {
                    lista.Add(exp);
                }
            }
            gcExportacaoCampos.DataSource = lista;
        }

        private void CarregaFormCampos(Modelo.Acao pAcao, int pCodigo)
        {
            if (pAcao != Modelo.Acao.Incluir && pCodigo == 0)
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
            else
            {
                UI.FormManutExportacaoCampos form = new UI.FormManutExportacaoCampos(objLayoutExportacao);
                form.cwAcao = pAcao;
                form.cwID = pCodigo;
                form.cwTabela = "Exportação Campos";
                form.ShowDialog();

                if (pAcao != Modelo.Acao.Consultar)
                {
                    LoadCampos();
                }

                AtualizaString();
            }
        }

        private void AtualizaString()
        {
            lblCampos.Text = BLL.ExportacaoCampos.MontaStringExportacao(objLayoutExportacao.ExportacaoCampos);
        }

        private Int32 CampoSelecionado()
        {
            Int32 seq;
            try
            {
                seq = (int)gvExportacaoCampos.GetFocusedRowCellValue("Codigo");
            }
            catch (Exception)
            {
                seq = 0;
            }
            return seq;
        }

        private void gcExportacaoCampos_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CarregaFormCampos(Modelo.Acao.Alterar, CampoSelecionado());
                    break;
            }
        }

        private void gcExportacaoCampos_DoubleClick(object sender, EventArgs e)
        {
            CarregaFormCampos(Modelo.Acao.Alterar, CampoSelecionado());
        }

        private void sbIncluirExpCampos_Click(object sender, EventArgs e)
        {
            CarregaFormCampos(Modelo.Acao.Incluir, 0);
        }

        private void sbAlterarExpCampos_Click(object sender, EventArgs e)
        {
            CarregaFormCampos(Modelo.Acao.Alterar, CampoSelecionado());
        }

        private void sbExcluirExpCampos_Click(object sender, EventArgs e)
        {
            CarregaFormCampos(Modelo.Acao.Excluir, CampoSelecionado());
        }
        #endregion

        private void sbVisualizar_Click(object sender, EventArgs e)
        {
            FormVisualizarLayout form = new FormVisualizarLayout(objLayoutExportacao.ExportacaoCampos);
            form.ShowDialog();
        }
    }
}
