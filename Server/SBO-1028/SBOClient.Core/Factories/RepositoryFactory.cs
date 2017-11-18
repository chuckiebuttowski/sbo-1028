using SBOClient.Core.Abstracts;
using SBOClient.Core.DAL;
using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.Factories
{
    public class RepositoryFactory
    {
        protected DbContext db = null;

        public RepositoryFactory()
        {
            db = new SBOIntegrationDataContext();
        }

        public RepositoryFactory(DbContext context)
        {
            db = context;
        }

        public IUserRepository CreateUserRepository()
        {
            return new UserRepository();
        }

        public IServerConfigurationRepository CreateConfigurationRepository()
        {
            return new ServerConfigurationRepository();
        }

        public ITransactionLogRepository CreateTransactionLogRepository()
        {
            return new TransactionLogRepository();
        }

        public IClientAPIRepository CreateClientAPIRepository()
        {
            return new ClientAPIRepository();
        }

        public IRepository<T> CreateRepository<T>() where T : BaseEntity
        {
            return new BaseRepository<T>();
        }

        internal GenericRepository<T> CreateGenericRepository<T>() where T : BaseEntity
        {
            return new GenericRepository<T>(db);
        }
    }
}
