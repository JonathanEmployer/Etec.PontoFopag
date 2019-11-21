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

namespace TopPonto
{
    public class Topponto40 : IImportacaoTopponto
    {
        private string arquivo;
        private BLL.Afastamento bllAfastamento;
        private BLL.ImportacaoTopPonto bllImportacaoTopPonto;
        private BLL.BancoHoras bllBancoHoras;
        private BLL.Funcionario bllFuncionario;
        private BLL.BilhetesImp bllBilhetes;
        private BLL.Horario bllHorario;
        private BLL.Departamento bllDepartamento;
        private BLL.Empresa bllEmpresa;
        private BLL.FechamentoBH bllFechamento;
        private BLL.Feriado bllFeriado;
        private BLL.FechamentoBHD bllFechamentoBHD;
        private BLL.Funcao bllFuncao;
        private BLL.HorarioDetalhe bllHorarioDetalhe;
        private BLL.Parametros bllParametro;
        private BLL.Jornada bllJornada;
        private BLL.Ocorrencia bllOcorrencia;
        private Topponto40(string parquivo)
        {
            arquivo = parquivo;
            bllAfastamento = new BLL.Afastamento();
            bllBancoHoras = new BLL.BancoHoras();
            bllFuncionario = new BLL.Funcionario();
            bllBilhetes = new BLL.BilhetesImp();
            bllHorario = new BLL.Horario();
            bllImportacaoTopPonto = new BLL.ImportacaoTopPonto();
            bllDepartamento = new BLL.Departamento();
            bllEmpresa = new BLL.Empresa();
            bllFechamento = new BLL.FechamentoBH();
            bllFechamentoBHD = new BLL.FechamentoBHD();
            bllFeriado = new BLL.Feriado();
            bllFuncao = new BLL.Funcao();
            bllHorarioDetalhe = new BLL.HorarioDetalhe();
            bllParametro = new BLL.Parametros();
            bllJornada = new BLL.Jornada();
            bllOcorrencia = new BLL.Ocorrencia();
        }

        #region Singleton
        private static Topponto40 instancia;
        public static Topponto40 getInstance(string pArquivo)
        {
            if (instancia == null) instancia = new Topponto40(pArquivo);
            return instancia;
        }
        #endregion

        #region Importação das Tabelas

        /// <summary>
        /// Método para importar os afastamentos do Access para PontoMT
        /// </summary>
        /// <param name="pProgressBar"></param>
        public void Afastamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            //Prepara a Conexao e os sql para importacao dos Dados do Access
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim() + ";Jet OLEDB:Database Password=datatop");
            StringBuilder sql = new StringBuilder();
            DataTable dt = new DataTable();
            try
            {
                sql.AppendLine("SELECT A.CodAfast, A.DtInicio, A.DtFim, A.Abonado, A.CodFunc, A.CodMotivo");
                sql.AppendLine("FROM Afastamentos A");
                OleDbCommand cmd = new OleDbCommand(sql.ToString(), aConnection);
                OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(CodAfast) FROM Afastamentos", aConnection);
                aConnection.Open();

                //Executa a consulta do Access
                dt.Load(cmd.ExecuteReader());
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Erro! \nMensagem: " + ex.Message);
            }
            finally
            {
                aConnection.Close();
            }

            //Verifica se existe registro para importar
            if (dt.Rows.Count > 0)
            {
                //Atualiza o ProgressBar da importação de funcionario
                pProgressBar.Properties.Step = 1;
                pProgressBar.Properties.PercentView = true;
                pProgressBar.Properties.Minimum = 0;
                pProgressBar.Properties.Maximum = dt.Rows.Count;

                //Cria as BLL´s e Objeto 
                

                //Processa os registros importando para o PontoMT
                try
                {
                    
                    Hashtable funcionarios = bllImportacaoTopPonto.GetHashFuncCodigoId();
                    Hashtable ocorrencias = bllImportacaoTopPonto.GetHashOcorrenciaCodigoId();

                    List<Modelo.Afastamento> afastamentos = new List<Modelo.Afastamento>();
                    int codigoFuncionario = 0, codigoOcorrencia = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        Modelo.Afastamento objAfast = new Modelo.Afastamento();
                        objAfast.Codigo = Convert.ToInt32(row["CodAfast"]);
                        objAfast.Datai = Convert.ToDateTime(row["DtInicio"]);
                        objAfast.Dataf = Convert.ToDateTime(row["DtFim"]);
                        objAfast.Abonado = Convert.ToInt16(row["Abonado"]);
                        objAfast.Tipo = 0;
                        objAfast.Acao = Modelo.Acao.Incluir;

                        codigoFuncionario = row["CodFunc"] is DBNull ? 0 : Convert.ToInt32(row["CodFunc"]);
                        codigoOcorrencia = row["CodMotivo"] is DBNull ? 0 : Convert.ToInt32(row["CodMotivo"]);

                        if (funcionarios.ContainsKey(codigoFuncionario))
                        {
                            objAfast.IdFuncionario = (int)funcionarios[codigoFuncionario];
                        }
                        else
                        {
                            continue;
                        }

                        if (ocorrencias.ContainsKey(codigoOcorrencia))
                        {
                            objAfast.IdOcorrencia = (int)ocorrencias[codigoOcorrencia];
                        }
                        else
                        {
                            continue;
                        }

                        afastamentos.Add(objAfast);
                        pProgressBar.PerformStep();
                        pProgressBar.Update();
                        Application.DoEvents();
                    }
                    bllAfastamento.Incluir(afastamentos);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro! \nMensagem: " + ex.Message);
                }
                finally
                {
                    dt.Dispose();
                }
            }

        }

        /// <summary>
        /// Método para importar os Bancos de Horas do Access para PontoMT
        /// </summary>
        /// <param name="pProgressBar"></param>
        public void BancoHoras(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            //Prepara a Conexao e os sql para importacao dos Dados do Access
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim() + ";Jet OLEDB:Database Password=datatop");
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT B.CodBH, BF.CodFunc, BF.DtInicio, BF.DtFim, B.LimiteNormal, B.LimiteFolga, B.LimiteFeriado");
            sql.AppendLine("FROM BancosHoras B, BH_Func BF");
            sql.AppendLine("WHERE B.CodBH = BF.CodBH");
            OleDbCommand cmd = new OleDbCommand(sql.ToString(), aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(BF.CodFunc) FROM BancosHoras B, BH_Func BF WHERE B.CodBH = BF.CodBH", aConnection);
            aConnection.Open();
            //Cria as BLL´s e Objeto 
            

            //Executa a consulta do Access
            OleDbDataReader reader = cmd.ExecuteReader();

            //Atualiza o ProgressBar da importação de funcionario
            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = (int)cmd2.ExecuteScalar();

            try
            {
                //Verifica se existe registro para importar
                if (reader.HasRows)
                {
                    //Processa os registros importando para o PontoMT
                    try
                    {
                        while (reader.Read())
                        {
                            Modelo.BancoHoras objBH = new Modelo.BancoHoras();
                            objBH.Codigo = Convert.ToInt32(reader["CodBH"]);
                            objBH.Bancoprimeiro = 1;
                            objBH.DataInicial = Convert.ToDateTime(reader["DtInicio"]);
                            objBH.DataFinal = Convert.ToDateTime(reader["DtFim"]);
                            objBH.LimiteHoras_1 = Convert.ToString(reader["LimiteNormal"]).Substring(11, 5);
                            objBH.LimiteHoras_2 = Convert.ToString(reader["LimiteNormal"]).Substring(11, 5);
                            objBH.LimiteHoras_3 = Convert.ToString(reader["LimiteNormal"]).Substring(11, 5);
                            objBH.LimiteHoras_4 = Convert.ToString(reader["LimiteNormal"]).Substring(11, 5);
                            objBH.LimiteHoras_5 = Convert.ToString(reader["LimiteNormal"]).Substring(11, 5);
                            objBH.LimiteHoras_6 = Convert.ToString(reader["LimiteNormal"]).Substring(11, 5);
                            objBH.LimiteHoras_7 = Convert.ToString(reader["LimiteNormal"]).Substring(11, 5);
                            objBH.LimiteHoras_8 = Convert.ToString(reader["LimiteFeriado"]).Substring(11, 5);
                            objBH.LimiteHoras_9 = Convert.ToString(reader["LimiteFolga"]).Substring(11, 5);

                            if (objBH.LimiteHoras_8 == "00:00")
                                objBH.LimiteHoras_8 = "--:--";
                            if (objBH.LimiteHoras_9 == "00:00")
                                objBH.LimiteHoras_9 = "--:--";

                            objBH.Dias_1 = objBH.LimiteHoras_1 != "--:--" ? 1 : 0;
                            objBH.Dias_2 = objBH.LimiteHoras_2 != "--:--" ? 1 : 0;
                            objBH.Dias_3 = objBH.LimiteHoras_3 != "--:--" ? 1 : 0;
                            objBH.Dias_4 = objBH.LimiteHoras_4 != "--:--" ? 1 : 0;
                            objBH.Dias_5 = objBH.LimiteHoras_5 != "--:--" ? 1 : 0;
                            objBH.Dias_6 = objBH.LimiteHoras_6 != "--:--" ? 1 : 0;
                            objBH.Dias_7 = objBH.LimiteHoras_7 != "--:--" ? 1 : 0;
                            objBH.Dias_8 = objBH.LimiteHoras_8 != "--:--" ? 1 : 0;
                            objBH.Dias_9 = objBH.LimiteHoras_9 != "--:--" ? 1 : 0;
                            objBH.Tipo = 2;
                            objBH.Identificacao = bllFuncionario.getId(Convert.ToInt32(reader["CodFunc"]), null, null);

                            bllBancoHoras.Salvar(Modelo.Acao.Incluir, objBH);
                            pProgressBar.PerformStep();
                            pProgressBar.Update();
                            Application.DoEvents();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro! \nMensagem: " + ex.Message);
                    }
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Erro! \nMensagem: " + ex.Message);
            }
            finally
            {
                aConnection.Close();
            }
        }

        public void Bilhetes(DevExpress.XtraEditors.ProgressBarControl pProgressBar, DateTime? pData, DateTime? pDataFinal)
        {
            //Prepara a Conexao e os sql para importacao dos Dados do Access
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim() + ";Jet OLEDB:Database Password=datatop");
            StringBuilder sql = new StringBuilder();
            StringBuilder sql3 = new StringBuilder();
            DataTable dt = new DataTable();
            try
            {
                sql.AppendLine("SELECT B.CodFunc, B.DataHora, B.NumInner, B.Tipo");
                sql.AppendLine("FROM Bilhetes B");
                if (String.Format("{0:dd/MM/yyyy}", pData) != "01/01/0001")
                    sql.AppendLine("WHERE B.DataHora >= DateValue('" + String.Format("{0:dd/MM/yyyy}", pData) + "')");
                sql.AppendLine("ORDER BY B.CodFunc, B.DataHora");
                OleDbCommand cmd = new OleDbCommand(sql.ToString(), aConnection);

                aConnection.Open();

                //Executa a consulta do Access
                dt.Load(cmd.ExecuteReader());
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Erro! \nMensagem: " + ex.Message);
            }
            finally
            {
                aConnection.Close();
            }

            //Verifica se existe registro para importar
            if (dt.Rows.Count > 0)
            {
                //Atualiza o ProgressBar da importação de funcionario
                pProgressBar.Properties.Step = 1;
                pProgressBar.Properties.PercentView = true;
                pProgressBar.Properties.Minimum = 0;
                pProgressBar.Properties.Maximum = dt.Rows.Count;

                //Cria atributos de controle para calcular os bilhetes
                Int32 codFuncionario = 0;
                string dscodigo = String.Empty;

                List<Modelo.BilhetesImp> listaBilhetes = new List<Modelo.BilhetesImp>();
                Modelo.BilhetesImp objBilhete = null;

                //Processa os registros importando para o PontoMT
                try
                {
                    Hashtable funcionarios = bllImportacaoTopPonto.GetHashFuncCodigoDscodigo();

                    int MaxCodigo = bllBilhetes.MaxCodigo(), count = 0;
                    int periodo = (int)Math.Floor((decimal)dt.Rows.Count / 4);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["DataHora"].ToString().Length != 19)
                            continue;

                        codFuncionario = row["CodFunc"] is DBNull ? 0 : Convert.ToInt32(row["CodFunc"]);
                        if (funcionarios.ContainsKey(codFuncionario))
                        {
                            dscodigo = (string)funcionarios[codFuncionario];
                            objBilhete = new Modelo.BilhetesImp();
                            objBilhete.Codigo = MaxCodigo;
                            objBilhete.Data = Convert.ToDateTime(row["DataHora"].ToString().Substring(0, 10));
                            objBilhete.Func = dscodigo;
                            objBilhete.DsCodigo = dscodigo;
                            objBilhete.Ordem = String.Format("{0:000}", Convert.ToInt32(row["Tipo"]));
                            objBilhete.Hora = Convert.ToString(row["DataHora"]).Substring(11, 5);
                            objBilhete.Importado = 0;
                            objBilhete.Posicao = 0;
                            objBilhete.Ent_sai = String.Empty;
                            objBilhete.Relogio = String.Format("{0:00}", Convert.ToInt32(row["NumInner"]));
                            objBilhete.Mar_data = objBilhete.Data;
                            objBilhete.Mar_hora = objBilhete.Hora;
                            objBilhete.Mar_relogio = objBilhete.Relogio;

                            listaBilhetes.Add(objBilhete);
                            MaxCodigo++;
                            count++;
                            if (count == periodo)
                            {
                                bllBilhetes.Salvar(Modelo.Acao.Incluir, listaBilhetes);
                                listaBilhetes.Clear();
                                count = 0;
                            }
                        }
                        else
                        {
                            continue;
                        }

                        pProgressBar.PerformStep();
                        pProgressBar.Update();
                        Application.DoEvents();
                    }

                    if (listaBilhetes.Count > 0)
                        bllBilhetes.Salvar(Modelo.Acao.Incluir, listaBilhetes);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro! \nMensagem: " + ex.Message);
                }
                finally
                {
                    dt.Dispose();
                }
            }

        }

        /// <summary>
        /// Método para importar Departamentos
        /// </summary>
        public void Departamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim() + ";Jet OLEDB:Database Password=datatop");
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Departamentos", aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(CodDepto) FROM Departamentos", aConnection);
            aConnection.Open();

            OleDbDataReader reader = cmd.ExecuteReader();

            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = (int)cmd2.ExecuteScalar();

            try
            {
                while (reader.Read())
                {
                    Modelo.Departamento objDepartamento = new Modelo.Departamento();
                    objDepartamento.Codigo = reader.GetInt32(0);
                    objDepartamento.Descricao = reader.GetString(1);
                    objDepartamento.IdEmpresa = bllEmpresa.getId(reader.GetInt32(2), null, null);

                    bllDepartamento.Salvar(Modelo.Acao.Incluir, objDepartamento);
                    pProgressBar.PerformStep();
                    pProgressBar.Update();
                    Application.DoEvents();
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Erro! \nMensagem: " + ex.Message);
            }
            finally
            {
                aConnection.Close();
            }
        }

        /// <summary>
        /// Método para importar os Cargos (Funções)
        /// </summary>
        public void Funcao(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim() + ";Jet OLEDB:Database Password=datatop");
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Cargos", aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(CodCargo) FROM Cargos", aConnection);
            aConnection.Open();
            OleDbDataReader reader = cmd.ExecuteReader();

            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = (int)cmd2.ExecuteScalar();

            try
            {
                while (reader.Read())
                {
                    Modelo.Funcao objFuncao = new Modelo.Funcao();
                    objFuncao.Codigo = reader.GetInt32(0);
                    objFuncao.Descricao = reader.GetString(1);

                    bllFuncao.Salvar(Modelo.Acao.Incluir, objFuncao);
                    pProgressBar.PerformStep();
                    pProgressBar.Update();
                    Application.DoEvents();
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Erro! \nMensagem: " + ex.Message);
            }
            finally
            {
                aConnection.Close();
            }
        }

        /// <summary>
        /// Método para importar os feriados
        /// </summary>
        public void Feriado(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim() + ";Jet OLEDB:Database Password=datatop");
            OleDbCommand cmd = new OleDbCommand("SELECT DISTINCT f1.DtInicio, f1.CodCalendColetivo, f2.Descricao FROM Calend_Func AS f1 INNER JOIN CalendColetivos f2 ON f2.CodCalendColetivo = f1.CodCalendColetivo", aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(v.Data) FROM (SELECT DISTINCT f1.DtInicio AS Data, f1.CodCalendColetivo, f2.Descricao FROM Calend_Func AS f1 INNER JOIN CalendColetivos f2 ON f2.CodCalendColetivo = f1.CodCalendColetivo) AS  V", aConnection);
            aConnection.Open();

            OleDbDataReader reader = cmd.ExecuteReader();

            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = (int)cmd2.ExecuteScalar();

            try
            {
                UI.FormProgressBar2 progressbar = new UI.FormProgressBar2();
                bllFeriado.ObjProgressBar = progressbar.ObjProgressBar;
                while (reader.Read())
                {
                    Modelo.Feriado objFeriado = new Modelo.Feriado();
                    objFeriado.Codigo = reader.GetInt32(1);
                    objFeriado.Data = reader.GetDateTime(0);
                    objFeriado.Descricao = reader.GetString(2);

                    bllFeriado.Salvar(Modelo.Acao.Incluir, objFeriado);
                    pProgressBar.PerformStep();
                    pProgressBar.Update();
                    Application.DoEvents();
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Erro! \nMensagem: " + ex.Message);
            }
            finally
            {
                aConnection.Close();
            }
        }

        /// <summary>
        /// Método para importar os funcionários do Access para PontoMT
        /// </summary>
        /// <param name="pProgressBar"></param>
        public void Funcionario(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            //Prepara a Conexao e os sql para importacao dos Dados do Access
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim() + ";Jet OLEDB:Database Password=datatop");
            DataTable dt = new DataTable();
            //variavel para pegar o horario minimo
            int codigoHorarioMinimo = 0;
            try
            {
                StringBuilder sql = new StringBuilder();
                StringBuilder sql1 = new StringBuilder();

                aConnection.Open();

                sql1.AppendLine("SELECT TOP 1 MIN(CodJornada) AS Jornada FROM Jornadas_Func");
                OleDbCommand cmd3 = new OleDbCommand(sql1.ToString(), aConnection);
                OleDbDataReader DataReader = cmd3.ExecuteReader();
                DataReader.Read();
                codigoHorarioMinimo = Convert.ToInt16(DataReader["Jornada"]);
                DataReader.Close();

                sql.AppendLine("SELECT DISTINCT F.CodFunc, F.Nome, F.Carteira, F.Matricula, F.DtAdmissao, F.DtDemissao, F.CodCargo, F.CodDepto");
                sql.AppendLine(", D.CodEmpresa");
                sql.AppendLine(", (SELECT TOP 1 Crachas.Cartao FROM Crachas WHERE Crachas.CodFunc = F.CodFunc ORDER BY Crachas.DtInicio DESC) AS Cartao");
                sql.AppendLine(", (SELECT CodJornada FROM Jornadas_Func WHERE (SELECT MAX(Jornadas_Func.DtInicio) FROM Jornadas_Func");
                sql.AppendLine("WHERE Jornadas_Func.CodFunc = F.CodFunc) = DtInicio AND CodFunc = F.CodFunc) AS CodJornada");
                sql.AppendLine("FROM Funcionarios F, Departamentos D");
                sql.AppendLine("WHERE F.CodDepto = D.CodDepto");

                OleDbCommand cmd = new OleDbCommand(sql.ToString(), aConnection);

                //Executa a consulta do Access
                dt.Load(cmd.ExecuteReader());
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Erro! \nMensagem: " + ex.Message);
            }
            finally
            {
                aConnection.Close();
            }

            //Verifica se existe registro para importar
            if (dt.Rows.Count > 0)
            {
                //Processa os registros importando para o PontoMT
                try
                {
                    //Atualiza o ProgressBar da importação de funcionario
                    pProgressBar.Properties.Step = 1;
                    pProgressBar.Properties.PercentView = true;
                    pProgressBar.Properties.Minimum = 0;
                    pProgressBar.Properties.Maximum = dt.Rows.Count;
                    UI.FormProgressBar2 progressbar = new UI.FormProgressBar2();
                    bllFuncionario.ObjProgressBar = progressbar.ObjProgressBar;

                    //Busca os hashtables com o código (descrição para função) e o id correspondente
                    Hashtable empresas = bllImportacaoTopPonto.GetHashEmpresa();
                    Hashtable departamentos = bllImportacaoTopPonto.GetHashDepartamento();
                    Hashtable funcoes = bllImportacaoTopPonto.GetHashFuncao();
                    Hashtable horarios = bllImportacaoTopPonto.GetHashHorario();

                    int codigoDepartamento, codigoEmpresa, codigoFuncao, codigoHorario, idHorarioMinimo;

                    if (horarios.ContainsKey(codigoHorarioMinimo))
                        idHorarioMinimo = (int)horarios[codigoHorarioMinimo];
                    else
                        idHorarioMinimo = 0;

                    List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();
                    Modelo.Funcionario objFunc;
                    foreach (DataRow row in dt.Rows)
                    {
                        codigoDepartamento = Convert.ToInt32(row["CodDepto"]);
                        codigoEmpresa = Convert.ToInt32(row["CodEmpresa"]);
                        codigoFuncao = row["CodCargo"] is DBNull ? 0 : Convert.ToInt32(row["CodCargo"]);
                        codigoHorario = row["CodJornada"] is DBNull ? 0 : Convert.ToInt32(row["CodJornada"]);


                        objFunc = new Modelo.Funcionario();
                        objFunc.Dscodigo = Convert.ToString(row["Cartao"]).TrimEnd();
                        if (String.IsNullOrEmpty(objFunc.Dscodigo))
                        {
                            continue;
                        }
                        objFunc.Codigo = Convert.ToInt32(row["CodFunc"]);
                        objFunc.Nome = Convert.ToString(row["Nome"]).TrimEnd();
                        objFunc.Carteira = Convert.ToString(row["Carteira"]).TrimEnd();
                        objFunc.Matricula = Convert.ToString(row["Matricula"]).TrimEnd();

                        if (row["DtAdmissao"].ToString() != "")
                            objFunc.Dataadmissao = Convert.ToDateTime(row["DtAdmissao"]);

                        if (row["DtDemissao"].ToString() != "")
                            objFunc.Datademissao = Convert.ToDateTime(row["DtDemissao"]);

                        //Pega o id do departamento. Se não encontrar o funcionário não é incluído
                        if (departamentos.ContainsKey(codigoDepartamento))
                            objFunc.Iddepartamento = (int)departamentos[codigoDepartamento];
                        else
                        {
                            row["DtDemissao"].ToString();
                            continue;
                        }

                        //Pega o id da empresa. Se não encontrar, o funcionário não é incluído
                        if (empresas.ContainsKey(codigoEmpresa))
                            objFunc.Idempresa = (int)empresas[codigoEmpresa];
                        else
                        {
                            row["DtDemissao"].ToString();
                            continue;
                        }

                        //Pega o id da função
                        if (funcoes.ContainsKey(codigoFuncao))
                        {
                            objFunc.Idfuncao = (int)funcoes[codigoFuncao];
                        }
                        else if (funcoes.Count == 0)
                        {
                            //Caso a função não exista, inclui uma nova função e pega o ID
                            Modelo.Funcao objFuncao = new Modelo.Funcao();
                            objFuncao.Codigo = bllFuncao.MaxCodigo();
                            objFuncao.Descricao = Convert.ToString("GERAL");
                            bllFuncao.Salvar(Modelo.Acao.Incluir, objFuncao);
                            objFunc.Idfuncao = objFuncao.Id;
                            funcoes.Add(objFuncao.Codigo, objFuncao.Id);
                        }
                        else
                        {
                            foreach (var item in funcoes.Values)
                            {
                                objFunc.Idfuncao = Convert.ToInt32(item);
                                break;
                            }
                        }

                        //Pega o id do horário
                        if (horarios.ContainsKey(codigoHorario))
                            objFunc.Idhorario = (int)horarios[codigoHorario];
                        else if (idHorarioMinimo != 0)
                            objFunc.Idhorario = idHorarioMinimo;
                        else
                        {
                            row["DtDemissao"].ToString();
                            continue;
                        }

                        objFunc.Pis = ".";
                        objFunc.Funcionarioativo = 1;
                        objFunc.Tipohorario = 1;

                        funcionarios.Add(objFunc);

                        pProgressBar.PerformStep();
                        pProgressBar.Update();
                        Application.DoEvents();
                    }

                    bllFuncionario.Incluir(funcionarios, false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro! \nMensagem: " + ex.Message);
                }
                finally
                {
                    dt.Dispose();
                }
            }
        }

        /// <summary>
        /// Método para importar os Fechamento de Bancos de Horas do Access para PontoMT
        /// </summary>
        /// <param name="pProgressBar"></param>
        public void FechamentoBH(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            //Prepara a Conexao e os sql para importacao dos Dados do Access
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim() + ";Jet OLEDB:Database Password=datatop");
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT F.CodFunc, F.Data, F.Credito, F.Debito");
            sql.AppendLine("FROM Fechamentos F");
            OleDbCommand cmd = new OleDbCommand(sql.ToString(), aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(F.CodFunc) FROM Fechamentos F", aConnection);
            aConnection.Open();

            //Executa a consulta do Access
            OleDbDataReader reader = cmd.ExecuteReader();

            //Atualiza o ProgressBar da importação de funcionario
            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = (int)cmd2.ExecuteScalar();

            int credito = 0;
            int debito = 0;

            try
            {
                //Verifica se existe registro para importar
                if (reader.HasRows)
                {
                    //Processa os registros importando para o PontoMT
                    try
                    {
                        UI.FormProgressBar2 progressbar = new UI.FormProgressBar2();
                        bllFechamento.ObjProgressBar = progressbar.ObjProgressBar;
                        bllFechamentoBHD.ObjProgressBar = progressbar.ObjProgressBar;
                        while (reader.Read())
                        {


                            //Cria o Fechamento BH para cada funcionário
                            Modelo.FechamentoBH objFBH = new Modelo.FechamentoBH();
                            objFBH.Codigo = bllFechamento.MaxCodigo();
                            objFBH.Data = Convert.ToDateTime(reader["Data"]);
                            objFBH.Efetivado = 1;
                            objFBH.Tipo = 2;
                            objFBH.Identificacao = bllFuncionario.getId(Convert.ToInt32(reader["CodFunc"]), null, null);
                            bllFechamento.Salvar(Modelo.Acao.Incluir, objFBH);

                            //Cria o Fechamento BHD - Tabela onde fica registrado o crédito e débito de fechamento
                            Modelo.FechamentoBHD objBHD = new Modelo.FechamentoBHD();
                            objBHD.Codigo = bllFechamentoBHD.MaxCodigo();
                            objBHD.Idfechamentobh = objFBH.Id;
                            objBHD.Identificacao = objFBH.Identificacao;
                            objBHD.DataFechamento = objFBH.Data;

                            if (reader["Credito"].ToString().Length != 0)
                            {
                                objBHD.Credito = Convert.ToString(reader["Credito"]).Substring(11, 5);
                                credito = Modelo.cwkFuncoes.ConvertHorasMinuto(objBHD.Credito);
                            }
                            else
                            {
                                objBHD.Credito = "--:--";
                                credito = 0;
                            }
                            if (reader["Debito"].ToString().Length != 0)
                            {
                                objBHD.Debito = Convert.ToString(reader["Debito"]).Substring(11, 5);
                                debito = Modelo.cwkFuncoes.ConvertHorasMinuto(objBHD.Debito);
                            }
                            else
                            {
                                objBHD.Debito = "--:--";
                                debito = 0;
                            }

                            objBHD.Seq = 1;
                            objBHD.Saldo = Modelo.cwkFuncoes.ConvertMinutosHora2(5, (credito - debito));
                            objBHD.Saldobh = "00000:00";
                            if (credito > debito)
                                objBHD.Tiposaldo = 0;
                            else
                                objBHD.Tiposaldo = 1;

                            bllFechamentoBHD.Salvar(Modelo.Acao.Incluir, objBHD);
                            pProgressBar.PerformStep();
                            pProgressBar.Update();
                            Application.DoEvents();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro! \nMensagem: " + ex.Message);
                    }
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Erro! \nMensagem: " + ex.Message);
            }
            finally
            {
                aConnection.Close();
            }
        }

        /// <summary>
        /// Método para importar as ocorrencias
        /// </summary>
        public void Ocorrencia(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim() + ";Jet OLEDB:Database Password=datatop");
            OleDbCommand cmd = new OleDbCommand("SELECT CodMotivo, Descricao FROM MotivosAfastamento", aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(CodMotivo) FROM MotivosAfastamento", aConnection);
            aConnection.Open();

            OleDbDataReader reader = cmd.ExecuteReader();

            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = (int)cmd2.ExecuteScalar();

            try
            {
                while (reader.Read())
                {
                    Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
                    objOcorrencia.Codigo = reader.GetInt32(0);
                    objOcorrencia.Descricao = reader.GetString(1);

                    bllOcorrencia.Salvar(Modelo.Acao.Incluir, objOcorrencia);
                    pProgressBar.PerformStep();
                    pProgressBar.Update();
                    Application.DoEvents();
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Erro! \nMensagem: " + ex.Message);
            }
            finally
            {
                aConnection.Close();
            }
        }

        /// <summary>
        /// Método para importar os horários do Access para PontoMT
        /// </summary>
        /// <param name="pProgressBar"></param>
        public void Horario(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            //Prepara a Conexao com o banco access e prepara o sql para pegar os dados
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim() + ";Jet OLEDB:Database Password=datatop");
            OleDbCommand cmd = new OleDbCommand("SELECT J.CodJornada, J.Descricao FROM Jornadas J", aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(J.CodJornada) FROM Jornadas J", aConnection);
            aConnection.Open();

            //Cria os objetos para serem utilizado para armazenar as informações vindas do access
            
            Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

            //Cria as variáveis auxiliares para controle
            string[] entradas = new string[] { "--:--", "--:--", "--:--", "--:--" };
            string[] saidas = new string[] { "--:--", "--:--", "--:--", "--:--" };

            string totaltrabalhadadiurna;
            string totaltrabalhadanoturna;

            //Executa o sql para importação
            OleDbDataReader reader = cmd.ExecuteReader();

            //Atualiza o ProgressBar da importação
            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = (int)cmd2.ExecuteScalar();

            try
            {
                //Processa os registro do access na base do PontoMT
                while (reader.Read())
                {
                    //Atribui as informações do Horário
                    Modelo.Horario objHorario = new Modelo.Horario();
                    objHorario.Codigo = reader.GetInt32(0);
                    objHorario.Descricao = reader.GetString(1);
                    objHorario.Idparametro = bllParametro.getId(1, null, null);
                    objHorario.Horasnormais = 1;
                    objHorario.Limitemin = "03:00";
                    objHorario.Limitemax = "03:00";
                    objHorario.TipoHorario = 1;

                    //Cria o sql para pegar os horários da jornada que está sendo processada
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT v.sequencia, MAX(v.E1) AS Ent1, MAX(v.S1) AS Sai1, MAX(v.E2) AS Ent2, MAX(v.S2) AS Sai2");
                    sql.AppendLine("from (SELECT Horarios_Jornada.Sequencia, Ent_1.Marcacao AS E1, 0 AS S1, 0 AS E2, 0 AS S2");
                    sql.AppendLine("FROM Horarios_Jornada ");
                    sql.AppendLine("LEFT JOIN Marcacoes AS Ent_1 ON (Ent_1.CodHorario = Horarios_Jornada.CodHorario AND  Ent_1.Sequencia = 1)");
                    sql.AppendLine("WHERE (Horarios_Jornada.CodJornada = " + objHorario.Codigo.ToString() + " AND Horarios_Jornada.Sequencia < 8)");
                    sql.AppendLine("union all SELECT Horarios_Jornada.Sequencia, 0 AS E1,Sai_1.Marcacao AS S1,  0 AS E2, 0 AS S2");
                    sql.AppendLine("FROM Horarios_Jornada ");
                    sql.AppendLine("LEFT JOIN Marcacoes AS Sai_1 ON (Sai_1.CodHorario = Horarios_Jornada.CodHorario AND  Sai_1.Sequencia = 2)");
                    sql.AppendLine("WHERE (Horarios_Jornada.CodJornada = " + objHorario.Codigo.ToString() + " AND Horarios_Jornada.Sequencia < 8)");
                    sql.AppendLine("union all SELECT Horarios_Jornada.Sequencia, 0 AS E1,0 AS S1, Ent_2.Marcacao AS E2, 0 AS S2");
                    sql.AppendLine("FROM Horarios_Jornada ");
                    sql.AppendLine("LEFT JOIN Marcacoes AS Ent_2 ON (Ent_2.CodHorario = Horarios_Jornada.CodHorario AND  Ent_2.Sequencia = 3)");
                    sql.AppendLine("WHERE (Horarios_Jornada.CodJornada = " + objHorario.Codigo.ToString() + " AND Horarios_Jornada.Sequencia < 8)");
                    sql.AppendLine("union all SELECT Horarios_Jornada.Sequencia, 0 AS E1,0 AS S1, 0 AS E2, Sai_2.Marcacao AS S2");
                    sql.AppendLine("FROM Horarios_Jornada ");
                    sql.AppendLine("LEFT JOIN Marcacoes AS Sai_2 ON (Sai_2.CodHorario = Horarios_Jornada.CodHorario AND  Sai_2.Sequencia = 4)");
                    sql.AppendLine("WHERE (Horarios_Jornada.CodJornada = " + objHorario.Codigo.ToString() + " AND Horarios_Jornada.Sequencia < 8)");
                    sql.AppendLine(") as v group by v.Sequencia");
                    OleDbCommand cmd3 = new OleDbCommand(sql.ToString(), aConnection);
                    OleDbDataReader dr = cmd3.ExecuteReader();

                    //Verifica se encontrou algum registro
                    if (dr.HasRows)
                    {
                        try
                        {
                            //Processa os horários
                            while (dr.Read())
                            {
                                //Atribui os horários para o objeto HorárioDetalhe
                                Modelo.HorarioDetalhe objHD = new Modelo.HorarioDetalhe();
                                objHD.Codigo = bllHorarioDetalhe.MaxCodigo();
                                objHD.Dia = Convert.ToInt32(dr["Sequencia"]);
                                objHD.DiaStr = ConverteDiaStr(objHD.Dia);
                                objHD.Entrada_1 = Convert.ToString(dr["Ent1"]).Length <= 1 ? "--:--" : Convert.ToString(dr["Ent1"]).Substring(0, 5);
                                objHD.Saida_1 = Convert.ToString(dr["Sai1"]).Length <= 1 ? "--:--" : Convert.ToString(dr["Sai1"]).Substring(0, 5);
                                objHD.Entrada_2 = Convert.ToString(dr["Ent2"]).Length <= 1 ? "--:--" : Convert.ToString(dr["Ent2"]).Substring(0, 5);
                                objHD.Saida_2 = Convert.ToString(dr["Sai2"]).Length <= 1 ? "--:--" : Convert.ToString(dr["Sai2"]).Substring(0, 5);
                                objHD.Entrada_3 = "--:--";
                                objHD.Saida_3 = "--:--";
                                objHD.Entrada_4 = "--:--";
                                objHD.Saida_4 = "--:--";

                                if (objHD.Entrada_1 != "--:--" && objHD.Saida_1 != "--:--")
                                    objHD.bCarregar = 1;

                                objHD.Acao = Modelo.Acao.Incluir;

                                //Calcula a carga horária para o dia
                                entradas[0] = objHD.Entrada_1;
                                entradas[1] = objHD.Entrada_2;
                                entradas[2] = objHD.Entrada_3;
                                entradas[3] = objHD.Entrada_4;
                                saidas[0] = objHD.Saida_1;
                                saidas[1] = objHD.Saida_2;
                                saidas[2] = objHD.Saida_3;
                                saidas[3] = objHD.Saida_4;

                                AuxCalculaHoras(entradas, saidas, out totaltrabalhadadiurna, out totaltrabalhadanoturna, objParametro);

                                objHD.Totaltrabalhadadiurna = (totaltrabalhadadiurna != "00:00" ? totaltrabalhadadiurna : "--:--");
                                objHD.Totaltrabalhadanoturna = (totaltrabalhadanoturna != "00:00" ? totaltrabalhadanoturna : "--:--");

                                objHorario.HorariosDetalhe[objHD.Dia - 1] = objHD;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro! \nMensagem: " + ex.Message);
                        }
                    }
                    dr.Close();

                    //Termina de atribuir os dados dos horários tendo como base os horários de Segunda
                    objHorario.Horariodescricao_1 = objHorario.HorariosDetalhe[0].Entrada_1;
                    objHorario.Horariodescricao_2 = objHorario.HorariosDetalhe[0].Entrada_2;
                    objHorario.Horariodescricao_3 = objHorario.HorariosDetalhe[0].Entrada_3;
                    objHorario.Horariodescricao_4 = objHorario.HorariosDetalhe[0].Entrada_4;

                    objHorario.Horariodescricaosai_1 = objHorario.HorariosDetalhe[0].Saida_1;
                    objHorario.Horariodescricaosai_2 = objHorario.HorariosDetalhe[0].Saida_2;
                    objHorario.Horariodescricaosai_3 = objHorario.HorariosDetalhe[0].Saida_3;
                    objHorario.Horariodescricaosai_4 = objHorario.HorariosDetalhe[0].Saida_4;

                    //Salva o horário no PontoMT
                    bllHorario.Salvar(Modelo.Acao.Incluir, objHorario);
                    pProgressBar.PerformStep();
                    pProgressBar.Update();
                    Application.DoEvents();
                }

                #region Cria Jornadas

                List<Modelo.HorarioDetalhe> horariosDetalhe = bllHorarioDetalhe.GetAllList();
                List<Modelo.Jornada> jornadas = new List<Modelo.Jornada>();
                Modelo.Jornada objJornada;
                int codJornada = 1;
                StringBuilder desc = new StringBuilder();
                foreach (Modelo.HorarioDetalhe hd in horariosDetalhe)
                {
                    if (jornadas.Where(j => j.Entrada_1 == hd.Entrada_1 && j.Entrada_2 == hd.Entrada_2 && j.Entrada_3 == hd.Entrada_3
                        && j.Saida_1 == hd.Saida_1 && j.Saida_2 == hd.Saida_2 && j.Saida_3 == hd.Saida_3 && j.Saida_4 == hd.Saida_4).Count() == 0)
                    {
                        objJornada = new Modelo.Jornada();
                        objJornada.Entrada_1 = hd.Entrada_1;
                        objJornada.Entrada_2 = hd.Entrada_2;
                        objJornada.Entrada_3 = hd.Entrada_3;
                        objJornada.Entrada_4 = hd.Entrada_4;
                        objJornada.Saida_1 = hd.Saida_1;
                        objJornada.Saida_2 = hd.Saida_2;
                        objJornada.Saida_3 = hd.Saida_3;
                        objJornada.Saida_4 = hd.Saida_4;
                        objJornada.Codigo = codJornada++;
                        desc.Remove(0, desc.Length);
                        desc.Append(hd.Entrada_1 + " - " + hd.Saida_1);
                        desc.Append(" - " + hd.Entrada_2 + " - " + hd.Saida_2);
                        desc.Append(" - " + hd.Entrada_3 + " - " + hd.Saida_3);
                        desc.Append(" - " + hd.Entrada_4 + " - " + hd.Saida_4);
                        objJornada.Descricao = desc.ToString();
                        jornadas.Add(objJornada);
                    }
                }

                foreach (Modelo.Jornada jornada in jornadas)
                {
                    if (jornada.Entrada_1 != "--:--" && jornada.Saida_1 != "--:--")
                    {
                        bllJornada.Salvar(Modelo.Acao.Incluir, jornada);
                    }
                }

                bllHorarioDetalhe.AtualizaHorarioDetalheJornada(jornadas);

                #endregion
            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Erro! \nMensagem: " + ex.Message);
            }
            finally
            {
                aConnection.Close();
            }
        }

        #endregion

        #region Métodos Auxiliares
        private static void AtribuiComOrdemBilhetes(OleDbDataReader reader, Modelo.Marcacao objMarcacao)
        {
            if (Convert.ToInt32(reader["Tipo"]) == 10)
            {
                //Entradas
                if (objMarcacao.Entrada_1 == "--:--")
                {
                    objMarcacao.Entrada_1 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Ent_num_relogio_1 = "01";
                }
                else if (objMarcacao.Entrada_2 == "--:--")
                {
                    objMarcacao.Entrada_2 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Ent_num_relogio_2 = "01";
                }
                else if (objMarcacao.Entrada_3 == "--:--")
                {
                    objMarcacao.Entrada_3 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Ent_num_relogio_3 = "01";
                }
                else if (objMarcacao.Entrada_4 == "--:--")
                {
                    objMarcacao.Entrada_4 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Ent_num_relogio_4 = "01";
                }
                else if (objMarcacao.Entrada_5 == "--:--")
                {
                    objMarcacao.Entrada_5 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Ent_num_relogio_5 = "01";
                }
                else if (objMarcacao.Entrada_6 == "--:--")
                {
                    objMarcacao.Entrada_6 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Ent_num_relogio_6 = "01";
                }
                else if (objMarcacao.Entrada_7 == "--:--")
                {
                    objMarcacao.Entrada_7 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Ent_num_relogio_7 = "01";
                }
                else if (objMarcacao.Entrada_8 == "--:--")
                {
                    objMarcacao.Entrada_8 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Ent_num_relogio_8 = "01";
                }
            }
            else
            {
                //Saidas
                if (objMarcacao.Saida_1 == "--:--")
                {
                    objMarcacao.Saida_1 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Sai_num_relogio_1 = "01";
                }
                else if (objMarcacao.Saida_2 == "--:--")
                {
                    objMarcacao.Saida_2 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Sai_num_relogio_2 = "01";
                }
                else if (objMarcacao.Saida_3 == "--:--")
                {
                    objMarcacao.Saida_3 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Sai_num_relogio_3 = "01";
                }
                else if (objMarcacao.Saida_4 == "--:--")
                {
                    objMarcacao.Saida_4 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Sai_num_relogio_4 = "01";
                }
                else if (objMarcacao.Saida_5 == "--:--")
                {
                    objMarcacao.Saida_5 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Sai_num_relogio_5 = "01";
                }
                else if (objMarcacao.Saida_6 == "--:--")
                {
                    objMarcacao.Saida_6 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Sai_num_relogio_6 = "01";
                }
                else if (objMarcacao.Saida_7 == "--:--")
                {
                    objMarcacao.Saida_7 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Sai_num_relogio_7 = "01";
                }
                else if (objMarcacao.Saida_8 == "--:--")
                {
                    objMarcacao.Saida_8 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                    objMarcacao.Sai_num_relogio_8 = "01";
                }
            }
        }

        private static void AtribuiSemOrdemBilhetes(OleDbDataReader reader, Modelo.Marcacao objMarcacao)
        {
            if (objMarcacao.Entrada_1 == "--:--")
            {
                objMarcacao.Entrada_1 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Ent_num_relogio_1 = "01";
            }
            else if (objMarcacao.Saida_1 == "--:--")
            {
                objMarcacao.Saida_1 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Sai_num_relogio_1 = "01";
            }
            else if (objMarcacao.Entrada_2 == "--:--")
            {
                objMarcacao.Entrada_2 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Ent_num_relogio_2 = "01";
            }
            else if (objMarcacao.Saida_2 == "--:--")
            {
                objMarcacao.Saida_2 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Sai_num_relogio_2 = "01";
            }
            else if (objMarcacao.Entrada_3 == "--:--")
            {
                objMarcacao.Entrada_3 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Ent_num_relogio_3 = "01";
            }
            else if (objMarcacao.Saida_3 == "--:--")
            {
                objMarcacao.Saida_3 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Sai_num_relogio_3 = "01";
            }
            else if (objMarcacao.Entrada_4 == "--:--")
            {
                objMarcacao.Entrada_4 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Ent_num_relogio_4 = "01";
            }
            else if (objMarcacao.Saida_4 == "--:--")
            {
                objMarcacao.Saida_4 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Sai_num_relogio_4 = "01";
            }
            else if (objMarcacao.Entrada_5 == "--:--")
            {
                objMarcacao.Entrada_5 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Ent_num_relogio_5 = "01";
            }
            else if (objMarcacao.Saida_5 == "--:--")
            {
                objMarcacao.Saida_5 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Sai_num_relogio_5 = "01";
            }
            else if (objMarcacao.Entrada_6 == "--:--")
            {
                objMarcacao.Entrada_6 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Ent_num_relogio_6 = "01";
            }
            else if (objMarcacao.Saida_6 == "--:--")
            {
                objMarcacao.Saida_6 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Sai_num_relogio_6 = "01";
            }
            else if (objMarcacao.Entrada_7 == "--:--")
            {
                objMarcacao.Entrada_7 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Ent_num_relogio_7 = "01";
            }
            else if (objMarcacao.Saida_7 == "--:--")
            {
                objMarcacao.Saida_7 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Sai_num_relogio_7 = "01";
            }
            else if (objMarcacao.Entrada_8 == "--:--")
            {
                objMarcacao.Entrada_8 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Ent_num_relogio_8 = "01";
            }
            else if (objMarcacao.Saida_8 == "--:--")
            {
                objMarcacao.Saida_8 = Convert.ToString(reader["DataHora"]).Substring(11, 5);
                objMarcacao.Sai_num_relogio_8 = "01";
            }
        }

        private string ConverteDiaStr(int pDia)
        {
            switch (pDia)
            {
                case 1:
                    return "Seg.";
                case 2:
                    return "Ter.";
                case 3:
                    return "Qua.";
                case 4:
                    return "Qui.";
                case 5:
                    return "Sex.";
                case 6:
                    return "Sáb.";
                case 7:
                    return "Dom.";
                default:
                    return "";
            }
        }

        private void AuxCalculaHoras(string[] pEntrada, string[] pSaida, out string totalD, out string totalN, Modelo.Parametros objParametros)
        {
            string InicioHNoturna = objParametros.InicioAdNoturno;
            string FimHNotura = objParametros.FimAdNoturno;

            totalD = "";
            totalN = "";

            BLL.CalculoHoras.QtdHorasDiurnaNoturnaStr(pEntrada, pSaida, InicioHNoturna, FimHNotura, ref totalD, ref totalN);
        }
        #endregion
    }
}
