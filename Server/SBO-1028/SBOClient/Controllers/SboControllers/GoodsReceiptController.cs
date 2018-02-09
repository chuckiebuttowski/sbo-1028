using sbo.fx;
using sbo.fx.Factories;
using sbo.fx.Interfaces;
using sbo.fx.Models;
using SBOClient.Core.DAL.Entities;
using SBOClient.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        public async Task<IList<oGoodsReceipt>> GetGoodsReceipt()
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
        /// Gets Goods Receipt filtered by batch no
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns>Goods Receipt document</returns>
        [Route("get-by-batch-no")]
        [HttpGet]
        public async Task<oGoodsReceipt> GetByBatchNumber(string batchNo)
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

                return await repo.GetByBatchNo(batchNo);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Gets Goods Receipts filtered by PO No.
        /// </summary>
        /// <param name="poNo"></param>
        /// <returns>List of Goods Receipts</returns>
        [Route("get-by-po-no")]
        [HttpGet]
        public async Task<List<oGoodsReceipt>> GetByPONumber(string poNo)
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

                return await repo.GetByPONumber(poNo);
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
        /// <returns>Status code 200, and Goods Receipt entry return object</returns>
        [Route("add-goods-receipt")]
        [HttpPost]
        public async Task<object> AddGR(oGoodsReceipt goodsReceipt)
        {
            try
            {
                oGoodsReceipt _grpo = null;

                string isOldItem = goodsReceipt.GoodsReceiptLines.Select(x => x.IsOldItem).First();
                if (isOldItem == null)
                {
                    errMsg = string.Format("Item classification cannot be null. (Is Old Item)", goodsReceipt.BatchNumber);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Field cannot be null";
                    ErrorLog _err = new ErrorLog();
                    _err.ErrorCode = (int)HttpStatusCode.Conflict;
                    _err.Message = errMsg;
                    _err.StackTrace = Environment.StackTrace;

                    var err = ErrorLogger.Log(_err);

                    transactionLogger.LogGoodsReceiptTransaction(goodsReceipt, false, "A", HttpContext.Current.Request.UserHostAddress, _err);
                    throw new HttpResponseException(resp);
                }


                goodsReceipt.BatchNumber = isOldItem.ToUpper() == "Y" ? $"R-{goodsReceipt.BatchNumber}" : $"N-{goodsReceipt.BatchNumber}";

                if (goodsReceipt.GoodsReceiptLines.Count > 0)
                {
                    foreach (var l in goodsReceipt.GoodsReceiptLines)
                    {
                        if (l.IsOldItem.ToLower() == "y")
                        {
                            if (l.WarehouseCode == "" || string.IsNullOrEmpty(l.WarehouseCode))
                            {
                                l.WarehouseCode = ConfigurationManager.AppSettings["DefaultRemaWarehouse"];
                                l.BinCode = Int32.Parse(ConfigurationManager.AppSettings["DefaultRemaBin"]);
                            }
                        }
                    }
                }

                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                if (goodsReceipt.BatchNumber != null) _grpo = await repo.GetByBatchNo(goodsReceipt.BatchNumber);

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
                    errMsg = string.Format("Goods Receipt {0} already exist.", goodsReceipt.BatchNumber);
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
                var grpo = await repo.GetByBatchNo(goodsReceipt.BatchNumber);
                return new { SAPGRDocumentNumber = grpo.DocNo, ReturnMessage = $"Goods receipt {goodsReceipt.BatchNumber} successfully added." };
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(ex.Response);
            }
        }
    }
}
