namespace UI
{
    partial class FormManutMudancaHorario
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
            this.cbIdFuncao = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdFuncao = new Componentes.devexpress.cwk_DevButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.sbIdFuncionario = new Componentes.devexpress.cwk_DevButton();
            this.cbIdEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.sbIdDepartamento = new Componentes.devexpress.cwk_DevButton();
            this.cbIdDepartamento = new Componentes.devexpress.cwk_DevLookup();
            this.cbIdFuncionario = new Componentes.devexpress.cwk_DevLookup();
            this.lblFuncionário = new DevExpress.XtraEditors.LabelControl();
            this.lblDepartamento = new DevExpress.XtraEditors.LabelControl();
            this.lblEmpresa = new DevExpress.XtraEditors.LabelControl();
            this.rgTipo = new DevExpress.XtraEditors.RadioGroup();
            this.lblData = new DevExpress.XtraEditors.LabelControl();
            this.txtData = new DevExpress.XtraEditors.DateEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.rgTipoHorario = new DevExpress.XtraEditors.RadioGroup();
            this.lblTurnoNormal = new DevExpress.XtraEditors.LabelControl();
            this.cbIdTurnoNormal = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdTurnoNormal = new Componentes.devexpress.cwk_DevButton();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoHorario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdTurnoNormal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 275);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.sbIdTurnoNormal);
            this.xtraTabPage1.Controls.Add(this.cbIdTurnoNormal);
            this.xtraTabPage1.Controls.Add(this.lblTurnoNormal);
            this.xtraTabPage1.Controls.Add(this.groupControl2);
            this.xtraTabPage1.Controls.Add(this.lblData);
            this.xtraTabPage1.Controls.Add(this.txtData);
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Size = new System.Drawing.Size(516, 266);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 293);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 293);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 293);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cbIdFuncao);
            this.groupControl1.Controls.Add(this.sbIdFuncao);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.sbIdFuncionario);
            this.groupControl1.Controls.Add(this.cbIdEmpresa);
            this.groupControl1.Controls.Add(this.sbIdDepartamento);
            this.groupControl1.Controls.Add(this.cbIdDepartamento);
            this.groupControl1.Controls.Add(this.sbIdEmpresa);
            this.groupControl1.Controls.Add(this.cbIdFuncionario);
            this.groupControl1.Controls.Add(this.lblFuncionário);
            this.groupControl1.Controls.Add(this.lblDepartamento);
            this.groupControl1.Controls.Add(this.lblEmpresa);
            this.groupControl1.Controls.Add(this.rgTipo);
            this.groupControl1.Location = new System.Drawing.Point(10, 13);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(497, 161);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Tipo Mudança";
            // 
            // cbIdFuncao
            // 
            this.cbIdFuncao.ButtonLookup = this.sbIdFuncao;
            this.cbIdFuncao.EditValue = 0;
            this.cbIdFuncao.Enabled = false;
            this.cbIdFuncao.Key = System.Windows.Forms.Keys.F5;
            this.cbIdFuncao.Location = new System.Drawing.Point(89, 110);
            this.cbIdFuncao.Name = "cbIdFuncao";
            this.cbIdFuncao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdFuncao.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdFuncao.Properties.DisplayMember = "descricao";
            this.cbIdFuncao.Properties.NullText = "";
            this.cbIdFuncao.Properties.ValueMember = "id";
            this.cbIdFuncao.Size = new System.Drawing.Size(364, 20);
            this.cbIdFuncao.TabIndex = 13;
            // 
            // sbIdFuncao
            // 
            this.sbIdFuncao.Enabled = false;
            this.sbIdFuncao.Location = new System.Drawing.Point(459, 110);
            this.sbIdFuncao.Name = "sbIdFuncao";
            this.sbIdFuncao.Size = new System.Drawing.Size(24, 20);
            this.sbIdFuncao.TabIndex = 12;
            this.sbIdFuncao.TabStop = false;
            this.sbIdFuncao.Text = "...";
            this.sbIdFuncao.Click += new System.EventHandler(this.sbIdFuncao_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(44, 113);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(39, 13);
            this.labelControl1.TabIndex = 10;
            this.labelControl1.Text = "Função:";
            // 
            // sbIdFuncionario
            // 
            this.sbIdFuncionario.Enabled = false;
            this.sbIdFuncionario.Location = new System.Drawing.Point(459, 136);
            this.sbIdFuncionario.Name = "sbIdFuncionario";
            this.sbIdFuncionario.Size = new System.Drawing.Size(24, 20);
            this.sbIdFuncionario.TabIndex = 9;
            this.sbIdFuncionario.TabStop = false;
            this.sbIdFuncionario.Text = "...";
            this.sbIdFuncionario.Click += new System.EventHandler(this.sbIdFuncionario_Click);
            // 
            // cbIdEmpresa
            // 
            this.cbIdEmpresa.ButtonLookup = this.sbIdEmpresa;
            this.cbIdEmpresa.EditValue = 0;
            this.cbIdEmpresa.Enabled = false;
            this.cbIdEmpresa.Key = System.Windows.Forms.Keys.F5;
            this.cbIdEmpresa.Location = new System.Drawing.Point(89, 58);
            this.cbIdEmpresa.Name = "cbIdEmpresa";
            this.cbIdEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdEmpresa.Properties.DisplayMember = "nome";
            this.cbIdEmpresa.Properties.NullText = "";
            this.cbIdEmpresa.Properties.ValueMember = "id";
            this.cbIdEmpresa.Size = new System.Drawing.Size(364, 20);
            this.cbIdEmpresa.TabIndex = 2;
            this.cbIdEmpresa.EditValueChanged += new System.EventHandler(this.cbIdEmpresa_EditValueChanged);
            // 
            // sbIdEmpresa
            // 
            this.sbIdEmpresa.Enabled = false;
            this.sbIdEmpresa.Location = new System.Drawing.Point(459, 58);
            this.sbIdEmpresa.Name = "sbIdEmpresa";
            this.sbIdEmpresa.Size = new System.Drawing.Size(24, 20);
            this.sbIdEmpresa.TabIndex = 3;
            this.sbIdEmpresa.TabStop = false;
            this.sbIdEmpresa.Text = "...";
            this.sbIdEmpresa.Click += new System.EventHandler(this.sbIdEmpresa_Click);
            // 
            // sbIdDepartamento
            // 
            this.sbIdDepartamento.Enabled = false;
            this.sbIdDepartamento.Location = new System.Drawing.Point(459, 84);
            this.sbIdDepartamento.Name = "sbIdDepartamento";
            this.sbIdDepartamento.Size = new System.Drawing.Size(24, 20);
            this.sbIdDepartamento.TabIndex = 6;
            this.sbIdDepartamento.TabStop = false;
            this.sbIdDepartamento.Text = "...";
            this.sbIdDepartamento.Click += new System.EventHandler(this.sbIdDepartamento_Click);
            // 
            // cbIdDepartamento
            // 
            this.cbIdDepartamento.ButtonLookup = this.sbIdDepartamento;
            this.cbIdDepartamento.EditValue = 0;
            this.cbIdDepartamento.Enabled = false;
            this.cbIdDepartamento.Key = System.Windows.Forms.Keys.F5;
            this.cbIdDepartamento.Location = new System.Drawing.Point(89, 84);
            this.cbIdDepartamento.Name = "cbIdDepartamento";
            this.cbIdDepartamento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdDepartamento.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdDepartamento.Properties.DisplayMember = "descricao";
            this.cbIdDepartamento.Properties.NullText = "";
            this.cbIdDepartamento.Properties.ValueMember = "id";
            this.cbIdDepartamento.Size = new System.Drawing.Size(364, 20);
            this.cbIdDepartamento.TabIndex = 5;
            // 
            // cbIdFuncionario
            // 
            this.cbIdFuncionario.ButtonLookup = this.sbIdFuncionario;
            this.cbIdFuncionario.EditValue = 0;
            this.cbIdFuncionario.Enabled = false;
            this.cbIdFuncionario.Key = System.Windows.Forms.Keys.F5;
            this.cbIdFuncionario.Location = new System.Drawing.Point(89, 136);
            this.cbIdFuncionario.Name = "cbIdFuncionario";
            this.cbIdFuncionario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdFuncionario.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dscodigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdFuncionario.Properties.DisplayMember = "nome";
            this.cbIdFuncionario.Properties.NullText = "";
            this.cbIdFuncionario.Properties.ValueMember = "id";
            this.cbIdFuncionario.Size = new System.Drawing.Size(364, 20);
            this.cbIdFuncionario.TabIndex = 8;
            // 
            // lblFuncionário
            // 
            this.lblFuncionário.Location = new System.Drawing.Point(24, 139);
            this.lblFuncionário.Name = "lblFuncionário";
            this.lblFuncionário.Size = new System.Drawing.Size(59, 13);
            this.lblFuncionário.TabIndex = 7;
            this.lblFuncionário.Text = "Funcionário:";
            // 
            // lblDepartamento
            // 
            this.lblDepartamento.Location = new System.Drawing.Point(10, 87);
            this.lblDepartamento.Name = "lblDepartamento";
            this.lblDepartamento.Size = new System.Drawing.Size(73, 13);
            this.lblDepartamento.TabIndex = 4;
            this.lblDepartamento.Text = "Departamento:";
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.Location = new System.Drawing.Point(38, 61);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(45, 13);
            this.lblEmpresa.TabIndex = 1;
            this.lblEmpresa.Text = "Empresa:";
            // 
            // rgTipo
            // 
            this.rgTipo.Location = new System.Drawing.Point(0, 19);
            this.rgTipo.Name = "rgTipo";
            this.rgTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Empresa"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Departamento"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "Função"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Funcionário")});
            this.rgTipo.Size = new System.Drawing.Size(497, 33);
            this.rgTipo.TabIndex = 0;
            this.rgTipo.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // lblData
            // 
            this.lblData.Location = new System.Drawing.Point(127, 216);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(88, 13);
            this.lblData.TabIndex = 2;
            this.lblData.Text = "Data da Mudança:";
            // 
            // txtData
            // 
            this.txtData.EditValue = null;
            this.txtData.Location = new System.Drawing.Point(221, 213);
            this.txtData.Name = "txtData";
            this.txtData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtData.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtData.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtData.Size = new System.Drawing.Size(79, 20);
            this.txtData.TabIndex = 3;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.rgTipoHorario);
            this.groupControl2.Location = new System.Drawing.Point(10, 189);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(109, 72);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "Tipo Turno";
            // 
            // rgTipoHorario
            // 
            this.rgTipoHorario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipoHorario.Location = new System.Drawing.Point(2, 20);
            this.rgTipoHorario.Name = "rgTipoHorario";
            this.rgTipoHorario.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Turno Normal"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Turno Móvel")});
            this.rgTipoHorario.Size = new System.Drawing.Size(105, 50);
            this.rgTipoHorario.TabIndex = 0;
            this.rgTipoHorario.SelectedIndexChanged += new System.EventHandler(this.rgTipoHorario_SelectedIndexChanged);
            // 
            // lblTurnoNormal
            // 
            this.lblTurnoNormal.Location = new System.Drawing.Point(183, 242);
            this.lblTurnoNormal.Name = "lblTurnoNormal";
            this.lblTurnoNormal.Size = new System.Drawing.Size(32, 13);
            this.lblTurnoNormal.TabIndex = 4;
            this.lblTurnoNormal.Text = "Turno:";
            // 
            // cbIdTurnoNormal
            // 
            this.cbIdTurnoNormal.ButtonLookup = this.sbIdTurnoNormal;
            this.cbIdTurnoNormal.EditValue = 0;
            this.cbIdTurnoNormal.Enabled = false;
            this.cbIdTurnoNormal.Key = System.Windows.Forms.Keys.F5;
            this.cbIdTurnoNormal.Location = new System.Drawing.Point(221, 239);
            this.cbIdTurnoNormal.Name = "cbIdTurnoNormal";
            this.cbIdTurnoNormal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdTurnoNormal.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdTurnoNormal.Properties.DisplayMember = "descricao";
            this.cbIdTurnoNormal.Properties.NullText = "";
            this.cbIdTurnoNormal.Properties.ValueMember = "id";
            this.cbIdTurnoNormal.Size = new System.Drawing.Size(256, 20);
            this.cbIdTurnoNormal.TabIndex = 5;
            // 
            // sbIdTurnoNormal
            // 
            this.sbIdTurnoNormal.Enabled = false;
            this.sbIdTurnoNormal.Location = new System.Drawing.Point(483, 239);
            this.sbIdTurnoNormal.Name = "sbIdTurnoNormal";
            this.sbIdTurnoNormal.Size = new System.Drawing.Size(24, 20);
            this.sbIdTurnoNormal.TabIndex = 6;
            this.sbIdTurnoNormal.TabStop = false;
            this.sbIdTurnoNormal.Text = "...";
            this.sbIdTurnoNormal.Click += new System.EventHandler(this.sbIdTurnoNormal_Click);
            // 
            // FormManutMudancaHorario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 328);
            this.Name = "FormManutMudancaHorario";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormManutMudancaHorario_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoHorario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdTurnoNormal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private Componentes.devexpress.cwk_DevButton sbIdFuncionario;
        private Componentes.devexpress.cwk_DevLookup cbIdEmpresa;
        private Componentes.devexpress.cwk_DevButton sbIdEmpresa;
        private Componentes.devexpress.cwk_DevButton sbIdDepartamento;
        private Componentes.devexpress.cwk_DevLookup cbIdFuncionario;
        private DevExpress.XtraEditors.LabelControl lblFuncionário;
        private Componentes.devexpress.cwk_DevLookup cbIdDepartamento;
        private DevExpress.XtraEditors.LabelControl lblDepartamento;
        private DevExpress.XtraEditors.LabelControl lblEmpresa;
        private DevExpress.XtraEditors.RadioGroup rgTipo;
        private DevExpress.XtraEditors.LabelControl lblData;
        private DevExpress.XtraEditors.DateEdit txtData;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.RadioGroup rgTipoHorario;
        private Componentes.devexpress.cwk_DevButton sbIdTurnoNormal;
        private Componentes.devexpress.cwk_DevLookup cbIdTurnoNormal;
        private DevExpress.XtraEditors.LabelControl lblTurnoNormal;
        private Componentes.devexpress.cwk_DevButton sbIdFuncao;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private Componentes.devexpress.cwk_DevLookup cbIdFuncao;
    }
}
