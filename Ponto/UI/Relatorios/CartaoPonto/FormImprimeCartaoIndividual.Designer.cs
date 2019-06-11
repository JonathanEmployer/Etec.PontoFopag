namespace UI.Relatorios.CartaoPonto
{
    partial class FormImprimeCartaoIndividual
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
            this.lblDataf = new DevExpress.XtraEditors.LabelControl();
            this.txtDataf = new DevExpress.XtraEditors.DateEdit();
            this.lblDatai = new DevExpress.XtraEditors.LabelControl();
            this.txtDatai = new DevExpress.XtraEditors.DateEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatai.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatai.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 78);
            this.chbSalvarFiltro.Visible = false;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 74);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(284, 74);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(205, 74);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(347, 56);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblDataf);
            this.tabPage1.Controls.Add(this.txtDatai);
            this.tabPage1.Controls.Add(this.txtDataf);
            this.tabPage1.Controls.Add(this.lblDatai);
            this.tabPage1.Size = new System.Drawing.Size(338, 47);
            // 
            // lblDataf
            // 
            this.lblDataf.Location = new System.Drawing.Point(185, 16);
            this.lblDataf.Name = "lblDataf";
            this.lblDataf.Size = new System.Drawing.Size(52, 13);
            this.lblDataf.TabIndex = 2;
            this.lblDataf.Text = "Data Final:";
            // 
            // txtDataf
            // 
            this.txtDataf.EditValue = null;
            this.txtDataf.Location = new System.Drawing.Point(243, 13);
            this.txtDataf.Name = "txtDataf";
            this.txtDataf.Properties.Appearance.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtDataf.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtDataf.Properties.Appearance.Options.UseBackColor = true;
            this.txtDataf.Properties.Appearance.Options.UseForeColor = true;
            this.txtDataf.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataf.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataf.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataf.Size = new System.Drawing.Size(79, 20);
            this.txtDataf.TabIndex = 3;
            // 
            // lblDatai
            // 
            this.lblDatai.Location = new System.Drawing.Point(20, 16);
            this.lblDatai.Name = "lblDatai";
            this.lblDatai.Size = new System.Drawing.Size(57, 13);
            this.lblDatai.TabIndex = 0;
            this.lblDatai.Text = "Data Inicial:";
            // 
            // txtDatai
            // 
            this.txtDatai.EditValue = null;
            this.txtDatai.Location = new System.Drawing.Point(83, 13);
            this.txtDatai.Name = "txtDatai";
            this.txtDatai.Properties.Appearance.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtDatai.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtDatai.Properties.Appearance.Options.UseBackColor = true;
            this.txtDatai.Properties.Appearance.Options.UseForeColor = true;
            this.txtDatai.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDatai.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDatai.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDatai.Size = new System.Drawing.Size(79, 20);
            this.txtDatai.TabIndex = 1;
            // 
            // FormImprimeCartaoIndividual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(371, 103);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimizeBox = false;
            this.Name = "FormImprimeCartaoIndividual";
            this.ShowInTaskbar = false;
            this.Text = "Relatório de Cartão Ponto Individual";
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatai.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatai.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblDataf;
        private DevExpress.XtraEditors.DateEdit txtDatai;
        private DevExpress.XtraEditors.DateEdit txtDataf;
        private DevExpress.XtraEditors.LabelControl lblDatai;
    }
}
