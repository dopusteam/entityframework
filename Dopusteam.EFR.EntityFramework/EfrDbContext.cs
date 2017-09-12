using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using Dopusteam.EFR.Core.Entities;

namespace Dopusteam.EFR.EntityFramework
{
    public class EfrDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Project> Projects { get; set; }

        public EfrDbContext() : base("DbConnection")
        {
            //Database.SetInitializer(new EfrDbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .ToTable("Efr_Students");

            modelBuilder.Entity<Group>()
                .ToTable("Efr_Groups");

            modelBuilder.Entity<Project>()
                .ToTable("Efr_Projects")
                .Property(p => p.Name);

            modelBuilder.Entity<Student>()
                .HasOptional(student => student.Group)
                .WithMany(group => group.Students);

            modelBuilder.Entity<Student>()
                .HasMany(student => student.Projects)
                .WithMany(project => project.Students)
                .Map(table => table
                    .MapLeftKey("studentId")
                    .MapRightKey("projectId")
                    .ToTable("Efr_StudentsProjects"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
