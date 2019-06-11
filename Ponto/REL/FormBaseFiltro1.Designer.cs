namespace REL
{
    partial class FormBaseFiltro1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sbIdFuncionario = new Componentes.devexpress.cwk_DevButton();
            this.cbIdFuncionario = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdDepartamento = new Componentes.devexpress.cwk_DevButton();
            this.cbIdDepartamento = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.cbIdEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 260);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 256);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(532, 256);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(453, 256);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(595, 238);
            this.TabControl1.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Size = new System.Drawing.Size(586, 229);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.sbIdFuncionario);
            this.groupBox1.Controls.Add(this.cbIdFuncionario);
            this.groupBox1.Controls.Add(this.sbIdDepartamento);
            this.groupBox1.Controls.Add(this.cbIdDepartamento);
            this.groupBox1.Controls.Add(this.sbIdEmpresa);
            this.groupBox1.Controls.Add(this.cbIdEmpresa);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(41, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(517, 160);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tipo";
            // 
            // sbIdFuncionario
            // 
            this.sbIdFuncionario.Location = new System.Drawing.Point(476, 120);
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
            this.cbIdFuncionario.Location = new System.Drawing.Point(81, 120);
            this.cbIdFuncionario.Name = "cbIdFuncionario";
            this.cbIdFuncionario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdFuncionario.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dscodigo", "Codigo", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdFuncionario.Properties.DisplayMember = "nome";
            this.cbIdFuncionario.Properties.NullText = "";
            this.cbIdFuncionario.Properties.ValueMember = "id";
            this.cbIdFuncionario.Size = new System.Drawing.Size(389, 20);
            this.cbIdFuncionario.TabIndex = 20;
            // 
            // sbIdDepartamento
            // 
            this.sbIdDepartamento.Location = new System.Drawing.Point(476, 90);
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
            this.cbIdDepartamento.Location = new System.Drawing.Point(81, 90);
            this.cbIdDepartamento.Name = "cbIdDepartamento";
            this.cbIdDepartamento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdDepartamento.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdDepartamento.Properties.DisplayMember = "descricao";
            this.cbIdDepartamento.Properties.NullText = "";
            this.cbIdDepartamento.Properties.ValueMember = "id";
            this.cbIdDepartamento.Size = new System.Drawing.Size(389, 20);
            this.cbIdDepartamento.TabIndex = 15;
            // 
            // sbIdEmpresa
            // 
            this.sbIdEmpresa.Location = new System.Drawing.Point(476, 59);
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
            this.cbIdEmpresa.Location = new System.Drawing.Point(81, 59);
            this.cbIdEmpresa.Name = "cbIdEmpresa";
            this.cbIdEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdEmpresa.Properties.DisplayMember = "nome";
            this.cbIdEmpresa.Properties.NullText = "";
            this.cbIdEmpresa.Properties.ValueMember = "id";
            this.cbIdEmpresa.Size = new System.Drawing.Size(389, 20);
            this.cbIdEmpresa.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Funcionário:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Departamento: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Empresa: ";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(390, 19);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(80, 17);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Funcionário";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(219, 19);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(92, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Departamento";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(78, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(66, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Empresa";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // FormBaseFiltro1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(619, 285);
            this.Name = "FormBaseFiltro1";
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private Componentes.devexpress.cwk_DevButton sbIdFuncionario;
        private Componentes.devexpress.cwk_DevLookup cbIdFuncionario;
        private Componentes.devexpress.cwk_DevButton sbIdDepartamento;
        private Componentes.devexpress.cwk_DevLookup cbIdDepartamento;
        private Componentes.devexpress.cwk_DevButton sbIdEmpresa;
        private Componentes.devexpress.cwk_DevLookup cbIdEmpresa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}
