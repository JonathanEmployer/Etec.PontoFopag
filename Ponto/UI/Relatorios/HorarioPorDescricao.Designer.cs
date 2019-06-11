namespace UI.Relatorios
{
    partial class HorarioPorDescricao
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gcHorarios = new DevExpress.XtraGrid.GridControl();
            this.gvHorarios = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColunaID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColunaDescricao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColunaTipo = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHorarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHorarios)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 195);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 191);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(649, 191);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(570, 191);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(712, 173);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelControl1);
            this.tabPage1.Controls.Add(this.gcHorarios);
            this.tabPage1.Size = new System.Drawing.Size(703, 164);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(17, 7);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(69, 23);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Horários";
            // 
            // gcHorarios
            // 
            this.gcHorarios.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gcHorarios.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcHorarios.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gcHorarios.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcHorarios.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gcHorarios.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcHorarios.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gcHorarios.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcHorarios.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcHorarios.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcHorarios.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcHorarios.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcHorarios.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcHorarios.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcHorarios.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcHorarios.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcHorarios.EmbeddedNavigator.Name = "";
            this.gcHorarios.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcHorarios.Location = new System.Drawing.Point(17, 36);
            this.gcHorarios.MainView = this.gvHorarios;
            this.gcHorarios.Name = "gcHorarios";
            this.gcHorarios.Size = new System.Drawing.Size(669, 111);
            this.gcHorarios.TabIndex = 3;
            this.gcHorarios.UseEmbeddedNavigator = true;
            this.gcHorarios.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvHorarios});
            // 
            // gvHorarios
            // 
            this.gvHorarios.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvHorarios.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvHorarios.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvHorarios.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvHorarios.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvHorarios.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvHorarios.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvHorarios.Appearance.Empty.Options.UseBackColor = true;
            this.gvHorarios.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvHorarios.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvHorarios.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvHorarios.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvHorarios.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvHorarios.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvHorarios.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvHorarios.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvHorarios.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvHorarios.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvHorarios.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvHorarios.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvHorarios.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvHorarios.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvHorarios.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvHorarios.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvHorarios.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvHorarios.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvHorarios.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvHorarios.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvHorarios.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvHorarios.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvHorarios.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvHorarios.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvHorarios.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvHorarios.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvHorarios.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvHorarios.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvHorarios.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvHorarios.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvHorarios.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvHorarios.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvHorarios.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvHorarios.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvHorarios.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvHorarios.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvHorarios.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvHorarios.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvHorarios.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvHorarios.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvHorarios.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvHorarios.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvHorarios.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvHorarios.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvHorarios.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvHorarios.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvHorarios.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvHorarios.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvHorarios.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvHorarios.Appearance.OddRow.Options.UseBackColor = true;
            this.gvHorarios.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.OddRow.Options.UseForeColor = true;
            this.gvHorarios.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvHorarios.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.Preview.Options.UseFont = true;
            this.gvHorarios.Appearance.Preview.Options.UseForeColor = true;
            this.gvHorarios.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvHorarios.Appearance.Row.Options.UseBackColor = true;
            this.gvHorarios.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvHorarios.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvHorarios.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvHorarios.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvHorarios.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvHorarios.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvHorarios.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvHorarios.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvHorarios.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvHorarios.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvHorarios.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ColunaID,
            this.ColunaDescricao,
            this.ColunaTipo});
            this.gvHorarios.GridControl = this.gcHorarios;
            this.gvHorarios.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvHorarios.Name = "gvHorarios";
            this.gvHorarios.OptionsBehavior.Editable = false;
            this.gvHorarios.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvHorarios.OptionsView.EnableAppearanceEvenRow = true;
            this.gvHorarios.OptionsView.EnableAppearanceOddRow = true;
            this.gvHorarios.OptionsView.ShowGroupPanel = false;
            // 
            // ColunaID
            // 
            this.ColunaID.Caption = "ID";
            this.ColunaID.FieldName = "id";
            this.ColunaID.Name = "ColunaID";
            // 
            // ColunaDescricao
            // 
            this.ColunaDescricao.Caption = "Descrição";
            this.ColunaDescricao.FieldName = "descricao";
            this.ColunaDescricao.Name = "ColunaDescricao";
            this.ColunaDescricao.Visible = true;
            this.ColunaDescricao.VisibleIndex = 0;
            this.ColunaDescricao.Width = 450;
            // 
            // ColunaTipo
            // 
            this.ColunaTipo.Caption = "Tipo";
            this.ColunaTipo.FieldName = "tipohorario";
            this.ColunaTipo.Name = "ColunaTipo";
            this.ColunaTipo.Visible = true;
            this.ColunaTipo.VisibleIndex = 1;
            this.ColunaTipo.Width = 198;
            // 
            // HorarioPorDescricao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(736, 220);
            this.MinimizeBox = false;
            this.Name = "HorarioPorDescricao";
            this.ShowInTaskbar = false;
            this.Text = "Relatório de Horário por Descrição";
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHorarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvHorarios)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.GridControl gcHorarios;
        private DevExpress.XtraGrid.Views.Grid.GridView gvHorarios;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaID;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaDescricao;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaTipo;
    }
}
