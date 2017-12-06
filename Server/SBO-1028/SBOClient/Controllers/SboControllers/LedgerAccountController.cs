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
    /// This end point is for accessing and adding general ledger accounts.
    /// </summary>
    [RoutePrefix("api/ledger-accounts")]
    public class LedgerAccountController : ApiController
    {
        IGlAccountRepository repo = new RepositoryFactory().GlAccountRepository();
        TransactionLogger transactionLogger;
        string errMsg = "";

        public LedgerAccountController()
        {
            repo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
            transactionLogger = new TransactionLogger();
        }

        /// <summary>
        /// Get all general ledger accounts
        /// </summary>
        /// <returns>List of all ledger accounts</returns>
        [Route("get-ledger-accounts")]
        [HttpGet]
        public async Task<IList<oGlAccount>> GetLedgerAccounts()
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
        /// Get general ledger account per account code.
        /// </summary>
        /// <param name="acctCode"></param>
        /// <returns>General Legder Account object.</returns>
        [Route("get-ledger-account")]
        [HttpGet]
        public async Task<oGlAccount> GetLedgerAccount(string acctCode)
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

                return await repo.GetByAccountCode(acctCode);
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Adds new general ledger account to SAP Database.
        /// </summary>
        /// <param name="glAccount"></param>
        /// <returns></returns>
        [Route("add-ledger-account")]
        [HttpPost]
        public async Task<IHttpActionResult> AddLedgerAccount(oGlAccount glAccount)
        {
            try
            {
                if (!GlobalInstance.Instance.IsConnected) GlobalInstance.Instance.InitializeSboComObject();
                var gl = await repo.GetByAccountCode(glAccount.AccntCode);


                string validationStr = ModelValidator.ValidateModel(glAccount);
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

                    transactionLogger.LogGlTransaction(glAccount, false, "A", err);
                    throw new HttpResponseException(resp);
                }

                if (gl != null)
                {
                    errMsg = string.Format("Ledger account {0}-{1} already exist.", glAccount.AccntCode, glAccount.AccntName);
                    var resp = new HttpResponseMessage(HttpStatusCode.Conflict);
                    resp.Content = new StringContent(errMsg);
                    resp.ReasonPhrase = "Object already exist.";
                    var err = ErrorLogger.Log(new ErrorLog
                    {
                        ErrorCode = (int)HttpStatusCode.Conflict,
                        Message = errMsg,
                        StackTrace = Environment.StackTrace

                    });

                    transactionLogger.LogGlTransaction(glAccount, false, "A", err);
                    throw new HttpResponseException(resp);
                }

                if (repo.Add(glAccount) < 0)
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

                    transactionLogger.LogGlTransaction(glAccount, false, "A", err);
                    throw new HttpResponseException(resp);
                }

                transactionLogger.LogGlTransaction(glAccount, true, "A");
                return Ok(string.Format("Ledger account {0}-{1} already exist.", glAccount.AccntCode, glAccount.AccntName));
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}
