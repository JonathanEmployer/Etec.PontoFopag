using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using System.Reflection;
//using System.Data.SqlClient;

namespace UI.Base
{
    public partial class GridBase : Form
    {
        #region Atributos
        private string _filtro;
        private string _tabela;
        private bool _selecionar;
        private int _retorno;
        private List<int> _listaretorno;
        private int? _id;
        private int _localiza;
        private bool _atualiza;
        private List<string> _telasAbertas;
        private string _topichelp;
        Bitmap groupPanelImage;
        #endregion

        #region Propriedades
        public string cwFiltro
        {
            get { return _filtro; }
            set { _filtro = value; }
        }
        public string cwTabela
        {
            get { return _tabela; }
            set { _tabela = value; }
        }
        public bool cwSelecionar
        {
            get { return _selecionar; }
            set { _selecionar = value; }
        }
        public int cwRetorno
        {
            get { return _retorno; }
            set { _retorno = value; }
        }
        public List<int> cwListaRetorno
        {
            get { return _listaretorno; }
            set { _listaretorno = value; }
        }
        public int cwLocaliza
        {
            get { return _localiza; }
            set { _localiza = value; }
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
        public List<string> TelasAbertas
        {
            get { return _telasAbertas; }
            set { _telasAbertas = value; }
        }
        public string TopicHelp
        {
            get { return _topichelp; }
            set { _topichelp = value; }
        }
        #endregion

        public GridBase()
        {
            InitializeComponent();
            //this.Name = "GridBase";
            groupPanelImage = (Bitmap)UI.Properties.Resources.HeaderGrid;
            groupPanelImage.MakeTransparent();
            this.Text = RetornaNomeTela();
        }

        #region Eventos Privados
        private void GridBase_Load(object sender, EventArgs e)
        {
            this.Text = RetornaNomeTela();

            CarregaGrid(cwFiltro);

            sbSelecionar.Visible = cwSelecionar;
        }

        private void txtLocalizar_TextChanged(object sender, EventArgs e)
        {
            SelecionaRegistro();
        }

        private void sbFechar_Click(object sender, EventArgs e)
        {
            cwRetorno = 0;
            this.Close();
        }
        
        private void sbConsultar_Click(object sender, EventArgs e)
        {
            CarregarManutencao(Modelo.Acao.Consultar, RegistroSelecionado());
        }

        protected virtual void sbIncluir_Click(object sender, EventArgs e)
        {
            CarregarManutencao(Modelo.Acao.Incluir, 0);
        }

        private void sbAlterar_Click(object sender, EventArgs e)
        {
            CarregarManutencao(Modelo.Acao.Alterar, RegistroSelecionado());
        }

        private void sbExcluir_Click(object sender, EventArgs e)
        {
            CarregarManutencao(Modelo.Acao.Excluir, RegistroSelecionado());
        }

        private void sbSelecionar_Click(object sender, EventArgs e)
        {
            ValidaSelectManutencao();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            ValidaSelectManutencao();
        }

        private void GridBase_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    cwRetorno = 0;
                    this.Close();
                    break;
                case Keys.F:
                    if (Form.ModifierKeys == Keys.Control)
                    {

                    }
                    break;
                case Keys.L:
                    if (Form.ModifierKeys == Keys.Control)
                    {
                        CarregaGrid("");
                        OrdenaGrid(dataGridView1.SortedColumns[0].FieldName, DevExpress.Data.ColumnSortOrder.Ascending);
                    }
                    break;
                case Keys.F12:
                    if (Form.ModifierKeys == Keys.Control)
                        cwkControleUsuario.Facade.ChamaControleAcesso(Form.ModifierKeys, this, cwTabela);
                    break;
                case Keys.F1:
                    sbAjudar_Click(sender, e);
                    break;
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    ValidaSelectManutencao();
                    break;
                case Keys.Back:
                    if (txtLocalizar.Text.Length > 0)
                    {
                        txtLocalizar.Text = txtLocalizar.Text.Substring(0, (txtLocalizar.Text.TrimEnd().Length - 1));
                    }
                    SelecionaRegistro();
                    break;
            }
        }

        private void ValidaSelectManutencao()
        {
            if (cwSelecionar == false)
            {
                if (sbAlterar.Enabled == true)
                {
                    CarregarManutencao(Modelo.Acao.Alterar, RegistroSelecionado());
                }
            }
            else
            {
                if (dataGridView1.OptionsSelection.MultiSelect == true)
                {
                    cwListaRetorno.Clear();
                    if (dataGridView1.GroupCount == 0)
                    {
                        for (int y = 0; y < dataGridView1.SelectedRowsCount; y++)
                        {
                            if (dataGridView1.GetSelectedRows()[y] >= 0)
                            {
                                cwListaRetorno.Add(int.Parse(dataGridView1.GetRowCellValue(dataGridView1.GetSelectedRows()[y], "id").ToString()));
                            }
                        }
                    }
                }
                else
                {
                    cwRetorno = RegistroSelecionado();
                }
                this.Close();
            }
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsControl(e.KeyChar))
            {
                txtLocalizar.Text = txtLocalizar.Text + e.KeyChar;
                SelecionaRegistro();
            }
        }

        private void txtLocalizar_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    ValidaSelectManutencao();
                    break;
            }
        }

        private void GridBase_Shown(object sender, EventArgs e)
        {
            if (cwSelecionar && this.cwId.HasValue && this.cwId != 0)
            {
                cwAtualiza = false;
                string aux = "id";
                SelecionaRegistroPorID(aux, (int)cwId);
            }
            else
            {
                dataGridView1.SelectRow(0);
                dataGridView1.FocusedRowHandle = 0;
            }
            PersonalizaGrid();
        }
        #endregion

        #region Métodos Concretos
        /// <summary>
        /// Método responsável por ordenar o grid de acordo com o nome da coluna
        /// passado como parâmetro em pSort.
        /// </summary>
        /// <param name="pSort">Nome da coluna sobre a qual será realizada a ordenação</param>
        /// <param name="sortOrder">Ordem em que será ordenado</param>
        public void OrdenaGrid(string pSort, DevExpress.Data.ColumnSortOrder sortOrder)
        {
            dataGridView1.SortInfo.Clear();
            dataGridView1.SortInfo.ClearSorting();
            dataGridView1.Columns[pSort].SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            dataGridView1.Columns[pSort].SortOrder = sortOrder;
        }

        public void SelecionaRegistroPorID(string col, int ID)
        {
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
                dataGridView1.ClearSelection();
                dataGridView1.SelectRow(0);
                dataGridView1.FocusedRowHandle = 0;
            }
        }

        public void SelecionaRegistroPorPos(int posicao)
        {
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
                dataGridView1.ClearSelection();
                dataGridView1.SelectRow(0);
                dataGridView1.FocusedRowHandle = 0;
            }
        }

        private void SelecionaRegistro()
        {
            if (dataGridView1.GroupCount == 0)
            {
                for (int y = 0; y < dataGridView1.RowCount; y++)
                {
                    if (dataGridView1.GetRowCellValue(y, dataGridView1.SortedColumns[0]).ToString().ToLower().IndexOf(txtLocalizar.Text.ToLower()) == 0)
                    {
                        dataGridView1.FocusedRowHandle = y;
                        dataGridView1.SelectRow(dataGridView1.FocusedRowHandle);
                        break;
                    }
                }
            }
        }

        protected Int32 RegistroSelecionado()
        {
            Int32 id;
            try
            {
                id = (int)dataGridView1.GetFocusedRowCellValue("id");
            }
            catch (Exception)
            {
                id = 0;
            }
            return id;
        }

        protected void CarregarManutencao(Modelo.Acao pAcao, int pID)
        {
            try
            {
                if ((pAcao != Modelo.Acao.Incluir) && (pID == 0))
                {
                    MessageBox.Show("Nenhum registro selecionado.");
                }
                else
                {
                    CarregaFormulario(pAcao, pID);
                }

                switch (pAcao)
                {
                    case Modelo.Acao.Incluir:
                        cwAtualiza = true;
                        CarregaGrid("");
                        SelecionaRegistroPorPos(dataGridView1.RowCount - 1);
                        break;
                    case Modelo.Acao.Consultar:
                        break;
                    default:
                        cwAtualiza = true;
                        int pos = dataGridView1.FocusedRowHandle;
                        CarregaGrid("");
                        if (!(dataGridView1.GetFocusedRow() == null || dataGridView1.FocusedRowHandle < 0))
                        {
                            SelecionaRegistroPorPos(pos);
                        }
                        break;
                }
            }
            //catch (SqlException ex)
            //{
            //    //MessageBox.Show(Modelo.MetodosEstaticos.SqlExcecao(ex), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //    MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Hand);

            }
        }
        #endregion

        #region Métodos Virtuais
        /// <summary>
        /// Método responsável por alterar as propriedades do grid de acordo
        /// com as necessidades
        /// </summary>
        protected virtual void PersonalizaGrid()
        {
            //dataGridView1.Columns["id"].Visible = false;
        }

        /// <summary>
        /// Método responsável por alimentar o DataGrid. Deve ser implementado, no final chamar o método OrdenaGrid.
        /// </summary>
        /// <param name="pFiltro">Filtro utilizado na pesquisa</param>
        protected virtual void CarregaGrid(string pFiltro)
        {
        }

        /// <summary>
        /// Método responsável por carregar uma tela de manutenção
        /// </summary>
        /// <param name="pAcao">Ação que será executada na tela</param>
        /// <param name="pID">ID do registro que será utilizado na tela</param>
        protected virtual void CarregaFormulario(Modelo.Acao pAcao, int pID)
        {
        }

        #endregion

        public virtual void dataGridView1_CustomRowFilter(object sender, DevExpress.XtraGrid.Views.Base.RowFilterEventArgs e)
        {

        }

        public virtual void GridBase_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (TelasAbertas != null)
            {
                TelasAbertas.Remove(this.Name);
            }            
        }

        private void GridBase_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void sbAjudar_Click(object sender, EventArgs e)
        {
            ChamaHelp();
        }

        protected virtual void ChamaHelp()
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, this.Name.ToLower() + ".htm");
        }

        private void dataGridView1_CustomDrawGroupPanel(object sender, CustomDrawEventArgs e)
        {
            Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.LightBlue, Color.WhiteSmoke,
              System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
            e.Graphics.FillRectangle(brush, e.Bounds);
            Image img = groupPanelImage;
            Rectangle r = new Rectangle(e.Bounds.X + e.Bounds.Width - img.Size.Width - 5,
              e.Bounds.Y + (e.Bounds.Height - img.Size.Height) / 2, img.Width, img.Height);
            e.Graphics.DrawImageUnscaled(img, r);
            e.Handled = true;
        }

        private string RetornaNomeTela()
        {
            return "Tabela de " + this.cwTabela;
        }
    }
}
