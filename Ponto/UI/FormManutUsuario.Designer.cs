namespace UI
{
    partial class FormManutUsuario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManutUsuario));
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.lblLogin = new DevExpress.XtraEditors.LabelControl();
            this.lblSenha = new DevExpress.XtraEditors.LabelControl();
            this.lblNome = new DevExpress.XtraEditors.LabelControl();
            this.lblGrupo = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.txtLogin = new DevExpress.XtraEditors.TextEdit();
            this.txtSenha = new DevExpress.XtraEditors.TextEdit();
            this.txtNome = new DevExpress.XtraEditors.TextEdit();
            this.sbIdGrupo = new Componentes.devexpress.cwk_DevButton();
            this.cbIdGrupo = new Componentes.devexpress.cwk_DevLookup();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.rgTipo = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLogin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNome.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdGrupo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 215);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Controls.Add(this.sbIdGrupo);
            this.xtraTabPage1.Controls.Add(this.cbIdGrupo);
            this.xtraTabPage1.Controls.Add(this.txtNome);
            this.xtraTabPage1.Controls.Add(this.txtSenha);
            this.xtraTabPage1.Controls.Add(this.txtLogin);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblGrupo);
            this.xtraTabPage1.Controls.Add(this.lblNome);
            this.xtraTabPage1.Controls.Add(this.lblSenha);
            this.xtraTabPage1.Controls.Add(this.lblLogin);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(516, 206);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 233);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.Images.SetKeyName(0, "Help copy.ico");
            this.imageList1.Images.SetKeyName(1, "Gravar copy.ico");
            this.imageList1.Images.SetKeyName(2, "cancelar copy.ico");
            this.imageList1.Images.SetKeyName(3, "Inserir copy.ico");
            this.imageList1.Images.SetKeyName(4, "Alterar copy.ico");
            this.imageList1.Images.SetKeyName(5, "Excluir copy.ico");
            this.imageList1.Images.SetKeyName(6, "Consulta copy.ico");
            this.imageList1.Images.SetKeyName(7, "Selecionar.ico");
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 233);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 233);
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(10, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // lblLogin
            // 
            this.lblLogin.Location = new System.Drawing.Point(18, 42);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(29, 13);
            this.lblLogin.TabIndex = 2;
            this.lblLogin.Text = "Login:";
            // 
            // lblSenha
            // 
            this.lblSenha.Location = new System.Drawing.Point(13, 68);
            this.lblSenha.Name = "lblSenha";
            this.lblSenha.Size = new System.Drawing.Size(34, 13);
            this.lblSenha.TabIndex = 4;
            this.lblSenha.Text = "Senha:";
            // 
            // lblNome
            // 
            this.lblNome.Location = new System.Drawing.Point(16, 94);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(31, 13);
            this.lblNome.TabIndex = 6;
            this.lblNome.Text = "Nome:";
            // 
            // lblGrupo
            // 
            this.lblGrupo.Location = new System.Drawing.Point(14, 120);
            this.lblGrupo.Name = "lblGrupo";
            this.lblGrupo.Size = new System.Drawing.Size(33, 13);
            this.lblGrupo.TabIndex = 8;
            this.lblGrupo.Text = "Grupo:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(53, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Properties.ValidateOnEnterKey = true;
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(53, 39);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(172, 20);
            this.txtLogin.TabIndex = 3;
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(53, 65);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Properties.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(172, 20);
            this.txtSenha.TabIndex = 5;
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(53, 91);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(453, 20);
            this.txtNome.TabIndex = 7;
            // 
            // sbIdGrupo
            // 
            this.sbIdGrupo.Location = new System.Drawing.Point(482, 117);
            this.sbIdGrupo.Name = "sbIdGrupo";
            this.sbIdGrupo.Size = new System.Drawing.Size(24, 20);
            this.sbIdGrupo.TabIndex = 10;
            this.sbIdGrupo.TabStop = false;
            this.sbIdGrupo.Text = "...";
            this.sbIdGrupo.Click += new System.EventHandler(this.sbIdGrupo_Click);
            // 
            // cbIdGrupo
            // 
            this.cbIdGrupo.ButtonLookup = this.sbIdGrupo;
            this.cbIdGrupo.EditValue = 0;
            this.cbIdGrupo.Key = System.Windows.Forms.Keys.F5;
            this.cbIdGrupo.Location = new System.Drawing.Point(53, 117);
            this.cbIdGrupo.Name = "cbIdGrupo";
            this.cbIdGrupo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdGrupo.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "Name10", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdGrupo.Properties.DisplayMember = "nome";
            this.cbIdGrupo.Properties.NullText = "";
            this.cbIdGrupo.Properties.ValueMember = "id";
            this.cbIdGrupo.Size = new System.Drawing.Size(423, 20);
            this.cbIdGrupo.TabIndex = 9;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.rgTipo);
            this.groupControl1.Location = new System.Drawing.Point(53, 143);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(412, 49);
            this.groupControl1.TabIndex = 11;
            this.groupControl1.Text = "Tipo";
            // 
            // rgTipo
            // 
            this.rgTipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipo.Location = new System.Drawing.Point(2, 20);
            this.rgTipo.Name = "rgTipo";
            this.rgTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Supervisor"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Operador"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Gerente")});
            this.rgTipo.Size = new System.Drawing.Size(408, 27);
            this.rgTipo.TabIndex = 0;
            // 
            // FormManutUsuario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 268);
            this.Name = "FormManutUsuario";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLogin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSenha.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNome.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdGrupo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblGrupo;
        private DevExpress.XtraEditors.LabelControl lblNome;
        private DevExpress.XtraEditors.LabelControl lblSenha;
        private DevExpress.XtraEditors.LabelControl lblLogin;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.TextEdit txtNome;
        private DevExpress.XtraEditors.TextEdit txtSenha;
        private DevExpress.XtraEditors.TextEdit txtLogin;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private Componentes.devexpress.cwk_DevButton sbIdGrupo;
        private Componentes.devexpress.cwk_DevLookup cbIdGrupo;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.RadioGroup rgTipo;
    }
}
