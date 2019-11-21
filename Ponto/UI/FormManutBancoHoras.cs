using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutBancoHoras : UI.Base.ManutBase
    {
        private Modelo.BancoHoras objBancoHoras;
        private BLL.BancoHoras bllBancoHoras;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcao bllFuncao;
        private BLL.Funcionario bllFuncionario;

        private bool bmarcar;


        public FormManutBancoHoras()
        {
            InitializeComponent();
            this.Name = "FormManutBancoHoras";
            bllBancoHoras = new BLL.BancoHoras();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncao = new BLL.Funcao();
            bllFuncionario = new BLL.Funcionario();
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objBancoHoras = new Modelo.BancoHoras();
                    objBancoHoras.Codigo = bllBancoHoras.MaxCodigo();
                    objBancoHoras.DataInicial = null;
                    objBancoHoras.DataFinal = null;
                    objBancoHoras.Tipo = -1;
                    objBancoHoras.LimiteHoras_1 = "--:--";
                    objBancoHoras.LimiteHoras_2 = "--:--";
                    objBancoHoras.LimiteHoras_3 = "--:--";
                    objBancoHoras.LimiteHoras_4 = "--:--";
                    objBancoHoras.LimiteHoras_5 = "--:--";
                    objBancoHoras.LimiteHoras_6 = "--:--";
                    objBancoHoras.LimiteHoras_7 = "--:--";
                    objBancoHoras.LimiteHoras_8 = "--:--";
                    objBancoHoras.LimiteHoras_9 = "--:--";
                    objBancoHoras.LimiteHorasextras_1 = "--:--";
                    objBancoHoras.LimiteHorasextras_2 = "--:--";
                    objBancoHoras.LimiteHorasextras_3 = "--:--";
                    objBancoHoras.LimiteHorasextras_4 = "--:--";
                    objBancoHoras.LimiteHorasextras_5 = "--:--";
                    objBancoHoras.LimiteHorasextras_6 = "--:--";
                    objBancoHoras.LimiteHorasextras_7 = "--:--";
                    objBancoHoras.LimiteHorasextras_8 = "--:--";
                    objBancoHoras.LimiteHorasextras_9 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_1 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_2 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_3 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_4 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_5 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_6 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_7 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_8 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_9 = "--:--";
                    objBancoHoras.LimiteQtdHoras_1 = "--:--";
                    objBancoHoras.LimiteQtdHoras_2 = "--:--";
                    objBancoHoras.LimiteQtdHoras_3 = "--:--";
                    objBancoHoras.LimiteQtdHoras_4 = "--:--";
                    objBancoHoras.LimiteQtdHoras_5 = "--:--";
                    objBancoHoras.LimiteQtdHoras_6 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_1 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_2 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_3 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_4 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_5 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_6 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_7 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_8 = "--:--";
                    objBancoHoras.LimiteHorasDiarios_9 = "--:--";


                    HabilitaPrimeiroBanco(false);
                    HabilitaPrimeiroExtra(false);                    
                    break;
                default:
                    objBancoHoras = bllBancoHoras.LoadObject(cwID);                    

                    chbExtraPrimeiro.Checked = Convert.ToBoolean(objBancoHoras.ExtraPrimeiro);
                    chbBancoprimeiro.Checked = Convert.ToBoolean(objBancoHoras.Bancoprimeiro);

                    if (!chbExtraPrimeiro.Checked)
                    {
                        HabilitaPrimeiroExtra(false);
                    }
                    
                    if (!chbBancoprimeiro.Checked)
                    {
                        HabilitaPrimeiroBanco(false);
                    }
                    break;
            }

            HabilitaPercentuais(false);
            bmarcar = !(objBancoHoras.Dias_1 == 1 && objBancoHoras.Dias_2 == 1 && objBancoHoras.Dias_3 == 1 && objBancoHoras.Dias_4 == 1 && objBancoHoras.Dias_5 == 1 && objBancoHoras.Dias_6 == 1 && objBancoHoras.Dias_7 == 1 && objBancoHoras.Dias_8 == 1 && objBancoHoras.Dias_9 == 1);

            txtCodigo.DataBindings.Add("EditValue", objBancoHoras, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipo.DataBindings.Add("EditValue", objBancoHoras, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdentificacao.DataBindings.Add("EditValue", objBancoHoras, "Identificacao", true, DataSourceUpdateMode.OnPropertyChanged);
            //chbBancoprimeiro.DataBindings.Add("Checked", objBancoHoras, "Bancoprimeiro", true, DataSourceUpdateMode.OnPropertyChanged);
            //chbExtraPrimeiro.DataBindings.Add("Checked", objBancoHoras, "ExtraPrimeiro", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_1.DataBindings.Add("Checked", objBancoHoras, "Dias_1", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_2.DataBindings.Add("Checked", objBancoHoras, "Dias_2", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_3.DataBindings.Add("Checked", objBancoHoras, "Dias_3", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_4.DataBindings.Add("Checked", objBancoHoras, "Dias_4", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_5.DataBindings.Add("Checked", objBancoHoras, "Dias_5", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_6.DataBindings.Add("Checked", objBancoHoras, "Dias_6", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_7.DataBindings.Add("Checked", objBancoHoras, "Dias_7", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_8.DataBindings.Add("Checked", objBancoHoras, "Dias_8", true, DataSourceUpdateMode.OnPropertyChanged);
            chbDias_9.DataBindings.Add("Checked", objBancoHoras, "Dias_9", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHoras_1.DataBindings.Add("EditValue", objBancoHoras, "LimiteHoras_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHoras_2.DataBindings.Add("EditValue", objBancoHoras, "LimiteHoras_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHoras_3.DataBindings.Add("EditValue", objBancoHoras, "LimiteHoras_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHoras_4.DataBindings.Add("EditValue", objBancoHoras, "LimiteHoras_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHoras_5.DataBindings.Add("EditValue", objBancoHoras, "LimiteHoras_5", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHoras_6.DataBindings.Add("EditValue", objBancoHoras, "LimiteHoras_6", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHoras_7.DataBindings.Add("EditValue", objBancoHoras, "LimiteHoras_7", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHoras_8.DataBindings.Add("EditValue", objBancoHoras, "LimiteHoras_8", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHoras_9.DataBindings.Add("EditValue", objBancoHoras, "LimiteHoras_9", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasDiarios_1.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasDiarios_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasDiarios_2.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasDiarios_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasDiarios_3.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasDiarios_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasDiarios_4.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasDiarios_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasDiarios_5.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasDiarios_5", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasDiarios_6.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasDiarios_6", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasDiarios_7.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasDiarios_7", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasDiarios_8.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasDiarios_8", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasDiarios_9.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasDiarios_9", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasextras_1.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasextras_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasextras_2.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasextras_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasextras_3.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasextras_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasextras_4.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasextras_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasextras_5.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasextras_5", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasextras_6.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasextras_6", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasextras_7.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasextras_7", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasextras_8.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasextras_8", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimiteHorasextras_9.DataBindings.Add("EditValue", objBancoHoras, "LimiteHorasextras_9", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentuais_1.DataBindings.Add("EditValue", objBancoHoras, "Percentuais_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentuais_2.DataBindings.Add("EditValue", objBancoHoras, "Percentuais_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentuais_3.DataBindings.Add("EditValue", objBancoHoras, "Percentuais_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentuais_4.DataBindings.Add("EditValue", objBancoHoras, "Percentuais_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentuais_5.DataBindings.Add("EditValue", objBancoHoras, "Percentuais_5", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentuais_6.DataBindings.Add("EditValue", objBancoHoras, "Percentuais_6", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentuais_7.DataBindings.Add("EditValue", objBancoHoras, "Percentuais_7", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentuais_8.DataBindings.Add("EditValue", objBancoHoras, "Percentuais_8", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPercentuais_9.DataBindings.Add("EditValue", objBancoHoras, "Percentuais_9", true, DataSourceUpdateMode.OnPropertyChanged);
            chbFaltaDebito.DataBindings.Add("Checked", objBancoHoras, "FaltaDebito", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDataInicial.DataBindings.Add("DateTime", objBancoHoras, "DataInicial", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDataFinal.DataBindings.Add("DateTime", objBancoHoras, "DataFinal", true, DataSourceUpdateMode.OnPropertyChanged);
            chbBancohorasacumulativo.DataBindings.Add("Checked", objBancoHoras, "BancoHorasAcumulativo", true, DataSourceUpdateMode.OnPropertyChanged);
            chbLimite1.DataBindings.Add("Checked", objBancoHoras, "Limite_1", true, DataSourceUpdateMode.OnPropertyChanged);
            chbLimite2.DataBindings.Add("Checked", objBancoHoras, "Limite_2", true, DataSourceUpdateMode.OnPropertyChanged);
            chbLimite3.DataBindings.Add("Checked", objBancoHoras, "Limite_3", true, DataSourceUpdateMode.OnPropertyChanged);
            chbLimite4.DataBindings.Add("Checked", objBancoHoras, "Limite_4", true, DataSourceUpdateMode.OnPropertyChanged);
            chbLimite5.DataBindings.Add("Checked", objBancoHoras, "Limite_5", true, DataSourceUpdateMode.OnPropertyChanged);
            chbLimite6.DataBindings.Add("Checked", objBancoHoras, "Limite_6", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimite1.DataBindings.Add("EditValue", objBancoHoras, "LimitePctHoras_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimite2.DataBindings.Add("EditValue", objBancoHoras, "LimitePctHoras_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimite3.DataBindings.Add("EditValue", objBancoHoras, "LimitePctHoras_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimite4.DataBindings.Add("EditValue", objBancoHoras, "LimitePctHoras_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimite5.DataBindings.Add("EditValue", objBancoHoras, "LimitePctHoras_5", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLimite6.DataBindings.Add("EditValue", objBancoHoras, "LimitePctHoras_6", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQtdHoras1.DataBindings.Add("EditValue", objBancoHoras, "LimiteQtdHoras_1", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQtdHoras2.DataBindings.Add("EditValue", objBancoHoras, "LimiteQtdHoras_2", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQtdHoras3.DataBindings.Add("EditValue", objBancoHoras, "LimiteQtdHoras_3", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQtdHoras4.DataBindings.Add("EditValue", objBancoHoras, "LimiteQtdHoras_4", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQtdHoras5.DataBindings.Add("EditValue", objBancoHoras, "LimiteQtdHoras_5", true, DataSourceUpdateMode.OnPropertyChanged);
            txtQtdHoras6.DataBindings.Add("EditValue", objBancoHoras, "LimiteQtdHoras_6", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipoAcumulo.DataBindings.Add("EditValue", objBancoHoras, "TipoAcumulo", true, DataSourceUpdateMode.OnPropertyChanged);
            chbBancoHorasPorPercentual.Checked = objBancoHoras.BancoHorasPorPercentual;
            chbPerccomohoraextra.Checked = objBancoHoras.PercentualComoHoraExtra;
            chbBancohorasacumulativo.Checked = objBancoHoras.BancoHorasAcumulativo;
			if (objBancoHoras.BancoHorasAcumulativo)
            {
                HabilitaPrimeiroBanco(objBancoHoras.BancoHorasAcumulativo);
                HabilitaPrimeiroExtra(!objBancoHoras.BancoHorasAcumulativo);
            }
            tpTipoAcumulo.PageEnabled = chbBancohorasacumulativo.Checked;
            tpTipoAcumulo.PageVisible = chbBancohorasacumulativo.Checked;
            TrocaStatusEdicaoCampo(chbLimite1.Checked, new DevExpress.XtraEditors.BaseEdit[] { txtLimite1, txtQtdHoras1 });
            TrocaStatusEdicaoCampo(chbLimite2.Checked, new DevExpress.XtraEditors.BaseEdit[] { txtLimite2, txtQtdHoras2 });
            TrocaStatusEdicaoCampo(chbLimite3.Checked, new DevExpress.XtraEditors.BaseEdit[] { txtLimite3, txtQtdHoras3 });
            TrocaStatusEdicaoCampo(chbLimite4.Checked, new DevExpress.XtraEditors.BaseEdit[] { txtLimite4, txtQtdHoras4 });
            TrocaStatusEdicaoCampo(chbLimite5.Checked, new DevExpress.XtraEditors.BaseEdit[] { txtLimite5, txtQtdHoras5 });
            TrocaStatusEdicaoCampo(chbLimite6.Checked, new DevExpress.XtraEditors.BaseEdit[] { txtLimite6, txtQtdHoras6 });
            TrocaStatusEdicaoCampo(!chbBancohorasacumulativo.Checked, new DevExpress.XtraEditors.BaseEdit[]{
                txtPercentuais_1,
                txtPercentuais_2,
                txtPercentuais_3,
                txtPercentuais_4,
                txtPercentuais_5,
                chbExtraPrimeiro
            });
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            this.Enabled = false;
            objBancoHoras.ExtraPrimeiro = Convert.ToInt32(chbExtraPrimeiro.Checked);
            objBancoHoras.Bancoprimeiro = Convert.ToInt32(chbBancoprimeiro.Checked);
            objBancoHoras.PercentualComoHoraExtra = chbPerccomohoraextra.Checked;
            objBancoHoras.BancoHorasPorPercentual = chbBancoHorasPorPercentual.Checked; 
            base.Salvar();
            Dictionary<string, string> ret = bllBancoHoras.Salvar(cwAcao, objBancoHoras);

            if (ret.Count == 0)
            {
                FormProgressBar2 pb = new FormProgressBar2();
                pb.Show(this);
                try
                {
                    bllBancoHoras.ObjProgressBar = pb.ObjProgressBar;
                    bllBancoHoras.AtualizaMarcacao(cwAcao, objBancoHoras);
                }
                catch (Exception ex)
                {
                    pb.Close();
                    MessageBox.Show(this, ex.Message, "Mensagem");
                }
                finally
                {
                    pb.Close();
                }
            }
            this.Enabled = true;

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
                    if (objBancoHoras.Tipo_Ant != (int)rgTipo.EditValue)
                    {
                        objBancoHoras.Identificacao = 0;
                    }
                    cbIdentificacao.Properties.Columns["nome"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllEmpresa.GetAll();
                    cbIdentificacao.Properties.Columns["descricao"].Visible = false;
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "nome";
                    break;
                case 1: // Departamento
                    if (objBancoHoras.Tipo_Ant != (int)rgTipo.EditValue)
                    {
                        objBancoHoras.Identificacao = 0;
                    }
                    cbIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllDepartamento.GetAll();
                    cbIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdentificacao.Properties.Columns["empresa"].Visible = true;
                    cbIdentificacao.Properties.DisplayMember = "descricao";
                    break;
                case 2: // Funcionário
                    if (objBancoHoras.Tipo_Ant != (int)rgTipo.EditValue)
                    {
                        objBancoHoras.Identificacao = 0;
                    }
                    cbIdentificacao.Properties.DataSource = bllFuncionario.GetFuncionariosAtivos();
                    cbIdentificacao.Properties.Columns["nome"].Visible = true;
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdentificacao.Properties.Columns["descricao"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "nome";

                    break;
                case 3: // Função
                    if (objBancoHoras.Tipo_Ant != (int)rgTipo.EditValue)
                    {
                        objBancoHoras.Identificacao = 0;
                    }
                    cbIdentificacao.Properties.Columns["descricao"].Visible = true;
                    cbIdentificacao.Properties.DataSource = bllFuncao.GetAll();
                    cbIdentificacao.Properties.Columns["nome"].Visible = false;
                    cbIdentificacao.Properties.Columns["empresa"].Visible = false;
                    cbIdentificacao.Properties.DisplayMember = "descricao";
                    break;

            }
        }

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

        private void sbMarcaDesmarca_Click(object sender, EventArgs e)
        {
            chbDias_1.Checked = bmarcar;
            chbDias_2.Checked = bmarcar;
            chbDias_3.Checked = bmarcar;
            chbDias_4.Checked = bmarcar;
            chbDias_5.Checked = bmarcar;
            chbDias_6.Checked = bmarcar;
            chbDias_7.Checked = bmarcar;
            chbDias_8.Checked = bmarcar;
            chbDias_9.Checked = bmarcar;

            bmarcar = !bmarcar;
        }

        private void chbBancoprimeiro_CheckedChanged(object sender, EventArgs e)
        {
            HabilitaPrimeiroBanco(chbBancoprimeiro.Checked);
        }

        private void HabilitaPrimeiroBanco(bool Hab)
        {
            if (Hab)
            {
                chbExtraPrimeiro.Checked = false;
            }

            else
            {
                txtLimiteHoras_1.EditValue = "--:--";
                txtLimiteHoras_2.EditValue = "--:--";
                txtLimiteHoras_3.EditValue = "--:--";
                txtLimiteHoras_4.EditValue = "--:--";
                txtLimiteHoras_5.EditValue = "--:--";
                txtLimiteHoras_6.EditValue = "--:--";
                txtLimiteHoras_7.EditValue = "--:--";
                txtLimiteHoras_8.EditValue = "--:--";
                txtLimiteHoras_9.EditValue = "--:--";

                txtLimiteHorasDiarios_1.EditValue = "--:--";
                txtLimiteHorasDiarios_2.EditValue = "--:--";
                txtLimiteHorasDiarios_3.EditValue = "--:--";
                txtLimiteHorasDiarios_4.EditValue = "--:--";
                txtLimiteHorasDiarios_5.EditValue = "--:--";
                txtLimiteHorasDiarios_6.EditValue = "--:--";
                txtLimiteHorasDiarios_7.EditValue = "--:--";
                txtLimiteHorasDiarios_8.EditValue = "--:--";
                txtLimiteHorasDiarios_9.EditValue = "--:--";
            }

            txtLimiteHoras_1.Enabled = (Hab && chbDias_1.Checked);
            txtLimiteHoras_2.Enabled = (Hab && chbDias_2.Checked);
            txtLimiteHoras_3.Enabled = (Hab && chbDias_3.Checked);
            txtLimiteHoras_4.Enabled = (Hab && chbDias_4.Checked);
            txtLimiteHoras_5.Enabled = (Hab && chbDias_5.Checked);
            txtLimiteHoras_6.Enabled = (Hab && chbDias_6.Checked);
            txtLimiteHoras_7.Enabled = (Hab && chbDias_7.Checked);
            txtLimiteHoras_8.Enabled = (Hab && chbDias_8.Checked);
            txtLimiteHoras_9.Enabled = (Hab && chbDias_9.Checked);


            txtLimiteHorasDiarios_1.Enabled = (Hab && chbDias_1.Checked);
            txtLimiteHorasDiarios_2.Enabled = (Hab && chbDias_2.Checked);
            txtLimiteHorasDiarios_3.Enabled = (Hab && chbDias_3.Checked);
            txtLimiteHorasDiarios_4.Enabled = (Hab && chbDias_4.Checked);
            txtLimiteHorasDiarios_5.Enabled = (Hab && chbDias_5.Checked);
            txtLimiteHorasDiarios_6.Enabled = (Hab && chbDias_6.Checked);
            txtLimiteHorasDiarios_7.Enabled = (Hab && chbDias_7.Checked);
            txtLimiteHorasDiarios_8.Enabled = (Hab && chbDias_8.Checked);
            txtLimiteHorasDiarios_9.Enabled = (Hab && chbDias_9.Checked);
        }

        private void HabilitaPrimeiroExtra(bool Hab)
        {
            if (Hab)
            {
                chbBancoprimeiro.Checked = false;
            }

            else
            {
                txtLimiteHorasextras_1.EditValue = "--:--";
                txtLimiteHorasextras_2.EditValue = "--:--";
                txtLimiteHorasextras_3.EditValue = "--:--";
                txtLimiteHorasextras_4.EditValue = "--:--";
                txtLimiteHorasextras_5.EditValue = "--:--";
                txtLimiteHorasextras_6.EditValue = "--:--";
                txtLimiteHorasextras_7.EditValue = "--:--";
                txtLimiteHorasextras_8.EditValue = "--:--";
                txtLimiteHorasextras_9.EditValue = "--:--";
            }

            txtLimiteHorasextras_1.Enabled = (Hab && chbDias_1.Checked);
            txtLimiteHorasextras_2.Enabled = (Hab && chbDias_2.Checked);
            txtLimiteHorasextras_3.Enabled = (Hab && chbDias_3.Checked);
            txtLimiteHorasextras_4.Enabled = (Hab && chbDias_4.Checked);
            txtLimiteHorasextras_5.Enabled = (Hab && chbDias_5.Checked);
            txtLimiteHorasextras_6.Enabled = (Hab && chbDias_6.Checked);
            txtLimiteHorasextras_7.Enabled = (Hab && chbDias_7.Checked);
            txtLimiteHorasextras_8.Enabled = (Hab && chbDias_8.Checked);
            txtLimiteHorasextras_9.Enabled = (Hab && chbDias_9.Checked);            
        }

        private void chbExtraPrimeiro_CheckedChanged(object sender, EventArgs e)
        {
            HabilitaPrimeiroExtra(chbExtraPrimeiro.Checked);
        }

        private void chbDias_1_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentuais_1.Enabled = chbDias_1.Checked;           
            HabilitaDia();
        }
        private void chbDias_2_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentuais_2.Enabled = chbDias_2.Checked;            
            HabilitaDia();
        }
        private void chbDias_3_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentuais_3.Enabled = chbDias_3.Checked;            
            HabilitaDia();
        }
        private void chbDias_4_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentuais_4.Enabled = chbDias_4.Checked;            
            HabilitaDia();
        }
        private void chbDias_5_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentuais_5.Enabled = chbDias_5.Checked;            
            HabilitaDia();
        }
        private void chbDias_6_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentuais_6.Enabled = chbDias_6.Checked;            
            HabilitaDia();
        }
        private void chbDias_7_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentuais_7.Enabled = chbDias_7.Checked;            
            HabilitaDia();
        }
        private void chbDias_8_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentuais_8.Enabled = chbDias_8.Checked;
            HabilitaDia();
        }
        private void chbDias_9_CheckedChanged(object sender, EventArgs e)
        {
            txtPercentuais_9.Enabled = chbDias_9.Checked;
            HabilitaDia();
        }

        private void HabilitaDia()
        {
            if (chbBancoprimeiro.Checked)
            {
                HabilitaPrimeiroBanco(chbBancoprimeiro.Checked);
            }

            else if (chbExtraPrimeiro.Checked)
            {
                HabilitaPrimeiroExtra(chbExtraPrimeiro.Checked);
            }
        }

        private void HabilitaPercentuais(bool Hab)
        {
            txtPercentuais_1.Enabled = Hab;
            txtPercentuais_2.Enabled = Hab;
            txtPercentuais_3.Enabled = Hab;
            txtPercentuais_4.Enabled = Hab;
            txtPercentuais_5.Enabled = Hab;
            txtPercentuais_6.Enabled = Hab;
            txtPercentuais_7.Enabled = Hab;
            txtPercentuais_8.Enabled = Hab;
            txtPercentuais_9.Enabled = Hab;
        }

        private void chbBancohorasacumulativo_CheckedChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.CheckEdit chk = (DevExpress.XtraEditors.CheckEdit)sender;
            if (!chk.Checked && tcConfBH.SelectedTabPage == tpTipoAcumulo)
            {
                tcConfBH.SelectedTabPage = tpParmsBH;
            }
            if (chk.Checked)
            {
                chbBancoprimeiro.Checked = true;
                HabilitaPrimeiroBanco(chbBancoprimeiro.Checked);
                objBancoHoras.Percentuais_1 = 0;
                objBancoHoras.Percentuais_2 = 0;
                objBancoHoras.Percentuais_3 = 0;
                objBancoHoras.Percentuais_4 = 0;
                objBancoHoras.Percentuais_5 = 0;
                objBancoHoras.LimiteHorasextras_1 = "--:--";
                objBancoHoras.LimiteHorasextras_2 = "--:--";
                objBancoHoras.LimiteHorasextras_3 = "--:--";
                objBancoHoras.LimiteHorasextras_4 = "--:--";
                objBancoHoras.LimiteHorasextras_5 = "--:--";
                objBancoHoras.ExtraPrimeiro = 0;
                if (chbExtraPrimeiro.Checked)
                {
                    chbExtraPrimeiro.Checked = false;
                }
            }
            tpTipoAcumulo.PageEnabled = chk.Checked;
            tpTipoAcumulo.PageVisible = chk.Checked;
            TrocaStatusEdicaoCampo(!chk.Checked, new DevExpress.XtraEditors.BaseEdit[]{
                txtPercentuais_1,
                txtPercentuais_2,
                txtPercentuais_3,
                txtPercentuais_4,
                txtPercentuais_5,
                chbExtraPrimeiro
            });
        }

        private void chbLimite1_CheckedChanged(object sender, EventArgs e)
        {
            bool selecionado = ((DevExpress.XtraEditors.CheckEdit)sender).Checked;
            TrocaStatusEdicaoCampo(selecionado, new DevExpress.XtraEditors.BaseEdit[] { txtLimite1, txtQtdHoras1 });
        }

        private void chbLimite2_CheckedChanged(object sender, EventArgs e)
        {
            bool selecionado = ((DevExpress.XtraEditors.CheckEdit)sender).Checked;
            TrocaStatusEdicaoCampo(selecionado, new DevExpress.XtraEditors.BaseEdit[] { txtLimite2, txtQtdHoras2 });
        }

        private void chbLimite3_CheckedChanged(object sender, EventArgs e)
        {
            bool selecionado = ((DevExpress.XtraEditors.CheckEdit)sender).Checked;
            TrocaStatusEdicaoCampo(selecionado, new DevExpress.XtraEditors.BaseEdit[] { txtLimite3, txtQtdHoras3 });
        }

        private void chbLimite4_CheckedChanged(object sender, EventArgs e)
        {
            bool selecionado = ((DevExpress.XtraEditors.CheckEdit)sender).Checked;
            TrocaStatusEdicaoCampo(selecionado, new DevExpress.XtraEditors.BaseEdit[] { txtLimite4, txtQtdHoras4 });
        }

        private void chbLimite5_CheckedChanged(object sender, EventArgs e)
        {
            bool selecionado = ((DevExpress.XtraEditors.CheckEdit)sender).Checked;
            TrocaStatusEdicaoCampo(selecionado, new DevExpress.XtraEditors.BaseEdit[] { txtLimite5, txtQtdHoras5 });
        }

        private void chbLimite6_CheckedChanged(object sender, EventArgs e)
        {
            bool selecionado = ((DevExpress.XtraEditors.CheckEdit)sender).Checked;
            TrocaStatusEdicaoCampo(selecionado, new DevExpress.XtraEditors.BaseEdit[] { txtLimite6, txtQtdHoras6 });
        }

        private void TrocaStatusEdicaoCampo(bool status, DevExpress.XtraEditors.BaseEdit componente)
        {
            componente.Enabled = status;
        }

        private void TrocaStatusEdicaoCampo(bool status, DevExpress.XtraEditors.BaseEdit[] componentes)
        {
            foreach (var item in componentes)
            {
                TrocaStatusEdicaoCampo(status, item);
            }
        }

        private void rgTipoAcumulo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chbBancoHorasPorPercentual_CheckedChanged(object sender, EventArgs e)
        {
            if (chbBancoHorasPorPercentual.Checked == true)
            {
                chbPerccomohoraextra.Checked = false;
                chbBancoHorasPorPercentual.Checked = true;
            }
            else
            {
                chbBancoHorasPorPercentual.Checked = false;
            }
        }

        private void chbPerccomohoraextra_CheckedChanged(object sender, EventArgs e)
        {
            if (chbPerccomohoraextra.Checked == true)
            {
                chbPerccomohoraextra.Checked = true;
                chbBancoHorasPorPercentual.Checked = false;
            }
            else
            {
                chbPerccomohoraextra.Checked = false;
            }
        }

        #region Habilita e Dasabilita TxtLimiteHoras
        private void txtLimiteHoras_1_Leave(object sender, EventArgs e)
        {
            if (txtLimiteHoras_1.Text != "--:--")
            {
                 txtLimiteHorasDiarios_1.Text = "--:--";
            }
        }

        private void  txtLimiteHorasDiarios_1_Leave(object sender, EventArgs e)
        {
            if ( txtLimiteHorasDiarios_1.Text != "--:--")
            {
                txtLimiteHoras_1.Text = "--:--";
            }
        }

        private void txtLimiteHoras_2_Leave(object sender, EventArgs e)
        {
            if (txtLimiteHoras_2.Text != "--:--")
            {
                 txtLimiteHorasDiarios_2.Text = "--:--";
            }
        }

        private void  txtLimiteHorasDiarios_2_Leave(object sender, EventArgs e)
        {
            if ( txtLimiteHorasDiarios_2.Text != "--:--")
            {
                txtLimiteHoras_2.Text = "--:--";
            }
        }

        private void txtLimiteHoras_3_Leave(object sender, EventArgs e)
        {
            if (txtLimiteHoras_3.Text != "--:--")
            {
                 txtLimiteHorasDiarios_3.Text = "--:--";
            }
        }

        private void  txtLimiteHorasDiarios_3_Leave(object sender, EventArgs e)
        {
            if ( txtLimiteHorasDiarios_3.Text != "--:--")
            {
                txtLimiteHoras_3.Text = "--:--";
            }
        }

        private void txtLimiteHoras_4_Leave(object sender, EventArgs e)
        {
            if (txtLimiteHoras_4.Text != "--:--")
            {
                 txtLimiteHorasDiarios_4.Text = "--:--";
            }
        }

        private void  txtLimiteHorasDiarios_4_Leave(object sender, EventArgs e)
        {
            if ( txtLimiteHorasDiarios_4.Text != "--:--")
            {
                txtLimiteHoras_4.Text = "--:--";
            }
        }

        private void txtLimiteHoras_5_Leave(object sender, EventArgs e)
        {
            if (txtLimiteHoras_5.Text != "--:--")
            {
                 txtLimiteHorasDiarios_5.Text = "--:--";
            }
        }

        private void  txtLimiteHorasDiarios_5_Leave(object sender, EventArgs e)
        {
            if ( txtLimiteHorasDiarios_5.Text != "--:--")
            {
                txtLimiteHoras_5.Text = "--:--";
            }
        }

        private void txtLimiteHoras_6_Leave(object sender, EventArgs e)
        {
            if (txtLimiteHoras_6.Text != "--:--")
            {
                 txtLimiteHorasDiarios_6.Text = "--:--";
            }
        }

        private void  txtLimiteHorasDiarios_6_Leave(object sender, EventArgs e)
        {
            if ( txtLimiteHorasDiarios_6.Text != "--:--")
            {
                txtLimiteHoras_6.Text = "--:--";
            }
        }

        private void txtLimiteHoras_7_Leave(object sender, EventArgs e)
        {
            if (txtLimiteHoras_7.Text != "--:--")
            {
                 txtLimiteHorasDiarios_7.Text = "--:--";
            }
        }

        private void  txtLimiteHorasDiarios_7_Leave(object sender, EventArgs e)
        {
            if ( txtLimiteHorasDiarios_7.Text != "--:--")
            {
                txtLimiteHoras_7.Text = "--:--";
            }
        }

        private void txtLimiteHoras_8_Leave(object sender, EventArgs e)
        {
            if (txtLimiteHoras_8.Text != "--:--")
            {
                 txtLimiteHorasDiarios_8.Text = "--:--";
            }
        }

        private void  txtLimiteHorasDiarios_8_Leave(object sender, EventArgs e)
        {
            if ( txtLimiteHorasDiarios_8.Text != "--:--")
            {
                txtLimiteHoras_8.Text = "--:--";
            }
        }

        private void txtLimiteHoras_9_Leave(object sender, EventArgs e)
        {
            if (txtLimiteHoras_9.Text != "--:--")
            {
                 txtLimiteHorasDiarios_9.Text = "--:--";
            }
        }

        private void  txtLimiteHorasDiarios_9_Leave(object sender, EventArgs e)
        {
            if ( txtLimiteHorasDiarios_9.Text != "--:--")
            {
                txtLimiteHoras_9.Text = "--:--";
            }
        }
        #endregion

    }
}
