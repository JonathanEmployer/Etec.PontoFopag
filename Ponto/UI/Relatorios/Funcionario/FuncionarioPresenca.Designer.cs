namespace UI.Relatorios.Funcionario
{
    partial class FuncionarioPresenca
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
            this.txtPeriodoI = new DevExpress.XtraEditors.DateEdit();
            this.lblPeriodoI = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.Options.UseFont = true;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            // 
            // rgTipo
            // 
            this.rgTipo.EditValue = 0;
            this.rgTipo.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 444);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 440);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(678, 440);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(599, 440);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(741, 422);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtPeriodoI);
            this.tabPage1.Controls.Add(this.lblPeriodoI);
            this.tabPage1.Size = new System.Drawing.Size(732, 413);
            this.tabPage1.Controls.SetChildIndex(this.labelControl1, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl2, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl3, 0);
            this.tabPage1.Controls.SetChildIndex(this.groupControl1, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarEmpresas, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparEmpresa, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarFuncionarios, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparFuncionarios, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarDepartamentos, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparDepartamento, 0);
            this.tabPage1.Controls.SetChildIndex(this.lblPeriodoI, 0);
            this.tabPage1.Controls.SetChildIndex(this.txtPeriodoI, 0);
            // 
            // txtPeriodoI
            // 
            this.txtPeriodoI.EditValue = null;
            this.txtPeriodoI.Location = new System.Drawing.Point(527, 20);
            this.txtPeriodoI.Name = "txtPeriodoI";
            this.txtPeriodoI.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtPeriodoI.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtPeriodoI.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPeriodoI.Size = new System.Drawing.Size(79, 20);
            this.txtPeriodoI.TabIndex = 2;
            // 
            // lblPeriodoI
            // 
            this.lblPeriodoI.AutoSize = true;
            this.lblPeriodoI.Location = new System.Drawing.Point(488, 23);
            this.lblPeriodoI.Name = "lblPeriodoI";
            this.lblPeriodoI.Size = new System.Drawing.Size(33, 13);
            this.lblPeriodoI.TabIndex = 1;
            this.lblPeriodoI.Text = "Data:";
            // 
            // FuncionarioPresenca
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(764, 469);
            this.MinimizeBox = false;
            this.Name = "FuncionarioPresenca";
            this.ShowInTaskbar = false;
            this.Text = "Relatório de Funcionário por Presença";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit txtPeriodoI;
        private System.Windows.Forms.Label lblPeriodoI;
    }
}
