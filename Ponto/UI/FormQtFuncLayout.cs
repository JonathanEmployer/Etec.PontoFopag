using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormQtFuncLayout : Form
    {
        public int cwQuantidade { get; private set; }
        public bool cwOk { get; private set; }

        public FormQtFuncLayout()
        {
            InitializeComponent();
            this.Name = "FormQtFuncLayout";
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            cwQuantidade = 0;
            cwOk = false;
            this.Close();
        }

        private void sbOk_Click(object sender, EventArgs e)
        {
            if ((int)txtQtDigitos.Value > 0 && (int)txtQtDigitos.Value < 17)
            {
                cwQuantidade = (int)txtQtDigitos.Value;
                cwOk = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Quantidade de dígitos incorreta.");
            }
        }
    }
}
