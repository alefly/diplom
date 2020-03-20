namespace Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attribute_TimeSeriesPointAttribute",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AttributeId = c.Int(nullable: false),
                        TimeSeriesPointAttributeId = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Attributes", t => t.AttributeId, cascadeDelete: true)
                .ForeignKey("dbo.TimeSeriesPointAttributes", t => t.TimeSeriesPointAttributeId, cascadeDelete: true)
                .Index(t => t.AttributeId)
                .Index(t => t.TimeSeriesPointAttributeId);
            
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DBs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Server = c.String(),
                        Port = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                        isMonitored = c.Boolean(nullable: false),
                        Timer = c.Int(nullable: false),
                        LastCheck = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Entities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DBId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DBs", t => t.DBId, cascadeDelete: true)
                .Index(t => t.DBId);
            
            CreateTable(
                "dbo.TimeSeriesPointEntities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Value = c.Int(nullable: false),
                        EntityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entities", t => t.EntityId, cascadeDelete: true)
                .Index(t => t.EntityId);
            
            CreateTable(
                "dbo.TimeSeriesPointAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attribute_TimeSeriesPointAttribute", "TimeSeriesPointAttributeId", "dbo.TimeSeriesPointAttributes");
            DropForeignKey("dbo.Entities", "DBId", "dbo.DBs");
            DropForeignKey("dbo.TimeSeriesPointEntities", "EntityId", "dbo.Entities");
            DropForeignKey("dbo.Attribute_TimeSeriesPointAttribute", "AttributeId", "dbo.Attributes");
            DropIndex("dbo.Attribute_TimeSeriesPointAttribute", new[] { "TimeSeriesPointAttributeId" });
            DropIndex("dbo.Entities", new[] { "DBId" });
            DropIndex("dbo.TimeSeriesPointEntities", new[] { "EntityId" });
            DropIndex("dbo.Attribute_TimeSeriesPointAttribute", new[] { "AttributeId" });
            DropTable("dbo.TimeSeriesPointAttributes");
            DropTable("dbo.TimeSeriesPointEntities");
            DropTable("dbo.Entities");
            DropTable("dbo.DBs");
            DropTable("dbo.Attributes");
            DropTable("dbo.Attribute_TimeSeriesPointAttribute");
        }
    }
}
