namespace UI.Relatorios
{
    partial class FormRelatorioAbono
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
            this.dataFinal = new DevExpress.XtraEditors.DateEdit();
            this.lbdataFinal = new System.Windows.Forms.Label();
            this.lbdataInicial = new System.Windows.Forms.Label();
            this.dataInicial = new DevExpress.XtraEditors.DateEdit();
            this.cbAgruparPorDepartamento = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.gcOcorrencias = new DevExpress.XtraGrid.GridControl();
            this.gvOcorrencias = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColunaIDFunc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataInicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAgruparPorDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcOcorrencias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOcorrencias)).BeginInit();
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
            // sbLimparDepartamento
            // 
            this.sbLimparDepartamento.TabIndex = 10;
            // 
            // sbSelecionarDepartamentos
            // 
            this.sbSelecionarDepartamentos.TabIndex = 9;
            // 
            // rgTipo
            // 
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 434);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 430);
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(752, 430);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(673, 430);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(815, 412);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelControl4);
            this.tabPage1.Controls.Add(this.gcOcorrencias);
            this.tabPage1.Controls.Add(this.cbAgruparPorDepartamento);
            this.tabPage1.Controls.Add(this.dataFinal);
            this.tabPage1.Controls.Add(this.lbdataFinal);
            this.tabPage1.Controls.Add(this.dataInicial);
            this.tabPage1.Controls.Add(this.lbdataInicial);
            this.tabPage1.Size = new System.Drawing.Size(809, 406);
            this.tabPage1.Controls.SetChildIndex(this.lbdataInicial, 0);
            this.tabPage1.Controls.SetChildIndex(this.dataInicial, 0);
            this.tabPage1.Controls.SetChildIndex(this.lbdataFinal, 0);
            this.tabPage1.Controls.SetChildIndex(this.dataFinal, 0);
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
            this.tabPage1.Controls.SetChildIndex(this.cbAgruparPorDepartamento, 0);
            this.tabPage1.Controls.SetChildIndex(this.gcOcorrencias, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl4, 0);
            // 
            // dataFinal
            // 
            this.dataFinal.EditValue = null;
            this.dataFinal.Location = new System.Drawing.Point(662, 31);
            this.dataFinal.Name = "dataFinal";
            this.dataFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dataFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dataFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dataFinal.Size = new System.Drawing.Size(100, 20);
            this.dataFinal.TabIndex = 8;
            // 
            // lbdataFinal
            // 
            this.lbdataFinal.AutoSize = true;
            this.lbdataFinal.Location = new System.Drawing.Point(595, 34);
            this.lbdataFinal.Name = "lbdataFinal";
            this.lbdataFinal.Size = new System.Drawing.Size(58, 13);
            this.lbdataFinal.TabIndex = 8;
            this.lbdataFinal.Text = "Data Final:";
            // 
            // lbdataInicial
            // 
            this.lbdataInicial.AutoSize = true;
            this.lbdataInicial.Location = new System.Drawing.Point(423, 34);
            this.lbdataInicial.Name = "lbdataInicial";
            this.lbdataInicial.Size = new System.Drawing.Size(63, 13);
            this.lbdataInicial.TabIndex = 6;
            this.lbdataInicial.Text = "Data Inicial:";
            // 
            // dataInicial
            // 
            this.dataInicial.EditValue = null;
            this.dataInicial.Location = new System.Drawing.Point(489, 31);
            this.dataInicial.Name = "dataInicial";
            this.dataInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dataInicial.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dataInicial.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dataInicial.Size = new System.Drawing.Size(100, 20);
            this.dataInicial.TabIndex = 7;
            // 
            // cbAgruparPorDepartamento
            // 
            this.cbAgruparPorDepartamento.Location = new System.Drawing.Point(424, 6);
            this.cbAgruparPorDepartamento.Name = "cbAgruparPorDepartamento";
            this.cbAgruparPorDepartamento.Properties.Caption = "Agrupar por Departamento";
            this.cbAgruparPorDepartamento.Size = new System.Drawing.Size(167, 19);
            this.cbAgruparPorDepartamento.TabIndex = 19;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Location = new System.Drawing.Point(418, 200);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(91, 18);
            this.labelControl4.TabIndex = 20;
            this.labelControl4.Text = "Ocorrências";
            // 
            // gcOcorrencias
            // 
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcOcorrencias.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcOcorrencias.Location = new System.Drawing.Point(418, 224);
            this.gcOcorrencias.MainView = this.gvOcorrencias;
            this.gcOcorrencias.Name = "gcOcorrencias";
            this.gcOcorrencias.Size = new System.Drawing.Size(375, 169);
            this.gcOcorrencias.TabIndex = 21;
            this.gcOcorrencias.UseEmbeddedNavigator = true;
            this.gcOcorrencias.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOcorrencias});
            // 
            // gvOcorrencias
            // 
            this.gvOcorrencias.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.Empty.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvOcorrencias.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvOcorrencias.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvOcorrencias.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvOcorrencias.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvOcorrencias.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvOcorrencias.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.OddRow.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.OddRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvOcorrencias.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.Preview.Options.UseFont = true;
            this.gvOcorrencias.Appearance.Preview.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.Row.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvOcorrencias.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ColunaIDFunc,
            this.gridColumn4});
            this.gvOcorrencias.GridControl = this.gcOcorrencias;
            this.gvOcorrencias.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvOcorrencias.Name = "gvOcorrencias";
            this.gvOcorrencias.OptionsBehavior.Editable = false;
            this.gvOcorrencias.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvOcorrencias.OptionsView.EnableAppearanceEvenRow = true;
            this.gvOcorrencias.OptionsView.EnableAppearanceOddRow = true;
            this.gvOcorrencias.OptionsView.ShowAutoFilterRow = true;
            this.gvOcorrencias.OptionsView.ShowGroupPanel = false;
            this.gvOcorrencias.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
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
            this.gridColumn4.FieldName = "descricao";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            // 
            // FormRelatorioAbono
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(838, 459);
            this.Name = "FormRelatorioAbono";
            this.Text = "Relatório de Abono";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormRelatorioAbono_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormRelatorioAbono_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataInicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAgruparPorDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcOcorrencias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOcorrencias)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit dataFinal;
        private System.Windows.Forms.Label lbdataFinal;
        private System.Windows.Forms.Label lbdataInicial;
        private DevExpress.XtraEditors.DateEdit dataInicial;
        private DevExpress.XtraEditors.CheckEdit cbAgruparPorDepartamento;
        public DevExpress.XtraEditors.LabelControl labelControl4;
        public DevExpress.XtraGrid.GridControl gcOcorrencias;
        public DevExpress.XtraGrid.Views.Grid.GridView gvOcorrencias;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaIDFunc;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}
