namespace DBLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTestEntityTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblTestEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Age = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        Male = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblTestEntities");
        }
    }
}
