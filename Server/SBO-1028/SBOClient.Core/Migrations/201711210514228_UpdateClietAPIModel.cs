namespace SBOClient.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateClietAPIModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientAPIs", "Action", c => c.String());
            AddColumn("dbo.ClientAPIs", "SboTransactionType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClientAPIs", "SboTransactionType");
            DropColumn("dbo.ClientAPIs", "Action");
        }
    }
}
