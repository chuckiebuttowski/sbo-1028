using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SBOClient.Controllers
{
    /// <summary>
    /// Test Documentation
    /// </summary>
    [Authorize]
    [RoutePrefix("api/client-api")]
    public class ClientAPIController : ApiController
    {
        /// <summary>
        /// Get all client APIs stored in the database
        /// </summary>
        /// <returns>List of Client APIs</returns>
        [HttpGet]
        [Route("get-all")]
        [ResponseType(typeof(List<ClientAPI>))]
        public IEnumerable<ClientAPI> GetAllClientAPIs()
        {
            var repo = new RepositoryFactory().CreateClientAPIRepository();
            return repo.GetAll();
        }

        [HttpPost]
        [Route("save-api")]
        public string SaveAPI(ClientAPI api)
        {
            try
            {
                var repo = new RepositoryFactory().CreateClientAPIRepository();
                repo.AddOrUpdate(api);

                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to save new API. Please contact administrator.";
            }
        }

        [HttpGet]
        [Route("delete-api")]
        public string DeleteAPI(int id)
        {
            try
            {
                var repo = new RepositoryFactory().CreateClientAPIRepository();
                repo.DeleteApi(id);

                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to delete api. Please contact administrator.";
            }
        }
    }
}
