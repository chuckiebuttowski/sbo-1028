using SBOClient.Core.DAL.Entities;
using SBOClient.Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SBOClient.Controllers
{
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

                logs.Add(new TransactionLog() { ID = 1, Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.APV, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = true });
                logs.Add(new TransactionLog() { ID = 2, Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.BP, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = true });
                logs.Add(new TransactionLog() { ID = 3, Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.GI, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = true });
                logs.Add(new TransactionLog() { ID = 4, Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.GL, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = false });
                logs.Add(new TransactionLog() { ID = 5, Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.GR, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = false });
                logs.Add(new TransactionLog() { ID = 6, Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.GRPO, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = false });
                logs.Add(new TransactionLog() { ID = 7, Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.IP, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = false });

                return logs;

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
            //please include raw data and error info here
            //return new TransactionLog() { ID = id, Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.APV, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = true, RawData = new TransactionData() { RawData = "some raw data goes here" } };
            return new TransactionLog() { ID = id, Origin = "SAP", TransactionNo = "1123123123123", Type = TransactionLog.SBOType.APV, LogDate = DateTime.Now, Remarks = "Harded code data", IsPosted = false, RawData =new TransactionData() { RawData = "some raw data goes here" }, Error = new ErrorLog() { Message = "Some error message here", StackTrace = "This is the error stack trace: The quick brown fox jump over the lazy dog" } };
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
