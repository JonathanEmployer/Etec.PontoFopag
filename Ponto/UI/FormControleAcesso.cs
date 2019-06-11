using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormControleAcesso : UI.Base.ManutBase
    {
        private Modelo.Cw_Acesso objAcesso = new Modelo.Cw_Acesso();
        private BLL.Cw_Acesso bllAcesso = BLL.Cw_Acesso.GetInstancia;

        public string cwFormulario { get; set; }

        public FormControleAcesso()
        {
            InitializeComponent();
            this.Name = "FormControleAcesso";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objAcesso = new Modelo.Cw_Acesso();
                    objAcesso.Codigo = bllAcesso.MaxCodigo();
                    break;
                default:
                    objAcesso = bllAcesso.LoadObject(Modelo.Global.objUsuarioLogado.IdGrupo, cwFormulario);
                    break;
            }
            gridControl1.DataSource = objAcesso.Campos;

            txtAcesso.DataBindings.Add("EditValue", objAcesso, "Acesso", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            base.Salvar();
            return bllAcesso.Salvar(cwAcao, objAcesso);
        }

        private void dataGridView1_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            foreach (Modelo.Cw_AcessoCampo c in objAcesso.Campos)
            {
                if (c.Id == (int)dataGridView1.GetFocusedRowCellValue("Id"))
                {
                    c.Acao = Modelo.Acao.Alterar;
                    break;
                }
            }
        }
    }
}
