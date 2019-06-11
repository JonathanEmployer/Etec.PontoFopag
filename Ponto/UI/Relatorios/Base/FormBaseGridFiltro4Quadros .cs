using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.ReportingServices.ReportRendering;

namespace UI.Relatorios.Base
{
    public partial class FormBaseGridFiltro4Quadros : UI.Relatorios.Base.FormBaseSemEmpresa
    {
        protected List<Modelo.Empresa> listaEmpresas = new List<Modelo.Empresa>();
        protected List<int> listaRowHandleEmpresa = new List<int>();

        protected BLL.Empresa bllEmpresa;
        protected Modelo.Empresa objEmpresa = new Modelo.Empresa();

        protected List<Modelo.Departamento> listaDepartamentos = new List<Modelo.Departamento>();
        protected List<int> listaRowHandleDepartamento = new List<int>();

        protected BLL.Departamento bllDepartamento;
        protected Modelo.Departamento objDepartamento = new Modelo.Departamento();

        protected List<Modelo.Funcionario> listaFuncionarios = new List<Modelo.Funcionario>();
        protected List<int> listaRowHandleFuncionario = new List<int>();

        protected BLL.Funcionario bllFuncionario;
        protected Modelo.Funcionario objFuncionario = new Modelo.Funcionario();

        protected BLL.Ocorrencia bllOcorrencia;
        protected List<Modelo.Ocorrencia> listaDeOcorrencias = new List<Modelo.Ocorrencia>();
        protected List<int> listaRowHandleOcorrencias = new List<int>();


        public FormBaseGridFiltro4Quadros()
        {
            InitializeComponent();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncionario = new BLL.Funcionario();
            bllOcorrencia = new BLL.Ocorrencia();
        }

        protected override void FormBase_Load(object sender, EventArgs e)
        {
            this.gvEmpresas.OptionsSelection.MultiSelect = true;
            this.gvDepartamentos.OptionsSelection.MultiSelect = true;
            this.gvFuncionarios.OptionsSelection.MultiSelect = true;
        }

        protected virtual void SelecionaRegistroPorID(string col, int ID, DevExpress.XtraGrid.Views.Grid.GridView dataGridView1)
        {
            dataGridView1.ClearSelection();
            int posicao = dataGridView1.LocateByDisplayText(0, dataGridView1.Columns.ColumnByFieldName(col), ID.ToString());
            if (posicao >= 0)
            {
                if (posicao > dataGridView1.RowCount - 1)
                {
                    posicao = dataGridView1.RowCount - 1;
                }
                dataGridView1.FocusedRowHandle = posicao;
                dataGridView1.SelectRow(posicao);
            }
            else
            {
                dataGridView1.SelectRow(0);
                dataGridView1.FocusedRowHandle = 0;
            }
        }

        protected virtual void ValidaSelectManutencao()
        {
            ValidaSelectEmpresa();

            ValidaSelectDepartamento();

            ValidaSelectFuncionario();
        }

        private void ValidaSelectFuncionario()
        {
            if (gvFuncionarios.OptionsSelection.MultiSelect == true)
            {
                List<Modelo.Funcionario> funcionarios = bllFuncionario.GetAllList(false);
                listaFuncionarios.Clear();
                listaRowHandleFuncionario.Clear();
                if (gvFuncionarios.GroupCount == 0)
                {
                    for (int y = 0; y < gvFuncionarios.SelectedRowsCount; y++)
                    {
                        if (gvFuncionarios.GetSelectedRows()[y] >= 0)
                        {
                            listaRowHandleFuncionario.Add(gvFuncionarios.GetSelectedRows()[y]);
                            listaFuncionarios.AddRange(funcionarios.Where(f => f.Id == (int)gvFuncionarios.GetRowCellValue(gvFuncionarios.GetSelectedRows()[y], gvFuncionarios.Columns["id"])).ToList());
                        }
                    }
                }
            }
        }

       

        private void ValidaSelectDepartamento()
        {
            if (gvDepartamentos.OptionsSelection.MultiSelect == true)
            {
                List<Modelo.Departamento> departamentos = bllDepartamento.GetAllList();
                listaDepartamentos.Clear();
                listaRowHandleDepartamento.Clear();
                if (gvDepartamentos.GroupCount == 0)
                {
                    for (int y = 0; y < gvDepartamentos.SelectedRowsCount; y++)
                    {
                        if (gvDepartamentos.GetSelectedRows()[y] >= 0)
                        {
                            listaRowHandleDepartamento.Add(gvDepartamentos.GetSelectedRows()[y]);
                            listaDepartamentos.AddRange(departamentos.Where(d => d.Id == (int)gvDepartamentos.GetRowCellValue(gvDepartamentos.GetSelectedRows()[y], "id")).ToList());
                        }
                    }
                }
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

        protected string MontaStringFuncionarios()
        {
            StringBuilder ret = new StringBuilder("(");
            int count = 0;
            foreach (Modelo.Funcionario e in listaFuncionarios)
            {
                ret.Append(e.Id.ToString());
                if (count < listaFuncionarios.Count - 1)
                {
                    ret.Append(", ");
                }
                count++;
            }
            ret.Append(")");

            return ret.ToString();
        }

        protected string MontaStringOcorrencias()
        {
            StringBuilder ret = new StringBuilder("(");
            string retorno = String.Empty;
            if (listaDeOcorrencias.Count > 0)
            {
                string idsOcorrencias = String.Join(",", listaDeOcorrencias.Select(s => "'" + s.Descricao + "'"));
                ret.Append(idsOcorrencias);
            }
            ret.Append(")");

            return ret.ToString();
        }

        protected int MontaIntTipo()
        {
            return rgTipo.SelectedIndex;
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

        protected static void SetValorCampoFiltro(XmlNode filtros, Control campo)
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
            else if (campo is DevExpress.XtraEditors.RadioGroup)
            {
                ((DevExpress.XtraEditors.RadioGroup)campo).SelectedIndex = Convert.ToInt32(xmlCampo.InnerText);
            }
            else if (campo is DevExpress.XtraEditors.GroupControl)
            {
                foreach (Control campo2 in campo.Controls)
                {
                    SetValorCampoFiltro(filtros, campo2);
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

        protected static void SalvaValorCampoXml(XmlDocument documentoXml, XmlElement element, Control campo)
        {
            XmlElement c;
            if (campo is DevExpress.XtraEditors.CheckEdit)
            {
                c = documentoXml.CreateElement(campo.Name);
                c.InnerText = ((DevExpress.XtraEditors.CheckEdit)campo).Checked.ToString();
                element.AppendChild(c);
            }
            else if (campo is DevExpress.XtraEditors.DateEdit)
            {
                c = documentoXml.CreateElement(campo.Name);
                c.InnerText = ((DevExpress.XtraEditors.DateEdit)campo).DateTime.ToString();
                element.AppendChild(c);
            }
            else if (campo is DevExpress.XtraEditors.BaseEdit)
            {
                c = documentoXml.CreateElement(campo.Name);
                c.InnerText = ((DevExpress.XtraEditors.BaseEdit)campo).EditValue.ToString();
                element.AppendChild(c);
            }
            else if (campo is DevExpress.XtraEditors.RadioGroup)
            {
                c = documentoXml.CreateElement(campo.Name);
                c.InnerText = ((DevExpress.XtraEditors.RadioGroup)campo).SelectedIndex.ToString();
                element.AppendChild(c);
            }
            else if (campo is DevExpress.XtraEditors.GroupControl)
            {
                foreach (Control campo2 in campo.Controls)
                {
                    SalvaValorCampoXml(documentoXml, element, campo2);
                }
            }
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            ValidaSelectManutencao();

            if (listaEmpresas.Count == 0)
            {
                dxErrorProvider1.SetError(rgTipo, "Não há empresa(s) selecionada(s).");
                throw new Exception("Preencha os campos corretamente.");
            }
            else if (rgTipo.SelectedIndex == -1)
            {
                dxErrorProvider1.SetError(rgTipo, "Selecione um tipo.");
                throw new Exception("Preencha os campos corretamente.");
            }
            else if (listaDepartamentos.Count == 0 && rgTipo.SelectedIndex == 1)
            {
                dxErrorProvider1.SetError(rgTipo, "Não há departamento(s) selecionado(s).");
                throw new Exception("Preencha os campos corretamente.");
            }
            else if (listaFuncionarios.Count == 0 && rgTipo.SelectedIndex == 2)
            {
                dxErrorProvider1.SetError(rgTipo, "Não há funcionário(s) selecionado(s).");
                throw new Exception("Preencha os campos corretamente.");
            }
            else
            {
                dxErrorProvider1.SetError(rgTipo, "");
                Titulo = listaEmpresas[0].Nome;
                GravaXML(chbSalvarFiltro.Checked);
            }
        }

        protected virtual void rgTipo_EditValueChanged(object sender, EventArgs e)
        {
        }

        protected virtual void TipoAlterado()
        {
            gvEmpresas.ClearSelection();
            gvDepartamentos.ClearSelection();
            gvFuncionarios.ClearSelection();
            gvEmpresas.FocusedRowHandle = 0;
            gvDepartamentos.FocusedRowHandle = 0;
            gvFuncionarios.FocusedRowHandle = 0;
            switch (rgTipo.SelectedIndex)
            {
                case -1:
                    gcEmpresas.Enabled = false;
                    sbLimparEmpresa.Enabled = false;
                    sbSelecionarEmpresas.Enabled = false;

                    gcDepartamentos.Enabled = false;
                    sbLimparDepartamento.Enabled = false;
                    sbSelecionarDepartamentos.Enabled = false;

                    gcFuncionarios.Enabled = false;
                    sbLimparFuncionarios.Enabled = false;
                    sbSelecionarFuncionarios.Enabled = false;

                    gcEmpresas.DataSource = null;
                    gcDepartamentos.DataSource = null;
                    gcFuncionarios.DataSource = null;
                    break;
                case 0:
                    gcEmpresas.Enabled = true;
                    sbLimparEmpresa.Enabled = true;
                    sbSelecionarEmpresas.Enabled = true;

                    gcDepartamentos.Enabled = false;
                    sbLimparDepartamento.Enabled = false;
                    sbSelecionarDepartamentos.Enabled = false;

                    gcFuncionarios.Enabled = false;
                    sbLimparFuncionarios.Enabled = false;
                    sbSelecionarFuncionarios.Enabled = false;

                    gcEmpresas.DataSource = bllEmpresa.GetAll();
                    gcDepartamentos.DataSource = null;
                    gcFuncionarios.DataSource = null;
                    break;
                case 1:
                    gcEmpresas.Enabled = true;
                    sbLimparEmpresa.Enabled = true;
                    sbSelecionarEmpresas.Enabled = true;

                    gcDepartamentos.Enabled = true;
                    sbLimparDepartamento.Enabled = true;
                    sbSelecionarDepartamentos.Enabled = true;

                    gcFuncionarios.Enabled = false;
                    sbLimparFuncionarios.Enabled = false;
                    sbSelecionarFuncionarios.Enabled = false;

                    gcEmpresas.DataSource = bllEmpresa.GetAll();
                    if (gvEmpresas.RowCount > 0)
                    {
                        gvEmpresas.SelectAll();
                    }
                    gcFuncionarios.DataSource = null;
                    break;
                case 2:
                    gcEmpresas.Enabled = true;
                    sbLimparEmpresa.Enabled = true;
                    sbSelecionarEmpresas.Enabled = true;

                    gcDepartamentos.Enabled = true;
                    sbLimparDepartamento.Enabled = true;
                    sbSelecionarDepartamentos.Enabled = true;

                    gcFuncionarios.Enabled = true;
                    sbLimparFuncionarios.Enabled = true;
                    sbSelecionarFuncionarios.Enabled = true;

                    gcEmpresas.DataSource = bllEmpresa.GetAll();
                    if (gvEmpresas.RowCount > 0)
                    {
                        gvEmpresas.SelectAll();
                    }
                    if (gvDepartamentos.RowCount > 0)
                    {
                        gvDepartamentos.SelectAll();
                    }
                    break;
            }
        }

        protected virtual void sbSelecionarEmpresas_Click(object sender, EventArgs e)
        {
            gvEmpresas.SelectAll();
        }

        protected virtual void sbLimparEmpresa_Click(object sender, EventArgs e)
        {
            listaRowHandleEmpresa.Clear();
            listaEmpresas.Clear();
            gvEmpresas.ClearSelection();
            gvEmpresas.SelectRow(0);
            gvEmpresas.FocusedRowHandle = 0;
        }

        protected virtual void sbSelecionarDepartamentos_Click(object sender, EventArgs e)
        {
            gvDepartamentos.SelectAll();
        }

        protected virtual void sbLimparDepartamento_Click(object sender, EventArgs e)
        {
            gvDepartamentos.ClearSelection();
            listaRowHandleDepartamento.Clear();
            listaDepartamentos.Clear();
            gvDepartamentos.SelectRow(0);
            gvDepartamentos.FocusedRowHandle = 0;
        }

        protected virtual void sbSelecionarFuncionarios_Click(object sender, EventArgs e)
        {
            gvFuncionarios.SelectAll();
        }

        protected virtual void sbLimparFuncionarios_Click(object sender, EventArgs e)
        {
            gvFuncionarios.ClearSelection();
            listaRowHandleFuncionario.Clear();
            listaFuncionarios.Clear();
            gvFuncionarios.SelectRow(0);
            gvFuncionarios.FocusedRowHandle = 0;
        }

        private void gvEmpresas_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            AtualizaGrids();
        }

        private void AtualizaGrids()
        {
            ValidaSelectEmpresa();
            if (listaRowHandleEmpresa.Count > 0)
            {
                if (rgTipo.SelectedIndex == 1 || rgTipo.SelectedIndex == 2)
                {
                    gcDepartamentos.DataSource = bllDepartamento.GetPorEmpresa(MontaStringEmpresas());
                    gvDepartamentos.ClearSelection();
                    gvDepartamentos.SelectRow(0);
                    gvDepartamentos.FocusedRowHandle = 0;
                }
            }
        }

        private void AuxAtualizaGrids()
        {
            ValidaSelectDepartamento();
            if (rgTipo.SelectedIndex == 2)
            {
                if (listaRowHandleDepartamento.Count > 0)
                {
                    gcFuncionarios.DataSource = bllFuncionario.GetPorDepartamentoRel(MontaStringDepartamentos());
                    gvFuncionarios.ClearSelection();
                    gvFuncionarios.SelectRow(0);
                    gvFuncionarios.FocusedRowHandle = 0;
                }
                else
                {
                    gcFuncionarios.DataSource = null;
                }
            }
        }

        private void gvDepartamentos_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            AuxAtualizaGrids();
        }

        private void FormBaseGridFiltro1_KeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.KeyCode)
            //{
            //    case Keys.Escape:
            //        btCancelar.Focus();
            //        btCancelar_Click(sender, e);
            //        break;
            //    case Keys.Enter:
            //        btOk.Focus();
            //        btOk_Click(sender, e);
            //        break;
            //    case Keys.F12:
            //        if (Form.ModifierKeys == Keys.Control)
            //            cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, this.Text);;
            //        break;
            //}
        }

    }
}
