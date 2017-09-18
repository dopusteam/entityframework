namespace Dopusteam.EFR.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.Efr_Groups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Efr_Students",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        LastName = c.String(),
                        GroupId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("public.Efr_Groups", t => t.GroupId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "public.Efr_Projects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.Efr_StudentsProjects",
                c => new
                    {
                        studentId = c.Long(nullable: false),
                        projectId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.studentId, t.projectId })
                .ForeignKey("public.Efr_Students", t => t.studentId, cascadeDelete: true)
                .ForeignKey("public.Efr_Projects", t => t.projectId, cascadeDelete: true)
                .Index(t => t.studentId)
                .Index(t => t.projectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("public.Efr_StudentsProjects", "projectId", "public.Efr_Projects");
            DropForeignKey("public.Efr_StudentsProjects", "studentId", "public.Efr_Students");
            DropForeignKey("public.Efr_Students", "GroupId", "public.Efr_Groups");
            DropIndex("public.Efr_StudentsProjects", new[] { "projectId" });
            DropIndex("public.Efr_StudentsProjects", new[] { "studentId" });
            DropIndex("public.Efr_Students", new[] { "GroupId" });
            DropTable("public.Efr_StudentsProjects");
            DropTable("public.Efr_Projects");
            DropTable("public.Efr_Students");
            DropTable("public.Efr_Groups");
        }
    }
}
