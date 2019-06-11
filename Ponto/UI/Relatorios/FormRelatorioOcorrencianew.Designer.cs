namespace UI.Relatorios
{
    partial class FormRelatorioOcorrencia
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
            this.groupTipo = new DevExpress.XtraEditors.GroupControl();
            this.ckAtraso = new DevExpress.XtraEditors.CheckEdit();
            this.cbHorasExtras = new DevExpress.XtraEditors.CheckEdit();
            this.ckSaidaAntecipada = new DevExpress.XtraEditors.CheckEdit();
            this.ckEntrAtrasada = new DevExpress.XtraEditors.CheckEdit();
            this.ckDebitoBH = new DevExpress.XtraEditors.CheckEdit();
            this.ckMarcIncorretas = new DevExpress.XtraEditors.CheckEdit();
            this.ckOcorrencia = new DevExpress.XtraEditors.CheckEdit();
            this.ckFalta = new DevExpress.XtraEditors.CheckEdit();
            this.dataFinal = new DevExpress.XtraEditors.DateEdit();
            this.lbdataFinal = new System.Windows.Forms.Label();
            this.lbdataInicial = new System.Windows.Forms.Label();
            this.dataInicial = new DevExpress.XtraEditors.DateEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.rgOrdena = new DevExpress.XtraEditors.RadioGroup();
            this.cbAgruparPorDepartamento = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.gcOcorrencias = new DevExpress.XtraGrid.GridControl();
            this.gvOcorrencias = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.ColunaIDFunc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).BeginInit();
            this.TabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupTipo)).BeginInit();
            this.groupTipo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ckAtraso.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbHorasExtras.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckSaidaAntecipada.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEntrAtrasada.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckDebitoBH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckMarcIncorretas.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckOcorrencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckFalta.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataFinal.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataFinal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataInicial.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataInicial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rgOrdena.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAgruparPorDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcOcorrencias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOcorrencias)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            // 
            // rgTipo
            // 
            // 
            // chbSalvarFiltro
            // 
            this.chbSalvarFiltro.Location = new System.Drawing.Point(106, 528);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(12, 524);
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(745, 524);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(666, 524);
            // 
            // TabControl1
            // 
            this.TabControl1.Size = new System.Drawing.Size(808, 506);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelControl4);
            this.tabPage1.Controls.Add(this.gcOcorrencias);
            this.tabPage1.Controls.Add(this.cbAgruparPorDepartamento);
            this.tabPage1.Controls.Add(this.groupControl3);
            this.tabPage1.Controls.Add(this.groupTipo);
            this.tabPage1.Controls.Add(this.dataFinal);
            this.tabPage1.Controls.Add(this.lbdataFinal);
            this.tabPage1.Controls.Add(this.dataInicial);
            this.tabPage1.Controls.Add(this.lbdataInicial);
            this.tabPage1.Size = new System.Drawing.Size(802, 500);
            this.tabPage1.Controls.SetChildIndex(this.lbdataInicial, 0);
            this.tabPage1.Controls.SetChildIndex(this.dataInicial, 0);
            this.tabPage1.Controls.SetChildIndex(this.lbdataFinal, 0);
            this.tabPage1.Controls.SetChildIndex(this.dataFinal, 0);
            this.tabPage1.Controls.SetChildIndex(this.groupTipo, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl1, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl2, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl3, 0);
            this.tabPage1.Controls.SetChildIndex(this.groupControl1, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarEmpresas, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparEmpresa, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarFuncionarios, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparFuncionarios, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbSelecionarDepartamentos, 0);
            this.tabPage1.Controls.SetChildIndex(this.sbLimparDepartamento, 0);
            this.tabPage1.Controls.SetChildIndex(this.groupControl3, 0);
            this.tabPage1.Controls.SetChildIndex(this.cbAgruparPorDepartamento, 0);
            this.tabPage1.Controls.SetChildIndex(this.gcOcorrencias, 0);
            this.tabPage1.Controls.SetChildIndex(this.labelControl4, 0);
            // 
            // groupTipo
            // 
            this.groupTipo.Controls.Add(this.ckAtraso);
            this.groupTipo.Controls.Add(this.cbHorasExtras);
            this.groupTipo.Controls.Add(this.ckSaidaAntecipada);
            this.groupTipo.Controls.Add(this.ckEntrAtrasada);
            this.groupTipo.Controls.Add(this.ckDebitoBH);
            this.groupTipo.Controls.Add(this.ckMarcIncorretas);
            this.groupTipo.Controls.Add(this.ckOcorrencia);
            this.groupTipo.Controls.Add(this.ckFalta);
            this.groupTipo.Location = new System.Drawing.Point(6, 399);
            this.groupTipo.Name = "groupTipo";
            this.groupTipo.Size = new System.Drawing.Size(480, 98);
            this.groupTipo.TabIndex = 5;
            this.groupTipo.Text = "Tipo da Ocorrência";
            // 
            // ckAtraso
            // 
            this.ckAtraso.Location = new System.Drawing.Point(381, 42);
            this.ckAtraso.Name = "ckAtraso";
            this.ckAtraso.Properties.Caption = "Atraso";
            this.ckAtraso.Size = new System.Drawing.Size(88, 19);
            this.ckAtraso.TabIndex = 8;
            // 
            // cbHorasExtras
            // 
            this.cbHorasExtras.Location = new System.Drawing.Point(381, 20);
            this.cbHorasExtras.Name = "cbHorasExtras";
            this.cbHorasExtras.Properties.Caption = "Horas Extras";
            this.cbHorasExtras.Size = new System.Drawing.Size(88, 19);
            this.cbHorasExtras.TabIndex = 7;
            // 
            // ckSaidaAntecipada
            // 
            this.ckSaidaAntecipada.Location = new System.Drawing.Point(254, 42);
            this.ckSaidaAntecipada.Name = "ckSaidaAntecipada";
            this.ckSaidaAntecipada.Properties.Caption = "Saída Antecipada";
            this.ckSaidaAntecipada.Size = new System.Drawing.Size(131, 19);
            this.ckSaidaAntecipada.TabIndex = 5;
            // 
            // ckEntrAtrasada
            // 
            this.ckEntrAtrasada.Location = new System.Drawing.Point(254, 20);
            this.ckEntrAtrasada.Name = "ckEntrAtrasada";
            this.ckEntrAtrasada.Properties.Caption = "Entrada Atrasada";
            this.ckEntrAtrasada.Size = new System.Drawing.Size(120, 19);
            this.ckEntrAtrasada.TabIndex = 2;
            // 
            // ckDebitoBH
            // 
            this.ckDebitoBH.Location = new System.Drawing.Point(9, 42);
            this.ckDebitoBH.Name = "ckDebitoBH";
            this.ckDebitoBH.Properties.Caption = "Débito B.H.";
            this.ckDebitoBH.Size = new System.Drawing.Size(75, 19);
            this.ckDebitoBH.TabIndex = 3;
            // 
            // ckMarcIncorretas
            // 
            this.ckMarcIncorretas.Location = new System.Drawing.Point(106, 42);
            this.ckMarcIncorretas.Name = "ckMarcIncorretas";
            this.ckMarcIncorretas.Properties.Caption = "Marcações Incorretas";
            this.ckMarcIncorretas.Size = new System.Drawing.Size(131, 19);
            this.ckMarcIncorretas.TabIndex = 4;
            // 
            // ckOcorrencia
            // 
            this.ckOcorrencia.Location = new System.Drawing.Point(106, 20);
            this.ckOcorrencia.Name = "ckOcorrencia";
            this.ckOcorrencia.Properties.Caption = "Ocorrência";
            this.ckOcorrencia.Size = new System.Drawing.Size(75, 19);
            this.ckOcorrencia.TabIndex = 1;
            this.ckOcorrencia.CheckedChanged += new System.EventHandler(this.ckOcorrencia_CheckedChanged);
            // 
            // ckFalta
            // 
            this.ckFalta.Location = new System.Drawing.Point(9, 20);
            this.ckFalta.Name = "ckFalta";
            this.ckFalta.Properties.Caption = "Falta";
            this.ckFalta.Size = new System.Drawing.Size(75, 19);
            this.ckFalta.TabIndex = 0;
            // 
            // dataFinal
            // 
            this.dataFinal.EditValue = null;
            this.dataFinal.Location = new System.Drawing.Point(662, 31);
            this.dataFinal.Name = "dataFinal";
            this.dataFinal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dataFinal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dataFinal.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dataFinal.Size = new System.Drawing.Size(100, 20);
            this.dataFinal.TabIndex = 9;
            // 
            // lbdataFinal
            // 
            this.lbdataFinal.AutoSize = true;
            this.lbdataFinal.Location = new System.Drawing.Point(595, 34);
            this.lbdataFinal.Name = "lbdataFinal";
            this.lbdataFinal.Size = new System.Drawing.Size(58, 13);
            this.lbdataFinal.TabIndex = 8;
            this.lbdataFinal.Text = "Data Final:";
            // 
            // lbdataInicial
            // 
            this.lbdataInicial.AutoSize = true;
            this.lbdataInicial.Location = new System.Drawing.Point(423, 34);
            this.lbdataInicial.Name = "lbdataInicial";
            this.lbdataInicial.Size = new System.Drawing.Size(63, 13);
            this.lbdataInicial.TabIndex = 6;
            this.lbdataInicial.Text = "Data Inicial:";
            // 
            // dataInicial
            // 
            this.dataInicial.EditValue = null;
            this.dataInicial.Location = new System.Drawing.Point(489, 31);
            this.dataInicial.Name = "dataInicial";
            this.dataInicial.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dataInicial.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.dataInicial.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dataInicial.Size = new System.Drawing.Size(100, 20);
            this.dataInicial.TabIndex = 7;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.rgOrdena);
            this.groupControl3.Location = new System.Drawing.Point(492, 399);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(301, 98);
            this.groupControl3.TabIndex = 18;
            this.groupControl3.Text = "Ordena Por:";
            // 
            // rgOrdena
            // 
            this.rgOrdena.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rgOrdena.EditValue = 0;
            this.rgOrdena.Location = new System.Drawing.Point(2, 21);
            this.rgOrdena.Name = "rgOrdena";
            this.rgOrdena.Properties.EnableFocusRect = true;
            this.rgOrdena.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Data / Funcionário"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Funcionário / Data"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "Matrícula")});
            this.rgOrdena.Size = new System.Drawing.Size(297, 75);
            this.rgOrdena.TabIndex = 0;
            // 
            // cbAgruparPorDepartamento
            // 
            this.cbAgruparPorDepartamento.Location = new System.Drawing.Point(424, 6);
            this.cbAgruparPorDepartamento.Name = "cbAgruparPorDepartamento";
            this.cbAgruparPorDepartamento.Properties.Caption = "Agrupar por Departamento";
            this.cbAgruparPorDepartamento.Size = new System.Drawing.Size(167, 19);
            this.cbAgruparPorDepartamento.TabIndex = 19;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Location = new System.Drawing.Point(418, 200);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(91, 18);
            this.labelControl4.TabIndex = 20;
            this.labelControl4.Text = "Ocorrências";
            // 
            // gcOcorrencias
            // 
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.First.Hint = "Primeiro registro";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Last.Hint = "Último registro";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Next.Hint = "Próximo registro";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.NextPage.Hint = "Próxima página";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Prev.Hint = "Registro anterior";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.PrevPage.Hint = "Página anterior";
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gcOcorrencias.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcOcorrencias.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gcOcorrencias.Location = new System.Drawing.Point(418, 224);
            this.gcOcorrencias.MainView = this.gvOcorrencias;
            this.gcOcorrencias.Name = "gcOcorrencias";
            this.gcOcorrencias.Size = new System.Drawing.Size(375, 169);
            this.gcOcorrencias.TabIndex = 21;
            this.gcOcorrencias.UseEmbeddedNavigator = true;
            this.gcOcorrencias.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvOcorrencias});
            // 
            // gvOcorrencias
            // 
            this.gvOcorrencias.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.ColumnFilterButton.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.ColumnFilterButton.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.Empty.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.EvenRow.BackColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.EvenRow.BorderColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.EvenRow.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.EvenRow.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.EvenRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvOcorrencias.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(104)))), ((int)(((byte)(184)))), ((int)(((byte)(251)))));
            this.gvOcorrencias.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.FilterCloseButton.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FilterCloseButton.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.FilterCloseButton.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvOcorrencias.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.FilterPanel.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FilterPanel.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.FixedLine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.FixedLine.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.FocusedCell.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.FocusedCell.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FocusedCell.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.FocusedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.FocusedRow.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FocusedRow.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.FooterPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.FooterPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.FooterPanel.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.FooterPanel.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.FooterPanel.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.GroupButton.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.GroupButton.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.GroupFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.GroupFooter.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.GroupFooter.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.GroupFooter.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.GroupFooter.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(246)))), ((int)(((byte)(255)))));
            this.gvOcorrencias.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.GroupPanel.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.GroupPanel.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.GroupRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvOcorrencias.Appearance.HeaderPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(201)))), ((int)(((byte)(254)))));
            this.gvOcorrencias.Appearance.HeaderPanel.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.HeaderPanel.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.HeaderPanel.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.HideSelectionRow.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.OddRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.OddRow.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.OddRow.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.OddRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5F);
            this.gvOcorrencias.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.Preview.Options.UseFont = true;
            this.gvOcorrencias.Appearance.Preview.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.Row.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(229)))), ((int)(((byte)(231)))));
            this.gvOcorrencias.Appearance.Row.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.RowSeparator.BackColor = System.Drawing.Color.Black;
            this.gvOcorrencias.Appearance.RowSeparator.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(206)))), ((int)(((byte)(57)))));
            this.gvOcorrencias.Appearance.SelectedRow.ForeColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvOcorrencias.Appearance.SelectedRow.Options.UseBorderColor = true;
            this.gvOcorrencias.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvOcorrencias.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
            this.gvOcorrencias.Appearance.TopNewRow.Options.UseBackColor = true;
            this.gvOcorrencias.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.ColunaIDFunc,
            this.gridColumn4});
            this.gvOcorrencias.GridControl = this.gcOcorrencias;
            this.gvOcorrencias.GroupPanelText = "Arraste uma coluna aqui para agrupar os registros";
            this.gvOcorrencias.Name = "gvOcorrencias";
            this.gvOcorrencias.OptionsBehavior.Editable = false;
            this.gvOcorrencias.OptionsBehavior.FocusLeaveOnTab = true;
            this.gvOcorrencias.OptionsView.EnableAppearanceEvenRow = true;
            this.gvOcorrencias.OptionsView.EnableAppearanceOddRow = true;
            this.gvOcorrencias.OptionsView.ShowAutoFilterRow = true;
            this.gvOcorrencias.OptionsView.ShowGroupPanel = false;
            this.gvOcorrencias.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn4, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // ColunaIDFunc
            // 
            this.ColunaIDFunc.Caption = "ID";
            this.ColunaIDFunc.FieldName = "id";
            this.ColunaIDFunc.Name = "ColunaIDFunc";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Nome";
            this.gridColumn4.FieldName = "descricao";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            // 
            // FormRelatorioOcorrencia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(831, 553);
            this.Name = "FormRelatorioOcorrencia";
            this.Text = "Relatório de Ocorrências";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormRelatorioOcorrencianew_FormClosed);
            this.Shown += new System.EventHandler(this.FormRelatorioOcorrencia_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormRelatorioOcorrencianew_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSalvarFiltro.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl1)).EndInit();
            this.TabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErrorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupTipo)).EndInit();
            this.groupTipo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ckAtraso.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbHorasExtras.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckSaidaAntecipada.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckEntrAtrasada.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckDebitoBH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckMarcIncorretas.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckOcorrencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckFalta.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataFinal.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataFinal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataInicial.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataInicial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rgOrdena.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAgruparPorDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcOcorrencias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvOcorrencias)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupTipo;
        private DevExpress.XtraEditors.CheckEdit ckSaidaAntecipada;
        private DevExpress.XtraEditors.CheckEdit ckEntrAtrasada;
        private DevExpress.XtraEditors.CheckEdit ckDebitoBH;
        private DevExpress.XtraEditors.CheckEdit ckMarcIncorretas;
        private DevExpress.XtraEditors.CheckEdit ckOcorrencia;
        private DevExpress.XtraEditors.CheckEdit ckFalta;
        private DevExpress.XtraEditors.DateEdit dataFinal;
        private System.Windows.Forms.Label lbdataFinal;
        private System.Windows.Forms.Label lbdataInicial;
        private DevExpress.XtraEditors.DateEdit dataInicial;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.RadioGroup rgOrdena;
        private DevExpress.XtraEditors.CheckEdit cbAgruparPorDepartamento;
        private DevExpress.XtraEditors.CheckEdit cbHorasExtras;
        public DevExpress.XtraEditors.LabelControl labelControl4;
        public DevExpress.XtraGrid.GridControl gcOcorrencias;
        public DevExpress.XtraGrid.Views.Grid.GridView gvOcorrencias;
        private DevExpress.XtraGrid.Columns.GridColumn ColunaIDFunc;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.CheckEdit ckAtraso;
    }
}
