using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApiTestApp.Models
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("AdventureWorks") { }

        public virtual IDbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ProductContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}