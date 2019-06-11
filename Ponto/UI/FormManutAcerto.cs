using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace UI
{
    public partial class FormManutAcerto : UI.Base.ManutBase
    {
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcionario bllFuncionario;
        private BLL.Funcao bllFuncao;
        private BLL.Marcacao bllMarcacao;
        private BLL.BancoHoras bllBancoHoras;
        private BLL.FechamentoBH bllFechamentoBH;

        Modelo.FechamentoBHD objFechBHD;
        Modelo.FechamentoBH  objFechBH;

        public FormManutAcerto()
        {
            InitializeComponent();
            this.Name = "FormManutAcerto";
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncionario = new BLL.Funcionario();
            bllFuncao = new BLL.Funcao();
            bllMarcacao = new BLL.Marcacao();
            bllBancoHoras = new BLL.BancoHoras();
            bllFechamentoBH = new BLL.FechamentoBH();
        }

        public override void CarregaObjeto()
        {
            objFechBHD = new Modelo.FechamentoBHD();
            objFechBH = new Modelo.FechamentoBH();

            cbBancoHoras.Properties.DataSource = bllBancoHoras.GetAll();
        }

        private void sbIdIdentificacao_Click(object sender, EventArgs e)
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
                    FormGridFuncionario formFun = new FormGridFuncionario();
                    formFun.cwTabela = "Funcionário";
                    formFun.cwId = (int)cbIdentificacao.EditValue;
                    GridSelecao(formFun, cbIdentificacao, bllFuncionario);
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

        private void btBancoHoras_Click(object sender, EventArgs e)
        {
            FormGridBancoHoras form = new FormGridBancoHoras();
            form.cwTabela = "Banco de Horas";
            form.cwId = (int)cbBancoHoras.EditValue;
            GridSelecao(form, cbBancoHoras, bllBancoHoras);
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
                    cbIdentificacao.Properties.Columns["dscodigo"].Visible = false;
                    cbIdentificacao.Properties.Columns["codigo"].Visible = true;
                    break;
                case 1: // Departamento
                    cbIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllDepartamento.GetAll();
                    cbIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "descricao";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = true;
                    cbIdentificacao.Properties.Columns["dscodigo"].Visible = false;
                    cbIdentificacao.Properties.Columns["codigo"].Visible = true;
                    break;

                case 2: // Funcionário
                    cbIdentificacao.Properties.DataSource = bllFuncionario.GetFuncionariosAtivos();
                    cbIdentificacao.Properties.Columns["descricao"].Visible = false;
                    cbIdentificacao.Properties.Columns["nome"].Visible = true;
                    cbIdentificacao.Properties.DisplayMember = "nome";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdentificacao.Properties.Columns["codigo"].Visible = false;
                    cbIdentificacao.Properties.Columns["dscodigo"].Visible = true;
                    cbIdentificacao.Properties.Columns["dscodigo"].Caption = "Código";                
                    break;
                case 3: // Função
                    cbIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllFuncao.GetAll();
                    cbIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "descricao";
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdentificacao.Properties.Columns["dscodigo"].Visible = false;
                    cbIdentificacao.Properties.Columns["codigo"].Visible = true;
                    break;
            }
        }

        public override Dictionary<string, string> Salvar()
        {
            this.Enabled = false;
            Dictionary<string, string> ret = new Dictionary<string, string>();
            FormProgressBar2 pbAcerto = new FormProgressBar2();

            if (Convert.ToInt16(rgTipo.EditValue) == -1 )
            {
                ret.Add("rgTipo", "Campo obrigatório");
            }
            if ((int)cbIdentificacao.EditValue == -1 )
            {
                ret.Add("cbIdentificacao", "Campo obrigatório");
            }
            if ((int)cbBancoHoras.EditValue == -1 )
            {
                ret.Add("cbBancoHoras", "Campo obrigatório");
            }
            if (data.DateTime == new DateTime())
            {
                ret.Add("data", "Campo obrigatório");
            }

            if (ret.Count == 0)
            {

                try
                {
                    pbAcerto.Show(this);
                    bllFechamentoBH.ObjProgressBar = pbAcerto.ObjProgressBar;
                    ret = bllFechamentoBH.FechamentoBancoHoras(Convert.ToInt16(rgTipo.EditValue), (int)cbIdentificacao.EditValue, (int)cbBancoHoras.EditValue, data.DateTime, "", chbPagamentoHoraAutomatico.Checked, chbPagamentoHoraAutomaticoDebito.Checked, "","");

                }
                catch (Exception ex)
                {
                    pbAcerto.Close();
                    MessageBox.Show(this, ex.Message, "Mensagem");
                }
                finally
                {
                    pbAcerto.Close();
                }
                
            }
            this.Enabled = true;
            return ret;
        }

        private bool ValidaCampos()
        {
            bool ret = true;

            if ((int)cbIdentificacao.EditValue > 0)
                errorProvider1.SetError(cbIdentificacao, "");
            else
            {
                errorProvider1.SetError(cbIdentificacao, "Escolha a identificação.");
                ret = false;
            }

            if ((int)cbBancoHoras.EditValue > 0)
                errorProvider1.SetError(cbBancoHoras, "");
            else
            {
                errorProvider1.SetError(cbBancoHoras, "Escolha o banco de horas.");
                ret = false;
            }

            if ((int)rgTipo.EditValue > 0)
                errorProvider1.SetError(rgTipo, "");
            else
            {
                errorProvider1.SetError(rgTipo, "Escolha o tipo do acerto.");
                ret = false;
            }

            if (data.DateTime != new DateTime())
            {
                errorProvider1.SetError(data, "");
            }
            else
            {
                errorProvider1.SetError(data, "Selecione a data do fechamento do banco.");
                ret = false;
            }

            return ret;

        }
    }
}
