namespace cwkComunicadorWebAPIPontoWeb.Forms
{
    partial class FormConfiguracoesDataHora
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConfiguracoesDataHora));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gcImportacaoes = new DevExpress.XtraGrid.GridControl();
            this.bandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gcCodigo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcNumRel = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcLocalRel = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcModeloRel = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcEnviaDataHoraServidor = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gcEnviaHorarioVerao = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcDtInicioHorarioVerao = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcDtFimHorarioVerao = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gcIncDataHora = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcUsuInclusao = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.lkpTipoImportacao = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.chbAtivarImportacao = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.seTempoImportacao = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.pnlBotoes = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.sbEnviarDadosRep = new DevExpress.XtraEditors.SimpleButton();
            this.sbFechar = new DevExpress.XtraEditors.SimpleButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbInfoEnviarConfig = new System.Windows.Forms.Label();
            this.gbEnviaConfigDataHora = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gbHorarioVerao = new System.Windows.Forms.GroupBox();
            this.dtPickerTermino = new DevExpress.XtraEditors.DateEdit();
            this.dtPickerInicio = new DevExpress.XtraEditors.DateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gbDataHora = new System.Windows.Forms.GroupBox();
            this.dtPickerDataHora = new DevExpress.XtraEditors.DateEdit();
            this.ckbDataHoraComputador = new System.Windows.Forms.CheckBox();
            this.rbDataHora = new System.Windows.Forms.RadioButton();
            this.rbHorarioVerao = new System.Windows.Forms.RadioButton();
            this.lbRelogio = new System.Windows.Forms.Label();
            this.ckbRelogio = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcImportacaoes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipoImportacao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAtivarImportacao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seTempoImportacao)).BeginInit();
            this.pnlBotoes.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gbEnviaConfigDataHora.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gbHorarioVerao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerTermino.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerTermino.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerInicio.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerInicio.Properties)).BeginInit();
            this.gbDataHora.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerDataHora.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerDataHora.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(898, 277);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gcImportacaoes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(878, 257);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configurações de Data e Hora a Serem Enviadas";
            // 
            // gcImportacaoes
            // 
            this.gcImportacaoes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcImportacaoes.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcImportacaoes.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcImportacaoes.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcImportacaoes.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcImportacaoes.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcImportacaoes.Location = new System.Drawing.Point(3, 16);
            this.gcImportacaoes.MainView = this.bandedGridView1;
            this.gcImportacaoes.Name = "gcImportacaoes";
            this.gcImportacaoes.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkpTipoImportacao,
            this.chbAtivarImportacao,
            this.seTempoImportacao});
            this.gcImportacaoes.Size = new System.Drawing.Size(872, 238);
            this.gcImportacaoes.TabIndex = 31;
            this.gcImportacaoes.UseEmbeddedNavigator = true;
            this.gcImportacaoes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView1});
            // 
            // bandedGridView1
            // 
            this.bandedGridView1.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.bandedGridView1.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.bandedGridView1.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.Empty.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.EvenRow.Options.UseBorderColor = true;
            this.bandedGridView1.Appearance.EvenRow.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.bandedGridView1.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.bandedGridView1.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.bandedGridView1.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.bandedGridView1.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.FilterPanel.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.FilterPanel.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.FixedLine.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.RoyalBlue;
            this.bandedGridView1.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.FocusedCell.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.bandedGridView1.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.FocusedRow.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.FocusedRow.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.FooterPanel.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.bandedGridView1.Appearance.FooterPanel.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.GroupButton.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.GroupButton.Options.UseBorderColor = true;
            this.bandedGridView1.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.GroupFooter.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.bandedGridView1.Appearance.GroupFooter.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.bandedGridView1.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.GroupPanel.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.GroupPanel.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.GroupRow.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.bandedGridView1.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.bandedGridView1.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.bandedGridView1.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.bandedGridView1.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.OddRow.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.OddRow.Options.UseBorderColor = true;
            this.bandedGridView1.Appearance.OddRow.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.bandedGridView1.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.Preview.Options.UseFont = true;
            this.bandedGridView1.Appearance.Preview.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.Row.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.Row.Options.UseTextOptions = true;
            this.bandedGridView1.Appearance.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.bandedGridView1.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.RowSeparator.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.SelectedRow.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.TopNewRow.Options.UseBackColor = true;
            this.bandedGridView1.AppearancePrint.Row.Options.UseTextOptions = true;
            this.bandedGridView1.AppearancePrint.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.bandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2,
            this.gridBand3});
            this.bandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.gcCodigo,
            this.gcNumRel,
            this.gcLocalRel,
            this.gcModeloRel,
            this.gcEnviaDataHoraServidor,
            this.gcEnviaHorarioVerao,
            this.gcDtInicioHorarioVerao,
            this.gcDtFimHorarioVerao,
            this.gcIncDataHora,
            this.gcUsuInclusao});
            this.bandedGridView1.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.bandedGridView1.GridControl = this.gcImportacaoes;
            this.bandedGridView1.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.bandedGridView1.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.bandedGridView1.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.bandedGridView1.Name = "bandedGridView1";
            this.bandedGridView1.OptionsBehavior.FocusLeaveOnTab = true;
            this.bandedGridView1.OptionsCustomization.AllowGroup = false;
            this.bandedGridView1.OptionsDetail.EnableMasterViewMode = false;
            this.bandedGridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.bandedGridView1.OptionsView.ColumnAutoWidth = false;
            this.bandedGridView1.OptionsView.EnableAppearanceEvenRow = true;
            this.bandedGridView1.OptionsView.EnableAppearanceOddRow = true;
            this.bandedGridView1.OptionsView.RowAutoHeight = true;
            this.bandedGridView1.OptionsView.ShowAutoFilterRow = true;
            this.bandedGridView1.OptionsView.ShowFooter = true;
            this.bandedGridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand1.Caption = "Rep";
            this.gridBand1.Columns.Add(this.gcCodigo);
            this.gridBand1.Columns.Add(this.gcNumRel);
            this.gridBand1.Columns.Add(this.gcLocalRel);
            this.gridBand1.Columns.Add(this.gcModeloRel);
            this.gridBand1.Columns.Add(this.gcEnviaDataHoraServidor);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.Width = 617;
            // 
            // gcCodigo
            // 
            this.gcCodigo.AppearanceCell.Options.UseTextOptions = true;
            this.gcCodigo.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gcCodigo.AppearanceHeader.Options.UseTextOptions = true;
            this.gcCodigo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcCodigo.Caption = "Código";
            this.gcCodigo.FieldName = "Codigo";
            this.gcCodigo.Name = "gcCodigo";
            this.gcCodigo.OptionsColumn.AllowEdit = false;
            this.gcCodigo.OptionsColumn.ReadOnly = true;
            this.gcCodigo.Visible = true;
            this.gcCodigo.Width = 66;
            // 
            // gcNumRel
            // 
            this.gcNumRel.AppearanceHeader.Options.UseTextOptions = true;
            this.gcNumRel.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcNumRel.Caption = "Núm. Relógio";
            this.gcNumRel.FieldName = "RepVM.NumRelogio";
            this.gcNumRel.Name = "gcNumRel";
            this.gcNumRel.OptionsColumn.AllowEdit = false;
            this.gcNumRel.OptionsColumn.ReadOnly = true;
            this.gcNumRel.Visible = true;
            this.gcNumRel.Width = 71;
            // 
            // gcLocalRel
            // 
            this.gcLocalRel.AppearanceHeader.Options.UseTextOptions = true;
            this.gcLocalRel.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcLocalRel.Caption = "Local Relógio";
            this.gcLocalRel.FieldName = "RepVM.EnderecoEmpregador";
            this.gcLocalRel.Name = "gcLocalRel";
            this.gcLocalRel.OptionsColumn.AllowEdit = false;
            this.gcLocalRel.OptionsColumn.ReadOnly = true;
            this.gcLocalRel.Visible = true;
            this.gcLocalRel.Width = 189;
            // 
            // gcModeloRel
            // 
            this.gcModeloRel.AppearanceHeader.Options.UseTextOptions = true;
            this.gcModeloRel.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcModeloRel.Caption = "Modelo Relógio";
            this.gcModeloRel.FieldName = "RepVM.NomeModelo";
            this.gcModeloRel.Name = "gcModeloRel";
            this.gcModeloRel.OptionsColumn.AllowEdit = false;
            this.gcModeloRel.OptionsColumn.ReadOnly = true;
            this.gcModeloRel.Visible = true;
            this.gcModeloRel.Width = 185;
            // 
            // gcEnviaDataHoraServidor
            // 
            this.gcEnviaDataHoraServidor.AppearanceHeader.Options.UseTextOptions = true;
            this.gcEnviaDataHoraServidor.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcEnviaDataHoraServidor.Caption = "Enviar Data/Hora";
            this.gcEnviaDataHoraServidor.FieldName = "EnviaDataHoraServidor";
            this.gcEnviaDataHoraServidor.Name = "gcEnviaDataHoraServidor";
            this.gcEnviaDataHoraServidor.OptionsColumn.AllowEdit = false;
            this.gcEnviaDataHoraServidor.OptionsColumn.ReadOnly = true;
            this.gcEnviaDataHoraServidor.Visible = true;
            this.gcEnviaDataHoraServidor.Width = 106;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "Horário de Verão";
            this.gridBand2.Columns.Add(this.gcEnviaHorarioVerao);
            this.gridBand2.Columns.Add(this.gcDtInicioHorarioVerao);
            this.gridBand2.Columns.Add(this.gcDtFimHorarioVerao);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.Width = 227;
            // 
            // gcEnviaHorarioVerao
            // 
            this.gcEnviaHorarioVerao.AppearanceHeader.Options.UseTextOptions = true;
            this.gcEnviaHorarioVerao.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcEnviaHorarioVerao.Caption = "Enviar";
            this.gcEnviaHorarioVerao.FieldName = "EnviaHorarioVerao";
            this.gcEnviaHorarioVerao.Name = "gcEnviaHorarioVerao";
            this.gcEnviaHorarioVerao.OptionsColumn.AllowEdit = false;
            this.gcEnviaHorarioVerao.OptionsColumn.ReadOnly = true;
            this.gcEnviaHorarioVerao.Visible = true;
            this.gcEnviaHorarioVerao.Width = 57;
            // 
            // gcDtInicioHorarioVerao
            // 
            this.gcDtInicioHorarioVerao.AppearanceHeader.Options.UseTextOptions = true;
            this.gcDtInicioHorarioVerao.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcDtInicioHorarioVerao.Caption = "Data Inícial";
            this.gcDtInicioHorarioVerao.FieldName = "dtInicioHorarioVerao";
            this.gcDtInicioHorarioVerao.Name = "gcDtInicioHorarioVerao";
            this.gcDtInicioHorarioVerao.OptionsColumn.AllowEdit = false;
            this.gcDtInicioHorarioVerao.OptionsColumn.ReadOnly = true;
            this.gcDtInicioHorarioVerao.Visible = true;
            this.gcDtInicioHorarioVerao.Width = 93;
            // 
            // gcDtFimHorarioVerao
            // 
            this.gcDtFimHorarioVerao.AppearanceHeader.Options.UseTextOptions = true;
            this.gcDtFimHorarioVerao.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcDtFimHorarioVerao.Caption = "Data Final";
            this.gcDtFimHorarioVerao.FieldName = "dtFimHorarioVerao";
            this.gcDtFimHorarioVerao.Name = "gcDtFimHorarioVerao";
            this.gcDtFimHorarioVerao.OptionsColumn.AllowEdit = false;
            this.gcDtFimHorarioVerao.OptionsColumn.ReadOnly = true;
            this.gcDtFimHorarioVerao.Visible = true;
            this.gcDtFimHorarioVerao.Width = 77;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.Caption = "Inclusão";
            this.gridBand3.Columns.Add(this.gcIncDataHora);
            this.gridBand3.Columns.Add(this.gcUsuInclusao);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.Width = 240;
            // 
            // gcIncDataHora
            // 
            this.gcIncDataHora.AppearanceCell.Options.UseTextOptions = true;
            this.gcIncDataHora.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gcIncDataHora.AppearanceHeader.Options.UseTextOptions = true;
            this.gcIncDataHora.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcIncDataHora.Caption = "Data/Hora";
            this.gcIncDataHora.DisplayFormat.FormatString = "g";
            this.gcIncDataHora.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gcIncDataHora.FieldName = "Inchora";
            this.gcIncDataHora.Name = "gcIncDataHora";
            this.gcIncDataHora.OptionsColumn.AllowEdit = false;
            this.gcIncDataHora.OptionsColumn.ReadOnly = true;
            this.gcIncDataHora.Visible = true;
            this.gcIncDataHora.Width = 105;
            // 
            // gcUsuInclusao
            // 
            this.gcUsuInclusao.AppearanceHeader.Options.UseTextOptions = true;
            this.gcUsuInclusao.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcUsuInclusao.Caption = "Usuário";
            this.gcUsuInclusao.FieldName = "Incusuario";
            this.gcUsuInclusao.Name = "gcUsuInclusao";
            this.gcUsuInclusao.OptionsColumn.AllowEdit = false;
            this.gcUsuInclusao.OptionsColumn.ReadOnly = true;
            this.gcUsuInclusao.Visible = true;
            this.gcUsuInclusao.Width = 135;
            // 
            // lkpTipoImportacao
            // 
            this.lkpTipoImportacao.AutoHeight = false;
            this.lkpTipoImportacao.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpTipoImportacao.Name = "lkpTipoImportacao";
            this.lkpTipoImportacao.NullText = "[Selecione um Valor]";
            this.lkpTipoImportacao.NullValuePrompt = "[Selecione um Valor]";
            this.lkpTipoImportacao.NullValuePromptShowForEmptyValue = true;
            // 
            // chbAtivarImportacao
            // 
            this.chbAtivarImportacao.AutoHeight = false;
            this.chbAtivarImportacao.Name = "chbAtivarImportacao";
            // 
            // seTempoImportacao
            // 
            this.seTempoImportacao.AutoHeight = false;
            this.seTempoImportacao.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.seTempoImportacao.MaxValue = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.seTempoImportacao.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.seTempoImportacao.Name = "seTempoImportacao";
            this.seTempoImportacao.NullText = "[Insira um valor]";
            this.seTempoImportacao.NullValuePrompt = "[Insira um valor]";
            this.seTempoImportacao.NullValuePromptShowForEmptyValue = true;
            // 
            // pnlBotoes
            // 
            this.pnlBotoes.Controls.Add(this.groupBox4);
            this.pnlBotoes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotoes.Location = new System.Drawing.Point(0, 481);
            this.pnlBotoes.Name = "pnlBotoes";
            this.pnlBotoes.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.pnlBotoes.Size = new System.Drawing.Size(898, 57);
            this.pnlBotoes.TabIndex = 23;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.sbEnviarDadosRep);
            this.groupBox4.Controls.Add(this.sbFechar);
            this.groupBox4.Controls.Add(this.progressBar);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(10, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(878, 47);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            // 
            // sbEnviarDadosRep
            // 
            this.sbEnviarDadosRep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbEnviarDadosRep.Image = global::cwkComunicadorWebAPIPontoWeb.Properties.Resources.Alterar_copy;
            this.sbEnviarDadosRep.ImageIndex = 6;
            this.sbEnviarDadosRep.Location = new System.Drawing.Point(716, 16);
            this.sbEnviarDadosRep.Name = "sbEnviarDadosRep";
            this.sbEnviarDadosRep.Size = new System.Drawing.Size(75, 23);
            this.sbEnviarDadosRep.TabIndex = 25;
            this.sbEnviarDadosRep.Text = "Enviar";
            this.sbEnviarDadosRep.Click += new System.EventHandler(this.sbEnviarDadosRep_Click);
            // 
            // sbFechar
            // 
            this.sbFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbFechar.Image = global::cwkComunicadorWebAPIPontoWeb.Properties.Resources.cancelar_copy;
            this.sbFechar.ImageIndex = 7;
            this.sbFechar.Location = new System.Drawing.Point(797, 16);
            this.sbFechar.Name = "sbFechar";
            this.sbFechar.Size = new System.Drawing.Size(75, 23);
            this.sbFechar.TabIndex = 26;
            this.sbFechar.Text = "&Fechar";
            this.sbFechar.Click += new System.EventHandler(this.sbFechar_Click_2);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 16);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(704, 23);
            this.progressBar.TabIndex = 24;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbInfoEnviarConfig);
            this.panel2.Controls.Add(this.gbEnviaConfigDataHora);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 277);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(898, 204);
            this.panel2.TabIndex = 24;
            // 
            // lbInfoEnviarConfig
            // 
            this.lbInfoEnviarConfig.AutoSize = true;
            this.lbInfoEnviarConfig.ForeColor = System.Drawing.Color.Red;
            this.lbInfoEnviarConfig.Location = new System.Drawing.Point(252, 9);
            this.lbInfoEnviarConfig.Name = "lbInfoEnviarConfig";
            this.lbInfoEnviarConfig.Size = new System.Drawing.Size(394, 13);
            this.lbInfoEnviarConfig.TabIndex = 30;
            this.lbInfoEnviarConfig.Text = "Opção Disponível Apenas Quando Não Houver Configurações a Serem Enviadas";
            // 
            // gbEnviaConfigDataHora
            // 
            this.gbEnviaConfigDataHora.Controls.Add(this.groupBox3);
            this.gbEnviaConfigDataHora.Controls.Add(this.lbRelogio);
            this.gbEnviaConfigDataHora.Controls.Add(this.ckbRelogio);
            this.gbEnviaConfigDataHora.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbEnviaConfigDataHora.Location = new System.Drawing.Point(10, 10);
            this.gbEnviaConfigDataHora.Name = "gbEnviaConfigDataHora";
            this.gbEnviaConfigDataHora.Size = new System.Drawing.Size(878, 184);
            this.gbEnviaConfigDataHora.TabIndex = 25;
            this.gbEnviaConfigDataHora.TabStop = false;
            this.gbEnviaConfigDataHora.Text = "Enviar Configuração de Data e Hora";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gbHorarioVerao);
            this.groupBox3.Controls.Add(this.gbDataHora);
            this.groupBox3.Controls.Add(this.rbDataHora);
            this.groupBox3.Controls.Add(this.rbHorarioVerao);
            this.groupBox3.Location = new System.Drawing.Point(36, 57);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(542, 121);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Envia";
            // 
            // gbHorarioVerao
            // 
            this.gbHorarioVerao.Controls.Add(this.dtPickerTermino);
            this.gbHorarioVerao.Controls.Add(this.dtPickerInicio);
            this.gbHorarioVerao.Controls.Add(this.label2);
            this.gbHorarioVerao.Controls.Add(this.label1);
            this.gbHorarioVerao.Location = new System.Drawing.Point(288, 36);
            this.gbHorarioVerao.Name = "gbHorarioVerao";
            this.gbHorarioVerao.Size = new System.Drawing.Size(220, 75);
            this.gbHorarioVerao.TabIndex = 3;
            this.gbHorarioVerao.TabStop = false;
            // 
            // dtPickerTermino
            // 
            this.dtPickerTermino.EditValue = null;
            this.dtPickerTermino.Location = new System.Drawing.Point(66, 46);
            this.dtPickerTermino.Name = "dtPickerTermino";
            this.dtPickerTermino.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtPickerTermino.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtPickerTermino.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtPickerTermino.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtPickerTermino.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dtPickerTermino.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtPickerTermino.Size = new System.Drawing.Size(133, 20);
            this.dtPickerTermino.TabIndex = 27;
            // 
            // dtPickerInicio
            // 
            this.dtPickerInicio.EditValue = null;
            this.dtPickerInicio.Location = new System.Drawing.Point(66, 17);
            this.dtPickerInicio.Name = "dtPickerInicio";
            this.dtPickerInicio.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtPickerInicio.Properties.DisplayFormat.FormatString = "dd/MM/yyyy";
            this.dtPickerInicio.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtPickerInicio.Properties.EditFormat.FormatString = "dd/MM/yyyy";
            this.dtPickerInicio.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtPickerInicio.Properties.Mask.EditMask = "dd/MM/yyyy";
            this.dtPickerInicio.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dtPickerInicio.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtPickerInicio.Size = new System.Drawing.Size(133, 20);
            this.dtPickerInicio.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Término";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Início";
            // 
            // gbDataHora
            // 
            this.gbDataHora.Controls.Add(this.dtPickerDataHora);
            this.gbDataHora.Controls.Add(this.ckbDataHoraComputador);
            this.gbDataHora.Location = new System.Drawing.Point(30, 36);
            this.gbDataHora.Name = "gbDataHora";
            this.gbDataHora.Size = new System.Drawing.Size(219, 75);
            this.gbDataHora.TabIndex = 2;
            this.gbDataHora.TabStop = false;
            // 
            // dtPickerDataHora
            // 
            this.dtPickerDataHora.EditValue = null;
            this.dtPickerDataHora.Location = new System.Drawing.Point(16, 46);
            this.dtPickerDataHora.Name = "dtPickerDataHora";
            this.dtPickerDataHora.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtPickerDataHora.Properties.DisplayFormat.FormatString = "g";
            this.dtPickerDataHora.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtPickerDataHora.Properties.Mask.EditMask = "g";
            this.dtPickerDataHora.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dtPickerDataHora.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtPickerDataHora.Size = new System.Drawing.Size(184, 20);
            this.dtPickerDataHora.TabIndex = 2;
            // 
            // ckbDataHoraComputador
            // 
            this.ckbDataHoraComputador.AutoSize = true;
            this.ckbDataHoraComputador.Location = new System.Drawing.Point(16, 19);
            this.ckbDataHoraComputador.Name = "ckbDataHoraComputador";
            this.ckbDataHoraComputador.Size = new System.Drawing.Size(184, 17);
            this.ckbDataHoraComputador.TabIndex = 1;
            this.ckbDataHoraComputador.Text = "Usar Data e Hora do Computador";
            this.ckbDataHoraComputador.UseVisualStyleBackColor = true;
            this.ckbDataHoraComputador.CheckStateChanged += new System.EventHandler(this.ckbDataHoraComputador_CheckStateChanged);
            // 
            // rbDataHora
            // 
            this.rbDataHora.AutoSize = true;
            this.rbDataHora.Location = new System.Drawing.Point(46, 19);
            this.rbDataHora.Name = "rbDataHora";
            this.rbDataHora.Size = new System.Drawing.Size(83, 17);
            this.rbDataHora.TabIndex = 0;
            this.rbDataHora.TabStop = true;
            this.rbDataHora.Text = "Data e Hora";
            this.rbDataHora.UseVisualStyleBackColor = true;
            this.rbDataHora.Click += new System.EventHandler(this.rbDataHora_Click);
            // 
            // rbHorarioVerao
            // 
            this.rbHorarioVerao.AutoSize = true;
            this.rbHorarioVerao.Location = new System.Drawing.Point(306, 19);
            this.rbHorarioVerao.Name = "rbHorarioVerao";
            this.rbHorarioVerao.Size = new System.Drawing.Size(105, 17);
            this.rbHorarioVerao.TabIndex = 1;
            this.rbHorarioVerao.TabStop = true;
            this.rbHorarioVerao.Text = "Horário de Verão";
            this.rbHorarioVerao.UseVisualStyleBackColor = true;
            this.rbHorarioVerao.CheckedChanged += new System.EventHandler(this.rbDataHora_Click);
            // 
            // lbRelogio
            // 
            this.lbRelogio.AutoSize = true;
            this.lbRelogio.Location = new System.Drawing.Point(33, 26);
            this.lbRelogio.Name = "lbRelogio";
            this.lbRelogio.Size = new System.Drawing.Size(43, 13);
            this.lbRelogio.TabIndex = 24;
            this.lbRelogio.Text = "Relógio";
            // 
            // ckbRelogio
            // 
            this.ckbRelogio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ckbRelogio.FormattingEnabled = true;
            this.ckbRelogio.Location = new System.Drawing.Point(82, 23);
            this.ckbRelogio.Name = "ckbRelogio";
            this.ckbRelogio.Size = new System.Drawing.Size(496, 21);
            this.ckbRelogio.TabIndex = 23;
            // 
            // FormConfiguracoesDataHora
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 538);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.pnlBotoes);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormConfiguracoesDataHora";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enviar Configurações de Data e Hora";
            this.Load += new System.EventHandler(this.FormConfiguracoesDataHora_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcImportacaoes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipoImportacao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAtivarImportacao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seTempoImportacao)).EndInit();
            this.pnlBotoes.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gbEnviaConfigDataHora.ResumeLayout(false);
            this.gbEnviaConfigDataHora.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gbHorarioVerao.ResumeLayout(false);
            this.gbHorarioVerao.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerTermino.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerTermino.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerInicio.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerInicio.Properties)).EndInit();
            this.gbDataHora.ResumeLayout(false);
            this.gbDataHora.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerDataHora.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtPickerDataHora.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraGrid.GridControl gcImportacaoes;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkpTipoImportacao;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chbAtivarImportacao;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit seTempoImportacao;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcCodigo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcNumRel;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcLocalRel;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcModeloRel;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcEnviaDataHoraServidor;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcEnviaHorarioVerao;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcDtInicioHorarioVerao;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcDtFimHorarioVerao;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcIncDataHora;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcUsuInclusao;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private System.Windows.Forms.Panel pnlBotoes;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox gbEnviaConfigDataHora;
        private System.Windows.Forms.Label lbRelogio;
        private System.Windows.Forms.ComboBox ckbRelogio;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbHorarioVerao;
        private System.Windows.Forms.RadioButton rbDataHora;
        private System.Windows.Forms.GroupBox gbHorarioVerao;
        private System.Windows.Forms.GroupBox gbDataHora;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ckbDataHoraComputador;
        private DevExpress.XtraEditors.DateEdit dtPickerDataHora;
        private DevExpress.XtraEditors.DateEdit dtPickerTermino;
        private DevExpress.XtraEditors.DateEdit dtPickerInicio;
        private System.Windows.Forms.GroupBox groupBox4;
        public DevExpress.XtraEditors.SimpleButton sbEnviarDadosRep;
        public DevExpress.XtraEditors.SimpleButton sbFechar;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lbInfoEnviarConfig;
    }
}