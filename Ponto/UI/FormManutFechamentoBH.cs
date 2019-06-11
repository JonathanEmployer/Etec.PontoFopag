using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutFechamentoBH : UI.Base.ManutBase
    {
        private BLL.FechamentoBH bllFechamentoBH;
        private BLL.Marcacao bllMarcacao;
        private Modelo.FechamentoBH objFechamentoBH;

        public FormManutFechamentoBH()
        {
            InitializeComponent();
            bllFechamentoBH = new BLL.FechamentoBH();
            bllMarcacao = new BLL.Marcacao();
            this.Name = "FormManutFechamentoBH";
        }

         public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objFechamentoBH = new Modelo.FechamentoBH();
                    objFechamentoBH.Codigo = bllFechamentoBH.MaxCodigo();
                    objFechamentoBH.Tipo = -1;
                    objFechamentoBH.Data = null;
                    break;
                default:
                    objFechamentoBH = bllFechamentoBH.LoadObject(cwID);

                    objFechamentoBH.Data_Ant = objFechamentoBH.Data;
                    objFechamentoBH.Tipo_Ant = objFechamentoBH.Tipo;
                    objFechamentoBH.Identificacao_Ant = objFechamentoBH.Identificacao;
                    break;
            }

             txtCodigo.DataBindings.Add("EditValue", objFechamentoBH, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
             txtData.DataBindings.Add("DateTime", objFechamentoBH, "Data", true, DataSourceUpdateMode.OnPropertyChanged);
             rgTipo.DataBindings.Add("EditValue", objFechamentoBH, "Tipo", true, DataSourceUpdateMode.OnPropertyChanged);
             chbEfetivado.DataBindings.Add("Checked", objFechamentoBH, "Efetivado", true, DataSourceUpdateMode.OnPropertyChanged);
             switch (objFechamentoBH.Tipo)
             {
                 case 0:
                     rgTipo.SelectedIndex = 0;
                     break;
                 case 1:
                     rgTipo.SelectedIndex = 1;
                     break;
                 case 2:
                     rgTipo.SelectedIndex = 3;
                     break;
                 case 3:
                     rgTipo.SelectedIndex = 2;
                     break;

             }
             
             base.CarregaObjeto();
        }

         public override Dictionary<string, string> Salvar()
         {
             FormProgressBar2 pbExcluir = new FormProgressBar2();
             pbExcluir.Show(this);
             
             bllFechamentoBH.ExcluirFechamento(objFechamentoBH.Id);
             bllMarcacao.RecalculaMarcacao(objFechamentoBH.Tipo, objFechamentoBH.Identificacao, objFechamentoBH.Data.Value, objFechamentoBH.Data.Value, pbExcluir.ObjProgressBar);
             pbExcluir.Close();
             return new Dictionary<string, string>();
         }

         protected override void ChamaHelp()
         {
             Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, "formgridfechamentobh.htm");
         }
    }
}
