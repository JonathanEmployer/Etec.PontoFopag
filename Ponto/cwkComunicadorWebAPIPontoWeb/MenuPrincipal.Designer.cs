namespace cwkComunicadorWebAPIPontoWeb
{
    partial class MenuPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuPrincipal));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon();
            this.cmsTray = new System.Windows.Forms.ContextMenuStrip();
            this.listagemDeREPsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarDadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarDadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bwJobs = new System.ComponentModel.BackgroundWorker();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager();
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.pnlLogDireita = new System.Windows.Forms.Panel();
            this.sbAtualizarAplicacao = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sbSitInternet = new DevExpress.XtraEditors.SimpleButton();
            this.sbSitWs = new DevExpress.XtraEditors.SimpleButton();
            this.sbSitRep = new DevExpress.XtraEditors.SimpleButton();
            this.pnlLog = new System.Windows.Forms.Panel();
            this.btnEnviarLog = new DevExpress.XtraEditors.SimpleButton();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.dockPnlMenu = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tspBtnReps = new System.Windows.Forms.ToolStripButton();
            this.tsbImportarDados = new System.Windows.Forms.ToolStripButton();
            this.tsbExportarDados = new System.Windows.Forms.ToolStripButton();
            this.tsbDataHora = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbTrocarUsuario = new System.Windows.Forms.ToolStripButton();
            this.tsbSair = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.lbInfoServer = new System.Windows.Forms.Label();
            this.cmsTray.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            this.pnlLogDireita.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlLog.SuspendLayout();
            this.dockPnlMenu.SuspendLayout();
            this.dockPanel2_Container.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.cmsTray;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Pontofopag Comunicador";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // cmsTray
            // 
            this.cmsTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.listagemDeREPsToolStripMenuItem,
            this.importarDadosToolStripMenuItem,
            this.exportarDadosToolStripMenuItem,
            this.toolStripSeparator1,
            this.sairToolStripMenuItem});
            this.cmsTray.Name = "cmsTray";
            this.cmsTray.Size = new System.Drawing.Size(163, 98);
            this.cmsTray.Text = "Pontofopag Comunicador";
            // 
            // listagemDeREPsToolStripMenuItem
            // 
            this.listagemDeREPsToolStripMenuItem.Name = "listagemDeREPsToolStripMenuItem";
            this.listagemDeREPsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.listagemDeREPsToolStripMenuItem.Text = "Configurar RE&P\'s";
            this.listagemDeREPsToolStripMenuItem.Click += new System.EventHandler(this.listagemDeREPsToolStripMenuItem_Click);
            // 
            // importarDadosToolStripMenuItem
            // 
            this.importarDadosToolStripMenuItem.Name = "importarDadosToolStripMenuItem";
            this.importarDadosToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.importarDadosToolStripMenuItem.Text = "Importar Dados";
            this.importarDadosToolStripMenuItem.Click += new System.EventHandler(this.importarDadosToolStripMenuItem_Click);
            // 
            // exportarDadosToolStripMenuItem
            // 
            this.exportarDadosToolStripMenuItem.Name = "exportarDadosToolStripMenuItem";
            this.exportarDadosToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.exportarDadosToolStripMenuItem.Text = "Exportar Dados";
            this.exportarDadosToolStripMenuItem.Click += new System.EventHandler(this.tsbExportarDados_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(159, 6);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.sairToolStripMenuItem.Text = "Sai&r";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // bwJobs
            // 
            this.bwJobs.WorkerReportsProgress = true;
            this.bwJobs.WorkerSupportsCancellation = true;
            this.bwJobs.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwJobs_DoWork);
            this.bwJobs.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bwJobs_ProgressChanged);
            this.bwJobs.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwJobs_RunWorkerCompleted);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1,
            this.dockPnlMenu});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.dockPanel1.ID = new System.Guid("c267951c-68eb-4e1a-8d64-3df4e14c169f");
            this.dockPanel1.Location = new System.Drawing.Point(0, 613);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Options.AllowFloating = false;
            this.dockPanel1.Options.FloatOnDblClick = false;
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 116);
            this.dockPanel1.Size = new System.Drawing.Size(1011, 116);
            this.dockPanel1.Text = "Informações";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.pnlLogDireita);
            this.dockPanel1_Container.Controls.Add(this.panel1);
            this.dockPanel1_Container.Controls.Add(this.pnlLog);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(1003, 89);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // pnlLogDireita
            // 
            this.pnlLogDireita.Controls.Add(this.sbAtualizarAplicacao);
            this.pnlLogDireita.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlLogDireita.Location = new System.Drawing.Point(928, 0);
            this.pnlLogDireita.Name = "pnlLogDireita";
            this.pnlLogDireita.Size = new System.Drawing.Size(75, 89);
            this.pnlLogDireita.TabIndex = 74;
            // 
            // sbAtualizarAplicacao
            // 
            this.sbAtualizarAplicacao.Dock = System.Windows.Forms.DockStyle.Right;
            this.sbAtualizarAplicacao.Image = ((System.Drawing.Image)(resources.GetObject("sbAtualizarAplicacao.Image")));
            this.sbAtualizarAplicacao.ImageIndex = 7;
            this.sbAtualizarAplicacao.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.sbAtualizarAplicacao.Location = new System.Drawing.Point(0, 0);
            this.sbAtualizarAplicacao.Name = "sbAtualizarAplicacao";
            this.sbAtualizarAplicacao.Size = new System.Drawing.Size(75, 89);
            this.sbAtualizarAplicacao.TabIndex = 67;
            this.sbAtualizarAplicacao.Text = "Atualizar";
            this.sbAtualizarAplicacao.ToolTip = "Verifica a se o aplicativo esta na última versão disponível";
            this.sbAtualizarAplicacao.Click += new System.EventHandler(this.sbAtualizarAplicacao_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sbSitInternet);
            this.panel1.Controls.Add(this.sbSitWs);
            this.panel1.Controls.Add(this.sbSitRep);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(243, 89);
            this.panel1.TabIndex = 73;
            // 
            // sbSitInternet
            // 
            this.sbSitInternet.Image = global::cwkComunicadorWebAPIPontoWeb.Properties.Resources.InternetOk64;
            this.sbSitInternet.ImageIndex = 7;
            this.sbSitInternet.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.sbSitInternet.Location = new System.Drawing.Point(3, 2);
            this.sbSitInternet.Name = "sbSitInternet";
            this.sbSitInternet.Size = new System.Drawing.Size(75, 86);
            this.sbSitInternet.TabIndex = 66;
            this.sbSitInternet.Text = "Internet";
            this.sbSitInternet.ToolTip = "Verifica conexão com a internet.";
            this.sbSitInternet.Click += new System.EventHandler(this.sbSitInternet_Click);
            // 
            // sbSitWs
            // 
            this.sbSitWs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sbSitWs.Image = global::cwkComunicadorWebAPIPontoWeb.Properties.Resources.WebserviceOK64;
            this.sbSitWs.ImageIndex = 7;
            this.sbSitWs.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.sbSitWs.Location = new System.Drawing.Point(165, 2);
            this.sbSitWs.Name = "sbSitWs";
            this.sbSitWs.Size = new System.Drawing.Size(75, 86);
            this.sbSitWs.TabIndex = 68;
            this.sbSitWs.Text = "Servidor";
            this.sbSitWs.ToolTip = "Verifica a conexão com o Webservice";
            this.sbSitWs.Click += new System.EventHandler(this.sbSitWs_Click);
            // 
            // sbSitRep
            // 
            this.sbSitRep.Image = global::cwkComunicadorWebAPIPontoWeb.Properties.Resources.RepOk64;
            this.sbSitRep.ImageIndex = 7;
            this.sbSitRep.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.sbSitRep.Location = new System.Drawing.Point(84, 2);
            this.sbSitRep.Name = "sbSitRep";
            this.sbSitRep.Size = new System.Drawing.Size(75, 86);
            this.sbSitRep.TabIndex = 67;
            this.sbSitRep.Text = "Rep";
            this.sbSitRep.ToolTip = "Verifica conexão com o relógio.";
            this.sbSitRep.Click += new System.EventHandler(this.sbSitRep_Click);
            // 
            // pnlLog
            // 
            this.pnlLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLog.Controls.Add(this.btnEnviarLog);
            this.pnlLog.Controls.Add(this.rtbLog);
            this.pnlLog.Location = new System.Drawing.Point(246, 1);
            this.pnlLog.Name = "pnlLog";
            this.pnlLog.Size = new System.Drawing.Size(678, 86);
            this.pnlLog.TabIndex = 0;
            // 
            // btnEnviarLog
            // 
            this.btnEnviarLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEnviarLog.Image = ((System.Drawing.Image)(resources.GetObject("btnEnviarLog.Image")));
            this.btnEnviarLog.ImageIndex = 7;
            this.btnEnviarLog.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.btnEnviarLog.Location = new System.Drawing.Point(596, 29);
            this.btnEnviarLog.Name = "btnEnviarLog";
            this.btnEnviarLog.Size = new System.Drawing.Size(64, 56);
            this.btnEnviarLog.TabIndex = 69;
            this.btnEnviarLog.Text = "Enviar Log";
            this.btnEnviarLog.Visible = false;
            this.btnEnviarLog.Click += new System.EventHandler(this.btnEnviarLog_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(678, 86);
            this.rtbLog.TabIndex = 5;
            this.rtbLog.Text = "";
            this.rtbLog.TextChanged += new System.EventHandler(this.rtbLog_TextChanged);
            // 
            // dockPnlMenu
            // 
            this.dockPnlMenu.Controls.Add(this.dockPanel2_Container);
            this.dockPnlMenu.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPnlMenu.ID = new System.Guid("7c1814e4-0308-4c50-8d3b-e09184884963");
            this.dockPnlMenu.Location = new System.Drawing.Point(0, 0);
            this.dockPnlMenu.Name = "dockPnlMenu";
            this.dockPnlMenu.Options.AllowDockBottom = false;
            this.dockPnlMenu.Options.AllowDockTop = false;
            this.dockPnlMenu.Options.AllowFloating = false;
            this.dockPnlMenu.Options.FloatOnDblClick = false;
            this.dockPnlMenu.Options.ShowCloseButton = false;
            this.dockPnlMenu.OriginalSize = new System.Drawing.Size(99, 200);
            this.dockPnlMenu.Size = new System.Drawing.Size(99, 613);
            this.dockPnlMenu.Text = "Menu";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Controls.Add(this.toolStripContainer1);
            this.dockPanel2_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(91, 586);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Enabled = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(91, 561);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // toolStripContainer1.LeftToolStripPanel
            // 
            this.toolStripContainer1.LeftToolStripPanel.Enabled = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            this.toolStripContainer1.RightToolStripPanel.Enabled = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(91, 586);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Enabled = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspBtnReps,
            this.tsbImportarDados,
            this.tsbExportarDados,
            this.tsbDataHora,
            this.toolStripSeparator2,
            this.tsbTrocarUsuario,
            this.tsbSair});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(94, 561);
            this.toolStrip1.TabIndex = 10;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // tspBtnReps
            // 
            this.tspBtnReps.Image = ((System.Drawing.Image)(resources.GetObject("tspBtnReps.Image")));
            this.tspBtnReps.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tspBtnReps.Name = "tspBtnReps";
            this.tspBtnReps.Size = new System.Drawing.Size(91, 83);
            this.tspBtnReps.Text = "Config. Reps";
            this.tspBtnReps.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tspBtnReps.Click += new System.EventHandler(this.tspBtnReps_Click);
            // 
            // tsbImportarDados
            // 
            this.tsbImportarDados.Image = ((System.Drawing.Image)(resources.GetObject("tsbImportarDados.Image")));
            this.tsbImportarDados.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportarDados.Name = "tsbImportarDados";
            this.tsbImportarDados.Size = new System.Drawing.Size(91, 83);
            this.tsbImportarDados.Text = "Importar Dados";
            this.tsbImportarDados.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.tsbImportarDados.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbImportarDados.Click += new System.EventHandler(this.tsbImportarDados_Click);
            // 
            // tsbExportarDados
            // 
            this.tsbExportarDados.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportarDados.Image")));
            this.tsbExportarDados.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportarDados.Name = "tsbExportarDados";
            this.tsbExportarDados.Size = new System.Drawing.Size(91, 83);
            this.tsbExportarDados.Text = "Exportar Dados";
            this.tsbExportarDados.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbExportarDados.ToolTipText = "Exportar Dados Rep";
            this.tsbExportarDados.Click += new System.EventHandler(this.tsbExportarDados_Click);
            // 
            // tsbDataHora
            // 
            this.tsbDataHora.Image = ((System.Drawing.Image)(resources.GetObject("tsbDataHora.Image")));
            this.tsbDataHora.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDataHora.Name = "tsbDataHora";
            this.tsbDataHora.Size = new System.Drawing.Size(91, 83);
            this.tsbDataHora.Text = "Data e Hora";
            this.tsbDataHora.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.tsbDataHora.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbDataHora.ToolTipText = "Enviar Data e Hora";
            this.tsbDataHora.Click += new System.EventHandler(this.tsbDataHora_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(91, 6);
            // 
            // tsbTrocarUsuario
            // 
            this.tsbTrocarUsuario.Image = ((System.Drawing.Image)(resources.GetObject("tsbTrocarUsuario.Image")));
            this.tsbTrocarUsuario.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTrocarUsuario.Name = "tsbTrocarUsuario";
            this.tsbTrocarUsuario.Size = new System.Drawing.Size(91, 83);
            this.tsbTrocarUsuario.Text = "Trocar Usuário";
            this.tsbTrocarUsuario.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbTrocarUsuario.Click += new System.EventHandler(this.tsbTrocarUsuario_Click);
            // 
            // tsbSair
            // 
            this.tsbSair.Image = ((System.Drawing.Image)(resources.GetObject("tsbSair.Image")));
            this.tsbSair.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSair.Name = "tsbSair";
            this.tsbSair.Size = new System.Drawing.Size(91, 83);
            this.tsbSair.Text = "Sair";
            this.tsbSair.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSair.Click += new System.EventHandler(this.tsbSair_Click_2);
            // 
            // lbInfoServer
            // 
            this.lbInfoServer.AutoSize = true;
            this.lbInfoServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInfoServer.ForeColor = System.Drawing.Color.DarkRed;
            this.lbInfoServer.Location = new System.Drawing.Point(105, 9);
            this.lbInfoServer.Name = "lbInfoServer";
            this.lbInfoServer.Size = new System.Drawing.Size(0, 31);
            this.lbInfoServer.TabIndex = 4;
            // 
            // MenuPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1011, 729);
            this.Controls.Add(this.lbInfoServer);
            this.Controls.Add(this.dockPnlMenu);
            this.Controls.Add(this.dockPanel1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1024, 726);
            this.Name = "MenuPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pontofopag Comunicador";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.MenuPrincipal_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MenuPrincipal_FormClosing);
            this.Load += new System.EventHandler(this.MenuPrincipal_Load);
            this.Shown += new System.EventHandler(this.MenuPrincipal_Shown);
            this.Resize += new System.EventHandler(this.MenuPrincipal_Resize);
            this.cmsTray.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.pnlLogDireita.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlLog.ResumeLayout(false);
            this.dockPnlMenu.ResumeLayout(false);
            this.dockPanel2_Container.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip cmsTray;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listagemDeREPsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.ComponentModel.BackgroundWorker bwJobs;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPnlMenu;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem importarDadosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarDadosToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tspBtnReps;
        private System.Windows.Forms.ToolStripButton tsbImportarDados;
        private System.Windows.Forms.ToolStripButton tsbExportarDados;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbTrocarUsuario;
        private System.Windows.Forms.ToolStripButton tsbSair;
        private System.Windows.Forms.Panel pnlLogDireita;
        public DevExpress.XtraEditors.SimpleButton sbAtualizarAplicacao;
        private System.Windows.Forms.Panel panel1;
        public DevExpress.XtraEditors.SimpleButton sbSitInternet;
        public DevExpress.XtraEditors.SimpleButton sbSitWs;
        public DevExpress.XtraEditors.SimpleButton sbSitRep;
        private System.Windows.Forms.Panel pnlLog;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.ToolStripButton tsbDataHora;
        private System.Windows.Forms.Label lbInfoServer;
        public DevExpress.XtraEditors.SimpleButton btnEnviarLog;
    }
}

