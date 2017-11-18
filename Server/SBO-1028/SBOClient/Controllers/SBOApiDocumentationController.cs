using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SBOClient.Controllers
{
    [Authorize]
    [RoutePrefix("api/sbo-api-docs")]
    public class SBOApiDocumentationController : ApiController
    {
        public SBOApiDocumentationController() : this(GlobalConfiguration.Configuration)
        {

        }

        public SBOApiDocumentationController(HttpConfiguration config)
        {

        }

        [HttpGet]
        [Route("explore-sbo-apis")]
        public IApiExplorer ExploreSBOApis()
        {
            return Configuration.Services.GetApiExplorer();
        }
    }
}
