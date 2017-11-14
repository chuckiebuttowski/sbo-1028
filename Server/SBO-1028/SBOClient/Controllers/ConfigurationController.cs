using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SBOClient.Controllers
{
    [Authorize]
    [RoutePrefix("api/configuration")]
    public class ConfigurationController : ApiController
    {
        [HttpGet]
        [Route("get-recent-servers")]
        public IEnumerable<ServerConfig> GetRecentServers()
        {
            var repo = new RepositoryFactory().CreateRepository<ServerConfig>();
            return repo.GetAll();
        }

        [HttpGet]
        [Route("activate-server")]
        public string ActivateServer(int id)
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

                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to update configuration. Please contact administrator.";
            }
        }

        [HttpPost]
        [Route("add")]
        public string AddConfiguration(ServerConfig config, bool isActive)
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

                return "Success";
            }
            catch (Exception ex)
            {
                //log error here
                return "Unable to save configuration. Please contact administrator.";
            }
        }

        [HttpGet]
        [Route("delete")]
        public string DeleteConfig(int id)
        {
            try
            {
                var repo = new RepositoryFactory().CreateRepository<ServerConfig>();
                repo.Delete(repo.Get(x => x.ID == id).FirstOrDefault());

                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to delete configuration. Please contact administrator";
            }
        }

    }
}
