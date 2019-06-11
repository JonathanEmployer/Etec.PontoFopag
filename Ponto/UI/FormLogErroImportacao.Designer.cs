namespace UI
{
    partial class FormLogErroImportacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogErroImportacao));
            this.gcDadosImportacao = new DevExpress.XtraGrid.GridControl();
            this.gvDadosImportacao = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Identificador = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Tabela = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Erro = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gcDadosImportacao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDadosImportacao)).BeginInit();
            this.SuspendLayout();
            // 
            // gcDadosImportacao
            // 
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcDadosImportacao.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcDadosImportacao.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDadosImportacao.Location = new System.Drawing.Point(12, 12);
            this.gcDadosImportacao.LookAndFeel.SkinName = "Office 2010 Blue";
            this.gcDadosImportacao.LookAndFeel.UseWindowsXPTheme = true;
            this.gcDadosImportacao.MainView = this.gvDadosImportacao;
            this.gcDadosImportacao.Name = "gcDadosImportacao";
            this.gcDadosImportacao.Size = new System.Drawing.Size(832, 388);
            this.gcDadosImportacao.TabIndex = 1;
            this.gcDadosImportacao.UseEmbeddedNavigator = true;
            this.gcDadosImportacao.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDadosImportacao});
            // 
            // gvDadosImportacao
            // 
            this.gvDadosImportacao.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvDadosImportacao.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.Empty.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvDadosImportacao.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvDadosImportacao.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvDadosImportacao.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvDadosImportacao.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvDadosImportacao.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDadosImportacao.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDadosImportacao.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDadosImportacao.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvDadosImportacao.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvDadosImportacao.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvDadosImportacao.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvDadosImportacao.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvDadosImportacao.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvDadosImportacao.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvDadosImportacao.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvDadosImportacao.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDadosImportacao.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvDadosImportacao.Appearance.OddRow.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.OddRow.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvDadosImportacao.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.Preview.Options.UseFont = true;
            this.gvDadosImportacao.Appearance.Preview.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvDadosImportacao.Appearance.Row.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvDadosImportacao.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDadosImportacao.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvDadosImportacao.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvDadosImportacao.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvDadosImportacao.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvDadosImportacao.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvDadosImportacao.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvDadosImportacao.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Identificador,
            this.Tabela,
            this.Erro});
            this.gvDadosImportacao.GridControl = this.gcDadosImportacao;
            this.gvDadosImportacao.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvDadosImportacao.Name = "gvDadosImportacao";
            this.gvDadosImportacao.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvDadosImportacao.OptionsBehavior.Editable = false;
            this.gvDadosImportacao.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvDadosImportacao.OptionsNavigation.UseTabKey = false;
            this.gvDadosImportacao.OptionsView.ColumnAutoWidth = false;
            this.gvDadosImportacao.OptionsView.EnableAppearanceEvenRow = true;
            this.gvDadosImportacao.OptionsView.EnableAppearanceOddRow = true;
            this.gvDadosImportacao.OptionsView.ShowAutoFilterRow = true;
            // 
            // Identificador
            // 
            this.Identificador.Caption = "Identificador";
            this.Identificador.FieldName = "Identificador";
            this.Identificador.Name = "Identificador";
            this.Identificador.Visible = true;
            this.Identificador.VisibleIndex = 0;
            // 
            // Tabela
            // 
            this.Tabela.Caption = "Tabela";
            this.Tabela.FieldName = "Tabela";
            this.Tabela.Name = "Tabela";
            this.Tabela.Visible = true;
            this.Tabela.VisibleIndex = 1;
            this.Tabela.Width = 110;
            // 
            // Erro
            // 
            this.Erro.Caption = "Erro";
            this.Erro.FieldName = "Erro";
            this.Erro.Name = "Erro";
            this.Erro.Visible = true;
            this.Erro.VisibleIndex = 2;
            this.Erro.Width = 628;
            // 
            // FormLogErroImportacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 410);
            this.Controls.Add(this.gcDadosImportacao);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLogErroImportacao";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log Importação";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormLogErroImportacao_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.gcDadosImportacao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDadosImportacao)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraGrid.GridControl gcDadosImportacao;
        public DevExpress.XtraGrid.Views.Grid.GridView gvDadosImportacao;
        private DevExpress.XtraGrid.Columns.GridColumn Identificador;
        private DevExpress.XtraGrid.Columns.GridColumn Tabela;
        private DevExpress.XtraGrid.Columns.GridColumn Erro;

    }
}