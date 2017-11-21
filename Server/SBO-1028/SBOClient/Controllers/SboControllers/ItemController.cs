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
    [RoutePrefix("api/items")]
    public class ItemController : ApiController
    {
        IItemRepository repo = new RepositoryFactory().ItemRepository();
        string errMsg;

        public ItemController()
        {
            repo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
        }

        [Route("get-items")]
        [HttpGet]
        public async Task<IList<oItem>> GetItems()
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

        [Route("get-items-by-group")]
        [HttpGet]
        public async Task<IList<oItem>> GetItemsByGroup(int groupCode)
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

                return await repo.GetByItemGroup(groupCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("get-items-by-series")]
        [HttpGet]
        public async Task<IList<oItem>> GetItemsBySeries(int series)
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

                return await repo.GetByItemSeries(series);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("get-items-by-series")]
        [HttpGet]
        public async Task<oItem> GetItemsByItemCode(string itemCode)
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

                return await repo.GetItemByItemCode(itemCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("add-item")]
        [HttpPost]
        public async Task<IHttpActionResult> AddItem(oItem item)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();

                var itm = await repo.GetItemByItemCode(item.ItemCode);
                if (itm != null)
                {
                    errMsg = string.Format("Item {0} - {2} already exist.", item.ItemCode, item.Description);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object already exist.";
                    throw new HttpResponseException(resp);
                }

                if (repo.Add(item) < 0)
                {
                    errMsg = GlobalInstance.Instance.SBOErrorMessage;
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "SBO Error";
                    throw new HttpResponseException(resp);
                }

                return Ok(string.Format("Item {0} - {2} successfully added.", item.ItemCode, item.Description));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("add-multiple-items")]
        [HttpPost]
        public async Task<IHttpActionResult> AddMultipleItems(List<oItem> items)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();

                if (repo.AddMultiple(items) < 0)
                {
                    errMsg = GlobalInstance.Instance.SBOErrorMessage;
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "SBO Error";
                    throw new HttpResponseException(resp);
                }

                return Ok("Successfully added items.");
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("update-item")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateItem(oItem item)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var itm = await repo.GetItemByItemCode(item.ItemCode);
                if (itm == null)
                {
                    errMsg = string.Format("Item {0} - {2} does not exist.", item.ItemCode, item.Description);
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object not found.";
                    throw new HttpResponseException(resp);
                }

                if (repo.Update(item) < 0)
                {
                    errMsg = GlobalInstance.Instance.SBOErrorMessage;
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "SBO Error";
                    throw new HttpResponseException(resp);
                }

                return Ok(string.Format("Item {0} - {2} successfully updated.", item.ItemCode, item.Description));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

    }
}
