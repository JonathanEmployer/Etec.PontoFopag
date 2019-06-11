using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Modelo;

namespace UI.Relatorios.Base
{
    public partial class FormBase : Form
    {
        protected string nomerel;
        protected string ds;
        protected List<Microsoft.Reporting.WinForms.ReportParameter> parametros;
        protected DataTable Dt;

        protected StringBuilder Arquivo { get; set; }
        protected string Titulo { get; set; }
        protected List<Modelo.Empresa> listaEmpresas = new List<Empresa>();
        protected List<int> listaRowHandleEmpresa = new List<int>();

        protected BLL.Empresa bllEmpresa;
        protected Modelo.Empresa objEmpresa = new Modelo.Empresa();

        public List<string> TelasAbertas { get; set; }

        public FormBase()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();

        }

        protected virtual void FormBase_Load(object sender, EventArgs e)
        {
            //Carrega();
        }

        protected virtual void Carrega()
        {            
            gcEmpresas.DataSource = bllEmpresa.GetAll();

            this.gvEmpresas.OptionsSelection.MultiSelect = true;

            if (gvEmpresas.RowCount > 0)
            {
                gvEmpresas.SelectRow(0);
            }
   
            setaNomeArquivo(this.Name);
            LeXML();
        }

        protected virtual void btOk_Click(object sender, EventArgs e)
        {
            ValidaSelectManutencao();
            
            if (listaEmpresas.Count > 0)
            {
                Titulo = listaEmpresas[0].Nome;
            }
            else
            {
                Titulo = "Cwork Sistemas";
            }

            GravaXML(chbSalvarFiltro.Checked);
        }

        protected virtual void btCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gravar"></param>
        protected virtual void GravaXML(bool gravar)
        {          
                XmlDocument documentoXml = new XmlDocument();

                documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "UTF-8", null));

                XmlElement noPai = documentoXml.CreateElement(this.Name);

                XmlElement element = null;
                XmlElement empresas = null;
                XmlElement ler = documentoXml.CreateElement("ler");
                ler.InnerText = gravar.ToString();
                noPai.AppendChild(ler);
                if (gravar)
                {
                    XmlElement xmlTitulo = documentoXml.CreateElement("titulo");
                    xmlTitulo.InnerText = Titulo;
                    noPai.AppendChild(xmlTitulo);

                    element = documentoXml.CreateElement("filtros");

                    if (listaRowHandleEmpresa.Count > 0)
                    {
                        empresas = documentoXml.CreateElement("empresas");

                        foreach (int f in listaRowHandleEmpresa)
                        {
                            XmlElement empresa = documentoXml.CreateElement("empresa");
                            empresa.InnerText = f.ToString();
                            empresas.AppendChild(empresa);
                        }

                        element.AppendChild(empresas);
                    }

                    XmlElement c;
                    foreach (Control campo in tabPage1.Controls)
                    {
                        if (campo is DevExpress.XtraEditors.CheckEdit)
                        {
                            c = documentoXml.CreateElement(campo.Name);
                            c.InnerText = ((DevExpress.XtraEditors.CheckEdit)campo).Checked.ToString();
                            element.AppendChild(c);
                        }
                        else if (campo is DevExpress.XtraEditors.DateEdit)
                        {
                            c = documentoXml.CreateElement(campo.Name);
                            if (((DevExpress.XtraEditors.DateEdit)campo).DateTime != null)
                            {
                                c.InnerText = ((DevExpress.XtraEditors.DateEdit)campo).DateTime.ToString();
                            }
                            else
                            {
                                c.InnerText = "";
                            }
                            element.AppendChild(c);
                        }
                        else if (campo is DevExpress.XtraEditors.BaseEdit)
                        {
                            c = documentoXml.CreateElement(campo.Name);
                            if (((DevExpress.XtraEditors.BaseEdit)campo).EditValue != null)
                            {
                                c.InnerText = ((DevExpress.XtraEditors.BaseEdit)campo).EditValue.ToString();
                            }
                            else
                            {
                                c.InnerText = "";
                            }
                            element.AppendChild(c);
                        }
                    }

                    noPai.AppendChild(element);
                }
                documentoXml.AppendChild(noPai);

                documentoXml.Save(Arquivo.ToString());            
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void LeXML()
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
                                    if(xmlCampo != null)
                                        if (!String.IsNullOrEmpty(xmlCampo.InnerText))
                                            ((DevExpress.XtraEditors.DateEdit)campo).DateTime = Convert.ToDateTime(xmlCampo.InnerText);
                                    break;
                                default:
                                    ((DevExpress.XtraEditors.PopupBaseEdit)campo).EditValue = xmlCampo.InnerText;
                                    break;
                            }
                        }
                        else if (campo is DevExpress.XtraEditors.CheckEdit)
                        {
                            ((DevExpress.XtraEditors.CheckEdit)campo).Checked = Convert.ToBoolean(xmlCampo.InnerText);
                        }
                        else if (campo is DevExpress.XtraEditors.SpinEdit)
                        {
                            ((DevExpress.XtraEditors.SpinEdit)campo).Value = Convert.ToInt32(xmlCampo.InnerText);
                        }
                        else if (campo is DevExpress.XtraEditors.TextEdit)
                        {
                            ((DevExpress.XtraEditors.TextEdit)campo).EditValue = xmlCampo.InnerText;
                        }
                    }
                    int count = 0;
                    XmlNode xmlEmpresas = filtros.SelectSingleNode("empresas");
                    foreach (XmlNode item in xmlEmpresas.ChildNodes)
                    {
                        if (count == 0)
                        {
                            gvEmpresas.ClearSelection();
                        }
                        int aux = Convert.ToInt32(item.InnerText);
                        listaRowHandleEmpresa.Add(aux);
                        gvEmpresas.FocusedRowHandle = aux;
                        gvEmpresas.SelectRow(gvEmpresas.FocusedRowHandle);
                        count++;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string MontaStringEmpresas()
        {
            StringBuilder ret = new StringBuilder("(");
            int count = 0;
            foreach (Modelo.Empresa e in listaEmpresas)
            {
                ret.Append(e.Id.ToString());
                if (count < listaEmpresas.Count - 1)
                {
                    ret.Append(", ");
                }
                count++;
            }
            ret.Append(")");

            return ret.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void ValidaSelectManutencao()
        {
            if (gvEmpresas.OptionsSelection.MultiSelect == true)
            {
                List<Modelo.Empresa> empresas = bllEmpresa.GetAllList();
                listaEmpresas.Clear();
                listaRowHandleEmpresa.Clear();
                if (gvEmpresas.GroupCount == 0)
                {
                    for (int y = 0; y < gvEmpresas.SelectedRowsCount; y++)
                    {
                        if (gvEmpresas.GetSelectedRows()[y] >= 0)
                        {
                            listaRowHandleEmpresa.Add(gvEmpresas.GetSelectedRows()[y]);
                            listaEmpresas.AddRange(empresas.Where(e => e.Id == (int)gvEmpresas.GetRowCellValue(gvEmpresas.GetSelectedRows()[y], "id")).ToList());
                        }
                    }
                }
            }
        }

        protected void gcEmpresas_Click(object sender, EventArgs e)
        {
        }

        private void FormBase_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F9:
                    btOk_Click(sender, e);
                    break;
                case Keys.Escape:
                    btCancelar_Click(sender, e);
                    break;
                case Keys.Enter:
                    btOk.Focus();
                    btOk_Click(sender, e);
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, this.Text);
                    break;
                case Keys.F1:
                    simpleButton2_Click(sender, e);
                    break;
            }
        }

        private void FormBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }

        protected virtual void gvEmpresas_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {

        }
    }
}
