namespace UI
{
    partial class FormManutMotivoMarcacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManutMotivoMarcacao));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtOcorrencia = new DevExpress.XtraEditors.TextEdit();
            this.txtMotivo = new DevExpress.XtraEditors.TextEdit();
            this.sbIdJustificativa = new Componentes.devexpress.cwk_DevButton();
            this.cbIdJustificativa = new Componentes.devexpress.cwk_DevLookup();
            this.lblFuncao = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOcorrencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMotivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdJustificativa.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(540, 102);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.sbIdJustificativa);
            this.xtraTabPage1.Controls.Add(this.cbIdJustificativa);
            this.xtraTabPage1.Controls.Add(this.lblFuncao);
            this.xtraTabPage1.Controls.Add(this.txtMotivo);
            this.xtraTabPage1.Controls.Add(this.txtOcorrencia);
            this.xtraTabPage1.Controls.Add(this.labelControl2);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Size = new System.Drawing.Size(531, 93);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(477, 120);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(396, 120);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 120);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(56, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Ocorrência:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(30, 68);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 13);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Motivo:";
            // 
            // txtOcorrencia
            // 
            this.txtOcorrencia.Location = new System.Drawing.Point(72, 13);
            this.txtOcorrencia.Name = "txtOcorrencia";
            this.txtOcorrencia.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtOcorrencia.Properties.Appearance.Options.UseBackColor = true;
            this.txtOcorrencia.Properties.ReadOnly = true;
            this.txtOcorrencia.Size = new System.Drawing.Size(34, 20);
            this.txtOcorrencia.TabIndex = 1;
            this.txtOcorrencia.TabStop = false;
            // 
            // txtMotivo
            // 
            this.txtMotivo.Location = new System.Drawing.Point(72, 65);
            this.txtMotivo.Name = "txtMotivo";
            this.txtMotivo.Size = new System.Drawing.Size(448, 20);
            this.txtMotivo.TabIndex = 6;
            // 
            // sbIdJustificativa
            // 
            this.sbIdJustificativa.Location = new System.Drawing.Point(496, 39);
            this.sbIdJustificativa.Name = "sbIdJustificativa";
            this.sbIdJustificativa.Size = new System.Drawing.Size(24, 20);
            this.sbIdJustificativa.TabIndex = 4;
            this.sbIdJustificativa.TabStop = false;
            this.sbIdJustificativa.Text = "...";
            this.sbIdJustificativa.Click += new System.EventHandler(this.sbIdJustificativa_Click);
            // 
            // cbIdJustificativa
            // 
            this.cbIdJustificativa.ButtonLookup = this.sbIdJustificativa;
            this.cbIdJustificativa.EditValue = 0;
            this.cbIdJustificativa.Key = System.Windows.Forms.Keys.F5;
            this.cbIdJustificativa.Location = new System.Drawing.Point(72, 39);
            this.cbIdJustificativa.Name = "cbIdJustificativa";
            this.cbIdJustificativa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdJustificativa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdJustificativa.Properties.DisplayMember = "descricao";
            this.cbIdJustificativa.Properties.NullText = "";
            this.cbIdJustificativa.Properties.ValueMember = "id";
            this.cbIdJustificativa.Size = new System.Drawing.Size(418, 20);
            this.cbIdJustificativa.TabIndex = 3;
            this.cbIdJustificativa.EditValueChanged += new System.EventHandler(this.cbIdJustificativa_EditValueChanged);
            // 
            // lblFuncao
            // 
            this.lblFuncao.Location = new System.Drawing.Point(5, 42);
            this.lblFuncao.Name = "lblFuncao";
            this.lblFuncao.Size = new System.Drawing.Size(61, 13);
            this.lblFuncao.TabIndex = 2;
            this.lblFuncao.Text = "Justificativa:";
            // 
            // FormManutMotivoMarcacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(564, 155);
            this.MinimizeBox = false;
            this.Name = "FormManutMotivoMarcacao";
            this.Text = "Motivo";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOcorrencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMotivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdJustificativa.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtOcorrencia;
        private DevExpress.XtraEditors.TextEdit txtMotivo;
        private Componentes.devexpress.cwk_DevButton sbIdJustificativa;
        private Componentes.devexpress.cwk_DevLookup cbIdJustificativa;
        private DevExpress.XtraEditors.LabelControl lblFuncao;
    }
}
