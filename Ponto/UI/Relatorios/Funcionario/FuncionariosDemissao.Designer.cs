namespace UI.Relatorios.Funcionario
{
    partial class FuncionariosDemissao
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
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.DTFinal = new DevExpress.XtraEditors.DateEdit();
            this.DTInicial = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 230);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 226);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(649, 226);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(570, 226);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(712, 208);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelControl3);
            this.tabPage1.Controls.Add(this.DTFinal);
            this.tabPage1.Controls.Add(this.DTInicial);
            this.tabPage1.Controls.Add(this.labelControl2);
            this.tabPage1.Size = new System.Drawing.Size(703, 199);
            this.tabPage1.Controls.SetChildIndex(this.labelControl2, 0);
            this.tabPage1.Controls.SetChildIndex(this.DTInicial, 0);
            this.tabPage1.Controls.SetChildIndex(this.DTFinal, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl3, 0);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(548, 165);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(52, 13);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "Data Final:";
            // 
            // DTFinal
            // 
            this.DTFinal.EditValue = null;
            this.DTFinal.Location = new System.Drawing.Point(606, 162);
            this.DTFinal.Name = "DTFinal";
            this.DTFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DTFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.DTFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DTFinal.Size = new System.Drawing.Size(80, 20);
            this.DTFinal.TabIndex = 8;
            // 
            // DTInicial
            // 
            this.DTInicial.EditValue = null;
            this.DTInicial.Location = new System.Drawing.Point(444, 162);
            this.DTInicial.Name = "DTInicial";
            this.DTInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DTInicial.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.DTInicial.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DTInicial.Size = new System.Drawing.Size(80, 20);
            this.DTInicial.TabIndex = 6;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(381, 165);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(57, 13);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "Data Inicial:";
            // 
            // FuncionariosDemissao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(736, 255);
            this.MinimizeBox = false;
            this.Name = "FuncionariosDemissao";
            this.ShowInTaskbar = false;
            this.Text = "Relatório de Funcionário por Demissão";
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.DateEdit DTFinal;
        private DevExpress.XtraEditors.DateEdit DTInicial;
        private DevExpress.XtraEditors.LabelControl labelControl2;

    }
}
