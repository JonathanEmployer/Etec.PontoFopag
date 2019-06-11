using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace Secullum
{
    public class SecullumREP : IImportacaoSecullum
    {
        private string arquivo;
        private BLL.Funcionario bllFuncionario;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.Funcao bllFuncao;

        private SecullumREP(string parquivo)
        {
            arquivo = parquivo;
            bllFuncionario = new BLL.Funcionario();
            bllEmpresa = new BLL.Empresa();
            bllFuncao = new BLL.Funcao();
            bllDepartamento = new BLL.Departamento();
        }

        #region Singleton
        private static SecullumREP instancia;
        public static SecullumREP getInstance(string pArquivo)
        {
            if (instancia == null) instancia = new SecullumREP(pArquivo);
            return instancia;
        }
        #endregion

        #region Importação das Tabelas

        /// <summary>
        /// Método para importar Departamentos
        /// </summary>
        public void Departamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar, int IDEmpresa, ref IList<Modelo.pxyLogErroImportacao> listaLogErro)
        {
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());
            OleDbCommand cmd = new OleDbCommand("SELECT id, Descricao FROM Departamentos", aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(id) FROM Departamentos", aConnection);
            aConnection.Open();

            OleDbDataReader reader = cmd.ExecuteReader();

            Modelo.Departamento objDepartamentoImportacao;            

            Modelo.pxyLogErroImportacao logErro;
            Dictionary<string, string> retornoSalvar;
            
            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = (int)cmd2.ExecuteScalar();


            while (reader.Read())
            {
                logErro = new Modelo.pxyLogErroImportacao();
                retornoSalvar = new Dictionary<string, string>();
                try
                {
                    Modelo.Departamento depCarregado = bllDepartamento.LoadObjectByCodigo(reader.GetInt32(0));
                    if ((depCarregado.Codigo != reader.GetInt32(0)) &&
                        (depCarregado.Descricao != reader.GetString(1)))
                    {
                        objDepartamentoImportacao = new Modelo.Departamento();
                        objDepartamentoImportacao.Codigo = reader.GetInt32(0);
                        objDepartamentoImportacao.Descricao = reader.GetString(1);
                        objDepartamentoImportacao.IdEmpresa = IDEmpresa;

                        retornoSalvar = bllDepartamento.Salvar(Modelo.Acao.Incluir, objDepartamentoImportacao);
                        if (retornoSalvar.Count > 0)
                        {
                            StringBuilder strBuilder = new StringBuilder();
                            logErro.Identificador = reader.GetInt32(0);
                            logErro.Tabela = "Departamentos";
                            foreach (var item in retornoSalvar)
                                strBuilder.AppendLine(item.Key + " " + item.Value);
                            logErro.Erro = strBuilder.ToString();
                            listaLogErro.Add(logErro);
                        }
                    }
                    
                    pProgressBar.PerformStep();
                    pProgressBar.Update();
                    Application.DoEvents();
                }
                catch (OleDbException ex)
                {
                    logErro.Identificador = reader.GetInt32(0);
                    logErro.Tabela = "Departamentos";
                    logErro.Erro = ex.Message;
                    listaLogErro.Add(logErro);
                }
            }
            aConnection.Close();
        }

        /// <summary>
        /// Método para importar os Cargos (Funções)
        /// </summary>
               
        public void Funcao(DevExpress.XtraEditors.ProgressBarControl pProgressBar, ref IList<Modelo.pxyLogErroImportacao> listaLogErro)
        {
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());
            OleDbCommand cmd = new OleDbCommand("SELECT id, descricao FROM funcoes", aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(id) FROM funcoes", aConnection);
            aConnection.Open();

            OleDbDataReader reader = cmd.ExecuteReader();
            
            Modelo.Funcao objFuncaoImportacao = new Modelo.Funcao();

            Modelo.pxyLogErroImportacao logErro;
            Dictionary<string, string> retornoSalvar;

            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = (int)cmd2.ExecuteScalar();
            
            while (reader.Read())
            {
                logErro = new Modelo.pxyLogErroImportacao();
                retornoSalvar = new Dictionary<string, string>();
                try
                {
                    Modelo.Funcao funcaoCarregada = bllFuncao.LoadObjectByCodigo(reader.GetInt32(0));
                    if ((funcaoCarregada.Codigo != reader.GetInt32(0)) &&
                       (funcaoCarregada.Descricao != reader.GetString(1)))
                    {
                        objFuncaoImportacao = new Modelo.Funcao();
                        objFuncaoImportacao.Codigo = reader.GetInt32(0);
                        objFuncaoImportacao.Descricao = reader.GetString(1);
                        retornoSalvar = bllFuncao.Salvar(Modelo.Acao.Incluir, objFuncaoImportacao);
                        if (retornoSalvar.Count > 0)
                        {
                            StringBuilder strBuilder = new StringBuilder();
                            logErro.Identificador = reader.GetInt32(0);
                            logErro.Tabela = "Funções";
                            foreach (var item in retornoSalvar)
                                strBuilder.AppendLine(item.Key + " " + item.Value);
                            logErro.Erro = strBuilder.ToString();
                            listaLogErro.Add(logErro);
                        }
                    }

                    pProgressBar.PerformStep();
                    pProgressBar.Update();
                    Application.DoEvents();
                }
                catch (OleDbException ex)
                {
                    logErro.Identificador = reader.GetInt32(0);
                    logErro.Tabela = "Funções";
                    logErro.Erro = ex.Message;
                    listaLogErro.Add(logErro);
                }
            }
            aConnection.Close();
        }

        /// <summary>
        /// Método para importar os funcionários do Access para PontoMT
        /// </summary>
        /// <param name="pProgressBar"></param>
        public void Funcionario(DevExpress.XtraEditors.ProgressBarControl pProgressBar, int IDEmpresa, int IDHorario, ref IList<Modelo.pxyLogErroImportacao> listaLogErro)
        {
            int idEmpresaAcces = DevolveIDEmpresaBancoAcces(IDEmpresa, bllEmpresa);

            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());
            OleDbCommand cmd = new OleDbCommand("SELECT id, n_folha, n_identificador, nome, departamento_id, funcao_id, n_pis, admissao, demissao FROM funcionarios Where empresa_id = " + idEmpresaAcces, aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(id) FROM funcionarios", aConnection);
            
            aConnection.Open();
            
            OleDbDataReader reader = cmd.ExecuteReader();

           

            Modelo.Funcionario objFuncionarioImportacao;
            Modelo.Empresa objEmpresaImportacao;
            Modelo.Departamento objDepartamentoImportacao;
            Modelo.Funcao objFuncaoImportacao;

            Modelo.pxyLogErroImportacao logErro;
            Dictionary<string, string> retornoSalvar;

            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = (int)cmd2.ExecuteScalar();

           
            while (reader.Read())
            {
                logErro = new Modelo.pxyLogErroImportacao();
                retornoSalvar = new Dictionary<string, string>();

                objFuncionarioImportacao = new Modelo.Funcionario();
                objEmpresaImportacao = new Modelo.Empresa();
                objDepartamentoImportacao = new Modelo.Departamento();
                objFuncaoImportacao = new Modelo.Funcao();

                try
                {
                     Modelo.Funcionario funcionarioCarregado = bllFuncionario.LoadObjectByCodigo(reader.GetInt32(0));
                     if ((funcionarioCarregado.Codigo != reader.GetInt32(0)) &&
                         (funcionarioCarregado.Pis != reader.GetString(6)))
                     {
                         objEmpresaImportacao = bllEmpresa.LoadObject(IDEmpresa);
                         objDepartamentoImportacao = bllDepartamento.LoadObjectByCodigo(reader.GetInt32(4));
                         objFuncaoImportacao = bllFuncao.LoadObjectByCodigo(reader.GetInt32(5));

                         objFuncionarioImportacao.Nome = reader.GetString(3);
                         objFuncionarioImportacao.Codigo = reader.GetInt32(0);
                         objFuncionarioImportacao.bFuncionarioativo = true;

                         retornoSalvar = new Dictionary<string, string>();

                         if (reader.IsDBNull(2))
                         {
                             objFuncionarioImportacao.bFuncionarioativo = false;
                             objFuncionarioImportacao.Datademissao = reader.GetDateTime(8);
                         }
                         else
                             objFuncionarioImportacao.bFuncionarioativo = true;

                         objFuncionarioImportacao.Dscodigo = reader.GetString(1);
                         objFuncionarioImportacao.Empresa = objEmpresaImportacao.Nome;
                         objFuncionarioImportacao.Departamento = objDepartamentoImportacao.Descricao;
                         objFuncionarioImportacao.Funcao = objFuncaoImportacao.Descricao;
                         objFuncionarioImportacao.Pis = reader.GetString(6);

                         objFuncionarioImportacao.Dataadmissao = reader.GetDateTime(7);
                         objFuncionarioImportacao.Idempresa = objEmpresaImportacao.Id;
                         objFuncionarioImportacao.Iddepartamento = objDepartamentoImportacao.Id;
                         objFuncionarioImportacao.Idfuncao = objFuncaoImportacao.Id;
                         objFuncionarioImportacao.Idhorario = IDHorario;


                         if (bllFuncionario.ValidaPis(objFuncionarioImportacao.Pis))
                         {

                             retornoSalvar = bllFuncionario.Salvar(Modelo.Acao.Incluir, objFuncionarioImportacao);
                             if (retornoSalvar.Count > 0)
                             {
                                 StringBuilder strBuilder = new StringBuilder();
                                 logErro.Identificador = reader.GetInt32(0);
                                 logErro.Tabela = "Funcionário";
                                 foreach (var item in retornoSalvar)
                                     strBuilder.AppendLine(item.Key + " " + item.Value);
                                 logErro.Erro = strBuilder.ToString();
                                 listaLogErro.Add(logErro);
                             }

                         }
                         else
                         {
                             logErro.Identificador = reader.GetInt32(0);
                             logErro.Tabela = "Funcionário";
                             logErro.Erro = "PIS inválido";
                             listaLogErro.Add(logErro);
                         }
                     }
                     pProgressBar.PerformStep();
                     pProgressBar.Update();
                     Application.DoEvents();
                }
                catch (OleDbException ex)
                {
                    logErro.Identificador = reader.GetInt32(0);
                    logErro.Tabela = "Funcionário";
                    logErro.Erro = ex.Message;
                    listaLogErro.Add(logErro);
                }
            }
            aConnection.Close();
        }

        private int DevolveIDEmpresaBancoAcces(int IDEmpresa, BLL.Empresa bllEmpresa)
        {
            Modelo.Empresa empresaCarregada = bllEmpresa.LoadObject(IDEmpresa);
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());

            OleDbCommand cmdEmpresa = new OleDbCommand("SELECT id FROM empresas WHERE cnpj = " + "'" + empresaCarregada.Cnpj + "'", aConnection);
            aConnection.Open();

            int idEmpresa = (int)cmdEmpresa.ExecuteScalar();

            aConnection.Close();
            return idEmpresa;
        }


        public bool ValidaEmpresa(int IDEmpresa)
        {
            Modelo.Empresa empresaCarregada = bllEmpresa.LoadObject(IDEmpresa);

            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());
            OleDbCommand cmd = new OleDbCommand("SELECT COUNT(id) FROM empresas WHERE cnpj = " + "'"+empresaCarregada.Cnpj+"'", aConnection);
            aConnection.Open();

            int numEmpresas = (int)cmd.ExecuteScalar();

            if (numEmpresas == 1)
                return true;
            else
                return false;           
        }
        #endregion

        #region Métodos Auxiliares
        //private static void AtribuiComOrdemBilhetes(OleDbDataReader reader, Modelo.Marcacao objMarcacao)
        //{
        //    if (Convert.ToInt32(reader["Tipo"]) == 10)
        //    {
        //        //Entradas
        //        if (objMarcacao.Entrada_1 == "--:--")
        //        {
        //            objMarcacao.Entrada_1 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Ent_num_relogio_1 = "01";
        //        }
        //        else if (objMarcacao.Entrada_2 == "--:--")
        //        {
        //            objMarcacao.Entrada_2 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Ent_num_relogio_2 = "01";
        //        }
        //        else if (objMarcacao.Entrada_3 == "--:--")
        //        {
        //            objMarcacao.Entrada_3 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Ent_num_relogio_3 = "01";
        //        }
        //        else if (objMarcacao.Entrada_4 == "--:--")
        //        {
        //            objMarcacao.Entrada_4 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Ent_num_relogio_4 = "01";
        //        }
        //        else if (objMarcacao.Entrada_5 == "--:--")
        //        {
        //            objMarcacao.Entrada_5 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Ent_num_relogio_5 = "01";
        //        }
        //        else if (objMarcacao.Entrada_6 == "--:--")
        //        {
        //            objMarcacao.Entrada_6 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Ent_num_relogio_6 = "01";
        //        }
        //        else if (objMarcacao.Entrada_7 == "--:--")
        //        {
        //            objMarcacao.Entrada_7 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Ent_num_relogio_7 = "01";
        //        }
        //        else if (objMarcacao.Entrada_8 == "--:--")
        //        {
        //            objMarcacao.Entrada_8 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Ent_num_relogio_8 = "01";
        //        }
        //    }
        //    else
        //    {
        //        //Saidas
        //        if (objMarcacao.Saida_1 == "--:--")
        //        {
        //            objMarcacao.Saida_1 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Sai_num_relogio_1 = "01";
        //        }
        //        else if (objMarcacao.Saida_2 == "--:--")
        //        {
        //            objMarcacao.Saida_2 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Sai_num_relogio_2 = "01";
        //        }
        //        else if (objMarcacao.Saida_3 == "--:--")
        //        {
        //            objMarcacao.Saida_3 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Sai_num_relogio_3 = "01";
        //        }
        //        else if (objMarcacao.Saida_4 == "--:--")
        //        {
        //            objMarcacao.Saida_4 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Sai_num_relogio_4 = "01";
        //        }
        //        else if (objMarcacao.Saida_5 == "--:--")
        //        {
        //            objMarcacao.Saida_5 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Sai_num_relogio_5 = "01";
        //        }
        //        else if (objMarcacao.Saida_6 == "--:--")
        //        {
        //            objMarcacao.Saida_6 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Sai_num_relogio_6 = "01";
        //        }
        //        else if (objMarcacao.Saida_7 == "--:--")
        //        {
        //            objMarcacao.Saida_7 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Sai_num_relogio_7 = "01";
        //        }
        //        else if (objMarcacao.Saida_8 == "--:--")
        //        {
        //            objMarcacao.Saida_8 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //            objMarcacao.Sai_num_relogio_8 = "01";
        //        }
        //    }
        //}

        //private static void AtribuiSemOrdemBilhetes(OleDbDataReader reader, Modelo.Marcacao objMarcacao)
        //{
        //    if (objMarcacao.Entrada_1 == "--:--")
        //    {
        //        objMarcacao.Entrada_1 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Ent_num_relogio_1 = "01";
        //    }
        //    else if (objMarcacao.Saida_1 == "--:--")
        //    {
        //        objMarcacao.Saida_1 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Sai_num_relogio_1 = "01";
        //    }
        //    else if (objMarcacao.Entrada_2 == "--:--")
        //    {
        //        objMarcacao.Entrada_2 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Ent_num_relogio_2 = "01";
        //    }
        //    else if (objMarcacao.Saida_2 == "--:--")
        //    {
        //        objMarcacao.Saida_2 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Sai_num_relogio_2 = "01";
        //    }
        //    else if (objMarcacao.Entrada_3 == "--:--")
        //    {
        //        objMarcacao.Entrada_3 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Ent_num_relogio_3 = "01";
        //    }
        //    else if (objMarcacao.Saida_3 == "--:--")
        //    {
        //        objMarcacao.Saida_3 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Sai_num_relogio_3 = "01";
        //    }
        //    else if (objMarcacao.Entrada_4 == "--:--")
        //    {
        //        objMarcacao.Entrada_4 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Ent_num_relogio_4 = "01";
        //    }
        //    else if (objMarcacao.Saida_4 == "--:--")
        //    {
        //        objMarcacao.Saida_4 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Sai_num_relogio_4 = "01";
        //    }
        //    else if (objMarcacao.Entrada_5 == "--:--")
        //    {
        //        objMarcacao.Entrada_5 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Ent_num_relogio_5 = "01";
        //    }
        //    else if (objMarcacao.Saida_5 == "--:--")
        //    {
        //        objMarcacao.Saida_5 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Sai_num_relogio_5 = "01";
        //    }
        //    else if (objMarcacao.Entrada_6 == "--:--")
        //    {
        //        objMarcacao.Entrada_6 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Ent_num_relogio_6 = "01";
        //    }
        //    else if (objMarcacao.Saida_6 == "--:--")
        //    {
        //        objMarcacao.Saida_6 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Sai_num_relogio_6 = "01";
        //    }
        //    else if (objMarcacao.Entrada_7 == "--:--")
        //    {
        //        objMarcacao.Entrada_7 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Ent_num_relogio_7 = "01";
        //    }
        //    else if (objMarcacao.Saida_7 == "--:--")
        //    {
        //        objMarcacao.Saida_7 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Sai_num_relogio_7 = "01";
        //    }
        //    else if (objMarcacao.Entrada_8 == "--:--")
        //    {
        //        objMarcacao.Entrada_8 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Ent_num_relogio_8 = "01";
        //    }
        //    else if (objMarcacao.Saida_8 == "--:--")
        //    {
        //        objMarcacao.Saida_8 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
        //        objMarcacao.Sai_num_relogio_8 = "01";
        //    }
        //}

        //private string ConverteDiaStr(int pDia)
        //{
        //    switch (pDia)
        //    {
        //        case 1:
        //            return "Seg.";
        //        case 2:
        //            return "Ter.";
        //        case 3:
        //            return "Qua.";
        //        case 4:
        //            return "Qui.";
        //        case 5:
        //            return "Sex.";
        //        case 6:
        //            return "Sáb.";
        //        case 7:
        //            return "Dom.";
        //        default:
        //            return "";
        //    }
        //}

        //private void AuxCalculaHoras(string[] pEntrada, string[] pSaida, out string totalD, out string totalN, Modelo.Parametros objParametros)
        //{
        //    string InicioHNoturna = objParametros.InicioAdNoturno;
        //    string FimHNotura = objParametros.FimAdNoturno;

        //    totalD = "";
        //    totalN = "";

        //    BLL.CalculoHoras.QtdHorasDiurnaNoturnaStr(pEntrada, pSaida, InicioHNoturna, FimHNotura, ref totalD, ref totalN);
        //}
        #endregion
    }
}
