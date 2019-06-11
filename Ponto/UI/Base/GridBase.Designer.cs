namespace UI.Base
{
    partial class GridBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridBase));
            this.txtLocalizar = new DevExpress.XtraEditors.TextEdit();
            this.btFiltro = new DevExpress.XtraEditors.SimpleButton();
            this.sbFechar = new DevExpress.XtraEditors.SimpleButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sbExcluir = new DevExpress.XtraEditors.SimpleButton();
            this.sbAlterar = new DevExpress.XtraEditors.SimpleButton();
            this.sbIncluir = new DevExpress.XtraEditors.SimpleButton();
            this.sbConsultar = new DevExpress.XtraEditors.SimpleButton();
            this.sbAjudar = new DevExpress.XtraEditors.SimpleButton();
            this.sbSelecionar = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.dataGridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLocalizar
            // 
            this.txtLocalizar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalizar.Location = new System.Drawing.Point(12, 11);
            this.txtLocalizar.Name = "txtLocalizar";
            this.txtLocalizar.Size = new System.Drawing.Size(787, 20);
            this.txtLocalizar.TabIndex = 0;
            this.txtLocalizar.Visible = false;
            this.txtLocalizar.TextChanged += new System.EventHandler(this.txtLocalizar_TextChanged);
            this.txtLocalizar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLocalizar_KeyDown);
            // 
            // btFiltro
            // 
            this.btFiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btFiltro.Location = new System.Drawing.Point(805, 8);
            this.btFiltro.Name = "btFiltro";
            this.btFiltro.Size = new System.Drawing.Size(75, 23);
            this.btFiltro.TabIndex = 1;
            this.btFiltro.TabStop = false;
            this.btFiltro.Text = "Filtar";
            this.btFiltro.Visible = false;
            // 
            // sbFechar
            // 
            this.sbFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbFechar.ImageIndex = 7;
            this.sbFechar.ImageList = this.imageList1;
            this.sbFechar.Location = new System.Drawing.Point(805, 431);
            this.sbFechar.Name = "sbFechar";
            this.sbFechar.Size = new System.Drawing.Size(75, 23);
            this.sbFechar.TabIndex = 7;
            this.sbFechar.Text = "&Fechar";
            this.sbFechar.Click += new System.EventHandler(this.sbFechar_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Logo.ico");
            this.imageList1.Images.SetKeyName(1, "Help copy.ico");
            this.imageList1.Images.SetKeyName(2, "Selecionar copy.ico");
            this.imageList1.Images.SetKeyName(3, "Consulta copy.ico");
            this.imageList1.Images.SetKeyName(4, "Inserir copy.ico");
            this.imageList1.Images.SetKeyName(5, "Alterar copy.ico");
            this.imageList1.Images.SetKeyName(6, "Excluir copy.ico");
            this.imageList1.Images.SetKeyName(7, "Fechar.ico");
            // 
            // sbExcluir
            // 
            this.sbExcluir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbExcluir.ImageIndex = 6;
            this.sbExcluir.ImageList = this.imageList1;
            this.sbExcluir.Location = new System.Drawing.Point(805, 402);
            this.sbExcluir.Name = "sbExcluir";
            this.sbExcluir.Size = new System.Drawing.Size(75, 23);
            this.sbExcluir.TabIndex = 4;
            this.sbExcluir.Text = "&Excluir";
            this.sbExcluir.Click += new System.EventHandler(this.sbExcluir_Click);
            // 
            // sbAlterar
            // 
            this.sbAlterar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbAlterar.ImageIndex = 5;
            this.sbAlterar.ImageList = this.imageList1;
            this.sbAlterar.Location = new System.Drawing.Point(724, 402);
            this.sbAlterar.Name = "sbAlterar";
            this.sbAlterar.Size = new System.Drawing.Size(75, 23);
            this.sbAlterar.TabIndex = 3;
            this.sbAlterar.Text = "&Alterar";
            this.sbAlterar.Click += new System.EventHandler(this.sbAlterar_Click);
            // 
            // sbIncluir
            // 
            this.sbIncluir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbIncluir.ImageIndex = 4;
            this.sbIncluir.ImageList = this.imageList1;
            this.sbIncluir.Location = new System.Drawing.Point(643, 402);
            this.sbIncluir.Name = "sbIncluir";
            this.sbIncluir.Size = new System.Drawing.Size(75, 23);
            this.sbIncluir.TabIndex = 2;
            this.sbIncluir.Text = "&Incluir";
            this.sbIncluir.Click += new System.EventHandler(this.sbIncluir_Click);
            // 
            // sbConsultar
            // 
            this.sbConsultar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbConsultar.ImageIndex = 3;
            this.sbConsultar.ImageList = this.imageList1;
            this.sbConsultar.Location = new System.Drawing.Point(562, 402);
            this.sbConsultar.Name = "sbConsultar";
            this.sbConsultar.Size = new System.Drawing.Size(75, 23);
            this.sbConsultar.TabIndex = 1;
            this.sbConsultar.Text = "&Consultar";
            this.sbConsultar.Click += new System.EventHandler(this.sbConsultar_Click);
            // 
            // sbAjudar
            // 
            this.sbAjudar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sbAjudar.ImageIndex = 1;
            this.sbAjudar.ImageList = this.imageList1;
            this.sbAjudar.Location = new System.Drawing.Point(12, 431);
            this.sbAjudar.Name = "sbAjudar";
            this.sbAjudar.Size = new System.Drawing.Size(75, 23);
            this.sbAjudar.TabIndex = 5;
            this.sbAjudar.Text = "A&juda";
            this.sbAjudar.Click += new System.EventHandler(this.sbAjudar_Click);
            // 
            // sbSelecionar
            // 
            this.sbSelecionar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sbSelecionar.ImageIndex = 2;
            this.sbSelecionar.ImageList = this.imageList1;
            this.sbSelecionar.Location = new System.Drawing.Point(93, 431);
            this.sbSelecionar.Name = "sbSelecionar";
            this.sbSelecionar.Size = new System.Drawing.Size(83, 23);
            this.sbSelecionar.TabIndex = 6;
            this.sbSelecionar.Text = "&Selecionar";
            this.sbSelecionar.Click += new System.EventHandler(this.sbSelecionar_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gridControl1.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gridControl1.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gridControl1.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gridControl1.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gridControl1.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gridControl1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridControl1.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridControl1.Location = new System.Drawing.Point(12, 8);
            this.gridControl1.LookAndFeel.SkinName = "Office 2010 Blue";
            this.gridControl1.LookAndFeel.UseWindowsXPTheme = true;
            this.gridControl1.MainView = this.dataGridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(868, 388);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.UseEmbeddedNavigator = true;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataGridView1});
            this.gridControl1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.gridControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            this.gridControl1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridView1_KeyPress);
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
            this.dataGridView1.GridControl = this.gridControl1;
            this.dataGridView1.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.OptionsBehavior.AllowIncrementalSearch = true;
            this.dataGridView1.OptionsBehavior.Editable = false;
            this.dataGridView1.OptionsBehavior.FocusLeaveOnTab = true;
            this.dataGridView1.OptionsNavigation.UseTabKey = false;
            this.dataGridView1.OptionsView.ColumnAutoWidth = false;
            this.dataGridView1.OptionsView.EnableAppearanceEvenRow = true;
            this.dataGridView1.OptionsView.EnableAppearanceOddRow = true;
            this.dataGridView1.OptionsView.ShowAutoFilterRow = true;
            this.dataGridView1.CustomDrawGroupPanel += new DevExpress.XtraGrid.Views.Base.CustomDrawEventHandler(this.dataGridView1_CustomDrawGroupPanel);
            this.dataGridView1.CustomRowFilter += new DevExpress.XtraGrid.Views.Base.RowFilterEventHandler(this.dataGridView1_CustomRowFilter);
            // 
            // GridBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 466);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.sbSelecionar);
            this.Controls.Add(this.sbAjudar);
            this.Controls.Add(this.sbConsultar);
            this.Controls.Add(this.sbIncluir);
            this.Controls.Add(this.sbAlterar);
            this.Controls.Add(this.sbExcluir);
            this.Controls.Add(this.sbFechar);
            this.Controls.Add(this.btFiltro);
            this.Controls.Add(this.txtLocalizar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "GridBase";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GridBase";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GridBase_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GridBase_FormClosed);
            this.Load += new System.EventHandler(this.GridBase_Load);
            this.Shown += new System.EventHandler(this.GridBase_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridBase_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.TextEdit txtLocalizar;
        public DevExpress.XtraEditors.SimpleButton btFiltro;
        public DevExpress.XtraEditors.SimpleButton sbFechar;
        public DevExpress.XtraEditors.SimpleButton sbExcluir;
        public DevExpress.XtraEditors.SimpleButton sbAlterar;
        public DevExpress.XtraEditors.SimpleButton sbIncluir;
        public DevExpress.XtraEditors.SimpleButton sbConsultar;
        public DevExpress.XtraEditors.SimpleButton sbAjudar;
        public DevExpress.XtraEditors.SimpleButton sbSelecionar;
        private System.Windows.Forms.ImageList imageList1;
        public DevExpress.XtraGrid.GridControl gridControl1;
        public DevExpress.XtraGrid.Views.Grid.GridView dataGridView1;
    }
}