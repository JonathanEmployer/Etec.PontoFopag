namespace UI
{
    partial class FormVisualizarLayout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormVisualizarLayout));
            this.lblCampos = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lblQtdCaracteres = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // lblCampos
            // 
            this.lblCampos.Location = new System.Drawing.Point(12, 12);
            this.lblCampos.Name = "lblCampos";
            this.lblCampos.Size = new System.Drawing.Size(48, 13);
            this.lblCampos.TabIndex = 0;
            this.lblCampos.Text = "lblCampos";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 31);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(78, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Qtd Caracteres:";
            // 
            // lblQtdCaracteres
            // 
            this.lblQtdCaracteres.Location = new System.Drawing.Point(96, 31);
            this.lblQtdCaracteres.Name = "lblQtdCaracteres";
            this.lblQtdCaracteres.Size = new System.Drawing.Size(6, 13);
            this.lblQtdCaracteres.TabIndex = 2;
            this.lblQtdCaracteres.Text = "0";
            // 
            // FormVisualizarLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 61);
            this.Controls.Add(this.lblQtdCaracteres);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.lblCampos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormVisualizarLayout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Layout Exportação";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormVisualizarLayout_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblCampos;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl lblQtdCaracteres;
    }
}