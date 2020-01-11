using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MVC.Models;

namespace MVC.Dal
{
    public class CourseDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().ToTable("Courses");
        }
        public DbSet<Course> Course { get; set; }
    }
}