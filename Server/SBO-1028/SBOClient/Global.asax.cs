using Newtonsoft.Json;
using sbo.fx;
using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace SBOClient
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.Formatters
                   .JsonFormatter.SerializerSettings.Re‌​ferenceLoopHandling
                   = ReferenceLoopHandling.Ignore;

            InitializeSbo();
        }

        private void InitializeSbo()
        {
            var repo = new RepositoryFactory().CreateConfigurationRepository();
            ServerConfig serverConfig = repo.Get(x => x.IsActive == true).FirstOrDefault();

            if (serverConfig != null)
            {
                GlobalInstance.Instance.DatabaseServerType = DBType.MSSQL2012;
                GlobalInstance.Instance.Server = serverConfig.ServerName;
                GlobalInstance.Instance.DBName = serverConfig.DatabaseName;
                GlobalInstance.Instance.DBUName = serverConfig.Username;
                GlobalInstance.Instance.DBPword = serverConfig.Password;
                GlobalInstance.Instance.UName = ConfigurationManager.AppSettings["uid"];
                GlobalInstance.Instance.Pword = ConfigurationManager.AppSettings["pword"];
                GlobalInstance.Instance.DefaultWarehouse = ConfigurationManager.AppSettings["DefaultWarehouse"];
                GlobalInstance.Instance.DefaultBinLocation = Int32.Parse(ConfigurationManager.AppSettings["DefaultBin"]);
                GlobalInstance.Instance.DefaultBranch = ConfigurationManager.AppSettings["DefaultBranch"];

                GlobalInstance.Instance.InitializeSboComObject();
                GlobalInstance.Instance.InitializeSqlObject();
            }
        }
    }
}
