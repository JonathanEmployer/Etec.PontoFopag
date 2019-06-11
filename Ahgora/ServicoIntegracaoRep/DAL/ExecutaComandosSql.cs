using AutoMapper;
using ServicoIntegracaoRep.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ServicoIntegracaoRep.DAL
{
    public class ExecutaComandosSql
    {
        /// <summary>
        ///     Método para pegar dados direto da central do cliente.
        /// </summary>
        /// <typeparam name="T">Objeto que recebera o resultado</typeparam>
        /// <param name="queryString">Select da Consulta</param>
        /// <returns></returns>
        public static List<T> LerDados<T>(string queryString)
        {
            using (var connection = new SqlConnection(VariaveisGlobais.ConnectionString))
            using (var command = new SqlCommand(queryString, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                    if (reader.HasRows)
                        return Mapper.DynamicMap<IDataReader, List<T>>(reader);
            }
            return null;
        }

        /// <summary>
        /// Método para pegar dados do Banco de dados
        /// </summary>
        /// <typeparam name="T">Objeto que recebera o resultado</typeparam>
        /// <param name="conn">Conexão do banco onde será realizada a consulta</param>
        /// <param name="queryString">Select da Consulta</param>
        /// <returns></returns>
        public static List<T> LerDados<T>(string conn, string queryString)
        {
            try
            {
                using (var connection = new SqlConnection(conn))
                using (var command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                        if (reader.HasRows)
                            return DataReaderMapToList<T>(reader);
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        /// <summary>
        ///     Método para realizar updates em Banco de Dados
        /// </summary>
        /// <param name="conexao">conexão do banco que contém a tabela que receberá o update.</param>
        /// <param name="comando">String com o Sql que contém o comando de update.</param>
        /// <param name="parametros">Parâmetros do comando de update.</param>
        /// <returns></returns>
        public static int ExecutaComando(string conexao, string comando, List<SqlParameter> parametros)
        {
            using (SqlConnection con = new SqlConnection(conexao))
            {
                using (SqlCommand cmd = new SqlCommand(comando, con))
                {
                    if (parametros != null && parametros.Count > 0)
                    {
                        cmd.Parameters.AddRange(parametros.ToArray());
                    }
                    cmd.CommandTimeout = 300;
                    /// Quantidade de Registros que sofreram update.
                    int updated = 0;
                    try
                    {
                        con.Open();
                        updated = cmd.ExecuteNonQuery();
                        return updated;
                    }
                    catch (Exception err)
                    {
                        throw err;
                    }
                }
            }
        }

        /// <summary>
        ///     Método para realizar update direto na central do cliente.
        /// </summary>
        /// <param name="comando">String com o Sql que contém o comando de update.</param>
        /// <param name="parametros">Parâmetros do comando de update.</param>
        /// <returns></returns>
        public static int ExecutaComando(string comando, List<SqlParameter> parametros)
        {
            return ExecutaComando(VariaveisGlobais.ConnectionString, comando, parametros);
        }

        public static int DeletaEnvioDadosRep(string conn, string IdsEnvioDadosRep, string IdsEnvioDadosRepDet)
        {
            string sql = @"delete from EnvioDadosRepDet 
                            where id in (select * from F_RetornaTabelaLista(@ids,','))";

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@ids", IdsEnvioDadosRepDet));
            ExecutaComandosSql.ExecutaComando(conn, sql, parms);

            sql = @"delete from EnvioDadosRep
                     where id in (select * from F_RetornaTabelaLista(@ids,','))
                       and not exists (select * from EnvioDadosRepDet where idEnvioDadosRep in (select * from F_RetornaTabelaLista(@ids,',')))";

            parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@ids", IdsEnvioDadosRep));
            return ExecutaComandosSql.ExecutaComando(conn, sql, parms);

        }

        public static int DeletaEnvioDataHoraHorarioVerao(string conn, string ids)
        {
            string sql = @"delete from envioconfiguracoesdatahora 
                            where id in (select * from F_RetornaTabelaLista(@ids,','))";

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@ids", ids));
            return ExecutaComandosSql.ExecutaComando(conn, sql, parms);
        }


        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            List<string> colunas = dr.GetSchemaTable().Rows
                                     .Cast<DataRow>()
                                     .Select(r => (string)r["ColumnName"])
                                     .ToList();
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (colunas.Contains(prop.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            try
                            {
                                var valor = ChangeType(dr[prop.Name], prop.PropertyType);
                                prop.SetValue(obj, valor, null);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                        }
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            if (conversion.GetTypeInfo().IsEnum)
                return Enum.Parse(conversion, value.ToString());

            return Convert.ChangeType(value, t);
        }
    }
}
