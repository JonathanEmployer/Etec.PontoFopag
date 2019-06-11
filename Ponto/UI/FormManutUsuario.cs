using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutUsuario : UI.Base.ManutBase
    {
        private BLL.Cw_Usuario bllCw_Usuario = BLL.Cw_Usuario.GetInstancia;
        private Modelo.Cw_Usuario objCw_Usuario;
        private BLL.Cw_Grupo bllCw_Grupo = BLL.Cw_Grupo.GetInstancia;

        public FormManutUsuario()
        {
            InitializeComponent();
            this.Name = "FormManutUsuario";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objCw_Usuario = new Modelo.Cw_Usuario();
                    objCw_Usuario.Codigo = bllCw_Usuario.MaxCodigo();
                    break;
                default:
                    objCw_Usuario = bllCw_Usuario.LoadObject(cwID);
                    break;
            }
            cbIdGrupo.Properties.DataSource = bllCw_Grupo.GetAll();

            txtCodigo.DataBindings.Add("EditValue", objCw_Usuario, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLogin.DataBindings.Add("EditValue", objCw_Usuario, "Login", true, DataSourceUpdateMode.OnPropertyChanged);
            txtSenha.DataBindings.Add("EditValue", objCw_Usuario, "Senha", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNome.DataBindings.Add("EditValue", objCw_Usuario, "Nome", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdGrupo.DataBindings.Add("EditValue", objCw_Usuario, "IdGrupo", true, DataSourceUpdateMode.OnPropertyChanged);
            rgTipo.DataBindings.Add("EditValue", objCw_Usuario, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);

            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            return bllCw_Usuario.Salvar(cwAcao, objCw_Usuario);
        }

        private void sbIdGrupo_Click(object sender, EventArgs e)
        {
            FormGridGrupo form = new FormGridGrupo();
            form.cwTabela = "Grupo";
            form.cwId = (int)cbIdGrupo.EditValue;
            GridSelecao(form, cbIdGrupo, bllCw_Grupo);
        }
    }
}
