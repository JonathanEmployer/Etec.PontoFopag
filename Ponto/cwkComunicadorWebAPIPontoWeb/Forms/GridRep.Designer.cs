namespace cwkComunicadorWebAPIPontoWeb
{
    partial class GridRep
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridRep));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gcReps = new DevExpress.XtraGrid.GridControl();
            this.gvReps = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcNumRelogio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcLocal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcIP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcPorta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcBiometrico = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcFabricante = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcModelo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcUltimoNsr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcAtivarImportacao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.chbAtivarImportacao = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gcTipoImportacao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkpTipoImportacao = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gcTempoImportacao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.seTempoImportacao = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.sbForcarComunicacao = new DevExpress.XtraEditors.SimpleButton();
            this.sbFechar = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcReps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAtivarImportacao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipoImportacao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seTempoImportacao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(10, 10);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(10);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(878, 441);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gcReps);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(2, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(874, 370);
            this.panelControl3.TabIndex = 1;
            // 
            // gcReps
            // 
            this.gcReps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcReps.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcReps.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcReps.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcReps.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcReps.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcReps.Location = new System.Drawing.Point(2, 2);
            this.gcReps.MainView = this.gvReps;
            this.gcReps.Name = "gcReps";
            this.gcReps.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkpTipoImportacao,
            this.chbAtivarImportacao,
            this.seTempoImportacao});
            this.gcReps.Size = new System.Drawing.Size(870, 366);
            this.gcReps.TabIndex = 29;
            this.gcReps.UseEmbeddedNavigator = true;
            this.gcReps.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvReps});
            this.gcReps.Load += new System.EventHandler(this.gcReps_Load);
            this.gcReps.Leave += new System.EventHandler(this.gcReps_Leave);
            // 
            // gvReps
            // 
            this.gvReps.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvReps.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvReps.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvReps.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvReps.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvReps.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvReps.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvReps.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvReps.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvReps.Appearance.Empty.Options.UseBackColor = true;
            this.gvReps.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvReps.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvReps.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvReps.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvReps.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvReps.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvReps.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvReps.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvReps.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvReps.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvReps.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvReps.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvReps.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvReps.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvReps.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvReps.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvReps.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvReps.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvReps.Appearance.FocusedCell.BackColor = System.Drawing.Color.RoyalBlue;
            this.gvReps.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvReps.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvReps.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvReps.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvReps.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvReps.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvReps.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvReps.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvReps.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvReps.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvReps.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvReps.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvReps.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvReps.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvReps.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvReps.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvReps.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvReps.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvReps.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvReps.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvReps.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvReps.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvReps.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvReps.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvReps.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvReps.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvReps.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvReps.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvReps.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvReps.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvReps.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvReps.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvReps.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvReps.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvReps.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvReps.Appearance.OddRow.Options.UseBackColor = true;
            this.gvReps.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvReps.Appearance.OddRow.Options.UseForeColor = true;
            this.gvReps.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvReps.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.Preview.Options.UseFont = true;
            this.gvReps.Appearance.Preview.Options.UseForeColor = true;
            this.gvReps.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvReps.Appearance.Row.Options.UseBackColor = true;
            this.gvReps.Appearance.Row.Options.UseTextOptions = true;
            this.gvReps.Appearance.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gvReps.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvReps.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
            this.gvReps.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvReps.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvReps.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvReps.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvReps.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvReps.AppearancePrint.Row.Options.UseTextOptions = true;
            this.gvReps.AppearancePrint.Row.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gvReps.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcCodigo,
            this.gcNumRelogio,
            this.gcLocal,
            this.gcIP,
            this.gcPorta,
            this.gcBiometrico,
            this.gcFabricante,
            this.gcModelo,
            this.gcUltimoNsr,
            this.gcAtivarImportacao,
            this.gcTipoImportacao,
            this.gcTempoImportacao});
            this.gvReps.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gvReps.GridControl = this.gcReps;
            this.gvReps.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.gvReps.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvReps.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.gvReps.Name = "gvReps";
            this.gvReps.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvReps.OptionsCustomization.AllowGroup = false;
            this.gvReps.OptionsDetail.EnableMasterViewMode = false;
            this.gvReps.OptionsSelection.MultiSelect = true;
            this.gvReps.OptionsView.ColumnAutoWidth = false;
            this.gvReps.OptionsView.EnableAppearanceEvenRow = true;
            this.gvReps.OptionsView.EnableAppearanceOddRow = true;
            this.gvReps.OptionsView.RowAutoHeight = true;
            this.gvReps.OptionsView.ShowAutoFilterRow = true;
            this.gvReps.OptionsView.ShowFooter = true;
            this.gvReps.OptionsView.ShowGroupPanel = false;
            this.gvReps.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gvReps_InvalidRowException);
            this.gvReps.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvReps_BeforeLeaveRow);
            this.gvReps.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gvReps_ValidateRow);
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
            this.gcCodigo.VisibleIndex = 0;
            // 
            // gcNumRelogio
            // 
            this.gcNumRelogio.AppearanceCell.Options.UseTextOptions = true;
            this.gcNumRelogio.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gcNumRelogio.AppearanceHeader.Options.UseTextOptions = true;
            this.gcNumRelogio.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcNumRelogio.Caption = "Nº Relógio";
            this.gcNumRelogio.FieldName = "NumSerie";
            this.gcNumRelogio.Name = "gcNumRelogio";
            this.gcNumRelogio.OptionsColumn.AllowEdit = false;
            this.gcNumRelogio.OptionsColumn.AllowFocus = false;
            this.gcNumRelogio.OptionsColumn.ReadOnly = true;
            this.gcNumRelogio.Visible = true;
            this.gcNumRelogio.VisibleIndex = 1;
            this.gcNumRelogio.Width = 100;
            // 
            // gcLocal
            // 
            this.gcLocal.AppearanceHeader.Options.UseTextOptions = true;
            this.gcLocal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcLocal.Caption = "Local";
            this.gcLocal.FieldName = "EnderecoEmpregador";
            this.gcLocal.Name = "gcLocal";
            this.gcLocal.OptionsColumn.AllowEdit = false;
            this.gcLocal.OptionsColumn.AllowFocus = false;
            this.gcLocal.OptionsColumn.ReadOnly = true;
            this.gcLocal.Visible = true;
            this.gcLocal.VisibleIndex = 2;
            this.gcLocal.Width = 200;
            // 
            // gcIP
            // 
            this.gcIP.AppearanceCell.Options.UseTextOptions = true;
            this.gcIP.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gcIP.AppearanceHeader.Options.UseTextOptions = true;
            this.gcIP.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcIP.Caption = "End. IP";
            this.gcIP.FieldName = "Ip";
            this.gcIP.Name = "gcIP";
            this.gcIP.OptionsColumn.AllowEdit = false;
            this.gcIP.OptionsColumn.AllowFocus = false;
            this.gcIP.OptionsColumn.ReadOnly = true;
            this.gcIP.Visible = true;
            this.gcIP.VisibleIndex = 3;
            this.gcIP.Width = 100;
            // 
            // gcPorta
            // 
            this.gcPorta.AppearanceCell.Options.UseTextOptions = true;
            this.gcPorta.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gcPorta.AppearanceHeader.Options.UseTextOptions = true;
            this.gcPorta.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcPorta.Caption = "Porta";
            this.gcPorta.FieldName = "Porta";
            this.gcPorta.Name = "gcPorta";
            this.gcPorta.OptionsColumn.AllowEdit = false;
            this.gcPorta.OptionsColumn.AllowFocus = false;
            this.gcPorta.OptionsColumn.ReadOnly = true;
            this.gcPorta.Visible = true;
            this.gcPorta.VisibleIndex = 4;
            // 
            // gcBiometrico
            // 
            this.gcBiometrico.AppearanceCell.Options.UseTextOptions = true;
            this.gcBiometrico.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcBiometrico.AppearanceHeader.Options.UseTextOptions = true;
            this.gcBiometrico.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcBiometrico.Caption = "Biométrico";
            this.gcBiometrico.FieldName = "UtilizaBiometria";
            this.gcBiometrico.Name = "gcBiometrico";
            this.gcBiometrico.OptionsColumn.AllowEdit = false;
            this.gcBiometrico.OptionsColumn.AllowFocus = false;
            this.gcBiometrico.OptionsColumn.ReadOnly = true;
            this.gcBiometrico.Visible = true;
            this.gcBiometrico.VisibleIndex = 5;
            // 
            // gcFabricante
            // 
            this.gcFabricante.AppearanceHeader.Options.UseTextOptions = true;
            this.gcFabricante.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcFabricante.Caption = "Fabricante";
            this.gcFabricante.FieldName = "NomeFabricante";
            this.gcFabricante.Name = "gcFabricante";
            this.gcFabricante.OptionsColumn.AllowEdit = false;
            this.gcFabricante.OptionsColumn.AllowFocus = false;
            this.gcFabricante.OptionsColumn.ReadOnly = true;
            this.gcFabricante.Visible = true;
            this.gcFabricante.VisibleIndex = 6;
            this.gcFabricante.Width = 100;
            // 
            // gcModelo
            // 
            this.gcModelo.AppearanceHeader.Options.UseTextOptions = true;
            this.gcModelo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcModelo.Caption = "Modelo";
            this.gcModelo.FieldName = "NomeModelo";
            this.gcModelo.Name = "gcModelo";
            this.gcModelo.OptionsColumn.AllowEdit = false;
            this.gcModelo.OptionsColumn.AllowFocus = false;
            this.gcModelo.OptionsColumn.ReadOnly = true;
            this.gcModelo.Visible = true;
            this.gcModelo.VisibleIndex = 7;
            this.gcModelo.Width = 100;
            // 
            // gcUltimoNsr
            // 
            this.gcUltimoNsr.Caption = "Último NSR";
            this.gcUltimoNsr.FieldName = "UltimoNsr";
            this.gcUltimoNsr.Name = "gcUltimoNsr";
            this.gcUltimoNsr.OptionsColumn.AllowEdit = false;
            this.gcUltimoNsr.OptionsColumn.AllowFocus = false;
            this.gcUltimoNsr.OptionsColumn.ReadOnly = true;
            this.gcUltimoNsr.Visible = true;
            this.gcUltimoNsr.VisibleIndex = 11;
            // 
            // gcAtivarImportacao
            // 
            this.gcAtivarImportacao.AppearanceCell.Options.UseTextOptions = true;
            this.gcAtivarImportacao.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcAtivarImportacao.AppearanceHeader.Options.UseTextOptions = true;
            this.gcAtivarImportacao.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcAtivarImportacao.Caption = "Ativar Importação";
            this.gcAtivarImportacao.ColumnEdit = this.chbAtivarImportacao;
            this.gcAtivarImportacao.FieldName = "ImportacaoAtivada";
            this.gcAtivarImportacao.Name = "gcAtivarImportacao";
            this.gcAtivarImportacao.Visible = true;
            this.gcAtivarImportacao.VisibleIndex = 8;
            this.gcAtivarImportacao.Width = 100;
            // 
            // chbAtivarImportacao
            // 
            this.chbAtivarImportacao.AutoHeight = false;
            this.chbAtivarImportacao.Name = "chbAtivarImportacao";
            // 
            // gcTipoImportacao
            // 
            this.gcTipoImportacao.AppearanceHeader.Options.UseTextOptions = true;
            this.gcTipoImportacao.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcTipoImportacao.Caption = "Tipo Importação";
            this.gcTipoImportacao.ColumnEdit = this.lkpTipoImportacao;
            this.gcTipoImportacao.FieldName = "TipoImportacao";
            this.gcTipoImportacao.Name = "gcTipoImportacao";
            this.gcTipoImportacao.Visible = true;
            this.gcTipoImportacao.VisibleIndex = 9;
            this.gcTipoImportacao.Width = 100;
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
            // gcTempoImportacao
            // 
            this.gcTempoImportacao.AppearanceCell.Options.UseTextOptions = true;
            this.gcTempoImportacao.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gcTempoImportacao.AppearanceHeader.Options.UseTextOptions = true;
            this.gcTempoImportacao.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcTempoImportacao.Caption = "Tempo Importação";
            this.gcTempoImportacao.ColumnEdit = this.seTempoImportacao;
            this.gcTempoImportacao.FieldName = "TempoImportacao";
            this.gcTempoImportacao.Name = "gcTempoImportacao";
            this.gcTempoImportacao.Visible = true;
            this.gcTempoImportacao.VisibleIndex = 10;
            this.gcTempoImportacao.Width = 100;
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
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.sbForcarComunicacao);
            this.panelControl2.Controls.Add(this.sbFechar);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(2, 372);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(874, 67);
            this.panelControl2.TabIndex = 0;
            // 
            // sbForcarComunicacao
            // 
            this.sbForcarComunicacao.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbForcarComunicacao.Image = global::cwkComunicadorWebAPIPontoWeb.Properties.Resources.Alterar_copy;
            this.sbForcarComunicacao.ImageIndex = 6;
            this.sbForcarComunicacao.Location = new System.Drawing.Point(719, 6);
            this.sbForcarComunicacao.Name = "sbForcarComunicacao";
            this.sbForcarComunicacao.Size = new System.Drawing.Size(150, 23);
            this.sbForcarComunicacao.TabIndex = 11;
            this.sbForcarComunicacao.Text = "Forçar Comunicação";
            this.sbForcarComunicacao.Click += new System.EventHandler(this.sbForcarComunicacao_Click);
            // 
            // sbFechar
            // 
            this.sbFechar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbFechar.Image = global::cwkComunicadorWebAPIPontoWeb.Properties.Resources.cancelar_copy;
            this.sbFechar.ImageIndex = 7;
            this.sbFechar.Location = new System.Drawing.Point(794, 35);
            this.sbFechar.Name = "sbFechar";
            this.sbFechar.Size = new System.Drawing.Size(75, 23);
            this.sbFechar.TabIndex = 14;
            this.sbFechar.Text = "&Fechar";
            this.sbFechar.Click += new System.EventHandler(this.sbFechar_Click);
            // 
            // GridRep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 461);
            this.Controls.Add(this.panelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GridRep";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tabela de REP";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GridRep_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcReps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAtivarImportacao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpTipoImportacao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seTempoImportacao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        public DevExpress.XtraEditors.SimpleButton sbForcarComunicacao;
        public DevExpress.XtraEditors.SimpleButton sbFechar;
        private DevExpress.XtraGrid.GridControl gcReps;
        private DevExpress.XtraGrid.Views.Grid.GridView gvReps;
        private DevExpress.XtraGrid.Columns.GridColumn gcCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn gcNumRelogio;
        private DevExpress.XtraGrid.Columns.GridColumn gcLocal;
        private DevExpress.XtraGrid.Columns.GridColumn gcIP;
        private DevExpress.XtraGrid.Columns.GridColumn gcPorta;
        private DevExpress.XtraGrid.Columns.GridColumn gcBiometrico;
        private DevExpress.XtraGrid.Columns.GridColumn gcFabricante;
        private DevExpress.XtraGrid.Columns.GridColumn gcModelo;
        private DevExpress.XtraGrid.Columns.GridColumn gcAtivarImportacao;
        private DevExpress.XtraGrid.Columns.GridColumn gcTipoImportacao;
        private DevExpress.XtraGrid.Columns.GridColumn gcTempoImportacao;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chbAtivarImportacao;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkpTipoImportacao;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit seTempoImportacao;
        private DevExpress.XtraGrid.Columns.GridColumn gcUltimoNsr;

    }
}