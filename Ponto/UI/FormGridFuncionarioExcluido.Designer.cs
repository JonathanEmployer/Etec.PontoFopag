namespace UI
{
    partial class FormGridFuncionarioExcluido
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
            this.sbRestauraFunc = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLocalizar
            // 
            // 
            // sbExcluir
            // 
            this.sbExcluir.Location = new System.Drawing.Point(619, 402);
            this.sbExcluir.TabStop = false;
            this.sbExcluir.Visible = false;
            // 
            // sbAlterar
            // 
            this.sbAlterar.Location = new System.Drawing.Point(538, 402);
            this.sbAlterar.TabStop = false;
            this.sbAlterar.Visible = false;
            // 
            // sbIncluir
            // 
            this.sbIncluir.Location = new System.Drawing.Point(457, 402);
            this.sbIncluir.TabStop = false;
            this.sbIncluir.Visible = false;
            // 
            // sbConsultar
            // 
            this.sbConsultar.Location = new System.Drawing.Point(376, 402);
            this.sbConsultar.TabStop = false;
            this.sbConsultar.Visible = false;
            // 
            // sbRestauraFunc
            // 
            this.sbRestauraFunc.Image = global::UI.Properties.Resources.Troca_de_Usuário;
            this.sbRestauraFunc.Location = new System.Drawing.Point(12, 402);
            this.sbRestauraFunc.Name = "sbRestauraFunc";
            this.sbRestauraFunc.Size = new System.Drawing.Size(180, 23);
            this.sbRestauraFunc.TabIndex = 9;
            this.sbRestauraFunc.Text = "Restaurar Funcionário Excluído";
            this.sbRestauraFunc.Click += new System.EventHandler(this.sbRestauraFunc_Click_1);
            // 
            // FormGridFuncionarioExcluido
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 466);
            this.Controls.Add(this.sbRestauraFunc);
            this.cwTabela = "";
            this.Name = "FormGridFuncionarioExcluido";
            this.Text = "Tabela de Funcionários Excluídos";
            this.Controls.SetChildIndex(this.sbRestauraFunc, 0);
            this.Controls.SetChildIndex(this.txtLocalizar, 0);
            this.Controls.SetChildIndex(this.btFiltro, 0);
            this.Controls.SetChildIndex(this.sbFechar, 0);
            this.Controls.SetChildIndex(this.sbExcluir, 0);
            this.Controls.SetChildIndex(this.sbAlterar, 0);
            this.Controls.SetChildIndex(this.sbIncluir, 0);
            this.Controls.SetChildIndex(this.sbConsultar, 0);
            this.Controls.SetChildIndex(this.sbAjudar, 0);
            this.Controls.SetChildIndex(this.sbSelecionar, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbRestauraFunc;

    }
}
