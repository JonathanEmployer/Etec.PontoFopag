using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FirebirdSql.Data.FirebirdClient;

namespace ConversorBaseParaVersao103014R28
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

        static void Main(string[] args)
        {
            Modelo.Cw_Usuario objUsuario = new Modelo.Cw_Usuario();
            objUsuario.Login = "cwork";
            Modelo.cwkGlobal.objUsuarioLogado = objUsuario;

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
                            Console.WriteLine("Conversão efetuada com sucesso.");
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


                    Console.WriteLine("Conversao efetuada com sucesso. Tecle para sair...");
                    Console.ReadLine();
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

            Console.WriteLine("Fim da conversão. Tecle para sair...");
            Console.ReadLine();
        }
    }
}
