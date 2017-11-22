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
        
        public void LogJournalTransaction(oJournal obj, bool isPosted, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.TransId.ToString();
                log.Origin = string.Format("{0}-{1}", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserHostName);
                log.Type = TransactionLog.SBOType.JE;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;
                log.TransactionDataID = obj.TransId;
                log.RawData.PostedOn = obj.DocDate;
                log.RawData.RawData = JsonConvert.SerializeObject(obj);
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            { 
                throw new Exception(ex.Message);
            }
        }

        public void LogInvoiceTransaction(oInvoice obj, bool isPosted, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.TransId.ToString();
                log.Origin = string.Format("{0}-{1}", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserHostName);
                log.Type = obj.InvoiceTransactionType == InvoiceType.AccountsReceivable? TransactionLog.SBOType.ARINV: TransactionLog.SBOType.APINV;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;
                log.TransactionDataID = obj.TransId;
                log.RawData.PostedOn = obj.DocDate;
                log.RawData.RawData = JsonConvert.SerializeObject(obj);
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LogInventoryTransaction(oInventoryTransaction obj, bool isPosted, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.DocNum.ToString();
                log.Origin = string.Format("{0}-{1}", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserHostName);
                log.Type = obj.InventoryTransactionType == InventoryType.In? TransactionLog.SBOType.GR: TransactionLog.SBOType.GI;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;
                log.TransactionDataID = log.RawData.ID;
                log.RawData.PostedOn = obj.DocDate;
                log.RawData.RawData = JsonConvert.SerializeObject(obj);
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LogItemTransaction(oItem obj, bool isPosted, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.ItemCode;
                log.Origin = string.Format("{0}-{1}", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserHostName);
                log.Type = TransactionLog.SBOType.ITM;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;
                log.TransactionDataID = log.RawData.ID;
                log.RawData.PostedOn = obj.CreateDate;
                log.RawData.RawData = JsonConvert.SerializeObject(obj);
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LogBPTransaction(oBusinessPartner obj, bool isPosted, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.CardCode;
                log.Origin = string.Format("{0}-{1}", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserHostName);
                log.Type = TransactionLog.SBOType.BP;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;
                log.TransactionDataID = log.RawData.ID;
                log.RawData.PostedOn = obj.CreateDate;
                log.RawData.RawData = JsonConvert.SerializeObject(obj);
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void LogglTransaction(oGlAccount obj, bool isPosted, ErrorLog errLog = null)
        {
            try
            {
                log.TransactionNo = obj.AccntCode;
                log.Origin = string.Format("{0}-{1}", HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserHostName);
                log.Type = TransactionLog.SBOType.GL;
                log.LogDate = DateTime.Now;
                log.IsPosted = isPosted;
                log.TransactionDataID = log.RawData.ID;
                log.RawData.PostedOn = obj.CreateDate;
                log.RawData.RawData = JsonConvert.SerializeObject(obj);
                repo.AddOrUpdate(log);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}