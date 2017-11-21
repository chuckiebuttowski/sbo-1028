namespace SBOClient.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClientAPIEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientAPIs", "ValueType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientAPIs", "ValueType");
        }
    }
}
