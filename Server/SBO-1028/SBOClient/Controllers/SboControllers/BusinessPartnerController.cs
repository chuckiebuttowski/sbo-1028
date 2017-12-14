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
    /// This end point is for accessing and adding business partners
    /// </summary>
    [RoutePrefix("api/business-partners")]
    [AllowAnonymous]
    public class BusinessPartnerController : ApiController
    {
        IBusinessPartnerRepository repo = new RepositoryFactory().BusinessPartnerRepository();
        TransactionLogger transactionLogger;
        string errMsg = "";

        public BusinessPartnerController()
        {
            repo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
            transactionLogger = new TransactionLogger();
        }

        /// <summary>
        /// Get all business partners
        /// </summary>
        /// <returns>List of business partner</returns>
        [Route("get-all-bps")]
        [HttpGet]
        public async Task<IList<oBusinessPartner>> GetBusinessPartners()
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
        /// Get all business partner filtered by business partner type (i.e. customer or supplier)
        /// </summary>
        /// <param name="cardType"></param>
        /// <returns>List of business partner</returns>
        [Route("get-bps-by-type")]
        [HttpGet]
        public async Task<IList<oBusinessPartner>> GetBusinessPartnersByType(string cardType)
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

                return await repo.GetByCardType(cardType);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Get all business partner filtered by group code
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns>List of business partner</returns>
        [Route("get-bps-by-group-code")]
        [HttpGet]
        public async Task<IList<oBusinessPartner>> GetBusinessPartnersByGroupCode(int groupCode)
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

                return await repo.GetByGroupCode(groupCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        ///  Get all business partner filtered by series
        /// </summary>
        /// <param name="series">Refer to documentation</param>
        /// <returns>List of business partner</returns>
        [Route("get-bps-by-series")]
        [HttpGet]
        public async Task<IList<oBusinessPartner>> GetBusinessPartnersBySeries(int series)
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

                return await repo.GetBySeries(series);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Get all business partner filtered by status
        /// </summary>
        /// <param name="status">Values: "Y" or "N"</param>
        /// <example>Values: "Y" or "N"</example>
        /// <returns>List of business partner</returns>
        [Route("get-bps-by-active-status")]
        [HttpGet]
        public async Task<IList<oBusinessPartner>> GetBusinessPartnersByStatus(string status)
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

                return await repo.GetByStatus(status);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Get business partner filtered by business partner code
        /// </summary>
        /// <param name="cardCode"></param>
        /// <returns>Business partner</returns>
        [Route("get-bps-by-card-code")]
        [HttpGet]
        public async Task<oBusinessPartner> GetBusinessPartnersByCardCode(string cardCode)
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
        /// Adds new business partner to SAP database
        /// </summary>
        /// <param name="businessPartner"></param>
        /// <returns>If successful, returns status code 200 and business partner entry return message</returns>
        [Route("add-business-partner")]
        [HttpPost]
        public async Task<IHttpActionResult> AddBusinessPartner(oBusinessPartner businessPartner)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var bp = await repo.GetByCardCode(businessPartner.CardCode);

                string validationStr = ModelValidator.ValidateModel(businessPartner);

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

                    transactionLogger.LogBPTransaction(businessPartner, false, "A", this.Request.Headers.Host, err);
                    throw new HttpResponseException(resp);
                }

                if (businessPartner != null)
                {
                    errMsg = string.Format("Business partner {0} already exist.", businessPartner.CardCode);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object already exist.";
                    var err = ErrorLogger.Log(new ErrorLog
                    {
                        ErrorCode = (int)HttpStatusCode.Conflict,
                        Message = errMsg,
                        StackTrace = Environment.StackTrace

                    });

                    transactionLogger.LogBPTransaction(businessPartner, false, "A", this.Request.Headers.Host, err);
                    throw new HttpResponseException(resp);
                }

                if (repo.Add(businessPartner) < 0)
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

                    transactionLogger.LogBPTransaction(businessPartner, true, "A", this.Request.Headers.Host, err);
                    throw new HttpResponseException(resp);
                }

                transactionLogger.LogBPTransaction(businessPartner, true, "A", this.Request.Headers.Host);
                return Ok(string.Format("Business partner {0} succesfully added.", businessPartner.CardCode));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Updates business partner from SAP database
        /// </summary>
        /// <param name="businessPartner"></param>
        /// <returns>If successful, returns status code 200 and business partner update return message</returns>
        [Route("update-business-partner")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdateBusinessPartner(oBusinessPartner businessPartner)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var bp = await repo.GetByCardCode(businessPartner.CardCode);

                string validationStr = ModelValidator.ValidateModel(businessPartner);

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

                    transactionLogger.LogBPTransaction(businessPartner, false, "U", this.Request.Headers.Host, err);
                    throw new HttpResponseException(resp);
                }

                if (businessPartner == null)
                {
                    errMsg = string.Format("Business partner {0} does not exist.", businessPartner.CardCode);
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object not found.";
                    var err = ErrorLogger.Log(new ErrorLog
                    {
                        ErrorCode = (int)HttpStatusCode.Conflict,
                        Message = errMsg,
                        StackTrace = Environment.StackTrace

                    });

                    transactionLogger.LogBPTransaction(businessPartner, false, "U", this.Request.Headers.Host, err);
                    throw new HttpResponseException(resp);
                }

                if (repo.Update(businessPartner) < 0)
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

                    transactionLogger.LogBPTransaction(businessPartner, false, "U", this.Request.Headers.Host, err);
                    throw new HttpResponseException(resp);
                }

                transactionLogger.LogBPTransaction(businessPartner, true, "U", this.Request.Headers.Host);
                return Ok(string.Format("Business partner {0} succesfully added.", businessPartner.CardCode));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

    }
}
