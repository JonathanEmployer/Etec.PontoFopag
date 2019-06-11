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
    public partial class HorarioPorDescricao : REL.FormBaseSemEmpresa
    {
        public HorarioPorDescricao()
        {
            InitializeComponent();
        }

        private List<Modelo.Horario> listaHorarios = new List<Modelo.Horario>();
        private List<int> listaRowHandleHorario = new List<int>();

        private BLL.Horario bllHorario = new BLL.Horario();
        private Modelo.Horario objHorario = new Modelo.Horario();

        protected override void Carrega()
        {
            gcHorarios.DataSource = bllHorario.GetAll();

            this.gvHorarios.OptionsSelection.MultiSelect = true;

            if (gvHorarios.RowCount > 0)
            {
                gvHorarios.SelectRow(0);
            }

            setaNomeArquivo(this.Name);
            LeXML();
        }

        protected override void GravaXML(bool gravar)
        {
            XmlDocument documentoXml = new XmlDocument();

            documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlElement noPai = documentoXml.CreateElement(this.Name);

            XmlElement element = null;
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
                                    ((DevExpress.XtraEditors.DateEdit)campo).EditValue = Convert.ToDateTime(xmlCampo.InnerText);
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
                    XmlNode xmlHorarios = filtros.SelectSingleNode("horarios");
                    if (xmlHorarios != null)
                    {
                        foreach (XmlNode item in xmlHorarios.ChildNodes)
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
            }
            catch (FileNotFoundException)
            {

            }
        }

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

        protected void ValidaSelectManutencao()
        {
            if (gvHorarios.OptionsSelection.MultiSelect == true)
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
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            Carrega();
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            base.btOk_Click(sender, e);
            ValidaSelectManutencao();
            Dt = bllHorario.GetPorDescricao(MontaStringHorarios());
            nomerel = "rptHorarioDescricao.rdlc";
            ds = "dsHorario_horario";
            parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
            FormRelatorioBase form = new FormRelatorioBase(nomerel, ds, Dt, parametros);
            form.Text = "Relatório de Horários por Descrição";
            form.Show();
            this.Close();  
        }
    }
}
