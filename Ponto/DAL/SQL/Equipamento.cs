using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace DAL.SQL
{
    public class Equipamento : DAL.SQL.DALBase, DAL.IEquipamento
    {
        public string SELECTATIVO { get; set; }

        public Equipamento(DataBase database)
        {
            db = database;
            TABELA = "equipamento";

            SELECTPID = @"SELECT * FROM equipamento WHERE id = @id";

            SELECTALL = @"SELECT equipamento.id, equipamento.descricao, equipamento.codigo FROM equipamento";

            INSERT = @"     INSERT INTO equipamento 
                                (codigo        , descricao      , mensagempadrao, entrada            , saida              , listaacesso    ,
                                 ativaonline   , mostrardatahora, utilizacatraca, tipocartao         , numinner           , aceitatecladoon, ecoteclado   , tipoleitoron   ,
                                 operaleitor1on, acionamento1on , acionamento2on, tempoaciona1on, tempoaciona2on, numerodigitoson, codempmenoson, nivelcontroleon,
                                 formasentradas, tempomaximo    , posicaocursor , totaldigitos       , incdata            , inchora        , incusuario)
                            VALUES
                                (@codigo        , @descricao      , @mensagempadrao, @entrada            , @saida              , @listaacesso    ,
                                 @ativaonline   , @mostrardatahora, @utilizacatraca, @tipocartao         , @numinner           , @aceitatecladoon, @ecoteclado   , @tipoleitoron   ,
                                 @operaleitor1on, @acionamento1on , @acionamento2on, @tempoaciona1on, @tempoaciona2on, @numerodigitoson, @codempmenoson, @nivelcontroleon,
                                 @formasentradas, @tempomaximo    , @posicaocursor , @totaldigitos       , @incdata            , @inchora        , @incusuario)
                            SET @id = SCOPE_IDENTITY()";

            UPDATE = @"     UPDATE equipamento SET
                                  codigo = @codigo
                                , descricao = @descricao
                                , mensagempadrao = @mensagempadrao    
                                , entrada = @entrada       
                                , saida = @saida
                                , listaacesso = @listaacesso
                                , ativaonline = @ativaonline
                                , mostrardatahora = @mostrardatahora
                                , utilizacatraca = @utilizacatraca
                                , tipocartao = @tipocartao
                                , numinner = @numinner
                                , aceitatecladoon = @aceitatecladoon
                                , ecoteclado = @ecoteclado
                                , tipoleitoron = @tipoleitoron
                                , operaleitor1on = @operaleitor1on
                                , acionamento1on = @acionamento1on
                                , acionamento2on = @acionamento2on
                                , tempoaciona1on = @tempoaciona1on
                                , tempoaciona2on = @tempoaciona2on
                                , numerodigitoson = @numerodigitoson
                                , codempmenoson = @codempmenoson
                                , nivelcontroleon = @nivelcontroleon
                                , formasentradas = @formasentradas
                                , tempomaximo = @tempomaximo
                                , posicaocursor = @posicaocursor
                                , totaldigitos = @totaldigitos
                                , altdata = @altdata
                                , althora = @althora
                                , altusuario = @altusuario
                            WHERE id = @id";

            DELETE = @"DELETE FROM equipamento WHERE id = @id";

            MAXCOD = @"SELECT MAX(codigo) AS codigo FROM equipamento";

            SELECTATIVO = @"SELECT id FROM equipamento WHERE ISNULL(AtivaOnLine,0) = 1";
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

                    ((Modelo.Equipamento)obj).Codigo = Convert.ToInt32(dr["codigo"]);

                    ((Modelo.Equipamento)obj).AceitaTecladoOn = Convert.ToInt32(dr["aceitatecladoon"]);
                    ((Modelo.Equipamento)obj).Acionamento1On = Convert.ToInt32(dr["acionamento1on"]);
                    ((Modelo.Equipamento)obj).Acionamento2On = Convert.ToInt32(dr["acionamento2on"]);
                    ((Modelo.Equipamento)obj).AtivaOnLine = Convert.ToInt32(dr["ativaonline"]);
                    ((Modelo.Equipamento)obj).CodEmpMenosOn = Convert.ToByte(dr["codempmenoson"]);

                    ((Modelo.Equipamento)obj).Descricao = Convert.ToString(dr["descricao"]);
                    ((Modelo.Equipamento)obj).EcoTeclado = Convert.ToInt32(dr["ecoteclado"]);
                    ((Modelo.Equipamento)obj).Entrada = Convert.ToString(dr["entrada"]);
                    ((Modelo.Equipamento)obj).FormasEntradas = Convert.ToInt32(dr["formasentradas"]);
                    ((Modelo.Equipamento)obj).ListaAcesso = Convert.ToInt32(dr["listaacesso"]);
                    ((Modelo.Equipamento)obj).MensagemPadrao = Convert.ToString(dr["mensagempadrao"]);
                    ((Modelo.Equipamento)obj).MostrarDataHora = Convert.ToInt32(dr["mostrardatahora"]);
                    ((Modelo.Equipamento)obj).NivelControleOn = Convert.ToByte(dr["nivelcontroleon"]);
                    ((Modelo.Equipamento)obj).NumeroDigitosOn = Convert.ToByte(dr["numerodigitoson"]);
                    ((Modelo.Equipamento)obj).NumInner = Convert.ToByte(dr["numinner"]);
                    ((Modelo.Equipamento)obj).OperaLeitor1On = Convert.ToInt32(dr["operaleitor1on"]);
                    ((Modelo.Equipamento)obj).PosicaoCursor = Convert.ToByte(dr["posicaocursor"]);
                    ((Modelo.Equipamento)obj).Saida = Convert.ToString(dr["saida"]);
                    ((Modelo.Equipamento)obj).TempoAciona1On = Convert.ToByte(dr["tempoaciona1on"]);
                    ((Modelo.Equipamento)obj).TempoAciona2On = Convert.ToByte(dr["tempoaciona2on"]);
                    ((Modelo.Equipamento)obj).TempoMaximo = Convert.ToByte(dr["tempomaximo"]);
                    ((Modelo.Equipamento)obj).TipoCartao = Convert.ToInt32(dr["tipocartao"]);
                    ((Modelo.Equipamento)obj).TipoLeitorOn = Convert.ToInt32(dr["tipoleitoron"]);
                    ((Modelo.Equipamento)obj).TotalDigitos = Convert.ToByte(dr["totaldigitos"]);
                    ((Modelo.Equipamento)obj).UtilizaCatraca = Convert.ToInt32(dr["utilizacatraca"]);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Equipamento();
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
                new SqlParameter ("@descricao", SqlDbType.VarChar, 60),
                new SqlParameter ("@mensagempadrao", SqlDbType.VarChar, 80),   
                new SqlParameter ("@entrada", SqlDbType.VarChar, 80),
                new SqlParameter ("@saida", SqlDbType.VarChar, 80),
                new SqlParameter ("@listaacesso", SqlDbType.TinyInt, 1),
                new SqlParameter ("@ativaonline", SqlDbType.TinyInt, 1),
                new SqlParameter ("@mostrardatahora", SqlDbType.TinyInt, 1),
                new SqlParameter ("@utilizacatraca", SqlDbType.TinyInt, 1),
                new SqlParameter ("@tipocartao", SqlDbType.SmallInt, 2),
                new SqlParameter ("@numinner", SqlDbType.SmallInt, 2),
                new SqlParameter ("@aceitatecladoon", SqlDbType.TinyInt, 1),
                new SqlParameter ("@ecoteclado", SqlDbType.TinyInt, 1),
                new SqlParameter ("@tipoleitoron", SqlDbType.SmallInt, 2),
                new SqlParameter ("@operaleitor1on", SqlDbType.SmallInt, 2),
                new SqlParameter ("@acionamento1on", SqlDbType.SmallInt, 2),
                new SqlParameter ("@acionamento2on", SqlDbType.SmallInt, 2),
                new SqlParameter ("@tempoaciona1on", SqlDbType.SmallInt, 2),
                new SqlParameter ("@tempoaciona2on", SqlDbType.SmallInt, 2),
                new SqlParameter ("@numerodigitoson", SqlDbType.SmallInt, 2),
                new SqlParameter ("@codempmenoson", SqlDbType.SmallInt, 2),
                new SqlParameter ("@nivelcontroleon", SqlDbType.SmallInt, 2),
                new SqlParameter ("@formasentradas", SqlDbType.SmallInt, 2),
                new SqlParameter ("@tempomaximo", SqlDbType.SmallInt, 2),
                new SqlParameter ("@posicaocursor", SqlDbType.SmallInt, 2),
                new SqlParameter ("@totaldigitos", SqlDbType.SmallInt, 2),
                new SqlParameter ("@altdata", SqlDbType.DateTime, 8),
                new SqlParameter ("@althora", SqlDbType.DateTime, 8),
                new SqlParameter ("@altusuario", SqlDbType.VarChar, 20),
                new SqlParameter ("@incdata", SqlDbType.DateTime, 8),
                new SqlParameter ("@inchora", SqlDbType.DateTime, 8),
                new SqlParameter ("@incusuario", SqlDbType.VarChar, 20)
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
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }

            parms[1].Value = ((Modelo.Equipamento)obj).Codigo;
            parms[2].Value = ((Modelo.Equipamento)obj).Descricao;
            parms[3].Value = ((Modelo.Equipamento)obj).MensagemPadrao;
            parms[4].Value = ((Modelo.Equipamento)obj).Entrada;
            parms[5].Value = ((Modelo.Equipamento)obj).Saida;
            parms[6].Value = ((Modelo.Equipamento)obj).ListaAcesso;
            parms[7].Value = ((Modelo.Equipamento)obj).AtivaOnLine;
            parms[8].Value = ((Modelo.Equipamento)obj).MostrarDataHora;
            parms[9].Value = ((Modelo.Equipamento)obj).UtilizaCatraca;
            parms[10].Value = ((Modelo.Equipamento)obj).TipoCartao;
            parms[11].Value = ((Modelo.Equipamento)obj).NumInner;
            parms[12].Value = ((Modelo.Equipamento)obj).AceitaTecladoOn;
            parms[13].Value = ((Modelo.Equipamento)obj).EcoTeclado;
            parms[14].Value = ((Modelo.Equipamento)obj).TipoLeitorOn;
            parms[15].Value = ((Modelo.Equipamento)obj).OperaLeitor1On;
            parms[16].Value = ((Modelo.Equipamento)obj).Acionamento1On;
            parms[17].Value = ((Modelo.Equipamento)obj).Acionamento2On;
            parms[18].Value = ((Modelo.Equipamento)obj).TempoAciona1On;
            parms[19].Value = ((Modelo.Equipamento)obj).TempoAciona2On;
            parms[20].Value = ((Modelo.Equipamento)obj).NumeroDigitosOn;
            parms[21].Value = ((Modelo.Equipamento)obj).CodEmpMenosOn;
            parms[22].Value = ((Modelo.Equipamento)obj).NivelControleOn;
            parms[23].Value = ((Modelo.Equipamento)obj).FormasEntradas;
            parms[24].Value = ((Modelo.Equipamento)obj).TempoMaximo;
            parms[25].Value = ((Modelo.Equipamento)obj).PosicaoCursor;
            parms[26].Value = ((Modelo.Equipamento)obj).TotalDigitos;

            parms[27].Value = obj.Altdata;
            parms[28].Value = obj.Althora;
            parms[29].Value = obj.Altusuario;
            parms[30].Value = obj.Incdata;
            parms[31].Value = obj.Inchora;
            parms[32].Value = obj.Incusuario;
        }

        /// <summary>
        /// Método que retorna um objeto do banco de dados
        /// </summary>
        /// <param name="id">chave única do registro no banco de dados</param>
        /// <returns>Objeto preenchido</returns>
        public Modelo.Equipamento LoadObject(int id)
        {
            SqlDataReader dr = LoadDataReader(id);
            Modelo.Equipamento objEquipamento = new Modelo.Equipamento();
            try
            {
                SetInstance(dr, objEquipamento);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objEquipamento;
        }

        public DataTable GetEquipamentoAtivo()
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTATIVO, parms);
            dt.Load(dr);
            if (dr.IsClosed)
            {
                dr.Close();
            }
            dr.Dispose();
            return dt;
        }

        public bool BuscaInner(int pInner, int pId)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@numinner", SqlDbType.Int),
                new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = pInner;
            parms[1].Value = pId;

            string cmd = @" SELECT COUNT (numinner) 
                            FROM equipamento
                            WHERE numinner = @numinner AND id <> @id";

            int valor = (int)db.ExecuteScalar(CommandType.Text, cmd, parms);

            return valor == 0 ? false : true;

        }

        #endregion
    }
}
