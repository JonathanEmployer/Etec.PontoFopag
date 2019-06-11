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
    public partial class FormVisualizarLayout : Form
    {
        public FormVisualizarLayout(List<Modelo.ExportacaoCampos> campos)
        {
            InitializeComponent();
            lblCampos.Text = BLL.ExportacaoCampos.MontaStringExportacao(campos);
            lblQtdCaracteres.Text = lblCampos.Text.Length.ToString();
        }

        private void FormVisualizarLayout_KeyDown(object sender, KeyEventArgs e)
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
