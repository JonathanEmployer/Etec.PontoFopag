using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace REL
{
    public partial class FuncionariosPorDepartamento : REL.FormBase2
    {
        private List<Modelo.Departamento> listaDepartamentos = new List<Modelo.Departamento>();
        private List<int> listaRowHandleDepartamentos = new List<int>();
        private BLL.Departamento bllDepartamento = new BLL.Departamento();
        private BLL.Funcionario bllFuncionario = new BLL.Funcionario();
        private Modelo.Departamento objDepartamento = new Modelo.Departamento();

        public FuncionariosPorDepartamento()
        {
            InitializeComponent();
        }

        protected override void Carrega()
        {
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAll();

            this.gvDepartamentos.OptionsSelection.MultiSelect = true;

            if (gvDepartamentos.RowCount > 0)
            {
                gvDepartamentos.SelectRow(0);
            }

            setaNomeArquivo(this.Name);
            LeXML();
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            Carrega();
        }

        protected virtual void ValidaSelectDepartamento()
        {
            listaDepartamentos.Clear();
            listaRowHandleDepartamentos.Clear();
            if (gvDepartamentos.GroupCount == 0)
            {
                for (int y = 0; y < gvDepartamentos.SelectedRowsCount; y++)
                {
                    if (gvDepartamentos.GetSelectedRows()[y] >= 0)
                    {
                        listaRowHandleDepartamentos.Add(gvDepartamentos.GetSelectedRows()[y]);
                        listaDepartamentos.Add(bllDepartamento.LoadObject((int)gvDepartamentos.GetRowCellValue(gvDepartamentos.GetSelectedRows()[y], "id")));
                    }
                }
            }
        }

        protected override void GravaXML(bool gravar)
        {
            XmlDocument documentoXml = new XmlDocument();

            documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlElement noPai = documentoXml.CreateElement(this.Name);

            XmlElement element = null;
            XmlElement departamentos = null;
            XmlElement ler = documentoXml.CreateElement("ler");
            ler.InnerText = gravar.ToString();
            noPai.AppendChild(ler);
            if (gravar)
            {
                XmlElement xmlTitulo = documentoXml.CreateElement("titulo");
                xmlTitulo.InnerText = Titulo;
                noPai.AppendChild(xmlTitulo);

                element = documentoXml.CreateElement("filtros");
                
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
                        c.InnerText = ((DevExpress.XtraEditors.DateEdit)campo).EditValue.ToString();
                        element.AppendChild(c);
                    }
                    else if (campo is DevExpress.XtraEditors.BaseEdit)
                    {
                        c = documentoXml.CreateElement(campo.Name);
                        c.InnerText = ((DevExpress.XtraEditors.BaseEdit)campo).EditValue.ToString();
                        element.AppendChild(c);
                    }
                }

                if (listaRowHandleDepartamentos.Count > 0)
                {
                    departamentos = documentoXml.CreateElement("departamentos");

                    foreach (int f in listaRowHandleDepartamentos)
                    {
                        XmlElement departamento = documentoXml.CreateElement("departamento");
                        departamento.InnerText = f.ToString();
                        departamentos.AppendChild(departamento);
                    }

                    element.AppendChild(departamentos);
                }

                noPai.AppendChild(element);
            }
            documentoXml.AppendChild(noPai);

            documentoXml.Save(Arquivo.ToString());
        }

        protected override void LeXML()
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
                                    ((DevExpress.XtraEditors.DateEdit)campo).EditValue = Convert.ToDateTime(xmlCampo.Value);
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

                    int count = 0;
                    XmlNode xmlDepartamentos = filtros.SelectSingleNode("departamentos");
                    foreach (XmlNode item in xmlDepartamentos.ChildNodes)
                    {
                        if (count == 0)
                        {
                            gvDepartamentos.ClearSelection();
                        }
                        int aux = Convert.ToInt32(item.InnerText);
                        listaRowHandleDepartamentos.Add(aux);
                        gvDepartamentos.FocusedRowHandle = aux;
                        gvDepartamentos.SelectRow(gvDepartamentos.FocusedRowHandle);
                        count++;
                    }
                }
            }
            catch (FileNotFoundException)
            {

            }
        }

        private void cbIdEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)cbIdEmpresa.EditValue > 0)
            {
                gcDepartamentos.DataSource = bllDepartamento.GetPorEmpresa((int)cbIdEmpresa.EditValue);
            }
        }

        protected string MontaStringDepartamentos()
        {
            StringBuilder ret = new StringBuilder("(");
            int count = 0;
            foreach (Modelo.Departamento e in listaDepartamentos)
            {
                ret.Append(e.Id.ToString());
                if (count < listaDepartamentos.Count - 1)
                {
                    ret.Append(", ");
                }
                count++;
            }
            ret.Append(")");

            return ret.ToString();
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            ValidaSelectDepartamento();
            base.btOk_Click(sender, e);

            Dt = bllFuncionario.GetPorDepartamentoRel(MontaStringDepartamentos());
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
            parametros.Add(p1);
            nomerel = "rptFuncionariosPorDepartamento.rdlc";
            ds = "dsFuncionarios_Funcionarios";
            FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Funcionários por Departamento";
            form.Show();
            this.Close();
        }
    }
}
