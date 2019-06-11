using cwkComunicadorWebAPIPontoWeb.BLL;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cwkComunicadorWebAPIPontoWeb
{
    public partial class GridDadosExportacaoRep : Form
    {
        public Progress<ReportaErro> progress { get; set; }

        public GridDadosExportacaoRep(Progress<ReportaErro> progresso)
        {
            InitializeComponent();
            progress = progresso;
            ReportarProgresso(new ReportaErro() { Mensagem = "Iniciando abertura da tela de importação de dados!", TipoMsg = TipoMensagem.Info }, progress);
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;
            progressBar.Hide();
        }

        private async Task<List<ImportacaoDadosRepViewModel>> buscaDadosWS()
        {
            try
            {
                List<ImportacaoDadosRepViewModel> DadosImp = new List<ImportacaoDadosRepViewModel>();
                CancellationToken ct = new CancellationToken();
                LoginBLL loginBll = new LoginBLL();
                RepBLL repBll = new RepBLL();
                ImportacaoDadosRepBLL impBll = new ImportacaoDadosRepBLL();
                TokenResponseViewModel userData = await loginBll.GetXmlRegisterDataAsync();
                ListaRepsViewModel lReps = await repBll.GetXmlRepDataAsync();
                List<int> idsReps = new List<int>();
                if (lReps.Reps.Where(w => w.ImportacaoAtivada).Count() == 0)
                {
                    ReportarProgresso(new ReportaErro() { Mensagem = "Nenhum Rep Ativado para Importação. Entre na Opção \"Config. Reps\" e Ative a opção \"Ativar Importação\" o Rep Desejado!", TipoMsg = TipoMensagem.Aviso }, progress);
                    System.Windows.Forms.MessageBox.Show("Nenhum Rep Ativado para Importação. Entre na Opção \"Config. Reps\" e Ative a opção \"Ativar Importação\" o Rep Desejado!");
                    return new List<ImportacaoDadosRepViewModel>();
                }
                foreach (RepViewModel rep in lReps.Reps.Where(x => x.ImportacaoAtivada == true))
                {
                    idsReps.Add(rep.Id);
                }
                ImportacaoDadosRepBLL idr = new ImportacaoDadosRepBLL();
                DadosImp = await idr.GetImportacaoDadosRepAsync(idsReps, userData.AccessToken, VariaveisGlobais.URL_WS, ct, progress);
                return DadosImp;
            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                {
                    ex = ((AggregateException)ex).Flatten();
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in ((AggregateException)ex).InnerExceptions)
                    {
                        sb.Append(item.Message);
                    }
                    MessageBox.Show(sb.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return new List<ImportacaoDadosRepViewModel>();
            }
        }

        private async void GridDadosExportacaoRep_Load(object sender, EventArgs e)
        {
            List<ImportacaoDadosRepViewModel> DadosImp = await buscaDadosWS();
            if (DadosImp.Count() > 0)
            {
                gcImportacaoes.DataSource = DadosImp;
                ImportacaoDadosRepViewModel selecionado = new ImportacaoDadosRepViewModel();
                int handle = gvImportacoes.GetSelectedRows()[0];
                selecionado = (ImportacaoDadosRepViewModel)gvImportacoes.GetRow(handle);
                CarregarGridEmpresa(selecionado);
                CarregarGridFuncionarios(selecionado);
                sbEnviarDadosRep.Enabled = true;
            }
            else
            {
                sbEnviarDadosRep.Enabled = false;
            }
        }

        private void CarregarGridEmpresa(ImportacaoDadosRepViewModel selecionado)
        {
            IList<EmpresaViewModel> EmpresasN = new List<EmpresaViewModel>();
            foreach (cwkPontoMT.Integracao.Entidades.Empresa emp in selecionado.Empresas)
            {
                EmpresaViewModel empN = new EmpresaViewModel();
                empN.TipoDocumento = emp.TipoDocumento;
                empN.Documento = emp.Documento;
                empN.CEI = emp.CEI;
                empN.RazaoSocial = emp.RazaoSocial;
                empN.Local = emp.Local;
                EmpresasN.Add(empN);
            }
            gcEmpresas.DataSource = EmpresasN;
        }
        private void CarregarGridFuncionarios(ImportacaoDadosRepViewModel selecionado)
        {
            IList<EmpregadoViewModel> EmpregadoN = new List<EmpregadoViewModel>();
            foreach (cwkPontoMT.Integracao.Entidades.Empregado func in selecionado.Empregados)
            {
                EmpregadoViewModel funcN = new EmpregadoViewModel();
                funcN.Biometria = func.Biometria;
                funcN.DsCodigo = func.DsCodigo;
                funcN.Nome = func.Nome;
                funcN.Pis = func.Pis;
                funcN.Senha = func.Senha;
                EmpregadoN.Add(funcN);
            }

            gcFuncionarios.DataSource = EmpregadoN;
        }

        private void ReportarProgresso(ReportaErro texto, IProgress<ReportaErro> progress)
        {
            if (progress != null)
            {
                progress.Report(texto);
            }
        }

        private void gvImportacoes_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvImportacoes.RowCount > 0)
            {
                ImportacaoDadosRepViewModel selecionado = new ImportacaoDadosRepViewModel();
                int handle = gvImportacoes.GetSelectedRows()[0];
                if (handle >= 0)
                {
                    selecionado = (ImportacaoDadosRepViewModel)gvImportacoes.GetRow(handle);
                    CarregarGridEmpresa(selecionado);
                    CarregarGridFuncionarios(selecionado);
                }
            }
        }

        private async void sbEnviarDadosRep_Click(object sender, EventArgs e)
        {
            progressBar.Show();
            string textoBotao = sbEnviarDadosRep.Text;
            try
            {
                sbEnviarDadosRep.Text = "Aguarde!";
                sbEnviarDadosRep.Enabled = false;
                sbFechar.Enabled = false;
                CancellationToken ct = new CancellationToken();
                LoginBLL loginBll = new LoginBLL();
                RepBLL repBll = new RepBLL();
                ImportacaoDadosRepBLL impBll = new ImportacaoDadosRepBLL();
                TokenResponseViewModel userData = await loginBll.GetXmlRegisterDataAsync();
                ListaRepsViewModel lReps = await repBll.GetXmlRepDataAsync();
                ReportarProgresso(new ReportaErro() { Mensagem = "Enviando dados para o relógio", TipoMsg = TipoMensagem.Info }, progress);
                await impBll.EnviarEmpresaFuncionarios(lReps.Reps, userData.AccessToken, ViewModels.VariaveisGlobais.URL_WS, ct, progress);
                ReportarProgresso(new ReportaErro() { Mensagem = "Dados Enviados para o relógio com sucesso!", TipoMsg = TipoMensagem.Sucesso }, progress);
                MessageBox.Show("Dados Enviados", "Comunicador", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                {
                    ex = ((AggregateException)ex).Flatten();
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in ((AggregateException)ex).InnerExceptions)
                    {
                        sb.Append(item.Message);
                    }
                    MessageBox.Show(sb.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ReportarProgresso(new ReportaErro() { Mensagem = sb.ToString(), TipoMsg = TipoMensagem.Erro }, progress);
                }
                else
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ReportarProgresso(new ReportaErro() { Mensagem = ex.Message, TipoMsg = TipoMensagem.Erro }, progress);
                }
            }
            finally
            {
                progressBar.Hide();
                sbEnviarDadosRep.Text = textoBotao;
                sbEnviarDadosRep.Enabled = true;
                sbFechar.Enabled = true;
            }
        }

        private void sbFechar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
