namespace UI
{
    partial class FormAbono
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbono));
            this.lblAbonoDiurno = new DevExpress.XtraEditors.LabelControl();
            this.lblAbonoNoturno = new DevExpress.XtraEditors.LabelControl();
            this.lblFaltaDiurna = new DevExpress.XtraEditors.LabelControl();
            this.lblFaltaNoturna = new DevExpress.XtraEditors.LabelControl();
            this.txtAbonoDiurno = new Componentes.devexpress.cwkEditHora();
            this.txtFaltaDiurna = new Componentes.devexpress.cwkEditHora();
            this.txtAbonoNoturno = new Componentes.devexpress.cwkEditHora();
            this.txtFaltaNoturna = new Componentes.devexpress.cwkEditHora();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAbonoDiurno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaltaDiurna.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAbonoNoturno.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaltaNoturna.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(421, 79);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.txtFaltaNoturna);
            this.xtraTabPage1.Controls.Add(this.txtAbonoNoturno);
            this.xtraTabPage1.Controls.Add(this.txtFaltaDiurna);
            this.xtraTabPage1.Controls.Add(this.txtAbonoDiurno);
            this.xtraTabPage1.Controls.Add(this.lblFaltaNoturna);
            this.xtraTabPage1.Controls.Add(this.lblFaltaDiurna);
            this.xtraTabPage1.Controls.Add(this.lblAbonoNoturno);
            this.xtraTabPage1.Controls.Add(this.lblAbonoDiurno);
            this.xtraTabPage1.Size = new System.Drawing.Size(412, 70);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(358, 97);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(277, 97);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 97);
            // 
            // lblAbonoDiurno
            // 
            this.lblAbonoDiurno.Location = new System.Drawing.Point(50, 15);
            this.lblAbonoDiurno.Name = "lblAbonoDiurno";
            this.lblAbonoDiurno.Size = new System.Drawing.Size(69, 13);
            this.lblAbonoDiurno.TabIndex = 0;
            this.lblAbonoDiurno.Text = "Abono Diurno:";
            // 
            // lblAbonoNoturno
            // 
            this.lblAbonoNoturno.Location = new System.Drawing.Point(244, 15);
            this.lblAbonoNoturno.Name = "lblAbonoNoturno";
            this.lblAbonoNoturno.Size = new System.Drawing.Size(77, 13);
            this.lblAbonoNoturno.TabIndex = 1;
            this.lblAbonoNoturno.Text = "Abono Noturno:";
            // 
            // lblFaltaDiurna
            // 
            this.lblFaltaDiurna.Location = new System.Drawing.Point(57, 41);
            this.lblFaltaDiurna.Name = "lblFaltaDiurna";
            this.lblFaltaDiurna.Size = new System.Drawing.Size(62, 13);
            this.lblFaltaDiurna.TabIndex = 2;
            this.lblFaltaDiurna.Text = "Falta Diurna:";
            // 
            // lblFaltaNoturna
            // 
            this.lblFaltaNoturna.Location = new System.Drawing.Point(251, 41);
            this.lblFaltaNoturna.Name = "lblFaltaNoturna";
            this.lblFaltaNoturna.Size = new System.Drawing.Size(70, 13);
            this.lblFaltaNoturna.TabIndex = 3;
            this.lblFaltaNoturna.Text = "Falta Noturna:";
            // 
            // txtAbonoDiurno
            // 
            this.txtAbonoDiurno.cwErro = false;
            this.txtAbonoDiurno.EditValue = "--:--";
            this.txtAbonoDiurno.Layout = Componentes.devexpress.LayoutsHorario.horario2Digitos;
            this.txtAbonoDiurno.lblLegenda = null;
            this.txtAbonoDiurno.lblNRelogio = null;
            this.txtAbonoDiurno.Location = new System.Drawing.Point(125, 12);
            this.txtAbonoDiurno.Name = "txtAbonoDiurno";
            this.txtAbonoDiurno.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAbonoDiurno.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtAbonoDiurno.Properties.MaxLength = 5;
            this.txtAbonoDiurno.Size = new System.Drawing.Size(35, 20);
            this.txtAbonoDiurno.TabIndex = 4;
            this.txtAbonoDiurno.ValorAnterior = null;
            this.txtAbonoDiurno.Validating += new System.ComponentModel.CancelEventHandler(this.txtAbonoDiurno_Validating);
            // 
            // txtFaltaDiurna
            // 
            this.txtFaltaDiurna.cwErro = false;
            this.txtFaltaDiurna.EditValue = "--:--";
            this.txtFaltaDiurna.Layout = Componentes.devexpress.LayoutsHorario.horario2Digitos;
            this.txtFaltaDiurna.lblLegenda = null;
            this.txtFaltaDiurna.lblNRelogio = null;
            this.txtFaltaDiurna.Location = new System.Drawing.Point(125, 38);
            this.txtFaltaDiurna.Name = "txtFaltaDiurna";
            this.txtFaltaDiurna.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtFaltaDiurna.Properties.Appearance.Options.UseBackColor = true;
            this.txtFaltaDiurna.Properties.Appearance.Options.UseTextOptions = true;
            this.txtFaltaDiurna.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtFaltaDiurna.Properties.MaxLength = 5;
            this.txtFaltaDiurna.Properties.ReadOnly = true;
            this.txtFaltaDiurna.Size = new System.Drawing.Size(35, 20);
            this.txtFaltaDiurna.TabIndex = 5;
            this.txtFaltaDiurna.ValorAnterior = null;
            // 
            // txtAbonoNoturno
            // 
            this.txtAbonoNoturno.cwErro = false;
            this.txtAbonoNoturno.EditValue = "--:--";
            this.txtAbonoNoturno.Layout = Componentes.devexpress.LayoutsHorario.horario2Digitos;
            this.txtAbonoNoturno.lblLegenda = null;
            this.txtAbonoNoturno.lblNRelogio = null;
            this.txtAbonoNoturno.Location = new System.Drawing.Point(327, 12);
            this.txtAbonoNoturno.Name = "txtAbonoNoturno";
            this.txtAbonoNoturno.Properties.Appearance.Options.UseTextOptions = true;
            this.txtAbonoNoturno.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtAbonoNoturno.Properties.MaxLength = 5;
            this.txtAbonoNoturno.Size = new System.Drawing.Size(35, 20);
            this.txtAbonoNoturno.TabIndex = 6;
            this.txtAbonoNoturno.ValorAnterior = null;
            this.txtAbonoNoturno.Validating += new System.ComponentModel.CancelEventHandler(this.txtAbonoNoturno_Validating);
            // 
            // txtFaltaNoturna
            // 
            this.txtFaltaNoturna.cwErro = false;
            this.txtFaltaNoturna.EditValue = "--:--";
            this.txtFaltaNoturna.Layout = Componentes.devexpress.LayoutsHorario.horario2Digitos;
            this.txtFaltaNoturna.lblLegenda = null;
            this.txtFaltaNoturna.lblNRelogio = null;
            this.txtFaltaNoturna.Location = new System.Drawing.Point(327, 38);
            this.txtFaltaNoturna.Name = "txtFaltaNoturna";
            this.txtFaltaNoturna.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtFaltaNoturna.Properties.Appearance.Options.UseBackColor = true;
            this.txtFaltaNoturna.Properties.Appearance.Options.UseTextOptions = true;
            this.txtFaltaNoturna.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtFaltaNoturna.Properties.MaxLength = 5;
            this.txtFaltaNoturna.Properties.ReadOnly = true;
            this.txtFaltaNoturna.Size = new System.Drawing.Size(35, 20);
            this.txtFaltaNoturna.TabIndex = 7;
            this.txtFaltaNoturna.ValorAnterior = null;
            // 
            // FormAbono
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(445, 132);
            this.Name = "FormAbono";
            this.Text = "Abono Parcial";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAbonoDiurno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaltaDiurna.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAbonoNoturno.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFaltaNoturna.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblFaltaNoturna;
        private DevExpress.XtraEditors.LabelControl lblFaltaDiurna;
        private DevExpress.XtraEditors.LabelControl lblAbonoNoturno;
        private DevExpress.XtraEditors.LabelControl lblAbonoDiurno;
        private Componentes.devexpress.cwkEditHora txtFaltaNoturna;
        private Componentes.devexpress.cwkEditHora txtAbonoNoturno;
        private Componentes.devexpress.cwkEditHora txtFaltaDiurna;
        private Componentes.devexpress.cwkEditHora txtAbonoDiurno;
    }
}
