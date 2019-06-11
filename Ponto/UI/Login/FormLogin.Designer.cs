namespace UI.Login
{
    partial class FormLogin
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtUsuario = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtSenha = new DevExpress.XtraEditors.TextEdit();
            this.sbEntrar = new DevExpress.XtraEditors.SimpleButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.lblVersao = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsuario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(35, 307);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Usuário:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(81, 304);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Properties.MaxLength = 15;
            this.txtUsuario.Size = new System.Drawing.Size(100, 20);
            this.txtUsuario.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(41, 333);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(34, 13);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Senha:";
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(81, 330);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Properties.MaxLength = 10;
            this.txtSenha.Properties.PasswordChar = '●';
            this.txtSenha.Size = new System.Drawing.Size(100, 20);
            this.txtSenha.TabIndex = 3;
            // 
            // sbEntrar
            // 
            this.sbEntrar.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.sbEntrar.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.sbEntrar.Appearance.ForeColor = System.Drawing.Color.White;
            this.sbEntrar.Appearance.Options.UseBackColor = true;
            this.sbEntrar.Appearance.Options.UseFont = true;
            this.sbEntrar.Appearance.Options.UseForeColor = true;
            this.sbEntrar.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.sbEntrar.ImageIndex = 0;
            this.sbEntrar.ImageList = this.imageList1;
            this.sbEntrar.Location = new System.Drawing.Point(193, 301);
            this.sbEntrar.Name = "sbEntrar";
            this.sbEntrar.Size = new System.Drawing.Size(75, 23);
            this.sbEntrar.TabIndex = 4;
            this.sbEntrar.Text = "&Entrar";
            this.sbEntrar.Click += new System.EventHandler(this.sbEntrar_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Logo.ico");
            this.imageList1.Images.SetKeyName(1, "cancelar copy.ico");
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.simpleButton1.Appearance.ForeColor = System.Drawing.Color.White;
            this.simpleButton1.Appearance.Options.UseBackColor = true;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Appearance.Options.UseForeColor = true;
            this.simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.simpleButton1.ImageIndex = 1;
            this.simpleButton1.ImageList = this.imageList1;
            this.simpleButton1.Location = new System.Drawing.Point(193, 331);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(75, 22);
            this.simpleButton1.TabIndex = 5;
            this.simpleButton1.Text = "&Sair";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // lblVersao
            // 
            this.lblVersao.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersao.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblVersao.Appearance.Options.UseFont = true;
            this.lblVersao.Appearance.Options.UseForeColor = true;
            this.lblVersao.Location = new System.Drawing.Point(15, 282);
            this.lblVersao.Name = "lblVersao";
            this.lblVersao.Size = new System.Drawing.Size(39, 13);
            this.lblVersao.TabIndex = 6;
            this.lblVersao.Text = "Versão";
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(320, 376);
            this.Controls.Add(this.lblVersao);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.sbEntrar);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cwork-Ponto";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormLogin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtUsuario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtUsuario;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtSenha;
        private DevExpress.XtraEditors.SimpleButton sbEntrar;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.LabelControl lblVersao;
    }
}