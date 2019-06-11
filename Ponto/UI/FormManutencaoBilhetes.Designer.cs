namespace UI
{
    partial class FormManutencaoBilhetes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormManutencaoBilhetes));
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn5 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.sbAjuda = new DevExpress.XtraEditors.SimpleButton();
            this.sbAlterar = new DevExpress.XtraEditors.SimpleButton();
            this.sbFechar = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lblRelBilhete = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtHoraBilhete = new Componentes.devexpress.cwkEditHora();
            this.txtDataBilhete = new DevExpress.XtraEditors.DateEdit();
            this.lblDataCompensada = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.lblRelMarcacao = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtHoraMarcacao = new Componentes.devexpress.cwkEditHora();
            this.txtDataMarcacao = new DevExpress.XtraEditors.DateEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.sbCancelar = new DevExpress.XtraEditors.SimpleButton();
            this.sbGravar = new DevExpress.XtraEditors.SimpleButton();
            this.lblManutencao = new DevExpress.XtraEditors.LabelControl();
            this.cbManutencao = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblAtencao2 = new DevExpress.XtraEditors.LabelControl();
            this.lblAtencao1 = new DevExpress.XtraEditors.LabelControl();
            this.lblAtencao3 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHoraBilhete.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataBilhete.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataBilhete.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHoraMarcacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataMarcacao.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataMarcacao.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbManutencao.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridControl1.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gridControl1.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gridControl1.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gridControl1.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gridControl1.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gridControl1.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gridControl1.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridControl1.EmbeddedNavigator.Name = "";
            this.gridControl1.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridControl1.Location = new System.Drawing.Point(12, 12);
            this.gridControl1.LookAndFeel.UseWindowsXPTheme = true;
            this.gridControl1.MainView = this.bandedGridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(389, 282);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.UseEmbeddedNavigator = true;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bandedGridView1});
            this.gridControl1.DoubleClick += new System.EventHandler(this.gridControl1_DoubleClick);
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
            this.bandedGridView1.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.bandedGridView1.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.FocusedCell.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.FocusedCell.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.bandedGridView1.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.bandedGridView1.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.FocusedRow.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.FocusedRow.Options.UseBorderColor = true;
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
            this.bandedGridView1.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.bandedGridView1.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.HideSelectionRow.Options.UseBorderColor = true;
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
            this.bandedGridView1.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.bandedGridView1.Appearance.RowSeparator.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.bandedGridView1.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.bandedGridView1.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.SelectedRow.Options.UseBackColor = true;
            this.bandedGridView1.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.bandedGridView1.Appearance.SelectedRow.Options.UseForeColor = true;
            this.bandedGridView1.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.bandedGridView1.Appearance.TopNewRow.Options.UseBackColor = true;
            this.bandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2});
            this.bandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.bandedGridColumn1,
            this.bandedGridColumn2,
            this.bandedGridColumn3,
            this.bandedGridColumn4,
            this.bandedGridColumn5});
            this.bandedGridView1.GridControl = this.gridControl1;
            this.bandedGridView1.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.bandedGridView1.Name = "bandedGridView1";
            this.bandedGridView1.OptionsBehavior.AllowIncrementalSearch = true;
            this.bandedGridView1.OptionsBehavior.Editable = false;
            this.bandedGridView1.OptionsNavigation.UseTabKey = false;
            this.bandedGridView1.OptionsView.ColumnAutoWidth = false;
            this.bandedGridView1.OptionsView.EnableAppearanceEvenRow = true;
            this.bandedGridView1.OptionsView.EnableAppearanceOddRow = true;
            this.bandedGridView1.OptionsView.ShowAutoFilterRow = true;
            this.bandedGridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand1.Caption = "Bilhete";
            this.gridBand1.Columns.Add(this.gridColumn1);
            this.gridBand1.Columns.Add(this.gridColumn2);
            this.gridBand1.Columns.Add(this.bandedGridColumn1);
            this.gridBand1.Columns.Add(this.bandedGridColumn2);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.Width = 184;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "ID";
            this.gridColumn1.FieldName = "Id";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Data";
            this.gridColumn2.FieldName = "Data";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            // 
            // bandedGridColumn1
            // 
            this.bandedGridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn1.Caption = "Hora";
            this.bandedGridColumn1.FieldName = "Hora";
            this.bandedGridColumn1.Name = "bandedGridColumn1";
            this.bandedGridColumn1.Visible = true;
            this.bandedGridColumn1.Width = 50;
            // 
            // bandedGridColumn2
            // 
            this.bandedGridColumn2.Caption = "Relógio";
            this.bandedGridColumn2.FieldName = "Relogio";
            this.bandedGridColumn2.Name = "bandedGridColumn2";
            this.bandedGridColumn2.Visible = true;
            this.bandedGridColumn2.Width = 59;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "Marcação";
            this.gridBand2.Columns.Add(this.bandedGridColumn3);
            this.gridBand2.Columns.Add(this.bandedGridColumn4);
            this.gridBand2.Columns.Add(this.bandedGridColumn5);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.Width = 184;
            // 
            // bandedGridColumn3
            // 
            this.bandedGridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn3.Caption = "Data";
            this.bandedGridColumn3.FieldName = "Mar_data";
            this.bandedGridColumn3.Name = "bandedGridColumn3";
            this.bandedGridColumn3.Visible = true;
            // 
            // bandedGridColumn4
            // 
            this.bandedGridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn4.Caption = "Hora";
            this.bandedGridColumn4.FieldName = "Mar_hora";
            this.bandedGridColumn4.Name = "bandedGridColumn4";
            this.bandedGridColumn4.Visible = true;
            this.bandedGridColumn4.Width = 50;
            // 
            // bandedGridColumn5
            // 
            this.bandedGridColumn5.Caption = "Relógio";
            this.bandedGridColumn5.FieldName = "Mar_relogio";
            this.bandedGridColumn5.Name = "bandedGridColumn5";
            this.bandedGridColumn5.Visible = true;
            this.bandedGridColumn5.Width = 59;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Logo.ico");
            this.imageList1.Images.SetKeyName(1, "Help copy.ico");
            this.imageList1.Images.SetKeyName(2, "Selecionar copy.ico");
            this.imageList1.Images.SetKeyName(3, "Consulta copy.ico");
            this.imageList1.Images.SetKeyName(4, "Inserir copy.ico");
            this.imageList1.Images.SetKeyName(5, "Alterar copy.ico");
            this.imageList1.Images.SetKeyName(6, "Excluir copy.ico");
            this.imageList1.Images.SetKeyName(7, "Fechar.ico");
            this.imageList1.Images.SetKeyName(8, "Gravar copy.ico");
            this.imageList1.Images.SetKeyName(9, "cancelar copy.ico");
            // 
            // sbAjuda
            // 
            this.sbAjuda.ImageIndex = 1;
            this.sbAjuda.ImageList = this.imageList1;
            this.sbAjuda.Location = new System.Drawing.Point(10, 300);
            this.sbAjuda.Name = "sbAjuda";
            this.sbAjuda.Size = new System.Drawing.Size(75, 23);
            this.sbAjuda.TabIndex = 2;
            this.sbAjuda.Text = "A&juda";
            this.sbAjuda.Click += new System.EventHandler(this.sbAjuda_Click);
            // 
            // sbAlterar
            // 
            this.sbAlterar.ImageIndex = 5;
            this.sbAlterar.ImageList = this.imageList1;
            this.sbAlterar.Location = new System.Drawing.Point(327, 300);
            this.sbAlterar.Name = "sbAlterar";
            this.sbAlterar.Size = new System.Drawing.Size(75, 23);
            this.sbAlterar.TabIndex = 3;
            this.sbAlterar.Text = "&Alterar";
            this.sbAlterar.Click += new System.EventHandler(this.sbAlterar_Click);
            // 
            // sbFechar
            // 
            this.sbFechar.ImageIndex = 7;
            this.sbFechar.ImageList = this.imageList1;
            this.sbFechar.Location = new System.Drawing.Point(541, 300);
            this.sbFechar.Name = "sbFechar";
            this.sbFechar.Size = new System.Drawing.Size(75, 23);
            this.sbFechar.TabIndex = 4;
            this.sbFechar.Text = "&Fechar";
            this.sbFechar.Click += new System.EventHandler(this.sbFechar_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lblRelBilhete);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.txtHoraBilhete);
            this.groupControl1.Controls.Add(this.txtDataBilhete);
            this.groupControl1.Controls.Add(this.lblDataCompensada);
            this.groupControl1.Location = new System.Drawing.Point(407, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(220, 79);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "Bilhete";
            // 
            // lblRelBilhete
            // 
            this.lblRelBilhete.Appearance.Options.UseTextOptions = true;
            this.lblRelBilhete.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblRelBilhete.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblRelBilhete.Location = new System.Drawing.Point(177, 56);
            this.lblRelBilhete.Name = "lblRelBilhete";
            this.lblRelBilhete.Size = new System.Drawing.Size(28, 13);
            this.lblRelBilhete.TabIndex = 9;
            this.lblRelBilhete.Text = "*";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(141, 33);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(27, 13);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "Hora:";
            // 
            // txtHoraBilhete
            // 
            this.txtHoraBilhete.cwErro = false;
            this.txtHoraBilhete.EditValue = "--:--";
            this.txtHoraBilhete.EnterMoveNextControl = true;
            this.txtHoraBilhete.Layout = Componentes.devexpress.LayoutsHorario.horario2Digitos;
            this.txtHoraBilhete.lblLegenda = null;
            this.txtHoraBilhete.lblNRelogio = null;
            this.txtHoraBilhete.Location = new System.Drawing.Point(174, 30);
            this.txtHoraBilhete.Name = "txtHoraBilhete";
            this.txtHoraBilhete.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtHoraBilhete.Properties.Appearance.Options.UseBackColor = true;
            this.txtHoraBilhete.Properties.Appearance.Options.UseTextOptions = true;
            this.txtHoraBilhete.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtHoraBilhete.Properties.MaxLength = 5;
            this.txtHoraBilhete.Properties.ReadOnly = true;
            this.txtHoraBilhete.Size = new System.Drawing.Size(35, 20);
            this.txtHoraBilhete.TabIndex = 7;
            this.txtHoraBilhete.ValorAnterior = null;
            // 
            // txtDataBilhete
            // 
            this.txtDataBilhete.EditValue = null;
            this.txtDataBilhete.Location = new System.Drawing.Point(51, 30);
            this.txtDataBilhete.Name = "txtDataBilhete";
            this.txtDataBilhete.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtDataBilhete.Properties.Appearance.Options.UseBackColor = true;
            this.txtDataBilhete.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataBilhete.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataBilhete.Properties.ReadOnly = true;
            this.txtDataBilhete.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataBilhete.Size = new System.Drawing.Size(80, 20);
            this.txtDataBilhete.TabIndex = 5;
            // 
            // lblDataCompensada
            // 
            this.lblDataCompensada.Location = new System.Drawing.Point(18, 33);
            this.lblDataCompensada.Name = "lblDataCompensada";
            this.lblDataCompensada.Size = new System.Drawing.Size(27, 13);
            this.lblDataCompensada.TabIndex = 4;
            this.lblDataCompensada.Text = "Data:";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.lblRelMarcacao);
            this.groupControl2.Controls.Add(this.labelControl4);
            this.groupControl2.Controls.Add(this.txtHoraMarcacao);
            this.groupControl2.Controls.Add(this.txtDataMarcacao);
            this.groupControl2.Controls.Add(this.labelControl5);
            this.groupControl2.Location = new System.Drawing.Point(407, 97);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(220, 79);
            this.groupControl2.TabIndex = 6;
            this.groupControl2.Text = "Marcação";
            // 
            // lblRelMarcacao
            // 
            this.lblRelMarcacao.Appearance.Options.UseTextOptions = true;
            this.lblRelMarcacao.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblRelMarcacao.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblRelMarcacao.Location = new System.Drawing.Point(178, 56);
            this.lblRelMarcacao.Name = "lblRelMarcacao";
            this.lblRelMarcacao.Size = new System.Drawing.Size(28, 13);
            this.lblRelMarcacao.TabIndex = 14;
            this.lblRelMarcacao.Text = "*";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(142, 33);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(27, 13);
            this.labelControl4.TabIndex = 13;
            this.labelControl4.Text = "Hora:";
            // 
            // txtHoraMarcacao
            // 
            this.txtHoraMarcacao.cwErro = false;
            this.txtHoraMarcacao.EditValue = "--:--";
            this.txtHoraMarcacao.EnterMoveNextControl = true;
            this.txtHoraMarcacao.Layout = Componentes.devexpress.LayoutsHorario.horario2Digitos;
            this.txtHoraMarcacao.lblLegenda = null;
            this.txtHoraMarcacao.lblNRelogio = null;
            this.txtHoraMarcacao.Location = new System.Drawing.Point(175, 30);
            this.txtHoraMarcacao.Name = "txtHoraMarcacao";
            this.txtHoraMarcacao.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtHoraMarcacao.Properties.Appearance.Options.UseBackColor = true;
            this.txtHoraMarcacao.Properties.Appearance.Options.UseTextOptions = true;
            this.txtHoraMarcacao.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtHoraMarcacao.Properties.MaxLength = 5;
            this.txtHoraMarcacao.Properties.ReadOnly = true;
            this.txtHoraMarcacao.Size = new System.Drawing.Size(35, 20);
            this.txtHoraMarcacao.TabIndex = 12;
            this.txtHoraMarcacao.ValorAnterior = null;
            // 
            // txtDataMarcacao
            // 
            this.txtDataMarcacao.EditValue = null;
            this.txtDataMarcacao.Location = new System.Drawing.Point(51, 30);
            this.txtDataMarcacao.Name = "txtDataMarcacao";
            this.txtDataMarcacao.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtDataMarcacao.Properties.Appearance.Options.UseBackColor = true;
            this.txtDataMarcacao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataMarcacao.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataMarcacao.Properties.ReadOnly = true;
            this.txtDataMarcacao.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataMarcacao.Size = new System.Drawing.Size(80, 20);
            this.txtDataMarcacao.TabIndex = 11;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(18, 33);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(27, 13);
            this.labelControl5.TabIndex = 10;
            this.labelControl5.Text = "Data:";
            // 
            // sbCancelar
            // 
            this.sbCancelar.ImageIndex = 9;
            this.sbCancelar.ImageList = this.imageList1;
            this.sbCancelar.Location = new System.Drawing.Point(538, 271);
            this.sbCancelar.Name = "sbCancelar";
            this.sbCancelar.Size = new System.Drawing.Size(75, 23);
            this.sbCancelar.TabIndex = 7;
            this.sbCancelar.Text = "&Cancelar";
            this.sbCancelar.Visible = false;
            this.sbCancelar.Click += new System.EventHandler(this.sbCancelar_Click);
            // 
            // sbGravar
            // 
            this.sbGravar.ImageIndex = 8;
            this.sbGravar.ImageList = this.imageList1;
            this.sbGravar.Location = new System.Drawing.Point(460, 271);
            this.sbGravar.Name = "sbGravar";
            this.sbGravar.Size = new System.Drawing.Size(75, 23);
            this.sbGravar.TabIndex = 8;
            this.sbGravar.Text = "&Gravar";
            this.sbGravar.Visible = false;
            this.sbGravar.Click += new System.EventHandler(this.sbGravar_Click);
            // 
            // lblManutencao
            // 
            this.lblManutencao.Location = new System.Drawing.Point(409, 182);
            this.lblManutencao.Name = "lblManutencao";
            this.lblManutencao.Size = new System.Drawing.Size(63, 13);
            this.lblManutencao.TabIndex = 9;
            this.lblManutencao.Text = "Manutenção:";
            this.lblManutencao.Visible = false;
            // 
            // cbManutencao
            // 
            this.cbManutencao.Location = new System.Drawing.Point(478, 179);
            this.cbManutencao.Name = "cbManutencao";
            this.cbManutencao.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbManutencao.Properties.Items.AddRange(new object[] {
            "Dia Anterior",
            "Mesmo Dia",
            "Dia Posterior"});
            this.cbManutencao.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbManutencao.Size = new System.Drawing.Size(108, 20);
            this.cbManutencao.TabIndex = 10;
            this.cbManutencao.Visible = false;
            // 
            // lblAtencao2
            // 
            this.lblAtencao2.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAtencao2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblAtencao2.Appearance.Options.UseFont = true;
            this.lblAtencao2.Appearance.Options.UseForeColor = true;
            this.lblAtencao2.Location = new System.Drawing.Point(456, 224);
            this.lblAtencao2.Name = "lblAtencao2";
            this.lblAtencao2.Size = new System.Drawing.Size(146, 13);
            this.lblAtencao2.TabIndex = 11;
            this.lblAtencao2.Text = "A manutenção é baseada ";
            this.lblAtencao2.Visible = false;
            // 
            // lblAtencao1
            // 
            this.lblAtencao1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAtencao1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblAtencao1.Appearance.Options.UseFont = true;
            this.lblAtencao1.Appearance.Options.UseForeColor = true;
            this.lblAtencao1.Location = new System.Drawing.Point(501, 205);
            this.lblAtencao1.Name = "lblAtencao1";
            this.lblAtencao1.Size = new System.Drawing.Size(56, 13);
            this.lblAtencao1.TabIndex = 12;
            this.lblAtencao1.Text = "Atenção!!!";
            this.lblAtencao1.Visible = false;
            // 
            // lblAtencao3
            // 
            this.lblAtencao3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAtencao3.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblAtencao3.Appearance.Options.UseFont = true;
            this.lblAtencao3.Appearance.Options.UseForeColor = true;
            this.lblAtencao3.Location = new System.Drawing.Point(477, 240);
            this.lblAtencao3.Name = "lblAtencao3";
            this.lblAtencao3.Size = new System.Drawing.Size(105, 13);
            this.lblAtencao3.TabIndex = 13;
            this.lblAtencao3.Text = "na data do bilhete.";
            this.lblAtencao3.Visible = false;
            // 
            // FormManutencaoBilhetes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 329);
            this.Controls.Add(this.lblAtencao3);
            this.Controls.Add(this.lblAtencao1);
            this.Controls.Add(this.lblAtencao2);
            this.Controls.Add(this.cbManutencao);
            this.Controls.Add(this.lblManutencao);
            this.Controls.Add(this.sbGravar);
            this.Controls.Add(this.sbCancelar);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.sbFechar);
            this.Controls.Add(this.sbAlterar);
            this.Controls.Add(this.sbAjuda);
            this.Controls.Add(this.gridControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormManutencaoBilhetes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manutenção de Bilhetes";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormManutencaoBilhetes_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bandedGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHoraBilhete.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataBilhete.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataBilhete.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtHoraMarcacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataMarcacao.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataMarcacao.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbManutencao.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraGrid.GridControl gridControl1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.SimpleButton sbAjuda;
        private DevExpress.XtraEditors.SimpleButton sbAlterar;
        private DevExpress.XtraEditors.SimpleButton sbFechar;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton sbCancelar;
        private DevExpress.XtraEditors.SimpleButton sbGravar;
        private DevExpress.XtraEditors.LabelControl lblManutencao;
        private DevExpress.XtraEditors.ComboBoxEdit cbManutencao;
        private DevExpress.XtraEditors.DateEdit txtDataBilhete;
        private DevExpress.XtraEditors.LabelControl lblDataCompensada;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private Componentes.devexpress.cwkEditHora txtHoraBilhete;
        private DevExpress.XtraEditors.LabelControl lblRelBilhete;
        private DevExpress.XtraEditors.LabelControl lblRelMarcacao;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private Componentes.devexpress.cwkEditHora txtHoraMarcacao;
        private DevExpress.XtraEditors.DateEdit txtDataMarcacao;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl lblAtencao2;
        private DevExpress.XtraEditors.LabelControl lblAtencao1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bandedGridView1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn2;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn4;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn5;
        private DevExpress.XtraEditors.LabelControl lblAtencao3;
    }
}