using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace UI
{
    public partial class FormManutExportacaoCampos : UI.Base.ManutBase
    {
        private BLL.ExportacaoCampos bllExportacaoCampos;
        private Modelo.ExportacaoCampos objExportacaoCampos;
        private Modelo.LayoutExportacao objLayoutExportacao;

        public FormManutExportacaoCampos(Modelo.LayoutExportacao pLayoutExportacao)
        {
            InitializeComponent();
            bllExportacaoCampos = new BLL.ExportacaoCampos();
            this.Name = "FormManutExportacaoCampos";
            objLayoutExportacao = pLayoutExportacao;
            lblPosicaoDispo.Text = Convert.ToString(BLL.ExportacaoCampos.MontaStringExportacao(pLayoutExportacao.ExportacaoCampos).Length + 1);
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objExportacaoCampos = new Modelo.ExportacaoCampos();
                    objExportacaoCampos.Codigo = bllExportacaoCampos.CodigoMaximo(objLayoutExportacao.ExportacaoCampos);                   
                    objExportacaoCampos.Delimitador = "[nenhum]";
                    objExportacaoCampos.Qualificador = "[nenhum]";
                    break;
                default:
                    objExportacaoCampos = objLayoutExportacao.ExportacaoCampos.Where(e => e.Codigo == cwID).First();                    
                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objExportacaoCampos, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTamanho.DataBindings.Add("EditValue", objExportacaoCampos, "Tamanho", true, DataSourceUpdateMode.OnPropertyChanged);
            cbDelimitador.DataBindings.Add("EditValue", objExportacaoCampos, "Delimitador", true, DataSourceUpdateMode.OnPropertyChanged);            
            txtCabecalho.DataBindings.Add("EditValue", objExportacaoCampos, "Cabecalho", true, DataSourceUpdateMode.OnPropertyChanged);
            txtFormatoevento.DataBindings.Add("EditValue", objExportacaoCampos, "Formatoevento", true, DataSourceUpdateMode.OnPropertyChanged);
            cbTipo.DataBindings.Add("EditValue", objExportacaoCampos, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPosicao.DataBindings.Add("EditValue", objExportacaoCampos, "Posicao", true, DataSourceUpdateMode.OnPropertyChanged);
            cbQualificador.DataBindings.Add("EditValue", objExportacaoCampos, "Qualificador", true, DataSourceUpdateMode.OnPropertyChanged);
            chbZeroesquerda.DataBindings.Add("Checked", objExportacaoCampos, "Zeroesquerda", true, DataSourceUpdateMode.OnPropertyChanged);

            txtTexto.EditValue = objExportacaoCampos.Texto;
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            objExportacaoCampos.Texto = txtTexto.Text;
            base.Salvar();
            return bllExportacaoCampos.Salvar(objExportacaoCampos, objLayoutExportacao.ExportacaoCampos, cwAcao);
        }

        private void cbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCabecalho.Enabled = false;
            txtFormatoevento.Enabled = false;
            txtTexto.Enabled = false;
            txtTexto.EditValue = "";
            txtPosicao.Properties.ReadOnly = false;
            txtTamanho.Properties.ReadOnly = false;
            switch (cbTipo.SelectedIndex)
            {
                case 0:
                    txtCabecalho.Enabled = true;
                    txtPosicao.EditValue = 1;
                    txtPosicao.Properties.ReadOnly = true;
                    break;
                case 5:
                    txtTamanho.EditValue = txtTexto.Text.Length;
                    txtTexto.Enabled = true;
                    txtTamanho.Properties.ReadOnly = true;
                    break;
                case 8:
                    txtFormatoevento.Enabled = true;
                    break;
                default:
                    txtTamanho.Properties.ReadOnly = false;
                    txtPosicao.Properties.ReadOnly = false;
                    break;
            }
        }

        private void txtTexto_EditValueChanged(object sender, EventArgs e)
        {
            if (cbTipo.SelectedIndex == 5)
            {
                txtTamanho.EditValue = txtTexto.Text.Length;
            }
        }
    }
}