namespace UI
{
    partial class FormManutFeriado
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
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.lblDescricao = new DevExpress.XtraEditors.LabelControl();
            this.lblData = new DevExpress.XtraEditors.LabelControl();
            this.txtDescricao = new DevExpress.XtraEditors.TextEdit();
            this.txtData = new DevExpress.XtraEditors.DateEdit();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cbIdEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.sbIdDepartamento = new Componentes.devexpress.cwk_DevButton();
            this.cbIdDepartamento = new Componentes.devexpress.cwk_DevLookup();
            this.lblDepartamento = new DevExpress.XtraEditors.LabelControl();
            this.lblEmpresa = new DevExpress.XtraEditors.LabelControl();
            this.rgTipoFeriado = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoFeriado.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 226);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.txtData);
            this.xtraTabPage1.Controls.Add(this.txtDescricao);
            this.xtraTabPage1.Controls.Add(this.lblData);
            this.xtraTabPage1.Controls.Add(this.lblDescricao);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(516, 217);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 244);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 244);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 244);
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(23, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // lblDescricao
            // 
            this.lblDescricao.Location = new System.Drawing.Point(10, 42);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(50, 13);
            this.lblDescricao.TabIndex = 2;
            this.lblDescricao.Text = "Descrição:";
            // 
            // lblData
            // 
            this.lblData.Location = new System.Drawing.Point(33, 68);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(27, 13);
            this.lblData.TabIndex = 4;
            this.lblData.Text = "Data:";
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(66, 39);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Properties.MaxLength = 50;
            this.txtDescricao.Size = new System.Drawing.Size(440, 20);
            this.txtDescricao.TabIndex = 3;
            // 
            // txtData
            // 
            this.txtData.EditValue = null;
            this.txtData.Location = new System.Drawing.Point(66, 65);
            this.txtData.Name = "txtData";
            this.txtData.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtData.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtData.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtData.Size = new System.Drawing.Size(80, 20);
            this.txtData.TabIndex = 5;
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(66, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cbIdEmpresa);
            this.groupControl1.Controls.Add(this.sbIdDepartamento);
            this.groupControl1.Controls.Add(this.cbIdDepartamento);
            this.groupControl1.Controls.Add(this.sbIdEmpresa);
            this.groupControl1.Controls.Add(this.lblDepartamento);
            this.groupControl1.Controls.Add(this.lblEmpresa);
            this.groupControl1.Controls.Add(this.rgTipoFeriado);
            this.groupControl1.Location = new System.Drawing.Point(10, 91);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(497, 117);
            this.groupControl1.TabIndex = 6;
            this.groupControl1.Text = "Tipo";
            // 
            // cbIdEmpresa
            // 
            this.cbIdEmpresa.ButtonLookup = this.sbIdEmpresa;
            this.cbIdEmpresa.EditValue = 0;
            this.cbIdEmpresa.Enabled = false;
            this.cbIdEmpresa.Key = System.Windows.Forms.Keys.F5;
            this.cbIdEmpresa.Location = new System.Drawing.Point(92, 61);
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
            this.sbIdEmpresa.Location = new System.Drawing.Point(462, 61);
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
            this.sbIdDepartamento.Location = new System.Drawing.Point(462, 87);
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
            this.cbIdDepartamento.Location = new System.Drawing.Point(92, 87);
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
            // lblDepartamento
            // 
            this.lblDepartamento.Location = new System.Drawing.Point(13, 90);
            this.lblDepartamento.Name = "lblDepartamento";
            this.lblDepartamento.Size = new System.Drawing.Size(73, 13);
            this.lblDepartamento.TabIndex = 4;
            this.lblDepartamento.Text = "Departamento:";
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.Location = new System.Drawing.Point(41, 64);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(45, 13);
            this.lblEmpresa.TabIndex = 1;
            this.lblEmpresa.Text = "Empresa:";
            // 
            // rgTipoFeriado
            // 
            this.rgTipoFeriado.Location = new System.Drawing.Point(0, 19);
            this.rgTipoFeriado.Name = "rgTipoFeriado";
            this.rgTipoFeriado.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Geral"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Empresa"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Departamento")});
            this.rgTipoFeriado.Size = new System.Drawing.Size(497, 33);
            this.rgTipoFeriado.TabIndex = 0;
            this.rgTipoFeriado.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // FormManutFeriado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 279);
            this.Name = "FormManutFeriado";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtData.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoFeriado.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.LabelControl lblData;
        private DevExpress.XtraEditors.LabelControl lblDescricao;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.DateEdit txtData;
        private DevExpress.XtraEditors.TextEdit txtDescricao;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl lblDepartamento;
        private DevExpress.XtraEditors.LabelControl lblEmpresa;
        private DevExpress.XtraEditors.RadioGroup rgTipoFeriado;
        private Componentes.devexpress.cwk_DevButton sbIdEmpresa;
        private Componentes.devexpress.cwk_DevLookup cbIdEmpresa;
        private Componentes.devexpress.cwk_DevButton sbIdDepartamento;
        private Componentes.devexpress.cwk_DevLookup cbIdDepartamento;
    }
}
