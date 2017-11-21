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

        [Route("get-journals-by-projectcode")]
        [HttpGet]
        public async Task<IList<oJournal>> GetJournasByProjectCode(string projectCode)
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

                return await repo.GetByProject(projectCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        [Route("get-journals-by-date-range")]
        [HttpGet]
        public async Task<IList<oJournal>> GetJournasByProjectCode(DateTime from, DateTime to)
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

        [Route("add-journal")]
        [HttpPost]
        public async Task<IHttpActionResult> AddJournal(oJournal jrnal)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();

                var j = await repo.GetByTransId(jrnal.TransId);
                if (j != null)
                {
                    errMsg = string.Format("Journal {0} already exist.", jrnal.TransId);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object already exist.";
                    var err = ErrorLogger.Log(new ErrorLog
                    {
                        ErrorCode = GlobalInstance.Instance.SBOErrorCode,
                        Message = errMsg,
                        StackTrace = Environment.StackTrace

                    });

                    transactionLogger.LogJournalTransaction(jrnal, false, err);
                    throw new HttpResponseException(resp);
                }

                if (repo.Add(jrnal) != 0)
                {
                    errMsg = GlobalInstance.Instance.SBOErrorMessage;
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "SBO Error";
                    var err = ErrorLogger.Log(new ErrorLog {
                        ErrorCode = GlobalInstance.Instance.SBOErrorCode,
                        Message = errMsg,
                        StackTrace = Environment.StackTrace

                    });

                    transactionLogger.LogJournalTransaction(jrnal, false, err);
                    throw new HttpResponseException(resp);
                }

                transactionLogger.LogJournalTransaction(jrnal, true);
                return Ok(string.Format("Journal {0} succesfully added.", jrnal.TransId));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        //[Route("add-multiple-journal")]
        //[HttpPost]
        //public async Task<IHttpActionResult> AddMultipleJournal(List<oJournal> jrnals)
        //{
        //    try
        //    {
        //        if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                
        //        if (repo.AddMultiple(jrnals) != 0)
        //        {
        //            errMsg = GlobalInstance.Instance.SBOErrorMessage;
        //            var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
        //            resp.Content = new StringContent(errMsg);
        //            resp.ReasonPhrase = "SBO Error";
        //            throw new HttpResponseException(resp);
        //        }

        //        return Ok("Successfully added journal entries.");
        //    }
        //    catch (HttpResponseException ex)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.BadRequest);
        //    }
        //}

        [Route("update-journal")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateJournal(oJournal jrnal)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();

                var j = await repo.GetByTransId(jrnal.TransId);
                if (j == null)
                {
                    errMsg = string.Format("Journal {0} does exist.", jrnal.TransId);
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object not found.";
                    throw new HttpResponseException(resp);
                }

                if (repo.Update(jrnal) != 0)
                {
                    errMsg = GlobalInstance.Instance.SBOErrorMessage;
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "SBO Error";
                    throw new HttpResponseException(resp);
                }

                return Ok(string.Format("Journal {0} succesfully updated.", jrnal.TransId));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        private void LogTransaction(oJournal jrnal)
        {
            try
            {
                var tLogRepo = new SBOClient.Core.Factories.RepositoryFactory().CreateTransactionLogRepository();
                TransactionLog tLog = new TransactionLog();

                tLog.TransactionNo = jrnal.TransId.ToString(); ;
                tLog.Origin = string.Format("{0}-{1}", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserHostName);
                tLog.Type = TransactionLog.SBOType.JE;
                tLog.LogDate = DateTime.Now;
                tLog.IsPosted = true;
                tLog.TransactionDataID = jrnal.TransId;
                tLog.RawData.PostedOn = jrnal.DocDate;
                tLog.RawData.RawData = JsonConvert.SerializeObject(jrnal);
                tLogRepo.AddOrUpdate(tLog);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
