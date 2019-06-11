namespace UI
{
    partial class FormManutLayoutExportacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManutLayoutExportacao));
            this.txtDescricao = new DevExpress.XtraEditors.TextEdit();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.lblDescricao = new DevExpress.XtraEditors.LabelControl();
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.sbIncluirExpCampos = new DevExpress.XtraEditors.SimpleButton();
            this.sbAlterarExpCampos = new DevExpress.XtraEditors.SimpleButton();
            this.sbExcluirExpCampos = new DevExpress.XtraEditors.SimpleButton();
            this.gcExportacaoCampos = new DevExpress.XtraGrid.GridControl();
            this.gvExportacaoCampos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblCampos = new DevExpress.XtraEditors.LabelControl();
            this.sbVisualizar = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcExportacaoCampos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExportacaoCampos)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(517, 323);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.sbVisualizar);
            this.xtraTabPage1.Controls.Add(this.lblCampos);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.sbIncluirExpCampos);
            this.xtraTabPage1.Controls.Add(this.sbAlterarExpCampos);
            this.xtraTabPage1.Controls.Add(this.sbExcluirExpCampos);
            this.xtraTabPage1.Controls.Add(this.gcExportacaoCampos);
            this.xtraTabPage1.Controls.Add(this.txtDescricao);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblDescricao);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(508, 314);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(454, 341);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(373, 341);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 341);
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(62, 37);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Properties.MaxLength = 50;
            this.txtDescricao.Size = new System.Drawing.Size(441, 20);
            this.txtDescricao.TabIndex = 3;
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(62, 11);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Properties.ReadOnly = true;
            this.txtCodigo.Properties.ValidateOnEnterKey = true;
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // lblDescricao
            // 
            this.lblDescricao.Location = new System.Drawing.Point(6, 40);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(50, 13);
            this.lblDescricao.TabIndex = 2;
            this.lblDescricao.Text = "Descrição:";
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(19, 14);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // sbIncluirExpCampos
            // 
            this.sbIncluirExpCampos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbIncluirExpCampos.Image = ((System.Drawing.Image)(resources.GetObject("sbIncluirExpCampos.Image")));
            this.sbIncluirExpCampos.ImageIndex = 3;
            this.sbIncluirExpCampos.Location = new System.Drawing.Point(272, 287);
            this.sbIncluirExpCampos.Name = "sbIncluirExpCampos";
            this.sbIncluirExpCampos.Size = new System.Drawing.Size(75, 23);
            this.sbIncluirExpCampos.TabIndex = 7;
            this.sbIncluirExpCampos.Text = "&Incluir";
            this.sbIncluirExpCampos.Click += new System.EventHandler(this.sbIncluirExpCampos_Click);
            // 
            // sbAlterarExpCampos
            // 
            this.sbAlterarExpCampos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbAlterarExpCampos.Image = ((System.Drawing.Image)(resources.GetObject("sbAlterarExpCampos.Image")));
            this.sbAlterarExpCampos.ImageIndex = 4;
            this.sbAlterarExpCampos.Location = new System.Drawing.Point(350, 287);
            this.sbAlterarExpCampos.Name = "sbAlterarExpCampos";
            this.sbAlterarExpCampos.Size = new System.Drawing.Size(75, 23);
            this.sbAlterarExpCampos.TabIndex = 8;
            this.sbAlterarExpCampos.Text = "&Alterar";
            this.sbAlterarExpCampos.Click += new System.EventHandler(this.sbAlterarExpCampos_Click);
            // 
            // sbExcluirExpCampos
            // 
            this.sbExcluirExpCampos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbExcluirExpCampos.Image = ((System.Drawing.Image)(resources.GetObject("sbExcluirExpCampos.Image")));
            this.sbExcluirExpCampos.ImageIndex = 5;
            this.sbExcluirExpCampos.Location = new System.Drawing.Point(428, 287);
            this.sbExcluirExpCampos.Name = "sbExcluirExpCampos";
            this.sbExcluirExpCampos.Size = new System.Drawing.Size(75, 23);
            this.sbExcluirExpCampos.TabIndex = 9;
            this.sbExcluirExpCampos.Text = "&Excluir";
            this.sbExcluirExpCampos.Click += new System.EventHandler(this.sbExcluirExpCampos_Click);
            // 
            // gcExportacaoCampos
            // 
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcExportacaoCampos.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcExportacaoCampos.EmbeddedNavigator.Name = "";
            this.gcExportacaoCampos.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcExportacaoCampos.Location = new System.Drawing.Point(4, 85);
            this.gcExportacaoCampos.LookAndFeel.UseWindowsXPTheme = true;
            this.gcExportacaoCampos.MainView = this.gvExportacaoCampos;
            this.gcExportacaoCampos.Name = "gcExportacaoCampos";
            this.gcExportacaoCampos.Size = new System.Drawing.Size(499, 196);
            this.gcExportacaoCampos.TabIndex = 5;
            this.gcExportacaoCampos.UseEmbeddedNavigator = true;
            this.gcExportacaoCampos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvExportacaoCampos});
            this.gcExportacaoCampos.DoubleClick += new System.EventHandler(this.gcExportacaoCampos_DoubleClick);
            this.gcExportacaoCampos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gcExportacaoCampos_KeyDown);
            // 
            // gvExportacaoCampos
            // 
            this.gvExportacaoCampos.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.ColumnFilterButton.Options.UseTextOptions = true;
            this.gvExportacaoCampos.Appearance.ColumnFilterButton.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvExportacaoCampos.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.Empty.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.EvenRow.BackColor2 = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvExportacaoCampos.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvExportacaoCampos.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvExportacaoCampos.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvExportacaoCampos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvExportacaoCampos.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvExportacaoCampos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvExportacaoCampos.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvExportacaoCampos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvExportacaoCampos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvExportacaoCampos.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvExportacaoCampos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvExportacaoCampos.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.OddRow.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.OddRow.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvExportacaoCampos.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.Preview.Options.UseFont = true;
            this.gvExportacaoCampos.Appearance.Preview.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvExportacaoCampos.Appearance.Row.BorderColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.Row.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.Row.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.Row.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.Row.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.RowSeparator.BackColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.RowSeparator.ForeColor = System.Drawing.Color.Red;
            this.gvExportacaoCampos.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.RowSeparator.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvExportacaoCampos.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvExportacaoCampos.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
            this.gvExportacaoCampos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvExportacaoCampos.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvExportacaoCampos.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvExportacaoCampos.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvExportacaoCampos.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvExportacaoCampos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gvExportacaoCampos.GridControl = this.gcExportacaoCampos;
            this.gvExportacaoCampos.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvExportacaoCampos.Name = "gvExportacaoCampos";
            this.gvExportacaoCampos.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvExportacaoCampos.OptionsBehavior.Editable = false;
            this.gvExportacaoCampos.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvExportacaoCampos.OptionsNavigation.UseTabKey = false;
            this.gvExportacaoCampos.OptionsView.EnableAppearanceEvenRow = true;
            this.gvExportacaoCampos.OptionsView.EnableAppearanceOddRow = true;
            this.gvExportacaoCampos.OptionsView.ShowAutoFilterRow = true;
            this.gvExportacaoCampos.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Id";
            this.gridColumn1.FieldName = "Id";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Código";
            this.gridColumn2.FieldName = "Codigo";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 118;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Tipo";
            this.gridColumn3.FieldName = "Tipo";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 360;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(6, 63);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(162, 16);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "Campos para exportação";
            // 
            // lblCampos
            // 
            this.lblCampos.Location = new System.Drawing.Point(204, 287);
            this.lblCampos.Name = "lblCampos";
            this.lblCampos.Size = new System.Drawing.Size(48, 13);
            this.lblCampos.TabIndex = 9;
            this.lblCampos.Text = "lblCampos";
            this.lblCampos.Visible = false;
            // 
            // sbVisualizar
            // 
            this.sbVisualizar.Image = global::UI.Properties.Resources.Consulta_copy;
            this.sbVisualizar.Location = new System.Drawing.Point(4, 287);
            this.sbVisualizar.Name = "sbVisualizar";
            this.sbVisualizar.Size = new System.Drawing.Size(183, 23);
            this.sbVisualizar.TabIndex = 6;
            this.sbVisualizar.Text = "Visualizar Layout";
            this.sbVisualizar.Click += new System.EventHandler(this.sbVisualizar_Click);
            // 
            // FormManutLayoutExportacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(541, 376);
            this.Name = "FormManutLayoutExportacao";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcExportacaoCampos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvExportacaoCampos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtDescricao;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.LabelControl lblDescricao;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        public DevExpress.XtraEditors.SimpleButton sbIncluirExpCampos;
        public DevExpress.XtraEditors.SimpleButton sbAlterarExpCampos;
        public DevExpress.XtraEditors.SimpleButton sbExcluirExpCampos;
        public DevExpress.XtraGrid.GridControl gcExportacaoCampos;
        public DevExpress.XtraGrid.Views.Grid.GridView gvExportacaoCampos;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.LabelControl lblCampos;
        private DevExpress.XtraEditors.SimpleButton sbVisualizar;

    }
}
