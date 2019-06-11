namespace Secullum
{
    partial class FormImportacaoSecullum
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportacaoSecullum));
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.gcCadastros = new DevExpress.XtraEditors.GroupControl();
            this.cbIdEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.lblEmpresa = new DevExpress.XtraEditors.LabelControl();
            this.sbIdTurnoNormal = new Componentes.devexpress.cwk_DevButton();
            this.cbIdTurnoNormal = new Componentes.devexpress.cwk_DevLookup();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.rgTipoHorario = new DevExpress.XtraEditors.RadioGroup();
            this.BotaoDiretorio = new Componentes.devexpress.cwk_DevButton();
            this.txtArquivo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtDataFinal = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.dtMarcacao = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.chkFuncionario = new DevExpress.XtraEditors.CheckEdit();
            this.chkFuncao = new DevExpress.XtraEditors.CheckEdit();
            this.chkDepartamento = new DevExpress.XtraEditors.CheckEdit();
            this.pbTabela = new DevExpress.XtraEditors.ProgressBarControl();
            this.pbGeral = new DevExpress.XtraEditors.ProgressBarControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.sbFechar = new DevExpress.XtraEditors.SimpleButton();
            this.sbImportar = new DevExpress.XtraEditors.SimpleButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sbAjuda = new DevExpress.XtraEditors.SimpleButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCadastros)).BeginInit();
            this.gcCadastros.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdTurnoNormal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoHorario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArquivo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMarcacao.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMarcacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFuncionario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFuncao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTabela.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGeral.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
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
            this.xtraTabControl1.Size = new System.Drawing.Size(626, 361);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.gcCadastros);
            this.xtraTabPage2.Controls.Add(this.pbTabela);
            this.xtraTabPage2.Controls.Add(this.pbGeral);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(620, 355);
            this.xtraTabPage2.Text = "xtraTabPage2";
            // 
            // gcCadastros
            // 
            this.gcCadastros.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gcCadastros.Controls.Add(this.cbIdEmpresa);
            this.gcCadastros.Controls.Add(this.sbIdEmpresa);
            this.gcCadastros.Controls.Add(this.lblEmpresa);
            this.gcCadastros.Controls.Add(this.sbIdTurnoNormal);
            this.gcCadastros.Controls.Add(this.cbIdTurnoNormal);
            this.gcCadastros.Controls.Add(this.labelControl4);
            this.gcCadastros.Controls.Add(this.groupControl2);
            this.gcCadastros.Controls.Add(this.BotaoDiretorio);
            this.gcCadastros.Controls.Add(this.txtArquivo);
            this.gcCadastros.Controls.Add(this.labelControl2);
            this.gcCadastros.Controls.Add(this.txtDataFinal);
            this.gcCadastros.Controls.Add(this.labelControl3);
            this.gcCadastros.Controls.Add(this.dtMarcacao);
            this.gcCadastros.Controls.Add(this.labelControl1);
            this.gcCadastros.Controls.Add(this.chkFuncionario);
            this.gcCadastros.Controls.Add(this.chkFuncao);
            this.gcCadastros.Controls.Add(this.chkDepartamento);
            this.gcCadastros.Location = new System.Drawing.Point(25, 17);
            this.gcCadastros.Name = "gcCadastros";
            this.gcCadastros.Size = new System.Drawing.Size(570, 244);
            this.gcCadastros.TabIndex = 1;
            this.gcCadastros.Text = "Parâmetros para Importação";
            // 
            // cbIdEmpresa
            // 
            this.cbIdEmpresa.ButtonLookup = this.sbIdEmpresa;
            this.cbIdEmpresa.EditValue = 0;
            this.cbIdEmpresa.Key = System.Windows.Forms.Keys.F5;
            this.cbIdEmpresa.Location = new System.Drawing.Point(62, 24);
            this.cbIdEmpresa.Name = "cbIdEmpresa";
            this.cbIdEmpresa.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.cbIdEmpresa.Properties.Appearance.Options.UseForeColor = true;
            this.cbIdEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("cnpj_cpf", "CNPJ/CPF")});
            this.cbIdEmpresa.Properties.DisplayMember = "nome";
            this.cbIdEmpresa.Properties.NullText = "";
            this.cbIdEmpresa.Properties.ValueMember = "id";
            this.cbIdEmpresa.Size = new System.Drawing.Size(470, 20);
            this.cbIdEmpresa.TabIndex = 3;
            // 
            // sbIdEmpresa
            // 
            this.sbIdEmpresa.Location = new System.Drawing.Point(538, 24);
            this.sbIdEmpresa.Name = "sbIdEmpresa";
            this.sbIdEmpresa.Size = new System.Drawing.Size(24, 20);
            this.sbIdEmpresa.TabIndex = 4;
            this.sbIdEmpresa.TabStop = false;
            this.sbIdEmpresa.Text = "...";
            this.sbIdEmpresa.Click += new System.EventHandler(this.sbIdEmpresa_Click);
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.Location = new System.Drawing.Point(11, 27);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(45, 13);
            this.lblEmpresa.TabIndex = 2;
            this.lblEmpresa.Text = "Empresa:";
            // 
            // sbIdTurnoNormal
            // 
            this.sbIdTurnoNormal.Enabled = false;
            this.sbIdTurnoNormal.Location = new System.Drawing.Point(538, 76);
            this.sbIdTurnoNormal.Name = "sbIdTurnoNormal";
            this.sbIdTurnoNormal.Size = new System.Drawing.Size(24, 20);
            this.sbIdTurnoNormal.TabIndex = 12;
            this.sbIdTurnoNormal.TabStop = false;
            this.sbIdTurnoNormal.Text = "...";
            this.sbIdTurnoNormal.Click += new System.EventHandler(this.sbIdTurnoNormal_Click);
            // 
            // cbIdTurnoNormal
            // 
            this.cbIdTurnoNormal.ButtonLookup = this.sbIdTurnoNormal;
            this.cbIdTurnoNormal.EditValue = 0;
            this.cbIdTurnoNormal.Enabled = false;
            this.cbIdTurnoNormal.Key = System.Windows.Forms.Keys.F5;
            this.cbIdTurnoNormal.Location = new System.Drawing.Point(168, 76);
            this.cbIdTurnoNormal.Name = "cbIdTurnoNormal";
            this.cbIdTurnoNormal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdTurnoNormal.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código")});
            this.cbIdTurnoNormal.Properties.DisplayMember = "descricao";
            this.cbIdTurnoNormal.Properties.NullText = "";
            this.cbIdTurnoNormal.Properties.ValueMember = "id";
            this.cbIdTurnoNormal.Size = new System.Drawing.Size(364, 20);
            this.cbIdTurnoNormal.TabIndex = 11;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(130, 79);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(32, 13);
            this.labelControl4.TabIndex = 10;
            this.labelControl4.Text = "Turno:";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.rgTipoHorario);
            this.groupControl2.Location = new System.Drawing.Point(11, 76);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(109, 81);
            this.groupControl2.TabIndex = 8;
            this.groupControl2.Text = "Tipo Turno";
            // 
            // rgTipoHorario
            // 
            this.rgTipoHorario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTipoHorario.Location = new System.Drawing.Point(2, 21);
            this.rgTipoHorario.Name = "rgTipoHorario";
            this.rgTipoHorario.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Turno Normal"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Turno Móvel")});
            this.rgTipoHorario.Size = new System.Drawing.Size(105, 58);
            this.rgTipoHorario.TabIndex = 9;
            this.rgTipoHorario.SelectedIndexChanged += new System.EventHandler(this.rgTipoHorario_SelectedIndexChanged);
            // 
            // BotaoDiretorio
            // 
            this.BotaoDiretorio.Location = new System.Drawing.Point(538, 50);
            this.BotaoDiretorio.Name = "BotaoDiretorio";
            this.BotaoDiretorio.Size = new System.Drawing.Size(24, 20);
            this.BotaoDiretorio.TabIndex = 7;
            this.BotaoDiretorio.TabStop = false;
            this.BotaoDiretorio.Text = "...";
            this.BotaoDiretorio.Click += new System.EventHandler(this.BotaoDiretorio_Click);
            // 
            // txtArquivo
            // 
            this.txtArquivo.Location = new System.Drawing.Point(62, 50);
            this.txtArquivo.Name = "txtArquivo";
            this.txtArquivo.Properties.ReadOnly = true;
            this.txtArquivo.Size = new System.Drawing.Size(470, 20);
            this.txtArquivo.TabIndex = 6;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(15, 53);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(41, 13);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Arquivo:";
            // 
            // txtDataFinal
            // 
            this.txtDataFinal.EditValue = null;
            this.txtDataFinal.Location = new System.Drawing.Point(462, 128);
            this.txtDataFinal.Name = "txtDataFinal";
            this.txtDataFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataFinal.Size = new System.Drawing.Size(100, 20);
            this.txtDataFinal.TabIndex = 16;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(439, 131);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(16, 13);
            this.labelControl3.TabIndex = 15;
            this.labelControl3.Text = "até";
            // 
            // dtMarcacao
            // 
            this.dtMarcacao.EditValue = null;
            this.dtMarcacao.Location = new System.Drawing.Point(462, 102);
            this.dtMarcacao.Name = "dtMarcacao";
            this.dtMarcacao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtMarcacao.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dtMarcacao.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtMarcacao.Size = new System.Drawing.Size(100, 20);
            this.dtMarcacao.TabIndex = 14;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(406, 105);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(50, 13);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "a partir de";
            // 
            // chkFuncionario
            // 
            this.chkFuncionario.EditValue = true;
            this.chkFuncionario.Location = new System.Drawing.Point(15, 210);
            this.chkFuncionario.Name = "chkFuncionario";
            this.chkFuncionario.Properties.Caption = "Funcionários";
            this.chkFuncionario.Properties.ReadOnly = true;
            this.chkFuncionario.Size = new System.Drawing.Size(90, 19);
            this.chkFuncionario.TabIndex = 19;
            // 
            // chkFuncao
            // 
            this.chkFuncao.EditValue = true;
            this.chkFuncao.Location = new System.Drawing.Point(15, 187);
            this.chkFuncao.Name = "chkFuncao";
            this.chkFuncao.Properties.Caption = "Função";
            this.chkFuncao.Properties.ReadOnly = true;
            this.chkFuncao.Size = new System.Drawing.Size(66, 19);
            this.chkFuncao.TabIndex = 18;
            // 
            // chkDepartamento
            // 
            this.chkDepartamento.EditValue = true;
            this.chkDepartamento.Location = new System.Drawing.Point(15, 163);
            this.chkDepartamento.Name = "chkDepartamento";
            this.chkDepartamento.Properties.Caption = "Departamento";
            this.chkDepartamento.Properties.ReadOnly = true;
            this.chkDepartamento.Size = new System.Drawing.Size(100, 19);
            this.chkDepartamento.TabIndex = 17;
            // 
            // pbTabela
            // 
            this.pbTabela.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbTabela.Location = new System.Drawing.Point(25, 313);
            this.pbTabela.Name = "pbTabela";
            this.pbTabela.Properties.ShowTitle = true;
            this.pbTabela.Size = new System.Drawing.Size(570, 27);
            this.pbTabela.TabIndex = 3;
            // 
            // pbGeral
            // 
            this.pbGeral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbGeral.Location = new System.Drawing.Point(25, 280);
            this.pbGeral.Name = "pbGeral";
            this.pbGeral.Properties.ShowTitle = true;
            this.pbGeral.Size = new System.Drawing.Size(570, 27);
            this.pbGeral.TabIndex = 2;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(620, 355);
            this.xtraTabPage1.Text = "xtraTabPage1";
            // 
            // sbFechar
            // 
            this.sbFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbFechar.Image = ((System.Drawing.Image)(resources.GetObject("sbFechar.Image")));
            this.sbFechar.Location = new System.Drawing.Point(557, 379);
            this.sbFechar.Name = "sbFechar";
            this.sbFechar.Size = new System.Drawing.Size(75, 23);
            this.sbFechar.TabIndex = 22;
            this.sbFechar.Text = "&Fechar";
            this.sbFechar.Click += new System.EventHandler(this.sbFechar_Click);
            // 
            // sbImportar
            // 
            this.sbImportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbImportar.Image = ((System.Drawing.Image)(resources.GetObject("sbImportar.Image")));
            this.sbImportar.Location = new System.Drawing.Point(476, 379);
            this.sbImportar.Name = "sbImportar";
            this.sbImportar.Size = new System.Drawing.Size(75, 23);
            this.sbImportar.TabIndex = 21;
            this.sbImportar.Text = "&Importar";
            this.sbImportar.Click += new System.EventHandler(this.sbImportar_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // sbAjuda
            // 
            this.sbAjuda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sbAjuda.Image = ((System.Drawing.Image)(resources.GetObject("sbAjuda.Image")));
            this.sbAjuda.Location = new System.Drawing.Point(19, 379);
            this.sbAjuda.Name = "sbAjuda";
            this.sbAjuda.Size = new System.Drawing.Size(75, 23);
            this.sbAjuda.TabIndex = 20;
            this.sbAjuda.Text = "A&juda";
            this.sbAjuda.Click += new System.EventHandler(this.btAjuda_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // FormImportacaoSecullum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 414);
            this.Controls.Add(this.sbAjuda);
            this.Controls.Add(this.sbImportar);
            this.Controls.Add(this.sbFechar);
            this.Controls.Add(this.xtraTabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormImportacaoSecullum";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormImportacaoTopPonto";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormImportacaoTopPonto_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCadastros)).EndInit();
            this.gcCadastros.ResumeLayout(false);
            this.gcCadastros.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdTurnoNormal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipoHorario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArquivo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMarcacao.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtMarcacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFuncionario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkFuncao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbTabela.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGeral.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
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
        private DevExpress.XtraEditors.GroupControl gcCadastros;
        private DevExpress.XtraEditors.CheckEdit chkFuncionario;
        private DevExpress.XtraEditors.CheckEdit chkFuncao;
        private DevExpress.XtraEditors.CheckEdit chkDepartamento;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dtMarcacao;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevExpress.XtraEditors.SimpleButton sbAjuda;
        private DevExpress.XtraEditors.DateEdit txtDataFinal;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private Componentes.devexpress.cwk_DevButton BotaoDiretorio;
        private DevExpress.XtraEditors.TextEdit txtArquivo;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private Componentes.devexpress.cwk_DevButton sbIdTurnoNormal;
        private Componentes.devexpress.cwk_DevLookup cbIdTurnoNormal;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.RadioGroup rgTipoHorario;
        private Componentes.devexpress.cwk_DevButton sbIdEmpresa;
        private DevExpress.XtraEditors.LabelControl lblEmpresa;
        private Componentes.devexpress.cwk_DevLookup cbIdEmpresa;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
    }
}