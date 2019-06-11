using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Drawing.Printing;
using System.Xml;
using System.Runtime.InteropServices;
using UI.Relatorios;
using System.Reflection;
using UI.Util;
using Secullum;

namespace UI
{
    public partial class MenuInicial : Form
    {
        private BLL.Parametros bllParametros;
        private BLL.TipoBilhetes bllTipoBilhetes;
        private BLL.ImportacaoBilhetes bllImportacaoBilhetes;
        private BLL.Empresa bllEmpresa;
        private BLL.Funcionario bllFuncionario;
        private List<string> telasAbertas = new List<string>();
        private bool _toolStripVisivel = true;
        private ToolStripLayoutStyle _toolStripPosicao;
        private DockStyle _dockStyle;

        public MenuInicial()
        {
            InitializeComponent();
            this.Text = "Cwork Ponto Plus (Versão "+ Modelo.Global.Versao +")";
            this.Name = "MenuInicial";
            
            bllParametros = new BLL.Parametros();
            bllTipoBilhetes = new BLL.TipoBilhetes();
            bllImportacaoBilhetes = new BLL.ImportacaoBilhetes();
            bllEmpresa = new BLL.Empresa();
            bllFuncionario = new BLL.Funcionario();
            
            SetaLabelUsuarioLogado();
            AplicaRegraValidadeLicenca();
            if (Modelo.cwkGlobal.BD == 1)
                importacaoBaseDeDadosFirebirdToolStripMenuItem.Visible = true;
            Modelo.Empresa emp = bllEmpresa.GetEmpresaPrincipal();
            absenteísmoToolStripMenuItem.Visible = (emp.Relatorioabsenteismo);
            refeitorioMenuItem.Visible = (emp.ModuloRefeitorio);
            inconsistênciaToolStripMenuItem.Visible = (emp.relatorioInconsistencia);
            comparaçãoDeBilhetesToolStripMenuItem.Visible = (emp.relatorioComparacaoBilhetes);
            InicializaToolStrip();
        }

        private void InicializaToolStrip()
        {
            try
            {
                string user = Modelo.cwkGlobal.objUsuarioLogado.Nome;
                XmlDocument xml = new XmlDocument();
                FileInfo file = new FileInfo(Application.StartupPath + "\\XML\\" + "ConfigTela" + user + ".xml");
                if (file.Exists)
                {
                    xml.Load(Application.StartupPath + "\\XML\\" + "ConfigTela"+user+".xml");
                    XmlNode noPai = xml.SelectSingleNode("Config");
                    if (noPai.SelectSingleNode("posicao").InnerText == ToolStripLayoutStyle.VerticalStackWithOverflow.ToString())
                    {
                        _toolStripPosicao = ToolStripLayoutStyle.VerticalStackWithOverflow;
                    }
                    else
                    {
                        _toolStripPosicao = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                    }

                    switch (noPai.SelectSingleNode("dock").InnerText)
                    {
                        case "Bottom":
                            _dockStyle = DockStyle.Bottom;
                            break;
                        case "Left":
                            _dockStyle = DockStyle.Left;
                            break;
                        case "Top":
                            _dockStyle = DockStyle.Top;
                            break;
                        default:
                            _dockStyle = DockStyle.Right;
                            break;
                    }
                    string teste = noPai.SelectSingleNode("pin").InnerText;
                    _toolStripVisivel = Convert.ToBoolean(teste);
                }
                else
                {
                    posicaoPadrao(ref _toolStripPosicao, ref _dockStyle);
                }
            }
            catch (Exception)
            {
                posicaoPadrao(ref _toolStripPosicao, ref _dockStyle);
            }
            reorganizaToolStrip(_toolStripPosicao, _dockStyle);
        }

        private void posicaoPadrao(ref ToolStripLayoutStyle _toolStripPosicao, ref DockStyle _dockStyle)
        {
            _toolStripPosicao = ToolStripLayoutStyle.VerticalStackWithOverflow;
            _dockStyle = DockStyle.Left;
        }

        [DllImport("Winspool.drv")]
        private static extern bool SetDefaultPrinter(string printerName);

        private void SetaLabelUsuarioLogado()
        {
            toolStripStatusLabel1.Text = "Usuário: " + Modelo.cwkGlobal.objUsuarioLogado.Nome;
        }

        private void AplicaRegraValidadeLicenca()
        {
            if (Modelo.cwkGlobal.objUsuarioLogado.Nome.ToLower().Equals("revenda") ||
                Modelo.cwkGlobal.objUsuarioLogado.Nome.ToLower().Equals("cwork"))
            {
                validadeDaLicencaToolStripMenuItem.Enabled = true;
                validadeDaLicencaToolStripMenuItem.Visible = true;
            }
            else
            {
                validadeDaLicencaToolStripMenuItem.Enabled = false;
                validadeDaLicencaToolStripMenuItem.Visible = false;
            }

            Modelo.Empresa modEmp = bllEmpresa.GetEmpresaPrincipal();
            if (modEmp.Validade.HasValue && modEmp.Validade.GetValueOrDefault().Date < DateTime.MaxValue.Date)
            {
                SetaLabelUsuarioLogado();
                toolStripStatusLabel1.Text += " | Validade da Licença: " + 
                    modEmp.Validade.GetValueOrDefault().ToShortDateString();
            }
            else
            {
                SetaLabelUsuarioLogado();
            }
        }

        public MenuInicial getMenuInicial()
        {
            return this;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            FazerBackup();
        }

        private void FazerBackup()
        {
            try
            {
                FormAguardeBackup form = new FormAguardeBackup(2);
                //form.MdiParent = this;
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sairDoSistemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SairDoSistema();
        }

        private void SairDoSistema()
        {
            bool fechar = true;
            if (telasAbertas.Count > 0)
            {
                fechar = (MessageBox.Show("Deseja realmente sair do sistema?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes);
            }

            if (fechar)
            {
                Application.Exit();
            }
        }

        #region Cadastro

        private void empresaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.FormGridEmpresa form = new UI.FormGridEmpresa();
            UI.Util.Funcoes.ChamaGrid(this, form, "Empresa", telasAbertas);
        }

        private void departamentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.FormGridDepartamento form = new UI.FormGridDepartamento();
            UI.Util.Funcoes.ChamaGrid(this, form, "Departamento", telasAbertas);
        }

        private void funçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.FormGridFuncao form = new UI.FormGridFuncao();
            UI.Util.Funcoes.ChamaGrid(this, form, "Função", telasAbertas);
        }

        private void funcionárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.FormGridFuncionario form = new UI.FormGridFuncionario();
            UI.Util.Funcoes.ChamaGrid(this, form, "Funcionário", telasAbertas);
        }

        private void ocorrênciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.FormGridOcorrencia form = new UI.FormGridOcorrencia();
            UI.Util.Funcoes.ChamaGrid(this, form, "Ocorrência", telasAbertas);
        }

        private void feriadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.FormGridFeriado form = new UI.FormGridFeriado();
            UI.Util.Funcoes.ChamaGrid(this, form, "Feriado", telasAbertas);
        }

        private void compensaçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridCompensacao form = new FormGridCompensacao();
            UI.Util.Funcoes.ChamaGrid(this, form, "Compensação", telasAbertas);
        }

        private void afastamentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridAfastamento form = new FormGridAfastamento();
            UI.Util.Funcoes.ChamaGrid(this, form, "Afastamento", telasAbertas);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FormGridJornada form = new FormGridJornada();
            UI.Util.Funcoes.ChamaGrid(this, form, "Jornada", telasAbertas);
        }

        private void jornadaAlternativaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridJornadaAlternativa form = new FormGridJornadaAlternativa();
            UI.Util.Funcoes.ChamaGrid(this, form, "Jornada Alternativa", telasAbertas);
        }

        private void horárioNormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridHorario form = new FormGridHorario();
            UI.Util.Funcoes.ChamaGrid(this, form, "Horário Normal", telasAbertas);
        }

        private void horárioFlexívelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridHorarioMovel form = new FormGridHorarioMovel();
            UI.Util.Funcoes.ChamaGrid(this, form, "Horário Flexível", telasAbertas);
        }


        private void rEPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridREP form = new FormGridREP();
            UI.Util.Funcoes.ChamaGrid(this, form, "REP", telasAbertas);
        }

        private void justificativaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridJustificativa form = new FormGridJustificativa();
            UI.Util.Funcoes.ChamaGrid(this, form, "Justificativa", telasAbertas);
        }


        #endregion

        #region Banco de Horas

        private void cadastroDeBancoDeHorasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridBancoHoras form = new FormGridBancoHoras();
            UI.Util.Funcoes.ChamaGrid(this, form, "Banco de Horas", telasAbertas);
        }

        private void lançamentoDeCréditoDébitoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridInclusaoBanco form = new FormGridInclusaoBanco();
            UI.Util.Funcoes.ChamaGrid(this, form, "Lançamento de Crédito/Débito", telasAbertas);
        }

        private void fechamentoDoBancoDeHorasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridFechamentoBH form = new FormGridFechamentoBH();
            UI.Util.Funcoes.ChamaGrid(this, form, "Fechamento do Banco de Horas", telasAbertas);
        }

        #endregion

        #region Configuração

        private void parâmetroToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.FormGridParametros form = new UI.FormGridParametros();
            UI.Util.Funcoes.ChamaGrid(this, form, "Parâmetro", telasAbertas);
        }

        private void tipoDeBilheteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridTipoBilhetes form = new FormGridTipoBilhetes();
            UI.Util.Funcoes.ChamaGrid(this, form, "Tipo de Bilhete", telasAbertas);
        }

        private void geralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormManutConfiguracoesGerais form = new FormManutConfiguracoesGerais();
                UI.Util.Funcoes.ChamaManut(this, form, "Configuração Geral", Modelo.Acao.Alterar, telasAbertas);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Manutenção

        private void marcaçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormTabelaMarcacoes form = new FormTabelaMarcacoes(this);
            ShowForm(form, "Marcação");
        }

        private void ShowForm(Form form, String cwTabela)
        {
            if (!form.Text.Contains(cwTabela))
            {
                form.Text += cwTabela;
            }
            if (cwkControleUsuario.Facade.ControleAcesso(form))
            {
                if (!telasAbertas.Contains(form.Name))
                {
                    PropertyInfo[] props = form.GetType().GetProperties();
                    PropertyInfo propTelaAberta = props.FirstOrDefault(w => w.Name == "TelasAbertas");
                    PropertyInfo propCwTabela = props.FirstOrDefault(w => w.Name == "cwTabela");

                    telasAbertas.Add(form.Name);
                    if (propTelaAberta != null)
                    {
                        propTelaAberta.SetValue(form, telasAbertas, null);
                    }
                    if (propCwTabela != null && !String.IsNullOrEmpty(cwTabela))
                    {
                        propCwTabela.SetValue(form, cwTabela, null);
                    }
                    form.MdiParent = this;
                    form.Show();
                }
                else
                {
                    if (toolStripContainer1.ContentPanel.Controls.ContainsKey(form.Name))
                    {
                        toolStripContainer1.ContentPanel.Controls[form.Name].BringToFront();
                    }
                    form.Dispose();
                }
            }
        }

        private void manutençãoDiáriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormManutDiaria form = new FormManutDiaria();
            ShowForm(form, "");
        }

        private void mudançaHorárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormManutMudancaHorario form = new FormManutMudancaHorario();
            UI.Util.Funcoes.ChamaManut(this, form, "Mudança Horário", Modelo.Acao.Incluir, telasAbertas);
        }

        private void códigoProvisórioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridProvisorio form = new FormGridProvisorio();
            UI.Util.Funcoes.ChamaGrid(this, form, "Código Provisório", telasAbertas);
        }

        private void mudaCódigoDoFuncionárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridMudancaCodigo form = new FormGridMudancaCodigo();
            ShowForm(form, "Alteração do Código do Funcionário");
        }

        private void funcionárioExcluídoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridFuncionarioExcluido form = new FormGridFuncionarioExcluido();
            UI.Util.Funcoes.ChamaGrid(this, form, "Funcionário Excluído", telasAbertas);
        }

        private void recalculaMarcacaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRecalculaMarcacao form = new FormRecalculaMarcacao();
            ShowForm(form, "");
        }

        private void importaçãoDeBilheteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormImportacaoBilhetes form = new FormImportacaoBilhetes();
            ShowForm(form, "");
        }

        #endregion

        #region Exportação

        private void exportaçãoFolhaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridLayoutExportacao form = new FormGridLayoutExportacao();
            UI.Util.Funcoes.ChamaGrid(this, form, "Exportação", telasAbertas);
        }

        private void eventosParaExportaçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridEventos form = new FormGridEventos();
            UI.Util.Funcoes.ChamaGrid(this, form, "Eventos Para Exportação", telasAbertas);
        }

        #endregion

        #region Relatório

        #region Relatório Afastamento

        private void porFuncionárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Afastamento.AfastamentoTipo form = new UI.Relatorios.Afastamento.AfastamentoTipo();
            ShowForm(form, "");
        }

        private void porToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Afastamento.AfastamentoOcorrencia form = new UI.Relatorios.Afastamento.AfastamentoOcorrencia();
            ShowForm(form, "");
        }

        #endregion

        #region Relatório Funcionário

        private void porCódigoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Funcionario.FuncionariosPorCodigo form = new UI.Relatorios.Funcionario.FuncionariosPorCodigo();
            ShowForm(form, "");
        }

        private void porDepartamentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Funcionario.FuncionariosPorDepartamento form = new UI.Relatorios.Funcionario.FuncionariosPorDepartamento();
            ShowForm(form, "");
        }

        private void porEmpresaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Funcionario.FuncionariosPorEmpresa form = new UI.Relatorios.Funcionario.FuncionariosPorEmpresa();
            ShowForm(form, "");
        }

        private void porNomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Funcionario.FuncionariosPorNome form = new UI.Relatorios.Funcionario.FuncionariosPorNome();
            ShowForm(form, "");
        }

        private void ativosInativosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Funcionario.FuncionariosAtivosInativos form = new UI.Relatorios.Funcionario.FuncionariosAtivosInativos();
            ShowForm(form, "");
        }

        private void admitidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Funcionario.FuncionariosAdmissao form = new UI.Relatorios.Funcionario.FuncionariosAdmissao();
            ShowForm(form, "");
        }

        private void demitidosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Funcionario.FuncionariosDemissao form = new UI.Relatorios.Funcionario.FuncionariosDemissao();
            ShowForm(form, "");
        }

        private void porDescriçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.HorarioPorDescricao form = new UI.Relatorios.HorarioPorDescricao();
            ShowForm(form, "");
        }

        private void porHorárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Funcionario.FuncionariosHorario form = new UI.Relatorios.Funcionario.FuncionariosHorario();
            ShowForm(form, "");
        }

        private void históricoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Funcionario.FuncionarioHistorico form = new UI.Relatorios.Funcionario.FuncionarioHistorico();
            ShowForm(form, "");
        }

        #endregion

        private void espelhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.EspelhoPonto form = new UI.Relatorios.EspelhoPonto();
            ShowForm(form, "");
        }

        private void presençaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Funcionario.FuncionarioPresenca form = new UI.Relatorios.Funcionario.FuncionarioPresenca();
            ShowForm(form, "");
        }

        private void cartãoPontoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.CartaoPonto.CartaoPonto form = new UI.Relatorios.CartaoPonto.CartaoPonto();
            ShowForm(form, "");
        }

        private void resumoDoBancoDeHorasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.BancoHorasResumo form = new UI.Relatorios.BancoHorasResumo();
            ShowForm(form, "");
        }

        private void horárioNormalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UI.Relatorios.BancoHorasIndividual form = new UI.Relatorios.BancoHorasIndividual();
            ShowForm(form, "");
        }

        private void porPercentualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.PercentualHExtra form = new UI.Relatorios.PercentualHExtra();
            ShowForm(form, "");
        }

        private void porDepartamentoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UI.Relatorios.HExtraPorDepartamento form = new UI.Relatorios.HExtraPorDepartamento();
            ShowForm(form, "");
        }

        private void acumuladoPorFuncionárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.HExtraPorFuncionario form = new UI.Relatorios.HExtraPorFuncionario();
            ShowForm(form, "");
        }

        private void ocorrênciaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UI.Relatorios.FormRelatorioOcorrencia form = new UI.Relatorios.FormRelatorioOcorrencia();
            ShowForm(form, "");
        }

        #endregion

        #region Segurança

        private void grupoDeUsuáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cwkControleUsuario.Facade.ChamaGridGrupoUsuario(this, telasAbertas);
        }

        private void usuáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cwkControleUsuario.Facade.ChamaGridUsuario(this, telasAbertas);
        }

        private void trocaUsuárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cwkControleUsuario.Facade.ChamaAutenticacao(LicenceLibrary.Sistema.Ponto, Modelo.Global.Versao, false))
            {
                SetaLabelUsuarioLogado();
                AplicaRegraValidadeLicenca();
            }
        }

        #endregion

        #region Ajuda

        private void sobreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sobre form = new sobre();
            form.MdiParent = this;
            form.Show();
        }

        #endregion

        #region ToolStrip

        private void tsbBackupSistema_Click(object sender, EventArgs e)
        {
            FormAguardeBackup form = new FormAguardeBackup(3);
            form.MdiParent = this;
            form.Show();
        }

        #endregion

        private void MenuInicial_Load(object sender, EventArgs e)
        {
            menuStrip1.RenderMode = ToolStripRenderMode.Professional;
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new UI.Util.ToolStripColor());
            toolStrip1.Renderer = new ToolStripProfessionalRenderer(new UI.Util.ToolStripColor());
            var empresa = bllEmpresa.GetEmpresaPrincipal();

            Dictionary<int, int> IDEmpresa = CriaDicionarioIDEmpresa();

            try
            {
                switch (IDEmpresa[empresa.IDRevenda])
                {
                    case 242://"Astec"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_Astec;
                        break;
                    case 245://"Comatel"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_Comatel;
                        break;
                    case 236://"Express"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_Express;
                        break;
                    case 160://"Infoponto"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_Infoponto;
                        break;
                    case 296://"SuperSOFT"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_Supersoft;
                        break;
                    case 283://"MTK"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_MTK;
                        break;
                    case 423://"Data Ponto World"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_DataPonto;
                        break;
                    case 435://"InfoTec"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_InfoTec;
                        break;
                    case 437://"Pontual"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_Pontual;
                        break;
                    case 438://"Relocon"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_Relocon;
                        break;
                    case 439://"Reloeste"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_Reloeste;
                        break;
                    case 440://"Mundo Tech"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_MundoTech;
                        break;
                    case 442://"AeB"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_AeB;
                        break;
                    case 445://"Maringá Relógios"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_MaringaRelogios;
                        break;
                    case 454://"Relógio Ponto Timbó"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_Timbo;
                        break;
                    case 458://"Techno Empreendimentos"
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.bg_Techno;
                        break;
                    default:
                        toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.Background;
                        break;
                }
            }
            catch (Exception)
            {
                toolStripContainer1.ContentPanel.BackgroundImage = UI.Properties.Resources.Background;
            }
            
            List<Modelo.Parametros> parametros = bllParametros.GetAllList();
            foreach (Modelo.Parametros parm in parametros)
            {
                if (parm.VerificarBilhetes == 1)
                {
                    ChamaImportacaoBilhetes();
                    break;
                }
            }
        }

        private static Dictionary<int, int> CriaDicionarioIDEmpresa()
        {
            Dictionary<int, int> IDEmpresa = new Dictionary<int, int>();
            IDEmpresa.Add(1, 242);
            IDEmpresa.Add(2, 245);
            IDEmpresa.Add(3, 154);
            IDEmpresa.Add(4, 160);
            IDEmpresa.Add(5, 236);
            IDEmpresa.Add(6, 296);
            IDEmpresa.Add(7, 283);
            IDEmpresa.Add(8, 382);
            IDEmpresa.Add(9, 423);
            IDEmpresa.Add(10, 435);
            IDEmpresa.Add(11, 437);
            IDEmpresa.Add(12, 438);
            IDEmpresa.Add(13, 439);
            IDEmpresa.Add(14, 440);
            IDEmpresa.Add(15, 442);
            IDEmpresa.Add(16, 445);
            IDEmpresa.Add(17, 454);
            IDEmpresa.Add(18, 458);
            return IDEmpresa;
        }

        private void ChamaImportacaoBilhetes()
        {
            List<Modelo.TipoBilhetes> listaTipoBilhetes = bllTipoBilhetes.getListaImportacao();
            if (listaTipoBilhetes.Where(l => l.BImporta && (l.FormatoBilhete == 3 || l.FormatoBilhete == 4) || l.FormatoBilhete == 5).Count() > 0)
            {
                FormPeriodoImportacao form = new FormPeriodoImportacao();
                form.ShowDialog();
                if (form.Importar)
                {
                    ImportarBilhetes(listaTipoBilhetes, form.DataInicial, form.DataFinal);
                }
            }
            else if (MessageBox.Show("Deseja importar os bilhetes?", "Mensagem", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ImportarBilhetes(listaTipoBilhetes, null, null);
            }
        }

        public void ImportarBilhetes(List<Modelo.TipoBilhetes> listaTipoBilhetes, DateTime? dataI, DateTime? dataF)
        {
            FormImportacaoBilhetes form = new FormImportacaoBilhetes();
            if (cwkControleUsuario.Facade.ControleAcesso(form))
            {
                if (form.sbImportar.Enabled == true)
                {
                    this.Enabled = false;
                    this.Focus();
                    FormProgressBar2 pb = new FormProgressBar2();
                    try
                    {
                        pb.Show(this);
                        bool bErro = false;
                        if (listaTipoBilhetes.Count > 0)
                        {
                            string mensagem = "";
                            bool ok = bllImportacaoBilhetes.ImportacaoBilhete(pb.ObjProgressBar, listaTipoBilhetes, "", 0, false, "", dataI, dataF, out mensagem);

                            pb.Close();
                            if (MessageBox.Show(this, mensagem + "\nDeseja visualizar o arquivo de log?", "Mensagem", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                try
                                {
                                    System.Diagnostics.Process.Start(Modelo.cwkGlobal.DirApp + "\\logImportacao.txt");
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Não foi possível abrir o arquivo de log:\n" + ex);
                                }
                            }
                        }
                        else
                        {
                            pb.Close();
                            MessageBox.Show(this, "Para realizar a importação de bilhetes é necessário no mínimo 1 tipo de bilhete.", "Mensagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        pb.Close();
                        MessageBox.Show(this, ex.Message);
                    }
                    this.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Acesso não permitido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tsbImportacaoBilhetes_Click(object sender, EventArgs e)
        {
            ChamaImportacaoBilhetes();
        }

        private void testeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TesteGrid form = new TesteGrid();
            form.Show();
        }

        private void importaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TopPonto.FormImportacaoTopPonto form = new TopPonto.FormImportacaoTopPonto();
            if (cwkControleUsuario.Facade.ControleAcesso(form))
            {
                form.Text = " Importação TopPonto";
                form.ShowInTaskbar = false;
                form.MdiParent = this;
                form.Show();
            }
        }

        private void tsbCalculoHoras_Click(object sender, EventArgs e)
        {
            FormCalc form = new FormCalc();
            if (cwkControleUsuario.Facade.ControleAcesso(form))
            {
                if (!telasAbertas.Contains(form.Name))
                {
                    telasAbertas.Add(form.Name);
                    form.TelasAbertas = telasAbertas;
                    form.Text = " Cálculo de Horas";
                    form.ShowInTaskbar = false;
                    form.Show();
                }
                else
                {
                    form.Dispose();
                }
            }
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Modelo.cwkGlobal.DirApp + Modelo.cwkGlobal.ArquivoHelp, "cworkponto");
        }

        private void ministérioDoTrabahoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormExportaArquivos form = new FormExportaArquivos();
            ShowForm(form, "");
        }

        private void importacaoBaseDeDadosFirebirdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string aux;
            ImportacaoSQL.FormImportacaoSQL form = new ImportacaoSQL.FormImportacaoSQL(Modelo.cwkGlobal.CONN_STRING);
            form.MdiParent = this;
            form.Show();
            BLL.Cw_Usuario bllUsuario = new BLL.Cw_Usuario();
            aux = bllUsuario.GetIdAdmin();
            if (!String.IsNullOrEmpty(aux))
            {
                Modelo.cwkGlobal.objUsuarioLogado = bllUsuario.LoadObject(Convert.ToInt32(aux));
                SetaLabelUsuarioLogado();
            }
        }

        private void alternativoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormGridBackup form = new FormGridBackup();
            UI.Util.Funcoes.ChamaGrid(this, form, "Backup Alternativo", telasAbertas);
        }

        private void configServidorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormConfigServidor form = new FormConfigServidor();
            UI.Util.Funcoes.ChamaManut(this, form, "Configuração Servidor", Modelo.Acao.Alterar, telasAbertas);
        }

        private void MenuInicial_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    manualToolStripMenuItem_Click(sender, e);
                    break;
            }
        }

        private void ManutencaoDiariatoolStripMenuItem_Click(object sender, EventArgs e)
        {
            Relatorios.CartaoPonto.relatorio_manutencao_diaria form = new Relatorios.CartaoPonto.relatorio_manutencao_diaria();
            ShowForm(form, "");
        }

        private void iToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormManutImportaTxt form = new FormManutImportaTxt();
            UI.Util.Funcoes.ChamaManut(this, form, "Importação de Funcionários", Modelo.Acao.Alterar, telasAbertas);
        }

        private void enviarFuncionáriosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new IntegracaoRelogio.FormEnviarEmpresaEFuncionarios();
            ShowForm(form, "");
        }

        private void configurarHorárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IntegracaoRelogio.FormConfiguraHorario form = new IntegracaoRelogio.FormConfiguraHorario();
            ShowForm(form, "");
        }

        private void absenteísmoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new UI.Relatorios.FormAbsenteismo();
            ShowForm(form, "");
        }

        private void equipamentoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.FormGridEquipamento form = new UI.FormGridEquipamento();
            UI.Util.Funcoes.ChamaGrid(this, form, "Equipamento", telasAbertas);
        }

        private void configuracaoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.FormManutConfiguracaoRefeitorio form = new UI.FormManutConfiguracaoRefeitorio();
            UI.Util.Funcoes.ChamaManut(this, form, "Configuração do Refeitório", Modelo.Acao.Alterar, telasAbertas);
        }

        private void relatórioAcessoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormRelatorioAcessoFuncionario form = new FormRelatorioAcessoFuncionario();
            ShowForm(form, "");            
        }

        private void exportaçãoFuncioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja exportar funcionários?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bllFuncionario.ExportacaoRelogioTopData();
                MessageBox.Show("Arquivo gerado com sucesso.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsbPosicaoToolStrip_Click(object sender, EventArgs e)
        {
            if (toolStrip1.Dock == DockStyle.Top)
            {
                _toolStripPosicao = ToolStripLayoutStyle.VerticalStackWithOverflow;
                _dockStyle = DockStyle.Left;
                tsbPosicaoToolStrip.Image = UI.Properties.Resources.BarraInferior481;
                tsbPosicaoToolStrip.ToolTipText = "Move a Barra de Ícones Para a Parte Inferior";
            }
            else if (toolStrip1.Dock == DockStyle.Left)
            {
                _toolStripPosicao = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                _dockStyle = DockStyle.Bottom;
                tsbPosicaoToolStrip.Image = UI.Properties.Resources.BarraDireita481;
                tsbPosicaoToolStrip.ToolTipText = "Move a Barra de Ícones Para a Direita";
            }
            else if (toolStrip1.Dock == DockStyle.Bottom)
            {
                _toolStripPosicao = ToolStripLayoutStyle.VerticalStackWithOverflow;
                _dockStyle = DockStyle.Right;
                tsbPosicaoToolStrip.Image = UI.Properties.Resources.BarraSuperior481;
                tsbPosicaoToolStrip.ToolTipText = "Move a Barra de Ícones Para a Parte Superior";
            }
            else
            {
                _toolStripPosicao = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                _dockStyle = DockStyle.Top;
                tsbPosicaoToolStrip.Image = UI.Properties.Resources.Barraesquerda481;
                tsbPosicaoToolStrip.ToolTipText = "Move a Barra de Ícones Para a Parte a Esquerda";
            }

            reorganizaToolStrip(_toolStripPosicao, _dockStyle);
        }

        private void tsbFixar_Click(object sender, EventArgs e)
        {
            if (_toolStripVisivel)
                _toolStripVisivel = false;
            else _toolStripVisivel = true;
            exibeToolStrip(_toolStripVisivel);
            salvaConfigToolStrip();
        }

        private void exibeToolStrip(bool toolStripVisivel)
        {
            if (toolStripVisivel)
            {
                toolStrip1.LayoutStyle = _toolStripPosicao;
                toolStrip1.Dock = _dockStyle;
                if (_toolStripPosicao == ToolStripLayoutStyle.HorizontalStackWithOverflow)
                {
                    toolStrip1.Height = 55;
                }
                else
                {
                    toolStrip1.Width = 53;
                }
            }
            else
            {
                toolStrip1.AutoSize = false;
                if ((_dockStyle == DockStyle.Top) || (_dockStyle == DockStyle.Bottom))
                {
                    toolStrip1.Height = 10;
                    toolStrip1.Width = 10;
                    toolStrip1.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
                }
                else
                {
                    toolStrip1.Width = 10;
                    toolStrip1.Height = 10;
                    toolStrip1.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
                }
            }
        }

        private void reorganizaToolStrip(ToolStripLayoutStyle toolStripPosicao, DockStyle dockStyle)
        {
            toolStrip1.LayoutStyle = toolStripPosicao;
            toolStrip1.Dock = dockStyle;
            exibeToolStrip(_toolStripVisivel);
            salvaConfigToolStrip();
        }

        private void salvaConfigToolStrip()
        {
            string user = Modelo.cwkGlobal.objUsuarioLogado.Nome;
            string local = Application.StartupPath + "\\XML\\" + "ConfigTela" + user + ".xml";
            XmlTextWriter escritor = new XmlTextWriter(local, System.Text.Encoding.UTF8);
            escritor.Formatting = Formatting.Indented;
            escritor.WriteStartDocument();
            escritor.WriteStartElement("Config");
            escritor.WriteElementString("posicao", _toolStripPosicao.ToString());
            escritor.WriteElementString("dock", _dockStyle.ToString());
            escritor.WriteElementString("pin", _toolStripVisivel.ToString());
            escritor.WriteEndElement();
            escritor.WriteEndDocument();
            escritor.Close();
        }

        private void toolStrip1_MouseEnter(object sender, EventArgs e)
        {
            if (!_toolStripVisivel)
            {
                exibeToolStrip(true);
            }
        }

        private void toolStrip1_MouseLeave(object sender, EventArgs e)
        {
            if (!_toolStripVisivel)
            {
                exibeToolStrip(false);
            }
        }

        private void MenuInicial_MdiChildActivate(object sender, EventArgs e)
        {
            Form f = this.ActiveMdiChild;
            toolStripContainer1.ContentPanel.Controls.Add(f);
            f.BringToFront();
            f.Focus();
        }

        private void validadeDaLicencaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormValidade form = new FormValidade();
            if (!telasAbertas.Contains(form.Name))
            {
                PropertyInfo[] props = form.GetType().GetProperties();
                PropertyInfo propTelaAberta = props.FirstOrDefault(w => w.Name == "TelasAbertas");
                PropertyInfo propCwTabela = props.FirstOrDefault(w => w.Name == "cwTabela");

                telasAbertas.Add(form.Name);
                if (propTelaAberta != null)
                {
                    propTelaAberta.SetValue(form, telasAbertas, null);
                }
                //form.MdiParent = this;
                form.ShowDialog();
                AplicaRegraValidadeLicenca();
            }
            else
            {
                if (toolStripContainer1.ContentPanel.Controls.ContainsKey(form.Name))
                {
                    toolStripContainer1.ContentPanel.Controls[form.Name].BringToFront();
                }
                form.Dispose();
            }
        }

        private void importaçãoDadosSecullumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormImportacaoSecullum form = new FormImportacaoSecullum();
            if (cwkControleUsuario.Facade.ControleAcesso(form))
            {
                form.Text = " Importação Secullum";
                form.ShowInTaskbar = false;
                form.MdiParent = this;
                form.Show();
            }
        }

        private void limiteHorárioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Inconsistencias form = new UI.Relatorios.Inconsistencias();
            ShowForm(form, "");
        }

        private void intervalosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.Intervalos form = new UI.Relatorios.Intervalos();
            ShowForm(form, "");
        }

        private void porMetaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.FormRelatorioMetaHorasExtras form = new UI.Relatorios.FormRelatorioMetaHorasExtras();
            ShowForm(form, "");
        }

        private void comparaçãoDeBilhetesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.ComparacaodeBilhetes form = new UI.Relatorios.ComparacaodeBilhetes();
            ShowForm(form, "");
        }

        private void abonoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UI.Relatorios.FormRelatorioAbono form = new UI.Relatorios.FormRelatorioAbono();
            ShowForm(form, "");
        }
    }
}
