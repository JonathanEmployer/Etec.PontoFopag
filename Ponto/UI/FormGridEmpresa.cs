using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using REL;
using UI.Util;

namespace UI
{
    public partial class FormGridEmpresa : UI.Base.GridBase
    {
        private BLL.Empresa bllEmpresa;

        public FormGridEmpresa()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            this.Name = "FormGridEmpresa";
            if (bllEmpresa.GetEmpresaPrincipal().Bloqueiousuarios)
            {
                sbUsuarios.Visible = true;
            }
        }
        
        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllEmpresa.GetAll();
            OrdenaGrid("nome", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutEmpresa form = new FormManutEmpresa();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Empresa";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["nome"].Width = 300;
            dataGridView1.Columns["codigo"].Caption = "Código";
            dataGridView1.Columns["codigo"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["codigo"].Width = 80;
            dataGridView1.Columns["cnpj_cpf"].Caption = "CNPJ/CPF";
            dataGridView1.Columns["cnpj_cpf"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["cnpj_cpf"].Width = 115;
            dataGridView1.Columns["endereco"].Caption = "Endereço";
            dataGridView1.Columns["endereco"].Width = 200;
            dataGridView1.Columns["endereco"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["cidade"].Caption = "Cidade";
            dataGridView1.Columns["cidade"].Width = 150;
            dataGridView1.Columns["cep"].Caption = "CEP";
            dataGridView1.Columns["cep"].Width = 150;
            dataGridView1.Columns["cei"].Caption = "CEI";
            dataGridView1.Columns["cei"].Width = 150;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int id = RegistroSelecionado();
            Modelo.Empresa objEmpresa;
            objEmpresa = bllEmpresa.LoadObject(id);

            string parametro = CriptoString.Encrypt(objEmpresa.Cnpj + "|" + objEmpresa.Nome);
            parametro = Uri.EscapeDataString(parametro);
            System.Diagnostics.Process.Start("http://177.72.160.122:8083/AtestadoTecnico/GerarAtestado?parametro=" + parametro);
            //DataTable Dt;
            //List<Microsoft.Reporting.WinForms.ReportParameter> parametros;
            //string nomerel;
            //string ds;
            //Dt = bllEmpresa.GetEmpresaAtestado(RegistroSelecionado());
            //parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            //nomerel = "rptAtestado.rdlc";
            //ds = "dsEmpresa_Empresa";
            //UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
            //form.Text = "Atestado Técnico";
            //form.Show();
            //this.Close();
        }

        private void sbUsuarios_Click(object sender, EventArgs e)
        {
            int id = RegistroSelecionado();
            if (id > 0)
            {
                var form = new FormManutEmpresaCw_Usuario(id);                 
                Util.Funcoes.ChamaManut(null, form, "Empresa / Usuários", Modelo.Acao.Alterar, TelasAbertas);
            }
            else
            {
                MessageBox.Show("Nenhuma empresa selecionada.");
            }
        }
    }
}
