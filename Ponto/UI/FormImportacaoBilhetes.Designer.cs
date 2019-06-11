namespace UI
{
	partial class FormImportacaoBilhetes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportacaoBilhetes));
            this.ColunaId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.cbIdFuncionario = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdIdentificacao = new Componentes.devexpress.cwk_DevButton();
            this.chbMarcacaoIndividual = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.DTFinal = new DevExpress.XtraEditors.DateEdit();
            this.lblFuncionario = new DevExpress.XtraEditors.LabelControl();
            this.DTInicial = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.gcTotaisMarcacao = new DevExpress.XtraGrid.GridControl();
            this.gvTotaisMarcacao = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColunaCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColunaDescricao = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColunaDiretorio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcImportar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.txtDiretorio = new DevExpress.XtraEditors.TextEdit();
            this.lblDiretorio = new DevExpress.XtraEditors.LabelControl();
            this.sbIdDiretorio = new Componentes.devexpress.cwk_DevButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sbImportar = new DevExpress.XtraEditors.SimpleButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.sbAjuda = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbMarcacaoIndividual.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTotaisMarcacao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTotaisMarcacao)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiretorio.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ColunaId
            // 
            this.ColunaId.Caption = "ID";
            this.ColunaId.FieldName = "Id";
            this.ColunaId.Name = "ColunaId";
            this.ColunaId.OptionsColumn.AllowEdit = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Controls.Add(this.gcTotaisMarcacao);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Location = new System.Drawing.Point(12, 12);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(579, 335);
            this.panelControl1.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.cbIdFuncionario);
            this.groupControl2.Controls.Add(this.chbMarcacaoIndividual);
            this.groupControl2.Controls.Add(this.labelControl3);
            this.groupControl2.Controls.Add(this.sbIdIdentificacao);
            this.groupControl2.Controls.Add(this.DTFinal);
            this.groupControl2.Controls.Add(this.lblFuncionario);
            this.groupControl2.Controls.Add(this.DTInicial);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Location = new System.Drawing.Point(5, 218);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(569, 111);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.TabStop = true;
            this.groupControl2.Text = "Filtro de Importação";
            // 
            // cbIdFuncionario
            // 
            this.cbIdFuncionario.ButtonLookup = this.sbIdIdentificacao;
            this.cbIdFuncionario.Enabled = false;
            this.cbIdFuncionario.Key = System.Windows.Forms.Keys.F5;
            this.cbIdFuncionario.Location = new System.Drawing.Point(75, 83);
            this.cbIdFuncionario.Name = "cbIdFuncionario";
            this.cbIdFuncionario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdFuncionario.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "ID", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dscodigo", "Código"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("matricula", "Name6", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("jornada", "Name7", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("empresa", "Name8", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("departamento", "Name9", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("carteira", "Name10", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dataadmissao", "Name59", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.cbIdFuncionario.Properties.DisplayMember = "nome";
            this.cbIdFuncionario.Properties.NullText = "";
            this.cbIdFuncionario.Properties.ValueMember = "id";
            this.cbIdFuncionario.Size = new System.Drawing.Size(458, 20);
            this.cbIdFuncionario.TabIndex = 6;
            // 
            // sbIdIdentificacao
            // 
            this.sbIdIdentificacao.Enabled = false;
            this.sbIdIdentificacao.Location = new System.Drawing.Point(539, 83);
            this.sbIdIdentificacao.Name = "sbIdIdentificacao";
            this.sbIdIdentificacao.Size = new System.Drawing.Size(24, 20);
            this.sbIdIdentificacao.TabIndex = 7;
            this.sbIdIdentificacao.TabStop = false;
            this.sbIdIdentificacao.Text = "...";
            this.sbIdIdentificacao.Click += new System.EventHandler(this.sbIdIdentificacao_Click);
            // 
            // chbMarcacaoIndividual
            // 
            this.chbMarcacaoIndividual.AutoSizeInLayoutControl = true;
            this.chbMarcacaoIndividual.Location = new System.Drawing.Point(73, 58);
            this.chbMarcacaoIndividual.Name = "chbMarcacaoIndividual";
            this.chbMarcacaoIndividual.Properties.Caption = "Marcação Individual";
            this.chbMarcacaoIndividual.Size = new System.Drawing.Size(121, 19);
            this.chbMarcacaoIndividual.TabIndex = 4;
            this.chbMarcacaoIndividual.CheckedChanged += new System.EventHandler(this.chbMarcacaoIndividual_CheckedChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(161, 35);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(6, 13);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "à";
            // 
            // DTFinal
            // 
            this.DTFinal.EditValue = null;
            this.DTFinal.Location = new System.Drawing.Point(173, 32);
            this.DTFinal.Name = "DTFinal";
            this.DTFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DTFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.DTFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DTFinal.Size = new System.Drawing.Size(80, 20);
            this.DTFinal.TabIndex = 3;
            // 
            // lblFuncionario
            // 
            this.lblFuncionario.Location = new System.Drawing.Point(10, 86);
            this.lblFuncionario.Name = "lblFuncionario";
            this.lblFuncionario.Size = new System.Drawing.Size(59, 13);
            this.lblFuncionario.TabIndex = 5;
            this.lblFuncionario.Text = "Funcionário:";
            // 
            // DTInicial
            // 
            this.DTInicial.EditValue = null;
            this.DTInicial.Location = new System.Drawing.Point(75, 32);
            this.DTInicial.Name = "DTInicial";
            this.DTInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DTInicial.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.DTInicial.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.DTInicial.Size = new System.Drawing.Size(80, 20);
            this.DTInicial.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(14, 35);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(55, 13);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "Período de:";
            // 
            // gcTotaisMarcacao
            // 
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcTotaisMarcacao.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcTotaisMarcacao.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcTotaisMarcacao.Location = new System.Drawing.Point(5, 5);
            this.gcTotaisMarcacao.LookAndFeel.UseWindowsXPTheme = true;
            this.gcTotaisMarcacao.MainView = this.gvTotaisMarcacao;
            this.gcTotaisMarcacao.Name = "gcTotaisMarcacao";
            this.gcTotaisMarcacao.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.gcTotaisMarcacao.Size = new System.Drawing.Size(569, 136);
            this.gcTotaisMarcacao.TabIndex = 0;
            this.gcTotaisMarcacao.TabStop = false;
            this.gcTotaisMarcacao.UseEmbeddedNavigator = true;
            this.gcTotaisMarcacao.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTotaisMarcacao});
            // 
            // gvTotaisMarcacao
            // 
            this.gvTotaisMarcacao.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvTotaisMarcacao.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.Empty.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvTotaisMarcacao.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvTotaisMarcacao.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvTotaisMarcacao.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvTotaisMarcacao.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvTotaisMarcacao.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvTotaisMarcacao.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvTotaisMarcacao.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvTotaisMarcacao.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvTotaisMarcacao.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvTotaisMarcacao.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvTotaisMarcacao.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvTotaisMarcacao.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvTotaisMarcacao.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvTotaisMarcacao.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvTotaisMarcacao.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvTotaisMarcacao.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvTotaisMarcacao.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvTotaisMarcacao.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvTotaisMarcacao.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvTotaisMarcacao.Appearance.OddRow.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.OddRow.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvTotaisMarcacao.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.Preview.Options.UseFont = true;
            this.gvTotaisMarcacao.Appearance.Preview.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvTotaisMarcacao.Appearance.Row.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvTotaisMarcacao.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvTotaisMarcacao.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvTotaisMarcacao.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvTotaisMarcacao.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvTotaisMarcacao.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvTotaisMarcacao.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvTotaisMarcacao.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ColunaId,
            this.ColunaCodigo,
            this.ColunaDescricao,
            this.ColunaDiretorio,
            this.gcImportar});
            this.gvTotaisMarcacao.GridControl = this.gcTotaisMarcacao;
            this.gvTotaisMarcacao.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvTotaisMarcacao.Name = "gvTotaisMarcacao";
            this.gvTotaisMarcacao.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvTotaisMarcacao.OptionsView.EnableAppearanceEvenRow = true;
            this.gvTotaisMarcacao.OptionsView.EnableAppearanceOddRow = true;
            this.gvTotaisMarcacao.OptionsView.ShowAutoFilterRow = true;
            this.gvTotaisMarcacao.OptionsView.ShowGroupPanel = false;
            this.gvTotaisMarcacao.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.Default;
            this.gvTotaisMarcacao.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvTotaisMarcacao_FocusedRowChanged);
            // 
            // ColunaCodigo
            // 
            this.ColunaCodigo.Caption = "Código";
            this.ColunaCodigo.FieldName = "Codigo";
            this.ColunaCodigo.Name = "ColunaCodigo";
            this.ColunaCodigo.OptionsColumn.AllowEdit = false;
            this.ColunaCodigo.Visible = true;
            this.ColunaCodigo.VisibleIndex = 0;
            this.ColunaCodigo.Width = 80;
            // 
            // ColunaDescricao
            // 
            this.ColunaDescricao.Caption = "Descrição";
            this.ColunaDescricao.FieldName = "Descricao";
            this.ColunaDescricao.Name = "ColunaDescricao";
            this.ColunaDescricao.OptionsColumn.AllowEdit = false;
            this.ColunaDescricao.Visible = true;
            this.ColunaDescricao.VisibleIndex = 1;
            this.ColunaDescricao.Width = 372;
            // 
            // ColunaDiretorio
            // 
            this.ColunaDiretorio.Caption = "Diretório";
            this.ColunaDiretorio.FieldName = "Diretorio";
            this.ColunaDiretorio.Name = "ColunaDiretorio";
            this.ColunaDiretorio.OptionsColumn.AllowEdit = false;
            // 
            // gcImportar
            // 
            this.gcImportar.Caption = "Importar";
            this.gcImportar.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gcImportar.FieldName = "BImporta";
            this.gcImportar.Name = "gcImportar";
            this.gcImportar.Visible = true;
            this.gcImportar.VisibleIndex = 2;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtDiretorio);
            this.groupControl1.Controls.Add(this.lblDiretorio);
            this.groupControl1.Controls.Add(this.sbIdDiretorio);
            this.groupControl1.Location = new System.Drawing.Point(5, 147);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(569, 65);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.TabStop = true;
            this.groupControl1.Text = "Processa Marcações Funcionários";
            // 
            // txtDiretorio
            // 
            this.txtDiretorio.Location = new System.Drawing.Point(75, 31);
            this.txtDiretorio.Name = "txtDiretorio";
            this.txtDiretorio.Size = new System.Drawing.Size(458, 20);
            this.txtDiretorio.TabIndex = 1;
            this.txtDiretorio.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDiretorio_KeyDown);
            // 
            // lblDiretorio
            // 
            this.lblDiretorio.Location = new System.Drawing.Point(24, 34);
            this.lblDiretorio.Name = "lblDiretorio";
            this.lblDiretorio.Size = new System.Drawing.Size(45, 13);
            this.lblDiretorio.TabIndex = 0;
            this.lblDiretorio.Text = "Diretório:";
            // 
            // sbIdDiretorio
            // 
            this.sbIdDiretorio.Location = new System.Drawing.Point(539, 31);
            this.sbIdDiretorio.Name = "sbIdDiretorio";
            this.sbIdDiretorio.Size = new System.Drawing.Size(24, 20);
            this.sbIdDiretorio.TabIndex = 2;
            this.sbIdDiretorio.TabStop = false;
            this.sbIdDiretorio.Text = "...";
            this.sbIdDiretorio.Click += new System.EventHandler(this.sbIdDiretorio_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // sbImportar
            // 
            this.sbImportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbImportar.ImageIndex = 0;
            this.sbImportar.ImageList = this.imageList1;
            this.sbImportar.Location = new System.Drawing.Point(435, 353);
            this.sbImportar.Name = "sbImportar";
            this.sbImportar.Size = new System.Drawing.Size(75, 23);
            this.sbImportar.TabIndex = 1;
            this.sbImportar.Text = "&Importar";
            this.sbImportar.Click += new System.EventHandler(this.sbImportar_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Importação de Bilhetes copy.ico");
            this.imageList1.Images.SetKeyName(1, "cancelar copy.ico");
            this.imageList1.Images.SetKeyName(2, "Help copy.ico");
            // 
            // sbCancelar
            // 
            this.sbCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sbCancelar.ImageIndex = 1;
            this.sbCancelar.ImageList = this.imageList1;
            this.sbCancelar.Location = new System.Drawing.Point(516, 353);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(75, 23);
            this.sbCancelar.TabIndex = 2;
            this.sbCancelar.Text = "&Cancelar";
            this.sbCancelar.Click += new System.EventHandler(this.sbCancelar_Click);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sbAjuda.ImageIndex = 2;
            this.sbAjuda.ImageList = this.imageList1;
            this.sbAjuda.Location = new System.Drawing.Point(12, 353);
            this.sbAjuda.Name = "sbAjuda";
            this.sbAjuda.Size = new System.Drawing.Size(75, 23);
            this.sbAjuda.TabIndex = 3;
            this.sbAjuda.Text = "A&juda";
            this.sbAjuda.Click += new System.EventHandler(this.sbAjuda_Click);
            // 
            // FormImportacaoBilhetes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 381);
            this.Controls.Add(this.sbAjuda);
            this.Controls.Add(this.sbImportar);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormImportacaoBilhetes";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Importação Bilhetes";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormImportacaoBilhetes_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormImportacaoBilhetes_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbMarcacaoIndividual.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DTInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcTotaisMarcacao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTotaisMarcacao)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDiretorio.Properties)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private Componentes.devexpress.cwk_DevLookup cbIdFuncionario;
        private Componentes.devexpress.cwk_DevButton sbIdIdentificacao;
        private DevExpress.XtraEditors.LabelControl lblFuncionario;
        private DevExpress.XtraEditors.CheckEdit chbMarcacaoIndividual;
        private Componentes.devexpress.cwk_DevButton sbIdDiretorio;
        private DevExpress.XtraEditors.TextEdit txtDiretorio;
        private DevExpress.XtraEditors.LabelControl lblDiretorio;
        public DevExpress.XtraGrid.GridControl gcTotaisMarcacao;
        public DevExpress.XtraGrid.Views.Grid.GridView gvTotaisMarcacao;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public DevExpress.XtraEditors.SimpleButton sbImportar;
        public DevExpress.XtraEditors.SimpleButton sbCancelar;
        public DevExpress.XtraEditors.SimpleButton sbAjuda;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.DateEdit DTFinal;
        private DevExpress.XtraEditors.DateEdit DTInicial;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaId;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaDescricao;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaDiretorio;
        private DevExpress.XtraGrid.Columns.GridColumn gcImportar;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;

    }
}