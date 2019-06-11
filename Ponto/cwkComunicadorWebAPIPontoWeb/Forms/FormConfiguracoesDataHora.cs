using cwkComunicadorWebAPIPontoWeb.BLL;
using cwkComunicadorWebAPIPontoWeb.Integracao;
using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cwkComunicadorWebAPIPontoWeb.Forms
{
    public partial class FormConfiguracoesDataHora : Form
    {
        public Progress<ReportaErro> progress { get; set; }
        private List<EnvioConfiguracoesDataHoraViewModel> DadosImp = new List<EnvioConfiguracoesDataHoraViewModel>();
        private readonly CancellationToken ct;

        public FormConfiguracoesDataHora(Progress<ReportaErro> progresso)
        {
            InitializeComponent();
            progress = progresso;
            ReportarProgresso(new ReportaErro() { Mensagem = "Iniciando abertura da tela de importação de dados!", TipoMsg = TipoMensagem.Info }, progress);
            progressBar.Style = ProgressBarStyle.Marquee;
            progressBar.MarqueeAnimationSpeed = 30;
            progressBar.Hide();
        }

        private void ReportarProgresso(ReportaErro texto, IProgress<ReportaErro> progress)
        {
            if (progress != null)
            {
                progress.Report(texto);
            }
        }

        private async void FormConfiguracoesDataHora_Load(object sender, EventArgs e)
        {
            await PreencheDadosTela();
        }

        private async Task PreencheDadosTela()
        {
            LoginBLL loginBll = new LoginBLL();
            TokenResponseViewModel userData = await loginBll.GetXmlRegisterDataAsync();
            RepBLL repBll = new RepBLL();
            ListaRepsViewModel lReps = await repBll.GetXmlRepDataAsync();
            List<RepViewModel> lRepsAtivados = lReps.Reps.Where(x => x.ImportacaoAtivada == true).ToList();
            ImportacaoConfigDataHoraBLL icdh = new ImportacaoConfigDataHoraBLL();
            DadosImp = await icdh.buscaDadosConfigDataHoraWS(lRepsAtivados, userData.AccessToken, ct, progress, true);
            rbDataHora.Checked = true;
            HabilitaDataHora(rbDataHora.Checked);
            if (DadosImp.Count() > 0)
            {
                gbEnviaConfigDataHora.Enabled = false;
                lbInfoEnviarConfig.Visible = true;
            }
            else
            {
                gbEnviaConfigDataHora.Enabled = true;
                lbInfoEnviarConfig.Visible = false;
            }
            gcImportacaoes.DataSource = DadosImp;
            ckbRelogio.DataSource = lRepsAtivados;
            ckbRelogio.ValueMember = "Id";
            ckbRelogio.DisplayMember = "DescRelogio";
        }

        private void rbDataHora_Click(object sender, EventArgs e)
        {
            HabilitaDataHora(rbDataHora.Checked);
        }

        private void HabilitaDataHora(bool marca)
        {
            ckbDataHoraComputador.Enabled = marca;
            ckbDataHoraComputador.Checked = marca;
            dtPickerDataHora.Enabled = marca;
            dtPickerInicio.Enabled = !marca;
            dtPickerTermino.Enabled = !marca;
            HorarioComputador();
        }

        private void HorarioComputador()
        {
            if ((rbDataHora.Checked && ckbDataHoraComputador.Checked) || !rbDataHora.Checked)
            {
                dtPickerDataHora.Enabled = false;
                dtPickerDataHora.Text = "";
            }
            else
            {
                dtPickerDataHora.Enabled = true;
                dtPickerDataHora.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
            }
        }

        private void ckbDataHoraComputador_CheckStateChanged(object sender, EventArgs e)
        {
            HorarioComputador();
        }

        private void sbFechar_Click_2(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void sbEnviarDadosRep_Click(object sender, EventArgs e)
        {
            RepBLL repBll = new RepBLL();
            ListaRepsViewModel lReps = await repBll.GetXmlRepDataAsync();
            if (!gbEnviaConfigDataHora.Enabled)
            {
                LoginBLL loginBll = new LoginBLL();
                TokenResponseViewModel userData = await loginBll.GetXmlRegisterDataAsync();
                ImportacaoConfigDataHoraBLL icdh = new ImportacaoConfigDataHoraBLL();
                await icdh.EnviarConfigDataHora(lReps.Reps, userData.AccessToken, ct, progress, true);
                await PreencheDadosTela();
            }
            else
            {
                int idRepSelecionado = 0;
                if (ckbRelogio.SelectedItem == null)
                {
                    MessageBox.Show("Nenhum Rep Selecionado para Enviar as Configurações de Data e Hora.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (dtPickerInicio.DateTime > dtPickerTermino.DateTime)
                {
                    MessageBox.Show("Data de Início do Horário de Verão Deve ser Menor que Data Fim.", "Erro" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string selecionado = ckbRelogio.SelectedValue.ToString();
                idRepSelecionado = int.Parse(selecionado);
                RepViewModel rvm = lReps.Reps.Where(x => x.Id == idRepSelecionado).FirstOrDefault();
                bool exibirLog;
                try
                {
                    ConfiguracaoHorario ch = new ConfiguracaoHorario(rvm);
                    bool enviar = false;
                    if (rbDataHora.Checked)
                    {
                        if (!ckbDataHoraComputador.Checked && String.IsNullOrEmpty(dtPickerDataHora.Text))
                        {
                            ReportarProgresso(new ReportaErro() { Mensagem = "Para Enviar Configuração de Data e Hora é Necessário Selecionar a Opção \"Usar Data e Hora do Computador\" ou Informar o Valor a Ser Enviado.", TipoMsg = TipoMensagem.Erro }, progress);
                            MessageBox.Show("Para Enviar Configuração de Data e Hora é Necessário Selecionar a Opção \"Usar Data e Hora do Computador\" ou Informar o Valor a Ser Enviado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (ckbDataHoraComputador.Checked)
                        {
                            ReportarProgresso(new ReportaErro() { Mensagem = "Enviando Data e Hora para o REP " + rvm.DescRelogio + " (ID: " + rvm.Id + ")", TipoMsg = TipoMensagem.Info }, progress);
                            ch.EnviarDataHoraComputador = ckbDataHoraComputador.Checked;
                            enviar = true;
                        }
                        else
                        {
                            ReportarProgresso(new ReportaErro() { Mensagem = "Enviando Data e Hora Digitada para o REP " + rvm.DescRelogio + " (ID: " + rvm.Id + ")", TipoMsg = TipoMensagem.Info }, progress);
                            ch.SetDataHoraAtual(DateTime.Parse(dtPickerDataHora.Text));
                            enviar = true;
                        }

                    }
                    else
                    {
                        if (String.IsNullOrEmpty(dtPickerInicio.Text) || String.IsNullOrEmpty(dtPickerTermino.Text))
                        {
                            ReportarProgresso(new ReportaErro() { Mensagem = "Para Enviar Configuração de Horário de Verão é Necessário Informar os Campos de Início e Término.", TipoMsg = TipoMensagem.Erro }, progress);
                            MessageBox.Show("Para Enviar Configuração de Horário de Verão é Necessário Informar os Campos de Início e Término.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        ReportarProgresso(new ReportaErro() { Mensagem = "Enviando Horário de Verão Informado para o REP " + rvm.DescRelogio + " (ID: " + rvm.Id + ")", TipoMsg = TipoMensagem.Info }, progress);
                        ch.SetHorarioVerao(DateTime.Parse(dtPickerInicio.Text), DateTime.Parse(dtPickerTermino.Text));
                        enviar = true;
                    }
                    if (enviar)
                    {
                        string log = String.Empty;
                        bool enviou = await Task.Factory.StartNew<bool>(() => { return ch.Enviar(out exibirLog, out log); }, ct);
                        if (!enviou)
                        {
                            if (String.IsNullOrEmpty(log))
                            {
                                throw new Exception("Comando não enviado. Possível Causa: Falta de Conexão Com o Rep.");
                            }
                            else
                            {
                                throw new Exception("Comando não enviado. Erro: " + log);
                            }
                        }
                        ReportarProgresso(new ReportaErro() { Mensagem = "Configuração de Horário de Verão Enviada para o REP " + rvm.DescRelogio + " (ID: " + rvm.Id + ")", TipoMsg = TipoMensagem.Sucesso }, progress);
                        MessageBox.Show("Configuração Enviada para o REP com sucesso");
                    }
                }
                catch (Exception exc)
                {
                    ReportarProgresso(new ReportaErro() { Mensagem = "Houve um erro ao enviar os comandos ao REP " + rvm.DescRelogio + " (ID: " + rvm.Id + ") \r\n" + exc.Message, TipoMsg = TipoMensagem.Erro }, progress);
                    CwkUtils.LogarExceptions("Log Envio Data Hora Rep", exc, CwkUtils.FileLogStringUtil());
                }
            }

        }
    }
}
