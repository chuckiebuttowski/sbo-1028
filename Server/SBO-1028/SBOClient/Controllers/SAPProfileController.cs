using sbo.fx;
using SBOClient.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SBOClient.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [RoutePrefix("api/sap-profile")]
    public class SAPProfileController : ApiController
    {
        [HttpGet]
        [Route("get-profile")]
        public SAPProfile GetSAPProfile()
        {
            //get it from where chuckie put this one
            return new SAPProfile() { UserId = ConfigurationManager.AppSettings["uid"], Password = ConfigurationManager.AppSettings["pword"] };//sample data
        }

        [HttpPost]
        [Route("update-profile")]
        public string UpdateSAPProfile(SAPProfile profile)
        {
            try
            {
                ConfigurationManager.AppSettings["uid"] = profile.UserId;
                ConfigurationManager.AppSettings["pword"] = profile.Password;
                return "Success";
            }
            catch (Exception)
            {
                return "Unable to update profile. Please contact administrator.";
            }
        }

        [HttpPost]
        [Route("test-sap-connection")]
        public string TestSapConnection(SAPProfile profile)
        {
            try
            {
                 string returnStr = "Success";

                //var repo = new RepositoryFactory().CreateConfigurationRepository();
                //ServerConfig serverConfig = repo.Get(x => x.IsActive == true).FirstOrDefault();

                GlobalInstance.Instance.UName = profile.UserId;
                GlobalInstance.Instance.Pword = profile.Password;

                GlobalInstance.Instance.DisposeSboComObject();
                GlobalInstance.Instance.InitializeSboComObject();

                if (!GlobalInstance.Instance.IsConnected) returnStr = GlobalInstance.Instance.SBOErrorMessage;

                return returnStr;
            }
            catch (Exception ex)
            {
                //log error here
                return $"Unable to test profile. {ex.Message} Please contact administrator.";
            }
        }
    }
}
