namespace UI.popup
{
    partial class PopUpMarcacao
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sbDesconsideraMarcacao = new DevExpress.XtraEditors.SimpleButton();
            this.sbAtualizaMotivo = new DevExpress.XtraEditors.SimpleButton();
            this.sbRemoveTratamento = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // sbDesconsideraMarcacao
            // 
            this.sbDesconsideraMarcacao.Location = new System.Drawing.Point(0, 1);
            this.sbDesconsideraMarcacao.Name = "sbDesconsideraMarcacao";
            this.sbDesconsideraMarcacao.Size = new System.Drawing.Size(168, 23);
            this.sbDesconsideraMarcacao.TabIndex = 0;
            this.sbDesconsideraMarcacao.Text = "Desconsidera Marcação";
            // 
            // sbAtualizaMotivo
            // 
            this.sbAtualizaMotivo.Location = new System.Drawing.Point(0, 26);
            this.sbAtualizaMotivo.Name = "sbAtualizaMotivo";
            this.sbAtualizaMotivo.Size = new System.Drawing.Size(168, 23);
            this.sbAtualizaMotivo.TabIndex = 1;
            this.sbAtualizaMotivo.Text = "Atualiza Motivo";
            // 
            // sbRemoveTratamento
            // 
            this.sbRemoveTratamento.Location = new System.Drawing.Point(0, 51);
            this.sbRemoveTratamento.Name = "sbRemoveTratamento";
            this.sbRemoveTratamento.Size = new System.Drawing.Size(168, 23);
            this.sbRemoveTratamento.TabIndex = 2;
            this.sbRemoveTratamento.Text = "Remove Tratamento";
            // 
            // PopUpMarcacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.sbRemoveTratamento);
            this.Controls.Add(this.sbAtualizaMotivo);
            this.Controls.Add(this.sbDesconsideraMarcacao);
            this.Name = "PopUpMarcacao";
            this.Size = new System.Drawing.Size(168, 75);
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.SimpleButton sbDesconsideraMarcacao;
        public DevExpress.XtraEditors.SimpleButton sbAtualizaMotivo;
        public DevExpress.XtraEditors.SimpleButton sbRemoveTratamento;

    }
}
