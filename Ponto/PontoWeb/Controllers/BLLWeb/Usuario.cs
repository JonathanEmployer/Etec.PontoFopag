using PontoWeb.Models;
using SimpleCrypto;
using System;
using System.Linq;
using System.Web;
using PontoWeb.Utils;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using Modelo;

namespace PontoWeb.Controllers.BLLWeb
{
    public static class Usuario
	{
		public static bool ValidaUsuario(UsuarioLogin userLogin, ref string retorno)
		{
			ICryptoService crypto = new PBKDF2();
			bool IsValid = false;
			CentralCliente.Usuario user = BuscaUsuarioCentralCliente(userLogin.login);
			if (user != null)
			{
				if (String.IsNullOrEmpty(user.connectionString))
				{
					retorno = "Nenhum banco de dados vinculado ao usuário, Por favor entre em contato com o suporte!";
					return IsValid;
				}
				string passCrypto = crypto.Compute(userLogin.Password, user.PasswordSalt);
				if (crypto.Compare(user.Password, passCrypto))
				{
					try
					{
						user.UltimoAcesso = DateTime.Now;
						AdicionaUsuarioCache(user);
						AtualizaUsuarioCentralCliente(user);
						IsValid = true;
                        //Adiciona o login como o usuário, pois se o usuário logou com e-mail, altero para o nome de usuário
                        userLogin.login = user.Login;

                    }
					catch (Exception e)
					{
						retorno = e.Message;
						IsValid = false;
					}
				}
				else
                    retorno = "Usuário ou Senha inválido!";
			}
			else
                retorno = "Usuário ou Senha inválido!";

			return IsValid;
		}

		#region Metodos para manipular usuário no banco da central do cliente

		public static bool DeletarUsuarioCentralCliente(CentralCliente.Usuario user)
		{
			bool retorno = true;

			string delete = @"Delete from usuario 
								 where id = @Id;";

			try
			{
				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnCentralCliente"].ConnectionString))
				{
                    using (SqlCommand cmd = new SqlCommand(delete, conn))
					{
						cmd.Parameters.AddWithValue("@Id", user.ID);

						conn.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				retorno = false;
			}

			return retorno;
		}

        public static bool DeletarUsuarioCentralClienteByUsuario(string user)
        {
            bool retorno = true;

            string delete = @"Delete from usuario 
								 where Login = @login;";

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnCentralCliente"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(delete, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login", user);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                retorno = false;
            }

            return retorno;
        }

		public static bool AtualizaUsuarioCentralCliente(CentralCliente.Usuario user)
		{
			bool retorno = true;

			string sqlUpdate = @"Update usuario 
									set Login = @Login,
										Senha = @Senha,
										PasswordSalt = @PasswordSalt,
										Password = @Password,
										EMAIL = @EMAIL,
										UltimoAcesso = @UltimoAcesso, 
										connectionString = @connectionString
								  where id = @Id;";

			try
			{
				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnCentralCliente"].ConnectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sqlUpdate, conn))
					{
						cmd.Parameters.AddWithValue("@Login", user.Login);
						cmd.Parameters.AddWithValue("@Senha", user.Senha);
						cmd.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
						cmd.Parameters.AddWithValue("@Password", user.Password);
						cmd.Parameters.AddWithValue("@EMAIL", String.IsNullOrEmpty(user.EMAIL) ? "" : user.EMAIL);
						cmd.Parameters.AddWithValue("@UltimoAcesso", user.UltimoAcesso);
						cmd.Parameters.AddWithValue("@connectionString", user.connectionString);
						cmd.Parameters.AddWithValue("@Id", user.ID);

						conn.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				retorno = false;
			}


			return retorno;
		}

		public static bool IncluiUsuarioCentralCliente(CentralCliente.Usuario user)
		{
			bool retorno = true;
			string sqlUpdate = @"INSERT INTO usuario (Login, Senha, PasswordSalt, Password, EMAIL, UltimoAcesso, connectionString, UtilizaPontoWeb)
									VALUES
									(@Login, @Senha, @PasswordSalt, @Password, @EMAIL, @UltimoAcesso, @connectionString, @UtilizaPontoWeb)";

			try
			{
				using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnCentralCliente"].ConnectionString))
				{
					using (SqlCommand cmd = new SqlCommand(sqlUpdate, conn))
					{
						cmd.Parameters.AddWithValue("@Login", user.Login);
						cmd.Parameters.AddWithValue("@Senha", user.Senha);
						cmd.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
						cmd.Parameters.AddWithValue("@Password", user.Password);
						cmd.Parameters.AddWithValue("@EMAIL", user.EMAIL);
						cmd.Parameters.AddWithValue("@UltimoAcesso", user.UltimoAcesso);
						cmd.Parameters.AddWithValue("@connectionString", user.connectionString);
						cmd.Parameters.AddWithValue("@UtilizaPontoWeb", user.UtilizaPontoWeb);

						conn.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				retorno = false;
			}

			return retorno;

		}

		//        private static string BuscaConnectionStringCentralCliente(string cnpj_cpf, string _connectionString)
		//        {
		//            SqlConnectionStringBuilder newConection = new SqlConnectionStringBuilder();
		//            CentralCliente.Empresa empresa = new CentralCliente.Empresa();
		//            using (var conn = new SqlConnection(_connectionString))
		//            {
		//                using (var cmd = conn.CreateCommand())
		//                {
		//                    conn.Open();
		//                    cmd.CommandText = @"SELECT emp.* from empresa emp
		//                                        join entidade ent on emp.IDEntidade = ent.ID
		//                                        where ent.CNPJ_CPF = @CNPJ_CPF";
		//                    cmd.Parameters.AddWithValue("@CNPJ_CPF", cnpj_cpf);

		//                    using (var reader = cmd.ExecuteReader())
		//                    {
		//                        AutoMapper.Mapper.CreateMap<IDataReader, CentralCliente.Usuario>();
		//                        empresa = AutoMapper.Mapper.Map<List<CentralCliente.Empresa>>(reader).FirstOrDefault();
		//                    }
		//                }
		//            }
		//            return newConection.ConnectionString;
		//        }


		public static IList<CentralCliente.Usuario> GetAll()
		{
			IList<CentralCliente.Usuario> listaUsuarios = new List<CentralCliente.Usuario>();

			string _connectionString = ConfigurationManager.ConnectionStrings["ConnCentralCliente"].ConnectionString;
			using (var conn = new SqlConnection(_connectionString))
			{
				using (var cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandText = "select * from usuario where utilizaPontoWeb = 1";

					using (var reader = cmd.ExecuteReader())
					{
						AutoMapper.Mapper.CreateMap<IDataReader, CentralCliente.Usuario>();
						listaUsuarios = AutoMapper.Mapper.Map<List<CentralCliente.Usuario>>(reader);
					}
				}
			}

			return listaUsuarios;
		}

		public static CentralCliente.Usuario BuscaUsuarioCentralCliente(string usuario)
		{
			CentralCliente.Usuario user = new CentralCliente.Usuario();
			string _connectionString = ConfigurationManager.ConnectionStrings["ConnCentralCliente"].ConnectionString;

			using (var conn = new SqlConnection(_connectionString))
			{
				using (var cmd = conn.CreateCommand())
				{
                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@login";
                    param.Value = usuario;
                    param.SqlDbType = SqlDbType.VarChar;
					conn.Open();
					cmd.CommandText = "select * from usuario where (login = @login or email = @login) and utilizaPontoWeb = 1";
                    cmd.Parameters.Add(param);
					using (var reader = cmd.ExecuteReader())
					{
						AutoMapper.Mapper.CreateMap<IDataReader, CentralCliente.Usuario>();
						user = AutoMapper.Mapper.Map<List<CentralCliente.Usuario>>(reader).FirstOrDefault();
					}
				}
			}
			if ((user != null) &&
				(!String.IsNullOrEmpty(user.connectionString)) &&
				(user.connectionString.Contains("Data Source")))
			{
				user.connectionString = BLL.CriptoString.Encrypt(user.connectionString);
			}
			return user;
		}

		public static CentralCliente.Usuario BuscaUsuarioCentralClientePorId(int ID)
		{
			CentralCliente.Usuario user = new CentralCliente.Usuario();
			string _connectionString = ConfigurationManager.ConnectionStrings["ConnCentralCliente"].ConnectionString;

			using (var conn = new SqlConnection(_connectionString))
			{
				using (var cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandText = "select * from usuario where ID = @ID and utilizaPontoWeb = 1";
					cmd.Parameters.AddWithValue("@ID", ID);
					using (var reader = cmd.ExecuteReader())
					{
						AutoMapper.Mapper.CreateMap<IDataReader, CentralCliente.Usuario>();
						user = AutoMapper.Mapper.Map<List<CentralCliente.Usuario>>(reader).FirstOrDefault();
					}
				}
			}
			if ((user != null) &&
				(!String.IsNullOrEmpty(user.connectionString)) &&
				(user.connectionString.Contains("Data Source")))
			{
				user.connectionString = BLL.CriptoString.Encrypt(user.connectionString);
			}
			return user;
		}

		public static CentralCliente.Usuario ValidaUsuarioPorLogin(string login)
		{
			string _connectionString = ConfigurationManager.ConnectionStrings["ConnCentralCliente"].ConnectionString;
			CentralCliente.Usuario user = new CentralCliente.Usuario();
			using (var conn = new SqlConnection(_connectionString))
			{
				using (var cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandText = "select * from usuario where login = @login and utilizaPontoWeb = 1";
					cmd.Parameters.AddWithValue("@login", login);
					using (var reader = cmd.ExecuteReader())
					{
						AutoMapper.Mapper.CreateMap<IDataReader, CentralCliente.Usuario>();
						user = AutoMapper.Mapper.Map<List<CentralCliente.Usuario>>(reader).FirstOrDefault();
						if (user != null)
							user.connectionString = BLL.CriptoString.Encrypt(user.connectionString);
					}
				}
			}

			return user;
		}

		public static CentralCliente.Usuario ValidaUsuarioPorEmail(string email)
		{
			string _connectionString = ConfigurationManager.ConnectionStrings["ConnCentralCliente"].ConnectionString;
			CentralCliente.Usuario user = new CentralCliente.Usuario();
			using (var conn = new SqlConnection(_connectionString))
			{
				using (var cmd = conn.CreateCommand())
				{
					conn.Open();
					cmd.CommandText = "select * from usuario where EMAIL = @email and utilizaPontoWeb = 1";
					cmd.Parameters.AddWithValue("@email", email);
					using (var reader = cmd.ExecuteReader())
					{
						AutoMapper.Mapper.CreateMap<IDataReader, CentralCliente.Usuario>();
						user = AutoMapper.Mapper.Map<List<CentralCliente.Usuario>>(reader).FirstOrDefault();
						if (user != null)
							user.connectionString = BLL.CriptoString.Encrypt(user.connectionString);
					}
				}
			}

			return user;
		}

		#endregion

		#region Métodos para manipular cache do usuário
		public static cw_usuario GetUsuarioLogadoCache()
		{
			cw_usuario usuario = new cw_usuario();
			Cacher<cw_usuario> cache2 = new Cacher<cw_usuario>("UsuarioLogado", () => usuario);
			usuario = cache2.Value;
			// Se não retornar usuário do cache ou retornou o usuário vazio, busca o usuário novamente no banco e seta no cache.
			if (usuario == null || String.IsNullOrEmpty(usuario.login))
			{
				CentralCliente.Usuario usuarioRec = GetUsuarioLogado();
				// se conseguiu pegar no banco, seta no cache.
				if (usuarioRec != null && !String.IsNullOrEmpty(usuarioRec.Login))
				{
					cache2.Clear();
					AdicionaUsuarioCache(usuarioRec);
					Cacher<cw_usuario> cacheRec = new Cacher<cw_usuario>("UsuarioLogado", () => usuario);
					usuario = cacheRec.Value;
				}
			}
			return usuario;
		}
		public static UsuarioPontoWeb GetUsuarioPontoWebLogadoCache()
		{
			UsuarioPontoWeb usuarioPontoWeb = new UsuarioPontoWeb();
			Cacher<UsuarioPontoWeb> cacheUsuarioPontoWeb = new Cacher<UsuarioPontoWeb>("UsuarioPontoWebLogado", () => usuarioPontoWeb);
			usuarioPontoWeb = cacheUsuarioPontoWeb.Value;
			// Se não retornar usuário do cache ou retornou o usuário vazio, busca o usuário novamente no banco e seta no cache.
            if (usuarioPontoWeb == null || String.IsNullOrEmpty(usuarioPontoWeb.Login) || (HttpContext.Current.User != null && HttpContext.Current.User.Identity.Name != usuarioPontoWeb.Login))
			{
				var cwu = Usuario.GetUsuarioLogadoCache();
                 if (!String.IsNullOrEmpty(cwu.ConnectionStringDecrypt))
                {
				BLL.UsuarioPontoWeb bllUpw = new BLL.UsuarioPontoWeb(cwu.ConnectionStringDecrypt, usuarioPontoWeb);
				usuarioPontoWeb = bllUpw.LoadObject(cwu.id);
				BLL.Empresa bllEmp = new BLL.Empresa(cwu.ConnectionStringDecrypt, usuarioPontoWeb);
				usuarioPontoWeb.EmpresaPrincipal = bllEmp.GetEmpresaPrincipal();                    
                usuarioPontoWeb.ConnectionString = cwu.ConnectionStringDecrypt;
                usuarioPontoWeb.ConsultaUtilizaRegistradorAllEmp = bllEmp.ConsultaUtilizaRegistradorAllEmp();
				// se conseguiu pegar no banco, seta no cache.
				if (usuarioPontoWeb != null && !String.IsNullOrEmpty(usuarioPontoWeb.Login))
				{
					cacheUsuarioPontoWeb.Clear();
					Cacher<UsuarioPontoWeb> cache = new Cacher<UsuarioPontoWeb>("UsuarioPontoWebLogado", () => usuarioPontoWeb);
					cache.AlteraCache(() => usuarioPontoWeb);
					//Atualiza o Cache
					//cache.Refresh();
					usuarioPontoWeb = cache.Value;
				}
                }
                 else
                 {
                     if (HttpContext.Current.User != null)
                     {
                         HttpContext.Current.User = null;
                         HttpContext.Current.Session.Clear();
                         HttpContext.Current.Session.Abandon();
                         Usuario.LimpaCacheUsuario();
                         throw new Exception("Erro ao recuperar usuário");
                     }
                 }
			}

			return usuarioPontoWeb;
		}

		public static void LimpaUsuarioPontoWebLogadoCache()
		{
			UsuarioPontoWeb usuario = new UsuarioPontoWeb();
			Cacher<UsuarioPontoWeb> cache = new Cacher<UsuarioPontoWeb>("UsuarioPontoWebLogado", () => usuario);
			cache.Clear();
		}

		private static void AdicionaUsuarioCache(CentralCliente.Usuario user)
		{
			cw_usuario userCache = new cw_usuario();
			userCache = BuscaUsuarioPonto(user);
			if (userCache != null)
			{
				try
				{
					//Passa o usuário para o novo cache a ser gerado
					Cacher<cw_usuario> cache = new Cacher<cw_usuario>("UsuarioLogado", () => userCache);
					//Atualiza o Cache
					cache.Refresh();
					AdicionaAcessoCache(userCache);
					userCache.UltimoAcesso = user.UltimoAcesso;

                    AtualizaLogoEmpresa(userCache);

					AtualizaUsuarioPonto(user, userCache);
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			else
			{
				throw new Exception("Erro ao validar usuário no ponto");
			}
		}

        public static void AtualizaLogoEmpresa()
        {
            cw_usuario userCache = GetUsuarioLogadoCache();
            Cacher<cw_usuario> cache = new Cacher<cw_usuario>("UsuarioLogado", () => userCache);
            //Atualiza o Cache
            cache.Refresh();
            AtualizaLogoEmpresa(userCache);
        }

        private static void AtualizaLogoEmpresa(cw_usuario userCache)
        {
            #region Busca Logo Empresa para Layout
            UsuarioPontoWeb usuarioPontoWeb = new UsuarioPontoWeb();
            usuarioPontoWeb = GetUsuarioPontoWebLogadoCache();
            BLL.Parametros bllParms = new BLL.Parametros(userCache.ConnectionStringDecrypt, usuarioPontoWeb);
            Parametros parms = bllParms.LoadPrimeiro();
            userCache.LogoEmpresa = parms.LogoEmpresa;
            #endregion
        }
		public static void LimpaCacheUsuario()
		{
			HttpContext.Current.User = null;
			cw_usuario usuario = new cw_usuario();
			Cacher<cw_usuario> cache = new Cacher<cw_usuario>("UsuarioLogado", () => usuario);
			cache.Clear();
			LimpaAcessoCache();
		}
		#endregion
        #region Adiciona tentativas login cache
        public static void AdicionaTentativasLogin(TentativasLogin tl)
        {
            IList<TentativasLogin> tll = new List<TentativasLogin>();
            Cacher<IList<TentativasLogin>> cache = new Cacher<IList<TentativasLogin>>("TentavivasLogin", () => tll);
            if (tll == null)
            {
                tll = new List<TentativasLogin>();
            }

            TentativasLogin tla = tll.Where(x => x.Usuario == tl.Usuario).FirstOrDefault();
            if (tla != null)
            {
                tla.Tentativas = tl.Tentativas;
                tla.UltimaTentativa = tl.UltimaTentativa;
            }
            else
            {
                tll.Add(tl);
            }

            cache.Refresh();
        }

        public static IList<TentativasLogin> GetTentativasLogin()
        {
            IList<TentativasLogin> tll = new List<TentativasLogin>();
            Cacher<IList<TentativasLogin>> cache = new Cacher<IList<TentativasLogin>>("TentavivasLogin", () => tll);
            tll = cache.Value;
            return tll;
        }

        public static TentativasLogin GetTentativaLogin(string usuario)
        {
            IList<TentativasLogin> tll = GetTentativasLogin();
            if (tll == null || tll.Count() == 0)
            {
                return new TentativasLogin();
            }
            else
            {
                return tll.Where(x => x.Usuario == usuario).FirstOrDefault();
            }
        }
        #endregion
        #region Adiciona as permissões no cache
        internal static void AdicionaAcessoCache(cw_usuario usuarioPonto)
		{
			if (usuarioPonto != null)
			{
				try
				{
					string _connectionString = BLL.CriptoString.Decrypt(usuarioPonto.connectionString);
					List<string> permissoes = new List<string>();
					List<Cw_GrupoAcesso> acesso = new List<Cw_GrupoAcesso>();
					using (var conn = new SqlConnection(_connectionString))
					{
						string sql = "";
						if (usuarioPonto.tipo != 0 && usuarioPonto.idgrupo.GetValueOrDefault() > 0)
						{
							sql = @"select a.id, pa.IDCw_Grupo, pa.IDCw_Acesso, pa.incdata, pa.inchora, pa.incusuario, pa.altdata, pa.althora, pa.altusuario, a.controller, pa.Consultar, pa.Excluir, pa.Cadastrar, pa.Alterar
													  from cw_grupoacesso as pa
													 inner join cw_acesso as a on pa.IDCw_Acesso = a.id
													 where pa.idcw_grupo = @id";
						}
						else
						{
							if (usuarioPonto.tipo == 0)
							{
								sql = @" select 0, pa.idgrupo IDCw_Grupo, pa.id IDCw_Acesso, pa.incdata, pa.inchora, pa.incusuario, pa.altdata, pa.althora, pa.altusuario, pa.controller, pa.Consultar, pa.Excluir, pa.Cadastrar, pa.Alterar
																		  from cw_acesso as pa
																		 where controller is not null and controller != ''";
							}
								
						}
						using (var cmd = conn.CreateCommand())
						{
							conn.Open();
							cmd.CommandText = sql;
							if (usuarioPonto.idgrupo.GetValueOrDefault() > 0)
							{
								cmd.Parameters.AddWithValue("@id", usuarioPonto.idgrupo);
							}
							
							using (var reader = cmd.ExecuteReader())
							{
								AutoMapper.Mapper.CreateMap<IDataReader, Cw_GrupoAcesso>();
								acesso = AutoMapper.Mapper.Map<List<Cw_GrupoAcesso>>(reader).ToList();
							}
						}
					}
					foreach (Cw_GrupoAcesso acess in acesso)
					{
						if (acess.bCadastrar || acess.bConsultar || acess.bAlterar || acess.bExcluir)
						{
							permissoes.Add(acess.Controller);
							if (acess.Consultar == 1) permissoes.Add(acess.Controller + "Consultar");
							if (acess.Cadastrar == 1) permissoes.Add(acess.Controller + "Cadastrar");
							if (acess.Alterar == 1) permissoes.Add(acess.Controller + "Alterar");
							if (acess.Excluir == 1) permissoes.Add(acess.Controller + "Excluir"); 
						}

					}
					//Passa as permissões para o novo cache a ser gerado
					Cacher<List<string>> cacheAcesso = new Cacher<List<string>>("PermissoesAcesso", () => permissoes);
					//Atualiza o Cache
					cacheAcesso.Refresh();
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			else
			{
				throw new Exception("Erro ao validar usuário no ponto");
			}
		}

		public static void LimpaAcessoCache()
		{
			Cw_GrupoAcesso GrupoAcesso = new Cw_GrupoAcesso();
			Cacher<Cw_GrupoAcesso> cacheAcesso = new Cacher<Cw_GrupoAcesso>("PermissoesAcesso", () => GrupoAcesso);
			cacheAcesso.Clear();
		}

		public static List<string> GetAcessoCache()
		{
			List<string> acesso = new List<string>();
			Cacher<List<string>> cacheAcesso = new Cacher<List<string>>("PermissoesAcesso", () => acesso);
			acesso = cacheAcesso.Value;
			return acesso;
		}
		#endregion
		public static CentralCliente.Usuario GetUsuarioLogado()
		{
			string _login = HttpContext.Current.User.Identity.Name;

			if (_login == "")
			{
				return new CentralCliente.Usuario();
			}
			else
			{
				CentralCliente.Usuario user = BuscaUsuarioCentralCliente(_login);
				return user;
			}
		}

		#region Metodos para manipular usuário no banco do ponto
		private static void AtualizaUsuarioPonto(CentralCliente.Usuario user, cw_usuario userPonto)
		{
			string sqlUpdate = @"Update cw_usuario 
								   set UltimoAcesso = @UltimoAcesso, 
									   connectionString = @connectionString,
									   Password = @Password,
									   PasswordSalt = @PasswordSalt,
									   email = @email
								 where id = @Id;";
			string _connectionString = BLL.CriptoString.Decrypt(user.connectionString);
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				using (SqlCommand cmd = new SqlCommand(sqlUpdate, conn))
				{
					cmd.Parameters.AddWithValue("@UltimoAcesso", user.UltimoAcesso);
					cmd.Parameters.AddWithValue("@connectionString", user.connectionString);
					cmd.Parameters.AddWithValue("@Password", user.Password);
					cmd.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
					cmd.Parameters.AddWithValue("@email", user.EMAIL);
					cmd.Parameters.AddWithValue("@Id", userPonto.id);

					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}

		private static cw_usuario BuscaUsuarioPonto(CentralCliente.Usuario user)
		{
			cw_usuario usuarioPonto = new cw_usuario();
			string _connectionString = BLL.CriptoString.Decrypt(user.connectionString);
			using (SqlConnection conn = new SqlConnection(_connectionString))
			{
				using (SqlCommand cmd = conn.CreateCommand())
				{
					try
					{
						conn.Open();
					}
					catch (Exception e)
					{
						throw new Exception("Conexão não encontrada, contate o suporte!");
					}
					cmd.CommandText = "select * from cw_usuario where idUsuarioCentralCliente = @id";
					cmd.Parameters.AddWithValue("@id", user.ID);
					using (var reader = cmd.ExecuteReader())
					{
						AutoMapper.Mapper.CreateMap<IDataReader, cw_usuario>();
						usuarioPonto = AutoMapper.Mapper.Map<List<cw_usuario>>(reader).FirstOrDefault();
						if (usuarioPonto != null)
						{
							usuarioPonto.connectionString = user.connectionString; 
						}
						else
						{
							throw new Exception("O usuário informado não foi registrado na base de dados do Pontofopag. Entre em contato com o Suporte.");
						}
					}
				}
			}

			return usuarioPonto;
		}
		#endregion

	}
}