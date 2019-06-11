namespace UI
{
    partial class FormGridExportacaoCampos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridExportacaoCampos));
            this.sbExportacaoFolha = new DevExpress.XtraEditors.SimpleButton();
            this.lblCampos = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLocalizar
            // 
            // 
            // sbExportacaoFolha
            // 
            this.sbExportacaoFolha.Image = ((System.Drawing.Image)(resources.GetObject("sbExportacaoFolha.Image")));
            this.sbExportacaoFolha.Location = new System.Drawing.Point(12, 402);
            this.sbExportacaoFolha.Name = "sbExportacaoFolha";
            this.sbExportacaoFolha.Size = new System.Drawing.Size(155, 23);
            this.sbExportacaoFolha.TabIndex = 8;
            this.sbExportacaoFolha.Text = "E&xportação para Folha";
            this.sbExportacaoFolha.Click += new System.EventHandler(this.sbExportacaoFolha_Click);
            // 
            // lblCampos
            // 
            this.lblCampos.Location = new System.Drawing.Point(101, 437);
            this.lblCampos.Name = "lblCampos";
            this.lblCampos.Size = new System.Drawing.Size(48, 13);
            this.lblCampos.TabIndex = 9;
            this.lblCampos.Text = "lblCampos";
            // 
            // FormGridExportacaoCampos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 466);
            this.Controls.Add(this.sbExportacaoFolha);
            this.Controls.Add(this.lblCampos);
            this.Name = "FormGridExportacaoCampos";
            this.Text = "";
            this.Controls.SetChildIndex(this.lblCampos, 0);
            this.Controls.SetChildIndex(this.sbExportacaoFolha, 0);
            this.Controls.SetChildIndex(this.txtLocalizar, 0);
            this.Controls.SetChildIndex(this.btFiltro, 0);
            this.Controls.SetChildIndex(this.sbFechar, 0);
            this.Controls.SetChildIndex(this.sbExcluir, 0);
            this.Controls.SetChildIndex(this.sbAlterar, 0);
            this.Controls.SetChildIndex(this.sbIncluir, 0);
            this.Controls.SetChildIndex(this.sbConsultar, 0);
            this.Controls.SetChildIndex(this.sbAjudar, 0);
            this.Controls.SetChildIndex(this.sbSelecionar, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbExportacaoFolha;
        private DevExpress.XtraEditors.LabelControl lblCampos;
    }
}
