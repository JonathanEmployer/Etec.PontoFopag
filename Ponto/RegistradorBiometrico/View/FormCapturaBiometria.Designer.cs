namespace RegistradorBiometrico.View
{
    partial class FormCapturaBiometria
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCapturaBiometria));
            this.pbFinger = new System.Windows.Forms.PictureBox();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.cbConsolidacao1 = new DevExpress.XtraEditors.CheckEdit();
            this.cbConsolidacao3 = new DevExpress.XtraEditors.CheckEdit();
            this.cbConsolidacao2 = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.sbInserir = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbFinger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbConsolidacao1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbConsolidacao3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbConsolidacao2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pbFinger
            // 
            this.pbFinger.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pbFinger.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbFinger.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbFinger.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pbFinger.Location = new System.Drawing.Point(12, 36);
            this.pbFinger.Name = "pbFinger";
            this.pbFinger.Size = new System.Drawing.Size(294, 284);
            this.pbFinger.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbFinger.TabIndex = 11;
            this.pbFinger.TabStop = false;
            // 
            // sbCancelar
            // 
            this.sbCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbCancelar.Image = ((System.Drawing.Image)(resources.GetObject("sbCancelar.Image")));
            this.sbCancelar.Location = new System.Drawing.Point(165, 357);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(118, 37);
            this.sbCancelar.TabIndex = 19;
            this.sbCancelar.Text = "&Cancelar";
            this.sbCancelar.Click += new System.EventHandler(this.sbCancelar_Click);
            // 
            // cbConsolidacao1
            // 
            this.cbConsolidacao1.Location = new System.Drawing.Point(10, 326);
            this.cbConsolidacao1.Name = "cbConsolidacao1";
            this.cbConsolidacao1.Properties.Caption = "Consolidacao 1";
            this.cbConsolidacao1.Properties.ReadOnly = true;
            this.cbConsolidacao1.Size = new System.Drawing.Size(94, 19);
            this.cbConsolidacao1.TabIndex = 23;
            // 
            // cbConsolidacao3
            // 
            this.cbConsolidacao3.Location = new System.Drawing.Point(210, 326);
            this.cbConsolidacao3.Name = "cbConsolidacao3";
            this.cbConsolidacao3.Properties.Caption = "Consolidacao 3";
            this.cbConsolidacao3.Properties.ReadOnly = true;
            this.cbConsolidacao3.Size = new System.Drawing.Size(94, 19);
            this.cbConsolidacao3.TabIndex = 24;
            // 
            // cbConsolidacao2
            // 
            this.cbConsolidacao2.Location = new System.Drawing.Point(110, 326);
            this.cbConsolidacao2.Name = "cbConsolidacao2";
            this.cbConsolidacao2.Properties.Caption = "Consolidacao 2";
            this.cbConsolidacao2.Properties.ReadOnly = true;
            this.cbConsolidacao2.Size = new System.Drawing.Size(94, 19);
            this.cbConsolidacao2.TabIndex = 25;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(12, 12);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(188, 13);
            this.labelControl8.TabIndex = 26;
            this.labelControl8.Text = "Capture três imagens do mesmo dedo. ";
            this.labelControl8.ToolTip = "As três imagens do mesmo dedo ajudam o programa a formar uma Biometria de melhor " +
    "qualidade.";
            // 
            // sbInserir
            // 
            this.sbInserir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbInserir.Image = ((System.Drawing.Image)(resources.GetObject("sbInserir.Image")));
            this.sbInserir.Location = new System.Drawing.Point(41, 357);
            this.sbInserir.Name = "sbInserir";
            this.sbInserir.Size = new System.Drawing.Size(118, 37);
            this.sbInserir.TabIndex = 27;
            this.sbInserir.Text = "&Inserir";
            this.sbInserir.Click += new System.EventHandler(this.sbInserir_Click);
            // 
            // FormCapturaBiometria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 406);
            this.Controls.Add(this.sbInserir);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.cbConsolidacao2);
            this.Controls.Add(this.cbConsolidacao3);
            this.Controls.Add(this.cbConsolidacao1);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.pbFinger);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(331, 445);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(288, 357);
            this.Name = "FormCapturaBiometria";
            this.Text = "Capturar Biometria";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCapturaBiometria_FormClosing);
            this.Shown += new System.EventHandler(this.FormCapturaBiometria_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbFinger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbConsolidacao1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbConsolidacao3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbConsolidacao2.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbFinger;
        public DevExpress.XtraEditors.SimpleButton sbCancelar;
        private DevExpress.XtraEditors.CheckEdit cbConsolidacao1;
        private DevExpress.XtraEditors.CheckEdit cbConsolidacao3;
        private DevExpress.XtraEditors.CheckEdit cbConsolidacao2;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        public DevExpress.XtraEditors.SimpleButton sbInserir;
    }
}