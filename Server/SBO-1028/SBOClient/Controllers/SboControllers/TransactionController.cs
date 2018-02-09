using sbo.fx;
using sbo.fx.Factories;
using sbo.fx.Interfaces;
using sbo.fx.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SBOClient.Controllers.SboControllers
{
    [RoutePrefix("api/transactions")]
    public class TransactionController : ApiController
    {
        ITransactionRepository repo = new RepositoryFactory().TransactionRepository();
        string errMsg = "";

        public TransactionController()
        {
            repo.InitRepository(GlobalInstance.Instance.SqlObject);
        }

        /// <summary>
        /// Gets all transactions
        /// </summary>
        /// <returns>List of Transaction</returns>
        [Route("get-all-transactions")]
        [HttpGet]
        public async Task<List<oTransaction>> GetAllTransactions()
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

                return await repo.GetAllTransactions();
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }

        /// <summary>
        /// Get transactions by date and transaction type
        /// </summary>
        /// <param name="transDate"></param>
        /// <param name="transType"></param>
        /// <returns>List of transaction</returns>
        [Route("get-transactions-by-date-and-type")]
        [HttpGet]
        public async Task<List<oTransaction>> GetAllTransactionByDateAndType(DateTime transDate, string transType)
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

                return await repo.GetTransactionsByDateAndType(transDate, transType);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }

        /// <summary>
        /// Get transactions by date and transaction type per branch
        /// </summary>
        /// <param name="transDate"></param>
        /// <param name="transType"></param>
        /// <param name="branchCode"></param>
        /// <returns></returns>
        [Route("get-transactions-by-date-and-type-per-branch")]
        [HttpGet]
        public async Task<List<oTransaction>> GetAllTransactionByDateAndTypePerBranch(DateTime transDate, string transType, string branchCode)
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

                return await repo.GetTransactionsByDateAndTypePerBranch(transDate, transType, branchCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }

        /// <summary>
        /// Gets all transaction types
        /// </summary>
        /// <returns>List of transaction types</returns>
        [Route("get-transaction-types")]
        [HttpGet]
        public List<object> GetTransactionTypes()
        {
            List<object> transTypes = new List<object>();

            NameValueCollection tTypes = new NameValueCollection();
            tTypes = (NameValueCollection)ConfigurationManager.GetSection("TransactionTypes");
            foreach(string key in tTypes.AllKeys)
            {
                string val = tTypes[key];
                transTypes.Add(new { ID = key, TransType = val });
            }

            //transTypes.Add(new { ID = 1, TransType = "CASH TRANSACTION"});
            //transTypes.Add(new { ID = 2, TransType = "SALES ON ACCOUNT LAP" });
            //transTypes.Add(new { ID = 3, TransType = "SALES ON ACCOUNT DEBIT/CREDIT CARD" });
            //transTypes.Add(new { ID = 4, TransType = "LAP COLLECTION" });
            //transTypes.Add(new { ID = 5, TransType = "SALES RETURN" });
            //transTypes.Add(new { ID = 6, TransType = "TRANSMITTAL-[Branch_Code]" });//TRANSMITTAL-991
            //transTypes.Add(new { ID = 7, TransType = "DEPOSIT" });

            return transTypes;
        }
    }
}
