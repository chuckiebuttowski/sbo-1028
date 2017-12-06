using sbo.fx;
using sbo.fx.Factories;
using sbo.fx.Interfaces;
using sbo.fx.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SBOClient.Controllers.SboControllers
{
    /// <summary>
    /// This end point is for accessing business partner group from SAP
    /// </summary>
    [RoutePrefix("api/bp-groups")]
    public class BusinessPartnerGroupController : ApiController
    {
        IRepository<oBusinessPartnerGroup> repo = new RepositoryFactory().BusinessPartnerGroupRepository();
        string errMsg = "";

        public BusinessPartnerGroupController()
        {
            repo.InitRepository(GlobalInstance.Instance.SqlObject);
        }

        /// <summary>
        /// Gets all business partner groups
        /// </summary>
        /// <returns>List of Business Partner Groups</returns>
        [Route("get-bp-groups")]
        [HttpGet]
        public async Task<List<oBusinessPartnerGroup>> GetBpGroups()
        {
            try
            {
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed)
                {
                    errMsg = "Unable to connect to server.";
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "No Server Connection";
                    throw new HttpResponseException(resp);
                }

                return await repo.GetList(null);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets business partner group filtered by group code
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns>Business Partner Group</returns>
        [Route("get-bp-group")]
        [HttpGet]
        public async Task<oBusinessPartnerGroup> GetBpGroup(int groupCode)
        {
            try
            {
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed)
                {
                    errMsg = "Unable to connect to server.";
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "No Server Connection";
                    throw new HttpResponseException(resp);
                }

                var list = await repo.GetList(null);
                return list.FirstOrDefault(x => x.GroupCode == groupCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
