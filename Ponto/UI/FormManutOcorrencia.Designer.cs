namespace UI
{
    partial class FormManutOcorrencia
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
            this.chbAbsenteismo = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAbsenteismo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 78);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.chbAbsenteismo);
            this.xtraTabPage1.Controls.Add(this.txtDescricao);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblDescricao);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(519, 72);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 96);
            this.sbCancelar.TabIndex = 6;
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 96);
            this.sbGravar.TabIndex = 5;
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 96);
            this.sbAjuda.TabIndex = 4;
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(66, 39);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Size = new System.Drawing.Size(441, 20);
            this.txtDescricao.TabIndex = 3;
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(66, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, false)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Properties.ValidateOnEnterKey = true;
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // lblDescricao
            // 
            this.lblDescricao.Location = new System.Drawing.Point(10, 42);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(50, 13);
            this.lblDescricao.TabIndex = 3;
            this.lblDescricao.Text = "Descrição:";
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(23, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // chbAbsenteismo
            // 
            this.chbAbsenteismo.Location = new System.Drawing.Point(152, 13);
            this.chbAbsenteismo.Name = "chbAbsenteismo";
            this.chbAbsenteismo.Properties.Caption = "Considerar no relatório de absenteísmo";
            this.chbAbsenteismo.Size = new System.Drawing.Size(212, 19);
            this.chbAbsenteismo.TabIndex = 2;
            // 
            // FormManutOcorrencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 131);
            this.Name = "FormManutOcorrencia";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAbsenteismo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtDescricao;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.LabelControl lblDescricao;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.CheckEdit chbAbsenteismo;
    }
}
