using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutInclusaoBanco : UI.Base.ManutBase
    {
        private BLL.InclusaoBanco bllInclusaoBanco;
        private Modelo.InclusaoBanco objInclusaoBanco;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcionario bllFuncionario;
        private BLL.Funcao bllFuncao;

        public FormManutInclusaoBanco()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncao = new BLL.Funcao();
            bllFuncionario = new BLL.Funcionario();
            bllInclusaoBanco = new BLL.InclusaoBanco();
            this.Name = "FormManutInclusaoBanco";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objInclusaoBanco = new Modelo.InclusaoBanco();
                    objInclusaoBanco.Codigo = bllInclusaoBanco.MaxCodigo();
                    objInclusaoBanco.Tipo = -1;
                    objInclusaoBanco.Tipocreditodebito = -1;
                    objInclusaoBanco.Data = null;
                    txtDebito.Enabled = false;
                    txtCredito.Enabled = false;
                    break;
                default:
                    objInclusaoBanco = bllInclusaoBanco.LoadObject(cwID);
                    objInclusaoBanco.Tipo_Ant = objInclusaoBanco.Tipo;
                    objInclusaoBanco.Data_Ant = objInclusaoBanco.Data;
                    objInclusaoBanco.Identificacao_Ant = objInclusaoBanco.Identificacao;

                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objInclusaoBanco, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipo.DataBindings.Add("EditValue", objInclusaoBanco, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdentificacao.DataBindings.Add("EditValue", objInclusaoBanco, "Identificacao", true, DataSourceUpdateMode.OnPropertyChanged);
            txtData.DataBindings.Add("DateTime", objInclusaoBanco, "Data", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipocreditodebito.DataBindings.Add("EditValue", objInclusaoBanco, "Tipocreditodebito", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCredito.DataBindings.Add("EditValue", objInclusaoBanco, "Credito", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDebito.DataBindings.Add("EditValue", objInclusaoBanco, "Debito", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            FormProgressBar2 pb = new FormProgressBar2();
            pb.Show(this);
            bllInclusaoBanco.ObjProgressBar = pb.ObjProgressBar;
            Dictionary<string, string> ret = bllInclusaoBanco.Salvar(cwAcao, objInclusaoBanco);
            pb.Close();
            return ret;
        }

        private void rgTipocreditodebito_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((int)rgTipocreditodebito.EditValue)
            {
                case 0:
                    txtCredito.Enabled = true;
                    txtDebito.Enabled = false;
                    txtDebito.EditValue = "---:--";
                    objInclusaoBanco.Debito = "---:--";
                    break;
                case 1:
                    txtDebito.Enabled = true;
                    txtCredito.Enabled = false;
                    txtCredito.EditValue = "---:--";
                    objInclusaoBanco.Credito = "---:--";
                    break;
                default:
                    txtCredito.Enabled = false;
                    txtDebito.Enabled = false;
                    break;
            }
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbIdentificacao.Properties.DataSource = null;
            switch ((int)rgTipo.EditValue)
            {
                case -1:
                    cbIdentificacao.Properties.DataSource = null;
                    break;
                case 0: // Empresa
                    cbIdentificacao.Properties.Columns["nome"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllEmpresa.GetAll();
                    cbIdentificacao.Properties.Columns["descricao"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "nome";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    break;
                case 1: // Departamento
                    cbIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllDepartamento.GetAll();
                    cbIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "descricao";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = true;
                    break;
                case 2: // Funcionário
                    cbIdentificacao.Properties.DataSource = bllFuncionario.GetFuncionariosAtivos();
                    cbIdentificacao.Properties.Columns["descricao"].Visible = false;
                    cbIdentificacao.Properties.Columns["nome"].Visible = true;
                    cbIdentificacao.Properties.DisplayMember = "nome";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    break;
                case 3: // Função
                    cbIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllFuncao.GetAll();
                    cbIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "descricao";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    break;
            }
        }

        private void sbIdentificacao_Click(object sender, EventArgs e)
        {
            switch ((int)rgTipo.EditValue)
            {
                case 0:
                    FormGridEmpresa formEmp = new FormGridEmpresa();
                    formEmp.cwTabela = "Empresa";
                    formEmp.cwId = (int)cbIdentificacao.EditValue;
                    GridSelecao(formEmp, cbIdentificacao, bllEmpresa);
                    break;
                case 1:
                    FormGridDepartamento formDep = new FormGridDepartamento();
                    formDep.cwTabela = "Departamento";
                    formDep.cwId = (int)cbIdentificacao.EditValue;
                    GridSelecao(formDep, cbIdentificacao, bllDepartamento);
                    break;
                case 2:
                    FormGridFuncionario formFunc = new FormGridFuncionario();
                    formFunc.cwTabela = "Funcionário";
                    formFunc.cwId = (int)cbIdentificacao.EditValue;
                    GridSelecao(formFunc, cbIdentificacao, bllFuncionario);
                    break;
                case 3:
                    FormGridFuncao formFuncao = new FormGridFuncao();
                    formFuncao.cwTabela = "Função";
                    formFuncao.cwId = (int)cbIdentificacao.EditValue;
                    GridSelecao(formFuncao, cbIdentificacao, bllFuncao);
                    break;
                default:
                    break;
            }
        }
    }
}
