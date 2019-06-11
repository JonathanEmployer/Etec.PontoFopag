namespace UI
{
    partial class FormGridFechamentoBH
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridFechamentoBH));
            this.sbFuncionarios = new DevExpress.XtraEditors.SimpleButton();
            this.sbAcerto = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLocalizar
            // 
            // 
            // sbAlterar
            // 
            this.sbAlterar.Enabled = false;
            this.sbAlterar.Location = new System.Drawing.Point(724, 431);
            this.sbAlterar.Visible = false;
            // 
            // sbIncluir
            // 
            this.sbIncluir.Enabled = false;
            this.sbIncluir.Location = new System.Drawing.Point(643, 431);
            this.sbIncluir.Visible = false;
            // 
            // sbConsultar
            // 
            this.sbConsultar.Location = new System.Drawing.Point(724, 402);
            // 
            // sbFuncionarios
            // 
            this.sbFuncionarios.Image = ((System.Drawing.Image)(resources.GetObject("sbFuncionarios.Image")));
            this.sbFuncionarios.Location = new System.Drawing.Point(12, 402);
            this.sbFuncionarios.Name = "sbFuncionarios";
            this.sbFuncionarios.Size = new System.Drawing.Size(119, 23);
            this.sbFuncionarios.TabIndex = 8;
            this.sbFuncionarios.Text = "F&uncionários";
            this.sbFuncionarios.Click += new System.EventHandler(this.sbFuncionarios_Click);
            // 
            // sbAcerto
            // 
            this.sbAcerto.Image = ((System.Drawing.Image)(resources.GetObject("sbAcerto.Image")));
            this.sbAcerto.Location = new System.Drawing.Point(137, 402);
            this.sbAcerto.Name = "sbAcerto";
            this.sbAcerto.Size = new System.Drawing.Size(119, 23);
            this.sbAcerto.TabIndex = 9;
            this.sbAcerto.Text = "Ac&erto";
            this.sbAcerto.Click += new System.EventHandler(this.sbAcerto_Click);
            // 
            // FormGridFechamentoBH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 466);
            this.Controls.Add(this.sbFuncionarios);
            this.Controls.Add(this.sbAcerto);
            this.Name = "FormGridFechamentoBH";
            this.Text = "Tabela de Fechamento do Banco de Horas";
            this.Controls.SetChildIndex(this.sbConsultar, 0);
            this.Controls.SetChildIndex(this.sbAlterar, 0);
            this.Controls.SetChildIndex(this.txtLocalizar, 0);
            this.Controls.SetChildIndex(this.sbIncluir, 0);
            this.Controls.SetChildIndex(this.sbAcerto, 0);
            this.Controls.SetChildIndex(this.sbFuncionarios, 0);
            this.Controls.SetChildIndex(this.sbAjudar, 0);
            this.Controls.SetChildIndex(this.btFiltro, 0);
            this.Controls.SetChildIndex(this.sbFechar, 0);
            this.Controls.SetChildIndex(this.sbExcluir, 0);
            this.Controls.SetChildIndex(this.sbSelecionar, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbFuncionarios;
        private DevExpress.XtraEditors.SimpleButton sbAcerto;
    }
}
