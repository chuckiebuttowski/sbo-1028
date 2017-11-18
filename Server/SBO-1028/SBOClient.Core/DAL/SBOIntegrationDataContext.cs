namespace SBOClient.Core.DAL
{
    using SBOClient.Core.DAL.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SBOIntegrationDataContext : DbContext
    {
        // Your context has been configured to use a 'SBOIntegrationDataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'SBOClient.Core.DAL.SBOIntegrationDataContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SBOIntegrationDataContext' 
        // connection string in the application configuration file.
        public SBOIntegrationDataContext()
            : base("name=SBOIntegrationDataContext")
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ServerConfig> SAPServerConfigurations { get; set; }
        public virtual DbSet<TransactionLog> TransactionLogs { get; set; }
        public virtual DbSet<TransactionData> TransactionData { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<ClientAPI> ClientAPIs { get; set; }
        public virtual DbSet<APIParameter> ApiParameters { get; set; }
    }
}