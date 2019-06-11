namespace UI
{
    partial class FormGridLayoutExportacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridLayoutExportacao));
            this.sbHistoricoMudancaHorario = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLocalizar
            // 
            // 
            // sbFechar
            // 
            this.sbFechar.TabIndex = 10;
            // 
            // sbExcluir
            // 
            this.sbExcluir.TabIndex = 8;
            // 
            // sbAlterar
            // 
            this.sbAlterar.TabIndex = 7;
            // 
            // sbIncluir
            // 
            this.sbIncluir.TabIndex = 6;
            // 
            // sbConsultar
            // 
            this.sbConsultar.TabIndex = 5;
            // 
            // sbAjudar
            // 
            this.sbAjudar.TabIndex = 9;
            // 
            // sbHistoricoMudancaHorario
            // 
            this.sbHistoricoMudancaHorario.Image = ((System.Drawing.Image)(resources.GetObject("sbHistoricoMudancaHorario.Image")));
            this.sbHistoricoMudancaHorario.Location = new System.Drawing.Point(12, 402);
            this.sbHistoricoMudancaHorario.Name = "sbHistoricoMudancaHorario";
            this.sbHistoricoMudancaHorario.Size = new System.Drawing.Size(176, 23);
            this.sbHistoricoMudancaHorario.TabIndex = 2;
            this.sbHistoricoMudancaHorario.Text = "E&xportar";
            this.sbHistoricoMudancaHorario.Click += new System.EventHandler(this.sbHistoricoMudancaHorario_Click);
            // 
            // FormGridLayoutExportacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 466);
            this.Controls.Add(this.sbHistoricoMudancaHorario);
            this.Name = "FormGridLayoutExportacao";
            this.Text = "Tabela de Exportação";
            this.Controls.SetChildIndex(this.txtLocalizar, 0);
            this.Controls.SetChildIndex(this.btFiltro, 0);
            this.Controls.SetChildIndex(this.sbFechar, 0);
            this.Controls.SetChildIndex(this.sbExcluir, 0);
            this.Controls.SetChildIndex(this.sbAlterar, 0);
            this.Controls.SetChildIndex(this.sbIncluir, 0);
            this.Controls.SetChildIndex(this.sbConsultar, 0);
            this.Controls.SetChildIndex(this.sbAjudar, 0);
            this.Controls.SetChildIndex(this.sbSelecionar, 0);
            this.Controls.SetChildIndex(this.sbHistoricoMudancaHorario, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbHistoricoMudancaHorario;

    }
}
