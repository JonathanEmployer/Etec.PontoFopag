using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace MonitorJobs.Negocio
{
    public class EnviarRegistroPonto
    {
        [DisplayName("Enviar Registros Ponto - CS: {0}")]
        public static void EnviarRegistroPontoCS(string database)
        {
            var conn = BLL.cwkFuncoes.ConstroiConexao(database);
            Modelo.Cw_Usuario user = new Modelo.Cw_Usuario() { Nome = "ServGeraMarcacao", Login = "ServGeraMarcacao" };
            List<DadosEnviarSMS> enviar = CarregarDados(conn);
            if (enviar.Count > 0)
            {
                EnviarSMSPonteAzul(enviar);
                ConfirmaEnvioBilheteImp(enviar, conn.ConnectionString); 
            }
        }

        private static List<DadosEnviarSMS> CarregarDados(SqlConnectionStringBuilder conn)
        {
            #region Consulta
            string sql = @"SELECT t.*, f.nome, hd.entrada_1, hd.entrada_2, hd.entrada_3, hd.entrada_4, hd.saida_1, hd.saida_2, hd.saida_3, hd.saida_4
                                FROM (
	                            SELECT b.id idBilhete, BFA.telefone, b.IdFuncionario, b.hora, b.ent_sai, b.posicao, b.data
	                                FROM dbo.bilhetesimp b
	                            INNER JOIN BilhetesImpFuncionariosEnviarAviso BFA ON b.idfuncionario = bfa.idfuncionario
	                            WHERE NOT EXISTS (SELECT top(1) id FROM dbo.BilhetesImpEnvioAviso biea WHERE biea.idbilhetesimp = b.id)
	                                AND b.id > 4690178
	                                AND b.incdata >= Convert(date, getdate())
	                                AND b.importado = 1
	                                AND b.data = Convert(date, getdate())
                                ) t
                                INNER JOIN dbo.funcionario f on f.id = t.idfuncionario
                                INNER JOIN marcacao m ON t.data = m.data AND m.idfuncionario = f.id
                                INNER JOIN horario h ON h.id = m.idhorario 
                                LEFT JOIN horariodetalhe hd ON hd.idhorario = m.idhorario 
                                    AND ((h.tipohorario = 1 AND hd.dia = (CASE WHEN (CAST(DATEPART(WEEKDAY, m.data) AS INT) - 1) = 0 THEN 7 ELSE (CAST(DATEPART(WEEKDAY, m.data) AS INT) - 1) END) ) OR
				                            (h.tipohorario = 2 AND hd.data = m.data)
			                            )";
            #endregion
            #region Busca Dados
            List<DadosEnviarSMS> enviar = new List<DadosEnviarSMS>();
            using (SqlConnection connection = new SqlConnection(conn.ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                try
                {
                    connection.Open();
                    SqlDataReader dr = command.ExecuteReader();
                    AutoMapper.Mapper.CreateMap<IDataReader, DadosEnviarSMS>();
                    enviar = AutoMapper.Mapper.Map<List<DadosEnviarSMS>>(dr);
                    dr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            #endregion
            return enviar;
        }

        private static void EnviarSMSPonteAzul(List<DadosEnviarSMS> enviar)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"data source=empvw0308\prdst;initial catalog=SERVICO_EMAIL_PRD;user id=ponteazul_app;password=eoint2567!";
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter("SELECT top(1) * FROM tab_sms WHERE 1 = 0", con);
            SqlCommandBuilder cmdbl = new SqlCommandBuilder(ad);
            DataSet ds = new DataSet("detail1");
            ad.Fill(ds, "detail1");

            foreach (var item in enviar)
            {
                DataRow row = ds.Tables["detail1"].NewRow();
                row["Num_Sistema"] = 1;
                row["Num_Celular"] = item.Telefone;
                row["Des_Mensagem"] = item.Mensagem;
                row["Dta_Cadastro"] = DateTime.Now;
                row["Idf_Remetente_Sms"] = 1;
                row["Idf_Status_Sms"] = 1;
                row["Idf_Origem"] = 2;
                ds.Tables["detail1"].Rows.Add(row);
            }

            ad.Update(ds, "detail1");
            con.Close();
        }

        private static void ConfirmaEnvioBilheteImp(List<DadosEnviarSMS> enviar, string conn)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = conn;
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter("SELECT top(1)* FROM BilhetesImpEnvioAviso WHERE 1 = 0", con);
            SqlCommandBuilder cmdbl = new SqlCommandBuilder(ad);
            DataSet ds = new DataSet("detail1");
            ad.Fill(ds, "detail1");

            foreach (var item in enviar)
            {
                DataRow row = ds.Tables["detail1"].NewRow();
                row["codigo"] = 1;
                row["incdata"] = DateTime.Now.Date;
                row["inchora"] = DateTime.Now;
                row["incusuario"] = "ServImportacao";
                row["idBilhetesImp"] = item.IdBilhete;
                ds.Tables["detail1"].Rows.Add(row);
            }

            ad.Update(ds, "detail1");
            con.Close();
        }
    }

    public class DadosEnviarSMS
    {
        public int IdBilhete { get; set; }
        public Int64 Telefone { get; set; }
        public int IdFuncionario { get; set; }
        public string Hora { get; set; }
        public string Ent_sai { get; set; }
        public short Posicao { get; set; }
        public DateTime Data { get; set; }
        public string Nome { get; set; }
        public string Entrada_1 { get; set; }
        public string Entrada_2 { get; set; }
        public string Entrada_3 { get; set; }
        public string Entrada_4 { get; set; }
        public string Saida_1 { get; set; }
        public string Saida_2 { get; set; }
        public string Saida_3 { get; set; }
        public string Saida_4 { get; set; }
        public string Mensagem { get {
                switch (Posicao)
                {
                    case 1:
                        if (Ent_sai == "E")
                        {
                            return Nome.Split(' ')[0] + ", Ponto " + Hora + " registrado, Bom trabalho! já tomou seu café hoje?";
                        }
                        else
                        {
                            return Nome.Split(' ')[0] + ", Ponto " + Hora + " registrado, Bom almoço, te esperamos as " + Convert.ToDateTime("13/03/2019 " + Hora).AddHours(1).ToString("HH:mm");
                        }
                    case 2:
                        if (Ent_sai == "E")
                        {
                            return Nome.Split(' ')[0] + ", Ponto " + Hora + " registrado, Bom trabalho! que tal um café?";
                        }
                        else
                        {
                            return Nome.Split(' ')[0] + ", Ponto " + Hora + " registrado, Bom descanso! que tal um Happy Hour!";
                        }
                    default:
                        return "Seu ponto " + Hora + " foi registrado";
                }
            } }
    }
}