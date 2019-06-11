namespace UI.Relatorios.Funcionario
{
    partial class FuncionariosPorCodigo
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
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigoFinal = new DevExpress.XtraEditors.SpinEdit();
            this.txtCodigoInicial = new DevExpress.XtraEditors.SpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigoFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigoInicial.Properties)).BeginInit();
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
            this.tabPage1.Controls.Add(this.txtCodigoFinal);
            this.tabPage1.Controls.Add(this.txtCodigoInicial);
            this.tabPage1.Controls.Add(this.labelControl3);
            this.tabPage1.Controls.Add(this.labelControl2);
            this.tabPage1.Size = new System.Drawing.Size(703, 183);
            this.tabPage1.Controls.SetChildIndex(this.labelControl2, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl3, 0);
            this.tabPage1.Controls.SetChildIndex(this.txtCodigoInicial, 0);
            this.tabPage1.Controls.SetChildIndex(this.txtCodigoFinal, 0);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(332, 156);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(65, 13);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Código inicial:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(520, 156);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 13);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "Código final:";
            // 
            // txtCodigoFinal
            // 
            this.txtCodigoFinal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtCodigoFinal.Location = new System.Drawing.Point(586, 153);
            this.txtCodigoFinal.Name = "txtCodigoFinal";
            this.txtCodigoFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtCodigoFinal.Properties.IsFloatValue = false;
            this.txtCodigoFinal.Properties.Mask.EditMask = "N00";
            this.txtCodigoFinal.Properties.MaxLength = 10;
            this.txtCodigoFinal.Properties.MaxValue = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.txtCodigoFinal.Size = new System.Drawing.Size(100, 20);
            this.txtCodigoFinal.TabIndex = 5;
            // 
            // txtCodigoInicial
            // 
            this.txtCodigoInicial.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtCodigoInicial.Location = new System.Drawing.Point(403, 153);
            this.txtCodigoInicial.Name = "txtCodigoInicial";
            this.txtCodigoInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtCodigoInicial.Properties.IsFloatValue = false;
            this.txtCodigoInicial.Properties.Mask.EditMask = "N00";
            this.txtCodigoInicial.Properties.MaxLength = 10;
            this.txtCodigoInicial.Properties.MaxValue = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.txtCodigoInicial.Size = new System.Drawing.Size(100, 20);
            this.txtCodigoInicial.TabIndex = 3;
            // 
            // FuncionariosPorCodigo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(736, 239);
            this.MinimizeBox = false;
            this.Name = "FuncionariosPorCodigo";
            this.ShowInTaskbar = false;
            this.Text = "Relatório de Funcionário por Código";
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigoFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigoInicial.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit txtCodigoFinal;
        private DevExpress.XtraEditors.SpinEdit txtCodigoInicial;
    }
}
