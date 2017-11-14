using SBOClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SBOClient.Controllers
{
    [Authorize]
    [RoutePrefix("api/sap-profile")]
    public class SAPProfileController : ApiController
    {
        [HttpGet]
        [Route("get-profile")]
        public SAPProfile GetSAPProfile()
        {
            //get it from where chuckie put this one
            return new SAPProfile() { UserId = "SAP123435435", Password = "admin123" };//sample data
        }

        [HttpPost]
        [Route("update-profile")]
        public string UpdateSAPProfile(SAPProfile profile)
        {
            try
            {
                return "Success";
            }
            catch (Exception)
            {
                return "Unable to update profile. Please contact administrator.";
            }
        }
    }
}
