using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Transactions;
using DAL.SQL;
using ConversorMultBanco;

namespace ConversorSQL
{
    public partial class Form1 : Form
    {
        private static string VERSAO;


        public Form1()
        {
            InitializeComponent();
            string versao = new Version(Application.ProductVersion).ToString();
            string[] splitVersao = versao.Split('.');
            VERSAO = String.Format("{0}.{1:00}.{2:000}", splitVersao[0], Convert.ToInt32(splitVersao[1]), Convert.ToInt32(splitVersao[2]));
        }

        private void Converter()
        {
            button1.Enabled = false;
            Modelo.cwkGlobal.objUsuarioLogado = new Modelo.Cw_Usuario();
            Modelo.cwkGlobal.objUsuarioLogado.Login = "cwork";
            if (cwkControleUsuario.Facade.ValidaDAL())
            {
                DataBase db = new DataBase(Modelo.cwkGlobal.CONN_STRING);
                txtLog.Text = "Iniciando conversão...";
                try
                {
                    bool converterSenhas = false;
                    using (TransactionScope trans = new TransactionScope())
                    {
                        
                        AtualizarLog(Versao201029.Converter(db));
                        Versao202030.Converter(db);
                        Versao203033.Converter(db);
                        Versao204035.Converter(db);
                        Versao205036.Converter(db);
                        Versao206037.Converter(db);
                        Versao207039.Converter(db);
                        Versao208040.Converter(db);
                        Versao210046.Converter(db);
                        Versao211047.Converter(db);
                        Versao211048.Converter(db);
                        Versao212049.Converter(db);
                        Versao213050.Converter(db);
                        Versao214051.Converter(db);
                        Versao215052.Converter(db);
                        Versao216053.Converter(db);
                        Versao217054.Converter(db);
                        Versao302002.Converter(db);
                        Versao303003.Converter(db);
                        Versao304004.Converter(db);
                        Versao305005.Converter(db);
                        Versao306006.Converter(db);
                        Versao307007.Converter(db);
                        Versao308008.Converter(db);
                        Versao309009.Converter(db);
                        Versao310010.Converter(db);
                        Versao311011.Converter(db);
                        Versao312012.Converter(db);
                        Versao313013.Converter(db);
                        Versao314014.Converter(db);
                        Versao315015.Converter(db);
                        Versao316016.Converter(db); 
                        Versao317017.Converter(db);
                        Versao318018.Converter(db);
                        Versao319019.Converter(db);
                        Versao320020.Converter(db);
                        Versao321021.Converter(db);
                        Versao322023.Converter(db);
                        Versao323024.Converter(db);

                        if (!EncriptaSenhas.CampoSenhaConvertido())
                        {
                            EncriptaSenhas.AumentarCampoSenha(out converterSenhas);
                        }
                        ConversorMultBanco.Converter.AtualizarVersao(db, VERSAO);

                        trans.Complete();
                    }

                    using (TransactionScope trans = new TransactionScope())
                    {

                        Versao202030.AtualizarHorarioPHExtra(db);
                        if (converterSenhas)
                        {
                            AtualizarLog(EncriptaSenhas.Encriptar());
                        }
                        Versao205036.AtualizarAfastamentosParciais(db);
                        
                        trans.Complete();
                    }

                    AtualizarLog("Base de dados atualizada para a versão "+ VERSAO +".");
                }
                catch (Exception ex)
                {
                    AtualizarLog("Erro ao realizar conversão:\n" + ex.ToString());
                }
                AtualizarLog("Clique no botão Fechar para sair.");
            }
            else
            {
                AtualizarLog("Configure corretamente a conexão com o banco de dados antes de efetuar a conversão.");
            }
            button1.Enabled = true;
        }

        private void AtualizarLog(string mensagem)
        {
            txtLog.Lines = (txtLog.Text + "\n" + mensagem).Split(new char[] { '\n' });
            txtLog.Select(txtLog.Text.Length, 0);
            txtLog.ScrollToCaret();
            txtLog.Refresh();
            Application.DoEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Converter();
        }


    }
}
