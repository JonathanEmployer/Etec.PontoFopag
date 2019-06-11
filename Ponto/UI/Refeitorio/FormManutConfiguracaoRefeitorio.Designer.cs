namespace UI
{
    partial class FormManutConfiguracaoRefeitorio
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
            this.gcTipoConexao = new DevExpress.XtraEditors.GroupControl();
            this.rgTipoConexao = new DevExpress.XtraEditors.RadioGroup();
            this.gcPortaSerial = new DevExpress.XtraEditors.GroupControl();
            this.rgPorta = new DevExpress.XtraEditors.RadioGroup();
            this.txtPortaTCP = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.txtCartaoMestre = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtQtDias = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTipoConexao)).BeginInit();
            this.gcTipoConexao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoConexao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPortaSerial)).BeginInit();
            this.gcPortaSerial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgPorta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPortaTCP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCartaoMestre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQtDias.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.xtraTabControl1.Size = new System.Drawing.Size(492, 183);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Appearance.PageClient.BackColor = System.Drawing.Color.DimGray;
            this.xtraTabPage1.Appearance.PageClient.Options.UseBackColor = true;
            this.xtraTabPage1.Controls.Add(this.labelControl4);
            this.xtraTabPage1.Controls.Add(this.txtQtDias);
            this.xtraTabPage1.Controls.Add(this.labelControl3);
            this.xtraTabPage1.Controls.Add(this.labelControl2);
            this.xtraTabPage1.Controls.Add(this.txtCartaoMestre);
            this.xtraTabPage1.Controls.Add(this.txtPortaTCP);
            this.xtraTabPage1.Controls.Add(this.labelControl7);
            this.xtraTabPage1.Controls.Add(this.gcPortaSerial);
            this.xtraTabPage1.Controls.Add(this.gcTipoConexao);
            this.xtraTabPage1.Size = new System.Drawing.Size(483, 152);
            this.xtraTabPage1.Text = "Equipamento";
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(429, 201);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(348, 201);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 201);
            // 
            // gcTipoConexao
            // 
            this.gcTipoConexao.Controls.Add(this.rgTipoConexao);
            this.gcTipoConexao.Location = new System.Drawing.Point(86, 11);
            this.gcTipoConexao.Name = "gcTipoConexao";
            this.gcTipoConexao.Size = new System.Drawing.Size(179, 78);
            this.gcTipoConexao.TabIndex = 0;
            this.gcTipoConexao.TabStop = true;
            this.gcTipoConexao.Text = "Tipo Conexão";
            // 
            // rgTipoConexao
            // 
            this.rgTipoConexao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipoConexao.EditValue = -1;
            this.rgTipoConexao.Location = new System.Drawing.Point(2, 20);
            this.rgTipoConexao.Name = "rgTipoConexao";
            this.rgTipoConexao.Properties.EnableFocusRect = true;
            this.rgTipoConexao.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Serial"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "TCP/IP com porta variável"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "TCP/IP com porta fixa")});
            this.rgTipoConexao.Size = new System.Drawing.Size(175, 56);
            this.rgTipoConexao.TabIndex = 0;
            this.rgTipoConexao.SelectedIndexChanged += new System.EventHandler(this.rgTipoConexao_SelectedIndexChanged);
            // 
            // gcPortaSerial
            // 
            this.gcPortaSerial.Controls.Add(this.rgPorta);
            this.gcPortaSerial.Location = new System.Drawing.Point(271, 11);
            this.gcPortaSerial.Name = "gcPortaSerial";
            this.gcPortaSerial.Size = new System.Drawing.Size(203, 78);
            this.gcPortaSerial.TabIndex = 1;
            this.gcPortaSerial.TabStop = true;
            this.gcPortaSerial.Text = "Porta Serial";
            // 
            // rgPorta
            // 
            this.rgPorta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgPorta.EditValue = -1;
            this.rgPorta.Enabled = false;
            this.rgPorta.Location = new System.Drawing.Point(2, 20);
            this.rgPorta.Name = "rgPorta";
            this.rgPorta.Properties.EnableFocusRect = true;
            this.rgPorta.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Com1"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Com2"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "Com3"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(4, "Com4")});
            this.rgPorta.Size = new System.Drawing.Size(199, 56);
            this.rgPorta.TabIndex = 0;
            // 
            // txtPortaTCP
            // 
            this.txtPortaTCP.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtPortaTCP.Enabled = false;
            this.txtPortaTCP.Location = new System.Drawing.Point(86, 95);
            this.txtPortaTCP.Name = "txtPortaTCP";
            this.txtPortaTCP.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtPortaTCP.Properties.IsFloatValue = false;
            this.txtPortaTCP.Properties.Mask.EditMask = "N00";
            this.txtPortaTCP.Size = new System.Drawing.Size(80, 20);
            this.txtPortaTCP.TabIndex = 3;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(14, 98);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(66, 13);
            this.labelControl7.TabIndex = 2;
            this.labelControl7.Text = "Porta TCP/IP:";
            // 
            // txtCartaoMestre
            // 
            this.txtCartaoMestre.Location = new System.Drawing.Point(84, 124);
            this.txtCartaoMestre.Name = "txtCartaoMestre";
            this.txtCartaoMestre.Properties.PasswordChar = '*';
            this.txtCartaoMestre.Size = new System.Drawing.Size(109, 20);
            this.txtCartaoMestre.TabIndex = 6;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(5, 127);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(73, 13);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Cartão mestre:";
            // 
            // txtQtDias
            // 
            this.txtQtDias.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtQtDias.Location = new System.Drawing.Point(392, 124);
            this.txtQtDias.Name = "txtQtDias";
            this.txtQtDias.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtQtDias.Properties.IsFloatValue = false;
            this.txtQtDias.Properties.Mask.EditMask = "N00";
            this.txtQtDias.Size = new System.Drawing.Size(80, 20);
            this.txtQtDias.TabIndex = 8;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(234, 127);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(152, 13);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "Dias para o bloqueio de acesso:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(-118, 323);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(37, 13);
            this.labelControl4.TabIndex = 16;
            this.labelControl4.Text = "Médico:";
            // 
            // FormManutConfiguracaoRefeitorio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(516, 236);
            this.Name = "FormManutConfiguracaoRefeitorio";
            this.Text = "Configuração do Refeitório";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTipoConexao)).EndInit();
            this.gcTipoConexao.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoConexao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPortaSerial)).EndInit();
            this.gcPortaSerial.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgPorta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPortaTCP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCartaoMestre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtQtDias.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gcTipoConexao;
        private DevExpress.XtraEditors.RadioGroup rgTipoConexao;
        private DevExpress.XtraEditors.GroupControl gcPortaSerial;
        private DevExpress.XtraEditors.RadioGroup rgPorta;
        private DevExpress.XtraEditors.SpinEdit txtPortaTCP;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit txtCartaoMestre;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit txtQtDias;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}