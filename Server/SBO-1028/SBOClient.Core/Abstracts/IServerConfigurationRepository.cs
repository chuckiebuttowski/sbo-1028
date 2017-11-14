using SBOClient.Core.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBOClient.Core.Abstracts
{
    public interface IServerConfigurationRepository : IRepository<ServerConfig>
    {
        void ActivateConfiguration(int id);
        IEnumerable<ServerConfig> GetRecentlyAddedConfiguraions();
        void AddConfiguration(ServerConfig config, bool isActive);
        void DeleteConfiguration(int id);
    }
}
