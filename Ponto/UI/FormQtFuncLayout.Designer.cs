namespace UI
{
    partial class FormQtFuncLayout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormQtFuncLayout));
            this.sbOk = new DevExpress.XtraEditors.SimpleButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.txtQtDigitos = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQtDigitos.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // sbOk
            // 
            this.sbOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbOk.ImageIndex = 0;
            this.sbOk.ImageList = this.imageList1;
            this.sbOk.Location = new System.Drawing.Point(277, 78);
            this.sbOk.Name = "sbOk";
            this.sbOk.Size = new System.Drawing.Size(75, 23);
            this.sbOk.TabIndex = 1;
            this.sbOk.Text = "&Ok";
            this.sbOk.Click += new System.EventHandler(this.sbOk_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Selecionar copy.ico");
            this.imageList1.Images.SetKeyName(1, "cancelar copy.ico");
            // 
            // sbCancelar
            // 
            this.sbCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbCancelar.ImageIndex = 1;
            this.sbCancelar.ImageList = this.imageList1;
            this.sbCancelar.Location = new System.Drawing.Point(358, 78);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(75, 23);
            this.sbCancelar.TabIndex = 2;
            this.sbCancelar.Text = "&Cancelar";
            this.sbCancelar.Click += new System.EventHandler(this.sbCancelar_Click);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraTabControl1.Location = new System.Drawing.Point(12, 12);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControl1.Size = new System.Drawing.Size(421, 60);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            this.xtraTabControl1.Text = "xtraTabControl1";
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtQtDigitos);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(412, 51);
            this.xtraTabPage1.Text = "xtraTabPage1";
            // 
            // txtQtDigitos
            // 
            this.txtQtDigitos.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtQtDigitos.Location = new System.Drawing.Point(299, 17);
            this.txtQtDigitos.Name = "txtQtDigitos";
            this.txtQtDigitos.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtQtDigitos.Properties.IsFloatValue = false;
            this.txtQtDigitos.Properties.Mask.EditMask = "N00";
            this.txtQtDigitos.Properties.MaxLength = 2;
            this.txtQtDigitos.Properties.MaxValue = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.txtQtDigitos.Size = new System.Drawing.Size(100, 20);
            this.txtQtDigitos.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(280, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Informe o número de dígitos do funcionário (entre 1 e 16):";
            // 
            // FormQtFuncLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 113);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.sbOk);
            this.Controls.Add(this.sbCancelar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormQtFuncLayout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Número de dígitos do funcionário";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtQtDigitos.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.SimpleButton sbOk;
        public DevExpress.XtraEditors.SimpleButton sbCancelar;
        public DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        public DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.SpinEdit txtQtDigitos;
    }
}