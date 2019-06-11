using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao322023
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ADICIONAR_CAMPO_DESC_FERIADO, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_CAMPO_FERIADO, null);
            db.ExecuteNonQuery(CommandType.Text, CAMPO_DESC_FERIADO_NOTNULL, null);
            db.ExecuteNonQuery(CommandType.Text, CONSTRAINT_CHECK_DESC_FERIADO, null);
            db.ExecuteNonQuery(CommandType.Text, PERMISSOES_PONTO_WEB, null);
        }

        #region ALTERS
        private static readonly string ADICIONAR_CAMPO_DESC_FERIADO = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'desconsiderarferiado' AND tabela.name = N'horario')
                    BEGIN
                        ALTER TABLE dbo.horario ADD
	                    desconsiderarferiado int NULL
                    END";

        private static readonly string UPDATE_CAMPO_FERIADO = @"
            IF EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'desconsiderarferiado' AND tabela.name = N'horario')
                    BEGIN
                        UPDATE Horario SET desconsiderarferiado = 0
                        WHERE desconsiderarferiado  IS NULL
                    END";            

        private static readonly string CAMPO_DESC_FERIADO_NOTNULL = @"
            IF EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'desconsiderarferiado' AND tabela.name = N'horario')
                    BEGIN
                        ALTER TABLE Horario ALTER COLUMN desconsiderarferiado INT NOT NULL
                    END";

        private static readonly string CONSTRAINT_CHECK_DESC_FERIADO = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'desconsiderarferiado' AND tabela.name = N'horario')
                    BEGIN
                        ALTER TABLE Horario ADD CONSTRAINT CK_DESCONSIDERA_FERIADO
                        CHECK (desconsiderarferiado IN (0,1))
                    END";

        private static readonly string PERMISSOES_PONTO_WEB = @"
            if ((select count(*) from dbo.cw_acesso where controller like 'Afastamento') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1000,'Afastamento','Afastamento','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'BancoHoras') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1001,'BancoHoras','Banco de Horas','Banco Horas',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Compensacao') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1002,'Compensacao','Compensação','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'ConfiguracaoGeral') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1003,'ConfiguracaoGeral','Configuracão Geral','Configuração',0,0,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Departamento') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1004,'Departamento','Departamento','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Empresa') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1005,'Empresa','Empresa','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Eventos') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1006,'Eventos','Eventos para Exportação','Exportação',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'ExportacaoMinisterioTrabalho') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1007,'ExportacaoMinisterioTrabalho','Exportacao para Ministério do Trabalho','Exportação',0,0,1,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'FechamentoBH') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1008,'FechamentoBH','Fechamento do Banco de Horas','Banco Horas',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Feriado') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1009,'Feriado','Feriado','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Funcao') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1010,'Funcao','Função','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Funcionario') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1011,'Funcionario','Funcionário','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'FuncionarioExcluido') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1012,'FuncionarioExcluido','Exclusão de Funcionário','Manutenção',0,0,0,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'FuncionarioHistorico') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1013,'FuncionarioHistorico','Histórico de Funcionário','Manutenção',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'GrupoUsuario') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1014,'GrupoUsuario','Grupo de Usuários','Segurança',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Horario') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1015,'Horario','Horário','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'HorarioMovel') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1016,'HorarioMovel','Horário Flexível','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'ImportacaoBilhetes') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1017,'ImportacaoBilhetes','Importação de AFD','Manutenção',0,0,0,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'ImpressaoCartaoPonto') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1018,'ImpressaoCartaoPonto','Impressão de Cartão Ponto','Tabela de Marcações',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'JornadaAlternativa') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1019,'JornadaAlternativa','Jornada Alternativa','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Jornada') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1020,'Jornada','Jornada','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Justificativa') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1021,'Justificativa','Justificativa','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'LancamentoCreditoDebito') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1022,'LancamentoCreditoDebito','Lançamento de Crédito/Débito','Banco Horas',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'ManutencaoBilhetes') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1023,'ManutencaoBilhetes','Manutenção de Bilhetes','Manutenção',0,0,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Marcacao') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1024,'Marcacao','Marcação','Manutenção',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'MudancaHorario') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1025,'MudancaHorario','Mudança de Horário','Manutenção',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'MudCodigoFunc') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1026,'MudCodigoFunc','Mudança de Código de Funcionário','Manutenção',1,0,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Ocorrencia') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1027,'Ocorrencia','Ocorrência','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Parametro') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1028,'Parametro','Parâmetro','Configuração',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Provisorio') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1029,'Provisorio','Código Provisório','Manutenção',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'RecalculaMarcacao') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1030,'RecalculaMarcacao','Recalcular Marcações','Manutenção',0,0,0,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Rep') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1031,'Rep','Rep','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Usuario') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1032,'Usuario','Usuário','Segurança',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioAbsenteismo') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1033,'RelatorioAbsenteismo','Relatório de Absenteísmo','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioAfastamento') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1034,'RelatorioAfastamento','Relatório de Afastamento','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioBancoHoras') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1035,'RelatorioBancoHoras','Relatório de Banco de Horas','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioCartaoPonto') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1036,'RelatorioCartaoPonto','Relatório de Cartão Ponto','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioEspelho') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1037,'RelatorioEspelho','Relatório Espelho','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioFuncionario') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1038,'RelatorioFuncionario','Relatório de Funcionário','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioHistorico') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1039,'RelatorioHistorico','Relatório de Histórico','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioHoraExtra') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1040,'RelatorioHoraExtra','Relatório de Hora Extra','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioHorario') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1041,'RelatorioHorario','Relatório de Horário','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioManutencaoDiaria') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1042,'RelatorioManutencaoDiaria','Relatório de Manutenção Diária','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioOcorrencias') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1043,'RelatorioOcorrencias','Relatório de Ocorrências','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioPresenca') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1044,'RelatorioPresenca','Relatório de Presença','Relatório',1,0,0,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'FechamentoBHD') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1045,'FechamentoBHD','Fechamento BH por Funcionário','Banco Horas',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'LayoutExportacao') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1046,'LayoutExportacao','Exportação','Exportação',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'GrupoAcesso') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1047,'GrupoAcesso','Acesso por Grupo','Segurança',1,0,0,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'ExportacaoFolha') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1048,'ExportacaoFolha','Exportação para Folha','Tela de Exportação',0,0,0,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'EnvioEmpresaFuncionariosRep') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1049,'EnvioEmpresaFuncionariosRep','Envio de Empresa e Funcionários','Manutenção',0,0,1,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'EmpresaUsuario') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1050,'EmpresaUsuario','Alteração dos Registros de Empresa/Usuários','Tabela de Empresa',0,0,0,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'Contrato') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1051,'Contrato','Contrato','Cadastro',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'ContratoUsuario') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1052,'ContratoUsuario','Alteração dos Registros de Contratos/Usuários','Tabela de Contrato',0,0,0,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'ContratoFuncionario') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1053,'ContratoFuncionario','Alteração dos Registros de Contratos/Funcionário','Tabela de Contrato',0,0,0,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'EnvioConfiguracoesDataHora') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1054,'EnvioConfiguracoesDataHora','Enviar Configurações de Data e Hora','Manutenção',0,0,1,0);
                if ((select count(*) from dbo.cw_acesso where controller like 'LayoutExportacao') <= 0)
                INSERT INTO dbo.cw_acesso (codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1170,'LayoutExportacao','Exportação','Exportação',1,1,1,1)
                if ((select count(*) from dbo.cw_acesso where controller like 'Alertas') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1071,'Alertas','Alertas','Configuração',1,1,1,1);
                if ((select count(*) from dbo.cw_acesso where controller like 'RelatorioAbono') <= 0)
                INSERT INTO dbo.cw_acesso(codigo,Controller,Nome,Menu,Consultar,Excluir,Cadastrar,Alterar) VALUES (1072,'RelatorioAbono','Relatório de Abonos','Relatório',1,0,0,0);";

        #endregion
    }
}
