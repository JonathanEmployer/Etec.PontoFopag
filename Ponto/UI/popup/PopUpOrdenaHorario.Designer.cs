namespace UI.popup
{
    partial class PopUpOrdenaHorario
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
            this.sbHorario = new DevExpress.XtraEditors.SimpleButton();
            this.sbPeriodo = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // sbHorario
            // 
            this.sbHorario.Location = new System.Drawing.Point(0, 0);
            this.sbHorario.Name = "sbHorario";
            this.sbHorario.Size = new System.Drawing.Size(168, 23);
            this.sbHorario.TabIndex = 0;
            this.sbHorario.Text = "F5 -> Ordena por Dia";
            // 
            // sbPeriodo
            // 
            this.sbPeriodo.Location = new System.Drawing.Point(0, 23);
            this.sbPeriodo.Name = "sbPeriodo";
            this.sbPeriodo.Size = new System.Drawing.Size(168, 23);
            this.sbPeriodo.TabIndex = 1;
            this.sbPeriodo.Text = "CTRL+F5 -> Ordena por Período";
            // 
            // PopUpOrdenaHorario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.sbPeriodo);
            this.Controls.Add(this.sbHorario);
            this.Name = "PopUpOrdenaHorario";
            this.Size = new System.Drawing.Size(168, 45);
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.SimpleButton sbHorario;
        public DevExpress.XtraEditors.SimpleButton sbPeriodo;


    }
}
