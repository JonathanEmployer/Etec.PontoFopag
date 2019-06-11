using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Data;
using DAL.SQL;

namespace ConversorSQL
{
    public class EncriptaSenhas
    {

        public static void AumentarCampoSenha(out bool converterSenhas)
        {
            try
            {
                DataBase db = new DataBase(Modelo.cwkGlobal.CONN_STRING);
                db.ExecuteNonQuery(CommandType.Text, AUMENTA_SENHA_CW_USUARIO, null);
                converterSenhas = true;
            }
            catch (Exception ex)
            {
                converterSenhas = false;
                throw new Exception("Erro ao converter tabela de usuários: " + MontaMsgErro(ex));
            }
        }

        public static bool CampoSenhaConvertido()
        {
            DataBase db = new DataBase(Modelo.cwkGlobal.CONN_STRING);
            bool ret = false;
            string cmd = @"SELECT st.max_length FROM sys.all_columns st
inner join sys.sysobjects tabela on tabela.id = st.object_id 
WHERE tabela.name = N'cw_usuario' AND st.name = N'senha'";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, cmd, null);
            if (dr.Read())
            {
                ret = dr["max_length"].ToString() == "100";
            }
            dr.Close();
            dr.Dispose();
            return ret;
        }

        public static string Encriptar()
        {
            DataBase db = new DataBase(Modelo.cwkGlobal.CONN_STRING);
            Dictionary<int, string> usuarios = new Dictionary<int, string>();
            SqlDataReader reader = null;
            try
            {
                string consulta = "select id, senha from cw_usuario";
                reader = db.ExecuteReader(CommandType.Text, consulta, null);
                while (reader.Read())
                {
                    usuarios.Add(Convert.ToInt32(reader["id"]), reader["senha"].ToString());
                }
            }
            catch (Exception ex) { throw new Exception("Erro na consulta: " + MontaMsgErro(ex)); }
            finally
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    reader.Dispose();
                }
            }
            try
            {
                foreach (int id in usuarios.Keys)
                {
                    if (usuarios[id].ToString().Length == 32) continue;
                    string cmd = "update cw_usuario set senha = '" + Cifrar(usuarios[id]) + "' where id = " + id;
                    db.ExecuteNonQuery(CommandType.Text, cmd, null);
                }
                return usuarios.Keys.Count + " usuários atualizados.";
            }
            catch (Exception ex) { throw new Exception("Erro na atualização: " + MontaMsgErro(ex)); }
        }

        private static string Cifrar(string p)
        {
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            byte[] tBytes = Encoding.ASCII.GetBytes(p);
            byte[] hBytes = hasher.ComputeHash(tBytes);

            StringBuilder sb = new StringBuilder();
            for (int c = 0; c < hBytes.Length; c++)
                sb.AppendFormat("{0:x2}", hBytes[c]);

            return (sb.ToString());
        }

        private static String MontaMsgErro(Exception exc)
        {
            if (exc != null)
                return exc.Message + "\n" + MontaMsgErro(exc.InnerException);
            else
                return "";
        }

        #region Scripts
        private static readonly string AUMENTA_SENHA_CW_USUARIO = @"BEGIN TRANSACTION
;
ALTER TABLE dbo.cw_usuario
	DROP CONSTRAINT FK_cw_usuario_cw_grupo
;
ALTER TABLE dbo.cw_grupo SET (LOCK_ESCALATION = TABLE)
;
COMMIT
BEGIN TRANSACTION
;
CREATE TABLE dbo.Tmp_cw_usuario
	(
	id int NOT NULL IDENTITY (1, 1),
	codigo int NOT NULL,
	login varchar(50) NULL,
	senha varchar(100) NULL,
	nome varchar(60) NULL,
	tipo int NULL,
	idgrupo int NULL,
	incdata datetime NULL,
	inchora datetime NULL,
	incusuario varchar(20) NULL,
	altdata datetime NULL,
	althora datetime NULL,
	altusuario varchar(20) NULL
	)  ON [PRIMARY]
;
ALTER TABLE dbo.Tmp_cw_usuario SET (LOCK_ESCALATION = TABLE)
;
SET IDENTITY_INSERT dbo.Tmp_cw_usuario ON
;
ALTER TABLE dbo.empresacwusuario
	DROP CONSTRAINT FK_empresacwusuario_cw_usuario
;
IF EXISTS(SELECT * FROM dbo.cw_usuario)
	 EXEC('INSERT INTO dbo.Tmp_cw_usuario (id, codigo, login, senha, nome, tipo, idgrupo, incdata, inchora, incusuario, altdata, althora, altusuario)
		SELECT id, codigo, login, senha, nome, tipo, idgrupo, incdata, inchora, incusuario, altdata, althora, altusuario FROM dbo.cw_usuario WITH (HOLDLOCK TABLOCKX)')
;
SET IDENTITY_INSERT dbo.Tmp_cw_usuario OFF
;
DROP TABLE dbo.cw_usuario
;
EXECUTE sp_rename N'dbo.Tmp_cw_usuario', N'cw_usuario', 'OBJECT' 
;
ALTER TABLE dbo.cw_usuario ADD CONSTRAINT
	PK_cw_usuario PRIMARY KEY CLUSTERED 
	(
	id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

;
ALTER TABLE dbo.cw_usuario ADD CONSTRAINT
	FK_cw_usuario_cw_grupo FOREIGN KEY
	(
	idgrupo
	) REFERENCES dbo.cw_grupo
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
;
COMMIT
BEGIN TRANSACTION
;
ALTER TABLE dbo.empresacwusuario ADD CONSTRAINT
	FK_empresacwusuario_cw_usuario FOREIGN KEY
	(
	idcw_usuario
	) REFERENCES dbo.cw_usuario
	(
	id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
;
COMMIT";
        #endregion
    }
}
