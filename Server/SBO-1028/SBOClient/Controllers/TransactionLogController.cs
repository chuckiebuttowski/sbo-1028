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
        [Route("get-counts")]
        public IEnumerable<object> GetCounts()
        {
            var list = new List<object>();

            var repo = new RepositoryFactory().CreateTransactionLogRepository();

            DateTime last24Start = DateTime.Now.AddHours(-24);

            //list.Add(new { Name = "Total",  Count = GenerateTestData().Where(x => x.LogDate >= last24Start).Count() });
            //list.Add(new { Name = "Posted", Count = GenerateTestData().Where(x => x.IsPosted && x.LogDate >= last24Start).Count() });
            //list.Add(new { Name = "NotPosted", Count = GenerateTestData().Where(x => !x.IsPosted && x.LogDate >= last24Start).Count() });

            list.Add(new { Name = "Total", Count = GetData().Where(x => x.LogDate >= last24Start).Count() });
            list.Add(new { Name = "Posted", Count = GetData().Where(x => x.IsPosted && x.LogDate >= last24Start).Count() });
            list.Add(new { Name = "NotPosted", Count = GetData().Where(x => !x.IsPosted && x.LogDate >= last24Start).Count() });
            return list;
        }

        [HttpGet]
        [Route("get-all")]
        public IEnumerable<TransactionLog> GetAllLogs()
        {
            try
            {
                return GetData();
                //return GenerateTestData();
                //var repo = new RepositoryFactory().CreateTransactionLogRepository();

                //return repo.GetAll();
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
            //return GenerateTestData().FirstOrDefault(x => x.ID == id);
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

        public IEnumerable<TransactionLog> GenerateTestData()
        {
            var list = new List<TransactionLog>();

            list.Add(new TransactionLog() { ID = 1, Origin = "SAP", TransactionNo = "2", IsPosted = true, LogDate = DateTime.Now });
            list.Add(new TransactionLog() { ID = 2, Origin = "RD", TransactionNo = "1", IsPosted = true, LogDate = DateTime.Now });
            list.Add(new TransactionLog() { ID = 3, Origin = "SAP", TransactionNo = "4", IsPosted = true, LogDate = DateTime.Now });
            list.Add(new TransactionLog() { ID = 4, Origin = "RD", TransactionNo = "234", IsPosted = true, LogDate = DateTime.Now });
            list.Add(new TransactionLog() { ID = 5, Origin = "SAP", TransactionNo = "sd", IsPosted = false, LogDate = DateTime.Now });
            list.Add(new TransactionLog() { ID = 6, Origin = "SAP", TransactionNo = "12312313123123", IsPosted = false, LogDate = DateTime.Now });
            list.Add(new TransactionLog() { ID = 7, Origin = "SAP", TransactionNo = "ada", IsPosted = false, LogDate = DateTime.Now });

            return list;
        }

        public IEnumerable<TransactionLog> GetData()
        {
            var repo = new RepositoryFactory().CreateTransactionLogRepository();

            return repo.GetAll();
        }
    }
}
