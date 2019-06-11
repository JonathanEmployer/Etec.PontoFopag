using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace UI.Relatorios.CartaoPonto
{
    public partial class ManutencaoDiariaold : Form
    {
        private BLL.Parametros bllParametros;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.CartaoPonto bllCartaoPonto;
        private DataTable Departamentos;
        private StringBuilder Arquivo { get; set; }
        protected string Titulo { get; set; }

        private List<string> _telasAbertas;
        public List<string> TelasAbertas
        {
            get { return _telasAbertas; }
            set { _telasAbertas = value; }
        }

        public ManutencaoDiariaold()
        {
            InitializeComponent();
            this.Name = "ManutencaoDiaria";
            bllParametros = new BLL.Parametros();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllCartaoPonto = new BLL.CartaoPonto();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nome"></param>
        protected void setaNomeArquivo(string nome)
        {
            Arquivo = new StringBuilder(Application.StartupPath + "\\XML\\");
            Arquivo.Append(nome);
            Arquivo.Append(".xml");
        }

        private void sbAjuda_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }

        private void LoadDepartamentos()
        {
            if ((int)cbIdEmpresa.EditValue > 0)
            {
                Departamentos = bllDepartamento.GetPorEmpresa((int)cbIdEmpresa.EditValue);
            }
            else if ((int)cbIdEmpresa.EditValue == 0)
            {
                Departamentos = bllDepartamento.GetAllComOpcaoTodos();
            }

            Departamentos.TableName = "departamento";

            cbIdDepartamento.Properties.DataSource = Departamentos;

            cbIdDepartamento.DataBindings.Clear();
            cbIdDepartamento.DataBindings.Add("EditValue", Departamentos, "id", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void sbCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool CamposValidos()
        {
            bool ret = true;

            if (txtData.DateTime == new DateTime() || txtData.DateTime == null)
            {
                dxErrorProvider1.SetError(txtData, "Campo obrigatório.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(txtData, "");
            }
            return ret;
        }

        private void sbGravar_Click(object sender, EventArgs e)
        {
            sbGravar.Enabled = false;
            sbCancelar.Enabled = false;
            try
            {
                if (CamposValidos())
                {
                    string nomerel;
                    string ds;
                    List<Microsoft.Reporting.WinForms.ReportParameter> parametros;
                    DataTable Dt;
                    FormProgressBar pb = new FormProgressBar();
                    //Colocado para exibir no relatório apenas os funcionários que estão aparecendo na tela
                    string empresas = "", departamentos = "";
                    int tipo = -1;
                    if ((int)cbIdEmpresa.EditValue != 0)
                    {
                        empresas = "(" + Convert.ToString(cbIdEmpresa.EditValue) + ")";
                        tipo = 0;
                    }
                    if ((int)cbIdDepartamento.EditValue != 0)
                    {
                        departamentos = "(" + Convert.ToString(cbIdDepartamento.EditValue) + ")";
                        tipo = 1;
                    }

                    Dt = bllCartaoPonto.GetCartaoPontoDiaria(txtData.DateTime, txtData.DateTime, empresas, departamentos, tipo, pb.progressBar);
                    pb.Dispose();
                    nomerel = "rptManutencaoDiaria.rdlc";
                    ds = "dsCartaoPonto_DataTable1";

                    Modelo.Parametros objParametro = bllParametros.LoadPrimeiro();

                    parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                    Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("data", txtData.DateTime.ToShortDateString());
                    parametros.Add(p1);
                    UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                    form.Text = "Relatório de Manutenção Diária";
                    form.Show();
                    this.Close();

                    Titulo = cbIdEmpresa.Text;
                    GravaXML(chbSalvarFiltro.Checked);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sbGravar.Enabled = true;
                sbCancelar.Enabled = true;
            }
        }

        private void ManutencaoDiaria_Load(object sender, EventArgs e)
        {
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAllComOpcaoTodos();
            cbIdDepartamento.Properties.DataSource = bllDepartamento.GetAllComOpcaoTodos();
            cbIdDepartamento.Enabled = false;
            sbIdDepartamento.Enabled = false;
            LoadDepartamentos();
            setaNomeArquivo(this.Name);
            LeXML();
        }

        private void ManutencaoDiaria_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }

        protected virtual void GridSelecao<T>(UI.Base.GridBase pGrid, Componentes.devexpress.cwk_DevLookup pCb, BLL.IBLL<T> bll)
        {

            UI.Util.Funcoes.ChamaGridSelecao(pGrid);
            if (pGrid.cwAtualiza == true)
            {
                pCb.Properties.DataSource = bll.GetAll();
            }
            if (pGrid.cwRetorno != 0)
            {
                pCb.EditValue = pGrid.cwRetorno;
            }
            pCb.Focus();
        }

        private void sbIdEmpresa_Click(object sender, EventArgs e)
        {
            FormGridEmpresa form = new FormGridEmpresa();
            form.cwTabela = "Empresa";
            form.cwId = (int)cbIdEmpresa.EditValue;
            GridSelecao(form, cbIdEmpresa, bllEmpresa);
        }

        private void sbIdDepartamento_Click(object sender, EventArgs e)
        {
            FormGridDepartamento form = new FormGridDepartamento();
            form.cwTabela = "Departamento";
            form.cwId = (int)cbIdDepartamento.EditValue;
            GridSelecao(form, cbIdDepartamento, bllDepartamento);
        }

        private void cbIdEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(cbIdEmpresa.EditValue);
            if (id > 0)
            {
                cbIdDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresaComOpcaoTodos((int)cbIdEmpresa.EditValue);
                cbIdDepartamento.EditValue = 0;
                cbIdDepartamento.Enabled = true;
                sbIdDepartamento.Enabled = true;
            }
            else
            {
                cbIdDepartamento.Properties.DataSource = bllDepartamento.GetAllComOpcaoTodos();
                cbIdDepartamento.EditValue = 0;
                cbIdDepartamento.Enabled = false;
                cbIdDepartamento.Enabled = false;
            }
        }

        private void ManutencaoDiaria_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F9:
                        sbGravar.Focus();
                        sbGravar_Click(sender, e);
                    break;
                case Keys.Enter:
                        sbGravar.Focus();
                        sbGravar_Click(sender, e);
                    break;
                case Keys.Escape:
                    if (MessageBox.Show("Tem certeza de que deseja fechar esta janela sem salvar as alterações?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        sbCancelar.Focus();
                        sbCancelar_Click(sender, e);
                    }
                    break;
                case Keys.F1:
                    sbAjuda_Click(sender, e);
                    break;
            }
        }

        protected void GravaXML(bool gravar)
        {
            XmlDocument documentoXml = new XmlDocument();

            documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlElement noPai = documentoXml.CreateElement(this.Name);

            XmlElement element = null;
            XmlElement ler = documentoXml.CreateElement("ler");
            ler.InnerText = gravar.ToString();
            noPai.AppendChild(ler);
            if (gravar)
            {
                XmlElement xmlTitulo = documentoXml.CreateElement("titulo");
                xmlTitulo.InnerText = Titulo;
                noPai.AppendChild(xmlTitulo);

                element = documentoXml.CreateElement("filtros");

                XmlElement empresa = documentoXml.CreateElement(cbIdEmpresa.Name);
                empresa.InnerText = cbIdEmpresa.EditValue.ToString();
                element.AppendChild(empresa);

                XmlElement departamento = documentoXml.CreateElement(cbIdDepartamento.Name);
                departamento.InnerText = cbIdDepartamento.EditValue.ToString();
                element.AppendChild(departamento);

                XmlElement data = documentoXml.CreateElement(txtData.Name);
                data.InnerText = txtData.DateTime.ToString();
                element.AppendChild(data);

                noPai.AppendChild(element);
            }
            documentoXml.AppendChild(noPai);

            documentoXml.Save(Arquivo.ToString());
        }

        protected void LeXML()
        {
            try
            {
                XmlDocument xml = new XmlDocument();

                xml.Load(Arquivo.ToString());
                XmlNode noPai = xml.SelectSingleNode(this.Name);

                if (noPai.SelectSingleNode("ler").InnerText == "True")
                {
                    chbSalvarFiltro.Checked = true;

                    XmlNode filtros = noPai.SelectSingleNode("filtros");

                    foreach (Control campo in tabPage1.Controls)
                    {
                        XmlNode xmlCampo = filtros.SelectSingleNode(campo.Name);
                        if (campo is DevExpress.XtraEditors.PopupBaseEdit)
                        {
                            switch (campo.GetType().ToString())
                            {
                                case "Componentes.devexpress.cwk_DevLookup":
                                    ((DevExpress.XtraEditors.PopupBaseEdit)campo).EditValue = Convert.ToInt32(xmlCampo.InnerText);
                                    break;
                                case "DevExpress.XtraEditors.DateEdit":
                                    ((DevExpress.XtraEditors.DateEdit)campo).EditValue = Convert.ToDateTime(xmlCampo.InnerText);
                                    break;
                                default:
                                    ((DevExpress.XtraEditors.PopupBaseEdit)campo).EditValue = xmlCampo.InnerText;
                                    break;
                            }
                        }
                        else if (campo is DevExpress.XtraEditors.CheckEdit)
                        {
                            ((DevExpress.XtraEditors.CheckEdit)campo).Checked = Convert.ToBoolean(xmlCampo.Value);

                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {

            }
        }
    }
}
