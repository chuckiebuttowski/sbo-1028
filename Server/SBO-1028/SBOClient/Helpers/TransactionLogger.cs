using Newtonsoft.Json;
using sbo.fx;
using sbo.fx.Models;
using SBOClient.Core.Abstracts;
using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOClient.Helpers
{
    public class TransactionLogger
    {
        ITransactionLogRepository repo = null;
        TransactionLog log;

        public TransactionLogger()
        {
            repo = new RepositoryFactory().CreateTransactionLogRepository();
            log = new TransactionLog();
        }
        
        public void LogJournalTransaction(oJournal obj, bool isPosted, string action, string origin, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.TransId.ToString();
                log.Origin = origin;
                log.Type = TransactionLog.SBOType.JE;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;

                TransactionData rawData = new TransactionData();
                rawData.PostedOn = obj.DocDate;
                rawData.RawData = JsonConvert.SerializeObject(obj);

                log.RawData = rawData;
                log.Action = action;
                log.CreatedBy = obj.CreatedBy;
                log.CreatedOn = obj.CreateDate;
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
        }

        public void LogInvoiceTransaction(oInvoice obj, bool isPosted, string action, string origin, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.DocNo.ToString();
                log.Origin = origin;
                log.Type = obj.InvoiceTransactionType == InvoiceType.AccountsReceivable? TransactionLog.SBOType.ARINV: TransactionLog.SBOType.APINV;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;

                TransactionData rawData = new TransactionData();
                rawData.PostedOn = obj.DocDate;
                rawData.RawData = JsonConvert.SerializeObject(obj);

                log.RawData = rawData;
                log.Action = action;
                //log.CreatedBy = obj.CreatedBy;
                //log.CreatedOn = obj.CreateDate;
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LogInventoryTransaction(oInventoryTransaction obj, bool isPosted, string action, string origin, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.DocNum.ToString();
                log.Origin = origin;
                log.Type = obj.InventoryTransactionType == InventoryType.In? TransactionLog.SBOType.GR: TransactionLog.SBOType.GI;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;

                TransactionData rawData = new TransactionData();
                rawData.PostedOn = obj.DocDate;
                rawData.RawData = JsonConvert.SerializeObject(obj);

                log.RawData = rawData;
                log.Action = action;
                log.CreatedBy = obj.CreatedBy;
                log.CreatedOn = obj.CreateDate;
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LogItemTransaction(oItem obj, bool isPosted, string action, string origin, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.ItemCode;
                log.Origin = origin;
                log.Type = TransactionLog.SBOType.ITM;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;
                TransactionData rawData = new TransactionData();
                rawData.PostedOn = obj.CreateDate;
                rawData.RawData = JsonConvert.SerializeObject(obj);

                log.RawData = rawData;
                log.Action = action;
                log.CreatedBy = obj.CreatedBy;
                log.CreatedOn = obj.CreateDate;
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LogBPTransaction(oBusinessPartner obj, bool isPosted, string action, string origin, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.CardCode;
                log.Origin = origin;
                log.Type = TransactionLog.SBOType.BP;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;
                TransactionData rawData = new TransactionData();
                rawData.PostedOn = obj.CreateDate;
                rawData.RawData = JsonConvert.SerializeObject(obj);

                log.RawData = rawData;
                log.Action = action;
                log.CreatedBy = obj.CreatedBy;
                log.CreatedOn = obj.CreateDate;
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LogGlTransaction(oGlAccount obj, bool isPosted, string action, string origin, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.AccntCode;
                log.Origin = origin;
                log.Type = TransactionLog.SBOType.GL;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;
                TransactionData rawData = new TransactionData();
                rawData.PostedOn = obj.CreateDate;
                rawData.RawData = JsonConvert.SerializeObject(obj);

                log.RawData = rawData;
                log.Action = action;
                log.CreatedBy = obj.CreatedBy;
                log.CreatedOn = obj.CreateDate;
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LogGoodsReceiptTransaction(oGoodsReceipt obj, bool isPosted, string action, string origin, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.DocNo.ToString();
                log.Origin = origin;
                log.Type = TransactionLog.SBOType.GR;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;

                TransactionData rawData = new TransactionData();
                rawData.PostedOn = obj.DocDate;
                rawData.RawData = JsonConvert.SerializeObject(obj);

                log.RawData = rawData;
                log.Action = action;
                log.CreatedBy = obj.CreatedBy;
                log.CreatedOn = obj.CreateDate;
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LogDisbursementTransaction(oPayment obj, bool isPosted, string action, string origin, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.DocNo.ToString();
                log.Origin = origin;
                log.Type = TransactionLog.SBOType.OP;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;

                TransactionData rawData = new TransactionData();
                rawData.PostedOn = obj.DocDate;
                rawData.RawData = JsonConvert.SerializeObject(obj);

                log.RawData = rawData;
                log.Action = action;
                //log.CreatedBy = obj.CreatedBy;
                //log.CreatedOn = obj.CreateDate;
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}