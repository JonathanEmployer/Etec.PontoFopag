namespace TopPonto
{
    partial class FormImportacaoTopPonto
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportacaoTopPonto));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gcParametros = new DevExpress.XtraEditors.GroupControl();
            this.radioGroup1 = new DevExpress.XtraEditors.RadioGroup();
            this.BotaoDiretorio = new Componentes.devexpress.cwk_DevButton();
            this.txtArquivo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chkTodos = new DevExpress.XtraEditors.CheckEdit();
            this.gcCadastros = new DevExpress.XtraEditors.GroupControl();
            this.txtDataFinal = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.chbOrdemBilhetes = new DevExpress.XtraEditors.CheckEdit();
            this.dtMarcacao = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.chkMarcacao = new DevExpress.XtraEditors.CheckEdit();
            this.chkFechamento = new DevExpress.XtraEditors.CheckEdit();
            this.chkBancoHoras = new DevExpress.XtraEditors.CheckEdit();
            this.chkAfastamentos = new DevExpress.XtraEditors.CheckEdit();
            this.chkFuncionario = new DevExpress.XtraEditors.CheckEdit();
            this.chkHorario = new DevExpress.XtraEditors.CheckEdit();
            this.chkOcorrencia = new DevExpress.XtraEditors.CheckEdit();
            this.chkFeriado = new DevExpress.XtraEditors.CheckEdit();
            this.chkFuncao = new DevExpress.XtraEditors.CheckEdit();
            this.chkDepartamento = new DevExpress.XtraEditors.CheckEdit();
            this.pbTabela = new DevExpress.XtraEditors.ProgressBarControl();
            this.pbGeral = new DevExpress.XtraEditors.ProgressBarControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.sbFechar = new DevExpress.XtraEditors.SimpleButton();
            this.sbImportar = new DevExpress.XtraEditors.SimpleButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sbAjuda = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcParametros)).BeginInit();
            this.gcParametros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArquivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTodos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCadastros)).BeginInit();
            this.gcCadastros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbOrdemBilhetes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMarcacao.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMarcacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMarcacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFechamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBancoHoras.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAfastamentos.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFuncionario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHorario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOcorrencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFeriado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFuncao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTabela.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGeral.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraTabControl1.Location = new System.Drawing.Point(12, 12);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage2;
            this.xtraTabControl1.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            this.xtraTabControl1.Size = new System.Drawing.Size(626, 372);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gcParametros);
            this.xtraTabPage2.Controls.Add(this.gcCadastros);
            this.xtraTabPage2.Controls.Add(this.pbTabela);
            this.xtraTabPage2.Controls.Add(this.pbGeral);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(620, 366);
            this.xtraTabPage2.Text = "xtraTabPage2";
            // 
            // gcParametros
            // 
            this.gcParametros.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcParametros.Controls.Add(this.radioGroup1);
            this.gcParametros.Controls.Add(this.BotaoDiretorio);
            this.gcParametros.Controls.Add(this.txtArquivo);
            this.gcParametros.Controls.Add(this.labelControl2);
            this.gcParametros.Controls.Add(this.chkTodos);
            this.gcParametros.Location = new System.Drawing.Point(24, 13);
            this.gcParametros.Name = "gcParametros";
            this.gcParametros.Size = new System.Drawing.Size(570, 112);
            this.gcParametros.TabIndex = 4;
            this.gcParametros.Text = "Parâmetros de importação";
            // 
            // radioGroup1
            // 
            this.radioGroup1.Location = new System.Drawing.Point(6, 23);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.radioGroup1.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroup1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Top Ponto 3.4"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "TopPonto 4.0"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Top Ponto REP")});
            this.radioGroup1.Size = new System.Drawing.Size(113, 84);
            this.radioGroup1.TabIndex = 12;
            this.radioGroup1.SelectedIndexChanged += new System.EventHandler(this.radioGroup1_SelectedIndexChanged);
            // 
            // BotaoDiretorio
            // 
            this.BotaoDiretorio.Location = new System.Drawing.Point(528, 58);
            this.BotaoDiretorio.Name = "BotaoDiretorio";
            this.BotaoDiretorio.Size = new System.Drawing.Size(24, 20);
            this.BotaoDiretorio.TabIndex = 11;
            this.BotaoDiretorio.TabStop = false;
            this.BotaoDiretorio.Text = "...";
            this.BotaoDiretorio.Click += new System.EventHandler(this.BotaoDiretorio_Click);
            // 
            // txtArquivo
            // 
            this.txtArquivo.Location = new System.Drawing.Point(216, 58);
            this.txtArquivo.Name = "txtArquivo";
            this.txtArquivo.Size = new System.Drawing.Size(306, 20);
            this.txtArquivo.TabIndex = 4;
            this.txtArquivo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtArquivo_KeyDown);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(169, 61);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(41, 13);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Arquivo:";
            // 
            // chkTodos
            // 
            this.chkTodos.Location = new System.Drawing.Point(167, 33);
            this.chkTodos.Name = "chkTodos";
            this.chkTodos.Properties.Caption = "Importar todos os Cadastros";
            this.chkTodos.Size = new System.Drawing.Size(163, 19);
            this.chkTodos.TabIndex = 0;
            this.chkTodos.CheckedChanged += new System.EventHandler(this.chkTodos_CheckedChanged);
            // 
            // gcCadastros
            // 
            this.gcCadastros.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcCadastros.Controls.Add(this.txtDataFinal);
            this.gcCadastros.Controls.Add(this.labelControl3);
            this.gcCadastros.Controls.Add(this.chbOrdemBilhetes);
            this.gcCadastros.Controls.Add(this.dtMarcacao);
            this.gcCadastros.Controls.Add(this.labelControl1);
            this.gcCadastros.Controls.Add(this.chkMarcacao);
            this.gcCadastros.Controls.Add(this.chkFechamento);
            this.gcCadastros.Controls.Add(this.chkBancoHoras);
            this.gcCadastros.Controls.Add(this.chkAfastamentos);
            this.gcCadastros.Controls.Add(this.chkFuncionario);
            this.gcCadastros.Controls.Add(this.chkHorario);
            this.gcCadastros.Controls.Add(this.chkOcorrencia);
            this.gcCadastros.Controls.Add(this.chkFeriado);
            this.gcCadastros.Controls.Add(this.chkFuncao);
            this.gcCadastros.Controls.Add(this.chkDepartamento);
            this.gcCadastros.Location = new System.Drawing.Point(24, 140);
            this.gcCadastros.Name = "gcCadastros";
            this.gcCadastros.Size = new System.Drawing.Size(570, 139);
            this.gcCadastros.TabIndex = 1;
            this.gcCadastros.Text = "Cadastros para Importação";
            // 
            // txtDataFinal
            // 
            this.txtDataFinal.EditValue = null;
            this.txtDataFinal.Location = new System.Drawing.Point(452, 57);
            this.txtDataFinal.Name = "txtDataFinal";
            this.txtDataFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataFinal.Size = new System.Drawing.Size(100, 20);
            this.txtDataFinal.TabIndex = 14;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(430, 60);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(16, 13);
            this.labelControl3.TabIndex = 13;
            this.labelControl3.Text = "até";
            // 
            // chbOrdemBilhetes
            // 
            this.chbOrdemBilhetes.Location = new System.Drawing.Point(435, 107);
            this.chbOrdemBilhetes.Name = "chbOrdemBilhetes";
            this.chbOrdemBilhetes.Properties.Caption = "Atribui Ordem Bilhetes";
            this.chbOrdemBilhetes.Size = new System.Drawing.Size(130, 19);
            this.chbOrdemBilhetes.TabIndex = 12;
            this.chbOrdemBilhetes.Visible = false;
            // 
            // dtMarcacao
            // 
            this.dtMarcacao.EditValue = null;
            this.dtMarcacao.Location = new System.Drawing.Point(452, 32);
            this.dtMarcacao.Name = "dtMarcacao";
            this.dtMarcacao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtMarcacao.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dtMarcacao.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtMarcacao.Size = new System.Drawing.Size(100, 20);
            this.dtMarcacao.TabIndex = 10;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(396, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(50, 13);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "a partir de";
            // 
            // chkMarcacao
            // 
            this.chkMarcacao.Location = new System.Drawing.Point(307, 32);
            this.chkMarcacao.Name = "chkMarcacao";
            this.chkMarcacao.Properties.Caption = "Marcações";
            this.chkMarcacao.Size = new System.Drawing.Size(83, 19);
            this.chkMarcacao.TabIndex = 8;
            this.chkMarcacao.CheckedChanged += new System.EventHandler(this.chkMarcacao_CheckedChanged);
            // 
            // chkFechamento
            // 
            this.chkFechamento.Location = new System.Drawing.Point(307, 57);
            this.chkFechamento.Name = "chkFechamento";
            this.chkFechamento.Properties.Caption = "Fechamento BH";
            this.chkFechamento.Size = new System.Drawing.Size(108, 19);
            this.chkFechamento.TabIndex = 11;
            // 
            // chkBancoHoras
            // 
            this.chkBancoHoras.Location = new System.Drawing.Point(167, 107);
            this.chkBancoHoras.Name = "chkBancoHoras";
            this.chkBancoHoras.Properties.Caption = "Banco Horas";
            this.chkBancoHoras.Size = new System.Drawing.Size(90, 19);
            this.chkBancoHoras.TabIndex = 7;
            // 
            // chkAfastamentos
            // 
            this.chkAfastamentos.Location = new System.Drawing.Point(167, 82);
            this.chkAfastamentos.Name = "chkAfastamentos";
            this.chkAfastamentos.Properties.Caption = "Afastamentos";
            this.chkAfastamentos.Size = new System.Drawing.Size(90, 19);
            this.chkAfastamentos.TabIndex = 6;
            // 
            // chkFuncionario
            // 
            this.chkFuncionario.Location = new System.Drawing.Point(167, 57);
            this.chkFuncionario.Name = "chkFuncionario";
            this.chkFuncionario.Properties.Caption = "Funcionários";
            this.chkFuncionario.Size = new System.Drawing.Size(90, 19);
            this.chkFuncionario.TabIndex = 5;
            // 
            // chkHorario
            // 
            this.chkHorario.Location = new System.Drawing.Point(167, 32);
            this.chkHorario.Name = "chkHorario";
            this.chkHorario.Properties.Caption = "Horários";
            this.chkHorario.Size = new System.Drawing.Size(68, 19);
            this.chkHorario.TabIndex = 4;
            // 
            // chkOcorrencia
            // 
            this.chkOcorrencia.Location = new System.Drawing.Point(19, 107);
            this.chkOcorrencia.Name = "chkOcorrencia";
            this.chkOcorrencia.Properties.Caption = "Ocorrencias";
            this.chkOcorrencia.Size = new System.Drawing.Size(87, 19);
            this.chkOcorrencia.TabIndex = 3;
            // 
            // chkFeriado
            // 
            this.chkFeriado.Location = new System.Drawing.Point(19, 82);
            this.chkFeriado.Name = "chkFeriado";
            this.chkFeriado.Properties.Caption = "Feriado";
            this.chkFeriado.Size = new System.Drawing.Size(66, 19);
            this.chkFeriado.TabIndex = 2;
            // 
            // chkFuncao
            // 
            this.chkFuncao.Location = new System.Drawing.Point(19, 57);
            this.chkFuncao.Name = "chkFuncao";
            this.chkFuncao.Properties.Caption = "Função";
            this.chkFuncao.Size = new System.Drawing.Size(66, 19);
            this.chkFuncao.TabIndex = 1;
            // 
            // chkDepartamento
            // 
            this.chkDepartamento.Location = new System.Drawing.Point(19, 32);
            this.chkDepartamento.Name = "chkDepartamento";
            this.chkDepartamento.Properties.Caption = "Departamento";
            this.chkDepartamento.Size = new System.Drawing.Size(100, 19);
            this.chkDepartamento.TabIndex = 0;
            // 
            // pbTabela
            // 
            this.pbTabela.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTabela.Location = new System.Drawing.Point(24, 323);
            this.pbTabela.Name = "pbTabela";
            this.pbTabela.Properties.ShowTitle = true;
            this.pbTabela.Size = new System.Drawing.Size(570, 27);
            this.pbTabela.TabIndex = 3;
            // 
            // pbGeral
            // 
            this.pbGeral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbGeral.Location = new System.Drawing.Point(24, 290);
            this.pbGeral.Name = "pbGeral";
            this.pbGeral.Properties.ShowTitle = true;
            this.pbGeral.Size = new System.Drawing.Size(570, 27);
            this.pbGeral.TabIndex = 2;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(620, 366);
            this.xtraTabPage1.Text = "xtraTabPage1";
            // 
            // sbFechar
            // 
            this.sbFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbFechar.Image = ((System.Drawing.Image)(resources.GetObject("sbFechar.Image")));
            this.sbFechar.Location = new System.Drawing.Point(557, 390);
            this.sbFechar.Name = "sbFechar";
            this.sbFechar.Size = new System.Drawing.Size(75, 23);
            this.sbFechar.TabIndex = 2;
            this.sbFechar.Text = "&Fechar";
            this.sbFechar.Click += new System.EventHandler(this.sbFechar_Click);
            // 
            // sbImportar
            // 
            this.sbImportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbImportar.Image = ((System.Drawing.Image)(resources.GetObject("sbImportar.Image")));
            this.sbImportar.Location = new System.Drawing.Point(476, 390);
            this.sbImportar.Name = "sbImportar";
            this.sbImportar.Size = new System.Drawing.Size(75, 23);
            this.sbImportar.TabIndex = 1;
            this.sbImportar.Text = "&Importar";
            this.sbImportar.Click += new System.EventHandler(this.sbImportar_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // sbAjuda
            // 
            this.sbAjuda.Image = ((System.Drawing.Image)(resources.GetObject("sbAjuda.Image")));
            this.sbAjuda.Location = new System.Drawing.Point(12, 389);
            this.sbAjuda.Name = "sbAjuda";
            this.sbAjuda.Size = new System.Drawing.Size(75, 23);
            this.sbAjuda.TabIndex = 3;
            this.sbAjuda.Text = "A&juda";
            this.sbAjuda.Click += new System.EventHandler(this.btAjuda_Click);
            // 
            // FormImportacaoTopPonto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 425);
            this.Controls.Add(this.sbAjuda);
            this.Controls.Add(this.sbImportar);
            this.Controls.Add(this.sbFechar);
            this.Controls.Add(this.xtraTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImportacaoTopPonto";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormImportacaoTopPonto";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormImportacaoTopPonto_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcParametros)).EndInit();
            this.gcParametros.ResumeLayout(false);
            this.gcParametros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArquivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTodos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCadastros)).EndInit();
            this.gcCadastros.ResumeLayout(false);
            this.gcCadastros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbOrdemBilhetes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMarcacao.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMarcacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkMarcacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFechamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkBancoHoras.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkAfastamentos.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFuncionario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkHorario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkOcorrencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFeriado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFuncao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTabela.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGeral.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraEditors.SimpleButton sbFechar;
        private DevExpress.XtraEditors.SimpleButton sbImportar;
        private DevExpress.XtraEditors.ProgressBarControl pbGeral;
        private DevExpress.XtraEditors.ProgressBarControl pbTabela;
        private DevExpress.XtraEditors.CheckEdit chkTodos;
        private DevExpress.XtraEditors.GroupControl gcCadastros;
        private DevExpress.XtraEditors.CheckEdit chkFuncionario;
        private DevExpress.XtraEditors.CheckEdit chkHorario;
        private DevExpress.XtraEditors.CheckEdit chkOcorrencia;
        private DevExpress.XtraEditors.CheckEdit chkFeriado;
        private DevExpress.XtraEditors.CheckEdit chkFuncao;
        private DevExpress.XtraEditors.CheckEdit chkDepartamento;
        private DevExpress.XtraEditors.CheckEdit chkAfastamentos;
        private DevExpress.XtraEditors.CheckEdit chkBancoHoras;
        private DevExpress.XtraEditors.CheckEdit chkFechamento;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chkMarcacao;
        private DevExpress.XtraEditors.DateEdit dtMarcacao;
        private DevExpress.XtraEditors.GroupControl gcParametros;
        private DevExpress.XtraEditors.TextEdit txtArquivo;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Componentes.devexpress.cwk_DevButton BotaoDiretorio;
        private DevExpress.XtraEditors.CheckEdit chbOrdemBilhetes;
        private DevExpress.XtraEditors.SimpleButton sbAjuda;
        private DevExpress.XtraEditors.RadioGroup radioGroup1;
        private DevExpress.XtraEditors.DateEdit txtDataFinal;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}