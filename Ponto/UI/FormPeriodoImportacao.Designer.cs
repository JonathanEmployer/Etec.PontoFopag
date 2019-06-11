namespace UI
{
    partial class FormPeriodoImportacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPeriodoImportacao));
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtDataFinal = new DevExpress.XtraEditors.DateEdit();
            this.txtDataInicial = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.sbImportar = new DevExpress.XtraEditors.SimpleButton();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataInicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(183, 40);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(6, 13);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "à";
            // 
            // txtDataFinal
            // 
            this.txtDataFinal.EditValue = null;
            this.txtDataFinal.Location = new System.Drawing.Point(195, 37);
            this.txtDataFinal.Name = "txtDataFinal";
            this.txtDataFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataFinal.Size = new System.Drawing.Size(104, 20);
            this.txtDataFinal.TabIndex = 4;
            // 
            // txtDataInicial
            // 
            this.txtDataInicial.EditValue = null;
            this.txtDataInicial.Location = new System.Drawing.Point(73, 37);
            this.txtDataInicial.Name = "txtDataInicial";
            this.txtDataInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataInicial.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataInicial.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataInicial.Size = new System.Drawing.Size(104, 20);
            this.txtDataInicial.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 40);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(55, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Período de:";
            // 
            // sbImportar
            // 
            this.sbImportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbImportar.Image = ((System.Drawing.Image)(resources.GetObject("sbImportar.Image")));
            this.sbImportar.ImageIndex = 0;
            this.sbImportar.Location = new System.Drawing.Point(145, 70);
            this.sbImportar.Name = "sbImportar";
            this.sbImportar.Size = new System.Drawing.Size(75, 23);
            this.sbImportar.TabIndex = 5;
            this.sbImportar.Text = "&Sim";
            this.sbImportar.Click += new System.EventHandler(this.sbImportar_Click);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sbCancelar.Image = ((System.Drawing.Image)(resources.GetObject("sbCancelar.Image")));
            this.sbCancelar.ImageIndex = 1;
            this.sbCancelar.Location = new System.Drawing.Point(226, 70);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(75, 23);
            this.sbCancelar.TabIndex = 6;
            this.sbCancelar.Text = "&Não";
            this.sbCancelar.Click += new System.EventHandler(this.sbCancelar_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(32, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(248, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Deseja importar os bilhetes no período selecionado?";
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // FormPeriodoImportacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 105);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.sbImportar);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtDataFinal);
            this.Controls.Add(this.txtDataInicial);
            this.Controls.Add(this.labelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPeriodoImportacao";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Importação de Bilhetes";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormPeriodoImportacao_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataInicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.DateEdit txtDataFinal;
        private DevExpress.XtraEditors.DateEdit txtDataInicial;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        public DevExpress.XtraEditors.SimpleButton sbImportar;
        public DevExpress.XtraEditors.SimpleButton sbCancelar;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
    }
}