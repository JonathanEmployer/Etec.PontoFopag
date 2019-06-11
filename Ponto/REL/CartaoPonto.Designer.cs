namespace REL
{
    partial class CartaoPonto
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
            this.txtPeriodoF = new DevExpress.XtraEditors.DateEdit();
            this.lblPeriodoF = new System.Windows.Forms.Label();
            this.txtPeriodoI = new DevExpress.XtraEditors.DateEdit();
            this.lblPeriodoI = new System.Windows.Forms.Label();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.rgTipoTurno = new DevExpress.XtraEditors.RadioGroup();
            this.pbRelatorio = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoF.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoF.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoTurno.Properties)).BeginInit();
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
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 489);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 485);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(678, 485);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(599, 485);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(741, 467);
            this.TabControl1.Text = "Relatório de Cartão Ponto";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupControl2);
            this.tabPage1.Controls.Add(this.txtPeriodoF);
            this.tabPage1.Controls.Add(this.lblPeriodoF);
            this.tabPage1.Controls.Add(this.txtPeriodoI);
            this.tabPage1.Controls.Add(this.lblPeriodoI);
            this.tabPage1.Size = new System.Drawing.Size(732, 458);
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
            this.tabPage1.Controls.SetChildIndex(this.lblPeriodoF, 0);
            this.tabPage1.Controls.SetChildIndex(this.txtPeriodoF, 0);
            this.tabPage1.Controls.SetChildIndex(this.groupControl2, 0);
            // 
            // txtPeriodoF
            // 
            this.txtPeriodoF.EditValue = null;
            this.txtPeriodoF.Location = new System.Drawing.Point(647, 20);
            this.txtPeriodoF.Name = "txtPeriodoF";
            this.txtPeriodoF.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtPeriodoF.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPeriodoF.Size = new System.Drawing.Size(79, 20);
            this.txtPeriodoF.TabIndex = 2;
            // 
            // lblPeriodoF
            // 
            this.lblPeriodoF.AutoSize = true;
            this.lblPeriodoF.Location = new System.Drawing.Point(568, 23);
            this.lblPeriodoF.Name = "lblPeriodoF";
            this.lblPeriodoF.Size = new System.Drawing.Size(73, 13);
            this.lblPeriodoF.TabIndex = 23;
            this.lblPeriodoF.Text = "Período Final:";
            // 
            // txtPeriodoI
            // 
            this.txtPeriodoI.EditValue = null;
            this.txtPeriodoI.Location = new System.Drawing.Point(464, 20);
            this.txtPeriodoI.Name = "txtPeriodoI";
            this.txtPeriodoI.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtPeriodoI.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPeriodoI.Size = new System.Drawing.Size(79, 20);
            this.txtPeriodoI.TabIndex = 1;
            // 
            // lblPeriodoI
            // 
            this.lblPeriodoI.AutoSize = true;
            this.lblPeriodoI.Location = new System.Drawing.Point(380, 23);
            this.lblPeriodoI.Name = "lblPeriodoI";
            this.lblPeriodoI.Size = new System.Drawing.Size(78, 13);
            this.lblPeriodoI.TabIndex = 21;
            this.lblPeriodoI.Text = "Período Inicial:";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.rgTipoTurno);
            this.groupControl2.Location = new System.Drawing.Point(8, 399);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(353, 56);
            this.groupControl2.TabIndex = 15;
            this.groupControl2.Text = "Tipo Turno";
            // 
            // rgTipoTurno
            // 
            this.rgTipoTurno.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipoTurno.Location = new System.Drawing.Point(2, 20);
            this.rgTipoTurno.Name = "rgTipoTurno";
            this.rgTipoTurno.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Todos"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Normal"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Flexível")});
            this.rgTipoTurno.Size = new System.Drawing.Size(349, 34);
            this.rgTipoTurno.TabIndex = 0;
            // 
            // pbRelatorio
            // 
            this.pbRelatorio.Location = new System.Drawing.Point(195, 485);
            this.pbRelatorio.Name = "pbRelatorio";
            this.pbRelatorio.Size = new System.Drawing.Size(392, 23);
            this.pbRelatorio.TabIndex = 5;
            // 
            // CartaoPonto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(764, 514);
            this.Controls.Add(this.pbRelatorio);
            this.Name = "CartaoPonto";
            this.Text = "Cartão Ponto Individual";
            this.Controls.SetChildIndex(this.pbRelatorio, 0);
            this.Controls.SetChildIndex(this.TabControl1, 0);
            this.Controls.SetChildIndex(this.btCancelar, 0);
            this.Controls.SetChildIndex(this.btOk, 0);
            this.Controls.SetChildIndex(this.simpleButton2, 0);
            this.Controls.SetChildIndex(this.chbSalvarFiltro, 0);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoF.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoF.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPeriodoI.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoTurno.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit txtPeriodoF;
        private System.Windows.Forms.Label lblPeriodoF;
        private DevExpress.XtraEditors.DateEdit txtPeriodoI;
        private System.Windows.Forms.Label lblPeriodoI;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.RadioGroup rgTipoTurno;
        private System.Windows.Forms.ProgressBar pbRelatorio;
    }
}
