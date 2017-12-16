using sbo.fx;
using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using SBOClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data.Entity;
using static SBOClient.Core.DAL.Entities.ClientAPI;
using SBOClient.Helpers;
using sbo.fx.Models;
using Newtonsoft.Json;

namespace SBOClient.Controllers.SboControllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [RoutePrefix("api/sql-domain-broker")]
    public class SQLDomainBrokerController : ApiController
    {
        TransactionLogger logger;

        public SQLDomainBrokerController()
        {
            logger = new TransactionLogger();
        }

        [Route("invoke-domain-endpoint")]
        [HttpPost]
        public async Task<IHttpActionResult> InvokeDomainEndPoint(APICallSignature callSig)
        {
            try
            {
                ClientAPI clientApi = ResolveUri(callSig);
                object obj = ResolveObject(callSig);
                string errorMsg = "";
                
                if (obj == null)
                {
                    errorMsg = "Object is null";
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                    resp.Content = new StringContent(errorMsg);
                    resp.ReasonPhrase = "Object not found.";
                    throw new HttpResponseException(resp);
                }

                if (string.IsNullOrEmpty(clientApi.URL))
                {
                    errorMsg = "URL invalid";
                    var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                    resp.Content = new StringContent(errorMsg);
                    resp.ReasonPhrase = "No API.";
                    throw new HttpResponseException(resp);
                }


                await InvokeClientAPI(obj, clientApi.URL, clientApi.SboTransactionType);
                return Ok("Success");
            }
            catch (HttpResponseException ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task<IHttpActionResult> InvokeClientAPI(object paramObj, string uri, string transactionType)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                switch (transactionType)
                {
                    case "JE"://JE
                        response = await client.PostAsJsonAsync(uri, (oJournal)paramObj);
                        if (response.IsSuccessStatusCode) logger.LogJournalTransaction((oJournal)paramObj, true, "A", this.Request.Headers.Host);
                        else throw new HttpResponseException(response);
                        break;
                    case "AP INV"://AP
                    case "AR INV"://AR
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogInvoiceTransaction((oInvoice)paramObj, true, "A", this.Request.Headers.Host);
                        break;
                    case "GI"://GI
                    case "GR"://GR
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogInventoryTransaction((oInventoryTransaction)paramObj, true, "A", this.Request.Headers.Host);
                        break;
                    case "ITM"://ITM
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogItemTransaction((oItem)paramObj, true, "A", this.Request.Headers.Host);
                        break;
                    case "BP"://BP
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogBPTransaction((oBusinessPartner)paramObj, true, "A", this.Request.Headers.Host);
                        break;
                    case "GL"://GL
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogGlTransaction((oGlAccount)paramObj, true, "A", this.Request.Headers.Host);
                        break;
                }
            }
            catch (HttpResponseException ex)
            {
                ErrorLog err = null;
                ErrorLog errLog = new ErrorLog();
                switch (transactionType)
                {
                    case "JE":
                        err = new ErrorLog {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };

                        errLog = ErrorLogger.Log(err);

                        logger.LogJournalTransaction((oJournal)paramObj, false, "A", this.Request.Headers.Host, errLog);
                        break;
                    case "AR INV":
                    case "AP INV":
                        err = new ErrorLog
                        {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };

                        errLog = ErrorLogger.Log(err);

                        logger.LogInvoiceTransaction((oInvoice)paramObj, false, "A", this.Request.Headers.Host, errLog);
                        break;
                    case "GI"://GI
                    case "GR"://GR
                        err = new ErrorLog
                        {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };

                        errLog = ErrorLogger.Log(err);
                        logger.LogInventoryTransaction((oInventoryTransaction)paramObj, false, "A", this.Request.Headers.Host, errLog);
                        break;
                    case "ITM"://ITM
                        err = new ErrorLog
                        {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };

                        errLog = ErrorLogger.Log(err);
                        logger.LogItemTransaction((oItem)paramObj, false, "A", this.Request.Headers.Host, errLog);
                        break;
                    case "BP"://BP
                        err = new ErrorLog
                        {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };

                        errLog = ErrorLogger.Log(err);
                        logger.LogBPTransaction((oBusinessPartner)paramObj, false, "A", this.Request.Headers.Host, errLog);
                        break;
                    case "GL"://GL
                        err = new ErrorLog
                        {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };

                        errLog = ErrorLogger.Log(err);
                        logger.LogGlTransaction((oGlAccount)paramObj, false, "A", this.Request.Headers.Host, errLog);
                        break;
                }
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return Ok();
        }

        private object ResolveObject(APICallSignature callSig)
        {
            try
            {
                object obj = null;
                Task.Run(async () => { 
                    switch (callSig.CallObjCode)
                    {
                        case "JE":
                            var journalRepo = new sbo.fx.Factories.RepositoryFactory().JournalRepository();
                            journalRepo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
                            if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                            if (GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Closed)
                            {
                                obj = await journalRepo.GetByTransId(int.Parse(callSig.CallKey));
                            }
                            break;
                        case "GI":
                            var inventoryOutRepo = new sbo.fx.Factories.RepositoryFactory().InventoryTransactionRepository();
                            inventoryOutRepo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
                            if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                            if (GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Closed)
                            {
                                obj = await inventoryOutRepo.GetTransactionByDocNo(int.Parse(callSig.CallKey), InventoryType.Out);
                            }
                            break;
                        case "GR":
                            var inventoryInRepo = new sbo.fx.Factories.RepositoryFactory().InventoryTransactionRepository();
                            inventoryInRepo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
                            if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                            if (GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Closed)
                            {
                                obj = await inventoryInRepo.GetTransactionByDocNo(int.Parse(callSig.CallKey), InventoryType.In);
                            }
                            break;
                        case "ITM":
                            var itemRepo = new sbo.fx.Factories.RepositoryFactory().ItemRepository();
                            itemRepo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
                            if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                            if (GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Closed)
                            {
                                obj = await itemRepo.GetItemByItemCode(callSig.CallKey);
                            }
                            break;
                        case "BP":
                            var bpRepo = new sbo.fx.Factories.RepositoryFactory().BusinessPartnerRepository();
                            bpRepo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
                            if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                            if (GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Closed)
                            {
                                obj = await bpRepo.GetByCardCode(callSig.CallKey);
                            }
                            break;
                        case "GL":
                            var glRepo = new sbo.fx.Factories.RepositoryFactory().GlAccountRepository();
                            glRepo.InitRepository(GlobalInstance.Instance.SboComObject, GlobalInstance.Instance.SqlObject);
                            if (GlobalInstance.Instance.SqlObject.State == System.Data.ConnectionState.Closed) GlobalInstance.Instance.SqlObject.Open();
                            if (GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Broken || GlobalInstance.Instance.SqlObject.State != System.Data.ConnectionState.Closed)
                            {
                                obj = await glRepo.GetByAccountCode(callSig.CallKey);
                            }
                            break;
                    }
                }).Wait();

                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }

        private ClientAPI ResolveUri(APICallSignature callSig)
        {
            string uri = "";
            ClientAPI clientApi = new ClientAPI();
            var repo = new RepositoryFactory().CreateClientAPIRepository();

            switch (callSig.CallObjCode)
            {
                case "JE"://Journal Entry
                    clientApi = repo.Get(x => x.Action.ToUpper() == callSig.Action.ToUpper() && x.SboTransactionType.ToUpper() == "JE" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
                case "GI"://Goods Issue
                    clientApi = repo.Get(x => x.Action.ToUpper() == callSig.Action.ToUpper() && x.SboTransactionType.ToUpper() == "GI" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
                case "GR"://Goods Receipt
                    clientApi = repo.Get(x => x.Action.ToUpper() == callSig.Action.ToUpper() && x.SboTransactionType.ToUpper() == "GR" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
                case "ITM"://item
                    clientApi = repo.Get(x => x.Action.ToUpper() == callSig.Action.ToUpper() && x.SboTransactionType.ToUpper() == "ITM" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
                case "BP"://Business Partner
                    clientApi = repo.Get(x => x.Action.ToUpper() == callSig.Action.ToUpper() && x.SboTransactionType.ToUpper() == "BP" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
                case "GL"://Gl Account
                    clientApi = repo.Get(x => x.Action.ToUpper() == callSig.Action.ToUpper() && x.SboTransactionType.ToUpper() == "GL" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
            }

            return clientApi;
        }
    }
}
