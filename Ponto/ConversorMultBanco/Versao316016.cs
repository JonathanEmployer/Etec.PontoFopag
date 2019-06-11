using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao316016
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_01, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_02, null);
            db.ExecuteNonQuery(CommandType.Text, ALTER_TBL_03, null);
            db.ExecuteNonQueryNoKey(CommandType.Text, INSERT_EQUIPAMENTO_HOMOLOGADO_HEXA, null);
        }

        #region ALTERS
        private static readonly string ALTER_TBL_01 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'SenhaRep' AND tabela.name = N'cw_usuario')
                    BEGIN
                        ALTER TABLE dbo.cw_usuario ADD
                            SenhaRep varchar(255) NULL
                    END";

        private static readonly string ALTER_TBL_02 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'LoginRep' AND tabela.name = N'cw_usuario')
                    BEGIN
                        ALTER TABLE dbo.cw_usuario ADD
                            LoginRep varchar(255) NULL
                    END";

        private static readonly string ALTER_TBL_03 = @"
            IF NOT EXISTS (	SELECT tabela.name as tabela, st.* FROM sys.all_columns st
                            inner join sys.sysobjects tabela on tabela.id = st.object_id 
                            WHERE st.name = N'EquipamentoHomologadoInmetro' AND tabela.name = N'equipamentohomologado')
                    BEGIN
                        ALTER TABLE dbo.equipamentohomologado ADD
                            EquipamentoHomologadoInmetro bit NOT NULL CONSTRAINT DF_equipamentohomologado_EquipHomologadoInmetro DEFAULT ((0))
                    END";

        private static readonly string INSERT_EQUIPAMENTO_HOMOLOGADO_HEXA = @"
            if not((select count(id) from equipamentohomologado where id = 248) != 0)
            begin
	            set identity_insert dbo.equipamentohomologado on;
	            INSERT INTO [dbo].[equipamentohomologado]
				            ([id]
				            ,[codigoModelo]
				            ,[nomeModelo]
				            ,[nomeFabricante]
				            ,[numeroFabricante]
				            ,[identificacaoRelogio]
				            ,[incdata]
				            ,[inchora]
				            ,[incusuario]
                            ,[EquipamentoHomologadoInmetro])
			            VALUES
				            (248
				            ,'00252'
				            ,'HEXA C'
				            ,'Henry Equipamentos Eletrônicos e Sistemas Ltda.'
				            ,'00004'
				            ,12
				            ,'2015-06-06 00:00:00.000'
				            ,'2015-06-06 00:00:00.000'
				            ,'cwork'
                            ,1);
	            set identity_insert dbo.equipamentohomologado off;
            end
            else
            begin
	            UPDATE equipamentohomologado set EquipamentoHomologadoInmetro = 1 where id = 248
            end";

        #endregion
    }
}
