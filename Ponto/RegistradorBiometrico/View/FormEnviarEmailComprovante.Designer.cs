namespace RegistradorBiometrico.View
{
    partial class FormEnviarEmailComprovante
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEnviarEmailComprovante));
            this.txtDestinatario = new DevExpress.XtraEditors.TextEdit();
            this.sbFechar = new DevExpress.XtraEditors.SimpleButton();
            this.sbEnviar = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinatario.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDestinatario
            // 
            this.txtDestinatario.EditValue = "";
            this.txtDestinatario.Location = new System.Drawing.Point(62, 33);
            this.txtDestinatario.Name = "txtDestinatario";
            this.txtDestinatario.Size = new System.Drawing.Size(375, 20);
            this.txtDestinatario.TabIndex = 0;
            // 
            // sbFechar
            // 
            this.sbFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbFechar.Location = new System.Drawing.Point(362, 63);
            this.sbFechar.Name = "sbFechar";
            this.sbFechar.Size = new System.Drawing.Size(75, 23);
            this.sbFechar.TabIndex = 22;
            this.sbFechar.Text = "Fechar";
            this.sbFechar.Click += new System.EventHandler(this.sbFechar_Click);
            // 
            // sbEnviar
            // 
            this.sbEnviar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbEnviar.Location = new System.Drawing.Point(269, 63);
            this.sbEnviar.Name = "sbEnviar";
            this.sbEnviar.Size = new System.Drawing.Size(87, 23);
            this.sbEnviar.TabIndex = 21;
            this.sbEnviar.Text = "Enviar";
            this.sbEnviar.Click += new System.EventHandler(this.sbEnviar_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(28, 36);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(28, 13);
            this.labelControl2.TabIndex = 18;
            this.labelControl2.Text = "Email:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(28, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(369, 13);
            this.labelControl1.TabIndex = 23;
            this.labelControl1.Text = "Para incluir mais de um e-mail utilize \",\" para separar os endereços digitados. ";
            // 
            // FormEnviarEmailComprovante
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 98);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.sbFechar);
            this.Controls.Add(this.sbEnviar);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtDestinatario);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEnviarEmailComprovante";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enviar Comprovante de Registro";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormEnviarEmailComprovante_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtDestinatario.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtDestinatario;
        protected DevExpress.XtraEditors.SimpleButton sbFechar;
        protected DevExpress.XtraEditors.SimpleButton sbEnviar;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}