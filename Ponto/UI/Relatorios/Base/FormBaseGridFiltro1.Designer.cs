namespace UI.Relatorios.Base
{
    partial class FormBaseGridFiltro1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBaseGridFiltro1));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gcEmpresas = new DevExpress.XtraGrid.GridControl();
            this.gvEmpresas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.gcDepartamentos = new DevExpress.XtraGrid.GridControl();
            this.gvDepartamentos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColunaEmpresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.gcFuncionarios = new DevExpress.XtraGrid.GridControl();
            this.gvFuncionarios = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColunaIDFunc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.rgTipo = new DevExpress.XtraEditors.RadioGroup();
            this.sbSelecionarEmpresas = new DevExpress.XtraEditors.SimpleButton();
            this.sbLimparEmpresa = new DevExpress.XtraEditors.SimpleButton();
            this.sbLimparFuncionarios = new DevExpress.XtraEditors.SimpleButton();
            this.sbSelecionarFuncionarios = new DevExpress.XtraEditors.SimpleButton();
            this.sbLimparDepartamento = new DevExpress.XtraEditors.SimpleButton();
            this.sbSelecionarDepartamentos = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcEmpresas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEmpresas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDepartamentos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDepartamentos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcFuncionarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFuncionarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 555);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 551);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(678, 551);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(599, 551);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(741, 533);
            this.TabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.sbLimparDepartamento);
            this.tabPage1.Controls.Add(this.sbSelecionarDepartamentos);
            this.tabPage1.Controls.Add(this.sbLimparFuncionarios);
            this.tabPage1.Controls.Add(this.sbSelecionarFuncionarios);
            this.tabPage1.Controls.Add(this.sbLimparEmpresa);
            this.tabPage1.Controls.Add(this.sbSelecionarEmpresas);
            this.tabPage1.Controls.Add(this.groupControl1);
            this.tabPage1.Controls.Add(this.labelControl3);
            this.tabPage1.Controls.Add(this.gcFuncionarios);
            this.tabPage1.Controls.Add(this.labelControl2);
            this.tabPage1.Controls.Add(this.gcDepartamentos);
            this.tabPage1.Controls.Add(this.labelControl1);
            this.tabPage1.Controls.Add(this.gcEmpresas);
            this.tabPage1.Size = new System.Drawing.Size(735, 527);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Location = new System.Drawing.Point(6, 59);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 18);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Empresas";
            // 
            // gcEmpresas
            // 
            this.gcEmpresas.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcEmpresas.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcEmpresas.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcEmpresas.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcEmpresas.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcEmpresas.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcEmpresas.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcEmpresas.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcEmpresas.Location = new System.Drawing.Point(6, 83);
            this.gcEmpresas.MainView = this.gvEmpresas;
            this.gcEmpresas.Name = "gcEmpresas";
            this.gcEmpresas.Size = new System.Drawing.Size(357, 111);
            this.gcEmpresas.TabIndex = 6;
            this.gcEmpresas.UseEmbeddedNavigator = true;
            this.gcEmpresas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvEmpresas});
            // 
            // gvEmpresas
            // 
            this.gvEmpresas.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.Empty.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvEmpresas.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvEmpresas.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvEmpresas.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvEmpresas.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvEmpresas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvEmpresas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.OddRow.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.OddRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvEmpresas.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.Preview.Options.UseFont = true;
            this.gvEmpresas.Appearance.Preview.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.Row.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvEmpresas.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn5});
            this.gvEmpresas.GridControl = this.gcEmpresas;
            this.gvEmpresas.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvEmpresas.Name = "gvEmpresas";
            this.gvEmpresas.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvEmpresas.OptionsBehavior.Editable = false;
            this.gvEmpresas.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvEmpresas.OptionsView.ColumnAutoWidth = false;
            this.gvEmpresas.OptionsView.EnableAppearanceEvenRow = true;
            this.gvEmpresas.OptionsView.EnableAppearanceOddRow = true;
            this.gvEmpresas.OptionsView.ShowGroupPanel = false;
            this.gvEmpresas.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gvEmpresas_SelectionChanged);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "ID";
            this.gridColumn3.FieldName = "id";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Nome";
            this.gridColumn5.FieldName = "nome";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 500;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Location = new System.Drawing.Point(369, 59);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(114, 18);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "Departamentos";
            // 
            // gcDepartamentos
            // 
            this.gcDepartamentos.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gcDepartamentos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDepartamentos.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gcDepartamentos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDepartamentos.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gcDepartamentos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDepartamentos.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gcDepartamentos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDepartamentos.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcDepartamentos.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcDepartamentos.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcDepartamentos.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcDepartamentos.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcDepartamentos.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcDepartamentos.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcDepartamentos.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDepartamentos.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDepartamentos.Location = new System.Drawing.Point(369, 83);
            this.gcDepartamentos.MainView = this.gvDepartamentos;
            this.gcDepartamentos.Name = "gcDepartamentos";
            this.gcDepartamentos.Size = new System.Drawing.Size(357, 111);
            this.gcDepartamentos.TabIndex = 10;
            this.gcDepartamentos.UseEmbeddedNavigator = true;
            this.gcDepartamentos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDepartamentos});
            // 
            // gvDepartamentos
            // 
            this.gvDepartamentos.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvDepartamentos.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.Empty.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvDepartamentos.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvDepartamentos.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvDepartamentos.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvDepartamentos.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvDepartamentos.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDepartamentos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDepartamentos.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDepartamentos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvDepartamentos.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvDepartamentos.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvDepartamentos.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvDepartamentos.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDepartamentos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvDepartamentos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvDepartamentos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvDepartamentos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDepartamentos.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvDepartamentos.Appearance.OddRow.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.OddRow.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvDepartamentos.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.Preview.Options.UseFont = true;
            this.gvDepartamentos.Appearance.Preview.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDepartamentos.Appearance.Row.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvDepartamentos.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDepartamentos.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDepartamentos.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDepartamentos.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvDepartamentos.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvDepartamentos.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvDepartamentos.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvDepartamentos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.ColunaEmpresa});
            this.gvDepartamentos.GridControl = this.gcDepartamentos;
            this.gvDepartamentos.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvDepartamentos.Name = "gvDepartamentos";
            this.gvDepartamentos.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvDepartamentos.OptionsBehavior.Editable = false;
            this.gvDepartamentos.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvDepartamentos.OptionsView.ColumnAutoWidth = false;
            this.gvDepartamentos.OptionsView.EnableAppearanceEvenRow = true;
            this.gvDepartamentos.OptionsView.EnableAppearanceOddRow = true;
            this.gvDepartamentos.OptionsView.ShowGroupPanel = false;
            this.gvDepartamentos.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn2, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvDepartamentos.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gvDepartamentos_SelectionChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "ID";
            this.gridColumn1.FieldName = "id";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Nome";
            this.gridColumn2.FieldName = "descricao";
            this.gridColumn2.MinWidth = 180;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 300;
            // 
            // ColunaEmpresa
            // 
            this.ColunaEmpresa.Caption = "Empresa";
            this.ColunaEmpresa.FieldName = "empresa";
            this.ColunaEmpresa.Name = "ColunaEmpresa";
            this.ColunaEmpresa.Visible = true;
            this.ColunaEmpresa.VisibleIndex = 1;
            this.ColunaEmpresa.Width = 300;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Location = new System.Drawing.Point(6, 200);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(96, 18);
            this.labelControl3.TabIndex = 11;
            this.labelControl3.Text = "Funcionários";
            // 
            // gcFuncionarios
            // 
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcFuncionarios.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcFuncionarios.Location = new System.Drawing.Point(6, 224);
            this.gcFuncionarios.MainView = this.gvFuncionarios;
            this.gcFuncionarios.Name = "gcFuncionarios";
            this.gcFuncionarios.Size = new System.Drawing.Size(721, 169);
            this.gcFuncionarios.TabIndex = 14;
            this.gcFuncionarios.UseEmbeddedNavigator = true;
            this.gcFuncionarios.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvFuncionarios});
            // 
            // gvFuncionarios
            // 
            this.gvFuncionarios.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.Empty.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvFuncionarios.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvFuncionarios.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvFuncionarios.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvFuncionarios.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvFuncionarios.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvFuncionarios.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.OddRow.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.OddRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvFuncionarios.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.Preview.Options.UseFont = true;
            this.gvFuncionarios.Appearance.Preview.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.Row.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvFuncionarios.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ColunaIDFunc,
            this.gridColumn4});
            this.gvFuncionarios.GridControl = this.gcFuncionarios;
            this.gvFuncionarios.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvFuncionarios.Name = "gvFuncionarios";
            this.gvFuncionarios.OptionsBehavior.Editable = false;
            this.gvFuncionarios.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvFuncionarios.OptionsView.EnableAppearanceEvenRow = true;
            this.gvFuncionarios.OptionsView.EnableAppearanceOddRow = true;
            this.gvFuncionarios.OptionsView.ShowAutoFilterRow = true;
            this.gvFuncionarios.OptionsView.ShowGroupPanel = false;
            this.gvFuncionarios.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn4, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // ColunaIDFunc
            // 
            this.ColunaIDFunc.Caption = "ID";
            this.ColunaIDFunc.FieldName = "id";
            this.ColunaIDFunc.Name = "ColunaIDFunc";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Nome";
            this.gridColumn4.FieldName = "nome";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Selecionar_timao.ico");
            this.imageList1.Images.SetKeyName(1, "cancelar.ico");
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.rgTipo);
            this.groupControl1.Location = new System.Drawing.Point(6, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(357, 50);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Tipo";
            // 
            // rgTipo
            // 
            this.rgTipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipo.Location = new System.Drawing.Point(2, 21);
            this.rgTipo.Name = "rgTipo";
            this.rgTipo.Properties.EnableFocusRect = true;
            this.rgTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Empresa"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Departamento"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Funcionário")});
            this.rgTipo.Size = new System.Drawing.Size(353, 27);
            this.rgTipo.TabIndex = 0;
            this.rgTipo.EditValueChanged += new System.EventHandler(this.rgTipo_EditValueChanged);
            // 
            // sbSelecionarEmpresas
            // 
            this.sbSelecionarEmpresas.ImageIndex = 0;
            this.sbSelecionarEmpresas.ImageList = this.imageList1;
            this.sbSelecionarEmpresas.Location = new System.Drawing.Point(126, 59);
            this.sbSelecionarEmpresas.Name = "sbSelecionarEmpresas";
            this.sbSelecionarEmpresas.Size = new System.Drawing.Size(117, 23);
            this.sbSelecionarEmpresas.TabIndex = 4;
            this.sbSelecionarEmpresas.Text = "Selecionar Todas";
            this.sbSelecionarEmpresas.Click += new System.EventHandler(this.sbSelecionarEmpresas_Click);
            // 
            // sbLimparEmpresa
            // 
            this.sbLimparEmpresa.ImageIndex = 1;
            this.sbLimparEmpresa.ImageList = this.imageList1;
            this.sbLimparEmpresa.Location = new System.Drawing.Point(246, 59);
            this.sbLimparEmpresa.Name = "sbLimparEmpresa";
            this.sbLimparEmpresa.Size = new System.Drawing.Size(117, 23);
            this.sbLimparEmpresa.TabIndex = 5;
            this.sbLimparEmpresa.Text = "Limpar Seleção";
            this.sbLimparEmpresa.Click += new System.EventHandler(this.sbLimparEmpresa_Click);
            // 
            // sbLimparFuncionarios
            // 
            this.sbLimparFuncionarios.ImageIndex = 1;
            this.sbLimparFuncionarios.ImageList = this.imageList1;
            this.sbLimparFuncionarios.Location = new System.Drawing.Point(246, 200);
            this.sbLimparFuncionarios.Name = "sbLimparFuncionarios";
            this.sbLimparFuncionarios.Size = new System.Drawing.Size(117, 23);
            this.sbLimparFuncionarios.TabIndex = 13;
            this.sbLimparFuncionarios.Text = "Limpar Seleção";
            this.sbLimparFuncionarios.Click += new System.EventHandler(this.sbLimparFuncionarios_Click);
            // 
            // sbSelecionarFuncionarios
            // 
            this.sbSelecionarFuncionarios.ImageIndex = 0;
            this.sbSelecionarFuncionarios.ImageList = this.imageList1;
            this.sbSelecionarFuncionarios.Location = new System.Drawing.Point(126, 200);
            this.sbSelecionarFuncionarios.Name = "sbSelecionarFuncionarios";
            this.sbSelecionarFuncionarios.Size = new System.Drawing.Size(117, 23);
            this.sbSelecionarFuncionarios.TabIndex = 12;
            this.sbSelecionarFuncionarios.Text = "Selecionar Todos";
            this.sbSelecionarFuncionarios.Click += new System.EventHandler(this.sbSelecionarFuncionarios_Click);
            // 
            // sbLimparDepartamento
            // 
            this.sbLimparDepartamento.ImageIndex = 1;
            this.sbLimparDepartamento.ImageList = this.imageList1;
            this.sbLimparDepartamento.Location = new System.Drawing.Point(609, 59);
            this.sbLimparDepartamento.Name = "sbLimparDepartamento";
            this.sbLimparDepartamento.Size = new System.Drawing.Size(117, 23);
            this.sbLimparDepartamento.TabIndex = 9;
            this.sbLimparDepartamento.Text = "Limpar Seleção";
            this.sbLimparDepartamento.Click += new System.EventHandler(this.sbLimparDepartamento_Click);
            // 
            // sbSelecionarDepartamentos
            // 
            this.sbSelecionarDepartamentos.ImageIndex = 0;
            this.sbSelecionarDepartamentos.ImageList = this.imageList1;
            this.sbSelecionarDepartamentos.Location = new System.Drawing.Point(489, 59);
            this.sbSelecionarDepartamentos.Name = "sbSelecionarDepartamentos";
            this.sbSelecionarDepartamentos.Size = new System.Drawing.Size(117, 23);
            this.sbSelecionarDepartamentos.TabIndex = 8;
            this.sbSelecionarDepartamentos.Text = "Selecionar Todos";
            this.sbSelecionarDepartamentos.Click += new System.EventHandler(this.sbSelecionarDepartamentos_Click);
            // 
            // FormBaseGridFiltro1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(764, 580);
            this.Name = "FormBaseGridFiltro1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormBaseGridFiltro1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcEmpresas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEmpresas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDepartamentos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDepartamentos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcFuncionarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFuncionarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.Columns.GridColumn ColunaIDFunc;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private System.Windows.Forms.ImageList imageList1;
        public DevExpress.XtraEditors.LabelControl labelControl1;
        public DevExpress.XtraGrid.GridControl gcEmpresas;
        public DevExpress.XtraEditors.LabelControl labelControl3;
        public DevExpress.XtraGrid.GridControl gcFuncionarios;
        public DevExpress.XtraEditors.LabelControl labelControl2;
        public DevExpress.XtraGrid.GridControl gcDepartamentos;
        public DevExpress.XtraEditors.GroupControl groupControl1;
        public DevExpress.XtraEditors.SimpleButton sbSelecionarEmpresas;
        public DevExpress.XtraEditors.SimpleButton sbLimparEmpresa;
        public DevExpress.XtraEditors.SimpleButton sbLimparFuncionarios;
        public DevExpress.XtraEditors.SimpleButton sbSelecionarFuncionarios;
        public DevExpress.XtraEditors.SimpleButton sbLimparDepartamento;
        public DevExpress.XtraEditors.SimpleButton sbSelecionarDepartamentos;
        public DevExpress.XtraEditors.RadioGroup rgTipo;
        public DevExpress.XtraGrid.Views.Grid.GridView gvEmpresas;
        public DevExpress.XtraGrid.Views.Grid.GridView gvFuncionarios;
        public DevExpress.XtraGrid.Views.Grid.GridView gvDepartamentos;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaEmpresa;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    }
}
