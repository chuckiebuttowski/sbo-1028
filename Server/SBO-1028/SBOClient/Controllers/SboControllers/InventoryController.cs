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
    [RoutePrefix("api/inventory")]
    public class InventoryController : ApiController
    {
        IInventoryTransactionRepository repo;
        string errMsg = "";

        public InventoryController()
        {
            repo = new RepositoryFactory().InventoryTransactionRepository();
            repo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
        }

        [Route("get-inventory-transactions")]
        [HttpGet]
        public async Task<IList<oInventoryTransaction>> GetInventoryTransactions(InventoryType invType)
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

                return await repo.GetTransactionList(null, invType);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("get-inventory-transactions-by-date-range")]
        [HttpGet]
        public async Task<IList<oInventoryTransaction>> GetInventoryTransactionsByDateRange(DateTime from, DateTime to, InventoryType invType)
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

                return await repo.GetTransactionByDateRange(from, to, invType);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("get-inventory-transaction-by-docno")]
        [HttpGet]
        public async Task<oInventoryTransaction> GetInventoryTransactionByDocNo(int docNo, InventoryType invType)
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

                return await repo.GetTransactionByDocNo(docNo, invType);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task<IHttpActionResult> AddInventory(oInventoryTransaction transaction)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var t = await repo.GetTransactionByDocNo(transaction.DocNum, transaction.InventoryTransactionType);
                if (t != null)
                {
                    errMsg = string.Format("Inventory transaction document {0} already exist.", transaction.DocNum);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object already exist.";
                    throw new HttpResponseException(resp);
                }

                if (repo.Add(transaction) < 0)
                {
                    errMsg = GlobalInstance.Instance.SBOErrorMessage;
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "SBO Error";
                    throw new HttpResponseException(resp);
                }

                return Ok(string.Format("Inventory {0} succesful: document no.: {1}", transaction.InventoryTransactionType.ToString(), transaction.DocNum));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
