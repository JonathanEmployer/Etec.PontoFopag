namespace REL
{
    partial class AfastamentoData
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
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.DTFinal = new DevExpress.XtraEditors.DateEdit();
            this.DTInicial = new DevExpress.XtraEditors.DateEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 465);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 461);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(678, 461);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(599, 461);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(741, 443);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelControl4);
            this.tabPage1.Controls.Add(this.DTFinal);
            this.tabPage1.Controls.Add(this.DTInicial);
            this.tabPage1.Controls.Add(this.labelControl5);
            this.tabPage1.Size = new System.Drawing.Size(732, 434);
            this.tabPage1.Controls.SetChildIndex(this.labelControl5, 0);
            this.tabPage1.Controls.SetChildIndex(this.DTInicial, 0);
            this.tabPage1.Controls.SetChildIndex(this.DTFinal, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl4, 0);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(588, 402);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(52, 13);
            this.labelControl4.TabIndex = 21;
            this.labelControl4.Text = "Data Final:";
            // 
            // DTFinal
            // 
            this.DTFinal.EditValue = null;
            this.DTFinal.Location = new System.Drawing.Point(646, 399);
            this.DTFinal.Name = "DTFinal";
            this.DTFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DTFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DTFinal.Size = new System.Drawing.Size(80, 20);
            this.DTFinal.TabIndex = 20;
            // 
            // DTInicial
            // 
            this.DTInicial.EditValue = null;
            this.DTInicial.Location = new System.Drawing.Point(484, 399);
            this.DTInicial.Name = "DTInicial";
            this.DTInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DTInicial.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DTInicial.Size = new System.Drawing.Size(80, 20);
            this.DTInicial.TabIndex = 18;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(421, 402);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(57, 13);
            this.labelControl5.TabIndex = 19;
            this.labelControl5.Text = "Data Inicial:";
            // 
            // AfastamentoData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(764, 490);
            this.Name = "AfastamentoData";
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.DateEdit DTFinal;
        private DevExpress.XtraEditors.DateEdit DTInicial;
        private DevExpress.XtraEditors.LabelControl labelControl5;

    }
}
