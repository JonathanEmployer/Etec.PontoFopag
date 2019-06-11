namespace RegistradorBiometrico.View
{
    partial class FormRegistraPonto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegistraPonto));
            this.bwJobs = new System.ComponentModel.BackgroundWorker();
            this.btnConfiguracao = new DevExpress.XtraEditors.SimpleButton();
            this.sbRegistrarPonto = new DevExpress.XtraEditors.SimpleButton();
            this.lblHorario = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // bwJobs
            // 
            this.bwJobs.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwJobs_DoWork);
            // 
            // btnConfiguracao
            // 
            this.btnConfiguracao.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnConfiguracao.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnConfiguracao.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnConfiguracao.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("btnConfiguracao.Appearance.Image")));
            this.btnConfiguracao.Appearance.Options.UseBackColor = true;
            this.btnConfiguracao.Appearance.Options.UseFont = true;
            this.btnConfiguracao.Appearance.Options.UseForeColor = true;
            this.btnConfiguracao.Appearance.Options.UseImage = true;
            this.btnConfiguracao.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConfiguracao.Location = new System.Drawing.Point(12, 475);
            this.btnConfiguracao.Name = "btnConfiguracao";
            this.btnConfiguracao.Size = new System.Drawing.Size(113, 40);
            this.btnConfiguracao.TabIndex = 21;
            this.btnConfiguracao.Text = "Configurações";
            this.btnConfiguracao.Click += new System.EventHandler(this.btnConfiguracao_Click);
            // 
            // sbRegistrarPonto
            // 
            this.sbRegistrarPonto.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.sbRegistrarPonto.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F);
            this.sbRegistrarPonto.Appearance.ForeColor = System.Drawing.Color.Black;
            this.sbRegistrarPonto.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("sbRegistrarPonto.Appearance.Image")));
            this.sbRegistrarPonto.Appearance.Options.UseBackColor = true;
            this.sbRegistrarPonto.Appearance.Options.UseFont = true;
            this.sbRegistrarPonto.Appearance.Options.UseForeColor = true;
            this.sbRegistrarPonto.Appearance.Options.UseImage = true;
            this.sbRegistrarPonto.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sbRegistrarPonto.Location = new System.Drawing.Point(51, 319);
            this.sbRegistrarPonto.Margin = new System.Windows.Forms.Padding(0);
            this.sbRegistrarPonto.Name = "sbRegistrarPonto";
            this.sbRegistrarPonto.Size = new System.Drawing.Size(280, 60);
            this.sbRegistrarPonto.TabIndex = 20;
            this.sbRegistrarPonto.Text = "Registrar Ponto";
            // 
            // lblHorario
            // 
            this.lblHorario.Appearance.Font = new System.Drawing.Font("Tahoma", 35F);
            this.lblHorario.Appearance.ForeColor = System.Drawing.SystemColors.Control;
            this.lblHorario.Location = new System.Drawing.Point(99, 218);
            this.lblHorario.Name = "lblHorario";
            this.lblHorario.Size = new System.Drawing.Size(190, 57);
            this.lblHorario.TabIndex = 5;
            this.lblHorario.Text = "00:00:00";
            // 
            // FormRegistraPonto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(380, 527);
            this.Controls.Add(this.btnConfiguracao);
            this.Controls.Add(this.sbRegistrarPonto);
            this.Controls.Add(this.lblHorario);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(396, 566);
            this.MinimumSize = new System.Drawing.Size(396, 566);
            this.Name = "FormRegistraPonto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registrar Ponto";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRegistraPonto_FormClosing);
            this.Load += new System.EventHandler(this.FormRegistraPonto_Load);
            this.Shown += new System.EventHandler(this.FormRegistraPonto_Shown);
            this.Resize += new System.EventHandler(this.FormRegistraPonto_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblHorario;
        private System.ComponentModel.BackgroundWorker bwJobs;
        private DevExpress.XtraEditors.SimpleButton sbRegistrarPonto;
        private DevExpress.XtraEditors.SimpleButton btnConfiguracao;
    }
}