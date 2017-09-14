namespace Dopusteam.EFR.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchemaNameChanged : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.Efr_Groups", newSchema: "public");
            MoveTable(name: "dbo.Efr_Students", newSchema: "public");
            MoveTable(name: "dbo.Efr_Projects", newSchema: "public");
            MoveTable(name: "dbo.Efr_StudentsProjects", newSchema: "public");
        }
        
        public override void Down()
        {
            MoveTable(name: "public.Efr_StudentsProjects", newSchema: "dbo");
            MoveTable(name: "public.Efr_Projects", newSchema: "dbo");
            MoveTable(name: "public.Efr_Students", newSchema: "dbo");
            MoveTable(name: "public.Efr_Groups", newSchema: "dbo");
        }
    }
}
