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
    internal class ServerConfigurationRepository : BaseRepository<ServerConfig>, IServerConfigurationRepository
    {
        public void ActivateConfiguration(int id)
        {
            try
            {
                var repo = new RepositoryFactory().CreateRepository<ServerConfig>();

                ServerConfig config = null;

                //deactivate currently activated server
                config = repo.Get(x => x.IsActive).FirstOrDefault();
                if (config != null)
                {
                    config.IsActive = false;
                    repo.AddOrUpdate(config);
                }

                //activate server
                config = repo.Get(x => x.ID == id).FirstOrDefault();
                config.IsActive = true;
                repo.AddOrUpdate(config);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddConfiguration(ServerConfig config, bool isActive)
        {
            try
            {
                var repo = new RepositoryFactory().CreateRepository<ServerConfig>();
                if (isActive)
                {
                    config.IsActive = true;
                    var deactivateConfig = repo.Get(x => x.IsActive).FirstOrDefault();
                    if (deactivateConfig != null)
                    {
                        deactivateConfig.IsActive = false;
                        repo.AddOrUpdate(deactivateConfig);
                    }
                }

                repo.AddOrUpdate(config);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteConfiguration(int id)
        {
            try
            {
                var repo = new RepositoryFactory().CreateRepository<ServerConfig>();
                repo.Delete(repo.Get(x => x.ID == id).FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<ServerConfig> GetRecentlyAddedConfiguraions()
        {
            try
            {
                var repo = new RepositoryFactory().CreateRepository<ServerConfig>();
                return repo.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
