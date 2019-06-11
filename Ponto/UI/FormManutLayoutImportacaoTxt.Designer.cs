namespace UI
{
    partial class FormManutLayoutImportacaoTxt
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
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.rgTipoCampo = new DevExpress.XtraEditors.RadioGroup();
            this.cbDelimitador = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbDescricaoCampo = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtPosicao = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtTamanho = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoCampo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbDelimitador.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbDescricaoCampo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPosicao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTamanho.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(457, 123);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtTamanho);
            this.xtraTabPage1.Controls.Add(this.labelControl7);
            this.xtraTabPage1.Controls.Add(this.txtPosicao);
            this.xtraTabPage1.Controls.Add(this.labelControl6);
            this.xtraTabPage1.Controls.Add(this.labelControl5);
            this.xtraTabPage1.Controls.Add(this.labelControl4);
            this.xtraTabPage1.Controls.Add(this.cbDescricaoCampo);
            this.xtraTabPage1.Controls.Add(this.cbDelimitador);
            this.xtraTabPage1.Controls.Add(this.labelControl2);
            this.xtraTabPage1.Controls.Add(this.rgTipoCampo);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.labelControl3);
            this.xtraTabPage1.Size = new System.Drawing.Size(448, 114);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(394, 141);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(313, 141);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 141);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(67, 10);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(37, 13);
            this.labelControl3.TabIndex = 24;
            this.labelControl3.Text = "Código:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(110, 7);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Mask.EditMask = "\\d*";
            this.txtCodigo.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtCodigo.Size = new System.Drawing.Size(64, 20);
            this.txtCodigo.TabIndex = 25;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(29, 38);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 13);
            this.labelControl2.TabIndex = 28;
            this.labelControl2.Text = "Tipo do Campo:";
            // 
            // rgTipoCampo
            // 
            this.rgTipoCampo.EditValue = true;
            this.rgTipoCampo.Location = new System.Drawing.Point(110, 33);
            this.rgTipoCampo.Name = "rgTipoCampo";
            this.rgTipoCampo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(true, "Fixo"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(false, "Variável")});
            this.rgTipoCampo.Size = new System.Drawing.Size(204, 24);
            this.rgTipoCampo.TabIndex = 27;
            this.rgTipoCampo.SelectedIndexChanged += new System.EventHandler(this.rgTipoCampo_SelectedIndexChanged);
            // 
            // cbDelimitador
            // 
            this.cbDelimitador.Location = new System.Drawing.Point(110, 89);
            this.cbDelimitador.Name = "cbDelimitador";
            this.cbDelimitador.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbDelimitador.Properties.Items.AddRange(new object[] {
            ",",
            ";",
            "|",
            "-",
            "_"});
            this.cbDelimitador.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbDelimitador.Size = new System.Drawing.Size(204, 20);
            this.cbDelimitador.TabIndex = 29;
            // 
            // cbDescricaoCampo
            // 
            this.cbDescricaoCampo.Location = new System.Drawing.Point(110, 63);
            this.cbDescricaoCampo.Name = "cbDescricaoCampo";
            this.cbDescricaoCampo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbDescricaoCampo.Properties.Items.AddRange(new object[] {
            "Código",
            "Matrícula",
            "Cód. Folha",
            "Nome Funcionário",
            "CTPS",
            "Cód. Depto.",
            "Desc. Depto.",
            "Cód. Função",
            "Desc. Função",
            "Data Admissão",
            "Dia Admissão",
            "Mês Admissão",
            "Ano Admissão",
            "Data Demissão",
            "Dia Demissão",
            "Mês Demissão",
            "Ano Demissão",
            "PIS"});
            this.cbDescricaoCampo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbDescricaoCampo.Size = new System.Drawing.Size(204, 20);
            this.cbDescricaoCampo.TabIndex = 30;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(3, 66);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(101, 13);
            this.labelControl4.TabIndex = 31;
            this.labelControl4.Text = "Descrição do Campo:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(47, 92);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(57, 13);
            this.labelControl5.TabIndex = 32;
            this.labelControl5.Text = "Delimitador:";
            // 
            // txtPosicao
            // 
            this.txtPosicao.Location = new System.Drawing.Point(374, 63);
            this.txtPosicao.Name = "txtPosicao";
            this.txtPosicao.Properties.Mask.EditMask = "\\d{1,5}";
            this.txtPosicao.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtPosicao.Properties.Mask.ShowPlaceHolders = false;
            this.txtPosicao.Properties.MaxLength = 5;
            this.txtPosicao.Size = new System.Drawing.Size(69, 20);
            this.txtPosicao.TabIndex = 34;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(328, 66);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(40, 13);
            this.labelControl6.TabIndex = 33;
            this.labelControl6.Text = "Posição:";
            // 
            // txtTamanho
            // 
            this.txtTamanho.Location = new System.Drawing.Point(374, 89);
            this.txtTamanho.Name = "txtTamanho";
            this.txtTamanho.Properties.Mask.EditMask = "\\d{1,5}";
            this.txtTamanho.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtTamanho.Properties.Mask.ShowPlaceHolders = false;
            this.txtTamanho.Properties.MaxLength = 5;
            this.txtTamanho.Size = new System.Drawing.Size(69, 20);
            this.txtTamanho.TabIndex = 36;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(320, 92);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(48, 13);
            this.labelControl7.TabIndex = 35;
            this.labelControl7.Text = "Tamanho:";
            // 
            // FormManutLayoutImportacaoTxt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(481, 176);
            this.Name = "FormManutLayoutImportacaoTxt";
            this.Text = "Layout  Arquivo Importação";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoCampo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbDelimitador.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbDescricaoCampo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPosicao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTamanho.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtCodigo;
        private DevExpress.XtraEditors.TextEdit txtTamanho;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit txtPosicao;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit cbDescricaoCampo;
        private DevExpress.XtraEditors.ComboBoxEdit cbDelimitador;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.RadioGroup rgTipoCampo;
    }
}
