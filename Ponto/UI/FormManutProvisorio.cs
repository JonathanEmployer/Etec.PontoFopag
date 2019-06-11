using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormManutProvisorio : UI.Base.ManutBase
    {
        private BLL.Provisorio bllProvisorio;
        private Modelo.Provisorio objProvisorio;
        private BLL.Funcionario bllFuncionario;
        string CodigoParaVerificacaoBilhetesNovo, CodigoParaVerificacaoBilhetes;
        DateTime datainicial, datafinal;

        public FormManutProvisorio()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            bllProvisorio = new BLL.Provisorio();
            this.Name = "FormManutProvisorio";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    objProvisorio= new Modelo.Provisorio();
                    objProvisorio.Codigo = bllProvisorio.MaxCodigo();
                    objProvisorio.Dt_inicial = null;
                    objProvisorio.Dt_final = null;
                    cbIdDsfuncionario.EditValue = 0;
                    break;
                default:
                    objProvisorio = bllProvisorio.LoadObject(cwID);
                    CodigoParaVerificacaoBilhetesNovo = objProvisorio.Dsfuncionarionovo;
                    datafinal = (DateTime)objProvisorio.Dt_final;
                    datainicial = (DateTime)objProvisorio.Dt_inicial;
                    CodigoParaVerificacaoBilhetes = objProvisorio.Dsfuncionario;
                    break;
            }
            cbIdDsfuncionario.Properties.DataSource = bllFuncionario.GetParaProvisorio();
           

            txtCodigo.DataBindings.Add("EditValue", objProvisorio, "Codigo", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDsfuncionarionovo.DataBindings.Add("EditValue", objProvisorio, "Dsfuncionarionovo", true, DataSourceUpdateMode.OnPropertyChanged);
            cbIdDsfuncionario.DataBindings.Add("EditValue", objProvisorio, "Dsfuncionario", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDt_inicial.DataBindings.Add("EditValue", objProvisorio, "Dt_inicial", true, DataSourceUpdateMode.OnPropertyChanged);
            txtDt_final.DataBindings.Add("EditValue", objProvisorio, "Dt_final", true, DataSourceUpdateMode.OnPropertyChanged);
            base.CarregaObjeto();
        }

        public override Dictionary<string, string> Salvar()
        {
            DateTime? ultimaData = null;
            if (cwAcao == Modelo.Acao.Excluir || cwAcao == Modelo.Acao.Alterar)
            {
                if (objProvisorio.Dsfuncionarionovo == CodigoParaVerificacaoBilhetesNovo && 
                    objProvisorio.Dt_inicial == datainicial && 
                    objProvisorio.Dt_final == datafinal && 
                    objProvisorio.Dsfuncionario == CodigoParaVerificacaoBilhetes)
                {
                    if (cwAcao == Modelo.Acao.Excluir)
                    {
                        if (!bllProvisorio.VerificaBilhete(CodigoParaVerificacaoBilhetesNovo, datainicial, datafinal, out ultimaData))
                        {
                            return bllProvisorio.Salvar(cwAcao, objProvisorio);
                        }
                        else
                        {
                            MessageBox.Show("Código provisório não pode ser deletado.\nJá existem bilhetes importados com este código.");
                            return new Dictionary<string, string>();
                        }
                    }
                    else
                    {
                        base.Salvar();
                        return bllProvisorio.Salvar(cwAcao, objProvisorio);
                    }
                }

                bllProvisorio.VerificaBilhete(CodigoParaVerificacaoBilhetesNovo, datainicial, datafinal, out ultimaData);
                if (ultimaData.HasValue)
                {
                    if (ultimaData > objProvisorio.Dt_final)
                    {
                        Dictionary<string, string> ret = new Dictionary<string, string>();
                        ret.Add("txtDt_final", "Já existem bilhetes importados com este código provisório na data " + ultimaData.Value.ToShortDateString()
                            + ".\nA data final do código provisório deve ser maior ou igual a data do ultimo bilhete importado com este código.");
                        return ret;
                    }
                }
                return bllProvisorio.Salvar(cwAcao, objProvisorio);
            }
            else
            {
                return bllProvisorio.Salvar(cwAcao, objProvisorio);
            }
        }
    }
}
