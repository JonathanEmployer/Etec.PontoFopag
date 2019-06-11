namespace UI
{
    partial class FormManutFechamentoBHD
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
            this.lblFechamentoBH = new DevExpress.XtraEditors.LabelControl();
            this.lblSeq = new DevExpress.XtraEditors.LabelControl();
            this.txtSeq = new DevExpress.XtraEditors.TextEdit();
            this.lblIdentificacao = new DevExpress.XtraEditors.LabelControl();
            this.lblCredito = new DevExpress.XtraEditors.LabelControl();
            this.lblDebito = new DevExpress.XtraEditors.LabelControl();
            this.lblHorasPagas = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.rgTiposaldo = new DevExpress.XtraEditors.RadioGroup();
            this.txtCredito = new DevExpress.XtraEditors.TextEdit();
            this.txtDebito = new DevExpress.XtraEditors.TextEdit();
            this.lblSaldoBh = new DevExpress.XtraEditors.LabelControl();
            this.txtSaldoBh = new DevExpress.XtraEditors.TextEdit();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.txtNome = new DevExpress.XtraEditors.TextEdit();
            this.txtHorasPagas = new Componentes.devexpress.cwkEditHora();
            this.lblBancoHorasPct = new DevExpress.XtraEditors.LabelControl();
            this.gvServicos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Empresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Servico = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Qtde = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Inicio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Total = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView4 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcPercentualBH = new DevExpress.XtraGrid.GridControl();
            this.gvPercentualBH = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Percentual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CreditoPercentual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DebitoPercentual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.SaldoPercentual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.HorasPagasPercentual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCalcEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeq.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTiposaldo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCredito.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebito.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldoBh.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNome.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHorasPagas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvServicos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPercentualBH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPercentualBH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(519, 358);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gcPercentualBH);
            this.xtraTabPage1.Controls.Add(this.lblBancoHorasPct);
            this.xtraTabPage1.Controls.Add(this.txtHorasPagas);
            this.xtraTabPage1.Controls.Add(this.txtNome);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.txtSaldoBh);
            this.xtraTabPage1.Controls.Add(this.lblSaldoBh);
            this.xtraTabPage1.Controls.Add(this.txtDebito);
            this.xtraTabPage1.Controls.Add(this.txtCredito);
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Controls.Add(this.lblDebito);
            this.xtraTabPage1.Controls.Add(this.lblHorasPagas);
            this.xtraTabPage1.Controls.Add(this.lblCredito);
            this.xtraTabPage1.Controls.Add(this.lblIdentificacao);
            this.xtraTabPage1.Controls.Add(this.txtSeq);
            this.xtraTabPage1.Controls.Add(this.lblSeq);
            this.xtraTabPage1.Controls.Add(this.lblFechamentoBH);
            this.xtraTabPage1.Size = new System.Drawing.Size(510, 349);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(456, 376);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(375, 376);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 376);
            // 
            // lblFechamentoBH
            // 
            this.lblFechamentoBH.Location = new System.Drawing.Point(0, 16);
            this.lblFechamentoBH.Name = "lblFechamentoBH";
            this.lblFechamentoBH.Size = new System.Drawing.Size(79, 13);
            this.lblFechamentoBH.TabIndex = 0;
            this.lblFechamentoBH.Text = "Fechamento BH:";
            // 
            // lblSeq
            // 
            this.lblSeq.Location = new System.Drawing.Point(384, 16);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(24, 13);
            this.lblSeq.TabIndex = 2;
            this.lblSeq.Text = "SEQ:";
            // 
            // txtSeq
            // 
            this.txtSeq.Location = new System.Drawing.Point(414, 13);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.txtSeq.Properties.ReadOnly = true;
            this.txtSeq.Size = new System.Drawing.Size(80, 20);
            this.txtSeq.TabIndex = 3;
            // 
            // lblIdentificacao
            // 
            this.lblIdentificacao.Location = new System.Drawing.Point(13, 42);
            this.lblIdentificacao.Name = "lblIdentificacao";
            this.lblIdentificacao.Size = new System.Drawing.Size(66, 13);
            this.lblIdentificacao.TabIndex = 4;
            this.lblIdentificacao.Text = "Identificação:";
            // 
            // lblCredito
            // 
            this.lblCredito.Location = new System.Drawing.Point(40, 66);
            this.lblCredito.Name = "lblCredito";
            this.lblCredito.Size = new System.Drawing.Size(39, 13);
            this.lblCredito.TabIndex = 6;
            this.lblCredito.Text = "Crédito:";
            // 
            // lblDebito
            // 
            this.lblDebito.Location = new System.Drawing.Point(44, 91);
            this.lblDebito.Name = "lblDebito";
            this.lblDebito.Size = new System.Drawing.Size(35, 13);
            this.lblDebito.TabIndex = 8;
            this.lblDebito.Text = "Débito:";
            // 
            // lblHorasPagas
            // 
            this.lblHorasPagas.Location = new System.Drawing.Point(15, 122);
            this.lblHorasPagas.Name = "lblHorasPagas";
            this.lblHorasPagas.Size = new System.Drawing.Size(64, 13);
            this.lblHorasPagas.TabIndex = 11;
            this.lblHorasPagas.Text = "Horas Pagas:";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.rgTiposaldo);
            this.groupControl1.Location = new System.Drawing.Point(248, 65);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(246, 46);
            this.groupControl1.TabIndex = 10;
            this.groupControl1.Text = "Tipo";
            // 
            // rgTiposaldo
            // 
            this.rgTiposaldo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgTiposaldo.Location = new System.Drawing.Point(2, 20);
            this.rgTiposaldo.Name = "rgTiposaldo";
            this.rgTiposaldo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Crédito"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Débito")});
            this.rgTiposaldo.Properties.ReadOnly = true;
            this.rgTiposaldo.Size = new System.Drawing.Size(242, 24);
            this.rgTiposaldo.TabIndex = 0;
            // 
            // txtCredito
            // 
            this.txtCredito.Location = new System.Drawing.Point(86, 65);
            this.txtCredito.Name = "txtCredito";
            this.txtCredito.Properties.ReadOnly = true;
            this.txtCredito.Size = new System.Drawing.Size(156, 20);
            this.txtCredito.TabIndex = 7;
            // 
            // txtDebito
            // 
            this.txtDebito.Location = new System.Drawing.Point(86, 91);
            this.txtDebito.Name = "txtDebito";
            this.txtDebito.Properties.ReadOnly = true;
            this.txtDebito.Size = new System.Drawing.Size(156, 20);
            this.txtDebito.TabIndex = 9;
            // 
            // lblSaldoBh
            // 
            this.lblSaldoBh.Location = new System.Drawing.Point(309, 120);
            this.lblSaldoBh.Name = "lblSaldoBh";
            this.lblSaldoBh.Size = new System.Drawing.Size(46, 13);
            this.lblSaldoBh.TabIndex = 13;
            this.lblSaldoBh.Text = "Saldo BH:";
            // 
            // txtSaldoBh
            // 
            this.txtSaldoBh.Location = new System.Drawing.Point(368, 119);
            this.txtSaldoBh.Name = "txtSaldoBh";
            this.txtSaldoBh.Properties.ReadOnly = true;
            this.txtSaldoBh.Size = new System.Drawing.Size(126, 20);
            this.txtSaldoBh.TabIndex = 14;
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(86, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Properties.ReadOnly = true;
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // txtNome
            // 
            this.txtNome.Location = new System.Drawing.Point(86, 39);
            this.txtNome.Name = "txtNome";
            this.txtNome.Properties.ReadOnly = true;
            this.txtNome.Size = new System.Drawing.Size(408, 20);
            this.txtNome.TabIndex = 5;
            // 
            // txtHorasPagas
            // 
            this.txtHorasPagas.cwErro = false;
            this.txtHorasPagas.EditValue = "-----:--";
            this.txtHorasPagas.Layout = Componentes.devexpress.LayoutsHorario.horario5Digitos;
            this.txtHorasPagas.lblLegenda = null;
            this.txtHorasPagas.lblNRelogio = null;
            this.txtHorasPagas.Location = new System.Drawing.Point(85, 117);
            this.txtHorasPagas.Name = "txtHorasPagas";
            this.txtHorasPagas.Properties.Appearance.Options.UseTextOptions = true;
            this.txtHorasPagas.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtHorasPagas.Properties.MaxLength = 8;
            this.txtHorasPagas.Size = new System.Drawing.Size(52, 20);
            this.txtHorasPagas.TabIndex = 12;
            this.txtHorasPagas.ValorAnterior = null;
            this.txtHorasPagas.Validated += new System.EventHandler(this.txtHorasPagas_Validated);
            // 
            // lblBancoHorasPct
            // 
            this.lblBancoHorasPct.Location = new System.Drawing.Point(15, 148);
            this.lblBancoHorasPct.Name = "lblBancoHorasPct";
            this.lblBancoHorasPct.Size = new System.Drawing.Size(89, 13);
            this.lblBancoHorasPct.TabIndex = 44;
            this.lblBancoHorasPct.Text = "% Banco de Horas";
            // 
            // gvServicos
            // 
            this.gvServicos.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvServicos.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvServicos.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvServicos.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvServicos.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvServicos.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvServicos.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvServicos.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvServicos.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvServicos.Appearance.Empty.Options.UseBackColor = true;
            this.gvServicos.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvServicos.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvServicos.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvServicos.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvServicos.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvServicos.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvServicos.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvServicos.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvServicos.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvServicos.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvServicos.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvServicos.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvServicos.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvServicos.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvServicos.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvServicos.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvServicos.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvServicos.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvServicos.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvServicos.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvServicos.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvServicos.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvServicos.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvServicos.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvServicos.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvServicos.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvServicos.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvServicos.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvServicos.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvServicos.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvServicos.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvServicos.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvServicos.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvServicos.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvServicos.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvServicos.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvServicos.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvServicos.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvServicos.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvServicos.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvServicos.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvServicos.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvServicos.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvServicos.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvServicos.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvServicos.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvServicos.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvServicos.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvServicos.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvServicos.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvServicos.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvServicos.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvServicos.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvServicos.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvServicos.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvServicos.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvServicos.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvServicos.Appearance.OddRow.Options.UseBackColor = true;
            this.gvServicos.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvServicos.Appearance.OddRow.Options.UseForeColor = true;
            this.gvServicos.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvServicos.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.Preview.Options.UseFont = true;
            this.gvServicos.Appearance.Preview.Options.UseForeColor = true;
            this.gvServicos.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvServicos.Appearance.Row.Options.UseBackColor = true;
            this.gvServicos.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvServicos.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvServicos.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvServicos.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvServicos.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvServicos.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvServicos.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvServicos.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvServicos.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvServicos.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvServicos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Empresa,
            this.Servico,
            this.Qtde,
            this.Inicio,
            this.Total});
            this.gvServicos.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvServicos.Name = "gvServicos";
            this.gvServicos.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvServicos.OptionsBehavior.Editable = false;
            this.gvServicos.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvServicos.OptionsView.EnableAppearanceEvenRow = true;
            this.gvServicos.OptionsView.EnableAppearanceOddRow = true;
            this.gvServicos.OptionsView.ShowAutoFilterRow = true;
            this.gvServicos.OptionsView.ShowGroupPanel = false;
            // 
            // Empresa
            // 
            this.Empresa.Caption = "Empresa";
            this.Empresa.FieldName = "PessoaServico";
            this.Empresa.Name = "Empresa";
            this.Empresa.Visible = true;
            this.Empresa.VisibleIndex = 0;
            this.Empresa.Width = 236;
            // 
            // Servico
            // 
            this.Servico.Caption = "Serviço";
            this.Servico.FieldName = "Servico";
            this.Servico.Name = "Servico";
            this.Servico.Visible = true;
            this.Servico.VisibleIndex = 1;
            this.Servico.Width = 226;
            // 
            // Qtde
            // 
            this.Qtde.AppearanceHeader.Options.UseTextOptions = true;
            this.Qtde.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Qtde.Caption = "Qtde";
            this.Qtde.FieldName = "Quantidade";
            this.Qtde.Name = "Qtde";
            this.Qtde.Visible = true;
            this.Qtde.VisibleIndex = 2;
            this.Qtde.Width = 67;
            // 
            // Inicio
            // 
            this.Inicio.AppearanceCell.Options.UseTextOptions = true;
            this.Inicio.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Inicio.AppearanceHeader.Options.UseTextOptions = true;
            this.Inicio.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Inicio.Caption = "Início";
            this.Inicio.FieldName = "DataInicioFaturamento";
            this.Inicio.Name = "Inicio";
            this.Inicio.Visible = true;
            this.Inicio.VisibleIndex = 3;
            // 
            // Total
            // 
            this.Total.AppearanceHeader.Options.UseTextOptions = true;
            this.Total.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Total.Caption = "Total";
            this.Total.DisplayFormat.FormatString = "c2";
            this.Total.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Total.FieldName = "Total";
            this.Total.Name = "Total";
            this.Total.Visible = true;
            this.Total.VisibleIndex = 4;
            this.Total.Width = 113;
            // 
            // gridView4
            // 
            this.gridView4.Name = "gridView4";
            // 
            // gcPercentualBH
            // 
            this.gcPercentualBH.EmbeddedNavigator.Name = "";
            this.gcPercentualBH.Location = new System.Drawing.Point(13, 167);
            this.gcPercentualBH.MainView = this.gvPercentualBH;
            this.gcPercentualBH.Name = "gcPercentualBH";
            this.gcPercentualBH.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCalcEdit1});
            this.gcPercentualBH.Size = new System.Drawing.Size(481, 168);
            this.gcPercentualBH.TabIndex = 45;
            this.gcPercentualBH.TabStop = false;
            this.gcPercentualBH.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPercentualBH,
            this.gridView2});
            // 
            // gvPercentualBH
            // 
            this.gvPercentualBH.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvPercentualBH.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.Empty.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvPercentualBH.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvPercentualBH.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvPercentualBH.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvPercentualBH.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvPercentualBH.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvPercentualBH.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvPercentualBH.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvPercentualBH.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvPercentualBH.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvPercentualBH.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvPercentualBH.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvPercentualBH.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvPercentualBH.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvPercentualBH.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvPercentualBH.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvPercentualBH.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvPercentualBH.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvPercentualBH.Appearance.OddRow.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.OddRow.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvPercentualBH.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.Preview.Options.UseFont = true;
            this.gvPercentualBH.Appearance.Preview.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvPercentualBH.Appearance.Row.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvPercentualBH.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvPercentualBH.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvPercentualBH.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvPercentualBH.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvPercentualBH.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvPercentualBH.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvPercentualBH.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvPercentualBH.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Percentual,
            this.CreditoPercentual,
            this.DebitoPercentual,
            this.SaldoPercentual,
            this.HorasPagasPercentual});
            this.gvPercentualBH.GridControl = this.gcPercentualBH;
            this.gvPercentualBH.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvPercentualBH.Name = "gvPercentualBH";
            this.gvPercentualBH.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvPercentualBH.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvPercentualBH.OptionsView.EnableAppearanceEvenRow = true;
            this.gvPercentualBH.OptionsView.EnableAppearanceOddRow = true;
            this.gvPercentualBH.OptionsView.ShowAutoFilterRow = true;
            this.gvPercentualBH.OptionsView.ShowGroupPanel = false;
            this.gvPercentualBH.RowUpdated += new DevExpress.XtraGrid.Views.Base.RowObjectEventHandler(this.gvPercentualBH_RowUpdated);
            this.gvPercentualBH.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.gvPercentualBH_InvalidRowException);
            this.gvPercentualBH.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gvPercentualBH_ValidateRow);
            // 
            // Percentual
            // 
            this.Percentual.Caption = "Percentual";
            this.Percentual.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.Percentual.FieldName = "Percentual";
            this.Percentual.Name = "Percentual";
            this.Percentual.OptionsColumn.ReadOnly = true;
            this.Percentual.Visible = true;
            this.Percentual.VisibleIndex = 0;
            this.Percentual.Width = 90;
            // 
            // CreditoPercentual
            // 
            this.CreditoPercentual.Caption = "Crédito";
            this.CreditoPercentual.FieldName = "CreditoPercentual";
            this.CreditoPercentual.Name = "CreditoPercentual";
            this.CreditoPercentual.OptionsColumn.ReadOnly = true;
            this.CreditoPercentual.Visible = true;
            this.CreditoPercentual.VisibleIndex = 1;
            // 
            // DebitoPercentual
            // 
            this.DebitoPercentual.Caption = "Débito";
            this.DebitoPercentual.FieldName = "DebitoPercentual";
            this.DebitoPercentual.Name = "DebitoPercentual";
            this.DebitoPercentual.OptionsColumn.ReadOnly = true;
            this.DebitoPercentual.Visible = true;
            this.DebitoPercentual.VisibleIndex = 2;
            // 
            // SaldoPercentual
            // 
            this.SaldoPercentual.Caption = "Saldo";
            this.SaldoPercentual.FieldName = "SaldoPercentual";
            this.SaldoPercentual.Name = "SaldoPercentual";
            this.SaldoPercentual.OptionsColumn.ReadOnly = true;
            this.SaldoPercentual.Visible = true;
            this.SaldoPercentual.VisibleIndex = 3;
            this.SaldoPercentual.Width = 82;
            // 
            // HorasPagasPercentual
            // 
            this.HorasPagasPercentual.AppearanceHeader.Options.UseTextOptions = true;
            this.HorasPagasPercentual.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.HorasPagasPercentual.Caption = "Horas Pagas";
            this.HorasPagasPercentual.FieldName = "HorasPagasPercentual";
            this.HorasPagasPercentual.Name = "HorasPagasPercentual";
            this.HorasPagasPercentual.Visible = true;
            this.HorasPagasPercentual.VisibleIndex = 4;
            this.HorasPagasPercentual.Width = 136;
            // 
            // repositoryItemCalcEdit1
            // 
            this.repositoryItemCalcEdit1.AutoHeight = false;
            this.repositoryItemCalcEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemCalcEdit1.Name = "repositoryItemCalcEdit1";
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gcPercentualBH;
            this.gridView2.Name = "gridView2";
            // 
            // FormManutFechamentoBHD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(543, 411);
            this.Name = "FormManutFechamentoBHD";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeq.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTiposaldo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCredito.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDebito.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSaldoBh.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNome.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHorasPagas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvServicos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPercentualBH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPercentualBH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCalcEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblSeq;
        private DevExpress.XtraEditors.LabelControl lblFechamentoBH;
        private DevExpress.XtraEditors.TextEdit txtSeq;
        private DevExpress.XtraEditors.LabelControl lblIdentificacao;
        private DevExpress.XtraEditors.LabelControl lblHorasPagas;
        private DevExpress.XtraEditors.LabelControl lblDebito;
        private DevExpress.XtraEditors.LabelControl lblCredito;
        private DevExpress.XtraEditors.TextEdit txtCredito;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.RadioGroup rgTiposaldo;
        private DevExpress.XtraEditors.TextEdit txtSaldoBh;
        private DevExpress.XtraEditors.LabelControl lblSaldoBh;
        private DevExpress.XtraEditors.TextEdit txtDebito;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.TextEdit txtNome;
        private Componentes.devexpress.cwkEditHora txtHorasPagas;
        private DevExpress.XtraEditors.LabelControl lblBancoHorasPct;
        private DevExpress.XtraGrid.Views.Grid.GridView gvServicos;
        private DevExpress.XtraGrid.Columns.GridColumn Empresa;
        private DevExpress.XtraGrid.Columns.GridColumn Servico;
        private DevExpress.XtraGrid.Columns.GridColumn Qtde;
        private DevExpress.XtraGrid.Columns.GridColumn Inicio;
        private DevExpress.XtraGrid.Columns.GridColumn Total;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView4;
        private DevExpress.XtraGrid.GridControl gcPercentualBH;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPercentualBH;
        private DevExpress.XtraGrid.Columns.GridColumn Percentual;
        private DevExpress.XtraGrid.Columns.GridColumn HorasPagasPercentual;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn CreditoPercentual;
        private DevExpress.XtraGrid.Columns.GridColumn DebitoPercentual;
        private DevExpress.XtraGrid.Columns.GridColumn SaldoPercentual;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemCalcEdit1;

    }
}
