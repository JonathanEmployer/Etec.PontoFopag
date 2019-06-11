using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using DAL.SQL;

namespace BLL
{
    public class ImportacaoSecullum
    {
        private DAL.IImportacaoSecullum dalImportacaoSecullum;
        private string ConnectionString;
        public ImportacaoSecullum() : this(null)
        {
            
        }

        public ImportacaoSecullum(string connString)
        {
            if (!String.IsNullOrEmpty(connString))
            {
                ConnectionString = connString;
            }
            else
            {
                ConnectionString = Modelo.cwkGlobal.CONN_STRING;
            }
            dalImportacaoSecullum = new DAL.SQL.ImportacaoSecullum(new DataBase(ConnectionString));
        }

        public Hashtable GetHashDepartamento()
        {
            return dalImportacaoSecullum.GetHashDepartamento();
        }

        public Hashtable GetHashFuncao()
        {
            return dalImportacaoSecullum.GetHashFuncao();
        }

        public Hashtable GetHashFuncionario()
        {
            return dalImportacaoSecullum.GetHashFuncionario();
        }
    }
}
