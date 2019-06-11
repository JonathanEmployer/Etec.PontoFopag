using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;
using System.Linq;

namespace ConversorBaseVersao109027
{
    class Program
    {
        #region Scripts

        const string ATUALIZACAO1 = " CREATE TABLE \"rep\" ( "
                           + "\"id\" INTEGER NOT NULL, "
                           + "\"codigo\" INTEGER NOT NULL, "
                           + "\"numserie\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE, "
                           + "\"local\" VARCHAR(100) CHARACTER SET NONE COLLATE NONE, "
                           + "\"incdata\" TIMESTAMP, "
                           + " \"inchora\" TIMESTAMP, "
                           + " \"incusuario\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE, "
                           + " \"altdata\" TIMESTAMP, "
                           + " \"althora\" TIMESTAMP, "
                           + " \"altusuario\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE, "
                           + " \"numrelogio\" VARCHAR(3) CHARACTER SET NONE COLLATE NONE); ";

        const string ATUALIZACAO2 = " ALTER TABLE \"rep\" ADD CONSTRAINT \"rep_porcodigo\" PRIMARY KEY (\"id\"); ";

        const string ATUALIZACAO3 = " ALTER TABLE \"rep\" ADD CONSTRAINT UQ_REP UNIQUE (\"numrelogio\"); ";

        const string ATUALIZACAO4 = " CREATE GENERATOR \"GEN_rep_id\"; ";

        const string ATUALIZACAO5 = " SET GENERATOR \"GEN_rep_id\" TO 0; ";

        const string ATUALIZACAO6 = " CREATE TRIGGER \"BI_rep_id\" FOR \"rep\" "
                            + " ACTIVE BEFORE INSERT "
                            + " POSITION 0 "
                            + " AS "
                            + " BEGIN "
                            + " IF (NEW.\"id\" IS NULL) THEN "
                            + " NEW.\"id\" = GEN_ID(\"GEN_rep_id\", 1); "
                            + " END; ";

        const string ATUALIZACAO7 = " ALTER TABLE \"empresa\" ADD \"cei\" VARCHAR(20); ";

        const string ATUALIZACAO8 = " ALTER TABLE \"tratamentomarcacao\" ADD \"sequencia\" SMALLINT; ";

        #endregion

        static int count;

        static void Main(string[] args)
        {
            try
            {
                Modelo.Cw_Usuario objUsuario = new Modelo.Cw_Usuario();
                objUsuario.Login = "cwork";
                Modelo.cwkGlobal.objUsuarioLogado = objUsuario;
                Modelo.cwkGlobal.BD = 2;
                Console.WriteLine("Cwork Sistemas");

                FbParameter[] parms = new FbParameter[0];
                #region 1.00.003 para 1.00.004
                try
                {
                    string sql = " SELECT * FROM \"rep\" ";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Conversor 1.00.003 para 1.00.004");

                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        conn.Open();
                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO1, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO2, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO3, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO4, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO5, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO6, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO7, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO8, true, parms);
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine("Ocorreu um erro ao converter a base. Verifique: ");
                                Console.WriteLine(ex.Message);
                                return;
                            }
                        }

                        Console.WriteLine("Conversao efetuada com sucesso....");
                    }
                }
                #endregion

                Versao101007();//Versão 1.01.006 para 1.01.07
                Versao101010();//Outras para 1.01.010

                #region 1.01.010 para 1.01.011

                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    conn.Open();

                    Console.WriteLine("Iniciando correção na tabela de importação de bilhetes...");
                    using (FbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string ATUALIZACAO = "UPDATE \"bilhetesimp\" SET \"mar_data\" = \"data\", \"mar_hora\" = \"hora\", \"mar_relogio\" = \"relogio\" WHERE \"importado\" = 0";

                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO, true, parms);
                            trans.Commit();
                            Console.WriteLine("Correção na tabela de importação de bilhetes realizada com sucesso.");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            Console.WriteLine("Erro ao converter base:");
                            Console.WriteLine(ex.Message);
                            return;
                        }
                    }
                }

                #endregion

                #region 1.01.011 para 1.02.013

                #region Alteração na tabela de fechamento de Banco de Horas

                try
                {
                    string sql = " SELECT \"codigo\" FROM \"fechamentobhd\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        conn.Open();
                        Console.WriteLine("Iniciando correção na tabela de fechamento de banco de horas...");
                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                string ATUALIZACAO = "Alter table \"fechamentobhd\"" +
                                                      " Add \"codigo\" Integer," +
                                                      " Alter column \"credido\" to \"credito\"," +
                                                      " Drop \"tiposaldo\"," +
                                                      " Add \"tiposaldo\" Integer;";

                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO, true, parms);
                                trans.Commit();
                                Console.WriteLine("Correção na tabela de fechamento de banco de horas realizada com sucesso.");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine("Erro ao converter base:");
                                Console.WriteLine(ex.Message);
                                return;
                            }
                        }
                    }
                }
                #endregion

                #region Intervalo Automático no horário
                try
                {
                    string sql = " SELECT \"intervaloautomatico\" FROM \"horario\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Iniciando correção na tabela de horários...");
                    try
                    {
                        string ATUALIZACAO = "Alter table \"horario\"" +
                                              " Add \"intervaloautomatico\" Integer," +
                                              " Add \"preassinaladas1\" Integer," +
                                              " Add \"preassinaladas2\" Integer," +
                                              " Add \"preassinaladas3\" Integer;";

                        DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, ATUALIZACAO, true, parms);
                        Console.WriteLine("Correção na tabela de horários realizada com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao converter base:");
                        Console.WriteLine(ex.Message);
                        return;
                    }

                    Console.WriteLine("Iniciando correção na tabela de horários detalhe...");
                    try
                    {
                        string ATUALIZACAO = "Alter table \"horariodetalhe\"" +
                                              " Add \"intervaloautomatico\" Integer," +
                                              " Add \"preassinaladas1\" Integer," +
                                              " Add \"preassinaladas2\" Integer," +
                                              " Add \"preassinaladas3\" Integer;";

                        DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, ATUALIZACAO, true, parms);
                        Console.WriteLine("Correção na tabela de horários detalhe realizada com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao converter base:");
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
                #endregion

                #region Intervalo Automático na jornada alternativa
                try
                {
                    string sql = " SELECT \"intervaloautomatico\" FROM \"jornadaalternativa\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Iniciando correção na tabela de jornadas alternativas...");
                    try
                    {
                        string ATUALIZACAO = "Alter table \"jornadaalternativa\"" +
                                              " Add \"intervaloautomatico\" Integer," +
                                              " Add \"preassinaladas1\" Integer," +
                                              " Add \"preassinaladas2\" Integer," +
                                              " Add \"preassinaladas3\" Integer;";

                        DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, ATUALIZACAO, true, parms);
                        Console.WriteLine("Correção na tabela de jornadas alternativas realizada com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao converter base:");
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
                #endregion

                #region Marcação - Folga
                try
                {
                    string sql = " SELECT \"folga\" FROM \"marcacao\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Iniciando correção na tabela de marcação...");
                    try
                    {
                        string ATUALIZACAO = "Alter table \"marcacao\"" +
                                              " Add \"folga\" Integer;";

                        DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, ATUALIZACAO, true, parms);
                        Console.WriteLine("Correção na tabela de marcação realizada com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao converter base:");
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }

                #endregion

                #region Cadastro de Backup
                try
                {
                    string sql = " SELECT * FROM \"backup\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Criando tabela de backup...");

                    string ATUALIZACAO1 = " CREATE TABLE \"backup\" ( "
                               + "\"id\" INTEGER NOT NULL, "
                               + "\"codigo\" INTEGER NOT NULL, "
                               + "\"descricao\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE, "
                               + "\"diretorio\" VARCHAR(100) CHARACTER SET NONE COLLATE NONE, "
                               + "\"incdata\" TIMESTAMP, "
                               + " \"inchora\" TIMESTAMP, "
                               + " \"incusuario\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE, "
                               + " \"altdata\" TIMESTAMP, "
                               + " \"althora\" TIMESTAMP, "
                               + " \"altusuario\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE); ";

                    string ATUALIZACAO2 = " ALTER TABLE \"backup\" ADD CONSTRAINT \"backup_porcodigo\" PRIMARY KEY (\"id\"); ";

                    string ATUALIZACAO3 = " CREATE GENERATOR \"GEN_backup_id\"; ";

                    string ATUALIZACAO4 = " SET GENERATOR \"GEN_backup_id\" TO 0; ";

                    string ATUALIZACAO5 = " CREATE TRIGGER \"BI_backup_id\" FOR \"backup\" "
                                        + " ACTIVE BEFORE INSERT "
                                        + " POSITION 0 "
                                        + " AS "
                                        + " BEGIN "
                                        + " IF (NEW.\"id\" IS NULL) THEN "
                                        + " NEW.\"id\" = GEN_ID(\"GEN_backup_id\", 1); "
                                        + " END; ";

                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        conn.Open();
                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO1, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO2, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO3, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO4, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO5, true, parms);
                                trans.Commit();
                                Console.WriteLine("Tabela de backup criada com sucesso.");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine("Ocorreu um erro ao converter a base. Verifique: ");
                                Console.WriteLine(ex.Message);
                                return;
                            }
                        }
                    }
                }
                #endregion

                #endregion

                #region 1.02.013 para 1.03.014

                try
                {
                    string sql = " SELECT * FROM \"justificativa\" ";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Criando tabela de justificativas.");

                    string ATUALIZACAO1 = " CREATE TABLE \"justificativa\" ( "
                               + "\"id\" INTEGER NOT NULL, "
                               + "\"codigo\" INTEGER NOT NULL, "
                               + "\"descricao\" VARCHAR(80) CHARACTER SET NONE COLLATE NONE, "
                               + "\"incdata\" TIMESTAMP, "
                               + " \"inchora\" TIMESTAMP, "
                               + " \"incusuario\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE, "
                               + " \"altdata\" TIMESTAMP, "
                               + " \"althora\" TIMESTAMP, "
                               + " \"altusuario\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE); ";

                    string ATUALIZACAO2 = " ALTER TABLE \"justificativa\" ADD CONSTRAINT \"justificativa_porcodigo\" PRIMARY KEY (\"id\"); ";

                    string ATUALIZACAO3 = " CREATE GENERATOR \"GEN_justificativa_id\"; ";

                    string ATUALIZACAO4 = " SET GENERATOR \"GEN_justificativa_id\" TO 0; ";

                    string ATUALIZACAO5 = " CREATE TRIGGER \"BI_justificativa_id\" FOR \"justificativa\" "
                                        + " ACTIVE BEFORE INSERT "
                                        + " POSITION 0 "
                                        + " AS "
                                        + " BEGIN "
                                        + " IF (NEW.\"id\" IS NULL) THEN "
                                        + " NEW.\"id\" = GEN_ID(\"GEN_justificativa_id\", 1); "
                                        + " END; ";

                    string ATUALIZACAO6 = "Alter table \"tratamentomarcacao\"" +
                                                      " Add \"idjustificativa\" Integer;";

                    string ATUALIZACAO7 = "ALTER TABLE \"tratamentomarcacao\" ADD CONSTRAINT \"fk_tratamento_justificativa\" FOREIGN KEY (\"idjustificativa\") REFERENCES \"justificativa\"(\"id\");";

                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        conn.Open();
                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO1, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO2, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO3, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO4, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO5, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO6, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO7, true, parms);
                                trans.Commit();
                                Console.WriteLine("Tabela de justificativas criada com sucesso.");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine("Ocorreu um erro ao converter a base. Verifique: ");
                                Console.WriteLine(ex.Message);
                                return;
                            }
                        }
                    }
                }

                #endregion

                #region Campo Número de série na tabela empresa
                try
                {
                    string sql = " SELECT \"numeroserie\" FROM \"empresa\" ";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Inserindo campo numero de serie na tabela empresa...");

                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        conn.Open();
                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                string ATUALIZACAO1 = "ALTER TABLE \"empresa\" ADD \"numeroserie\" VARCHAR(20); ";

                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO1, true, parms);
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine("Ocorreu um erro ao converter a base. Verifique: ");
                                Console.WriteLine(ex.Message);
                                return;
                            }
                        }


                        Console.WriteLine("Campo criado com sucesso...");
                    }
                }
                #endregion

                #region Inserção de bdalterado na tabela empresa

                try
                {
                    string sql = " SELECT \"bdalterado\" FROM \"empresa\" ";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Criando campos na tabela de empresa...");
                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        conn.Open();
                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                string ATUALIZACAO1 = "ALTER TABLE \"empresa\" ADD \"bdalterado\" Integer; ";

                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO1, true, parms);
                                trans.Commit();
                                Console.WriteLine("Campos criados com sucesso...");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine("Ocorreu um erro ao converter a base. Verifique: ");
                                Console.WriteLine(ex.Message);
                                return;
                            }
                        }

                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                DAL.FB.Empresa dalEmpresa = DAL.FB.Empresa.GetInstancia;
                                List<Modelo.Empresa> empresas = dalEmpresa.GetAllList();

                                foreach (Modelo.Empresa emp in empresas)
                                {
                                    emp.Chave = MD5HashGenerator.GenerateKey(emp.ToMD5());
                                    dalEmpresa.Alterar(trans, emp);
                                }
                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine("Erro ao converter base:");
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }

                #endregion

                DAL.FB.Funcionario dalFuncionario = DAL.FB.Funcionario.GetInstancia;
                List<Modelo.Funcionario> funcionarios = dalFuncionario.GetAllList(true);

                #region Inserção de chave na marcação
                try
                {
                    string sql = " SELECT \"chave\" FROM \"marcacao\" ";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Criando campos na tabela de marcacao...");
                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        conn.Open();
                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                string ATUALIZACAO1 = "ALTER TABLE \"marcacao\" ADD \"chave\" Varchar(255); ";

                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO1, true, parms);
                                trans.Commit();
                                Console.WriteLine("Campos criados com sucesso.");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine("Ocorreu um erro ao converter a base. Verifique: ");
                                Console.WriteLine(ex.Message);
                                return;
                            }
                        }

                        Console.WriteLine("Atualizando marcacoes...");
                        Console.WriteLine("Aguarde, caso existam muitas marcações este processo pode demorar...");
                        Console.WriteLine();
                        DAL.FB.Marcacao dalMarcacao = DAL.FB.Marcacao.GetInstancia;
                        int qtd = funcionarios.Count;
                        decimal perido = Math.Round((decimal)qtd / 100);
                        FbBatchExecution bath = new FbBatchExecution(conn);
                        StringBuilder str = new StringBuilder();
                        List<Modelo.Marcacao> marcacoes = null;
                        count = 0;
                        foreach (Modelo.Funcionario func in funcionarios)
                        {
                            marcacoes = GetMarcPorFuncionario(func.Id);
                            foreach (Modelo.Marcacao marc in marcacoes)
                            {
                                marc.Chave = marc.ToMD5();
                                str.Remove(0, str.Length);
                                str.Append("UPDATE \"marcacao\" SET \"chave\" = '");
                                str.Append(marc.Chave);
                                str.Append("' WHERE \"marcacao\".\"id\" = ");
                                str.Append(marc.Id);
                                bath.SqlStatements.Add(str.ToString());
                            }
                            progresso();
                        }
                        Console.WriteLine();
                        Console.WriteLine("Salvando Marcações...");
                        if (bath.SqlStatements.Count > 0)
                        {
                            bath.Execute();
                        }
                        Console.WriteLine();
                    }
                }
                #endregion

                #region Inserção de chave na bilhetes imp
                try
                {
                    string sql = " SELECT \"chave\" FROM \"bilhetesimp\" ";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Criando campos na tabela de bilhetes...");
                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        conn.Open();
                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                string ATUALIZACAO1 = "ALTER TABLE \"bilhetesimp\" ADD \"chave\" Varchar(255); ";

                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO1, true, parms);
                                trans.Commit();
                                Console.WriteLine("Campos criados com sucesso.");
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine("Ocorreu um erro ao converter a base. Verifique: ");
                                Console.WriteLine(ex.Message);
                                return;
                            }
                        }

                        Console.WriteLine("Atualizando bilhetes...");
                        Console.WriteLine("Aguarde, caso existam muitos bilhetes este processo pode demorar...");
                        Console.WriteLine();

                        //Colocado para não dar erro caso esteja convertendo uma versão antiga
                        string comando3 = "ALTER TABLE \"bilhetesimp\" ADD \"dscodigo\" VARCHAR(50)";
                        DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando3, true, parms);

                        List<Modelo.BilhetesImp> bilhetes = LoadBilhetesPorFuncionario(null);
                        int qtd = bilhetes.Count;
                        decimal perido = Math.Round((decimal)qtd / 100);
                        FbBatchExecution bath = new FbBatchExecution(conn);
                        StringBuilder str = new StringBuilder();
                        count = 0;
                        foreach (Modelo.BilhetesImp bil in bilhetes)
                        {
                            bil.Chave = bil.ToMD5();
                            str.Remove(0, str.Length);
                            str.Append("UPDATE \"bilhetesimp\" SET \"chave\" = '");
                            str.Append(bil.Chave);
                            str.Append("' WHERE \"bilhetesimp\".\"id\" = ");
                            str.Append(bil.Id);
                            bath.SqlStatements.Add(str.ToString());
                            progresso();
                        }
                        Console.WriteLine();
                        Console.WriteLine("Salvando Bilhetes...");
                        if (bath.SqlStatements.Count > 0)
                        {
                            bath.Execute();
                        }
                        Console.WriteLine();
                    }
                }

                #endregion

                CriacaoIndicesTabelaBilhetesImp();

                CorrecaoHorarioFlexivelCargaHorariaMista();

                AtualizacaoVersao104016(funcionarios);

                #region Versão 1.05.018

                try
                {
                    string sql = " SELECT \"he50n\" FROM \"eventos\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    Console.WriteLine("Iniciando correção na tabela de eventos para exportação...");
                    try
                    {
                        string ATUALIZACAO = "Alter table \"eventos\"" +
                                              " Add \"he50n\" Integer," +
                                              " Add \"he60n\" Integer," +
                                              " Add \"he70n\" Integer," +
                                              " Add \"he80n\" Integer," +
                                              " Add \"he90n\" Integer," +
                                              " Add \"he100n\" Integer;";

                        DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, ATUALIZACAO, true, parms);
                        Console.WriteLine("Correção na tabela de eventos realizada com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao converter base:");
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }

                try
                {
                    string sql = " SELECT \"hesabn\" FROM \"eventos\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    try
                    {
                        string ATUALIZACAO = "Alter table \"eventos\"" +
                                              " Add \"hesabn\" Integer," +
                                              " Add \"hedomn\" Integer," +
                                              " Add \"hefern\" Integer," +
                                              " Add \"folgan\" Integer;";

                        DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, ATUALIZACAO, true, parms);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao converter base:");
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }

                #endregion

                AtualizacaoVersao105019();

                AtualizacaoVersao106020();

                AtualizacaoVersao106021();

                AtualizacaoVersao106022();

                bool rec = AtualizacaoVersao107024();

                bool atualizarBil = AtualizacaoVersao108026(funcionarios);

                AtualizacaoVersao109027(rec, atualizarBil, funcionarios);

                #region Exclui tabela de tratamentos
                //string excluir1 = "DROP TABLE \"tratamentomarcacao\"";
                //DAL.FB.DataBase.ExecuteNonQueryCmd(CommandType.Text, excluir1, new FbParameter[]{});
                #endregion

                Console.WriteLine("Fim da conversão. Tecle para sair...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //versões anteriores para 1.01.010
        private static void Versao101010()
        {
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                FbParameter[] parms = new FbParameter[0];

                Console.WriteLine("Iniciando correção na tabela de horários...");
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string ATUALIZACAO = "UPDATE \"horariodetalhe\" SET \"flagfolga\" = 1 WHERE \"bcarregar\" = 0 AND \"data\" IS NULL";

                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO, true, parms);
                        trans.Commit();
                        Console.WriteLine("Correção na tabela de horários realizada com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        Console.WriteLine("Erro ao converter base:");
                        Console.WriteLine(ex.Message);
                    }
                }

                try
                {
                    string sql = " SELECT \"cargamista\", \"totalmista\" FROM \"jornadaalternativa\" ";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    using (FbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Console.WriteLine("Iniciando correção na tabela de jornada alternativa...");
                            string ATUALIZACAO1 = "Alter table \"jornadaalternativa\" Add \"cargamista\" Integer;";
                            string ATUALIZACAO2 = "Alter table \"jornadaalternativa\" Add \"totalmista\" Varchar(5);";

                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO1, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO2, true, parms);
                            trans.Commit();
                            Console.WriteLine("Correção na tabela de jornada alternativa realizada com sucesso.");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            Console.WriteLine("Erro ao converter base:");
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

        //1.00.006 para 1.01.007
        private static void Versao101007()
        {
            #region Correção na tabela de empresa
            try
            {
                FbParameter[] parms = new FbParameter[0];

                string sql = " SELECT \"bprincipal\", \"tipolicenca\", \"quantidade\" FROM \"empresa\" ";
                FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            }
            catch (FbException)
            {
                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    Console.WriteLine("Iniciando correção na tabela de empresa...");
                    conn.Open();
                    FbParameter[] parms = new FbParameter[0];

                    using (FbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string ATUALIZACAO1 = "Alter table \"empresa\" Add \"bprincipal\" Integer;";
                            string ATUALIZACAO2 = "Alter table \"empresa\" Add \"tipolicenca\" Integer;";
                            string ATUALIZACAO3 = "Alter table \"empresa\" Add \"quantidade\" Integer;";

                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO1, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO2, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO3, true, parms);
                            trans.Commit();

                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            Console.WriteLine("Erro ao converter base:");
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (FbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string ATUALIZACAO4 = "update \"empresa\" set \"bprincipal\" = 1, \"tipolicenca\" = 1, \"quantidade\" = 0 where \"codigo\" = 1;";
                            string ATUALIZACAO5 = "update \"empresa\" set \"bprincipal\" = 0, \"tipolicenca\" = 1, \"quantidade\" = 0 where \"codigo\" > 1;";

                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO4, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, ATUALIZACAO5, true, parms);

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            Console.WriteLine("Erro ao converter base:");
                            Console.WriteLine(ex.Message);
                        }

                    }

                    Console.WriteLine("Conversao efetuada com sucesso...");

                }
            }
            #endregion

            #region Criação dos indices
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                FbParameter[] parms = new FbParameter[0];
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    Console.WriteLine("Iniciando criação dos indices...");

                    string INDICE1 = "CREATE INDEX \"IDX_funcionario_empresa\" ON \"funcionario\"(\"idempresa\");";

                    string INDICE2 = "CREATE INDEX \"IDX_funcionario_departamento\" ON \"funcionario\"(\"iddepartamento\");";

                    string INDICE3 = "CREATE INDEX \"IDX_funcionario_horario\" ON \"funcionario\"(\"idhorario\");";

                    string INDICE4 = "CREATE INDEX \"IDX_funcionario_idfuncao\" ON \"funcionario\"(\"idfuncao\");";

                    string INDICE5 = "CREATE INDEX \"IDX_horario_idparametro\" ON \"horario\"(\"idparametro\");";

                    string INDICE6 = "CREATE INDEX \"IDX_horariodetalhe_horario\" ON \"horariodetalhe\"(\"idhorario\");";

                    string INDICE7 = "CREATE INDEX \"IDX_horariodetalhe_data\" ON \"horariodetalhe\"(\"data\");";

                    string INDICE8 = "CREATE INDEX \"IDX_bancohoras_datainicial\" ON \"bancohoras\"(\"datainicial\");";

                    string INDICE9 = "CREATE INDEX \"IDX_marcacao_idfuncionario\" ON \"marcacao\"(\"idfuncionario\");";

                    string INDICE10 = "CREATE INDEX \"IDX_marcacao_idhorario\" ON \"marcacao\"(\"idhorario\");";

                    try
                    {
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE1, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE2, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE3, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE4, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE5, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE6, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE7, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE8, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE9, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE10, true, parms);
                        trans.Commit();

                        Console.WriteLine("Indices criados com sucesso.");
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        Console.WriteLine("Indices já existem na base de dados.");
                    }
                }
            }
            #endregion
        }

        private static void CriacaoIndicesTabelaBilhetesImp()
        {
            #region Criação de Indices

            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                FbParameter[] parms = new FbParameter[0];

                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Criando indices na tabela de bilhetesimp...");
                        string INDICE1 = "CREATE INDEX \"IDX_bilhetesimp_data\" ON \"bilhetesimp\"(\"data\");";
                        string INDICE2 = "CREATE INDEX \"IDX_bilhetesimp_hora\" ON \"bilhetesimp\"(\"hora\");";
                        string INDICE3 = "CREATE INDEX \"IDX_bilhetesimp_func\" ON \"bilhetesimp\"(\"func\");";
                        string INDICE4 = "CREATE INDEX \"IDX_bilhetesimp_relogio\" ON \"bilhetesimp\"(\"relogio\");";
                        string INDICE5 = "CREATE INDEX \"IDX_provisorio_dsfuncionario\" ON \"provisorio\"(\"dsfuncionario\");";
                        string INDICE6 = "CREATE INDEX \"IDX_provisorio_dt_inicial\" ON \"provisorio\"(\"dt_inicial\");";
                        string INDICE7 = "CREATE INDEX \"IDX_provisorio_dt_final\" ON \"provisorio\"(\"dt_final\");";


                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE1, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE2, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE3, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE4, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE5, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE6, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE7, true, parms);

                        trans.Commit();

                        Console.WriteLine("Indices criados com sucesso.");
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        Console.WriteLine("Indices já existem na base de dados.");
                    }
                }
            }
            #endregion

            #region Alteração de Chave Estrangeira

            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                FbParameter[] parms = new FbParameter[0];
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Criando fk na tabela de marcacao...");
                        string FK = "ALTER TABLE \"marcacao\" ADD CONSTRAINT \"fk_marcacao_funcionario_new\" FOREIGN KEY (\"idfuncionario\") REFERENCES \"funcionario\"(\"id\");";


                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, FK, true, parms);

                        trans.Commit();

                        Console.WriteLine("fk criada com sucesso.");
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        Console.WriteLine("fk já existe na base de dados.");
                    }
                }
            }
            #endregion
        }

        //Correção na tabela de horário flexível para cargamista
        private static void CorrecaoHorarioFlexivelCargaHorariaMista()
        {
            try
            {
                FbParameter[] parms = new FbParameter[0];

                string sql = " SELECT \"marcacargahorariamista\" FROM \"horariodetalhe\" ";
                FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            }
            catch (FbException)
            {
                Console.WriteLine("Criando campo na tabela de horarioDetalhe...");
                FbParameter[] parms = new FbParameter[0];
                DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, "ALTER TABLE \"horariodetalhe\" ADD \"marcacargahorariamista\" Integer;", true, parms);
                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    Console.WriteLine("Aguarde, caso existam muitos horários este processo pode demorar...");
                    FbBatchExecution batch = new FbBatchExecution(conn);
                    DAL.FB.HorarioDetalhe dalHorarioDetalhe = DAL.FB.HorarioDetalhe.GetInstancia;
                    DataTable horariosDetalhe = dalHorarioDetalhe.GetAll();
                    StringBuilder str = new StringBuilder();
                    int qtd = horariosDetalhe.Rows.Count;
                    decimal perido = Math.Round((decimal)qtd / 100);
                    count = 0;
                    foreach (DataRow hd in horariosDetalhe.Rows)
                    {
                        str.Remove(0, str.Length);
                        str.Append("UPDATE \"horariodetalhe\" SET \"marcacargahorariamista\" = ");
                        if (hd["cargahorariamista"] is DBNull)
                        {
                            str.Append("0");
                        }
                        else if (hd["cargahorariamista"] != String.Empty && hd["cargahorariamista"] != "--:--")
                        {
                            str.Append("1");
                        }
                        else
                        {
                            str.Append("0");
                        }
                        str.Append(" WHERE \"horariodetalhe\".\"id\" = '");
                        str.Append(hd["id"].ToString() + "'");
                        batch.SqlStatements.Add(str.ToString());

                        progresso();
                    }
                    Console.WriteLine();
                    Console.WriteLine("Salvando Horários...");
                    if (batch.SqlStatements.Count > 0)
                    {
                        batch.Execute();
                    }
                    Console.WriteLine();
                }
            }
        }

        private static void AtualizacaoVersao104016(List<Modelo.Funcionario> funcionarios)
        {
            try
            {
                Console.WriteLine("Criando indice na tabela empresa...");
                FbParameter[] parms = new FbParameter[0];

                string indice = "CREATE INDEX \"IDX_empresa_nome\" ON \"empresa\"(\"nome\");";

                DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, indice, true, parms);

                Console.WriteLine("Indice criado com sucesso.");

                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    conn.Open();
                    using (FbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            DAL.FB.Empresa dalEmpresa = DAL.FB.Empresa.GetInstancia;
                            List<Modelo.Empresa> empresas = dalEmpresa.GetAllList();

                            foreach (Modelo.Empresa emp in empresas)
                            {
                                emp.BDAlterado = false;
                                emp.Chave = MD5HashGenerator.GenerateKey(emp.ToMD5());
                                dalEmpresa.Alterar(trans, emp);
                            }
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            Console.WriteLine("Erro ao converter base:");
                            Console.WriteLine(ex.Message);
                        }
                    }

                    Console.WriteLine("Corrigindo tabela de marcacoes...");
                    Console.WriteLine("Aguarde, caso existam muitas marcações este processo pode demorar...");
                    Console.WriteLine();
                    DAL.FB.Marcacao dalMarcacao = DAL.FB.Marcacao.GetInstancia;
                    int qtd = funcionarios.Count;
                    decimal perido = Math.Round((decimal)qtd / 100);
                    FbBatchExecution bath = new FbBatchExecution(conn);
                    StringBuilder str = new StringBuilder();
                    List<Modelo.Marcacao> marcacoes = null;
                    count = 0;
                    foreach (Modelo.Funcionario func in funcionarios)
                    {
                        marcacoes = GetMarcPorFuncionario(func.Id);
                        foreach (Modelo.Marcacao marc in marcacoes)
                        {
                            marc.Chave = marc.ToMD5();
                            str.Remove(0, str.Length);
                            str.Append("UPDATE \"marcacao\" SET \"chave\" = '");
                            str.Append(marc.Chave);
                            str.Append("' WHERE \"marcacao\".\"id\" = ");
                            str.Append(marc.Id);
                            bath.SqlStatements.Add(str.ToString());
                        }
                        progresso();
                    }
                    Console.WriteLine();
                    Console.WriteLine("Salvando Marcações...");
                    if (bath.SqlStatements.Count > 0)
                    {
                        bath.Execute();
                    }
                    Console.WriteLine();

                    try
                    {
                        string comando3 = "ALTER TABLE \"bilhetesimp\" ADD \"dscodigo\" VARCHAR(50)";
                        DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando3, true, parms);
                    }
                    catch (Exception) { }

                    Console.WriteLine("Corrigindo tabela de bilhetes...");
                    Console.WriteLine("Aguarde, caso existam muitos bilhetes este processo pode demorar...");
                    Console.WriteLine();

                    List<Modelo.BilhetesImp> bilhetes = LoadBilhetesPorFuncionario(null);
                    qtd = bilhetes.Count;
                    perido = Math.Round((decimal)qtd / 100);
                    bath = new FbBatchExecution(conn);
                    str = new StringBuilder();
                    count = 0;
                    foreach (Modelo.BilhetesImp bil in bilhetes)
                    {
                        bil.Chave = bil.ToMD5();
                        str.Remove(0, str.Length);
                        str.Append("UPDATE \"bilhetesimp\" SET \"chave\" = '");
                        str.Append(bil.Chave);
                        str.Append("' WHERE \"bilhetesimp\".\"id\" = ");
                        str.Append(bil.Id);
                        bath.SqlStatements.Add(str.ToString());
                        progresso();
                    }
                    Console.WriteLine();
                    Console.WriteLine("Salvando Bilhetes...");
                    if (bath.SqlStatements.Count > 0)
                    {
                        bath.Execute();
                    }
                    Console.WriteLine();
                }
            }
            catch (FbException)
            {
                Console.WriteLine("Indice já existe na base de dados.");
            }

        }

        private static void AtualizacaoVersao105019()
        {
            #region Criação de indices
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                FbParameter[] parms = new FbParameter[0];
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    Console.WriteLine("Iniciando criação dos indices...");

                    string INDICE1 = "CREATE INDEX \"IDX_horariophextra_codigo\" ON \"horariophextra\"(\"codigo\");";

                    string INDICE2 = "CREATE INDEX \"IDX_horariophextra_horario\" ON \"horariophextra\"(\"idhorario\");";

                    string INDICE3 = "CREATE INDEX \"IDX_horariodetalhe_dia\" ON \"horariodetalhe\"(\"dia\");";

                    string INDICE4 = "CREATE INDEX \"IDX_horariodetalhe_codigo\" ON \"horariodetalhe\"(\"codigo\");";

                    string INDICE5 = "CREATE INDEX \"IDX_ja_datainicial\" ON \"jornadaalternativa\"(\"datainicial\");";

                    string INDICE6 = "CREATE INDEX \"IDX_ja_datafinal\" ON \"jornadaalternativa\"(\"datafinal\");";

                    string INDICE7 = "CREATE INDEX \"IDX_ja_tipo\" ON \"jornadaalternativa\"(\"tipo\");";

                    string INDICE8 = "CREATE INDEX \"IDX_bancohoras_datafinal\" ON \"bancohoras\"(\"datafinal\");";

                    string INDICE9 = "CREATE INDEX \"IDX_trat_indicador\" ON \"tratamentomarcacao\"(\"indicador\");";

                    string INDICE10 = "CREATE INDEX \"IDX_trat_idmarcacao\" ON \"tratamentomarcacao\"(\"idmarcacao\");";

                    string INDICE11 = "CREATE INDEX \"IDX_trat_sequencia\" ON \"tratamentomarcacao\"(\"sequencia\");";

                    string INDICE12 = "CREATE INDEX \"IDX_bilhetesimp_mar_data\" ON \"bilhetesimp\"(\"mar_data\");";

                    string INDICE13 = "CREATE INDEX \"IDX_bilhetesimp_mar_hora\" ON \"bilhetesimp\"(\"mar_hora\");";

                    string INDICE14 = "CREATE INDEX \"IDX_bilhetesimp_mar_relogio\" ON \"bilhetesimp\"(\"mar_relogio\");";

                    string INDICE15 = "CREATE INDEX \"IDX_feriado_data\" ON \"feriado\"(\"data\");";

                    string INDICE16 = "CREATE INDEX \"IDX_feriado_tipoferiado\" ON \"feriado\"(\"tipoferiado\");";

                    string INDICE17 = "CREATE INDEX \"IDX_dja_datacompensada\" ON \"diasjornadaalternativa\"(\"datacompensada\");";

                    try
                    {
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE1, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE2, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE3, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE4, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE5, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE6, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE7, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE8, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE9, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE10, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE11, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE12, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE13, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE14, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE15, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE16, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, INDICE17, true, parms);
                        trans.Commit();

                        Console.WriteLine("Indices criados com sucesso.");
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        Console.WriteLine("Indices já existem na base de dados.");
                    }
                }
            }

            #endregion

            #region Criação da View

            try
            {
                FbParameter[] parms = new FbParameter[0];
                string sql = " SELECT * FROM \"jornadaalternativa_view\" ";
                FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            }
            catch (Exception)
            {
                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    conn.Open();
                    FbParameter[] parms = new FbParameter[0];
                    using (FbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            Console.WriteLine("Criando view Jornada Alternativa...");

                            string cmd = "CREATE VIEW \"jornadaalternativa_view\" " +
                                        " (\"id\", \"tipo\", \"identificacao\", \"datainicial\", \"datafinal\", \"datacompensada\") " +
                                        " AS " +
                                        " SELECT J.\"id\", J.\"tipo\", J.\"identificacao\", J.\"datainicial\", J.\"datafinal\", NULL AS \"datacompensada\" " +
                                        " FROM \"jornadaalternativa\" AS J " +
                                        " UNION ALL " +
                                        " SELECT J.\"id\", J.\"tipo\", J.\"identificacao\", J.\"datainicial\", J.\"datafinal\", JD.\"datacompensada\" " +
                                        " FROM \"jornadaalternativa\" AS J " +
                                        " INNER JOIN \"diasjornadaalternativa\" AS JD ON JD.\"idjornadaalternativa\" = J.\"id\"";

                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd, true, parms);
                            trans.Commit();
                            Console.WriteLine("View criada com sucesso.");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            Console.WriteLine("Erro ao criar view: " + ex.Message);
                        }
                    }
                }

            }

            #endregion

            #region Criação de Procedimentos
            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                FbParameter[] parms = new FbParameter[0];
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("Criando funções auxiliares...");

                        string cmd1 = @"CREATE PROCEDURE CONVERTHORAMINUTO(
                                            HORA VARCHAR(10) CHARACTER SET NONE)
                                            RETURNS(
                                              RET INTEGER)
                                            AS
                                            DECLARE VARIABLE tam INTEGER;
                                            DECLARE VARIABLE hora_aux VARCHAR(10);
                                            BEGIN   
                                               hora_aux = TRIM(hora);
                                               
                                               IF (SUBSTRING(hora_aux FROM 1 FOR 1) = '-' OR SUBSTRING(hora_aux FROM 1 FOR 1) = ' ') THEN
                                               BEGIN
                                                  ret = 0;
                                               END
                                               ELSE BEGIN
                                                  tam = CHAR_LENGTH(hora_aux);
                                                  IF (tam < 5) THEN
                                                  BEGIN
      	                                            ret = 0;
                                                  END ELSE BEGIN
                                                    IF (tam = 5) THEN
                                                    BEGIN
                                                      ret = CAST(SUBSTRING(hora_aux FROM 1 FOR 2) AS INTEGER) * 60 + CAST(SUBSTRING(hora_aux FROM 4 FOR 2) AS INTEGER);
                                                    END ELSE
                                                    IF (tam = 6) THEN
                                                    BEGIN
                                                      ret = CAST(SUBSTRING(hora_aux FROM 1 FOR 3) AS INTEGER) * 60 + CAST(SUBSTRING(hora_aux FROM 5 FOR 2) AS INTEGER);
                                                    END ELSE
                                                    IF (tam = 7) THEN
                                                    BEGIN
                                                      ret = CAST(SUBSTRING(hora_aux FROM 1 FOR 4) AS INTEGER) * 60 + CAST(SUBSTRING(hora_aux FROM 6 FOR 2) AS INTEGER);
                                                    END
                                                    ELSE BEGIN
                                                      ret = 0;
                                                    END
                                                  END        
                                               END
                                               SUSPEND;            
                                            END";

                        string cmd2 = @" CREATE PROCEDURE CONVERTBATIDAMINUTO (phora VARCHAR(10))
                                            RETURNS (ret INTEGER)
                                            AS
                                            DECLARE VARIABLE tam INTEGER;
                                            DECLARE VARIABLE hora VARCHAR(10);
                                            BEGIN  
                                               hora = TRIM(phora); 
                                               IF (SUBSTRING(hora FROM 1 FOR 1) = '-') THEN
                                               BEGIN
                                                  ret = -1;
                                               END
                                               ELSE BEGIN
                                                  tam = CHAR_LENGTH(hora);
                                                  IF (tam < 5) THEN
                                                  BEGIN
                                                    ret = -1;
                                                  END ELSE BEGIN
                                                    IF (tam = 5) THEN
                                                    BEGIN
                                                      ret = CAST(SUBSTRING(hora FROM 1 FOR 2) AS INTEGER) * 60 + CAST(SUBSTRING(hora FROM 4 FOR 2) AS INTEGER);
                                                    END ELSE
                                                    IF (tam = 6) THEN
                                                    BEGIN
                                                      ret = CAST(SUBSTRING(hora FROM 1 FOR 3) AS INTEGER) * 60 + CAST(SUBSTRING(hora FROM 5 FOR 2) AS INTEGER);
                                                    END ELSE
                                                    IF (tam = 7) THEN
                                                    BEGIN
                                                      ret = CAST(SUBSTRING(hora FROM 1 FOR 4) AS INTEGER) * 60 + CAST(SUBSTRING(hora FROM 6 FOR 2) AS INTEGER);
                                                    END
                                                    ELSE BEGIN
                                                      ret = -1;
                                                    END
                                                  END        
                                               END
                                               SUSPEND;            
                                            END";

                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd1, true, parms);
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd2, true, parms);
                        trans.Commit();
                        Console.WriteLine("Funções criadas com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        Console.WriteLine("Erro ao criar função: " + ex.Message);
                    }
                }
            }

            #endregion
        }

        private static void AtualizacaoVersao106020()
        {
            string comando1 = "ALTER TABLE \"jornadaalternativa\" ADD \"conversaohoranoturna\" SMALLINT";
            string comando2 = "ALTER TABLE \"jornadaalternativa\" ADD \"calculoadnoturno\" SMALLINT";
            string comando4 = "UPDATE \"bilhetesimp\" set \"dscodigo\" = \"func\"";

            try
            {
                FbParameter[] parms = new FbParameter[0];
                string sql = " SELECT \"conversaohoranoturna\" FROM \"jornadaalternativa\" ";
                FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            }
            catch (Exception)
            {
                FbParameter[] parms = new FbParameter[0];
                try
                {
                    string comando3 = "ALTER TABLE \"bilhetesimp\" ADD \"dscodigo\" VARCHAR(50)";
                    DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando3, true, parms);
                }
                catch (Exception ex)
                {
                }

                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    conn.Open();
                    using (FbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, comando1, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, comando2, true, parms);

                            trans.Commit();
                            Console.WriteLine("Atualização da tabela jornada alternativa.");
                            Console.WriteLine("Atualização da tabela bilhetes imp.");
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            Console.WriteLine("Erro ao criar os campos (Jornada alternativa e Bilhetes Imp): " + ex.Message);
                        }
                    }

                    //Essa atualização deve ser feita depois de inserido o campo, por isso está numa nova transação
                    using (FbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, comando4, true, parms);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            Console.WriteLine("Erro ao atualizar o campo de Bilhetes Imp: " + ex.Message);
                        }
                    }
                }
            }
        }

        private static void AtualizacaoVersao106021()
        {
            string comando = "UPDATE \"bilhetesimp\" set \"mar_data\" = \"data\", \"mar_hora\" = \"hora\", \"mar_relogio\" = \"relogio\"  WHERE \"mar_data\" IS NULL";

            using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
            {
                conn.Open();
                FbParameter[] parms = new FbParameter[0];
                using (FbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, comando, true, parms);

                        trans.Commit();
                        Console.WriteLine("Atualização da tabela bilhetes imp [Mar_Data].");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        Console.WriteLine("Erro ao atualizar Bilhetes Imp [Mar_Data]): " + ex.Message);
                    }
                }
            }
            //Corrigi cadastro de provisório
            Console.WriteLine("Corrigindo tabela de código provisório.");
            BLL.Provisorio bllProvisorio = BLL.Provisorio.GetInstancia;
            List<Modelo.Provisorio> provisorios = bllProvisorio.GetAllList();
            Modelo.Provisorio objProvisorio;
            string aux = "";
            foreach (Modelo.Provisorio prov in provisorios)
            {
                objProvisorio = prov;
                aux = objProvisorio.Dsfuncionario;
                objProvisorio.Dsfuncionario = objProvisorio.Dsfuncionarionovo;
                objProvisorio.Dsfuncionarionovo = aux;
                bllProvisorio.Salvar(Modelo.Acao.Alterar, objProvisorio);
            }
        }

        private static void AtualizacaoVersao106022()
        {
            try
            {
                FbParameter[] parms = new FbParameter[0];
                string sql = " SELECT * FROM \"cwkvsnsys\" ";
                FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            }
            catch (Exception)
            {
                Console.WriteLine("Corrigindo campo código das tabelas de marcacao, funcionario, provisorio e mudanca de codigo.");
                FbParameter[] parms = new FbParameter[0];
                string comando1 = "UPDATE \"marcacao\" SET \"dscodigo\" = CAST(CAST(\"marcacao\".\"dscodigo\" AS BIGINT) AS VARCHAR(16)) WHERE \"marcacao\".\"dscodigo\" <> ''";
                string comando2 = "UPDATE \"funcionario\" SET \"dscodigo\" = CAST(CAST(\"funcionario\".\"dscodigo\" AS BIGINT) AS VARCHAR(16))";
                string comando3 = "UPDATE \"provisorio\" SET \"dsfuncionario\" = CAST(CAST(\"provisorio\".\"dsfuncionario\" AS BIGINT) AS VARCHAR(16))";
                string comando4 = "UPDATE \"mudcodigofunc\" SET \"dscodigoantigo\" = CAST(CAST(\"mudcodigofunc\".\"dscodigoantigo\" AS BIGINT) AS VARCHAR(16))";
                DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando1, true, parms);
                DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando2, true, parms);
                DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando3, true, parms);
                DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando4, true, parms);
            }
        }

        #region Versão 107024
        private static bool AtualizacaoVersao107024()
        {
            try
            {
                FbParameter[] parms = new FbParameter[0];
                string sql = " SELECT * FROM \"cwkvsnsys\" ";
                FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            }
            catch (Exception)
            {
                Console.WriteLine("Atualizando para a versão 1.07.024, aguarde...");
                FbParameter[] parms = new FbParameter[0];

                string cmd1 = "CREATE TABLE \"cwkvsnsys\" ( "
                               + "\"id\" INTEGER NOT NULL, "
                               + "\"cwk\" BLOB NOT NULL)";
                string cmd2 = " ALTER TABLE \"cwkvsnsys\" ADD CONSTRAINT \"cwkvsnsys_porcodigo\" PRIMARY KEY (\"id\"); ";

                string cmd3 = " CREATE GENERATOR \"GEN_cwkvsnsys_id\"; ";

                string cmd4 = " SET GENERATOR \"GEN_cwkvsnsys_id\" TO 0; ";

                string cmd5 = " CREATE TRIGGER \"BI_cwkvsnsys_id\" FOR \"cwkvsnsys\" "
                                    + " ACTIVE BEFORE INSERT "
                                    + " POSITION 0 "
                                    + " AS "
                                    + " BEGIN "
                                    + " IF (NEW.\"id\" IS NULL) THEN "
                                    + " NEW.\"id\" = GEN_ID(\"GEN_cwkvsnsys_id\", 1); "
                                    + " END; ";
                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    conn.Open();
                    using (FbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd1, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd2, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd3, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd4, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd5, true, parms);
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            Console.WriteLine("Erro ao converter base: ");
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                BLL.Empresa bllEmpresa = BLL.Empresa.GetInstancia;
                FbParameter[] parms2 = new FbParameter[]
                {
                    new FbParameter("@cwk", FbDbType.Binary)
                };
                Modelo.Empresa objEmpresa = bllEmpresa.GetEmpresaPrincipal();
                parms2[0].Value = bllEmpresa.GeraVersao("1.07.024" + objEmpresa.Numeroserie);
                string cmd6 = "INSERT INTO \"cwkvsnsys\" (\"cwk\") VALUES (@cwk)";
                DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, cmd6, true, parms2);
                return true;
            }
            return false;
        }
        private static void IncrementaProgressBar(int incremento)
        {
            if (count > 3)
                count = 0;
            progresso();
        }
        private static void SetaMinMaxProgressBar(int min, int max)
        {
        }
        private static void SetaValorProgressBar(int valor)
        {
        }
        private static void SetaMensagem(string msg)
        {
            //Console.WriteLine(msg);
        }
        #endregion

        private static bool AtualizacaoVersao108026(List<Modelo.Funcionario> funcionarios)
        {
            FbParameter[] parms = new FbParameter[0];
            try
            {
                string sql = " SELECT \"ocorrencia\" FROM \"bilhetesimp\" ";
                FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            }
            catch (Exception)
            {
                try
                {
                    List<string> comandos = new List<string>();
                    StringBuilder str = new StringBuilder();
                    DAL.FB.Marcacao dalMarcacao = DAL.FB.Marcacao.GetInstancia;
                    DAL.FB.BilhetesImp dalBilhetes = DAL.FB.BilhetesImp.GetInstancia;
                    DAL.FB.Tratamentomarcacao dalTratamento = DAL.FB.Tratamentomarcacao.GetInstancia;
                    List<Modelo.Marcacao> marcacoes = null;
                    List<Modelo.BilhetesImp> bilhetes = null;
                    Modelo.BilhetesImp bilhete = null;

                    #region Corrigi posição dos bilhetes
                    Console.WriteLine("Corrigindo tabela de bilhetes...");
                    string[] numRel = new string[] { "MA", "PA", "  " };
                    count = 0;
                    foreach (Modelo.Funcionario func in funcionarios)
                    {
                        marcacoes = GetMarcPorFuncionario(func.Id);
                        bilhetes = LoadBilhetesPorFuncionario(func.Dscodigo);
                        foreach (Modelo.Marcacao marc in marcacoes)
                        {
                            if (!numRel.Contains(marc.Ent_num_relogio_1))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Entrada_1, 1, "E", str, comandos);
                            if (!numRel.Contains(marc.Ent_num_relogio_2))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Entrada_2, 2, "E", str, comandos);
                            if (!numRel.Contains(marc.Ent_num_relogio_3))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Entrada_3, 3, "E", str, comandos);
                            if (!numRel.Contains(marc.Ent_num_relogio_4))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Entrada_4, 4, "E", str, comandos);
                            if (!numRel.Contains(marc.Ent_num_relogio_5))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Entrada_5, 5, "E", str, comandos);
                            if (!numRel.Contains(marc.Ent_num_relogio_6))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Entrada_6, 6, "E", str, comandos);
                            if (!numRel.Contains(marc.Ent_num_relogio_7))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Entrada_7, 7, "E", str, comandos);
                            if (!numRel.Contains(marc.Ent_num_relogio_8))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Entrada_8, 8, "E", str, comandos);
                            if (!numRel.Contains(marc.Sai_num_relogio_1))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Saida_1, 1, "S", str, comandos);
                            if (!numRel.Contains(marc.Sai_num_relogio_2))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Saida_2, 2, "S", str, comandos);
                            if (!numRel.Contains(marc.Sai_num_relogio_3))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Saida_3, 3, "S", str, comandos);
                            if (!numRel.Contains(marc.Sai_num_relogio_4))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Saida_4, 4, "S", str, comandos);
                            if (!numRel.Contains(marc.Sai_num_relogio_5))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Saida_5, 5, "S", str, comandos);
                            if (!numRel.Contains(marc.Sai_num_relogio_6))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Saida_6, 6, "S", str, comandos);
                            if (!numRel.Contains(marc.Sai_num_relogio_7))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Saida_7, 7, "S", str, comandos);
                            if (!numRel.Contains(marc.Sai_num_relogio_8))
                                AlteraPosicaoBilhete(bilhetes, bilhete, marc, marc.Saida_8, 8, "S", str, comandos);
                        }
                        progresso();
                    }
                    Console.WriteLine();

                    if (comandos.Count > 0)
                    {
                        Console.WriteLine("Aguarde...");
                        using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                        {
                            FbBatchExecution batch = new FbBatchExecution(conn);
                            int metade = (int)Math.Floor((decimal)comandos.Count / 2);
                            if (metade > 0)
                            {
                                batch.SqlStatements.AddRange(comandos.GetRange(0, metade));
                                batch.Execute();
                                batch.SqlStatements.Clear();
                            }
                            List<string> l = comandos.GetRange(metade, comandos.Count - metade);
                            if (l.Count > 0)
                            {
                                batch.SqlStatements.AddRange(l);
                                batch.Execute();
                            }
                        }
                        comandos.Clear();
                    }
                    #endregion

                    Console.WriteLine("Alterando tabela de bilhetes...");
                    #region Cria novos campos na tabela de bilhetes
                    string comando1 = "ALTER TABLE \"bilhetesimp\" ADD \"ocorrencia\" CHAR(1)";
                    string comando2 = "ALTER TABLE \"bilhetesimp\" ADD \"motivo\" VARCHAR(100)";
                    string comando3 = "ALTER TABLE \"bilhetesimp\" ADD \"idjustificativa\" INTEGER";
                    string comando4 = "ALTER TABLE \"bilhetesimp\" ADD CONSTRAINT \"fk_bilhetesimp_justificativa\" FOREIGN KEY (\"idjustificativa\") REFERENCES \"justificativa\"(\"id\");";
                    DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando1, true, parms);
                    DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando2, true, parms);
                    DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando3, true, parms);
                    DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, comando4, true, parms);
                    #endregion



                    dalBilhetes = null;
                    dalTratamento = null;
                    dalMarcacao = null;

                    BLL.Empresa bllEmpresa = BLL.Empresa.GetInstancia;
                    FbParameter[] parms2 = new FbParameter[]
                    {
                        new FbParameter("@cwk", FbDbType.Binary)
                    };
                    Modelo.Empresa objEmpresa = bllEmpresa.GetEmpresaPrincipal();
                    parms2[0].Value = bllEmpresa.GeraVersao("1.08.026" + objEmpresa.Numeroserie);
                    string cmd6 = "UPDATE \"cwkvsnsys\" SET \"cwk\" = @cwk";
                    DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, cmd6, true, parms2);
                    string cmd7 = "UPDATE \"bilhetesimp\" SET \"dscodigo\" = \"func\" WHERE \"bilhetesimp\".\"dscodigo\" = ''";
                    DAL.FB.DataBase.ExecNonQueryCmd(CommandType.Text, cmd7, true, parms2);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return false;
        }

        private static void AlteraPosicaoBilhete(List<Modelo.BilhetesImp> bilhetes, Modelo.BilhetesImp bilhete, Modelo.Marcacao marc, string hora, int posicao, string ent_sai, StringBuilder str, List<string> cmds)
        {
            var bilhetesHora = bilhetes.Where(b => b.Acao != Modelo.Acao.Alterar && b.Mar_data == marc.Data && b.Hora == hora);
            if (bilhetesHora.Count() > 0)
            {
                bilhete = bilhetesHora.First();
                bilhete.Acao = Modelo.Acao.Alterar;
                if (bilhete.Posicao != posicao || bilhete.Ent_sai != ent_sai)
                {
                    bilhete.Posicao = posicao;
                    bilhete.Ent_sai = ent_sai;
                    bilhete.Chave = bilhete.ToMD5();

                    str.Remove(0, str.Length);
                    str.AppendLine("UPDATE \"bilhetesimp\" SET");
                    str.AppendLine("\"posicao\" = " + bilhete.Posicao);
                    str.AppendLine(",\"ent_sai\" = '" + bilhete.Ent_sai + "'");
                    str.AppendLine(",\"chave\" = '" + bilhete.Chave + "'");
                    str.AppendLine("WHERE \"bilhetesimp\".\"id\" = " + bilhete.Id);
                    cmds.Add(str.ToString());
                }
            }
        }

        private static List<Modelo.BilhetesImp> LoadBilhetesPorFuncionario(string pDsCodigoFunc)
        {
            FbParameter[] parms = new FbParameter[]
            {
                new FbParameter("@func", FbDbType.VarChar)
            };
            string comando;
            if (!String.IsNullOrEmpty(pDsCodigoFunc))
            {
                parms[0].Value = pDsCodigoFunc;

                comando = "SELECT * FROM \"bilhetesimp\" WHERE \"dscodigo\" = @func AND \"importado\" = 1";
            }
            else
            {
                comando = "SELECT * FROM \"bilhetesimp\" WHERE \"importado\" = 1";
            }

            List<Modelo.BilhetesImp> ret = new List<Modelo.BilhetesImp>();

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, comando, parms);

            while (dr.Read())
            {
                Modelo.BilhetesImp obj = new Modelo.BilhetesImp();
                obj.Id = Convert.ToInt32(dr["id"]);
                obj.Altdata = (dr["altdata"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["altdata"]));
                obj.Althora = (dr["althora"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["althora"]));
                obj.Altusuario = (dr["altusuario"] is DBNull ? null : Convert.ToString(dr["altusuario"]));
                obj.Incdata = (dr["incdata"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["incdata"]));
                obj.Inchora = (dr["inchora"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["inchora"]));
                obj.Incusuario = (dr["incusuario"] is DBNull ? null : Convert.ToString(dr["incusuario"]));
                obj.Ordem = Convert.ToString(dr["ordem"]);
                obj.Data = Convert.ToDateTime(dr["data"]);
                obj.Hora = Convert.ToString(dr["hora"]);
                obj.Func = Convert.ToString(dr["func"]);
                obj.Relogio = Convert.ToString(dr["relogio"]);
                obj.Importado = Convert.ToInt16(dr["importado"]);
                obj.Mar_data = (dr["mar_data"] is DBNull ? null : (DateTime?)dr["mar_data"]);
                obj.Mar_hora = Convert.ToString(dr["mar_hora"]);
                obj.Mar_relogio = Convert.ToString(dr["mar_relogio"]);
                obj.Posicao = Convert.ToInt32(dr["posicao"]);
                obj.Ent_sai = Convert.ToString(dr["ent_sai"]);
                obj.Codigo = Convert.ToInt32(dr["codigo"]);
                obj.Chave = dr["chave"] is DBNull ? "" : Convert.ToString(dr["chave"]);
                obj.DsCodigo = dr["dscodigo"] is DBNull ? "" : Convert.ToString(dr["dscodigo"]);
                ret.Add(obj);
            }
            return ret;
        }

        private static List<Modelo.Marcacao> GetMarcPorFuncionario(int pIdFuncionario)
        {
            FbParameter[] parms = new FbParameter[1]
            { 
                    new FbParameter("@idfuncionario", FbDbType.Integer)
            };
            parms[0].Value = pIdFuncionario;

            string SELECTPFU = "SELECT \"marcacao\".* " +
                               ", \"funcionario\".\"nome\" AS \"funcionario\" " +
                        " FROM \"marcacao\" " +
                        " INNER JOIN \"funcionario\" ON \"funcionario\".\"id\" = \"marcacao\".\"idfuncionario\" " +
                        " WHERE \"marcacao\".\"idfuncionario\" = @idfuncionario ";

            FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, SELECTPFU, parms);

            List<Modelo.Marcacao> lista = new List<Modelo.Marcacao>();
            Modelo.Marcacao objMarcacao = null;
            while (dr.Read())
            {
                objMarcacao = new Modelo.Marcacao();
                objMarcacao.Id = Convert.ToInt32(dr["id"]);
                objMarcacao.Altdata = (dr["altdata"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["altdata"]));
                objMarcacao.Althora = (dr["althora"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["althora"]));
                objMarcacao.Altusuario = (dr["altusuario"] is DBNull ? null : Convert.ToString(dr["altusuario"]));
                objMarcacao.Incdata = (dr["incdata"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dr["incdata"]));
                objMarcacao.Inchora = (dr["inchora"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["inchora"]));
                objMarcacao.Incusuario = (dr["incusuario"] is DBNull ? null : Convert.ToString(dr["incusuario"]));
                objMarcacao.Idfuncionario = Convert.ToInt32(dr["idfuncionario"]);
                objMarcacao.Codigo = Convert.ToInt32(dr["codigo"]);
                objMarcacao.Dscodigo = Convert.ToString(dr["dscodigo"]);
                objMarcacao.Legenda = Convert.ToString(dr["legenda"]);
                objMarcacao.Data = Convert.ToDateTime(dr["data"]);
                objMarcacao.Dia = Convert.ToString(dr["dia"]);
                objMarcacao.Entrada_1 = Convert.ToString(dr["entrada_1"]);
                objMarcacao.Entrada_2 = Convert.ToString(dr["entrada_2"]);
                objMarcacao.Entrada_3 = Convert.ToString(dr["entrada_3"]);
                objMarcacao.Entrada_4 = Convert.ToString(dr["entrada_4"]);
                objMarcacao.Entrada_5 = Convert.ToString(dr["entrada_5"]);
                objMarcacao.Entrada_6 = Convert.ToString(dr["entrada_6"]);
                objMarcacao.Entrada_7 = Convert.ToString(dr["entrada_7"]);
                objMarcacao.Entrada_8 = Convert.ToString(dr["entrada_8"]);
                objMarcacao.Saida_1 = Convert.ToString(dr["saida_1"]);
                objMarcacao.Saida_2 = Convert.ToString(dr["saida_2"]);
                objMarcacao.Saida_3 = Convert.ToString(dr["saida_3"]);
                objMarcacao.Saida_4 = Convert.ToString(dr["saida_4"]);
                objMarcacao.Saida_5 = Convert.ToString(dr["saida_5"]);
                objMarcacao.Saida_6 = Convert.ToString(dr["saida_6"]);
                objMarcacao.Saida_7 = Convert.ToString(dr["saida_7"]);
                objMarcacao.Saida_8 = Convert.ToString(dr["saida_8"]);
                objMarcacao.Horastrabalhadas = Convert.ToString(dr["horastrabalhadas"]);
                objMarcacao.Horasextrasdiurna = Convert.ToString(dr["horasextrasdiurna"]);
                objMarcacao.Horasfaltas = Convert.ToString(dr["horasfaltas"]);
                objMarcacao.Entradaextra = Convert.ToString(dr["entradaextra"]);
                objMarcacao.Saidaextra = Convert.ToString(dr["saidaextra"]);
                objMarcacao.Horastrabalhadasnoturnas = Convert.ToString(dr["horastrabalhadasnoturnas"]);
                objMarcacao.Horasextranoturna = Convert.ToString(dr["horasextranoturna"]);
                objMarcacao.Horasfaltanoturna = Convert.ToString(dr["horasfaltanoturna"]);
                objMarcacao.Ocorrencia = Convert.ToString(dr["ocorrencia"]);
                objMarcacao.Idhorario = Convert.ToInt32(dr["idhorario"]);
                objMarcacao.Bancohorascre = Convert.ToString(dr["bancohorascre"]);
                objMarcacao.Bancohorasdeb = Convert.ToString(dr["bancohorasdeb"]);
                objMarcacao.Idfechamentobh = (dr["idfechamentobh"] is DBNull ? 0 : Convert.ToInt32(dr["idfechamentobh"]));
                objMarcacao.Semcalculo = (dr["semcalculo"] is DBNull ? (short)0 : Convert.ToInt16(dr["semcalculo"]));
                objMarcacao.Ent_num_relogio_1 = Convert.ToString(dr["ent_num_relogio_1"]);
                objMarcacao.Ent_num_relogio_2 = Convert.ToString(dr["ent_num_relogio_2"]);
                objMarcacao.Ent_num_relogio_3 = Convert.ToString(dr["ent_num_relogio_3"]);
                objMarcacao.Ent_num_relogio_4 = Convert.ToString(dr["ent_num_relogio_4"]);
                objMarcacao.Ent_num_relogio_5 = Convert.ToString(dr["ent_num_relogio_5"]);
                objMarcacao.Ent_num_relogio_6 = Convert.ToString(dr["ent_num_relogio_6"]);
                objMarcacao.Ent_num_relogio_7 = Convert.ToString(dr["ent_num_relogio_7"]);
                objMarcacao.Ent_num_relogio_8 = Convert.ToString(dr["ent_num_relogio_8"]);
                objMarcacao.Sai_num_relogio_1 = Convert.ToString(dr["sai_num_relogio_1"]);
                objMarcacao.Sai_num_relogio_2 = Convert.ToString(dr["sai_num_relogio_2"]);
                objMarcacao.Sai_num_relogio_3 = Convert.ToString(dr["sai_num_relogio_3"]);
                objMarcacao.Sai_num_relogio_4 = Convert.ToString(dr["sai_num_relogio_4"]);
                objMarcacao.Sai_num_relogio_5 = Convert.ToString(dr["sai_num_relogio_5"]);
                objMarcacao.Sai_num_relogio_6 = Convert.ToString(dr["sai_num_relogio_6"]);
                objMarcacao.Sai_num_relogio_7 = Convert.ToString(dr["sai_num_relogio_7"]);
                objMarcacao.Sai_num_relogio_8 = Convert.ToString(dr["sai_num_relogio_8"]);
                objMarcacao.Naoentrarbanco = (dr["naoentrarbanco"] is DBNull ? (short)0 : Convert.ToInt16(dr["naoentrarbanco"]));
                objMarcacao.Naoentrarnacompensacao = (dr["naoentrarnacompensacao"] is DBNull ? (short)0 : Convert.ToInt16(dr["naoentrarnacompensacao"]));
                objMarcacao.Horascompensadas = Convert.ToString(dr["horascompensadas"]);
                objMarcacao.Idcompensado = (dr["idcompensado"] is DBNull ? 0 : Convert.ToInt32(dr["idcompensado"]));
                objMarcacao.Naoconsiderarcafe = (dr["naoconsiderarcafe"] is DBNull ? (short)0 : Convert.ToInt16(dr["naoconsiderarcafe"]));
                objMarcacao.Dsr = (dr["dsr"] is DBNull ? (short)0 : Convert.ToInt16(dr["dsr"]));
                objMarcacao.Valordsr = Convert.ToString(dr["valordsr"]);
                objMarcacao.Abonardsr = (dr["abonardsr"] is DBNull ? (short)0 : Convert.ToInt16(dr["abonardsr"]));
                objMarcacao.Totalizadoresalterados = (dr["totalizadoresalterados"] is DBNull ? (short)0 : Convert.ToInt16(dr["totalizadoresalterados"]));
                objMarcacao.Calchorasextrasdiurna = (dr["calchorasextrasdiurna"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasextrasdiurna"]));
                objMarcacao.Calchorasextranoturna = (dr["calchorasextranoturna"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasextranoturna"]));
                objMarcacao.Calchorasfaltas = (dr["calchorasfaltas"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasfaltas"]));
                objMarcacao.Calchorasfaltanoturna = (dr["calchorasfaltanoturna"] is DBNull ? 0 : Convert.ToInt32(dr["calchorasfaltanoturna"]));
                objMarcacao.Funcionario = Convert.ToString(dr["funcionario"]);
                objMarcacao.Folga = (dr["folga"] is DBNull ? (short)0 : Convert.ToInt16(dr["folga"]));
                objMarcacao.Chave = dr["chave"] is DBNull ? "" : Convert.ToString(dr["chave"]);
                lista.Add(objMarcacao);
            }
            return lista;
        }

        private static void AtualizacaoVersao109027(bool recalcular, bool atualizarBil, List<Modelo.Funcionario> funcionarios)
        {
            FbParameter[] parms = new FbParameter[] { };
            try
            {
                string sql = " SELECT \"idjornada\" FROM \"jornadaalternativa\" ";
                FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
            }
            catch (Exception)
            {
                Console.WriteLine("Atualizando para a versão 1.09.027...");
                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    conn.Open();
                    bool executou = false;
                    using (FbTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string cmd1 = "ALTER TABLE \"bilhetesimp\" DROP CONSTRAINT \"ix_bilhetesimp\"";
                            string cmd2 = "ALTER TABLE \"bilhetesimp\" ADD CONSTRAINT \"ix_bilhetesimp\" UNIQUE (\"data\",\"hora\",\"func\",\"relogio\",\"mar_data\")";
                            string cmd3 = "CREATE TABLE \"jornada\" ( "
                                       + "\"id\" INTEGER NOT NULL, "
                                       + "\"codigo\" INTEGER NOT NULL, "
                                       + "\"descricao\" VARCHAR(70) CHARACTER SET NONE COLLATE NONE, "
                                       + "\"entrada_1\" VARCHAR(5) CHARACTER SET NONE COLLATE NONE, "
                                       + "\"entrada_2\" VARCHAR(5) CHARACTER SET NONE COLLATE NONE, "
                                       + "\"entrada_3\" VARCHAR(5) CHARACTER SET NONE COLLATE NONE, "
                                       + "\"entrada_4\" VARCHAR(5) CHARACTER SET NONE COLLATE NONE, "
                                       + "\"saida_1\" VARCHAR(5) CHARACTER SET NONE COLLATE NONE, "
                                       + "\"saida_2\" VARCHAR(5) CHARACTER SET NONE COLLATE NONE, "
                                       + "\"saida_3\" VARCHAR(5) CHARACTER SET NONE COLLATE NONE, "
                                       + "\"saida_4\" VARCHAR(5) CHARACTER SET NONE COLLATE NONE, "
                                       + "\"incdata\" TIMESTAMP, "
                                       + " \"inchora\" TIMESTAMP, "
                                       + " \"incusuario\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE, "
                                       + " \"altdata\" TIMESTAMP, "
                                       + " \"althora\" TIMESTAMP, "
                                       + " \"altusuario\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE, "
                                       + " \"numrelogio\" VARCHAR(3) CHARACTER SET NONE COLLATE NONE); ";

                            string cmd4 = " ALTER TABLE \"jornada\" ADD CONSTRAINT \"jornada_porcodigo\" PRIMARY KEY (\"id\"); ";

                            string cmd5 = " CREATE GENERATOR \"GEN_jornada_id\"; ";

                            string cmd6 = " SET GENERATOR \"GEN_jornada_id\" TO 0; ";

                            string cmd7 = " CREATE TRIGGER \"BI_jornada_id\" FOR \"jornada\" "
                                + " ACTIVE BEFORE INSERT "
                                + " POSITION 0 "
                                + " AS "
                                + " BEGIN "
                                + " IF (NEW.\"id\" IS NULL) THEN "
                                + " NEW.\"id\" = GEN_ID(\"GEN_jornada_id\", 1); "
                                + " END; ";

                            string cmd8 = "ALTER TABLE \"horariodetalhe\" ADD \"idjornada\" integer";
                            string cmd9 = "ALTER TABLE \"jornadaalternativa\" ADD \"idjornada\" integer";

                            string cmd10 = "ALTER TABLE \"marcacao\" ADD \"exphorasextranoturna\" varchar(5)";
                            string cmd11 = "ALTER TABLE \"eventos\" ADD \"extranoturnabh\" integer";
                            string cmd12 = "ALTER TABLE \"marcacao\" ADD \"tipohoraextrafalta\" integer";

                            string cmd13 = "CREATE TABLE \"layoutexportacao\" ( "
                                       + "\"id\" INTEGER NOT NULL, "
                                       + "\"codigo\" INTEGER NOT NULL, "
                                       + "\"descricao\" VARCHAR(50) CHARACTER SET NONE COLLATE NONE, "
                                       + "\"incdata\" TIMESTAMP, "
                                       + " \"inchora\" TIMESTAMP, "
                                       + " \"incusuario\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE, "
                                       + " \"altdata\" TIMESTAMP, "
                                       + " \"althora\" TIMESTAMP, "
                                       + " \"altusuario\" VARCHAR(20) CHARACTER SET NONE COLLATE NONE); ";

                            string cmd14 = " ALTER TABLE \"layoutexportacao\" ADD CONSTRAINT \"layoutexportacao_porcodigo\" PRIMARY KEY (\"id\"); ";

                            string cmd15 = " CREATE GENERATOR \"GEN_layoutexportacao_id\"; ";

                            string cmd16 = " SET GENERATOR \"GEN_layoutexportacao_id\" TO 0; ";

                            string cmd17 = " CREATE TRIGGER \"BI_layoutexportacao_id\" FOR \"layoutexportacao\" "
                                + " ACTIVE BEFORE INSERT "
                                + " POSITION 0 "
                                + " AS "
                                + " BEGIN "
                                + " IF (NEW.\"id\" IS NULL) THEN "
                                + " NEW.\"id\" = GEN_ID(\"GEN_layoutexportacao_id\", 1); "
                                + " END; ";

                            string cmd18 = "ALTER TABLE \"exportacaocampos\" ADD \"idlayoutexportacao\" integer";

                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd1, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd2, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd3, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd4, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd5, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd6, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd7, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd8, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd9, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd10, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd11, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd12, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd13, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd14, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd15, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd16, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd17, true, parms);
                            DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd18, true, parms);

                            trans.Commit();
                            executou = true;
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            Console.WriteLine(ex.Message);
                            Console.ReadKey();
                        }
                    }
                    if (executou)
                    {
                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                string cmd19 = "ALTER TABLE \"horariodetalhe\" ADD CONSTRAINT \"fk_horariodetalhe_jornada\" FOREIGN KEY (\"idjornada\") REFERENCES \"jornada\"(\"id\");";
                                string cmd20 = "ALTER TABLE \"jornadaalternativa\" ADD CONSTRAINT \"fk_jornadaalternativa_jornada\" FOREIGN KEY (\"idjornada\") REFERENCES \"jornada\"(\"id\");";
                                string cmd21 = "ALTER TABLE \"exportacaocampos\" ADD CONSTRAINT \"fk_expcampos_layoutexp\" FOREIGN KEY (\"idlayoutexportacao\") REFERENCES \"layoutexportacao\"(\"id\") ON DELETE CASCADE ON UPDATE CASCADE;";
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd19, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd20, true, parms);
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd21, true, parms);

                                #region Atualiza Versão
                                BLL.Empresa bllEmpresa = BLL.Empresa.GetInstancia;
                                FbParameter[] parms2 = new FbParameter[]
                            {
                                new FbParameter("@cwk", FbDbType.Binary)
                            };
                                Modelo.Empresa objEmpresa = bllEmpresa.GetEmpresaPrincipal();
                                parms2[0].Value = bllEmpresa.GeraVersao("1.09.027" + objEmpresa.Numeroserie);
                                string cmd15 = "UPDATE \"cwkvsnsys\" SET \"cwk\" = @cwk";
                                DAL.FB.DataBase.ExecNonQueryCmd(trans, CommandType.Text, cmd15, true, parms2);
                                #endregion

                                trans.Commit();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                Console.WriteLine(ex.Message);
                                Console.ReadKey();
                            }
                        }
                    }
                }

                List<string> comandos;
                if (atualizarBil)
                {
                    #region Incui e atualiza os bilhetes que possuem tratamentos
                    int posicao = 0;
                    string ent_sai = "";
                    DAL.FB.Tratamentomarcacao dalTratamento = DAL.FB.Tratamentomarcacao.GetInstancia;
                    DAL.FB.BilhetesImp dalBilhetes = DAL.FB.BilhetesImp.GetInstancia;
                    comandos = new List<string>();
                    List<Modelo.BilhetesImp> bilhetesIncluir = new List<Modelo.BilhetesImp>();
                    List<Modelo.Tratamentomarcacao> tratamentos = null;
                    List<Modelo.Marcacao> marcacoes = null;
                    List<Modelo.BilhetesImp> bilhetes = null;
                    Modelo.BilhetesImp bilhete = null;
                    StringBuilder str = new StringBuilder();
                    count = 0;
                    foreach (Modelo.Funcionario func in funcionarios)
                    {
                        marcacoes = GetMarcPorFuncionario(func.Id);
                        bilhetes = dalBilhetes.LoadPorFuncionario(func.Dscodigo);
                        tratamentos = dalTratamento.LoadPorFuncionario(func.Id);
                        foreach (Modelo.Marcacao marc in marcacoes)
                        {
                            var bilhetesData = bilhetes.Where(b => b.Mar_data == marc.Data);
                            var tratMarcacao = tratamentos.Where(t => t.Idmarcacao == marc.Id);
                            if (tratMarcacao.Count() > 0)
                            {
                                foreach (Modelo.Tratamentomarcacao trat in tratMarcacao)
                                {
                                    ent_sai = trat.Indicador.Substring(3, 1);
                                    posicao = Convert.ToInt32(trat.Indicador.Substring(trat.Indicador.Length - 1, 1));
                                    if (trat.Ocorrencia == 'D')
                                    {
                                        var up = bilhetesData.Where(b => b.Ent_sai == ent_sai && b.Posicao == posicao);
                                        if (up.Count() > 0)
                                        {
                                            bilhete = up.First();
                                            str.Remove(0, str.Length);
                                            str.AppendLine("UPDATE \"bilhetesimp\" SET");
                                            str.AppendLine("\"ocorrencia\" = 'D'");
                                            if (trat.Motivo.Length > 100)
                                            {
                                                trat.Motivo = trat.Motivo.Substring(0, 100);
                                            }
                                            str.AppendLine(",\"motivo\" = '" + trat.Motivo.Replace("'", String.Empty).Replace("\"", String.Empty).Replace("´", String.Empty) + "'");
                                            if (trat.Idjustificativa > 0)
                                                str.AppendLine(",\"idjustificativa\" = " + trat.Idjustificativa);
                                            str.AppendLine("WHERE \"bilhetesimp\".\"id\" = " + bilhete.Id);
                                            comandos.Add(str.ToString());
                                        }
                                    }
                                    else if (trat.Ocorrencia == 'I' || trat.Ocorrencia == 'P')
                                    {
                                        bilhete = new Modelo.BilhetesImp();
                                        bilhete.Acao = Modelo.Acao.Incluir;
                                        bilhete.Incdata = DateTime.Now;
                                        bilhete.Inchora = DateTime.Now;
                                        bilhete.Incusuario = "cwork";
                                        bilhete.Importado = 1;
                                        bilhete.DsCodigo = func.Dscodigo;
                                        bilhete.Func = marc.Dscodigo;
                                        bilhete.Mar_data = marc.Data;
                                        bilhete.Data = marc.Data;
                                        bilhete.Codigo = 0;
                                        bilhete.Ent_sai = ent_sai;
                                        bilhete.Posicao = posicao;
                                        bilhete.Relogio = trat.Ocorrencia == 'P' ? "PA" : "MA";
                                        bilhete.Mar_relogio = bilhete.Relogio;
                                        bilhete.Motivo = trat.Motivo;
                                        bilhete.Ocorrencia = trat.Ocorrencia;
                                        bilhete.Idjustificativa = trat.Idjustificativa;
                                        #region Seta a hora do bilhete
                                        switch (trat.Indicador)
                                        {
                                            case "txtEntrada_1":
                                                bilhete.Hora = marc.Entrada_1;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtSaida_1":
                                                bilhete.Hora = marc.Saida_1;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtEntrada_2":
                                                bilhete.Hora = marc.Entrada_2;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtSaida_2":
                                                bilhete.Hora = marc.Saida_2;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtEntrada_3":
                                                bilhete.Hora = marc.Entrada_3;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtSaida_3":
                                                bilhete.Hora = marc.Saida_3;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtEntrada_4":
                                                bilhete.Hora = marc.Entrada_4;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtSaida_4":
                                                bilhete.Hora = marc.Saida_4;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtEntrada_5":
                                                bilhete.Hora = marc.Entrada_5;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtSaida_5":
                                                bilhete.Hora = marc.Saida_5;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtEntrada_6":
                                                bilhete.Hora = marc.Entrada_6;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtSaida_6":
                                                bilhete.Hora = marc.Saida_6;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtEntrada_7":
                                                bilhete.Hora = marc.Entrada_7;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtSaida_7":
                                                bilhete.Hora = marc.Saida_7;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtEntrada_8":
                                                bilhete.Hora = marc.Entrada_8;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                            case "txtSaida_8":
                                                bilhete.Hora = marc.Saida_8;
                                                bilhete.Mar_hora = bilhete.Hora;
                                                break;
                                        }
                                        #endregion
                                        bilhete.Chave = bilhete.ToMD5();
                                        bilhetesIncluir.Add(bilhete);
                                    }
                                }
                            }
                        }
                        progresso();
                    }
                    Console.WriteLine();

                    Console.Write("Aguarde...");
                    if (comandos.Count > 0)
                    {
                        using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                        {
                            FbBatchExecution batch = new FbBatchExecution(conn);
                            batch.SqlStatements.AddRange(comandos);
                            batch.Execute();
                        }
                    }

                    if (bilhetesIncluir.Count > 0)
                    {
                        dalBilhetes.IncluirbilhetesEmLote(bilhetesIncluir);
                    }
                    #endregion
                }

                Console.WriteLine("Alterando tabela de marcação [separa extra falta]");
                Console.WriteLine("Aguarde...");
                BLL.Parametros bllParametros = BLL.Parametros.GetInstancia;
                List<Modelo.Parametros> parametros = bllParametros.GetAllList();
                foreach (Modelo.Parametros p in parametros)
                {
                    bllParametros.AtualizaTipoExtraFaltaMarcacoes(p.Id, p.TipoHoraExtraFalta, null, null);
                }

                Console.WriteLine("Alterando tabela de horários [jornada]");
                Console.WriteLine("Aguarde...");
                BLL.HorarioDetalhe bllHorarioDetalhe = BLL.HorarioDetalhe.GetInstancia;
                List<Modelo.HorarioDetalhe> horariosDetalhe = bllHorarioDetalhe.GetAllList();
                List<Modelo.Jornada> jornadas = new List<Modelo.Jornada>();
                Modelo.Jornada objJornada;
                int codigo = 1;
                StringBuilder desc = new StringBuilder();
                count = 0;
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
                        objJornada.Codigo = codigo++;
                        desc.Remove(0, desc.Length);
                        desc.Append(hd.Entrada_1 + " - " + hd.Saida_1);
                        desc.Append(" - " + hd.Entrada_2 + " - " + hd.Saida_2);
                        desc.Append(" - " + hd.Entrada_3 + " - " + hd.Saida_3);
                        desc.Append(" - " + hd.Entrada_4 + " - " + hd.Saida_4);
                        objJornada.Descricao = desc.ToString();
                        jornadas.Add(objJornada);
                    }
                    progresso();
                }
                Console.WriteLine();

                Console.WriteLine("Alterando tabela de jornada alternativa [jornada]");
                Console.WriteLine("Aguarde...");
                BLL.JornadaAlternativa bllJornadaAlternativa = BLL.JornadaAlternativa.GetInstancia;
                List<Modelo.JornadaAlternativa> jornadasAlternativas = bllJornadaAlternativa.GetAllList(false);
                count = 0;
                foreach (Modelo.JornadaAlternativa ja in jornadasAlternativas)
                {
                    if (jornadas.Where(j => j.Entrada_1 == ja.Entrada_1 && j.Entrada_2 == ja.Entrada_2 && j.Entrada_3 == ja.Entrada_3
                        && j.Saida_1 == ja.Saida_1 && j.Saida_2 == ja.Saida_2 && j.Saida_3 == ja.Saida_3 && j.Saida_4 == ja.Saida_4).Count() == 0)
                    {
                        objJornada = new Modelo.Jornada();
                        objJornada.Entrada_1 = ja.Entrada_1;
                        objJornada.Entrada_2 = ja.Entrada_2;
                        objJornada.Entrada_3 = ja.Entrada_3;
                        objJornada.Entrada_4 = ja.Entrada_4;
                        objJornada.Saida_1 = ja.Saida_1;
                        objJornada.Saida_2 = ja.Saida_2;
                        objJornada.Saida_3 = ja.Saida_3;
                        objJornada.Saida_4 = ja.Saida_4;
                        objJornada.Codigo = codigo++;
                        desc.Remove(0, desc.Length);
                        desc.Append(ja.Entrada_1 + " - " + ja.Saida_1);
                        desc.Append(" - " + ja.Entrada_2 + " - " + ja.Saida_2);
                        desc.Append(" - " + ja.Entrada_3 + " - " + ja.Saida_3);
                        desc.Append(" - " + ja.Entrada_4 + " - " + ja.Saida_4);
                        objJornada.Descricao = desc.ToString();
                        jornadas.Add(objJornada);
                    }
                    progresso();
                }
                Console.WriteLine();

                Console.WriteLine("Salvando Jornadas...");
                BLL.Jornada bllJornada = BLL.Jornada.GetInstancia;
                comandos = new List<string>();
                StringBuilder cmdj = new StringBuilder();
                foreach (Modelo.Jornada jornada in jornadas)
                {
                    if (jornada.Entrada_1 != "--:--" && jornada.Saida_1 != "--:--")
                    {
                        bllJornada.Salvar(Modelo.Acao.Incluir, jornada);
                        cmdj.Remove(0, cmdj.Length);
                        cmdj.AppendLine("UPDATE \"horariodetalhe\" SET \"idjornada\" = ");
                        cmdj.AppendLine(jornada.Id.ToString());
                        cmdj.AppendLine("WHERE");
                        cmdj.AppendLine("\"entrada_1\" = '" + jornada.Entrada_1 + "'");
                        cmdj.AppendLine("AND \"entrada_2\" = '" + jornada.Entrada_2 + "'");
                        cmdj.AppendLine("AND \"entrada_3\" = '" + jornada.Entrada_3 + "'");
                        cmdj.AppendLine("AND \"entrada_4\" = '" + jornada.Entrada_4 + "'");
                        cmdj.AppendLine("AND \"saida_1\" = '" + jornada.Saida_1 + "'");
                        cmdj.AppendLine("AND \"saida_2\" = '" + jornada.Saida_2 + "'");
                        cmdj.AppendLine("AND \"saida_3\" = '" + jornada.Saida_3 + "'");
                        cmdj.AppendLine("AND \"saida_4\" = '" + jornada.Saida_4 + "'");
                        comandos.Add(cmdj.ToString());

                        cmdj.Remove(0, cmdj.Length);
                        cmdj.AppendLine("UPDATE \"jornadaalternativa\" SET \"idjornada\" = ");
                        cmdj.AppendLine(jornada.Id.ToString());
                        cmdj.AppendLine("WHERE");
                        cmdj.AppendLine("\"entrada_1\" = '" + jornada.Entrada_1 + "'");
                        cmdj.AppendLine("AND \"entrada_2\" = '" + jornada.Entrada_2 + "'");
                        cmdj.AppendLine("AND \"entrada_3\" = '" + jornada.Entrada_3 + "'");
                        cmdj.AppendLine("AND \"entrada_4\" = '" + jornada.Entrada_4 + "'");
                        cmdj.AppendLine("AND \"saida_1\" = '" + jornada.Saida_1 + "'");
                        cmdj.AppendLine("AND \"saida_2\" = '" + jornada.Saida_2 + "'");
                        cmdj.AppendLine("AND \"saida_3\" = '" + jornada.Saida_3 + "'");
                        cmdj.AppendLine("AND \"saida_4\" = '" + jornada.Saida_4 + "'");
                        comandos.Add(cmdj.ToString());
                    }
                }
                cmdj.Remove(0, cmdj.Length);
                cmdj.AppendLine("UPDATE \"jornadaalternativa\" SET \"idjornada\" = NULL");
                cmdj.AppendLine("WHERE \"entrada_1\" = '--:--'");
                comandos.Add(cmdj.ToString());

                cmdj.Remove(0, cmdj.Length);
                cmdj.AppendLine("UPDATE \"horariodetalhe\" SET \"idjornada\" = NULL");
                cmdj.AppendLine("WHERE \"entrada_1\" = '--:--'");
                comandos.Add(cmdj.ToString());

                if (comandos.Count > 0)
                {
                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        FbBatchExecution batch = new FbBatchExecution(conn);
                        batch.SqlStatements.AddRange(comandos);
                        batch.Execute();
                    }
                }

                Console.WriteLine("Atualizando Exportação para Folha...");

                BLL.ExportacaoCampos bllExportacao = BLL.ExportacaoCampos.GetInstancia;
                List<Modelo.ExportacaoCampos> campos = bllExportacao.GetAllList();
                if (campos.Count > 0)
                {
                    BLL.LayoutExportacao bllLayout = BLL.LayoutExportacao.GetInstancia;
                    Modelo.LayoutExportacao objLayout = new Modelo.LayoutExportacao();
                    objLayout.Codigo = 1;
                    objLayout.Descricao = "Layout Geral";
                    bllLayout.Salvar(Modelo.Acao.Incluir, objLayout);

                    string cmd = "UPDATE \"exportacaocampos\" SET \"idlayoutexportacao\" = " + objLayout.Id;
                    DAL.FB.DataBase.ExecuteNonQuery(CommandType.Text, cmd, parms);
                }

                if (recalcular)
                {
                    StringBuilder cmd = new StringBuilder();
                    cmd.AppendLine("select min(\"data\") as \"min\", max(\"data\") as \"max\"");
                    cmd.AppendLine("from (");
                    cmd.AppendLine("select \"marcacao\".\"data\"");
                    cmd.AppendLine("from \"marcacao\"");
                    cmd.AppendLine("where \"marcacao\".\"horastrabalhadas\" is NULL");
                    cmd.AppendLine(")");
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, cmd.ToString(), parms);

                    if (dr.Read())
                    {
                        DateTime? min = dr["min"] is DBNull ? null : (DateTime?)dr["min"];
                        DateTime? max = dr["max"] is DBNull ? null : (DateTime?)dr["max"];

                        if (min != null && max != null)
                        {
                            Console.WriteLine("Corrigindo tabela de marcações.");
                            Console.WriteLine("Aguarde, caso existam muitas marcações este processo pode demorar...");
                            Console.WriteLine();
                            Modelo.ProgressBar pb = new Modelo.ProgressBar();
                            pb.incrementaPB = IncrementaProgressBar;
                            pb.setaMensagem = SetaMensagem;
                            pb.setaMinMaxPB = SetaMinMaxProgressBar;
                            pb.setaValorPB = SetaValorProgressBar;
                            BLL.CalculaMarcacao bllCalcula = new BLL.CalculaMarcacao(null, 0, min.Value, max.Value, pb, true);
                            bllCalcula.CalculaMarcacoes();
                            bllCalcula = null;
                        }
                    }
                }

                bool alterou = false;
                foreach (Modelo.Parametros parm in parametros)
                {
                    if (Modelo.cwkFuncoes.ConvertHorasMinuto(parm.THoraExtra) > 10)
                    {
                        parm.THoraExtra = "00:10";
                        alterou = true;
                    }

                    if (Modelo.cwkFuncoes.ConvertHorasMinuto(parm.THoraFalta) < 5)
                    {
                        parm.THoraFalta = "00:05";
                        alterou = true;
                    }

                    if (alterou)
                    {
                        Console.WriteLine("Atualizando parametro " + parm.Descricao);
                        bllParametros.Salvar(Modelo.Acao.Alterar, parm);
                        alterou = false;
                    }
                }
                Console.WriteLine("A tolerancia para hora extra foi ajustada para no maximo 10 minutos");
                Console.WriteLine("A tolerancia para hora falta foi ajustada para no minimo 05 minutos");
            }
        }

        private static void corrigeBilhetes()
        {
            List<string> comandos = new List<string>();
            StringBuilder str = new StringBuilder();
            DAL.FB.Funcionario dalFuncionario = DAL.FB.Funcionario.GetInstancia;
            DAL.FB.Marcacao dalMarcacao = DAL.FB.Marcacao.GetInstancia;
            DAL.FB.BilhetesImp dalBilhetes = DAL.FB.BilhetesImp.GetInstancia;
            DAL.FB.Tratamentomarcacao dalTratamento = DAL.FB.Tratamentomarcacao.GetInstancia;
            List<Modelo.Funcionario> funcionarios = dalFuncionario.GetAllList(true);
            List<Modelo.BilhetesImp> bilhetes = null;

            foreach (Modelo.Funcionario func in funcionarios)
            {
                bilhetes = LoadBilhetesPorFuncionario(func.Dscodigo);
                foreach (Modelo.BilhetesImp bil in bilhetes)
                {
                    bil.Chave = bil.ToMD5();
                    str.Remove(0, str.Length);
                    str.AppendLine("UPDATE \"bilhetesimp\" SET");
                    str.AppendLine("\"chave\" = '" + bil.Chave + "'");
                    str.AppendLine("WHERE \"bilhetesimp\".\"id\" = " + bil.Id);
                    comandos.Add(str.ToString());
                }
            }

            if (comandos.Count > 0)
            {
                Console.WriteLine("Aguarde...");
                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    FbBatchExecution batch = new FbBatchExecution(conn);
                    int metade = (int)Math.Floor((decimal)comandos.Count / 2);
                    if (metade > 0)
                    {
                        batch.SqlStatements.AddRange(comandos.GetRange(0, metade));
                        batch.Execute();
                        batch.SqlStatements.Clear();
                    }
                    List<string> l = comandos.GetRange(metade, comandos.Count - metade);
                    if (l.Count > 0)
                    {
                        batch.SqlStatements.AddRange(l);
                        batch.Execute();
                    }
                }
            }

        }

        private static void progresso()
        {
            switch (count++)
            {
                case 0:
                    Console.Write("\\\r");
                    break;
                case 1:
                    Console.Write("|\r");
                    break;
                case 2:
                    Console.Write("/\r");
                    break;
                case 3:
                    Console.Write("-\r");
                    count = 0;
                    break;
            }
        }
    }
}
