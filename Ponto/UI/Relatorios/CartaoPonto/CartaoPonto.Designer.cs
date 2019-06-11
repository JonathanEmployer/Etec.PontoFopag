namespace UI.Relatorios.CartaoPonto
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
            this.chkAgruparPorDepartamento = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.rgOrientacao = new DevExpress.XtraEditors.RadioGroup();
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
            ((System.ComponentModel.ISupportInitialize)(this.chkAgruparPorDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgOrientacao.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
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
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupControl3);
            this.tabPage1.Controls.Add(this.groupControl2);
            this.tabPage1.Controls.Add(this.txtPeriodoF);
            this.tabPage1.Controls.Add(this.lblPeriodoF);
            this.tabPage1.Controls.Add(this.txtPeriodoI);
            this.tabPage1.Controls.Add(this.lblPeriodoI);
            this.tabPage1.Size = new System.Drawing.Size(735, 461);
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
            this.tabPage1.Controls.SetChildIndex(this.groupControl3, 0);
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
            this.txtPeriodoI.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
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
            this.groupControl2.Location = new System.Drawing.Point(6, 399);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(353, 56);
            this.groupControl2.TabIndex = 15;
            this.groupControl2.Text = "Tipo Turno";
            // 
            // rgTipoTurno
            // 
            this.rgTipoTurno.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipoTurno.Location = new System.Drawing.Point(2, 21);
            this.rgTipoTurno.Name = "rgTipoTurno";
            this.rgTipoTurno.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Todos"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Normal"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Flexível")});
            this.rgTipoTurno.Size = new System.Drawing.Size(349, 33);
            this.rgTipoTurno.TabIndex = 0;
            // 
            // chkAgruparPorDepartamento
            // 
            this.chkAgruparPorDepartamento.Location = new System.Drawing.Point(201, 489);
            this.chkAgruparPorDepartamento.Name = "chkAgruparPorDepartamento";
            this.chkAgruparPorDepartamento.Properties.Caption = "Ordenar por Departamento";
            this.chkAgruparPorDepartamento.Size = new System.Drawing.Size(160, 19);
            this.chkAgruparPorDepartamento.TabIndex = 5;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.rgOrientacao);
            this.groupControl3.Location = new System.Drawing.Point(374, 399);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(353, 56);
            this.groupControl3.TabIndex = 16;
            this.groupControl3.Text = "Orientação";
            // 
            // rgOrientacao
            // 
            this.rgOrientacao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgOrientacao.EditValue = 0;
            this.rgOrientacao.Location = new System.Drawing.Point(2, 21);
            this.rgOrientacao.Name = "rgOrientacao";
            this.rgOrientacao.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Retrato"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Paisagem")});
            this.rgOrientacao.Size = new System.Drawing.Size(349, 33);
            this.rgOrientacao.TabIndex = 0;
            // 
            // CartaoPonto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(764, 514);
            this.Controls.Add(this.chkAgruparPorDepartamento);
            this.MinimizeBox = false;
            this.Name = "CartaoPonto";
            this.ShowInTaskbar = false;
            this.Text = "Relatório de Cartão Ponto";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CartaoPonto_KeyDown);
            this.Controls.SetChildIndex(this.TabControl1, 0);
            this.Controls.SetChildIndex(this.btCancelar, 0);
            this.Controls.SetChildIndex(this.btOk, 0);
            this.Controls.SetChildIndex(this.simpleButton2, 0);
            this.Controls.SetChildIndex(this.chbSalvarFiltro, 0);
            this.Controls.SetChildIndex(this.chkAgruparPorDepartamento, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.chkAgruparPorDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgOrientacao.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.DateEdit txtPeriodoF;
        private System.Windows.Forms.Label lblPeriodoF;
        private DevExpress.XtraEditors.DateEdit txtPeriodoI;
        private System.Windows.Forms.Label lblPeriodoI;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.RadioGroup rgTipoTurno;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.RadioGroup rgOrientacao;
        private DevExpress.XtraEditors.CheckEdit chkAgruparPorDepartamento;
    }
}
