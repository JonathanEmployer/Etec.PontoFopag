namespace UI
{
    partial class FormCapturaFoto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCapturaFoto));
            this.lbcDispositivos = new DevExpress.XtraEditors.ListBoxControl();
            this.sbIniciarVisualizacao = new DevExpress.XtraEditors.SimpleButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sbSalvar = new DevExpress.XtraEditors.SimpleButton();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.sbAjuda = new DevExpress.XtraEditors.SimpleButton();
            this.picCapture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.lbcDispositivos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCapture)).BeginInit();
            this.SuspendLayout();
            // 
            // lbcDispositivos
            // 
            this.lbcDispositivos.Location = new System.Drawing.Point(12, 29);
            this.lbcDispositivos.Name = "lbcDispositivos";
            this.lbcDispositivos.Size = new System.Drawing.Size(218, 241);
            this.lbcDispositivos.TabIndex = 1;
            // 
            // sbIniciarVisualizacao
            // 
            this.sbIniciarVisualizacao.ImageIndex = 2;
            this.sbIniciarVisualizacao.ImageList = this.imageList1;
            this.sbIniciarVisualizacao.Location = new System.Drawing.Point(321, 276);
            this.sbIniciarVisualizacao.Name = "sbIniciarVisualizacao";
            this.sbIniciarVisualizacao.Size = new System.Drawing.Size(75, 23);
            this.sbIniciarVisualizacao.TabIndex = 3;
            this.sbIniciarVisualizacao.Text = "&Visualizar";
            this.sbIniciarVisualizacao.Click += new System.EventHandler(this.sbIniciarVisualizacao_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Gravar copy.ico");
            this.imageList1.Images.SetKeyName(1, "cancelar copy.ico");
            this.imageList1.Images.SetKeyName(2, "Consulta copy.ico");
            this.imageList1.Images.SetKeyName(3, "Help copy.ico");
            // 
            // sbSalvar
            // 
            this.sbSalvar.ImageIndex = 0;
            this.sbSalvar.ImageList = this.imageList1;
            this.sbSalvar.Location = new System.Drawing.Point(402, 276);
            this.sbSalvar.Name = "sbSalvar";
            this.sbSalvar.Size = new System.Drawing.Size(75, 23);
            this.sbSalvar.TabIndex = 4;
            this.sbSalvar.Text = "&Salvar";
            this.sbSalvar.Click += new System.EventHandler(this.sbSalvar_Click);
            // 
            // sbCancelar
            // 
            this.sbCancelar.ImageIndex = 1;
            this.sbCancelar.ImageList = this.imageList1;
            this.sbCancelar.Location = new System.Drawing.Point(483, 276);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(75, 23);
            this.sbCancelar.TabIndex = 5;
            this.sbCancelar.Text = "&Cancelar";
            this.sbCancelar.Click += new System.EventHandler(this.sbCancelar_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(32, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(119, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Dispositivos encontrados";
            // 
            // sbAjuda
            // 
            this.sbAjuda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sbAjuda.ImageIndex = 3;
            this.sbAjuda.ImageList = this.imageList1;
            this.sbAjuda.Location = new System.Drawing.Point(12, 276);
            this.sbAjuda.Name = "sbAjuda";
            this.sbAjuda.Size = new System.Drawing.Size(75, 23);
            this.sbAjuda.TabIndex = 6;
            this.sbAjuda.Text = "A&juda";
            this.sbAjuda.Click += new System.EventHandler(this.sbAjuda_Click);
            // 
            // picCapture
            // 
            this.picCapture.Location = new System.Drawing.Point(236, 29);
            this.picCapture.Name = "picCapture";
            this.picCapture.Size = new System.Drawing.Size(322, 241);
            this.picCapture.TabIndex = 7;
            this.picCapture.TabStop = false;
            // 
            // FormCapturaFoto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 304);
            this.Controls.Add(this.picCapture);
            this.Controls.Add(this.sbAjuda);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.sbSalvar);
            this.Controls.Add(this.sbIniciarVisualizacao);
            this.Controls.Add(this.lbcDispositivos);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormCapturaFoto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Captura Foto";
            this.Load += new System.EventHandler(this.FormCapturaFoto_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCapturaFoto_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.lbcDispositivos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCapture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl lbcDispositivos;
        private DevExpress.XtraEditors.SimpleButton sbIniciarVisualizacao;
        private DevExpress.XtraEditors.SimpleButton sbSalvar;
        private DevExpress.XtraEditors.SimpleButton sbCancelar;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.ImageList imageList1;
        public DevExpress.XtraEditors.SimpleButton sbAjuda;
        private System.Windows.Forms.PictureBox picCapture;
    }
}