using SBOClient.Core.Abstracts;
using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.Repositories
{
    internal class UserRepository : BaseRepository<User>, IUserRepository
    {
        public User GetUser(string username, string password)
        {
            User user = null;

            var repo = new RepositoryFactory().CreateGenericRepository<User>();
            Task.Run(async () => {
                user = await repo.GetFirstAsync(x => x.Username == username && x.Password == password && x.IsActive);    
            }).Wait();

            return user;
        }
    }
}
