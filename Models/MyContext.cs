using System;
using Microsoft.EntityFrameworkCore;

namespace Deborah.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options)
        {

        }

        //複合キー設定
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mst_User>()
            .HasKey(c => new {c.Id, c.Login_Id});
        }
        public DbSet<Mst_System> Mst_System { get; set; }
        public DbSet<Mst_Status> Mst_Status { get; set; }
        public DbSet<Mst_Communication> Mst_Communication { get; set; }
        public DbSet<Mst_Type> Mst_Type { get; set; }
        public DbSet<Mst_User> Mst_User { get; set; }
        public DbSet<Tra_Entry> Tra_Entry { get; set; }
        public DbSet<Tra_Inqury> Tra_Inqury { get; set; }
        public DbSet<Mst_Download> Mst_Download { get; set; }
    }
}