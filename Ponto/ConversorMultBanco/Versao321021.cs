using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao321021
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, UPDATE_HOMOLOGA_MADIS, null);
            db.ExecuteNonQuery(CommandType.Text, NOVOS_REP_HOMOLOGADO_MTE, null);
            db.ExecuteNonQuery(CommandType.Text, HOMOLOGACAO_INNER_REP_PLUS, null);
            db.ExecuteNonQuery(CommandType.Text, AUMENTA_CAMPO_SENHA, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_TRATAMENTO_BILHETES, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_FUNCAO_TRATAMENTO_BILHETES, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TAMANHO_NUM_REL, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_IMPORTA_MARCACAO, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_INSERT_MARCARCAO, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_UPDATE_BILHETE, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_UPDATE_MARCACAO, null);
            db.ExecuteNonQuery(CommandType.Text, DROP_CREATE_MARCACAO_LOTE, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_IMPORTA_MARCACAO, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_INSERT_MARCACAO, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_UPDATE_BILHETE, null);
            db.ExecuteNonQuery(CommandType.Text, CREATE_UPDATE_MARCACAO, null);
        }

        #region ALTERS

        private static readonly string UPDATE_HOMOLOGA_MADIS = @"
            update equipamentohomologado set identificacaoRelogio = 11 where numeroFabricante = '00005' -- Utiliza mesma integração do Dimep_PrintPoint
            update equipamentohomologado set identificacaoRelogio = 19 where nomemodelo like '%0705%' and numeroFabricante = '00005'
            update equipamentohomologado set identificacaoRelogio = 20 where nomemodelo like '%1704%' and numeroFabricante = '00005'
            update rep set relogio = eh.identificacaoRelogio 
              from rep
             inner join equipamentohomologado eh on rep.idequipamentohomologado = eh.id
             where eh.numeroFabricante = '00005'
               and rep.relogio = 1";



        private static readonly string NOVOS_REP_HOMOLOGADO_MTE = @"
            if ((select count(*) from equipamentohomologado where codigoModelo =  247) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'247',5),'EXATA REP 1570','Guirado & Grégio Ltda. (GRUPO EXATAID)',right(replicate('0',5)+'17',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  248) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'248',5),'EXATA REP 1570-A','Angela Maria Brambati - EPP',right(replicate('0',5)+'44',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  249) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'249',5),'ECO500PR','ZPM Indústria e Comércio Ltda.',right(replicate('0',5)+'21',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  250) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'250',5),'HEXA A','Henry Equipamentos Eletrônicos e Sistemas Ltda.',right(replicate('0',5)+'4',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  251) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'251',5),'HEXA B','Henry Equipamentos Eletrônicos e Sistemas Ltda.',right(replicate('0',5)+'4',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  252) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'252',5),'HEXA C','Henry Equipamentos Eletrônicos e Sistemas Ltda.',right(replicate('0',5)+'4',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  253) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'253',5),'HEXA D','Henry Equipamentos Eletrônicos e Sistemas Ltda.',right(replicate('0',5)+'4',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  254) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'254',5),'HEXA E','Henry Equipamentos Eletrônicos e Sistemas Ltda.',right(replicate('0',5)+'4',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  255) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'255',5),'PRINTPOINT III B_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  256) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'256',5),'PRINTPOINT III BP_LCD-1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  257) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'257',5),'PRINTPOINT III BH_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  258) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'258',5),'PRINTPOINT III BHW_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  259) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'259',5),'PRINTPOINT III BM_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  260) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'260',5),'PRINTPOINT III BMW_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  261) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'261',5),'PRINTPOINT III BS_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  262) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'262',5),'PRINTPOINT III BHI_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  263) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'263',5),'PRINTPOINT III B_S_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  264) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'264',5),'PRINTPOINT III BP_S_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  265) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'265',5),'PRINTPOINT III BPW_S_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  266) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'266',5),'PRINTPOINT III BH_S_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  267) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'267',5),'PRINTPOINT III BHW_S_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  268) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'268',5),'PRINTPOINT III BM_S_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  269) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'269',5),'PRINTPOINT III BMW_S_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  270) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'270',5),'PRINTPOINT III BS_S_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  271) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'271',5),'PRINTPOINT III BHI_S_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  272) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'272',5),'PRINTPOINT III B_V_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  273) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'273',5),'PRINTPOINT III BP_V-LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  274) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'274',5),'PRINTPOINT III BPW_V_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  275) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'275',5),'PRINTPOINT III BH_V_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  276) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'276',5),'PRINTPOINT III BHW_V_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  277) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'277',5),'PRINTPOINT III BM_V_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  278) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'278',5),'PRINTPOINT III BMW_V_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  279) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'279',5),'PRINTPOINT III BS_V_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  280) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'280',5),'PRINTPOINT III BHI_V_LCD_1P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  281) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'281',5),'PRINTPOINT III B_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  282) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'282',5),'PRINTPOINT III BP_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  283) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'283',5),'PRINTPOINT III BPW_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  284) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'284',5),'PRINTPOINT III BH_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  285) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'285',5),'PRINTPOINT III BM_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  286) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'286',5),'PRINTPOINT III BMW_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  287) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'287',5),'PRINTPOINT III BS_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  288) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'288',5),'PRINTPOINT III BHI_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  289) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'289',5),'PRINTPOINT III B_S_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  290) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'290',5),'PRINTPOINT III BP_S_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  291) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'291',5),'PRINTPOINT III BPW_S_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  292) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'292',5),'PRINTPOINT III BH_S_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  293) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'293',5),'PRINTPOINT III BHW_S_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  294) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'294',5),'PRINTPOINT III BM_S_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  295) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'295',5),'PRINTPOINT III BMW_S_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  296) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'296',5),'PRINTPOINT III BS_S_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  297) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'297',5),'PRINTPOINT III BHI_S_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  298) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'298',5),'PRINTPOINT III B_V_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  299) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'299',5),'PRINTPOINT III BP_V_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  300) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'300',5),'PRINTPOINT III BPW_V_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  301) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'301',5),'PRINTPOINT III BH_V_LCD_2P','Dimas de Melo Pimenta Sistemas de Ponto e Acesso Ltda. (Dimep)',right(replicate('0',5)+'3',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  302) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'302',5),'MD REP EVO B_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  303) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'303',5),'MD REP EVO BP_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  304) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'304',5),'MD REP EVO BPW_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  305) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'305',5),'MD REP EVO BH_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  306) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'306',5),'MD REP EVO BHW_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  307) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'307',5),'MD REP EVO BM_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  308) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'308',5),'MD REP EVO BMW_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  309) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'309',5),'MD REP EVO BS_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  310) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'310',5),'MD REP EVO BHI_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  311) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'311',5),'MD REP EVO B_S_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  312) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'312',5),'MD REP EVO BP_S_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  313) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'313',5),'MD REP EVO BPW_S_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  314) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'314',5),'MD REP EVO BH_S_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  315) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'315',5),'MD REP EVO BHW_S_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  316) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'316',5),'MD REP EVO BM_S_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  317) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'317',5),'MD REP EVO BMW_S_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  318) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'318',5),'MD REP EVO BS_S_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  319) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'319',5),'MD REP EVO BHI_S_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  320) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'320',5),'MD REP EVO B_V_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  321) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'321',5),'MD REP EVO BP_V_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  322) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'322',5),'MD REP EVO BPW_V_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  323) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'323',5),'MD REP EVO BH_V_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  324) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'324',5),'MD REP EVO BHW_V_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  325) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'325',5),'MD REP EVO BM_V_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  326) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'326',5),'MD REP EVO BMW_V_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  327) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'327',5),'MD REP EVO BS_V_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  328) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'328',5),'MD REP EVO BHI_V_1I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  329) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'329',5),'MD REP EVO B_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  330) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'330',5),'MD REP EVO BP_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  331) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'331',5),'MD REP EVO BPW_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  332) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'332',5),'MD REP EVO BH_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  333) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'333',5),'MD REP EVO BHW_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  334) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'334',5),'MD REP EVO BM_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  335) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'335',5),'MD REP EVO BMW_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  336) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'336',5),'MD REP EVO BS_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  337) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'337',5),'MD REP EVO BHI_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  338) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'338',5),'MD REP EVO B_S_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  339) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'339',5),'MD REP EVO BP_S_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  340) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'340',5),'MD REP EVO BPW_S_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  341) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'341',5),'MD REP EVO BH_S_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  342) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'342',5),'MD REP EVO BHW_S_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  343) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'343',5),'MD REP EVO BM_S_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  344) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'344',5),'MD REP EVO BMW_S_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  345) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'345',5),'MD REP EVO BS_S_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  346) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'346',5),'MD REP EVO BHI_S_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  347) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'347',5),'MD REP EVO B_V_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  348) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'348',5),'MD REP EVO BP_V_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  349) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'349',5),'MD REP EVO BPW_V_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  350) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'350',5),'MD REP EVO BH_V_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  351) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'351',5),'MD REP EVO BHW_V_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  352) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'352',5),'MD REP EVO BM_V_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  353) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'353',5),'MD REP EVO BMW_V_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  354) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'354',5),'MD REP EVO BS_V_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  355) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'355',5),'MD REP EVO BHI_V_2I','Madis Rodbel Soluções de Ponto e Acesso Ltda. (Rodbel)',right(replicate('0',5)+'5',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  356) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'356',5),'REPv8 SAP Card','Milênio 3 Sistemas Eletrônicos Ltda.',right(replicate('0',5)+'46',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  357) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'357',5),'REPv8 SAP Bio','Milênio 3 Sistemas Eletrônicos Ltda.',right(replicate('0',5)+'46',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  358) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'358',5),'REPv8 SAP Prox','Milênio 3 Sistemas Eletrônicos Ltda.',right(replicate('0',5)+'46',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  359) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'359',5),'REPv8 SAP Card+Bio','Milênio 3 Sistemas Eletrônicos Ltda.',right(replicate('0',5)+'46',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  360) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'360',5),'REPv8 SAPCard+Prox','Milênio 3 Sistemas Eletrônicos Ltda.',right(replicate('0',5)+'46',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  361) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'361',5),'REPv8 SAP Bio+Prox','Milênio 3 Sistemas Eletrônicos Ltda.',right(replicate('0',5)+'46',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  362) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'362',5),'REPv8 SAP Card+Bio+Prox','Milênio 3 Sistemas Eletrônicos Ltda.',right(replicate('0',5)+'46',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  363) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'363',5),'INNER REP PLUS','Topdata Sistemas de Automação Ltda.',right(replicate('0',5)+'9',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  364) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'364',5),'INNER REP PLUS','Topdata Sistemas de Automação Ltda.',right(replicate('0',5)+'9',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  365) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'365',5),'INNER REP PLUS BIO PROX','Topdata Sistemas de Automação Ltda.',right(replicate('0',5)+'9',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  366) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'366',5),'INNER REP PLUS BIO BARRAS','Topdata Sistemas de Automação Ltda.',right(replicate('0',5)+'9',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  367) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'367',5),'iDClass Barras','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  368) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'368',5),'iDClass Barras S','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  369) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'369',5),'iDClass Prox','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  370) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'370',5),'iDClass Prox S','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  371) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'371',5),'iDClass Bio','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  372) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'372',5),'iDClass Bio S','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  373) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'373',5),'iDClass Bio Barras','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  374) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'374',5),'iDClass Bio Barras S','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  375) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'375',5),'iDClass Bio Prox','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  376) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'376',5),'iDClass Bio Prox S','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  377) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'377',5),'iDClass Mult','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)
            if ((select count(*) from equipamentohomologado where codigoModelo =  378) = 0)
            insert into equipamentohomologado (codigoModelo, nomeModelo, nomeFabricante, numeroFabricante, identificacaoRelogio, EquipamentoHomologadoInmetro) Values (right(replicate('0',5)+'378',5),'iDClass Mult S','Control id Indústria Comércio de Hardware e Serviços de Tecnologia Ltda. (Control id)',right(replicate('0',5)+'14',5),1, 0)";

        private static readonly string HOMOLOGACAO_INNER_REP_PLUS = @"
            update equipamentohomologado
           set identificacaoRelogio = 21,
	           EquipamentoHomologadoInmetro = 1
         where nomeFabricante like '%top%'
           and nomeModelo like '%INNER REP PLUS%'";

        private static readonly string AUMENTA_CAMPO_SENHA = @"
            ALTER TABLE Funcionario ALTER COLUMN Mob_Senha VARCHAR(500)";

        private static readonly string DROP_TRATAMENTO_BILHETES = @"
            IF OBJECT_ID('[FnGetTratamentoBilhetes]') is not null
	            DROP FUNCTION [dbo].[FnGetTratamentoBilhetes]";

        private static readonly string CREATE_FUNCAO_TRATAMENTO_BILHETES = @"
            CREATE FUNCTION [dbo].[FnGetTratamentoBilhetes](@DataInicial datetime, @DataFinal datetime)
            RETURNS  @rtnTable TABLE 
            (
                DsCodigo varchar(50),
                Data datetime,
	            tratent_1 char(1),
	            tratent_2 char(1),
	            tratent_3 char(1),
	            tratent_4 char(1),
	            tratent_5 char(1),
	            tratent_6 char(1),
	            tratent_7 char(1),
	            tratent_8 char(1),
	            tratsai_1 char(1),
	            tratsai_2 char(1),
	            tratsai_3 char(1),
	            tratsai_4 char(1),
	            tratsai_5 char(1),
	            tratsai_6 char(1),
	            tratsai_7 char(1),
	            tratsai_8 char(1)
    
            )
            AS
            BEGIN
            insert into @rtnTable
            select dscodigo, data,
	               IsNull(E1,'') as E1,  
	               IsNull(E2,'') as E2, 
	               IsNull(E3,'') as E3, 
	               IsNull(E4,'') as E4, 
	               IsNull(E5,'') as E5, 
	               IsNull(E6,'') as E6, 
	               IsNull(E7,'') as E7, 
	               IsNull(E8,'') as E8, 
	               IsNull(S1,'') as S1,  
	               IsNull(S2,'') as S2, 
	               IsNull(S3,'') as S3, 
	               IsNull(S4,'') as S4, 
	               IsNull(S5,'') as S5, 
	               IsNull(S6,'') as S6, 
	               IsNull(S7,'') as S7, 
	               IsNull(S8,'') as S8
             from (
            select dscodigo, data, 
	               [E1] as E1,  [E2] as E2, [E3] as E3, [E4] as E4, [E5] as E5, [E6] as E6, [E7] as E7, [E8] as E8, 
	               [S1] as S1,  [S2] as S2, [S3] as S3, [S4] as S4, [S5] as S5, [S6] as S6, [S7] as S7, [S8] as S8
              from (
	            SELECT	dscodigo, mar_data data, ent_sai+cast(posicao as varchar) coluna,
			            ocorrencia
	            FROM bilhetesimp AS bie
		            where bie.mar_data >= @datainicial
	              AND bie.mar_data <= @datafinal
	               ) t
            Pivot(
              max(ocorrencia) for coluna in ([E1],  [E2], [E3], [E4], [E5], [E6], [E7], [E8], 
								             [S1],  [S2], [S3], [S4], [S5], [S6], [S7], [S8])) as pvt
				               ) f
            return
            END";

        private static readonly string ALTER_TAMANHO_NUM_REL = @"
            ALTER TABLE MARCACAO ALTER COLUMN ent_num_relogio_1 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN ent_num_relogio_2 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN ent_num_relogio_3 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN ent_num_relogio_4 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN ent_num_relogio_5 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN ent_num_relogio_6 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN ent_num_relogio_7 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN ent_num_relogio_8 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN sai_num_relogio_1 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN sai_num_relogio_2 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN sai_num_relogio_3 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN sai_num_relogio_4 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN sai_num_relogio_5 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN sai_num_relogio_6 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN sai_num_relogio_7 char(3);
            ALTER TABLE MARCACAO ALTER COLUMN sai_num_relogio_8 char(3);

";

        private static readonly string DROP_IMPORTA_MARCACAO = @"
           IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[importa_marcacao]') AND type in (N'P', N'PC'))
            DROP PROCEDURE [dbo].[importa_marcacao]";

        private static readonly string DROP_INSERT_MARCARCAO = @"
            IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[insert_marcacao]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[insert_marcacao]";

        private static readonly string DROP_UPDATE_BILHETE = @"
            IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[update_bilhete]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[update_bilhete]";

        private static readonly string DROP_UPDATE_MARCACAO = @"
            IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[update_marcacao]') AND type in (N'P', N'PC'))
                DROP PROCEDURE [dbo].[update_marcacao]";

        private static readonly string DROP_CREATE_MARCACAO_LOTE = @"
            IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'marcacao_lote' AND ss.name = N'dbo')
            DROP TYPE [dbo].[marcacao_lote]
 
            CREATE TYPE [dbo].[marcacao_lote] AS TABLE(
	            [id] [int] NULL,
	            [codigo] [int] NULL,
	            [idfuncionario] [int] NULL,
	            [dscodigo] [varchar](16) NULL,
	            [legenda] [char](1) NULL,
	            [data] [datetime] NULL,
	            [dia] [varchar](10) NULL,
	            [entradaextra] [varchar](5) NULL,
	            [saidaextra] [varchar](5) NULL,
	            [ocorrencia] [varchar](60) NULL,
	            [idhorario] [int] NULL,
	            [idfechamentobh] [int] NULL,
	            [semcalculo] [int] NULL,
	            [ent_num_relogio_1] [char](3) NULL,
	            [ent_num_relogio_2] [char](3) NULL,
	            [ent_num_relogio_3] [char](3) NULL,
	            [ent_num_relogio_4] [char](3) NULL,
	            [ent_num_relogio_5] [char](3) NULL,
	            [ent_num_relogio_6] [char](3) NULL,
	            [ent_num_relogio_7] [char](3) NULL,
	            [ent_num_relogio_8] [char](3) NULL,
	            [sai_num_relogio_1] [char](3) NULL,
	            [sai_num_relogio_2] [char](3) NULL,
	            [sai_num_relogio_3] [char](3) NULL,
	            [sai_num_relogio_4] [char](3) NULL,
	            [sai_num_relogio_5] [char](3) NULL,
	            [sai_num_relogio_6] [char](3) NULL,
	            [sai_num_relogio_7] [char](3) NULL,
	            [sai_num_relogio_8] [char](3) NULL,
	            [naoentrarbanco] [int] NULL,
	            [naoentrarnacompensacao] [int] NULL,
	            [horascompensadas] [varchar](6) NULL,
	            [idcompensado] [int] NULL,
	            [naoconsiderarcafe] [int] NULL,
	            [dsr] [int] NULL,
	            [abonardsr] [int] NULL,
	            [totalizadoresalterados] [int] NULL,
	            [calchorasextrasdiurna] [int] NULL,
	            [calchorasextranoturna] [int] NULL,
	            [calchorasfaltas] [int] NULL,
	            [calchorasfaltanoturna] [int] NULL,
	            [incdata] [datetime] NULL,
	            [inchora] [datetime] NULL,
	            [incusuario] [varchar](20) NULL,
	            [altdata] [datetime] NULL,
	            [althora] [datetime] NULL,
	            [altusuario] [varchar](20) NULL,
	            [folga] [int] NULL,
	            [neutro] [int] NULL,
	            [totalHorasTrabalhadas] [varchar](6) NULL,
	            [chave] [varchar](255) NULL,
	            [tipohoraextrafalta] [int] NULL,
	            [campo01] [varchar](5) NULL,
	            [campo02] [varchar](5) NULL,
	            [campo03] [varchar](5) NULL,
	            [campo04] [varchar](5) NULL,
	            [campo05] [varchar](5) NULL,
	            [campo06] [varchar](5) NULL,
	            [campo07] [varchar](5) NULL,
	            [campo08] [varchar](5) NULL,
	            [campo09] [varchar](5) NULL,
	            [campo10] [varchar](5) NULL,
	            [campo11] [varchar](5) NULL,
	            [campo12] [varchar](5) NULL,
	            [campo13] [varchar](5) NULL,
	            [campo14] [varchar](5) NULL,
	            [campo15] [varchar](5) NULL,
	            [campo16] [varchar](5) NULL,
	            [campo17] [varchar](5) NULL,
	            [campo18] [varchar](5) NULL,
	            [campo19] [varchar](5) NULL,
	            [campo20] [varchar](5) NULL,
	            [campo21] [varchar](5) NULL,
	            [campo22] [varchar](5) NULL,
	            [campo23] [varchar](6) NULL,
	            [campo24] [varchar](6) NULL,
	            [campo25] [varchar](6) NULL,
	            [campo26] [varchar](5) NULL
            )";

        private static readonly string CREATE_IMPORTA_MARCACAO = @"
            CREATE PROCEDURE [dbo].[importa_marcacao]
            (
                @dados AS dbo.marcacao_lote readonly
            )
            AS
            BEGIN
                SET IDENTITY_INSERT marcacao ON
                INSERT INTO dbo.marcacao 
                    (id
                    ,idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , campo01
                    , campo02
                    , campo03
                    , campo04
                    , campo05
                    , campo06
                    , campo07
                    , campo08
                    , campo09
                    , campo10
                    , campo11
                    , campo12
                    , campo13
                    , campo14
                    , campo15
                    , campo16
                    , campo17
                    , campo18
                    , campo19
                    , entradaextra
                    , saidaextra
                    , campo20
                    , campo21
                    , campo22
                    , ocorrencia
                    , idhorario
                    , campo23
                    , campo24
                    , idfechamentobh
                    , semcalculo
                    , ent_num_relogio_1
                    , ent_num_relogio_2
                    , ent_num_relogio_3
                    , ent_num_relogio_4
                    , ent_num_relogio_5
                    , ent_num_relogio_6
                    , ent_num_relogio_7
                    , ent_num_relogio_8
                    , sai_num_relogio_1
                    , sai_num_relogio_2
                    , sai_num_relogio_3
                    , sai_num_relogio_4
                    , sai_num_relogio_5
                    , sai_num_relogio_6
                    , sai_num_relogio_7
                    , sai_num_relogio_8
                    , naoentrarbanco
                    , naoentrarnacompensacao
                    , horascompensadas
                    , idcompensado
                    , naoconsiderarcafe
                    , dsr
                    , campo25
                    , abonardsr
                    , totalizadoresalterados
                    , calchorasextrasdiurna
                    , calchorasextranoturna
                    , calchorasfaltas
                    , calchorasfaltanoturna
                    , incdata
                    , inchora
                    , incusuario
                    , folga
                    , neutro
                    , totalHorasTrabalhadas
                    , campo26
                    , tipohoraextrafalta
                    , chave)
                SELECT 
                    id
                    ,idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , case when campo01 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo01)) end
                    , case when campo02 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo02)) end
                    , case when campo03 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo03)) end
                    , case when campo04 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo04)) end
                    , case when campo05 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo05)) end
                    , case when campo06 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo06)) end
                    , case when campo07 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo07)) end
                    , case when campo08 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo08)) end
                    , case when campo09 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo09)) end
                    , case when campo10 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo10)) end
                    , case when campo11 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo11)) end
                    , case when campo12 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo12)) end
                    , case when campo13 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo13)) end
                    , case when campo14 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo14)) end
                    , case when campo15 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo15)) end
                    , case when campo16 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo16)) end
                    , case when campo17 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo17)) end
                    , case when campo18 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo18)) end
                    , case when campo19 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo19)) end
                    , entradaextra
                    , saidaextra
                    , case when campo20 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo20)) end
                    , case when campo21 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo21)) end
                    , case when campo22 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo22)) end
                    , ocorrencia
                    , idhorario
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo23))
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo24))
                    , idfechamentobh
                    , semcalculo
                    , ent_num_relogio_1
                    , ent_num_relogio_2
                    , ent_num_relogio_3
                    , ent_num_relogio_4
                    , ent_num_relogio_5
                    , ent_num_relogio_6
                    , ent_num_relogio_7
                    , ent_num_relogio_8
                    , sai_num_relogio_1
                    , sai_num_relogio_2
                    , sai_num_relogio_3
                    , sai_num_relogio_4
                    , sai_num_relogio_5
                    , sai_num_relogio_6
                    , sai_num_relogio_7
                    , sai_num_relogio_8
                    , naoentrarbanco
                    , naoentrarnacompensacao
                    , horascompensadas
                    , idcompensado
                    , naoconsiderarcafe
                    , dsr
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo25))
                    , abonardsr
                    , totalizadoresalterados
                    , calchorasextrasdiurna
                    , calchorasextranoturna
                    , calchorasfaltas
                    , calchorasfaltanoturna
                    , incdata
                    , inchora
                    , incusuario
                    , folga
                    , neutro
                    , totalHorasTrabalhadas
                    , case when campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo26)) end
                    , tipohoraextrafalta
                    , chave
                    FROM @dados;
                    SET IDENTITY_INSERT marcacao OFF
                  END";

        private static readonly string CREATE_INSERT_MARCACAO = @"
            CREATE PROCEDURE [dbo].[insert_marcacao]
            (
                @dados AS dbo.marcacao_lote readonly
            )
            AS
            BEGIN
                INSERT INTO dbo.marcacao 
                    (idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , campo01
                    , campo02
                    , campo03
                    , campo04
                    , campo05
                    , campo06
                    , campo07
                    , campo08
                    , campo09
                    , campo10
                    , campo11
                    , campo12
                    , campo13
                    , campo14
                    , campo15
                    , campo16
                    , campo17
                    , campo18
                    , campo19
                    , entradaextra
                    , saidaextra
                    , campo20
                    , campo21
                    , campo22
                    , ocorrencia
                    , idhorario
                    , campo23
                    , campo24
                    , idfechamentobh
                    , semcalculo
                    , ent_num_relogio_1
                    , ent_num_relogio_2
                    , ent_num_relogio_3
                    , ent_num_relogio_4
                    , ent_num_relogio_5
                    , ent_num_relogio_6
                    , ent_num_relogio_7
                    , ent_num_relogio_8
                    , sai_num_relogio_1
                    , sai_num_relogio_2
                    , sai_num_relogio_3
                    , sai_num_relogio_4
                    , sai_num_relogio_5
                    , sai_num_relogio_6
                    , sai_num_relogio_7
                    , sai_num_relogio_8
                    , naoentrarbanco
                    , naoentrarnacompensacao
                    , horascompensadas
                    , idcompensado
                    , naoconsiderarcafe
                    , dsr
                    , campo25
                    , abonardsr
                    , totalizadoresalterados
                    , calchorasextrasdiurna
                    , calchorasextranoturna
                    , calchorasfaltas
                    , calchorasfaltanoturna
                    , incdata
                    , inchora
                    , incusuario
                    , folga
                    , neutro
                    , totalHorasTrabalhadas
                    , campo26
                    , tipohoraextrafalta
                    , chave)
                SELECT 
                    idfuncionario
                    , codigo
                    , dscodigo
                    , legenda
                    , data
                    , dia
                    , case when campo01 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo01)) end
                    , case when campo02 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo02)) end
                    , case when campo03 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo03)) end
                    , case when campo04 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo04)) end
                    , case when campo05 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo05)) end
                    , case when campo06 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo06)) end
                    , case when campo07 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo07)) end
                    , case when campo08 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo08)) end
                    , case when campo09 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo09)) end
                    , case when campo10 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo10)) end
                    , case when campo11 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo11)) end
                    , case when campo12 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo12)) end
                    , case when campo13 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo13)) end
                    , case when campo14 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo14)) end
                    , case when campo15 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo15)) end
                    , case when campo16 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo16)) end
                    , case when campo17 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo17)) end
                    , case when campo18 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo18)) end
                    , case when campo19 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo19)) end
                    , entradaextra
                    , saidaextra
                    , case when campo20 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo20)) end
                    , case when campo21 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo21)) end
                    , case when campo22 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo22)) end
                    , ocorrencia
                    , idhorario
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo23))
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo24))
                    , idfechamentobh
                    , semcalculo
                    , ent_num_relogio_1
                    , ent_num_relogio_2
                    , ent_num_relogio_3
                    , ent_num_relogio_4
                    , ent_num_relogio_5
                    , ent_num_relogio_6
                    , ent_num_relogio_7
                    , ent_num_relogio_8
                    , sai_num_relogio_1
                    , sai_num_relogio_2
                    , sai_num_relogio_3
                    , sai_num_relogio_4
                    , sai_num_relogio_5
                    , sai_num_relogio_6
                    , sai_num_relogio_7
                    , sai_num_relogio_8
                    , naoentrarbanco
                    , naoentrarnacompensacao
                    , horascompensadas
                    , idcompensado
                    , naoconsiderarcafe
                    , dsr
                    , encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo25))
                    , abonardsr
                    , totalizadoresalterados
                    , calchorasextrasdiurna
                    , calchorasextranoturna
                    , calchorasfaltas
                    , calchorasfaltanoturna
                    , incdata
                    , inchora
                    , incusuario
                    , folga
                    , neutro
                    , totalHorasTrabalhadas
                    , case when campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar, campo26)) end
                    , tipohoraextrafalta
                    , chave
                    FROM @dados
                   END";

        private static readonly string CREATE_UPDATE_BILHETE = @"
            CREATE PROCEDURE [dbo].[update_bilhete] 
            (
	            @dados AS dbo.bilhete_lote readonly
            )
            AS
            BEGIN
            UPDATE dbo.bilhetesimp
                SET 
		            bilhetesimp.codigo = lote.codigo
		            , bilhetesimp.ordem = lote.ordem
		            , bilhetesimp.data = lote.data
		            , bilhetesimp.hora = lote.hora
		            , bilhetesimp.func = lote.func
		            , bilhetesimp.relogio = lote.relogio
		            , bilhetesimp.importado = lote.importado
		            , bilhetesimp.mar_data = lote.mar_data
		            , bilhetesimp.mar_hora = lote.mar_hora
		            , bilhetesimp.mar_relogio = lote.mar_relogio
		            , bilhetesimp.posicao = lote.posicao
		            , bilhetesimp.ent_sai = lote.ent_sai
		            , bilhetesimp.altdata = lote.altdata
		            , bilhetesimp.althora = lote.althora
		            , bilhetesimp.altusuario = lote.altusuario
		            , bilhetesimp.chave = lote.chave
		            , bilhetesimp.dscodigo = lote.dscodigo
		            , bilhetesimp.ocorrencia = lote.ocorrencia
		            , bilhetesimp.motivo = lote.motivo
		            , bilhetesimp.idjustificativa = lote.idjustificativa
		            , bilhetesimp.nsr = lote.nsr
                FROM dbo.bilhetesimp INNER JOIN @dados AS lote
                ON dbo.bilhetesimp.id = lote.id
               END";

        private static readonly string CREATE_UPDATE_MARCACAO = @"
            CREATE PROCEDURE [dbo].[update_marcacao] 
            (
                @dados AS dbo.marcacao_lote readonly
            )
            AS
            BEGIN
            UPDATE dbo.marcacao
                SET 
                marcacao.idfuncionario = lote.idfuncionario
                , marcacao.dscodigo = lote.dscodigo
                , marcacao.codigo = lote.codigo
                , marcacao.legenda = lote.legenda
                , marcacao.data = lote.data
                , marcacao.dia = lote.dia
                , marcacao.campo01 = case when lote.campo01 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo01 )) end
                , marcacao.campo02 = case when lote.campo02 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo02 )) end
                , marcacao.campo03 = case when lote.campo03 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo03 )) end
                , marcacao.campo04 = case when lote.campo04 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo04 )) end
                , marcacao.campo05 = case when lote.campo05 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo05 )) end
                , marcacao.campo06 = case when lote.campo06 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo06 )) end
                , marcacao.campo07 = case when lote.campo07 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo07 )) end
                , marcacao.campo08 = case when lote.campo08 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo08 )) end
                , marcacao.campo09 = case when lote.campo09 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo09 )) end
                , marcacao.campo10 = case when lote.campo10 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo10 )) end
                , marcacao.campo11 = case when lote.campo11 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo11 )) end
                , marcacao.campo12 = case when lote.campo12 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo12 )) end
                , marcacao.campo13 = case when lote.campo13 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo13 )) end
                , marcacao.campo14 = case when lote.campo14 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo14 )) end
                , marcacao.campo15 = case when lote.campo15 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo15 )) end
                , marcacao.campo16 = case when lote.campo16 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo16 )) end
                , marcacao.campo17 = case when lote.campo17 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo17 )) end
                , marcacao.campo18 = case when lote.campo18 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo18 )) end
                , marcacao.campo19 = case when lote.campo19 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo19 )) end
                , marcacao.entradaextra = lote.entradaextra
                , marcacao.saidaextra = lote.saidaextra
                , marcacao.campo20 = case when lote.campo20 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo20 )) end
                , marcacao.campo21 = case when lote.campo21 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo21 )) end
                , marcacao.campo22 = case when lote.campo22 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo22 )) end
                , marcacao.ocorrencia = lote.ocorrencia
                , marcacao.idhorario = lote.idhorario
                , marcacao.campo23 = encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo23 ))
                , marcacao.campo24 = encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo24 ))
                , marcacao.idfechamentobh = lote.idfechamentobh
                , marcacao.semcalculo = lote.semcalculo
                , marcacao.ent_num_relogio_1 = lote.ent_num_relogio_1
                , marcacao.ent_num_relogio_2 = lote.ent_num_relogio_2
                , marcacao.ent_num_relogio_3 = lote.ent_num_relogio_3
                , marcacao.ent_num_relogio_4 = lote.ent_num_relogio_4
                , marcacao.ent_num_relogio_5 = lote.ent_num_relogio_5
                , marcacao.ent_num_relogio_6 = lote.ent_num_relogio_6
                , marcacao.ent_num_relogio_7 = lote.ent_num_relogio_7
                , marcacao.ent_num_relogio_8 = lote.ent_num_relogio_8
                , marcacao.sai_num_relogio_1 = lote.sai_num_relogio_1
                , marcacao.sai_num_relogio_2 = lote.sai_num_relogio_2
                , marcacao.sai_num_relogio_3 = lote.sai_num_relogio_3
                , marcacao.sai_num_relogio_4 = lote.sai_num_relogio_4
                , marcacao.sai_num_relogio_5 = lote.sai_num_relogio_5
                , marcacao.sai_num_relogio_6 = lote.sai_num_relogio_6
                , marcacao.sai_num_relogio_7 = lote.sai_num_relogio_7
                , marcacao.sai_num_relogio_8 = lote.sai_num_relogio_8
                , marcacao.naoentrarbanco = lote.naoentrarbanco
                , marcacao.naoentrarnacompensacao = lote.naoentrarnacompensacao
                , marcacao.horascompensadas = lote.horascompensadas
                , marcacao.idcompensado = lote.idcompensado
                , marcacao.naoconsiderarcafe = lote.naoconsiderarcafe
                , marcacao.dsr = lote.dsr
                , marcacao.campo25 = encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo25 ))
                , marcacao.abonardsr = lote.abonardsr
                , marcacao.totalizadoresalterados = lote.totalizadoresalterados
                , marcacao.calchorasextrasdiurna = lote.calchorasextrasdiurna
                , marcacao.calchorasextranoturna = lote.calchorasextranoturna
                , marcacao.calchorasfaltas = lote.calchorasfaltas
                , marcacao.calchorasfaltanoturna = lote.calchorasfaltanoturna
                , marcacao.altdata = lote.altdata
                , marcacao.althora = lote.althora
                , marcacao.altusuario = lote.altusuario
                , marcacao.folga = lote.folga
                , marcacao.neutro = lote.neutro
                , marcacao.totalHorasTrabalhadas = lote.totalHorasTrabalhadas
                , marcacao.campo26 = case when lote.campo26 = '--:--' then null else encryptbykey(key_guid ('PontoMTKey'), convert(varchar,  lote.campo26 )) end
                , marcacao.tipohoraextrafalta = lote.tipohoraextrafalta
                , marcacao.chave = lote.chave
                FROM dbo.marcacao INNER JOIN @dados AS lote
                ON dbo.marcacao.id = lote.id
               END";

        #endregion
    }


}
