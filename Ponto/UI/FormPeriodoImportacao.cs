using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormPeriodoImportacao : Form
    {
        public bool Importar { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        private BLL.ConfiguracoesGerais bllConfiguracoesGerais;

        public FormPeriodoImportacao()
        {
            InitializeComponent();
            bllConfiguracoesGerais = new BLL.ConfiguracoesGerais();
            Importar = false;

            DateTime dataInicial;
            DateTime dataFinal;
            bool mudadataautomaticamente;
            bllConfiguracoesGerais.AtribuiDatas(Application.StartupPath, out dataInicial, out dataFinal, out mudadataautomaticamente);

            txtDataInicial.DateTime = dataInicial;
            txtDataFinal.DateTime = dataFinal;

            if (txtDataInicial.DateTime == new DateTime())
            {
                txtDataInicial.EditValue = null;
            }

            if (txtDataFinal.DateTime == new DateTime())
            {
                txtDataFinal.EditValue = null;
            }
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            Importar = false;
            this.Close();
        }

        private void sbImportar_Click(object sender, EventArgs e)
        {
            if (txtDataInicial.EditValue == null || txtDataFinal.EditValue == null)
            {
                if (txtDataInicial.EditValue == null)
                    dxErrorProvider1.SetError(txtDataInicial, "Preencha a data inicial do período de importação.");
                else
                    dxErrorProvider1.SetError(txtDataInicial, String.Empty);
                if (txtDataFinal.EditValue == null)
                    dxErrorProvider1.SetError(txtDataFinal, "Preencha a data final do período de importação.");
                else
                    dxErrorProvider1.SetError(txtDataFinal, String.Empty);
            }
            else
            {
                Importar = true;
                DataInicial = txtDataInicial.DateTime;
                DataFinal = txtDataFinal.DateTime;
                this.Close();
            }
        }

        private void FormPeriodoImportacao_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }
    }
}

