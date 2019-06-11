using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Modelo.Proxy;
using Modelo;
using System.Xml;
using System.IO;

namespace UI.Relatorios
{
    public partial class Inconsistencias : UI.Relatorios.Base.FormBaseGridFiltro4Quadros
    {
        private BLL.Marcacao bllMarcacao;
        private BLL.Parametros bllParametro;
        private BLL.CartaoPonto bllCartaoPonto;
        private BLL.Inconsistencia bllInconsistencia;
        protected List<int> listaRowHandleInconsistencias = new List<int>();

        private bool bCarrega { get; set; }

        public Inconsistencias()
        {
            InitializeComponent();
            this.Name = "LimiteHorario";
            rgTipo.SelectedIndex = -1;
            bCarrega = true;
            bllMarcacao = new BLL.Marcacao();
            bllParametro = new BLL.Parametros();
            bllCartaoPonto = new BLL.CartaoPonto();
            bllInconsistencia = new BLL.Inconsistencia();
        }

        public Inconsistencias(DateTime dataInicial, DateTime dataFinal, Modelo.Funcionario pFuncionario)
        {
            InitializeComponent();
            txtPeriodoI.EditValue = dataInicial;
            txtPeriodoF.EditValue = dataFinal;
            objFuncionario = pFuncionario;
            bCarrega = false;
        }

        private void SelecionarFuncionario()
        {
            rgTipo.SelectedIndex = 2;
            SelecionaRegistroPorID("id", objFuncionario.Idempresa, gvEmpresas);
            SelecionaRegistroPorID("id", objFuncionario.Iddepartamento, gvDepartamentos);
            SelecionaRegistroPorID("id", objFuncionario.Id, gvFuncionarios);
        }

        protected override void FormBase_Load(object sender, System.EventArgs e)
        {
            base.FormBase_Load(sender, e);
            CarregaInconsistencias();
            setaNomeArquivo(this.Name);
            if (bCarrega)
            {
                Carrega();
                LeXML();
            }
            else
            {
                SelecionarFuncionario();
            }
        }

        private void CarregaInconsistencias()
        {
            IList<Modelo.Inconsistencia> inconsistencias = new List<Modelo.Inconsistencia>();
            inconsistencias = bllInconsistencia.GetAllList();

            gcInconsistencias.DataSource = inconsistencias;
        }

        protected override void btOk_Click(object sender, EventArgs e)
        {
            btOk.Enabled = false;
            btCancelar.Enabled = false;

            FormProgressBar2 pb = new FormProgressBar2();
            try
            {
                if (txtPeriodoI.DateTime > txtPeriodoF.DateTime)
                {
                    MessageBox.Show("Data inicial não pode ser maior que Data final.");
                }
                else
                {
                    if (ValidaCampos())
                    {
                        pb.Show(this);
                        pb.SetaMensagem("Carregando dados...");

                        ValidaSelectInconsistencias();

                        base.btOk_Click(sender, e);
                        Dt = bllCartaoPonto.GetCartaoPontoRel(txtPeriodoI.DateTime, txtPeriodoF.DateTime, MontaStringEmpresas(), MontaStringDepartamentos(), MontaStringFuncionarios(), MontaIntTipo(), 0, 0, pb.ObjProgressBar, false, "");

                        if (Dt.Rows.Count > 0)
                        {

                            DataTable DtIteracao;
                            bool bLimMaxHorasTrab;
                            bool bLimMinHorasAlmoco;

                            ValidaSelecaoInconsistencias(out bLimMaxHorasTrab, out bLimMinHorasAlmoco);
                            try
                            {
                                DtIteracao = Dt.AsEnumerable().Where(s => (VerificaLimiteCargaHoraria(s, bLimMaxHorasTrab)) ||
                                    (VerificaLimiteMinAlmoco(s, bLimMinHorasAlmoco))).CopyToDataTable();
                            }
                            catch (Exception)
                            {
                                DtIteracao = new DataTable();
                            }

                            Dt = DtIteracao;
                        }


                        pb.Dispose();

                        Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

                        parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();

                        Microsoft.Reporting.WinForms.ReportParameter p1 =
                            new Microsoft.Reporting.WinForms.ReportParameter("datainicial",
                                txtPeriodoI.DateTime.ToShortDateString());
                        parametros.Add(p1);

                        Microsoft.Reporting.WinForms.ReportParameter p2 =
                            new Microsoft.Reporting.WinForms.ReportParameter("datafinal",
                                txtPeriodoF.DateTime.ToShortDateString());
                        parametros.Add(p2);

                        Microsoft.Reporting.WinForms.ReportParameter p3 =
                            new Microsoft.Reporting.WinForms.ReportParameter("observacao",
                                objParametro.ImprimeObservacao == 1 ? objParametro.CampoObservacao : "");
                        parametros.Add(p3);

                        Microsoft.Reporting.WinForms.ReportParameter p4 =
                            new Microsoft.Reporting.WinForms.ReportParameter("responsavel", objParametro.ImprimeResponsavel.ToString());
                        parametros.Add(p4);

                        nomerel = "rptInconsistencias.rdlc";
                        ds = "dsCartaoPonto_DataTable1";

                        UI.Relatorios.Base.FormRelatorioBase form =
                            new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                        form.Text = "Relatório de Inconsistências";
                        form.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Preencha os campos corretamente.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                pb.Dispose();
                btOk.Enabled = true;
                btCancelar.Enabled = true;
            }
        }

        private void ValidaSelecaoInconsistencias(out bool bLimMaxHorasTrab, out bool bLimMinHorasAlmoco)
        {
            Modelo.Inconsistencia inconsistencia;
            bLimMaxHorasTrab = bLimMinHorasAlmoco = false;

            foreach (int indice in gvInconsistencias.GetSelectedRows())
            {
                inconsistencia = (Modelo.Inconsistencia)gvInconsistencias.GetRow(indice);
                
                if (inconsistencia.Legenda.Equals("LMT"))
                    bLimMaxHorasTrab = true;
                if (inconsistencia.Legenda.Equals("LHA"))
                    bLimMinHorasAlmoco = true;
            }
        }

        private IList<Modelo.Inconsistencia> PegaInconsistenciasSelecionadas(ref bool bLimMaxHorTrab, ref bool bLimMinHorAlmoco)
        {
            IList<Modelo.Inconsistencia> listaInconsistenciasSelecionadas = new List<Modelo.Inconsistencia>();
            foreach (int item in gvInconsistencias.GetSelectedRows())
            {
                Modelo.Inconsistencia objSelecionado = (Modelo.Inconsistencia)gvInconsistencias.GetRow(item);
                if (objSelecionado.Legenda == "LMT")
                {
                    bLimMaxHorTrab = true;
                }
                else if (objSelecionado.Legenda == "LHA")
                {
                    bLimMinHorAlmoco = true;
                }
                listaInconsistenciasSelecionadas.Add(objSelecionado);
            }

            return listaInconsistenciasSelecionadas;
        }

        private static DataColumn CriaColunaObservacao(String nomeColuna, String valorColuna)
        {
            DataColumn coluna = new DataColumn(nomeColuna);
            coluna.DataType = typeof(String);
            coluna.DefaultValue = valorColuna;
            return coluna;
        }

        private void ValidaSelectInconsistencias()
        {
            listaRowHandleInconsistencias.Clear();
            if (gvInconsistencias.GroupCount == 0)
            {
                for (int y = 0; y < gvInconsistencias.SelectedRowsCount; y++)
                {
                    if (gvInconsistencias.GetSelectedRows()[y] >= 0)
                    {
                        listaRowHandleInconsistencias.Add(gvInconsistencias.GetSelectedRows()[y]);
                    }
                }
            }
        }

        private static bool VerificaLimiteMinAlmoco(DataRow s, bool bValidaLimite)
        {
            bool retorno = false;
            if (bValidaLimite)
            {
                string limiteminimohorasalmoco = s.Field<String>("limiteminimohorasalmoco");
                string totalHorasTrabalhadas = s.Field<String>("TotalHorasTrabalhadas");
                string totalHorasAlmoco = s.Field<String>("TotalHorasAlmoco");

                retorno = ((cwkFuncoes.ConvertHorasMinuto(limiteminimohorasalmoco) > 0) &&
                    (cwkFuncoes.ConvertHorasMinuto(totalHorasTrabalhadas) > 0) &&
                    (cwkFuncoes.ConvertHorasMinuto(totalHorasAlmoco) < cwkFuncoes.ConvertHorasMinuto(limiteminimohorasalmoco)));
            }
            return retorno;
        }

        private static bool VerificaLimiteCargaHoraria(DataRow s, bool bValidaLimite)
        {
            bool retorno = false;
            if (bValidaLimite)
            {
                string totalHorasTrabalhadas = s.Field<String>("TotalHorasTrabalhadas");
                string limitehorastrabalhadasdia = s.Field<String>("limitehorastrabalhadasdia");

                retorno = ((cwkFuncoes.ConvertHorasMinuto(totalHorasTrabalhadas) > 0) &&
                    (cwkFuncoes.ConvertHorasMinuto(limitehorastrabalhadasdia) > 0) &&
                    (cwkFuncoes.ConvertHorasMinuto(totalHorasTrabalhadas) > cwkFuncoes.ConvertHorasMinuto(limitehorastrabalhadasdia)));
            }
            return retorno;
        }


        public DataTable ConvertListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        public bool ValidaCampos()
        {
            bool ret = true;

            if (txtPeriodoI.DateTime == new DateTime() || txtPeriodoI.DateTime == null)
            {
                dxErrorProvider1.SetError(txtPeriodoI, "Campo obrigatório.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(txtPeriodoI, "");
            }

            if (txtPeriodoF.DateTime == new DateTime() || txtPeriodoF.DateTime == null)
            {
                dxErrorProvider1.SetError(txtPeriodoF, "Campo obrigatório.");
                ret = false;
            }
            else
            {
                dxErrorProvider1.SetError(txtPeriodoF, "");
            }

            if (!((gvInconsistencias.GetSelectedRows() != null) && (gvInconsistencias.GetSelectedRows().Length > 0)))
            {
                MessageBox.Show("É obrigatório selecionar pelo menos uma inconsistência.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ret = false;

            }

            return ret;
        }

        private void rgTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TipoAlterado();
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
                    XmlNode xmlInconsistencias = filtros.SelectSingleNode("inconsistencias");
                    if (xmlInconsistencias != null)
                    {
                        listaRowHandleInconsistencias.Clear();
                        gvInconsistencias.ClearSelection();
                        foreach (XmlNode item in xmlInconsistencias.ChildNodes)
                        {
                            int aux = Convert.ToInt32(item.InnerText);
                            listaRowHandleInconsistencias.Add(aux);
                            gvInconsistencias.FocusedRowHandle = aux;
                            gvInconsistencias.SelectRow(gvInconsistencias.FocusedRowHandle);
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
            XmlElement inconsistencias = null;

            XmlElement ler = documentoXml.CreateElement("ler");
            ler.InnerText = gravar.ToString();
            noPai.AppendChild(ler);
            if (gravar)
            {
                XmlElement xmlTitulo = documentoXml.CreateElement("titulo");
                xmlTitulo.InnerText = Titulo;
                noPai.AppendChild(xmlTitulo);

                element = documentoXml.CreateElement("filtros");

                if (listaRowHandleInconsistencias.Count > 0)
                {
                    inconsistencias = documentoXml.CreateElement("inconsistencias");

                    foreach (int f in listaRowHandleInconsistencias)
                    {
                        XmlElement inconsistencia = documentoXml.CreateElement("inconsistencias");
                        inconsistencia.InnerText = f.ToString();
                        inconsistencias.AppendChild(inconsistencia);
                    }

                    element.AppendChild(inconsistencias);
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

        private void sbSelecionarInconsistencias_Click(object sender, EventArgs e)
        {
            gvInconsistencias.SelectAll();
        }

        private void sbLimparInconsistencias_Click(object sender, EventArgs e)
        {
            gvInconsistencias.ClearSelection();
        }
    }
}
