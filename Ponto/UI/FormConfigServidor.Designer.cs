namespace UI
{
    partial class FormConfigServidor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfigServidor));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtServidor = new DevExpress.XtraEditors.TextEdit();
            this.txtCompartilhamento = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServidor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompartilhamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(562, 96);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Size = new System.Drawing.Size(553, 87);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(499, 114);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(418, 114);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 114);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(53, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(88, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Caminho Servidor:";
            // 
            // txtServidor
            // 
            this.txtServidor.Location = new System.Drawing.Point(147, 32);
            this.txtServidor.Name = "txtServidor";
            this.txtServidor.Size = new System.Drawing.Size(401, 20);
            this.txtServidor.TabIndex = 1;
            // 
            // txtCompartilhamento
            // 
            this.txtCompartilhamento.Location = new System.Drawing.Point(147, 58);
            this.txtCompartilhamento.Name = "txtCompartilhamento";
            this.txtCompartilhamento.Size = new System.Drawing.Size(401, 20);
            this.txtCompartilhamento.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(6, 61);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(135, 13);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Caminho Compartilhamento:";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtCompartilhamento);
            this.groupControl1.Controls.Add(this.txtServidor);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(-3, -3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(557, 91);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Backup";
            // 
            // FormConfigServidor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(586, 149);
            this.Name = "FormConfigServidor";
            this.Text = "Configuração Servidor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormManutConfiguracoesGerais_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServidor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCompartilhamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtServidor;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtCompartilhamento;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;

    }
}
