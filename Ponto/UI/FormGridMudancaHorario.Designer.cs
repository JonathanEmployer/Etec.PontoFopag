namespace UI
{
    partial class FormGridMudancaHorario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridMudancaHorario));
            this.sbExcluirMudancaHorario = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLocalizar
            // 
            // 
            // sbExcluir
            // 
            this.sbExcluir.Enabled = false;
            this.sbExcluir.Location = new System.Drawing.Point(532, 402);
            this.sbExcluir.Visible = false;
            // 
            // sbAlterar
            // 
            this.sbAlterar.Enabled = false;
            this.sbAlterar.Location = new System.Drawing.Point(451, 402);
            this.sbAlterar.Visible = false;
            // 
            // sbIncluir
            // 
            this.sbIncluir.Enabled = false;
            this.sbIncluir.Location = new System.Drawing.Point(370, 402);
            this.sbIncluir.Visible = false;
            // 
            // sbConsultar
            // 
            this.sbConsultar.Enabled = false;
            this.sbConsultar.Location = new System.Drawing.Point(289, 402);
            this.sbConsultar.Visible = false;
            // 
            // sbExcluirMudancaHorario
            // 
            this.sbExcluirMudancaHorario.Image = ((System.Drawing.Image)(resources.GetObject("sbExcluirMudancaHorario.Image")));
            this.sbExcluirMudancaHorario.Location = new System.Drawing.Point(690, 402);
            this.sbExcluirMudancaHorario.Name = "sbExcluirMudancaHorario";
            this.sbExcluirMudancaHorario.Size = new System.Drawing.Size(190, 23);
            this.sbExcluirMudancaHorario.TabIndex = 8;
            this.sbExcluirMudancaHorario.Text = "Excluir Mudança de Horário";
            this.sbExcluirMudancaHorario.Click += new System.EventHandler(this.sbExcluirMudancaHorario_Click);
            // 
            // FormGridMudancaHorario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 466);
            this.Controls.Add(this.sbExcluirMudancaHorario);
            this.Name = "FormGridMudancaHorario";
            this.Text = "";
            this.Controls.SetChildIndex(this.sbExcluirMudancaHorario, 0);
            this.Controls.SetChildIndex(this.sbConsultar, 0);
            this.Controls.SetChildIndex(this.sbIncluir, 0);
            this.Controls.SetChildIndex(this.sbAlterar, 0);
            this.Controls.SetChildIndex(this.sbExcluir, 0);
            this.Controls.SetChildIndex(this.txtLocalizar, 0);
            this.Controls.SetChildIndex(this.btFiltro, 0);
            this.Controls.SetChildIndex(this.sbFechar, 0);
            this.Controls.SetChildIndex(this.sbAjudar, 0);
            this.Controls.SetChildIndex(this.sbSelecionar, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbExcluirMudancaHorario;
    }
}
