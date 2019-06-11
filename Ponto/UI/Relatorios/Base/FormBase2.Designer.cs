namespace UI.Relatorios.Base
{
    partial class FormBase2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBase2));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.chbSalvarFiltro = new DevExpress.XtraEditors.CheckEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.btCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.btOk = new DevExpress.XtraEditors.SimpleButton();
            this.TabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.sbIdEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.cbIdEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.lblEmpresa = new DevExpress.XtraEditors.LabelControl();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Gravar.ico");
            this.imageList1.Images.SetKeyName(1, "cancelar.ico");
            this.imageList1.Images.SetKeyName(2, "help.ico");
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 522);
            this.chbSalvarFiltro.Name = "chbSalvarFiltro";
            this.chbSalvarFiltro.Properties.Caption = "Salvar Filtro";
            this.chbSalvarFiltro.Size = new System.Drawing.Size(89, 19);
            this.chbSalvarFiltro.TabIndex = 3;
            // 
            // simpleButton2
            // 
            this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.simpleButton2.ImageIndex = 2;
            this.simpleButton2.ImageList = this.imageList1;
            this.simpleButton2.Location = new System.Drawing.Point(12, 518);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(75, 23);
            this.simpleButton2.TabIndex = 4;
            this.simpleButton2.Text = "A&juda";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btCancelar
            // 
            this.btCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancelar.ImageIndex = 1;
            this.btCancelar.ImageList = this.imageList1;
            this.btCancelar.Location = new System.Drawing.Point(561, 518);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 23);
            this.btCancelar.TabIndex = 2;
            this.btCancelar.Text = "&Cancelar";
            this.btCancelar.Click += new System.EventHandler(this.btCancelar_Click);
            // 
            // btOk
            // 
            this.btOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btOk.ImageIndex = 0;
            this.btOk.ImageList = this.imageList1;
            this.btOk.Location = new System.Drawing.Point(482, 518);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 23);
            this.btOk.TabIndex = 1;
            this.btOk.Text = "&Ok";
            this.btOk.Click += new System.EventHandler(this.btOk_Click);
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
            this.TabControl1.Size = new System.Drawing.Size(624, 500);
            this.TabControl1.TabIndex = 0;
            this.TabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabPage1});
            this.TabControl1.TabStop = false;
            this.TabControl1.Text = "relatorioBase";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.sbIdEmpresa);
            this.tabPage1.Controls.Add(this.cbIdEmpresa);
            this.tabPage1.Controls.Add(this.lblEmpresa);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(615, 491);
            this.tabPage1.Text = "xtraTabPage1";
            // 
            // sbIdEmpresa
            // 
            this.sbIdEmpresa.Location = new System.Drawing.Point(578, 14);
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
            this.cbIdEmpresa.Location = new System.Drawing.Point(82, 14);
            this.cbIdEmpresa.Name = "cbIdEmpresa";
            this.cbIdEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("cnpj_cpf", "CNPJ/CPF", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdEmpresa.Properties.DisplayMember = "nome";
            this.cbIdEmpresa.Properties.NullText = "";
            this.cbIdEmpresa.Properties.ValueMember = "id";
            this.cbIdEmpresa.Size = new System.Drawing.Size(490, 20);
            this.cbIdEmpresa.TabIndex = 1;
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.Location = new System.Drawing.Point(31, 17);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(45, 13);
            this.lblEmpresa.TabIndex = 0;
            this.lblEmpresa.Text = "Empresa:";
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // FormBase2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(648, 547);
            this.Controls.Add(this.chbSalvarFiltro);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.TabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormBase2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormBase2";
            this.Load += new System.EventHandler(this.FormBase_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormBase2_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormBase_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        protected DevExpress.XtraEditors.CheckEdit chbSalvarFiltro;
        protected DevExpress.XtraEditors.SimpleButton simpleButton2;
        protected DevExpress.XtraEditors.SimpleButton btCancelar;
        protected DevExpress.XtraEditors.SimpleButton btOk;
        protected DevExpress.XtraTab.XtraTabControl TabControl1;
        protected DevExpress.XtraTab.XtraTabPage tabPage1;
        public DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
        private DevExpress.XtraEditors.LabelControl lblEmpresa;
        public Componentes.devexpress.cwk_DevLookup cbIdEmpresa;
        public Componentes.devexpress.cwk_DevButton sbIdEmpresa;
    }
}