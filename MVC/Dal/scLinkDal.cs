using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MVC.Models;

namespace MVC.Dal
{
    public class scLinkDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<scLink>().ToTable("scLink");
        }
        public DbSet<scLink> scLink { get; set; }
    }
}