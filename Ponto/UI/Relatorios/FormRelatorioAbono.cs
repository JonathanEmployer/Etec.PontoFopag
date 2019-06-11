using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Xml;
using System.IO;


namespace UI.Relatorios
{
    public partial class FormRelatorioAbono : UI.Relatorios.Base.FormBaseGridFiltro4Quadros
    {
        private BLL.RelatorioAbono bllRelatorioAbono;

        private string idTipo = "";

        public List<string> TelasAbertas { get; set; }

        public FormRelatorioAbono()
        {
            InitializeComponent();
            //Carrega();
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            IList<String> lstNomeOcorrencias = new List<String>();
            btOk.Enabled = false;
            btCancelar.Enabled = false;
            ValidaSelectOcorrencias();
            preencheDados();
            if (gvOcorrencias.SelectedRowsCount > 0)
            {
                if (ValidaCampos())
                {
                    UI.FormProgressBar pbRelatorioOcorrencia = new UI.FormProgressBar();
                    try
                    {
                        int agruparDepartamento = 0;
                        if (cbAgruparPorDepartamento.Checked)
                            agruparDepartamento = 1;
                        string idsOcorrenciasSelecionadas = String.Join(",",listaDeOcorrencias.Select(x => x.Id).ToList());
                        bllRelatorioAbono = new BLL.RelatorioAbono(dataInicial.DateTime, dataFinal.DateTime, rgTipo.SelectedIndex, idTipo, 0, agruparDepartamento, idsOcorrenciasSelecionadas);

                        DataTable dt = bllRelatorioAbono.GeraRelatorio();

                        DataTable novoDatatable = dt.Copy();

                        string nomerel = "rptAbonoPorFuncionarioData.rdlc";
                        string texto = "Relatório de Abono por Funcionário/Data";

                        string ds = "dsOcorrencia_abono";

                        List<Microsoft.Reporting.WinForms.ReportParameter> parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                        Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("datainicial", dataInicial.DateTime.ToShortDateString());
                        parametros.Add(p1);
                        Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("datafinal", dataFinal.DateTime.ToShortDateString());
                        parametros.Add(p2);
                        Microsoft.Reporting.WinForms.ReportParameter p3 = new Microsoft.Reporting.WinForms.ReportParameter("nomeRelatorio", texto.ToString());
                        parametros.Add(p3);
                        Microsoft.Reporting.WinForms.ReportParameter p4 = new Microsoft.Reporting.WinForms.ReportParameter("quebraDepartamento", agruparDepartamento.ToString());
                        parametros.Add(p4);

                        UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, novoDatatable, parametros);
                        form.Text = texto;
                        form.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Preencha os campos corretamente.");
                }
            }
            else
            {
                MessageBox.Show("Selecione pelo menos um tipo de ocorrência.");
            }
            btOk.Enabled = true;
            btCancelar.Enabled = true;
        }

        protected void ValidaSelectOcorrencias()
        {
            if (gvOcorrencias.OptionsSelection.MultiSelect == true)
            {
                List<Modelo.Ocorrencia> ocorrencias = bllOcorrencia.GetAllList();
                listaDeOcorrencias.Clear();
                listaRowHandleOcorrencias.Clear();
                if (gvOcorrencias.GroupCount == 0)
                {
                    for (int y = 0; y < gvOcorrencias.SelectedRowsCount; y++)
                    {
                        if (gvOcorrencias.GetSelectedRows()[y] >= 0)
                        {
                            listaRowHandleOcorrencias.Add(gvOcorrencias.GetSelectedRows()[y]);
                            listaDeOcorrencias.AddRange(ocorrencias.Where(e => e.Id == (int)gvOcorrencias.GetRowCellValue(gvOcorrencias.GetSelectedRows()[y], "id")).ToList());
                        }
                    }
                }
            }
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
                        SetValorCampoFiltro(filtros, campo);
                    }
                    int count = 0;
                    XmlNode xmlEmpresas = filtros.SelectSingleNode("empresas");
                    XmlNode xmlOcorrencias = filtros.SelectSingleNode("ocorrencias");
                    if (xmlOcorrencias != null)
                    {
                        listaDeOcorrencias.Clear();
                        listaRowHandleOcorrencias.Clear();
                        gvOcorrencias.ClearSelection();
                        foreach (XmlNode item in xmlOcorrencias.ChildNodes)
                        {
                            int aux = Convert.ToInt32(item.InnerText);
                            listaRowHandleOcorrencias.Add(aux);
                            gvOcorrencias.FocusedRowHandle = aux;
                            gvOcorrencias.SelectRow(gvOcorrencias.FocusedRowHandle);
                            count++;
                        }
                    }
                    if (xmlEmpresas != null)
                    {
                        listaEmpresas.Clear();
                        listaRowHandleEmpresa.Clear();
                        gvEmpresas.ClearSelection();
                        foreach (XmlNode item in xmlEmpresas.ChildNodes)
                        {
                            int aux = Convert.ToInt32(item.InnerText);
                            listaRowHandleEmpresa.Add(aux);
                            gvEmpresas.FocusedRowHandle = aux;
                            gvEmpresas.SelectRow(gvEmpresas.FocusedRowHandle);
                            count++;
                        }

                        XmlNode xmlDepartamentos = filtros.SelectSingleNode("departamentos");
                        if (xmlDepartamentos != null)
                        {
                            listaDepartamentos.Clear();
                            listaRowHandleDepartamento.Clear();
                            gvDepartamentos.ClearSelection();
                            foreach (XmlNode item in xmlDepartamentos.ChildNodes)
                            {
                                int aux = Convert.ToInt32(item.InnerText);
                                listaRowHandleDepartamento.Add(aux);
                                gvDepartamentos.FocusedRowHandle = aux;
                                gvDepartamentos.SelectRow(gvDepartamentos.FocusedRowHandle);
                                count++;
                            }


                            XmlNode xmlFuncionarios = filtros.SelectSingleNode("funcionarios");
                            if (xmlFuncionarios != null)
                            {
                                listaFuncionarios.Clear();
                                listaRowHandleFuncionario.Clear();
                                gvFuncionarios.ClearSelection();
                                foreach (XmlNode item in xmlFuncionarios.ChildNodes)
                                {
                                    int aux = Convert.ToInt32(item.InnerText);
                                    listaRowHandleFuncionario.Add(aux);
                                    gvFuncionarios.FocusedRowHandle = aux;
                                    gvFuncionarios.SelectRow(gvFuncionarios.FocusedRowHandle);
                                    count++;
                                }
                            }
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
            XmlElement funcionarios = null;
            XmlElement ocorrencias = null;

            XmlElement ler = documentoXml.CreateElement("ler");
            ler.InnerText = gravar.ToString();
            noPai.AppendChild(ler);
            if (gravar)
            {
                XmlElement xmlTitulo = documentoXml.CreateElement("titulo");
                xmlTitulo.InnerText = Titulo;
                noPai.AppendChild(xmlTitulo);

                element = documentoXml.CreateElement("filtros");

                if (listaRowHandleOcorrencias.Count > 0)
                {
                    ocorrencias = documentoXml.CreateElement("ocorrencias");

                    foreach (int f in listaRowHandleOcorrencias)
                    {
                        XmlElement ocorrencia = documentoXml.CreateElement("ocorrencias");
                        ocorrencia.InnerText = f.ToString();
                        ocorrencias.AppendChild(ocorrencia);
                    }

                    element.AppendChild(ocorrencias);
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

                if (listaRowHandleDepartamento.Count > 0)
                {
                    departamentos = documentoXml.CreateElement("departamentos");

                    foreach (int f in listaRowHandleDepartamento)
                    {
                        XmlElement departamento = documentoXml.CreateElement("departamento");
                        departamento.InnerText = f.ToString();
                        departamentos.AppendChild(departamento);
                    }

                    element.AppendChild(departamentos);
                }

                if (listaRowHandleFuncionario.Count > 0)
                {
                    funcionarios = documentoXml.CreateElement("funcionarios");

                    foreach (int f in listaRowHandleFuncionario)
                    {
                        XmlElement funcionario = documentoXml.CreateElement("funcionario");
                        funcionario.InnerText = f.ToString();
                        funcionarios.AppendChild(funcionario);
                    }

                    element.AppendChild(funcionarios);
                }

                foreach (Control campo in tabPage1.Controls)
                {
                    SalvaValorCampoXml(documentoXml, element, campo);
                }

                noPai.AppendChild(element);
            }
            documentoXml.AppendChild(noPai);

            documentoXml.Save(Arquivo.ToString());
        }

        private bool ValidaCampos()
        {

            bool ret = true;

            if ((int)rgTipo.SelectedIndex > -1)
                dxErrorProvider1.SetError(rgTipo, "");
            else
            {
                dxErrorProvider1.SetError(rgTipo, "Selecione um tipo.");
                ret = false;
            }

            if (dataInicial.DateTime != new DateTime() && dataFinal.DateTime != new DateTime())
            {
                dxErrorProvider1.SetError(dataInicial, "");
                if (dataInicial.DateTime > dataFinal.DateTime)
                {
                    dxErrorProvider1.SetError(dataFinal, "A data final deve ser maior ou igual a data inicial.");
                    ret = false;
                }
                else
                {
                    dxErrorProvider1.SetError(dataFinal, "");
                }
            }
            else
            {
                if (dataInicial.DateTime != new DateTime())
                {
                    dxErrorProvider1.SetError(dataInicial, "");
                }
                else
                {
                    dxErrorProvider1.SetError(dataInicial, "Selecione a data inicial.");
                    ret = false;
                }


                if (dataFinal.DateTime != new DateTime())
                {
                    dxErrorProvider1.SetError(dataFinal, "");
                }
                else
                {
                    dxErrorProvider1.SetError(dataFinal, "Selecione a data final.");
                    ret = false;
                }
            }
            switch (rgTipo.SelectedIndex)
            {
                case 0:
                    if (MontaStringEmpresas() == "()")
                    {
                        dxErrorProvider1.SetError(rgTipo, "Não há empresa(s) selecionada(s).");
                        ret = false;
                    }
                    break;
                case 1:
                    if (MontaStringDepartamentos() == "()")
                    {
                        dxErrorProvider1.SetError(rgTipo, "Não há departamento(s) selecionado(s).");
                        ret = false;
                    }
                    break;
                case 2:
                    if (MontaStringFuncionarios() == "()")
                    {
                        dxErrorProvider1.SetError(rgTipo, "Não há funcionário(s) selecionado(s).");
                        ret = false;
                    }
                    break;
            }

            return ret;
        }

        private void preencheDados()
        {
            ValidaSelectManutencao();
            switch (rgTipo.SelectedIndex)
            {
                case 0:
                    idTipo = MontaStringEmpresas();
                    break;
                case 1:
                    idTipo = MontaStringDepartamentos();
                    break;
                case 2:
                    idTipo = MontaStringFuncionarios();
                    break;
                default: break;
            }
        }

        private void FormRelatorioAbono_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    btOk.Focus();
                    btOk_Click(sender, e);
                    break;
                case Keys.Escape:
                    btCancelar.Focus();
                    btCancelar_Click(sender, e);
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }

        private void FormRelatorioAbono_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
            this.GravaXML(chbSalvarFiltro.Checked);
        }

        protected override void Carrega()
        {
            if (rgTipo.SelectedIndex == 0)
            {
                gcEmpresas.DataSource = bllEmpresa.GetAll();
                if (gvEmpresas.RowCount > 0)
                {
                    gvEmpresas.SelectRow(0);
                }
                gcDepartamentos.DataSource = null;
                gcDepartamentos.Enabled = false;
                sbSelecionarDepartamentos.Enabled = false;
                sbLimparDepartamento.Enabled = false;
                gcFuncionarios.DataSource = null;
                gcFuncionarios.Enabled = false;
                sbSelecionarFuncionarios.Enabled = false;
                sbLimparFuncionarios.Enabled = false;

            }
            else if (rgTipo.SelectedIndex == 1)
            {
                gcEmpresas.DataSource = bllEmpresa.GetAll();
                if (gvEmpresas.RowCount > 0)
                {
                    gvEmpresas.SelectAll();
                }
                if (gvDepartamentos.RowCount > 0)
                {
                    gvDepartamentos.SelectRow(0);
                }
                gcDepartamentos.Enabled = true;
                sbSelecionarDepartamentos.Enabled = true;
                sbLimparDepartamento.Enabled = true;
                gcFuncionarios.DataSource = null;
                gcFuncionarios.Enabled = false;
                sbSelecionarFuncionarios.Enabled = false;
                sbLimparFuncionarios.Enabled = false;
            }
            else
            {
                gcEmpresas.DataSource = bllEmpresa.GetAll();
                if (gvEmpresas.RowCount > 0)
                {
                    gvEmpresas.SelectAll();
                }
                if (gvDepartamentos.RowCount > 0)
                {
                    gvDepartamentos.SelectAll();
                }
                if (gvFuncionarios.RowCount > 0)
                {
                    gvFuncionarios.SelectRow(0);
                }
                gcDepartamentos.Enabled = true;
                sbSelecionarDepartamentos.Enabled = true;
                sbLimparDepartamento.Enabled = true;
                gcFuncionarios.Enabled = true;
                sbSelecionarFuncionarios.Enabled = true;
                sbLimparFuncionarios.Enabled = true;
            }
        }


        protected override void rgTipo_EditValueChanged(object sender, EventArgs e)
        {
            //Carrega();
            TipoAlterado();
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            ConfiguraGridOcorrencia();
            base.FormBase_Load(sender, e);
            setaNomeArquivo(this.Name);            
            LeXML();
        }

        private void ConfiguraGridOcorrencia()
        {
            this.gvOcorrencias.OptionsSelection.MultiSelect = true;
            gcOcorrencias.DataSource = bllOcorrencia.GetAll();
        }
    }
}
