namespace REL
{
    partial class FuncionariosPorNome
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
            this.lblLetraInicial = new DevExpress.XtraEditors.LabelControl();
            this.lblLetraFinal = new DevExpress.XtraEditors.LabelControl();
            this.txtLetraInicial = new DevExpress.XtraEditors.TextEdit();
            this.txtLetraFinal = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetraInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetraFinal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 214);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 210);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(649, 210);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(570, 210);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(712, 192);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtLetraInicial);
            this.tabPage1.Controls.Add(this.lblLetraInicial);
            this.tabPage1.Controls.Add(this.txtLetraFinal);
            this.tabPage1.Controls.Add(this.lblLetraFinal);
            this.tabPage1.Size = new System.Drawing.Size(703, 183);
            this.tabPage1.Controls.SetChildIndex(this.lblLetraFinal, 0);
            this.tabPage1.Controls.SetChildIndex(this.txtLetraFinal, 0);
            this.tabPage1.Controls.SetChildIndex(this.lblLetraInicial, 0);
            this.tabPage1.Controls.SetChildIndex(this.txtLetraInicial, 0);
            // 
            // lblLetraInicial
            // 
            this.lblLetraInicial.Location = new System.Drawing.Point(338, 156);
            this.lblLetraInicial.Name = "lblLetraInicial";
            this.lblLetraInicial.Size = new System.Drawing.Size(59, 13);
            this.lblLetraInicial.TabIndex = 2;
            this.lblLetraInicial.Text = "Letra Inicial:";
            // 
            // lblLetraFinal
            // 
            this.lblLetraFinal.Location = new System.Drawing.Point(526, 156);
            this.lblLetraFinal.Name = "lblLetraFinal";
            this.lblLetraFinal.Size = new System.Drawing.Size(54, 13);
            this.lblLetraFinal.TabIndex = 3;
            this.lblLetraFinal.Text = "Letra Final:";
            // 
            // txtLetraInicial
            // 
            this.txtLetraInicial.Location = new System.Drawing.Point(403, 153);
            this.txtLetraInicial.Name = "txtLetraInicial";
            this.txtLetraInicial.Size = new System.Drawing.Size(100, 20);
            this.txtLetraInicial.TabIndex = 4;
            // 
            // txtLetraFinal
            // 
            this.txtLetraFinal.Location = new System.Drawing.Point(586, 153);
            this.txtLetraFinal.Name = "txtLetraFinal";
            this.txtLetraFinal.Size = new System.Drawing.Size(100, 20);
            this.txtLetraFinal.TabIndex = 5;
            // 
            // FuncionariosPorNome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(736, 239);
            this.Name = "FuncionariosPorNome";
            this.Text = "Relatório de Funcionários por Nome";
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetraInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetraFinal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblLetraFinal;
        private DevExpress.XtraEditors.TextEdit txtLetraInicial;
        private DevExpress.XtraEditors.LabelControl lblLetraInicial;
        private DevExpress.XtraEditors.TextEdit txtLetraFinal;

    }
}
