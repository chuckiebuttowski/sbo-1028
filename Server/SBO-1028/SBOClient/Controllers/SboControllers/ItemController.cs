using sbo.fx;
using sbo.fx.Factories;
using sbo.fx.Interfaces;
using sbo.fx.Models;
using SBOClient.Core.DAL.Entities;
using SBOClient.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SBOClient.Controllers.SboControllers
{

    /// <summary>
    /// This end point is for accessing and adding items.
    /// </summary>
    [RoutePrefix("api/items")]
    public class ItemController : ApiController
    {
        IItemRepository repo = new RepositoryFactory().ItemRepository();
        TransactionLogger transactionLogger;
        string errMsg;

        public ItemController()
        {
            repo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
            transactionLogger = new TransactionLogger();
        }

        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns>List of items</returns>
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

        /// <summary>
        /// Gets all items filtered by group code
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns>List of items</returns>
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

        /// <summary>
        /// Gets all item filtered by RD item code
        /// </summary>
        /// <param name="rdItemCode"></param>
        /// <returns>List of items</returns>
        [Route("get-items-by-rd-itemcode")]
        [HttpGet]
        public async Task<oItem> GetItemsByRDItemCode(string rdItemCode)
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

                return await repo.GetByRDItemCode(rdItemCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Get item filtered by item code
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns>Item</returns>
        [Route("get-items-by-itemcode")]
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

        /// <summary>
        /// Get item filtered by warehouse
        /// </summary>
        /// <param name="warehouse"></param>
        /// <returns>List of items with inventory columns</returns>
        [Route("get-items-by-warehouse")]
        [HttpGet]
        public async Task<List<oItemInventory>> GetItemsByWarehouse(string warehouse)
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

                return await repo.GetByWarehouse(warehouse);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Get items filtered by bin location
        /// </summary>
        /// <param name="binCode"></param>
        /// <returns>List of items</returns>
        [Route("get-items-by-bin")]
        [HttpGet]
        public async Task<List<oItemInventory>> GetItemsByBin(int binCode)
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

                return await repo.GetByBinLocation(binCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }


        /// <summary>
        /// Adds new item to SAP database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>If successful, returns status code 200 and item entry return message</returns>
        [Route("add-item")]
        [HttpPost]
        public async Task<object> AddItem(oItem item)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var list = await repo.GetList(null);
                var itm = list.FirstOrDefault(x => x.RDItemCode == item.RDItemCode);

                string validationStr = ModelValidator.ValidateModel(item);

                if (!string.IsNullOrEmpty(validationStr))
                {
                    errMsg = string.Format(validationStr);
                    ErrorLog _err = new ErrorLog();
                    _err.ErrorCode = (int)HttpStatusCode.Conflict;
                    _err.Message = errMsg;
                    _err.StackTrace = Environment.StackTrace;

                    var err = ErrorLogger.Log(_err);

                    transactionLogger.LogItemTransaction(item, false, "A", HttpContext.Current.Request.UserHostAddress, _err);
                    throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.Conflict, Content = new StringContent(errMsg) });
                }

                if (itm != null)
                {
                    errMsg = string.Format("Item {0} - {1} already exist.", itm.RDItemCode, itm.Description);
                    ErrorLog _err = new ErrorLog();
                    _err.ErrorCode = (int)HttpStatusCode.Conflict;
                    _err.Message = errMsg;
                    _err.StackTrace = Environment.StackTrace;

                    var err = ErrorLogger.Log(_err);

                    transactionLogger.LogItemTransaction(item, false, "A", HttpContext.Current.Request.UserHostAddress, _err);
                    throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.Conflict, Content = new StringContent(errMsg) });
                }

                if (repo.Add(item) < 0)
                {
                    errMsg = GlobalInstance.Instance.SBOErrorMessage;
                    ErrorLog _err = new ErrorLog();
                    _err.ErrorCode = (int)HttpStatusCode.Conflict;
                    _err.Message = errMsg;
                    _err.StackTrace = Environment.StackTrace;

                    var err = ErrorLogger.Log(_err);

                    transactionLogger.LogItemTransaction(item, false, "A", HttpContext.Current.Request.UserHostAddress, _err);
                    throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.Conflict, Content = new StringContent(errMsg) });
                }

                transactionLogger.LogItemTransaction(item, true, "A", HttpContext.Current.Request.UserHostAddress);
                var _itm = await repo.GetByRDItemCode(item.RDItemCode);

                return new { SAPItemCode = _itm.ItemCode, ReturnMessage = $"Item {item.RDItemCode} - {item.Description} successfully added." };

            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }

        /// <summary>
        /// Updates item from SAP database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>If successful, returns status code 200 and item entry return message</returns>
        [Route("update-item")]
        [HttpPut]
        public async Task<object> UpdateItem(oItem item)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var itm = await repo.GetItemByItemCode(item.ItemCode);

                string validationStr = ModelValidator.ValidateModel(item);

                if (!string.IsNullOrEmpty(validationStr))
                {
                    errMsg = string.Format(validationStr);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object property validation error";
                    var err = ErrorLogger.Log(new ErrorLog
                    {
                        ErrorCode = (int)HttpStatusCode.Conflict,
                        Message = errMsg,
                        StackTrace = Environment.StackTrace

                    });

                    transactionLogger.LogItemTransaction(item, false, "U", this.Request.Headers.Host, err);
                    throw new HttpResponseException(resp);
                }

                if (itm == null)
                {
                    errMsg = string.Format("Item {0} - {1} does not exist.", item.ItemCode, item.Description);
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object not found.";
                    var err = ErrorLogger.Log(new ErrorLog
                    {
                        ErrorCode = (int)HttpStatusCode.Conflict,
                        Message = errMsg,
                        StackTrace = Environment.StackTrace

                    });

                    transactionLogger.LogItemTransaction(item, false, "U", this.Request.Headers.Host, err);
                    throw new HttpResponseException(resp);
                }

                if (repo.Update(item) < 0)
                {
                    errMsg = GlobalInstance.Instance.SBOErrorMessage;
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "SBO Error";
                    var err = ErrorLogger.Log(new ErrorLog
                    {
                        ErrorCode = (int)HttpStatusCode.Conflict,
                        Message = errMsg,
                        StackTrace = Environment.StackTrace

                    });

                    transactionLogger.LogItemTransaction(item, false, "U", this.Request.Headers.Host, err);
                    throw new HttpResponseException(resp);
                }

                transactionLogger.LogItemTransaction(item, true, "U", this.Request.Headers.Host);
                return new { SAPItemCode = item.ItemCode, ReturnMessage = $"Item {item.ItemCode} - {item.Description} successfully added." };
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }

    }
}
