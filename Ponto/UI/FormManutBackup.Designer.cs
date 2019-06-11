namespace UI
{
    partial class FormManutBackup
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.txtDescricao = new DevExpress.XtraEditors.TextEdit();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.lblDescricao = new DevExpress.XtraEditors.LabelControl();
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.txtDiretorio = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BotaoDiretorio = new Componentes.devexpress.cwk_DevButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiretorio.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(548, 104);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.BotaoDiretorio);
            this.xtraTabPage1.Controls.Add(this.txtDiretorio);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.txtDescricao);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblDescricao);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(542, 98);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(485, 122);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(404, 122);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 122);
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(60, 37);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(441, 20);
            this.txtDescricao.TabIndex = 7;
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(60, 11);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, false)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Properties.ReadOnly = true;
            this.txtCodigo.Properties.ValidateOnEnterKey = true;
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 5;
            this.txtCodigo.TabStop = false;
            // 
            // lblDescricao
            // 
            this.lblDescricao.Location = new System.Drawing.Point(4, 40);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(50, 13);
            this.lblDescricao.TabIndex = 6;
            this.lblDescricao.Text = "Descrição:";
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(17, 14);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 4;
            this.lblCodigo.Text = "Código:";
            // 
            // txtDiretorio
            // 
            this.txtDiretorio.Location = new System.Drawing.Point(60, 63);
            this.txtDiretorio.Name = "txtDiretorio";
            this.txtDiretorio.Size = new System.Drawing.Size(441, 20);
            this.txtDiretorio.TabIndex = 9;
            this.txtDiretorio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDiretorio_KeyDown);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(9, 66);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(45, 13);
            this.labelControl1.TabIndex = 8;
            this.labelControl1.Text = "Diretório:";
            // 
            // BotaoDiretorio
            // 
            this.BotaoDiretorio.Location = new System.Drawing.Point(507, 63);
            this.BotaoDiretorio.Name = "BotaoDiretorio";
            this.BotaoDiretorio.Size = new System.Drawing.Size(24, 20);
            this.BotaoDiretorio.TabIndex = 10;
            this.BotaoDiretorio.TabStop = false;
            this.BotaoDiretorio.Text = "...";
            this.BotaoDiretorio.Click += new System.EventHandler(this.BotaoDiretorio_Click);
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // FormManutBackup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(572, 157);
            this.Name = "FormManutBackup";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiretorio.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtDiretorio;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtDescricao;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.LabelControl lblDescricao;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private Componentes.devexpress.cwk_DevButton BotaoDiretorio;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;

    }
}
