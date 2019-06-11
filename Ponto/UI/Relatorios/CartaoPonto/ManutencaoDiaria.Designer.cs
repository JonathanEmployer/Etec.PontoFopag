namespace UI.Relatorios.CartaoPonto
{
    partial class ManutencaoDiariaold
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManutencaoDiariaold));
            this.chbSalvarFiltro = new DevExpress.XtraEditors.CheckEdit();
            this.sbAjuda = new DevExpress.XtraEditors.SimpleButton();
            this.sbGravar = new DevExpress.XtraEditors.SimpleButton();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.TabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.txtData = new DevExpress.XtraEditors.DateEdit();
            this.lblDataFinal = new DevExpress.XtraEditors.LabelControl();
            this.sbIdDepartamento = new Componentes.devexpress.cwk_DevButton();
            this.cbIdDepartamento = new Componentes.devexpress.cwk_DevLookup();
            this.lblDepartamento = new DevExpress.XtraEditors.LabelControl();
            this.sbIdEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.cbIdEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.lblEmpresa = new DevExpress.XtraEditors.LabelControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 120);
            this.chbSalvarFiltro.Name = "chbSalvarFiltro";
            this.chbSalvarFiltro.Properties.Caption = "Salvar Filtro";
            this.chbSalvarFiltro.Size = new System.Drawing.Size(89, 19);
            this.chbSalvarFiltro.TabIndex = 4;
            // 
            // sbAjuda
            // 
            this.sbAjuda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sbAjuda.Image = ((System.Drawing.Image)(resources.GetObject("sbAjuda.Image")));
            this.sbAjuda.ImageIndex = 2;
            this.sbAjuda.Location = new System.Drawing.Point(12, 116);
            this.sbAjuda.Name = "sbAjuda";
            this.sbAjuda.Size = new System.Drawing.Size(75, 23);
            this.sbAjuda.TabIndex = 3;
            this.sbAjuda.Text = "A&juda";
            this.sbAjuda.Click += new System.EventHandler(this.sbAjuda_Click);
            // 
            // sbGravar
            // 
            this.sbGravar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbGravar.Image = ((System.Drawing.Image)(resources.GetObject("sbGravar.Image")));
            this.sbGravar.ImageIndex = 0;
            this.sbGravar.Location = new System.Drawing.Point(474, 116);
            this.sbGravar.Name = "sbGravar";
            this.sbGravar.Size = new System.Drawing.Size(75, 23);
            this.sbGravar.TabIndex = 1;
            this.sbGravar.Text = "&Ok";
            this.sbGravar.Click += new System.EventHandler(this.sbGravar_Click);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbCancelar.Image = ((System.Drawing.Image)(resources.GetObject("sbCancelar.Image")));
            this.sbCancelar.ImageIndex = 1;
            this.sbCancelar.Location = new System.Drawing.Point(553, 116);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(75, 23);
            this.sbCancelar.TabIndex = 2;
            this.sbCancelar.Text = "&Cancelar";
            this.sbCancelar.Click += new System.EventHandler(this.sbCancelar_Click);
            // 
            // TabControl1
            // 
            this.TabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl1.Location = new System.Drawing.Point(12, 12);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedTabPage = this.tabPage1;
            this.TabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.TabControl1.Size = new System.Drawing.Size(616, 98);
            this.TabControl1.TabIndex = 0;
            this.TabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPage1});
            this.TabControl1.TabStop = false;
            this.TabControl1.Text = "relatorioBase";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtData);
            this.tabPage1.Controls.Add(this.lblDataFinal);
            this.tabPage1.Controls.Add(this.sbIdDepartamento);
            this.tabPage1.Controls.Add(this.cbIdDepartamento);
            this.tabPage1.Controls.Add(this.lblDepartamento);
            this.tabPage1.Controls.Add(this.sbIdEmpresa);
            this.tabPage1.Controls.Add(this.cbIdEmpresa);
            this.tabPage1.Controls.Add(this.lblEmpresa);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(607, 89);
            this.tabPage1.Text = "xtraTabPage1";
            // 
            // txtData
            // 
            this.txtData.EditValue = null;
            this.txtData.Location = new System.Drawing.Point(82, 61);
            this.txtData.Name = "txtData";
            this.txtData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtData.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtData.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtData.Size = new System.Drawing.Size(98, 20);
            this.txtData.TabIndex = 7;
            // 
            // lblDataFinal
            // 
            this.lblDataFinal.Location = new System.Drawing.Point(49, 64);
            this.lblDataFinal.Name = "lblDataFinal";
            this.lblDataFinal.Size = new System.Drawing.Size(27, 13);
            this.lblDataFinal.TabIndex = 6;
            this.lblDataFinal.Text = "Data:";
            // 
            // sbIdDepartamento
            // 
            this.sbIdDepartamento.Location = new System.Drawing.Point(578, 35);
            this.sbIdDepartamento.Name = "sbIdDepartamento";
            this.sbIdDepartamento.Size = new System.Drawing.Size(24, 20);
            this.sbIdDepartamento.TabIndex = 5;
            this.sbIdDepartamento.TabStop = false;
            this.sbIdDepartamento.Text = "...";
            this.sbIdDepartamento.Click += new System.EventHandler(this.sbIdDepartamento_Click);
            // 
            // cbIdDepartamento
            // 
            this.cbIdDepartamento.ButtonLookup = this.sbIdDepartamento;
            this.cbIdDepartamento.EditValue = 0;
            this.cbIdDepartamento.Key = System.Windows.Forms.Keys.F5;
            this.cbIdDepartamento.Location = new System.Drawing.Point(82, 35);
            this.cbIdDepartamento.Name = "cbIdDepartamento";
            this.cbIdDepartamento.Properties.AutoSearchColumnIndex = 1;
            this.cbIdDepartamento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdDepartamento.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdDepartamento.Properties.DisplayMember = "descricao";
            this.cbIdDepartamento.Properties.ImmediatePopup = true;
            this.cbIdDepartamento.Properties.NullText = "";
            this.cbIdDepartamento.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
            this.cbIdDepartamento.Properties.SortColumnIndex = 1;
            this.cbIdDepartamento.Properties.ValueMember = "id";
            this.cbIdDepartamento.Size = new System.Drawing.Size(490, 20);
            this.cbIdDepartamento.TabIndex = 4;
            // 
            // lblDepartamento
            // 
            this.lblDepartamento.Location = new System.Drawing.Point(3, 38);
            this.lblDepartamento.Name = "lblDepartamento";
            this.lblDepartamento.Size = new System.Drawing.Size(73, 13);
            this.lblDepartamento.TabIndex = 3;
            this.lblDepartamento.Text = "Departamento:";
            // 
            // sbIdEmpresa
            // 
            this.sbIdEmpresa.Location = new System.Drawing.Point(578, 9);
            this.sbIdEmpresa.Name = "sbIdEmpresa";
            this.sbIdEmpresa.Size = new System.Drawing.Size(24, 20);
            this.sbIdEmpresa.TabIndex = 2;
            this.sbIdEmpresa.TabStop = false;
            this.sbIdEmpresa.Text = "...";
            this.sbIdEmpresa.Click += new System.EventHandler(this.sbIdEmpresa_Click);
            // 
            // cbIdEmpresa
            // 
            this.cbIdEmpresa.ButtonLookup = this.sbIdEmpresa;
            this.cbIdEmpresa.EditValue = 0;
            this.cbIdEmpresa.Key = System.Windows.Forms.Keys.F5;
            this.cbIdEmpresa.Location = new System.Drawing.Point(82, 9);
            this.cbIdEmpresa.Name = "cbIdEmpresa";
            this.cbIdEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("cnpj_cpf", "CNPJ/CPF", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdEmpresa.Properties.DisplayMember = "nome";
            this.cbIdEmpresa.Properties.NullText = "";
            this.cbIdEmpresa.Properties.ValueMember = "id";
            this.cbIdEmpresa.Size = new System.Drawing.Size(490, 20);
            this.cbIdEmpresa.TabIndex = 1;
            this.cbIdEmpresa.EditValueChanged += new System.EventHandler(this.cbIdEmpresa_EditValueChanged);
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.Location = new System.Drawing.Point(31, 12);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(45, 13);
            this.lblEmpresa.TabIndex = 0;
            this.lblEmpresa.Text = "Empresa:";
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // ManutencaoDiariaold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 151);
            this.Controls.Add(this.chbSalvarFiltro);
            this.Controls.Add(this.sbAjuda);
            this.Controls.Add(this.sbGravar);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.TabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "ManutencaoDiariaold";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Relatório de Manutenção Diária";
            this.Load += new System.EventHandler(this.ManutencaoDiaria_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ManutencaoDiaria_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManutencaoDiaria_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.CheckEdit chbSalvarFiltro;
        protected DevExpress.XtraEditors.SimpleButton sbAjuda;
        protected DevExpress.XtraEditors.SimpleButton sbGravar;
        protected DevExpress.XtraEditors.SimpleButton sbCancelar;
        protected DevExpress.XtraTab.XtraTabControl TabControl1;
        protected DevExpress.XtraTab.XtraTabPage tabPage1;
        public Componentes.devexpress.cwk_DevButton sbIdEmpresa;
        public Componentes.devexpress.cwk_DevLookup cbIdEmpresa;
        private DevExpress.XtraEditors.LabelControl lblEmpresa;
        private Componentes.devexpress.cwk_DevButton sbIdDepartamento;
        private Componentes.devexpress.cwk_DevLookup cbIdDepartamento;
        private DevExpress.XtraEditors.LabelControl lblDepartamento;
        private DevExpress.XtraEditors.DateEdit txtData;
        private DevExpress.XtraEditors.LabelControl lblDataFinal;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
    }
}