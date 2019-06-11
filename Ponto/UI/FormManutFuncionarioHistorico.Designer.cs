namespace UI
{
    partial class FormManutFuncionarioHistorico
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
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.lblData = new DevExpress.XtraEditors.LabelControl();
            this.lblHora = new DevExpress.XtraEditors.LabelControl();
            this.lblHistorico = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.txtData = new DevExpress.XtraEditors.DateEdit();
            this.txtHistorico = new DevExpress.XtraEditors.MemoEdit();
            this.txtHora = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHistorico.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHora.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHora.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 198);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtHora);
            this.xtraTabPage1.Controls.Add(this.txtHistorico);
            this.xtraTabPage1.Controls.Add(this.txtData);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblHistorico);
            this.xtraTabPage1.Controls.Add(this.lblHora);
            this.xtraTabPage1.Controls.Add(this.lblData);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(516, 189);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 216);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 216);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 216);
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(18, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // lblData
            // 
            this.lblData.Location = new System.Drawing.Point(28, 42);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(27, 13);
            this.lblData.TabIndex = 2;
            this.lblData.Text = "Data:";
            // 
            // lblHora
            // 
            this.lblHora.Location = new System.Drawing.Point(198, 42);
            this.lblHora.Name = "lblHora";
            this.lblHora.Size = new System.Drawing.Size(27, 13);
            this.lblHora.TabIndex = 4;
            this.lblHora.Text = "Hora:";
            // 
            // lblHistorico
            // 
            this.lblHistorico.Location = new System.Drawing.Point(10, 68);
            this.lblHistorico.Name = "lblHistorico";
            this.lblHistorico.Size = new System.Drawing.Size(45, 13);
            this.lblHistorico.TabIndex = 6;
            this.lblHistorico.Text = "Histórico:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(61, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // txtData
            // 
            this.txtData.EditValue = null;
            this.txtData.Location = new System.Drawing.Point(61, 39);
            this.txtData.Name = "txtData";
            this.txtData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtData.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtData.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtData.Size = new System.Drawing.Size(79, 20);
            this.txtData.TabIndex = 3;
            // 
            // txtHistorico
            // 
            this.txtHistorico.Location = new System.Drawing.Point(61, 65);
            this.txtHistorico.Name = "txtHistorico";
            this.txtHistorico.Properties.MaxLength = 360;
            this.txtHistorico.Size = new System.Drawing.Size(443, 111);
            this.txtHistorico.TabIndex = 7;
            // 
            // txtHora
            // 
            this.txtHora.EditValue = null;
            this.txtHora.Location = new System.Drawing.Point(231, 39);
            this.txtHora.Name = "txtHora";
            this.txtHora.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtHora.Properties.DisplayFormat.FormatString = "t";
            this.txtHora.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtHora.Properties.EditFormat.FormatString = "t";
            this.txtHora.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtHora.Properties.Mask.EditMask = "t";
            this.txtHora.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtHora.Size = new System.Drawing.Size(35, 20);
            this.txtHora.TabIndex = 5;
            // 
            // FormManutFuncionarioHistorico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 251);
            this.Name = "FormManutFuncionarioHistorico";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHistorico.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHora.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHora.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblHistorico;
        private DevExpress.XtraEditors.LabelControl lblHora;
        private DevExpress.XtraEditors.LabelControl lblData;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.MemoEdit txtHistorico;
        private DevExpress.XtraEditors.DateEdit txtData;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.DateEdit txtHora;
    }
}
