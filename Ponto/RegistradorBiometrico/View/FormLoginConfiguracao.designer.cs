namespace RegistradorBiometrico.View
{
    partial class FormLoginConfiguracao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoginConfiguracao));
            this.TabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.txtSenha = new DevExpress.XtraEditors.TextEdit();
            this.txtLogin = new DevExpress.XtraEditors.TextEdit();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblPessoa = new DevExpress.XtraEditors.LabelControl();
            this.sbOk = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLogin.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // TabControl1
            // 
            this.TabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl1.Location = new System.Drawing.Point(12, 12);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedTabPage = this.tabPage1;
            this.TabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.TabControl1.Size = new System.Drawing.Size(283, 123);
            this.TabControl1.TabIndex = 0;
            this.TabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPage1});
            this.TabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtSenha);
            this.tabPage1.Controls.Add(this.txtLogin);
            this.tabPage1.Controls.Add(this.sbCancelar);
            this.tabPage1.Controls.Add(this.labelControl1);
            this.tabPage1.Controls.Add(this.lblPessoa);
            this.tabPage1.Controls.Add(this.sbOk);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(277, 117);
            this.tabPage1.Text = "xtraTabPage1";
            // 
            // txtSenha
            // 
            this.txtSenha.EnterMoveNextControl = true;
            this.txtSenha.Location = new System.Drawing.Point(50, 43);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Properties.UseSystemPasswordChar = true;
            this.txtSenha.Size = new System.Drawing.Size(209, 20);
            this.txtSenha.TabIndex = 1;
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(50, 17);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(209, 20);
            this.txtLogin.TabIndex = 0;
            // 
            // sbCancelar
            // 
            this.sbCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbCancelar.Location = new System.Drawing.Point(184, 79);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(75, 23);
            this.sbCancelar.TabIndex = 3;
            this.sbCancelar.Text = "&Cancelar";
            this.sbCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 46);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(34, 13);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "Senha:";
            // 
            // lblPessoa
            // 
            this.lblPessoa.Location = new System.Drawing.Point(15, 20);
            this.lblPessoa.Name = "lblPessoa";
            this.lblPessoa.Size = new System.Drawing.Size(29, 13);
            this.lblPessoa.TabIndex = 4;
            this.lblPessoa.Text = "Login:";
            // 
            // sbOk
            // 
            this.sbOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbOk.Location = new System.Drawing.Point(103, 79);
            this.sbOk.Name = "sbOk";
            this.sbOk.Size = new System.Drawing.Size(75, 23);
            this.sbOk.TabIndex = 2;
            this.sbOk.Text = "&Ok";
            this.sbOk.Click += new System.EventHandler(this.btOk_Click);
            // 
            // FormLoginConfiguracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(307, 147);
            this.Controls.Add(this.TabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLoginConfiguracao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Acesso Configurações";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormLoginConfiguracao_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLogin.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraTab.XtraTabControl TabControl1;
        protected DevExpress.XtraTab.XtraTabPage tabPage1;
        protected DevExpress.XtraEditors.SimpleButton sbOk;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblPessoa;
        protected DevExpress.XtraEditors.SimpleButton sbCancelar;
        private DevExpress.XtraEditors.TextEdit txtSenha;
        private DevExpress.XtraEditors.TextEdit txtLogin;
    }
}