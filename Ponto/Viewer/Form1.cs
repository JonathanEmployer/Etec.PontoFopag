using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Viewer
{
    public partial class Form1 : Form
    {
        private DAL.SQL.BilhetesImp bllBilhes = DAL.SQL.BilhetesImp.GetInstancia;
        private DAL.SQL.Marcacao bllMarcacao = DAL.SQL.Marcacao.GetInstancia;

        public Form1()
        {
            InitializeComponent();

            Modelo.Cw_Usuario objUsuario = new Modelo.Cw_Usuario();
            objUsuario.Login = "cwork";
            Modelo.cwkGlobal.objUsuarioLogado = objUsuario;
            Modelo.cwkGlobal.BD = 1;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gridControl1.DataSource = null;
            gridControl1.RefreshDataSource();
            dataGridView1.Columns.Clear();
            switch (comboBoxEdit1.SelectedIndex)
            {
                case 0:
                    gridControl1.DataSource = bllBilhes.GetListaNaoImportados();
                    break;
                case 1:
                    gridControl1.DataSource = bllMarcacao.GetPorPeriodo(txtDataInicial.DateTime, txtDataFinal.DateTime);
                    break;
            }
        }
    }
}
