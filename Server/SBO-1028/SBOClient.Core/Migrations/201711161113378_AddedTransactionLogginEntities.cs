namespace SBOClient.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTransactionLogginEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ErrorCode = c.Int(nullable: false),
                        Message = c.String(),
                        StackTrace = c.String(),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedOn = c.DateTime(nullable: false),
                        Remarks = c.String(),
                        RecordStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TransactionDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PostedOn = c.DateTime(nullable: false),
                        RawData = c.String(),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedOn = c.DateTime(nullable: false),
                        Remarks = c.String(),
                        RecordStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TransactionLogs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Origin = c.String(),
                        TransactionNo = c.String(),
                        Type = c.Int(nullable: false),
                        LogDate = c.DateTime(nullable: false),
                        IsPosted = c.Boolean(nullable: false),
                        TransactionDataID = c.Int(nullable: false),
                        ErrorID = c.Int(),
                        CreatedBy = c.String(),
                        CreatedOn = c.DateTime(nullable: false),
                        LastModifiedBy = c.String(),
                        LastModifiedOn = c.DateTime(nullable: false),
                        Remarks = c.String(),
                        RecordStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ErrorLogs", t => t.ErrorID)
                .ForeignKey("dbo.TransactionDatas", t => t.TransactionDataID, cascadeDelete: true)
                .Index(t => t.TransactionDataID)
                .Index(t => t.ErrorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransactionLogs", "TransactionDataID", "dbo.TransactionDatas");
            DropForeignKey("dbo.TransactionLogs", "ErrorID", "dbo.ErrorLogs");
            DropIndex("dbo.TransactionLogs", new[] { "ErrorID" });
            DropIndex("dbo.TransactionLogs", new[] { "TransactionDataID" });
            DropTable("dbo.TransactionLogs");
            DropTable("dbo.TransactionDatas");
            DropTable("dbo.ErrorLogs");
        }
    }
}
