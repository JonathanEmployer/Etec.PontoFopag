namespace RegistradorPonto.View.Util
{
    partial class FormErro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormErro));
            this.txtDetalhes = new DevExpress.XtraEditors.MemoEdit();
            this.btnDetalhes = new DevExpress.XtraEditors.SimpleButton();
            this.btnFechar = new DevExpress.XtraEditors.SimpleButton();
            this.txtMensagem = new DevExpress.XtraEditors.MemoEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDetalhes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMensagem.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDetalhes
            // 
            this.txtDetalhes.Location = new System.Drawing.Point(12, 146);
            this.txtDetalhes.Name = "txtDetalhes";
            this.txtDetalhes.Properties.ReadOnly = true;
            this.txtDetalhes.Properties.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDetalhes.Size = new System.Drawing.Size(421, 122);
            this.txtDetalhes.TabIndex = 8;
            this.txtDetalhes.TabStop = false;
            // 
            // btnDetalhes
            // 
            this.btnDetalhes.Location = new System.Drawing.Point(12, 96);
            this.btnDetalhes.Name = "btnDetalhes";
            this.btnDetalhes.Size = new System.Drawing.Size(75, 23);
            this.btnDetalhes.TabIndex = 7;
            this.btnDetalhes.Text = "Detalhes";
            this.btnDetalhes.Click += new System.EventHandler(this.btnDetalhes_Click);
            // 
            // btnFechar
            // 
            this.btnFechar.Location = new System.Drawing.Point(358, 96);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 23);
            this.btnFechar.TabIndex = 5;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // txtMensagem
            // 
            this.txtMensagem.Location = new System.Drawing.Point(102, 13);
            this.txtMensagem.Name = "txtMensagem";
            this.txtMensagem.Properties.ReadOnly = true;
            this.txtMensagem.Size = new System.Drawing.Size(331, 77);
            this.txtMensagem.TabIndex = 6;
            this.txtMensagem.TabStop = false;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(12, 12);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit1.Size = new System.Drawing.Size(84, 78);
            this.pictureEdit1.TabIndex = 9;
            // 
            // FormErro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 289);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.txtDetalhes);
            this.Controls.Add(this.btnDetalhes);
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.txtMensagem);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormErro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Erro";
            ((System.ComponentModel.ISupportInitialize)(this.txtDetalhes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMensagem.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.MemoEdit txtDetalhes;
        private DevExpress.XtraEditors.SimpleButton btnDetalhes;
        private DevExpress.XtraEditors.SimpleButton btnFechar;
        private DevExpress.XtraEditors.MemoEdit txtMensagem;
    }
}