namespace REL
{
    partial class FuncionariosPorDepartamento
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
            this.gcDepartamentos = new DevExpress.XtraGrid.GridControl();
            this.gvDepartamentos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colunaID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColunaCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColunaDescricao = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDepartamentos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDepartamentos)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 221);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 217);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(561, 217);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(482, 217);
            // 
            // TabControl1
            // 
            this.TabControl1.SelectedTabPage = this.tabPage1;
            this.TabControl1.Size = new System.Drawing.Size(624, 199);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelControl1);
            this.tabPage1.Controls.Add(this.gcDepartamentos);
            this.tabPage1.Size = new System.Drawing.Size(615, 190);
            this.tabPage1.Controls.SetChildIndex(this.cbIdEmpresa, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbIdEmpresa, 0);
            this.tabPage1.Controls.SetChildIndex(this.gcDepartamentos, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl1, 0);
            // 
            // cbIdEmpresa
            // 
            this.cbIdEmpresa.EditValueChanged += new System.EventHandler(this.cbIdEmpresa_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(31, 48);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(95, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Departamentos";
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
            this.gcDepartamentos.EmbeddedNavigator.Name = "";
            this.gcDepartamentos.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcDepartamentos.Location = new System.Drawing.Point(31, 67);
            this.gcDepartamentos.MainView = this.gvDepartamentos;
            this.gcDepartamentos.Name = "gcDepartamentos";
            this.gcDepartamentos.Size = new System.Drawing.Size(571, 111);
            this.gcDepartamentos.TabIndex = 4;
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
            this.colunaID,
            this.ColunaCodigo,
            this.ColunaDescricao});
            this.gvDepartamentos.GridControl = this.gcDepartamentos;
            this.gvDepartamentos.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvDepartamentos.Name = "gvDepartamentos";
            this.gvDepartamentos.OptionsBehavior.Editable = false;
            this.gvDepartamentos.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvDepartamentos.OptionsView.EnableAppearanceEvenRow = true;
            this.gvDepartamentos.OptionsView.EnableAppearanceOddRow = true;
            this.gvDepartamentos.OptionsView.ShowGroupPanel = false;
            // 
            // colunaID
            // 
            this.colunaID.Caption = "ID";
            this.colunaID.FieldName = "id";
            this.colunaID.Name = "colunaID";
            // 
            // ColunaCodigo
            // 
            this.ColunaCodigo.Caption = "Código";
            this.ColunaCodigo.FieldName = "codigo";
            this.ColunaCodigo.Name = "ColunaCodigo";
            this.ColunaCodigo.Visible = true;
            this.ColunaCodigo.VisibleIndex = 0;
            // 
            // ColunaDescricao
            // 
            this.ColunaDescricao.Caption = "Descrição";
            this.ColunaDescricao.FieldName = "descricao";
            this.ColunaDescricao.Name = "ColunaDescricao";
            this.ColunaDescricao.Visible = true;
            this.ColunaDescricao.VisibleIndex = 1;
            // 
            // FuncionariosPorDepartamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(648, 246);
            this.Name = "FuncionariosPorDepartamento";
            this.Text = "Funcionários por departamento";
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDepartamentos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDepartamentos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.GridControl gcDepartamentos;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDepartamentos;
        private DevExpress.XtraGrid.Columns.GridColumn colunaID;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaDescricao;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaCodigo;
    }
}
