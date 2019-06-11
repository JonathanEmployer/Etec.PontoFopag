namespace cwkComunicadorWebAPIPontoWeb.Forms
{
    partial class FormAtualizacaoAplicacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAtualizacaoAplicacao));
            this.lbVersaoInstalada = new System.Windows.Forms.Label();
            this.txtbVersaoInstalada = new System.Windows.Forms.TextBox();
            this.txtbVersaoAtual = new System.Windows.Forms.TextBox();
            this.lbVersaoAtual = new System.Windows.Forms.Label();
            this.btnAtualizar = new DevExpress.XtraEditors.SimpleButton();
            this.gcArquivosAtualizar = new DevExpress.XtraGrid.GridControl();
            this.gvArquivosAtualizar = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnNomeArquivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTamanhoKB = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnSituação = new DevExpress.XtraGrid.Columns.GridColumn();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lbArquivo = new System.Windows.Forms.Label();
            this.lbProgress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gcArquivosAtualizar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvArquivosAtualizar)).BeginInit();
            this.SuspendLayout();
            // 
            // lbVersaoInstalada
            // 
            this.lbVersaoInstalada.AutoSize = true;
            this.lbVersaoInstalada.Location = new System.Drawing.Point(12, 12);
            this.lbVersaoInstalada.Name = "lbVersaoInstalada";
            this.lbVersaoInstalada.Size = new System.Drawing.Size(89, 13);
            this.lbVersaoInstalada.TabIndex = 0;
            this.lbVersaoInstalada.Text = "Versão Instalada:";
            // 
            // txtbVersaoInstalada
            // 
            this.txtbVersaoInstalada.Location = new System.Drawing.Point(107, 9);
            this.txtbVersaoInstalada.Name = "txtbVersaoInstalada";
            this.txtbVersaoInstalada.ReadOnly = true;
            this.txtbVersaoInstalada.Size = new System.Drawing.Size(343, 20);
            this.txtbVersaoInstalada.TabIndex = 1;
            // 
            // txtbVersaoAtual
            // 
            this.txtbVersaoAtual.Location = new System.Drawing.Point(107, 35);
            this.txtbVersaoAtual.Name = "txtbVersaoAtual";
            this.txtbVersaoAtual.ReadOnly = true;
            this.txtbVersaoAtual.Size = new System.Drawing.Size(343, 20);
            this.txtbVersaoAtual.TabIndex = 3;
            // 
            // lbVersaoAtual
            // 
            this.lbVersaoAtual.AutoSize = true;
            this.lbVersaoAtual.Location = new System.Drawing.Point(31, 38);
            this.lbVersaoAtual.Name = "lbVersaoAtual";
            this.lbVersaoAtual.Size = new System.Drawing.Size(70, 13);
            this.lbVersaoAtual.TabIndex = 2;
            this.lbVersaoAtual.Text = "Versão Atual:";
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Enabled = false;
            this.btnAtualizar.Image = global::cwkComunicadorWebAPIPontoWeb.Properties.Resources.Alterar_copy;
            this.btnAtualizar.Location = new System.Drawing.Point(12, 61);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(438, 23);
            this.btnAtualizar.TabIndex = 4;
            this.btnAtualizar.Text = "Atualizar";
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // gcArquivosAtualizar
            // 
            this.gcArquivosAtualizar.Location = new System.Drawing.Point(12, 90);
            this.gcArquivosAtualizar.MainView = this.gvArquivosAtualizar;
            this.gcArquivosAtualizar.Name = "gcArquivosAtualizar";
            this.gcArquivosAtualizar.Size = new System.Drawing.Size(438, 264);
            this.gcArquivosAtualizar.TabIndex = 5;
            this.gcArquivosAtualizar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvArquivosAtualizar});
            // 
            // gvArquivosAtualizar
            // 
            this.gvArquivosAtualizar.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnNomeArquivo,
            this.gridColumnTamanhoKB,
            this.gridColumnSituação});
            this.gvArquivosAtualizar.GridControl = this.gcArquivosAtualizar;
            this.gvArquivosAtualizar.Name = "gvArquivosAtualizar";
            this.gvArquivosAtualizar.OptionsBehavior.Editable = false;
            this.gvArquivosAtualizar.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumnNomeArquivo
            // 
            this.gridColumnNomeArquivo.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumnNomeArquivo.AppearanceHeader.Options.UseFont = true;
            this.gridColumnNomeArquivo.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnNomeArquivo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnNomeArquivo.Caption = "Arquivo";
            this.gridColumnNomeArquivo.FieldName = "NomeArquivo";
            this.gridColumnNomeArquivo.Name = "gridColumnNomeArquivo";
            this.gridColumnNomeArquivo.Visible = true;
            this.gridColumnNomeArquivo.VisibleIndex = 0;
            this.gridColumnNomeArquivo.Width = 657;
            // 
            // gridColumnTamanhoKB
            // 
            this.gridColumnTamanhoKB.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumnTamanhoKB.AppearanceHeader.Options.UseFont = true;
            this.gridColumnTamanhoKB.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnTamanhoKB.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnTamanhoKB.Caption = "Tamanho (KB)";
            this.gridColumnTamanhoKB.FieldName = "TamanhoKB";
            this.gridColumnTamanhoKB.Name = "gridColumnTamanhoKB";
            this.gridColumnTamanhoKB.Visible = true;
            this.gridColumnTamanhoKB.VisibleIndex = 1;
            this.gridColumnTamanhoKB.Width = 238;
            // 
            // gridColumnSituação
            // 
            this.gridColumnSituação.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumnSituação.AppearanceHeader.Options.UseFont = true;
            this.gridColumnSituação.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnSituação.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnSituação.Caption = "Situação";
            this.gridColumnSituação.FieldName = "Acao";
            this.gridColumnSituação.Name = "gridColumnSituação";
            this.gridColumnSituação.Visible = true;
            this.gridColumnSituação.VisibleIndex = 2;
            this.gridColumnSituação.Width = 183;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 382);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(438, 23);
            this.progressBar.TabIndex = 6;
            this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // lbArquivo
            // 
            this.lbArquivo.AutoSize = true;
            this.lbArquivo.BackColor = System.Drawing.Color.Transparent;
            this.lbArquivo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbArquivo.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbArquivo.Location = new System.Drawing.Point(12, 363);
            this.lbArquivo.Name = "lbArquivo";
            this.lbArquivo.Size = new System.Drawing.Size(27, 15);
            this.lbArquivo.TabIndex = 8;
            this.lbArquivo.Text = "0/0";
            // 
            // lbProgress
            // 
            this.lbProgress.AutoSize = true;
            this.lbProgress.BackColor = System.Drawing.Color.Transparent;
            this.lbProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbProgress.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbProgress.Location = new System.Drawing.Point(12, 408);
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(27, 15);
            this.lbProgress.TabIndex = 9;
            this.lbProgress.Text = "0/0";
            // 
            // FormAtualizacaoAplicacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 443);
            this.Controls.Add(this.lbProgress);
            this.Controls.Add(this.lbArquivo);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.gcArquivosAtualizar);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.txtbVersaoAtual);
            this.Controls.Add(this.lbVersaoAtual);
            this.Controls.Add(this.txtbVersaoInstalada);
            this.Controls.Add(this.lbVersaoInstalada);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAtualizacaoAplicacao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Controle de Versão";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAtualizacaoAplicacao_FormClosing);
            this.Load += new System.EventHandler(this.FormAtualizacaoAplicacao_Load);
            this.Shown += new System.EventHandler(this.FormAtualizacaoAplicacao_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gcArquivosAtualizar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvArquivosAtualizar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbVersaoInstalada;
        private System.Windows.Forms.TextBox txtbVersaoInstalada;
        private System.Windows.Forms.TextBox txtbVersaoAtual;
        private System.Windows.Forms.Label lbVersaoAtual;
        private DevExpress.XtraEditors.SimpleButton btnAtualizar;
        private DevExpress.XtraGrid.GridControl gcArquivosAtualizar;
        private DevExpress.XtraGrid.Views.Grid.GridView gvArquivosAtualizar;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnNomeArquivo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTamanhoKB;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnSituação;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lbArquivo;
        private System.Windows.Forms.Label lbProgress;
    }
}