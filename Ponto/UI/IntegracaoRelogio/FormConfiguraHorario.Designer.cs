namespace UI.IntegracaoRelogio
{
    partial class FormConfiguraHorario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguraHorario));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.chbEnviarDataEHoraAtual = new DevExpress.XtraEditors.CheckEdit();
            this.chbEnviarHorarioDeVerao = new DevExpress.XtraEditors.CheckEdit();
            this.gcDataHoraRelogio = new DevExpress.XtraEditors.GroupControl();
            this.chbDataHoraComputador = new DevExpress.XtraEditors.CheckEdit();
            this.txtDataHoraAtual = new DevExpress.XtraEditors.DateEdit();
            this.gcHorarioVerao = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtInicioHorarioVerao = new DevExpress.XtraEditors.DateEdit();
            this.txtTerminoHorarioVerao = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sbExportar = new DevExpress.XtraEditors.SimpleButton();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.sbAjuda = new DevExpress.XtraEditors.SimpleButton();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.cbIdRep = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdRep = new Componentes.devexpress.cwk_DevButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chbEnviarDataEHoraAtual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbEnviarHorarioDeVerao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDataHoraRelogio)).BeginInit();
            this.gcDataHoraRelogio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chbDataHoraComputador.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataHoraAtual.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataHoraAtual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHorarioVerao)).BeginInit();
            this.gcHorarioVerao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInicioHorarioVerao.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInicioHorarioVerao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerminoHorarioVerao.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerminoHorarioVerao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdRep.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.chbEnviarDataEHoraAtual);
            this.panelControl1.Controls.Add(this.chbEnviarHorarioDeVerao);
            this.panelControl1.Controls.Add(this.gcDataHoraRelogio);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.sbIdRep);
            this.panelControl1.Controls.Add(this.cbIdRep);
            this.panelControl1.Controls.Add(this.gcHorarioVerao);
            this.panelControl1.Location = new System.Drawing.Point(12, 12);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(508, 156);
            this.panelControl1.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(6, 37);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(40, 13);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "Opções:";
            // 
            // chbEnviarDataEHoraAtual
            // 
            this.chbEnviarDataEHoraAtual.Location = new System.Drawing.Point(51, 34);
            this.chbEnviarDataEHoraAtual.Name = "chbEnviarDataEHoraAtual";
            this.chbEnviarDataEHoraAtual.Properties.Caption = "Enviar Data e Hora Atual";
            this.chbEnviarDataEHoraAtual.Size = new System.Drawing.Size(146, 19);
            this.chbEnviarDataEHoraAtual.TabIndex = 4;
            this.chbEnviarDataEHoraAtual.CheckedChanged += new System.EventHandler(this.chbEnviarDataEHoraAtual_CheckedChanged);
            // 
            // chbEnviarHorarioDeVerao
            // 
            this.chbEnviarHorarioDeVerao.Location = new System.Drawing.Point(265, 34);
            this.chbEnviarHorarioDeVerao.Name = "chbEnviarHorarioDeVerao";
            this.chbEnviarHorarioDeVerao.Properties.Caption = "Enviar Horário de Verão";
            this.chbEnviarHorarioDeVerao.Size = new System.Drawing.Size(140, 19);
            this.chbEnviarHorarioDeVerao.TabIndex = 6;
            this.chbEnviarHorarioDeVerao.CheckedChanged += new System.EventHandler(this.chbEnviarHorarioDeVerao_CheckedChanged);
            // 
            // gcDataHoraRelogio
            // 
            this.gcDataHoraRelogio.Controls.Add(this.chbDataHoraComputador);
            this.gcDataHoraRelogio.Controls.Add(this.txtDataHoraAtual);
            this.gcDataHoraRelogio.Location = new System.Drawing.Point(53, 59);
            this.gcDataHoraRelogio.Name = "gcDataHoraRelogio";
            this.gcDataHoraRelogio.Size = new System.Drawing.Size(204, 88);
            this.gcDataHoraRelogio.TabIndex = 5;
            this.gcDataHoraRelogio.Text = "Data e Hora Atual";
            // 
            // chbDataHoraComputador
            // 
            this.chbDataHoraComputador.Location = new System.Drawing.Point(10, 23);
            this.chbDataHoraComputador.Name = "chbDataHoraComputador";
            this.chbDataHoraComputador.Properties.Caption = "Usar Data e Hora do Computador";
            this.chbDataHoraComputador.Size = new System.Drawing.Size(185, 19);
            this.chbDataHoraComputador.TabIndex = 0;
            this.chbDataHoraComputador.CheckedChanged += new System.EventHandler(this.chbDataHoraComputador_CheckedChanged);
            // 
            // txtDataHoraAtual
            // 
            this.txtDataHoraAtual.EditValue = null;
            this.txtDataHoraAtual.Location = new System.Drawing.Point(36, 52);
            this.txtDataHoraAtual.Name = "txtDataHoraAtual";
            this.txtDataHoraAtual.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataHoraAtual.Properties.DisplayFormat.FormatString = "g";
            this.txtDataHoraAtual.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtDataHoraAtual.Properties.Mask.EditMask = "g";
            this.txtDataHoraAtual.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataHoraAtual.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataHoraAtual.Size = new System.Drawing.Size(133, 20);
            this.txtDataHoraAtual.TabIndex = 1;
            // 
            // gcHorarioVerao
            // 
            this.gcHorarioVerao.Controls.Add(this.labelControl2);
            this.gcHorarioVerao.Controls.Add(this.txtInicioHorarioVerao);
            this.gcHorarioVerao.Controls.Add(this.txtTerminoHorarioVerao);
            this.gcHorarioVerao.Controls.Add(this.labelControl1);
            this.gcHorarioVerao.Location = new System.Drawing.Point(267, 59);
            this.gcHorarioVerao.Name = "gcHorarioVerao";
            this.gcHorarioVerao.Size = new System.Drawing.Size(204, 88);
            this.gcHorarioVerao.TabIndex = 7;
            this.gcHorarioVerao.Text = "Horário de Verão";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(5, 55);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(42, 13);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Término:";
            // 
            // txtInicioHorarioVerao
            // 
            this.txtInicioHorarioVerao.EditValue = null;
            this.txtInicioHorarioVerao.Location = new System.Drawing.Point(53, 26);
            this.txtInicioHorarioVerao.Name = "txtInicioHorarioVerao";
            this.txtInicioHorarioVerao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtInicioHorarioVerao.Properties.DisplayFormat.FormatString = "g";
            this.txtInicioHorarioVerao.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtInicioHorarioVerao.Properties.Mask.EditMask = "g";
            this.txtInicioHorarioVerao.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtInicioHorarioVerao.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtInicioHorarioVerao.Size = new System.Drawing.Size(133, 20);
            this.txtInicioHorarioVerao.TabIndex = 1;
            // 
            // txtTerminoHorarioVerao
            // 
            this.txtTerminoHorarioVerao.EditValue = null;
            this.txtTerminoHorarioVerao.Location = new System.Drawing.Point(53, 52);
            this.txtTerminoHorarioVerao.Name = "txtTerminoHorarioVerao";
            this.txtTerminoHorarioVerao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtTerminoHorarioVerao.Properties.DisplayFormat.FormatString = "g";
            this.txtTerminoHorarioVerao.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.txtTerminoHorarioVerao.Properties.Mask.EditMask = "g";
            this.txtTerminoHorarioVerao.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtTerminoHorarioVerao.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtTerminoHorarioVerao.Size = new System.Drawing.Size(133, 20);
            this.txtTerminoHorarioVerao.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(18, 29);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(29, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Início:";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Importação de Bilhetes copy.ico");
            this.imageList1.Images.SetKeyName(1, "cancelar copy.ico");
            this.imageList1.Images.SetKeyName(2, "Help copy.ico");
            this.imageList1.Images.SetKeyName(3, "Selecionar_timao.ico");
            // 
            // sbExportar
            // 
            this.sbExportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbExportar.Image = ((System.Drawing.Image)(resources.GetObject("sbExportar.Image")));
            this.sbExportar.ImageIndex = 0;
            this.sbExportar.Location = new System.Drawing.Point(364, 174);
            this.sbExportar.Name = "sbExportar";
            this.sbExportar.Size = new System.Drawing.Size(75, 23);
            this.sbExportar.TabIndex = 1;
            this.sbExportar.Text = "&Enviar";
            this.sbExportar.Click += new System.EventHandler(this.sbExportar_Click);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sbCancelar.ImageIndex = 1;
            this.sbCancelar.ImageList = this.imageList1;
            this.sbCancelar.Location = new System.Drawing.Point(445, 174);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(75, 23);
            this.sbCancelar.TabIndex = 2;
            this.sbCancelar.Text = "&Cancelar";
            this.sbCancelar.Click += new System.EventHandler(this.sbCancelar_Click);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sbAjuda.ImageIndex = 2;
            this.sbAjuda.ImageList = this.imageList1;
            this.sbAjuda.Location = new System.Drawing.Point(12, 174);
            this.sbAjuda.Name = "sbAjuda";
            this.sbAjuda.Size = new System.Drawing.Size(75, 23);
            this.sbAjuda.TabIndex = 3;
            this.sbAjuda.Text = "A&juda";
            this.sbAjuda.Click += new System.EventHandler(this.sbAjuda_Click);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // cbIdRep
            // 
            this.cbIdRep.ButtonLookup = this.sbIdRep;
            this.cbIdRep.EditValue = 0;
            this.cbIdRep.Key = System.Windows.Forms.Keys.F5;
            this.cbIdRep.Location = new System.Drawing.Point(53, 8);
            this.cbIdRep.Name = "cbIdRep";
            this.cbIdRep.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdRep.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("local", "Local", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdRep.Properties.DisplayMember = "local";
            this.cbIdRep.Properties.NullText = "";
            this.cbIdRep.Properties.ValueMember = "id";
            this.cbIdRep.Size = new System.Drawing.Size(418, 20);
            this.cbIdRep.TabIndex = 1;
            this.cbIdRep.EditValueChanged += new System.EventHandler(this.cbIdRep_EditValueChanged);
            // 
            // sbIdRep
            // 
            this.sbIdRep.Location = new System.Drawing.Point(477, 8);
            this.sbIdRep.Name = "sbIdRep";
            this.sbIdRep.Size = new System.Drawing.Size(24, 20);
            this.sbIdRep.TabIndex = 2;
            this.sbIdRep.TabStop = false;
            this.sbIdRep.Text = "...";
            this.sbIdRep.Click += new System.EventHandler(this.sbIdRep_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(8, 11);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(39, 13);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "Relógio:";
            // 
            // FormConfiguraHorario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 202);
            this.Controls.Add(this.sbAjuda);
            this.Controls.Add(this.sbExportar);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormConfiguraHorario";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "REP - Configurar Data e Hora";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormEnviarFuncionarios_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormImportacaoBilhetes_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chbEnviarDataEHoraAtual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbEnviarHorarioDeVerao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDataHoraRelogio)).EndInit();
            this.gcDataHoraRelogio.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chbDataHoraComputador.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataHoraAtual.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataHoraAtual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHorarioVerao)).EndInit();
            this.gcHorarioVerao.ResumeLayout(false);
            this.gcHorarioVerao.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInicioHorarioVerao.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInicioHorarioVerao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerminoHorarioVerao.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTerminoHorarioVerao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdRep.Properties)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        public DevExpress.XtraEditors.SimpleButton sbExportar;
        public DevExpress.XtraEditors.SimpleButton sbCancelar;
        public DevExpress.XtraEditors.SimpleButton sbAjuda;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
        private DevExpress.XtraEditors.GroupControl gcDataHoraRelogio;
        private DevExpress.XtraEditors.DateEdit txtDataHoraAtual;
        private DevExpress.XtraEditors.CheckEdit chbEnviarDataEHoraAtual;
        private DevExpress.XtraEditors.CheckEdit chbDataHoraComputador;
        private DevExpress.XtraEditors.CheckEdit chbEnviarHorarioDeVerao;
        private DevExpress.XtraEditors.GroupControl gcHorarioVerao;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit txtInicioHorarioVerao;
        private DevExpress.XtraEditors.DateEdit txtTerminoHorarioVerao;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private Componentes.devexpress.cwk_DevButton sbIdRep;
        private Componentes.devexpress.cwk_DevLookup cbIdRep;

    }
}