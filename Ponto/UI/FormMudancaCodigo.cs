using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class FormMudancaCodigo : UI.Base.ManutBase
    {
        private BLL.Funcionario bllFuncionario;
        private BLL.MudCodigoFunc bllMudCodigoFunc;

        public FormMudancaCodigo()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            bllMudCodigoFunc = new BLL.MudCodigoFunc();
            this.Name = "FormMudancaCodigo";
        }

        public override void CarregaObjeto()
        {
            switch (cwAcao)
            {
                case Modelo.Acao.Incluir:
                    cbIdFuncionario.Properties.DataSource = bllFuncionario.GetAll();
                    break;
                default:
                    break;
            }

        }

        public override Dictionary<string, string> Salvar()
        {
            DateTime data = DateTime.Now.Date;
            Dictionary<string, string> ret = new Dictionary<string, string>();
            Int64 codigo;

            BLL.Marcacao bllMarcacao = new BLL.Marcacao();
            BLL.InclusaoBanco bllInclusao = new BLL.InclusaoBanco();
            BLL.Provisorio bllProvisorio = new BLL.Provisorio();

            //Verifica se existe algum bilhete importado depois da data da mudança
            try
            {                
                codigo = Convert.ToInt64(txtCodigo.EditValue);
            }
            catch (Exception)
            {
                ret.Add("txtCodigo", "O campo código só pode conter números!");
                return ret;
            }
            if (bllMudCodigoFunc.VerificaMarcacao((int)cbIdFuncionario.EditValue, data))
            {
                if ((int)cbIdFuncionario.EditValue != 0 && (txtCodigo.EditValue != null) && (codigo > 0))
                {
                    
                    //Verifica se existe algum provisório no período
                    if (!bllProvisorio.ExisteProvisorio(txtCodigo.EditValue.ToString(), data))
                    {
                        int idfunc = (int)cbIdFuncionario.EditValue;
                        
                        DateTime? ultimaData = bllMarcacao.GetUltimaDataFuncionario(idfunc);
                        if (ultimaData == null || ultimaData == new DateTime())
                        {                           
                                //Cria as marcações que não existem desde a data de admissao do funcionario até a data da mudanca
                                
                                Modelo.Funcionario objFuncionario = bllFuncionario.LoadObject(idfunc);
                                List<Modelo.InclusaoBanco> inclusaoBancoList = bllInclusao.GetAllList();
                                bllMarcacao.AtualizaData(objFuncionario.Dataadmissao.Value, data.AddDays(-1), objFuncionario);                            
                        }
                        else
                        {
                            if (ultimaData < data)
                            {
                                //Cria as marcações que não existem no período anterior à mudança de código
                                Modelo.Funcionario objFuncionario = bllFuncionario.LoadObject(idfunc);
                                List<Modelo.InclusaoBanco> inclusaoBancoList = bllInclusao.GetAllList();
                                bllMarcacao.AtualizaData(ultimaData.Value, data.AddDays(-1), objFuncionario);
                            }
                        }
                        //Realiza a mudança de código
                        if (bllFuncionario.MudaCodigoFuncionario(idfunc, txtCodigo.EditValue.ToString(), data))
                        {
                            MessageBox.Show("Alteração do código executada com sucesso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        ret.Add("txtCodigo", "Existe um código provisório cadastrado no mesmo período da mudança de código. Escolha outro código.");
                    }
                }
                else
                {
                    MessageBox.Show("Campos em branco. Verifique!");
                    ret.Add("", "");
                }
            }
            else
            {
                MessageBox.Show(this, "Mudança de código não permitida \nJá existem registros gravados superiores a data da mudança.");
                ret.Add("", "");
            }
            return ret;
        }

        private void sbIdIdentificacao_Click(object sender, EventArgs e)
        {
            FormGridFuncionario form = new FormGridFuncionario();
            form.cwTabela = "Funcionário";
            form.cwId = (int)cbIdFuncionario.EditValue;
            GridSelecao(form, cbIdFuncionario, bllFuncionario);
        }
    }
}
