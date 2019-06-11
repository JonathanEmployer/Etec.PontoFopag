namespace UI.Relatorios.Afastamento
{
    partial class AfastamentoOcorrencia
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
            this.lblOcorrencia = new System.Windows.Forms.Label();
            this.cbIdOcorrencia = new Componentes.devexpress.cwk_DevLookup();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdOcorrencia.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // rgTipo
            // 
            this.rgTipo.EditValue = 0;
            this.rgTipo.Location = new System.Drawing.Point(2, 21);
            this.rgTipo.Size = new System.Drawing.Size(353, 27);
            this.rgTipo.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 431);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 427);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(678, 427);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(599, 427);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(741, 409);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbIdOcorrencia);
            this.tabPage1.Controls.Add(this.lblOcorrencia);
            this.tabPage1.Size = new System.Drawing.Size(735, 403);
            this.tabPage1.Controls.SetChildIndex(this.lblOcorrencia, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl1, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl2, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl3, 0);
            this.tabPage1.Controls.SetChildIndex(this.groupControl1, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarEmpresas, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparEmpresa, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarFuncionarios, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparFuncionarios, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarDepartamentos, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparDepartamento, 0);
            this.tabPage1.Controls.SetChildIndex(this.cbIdOcorrencia, 0);
            // 
            // lblOcorrencia
            // 
            this.lblOcorrencia.AutoSize = true;
            this.lblOcorrencia.Location = new System.Drawing.Point(379, 23);
            this.lblOcorrencia.Name = "lblOcorrencia";
            this.lblOcorrencia.Size = new System.Drawing.Size(62, 13);
            this.lblOcorrencia.TabIndex = 1;
            this.lblOcorrencia.Text = "Ocorrência:";
            // 
            // cbIdOcorrencia
            // 
            this.cbIdOcorrencia.ButtonLookup = null;
            this.cbIdOcorrencia.EditValue = 0;
            this.cbIdOcorrencia.Key = System.Windows.Forms.Keys.None;
            this.cbIdOcorrencia.Location = new System.Drawing.Point(447, 20);
            this.cbIdOcorrencia.Name = "cbIdOcorrencia";
            this.cbIdOcorrencia.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdOcorrencia.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição")});
            this.cbIdOcorrencia.Properties.DisplayMember = "descricao";
            this.cbIdOcorrencia.Properties.NullText = "";
            this.cbIdOcorrencia.Properties.ValueMember = "id";
            this.cbIdOcorrencia.Size = new System.Drawing.Size(279, 20);
            this.cbIdOcorrencia.TabIndex = 2;
            // 
            // AfastamentoOcorrencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(764, 478);
            this.MinimizeBox = false;
            this.Name = "AfastamentoOcorrencia";
            this.ShowInTaskbar = false;
            this.Text = "Relatório de Afastamento por Ocorrência";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AfastamentoOcorrencia_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdOcorrencia.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblOcorrencia;
        private Componentes.devexpress.cwk_DevLookup cbIdOcorrencia;
    }
}
