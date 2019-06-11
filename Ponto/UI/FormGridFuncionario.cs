using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace UI
{
    public partial class FormGridFuncionario : UI.Base.GridBase
    {
        private BLL.Funcionario bllFuncionario;
        private BLL.ConfiguracoesGerais bllConfiguracoes;
        private DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1;
        private BLL.Empresa bllEmpresa;

        public FormGridFuncionario()
        {
            InitializeComponent();
            bllFuncionario = new BLL.Funcionario();
            bllConfiguracoes = new BLL.ConfiguracoesGerais();
            bllEmpresa = new BLL.Empresa();
            this.Name = "FormGridFuncionario";
            styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();

            try
            {
                XmlDocument xml = new XmlDocument();
                FileInfo file = new FileInfo(Application.StartupPath + "\\XML\\" + "EstadoGridFuncionario.xml");
                if (file.Exists)
                {
                    xml.Load(Application.StartupPath + "\\XML\\" + "EstadoGridFuncionario.xml");
                    XmlNode noPai = xml.SelectSingleNode("Funcionario");
                    ckEsconderInativos.Checked = Convert.ToBoolean(noPai.SelectSingleNode("Filtro").InnerText);
                }               
            }
            catch (FormatException)
            {
                MessageBox.Show("Os dados do arquivo de configuração estão incorretos. Verifique.");
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("Diretório não encontrado.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            
        }

        protected override void CarregaGrid(string pFiltro)
        {
            base.CarregaGrid(pFiltro);
            gridControl1.DataSource = bllFuncionario.GetAll();            
            OrdenaGrid("nome", DevExpress.Data.ColumnSortOrder.Ascending);
        }

        protected override void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            FormManutFuncionario form = new FormManutFuncionario();
            form.cwAcao = pAcao;
            form.cwID = pID;
            form.cwTabela = "Funcionário";
            form.ShowDialog();
        }

        protected override void PersonalizaGrid()
        {
            dataGridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            dataGridView1.Columns["id"].Visible = false;
            dataGridView1.Columns["funcionarioativo"].Visible = false;
            dataGridView1.Columns["nome"].Caption = "Nome";
            dataGridView1.Columns["nome"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["nome"].Width = 210;
            dataGridView1.Columns["dscodigo"].Caption = "Código";
            dataGridView1.Columns["dscodigo"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["dscodigo"].Width = 110;
            dataGridView1.Columns["dscodigo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dataGridView1.Columns["matricula"].Caption = "Matrícula";
            dataGridView1.Columns["matricula"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["matricula"].Width = 80;
            dataGridView1.Columns["matricula"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dataGridView1.Columns["jornada"].Caption = "Jornada";
            dataGridView1.Columns["jornada"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["jornada"].Width = 135;
            dataGridView1.Columns["empresa"].Caption = "Empresa";
            dataGridView1.Columns["empresa"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            dataGridView1.Columns["empresa"].Width = 180;
            dataGridView1.Columns["departamento"].Caption = "Departamento";
            dataGridView1.Columns["departamento"].Width = 130;
            dataGridView1.Columns["funcao"].Caption = "Função";
            dataGridView1.Columns["funcao"].Width = 130;
            dataGridView1.Columns["carteira"].Caption = "Carteira";
            dataGridView1.Columns["carteira"].Width = 130;
            dataGridView1.Columns["carteira"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            dataGridView1.Columns["dataadmissao"].Caption = "Data Admissão";
            dataGridView1.Columns["dataadmissao"].Width = 130;
            dataGridView1.Columns["dataadmissao"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;

            styleFormatCondition1.Appearance.ForeColor = System.Drawing.Color.Red;
            styleFormatCondition1.Appearance.Options.UseForeColor = true;
            styleFormatCondition1.ApplyToRow = true;
            styleFormatCondition1.Column = dataGridView1.Columns["funcionarioativo"];
            styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;            
            styleFormatCondition1.Value1 = 0;
            this.dataGridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
        }       

        private void sbImprimirCartaoPonto_Click(object sender, EventArgs e)
        {
            int idfuncionario = RegistroSelecionado();
            if (idfuncionario > 0)
            {
                Modelo.Funcionario objFuncionario;
                objFuncionario = bllFuncionario.LoadObject(idfuncionario);
                DateTime datai, dataf;
                bool mudadataautomaticamente;
                bllConfiguracoes.AtribuiDatas(Application.StartupPath, out datai, out dataf, out mudadataautomaticamente);
                UI.Relatorios.CartaoPonto.FormImprimeCartaoIndividual form = new UI.Relatorios.CartaoPonto.FormImprimeCartaoIndividual(datai, dataf, objFuncionario);
                form.ShowDialog();
            }
        }

        private void sbTurnoFuncionario_Click(object sender, EventArgs e)
        {
            int idfuncionario = RegistroSelecionado();
            if (idfuncionario > 0)
            {
                Modelo.Funcionario objFuncionario;
                objFuncionario = bllFuncionario.LoadObject(idfuncionario);
                if (objFuncionario.Tipohorario == 1)
                {
                    FormManutHorario form = new FormManutHorario();
                    form.cwAcao = Modelo.Acao.Consultar;
                    form.cwID = objFuncionario.Idhorario;
                    form.cwTabela = "Horário Normal";
                    form.ShowDialog();
                }
                else if (objFuncionario.Tipohorario == 2)
                {
                    FormManutHorarioMovel form = new FormManutHorarioMovel();
                    form.cwAcao = Modelo.Acao.Consultar;
                    form.cwID = objFuncionario.Idhorario;
                    form.cwTabela = "Horário Flexível";
                    form.ShowDialog();
                }
            }
        }

        public override void dataGridView1_CustomRowFilter(object sender, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e)
        {
            bool filtro = ckEsconderInativos.Checked;
            base.dataGridView1_CustomRowFilter(sender, e);

            if (filtro)
            {
                if (!dataGridView1.GetRow(e.ListSourceRow).Equals(null) && (System.Convert.ToInt16(dataGridView1.GetRowCellValue(e.ListSourceRow, "funcionarioativo")) == 0))
                {
                    e.Visible = false;
                }
            }            
            e.Handled = filtro;
        }

        private void ckEsconderInativos_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.RefreshData();
        }
        
        public void SalvarEstado()
        {
            try
            {
                XmlDocument documentoXml = new XmlDocument();

                documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "UTF-8", null));

                XmlElement noPai = documentoXml.CreateElement("Funcionario");
                XmlElement xmlFiltro = documentoXml.CreateElement("Filtro");
                xmlFiltro.InnerText = ckEsconderInativos.Checked.ToString();
                noPai.AppendChild(xmlFiltro);
                documentoXml.AppendChild(noPai);

                documentoXml.Save(Application.StartupPath + "\\XML\\" + "EstadoGridFuncionario.xml");
            }
            catch (Exception)
            {
                MessageBox.Show("Ocorreu um erro ao salvar o arquivo de configuração. Verifique as permissões da pasta \n " + Application.StartupPath);
            }
        }

        private void FormGridFuncionario_FormClosed(object sender, FormClosedEventArgs e)
        {
            SalvarEstado();
        }

        protected override void sbIncluir_Click(object sender, EventArgs e)
        {
            //int limitefunc = 0;
            //if (bllEmpresa.ValidaLicenca(out limitefunc, false))
                CarregarManutencao(Modelo.Acao.Incluir, 0);
            //else
            //    MessageBox.Show("A quantidade de funcionários chegou no limite de " + limitefunc + " funcionários ativos. Entre em contato com a revenda.");          
        }

        private void sbHistoricoMudancaHorario_Click(object sender, EventArgs e)
        {
            int id = RegistroSelecionado();
            if (id > 0)
            {
                FormGridMudancaHorario form = new FormGridMudancaHorario(id);
                form.cwId = 0;
                form.cwTabela = "Mudança de Horário";
                form.ShowDialog();

                CarregaGrid("");
            }
            else
            {
                MessageBox.Show("Nenhum funcionário selecionado.");
            }
        }
    }
}
