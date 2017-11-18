namespace SBOClient.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedClientAPIEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.APIParameters",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        ApiID = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedOn = c.DateTime(nullable: false),
                        Remarks = c.String(),
                        RecordStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ClientAPIs", t => t.ApiID, cascadeDelete: true)
                .Index(t => t.ApiID);
            
            CreateTable(
                "dbo.ClientAPIs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        URL = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedOn = c.DateTime(nullable: false),
                        Remarks = c.String(),
                        RecordStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.APIParameters", "ApiID", "dbo.ClientAPIs");
            DropIndex("dbo.APIParameters", new[] { "ApiID" });
            DropTable("dbo.ClientAPIs");
            DropTable("dbo.APIParameters");
        }
    }
}
