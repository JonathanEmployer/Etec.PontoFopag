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
    public partial class FuncionariosHorario : REL.FormBase
    {
        //private List<Modelo.Empresa> listaEmpresas = new List<Modelo.Empresa>();
        //private List<int> listaRowHandleEmpresa = new List<int>();
        //private BLL.Empresa bllEmpresa = new BLL.Empresa();        
        //private Modelo.Empresa objEmpresa = new Modelo.Empresa();

        private BLL.Funcionario bllFuncionario = new BLL.Funcionario();

        private List<Modelo.Horario> listaHorarios = new List<Modelo.Horario>();
        private List<int> listaRowHandleHorario = new List<int>();
        private BLL.Horario bllHorario = new BLL.Horario();
        private Modelo.Horario objHorario = new Modelo.Horario();


        public FuncionariosHorario()
        {
            InitializeComponent();
        }

        protected virtual void CarregaHorario()
        {
            gcHorarios.DataSource = bllHorario.GetAll();

            this.gvHorarios.OptionsSelection.MultiSelect = true;

            if (gvHorarios.RowCount > 0)
            {
                gvHorarios.SelectRow(0);
            }
            Carrega();
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);            
            CarregaHorario();
        }

        protected virtual void ValidaSelectHorario()
        {
            listaHorarios.Clear();
            listaRowHandleHorario.Clear();
            if (gvHorarios.GroupCount == 0)
            {
                for (int y = 0; y < gvHorarios.SelectedRowsCount; y++)
                {
                    if (gvHorarios.GetSelectedRows()[y] >= 0)
                    {
                        listaRowHandleHorario.Add(gvHorarios.GetSelectedRows()[y]);
                        listaHorarios.Add(bllHorario.LoadObject((int)gvHorarios.GetRowCellValue(gvHorarios.GetSelectedRows()[y], "id")));
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
            XmlElement empresas = null;
            XmlElement horarios = null;
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

                if (listaRowHandleHorario.Count > 0)
                {
                    horarios = documentoXml.CreateElement("horarios");

                    foreach (int f in listaRowHandleHorario)
                    {
                        XmlElement horario = documentoXml.CreateElement("horario");
                        horario.InnerText = f.ToString();
                        horarios.AppendChild(horario);
                    }

                    element.AppendChild(horarios);
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
                    XmlNode xmlempresas = filtros.SelectSingleNode("empresas");
                    foreach (XmlNode item in xmlempresas.ChildNodes)
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

                    count = 0;
                    XmlNode xmlhorarios = filtros.SelectSingleNode("horarios");
                    foreach (XmlNode item in xmlhorarios.ChildNodes)
                    {
                        if (count == 0)
                        {
                            gvHorarios.ClearSelection();
                        }
                        int aux = Convert.ToInt32(item.InnerText);
                        listaRowHandleHorario.Add(aux);
                        gvHorarios.FocusedRowHandle = aux;
                        gvHorarios.SelectRow(gvHorarios.FocusedRowHandle);
                        count++;
                    }
                }
            }
            catch (FileNotFoundException)
            {

            }
        }

        //protected string MontaStringEmpresas()
        //{
        //    StringBuilder ret = new StringBuilder("(");
        //    int count = 0;
        //    foreach (Modelo.Empresa e in listaEmpresas)
        //    {
        //        ret.Append(e.Id.ToString());
        //        if (count < listaEmpresas.Count - 1)
        //        {
        //            ret.Append(", ");
        //        }
        //        count++;
        //    }
        //    ret.Append(")");

        //    return ret.ToString();
            
        //}

        protected string MontaStringHorarios()
        {
            StringBuilder ret = new StringBuilder("(");
            int count = 0;
            foreach (Modelo.Horario e in listaHorarios)
            {
                ret.Append(e.Id.ToString());
                if (count < listaHorarios.Count - 1)
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
            ValidaSelectManutencao();
            ValidaSelectHorario();
            base.btOk_Click(sender, e);

            Dt = bllFuncionario.GetPorHorarioRel(MontaStringHorarios(),MontaStringEmpresas());
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("empresa", Titulo);
            parametros.Add(p1);
            nomerel = "rptFuncionariosPorHorario.rdlc";
            ds = "dsFuncionarios_Funcionarios";
            FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Funcionários por Horário";
            form.Show();
            this.Close();
        }
    }
        
}
