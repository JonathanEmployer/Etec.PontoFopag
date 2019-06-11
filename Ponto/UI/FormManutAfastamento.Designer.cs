namespace UI
{
    partial class FormManutAfastamento
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.sbIdFuncionario = new Componentes.devexpress.cwk_DevButton();
            this.cbIdEmpresa = new Componentes.devexpress.cwk_DevLookup();
            this.sbIdEmpresa = new Componentes.devexpress.cwk_DevButton();
            this.sbIdDepartamento = new Componentes.devexpress.cwk_DevButton();
            this.cbIdFuncionario = new Componentes.devexpress.cwk_DevLookup();
            this.lblFuncionário = new DevExpress.XtraEditors.LabelControl();
            this.cbIdDepartamento = new Componentes.devexpress.cwk_DevLookup();
            this.lblDepartamento = new DevExpress.XtraEditors.LabelControl();
            this.lblEmpresa = new DevExpress.XtraEditors.LabelControl();
            this.rgTipo = new DevExpress.XtraEditors.RadioGroup();
            this.lblOcorrencia = new DevExpress.XtraEditors.LabelControl();
            this.sbIdOcorrencia = new Componentes.devexpress.cwk_DevButton();
            this.cbIdOcorrencia = new Componentes.devexpress.cwk_DevLookup();
            this.txtDatai = new DevExpress.XtraEditors.DateEdit();
            this.lblDatai = new DevExpress.XtraEditors.LabelControl();
            this.lblDataf = new DevExpress.XtraEditors.LabelControl();
            this.txtDataf = new DevExpress.XtraEditors.DateEdit();
            this.txtHorai = new Componentes.devexpress.cwkEditHora();
            this.lblHorai = new DevExpress.XtraEditors.LabelControl();
            this.lblHoraf = new DevExpress.XtraEditors.LabelControl();
            this.txtHoraf = new Componentes.devexpress.cwkEditHora();
            this.chbSemCalculo = new DevExpress.XtraEditors.CheckEdit();
            this.chbAbonado = new DevExpress.XtraEditors.CheckEdit();
            this.chbParcial = new DevExpress.XtraEditors.CheckEdit();
            this.lblCodigo = new DevExpress.XtraEditors.LabelControl();
            this.txtCodigo = new DevExpress.XtraEditors.CalcEdit();
            this.chbSuspensao = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdOcorrencia.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatai.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatai.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties.VistaTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHorai.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHoraf.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSemCalculo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAbonado.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbParcial.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSuspensao.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Size = new System.Drawing.Size(525, 295);
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.chbSuspensao);
            this.xtraTabPage1.Controls.Add(this.chbParcial);
            this.xtraTabPage1.Controls.Add(this.chbAbonado);
            this.xtraTabPage1.Controls.Add(this.chbSemCalculo);
            this.xtraTabPage1.Controls.Add(this.txtHoraf);
            this.xtraTabPage1.Controls.Add(this.lblHoraf);
            this.xtraTabPage1.Controls.Add(this.lblHorai);
            this.xtraTabPage1.Controls.Add(this.txtHorai);
            this.xtraTabPage1.Controls.Add(this.lblDataf);
            this.xtraTabPage1.Controls.Add(this.txtDataf);
            this.xtraTabPage1.Controls.Add(this.lblDatai);
            this.xtraTabPage1.Controls.Add(this.txtDatai);
            this.xtraTabPage1.Controls.Add(this.sbIdOcorrencia);
            this.xtraTabPage1.Controls.Add(this.cbIdOcorrencia);
            this.xtraTabPage1.Controls.Add(this.lblOcorrencia);
            this.xtraTabPage1.Controls.Add(this.groupControl1);
            this.xtraTabPage1.Controls.Add(this.txtCodigo);
            this.xtraTabPage1.Controls.Add(this.lblCodigo);
            this.xtraTabPage1.Size = new System.Drawing.Size(519, 289);
            // 
            // sbCancelar
            // 
            this.sbCancelar.Location = new System.Drawing.Point(462, 313);
            // 
            // sbGravar
            // 
            this.sbGravar.Location = new System.Drawing.Point(381, 313);
            // 
            // sbAjuda
            // 
            this.sbAjuda.Location = new System.Drawing.Point(12, 313);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.sbIdFuncionario);
            this.groupControl1.Controls.Add(this.cbIdEmpresa);
            this.groupControl1.Controls.Add(this.sbIdDepartamento);
            this.groupControl1.Controls.Add(this.cbIdFuncionario);
            this.groupControl1.Controls.Add(this.lblFuncionário);
            this.groupControl1.Controls.Add(this.cbIdDepartamento);
            this.groupControl1.Controls.Add(this.sbIdEmpresa);
            this.groupControl1.Controls.Add(this.lblDepartamento);
            this.groupControl1.Controls.Add(this.lblEmpresa);
            this.groupControl1.Controls.Add(this.rgTipo);
            this.groupControl1.Location = new System.Drawing.Point(10, 39);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(497, 137);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "Tipo Afastamento";
            // 
            // sbIdFuncionario
            // 
            this.sbIdFuncionario.Enabled = false;
            this.sbIdFuncionario.Location = new System.Drawing.Point(459, 110);
            this.sbIdFuncionario.Name = "sbIdFuncionario";
            this.sbIdFuncionario.Size = new System.Drawing.Size(24, 20);
            this.sbIdFuncionario.TabIndex = 9;
            this.sbIdFuncionario.TabStop = false;
            this.sbIdFuncionario.Text = "...";
            this.sbIdFuncionario.Click += new System.EventHandler(this.sbIdFuncionario_Click);
            // 
            // cbIdEmpresa
            // 
            this.cbIdEmpresa.ButtonLookup = this.sbIdEmpresa;
            this.cbIdEmpresa.EditValue = 0;
            this.cbIdEmpresa.Enabled = false;
            this.cbIdEmpresa.Key = System.Windows.Forms.Keys.F5;
            this.cbIdEmpresa.Location = new System.Drawing.Point(89, 58);
            this.cbIdEmpresa.Name = "cbIdEmpresa";
            this.cbIdEmpresa.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdEmpresa.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código")});
            this.cbIdEmpresa.Properties.DisplayMember = "nome";
            this.cbIdEmpresa.Properties.NullText = "";
            this.cbIdEmpresa.Properties.ValueMember = "id";
            this.cbIdEmpresa.Size = new System.Drawing.Size(364, 20);
            this.cbIdEmpresa.TabIndex = 2;
            this.cbIdEmpresa.EditValueChanged += new System.EventHandler(this.cbIdEmpresa_EditValueChanged);
            // 
            // sbIdEmpresa
            // 
            this.sbIdEmpresa.Enabled = false;
            this.sbIdEmpresa.Location = new System.Drawing.Point(459, 58);
            this.sbIdEmpresa.Name = "sbIdEmpresa";
            this.sbIdEmpresa.Size = new System.Drawing.Size(24, 20);
            this.sbIdEmpresa.TabIndex = 3;
            this.sbIdEmpresa.TabStop = false;
            this.sbIdEmpresa.Text = "...";
            this.sbIdEmpresa.Click += new System.EventHandler(this.sbIdEmpresa_Click);
            // 
            // sbIdDepartamento
            // 
            this.sbIdDepartamento.Enabled = false;
            this.sbIdDepartamento.Location = new System.Drawing.Point(459, 84);
            this.sbIdDepartamento.Name = "sbIdDepartamento";
            this.sbIdDepartamento.Size = new System.Drawing.Size(24, 20);
            this.sbIdDepartamento.TabIndex = 6;
            this.sbIdDepartamento.TabStop = false;
            this.sbIdDepartamento.Text = "...";
            this.sbIdDepartamento.Click += new System.EventHandler(this.sbIdDepartamento_Click);
            // 
            // cbIdFuncionario
            // 
            this.cbIdFuncionario.ButtonLookup = this.sbIdFuncionario;
            this.cbIdFuncionario.EditValue = 0;
            this.cbIdFuncionario.Enabled = false;
            this.cbIdFuncionario.Key = System.Windows.Forms.Keys.F5;
            this.cbIdFuncionario.Location = new System.Drawing.Point(89, 110);
            this.cbIdFuncionario.Name = "cbIdFuncionario";
            this.cbIdFuncionario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdFuncionario.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nome", "Nome"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("dscodigo", "Código")});
            this.cbIdFuncionario.Properties.DisplayMember = "nome";
            this.cbIdFuncionario.Properties.NullText = "";
            this.cbIdFuncionario.Properties.ValueMember = "id";
            this.cbIdFuncionario.Size = new System.Drawing.Size(364, 20);
            this.cbIdFuncionario.TabIndex = 8;
            // 
            // lblFuncionário
            // 
            this.lblFuncionário.Location = new System.Drawing.Point(24, 113);
            this.lblFuncionário.Name = "lblFuncionário";
            this.lblFuncionário.Size = new System.Drawing.Size(59, 13);
            this.lblFuncionário.TabIndex = 7;
            this.lblFuncionário.Text = "Funcionário:";
            // 
            // cbIdDepartamento
            // 
            this.cbIdDepartamento.ButtonLookup = this.sbIdDepartamento;
            this.cbIdDepartamento.EditValue = 0;
            this.cbIdDepartamento.Enabled = false;
            this.cbIdDepartamento.Key = System.Windows.Forms.Keys.F5;
            this.cbIdDepartamento.Location = new System.Drawing.Point(89, 84);
            this.cbIdDepartamento.Name = "cbIdDepartamento";
            this.cbIdDepartamento.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdDepartamento.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código")});
            this.cbIdDepartamento.Properties.DisplayMember = "descricao";
            this.cbIdDepartamento.Properties.NullText = "";
            this.cbIdDepartamento.Properties.ValueMember = "id";
            this.cbIdDepartamento.Size = new System.Drawing.Size(364, 20);
            this.cbIdDepartamento.TabIndex = 5;
            // 
            // lblDepartamento
            // 
            this.lblDepartamento.Location = new System.Drawing.Point(10, 87);
            this.lblDepartamento.Name = "lblDepartamento";
            this.lblDepartamento.Size = new System.Drawing.Size(73, 13);
            this.lblDepartamento.TabIndex = 4;
            this.lblDepartamento.Text = "Departamento:";
            // 
            // lblEmpresa
            // 
            this.lblEmpresa.Location = new System.Drawing.Point(38, 61);
            this.lblEmpresa.Name = "lblEmpresa";
            this.lblEmpresa.Size = new System.Drawing.Size(45, 13);
            this.lblEmpresa.TabIndex = 1;
            this.lblEmpresa.Text = "Empresa:";
            // 
            // rgTipo
            // 
            this.rgTipo.Location = new System.Drawing.Point(0, 19);
            this.rgTipo.Name = "rgTipo";
            this.rgTipo.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(2, "Empresa"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "Departamento"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "Funcionário")});
            this.rgTipo.Size = new System.Drawing.Size(497, 33);
            this.rgTipo.TabIndex = 0;
            this.rgTipo.SelectedIndexChanged += new System.EventHandler(this.rgTipo_SelectedIndexChanged);
            // 
            // lblOcorrencia
            // 
            this.lblOcorrencia.Location = new System.Drawing.Point(23, 186);
            this.lblOcorrencia.Name = "lblOcorrencia";
            this.lblOcorrencia.Size = new System.Drawing.Size(56, 13);
            this.lblOcorrencia.TabIndex = 3;
            this.lblOcorrencia.Text = "Ocorrência:";
            // 
            // sbIdOcorrencia
            // 
            this.sbIdOcorrencia.Location = new System.Drawing.Point(394, 183);
            this.sbIdOcorrencia.Name = "sbIdOcorrencia";
            this.sbIdOcorrencia.Size = new System.Drawing.Size(24, 20);
            this.sbIdOcorrencia.TabIndex = 5;
            this.sbIdOcorrencia.TabStop = false;
            this.sbIdOcorrencia.Text = "...";
            this.sbIdOcorrencia.Click += new System.EventHandler(this.sbIdOcorrencia_Click);
            // 
            // cbIdOcorrencia
            // 
            this.cbIdOcorrencia.ButtonLookup = this.sbIdOcorrencia;
            this.cbIdOcorrencia.EditValue = 0;
            this.cbIdOcorrencia.Key = System.Windows.Forms.Keys.F5;
            this.cbIdOcorrencia.Location = new System.Drawing.Point(84, 183);
            this.cbIdOcorrencia.Name = "cbIdOcorrencia";
            this.cbIdOcorrencia.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbIdOcorrencia.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("descricao", "Descrição", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "id", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("codigo", "Código")});
            this.cbIdOcorrencia.Properties.DisplayMember = "descricao";
            this.cbIdOcorrencia.Properties.NullText = "";
            this.cbIdOcorrencia.Properties.ValueMember = "id";
            this.cbIdOcorrencia.Size = new System.Drawing.Size(304, 20);
            this.cbIdOcorrencia.TabIndex = 4;
            // 
            // txtDatai
            // 
            this.txtDatai.EditValue = null;
            this.txtDatai.Location = new System.Drawing.Point(84, 218);
            this.txtDatai.Name = "txtDatai";
            this.txtDatai.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDatai.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDatai.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDatai.Size = new System.Drawing.Size(79, 20);
            this.txtDatai.TabIndex = 7;
            // 
            // lblDatai
            // 
            this.lblDatai.Location = new System.Drawing.Point(22, 221);
            this.lblDatai.Name = "lblDatai";
            this.lblDatai.Size = new System.Drawing.Size(57, 13);
            this.lblDatai.TabIndex = 6;
            this.lblDatai.Text = "Data Inicial:";
            // 
            // lblDataf
            // 
            this.lblDataf.Location = new System.Drawing.Point(251, 222);
            this.lblDataf.Name = "lblDataf";
            this.lblDataf.Size = new System.Drawing.Size(52, 13);
            this.lblDataf.TabIndex = 8;
            this.lblDataf.Text = "Data Final:";
            // 
            // txtDataf
            // 
            this.txtDataf.EditValue = null;
            this.txtDataf.Location = new System.Drawing.Point(309, 219);
            this.txtDataf.Name = "txtDataf";
            this.txtDataf.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDataf.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTimeAdvancingCaret;
            this.txtDataf.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDataf.Size = new System.Drawing.Size(79, 20);
            this.txtDataf.TabIndex = 9;
            // 
            // txtHorai
            // 
            this.txtHorai.cwErro = false;
            this.txtHorai.EditValue = "--:--";
            this.txtHorai.Enabled = false;
            this.txtHorai.Layout = Componentes.devexpress.LayoutsHorario.horario2Digitos;
            this.txtHorai.lblLegenda = null;
            this.txtHorai.lblNRelogio = null;
            this.txtHorai.Location = new System.Drawing.Point(84, 255);
            this.txtHorai.Name = "txtHorai";
            this.txtHorai.Properties.Appearance.Options.UseTextOptions = true;
            this.txtHorai.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtHorai.Properties.MaxLength = 5;
            this.txtHorai.Size = new System.Drawing.Size(35, 20);
            this.txtHorai.TabIndex = 11;
            this.txtHorai.ValorAnterior = null;
            // 
            // lblHorai
            // 
            this.lblHorai.Location = new System.Drawing.Point(10, 258);
            this.lblHorai.Name = "lblHorai";
            this.lblHorai.Size = new System.Drawing.Size(69, 13);
            this.lblHorai.TabIndex = 10;
            this.lblHorai.Text = "Abono Diurno:";
            // 
            // lblHoraf
            // 
            this.lblHoraf.Location = new System.Drawing.Point(270, 258);
            this.lblHoraf.Name = "lblHoraf";
            this.lblHoraf.Size = new System.Drawing.Size(77, 13);
            this.lblHoraf.TabIndex = 12;
            this.lblHoraf.Text = "Abono Noturno:";
            // 
            // txtHoraf
            // 
            this.txtHoraf.cwErro = false;
            this.txtHoraf.EditValue = "--:--";
            this.txtHoraf.Enabled = false;
            this.txtHoraf.Layout = Componentes.devexpress.LayoutsHorario.horario2Digitos;
            this.txtHoraf.lblLegenda = null;
            this.txtHoraf.lblNRelogio = null;
            this.txtHoraf.Location = new System.Drawing.Point(353, 255);
            this.txtHoraf.Name = "txtHoraf";
            this.txtHoraf.Properties.Appearance.Options.UseTextOptions = true;
            this.txtHoraf.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.txtHoraf.Properties.MaxLength = 5;
            this.txtHoraf.Size = new System.Drawing.Size(35, 20);
            this.txtHoraf.TabIndex = 13;
            this.txtHoraf.ValorAnterior = null;
            // 
            // chbSemCalculo
            // 
            this.chbSemCalculo.Location = new System.Drawing.Point(424, 183);
            this.chbSemCalculo.Name = "chbSemCalculo";
            this.chbSemCalculo.Properties.Caption = "Sem Cálculo";
            this.chbSemCalculo.Size = new System.Drawing.Size(83, 19);
            this.chbSemCalculo.TabIndex = 14;
            this.chbSemCalculo.CheckedChanged += new System.EventHandler(this.chbSemCalculo_CheckedChanged);
            // 
            // chbAbonado
            // 
            this.chbAbonado.Location = new System.Drawing.Point(424, 206);
            this.chbAbonado.Name = "chbAbonado";
            this.chbAbonado.Properties.Caption = "Abonado";
            this.chbAbonado.Size = new System.Drawing.Size(75, 19);
            this.chbAbonado.TabIndex = 15;
            this.chbAbonado.CheckedChanged += new System.EventHandler(this.chbAbonado_CheckedChanged);
            // 
            // chbParcial
            // 
            this.chbParcial.Location = new System.Drawing.Point(424, 231);
            this.chbParcial.Name = "chbParcial";
            this.chbParcial.Properties.Caption = "Parcial";
            this.chbParcial.Size = new System.Drawing.Size(63, 19);
            this.chbParcial.TabIndex = 16;
            this.chbParcial.CheckedChanged += new System.EventHandler(this.chbParcial_CheckedChanged);
            // 
            // lblCodigo
            // 
            this.lblCodigo.Location = new System.Drawing.Point(14, 16);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(37, 13);
            this.lblCodigo.TabIndex = 0;
            this.lblCodigo.Text = "Código:";
            // 
            // txtCodigo
            // 
            this.txtCodigo.Location = new System.Drawing.Point(57, 13);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, false)});
            this.txtCodigo.Properties.Mask.EditMask = "n0";
            this.txtCodigo.Size = new System.Drawing.Size(80, 20);
            this.txtCodigo.TabIndex = 1;
            this.txtCodigo.TabStop = false;
            // 
            // chbSuspensao
            // 
            this.chbSuspensao.Location = new System.Drawing.Point(424, 256);
            this.chbSuspensao.Name = "chbSuspensao";
            this.chbSuspensao.Properties.Caption = "Suspensão";
            this.chbSuspensao.Size = new System.Drawing.Size(83, 19);
            this.chbSuspensao.TabIndex = 17;
            this.chbSuspensao.CheckedChanged += new System.EventHandler(this.chbSuspensao_CheckedChanged);
            // 
            // FormManutAfastamento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(549, 348);
            this.Name = "FormManutAfastamento";
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxErroProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdEmpresa.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdFuncionario.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdDepartamento.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgTipo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbIdOcorrencia.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatai.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDatai.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties.VistaTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataf.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHorai.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtHoraf.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSemCalculo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbAbonado.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbParcial.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCodigo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chbSuspensao.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private Componentes.devexpress.cwk_DevLookup cbIdEmpresa;
        private Componentes.devexpress.cwk_DevButton sbIdEmpresa;
        private Componentes.devexpress.cwk_DevButton sbIdDepartamento;
        private Componentes.devexpress.cwk_DevLookup cbIdDepartamento;
        private DevExpress.XtraEditors.LabelControl lblDepartamento;
        private DevExpress.XtraEditors.LabelControl lblEmpresa;
        private DevExpress.XtraEditors.RadioGroup rgTipo;
        private Componentes.devexpress.cwk_DevButton sbIdFuncionario;
        private Componentes.devexpress.cwk_DevLookup cbIdFuncionario;
        private DevExpress.XtraEditors.LabelControl lblFuncionário;
        private DevExpress.XtraEditors.LabelControl lblOcorrencia;
        private Componentes.devexpress.cwk_DevButton sbIdOcorrencia;
        private Componentes.devexpress.cwk_DevLookup cbIdOcorrencia;
        private DevExpress.XtraEditors.LabelControl lblDataf;
        private DevExpress.XtraEditors.DateEdit txtDataf;
        private DevExpress.XtraEditors.LabelControl lblDatai;
        private DevExpress.XtraEditors.DateEdit txtDatai;
        private DevExpress.XtraEditors.LabelControl lblHorai;
        private Componentes.devexpress.cwkEditHora txtHorai;
        private Componentes.devexpress.cwkEditHora txtHoraf;
        private DevExpress.XtraEditors.LabelControl lblHoraf;
        private DevExpress.XtraEditors.CheckEdit chbSemCalculo;
        private DevExpress.XtraEditors.CheckEdit chbParcial;
        private DevExpress.XtraEditors.CheckEdit chbAbonado;
        private DevExpress.XtraEditors.CalcEdit txtCodigo;
        private DevExpress.XtraEditors.LabelControl lblCodigo;
        private DevExpress.XtraEditors.CheckEdit chbSuspensao;
    }
}
