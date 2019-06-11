using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DAL.SQL;

namespace ConversorMultBanco
{
    public class Versao210046
    {
        public static void Converter(DataBase db)
        {
            var validaChave = db.ExecuteScalar(CommandType.Text, tgChaveUnica, null);
            var validaBilhetesImp = db.ExecuteScalar(CommandType.Text, tgBilhetesImp, null);
            var validaMarcacao = db.ExecuteScalar(CommandType.Text, tgMarcacao, null);

            db.ExecuteNonQuery(CommandType.Text, Parametros, null);

            if (validaBilhetesImp!= null)
            {
                if (!validaBilhetesImp.ToString().Contains("TRG_BilhetesImp"))
                {
                    db.ExecuteNonQuery(CommandType.Text, TRG_BilhetesImp, null);
                }
            }
            else
            {
                db.ExecuteNonQuery(CommandType.Text, TRG_BilhetesImp, null);
            }

            if (validaMarcacao != null)
            {
                if (!validaMarcacao.ToString().Contains("TRG_Marcacao"))
                {
                    db.ExecuteNonQuery(CommandType.Text, TRG_Marcacao, null);
                }
            }
            else
            {
                db.ExecuteNonQuery(CommandType.Text, TRG_Marcacao, null);
            }

            if (validaChave != null)
            {
                if (!validaChave.ToString().Contains("Unq_Chave"))
                {
                    db.ExecuteNonQuery(CommandType.Text, Unq_Chave, null);
                }  
            }
            else
            {
                db.ExecuteNonQuery(CommandType.Text, Unq_Chave, null);
            }
           
                     
        }

        #region Scripts

        private const string tgBilhetesImp =
        @"EXEC sp_helptrigger 'dbo.bilhetesimp'";

        private const string tgMarcacao =
        @"EXEC sp_helptrigger 'dbo.marcacao'";

        private const string tgChaveUnica =
        @"select i.name from sys.Tables as t 
        inner join sys.Indexes as i on i.object_id = t.object_id
        where i.name = 'Unq_Chave' and t.name = 'bilhetesimp'";

        private const string Parametros =
        @"IF NOT EXISTS(SELECT tabela.name as tabela, st.* FROM sys.all_columns st
        inner join sys.sysobjects tabela on tabela.id = st.object_id
        WHERE st.name = N'ExportarValorZerado'
        AND tabela.name = N'parametros')
        BEGIN
            ALTER TABLE dbo.parametros ADD
	        ExportarValorZerado bit NULL
        end";

        private const string TRG_BilhetesImp =
        @"CREATE TRIGGER TRG_BilhetesImp
        ON dbo.bilhetesimp
        FOR UPDATE
        AS
          BEGIN
              DECLARE @newImportado INTEGER,
                      @oldImportado INTEGER,
                      @idBilhete    INTEGER;

              IF UPDATE(importado)
                BEGIN
                    SELECT @newImportado = importado
                      FROM INSERTED;

                    SELECT @oldImportado = importado
                      FROM DELETED;

                    IF @newImportado = 0
                       AND @oldImportado = 1
                      BEGIN
                          RAISERROR ('Não é permitido alterar a situação do bilhete importado para não importado!',16,100)
                          RETURN
                      END;
                END;
          END";

        private const string TRG_Marcacao =
       @"CREATE TRIGGER TRG_Marcacao ON dbo.marcacao
        FOR INSERT, UPDATE
        AS
        BEGIN
          OPEN SYMMETRIC KEY PontoMTKey DECRYPTION BY PASSWORD = 'Pc0W10R#m';
          DECLARE @idFuncionario INTEGER, @id INTEGER, 
	            @ent1 varchar(10), @ent2 varchar(10), @ent3 varchar(10), @ent4 varchar(10), @ent5 varchar(10), @ent6 varchar(10), @ent7 varchar(10), @ent8 varchar(10),
                @sai1 varchar(10), @sai2 varchar(10), @sai3 varchar(10), @sai4 varchar(10), @sai5 varchar(10), @sai6 varchar(10), @sai7 varchar(10), @sai8 varchar(10),
	            @msgerror varchar(1000),
	            @data DATETIME;

          

          DECLARE cur_marcacao CURSOR
          LOCAL FAST_FORWARD READ_ONLY FOR
            SELECT  data, idfuncionario,
                        CONVERT(VARCHAR, Decryptbykey(campo01)) ent1,
                        CONVERT(VARCHAR, Decryptbykey(campo02)) ent2,
                        CONVERT(VARCHAR, Decryptbykey(campo03)) ent3,
                        CONVERT(VARCHAR, Decryptbykey(campo04)) ent4,
                        CONVERT(VARCHAR, Decryptbykey(campo05)) ent5,
                        CONVERT(VARCHAR, Decryptbykey(campo06)) ent6,
                        CONVERT(VARCHAR, Decryptbykey(campo07)) ent7,
                        CONVERT(VARCHAR, Decryptbykey(campo08)) ent8,
                        CONVERT(VARCHAR, Decryptbykey(campo09)) sai1,
                        CONVERT(VARCHAR, Decryptbykey(campo10)) sai2,
                        CONVERT(VARCHAR, Decryptbykey(campo11)) sai3,
                        CONVERT(VARCHAR, Decryptbykey(campo12)) sai4,
                        CONVERT(VARCHAR, Decryptbykey(campo13)) sai5,
                        CONVERT(VARCHAR, Decryptbykey(campo14)) sai6,
                        CONVERT(VARCHAR, Decryptbykey(campo15)) sai7,
                        CONVERT(VARCHAR, Decryptbykey(campo16)) sai8
            FROM INSERTED
          OPEN cur_marcacao

          while 1 = 1
          begin
            fetch next from cur_marcacao into @data, @idFuncionario, @ent1 , @ent2 , @ent3 , @ent4 , @ent5 , @ent6 , @ent7 , @ent8 ,
                @sai1 , @sai2 , @sai3 , @sai4 , @sai5 , @sai6 , @sai7 , @sai8 
            
            if @@fetch_status <> 0 break
         
            IF Isnull(@ent1, 1) = Isnull(@ent2, 2)
            OR Isnull(@ent1, 1) = Isnull(@ent3, 3)
            OR Isnull(@ent1, 1) = Isnull(@ent4, 4)
            OR Isnull(@ent1, 1) = Isnull(@sai1, 9)
            OR Isnull(@ent1, 1) = Isnull(@sai2, 10)
            OR Isnull(@ent1, 1) = Isnull(@sai3, 11)
            OR Isnull(@ent1, 1) = Isnull(@sai4, 12)
            OR Isnull(@ent2, 2) = Isnull(@ent3, 3)
            OR Isnull(@ent2, 2) = Isnull(@ent4, 4)
            OR Isnull(@ent2, 2) = Isnull(@sai1, 9)
            OR Isnull(@ent2, 2) = Isnull(@sai2, 10)
            OR Isnull(@ent2, 2) = Isnull(@sai3, 11)
            OR Isnull(@ent2, 2) = Isnull(@sai4, 12)
            OR Isnull(@ent3, 3) = Isnull(@ent4, 4)
            OR Isnull(@ent3, 3) = Isnull(@sai1, 9)
            OR Isnull(@ent3, 3) = Isnull(@sai2, 10)
            OR Isnull(@ent3, 3) = Isnull(@sai3, 11)
            OR Isnull(@ent3, 3) = Isnull(@sai4, 12)
            OR Isnull(@ent4, 4) = Isnull(@sai1, 9)
            OR Isnull(@ent4, 4) = Isnull(@sai2, 10)
            OR Isnull(@ent4, 4) = Isnull(@sai3, 11)
            OR Isnull(@ent4, 4) = Isnull(@sai4, 12)
            OR Isnull(@sai1, 9) = Isnull(@sai2, 10)
            OR Isnull(@sai1, 9) = Isnull(@sai3, 11)
            OR Isnull(@sai1, 9) = Isnull(@sai4, 12)
            OR Isnull(@sai1, 9) = Isnull(@sai5, 13)
            OR Isnull(@sai1, 9) = Isnull(@sai6, 14)
            OR Isnull(@sai1, 9) = Isnull(@sai7, 15)
            OR Isnull(@sai1, 9) = Isnull(@sai8, 16)
            OR Isnull(@sai2, 10) = Isnull(@sai3, 11)
            OR Isnull(@sai2, 10) = Isnull(@sai4, 12)
            OR Isnull(@sai2, 10) = Isnull(@sai5, 13)
            OR Isnull(@sai2, 10) = Isnull(@sai6, 14)
            OR Isnull(@sai2, 10) = Isnull(@sai7, 15)
            OR Isnull(@sai2, 10) = Isnull(@sai8, 16)
            OR Isnull(@sai3, 11) = Isnull(@sai4, 12)
            OR Isnull(@sai3, 11) = Isnull(@sai5, 13)
            OR Isnull(@sai3, 11) = Isnull(@sai6, 14)
            OR Isnull(@sai3, 11) = Isnull(@sai7, 15)
            OR Isnull(@sai3, 11) = Isnull(@sai8, 16)
            OR Isnull(@sai4, 12) = Isnull(@sai5, 13)
            OR Isnull(@sai4, 12) = Isnull(@sai6, 14)
            OR Isnull(@sai4, 12) = Isnull(@sai7, 15)
            OR Isnull(@sai4, 12) = Isnull(@sai8, 16)
            OR Isnull(@sai5, 13) = Isnull(@sai6, 14)
            OR Isnull(@sai5, 13) = Isnull(@sai7, 15)
            OR Isnull(@sai5, 13) = Isnull(@sai8, 16)
            OR Isnull(@sai6, 14) = Isnull(@sai7, 15)
            OR Isnull(@sai6, 14) = Isnull(@sai8, 16)
            OR Isnull(@sai7, 15) = Isnull(@sai8, 16)
            BEGIN
              SET @msgerror = 'Problemas ao gerar marcações, existem duplicidades. Por favor verifique se existe outro processo sendo executado, caso existe aguarde o termino e tente novamente! Erro encontrado em: id funcionario: '+ CONVERT(VARCHAR,@idFuncionario)+' Data: '+CONVERT(VARCHAR,@data) +
				              'Entradas: '+ISNULL(@ent1,'')+', '+ISNULL(@ent2,'')+', '+ISNULL(@ent3,'')+', '+ISNULL(@ent4,'')+', '+ISNULL(@ent5,'')+', '+ISNULL(@ent6,'')+', '+ISNULL(@ent7,'')+', '+ISNULL(@ent8,'')+
                              ' Saidas :'+ISNULL(@sai1,'')+', '+ISNULL(@sai2,'')+', '+ISNULL(@sai3,'')+', '+ISNULL(@sai4,'')+', '+ISNULL(@sai5,'')+', '+ISNULL(@sai6,'')+', '+ISNULL(@sai7,'')+', '+ISNULL(@sai8,'');

              close cur_marcacao
              deallocate cur_marcacao   
        	  
              RAISERROR (@msgerror, 14, 100)
              RETURN
            END
             
             
          end
          close cur_marcacao
          deallocate cur_marcacao      
        END";

        private const string Unq_Chave =
       @"CREATE UNIQUE INDEX Unq_Chave ON dbo.bilhetesimp (chave)";

        #endregion
    }
}
