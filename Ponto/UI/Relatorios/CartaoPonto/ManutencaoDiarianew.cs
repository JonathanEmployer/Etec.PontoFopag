using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Xml;

namespace UI.Relatorios.CartaoPonto
{
    public partial class relatorio_manutencao_diaria : UI.Relatorios.Base.FormBase
    {
        BLL.Departamento bllDepartamento;
        BLL.CartaoPonto bllCartaoPonto;
        BLL.Parametros bllParametro;
        public relatorio_manutencao_diaria()
        {
            InitializeComponent();
            bllDepartamento = new BLL.Departamento();
            bllCartaoPonto = new BLL.CartaoPonto();
            bllParametro = new BLL.Parametros();
            this.Text = "Relatório de Manutenção Diária";
        }

        private void sbSelecionarEmpresas_Click(object sender, EventArgs e)
        {
            gvEmpresas.SelectAll();
        }

        private void sbSelecionarDepartamento_Click(object sender, EventArgs e)
        {
            gvDepartamentos.SelectAll();
        }

        private void sbLimparEmpresa_Click(object sender, EventArgs e)
        {
            gvEmpresas.ClearSelection();
            gvEmpresas.SelectRow(0);
            gvEmpresas.FocusedRowHandle = 0;
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
                                    if (xmlCampo != null)
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
                     XmlNode xmlDepartamentos = filtros.SelectSingleNode("departamentos");
                     if (xmlDepartamentos != null)
                     {                                                
                         gvDepartamentos.ClearSelection();
                         foreach (XmlNode item in xmlDepartamentos.ChildNodes)
                         {
                             int aux = Convert.ToInt32(item.InnerText);                            
                             gvDepartamentos.FocusedRowHandle = aux;
                             gvDepartamentos.SelectRow(gvDepartamentos.FocusedRowHandle);
                             count++;
                         }
                     }

                }
            }
            catch (FileNotFoundException)
            {

            }
        }

        protected override void GravaXML(bool gravar)
        {
            XmlDocument documentoXml = new XmlDocument();

            documentoXml.AppendChild(documentoXml.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlElement noPai = documentoXml.CreateElement(this.Name);

            XmlElement element = null;
            XmlElement empresas = null;
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

                if (gvDepartamentos.SelectedRowsCount > 0)
                {
                    departamentos = documentoXml.CreateElement("departamentos");
                    int[] listaDepartamento = gvDepartamentos.GetSelectedRows();

                    foreach (int f in listaDepartamento)
                    {
                        XmlElement departamento = documentoXml.CreateElement("departamento");
                        departamento.InnerText = f.ToString();
                        departamentos.AppendChild(departamento);
                    }
                    element.AppendChild(departamentos);
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

        private void sbLimparDepartamento_Click(object sender, EventArgs e)
        {
            gvDepartamentos.ClearSelection();
            gvDepartamentos.SelectRow(0);
            gvDepartamentos.FocusedRowHandle = 0;
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            setaNomeArquivo(this.Name);
            Carrega();
            LeXML();
        }

        protected override void gvEmpresas_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            AtualizaGrids();
        }
        private void AtualizaGrids()
        {
            ValidaSelectEmpresa();
            if (listaRowHandleEmpresa.Count > 0)
            {                
                    gcDepartamentos.DataSource = bllDepartamento.GetPorEmpresa(MontaStringEmpresas());
                    gvDepartamentos.ClearSelection();
                    gvDepartamentos.SelectRow(0);
                    gvDepartamentos.FocusedRowHandle = 0;                
            }
        }

        protected string MontaStringEmpresas()
        {
            if (listaEmpresas.Count > 0)
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
            else
            {
                return "";
            }

            
        }
        private void ValidaSelectEmpresa()
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

        protected string MontaStringDepartamentos()
        {
            
            if (gvDepartamentos.GetSelectedRows().Count() > 0)
            {
                StringBuilder ret = new StringBuilder("(");
                int[] RegSelecionados = gvDepartamentos.GetSelectedRows();
                for (int i = 0; i < RegSelecionados.Count(); i++)
                {
                    ret.Append(gvDepartamentos.GetRowCellValue(RegSelecionados[i], gvDepartamentos.Columns["id"]));
                    if (i != RegSelecionados.Count() - 1)
                        ret.Append(", ");
                }
                ret.Append(")");
                return ret.ToString();
            }
            else
            {
                return "";
            }
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

        //protected override void ValidaSelectManutencao()
        //{
        //    if (gvEmpresas.OptionsSelection.MultiSelect == true)
        //    {
        //        List<Modelo.Empresa> empresas = bllEmpresa.GetAllList();
        //        listaEmpresas.Clear();
        //        listaRowHandleEmpresa.Clear();
        //        if (gvEmpresas.GroupCount == 0)
        //        {
        //            for (int y = 0; y < gvEmpresas.SelectedRowsCount; y++)
        //            {
        //                if (gvEmpresas.GetSelectedRows()[y] >= 0)
        //                {
        //                    listaRowHandleEmpresa.Add(gvEmpresas.GetSelectedRows()[y]);
        //                    listaEmpresas.AddRange(empresas.Where(e => e.Id == (int)gvEmpresas.GetRowCellValue(gvEmpresas.GetSelectedRows()[y], "id")).ToList());
        //                }
        //            }
        //        }
        //    } 
        //    if (gvDepartamentos.OptionsSelection.MultiSelect == true)
        //    {
        //        List<Modelo.Departamento> departamentos = bllDepartamento.GetAllList();
        //        listaDepartamentos.Clear();
        //        listaRowHandleDepartamento.Clear();
        //        if (gvDepartamentos.GroupCount == 0)
        //        {
        //            for (int y = 0; y < gvDepartamentos.SelectedRowsCount; y++)
        //            {
        //                if (gvDepartamentos.GetSelectedRows()[y] >= 0)
        //                {
        //                    listaRowHandleDepartamento.Add(gvEmpresas.GetSelectedRows()[y]);
        //                    listaDepartamentos.AddRange(departamentos.Where(e => e.Id == (int)gvDepartamentos.GetRowCellValue(gvDepartamentos.GetSelectedRows()[y], "id")).ToList());
        //                }
        //            }
        //        }
        //    }
        //}

        protected override void btOk_Click(object sender, EventArgs e)
        {
            btOk.Enabled = false;
            btCancelar.Enabled = false;
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
                    if (listaEmpresas.Count > 0)
                    {
                        empresas = MontaStringEmpresas();
                        tipo = 0;
                    }
                    if (!String.IsNullOrEmpty(MontaStringDepartamentos()))
                    {
                        departamentos = MontaStringDepartamentos();
                        tipo = 1;
                    }                    

                    Dt = bllCartaoPonto.GetCartaoPontoDiaria(txtData.DateTime, txtDataFinal.DateTime, empresas, departamentos, tipo, pb.progressBar);
                    pb.Dispose();
                    nomerel = "rptManutencaoDiaria.rdlc";
                    ds = "dsCartaoPonto_DataTable1";

                    Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

                    parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                    Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("dataInicial", txtData.DateTime.ToShortDateString());
                    Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("dataFinal", txtDataFinal.DateTime.ToShortDateString());
                    parametros.Add(p1);
                    parametros.Add(p2);
                    UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                    form.Text = "Relatório de Manutenção Diária";
                    form.Show();                    
                    if (listaEmpresas.Count > 1)
                    {
                        Titulo = "Todas Empresas";
                    }
                    else
                    {
                        foreach (Modelo.Empresa item in listaEmpresas)
                        {
                            Titulo = item.Nome;
                            break;
                        }
                    }                    
                    GravaXML(chbSalvarFiltro.Checked);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btOk.Enabled = true;
                btCancelar.Enabled = true;
            }
        }
      

        
    }
}
