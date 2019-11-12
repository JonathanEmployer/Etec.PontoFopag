using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RegistradorPontoWeb.Models.Ponto
{
    public partial class PontofopagEntities: DbContext
    {
        private string usuarioLogado;
        public override int SaveChanges()
        {
            foreach (var entry in this.ChangeTracker.Entries().Where(e => e.Entity.GetType().GetProperty("IncData") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("IncData").CurrentValue = DateTime.Now.Date;
                    entry.Property("IncHora").CurrentValue = DateTime.Now;
                    entry.Property("IncUsuario").CurrentValue = usuarioLogado ?? "Pontofopag";
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("AltData").CurrentValue = DateTime.Now.Date;
                    entry.Property("AltHora").CurrentValue = DateTime.Now;
                    entry.Property("AltUsario").CurrentValue = usuarioLogado ?? "Pontofopag";
                }
            }
            return base.SaveChanges();
        }

        public PontofopagEntities(string host, string catalog, string user, string pass)
            : this(host, catalog, user, pass, null)
        {
        }

        public PontofopagEntities(string host, string catalog, string user, string pass, string usuarioLogado)
            : base(ConnectToSqlServer(host, catalog, user, pass))
        {
            this.usuarioLogado = usuarioLogado;
        }

        private static string ConnectToSqlServer(string dataSource, string initialCatalog, string user, string pass)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                InitialCatalog = initialCatalog,
                PersistSecurityInfo = true,
                IntegratedSecurity = false,
                MultipleActiveResultSets = true,
                ApplicationName = "RegistradorWeb",
                UserID = user,
                Password = pass,
            };

            //<add name="PontofopagEntities" connectionString="metadata=res://*/Models.Ponto.Pontofopag.csdl|res://*/Models.Ponto.Pontofopag.ssdl|res://*/Models.Ponto.Pontofopag.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=empvw02210\dev308;initial catalog=PONTOFOPAG_EMPLOYER_DEV;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework&quot;" providerName="System.Data.EntityClient" />
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = sqlBuilder.ConnectionString,
                Metadata = "res://*/Models.Ponto.Pontofopag.csdl|res://*/Models.Ponto.Pontofopag.ssdl|res://*/Models.Ponto.Pontofopag.msl",
            };
            string conn = entityConnectionStringBuilder.ConnectionString;

            return conn;
        }
    }
}