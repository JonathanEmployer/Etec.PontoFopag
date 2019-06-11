namespace UI
{
    partial class FormManutProvisorio
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
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.lblCodigoTemporario = new DevExpress.XtraEditors.LabelControl();
            this.lblCodigoFuncionario = new DevExpress.XtraEditors.LabelControl();
            this.lblDataf = new DevExpress.XtraEditors.LabelControl();
            this.txtDt_final = new DevExpress.XtraEditors.DateEdit();
            this.lblDatai = new DevExpress.XtraEditors.LabelControl();
            this.txtDt_inicial = new DevExpress.XtraEditors.DateEdit();
            this.cbIdDsfuncionario = new Componentes.devexpress.cwk_DevLookup();
            this.txtDsfuncionarionovo = new DevExpress.XtraEditors.CalcEdit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDt_final.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDt_final.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDt_inicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDt_inicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDsfuncionario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDsfuncionarionovo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 127);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtDsfuncionarionovo);
            this.xtraTabPage1.Controls.Add(this.cbIdDsfuncionario);
            this.xtraTabPage1.Controls.Add(this.lblDataf);
            this.xtraTabPage1.Controls.Add(this.txtDt_final);
            this.xtraTabPage1.Controls.Add(this.lblDatai);
            this.xtraTabPage1.Controls.Add(this.txtDt_inicial);
            this.xtraTabPage1.Controls.Add(this.lblCodigoFuncionario);
            this.xtraTabPage1.Controls.Add(this.lblCodigoTemporario);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(516, 118);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 145);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 145);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 145);
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(110, 10);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Size = new System.Drawing.Size(130, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(67, 13);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // lblCodigoTemporario
            // 
            this.lblCodigoTemporario.Location = new System.Drawing.Point(10, 39);
            this.lblCodigoTemporario.Name = "lblCodigoTemporario";
            this.lblCodigoTemporario.Size = new System.Drawing.Size(94, 13);
            this.lblCodigoTemporario.TabIndex = 2;
            this.lblCodigoTemporario.Text = "Código Temporário:";
            // 
            // lblCodigoFuncionario
            // 
            this.lblCodigoFuncionario.Location = new System.Drawing.Point(9, 65);
            this.lblCodigoFuncionario.Name = "lblCodigoFuncionario";
            this.lblCodigoFuncionario.Size = new System.Drawing.Size(95, 13);
            this.lblCodigoFuncionario.TabIndex = 4;
            this.lblCodigoFuncionario.Text = "Código Funcionário:";
            // 
            // lblDataf
            // 
            this.lblDataf.Location = new System.Drawing.Point(370, 91);
            this.lblDataf.Name = "lblDataf";
            this.lblDataf.Size = new System.Drawing.Size(52, 13);
            this.lblDataf.TabIndex = 8;
            this.lblDataf.Text = "Data Final:";
            // 
            // txtDt_final
            // 
            this.txtDt_final.EditValue = null;
            this.txtDt_final.Location = new System.Drawing.Point(428, 88);
            this.txtDt_final.Name = "txtDt_final";
            this.txtDt_final.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDt_final.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDt_final.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDt_final.Size = new System.Drawing.Size(80, 20);
            this.txtDt_final.TabIndex = 9;
            // 
            // lblDatai
            // 
            this.lblDatai.Location = new System.Drawing.Point(47, 91);
            this.lblDatai.Name = "lblDatai";
            this.lblDatai.Size = new System.Drawing.Size(57, 13);
            this.lblDatai.TabIndex = 6;
            this.lblDatai.Text = "Data Inicial:";
            // 
            // txtDt_inicial
            // 
            this.txtDt_inicial.EditValue = null;
            this.txtDt_inicial.Location = new System.Drawing.Point(110, 88);
            this.txtDt_inicial.Name = "txtDt_inicial";
            this.txtDt_inicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDt_inicial.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDt_inicial.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDt_inicial.Size = new System.Drawing.Size(80, 20);
            this.txtDt_inicial.TabIndex = 7;
            // 
            // cbIdDsfuncionario
            // 
            this.cbIdDsfuncionario.ButtonLookup = null;
            this.cbIdDsfuncionario.Key = System.Windows.Forms.Keys.F5;
            this.cbIdDsfuncionario.Location = new System.Drawing.Point(110, 62);
            this.cbIdDsfuncionario.Name = "cbIdDsfuncionario";
            this.cbIdDsfuncionario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdDsfuncionario.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dscodigo", "Código", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None)});
            this.cbIdDsfuncionario.Properties.DisplayMember = "nome";
            this.cbIdDsfuncionario.Properties.NullText = "";
            this.cbIdDsfuncionario.Properties.ValueMember = "dscodigo";
            this.cbIdDsfuncionario.Size = new System.Drawing.Size(398, 20);
            this.cbIdDsfuncionario.TabIndex = 5;
            // 
            // txtDsfuncionarionovo
            // 
            this.txtDsfuncionarionovo.Location = new System.Drawing.Point(110, 36);
            this.txtDsfuncionarionovo.Name = "txtDsfuncionarionovo";
            this.txtDsfuncionarionovo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtDsfuncionarionovo.Properties.Mask.EditMask = "n0";
            this.txtDsfuncionarionovo.Size = new System.Drawing.Size(130, 20);
            this.txtDsfuncionarionovo.TabIndex = 3;
            // 
            // FormManutProvisorio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 180);
            this.Name = "FormManutProvisorio";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDt_final.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDt_final.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDt_inicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDt_inicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDsfuncionario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDsfuncionarionovo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblCodigoFuncionario;
        private DevExpress.XtraEditors.LabelControl lblCodigoTemporario;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.LabelControl lblDataf;
        private DevExpress.XtraEditors.DateEdit txtDt_final;
        private DevExpress.XtraEditors.LabelControl lblDatai;
        private DevExpress.XtraEditors.DateEdit txtDt_inicial;
        private Componentes.devexpress.cwk_DevLookup cbIdDsfuncionario;
        private DevExpress.XtraEditors.CalcEdit txtDsfuncionarionovo;
    }
}
