namespace ServicoIntegracaoRep
{
    partial class FormPrincipal
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.pnlParamsServico = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbStatusServico = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.LblTotalReq = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlDados = new System.Windows.Forms.Panel();
            this.btnAtualizar = new System.Windows.Forms.Button();
            this.gridRequisicoes = new DevExpress.XtraGrid.GridControl();
            this.gridViewRequisicoes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnEmpresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnNumSerie = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnRequisicao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnRetorno = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTempoDormir = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnDataHoraRequisicao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnRequisicoesExecucaoAtual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnTotalDeRequisicoes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnErro = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lbReqAtual = new System.Windows.Forms.Label();
            this.lbRequisicoesAtuais = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlParamsServico.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlDados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRequisicoes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewRequisicoes)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlParamsServico
            // 
            this.pnlParamsServico.BackColor = System.Drawing.SystemColors.HotTrack;
            this.pnlParamsServico.Controls.Add(this.pictureBox1);
            this.pnlParamsServico.Controls.Add(this.lbStatusServico);
            this.pnlParamsServico.Controls.Add(this.label1);
            this.pnlParamsServico.Controls.Add(this.btnIniciar);
            this.pnlParamsServico.Controls.Add(this.label4);
            this.pnlParamsServico.Controls.Add(this.txtURL);
            this.pnlParamsServico.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlParamsServico.Location = new System.Drawing.Point(0, 0);
            this.pnlParamsServico.Name = "pnlParamsServico";
            this.pnlParamsServico.Size = new System.Drawing.Size(1019, 65);
            this.pnlParamsServico.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(843, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(173, 61);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // lbStatusServico
            // 
            this.lbStatusServico.AutoSize = true;
            this.lbStatusServico.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatusServico.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbStatusServico.Location = new System.Drawing.Point(125, 39);
            this.lbStatusServico.Name = "lbStatusServico";
            this.lbStatusServico.Size = new System.Drawing.Size(47, 13);
            this.lbStatusServico.TabIndex = 21;
            this.lbStatusServico.Text = "Parado";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(10, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Status do Serviço:";
            // 
            // btnIniciar
            // 
            this.btnIniciar.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnIniciar.FlatAppearance.BorderSize = 0;
            this.btnIniciar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIniciar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIniciar.Image = global::ServicoIntegracaoRep.Properties.Resources.stop_alt;
            this.btnIniciar.Location = new System.Drawing.Point(715, 2);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(42, 39);
            this.btnIniciar.TabIndex = 19;
            this.toolTip1.SetToolTip(this.btnIniciar, "Inicia/Pausa o serviço HTTP que recebe as requisições.");
            this.btnIniciar.UseVisualStyleBackColor = false;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label4.Location = new System.Drawing.Point(12, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Endereço do Serviço:";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(128, 12);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(581, 20);
            this.txtURL.TabIndex = 17;
            this.txtURL.Text = "http://192.168.2.57:10949/";
            this.toolTip1.SetToolTip(this.txtURL, "Endereço onde a aplicação ficará \"Escutando\" as requisições HTTP");
            this.txtURL.TextChanged += new System.EventHandler(this.txtURL_TextChanged);
            // 
            // LblTotalReq
            // 
            this.LblTotalReq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LblTotalReq.AutoSize = true;
            this.LblTotalReq.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTotalReq.Location = new System.Drawing.Point(523, 15);
            this.LblTotalReq.Name = "LblTotalReq";
            this.LblTotalReq.Size = new System.Drawing.Size(14, 13);
            this.LblTotalReq.TabIndex = 23;
            this.LblTotalReq.Text = "0";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(390, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Total de Requisições:";
            // 
            // pnlDados
            // 
            this.pnlDados.Controls.Add(this.btnAtualizar);
            this.pnlDados.Controls.Add(this.gridRequisicoes);
            this.pnlDados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDados.Location = new System.Drawing.Point(0, 65);
            this.pnlDados.Name = "pnlDados";
            this.pnlDados.Size = new System.Drawing.Size(1019, 531);
            this.pnlDados.TabIndex = 1;
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAtualizar.Image = global::ServicoIntegracaoRep.Properties.Resources.pause;
            this.btnAtualizar.Location = new System.Drawing.Point(971, 491);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(47, 39);
            this.btnAtualizar.TabIndex = 1;
            this.btnAtualizar.Tag = "";
            this.toolTip1.SetToolTip(this.btnAtualizar, "Pausa a Atualização da Grid");
            this.btnAtualizar.UseVisualStyleBackColor = true;
            this.btnAtualizar.Click += new System.EventHandler(this.btnAtualizar_Click);
            // 
            // gridRequisicoes
            // 
            this.gridRequisicoes.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.gridRequisicoes.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gridRequisicoes.Location = new System.Drawing.Point(0, 0);
            this.gridRequisicoes.MainView = this.gridViewRequisicoes;
            this.gridRequisicoes.Name = "gridRequisicoes";
            this.gridRequisicoes.Size = new System.Drawing.Size(1019, 531);
            this.gridRequisicoes.TabIndex = 0;
            this.gridRequisicoes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewRequisicoes});
            // 
            // gridViewRequisicoes
            // 
            this.gridViewRequisicoes.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnEmpresa,
            this.gridColumnNumSerie,
            this.gridColumnRequisicao,
            this.gridColumnRetorno,
            this.gridColumnTempoDormir,
            this.gridColumnDataHoraRequisicao,
            this.gridColumnRequisicoesExecucaoAtual,
            this.gridColumnTotalDeRequisicoes,
            this.gridColumnErro});
            this.gridViewRequisicoes.GridControl = this.gridRequisicoes;
            this.gridViewRequisicoes.GroupCount = 1;
            this.gridViewRequisicoes.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Max, "RequisicoesExecucaoAtual", null, ", Atual: {0:n0}", "Atual"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Max, "TotalDeRequisicoes", null, " Total: {0:n0}")});
            this.gridViewRequisicoes.Name = "gridViewRequisicoes";
            this.gridViewRequisicoes.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumnNumSerie, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gridViewRequisicoes.CustomDrawGroupRow += new DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventHandler(this.gridViewRequisicoes_CustomDrawGroupRow);
            this.gridViewRequisicoes.EndGrouping += new System.EventHandler(this.gridViewRequisicoes_EndGrouping);
            // 
            // gridColumnEmpresa
            // 
            this.gridColumnEmpresa.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnEmpresa.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnEmpresa.Caption = "Empresa";
            this.gridColumnEmpresa.FieldName = "Empresa";
            this.gridColumnEmpresa.Name = "gridColumnEmpresa";
            this.gridColumnEmpresa.Visible = true;
            this.gridColumnEmpresa.VisibleIndex = 0;
            this.gridColumnEmpresa.Width = 239;
            // 
            // gridColumnNumSerie
            // 
            this.gridColumnNumSerie.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnNumSerie.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnNumSerie.Caption = "Num. Série";
            this.gridColumnNumSerie.FieldName = "NumSerie";
            this.gridColumnNumSerie.Name = "gridColumnNumSerie";
            this.gridColumnNumSerie.Visible = true;
            this.gridColumnNumSerie.VisibleIndex = 1;
            // 
            // gridColumnRequisicao
            // 
            this.gridColumnRequisicao.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnRequisicao.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnRequisicao.Caption = "Requisição";
            this.gridColumnRequisicao.FieldName = "Requisicao";
            this.gridColumnRequisicao.Name = "gridColumnRequisicao";
            this.gridColumnRequisicao.Visible = true;
            this.gridColumnRequisicao.VisibleIndex = 1;
            this.gridColumnRequisicao.Width = 291;
            // 
            // gridColumnRetorno
            // 
            this.gridColumnRetorno.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnRetorno.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnRetorno.Caption = "Retorno";
            this.gridColumnRetorno.FieldName = "Retorno";
            this.gridColumnRetorno.Name = "gridColumnRetorno";
            this.gridColumnRetorno.Visible = true;
            this.gridColumnRetorno.VisibleIndex = 2;
            this.gridColumnRetorno.Width = 288;
            // 
            // gridColumnTempoDormir
            // 
            this.gridColumnTempoDormir.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnTempoDormir.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnTempoDormir.Caption = "Tempo Dormir";
            this.gridColumnTempoDormir.FieldName = "TempoDormir";
            this.gridColumnTempoDormir.Name = "gridColumnTempoDormir";
            this.gridColumnTempoDormir.Visible = true;
            this.gridColumnTempoDormir.VisibleIndex = 3;
            this.gridColumnTempoDormir.Width = 78;
            // 
            // gridColumnDataHoraRequisicao
            // 
            this.gridColumnDataHoraRequisicao.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnDataHoraRequisicao.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnDataHoraRequisicao.Caption = "Data/Hora";
            this.gridColumnDataHoraRequisicao.DisplayFormat.FormatString = "G";
            this.gridColumnDataHoraRequisicao.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumnDataHoraRequisicao.FieldName = "DataHoraRequisicao";
            this.gridColumnDataHoraRequisicao.Name = "gridColumnDataHoraRequisicao";
            this.gridColumnDataHoraRequisicao.Visible = true;
            this.gridColumnDataHoraRequisicao.VisibleIndex = 4;
            this.gridColumnDataHoraRequisicao.Width = 116;
            // 
            // gridColumnRequisicoesExecucaoAtual
            // 
            this.gridColumnRequisicoesExecucaoAtual.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnRequisicoesExecucaoAtual.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnRequisicoesExecucaoAtual.Caption = "Atuais";
            this.gridColumnRequisicoesExecucaoAtual.DisplayFormat.FormatString = "n0";
            this.gridColumnRequisicoesExecucaoAtual.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumnRequisicoesExecucaoAtual.FieldName = "RequisicoesExecucaoAtual";
            this.gridColumnRequisicoesExecucaoAtual.Name = "gridColumnRequisicoesExecucaoAtual";
            this.gridColumnRequisicoesExecucaoAtual.Width = 106;
            // 
            // gridColumnTotalDeRequisicoes
            // 
            this.gridColumnTotalDeRequisicoes.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnTotalDeRequisicoes.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnTotalDeRequisicoes.Caption = "Total";
            this.gridColumnTotalDeRequisicoes.DisplayFormat.FormatString = "n0";
            this.gridColumnTotalDeRequisicoes.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumnTotalDeRequisicoes.FieldName = "TotalDeRequisicoes";
            this.gridColumnTotalDeRequisicoes.Name = "gridColumnTotalDeRequisicoes";
            this.gridColumnTotalDeRequisicoes.Width = 122;
            // 
            // gridColumnErro
            // 
            this.gridColumnErro.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumnErro.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumnErro.Caption = "Erro";
            this.gridColumnErro.FieldName = "Erro";
            this.gridColumnErro.Name = "gridColumnErro";
            this.gridColumnErro.Visible = true;
            this.gridColumnErro.VisibleIndex = 5;
            this.gridColumnErro.Width = 242;
            // 
            // lbReqAtual
            // 
            this.lbReqAtual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbReqAtual.AutoSize = true;
            this.lbReqAtual.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbReqAtual.Location = new System.Drawing.Point(788, 15);
            this.lbReqAtual.Name = "lbReqAtual";
            this.lbReqAtual.Size = new System.Drawing.Size(14, 13);
            this.lbReqAtual.TabIndex = 25;
            this.lbReqAtual.Text = "0";
            // 
            // lbRequisicoesAtuais
            // 
            this.lbRequisicoesAtuais.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbRequisicoesAtuais.AutoSize = true;
            this.lbRequisicoesAtuais.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRequisicoesAtuais.Location = new System.Drawing.Point(590, 15);
            this.lbRequisicoesAtuais.Name = "lbRequisicoesAtuais";
            this.lbRequisicoesAtuais.Size = new System.Drawing.Size(192, 13);
            this.lbRequisicoesAtuais.TabIndex = 24;
            this.lbRequisicoesAtuais.Text = "Requisições Execução Corrente:";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.lbReqAtual);
            this.pnlBottom.Controls.Add(this.lbRequisicoesAtuais);
            this.pnlBottom.Controls.Add(this.label3);
            this.pnlBottom.Controls.Add(this.LblTotalReq);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 596);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1019, 41);
            this.pnlBottom.TabIndex = 27;
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 637);
            this.Controls.Add(this.pnlDados);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlParamsServico);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serviço de Integração com Rep HTTP";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.FormPrincipal_Load);
            this.pnlParamsServico.ResumeLayout(false);
            this.pnlParamsServico.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlDados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRequisicoes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewRequisicoes)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlParamsServico;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Panel pnlDados;
        private System.Windows.Forms.Label lbStatusServico;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraGrid.GridControl gridRequisicoes;
        private System.Windows.Forms.Label LblTotalReq;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbReqAtual;
        private System.Windows.Forms.Label lbRequisicoesAtuais;
        private System.Windows.Forms.Panel pnlBottom;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewRequisicoes;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnEmpresa;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnNumSerie;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnRequisicao;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnRetorno;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTempoDormir;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnDataHoraRequisicao;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnRequisicoesExecucaoAtual;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnTotalDeRequisicoes;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumnErro;
        private System.Windows.Forms.Button btnAtualizar;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

