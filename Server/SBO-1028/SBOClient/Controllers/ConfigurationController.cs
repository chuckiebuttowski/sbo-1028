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
        [Route("test")]
        public string Test()
        {
            return "Hello world";
        }

        [HttpGet]
        [Route("get-recent-servers")]
        public IEnumerable<ServerConfig> GetRecentServers()
        {
            //var servers = new List<ServerConfig>();
            //servers.Add(new ServerConfig() { ServerName = "Server 1", DatabaseName = "DB1", Username = "User1", Password = "pass1" });
            //servers.Add(new ServerConfig() { ServerName = "Server 1", DatabaseName = "DB1", Username = "User1", Password = "pass1" });
            //servers.Add(new ServerConfig() { ServerName = "Server 1", DatabaseName = "DB1", Username = "User1", Password = "pass1", IsActive = true });
            //servers.Add(new ServerConfig() { ServerName = "Server 1", DatabaseName = "DB1", Username = "User1", Password = "pass1" });
            //servers.Add(new ServerConfig() { ServerName = "Server 1", DatabaseName = "DB1", Username = "User1", Password = "pass1" });
            //servers.Add(new ServerConfig() { ServerName = "Server 1", DatabaseName = "DB1", Username = "User1", Password = "pass1" });
            //return servers;

            var repo = new RepositoryFactory().CreateRepository<ServerConfig>();
            return repo.GetAll();
        }

        [HttpGet]
        [Route("get-active-server")]
        public ServerConfig GetActiveServer()
        {
            var repo = new RepositoryFactory().CreateRepository<ServerConfig>();
            return repo.Get(x => x.IsActive).FirstOrDefault();
        }

        [HttpPost]
        [Route("add")]
        public string AddConfiguration(ServerConfig config)
        {
            try
            {
                var repo = new RepositoryFactory().CreateRepository<ServerConfig>();
                repo.AddOrUpdate(config);

                return "Success";
            }
            catch (Exception ex)
            {
                //log error here
                return "Unable to save configuration. Please contact administrator.";
            }
        }

    }
}
