using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DAL.SQL
{
    public class UsuarioPontoWeb : DAL.SQL.cwkDALBase, DAL.IUsuarioPontoWeb
    {
        private DataBase db;

        protected virtual int IdUsuarioLogado { get; set; }
        private string SELECTALLLIST { get; set; }
        private string SELECTLOGIN { get; set; }

        private string SELECTALLLISTWEB  = @"   SELECT   us.*
                                                         , case when us.tipo = 0 then 'Administrador' when us.tipo = 1 then 'Operador' when us.tipo = 2 then 'Gerente' end AS tipo
                                                         , CAST(gp.codigo as varchar) + ' | ' + gp.nome AS GrupoUsuario 
                                                FROM cw_usuario us
                                                LEFT JOIN cw_grupo gp ON gp.id = us.idgrupo  
                                                WHERE us.login not in ('revenda','cwork')
                                                AND us.idUsuarioCentralCliente is not null
                                                AND us.idUsuarioCentralCliente <> ''";
        public UsuarioPontoWeb(DataBase database)
        {
            db = database;
            base.db = new cwkDataBase(db.ConnectionString);
            TABELA = "cw_usuario";

            SELECTPID = @"   SELECT us.*, 
                                    CAST(gp.codigo as varchar) + ' | ' + gp.nome AS GrupoUsuario
                             FROM cw_usuario us
                             LEFT JOIN cw_grupo gp ON gp.id = us.idgrupo 
                             WHERE us.id = @id";

            SELECTLOGIN = @"SELECT us.*,
                                    CAST(gp.codigo as varchar) + ' | ' + gp.nome AS GrupoUsuario 
                            FROM cw_usuario us
                            LEFT JOIN cw_grupo gp ON gp.id = us.idgrupo 
                            WHERE us.login = @login";

            SELECTALL = @"   SELECT   us.id
                                    , us.codigo
                                    , us.login
                                    , us.nome
                                    , gp.nome AS grupo
                                    , us.EMAIL
                                    , us.SENHAEMAIL
                                    , us.SMTP
                                    , us.SSL
                                    , us.PORTA
                                    , us.Ativo
                                    , case when us.tipo = 0 then 'Administrador' when us.tipo = 1 then 'Operador' when us.tipo = 2 then 'Gerente' end AS tipo
                             FROM cw_usuario us
                             LEFT JOIN cw_grupo gp ON gp.id = us.idgrupo Where us.login not in ('revenda','cwork')";

            SELECTALLLIST = @"   SELECT   us.*
                                    , case when us.tipo = 0 then 'Administrador' when us.tipo = 1 then 'Operador' when us.tipo = 2 then 'Gerente' end AS tipo
                                    , CAST(gp.codigo as varchar) + ' | ' + gp.nome AS GrupoUsuario 
                             FROM cw_usuario us
                             LEFT JOIN cw_grupo gp ON gp.id = us.idgrupo Where us.login not in ('revenda','cwork')";

            INSERT = @"  INSERT INTO cw_usuario
                            (codigo, login, senha, nome, tipo, idgrupo, incdata, inchora, incusuario, altdata, althora, altusuario, EMAIL, SENHAEMAIL, 
                             SMTP, SSL, PORTA, PasswordSalt, Password, UltimoAcesso, connectionString, idUsuarioCentralCliente, UtilizaControleContratos, UtilizaControleEmpresa, UtilizaControleSupervisor, Cpf, SenhaRep, LoginRep, utilizaregistradordesktop, Ativo
                             CpfUsuario, PermissaoConcluirFluxoPnl)
							VALUES
							(@codigo, @login, @senha, @nome, @tipo, @idgrupo, @incdata, @inchora, @incusuario, @altdata, @althora, @altusuario, @EMAIL, @SENHAEMAIL, 
                             @SMTP, @SSL, @PORTA, @PasswordSalt, @Password, @UltimoAcesso, @connectionString, @idUsuarioCentralCliente, @UtilizaControleContratos, @UtilizaControleEmpresa, @UtilizaControleSupervisor, @Cpf, @SenhaRep, @LoginRep, @utilizaregistradordesktop, @Ativo
                             @CpfUsuario, @PermissaoConcluirFluxoPnl) 
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE cw_usuario SET 
							 codigo = @codigo
							, login = @login
							, senha = @senha
							, nome = @nome
							, tipo = @tipo
							, idgrupo = @idgrupo
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , EMAIL = @EMAIL
                            , SENHAEMAIL = @SENHAEMAIL
                            , SMTP = @SMTP
                            , SSL = @SSL
                            , PORTA = @PORTA
                            , PasswordSalt = @PasswordSalt
                            , Password = @Password
                            , UltimoAcesso = @UltimoAcesso
                            , connectionString = @connectionString
                            , idUsuarioCentralCliente = @idUsuarioCentralCliente
                            , UtilizaControleContratos = @UtilizaControleContratos
                            , UtilizaControleEmpresa = @UtilizaControleEmpresa
                            , UtilizaControleSupervisor = @UtilizaControleSupervisor
                            , Cpf = @Cpf
                            , SenhaRep = @SenhaRep
                            , LoginRep = @LoginRep
                            , utilizaregistradordesktop = @utilizaregistradordesktop
                            , CpfUsuario = @CpfUsuario
                            , PermissaoConcluirFluxoPnl = @PermissaoConcluirFluxoPnl
                            , Ativo = @Ativo 
						WHERE id = @id";

            DELETE = @"  DELETE FROM cw_usuario WHERE id = @id";

            MAXCOD = @"  SELECT MAX(codigo) AS codigo FROM cw_usuario";
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@login", SqlDbType.VarChar),
                new SqlParameter ("@senha", SqlDbType.VarChar),
                new SqlParameter ("@nome", SqlDbType.VarChar),
                new SqlParameter ("@tipo", SqlDbType.Int),
                new SqlParameter ("@idgrupo", SqlDbType.Int),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@EMAIL", SqlDbType.VarChar),
                new SqlParameter ("@SENHAEMAIL", SqlDbType.VarChar),
                new SqlParameter ("@SMTP", SqlDbType.VarChar),
                new SqlParameter ("@SSL", SqlDbType.Bit),
                new SqlParameter ("@PORTA", SqlDbType.VarChar),
                new SqlParameter ("@PasswordSalt", SqlDbType.VarChar),
                new SqlParameter ("@Password", SqlDbType.VarChar),
                new SqlParameter ("@UltimoAcesso", SqlDbType.DateTime),
                new SqlParameter ("@connectionString", SqlDbType.VarChar),
                new SqlParameter ("@idUsuarioCentralCliente", SqlDbType.Int),
                new SqlParameter ("@UtilizaControleContratos", SqlDbType.Bit),
                new SqlParameter ("@UtilizaControleEmpresa", SqlDbType.Bit),
                new SqlParameter ("@UtilizaControleSupervisor", SqlDbType.Bit),
                new SqlParameter ("@Cpf", SqlDbType.VarChar),
                new SqlParameter ("@SenhaRep", SqlDbType.VarChar),
                new SqlParameter ("@LoginRep", SqlDbType.VarChar),
                new SqlParameter ("@utilizaregistradordesktop", SqlDbType.Bit),
                new SqlParameter ("@CpfUsuario", SqlDbType.VarChar),
                new SqlParameter ("@PermissaoConcluirFluxoPnl", SqlDbType.Bit),
                new SqlParameter ("@Ativo", SqlDbType.Bit),
            };
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.cwkModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.UsuarioPontoWeb)obj).Codigo;
            parms[2].Value = ((Modelo.UsuarioPontoWeb)obj).Login;
            parms[3].Value = ((Modelo.UsuarioPontoWeb)obj).Senha;
            parms[4].Value = ((Modelo.UsuarioPontoWeb)obj).Nome;
            parms[5].Value = ((Modelo.UsuarioPontoWeb)obj).Tipo;
            if (((Modelo.Cw_Usuario)obj).IdGrupo > 0)
                parms[6].Value = ((Modelo.UsuarioPontoWeb)obj).IdGrupo;
            parms[7].Value = ((Modelo.UsuarioPontoWeb)obj).Incdata;
            parms[8].Value = ((Modelo.UsuarioPontoWeb)obj).Inchora;
            parms[9].Value = ((Modelo.UsuarioPontoWeb)obj).Incusuario;
            parms[10].Value = ((Modelo.UsuarioPontoWeb)obj).Altdata;
            parms[11].Value = ((Modelo.UsuarioPontoWeb)obj).Althora;
            parms[12].Value = ((Modelo.UsuarioPontoWeb)obj).Altusuario;

            parms[13].Value = ((Modelo.UsuarioPontoWeb)obj).Email;
            parms[14].Value = ((Modelo.UsuarioPontoWeb)obj).SenhaEmail;
            parms[15].Value = ((Modelo.UsuarioPontoWeb)obj).SMTP;
            parms[16].Value = ((Modelo.UsuarioPontoWeb)obj).SSL;
            parms[17].Value = ((Modelo.UsuarioPontoWeb)obj).Porta;

            parms[18].Value = ((Modelo.UsuarioPontoWeb)obj).PasswordSalt;
            parms[19].Value = ((Modelo.UsuarioPontoWeb)obj).Password;
            parms[20].Value = ((Modelo.UsuarioPontoWeb)obj).UltimoAcesso;
            parms[21].Value = ((Modelo.UsuarioPontoWeb)obj).ConnectionString;
            parms[22].Value = ((Modelo.UsuarioPontoWeb)obj).idUsuarioCentralCliente;
            parms[23].Value = ((Modelo.Cw_Usuario)obj).UtilizaControleContratos;
            parms[24].Value = ((Modelo.Cw_Usuario)obj).UtilizaControleEmpresa;
            parms[25].Value = ((Modelo.Cw_Usuario)obj).UtilizaControleSupervisor;
            parms[26].Value = ((Modelo.Cw_Usuario)obj).Cpf;
            parms[27].Value = ((Modelo.Cw_Usuario)obj).SenhaRep;
            parms[28].Value = ((Modelo.Cw_Usuario)obj).LoginRep;
            parms[29].Value = ((Modelo.Cw_Usuario)obj).utilizaregistradordesktop;
            parms[30].Value = ((Modelo.Cw_Usuario)obj).CPFUsuario;
            parms[31].Value = ((Modelo.Cw_Usuario)obj).PermissaoConcluirFluxoPnl;
            parms[32].Value = ((Modelo.UsuarioPontoWeb)obj).Ativo;

            if (((Modelo.Cw_Usuario)obj).UtilizaControleEmpresa)
            {
                SetaBloqueiousuarios();
            }
            else
            {
                int UtilizaControleEmpresa = 1;
                if (!ConsultaUtilizaControleEmpresa(UtilizaControleEmpresa))
                {
                    ZeraBloqueiousuarios();
                }
            }                      
        }

        public List<Modelo.UsuarioPontoWeb> GetAllList()
        {
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALLLIST, null);
            List<Modelo.UsuarioPontoWeb> listaRetorno = new List<Modelo.UsuarioPontoWeb>();

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.UsuarioPontoWeb>();
                listaRetorno = AutoMapper.Mapper.Map<List<Modelo.UsuarioPontoWeb>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return listaRetorno;
        }

        public List<Modelo.UsuarioPontoWeb> GetAllListWeb()
        {
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTALLLISTWEB, null);
            List<Modelo.UsuarioPontoWeb> listaRetorno = new List<Modelo.UsuarioPontoWeb>();

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.UsuarioPontoWeb>();
                listaRetorno = AutoMapper.Mapper.Map<List<Modelo.UsuarioPontoWeb>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return listaRetorno;
        }

        public Modelo.UsuarioPontoWeb LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.VarChar, 15) };
            parms[0].Value = id;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPID, parms);

            Modelo.UsuarioPontoWeb objCw_Usuario = new Modelo.UsuarioPontoWeb();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.UsuarioPontoWeb>();
                objCw_Usuario = AutoMapper.Mapper.Map<List<Modelo.UsuarioPontoWeb>>(dr).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCw_Usuario;
        }

        public Modelo.UsuarioPontoWeb LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@codigo", SqlDbType.VarChar, 15) };
            parms[0].Value = codigo;

            string sql = SELECTALLLISTWEB + " AND us.codigo = @codigo ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Modelo.UsuarioPontoWeb objCw_Usuario = new Modelo.UsuarioPontoWeb();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.UsuarioPontoWeb>();
                objCw_Usuario = AutoMapper.Mapper.Map<List<Modelo.UsuarioPontoWeb>>(dr).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return objCw_Usuario;
        }

        public Modelo.UsuarioPontoWeb LoadObjectLogin(string pLogin)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@login", SqlDbType.VarChar) };
            parms[0].Value = pLogin;

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTLOGIN, parms);

            Modelo.UsuarioPontoWeb objCw_Usuario = new Modelo.UsuarioPontoWeb();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.UsuarioPontoWeb>();
                objCw_Usuario = AutoMapper.Mapper.Map<List<Modelo.UsuarioPontoWeb>>(dr).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return objCw_Usuario;
        }

        public int GetIdAdmin()
        {
            SqlParameter[] parms = new SqlParameter[0];
            string aux = @"SELECT MAX(id) FROM cw_usuario WHERE tipo = 0";

            return (int)db.ExecuteScalar(CommandType.Text, aux, parms);
        }

        protected override bool SetInstance(SqlDataReader dr, Modelo.cwkModeloBase obj)
        {
            throw new NotImplementedException();
        }

        public override void Excluir(Modelo.cwkModeloBase obj)
        {
            ApagarVinculoFuncionario(obj);
            ApagarVinculoContratoUsuario(obj);
            ApagarVinculoEmpresaCwUsuario(obj);
            base.Excluir(obj);
        }

        private void ApagarVinculoFuncionario(Modelo.cwkModeloBase obj)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int) };
            parms[0].Value = obj.Id;
            string comando = @"UPDATE funcionario SET idcw_usuario = NULL WHERE idcw_usuario = @id";
            db.ExecNonQueryCmd(CommandType.Text, comando, true, parms);
        }

        private void ApagarVinculoContratoUsuario(Modelo.cwkModeloBase obj)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int) };
            parms[0].Value = obj.Id;
            string comando = @"DELETE FROM contratousuario WHERE idcwusuario = @id";
            db.ExecNonQueryCmd(CommandType.Text, comando, true, parms);
        }

        private void ApagarVinculoEmpresaCwUsuario(Modelo.cwkModeloBase obj)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int) };
            parms[0].Value = obj.Id;
            string comando = @"DELETE FROM empresacwusuario WHERE idcw_usuario = @id";
            db.ExecNonQueryCmd(CommandType.Text, comando, true, parms);
        }

        public bool ConsultaUtilizaControleEmpresa(int utilizacontroleempresa)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@utilizacontroleempresa", SqlDbType.Int) };
            parms[0].Value = utilizacontroleempresa;
            string aux = @"SELECT utilizacontroleempresa, Count(*) FROM cw_usuario where utilizacontroleempresa = @utilizacontroleempresa GROUP BY utilizacontroleempresa HAVING Count(*) > 1 ";

             var controEmp = db.ExecuteScalar(CommandType.Text, aux, parms);
            if (controEmp==null)
            {
                return false;
            }
            return true;
        }

        public void SetaBloqueiousuarios()
        {
            Modelo.Empresa objEmpresa = new Modelo.Empresa();
            SqlParameter[] parms = new SqlParameter[] { };
            string aux = @"UPDATE empresa set bloqueiousuarios = 1 WHERE bloqueiousuarios = 0";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            
            return;
        }

        public void ZeraBloqueiousuarios()
        {
            Modelo.Empresa objEmpresa = new Modelo.Empresa();
            SqlParameter[] parms = new SqlParameter[] { };
            string aux = @"UPDATE empresa set bloqueiousuarios = 0 WHERE bloqueiousuarios = 1";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            return;
        }
    }
}
