using DAL.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ConversorMultBanco
{
    public class Versao311011
    {
        public static void Converter(DataBase db)
        {
            db.ExecuteNonQuery(CommandType.Text, UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_01, null);
        }

        #region Alters
        private static readonly string UPDATE_TBL_EQUIPAMENTOHOMOLOGADO_01 =
        @"update equipamentohomologado set identificacaoRelogio = 8 where id in (245, 246);";
        
        #endregion
    }
}
