namespace SBOClient.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServerConfigs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ServerName = c.String(),
                        DatabaseName = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedOn = c.DateTime(nullable: false),
                        Remarks = c.String(),
                        RecordStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(),
                        Username = c.String(),
                        Password = c.String(),
                        Image = c.Binary(),
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
            DropTable("dbo.Users");
            DropTable("dbo.ServerConfigs");
        }
    }
}
