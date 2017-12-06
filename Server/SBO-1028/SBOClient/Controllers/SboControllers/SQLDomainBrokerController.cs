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

                switch (transactionType)
                {
                    case "30"://JE
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogJournalTransaction((oJournal)paramObj, true, "A");
                        break;
                    case "18"://AP
                    case "13"://AR
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogInvoiceTransaction((oInvoice)paramObj, true, "A");
                        break;
                    case "60"://GI
                    case "59"://GR
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogInventoryTransaction((oInventoryTransaction)paramObj, true, "A");
                        break;
                    case "4"://ITM
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogItemTransaction((oItem)paramObj, true, "A");
                        break;
                    case "2"://BP
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogBPTransaction((oBusinessPartner)paramObj, true, "A");
                        break;
                    case "1"://GL
                        response = await client.PostAsJsonAsync(uri, paramObj);
                        if (response.IsSuccessStatusCode) logger.LogGlTransaction((oGlAccount)paramObj, true, "A");
                        break;
                }
                
                response = await client.PostAsJsonAsync(uri, paramObj);
            }
            catch (HttpResponseException ex)
            {
                ErrorLog err = null;
                switch (transactionType)
                {
                    case "30":
                        err = new ErrorLog {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };
                        logger.LogJournalTransaction((oJournal)paramObj, false, "A", ErrorLogger.Log(err));
                        break;
                    case "18":
                    case "13":
                        err = new ErrorLog
                        {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };
                        logger.LogInvoiceTransaction((oInvoice)paramObj, false, "A", ErrorLogger.Log(err));
                        break;
                    case "60"://GI
                    case "59"://GR
                        err = new ErrorLog
                        {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };
                        logger.LogInventoryTransaction((oInventoryTransaction)paramObj, false, "A", ErrorLogger.Log(err));
                        break;
                    case "4"://ITM
                        err = new ErrorLog
                        {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };
                        logger.LogItemTransaction((oItem)paramObj, false, "A", ErrorLogger.Log(err));
                        break;
                    case "2"://BP
                        err = new ErrorLog
                        {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };
                        logger.LogBPTransaction((oBusinessPartner)paramObj, false, "A", ErrorLogger.Log(err));
                        break;
                    case "1"://GL
                        err = new ErrorLog
                        {
                            ErrorCode = (int)HttpStatusCode.BadRequest,
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        };
                        logger.LogGlTransaction((oGlAccount)paramObj, false, "A", ErrorLogger.Log(err));
                        break;
                }
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return Ok();
        }

        private object ResolveObject(APICallSignature callSig)
        {
            object obj = null;
            Task.Run(async () => {
                switch (callSig.CallObjCode)
                {
                    case "30":
                        var journalRepo = new sbo.fx.Factories.RepositoryFactory().JournalRepository();
                        obj = await journalRepo.GetByTransId(int.Parse(callSig.CallKey));
                        break;
                    case "60":
                        var inventoryOutRepo = new sbo.fx.Factories.RepositoryFactory().InventoryTransactionRepository();
                        obj = await inventoryOutRepo.GetTransactionByDocNo(int.Parse(callSig.CallKey), InventoryType.Out);
                        break;
                    case "59":
                        var inventoryInRepo = new sbo.fx.Factories.RepositoryFactory().InventoryTransactionRepository();
                        obj = await inventoryInRepo.GetTransactionByDocNo(int.Parse(callSig.CallKey), InventoryType.In);
                        break;
                    case "4":
                        var itemRepo = new sbo.fx.Factories.RepositoryFactory().ItemRepository();
                        obj = await itemRepo.GetItemByItemCode(callSig.CallKey);
                        break;
                    case "2":
                        var bpRepo = new sbo.fx.Factories.RepositoryFactory().BusinessPartnerRepository();
                        obj = await bpRepo.GetByCardCode(callSig.CallKey);
                        break;
                    case "1":
                        var glRepo = new sbo.fx.Factories.RepositoryFactory().GlAccountRepository();
                        obj = await glRepo.GetByAccountCode(callSig.CallKey);
                        break;
                }
            });

            return obj;
        }

        private ClientAPI ResolveUri(APICallSignature callSig)
        {
            string uri = "";
            ClientAPI clientApi = new ClientAPI();
            var repo = new RepositoryFactory().CreateClientAPIRepository();

            switch (callSig.CallObjCode)
            {
                case "30"://Journal Entry
                    clientApi = repo.Get(x => x.Action == callSig.Action && x.SboTransactionType == "30" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
                case "60"://Goods Issue
                    clientApi = repo.Get(x => x.Action == callSig.Action && x.SboTransactionType == "60" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
                case "59"://Goods Receipt
                    clientApi = repo.Get(x => x.Action == callSig.Action && x.SboTransactionType == "59" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
                case "4"://item
                    clientApi = repo.Get(x => x.Action == callSig.Action && x.SboTransactionType == "4" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
                case "2"://Business Partner
                    clientApi = repo.Get(x => x.Action == callSig.Action && x.SboTransactionType == "2" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
                case "1"://Gl Account
                    clientApi = repo.Get(x => x.Action == callSig.Action && x.SboTransactionType == "1" && x.ValueType == (PostDataValueType)(callSig.ValueType == "S" ? 0 : 1)).Include("Params").FirstOrDefault();
                    break;
            }

            return clientApi;
        }
    }
}
