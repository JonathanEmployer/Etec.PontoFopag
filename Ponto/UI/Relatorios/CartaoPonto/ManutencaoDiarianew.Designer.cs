namespace UI.Relatorios.CartaoPonto
{
    partial class relatorio_manutencao_diaria
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
            this.gcDepartamentos = new DevExpress.XtraGrid.GridControl();
            this.gvDepartamentos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColunaEmpresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sbLimparEmpresa = new DevExpress.XtraEditors.SimpleButton();
            this.sbSelecionarEmpresas = new DevExpress.XtraEditors.SimpleButton();
            this.sbLimparDepartamento = new DevExpress.XtraEditors.SimpleButton();
            this.sbSelecionarDepartamento = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtData = new DevExpress.XtraEditors.DateEdit();
            this.lblDataFinal = new DevExpress.XtraEditors.LabelControl();
            this.txtDataFinal = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDepartamentos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDepartamentos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 388);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 384);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(649, 384);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(570, 384);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(712, 366);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelControl3);
            this.tabPage1.Controls.Add(this.txtDataFinal);
            this.tabPage1.Controls.Add(this.txtData);
            this.tabPage1.Controls.Add(this.lblDataFinal);
            this.tabPage1.Controls.Add(this.sbLimparDepartamento);
            this.tabPage1.Controls.Add(this.sbSelecionarDepartamento);
            this.tabPage1.Controls.Add(this.labelControl2);
            this.tabPage1.Controls.Add(this.sbLimparEmpresa);
            this.tabPage1.Controls.Add(this.sbSelecionarEmpresas);
            this.tabPage1.Controls.Add(this.gcDepartamentos);
            this.tabPage1.Size = new System.Drawing.Size(706, 360);
            this.tabPage1.Controls.SetChildIndex(this.gcDepartamentos, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarEmpresas, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparEmpresa, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl2, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarDepartamento, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparDepartamento, 0);
            this.tabPage1.Controls.SetChildIndex(this.lblDataFinal, 0);
            this.tabPage1.Controls.SetChildIndex(this.txtData, 0);
            this.tabPage1.Controls.SetChildIndex(this.txtDataFinal, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl3, 0);
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
            this.gcDepartamentos.Location = new System.Drawing.Point(17, 182);
            this.gcDepartamentos.MainView = this.gvDepartamentos;
            this.gcDepartamentos.Name = "gcDepartamentos";
            this.gcDepartamentos.Size = new System.Drawing.Size(669, 163);
            this.gcDepartamentos.TabIndex = 11;
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
            this.gvDepartamentos.OptionsBehavior.Editable = false;
            this.gvDepartamentos.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvDepartamentos.OptionsSelection.MultiSelect = true;
            this.gvDepartamentos.OptionsView.EnableAppearanceEvenRow = true;
            this.gvDepartamentos.OptionsView.EnableAppearanceOddRow = true;
            this.gvDepartamentos.OptionsView.ShowGroupPanel = false;
            this.gvDepartamentos.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn2, DevExpress.Data.ColumnSortOrder.Ascending)});
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
            this.gridColumn2.Width = 180;
            // 
            // ColunaEmpresa
            // 
            this.ColunaEmpresa.Caption = "Empresa";
            this.ColunaEmpresa.FieldName = "empresa";
            this.ColunaEmpresa.Name = "ColunaEmpresa";
            this.ColunaEmpresa.Visible = true;
            this.ColunaEmpresa.VisibleIndex = 1;
            this.ColunaEmpresa.Width = 100;
            // 
            // sbLimparEmpresa
            // 
            this.sbLimparEmpresa.Image = global::UI.Properties.Resources.cancelar_copy;
            this.sbLimparEmpresa.ImageIndex = 1;
            this.sbLimparEmpresa.Location = new System.Drawing.Point(288, 7);
            this.sbLimparEmpresa.Name = "sbLimparEmpresa";
            this.sbLimparEmpresa.Size = new System.Drawing.Size(117, 23);
            this.sbLimparEmpresa.TabIndex = 14;
            this.sbLimparEmpresa.Text = "Limpar Seleção";
            this.sbLimparEmpresa.Click += new System.EventHandler(this.sbLimparEmpresa_Click);
            // 
            // sbSelecionarEmpresas
            // 
            this.sbSelecionarEmpresas.Image = global::UI.Properties.Resources.Selecionar_roxo;
            this.sbSelecionarEmpresas.ImageIndex = 0;
            this.sbSelecionarEmpresas.Location = new System.Drawing.Point(165, 7);
            this.sbSelecionarEmpresas.Name = "sbSelecionarEmpresas";
            this.sbSelecionarEmpresas.Size = new System.Drawing.Size(117, 23);
            this.sbSelecionarEmpresas.TabIndex = 13;
            this.sbSelecionarEmpresas.Text = "Selecionar Todas";
            this.sbSelecionarEmpresas.Click += new System.EventHandler(this.sbSelecionarEmpresas_Click);
            // 
            // sbLimparDepartamento
            // 
            this.sbLimparDepartamento.Image = global::UI.Properties.Resources.cancelar_copy;
            this.sbLimparDepartamento.ImageIndex = 1;
            this.sbLimparDepartamento.Location = new System.Drawing.Point(285, 153);
            this.sbLimparDepartamento.Name = "sbLimparDepartamento";
            this.sbLimparDepartamento.Size = new System.Drawing.Size(117, 23);
            this.sbLimparDepartamento.TabIndex = 17;
            this.sbLimparDepartamento.Text = "Limpar Seleção";
            this.sbLimparDepartamento.Click += new System.EventHandler(this.sbLimparDepartamento_Click);
            // 
            // sbSelecionarDepartamento
            // 
            this.sbSelecionarDepartamento.Appearance.Options.UseImage = true;
            this.sbSelecionarDepartamento.Image = global::UI.Properties.Resources.Selecionar_roxo;
            this.sbSelecionarDepartamento.ImageIndex = 0;
            this.sbSelecionarDepartamento.Location = new System.Drawing.Point(162, 153);
            this.sbSelecionarDepartamento.Name = "sbSelecionarDepartamento";
            this.sbSelecionarDepartamento.Size = new System.Drawing.Size(117, 23);
            this.sbSelecionarDepartamento.TabIndex = 16;
            this.sbSelecionarDepartamento.Text = "Selecionar Todos";
            this.sbSelecionarDepartamento.Click += new System.EventHandler(this.sbSelecionarDepartamento_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Location = new System.Drawing.Point(17, 153);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(114, 18);
            this.labelControl2.TabIndex = 15;
            this.labelControl2.Text = "Departamentos";
            // 
            // txtData
            // 
            this.txtData.EditValue = null;
            this.txtData.Location = new System.Drawing.Point(472, 12);
            this.txtData.Name = "txtData";
            this.txtData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtData.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtData.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtData.Size = new System.Drawing.Size(98, 20);
            this.txtData.TabIndex = 9;
            // 
            // lblDataFinal
            // 
            this.lblDataFinal.Location = new System.Drawing.Point(426, 14);
            this.lblDataFinal.Name = "lblDataFinal";
            this.lblDataFinal.Size = new System.Drawing.Size(40, 13);
            this.lblDataFinal.TabIndex = 8;
            this.lblDataFinal.Text = "Período:";
            // 
            // txtDataFinal
            // 
            this.txtDataFinal.EditValue = null;
            this.txtDataFinal.Location = new System.Drawing.Point(588, 12);
            this.txtDataFinal.Name = "txtDataFinal";
            this.txtDataFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataFinal.Size = new System.Drawing.Size(98, 20);
            this.txtDataFinal.TabIndex = 10;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(576, 14);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(6, 13);
            this.labelControl3.TabIndex = 19;
            this.labelControl3.Text = "à";
            // 
            // relatorio_manutencao_diaria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(736, 413);
            this.Name = "relatorio_manutencao_diaria";
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDepartamentos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDepartamentos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraGrid.GridControl gcDepartamentos;
        public DevExpress.XtraGrid.Views.Grid.GridView gvDepartamentos;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaEmpresa;
        public DevExpress.XtraEditors.SimpleButton sbLimparDepartamento;
        public DevExpress.XtraEditors.SimpleButton sbSelecionarDepartamento;
        public DevExpress.XtraEditors.LabelControl labelControl2;
        public DevExpress.XtraEditors.SimpleButton sbLimparEmpresa;
        public DevExpress.XtraEditors.SimpleButton sbSelecionarEmpresas;
        private DevExpress.XtraEditors.DateEdit txtData;
        private DevExpress.XtraEditors.LabelControl lblDataFinal;
        private DevExpress.XtraEditors.DateEdit txtDataFinal;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}
