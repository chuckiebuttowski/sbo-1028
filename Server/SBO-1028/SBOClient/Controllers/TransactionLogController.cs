using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SBOClient.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [RoutePrefix("api/transaction-log")]
    public class TransactionLogController : ApiController
    {
        [HttpGet]
        [Route("get-all")]
        public IEnumerable<TransactionLog> GetAllLogs()
        {
            try
            {
                var logs = new List<TransactionLog>();
                var repo = new RepositoryFactory().CreateTransactionLogRepository();

                return repo.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("get-log-detail")]
        public TransactionLog GetLogDetailByLogID(int id)
        {
            var repo = new RepositoryFactory().CreateTransactionLogRepository();
            return repo.Get(x => x.ID == id).FirstOrDefault();
        }

        [HttpGet]
        [Route("retry-posting")]
        public string RetryPosting(int id)
        {
            try
            {
                var repo = new RepositoryFactory().CreateTransactionLogRepository();
                repo.RetryPosting(id);

                return "Success";
            }
            catch (Exception ex)
            {
                return "Unable to post this payload. Please contact administrator.";
            }
        }
    }
}
