using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace DAL.SQL
{
    public class LoteCalculoFuncionario
    {
        private static string INSERT = @"INSERT INTO LoteCalculoFuncionario(DataInicio, DataFim) VALUES (@DataInicio, @DataFim)";
        public Modelo.Cw_Usuario UsuarioLogado { get; set; }
        protected DataBase db { get; set; }

        public LoteCalculoFuncionario(DataBase dataBase)
        {
            db = dataBase;
        }


        public void Adicionar(List<int> idsFuncionarios, Guid idLote)
        {
            var nome = "Identificadores";
            DataTable table = new DataTable(nome);
            table.Columns.Add(nome, typeof(long));
            foreach (long id in idsFuncionarios.Select(s => (long)s))
            {
                table.Rows.Add(id);
            }
            table.SetTypeName("Identificadores");
            using (var connection = new SqlConnection(db.ConnectionString))
            {
                connection.Open();
                connection.Query($@"INSERT INTO LoteCalculoFuncionario(IdLote, IdFuncionario) 
                                            SELECT @IdLote, f.Identificador
                                            FROM @Identificadores f", new { Identificadores = table.AsTableValuedParameter(), IdLote = idLote });
                connection.Close();
            }
        }
    }
}