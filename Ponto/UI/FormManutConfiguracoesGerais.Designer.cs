namespace UI
{
    partial class FormManutConfiguracoesGerais
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
            this.gcDiasFechamentoPonto = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtDataFinal = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtDataInicial = new DevExpress.XtraEditors.SpinEdit();
            this.chbMudarPeriodoAposDataFinal = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDiasFechamentoPonto)).BeginInit();
            this.gcDiasFechamentoPonto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbMudarPeriodoAposDataFinal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(358, 101);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gcDiasFechamentoPonto);
            this.xtraTabPage1.Size = new System.Drawing.Size(349, 92);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(295, 119);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(214, 119);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 119);
            // 
            // gcDiasFechamentoPonto
            // 
            this.gcDiasFechamentoPonto.Controls.Add(this.chbMudarPeriodoAposDataFinal);
            this.gcDiasFechamentoPonto.Controls.Add(this.labelControl2);
            this.gcDiasFechamentoPonto.Controls.Add(this.txtDataFinal);
            this.gcDiasFechamentoPonto.Controls.Add(this.labelControl1);
            this.gcDiasFechamentoPonto.Controls.Add(this.txtDataInicial);
            this.gcDiasFechamentoPonto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDiasFechamentoPonto.Location = new System.Drawing.Point(0, 0);
            this.gcDiasFechamentoPonto.Name = "gcDiasFechamentoPonto";
            this.gcDiasFechamentoPonto.Size = new System.Drawing.Size(349, 92);
            this.gcDiasFechamentoPonto.TabIndex = 0;
            this.gcDiasFechamentoPonto.Text = "Data para fechamento do ponto";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(194, 40);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(52, 13);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Data Final:";
            // 
            // txtDataFinal
            // 
            this.txtDataFinal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtDataFinal.Location = new System.Drawing.Point(252, 37);
            this.txtDataFinal.Name = "txtDataFinal";
            this.txtDataFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataFinal.Properties.IsFloatValue = false;
            this.txtDataFinal.Properties.Mask.EditMask = "N00";
            this.txtDataFinal.Properties.MaxLength = 2;
            this.txtDataFinal.Properties.MaxValue = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.txtDataFinal.Size = new System.Drawing.Size(87, 20);
            this.txtDataFinal.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(9, 40);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(57, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Data Inicial:";
            // 
            // txtDataInicial
            // 
            this.txtDataInicial.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtDataInicial.Location = new System.Drawing.Point(72, 37);
            this.txtDataInicial.Name = "txtDataInicial";
            this.txtDataInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataInicial.Properties.IsFloatValue = false;
            this.txtDataInicial.Properties.Mask.EditMask = "N00";
            this.txtDataInicial.Properties.MaxLength = 2;
            this.txtDataInicial.Properties.MaxValue = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.txtDataInicial.Size = new System.Drawing.Size(87, 20);
            this.txtDataInicial.TabIndex = 1;
            // 
            // chbMudarPeriodoAposDataFinal
            // 
            this.chbMudarPeriodoAposDataFinal.AutoSizeInLayoutControl = true;
            this.chbMudarPeriodoAposDataFinal.Location = new System.Drawing.Point(70, 63);
            this.chbMudarPeriodoAposDataFinal.Name = "chbMudarPeriodoAposDataFinal";
            this.chbMudarPeriodoAposDataFinal.Properties.Caption = "Mudar período imediatamente após data final";
            this.chbMudarPeriodoAposDataFinal.Size = new System.Drawing.Size(244, 19);
            this.chbMudarPeriodoAposDataFinal.TabIndex = 5;
            // 
            // FormManutConfiguracoesGerais
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(382, 154);
            this.Name = "FormManutConfiguracoesGerais";
            this.Text = "Configurações Gerais";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormManutConfiguracoesGerais_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDiasFechamentoPonto)).EndInit();
            this.gcDiasFechamentoPonto.ResumeLayout(false);
            this.gcDiasFechamentoPonto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbMudarPeriodoAposDataFinal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gcDiasFechamentoPonto;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit txtDataFinal;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit txtDataInicial;
        private DevExpress.XtraEditors.CheckEdit chbMudarPeriodoAposDataFinal;
    }
}
