namespace UI
{
	partial class FormRecalculaMarcacao
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecalculaMarcacao));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.btIdentificacao = new Componentes.devexpress.cwk_DevButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.rgTipo = new DevExpress.XtraEditors.RadioGroup();
            this.cbIdentificacao = new Componentes.devexpress.cwk_DevLookup();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.dateFinal = new DevExpress.XtraEditors.DateEdit();
            this.dateInicial = new DevExpress.XtraEditors.DateEdit();
            this.lbDtFinal = new System.Windows.Forms.Label();
            this.lbDtInicial = new System.Windows.Forms.Label();
            this.lbIdent = new System.Windows.Forms.Label();
            this.btCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btRecalcula = new DevExpress.XtraEditors.SimpleButton();
            this.btAjuda = new DevExpress.XtraEditors.SimpleButton();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdentificacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Location = new System.Drawing.Point(12, 12);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControl1.Size = new System.Drawing.Size(458, 190);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            this.xtraTabControl1.Text = "xtraTabControl1";
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.btIdentificacao);
            this.xtraTabPage1.Controls.Add(this.groupControl2);
            this.xtraTabPage1.Controls.Add(this.cbIdentificacao);
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Controls.Add(this.lbIdent);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(449, 181);
            this.xtraTabPage1.Text = "xtraTabPage1";
            // 
            // btIdentificacao
            // 
            this.btIdentificacao.Location = new System.Drawing.Point(422, 145);
            this.btIdentificacao.Name = "btIdentificacao";
            this.btIdentificacao.Size = new System.Drawing.Size(24, 20);
            this.btIdentificacao.TabIndex = 4;
            this.btIdentificacao.TabStop = false;
            this.btIdentificacao.Text = "...";
            this.btIdentificacao.Click += new System.EventHandler(this.btIdentificacao_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.rgTipo);
            this.groupControl2.Location = new System.Drawing.Point(3, 79);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(443, 55);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "Escolha o tipo de Recalculo";
            // 
            // rgTipo
            // 
            this.rgTipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipo.EditValue = -1;
            this.rgTipo.Location = new System.Drawing.Point(2, 20);
            this.rgTipo.Name = "rgTipo";
            this.rgTipo.Properties.EnableFocusRect = true;
            this.rgTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Empresa"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Departamento"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "Função"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Funcionário")});
            this.rgTipo.Size = new System.Drawing.Size(439, 33);
            this.rgTipo.TabIndex = 0;
            this.rgTipo.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // cbIdentificacao
            // 
            this.cbIdentificacao.ButtonLookup = this.btIdentificacao;
            this.cbIdentificacao.EditValue = 0;
            this.cbIdentificacao.Key = System.Windows.Forms.Keys.F5;
            this.cbIdentificacao.Location = new System.Drawing.Point(73, 145);
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
            this.cbIdentificacao.Size = new System.Drawing.Size(343, 20);
            this.cbIdentificacao.TabIndex = 3;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.dateFinal);
            this.groupControl1.Controls.Add(this.dateInicial);
            this.groupControl1.Controls.Add(this.lbDtFinal);
            this.groupControl1.Controls.Add(this.lbDtInicial);
            this.groupControl1.Location = new System.Drawing.Point(3, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(443, 63);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Escolha o Período";
            // 
            // dateFinal
            // 
            this.dateFinal.EditValue = null;
            this.dateFinal.Location = new System.Drawing.Point(295, 30);
            this.dateFinal.Name = "dateFinal";
            this.dateFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dateFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFinal.Size = new System.Drawing.Size(100, 20);
            this.dateFinal.TabIndex = 3;
            // 
            // dateInicial
            // 
            this.dateInicial.EditValue = null;
            this.dateInicial.Location = new System.Drawing.Point(84, 30);
            this.dateInicial.Name = "dateInicial";
            this.dateInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateInicial.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dateInicial.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateInicial.Size = new System.Drawing.Size(100, 20);
            this.dateInicial.TabIndex = 1;
            // 
            // lbDtFinal
            // 
            this.lbDtFinal.AutoSize = true;
            this.lbDtFinal.Location = new System.Drawing.Point(211, 33);
            this.lbDtFinal.Name = "lbDtFinal";
            this.lbDtFinal.Size = new System.Drawing.Size(58, 13);
            this.lbDtFinal.TabIndex = 2;
            this.lbDtFinal.Text = "Data Final:";
            // 
            // lbDtInicial
            // 
            this.lbDtInicial.AutoSize = true;
            this.lbDtInicial.Location = new System.Drawing.Point(15, 33);
            this.lbDtInicial.Name = "lbDtInicial";
            this.lbDtInicial.Size = new System.Drawing.Size(63, 13);
            this.lbDtInicial.TabIndex = 0;
            this.lbDtInicial.Text = "Data Inicial:";
            // 
            // lbIdent
            // 
            this.lbIdent.AutoSize = true;
            this.lbIdent.Location = new System.Drawing.Point(0, 148);
            this.lbIdent.Name = "lbIdent";
            this.lbIdent.Size = new System.Drawing.Size(71, 13);
            this.lbIdent.TabIndex = 2;
            this.lbIdent.Text = "Identificação:";
            // 
            // btCancel
            // 
            this.btCancel.Image = ((System.Drawing.Image)(resources.GetObject("btCancel.Image")));
            this.btCancel.Location = new System.Drawing.Point(391, 208);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Cancelar";
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btRecalcula
            // 
            this.btRecalcula.Image = ((System.Drawing.Image)(resources.GetObject("btRecalcula.Image")));
            this.btRecalcula.Location = new System.Drawing.Point(300, 208);
            this.btRecalcula.Name = "btRecalcula";
            this.btRecalcula.Size = new System.Drawing.Size(85, 23);
            this.btRecalcula.TabIndex = 1;
            this.btRecalcula.Text = "Recalcular";
            this.btRecalcula.Click += new System.EventHandler(this.btRecalcula_Click);
            // 
            // btAjuda
            // 
            this.btAjuda.Image = ((System.Drawing.Image)(resources.GetObject("btAjuda.Image")));
            this.btAjuda.Location = new System.Drawing.Point(15, 208);
            this.btAjuda.Name = "btAjuda";
            this.btAjuda.Size = new System.Drawing.Size(75, 23);
            this.btAjuda.TabIndex = 3;
            this.btAjuda.Text = "Ajuda";
            this.btAjuda.Click += new System.EventHandler(this.btAjuda_Click);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // FormRecalculaMarcacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 241);
            this.Controls.Add(this.btAjuda);
            this.Controls.Add(this.btRecalcula);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.xtraTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormRecalculaMarcacao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recalcula Marcações";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormRecalculaMarcacao_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormRecalculaMarcacao_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdentificacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.RadioGroup rgTipo;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Label lbDtFinal;
        private System.Windows.Forms.Label lbDtInicial;
        private DevExpress.XtraEditors.DateEdit dateInicial;
        private DevExpress.XtraEditors.DateEdit dateFinal;
        private DevExpress.XtraEditors.SimpleButton btCancel;
        private DevExpress.XtraEditors.SimpleButton btRecalcula;
        private DevExpress.XtraEditors.SimpleButton btAjuda;
        private System.Windows.Forms.Label lbIdent;
        private Componentes.devexpress.cwk_DevLookup cbIdentificacao;
        private Componentes.devexpress.cwk_DevButton btIdentificacao;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
	}
}