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
    public partial class FormLogErroImportacao : Form
    {
        public FormLogErroImportacao(IList<Modelo.pxyLogErroImportacao> dados)
        {
            InitializeComponent();
            gcDadosImportacao.DataSource = dados;
        }

        private void FormLogErroImportacao_FormClosed(object sender, FormClosedEventArgs e)
        {
            gcDadosImportacao.DataSource = null;
        }


    }
}
