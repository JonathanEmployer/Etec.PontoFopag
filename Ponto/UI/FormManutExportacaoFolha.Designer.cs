namespace UI
{
    partial class FormManutExportacaoFolha
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
            this.rgTipo = new DevExpress.XtraEditors.RadioGroup();
            this.sbIdFuncionario = new Componentes.devexpress.cwk_DevButton();
            this.cbIdFuncionario = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdDepartamento = new Componentes.devexpress.cwk_DevButton();
            this.cbIdDepartamento = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.cbIdEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.sbFuncionario = new Componentes.devexpress.cwk_DevButton();
            this.sbDepartamento = new Componentes.devexpress.cwk_DevButton();
            this.sbEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.txtDataFinal = new DevExpress.XtraEditors.DateEdit();
            this.lblDataFinal = new System.Windows.Forms.Label();
            this.txtDataInicial = new DevExpress.XtraEditors.DateEdit();
            this.lblDataInicial = new System.Windows.Forms.Label();
            this.txtCaminho = new DevExpress.XtraEditors.TextEdit();
            this.sbCaminho = new Componentes.devexpress.cwk_DevButton();
            this.label4 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataInicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCaminho.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(511, 214);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.label4);
            this.xtraTabPage1.Controls.Add(this.sbCaminho);
            this.xtraTabPage1.Controls.Add(this.txtCaminho);
            this.xtraTabPage1.Controls.Add(this.txtDataFinal);
            this.xtraTabPage1.Controls.Add(this.lblDataFinal);
            this.xtraTabPage1.Controls.Add(this.txtDataInicial);
            this.xtraTabPage1.Controls.Add(this.lblDataInicial);
            this.xtraTabPage1.Controls.Add(this.sbFuncionario);
            this.xtraTabPage1.Controls.Add(this.sbDepartamento);
            this.xtraTabPage1.Controls.Add(this.sbEmpresa);
            this.xtraTabPage1.Controls.Add(this.sbIdFuncionario);
            this.xtraTabPage1.Controls.Add(this.cbIdFuncionario);
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Controls.Add(this.sbIdDepartamento);
            this.xtraTabPage1.Controls.Add(this.cbIdEmpresa);
            this.xtraTabPage1.Controls.Add(this.cbIdDepartamento);
            this.xtraTabPage1.Controls.Add(this.label1);
            this.xtraTabPage1.Controls.Add(this.sbIdEmpresa);
            this.xtraTabPage1.Controls.Add(this.label2);
            this.xtraTabPage1.Controls.Add(this.label3);
            this.xtraTabPage1.Size = new System.Drawing.Size(505, 208);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(448, 232);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(367, 232);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 232);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.rgTipo);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(483, 50);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Tipo";
            // 
            // rgTipo
            // 
            this.rgTipo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipo.Location = new System.Drawing.Point(2, 21);
            this.rgTipo.Name = "rgTipo";
            this.rgTipo.Properties.EnableFocusRect = true;
            this.rgTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Empresa"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Departamento"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Funcionário")});
            this.rgTipo.Size = new System.Drawing.Size(479, 27);
            this.rgTipo.TabIndex = 0;
            this.rgTipo.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // sbIdFuncionario
            // 
            this.sbIdFuncionario.Location = new System.Drawing.Point(850, 86);
            this.sbIdFuncionario.Name = "sbIdFuncionario";
            this.sbIdFuncionario.Size = new System.Drawing.Size(24, 20);
            this.sbIdFuncionario.TabIndex = 21;
            this.sbIdFuncionario.TabStop = false;
            this.sbIdFuncionario.Text = "...";
            // 
            // cbIdFuncionario
            // 
            this.cbIdFuncionario.ButtonLookup = this.sbIdFuncionario;
            this.cbIdFuncionario.EditValue = 0;
            this.cbIdFuncionario.Key = System.Windows.Forms.Keys.F5;
            this.cbIdFuncionario.Location = new System.Drawing.Point(87, 120);
            this.cbIdFuncionario.Name = "cbIdFuncionario";
            this.cbIdFuncionario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdFuncionario.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dscodigo", "Codigo"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome")});
            this.cbIdFuncionario.Properties.DisplayMember = "nome";
            this.cbIdFuncionario.Properties.NullText = "";
            this.cbIdFuncionario.Properties.ValueMember = "id";
            this.cbIdFuncionario.Size = new System.Drawing.Size(378, 20);
            this.cbIdFuncionario.TabIndex = 8;
            this.cbIdFuncionario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbIdFuncionario_KeyDown);
            // 
            // sbIdDepartamento
            // 
            this.sbIdDepartamento.Location = new System.Drawing.Point(850, 56);
            this.sbIdDepartamento.Name = "sbIdDepartamento";
            this.sbIdDepartamento.Size = new System.Drawing.Size(24, 20);
            this.sbIdDepartamento.TabIndex = 16;
            this.sbIdDepartamento.TabStop = false;
            this.sbIdDepartamento.Text = "...";
            // 
            // cbIdDepartamento
            // 
            this.cbIdDepartamento.ButtonLookup = this.sbIdDepartamento;
            this.cbIdDepartamento.EditValue = 0;
            this.cbIdDepartamento.Key = System.Windows.Forms.Keys.F5;
            this.cbIdDepartamento.Location = new System.Drawing.Point(87, 94);
            this.cbIdDepartamento.Name = "cbIdDepartamento";
            this.cbIdDepartamento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdDepartamento.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição")});
            this.cbIdDepartamento.Properties.DisplayMember = "descricao";
            this.cbIdDepartamento.Properties.NullText = "";
            this.cbIdDepartamento.Properties.ValueMember = "id";
            this.cbIdDepartamento.Size = new System.Drawing.Size(378, 20);
            this.cbIdDepartamento.TabIndex = 5;
            this.cbIdDepartamento.EditValueChanged += new System.EventHandler(this.cbIdDepartamento_EditValueChanged);
            this.cbIdDepartamento.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbIdDepartamento_KeyDown);
            // 
            // sbIdEmpresa
            // 
            this.sbIdEmpresa.Location = new System.Drawing.Point(850, 25);
            this.sbIdEmpresa.Name = "sbIdEmpresa";
            this.sbIdEmpresa.Size = new System.Drawing.Size(24, 20);
            this.sbIdEmpresa.TabIndex = 11;
            this.sbIdEmpresa.TabStop = false;
            this.sbIdEmpresa.Text = "...";
            // 
            // cbIdEmpresa
            // 
            this.cbIdEmpresa.ButtonLookup = this.sbIdEmpresa;
            this.cbIdEmpresa.EditValue = 0;
            this.cbIdEmpresa.Key = System.Windows.Forms.Keys.F5;
            this.cbIdEmpresa.Location = new System.Drawing.Point(87, 68);
            this.cbIdEmpresa.Name = "cbIdEmpresa";
            this.cbIdEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.cbIdEmpresa.Properties.DisplayMember = "nome";
            this.cbIdEmpresa.Properties.NullText = "";
            this.cbIdEmpresa.Properties.ValueMember = "id";
            this.cbIdEmpresa.Size = new System.Drawing.Size(378, 20);
            this.cbIdEmpresa.TabIndex = 2;
            this.cbIdEmpresa.EditValueChanged += new System.EventHandler(this.cbIdEmpresa_EditValueChanged);
            this.cbIdEmpresa.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbIdEmpresa_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Funcionário:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Departamento: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Empresa: ";
            // 
            // sbFuncionario
            // 
            this.sbFuncionario.Location = new System.Drawing.Point(471, 120);
            this.sbFuncionario.Name = "sbFuncionario";
            this.sbFuncionario.Size = new System.Drawing.Size(24, 20);
            this.sbFuncionario.TabIndex = 9;
            this.sbFuncionario.TabStop = false;
            this.sbFuncionario.Text = "...";
            this.sbFuncionario.Click += new System.EventHandler(this.sbFuncionario_Click);
            // 
            // sbDepartamento
            // 
            this.sbDepartamento.Location = new System.Drawing.Point(471, 94);
            this.sbDepartamento.Name = "sbDepartamento";
            this.sbDepartamento.Size = new System.Drawing.Size(24, 20);
            this.sbDepartamento.TabIndex = 6;
            this.sbDepartamento.TabStop = false;
            this.sbDepartamento.Text = "...";
            this.sbDepartamento.Click += new System.EventHandler(this.sbDepartamento_Click);
            // 
            // sbEmpresa
            // 
            this.sbEmpresa.Location = new System.Drawing.Point(471, 68);
            this.sbEmpresa.Name = "sbEmpresa";
            this.sbEmpresa.Size = new System.Drawing.Size(24, 20);
            this.sbEmpresa.TabIndex = 3;
            this.sbEmpresa.TabStop = false;
            this.sbEmpresa.Text = "...";
            this.sbEmpresa.Click += new System.EventHandler(this.sbEmpresa_Click);
            // 
            // txtDataFinal
            // 
            this.txtDataFinal.EditValue = null;
            this.txtDataFinal.Location = new System.Drawing.Point(386, 172);
            this.txtDataFinal.Name = "txtDataFinal";
            this.txtDataFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataFinal.Size = new System.Drawing.Size(79, 20);
            this.txtDataFinal.TabIndex = 16;
            // 
            // lblDataFinal
            // 
            this.lblDataFinal.AutoSize = true;
            this.lblDataFinal.Location = new System.Drawing.Point(322, 175);
            this.lblDataFinal.Name = "lblDataFinal";
            this.lblDataFinal.Size = new System.Drawing.Size(58, 13);
            this.lblDataFinal.TabIndex = 15;
            this.lblDataFinal.Text = "Data Final:";
            // 
            // txtDataInicial
            // 
            this.txtDataInicial.EditValue = null;
            this.txtDataInicial.Location = new System.Drawing.Point(87, 172);
            this.txtDataInicial.Name = "txtDataInicial";
            this.txtDataInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataInicial.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataInicial.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataInicial.Size = new System.Drawing.Size(79, 20);
            this.txtDataInicial.TabIndex = 14;
            // 
            // lblDataInicial
            // 
            this.lblDataInicial.AutoSize = true;
            this.lblDataInicial.Location = new System.Drawing.Point(18, 175);
            this.lblDataInicial.Name = "lblDataInicial";
            this.lblDataInicial.Size = new System.Drawing.Size(63, 13);
            this.lblDataInicial.TabIndex = 13;
            this.lblDataInicial.Text = "Data Inicial:";
            // 
            // txtCaminho
            // 
            this.txtCaminho.Location = new System.Drawing.Point(87, 146);
            this.txtCaminho.Name = "txtCaminho";
            this.txtCaminho.Size = new System.Drawing.Size(378, 20);
            this.txtCaminho.TabIndex = 11;
            this.txtCaminho.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCaminho_KeyDown);
            // 
            // sbCaminho
            // 
            this.sbCaminho.Location = new System.Drawing.Point(471, 146);
            this.sbCaminho.Name = "sbCaminho";
            this.sbCaminho.Size = new System.Drawing.Size(24, 20);
            this.sbCaminho.TabIndex = 12;
            this.sbCaminho.TabStop = false;
            this.sbCaminho.Text = "...";
            this.sbCaminho.Click += new System.EventHandler(this.sbCaminho_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Caminho:";
            // 
            // FormManutExportacaoFolha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(535, 267);
            this.Name = "FormManutExportacaoFolha";
            this.Text = "Exportação";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataInicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCaminho.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.GroupControl groupControl1;
        public DevExpress.XtraEditors.RadioGroup rgTipo;
        private Componentes.devexpress.cwk_DevButton sbIdFuncionario;
        private Componentes.devexpress.cwk_DevLookup cbIdFuncionario;
        private Componentes.devexpress.cwk_DevButton sbIdDepartamento;
        private Componentes.devexpress.cwk_DevLookup cbIdEmpresa;
        private Componentes.devexpress.cwk_DevButton sbIdEmpresa;
        private Componentes.devexpress.cwk_DevLookup cbIdDepartamento;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Componentes.devexpress.cwk_DevButton sbFuncionario;
        private Componentes.devexpress.cwk_DevButton sbDepartamento;
        private Componentes.devexpress.cwk_DevButton sbEmpresa;
        private System.Windows.Forms.Label label4;
        private Componentes.devexpress.cwk_DevButton sbCaminho;
        private DevExpress.XtraEditors.TextEdit txtCaminho;
        private DevExpress.XtraEditors.DateEdit txtDataFinal;
        private System.Windows.Forms.Label lblDataFinal;
        private DevExpress.XtraEditors.DateEdit txtDataInicial;
        private System.Windows.Forms.Label lblDataInicial;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}
