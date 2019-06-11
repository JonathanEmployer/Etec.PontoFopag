using cwkAtualizadorComunicadorPontoWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using cwkComunicadorWebAPIPontoWeb.Utils;
using cwkComunicadorWebAPIPontoWeb.ViewModels;
using Microsoft;

namespace cwkComunicadorWebAPIPontoWeb.Forms
{
    public partial class FormAtualizacaoAplicacao : Form
    {
        public Progress<ReportaErro> progress { get; set; }
        AtualizarAplicativo AtualizarApp;
        public bool AtualizarDireto { get; set; }
        public FormAtualizacaoAplicacao(Progress<ReportaErro> progresso, bool atualizarDireto)
        {
            InitializeComponent();
            progress = progresso;
            AtualizarDireto = atualizarDireto;
        }

        private void FormAtualizacaoAplicacao_Load(object sender, EventArgs e)
        {
            try
            {
                AtualizarApp = new AtualizarAplicativo(progress);
                String versaoAtual, versaoFTP;
                List<ArquivosAtualizacaoViewModel> lArquivosAtualizar = AtualizarApp.VerificaAtualizacao(out versaoAtual, out versaoFTP);
                txtbVersaoInstalada.Text = versaoAtual;
                txtbVersaoAtual.Text = versaoFTP;
                lbProgress.Text = "";
                lbArquivo.Text = "Sistema atualizado!";
                if (lArquivosAtualizar.Count() > 0)
                {
                    gcArquivosAtualizar.DataSource = lArquivosAtualizar;
                    txtbVersaoInstalada.Text = versaoAtual;
                    txtbVersaoAtual.Text = versaoFTP;
                    lbArquivo.Text = lArquivosAtualizar.Count() + " Arquivos a serem atualizados";
                    double totalKB = lArquivosAtualizar.Sum(x => x.TamanhoKB);
                    int totalKBint;
                    Int32.TryParse(totalKB.ToString(), out totalKBint);
                    progressBar.Maximum = totalKBint;
                    lbProgress.Text = "0 / " + totalKBint.ToString() + " KB";
                    if (AtualizarDireto)
                    {
                        btnAtualizar.Enabled = false;
                        btnAtualizar.Text = "Atualizando o Sistema, Por Favor Aguarde!";
                        this.Text = "Atualizando o Sistema, Por Favor Aguarde!";
                    }
                    else
                    {
                        btnAtualizar.Enabled = true;
                        btnAtualizar.Text = "Atualizar";
                        this.Text = "Controle de Versão";
                    }
                }
                else
                {
                    btnAtualizar.Enabled = false;
                    btnAtualizar.Text = "Atualizar";
                    this.Text = "Controle de Versão!";
                    MessageBox.Show("Sistema já se encontra atualizado. Versão do sistema: " + versaoAtual);
                }
            }
            catch (Exception ex)
            {
                ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao tentar realizar a atualização do sistema! Erro ao solicitar os arquivos de atualização! Detalhes: " + ex.Message, TipoMsg = TipoMensagem.Erro }, progress);
                CwkUtils.LogarExceptions("Log Atualização do Sistema", ex, CwkUtils.FileLogStringUtil());
                lbArquivo.Text = "Erro ao verificar atualizações, Verifique o log!";
                lbProgress.Text = "Erro: " + ex.Message;
            }
        }



        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                btnAtualizar.Enabled = false;
                gcArquivosAtualizar.DataSource = AtualizarApp.BaixarArquivosParaAtualizacao(txtbVersaoAtual.Text, progressBar, lbProgress, lbArquivo, txtbVersaoInstalada.Text);
                gvArquivosAtualizar.RefreshData();
                btnAtualizar.Enabled = true;
                if (AtualizarDireto)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ReportarProgresso(new ReportaErro() { Mensagem = "Erro ao tentar realizar a atualização do sistema! Erro ao salvar os arquivos do servidor! Detalhes: "+ex.Message, TipoMsg = TipoMensagem.Erro }, progress);
                CwkUtils.LogarExceptions("Log Atualização do Sistema", ex, CwkUtils.FileLogStringUtil());
                btnAtualizar.Enabled = true;
                lbArquivo.Text = "Erro ao realizar o download dos arquivos, Verifique o log!";
                lbProgress.Text = "Erro: " + ex.Message;
                progressBar.Minimum = 0;
            }
        }

        private void ReportarProgresso(ReportaErro texto, IProgress<ReportaErro> progress)
        {
            if (progress != null)
            {
                progress.Report(texto);
            }
        }

        private void FormAtualizacaoAplicacao_Shown(object sender, EventArgs e)
        {
            if (AtualizarDireto)
            {
                btnAtualizar_Click(sender, e);
            }
        }

        private void lbProgress_Click(object sender, EventArgs e)
        {

        }

        private void progressBar_Click(object sender, EventArgs e)
        {

        }

        private void FormAtualizacaoAplicacao_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnAtualizar.Enabled == false && btnAtualizar.Text != "Atualizar")
            {
                MessageBox.Show("O sistema está sendo atualizado, por favor aguarde!");
                return;
            }
        }


    }
}
