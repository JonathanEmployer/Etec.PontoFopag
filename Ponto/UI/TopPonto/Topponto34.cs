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
    public class Topponto34 : IImportacaoTopponto
    {
        private string arquivo;
        private BLL.Afastamento bllAfastamento;
        private BLL.Ocorrencia bllOcorrencia;
        private BLL.Funcionario bllFuncionario;
        private BLL.Empresa bllEmpresa;
        private BLL.Departamento bllDepartamento;
        private BLL.BilhetesImp bllBilhetes;
        private BLL.ImportacaoTopPonto bllImportacaoTopPonto;
        private BLL.Feriado bllFeriado;
        private BLL.Funcao bllFuncao;
        private BLL.Horario bllHorario;
        private BLL.HorarioDetalhe bllHorarioDetalhe;
        private BLL.Parametros bllParametro;
        private BLL.Jornada bllJornada;
        private Topponto34(string parquivo)
        {
            arquivo = parquivo;
            bllAfastamento = new BLL.Afastamento();
            bllOcorrencia = new BLL.Ocorrencia();
            bllFuncionario = new BLL.Funcionario();
            bllEmpresa = new BLL.Empresa();
            bllDepartamento = new BLL.Departamento();
            bllBilhetes = new BLL.BilhetesImp();
            bllImportacaoTopPonto = new BLL.ImportacaoTopPonto();
            bllFeriado = new BLL.Feriado();
            bllFuncao = new BLL.Funcao();
            bllHorario = new BLL.Horario();
            bllHorarioDetalhe = new BLL.HorarioDetalhe();
            bllParametro = new BLL.Parametros();
            bllJornada = new BLL.Jornada();
        }

        #region Singleton
        private static Topponto34 instancia;
        public static Topponto34 getInstance(string pArquivo)
        {
            if (instancia == null) instancia = new Topponto34(pArquivo);
            return instancia;
        }
        #endregion

        #region Importação das Tabelas

        public void Afastamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            //Prepara a Conexao e os sql para importacao dos Dados do Access
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());
            DataTable dt = new DataTable();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.AppendLine("SELECT A.Tipo, A.Inicio, A.Fim, A.Motivo, A.Identificacao, A.Codigo");
                sql.AppendLine("FROM Afastamento A");
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

            //Cria as BLL´s e Objeto 
            

            //Atualiza o ProgressBar da importação de funcionario
            pProgressBar.Properties.Step = 1;
            pProgressBar.Properties.PercentView = true;
            pProgressBar.Properties.Minimum = 0;
            pProgressBar.Properties.Maximum = dt.Rows.Count;

            UI.FormProgressBar2 barradeprogresso = new UI.FormProgressBar2();
            bllAfastamento.ObjProgressBar = barradeprogresso.ObjProgressBar;

            //Verifica se existe registro para importar
            if (dt.Rows.Count > 0)
            {
                //Processa os registros importando para o PontoMT
                try
                {
                    //Busca os hashtables com o código (descrição para função) e o id correspondente
                    Hashtable empresas = bllImportacaoTopPonto.GetHashEmpresa();
                    Hashtable departamentos = bllImportacaoTopPonto.GetHashDepartamento();
                    Hashtable funcionarios = bllImportacaoTopPonto.GetHashFuncionario();
                    Hashtable ocorrencias = bllImportacaoTopPonto.GetHashOcorrenciaDescId();
                    int auxId = 0;
                    string cartao, motivo;
                    List<Modelo.Afastamento> afastamentos = new List<Modelo.Afastamento>();
                    foreach (DataRow row in dt.Rows)
                    {
                        Modelo.Afastamento objAfast = new Modelo.Afastamento();
                        objAfast.Codigo = Convert.ToInt32(row["Codigo"]);
                        objAfast.Datai = Convert.ToDateTime(row["Inicio"]);
                        objAfast.Dataf = Convert.ToDateTime(row["Fim"]);
                        objAfast.Abonado = 1;

                        if (Convert.ToInt32(row["Tipo"]) == 0)
                        {
                            objAfast.Tipo = 2;
                            auxId = Convert.ToInt32(row["Identificacao"]);
                            if (empresas.ContainsKey(auxId))
                            {
                                objAfast.IdEmpresa = (int)empresas[auxId];
                            }
                        }
                        else if (Convert.ToInt32(row["Tipo"]) == 1)
                        {
                            objAfast.Tipo = 1;
                            auxId = Convert.ToInt32(row["Identificacao"]);
                            if (departamentos.ContainsKey(auxId))
                            {
                                objAfast.IdDepartamento = (int)departamentos[auxId];
                            }
                        }
                        else if (Convert.ToInt32(row["Tipo"]) == 2)
                        {
                            objAfast.Tipo = 0;
                            cartao = Convert.ToString(row["Identificacao"]);
                            if (funcionarios.ContainsKey(cartao))
                            {
                                objAfast.IdFuncionario = (int)funcionarios[cartao];
                            }
                        }

                        motivo = Convert.ToString(row["Motivo"]).TrimEnd().Replace("\"", String.Empty);
                        if (!ocorrencias.ContainsKey(motivo))
                        {
                            if (!String.IsNullOrEmpty(motivo.TrimEnd()))
                            {
                                Modelo.Ocorrencia objOcorrencia = new Modelo.Ocorrencia();
                                objOcorrencia.Codigo = bllOcorrencia.MaxCodigo();
                                objOcorrencia.Descricao = motivo;
                                bllOcorrencia.Salvar(Modelo.Acao.Incluir, objOcorrencia);

                                ocorrencias.Add(motivo, objOcorrencia.Id);
                                objAfast.IdOcorrencia = objOcorrencia.Id;
                            }
                            else
                                continue;
                        }
                        else
                        {
                            objAfast.IdOcorrencia = (int)ocorrencias[motivo];
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

        public void Bilhetes(DevExpress.XtraEditors.ProgressBarControl pProgressBar, DateTime? pData, DateTime? pDataFinal)
        {
            //Prepara a Conexao e os sql para importacao dos Dados do Access
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());
            DataTable dt = new DataTable();
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT B.Tipo, B.Data, B.Hora, B.Cartao, B.Inner");
                sql.AppendLine("FROM Bilhete B");
                if (String.Format("{0:dd/MM/yyyy}", pData) != "01/01/0001")
                    sql.AppendLine("WHERE B.Data >= DateValue('" + String.Format("{0:dd/MM/yyyy}", pData) + "')");
                sql.AppendLine("ORDER BY B.Cartao, B.Data, B.Hora");
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
                //Cria as BLL´s e Objeto 
                

                //Atualiza o ProgressBar da importação de funcionario
                pProgressBar.Properties.Step = 1;
                pProgressBar.Properties.PercentView = true;
                pProgressBar.Properties.Minimum = 0;
                pProgressBar.Properties.Maximum = dt.Rows.Count;

                //Processa os registros importando para o PontoMT
                try
                {
                    Hashtable funcionarios = bllImportacaoTopPonto.GetHashFuncionario();

                    //Cria atributos de controle para calcular as marcações
                    string dscodigo = String.Empty;
                    List<Modelo.BilhetesImp> listaBilhetes = new List<Modelo.BilhetesImp>();
                    Modelo.BilhetesImp objBilhete = null;
                    int MaxCodigo = bllBilhetes.MaxCodigo(), count = 0;
                    int periodo = (int)Math.Floor((decimal)dt.Rows.Count / 4);
                    foreach (DataRow row in dt.Rows)
                    {
                        dscodigo = row["Cartao"] is DBNull ? String.Empty : Convert.ToString(row["Cartao"]);
                        if (funcionarios.ContainsKey(dscodigo))
                        {
                            objBilhete = new Modelo.BilhetesImp();
                            objBilhete.Codigo = MaxCodigo;
                            objBilhete.Data = Convert.ToDateTime(row["Data"]);
                            objBilhete.Func = dscodigo;
                            objBilhete.DsCodigo = dscodigo;
                            objBilhete.Ordem = Convert.ToString(row["Tipo"]);
                            objBilhete.Hora = Convert.ToString(row["Hora"]).Substring(11, 5);
                            objBilhete.Importado = 0;
                            objBilhete.Posicao = 0;
                            objBilhete.Ent_sai = String.Empty;
                            if (Convert.ToString(row["Inner"]) == "MA")
                            {
                                objBilhete.Relogio = "01";
                            }
                            else
                            {
                                objBilhete.Relogio = String.Format("{0:00}", Convert.ToInt32(row["Inner"]));
                            }
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
                            continue;

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

        public void Departamento(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Departamento", aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(Codigo) FROM Departamento", aConnection);
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
                    objDepartamento.Codigo = reader.GetInt32(2);
                    objDepartamento.Descricao = reader.GetString(0);
                    objDepartamento.IdEmpresa = bllEmpresa.getId(reader.GetInt32(3), null, null);

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

        public void Feriado(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());
            OleDbCommand cmd = new OleDbCommand("SELECT * FROM Dias", aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(Data) FROM Dias", aConnection);
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
                    Modelo.Feriado objFeriado = new Modelo.Feriado();
                    objFeriado.Codigo = bllFeriado.MaxCodigo();
                    objFeriado.Data = reader.GetDateTime(0);
                    objFeriado.Descricao = reader.GetString(1);
                    UI.FormProgressBar2 barradeprogresso = new UI.FormProgressBar2();
                    bllFeriado.ObjProgressBar = barradeprogresso.ObjProgressBar;

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

        public void Funcionario(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());
            DataTable dt = new DataTable();
            try
            {
                //Prepara a Conexao e os sql para importacao dos Dados do Access
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT F.Cartao, F.Matricula, F.Nome, F.Departamento, F.Funcao, F.Data, F.Jornada, F.Carteira, F.Empresa, J.Tipo");
                sql.AppendLine("FROM Funcionario F, Jornada J");
                sql.AppendLine("WHERE J.Codigo = F.Jornada");
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
                //Processa os registros importando para o PontoMT
                try
                {
                    //Atualiza o ProgressBar da importação de funcionario
                    pProgressBar.Properties.Step = 1;
                    pProgressBar.Properties.PercentView = true;
                    pProgressBar.Properties.Minimum = 0;
                    pProgressBar.Properties.Maximum = dt.Rows.Count;

                    UI.FormProgressBar2 barradeprogresso = new UI.FormProgressBar2();
                    bllFuncionario.ObjProgressBar = barradeprogresso.ObjProgressBar;

                    //Busca os hashtables com o código (descrição para função) e o id correspondente
                    Hashtable empresas = bllImportacaoTopPonto.GetHashEmpresa();
                    Hashtable departamentos = bllImportacaoTopPonto.GetHashDepartamento();
                    Hashtable funcoes = bllImportacaoTopPonto.GetHashFuncaoDescricao();
                    Hashtable horarios = bllImportacaoTopPonto.GetHashHorario();

                    string[] ArrayMatricula;
                    int codigoDepartamento, codigoEmpresa, codigoHorario;
                    string Matricula, descricaoFuncao;

                    List<Modelo.Funcionario> funcionarios = new List<Modelo.Funcionario>();
                    Modelo.Funcionario objFunc;
                    foreach (DataRow row in dt.Rows)
                    {
                        codigoDepartamento = Convert.ToInt32(row["Departamento"]);
                        codigoEmpresa = Convert.ToInt32(row["Empresa"]);
                        codigoHorario = Convert.ToInt32(row["Jornada"]);
                        descricaoFuncao = row["Funcao"].ToString().Replace("\"", String.Empty).TrimEnd().ToUpper();

                        objFunc = new Modelo.Funcionario();
                        objFunc.Codigo = Convert.ToInt32(row["Cartao"]);
                        objFunc.Dscodigo = Convert.ToString(row["Cartao"]).TrimEnd();
                        objFunc.Nome = Convert.ToString(row["Nome"]).TrimEnd();
                        objFunc.Carteira = Convert.ToString(row["Carteira"]).TrimEnd();

                        ArrayMatricula = Convert.ToString(row["Matricula"]).Split('/');
                        if (ArrayMatricula.Length > 1)
                        {
                            Matricula = ArrayMatricula[0] + ArrayMatricula[1];
                        }
                        else
                        {
                            Matricula = ArrayMatricula[0];
                        }
                        objFunc.Matricula = Matricula.TrimEnd();

                        if (row["Data"].ToString() != String.Empty)
                            objFunc.Dataadmissao = Convert.ToDateTime(row["Data"]);

                        //Pega o id do departamento. Se não encontrar o funcionário não é incluído
                        if (departamentos.ContainsKey(codigoDepartamento))
                            objFunc.Iddepartamento = (int)departamentos[codigoDepartamento];
                        else
                        {
                            continue;
                        }

                        //Pega o id da empresa. Se não encontrar, o funcionário não é incluído
                        if (empresas.ContainsKey(codigoEmpresa))
                            objFunc.Idempresa = (int)empresas[codigoEmpresa];
                        else
                        {
                            continue;
                        }

                        //Pega o id da função. Se não encontrar, a função é incluída no banco
                        if (funcoes.ContainsKey(descricaoFuncao))
                            objFunc.Idfuncao = (int)funcoes[descricaoFuncao];
                        else
                        {
                            //Caso a função não exista, inclui uma nova função e pega o ID
                            Modelo.Funcao objFuncao = new Modelo.Funcao();
                            objFuncao.Codigo = bllFuncao.MaxCodigo();
                            objFuncao.Descricao = descricaoFuncao;
                            bllFuncao.Salvar(Modelo.Acao.Incluir, objFuncao);
                            objFunc.Idfuncao = objFuncao.Id;
                            funcoes.Add(objFuncao.Descricao, objFuncao.Id);
                        }

                        //Pega o id do horário. Se não encontrar, o funcionário não é incluído
                        if (horarios.ContainsKey(codigoHorario))
                            objFunc.Idhorario = (int)horarios[codigoHorario];
                        else
                        {
                            continue;
                        }

                        objFunc.Pis = ".";
                        objFunc.Funcionarioativo = 1;

                        if (row["Tipo"].ToString() == "Semanal")
                            objFunc.Tipohorario = 1;
                        else
                            objFunc.Tipohorario = 2;

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

        public void Horario(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
            //Prepara a Conexao com o banco access e prepara o sql para pegar os dados
            OleDbConnection aConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + arquivo.Trim());
            OleDbCommand cmd = new OleDbCommand("SELECT J.Codigo, J.Descricao, J.Tipo FROM Jornada J", aConnection);
            OleDbCommand cmd2 = new OleDbCommand("SELECT COUNT(J.Codigo) FROM Jornada J", aConnection);
            aConnection.Open();
            int dia = 0;
            DateTime DiaSemana;

            //Cria os objetos para serem utilizado para armazenar as informações vindas do access
            
            Modelo.Parametros objParametro = bllParametro.LoadPrimeiro();

            //Cria as variáveis auxiliares para controle
            string[] entradas = new string[] { "--:--", "--:--", "--:--", "--:--" };
            string[] saidas = new string[] { "--:--", "--:--", "--:--", "--:--" };

            string totaltrabalhadadiurna;
            string totaltrabalhadanoturna;

            //Executa o sql para importação
            OleDbDataReader reader = cmd.ExecuteReader();

            DataTable dt = new DataTable();

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
                    if (reader.GetString(2) == "Semanal")
                        objHorario.TipoHorario = 1;
                    else
                        objHorario.TipoHorario = 2;

                    //Cria o sql para pegar os horários da jornada que está sendo processada
                    StringBuilder sql = new StringBuilder();
                    if (objHorario.TipoHorario == 1)
                    {
                        sql.AppendLine("SELECT S.Dia");
                        sql.AppendLine(", S.Entrada1 AS Ent1");
                        sql.AppendLine(", S.Saida1 AS Sai1");
                        sql.AppendLine(", S.Entrada2 AS Ent2");
                        sql.AppendLine(", S.Saida2 AS Sai2");
                        sql.AppendLine(", '' AS Data");
                        sql.AppendLine("FROM Semanal AS S");
                        sql.AppendLine("WHERE (S.Codigo = " + objHorario.Codigo.ToString() + ")");
                        sql.AppendLine("ORDER BY S.Dia");
                    }
                    else
                    {
                        sql.AppendLine("SELECT 0 AS Dia");
                        sql.AppendLine(", M.Entrada1 AS Ent1");
                        sql.AppendLine(", M.Saida1 AS Sai1");
                        sql.AppendLine(", M.Entrada2 AS Ent2");
                        sql.AppendLine(", M.Saida2 AS Sai2");
                        sql.AppendLine(", M.Data AS Data");
                        sql.AppendLine("FROM Mensal AS M");
                        sql.AppendLine("WHERE (M.Codigo = " + objHorario.Codigo.ToString() + ")");
                        sql.AppendLine("ORDER BY M.Data");
                        dia = 0;
                    }

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
                                if (objHorario.TipoHorario == 2)
                                {
                                    DiaSemana = Convert.ToDateTime(dr["Data"]);
                                    string diasemana = DiaSemana.DayOfWeek.ToString();
                                    dia = ConverteDiaInt(diasemana);
                                    objHD.Dia = dia;
                                    objHD.Data = Convert.ToDateTime(dr["Data"]);
                                }
                                else
                                {
                                    objHD.Dia = Convert.ToInt32(dr["Dia"]) + 1;
                                }
                                objHD.DiaStr = ConverteDiaStr(objHD.Dia);
                                objHD.Entrada_1 = Convert.ToString(dr["Ent1"]);
                                objHD.Saida_1 = Convert.ToString(dr["Sai1"]);
                                objHD.Entrada_2 = Convert.ToString(dr["Ent2"]);
                                objHD.Saida_2 = Convert.ToString(dr["Sai2"]);
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

                                if (objHorario.TipoHorario == 1)
                                    objHorario.HorariosDetalhe[objHD.Dia - 1] = objHD;
                                else
                                    objHorario.HorariosFlexiveis.Add(objHD);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro! \nMensagem: " + ex.Message);
                        }
                    }
                    dr.Close();

                    //Termina de atribuir os dados dos horários tendo como base os horários de Segunda
                    if (objHorario.TipoHorario != 2)
                    {
                        for (int i = 0; i <= 6; i++)
                        {
                            if (objHorario.HorariosDetalhe[i].Entrada_1 == "--:--")
                            {
                                continue;
                            }
                            else
                            {
                                objHorario.Horariodescricao_1 = objHorario.HorariosDetalhe[i].Entrada_1;
                                objHorario.Horariodescricao_2 = objHorario.HorariosDetalhe[i].Entrada_2;
                                objHorario.Horariodescricao_3 = objHorario.HorariosDetalhe[i].Entrada_3;
                                objHorario.Horariodescricao_4 = objHorario.HorariosDetalhe[i].Entrada_4;

                                objHorario.Horariodescricaosai_1 = objHorario.HorariosDetalhe[i].Saida_1;
                                objHorario.Horariodescricaosai_2 = objHorario.HorariosDetalhe[i].Saida_2;
                                objHorario.Horariodescricaosai_3 = objHorario.HorariosDetalhe[i].Saida_3;
                                objHorario.Horariodescricaosai_4 = objHorario.HorariosDetalhe[i].Saida_4;
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= objHorario.HorariosFlexiveis.Count() - 1; i++)
                        {
                            if (objHorario.HorariosFlexiveis[i].Entrada_1 == "--:--")
                            {
                                continue;
                            }
                            else
                            {
                                objHorario.Horariodescricao_1 = objHorario.HorariosFlexiveis[i].Entrada_1;
                                objHorario.Horariodescricao_2 = objHorario.HorariosFlexiveis[i].Entrada_2;
                                objHorario.Horariodescricao_3 = objHorario.HorariosFlexiveis[i].Entrada_3;
                                objHorario.Horariodescricao_4 = objHorario.HorariosFlexiveis[i].Entrada_4;

                                objHorario.Horariodescricaosai_1 = objHorario.HorariosFlexiveis[i].Saida_1;
                                objHorario.Horariodescricaosai_2 = objHorario.HorariosFlexiveis[i].Saida_2;
                                objHorario.Horariodescricaosai_3 = objHorario.HorariosFlexiveis[i].Saida_3;
                                objHorario.Horariodescricaosai_4 = objHorario.HorariosFlexiveis[i].Saida_4;
                                break;
                            }
                        }
                    }



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

        #region Funções Auxiliares
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

        private int ConverteDiaInt(string pDiaSemana)
        {
            switch (pDiaSemana)
            {
                case "Monday":
                    return 1;
                case "Tuesday":
                    return 2;
                case "Wednesday":
                    return 3;
                case "Thursday":
                    return 4;
                case "Friday":
                    return 5;
                case "Saturday":
                    return 6;
                case "Sunday":
                    return 7;
                default:
                    return 0;
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

        #region Tabelas não importadas
        public void BancoHoras(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
        }

        public void FechamentoBH(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
        }

        public void Funcao(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
        }

        public void Ocorrencia(DevExpress.XtraEditors.ProgressBarControl pProgressBar)
        {
        }
        #endregion
    }
}
