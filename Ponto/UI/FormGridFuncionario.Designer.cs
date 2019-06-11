namespace UI
{
    partial class FormGridFuncionario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGridFuncionario));
            this.sbImprimirCartaoPonto = new DevExpress.XtraEditors.SimpleButton();
            this.sbTurnoFuncionario = new DevExpress.XtraEditors.SimpleButton();
            this.sbHistoricoMudancaHorario = new DevExpress.XtraEditors.SimpleButton();
            this.ckEsconderInativos = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEsconderInativos.Properties)).BeginInit();
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
            // sbImprimirCartaoPonto
            // 
            this.sbImprimirCartaoPonto.Image = ((System.Drawing.Image)(resources.GetObject("sbImprimirCartaoPonto.Image")));
            this.sbImprimirCartaoPonto.Location = new System.Drawing.Point(376, 402);
            this.sbImprimirCartaoPonto.Name = "sbImprimirCartaoPonto";
            this.sbImprimirCartaoPonto.Size = new System.Drawing.Size(176, 23);
            this.sbImprimirCartaoPonto.TabIndex = 4;
            this.sbImprimirCartaoPonto.Text = "I&mprimir Cartão Ponto";
            this.sbImprimirCartaoPonto.Click += new System.EventHandler(this.sbImprimirCartaoPonto_Click);
            // 
            // sbTurnoFuncionario
            // 
            this.sbTurnoFuncionario.Image = ((System.Drawing.Image)(resources.GetObject("sbTurnoFuncionario.Image")));
            this.sbTurnoFuncionario.Location = new System.Drawing.Point(194, 402);
            this.sbTurnoFuncionario.Name = "sbTurnoFuncionario";
            this.sbTurnoFuncionario.Size = new System.Drawing.Size(176, 23);
            this.sbTurnoFuncionario.TabIndex = 3;
            this.sbTurnoFuncionario.Text = "&Turno Funcionário";
            this.sbTurnoFuncionario.Click += new System.EventHandler(this.sbTurnoFuncionario_Click);
            // 
            // sbHistoricoMudancaHorario
            // 
            this.sbHistoricoMudancaHorario.Image = ((System.Drawing.Image)(resources.GetObject("sbHistoricoMudancaHorario.Image")));
            this.sbHistoricoMudancaHorario.Location = new System.Drawing.Point(12, 402);
            this.sbHistoricoMudancaHorario.Name = "sbHistoricoMudancaHorario";
            this.sbHistoricoMudancaHorario.Size = new System.Drawing.Size(176, 23);
            this.sbHistoricoMudancaHorario.TabIndex = 2;
            this.sbHistoricoMudancaHorario.Text = "&Histório Mudança de Horário";
            this.sbHistoricoMudancaHorario.Click += new System.EventHandler(this.sbHistoricoMudancaHorario_Click);
            // 
            // ckEsconderInativos
            // 
            this.ckEsconderInativos.Location = new System.Drawing.Point(192, 435);
            this.ckEsconderInativos.Name = "ckEsconderInativos";
            this.ckEsconderInativos.Properties.Caption = "Esconder inativos";
            this.ckEsconderInativos.Size = new System.Drawing.Size(114, 19);
            this.ckEsconderInativos.TabIndex = 11;
            this.ckEsconderInativos.CheckedChanged += new System.EventHandler(this.ckEsconderInativos_CheckedChanged);
            // 
            // FormGridFuncionario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(892, 466);
            this.Controls.Add(this.ckEsconderInativos);
            this.Controls.Add(this.sbHistoricoMudancaHorario);
            this.Controls.Add(this.sbTurnoFuncionario);
            this.Controls.Add(this.sbImprimirCartaoPonto);
            this.Name = "FormGridFuncionario";
            this.Text = "Tabela de Funcionário";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormGridFuncionario_FormClosed);
            this.Controls.SetChildIndex(this.sbImprimirCartaoPonto, 0);
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
            this.Controls.SetChildIndex(this.sbHistoricoMudancaHorario, 0);
            this.Controls.SetChildIndex(this.ckEsconderInativos, 0);
            ((System.ComponentModel.ISupportInitialize)(this.txtLocalizar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEsconderInativos.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton sbImprimirCartaoPonto;
        private DevExpress.XtraEditors.SimpleButton sbTurnoFuncionario;
        private DevExpress.XtraEditors.SimpleButton sbHistoricoMudancaHorario;
        private DevExpress.XtraEditors.CheckEdit ckEsconderInativos;

    }
}
