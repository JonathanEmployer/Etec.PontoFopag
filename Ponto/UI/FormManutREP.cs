using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutREP : UI.Base.ManutBase
    {
        private BLL.REP bllREP;
        private BLL.Empresa bllEmpresa;
        private BLL.EquipamentoHomologado bllEquipamentoHomologado;
        private Modelo.REP objREP;

        public FormManutREP()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllREP = new BLL.REP();
            bllEquipamentoHomologado = new BLL.EquipamentoHomologado();
            this.Name = "FormManutREP";

            //cbRelogio.Properties.DataSource = bllREP.GetRelogios();
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objREP = new Modelo.REP();
                    objREP.Codigo = bllREP.MaxCodigo();
                    objREP.TipoComunicacao = -1;
                    break;
                default:
                    objREP = bllREP.LoadObject(cwID);
                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objREP, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNumSerie.DataBindings.Add("EditValue", objREP, "NumSerie", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNumRelogio.DataBindings.Add("EditValue", objREP, "NumRelogio", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLocal.DataBindings.Add("EditValue", objREP, "Local", true, DataSourceUpdateMode.OnPropertyChanged);

            /*cbRelogio.DataBindings.Add("EditValue", objREP, "Relogio", true, DataSourceUpdateMode.OnPropertyChanged);*/
            txtRelogio.DataBindings.Add("EditValue", objREP, "modeloNome", true, DataSourceUpdateMode.OnPropertyChanged);

            txtSenha.DataBindings.Add("EditValue", objREP, "Senha", true, DataSourceUpdateMode.OnPropertyChanged);
            txtPorta.DataBindings.Add("EditValue", objREP, "Porta", true, DataSourceUpdateMode.OnPropertyChanged);
            txtIP.DataBindings.Add("EditValue", objREP, "IP", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDigitos.DataBindings.Add("EditValue", objREP, "QtdDigitos", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdEmpresa.DataBindings.Add("EditValue", objREP, "IdEmpresa", true, DataSourceUpdateMode.OnPropertyChanged);
            chbBiometrico.Checked = objREP.Biometrico;
            cbTipoComunicacao.SelectedIndex = objREP.TipoComunicacao;
            

            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            EquipamentoHomologado eq = bllEquipamentoHomologado.LoadObject(objREP.IdEquipamentoHomologado);
            if (eq != null)
            {
                if (eq.EquipamentoHomologadoInmetro)
                {
                    MessageBox.Show("ATENÇÃO! Este equipamento é homologado pelo INMETRO, e possui função de criptografia.\r\n \r\n"
                        + "Para realizar qualquer comunicação com este equipamento, bem como a coleta de bilhetes, consulte o manual do "
                        + "fabricante e/ou contate a revenda do REP para realizar o cadastro do usuário para comunicação no equipamento.\r\n \r\n"
                        + "Altere o Cadastro de Usuário do Cwork Ponto Plus (Menú \"Segurança\" na opção \"Usuários\") e inclua também "
                        + "o nome de usuário, senha e CPF para acesso à este REP (Campos \"Login REP\", \"Senha REP\" e \"CPF\")."
                        , "Atenção - Equipamento Homologado INMETRO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            
            objREP.TipoComunicacao = Convert.ToInt16(cbTipoComunicacao.SelectedIndex);
            objREP.Biometrico = chbBiometrico.Checked;
            base.Salvar();
            return bllREP.Salvar(cwAcao, objREP);
        }

        private void cbTipoComunicacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTipoComunicacao.SelectedIndex == 1)
            {
                txtIP.EditValue = String.Empty;
                txtIP.Enabled = false;
            }
            else
                txtIP.Enabled = true;
        }

        private void sbIdEmpresa_Click(object sender, EventArgs e)
        {
            FormGridEmpresa form = new FormGridEmpresa();
            form.cwTabela = "Empresa";
            form.cwId = (int)cbIdEmpresa.EditValue;
            GridSelecao(form, cbIdEmpresa, bllEmpresa);
        }

        private void txtNumSerie_Leave(object sender, EventArgs e)
        {
            objREP.EquipamentoHomologado = bllEquipamentoHomologado.LoadByCodigoModelo(txtNumSerie.Text);
        }
    }
}
