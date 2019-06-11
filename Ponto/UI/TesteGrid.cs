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
    public partial class TesteGrid : Form
    {
        BLL.Funcionario bllFuncionario;
        public TesteGrid()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            dataGridView1.DataSource = bllFuncionario.GetAll();
        }
    }
}
