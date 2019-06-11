namespace UI
{
    partial class FormManutAcerto
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.sbIdIdentificacao = new Componentes.devexpress.cwk_DevButton();
            this.cbIdentificacao = new Componentes.devexpress.cwk_DevLookup();
            this.lblIdentificacao = new DevExpress.XtraEditors.LabelControl();
            this.rgTipo = new DevExpress.XtraEditors.RadioGroup();
            this.lblBancoHoras = new DevExpress.XtraEditors.LabelControl();
            this.cbBancoHoras = new Componentes.devexpress.cwk_DevLookup();
            this.btBancoHoras = new Componentes.devexpress.cwk_DevButton();
            this.lblDataFechamento = new DevExpress.XtraEditors.LabelControl();
            this.data = new DevExpress.XtraEditors.DateEdit();
            this.chbPagamentoHoraAutomatico = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbPagamentoHoraAutomaticoDebito = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdentificacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbBancoHoras.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.data.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.data.Properties)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 207);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.groupBox1);
            this.xtraTabPage1.Controls.Add(this.data);
            this.xtraTabPage1.Controls.Add(this.lblDataFechamento);
            this.xtraTabPage1.Controls.Add(this.btBancoHoras);
            this.xtraTabPage1.Controls.Add(this.cbBancoHoras);
            this.xtraTabPage1.Controls.Add(this.lblBancoHoras);
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Size = new System.Drawing.Size(519, 201);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 225);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 225);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 225);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.sbIdIdentificacao);
            this.groupControl1.Controls.Add(this.cbIdentificacao);
            this.groupControl1.Controls.Add(this.lblIdentificacao);
            this.groupControl1.Controls.Add(this.rgTipo);
            this.groupControl1.Location = new System.Drawing.Point(10, 13);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(497, 76);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Tipo";
            // 
            // sbIdIdentificacao
            // 
            this.sbIdIdentificacao.Location = new System.Drawing.Point(467, 51);
            this.sbIdIdentificacao.Name = "sbIdIdentificacao";
            this.sbIdIdentificacao.Size = new System.Drawing.Size(24, 20);
            this.sbIdIdentificacao.TabIndex = 3;
            this.sbIdIdentificacao.TabStop = false;
            this.sbIdIdentificacao.Text = "...";
            this.sbIdIdentificacao.Click += new System.EventHandler(this.sbIdIdentificacao_Click);
            // 
            // cbIdentificacao
            // 
            this.cbIdentificacao.ButtonLookup = this.sbIdIdentificacao;
            this.cbIdentificacao.EditValue = -1;
            this.cbIdentificacao.Key = System.Windows.Forms.Keys.F5;
            this.cbIdentificacao.Location = new System.Drawing.Point(109, 51);
            this.cbIdentificacao.Name = "cbIdentificacao";
            this.cbIdentificacao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdentificacao.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("empresa", "Empresa", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dscodigo", "dscodigo", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.cbIdentificacao.Properties.NullText = "";
            this.cbIdentificacao.Properties.ValueMember = "id";
            this.cbIdentificacao.Size = new System.Drawing.Size(352, 20);
            this.cbIdentificacao.TabIndex = 2;
            // 
            // lblIdentificacao
            // 
            this.lblIdentificacao.Location = new System.Drawing.Point(37, 54);
            this.lblIdentificacao.Name = "lblIdentificacao";
            this.lblIdentificacao.Size = new System.Drawing.Size(66, 13);
            this.lblIdentificacao.TabIndex = 1;
            this.lblIdentificacao.Text = "Identificação:";
            // 
            // rgTipo
            // 
            this.rgTipo.Dock = System.Windows.Forms.DockStyle.Top;
            this.rgTipo.EditValue = -1;
            this.rgTipo.Location = new System.Drawing.Point(2, 21);
            this.rgTipo.Name = "rgTipo";
            this.rgTipo.Properties.EnableFocusRect = true;
            this.rgTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Empresa"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Departamento"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "Função"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Funcionário")});
            this.rgTipo.Size = new System.Drawing.Size(493, 25);
            this.rgTipo.TabIndex = 0;
            this.rgTipo.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // lblBancoHoras
            // 
            this.lblBancoHoras.Location = new System.Drawing.Point(49, 98);
            this.lblBancoHoras.Name = "lblBancoHoras";
            this.lblBancoHoras.Size = new System.Drawing.Size(64, 13);
            this.lblBancoHoras.TabIndex = 1;
            this.lblBancoHoras.Text = "Banco Horas:";
            // 
            // cbBancoHoras
            // 
            this.cbBancoHoras.ButtonLookup = this.btBancoHoras;
            this.cbBancoHoras.EditValue = -1;
            this.cbBancoHoras.Key = System.Windows.Forms.Keys.F5;
            this.cbBancoHoras.Location = new System.Drawing.Point(119, 95);
            this.cbBancoHoras.Name = "cbBancoHoras";
            this.cbBancoHoras.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbBancoHoras.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("datainicial", "Início ", 20, DevExpress.Utils.FormatType.DateTime, "d", true, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("datafinal", "Final", 20, DevExpress.Utils.FormatType.DateTime, "d", true, DevExpress.Utils.HorzAlignment.Default)});
            this.cbBancoHoras.Properties.DisplayMember = "nome";
            this.cbBancoHoras.Properties.NullText = "";
            this.cbBancoHoras.Properties.ValueMember = "id";
            this.cbBancoHoras.Size = new System.Drawing.Size(352, 20);
            this.cbBancoHoras.TabIndex = 2;
            // 
            // btBancoHoras
            // 
            this.btBancoHoras.Location = new System.Drawing.Point(477, 95);
            this.btBancoHoras.Name = "btBancoHoras";
            this.btBancoHoras.Size = new System.Drawing.Size(24, 20);
            this.btBancoHoras.TabIndex = 3;
            this.btBancoHoras.TabStop = false;
            this.btBancoHoras.Text = "...";
            this.btBancoHoras.Click += new System.EventHandler(this.btBancoHoras_Click);
            // 
            // lblDataFechamento
            // 
            this.lblDataFechamento.Location = new System.Drawing.Point(9, 124);
            this.lblDataFechamento.Name = "lblDataFechamento";
            this.lblDataFechamento.Size = new System.Drawing.Size(104, 13);
            this.lblDataFechamento.TabIndex = 4;
            this.lblDataFechamento.Text = "Data de Fechamento:";
            // 
            // data
            // 
            this.data.EditValue = null;
            this.data.Location = new System.Drawing.Point(119, 121);
            this.data.Name = "data";
            this.data.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.data.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.data.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.data.Size = new System.Drawing.Size(79, 20);
            this.data.TabIndex = 5;
            // 
            // chbPagamentoHoraAutomatico
            // 
            this.chbPagamentoHoraAutomatico.AutoSize = true;
            this.chbPagamentoHoraAutomatico.Location = new System.Drawing.Point(23, 19);
            this.chbPagamentoHoraAutomatico.Name = "chbPagamentoHoraAutomatico";
            this.chbPagamentoHoraAutomatico.Size = new System.Drawing.Size(64, 17);
            this.chbPagamentoHoraAutomatico.TabIndex = 6;
            this.chbPagamentoHoraAutomatico.Text = "Créditos";
            this.chbPagamentoHoraAutomatico.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbPagamentoHoraAutomaticoDebito);
            this.groupBox1.Controls.Add(this.chbPagamentoHoraAutomatico);
            this.groupBox1.Location = new System.Drawing.Point(119, 147);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 42);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Efetuar pagamento automaticamente.";
            // 
            // chbPagamentoHoraAutomaticoDebito
            // 
            this.chbPagamentoHoraAutomaticoDebito.AutoSize = true;
            this.chbPagamentoHoraAutomaticoDebito.Location = new System.Drawing.Point(129, 19);
            this.chbPagamentoHoraAutomaticoDebito.Name = "chbPagamentoHoraAutomaticoDebito";
            this.chbPagamentoHoraAutomaticoDebito.Size = new System.Drawing.Size(62, 17);
            this.chbPagamentoHoraAutomaticoDebito.TabIndex = 7;
            this.chbPagamentoHoraAutomaticoDebito.Text = "Débitos";
            this.chbPagamentoHoraAutomaticoDebito.UseVisualStyleBackColor = true;
            // 
            // FormManutAcerto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 260);
            this.Name = "FormManutAcerto";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdentificacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbBancoHoras.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.data.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.data.Properties)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private Componentes.devexpress.cwk_DevButton sbIdIdentificacao;
        private Componentes.devexpress.cwk_DevLookup cbIdentificacao;
        private DevExpress.XtraEditors.LabelControl lblIdentificacao;
        private DevExpress.XtraEditors.RadioGroup rgTipo;
        private DevExpress.XtraEditors.LabelControl lblDataFechamento;
        private Componentes.devexpress.cwk_DevButton btBancoHoras;
        private Componentes.devexpress.cwk_DevLookup cbBancoHoras;
        private DevExpress.XtraEditors.LabelControl lblBancoHoras;
        private DevExpress.XtraEditors.DateEdit data;
        private System.Windows.Forms.CheckBox chbPagamentoHoraAutomatico;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chbPagamentoHoraAutomaticoDebito;
    }
}
