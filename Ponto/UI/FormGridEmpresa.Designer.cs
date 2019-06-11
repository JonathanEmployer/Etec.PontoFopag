namespace UI
{
    partial class FormGridEmpresa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridEmpresa));
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.sbUsuarios = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLocalizar
            // 
            // 
            // btFiltro
            // 
            this.btFiltro.Text = "Filtrar";
            // 
            // sbExcluir
            // 
            this.sbExcluir.Enabled = false;
            this.sbExcluir.Location = new System.Drawing.Point(643, 402);
            this.sbExcluir.Visible = false;
            // 
            // sbIncluir
            // 
            this.sbIncluir.Enabled = false;
            this.sbIncluir.Location = new System.Drawing.Point(562, 402);
            this.sbIncluir.Visible = false;
            // 
            // sbConsultar
            // 
            this.sbConsultar.Location = new System.Drawing.Point(805, 402);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = global::UI.Properties.Resources.Imprimir_Cartão_Ponto_copy;
            this.simpleButton1.Location = new System.Drawing.Point(12, 402);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(121, 23);
            this.simpleButton1.TabIndex = 8;
            this.simpleButton1.Text = "Imprimir Atestado";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // sbUsuarios
            // 
            this.sbUsuarios.Image = ((System.Drawing.Image)(resources.GetObject("sbUsuarios.Image")));
            this.sbUsuarios.Location = new System.Drawing.Point(139, 402);
            this.sbUsuarios.Name = "sbUsuarios";
            this.sbUsuarios.Size = new System.Drawing.Size(125, 23);
            this.sbUsuarios.TabIndex = 9;
            this.sbUsuarios.Text = "Usuários";
            this.sbUsuarios.Visible = false;
            this.sbUsuarios.Click += new System.EventHandler(this.sbUsuarios_Click);
            // 
            // FormGridEmpresa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 466);
            this.Controls.Add(this.sbUsuarios);
            this.Controls.Add(this.simpleButton1);
            this.Name = "FormGridEmpresa";
            this.Text = "Tabela de Empresa";
            this.Controls.SetChildIndex(this.sbExcluir, 0);
            this.Controls.SetChildIndex(this.sbAlterar, 0);
            this.Controls.SetChildIndex(this.simpleButton1, 0);
            this.Controls.SetChildIndex(this.txtLocalizar, 0);
            this.Controls.SetChildIndex(this.btFiltro, 0);
            this.Controls.SetChildIndex(this.sbFechar, 0);
            this.Controls.SetChildIndex(this.sbIncluir, 0);
            this.Controls.SetChildIndex(this.sbConsultar, 0);
            this.Controls.SetChildIndex(this.sbAjudar, 0);
            this.Controls.SetChildIndex(this.sbSelecionar, 0);
            this.Controls.SetChildIndex(this.sbUsuarios, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton sbUsuarios;
    }
}
