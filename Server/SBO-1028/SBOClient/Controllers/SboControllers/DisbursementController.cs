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
    /// This end point is for accessing and adding Disbursements
    /// </summary>
    [RoutePrefix("api/disbursements")]
    public class DisbursementController : ApiController
    {
        IDisbursementRepository repo = new RepositoryFactory().DisbursementRepository();
        TransactionLogger transactionLogger;
        string errMsg;

        public DisbursementController()
        {
            repo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
            transactionLogger = new TransactionLogger();
        }

        /// <summary>
        /// Gets all disbursement documents
        /// </summary>
        /// <returns>List of disbursement document</returns>
        [Route("get-disbursements")]
        [HttpGet]
        public async Task<List<oPayment>> GetDisbursements()
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
        /// Gets disbursement document filtered by document number
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns>Disbursement document</returns>
        [Route("get-by-document-number")]
        [HttpGet]
        public async Task<oPayment> GetByDocumentNumber(int documentNumber)
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

                return await repo.GetByDocumentNumber(documentNumber);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets disbursement documents filtered by card code
        /// </summary>
        /// <param name="cardCode"></param>
        /// <returns>List of disbursement document</returns>
        [Route("get-by-cardcode")]
        [HttpGet]
        public async Task<List<oPayment>> GetByCardCode(string cardCode)
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

                return await repo.GetByCardCode(cardCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets disbursement document by reference number
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns>Disbursement document</returns>
        [Route("get-by-reference-number")]
        [HttpGet]
        public async Task<oPayment> GetByReferenceNumber(string refNo)
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

                return await repo.GetByReferenceNumber(refNo);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Adds new disbursement document to SAP database
        /// </summary>
        /// <param name="disbursement"></param>
        /// <returns>Status code 200, and disbursement entry return message</returns>
        [Route("add-disbursement")]
        [HttpPost]
        public async Task<IHttpActionResult> AddDisbursement(oPayment disbursement)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var _grpo = await repo.GetByReferenceNumber(disbursement.ReferenceNo);

                string validationStr = ModelValidator.ValidateModel(disbursement);

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

                    transactionLogger.LogDisbursementTransaction(disbursement, false, "A", this.Request.Headers.Host, _err);
                    throw new HttpResponseException(resp);
                }

                if (_grpo != null)
                {
                    errMsg = string.Format("Goods Receipt PO {0} already exist.", disbursement.ReferenceNo);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object already exist.";
                    ErrorLog _err = new ErrorLog();
                    _err.ErrorCode = (int)HttpStatusCode.Conflict;
                    _err.Message = errMsg;
                    _err.StackTrace = Environment.StackTrace;

                    var err = ErrorLogger.Log(_err);

                    transactionLogger.LogDisbursementTransaction(disbursement, false, "A", this.Request.Headers.Host, _err);
                    throw new HttpResponseException(resp);
                }

                if (repo.Add(disbursement) != 0)
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

                    transactionLogger.LogDisbursementTransaction(disbursement, false, "A", this.Request.Headers.Host, _err);
                    throw new HttpResponseException(resp);
                }

                transactionLogger.LogDisbursementTransaction(disbursement, true, "A", this.Request.Headers.Host);
                return Ok(string.Format("Disbursement {0} succesfully added.", disbursement.ReferenceNo));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
