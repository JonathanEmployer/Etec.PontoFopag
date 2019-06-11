namespace RegistradorBiometrico.View
{
    partial class FormConfiguracao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguracao));
            this.TabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblNome = new DevExpress.XtraEditors.LabelControl();
            this.lblBio1 = new DevExpress.XtraEditors.LabelControl();
            this.lblBio2 = new DevExpress.XtraEditors.LabelControl();
            this.txtCPF = new DevExpress.XtraEditors.TextEdit();
            this.txtLocal = new DevExpress.XtraEditors.TextEdit();
            this.btnSalvarBiometria = new DevExpress.XtraEditors.SimpleButton();
            this.lblStatusBiometria2 = new DevExpress.XtraEditors.LabelControl();
            this.lblStatusBiometria1 = new DevExpress.XtraEditors.LabelControl();
            this.btnCadastrarBiometria2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnCadastrarBiometria1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.btnVerificaCpfBiometria = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cbUTC = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.lblLocal = new DevExpress.XtraEditors.LabelControl();
            this.btnSalvar = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCPF.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbUTC.Properties)).BeginInit();
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
            this.TabControl1.Size = new System.Drawing.Size(433, 313);
            this.TabControl1.TabIndex = 0;
            this.TabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPage1});
            this.TabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelControl1);
            this.tabPage1.Controls.Add(this.lblNome);
            this.tabPage1.Controls.Add(this.lblBio1);
            this.tabPage1.Controls.Add(this.lblBio2);
            this.tabPage1.Controls.Add(this.txtCPF);
            this.tabPage1.Controls.Add(this.txtLocal);
            this.tabPage1.Controls.Add(this.btnSalvarBiometria);
            this.tabPage1.Controls.Add(this.lblStatusBiometria2);
            this.tabPage1.Controls.Add(this.lblStatusBiometria1);
            this.tabPage1.Controls.Add(this.btnCadastrarBiometria2);
            this.tabPage1.Controls.Add(this.btnCadastrarBiometria1);
            this.tabPage1.Controls.Add(this.labelControl6);
            this.tabPage1.Controls.Add(this.labelControl5);
            this.tabPage1.Controls.Add(this.btnVerificaCpfBiometria);
            this.tabPage1.Controls.Add(this.labelControl4);
            this.tabPage1.Controls.Add(this.labelControl3);
            this.tabPage1.Controls.Add(this.labelControl2);
            this.tabPage1.Controls.Add(this.cbUTC);
            this.tabPage1.Controls.Add(this.btnCancelar);
            this.tabPage1.Controls.Add(this.lblLocal);
            this.tabPage1.Controls.Add(this.btnSalvar);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(427, 307);
            this.tabPage1.Text = "xtraTabPage1";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(29, 138);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(31, 13);
            this.labelControl1.TabIndex = 53;
            this.labelControl1.Text = "Nome:";
            // 
            // lblNome
            // 
            this.lblNome.Location = new System.Drawing.Point(66, 138);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(27, 13);
            this.lblNome.TabIndex = 52;
            this.lblNome.Text = "Nome";
            // 
            // lblBio1
            // 
            this.lblBio1.Location = new System.Drawing.Point(66, 164);
            this.lblBio1.Name = "lblBio1";
            this.lblBio1.Size = new System.Drawing.Size(0, 13);
            this.lblBio1.TabIndex = 51;
            // 
            // lblBio2
            // 
            this.lblBio2.Location = new System.Drawing.Point(66, 193);
            this.lblBio2.Name = "lblBio2";
            this.lblBio2.Size = new System.Drawing.Size(0, 13);
            this.lblBio2.TabIndex = 50;
            // 
            // txtCPF
            // 
            this.txtCPF.Location = new System.Drawing.Point(49, 110);
            this.txtCPF.Name = "txtCPF";
            this.txtCPF.Properties.Mask.EditMask = "\\d\\d\\d\\.\\d\\d\\d\\.\\d\\d\\d-\\d\\d";
            this.txtCPF.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Regular;
            this.txtCPF.Size = new System.Drawing.Size(148, 20);
            this.txtCPF.TabIndex = 2;
            // 
            // txtLocal
            // 
            this.txtLocal.Location = new System.Drawing.Point(49, 46);
            this.txtLocal.Name = "txtLocal";
            this.txtLocal.Size = new System.Drawing.Size(361, 20);
            this.txtLocal.TabIndex = 1;
            // 
            // btnSalvarBiometria
            // 
            this.btnSalvarBiometria.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalvarBiometria.Location = new System.Drawing.Point(49, 234);
            this.btnSalvarBiometria.Name = "btnSalvarBiometria";
            this.btnSalvarBiometria.Size = new System.Drawing.Size(134, 23);
            this.btnSalvarBiometria.TabIndex = 6;
            this.btnSalvarBiometria.Text = "Salvar Biometria";
            this.btnSalvarBiometria.Click += new System.EventHandler(this.btnSalvarBiometria_Click);
            // 
            // lblStatusBiometria2
            // 
            this.lblStatusBiometria2.Location = new System.Drawing.Point(96, 193);
            this.lblStatusBiometria2.Name = "lblStatusBiometria2";
            this.lblStatusBiometria2.Size = new System.Drawing.Size(0, 13);
            this.lblStatusBiometria2.TabIndex = 49;
            // 
            // lblStatusBiometria1
            // 
            this.lblStatusBiometria1.Location = new System.Drawing.Point(96, 164);
            this.lblStatusBiometria1.Name = "lblStatusBiometria1";
            this.lblStatusBiometria1.Size = new System.Drawing.Size(0, 13);
            this.lblStatusBiometria1.TabIndex = 48;
            // 
            // btnCadastrarBiometria2
            // 
            this.btnCadastrarBiometria2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCadastrarBiometria2.Location = new System.Drawing.Point(216, 189);
            this.btnCadastrarBiometria2.Name = "btnCadastrarBiometria2";
            this.btnCadastrarBiometria2.Size = new System.Drawing.Size(195, 23);
            this.btnCadastrarBiometria2.TabIndex = 5;
            this.btnCadastrarBiometria2.Text = "Cadastrar Biometria 2";
            this.btnCadastrarBiometria2.Click += new System.EventHandler(this.btnCadastrarBiometria2_Click);
            // 
            // btnCadastrarBiometria1
            // 
            this.btnCadastrarBiometria1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCadastrarBiometria1.Location = new System.Drawing.Point(216, 162);
            this.btnCadastrarBiometria1.Name = "btnCadastrarBiometria1";
            this.btnCadastrarBiometria1.Size = new System.Drawing.Size(195, 23);
            this.btnCadastrarBiometria1.TabIndex = 4;
            this.btnCadastrarBiometria1.Text = "Cadastrar Biometria 1";
            this.btnCadastrarBiometria1.Click += new System.EventHandler(this.btnCadastrarBiometria1_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(3, 193);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(57, 13);
            this.labelControl6.TabIndex = 45;
            this.labelControl6.Text = "Biometria 2:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(3, 164);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(57, 13);
            this.labelControl5.TabIndex = 44;
            this.labelControl5.Text = "Biometria 1:";
            // 
            // btnVerificaCpfBiometria
            // 
            this.btnVerificaCpfBiometria.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVerificaCpfBiometria.Location = new System.Drawing.Point(216, 109);
            this.btnVerificaCpfBiometria.Name = "btnVerificaCpfBiometria";
            this.btnVerificaCpfBiometria.Size = new System.Drawing.Size(195, 23);
            this.btnVerificaCpfBiometria.TabIndex = 3;
            this.btnVerificaCpfBiometria.Text = "Verificar CPFs e Biometrias";
            this.btnVerificaCpfBiometria.Click += new System.EventHandler(this.btnVerificaCpfBiometria_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(20, 113);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(23, 13);
            this.labelControl4.TabIndex = 41;
            this.labelControl4.Text = "CPF:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(15, 83);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(104, 13);
            this.labelControl3.TabIndex = 40;
            this.labelControl3.Text = "Cadastrar Biometrias:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(20, 18);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 13);
            this.labelControl2.TabIndex = 39;
            this.labelControl2.Text = "UTC:";
            // 
            // cbUTC
            // 
            this.cbUTC.Location = new System.Drawing.Point(49, 15);
            this.cbUTC.Name = "cbUTC";
            this.cbUTC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbUTC.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbUTC.Size = new System.Drawing.Size(225, 20);
            this.cbUTC.TabIndex = 0;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(336, 279);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 8;
            this.btnCancelar.Text = "&Cancelar";
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblLocal
            // 
            this.lblLocal.Location = new System.Drawing.Point(15, 49);
            this.lblLocal.Name = "lblLocal";
            this.lblLocal.Size = new System.Drawing.Size(28, 13);
            this.lblLocal.TabIndex = 4;
            this.lblLocal.Text = "Local:";
            // 
            // btnSalvar
            // 
            this.btnSalvar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalvar.Location = new System.Drawing.Point(255, 279);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 7;
            this.btnSalvar.Text = "&Salvar";
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // FormConfiguracao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(457, 337);
            this.Controls.Add(this.TabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormConfiguracao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurações";
            this.Shown += new System.EventHandler(this.FormConfiguracao_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCPF.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbUTC.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraTab.XtraTabControl TabControl1;
        protected DevExpress.XtraTab.XtraTabPage tabPage1;
        protected DevExpress.XtraEditors.SimpleButton btnSalvar;
        private DevExpress.XtraEditors.LabelControl lblLocal;
        protected DevExpress.XtraEditors.SimpleButton btnCancelar;
        private DevExpress.XtraEditors.ComboBoxEdit cbUTC;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        protected DevExpress.XtraEditors.SimpleButton btnCadastrarBiometria2;
        protected DevExpress.XtraEditors.SimpleButton btnCadastrarBiometria1;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        protected DevExpress.XtraEditors.SimpleButton btnVerificaCpfBiometria;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl lblStatusBiometria2;
        private DevExpress.XtraEditors.LabelControl lblStatusBiometria1;
        protected DevExpress.XtraEditors.SimpleButton btnSalvarBiometria;
        private DevExpress.XtraEditors.TextEdit txtCPF;
        private DevExpress.XtraEditors.TextEdit txtLocal;
        private DevExpress.XtraEditors.LabelControl lblBio1;
        private DevExpress.XtraEditors.LabelControl lblBio2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lblNome;
    }
}