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
            var repo = new RepositoryFactory().CreateConfigurationRepository();
            return repo.GetRecentlyAddedConfiguraions();
        }

        [HttpGet]
        [Route("activate-server")]
        public string ActivateServer(int id)
        {
            try
            {
                var repo = new RepositoryFactory().CreateConfigurationRepository();
                repo.ActivateConfiguration(id);
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
                var repo = new RepositoryFactory().CreateConfigurationRepository();
                repo.AddConfiguration(config, isActive);

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
                var repo = new RepositoryFactory().CreateConfigurationRepository();
                repo.DeleteConfiguration(id);
                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to delete configuration. Please contact administrator";
            }
        }

    }
}
