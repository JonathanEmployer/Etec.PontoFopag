using DevExpress.XtraEditors;

namespace RegistradorBiometrico.View
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
            this.txtUsuario = new DevExpress.XtraEditors.TextEdit();
            this.txtSenha = new DevExpress.XtraEditors.TextEdit();
            this.lblProgress = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.sbEntrar = new DevExpress.XtraEditors.SimpleButton();
            this.SBtnFechar = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsuario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(92, 228);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Properties.Appearance.BackColor = System.Drawing.Color.MintCream;
            this.txtUsuario.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.txtUsuario.Properties.Appearance.Options.UseBackColor = true;
            this.txtUsuario.Properties.Appearance.Options.UseFont = true;
            this.txtUsuario.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtUsuario.Size = new System.Drawing.Size(227, 28);
            this.txtUsuario.TabIndex = 0;
            this.txtUsuario.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtUsuario_KeyUp);
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(91, 323);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Properties.Appearance.BackColor = System.Drawing.Color.MintCream;
            this.txtSenha.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.txtSenha.Properties.Appearance.Options.UseBackColor = true;
            this.txtSenha.Properties.Appearance.Options.UseFont = true;
            this.txtSenha.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtSenha.Properties.UseSystemPasswordChar = true;
            this.txtSenha.Size = new System.Drawing.Size(227, 28);
            this.txtSenha.TabIndex = 1;
            this.txtSenha.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSenha_KeyUp);
            // 
            // lblProgress
            // 
            this.lblProgress.Location = new System.Drawing.Point(51, 411);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.PaleTurquoise;
            this.labelControl1.Location = new System.Drawing.Point(51, 427);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(0, 14);
            this.labelControl1.TabIndex = 18;
            // 
            // sbEntrar
            // 
            this.sbEntrar.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.sbEntrar.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.sbEntrar.Appearance.ForeColor = System.Drawing.Color.Black;
            this.sbEntrar.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Appearance.Image")));
            this.sbEntrar.Appearance.Options.UseBackColor = true;
            this.sbEntrar.Appearance.Options.UseFont = true;
            this.sbEntrar.Appearance.Options.UseForeColor = true;
            this.sbEntrar.Appearance.Options.UseImage = true;
            this.sbEntrar.Image = ((System.Drawing.Image)(resources.GetObject("sbEntrar.Image")));
            this.sbEntrar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sbEntrar.Location = new System.Drawing.Point(54, 366);
            this.sbEntrar.Name = "sbEntrar";
            this.sbEntrar.Size = new System.Drawing.Size(135, 42);
            this.sbEntrar.TabIndex = 19;
            this.sbEntrar.Text = "Entrar";
            this.sbEntrar.Click += new System.EventHandler(this.sbEntrar_Click);
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
            this.SBtnFechar.TabIndex = 20;
            this.SBtnFechar.Text = "Sair  ";
            this.SBtnFechar.Click += new System.EventHandler(this.SBtnFechar_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::RegistradorBiometrico.Properties.Resources.Login;
            this.ClientSize = new System.Drawing.Size(380, 530);
            this.ControlBox = false;
            this.Controls.Add(this.SBtnFechar);
            this.Controls.Add(this.sbEntrar);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.txtUsuario);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(380, 530);
            this.MinimumSize = new System.Drawing.Size(380, 530);
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormLogin";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormLogin_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.txtUsuario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextEdit txtUsuario;
        private TextEdit txtSenha;
        private LabelControl lblProgress;
        protected LabelControl labelControl1;
        private SimpleButton sbEntrar;
        private SimpleButton SBtnFechar;
    }
}