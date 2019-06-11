namespace UI
{
    partial class FormManutDepartamento
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.lblDescricao = new DevExpress.XtraEditors.LabelControl();
            this.lblEmpresa = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.txtDescricao = new DevExpress.XtraEditors.TextEdit();
            this.cbIdEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.txtPercentualMaximoHorasExtras = new DevExpress.XtraEditors.CalcEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPercentualMaximoHorasExtras.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 107);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtPercentualMaximoHorasExtras);
            this.xtraTabPage1.Controls.Add(this.labelControl1);
            this.xtraTabPage1.Controls.Add(this.sbIdEmpresa);
            this.xtraTabPage1.Controls.Add(this.cbIdEmpresa);
            this.xtraTabPage1.Controls.Add(this.txtDescricao);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblEmpresa);
            this.xtraTabPage1.Controls.Add(this.lblDescricao);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(519, 101);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 125);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 125);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 125);
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
            this.lblDescricao.TabIndex = 4;
            this.lblDescricao.Text = "Descrição:";
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.Location = new System.Drawing.Point(15, 68);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(45, 13);
            this.lblEmpresa.TabIndex = 6;
            this.lblEmpresa.Text = "Empresa:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(66, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, false)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // txtDescricao
            // 
            this.txtDescricao.Location = new System.Drawing.Point(66, 39);
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.Properties.MaxLength = 50;
            this.txtDescricao.Size = new System.Drawing.Size(442, 20);
            this.txtDescricao.TabIndex = 5;
            // 
            // cbIdEmpresa
            // 
            this.cbIdEmpresa.ButtonLookup = this.sbIdEmpresa;
            this.cbIdEmpresa.EditValue = 0;
            this.cbIdEmpresa.Key = System.Windows.Forms.Keys.F5;
            this.cbIdEmpresa.Location = new System.Drawing.Point(66, 65);
            this.cbIdEmpresa.Name = "cbIdEmpresa";
            this.cbIdEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("cnpj_cpf", "CNPJ/CPF")});
            this.cbIdEmpresa.Properties.DisplayMember = "nome";
            this.cbIdEmpresa.Properties.NullText = "";
            this.cbIdEmpresa.Properties.ValueMember = "id";
            this.cbIdEmpresa.Size = new System.Drawing.Size(412, 20);
            this.cbIdEmpresa.TabIndex = 7;
            // 
            // sbIdEmpresa
            // 
            this.sbIdEmpresa.Location = new System.Drawing.Point(484, 65);
            this.sbIdEmpresa.Name = "sbIdEmpresa";
            this.sbIdEmpresa.Size = new System.Drawing.Size(24, 20);
            this.sbIdEmpresa.TabIndex = 8;
            this.sbIdEmpresa.TabStop = false;
            this.sbIdEmpresa.Text = "...";
            this.sbIdEmpresa.Click += new System.EventHandler(this.sbIdEmpresa_Click);
            // 
            // txtPercentualMaximoHorasExtras
            // 
            this.txtPercentualMaximoHorasExtras.Location = new System.Drawing.Point(334, 13);
            this.txtPercentualMaximoHorasExtras.Name = "txtPercentualMaximoHorasExtras";
            this.txtPercentualMaximoHorasExtras.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtPercentualMaximoHorasExtras.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, false)});
            this.txtPercentualMaximoHorasExtras.Properties.Mask.EditMask = "P";
            this.txtPercentualMaximoHorasExtras.Properties.Mask.ShowPlaceHolders = false;
            this.txtPercentualMaximoHorasExtras.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.txtPercentualMaximoHorasExtras.Size = new System.Drawing.Size(80, 20);
            this.txtPercentualMaximoHorasExtras.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(179, 16);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(149, 13);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Percentual Máximo Hora Extra:";
            // 
            // FormManutDepartamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 160);
            this.Name = "FormManutDepartamento";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescricao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPercentualMaximoHorasExtras.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblEmpresa;
        private DevExpress.XtraEditors.LabelControl lblDescricao;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private Componentes.devexpress.cwk_DevButton sbIdEmpresa;
        private Componentes.devexpress.cwk_DevLookup cbIdEmpresa;
        private DevExpress.XtraEditors.TextEdit txtDescricao;
        private DevExpress.XtraEditors.CalcEdit txtPercentualMaximoHorasExtras;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}
