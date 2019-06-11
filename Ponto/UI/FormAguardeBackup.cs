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
    public partial class FormAguardeBackup : Form
    {
        private BLL.Parametros bllParametros;
        private int parametroBackup;
        public FormAguardeBackup(int pEntradaSaida)
        {
            InitializeComponent();
            bllParametros = new BLL.Parametros();
            parametroBackup = pEntradaSaida;
        }

        private void FormAguardeBackup_Shown(object sender, EventArgs e)
        {
            try
            {
                this.Refresh();
                bllParametros.Backup(parametroBackup);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }
    }
}
