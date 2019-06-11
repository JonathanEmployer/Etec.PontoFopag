using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutCompensacao : UI.Base.ManutBase
    {
        private Modelo.Compensacao objCompensacao;
        private BLL.Compensacao bllCompensacao;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcao bllFuncao;
        private BLL.Funcionario bllFuncionario;
        private BLL.Marcacao bllMarcacao;

        public FormManutCompensacao()
        {
            InitializeComponent();
            this.Name = "FormManutCompensacao";
            bllCompensacao = new BLL.Compensacao();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncao = new BLL.Funcao();
            bllFuncionario = new BLL.Funcionario();
            bllMarcacao = new BLL.Marcacao();
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objCompensacao = new Modelo.Compensacao();
                    objCompensacao.DiasC = new List<Modelo.DiasCompensacao>();
                    objCompensacao.Codigo = bllCompensacao.MaxCodigo();
                    objCompensacao.Diacompensarinicial = null;
                    objCompensacao.Diacompensarfinal = null;
                    objCompensacao.Periodoinicial = null;
                    objCompensacao.Periodofinal = null;
                    objCompensacao.Tipo = -1;
                    objCompensacao.Totalhorassercompensadas_1 = "--:--";
                    objCompensacao.Totalhorassercompensadas_2 = "--:--";
                    objCompensacao.Totalhorassercompensadas_3 = "--:--";
                    objCompensacao.Totalhorassercompensadas_4 = "--:--";
                    objCompensacao.Totalhorassercompensadas_5 = "--:--";
                    objCompensacao.Totalhorassercompensadas_6 = "--:--";
                    objCompensacao.Totalhorassercompensadas_7 = "--:--";
                    objCompensacao.Totalhorassercompensadas_8 = "--:--";
                    break;
                default:
                    objCompensacao = bllCompensacao.LoadObject(cwID);
                    break;
            }

            LoadDiasC();

            txtCodigo.DataBindings.Add("EditValue", objCompensacao, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipo.DataBindings.Add("EditValue", objCompensacao, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdentificacao.DataBindings.Add("EditValue", objCompensacao, "Identificacao", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_1.DataBindings.Add("Checked", objCompensacao, "Dias_1", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_2.DataBindings.Add("Checked", objCompensacao, "Dias_2", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_3.DataBindings.Add("Checked", objCompensacao, "Dias_3", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_4.DataBindings.Add("Checked", objCompensacao, "Dias_4", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_5.DataBindings.Add("Checked", objCompensacao, "Dias_5", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_6.DataBindings.Add("Checked", objCompensacao, "Dias_6", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_7.DataBindings.Add("Checked", objCompensacao, "Dias_7", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_8.DataBindings.Add("Checked", objCompensacao, "Dias_8", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotalhorassercompensadas_1.DataBindings.Add("EditValue", objCompensacao, "Totalhorassercompensadas_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotalhorassercompensadas_2.DataBindings.Add("EditValue", objCompensacao, "Totalhorassercompensadas_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotalhorassercompensadas_3.DataBindings.Add("EditValue", objCompensacao, "Totalhorassercompensadas_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotalhorassercompensadas_4.DataBindings.Add("EditValue", objCompensacao, "Totalhorassercompensadas_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotalhorassercompensadas_5.DataBindings.Add("EditValue", objCompensacao, "Totalhorassercompensadas_5", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotalhorassercompensadas_6.DataBindings.Add("EditValue", objCompensacao, "Totalhorassercompensadas_6", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotalhorassercompensadas_7.DataBindings.Add("EditValue", objCompensacao, "Totalhorassercompensadas_7", true, DataSourceUpdateMode.OnPropertyChanged);
            txtTotalhorassercompensadas_8.DataBindings.Add("EditValue", objCompensacao, "Totalhorassercompensadas_8", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPeriodoinicial.DataBindings.Add("EditValue", objCompensacao, "Periodoinicial", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPeriodofinal.DataBindings.Add("EditValue", objCompensacao, "Periodofinal", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDiascompensarinicial.DataBindings.Add("EditValue", objCompensacao, "Diacompensarinicial", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDiascompensarfinal.DataBindings.Add("EditValue", objCompensacao, "Diacompensarfinal", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            Dictionary<string,string> ret = new Dictionary<string,string> ();
            if (bllMarcacao.QuantidadeCompensada(objCompensacao.Id) > 0)
            {
                MessageBox.Show("Você deve desfazer esta compensação antes de excluí-la!");
                ret.Add("", "");
            }
            else
            {
                if ((chbDias_1.Checked == false) && (chbDias_2.Checked == false) && (chbDias_3.Checked == false) && (chbDias_4.Checked == false) &&
                   (chbDias_5.Checked == false) && (chbDias_6.Checked == false) && (chbDias_7.Checked == false) && (chbDias_8.Checked == false))
                {
                    ret.Add("", "");
                    MessageBox.Show("Você deve selecionar um dia da semana para ser compensado");
                }
                else
                {
                    this.Enabled = false;
                    FormProgressBar2 pb = new FormProgressBar2();
                    pb.Show();
                    bllCompensacao.ObjProgressBar = pb.ObjProgressBar;
                    ret = bllCompensacao.Salvar(cwAcao, objCompensacao);
                    pb.Close();
                    this.Enabled = true;
                }
            }
            return ret;
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
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "nome";
                    break;
                case 1: // Departamento
                    cbIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllDepartamento.GetAll();
                    cbIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdentificacao.Properties.Columns["empresa"].Visible = true;
                    cbIdentificacao.Properties.DisplayMember = "descricao";
                    break;

                case 2: // Funcionário
                    cbIdentificacao.Properties.DataSource = bllFuncionario.GetFuncionariosAtivos();
                    cbIdentificacao.Properties.Columns["nome"].Visible = true;
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdentificacao.Properties.Columns["descricao"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "nome";
                    break;
                case 3: // Função
                    cbIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllFuncao.GetAll();
                    cbIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "descricao";
                    break;     
            }
        }

        #region Data Compensada

        private void LoadDiasC()
        {
            List<Modelo.DiasCompensacao> lista = new List<Modelo.DiasCompensacao>();
            foreach (Modelo.DiasCompensacao dja in objCompensacao.DiasC)
            {
                if (dja.Acao != Modelo.Acao.Excluir)
                {
                    lista.Add(dja);
                }
            }
            gcDiasCompensacao.DataSource = lista;
        }

        private void CarregaFormDiasCompensacao(Modelo.Acao pAcao, int pCodigo)
        {
            if (pAcao != Modelo.Acao.Incluir && pCodigo == 0)
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
            else
            {
                UI.FormManutDiasCompensacao form = new UI.FormManutDiasCompensacao(objCompensacao);
                form.cwAcao = pAcao;
                form.cwID = pCodigo;
                form.cwTabela = "Dias Compensação";
                form.ShowDialog();

                if (pAcao != Modelo.Acao.Consultar)
                {
                    LoadDiasC();
                }
            }
        }

        private Int32 DiasCSelecionado()
        {
            Int32 seq;
            try
            {
                seq = (int)gvDiasCompensacao.GetFocusedRowCellValue("Codigo");
            }
            catch (Exception)
            {
                seq = 0;
            }
            return seq;
        }

        private void sbIncluir_Click(object sender, EventArgs e)
        {
            CarregaFormDiasCompensacao(Modelo.Acao.Incluir, 0);
        }

        private void sbAlterar_Click(object sender, EventArgs e)
        {
            CarregaFormDiasCompensacao(Modelo.Acao.Alterar, DiasCSelecionado());
        }

        private void sbExcluir_Click(object sender, EventArgs e)
        {
            CarregaFormDiasCompensacao(Modelo.Acao.Excluir, DiasCSelecionado());
        }

        private void gcDiasCompensacao_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CarregaFormDiasCompensacao(Modelo.Acao.Alterar, DiasCSelecionado());
                    break;
            }
        }

        private void gcDiasCompensacao_DoubleClick(object sender, EventArgs e)
        {
            CarregaFormDiasCompensacao(Modelo.Acao.Alterar, DiasCSelecionado());
        }

        #endregion

        private void sbIdentificacao_Click(object sender, EventArgs e)
        {
            switch ((int) rgTipo.EditValue)
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

        private void chbDias_1_CheckedChanged(object sender, EventArgs e)
        {
            txtTotalhorassercompensadas_1.Enabled = chbDias_1.Checked;

            if (!chbDias_1.Checked)
            {
                objCompensacao.Totalhorassercompensadas_1 = "--:--";
            }
        }

        private void chbDias_2_CheckedChanged(object sender, EventArgs e)
        {
            txtTotalhorassercompensadas_2.Enabled = chbDias_2.Checked;

            if (!chbDias_2.Checked)
            {
                objCompensacao.Totalhorassercompensadas_2 = "--:--";
            }
        }

        private void chbDias_3_CheckedChanged(object sender, EventArgs e)
        {
            txtTotalhorassercompensadas_3.Enabled = chbDias_3.Checked;

            if (!chbDias_3.Checked)
            {
                objCompensacao.Totalhorassercompensadas_3 = "--:--";
            }
        }

        private void chbDias_4_CheckedChanged(object sender, EventArgs e)
        {
            txtTotalhorassercompensadas_4.Enabled = chbDias_4.Checked;

            if (!chbDias_4.Checked)
            {
                objCompensacao.Totalhorassercompensadas_4 = "--:--";
            }
        }

        private void chbDias_5_CheckedChanged(object sender, EventArgs e)
        {
            txtTotalhorassercompensadas_5.Enabled = chbDias_5.Checked;

            if (!chbDias_5.Checked)
            {
                objCompensacao.Totalhorassercompensadas_5 = "--:--";
            }
        }

        private void chbDias_6_CheckedChanged(object sender, EventArgs e)
        {
            txtTotalhorassercompensadas_6.Enabled = chbDias_6.Checked;

            if (!chbDias_6.Checked)
            {
                objCompensacao.Totalhorassercompensadas_6 = "--:--";
            }
        }

        private void chbDias_7_CheckedChanged(object sender, EventArgs e)
        {
            txtTotalhorassercompensadas_7.Enabled = chbDias_7.Checked;

            if (!chbDias_7.Checked)
            {
                objCompensacao.Totalhorassercompensadas_7 = "--:--";
            }
        }

        private void chbDias_8_CheckedChanged(object sender, EventArgs e)
        {
            txtTotalhorassercompensadas_8.Enabled = chbDias_8.Checked;

            if (!chbDias_8.Checked)
            {
                objCompensacao.Totalhorassercompensadas_8 = "--:--";
            }
        }
    }
}
