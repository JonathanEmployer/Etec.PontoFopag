using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI.Util
{
    public partial class FormValidade : UI.Base.ManutBase
    {
        private BLL.Empresa bllEmp;
        public FormValidade()
        {
            InitializeComponent();
            bllEmp = new BLL.Empresa();
        }

        public override Dictionary<string, string> Salvar()
        {
            cwErro = new Dictionary<string, string>();
            if (deDataVencimento.EditValue == null && MessageBox.Show("Deixando a data de validade vazia " + 
                "irá desativar o controle de validade. Deseja continuar?", "Atenção", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
            {
                deDataVencimento.EditValue = DateTime.MaxValue.Date;
            }
            if (deDataVencimento.EditValue != null && deDataVencimento.DateTime.Date < DateTime.Now.Date)
            {
                cwErro.Add(deDataVencimento.Name, "Data em formato incorreto (não pode ser inferior à data atual)");
            }
            else
            {
                Modelo.Empresa emp = bllEmp.GetEmpresaPrincipal();
                if (emp.EmpresaOK())
                {
                    emp.Validade = deDataVencimento.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                    try
                    {
                        emp.Chave = emp.HashMD5ComRelatoriosValidacaoNova();
                        bllEmp.Salvar(Modelo.Acao.Alterar, emp);
                    }
                    catch (Exception e)
                    {
                        cwErro.Add(sbGravar.Name, e.Message);
                        throw e;
                    }
                }
            }

            return cwErro;
        }
    }
}
