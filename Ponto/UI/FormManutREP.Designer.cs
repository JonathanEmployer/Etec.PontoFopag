namespace UI
{
    partial class FormManutREP
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.txtNumSerie = new DevExpress.XtraEditors.TextEdit();
            this.lblNumSerie = new DevExpress.XtraEditors.LabelControl();
            this.txtLocal = new DevExpress.XtraEditors.TextEdit();
            this.lblLocal = new DevExpress.XtraEditors.LabelControl();
            this.txtNumRelogio = new DevExpress.XtraEditors.TextEdit();
            this.lblNumRelogio = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtIP = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cbTipoComunicacao = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtPorta = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtSenha = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtDigitos = new DevExpress.XtraEditors.TextEdit();
            this.chbBiometrico = new DevExpress.XtraEditors.CheckEdit();
            this.sbIdEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.cbIdEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.lblEmpresa = new DevExpress.XtraEditors.LabelControl();
            this.txtRelogio = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumSerie.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumRelogio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTipoComunicacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDigitos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbBiometrico.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRelogio.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(503, 201);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtRelogio);
            this.xtraTabPage1.Controls.Add(this.sbIdEmpresa);
            this.xtraTabPage1.Controls.Add(this.cbIdEmpresa);
            this.xtraTabPage1.Controls.Add(this.lblEmpresa);
            this.xtraTabPage1.Controls.Add(this.chbBiometrico);
            this.xtraTabPage1.Controls.Add(this.txtDigitos);
            this.xtraTabPage1.Controls.Add(this.labelControl6);
            this.xtraTabPage1.Controls.Add(this.txtSenha);
            this.xtraTabPage1.Controls.Add(this.labelControl5);
            this.xtraTabPage1.Controls.Add(this.txtPorta);
            this.xtraTabPage1.Controls.Add(this.labelControl4);
            this.xtraTabPage1.Controls.Add(this.labelControl3);
            this.xtraTabPage1.Controls.Add(this.cbTipoComunicacao);
            this.xtraTabPage1.Controls.Add(this.txtIP);
            this.xtraTabPage1.Controls.Add(this.labelControl2);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.txtNumRelogio);
            this.xtraTabPage1.Controls.Add(this.lblNumRelogio);
            this.xtraTabPage1.Controls.Add(this.txtLocal);
            this.xtraTabPage1.Controls.Add(this.lblLocal);
            this.xtraTabPage1.Controls.Add(this.txtNumSerie);
            this.xtraTabPage1.Controls.Add(this.lblNumSerie);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(497, 195);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(440, 219);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(359, 219);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 219);
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(100, 10);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, false)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(57, 13);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // txtNumSerie
            // 
            this.txtNumSerie.Location = new System.Drawing.Point(100, 36);
            this.txtNumSerie.Name = "txtNumSerie";
            this.txtNumSerie.Properties.MaxLength = 17;
            this.txtNumSerie.Size = new System.Drawing.Size(135, 20);
            this.txtNumSerie.TabIndex = 4;
            this.txtNumSerie.Leave += new System.EventHandler(this.txtNumSerie_Leave);
            // 
            // lblNumSerie
            // 
            this.lblNumSerie.Location = new System.Drawing.Point(27, 39);
            this.lblNumSerie.Name = "lblNumSerie";
            this.lblNumSerie.Size = new System.Drawing.Size(67, 13);
            this.lblNumSerie.TabIndex = 3;
            this.lblNumSerie.Text = "Número série:";
            // 
            // txtLocal
            // 
            this.txtLocal.Location = new System.Drawing.Point(100, 63);
            this.txtLocal.Name = "txtLocal";
            this.txtLocal.Size = new System.Drawing.Size(390, 20);
            this.txtLocal.TabIndex = 8;
            // 
            // lblLocal
            // 
            this.lblLocal.Location = new System.Drawing.Point(66, 66);
            this.lblLocal.Name = "lblLocal";
            this.lblLocal.Size = new System.Drawing.Size(28, 13);
            this.lblLocal.TabIndex = 7;
            this.lblLocal.Text = "Local:";
            // 
            // txtNumRelogio
            // 
            this.txtNumRelogio.Location = new System.Drawing.Point(441, 37);
            this.txtNumRelogio.Name = "txtNumRelogio";
            this.txtNumRelogio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtNumRelogio.Properties.Mask.EditMask = "n0";
            this.txtNumRelogio.Properties.MaxLength = 4;
            this.txtNumRelogio.Size = new System.Drawing.Size(49, 20);
            this.txtNumRelogio.TabIndex = 6;
            // 
            // lblNumRelogio
            // 
            this.lblNumRelogio.Location = new System.Drawing.Point(359, 40);
            this.lblNumRelogio.Name = "lblNumRelogio";
            this.lblNumRelogio.Size = new System.Drawing.Size(76, 13);
            this.lblNumRelogio.TabIndex = 5;
            this.lblNumRelogio.Text = "Número relógio:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(55, 118);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(39, 13);
            this.labelControl1.TabIndex = 12;
            this.labelControl1.Text = "Relógio:";
            // 
            // txtIP
            // 
            this.txtIP.EditValue = "200.200.200.200";
            this.txtIP.Location = new System.Drawing.Point(100, 167);
            this.txtIP.Name = "txtIP";
            this.txtIP.Properties.Mask.EditMask = "(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(" +
    "25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";
            this.txtIP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtIP.Size = new System.Drawing.Size(92, 20);
            this.txtIP.TabIndex = 19;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(80, 170);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(14, 13);
            this.labelControl2.TabIndex = 18;
            this.labelControl2.Text = "IP:";
            // 
            // cbTipoComunicacao
            // 
            this.cbTipoComunicacao.EditValue = "";
            this.cbTipoComunicacao.Location = new System.Drawing.Point(100, 141);
            this.cbTipoComunicacao.Name = "cbTipoComunicacao";
            this.cbTipoComunicacao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbTipoComunicacao.Properties.Items.AddRange(new object[] {
            "TCP/IP",
            "Serial"});
            this.cbTipoComunicacao.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbTipoComunicacao.Size = new System.Drawing.Size(135, 20);
            this.cbTipoComunicacao.TabIndex = 15;
            this.cbTipoComunicacao.SelectedIndexChanged += new System.EventHandler(this.cbTipoComunicacao_SelectedIndexChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(4, 144);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(90, 13);
            this.labelControl3.TabIndex = 14;
            this.labelControl3.Text = "Tipo Comunicação:";
            // 
            // txtPorta
            // 
            this.txtPorta.Location = new System.Drawing.Point(355, 141);
            this.txtPorta.Name = "txtPorta";
            this.txtPorta.Properties.MaxLength = 10;
            this.txtPorta.Size = new System.Drawing.Size(135, 20);
            this.txtPorta.TabIndex = 17;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(319, 144);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(30, 13);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "Porta:";
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(390, 167);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Properties.MaxLength = 20;
            this.txtSenha.Properties.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(100, 20);
            this.txtSenha.TabIndex = 23;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(350, 170);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(34, 13);
            this.labelControl5.TabIndex = 22;
            this.labelControl5.Text = "Senha:";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(202, 170);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(57, 13);
            this.labelControl6.TabIndex = 20;
            this.labelControl6.Text = "Qtd Dígitos:";
            // 
            // txtDigitos
            // 
            this.txtDigitos.Location = new System.Drawing.Point(263, 167);
            this.txtDigitos.Name = "txtDigitos";
            this.txtDigitos.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.txtDigitos.Properties.Mask.EditMask = "\\d{0,2}";
            this.txtDigitos.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtDigitos.Properties.Mask.PlaceHolder = '0';
            this.txtDigitos.Properties.Mask.ShowPlaceHolders = false;
            this.txtDigitos.Size = new System.Drawing.Size(76, 20);
            this.txtDigitos.TabIndex = 21;
            // 
            // chbBiometrico
            // 
            this.chbBiometrico.Location = new System.Drawing.Point(419, 10);
            this.chbBiometrico.Name = "chbBiometrico";
            this.chbBiometrico.Properties.Caption = "Biométrico";
            this.chbBiometrico.Size = new System.Drawing.Size(71, 19);
            this.chbBiometrico.TabIndex = 2;
            // 
            // sbIdEmpresa
            // 
            this.sbIdEmpresa.Location = new System.Drawing.Point(466, 89);
            this.sbIdEmpresa.Name = "sbIdEmpresa";
            this.sbIdEmpresa.Size = new System.Drawing.Size(24, 20);
            this.sbIdEmpresa.TabIndex = 11;
            this.sbIdEmpresa.TabStop = false;
            this.sbIdEmpresa.Text = "...";
            this.sbIdEmpresa.Click += new System.EventHandler(this.sbIdEmpresa_Click);
            // 
            // cbIdEmpresa
            // 
            this.cbIdEmpresa.ButtonLookup = this.sbIdEmpresa;
            this.cbIdEmpresa.EditValue = 0;
            this.cbIdEmpresa.Key = System.Windows.Forms.Keys.F5;
            this.cbIdEmpresa.Location = new System.Drawing.Point(100, 89);
            this.cbIdEmpresa.Name = "cbIdEmpresa";
            this.cbIdEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("cnpj_cpf", "CNPJ/CPF")});
            this.cbIdEmpresa.Properties.DisplayMember = "nome";
            this.cbIdEmpresa.Properties.NullText = "";
            this.cbIdEmpresa.Properties.ValueMember = "id";
            this.cbIdEmpresa.Size = new System.Drawing.Size(360, 20);
            this.cbIdEmpresa.TabIndex = 10;
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.Location = new System.Drawing.Point(49, 92);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(45, 13);
            this.lblEmpresa.TabIndex = 9;
            this.lblEmpresa.Text = "Empresa:";
            // 
            // txtRelogio
            // 
            this.txtRelogio.Enabled = false;
            this.txtRelogio.Location = new System.Drawing.Point(100, 115);
            this.txtRelogio.Name = "txtRelogio";
            this.txtRelogio.Properties.ReadOnly = true;
            this.txtRelogio.Size = new System.Drawing.Size(390, 20);
            this.txtRelogio.TabIndex = 13;
            // 
            // FormManutREP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(527, 254);
            this.Name = "FormManutREP";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumSerie.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumRelogio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbTipoComunicacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDigitos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbBiometrico.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRelogio.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.TextEdit txtLocal;
        private DevExpress.XtraEditors.LabelControl lblLocal;
        private DevExpress.XtraEditors.TextEdit txtNumSerie;
        private DevExpress.XtraEditors.LabelControl lblNumSerie;
        private DevExpress.XtraEditors.TextEdit txtNumRelogio;
        private DevExpress.XtraEditors.LabelControl lblNumRelogio;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cbTipoComunicacao;
        private DevExpress.XtraEditors.TextEdit txtIP;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtPorta;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtSenha;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtDigitos;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.CheckEdit chbBiometrico;
        private Componentes.devexpress.cwk_DevButton sbIdEmpresa;
        private Componentes.devexpress.cwk_DevLookup cbIdEmpresa;
        private DevExpress.XtraEditors.LabelControl lblEmpresa;
        private DevExpress.XtraEditors.TextEdit txtRelogio;
    }
}
