namespace UI
{
    partial class FormMudancaCodigo
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
            this.lblFuncionario = new DevExpress.XtraEditors.LabelControl();
            this.cbIdFuncionario = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdIdentificacao = new Componentes.devexpress.cwk_DevButton();
            this.lblNovoCodigo = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigo = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 82);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.sbIdIdentificacao);
            this.xtraTabPage1.Controls.Add(this.lblNovoCodigo);
            this.xtraTabPage1.Controls.Add(this.cbIdFuncionario);
            this.xtraTabPage1.Controls.Add(this.lblFuncionario);
            this.xtraTabPage1.Size = new System.Drawing.Size(516, 73);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 100);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 100);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 100);
            // 
            // lblFuncionario
            // 
            this.lblFuncionario.Location = new System.Drawing.Point(22, 16);
            this.lblFuncionario.Name = "lblFuncionario";
            this.lblFuncionario.Size = new System.Drawing.Size(59, 13);
            this.lblFuncionario.TabIndex = 0;
            this.lblFuncionario.Text = "Funcionário:";
            // 
            // cbIdFuncionario
            // 
            this.cbIdFuncionario.ButtonLookup = this.sbIdIdentificacao;
            this.cbIdFuncionario.EditValue = 0;
            this.cbIdFuncionario.Key = System.Windows.Forms.Keys.F5;
            this.cbIdFuncionario.Location = new System.Drawing.Point(87, 13);
            this.cbIdFuncionario.Name = "cbIdFuncionario";
            this.cbIdFuncionario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdFuncionario.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "Name2", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dscodigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("matricula", "Name6", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("jornada", "Name7", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("empresa", "Name8", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("departamento", "Name9", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("carteira", "Name10", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dataadmissao", "Name59", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdFuncionario.Properties.DisplayMember = "nome";
            this.cbIdFuncionario.Properties.NullText = "";
            this.cbIdFuncionario.Properties.ValueMember = "id";
            this.cbIdFuncionario.Size = new System.Drawing.Size(391, 20);
            this.cbIdFuncionario.TabIndex = 1;
            // 
            // sbIdIdentificacao
            // 
            this.sbIdIdentificacao.Location = new System.Drawing.Point(484, 13);
            this.sbIdIdentificacao.Name = "sbIdIdentificacao";
            this.sbIdIdentificacao.Size = new System.Drawing.Size(24, 20);
            this.sbIdIdentificacao.TabIndex = 2;
            this.sbIdIdentificacao.TabStop = false;
            this.sbIdIdentificacao.Text = "...";
            this.sbIdIdentificacao.Click += new System.EventHandler(this.sbIdIdentificacao_Click);
            // 
            // lblNovoCodigo
            // 
            this.lblNovoCodigo.Location = new System.Drawing.Point(16, 42);
            this.lblNovoCodigo.Name = "lblNovoCodigo";
            this.lblNovoCodigo.Size = new System.Drawing.Size(65, 13);
            this.lblNovoCodigo.TabIndex = 3;
            this.lblNovoCodigo.Text = "Novo Código:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.EditValue = "";
            this.txtCodigo.Location = new System.Drawing.Point(87, 39);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtCodigo.Properties.MaxLength = 16;
            this.txtCodigo.Size = new System.Drawing.Size(128, 20);
            this.txtCodigo.TabIndex = 4;
            // 
            // FormMudancaCodigo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 135);
            this.Name = "FormMudancaCodigo";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Componentes.devexpress.cwk_DevLookup cbIdFuncionario;
        private DevExpress.XtraEditors.LabelControl lblFuncionario;
        private DevExpress.XtraEditors.LabelControl lblNovoCodigo;
        private Componentes.devexpress.cwk_DevButton sbIdIdentificacao;
        private DevExpress.XtraEditors.TextEdit txtCodigo;
    }
}
