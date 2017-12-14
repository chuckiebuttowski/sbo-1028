﻿using sbo.fx;
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
    /// This end point is for accessing and adding GRPOs
    /// </summary>
    [RoutePrefix("api/goodsreceiptpos")]
    public class GoodsReceiptPOController : ApiController
    {
        IGoodsReceiptPORepository repo = new RepositoryFactory().GooodsReceiptPORepository();
        TransactionLogger transactionLogger;
        string errMsg;

        public GoodsReceiptPOController()
        {
            repo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
            transactionLogger = new TransactionLogger();
        }

        /// <summary>
        /// Gets all GRPOs with serial numbers
        /// </summary>
        /// <returns>List of GRPOs</returns>
        [Route("get-grpos")]
        [HttpGet]
        public async Task<IList<oGoodsReceiptPO>> GetGRPOS()
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
        /// Gets all GRPOs filtered by card code
        /// </summary>
        /// <param name="cardCode"></param>
        /// <returns>List of GRPOs</returns>
        [Route("get-by-cardcode")]
        [HttpGet]
        public async Task<IList<oGoodsReceiptPO>> GetByCardCode(string cardCode)
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
        /// Gets all GRPOs filtered by date range
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>List of GRPOs</returns>
        [Route("get-by-date-range")]
        [HttpGet]
        public async Task<IList<oGoodsReceiptPO>> GetByDateRange(DateTime from, DateTime to)
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
        /// Gets GRPO filtered by document number
        /// </summary>
        /// <param name="docNo"></param>
        /// <returns>GRPO document</returns>
        [Route("get-by-doc-num")]
        [HttpGet]
        public async Task<oGoodsReceiptPO> GetByDocumentNumber(int docNo)
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

                return await repo.GetByDocumentNumber(docNo);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets GRPO filtered by reference no
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns>GRPO document</returns>
        [Route("get-by-ref-num")]
        [HttpGet]
        public async Task<oGoodsReceiptPO> GetByReferenceNumber(string refNo)
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
        /// Adds new GRPO to SAP database
        /// </summary>
        /// <param name="grpo"></param>
        /// <returns>Status code 200, and GRPO entry return message</returns>
        [Route("add-grpo")]
        [HttpPost]
        public async Task<IHttpActionResult> AddGRPO(oGoodsReceiptPO grpo)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var _grpo = await repo.GetByReferenceNumber(grpo.ReferenceNo);

                string validationStr = ModelValidator.ValidateModel(grpo);

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

                    transactionLogger.LogGoodsReceiptPOTransaction(grpo, false, "A", this.Request.Headers.Host, _err);
                    throw new HttpResponseException(resp);
                }

                if (_grpo != null)
                {
                    errMsg = string.Format("Goods Receipt PO {0} already exist.", grpo.ReferenceNo);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object already exist.";
                    ErrorLog _err = new ErrorLog();
                    _err.ErrorCode = (int)HttpStatusCode.Conflict;
                    _err.Message = errMsg;
                    _err.StackTrace = Environment.StackTrace;

                    var err = ErrorLogger.Log(_err);

                    transactionLogger.LogGoodsReceiptPOTransaction(grpo, false, "A", this.Request.Headers.Host, _err);
                    throw new HttpResponseException(resp);
                }

                if (repo.Add(grpo) != 0)
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

                    transactionLogger.LogGoodsReceiptPOTransaction(grpo, false, "A", this.Request.Headers.Host, _err);
                    throw new HttpResponseException(resp);
                }

                transactionLogger.LogGoodsReceiptPOTransaction(grpo, true, "A", this.Request.Headers.Host);
                return Ok(string.Format("Goods Receipt PO {0} succesfully added.", grpo.ReferenceNo));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
