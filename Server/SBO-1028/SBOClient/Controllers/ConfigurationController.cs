using sbo.fx;
using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SBOClient.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
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
        [Route("test-server-connection")]
        public string TestServerConnection()
        {
            try
            {
                var repo = new RepositoryFactory().CreateConfigurationRepository();
                ServerConfig serverConfig = repo.Get(x => x.IsActive == true).FirstOrDefault();

                GlobalInstance.Instance.DatabaseServerType = DBType.MSSQL2012;
                GlobalInstance.Instance.Server = serverConfig.ServerName;
                GlobalInstance.Instance.DBName = serverConfig.DatabaseName;
                GlobalInstance.Instance.DBUName = serverConfig.Username;
                GlobalInstance.Instance.DBPword = serverConfig.Password;

                GlobalInstance.Instance.DisposeSqlObject();
                
                GlobalInstance.Instance.InitializeSqlObject();

                return "Success";
            }
            catch (SqlException ex)
            {
                //log error here
                return ex.Message;
            }
        }

        [HttpGet]
        [Route("test-sap-connection")]
        public string TestSapConnection()
        {
            try
            {
                string returnStr = "Success";

                var repo = new RepositoryFactory().CreateConfigurationRepository();
                ServerConfig serverConfig = repo.Get(x => x.IsActive == true).FirstOrDefault();

                GlobalInstance.Instance.UName = ConfigurationManager.AppSettings["uid"];
                GlobalInstance.Instance.Pword = ConfigurationManager.AppSettings["pword"];

                GlobalInstance.Instance.DisposeSboComObject();
                GlobalInstance.Instance.InitializeSboComObject();

                if (!GlobalInstance.Instance.IsConnected) returnStr = GlobalInstance.Instance.SBOErrorMessage;

                return returnStr;
            }
            catch (Exception ex)
            {
                //log error here
                return "Unable to test configuration. Please contact administrator.";
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
