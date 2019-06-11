using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;


namespace UI
{
    public partial class FormTabelaMarcacoes : Form
    {
        #region Atributos
        private string _tabela;
        private int? _id;
        private bool _atualiza;

        private bool _bTelaCarregada;

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

        private bool bTelaCarregada
        {
            get { return _bTelaCarregada; }
            set { _bTelaCarregada = value; }
        }

        public List<string> TelasAbertas { get; set; }

        private int IdFuncionarioAnt { get; set; }

        private DateTime dataInicialAnt { get; set; }
        private DateTime dataFinalAnt { get; set; }

        #endregion

        private MenuInicial menuInicial = new MenuInicial();

        private List<Modelo.Funcionario> Funcionarios = new List<Modelo.Funcionario>();
        private BLL.Funcionario bllFuncionario;
        private BLL.Funcao bllFuncao;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Marcacao bllMarcacao;
        private BLL.ConfiguracoesGerais bllConfiguracoesGerais;
        private BLL.Horario bllHorario;

        private Modelo.Funcionario objFuncionario;
        private Modelo.Empresa objEmpresa;
        private Modelo.Funcao objFuncao;

        private PopupControl.Popup complex;
        private popup.PopUpOrdenaHorario complexPopup;

        public FormTabelaMarcacoes(MenuInicial pMenuInicial)
        {
            InitializeComponent();

            bllFuncionario = new BLL.Funcionario();
            bllFuncao = new BLL.Funcao();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllMarcacao = new BLL.Marcacao();
            bllHorario = new BLL.Horario();
            bllConfiguracoesGerais = new BLL.ConfiguracoesGerais();

            bTelaCarregada = false;
            this.Name = "FormTabelaMarcacoes";
            this.menuInicial = pMenuInicial;
            lblCodigo.Text = "";
            lblTipoTurno.Text = "";
            lblTurno.Text = "";
            lblFuncao.Text = "";
            IdFuncionarioAnt = 0;

            DateTime dataInicial;
            DateTime dataFinal;
            bool mudadataautomaticamente;

            bllConfiguracoesGerais.AtribuiDatas(Application.StartupPath, out dataInicial, out dataFinal, out mudadataautomaticamente);

            txtDataInicial.DateTime = dataInicial;
            txtDataFinal.DateTime = dataFinal;
            dataInicialAnt = dataInicial;
            dataFinalAnt = dataFinal;

            if (txtDataInicial.DateTime == new DateTime())
            {
                txtDataInicial.EditValue = null;
            }

            if (txtDataFinal.DateTime == new DateTime())
            {
                txtDataFinal.EditValue = null;
            }

            if (mudadataautomaticamente == true)
            {
                objFuncionario = bllFuncionario.LoadObject((int)cbIdFuncionario.EditValue);
                bTelaCarregada = true;
                CarregaGrid();
            }


            complex = new PopupControl.Popup(complexPopup = new popup.PopUpOrdenaHorario());
            complex.Resizable = true;
            complexPopup.sbHorario.Click += new EventHandler(sbHorario_Click);
            complexPopup.sbPeriodo.Click += new EventHandler(sbPeriodo_Click);

        }

        private void sbHorario_Click(object sender, EventArgs e)
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
            else
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
            complex.Close();
        }

        // Aqui vai o método que ordena os horarios por período
        private void sbPeriodo_Click(object sender, EventArgs e)
        {

            FormProgressBar2 formPBRecalcula = new FormProgressBar2();
            formPBRecalcula.Show();

            this.Enabled = false;
            complex.Close();
            bllMarcacao.ObjProgressBar = formPBRecalcula.ObjProgressBar;
            bllMarcacao.OrdenaTodasMarcacoes(Convert.ToDateTime(txtDataInicial.EditValue), Convert.ToDateTime(txtDataFinal.EditValue), (int)cbIdFuncionario.EditValue);
            CarregaGrid();

            formPBRecalcula.Close();

            this.Enabled = true;

        }

        #region Eventos Principais

        private void cbIdEmpresa_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)cbIdEmpresa.EditValue > 0)
            {
                cbIdDepartamento.Properties.DataSource = bllDepartamento.GetPorEmpresaComOpcaoTodos((int)cbIdEmpresa.EditValue);
                cbIdDepartamento.EditValue = 0;
                cbIdDepartamento.Enabled = true;
                sbIdDepartamento.Enabled = true;
            }
            else if ((int)cbIdEmpresa.EditValue == 0)
            {
                cbIdDepartamento.Properties.DataSource = bllDepartamento.GetAllComOpcaoTodos();
                cbIdDepartamento.EditValue = 0;
                cbIdDepartamento.Enabled = false;
                cbIdDepartamento.Enabled = false;
            }
            LoadFuncionarios();
        }

        private void cbIdDepartamento_EditValueChanged(object sender, EventArgs e)
        {
            LoadFuncionarios();
        }

        private void cbIdFuncionario_EditValueChanged(object sender, EventArgs e)
        {
            if ((int)cbIdFuncionario.EditValue > 0 && (int)cbIdFuncionario.EditValue != IdFuncionarioAnt)
            {
                dnFuncionario.Position = Funcionarios.FindIndex(f => f.Id == Convert.ToInt32(cbIdFuncionario.EditValue));
                objFuncionario = bllFuncionario.LoadObject((int)cbIdFuncionario.EditValue);
                CarregaGrid();
                lblCodigo.Text = objFuncionario.Dscodigo;
                lblTurno.Text = objFuncionario.Horario;
                objFuncao = bllFuncao.LoadObject(objFuncionario.Idfuncao);
                objEmpresa = bllEmpresa.LoadObject(objFuncionario.Idempresa);
                lblEmp1.Text = objEmpresa.Nome;
                lblFuncao.Text = objFuncao.Descricao;
                if (objFuncionario.Tipohorario == 1)
                {
                    lblTipoTurno.Text = "Horário Normal";
                }
                else if (objFuncionario.Tipohorario == 2)
                {
                    lblTipoTurno.Text = "Horário Móvel";
                }
            }
        }

        #endregion

        #region Métodos Principais

        private bool PeriodoValido()
        {
            if (txtDataInicial.DateTime > txtDataFinal.DateTime)
                return false;
            TimeSpan ts = txtDataFinal.DateTime - txtDataInicial.DateTime;
            if (ts.Days <= 30)
                return true;
            return false;
        }

        private void LoadFuncionarios()
        {
            if ((int)cbIdDepartamento.EditValue > 0 && (int)cbIdEmpresa.EditValue > 0)
            {
                Funcionarios = bllFuncionario.GetTabelaMarcacao(2, (int)cbIdDepartamento.EditValue,"");
            }
            else if ((int)cbIdEmpresa.EditValue == 0)
            {
                cbIdDepartamento.Enabled = false;
                sbIdDepartamento.Enabled = false;
                Funcionarios = bllFuncionario.GetTabelaMarcacao(3, 0,"");
            }
            else if ((int)cbIdDepartamento.EditValue == 0)
            {
                Funcionarios = bllFuncionario.GetTabelaMarcacao(1, (int)cbIdEmpresa.EditValue,"");
            }

            dnFuncionario.DataSource = Funcionarios;
            cbIdFuncionario.Properties.DataSource = Funcionarios;
            cbIdFuncionario.DataBindings.Clear();
            cbIdFuncionario.DataBindings.Add("EditValue", Funcionarios, "Id", true, DataSourceUpdateMode.Never);
            if (Funcionarios.Count > 0)
            {
                int idf = Convert.ToInt32(Funcionarios[0].Id);
                cbIdFuncionario.EditValue = idf;
                objFuncionario = bllFuncionario.LoadObject(idf);
            }
            else
            {
                cbIdFuncionario.EditValue = 0;
            }

            CarregaGrid();
        }

        /// <summary>
        /// Método responsável por alimentar o DataGrid. Deve ser implementado, no final chamar o método OrdenaGrid.
        /// </summary>
        /// <param name="pFiltro">Filtro utilizado na pesquisa</param>
        protected void CarregaGrid()
        {
            if (objFuncionario != null && bTelaCarregada)
            {
                objFuncao = bllFuncao.LoadObject(objFuncionario.Idfuncao);
                objEmpresa = bllEmpresa.LoadObject(objFuncionario.Idempresa);

                if (txtDataInicial.EditValue != null && txtDataFinal.EditValue != null && (int)cbIdFuncionario.EditValue > 0)
                {
                    if ((int)cbIdEmpresa.EditValue == 0)
                    {
                        lblFuncao.Text = objFuncao.Descricao;
                        lblEmp1.Text = objEmpresa.Nome;
                    }
                    if (PeriodoValido())
                        gcMarcacoes.DataSource = bllMarcacao.GetMarcacaoListaPorFuncionario((int)cbIdFuncionario.EditValue, txtDataInicial.DateTime, txtDataFinal.DateTime);
                    else
                    {
                        txtDataInicial.Focus();
                        gcMarcacoes.DataSource = null;
                    }
                }
                else
                {
                    gcMarcacoes.DataSource = null;
                    lblCodigo.Text = "";
                    lblTurno.Text = "";
                    lblTipoTurno.Text = "";
                    lblFuncao.Text = "";
                }
            }
        }

        #endregion

        #region Eventos Secundarios

        private void FormTabelaMarcacoes_Load(object sender, EventArgs e)
        {
            this.Text = "Tabela de " + this.cwTabela;
            cbIdEmpresa.Properties.DataSource = bllEmpresa.GetAllComOpcaoTodos();
            cbIdDepartamento.Properties.DataSource = bllDepartamento.GetAllComOpcaoTodos();
            cbIdDepartamento.Enabled = false;
            sbIdDepartamento.Enabled = false;

            LoadFuncionarios();
            bTelaCarregada = true;
        }

        private void sbFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvMarcacoes_DoubleClick(object sender, EventArgs e)
        {
            if (sbManutMarcacao.Enabled == true)
                ValidaSelectManutencao();
        }

        private void FormTabelaMarcacoes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.F5:
                    if (Form.ModifierKeys == Keys.Control)
                        sbPeriodo_Click(sender, e);
                    else
                        sbHorario_Click(sender, e);
                    break;
                case Keys.F10:
                    if (sbManutBilhetes.Enabled == true)
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

        private void gvMarcacoes_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (sbManutMarcacao.Enabled == true)
                        ValidaSelectManutencao();
                    break;
            }
        }

        private void gvMarcacoes_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        #endregion

        #region Métodos Auxiliares

        private void ValidaSelectManutencao()
        {
            CarregarManutencao(Modelo.Acao.Alterar, RegistroSelecionado());
        }

        /// <summary>
        /// Método responsável por ordenar o grid de acordo com o nome da coluna
        /// passado como parâmetro em pSort.
        /// </summary>
        /// <param name="pSort">Nome da coluna sobre a qual será realizada a ordenação</param>
        /// <param name="sortOrder">Ordem em que será ordenado</param>
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

        private void CarregarManutencao(Modelo.Acao pAcao, int pID)
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

        private void sbTurnoFuncionario_Click(object sender, EventArgs e)
        {
            if ((txtDataInicial.EditValue != null && txtDataInicial.DateTime != new DateTime()) &&
               (txtDataFinal.EditValue != null && txtDataFinal.DateTime != new DateTime()))
            {
                if ((int)cbIdFuncionario.EditValue > 0)
                {
                    objFuncionario = bllFuncionario.LoadObject((int)cbIdFuncionario.EditValue);

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
                            form.ShowDialog();
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
                        MessageBox.Show("Não existe um horário cadastrado para essa marcação.");

                    }
                }
            }
        }

        private void sbManutMarcacao_Click(object sender, EventArgs e)
        {
            CarregarManutencao(Modelo.Acao.Alterar, RegistroSelecionado());
        }

        private void txtDataFinal_Validating(object sender, CancelEventArgs e)
        {
            if (txtDataInicial.DateTime > txtDataFinal.DateTime && txtDataFinal.DateTime != new DateTime())
            {
                MessageBox.Show("A data final deve ser maior do que a data inicial.");
            }
            else
            {
                TimeSpan ts = txtDataFinal.DateTime - txtDataInicial.DateTime;
                if (ts.Days <= 30)
                {
                    if (txtDataInicial.DateTime != dataInicialAnt || txtDataFinal.DateTime != dataFinalAnt)
                    {
                        CarregaGrid();
                    }
                }
                else
                {
                    MessageBox.Show("O intervalo gerado deve ser de no máximo 31 dias.");
                }
            }
            dataInicialAnt = txtDataInicial.DateTime;
            dataFinalAnt = txtDataFinal.DateTime;
        }

        private void sbCartaoPontoPeriodo_Click(object sender, EventArgs e)
        {
            if ((int)cbIdFuncionario.EditValue > 0)
            {
                objFuncionario = bllFuncionario.LoadObject((int)cbIdFuncionario.EditValue);

                if (txtDataInicial.DateTime != new DateTime() && txtDataFinal.DateTime != new DateTime())
                {
                    UI.Relatorios.CartaoPonto.FormImprimeCartaoIndividual form = new UI.Relatorios.CartaoPonto.FormImprimeCartaoIndividual(txtDataInicial.DateTime, txtDataFinal.DateTime, objFuncionario);
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Preencha as datas inicial e final.");
                }
            }
            else
            {
                MessageBox.Show("Selecione um funcionário.");
            }

        }

        protected virtual void GridSelecao<T>(Base.GridBase pGrid, Componentes.devexpress.cwk_DevLookup pCb, BLL.IBLL<T> bll)
        {
            //pGrid.cwSelecionar = true;
            //pGrid.ShowDialog();
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

        private void sbIdFuncionario_Click(object sender, EventArgs e)
        {
            FormGridFuncionario form = new FormGridFuncionario();
            form.cwTabela = "Funcionário";
            form.cwId = (int)cbIdFuncionario.EditValue;


            UI.Util.Funcoes.ChamaGridSelecao(form);
            if (form.cwAtualiza == true)
            {
                LoadFuncionarios();
            }
            if (form.cwRetorno != 0)
            {
                cbIdFuncionario.EditValue = form.cwRetorno;
            }
            cbIdFuncionario.Focus();
        }

        private void sbTotalHorasFuncionario_Click(object sender, EventArgs e)
        {
            if ((int)cbIdFuncionario.EditValue > 0)
            {
                if (txtDataInicial.DateTime != new DateTime() && txtDataFinal.DateTime != new DateTime())
                {
                    FormVisualizacaoResumoHoras form = new FormVisualizacaoResumoHoras(objFuncionario, txtDataInicial.DateTime, txtDataFinal.DateTime);
                    form.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Preencha as datas inicial e final.");
                }
            }
            else
            {
                MessageBox.Show("Selecione um funcionário.");
            }
        }

        private void sbOrdenaHorarios_Click(object sender, EventArgs e)
        {
            complex.Show(sbOrdenaHorarios);
        }

        private void sbRecalculaMarcacoes_Click(object sender, EventArgs e)
        {
            if ((txtDataInicial.EditValue != null && txtDataInicial.DateTime != new DateTime()) &&
                (txtDataFinal.EditValue != null && txtDataFinal.DateTime != new DateTime()))
            {
                if (objFuncionario != null)
                {
                    FormProgressBar2 formPBRecalcula = new FormProgressBar2();
                    formPBRecalcula.Show();

                    this.Enabled = false;

                    //recalcula marcação para aquele funcionario
                    bllMarcacao.RecalculaMarcacao(2, objFuncionario.Id, txtDataInicial.DateTime, txtDataFinal.DateTime, formPBRecalcula.ObjProgressBar);

                    formPBRecalcula.Close();

                    this.Enabled = true;

                    CarregaGrid();
                }
            }
        }

        private void sbManutBilhetes_Click(object sender, EventArgs e)
        {
            int id = RegistroSelecionado();
            if (id > 0)
            {
                Modelo.Marcacao objMarcacao = bllMarcacao.LoadObject(id);
                FormManutencaoBilhetes form = new FormManutencaoBilhetes(objMarcacao);
                form.ShowDialog();
                CarregaGrid();
                SelecionaRegistroPorID("Id", id);
            }
            else
            {
                MessageBox.Show("Nenhum registro selecionado.");
            }
        }

        private void FormTabelaMarcacoes_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }
        }

        private void FormTabelaMarcacoes_Shown(object sender, EventArgs e)
        {
            if (txtDataInicial.EditValue != null && txtDataFinal.EditValue != null)
            {
                gvMarcacoes.Focus();
            }
            else
            {
                txtDataInicial.Focus();
            }
        }

        private void sbAjudar_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }
    }
}
