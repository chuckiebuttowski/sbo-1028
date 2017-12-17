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
    /// This end point is for accessing and adding Goods Receipts
    /// </summary>
    [RoutePrefix("api/goodsreceipt")]
    public class GoodsReceiptController : ApiController
    {
        IGoodsReceiptRepository repo = new RepositoryFactory().GoodsReceiptRepository();
        TransactionLogger transactionLogger;
        string errMsg;

        public GoodsReceiptController()
        {
            repo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
            transactionLogger = new TransactionLogger();
        }

        /// <summary>
        /// Gets all Goods Receipts with serial numbers
        /// </summary>
        /// <returns>List of Goods Receipts</returns>
        [Route("get-goods-receipts")]
        [HttpGet]
        public async Task<IList<oGoodsReceipt>> GetGRPOS()
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
        /// Gets all Goods Receipts filtered by date range
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>List of Goods Receipts</returns>
        [Route("get-by-date-range")]
        [HttpGet]
        public async Task<IList<oGoodsReceipt>> GetByDateRange(DateTime from, DateTime to)
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
        /// Gets Goods Receipt filtered by document number
        /// </summary>
        /// <param name="docNo"></param>
        /// <returns>Goods Receipt document</returns>
        [Route("get-by-doc-num")]
        [HttpGet]
        public async Task<oGoodsReceipt> GetByDocumentNumber(int docNo)
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
        /// Gets Goods Receipt filtered by reference no
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns>Goods Receipt document</returns>
        [Route("get-by-ref-num")]
        [HttpGet]
        public async Task<oGoodsReceipt> GetByReferenceNumber(string refNo)
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

                return await repo.GetByReferenceNo(refNo);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Adds new Goods Receipt to SAP database
        /// </summary>
        /// <param name="goodsReceipt"></param>
        /// <returns>Status code 200, and Goods Receipt entry return message</returns>
        [Route("add-goods-receipt")]
        [HttpPost]
        public async Task<object> AddGR(oGoodsReceipt goodsReceipt)
        {
            try
            {
                oGoodsReceipt _grpo = null;
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                if (goodsReceipt.ReferenceNo != null) _grpo = await repo.GetByReferenceNo(goodsReceipt.ReferenceNo);

                string validationStr = ModelValidator.ValidateModel(goodsReceipt);

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

                    transactionLogger.LogGoodsReceiptTransaction(goodsReceipt, false, "A", HttpContext.Current.Request.UserHostAddress, _err);
                    throw new HttpResponseException(resp);
                }

                if (_grpo != null)
                {
                    errMsg = string.Format("Goods Receipt {0} already exist.", goodsReceipt.ReferenceNo);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object already exist.";
                    ErrorLog _err = new ErrorLog();
                    _err.ErrorCode = (int)HttpStatusCode.Conflict;
                    _err.Message = errMsg;
                    _err.StackTrace = Environment.StackTrace;

                    var err = ErrorLogger.Log(_err);

                    transactionLogger.LogGoodsReceiptTransaction(goodsReceipt, false, "A", HttpContext.Current.Request.UserHostAddress, _err);
                    throw new HttpResponseException(resp);
                }

                if (repo.Add(goodsReceipt) != 0)
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

                    transactionLogger.LogGoodsReceiptTransaction(goodsReceipt, false, "A", HttpContext.Current.Request.UserHostAddress, _err);
                    throw new HttpResponseException(resp);
                }

                transactionLogger.LogGoodsReceiptTransaction(goodsReceipt, true, "A", HttpContext.Current.Request.UserHostAddress);
                var grpo = await repo.GetByReferenceNo(goodsReceipt.ReferenceNo);
                return new { SAPGRDocumentNumber = grpo.DocNo, ReturnMessage = $"Goods receipt {goodsReceipt.ReferenceNo} successfully added." };
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }
    }
}
