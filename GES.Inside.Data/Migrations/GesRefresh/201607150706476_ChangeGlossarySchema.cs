namespace GES.Inside.Data.Migrations.GesRefresh
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGlossarySchema : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.Glossaries", newSchema: "ges");
        }
        
        public override void Down()
        {
            MoveTable(name: "ges.Glossaries", newSchema: "dbo");
        }
    }
}
