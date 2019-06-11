using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using Modelo;
using System.Xml;
using System.IO;
using System.Data.SqlClient;


namespace REL
{
    public partial class FormBaseFiltro1 : REL.FormBaseSemEmpresa
    {

        protected BLL.Empresa bllEmpresa = new BLL.Empresa();
        protected Modelo.Empresa objEmpresa = new Modelo.Empresa();

        protected BLL.Departamento bllDepartamento = new BLL.Departamento();
        protected Modelo.Departamento objDepartamento = new Modelo.Departamento();

        protected BLL.Funcionario bllFuncionario = new BLL.Funcionario();
        protected Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
        DataTable Funcionarios;

        public FormBaseFiltro1()
        {
            InitializeComponent();
            Carrega();
            radioButton1.Checked = true;

        }

        protected override void Carrega()
        {
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAllComOpcaoTodos();
            setaNomeArquivo(this.Name);

            cbIdDepartamento.Properties.DataSource = bllDepartamento.GetAll();
            setaNomeArquivo(this.Name);

            cbIdFuncionario.Properties.DataSource = bllFuncionario.GetAll();
            setaNomeArquivo(this.Name);
            LeXML();
        }

        private void cbIdEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)cbIdEmpresa.EditValue > 0)
            {
                cbIdDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresaComOpcaoTodos((int)cbIdEmpresa.EditValue);
                cbIdDepartamento.EditValue = 0;
                cbIdDepartamento.Enabled = true;
            }
            else if ((int)cbIdEmpresa.EditValue == 0)
            {
                cbIdDepartamento.Properties.DataSource = bllDepartamento.GetAllComOpcaoTodos();
                cbIdDepartamento.EditValue = 0;
                cbIdDepartamento.Enabled = false;
            }
            LoadFuncionarios();
        }

        private void cbIdDepartamento_EditValueChanged(object sender, EventArgs e)
        {
            LoadFuncionarios();
        }

        private void LoadFuncionarios()
        {
            if ((int)cbIdDepartamento.EditValue > 0 && (int)cbIdEmpresa.EditValue > 0)
            {
                Funcionarios = bllFuncionario.GetPorDepartamento((int)cbIdEmpresa.EditValue, (int)cbIdDepartamento.EditValue, false);
            }
            else if ((int)cbIdEmpresa.EditValue == 0)
            {
                cbIdDepartamento.Enabled = false;
                Funcionarios = bllFuncionario.GetAll();
            }
            else if ((int)cbIdDepartamento.EditValue == 0)
            {
                Funcionarios = bllFuncionario.GetPorEmpresa((int)cbIdEmpresa.EditValue, false);
            }

            cbIdFuncionario.Properties.DataSource = Funcionarios;
            cbIdFuncionario.DataBindings.Clear();
            cbIdFuncionario.DataBindings.Add("EditValue", Funcionarios, "id");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                cbIdDepartamento.Enabled = false;
                cbIdFuncionario.Enabled = false;
            }


        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                cbIdDepartamento.Enabled = true;
                cbIdFuncionario.Enabled = false;
            }

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                cbIdDepartamento.Enabled = true;
                cbIdFuncionario.Enabled = true;
            }

        }






    }
}
