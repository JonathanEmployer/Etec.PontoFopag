using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PontoWeb.Models
{
    public class PontoWebContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public PontoWebContext() : base("name=PontoWebContext")
        {
        }

        public System.Data.Entity.DbSet<Modelo.REP> REP { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<PontoWeb.Models.funcao> funcaos { get; set; }

        public System.Data.Entity.DbSet<PontoWeb.Models.departamento> departamentoes { get; set; }
    
    }
}
