namespace cwkComunicadorWebAPIPontoWeb
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.SBtnFechar = new DevExpress.XtraEditors.SimpleButton();
            this.sbEntrar = new DevExpress.XtraEditors.SimpleButton();
            this.txtSenha = new DevExpress.XtraEditors.TextEdit();
            this.txtUsuario = new DevExpress.XtraEditors.TextEdit();
            this.lblProgress = new DevExpress.XtraEditors.LabelControl();
            this.cbWS = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsuario.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // SBtnFechar
            // 
            this.SBtnFechar.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.SBtnFechar.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            this.SBtnFechar.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.SBtnFechar.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.SBtnFechar.Appearance.ForeColor = System.Drawing.Color.Black;
            this.SBtnFechar.Appearance.Options.UseBackColor = true;
            this.SBtnFechar.Appearance.Options.UseBorderColor = true;
            this.SBtnFechar.Appearance.Options.UseFont = true;
            this.SBtnFechar.Appearance.Options.UseForeColor = true;
            this.SBtnFechar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SBtnFechar.Image = ((System.Drawing.Image)(resources.GetObject("SBtnFechar.Image")));
            this.SBtnFechar.Location = new System.Drawing.Point(195, 366);
            this.SBtnFechar.Name = "SBtnFechar";
            this.SBtnFechar.Size = new System.Drawing.Size(135, 42);
            this.SBtnFechar.TabIndex = 14;
            this.SBtnFechar.Text = "Sair  ";
            this.SBtnFechar.Click += new System.EventHandler(this.SBtnFechar_Click);
            // 
            // sbEntrar
            // 
            this.sbEntrar.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.sbEntrar.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.sbEntrar.Appearance.ForeColor = System.Drawing.Color.Black;
            this.sbEntrar.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("sbEntrar.Appearance.Image")));
            this.sbEntrar.Appearance.Options.UseBackColor = true;
            this.sbEntrar.Appearance.Options.UseFont = true;
            this.sbEntrar.Appearance.Options.UseForeColor = true;
            this.sbEntrar.Appearance.Options.UseImage = true;
            this.sbEntrar.Image = ((System.Drawing.Image)(resources.GetObject("sbEntrar.Image")));
            this.sbEntrar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sbEntrar.Location = new System.Drawing.Point(51, 366);
            this.sbEntrar.Name = "sbEntrar";
            this.sbEntrar.Size = new System.Drawing.Size(135, 42);
            this.sbEntrar.TabIndex = 13;
            this.sbEntrar.Text = "Entrar";
            this.sbEntrar.Click += new System.EventHandler(this.sbEntrar_Click);
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(91, 323);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Properties.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
            this.txtSenha.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenha.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtSenha.Properties.Appearance.Options.UseBackColor = true;
            this.txtSenha.Properties.Appearance.Options.UseFont = true;
            this.txtSenha.Properties.Appearance.Options.UseForeColor = true;
            this.txtSenha.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtSenha.Properties.MaxLength = 20;
            this.txtSenha.Properties.PasswordChar = '●';
            this.txtSenha.Size = new System.Drawing.Size(232, 28);
            this.txtSenha.TabIndex = 16;
            this.txtSenha.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSenha_KeyUp);
            // 
            // txtUsuario
            // 
            this.txtUsuario.EditValue = "";
            this.txtUsuario.Location = new System.Drawing.Point(92, 228);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Properties.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
            this.txtUsuario.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtUsuario.Properties.Appearance.Options.UseBackColor = true;
            this.txtUsuario.Properties.Appearance.Options.UseFont = true;
            this.txtUsuario.Properties.Appearance.Options.UseForeColor = true;
            this.txtUsuario.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtUsuario.Properties.MaxLength = 15;
            this.txtUsuario.Size = new System.Drawing.Size(227, 28);
            this.txtUsuario.TabIndex = 15;
            this.txtUsuario.EditValueChanged += new System.EventHandler(this.txtUsuario_EditValueChanged);
            this.txtUsuario.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUsuario_KeyUp);
            // 
            // lblProgress
            // 
            this.lblProgress.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProgress.Appearance.ForeColor = System.Drawing.Color.PaleTurquoise;
            this.lblProgress.Location = new System.Drawing.Point(51, 414);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 14);
            this.lblProgress.TabIndex = 17;
            // 
            // cbWS
            // 
            this.cbWS.FormattingEnabled = true;
            this.cbWS.Items.AddRange(new object[] {
            "Producao",
            "Homologacao",
            "Local"});
            this.cbWS.Location = new System.Drawing.Point(12, 496);
            this.cbWS.Name = "cbWS";
            this.cbWS.Size = new System.Drawing.Size(161, 21);
            this.cbWS.TabIndex = 18;
            this.cbWS.Visible = false;
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::cwkComunicadorWebAPIPontoWeb.Properties.Resources.Login;
            this.ClientSize = new System.Drawing.Size(380, 529);
            this.ControlBox = false;
            this.Controls.Add(this.cbWS);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.SBtnFechar);
            this.Controls.Add(this.sbEntrar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pontofopag Comunicador";
            this.Shown += new System.EventHandler(this.FormLogin_Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormLogin_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormLogin_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsuario.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton SBtnFechar;
        private DevExpress.XtraEditors.SimpleButton sbEntrar;
        private DevExpress.XtraEditors.TextEdit txtSenha;
        private DevExpress.XtraEditors.TextEdit txtUsuario;
        protected DevExpress.XtraEditors.LabelControl lblProgress;
        private System.Windows.Forms.ComboBox cbWS;
    }
}