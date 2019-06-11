namespace UI.Relatorios
{
    partial class Inconsistencias
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
            this.txtPeriodoF = new DevExpress.XtraEditors.DateEdit();
            this.lblPeriodoF = new System.Windows.Forms.Label();
            this.txtPeriodoI = new DevExpress.XtraEditors.DateEdit();
            this.lblPeriodoI = new System.Windows.Forms.Label();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.gcInconsistencias = new DevExpress.XtraGrid.GridControl();
            this.gvInconsistencias = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColunaIDFunc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Descricao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sbSelecionarInconsistencias = new DevExpress.XtraEditors.SimpleButton();
            this.sbLimparInconsistencias = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoF.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoF.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcInconsistencias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInconsistencias)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // rgTipo
            // 
            this.rgTipo.EditValue = 0;
            this.rgTipo.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 425);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 421);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(745, 421);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(666, 421);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(808, 403);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.sbLimparInconsistencias);
            this.tabPage1.Controls.Add(this.sbSelecionarInconsistencias);
            this.tabPage1.Controls.Add(this.labelControl4);
            this.tabPage1.Controls.Add(this.gcInconsistencias);
            this.tabPage1.Controls.Add(this.txtPeriodoF);
            this.tabPage1.Controls.Add(this.lblPeriodoF);
            this.tabPage1.Controls.Add(this.txtPeriodoI);
            this.tabPage1.Controls.Add(this.lblPeriodoI);
            this.tabPage1.Size = new System.Drawing.Size(802, 397);
            this.tabPage1.Controls.SetChildIndex(this.labelControl1, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl2, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl3, 0);
            this.tabPage1.Controls.SetChildIndex(this.groupControl1, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarEmpresas, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparEmpresa, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarFuncionarios, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparFuncionarios, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarDepartamentos, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparDepartamento, 0);
            this.tabPage1.Controls.SetChildIndex(this.lblPeriodoI, 0);
            this.tabPage1.Controls.SetChildIndex(this.txtPeriodoI, 0);
            this.tabPage1.Controls.SetChildIndex(this.lblPeriodoF, 0);
            this.tabPage1.Controls.SetChildIndex(this.txtPeriodoF, 0);
            this.tabPage1.Controls.SetChildIndex(this.gcInconsistencias, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl4, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarInconsistencias, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparInconsistencias, 0);
            // 
            // txtPeriodoF
            // 
            this.txtPeriodoF.EditValue = null;
            this.txtPeriodoF.Location = new System.Drawing.Point(575, 21);
            this.txtPeriodoF.Name = "txtPeriodoF";
            this.txtPeriodoF.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtPeriodoF.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtPeriodoF.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPeriodoF.Size = new System.Drawing.Size(79, 20);
            this.txtPeriodoF.TabIndex = 2;
            // 
            // lblPeriodoF
            // 
            this.lblPeriodoF.AutoSize = true;
            this.lblPeriodoF.Location = new System.Drawing.Point(556, 24);
            this.lblPeriodoF.Name = "lblPeriodoF";
            this.lblPeriodoF.Size = new System.Drawing.Size(13, 13);
            this.lblPeriodoF.TabIndex = 23;
            this.lblPeriodoF.Text = "à";
            // 
            // txtPeriodoI
            // 
            this.txtPeriodoI.EditValue = null;
            this.txtPeriodoI.Location = new System.Drawing.Point(471, 21);
            this.txtPeriodoI.Name = "txtPeriodoI";
            this.txtPeriodoI.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtPeriodoI.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtPeriodoI.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPeriodoI.Size = new System.Drawing.Size(79, 20);
            this.txtPeriodoI.TabIndex = 1;
            // 
            // lblPeriodoI
            // 
            this.lblPeriodoI.AutoSize = true;
            this.lblPeriodoI.Location = new System.Drawing.Point(417, 24);
            this.lblPeriodoI.Name = "lblPeriodoI";
            this.lblPeriodoI.Size = new System.Drawing.Size(48, 13);
            this.lblPeriodoI.TabIndex = 21;
            this.lblPeriodoI.Text = "Período:";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Location = new System.Drawing.Point(418, 200);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(117, 18);
            this.labelControl4.TabIndex = 24;
            this.labelControl4.Text = "Inconsistências";
            // 
            // gcInconsistencias
            // 
            this.gcInconsistencias.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gcInconsistencias.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcInconsistencias.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gcInconsistencias.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcInconsistencias.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gcInconsistencias.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcInconsistencias.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gcInconsistencias.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcInconsistencias.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcInconsistencias.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcInconsistencias.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcInconsistencias.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcInconsistencias.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcInconsistencias.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcInconsistencias.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcInconsistencias.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcInconsistencias.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcInconsistencias.Location = new System.Drawing.Point(418, 224);
            this.gcInconsistencias.MainView = this.gvInconsistencias;
            this.gcInconsistencias.Name = "gcInconsistencias";
            this.gcInconsistencias.Size = new System.Drawing.Size(375, 169);
            this.gcInconsistencias.TabIndex = 25;
            this.gcInconsistencias.UseEmbeddedNavigator = true;
            this.gcInconsistencias.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvInconsistencias});
            // 
            // gvInconsistencias
            // 
            this.gvInconsistencias.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvInconsistencias.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.Empty.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvInconsistencias.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvInconsistencias.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvInconsistencias.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvInconsistencias.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvInconsistencias.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvInconsistencias.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvInconsistencias.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvInconsistencias.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvInconsistencias.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvInconsistencias.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvInconsistencias.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvInconsistencias.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvInconsistencias.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvInconsistencias.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvInconsistencias.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvInconsistencias.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvInconsistencias.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvInconsistencias.Appearance.OddRow.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.OddRow.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvInconsistencias.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.Preview.Options.UseFont = true;
            this.gvInconsistencias.Appearance.Preview.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvInconsistencias.Appearance.Row.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvInconsistencias.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvInconsistencias.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvInconsistencias.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvInconsistencias.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvInconsistencias.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvInconsistencias.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvInconsistencias.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvInconsistencias.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ColunaIDFunc,
            this.Descricao});
            this.gvInconsistencias.GridControl = this.gcInconsistencias;
            this.gvInconsistencias.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvInconsistencias.Name = "gvInconsistencias";
            this.gvInconsistencias.OptionsBehavior.Editable = false;
            this.gvInconsistencias.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvInconsistencias.OptionsSelection.MultiSelect = true;
            this.gvInconsistencias.OptionsView.EnableAppearanceEvenRow = true;
            this.gvInconsistencias.OptionsView.EnableAppearanceOddRow = true;
            this.gvInconsistencias.OptionsView.ShowAutoFilterRow = true;
            this.gvInconsistencias.OptionsView.ShowGroupPanel = false;
            this.gvInconsistencias.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.Descricao, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // ColunaIDFunc
            // 
            this.ColunaIDFunc.Caption = "ID";
            this.ColunaIDFunc.FieldName = "id";
            this.ColunaIDFunc.Name = "ColunaIDFunc";
            // 
            // Descricao
            // 
            this.Descricao.Caption = "Descricao";
            this.Descricao.FieldName = "Descricao";
            this.Descricao.Name = "Descricao";
            this.Descricao.Visible = true;
            this.Descricao.VisibleIndex = 0;
            // 
            // sbSelecionarInconsistencias
            // 
            this.sbSelecionarInconsistencias.Image = global::UI.Properties.Resources.Selecionar_roxo;
            this.sbSelecionarInconsistencias.ImageIndex = 0;
            this.sbSelecionarInconsistencias.Location = new System.Drawing.Point(553, 200);
            this.sbSelecionarInconsistencias.Name = "sbSelecionarInconsistencias";
            this.sbSelecionarInconsistencias.Size = new System.Drawing.Size(117, 23);
            this.sbSelecionarInconsistencias.TabIndex = 26;
            this.sbSelecionarInconsistencias.Text = "Selecionar Todos";
            this.sbSelecionarInconsistencias.Click += new System.EventHandler(this.sbSelecionarInconsistencias_Click);
            // 
            // sbLimparInconsistencias
            // 
            this.sbLimparInconsistencias.Image = global::UI.Properties.Resources.cancelar_copy;
            this.sbLimparInconsistencias.ImageIndex = 0;
            this.sbLimparInconsistencias.Location = new System.Drawing.Point(676, 200);
            this.sbLimparInconsistencias.Name = "sbLimparInconsistencias";
            this.sbLimparInconsistencias.Size = new System.Drawing.Size(117, 23);
            this.sbLimparInconsistencias.TabIndex = 27;
            this.sbLimparInconsistencias.Text = "Limpar Seleção";
            this.sbLimparInconsistencias.Click += new System.EventHandler(this.sbLimparInconsistencias_Click);
            // 
            // Inconsistencias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(831, 450);
            this.MinimizeBox = false;
            this.Name = "Inconsistencias";
            this.ShowInTaskbar = false;
            this.Text = "Relatório de Inconsistências";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoF.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoF.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcInconsistencias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvInconsistencias)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit txtPeriodoF;
        private System.Windows.Forms.Label lblPeriodoF;
        private DevExpress.XtraEditors.DateEdit txtPeriodoI;
        private System.Windows.Forms.Label lblPeriodoI;
        public DevExpress.XtraEditors.LabelControl labelControl4;
        public DevExpress.XtraGrid.GridControl gcInconsistencias;
        public DevExpress.XtraGrid.Views.Grid.GridView gvInconsistencias;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaIDFunc;
        private DevExpress.XtraGrid.Columns.GridColumn Descricao;
        public DevExpress.XtraEditors.SimpleButton sbLimparInconsistencias;
        public DevExpress.XtraEditors.SimpleButton sbSelecionarInconsistencias;
    }
}
