namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_attributes_to_entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attributes", "EntityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Attributes", "EntityId");
            AddForeignKey("dbo.Attributes", "EntityId", "dbo.Entities", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attributes", "EntityId", "dbo.Entities");
            DropIndex("dbo.Attributes", new[] { "EntityId" });
            DropColumn("dbo.Attributes", "EntityId");
        }
    }
}
