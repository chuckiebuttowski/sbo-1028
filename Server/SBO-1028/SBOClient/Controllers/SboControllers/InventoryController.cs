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
using System.Web.Http;

namespace SBOClient.Controllers.SboControllers
{
    /// <summary>
    /// This end point is for accessing and adding inventory in and out transactions
    /// </summary>
    [RoutePrefix("api/inventory")]
    public class InventoryController : ApiController
    {
        IInventoryTransactionRepository repo;
        TransactionLogger transactionLogger;
        string errMsg = "";

        public InventoryController()
        {
            repo = new RepositoryFactory().InventoryTransactionRepository();
            repo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
            transactionLogger = new TransactionLogger();
        }

        /// <summary>
        /// Get all inventory transaction filtered by inventory type(i.e. inventory in or inventory out)
        /// </summary>
        /// <param name="invType"></param>
        /// <returns>List of inventory transactions</returns>
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


        /// <summary>
        /// Get all inventory transaction filtered by date range and inventory type(i.e. inventory in or inventory out)
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="invType"></param>
        /// <returns>List of inventory transactions</returns>
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

        /// <summary>
        /// Get inventory transaction filtered by document no and inventory type(i.e. inventory in or inventory out)
        /// </summary>
        /// <param name="docNo"></param>
        /// <param name="invType"></param>
        /// <returns>Inventory transaction</returns>
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

        /// <summary>
        /// Adds new inventory transaction to SAP database.
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>If successful, returns status code 200 and inventory entry return message</returns>
        [Route("add-inventory")]
        [HttpPost]
        public async Task<IHttpActionResult> AddInventory(oInventoryTransaction transaction)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var t = await repo.GetTransactionByDocNo(transaction.DocNum, transaction.InventoryTransactionType);

                string validationStr = ModelValidator.ValidateModel(transaction);

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

                    transactionLogger.LogInventoryTransaction(transaction, false, "A", err);
                    throw new HttpResponseException(resp);
                }

                if (t != null)
                {
                    errMsg = string.Format("Inventory transaction document {0} already exist.", transaction.DocNum);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object already exist.";
                    var err = ErrorLogger.Log(new ErrorLog
                    {
                        ErrorCode = (int)HttpStatusCode.Conflict,
                        Message = errMsg,
                        StackTrace = Environment.StackTrace

                    });

                    transactionLogger.LogInventoryTransaction(transaction, false, "A", err);
                    throw new HttpResponseException(resp);
                }

                if (repo.Add(transaction) < 0)
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

                    transactionLogger.LogInventoryTransaction(transaction, false, "A", err);
                    throw new HttpResponseException(resp);
                }

                transactionLogger.LogInventoryTransaction(transaction, true, "A");
                return Ok(string.Format("Inventory {0} succesful: document no.: {1}", transaction.InventoryTransactionType.ToString(), transaction.DocNum));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
