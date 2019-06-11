namespace UI
{
    partial class FormExportaArquivos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportaArquivos));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.comboTipoArquivo = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lbTipo = new System.Windows.Forms.Label();
            this.btDiretorio = new Componentes.devexpress.cwk_DevButton();
            this.txtEdtDiretorio = new DevExpress.XtraEditors.TextEdit();
            this.lbDiretorio = new System.Windows.Forms.Label();
            this.btEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.comboEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.lbEmpresa = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.dateEditFinal = new DevExpress.XtraEditors.DateEdit();
            this.dateEditInicial = new DevExpress.XtraEditors.DateEdit();
            this.lbDataFinal = new System.Windows.Forms.Label();
            this.lbDataInicial = new System.Windows.Forms.Label();
            this.btAjuda = new DevExpress.XtraEditors.SimpleButton();
            this.btCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btGravar = new DevExpress.XtraEditors.SimpleButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboTipoArquivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEdtDiretorio.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditInicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Location = new System.Drawing.Point(10, 10);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage2;
            this.xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControl1.Size = new System.Drawing.Size(583, 110);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage2});
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.comboTipoArquivo);
            this.xtraTabPage2.Controls.Add(this.lbTipo);
            this.xtraTabPage2.Controls.Add(this.btDiretorio);
            this.xtraTabPage2.Controls.Add(this.txtEdtDiretorio);
            this.xtraTabPage2.Controls.Add(this.lbDiretorio);
            this.xtraTabPage2.Controls.Add(this.btEmpresa);
            this.xtraTabPage2.Controls.Add(this.comboEmpresa);
            this.xtraTabPage2.Controls.Add(this.lbEmpresa);
            this.xtraTabPage2.Controls.Add(this.groupControl1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(577, 104);
            this.xtraTabPage2.Text = "xtraTabPage2";
            // 
            // comboTipoArquivo
            // 
            this.comboTipoArquivo.EditValue = "Escolha o tipo de arquivo";
            this.comboTipoArquivo.Location = new System.Drawing.Point(372, 61);
            this.comboTipoArquivo.Name = "comboTipoArquivo";
            this.comboTipoArquivo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboTipoArquivo.Properties.Items.AddRange(new object[] {
            "AFDT",
            "ACJEF"});
            this.comboTipoArquivo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboTipoArquivo.Size = new System.Drawing.Size(169, 20);
            this.comboTipoArquivo.TabIndex = 8;
            // 
            // lbTipo
            // 
            this.lbTipo.AutoSize = true;
            this.lbTipo.Location = new System.Drawing.Point(282, 64);
            this.lbTipo.Name = "lbTipo";
            this.lbTipo.Size = new System.Drawing.Size(84, 13);
            this.lbTipo.TabIndex = 7;
            this.lbTipo.Text = "Tipo do arquivo:";
            // 
            // btDiretorio
            // 
            this.btDiretorio.Location = new System.Drawing.Point(547, 35);
            this.btDiretorio.Name = "btDiretorio";
            this.btDiretorio.Size = new System.Drawing.Size(24, 20);
            this.btDiretorio.TabIndex = 6;
            this.btDiretorio.TabStop = false;
            this.btDiretorio.Text = "...";
            this.btDiretorio.Click += new System.EventHandler(this.btDiretorio_Click);
            // 
            // txtEdtDiretorio
            // 
            this.txtEdtDiretorio.EditValue = "Escolha o Diretório";
            this.txtEdtDiretorio.Location = new System.Drawing.Point(242, 35);
            this.txtEdtDiretorio.Name = "txtEdtDiretorio";
            this.txtEdtDiretorio.Properties.ReadOnly = true;
            this.txtEdtDiretorio.Size = new System.Drawing.Size(299, 20);
            this.txtEdtDiretorio.TabIndex = 5;
            this.txtEdtDiretorio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEdtDiretorio_KeyDown);
            // 
            // lbDiretorio
            // 
            this.lbDiretorio.AutoSize = true;
            this.lbDiretorio.Location = new System.Drawing.Point(187, 38);
            this.lbDiretorio.Name = "lbDiretorio";
            this.lbDiretorio.Size = new System.Drawing.Size(49, 13);
            this.lbDiretorio.TabIndex = 4;
            this.lbDiretorio.Text = "Diretório:";
            // 
            // btEmpresa
            // 
            this.btEmpresa.Location = new System.Drawing.Point(547, 9);
            this.btEmpresa.Name = "btEmpresa";
            this.btEmpresa.Size = new System.Drawing.Size(24, 20);
            this.btEmpresa.TabIndex = 3;
            this.btEmpresa.TabStop = false;
            this.btEmpresa.Text = "...";
            this.btEmpresa.Click += new System.EventHandler(this.btEmpresa_Click);
            // 
            // comboEmpresa
            // 
            this.comboEmpresa.ButtonLookup = this.btEmpresa;
            this.comboEmpresa.EditValue = 0;
            this.comboEmpresa.Key = System.Windows.Forms.Keys.F5;
            this.comboEmpresa.Location = new System.Drawing.Point(242, 9);
            this.comboEmpresa.Name = "comboEmpresa";
            this.comboEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "Name2", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome")});
            this.comboEmpresa.Properties.DisplayMember = "nome";
            this.comboEmpresa.Properties.NullText = "";
            this.comboEmpresa.Properties.ValueMember = "id";
            this.comboEmpresa.Size = new System.Drawing.Size(299, 20);
            this.comboEmpresa.TabIndex = 2;
            this.comboEmpresa.EditValueChanged += new System.EventHandler(this.comboEmpresa_EditValueChanged);
            // 
            // lbEmpresa
            // 
            this.lbEmpresa.AutoSize = true;
            this.lbEmpresa.Location = new System.Drawing.Point(185, 12);
            this.lbEmpresa.Name = "lbEmpresa";
            this.lbEmpresa.Size = new System.Drawing.Size(51, 13);
            this.lbEmpresa.TabIndex = 1;
            this.lbEmpresa.Text = "Empresa:";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.dateEditFinal);
            this.groupControl1.Controls.Add(this.dateEditInicial);
            this.groupControl1.Controls.Add(this.lbDataFinal);
            this.groupControl1.Controls.Add(this.lbDataInicial);
            this.groupControl1.Location = new System.Drawing.Point(12, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(167, 88);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Período";
            // 
            // dateEditFinal
            // 
            this.dateEditFinal.EditValue = null;
            this.dateEditFinal.Location = new System.Drawing.Point(78, 56);
            this.dateEditFinal.Name = "dateEditFinal";
            this.dateEditFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dateEditFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditFinal.Size = new System.Drawing.Size(79, 20);
            this.dateEditFinal.TabIndex = 3;
            // 
            // dateEditInicial
            // 
            this.dateEditInicial.EditValue = null;
            this.dateEditInicial.Location = new System.Drawing.Point(78, 28);
            this.dateEditInicial.Name = "dateEditInicial";
            this.dateEditInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEditInicial.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dateEditInicial.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEditInicial.Size = new System.Drawing.Size(79, 20);
            this.dateEditInicial.TabIndex = 2;
            // 
            // lbDataFinal
            // 
            this.lbDataFinal.AutoSize = true;
            this.lbDataFinal.Location = new System.Drawing.Point(9, 59);
            this.lbDataFinal.Name = "lbDataFinal";
            this.lbDataFinal.Size = new System.Drawing.Size(58, 13);
            this.lbDataFinal.TabIndex = 1;
            this.lbDataFinal.Text = "Data Final:";
            // 
            // lbDataInicial
            // 
            this.lbDataInicial.AutoSize = true;
            this.lbDataInicial.Location = new System.Drawing.Point(9, 31);
            this.lbDataInicial.Name = "lbDataInicial";
            this.lbDataInicial.Size = new System.Drawing.Size(63, 13);
            this.lbDataInicial.TabIndex = 0;
            this.lbDataInicial.Text = "Data Inicial:";
            // 
            // btAjuda
            // 
            this.btAjuda.Image = ((System.Drawing.Image)(resources.GetObject("btAjuda.Image")));
            this.btAjuda.Location = new System.Drawing.Point(10, 126);
            this.btAjuda.Name = "btAjuda";
            this.btAjuda.Size = new System.Drawing.Size(75, 23);
            this.btAjuda.TabIndex = 3;
            this.btAjuda.Text = "Ajuda";
            this.btAjuda.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Question;
            this.btAjuda.Click += new System.EventHandler(this.btAjuda_Click);
            // 
            // btCancel
            // 
            this.btCancel.Image = ((System.Drawing.Image)(resources.GetObject("btCancel.Image")));
            this.btCancel.Location = new System.Drawing.Point(518, 126);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 2;
            this.btCancel.Text = "Cancelar";
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btGravar
            // 
            this.btGravar.Image = ((System.Drawing.Image)(resources.GetObject("btGravar.Image")));
            this.btGravar.Location = new System.Drawing.Point(399, 126);
            this.btGravar.Name = "btGravar";
            this.btGravar.Size = new System.Drawing.Size(113, 23);
            this.btGravar.TabIndex = 1;
            this.btGravar.Text = "Gerar Arquivo";
            this.btGravar.Click += new System.EventHandler(this.btGravar_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "txt";
            this.saveFileDialog1.FileName = "ArquivoExportado";
            this.saveFileDialog1.InitialDirectory = "c:";
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // FormExportaArquivos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 159);
            this.Controls.Add(this.btGravar);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btAjuda);
            this.Controls.Add(this.xtraTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormExportaArquivos";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ministério do Trabalho";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormExportaArquivos_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormExportaArquivos_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboTipoArquivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEdtDiretorio.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditInicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEditInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Label lbEmpresa;
        private System.Windows.Forms.Label lbDataFinal;
        private System.Windows.Forms.Label lbDataInicial;
        private DevExpress.XtraEditors.DateEdit dateEditInicial;
        private DevExpress.XtraEditors.DateEdit dateEditFinal;
        private Componentes.devexpress.cwk_DevButton btEmpresa;
        private Componentes.devexpress.cwk_DevLookup comboEmpresa;
        private Componentes.devexpress.cwk_DevButton btDiretorio;
        private DevExpress.XtraEditors.TextEdit txtEdtDiretorio;
        private System.Windows.Forms.Label lbDiretorio;
        private DevExpress.XtraEditors.SimpleButton btAjuda;
        private DevExpress.XtraEditors.SimpleButton btCancel;
        private DevExpress.XtraEditors.SimpleButton btGravar;
        private System.Windows.Forms.Label lbTipo;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private DevExpress.XtraEditors.ComboBoxEdit comboTipoArquivo;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;


    }
}