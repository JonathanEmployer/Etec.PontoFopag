namespace UI
{
    partial class FormManutEmpresaCw_Usuario
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
            this.gcUsuariosNaoAdicionados = new DevExpress.XtraGrid.GridControl();
            this.gvUsuariosNaoAdicionados = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colUNACodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUNALogin = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUNANome = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUsuariosAdicionados = new DevExpress.XtraGrid.GridControl();
            this.gvUsuariosAdicionados = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colUACodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUALogin = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUANome = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sbAdicionarTodos = new DevExpress.XtraEditors.SimpleButton();
            this.sbAdicionar = new DevExpress.XtraEditors.SimpleButton();
            this.sbRemoverTodos = new DevExpress.XtraEditors.SimpleButton();
            this.sbRemover = new DevExpress.XtraEditors.SimpleButton();
            this.txtEmpresa = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcUsuariosNaoAdicionados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUsuariosNaoAdicionados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcUsuariosAdicionados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUsuariosAdicionados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpresa.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(868, 449);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.labelControl3);
            this.xtraTabPage1.Controls.Add(this.labelControl2);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.txtEmpresa);
            this.xtraTabPage1.Controls.Add(this.sbRemover);
            this.xtraTabPage1.Controls.Add(this.sbRemoverTodos);
            this.xtraTabPage1.Controls.Add(this.sbAdicionar);
            this.xtraTabPage1.Controls.Add(this.sbAdicionarTodos);
            this.xtraTabPage1.Controls.Add(this.gcUsuariosAdicionados);
            this.xtraTabPage1.Controls.Add(this.gcUsuariosNaoAdicionados);
            this.xtraTabPage1.Size = new System.Drawing.Size(859, 440);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(805, 467);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(724, 467);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 467);
            // 
            // gcUsuariosNaoAdicionados
            // 
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.Name = "";
            this.gcUsuariosNaoAdicionados.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcUsuariosNaoAdicionados.Location = new System.Drawing.Point(3, 62);
            this.gcUsuariosNaoAdicionados.LookAndFeel.UseWindowsXPTheme = true;
            this.gcUsuariosNaoAdicionados.MainView = this.gvUsuariosNaoAdicionados;
            this.gcUsuariosNaoAdicionados.Name = "gcUsuariosNaoAdicionados";
            this.gcUsuariosNaoAdicionados.Size = new System.Drawing.Size(390, 373);
            this.gcUsuariosNaoAdicionados.TabIndex = 2;
            this.gcUsuariosNaoAdicionados.UseEmbeddedNavigator = true;
            this.gcUsuariosNaoAdicionados.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvUsuariosNaoAdicionados});
            // 
            // gvUsuariosNaoAdicionados
            // 
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.Empty.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosNaoAdicionados.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvUsuariosNaoAdicionados.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvUsuariosNaoAdicionados.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvUsuariosNaoAdicionados.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosNaoAdicionados.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosNaoAdicionados.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosNaoAdicionados.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosNaoAdicionados.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosNaoAdicionados.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosNaoAdicionados.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvUsuariosNaoAdicionados.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosNaoAdicionados.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosNaoAdicionados.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvUsuariosNaoAdicionados.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvUsuariosNaoAdicionados.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosNaoAdicionados.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosNaoAdicionados.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosNaoAdicionados.Appearance.OddRow.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.OddRow.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvUsuariosNaoAdicionados.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.Preview.Options.UseFont = true;
            this.gvUsuariosNaoAdicionados.Appearance.Preview.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosNaoAdicionados.Appearance.Row.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvUsuariosNaoAdicionados.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosNaoAdicionados.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosNaoAdicionados.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvUsuariosNaoAdicionados.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvUsuariosNaoAdicionados.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvUsuariosNaoAdicionados.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colUNACodigo,
            this.colUNALogin,
            this.colUNANome});
            this.gvUsuariosNaoAdicionados.GridControl = this.gcUsuariosNaoAdicionados;
            this.gvUsuariosNaoAdicionados.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvUsuariosNaoAdicionados.Name = "gvUsuariosNaoAdicionados";
            this.gvUsuariosNaoAdicionados.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvUsuariosNaoAdicionados.OptionsBehavior.Editable = false;
            this.gvUsuariosNaoAdicionados.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvUsuariosNaoAdicionados.OptionsNavigation.UseTabKey = false;
            this.gvUsuariosNaoAdicionados.OptionsSelection.MultiSelect = true;
            this.gvUsuariosNaoAdicionados.OptionsView.ColumnAutoWidth = false;
            this.gvUsuariosNaoAdicionados.OptionsView.EnableAppearanceEvenRow = true;
            this.gvUsuariosNaoAdicionados.OptionsView.EnableAppearanceOddRow = true;
            this.gvUsuariosNaoAdicionados.OptionsView.ShowAutoFilterRow = true;
            this.gvUsuariosNaoAdicionados.OptionsView.ShowGroupPanel = false;
            // 
            // colUNACodigo
            // 
            this.colUNACodigo.Caption = "Código";
            this.colUNACodigo.FieldName = "Codigo";
            this.colUNACodigo.Name = "colUNACodigo";
            this.colUNACodigo.Visible = true;
            this.colUNACodigo.VisibleIndex = 0;
            this.colUNACodigo.Width = 70;
            // 
            // colUNALogin
            // 
            this.colUNALogin.Caption = "Login";
            this.colUNALogin.FieldName = "Login";
            this.colUNALogin.Name = "colUNALogin";
            this.colUNALogin.Visible = true;
            this.colUNALogin.VisibleIndex = 1;
            this.colUNALogin.Width = 90;
            // 
            // colUNANome
            // 
            this.colUNANome.Caption = "Nome";
            this.colUNANome.FieldName = "Nome";
            this.colUNANome.Name = "colUNANome";
            this.colUNANome.Visible = true;
            this.colUNANome.VisibleIndex = 2;
            this.colUNANome.Width = 208;
            // 
            // gcUsuariosAdicionados
            // 
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcUsuariosAdicionados.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcUsuariosAdicionados.EmbeddedNavigator.Name = "";
            this.gcUsuariosAdicionados.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcUsuariosAdicionados.Location = new System.Drawing.Point(466, 62);
            this.gcUsuariosAdicionados.LookAndFeel.UseWindowsXPTheme = true;
            this.gcUsuariosAdicionados.MainView = this.gvUsuariosAdicionados;
            this.gcUsuariosAdicionados.Name = "gcUsuariosAdicionados";
            this.gcUsuariosAdicionados.Size = new System.Drawing.Size(390, 373);
            this.gcUsuariosAdicionados.TabIndex = 7;
            this.gcUsuariosAdicionados.UseEmbeddedNavigator = true;
            this.gcUsuariosAdicionados.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvUsuariosAdicionados});
            // 
            // gvUsuariosAdicionados
            // 
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.Empty.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosAdicionados.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvUsuariosAdicionados.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvUsuariosAdicionados.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvUsuariosAdicionados.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosAdicionados.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosAdicionados.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosAdicionados.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosAdicionados.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosAdicionados.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosAdicionados.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvUsuariosAdicionados.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosAdicionados.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosAdicionados.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvUsuariosAdicionados.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvUsuariosAdicionados.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosAdicionados.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosAdicionados.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvUsuariosAdicionados.Appearance.OddRow.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.OddRow.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvUsuariosAdicionados.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.Preview.Options.UseFont = true;
            this.gvUsuariosAdicionados.Appearance.Preview.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvUsuariosAdicionados.Appearance.Row.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvUsuariosAdicionados.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosAdicionados.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvUsuariosAdicionados.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvUsuariosAdicionados.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvUsuariosAdicionados.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvUsuariosAdicionados.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvUsuariosAdicionados.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colUACodigo,
            this.colUALogin,
            this.colUANome});
            this.gvUsuariosAdicionados.GridControl = this.gcUsuariosAdicionados;
            this.gvUsuariosAdicionados.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvUsuariosAdicionados.Name = "gvUsuariosAdicionados";
            this.gvUsuariosAdicionados.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvUsuariosAdicionados.OptionsBehavior.Editable = false;
            this.gvUsuariosAdicionados.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvUsuariosAdicionados.OptionsNavigation.UseTabKey = false;
            this.gvUsuariosAdicionados.OptionsSelection.MultiSelect = true;
            this.gvUsuariosAdicionados.OptionsView.ColumnAutoWidth = false;
            this.gvUsuariosAdicionados.OptionsView.EnableAppearanceEvenRow = true;
            this.gvUsuariosAdicionados.OptionsView.EnableAppearanceOddRow = true;
            this.gvUsuariosAdicionados.OptionsView.ShowAutoFilterRow = true;
            this.gvUsuariosAdicionados.OptionsView.ShowGroupPanel = false;
            // 
            // colUACodigo
            // 
            this.colUACodigo.Caption = "Código";
            this.colUACodigo.FieldName = "Codigo";
            this.colUACodigo.Name = "colUACodigo";
            this.colUACodigo.Visible = true;
            this.colUACodigo.VisibleIndex = 0;
            this.colUACodigo.Width = 70;
            // 
            // colUALogin
            // 
            this.colUALogin.Caption = "Login";
            this.colUALogin.FieldName = "Login";
            this.colUALogin.Name = "colUALogin";
            this.colUALogin.Visible = true;
            this.colUALogin.VisibleIndex = 1;
            this.colUALogin.Width = 90;
            // 
            // colUANome
            // 
            this.colUANome.Caption = "Nome";
            this.colUANome.FieldName = "Nome";
            this.colUANome.Name = "colUANome";
            this.colUANome.Visible = true;
            this.colUANome.VisibleIndex = 2;
            this.colUANome.Width = 208;
            // 
            // sbAdicionarTodos
            // 
            this.sbAdicionarTodos.Location = new System.Drawing.Point(399, 124);
            this.sbAdicionarTodos.Name = "sbAdicionarTodos";
            this.sbAdicionarTodos.Size = new System.Drawing.Size(61, 23);
            this.sbAdicionarTodos.TabIndex = 3;
            this.sbAdicionarTodos.Text = ">>";
            this.sbAdicionarTodos.Click += new System.EventHandler(this.sbAdicionarTodos_Click);
            // 
            // sbAdicionar
            // 
            this.sbAdicionar.Location = new System.Drawing.Point(399, 153);
            this.sbAdicionar.Name = "sbAdicionar";
            this.sbAdicionar.Size = new System.Drawing.Size(61, 23);
            this.sbAdicionar.TabIndex = 4;
            this.sbAdicionar.Text = ">";
            this.sbAdicionar.Click += new System.EventHandler(this.sbAdicionar_Click);
            // 
            // sbRemoverTodos
            // 
            this.sbRemoverTodos.Location = new System.Drawing.Point(399, 233);
            this.sbRemoverTodos.Name = "sbRemoverTodos";
            this.sbRemoverTodos.Size = new System.Drawing.Size(61, 23);
            this.sbRemoverTodos.TabIndex = 5;
            this.sbRemoverTodos.Text = "<<";
            this.sbRemoverTodos.Click += new System.EventHandler(this.sbRemoverTodos_Click);
            // 
            // sbRemover
            // 
            this.sbRemover.Location = new System.Drawing.Point(399, 262);
            this.sbRemover.Name = "sbRemover";
            this.sbRemover.Size = new System.Drawing.Size(61, 23);
            this.sbRemover.TabIndex = 6;
            this.sbRemover.Text = "<";
            this.sbRemover.Click += new System.EventHandler(this.sbRemover_Click);
            // 
            // txtEmpresa
            // 
            this.txtEmpresa.Location = new System.Drawing.Point(54, 6);
            this.txtEmpresa.Name = "txtEmpresa";
            this.txtEmpresa.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtEmpresa.Properties.Appearance.Options.UseBackColor = true;
            this.txtEmpresa.Properties.ReadOnly = true;
            this.txtEmpresa.Size = new System.Drawing.Size(802, 20);
            this.txtEmpresa.TabIndex = 1;
            this.txtEmpresa.TabStop = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(3, 9);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(45, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Empresa:";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(3, 33);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(96, 23);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "Bloqueados";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(466, 33);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(80, 23);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "Liberados";
            // 
            // FormManutEmpresaCw_Usuario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 502);
            this.Name = "FormManutEmpresaCw_Usuario";
            this.Shown += new System.EventHandler(this.FormManutEmpresaCw_Usuario_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcUsuariosNaoAdicionados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUsuariosNaoAdicionados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcUsuariosAdicionados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvUsuariosAdicionados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEmpresa.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraGrid.GridControl gcUsuariosAdicionados;
        public DevExpress.XtraGrid.Views.Grid.GridView gvUsuariosAdicionados;
        public DevExpress.XtraGrid.GridControl gcUsuariosNaoAdicionados;
        public DevExpress.XtraGrid.Views.Grid.GridView gvUsuariosNaoAdicionados;
        private DevExpress.XtraEditors.SimpleButton sbRemover;
        private DevExpress.XtraEditors.SimpleButton sbRemoverTodos;
        private DevExpress.XtraEditors.SimpleButton sbAdicionar;
        private DevExpress.XtraEditors.SimpleButton sbAdicionarTodos;
        private DevExpress.XtraGrid.Columns.GridColumn colUNACodigo;
        private DevExpress.XtraGrid.Columns.GridColumn colUNALogin;
        private DevExpress.XtraGrid.Columns.GridColumn colUNANome;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtEmpresa;
        private DevExpress.XtraGrid.Columns.GridColumn colUACodigo;
        private DevExpress.XtraGrid.Columns.GridColumn colUALogin;
        private DevExpress.XtraGrid.Columns.GridColumn colUANome;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}
