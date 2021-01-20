using System;

namespace Modelo
{
    public class UsuarioPontoWeb : Modelo.Cw_Usuario
    {
        public string PasswordSalt { get; set; }
        public string Password { get; set; }
        public DateTime UltimoAcesso { get; set; }
        public string ConnectionString { get; set; }
        public int idUsuarioCentralCliente { get; set; }
        public Empresa EmpresaPrincipal { get; set; }
        public string DataBase { get { 
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);
            return builder["Initial Catalog"] as string;
        } }
        public string CentroServico { get; set; }
        public bool ConsultaUtilizaRegistradorAllEmp { get; set; }
        public int ServicoCalculo { get; set; }
    }
}
