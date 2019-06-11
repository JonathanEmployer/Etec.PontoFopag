namespace UI.IntegracaoRelogio
{
    partial class FormEnviarEmpresaEFuncionarios
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEnviarEmpresaEFuncionarios));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.chGrupoEconomico = new DevExpress.XtraEditors.CheckEdit();
            this.chbEnviarFuncionarios = new DevExpress.XtraEditors.CheckEdit();
            this.chbEnviarEmpresa = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.sbIdRep = new Componentes.devexpress.cwk_DevButton();
            this.cbIdRep = new Componentes.devexpress.cwk_DevLookup();
            this.sbSelecionarFuncAtivos = new DevExpress.XtraEditors.SimpleButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sbLimparEmpresas = new DevExpress.XtraEditors.SimpleButton();
            this.sbSelecionarEmpresas = new DevExpress.XtraEditors.SimpleButton();
            this.sbLimparFuncionarios = new DevExpress.XtraEditors.SimpleButton();
            this.sbSelecionarFuncionarios = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.gcFuncionarios = new DevExpress.XtraGrid.GridControl();
            this.gvFuncionarios = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colunaIDFuncionario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colunaNomeFuncionario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colunaFuncionarioAtivoFuncionario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colunaPisFuncionario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colunaCodigoFuncionario = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.gcEmpresas = new DevExpress.XtraGrid.GridControl();
            this.gvEmpresas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colunaIDEmpresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colunaNomeEmpresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colunaCodigoEmpresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.sbExportar = new DevExpress.XtraEditors.SimpleButton();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.sbAjuda = new DevExpress.XtraEditors.SimpleButton();
            this.dxErrorProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.sbExcluirFuncionarios = new DevExpress.XtraEditors.SimpleButton();
            this.sbExportarArquivos = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chGrupoEconomico.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbEnviarFuncionarios.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbEnviarEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdRep.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcFuncionarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFuncionarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcEmpresas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEmpresas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControl1.Controls.Add(this.chGrupoEconomico);
            this.panelControl1.Controls.Add(this.chbEnviarFuncionarios);
            this.panelControl1.Controls.Add(this.chbEnviarEmpresa);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.sbIdRep);
            this.panelControl1.Controls.Add(this.cbIdRep);
            this.panelControl1.Controls.Add(this.sbSelecionarFuncAtivos);
            this.panelControl1.Controls.Add(this.sbLimparEmpresas);
            this.panelControl1.Controls.Add(this.sbSelecionarEmpresas);
            this.panelControl1.Controls.Add(this.sbLimparFuncionarios);
            this.panelControl1.Controls.Add(this.sbSelecionarFuncionarios);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.gcFuncionarios);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.gcEmpresas);
            this.panelControl1.Location = new System.Drawing.Point(12, 12);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(735, 403);
            this.panelControl1.TabIndex = 0;
            // 
            // chGrupoEconomico
            // 
            this.chGrupoEconomico.Location = new System.Drawing.Point(442, 40);
            this.chGrupoEconomico.Name = "chGrupoEconomico";
            this.chGrupoEconomico.Properties.Caption = "Utilizar Grupo Econômico";
            this.chGrupoEconomico.Size = new System.Drawing.Size(143, 19);
            this.chGrupoEconomico.TabIndex = 14;
            this.chGrupoEconomico.CheckedChanged += new System.EventHandler(this.chGrupoEconomico_CheckedChanged);
            // 
            // chbEnviarFuncionarios
            // 
            this.chbEnviarFuncionarios.Location = new System.Drawing.Point(600, 10);
            this.chbEnviarFuncionarios.Name = "chbEnviarFuncionarios";
            this.chbEnviarFuncionarios.Properties.Caption = "Enviar Funcionários";
            this.chbEnviarFuncionarios.Size = new System.Drawing.Size(128, 19);
            this.chbEnviarFuncionarios.TabIndex = 3;
            this.chbEnviarFuncionarios.CheckedChanged += new System.EventHandler(this.chbEnviarFuncionarios_CheckedChanged);
            // 
            // chbEnviarEmpresa
            // 
            this.chbEnviarEmpresa.Location = new System.Drawing.Point(600, 37);
            this.chbEnviarEmpresa.Name = "chbEnviarEmpresa";
            this.chbEnviarEmpresa.Properties.Caption = "Enviar Empresa";
            this.chbEnviarEmpresa.Size = new System.Drawing.Size(103, 19);
            this.chbEnviarEmpresa.TabIndex = 4;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(14, 12);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(39, 13);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "Relógio:";
            // 
            // sbIdRep
            // 
            this.sbIdRep.Location = new System.Drawing.Point(561, 9);
            this.sbIdRep.Name = "sbIdRep";
            this.sbIdRep.Size = new System.Drawing.Size(24, 20);
            this.sbIdRep.TabIndex = 2;
            this.sbIdRep.TabStop = false;
            this.sbIdRep.Text = "...";
            this.sbIdRep.Click += new System.EventHandler(this.sbIdRep_Click);
            // 
            // cbIdRep
            // 
            this.cbIdRep.ButtonLookup = this.sbIdRep;
            this.cbIdRep.EditValue = 0;
            this.cbIdRep.Key = System.Windows.Forms.Keys.F5;
            this.cbIdRep.Location = new System.Drawing.Point(59, 9);
            this.cbIdRep.Name = "cbIdRep";
            this.cbIdRep.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdRep.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "Id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("local", "Local", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código")});
            this.cbIdRep.Properties.DisplayMember = "local";
            this.cbIdRep.Properties.NullText = "";
            this.cbIdRep.Properties.ValueMember = "id";
            this.cbIdRep.Size = new System.Drawing.Size(496, 20);
            this.cbIdRep.TabIndex = 1;
            this.cbIdRep.EditValueChanged += new System.EventHandler(this.cbIdRep_EditValueChanged);
            // 
            // sbSelecionarFuncAtivos
            // 
            this.sbSelecionarFuncAtivos.ImageIndex = 3;
            this.sbSelecionarFuncAtivos.ImageList = this.imageList1;
            this.sbSelecionarFuncAtivos.Location = new System.Drawing.Point(247, 180);
            this.sbSelecionarFuncAtivos.Name = "sbSelecionarFuncAtivos";
            this.sbSelecionarFuncAtivos.Size = new System.Drawing.Size(114, 23);
            this.sbSelecionarFuncAtivos.TabIndex = 11;
            this.sbSelecionarFuncAtivos.Text = "Selecionar Ativos";
            this.sbSelecionarFuncAtivos.Click += new System.EventHandler(this.sbSelecionarFuncAtivos_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Importação de Bilhetes copy.ico");
            this.imageList1.Images.SetKeyName(1, "cancelar copy.ico");
            this.imageList1.Images.SetKeyName(2, "Help copy.ico");
            this.imageList1.Images.SetKeyName(3, "Selecionar_timao.ico");
            // 
            // sbLimparEmpresas
            // 
            this.sbLimparEmpresas.ImageIndex = 1;
            this.sbLimparEmpresas.ImageList = this.imageList1;
            this.sbLimparEmpresas.Location = new System.Drawing.Point(205, 39);
            this.sbLimparEmpresas.Name = "sbLimparEmpresas";
            this.sbLimparEmpresas.Size = new System.Drawing.Size(117, 23);
            this.sbLimparEmpresas.TabIndex = 7;
            this.sbLimparEmpresas.Text = "Limpar Seleção";
            this.sbLimparEmpresas.Click += new System.EventHandler(this.sbLimparEmpresas_Click);
            // 
            // sbSelecionarEmpresas
            // 
            this.sbSelecionarEmpresas.ImageIndex = 3;
            this.sbSelecionarEmpresas.ImageList = this.imageList1;
            this.sbSelecionarEmpresas.Location = new System.Drawing.Point(85, 39);
            this.sbSelecionarEmpresas.Name = "sbSelecionarEmpresas";
            this.sbSelecionarEmpresas.Size = new System.Drawing.Size(117, 23);
            this.sbSelecionarEmpresas.TabIndex = 6;
            this.sbSelecionarEmpresas.Text = "Selecionar Todas";
            this.sbSelecionarEmpresas.Click += new System.EventHandler(this.sbSelecionarEmpresas_Click);
            // 
            // sbLimparFuncionarios
            // 
            this.sbLimparFuncionarios.ImageIndex = 1;
            this.sbLimparFuncionarios.ImageList = this.imageList1;
            this.sbLimparFuncionarios.Location = new System.Drawing.Point(364, 180);
            this.sbLimparFuncionarios.Name = "sbLimparFuncionarios";
            this.sbLimparFuncionarios.Size = new System.Drawing.Size(117, 23);
            this.sbLimparFuncionarios.TabIndex = 12;
            this.sbLimparFuncionarios.Text = "Limpar Seleção";
            this.sbLimparFuncionarios.Click += new System.EventHandler(this.sbLimparFuncionarios_Click);
            // 
            // sbSelecionarFuncionarios
            // 
            this.sbSelecionarFuncionarios.ImageIndex = 3;
            this.sbSelecionarFuncionarios.ImageList = this.imageList1;
            this.sbSelecionarFuncionarios.Location = new System.Drawing.Point(127, 180);
            this.sbSelecionarFuncionarios.Name = "sbSelecionarFuncionarios";
            this.sbSelecionarFuncionarios.Size = new System.Drawing.Size(117, 23);
            this.sbSelecionarFuncionarios.TabIndex = 10;
            this.sbSelecionarFuncionarios.Text = "Selecionar Todos";
            this.sbSelecionarFuncionarios.Click += new System.EventHandler(this.sbSelecionarFuncionarios_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Location = new System.Drawing.Point(7, 180);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(96, 18);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "Funcionários";
            // 
            // gcFuncionarios
            // 
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcFuncionarios.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcFuncionarios.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcFuncionarios.Location = new System.Drawing.Point(7, 204);
            this.gcFuncionarios.MainView = this.gvFuncionarios;
            this.gcFuncionarios.Name = "gcFuncionarios";
            this.gcFuncionarios.Size = new System.Drawing.Size(721, 194);
            this.gcFuncionarios.TabIndex = 13;
            this.gcFuncionarios.UseEmbeddedNavigator = true;
            this.gcFuncionarios.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvFuncionarios});
            // 
            // gvFuncionarios
            // 
            this.gvFuncionarios.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.Empty.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvFuncionarios.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvFuncionarios.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvFuncionarios.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvFuncionarios.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvFuncionarios.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvFuncionarios.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.OddRow.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.OddRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvFuncionarios.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.Preview.Options.UseFont = true;
            this.gvFuncionarios.Appearance.Preview.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvFuncionarios.Appearance.Row.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvFuncionarios.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvFuncionarios.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvFuncionarios.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvFuncionarios.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvFuncionarios.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvFuncionarios.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvFuncionarios.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colunaIDFuncionario,
            this.colunaNomeFuncionario,
            this.colunaFuncionarioAtivoFuncionario,
            this.colunaPisFuncionario,
            this.colunaCodigoFuncionario});
            this.gvFuncionarios.GridControl = this.gcFuncionarios;
            this.gvFuncionarios.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvFuncionarios.Name = "gvFuncionarios";
            this.gvFuncionarios.OptionsBehavior.Editable = false;
            this.gvFuncionarios.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvFuncionarios.OptionsView.EnableAppearanceEvenRow = true;
            this.gvFuncionarios.OptionsView.EnableAppearanceOddRow = true;
            this.gvFuncionarios.OptionsView.ShowAutoFilterRow = true;
            this.gvFuncionarios.OptionsView.ShowGroupPanel = false;
            this.gvFuncionarios.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colunaNomeFuncionario, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colunaIDFuncionario
            // 
            this.colunaIDFuncionario.Caption = "ID";
            this.colunaIDFuncionario.FieldName = "id";
            this.colunaIDFuncionario.Name = "colunaIDFuncionario";
            // 
            // colunaNomeFuncionario
            // 
            this.colunaNomeFuncionario.Caption = "Nome";
            this.colunaNomeFuncionario.FieldName = "nome";
            this.colunaNomeFuncionario.Name = "colunaNomeFuncionario";
            this.colunaNomeFuncionario.Visible = true;
            this.colunaNomeFuncionario.VisibleIndex = 0;
            this.colunaNomeFuncionario.Width = 430;
            // 
            // colunaFuncionarioAtivoFuncionario
            // 
            this.colunaFuncionarioAtivoFuncionario.Caption = "Ativo";
            this.colunaFuncionarioAtivoFuncionario.FieldName = "funcionarioativo";
            this.colunaFuncionarioAtivoFuncionario.Name = "colunaFuncionarioAtivoFuncionario";
            // 
            // colunaPisFuncionario
            // 
            this.colunaPisFuncionario.Caption = "Pis";
            this.colunaPisFuncionario.FieldName = "pis";
            this.colunaPisFuncionario.Name = "colunaPisFuncionario";
            this.colunaPisFuncionario.Visible = true;
            this.colunaPisFuncionario.VisibleIndex = 1;
            this.colunaPisFuncionario.Width = 140;
            // 
            // colunaCodigoFuncionario
            // 
            this.colunaCodigoFuncionario.Caption = "Código";
            this.colunaCodigoFuncionario.FieldName = "codigo";
            this.colunaCodigoFuncionario.Name = "colunaCodigoFuncionario";
            this.colunaCodigoFuncionario.Visible = true;
            this.colunaCodigoFuncionario.VisibleIndex = 2;
            this.colunaCodigoFuncionario.Width = 140;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Location = new System.Drawing.Point(7, 39);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(72, 18);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Empresas";
            // 
            // gcEmpresas
            // 
            this.gcEmpresas.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcEmpresas.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcEmpresas.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcEmpresas.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcEmpresas.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcEmpresas.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcEmpresas.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcEmpresas.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcEmpresas.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcEmpresas.Location = new System.Drawing.Point(7, 63);
            this.gcEmpresas.MainView = this.gvEmpresas;
            this.gcEmpresas.Name = "gcEmpresas";
            this.gcEmpresas.Size = new System.Drawing.Size(721, 111);
            this.gcEmpresas.TabIndex = 8;
            this.gcEmpresas.UseEmbeddedNavigator = true;
            this.gcEmpresas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvEmpresas});
            // 
            // gvEmpresas
            // 
            this.gvEmpresas.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.Empty.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvEmpresas.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvEmpresas.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvEmpresas.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvEmpresas.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvEmpresas.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvEmpresas.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.OddRow.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.OddRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvEmpresas.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.Preview.Options.UseFont = true;
            this.gvEmpresas.Appearance.Preview.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvEmpresas.Appearance.Row.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvEmpresas.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvEmpresas.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvEmpresas.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvEmpresas.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvEmpresas.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvEmpresas.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvEmpresas.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colunaIDEmpresa,
            this.colunaNomeEmpresa,
            this.colunaCodigoEmpresa});
            this.gvEmpresas.GridControl = this.gcEmpresas;
            this.gvEmpresas.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvEmpresas.Name = "gvEmpresas";
            this.gvEmpresas.OptionsBehavior.Editable = false;
            this.gvEmpresas.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvEmpresas.OptionsView.EnableAppearanceEvenRow = true;
            this.gvEmpresas.OptionsView.EnableAppearanceOddRow = true;
            this.gvEmpresas.OptionsView.ShowGroupPanel = false;
            this.gvEmpresas.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colunaNomeEmpresa, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvEmpresas.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gvEmpresas_SelectionChanged);
            // 
            // colunaIDEmpresa
            // 
            this.colunaIDEmpresa.Caption = "ID";
            this.colunaIDEmpresa.FieldName = "id";
            this.colunaIDEmpresa.Name = "colunaIDEmpresa";
            // 
            // colunaNomeEmpresa
            // 
            this.colunaNomeEmpresa.Caption = "Nome";
            this.colunaNomeEmpresa.FieldName = "nome";
            this.colunaNomeEmpresa.MinWidth = 180;
            this.colunaNomeEmpresa.Name = "colunaNomeEmpresa";
            this.colunaNomeEmpresa.Visible = true;
            this.colunaNomeEmpresa.VisibleIndex = 0;
            this.colunaNomeEmpresa.Width = 550;
            // 
            // colunaCodigoEmpresa
            // 
            this.colunaCodigoEmpresa.Caption = "Código";
            this.colunaCodigoEmpresa.FieldName = "codigo";
            this.colunaCodigoEmpresa.Name = "colunaCodigoEmpresa";
            this.colunaCodigoEmpresa.Visible = true;
            this.colunaCodigoEmpresa.VisibleIndex = 1;
            this.colunaCodigoEmpresa.Width = 150;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // sbExportar
            // 
            this.sbExportar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbExportar.Image = ((System.Drawing.Image)(resources.GetObject("sbExportar.Image")));
            this.sbExportar.ImageIndex = 0;
            this.sbExportar.Location = new System.Drawing.Point(591, 421);
            this.sbExportar.Name = "sbExportar";
            this.sbExportar.Size = new System.Drawing.Size(75, 23);
            this.sbExportar.TabIndex = 1;
            this.sbExportar.Text = "&Enviar";
            this.sbExportar.Click += new System.EventHandler(this.sbExportar_Click);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sbCancelar.ImageIndex = 1;
            this.sbCancelar.ImageList = this.imageList1;
            this.sbCancelar.Location = new System.Drawing.Point(672, 421);
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
            this.sbAjuda.Location = new System.Drawing.Point(12, 421);
            this.sbAjuda.Name = "sbAjuda";
            this.sbAjuda.Size = new System.Drawing.Size(75, 23);
            this.sbAjuda.TabIndex = 3;
            this.sbAjuda.Text = "A&juda";
            this.sbAjuda.Click += new System.EventHandler(this.sbAjuda_Click);
            // 
            // dxErrorProvider1
            // 
            this.dxErrorProvider1.ContainerControl = this;
            // 
            // sbExcluirFuncionarios
            // 
            this.sbExcluirFuncionarios.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbExcluirFuncionarios.Image = global::UI.Properties.Resources.Fechamento_de_compensacao_copy;
            this.sbExcluirFuncionarios.ImageIndex = 0;
            this.sbExcluirFuncionarios.Location = new System.Drawing.Point(432, 421);
            this.sbExcluirFuncionarios.Name = "sbExcluirFuncionarios";
            this.sbExcluirFuncionarios.Size = new System.Drawing.Size(153, 23);
            this.sbExcluirFuncionarios.TabIndex = 4;
            this.sbExcluirFuncionarios.Text = "E&xcluir Funcionario(s)";
            this.sbExcluirFuncionarios.Click += new System.EventHandler(this.sbExcluirFuncionarios_Click);
            // 
            // sbExportarArquivos
            // 
            this.sbExportarArquivos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sbExportarArquivos.Enabled = false;
            this.sbExportarArquivos.Image = global::UI.Properties.Resources.exportar16;
            this.sbExportarArquivos.ImageIndex = 0;
            this.sbExportarArquivos.Location = new System.Drawing.Point(273, 421);
            this.sbExportarArquivos.Name = "sbExportarArquivos";
            this.sbExportarArquivos.Size = new System.Drawing.Size(153, 23);
            this.sbExportarArquivos.TabIndex = 5;
            this.sbExportarArquivos.Text = "Expor&tar Arquivo";
            this.sbExportarArquivos.Click += new System.EventHandler(this.sbExportarArquivos_Click);
            // 
            // FormEnviarEmpresaEFuncionarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 449);
            this.Controls.Add(this.sbExportarArquivos);
            this.Controls.Add(this.sbExcluirFuncionarios);
            this.Controls.Add(this.sbAjuda);
            this.Controls.Add(this.sbExportar);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormEnviarEmpresaEFuncionarios";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "REP - Envio de Empresa e Funcionários";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormEnviarFuncionarios_FormClosed);
            this.Load += new System.EventHandler(this.FormEnviarFuncionarios_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormImportacaoBilhetes_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chGrupoEconomico.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbEnviarFuncionarios.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbEnviarEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdRep.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcFuncionarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvFuncionarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcEmpresas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvEmpresas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public DevExpress.XtraEditors.SimpleButton sbExportar;
        public DevExpress.XtraEditors.SimpleButton sbCancelar;
        public DevExpress.XtraEditors.SimpleButton sbAjuda;
        private System.Windows.Forms.ImageList imageList1;
        public DevExpress.XtraEditors.SimpleButton sbLimparEmpresas;
        public DevExpress.XtraEditors.SimpleButton sbSelecionarEmpresas;
        public DevExpress.XtraEditors.SimpleButton sbLimparFuncionarios;
        public DevExpress.XtraEditors.SimpleButton sbSelecionarFuncionarios;
        public DevExpress.XtraEditors.LabelControl labelControl3;
        public DevExpress.XtraGrid.GridControl gcFuncionarios;
        public DevExpress.XtraGrid.Views.Grid.GridView gvFuncionarios;
        private DevExpress.XtraGrid.Columns.GridColumn colunaIDFuncionario;
        private DevExpress.XtraGrid.Columns.GridColumn colunaNomeFuncionario;
        public DevExpress.XtraEditors.LabelControl labelControl2;
        public DevExpress.XtraGrid.GridControl gcEmpresas;
        public DevExpress.XtraGrid.Views.Grid.GridView gvEmpresas;
        private DevExpress.XtraGrid.Columns.GridColumn colunaIDEmpresa;
        private DevExpress.XtraGrid.Columns.GridColumn colunaNomeEmpresa;
        private DevExpress.XtraGrid.Columns.GridColumn colunaCodigoEmpresa;
        public DevExpress.XtraEditors.SimpleButton sbSelecionarFuncAtivos;
        private DevExpress.XtraGrid.Columns.GridColumn colunaFuncionarioAtivoFuncionario;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private Componentes.devexpress.cwk_DevButton sbIdRep;
        private Componentes.devexpress.cwk_DevLookup cbIdRep;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider dxErrorProvider1;
        private DevExpress.XtraGrid.Columns.GridColumn colunaCodigoFuncionario;
        private DevExpress.XtraEditors.CheckEdit chbEnviarFuncionarios;
        private DevExpress.XtraEditors.CheckEdit chbEnviarEmpresa;
        private DevExpress.XtraGrid.Columns.GridColumn colunaPisFuncionario;
        private DevExpress.XtraEditors.CheckEdit chGrupoEconomico;
        public DevExpress.XtraEditors.SimpleButton sbExcluirFuncionarios;
        public DevExpress.XtraEditors.SimpleButton sbExportarArquivos;

    }
}