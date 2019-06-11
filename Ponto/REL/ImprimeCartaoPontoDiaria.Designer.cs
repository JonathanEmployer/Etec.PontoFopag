namespace REL
{
    partial class ImprimeCartaoPontoDiaria
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
            this.chbBuscaSaldo = new DevExpress.XtraEditors.CheckEdit();
            this.lblDataf = new DevExpress.XtraEditors.LabelControl();
            this.txtDataf = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbBuscaSaldo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 65);
            this.chbSalvarFiltro.Visible = false;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 61);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(276, 61);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(197, 61);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(339, 43);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chbBuscaSaldo);
            this.tabPage1.Controls.Add(this.lblDataf);
            this.tabPage1.Controls.Add(this.txtDataf);
            this.tabPage1.Size = new System.Drawing.Size(330, 34);
            // 
            // chbBuscaSaldo
            // 
            this.chbBuscaSaldo.Location = new System.Drawing.Point(146, 11);
            this.chbBuscaSaldo.Name = "chbBuscaSaldo";
            this.chbBuscaSaldo.Properties.Caption = "Busca Saldo do Banco de Horas";
            this.chbBuscaSaldo.Size = new System.Drawing.Size(176, 19);
            this.chbBuscaSaldo.TabIndex = 9;
            // 
            // lblDataf
            // 
            this.lblDataf.Location = new System.Drawing.Point(3, 13);
            this.lblDataf.Name = "lblDataf";
            this.lblDataf.Size = new System.Drawing.Size(27, 13);
            this.lblDataf.TabIndex = 7;
            this.lblDataf.Text = "Data:";
            // 
            // txtDataf
            // 
            this.txtDataf.EditValue = null;
            this.txtDataf.Location = new System.Drawing.Point(36, 10);
            this.txtDataf.Name = "txtDataf";
            this.txtDataf.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataf.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataf.Size = new System.Drawing.Size(79, 20);
            this.txtDataf.TabIndex = 8;
            // 
            // ImprimeCartaoPontoDiaria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(363, 90);
            this.Name = "ImprimeCartaoPontoDiaria";
            this.Text = "Cartão Ponto";
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbBuscaSaldo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit chbBuscaSaldo;
        private DevExpress.XtraEditors.LabelControl lblDataf;
        private DevExpress.XtraEditors.DateEdit txtDataf;
    }
}
