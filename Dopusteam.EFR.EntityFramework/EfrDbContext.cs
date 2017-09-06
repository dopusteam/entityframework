using System.Data.Entity;
using Dopusteam.EFR.Core.Entities;

namespace Dopusteam.EFR.EntityFramework
{
    public class EfrDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public EfrDbContext() : base("DbConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .ToTable("Efr_Students");

            base.OnModelCreating(modelBuilder);
        }
    }
}
