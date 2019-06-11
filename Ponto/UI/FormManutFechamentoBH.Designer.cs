namespace UI
{
    partial class FormManutFechamentoBH
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManutFechamentoBH));
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.lblData = new DevExpress.XtraEditors.LabelControl();
            this.txtData = new DevExpress.XtraEditors.DateEdit();
            this.chbEfetivado = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.rgTipo = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbEfetivado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 107);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Controls.Add(this.chbEfetivado);
            this.xtraTabPage1.Controls.Add(this.lblData);
            this.xtraTabPage1.Controls.Add(this.txtData);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(516, 98);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 125);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 125);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 125);
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(57, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Properties.ReadOnly = true;
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(14, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // lblData
            // 
            this.lblData.Location = new System.Drawing.Point(24, 42);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(27, 13);
            this.lblData.TabIndex = 2;
            this.lblData.Text = "Data:";
            // 
            // txtData
            // 
            this.txtData.EditValue = null;
            this.txtData.Location = new System.Drawing.Point(57, 39);
            this.txtData.Name = "txtData";
            this.txtData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtData.Properties.ReadOnly = true;
            this.txtData.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtData.Size = new System.Drawing.Size(80, 20);
            this.txtData.TabIndex = 3;
            // 
            // chbEfetivado
            // 
            this.chbEfetivado.Location = new System.Drawing.Point(55, 65);
            this.chbEfetivado.Name = "chbEfetivado";
            this.chbEfetivado.Properties.Caption = "Efetivado";
            this.chbEfetivado.Properties.ReadOnly = true;
            this.chbEfetivado.Size = new System.Drawing.Size(75, 19);
            this.chbEfetivado.TabIndex = 4;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.rgTipo);
            this.groupControl1.Location = new System.Drawing.Point(170, 13);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(335, 71);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "Tipo";
            // 
            // rgTipo
            // 
            this.rgTipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipo.Location = new System.Drawing.Point(2, 20);
            this.rgTipo.Name = "rgTipo";
            this.rgTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Empresa"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Departamento"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "Função"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Funcionário")});
            this.rgTipo.Properties.ReadOnly = true;
            this.rgTipo.Size = new System.Drawing.Size(331, 49);
            this.rgTipo.TabIndex = 0;
            // 
            // FormManutFechamentoBH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 160);
            this.Name = "FormManutFechamentoBH";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbEfetivado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.LabelControl lblData;
        private DevExpress.XtraEditors.DateEdit txtData;
        private DevExpress.XtraEditors.CheckEdit chbEfetivado;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.RadioGroup rgTipo;
    }
}
