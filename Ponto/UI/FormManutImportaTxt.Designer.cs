namespace UI
{
    partial class FormManutImportaTxt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManutImportaTxt));
            this.gcLayoutImportacao = new DevExpress.XtraGrid.GridControl();
            this.dataGridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Codigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tipo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Campo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Delimitador = new DevExpress.XtraGrid.Columns.GridColumn();
            this.sbConsultar = new DevExpress.XtraEditors.SimpleButton();
            this.sbIncluir = new DevExpress.XtraEditors.SimpleButton();
            this.sbAlterar = new DevExpress.XtraEditors.SimpleButton();
            this.sbExcluir = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnHorario = new Componentes.devexpress.cwk_DevButton();
            this.btnEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.cbEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.cbHorario = new Componentes.devexpress.cwk_DevLookup();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.btArqImportacao = new Componentes.devexpress.cwk_DevButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtCaminhoArqImportacao = new DevExpress.XtraEditors.TextEdit();
            this.rgTipoTurno = new DevExpress.XtraEditors.RadioGroup();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLayoutImportacao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbHorario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCaminhoArqImportacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoTurno.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(668, 399);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.rgTipoTurno);
            this.xtraTabPage1.Controls.Add(this.labelControl3);
            this.xtraTabPage1.Controls.Add(this.txtCaminhoArqImportacao);
            this.xtraTabPage1.Controls.Add(this.btArqImportacao);
            this.xtraTabPage1.Controls.Add(this.labelControl4);
            this.xtraTabPage1.Controls.Add(this.cbHorario);
            this.xtraTabPage1.Controls.Add(this.cbEmpresa);
            this.xtraTabPage1.Controls.Add(this.btnEmpresa);
            this.xtraTabPage1.Controls.Add(this.btnHorario);
            this.xtraTabPage1.Controls.Add(this.labelControl2);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.sbConsultar);
            this.xtraTabPage1.Controls.Add(this.sbIncluir);
            this.xtraTabPage1.Controls.Add(this.sbAlterar);
            this.xtraTabPage1.Controls.Add(this.sbExcluir);
            this.xtraTabPage1.Controls.Add(this.gcLayoutImportacao);
            this.xtraTabPage1.Size = new System.Drawing.Size(662, 393);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(605, 417);
            // 
            // sbGravar
            // 
            this.sbGravar.Image = global::UI.Properties.Resources.Importação_de_Bilhetes_copy;
            this.sbGravar.Location = new System.Drawing.Point(524, 417);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 417);
            // 
            // gcLayoutImportacao
            // 
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcLayoutImportacao.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcLayoutImportacao.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcLayoutImportacao.Location = new System.Drawing.Point(3, 85);
            this.gcLayoutImportacao.LookAndFeel.UseWindowsXPTheme = true;
            this.gcLayoutImportacao.MainView = this.dataGridView1;
            this.gcLayoutImportacao.Name = "gcLayoutImportacao";
            this.gcLayoutImportacao.Size = new System.Drawing.Size(653, 274);
            this.gcLayoutImportacao.TabIndex = 1;
            this.gcLayoutImportacao.UseEmbeddedNavigator = true;
            this.gcLayoutImportacao.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataGridView1});
            this.gcLayoutImportacao.DoubleClick += new System.EventHandler(this.gcLayoutImportacao_DoubleClick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.dataGridView1.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.dataGridView1.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.dataGridView1.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.dataGridView1.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.dataGridView1.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.dataGridView1.Appearance.Empty.Options.UseBackColor = true;
            this.dataGridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.dataGridView1.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.dataGridView1.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.dataGridView1.Appearance.EvenRow.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.EvenRow.Options.UseForeColor = true;
            this.dataGridView1.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.dataGridView1.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.dataGridView1.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.dataGridView1.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.dataGridView1.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.dataGridView1.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.dataGridView1.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.dataGridView1.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.Appearance.FilterPanel.Options.UseBackColor = true;
            this.dataGridView1.Appearance.FilterPanel.Options.UseForeColor = true;
            this.dataGridView1.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.FixedLine.Options.UseBackColor = true;
            this.dataGridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.dataGridView1.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.dataGridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.dataGridView1.Appearance.FocusedCell.Options.UseForeColor = true;
            this.dataGridView1.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.dataGridView1.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.dataGridView1.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.dataGridView1.Appearance.FocusedRow.Options.UseBackColor = true;
            this.dataGridView1.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.FocusedRow.Options.UseForeColor = true;
            this.dataGridView1.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.Appearance.FooterPanel.Options.UseBackColor = true;
            this.dataGridView1.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.FooterPanel.Options.UseForeColor = true;
            this.dataGridView1.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.GroupButton.Options.UseBackColor = true;
            this.dataGridView1.Appearance.GroupButton.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.Appearance.GroupFooter.Options.UseBackColor = true;
            this.dataGridView1.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.GroupFooter.Options.UseForeColor = true;
            this.dataGridView1.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.dataGridView1.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.dataGridView1.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.Appearance.GroupPanel.Options.UseBackColor = true;
            this.dataGridView1.Appearance.GroupPanel.Options.UseForeColor = true;
            this.dataGridView1.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.Appearance.GroupRow.Options.UseForeColor = true;
            this.dataGridView1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.dataGridView1.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.dataGridView1.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.dataGridView1.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.dataGridView1.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.dataGridView1.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.dataGridView1.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.dataGridView1.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.dataGridView1.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.dataGridView1.Appearance.OddRow.Options.UseBackColor = true;
            this.dataGridView1.Appearance.OddRow.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.OddRow.Options.UseForeColor = true;
            this.dataGridView1.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.dataGridView1.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.Preview.Options.UseFont = true;
            this.dataGridView1.Appearance.Preview.Options.UseForeColor = true;
            this.dataGridView1.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.dataGridView1.Appearance.Row.Options.UseBackColor = true;
            this.dataGridView1.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.dataGridView1.Appearance.RowSeparator.Options.UseBackColor = true;
            this.dataGridView1.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.dataGridView1.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.dataGridView1.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.dataGridView1.Appearance.SelectedRow.Options.UseBackColor = true;
            this.dataGridView1.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.dataGridView1.Appearance.SelectedRow.Options.UseForeColor = true;
            this.dataGridView1.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.dataGridView1.Appearance.TopNewRow.Options.UseBackColor = true;
            this.dataGridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ID,
            this.Codigo,
            this.tipo,
            this.Campo,
            this.Delimitador});
            this.dataGridView1.GridControl = this.gcLayoutImportacao;
            this.dataGridView1.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.OptionsBehavior.AllowIncrementalSearch = true;
            this.dataGridView1.OptionsBehavior.Editable = false;
            this.dataGridView1.OptionsBehavior.FocusLeaveOnTab = true;
            this.dataGridView1.OptionsCustomization.AllowColumnResizing = false;
            this.dataGridView1.OptionsNavigation.UseTabKey = false;
            this.dataGridView1.OptionsView.ColumnAutoWidth = false;
            this.dataGridView1.OptionsView.EnableAppearanceEvenRow = true;
            this.dataGridView1.OptionsView.EnableAppearanceOddRow = true;
            this.dataGridView1.CustomDrawGroupPanel += new DevExpress.XtraGrid.Views.Base.CustomDrawEventHandler(this.dataGridView1_CustomDrawGroupPanel);
            // 
            // ID
            // 
            this.ID.AppearanceHeader.Options.UseTextOptions = true;
            this.ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ID.Caption = "ID";
            this.ID.FieldName = "ID";
            this.ID.Name = "ID";
            // 
            // Codigo
            // 
            this.Codigo.AppearanceHeader.Options.UseTextOptions = true;
            this.Codigo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Codigo.Caption = "Código";
            this.Codigo.FieldName = "Codigo";
            this.Codigo.Name = "Codigo";
            this.Codigo.Visible = true;
            this.Codigo.VisibleIndex = 0;
            // 
            // tipo
            // 
            this.tipo.AppearanceHeader.Options.UseTextOptions = true;
            this.tipo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tipo.Caption = "Tipo do Campo";
            this.tipo.FieldName = "Tipo";
            this.tipo.Name = "tipo";
            this.tipo.Visible = true;
            this.tipo.VisibleIndex = 1;
            this.tipo.Width = 94;
            // 
            // Campo
            // 
            this.Campo.AppearanceHeader.Options.UseTextOptions = true;
            this.Campo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Campo.Caption = "Campo";
            this.Campo.FieldName = "Campo";
            this.Campo.Name = "Campo";
            this.Campo.Visible = true;
            this.Campo.VisibleIndex = 3;
            this.Campo.Width = 369;
            // 
            // Delimitador
            // 
            this.Delimitador.AppearanceHeader.Options.UseTextOptions = true;
            this.Delimitador.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Delimitador.Caption = "Delimitador";
            this.Delimitador.FieldName = "Delimitador";
            this.Delimitador.Name = "Delimitador";
            this.Delimitador.Visible = true;
            this.Delimitador.VisibleIndex = 2;
            this.Delimitador.Width = 91;
            // 
            // sbConsultar
            // 
            this.sbConsultar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbConsultar.Image = global::UI.Properties.Resources.Consulta_copy;
            this.sbConsultar.ImageIndex = 3;
            this.sbConsultar.Location = new System.Drawing.Point(338, 364);
            this.sbConsultar.Name = "sbConsultar";
            this.sbConsultar.Size = new System.Drawing.Size(75, 23);
            this.sbConsultar.TabIndex = 5;
            this.sbConsultar.Text = "&Consultar";
            this.sbConsultar.Click += new System.EventHandler(this.sbConsultar_Click);
            // 
            // sbIncluir
            // 
            this.sbIncluir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbIncluir.Image = ((System.Drawing.Image)(resources.GetObject("sbIncluir.Image")));
            this.sbIncluir.ImageIndex = 4;
            this.sbIncluir.Location = new System.Drawing.Point(419, 364);
            this.sbIncluir.Name = "sbIncluir";
            this.sbIncluir.Size = new System.Drawing.Size(75, 23);
            this.sbIncluir.TabIndex = 6;
            this.sbIncluir.Text = "&Incluir";
            this.sbIncluir.Click += new System.EventHandler(this.sbIncluir_Click);
            // 
            // sbAlterar
            // 
            this.sbAlterar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbAlterar.Image = ((System.Drawing.Image)(resources.GetObject("sbAlterar.Image")));
            this.sbAlterar.ImageIndex = 5;
            this.sbAlterar.Location = new System.Drawing.Point(500, 364);
            this.sbAlterar.Name = "sbAlterar";
            this.sbAlterar.Size = new System.Drawing.Size(75, 23);
            this.sbAlterar.TabIndex = 7;
            this.sbAlterar.Text = "&Alterar";
            this.sbAlterar.Click += new System.EventHandler(this.sbAlterar_Click);
            // 
            // sbExcluir
            // 
            this.sbExcluir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbExcluir.Image = ((System.Drawing.Image)(resources.GetObject("sbExcluir.Image")));
            this.sbExcluir.ImageIndex = 6;
            this.sbExcluir.Location = new System.Drawing.Point(581, 364);
            this.sbExcluir.Name = "sbExcluir";
            this.sbExcluir.ShowToolTips = false;
            this.sbExcluir.Size = new System.Drawing.Size(75, 23);
            this.sbExcluir.TabIndex = 8;
            this.sbExcluir.Text = "&Excluir";
            this.sbExcluir.Click += new System.EventHandler(this.sbExcluir_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(41, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(45, 13);
            this.labelControl1.TabIndex = 12;
            this.labelControl1.Text = "Empresa:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(220, 32);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(39, 13);
            this.labelControl2.TabIndex = 13;
            this.labelControl2.Text = "Horário:";
            // 
            // btnHorario
            // 
            this.btnHorario.Location = new System.Drawing.Point(632, 29);
            this.btnHorario.Name = "btnHorario";
            this.btnHorario.Size = new System.Drawing.Size(24, 20);
            this.btnHorario.TabIndex = 18;
            this.btnHorario.TabStop = false;
            this.btnHorario.Text = "...";
            this.btnHorario.Click += new System.EventHandler(this.btnHorario_Click);
            // 
            // btnEmpresa
            // 
            this.btnEmpresa.Location = new System.Drawing.Point(632, 3);
            this.btnEmpresa.Name = "btnEmpresa";
            this.btnEmpresa.Size = new System.Drawing.Size(24, 20);
            this.btnEmpresa.TabIndex = 19;
            this.btnEmpresa.TabStop = false;
            this.btnEmpresa.Text = "...";
            this.btnEmpresa.Click += new System.EventHandler(this.btnEmpresa_Click);
            // 
            // cbEmpresa
            // 
            this.cbEmpresa.ButtonLookup = null;
            this.cbEmpresa.EditValue = 0;
            this.cbEmpresa.Key = System.Windows.Forms.Keys.F5;
            this.cbEmpresa.Location = new System.Drawing.Point(92, 3);
            this.cbEmpresa.Name = "cbEmpresa";
            this.cbEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código")});
            this.cbEmpresa.Properties.DisplayMember = "nome";
            this.cbEmpresa.Properties.NullText = "";
            this.cbEmpresa.Properties.ValueMember = "id";
            this.cbEmpresa.Size = new System.Drawing.Size(534, 20);
            this.cbEmpresa.TabIndex = 20;
            this.cbEmpresa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbEmpresa_KeyDown);
            // 
            // cbHorario
            // 
            this.cbHorario.ButtonLookup = null;
            this.cbHorario.EditValue = 0;
            this.cbHorario.Key = System.Windows.Forms.Keys.F5;
            this.cbHorario.Location = new System.Drawing.Point(265, 29);
            this.cbHorario.Name = "cbHorario";
            this.cbHorario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbHorario.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Nome"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código")});
            this.cbHorario.Properties.DisplayMember = "descricao";
            this.cbHorario.Properties.NullText = "";
            this.cbHorario.Properties.ValueMember = "id";
            this.cbHorario.Size = new System.Drawing.Size(361, 20);
            this.cbHorario.TabIndex = 21;
            this.cbHorario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbHorario_KeyDown);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(3, 58);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(83, 13);
            this.labelControl4.TabIndex = 23;
            this.labelControl4.Text = "Arq. Importação:";
            // 
            // btArqImportacao
            // 
            this.btArqImportacao.Location = new System.Drawing.Point(632, 55);
            this.btArqImportacao.Name = "btArqImportacao";
            this.btArqImportacao.Size = new System.Drawing.Size(24, 20);
            this.btArqImportacao.TabIndex = 25;
            this.btArqImportacao.TabStop = false;
            this.btArqImportacao.Text = "...";
            this.btArqImportacao.Click += new System.EventHandler(this.btArqImportacao_Click);
            // 
            // txtCaminhoArqImportacao
            // 
            this.txtCaminhoArqImportacao.Location = new System.Drawing.Point(92, 55);
            this.txtCaminhoArqImportacao.Name = "txtCaminhoArqImportacao";
            this.txtCaminhoArqImportacao.Size = new System.Drawing.Size(534, 20);
            this.txtCaminhoArqImportacao.TabIndex = 26;
            this.txtCaminhoArqImportacao.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCaminhoArqImportacao_KeyDown);
            // 
            // rgTipoTurno
            // 
            this.rgTipoTurno.EditValue = true;
            this.rgTipoTurno.Location = new System.Drawing.Point(92, 29);
            this.rgTipoTurno.Name = "rgTipoTurno";
            this.rgTipoTurno.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(true, "Normal"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(false, "Flexível")});
            this.rgTipoTurno.Size = new System.Drawing.Size(122, 20);
            this.rgTipoTurno.TabIndex = 27;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(16, 32);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(70, 13);
            this.labelControl3.TabIndex = 28;
            this.labelControl3.Text = "Tipo do Turno:";
            // 
            // FormManutImportaTxt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(692, 452);
            this.Name = "FormManutImportaTxt";
            this.Text = "Importação de Arquivos Texto";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLayoutImportacao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbHorario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCaminhoArqImportacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoTurno.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraGrid.GridControl gcLayoutImportacao;
        public DevExpress.XtraGrid.Views.Grid.GridView dataGridView1;
        public DevExpress.XtraEditors.SimpleButton sbConsultar;
        public DevExpress.XtraEditors.SimpleButton sbIncluir;
        public DevExpress.XtraEditors.SimpleButton sbAlterar;
        public DevExpress.XtraEditors.SimpleButton sbExcluir;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private Componentes.devexpress.cwk_DevButton btnEmpresa;
        private Componentes.devexpress.cwk_DevButton btnHorario;
        private Componentes.devexpress.cwk_DevLookup cbHorario;
        private Componentes.devexpress.cwk_DevLookup cbEmpresa;
        private Componentes.devexpress.cwk_DevButton btArqImportacao;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private DevExpress.XtraEditors.TextEdit txtCaminhoArqImportacao;
        private DevExpress.XtraEditors.RadioGroup rgTipoTurno;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraGrid.Columns.GridColumn ID;
        private DevExpress.XtraGrid.Columns.GridColumn Codigo;
        private DevExpress.XtraGrid.Columns.GridColumn tipo;
        private DevExpress.XtraGrid.Columns.GridColumn Campo;
        private DevExpress.XtraGrid.Columns.GridColumn Delimitador;

    }
}
