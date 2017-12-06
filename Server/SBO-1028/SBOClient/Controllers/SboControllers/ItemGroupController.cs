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
    /// This end point is for accessing item groups from SAP 
    /// </summary>
    [RoutePrefix("api/item-groups")]
    public class ItemGroupController : ApiController
    {
        IRepository<oItemGroup> repo = new RepositoryFactory().ItemGroupRepository();
        string errMsg = "";

        public ItemGroupController()
        {
            repo.InitRepository(GlobalInstance.Instance.SqlObject);
        }

        /// <summary>
        /// Gets all item groups
        /// </summary>
        /// <returns>List of Item Groups</returns>
        [Route("get-item-groups")]
        [HttpGet]
        public async Task<List<oItemGroup>> GetItemGroups()
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
        /// Gets an item group
        /// </summary>
        /// <param name="itemGroupCode"></param>
        /// <returns>Item Group</returns>
        [Route("get-item-group")]
        [HttpGet]
        public async Task<oItemGroup> GetItemGroups(int itemGroupCode)
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
                return list.FirstOrDefault(x=> x.GroupCode == itemGroupCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
