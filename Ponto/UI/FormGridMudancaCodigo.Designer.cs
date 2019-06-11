namespace UI
{
    partial class FormGridMudancaCodigo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridMudancaCodigo));
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLocalizar
            // 
            // 
            // sbFechar
            // 
            this.sbFechar.Location = new System.Drawing.Point(805, 406);
            // 
            // sbConsultar
            // 
            this.sbConsultar.Location = new System.Drawing.Point(279, 406);
            this.sbConsultar.Visible = false;
            // 
            // sbAjudar
            // 
            this.sbAjudar.Location = new System.Drawing.Point(12, 406);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(617, 406);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(182, 23);
            this.simpleButton1.TabIndex = 11;
            this.simpleButton1.Text = "Alterar Código do Funcionário";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FormGridMudancaCodigo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 441);
            this.Controls.Add(this.simpleButton1);
            this.Name = "FormGridMudancaCodigo";
            this.Text = "Tabela de Alteração do Código do Funcionário";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormGridMudancaCodigo_KeyDown);
            this.Controls.SetChildIndex(this.simpleButton1, 0);
            this.Controls.SetChildIndex(this.sbConsultar, 0);
            this.Controls.SetChildIndex(this.txtLocalizar, 0);
            this.Controls.SetChildIndex(this.btFiltro, 0);
            this.Controls.SetChildIndex(this.sbFechar, 0);
            this.Controls.SetChildIndex(this.sbAjudar, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}
