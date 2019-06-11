using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace DAL.SQL
{
    public class ConfiguracaoRefeitorio : DAL.SQL.DALBase, DAL.IConfiguracaoRefeitorio
    {
        public ConfiguracaoRefeitorio(DataBase database)
        {
            db = database;
            TABELA = "configuracaorefeitorio";

            SELECTPID = @"SELECT * FROM configuracaorefeitorio WHERE id = @id";

            SELECTALL = @"SELECT * FROM configuracaorefeitorio";

            INSERT = @"     INSERT INTO configuracaorefeitorio 
                                (codigo, tipoconexao, porta, qtdias, portatcp, naopassarduasvezesentrada, somenteumavezentradasaida, naopassarduasvezessaida, entrardiretoonline, intervalopassadasentrada, intervalopassadassaida, cartaomestre, carregabiometria, incdata, inchora, incusuario)
                            VALUES
                                (@codigo, @tipoconexao, @porta, @qtdias, @portatcp, @naopassarduasvezesentrada, @somenteumavezentradasaida, @naopassarduasvezessaida, @entrardiretoonline, @intervalopassadasentrada, @intervalopassadassaida, @cartaomestre, @carregabiometria, @incdata, @inchora, @incusuario)";

            UPDATE = @"     UPDATE configuracaorefeitorio SET
                                  codigo = @codigo
                                , tipoconexao = @tipoconexao
                                , porta = @porta
                                , qtdias = @qtdias
                                , portatcp = @portatcp
                                , naopassarduasvezesentrada = @naopassarduasvezesentrada
                                , somenteumavezentradasaida = @somenteumavezentradasaida
                                , naopassarduasvezessaida = @naopassarduasvezessaida
                                , entrardiretoonline = @entrardiretoonline
                                , intervalopassadasentrada = @intervalopassadasentrada
                                , intervalopassadassaida = @intervalopassadassaida
                                , cartaomestre = @cartaomestre
                                , carregabiometria = @carregabiometria
                                , altdata = @altdata
                                , althora = @althora
                                , altusuario = @altusuario
                            WHERE id = @id";

            DELETE = @"DELETE FROM configuracaorefeitorio WHERE id = @id";
        }

        #region Métodos

        /// <summary>
        /// Preenche um objeto com os dados de um DataReader
        /// </summary>
        /// <param name="dr">DataReader que contém os dados</param>
        /// <param name="obj">Objeto que será preenchido</param>
        /// <returns>Retorna verdadeiro caso a operação seja realizada com sucesso e falso caso contrário</returns>
        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    SetInstanceBase(dr, obj);

                    ((Modelo.ConfiguracaoRefeitorio)obj).Codigo = Convert.ToInt32(dr["codigo"]);
                    ((Modelo.ConfiguracaoRefeitorio)obj).CarregaBiometria = (dr["carregabiometria"] is DBNull ? 0 : Convert.ToInt32(dr["carregabiometria"]));
                    ((Modelo.ConfiguracaoRefeitorio)obj).CartaoMestre = Convert.ToString(dr["cartaomestre"]);
                    ((Modelo.ConfiguracaoRefeitorio)obj).EntrarDiretoOnline = (dr["entrardiretoonline"] is DBNull ? 0 : Convert.ToInt32(dr["entrardiretoonline"]));
                    ((Modelo.ConfiguracaoRefeitorio)obj).IntervaloPassadasEntrada = (dr["intervalopassadasentrada"] is DBNull ? null : (DateTime?)dr["intervalopassadasentrada"]);
                    ((Modelo.ConfiguracaoRefeitorio)obj).IntervaloPassadasSaida = (dr["intervalopassadassaida"] is DBNull ? null : (DateTime?)dr["intervalopassadassaida"]);
                    ((Modelo.ConfiguracaoRefeitorio)obj).NaoPassarDuasVezesEntrada = (dr["naopassarduasvezesentrada"] is DBNull ? 0 : Convert.ToInt32(dr["naopassarduasvezesentrada"]));
                    ((Modelo.ConfiguracaoRefeitorio)obj).NaoPassarDuasVezesSaida = (dr["naopassarduasvezessaida"] is DBNull ? 0 : Convert.ToInt32(dr["naopassarduasvezessaida"]));
                    ((Modelo.ConfiguracaoRefeitorio)obj).Porta = (dr["porta"] is DBNull ? -1 : Convert.ToInt32(dr["porta"]));
                    ((Modelo.ConfiguracaoRefeitorio)obj).PortaTCP = (dr["portatcp"] is DBNull ? 0 : Convert.ToInt32(dr["portatcp"]));
                    ((Modelo.ConfiguracaoRefeitorio)obj).QtDias = (dr["qtdias"] is DBNull ? 0 : Convert.ToInt32(dr["qtdias"]));
                    ((Modelo.ConfiguracaoRefeitorio)obj).SomenteUmaVezEntradaSaida = (dr["somenteumavezentradasaida"] is DBNull ? 0 : Convert.ToInt32(dr["somenteumavezentradasaida"]));
                    ((Modelo.ConfiguracaoRefeitorio)obj).TipoConexao = (dr["tipoconexao"] is DBNull ? -1 : Convert.ToInt32(dr["tipoconexao"]));

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.ConfiguracaoRefeitorio();
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
            }
        }

        /// <summary>
        /// Método que retorna a lista de parâmetros utilizados na inclusão e na alteração
        /// </summary>
        /// <returns>Lista de parâmetros</returns>
        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {                 
                new SqlParameter ("@id", SqlDbType.Int, 4),
                new SqlParameter ("@codigo", SqlDbType.Int, 4),
                new SqlParameter ("@tipoconexao", SqlDbType.Int, 4),
                new SqlParameter ("@porta", SqlDbType.Int, 4),
                new SqlParameter ("@qtdias", SqlDbType.Int, 4),
                new SqlParameter ("@portatcp", SqlDbType.Int, 4),
                new SqlParameter ("@naopassarduasvezesentrada", SqlDbType.Int, 4),
                new SqlParameter ("@somenteumavezentradasaida", SqlDbType.Int, 4),                
                new SqlParameter ("@naopassarduasvezessaida", SqlDbType.Int, 4),
                new SqlParameter ("@entrardiretoonline", SqlDbType.Int, 4),
                new SqlParameter ("@intervalopassadasentrada", SqlDbType.DateTime, 8),
                new SqlParameter ("@intervalopassadassaida", SqlDbType.DateTime, 8),                
                new SqlParameter ("@cartaomestre", SqlDbType.VarChar, 20),
                new SqlParameter ("@carregabiometria", SqlDbType.Int, 4),
                new SqlParameter ("@altdata", SqlDbType.DateTime, 8),
                new SqlParameter ("@althora", SqlDbType.DateTime, 8),
                new SqlParameter ("@altusuario", SqlDbType.VarChar, 20),
                new SqlParameter ("@incdata", SqlDbType.DateTime, 8),
                new SqlParameter ("@inchora", SqlDbType.DateTime, 8),
                new SqlParameter ("@incusuario", SqlDbType.VarChar, 20),

            };
            return parms;
        }

        /// <summary>
        /// Método responsável por atribuir os valores de um objeto à uma lista de parâmetros
        /// </summary>
        /// <param name="parms">Lista de parâmetros que será preenchida</param>
        /// <param name="obj">Objeto que contém os valores</param>
        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            if (obj.Id == 0)
                parms[0].Value = 0;
            else
                parms[0].Value = obj.Id;

            parms[1].Value = ((Modelo.ConfiguracaoRefeitorio)obj).Codigo;
            parms[2].Value = ((Modelo.ConfiguracaoRefeitorio)obj).TipoConexao;
            parms[3].Value = ((Modelo.ConfiguracaoRefeitorio)obj).Porta;
            parms[4].Value = ((Modelo.ConfiguracaoRefeitorio)obj).QtDias;
            parms[5].Value = ((Modelo.ConfiguracaoRefeitorio)obj).PortaTCP;
            parms[6].Value = ((Modelo.ConfiguracaoRefeitorio)obj).NaoPassarDuasVezesEntrada;
            parms[7].Value = ((Modelo.ConfiguracaoRefeitorio)obj).SomenteUmaVezEntradaSaida;
            parms[8].Value = ((Modelo.ConfiguracaoRefeitorio)obj).NaoPassarDuasVezesSaida;
            parms[9].Value = ((Modelo.ConfiguracaoRefeitorio)obj).EntrarDiretoOnline;
            parms[10].Value = ((Modelo.ConfiguracaoRefeitorio)obj).IntervaloPassadasEntrada;
            parms[11].Value = ((Modelo.ConfiguracaoRefeitorio)obj).IntervaloPassadasSaida;
            parms[12].Value = ((Modelo.ConfiguracaoRefeitorio)obj).CartaoMestre;
            parms[13].Value = ((Modelo.ConfiguracaoRefeitorio)obj).CarregaBiometria;
            parms[14].Value = obj.Altdata;
            parms[15].Value = obj.Althora;
            parms[16].Value = obj.Altusuario;
            parms[17].Value = obj.Incdata;
            parms[18].Value = obj.Inchora;
            parms[19].Value = obj.Incusuario;
        }

        /// <summary>
        /// Método que retorna um objeto do banco de dados
        /// </summary>
        /// <param name="id">chave única do registro no banco de dados</param>
        /// <returns>Objeto preenchido</returns>
        public Modelo.ConfiguracaoRefeitorio LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);
            Modelo.ConfiguracaoRefeitorio objConfiguracaoRefeitorio = new Modelo.ConfiguracaoRefeitorio();
            
            try
            {
                SetInstance(dr, objConfiguracaoRefeitorio);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return objConfiguracaoRefeitorio;
        }

        #endregion
    }
}
