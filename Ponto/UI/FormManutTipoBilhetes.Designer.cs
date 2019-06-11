namespace UI
{
    partial class FormManutTipoBilhetes
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.lblDescricao = new DevExpress.XtraEditors.LabelControl();
            this.lblDiretorio = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.txtDescricao = new DevExpress.XtraEditors.TextEdit();
            this.txtDiretorio = new DevExpress.XtraEditors.TextEdit();
            this.sbIdDiretorio = new Componentes.devexpress.cwk_DevButton();
            this.sbOrdem = new DevExpress.XtraEditors.SimpleButton();
            this.sbDia = new DevExpress.XtraEditors.SimpleButton();
            this.sbMes = new DevExpress.XtraEditors.SimpleButton();
            this.sbAno2 = new DevExpress.XtraEditors.SimpleButton();
            this.sbAno4 = new DevExpress.XtraEditors.SimpleButton();
            this.sbHora = new DevExpress.XtraEditors.SimpleButton();
            this.sbMinuto = new DevExpress.XtraEditors.SimpleButton();
            this.sbFuncionario = new DevExpress.XtraEditors.SimpleButton();
            this.sbRelogio = new DevExpress.XtraEditors.SimpleButton();
            this.sbSeparador = new DevExpress.XtraEditors.SimpleButton();
            this.txtLayout = new DevExpress.XtraEditors.MemoEdit();
            this.sbDesfazer = new DevExpress.XtraEditors.SimpleButton();
            this.gcLayoutLivre = new DevExpress.XtraEditors.GroupControl();
            this.cbFormatoBilhete = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblFormato = new DevExpress.XtraEditors.LabelControl();
            this.chbBimporta = new DevExpress.XtraEditors.CheckEdit();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sbIdRep = new Componentes.devexpress.cwk_DevButton();
            this.cbIdRep = new Componentes.devexpress.cwk_DevLookup();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiretorio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLayout.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLayoutLivre)).BeginInit();
            this.gcLayoutLivre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbFormatoBilhete.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbBimporta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdRep.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 400);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.sbIdRep);
            this.xtraTabPage1.Controls.Add(this.cbIdRep);
            this.xtraTabPage1.Controls.Add(this.chbBimporta);
            this.xtraTabPage1.Controls.Add(this.lblFormato);
            this.xtraTabPage1.Controls.Add(this.cbFormatoBilhete);
            this.xtraTabPage1.Controls.Add(this.gcLayoutLivre);
            this.xtraTabPage1.Controls.Add(this.sbIdDiretorio);
            this.xtraTabPage1.Controls.Add(this.txtDiretorio);
            this.xtraTabPage1.Controls.Add(this.txtDescricao);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblDiretorio);
            this.xtraTabPage1.Controls.Add(this.lblDescricao);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(519, 394);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 418);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 418);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 418);
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(23, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // lblDescricao
            // 
            this.lblDescricao.Location = new System.Drawing.Point(10, 42);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(50, 13);
            this.lblDescricao.TabIndex = 2;
            this.lblDescricao.Text = "Descrição:";
            // 
            // lblDiretorio
            // 
            this.lblDiretorio.Location = new System.Drawing.Point(15, 68);
            this.lblDiretorio.Name = "lblDiretorio";
            this.lblDiretorio.Size = new System.Drawing.Size(45, 13);
            this.lblDiretorio.TabIndex = 4;
            this.lblDiretorio.Text = "Diretório:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(66, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, false)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(66, 39);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(441, 20);
            this.txtDescricao.TabIndex = 3;
            // 
            // txtDiretorio
            // 
            this.txtDiretorio.EditValue = "";
            this.txtDiretorio.Location = new System.Drawing.Point(66, 65);
            this.txtDiretorio.Name = "txtDiretorio";
            this.txtDiretorio.Properties.MaxLength = 255;
            this.txtDiretorio.Size = new System.Drawing.Size(411, 20);
            this.txtDiretorio.TabIndex = 5;
            this.txtDiretorio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDiretorio_KeyDown);
            // 
            // sbIdDiretorio
            // 
            this.sbIdDiretorio.Location = new System.Drawing.Point(483, 65);
            this.sbIdDiretorio.Name = "sbIdDiretorio";
            this.sbIdDiretorio.Size = new System.Drawing.Size(24, 20);
            this.sbIdDiretorio.TabIndex = 6;
            this.sbIdDiretorio.TabStop = false;
            this.sbIdDiretorio.Text = "...";
            this.sbIdDiretorio.Click += new System.EventHandler(this.sbIdDiretorio_Click);
            // 
            // sbOrdem
            // 
            this.sbOrdem.Location = new System.Drawing.Point(6, 31);
            this.sbOrdem.Name = "sbOrdem";
            this.sbOrdem.Size = new System.Drawing.Size(92, 46);
            this.sbOrdem.TabIndex = 0;
            this.sbOrdem.Text = "&Ordem";
            this.sbOrdem.Click += new System.EventHandler(this.sbOrdem_Click);
            // 
            // sbDia
            // 
            this.sbDia.Location = new System.Drawing.Point(104, 31);
            this.sbDia.Name = "sbDia";
            this.sbDia.Size = new System.Drawing.Size(92, 46);
            this.sbDia.TabIndex = 1;
            this.sbDia.Text = "&Dia";
            this.sbDia.Click += new System.EventHandler(this.sbDia_Click);
            // 
            // sbMes
            // 
            this.sbMes.Location = new System.Drawing.Point(202, 31);
            this.sbMes.Name = "sbMes";
            this.sbMes.Size = new System.Drawing.Size(92, 46);
            this.sbMes.TabIndex = 2;
            this.sbMes.Text = "&Mês";
            this.sbMes.Click += new System.EventHandler(this.sbMes_Click);
            // 
            // sbAno2
            // 
            this.sbAno2.Location = new System.Drawing.Point(300, 31);
            this.sbAno2.Name = "sbAno2";
            this.sbAno2.Size = new System.Drawing.Size(92, 46);
            this.sbAno2.TabIndex = 3;
            this.sbAno2.Text = "Ano(&2)";
            this.sbAno2.Click += new System.EventHandler(this.sbAno2_Click);
            // 
            // sbAno4
            // 
            this.sbAno4.Location = new System.Drawing.Point(398, 31);
            this.sbAno4.Name = "sbAno4";
            this.sbAno4.Size = new System.Drawing.Size(92, 46);
            this.sbAno4.TabIndex = 4;
            this.sbAno4.Text = "Ano(&4)";
            this.sbAno4.Click += new System.EventHandler(this.sbAno4_Click);
            // 
            // sbHora
            // 
            this.sbHora.Location = new System.Drawing.Point(6, 83);
            this.sbHora.Name = "sbHora";
            this.sbHora.Size = new System.Drawing.Size(92, 46);
            this.sbHora.TabIndex = 5;
            this.sbHora.Text = "&Hora";
            this.sbHora.Click += new System.EventHandler(this.sbHora_Click);
            // 
            // sbMinuto
            // 
            this.sbMinuto.Location = new System.Drawing.Point(104, 83);
            this.sbMinuto.Name = "sbMinuto";
            this.sbMinuto.Size = new System.Drawing.Size(92, 46);
            this.sbMinuto.TabIndex = 6;
            this.sbMinuto.Text = "M&inuto";
            this.sbMinuto.Click += new System.EventHandler(this.sbMinuto_Click);
            // 
            // sbFuncionario
            // 
            this.sbFuncionario.Location = new System.Drawing.Point(202, 83);
            this.sbFuncionario.Name = "sbFuncionario";
            this.sbFuncionario.Size = new System.Drawing.Size(92, 46);
            this.sbFuncionario.TabIndex = 7;
            this.sbFuncionario.Text = "&Funcionário";
            this.sbFuncionario.Click += new System.EventHandler(this.sbFuncionario_Click);
            // 
            // sbRelogio
            // 
            this.sbRelogio.Location = new System.Drawing.Point(300, 83);
            this.sbRelogio.Name = "sbRelogio";
            this.sbRelogio.Size = new System.Drawing.Size(92, 46);
            this.sbRelogio.TabIndex = 8;
            this.sbRelogio.Text = "&Relógio";
            this.sbRelogio.Click += new System.EventHandler(this.sbRelogio_Click);
            // 
            // sbSeparador
            // 
            this.sbSeparador.Location = new System.Drawing.Point(398, 83);
            this.sbSeparador.Name = "sbSeparador";
            this.sbSeparador.Size = new System.Drawing.Size(92, 46);
            this.sbSeparador.TabIndex = 9;
            this.sbSeparador.Text = "&Separador";
            this.sbSeparador.Click += new System.EventHandler(this.sbSeparador_Click);
            // 
            // txtLayout
            // 
            this.txtLayout.Location = new System.Drawing.Point(6, 135);
            this.txtLayout.Name = "txtLayout";
            this.txtLayout.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtLayout.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.txtLayout.Properties.Appearance.Options.UseBackColor = true;
            this.txtLayout.Properties.Appearance.Options.UseForeColor = true;
            this.txtLayout.Properties.ReadOnly = true;
            this.txtLayout.Size = new System.Drawing.Size(484, 71);
            this.txtLayout.TabIndex = 10;
            // 
            // sbDesfazer
            // 
            this.sbDesfazer.Location = new System.Drawing.Point(6, 212);
            this.sbDesfazer.Name = "sbDesfazer";
            this.sbDesfazer.Size = new System.Drawing.Size(75, 23);
            this.sbDesfazer.TabIndex = 11;
            this.sbDesfazer.Text = "D&esfazer";
            this.sbDesfazer.Click += new System.EventHandler(this.sbDesfazer_Click);
            // 
            // gcLayoutLivre
            // 
            this.gcLayoutLivre.Controls.Add(this.sbDesfazer);
            this.gcLayoutLivre.Controls.Add(this.txtLayout);
            this.gcLayoutLivre.Controls.Add(this.sbSeparador);
            this.gcLayoutLivre.Controls.Add(this.sbRelogio);
            this.gcLayoutLivre.Controls.Add(this.sbFuncionario);
            this.gcLayoutLivre.Controls.Add(this.sbMinuto);
            this.gcLayoutLivre.Controls.Add(this.sbHora);
            this.gcLayoutLivre.Controls.Add(this.sbAno4);
            this.gcLayoutLivre.Controls.Add(this.sbAno2);
            this.gcLayoutLivre.Controls.Add(this.sbMes);
            this.gcLayoutLivre.Controls.Add(this.sbDia);
            this.gcLayoutLivre.Controls.Add(this.sbOrdem);
            this.gcLayoutLivre.Location = new System.Drawing.Point(10, 143);
            this.gcLayoutLivre.Name = "gcLayoutLivre";
            this.gcLayoutLivre.Size = new System.Drawing.Size(497, 243);
            this.gcLayoutLivre.TabIndex = 13;
            this.gcLayoutLivre.Text = "Layout Livre";
            // 
            // cbFormatoBilhete
            // 
            this.cbFormatoBilhete.Location = new System.Drawing.Point(66, 91);
            this.cbFormatoBilhete.Name = "cbFormatoBilhete";
            this.cbFormatoBilhete.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbFormatoBilhete.Properties.Items.AddRange(new object[] {
            "TOPDATA (5 Digitos)",
            "TOPDATA (16 Digitos)",
            "Layout Livre",
            "AFD",
            "REP (Importação Direta do Relógio)",
            "AFD Inmetro"});
            this.cbFormatoBilhete.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbFormatoBilhete.Size = new System.Drawing.Size(327, 20);
            this.cbFormatoBilhete.TabIndex = 8;
            this.cbFormatoBilhete.SelectedIndexChanged += new System.EventHandler(this.cbFormatoBilhete_SelectedIndexChanged);
            // 
            // lblFormato
            // 
            this.lblFormato.Location = new System.Drawing.Point(16, 94);
            this.lblFormato.Name = "lblFormato";
            this.lblFormato.Size = new System.Drawing.Size(44, 13);
            this.lblFormato.TabIndex = 7;
            this.lblFormato.Text = "Formato:";
            // 
            // chbBimporta
            // 
            this.chbBimporta.Location = new System.Drawing.Point(399, 91);
            this.chbBimporta.Name = "chbBimporta";
            this.chbBimporta.Properties.Caption = "Importar Bilhetes?";
            this.chbBimporta.Size = new System.Drawing.Size(108, 19);
            this.chbBimporta.TabIndex = 9;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // sbIdRep
            // 
            this.sbIdRep.Location = new System.Drawing.Point(483, 117);
            this.sbIdRep.Name = "sbIdRep";
            this.sbIdRep.Size = new System.Drawing.Size(24, 20);
            this.sbIdRep.TabIndex = 12;
            this.sbIdRep.TabStop = false;
            this.sbIdRep.Text = "...";
            this.sbIdRep.Click += new System.EventHandler(this.sbIdRep_Click);
            // 
            // cbIdRep
            // 
            this.cbIdRep.ButtonLookup = this.sbIdRep;
            this.cbIdRep.EditValue = 0;
            this.cbIdRep.Key = System.Windows.Forms.Keys.F5;
            this.cbIdRep.Location = new System.Drawing.Point(66, 117);
            this.cbIdRep.Name = "cbIdRep";
            this.cbIdRep.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdRep.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("local", "Local", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código")});
            this.cbIdRep.Properties.DisplayMember = "local";
            this.cbIdRep.Properties.NullText = "";
            this.cbIdRep.Properties.ValueMember = "id";
            this.cbIdRep.Size = new System.Drawing.Size(411, 20);
            this.cbIdRep.TabIndex = 11;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(21, 120);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(39, 13);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "Relógio:";
            // 
            // FormManutTipoBilhetes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 453);
            this.Name = "FormManutTipoBilhetes";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiretorio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLayout.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcLayoutLivre)).EndInit();
            this.gcLayoutLivre.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbFormatoBilhete.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbBimporta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdRep.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.LabelControl lblDiretorio;
        private DevExpress.XtraEditors.LabelControl lblDescricao;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.TextEdit txtDescricao;
        private Componentes.devexpress.cwk_DevButton sbIdDiretorio;
        private DevExpress.XtraEditors.TextEdit txtDiretorio;
        private DevExpress.XtraEditors.GroupControl gcLayoutLivre;
        private DevExpress.XtraEditors.SimpleButton sbDesfazer;
        private DevExpress.XtraEditors.MemoEdit txtLayout;
        private DevExpress.XtraEditors.SimpleButton sbSeparador;
        private DevExpress.XtraEditors.SimpleButton sbRelogio;
        private DevExpress.XtraEditors.SimpleButton sbFuncionario;
        private DevExpress.XtraEditors.SimpleButton sbMinuto;
        private DevExpress.XtraEditors.SimpleButton sbHora;
        private DevExpress.XtraEditors.SimpleButton sbAno4;
        private DevExpress.XtraEditors.SimpleButton sbAno2;
        private DevExpress.XtraEditors.SimpleButton sbMes;
        private DevExpress.XtraEditors.SimpleButton sbDia;
        private DevExpress.XtraEditors.SimpleButton sbOrdem;
        private DevExpress.XtraEditors.LabelControl lblFormato;
        private DevExpress.XtraEditors.ComboBoxEdit cbFormatoBilhete;
        private DevExpress.XtraEditors.CheckEdit chbBimporta;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Componentes.devexpress.cwk_DevButton sbIdRep;
        private Componentes.devexpress.cwk_DevLookup cbIdRep;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
