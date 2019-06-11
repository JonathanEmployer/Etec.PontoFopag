namespace UI
{
    partial class FormManutGrupo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManutGrupo));
            this.txtNome = new DevExpress.XtraEditors.TextEdit();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.lblNome = new DevExpress.XtraEditors.LabelControl();
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.rgAcesso = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNome.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgAcesso.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 136);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Controls.Add(this.txtNome);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblNome);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(516, 127);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 154);
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
            this.sbGravar.Location = new System.Drawing.Point(381, 154);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 154);
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(55, 39);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(450, 20);
            this.txtNome.TabIndex = 3;
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(55, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Properties.ValidateOnEnterKey = true;
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // lblNome
            // 
            this.lblNome.Location = new System.Drawing.Point(18, 42);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(31, 13);
            this.lblNome.TabIndex = 2;
            this.lblNome.Text = "Nome:";
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(12, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.rgAcesso);
            this.groupControl1.Location = new System.Drawing.Point(55, 65);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(201, 51);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "Padrão - Liberado?";
            // 
            // rgAcesso
            // 
            this.rgAcesso.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgAcesso.Location = new System.Drawing.Point(2, 20);
            this.rgAcesso.Name = "rgAcesso";
            this.rgAcesso.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Sim"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Não")});
            this.rgAcesso.Size = new System.Drawing.Size(197, 29);
            this.rgAcesso.TabIndex = 0;
            // 
            // FormManutGrupo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 189);
            this.Name = "FormManutGrupo";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNome.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgAcesso.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.RadioGroup rgAcesso;
        private DevExpress.XtraEditors.TextEdit txtNome;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.LabelControl lblNome;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
    }
}
