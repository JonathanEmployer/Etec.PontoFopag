using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao306006
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_PARAMETROS_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_PARAMETROS_02, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_PARAMETROS_03, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_PARAMETROS_04, null);
            db.ExecuteNonQuery(CommandType.Text, INSERT_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_01, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_02, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_03, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_04, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_05, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_06, null);
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_07, null);
        }

        #region Alters
        private static readonly string ALTER_TBL_PARAMETROS_01 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'DiaFechamentoInicial' AND tabela.name = N'parametros')
		BEGIN
            ALTER TABLE dbo.parametros ADD
	            DiaFechamentoInicial smallint NULL;
		END";

        private static readonly string ALTER_TBL_PARAMETROS_02 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'DiaFechamentoFinal' AND tabela.name = N'parametros')
		BEGIN
            ALTER TABLE dbo.parametros ADD
	            DiaFechamentoFinal smallint NULL;
		END";

        private static readonly string ALTER_TBL_PARAMETROS_03 =
        @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				inner join sys.sysobjects tabela on tabela.id = st.object_id 
				WHERE st.name = N'MudaPeriodoImediatamento' AND tabela.name = N'parametros')
		BEGIN
            ALTER TABLE dbo.parametros ADD
	            MudaPeriodoImediatamento bit NULL;
		END";


        private static readonly string CREATE_TBL_01 =
            @"IF NOT EXISTS (select * from sys.objects where name = N'equipamentohomologado' and type = N'U')
            BEGIN
            CREATE TABLE [dbo].[equipamentohomologado](
	            [id] [int] IDENTITY(1,1) NOT NULL,
	            [codigoModelo] [varchar](50) NULL,
	            [nomeModelo] [varchar](200) NULL,
	            [nomeFabricante] [varchar](200) NULL,
	            [numeroFabricante] [varchar](50) NULL,
	            [identificacaoRelogio] [int] NULL,
	            [incdata] [datetime] NULL,
	            [inchora] [datetime] NULL,
	            [incusuario] [varchar](20) NULL,
	            [altdata] [datetime] NULL,
	            [althora] [datetime] NULL,
	            [altusuario] [varchar](20) NULL,
             CONSTRAINT [PK_equipamentohomologado] PRIMARY KEY CLUSTERED 
            (
	            [id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]
            END";

        private static readonly string ALTER_TBL_PARAMETROS_04 =
            @"IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
				    inner join sys.sysobjects tabela on tabela.id = st.object_id 
				    WHERE st.name = N'idequipamentohomologado' AND tabela.name = N'rep')
		    BEGIN
                ALTER TABLE dbo.rep ADD
	                idequipamentohomologado int NULL;
		    END";

        private static readonly string INSERT_TBL_01 =
            @"if ((select count(*) from dbo.equipamentohomologado) = 0)
            begin
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00001','POINTLINE 1510 - CARD','RW Tecnologia Indústria e Comércio Ltda','1',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00002','X REP-520 BB300','TRIX Tecnologia Ltda','2',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00003','PRINTPOINT II','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00004','X REP-520 BP11','TRIX Tecnologia Ltda','2',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00005','ORION 6A','Henry Equipamentos Eletrônicos e Sistemas Ltda. (A casa do Equipamento)','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00006','MD REP','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00007','ORION 6C','Henry Equipamentos Eletrônicos e Sistemas Ltda. (A casa do Equipamento)','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00008','ORION 6B','Henry Equipamentos Eletrônicos e Sistemas Ltda. (A casa do Equipamento)','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00009','ORION 6D','Henry Equipamentos Eletrônicos e Sistemas Ltda. (A casa do Equipamento)','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00010','XREP-520 BP','TRIX Tecnologia Ltda','2',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00011','CodinReP 2000 TTS','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00012','REP-1000 BARCODE','Trilobit Comércio, Montagem e Fabricação de Placas Eletrônicas Ltda. (Trilobit)','7',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00013','REP-1000 PROX','Trilobit Comércio, Montagem e Fabricação de Placas Eletrônicas Ltda. (Trilobit)','7',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00014','CodinReP 2000 TTI','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00015','CodinReP 2000 TII','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00016','CodinReP 2000 TEE','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00017','CodinReP 2000 TSS','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00018','CodinReP 2000 TTE','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00019','KURUMIM REP BIO BR','Proveu Indústria Eletrônica Ltda.','8',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00020','KURUMIM REP','Proveu Indústria Eletrônica Ltda.','8',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00021','KURUMIM REP BIO NT BR','Proveu Indústria Eletrônica Ltda.','8',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00022','KURUMIM REP MAX NT','Proveu Indústria Eletrônica Ltda.','8',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00023','KURUMIM REP NT','Proveu Indústria Eletrônica Ltda.','8',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00024','INNER REP BIO 2i','Topdata Sistemas de Automação Ltda.','9',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00025','INNER REP BARRAS 2i','Topdata Sistemas de Automação Ltda.','9',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00026','INNER REP BARRAS','Topdata Sistemas de Automação Ltda.','9',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00027','Ah-10 cb','Ahgora Sistemas Ltda. (Ahgora)','10',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00028','Ah-10 cbm','Ahgora Sistemas Ltda. (Ahgora)','10',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00029','Ah-10 c','Ahgora Sistemas Ltda. (Ahgora)','10',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00030','INFORREP-1510','Renato Zanotti Stagliório EPP (Inforcomp)','11',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00031','REP-1000 BIOPROX','Trilobit Comércio, Montagem e Fabricação de Placas Eletrônicas Ltda. (Trilobit)','7',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00032','REP-1000 BIOBARCODE','Trilobit Comércio, Montagem e Fabricação de Placas Eletrônicas Ltda. (Trilobit)','7',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00033','FORREP CODE','Athos Sistemas de Identificação Ltda.','12',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00034','FORREP PROXY','Athos Sistemas de Identificação Ltda.','12',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00035','FORREP BIO','Athos Sistemas de Identificação Ltda.','12',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00036','CODINReP MD, LEITORA CÓDIGO DE BARRAS','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00037','CODINReP MD, LEITORA BIOMÉTRICA','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00038','CODINReP MD, LEITORAS CÓDIGO DE BARRAS E BIOMÉTRICA','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00039','CODINReP MD, LEITORA PROXIMIDADE','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00040','CODINReP MD, LEITORAS PROXIMIDADE E BIOMÉTRICA','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00041','CODINReP MD, LEITORA SMART CARD Proximidade,','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00042','CODINReP MD, LEITORA SMART CARD E BIOMÉTRICA','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00043','DIGIREP SMART','Digicon S.A - Controle Eletrõnico para Mecânica.','13',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00044','DIGIREP PROX','Digicon S.A - Controle Eletrõnico para Mecânica.','13',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00045','POINTLINE 1510 BIO PROX','RW Tecnologia Indústria e Comércio Ltda','1',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00046','ID-BIO V2','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)','14',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00047','ID-BIO V1','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)','14',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00048','MD REP PB','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00049','MD REP B','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00050','MD REP BB','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00051','PRINTPOINT II P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00052','PRINTPOINT II BB','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00053','PRINTPOINT II B','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00054','INNER REP BIO','Topdata Sistemas de Automação Ltda.','9',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00055','XREP-520 P','TRIX Tecnologia Ltda','2',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00056','XREP-520 C','TRIX Tecnologia Ltda','2',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00057','XREP-520 CP','TRIX Tecnologia Ltda','2',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00058','BIO','Circuitec Indústria de Equipamentos Eletrônicos Ltda.','15',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00059','ÓPTICO','Circuitec Indústria de Equipamentos Eletrônicos Ltda.','15',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00060','INFORREP-1510 II','Renato Zanotti Stagliório EPP (Inforcomp)','11',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00061','INNER REP PROX','Topdata Sistemas de Automação Ltda.','9',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00062','INNER REP PROX 2i','Topdata Sistemas de Automação Ltda.','9',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00063','INNER REP BIO BARRAS 2i','Topdata Sistemas de Automação Ltda.','9',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00064','INNER REP BIO PROX 2i','Topdata Sistemas de Automação Ltda.','9',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00065','CodinReP 2000 TTT','Telemática Sistemas Inteligentes Ltda.','6',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00066','INOVA 2 REP C','Task Sistemas de Computação S.A','16',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00067','EXATA REP 1510 AJM','Guirado & Grégio Ltda. (GRUPO EXATAID)','17',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00068','POINTLINE 1510 DUOCARD BIO','RW Tecnologia Indústria e Comércio Ltda','1',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00069','POINTLINE 1510 DUOCARD','RW Tecnologia Indústria e Comércio Ltda','1',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00070','P1510-BIOBARRAS','Sanvitron Controle e Automação Ltda. (Sanvitron)','18',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00071','P1510-BARRAS','Sanvitron Controle e Automação Ltda. (Sanvitron)','18',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00072','P1510-BIOPROX','Sanvitron Controle e Automação Ltda. (Sanvitron)','18',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00073','P1510-PROX','Sanvitron Controle e Automação Ltda. (Sanvitron)','18',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00074','INOVA 2 REP CB+','Task Sistemas de Computação S.A','16',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00075','INOVA 2 REP CB','Task Sistemas de Computação S.A','16',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00076','INOVA 2 REP BM','Task Sistemas de Computação S.A','16',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00077','INOVA 2 REP BM+','Task Sistemas de Computação S.A','16',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00078','REP0570','ICOP Tecnologia da Informação Ltda. (INFOSPHERA)','19',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00079','INOVA 2 REP MCB+','Task Sistemas de Computação S.A','16',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00080','INOVA 2 REP PX','Task Sistemas de Computação S.A','16',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00081','INOVA 2 REP PX+','Task Sistemas de Computação S.A','16',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00082','BAR-M','Digicon S.A - Controle Eletrõnico para Mecânica.','13',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00083','BIO-BM','Digicon S.A - Controle Eletrõnico para Mecânica.','13',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00084','KP1510','Keypass Tecnologia Ltda.','20',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00085','R210','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00086','Prox-B','Digicon S.A - Controle Eletrõnico para Mecânica.','13',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00087','Bio-PB','Digicon S.A - Controle Eletrõnico para Mecânica.','13',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00088','Prox-A','Digicon S.A - Controle Eletrõnico para Mecânica.','13',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00089','Prox-C','Digicon S.A - Controle Eletrõnico para Mecânica.','13',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00090','REP-ip','Sisponto Sistemas Inteligentes Ltda ME','22',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00091','REP CB7 BTS','Westphal & Cia Ltda. (PONTO SYSTEM)','23',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00092','REP CB7 BSS','Westphal & Cia Ltda. (PONTO SYSTEM)','23',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00093','ID REP BP 51','Daiken Automação Ltda.','24',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00094','BeRep002','Be Safer Sistemas de Controle e Informação Ltda. (Be Safe)','25',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00095','BeRep001','Be Safer Sistemas de Controle e Informação Ltda. (Be Safe)','25',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00096','MD-REP 1704 PROXI','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00097','MD-REP 1704 BIO','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00098','MD-REP V2 P','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00099','PRINTPOINT II V2 BB','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00100','PRINTPOINT II V2 PB','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00101','MD-REP V2 PB','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00102','PRINTPOINT II V2 BPB','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00103','MD-REP V2 BB','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00104','MD-REP V2 B','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00105','MD-REP V2F BB','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00106','PRINTPOINT II V2F BB','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00107','PRINTPOINT II V2 P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00108','PRINTPOINT II V2 B','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00109','MD-REP V2 BPB','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00110','WXS-REP-B400ID','Wellcare Automação Ltda.','26',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00111','WXS-REP-B400','Wellcare Automação Ltda.','26',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00112','PASSFINGER 2040','Biometrus Indústria Eletro-Eletrônica S.A.','27',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00113','Cronoplus 5','Cronex Automação Empresarial Ltda.','28',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00114','Cronoplus 6','Cronex Automação Empresarial Ltda.','28',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00115','R200','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00116','SPACE','Codax Sistemas Ltda','29',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00117','SPACE BIO','Codax Sistemas Ltda','29',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00118','PROX-D','Digicon S.A - Controle Eletrõnico para Mecânica.','13',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00119','BIO-PA','Digicon S.A - Controle Eletrõnico para Mecânica.','13',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00120','R230','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00121','R100','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00122','R110','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00123','MD-REP V3 S','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00124','MD-REP V3 PB-H','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00125','MD-REP V3 P-M','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00126','MD-REP V3 P-H','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00127','MD-REP V3 SB','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00128','MD-REP V3 PB-M','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00129','PRINTPOINT II V3 P-H','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00130','PRINTPOINT II V3 SB','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00131','PRINTPOINT II V3 PB-M','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00132','PRINTPOINT II V3 S','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00133','PRINTPOINT II V3 P-M','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00134','PRINTPOINT II V3 PB-H','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00135','EXATA REP 1560 RFID Bio','Guirado & Grégio Ltda. (GRUPO EXATAID)','17',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00136','EXATA REP 1540 RFID','Guirado & Grégio Ltda. (GRUPO EXATAID)','17',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00137','EXATA REP 1520 BIO','Guirado & Grégio Ltda. (GRUPO EXATAID)','17',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00138','EXATA REP 1530 BIO','Guirado & Grégio Ltda. (GRUPO EXATAID)','17',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00139','EXATA REP 1550 PDA Card','Guirado & Grégio Ltda. (GRUPO EXATAID)','17',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00140','EXATA REP 1511 Barras','Guirado & Grégio Ltda. (GRUPO EXATAID)','17',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00141','MINIPRINT BR','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00142','MINIPRINT PA','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00143','MINIPRINT BM','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00144','MINIPRINT PS','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00145','MINIPRINT MG','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00146','MD0705 PA','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00147','MD0705 BR','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00148','MD0705 BM','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00149','WXS-REP-B400FP-ID','Wellcare Automação Ltda.','26',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00150','REP CB7 BBTS','Westphal & Cia Ltda. (PONTO SYSTEM)','23',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00151','REP CB7 BBSS','Westphal & Cia Ltda. (PONTO SYSTEM)','23',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00152','BIO REP-100','TRIX Tecnologia Ltda','2',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00153','MD-REP V4 PB','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00154','MD0705 PS','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00155','MD-REP V4 BB','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)','5',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00156','MINIPRINT BM-SA','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00157','Marque Ponto-BP','Tecvan Informática Ltda.','30',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00158','Marque Ponto-BPD','Tecvan Informática Ltda.','30',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00159','ETHOS BIO','CQS Tecnologia e Serviços Ltda.','31',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00160','ETHOS BAR','CQS Tecnologia e Serviços Ltda.','31',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00161','PRISMA E','Henry Equipamentos Eletrônicos e Sistemas Ltda.','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00162','PRISMA G','Henry Equipamentos Eletrônicos e Sistemas Ltda.','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00163','PRISMA H','Henry Equipamentos Eletrônicos e Sistemas Ltda.','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00164','PRISMA I','Henry Equipamentos Eletrônicos e Sistemas Ltda.','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00165','PRISMA J','Henry Equipamentos Eletrônicos e Sistemas Ltda.','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00166','SCI BIO PROX','S C I - Inovações Tecnolólicas Ltda ME','32',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00167','EvolutionBR RFID','Acronyn - Comércio de Produtos Eletrônicos Ltda.','33',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00168','EvolutionBR Bio','Acronyn - Comércio de Produtos Eletrônicos Ltda.','33',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00169','TL1.REP','Translarm Indústria Eletrônica Ltda.','34',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00170','TL1.BIO','Translarm Indústria Eletrônica Ltda.','34',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00171','PRISMA F','Henry Equipamentos Eletrônicos e Sistemas Ltda.','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00172','SMART PUNCH','Smart Card Solutions Ltda.','35',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00173','STARREP BIO PROXY','Athos Sistemas de Identificação Ltda.','12',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00174','STARREP MIFARE','Athos Sistemas de Identificação Ltda.','12',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00175','ID REP BI 01','Daiken Automação Ltda.','24',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00176','ID REP BP 31','Daiken Automação Ltda.','24',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00177','REP BIO I','Bioforlife Comércio e Prestação de Serviços em Biometria Ltda','37',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00178','BIO-PROX-BARRAS','RJF 2005 INFORMÁTICA LTDA','36',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00179','STARREP BIO','Athos Sistemas de Identificação Ltda.','12',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00180','IDNOX','DIXI - TI Serviços em Tecnologia da Informação Ltda - ME','38',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00181','PRINTPOINT II V3 BSB','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00182','ETHOS PROX-I','CQS Tecnologia e Serviços Ltda.','31',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00183','ETHOS PROX-II','CQS Tecnologia e Serviços Ltda.','31',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00184','VELTI C','Velti Tecnologia, Sistemas e Engenharia Ltda - ME','39',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00185','VELTI B','Velti Tecnologia, Sistemas e Engenharia Ltda - ME','39',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00186','VELTI A','Velti Tecnologia, Sistemas e Engenharia Ltda - ME','39',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00187','VELTI D','Velti Tecnologia, Sistemas e Engenharia Ltda - ME','39',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00188','MEGA 200','Mega Montagem e Manutenção de Equipamentos Ltda - ME','40',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00189','MEGA 300','Mega Montagem e Manutenção de Equipamentos Ltda - ME','40',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00190','SUPER FÁCIL R02','Henry Equipamentos Eletrônicos e Sistemas Ltda.','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00191','PRINTPOINT LI PROX','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00192','PRINTPOINT LI BIO','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00193','ADVANCED POINT REP PROX','IO ELETRÔNICA LTDA. - EPP','41',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00194','ADVANCED POINT REP BIO','IO ELETRÔNICA LTDA. - EPP','41',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00195','ADVANCED POINT REP CODE','IO ELETRÔNICA LTDA. - EPP','41',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00196','REP iDX MULT','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)','14',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00197','VELTI F','Velti Tecnologia, Sistemas e Engenharia Ltda - ME','39',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00198','VELTI G','Velti Tecnologia, Sistemas e Engenharia Ltda - ME','39',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00199','VELTI H','Velti Tecnologia, Sistemas e Engenharia Ltda - ME','39',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00200','VELTI I','Velti Tecnologia, Sistemas e Engenharia Ltda - ME','39',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00201','VELTI J','Velti Tecnologia, Sistemas e Engenharia Ltda - ME','39',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00202','PRISMA K','Henry Equipamentos Eletrônicos e Sistemas Ltda. (A casa do Equipamento)','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00203','MEGA 100','Mega Montagem e Manutenção de Equipamentos Ltda - ME','40',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00204','SUPER FÁCIL R01','Henry Equipamentos Eletrônicos e Sistemas Ltda. (A casa do Equipamento)','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00205','SUPER FÁCIL R04','Henry Equipamentos Eletrônicos e Sistemas Ltda. (A casa do Equipamento)','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00206','SUPER FÁCIL R03','Henry Equipamentos Eletrônicos e Sistemas Ltda. (A casa do Equipamento)','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00207','MEGA 400','Mega Montagem e Manutenção de Equipamentos Ltda - ME','40',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00208','VELTI E','Velti Tecnologia, Sistemas e Engenharia Ltda - ME','39',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00209','PRINTPOINT II V3 BS','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)','3',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00210','REP IDX CARD','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)','14',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00211','REP IDX BIO','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)','14',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00212','DATAREP i8','Diponto Comércio de Relógios Ltda','42',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00213','IDNOX LT PROX','DIXI - TI Serviços em Tecnologia da Informação Ltda - ME','38',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00214','IDNOX LT SMART PROX','DIXI - TI Serviços em Tecnologia da Informação Ltda - ME','38',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00215','IDNOX LT BIO','DIXI - TI Serviços em Tecnologia da Informação Ltda - ME','38',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00216','IDNOX LT BIO PROX','DIXI - TI Serviços em Tecnologia da Informação Ltda - ME','38',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00217','COMPACTO R01','Henry Equipamentos Eletrônicos e Sistemas Ltda. (A casa do Equipamento)','4',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00218','PointLine BIOPROX-S','Enterplak Produtos Eletrônicos Ltda','43',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00219','PointLine PROX-S','Enterplak Produtos Eletrônicos Ltda','43',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00220','PointLine BIOPROX-C','Enterplak Produtos Eletrônicos Ltda','43',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00221','PointLine BIOPROX-BC','Enterplak Produtos Eletrônicos Ltda','43',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00222','POINTLINE BIO-MBC','Enterplak Produtos Eletrônicos Ltda','43',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00223','POINTLINE BIOPROX-BS','Enterplak Produtos Eletrônicos Ltda','43',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00224','R300','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00225','DATAREP i8T','Diponto Comércio de Relógios Ltda','42',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00226','R130','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00227','R150','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00228','R310','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00229','R330','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00230','R410','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00231','R160','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00232','R430','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00233','ECO500BPR','ZPM Indústria e Comércio Ltda.','21',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00234','DATAREP4T','Diponto Comércio de Relógios Ltda','42',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00235','DATAREPi4T','Diponto Comércio de Relógios Ltda','42',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00236','Marque Ponto-D','Tecvan Informática Ltda.','30',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00237','Marque Ponto-P','Tecvan Informática Ltda.','30',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00238','EXATA REP 1570-F','Fatima Helena da Silva Gregio - EPP','45',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00239','EXATA REP 1580','Guirado & Grégio Ltda. (GRUPO EXATAID)','17',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00240','EXATA REP 1580-A','Angela Maria Brambati - EPP','44',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00241','EXATA REP 1580-F','Fatima Helena da Silva Gregio - EPP','45',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00242','Kurumim REP II','Proveu Indústria Eletrônica Ltda.','8',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00243','Kurumim REP II PX','Proveu Indústria Eletrônica Ltda.','8',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00244','Kurumim REP II BIO','Proveu Indústria Eletrônica Ltda.','8',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00245','Kurumim REP II BIO NT','Proveu Indústria Eletrônica Ltda.','8',1);
	            Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('00246','Kurumim REP II MAX','Proveu Indústria Eletrônica Ltda.','8',1);
                Insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio) values('99999','Não Homologado','Equipamento Não Homologado.','99999',1);
            end;";

        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_01 =
        @"update equipamentohomologado set numeroFabricante = replicate('0',5-(len(numeroFabricante)))+numeroFabricante where len(numeroFabricante) < 5;";

        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_02 =
        @"update r 
               set r.idequipamentohomologado = t.IdEquipamentoHomologado
              from rep r
             inner join (select *,	
 					            isnull((select top(1) nomeModelo from equipamentoHomologado where numeroFabricante = r.numeroFabricante and codigoModelo = r.codigoModelo),
		 					            (select top(1) nomemodelo from equipamentoHomologado where (identificacaoRelogio = r.relogio and r.relogio <> 1) or (r.relogio = 1 and codigoModelo = '00064'))) relogioHomologado,
					            isnull((select top(1) id from equipamentoHomologado where numeroFabricante = r.numeroFabricante and codigoModelo = r.codigoModelo),
							            (select top(1) id from equipamentoHomologado where (identificacaoRelogio = r.relogio and r.relogio <> 1) or (r.relogio = 1 and codigoModelo = '00064'))) IdEquipamentoHomologado
			               from (
				             select id, codigo, relogio, numserie,
						            case when Len(numserie) > 10 then
								            substring(rep.numserie,1,5)
							            else null end numeroFabricante,
						            case when Len(numserie) > 10 then
								            substring(rep.numserie,6,5)
							            else null end codigoModelo
				               from rep
				              where rep.idequipamentohomologado is null
					            ) r 
                        ) t on t.id = r.id;";

        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_03 =
        @"update equipamentohomologado set
            identificacaoRelogio = 2 
            where nomeModelo like '%Orion 6%';";

        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_04 =
        @"update equipamentohomologado set
            identificacaoRelogio = 3 
            where nomeFabricante like '%Trilobit%';";

        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_05 =
        @"update equipamentohomologado set
            identificacaoRelogio = 4 
            where nomeModelo like '%SUPER FÁCIL%';";

        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_06 =
        @"update equipamentohomologado set
            identificacaoRelogio = 5 
            where nomeModelo like '%REP IDX BIO%';";

        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_07 =
        @"update equipamentohomologado set
            identificacaoRelogio = 6 
            where nomeModelo like '%R130%';";


        #endregion
    }
}
