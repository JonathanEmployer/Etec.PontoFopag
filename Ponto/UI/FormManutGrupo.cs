using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutGrupo : UI.Base.ManutBase
    {
        private BLL.Cw_Grupo bllCw_Grupo = BLL.Cw_Grupo.GetInstancia;
        private Modelo.Cw_Grupo objCw_Grupo;

        public FormManutGrupo()
        {
            InitializeComponent();
            this.Name = "FormManutGrupo";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objCw_Grupo= new Modelo.Cw_Grupo();
                    objCw_Grupo.Codigo = bllCw_Grupo.MaxCodigo();
                    break;
                default:
                    objCw_Grupo= bllCw_Grupo.LoadObject(cwID);
                    break;
            }

            txtCodigo.DataBindings.Add("EditValue", objCw_Grupo, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtNome.DataBindings.Add("EditValue", objCw_Grupo, "Nome", true, DataSourceUpdateMode.OnPropertyChanged);
            rgAcesso.DataBindings.Add("EditValue", objCw_Grupo, "Acesso", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            return bllCw_Grupo.Salvar(cwAcao, objCw_Grupo);
        }


    }
}
