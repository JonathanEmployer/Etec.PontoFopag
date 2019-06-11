using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Modelo;

namespace UI
{
    public partial class FormManutLayoutImportacaoTxt : UI.Base.ManutBase
    {
        private List<Modelo.LayoutImportacao> lista;
        private Modelo.LayoutImportacao ObjLayoutImportacao = new Modelo.LayoutImportacao();
        private BLL.LayoutImportacao BLLImportacao;
        private int Codigo;

        public FormManutLayoutImportacaoTxt(List<Modelo.LayoutImportacao> pLista, int pCodigo, int pMaxCodigo)
        {
            InitializeComponent();
            BLLImportacao = new BLL.LayoutImportacao();
            this.Text = "Importação de Funcionários";
            CarregaObjeto();
            lista = pLista;
            sbGravar.Text = "OK";
            Codigo = pCodigo;
            rgTipoCampo.SelectedIndex = 0;
            txtCodigo.EditValue = pMaxCodigo;
            txtCodigo.Properties.ReadOnly = true;
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Alterar:
                    ObjLayoutImportacao = lista.Find(delegate(Modelo.LayoutImportacao Li) { return Li.Codigo == Codigo; });
                    AtribuiValores();
                    break;
                case Modelo.Acao.Excluir:
                    ObjLayoutImportacao = lista.Find(delegate(Modelo.LayoutImportacao Li) { return Li.Codigo == Codigo; });
                    AtribuiValores();
                    break;
                case Modelo.Acao.Consultar:
                    AtribuiValores();
                    txtCodigo.Enabled = false;
                    rgTipoCampo.Enabled = false;
                    cbDescricaoCampo.Enabled = false;
                    cbDelimitador.Enabled = false;
                    txtPosicao.Enabled = false;
                    txtTamanho.Enabled = false;
                    break;
            }
        }

        protected override void sbGravar_Click(object sender, EventArgs e)
        {
            //if (ObjLayoutImportacao.Id > 0)
                ObjLayoutImportacao.objAcao = cwAcao;

            StringBuilder msgm = new StringBuilder();
            if (txtCodigo.Text == String.Empty)
                msgm.AppendLine("Por favor digite um codigo.");
            if (cbDescricaoCampo.EditValue == null)
                msgm.AppendLine("Por favor escolha um campo.");
            if (rgTipoCampo.SelectedIndex == 0 && txtTamanho.Text == String.Empty)
                msgm.AppendLine("Por Favor digite um tamanho.");
            if (rgTipoCampo.SelectedIndex == 0 && txtPosicao.Text == String.Empty)
                msgm.AppendLine("Por Favor digite uma Posição.");
            if (rgTipoCampo.SelectedIndex == 1 && cbDelimitador.SelectedIndex == -1)
                msgm.AppendLine("Por Favor escolhar um delimitador.");
            if (Convert.ToInt32(txtCodigo.EditValue) >= int.MaxValue)
                msgm.AppendLine("O número do campo código é muito grande, por favor digite um numero menor.");
            if (rgTipoCampo.SelectedIndex == 0 && Convert.ToInt32(txtPosicao.EditValue) >= Int16.MaxValue)
                msgm.AppendLine("O número do campo posição é muito grande, por favor digite um numero menor.");
            if (rgTipoCampo.SelectedIndex == 0 && Convert.ToInt32(txtTamanho.EditValue) >= Int16.MaxValue)
                msgm.AppendLine("O número do campo tamanho é muito grande, por favor digite um numero menor.");


            if (msgm.Length == 0)
            {
                ObjLayoutImportacao.Codigo = Convert.ToInt32(txtCodigo.EditValue);
                ObjLayoutImportacao.Tipo = (tipo)(Convert.ToBoolean(rgTipoCampo.EditValue) ? 0 : 1);
                ObjLayoutImportacao.Campo = (campo)cbDescricaoCampo.SelectedIndex;
                ObjLayoutImportacao.Delimitador = Convert.ToChar(cbDelimitador.Text);
                ObjLayoutImportacao.Posicao = txtPosicao.Text == " " ? Convert.ToInt16(0) : Convert.ToInt16(txtPosicao.EditValue);
                ObjLayoutImportacao.Tamanho = txtTamanho.Text == " " ? Convert.ToInt16(0) : Convert.ToInt16(txtTamanho.EditValue);

                if (cwAcao == Acao.Incluir)
                    lista.Add(ObjLayoutImportacao);
                else if (cwAcao == Acao.Excluir && ObjLayoutImportacao.Id <= 0)
                    lista.Remove(ObjLayoutImportacao);

                this.Close();
            }
            else
                MessageBox.Show(msgm.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void AtribuiValores()
        {
            
            txtCodigo.EditValue = ObjLayoutImportacao.Codigo;
            rgTipoCampo.EditValue = ObjLayoutImportacao.Tipo == tipo.Fixo ? true : false;
            cbDescricaoCampo.SelectedIndex = Convert.ToInt32(ObjLayoutImportacao.Campo);
            cbDelimitador.Text = ObjLayoutImportacao.Delimitador.ToString();
            txtPosicao.EditValue = ObjLayoutImportacao.Posicao;
            txtTamanho.EditValue = ObjLayoutImportacao.Tamanho;
        }

        private void rgTipoCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rgTipoCampo.SelectedIndex == 0)
            {
                cbDelimitador.Enabled = false;
                cbDelimitador.Text = " ";
                txtPosicao.Enabled = true;
                txtTamanho.Enabled = true;
            }

            if (rgTipoCampo.SelectedIndex == 1)
            {
                txtPosicao.Enabled = false;
                txtPosicao.Text = " ";
                txtTamanho.Enabled = false;
                txtTamanho.Text = " ";
                cbDelimitador.Enabled = true;
            }
        }
    }
}
