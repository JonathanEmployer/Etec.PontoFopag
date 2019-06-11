namespace UI.Relatorios
{
    partial class FormAbsenteismo
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
            this.groupTipo = new DevExpress.XtraEditors.GroupControl();
            this.ckDebitoBH = new DevExpress.XtraEditors.CheckEdit();
            this.ckHorasAbonadas = new DevExpress.XtraEditors.CheckEdit();
            this.ckAtrasos = new DevExpress.XtraEditors.CheckEdit();
            this.ckFaltas = new DevExpress.XtraEditors.CheckEdit();
            this.gcTipoRelatorio = new DevExpress.XtraEditors.GroupControl();
            this.rgTipoRelatorio = new DevExpress.XtraEditors.RadioGroup();
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
            ((System.ComponentModel.ISupportInitialize)(this.groupTipo)).BeginInit();
            this.groupTipo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckDebitoBH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckHorasAbonadas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckAtrasos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckFaltas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTipoRelatorio)).BeginInit();
            this.gcTipoRelatorio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoRelatorio.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl1.TabIndex = 5;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // sbSelecionarEmpresas
            // 
            this.sbSelecionarEmpresas.TabIndex = 6;
            // 
            // sbLimparEmpresa
            // 
            this.sbLimparEmpresa.TabIndex = 7;
            // 
            // rgTipo
            // 
            this.rgTipo.Location = new System.Drawing.Point(2, 21);
            this.rgTipo.Size = new System.Drawing.Size(353, 27);
            this.rgTipo.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 495);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 491);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(678, 491);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(599, 491);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(741, 473);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.gcTipoRelatorio);
            this.tabPage1.Controls.Add(this.groupTipo);
            this.tabPage1.Controls.Add(this.txtPeriodoF);
            this.tabPage1.Controls.Add(this.lblPeriodoF);
            this.tabPage1.Controls.Add(this.txtPeriodoI);
            this.tabPage1.Controls.Add(this.lblPeriodoI);
            this.tabPage1.Size = new System.Drawing.Size(735, 467);
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
            this.tabPage1.Controls.SetChildIndex(this.groupTipo, 0);
            this.tabPage1.Controls.SetChildIndex(this.gcTipoRelatorio, 0);
            // 
            // txtPeriodoF
            // 
            this.txtPeriodoF.EditValue = null;
            this.txtPeriodoF.Location = new System.Drawing.Point(647, 20);
            this.txtPeriodoF.Name = "txtPeriodoF";
            this.txtPeriodoF.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtPeriodoF.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtPeriodoF.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtPeriodoF.Size = new System.Drawing.Size(79, 20);
            this.txtPeriodoF.TabIndex = 4;
            // 
            // lblPeriodoF
            // 
            this.lblPeriodoF.AutoSize = true;
            this.lblPeriodoF.Location = new System.Drawing.Point(568, 23);
            this.lblPeriodoF.Name = "lblPeriodoF";
            this.lblPeriodoF.Size = new System.Drawing.Size(73, 13);
            this.lblPeriodoF.TabIndex = 3;
            this.lblPeriodoF.Text = "Período Final:";
            // 
            // txtPeriodoI
            // 
            this.txtPeriodoI.EditValue = null;
            this.txtPeriodoI.Location = new System.Drawing.Point(464, 20);
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
            this.lblPeriodoI.Location = new System.Drawing.Point(380, 23);
            this.lblPeriodoI.Name = "lblPeriodoI";
            this.lblPeriodoI.Size = new System.Drawing.Size(78, 13);
            this.lblPeriodoI.TabIndex = 1;
            this.lblPeriodoI.Text = "Período Inicial:";
            // 
            // groupTipo
            // 
            this.groupTipo.Controls.Add(this.ckDebitoBH);
            this.groupTipo.Controls.Add(this.ckHorasAbonadas);
            this.groupTipo.Controls.Add(this.ckAtrasos);
            this.groupTipo.Controls.Add(this.ckFaltas);
            this.groupTipo.Location = new System.Drawing.Point(6, 399);
            this.groupTipo.Name = "groupTipo";
            this.groupTipo.Size = new System.Drawing.Size(452, 60);
            this.groupTipo.TabIndex = 15;
            this.groupTipo.Text = "Considerar como Absenteísmo";
            // 
            // ckDebitoBH
            // 
            this.ckDebitoBH.EditValue = true;
            this.ckDebitoBH.Location = new System.Drawing.Point(307, 27);
            this.ckDebitoBH.Name = "ckDebitoBH";
            this.ckDebitoBH.Properties.Caption = "Débito Banco Horas";
            this.ckDebitoBH.Size = new System.Drawing.Size(120, 19);
            this.ckDebitoBH.TabIndex = 3;
            // 
            // ckHorasAbonadas
            // 
            this.ckHorasAbonadas.EditValue = true;
            this.ckHorasAbonadas.Location = new System.Drawing.Point(181, 27);
            this.ckHorasAbonadas.Name = "ckHorasAbonadas";
            this.ckHorasAbonadas.Properties.Caption = "Horas Abonadas";
            this.ckHorasAbonadas.Size = new System.Drawing.Size(120, 19);
            this.ckHorasAbonadas.TabIndex = 2;
            // 
            // ckAtrasos
            // 
            this.ckAtrasos.EditValue = true;
            this.ckAtrasos.Location = new System.Drawing.Point(90, 27);
            this.ckAtrasos.Name = "ckAtrasos";
            this.ckAtrasos.Properties.Caption = "Atrasos";
            this.ckAtrasos.Size = new System.Drawing.Size(75, 19);
            this.ckAtrasos.TabIndex = 1;
            // 
            // ckFaltas
            // 
            this.ckFaltas.EditValue = true;
            this.ckFaltas.Location = new System.Drawing.Point(9, 27);
            this.ckFaltas.Name = "ckFaltas";
            this.ckFaltas.Properties.Caption = "Faltas";
            this.ckFaltas.Size = new System.Drawing.Size(75, 19);
            this.ckFaltas.TabIndex = 0;
            // 
            // gcTipoRelatorio
            // 
            this.gcTipoRelatorio.Controls.Add(this.rgTipoRelatorio);
            this.gcTipoRelatorio.Location = new System.Drawing.Point(464, 399);
            this.gcTipoRelatorio.Name = "gcTipoRelatorio";
            this.gcTipoRelatorio.Size = new System.Drawing.Size(262, 60);
            this.gcTipoRelatorio.TabIndex = 17;
            this.gcTipoRelatorio.Text = "Tipo Relatório";
            // 
            // rgTipoRelatorio
            // 
            this.rgTipoRelatorio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipoRelatorio.EditValue = 1;
            this.rgTipoRelatorio.Location = new System.Drawing.Point(2, 21);
            this.rgTipoRelatorio.Name = "rgTipoRelatorio";
            this.rgTipoRelatorio.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rgTipoRelatorio.Properties.Appearance.Options.UseBackColor = true;
            this.rgTipoRelatorio.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Analítico"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Sintético")});
            this.rgTipoRelatorio.Size = new System.Drawing.Size(258, 37);
            this.rgTipoRelatorio.TabIndex = 0;
            // 
            // FormAbsenteismo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(764, 520);
            this.Name = "FormAbsenteismo";
            this.Text = "Relatório de Absenteísmo";
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
            ((System.ComponentModel.ISupportInitialize)(this.groupTipo)).EndInit();
            this.groupTipo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ckDebitoBH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckHorasAbonadas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckAtrasos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckFaltas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTipoRelatorio)).EndInit();
            this.gcTipoRelatorio.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoRelatorio.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit txtPeriodoF;
        private System.Windows.Forms.Label lblPeriodoF;
        private DevExpress.XtraEditors.DateEdit txtPeriodoI;
        private System.Windows.Forms.Label lblPeriodoI;
        private DevExpress.XtraEditors.GroupControl groupTipo;
        private DevExpress.XtraEditors.CheckEdit ckHorasAbonadas;
        private DevExpress.XtraEditors.CheckEdit ckAtrasos;
        private DevExpress.XtraEditors.CheckEdit ckFaltas;
        private DevExpress.XtraEditors.GroupControl gcTipoRelatorio;
        private DevExpress.XtraEditors.RadioGroup rgTipoRelatorio;
        private DevExpress.XtraEditors.CheckEdit ckDebitoBH;
    }
}
