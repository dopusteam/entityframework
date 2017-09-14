using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.History;
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
            base.OnModelCreating(modelBuilder);

            var schemaName = "public";

            // задаём название схемы БД
            modelBuilder.HasDefaultSchema(schemaName);

            // Настраиваем БД
            // указываем название таблицы для хранение студентов
            // это же можно сделать используя атрибуты в классе с сущностью
            modelBuilder.Entity<Student>()
                .ToTable("Efr_Students");

            modelBuilder.Entity<Group>()
                .ToTable("Efr_Groups");

            modelBuilder.Entity<Project>()
                .ToTable("Efr_Projects");

            // Настриваем связи между сущностями (many-to-many)
            // сущность студент...
            modelBuilder.Entity<Student>()
                // имеет опциональную связь с таблицей групп...
                .HasOptional(student => student.Group)
                // каждя из которых сожерижт много студентов
                .WithMany(group => group.Students);

            modelBuilder.Entity<Student>()
                .HasMany(student => student.Projects)
                .WithMany(project => project.Students)
                .Map(table => table
                    .MapLeftKey("studentId")
                    .MapRightKey("projectId")
                    .ToTable("Efr_StudentsProjects"));
        }
    }
}
