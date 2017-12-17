using Newtonsoft.Json;
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
    /// This end point is for accessing and adding journal entries.
    /// </summary>
    [RoutePrefix("api/journals")]
    public class JournalController : ApiController
    {
        IJournalRepository repo;
        TransactionLogger transactionLogger;
        string errMsg = "";

        public JournalController()
        {
            repo = new RepositoryFactory().JournalRepository();
            transactionLogger = new TransactionLogger();

            repo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
        }

        /// <summary>
        /// Gets all journal entries.
        /// </summary>
        /// <returns>List of journal entries.</returns>
        [Route("get-journals")]
        [HttpGet]
        public async Task<IList<oJournal>> GetJournals()
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
        /// Gets all journal entries filtered by branch code.
        /// </summary>
        /// <param name="branchCode"></param>
        /// <returns>List of journal entries</returns>
        [Route("get-journals-by-branchcode")]
        [HttpGet]
        public async Task<IList<oJournal>> GetJournasByBranchCode(string branchCode)
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

                return await repo.GetByBranch(branchCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

       
        /// <summary>
        /// Get all journal entries filtered by date range.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>List of journal entries</returns>
        [Route("get-journals-by-date-range")]
        [HttpGet]
        public async Task<IList<oJournal>> GetJournasByDateRange(DateTime from, DateTime to)
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

                return await repo.GetByDateRange(from, to);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Get journal entry filtered by id
        /// </summary>
        /// <param name="transId"></param>
        /// <returns>Journal entry</returns>
        [Route("get-journal-by-id")]
        [HttpGet]
        public async Task<oJournal> GetJournalById(int transId)
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

                return await repo.GetByTransId(transId);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Adds new journal entry to SAP database.
        /// </summary>
        /// <param name="jrnal"></param>
        /// <returns>Status code 200, and journal entry return message</returns>
        [Route("add-journal")]
        [HttpPost]
        public async Task<object> AddJournal(oJournal jrnal)
        {

            HttpResponseMessage _resp = new HttpResponseMessage();

            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var j = await repo.GetByRDTransId(jrnal.TransactionId);

                string validationStr = ModelValidator.ValidateModel(jrnal);

                if (!string.IsNullOrEmpty(validationStr))
                {
                    errMsg = string.Format(validationStr);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object property validation error";

                    ErrorLog _err = new ErrorLog();
                    _err.ErrorCode = (int)HttpStatusCode.Conflict;
                    _err.Message = errMsg;
                    _err.StackTrace = Environment.StackTrace;

                    var err = ErrorLogger.Log(_err);

                    transactionLogger.LogJournalTransaction(jrnal, false, "A", HttpContext.Current.Request.UserHostAddress, _err);
                    _resp = resp;
                    throw new HttpResponseException(resp);
                }

                if (j != null)
                {
                    errMsg = string.Format("Journal {0} already exist.", jrnal.TransactionId);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object already exist.";

                    ErrorLog _err = new ErrorLog();
                    _err.ErrorCode = (int)HttpStatusCode.Conflict;
                    _err.Message = errMsg;
                    _err.StackTrace = Environment.StackTrace;

                    var err = ErrorLogger.Log(_err);

                    transactionLogger.LogJournalTransaction(jrnal, false, "A", HttpContext.Current.Request.UserHostAddress, _err);
                    _resp = resp;
                    throw new HttpResponseException(resp);
                }

                if (repo.Add(jrnal) != 0)
                {
                    errMsg = GlobalInstance.Instance.SBOErrorMessage;
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "SBO Error";
                    ErrorLog _err = new ErrorLog();
                    _err.ErrorCode = (int)HttpStatusCode.Conflict;
                    _err.Message = errMsg;
                    _err.StackTrace = Environment.StackTrace;

                    var err = ErrorLogger.Log(_err);

                    transactionLogger.LogJournalTransaction(jrnal, false, "A", HttpContext.Current.Request.UserHostAddress, _err);
                    _resp = resp;
                    throw new HttpResponseException(resp);
                }

                transactionLogger.LogJournalTransaction(jrnal, true, "A", HttpContext.Current.Request.UserHostAddress);
                var _j = await repo.GetByRDTransId(jrnal.TransactionId);
                return new { SAPTransactionId = _j.TransId, ReturnMessage = $"Journal {jrnal.TransactionId} successfully added." };
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }
    }
}
