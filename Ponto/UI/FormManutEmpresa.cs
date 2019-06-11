using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutEmpresa : UI.Base.ManutBase
    {
        private BLL.Empresa bllEmpresa;
        private Modelo.Empresa objEmpresa;

        public FormManutEmpresa()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            this.Name = "FormManutEmpresa";            
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    txtCodigo.Enabled = true;
                    txtNome.Enabled = true;
                    txtEndereco.Enabled = true;
                    txtEstado.Enabled = true;
                    txtCnpj.Enabled = true;
                    txtCpf.Enabled = true;
                    txtCidade.Enabled = true;
                    txtCep.Enabled = true;
                    objEmpresa= new Modelo.Empresa();
                    objEmpresa.Codigo = bllEmpresa.MaxCodigo();

                    break;
                case Modelo.Acao.Consultar:
                    txtCodigo.Properties.ReadOnly = true;
                    txtNome.Properties.ReadOnly = true;
                    txtEndereco.Properties.ReadOnly = true;
                    txtEstado.Properties.ReadOnly = true;
                    txtCnpj.Properties.ReadOnly = true;
                    txtCpf.Properties.ReadOnly = true;
                    txtCidade.Properties.ReadOnly = true;
                    txtCep.Properties.ReadOnly = true;
                    txtNumeroserie.Properties.ReadOnly = true;
                    txtCodigo.Enabled = true;
                    txtNome.Enabled = true;
                    txtEndereco.Enabled = true;
                    txtEstado.Enabled = true;
                    txtCnpj.Enabled = true;
                    txtCpf.Enabled = true;
                    txtCidade.Enabled = true;
                    txtCep.Enabled = true;
                    txtNumeroserie.Enabled = true;
                    objEmpresa= bllEmpresa.LoadObject(cwID);
                    break;
                default:
                    objEmpresa = bllEmpresa.LoadObject(cwID);
                    
                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objEmpresa, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNome.DataBindings.Add("EditValue", objEmpresa, "Nome", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEndereco.DataBindings.Add("EditValue", objEmpresa, "Endereco", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCnpj.DataBindings.Add("EditValue", objEmpresa, "Cnpj", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCpf.DataBindings.Add("EditValue", objEmpresa, "Cpf", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCidade.DataBindings.Add("EditValue", objEmpresa, "Cidade", true, DataSourceUpdateMode.OnPropertyChanged);
            txtEstado.DataBindings.Add("EditValue", objEmpresa, "Estado", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCep.DataBindings.Add("EditValue", objEmpresa, "Cep", true, DataSourceUpdateMode.OnPropertyChanged);
            txtCEI.DataBindings.Add("EditValue", objEmpresa, "CEI", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNumeroserie.DataBindings.Add("EditValue", objEmpresa, "Numeroserie", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            return bllEmpresa.Salvar(cwAcao, objEmpresa);
        }

        protected override void ChamaHelp()
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, "FormGridEmpresa.htm");
        }
    }
}
