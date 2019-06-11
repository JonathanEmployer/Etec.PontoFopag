using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;

namespace UI
{
    public partial class FormManutDiaria : Form
    {
        #region Atributos
        private string _tabela;
        private int? _id;
        private bool _atualiza;
        #endregion

        #region Propriedades
        public string cwTabela
        {
            get { return _tabela; }
            set { _tabela = value; }
        }
        public bool cwAtualiza
        {
            get { return _atualiza; }
            set { _atualiza = value; }
        }
        public int? cwId
        {
            get { return _id; }
            set { _id = value; }
        }

        public List<string> TelasAbertas { get; set; }
        #endregion


        private DataTable Departamentos;

        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcionario bllFuncionario;
        private BLL.Marcacao bllMarcacao;
        private BLL.ConfiguracoesGerais bllConfiguracoesGerais;
        private BLL.CartaoPonto bllCartaoPonto;
        private BLL.Parametros bllParametro;
        private BLL.Horario bllHorario;
        private DateTime dataIniAnt, dataFinAnt;
        private Hashtable empresasDataCarregadas = new Hashtable();
        private Hashtable departamentosDataCarregadas = new Hashtable();

        protected List<Modelo.Funcionario> listaFuncionarios = new List<Modelo.Funcionario>();
        protected List<int> listaRowHandleFuncionario = new List<int>();

        Modelo.Departamento objDepartamento;
        Modelo.Funcionario objFuncionario;

        public FormManutDiaria()
        {
            InitializeComponent();
            this.Name = "FormManutDiaria";
            txtDataFinal.EditValue = null;
            txtDataInicial.EditValue = null;
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllFuncionario = new BLL.Funcionario();
            bllMarcacao = new BLL.Marcacao();
            bllConfiguracoesGerais = new BLL.ConfiguracoesGerais();
            bllCartaoPonto = new BLL.CartaoPonto();
            bllParametro = new BLL.Parametros();
            bllHorario = new BLL.Horario();
        }

        #region Métodos Principais

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

        protected void CarregaGrid()
        {
            if (txtDataFinal.EditValue != null)
            {
                int idEmp = (int)cbIdEmpresa.EditValue;
                int idDep = (int)cbIdDepartamento.EditValue;
                bool aux = false;
                //Verifica se aquela data para aquela empresa ou departamento ja foi carregada
                aux = (empresasDataCarregadas.ContainsKey(idEmp.ToString() + txtDataFinal.DateTime.ToString()) || empresasDataCarregadas.ContainsKey("0" + txtDataFinal.DateTime.ToString()));
                if (idDep > 0)
                {
                    aux = aux || (departamentosDataCarregadas.ContainsKey(idDep.ToString() + txtDataFinal.DateTime.ToString()));
                }                
                if (!aux)
                {
                    CarregaGridIncluindoMarcacoes(idEmp, idDep);
                }
                else
                {
                    PegaMarcacoesDoBanco(idEmp, idDep);
                }
            }
        }

        private void CarregaGridIncluindoMarcacoes(int idEmp, int idDep)
        {
            FormProgressBar2 progressBar1 = new FormProgressBar2();

            bllMarcacao.ObjProgressBar = progressBar1.ObjProgressBar;
            progressBar1.Show();
            Application.DoEvents();
            this.Enabled = false;
            this.Focus();

            if (idEmp == 0)//Todos
            {
                gcMarcacoes.DataSource = bllMarcacao.GetPorDataManutDiaria(txtDataInicial.DateTime, txtDataFinal.DateTime, false, true);
            }
            else if (idDep > 0)//Departamento
            {
                gcMarcacoes.DataSource = bllMarcacao.GetPorManutDiariaDep((int)cbIdDepartamento.EditValue, txtDataInicial.DateTime, txtDataFinal.DateTime, false, true);
            }
            else if (idDep == 0 && idEmp > 0)//Por empresa
            {
                gcMarcacoes.DataSource = bllMarcacao.GetPorManutDiariaEmp(idEmp, txtDataInicial.DateTime, txtDataFinal.DateTime, false, true);
            }
            else
            {
                this.Enabled = false;
                gcMarcacoes.DataSource = bllMarcacao.GetPorManutDiariaEmp(idEmp, txtDataInicial.DateTime, txtDataFinal.DateTime, false, true);
            }
            this.Enabled = true;
            progressBar1.Close();
            Application.DoEvents();
            //Adiciona um registro na lista dizendo que as marcações da empresa selecionada para a data selecionada ja foram carregadas
            //WNO - 20/05/2010   
            if (idDep > 0)
                departamentosDataCarregadas.Add(idDep.ToString() + txtDataFinal.DateTime.ToString(), null);
            else
                empresasDataCarregadas.Add(idEmp.ToString() + txtDataFinal.DateTime.ToString(), null);
        }

        protected void PegaMarcacoesDoBanco(int idEmp, int idDep)
        {
            FormProgressBar2 progressBar1 = new FormProgressBar2();
            bllMarcacao.ObjProgressBar = progressBar1.ObjProgressBar;
            Application.DoEvents();
            this.Enabled = false;
            this.Focus();
            if (idEmp == 0)//Todos
            {
                gcMarcacoes.DataSource = bllMarcacao.GetPorDataManutDiaria(txtDataInicial.DateTime, txtDataFinal.DateTime, false, false);
            }
            else if (idDep > 0)//Departamento
            {
                gcMarcacoes.DataSource = bllMarcacao.GetPorManutDiariaDep(idDep, txtDataInicial.DateTime, txtDataFinal.DateTime, false, false);
            }
            else if (idDep == 0 && idEmp > 0)//Por empresa
            {
                gcMarcacoes.DataSource = bllMarcacao.GetPorManutDiariaEmp(idEmp, txtDataInicial.DateTime, txtDataFinal.DateTime, false, false);
            }
            else
            {
                this.Enabled = false;
                gcMarcacoes.DataSource = bllMarcacao.GetPorManutDiariaEmp(idEmp, txtDataInicial.DateTime, txtDataFinal.DateTime, false, false);
            }
            this.Enabled = true;
            progressBar1.Close();
            Application.DoEvents();
        }

        #endregion

        #region Eventos Secundarios

        private void FormTabelaMarcacoes_Load(object sender, EventArgs e)
        {
            this.Text = "Tabela de Manutenção Diária";
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAllComOpcaoTodos();
            cbIdDepartamento.Properties.DataSource = bllDepartamento.GetAllComOpcaoTodos();
            cbIdDepartamento.Enabled = false;
            sbIdDepartamento.Enabled = false;
            LoadDepartamentos();
        }

        private void gvMarcacoes_DoubleClick(object sender, EventArgs e)
        {
            ValidaSelectManutencao();
        }

        private void FormTabelaMarcacoes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void gvMarcacoes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    ValidaSelectManutencao();
                    break;
            }
        }

        #endregion

        #region Métodos Auxiliares

        private void ValidaSelectManutencao()
        {
            CarregarManutencao(Modelo.Acao.Alterar, RegistroSelecionado());
        }

        public void OrdenaGrid(string pSort, DevExpress.Data.ColumnSortOrder sortOrder)
        {
            gvMarcacoes.SortInfo.Clear();
            gvMarcacoes.SortInfo.ClearSorting();
            gvMarcacoes.Columns[pSort].SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            gvMarcacoes.Columns[pSort].SortOrder = sortOrder;

        }

        public void SelecionaRegistroPorID(string col, int ID)
        {
            int posicao = gvMarcacoes.LocateByDisplayText(0, gvMarcacoes.Columns.ColumnByFieldName(col), ID.ToString());
            if (posicao >= 0)
            {
                if (posicao > gvMarcacoes.RowCount - 1)
                {
                    posicao = gvMarcacoes.RowCount - 1;
                }
                gvMarcacoes.FocusedRowHandle = posicao;
                gvMarcacoes.SelectRow(posicao);
            }
            else
            {
                gvMarcacoes.ClearSelection();
                gvMarcacoes.SelectRow(0);
                gvMarcacoes.FocusedRowHandle = 0;
            }
        }

        public void SelecionaRegistroPorPos(int posicao)
        {
            if (posicao >= 0)
            {
                if (posicao > gvMarcacoes.RowCount - 1)
                {
                    posicao = gvMarcacoes.RowCount - 1;
                }
                gvMarcacoes.FocusedRowHandle = posicao;
                gvMarcacoes.SelectRow(posicao);
            }
            else
            {
                gvMarcacoes.ClearSelection();
                gvMarcacoes.SelectRow(0);
                gvMarcacoes.FocusedRowHandle = 0;
            }
        }

        protected Int32 RegistroSelecionado()
        {
            Int32 id;
            try
            {
                id = (int)gvMarcacoes.GetFocusedRowCellValue("Id");
            }
            catch (Exception)
            {
                id = 0;
            }
            return id;
        }

        protected Int32 FuncionarioSelecionado()
        {
            Int32 id;
            try
            {
                id = (int)gvMarcacoes.GetFocusedRowCellValue("IdFuncionario");
            }
            catch (Exception)
            {
                id = 0;
            }
            return id;
        }

        private void CarregarManutencao(Modelo.Acao pAcao, int pID)
        {
            if (sbManutMarcacao.Enabled)
            {
                try
                {
                    if (pID == 0)
                    {
                        MessageBox.Show("Nenhum registro selecionado.");
                    }
                    else
                    {
                        CarregaFormulario(pAcao, pID);
                    }

                    cwAtualiza = true;
                    int pos = gvMarcacoes.FocusedRowHandle;
                    CarregaGrid();
                    if (!(gvMarcacoes.GetFocusedRow() == null || gvMarcacoes.FocusedRowHandle < 0))
                    {
                        SelecionaRegistroPorPos(pos);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);

                }
            }
        }

        /// <summary>
        /// Método responsável por carregar uma tela de manutenção
        /// </summary>
        /// <param name="pAcao">Ação que será executada na tela</param>
        /// <param name="pID">ID do registro que será utilizado na tela</param>
        protected virtual void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
            UI.FormManutMarcacao form = new FormManutMarcacao();
            form.cwAcao = Modelo.Acao.Alterar;
            form.cwID = pID;
            form.cwTabela = "Marcação";
            form.ShowDialog();
        }

        #endregion


        protected virtual void GridSelecao<T>(Base.GridBase pGrid, Componentes.devexpress.cwk_DevLookup pCb, BLL.IBLL<T> bll)
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

        private void sbFechar_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void cbIdDepartamento_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)cbIdDepartamento.EditValue > 0)
            {
                objDepartamento = bllDepartamento.LoadObject((int)cbIdDepartamento.EditValue);
            }
            CarregaGrid();
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
            CarregaGrid();
        }



        private void sbManutMarcacao_Click(object sender, EventArgs e)
        {
            CarregarManutencao(Modelo.Acao.Alterar, RegistroSelecionado());
        }

        private void sbCartaoPontoPeriodo_Click(object sender, EventArgs e)
        {
        }

        private void sbTurnoFuncionario_Click(object sender, EventArgs e)
        {
            if (FuncionarioSelecionado() >= 1)
            {
                objFuncionario = bllFuncionario.LoadObject(FuncionarioSelecionado());

                Modelo.Marcacao objMarcacao = new Modelo.Marcacao();
                objMarcacao = bllMarcacao.LoadObject(RegistroSelecionado());
                if (objMarcacao.Idhorario != 0)
                {
                    Modelo.Horario objHorario = bllHorario.LoadObject(objMarcacao.Idhorario);

                    Base.ManutBase form;
                    if (objHorario.TipoHorario == 1)
                    {
                        form = new FormManutHorario();
                        form.cwAcao = Modelo.Acao.Consultar;
                        form.cwID = objMarcacao.Idhorario;
                        form.cwTabela = "Horário Normal";
                        form.MdiParent = this.MdiParent;
                        form.Show();
                    }
                    else if (objHorario.TipoHorario == 2)
                    {
                        form = new FormManutHorarioMovel();
                        form.cwAcao = Modelo.Acao.Consultar;
                        form.cwID = objMarcacao.Idhorario;
                        form.cwTabela = "Horário Flexível";
                        form.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Não existe um horário cadastrado para esse funcionário.");

                }
            }
            else
            {
                MessageBox.Show("Selecione um funcionário!");
            }
        }

        private void sbTotalHorasFuncionario_Click(object sender, EventArgs e)
        {
            objFuncionario = bllFuncionario.LoadObject(FuncionarioSelecionado());
            if (FuncionarioSelecionado() >= 1)
            {
                if (txtDataFinal.DateTime != new DateTime())
                {
                    FormVisualizacaoResumoHoras form = new FormVisualizacaoResumoHoras(objFuncionario, txtDataInicial.DateTime, txtDataFinal.DateTime);
                    form.MdiParent = this.MdiParent;
                    form.Show();
                }
                else
                {
                    MessageBox.Show("Preencha a data.");
                }
            }
            else
            {
                MessageBox.Show("Selecione um funcionário.");
            }
        }

        private void sbRecalculaMarcacoes_Click(object sender, EventArgs e)
        {
            if (sbRecalculaMarcacoes.Enabled)
            {
                FormProgressBar2 formPBRecalcula = new FormProgressBar2();
                formPBRecalcula.Show();

                this.Enabled = false;

                if ((int)cbIdEmpresa.EditValue > 0)
                {
                    bllMarcacao.RecalculaMarcacao(0, (int)cbIdEmpresa.EditValue, txtDataInicial.DateTime, txtDataFinal.DateTime, formPBRecalcula.ObjProgressBar);
                }
                else if ((int)cbIdDepartamento.EditValue > 0)
                {
                    bllMarcacao.RecalculaMarcacao(1, (int)cbIdDepartamento.EditValue, txtDataInicial.DateTime, txtDataFinal.DateTime, formPBRecalcula.ObjProgressBar);
                }
                else
                {
                    bllMarcacao.RecalculaMarcacao(5, 0, txtDataInicial.DateTime, txtDataFinal.DateTime, formPBRecalcula.ObjProgressBar);
                }
                formPBRecalcula.Close();
                this.Enabled = true;
                CarregaGrid();
            }
        }

        private void sbManutBilhetes_Click(object sender, EventArgs e)
        {
            if (sbManutBilhetes.Enabled)
            {
                int id = RegistroSelecionado();
                if (id > 0)
                {
                    Modelo.Marcacao objMarcacao = bllMarcacao.LoadObject(id);
                    FormManutencaoBilhetes form = new FormManutencaoBilhetes(objMarcacao);
                    form.ShowDialog();
                    int pos = gvMarcacoes.FocusedRowHandle;
                    CarregaGrid();
                    if (!(gvMarcacoes.GetFocusedRow() == null || gvMarcacoes.FocusedRowHandle < 0))
                    {
                        SelecionaRegistroPorPos(pos);
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum registro selecionado.");
                }
            }
        }

        private void sbOrdenaHorarios_Click(object sender, EventArgs e)
        {
            if (sbOrdenaHorarios.Enabled)
            {
                FormProgressBar2 formPBRecalcula = new FormProgressBar2();
                int id = RegistroSelecionado();
                if (id > 0)
                {
                    Modelo.Marcacao objMarcacao = bllMarcacao.LoadObject(id);
                    bllMarcacao.ObjProgressBar = formPBRecalcula.ObjProgressBar;
                    bllMarcacao.OrdenaMarcacao(objMarcacao, true);
                    bllMarcacao.Salvar(Modelo.Acao.Alterar, objMarcacao);
                    CarregaGrid();
                    SelecionaRegistroPorID("Id", id);
                }
            }
        }

        private void FormManutDiaria_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    sbFechar_Click(sender, e);
                    break;
                case Keys.F5:
                    sbOrdenaHorarios_Click(sender, e);
                    break;
                case Keys.F10:
                    sbManutBilhetes_Click(sender, e);
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, cwTabela);
                    else
                        sbRecalculaMarcacoes_Click(sender, e);
                    break;
                case Keys.F1:
                    sbAjudar_Click(sender, e);
                    break;
            }
        }

        private void FormManutDiaria_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }



        private void sbRelatorioManutDiaria_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
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

                Dt = bllCartaoPonto.GetCartaoPontoDiaria(txtDataInicial.DateTime, txtDataFinal.DateTime, empresas, departamentos, tipo, pb.progressBar);
                pb.Dispose();
                nomerel = "rptManutencaoDiaria.rdlc";
                ds = "dsCartaoPonto_DataTable1";

                Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

                parametros = new List<Microsoft.Reporting.WinForms.ReportParameter>();
                Microsoft.Reporting.WinForms.ReportParameter p1 = new Microsoft.Reporting.WinForms.ReportParameter("dataInicial", txtDataInicial.DateTime.ToShortDateString());
                Microsoft.Reporting.WinForms.ReportParameter p2 = new Microsoft.Reporting.WinForms.ReportParameter("dataFinal", txtDataFinal.DateTime.ToShortDateString());
                parametros.Add(p1);
                parametros.Add(p2);
                UI.Relatorios.Base.FormRelatorioBase form = new UI.Relatorios.Base.FormRelatorioBase(nomerel, ds, Dt, parametros);
                form.Text = "Relatório de Manutenção Diária";
                form.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void FormManutDiaria_Shown(object sender, EventArgs e)
        {
            txtDataInicial.DateTime = DateTime.Today;
            txtDataFinal.DateTime = DateTime.Today;
            dataIniAnt = txtDataInicial.DateTime;
            dataFinAnt = txtDataFinal.DateTime;
            CarregaGrid();
        }

        private void txtDataFinal_Leave(object sender, EventArgs e)
        {
            if (dataFinAnt != txtDataFinal.DateTime || dataIniAnt != txtDataFinal.DateTime)
            {
                if (txtDataInicial.DateTime > txtDataFinal.DateTime)
                {
                    MessageBox.Show("Data Inicial Deve Ser Maior que a Data Final.");
                }
                else
                {
                    CarregaGrid();
                    dataFinAnt = txtDataFinal.DateTime;
                }
            }
        }

        private void sbAjudar_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }

        private void txtDataInicial_Leave(object sender, EventArgs e)
        {
            txtDataFinal_Leave(sender, e);
            dataIniAnt = txtDataInicial.DateTime;
        }
    }
}
