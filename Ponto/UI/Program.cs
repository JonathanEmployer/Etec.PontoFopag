using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.ServiceProcess;

namespace UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("DAL.xml");
                XmlNode no = doc.SelectSingleNode("DAL").SelectSingleNode("bancoDeDados");

                DirectoryInfo dir = new DirectoryInfo(@"XML");
                try
                {
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                string arquivoExe;
                arquivoExe = Assembly.GetEntryAssembly().Location;
                Modelo.cwkGlobal.DirApp = Path.Combine(Path.GetDirectoryName(arquivoExe), "");

                string versao = new Version(Application.ProductVersion).ToString();
                string[] splitVersao = versao.Split('.');

                Modelo.Global.Versao = String.Format("{0}.{1:00}.{2:000}", splitVersao[0], Convert.ToInt32(splitVersao[1]), Convert.ToInt32(splitVersao[2]));
                 Modelo.Global.Revisao = splitVersao[3];

                Modelo.cwkGlobal.ArquivoHelp = @"\Help\CworkPontoPlus.chm";

                if (Modelo.cwkGlobal.DirApp[Modelo.cwkGlobal.DirApp.Length - 1] == '\\')
                {
                    string aux = Modelo.cwkGlobal.DirApp;
                    Modelo.cwkGlobal.DirApp = String.Empty;
                    Modelo.cwkGlobal.DirApp = aux.Substring(0, aux.Length - 1);
                }

                if (!cwkControleUsuario.Facade.ValidaDAL())
                {
                    Application.Exit();
                    return;
                }

                BLL.Empresa bllEmpresa = new BLL.Empresa();
                string mensagem;
                if (!cwkControleUsuario.Facade.ChamaAutenticacao(LicenceLibrary.Sistema.Ponto, Modelo.Global.Versao, true))
                {
                    Application.Exit();
                    return;
                }
                else
                {
                    StartMSDTC();
                    if (!bllEmpresa.VersaoOK(out mensagem))
                    {
                        MessageBox.Show(mensagem);
                        Application.Exit();
                        return;
                    }
                    else
                    {
                        if (bllEmpresa.EmpresasValidas())
                        {
                            BLL.Parametros bllParametros = new BLL.Parametros();
                            Modelo.Empresa emp = bllEmpresa.GetEmpresaPrincipal();
                            DateTime UltimoAcesso = bllEmpresa.GetUltimoAcesso();
                            DateTime Validade = emp.Validade.GetValueOrDefault().Date;

                            if ((DateTime.Now.Date < UltimoAcesso) && 
                                (!(Modelo.cwkGlobal.objUsuarioLogado.Nome.Equals("revenda") ||
                                   Modelo.cwkGlobal.objUsuarioLogado.Nome.Equals("cwork"))))
                            {
                                MessageBox.Show("\r\nA data do sistema é menor que a data do último acesso ("
                                    + UltimoAcesso.ToShortDateString()
                                    + "). Corrija as configurações de data e hora do computador.");
                                Application.Exit();
                                return;
                            }

                            if ((!(Modelo.cwkGlobal.objUsuarioLogado.Nome.Equals("revenda") ||
                                   Modelo.cwkGlobal.objUsuarioLogado.Nome.Equals("cwork"))) &&
                                   Validade < DateTime.Now.Date)
                            {
                                MessageBox.Show("\r\nLicença expirada em " + emp.Validade.GetValueOrDefault().Date.ToShortDateString() +
                                    ". Entre em contato com a revenda.");
                                bllEmpresa.SetUltimoAcesso();
                                Application.Exit();
                                return;
                            }
                            try
                            {
                                bllParametros.Backup(1);
                                bllEmpresa.SetUltimoAcesso();
                                if (emp.Chave != emp.HashMD5ComRelatoriosValidacaoNova())
                                {
                                    emp.Chave = emp.HashMD5ComRelatoriosValidacaoNova();
                                    bllEmpresa.Salvar(Modelo.Acao.Alterar, emp);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }

                            int limiteFunc;
                            if (bllEmpresa.ValidaLicenca(out limiteFunc, true))
                            {
                                Application.Run(new MenuInicial());
                            }
                            else
                            {
                                MessageBox.Show("A quantidade de funcionarios excedeu ao limite de " + limiteFunc + " funcionarios. " +
                                    "Entre em contato com a revenda para adquirir a licença para mais funcionarios.");
                                Application.Exit();
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Cópia ilegal do software.");

                            Application.Exit();
                            return;
                        }
                    }
                }
                if (Modelo.cwkGlobal.objUsuarioLogado.Nome.Equals("revenda") || 
                    Modelo.cwkGlobal.objUsuarioLogado.Nome.Equals("cwork"))
                {
                    Modelo.Empresa empresa = bllEmpresa.GetEmpresaPrincipal();
                    bllEmpresa.SetUltimoAcesso();
                    if (empresa.Chave != empresa.HashMD5ComRelatoriosValidacaoNova())
                    {
                        empresa.Chave = empresa.HashMD5ComRelatoriosValidacaoNova();
                        bllEmpresa.Salvar(Modelo.Acao.Alterar, empresa);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro durante a execução do programa. Verifique: \n" + ex.Message);
            }
        }

        static void StartMSDTC()
        {
            ServiceController svc = new ServiceController("msdtc");
            try
            {
                if (svc.Status != ServiceControllerStatus.Running)
                {
                    svc.Start();
                    svc.WaitForStatus(ServiceControllerStatus.Running);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Houve um erro ao iniciar o serviço MSDTC, " + 
                    "necessário para a comunicação com o Banco de Dados. " + 
                    "Verifique se este usuário possui privilégios suficientes, " + 
                    "ou inicie o serviço MSDTC manualmente. \r\n\r\n" + e.Message, 
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }


}
