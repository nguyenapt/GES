namespace GES.Inside.Data.Migrations.GesRefresh
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGlosarriesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Glossaries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CategoryId = c.Guid(nullable: false),
                        Slug = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Glossaries");
        }
    }
}
