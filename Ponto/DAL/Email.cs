using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;

namespace DAL
{
    /// <summary>
    /// Classe auxiliar para envio de e-mail utilizando o Database E-mail.
    /// </summary>
    public class Email
    {
        private string connectionString;
        public Email(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// Retorna a Situação do E-mail Enviado.
        /// </summary>
        /// <param name="idEmail">Id do e-mail enviado.</param>
        /// <returns></returns>
        public Modelo.Proxy.pxyRetornoEmail VerificaSituacaoEmail(int idEmail)
        {
            Modelo.Proxy.pxyRetornoEmail RetornoEmail = new Modelo.Proxy.pxyRetornoEmail();
            bool enviouEmail = false;
            int tentativa = 0;
            string situacaoEmail = @"SELECT es.mailitem_id idEmail, es.sent_status StatusEnvio, 
		                                            case when es.sent_status = 0 then 'Não Enviado'
			                                             when es.sent_status = 1 then 'Enviado'
			                                             when es.sent_status = 2 then 'Falha no envio'
			                                             when es.sent_status = 3 then 'Tentando Enviar'
			                                             else 'Indefinido' end DescStatusEnvio
                                                   ,l.description DescFalha 
                                              FROM msdb.dbo.sysmail_mailitems es 
                                              left join msdb.dbo.sysmail_faileditems as ef on es.mailitem_id = ef.mailitem_id
                                              left JOIN msdb.dbo.sysmail_event_log AS l ON ef.mailitem_id = l.mailitem_id
                                             where es.mailitem_id = @idemail";
            Thread.Sleep(2000);
            while (enviouEmail == false)
            {
                tentativa++;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(situacaoEmail, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add(new SqlParameter("@idemail", SqlDbType.Int));
                        cmd.Parameters["@idemail"].Value = idEmail;
                        con.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        try
                        {
                            var map = AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyRetornoEmail>();
                            RetornoEmail = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyRetornoEmail>>(reader).FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            throw (ex);
                        }
                        finally
                        {
                            if (!reader.IsClosed)
                            {
                                reader.Close();
                            }
                            reader.Dispose();
                        }
                    }
                }
                if (RetornoEmail.StatusEnvio == 1 || RetornoEmail.StatusEnvio == 2)
                {
                    enviouEmail = true;
                }
                if (tentativa > 1 && enviouEmail == false)
                {
                    Thread.Sleep(3000);
                }
            }
            return RetornoEmail;
        }
        /// <summary>
        /// Envia E-mail de Teste Utilizando as Configuraçõs do Banco do Data Base e-mail
        /// </summary>
        /// <param name="destinatario">E-mail do Destinatário do Teste</param>
        /// <returns></returns>
        public int EnviarEmailTeste(string destinatario)
        {
            int idEmail = 0;
            string envioEmail = @"  DECLARE @nomeBanco varchar(200);
                                            DECLARE @pName varchar(200);
                                            SELECT @nomeBanco = DB_NAME();
                                            set @pName = 'ProfileCW'+@nomeBanco;
                                            EXEC msdb.dbo.sp_send_dbmail 
                                                 @profile_name = @pName, 
                                                 @recipients= @Destinatario,
                                                 @subject = 'E-mail de teste enviado pelo Cwork Ponto Web',
                                                 @body = 'E-mail enviado para testar as configurações do pontofopag para envio de e-mail.',
                                                 @mailitem_id = @idemail OUTPUT;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(envioEmail, con))
                {
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add(new SqlParameter("@idemail", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@Destinatario", SqlDbType.VarChar));

                    cmd.Parameters["@idemail"].Direction = ParameterDirection.Output;
                    cmd.Parameters["@Destinatario"].Value = destinatario;

                    con.Open();
                    cmd.ExecuteNonQuery();  // *** since you don't need the returned data - just call ExecuteNonQuery
                    idEmail = (int)cmd.Parameters["@idemail"].Value;
                }
            }
            return idEmail;
        }
        /// <summary>
        /// Cria/Recria Profiler E-mail do Database E-mail do SQL Server para o banco conectado.
        /// </summary>
        /// <param name="Email">E-mail que enviará as mensagens do sistema</param>
        /// <param name="SMTP">SMTP do servidor de e-mail</param>
        /// <param name="Porta">Porta de envio do servidor de e-mail</param>
        /// <param name="Senha">Senha do e-mail que enviará as mensagens</param>
        /// <param name="ssl">Indica se o e-mail necessita de Autenticação SSL</param>
        public void CriarRecriarProfileEmail(string Email, string SMTP, int Porta, string Senha, int ssl)
        {
            string script = @" DECLARE @nomeBanco varchar(200);
                                            DECLARE @cName varchar(200);
                                            DECLARE @pName varchar(200);
                                            SELECT @nomeBanco = DB_NAME();
                                            set @cName = 'ContaCW'+@nomeBanco;
                                            set @pName = 'ProfileCW'+@nomeBanco;
                                        
                                            BEGIN TRY
                                                execute msdb.dbo.sysmail_delete_profileaccount_sp
                                                @profile_name = @pName,
                                                @account_name = @cName;

                                                EXECUTE msdb.dbo.sysmail_delete_account_sp
                                                @account_name = @cName;

                                                execute msdb.dbo.sysmail_delete_profile_sp
                                                @profile_name = @pName;
                                            END TRY
										    BEGIN CATCH
										    END CATCH; ";
            if (!String.IsNullOrEmpty(Email) && !String.IsNullOrEmpty(SMTP) && !String.IsNullOrEmpty(Senha) && Porta > 0)
            {
                script += @"         
                                            -- Criando uma conta
                                            execute msdb.dbo.sysmail_add_account_sp
                                            @account_name = @cName,
                                            @description = 'Conta utilizada pela aplicação Cwork Ponto para envio de e-mails',
                                            @email_address = @email, -- E-mail responsável pelo envio.
                                            @display_name = 'Cwork Ponto', -- Nome que será exibido para quem receber o e-mail.
                                            @mailserver_name = @smtp, -- smtp do e-mail
                                            @port = @porta, -- número da porta de saída do smtp
                                            @username = @email, -- Nome de usuário do e-mail
                                            @password = @senha, -- senha do e-mail
                                            @enable_ssl =  @ssl -- SSL

                                            --Criando um Perfil
                                            execute msdb.dbo.sysmail_add_profile_sp
                                            @profile_name = @pName,
                                            @description = 'E-mail para envio de informações sobre o ponto';

                                            --Adicionando uma conta em um perfil
                                            execute msdb.dbo.sysmail_add_profileaccount_sp
                                            @profile_name = @pName,
                                            @account_name = @cName,
                                            @sequence_number = 1; ";
            }
            else
            {

            }

            System.Data.SqlClient.SqlParameter[] parametros = new System.Data.SqlClient.SqlParameter[5]
                        {
                            new System.Data.SqlClient.SqlParameter("@email", SqlDbType.VarChar),
                            new System.Data.SqlClient.SqlParameter("@smtp", SqlDbType.VarChar),
                            new System.Data.SqlClient.SqlParameter("@porta", SqlDbType.VarChar),
                            new System.Data.SqlClient.SqlParameter("@senha", SqlDbType.VarChar),
                            new System.Data.SqlClient.SqlParameter("@ssl", SqlDbType.Int),
                        };
            parametros[0].Value = Email;
            parametros[1].Value = SMTP;
            parametros[2].Value = Porta;
            parametros[3].Value = Senha;
            parametros[4].Value = ssl;
            DAL.SQL.DataBase db = new DAL.SQL.DataBase(connectionString);
            
            db.ExecuteNonQueryNoKey(CommandType.Text, script, parametros);
        }
    }
}
