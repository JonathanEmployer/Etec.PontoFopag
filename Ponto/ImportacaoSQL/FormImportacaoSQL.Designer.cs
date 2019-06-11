namespace ImportacaoSQL
{
    partial class FormImportacaoSQL
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportacaoSQL));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.sbAjuda = new DevExpress.XtraEditors.SimpleButton();
            this.sbImportar = new DevExpress.XtraEditors.SimpleButton();
            this.txtBanco = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtServidor = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMensagem = new System.Windows.Forms.Label();
            this.txtFB = new System.Windows.Forms.TextBox();
            this.sbAvancadas = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.sbFb = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // sbCancelar
            // 
            this.sbCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sbCancelar.Image = global::ImportacaoSQL.Properties.Resources.cancelar_copy;
            this.sbCancelar.ImageIndex = 2;
            this.sbCancelar.Location = new System.Drawing.Point(381, 104);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(75, 23);
            this.sbCancelar.TabIndex = 2;
            this.sbCancelar.Text = "&Cancelar";
            this.sbCancelar.Click += new System.EventHandler(this.sbCancelar_Click);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sbAjuda.Image = global::ImportacaoSQL.Properties.Resources.Help_copy;
            this.sbAjuda.ImageIndex = 0;
            this.sbAjuda.Location = new System.Drawing.Point(12, 104);
            this.sbAjuda.Name = "sbAjuda";
            this.sbAjuda.Size = new System.Drawing.Size(75, 23);
            this.sbAjuda.TabIndex = 3;
            this.sbAjuda.Text = "A&juda";
            this.sbAjuda.Click += new System.EventHandler(this.sbAjuda_Click);
            // 
            // sbImportar
            // 
            this.sbImportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbImportar.Image = ((System.Drawing.Image)(resources.GetObject("sbImportar.Image")));
            this.sbImportar.ImageIndex = 0;
            this.sbImportar.Location = new System.Drawing.Point(300, 104);
            this.sbImportar.Name = "sbImportar";
            this.sbImportar.Size = new System.Drawing.Size(75, 23);
            this.sbImportar.TabIndex = 1;
            this.sbImportar.Text = "&Importar";
            this.sbImportar.Click += new System.EventHandler(this.sbImportar_Click);
            // 
            // txtBanco
            // 
            this.txtBanco.Location = new System.Drawing.Point(322, 35);
            this.txtBanco.Name = "txtBanco";
            this.txtBanco.Size = new System.Drawing.Size(116, 20);
            this.txtBanco.TabIndex = 42;
            this.txtBanco.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(248, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "SQL Banco: ";
            this.label4.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "SQL Usuário: ";
            this.label7.Visible = false;
            // 
            // txtServidor
            // 
            this.txtServidor.Location = new System.Drawing.Point(85, 35);
            this.txtServidor.Name = "txtServidor";
            this.txtServidor.Size = new System.Drawing.Size(121, 20);
            this.txtServidor.TabIndex = 40;
            this.txtServidor.Visible = false;
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(85, 61);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(121, 20);
            this.txtUsuario.TabIndex = 8;
            this.txtUsuario.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(248, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "SQL Senha: ";
            this.label6.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(85, 35);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(353, 26);
            this.progressBar1.TabIndex = 38;
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(322, 61);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(116, 20);
            this.txtSenha.TabIndex = 10;
            this.txtSenha.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SQL Servidor: ";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Firebird:";
            // 
            // lblMensagem
            // 
            this.lblMensagem.AutoSize = true;
            this.lblMensagem.Location = new System.Drawing.Point(85, 64);
            this.lblMensagem.Name = "lblMensagem";
            this.lblMensagem.Size = new System.Drawing.Size(64, 13);
            this.lblMensagem.TabIndex = 7;
            this.lblMensagem.Text = "[mensagem]";
            // 
            // txtFB
            // 
            this.txtFB.Location = new System.Drawing.Point(85, 9);
            this.txtFB.Name = "txtFB";
            this.txtFB.Size = new System.Drawing.Size(317, 20);
            this.txtFB.TabIndex = 1;
            this.txtFB.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFB_KeyDown);
            // 
            // sbAvancadas
            // 
            this.sbAvancadas.Location = new System.Drawing.Point(65, 87);
            this.sbAvancadas.Name = "sbAvancadas";
            this.sbAvancadas.Size = new System.Drawing.Size(173, 30);
            this.sbAvancadas.TabIndex = 49;
            this.sbAvancadas.Text = "Opções Avançadas";
            this.sbAvancadas.Visible = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.sbAvancadas);
            this.panelControl1.Controls.Add(this.txtFB);
            this.panelControl1.Controls.Add(this.lblMensagem);
            this.panelControl1.Controls.Add(this.sbFb);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.txtSenha);
            this.panelControl1.Controls.Add(this.progressBar1);
            this.panelControl1.Controls.Add(this.label6);
            this.panelControl1.Controls.Add(this.txtUsuario);
            this.panelControl1.Controls.Add(this.txtServidor);
            this.panelControl1.Controls.Add(this.label7);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.txtBanco);
            this.panelControl1.Location = new System.Drawing.Point(11, 11);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(445, 87);
            this.panelControl1.TabIndex = 0;
            // 
            // sbFb
            // 
            this.sbFb.Location = new System.Drawing.Point(408, 9);
            this.sbFb.Name = "sbFb";
            this.sbFb.Size = new System.Drawing.Size(30, 20);
            this.sbFb.TabIndex = 2;
            this.sbFb.TabStop = false;
            this.sbFb.Text = "...";
            this.sbFb.Click += new System.EventHandler(this.sbFb_Click);
            // 
            // FormImportacaoSQL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 135);
            this.Controls.Add(this.sbImportar);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.sbAjuda);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImportacaoSQL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Importação Dados Cwork Ponto MT";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormImportacaoSQL_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public DevExpress.XtraEditors.SimpleButton sbAjuda;
        public DevExpress.XtraEditors.SimpleButton sbCancelar;
        public DevExpress.XtraEditors.SimpleButton sbImportar;
        private System.Windows.Forms.TextBox txtBanco;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtServidor;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMensagem;
        private System.Windows.Forms.TextBox txtFB;
        private DevExpress.XtraEditors.SimpleButton sbAvancadas;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton sbFb;
    }
}