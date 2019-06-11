namespace UI
{
    partial class FormManutInclusaoBanco
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
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.txtData = new DevExpress.XtraEditors.DateEdit();
            this.lblData = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.rgTipocreditodebito = new DevExpress.XtraEditors.RadioGroup();
            this.lblCredito = new DevExpress.XtraEditors.LabelControl();
            this.lblDebito = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.sbIdentificacao = new Componentes.devexpress.cwk_DevButton();
            this.cbIdentificacao = new Componentes.devexpress.cwk_DevLookup();
            this.lblIdentificacao = new DevExpress.XtraEditors.LabelControl();
            this.rgTipo = new DevExpress.XtraEditors.RadioGroup();
            this.txtCredito = new Componentes.devexpress.cwkEditHora();
            this.txtDebito = new Componentes.devexpress.cwkEditHora();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipocreditodebito.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdentificacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCredito.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebito.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 188);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtDebito);
            this.xtraTabPage1.Controls.Add(this.txtCredito);
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Controls.Add(this.lblDebito);
            this.xtraTabPage1.Controls.Add(this.lblCredito);
            this.xtraTabPage1.Controls.Add(this.groupControl2);
            this.xtraTabPage1.Controls.Add(this.txtData);
            this.xtraTabPage1.Controls.Add(this.lblData);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(516, 179);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 206);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 206);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 206);
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(58, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Properties.ReadOnly = true;
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(15, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // txtData
            // 
            this.txtData.EditValue = null;
            this.txtData.Location = new System.Drawing.Point(94, 135);
            this.txtData.Name = "txtData";
            this.txtData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtData.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtData.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtData.Size = new System.Drawing.Size(79, 20);
            this.txtData.TabIndex = 4;
            // 
            // lblData
            // 
            this.lblData.Location = new System.Drawing.Point(18, 138);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(70, 13);
            this.lblData.TabIndex = 3;
            this.lblData.Text = "Data Inclusão:";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.rgTipocreditodebito);
            this.groupControl2.Location = new System.Drawing.Point(218, 121);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(163, 45);
            this.groupControl2.TabIndex = 5;
            this.groupControl2.Text = "Tipo Lançamento";
            // 
            // rgTipocreditodebito
            // 
            this.rgTipocreditodebito.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipocreditodebito.Location = new System.Drawing.Point(2, 20);
            this.rgTipocreditodebito.Name = "rgTipocreditodebito";
            this.rgTipocreditodebito.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Crédito"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Débito")});
            this.rgTipocreditodebito.Size = new System.Drawing.Size(159, 23);
            this.rgTipocreditodebito.TabIndex = 0;
            this.rgTipocreditodebito.SelectedIndexChanged += new System.EventHandler(this.rgTipocreditodebito_SelectedIndexChanged);
            // 
            // lblCredito
            // 
            this.lblCredito.Location = new System.Drawing.Point(418, 124);
            this.lblCredito.Name = "lblCredito";
            this.lblCredito.Size = new System.Drawing.Size(39, 13);
            this.lblCredito.TabIndex = 6;
            this.lblCredito.Text = "Crédito:";
            // 
            // lblDebito
            // 
            this.lblDebito.Location = new System.Drawing.Point(422, 150);
            this.lblDebito.Name = "lblDebito";
            this.lblDebito.Size = new System.Drawing.Size(35, 13);
            this.lblDebito.TabIndex = 8;
            this.lblDebito.Text = "Débito:";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.sbIdentificacao);
            this.groupControl1.Controls.Add(this.cbIdentificacao);
            this.groupControl1.Controls.Add(this.lblIdentificacao);
            this.groupControl1.Controls.Add(this.rgTipo);
            this.groupControl1.Location = new System.Drawing.Point(12, 39);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(495, 76);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "Tipo";
            // 
            // sbIdentificacao
            // 
            this.sbIdentificacao.Location = new System.Drawing.Point(469, 51);
            this.sbIdentificacao.Name = "sbIdentificacao";
            this.sbIdentificacao.Size = new System.Drawing.Size(24, 20);
            this.sbIdentificacao.TabIndex = 3;
            this.sbIdentificacao.TabStop = false;
            this.sbIdentificacao.Text = "...";
            this.sbIdentificacao.Click += new System.EventHandler(this.sbIdentificacao_Click);
            // 
            // cbIdentificacao
            // 
            this.cbIdentificacao.ButtonLookup = this.sbIdentificacao;
            this.cbIdentificacao.EditValue = 0;
            this.cbIdentificacao.Key = System.Windows.Forms.Keys.F5;
            this.cbIdentificacao.Location = new System.Drawing.Point(84, 51);
            this.cbIdentificacao.Name = "cbIdentificacao";
            this.cbIdentificacao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdentificacao.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("empresa", "Empresa", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdentificacao.Properties.NullText = "";
            this.cbIdentificacao.Properties.ValueMember = "id";
            this.cbIdentificacao.Size = new System.Drawing.Size(379, 20);
            this.cbIdentificacao.TabIndex = 2;
            // 
            // lblIdentificacao
            // 
            this.lblIdentificacao.Location = new System.Drawing.Point(12, 54);
            this.lblIdentificacao.Name = "lblIdentificacao";
            this.lblIdentificacao.Size = new System.Drawing.Size(66, 13);
            this.lblIdentificacao.TabIndex = 1;
            this.lblIdentificacao.Text = "Identificação:";
            // 
            // rgTipo
            // 
            this.rgTipo.Dock = System.Windows.Forms.DockStyle.Top;
            this.rgTipo.Location = new System.Drawing.Point(2, 20);
            this.rgTipo.Name = "rgTipo";
            this.rgTipo.Properties.EnableFocusRect = true;
            this.rgTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Empresa"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Departamento"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "Função"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Funcionário")});
            this.rgTipo.Size = new System.Drawing.Size(491, 25);
            this.rgTipo.TabIndex = 0;
            this.rgTipo.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // txtCredito
            // 
            this.txtCredito.cwErro = false;
            this.txtCredito.EditValue = "---:--";
            this.txtCredito.Layout = Componentes.devexpress.LayoutsHorario.horario3Digitos;
            this.txtCredito.lblLegenda = null;
            this.txtCredito.lblNRelogio = null;
            this.txtCredito.Location = new System.Drawing.Point(463, 121);
            this.txtCredito.Name = "txtCredito";
            this.txtCredito.Properties.Appearance.Options.UseTextOptions = true;
            this.txtCredito.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtCredito.Properties.MaxLength = 6;
            this.txtCredito.Size = new System.Drawing.Size(42, 20);
            this.txtCredito.TabIndex = 9;
            this.txtCredito.ValorAnterior = null;
            // 
            // txtDebito
            // 
            this.txtDebito.cwErro = false;
            this.txtDebito.EditValue = "---:--";
            this.txtDebito.Layout = Componentes.devexpress.LayoutsHorario.horario3Digitos;
            this.txtDebito.lblLegenda = null;
            this.txtDebito.lblNRelogio = null;
            this.txtDebito.Location = new System.Drawing.Point(463, 147);
            this.txtDebito.Name = "txtDebito";
            this.txtDebito.Properties.Appearance.Options.UseTextOptions = true;
            this.txtDebito.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtDebito.Properties.MaxLength = 6;
            this.txtDebito.Size = new System.Drawing.Size(42, 20);
            this.txtDebito.TabIndex = 10;
            this.txtDebito.ValorAnterior = null;
            // 
            // FormManutInclusaoBanco
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 241);
            this.Name = "FormManutInclusaoBanco";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipocreditodebito.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdentificacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCredito.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebito.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.DateEdit txtData;
        private DevExpress.XtraEditors.LabelControl lblData;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.RadioGroup rgTipocreditodebito;
        private DevExpress.XtraEditors.LabelControl lblDebito;
        private DevExpress.XtraEditors.LabelControl lblCredito;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private Componentes.devexpress.cwk_DevButton sbIdentificacao;
        private Componentes.devexpress.cwk_DevLookup cbIdentificacao;
        private DevExpress.XtraEditors.LabelControl lblIdentificacao;
        private DevExpress.XtraEditors.RadioGroup rgTipo;
        private Componentes.devexpress.cwkEditHora txtDebito;
        private Componentes.devexpress.cwkEditHora txtCredito;
    }
}
