namespace Dopusteam.EFR.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectsAndGroupsAreAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Efr_Groups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Number = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Efr_Projects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Efr_StudentsProjects",
                c => new
                    {
                        studentId = c.Long(nullable: false),
                        projectId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.studentId, t.projectId })
                .ForeignKey("dbo.Efr_Students", t => t.studentId, cascadeDelete: true)
                .ForeignKey("dbo.Efr_Projects", t => t.projectId, cascadeDelete: true)
                .Index(t => t.studentId)
                .Index(t => t.projectId);
            
            AddColumn("dbo.Efr_Students", "GroupId", c => c.Long());
            CreateIndex("dbo.Efr_Students", "GroupId");
            AddForeignKey("dbo.Efr_Students", "GroupId", "dbo.Efr_Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Efr_StudentsProjects", "projectId", "dbo.Efr_Projects");
            DropForeignKey("dbo.Efr_StudentsProjects", "studentId", "dbo.Efr_Students");
            DropForeignKey("dbo.Efr_Students", "GroupId", "dbo.Efr_Groups");
            DropIndex("dbo.Efr_StudentsProjects", new[] { "projectId" });
            DropIndex("dbo.Efr_StudentsProjects", new[] { "studentId" });
            DropIndex("dbo.Efr_Students", new[] { "GroupId" });
            DropColumn("dbo.Efr_Students", "GroupId");
            DropTable("dbo.Efr_StudentsProjects");
            DropTable("dbo.Efr_Projects");
            DropTable("dbo.Efr_Groups");
        }
    }
}
