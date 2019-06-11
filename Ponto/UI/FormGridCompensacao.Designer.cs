namespace UI
{
    partial class FormGridCompensacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridCompensacao));
            this.sbTurnoFuncionario = new DevExpress.XtraEditors.SimpleButton();
            this.sbFecharCompensacao = new DevExpress.XtraEditors.SimpleButton();
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
            // sbTurnoFuncionario
            // 
            this.sbTurnoFuncionario.Image = ((System.Drawing.Image)(resources.GetObject("sbTurnoFuncionario.Image")));
            this.sbTurnoFuncionario.Location = new System.Drawing.Point(194, 402);
            this.sbTurnoFuncionario.Name = "sbTurnoFuncionario";
            this.sbTurnoFuncionario.Size = new System.Drawing.Size(176, 23);
            this.sbTurnoFuncionario.TabIndex = 3;
            this.sbTurnoFuncionario.Text = "Desfazer Compensação";
            this.sbTurnoFuncionario.Click += new System.EventHandler(this.sbDesfazerCompensacao_Click);
            // 
            // sbFecharCompensacao
            // 
            this.sbFecharCompensacao.Image = ((System.Drawing.Image)(resources.GetObject("sbFecharCompensacao.Image")));
            this.sbFecharCompensacao.Location = new System.Drawing.Point(12, 402);
            this.sbFecharCompensacao.Name = "sbFecharCompensacao";
            this.sbFecharCompensacao.Size = new System.Drawing.Size(176, 23);
            this.sbFecharCompensacao.TabIndex = 2;
            this.sbFecharCompensacao.Text = "Fechar Compensação";
            this.sbFecharCompensacao.Click += new System.EventHandler(this.sbFecharCompensacao_Click);
            // 
            // FormGridCompensacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 466);
            this.Controls.Add(this.sbFecharCompensacao);
            this.Controls.Add(this.sbTurnoFuncionario);
            this.Name = "FormGridCompensacao";
            this.Text = "Tabela de Compensação";
            this.Controls.SetChildIndex(this.txtLocalizar, 0);
            this.Controls.SetChildIndex(this.btFiltro, 0);
            this.Controls.SetChildIndex(this.sbFechar, 0);
            this.Controls.SetChildIndex(this.sbExcluir, 0);
            this.Controls.SetChildIndex(this.sbAlterar, 0);
            this.Controls.SetChildIndex(this.sbIncluir, 0);
            this.Controls.SetChildIndex(this.sbConsultar, 0);
            this.Controls.SetChildIndex(this.sbAjudar, 0);
            this.Controls.SetChildIndex(this.sbSelecionar, 0);
            this.Controls.SetChildIndex(this.sbTurnoFuncionario, 0);
            this.Controls.SetChildIndex(this.sbFecharCompensacao, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbTurnoFuncionario;
        private DevExpress.XtraEditors.SimpleButton sbFecharCompensacao;

    }
}
