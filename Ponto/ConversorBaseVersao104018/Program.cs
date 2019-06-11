using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Isql;
using System.Threading;


namespace ConversorBaseVersao104018
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

        static object locker = new object();
        static bool done;

        static void Main(string[] args)
        {
            try
            {
                Modelo.Cw_Usuario objUsuario = new Modelo.Cw_Usuario();
                objUsuario.Login = "cwork";
                Modelo.cwkGlobal.objUsuarioLogado = objUsuario;
                Modelo.cwkGlobal.BD = 2;

                Console.WriteLine("Cwork Sistemas");

                #region 1.00.003 para 1.00.004
                try
                {
                    FbParameter[] parms = new FbParameter[0];

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
                                FbParameter[] parms = new FbParameter[0];

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


                        Console.WriteLine("Conversao efetuada com sucesso....");
                    }
                }
                #endregion

                #region 1.00.006 para 1.01.007

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

                #endregion

                #region versões anteriores para 1.01.010

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

                #endregion

                #region 1.01.010 para 1.01.011

                using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                {
                    conn.Open();
                    FbParameter[] parms = new FbParameter[0];

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

                try
                {
                    FbParameter[] parms = new FbParameter[0];
                    string sql = " SELECT \"codigo\" FROM \"fechamentobhd\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    using (FbConnection conn = new FbConnection(Modelo.cwkGlobal.CONN_STRING))
                    {
                        conn.Open();
                        FbParameter[] parms = new FbParameter[0];
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

                //Intervalo Automático no horário
                try
                {
                    FbParameter[] parms = new FbParameter[0];
                    string sql = " SELECT \"intervaloautomatico\" FROM \"horario\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    FbParameter[] parms = new FbParameter[0];
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

                //Intervalo Automático na jornada alternativa
                try
                {
                    FbParameter[] parms = new FbParameter[0];
                    string sql = " SELECT \"intervaloautomatico\" FROM \"jornadaalternativa\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    FbParameter[] parms = new FbParameter[0];
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

                //Marcação - Folga
                try
                {
                    FbParameter[] parms = new FbParameter[0];
                    string sql = " SELECT \"folga\" FROM \"marcacao\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    FbParameter[] parms = new FbParameter[0];
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

                //Cadastro de Backup
                try
                {
                    FbParameter[] parms = new FbParameter[0];
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
                                FbParameter[] parms = new FbParameter[0];

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

                #region 1.02.013 para 1.03.014

                try
                {
                    FbParameter[] parms = new FbParameter[0];

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
                                FbParameter[] parms = new FbParameter[0];

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
                    FbParameter[] parms = new FbParameter[0];

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
                                FbParameter[] parms = new FbParameter[0];

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

                        using (FbTransaction trans = conn.BeginTransaction())
                        {
                            try
                            {
                                DAL.FB.Empresa dalEmpresa = DAL.FB.Empresa.GetInstancia;
                                List<Modelo.Empresa> empresas = dalEmpresa.GetAllList();

                                foreach (Modelo.Empresa emp in empresas)
                                {
                                    emp.Numeroserie = emp.Id.ToString();
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


                        Console.WriteLine("Campo criado com sucesso...");
                    }
                }
                #endregion

                #region Campo chave na tabela marcacao e campo bdalterado na tabela empresa

                try
                {
                    FbParameter[] parms = new FbParameter[0];

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
                                FbParameter[] parms = new FbParameter[0];

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

                DAL.FB.Funcionario dalFuncionario = DAL.FB.Funcionario.GetInstancia;
                List<Modelo.Funcionario> funcionarios = dalFuncionario.GetAllList(true);

                try
                {
                    FbParameter[] parms = new FbParameter[0];

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
                                FbParameter[] parms = new FbParameter[0];

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
                        int qtd = funcionarios.Count, count = 0;
                        decimal perido = Math.Round((decimal)qtd / 100);
                        FbBatchExecution bath = new FbBatchExecution(conn);
                        StringBuilder str = new StringBuilder();
                        List<Modelo.Marcacao> marcacoes = null;
                        foreach (Modelo.Funcionario func in funcionarios)
                        {
                            marcacoes = dalMarcacao.GetPorFuncionario(func.Id);
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
                            count++;
                            if (count == perido)
                            {
                                Console.Write("|");
                                count = 0;
                            }
                        }
                        Console.WriteLine("Salvando Marcações...");
                        if (bath.SqlStatements.Count > 0)
                        {
                            bath.Execute();
                        }
                        Console.WriteLine();
                    }
                }
                try
                {
                    FbParameter[] parms = new FbParameter[0];

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
                                FbParameter[] parms = new FbParameter[0];

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
                        DAL.FB.BilhetesImp dalBilhetesimp = DAL.FB.BilhetesImp.GetInstancia;
                        List<Modelo.BilhetesImp> bilhetes = dalBilhetesimp.GetAllList();
                        int qtd = bilhetes.Count, count = 0;
                        decimal perido = Math.Round((decimal)qtd / 100);
                        FbBatchExecution bath = new FbBatchExecution(conn);
                        StringBuilder str = new StringBuilder();
                        foreach (Modelo.BilhetesImp bil in bilhetes)
                        {
                            bil.Chave = bil.ToMD5();
                            str.Remove(0, str.Length);
                            str.Append("UPDATE \"bilhetesimp\" SET \"chave\" = '");
                            str.Append(bil.Chave);
                            str.Append("' WHERE \"bilhetesimp\".\"id\" = ");
                            str.Append(bil.Id);
                            bath.SqlStatements.Add(str.ToString());
                            count++;
                            if (count == perido)
                            {
                                Console.Write("|");
                                count = 0;
                            }
                        }
                        Console.WriteLine("Salvando Bilhetes...");
                        if (bath.SqlStatements.Count > 0)
                        {
                            bath.Execute();
                        }
                        Console.WriteLine();
                    }
                }

                #endregion

                #region Indices na tabela bilhetesimp
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

                #region Correção na tabela de horário flexível para cargamista

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
                        List<Modelo.HorarioDetalhe> horariosDetalhe = dalHorarioDetalhe.GetAllList();
                        StringBuilder str = new StringBuilder();
                        int qtd = horariosDetalhe.Count, count = 0;
                        decimal perido = Math.Round((decimal)qtd / 100);
                        foreach (Modelo.HorarioDetalhe hd in horariosDetalhe)
                        {
                            str.Remove(0, str.Length);
                            str.Append("UPDATE \"horariodetalhe\" SET \"marcacargahorariamista\" = ");
                            if (!String.IsNullOrEmpty(hd.Cargahorariamista) && hd.Cargahorariamista != "--:--")
                            {
                                str.Append("1");
                            }
                            else
                            {
                                str.Append("0");
                            }
                            str.Append(" WHERE \"horariodetalhe\".\"id\" = '");
                            str.Append(hd.Id + "'");
                            batch.SqlStatements.Add(str.ToString());

                            count++;
                            if (count == perido)
                            {
                                Console.Write("|");
                                count = 0;
                            }
                        }
                        Console.WriteLine("Salvando Horários...");
                        if (batch.SqlStatements.Count > 0)
                        {
                            batch.Execute();
                        }
                        Console.WriteLine();
                    }
                }
                #endregion

                #region Versão 1.04.016
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
                        int qtd = funcionarios.Count, count = 0;
                        decimal perido = Math.Round((decimal)qtd / 100);
                        FbBatchExecution bath = new FbBatchExecution(conn);
                        StringBuilder str = new StringBuilder();
                        List<Modelo.Marcacao> marcacoes = null;
                        foreach (Modelo.Funcionario func in funcionarios)
                        {
                            marcacoes = dalMarcacao.GetPorFuncionario(func.Id);
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
                            count++;
                            if (count == perido)
                            {
                                Console.Write("|");
                                count = 0;
                            }
                        }
                        Console.WriteLine("Salvando Marcações...");
                        if (bath.SqlStatements.Count > 0)
                        {
                            bath.Execute();
                        }
                        Console.WriteLine();

                        Console.WriteLine("Corrigindo tabela de bilhetes...");
                        Console.WriteLine("Aguarde, caso existam muitos bilhetes este processo pode demorar...");
                        Console.WriteLine();
                        DAL.FB.BilhetesImp dalBilhetesimp = DAL.FB.BilhetesImp.GetInstancia;
                        List<Modelo.BilhetesImp> bilhetes = dalBilhetesimp.GetAllList();
                        qtd = bilhetes.Count; count = 0;
                        perido = Math.Round((decimal)qtd / 100);
                        bath = new FbBatchExecution(conn);
                        str = new StringBuilder();
                        foreach (Modelo.BilhetesImp bil in bilhetes)
                        {
                            bil.Chave = bil.ToMD5();
                            str.Remove(0, str.Length);
                            str.Append("UPDATE \"bilhetesimp\" SET \"chave\" = '");
                            str.Append(bil.Chave);
                            str.Append("' WHERE \"bilhetesimp\".\"id\" = ");
                            str.Append(bil.Id);
                            bath.SqlStatements.Add(str.ToString());
                            count++;
                            if (count == perido)
                            {
                                Console.Write("|");
                                count = 0;
                            }
                        }
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
                #endregion

                #region Versão 1.05.018

                try
                {
                    FbParameter[] parms = new FbParameter[0];
                    string sql = " SELECT \"he50n\" FROM \"eventos\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    FbParameter[] parms = new FbParameter[0];
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
                    FbParameter[] parms = new FbParameter[0];
                    string sql = " SELECT \"hesabn\" FROM \"eventos\"";
                    FbDataReader dr = DAL.FB.DataBase.ExecuteReader(CommandType.Text, sql, parms);
                }
                catch (FbException)
                {
                    FbParameter[] parms = new FbParameter[0];
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

                #region Versão 1.05.019
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

                Console.WriteLine("Fim da conversão. Tecle para sair...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
